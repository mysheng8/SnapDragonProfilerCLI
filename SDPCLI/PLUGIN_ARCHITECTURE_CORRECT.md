# Plugin 架构设计（正确版本）

> **纠正说明**：本文档纠正对 SDP Plugin 体系的理解错误  
> **关键认知**：SDP 使用**三层 Plugin 架构**（一发一收模型），而非双层

---

## 1. 三层 Plugin 架构总览

```
┌─────────────────────────────────────────────────────────────────┐
│  Android 设备                                                    │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Layer 1: 设备端 Plugin（采集 - "发"）                    │  │
│  │  android/arm64-v8a/                                        │  │
│  │  - pluginGPU-Vulkan        ← 拦截 Vulkan API 调用         │  │
│  │  - pluginGPU-OpenGLES      ← 拦截 OpenGL ES API 调用      │  │
│  │  - pluginCPU               ← 采集 CPU 性能数据            │  │
│  │  - sdpservice              ← 加载和管理其他 plugin         │  │
│  └───────────────────────────────────────────────────────────┘  │
│             ↓ 生成 gfxr 文件 / 实时数据流                       │
└──────────────────────────────┬──────────────────────────────────┘
                               ↓ ADB 传输
┌──────────────────────────────┴──────────────────────────────────┐
│  PC 端                                                           │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Layer 2: Processor Plugin（处理 - "收"）                 │  │
│  │  plugins/processor/                                        │  │
│  │  - VulkanProcessor.dll     ← 处理 Vulkan gfxr → 数据库    │  │
│  │  - GLESProcessor.dll       ← 处理 OpenGL ES 数据 → 数据库 │  │
│  │  - CPUProcessor.dll        ← 处理 CPU 数据 → 数据库       │  │
│  │                                                            │  │
│  │  管理器：ProcessorPluginMgr (SDPCore.dll)                 │  │
│  └───────────────────────────────────────────────────────────┘  │
│             ↓ 写入 sdp.db 数据库                                │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Layer 3: Metric Plugin（展示 - UI）                      │  │
│  │  plugins/                                                  │  │
│  │  - QGLPlugin.dll           ← Vulkan 数据 UI 展示          │  │
│  │  - OpenGLPlugin.dll        ← OpenGL ES 数据 UI 展示       │  │
│  │  - CPUPlugin.dll           ← CPU 数据 UI 展示             │  │
│  │                                                            │  │
│  │  管理器：PluginManager (SDPClientFramework.dll)           │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. 三层 Plugin 详解

### 2.1 Layer 1: 设备端 Plugin（数据采集 - "发"）

**位置**: `SDPCLI/android/arm64-v8a/` 或 `SDPCLI/android/armeabi-v7a/`

**技术栈**: Native binaries (C/C++，无扩展名或 .so)

**运行环境**: Android 设备 `/data/local/tmp/sdp/`

**核心文件**:
```
pluginGPU-Vulkan          # Vulkan API 拦截和采集
pluginGPU-OpenGLES        # OpenGL ES API 拦截和采集
pluginGPU-GGPM            # GPU 利用率监控
pluginCPU                 # CPU 性能采集
pluginDSP-*               # DSP 采集
pluginPerfetto            # Perfetto 集成
sdpservice                # 主服务，加载其他 plugin
qhas_sdp                  # 核心库
libSDPCore.so             # SDK 核心库
```

**工作流程**:
1. 通过 ADB 推送到设备：`adb push android/arm64-v8a/* /data/local/tmp/sdp/`
2. `sdpservice` 启动并加载对应的 GPU/CPU plugin
3. Plugin 通过动态链接拦截应用的 API 调用（例如，Vulkan layer）
4. 采集数据写入 gfxr 文件或通过网络实时发送到 PC 端

---

### 2.2 Layer 2: Processor Plugin（数据处理 - "收"）

**位置**: `SDPCLI/plugins/processor/`

**技术栈**: Native DLLs (C++)

**运行环境**: PC 端（通过 SDPCore.dll 加载）

**核心文件**:
```
VulkanProcessor.dll       # 处理 Vulkan 采集的 gfxr 文件
GLESProcessor.dll         # 处理 OpenGL ES 采集的数据
DX12Processor.dll         # 处理 DirectX 12 数据
CPUProcessor.dll          # 处理 CPU 性能数据
GlobalGPUProcessor.dll    # 处理 GPU 利用率数据
DSPProcessor.dll          # 处理 DSP 数据
PerfettoProcessor.dll     # 处理 Perfetto trace
SystraceProcessor.dll     # 处理 Systrace 数据
```

**加载方式**:
```csharp
// 通过 ProcessorPluginMgr (SDPCore.dll 提供的 SWIG 封装)
var pluginMgr = ProcessorPluginMgr.Get();

// 加载 Processor Plugins（从 plugins/processor/ 目录）
pluginMgr.LoadPlugins(client, "plugins/processor");

// 获取特定 Plugin
SDPProcessorPlugin vulkanPlugin = pluginMgr.GetPlugin("SDP::QGLPluginProcessor");
// 注意："QGLPluginProcessor" 实际对应 VulkanProcessor.dll
```

**核心方法**:
```csharp
// 方法 1: ImportCapture - 处理整个 Capture
bool success = vulkanPlugin.ImportCapture(
    sourceCaptureID,      // 源 Capture ID
    destCaptureID,        // 目标 Capture ID
    dbPath,               // 数据库路径
    versionMajor,         // 版本号
    versionMinor,
    versionSubminor
);

// 方法 2: ImportGfxrFile - 处理 gfxr 文件
bool imported = vulkanPlugin.ImportGfxrFile(gfxrFilePath, dbPath);
```

**工作流程**:
1. 读取设备采集的 gfxr 文件或数据流
2. Replay graphics commands（重放图形命令）
3. 解析资源（Pipeline、Shader、Texture、Buffer）
4. 写入 sdp.db 数据库表（例如，`VulkanSnapshotGraphicsPipelines`）
5. **异步操作**：ImportCapture 是异步的，立即返回但后台继续处理

---

### 2.3 Layer 3: Metric Plugin（数据展示 - UI）

**位置**: `SDPCLI/plugins/`

**技术栈**: Managed DLLs (C#)

**运行环境**: PC 端（通过 SDPClientFramework.dll 加载）

**核心文件**:
```
QGLPlugin.dll             # Vulkan 数据 UI 展示（VkSnapshotModel）
OpenGLPlugin.dll          # OpenGL ES 数据 UI 展示
DX12Plugin.dll            # DirectX 12 数据 UI 展示
CPUPlugin.dll             # CPU 性能 UI 展示
GlobalGPUTracePlugin.dll  # GPU 利用率图表
DSPPlugin.dll             # DSP 数据 UI
SystracePlugin.dll        # Systrace 可视化
TraceImportExport.dll     # Trace 导入导出
```

**加载方式**:
```csharp
// 由 PluginManager (SDPClientFramework) 自动加载
// 从 plugins/ 目录扫描所有 *.dll

var pluginManager = new PluginManager();  // 构造函数中自动加载

// 获取处理特定 Metric 的 Plugin
IMetricPlugin plugin = pluginManager.GetMetricPlugin(metricDescription);
```

**接口定义**:
```csharp
public interface IMetricPlugin
{
    bool HandlesMetric(MetricDescription metricDesc);
    MetricTrackType GetMetricTrackType(MetricDescription metricDesc);
    string MetricDisplayName(Metric m);
    void StartCapture(MetricDescription metricDesc);
    void Shutdown();
    GroupLayoutController Container { get; set; }
}
```

**工作流程**:
1. 从 sdp.db 数据库读取数据（例如，`VulkanSnapshotGraphicsPipelines` 表）
2. 构建数据模型（例如，`VkSnapshotModel`）
3. 渲染 UI（图表、表格、3D 视图）
4. 响应用户交互（选择 DrawCall、查看 Shader）

---

## 3. 完整配对关系（一发一收）

| Graphics API | 设备端 Plugin (采集) | Processor Plugin (处理) | Metric Plugin (展示) |
|--------------|---------------------|------------------------|---------------------|
| **Vulkan** | `pluginGPU-Vulkan` | `VulkanProcessor.dll`<br>`SDP::QGLPluginProcessor` | `QGLPlugin.dll`<br>(VkSnapshotModel) |
| **OpenGL ES** | `pluginGPU-OpenGLES` | `GLESProcessor.dll`<br>`SDP::GLESPluginProcessor` | `OpenGLPlugin.dll`<br>(GLESModel) |
| **DirectX 12** | - (PC only) | `DX12Processor.dll`<br>`SDP::Dx12ProcessorPlugin` | `DX12Plugin.dll` |
| **CPU** | `pluginCPU` | `CPUProcessor.dll` | `CPUPlugin.dll` |
| **GPU Utilization** | `pluginGPU-GGPM` | `GlobalGPUProcessor.dll` | `GlobalGPUTracePlugin.dll` |
| **DSP** | `pluginDSP-*` | `DSPProcessor.dll` | `DSPPlugin.dll` |

---

## 4. 命名混淆问题

### 4.1 `QGLPluginProcessor` 的命名误导

**问题**: `SDP::QGLPluginProcessor` 名称容易误解为 "Qualcomm OpenGL Plugin Processor"

**实际**: 它是 **VulkanProcessor.dll** 的 ProcessorPlugin 名称

**证据**:
1. 源码路径：`dll/project/QGLPlugin/` 包含 `VkCapture.cs`、`VkSnapshotModel.cs`、`VulkanSnapshot*` 等 Vulkan 专有代码
2. OpenSessionDialogController.cs L414:
   ```csharp
   if (api == RenderingAPI.Vulkan || api == RenderingAPI.None)
   {
       SDPProcessorPlugin plugin2 = ProcessorPluginMgr.Get().GetPlugin("SDP::QGLPluginProcessor");
       // ...
   }
   ```
3. OpenSessionDialogController.cs L421:
   ```csharp
   if (api == RenderingAPI.OpenGL || api == RenderingAPI.None)
   {
       // OpenGL ES: 直接读取 GLESModel 数据库，不调用 ProcessorPlugin
   }
   ```

**推测**: 历史遗留命名，早期可能叫 "Qualcomm Graphics Library Plugin"，后来演化为 Vulkan 专用但未改名

---

### 4.2 正确的 Processor Plugin 名称映射

| Processor DLL 文件 | ProcessorPluginMgr 中的名称 | 用途 |
|-------------------|---------------------------|------|
| `VulkanProcessor.dll` | `SDP::QGLPluginProcessor` ⚠️ | Vulkan 数据处理 |
| `GLESProcessor.dll` | `SDP::GLESPluginProcessor` | OpenGL ES 数据处理 |
| `DX12Processor.dll` | `SDP::Dx12ProcessorPlugin` | DirectX 12 数据处理 |
| `CPUProcessor.dll` | `SDP::CPUProcessorPlugin` (推测) | CPU 数据处理 |

---

## 5. 工作流程示例（Vulkan Snapshot）

### 5.1 完整流程（从采集到展示）

```
1. 设备端采集（Layer 1）
   ┌──────────────────────────────┐
   │ Android 设备                  │
   │ pluginGPU-Vulkan 加载         │
   │   ↓                          │
   │ 拦截 vkCmdDraw() 等 Vulkan API │
   │   ↓                          │
   │ 记录 Pipeline、Shader、Texture │
   │   ↓                          │
   │ 写入 capture_3.gfxr           │
   └──────────────────────────────┘
              ↓ ADB pull
   
2. PC 端处理（Layer 2）
   ┌──────────────────────────────┐
   │ SDPCLI (Application.cs)       │
   │ ProcessorPluginMgr.Get()      │
   │   .GetPlugin("SDP::QGLPluginProcessor")
   │   .ImportGfxrFile(gfxrPath, dbPath)
   │   ↓                          │
   │ VulkanProcessor.dll 加载      │
   │   ↓                          │
   │ Replay gfxr 文件             │
   │   ↓                          │
   │ 解析 Vulkan 资源             │
   │   ↓                          │
   │ 写入 sdp.db:                 │
   │  - VulkanSnapshotGraphicsPipelines
   │  - VulkanSnapshotShaderModules
   │  - VulkanSnapshotImages      │
   │  - ...                       │
   └──────────────────────────────┘
              ↓
   
3. PC 端展示（Layer 3）
   ┌──────────────────────────────┐
   │ SDPClientFramework            │
   │ PluginManager.GetMetricPlugin()
   │   ↓                          │
   │ QGLPlugin.dll 加载            │
   │   ↓                          │
   │ 从 sdp.db 读取数据            │
   │   ↓                          │
   │ 构建 VkSnapshotModel          │
   │   ↓                          │
   │ 渲染 UI:                     │
   │  - Pipeline 列表             │
   │  - Shader 代码               │
   │  - Texture 预览              │
   └──────────────────────────────┘
```

---

### 5.2 代码示例（Application.cs 中的用法）

```csharp
// Phase 7: 使用 Processor Plugin 处理 gfxr
logger.Info("Phase 7: Replaying gfxr files into database...");

// 获取 Processor Plugin（Vulkan）
bool imported = ProcessorPluginMgr.Get()
    .GetPlugin("SDP::QGLPluginProcessor")  // ← 实际是 VulkanProcessor.dll
    ?.ImportCapture(captureId, captureId, dbPath, 1, 0, 9524) ?? false;

if (imported)
{
    logger.Info("✓ ImportCapture call succeeded - waiting for completion...");
    
    // ImportCapture 是异步的，需要等待完成
    // 通过轮询数据库表行数来判断处理是否完成
    WaitForProcessorPluginCompletion(dbPath);
}
else
{
    throw new Exception("Processor plugin ImportCapture failed");
}
```

---

## 6. Plugin 协调服务设计

基于正确的理解，设计 `PluginCoordinationService` 来管理三层 Plugin 的配对：

```csharp
namespace SnapdragonProfilerCLI.Services.Shared
{
    public class PluginCoordinationService
    {
        private readonly Dictionary<GraphicsAPI, PluginTriple> pluginMapping;
        
        public PluginCoordinationService()
        {
            // 三层 Plugin 映射表
            this.pluginMapping = new Dictionary<GraphicsAPI, PluginTriple>
            {
                { 
                    GraphicsAPI.Vulkan, 
                    new PluginTriple
                    {
                        DevicePlugin = "pluginGPU-Vulkan",      // Layer 1
                        ProcessorPlugin = "SDP::QGLPluginProcessor",  // Layer 2
                        MetricPlugin = "QGLPlugin.QGLPlugin"    // Layer 3
                    }
                },
                { 
                    GraphicsAPI.OpenGLES, 
                    new PluginTriple
                    {
                        DevicePlugin = "pluginGPU-OpenGLES",
                        ProcessorPlugin = "SDP::GLESPluginProcessor",
                        MetricPlugin = "OpenGLPlugin.OpenGLPlugin"
                    }
                },
                { 
                    GraphicsAPI.DirectX12, 
                    new PluginTriple
                    {
                        DevicePlugin = null,  // DirectX 无设备端 plugin
                        ProcessorPlugin = "SDP::Dx12ProcessorPlugin",
                        MetricPlugin = "DX12Plugin.DX12Plugin"
                    }
                }
            };
        }
        
        // 验证设备端 Plugin
        public bool ValidateDevicePlugins(IDevice device, GraphicsAPI api);
        
        // 部署设备端 Plugin
        public void DeployDevicePlugins(IDevice device, GraphicsAPI api, string arch);
        
        // 获取 Processor Plugin
        public SDPProcessorPlugin GetProcessorPlugin(GraphicsAPI api);
        
        // 获取完整的三元组
        public PluginTriple GetPluginTriple(GraphicsAPI api);
    }
    
    public class PluginTriple
    {
        public string? DevicePlugin { get; set; }      // Layer 1
        public string ProcessorPlugin { get; set; }    // Layer 2
        public string MetricPlugin { get; set; }       // Layer 3
    }
}
```

---

## 7. 关键要点总结

1. **三层架构，不是两层**：
   - Layer 1: 设备端 Plugin（采集 - "发"）
   - Layer 2: Processor Plugin（处理 - "收"）
   - Layer 3: Metric Plugin（展示 - UI）

2. **一发一收模型**：
   - 设备端采集数据（pluginGPU-Vulkan）
   - Processor 处理数据（VulkanProcessor.dll）
   - Metric 展示数据（QGLPlugin.dll）

3. **命名混淆**：
   - `SDP::QGLPluginProcessor` ≠ OpenGL，实际是 Vulkan
   - 原因：历史遗留命名

4. **两个 PluginManager**：
   - `ProcessorPluginMgr`（SDPCore.dll）管理 Layer 2
   - `PluginManager`（SDPClientFramework）管理 Layer 3

5. **关键区别**：
   - `plugins/processor/` = Native C++ DLLs（PC 端数据处理）
   - `plugins/` = Managed C# DLLs（PC 端 UI 展示）
   - `android/arm64-v8a/` = Native binaries（设备端数据采集）

6. **SDPCLI 的使用**：
   - 只需使用 Layer 2（Processor Plugin）
   - 因为 SDPCLI 是命令行工具，不需要 Layer 3（UI Plugin）
   - Layer 1（设备端）通过 ADB 部署和管理

---

**文档版本**: v1.0 (纠正版)  
**创建时间**: 2026-03-24  
