# VulkanSnapshotModel 使用指南

## 概述

`VulkanSnapshotModel`（`source/Models/VulkanSnapshotModel.cs`）用于解析和查询 Vulkan Snapshot 的 per-DrawCall 资源绑定。它复刻了 Snapdragon Profiler GUI 的 `VkSnapshotModel` 核心功能，包含完整的 API Trace 解析器和反向索引。

**主要能力**：
- 解析 `SnapshotDsbBuffer`（二进制 Buffer），重建所有 DescriptorSet 的绑定关系
- 解析 `SnapshotApiBuffer`（API Trace），追踪每个 DrawCall 绑定的 Pipeline、DescriptorSets、VertexBuffers
- 追踪每个 DrawCall 写入的 Framebuffer Attachments（RenderTarget）
- 提供反向索引：给定一个资源 ID，查询所有使用它的 DrawCalls
- 支持从 二进制 Buffer、CSV 文件、SQLite 数据库三种方式加载数据

---

## 快速开始

### 1. 集成到 Application.cs

```csharp
using SnapdragonProfilerCLI.Models;

public class Application
{
    private VulkanSnapshotModel? _snapshotModel;
    
    private void RunCaptureMode()
    {
        // ... 现有的 Capture 流程 ...
        
        // Phase 8: ImportCapture 完成后
        if (imported && sessionPath != null)
        {
            try
            {
                // 1. 获取 Buffer 数据
                var apiBuffer = ProcessorPluginMgr.Get()
                    .GetPlugin("SDP::QGLPluginProcessor")
                    ?.GetLocalBuffer(SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_API_DATA, 3, captureId);
                
                var bindingBuffer = ProcessorPluginMgr.Get()
                    .GetPlugin("SDP::QGLPluginProcessor")
                    ?.GetLocalBuffer(SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA, 3, captureId);
                
                if (bindingBuffer != null && bindingBuffer.size > 0)
                {
                    // 2. 创建并加载 VulkanSnapshotModel
                    _snapshotModel = new VulkanSnapshotModel();
                    _snapshotModel.LoadSnapshot(captureId, apiBuffer ?? new BinaryDataPair(), bindingBuffer);
                    
                    Console.WriteLine("✓ VulkanSnapshotModel loaded successfully");
                    
                    // 3. 可选：保存分析结果
                    SaveResourceAnalysis(captureId, sessionPath);
                }
                else
                {
                    Console.WriteLine("⚠ Binding buffer not available");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Failed to load VulkanSnapshotModel: {ex.Message}");
            }
        }
    }
    
    private void SaveResourceAnalysis(uint captureId, string sessionPath)
    {
        if (_snapshotModel == null) return;
        
        string reportPath = Path.Combine(sessionPath, "resource_analysis.txt");
        using var writer = new StreamWriter(reportPath);
        
        writer.WriteLine("=== Vulkan Resource Binding Analysis ===");
        writer.WriteLine($"Capture ID: {captureId}");
        writer.WriteLine($"Generated: {DateTime.Now}");
        writer.WriteLine();
        
        // 统计信息
        var allDrawCalls = _snapshotModel.GetAllDrawCalls(captureId);
        var allDescriptorSets = _snapshotModel.GetAllDescriptorSets();
        
        writer.WriteLine("=== Summary ===");
        writer.WriteLine($"Total DrawCalls: {allDrawCalls.Count}");
        writer.WriteLine($"Total DescriptorSets: {allDescriptorSets.Count}");
        writer.WriteLine();
        
        // 按 DescriptorSet 分组显示
        writer.WriteLine("=== DescriptorSets ===");
        foreach (var (descSetId, descSet) in allDescriptorSets)
        {
            writer.WriteLine($"\nDescriptorSet ID: {descSetId}");
            writer.WriteLine($"  Bindings: {descSet.Bindings.Count}");
            
            foreach (var (slotNum, binding) in descSet.Bindings)
            {
                writer.WriteLine($"    Slot {slotNum}: {binding}");
            }
        }
        
        Console.WriteLine($"✓ Resource analysis saved to: {reportPath}");
    }
}
```

---

## 核心功能

### 2.1 查询 DrawCall 的资源绑定

```csharp
// 获取 DrawCall 的绑定信息
var drawCallInfo = _snapshotModel.GetDrawCallInfo(captureId, drawCallId);

if (drawCallInfo != null)
{
    Console.WriteLine($"DrawCall {drawCallId}:");
    Console.WriteLine($"  Pipeline: {drawCallInfo.BoundPipeline}");
    Console.WriteLine($"  DescriptorSets: {drawCallInfo.BoundDescriptorSets.Count}");
    
    // 获取所有绑定的资源
    var resources = drawCallInfo.GetBoundResources();
    Console.WriteLine($"  Total Resources: {resources.Count}");
    
    // 按类型过滤
    var images = drawCallInfo.GetResourcesByType(ResourceType.Image);
    var buffers = drawCallInfo.GetResourcesByType(ResourceType.Buffer);
    
    Console.WriteLine($"  Images: {images.Count}");
    foreach (var res in images)
    {
        Console.WriteLine($"    - {res}");
    }
    
    Console.WriteLine($"  Buffers: {buffers.Count}");
    foreach (var res in buffers)
    {
        Console.WriteLine($"    - {res}");
    }
}
```

### 2.2 反向查询：找到使用某个资源的所有 DrawCall

```csharp
// 查找使用指定纹理的所有 DrawCall
ulong textureId = 12345;
var drawCalls = _snapshotModel.GetDrawCallsByResource(
    captureId, 
    ResourceType.Image, 
    textureId
);

Console.WriteLine($"Texture {textureId} is used by {drawCalls.Count} DrawCalls:");
foreach (var drawCallId in drawCalls)
{
    Console.WriteLine($"  - DrawCall {drawCallId}");
}
```

### 2.3 遍历所有 DrawCall

```csharp
var allDrawCalls = _snapshotModel.GetAllDrawCalls(captureId);

Console.WriteLine($"Total DrawCalls: {allDrawCalls.Count}");

foreach (var drawCallId in allDrawCalls)
{
    var info = _snapshotModel.GetDrawCallInfo(captureId, drawCallId);
    var imageCount = info.GetResourcesByType(ResourceType.Image).Count;
    
    Console.WriteLine($"DrawCall {drawCallId}: {imageCount} images");
}
```

### 2.4 检查 DescriptorSet

```csharp
// 获取所有 DescriptorSet（调试用）
var allDescriptorSets = _snapshotModel.GetAllDescriptorSets();

Console.WriteLine($"Total DescriptorSets: {allDescriptorSets.Count}");

foreach (var (descSetId, descSet) in allDescriptorSets)
{
    Console.WriteLine($"\nDescriptorSet {descSetId}:");
    Console.WriteLine($"  Bindings: {descSet.Bindings.Count}");
    
    foreach (var (slotNum, binding) in descSet.Bindings)
    {
        Console.WriteLine($"    Slot {slotNum}:");
        if (binding.imageViewID != 0)
            Console.WriteLine($"      Image: {binding.imageViewID}");
        if (binding.bufferID != 0)
            Console.WriteLine($"      Buffer: {binding.bufferID}");
        if (binding.samplerID != 0)
            Console.WriteLine($"      Sampler: {binding.samplerID}");
    }
}
```

---

## 集成到 Analysis 服务

### 3.1 创建分析服务

```csharp
// Services/Analysis/ResourceAnalysisService.cs
public class ResourceAnalysisService
{
    private readonly VulkanSnapshotModel _snapshotModel;
    private readonly DatabaseQueryService _dbService;
    
    public ResourceAnalysisService(
        VulkanSnapshotModel snapshotModel, 
        DatabaseQueryService dbService)
    {
        _snapshotModel = snapshotModel;
        _dbService = dbService;
    }
    
    public ResourceUsageReport GenerateReport(uint captureId)
    {
        var report = new ResourceUsageReport { CaptureId = captureId };
        
        // 统计每个资源的使用频率
        var resourceUsage = new Dictionary<ulong, int>();
        
        foreach (var drawCallId in _snapshotModel.GetAllDrawCalls(captureId))
        {
            var info = _snapshotModel.GetDrawCallInfo(captureId, drawCallId);
            
            foreach (var resource in info.GetBoundResources())
            {
                if (!resourceUsage.ContainsKey(resource.ResourceId))
                {
                    resourceUsage[resource.ResourceId] = 0;
                }
                resourceUsage[resource.ResourceId]++;
            }
        }
        
        // 找出最常用的资源
        var topResources = resourceUsage
            .OrderByDescending(kvp => kvp.Value)
            .Take(10)
            .ToList();
        
        foreach (var (resourceId, count) in topResources)
        {
            report.TopResources.Add(new ResourceUsageInfo
            {
                ResourceId = resourceId,
                UsageCount = count
            });
        }
        
        return report;
    }
}

public class ResourceUsageReport
{
    public uint CaptureId { get; set; }
    public List<ResourceUsageInfo> TopResources { get; set; } = new();
}

public class ResourceUsageInfo
{
    public ulong ResourceId { get; set; }
    public int UsageCount { get; set; }
}
```

### 3.2 集成到 Analysis Pipeline

```csharp
// Analysis/AnalysisPipeline.cs
public class AnalysisPipeline
{
    private VulkanSnapshotModel? _snapshotModel;
    
    public void Run(string sdpPath)
    {
        // 1. 提取数据库
        string dbPath = ExtractDatabase(sdpPath);
        
        // 2. 获取 Buffers（从 session 目录）
        string sessionDir = Path.GetDirectoryName(dbPath);
        var bindingBuffer = LoadBufferFromSession(sessionDir, "binding_data.bin");
        
        // 3. 加载 VulkanSnapshotModel
        _snapshotModel = new VulkanSnapshotModel();
        _snapshotModel.LoadSnapshot(captureId, new BinaryDataPair(), bindingBuffer);
        
        // 4. 执行分析
        var resourceService = new ResourceAnalysisService(_snapshotModel, _dbService);
        var report = resourceService.GenerateReport(captureId);
        
        // 5. 输出报告
        Console.WriteLine($"Top 10 Most Used Resources:");
        foreach (var res in report.TopResources)
        {
            Console.WriteLine($"  Resource {res.ResourceId}: used {res.UsageCount} times");
        }
    }
}
```

---

## 当前状态

### ✅ 已实现功能

- **DescriptorSet 绑定解析**：从 `SnapshotDsbBuffer`（二进制 Buffer）解析所有 DescriptorSet → Binding 映射关系
- **API Trace 解析**（`ParseApiTrace`）：完整实现，解析以下 Vulkan API：
  - `vkCmdBindPipeline` → 记录当前 Pipeline
  - `vkCmdBindDescriptorSets` → 记录 DescriptorSet 绑定
  - `vkCmdBindVertexBuffers` / `vkCmdBindIndexBuffer` → 记录 Buffer 绑定
  - `vkCmdBeginRenderPass` / `vkCmdEndRenderPass` → 追踪 Framebuffer Attachment 写入
  - `vkCmdDraw*` → 快照当前绑定状态到 DrawCall
- **Framebuffer Attachments 追踪**：记录每个 DrawCall 写入的 RenderTarget
- **Pipeline Vertex Input State 解析**：从 `vkCreateGraphicsPipelines` 提取 Vertex Attribute 布局
- **反向索引**：从 Resource → 使用该资源的所有 DrawCalls
- **CSV 加载**：支持从预保存的 CSV 文件加载（`LoadSnapshotFromCSV`）
- **数据库加载**：支持从 `VulkanSnapshotDescriptorSetBindings` 表加载（`LoadSnapshotFromDatabase`）

### ⚠️ 已知限制

1. **API Trace Buffer 格式依赖**：`SnapshotApiBuffer` 的结构体格式为反编译分析所得，如 Snapdragon Profiler 版本升级可能需要更新 `VulkanSnapshotApi` 结构体。
2. **Compute Dispatch 未追踪**：`vkCmdDispatch*` 不在当前 DrawCall 识别范围内（仅识别 Graphics DrawCall）。
3. **DescriptorSet 数据可用性**：`SnapshotDsbBuffer` 不写入 SQLite 数据库；需要在 Capture 时从内存获取（通过 `ProcessorPlugin.GetLocalBuffer()`），或 Capture 时另存为文件。

---

## 从数据库重建绑定关系（临时方案）

```csharp
public void RebuildFromDatabase(string dbPath, uint captureId)
{
    using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
    conn.Open();
    
    // 1. 查询所有 DrawCall（从 VulkanSnapshotApiCalls 表）
    using var cmdDrawCalls = conn.CreateCommand();
    cmdDrawCalls.CommandText = @"
        SELECT apiID, functionName 
        FROM VulkanSnapshotApiCalls 
        WHERE captureID = @captureID 
          AND functionName LIKE 'vkCmdDraw%'
        ORDER BY apiID";
    cmdDrawCalls.Parameters.AddWithValue("@captureID", captureId);
    
    using var reader = cmdDrawCalls.ExecuteReader();
    while (reader.Read())
    {
        uint drawCallId = (uint)reader.GetInt32(0);
        
        // 创建 DrawCall 信息
        var drawCallInfo = new VulkanDrawCallInfo { DrawCallId = drawCallId };
        
        // TODO: 从数据库查询该 DrawCall 绑定的 DescriptorSet
        // 这需要额外的逻辑来模拟 vkCmdBindDescriptorSets 的状态
        
        AddDrawCallInfo(captureId, drawCallId, drawCallInfo);
    }
}
```

---

## 调试技巧

### 1. 验证 Buffer 数据

```csharp
Console.WriteLine($"Binding Buffer:");
Console.WriteLine($"  Size: {bindingBuffer.size} bytes");
Console.WriteLine($"  Pointer: {bindingBuffer.data}");
Console.WriteLine($"  Expected records: {bindingBuffer.size / Marshal.SizeOf<DescriptorBinding>()}");
```

### 2. 检查解析结果

```csharp
var allDescSets = _snapshotModel.GetAllDescriptorSets();
Console.WriteLine($"Loaded {allDescSets.Count} DescriptorSets");

foreach (var (id, descSet) in allDescSets.Take(5))
{
    Console.WriteLine($"DescriptorSet {id}: {descSet.Bindings.Count} bindings");
}
```

### 3. 导出 CSV（调试用）

```csharp
string csvPath = Path.Combine(sessionPath, "descriptor_sets.csv");
using var writer = new StreamWriter(csvPath);
writer.WriteLine("DescriptorSetID,SlotNum,ImageViewID,BufferID,SamplerID");

foreach (var (descSetId, descSet) in _snapshotModel.GetAllDescriptorSets())
{
    foreach (var (slotNum, binding) in descSet.Bindings)
    {
        writer.WriteLine($"{descSetId},{slotNum},{binding.imageViewID},{binding.bufferID},{binding.samplerID}");
    }
}

Console.WriteLine($"✓ Exported to: {csvPath}");
```

---

## 下一步优化方向

1. **Compute Dispatch 支持**（优先级：中）
   - 将 `vkCmdDispatch*` 纳入 DrawCall 识别范围
   - 记录 Compute Pipeline 和 DescriptorSet 绑定

2. **结构体版本兼容**（优先级：中）
   - `VulkanSnapshotApi` 结构体格式需跟 Snapdragon Profiler 版本保持同步
   - 建议在启动时验证结构体大小与实际 Buffer 的匹配性

3. **DescriptorSet 持久化**（优先级：高）
   - Capture 完成后自动将 SnapshotDsbBuffer 保存到 CSV 或 SQLite 自定义表
   - 确保 Analysis 模式下可以加载历史 Capture 的绑定数据

---

## 常见问题

### Q: 为什么 `GetDrawCallInfo()` 返回 null？

**A**: API Trace Buffer（apiBuffer）可能未传入，或 Buffer 中没有该 DrawCall 对应的命令。请确认 `apiBuffer` 有效（data != IntPtr.Zero 且 size > 0）。

### Q: DescriptorBinding 的 apiID 为什么是 0xFFFFFFFF？

**A**: 这是 Snapshot 模式的设计特点，rawBinding.apiID 不直接对应 DrawCall；DrawCall 关联关系由 API Trace 解析器通过 CommandBuffer 状态机重建。

### Q: 如何获取资源的详细信息（宽度、高度、格式）？

**A**: VulkanSnapshotModel 只存储绑定关系，资源详细信息需要查询数据库：
```sql
SELECT width, height, format 
FROM VulkanSnapshotImageViews 
WHERE captureID = ? AND resourceID = ?
```

### Q: Analysis 模式下如何加载 VulkanSnapshotModel？

**A**: SnapshotDsbBuffer 不持久化到 .sdp 的 SQLite 数据库。需要在 Capture 时保存到文件，然后 Analysis 模式用 `LoadSnapshotFromCSV()` 或 `LoadSnapshotFromDatabase()` 加载。

---

## 参考文档

- [SNAPSHOT_DATA_FLOW_ANALYSIS.md](SNAPSHOT_DATA_FLOW_ANALYSIS.md) - 完整的数据流分析
- GUI 源码参考:
  - `QGLPlugin/VkSnapshotModel.cs` - 数据结构设计
  - `QGLPlugin/VkAPITreeModelBuilder.cs` - API Trace 解析逻辑
