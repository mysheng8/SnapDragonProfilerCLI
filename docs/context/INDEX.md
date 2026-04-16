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

### IMPL-2026-04-14-http-server-mode.md
- topic: HTTP Server Mode — implementation record
- status: completed (build 0 errors)
- summary: 21 new files under SDPCLI/source/Server/ + Modes/ServerMode.cs; AnalysisPipeline.cs completedTargets; Main.cs/Application.cs server subcommand
  - session
  - device-session

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
