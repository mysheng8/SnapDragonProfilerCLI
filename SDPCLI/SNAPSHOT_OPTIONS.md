# Snapshot Launch Options 配置说明

## 问题背景

用户发现Snapshot capture后数据库中关键表（如`SCOPEDrawStages`）为空，没有捕获到per-drawcall性能数据。

## 根本原因

Snapdragon Profiler SDK通过`AppStartSettings.launchOptions`参数控制Snapshot时捕获哪些数据通道。之前代码中`launchOptions = ""`为空字符串，导致只捕获了基本数据，未启用扩展数据通道（GPU Scope, Shaders, Textures等）。

## GUI实现机制

在Snapdragon Profiler GUI中：

1. **Launch Application对话框**显示可选的数据通道（checkboxes）
2. 用户勾选需要的选项（如"Collect GPU Scope Data", "Collect Shader Data"等）
3. 选项被格式化为字符串：`"Option1:1;Option2:0;Option3:1;"`
4. 字符串通过`AppStartSettings.launchOptions`传递给SDK
5. SDK根据启用的选项激活相应的数据采集插件

## CLI实现方案

由于CLI环境没有交互式对话框，我们采用**默认启用所有常见Snapshot选项**的策略：

### 代码位置
[Application.cs](source/Application.cs) 第866-899行（`SelectAndLaunchApp`方法）

### 启用的选项

```csharp
string[] commonSnapshotOptions = new string[]
{
    "Collect Shader Data",          // 捕获shader binaries, disassembly, statistics
    "Collect Texture Data",         // 捕获texture信息
    "Collect Buffer Data",          // 捕获buffer信息
    "Collect Pipeline Data",        // 捕获pipeline state
    "Collect GPU Scope Data",       // 捕获per-drawcall性能数据 ← 关键！
    "Collect Screenshots",          // 捕获screenshot buffers
    "Collect API Calls",            // 捕获API call trace
    "Detailed Metrics",             // 启用detailed metrics collection
};
```

### 构建的launchOptions字符串示例

```
Collect Shader Data:1;Collect Texture Data:1;Collect Buffer Data:1;Collect Pipeline Data:1;Collect GPU Scope Data:1;Collect Screenshots:1;Collect API Calls:1;Detailed Metrics:1;DisableUGDFlag:1;
```

## 期望效果

启用这些选项后，Snapshot capture应该能够填充以下数据库表：

### GPU Scope / Drawcall Data
- `SCOPEDrawStages` - 每个drawcall的GPU阶段性能数据（用户需要的核心数据）
- `SCOPEDrawStageMetrics` - Drawcall stage metrics
- `SCOPEStageMetrics` - GPU Scope stage metrics
- `tblGPUScopeMarkers` - GPU Scope debug markers

### Shader Data
- `VulkanSnapshotShaderData` - Shader binary data
- `VulkanSnapshotShaderStages` - Shader stages
- `GLESSnapshotShaderStatsData` - Shader statistics
- `GLESSnapshotShaderDisasmData` - Shader disassembly

### Texture & Buffer Data
- `VulkanSnapshotTextures` - Texture information
- `VulkanSnapshotMemoryBuffers` - Memory buffers

### API Trace Data
- `VulkanSnapshotGraphicsPipelines` - Graphics pipelines
- `VulkanSnapshotComputePipelines` - Compute pipelines

### Screenshot
- Via `GetBufferData()` API (buffer types: `BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT`, etc.)

## 注意事项

### 1. Drawcall Data vs Trace Mode

**重要发现**：数据库中有一个`Per drawcall stages` metric，但其`captureTypeMask=2`（Trace only），不支持Snapshot mode（captureTypeMask=4）。

这意味着：
- **SCOPEDrawStages表**可能在某些配置下只在Trace mode工作
- Snapshot mode中的drawcall数据可能来自**GPU Scope Data**选项
- 如果仍然没有drawcall数据，可能需要切换到Trace capture type

### 2. Option名称的设备差异

Launch options的具体名称由native SDK定义，可能因：
- SDK版本
- 设备类型
- GPU型号
- 渲染API (Vulkan vs OpenGL)

而略有不同。如果标准选项名称不匹配，SDK会忽略它们（非致命错误）。

### 3. 捕获时机

即使启用了所有选项，如果capture时应用不在渲染状态（启动屏幕/加载界面），某些表仍可能为空。建议：
- 设置`config.ini`中`AutoStartCapture=false`
- 等待应用进入游戏主场景/战斗
- 手动按Enter触发capture

## 验证方法

运行capture后检查：

```powershell
cd test/<latest-timestamp>
sqlite3 sdp.db "SELECT COUNT(*) FROM SCOPEDrawStages"
```

如果返回值 > 0，说明drawcall数据捕获成功。

```powershell
# 检查所有非空表
sqlite3 sdp.db "SELECT name FROM sqlite_master WHERE type='table'" | ForEach-Object {
    $table = $_
    $count = sqlite3 sdp.db "SELECT COUNT(*) FROM $table" 2>$null
    if ($count -gt 0) { Write-Host "$table : $count" }
}
```

## 下一步

如果启用所有选项后`SCOPEDrawStages`仍然为空：

1. **验证Option名称**：对比GUI运行时的launchOptions字符串
2. **检查Metric激活**：确认"GPU Scope"相关metrics被激活
3. **考虑Trace Mode**：如果Snapshot确实不支持drawcall profiling，改用CaptureType=2（Trace）
4. **查看SDK日志**：检查`sdplog.txt`中是否有关于options的错误/警告信息

## 参考

- GUI Implementation: [dll/solution/SDPClientFramework/Sdp/LaunchApplicationDialogController.cs](../dll/solution/SDPClientFramework/Sdp/LaunchApplicationDialogController.cs)
- Options Transmission: [dll/solution/SDPClientFramework/Sdp/DataSourcesController.cs](../dll/solution/SDPClientFramework/Sdp/DataSourcesController.cs) Line 1163-1169
- AppStartSettings: [dll/project/SDPCoreWrapper/AppStartSettings.cs](../dll/project/SDPCoreWrapper/AppStartSettings.cs)
