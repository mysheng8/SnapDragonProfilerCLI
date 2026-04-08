---
type: finding
topic: shader and texture export structure / drawcall analysis output format
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/source/Modes/DrawCallAnalysisMode.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Models/DrawCallModels.cs
related_tags:
  - shader
  - texture
  - export
  - csv
  - json
  - drawcall-analysis
summary: |
  Investigation of current shader/texture export layout, per-capture folder isolation,
  CSV generation logic, and the absence of resource-file references in the output.
last_updated: 2026-04-07
---

# Finding: Shader and Texture Export Structure

## 1. Current Directory Layout (AnalysisMode)

`AnalysisPipeline.RunAnalysis()` builds the following structure per capture:

```
<outputDir>/
  <sdpName>/                            ← sessionDir
    snapshot_{captureId}/               ← captureOutDir
      shaders/
        dc_{DrawCallNumber}/            ← one sub-folder per DC
          pipeline_{pipelineId}_vert.spv
          pipeline_{pipelineId}_vert.hlsl
          pipeline_{pipelineId}_frag.spv
          pipeline_{pipelineId}_frag.hlsl
      textures/
        texture_{texId}.png             ← flat list, deduped within the capture
      DrawCallAnalysis_{timestamp}.csv
      DrawCallAnalysis_Summary_*.md
      report.json
      snapshot.png
```

**Key observations:**

- `shaderBaseDir` = `captureOutDir + "/shaders"`
- `textureBaseDir` = `captureOutDir + "/textures"`
- Both are **per-capture** folders; running two snapshots of the same SDP creates two independent copies of the same resources.
- **Shader existence check is folder-level** (lines ~100-103 in AnalysisPipeline.cs):
  ```csharp
  bool shadersExist  = Directory.Exists(shaderBaseDir);
  bool texturesExist = Directory.Exists(textureBaseDir);
  if (shadersExist && texturesExist) { /* skip all extraction */ }
  ```
- **Texture deduplication is per-capture** using `.Distinct()` on texture IDs before looping, but there is no cross-snapshot dedup.
- Each DC's shaders land in `shaders/dc_{DrawCallNumber}/` — different DCs that share the same pipeline still export the same shader files multiple times into different sub-folders.

## 2. Single-DC Mode (DrawCallAnalysisMode)

`DrawCallAnalysisMode.Run()` writes shaders and textures into:

```
<outputRoot>/
  shaders/    ← all stages for the single pipeline
  textures/   ← all textures bound to this draw call
  dc{N}_report.md
  snapshot.png
```

No cross-call deduplication logic; `outputRoot` is specific to the requested DC.

## 3. CSV Generation (ReportGenerationService.GenerateLabeledMetricsCsv)

Emits `DrawCallAnalysis_{timestamp}.csv` with columns:

```
DrawCall, Category, Detail, ApiName, PipelineID, ShaderCount, TextureCount,
ColorRT, ColorRTFormat, DepthRT, DepthRTFormat, RTWidth, RTHeight,
VBCount, HasIB, TypedBufViews, SmallBufs, PerInstanceBuf32,
VertexCount, IndexCount, InstanceCount,
Clocks, ReadTotal(Bytes), WriteTotal(Bytes), FragmentsShaded, VerticesShaded,
%ShadersBusy, %TexL1Miss, %TexL2Miss, %TexFetchStall,
FragmentInstructions, VertexInstructions, TexMemRead(Bytes)
```

**Missing:** no column for the actual extracted shader file paths or texture file paths.

The CSV path is used by `LoadLabelsFromCsv()` (called when `AnalysisOnlyGenerateReport=true`) to reload DC labels between runs. This dependency must be preserved when migrating to JSON.

## 4. report.json (CaptureReportService.GenerateReport)

`report.json` is a compact high-level digest with frame stats, category budget, top-N DCs, and bottleneck tags. It does **not** include per-DC resource file references. This is a separate file from the per-DC analysis output; it focuses on performance data, not on asset references.

## 5. ShaderExtractor Output File Names

`ShaderExtractor.ExtractShadersForPipeline()` produces files named:

```
pipeline_{pipelineId}_{stageName}.spv
pipeline_{pipelineId}_{stageName}.hlsl   (if spirv-cross available)
pipeline_{pipelineId}_{stageName}.glsl   (if format=glsl/both)
pipeline_{pipelineId}_{stageName}.disasm (optional)
```

The stage names are: `vert`, `frag`, `comp`, `tesc`, `tese`, `geom`, `rgen`, `rint`, `rahit`, `rchit`, `rmiss`.

Because the file names are pipeline-ID-based, multiple DCs that share the same `PipelineID` would produce identical file names — files are naturally deduplicated by content if placed in a flat shared directory.

## 6. TextureExtractor Output File Names

`TextureExtractor.ExtractTexture(resourceId, outputPath)` accepts an explicit output path. Current callers construct:

```csharp
$"texture_{texId}.png"   // in DrawCallAnalysisMode and AnalysisPipeline
```

The method itself appends `.png` if the path does not already end with it.

## 7. Key Problems to Solve

| # | Problem | Location |
|---|---------|----------|
| 1 | Shaders and textures duplicated across `snapshot_N/` for each capture | AnalysisPipeline.cs Step 1.5 |
| 2 | Shader existence check is folder-level, not file-level | AnalysisPipeline.cs ~L100 |
| 3 | Shader sub-folder per DC causes redundant extraction of same pipeline | AnalysisPipeline.cs ~L115 |
| 4 | CSV output does not reference extracted asset file paths | ReportGenerationService.cs |
| 5 | No JSON output for per-DC analysis table (only CSV) | ReportGenerationService.cs |
| 6 | `LoadLabelsFromCsv` hard-wires CSV format; must be updated when format changes | AnalysisPipeline.cs |
