# MODULE INDEX — PySdp.WebUI — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: FastAPI, uvicorn, WebUI, HTTP server, proxy, pipeline job
Concepts: REST routing, static serving, SDPCLI proxy, browser SPA, pipeline orchestration
Common Logs: WebUI starting, pipeline job started, pipeline job completed, pipeline step start, pipeline step failed, Unhandled exception, Seeded built-in questions, Seeded built-in dashboards
Entry Symbols: server.py, app, make_router (files), make_router (data), proxy_router, logs_router, PipelineJobManager, pipeline_manager

## Role

FastAPI-based WebUI server that proxies SDPCLI HTTP calls, exposes Python analysis pipeline as REST endpoints, serves a browser SPA, and orchestrates background analysis pipeline jobs (ingest → label → status → topdc → analysis_md).

## Entry Points

| Symbol | Location |
|--------|----------|
| `app` (FastAPI) | [pySdp/webui/server.py](../../../pySdp/webui/server.py#L48) |
| `uvicorn.run(app, ...)` | [pySdp/webui/server.py](../../../pySdp/webui/server.py#L115) |
| `make_router(db)` (files) | [pySdp/webui/routes/files.py](../../../pySdp/webui/routes/files.py#L18) |
| `make_router(db)` (data) | [pySdp/webui/routes/data.py](../../../pySdp/webui/routes/data.py#L50) |
| `proxy_router` | [pySdp/webui/routes/proxy.py](../../../pySdp/webui/routes/proxy.py) |
| `logs_router` | [pySdp/webui/routes/logs.py](../../../pySdp/webui/routes/logs.py) |
| `pipeline_manager` (singleton) | [pySdp/webui/jobs.py](../../../pySdp/webui/jobs.py#L281) |

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `PipelineJob` | Dataclass tracking one background analysis job (status/phase/progress/cancel) | [pySdp/webui/jobs.py](../../../pySdp/webui/jobs.py#L35) |
| `PipelineJobManager` | Thread-safe job store: submit, get, cancel, purge\_expired | [pySdp/webui/jobs.py](../../../pySdp/webui/jobs.py#L63) |

## Key Methods

| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| `PipelineJobManager.submit()` | Create PipelineJob + start background thread | [jobs.py:68](../../../pySdp/webui/jobs.py#L68) | POST /api/data/pipeline |
| `PipelineJobManager._run()` | Execute ordered steps in thread: screenshot→ingest→label→status→topdc→analysis | [jobs.py:107](../../../pySdp/webui/jobs.py#L107) | Internal, called by submit() |
| `_run_step()` | Dispatch step key to service function | [jobs.py:248](../../../pySdp/webui/jobs.py#L248) | Per step in _run() |
| `make_router(db)` (files) | Build APIRouter for /api/files/* file-system endpoints | [routes/files.py:18](../../../pySdp/webui/routes/files.py#L18) | server.py startup |
| `make_router(db)` (data) | Build APIRouter for /api/data/* DuckDB + pipeline endpoints | [routes/data.py:50](../../../pySdp/webui/routes/data.py#L50) | server.py startup |
| `list_analyses()` | Walk analysis root dir, enumerate runs/snapshots/file groups | [routes/files.py:76](../../../pySdp/webui/routes/files.py#L76) | GET /api/files/analyses |
| `run_label()` | Trigger label_service.generate_label_json for a snapshot dir | [routes/files.py:223](../../../pySdp/webui/routes/files.py#L223) | POST /api/files/label |
| `run_topdc()` | Trigger topdc_service.generate_topdc_json | [routes/files.py:246](../../../pySdp/webui/routes/files.py#L246) | POST /api/files/topdc |
| `start_pipeline()` | Submit PipelineJob with requested steps | [routes/data.py:71](../../../pySdp/webui/routes/data.py#L71) | POST /api/data/pipeline |
| `get_pipeline_job()` | Poll PipelineJob status | [routes/data.py:94](../../../pySdp/webui/routes/data.py#L94) | GET /api/data/pipeline/{job_id} |
| `ingest()` | Ingest snapshot dir into DuckDB | [routes/data.py:54](../../../pySdp/webui/routes/data.py#L54) | POST /api/data/ingest |
| `refresh_labels()` | Re-run label+status for a snapshot_id, persist to DB | [routes/data.py:236](../../../pySdp/webui/routes/data.py#L236) | POST /api/data/refresh_labels |

## Call Flow Skeleton

```
python webui/server.py
└── app = FastAPI()
    ├── WorkspaceDB()           -- DuckDB singleton init + schema
    ├── analysis.models import  -- triggers @register decorators
    ├── seed_builtin_questions()
    ├── seed_builtin_dashboards()
    ├── app.include_router(proxy_router,    /api/sdpcli)
    ├── app.include_router(files_router,    /api/files)
    ├── app.include_router(logs_router,     /api/logs)
    ├── app.include_router(data_router,     /api/data)
    └── app.mount(/static, StaticFiles)

POST /api/data/pipeline
└── PipelineJobManager.submit(snapshot_dir, steps, db)
    └── Thread: _run(job, db)
        ├── screenshot  → _copy_screenshot()
        ├── ingest      → ingest_snapshot(db, snapshot_dir)
        ├── label       → generate_label_json(snapshot_dir, db)
        ├── status      → generate_status_json(snapshot_dir, db)
        ├── topdc       → generate_topdc_json(snapshot_dir)
        └── analysis    → generate_analysis_md(snapshot_dir)
```

## Route Map

| Route | Method | Handler | Purpose |
|-------|--------|---------|---------|
| `/api/sdpcli/*` | any | proxy_router | Transparent proxy to SDPCLI :5000 |
| `/api/files/sdp` | GET | list_sdp | Enumerate .sdp files under a dir |
| `/api/files/analyses` | GET | list_analyses | Walk analysis run dirs |
| `/api/files/read` | GET | read_file | Read text file content |
| `/api/files/raw` | GET | serve_raw | Serve raw bytes (Three.js OBJLoader) |
| `/api/files/image` | GET | serve_image | Serve png/jpg/bmp image |
| `/api/files/label` | POST | run_label | Generate label.json |
| `/api/files/status` | POST | run_status | Generate status.json |
| `/api/files/topdc` | POST | run_topdc | Generate topdc.json |
| `/api/files/dashboard` | POST | run_dashboard | Generate dashboard.md |
| `/api/files/analysis_md` | POST | run_analysis_md | Generate analysis.md |
| `/api/data/ingest` | POST | ingest | Ingest snapshot into DuckDB |
| `/api/data/pipeline` | POST | start_pipeline | Submit background pipeline job |
| `/api/data/pipeline/{id}` | GET | get_pipeline_job | Poll job status |
| `/api/data/snapshots` | GET | list_snapshots | List ingested snapshots |
| `/api/data/draw_calls` | GET | draw_calls | Query DCs with labels + metrics |
| `/api/data/dc/{api_id}` | GET | dc_detail | Full DC detail |
| `/api/data/models` | GET | list_analysis_models | List registered analysis models |
| `/api/data/questions` | GET/POST | list/create questions | Question CRUD |
| `/api/data/dashboards` | GET/POST | list/create dashboards | Dashboard CRUD |
| `/api/logs/*` | GET | logs_router | WebUI log streaming |
| `/static/*` | GET | StaticFiles | Browser SPA assets |

## Log → Code Map

| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `"WebUI starting"` | [server.py:108](../../../pySdp/webui/server.py#L108) | uvicorn startup |
| `"Seeded N built-in questions"` | [server.py:59](../../../pySdp/webui/server.py#L59) | questions seeded on startup |
| `"Seeded N built-in dashboards"` | [server.py:64](../../../pySdp/webui/server.py#L64) | dashboards seeded on startup |
| `"pipeline job started"` | [jobs.py:110](../../../pySdp/webui/jobs.py#L110) | background job begins |
| `"pipeline step start: <step>"` | [jobs.py:131](../../../pySdp/webui/jobs.py#L131) | individual step begins |
| `"pipeline step failed: <step>"` | [jobs.py:135](../../../pySdp/webui/jobs.py#L135) | non-fatal step failure |
| `"pipeline job completed"` | [jobs.py:146](../../../pySdp/webui/jobs.py#L146) | all steps finished |
| `"pipeline job fatal error"` | [jobs.py:153](../../../pySdp/webui/jobs.py#L153) | unhandled exception in runner |
| `"pipeline job cancelled"` | [jobs.py:122](../../../pySdp/webui/jobs.py#L122) | cancel requested mid-run |
| `"Unhandled exception"` | [server.py:72](../../../pySdp/webui/server.py#L72) | global exception middleware |
| `"label generation failed"` | [routes/files.py:231](../../../pySdp/webui/routes/files.py#L231) | label route error |
| `"ingest failed"` | [routes/data.py:63](../../../pySdp/webui/routes/data.py#L63) | ingest route error |

## Search Hints

```
Find pipeline job submit:
grep -r "pipeline_manager.submit" pySdp/webui/

Find route registration:
grep -r "app.include_router" pySdp/webui/server.py

Find step dispatch:
grep -r "_run_step" pySdp/webui/jobs.py

Jump:
open pySdp/webui/server.py:48   # FastAPI app init
open pySdp/webui/jobs.py:63     # PipelineJobManager class
open pySdp/webui/routes/data.py:71  # start_pipeline route
```
