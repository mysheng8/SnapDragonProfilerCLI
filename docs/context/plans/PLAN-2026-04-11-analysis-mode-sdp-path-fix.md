---
type: plan
topic: fix analysis mode .sdp path resolution
status: proposed
based_on:
  - FINDING-2026-04-11-analysis-mode-sdp-path-bug.md
related_paths:
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/SdpFileService.cs
related_tags:
  - analysis
  - sdp
  - path-resolution
  - bug-fix
summary: |
  AnalysisMode passes .sdp path directly as dbPath to pipeline. Fix: call
  fileService.FindDatabasePath(selectedFile) before the per-capture loop, pass
  resolved dbPath to pipeline.RunAnalysis. Single-line insertion.
last_updated: 2026-04-11
---

# PLAN: Fix Analysis Mode .sdp → sdp.db Path Resolution

## Problem

`AnalysisMode.Run()` passes the selected `.sdp` file path directly as `dbPath` to
`AnalysisPipeline.RunAnalysis`. The pipeline tries to open it as SQLite and throws
"file is not a database". All capture output directories are created but remain empty.

**Root cause confirmed by:** [FINDING-2026-04-11-analysis-mode-sdp-path-bug.md]

## Fix Scope

**Single file**: `SDPCLI/source/Modes/AnalysisMode.cs`  
**Type**: One new method call + null-guard before the per-capture loop  
**Risk**: Low — `FindDatabasePath` already exists and handles the adjacent-dir case correctly

---

## Change: AnalysisMode.Run()

### Location

Both call sites in `Run()` pass `selectedFile` (the .sdp path) as `dbPath`:

```csharp
// line 122 (non-interactive path)
pipeline.RunAnalysis(selectedFile, resolvedOutput, captureId, cmdBufferFilter, analysisTarget);

// line 162 (interactive loop)
pipeline.RunAnalysis(selectedFile, resolvedOutput, captureId, cmdBufferFilter, analysisTarget);
```

### Required Fix

After resolving `selectedFile` and before the per-capture loop, add:

```csharp
// Resolve .sdp path → actual sdp.db path
string? dbPath = fileService.FindDatabasePath(selectedFile);
if (dbPath == null)
{
    logger.Error("Could not locate sdp.db. Ensure the .sdp archive or adjacent session directory is intact.");
    return;
}
```

Then replace both call sites:

```csharp
pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, cmdBufferFilter, analysisTarget);
```

### Placement in Run()

```csharp
var captureIds = ScanCaptureIds(selectedFile);
if (captureIds.Count == 0) { ... return; }

string resolvedOutput = ResolveOutputDir(selectedFile);
var analysisTarget = AnalysisTargetExtensions.Parse(targetArg);

// ← INSERT HERE: resolve selectedFile → dbPath
string? dbPath = fileService.FindDatabasePath(selectedFile);
if (dbPath == null)
{
    logger.Error("Could not locate sdp.db ...");
    return;
}

// Non-interactive path
if (specifiedSdpPath != null)
{
    foreach (var captureId in toRun)
        pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, ...);
    return;
}

// Interactive loop
while (true)
{
    foreach (var captureId in runList)
        pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, ...);
}
```

---

## Why FindDatabasePath Works for This Case

`SdpFileService.FindDatabasePath` Case 2 (adjacent directory):

```csharp
// Input: project/sdp/2026-04-11T09-50-42.sdp
// Case 2: adjacent dir
string adjacentDir = Path.Combine("project/sdp/", "2026-04-11T09-50-42");
// = project/sdp/2026-04-11T09-50-42/   ← EXISTS

string db = Path.Combine(adjacentDir, "sdp.db");
// = project/sdp/2026-04-11T09-50-42/sdp.db  ← EXISTS, returned
```

For the ZIP-only case (no adjacent dir), Case 3 extracts to temp and returns db path — also works, but gfxrz won't be accessible by path (not needed by current pipeline).

---

## Secondary Cleanup (Optional, Lower Priority)

`SdpFileService.FindDatabasePath` Case 1b is misleading:

```csharp
// Case 1b: checks sameDir = project/sdp/ for sdp.db → never exists in this layout
string sameDir = Path.GetDirectoryName(sdpPath);       // = project/sdp/
string dbInSameDir = Path.Combine(sameDir, "sdp.db");  // = project/sdp/sdp.db
```

This is harmless (falls through to Case 2) but can be removed in a cleanup pass.

---

## Validation

After fix:

1. Run: `sdpcli analysis 2026-04-11T09-50-42.sdp`
2. Expected: pipeline uses `project/sdp/2026-04-11T09-50-42/sdp.db`
3. Expected: `project/analysis/2026-04-11T09-50-42/snapshot_N/*.json` files generated
4. Verify: `dc.json`, `shaders.json`, `textures.json`, `metrics.json`, `status.json` present in at least one snapshot dir

---

## Execution Status

Implementation requires the Executor agent.
