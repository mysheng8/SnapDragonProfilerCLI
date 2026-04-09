---
type: plan
topic: directory layout redesign — projectDir / workspaceDir / sessionDir structure
status: proposed
based_on:
  - FINDING-2026-04-09-directory-layout-current-state.md
related_paths:
  - SDPCLI/config.ini
  - SDPCLI/source/Config.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/DataExportService.cs
  - SDPCLI/source/Services/Capture/SessionSummaryService.cs
  - SDPCLI/source/Services/Analysis/SdpFileService.cs
related_tags:
  - directory-layout
  - config
  - workspace
  - session
  - paths
  - refactor
summary: >
  Redesign the path model into three explicit layers: projectDir (repo root),
  workspaceDir (all captured/analyzed data, at projectDir/workspace/), and
  sessionDir (per-capture or per-analysis folder under workspaceDir). Move
  config.ini to projectDir. Config.cs searches projectDir before exeDir.
  Sub-directory names for shaders/textures/meshes/snapshot/db/gfxrz become
  explicit config keys shared between CLI and future UI.
last_updated: 2026-04-09
---

# PLAN-2026-04-09: Directory Layout Redesign

## 1. Target Directory Hierarchy

```
d:\snapdragon\                           ← projectDir (repo root)
├── config.ini                           ← MOVED from SDPCLI/; new canonical location
├── workspace\                           ← workspaceDir (was SDPCLI/test/)
│   └── <session-name>\                  ← sessionDir
│       ├── *.sdp                        ← SDP archive (placed here by capture mode)
│       ├── snapshot\                    ← extracted SDP content (sdp.db lives here)
│       │   └── sdp.db
│       ├── db\                          ← SDK-generated db / gfxrz files
│       │   ├── sdpframe_NNN.gfxrz
│       │   └── sdpframestripped_NNN.gfxrz
│       ├── shaders\                     ← extracted shader files
│       ├── textures\                    ← extracted texture images
│       ├── meshes\                      ← extracted 3D mesh files
│       ├── snapshot_{captureId}\        ← per-capture analysis outputs (CSV, JSON, screenshot)
│       └── session.json                 ← manifest with relative paths (generated)
├── dll\                                 ← dllDir (unchanged)
├── SDPCLI\                              ← cli sub-project
│   ├── source\                          ← srcDir (unchanged)
│   └── bin\                             ← exeDir (compiled output)
│       └── Debug\net472\
│           └── .log\                   ← consoleLog (unchanged)
└── sdpui\                               ← ui sub-project (future)
```

### Session Naming Convention

| Origin | Session Name Pattern | Example |
|--------|---------------------|---------|
| Capture (SDK-generated) | `yyyy-MM-ddTHH-mm-ss` | `2026-04-09T14-30-00` |
| Analysis of existing SDP | `<sdp-basename>` | `2026-04-02T12-06-33` |

Both patterns already occur in the codebase. The key change is the **root**:
they must land under `workspaceDir/`, not under `exeDir/` or `SDPCLI/test/`.

---

## 2. Config.ini Changes

### File Location
Move `SDPCLI/config.ini` → `projectDir/config.ini` (`d:\snapdragon\config.ini`).

The file currently in `SDPCLI/` becomes the canonical location. The build system
should reference `projectDir/config.ini` directly (or symlink). The copy in
`exeDir` is no longer the canonical file.

### New Config Keys

Add the following keys. All existing keys are preserved.

```ini
# ── Project directories ────────────────────────────────────────────────────
# Absolute path to the repo root. Used as base for all other relative paths.
# Defaults to two levels above config.ini location if not set.
ProjectDir=D:\snapdragon

# All captured and analyzed session data lives here (replaces SDPCLI/test/).
WorkspaceDir=D:\snapdragon\workspace

# ── Session sub-directory names (relative to any sessionDir) ──────────────
# Both CLI and UI must use these keys to locate assets.
Session.SnapshotDir=snapshot
Session.DbDir=db
Session.ShadersDir=shaders
Session.TexturesDir=textures
Session.MeshesDir=meshes
Session.GfxRzDir=db
```

> **Note on GfxRzDir**: gfxrz files and sdp.db are both SDK-generated and land
> in the same SDK session directory. After extraction, sdp.db moves to
> `Session.SnapshotDir`; gfxrz files stay in `Session.DbDir`. Both can be the
> same physical directory initially; they are separate keys for clarity.

### Keys to Remove / Deprecate

| Old Key | Replacement | Notes |
|---------|-------------|-------|
| `WorkingDirectory` | `ProjectDir` | rename; `WorkingDirectory` was overloaded (used both as project root and as DCSdpPath resolution base) |
| `TestDirectory` (undocumented) | `WorkspaceDir` | was a fallback for `testPath`; now explicit |

`DCSdpPath` and `DCOutputDir` can remain; they resolve relative to `WorkspaceDir`
instead of `WorkingDirectory`.

---

## 3. Config.cs — Search Order Change

### Problem
Currently loads from `exeDir/config.ini` (line ~24 of `Application.cs`).

### New Search Order

```
1. projectDir/config.ini       (projectDir = 4 levels above exeDir: exeDir → bin → Debug → SDPCLI → projectDir)
2. cwd/config.ini              (backward compat when running from repo root)
3. exeDir/config.ini           (last resort: legacy build-copy location)
```

### ProjectDir Discovery from ExeDir

```
exeDir = SDPCLI/bin/Debug/net472/
         ↑ (1) bin
         ↑ (2) Debug
         ↑ (3) SDPCLI      ← CLI project root
         ↑ (4) snapdragon  ← projectDir / repo root
```

Implementation note: `Path.GetFullPath(Path.Combine(exeDir, "../../../../"))` is
fragile when build output depth changes. A safer option is to walk up from exeDir
looking for a sentinel file (e.g., `config.ini` itself, or `.git/`). Recommended:
walk up max 5 levels looking for `config.ini` first; fall through to exeDir if
not found.

### Config.cs API Extension

Add a static factory:

```csharp
public static Config LoadDefault()
```

This replaces the ad-hoc path construction in `Application.cs`. It applies the
three-step search order and logs which file was found.

---

## 4. Main.cs — PrepareOutputDirectory Change

### Problem
Returns `SDPCLI/test/` by computing `projectRoot` (which is actually SDPCLI/).

### Change
`PrepareOutputDirectory()` should read `WorkspaceDir` from config and return it.

```
Old: 3 levels up from exeDir → SDPCLI/ → "test" subdirectory
New: read config.WorkspaceDir, default to projectDir/workspace/
```

Since config must be loaded before `PrepareOutputDirectory()` is called, the
load order in `Main.cs` must be adjusted:

```
1. Load Config (using new search order)
2. PrepareOutputDirectory (using WorkspaceDir from config)
3. SetupEnvironment (using exeDir for DLL paths, but NOT changing cwd to exeDir*)
4. Create Application, pass workspaceDir
```

*See section 6 for the cwd issue.

---

## 5. SDK Session Directory — workspaceDir Alignment

### Problem
`Main.cs::SetupEnvironment()` calls `Directory.SetCurrentDirectory(executableDir)`.
Because the Snapdragon SDK writes session data relative to cwd, this causes SDK
sessions to land in `exeDir/`.

### Change
After `workspaceDir` is established, set cwd to `workspaceDir`:

```csharp
Directory.SetCurrentDirectory(workspaceDir);
```

This makes the SDK write `<timestamp>/` session dirs directly under `workspaceDir`,
so capture sessions and analysis sessions share the same root.

**Risk**: The SDK setup verification logic in `SetupEnvironment()` also uses
`executableDir` to find `service/android/`. That lookup must remain based on
`executableDir`, not cwd. Separate the two concerns:
- DLL/plugin path setup → `executableDir`
- cwd for SDK session writes → `workspaceDir`

---

## 6. SnapshotCaptureMode — Session Output Alignment

### Problem
`_testPath` is used as a fallback when `sessionPath` is null.
`sessionPath` itself is already a timestamped dir under whatever cwd was set to.

### Change
After section 5 is applied (cwd = workspaceDir), the SDK-generated `sessionPath`
will already land under `workspaceDir/<timestamp>/`. No source change needed in
`SnapshotCaptureMode` itself — the cwd fix is sufficient.

However, `_testPath` (now `workspaceDir`) should be renamed in the constructor
parameter for clarity.

---

## 7. AnalysisPipeline — Sub-Dir Names from Config

### Problem
Sub-directory names are hardcoded strings in `AnalysisPipeline.cs`:

```csharp
string shaderBaseDir  = Path.Combine(sessionDir, "shaders");
string textureBaseDir = Path.Combine(sessionDir, "textures");
string meshBaseDir    = Path.Combine(sessionDir, "meshes");
string captureOutDir  = Path.Combine(sessionDir, $"snapshot_{captureId}");
```

### Change
Introduce a `SessionLayout` value object (or static helper) that reads from
config and provides named properties:

```csharp
// SessionLayout resolves sub-dir names from config, then combines with sessionDir
public class SessionLayout
{
    public string Root         { get; }   // absolute sessionDir
    public string SnapshotDir  { get; }   // Root/snapshot
    public string DbDir        { get; }   // Root/db
    public string ShadersDir   { get; }   // Root/shaders
    public string TexturesDir  { get; }   // Root/textures
    public string MeshesDir    { get; }   // Root/meshes
    public string GfxRzDir     { get; }   // Root/db  (same as DbDir by default)
    public string CaptureDir(uint captureId) => Path.Combine(Root, $"snapshot_{captureId}");
}
```

`SessionLayout` is constructed from `(Config config, string sessionDirRoot)`.
It reads `Session.ShadersDir`, `Session.TexturesDir`, `Session.MeshesDir`,
`Session.SnapshotDir`, `Session.DbDir`, `Session.GfxRzDir` from config with
defaults matching current hardcoded values (backward compatible).

Both CLI and future UI feed from the same config keys; the layout contract is
shared.

---

## 8. SdpFileService — WorkspaceDir as Scan Root

### Problem
`AnalysisMode` passes `testDir = config.Get("TestDirectory", outputPath)` to
`SdpFileService.ScanSdpFiles()`. This only covers one directory.

### Change
`SdpFileService` should offer a method that scans `workspaceDir` recursively:

```csharp
public List<SdpFileInfo> ScanWorkspace(string workspaceDir)
```

`AnalysisMode` and any future `sdpui` caller can call this method. The scan
traverses `workspaceDir/**/*.sdp`, returning all SDP files sorted by modification
time descending.

`TestDirectory` config key is removed; replaced by `WorkspaceDir`.

---

## 9. SessionSummaryService — JSON with Relative Paths

### Problem
SessionSummaryService builds a JSON manifest with absolute paths for some entries
(`optDir` returns absolute). gfxrz files reference `sessionPath` root (which is
currently the SDK-generated dir at exeDir), while analysis assets reference the
analysis session dir.

### Change
After the cwd fix (section 5), the SDK session path and the analysis session path
converge under `workspaceDir/<session-name>/`. The JSON manifest should:

1. Record `sessionDir` (absolute) once in the manifest root
2. All asset references inside the manifest use paths **relative to `sessionDir`**
3. `gfxrz` goes under `Session.DbDir` subfolder (i.e., `db/sdpframe_NNN.gfxrz`)

This makes the manifest portable; moving a session folder only requires updating
the absolute `sessionDir` field, not every individual path.

---

## 10. Implementation Touch Points Summary

| # | File | Change |
|---|------|--------|
| T1 | `projectDir/config.ini` (NEW location) | Move file; add new keys |
| T2 | `SDPCLI/source/Config.cs` | Add `LoadDefault()` with 3-step search; or update `Application.cs` |
| T3 | `SDPCLI/source/Application.cs` | Load config via new search order; `testPath` → `workspaceDir` |
| T4 | `SDPCLI/source/Main.cs` | `PrepareOutputDirectory()` → read `WorkspaceDir` from config; `SetCurrentDirectory` → `workspaceDir` |
| T5 | `SDPCLI/source/Modes/SnapshotCaptureMode.cs` | Rename `_testPath` → `_workspaceDir`; no logic change needed if cwd is fixed |
| T6 | `SDPCLI/source/Modes/AnalysisMode.cs` | `testDir` → use `WorkspaceDir` config key via `SdpFileService.ScanWorkspace()` |
| T7 | `SDPCLI/source/Analysis/AnalysisPipeline.cs` | Replace hardcoded sub-dir names with `SessionLayout` |
| T8 | `SDPCLI/source/Services/Analysis/SdpFileService.cs` | Add `ScanWorkspace(workspaceDir)` method |
| T9 | `SDPCLI/source/Services/Capture/SessionSummaryService.cs` | Rewrite path building to use `SessionLayout`; emit relative paths in JSON |
| T10 | Build scripts / `.csproj` | Remove legacy copy of config.ini to exeDir |

---

## 11. Sequencing

Recommended execution order to avoid breaking the current pipeline:

```
Phase 1 (Config location):
  T2 → T3 → T10 → T1
  Config.cs learns to search projectDir; Application.cs uses it;
  build stops copying config to exeDir; config.ini moves.

Phase 2 (WorkspaceDir):
  T4 (PrepareOutputDirectory only — not cwd change yet)
  → verify capture still works with new testPath
  → then T4 (cwd change) + T5

Phase 3 (SessionLayout + explicit sub-dirs):
  T7 → T9
  AnalysisPipeline and SessionSummaryService use SessionLayout.

Phase 4 (SDP discovery):
  T6 + T8
  AnalysisMode scans workspaceDir; SdpFileService exposes ScanWorkspace.
```

---

## 12. Validation Plan

| Check | Method |
|-------|--------|
| Config load from projectDir | Run CLI with config.ini only at projectDir; confirm "Loaded configuration from: D:\snapdragon\config.ini" |
| Workspace capture output | Run capture mode; confirm .sdp and session dir appear in `workspace/` not `SDPCLI/test/` |
| SDK session dir | Confirm `<timestamp>/` dir appears under `workspaceDir/` not `exeDir/` |
| Analysis pipeline | Run analysis on an SDP from workspaceDir; confirm shaders/textures/meshes land under `workspaceDir/<session>/` |
| Sub-dir config override | Change `Session.ShadersDir=shaders2` in config.ini; confirm pipeline uses that name |
| SDP discovery | Run analysis interactively; confirm files from all session sub-folders are listed |
| JSON relative paths | Open session.json; confirm all asset paths do not start with drive letter |
| Backward compat | Existing SDP files in SDPCLI/test/ still analyzable if user passes full path via -sdp |

---

## 13. Open Questions

1. **SDK session root control**: Can `SDPClient.SessionManager` be configured to
   write to a specific directory, or does it always use cwd? If cwd change is
   insufficient, a session move/copy step may be needed after capture completes.

2. **Build system**: Does the `.csproj` have an explicit `<Content Include="config.ini">
   CopyToOutputDirectory` entry? This must be removed (or made conditional) after
   Phase 1 lands.

3. **`WorkingDirectory` key**: Currently used in `Application.cs` for resolving
   `DCSdpPath` and `DCOutputDir`. After rename to `ProjectDir`, those keys
   resolve against `WorkspaceDir` instead. Confirm this is acceptable.

4. **`llm_cache.json` location**: Currently defaults to `WorkingDirectory/llm_cache.json`.
   After redesign, it should default to `workspaceDir/llm_cache.json` (data, not
   config) unless the user prefers `projectDir`.
