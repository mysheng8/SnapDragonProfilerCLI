# REPOSITORY SYSTEM INDEX — AUTHORITATIVE ROUTING

Rule:
Always consult this index before analyzing code.

## Module Router

| ModuleKey | Coverage | Entry Symbols | Common Logs | Module Index |
|-----------|----------|---------------|-------------|--------------|
| SDP.CoreWrapper | C# managed wrapper for native SDP profiler core (SWIG-generated P/Invoke) | Client, DeviceManager, CaptureManager, MetricManager, SessionManager, NetworkManager | Device connection, capture lifecycle, metric registration, session management | [docs/index/modules/SDP.CoreWrapper.md](docs/index/modules/SDP.CoreWrapper.md) |
| SDP.ClientFramework | Snapdragon Profiler desktop client orchestrating device connectivity, capture workflows, data visualization, plugin system | SdpApp, MainWindowController, ConnectionManager, ModelManager, UIManager, EventsManager, CommandManager, PluginManager | Logger.LogError, Logger.LogInformation, ConnectionManager, AnalyticsManager | [docs/index/modules/SDP.ClientFramework.md](docs/index/modules/SDP.ClientFramework.md) |
| SDP.Infrastructure.Patterns | Reusable infrastructure patterns: single-consumer action queues, cancellable resource invalidation framework | ActionQueue, CancellableActionQueue, IActionQueue, ResourcesInvalidator, IResourcesInvalidator, IResourcePopulator | m_errorLogger.LogException, ResourcesInvalidator | [docs/index/modules/SDP.Infrastructure.Patterns.md](docs/index/modules/SDP.Infrastructure.Patterns.md) |
| SDP.Testing.AutomatedWorkflow | Automated workflow execution framework for headless/scripted UI testing with widget abstraction layer | AutomatedWorkflowExecutor, ISDPAutomatedWorkflowPlugin, ITestableWidget, IClickableWidget, ITreeViewWidget, WidgetNames | AutomatedWorkflowExecutor, Workflow Executed Successfully, AutomatedWorkflowError.txt | [docs/index/modules/SDP.Testing.AutomatedWorkflow.md](docs/index/modules/SDP.Testing.AutomatedWorkflow.md) |
| SDP.Graphics.TextureConverter | P/Invoke wrapper for native texture format conversion supporting 100+ GPU formats (RGBA, DXT, ETC, ASTC, ATC, YUV) | QonvertWrapper, TextureConverterHelper, TFormats, ConvertImageToRGBA, ConvertRawToRGBA | DllImport TextureConverter.dll, conversion errors | [docs/index/modules/SDP.Graphics.TextureConverter.md](docs/index/modules/SDP.Graphics.TextureConverter.md) |
| SDP.UI.ViewInfrastructure | View infrastructure for event handling abstractions, Gantt chart coordination, selection/highlight management, timeline interaction | IMouseEventHandler, MouseEventController, GanttTrackCoordinator, SelectionViewModel, HighlightViewModel | m_logger (GanttTrackCoordinator) | [docs/index/modules/SDP.UI.ViewInfrastructure.md](docs/index/modules/SDP.UI.ViewInfrastructure.md) |
| Profiler.QGLPlugin.Core | Vulkan graphics profiler plugin: snapshot/trace capture, GFXReconstruct replay processing, resource management (pipelines/textures/shaders), TreeModel generation for CSV export | QGLPlugin.Initialize, connectionEvents_DataProcessed, VkSnapshotModel.PopulateDescSets, VkAPITreeModelBuilder.ProcessAllCalls, ResourcesViewMgr | Logger.LogInformation, Logger.LogWarning, Snapshot taken:, Replay received, SDP::QGLPluginProcessor | [docs/index/modules/Profiler.QGLPlugin.Core.md](docs/index/modules/Profiler.QGLPlugin.Core.md) |
| Profiler.QGLPlugin.ResourceBinding | Runtime binding of Vulkan descriptor sets (textures, buffers, samplers) to DrawCalls via binary buffer loading and in-memory tracking | PopulateDescSets, GetBoundInfo, UpdateDescSet, BoundDescriptorSets | descriptorSet, imageView, texture binding, bound resources | [docs/index/modules/Profiler.QGLPlugin.ResourceBinding.md](docs/index/modules/Profiler.QGLPlugin.ResourceBinding.md) |
| Profiler.DCAP.Wrapper | SWIG-generated C# P/Invoke wrapper for native libDCAP: OpenGL ES/EGL API trace file (.dcap) reading, writing, compression (LZ4/ZSTD), stripping, frame indexing | CaptureFileReader.Initialize, libDCAP.GenerateFrameIndex, GLAdapter, GLDecoder, DCAPCompressConsumer | libDCAP.so, pluginGPU-OpenGLES, OpenGLESDataPlugin, DCAP file operations | [docs/index/modules/Profiler.DCAP.Wrapper.md](docs/index/modules/Profiler.DCAP.Wrapper.md) |
| Profiler.GlobalGPUTrace.Plugin | Lightweight IMetricPlugin for "GPU % Utilization" visualization: processes GGPM trace data, creates stepped-line graph tracks | GlobalGPUTracePlugin, OnDataProcessed, AddDataToTracks, GlobalGPUTraceCaptureData | GPU % Utilization, BUFFER_TYPE_GGPM_TRACE_DATA, tblGlobalGPUTrace, Global GPU Metrics | [docs/index/modules/Profiler.GlobalGPUTrace.Plugin.md](docs/index/modules/Profiler.GlobalGPUTrace.Plugin.md) |
| Profiler.Systrace.Plugin | Comprehensive ftrace/systrace visualization: multi-depth Gantt tracks, graph/marker tracks, 15+ event types (sched_switch, CPU/GPU freq, IRQ, kworker, MDP), extensive statistics views | SystracePlugin, SystraceProcessor, SystraceModel.AddElement, FTraceEventDataRetriever | BUFFER_TYPE_SYSTRACE_DATA, tblSystrace* tables, Trace metric, sched_switch, cpu_frequency | [docs/index/modules/Profiler.Systrace.Plugin.md](docs/index/modules/Profiler.Systrace.Plugin.md) |
| SDPCLI | .NET 4.7.2 CLI tool wrapping Snapdragon Profiler SDK — root overview and mode routing | Program.Main, Application.Run | | [docs/index/modules/SDPCLI.md](docs/index/modules/SDPCLI.md) |
| SDPCLI.Snapshot | Capture pipeline: device connect → GPU snapshot → 7 CSVs → sdp.db (append per CaptureID) → .sdp ZIP | SnapshotCaptureMode.Run, CsvToDbService.ImportAllCsvs, QGLPluginService.ImportCapture | Failed to connect to device, Using SDK snapshot dir, ImportCapture succeeded, DB stable | [docs/index/modules/SDPCLI.Snapshot.md](docs/index/modules/SDPCLI.Snapshot.md) |
| SDPCLI.Analysis | Analysis pipeline: open .sdp → query sdp.db by CaptureID → extract shaders/textures → label DCs (rule-based + LLM) → Markdown report | AnalysisPipeline.RunAnalysis, DatabaseQueryService.GetDrawCallIds, DrawCallLabelService.Label | Step 1: Collecting DrawCalls, Joined metrics to | [docs/index/modules/SDPCLI.Analysis.md](docs/index/modules/SDPCLI.Analysis.md) |
| SDPCLI.Server | HTTP REST server mode: localhost-only HttpListener, async JobManager, DeviceSession state machine, per-operation job runners | ServerMode.Run, HttpServer.Start, JobManager.Submit | Listening on http://localhost, Job failed | [docs/index/modules/SDPCLI.Server.md](docs/index/modules/SDPCLI.Server.md) |
| PySdp.WebUI | FastAPI WebUI server: proxies SDPCLI HTTP calls, exposes Python analysis pipeline as REST, serves browser SPA, orchestrates background pipeline jobs | app, make_router (files), make_router (data), PipelineJobManager, pipeline_manager | WebUI starting, pipeline job started, pipeline job completed, pipeline step failed | [docs/index/modules/PySdp.WebUI.md](docs/index/modules/PySdp.WebUI.md) |
| PySdp.Analysis | Python analysis services: rule-based + LLM DrawCall labeling, bottleneck attribution, GPU category stats, Markdown report generation | generate_label_json, generate_status_json, generate_topdc_json, generate_dashboard_md, generate_analysis_md | label generation failed, status generation failed, topdc generation failed | [docs/index/modules/PySdp.Analysis.md](docs/index/modules/PySdp.Analysis.md) |
| PySdp.Data | DuckDB data layer: schema management + migration, snapshot ingestion (dc/shaders/textures/metrics/labels), typed query API, analysis model registry, question/dashboard CRUD | WorkspaceDB, ingest_snapshot, get_draw_calls, get_dc_detail, register, run_model | ingest failed, list_snapshots failed, draw_calls failed | [docs/index/modules/PySdp.Data.md](docs/index/modules/PySdp.Data.md) |
| PySdp.Client | Synchronous Python client package for SDPCLI Server HTTP API: blocking connect/launch/capture/analyze with job polling and typed exceptions | SdpClient, SdpClient.connect, SdpClient.capture, SdpClient.analyze, JobPoller | SdpConnectionError, Cannot connect to SDPCLI Server | [docs/index/modules/PySdp.Client.md](docs/index/modules/PySdp.Client.md) |


## Topic Router

Device connection → SDP.CoreWrapper, SDP.ClientFramework  
Capture workflow → SDP.CoreWrapper, SDP.ClientFramework  
Metric management → SDP.CoreWrapper, SDP.ClientFramework  
Session lifecycle → SDP.CoreWrapper, SDP.ClientFramework  
Application lifecycle → SDP.ClientFramework  
UI/Views → SDP.ClientFramework  
Plugin system → SDP.ClientFramework  
Event bus → SDP.ClientFramework  
Command pattern → SDP.ClientFramework  
Model management → SDP.ClientFramework  
SWIG/P/Invoke → SDP.CoreWrapper  
Native interop → SDP.CoreWrapper  
Threading/Concurrency → SDP.Infrastructure.Patterns  
Action queues → SDP.Infrastructure.Patterns  
Resource invalidation → SDP.Infrastructure.Patterns  
Task management → SDP.Infrastructure.Patterns  
Test automation → SDP.Testing.AutomatedWorkflow  
UI testing → SDP.Testing.AutomatedWorkflow  
Widget abstraction → SDP.Testing.AutomatedWorkflow  
Headless testing → SDP.Testing.AutomatedWorkflow  
Texture conversion → SDP.Graphics.TextureConverter  
GPU texture formats → SDP.Graphics.TextureConverter  
ASTC/DXT/ETC compression → SDP.Graphics.TextureConverter  
Image format conversion → SDP.Graphics.TextureConverter  
Mouse events → SDP.UI.ViewInfrastructure  
Keyboard events → SDP.UI.ViewInfrastructure  
Drag detection → SDP.UI.ViewInfrastructure  
Gantt visualization → SDP.UI.ViewInfrastructure  
Selection management → SDP.UI.ViewInfrastructure  
Highlight regions → SDP.UI.ViewInfrastructure  
Timeline interaction → SDP.UI.ViewInfrastructure  
Vulkan profiling → Profiler.QGLPlugin.Core  
Graphics profiling → Profiler.QGLPlugin.Core  
Snapshot capture → Profiler.QGLPlugin.Core  
GFXReconstruct → Profiler.QGLPlugin.Core  
DrawCall analysis → Profiler.QGLPlugin.Core  
Pipeline inspection → Profiler.QGLPlugin.Core  
Texture inspection → Profiler.QGLPlugin.Core  
Shader analysis → Profiler.QGLPlugin.Core  
Descriptor sets → Profiler.QGLPlugin.Core, Profiler.QGLPlugin.ResourceBinding  
Texture binding → Profiler.QGLPlugin.ResourceBinding  
Resource binding → Profiler.QGLPlugin.ResourceBinding  
DrawCall resources → Profiler.QGLPlugin.ResourceBinding  
Binary buffer loading → Profiler.QGLPlugin.ResourceBinding  
ImageView mapping → Profiler.QGLPlugin.ResourceBinding  
Bound resources tracking → Profiler.QGLPlugin.ResourceBinding  
TreeModel generation → Profiler.QGLPlugin.Core  
CSV export (GUI) → Profiler.QGLPlugin.Core  
Replay metrics → Profiler.QGLPlugin.Core  
View managers → Profiler.QGLPlugin.Core  
Buffer events → Profiler.QGLPlugin.Core    
OpenGL ES capture → Profiler.DCAP.Wrapper  
EGL tracing → Profiler.DCAP.Wrapper  
DCAP file format → Profiler.DCAP.Wrapper  
API trace compression → Profiler.DCAP.Wrapper  
Frame indexing → Profiler.DCAP.Wrapper  
CLI mode routing → SDPCLI  
CLI snapshot capture → SDPCLI.Snapshot  
Android device connect → SDPCLI.Snapshot  
APK install adb → SDPCLI.Snapshot  
Vulkan snapshot export → SDPCLI.Snapshot  
sdp.db multi-capture → SDPCLI.Snapshot  
CaptureID per-capture → SDPCLI.Snapshot  
VulkanSnapshotModel → SDPCLI.Snapshot  
DrawCall analysis CLI → SDPCLI.Analysis  
DrawCall metrics join → SDPCLI.Analysis  
Shader extraction CLI → SDPCLI.Analysis  
Texture extraction CLI → SDPCLI.Analysis  
SPIR-V to HLSL → SDPCLI.Analysis  
spirv-cross → SDPCLI.Analysis  
LLM labeling DrawCall → SDPCLI.Analysis  
Markdown GPU report → SDPCLI.Analysis  
SDP file analysis → SDPCLI.Analysis  
HTTP REST API → SDPCLI.Server  
Server mode → SDPCLI.Server  
Job queue async → SDPCLI.Server  
DeviceSession state machine → SDPCLI.Server  
localhost server → SDPCLI.Server  
Capture stripping → Profiler.DCAP.Wrapper  
libDCAP wrapper → Profiler.DCAP.Wrapper  
SWIG P/Invoke → Profiler.DCAP.Wrap  
GPU utilization metrics → Profiler.GlobalGPUTrace.Plugin  
GGPM trace data → Profiler.GlobalGPUTrace.Plugin  
Global GPU metrics → Profiler.GlobalGPUTrace.Plugin  
Stepped line graphs → Profiler.GlobalGPUTrace.Plugin  
Ftrace visualization → Profiler.Systrace.Plugin  
Systrace kernel tracing → Profiler.Systrace.Plugin  
sched_switch events → Profiler.Systrace.Plugin  
Multi-depth Gantt → Profiler.Systrace.Plugin  
CPU frequency analysis → Profiler.Systrace.Plugin  
IRQ distribution → Profiler.Systrace.Plugin  
Kworker analysis → Profiler.Systrace.Plugin  
MDP display traces → Profiler.Systrace.Plugin  
Task group hierarchy → Profiler.Systrace.Pluginper  
GL adapter → Profiler.DCAP.Wrapper
VulkanProcessor plugin → Profiler.QGLPlugin.Core
Python WebUI server → PySdp.WebUI
FastAPI routes → PySdp.WebUI
SDPCLI proxy → PySdp.WebUI
Background pipeline job → PySdp.WebUI
Browser SPA → PySdp.WebUI
DrawCall labeling Python → PySdp.Analysis
Rule-based classification → PySdp.Analysis
LLM classification → PySdp.Analysis
Bottleneck attribution → PySdp.Analysis
GPU category stats → PySdp.Analysis
label.json → PySdp.Analysis
topdc.json → PySdp.Analysis
DuckDB schema → PySdp.Data
Snapshot ingest → PySdp.Data
DC query API → PySdp.Data
Analysis model registry → PySdp.Data
Questions CRUD → PySdp.Data
Dashboard CRUD → PySdp.Data
Metric aggregation → PySdp.Data
Python SDPCLI client → PySdp.Client
Scripting profiling → PySdp.Client
CI capture automation → PySdp.Client
Job polling Python → PySdp.Client
