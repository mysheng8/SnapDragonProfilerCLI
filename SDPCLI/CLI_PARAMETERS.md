# SDPCLI Command Line Parameters

## Overview

SDPCLI supports three run modes:

| Mode | How to invoke |
|------|---------------|
| Interactive | No subcommand (`SDPCLI.exe`) |
| Analysis | `SDPCLI.exe analysis <sdp>` |
| Snapshot | `SDPCLI.exe snapshot <pkg\activity> -l|-c` |

---

## Subcommands

### `analysis` — Analyze an existing .sdp file

```
SDPCLI.exe analysis <sdp_path>
    [-s/-snapshot <N|1>]
    [-t/-target <target>]
    [-o/-output <dir>]
    [--no-extract]
    [--debug]
```

`<sdp_path>` is a positional argument (no flag keyword).

**Path resolution** (in order):
1. Absolute path → used as-is
2. Relative → resolved against `SdpDir` (default: `<ProjectDir>/sdp`)
3. Relative → resolved against `ProjectDir` (default: `<WorkingDirectory>/project`)

---

### `snapshot` — Capture a Vulkan snapshot from a connected device

```
SDPCLI.exe snapshot [<package\activity>]
    {-l/-launch | -c/-capture}
    [-o/-output <dir>]
    [--debug]
```

`<package\activity>` overrides `PackageActivity` in config.ini.

---

### Interactive mode (default)

```
SDPCLI.exe
```

Presents a menu for both snapshot and analysis operations.

---

## Flags

### `-s` / `-snapshot <N>`
Snapshot selector for analysis mode.
- `1` or omitted → analyze **all** snapshots in the file
- `N` (N ≥ 2) → analyze only `snapshot_N`

### `-t` / `-target <target>`
Analysis target for incremental execution.  
Processes only the selected step plus its cascaded prerequisites.

| Value | Output | Auto-includes |
|-------|--------|---------------|
| `dc` | `dc.json` | — |
| `shaders` | `shaders.json` + shader extraction | `dc` |
| `textures` | `textures.json` + texture extraction | `dc` |
| `buffers` | `buffers.json` + mesh extraction | `dc` |
| `label` | `label.json` | `dc`, `shaders` |
| `metrics` | `metrics.json` | `dc` |
| `status` | `status.json` | `dc`, `label`, `metrics` |
| `topdc` | `topdc.json` | `status` |
| `analysis` | `snapshot_N_analysis.md` (LLM) | `topdc` |
| `dashboard` | `snapshot_N_dashboard.md` | `topdc`, `status` |

Default: `all` (full pipeline).

### `-o` / `-output <dir>`
Output directory override.
- **Analysis**: Overrides `AnalysisDir/<sdp_basename>/`. Relative paths resolved against `AnalysisDir` then `ProjectDir`.
- **Snapshot**: Overrides `SdpDir`. Relative paths resolved against `SdpDir` then `ProjectDir`.

### `--no-extract`
Skip physical asset extraction (shaders/textures/meshes).  
Only re-generates JSON output files from existing DB data.  
Useful when re-labeling or re-running metrics without re-extracting files.

### `-l` / `-launch`
(Snapshot mode) Launch the target app but do **not** capture. Use to warm up the app before triggering capture separately.

### `-c` / `-capture`
(Snapshot mode) Launch the target app and immediately capture a single frame.

### `--debug`
Enable debug-level logging to `.log/consolelog.txt`.

---

## Legacy Flags (Deprecated)

These flags are accepted for backward compatibility but will print a deprecation warning.

| Old flag | Replacement |
|----------|-------------|
| `-mode analysis` | `analysis` (positional subcommand) |
| `-mode capture` | `snapshot` (positional subcommand) |
| `-sdp <path>` | `analysis <path>` (positional arg) |
| `-stats-only` | `-t status` |
| `-analysis-only` | `-t analysis` |
| `-pass-mode stats` | `-t status` |
| `-pass-mode analysis` | `-t analysis` |

---

## Config Keys (config.ini)

| Key | Default | Description |
|-----|---------|-------------|
| `ProjectDir` | `<WorkingDirectory>/project` | Root for relative path resolution |
| `SdpDir` | `sdp` (rel ProjectDir) | Where .sdp files are read/written |
| `AnalysisDir` | `analysis` (rel ProjectDir) | Where analysis output is written |
| `ShaderDir` | `shaders` | Shader extraction subdir (rel session) |
| `TextureDir` | `textures` | Texture extraction subdir (rel session) |
| `MeshDir` | `meshes` | Mesh extraction subdir (rel session) |
| `DataDir` | `data` | JSON/MD products subdir (rel session) |

---

## Examples

### Analyze all snapshots (auto-resolves via SdpDir)
```batch
SDPCLI.exe analysis 2026-04-07T18-57-50.sdp
```

### Analyze snapshot 3 only
```batch
SDPCLI.exe analysis 2026-04-07T18-57-50.sdp -s 3
```

### Incremental: re-run labeling only (after updating rules)
```batch
SDPCLI.exe analysis 2026-04-07T18-57-50.sdp -s 3 -t label
```

### Re-generate dashboard without re-extracting assets
```batch
SDPCLI.exe analysis 2026-04-07T18-57-50.sdp -s 3 -t dashboard --no-extract
```

### Analysis with absolute path
```batch
SDPCLI.exe analysis D:\captures\game.sdp -s 3 -t metrics
```

### Snapshot capture (launch + capture)
```batch
SDPCLI.exe snapshot com.example.game/com.example.game.MainActivity -c
```

### Snapshot with custom output directory
```batch
SDPCLI.exe snapshot com.example.game/.MainActivity -c -o D:\sdp_output
```

### Batch analysis
```batch
@echo off
for %%f in (project\sdp\*.sdp) do (
    echo Analyzing %%f...
    SDPCLI.exe analysis "%%f"
)
```

---

## Output Structure

```
<AnalysisDir>/<sdp_basename>/
  sdp.db, *.gfxrz            ← extracted SDP contents
  shaders/                   ← pipeline_N_stage.spv  (shared, per-session)
  textures/                  ← texture_N.png          (shared, per-session)
  meshes/                    ← mesh_N.obj             (shared, per-session)
  snapshot_N/
    dc.json                  ← core DC params + render targets
    shaders.json             ← shader stages + file refs
    textures.json            ← texture metadata + file refs
    buffers.json             ← vertex/index buffers + mesh refs
    label.json               ← classification results
    metrics.json             ← GPU performance counters
    status.json              ← percentile statistics
    topdc.json               ← top draw calls by attribution
    snapshot_N_analysis.md   ← LLM attribution report
    snapshot_N_dashboard.md  ← rule-based charts + tables
    snapshot_N_index.json    ← manifest of all product files
```

---

## Build and Run

```batch
dotnet build SDPCLI.sln -c Debug
cd SDPCLI\bin\Debug\net472
SDPCLI.exe analysis my_capture.sdp -s 3
```

Or use the helper script from the repo root:
```batch
SDPCLI.bat analysis my_capture.sdp -s 3
```

**Log files**: `SDPCLI\bin\Debug\net472\.log\consolelog.txt`
