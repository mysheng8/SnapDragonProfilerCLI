# MODULE INDEX — Profiler.QGLPlugin.Core — AUTHORITATIVE ROUTING

## Routing Keywords
**Systems**: Vulkan graphics profiler, snapshot replay, GFXReconstruct integration, resource inspection  
**Concepts**: plugin architecture, view managers, TreeModel generation, GFXR buffer handling, descriptor sets, DrawCall metrics  
**Common Logs**: `Logger.LogInformation`, `Logger.LogWarning`, `Logger.LogError`, `QGLPlugin`, `SnapshotViewMgr`  
**Entry Symbols**: `QGLPlugin.Initialize`, `connectionEvents_DataProcessed`, `VkSnapshotModel.PopulateDescSets`, `ResourcesViewMgr`, `VkAPITreeModelBuilder.ProcessAllCalls`

---

## Role
C# plugin layer orchestrating Vulkan snapshot/trace capture, GFXReconstruct replay processing, multi-view resource management, and TreeModel data structure generation for CSV export.

---

## Entry Points
| Symbol | Location |
|--------|----------|
| QGLPlugin.Initialize | [dll/project/QGLPlugin/QGLPlugin.cs:61](dll/project/QGLPlugin/QGLPlugin.cs#L61) |
| QGLPlugin (constructor) | [dll/project/QGLPlugin/QGLPlugin.cs:71](dll/project/QGLPlugin/QGLPlugin.cs#L71) |
| connectionEvents_DataProcessed | [dll/project/QGLPlugin/QGLPlugin.cs:286](dll/project/QGLPlugin/QGLPlugin.cs#L286) |
| connectionEvents_ClientBufferTransfer | [dll/project/QGLPlugin/QGLPlugin.cs:100](dll/project/QGLPlugin/QGLPlugin.cs#L100) |
| connectionEvents_OpenSnapshotFromSession | [dll/project/QGLPlugin/QGLPlugin.cs:98](dll/project/QGLPlugin/QGLPlugin.cs#L98) |
| VkSnapshotModel.PopulateDescSets | [dll/project/QGLPlugin/VkSnapshotModel.cs:135](dll/project/QGLPlugin/VkSnapshotModel.cs#L135) |
| VkAPITreeModelBuilder.ProcessAllCalls | [dll/project/QGLPlugin/VkAPITreeModelBuilder.cs:28](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L28) |

---

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| QGLPlugin | Main plugin orchestrator implementing IMetricPlugin, manages 9 view managers, handles buffer events | [dll/project/QGLPlugin/QGLPlugin.cs:19](dll/project/QGLPlugin/QGLPlugin.cs#L19) |
| VkSnapshotModel | Snapshot data model storing DrawCall info, descriptor sets, metrics, bound resources | [dll/project/QGLPlugin/VkSnapshotModel.cs:12](dll/project/QGLPlugin/VkSnapshotModel.cs#L12) |
| ResourcesViewMgr | Massive resource manager (5,272 lines) handling pipelines, textures, shaders, descriptor sets | [dll/project/QGLPlugin/ResourcesViewMgr.cs:21](dll/project/QGLPlugin/ResourcesViewMgr.cs#L21) |
| VkAPITreeModelBuilder | Constructs TreeModel hierarchies from replay data for CSV exports, processes all API calls | [dll/project/QGLPlugin/VkAPITreeModelBuilder.cs:13](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L13) |
| DataExplorerViewMgr | View manager building TreeModel data structures with columns/nodes for GUI display | [dll/project/QGLPlugin/DataExplorerViewMgr.cs](dll/project/QGLPlugin/DataExplorerViewMgr.cs) |
| CaptureViewMgr | Manages API trace captures, populates Gantt charts, processes debug markers | [dll/project/QGLPlugin/CaptureViewMgr.cs:16](dll/project/QGLPlugin/CaptureViewMgr.cs#L16) |
| SnapshotViewMgr | Handles snapshot mode lifecycle: capture, provider linking, process selection | [dll/project/QGLPlugin/SnapshotViewMgr.cs:9](dll/project/QGLPlugin/SnapshotViewMgr.cs#L9) |
| ShaderViewMgr | Shader analysis and display management for both snapshot and trace modes | [dll/project/QGLPlugin](dll/project/QGLPlugin) |
| VkCapture | Per-capture data container for resources, metrics, pipeline mappings | [dll/project/QGLPlugin/VkCapture.cs](dll/project/QGLPlugin/VkCapture.cs) |
| QGLModel | Database gateway for Vulkan tables (ModelObjectGateways pattern) | [dll/project/QGLPlugin/QGLModel.cs](dll/project/QGLPlugin/QGLModel.cs) |

---

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| Initialize(IServiceProvider) | Plugin entry point, initializes all view managers | [QGLPlugin.cs:61](dll/project/QGLPlugin/QGLPlugin.cs#L61) | Plugin loaded by framework |
| connectionEvents_DataProcessed | Main event handler dispatching buffer data by BUFFER_TYPE | [QGLPlugin.cs:286](dll/project/QGLPlugin/QGLPlugin.cs#L286) | VulkanProcessor completes replay |
| PopulateDescSets(captureID) | Populates descriptor set bindings from database | [VkSnapshotModel.cs:135](dll/project/QGLPlugin/VkSnapshotModel.cs#L135) | After GFXR buffer received |
| GetBoundInfo(captureID, drawCallID) | Retrieves pipeline/descriptor bindings for DrawCall | [VkSnapshotModel.cs:28](dll/project/QGLPlugin/VkSnapshotModel.cs#L28) | Resource view queries |
| AddDrawCallInfo(captureID, apiID, info) | Stores DrawCall bound info from TreeModel builder | [VkSnapshotModel.cs:92](dll/project/QGLPlugin/VkSnapshotModel.cs#L92) | TreeModel processing |
| ProcessAllCalls() | Iterates snapshot API buffer, builds TreeModel nodes | [VkAPITreeModelBuilder.cs:28](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L28) | DataExplorerViewMgr invalidates |
| InvalidateInitialData(captureId) | Triggers desc sets population, displays screenshot | [QGLPlugin.cs:433](dll/project/QGLPlugin/QGLPlugin.cs#L433) | GFXR buffer processed |
| UpdateBinaryData() | Refreshes resource views with new snapshot data | [ResourcesViewMgr](dll/project/QGLPlugin/ResourcesViewMgr.cs) | Capture has resources |
| PopulateCapture(captureID, apis) | Builds Gantt chart from API trace data | [CaptureViewMgr.cs:18](dll/project/QGLPlugin/CaptureViewMgr.cs#L18) | Trace buffer received |
| OnCaptureClicked | Executes SnapshotCommand when user clicks capture | [SnapshotViewMgr.cs:68](dll/project/QGLPlugin/SnapshotViewMgr.cs#L68) | Snapshot UI button |
| DisplayReplay(captureID, bufferID) | Displays Vulkan replay results | [QGLPlugin.cs:312](dll/project/QGLPlugin/QGLPlugin.cs#L312) | BUFFER_TYPE_VULKAN_REPLAY_DATA |
| GetPipelineDescriptorSets(captureID, pipelineId) | Retrieves descriptor sets for pipeline | [VkSnapshotModel.cs:104](dll/project/QGLPlugin/VkSnapshotModel.cs#L104) | Resource inspection |
| HasSnapshotBuffers() | Checks if API+Metrics buffers loaded | [QGLPlugin.cs:437](dll/project/QGLPlugin/QGLPlugin.cs#L437) | Before invalidating views |
| UpdateDrawcallID(node, submitIdx, cmdBufferIdx) | Generates DrawCall numbering (e.g., "1.1.2") | [VkAPITreeModelBuilder.cs:133](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L133) | TreeModel building |
| GetMetricValue(cmdBufferId, drawcallIdx) | Retrieves metrics for specific DrawCall | [VkAPITreeModelBuilder](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs) | TreeModel node population |
| InvalidateDrawcallCount() | Updates total DrawCall count | [VkAPITreeModelBuilder](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs) | After processing all calls |
| GetVulkanHandleMapping() | Maps capture handles to replay handles | [VkAPITreeModelBuilder.cs:46](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L46) | BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING |
| AddApiNode(vulkanSnapshotApi) | Adds TreeNode for API call | [VkAPITreeModelBuilder](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs) | Processing snapshot APIs |
| GetTexturesForRenderPass | Retrieves textures bound to render pass | [ResourcesViewMgr](dll/project/QGLPlugin/ResourcesViewMgr.cs) | Resource view queries |

---

## Call Flow Skeleton
```
Plugin Load
 ├── QGLPlugin.Initialize(IServiceProvider)
 │    ├── new DataExplorerViewMgr()
 │    ├── new CaptureViewMgr()
 │    ├── new ResourcesViewMgr()
 │    ├── new ShaderViewMgr()
 │    ├── new ScreenCaptureViewMgr()
 │    ├── new PixelHistoryViewMgr()
 │    ├── new VertexDataViewMgr()
 │    ├── new SnapshotViewMgr()
 │    └── Subscribe to ConnectionEvents
 │
Snapshot Capture Flow
 ├── SnapshotViewMgr.OnCaptureClicked
 │    └── Execute SnapshotCommand
 │
GFXR Buffer Transfer (from VulkanProcessor.dll)
 ├── connectionEvents_ClientBufferTransfer
 │    └── Progress bar updates
 │
GFXR Data Processed (after VulkanProcessor replay)
 ├── connectionEvents_DataProcessed(BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA)
 │    ├── GetLocalBuffer(SnapshotApiBuffer, SnapshotDsbBuffer)
 │    ├── InvalidateInitialData(captureId)
 │    │    ├── VkSnapshotModel.PopulateDescSets(captureId)
 │    │    │    └── Query VulkanSnapshotDescriptorSetBindings table
 │    │    └── DisplayInitialScreenshot()
 │    └── ResourcesViewMgr.UpdateBinaryData()
 │
connectionEvents_DataProcessed(BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
 ├── GetLocalBuffer(SnapshotMetricsBuffer)
 └── Execute AddSourceCommand(DataExplorer, "Vulkan")
 │
TreeModel Generation (GUI CSV Export)
 ├── DataExplorerViewMgr.InvalidateVulkanDataExplorerView(captureId)
 │    ├── new VkAPITreeModelBuilder(captureId, metricsModel)
 │    ├── VkAPITreeModelBuilder.ProcessAllCalls()
 │    │    ├── Read SnapshotApiBuffer (QGLPlugin.VulkanSnapshotApi structs)
 │    │    ├── For each API call:
 │    │    │    ├── AddApiNode(vulkanSnapshotApi)
 │    │    │    │    ├── UpdateDrawcallID() → generates "1.1.2" numbering
 │    │    │    │    └── VkSnapshotModel.AddDrawCallInfo(apiID, boundInfo)
 │    │    │    └── Populate TreeNode.Values[] with metrics
 │    │    └── InvalidateDrawcallCount()
 │    └── new TreeModel(treeNodes.ToArray())
 │         └── Raise DataExplorerViewInvalidateEvent(treeModel)
 │              └── GUI exports to CSV
 │
Resource Query Flow (e.g., DrawCallAnalysis.cs)
 ├── GetTexturesForDrawCall("1.1.2")
 │    ├── Query VulkanSnapshotGraphicsPipelines[index] → pipelineResourceID
 │    ├── Query VulkanSnapshotDescriptorSetBindings → imageViewIDs
 │    ├── Query VulkanSnapshotImageViews → textureIDs
 │    └── Return texture resourceIDs
 │
 └── VkSnapshotModel.GetBoundInfo(captureID, drawCallID)
      └── Return cached m_drawCallInfos[captureID][drawCallID]
```

---

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| SnapshotApiBuffer | VulkanProcessor.dll replay → GetLocalBuffer() | VkAPITreeModelBuilder.ProcessAllCalls() | QGLPlugin.ClearApiBuffer() |
| SnapshotMetricsBuffer | VulkanProcessor.dll metrics collection | VkAPITreeModelBuilder (for TreeNode.Values) | QGLPlugin.ClearMetricsBuffer() |
| SnapshotDsbBuffer | VulkanProcessor.dll descriptor sets | VkSnapshotModel.PopulateDescSets() | QGLPlugin.ClearDsbBuffer() |
| VkSnapshotModel | QGLPlugin constructor | All view managers | Session end |
| TreeModel | VkAPITreeModelBuilder.ProcessAllCalls() | GUI CSV exporter | View invalidation |
| m_drawCallInfos | VkAPITreeModelBuilder.AddDrawCallInfo() | ResourcesViewMgr, VkSnapshotModel.GetBoundInfo() | Capture cleared |
| m_metricsCaptured | VkMetricsCapturedModel (from buffer) | VkAPITreeModelBuilder metrics population | CloseCapture() |
| ResourcesPerDrawcall | VkAPITreeModelBuilder.UpdateDrawcallID() | ResourcesViewMgr resource queries | Capture end |
| sdpframe_NNN.gfxrz | VulkanProcessor extracts from .sdp | VulkanProcessor replay engine | Session cleanup |
| DataFilenames/StrippedDataFilenames | connectionEvents_DataProcessed | SnapshotModel, UI displays | Session end |

---

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `Snapshot taken:` | [SnapshotViewMgr.cs:80](dll/project/QGLPlugin/SnapshotViewMgr.cs#L80) | Snapshot command executed successfully |
| `No snapshot APIs are available for capture` | [VkAPITreeModelBuilder.cs:32](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L32) | SnapshotApiBuffer is null/empty |
| `Replay received` | [QGLPlugin.cs:308](dll/project/QGLPlugin/QGLPlugin.cs#L308) | BUFFER_TYPE_VULKAN_REPLAY_DATA processed |
| `Unexpected Buffer ID received:` | [QGLPlugin.cs:423](dll/project/QGLPlugin/QGLPlugin.cs#L423) | Unknown BufferID in DataProcessed event |
| `Couldn't find end capture image` | [VkAPITreeModelBuilder.cs:85](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L85) | VulkanEndCaptureImage query failed |
| `Drawcall metrics missing, Command Buffer Id:` | [VkAPITreeModelBuilder.cs:200+](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L200) | Metrics not found for DrawCall (warning) |
| `Unable to launch File Explorer for path:` | [QGLPlugin.cs:341](dll/project/QGLPlugin/QGLPlugin.cs#L341) | GLTF export path open failed |
| `Logger.LogInformation` | Framework-wide | Plugin initialization, status updates |
| `Logger.LogWarning` | Various classes (QGLPlugin, VkAPITreeModelBuilder, TensorHelper) | Non-fatal issues, missing data |
| `Logger.LogError` | Various classes | Fatal errors requiring user attention |
| `Tensor {0} not found in capture {1}` | [TensorHelper.cs:31](dll/project/QGLPlugin/TensorHelper.cs#L31) | Tensor resource missing |
| `GLTF file downloaded successfully.` | [QGLPlugin.cs:336](dll/project/QGLPlugin/QGLPlugin.cs#L336) | GLTF export completed |
| `SDP::QGLPluginProcessor` | [QGLPlugin.cs:352](dll/project/QGLPlugin/QGLPlugin.cs#L352) | Processor plugin registration name |

---

## Buffer Types Reference
```csharp
// Buffer categories handled by QGLPlugin (from SDPCore.h)
BUFFER_TYPE_QGL_TRACE_DATA                            // API trace packets (trace mode)
BUFFER_TYPE_VULKAN_REPLAY_DATA                        // Replay results
BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA  // Shader profiles
BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA               // Snapshot shader info
BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA                  // Trace shader info
BUFFER_TYPE_QGL_DATA                                  // General QGL data
BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF         // GLTF export
BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA                // Metrics from replay (snapshot)
BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA        // Snapshot API calls (from .gfxrz replay)
BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA                // .gfxrz file (full)
BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA       // .gfxrz file (stripped)
BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING              // Capture→Replay handle mapping
BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA     // Shader disassembly
```

---

## Component Architecture
```
QGLPlugin.dll (68 .cs files)
├── Core Plugin
│   ├── QGLPlugin.cs (994 lines) - Main orchestrator
│   ├── QGLModel.cs - Database gateway
│   └── VkSnapshotModel.cs (295 lines) - Snapshot data model
│
├── View Managers (9 classes)
│   ├── DataExplorerViewMgr.cs - TreeModel builder, CSV export
│   ├── ResourcesViewMgr.cs (5,272 lines) - Pipeline/texture/shader resources
│   ├── CaptureViewMgr.cs (621 lines) - API trace Gantt charts
│   ├── SnapshotViewMgr.cs (150 lines) - Snapshot lifecycle
│   ├── ShaderViewMgr.cs - Shader analysis
│   ├── ScreenCaptureViewMgr.cs - Screenshot display
│   ├── PixelHistoryViewMgr.cs - Pixel history tracking
│   ├── VertexDataViewMgr.cs - Vertex buffer inspection
│   └── InspectorViewMgr.cs - General inspector
│
├── TreeModel Builders
│   ├── VkAPITreeModelBuilder.cs (1,656 lines) - Main builder
│   └── TreeNode, TreeModel (from framework)
│
├── Data Models
│   ├── VkCapture.cs - Per-capture container
│   ├── VkMetricsCapturedModel.cs - Metrics storage
│   ├── VkBoundInfo.cs - DrawCall binding info
│   └── BinaryDataPair.cs (from framework)
│
├── Helpers
│   ├── VkHelper.cs - Vulkan utilities
│   ├── TensorHelper.cs - Tensor parsing
│   ├── SpirvCross.cs - Shader disassembly
│   └── TextureConverter.cs - Texture format conversion
│
├── Vulkan Enums (30+ files)
│   ├── VkAccessFlagBits.cs
│   ├── VkFormat.cs
│   ├── VkImageLayout.cs
│   └── ... (generated from Vulkan spec)
│
└── Database Gateways (ModelObjectGateways/)
    ├── QGLApiTracePackets.cs
    ├── QGLApiQueueSubmitTimings.cs
    ├── VulkanSnapshotGraphicsPipelines.cs
    ├── VulkanSnapshotDescriptorSetBindings.cs
    └── VulkanSnapshotTextures.cs
```

---

## Dependencies
```
SDPClientFramework.dll - Core framework (SdpApp, ConnectionManager, EventsManager)
TextureConverter.dll - Texture format conversions
VulkanProcessor.dll - Native C++ GFXReconstruct replay engine (closed-source)
Newtonsoft.Json - JSON parsing
System.Data.SQLite - Database access
Gtk, Cairo - GUI rendering
```

---

## Search Hints
```
Find main plugin entry:
search "class QGLPlugin.*IMetricPlugin"
open dll/project/QGLPlugin/QGLPlugin.cs:19

Find TreeModel generation:
search "VkAPITreeModelBuilder.*ProcessAllCalls"
open dll/project/QGLPlugin/VkAPITreeModelBuilder.cs:28

Find DrawCall info storage:
search "AddDrawCallInfo|GetBoundInfo"
open dll/project/QGLPlugin/VkSnapshotModel.cs:28

Find buffer event handler:
search "connectionEvents_DataProcessed"
open dll/project/QGLPlugin/QGLPlugin.cs:286

Find descriptor set population:
search "PopulateDescSets"
open dll/project/QGLPlugin/VkSnapshotModel.cs:135

Find snapshot capture trigger:
search "OnCaptureClicked"
open dll/project/QGLPlugin/SnapshotViewMgr.cs:68

Find resource view manager:
search "class ResourcesViewMgr"
open dll/project/QGLPlugin/ResourcesViewMgr.cs:21

Find metrics processing:
search "BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA"
open dll/project/QGLPlugin/QGLPlugin.cs:349
```

---

## Integration Points
| External System | Integration Method | Location |
|----------------|-------------------|----------|
| VulkanProcessor.dll | GetProcessorPlugin("SDP::QGLPluginProcessor") | [QGLPlugin.cs:352](dll/project/QGLPlugin/QGLPlugin.cs#L352) |
| SDPClientFramework | SdpApp.EventsManager.ConnectionEvents | [QGLPlugin.cs:94](dll/project/QGLPlugin/QGLPlugin.cs#L94) |
| Database (sdp.db) | QGLModel static methods, ModelObjectDataList | [QGLModel.cs](dll/project/QGLPlugin/QGLModel.cs) |
| GUI Views | SdpApp.EventsManager.Raise<EventArgs>() | Framework-wide |
| SDPCLI | SimpleClientDelegate.cs → same ConnectionEvents | [SDPCLI/source/SimpleClientDelegate.cs](SDPCLI/source/SimpleClientDelegate.cs) |
| DrawCallAnalysis | Database queries (VulkanSnapshot* tables) | [SDPCLI/source/DrawCallAnalysis.cs](SDPCLI/source/DrawCallAnalysis.cs) |

---

## Notes
- **Snapshot vs. Trace Mode**: Snapshot stores static objects in database + sequences in .gfxrz; Trace stores API calls in QGLApiTracePackets
- **DrawCall Numbering**: Format "submitIndex.cmdBufferIndex.drawcallIndex" (e.g., "1.1.2") generated by VkAPITreeModelBuilder at TreeModel construction time, NOT stored in database
- **CLI Limitations**: SDPCLI cannot access TreeModel data (exists only in GUI memory), must query database directly
- **GFXReconstruct Integration**: .gfxrz files are industry-standard Vulkan trace format, replayed by VulkanProcessor.dll native binary
- **Buffer Flow**: Device → VulkanProcessor.dll (replay) → ClientBufferTransfer events → QGLPlugin.GetLocalBuffer() → View managers
- **CSV Export**: GUI builds TreeModel in-memory → exports via DataExplorerView, no database table stores DrawCall-numbered rows
