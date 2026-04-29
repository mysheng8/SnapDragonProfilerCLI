---
type: implementation
topic: Python data layer Phase 1 — DuckDB WorkspaceDB + ingest_snapshot + /api/data/* endpoints
status: completed
based_on:
  - PLAN-2026-04-21-python-data-layer.md
  - IMPL-2026-04-21-python-data-layer-phase0.md
related_paths:
  - pySdp/data/__init__.py         (NEW)
  - pySdp/data/db.py               (NEW)
  - pySdp/data/ingest.py           (NEW)
  - pySdp/webui/routes/data.py     (NEW)
  - pySdp/webui/server.py          (MODIFIED)
  - pySdp/requirements.txt         (MODIFIED)
date: 2026-04-21
tags:
  - python
  - data-layer
  - duckdb
  - schema
  - ingest
  - phase1
---

# IMPL-2026-04-21: Python Data Layer Phase 1

## Files Created

### `pySdp/data/__init__.py`
Empty package marker (`# data layer`).

### `pySdp/data/db.py`
`WorkspaceDB` class — DuckDB connection manager + schema creation.

- DB path priority: `SDP_DB_PATH` env var → constructor argument → `pySdp/data/sdp.db` (default)
- `ensure_schema()` executes each `CREATE TABLE IF NOT EXISTS` statement individually via `conn.execute()` (DuckDB's Python API does not support `executescript()`; individual calls are idempotent and safe)
- 12 tables created: `snapshots`, `draw_calls`, `shader_stages`, `dc_shader_stages`, `textures`, `dc_textures`, `meshes`, `labels`, `metrics`, `snapshot_stats`, `questions`, `dashboards`
- `labels.embedding` pre-reserved as `FLOAT[]` (Phase 5 vector search)
- `labels.bottleneck_text` pre-reserved as `TEXT` (Phase 4 LLM)
- `metrics` table has 12 known columns only; unknown metric keys skipped in Phase 1 (plan: Phase 2+ adds `ALTER TABLE metrics ADD COLUMN IF NOT EXISTS`)

### `pySdp/data/ingest.py`
`ingest_snapshot(db: WorkspaceDB, snapshot_dir: str | Path) -> dict`

- Reads: `dc.json` (required), `shaders.json`, `textures.json`, `buffers.json`, `metrics.json`, `label.json` (all optional)
- All operations in a single transaction (`conn.begin()` / `conn.commit()` / `conn.rollback()` on error)
- Upsert via `INSERT OR REPLACE INTO` (DuckDB supports SQLite-style syntax)
- Bulk rows via `conn.executemany(sql, list_of_tuples)` for performance
- `run_name` derived as `Path(snapshot_dir).parent.name` (matches `{analysisRoot}/{run_name}/snapshot_{N}/` layout)
- `snapshot_id` read from `dc.json` envelope field
- `reason_tags` stored as JSON string via `json.dumps()`
- `labeled_at` / `ingested_at` use `datetime.now(timezone.utc).isoformat()`
- Returns: `{"snapshot_id": int, "counts": {"draw_calls", "shader_stages", "dc_shader_stages", "textures", "dc_textures", "meshes", "metrics", "labels"}}`

### `pySdp/webui/routes/data.py`
`make_router(db: WorkspaceDB) -> APIRouter` factory function.

Endpoints:
- `POST /api/data/ingest?snapshot_dir=<path>` → `{"ok": True, "snapshot_id": n, "counts": {...}}`
- `GET  /api/data/snapshots` → `{"ok": True, "data": [{snapshot_id, sdp_name, run_name, snapshot_dir, ingested_at}]}`

## Files Modified

### `pySdp/webui/server.py`
- Added imports: `from routes.data import make_router as _make_data_router`, `from data.db import WorkspaceDB`
- Created global DB singleton: `_db = WorkspaceDB()` (after FastAPI app construction)
- Registered router: `app.include_router(_make_data_router(_db), prefix="/api/data", tags=["data"])`

### `pySdp/requirements.txt`
- Added `duckdb` line (installed version: 1.5.2)

## Schema Summary

| Table | PK | Notes |
|---|---|---|
| snapshots | snapshot_id | Root entity; one row per ingested snapshot |
| draw_calls | (snapshot_id, api_id) | DC rows from dc.json |
| shader_stages | (snapshot_id, pipeline_id, stage) | Deduplicated across DCs sharing a pipeline |
| dc_shader_stages | (snapshot_id, api_id, pipeline_id, stage) | DC ↔ shader join |
| textures | (snapshot_id, texture_id) | Texture metadata |
| dc_textures | (snapshot_id, api_id, texture_id) | DC ↔ texture join |
| meshes | (snapshot_id, api_id) | OBJ file path per DC |
| labels | (snapshot_id, api_id) | Rule/LLM labels; includes bottleneck_text + embedding FLOAT[] |
| metrics | (snapshot_id, api_id) | 12 known GPU counter columns |
| snapshot_stats | (snapshot_id, category) | Aggregated stats (Phase 3 will populate) |
| questions | id | Question engine definitions (Phase 7) |
| dashboards | id | Dashboard definitions (Phase 7) |

## Deviations from Plan

None. The plan specified:
- `executescript()` not supported in DuckDB → confirmed, used individual `conn.execute()` calls per statement
- `INSERT OR REPLACE` syntax → confirmed working in DuckDB 1.5.2
- `?` placeholders → confirmed working

## Validation

```
Schema created OK
Validation passed
```

Router smoke test:
```
All imports OK
Router created OK, routes: ['/ingest', '/snapshots']
Smoke test passed
```
