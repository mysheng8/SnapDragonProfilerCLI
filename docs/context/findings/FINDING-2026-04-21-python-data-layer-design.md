---
type: finding
topic: Python persistent data layer design — C# raw JSON outputs → queryable SQLite entity model
status: investigated
related_paths:
  - pySdp/webui/analysis/label_service.py
  - pySdp/webui/analysis/status_service.py
  - pySdp/webui/analysis/topdc_service.py
  - pySdp/webui/analysis/dashboard_service.py
  - pySdp/webui/analysis/analysis_md_service.py
  - pySdp/webui/routes/files.py
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/analysis/attribution_rules.json
tags:
  - python
  - data-layer
  - sqlite
  - drawcall-analysis
  - schema
  - persistence
  - query-layer
  - label
  - metrics
  - dashboard
last_updated: 2026-04-21
---

# FINDING-2026-04-21: Python Persistent Data Layer Design

## 1. Current State — C# Raw Outputs

All C# output files share a common root envelope (schema 3.0):

```json
{
  "schema_version": "3.0",
  "snapshot_id": 2,
  "sdp_name": "foo.sdp",
  "generated_at": "...",
  "total_dc_count": 312,
  "draw_calls": [...]
}
```

The universal cross-file join key is `api_id` (uint). `dc_id` is a secondary sequential integer used only in label/metrics.

| File | Per-DC key fields |
|---|---|
| `dc.json` | api_id, dc_id, api_name, pipeline_id, vertex_count, index_count, render_targets[], shader_stages[], binding_summary |
| `shaders.json` | api_id, pipeline_id, shader_stages[{stage, module_id, entry_point, file}], shader_files[] |
| `textures.json` | api_id, texture_ids[], textures[{texture_id, width, height, depth, format, layers, levels}], texture_files[] |
| `buffers.json` | api_id, vertex_buffers[], index_buffer, mesh_file |
| `metrics.json` | api_id, dc_id, metrics_available, metrics{clocks, read_total_bytes, write_total_bytes, ~50 snake_case counters} |

`label.json` is Python-generated (not C#):

| File | Per-DC key fields |
|---|---|
| `label.json` | api_id, dc_id, label{category, subcategory, detail, reason_tags[], confidence, label_source} |

**Directory layout** (confirmed from `routes/files.py`):

```
{analysisRoot}/
  {run_name}/                    # e.g. "2026-04-20T18-11-00"
    snapshot_{captureId}/
      dc.json  shaders.json  textures.json  buffers.json
      metrics.json  label.json
      snapshot_{id}_status.json  snapshot_{id}_topdc.json
      snapshot_{id}_analysis.md  snapshot_{id}_dashboard.md
      per_dc_content/
    shaders/  textures/  meshes/  (session-level shared assets)
```

---

## 2. Current State — Python Analysis Services

All 5 Python services are **stateless file-to-file transforms**. Each API call re-reads all JSON files from disk and recomputes from scratch:

| Service | Reads | Writes |
|---|---|---|
| label_service | dc.json | label.json |
| status_service | label.json + metrics.json + dc.json | snapshot_{id}_status.json |
| topdc_service | label.json + metrics.json + status.json | snapshot_{id}_topdc.json |
| dashboard_service | label.json + metrics.json | snapshot_{id}_dashboard.md |
| analysis_md_service | label.json + metrics.json | snapshot_{id}_analysis.md |

**Zero persistence**: no SQLite, no pickle, no in-memory store, no cache. The Python layer does not touch the C# `sdp.db` file.

---

## 3. Gap Analysis

| Gap | Impact |
|---|---|
| No entity model — DCs, shaders, textures, meshes are raw JSON blobs | Cannot query across snapshots, filter by label, or do cross-capture comparisons |
| No persistent label store | LLM labels (expensive) would be recomputed every call |
| Stats recomputed per-call | Percentile blocks over 1000+ DCs are ~50ms each, wasteful on hot path |
| No primary key discipline — dc_id and api_id used inconsistently | label_service uses api_id as source-of-truth; status_service indexes by dc_id |
| No cross-snapshot query capability | Dashboard cannot ask "which categories regressed across runs?" |
| No workspace-level metadata | No record of when data was ingested, from which .sdp file |

---

## 4. Natural Primary Keys

| Entity | Natural PK | Notes |
|---|---|---|
| Snapshot | `snapshot_id` (uint) | From sdp_name in every JSON file. Unique per capture session. |
| DrawCall | `(snapshot_id, api_id)` | api_id = DrawCallApiID, stable within a capture |
| Shader | `(snapshot_id, pipeline_id, stage)` | Multiple DCs share one pipeline; one pipeline has multiple stages |
| Texture | `(snapshot_id, texture_id)` | texture_id = VulkanSnapshotTextures.resourceID |
| Mesh | `(snapshot_id, api_id)` | 1:1 with DrawCall (mesh_{api_id}.obj) |
| Label | `(snapshot_id, api_id)` | 1:1 derived from DC |
| Metrics | `(snapshot_id, api_id)` | 1:1 from C# |

**dc_id is not a stable PK** — it is a sequential counter whose meaning only holds within a single label.json/metrics.json pairing. `api_id` is the stable cross-file join key and should be the canonical PK.

---

## 5. Proposed Entity Model

### Core entities (immutable after C# ingestion)

```
snapshots
  snapshot_id   INTEGER  PK
  sdp_name      TEXT
  run_name      TEXT     (parent folder name, e.g. "2026-04-20T18-11-00")
  snapshot_dir  TEXT     (absolute path)
  ingested_at   TEXT

draw_calls
  snapshot_id   INTEGER  FK → snapshots
  api_id        INTEGER
  dc_id         INTEGER
  api_name      TEXT     (e.g. "vkCmdDrawIndexed")
  pipeline_id   INTEGER
  vertex_count  INTEGER
  index_count   INTEGER
  PRIMARY KEY (snapshot_id, api_id)

shader_stages
  snapshot_id   INTEGER  FK → snapshots
  pipeline_id   INTEGER
  stage         TEXT     (Vertex / Fragment / Compute)
  module_id     INTEGER
  entry_point   TEXT
  file_path     TEXT
  PRIMARY KEY (snapshot_id, pipeline_id, stage)

dc_shader_stages           (DC → shader many-to-many via pipeline)
  snapshot_id   INTEGER
  api_id        INTEGER  FK → draw_calls
  pipeline_id   INTEGER
  stage         TEXT
  PRIMARY KEY (snapshot_id, api_id, pipeline_id, stage)

textures
  snapshot_id   INTEGER  FK → snapshots
  texture_id    INTEGER
  width         INTEGER
  height        INTEGER
  depth         INTEGER
  format        TEXT
  layers        INTEGER
  levels        INTEGER
  file_path     TEXT
  PRIMARY KEY (snapshot_id, texture_id)

dc_textures                (DC → texture many-to-many)
  snapshot_id   INTEGER
  api_id        INTEGER  FK → draw_calls
  texture_id    INTEGER  FK → textures
  PRIMARY KEY (snapshot_id, api_id, texture_id)

meshes
  snapshot_id   INTEGER  FK → snapshots
  api_id        INTEGER  FK → draw_calls
  mesh_file     TEXT
  PRIMARY KEY (snapshot_id, api_id)
```

### Derived entities (written by Python services, refreshable)

```
labels
  snapshot_id   INTEGER  FK → draw_calls
  api_id        INTEGER
  category      TEXT
  subcategory   TEXT
  detail        TEXT
  reason_tags   TEXT     (JSON array)
  confidence    REAL
  label_source  TEXT     (rule / llm)
  labeled_at    TEXT
  PRIMARY KEY (snapshot_id, api_id)

metrics
  snapshot_id   INTEGER  FK → draw_calls
  api_id        INTEGER
  clocks                    INTEGER
  read_total_bytes          INTEGER
  write_total_bytes         INTEGER
  fragments_shaded          INTEGER
  vertices_shaded           INTEGER
  -- ~50 additional snake_case counter columns (nullable REAL)
  PRIMARY KEY (snapshot_id, api_id)
```

---

## 6. Technology Recommendation

**SQLite** via Python stdlib `sqlite3`, WAL mode, one file per workspace root.

- DB path: `{analysisRoot}/sdp_workspace.db`
- **Not** per-snapshot — workspace-level DB enables cross-snapshot queries
- WAL mode: allows concurrent reads during ingestion writes
- Zero extra dependencies — no pandas, no DuckDB needed at this scale (<10k DCs/snapshot)
- DuckDB considered but rejected: over-engineered for current scale; no stdlib, requires pip install
- Parquet considered but rejected: no efficient UPSERT path for label refresh

---

## 7. Ingestion Strategy

- **On-demand trigger** (not file-watcher) — POST `/api/data/ingest?snapshot_dir=...`
- Idempotent: UPSERT (INSERT OR REPLACE) on natural PKs
- Ingestion order: snapshots → draw_calls → shader_stages → dc_shader_stages → textures → dc_textures → meshes → metrics → labels
- Labels ingested separately from core data — can be refreshed without re-ingesting raw data
- C# raw format must not be changed — Python ingests as-is

---

## 8. Integration with Existing Python Services

The new data layer sits **alongside** the current stateless services, not replacing them initially:
- Existing services continue to work against disk JSON files
- New `WorkspaceDB` class introduced under `pySdp/webui/data/`
- Ingestion triggered from `routes/files.py` (new `/api/data/ingest` endpoint)
- Analysis services can optionally query DB instead of re-reading JSON (Phase 2)
