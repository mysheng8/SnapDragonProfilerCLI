---
type: implementation
topic: CLI Redesign — Positional Subcommand + AnalysisTarget Enum + Path Resolution
status: completed
based_on:
  - PLAN-2026-04-10-cli-redesign-analysis-split.md
related_paths:
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/InteractiveMode.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/RawJsonGenerationService.cs
  - SDPCLI/source/Services/Analysis/SubJsonLoadService.cs
  - SDPCLI/config.ini
summary: >
  P0 + P1 + P2 + Snapshot rollback complete.
  Positional subcommand routing, AnalysisTarget step gating,
  6 sub-JSON writers, snapshot index manifest, SubJsonLoadService,
  B-only shortcut path, deprecated alias warnings, --no-extract flag,
  CLI_PARAMETERS.md rewrite, SDPCLI.bat update.
  Rollback: removed -launch/-capture flags from SnapshotCaptureMode;
  restored ENTER/ESC minimal interactive loop (allows multi-capture per .sdp).
last_updated: 2026-04-10
---

## Plan Reference

PLAN-2026-04-10-cli-redesign-analysis-split.md §1–§3, §5 P0 checklist, §6 P1 checklist.

## Implementation Summary

### P0 (completed previously)
- `Main.cs`: new positional subcommand parser, `--debug` only (double-dash)
- `Application.cs`: `Run()` signature rewritten; routes to InteractiveMode/AnalysisMode/SnapshotCaptureMode
- `Modes/InteractiveMode.cs`: **new file** — wraps all legacy interactive menu logic
- `Modes/AnalysisMode.cs`: new constructor params (`snapshotId`, `targetArg`, `outputArg`), SdpDir→ProjectDir path resolution, `ResolveOutputDir()`
- `Modes/SnapshotCaptureMode.cs`: `packageActivity` positional arg; `-launch/-l` / `-capture/-c`; non-interactive single-shot capture; `outputArg` → `ResolveSdpOutputDir()`
- `Analysis/AnalysisPipeline.cs`: `AnalysisTarget [Flags]` enum + `ExpandWithDependencies()` + `Parse()`
- `config.ini`: documented 7 new keys as commented-out defaults

### P1 (completed this session)
- `Services/Analysis/RawJsonGenerationService.cs`: 6 new writer methods (`WriteDcJson`, `WriteLabelJson`, `WriteMetricsJson`, `WriteShadersJson`, `WriteTexturesJson`, `WriteBuffersJson`); `WriteIndexJson` manifest; private helpers `SaveSnapshotJson` + `BuildShaderFileList`
- `Analysis/AnalysisPipeline.cs`:
  - `RunAnalysis()` now accepts `AnalysisTarget target = AnalysisTarget.All`
  - `ExpandWithDependencies()` called at entry; all steps gated by per-flag booleans
  - 6 split writers called instead of monolithic `GenerateLabeledMetricsJson`
  - A5/A6 gated by `Status`/`TopDc`; B1/B2/B3 gated by `Analysis`/`Dashboard`
  - **B-only shortcut**: when target is exclusively `Analysis|Dashboard` and `dc.json` exists on disk, loads from `SubJsonLoadService` and skips Steps 1–3.5 entirely; missing prerequisites → clear error message
  - `snapshot_{N}_index.json` manifest written at pipeline end
- `Modes/AnalysisMode.cs`: `targetArg` → `AnalysisTargetExtensions.Parse()` → both `RunAnalysis()` call sites
- `Services/Analysis/SubJsonLoadService.cs`: **new file** — reads 6 sub-JSONs and rebuilds `DrawCallAnalysisReport`; join key `api_id`; partial loads supported (missing files silently skipped)
- `Main.cs`: added `[DEPRECATED]` console warnings to `-pass-mode`, `-stats-only`, `-analysis-only` with alternative `-t` suggestions

### P2 (completed this session)
- `Main.cs`: `--no-extract` flag parsed (double-dash, not lowercased)
- `Application.cs`: `noExtract` param added to `Run()` and `BuildAnalysisMode()`; sets `AnalysisNoExtract=true` in config when active
- `Analysis/AnalysisPipeline.cs`: reads `AnalysisNoExtract` config bool; `doExtractShaders/Textures/Meshes` all gated on `!noExtract`; logs `--no-extract` notice when active
- `SDPCLI/CLI_PARAMETERS.md`: full rewrite — new subcommand syntax, `-t`/`-s`/`-o`/`--no-extract` flags, target cascade table, config key table, output structure diagram, updated examples
- `SDPCLI.bat`: inline usage examples added as comments

### Fully deferred (P2 remaining)
- Timestamp validity check (`snapshot_N_index.json generated_at` vs `sdp.db` mtime)
- `AnalysisMode.SelectSdpFileInteractively()` → scan SdpDir instead of `TestDirectory`

## Files Changed

| File | Type | Summary |
|------|------|---------|
| `source/Main.cs` | Modified (P0+P1) | Subcommand parser + deprecated alias warnings |
| `source/Application.cs` | Modified (P0) | `Run()` routing + `BuildAnalysisMode()` helper |
| `source/Modes/InteractiveMode.cs` | **New** (P0) | Full interactive menu |
| `source/Modes/AnalysisMode.cs` | Modified (P0+P1) | New params + path resolution + target wire-up |
| `source/Modes/SnapshotCaptureMode.cs` | Modified (P0) | Non-interactive capture + outputArg |
| `source/Analysis/AnalysisPipeline.cs` | Modified (P0+P1+P2) | AnalysisTarget enum + full step gating + B-only shortcut + index manifest + noExtract |
| `source/Services/Analysis/RawJsonGenerationService.cs` | Modified (P1) | 6 sub-JSON writers + WriteIndexJson |
| `source/Services/Analysis/SubJsonLoadService.cs` | **New** (P1) | Rebuild report from 6 disk JSONs |
| `SDPCLI/CLI_PARAMETERS.md` | Modified (P2) | Full rewrite with new CLI syntax |
| `SDPCLI.bat` | Modified (P2) | Usage examples added |
| `config.ini` | Modified (P0) | 7 new keys (commented-out defaults) |

## Build / Validation

```
Iteration 1 (P2): CS1056 unexpected '\' in AnalysisPipeline.cs line 157 — literal `\n` embedded in string from replace_string_in_file → fixed by clean replacement
Iteration 2 (P2): Build succeeded.  0 Warning(s)  0 Error(s)
```

## Deviations from Plan

- `GenerateLabeledMetricsJson` retained for backward compat; pipeline now calls 6 writers instead
- `skipPassAGen` is reused (set to `true`) in the B-only shortcut path to suppress sub-JSON writes
- `db` declared as `SdpDatabase db = null!` (not created in B-only shortcut path); extraction steps are already gated by flags so null is never dereferenced
- Step B2/B3 individually gated inside `runPassB` block (allows `-t dashboard` without `-t analysis`)

## Issues Encountered

- `SubJsonLoadService` missing `using SnapdragonProfilerCLI.Modes` for ILogger → fixed in build iter 1
- Cascade expansion for `AnalysisTarget.Analysis` includes `Shaders` (via Label→Shaders); B-only shortcut forcibly overrides all extraction flags to false to prevent DB-based extraction from running

## Next Steps (remaining deferred)

All deferred items resolved. Implementation complete.

---

## Snapshot Rollback (2026-04-10)

### Reason

SDK `CreateTimestampedSubDirectory = true` → each `SDPClient.Initialize()` creates an independent session dir with its own `sdp.db`. Therefore `-launch` + `-capture` as a two-step non-interactive workflow is architecturally invalid (`-launch` exits with `Cleanup()` → connection lost; subsequent `-capture` must fully re-initialize). The original ENTER/ESC interactive loop is the correct design: single process, same session, multiple captures → one `.sdp`.

### Changes Made

| File | Change |
|------|--------|
| `source/Main.cs` | Removed `doLaunch`/`doCapture` bool vars; removed `-launch/-l` and `-capture/-c` flag cases; removed params from `app.Run()` call |
| `source/Application.cs` | Removed `doLaunch`/`doCapture` from `Run()` signature; removed guard `if (!doLaunch && !doCapture)` + error message; removed params from `SnapshotCaptureMode` constructor call |
| `source/Modes/SnapshotCaptureMode.cs` | Removed `doLaunch`/`doCapture` constructor params; removed `_NonInteractiveMode` config key writes; removed `IsNonInteractive` property; removed `|| IsNonInteractive` from `autoStart`; removed `-launch` early-return block; always print "Press ENTER…"; removed `if (IsNonInteractive) break` single-shot exit |
| `SDPCLI.bat` | Replaced `-c`/`-l` example lines with single `sdpcli snapshot com.pkg/.Activity` line |

### Build / Validation

```
Build succeeded.  0 Warning(s)  0 Error(s)
```

### New Behavior

```
sdpcli snapshot [<package\activity>]   # ENTER=capture (repeatable) / ESC=exit+.sdp
```

## Plan Reference

PLAN-2026-04-10-cli-redesign-analysis-split.md §1–§3, §5 P0 checklist, §6 P1 checklist.

## Implementation Summary

### P0 (completed previously)
- `Main.cs`: new positional subcommand parser, `--debug` only (double-dash)
- `Application.cs`: `Run()` signature rewritten; routes to InteractiveMode/AnalysisMode/SnapshotCaptureMode
- `Modes/InteractiveMode.cs`: **new file** — wraps all legacy interactive menu logic
- `Modes/AnalysisMode.cs`: new constructor params (`snapshotId`, `targetArg`, `outputArg`), SdpDir→ProjectDir path resolution, `ResolveOutputDir()`
- `Modes/SnapshotCaptureMode.cs`: `packageActivity` positional arg; `-launch/-l` / `-capture/-c`; non-interactive single-shot capture; `outputArg` → `ResolveSdpOutputDir()`
- `Analysis/AnalysisPipeline.cs`: `AnalysisTarget [Flags]` enum + `ExpandWithDependencies()` + `Parse()`
- `config.ini`: documented 7 new keys as commented-out defaults

### P1 (completed this session)
- `Services/Analysis/RawJsonGenerationService.cs`: 6 new writer methods (`WriteDcJson`, `WriteLabelJson`, `WriteMetricsJson`, `WriteShadersJson`, `WriteTexturesJson`, `WriteBuffersJson`); `WriteIndexJson` manifest; private helpers `SaveSnapshotJson` + `BuildShaderFileList`
- `Analysis/AnalysisPipeline.cs`: `RunAnalysis()` now accepts `AnalysisTarget target = AnalysisTarget.All`; `ExpandWithDependencies()` called at entry; all pipeline steps gated by `doExtractShaders/doExtractTextures/doLabel/doMetrics/doExtractMeshes/targetIsAll`; 6 split writers called instead of monolithic `GenerateLabeledMetricsJson`; A5/A6 gated by `Status`/`TopDc` flags; B1/B2/B3 gated by `Analysis`/`Dashboard` flags; `snapshot_{N}_index.json` written at end with all product paths
- `Modes/AnalysisMode.cs`: `targetArg` parsed via `AnalysisTargetExtensions.Parse()` and passed to both `RunAnalysis()` call sites

Not yet implemented (P1 remaining / P2):
- `SubJsonLoadService.cs` for `-t analysis` standalone (load report from 6 disk JSONs instead of DB)
- Cascade prerequisite file existence check (blocked on SubJsonLoadService)
- Deprecated alias conversion (`-stats-only` / `-analysis-only` / `-pass-mode` → AnalysisTarget)
- `AnalysisMode.SelectSdpFileInteractively()` still uses `TestDirectory` config key (not yet SdpDir scan)

## Files Changed

| File | Type | Summary |
|------|------|---------|
| `source/Main.cs` | Modified (P0) | New subcommand parser, `--debug` only |
| `source/Application.cs` | Modified (P0) | `Run()` signature + routing; `BuildAnalysisMode()` helper |
| `source/Modes/InteractiveMode.cs` | **New** (P0) | Full interactive menu extracted from Application.cs |
| `source/Modes/AnalysisMode.cs` | Modified (P0+P1) | New params, SdpDir/ProjectDir resolution; target parsing + RunAnalysis wire-up |
| `source/Modes/SnapshotCaptureMode.cs` | Modified (P0) | `packageActivity` param, `-launch/-l` / `-capture/-c`, single-shot exit |
| `source/Analysis/AnalysisPipeline.cs` | Modified (P0+P1) | `AnalysisTarget` enum; `RunAnalysis()` target param + full step gating; 6 writers; index manifest |
| `source/Services/Analysis/RawJsonGenerationService.cs` | Modified (P1) | 6 sub-JSON writer methods + `WriteIndexJson` + private helpers |
| `config.ini` | Modified (P0) | Documented 7 new keys (commented-out with defaults) |

## Build / Validation

```
Build succeeded.  0 Warning(s)  0 Error(s)   (Iteration 1, clean)
```

## Deviations from Plan

- `GenerateLabeledMetricsJson` retained for backward compat (caller outside scope might still use it); pipeline now calls 6 writers instead
- Step B2/B3 are individually gated inside the B block (`if Analysis` / `if Dashboard`) rather than at the outer runPassB level — allows running just Dashboard without Analysis
- `sdpName2` variable introduced in pipeline (= `Path.GetFileName(sessionDir)`) to avoid repeated `System.IO.Path.GetFileName(sessionDir)` calls

## Issues Encountered

- Multi-replace unicode escape sequences vs actual UTF-8 chars in file caused 2 failures on Step 2 / Step 3 changes — retried with direct Unicode literals
- B1/B2/B3 block replacement failed (string mismatch on System.IO.Path references); applied as separate single replacement after

## Next Steps

1. `SubJsonLoadService.cs` — rebuild `DrawCallAnalysisReport` from 6 disk JSONs (unblocks `-t analysis` standalone)
2. Cascade prerequisite file check before each step when target is non-All (depends on SubJsonLoadService)
3. Deprecated alias: `-stats-only` / `-analysis-only` → AnalysisTarget conversion in Main.cs
4. `AnalysisMode.SelectSdpFileInteractively()` — update scan from `TestDirectory` to SdpDir