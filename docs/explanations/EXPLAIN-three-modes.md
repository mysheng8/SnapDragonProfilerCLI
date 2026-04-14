---
type: explanation
topic: SDPCLI 三种模式概览与 SDK 架构
module_key: SDPCLI
source_scope:
  - SDPCLI/source/Modes/
  - SDPCLI/source/Server/
  - SDPCLI/source/Analysis/
  - SDPCLI/source/Services/
module_index: docs/index/modules/SDPCLI.md
based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
  - PLAN-2026-04-11-http-server-mode.md
  - PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
status: mixed
audience:
  - self
  - onboarding
last_updated: 2026-04-14
---

# SDPCLI 三种模式：概览与文档索引

## What this document explains

本文档是 SDPCLI 三种运行模式（snapshot / analysis / server）的**概览索引**，同时包含跨模式共享的 SDK 架构、DLL 依赖关系、核心 API 参考和关键数据结构。各模式的完整实现细节分别位于独立的 EXPLAIN 文档中。

## Scope

- **Included**：三模式总览与路由关系、SDK Plugin-Host 架构、DLL 依赖链、核心 API 示例（Capture + DataModel）、DescSetBindings 数据结构、sdp.db 表清单、代码与上下文的差异说明
- **Excluded**：各模式的完整调用链、Pipeline 步骤、Job Runner 阶段细节、输出文件结构（分别见 EXPLAIN-snapshot / EXPLAIN-analysis / EXPLAIN-server）

## Routing

- **ModuleKey**: SDPCLI
- **Module Index**: [docs/index/modules/SDPCLI.md](../index/modules/SDPCLI.md)
- **SourceScope**: `SDPCLI/source/Modes/`, `SDPCLI/source/Server/`, `SDPCLI/source/Analysis/`, `SDPCLI/source/Services/`

## Context Basis

- **Implementations**: IMPL-2026-04-14-http-server-mode.md, IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
- **Plans**: PLAN-2026-04-11-http-server-mode.md, PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
- **Findings**: FINDING-2026-04-07-shader-texture-export-structure.md, FINDING-2026-04-08-parallelism-thread-safety.md

## Reality Status

- **Stable**: Snapshot 模式（多次实测验证）、Analysis 模式（增量执行已验证）
- **WIP**: Server 模式（2026-04-14 新增，构建通过但未经实际设备测试）
- **Outdated or conflicting**: PassMode 体系（StatsOnly/AnalysisOnly）与 AnalysisTarget 并存，有功能重叠，建议未来统一为 AnalysisTarget

---

## 概述

SDPCLI 是 Snapdragon Profiler 的命令行替代工具，三种核心模式共用同一套 SDK 依赖链，但面向不同的使用场景：

| 模式 | 入口 | 目的 | 详细文档 |
|------|------|------|---------|
| `snapshot` | `SnapshotCaptureMode.Run()` | 实时连接设备，触发 GPU 抓帧，保存 .sdp | [EXPLAIN-snapshot.md](EXPLAIN-snapshot.md) |
| `analysis` | `AnalysisMode.Run()` / `AnalysisPipeline.RunAnalysis()` | 离线打开 .sdp，分析 DrawCall，生成报告 | [EXPLAIN-analysis.md](EXPLAIN-analysis.md) |
| `server` | `ServerMode.Run()` | 以 HTTP REST API 暴露上述两种模式，支持自动化调用 | [EXPLAIN-server.md](EXPLAIN-server.md) |

子命令路由位于 `Application.Run()`，最终通过 `IMode.Run()` 启动对应模式。

---

## 快速入口

- **Snapshot 模式全流程**（调用链、SDK 数据流、多帧共存、输出目录）→ [EXPLAIN-snapshot.md](EXPLAIN-snapshot.md)
- **Analysis 模式全流程**（AnalysisPipeline、AnalysisTarget、DataModel、报告结构）→ [EXPLAIN-analysis.md](EXPLAIN-analysis.md)
- **Server 模式全流程**（Job 系统、状态机、各 Runner 阶段、Request/Response 格式）→ [EXPLAIN-server.md](EXPLAIN-server.md)
- **SDK 架构 / DLL 依赖 / 核心 API / 数据结构** → 本文 §二 ~ §四

---

## 一、代码与上下文的差异说明

| 方面 | 代码现状（2026-04-14） | 备注 |
|------|----------------------|------|
| Snapshot 模式 | **稳定**，已多次验证 | 已知：captureId 通过扫描目录确认，非 API 直接返回 |
| Analysis 模式 | **稳定**，有增量执行 | PassMode 体系（StatsOnly/AnalysisOnly）是早期设计，已被 AnalysisTarget 部分替代但未移除 |
| Server 模式 | **新增（2026-04-14）**，构建通过，未经实际设备测试 | CaptureJobRunner 的 `CaptureExecutionService` 初始化方式与 SnapshotCaptureMode 中参数签名需实测验证 |
| completedTargets | **已实现**，`AnalysisPipeline.RunAnalysis()` 新增参数 | Server AnalysisJobRunner 使用；CLI 直接调用不受影响（默认 `None`） |
| PassMode vs AnalysisTarget | **并存**，有重叠 | PassMode 是粗粒度（3档），AnalysisTarget 是细粒度（10标志），建议未来统一为后者 |

---

## 二、SDK 架构与依赖关系

### 2.1 为什么必须依赖 SDPClientFramework

SDPCLI 不能直接调用 `SDPCoreWrapper.dll`，必须引用 `SDPClientFramework.dll`，原因是插件系统：

- **QGLPlugin** 在构造时订阅 `SdpApp.EventsManager.ConnectionEvents`，并使用 `SdpApp.ConnectionManager`——这些都来自 SDPClientFramework
- **SDPCLI** 通过 `ProcessorPluginMgr.Get().LoadPlugins()` 加载 QGLPlugin，通过 `GetPlugin("SDP::QGLPluginProcessor")` 获取实例
- 如果不引用 SDPClientFramework，就无法使用 `ProcessorPluginMgr`，也就无法加载 QGLPlugin

这是标准的**插件-宿主（Plugin-Host）架构**：

```
SDPCLI.exe
  ├── SDPClientFramework.dll（宿主框架）
  │     ├── SdpApp.EventsManager      ← QGLPlugin 订阅事件的来源
  │     ├── SdpApp.ConnectionManager  ← QGLPlugin 访问连接状态的来源
  │     └── ProcessorPluginMgr        ← SDPCLI 用来加载 QGLPlugin
  ├── SDPCoreWrapper.dll
  │     ├── Client, DeviceManager, CaptureManager
  │     └── P/Invoke → SDPCore.dll (Native)
  └── QGLPlugin.dll
        ├── 编译时依赖 SDPClientFramework（引用其 public API）
        └── 运行时被 ProcessorPluginMgr 动态加载
```

### 2.2 为什么不能访问 VkSnapshotModel

`QGLPlugin.VkSnapshotModel` 是 `internal` 类——在 QGLPlugin.dll 的程序集边界内不可见。  
外部项目（包括 SDPCLI）只能访问 QGLPlugin 的 **public 成员**。

```csharp
// QGLPlugin.cs（参考源码，不可引用）
internal static VkSnapshotModel VkSnapshotModel { get; set; }  // ❌ 外部不可见
public static BinaryDataPair SnapshotDsbBuffer { get; }         // ✅ 外部可访问
```

解决方案：在 Capture 模式的 replay 完成后，通过 `ProcessorPlugin.GetLocalBuffer(bufferID=3, captureID)` 获取 SnapshotDsbBuffer 的等效二进制数据，再在 SDPCLI 中解析并写入自定义表。

### 2.3 DLL 依赖一览

| DLL | 依赖方式 | 功能 |
|-----|---------|------|
| `SDPClientFramework.dll` | csproj 直接引用 | 插件宿主框架，事件系统，连接管理 |
| `QGLPlugin.dll` | csproj 引用 + ProcessorPluginMgr 动态加载 | Vulkan 数据处理，SnapshotDsbBuffer |
| `SDPCoreWrapper.dll` | csproj 直接引用 | C++/CLI 包装，设备管理，抓帧控制 |
| `SDPCore.dll` | 运行时 P/Invoke | Native C++ 核心库 |

---

## 三、关键 API 参考

### 3.1 设备连接与 Capture（Snapshot 模式）

```csharp
// Client & Session 初始化
Client client = new Client();
SessionSettings settings = new SessionSettings();
settings.SessionDirectoryRootPath = outputDir;
client.Init(settings);

// 设备连接
DeviceManager deviceManager = DeviceManager.Get();
Device device = deviceManager.GetConnectedDevice();
device.Connect(timeout, port);

// Capture 触发
CaptureManager captureManager = CaptureManager.Get();
uint captureId = captureManager.CreateCapture(CaptureType.Snapshot);
Capture capture = captureManager.GetCapture(captureId);
capture.Start();

// replay 完成后获取 DsbBuffer（QGLPlugin 的 public API）
SDPProcessorPlugin plugin = SdpApp.ConnectionManager
    .GetProcessorPlugin("SDP::QGLPluginProcessor");
BinaryDataPair buffer = plugin.GetLocalBuffer(
    SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA,
    bufferID: 3,   // SnapshotDsbBuffer
    captureID
);
```

### 3.2 数据库访问（Analysis 模式）

```csharp
// DataModel 读取（通过 SDPClientFramework）
DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
Model model = dataModel.GetModel("VulkanSnapshot");
ModelObject apiObj = model.GetModelObject("VulkanSnapshotApis");
ModelObjectDataList data = apiObj.GetData(new StringList { "captureID", "1" });

// 二进制 Buffer 解析（SnapshotDsbBuffer → DescSetBindings）
ModelObject bufObj = model.GetModelObject("VulkanSnapshotByteBuffers");
ModelObjectData bufData = bufObj.GetData(...)[0];
BinaryDataPair bdp = bufData.GetValuePtrBinaryDataPair("dataPair");
DescSetBindings.DescBindings binding = Marshal.PtrToStructure<...>(bdp.data);
```

---

## 四、数据结构参考

### 4.1 DescSetBindings.DescBindings（14 字段）

来源：`dll/project/QGLPlugin/DescSetBindings.cs`，SDPCLI 在 Capture 时解析后写入自定义表。

```csharp
public struct DescBindings
{
    public uint  captureID;        // Capture ID
    public uint  apiID;            // VulkanSnapshotApis.resourceID（DrawCall ID）
    public uint  descriptorSetID;  // Descriptor Set ID
    public uint  slotNum;          // Binding slot
    public ulong samplerID;        // vkCreateSampler handle
    public ulong imageViewID;      // vkCreateImageView handle → 关联 Texture
    public uint  imageLayout;      // VkImageLayout enum
    public ulong texBufferview;    // Texel buffer view
    public ulong bufferID;         // vkCreateBuffer handle
    public ulong offset;           // Buffer offset
    public ulong range;            // Buffer range
    public ulong accelStructID;    // Acceleration structure（ray tracing）
    public ulong tensorID;         // Tensor ID（AI/ML）
    public ulong tensorViewID;     // Tensor view
}
```

### 4.2 自定义表 Schema

SDPCLI 在 Capture 时写入，供 Analysis 模式查询 DrawCall→Texture/Buffer 绑定关系：

```sql
-- DrawCallDescriptorBindings（每个 DrawCall 的 Descriptor 绑定）
CREATE TABLE DrawCallDescriptorBindings (
    captureID       INTEGER,
    apiID           INTEGER,   -- 对应 VulkanSnapshotApis.resourceID
    descriptorSetID INTEGER,
    slotNum         INTEGER,
    samplerID       INTEGER,
    imageViewID     INTEGER,   -- 关联 VulkanSnapshotImageViews.resourceID
    imageLayout     INTEGER,
    texBufferview   INTEGER,
    bufferID        INTEGER,
    offset          INTEGER,
    range           INTEGER,
    accelStructID   INTEGER,
    tensorID        INTEGER,
    tensorViewID    INTEGER
);

-- DrawCallPipelines（DrawCall → Pipeline 关联）
CREATE TABLE DrawCallPipelines (
    captureID  INTEGER,
    apiID      INTEGER,
    pipelineID INTEGER    -- 关联 VulkanSnapshotGraphicsPipelines.resourceID
);
```

### 4.3 sdp.db 核心表（SDK 写入 vs SDPCLI 写入）

| 表 | 写入方 | 写入时机 | CaptureID 隔离 |
|----|-------|---------|----------------|
| `VulkanSnapshotApis` | SDK（ImportCapture） | Replay 完成 | `captureID` 列 |
| `VulkanSnapshotShaderStages` | SDK | Replay | `captureID` 列 |
| `VulkanSnapshotByteBuffers` | SDK | Replay | `captureID` 列 |
| `VulkanSnapshotImageViews` | SDK | Replay | `captureID` 列 |
| `VulkanSnapshotTextures` | SDK | Replay | `captureID` 列 |
| `DrawCallParameters` | SDPCLI（CsvToDbService） | CSV import | `CaptureID` 列 |
| `DrawCallBindings` | SDPCLI | CSV import | `CaptureID` 列 |
| `DrawCallDescriptorBindings` | SDPCLI | DsbBuffer 解析 | `captureID` 列 |
| `DrawCallPipelines` | SDPCLI | DsbBuffer 解析 | `captureID` 列 |
