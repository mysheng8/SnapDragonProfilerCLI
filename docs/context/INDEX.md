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
- Decisions = confirmed rules or constraints

When implementing:
→ Always prefer PLAN over FINDING

When investigating:
→ Start from FINDING

---

## 🔄 Maintenance Rules

When adding new documents:

- Add new FINDING entries under Findings
- Add new PLAN entries under Plans
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