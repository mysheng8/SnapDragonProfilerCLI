---
type: finding
topic: directory layout current state — projectDir / workspaceDir / sessionDir / config
status: investigated
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
summary: >
  Current path layout is implicit and fragmented. config.ini lives in SDPCLI/
  but is loaded from exeDir. testPath (SDPCLI/test/) mixes capture and analysis
  output. sessionDir is unnamed and created by the SDK at exeDir. Sub-directory
  names (shaders/, textures/, meshes/) are hardcoded strings, not config-driven.
  SDP file discovery is scoped to TestDirectory config key. JSON paths are
  relative to sessionDir but that root is not made explicit or portable.
last_updated: 2026-04-09
---

# FINDING-2026-04-09: Directory Layout Current State

## Overview

There is no single, coherent path model across the codebase. The effective
paths are computed in several places with different starting assumptions.

---

## 1. Actual Path Hierarchy (Current)

```
d:\snapdragon\                          ← repo root (informally "projectDir")
├── SDPCLI\
│   ├── config.ini                      ← config file (problem: should be at repo root)
│   ├── test\                           ← capture + analysis output (= testPath)
│   │   ├── *.sdp                       ← captured SDP archives (no sub-folder)
│   │   └── <sdp-basename>\             ← analysis pipeline output (AnalysisPipeline)
│   │       ├── shaders\                ← hardcoded "shaders"
│   │       ├── textures\               ← hardcoded "textures"
│   │       ├── meshes\                 ← hardcoded "meshes"
│   │       └── snapshot_{captureId}\  ← hardcoded "snapshot_{captureId}"
│   ├── source\
│   └── bin\
│       └── Debug\net472\              ← exeDir (AppDomain.CurrentDomain.BaseDirectory)
│           ├── config.ini             ← COPY placed here by build; this is what loads
│           ├── .log\                  ← consolelog (Main.cs: exeDir/.log/)
│           └── <yyyy-MM-ddTHH-mm-ss>\ ← SDK-generated session dir (SDK writes here)
│               ├── sdp.db
│               ├── sdpframe_NNN.gfxrz
│               └── sdpframestripped_NNN.gfxrz
└── dll\
```

---

## 2. How Each Key Path Is Computed

### config.ini Load Path
```csharp
// Application.cs line ~24
string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
```
- Resolves to `exeDir/config.ini` (i.e., the copy placed by the build system)
- `SDPCLI/config.ini` is the source; it must be copied to `exeDir` to take effect

### testPath (workspace placeholder)
```csharp
// Main.cs: PrepareOutputDirectory()
string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;  // = exeDir
string projectRoot  = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", ".."));
// assemblyPath = bin/Debug/net472 → 3 levels up → SDPCLI/
string testPath = Path.Combine(projectRoot, "test");  // = SDPCLI/test/
```
- This is `SDPCLI/test/`, NOT `projectDir/test/`
- `projectRoot` here means SDPCLI project root, not repo root

### SDK Session Directory
```csharp
// Main.cs: SetupEnvironment()
Directory.SetCurrentDirectory(executableDir);  // SDK writes sessions relative to cwd
// SnapshotCaptureMode.cs:
string? sessionPath = _sdpClient?.SessionManager?.GetSessionPath();
// Returns exeDir/<yyyy-MM-ddTHH-mm-ss>/ when cwd=exeDir
```
- SDK creates `<timestamp>/` under `cwd` = `exeDir`
- Fallback when sessionPath is null: `_testPath` = `SDPCLI/test/`
- The two paths are completely separate and inconsistent

### SDP File for Analysis
```csharp
// AnalysisMode.cs: SelectSdpFileInteractively()
string testDir = config.Get("TestDirectory", outputPath);  // fallback to testPath
fileService.ScanSdpFiles(testDir);
```
- `SdpFileService` scans a single directory (non-recursive in spirit, but uses
  `SearchOption.AllDirectories` in practice)
- `TestDirectory` config key is **not documented in config.ini**; it falls back
  silently to `testPath` (= `SDPCLI/test/`)

### sessionDir in Analysis Pipeline
```csharp
// AnalysisPipeline.cs
string sdpName    = Path.GetFileNameWithoutExtension(sdpPath);
string sessionDir = Path.Combine(outputDir, sdpName);
string captureOutDir  = Path.Combine(sessionDir, $"snapshot_{captureId}");
string shaderBaseDir  = Path.Combine(sessionDir, "shaders");
string textureBaseDir = Path.Combine(sessionDir, "textures");
string meshBaseDir    = Path.Combine(sessionDir, "meshes");
```
- Sub-directory names ("shaders", "textures", "meshes", "snapshot_{N}") are
  **hardcoded strings** — not config-driven
- `sessionDir` is not announced to any service as an explicit concept

### SessionSummaryService JSON Paths
```csharp
// gfxrz files: expected at sessionPath root (SDK session dir)
gfxrz = opt(Path.Combine(sessionPath, $"sdpframe_{padded}.gfxrz"))
// asset dirs referenced relative to captureSubDir / sessionDir
shaders  = optDir(Path.Combine(sub, "shaders")),
textures = optDir(Path.Combine(sub, "textures")),
meshes   = optDir(Path.Combine(sub, "meshes")),
```
- `gfxrz` files live at SDK session root; analysis assets live under analysisDir
- These two roots are currently separate on-disk locations
- JSON contains absolute paths (optDir returns absolute), not relative paths

---

## 3. Problems Summary

| # | Problem | Location | Impact |
|---|---------|----------|--------|
| P1 | `config.ini` must be copied to exeDir to take effect | Main.cs, Application.cs | fragile; source and live copy can diverge |
| P2 | `testPath` = `SDPCLI/test/`, not `projectDir/workspace/` | Main.cs::PrepareOutputDirectory | captured files clutter the source project |
| P3 | SDK writes sessions to `exeDir/<timestamp>/` via cwd | Main.cs::SetupEnvironment | mixes build artifacts with captured data |
| P4 | `sessionDir` concept exists in AnalysisPipeline but is not a named config entity | AnalysisPipeline.cs | UI layer cannot discover it independently |
| P5 | Sub-dir names ("shaders", "textures", "meshes") are hardcoded | AnalysisPipeline.cs | UI must duplicate this knowledge; can't be customized |
| P6 | `gfxrz` and DB files live at SDK session root; analysis assets live under workspaceDir | SessionSummaryService | two separate roots; JSON paths are absolute, not portable |
| P7 | `SdpFileService` scans only one flat directory; `TestDirectory` key is undocumented | SdpFileService.cs, AnalysisMode.cs | SDP discovery fragile; can miss files in sub-dirs |
| P8 | `WorkingDirectory` in config.ini does not feed into `PrepareOutputDirectory` | Main.cs | the key exists but doesn't control the output path |
| P9 | No explicit `projectDir` concept in code; only `WorkingDirectory` (used mostly for DCSdpPath resolution) | Application.cs | navigation from config key to actual code path unclear |
| P10 | No sdpui/ project yet; sub-dir names and session contracts are CLI-internal | — | blocked unless contracts are made explicit |

---

## 4. Capture vs Analysis Path Divergence

```
After a capture:
  SDK session:   exeDir/2026-04-09T10-00-00/  (sdp.db, gfxrz files)
  .sdp archive:  SDPCLI/test/2026-04-09T10-00-00.sdp  (or customSdpOutputPath)

When user runs analysis on that .sdp:
  SdpFileService scans: SDPCLI/test/
  Analysis reads:       SDPCLI/test/2026-04-09T10-00-00.sdp (extracts to temp dir)
  Analysis writes:      SDPCLI/test/2026-04-09T10-00-00/  (shaders/, textures/, etc.)
  gfxrz files:         NOT moved; they stay at exeDir/<timestamp>/
```

After the capture, the gfxrz files and the analysis assets are in different
trees. The session JSON references both.

---

## 5. What the Future UI Layer Needs

For `sdpui/` to function, it needs:

1. A well-known `workspaceDir` to discover sessions
2. Each session to be self-contained under `sessionDir`
3. Sub-directory names to come from a config contract, not from CLI source
4. An explicit JSON manifest at `sessionDir/session.json` with relative paths
5. A way to enumerate sessions without consulting CLI source code
