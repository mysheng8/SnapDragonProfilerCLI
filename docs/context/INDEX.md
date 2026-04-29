# Context Index

This file provides a structured index of all investigation findings, plans, and decisions.

Agents MUST:
- read this file before scanning the full context directory
- use this file to identify the most relevant context documents
- prioritize documents based on topic, paths, and tags

---

## 🔎 How to Use This Index

When searching for context:

1. Match by **topic**
2. Match by **related_paths**
3. Match by **tags**
4. Prefer newer entries if multiple matches exist
5. Prefer plans over findings when implementing

---

## 📂 Findings

### FINDING-2026-04-22-webui-new-tabs-plan.md
- topic: WebUI current state audit — tabs, routes, data model, analysis trigger, for new Explorer + Questions tabs
- summary: |
    Full audit of 4 current tabs (Snapshot/Analysis/Results/Logs), all /api/data/* and
    /api/files/* routes (19 data routes, 9 file routes), DuckDB schema (9 tables),
    and the DrawCall data model returned by get_dc_detail (base + label + metrics +
    shader_stages + textures + mesh_file). Identifies 3 gaps: no Explorer tab for single
    DC query, no Questions tab for label-aggregated metrics, and ingest_snapshot() is
    never called in the analysis trigger — DuckDB is not populated automatically.
- related_paths:
  - pySdp/webui/static/index.html
  - pySdp/webui/static/app.js
  - pySdp/webui/routes/files.py
  - pySdp/webui/routes/data.py
  - pySdp/data/query.py
  - pySdp/data/db.py
  - pySdp/data/ingest.py
- tags:
  - webui
  - frontend
  - explorer
  - questions
  - analysis-trigger
  - drawcall
  - duckdb

---

### FINDING-2026-04-21-analysis-model-question-engine.md
- topic: Analysis Model + Question Engine design — AnalysisModel interface, category_breakdown model, Question CRUD, Dashboard
- summary: |
    Full investigation of status_service output shape (4 output sections: overall, category_stats,
    label_stats, global_percentiles). Gap analysis of what is in DB (snapshot_stats: dc_count,
    clocks_sum, clocks_pct, avg_conf) vs what must be computed on-demand (percentile blocks, global
    percentiles, label quality detail). Proposed AnalysisModel ABC with ClassVar metadata + run()
    method. model_registry in data/ as pure dispatch table. category_breakdown model spec: primary
    path reads snapshot_stats, fallback computes from draw_calls+labels+metrics JOIN. 3 builtin
    models: category_breakdown (bar_chart), top_bottleneck_dcs (table), label_quality (bar_chart).
    Question = saved binding of (model_name + model_params + viz_type + viz_config + title).
    Dashboard = ordered list of question IDs; run = run all questions in sequence, non-fatal per panel.
    Import chain: server.py → analysis/models/__init__.py (triggers @register); data/model_registry.py
    is acyclic. No result caching for MVP.
- related_paths:
  - pySdp/analysis/status_service.py
  - pySdp/analysis/label_service.py
  - pySdp/data/query.py
  - pySdp/data/db.py
  - pySdp/data/model_registry.py
  - pySdp/analysis/models/
  - pySdp/webui/routes/data.py
- tags:
  - python
  - analysis-model
  - question-engine
  - dashboard
  - duckdb
  - model-registry
  - category-breakdown
  - viz-type
  - phase6
  - phase7

---

### FINDING-2026-04-21-python-data-layer-design.md
- topic: Python persistent data layer design — C# raw JSON outputs → queryable SQLite entity model
- summary: |
    Complete investigation of current Python analysis services (all stateless file-to-file,
    no persistence). Confirms api_id as canonical cross-file join key. Natural PKs for DC,
    Shader, Texture, Mesh, Label, Metrics. Proposes SQLite workspace DB at
    {analysisRoot}/sdp_workspace.db (one DB per workspace, not per snapshot). Full entity
    DDL for 9 tables: snapshots, draw_calls, shader_stages, dc_shader_stages, textures,
    dc_textures, meshes, labels, metrics. Recommends on-demand ingest trigger (not
    file-watcher). Technology: SQLite stdlib sqlite3 + WAL mode (DuckDB/Parquet rejected).
- related_paths:
  - pySdp/webui/analysis/label_service.py
  - pySdp/webui/analysis/status_service.py
  - pySdp/webui/routes/files.py
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/analysis/attribution_rules.json
- tags:
  - python
  - data-layer
  - sqlite
  - drawcall-analysis
  - schema
  - persistence
  - query-layer

---

### FINDING-2026-04-15-raw-data-schema.md
- topic: complete raw data schema for analysis pipeline — SQLite tables, CSV files, extracted assets, JSON outputs
- summary: |
    Full inventory of every raw artifact the analysis pipeline reads/writes.
    9 SQLite table groups (5 native SDP + 7 CSV-imported + DrawCallMetrics).
    Exact CSV column headers for all 8 CSV files. Shader/texture/mesh extraction
    file naming conventions. Exact JSON field names for dc.json, shaders.json,
    textures.json, buffers.json, label.json, metrics.json, status.json, and the
    legacy raw.json (schema 2.0). Full counter-name→snake_case key mapping table.
    Stability classification for each artifact type.
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Data/SdpDatabase.DrawCalls.cs
  - SDPCLI/source/Data/SdpDatabase.Schema.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Models/VulkanSnapshotModel.cs
  - SDPCLI/source/Services/Analysis/RawJsonGenerationService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Services/Capture/CsvToDbService.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
- tags:
  - database
  - sqlite
  - csv
  - schema
  - json
  - shader
  - texture
  - mesh
  - drawcall-analysis
  - python

---

### FINDING-2026-04-15-server-api-state-audit.md
- topic: HTTP server API endpoint inventory and DeviceStatus state machine audit
- summary: |
    Complete inventory of all 9 routes (GET /api/status, GET /api/device, POST /api/connect,
    POST /api/disconnect, POST /api/session/launch, POST /api/capture, POST /api/analysis,
    GET|POST|DELETE /api/jobs[/{id}[/cancel]]). DeviceStatus enum has 6 values; state machine
    has 9 guarded TryTransition calls + 1 forced assignment (Disconnect). Server does NOT exit
    after archive. Five gaps identified for WebUI design: no activeJobId on /api/device, no
    lastError field, no standard /health path, analysis not surfaced on /api/device, static version.
- related_paths:
  - SDPCLI/source/Server/HttpServer.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/DeviceSessionInfo.cs
  - SDPCLI/source/Server/Handlers/
  - SDPCLI/source/Server/Jobs/
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/routes/proxy.py
  - pySdp/webui/static/app.js
- tags:
  - server-mode
  - http-api
  - device-session
  - state-machine
  - webui
  - endpoints

---

### FINDING-2026-04-16-phase3-process-isolation-design.md
- topic: Phase 3 process isolation — technical constraints and IPC design validation
- summary: |
    Validates the child-process isolation architecture against real code constraints.
    Application.cs dispatch model makes adding snapshot-worker/analysis-worker subcommands
    trivial. TextureConverter.dll (P/Invoke) is needed by analysis children but covered by
    SetDllDirectory in Main.cs. ShaderExtractor uses spirv-cross.exe via Process.Start only (no P/Invoke).
    .NET 4.7.2 has full System.IO.Pipes. Snapshot worker: bidirectional named pipe JSON-lines;
    SnapshotWorkerProxy on main side awaits TaskCompletionSource keyed to pipe events.
    Analysis worker: stdout JSON-lines; BeginOutputReadLine+OutputDataReceived is the
    correct async pattern. Single-child lock: SnapshotWorkerManager returns 409
    confirm_required; force=true kills existing child and spawns new one.
- related_paths:
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Server/Jobs/JobManager.cs
  - pySdp/webui/static/app.js
- tags:
  - phase3
  - process-isolation
  - ipc
  - named-pipe
  - snapshot-worker
  - analysis-worker
  - child-process

---

### FINDING-2026-04-20-analysis-capture-disconnect.md
- topic: analysis → capture transition: disconnect, state desync, and health monitor false-positive
- summary: |
    Four root-cause candidates for the post-analysis capture disconnect symptom.
    RC1 (HIGH): health monitor calls Disconnect() on transient GetDeviceState()=Unknown during
    second capture, but Disconnect() uses a forced _status assignment (no TryTransition guard)
    so it overrides Capturing state. RC2 (HIGH): TargetProcessRemoved event from LaunchJobRunner
    fires during analysis if the Android app is killed, transitions SessionActive → Connected
    before the second capture starts — user gets 409 "Cannot capture from state 'Connected'".
    RC3 (MEDIUM): shared ManualResetEvent race between captures — trailing OnDataProcessed from
    first capture fires after second capture's Reset(), causing false early WaitOne return.
    RC4 (LOW): switchTab('snapshot') does not call syncDevice(); stale badge for up to 3s;
    doCapture error path re-enables button using stale state.device. Fix map: guard HealthMonitorProc
    against Capturing state; use TryTransition in Disconnect(); add per-capture ManualResetEvent
    instances; call syncDevice() on snapshot tab switch.
- related_paths:
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/LaunchJobRunner.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/Handlers/CaptureHandler.cs
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/ConsolePlatform.cs
  - pySdp/webui/static/app.js
- tags:
  - server-mode
  - state-machine
  - capture
  - analysis
  - health-monitor
  - disconnect
  - frontend
  - tab-switch
  - target-process
  - manual-reset-event

---

### FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
- topic: server mode snapshot/analysis coupling topology — layer-by-layer structural analysis
- summary: |
    Full layer-by-layer coupling map (5 layers). Key correction to prior finding: UpdateThreadProc
    has try/catch INSIDE the while loop — .NET exceptions from client.Update() do NOT kill the process.
    The only real process killer is ConsolePlatform.ExitApplication() → Environment.Exit(0). Analysis
    is already structurally decoupled (AnalysisJobRunner has zero SDK calls, zero DeviceSession mutation).
    Coupling Layer 1 (Critical): shared OS process + ExitApplication bomb. Layer 2 (High): SDK update
    thread runs continuously between captures and during analysis. Layer 3 (Clean): state machine — no
    coupling. Layer 4 (Low): static SdkInitialized singleton. Layer 5 (UI): stale tab-switch badge.
- related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/static/app.js
- tags:
  - server-mode
  - coupling
  - decoupling
  - snapshot
  - analysis
  - sdk-lifecycle
  - process-stability

---

### FINDING-2026-04-15-snapshot-analysis-mode-switch.md
- topic: snapshot → analysis → snapshot mode switch causing "cannot find SDPCLI" / 503 error
- summary: |
    Four root-cause candidates for the snapshot→analysis→snapshot failure in server mode.
    Primary: ConsolePlatform.ExitApplication() calls Environment.Exit(0) — if native SDK
    triggers IPlatform.ExitApplication() during update-thread execution, the entire SDPCLI
    process terminates. Secondary: SDPClient background update thread has no unhandled-exception
    guard (process death on native callback throw). AnalysisJobRunner itself is safe (pure offline,
    no SDK calls, no DeviceSession mutation). The "cannot find SDPCLI" message is 503 (ConnectionError),
    not a 409 state conflict. State machine is correct — CaptureJobRunner always resets to SessionActive.
    NOTE: The "no unhandled-exception guard" finding is CORRECTED by FINDING-2026-04-16 — the
    UpdateThreadProc loop DOES have try/catch inside the loop.
- related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - pySdp/webui/routes/proxy.py
  - pySdp/webui/static/app.js
- tags:
  - server-mode
  - device-session
  - analysis
  - snapshot
  - capture
  - mode-switch
  - environment-exit
  - state-machine

---

### FINDING-2026-04-07-shader-texture-export-structure.md
- topic: shader and texture export structure / drawcall analysis output format
- summary: Per-capture folder isolation duplicates shared assets across snapshots; CSV output lacks asset file references; no JSON for per-DC analysis table
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Modes/DrawCallAnalysisMode.cs
- tags:
  - shader
  - texture
  - export
  - csv
  - json
  - drawcall-analysis

---

### FINDING-2026-04-08-parallelism-thread-safety.md
- topic: parallelism thread-safety audit for analysis pipeline
- summary: |
    Thread-safety audit of ShaderExtractor (safe, full CPU), TextureExtractor/Qonvert DLL
    (conservative degree=4), DrawCallLabelService._llmCache (required ConcurrentDictionary
    fix), LlmApiWrapper (stateless HTTP, safe), AppLogger (internal lock, safe).
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Tools/LlmApiWrapper.cs
- tags:
  - parallel
  - async
  - thread-safety
  - shader
  - texture
  - llm
  - concurrency

---

### FINDING-2026-04-08-mesh-export-pipeline-integration.md
- topic: mesh export — current state and gaps for analysis pipeline integration
- summary: |
    MeshExtractor.cs and MeshExtractionMode.cs are fully implemented. Step 3.5 in
    AnalysisPipeline already calls MeshExtractor but only for top-5 DCs, into a per-capture
    folder, with folder-level existence check and serial execution. mesh_files absent from JSON.
    4 gaps identified: scope, location (sessionDir), per-file check, parallelism.
- related_paths:
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Modes/MeshExtractionMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
- tags:
  - mesh
  - vertex-buffer
  - index-buffer
  - OBJ
  - drawcall-analysis
  - parallel
  - json

---

### FINDING-2026-04-08-db-access-fragmentation.md
- topic: SQLite DB access fragmentation across analysis and extraction classes
- summary: |
    DB access spread across 7 classes with no shared abstraction. Three different connection
    string formats. ShaderExtractor missing Read Only=True. DatabaseQueryService is stateful
    (OpenDatabase pattern); DrawCallQueryService and Extractors are stateless. No unified
    place to configure WAL mode, busy timeout, or add cross-cutting concerns.
- related_paths:
  - SDPCLI/source/Services/Analysis/DatabaseQueryService.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
- tags:
  - database
  - sqlite
  - architecture
  - refactor
  - connection-factory

---

### FINDING-2026-04-08-table-validation.md
- topic: missing pre-analysis table existence validation — CSV custom tables and native SDP tables
- summary: |
    AnalysisPipeline has no pre-flight table check. Each class silently skips queries
    when tables are absent. Most critical: when CsvToDbService has not been run, all 7
    CSV-imported tables (DrawCallParameters, DrawCallBindings, DrawCallVertexBuffers,
    DrawCallIndexBuffers, DrawCallRenderTargets, PipelineVertexInputBindings,
    PipelineVertexInputAttributes) are absent — analysis produces empty/wrong output
    with no user-visible explanation. Defines severity classification: FATAL (no DC source
    at all → throw), ERROR (DrawCallParameters missing → DC list unreliable), WARNING
    (individual CSV/native tables missing). ValidateForAnalysis() proposed for SdpDatabase.Schema.cs.
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/CsvToDbService.cs
  - SDPCLI/source/Data/SdpDatabase.Schema.cs  (target for ValidateForAnalysis)
- tags:
  - validation
  - sqlite
  - csv-import
  - analysis-pipeline
  - table-existence

---

### FINDING-2026-04-09-directory-layout-current-state.md
- topic: directory layout current state — projectDir / workspaceDir / sessionDir / config
- summary: |
    config.ini must be copied to exeDir to load; testPath resolves to SDPCLI/test/ not repo workspace/;
    SDK writes sessions to exeDir/ via cwd; sessionDir concept exists in AnalysisPipeline but is
    unnamed in config; sub-dir names (shaders/textures/meshes) are hardcoded; gfxrz files and analysis
    assets split across two separate roots; SdpFileService scans single directory.
- related_paths:
  - SDPCLI/config.ini
  - SDPCLI/source/Config.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/SessionSummaryService.cs
  - SDPCLI/source/Services/Analysis/SdpFileService.cs
- tags:
  - directory-layout
  - config
  - workspace
  - session
  - paths

---

### FINDING-2026-04-08-drawanalysis-refactor-baseline.md
- topic: drawanalysis pipeline refactor baseline — current state audit
- summary: |
    审计当前 AnalysisPipeline 4-step 流程的耦合问题：DrawCallLabel 缺 subcategory/reason_tag/confidence；
    输出 JSON 混杂 raw 数据与统计；无百分位归因机制；MD 由单次 LLM 调用生成，无内容/归因分层；
    无展示层分离。为两 pass 重构提供基线。
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
- tags:
  - drawcall-analysis
  - refactor
  - json-schema
  - bottleneck
  - attribution
  - llm

---

### FINDING-2026-04-02-env-loading-bootstrap.md
- topic: env loading / config bootstrap
- summary: Investigation of environment variable resolution and config initialization path
- related_paths:
  - src/config
  - scripts/bootstrap
- tags:
  - env
  - config
  - startup

---

### FINDING-2026-04-02-db-query-path.md
- topic: database query path
- summary: Analysis of how queries flow through repository layer and ORM mapping
- related_paths:
  - src/db
  - src/repository
- tags:
  - database
  - query
  - orm

---

## 🧭 Plans

### PLAN-2026-04-22-webui-new-tabs.md
- topic: WebUI new tabs — Explorer (single DC query), Questions (label-aggregated metrics), analysis pipeline trigger wiring
- status: complete
- based_on:
  - FINDING-2026-04-22-webui-new-tabs-plan.md
  - IMPL-2026-04-22-analysis-model-phase-d.md
  - IMPL-2026-04-21-python-data-layer-phase2.md
- summary: |
    3-phase plan. Phase A: "Explorer" tab — snapshot selector + DC list + single DC detail
    panel (shader/texture/mesh/metrics). Phase B: "Questions" tab — new /api/data/label_metrics
    endpoint + frontend table showing per-label aggregated metrics (sum + avg per category).
    Phase C: wire ingest step into PY_STEPS array in app.js so DuckDB is populated automatically
    after C# analysis completes. Only 4 files modified: index.html, app.js, style.css, routes/data.py.
- related_paths:
  - pySdp/webui/static/index.html        (MODIFIED)
  - pySdp/webui/static/app.js            (MODIFIED)
  - pySdp/webui/static/style.css         (MODIFIED)
  - pySdp/webui/routes/data.py           (MODIFIED — new label_metrics endpoint)
- tags:
  - webui
  - frontend
  - explorer
  - questions
  - label-aggregation
  - analysis-trigger
  - duckdb

---

### PLAN-2026-04-21-analysis-model-question-engine.md
- topic: Analysis Model + Question Engine — AnalysisModel interface, model_registry, category_breakdown, Question CRUD, Dashboard
- status: complete
- based_on:
  - FINDING-2026-04-21-analysis-model-question-engine.md
  - PLAN-2026-04-21-python-data-layer.md
  - IMPL-2026-04-21-python-data-layer-phase3.md
- summary: |
    4-phase plan covering Phase 6+7 of the Python data layer. Phase A: AnalysisModel ABC
    (analysis/models/base.py) + model_registry (data/model_registry.py) + 3 builtin models
    (category_breakdown, top_bottleneck_dcs, label_quality) + GET /models and
    POST /models/{name}/run endpoints. Phase B: Question CRUD (5 endpoints), builtin question
    seeding at startup. Phase C: POST /questions/{id}/run endpoint — dispatches to model, merges
    viz_type + viz_config from question row. Phase D: Dashboard CRUD (5 endpoints) + POST
    /dashboards/{id}/run — runs all question panels in order, non-fatal per panel.
    Files: 6 new files in analysis/models/ and data/; 2 modified (server.py, routes/data.py).
- related_paths:
  - pySdp/data/model_registry.py            (NEW)
  - pySdp/analysis/models/__init__.py       (NEW)
  - pySdp/analysis/models/base.py           (NEW)
  - pySdp/analysis/models/category_breakdown.py  (NEW)
  - pySdp/analysis/models/top_bottleneck_dcs.py  (NEW)
  - pySdp/analysis/models/label_quality.py  (NEW)
  - pySdp/webui/routes/data.py              (MODIFIED)
  - pySdp/webui/server.py                   (MODIFIED)
- tags:
  - python
  - analysis-model
  - question-engine
  - dashboard
  - duckdb
  - model-registry
  - category-breakdown
  - phase6
  - phase7

---

### PLAN-2026-04-21-python-data-layer.md
- topic: Python project restructure + persistent data layer — DuckDB global DB, layered architecture
- status: phases-0-3-complete (phases 4-5 pending; phases 6-7 covered by analysis-model-question-engine plan)
- based_on:
  - FINDING-2026-04-21-python-data-layer-design.md
  - FINDING-2026-04-15-raw-data-schema.md
  - IMPL-2026-04-20-python-analysis-services-webui.md
- summary: |
    8-phase plan (Phase 0 = restructure first). Layered architecture: pysdp/ (C# client,
    unchanged) | data/ (DuckDB) | analysis/ (moved from webui/analysis/) | webui/ (thin
    HTTP only). Strict one-way deps: webui→analysis→data. DuckDB single global DB at
    pySdp/data/sdp.db. Phase 0: move webui/analysis/ → analysis/, fix imports, delete old
    dir. Phase 1: WorkspaceDB + ingest_snapshot + /api/data/ingest. Full schema: snapshots,
    draw_calls, shader_stages, dc_shader_stages, textures, dc_textures, meshes, labels
    (incl. bottleneck_text + embedding FLOAT[] pre-reserved), metrics, snapshot_stats,
    questions, dashboards. Phase 2: query.py read API. Phase 3: label/stats persistence.
    Phase 4: LLM bottleneck + DuckDB FTS BM25. Phase 5: vector FLOAT[] cosine similarity.
    Phase 6: analysis model registry (analysis/models/). Phase 7: question engine + dashboard.
    Parquet = export-only one-liner.
- related_paths:
  - pySdp/analysis/                    (NEW — moved from webui/analysis/)
  - pySdp/data/db.py                   (NEW)
  - pySdp/data/ingest.py               (NEW)
  - pySdp/data/query.py                (NEW)
  - pySdp/webui/routes/data.py         (NEW)
  - pySdp/webui/server.py              (MODIFIED)
- tags:
  - python
  - data-layer
  - duckdb
  - architecture
  - restructure
  - drawcall-analysis
  - persistence
  - fts
  - vector
  - label
  - metrics
  - dashboard
  - analysis-model
  - question-engine

---

### PLAN-2026-04-16-phase3-process-isolation.md
- topic: Phase 3 process isolation — snapshot worker child + parallel analysis workers
- status: proposed
- based_on:
  - FINDING-2026-04-16-phase3-process-isolation-design.md
  - FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
  - IMPL-2026-04-16-snapshot-analysis-decoupling-p1-p2.md
- summary: |
    Full design for Phase 3. Server main process becomes SDK-free. Snapshot ops run in a
    single named-pipe child process (same SDPCLI.exe as snapshot-worker subcommand); one child
    at a time enforced by SnapshotWorkerManager (HTTP 409 + force=true override). Analysis ops
    run as parallel child processes (analysis-worker subcommand) writing progress to stdout;
    main reads via BeginOutputReadLine+OutputDataReceived. New files: SnapshotWorkerMode.cs,
    SnapshotWorkerProxy.cs, SnapshotWorkerManager.cs, AnalysisWorkerMode.cs, AnalysisWorkerProxy.cs
    (~1020 lines). Modified: Application.cs, Main.cs, ConnectHandler.cs, app.js. Sub-phases:
    3A (analysis worker, independent), 3B (snapshot worker), 3C (force-connect + UI), 3D (cleanup).
- related_paths:
  - SDPCLI/source/Modes/SnapshotWorkerMode.cs         (NEW)
  - SDPCLI/source/Server/SnapshotWorkerProxy.cs        (NEW)
  - SDPCLI/source/Server/SnapshotWorkerManager.cs      (NEW)
  - SDPCLI/source/Modes/AnalysisWorkerMode.cs          (NEW)
  - SDPCLI/source/Server/AnalysisWorkerProxy.cs        (NEW)
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Server/Handlers/ConnectHandler.cs
  - pySdp/webui/static/app.js
- tags:
  - phase3
  - process-isolation
  - snapshot-worker
  - analysis-worker
  - named-pipe
  - child-process
  - parallel-analysis

---

### PLAN-2026-04-16-snapshot-analysis-decoupling.md
- topic: step-by-step decoupling of snapshot and analysis in server mode
- status: proposed
- based_on:
  - FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
  - FINDING-2026-04-15-snapshot-analysis-mode-switch.md
- summary: |
    Three-phase decoupling plan. Phase 1 (high priority, low risk): suppress
    ConsolePlatform.ExitApplication() in server mode — inject an Action into ConsolePlatform
    constructor so ServerMode can override exit behavior to log + signal graceful shutdown
    instead of calling Environment.Exit(0). Requires changes to ConsolePlatform.cs,
    DeviceSession.cs, ConnectJobRunner.cs, ServerMode.cs. Phase 2 (medium): device health
    monitor in DeviceSession — proactively detect device disconnect before SDK fires a fatal
    event. Phase 3 (optional long-term): child process isolation for snapshot SDK.
    Quick win with no dependency: call syncDevice() on snapshot tab-switch in app.js.
    NOTE: Phase 1 + Phase 2 are IMPLEMENTED (IMPL-2026-04-16). Phase 3 has its own plan.
- related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/static/app.js
- tags:
  - decoupling
  - server-mode
  - snapshot
  - analysis
  - sdk-lifecycle
  - process-stability
  - exit-application

---

### PLAN-2026-04-09-directory-layout-redesign.md
- topic: directory layout redesign — projectDir / workspaceDir / sessionDir structure
- status: proposed
- based_on:
  - FINDING-2026-04-09-directory-layout-current-state.md
- summary: |
    Three-layer path model: projectDir (d:\snapdragon\), workspaceDir (projectDir/workspace/),
    sessionDir (workspaceDir/<session-name>/). config.ini moves to projectDir; Config.cs
    searches projectDir before exeDir. Sub-directory names (shaders/textures/meshes/snapshot/
    db/gfxrz) become explicit config keys Session.ShadersDir etc., shared by CLI and future UI.
    SessionLayout value object reads these keys. SdpFileService.ScanWorkspace() scans workspaceDir.
    SDK session writes redirected to workspaceDir via cwd change. JSON manifest uses relative paths.
    4-phase rollout: P1 config location, P2 workspaceDir, P3 SessionLayout, P4 SDP discovery.
- related_paths:
  - SDPCLI/config.ini
  - SDPCLI/source/Config.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/SessionSummaryService.cs
  - SDPCLI/source/Services/Analysis/SdpFileService.cs
- tags:
  - directory-layout
  - config
  - workspace
  - session
  - paths
  - refactor

---

### PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
- topic: DrawAnalysis 两 pass 重构 — 统计 pass（无 LLM）+ 分析 pass（LLM）完整设计
- status: proposed
- based_on:
  - FINDING-2026-04-08-drawanalysis-refactor-baseline.md
  - FINDING-2026-04-07-shader-texture-export-structure.md
- summary: |
    将 AnalysisPipeline 拆分为 Pass A（统计，无 LLM）和 Pass B（分析，LLM）。
    Pass A 产出 3 个 JSON：snapshot_{id}_raw.json（DC 原始信息 + 扩展 label）、
    snapshot_{id}_status.json（整体统计 + 百分位 + label 质量）、
    snapshot_{id}_topdc.json（per-category top-N 三层归因结果）。
    新增 analysis/attribution_rules.json 三层规则引擎（指标 hint → 百分位比较 → 加权 bottleneck 得分）。
    Pass B 分两步 LLM：Step B1 分析 DC shader 内容，Step B2 输出归因报告 MD；
    Step B3 规则生成展示 Dashboard MD（分类饼图/柱图占位 + top5 表格）。
    定义 8 个新服务类和对应文件布局，DrawCallLabel 扩展为含 subcategory/reason_tags/confidence。
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs  (NEW)
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs  (NEW)
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs  (NEW)
  - SDPCLI/source/Services/Analysis/AttributionReportService.cs  (NEW)
  - SDPCLI/source/Services/Analysis/DashboardGenerationService.cs  (NEW)
  - analysis/attribution_rules.json  (NEW)
- tags:
  - drawcall-analysis
  - two-pass
  - json-schema
  - bottleneck
  - attribution
  - percentile
  - llm
  - refactor

---

### PLAN-2026-04-07-unified-shader-texture-export-json.md
- topic: unified shader/texture folder + drawcall analysis JSON output + parallel extraction + LLM response cache
- based_on:
  - FINDING-2026-04-07-shader-texture-export-structure.md
  - FINDING-2026-04-08-parallelism-thread-safety.md
- summary: |
    Phase 1: Session-level shared shaders/ and textures/ with per-file existence checks;
    DrawCallAnalysis CSV replaced by annotated JSON with shader_files and texture_files per DC.
    Phase 2: Parallel shader+texture extraction (Task.WhenAll + Parallel.ForEach, degree=4 for textures);
    parallel LLM labeling (ConcurrentDictionary + Parallel.ForEach).
    Phase 3: Disk-persisted LLM response ring-pool cache (LlmResponseCache, SHA-256 key, FIFO eviction,
    atomic JSON write); LlmApiWrapper integrated as L2 cache; LoadShaderCode path fixed to flat glob.
- related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Tools/LlmApiWrapper.cs
  - SDPCLI/source/Tools/LlmResponseCache.cs
  - SDPCLI/SDPCLI/config.ini
- tags:
  - shader
  - texture
  - export
  - json
  - deduplication
  - parallel
  - async
  - llm-cache
- status: implemented

---

### PLAN-2026-04-08-unified-db-access.md
- topic: unified SQLite DB access — SdpDatabase in Data/, full query abstraction via partial classes
- status: revised
- based_on:
  - FINDING-2026-04-08-db-access-fragmentation.md
  - FINDING-2026-04-08-table-validation.md
- summary: |
    SdpDatabase placed under source/Data/ (namespace SnapdragonProfilerCLI.Data) as a
    separate data access layer, distinct from Tools (execution logic). C# partial class
    split across 6 domain files (Capture.cs eliminated): SdpDatabase.cs (factory core),
    Schema.cs, DrawCalls.cs, Shaders.cs, Textures.cs, Buffers.cs.
    GetDrawCallIds + 3 helpers → DrawCalls.cs; GetAllTables/GetMetadata → Schema.cs.
    All SQL migrates from 5 in-scope classes into SdpDatabase. Extractors become
    file-I/O-only; QueryServices become thin delegates. Shared helpers deduplicated.
    ShaderExtractor Read Only fix automatic. Phase 7 adds ValidateForAnalysis() pre-flight.
    CsvToDbService and DataExportService out of scope.
- related_paths:
  - SDPCLI/source/Data/SdpDatabase.cs          (NEW — core)
  - SDPCLI/source/Data/SdpDatabase.Schema.cs   (NEW — schema + ValidateForAnalysis)
  - SDPCLI/source/Data/SdpDatabase.DrawCalls.cs (NEW — DC queries + GetDrawCallIds)
  - SDPCLI/source/Data/SdpDatabase.Shaders.cs  (NEW)
  - SDPCLI/source/Data/SdpDatabase.Textures.cs (NEW)
  - SDPCLI/source/Data/SdpDatabase.Buffers.cs  (NEW)
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Services/Analysis/DatabaseQueryService.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
- tags:
  - database
  - sqlite
  - architecture
  - refactor
  - connection-factory
- status: proposed

---

### PLAN-2026-04-11-http-server-mode.md
- topic: HTTP Server Mode — REST API for launch / capture / analysis with async job management
- status: implemented (see IMPL-2026-04-14-http-server-mode.md)

---

## 📦 Implementations

### IMPL-2026-04-29-webui-analysis-ux-fixes.md
- topic: WebUI analysis UX fixes — proxy timeouts, multi-snapshot pipeline, job persistence, explorer DC detail enhancements
- status: completed
- summary: |
    11 fixes across the analysis flow and Explorer DC detail panel:
    (1) proxy.py per-route timeout overrides (capture=300s, connect=120s, etc).
    (2) Multi-snapshot pipeline: JS detects {captureIds,sessionDir} result shape, runs _runPyStepsAll().
    (3) AnalysisJobRunner: per-snapshot phase label "[1/3] snapshot_2 / collect_dc" + scaled progress.
    (4) Job persistence: activeCsJob + activePipelineJob saved to localStorage; _resumeCsJobIfAny /
    _resumePipelineJobIfAny restore state after page refresh.
    (5) Shared snapshot picker auto-refreshes and auto-selects after analysis completes.
    (6) ingest.py: filter stale label rows by valid_api_ids to fix foreign key violation on re-ingest.
    (7) Screenshot extraction: _find_screenshot and _copy_screenshot now extract from .sdp ZIP archive.
    (8) DC detail metric heatmap: metric_stats (median/min/max) via percentile_cont in query.py;
    green→red heatmap coloring in JS; med:xxx display in style.css.
    (9) Render Targets section in DC detail: root cause was shared DB cursor conflict in get_dc_detail —
    fixed by using db.cursor() for every query; RT data read from dc.json.
    (10) Mesh viewer: wireframe toggle button + Verts/Tris stats overlay.
    (11) Shader download: GET /api/files/raw?download=1 sets Content-Disposition; ⬇ button in shader items.
- related_paths:
  - pySdp/webui/routes/proxy.py        (MODIFIED)
  - pySdp/webui/routes/data.py         (MODIFIED)
  - pySdp/webui/routes/files.py        (MODIFIED)
  - pySdp/webui/jobs.py                (MODIFIED)
  - pySdp/data/ingest.py               (MODIFIED)
  - pySdp/data/query.py                (MODIFIED)
  - pySdp/webui/static/app.js          (MODIFIED)
  - pySdp/webui/static/style.css       (MODIFIED)
  - pySdp/webui/static/index.html      (MODIFIED)
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs  (MODIFIED)
- tags: [webui, proxy, timeout, multi-snapshot, progress, job-persistence, ingest, screenshot, heatmap, render-targets, mesh-viewer, shader-download]

---

### IMPL-2026-04-27-result-snapshot-screenshot.md
- topic: Results tab snapshot screenshot display — /api/files/image endpoint + sdp dir fallback
- status: completed
- summary: |
    Each snapshot panel in the Results tab now shows the frame screenshot at the top.
    find_screenshot() first looks in the analysis snapshot dir, then falls back to the
    sibling sdp dir (project/analysis/<run>/snapshot_N → project/sdp/<run>/snapshot_N)
    because screenshots are only written during capture, not copied to analysis output.
    New GET /api/files/image?path= endpoint serves binary image files with correct Content-Type.
    classify_file() skips screenshot/snapshot png/bmp from file lists to avoid duplication.
- related_paths:
  - pySdp/webui/routes/files.py   (MODIFIED)
  - pySdp/webui/static/app.js     (MODIFIED)
  - pySdp/webui/static/style.css  (MODIFIED)
- tags: [webui, frontend, results-tab, screenshot, snapshot]

---

### IMPL-2026-04-27-server-side-pipeline-jobs.md
- topic: Server-side Python analysis pipeline job manager — browser refresh safe
- status: completed
- summary: |
    Python analysis steps (ingest→label→status→topdc→analysis→dashboard) moved from
    browser-orchestrated serial fetch to a server-side background thread. New jobs.py
    with PipelineJob + PipelineJobManager. Three new endpoints: POST /api/data/pipeline,
    GET /api/data/pipeline/{job_id}, POST /api/data/pipeline/{job_id}/cancel.
    Browser submits once and polls; refreshing the page resumes polling via localStorage.
    Old PY_STEPS array removed from app.js.
- related_paths:
  - pySdp/webui/jobs.py           (NEW)
  - pySdp/webui/routes/data.py    (MODIFIED)
  - pySdp/webui/static/app.js     (MODIFIED)
- tags: [webui, pipeline, jobs, threading, analysis-trigger, refresh-safe]

---

### IMPL-2026-04-27-webui-log-window-and-language-hook.md
- topic: WebUI live log CMD window + Korean language guard Stop hook
- status: completed
- summary: |
    WebUI Python process now launches in its own CMD window (same style as SDPCLI) so
    logs are visible live. logger.py adds StreamHandler(stdout) alongside the file handler.
    Stop hook .claude/hooks/check-language.py detects Korean Unicode (가-힣 + Jamo ranges)
    in the last assistant transcript message and blocks with a rewrite instruction.
    Registered in .claude/settings.local.json with 10s timeout.
- related_paths:
  - pySdp/webui/logger.py                    (MODIFIED)
  - pySdp/webui.ps1                          (MODIFIED)
  - .claude/hooks/check-language.py          (NEW)
  - .claude/settings.local.json              (MODIFIED)
- tags: [webui, logging, claude-hook, language-guard, stop-hook]

---

### IMPL-2026-04-22-webui-new-tabs.md
- topic: WebUI new tabs — Explorer (DC browser + detail panel), Questions (label-aggregated metrics), ingest wired into analysis trigger
- status: completed
- summary: |
    Added Explorer and Questions tabs to pySdp WebUI. Explorer: snapshot selector + DC list
    table + single DC detail panel (label, metrics, shaders, textures, mesh). Questions: snapshot
    selector + per-label metrics aggregation table (14 columns, sum + avg). Phase C: added ingest
    as first PY_STEPS entry so DuckDB is populated automatically after C# analysis. New backend
    endpoint: GET /api/data/label_metrics. Shared _loadSnapshotsIntoSelect() helper used by both
    tabs. 0 C# compiler errors.
- related_paths:
  - pySdp/webui/static/index.html        (MODIFIED)
  - pySdp/webui/static/app.js            (MODIFIED)
  - pySdp/webui/static/style.css         (MODIFIED)
  - pySdp/webui/routes/data.py           (MODIFIED)
- tags: [webui, frontend, explorer, questions, label-aggregation, analysis-trigger, duckdb]

---

### IMPL-2026-04-22-analysis-model-phase-d.md
- topic: Analysis Model Phase D — Dashboard CRUD + run endpoint
- status: completed
- summary: |
    Created pySdp/data/dashboards.py (create/list/get/update/delete/seed_builtin_dashboards).
    Seeds builtin-overview dashboard wiring 3 builtin questions at startup. Added 6 REST
    endpoints to routes/data.py (GET/POST /dashboards, GET/PUT/DELETE/POST-run /dashboards/{id}).
    Dashboard run executes each panel independently — one failure does not abort the rest.
    server.py seeds builtin dashboards after question seeding. 19 total routes confirmed.
- related_paths:
  - pySdp/data/dashboards.py           (NEW)
  - pySdp/webui/routes/data.py         (MODIFIED)
  - pySdp/webui/server.py              (MODIFIED)
- tags: [python, dashboard, question-engine, duckdb, crud, phase7]

---

### IMPL-2026-04-22-analysis-model-phase-c.md
- topic: Analysis Model Phase C — Question run endpoint
- status: completed
- summary: |
    Added POST /api/data/questions/{question_id}/run?snapshot_id=<n> to routes/data.py.
    Loads question, dispatches to run_model with question's model_params, overlays viz_type +
    viz_config, attaches question {id, title}. 404 for unknown question or model, 500 otherwise.
- related_paths:
  - pySdp/webui/routes/data.py   (MODIFIED)
- tags: [python, question-engine, analysis-model, duckdb, phase7]

---

### IMPL-2026-04-22-analysis-model-phase-b.md
- topic: Analysis Model Phase B — Question CRUD + builtin question seeding
- status: completed
- summary: |
    Created pySdp/data/questions.py (create/list/get/update/delete/seed_builtin_questions).
    CAST(created_at AS VARCHAR) avoids DuckDB pytz dependency. Seeds 3 built-in questions
    (builtin-category-breakdown, builtin-top-bottleneck-dcs, builtin-label-quality) idempotently.
    Added 5 REST endpoints + QuestionCreate/Update Pydantic models to routes/data.py.
    POST /questions validates model_name against registry (400 if unknown).
- related_paths:
  - pySdp/data/questions.py           (NEW)
  - pySdp/webui/routes/data.py        (MODIFIED)
  - pySdp/webui/server.py             (MODIFIED)
- tags: [python, question-engine, duckdb, crud, phase7]

---

### IMPL-2026-04-22-analysis-model-phase-a.md
- topic: Analysis Model Phase A — AnalysisModel ABC, model_registry, 3 builtin models, model list/run endpoints
- status: completed
- summary: |
    Created analysis/models/ package: AnalysisModel base class + 3 builtin models
    (category_breakdown bar_chart, top_bottleneck_dcs table, label_quality bar_chart).
    data/model_registry.py is a pure dispatch table (no analysis/ imports — no circularity).
    Registration triggered by `import analysis.models` in server.py. Added GET /models and
    POST /models/{name}/run?snapshot_id=<n> endpoints.
- related_paths:
  - pySdp/analysis/models/__init__.py       (NEW)
  - pySdp/analysis/models/base.py           (NEW)
  - pySdp/analysis/models/category_breakdown.py  (NEW)
  - pySdp/analysis/models/top_bottleneck_dcs.py  (NEW)
  - pySdp/analysis/models/label_quality.py  (NEW)
  - pySdp/data/model_registry.py            (NEW)
  - pySdp/webui/routes/data.py              (MODIFIED)
  - pySdp/webui/server.py                   (MODIFIED)
- tags: [python, analysis-model, model-registry, duckdb, phase6]

---

### IMPL-2026-04-21-python-data-layer-phase3.md
- topic: Python data layer Phase 3 — label/stats persistence to DuckDB + refresh_labels endpoint
- status: completed
- summary: |
    label_service.generate_label_json(db=None) upserts labels into DuckDB after writing label.json.
    status_service.generate_status_json(db=None) upserts per-category rows into snapshot_stats.
    files.py refactored to make_router(db=None) factory; /label and /status pass db to services.
    Added POST /api/data/refresh_labels?snapshot_id=<n> — looks up snapshot_dir from DB, re-runs
    label+status services, persists results.
- related_paths:
  - pySdp/analysis/label_service.py    (MODIFIED)
  - pySdp/analysis/status_service.py   (MODIFIED)
  - pySdp/webui/routes/files.py        (MODIFIED)
  - pySdp/webui/routes/data.py         (MODIFIED)
  - pySdp/webui/server.py              (MODIFIED)
- tags: [python, data-layer, duckdb, label, persistence, phase3]

---

### IMPL-2026-04-21-python-data-layer-phase2.md
- topic: Python data layer Phase 2 — typed query API + draw_calls / dc detail endpoints
- status: completed
- summary: |
    Created pySdp/data/query.py: get_draw_calls, get_labels, get_metrics, get_dc_detail, query_dcs.
    Tags filtering Python-side. Added GET /api/data/draw_calls and GET /api/data/dc/{api_id} endpoints.
- related_paths:
  - pySdp/data/query.py           (NEW)
  - pySdp/webui/routes/data.py    (MODIFIED)
- tags: [python, data-layer, duckdb, query-layer, phase2]

---

### IMPL-2026-04-21-python-data-layer-phase1.md
- topic: Python data layer Phase 1 — DuckDB WorkspaceDB + ingest_snapshot + /api/data/* endpoints
- status: completed
- summary: |
    Created pySdp/data/ package: WorkspaceDB (DuckDB at pySdp/data/sdp.db, configurable via
    SDP_DB_PATH), 12-table schema, ingest_snapshot() (INSERT OR REPLACE, single transaction).
    POST /api/data/ingest and GET /api/data/snapshots endpoints. duckdb added to requirements.txt.
- related_paths:
  - pySdp/data/__init__.py         (NEW)
  - pySdp/data/db.py               (NEW)
  - pySdp/data/ingest.py           (NEW)
  - pySdp/webui/routes/data.py     (NEW)
  - pySdp/webui/server.py          (MODIFIED)
  - pySdp/requirements.txt         (MODIFIED)
- tags: [python, data-layer, duckdb, schema, ingest, phase1]

---

### IMPL-2026-04-21-python-data-layer-phase0.md
- topic: Python project restructure Phase 0 — move webui/analysis/ → analysis/, fix sys.path in server.py
- status: completed
- summary: |
    Moved 5 analysis service files from pySdp/webui/analysis/ to new top-level pySdp/analysis/.
    Added sys.path.insert(0, pySdp_root) to server.py. Deleted pySdp/webui/analysis/.
- related_paths:
  - pySdp/analysis/                    (NEW)
  - pySdp/webui/server.py              (MODIFIED)
- tags: [python, restructure, analysis, data-layer, phase0]

---

### IMPL-2026-04-20-python-analysis-services-webui.md
- topic: Python WebUI analysis services (label/status/topdc/dashboard/analysis_md) + Results tab + hybrid C#/Python pipeline
- status: completed
- summary: |
    5 Python services under pySdp/webui/analysis/ (now moved to pySdp/analysis/ by phase0).
    label_service: rule-based DC classification. status_service: percentile blocks p50-p99.
    topdc_service: 3-layer attribution engine (attribution_rules.json). dashboard_service: Mermaid charts.
    analysis_md_service: per-category LLM hook + rule-based fallback. 5 POST endpoints under /api/files/.
    Results tab with run selector + 3-layer collapsible file sections. Hybrid C#/Python pipeline.
- related_paths:
  - pySdp/analysis/label_service.py
  - pySdp/analysis/status_service.py
  - pySdp/analysis/topdc_service.py
  - pySdp/analysis/dashboard_service.py
  - pySdp/analysis/analysis_md_service.py
  - pySdp/webui/routes/files.py
  - pySdp/webui/static/app.js
  - SDPCLI/analysis/attribution_rules.json
- tags: [python, webui, analysis, label, status, topdc, dashboard, attribution, results-tab]

---

### IMPL-2026-04-14-http-server-mode.md
- topic: HTTP Server Mode — implementation record
- status: completed (build 0 errors)
- summary: 21 new files under SDPCLI/source/Server/ + Modes/ServerMode.cs; AnalysisPipeline.cs completedTargets; Main.cs/Application.cs server subcommand
- tags: [server-mode, http-api, device-session]

---

### PLAN-2026-04-03-3d-model-extraction.md
- topic: 3D model extraction from vertex/index buffers + analysis pipeline mesh integration
- based_on:
  - FINDING-2026-04-03-3d-model-extraction-feasibility.md
  - FINDING-2026-04-08-mesh-export-pipeline-integration.md
- summary: |
    Phases 0-5 implemented: MeshExtractor.cs (OBJ from VB/IB binary via VulkanSnapshotByteBuffers)
    and MeshExtractionMode.cs (standalone CLI) are done. Phase 6 upgrades Step 3.5 in the
    analysis pipeline: session-shared meshes/ folder, all non-compute DCs with VB bindings,
    per-file existence check, Parallel.ForEach (MeshExtractionDegree=4), and mesh_files +
    vertex_buffers + index_buffer fields in DrawCallAnalysis JSON (schema_version 2.1).
- related_paths:
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Modes/MeshExtractionMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/SDPCLI/config.ini
- tags:
  - mesh
  - vertex-buffer
  - index-buffer
  - OBJ
  - drawcall-analysis
  - parallel
  - json
  - pipeline-integration
- status: partially-implemented

---

### PLAN-2026-04-02-fix-config-bootstrap.md
- topic: fix env loading strategy
- based_on:
  - FINDING-2026-04-02-env-loading-bootstrap.md
- summary: Proposed fix for config initialization ordering and fallback handling
- related_paths:
  - src/config
  - scripts/bootstrap
- tags:
  - env
  - config
  - fix

---

## 📌 Decisions

### DECISION-2026-04-02-context-resolution-required.md
- topic: mandatory context resolution before implementation
- summary: Agents must read README.md and context docs before making changes
- tags:
  - workflow
  - governance

---

## 🧠 Notes for Agents

- Findings = evidence and root cause analysis
- Plans = actionable solutions based on findings
- Implementations = actual execution state, deviations, and validation outcomes
- Decisions = confirmed rules or constraints

When implementing:
→ Always prefer PLAN over FINDING
→ Read IMPLEMENTATION records before iterating on existing work

When investigating:
→ Start from FINDING
→ Use IMPLEMENTATION records to understand real execution state

When rules or constraints conflict:
→ Prefer DECISION documents

---

## 🔄 Maintenance Rules

When adding new documents:

- Add new FINDING entries under Findings
- Add new PLAN entries under Plans
- Add new IMPL entries under Implementations
- Add new DECISION entries under Decisions

Keep:
- topic short and clear
- paths accurate
- tags consistent

Do NOT:
- rewrite entire index
- remove historical entries unless obsolete

---

## 📍 Quick Tag Reference (optional)

Common tags:
- env
- config
- startup
- database
- rendering
- pipeline
- performance
- shader
- build
- implementation
- partial
