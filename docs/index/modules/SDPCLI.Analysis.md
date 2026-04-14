# MODULE INDEX — SDPCLI.Analysis

## Role

Post-capture analysis pipeline for SDPCLI.  
Opens an `.sdp` ZIP, queries `sdp.db` filtered by `CaptureID`, collects DrawCall metadata, extracts SPIR-V shaders (→ HLSL via `spirv-cross`) and textures (→ PNG), labels each DrawCall with rule-based engine + optional LLM, joins GPU metrics CSV, emits a Markdown report.

---

## Entry Points

| Symbol | Location |
|--------|----------|
| `AnalysisMode.Run()` | [source/Modes/AnalysisMode.cs](../../../SDPCLI/source/Modes/AnalysisMode.cs) |
| `DrawCallAnalysisMode.Run()` | [source/Modes/DrawCallAnalysisMode.cs](../../../SDPCLI/source/Modes/DrawCallAnalysisMode.cs) |
| `AnalysisPipeline.RunAnalysis()` | [source/Analysis/AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L55) |

---

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `AnalysisPipeline` | 4-step orchestrator: collect DCs → extract → label → metrics → report | [source/Analysis/AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs) |
| `SdpFileService` | Extracts `sdp.db` from `.sdp` ZIP; locates `snapshot_{id}/` inside archive | [source/Services/Analysis/SdpFileService.cs](../../../SDPCLI/source/Services/Analysis/SdpFileService.cs) |
| `DatabaseQueryService` | `GetDrawCallIds(captureId, cmdBuf)` → `DrawCallParameters WHERE CaptureID=x` | [source/Services/Analysis/DatabaseQueryService.cs](../../../SDPCLI/source/Services/Analysis/DatabaseQueryService.cs) |
| `DrawCallQueryService` | `GetDrawCallInfo()` — pipeline/textures/RTs/VBs/IB/shaders per draw call | [source/Services/Analysis/DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs) |
| `ShaderExtractor` | `VulkanSnapshotShaderStages WHERE captureID=x` → SPIR-V → `spirv-cross` → HLSL | [source/Tools/ShaderExtractor.cs](../../../SDPCLI/source/Tools/ShaderExtractor.cs) |
| `TextureExtractor` | `VulkanSnapshotByteBuffers WHERE captureID=x` → PNG | [source/Tools/TextureExtractor.cs](../../../SDPCLI/source/Tools/TextureExtractor.cs) |
| `DrawCallLabelService` | Rule-based + LLM labeling → `Category`/`Detail` per DC | [source/Services/Analysis/DrawCallLabelService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallLabelService.cs) |
| `AttributionRuleEngine` | Evaluates `attribution_rules.json` against DrawCall properties | [source/Services/Analysis/AttributionRuleEngine.cs](../../../SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs) |
| `MetricsQueryService` | Loads `DrawCallMetrics` CSV from `snapshot_{id}/`; joins to DC list by `DrawCallApiID` | [source/Services/Analysis/MetricsQueryService.cs](../../../SDPCLI/source/Services/Analysis/MetricsQueryService.cs) |
| `ReportGenerationService` | `GenerateSummaryReport()` → Markdown; `GenerateLabeledMetricsCsv()` | [source/Services/Analysis/ReportGenerationService.cs](../../../SDPCLI/source/Services/Analysis/ReportGenerationService.cs) |
| `DashboardGenerationService` | Generates HTML dashboard from labeled DC data | [source/Services/Analysis/DashboardGenerationService.cs](../../../SDPCLI/source/Services/Analysis/DashboardGenerationService.cs) |
| `AttributionReportService` | Generates attribution summary by category | [source/Services/Analysis/AttributionReportService.cs](../../../SDPCLI/source/Services/Analysis/AttributionReportService.cs) |
| `DcContentAnalysisService` | Per-DC content inspection (texture formats, pipeline state) | [source/Services/Analysis/DcContentAnalysisService.cs](../../../SDPCLI/source/Services/Analysis/DcContentAnalysisService.cs) |

---

## Key Methods

| Method | Purpose | Location |
|--------|---------|----------|
| `AnalysisPipeline.RunAnalysis()` | Opens DB → Step1 collect → Step1.5 extract → Step2 label → Step3 metrics → Step4 report | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L55) |
| `DatabaseQueryService.GetDrawCallIds()` | Priority: `DrawCallParameters WHERE CaptureID=x` → `SCOPEDrawStages` → pipeline count | [DatabaseQueryService.cs](../../../SDPCLI/source/Services/Analysis/DatabaseQueryService.cs) |
| `DrawCallQueryService.GetDrawCallInfoByApiId()` | Resolves: params → pipeline (DrawCallBindings+CaptureID) → textures → RTs → VBs → shaders | [DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs#L173) |
| `DrawCallQueryService.ColumnExists()` | `PRAGMA table_info` check — conditionally adds `CaptureID` filter for legacy tables | [DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs) |

---

## Call Flow

```
AnalysisMode.Run()
├── SelectCaptureIdFromSdp()         ← scan ZIP for snapshot_* entries
└── AnalysisPipeline.RunAnalysis(sdpPath, outputDir, captureId)
    ├── SdpFileService.FindDatabasePath()     ← extract sdp.db from ZIP
    ├── DatabaseQueryService.OpenDatabase()
    ├── [Step 1]   GetDrawCallIds(captureId)
    │               ← SELECT DrawCallApiID FROM DrawCallParameters WHERE CaptureID=x
    │              DrawCallQueryService.GetDrawCallInfo() × N
    ├── [Step 1.5] ShaderExtractor  × pipeline  → HLSL files
    │              TextureExtractor × texture   → PNG files
    ├── [Step 2]   DrawCallLabelService.Label()  [+ LlmApiWrapper optional]
    ├── [Step 3]   MetricsQueryService.LoadMetrics(snapshot_{id}/)
    │              join by DrawCallApiID
    │              ReportGenerationService.GenerateLabeledMetricsCsv()
    └── [Step 4]   ReportGenerationService.GenerateSummaryReport() → .md
```

---

## DB Tables Read

| Table | Filter | Notes |
|-------|--------|-------|
| `DrawCallParameters` | `CaptureID` | primary DC source |
| `DrawCallBindings` | `CaptureID` | `PipelineID`, `ImageViewID` |
| `DrawCallRenderTargets` | `CaptureID` (if column exists) | `ColumnExists()` check for legacy |
| `DrawCallVertexBuffers` | `CaptureID` | |
| `DrawCallIndexBuffers` | `CaptureID` | |
| `DrawCallMetrics` | `CaptureID` | GPU counters per DC |
| `VulkanSnapshotShaderStages` | `captureID` | SPIR-V blobs |
| `VulkanSnapshotByteBuffers` | `captureID` | texture raw bytes |
| `VulkanSnapshotTextures` | `captureID` | texture metadata |
| `VulkanSnapshotImageViews` | `captureID` | image view → texture mapping |
| `VulkanSnapshotGraphicsPipelines` | `captureID` | pipeline state |

---

## Log → Code Map

| Log | Location |
|-----|----------|
| `"Step 1: Collecting DrawCalls"` | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L80) |
| `"→ Joined metrics to {n} / {total} DCs"` | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L218) |
