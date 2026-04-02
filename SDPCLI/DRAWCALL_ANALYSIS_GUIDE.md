# DrawCall Analysis Tool - 使用指南

## 功能说明

从Snapdragon Profiler的数据库中查询DrawCall使用的Texture资源。

### 输入
- DrawCall编号，支持两种格式：
  - **点分格式（GUI编码ID）**：`"1.1.31"` = submit #1 → primary commandBuffer #1 → 该CB内第31个draw call
  - **整数ApiID**：`"124943"` = DrawCallParameters表中的DrawCallApiID，直接定位

> **GUI编码ID原理**（来自QGLPlugin/VkAPITreeModelBuilder.cs源码）：
> - `vkQueueSubmit` 触发时 submitIdx++
> - `vkBeginCommandBuffer`（primary CB）触发时 cmdBufferIdx++，drawcallIdx 重置为0
> - 每个draw call（`vkCmdDraw*` / `vkCmdDispatch*` / `vkCmdClear*`）触发时 drawcallIdx++
> - 编码：`(submitIdx << 48) | (cmdBufferIdx << 32) | drawcallIdx`
>
> **为何我们的DrawCallParameters.csv有804行而GUI只显示201个**：
> 804行是整帧所有CB的draw call总和；GUI的`1.1.*`只显示第一个primary CB里的201个。
> 正确做法是在`ParseApiTrace`中追踪CB边界，并在CSV中记录`CmdBufferIdx`和`DrawcallIdx`列，然后按这两列查找而非按全局行位置。

### 输出
- TextureID数组：`uint[]`

---

## C# API 使用示例

### 方法1: 只获取TextureID数组

```csharp
using SnapdragonProfilerCLI;

// 最简单的用法
string dbPath = @"D:\snapdragon\SDPCLI\test\2026-03-20T20-36-12\sdp.db";
uint[] textureIDs = DrawCallAnalysisTool.GetTextureIDs(dbPath, "1.1.2");

Console.WriteLine($"Found {textureIDs.Length} textures");
foreach (uint id in textureIDs) {
    Console.WriteLine($"  Texture ID: {id}");
}
```

### 方法2: 获取详细信息

```csharp
using SnapdragonProfilerCLI;

string dbPath = @"D:\snapdragon\SDPCLI\test\2026-03-20T20-36-12\sdp.db";
var analyzer = new DrawCallAnalysis(dbPath, captureID: 3);

// 获取完整信息
var info = analyzer.GetDrawCallInfo("1.1.2");
if (info != null) {
    Console.WriteLine($"Pipeline ID: {info.PipelineID}");
    Console.WriteLine($"Layout ID: {info.LayoutID}");
    Console.WriteLine($"Texture Count: {info.TextureIDs.Length}");
    
    // 访问texture详细信息
    foreach (var tex in info.Textures) {
        Console.WriteLine($"Texture {tex.TextureID}: {tex.Width}x{tex.Height} {tex.FormatName}");
    }
    
    // 或直接打印
    info.Print();
}
```

### 方法3: 批量查询

```csharp
using SnapdragonProfilerCLI;

string dbPath = @"D:\snapdragon\SDPCLI\test\2026-03-20T20-36-12\sdp.db";
var analyzer = new DrawCallAnalysis(dbPath);

string[] drawCalls = { "1.1.1", "1.1.2", "1.1.5", "1.1.10" };

foreach (string dc in drawCalls) {
    uint[] textures = analyzer.GetTexturesForDrawCall(dc);
    Console.WriteLine($"DrawCall {dc}: {textures.Length} textures");
}
```

---

## PowerShell 测试脚本

### 运行测试脚本

```powershell
# 测试默认DrawCall (1.1.2)
.\test_drawcall_analysis.ps1

# 测试指定DrawCall
.\test_drawcall_analysis.ps1 -DrawCallNumber "1.1.10"

# 测试简单数字编号
.\test_drawcall_analysis.ps1 -DrawCallNumber "5"

# 指定数据库路径
.\test_drawcall_analysis.ps1 -DrawCallNumber "1.1.2" -CaptureFolder "D:\test\capture1"
```

### 输出示例

```
=== DrawCall Texture Analysis Test ===
DrawCall: 1.1.2
Capture: D:\snapdragon\SDPCLI\test\2026-03-20T20-36-12

DrawCall Index: 1 (0-based)

Step 1: Query Pipeline #1...
  Pipeline ID: 816
  Layout ID: 815
  RenderPass ID: 813

Step 2: Query Textures...

  TextureID   | Size           | Format | Slot
  ---------------------------------------------------
  2314        | 32x64          | 97     | 0
  2266        | 4x1            | 97     | 0
  2318        | 32x64          | 97     | 0
  ...

SUCCESS: Found 20 Textures
TextureID Array: [2314, 2266, 2318, ...]
```

---

## 数据库查询逻辑

### 查询流程

1. **解析DrawCall编号** → 转换为Pipeline索引
   - `"1.1.2"` → 索引 1 (取最后数字-1)
   - `"10"` → 索引 9

2. **查询Pipeline** → 获取Pipeline信息
   ```sql
   SELECT resourceID, layoutID, renderPass 
   FROM VulkanSnapshotGraphicsPipelines 
   WHERE captureID = 3
   ORDER BY resourceID
   LIMIT 1 OFFSET [index]
   ```

3. **查询Textures** → 通过Descriptor Set Bindings
   ```sql
   SELECT DISTINCT iv.imageID
   FROM VulkanSnapshotDescriptorSetBindings dsb
   JOIN VulkanSnapshotImageViews iv ON dsb.imageViewID = iv.resourceID
   WHERE dsb.captureID = 3 AND dsb.imageViewID > 0
   ```

### 表关系

```
Pipeline (VulkanSnapshotGraphicsPipelines)
  ↓ layoutID
PipelineLayout (VulkanSnapshotPipelineLayouts)
  ↓ descriptor sets
DescriptorSetBindings (VulkanSnapshotDescriptorSetBindings)
  ↓ imageViewID
ImageView (VulkanSnapshotImageViews)
  ↓ imageID
Texture (VulkanSnapshotTextures)
```

---

## 测试结果

### DrawCall 1.1.2 (Pipeline #1)
- Pipeline ID: 816
- Layout ID: 815
- Textures: 20 found (660 total in capture)
- Top Textures: 2314, 2266, 2318, 2342, 2347...

### DrawCall 1.1.10 (Pipeline #9)
- Pipeline ID: 1007
- Layout ID: 1006
- Textures: 20 found
- Top Textures: 2314, 2266, 2318, 2342, 2347...

### DrawCall 5 (Pipeline #4)
- Pipeline ID: 852
- Layout ID: 851
- Textures: 20 found
- Top Textures: 2314, 2266, 2318, 2342, 2347...

---

## 注意事项

⚠️ **Snapshot模式的局限性**

1. **静态捕获**
   - Snapshot捕获的是单帧的静态状态
   - 不包含动态执行轨迹（API trace packets）

2. **Texture绑定**
   - 无法确定运行时的精确Descriptor Set绑定
   - 返回的是该Capture中所有可能使用的Textures
   - 不是特定DrawCall运行时绑定的确切Textures

3. **DrawCall编号**
   - 编号基于Pipeline在数据库中的顺序
   - 不是GUI CSV导出中的执行顺序
   - Pipeline顺序 ≠ 执行顺序

4. **数量限制**
   - 当前返回前20个Texture（可修改LIMIT）
   - 总共660个unique textures，16640个bindings

---

## 文件清单

- `source/DrawCallAnalysis.cs` - 核心分析类
- `source/DrawCallAnalysisTool.cs` - 命令行工具接口
- `test_drawcall_analysis.ps1` - PowerShell测试脚本
- `DRAWCALL_ANALYSIS_GUIDE.md` - 本文档

---

## 后续改进方向

1. **更精确的绑定**
   - 需要Trace模式捕获（非Snapshot）
   - 记录API执行序列和参数

2. **性能优化**
   - 缓存Pipeline查询结果
   - 索引优化

3. **增强功能**
   - 按RenderPass过滤
   - 按Texture格式/尺寸过滤
   - 导出到JSON/CSV
