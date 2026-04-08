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