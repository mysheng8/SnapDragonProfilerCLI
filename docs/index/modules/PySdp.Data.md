# MODULE INDEX — PySdp.Data — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: DuckDB, WorkspaceDB, ingest, query, model_registry, questions, dashboards
Concepts: schema management, snapshot ingestion, draw call query, analysis model registry, question/dashboard CRUD, metric aggregation, label correlation
Common Logs: ingest failed, list_snapshots failed, draw_calls failed, dc_detail failed, label_metrics failed, label_agg failed, clock_correlation failed
Entry Symbols: WorkspaceDB, ingest_snapshot, get_draw_calls, get_dc_detail, query_dcs, get_labels, get_metrics, register, run_model, seed_builtin_questions, seed_builtin_dashboards

## Role

DuckDB-backed data layer for the pySdp WebUI: manages schema creation and migration, ingests C# JSON outputs into normalized tables (snapshots/draw_calls/shader_stages/textures/meshes/metrics/labels), provides typed query API, and hosts the analysis model registry plus question/dashboard CRUD.

## Entry Points

| Symbol | Location |
|--------|----------|
| `WorkspaceDB.__init__(db_path)` | [pySdp/data/db.py](../../../pySdp/data/db.py#L201) |
| `ingest_snapshot(db, snapshot_dir)` | [pySdp/data/ingest.py](../../../pySdp/data/ingest.py#L85) |
| `get_draw_calls(db, snapshot_id)` | [pySdp/data/query.py](../../../pySdp/data/query.py#L50) |
| `get_dc_detail(db, snapshot_id, api_id)` | [pySdp/data/query.py](../../../pySdp/data/query.py#L183) |
| `query_dcs(db, snapshot_id, ...)` | [pySdp/data/query.py](../../../pySdp/data/query.py#L333) |
| `register(model_cls)` | [pySdp/data/model_registry.py](../../../pySdp/data/model_registry.py#L13) |
| `run_model(name, db, snapshot_id)` | [pySdp/data/model_registry.py](../../../pySdp/data/model_registry.py#L32) |
| `seed_builtin_questions(db)` | [pySdp/data/questions.py](../../../pySdp/data/questions.py) |
| `seed_builtin_dashboards(db)` | [pySdp/data/dashboards.py](../../../pySdp/data/dashboards.py) |

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `WorkspaceDB` | DuckDB connection singleton: ensure_schema, cursor, conn, migrate | [pySdp/data/db.py](../../../pySdp/data/db.py#L201) |

## Key Methods

| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| `WorkspaceDB.ensure_schema()` | Run all CREATE TABLE IF NOT EXISTS DDL idempotently | [db.py:223](../../../pySdp/data/db.py#L223) | WorkspaceDB.__init__ |
| `WorkspaceDB._migrate()` | ALTER TABLE ADD COLUMN for columns added post-schema | [db.py:229](../../../pySdp/data/db.py#L229) | ensure_schema |
| `ingest_snapshot(db, snapshot_dir)` | Parse dc/shaders/textures/buffers/metrics/label JSON → DuckDB transaction | [ingest.py:85](../../../pySdp/data/ingest.py#L85) | POST /api/data/ingest or pipeline step "ingest" |
| `_ingest_all(conn, ...)` | Write all table rows within one transaction | [ingest.py:157](../../../pySdp/data/ingest.py#L157) | ingest_snapshot |
| `_resolve_asset_path(snap, rel)` | Resolve C# relative asset path (shaders/textures/meshes) to absolute | [ingest.py:46](../../../pySdp/data/ingest.py#L46) | _ingest_all per shader/texture/mesh |
| `get_draw_calls(db, snapshot_id)` | JOIN draw_calls+labels+metrics; optional category/tag filters | [query.py:50](../../../pySdp/data/query.py#L50) | GET /api/data/draw_calls |
| `get_dc_detail(db, snapshot_id, api_id)` | Full DC: base+label+metrics+metric_stats+shader_stages+textures+mesh+render_targets | [query.py:183](../../../pySdp/data/query.py#L183) | GET /api/data/dc/{api_id} |
| `get_labels(db, snapshot_id)` | All labels for snapshot keyed by api_id | [query.py:134](../../../pySdp/data/query.py#L134) | generate_status_json, generate_topdc_json |
| `get_metrics(db, snapshot_id)` | All metrics for snapshot keyed by api_id | [query.py:163](../../../pySdp/data/query.py#L163) | generate_status_json |
| `query_dcs(db, snapshot_id, ...)` | Filtered DC query by category/clocks/label_source/tags; clocks DESC | [query.py:333](../../../pySdp/data/query.py#L333) | analysis model runs |
| `register(model_cls)` | @register decorator: add class to _REGISTRY by .name | [model_registry.py:13](../../../pySdp/data/model_registry.py#L13) | analysis.models import at startup |
| `run_model(name, db, snapshot_id)` | Dispatch to registered model's .run() | [model_registry.py:32](../../../pySdp/data/model_registry.py#L32) | POST /api/data/models/{name}/run |

## Database Schema

| Table | Primary Key | Purpose |
|-------|-------------|---------|
| `snapshots` | snapshot_id | Ingested snapshot registry |
| `draw_calls` | (snapshot_id, api_id) | Per-DC geometry parameters |
| `shader_stages` | (snapshot_id, pipeline_id, stage) | Pipeline shader stage + file path |
| `dc_shader_stages` | (snapshot_id, api_id, pipeline_id, stage) | DC → shader stage join |
| `textures` | (snapshot_id, texture_id) | Texture metadata + file path |
| `dc_textures` | (snapshot_id, api_id, texture_id) | DC → texture join |
| `meshes` | (snapshot_id, api_id) | DC mesh OBJ file path |
| `labels` | (snapshot_id, api_id) | Classification result per DC |
| `metrics` | (snapshot_id, api_id) | GPU counter values per DC (50+ columns) |
| `snapshot_stats` | (snapshot_id, category) | Per-category clocks aggregation |
| `questions` | id (TEXT UUID) | Saved analysis questions |
| `dashboards` | id (TEXT UUID) | Dashboard containing ordered question_ids |

## Data Ownership Map

| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| `snapshots` rows | `ingest_snapshot` | all queries | n/a |
| `draw_calls` rows | `ingest_snapshot`, `_persist_labels_to_db` | get_draw_calls, get_dc_detail | n/a |
| `labels` rows | `ingest_snapshot`, `_persist_labels_to_db` | get_draw_calls, label_agg* routes | n/a |
| `metrics` rows | `ingest_snapshot` | get_dc_detail, label_agg*, correlation routes | n/a |
| `questions` rows | `create_question`, `seed_builtin_questions` | run_question, dashboards | `delete_question` |
| `dashboards` rows | `create_dashboard`, `seed_builtin_dashboards` | run_dashboard | `delete_dashboard` |
| `SDP_DB_PATH` env | caller / default data/sdp.db | WorkspaceDB.__init__ | n/a |

## Log → Code Map

| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `"ingest failed"` | [routes/data.py:63](../../../pySdp/webui/routes/data.py#L63) | ingest_snapshot raised exception |
| `"list_snapshots failed"` | [routes/data.py:195](../../../pySdp/webui/routes/data.py#L195) | DuckDB query error on snapshots table |
| `"draw_calls failed"` | [routes/data.py:212](../../../pySdp/webui/routes/data.py#L212) | get_draw_calls raised exception |
| `"dc_detail failed"` | [routes/data.py:232](../../../pySdp/webui/routes/data.py#L232) | get_dc_detail raised exception |
| `"run_analysis_model failed"` | [routes/data.py:289](../../../pySdp/webui/routes/data.py#L289) | model run raised exception |
| `"label_metrics failed"` | [routes/data.py:955](../../../pySdp/webui/routes/data.py#L955) | label_metrics aggregation error |

## Search Hints

```
Find schema DDL:
grep -n "CREATE TABLE" pySdp/data/db.py

Find ingest transaction:
open pySdp/data/ingest.py:85   # ingest_snapshot

Find DC query with metrics join:
open pySdp/data/query.py:50    # get_draw_calls

Find model registration:
grep -rn "@register" pySdp/analysis/models/

Jump:
open pySdp/data/db.py:201       # WorkspaceDB class
open pySdp/data/ingest.py:157   # _ingest_all transaction body
open pySdp/data/query.py:183    # get_dc_detail full detail
open pySdp/data/model_registry.py:32  # run_model dispatch
```
