---
type: plan
topic: unified shader/texture folder + drawcall analysis JSON output + parallel extraction + LLM response cache
status: implemented
based_on:
  - FINDING-2026-04-07-shader-texture-export-structure.md
  - FINDING-2026-04-08-parallelism-thread-safety.md
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/source/Modes/DrawCallAnalysisMode.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Tools/LlmApiWrapper.cs
  - SDPCLI/source/Tools/LlmResponseCache.cs
  - SDPCLI/SDPCLI/config.ini
related_tags:
  - shader
  - texture
  - export
  - deduplication
  - json
  - drawcall-analysis
  - parallel
  - async
  - llm-cache
summary: |
  Phase 1: Unified shader/texture export into session-level shared folders with per-file
  existence checks; DrawCallAnalysis CSV replaced by annotated JSON referencing exact asset
  files per draw call. Phase 2: Parallel shader and texture extraction using Task.WhenAll +
  Parallel.ForEach; parallel LLM labeling with ConcurrentDictionary. Phase 3: Disk-persisted
  LLM response ring-pool cache (LlmResponseCache) with SHA-256 key, FIFO eviction, and
  atomic JSON persistence.
last_updated: 2026-04-08
---

# Plan: Unified Shader/Texture Folder + DrawCallAnalysis JSON

## Goal

1. All shaders and textures from **all snapshots** of the same SDP session land in one shared folder.
2. Export uses a **per-file existence check** to skip assets already present (dedup across snapshots).
3. The `DrawCallAnalysis_*.csv` is replaced by `DrawCallAnalysis_*.json` that annotates each DC with the exact shader and texture file paths it uses.

---

## New Directory Layout

```
<outputDir>/
  <sdpName>/                            ← sessionDir (unchanged)
    shaders/                            ← NEW: shared across ALL snapshots
      pipeline_1234_vert.spv
      pipeline_1234_vert.hlsl
      pipeline_1234_frag.spv
      ...
    textures/                           ← NEW: shared across ALL snapshots
      texture_456.png
      texture_789.png
      ...
    snapshot_{captureId}/               ← captureOutDir (unchanged)
      DrawCallAnalysis_{timestamp}.json ← CHANGED: was .csv
      DrawCallAnalysis_Summary_*.md
      report.json
      snapshot.png
      meshes/
```

Relative paths stored inside the JSON use relative notation from `captureOutDir`:
- `../../shaders/pipeline_1234_vert.hlsl`  
  or equivalently `../shaders/pipeline_1234_vert.hlsl` (one level up from snapshot dir to session dir)

---

## Change 1 — AnalysisPipeline.cs Step 1.5: Shared Folder + File-Level Existence Check

### 1a. Move shaderBaseDir and textureBaseDir to session level

```csharp
// BEFORE
string shaderBaseDir  = Path.Combine(captureOutDir, "shaders");
string textureBaseDir = Path.Combine(captureOutDir, "textures");

// AFTER
string shaderBaseDir  = Path.Combine(sessionDir, "shaders");
string textureBaseDir = Path.Combine(sessionDir, "textures");
```

### 1b. Remove the folder-level skip; switch to per-pipeline/per-file checks

BEFORE (all-or-nothing folder check):
```csharp
bool shadersExist  = Directory.Exists(shaderBaseDir);
bool texturesExist = Directory.Exists(textureBaseDir);
if (shadersExist && texturesExist)
    logger.Info("Skipping extraction — shaders/ and textures/ already exist.");
else { /* extract */ }
```

AFTER (always enter extraction, check per-pipeline / per-file):
```csharp
Directory.CreateDirectory(shaderBaseDir);
Directory.CreateDirectory(textureBaseDir);
```

### 1c. Shader loop — write directly to shaderBaseDir, check per-pipeline

```csharp
// Collect unique pipeline IDs across all DCs
var seenPipelines = new HashSet<uint>();
int shaderOkCount = 0, shaderTotal = 0;
foreach (var dc in report.DrawCallResults)
{
    if (dc.PipelineID == 0) continue;
    if (!seenPipelines.Add(dc.PipelineID)) continue;   // deduplicate by pipelineID
    shaderTotal++;

    // Per-file existence check: skip if at least one .spv for this pipeline already exists
    string probeFile = Path.Combine(shaderBaseDir, $"pipeline_{dc.PipelineID}_vert.spv");
    string probeFile2 = Path.Combine(shaderBaseDir, $"pipeline_{dc.PipelineID}_frag.spv");
    if (Directory.GetFiles(shaderBaseDir, $"pipeline_{dc.PipelineID}_*.spv").Length > 0)
    {
        shaderOkCount++;  // already extracted
        continue;
    }

    var shExt = new Tools.ShaderExtractor(dbPath, (int)captureId)
    {
        SpirvCrossPath     = spirvCrossPath,
        ShaderOutputFormat = shaderFmt
    };
    if (shExt.ExtractShadersForPipeline(dc.PipelineID, shaderBaseDir)) shaderOkCount++;
}
```

### 1d. Texture loop — per-file existence check

```csharp
var allTexIds = report.DrawCallResults
    .SelectMany(dc => dc.TextureIDs.Select(id => (ulong)id))
    .Distinct().ToList();

var texExt = new Tools.TextureExtractor(dbPath, (int)captureId);
int texOk = 0;
foreach (var texId in allTexIds)
{
    string texFile = Path.Combine(textureBaseDir, $"texture_{texId}.png");
    if (File.Exists(texFile))  // ← per-file check
    {
        texOk++;
        continue;
    }
    if (texExt.ExtractTexture(texId, texFile)) texOk++;
}
```

### 1e. Pass shaderBaseDir / textureBaseDir downstream

`shaderBaseDir` and `textureBaseDir` must be passed to:
- `ReportGenerationService.GenerateLabeledMetricsJson()` (new method, see Change 2)
- `CaptureReportService.GenerateReport()` (optional, for adding asset refs to report.json)

---

## Change 2 — ReportGenerationService: Replace CSV with Annotated JSON

### 2a. New method signature

```csharp
public string GenerateLabeledMetricsJson(
    DrawCallAnalysisReport report,
    string captureOutDir,
    string shaderBaseDir,   // absolute path to shared shaders folder
    string textureBaseDir)  // absolute path to shared textures folder
```

### 2b. JSON structure (DrawCallAnalysis_*.json)

```json
{
  "generated_at": "2026-04-07T10:00:00Z",
  "drawcalls": [
    {
      "dc_id": "1.1.31",
      "category": "场景",
      "detail": "gbuffer opaque",
      "api_name": "vkCmdDrawIndexed",
      "pipeline_id": 1234,
      "vertex_count": 5832,
      "index_count": 8748,
      "instance_count": 1,
      "shader_files": [
        "../../shaders/pipeline_1234_vert.hlsl",
        "../../shaders/pipeline_1234_frag.hlsl"
      ],
      "texture_ids": [456, 789],
      "texture_files": [
        "../../textures/texture_456.png",
        "../../textures/texture_789.png"
      ],
      "render_targets": {
        "color": ["1001|1002"],
        "depth": ["1003"],
        "width": 1920,
        "height": 1080
      },
      "metrics": {
        "clocks": 120000,
        "read_total_bytes": 204800,
        "write_total_bytes": 65536,
        "fragments_shaded": 350000,
        "vertices_shaded": 5832,
        "shaders_busy_pct": 82.5,
        "tex_l1_miss_pct": 3.2,
        "tex_l2_miss_pct": 1.1,
        "tex_fetch_stall_pct": 0.4,
        "fragment_instructions": 450000,
        "vertex_instructions": 18000,
        "tex_mem_read_bytes": 102400
      }
    }
  ]
}
```

### 2c. Building shader_files list

For each DC, enumerate files in `shaderBaseDir` matching `pipeline_{pipelineId}_*`:

```csharp
string[] shaderFiles = Directory.Exists(shaderBaseDir)
    ? Directory.GetFiles(shaderBaseDir, $"pipeline_{dc.PipelineID}_*")
        .Select(f => "../../shaders/" + Path.GetFileName(f))
        .OrderBy(f => f)
        .ToArray()
    : Array.Empty<string>();
```

### 2d. Building texture_files list

```csharp
string[] textureFiles = dc.TextureIDs
    .Select(id => {
        string fname = $"texture_{id}.png";
        string abs   = Path.Combine(textureBaseDir, fname);
        return File.Exists(abs) ? "../../textures/" + fname : null;
    })
    .Where(p => p != null)
    .ToArray();
```

### 2e. Update LoadLabelsFromAnalysis (was LoadLabelsFromCsv)

Rename and update `LoadLabelsFromCsv` to also read from the new JSON format.
Search for the most recent `DrawCallAnalysis_*.json` first, fall back to `DrawCallAnalysis_*.csv` for backward compatibility:

```csharp
private void LoadLabelsFromAnalysis(DrawCallAnalysisReport report, string sessionDir)
{
    // Try JSON first
    string? jsonPath = Directory.GetFiles(sessionDir, "DrawCallAnalysis_*.json")
        .Where(f => !f.Contains("Summary"))
        .OrderByDescending(File.GetLastWriteTime)
        .FirstOrDefault();

    if (jsonPath != null)
    {
        // Parse dc_id → category + detail from JSON array
        // ...
        return;
    }

    // Fallback: legacy CSV
    string? csvPath = Directory.GetFiles(sessionDir, "DrawCallAnalysis_*.csv")
        .Where(f => !f.Contains("Summary"))
        .OrderByDescending(File.GetLastWriteTime)
        .FirstOrDefault();
    // existing CSV parse logic ...
}
```

---

## Change 3 — AnalysisPipeline.cs Step 3: Call New JSON Method

```csharp
// BEFORE
string labeledCsv = reportService.GenerateLabeledMetricsCsv(report, captureOutDir);
logger.Info($"  → CSV: {labeledCsv}");

// AFTER
string labeledJson = reportService.GenerateLabeledMetricsJson(
    report, captureOutDir, shaderBaseDir, textureBaseDir);
logger.Info($"  → JSON: {labeledJson}");
```

Update `LoadLabelsFromCsv(report, captureOutDir)` → `LoadLabelsFromAnalysis(report, captureOutDir)`.

---

## Change 4 — DrawCallAnalysisMode.cs (Single-DC Mode)

The single-DC mode is used for interactive per-draw-call investigation, not batch analysis.
The user's request is primarily about the batch `AnalysisMode` pipeline.

**Minimal change for single-DC mode:**

- Move `shaderOutput` and `textureOutput` to respect a session-level `--shared-assets` flag (optional future work).
- Per-file existence check before shader/texture extraction (lower priority).
- The Markdown report already links to the extracted files — no format migration required for this mode.

**This mode is lower priority; defer unless the user explicitly requests it.**

---

## Change 5 — AnalysisPipeline.cs Step 1.5: Remove DC Sub-folder for Shaders

After the change in 1c above, the shader extractor writes directly to `shaderBaseDir` (flat).
Delete the existing `dc_` sub-folder logic:

```csharp
// REMOVE this pattern:
string dcDir = Path.Combine(shaderBaseDir, "dc_" + dc.DrawCallNumber);
```

---

## Affected Files Summary

| File | Change type | Description |
|------|------------|-------------|
| `AnalysisPipeline.cs` | Modify | Move shaderBaseDir/textureBaseDir to sessionDir; per-file existence check; remove per-DC sub-folder; pass dirs to report service |
| `ReportGenerationService.cs` | Modify | Add `GenerateLabeledMetricsJson()` with shader_files and texture_files per DC |
| `ReportGenerationService.cs` | Modify | Rename `LoadLabelsFromCsv` → `LoadLabelsFromAnalysis`; support JSON + CSV fallback |
| `DrawCallAnalysisMode.cs` | Optional | Per-file existence check for single-DC extraction (defer) |

---

## Relative Path Strategy

The JSON is written to `snapshot_{captureId}/DrawCallAnalysis_*.json`.
The shared assets live at `<sessionDir>/shaders/` and `<sessionDir>/textures/`.
Relative path from the JSON file to assets = `../../shaders/` (up from `snapshot_N/` to `<sdpName>/`).

This means a single field change to the output path is sufficient — no absolute path leaks in the JSON.

---

## Implementation Sequence (Phase 1)

1. **AnalysisPipeline.cs** — relocate `shaderBaseDir` and `textureBaseDir` first (Step 1.5 refactor).
2. **AnalysisPipeline.cs** — replace folder-level with per-file checks (Step 1.5 logic).
3. **ReportGenerationService.cs** — add `GenerateLabeledMetricsJson()`.
4. **AnalysisPipeline.cs** — wire Step 3 call to new JSON method; update Step 2 label reload.
5. **Test** with a multi-snapshot SDP to confirm cross-snapshot deduplication.

---

## Implementation Status

All phases implemented on 2026-04-08. Phase 1 (shared folders + JSON), Phase 2 (parallelism),
and Phase 3 (LLM cache) are all merged into the codebase.

---

## Phase 2 — Parallel Shader + Texture Extraction and Parallel LLM Labeling

### 2a. Step 1.5: Task.WhenAll(shaderTask, textureTask)

`AnalysisPipeline.cs` Step 1.5 runs shader extraction and texture extraction concurrently:

```csharp
var shaderTask  = Task.Run(() => Parallel.ForEach(uniquePipelines,
    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
    pipelineId => { /* extract shaders */ }));

var textureTask = Task.Run(() => Parallel.ForEach(allTexIds,
    new ParallelOptions { MaxDegreeOfParallelism = _config.TextureExtractionDegree },
    texId => { /* extract texture */ }));

await Task.WhenAll(shaderTask, textureTask);
```

`TextureExtractionDegree` defaults to 4 (conservative; Qonvert DLL is P/Invoke, not fully reentrant).
Shader extraction uses full `ProcessorCount` (independent SQLite connections per `ShaderExtractor` instance).

### 2b. Step 2: Parallel LLM Labeling

```csharp
var categorySummary = new ConcurrentDictionary<string, int>();
int labelCount = 0;

Parallel.ForEach(report.DrawCallResults,
    new ParallelOptions { MaxDegreeOfParallelism = _config.LlmMaxConcurrentRequests },
    dc =>
    {
        var label = _labelService.Label(dc, shaderBaseDir);
        dc.Category = label.Category;
        dc.Detail   = label.Detail;
        categorySummary.AddOrUpdate(label.Category, 1, (_, v) => v + 1);
        Interlocked.Increment(ref labelCount);
    });
```

`DrawCallLabelService._llmCache` changed from `Dictionary<uint, DrawCallLabel>` to
`ConcurrentDictionary<uint, DrawCallLabel>` to support concurrent writes from labeler threads.

### 2c. New config.ini keys

```ini
LlmMaxConcurrentRequests=8
TextureExtractionDegree=4
```

### 2d. Thread-safety baseline

See `FINDING-2026-04-08-parallelism-thread-safety.md` for the full thread-safety audit
that justified these concurrency limits.

---

## Phase 3 — LLM Response Ring-Pool Cache

### 3a. New file: LlmResponseCache.cs

Location: `SDPCLI/source/Tools/LlmResponseCache.cs`

Design:
- Fixed-capacity ring pool (`CacheEntry?[] _ring`, size = `LlmCacheSize`, default 512)
- Key = SHA-256(prompt)[0..16 bytes] as 32-char lowercase hex
- `Dictionary<string, int> _index` maps key → ring slot (O(1) lookup)
- FIFO eviction: `int _writeHead` advances mod capacity; old entry evicted when slot overwritten
- Thread safety: `ReaderWriterLockSlim` (read lock for `TryGet`, write lock for `Put`)
- Persistence: full ring serialized to `<LlmCachePath>/llm_response_cache.json` via atomic write
  (`.tmp` → `File.Move(overwrite:true)`)
- `Load()` on construction restores ring and `_writeHead` from disk
- `IDisposable` → disposes `ReaderWriterLockSlim`

### 3b. LlmApiWrapper.cs integration

`LlmApiWrapper` now implements `IDisposable` and integrates `LlmResponseCache` as L2:

```csharp
// In Chat(prompt):
if (_cache != null && _cache.TryGet(prompt, out string? cached))
{
    logger.Debug("LLM cache hit");
    return cached;
}
string response = CallApi(prompt);
_cache?.Put(prompt, response);
return response;
```

`DrawCallLabelService._llmCache` (L1, keyed by `PipelineID`) sits above this —
if L1 hits, `LlmApiWrapper.Chat` is never called and L2 is not consulted.

### 3c. New config.ini keys

```ini
[LLMCache]
LlmCacheEnabled=true
LlmCacheSize=512
# LlmCachePath=<absolute or relative path; defaults to session output dir>
```

### 3d. LoadShaderCode path fix

`DrawCallLabelService.LoadShaderCode` updated to use the flat directory:

```csharp
// BEFORE: looked in dc_{drawCallNumber}/ subfolder
// AFTER:  globs pipeline_{pipelineId}_*.hlsl in shaderBaseDir directly
var files = Directory.GetFiles(shaderBaseDir, $"pipeline_{pipelineId}_*.hlsl")
         .Concat(Directory.GetFiles(shaderBaseDir, $"pipeline_{pipelineId}_*.glsl"));
```

Call site: `LoadShaderCode(shaderBaseDir, dc.DrawCallNumber)` → `LoadShaderCode(shaderBaseDir, dc.PipelineID)`.
