# MODULE INDEX — SDPCLI

## Role

.NET 4.7.2 CLI wrapper around Qualcomm Snapdragon Profiler SDK.

**Capture mode** — connects to Android device via SDK, triggers GPU snapshot frame, exports 7 CSVs (all with `CaptureID` column) into `snapshot_{id}/`, imports them into `sdp.db` (append per capture, never drop), archives session as `.sdp` ZIP.

**Analysis mode** — opens existing `.sdp`, queries `sdp.db` filtered by `CaptureID`, extracts SPIR-V shaders (→ HLSL via `spirv-cross`), extracts textures (→ PNG), labels each DrawCall with rules + optional LLM, joins GPU metrics CSV, emits Markdown summary report.

---

## Entry Points

| Symbol | Location |
|--------|----------|
| `Program.Main()` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L16) |
| `Program.SetupEnvironment()` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L140) |
| `Application.Run()` | [source/Application.cs](../../../SDPCLI/source/Application.cs#L100) |
| `SnapshotCaptureMode.Run()` | [source/Modes/SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L53) |
| `AnalysisMode.Run()` | [source/Modes/AnalysisMode.cs](../../../SDPCLI/source/Modes/AnalysisMode.cs) |
| `DrawCallAnalysisMode.Run()` | [source/Modes/DrawCallAnalysisMode.cs](../../../SDPCLI/source/Modes/DrawCallAnalysisMode.cs) |

---

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `Program` | DLL path setup, arg parse → `Application` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L11) |
| `Application` | Mode routing (1–5), wires services | [source/Application.cs](../../../SDPCLI/source/Application.cs#L14) |
| `SnapshotCaptureMode` | Full capture loop: init → APK → connect → launch → ENTER loop → export → archive | [source/Modes/SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L18) |
| `AnalysisPipeline` | 4-step orchestrator: collect DCs → label → join metrics → report | [source/Analysis/AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs) |
| `SDPClient` | Wraps `SDPCore.Client`; exposes `DeviceManager`, `SessionManager`, `CaptureManager` | [source/SDPClient.cs](../../../SDPCLI/source/SDPClient.cs) |
| `CliClientDelegate` | SDK callbacks: `OnCaptureComplete`, `OnDataProcessed`; caches `DsbBuffer`/`ApiBuffer`/`MetricsBuffer` | [source/CliClientDelegate.cs](../../../SDPCLI/source/CliClientDelegate.cs) |
| `DeviceConnectionService` | `adb devices` check, APK install, `SDPClient.ConnectDevice()` | [source/Services/Capture/DeviceConnectionService.cs](../../../SDPCLI/source/Services/Capture/DeviceConnectionService.cs) |
| `CaptureExecutionService` | Creates `Capture` (type 4 = snapshot), calls `Capture.Start()` | [source/Services/Capture/CaptureExecutionService.cs](../../../SDPCLI/source/Services/Capture/CaptureExecutionService.cs) |
| `QGLPluginService` | `SDPProcessorPlugin.ImportCapture(srcId, dstId, dbPath)` + `ExportMetricsToCsv` | [source/Services/Capture/QGLPluginService.cs](../../../SDPCLI/source/Services/Capture/QGLPluginService.cs) |
| `CsvToDbService` | `CREATE TABLE IF NOT EXISTS` + `DELETE WHERE CaptureID=x` + bulk INSERT (multi-capture safe) | [source/Services/Capture/CsvToDbService.cs](../../../SDPCLI/source/Services/Capture/CsvToDbService.cs) |
| `SessionArchiveService` | Zips session dir → `.sdp` at session close | [source/Services/Capture/SessionArchiveService.cs](../../../SDPCLI/source/Services/Capture/SessionArchiveService.cs) |
| `VulkanSnapshotModel` | Parses `ApiBuffer`+`DsbBuffer`; exports 7 CSVs (all with `CaptureID`) | [source/Models/VulkanSnapshotModel.cs](../../../SDPCLI/source/Models/VulkanSnapshotModel.cs) |
| `DatabaseQueryService` | `GetDrawCallIds(captureId, cmdBuf)` → `DrawCallParameters WHERE CaptureID=x` | [source/Services/Analysis/DatabaseQueryService.cs](../../../SDPCLI/source/Services/Analysis/DatabaseQueryService.cs) |
| `DrawCallQueryService` | `GetDrawCallInfo()` — pipeline/textures/RTs/VBs/IB/shaders per draw call | [source/Services/Analysis/DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs) |
| `ShaderExtractor` | `VulkanSnapshotShaderStages WHERE captureID=x` → SPIR-V → `spirv-cross` → HLSL | [source/Tools/ShaderExtractor.cs](../../../SDPCLI/source/Tools/ShaderExtractor.cs) |
| `TextureExtractor` | `VulkanSnapshotByteBuffers WHERE captureID=x` → PNG | [source/Tools/TextureExtractor.cs](../../../SDPCLI/source/Tools/TextureExtractor.cs) |
| `DrawCallLabelService` | Rule-based + LLM labeling → `Category`/`Detail` per DC | [source/Services/Analysis/DrawCallLabelService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallLabelService.cs) |
| `ReportGenerationService` | `GenerateSummaryReport()` → Markdown; `GenerateLabeledMetricsCsv()` | [source/Services/Analysis/ReportGenerationService.cs](../../../SDPCLI/source/Services/Analysis/ReportGenerationService.cs) |

---

## Key Methods

| Method | Purpose | Location |
|--------|---------|----------|
| `SnapshotCaptureMode.InitializeClient()` | `SdpApp.Init` → `QGLPlugin()` → `SDPClient.Initialize()` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L249) |
| `SnapshotCaptureMode.WaitForDataProcessed()` | Polls `OnDataProcessed` count until stable 2 s (max 90 s) | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L343) |
| `SnapshotCaptureMode.ReplayAndGetBuffers()` | `ImportCapture()` → polls `VulkanSnapshotGraphicsPipelines` → returns `DsbBuffer` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L383) |
| `SnapshotCaptureMode.ExportDrawCallData()` | `VulkanSnapshotModel.Export*ToCSV()` ×7 → `CsvToDbService.ImportAllCsvs()` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L412) |
| `DeviceConnectionService.Connect()` | `SDPClient.ConnectDevice()` → `Device` | [DeviceConnectionService.cs](../../../SDPCLI/source/Services/Capture/DeviceConnectionService.cs#L151) |
| `CsvToDbService.ImportAllCsvs()` | Per CSV: `CreateOrExtendTable` + `DELETE WHERE CaptureID=x` + INSERT | [CsvToDbService.cs](../../../SDPCLI/source/Services/Capture/CsvToDbService.cs#L29) |
| `CsvToDbService.CreateOrExtendTable()` | `CREATE TABLE IF NOT EXISTS`; `ALTER TABLE ADD COLUMN` for legacy SDP compat | [CsvToDbService.cs](../../../SDPCLI/source/Services/Capture/CsvToDbService.cs) |
| `DatabaseQueryService.GetDrawCallIds()` | Priority: `DrawCallParameters WHERE CaptureID=x` → `SCOPEDrawStages` → pipeline count | [DatabaseQueryService.cs](../../../SDPCLI/source/Services/Analysis/DatabaseQueryService.cs) |
| `DrawCallQueryService.GetDrawCallInfoByApiId()` | Resolves: params → pipeline (DrawCallBindings+CaptureID) → textures → RTs → VBs → shaders | [DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs#L173) |
| `DrawCallQueryService.ColumnExists()` | `PRAGMA table_info` check — used to conditionally add `CaptureID` filter for tables that may be legacy | [DrawCallQueryService.cs](../../../SDPCLI/source/Services/Analysis/DrawCallQueryService.cs) |
| `AnalysisPipeline.RunAnalysis()` | Opens DB → Step1 collect → Step1.5 extract → Step2 label → Step3 metrics → Step4 report | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L55) |

---

## Call Flow

### Capture
```
Program.Main()
└── Application.Run()  [mode=capture]
    └── SnapshotCaptureMode.Run()
        ├── InitializeClient()
        │   ├── SdpApp.Init(ConsolePlatform)
        │   ├── new QGLPlugin()
        │   └── SDPClient.Initialize(SessionSettings, CliClientDelegate)
        ├── DeviceConnectionService.CheckAndInstallAPKs()
        ├── DeviceConnectionService.Connect()          → Device
        ├── AppLaunchService.SelectAndLaunch()
        └── [ENTER loop]
            ├── CaptureExecutionService.StartCapture() → Capture.Start()
            ├── WaitOne(_captureCompleteEvent, 30s)
            ├── WaitForDataProcessed()
            ├── scan snapshot_* dirs → real captureId
            ├── ReplayAndGetBuffers(captureId)
            │   └── QGLPluginService.ImportCapture() → VulkanSnapshot* tables
            ├── ExportDrawCallData()
            │   ├── VulkanSnapshotModel.LoadSnapshot()
            │   ├── model.Export*ToCSV() ×7            → snapshot_{id}/*.csv
            │   └── CsvToDbService.ImportAllCsvs()     → sdp.db (APPEND per CaptureID)
            └── DataExportService.ExportData()         → screenshot
        [ESC] SessionArchiveService.CreateSessionArchive() → .sdp ZIP
```

### Analysis
```
Program.Main()
└── Application.Run()  [mode=analysis]
    └── AnalysisMode.Run()
        ├── SelectCaptureIdFromSdp()   ← scan ZIP for snapshot_* entries
        └── AnalysisPipeline.RunAnalysis(sdpPath, outputDir, captureId)
            ├── SdpFileService.FindDatabasePath()     ← extract sdp.db from ZIP
            ├── DatabaseQueryService.OpenDatabase()
            ├── [Step 1] GetDrawCallIds(captureId)
            │           ← SELECT DrawCallApiID FROM DrawCallParameters WHERE CaptureID=x
            │   DrawCallQueryService.GetDrawCallInfo() × N
            ├── [Step 1.5] ShaderExtractor × pipeline
            │              TextureExtractor × texture
            ├── [Step 2] DrawCallLabelService.Label()  [+ LlmApiWrapper optional]
            ├── [Step 3] MetricsCsvService.LoadMetricsFromSession(snapshot_{id}/)
            │            join by DrawCallApiID
            │            ReportGenerationService.GenerateLabeledMetricsCsv()
            └── [Step 4] ReportGenerationService.GenerateSummaryReport() → .md
```

---

## DB Table Map

| Table | Written by | Filter | Notes |
|-------|-----------|--------|-------|
| `DrawCallParameters` | `CsvToDbService` | `CaptureID` | +`CmdBufferIdx` optional |
| `DrawCallBindings` | `CsvToDbService` | `CaptureID` | `PipelineID`, `ImageViewID` |
| `DrawCallRenderTargets` | `CsvToDbService` | `CaptureID` (if column exists) | `ColumnExists()` check for legacy |
| `DrawCallVertexBuffers` | `CsvToDbService` | `CaptureID` | |
| `DrawCallIndexBuffers` | `CsvToDbService` | `CaptureID` | |
| `DrawCallMetrics` | `CsvToDbService` | `CaptureID` | from `QGLPluginService.ExportMetricsToCsv` |
| `VulkanSnapshotShaderStages` | `QGLPlugin` native | `captureID` | accumulates — never dropped |
| `VulkanSnapshotByteBuffers` | `QGLPlugin` native | `captureID` | accumulates |
| `VulkanSnapshotTextures` | `QGLPlugin` native | `captureID` | accumulates |
| `VulkanSnapshotImageViews` | `QGLPlugin` native | `captureID` | accumulates |
| `VulkanSnapshotGraphicsPipelines` | `QGLPlugin` native | `captureID` | accumulates |

---

## Log → Code Map

| Log | Location |
|-----|----------|
| `"Failed to connect to device. Exiting..."` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L83) |
| `"✗ No device connected via ADB"` | [DeviceConnectionService.cs](../../../SDPCLI/source/Services/Capture/DeviceConnectionService.cs#L47) |
| `"Using SDK snapshot dir: snapshot_{x}"` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L192) |
| `"ImportCapture succeeded - polling DB..."` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L395) |
| `"DB stable: {n} pipelines"` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L403) |
| `"Step 1: Collecting DrawCalls"` | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L80) |
| `"→ Joined metrics to {n} / {total} DCs"` | [AnalysisPipeline.cs](../../../SDPCLI/source/Analysis/AnalysisPipeline.cs#L218) |
| `"GetDrawCallIds: using snapshot CSV override"` | **removed** — DB now has CaptureID column |
