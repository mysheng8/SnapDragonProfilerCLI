---
type: finding
topic: WebUI current state audit — tabs, routes, data model, analysis trigger, for new Explorer + Questions tabs
status: investigated
related_paths:
  - pySdp/webui/static/index.html
  - pySdp/webui/static/app.js
  - pySdp/webui/static/style.css
  - pySdp/webui/server.py
  - pySdp/webui/routes/files.py
  - pySdp/webui/routes/data.py
  - pySdp/data/query.py
  - pySdp/data/db.py
  - pySdp/data/ingest.py
  - pySdp/analysis/label_service.py
  - pySdp/analysis/status_service.py
  - pySdp/analysis/models/
related_tags:
  - webui
  - frontend
  - tabs
  - explorer
  - questions
  - drawcall
  - metrics
  - analysis-trigger
  - duckdb
  - query-layer
summary: |
  Full audit of WebUI state (4 tabs: Snapshot/Analysis/Results/Logs), all /api/data/* and
  /api/files/* routes, the DuckDB data model (9 tables including snapshots, draw_calls,
  labels, metrics, shader_stages, textures, meshes), and the analysis pipeline trigger
  mechanism. Identifies gaps: (1) no "Explorer" tab for querying a single DC by api_id,
  (2) no "Questions" tab for label-aggregated metrics, (3) Python pipeline (ingest + label
  + status) is not wired into the analysis trigger — only raw Python steps (label/status/
  topdc/analysis_md/dashboard) run after C# job, but ingest_snapshot() is never called
  automatically.
last_updated: 2026-04-22
---

# Finding: WebUI New Tabs — Current State Audit

## Problem Statement

The user wants three incremental improvements to the WebUI:
1. A new "Explorer" tab for querying a single DrawCall's full data (shader, texture, mesh, metrics).
2. A new "Questions" tab showing metrics aggregated by label (sum and avg per label category).
3. Wiring the Python data pipeline (ingest + label/status) to run automatically when C# analysis completes.

## Evidence

### 1. Current WebUI Tabs (index.html lines 26-32)

Four tabs exist, in order:
- `Snapshot` — connect/launch/capture workflow
- `Analysis` — SDP file browser + analysis job trigger
- `Results` — analysis run selector + 3-layer file viewer (collapsible Analysis/Statistics/Raw)
- `Logs` — backend + frontend log viewer with badge

No "Explorer" tab. No "Questions" tab.

The tab switching mechanism is `switchTab(id)` in app.js (line 1148) which activates
`#tab-{id}` section and `.tab-btn[data-tab="{id}"]` button.

### 2. Current Route Inventory

#### /api/files/* (routes/files.py)

| Route | Method | Description |
|---|---|---|
| /api/files/sdp | GET | List .sdp files under a directory |
| /api/files/results | GET | List files in a result directory |
| /api/files/analyses | GET | List all analysis runs + snapshots + files |
| /api/files/read | GET | Read a text file and return content |
| /api/files/label | POST | Run label_service on a snapshot dir |
| /api/files/status | POST | Run status_service on a snapshot dir |
| /api/files/topdc | POST | Run topdc_service on a snapshot dir |
| /api/files/dashboard | POST | Run dashboard_service on a snapshot dir |
| /api/files/analysis_md | POST | Run analysis_md_service on a snapshot dir |

#### /api/data/* (routes/data.py) — 19 routes as of Phase D

| Route | Method | Description |
|---|---|---|
| /api/data/ingest | POST | Ingest snapshot dir into DuckDB |
| /api/data/snapshots | GET | List all ingested snapshots |
| /api/data/draw_calls | GET | List DCs for snapshot_id with optional filters |
| /api/data/dc/{api_id} | GET | Full DC detail: base+label+metrics+shaders+textures+mesh |
| /api/data/refresh_labels | POST | Re-run label+status, persist to DB |
| /api/data/models | GET | List registered analysis models |
| /api/data/models/{name}/run | POST | Run named model for snapshot_id |
| /api/data/questions | GET/POST | CRUD questions |
| /api/data/questions/{id} | GET/PUT/DELETE | Single question CRUD |
| /api/data/questions/{id}/run | POST | Run question against snapshot_id |
| /api/data/dashboards | GET/POST | CRUD dashboards |
| /api/data/dashboards/{id} | GET/PUT/DELETE | Single dashboard CRUD |
| /api/data/dashboards/{id}/run | POST | Run all dashboard panels |

### 3. DrawCall Data Model (query.py get_dc_detail)

`GET /api/data/dc/{api_id}?snapshot_id=<n>` returns a complete dict:
```
{
  api_id, dc_id, api_name, pipeline_id, vertex_count, index_count,
  label: {category, subcategory, detail, confidence, label_source, reason_tags, bottleneck_text} | null,
  metrics: {clocks, read_total_bytes, write_total_bytes, fragments_shaded, vertices_shaded,
            shaders_busy_pct, shaders_stalled_pct, tex_fetch_stall_pct, tex_l1_miss_pct,
            tex_l2_miss_pct, tex_pipes_busy_pct, time_alus_working_pct} | null,
  shader_stages: [{pipeline_id, stage, module_id, entry_point, file_path}, ...],
  textures: [{texture_id, width, height, depth, format, layers, levels, file_path}, ...],
  mesh_file: str | null
}
```

### 4. Label-Aggregated Metrics (snapshot_stats + DuckDB)

`snapshot_stats` table (populated by status_service) provides per-category aggregates:
- `category`, `dc_count`, `clocks_sum`, `clocks_pct`, `avg_conf`

The `category_breakdown` analysis model (Phase A) already reads this table and produces
bar_chart rows. However, this is not exposed as a tab — it's only accessible through the
model/question/dashboard API.

For "Questions" tab the user wants: metrics grouped by label (sum + avg per label category).
The DuckDB `metrics` table has all 13 metric columns per DC, and `labels` has category per DC.
A JOIN query `draw_calls + labels + metrics WHERE snapshot_id=?` grouped by `labels.category`
can produce: `category, dc_count, SUM(clocks), AVG(clocks), AVG(shaders_busy_pct), ...`

### 5. Analysis Trigger Gap (app.js _runPySteps)

Current analysis flow (app.js lines 633-653):
1. C# job runs (0-70% progress): `POST /api/sdpcli/analysis`
2. `_runPySteps()` runs sequentially after C# completes (70-100%):
   - POST /api/files/label
   - POST /api/files/status
   - POST /api/files/topdc
   - POST /api/files/analysis_md
   - POST /api/files/dashboard

**Gap**: `POST /api/data/ingest` is never called in `_runPySteps`. The DuckDB workspace DB
is not populated automatically when analysis completes. The `refresh_labels` endpoint also
persists label+status to DuckDB, but neither `ingest` nor `refresh_labels` is in `PY_STEPS`.

The `PY_STEPS` array in app.js (line 442) only references `/api/files/*` endpoints.

To use "Explorer" or "Questions" tabs on newly analyzed data, the user would need to manually
call `POST /api/data/ingest` — there is no automatic trigger.

### 6. Python Analysis Services Currently Existing

After Phase 0 migration (IMPL-2026-04-21-python-data-layer-phase0.md), the services are at:
- `pySdp/analysis/label_service.py` — rule-based DC classification, writes `label.json`
- `pySdp/analysis/status_service.py` — percentile blocks + category stats, writes `status.json`
- `pySdp/analysis/topdc_service.py` — 3-layer attribution engine
- `pySdp/analysis/dashboard_service.py` — Mermaid charts
- `pySdp/analysis/analysis_md_service.py` — per-category LLM hook + rule-based fallback
- `pySdp/analysis/models/` — AnalysisModel ABC + 3 builtin models (category_breakdown, top_bottleneck_dcs, label_quality)

The `pySdp/webui/analysis/` directory has been deleted (confirmed by git status showing
`D pySdp/webui/analysis/__init__.py` etc.).

CLAUDE.md still references the old location under `pySdp/webui/analysis/` — this is outdated
documentation, but the implementation is at the correct `pySdp/analysis/` location.

### 7. Snapshot Selector Context

The "Explorer" tab needs to know which snapshot_id and which snapshot to query. The DuckDB
`snapshots` table stores `(snapshot_id, sdp_name, run_name, snapshot_dir, ingested_at)`.
`GET /api/data/snapshots` already returns all ingested snapshots. The Explorer tab can use
this list to provide a snapshot selector before showing DC data.

## Analysis

### Gap 1: No Explorer Tab

`GET /api/data/draw_calls?snapshot_id=<n>` returns the list of DCs for a snapshot.
`GET /api/data/dc/{api_id}?snapshot_id=<n>` returns full detail for one DC.
These routes are fully implemented. The missing piece is the frontend tab + UI to:
- Select a snapshot from the ingested snapshots list
- Browse/search draw_calls (table or list)
- Click a DC to see its full detail (label, metrics, shaders, textures, mesh)

### Gap 2: No Questions Tab

No dedicated "Questions" frontend tab exists. The model/question/dashboard API exists
on the backend, but the frontend has no UI to interact with it. The "Questions" tab
should show label-aggregated metrics. Options:
- Option A: Use the existing `category_breakdown` model via `/api/data/models/category_breakdown/run`
  — but this only returns `category, dc_count, clocks_sum, clocks_pct, avg_conf`
- Option B: Add a new endpoint that returns a richer per-label metrics aggregation
  (SUM + AVG for all 13 metric columns grouped by category) — more useful for the user

Option B is recommended — the user specifically asked for "metrics data grouped/aggregated
by label (sum and average per label)".

The new backend endpoint would be:
`GET /api/data/label_metrics?snapshot_id=<n>` — JOIN draw_calls + labels + metrics
GROUP BY category, returning sum and average for all numeric metric columns.

### Gap 3: Ingest Not Wired to Analysis Trigger

The `PY_STEPS` array in app.js must include the ingest step before label/status.
Correct order for the full pipeline:
1. `ingest` — POST /api/data/ingest (populates DuckDB from C# JSON outputs)
2. `label` — POST /api/files/label (writes label.json, also persists to DuckDB if db= passed)
3. `status` — POST /api/files/status (writes status.json, also persists to DuckDB)
4. `topdc`, `analysis`, `dashboard` — optional downstream steps

Note: `generate_label_json(snapshot_dir, db=db)` in files.py already persists to DuckDB
when `db` is passed (Phase 3). So if ingest happens first to register the snapshot row,
then label/status will update label rows in DuckDB. This means the Python side is ready —
only the frontend needs to add the ingest step to `PY_STEPS`.

## Impact

- Without ingest wiring: Explorer and Questions tabs cannot show data for newly analyzed snapshots automatically.
- Without Explorer tab: users cannot drill into a single DC's shader/texture/mesh/metrics data via the WebUI.
- Without Questions tab: users cannot see per-label metrics aggregation in the WebUI.
- The backend API layer is fully capable — only frontend + one new backend endpoint are needed.

## Related Context

- Related findings: FINDING-2026-04-21-python-data-layer-design.md (data model design)
- Related plans: PLAN-2026-04-21-python-data-layer.md (phases 4-5 still pending)
- Related implementations: IMPL-2026-04-22-analysis-model-phase-d.md (19 routes completed)
- Related implementations: IMPL-2026-04-21-python-data-layer-phase2.md (query layer)
