# Snapdragon Profiler Command Line Tool

**无头模式的 Snapdragon Profiler CLI 工具**，实现 GUI 的核心功能，支持：
- **Capture 模式**：连接设备，截取 Vulkan 帧，导出 .sdp 文件
- **Analysis 模式**：离线分析 .sdp 文件，查询 DrawCall、Shader、Texture 等数据
- **Texture Extraction 模式**：从 .sdp 文件中提取纹理并导出为 PNG
- **Shader Extraction 模式**：从 .sdp 文件中提取 GLSL/SPIR-V Shader 代码

---

## 快速开始

### 前置要求
1. 安装 [Snapdragon Profiler](https://www.qualcomm.com/developer/software/snapdragon-profiler) 到默认路径：
   ```
   C:\Program Files\Qualcomm\Snapdragon Profiler
   ```

2. .NET SDK（用于编译）或 .NET Framework 4.7.2+（内置于 Windows 10+，无需单独安装）

3. USB 连接 Android 设备（仅 Capture 模式需要），启用 USB 调试

### 编译

```powershell
cd D:\snapdragon
dotnet build SDPCLI
# 输出：SDPCLI\bin\Debug\net472\SDPCLI.exe
```

### 运行模式

#### **模式 1：snapshot（设备抓帧）**
连接手机，启动 app，截取 Vulkan 帧：

```powershell
cd SDPCLI\bin\Debug\net472

# 交互模式（进入菜单）
SDPCLI.exe

# 直接抓帧（指定包名\活动）
SDPCLI.exe snapshot com.your.app\ActivityName -c

# 只启动不抓帧
SDPCLI.exe snapshot com.your.app\ActivityName -l
```

或者通过项目根目录的批处理脚本：
```batch
SDPCLI.bat
```

配置文件 `config.ini`（可省略，交互模式会提示输入）：
```ini
PackageName=com.your.app
ActivityName=.MainActivity
RenderingAPI=16   # 16=Vulkan
CaptureType=4     # 4=Snapshot
AutoStartCapture=false
```

**输出位置**：`<ProjectDir>/sdp/<timestamp>/`（默认 `<WorkingDirectory>/project/sdp/`）

#### **模式 2：analysis（离线分析）**
分析已有的 .sdp 文件，生成 DrawCall 报告：

```powershell
# 分析所有 snapshot（路径相对于 SdpDir）
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp

# 只分析 snapshot_3
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp -s 3

# 只重新生成 label（增量，不重新提取 shader/texture）
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp -s 3 -t label
```

**输出位置**：`<ProjectDir>/analysis/<sdp_basename>/snapshot_N/`

#### **模式 3：extract-texture（纹理提取）**
从 .sdp 文件中提取纹理，保存为 PNG：

```powershell
SDPCLI.exe extract-texture 2026-04-11T09-50-42.sdp -resource-id 23352

# 指定 captureID 和输出路径
SDPCLI.exe extract-texture 2026-04-11T09-50-42.sdp -resource-id 23352 -capture-id 3 -output out\texture.png
```

#### **模式 4：extract-shader（Shader 提取）**
从 .sdp 文件中提取 Shader 代码（SPIR-V/GLSL）：

```powershell
SDPCLI.exe extract-shader 2026-04-11T09-50-42.sdp -pipeline-id 42

# 指定输出目录
SDPCLI.exe extract-shader 2026-04-11T09-50-42.sdp -pipeline-id 42 -output shaders\
```

详细参数说明见 `CLI_PARAMETERS.md`。

---

## 项目目标

**用 CLI 替代 GUI** —— 实现 Snapdragon Profiler GUI 的核心功能：
1. **Capture**：自动连接设备、启动 app、截取帧、replay、导出 .sdp
2. **Analysis**：读取 .sdp、查询数据库、解析二进制 buffer、导出分析结果（DrawCall 报告）
3. **Texture Extraction**：从 .sdp 的 SQLite 数据库中提取纹理数据，转换为 PNG
4. **Shader Extraction**：从 .sdp 中提取 GLSL/SPIR-V Shader 代码
5. **自动化**：批量处理多个 .sdp 文件，生成 CSV 报告

## 架构说明

### 设计原则
- **无头模式**：无 GUI，纯命令行，适合 CI/CD 集成
- **四模式架构**：Capture（在线）、Analysis（离线）、Texture Extraction、Shader Extraction 独立运行
- **直接引用预编译 DLL**：使用 `SDPClientFramework.dll`、`QGLPlugin.dll` 等，**不修改不重新编译**
- **API 完全兼容**：通过公开的 `public` API 调用，如 `ProcessorPlugin.GetLocalBuffer()`
- **参考 GUI 源码**：`dll/project/*` 仅作参考，理解 API 用法和数据结构

### 关键约束
- **必须依赖 SDPClientFramework**：要使用 QGLPlugin，必须通过 `ProcessorPluginMgr` 加载
- **只能访问 public 成员**：`internal` 类（如 `VkSnapshotModel`）在外部项目不可见
- **事件驱动架构**：QGLPlugin 通过订阅 `SdpApp.EventsManager.ConnectionEvents` 接收数据处理通知
- **DescriptorSet 数据不持久化**：GUI 的 `VkSnapshotModel.PopulateDescSets()` 只填充内存字典，不写入 .sdp
- **解决方案**：SDPCLI 在 Capture 模式时自己解析并保存 DescriptorSet 绑定到自定义表

### DLL 架构与依赖关系

#### 源码目录结构
```
dll/project/
├── SDPClientFramework/    → SDPClientFramework.dll 源码（GUI框架）
├── SDPCoreWrapper/         → SDPCoreWrapper.dll 源码（C++/CLI包装）
└── QGLPlugin/              → QGLPlugin.dll 源码（Vulkan插件）
```

#### 依赖关系图
```
┌─────────────────────────────────────────────────────┐
│                  Snapdragon Profiler GUI (exe)       │  ← 官方 GUI 应用
└────────────────┬────────────────────────────────────┘
                 │ 依赖
                 ↓
┌─────────────────────────────────────────────────────┐
│           SDPClientFramework.dll                     │  ← 插件宿主框架
│  - SdpApp.EventsManager（事件系统）                 │
│  - SdpApp.ConnectionManager（连接管理）             │
│  - ProcessorPluginMgr（插件管理器）                 │
└────────────────┬────────────────────────────────────┘
                 │ 依赖                   ↑
                 ↓                        │ 编译时依赖
┌─────────────────────────────────────────────────────┐
│           SDPCoreWrapper.dll                         │  → QGLPlugin.dll ←┐
│  - Client, DeviceManager, CaptureManager            │    （订阅事件，访问 SdpApp）
└────────────────┬────────────────────────────────────┘    │
                 │ P/Invoke 调用                            │ 运行时加载
                 ↓                                          │
┌─────────────────────────────────────────────────────┐    │
│           SDPCore.dll (Native C++)                   │    │
└─────────────────────────────────────────────────────┘    │
                                                            │
        ProcessorPluginMgr.LoadPlugin("QGLPlugin.dll") ────┘
                      （运行时动态加载）
                      
        ┌─────────────────────────────────┐
        │  VulkanProcessor.dll (Android)   │  ← 设备端数据采集
        └─────────────────────────────────┘
```

**双向关系**：
- **编译时**：QGLPlugin 引用 SDPClientFramework.dll（依赖 `SdpApp` 基础设施）
- **运行时**：SDPClientFramework 通过 `ProcessorPluginMgr` 动态加载 QGLPlugin.dll（插件系统）

#### SDPCLI 架构（我们的项目）
```
┌─────────────────────────────────────────────────────┐
│              SDPCLI.exe (我们的CLI工具)              │  ← 替代 GUI
│  - 交互模式、Capture、Analysis                      │
│  - 只能访问 public API                              │
└────────────────┬────────────────────────────────────┘
                 │ 依赖（**必须**）
                 ↓
┌─────────────────────────────────────────────────────┐
│           SDPClientFramework.dll                     │  ← 插件宿主框架
│  - ProcessorPluginMgr（加载 QGLPlugin）             │
│  - ConnectionManager、DataModel 等                  │
└────────────────┬────────────────────────────────────┘
                 │ 依赖
                 ↓
┌─────────────────────────────────────────────────────┐
│           SDPCoreWrapper.dll                         │  ← C++/CLI 包装
│  - Client, DeviceManager, CaptureManager            │
└────────────────┬────────────────────────────────────┘
                 │
                 ↓
             SDPCore.dll

             + 运行时加载（通过 ProcessorPluginMgr）
                      ↓
        ┌─────────────────────────────────┐
        │      QGLPlugin.dll               │  ← Vulkan 数据处理插件
        │  - 订阅 ConnectionEvents         │
        │  - 只能访问 public 成员          │
        └─────────────────────────────────┘
```

**为什么必须依赖 SDPClientFramework？**
1. ✅ **QGLPlugin 依赖 SDPClientFramework**：
   - QGLPlugin 构造函数订阅 `SdpApp.EventsManager.ConnectionEvents`
   - QGLPlugin 使用 `SdpApp.ConnectionManager`
2. ✅ **SDPCLI 需要加载 QGLPlugin**：
   - 使用 `ProcessorPluginMgr.Get().LoadPlugins()` 加载插件
   - 使用 `ProcessorPluginMgr.Get().GetPlugin("SDP::QGLPluginProcessor")` 获取插件实例
   - 调用 `plugin.GetLocalBuffer()` 获取 SnapshotDsbBuffer
3. ✅ **无法绕过**：
   - 如果不引用 SDPClientFramework，无法加载 QGLPlugin
   - 如果不加载 QGLPlugin，无法处理 Vulkan Snapshot 数据

**关键差异**：
- **GUI 路径**：GUI.exe → SDPClientFramework.dll → SDPCoreWrapper.dll
- **CLI 路径**：SDPCLI.exe → SDPClientFramework.dll → SDPCoreWrapper.dll（**相同！**）
- **插件架构**：
  - **QGLPlugin 依赖 SDPClientFramework**：引用 `SdpApp.EventsManager`、`SdpApp.ConnectionManager` 等
  - **SDPClientFramework 加载 QGLPlugin**：运行时通过 `ProcessorPluginMgr` 动态加载
  - **SDPCLI 也依赖 SDPClientFramework**：必须引用才能使用 `ProcessorPluginMgr` 加载 QGLPlugin
- **访问限制**（重要）：
  - **QGLPlugin 是独立的 solution**：与 SDPClientFramework 分离编译
  - **运行时动态加载**：通过 `ProcessorPluginMgr.Get().GetPlugin()` 加载
  - **只能访问 public API**：外部项目（如 SDPCLI）只能访问 QGLPlugin 的 `public` 成员
  - **internal 成员不可见**：`VkSnapshotModel`（internal class）对外部项目不可见

**之前的误解**：
- ❌ **错误**："SDPCLI 可以跳过 Framework 层，直接使用 SDPCoreWrapper"
- ✅ **正确**：SDPCLI **必须依赖** SDPClientFramework.dll 才能使用 QGLPlugin

### 依赖的 DLL（预编译，直接引用）
| DLL | 功能 | SDPCLI 引用 |
|-----|------|-------------|
| `SDPClientFramework.dll` | 插件宿主，`ProcessorPluginMgr`、`ConnectionManager`、`DataModel` | ✅ **必须**（SDPCLI.csproj） |
| `QGLPlugin.dll` | Vulkan 插件，`SnapshotDsbBuffer`（public）、`VkSnapshotModel`（internal） | ✅ **必须**（通过 ProcessorPluginMgr 加载） |
| `SDPCoreWrapper.dll` | C++/CLI 包装，`Client`、`DeviceManager`、`CaptureManager` | ✅ **必须**（SDPCLI.csproj） |
| `SDPCore.dll` | Native C++ 核心库 | ✅ **必须**（运行时依赖） |

**依赖链**：
```
SDPCLI.exe
  ├── SDPClientFramework.dll（引用）
  │     ├── ProcessorPluginMgr.LoadPlugins() → QGLPlugin.dll
  │     └── ProcessorPluginMgr.GetPlugin() → SDPProcessorPlugin
  ├── SDPCoreWrapper.dll（引用）
  └── QGLPlugin.dll（引用，但主要通过 ProcessorPluginMgr 使用）
```

### 核心 API 使用（Capture 模式）

基于 `SDPClientFramework` 和 `SDPCoreWrapper`：

#### 1. Client & Session（会话管理）
```csharp
Client client = new Client();
SessionSettings settings = new SessionSettings();
settings.SessionDirectoryRootPath = "./output";  // 输出目录，可在 config.ini 配置
client.Init(settings);
```

#### 2. DeviceManager（设备管理）
```csharp
DeviceManager deviceManager = DeviceManager.Get();
Device device = deviceManager.GetConnectedDevice();
device.Connect(timeout, port);
```

#### 3. CaptureManager（捕获管理）
```csharp
CaptureManager captureManager = CaptureManager.Get();
uint captureId = captureManager.CreateCapture(CaptureType.Snapshot);
Capture capture = captureManager.GetCapture(captureId);
capture.Start();  // 触发截图
```

#### 4. ProcessorPlugin（数据处理）
```csharp
// Capture 完成后，replay 会填充 buffer
SDPProcessorPlugin plugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
BinaryDataPair buffer = plugin.GetLocalBuffer(
    SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA, 
    bufferID: 3,  // SnapshotDsbBuffer
    captureID
);
```

### 核心 API 使用（Analysis 模式）

#### 1. DataModel（数据库访问）
```csharp
DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
Model model = dataModel.GetModel("VulkanSnapshot");
ModelObject apiObj = model.GetModelObject("VulkanSnapshotApis");
ModelObjectDataList data = apiObj.GetData(new StringList { "captureID", "1" });
```

#### 2. 二进制 Buffer 解析
```csharp
// 读取 VulkanSnapshotByteBuffers 表（resourceID=3 是 SnapshotDsbBuffer）
ModelObject bufObj = model.GetModelObject("VulkanSnapshotByteBuffers");
ModelObjectData bufData = bufObj.GetData(...)[0];
BinaryDataPair bdp = bufData.GetValuePtrBinaryDataPair("dataPair");

// 解析 DescSetBindings.DescBindings 结构（14字段）
DescSetBindings.DescBindings binding = Marshal.PtrToStructure<...>(bdp.data);
```

---

## Capture 模式流程

### 单次 Capture 流程

完整的设备连接和截图流程：

```
Application.Run()
 ├── Phase 1: 初始化
 │    ├── SdpApp.Init(ConsolePlatform)
 │    ├── new QGLPlugin()（订阅 ConnectionEvents）
 │    └── SDPClient.Initialize(SessionSettings, CliClientDelegate)
 │
 ├── Phase 2: 设备连接
 │    ├── DeviceConnectionService.CheckAndInstallAPKs()  (adb install)
 │    ├── DeviceConnectionService.Connect()              → Device
 │    └── AppLaunchService.SelectAndLaunch(packageName)
 │
 ├── Phase 3: Capture 循环（每次 ENTER 一帧）
 │    ├── CaptureExecutionService.StartCapture()         → Capture.Start()
 │    ├── WaitOne(_captureCompleteEvent, 30s)
 │    ├── WaitForDataProcessed()                         → OnDataProcessed 稳定 2s
 │    ├── 扫描 snapshot_* 目录 → 确认 SDK 分配的真实 captureId
 │    ├── ReplayAndGetBuffers(captureId)                 → ImportCapture → DsbBuffer
 │    ├── ExportDrawCallData()                           → CSVs + DB import
 │    └── DataExportService.ExportData()                 → screenshot
 │
 └── Phase 4: Session 结束（ESC）
      └── SessionArchiveService.CreateSessionArchive()  → .sdp ZIP
```

### 多次 Capture 数据流（核心）

同一个 session 可以按多次 ENTER 连续捕获多帧，每次产生一个独立的 `captureId`：

```
第1次 ENTER → captureId=2 → snapshot_2/
第2次 ENTER → captureId=3 → snapshot_3/
第3次 ENTER → captureId=4 → snapshot_4/
```

**每次 Capture 的数据写入路径**：

```
ReplayAndGetBuffers(captureId)
 └── QGLPluginService.ImportCapture(captureId, sdp.db)
     └── 写入 VulkanSnapshot* 表（SDK 原生，按 captureId 积累）：
         VulkanSnapshotGraphicsPipelines  captureID=N
         VulkanSnapshotShaderStages       captureID=N
         VulkanSnapshotByteBuffers        captureID=N
         VulkanSnapshotImageViews         captureID=N
         VulkanSnapshotTextures           captureID=N

ExportDrawCallData(captureId)
 ├── VulkanSnapshotModel.LoadSnapshot()  → 解析 ApiBuffer + DsbBuffer
 ├── model.Export*ToCSV() ×7             → snapshot_{captureId}/*.csv
 │   每张 CSV 第二列均为 CaptureID：
 │   DrawCallParameters.csv     DrawCallApiID, CaptureID, ...
 │   DrawCallBindings.csv        DrawCallApiID, CaptureID, PipelineID, ImageViewID, ...
 │   DrawCallRenderTargets.csv   DrawCallApiID, CaptureID, ...
 │   DrawCallVertexBuffers.csv   DrawCallApiID, CaptureID, ...
 │   DrawCallIndexBuffers.csv    DrawCallApiID, CaptureID, ...
 │   DrawCallMetrics.csv         DrawCallApiID, CaptureID, ...
 │   PipelineVertexInput*.csv    PipelineID,    CaptureID, ...
 └── CsvToDbService.ImportAllCsvs(captureSubDir, sdp.db)
     ├── CREATE TABLE IF NOT EXISTS DrawCallParameters (...)   ← 不清空历史
     ├── DELETE FROM DrawCallParameters WHERE CaptureID=N      ← 幂等，仅删本次
     └── INSERT INTO DrawCallParameters ... (新行带 CaptureID=N)
```

**结果**：多次 Capture 后 `sdp.db` 中并存多份数据：

```sql
SELECT CaptureID, COUNT(*) FROM DrawCallParameters GROUP BY CaptureID;
-- captureID=2  174 rows
-- captureID=3  182 rows
-- captureID=4  176 rows
```

### 关键：哪些表是积累的，哪些是 per-capture 管理的

| 表 | 写入方 | 多 Capture 行为 | CaptureID 过滤 |
|----|-------|----------------|----------------|
| `VulkanSnapshotShaderStages` | SDK（ImportCapture）| **积累** — 不清空 | ✅ captureID 列 |
| `VulkanSnapshotByteBuffers` | SDK（ImportCapture）| **积累** | ✅ captureID 列 |
| `VulkanSnapshotTextures` | SDK（ImportCapture）| **积累** | ✅ captureID 列 |
| `VulkanSnapshotImageViews` | SDK（ImportCapture）| **积累** | ✅ captureID 列 |
| `VulkanSnapshotGraphicsPipelines` | SDK（ImportCapture）| **积累** | ✅ captureID 列 |
| `DrawCallParameters` | CsvToDbService | **per-capture**（DELETE+INSERT）| ✅ CaptureID 列 |
| `DrawCallBindings` | CsvToDbService | **per-capture** | ✅ CaptureID 列 |
| `DrawCallRenderTargets` | CsvToDbService | **per-capture** | ✅ CaptureID 列（动态检测）|
| `DrawCallVertexBuffers` | CsvToDbService | **per-capture** | ✅ CaptureID 列 |
| `DrawCallMetrics` | CsvToDbService | **per-capture** | ✅ CaptureID 列 |

> **历史遗留问题**（已修复）：旧版 CsvToDbService 使用 `DROP TABLE` 导致每次 Capture 覆盖全表，
> 多次 Capture 后 DB 只剩最新一次的 DrawCall 数据。已改为 `CREATE TABLE IF NOT EXISTS` +
> `DELETE WHERE CaptureID=x`，现在多 Capture 可以安全并存。

---

## Analysis 模式流程

### 数据库读取规则（多 Capture）

Analysis 模式打开 `.sdp` 文件时，`sdp.db` 中可能存在来自多次 Capture 的数据。
所有查询均以 `captureId` 为过滤条件，确保只读取所选 Capture 的数据：

```
AnalysisMode.Run()
 ├── SelectCaptureIdFromSdp()      ← 扫描 ZIP 内 snapshot_* 目录，列出可选 captureId
 │   示例：snapshot_2, snapshot_3, snapshot_4 → 用户选择其中一个
 └── AnalysisPipeline.RunAnalysis(sdpPath, outputDir, captureId=2)
     ├── SdpFileService.FindDatabasePath()   ← 从 ZIP 解压 sdp.db 到临时目录
     ├── DatabaseQueryService.OpenDatabase()
     │
     ├── [Step 1] GetDrawCallIds(captureId=2)
     │   SELECT DrawCallApiID FROM DrawCallParameters
     │   WHERE CaptureID=2 [AND CmdBufferIdx=N]   ← 精确隔离当前 Capture
     │   ORDER BY rowid
     │
     │   DrawCallQueryService.GetDrawCallInfo(captureId=2, apiId)
     │   ├── DrawCallParameters  WHERE DrawCallApiID=x            (apiId 在各 capture 间不重叠)
     │   ├── DrawCallBindings    WHERE DrawCallApiID=x AND CaptureID=2  → PipelineID, ImageViewID
     │   ├── DrawCallRenderTargets WHERE DrawCallApiID=x [AND CaptureID=2] (动态检测列存在)
     │   ├── DrawCallVertexBuffers WHERE DrawCallApiID=x
     │   ├── VulkanSnapshotShaderStages WHERE captureID=2 AND pipelineID=x  (积累表，始终有效)
     │   ├── VulkanSnapshotImageViews   WHERE captureID=2 AND resourceID=x
     │   └── VulkanSnapshotTextures     WHERE captureID=2 AND resourceID=x
     │
     ├── [Step 1.5] ShaderExtractor(dbPath, captureId=2)
     │   VulkanSnapshotShaderStages WHERE captureID=2 AND pipelineID=x → SPIR-V → HLSL
     │   （fallback：若 captureId=2 无数据，扫描其他 captureId）
     │
     │   TextureExtractor(dbPath, captureId=2)
     │   VulkanSnapshotTextures    WHERE captureID=2 AND resourceID=x → metadata
     │   VulkanSnapshotByteBuffers WHERE captureID=2 AND resourceID=x → raw bytes → PNG
     │
     ├── [Step 2] DrawCallLabelService.Label()  [+ LlmApiWrapper 可选]
     │
     ├── [Step 3] MetricsCsvService.LoadMetricsFromSession(snapshot_2/)
     │   ← 读取 snapshot_2/DrawCallMetrics.csv（直接从 ZIP 解压的 per-capture 文件）
     │   ← 读取 snapshot_2/DrawCallParameters.csv（用于对齐 apiId）
     │   join by DrawCallApiID
     │
     └── [Step 4] ReportGenerationService.GenerateSummaryReport() → .md
```

### 旧格式 SDP 兼容

旧版 SDP（CaptureID 列缺失）通过以下方式兼容：
- `DatabaseQueryService`：`PRAGMA table_info(DrawCallParameters)` 检测 `CaptureID` 列；若不存在则不加 WHERE 过滤（单 capture 行为）
- `DrawCallQueryService.ColumnExists()`：动态检测 `DrawCallRenderTargets.CaptureID`；不存在时只用 `DrawCallApiID` 过滤
- `CsvToDbService.CreateOrExtendTable()`：表已存在但缺少列时，执行 `ALTER TABLE ADD COLUMN` 补齐
- `TextureExtractor`：先查 `captureID=x AND resourceID=y`，找不到则 fallback 到只用 `resourceID=y`

---

## 目录结构

```
snapdragon/
├── SDPCLI/
│   ├── README.md                    # 本文件
│   ├── SDPCLI.sln                   # Visual Studio 解决方案
│   ├── SDPCLI.csproj                # 项目文件（.NET Framework 4.7.2）
│   ├── config.ini                   # Capture 模式配置
│   │
│   ├── source/                      # 源代码
│   │   ├── Main.cs                  # 入口：DLL 路径、插件加载、模式分发
│   │   ├── Application.cs           # 应用初始化与生命周期
│   │   ├── Config.cs                # config.ini 解析
│   │   ├── SDPClient.cs             # Client/Session 初始化
│   │   ├── CliClientDelegate.cs     # CLI 客户端事件委托
│   │   ├── CliDeviceDelegate.cs     # CLI 设备事件委托

│   │   ├── ConsoleLogSink.cs        # 控制台日志输出
│   │   ├── ConsolePlatform.cs       # 控制台平台适配
│   │   ├── Utility.cs               # 辅助工具
│   │   │
│   │   ├── Analysis/                # 分析管道
│   │   │   └── AnalysisPipeline.cs  # 离线分析流程编排
│   │   │
│   │   ├── Logging/                 # 日志系统
│   │   │   ├── AppLogger.cs         # 应用级日志器
│   │   │   ├── AppLogLevel.cs       # 日志级别定义
│   │   │   └── ContextLogger.cs     # 带上下文的日志器
│   │   │
│   │   ├── Models/                  # 数据模型
│   │   │   ├── DrawCallModels.cs    # DrawCall 相关数据结构
│   │   │   └── VulkanSnapshotModel.cs  # Vulkan Snapshot 数据模型
│   │   │
│   │   ├── Modes/                   # 运行模式
│   │   │   ├── IMode.cs             # 模式接口
│   │   │   ├── ILogger.cs           # 日志接口
│   │   │   ├── ConsoleLogger.cs     # 控制台日志实现
│   │   │   ├── SnapshotCaptureMode.cs     # Capture 模式入口
│   │   │   ├── AnalysisMode.cs      # Analysis 模式入口
│   │   │   ├── DrawCallAnalysisMode.cs    # DrawCall 分析子模式
│   │   │   ├── TextureExtractionMode.cs   # Texture 提取模式
│   │   │   └── ShaderExtractionMode.cs    # Shader 提取模式
│   │   │
│   │   ├── Services/                # 业务服务层
│   │   │   ├── Capture/             # Capture 相关服务
│   │   │   │   ├── DeviceConnectionService.cs   # 设备连接
│   │   │   │   ├── AppLaunchService.cs          # App 启动
│   │   │   │   ├── CaptureExecutionService.cs   # Capture 执行
│   │   │   │   ├── QGLPluginService.cs          # QGLPlugin 管理
│   │   │   │   ├── DataExportService.cs         # 数据导出
│   │   │   │   ├── CsvToDbService.cs            # CSV 转数据库
│   │   │   │   └── SessionArchiveService.cs     # Session 归档
│   │   │   └── Analysis/            # Analysis 相关服务
│   │   │       ├── DatabaseQueryService.cs      # 数据库查询
│   │   │       ├── DrawCallQueryService.cs      # DrawCall 查询
│   │   │       ├── DrawCallAnalysisService.cs   # DrawCall 分析
│   │   │       ├── DrawCallLabelService.cs      # DrawCall 标签
│   │   │       ├── MetricsCsvService.cs         # 指标 CSV 导出
│   │   │       ├── ReportGenerationService.cs   # 报告生成
│   │   │       └── SdpFileService.cs            # .sdp 文件服务
│   │   │
│   │   └── Tools/                   # 工具类
│   │       ├── TextureExtractor.cs  # 纹理提取
│   │       ├── ShaderExtractor.cs   # Shader 提取
│   │       └── LlmApiWrapper.cs     # LLM API 封装
│   │
│   ├── bin/Debug/net472/             # 编译输出
│   │   ├── SDPCLI.exe
│   │   ├── SDPClientFramework.dll   # 引用的 DLL
│   │   ├── QGLPlugin.dll
│   │   ├── SDPCoreWrapper.dll
│   │   ├── SDPCore.dll
│   │   ├── plugins/                 # 处理器插件（从安装目录复制）
│   │   │   └── processor/*.dll
│   │   └── .log/                    # 运行日志（相对于 exe，自动创建）
│   │       ├── consolelog.txt       # 当前日志（追加写入，10MB 后轮转）
│   │       └── consolelog.1.txt     # 轮转备份
│   │
│   ├── output/                      # Capture 输出目录（可在 config.ini 配置）
│   │   └── session_YYYYMMDD_HHMMSS/
│   │       └── 1.sdp                # 导出的 SQLite DB + 二进制数据
│   │
│   └── sdp/                         # 示例 .sdp 文件（用于测试 Analysis）
│       ├── 3-13-xiaomi12.sdp
│       └── 3-13-xiaomi12.csv
│
└── dll/project/                     # 参考源码（只读，不修改）
    ├── SDPClientFramework/
    ├── QGLPlugin/
    └── SDPCoreWrapper/
```

---

## 关键文件说明

### `source/Main.cs` - 程序入口
DLL 路径设置、插件加载、运行模式分发（基于 `IMode` 接口）。

### `source/Application.cs` - 应用初始化
应用生命周期管理，协调各 Mode 和 Service 的初始化。

### `source/Modes/` - 运行模式
每个模式实现 `IMode` 接口，独立编排对应业务流程：
- **`SnapshotCaptureMode.cs`**：Capture 流程（设备连接 → 截图 → 导出）
- **`AnalysisMode.cs`** / **`DrawCallAnalysisMode.cs`**：离线分析、DrawCall 查询
- **`TextureExtractionMode.cs`**：从 .sdp 提取 PNG 纹理
- **`ShaderExtractionMode.cs`**：从 .sdp 提取 GLSL/SPIR-V

### `source/Services/` - 业务服务层
职责分离的服务类，供 Mode 层调用：

| 服务 | 职责 |
|------|------|
| `Capture/DeviceConnectionService.cs` | 设备发现与连接 |
| `Capture/AppLaunchService.cs` | App 启动与进程监听 |
| `Capture/CaptureExecutionService.cs` | Capture 创建与触发 |
| `Capture/QGLPluginService.cs` | QGLPlugin 加载与 buffer 获取 |
| `Capture/DataExportService.cs` | .sdp 文件导出 |
| `Capture/CsvToDbService.cs` | CSV 数据写入数据库 |
| `Capture/SessionArchiveService.cs` | Session 目录归档 |
| `Analysis/DatabaseQueryService.cs` | 通用 SQL 查询 |
| `Analysis/DrawCallQueryService.cs` | DrawCall 数据查询 |
| `Analysis/DrawCallAnalysisService.cs` | DrawCall 分析逻辑 |
| `Analysis/DrawCallLabelService.cs` | DrawCall 标签解析 |
| `Analysis/MetricsCsvService.cs` | 指标 CSV 导出 |
| `Analysis/ReportGenerationService.cs` | 分析报告生成 |
| `Analysis/SdpFileService.cs` | .sdp 文件导入管理 |

### `source/Tools/` - 工具类
- **`TextureExtractor.cs`**：纹理二进制数据解码与 PNG 保存
- **`ShaderExtractor.cs`**：SPIR-V/GLSL Shader 数据提取
- **`LlmApiWrapper.cs`**：LLM API 调用封装（AI 辅助分析）

### `source/Models/` - 数据模型
- **`DrawCallModels.cs`**：DrawCall、Binding 等核心数据结构
- **`VulkanSnapshotModel.cs`**：Vulkan Snapshot 相关数据模型

### `source/Logging/` - 日志系统
- **`AppLogger.cs`** / **`ContextLogger.cs`**：结构化日志，支持上下文信息
- **`AppLogLevel.cs`**：日志级别（Debug / Info / Warning / Error）

**日志文件位置**：`<exe目录>/.log/consolelog.txt`，即：
```
SDPCLI\bin\Debug\net472\.log\consolelog.txt
```
- 启动时控制台会打印完整路径：`Log: D:\...\consolelog.txt`
- 文件以**追加**方式写入，超过 10 MB 自动轮转为 `consolelog.1.txt`
- 添加 `--debug` 参数启动可开启 DEBUG 级别日志和线程 ID 输出

### `source/Analysis/AnalysisPipeline.cs` - 分析管道
离线分析的完整流程编排，协调各 Analysis 服务完成端到端分析。

---

## 数据结构参考

### DescSetBindings.DescBindings（14 字段）
```csharp
public struct DescBindings
{
    public uint captureID;           // Capture ID
    public uint apiID;               // VulkanSnapshotApis.resourceID
    public uint descriptorSetID;     // Descriptor Set ID
    public uint slotNum;             // Binding slot
    public ulong samplerID;          // vkCreateSampler handle
    public ulong imageViewID;        // vkCreateImageView handle (Texture!)
    public uint imageLayout;         // VkImageLayout enum
    public ulong texBufferview;      // Texel buffer view
    public ulong bufferID;           // vkCreateBuffer handle
    public ulong offset;             // Buffer offset
    public ulong range;              // Buffer range
    public ulong accelStructID;      // Acceleration structure (ray tracing)
    public ulong tensorID;           // Tensor ID (AI/ML)
    public ulong tensorViewID;       // Tensor view
}
```

### 自定义表 Schema
```sql
-- DrawCallDescriptorBindings（每个 DrawCall 的绑定信息）
CREATE TABLE DrawCallDescriptorBindings (
    captureID INTEGER,
    apiID INTEGER,              -- 对应 VulkanSnapshotApis.resourceID
    descriptorSetID INTEGER,
    slotNum INTEGER,
    samplerID INTEGER,
    imageViewID INTEGER,        -- 关联 VulkanSnapshotImageViews.resourceID
    imageLayout INTEGER,
    texBufferview INTEGER,
    bufferID INTEGER,
    offset INTEGER,
    range INTEGER,
    accelStructID INTEGER,
    tensorID INTEGER,
    tensorViewID INTEGER
);

-- DrawCallPipelines（DrawCall 的 pipeline 关联）
CREATE TABLE DrawCallPipelines (
    captureID INTEGER,
    apiID INTEGER,
    pipelineID INTEGER          -- 关联 VulkanSnapshotPipelines.resourceID
);
```

---

## 参考文档

### 官方文档（dll/project）
- **SDPClientFramework** 源码：
  - `dll/project/SDPClientFramework/Sdp/ConnectionManager.cs` - ProcessorPlugin 用法
  - `dll/project/SDPClientFramework/Sdp/Helpers/ByteBufferGateway.cs` - 二进制 buffer 访问模式
  
- **QGLPlugin** 源码：
  - `dll/project/QGLPlugin/QGLPlugin.cs` - `SnapshotDsbBuffer`（public）、`VkSnapshotModel`（internal）
  - `dll/project/QGLPlugin/VkSnapshotModel.cs` - `PopulateDescSets()` 实现
  - `dll/project/QGLPlugin/DescSetBindings.cs` - `DescBindings` 结构定义

### 已创建的文档
- `QUICKSTART.md` - 快速开始指南
- `CLI_PARAMETERS.md` - 完整命令行参数说明
- `CONFIG_GUIDE.md` - config.ini 配置项说明

---

## 注意事项

### 1. DLL 访问权限
- **可用**：`QGLPlugin.SnapshotDsbBuffer`（public static 属性）
- **不可用**：`QGLPlugin.VkSnapshotModel`（internal class，外部项目不可见）
- **解决**：使用 `ProcessorPlugin.GetLocalBuffer()` 公开 API

### 2. Buffer 数据时序
- **Capture 模式**：SnapshotDsbBuffer 只在 **replay 完成后** 填充，不保存到 .sdp
- **Analysis 模式**：必须从 **自定义表** 读取（需在 Capture 时保存）
- **当前状态**：`QGLPluginService` + `CsvToDbService` 负责获取并写入 buffer 数据

### 3. 插件部署
- Capture 模式需要 `plugins/processor/*.dll`
- 从 Snapdragon Profiler 安装目录复制到 `bin/Debug/plugins/processor/`
- 或使用 SDPCLI.csproj 的 `<Target Name="CopyPlugins">` 自动复制

### 4. 配置文件
- `config.ini` 必须在 **exe 同目录**
- 关键参数：`PackageName`、`ActivityName`、`RenderingAPI=16`（Vulkan）
- **输出目录**：默认 `<WorkingDirectory>/project/sdp/`，可在 `config.ini` 中通过 `ProjectDir`/`SdpDir` 配置
- analysis 模式输出默认到 `<WorkingDirectory>/project/analysis/<sdp_basename>/snapshot_N/`

---

## 故障排查

### 找不到 ProcessorPlugin
**症状**：`SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor")` 返回 null

**解决**：
```powershell
# 检查插件目录
ls bin\Debug\plugins\processor\
# 应包含：QGLObserverPluginProcessor.dll, VulkanObserverPluginProcessor.dll 等

# 确保 Main.cs 设置了正确路径
string pluginPath = Path.Combine(exeDir, "plugins", "processor");
SdpApp.ConnectionManager.LoadProcessorPlugins(pluginPath);
```

### Texture 查询不准确
**症状**：Analysis 模式查询 texture 返回所有 capture 中的 texture，而非该 DrawCall 绑定的

**原因**：
1. SnapshotDsbBuffer 只在 **Capture 模式的 replay 阶段** 填充
2. .sdp 文件不包含该 buffer（未保存到 VulkanSnapshotByteBuffers 表）
3. Analysis 模式无法访问 buffer，回退到 "所有 texture" 查询

**解决步骤**：
1. **运行 Capture 模式**，然后查询自定义表：
   ```sql
   -- 查询 DrawCall 456 绑定的 texture
   SELECT imageViewID FROM DrawCallDescriptorBindings WHERE apiID=456;
   ```

2. **如果自定义表为空**：检查 `QGLPluginService` 是否正确获取到 buffer；回退到 "所有 texture" 查询。

### SnapshotDsbBuffer 为空
**症状**：`QGLPlugin.SnapshotDsbBuffer` 返回 null 或 size=0

**原因**：
- Replay 未完成，`VkSnapshotModel.PopulateDescSets()` 未调用
- 检查 `SimpleClientDelegate.OnDataProcessed` 是否接收到 `BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA` 事件

**调试**：
```csharp
// 在 SimpleClientDelegate.cs 添加日志
public override void OnDataProcessed(uint bufferCategory, uint captureID, uint bufferID)
{
    Console.WriteLine($"OnDataProcessed: category={bufferCategory}, captureID={captureID}, bufferID={bufferID}");
    if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA)
    {
        Console.WriteLine($"✓ Replay completed, SnapshotDsbBuffer should be available");
    }
}
```

### 编译错误："VkSnapshotModel is internal"
**症状**：`error CS0122: 'QGLPlugin.VkSnapshotModel' is inaccessible due to its protection level`

**原因**：SDPCLI 是外部项目，无法访问 QGLPlugin.dll 的 `internal` 成员

**正确做法**：
- ❌ 不能：`QGLPlugin.VkSnapshotModel.GetBoundInfo(...)`
- ✅ 可以：`ProcessorPlugin.GetLocalBuffer(...)` 或 `QGLPlugin.SnapshotDsbBuffer`

---

## 开发日志

### 2026-03-17: 项目初始化
- 创建 SDPCLI.sln，引用 SDPClientFramework.dll
- 实现 Main.cs（DLL 路径设置）
- 实现 SDPClient.cs（Client 初始化）

### 2026-03-18: Capture 模式完成
- Phase 1-4：设备连接、App 启动、Capture 创建
- Phase 5-6：截图触发、数据导出
- CliDeviceDelegate、SimpleClientDelegate 事件处理
- 成功导出第一个 .sdp 文件

### 2026-03-19: Analysis 模式实现
- CLI 参数解析（`-mode`, `-sdp`）
- ImportCapture 流程
- DrawCallAnalysis 工具类
- SQL 查询交互界面

### 2026-03-20: Shader 查询修复
- 发现 VulkanSnapshotGraphicsShaderStages 表结构变化
- 修复列名：`type` → `stageType`，添加 `pName` 列
- 实现 GetShaderForDrawCall() 完整查询

### 2026-03-21: Texture 查询问题分析
- 发现 texture 查询返回所有 texture（不准确）
- 调研 SnapshotDsbBuffer：只在 Capture 模式的 replay 填充
- 确认 .sdp 文件不包含该 buffer（VulkanSnapshotByteBuffers 表为空）
- 设计解决方案：Capture 时保存绑定到自定义表

### 2026-03-22: DrawCallBindingSaver 实现
- 创建 DrawCallBindingSaver.cs
- 实现三层数据源回退：ProcessorPlugin → SnapshotDsbBuffer → Database
- 实现 DumpBufferToTextFile（DEBUG 模式）
- 设计自定义表 Schema（DrawCallDescriptorBindings, DrawCallPipelines）
### 2026-03-23: 架构深入分析与依赖关系澄清
- 用户指出：dll/project 是**参考源码**（只读，不修改不编译）
- 确认 VkSnapshotModel 是 `internal`，外部项目不可访问
- **发现事件机制**：QGLPlugin 构造函数订阅 `ConnectionEvents.DataProcessed` 事件
- **验证数据流**：
  1. replay 完成 → `OnDataProcessed` 事件触发
  2. QGLPlugin 调用 `processorPlugin.GetLocalBuffer(bufferID=3)` 获取 SnapshotDsbBuffer
  3. `VkSnapshotModel.PopulateDescSets()` 解析到内存（`AllDescSetBindings` 字典）
  4. ❌ **不写入数据库**：`ClearDsbBuffer()` 释放内存，.sdp 文件中无此 buffer
- **插件-宿主架构分析**：
  - QGLPlugin **依赖** SDPClientFramework（引用 SdpApp.EventsManager 等）
  - SDPClientFramework **加载** QGLPlugin（运行时通过 ProcessorPluginMgr）
  - SDPCLI **必须依赖** SDPClientFramework 才能使用 ProcessorPluginMgr
- **关键澄清**：之前误以为 SDPCLI 可以跳过 SDPClientFramework，实际上**必须引用**
- **依赖链验证**：SDPCLI.csproj 已引用 SDPClientFramework.dll、QGLPlugin.dll、SDPCoreWrapper.dll
- **SDPCLI 方案确认**：必须在 Capture 时自己解析并保存到自定义表，供 Analysis 模式查询
- 更新 README.md，记录完整的插件-宿主架构和正确的依赖关系

### 2026-04-02: 代码结构重构
- 引入分层架构：`Modes/`、`Services/`、`Models/`、`Tools/`、`Logging/`、`Analysis/`
- `SimpleDeviceDelegate` → `CliDeviceDelegate`，`SimpleClientDelegate` 保留
- `DrawCallAnalysis.cs` → `Services/Analysis/` 下多个专职服务类
- `DrawCallBindingSaver.cs` → `Services/Capture/QGLPluginService.cs` + `CsvToDbService.cs`
- 新增 `Tools/LlmApiWrapper.cs`（LLM API 支持）
- 新增 `Models/DrawCallModels.cs`、`Models/VulkanSnapshotModel.cs`
- 新增 `Logging/` 日志子系统（`AppLogger`、`ContextLogger`、`AppLogLevel`）
- `ConsolePlatform.cs`、`ConsoleLogSink.cs` 新增控制台适配层

### **下一步计划**
1. **测试 Capture 模式**：验证新 Service 架构下的完整流程
2. **测试 Analysis 模式**：查询 DrawCallDescriptorBindings 表
3. **验证 Texture/Shader 提取**：TextureExtractionMode、ShaderExtractionMode

---

## 技术要点总结

### 为什么不能直接修改 GUI 源码？
1. **dll/project 是参考材料**：用于理解 API 调用流程，不是工作代码
2. **预编译 DLL**：SDPClientFramework.dll、QGLPlugin.dll 已编译，无法修改
3. **项目目标**：用 CLI 替代 GUI，**调用相同的 API**，而非重新编译 GUI
4. **依赖结构相同**：SDPCLI 和 GUI 使用相同的依赖链（SDPClientFramework → QGLPlugin）

### 为什么 SDPCLI 不能访问 VkSnapshotModel？
1. **Assembly 边界 + 访问修饰符**：
   - QGLPlugin 编译时**依赖** SDPClientFramework（可以调用其 public API）
   - 但外部项目（SDPCLI）只能访问 QGLPlugin 的 **public 成员**
   - `VkSnapshotModel` 是 `internal` → 对外部项目不可见
   
2. **插件-宿主架构**：
   ```
   [宿主] SDPClientFramework.dll
      └── 提供 SdpApp.EventsManager、SdpApp.ConnectionManager
      └── 运行时加载 [插件] QGLPlugin.dll
             └── 订阅事件：ConnectionEvents.DataProcessed
             └── 但 VkSnapshotModel 是 internal，外部不可访问
   ```
   
3. **C# 访问修饰符**：
   ```csharp
   // QGLPlugin.cs
   internal static VkSnapshotModel VkSnapshotModel { get; set; }  // ❌ SDPCLI 不可见
   public static BinaryDataPair SnapshotDsbBuffer { get; }         // ✅ SDPCLI 可访问
   ```

3. **GUI 源码参考价值**：
   - 看到 `ByteBufferGateway` 如何查询 VulkanSnapshotByteBuffers 表
   - 看到 `VkSnapshotModel.PopulateDescSets()` 的调用时机
   - 学习数据流：replay → buffer 填充 → 数据库保存
   - **不是为了复制代码**，而是理解 API 使用模式

### ProcessorPlugin.GetLocalBuffer() 是最佳方案
```csharp
// ✅ 正确：使用 public API
SDPProcessorPlugin plugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
BinaryDataPair buffer = plugin.GetLocalBuffer(
    SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA,
    bufferID: 3,  // SnapshotDsbBuffer
    captureID
);

// ❌ 错误：尝试访问 internal 成员（编译失败）
VkSnapshotModel model = QGLPlugin.VkSnapshotModel;  // CS0122 错误
```

### 插件架构模式
这是一个标准的**插件-宿主（Plugin-Host）架构**：
- **宿主（Host）**：SDPClientFramework 提供基础设施
  - 事件系统：`SdpApp.EventsManager`
  - 连接管理：`SdpApp.ConnectionManager`
  - 插件加载：`ProcessorPluginMgr`
- **插件（Plugin）**：QGLPlugin 实现具体功能
  - 编译时**依赖**宿主框架（引用 SDPClientFramework.dll）
  - 运行时被宿主**动态加载**（ProcessorPluginMgr.LoadPlugin）
  - 订阅事件、处理数据、但保持 internal 实现私有

**为什么 QGLPlugin 依赖 SDPClientFramework？**
- 插件需要访问宿主提供的服务（事件、连接、数据模型）
- 这是正常的插件架构设计，不是循环依赖

---

## License

与 Snapdragon Profiler 主项目保持一致。

---

## 联系方式

项目问题和建议请通过 issue 或内部 channel 反馈。
