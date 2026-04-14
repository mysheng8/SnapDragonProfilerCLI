# MODULE INDEX — SDPCLI.Snapshot

## Role

GPU snapshot capture pipeline for SDPCLI.  
Connects to Android device via SDK, triggers one frame snapshot, exports 7 CSVs (all stamped with `CaptureID`) into `snapshot_{id}/`, imports them into `sdp.db` (append-only, never drop), archives session as `.sdp` ZIP.

---

## Entry Points

| Symbol | Location |
|--------|----------|
| `SnapshotCaptureMode.Run()` | [source/Modes/SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L53) |

---

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `SnapshotCaptureMode` | Full capture loop: init → APK → connect → launch → ENTER loop → export → archive | [source/Modes/SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L18) |
| `DeviceConnectionService` | `adb devices` check, APK install, `SDPClient.ConnectDevice()` | [source/Services/Capture/DeviceConnectionService.cs](../../../SDPCLI/source/Services/Capture/DeviceConnectionService.cs) |
| `AppLaunchService` | Lists launchable apps, sends launch command to device | [source/Services/Capture/AppLaunchService.cs](../../../SDPCLI/source/Services/Capture/AppLaunchService.cs) |
| `CaptureExecutionService` | Creates `Capture` (type 4 = snapshot), calls `Capture.Start()` | [source/Services/Capture/CaptureExecutionService.cs](../../../SDPCLI/source/Services/Capture/CaptureExecutionService.cs) |
| `QGLPluginService` | `SDPProcessorPlugin.ImportCapture(srcId, dstId, dbPath)` + `ExportMetricsToCsv` | [source/Services/Capture/QGLPluginService.cs](../../../SDPCLI/source/Services/Capture/QGLPluginService.cs) |
| `VulkanSnapshotModel` | Parses `ApiBuffer`+`DsbBuffer`; exports 7 CSVs (all with `CaptureID`) | [source/Models/VulkanSnapshotModel.cs](../../../SDPCLI/source/Models/VulkanSnapshotModel.cs) |
| `CsvToDbService` | `CREATE TABLE IF NOT EXISTS` + `DELETE WHERE CaptureID=x` + bulk INSERT (multi-capture safe) | [source/Services/Capture/CsvToDbService.cs](../../../SDPCLI/source/Services/Capture/CsvToDbService.cs) |
| `DataExportService` | Exports screenshot after each capture | [source/Services/Capture/DataExportService.cs](../../../SDPCLI/source/Services/Capture/DataExportService.cs) |
| `SessionArchiveService` | Zips session dir → `.sdp` at session close | [source/Services/Capture/SessionArchiveService.cs](../../../SDPCLI/source/Services/Capture/SessionArchiveService.cs) |
| `SessionSummaryService` | Writes per-session summary metadata | [source/Services/Capture/SessionSummaryService.cs](../../../SDPCLI/source/Services/Capture/SessionSummaryService.cs) |

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

---

## Call Flow

```
SnapshotCaptureMode.Run()
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

---

## DB Tables Written

| Table | Filter | Notes |
|-------|--------|-------|
| `DrawCallParameters` | `CaptureID` | +`CmdBufferIdx` optional |
| `DrawCallBindings` | `CaptureID` | `PipelineID`, `ImageViewID` |
| `DrawCallRenderTargets` | `CaptureID` (if column exists) | `ColumnExists()` check for legacy |
| `DrawCallVertexBuffers` | `CaptureID` | |
| `DrawCallIndexBuffers` | `CaptureID` | |
| `DrawCallMetrics` | `CaptureID` | from `QGLPluginService.ExportMetricsToCsv` |
| `VulkanSnapshotShaderStages` | `captureID` | written by QGLPlugin native; accumulates |
| `VulkanSnapshotByteBuffers` | `captureID` | accumulates |
| `VulkanSnapshotTextures` | `captureID` | accumulates |
| `VulkanSnapshotImageViews` | `captureID` | accumulates |
| `VulkanSnapshotGraphicsPipelines` | `captureID` | polled to detect replay completion |

---

## Log → Code Map

| Log | Location |
|-----|----------|
| `"Failed to connect to device. Exiting..."` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L83) |
| `"✗ No device connected via ADB"` | [DeviceConnectionService.cs](../../../SDPCLI/source/Services/Capture/DeviceConnectionService.cs#L47) |
| `"Using SDK snapshot dir: snapshot_{x}"` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L192) |
| `"ImportCapture succeeded - polling DB..."` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L395) |
| `"DB stable: {n} pipelines"` | [SnapshotCaptureMode.cs](../../../SDPCLI/source/Modes/SnapshotCaptureMode.cs#L403) |
