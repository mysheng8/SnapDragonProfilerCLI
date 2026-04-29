"""ingest.py — ingest a snapshot directory into DuckDB."""
from __future__ import annotations

import json
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

from data.db import WorkspaceDB

# All known metric snake_case keys (mirrors DrawCallModels.cs CounterToKey).
# Used to validate keys from metrics.json before writing to DB.
_ALL_METRIC_KEYS = frozenset({
    "clocks", "preemptions", "avg_preemption_delay",
    "read_total_bytes", "write_total_bytes", "tex_mem_read_bytes",
    "vertex_mem_read_bytes", "sp_mem_read_bytes",
    "avg_bytes_per_fragment", "avg_bytes_per_vertex",
    "fragments_shaded", "vertices_shaded", "reused_vertices",
    "pre_clipped_polygons", "lrz_pixels_killed",
    "avg_polygon_area", "avg_vertices_per_polygon",
    "prims_clipped_pct", "prims_trivially_rejected_pct",
    "tex_fetch_stall_pct", "tex_l1_miss_pct", "tex_l2_miss_pct",
    "tex_pipes_busy_pct", "linear_filtered_pct", "nearest_filtered_pct",
    "anisotropic_filtered_pct", "non_base_level_tex_pct",
    "l1_tex_cache_miss_per_pixel", "textures_per_fragment", "textures_per_vertex",
    "shaders_busy_pct", "shaders_stalled_pct",
    "time_alus_working_pct", "time_efus_working_pct",
    "time_shading_vertices_pct", "time_shading_fragments_pct",
    "time_compute_pct", "shader_alu_capacity_pct",
    "wave_context_occupancy_pct", "instruction_cache_miss_pct",
    "fragment_instructions", "fragment_alu_instr_full",
    "fragment_alu_instr_half", "fragment_efu_instructions",
    "vertex_instructions", "alu_per_fragment", "alu_per_vertex",
    "efu_per_fragment", "efu_per_vertex",
    "vertex_fetch_stall_pct", "stalled_on_system_mem_pct",
})


def _read_json(path: Path) -> dict | None:
    """Read a JSON file, returning None if the file does not exist."""
    if not path.exists():
        return None
    return json.loads(path.read_text(encoding="utf-8-sig"))


def _resolve_asset_path(snap: Path, rel: str) -> str:
    """Resolve an asset path from C# JSON to an absolute path string.

    C# writes relative paths like '../../shaders/pipeline_X.hlsl'.  The relative
    anchor is ambiguous, so we use a two-step strategy:
      1. Try to find the file by its basename in the run-level asset dirs
         (run_dir/shaders/, run_dir/textures/, run_dir/meshes/) — reliable.
      2. Fall back to joining snap/rel and resolving — works if path is already absolute.
    Returns empty string if rel is empty/None or the file cannot be found.
    """
    if not rel:
        return ""
    run_dir = snap.parent
    fname = Path(rel).name  # basename: 'pipeline_X.hlsl', 'mesh_N.obj', etc.
    # Determine sub-directory from extension / prefix
    suffix = Path(fname).suffix.lower()
    if suffix in (".hlsl", ".spv", ".glsl"):
        candidate = run_dir / "shaders" / fname
    elif suffix == ".obj":
        candidate = run_dir / "meshes" / fname
    elif suffix in (".png", ".jpg", ".jpeg", ".bmp"):
        candidate = run_dir / "textures" / fname
    else:
        candidate = None

    if candidate and candidate.exists():
        return str(candidate)

    # Fallback: try joining with snap (handles already-absolute paths too)
    try:
        p = Path(rel)
        if p.is_absolute():
            return str(p) if p.exists() else ""
        resolved = (snap / rel).resolve()
        return str(resolved) if resolved.exists() else ""
    except Exception:
        return ""


def ingest_snapshot(db: WorkspaceDB, snapshot_dir: str | Path) -> dict:
    """
    Ingest all C# JSON outputs from snapshot_dir into DuckDB.

    Returns:
        {
            "snapshot_id": int,
            "counts": {
                "draw_calls": int,
                "shader_stages": int,
                "dc_shader_stages": int,
                "textures": int,
                "dc_textures": int,
                "meshes": int,
                "metrics": int,
                "labels": int,
            }
        }
    """
    snap = Path(snapshot_dir)
    conn = db.conn()

    # ── 1. Load JSON files ──────────────────────────────────────────────────────
    dc_data = _read_json(snap / "dc.json")
    if dc_data is None:
        raise FileNotFoundError(f"dc.json not found in {snap}")

    shaders_raw   = _read_json(snap / "shaders.json")
    textures_raw  = _read_json(snap / "textures.json")
    buffers_raw   = _read_json(snap / "buffers.json")
    metrics_raw   = _read_json(snap / "metrics.json")
    label_raw     = _read_json(snap / "label.json")

    # ── 2. Derive metadata ──────────────────────────────────────────────────────
    snapshot_id: int = dc_data.get("snapshot_id", 0)
    sdp_name: str    = dc_data.get("sdp_name", "")
    run_name: str    = snap.parent.name          # {analysisRoot}/{run_name}/snapshot_{N}/
    snapshot_dir_str = str(snap.resolve())
    ingested_at      = datetime.now(timezone.utc).isoformat()

    draw_calls: list[dict[str, Any]] = dc_data.get("draw_calls", [])

    # ── 3. Begin transaction ────────────────────────────────────────────────────
    try:
        conn.rollback()
    except Exception:
        pass
    conn.begin()
    try:
        counts = _ingest_all(
            conn,
            snap=snap,
            snapshot_id=snapshot_id,
            sdp_name=sdp_name,
            run_name=run_name,
            snapshot_dir_str=snapshot_dir_str,
            ingested_at=ingested_at,
            draw_calls=draw_calls,
            shaders_raw=shaders_raw,
            textures_raw=textures_raw,
            buffers_raw=buffers_raw,
            metrics_raw=metrics_raw,
            label_raw=label_raw,
        )
        conn.commit()
    except Exception:
        conn.rollback()
        raise

    return {"snapshot_id": snapshot_id, "counts": counts}


def _ingest_all(
    conn,
    *,
    snap: Path,
    snapshot_id: int,
    sdp_name: str,
    run_name: str,
    snapshot_dir_str: str,
    ingested_at: str,
    draw_calls: list[dict],
    shaders_raw: dict | None,
    textures_raw: dict | None,
    buffers_raw: dict | None,
    metrics_raw: dict | None,
    label_raw: dict | None,
) -> dict:
    counts = {
        "draw_calls": 0,
        "shader_stages": 0,
        "dc_shader_stages": 0,
        "textures": 0,
        "dc_textures": 0,
        "meshes": 0,
        "metrics": 0,
        "labels": 0,
    }

    # ── snapshots ───────────────────────────────────────────────────────────────
    conn.execute(
        "INSERT OR REPLACE INTO snapshots VALUES (?, ?, ?, ?, ?)",
        [snapshot_id, sdp_name, run_name, snapshot_dir_str, ingested_at],
    )

    # ── draw_calls ──────────────────────────────────────────────────────────────
    dc_rows = [
        (
            snapshot_id,
            dc.get("api_id", 0),
            dc.get("dc_id", 0),
            dc.get("api_name", ""),
            dc.get("pipeline_id"),
            dc.get("vertex_count", 0),
            dc.get("index_count", 0),
            dc.get("instance_count", 0),
            dc.get("first_vertex", 0),
            dc.get("first_index", 0),
            dc.get("vertex_offset", 0),
            dc.get("first_instance", 0),
            dc.get("draw_count", 0),
            dc.get("group_count_x", 0),
            dc.get("group_count_y", 0),
            dc.get("group_count_z", 0),
        )
        for dc in draw_calls
    ]
    if dc_rows:
        conn.executemany(
            "INSERT OR REPLACE INTO draw_calls VALUES "
            "(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            dc_rows,
        )
    counts["draw_calls"] = len(dc_rows)

    # ── shader_stages (from dc.json inline stages, deduped by (snapshot_id, pipeline_id, stage)) ──
    shader_stage_seen: set[tuple] = set()
    shader_stage_rows: list[tuple] = []
    dc_shader_rows: list[tuple] = []

    for dc in draw_calls:
        api_id      = dc.get("api_id", 0)
        pipeline_id = dc.get("pipeline_id")
        if pipeline_id is None:
            continue
        stages: list[dict] = dc.get("shader_stages") or []
        for s in stages:
            stage = s.get("stage", "")
            module_id   = s.get("module_id")
            entry_point = s.get("entry_point", "")
            file_path   = _resolve_asset_path(snap, s.get("file", "") or s.get("file_path", ""))
            key = (snapshot_id, pipeline_id, stage)
            if key not in shader_stage_seen:
                shader_stage_seen.add(key)
                shader_stage_rows.append((snapshot_id, pipeline_id, stage, module_id, entry_point, file_path))
            dc_shader_rows.append((snapshot_id, api_id, pipeline_id, stage))

    # Also pull from shaders.json if available (may have richer file_path info)
    if shaders_raw:
        shader_dcs: list[dict] = shaders_raw.get("draw_calls") or shaders_raw.get("shaders") or []
        # shaders.json structure: list of {api_id, pipeline_id, shader_stages: [...], shader_files: [...]}
        if isinstance(shader_dcs, list):
            for sdc in shader_dcs:
                pipeline_id = sdc.get("pipeline_id")
                api_id      = sdc.get("api_id")
                if pipeline_id is None:
                    continue
                stages = sdc.get("shader_stages") or []
                for s in stages:
                    stage       = s.get("stage", "")
                    module_id   = s.get("module_id")
                    entry_point = s.get("entry_point", "")
                    file_path   = _resolve_asset_path(snap, s.get("file", "") or s.get("file_path", ""))
                    key = (snapshot_id, pipeline_id, stage)
                    if key not in shader_stage_seen:
                        shader_stage_seen.add(key)
                        shader_stage_rows.append((snapshot_id, pipeline_id, stage, module_id, entry_point, file_path))
                    if api_id is not None:
                        row = (snapshot_id, api_id, pipeline_id, stage)
                        if row not in set(dc_shader_rows):
                            dc_shader_rows.append(row)

    if shader_stage_rows:
        conn.executemany(
            "INSERT OR REPLACE INTO shader_stages VALUES (?, ?, ?, ?, ?, ?)",
            shader_stage_rows,
        )
    counts["shader_stages"] = len(shader_stage_rows)

    if dc_shader_rows:
        # Deduplicate before insert
        unique_dc_shader = list({r: None for r in dc_shader_rows}.keys())
        conn.executemany(
            "INSERT OR REPLACE INTO dc_shader_stages VALUES (?, ?, ?, ?)",
            unique_dc_shader,
        )
        counts["dc_shader_stages"] = len(unique_dc_shader)

    # ── textures ────────────────────────────────────────────────────────────────
    texture_seen: set[int] = set()
    texture_rows: list[tuple] = []
    dc_texture_rows: list[tuple] = []

    # Run-level asset directories: siblings of snapshot_N/ under the run dir
    # e.g.  .../analysis/<run>/snapshot_2/ → .../analysis/<run>/textures/
    run_dir = snap.parent

    if textures_raw:
        texture_dcs: list[dict] = textures_raw.get("draw_calls") or textures_raw.get("textures") or []
        if isinstance(texture_dcs, list):
            for tdc in texture_dcs:
                api_id = tdc.get("api_id")
                # Per-DC texture list
                textures_list: list[dict] = tdc.get("textures") or []
                for t in textures_list:
                    tex_id = t.get("texture_id")
                    if tex_id is None:
                        continue
                    if tex_id not in texture_seen:
                        texture_seen.add(tex_id)
                        # Resolve texture file from run-level textures/ dir
                        tex_file = _resolve_asset_path(snap, t.get("file", "") or t.get("file_path", ""))
                        if not tex_file:
                            # C# stores textures at run_dir/textures/texture_{id}.png
                            candidate = run_dir / "textures" / f"texture_{tex_id}.png"
                            if candidate.exists():
                                tex_file = str(candidate)
                        texture_rows.append((
                            snapshot_id,
                            tex_id,
                            t.get("width"),
                            t.get("height"),
                            t.get("depth"),
                            t.get("format"),
                            t.get("layers"),
                            t.get("levels"),
                            tex_file,
                        ))
                    if api_id is not None:
                        dc_texture_rows.append((snapshot_id, api_id, tex_id))

                # Also cover flat texture_ids list
                for tex_id in (tdc.get("texture_ids") or []):
                    if tex_id is None:
                        continue
                    if tex_id not in texture_seen:
                        texture_seen.add(tex_id)
                        candidate = run_dir / "textures" / f"texture_{tex_id}.png"
                        tex_file = str(candidate) if candidate.exists() else ""
                        texture_rows.append((snapshot_id, tex_id, None, None, None, None, None, None, tex_file))
                    if api_id is not None:
                        dc_texture_rows.append((snapshot_id, api_id, tex_id))

    if texture_rows:
        conn.executemany(
            "INSERT OR REPLACE INTO textures VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)",
            texture_rows,
        )
    counts["textures"] = len(texture_rows)

    if dc_texture_rows:
        unique_dc_tex = list({r: None for r in dc_texture_rows}.keys())
        conn.executemany(
            "INSERT OR REPLACE INTO dc_textures VALUES (?, ?, ?)",
            unique_dc_tex,
        )
        counts["dc_textures"] = len(unique_dc_tex)

    # ── meshes ──────────────────────────────────────────────────────────────────
    mesh_rows: list[tuple] = []
    if buffers_raw:
        buffer_dcs: list[dict] = buffers_raw.get("draw_calls") or buffers_raw.get("buffers") or []
        if isinstance(buffer_dcs, list):
            for bdc in buffer_dcs:
                api_id    = bdc.get("api_id")
                mesh_file = _resolve_asset_path(snap, bdc.get("mesh_file", ""))
                if api_id is not None and mesh_file:
                    mesh_rows.append((snapshot_id, api_id, mesh_file))

    if mesh_rows:
        conn.executemany(
            "INSERT OR REPLACE INTO meshes VALUES (?, ?, ?)",
            mesh_rows,
        )
    counts["meshes"] = len(mesh_rows)

    # ── metrics ─────────────────────────────────────────────────────────────────
    # Discover which keys are present in this metrics.json (varies by MetricsWhitelist)
    # and build a dynamic INSERT with only those columns + the two PK columns.
    metrics_rows: list[tuple] = []
    metrics_cols: list[str] = []  # ordered non-PK columns actually found

    if metrics_raw:
        all_dcs = metrics_raw.get("draw_calls") or []
        # Collect the union of keys present across all DCs in this snapshot
        present_keys: set[str] = set()
        for dc in all_dcs:
            present_keys.update((dc.get("metrics") or {}).keys())
        # Keep only known schema columns, preserve a stable order
        _ORDERED = [k for k in [
            "clocks", "preemptions", "avg_preemption_delay",
            "read_total_bytes", "write_total_bytes", "tex_mem_read_bytes",
            "vertex_mem_read_bytes", "sp_mem_read_bytes",
            "avg_bytes_per_fragment", "avg_bytes_per_vertex",
            "fragments_shaded", "vertices_shaded", "reused_vertices",
            "pre_clipped_polygons", "lrz_pixels_killed",
            "avg_polygon_area", "avg_vertices_per_polygon",
            "prims_clipped_pct", "prims_trivially_rejected_pct",
            "tex_fetch_stall_pct", "tex_l1_miss_pct", "tex_l2_miss_pct",
            "tex_pipes_busy_pct", "linear_filtered_pct", "nearest_filtered_pct",
            "anisotropic_filtered_pct", "non_base_level_tex_pct",
            "l1_tex_cache_miss_per_pixel", "textures_per_fragment", "textures_per_vertex",
            "shaders_busy_pct", "shaders_stalled_pct",
            "time_alus_working_pct", "time_efus_working_pct",
            "time_shading_vertices_pct", "time_shading_fragments_pct",
            "time_compute_pct", "shader_alu_capacity_pct",
            "wave_context_occupancy_pct", "instruction_cache_miss_pct",
            "fragment_instructions", "fragment_alu_instr_full",
            "fragment_alu_instr_half", "fragment_efu_instructions",
            "vertex_instructions", "alu_per_fragment", "alu_per_vertex",
            "efu_per_fragment", "efu_per_vertex",
            "vertex_fetch_stall_pct", "stalled_on_system_mem_pct",
        ] if k in present_keys and k in _ALL_METRIC_KEYS]
        metrics_cols = _ORDERED

        # Integer columns
        _INT_COLS = frozenset({
            "clocks", "preemptions", "read_total_bytes", "write_total_bytes",
            "tex_mem_read_bytes", "vertex_mem_read_bytes", "sp_mem_read_bytes",
            "fragments_shaded", "vertices_shaded", "reused_vertices",
            "pre_clipped_polygons", "lrz_pixels_killed",
            "fragment_instructions", "fragment_alu_instr_full",
            "fragment_alu_instr_half", "fragment_efu_instructions",
            "vertex_instructions",
        })

        for dc in all_dcs:
            api_id = dc.get("api_id")
            if api_id is None:
                continue
            m = dc.get("metrics") or {}
            if not m:
                continue
            vals: list = [snapshot_id, api_id]
            for col in metrics_cols:
                raw = m.get(col)
                vals.append(_int_or_none(raw) if col in _INT_COLS else _float_or_none(raw))
            metrics_rows.append(tuple(vals))

    if metrics_rows and metrics_cols:
        col_names = ", ".join(metrics_cols)
        placeholders = ", ".join(["?"] * (2 + len(metrics_cols)))
        conn.executemany(
            f"INSERT OR REPLACE INTO metrics (snapshot_id, api_id, {col_names}) "
            f"VALUES ({placeholders})",
            metrics_rows,
        )
    counts["metrics"] = len(metrics_rows)

    # ── labels ──────────────────────────────────────────────────────────────────
    label_rows: list[tuple] = []
    if label_raw:
        labeled_at = datetime.now(timezone.utc).isoformat()
        for dc in (label_raw.get("draw_calls") or []):
            api_id = dc.get("api_id")
            if api_id is None:
                continue
            lb = dc.get("label") or {}
            label_rows.append((
                snapshot_id,
                api_id,
                lb.get("category", ""),
                lb.get("subcategory", ""),
                lb.get("detail", ""),
                json.dumps(lb.get("reason_tags") or []),
                float(lb.get("confidence", 0.0)),
                lb.get("label_source", "rule"),
                lb.get("bottleneck_text"),  # may be None
                None,                       # embedding — Phase 5
                labeled_at,
            ))

    if label_rows:
        # Only insert labels whose api_id exists in draw_calls (guards against stale label.json)
        valid_api_ids = {row[1] for row in dc_rows}
        label_rows = [r for r in label_rows if r[1] in valid_api_ids]
        conn.executemany(
            "INSERT OR REPLACE INTO labels VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            label_rows,
        )
    counts["labels"] = len(label_rows)

    return counts


# ── Type-coercion helpers ───────────────────────────────────────────────────────

def _int_or_none(v) -> int | None:
    if v is None:
        return None
    try:
        return int(v)
    except (TypeError, ValueError):
        return None


def _float_or_none(v) -> float | None:
    if v is None:
        return None
    try:
        return float(v)
    except (TypeError, ValueError):
        return None
