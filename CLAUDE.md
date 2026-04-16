# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## Build & Run

```powershell
# Build
dotnet build SDPCLI

# Run (interactive menu)
.\sdpcli.bat

# Run server mode
.\sdpcli.bat server --port 5000

# Run snapshot mode
.\sdpcli.bat snapshot

# Run analysis mode
.\sdpcli.bat analysis
```

Output binary: `SDPCLI\bin\Debug\net472\SDPCLI.exe`

The MSBuild targets automatically copy DLLs from `dll/`, plugins from `dll/plugins/`, android service files, and `config.ini` to the output directory. There is no separate test suite — validation is done by building and running the tool against real `.sdp` capture files in `SDPCLI/test/`.

---

## Architecture

**Snapdragon Profiler CLI** is a .NET Framework 4.7.2 (x64) headless tool wrapping the Qualcomm Snapdragon Profiler SDK via P/Invoke for GPU profiling, frame capture, and offline analysis.

### Three Operational Modes

**1. Snapshot Mode** — Connect to Android device via ADB, trigger a Vulkan frame capture, export GPU data as CSV, store in SQLite, archive as `.sdp` (ZIP).

**2. Analysis Mode** — Offline post-processing: extract `.sdp`, query SQLite DrawCall data, extract assets (SPIR-V→HLSL via `spirv-cross.exe`, textures→PNG via `TextureConverter.dll`, VB/IB→OBJ), optionally label DrawCalls via LLM, join GPU counter metrics, generate JSON + Markdown reports.

**3. Server Mode** — HTTP REST API (localhost only) wrapping snapshot and analysis as async jobs for CI/scripting use.

### Entry Point Chain

```
Program.Main()         # DLL path setup, arg parsing, logging init
  └─ Application.Run() # Service wiring + subcommand router
       ├─ SnapshotCaptureMode.Run()
       ├─ AnalysisMode.Run() → AnalysisPipeline.RunAnalysis()
       ├─ ServerMode.Run()   → HttpServer + JobManager
       └─ InteractiveMode.Run() (no subcommand)
```

Key source locations:
- [SDPCLI/source/Main.cs](SDPCLI/source/Main.cs) — entry point
- [SDPCLI/source/Application.cs](SDPCLI/source/Application.cs) — mode router
- [SDPCLI/source/Modes/](SDPCLI/source/Modes/) — SnapshotCaptureMode, AnalysisMode, ServerMode, InteractiveMode
- [SDPCLI/source/Services/](SDPCLI/source/Services/) — business logic services
- [SDPCLI/source/Data/](SDPCLI/source/Data/) — SdpDatabase (partitioned: Schema, DrawCalls, Shaders, Textures, Buffers)
- [SDPCLI/source/Server/](SDPCLI/source/Server/) — HttpServer, JobManager, job runners
- [SDPCLI/source/Tools/](SDPCLI/source/Tools/) — ShaderExtractor, TextureExtractor, MeshExtractor

### Data Pipeline Summary

| Stage | Input | Output |
|---|---|---|
| Capture | Device USB (SDK) | 7 CSVs + screenshot |
| Import | CSVs | `sdp.db` rows keyed by CaptureID |
| Archive | Session dir | `.sdp` ZIP |
| Extract | `.sdp` ZIP | `sdp.db` + `snapshot_N/` |
| Query | `sdp.db` + CaptureID | DrawCall objects with resource refs |
| Asset extraction | Shader/Texture/Buffer tables | HLSL, PNG, OBJ files |
| Label | DrawCalls + shaders | Category + detail (rule-based or LLM) |
| Report | Labeled DCs + GPU metrics | `dc.json`, `status.json`, `topdc.json`, `analysis.md`, `dashboard.md` |

### Native SDK Integration

The tool wraps native DLLs via P/Invoke. `Program.Main()` calls `SetDllDirectory()` and modifies `PATH` to point to `dll/` and `dll/plugins/` before any SDK types are loaded. Do not move DLL-dependent code above this initialization.

Key native dependencies: `SDPCoreWrapper.dll`, `SDPClientFramework.dll`, `QGLPlugin.dll`, `TextureConverter.dll`, `spirv-cross.exe` (Vulkan SDK).

### Configuration

`SDPCLI/config.ini` is copied to the exe directory at build time. It controls: target APK package/activity, rendering API, LLM endpoint, extraction parallelism, metrics whitelist, and output paths.

---

## Documentation & Context System

This repository uses a structured knowledge system. **Read this before modifying code.**

### Mandatory reading order before any work

1. `README.md`
2. `docs/context/INDEX.md` — active findings, plans, implementations
3. `INDEX.md` (repo root) — module routing index
4. `docs/index/modules/*.md` — per-module documentation
5. `docs/context/` relevant docs
6. Source code

### Context priority

```
decisions > implementations > plans > findings > code
```

Code may be WIP, partially migrated, or ahead of/behind the latest plan. Always reconcile with context before assuming code is ground truth.

### Documentation locations

| Path | Purpose |
|---|---|
| `docs/context/findings/` | Investigation results |
| `docs/context/plans/` | Proposed changes |
| `docs/context/implementations/` | What was actually built (write here after executing) |
| `docs/context/decisions/` | Stable, locked decisions |
| `docs/explanations/` | Durable project documentation (pipelines, architecture) — the ONLY place for this |
| `docs/index/modules/` | Per-module routing and scope docs |

**Do not** write project explanations into `docs/context/`. **Do not** write context/state into `docs/explanations/`.

### AI Workflow

```
Investigate → Plan → Execute → Validate → Document → Index Sync
```

- **Investigator**: analyze, write findings/plans. Never modify code.
- **Executor**: implement approved plan, validate via `dotnet build`, write to `docs/context/implementations/`.
- **Index Sync**: update `INDEX.md` and `docs/index/modules/*.md` when module boundaries change. No renames/merges/splits without explicit intent.
- **Doc Explanation**: write durable docs to `docs/explanations/` only. Read implementations before code.
