"""
label_service.py — DrawCall labeling: LLM (per pipeline) with rule-based fallback.

Mirrors C# DrawCallLabelService.Label():
  1. If LLM is configured and shader files exist for the pipeline → call LLM
     (result cached in-process by pipeline_id and on disk via llm_wrapper cache)
  2. Fall back to keyword + geometry heuristic rules when LLM is unavailable,
     returns empty/error, or shader files are missing.
"""

from __future__ import annotations

import json
import re
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

# ── Keyword rules (same order/priority as C# DrawCallLabelService.Rules) ──────
_RULES: list[tuple[list[str], str, str]] = [
    (["shadow", "planar", "shadowmap", "shadowdepth", "shadowpass", "shadowcaster"],
     "Shadow", "Shadow pass"),
    (["ui", "hud", "canvas", "glyph", "font", "widget", "overlay", "icon", "button", "menu"],
     "UI", "UI rendering"),
    (["particle", "vfx", "emitter", "ribbon", "trail", "spark", "smoke", "fire", "explosion", "distort"],
     "VFX", "Particle / VFX"),
    (["character", "skin", "skinning", "skinnedmesh", "morph", "morphtarget", "deform",
      "hair", "cloth", "body", "player", "hero", "humanoid", "avatar", "face", "eye"],
     "Character", "Character rendering"),
    (["terrain", "landscape", "heightfield", "heightmap"],
     "Terrain", "Terrain rendering"),
    (["blur", "bloom", "tonemap", "tonemapping", "ssao", "ao", "dof", "composite",
      "resolve", "postprocess", "taa", "fxaa", "msaa", "temporal", "upscale", "blit",
      "lut", "vignette", "chromatic", "grain", "sharpen", "motion", "velocity",
      "denoise", "irradiance", "specular", "prefilter", "brdf", "ssr", "reflection"],
     "PostProcess", "Post-process pass"),
    (["sky", "skybox", "water", "ocean", "grass", "foliage",
      "ground", "scene", "mesh", "world", "indoor", "outdoor", "building", "road"],
     "Scene", "Scene rendering"),
]


def _make_label(category: str, subcategory: str, detail: str,
                reason_tags: list[str], confidence: float,
                label_source: str = "rule") -> dict:
    return {
        "category":     category,
        "subcategory":  subcategory,
        "detail":       detail,
        "reason_tags":  reason_tags,
        "confidence":   confidence,
        "label_source": label_source,
    }


def _label_dc(dc: dict) -> dict:
    """Mirror C# DrawCallLabelService.LabelByRules()."""
    api_name   = dc.get("api_name", "")
    vc         = dc.get("vertex_count", 0) or 0
    ic         = dc.get("index_count", 0) or 0
    inst       = dc.get("instance_count", 0) or 0
    rts        = dc.get("render_targets") or []

    is_compute   = api_name == "vkCmdDispatch"
    # fullscreen quad: 3-6 verts, no index buffer, <=1 vertex buffer — same as C#
    is_fullscreen = (not is_compute
                     and vc in (3, 4, 6)
                     and ic == 0)

    # ── R1: RG-encoded shadow map (VSM/ESM) ───────────────────────────────────
    # Color-only 2-channel RT (R8G8/R16G16/R32G32), no depth, real geometry
    if not is_fullscreen and not is_compute and rts:
        has_depth    = any(r.get("attachment_type") in ("Depth", "Stencil", "DepthStencil") for r in rts)
        has_color    = any(r.get("attachment_type") == "Color" for r in rts)
        all_color_rg = all(
            r.get("attachment_type") != "Color" or _is_rg_format(r.get("format") or r.get("format_name") or "")
            for r in rts
        )
        if has_color and not has_depth and all_color_rg:
            return _make_label("Scene(Shadow)", "DepthOnly",
                               "Encoded shadow map (RG depth)",
                               ["shadow_depth_write"], 0.75)

    # ── R1a: Depth-only RT ────────────────────────────────────────────────────
    if rts:
        has_depth  = any(r.get("attachment_type") in ("Depth", "Stencil", "DepthStencil") for r in rts)
        has_color  = any(r.get("attachment_type") == "Color" for r in rts)
        if has_depth and not has_color:
            return _make_label("Scene(Shadow)", "DepthOnly",
                               "Depth-only / shadow map",
                               ["shadow_depth_write"], 0.75)

    # ── Keyword matching on shader entry points ───────────────────────────────
    stages = dc.get("shader_stages") or []
    entry_points = [
        (s.get("entry_point") or "").lower()
        for s in stages
        if (s.get("entry_point") or "") not in ("", "main")
    ]
    if entry_points:
        combined = " ".join(entry_points)
        for keywords, category, detail_hint in _RULES:
            for kw in keywords:
                if kw in combined:
                    return _make_label(category, "", detail_hint,
                                       [re.sub(r"[^a-z0-9]+", "_", kw).strip("_")], 0.65)

    # ── R2: Compute ───────────────────────────────────────────────────────────
    if is_compute:
        # Character compute: skinning / morph-target deformation shaders
        _CHARACTER_COMPUTE_KW = (
            "skin", "skinning", "skinnedmesh", "morph", "morphtarget", "deform",
            "cloth", "hair", "character", "player", "humanoid",
        )
        combined_cs = " ".join(entry_points)
        if any(kw in combined_cs for kw in _CHARACTER_COMPUTE_KW):
            return _make_label("Character", "Compute",
                               "Character skinning / morph compute",
                               ["skinned_mesh", "compute_dispatch"], 0.80)
        return _make_label("PostProcess", "Compute", "Compute dispatch",
                            ["compute_dispatch"], 0.70)

    # ── R3: Fullscreen quad → PostProcess ─────────────────────────────────────
    if is_fullscreen:
        return _make_label("PostProcess", "Fullscreen", "Fullscreen post-process quad",
                            ["tone_mapping"], 0.65)

    # ── R4: Geometry-scale heuristics ─────────────────────────────────────────
    if rts:
        has_depth = any(r.get("attachment_type") in ("Depth", "Stencil", "DepthStencil") for r in rts)
        has_color = any(r.get("attachment_type") == "Color" for r in rts)

        # UI: color-only, RGBA8, no depth
        if has_color and not has_depth:
            color_rts = [r for r in rts if r.get("attachment_type") == "Color"]
            if all(_is_rgba8_format(r.get("format") or r.get("format_name") or "") for r in color_rts):
                return _make_label("UI", "UICanvas", "UI / 2D overlay",
                                    ["ui_canvas"], 0.65)

        # VFX: color+depth, low verts-per-instance (billboard quads), many instances
        if has_color and has_depth and inst > 1:
            verts_per_inst = vc // inst if inst > 0 else vc
            if verts_per_inst <= 6:
                return _make_label("VFX", "Particle", "Particle / billboard",
                                    ["particle_billboard"], 0.65)

    # ── Default ───────────────────────────────────────────────────────────────
    return _make_label("Scene", "Opaque", "Scene rendering",
                        ["opaque_geometry"], 0.60)


def _is_rg_format(fmt: str) -> bool:
    """2-channel color format used for VSM/ESM encoded shadow maps."""
    f = fmt.upper()
    return (f.startswith("R8G8") or f.startswith("R16G16") or f.startswith("R32G32"))


def _is_rgba8_format(fmt: str) -> bool:
    f = fmt.upper()
    return "R8G8B8A8" in f or "B8G8R8A8" in f


# ── LLM labeling ──────────────────────────────────────────────────────────────

_ALLOWED_CATEGORIES = [
    "Scene", "Terrain", "Character", "Shadow", "PostProcess", "VFX", "UI", "Other",
    "Scene(Shadow)", "Terrain(Shadow)", "Character(Shadow)",
]

# In-process per-pipeline cache: pipeline_id → label dict  (mirrors C# _llmCache)
_pipeline_llm_cache: dict[int, dict] = {}


def _extract_shader_sections(src: str, resource_limit: int = 2500, main_limit: int = 5000) -> str:
    """Mirror C# DrawCallLabelService.ExtractRelevantShaderSections().

    Section 1 — resource declarations: Texture/Buffer/Sampler globals and small
      per-draw cbuffers (b0/b1/b2). The large shared per-pass cbuffer is skipped
      entirely — it is noise for classification (R3 rule in prompt).
    Section 2 — main() / frag_main() / vert_main() / comp_main() body only.
    """
    lines = src.split("\n")
    resources: list[str] = []
    main_body: list[str] = []

    # ── Section 1 ─────────────────────────────────────────────────────────────
    in_skipped  = False
    brace_depth = 0
    res_chars   = 0
    res_done    = False

    for raw in lines:
        line = raw.rstrip()

        if in_skipped:
            brace_depth += line.count("{") - line.count("}")
            if brace_depth <= 0:
                in_skipped = False
            continue

        # Large global cbuffer: anything NOT on register b0/b1/b2 → skip
        if line.startswith("cbuffer ") and \
                ": register(b0)" not in line and \
                ": register(b1)" not in line and \
                ": register(b2)" not in line:
            brace_depth = 1 if "{" in line else 0
            in_skipped  = True
            resources.append("// [large shared cbuffer skipped — see R3]")
            continue

        # Skip static local variable definitions (noise)
        if line.startswith("static "):
            continue

        if not res_done:
            resources.append(line)
            res_chars += len(line) + 1
            if res_chars >= resource_limit:
                res_done = True
                resources.append("// ... (resource section truncated)")

    # ── Section 2 ─────────────────────────────────────────────────────────────
    main_start = -1
    for i, raw in enumerate(lines):
        t = raw.lstrip()
        if (t.startswith("void main(") or t.startswith("void frag_main(") or
                t.startswith("void vert_main(") or t.startswith("void comp_main(")):
            main_start = i
            break

    if main_start >= 0:
        chars = 0
        for i in range(main_start, len(lines)):
            main_body.append(lines[i])
            chars += len(lines[i]) + 1
            if chars >= main_limit:
                main_body.append("// ... (main body truncated)")
                break

    # Detect empty/trivial shader (only braces + forwarder calls like `frag_main();`)
    main_str = "\n".join(main_body)
    stripped = re.sub(
        r"^\s*(\{|\}|[a-z]+_main\s*\(\s*\)\s*;|void\s+\w+\s*\(.*?\))\s*$",
        "", main_str, flags=re.MULTILINE,
    ).strip()
    if len(stripped) < 5:
        return "(empty shader — no executable statements in main)"

    return (
        "// -- Resource declarations (textures, buffers, small cbuffers) --\n"
        + "\n".join(resources)
        + "\n\n// -- main() / frag_main() body --\n"
        + main_str
    )


def _load_shader_code(run_dir: Path, pipeline_id: int) -> str:
    """Load pipeline shader files and extract relevant sections."""
    shader_dir = run_dir / "shaders"
    if not shader_dir.is_dir():
        return "(shader directory not found — shaders may not have been extracted yet)"

    files = sorted(
        list(shader_dir.glob(f"pipeline_{pipeline_id}_*.hlsl")) +
        list(shader_dir.glob(f"pipeline_{pipeline_id}_*.glsl"))
    )
    if not files:
        return "(no decompiled shader files — only raw SPIR-V available)"

    # If all files are tiny stubs (<200 bytes each) skip LLM entirely
    total_size = sum(f.stat().st_size for f in files)
    if total_size < 200 * len(files):
        return "(empty shader — trivial stub, no rendering operations)"

    parts: list[str] = []
    for f in files:
        parts.append(f"// === {f.name} ===")
        src = f.read_text(encoding="utf-8", errors="replace")
        parts.append(_extract_shader_sections(src))
    return "\n\n".join(parts)


def _build_llm_prompt(dc: dict, shader_code: str) -> str:
    """Mirror C# DrawCallLabelService.BuildPrompt() exactly."""
    cat_list  = "/".join(_ALLOWED_CATEGORIES)
    api_name  = dc.get("api_name", "")
    vc        = dc.get("vertex_count",  0) or 0
    ic        = dc.get("index_count",   0) or 0
    inst      = dc.get("instance_count",0) or 0
    textures  = dc.get("texture_ids")  or dc.get("textures") or []
    stages    = dc.get("shader_stages") or []
    stage_str = ", ".join(
        f"{s.get('stage','')}:{s.get('entry_point','')}" for s in stages
    ) or "none"
    rts = dc.get("render_targets") or []

    verts_per_inst = (vc // inst) if inst > 0 else vc

    out: list[str] = [
        "Classify this Vulkan draw call. Reply with JSON only.",
        "",
        f"API:{api_name}",
        f"Verts:{vc}  Indices:{ic}  Instances:{inst}"
        f"  VertsPerInst:{verts_per_inst}  Textures:{len(textures)}",
        f"Shaders: {stage_str}",
    ]

    if rts:
        out.append("Render targets:")
        for rt in rts:
            sz  = (f" {rt.get('width')}x{rt.get('height')}"
                   if rt.get("width") and rt.get("height") else "")
            fmt = f" {rt.get('format_name','')}" if rt.get("format_name") else ""
            out.append(f"  [{rt.get('attachment_index','')}]"
                       f"{rt.get('attachment_type','')}{sz}{fmt}")

    # Binding summary — mirrors C# BindingSummary block
    vb_count = len(dc.get("vertex_buffers") or [])
    has_ib   = bool(dc.get("index_buffer"))
    out += [
        "Bindings:",
        f"  VertexBuffers:{vb_count if vb_count else '0 (none)'}",
        f"  IndexBuffer:{'yes' if has_ib else 'none'}",
    ]

    out += [
        "",
        "Category definitions:",
        "  Scene              — static world geometry rendered to the main HDR buffer (buildings, props).",
        "  Terrain            — heightfield/ground mesh, uses virtual/heightfield textures or terrain lightmaps.",
        "  Character          — dynamic skinned mesh (players, crowd) with per-object SH probe lighting, no lightmap UVs.",
        "  PostProcess        — fullscreen-quad pass that reads from a previously rendered texture and writes to an RT.",
        "  VFX                — particle systems, billboard quads, or other effect geometry (many small instances).",
        "  UI                 — 2D interface elements, no depth RT, typically RGBA8 color RT.",
        "  Other              — compute dispatches.",
        "  Scene(Shadow)      — shadow map pass rendering SCENE geometry into a depth or encoded-depth RT.",
        "  Terrain(Shadow)    — shadow map pass rendering TERRAIN geometry into a depth or encoded-depth RT.",
        "  Character(Shadow)  — shadow map pass rendering CHARACTER geometry into a depth or encoded-depth RT.",
        "",
        "Rules (apply in order):",
        "R1 [Render targets first — HIGHEST PRIORITY, overrides everything else]",
        "  ** RULE R1a: Depth-only RT, no Color RT → SHADOW MAP PASS. Determine object type from shader and output",
        "     the matching shadow category: 'Scene(Shadow)', 'Terrain(Shadow)', or 'Character(Shadow)'.",
        "     Default to 'Scene(Shadow)' if indeterminate.",
        "  ** RULE R1b: Color RT with only 2 channels (R8G8/R16G16/R32G32/RG prefix), AND no Depth RT,",
        "     AND real geometry (VertexCount>6 or IndexBuffer) → ENCODED DEPTH SHADOW MAP (VSM/ESM).",
        "     Output matching shadow category. Do NOT output bare 'Scene'.",
        "  ** RULE R1b exception: VertexCount 3-6 AND no IndexBuffer → fullscreen shadow blur → PostProcess.",
        "  Color-only RT, no Depth, R8G8B8A8/B8G8R8A8 → UI",
        "  Color-only RT, no Depth, HDR/float format, screen-size → PostProcess",
        "  Color HDR + Depth, VertsPerInst<=6, many instances → VFX (particles/quads)",
        "  Color HDR + Depth, VertsPerInst>6, normal geometry → Scene/Character/Terrain",
        "R2 [Shader main() for Scene/Character/Terrain]",
        "  Scene:     lightmap textures sampled in main() using TEXCOORD2 UVs (irradiance/sky-visibility/baked).",
        "  Character: per-object SH probe Buffer<float4> loaded via per-instance offset — no lightmap sampling.",
        "             Also: per-instance cosmetic data (tint/recolor) or skinned vertex buffer offset.",
        "             vkCmdDispatch with entry point containing skin/skinning/morph/deform/cloth/hair → Character.",
        "  Terrain:   terrain-specific expression shader or virtual/heightfield texture sampling in main().",
        "R3 [cbuffer vs texture priority — CRITICAL]",
        "  The first/global cbuffer (b0 or b11) is a SHARED per-pass buffer present in EVERY draw call.",
        "  It typically contains terrain, shadow, lighting structs — ALL irrelevant unless READ in main().",
        "  IGNORE global cbuffer struct declarations. Only what is used inside main()/frag_main() matters.",
        "  TRUST ORDER: texture/sampler calls in main() > secondary cbuffers/typed buffers > global cbuffer.",
        "  If a typed Buffer<float4> or ByteAddressBuffer is loaded via a per-instance offset in main(),",
        "  that is a STRONG signal of Character (dynamic SH probe data). Terrain is always static/baked.",
        "",
        "Shader (analyze what is actually computed in main()):",
        shader_code,
        "",
        f"Categories: {cat_list}",
        "IMPORTANT RESTRICTIONS:",
        "  - 'Other' is for vkCmdDispatch compute that has NO clear category signal.",
        "    If the compute shader entry point or code mentions skin/skinning/morph/deform/cloth/hair",
        "    → use 'Character' (subcategory 'Compute'), NOT 'Other'.",
        "  - NEVER use 'Other' for vkCmdDraw or vkCmdDrawIndexed.",
        "  - If R1a/R1b apply (shadow map RT), you MUST use a shadow category.",
        "    Do NOT fall back to 'Other' — default to 'Scene(Shadow)'.",
        "  - If the shader mentions 'shadow', 'planar shadow', or 'depth encoding', category MUST end in '(Shadow)'.",
        "",
        f"Categories: {cat_list}",
        "Subcategory examples: Opaque, Transparent, DepthOnly, SkinMesh, GaussianBlur, ToneMapping, SSAO,"
        " Bloom, TAA, ShadowDepth, ParticleBillboard, UICanvas.",
        "ReasonTags — pick 1-4: pbr_material, multi_texture_blend, high_uv_sampling, skinned_mesh, morphing,"
        " instanced_draw, compute_dispatch, gaussian_blur, tone_mapping, ssao, bloom, taa, shadow_depth_write,"
        " shadow_pcf_sample, particle_billboard, trail_ribbon, ui_canvas, font_glyph, depth_only,"
        " opaque_geometry, transparent_geometry, large_render_target, mrt_output.",
        "Output JSON only, no markdown, confidence in [0,1]:",
        '{"category":"<category>","subcategory":"<subcategory>","detail":"<3-8 word description>",'
        '"reason_tags":["tag1"],"confidence":0.9}',
    ]
    return "\n".join(out)


def _parse_llm_response(text: str) -> dict | None:
    """Parse LLM JSON response, return label dict or None on failure."""
    if not text:
        return None
    # Strip markdown fences
    t = text.strip()
    if t.startswith("```"):
        start = t.find("\n") + 1
        end   = t.rfind("```")
        if end > start:
            t = t[start:end].strip()
    # Find JSON object
    start = t.find("{")
    end   = t.rfind("}")
    if start < 0 or end <= start:
        return None
    try:
        obj = json.loads(t[start:end + 1])
    except json.JSONDecodeError:
        return None

    raw_cat = (obj.get("category") or "").strip()
    # Match category (exact then partial then shadow compound)
    matched = next((c for c in _ALLOWED_CATEGORIES if c.lower() == raw_cat.lower()), None)
    if not matched:
        matched = next((c for c in _ALLOWED_CATEGORIES if c.lower() in raw_cat.lower()), None)
    if not matched:
        m = re.match(r"^(\w+)\(Shadow\)$", raw_cat, re.IGNORECASE)
        if m:
            base = m.group(1)
            if any(c.lower() == base.lower() for c in _ALLOWED_CATEGORIES):
                matched = base.capitalize() + "(Shadow)"
    if not matched:
        return None

    tags = obj.get("reason_tags") or []
    if isinstance(tags, str):
        tags = [tags]

    return _make_label(
        category    = matched,
        subcategory = (obj.get("subcategory") or "").strip(),
        detail      = (obj.get("detail") or "").strip(),
        reason_tags = [str(t) for t in tags],
        confidence  = float(min(1.0, max(0.0, obj.get("confidence") or 0.85))),
        label_source = "llm",
    )


def _label_dc_with_llm(dc: dict, run_dir: Path) -> dict | None:
    """Try LLM labeling for a DC. Returns label dict or None if LLM unavailable/fails."""
    from analysis.llm_wrapper import get_llm
    llm = get_llm()
    if not llm.is_enabled:
        return None

    pipeline_id: int | None = dc.get("pipeline_id")
    if not pipeline_id:
        return None

    # In-process pipeline cache (same pipeline → same shaders → same label)
    if pipeline_id in _pipeline_llm_cache:
        cached = dict(_pipeline_llm_cache[pipeline_id])
        cached["label_source"] = "cache"
        return cached

    shader_code = _load_shader_code(run_dir, pipeline_id)
    if shader_code.startswith("("):
        return None

    prompt   = _build_llm_prompt(dc, shader_code)
    response = llm.chat(prompt)
    if not response:
        return None

    label = _parse_llm_response(response)
    if label:
        _pipeline_llm_cache[pipeline_id] = label
    return label


# ── Public API ─────────────────────────────────────────────────────────────────

def generate_label_json(snapshot_dir: str | Path, db=None) -> Path:
    """
    Read dc.json from snapshot_dir, classify every draw call, write label.json.
    Returns the path to the written file.
    """
    snap = Path(snapshot_dir)
    dc_path = snap / "dc.json"
    if not dc_path.exists():
        raise FileNotFoundError(f"dc.json not found in {snap}")

    dc_data = json.loads(dc_path.read_text(encoding="utf-8-sig"))
    draw_calls: list[dict[str, Any]] = dc_data.get("draw_calls", [])

    # run_dir is one level up from snapshot_N/ — shaders live at run_dir/shaders/
    run_dir = snap.parent
    # Clear per-snapshot pipeline cache so different snapshots don't share entries
    _pipeline_llm_cache.clear()

    labeled: list[dict] = []
    for dc in draw_calls:
        label = _label_dc_with_llm(dc, run_dir) or _label_dc(dc)
        labeled.append({
            "dc_id":  dc.get("dc_id"),
            "api_id": dc.get("api_id"),
            "label":  label,
        })

    snapshot_id: int = dc_data.get("snapshot_id", 0)
    sdp_name: str    = dc_data.get("sdp_name", "")

    out = {
        "schema_version": "3.0",
        "snapshot_id":    snapshot_id,
        "sdp_name":       sdp_name,
        "generated_at":   datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
        "total_dc_count": len(labeled),
        "draw_calls":     labeled,
    }

    out_path = snap / "label.json"
    out_path.write_text(json.dumps(out, ensure_ascii=False, indent=2), encoding="utf-8")

    if db is not None:
        _persist_labels_to_db(db, snapshot_id, sdp_name, snap, labeled)

    return out_path


def _persist_labels_to_db(db, snapshot_id: int, sdp_name: str, snap: Path,
                           labeled: list[dict]) -> None:
    conn = db.conn()
    run_name = snap.parent.name

    existing = conn.execute(
        "SELECT snapshot_id FROM snapshots WHERE snapshot_id=?", [snapshot_id]
    ).fetchone()
    if not existing:
        conn.execute(
            "INSERT OR REPLACE INTO snapshots VALUES (?,?,?,?,?)",
            [snapshot_id, sdp_name, run_name, str(snap),
             datetime.now(timezone.utc).isoformat()],
        )

    dc_path = snap / "dc.json"
    if dc_path.exists():
        dc_data = json.loads(dc_path.read_text(encoding="utf-8-sig"))
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
            for dc in dc_data.get("draw_calls", [])
        ]
        if dc_rows:
            conn.executemany(
                "INSERT OR REPLACE INTO draw_calls VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                dc_rows,
            )

    labeled_at = datetime.now(timezone.utc).isoformat()
    label_rows = []
    for item in labeled:
        api_id = item.get("api_id")
        if api_id is None:
            continue
        lb = item.get("label") or {}
        label_rows.append((
            snapshot_id,
            api_id,
            lb.get("category", ""),
            lb.get("subcategory", ""),
            lb.get("detail", ""),
            json.dumps(lb.get("reason_tags") or []),
            float(lb.get("confidence", 0.0)),
            lb.get("label_source", "rule"),
            None,
            None,
            labeled_at,
        ))

    if label_rows:
        conn.executemany(
            "INSERT OR REPLACE INTO labels "
            "(snapshot_id, api_id, category, subcategory, detail, reason_tags, "
            "confidence, label_source, bottleneck_text, embedding, labeled_at) "
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            label_rows,
        )
