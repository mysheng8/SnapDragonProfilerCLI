---
type: finding
topic: analysis mode .sdp path not resolved to sdp.db before pipeline
status: investigated
related_paths:
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/SdpFileService.cs
  - SDPCLI/source/Data/SdpDatabase.cs
  - SDPCLI/source/Services/Capture/SessionArchiveService.cs
related_tags:
  - analysis
  - sdp
  - path-resolution
  - bug
summary: AnalysisMode passes the .sdp file path directly as dbPath to AnalysisPipeline.RunAnalysis. Pipeline tries to open it as SQLite and throws. Output dirs are created but empty. Compression is NOT the cause.
last_updated: 2026-04-11
---

# FINDING: Analysis Mode .sdp Path Not Resolved → Empty Output Directories

## Symptom

User observed: analysis mode ran, output directories `snapshot_2/` through `snapshot_7/` were created under `project/analysis/2026-04-11T09-50-42/`, but all directories were **completely empty**. User suspected compression issue ("压缩出问题").

**Confirmed state:**

```
project/analysis/2026-04-11T09-50-42/
  snapshot_2/    ← empty
  snapshot_3/    ← empty
  snapshot_4/    ← empty
  snapshot_5/    ← empty
  snapshot_6/    ← empty
  snapshot_7/    ← empty
```

## Root Cause: Missing .sdp → sdp.db Resolution in AnalysisMode

### Code Path

`AnalysisMode.Run()` selects a `.sdp` file path (e.g., `project/sdp/2026-04-11T09-50-42.sdp`) and passes it directly to `pipeline.RunAnalysis`:

```csharp
// AnalysisMode.cs line 122
pipeline.RunAnalysis(selectedFile,   // ← .sdp zip path passed as dbPath
                    resolvedOutput,
                    captureId, ...);
```

`AnalysisPipeline.RunAnalysis(string dbPath, ...)` signature expects actual `sdp.db` path:

```csharp
/// dbPath: absolute path to sdp.db (pre-extracted by SdpFileService.ExtractToAnalysis).
```

Inside pipeline, normal path:

```csharp
if (!System.IO.File.Exists(dbPath)) throw ...;   // passes: .sdp file exists
db = new SdpDatabase(dbPath, captureId);          // DbPath = .sdp zip path
db.ValidateForAnalysis(logger);                   // → OpenConnection()
```

`SdpDatabase.OpenConnection()`:

```csharp
string cs = $"Data Source={DbPath};Version=3;Read Only=True;";
var conn = new SQLiteConnection(cs);
conn.Open();  // ← throws "file is not a database" on ZIP file
```

### Exception Flow

→ exception propagates out of `ValidateForAnalysis`  
→ caught by `AnalysisMode.Run()`: `logger.Error($"Analysis failed for snapshot_{captureId}: ...")`  
→ `captureOutDir` already created by `Directory.CreateDirectory(captureOutDir)` before exception  
→ result: empty directories

### Why Directories Are Created

Pipeline creates `captureOutDir` before any DB access:

```csharp
string captureOutDir = Path.Combine(sessionDir, $"snapshot_{captureId}");
Directory.CreateDirectory(captureOutDir);  // ← runs before any exception
// ...
db.ValidateForAnalysis(logger);           // ← throws here
```

### Why ScanCaptureIds Worked

`AnalysisMode.ScanCaptureIds` uses the adjacent session directory, not the ZIP:

```csharp
// Case 2: adjacent dir project/sdp/2026-04-11T09-50-42/ exists
string adjacent = Path.Combine(parentDir, Path.GetFileNameWithoutExtension(sdpPath));
if (Directory.Exists(adjacent))
    foreach (var dir in Directory.GetDirectories(adjacent, "snapshot_*"))
```

This correctly finds snapshot_2 through snapshot_7, so the loop runs for each — but all fail at DB open.

### Missing Design Piece

Pipeline comment references `SdpFileService.ExtractToAnalysis` — this method does **not exist**. The intended step was:

```
AnalysisMode: selectedFile (.sdp) → ExtractToAnalysis() → dbPath → pipeline.RunAnalysis(dbPath)
```

This extraction step was never implemented. `AnalysisMode` has no call to `FindDatabasePath` or any equivalent before calling the pipeline.

## Compression Status: NOT the Issue

`SessionArchiveService.CreateSessionArchive` is correct:
- Uses `Directory.GetFiles(captureDir, "*", SearchOption.AllDirectories)` — recursive, all files
- Logs a WARNING if `sdp.db` or `version.txt` is missing before zipping
- For session `2026-04-11T09-50-42/`, source directory has full content (sdp.db, 6+6 gfxrz files, all snapshot subdirs)
- The .sdp archive itself is expected to be complete

## Fix Required

`AnalysisMode.Run()` must resolve `.sdp` path to actual `sdp.db` path before calling `pipeline.RunAnalysis`.

`SdpFileService.FindDatabasePath` already handles this correctly via Case 2 (adjacent directory):

```csharp
// For project/sdp/2026-04-11T09-50-42.sdp
// Case 2 resolves to: project/sdp/2026-04-11T09-50-42/sdp.db ← correct
string adjacent = Path.Combine(parentDir, Path.GetFileNameWithoutExtension(sdpPath));
```

### Required Change (single call in AnalysisMode.Run)

```csharp
// Before calling pipeline, resolve .sdp → sdp.db
string? dbPath = fileService.FindDatabasePath(selectedFile);
if (dbPath == null)
{
    logger.Error("Could not locate sdp.db in .sdp archive or adjacent session directory");
    return;
}

// Then per-capture loop:
pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, ...);
```

### SdpFileService.FindDatabasePath Case 1b Issue (minor)

Case 1b checks for `sdp.db` in the **same directory as the .sdp file** (i.e., `project/sdp/sdp.db`), which will never exist. This is harmless (falls through to Case 2) but misleading.

```csharp
// Case 1b: wrong — sdp.db is never in the same dir as the .sdp file in this layout
string sameDir = Path.GetDirectoryName(sdpPath);       // = project/sdp/
string dbInSameDir = Path.Combine(sameDir, "sdp.db");  // = project/sdp/sdp.db ← wrong place
```

Can be removed or corrected in a cleanup pass.

## Evidence

| Artifact | State |
|---|---|
| `project/sdp/2026-04-11T09-50-42/` | Complete: sdp.db + 12 gfxrz + snapshot_2/-7/ |
| `project/sdp/2026-04-11T09-50-42.sdp` | Exists (expected to be complete zip) |
| `project/analysis/2026-04-11T09-50-42/snapshot_2/ … /snapshot_7/` | All empty |
| `SdpDatabase.OpenConnection(dbPath)` on .sdp path | Throws "file is not a database" |
| `SdpFileService.ExtractToAnalysis` | Method does not exist |
