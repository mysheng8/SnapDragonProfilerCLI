# TreeModel生成流程与ImportCapture关系分析

## 概述

**问题**: GUI导出的CSV文件（如 report3-13.csv）是如何生成的？ImportCapture是如何写入TreeModel的？

## 核心发现

### 1️⃣ **TreeModel数据结构**

```csharp
// TreeModel.cs
public class TreeModel
{
    public List<TreeNode> Nodes;         // 树的根节点列表
    public Type[] ColumnTypes;           // 列类型定义
    public string[] ColumnNames;         // 列名（CSV header）
    
    public void ExportToCSV(StreamWriter sw)
    {
        // 1. 写入列名
        sw.WriteLine(string.Join(",", this.ColumnNames));
        
        // 2. 遍历所有节点
        foreach (TreeNode treeNode in this.Nodes)
        {
            this.ExportTreeNodeToCSV(sw, treeNode);
        }
    }
    
    private void ExportTreeNodeToCSV(StreamWriter sw, TreeNode node)
    {
        // 递归写入节点和子节点
        sw.WriteLine(string.Join(",", node.Values));
        foreach (TreeNode child in node.Children)
        {
            this.ExportTreeNodeToCSV(sw, child);
        }
    }
}

// TreeNode.cs
public class TreeNode
{
    public object[] Values;              // 节点数据（对应CSV的一行）
    public List<TreeNode> Children;      // 子节点
}
```

**关键点**:
- TreeModel是一个通用的树形数据结构
- 支持递归导出为CSV格式
- 每个TreeNode.Values数组对应CSV的一行

---

### 2️⃣ **Statistics系统架构**

```
UI Layer (View)
    ↓
StatisticsController
    ↓
IStatistic.GenerateViewModels(captureID)
    ↓
TreeViewStatisticDisplayViewModel
    ↓ Model属性
TreeModel
    ↓ ExportToCSV()
CSV文件
```

**代码流程**:

```csharp
// StatisticsController.cs
private void SelectedStatisticStateChanged(IStatistic stat)
{
    int captureID = this.m_view.SelectedCaptureID;
    
    // 在后台线程中生成ViewModels
    this.m_worker = new Thread(delegate
    {
        // 关键调用：生成统计数据的ViewModel
        IStatisticDisplayViewModel[] statVM = stat.GenerateViewModels(captureID);
        
        // 更新UI显示
        this.m_view.InvalidateOutputArea(stat, statVM);
    });
    this.m_worker.Start();
}

// TreeViewStatisticDisplayViewModel.cs
public class TreeViewStatisticDisplayViewModel : IStatisticDisplayViewModel
{
    public TreeModel Model { get; set; }
    
    public void ExportToCSV(StreamWriter sw)
    {
        this.Model.ExportToCSV(sw);
    }
}
```

---

### 3️⃣ **IStatistic实现（推测）**

GUI中每种统计项（如"Per DrawCall Performance"）都实现了`IStatistic`接口：

```csharp
public interface IStatistic
{
    string Name { get; }
    string Category { get; }
    StatisticDisplayType[] AvailableDisplays { get; }
    
    // 核心方法：生成ViewModels
    IStatisticDisplayViewModel[] GenerateViewModels(int captureID);
}
```

**推测的实现**（在QGLPlugin.dll等插件中）:

```csharp
public class VulkanDrawCallStatistic : IStatistic
{
    public string Name => "Per DrawCall Performance";
    
    public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
    {
        // 1. 创建TreeModel
        TreeModel model = new TreeModel(new Type[] 
        { 
            typeof(string),  // DrawCall #
            typeof(string),  // Function Name
            typeof(long),    // Write Total
            // ... 其他列
        });
        
        model.ColumnNames = new string[] 
        { 
            "#", "Name", "Write Total (Bytes)", 
            "Vertices Shaded", "% Texture L1 Miss", 
            // ... 其他列名
        };
        
        // 2. 从数据库查询数据
        DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
        Model captureModel = dataModel.GetModel("CaptureManager");
        
        // 可能查询的表：
        // - QGLApiTracePackets (API调用记录)
        // - SCOPEDrawStages (DrawCall GPU阶段)
        // - SCOPEDrawStageMetrics (性能指标)
        // - VulkanSnapshotGraphicsPipelines (Pipeline信息)
        
        // 3. 构建TreeNode层级
        //    Frame 1
        //      ↓ Submission 1
        //        ↓ DrawCall 1.1.1
        //        ↓ DrawCall 1.1.2
        //        ...
        
        TreeNode frameNode = new TreeNode();
        TreeNode submissionNode = new TreeNode();
        
        foreach (var drawCall in drawCallData)
        {
            TreeNode dcNode = new TreeNode();
            dcNode.Values = new object[]
            {
                "1.1.1",                    // DrawCall #
                "vkCmdDispatch",            // Function name
                393952,                     // Write Total
                0,                          // Vertices Shaded
                47.3484,                    // % Texture L1 Miss
                // ... 其他性能数据
                "( commandBuffer = 923 ... )", // Parameters
                "0x2"                       // Thread ID
            };
            
            submissionNode.Children.Add(dcNode);
        }
        
        frameNode.Children.Add(submissionNode);
        model.Nodes.Add(frameNode);
        
        // 4. 返回ViewModel
        return new IStatisticDisplayViewModel[]
        {
            new TreeViewStatisticDisplayViewModel 
            { 
                Model = model 
            }
        };
    }
}
```

---

### 4️⃣ **ImportCapture的角色**

```csharp
// Application.cs (CLI)
bool result = snapshotView
    ?.ImportCapture(captureId, captureId, dbPath, 1, 0, 9524) ?? false;
```

**ImportCapture做了什么**:

1. **读取.sdp文件** (实际是ZIP格式)
   - 解压缩
   - 提取sdp.db数据库
   - 提取其他资源

2. **Replay Snapshot数据**
   - 重建Vulkan对象状态
   - 填充数据库表：
     - `VulkanSnapshotGraphicsPipelines`
     - `VulkanSnapshotTextures`
     - `VulkanSnapshotDescriptorSetBindings`
     - 等等...

3. **触发Statistics计算**（GUI模式）
   - 插件（如QGLPlugin.dll）监听ImportCapture完成事件
   - 插件读取数据库数据
   - 生成Statistics（TreeModel）
   - 注册到StatisticsModel

4. **等待完成**
   - ImportCapture是**异步**的
   - CLI通过轮询数据库表行数来判断完成

---

### 5️⃣ **数据流向图**

```
Capture阶段 (运行时):
┌─────────────────────────┐
│ 应用程序执行            │
│ - vkCmdDraw*            │
│ - vkCmdDispatch         │
└───────────┬─────────────┘
            │ Profiler拦截
            ↓
┌─────────────────────────┐
│ 记录API调用序列         │
│ - 函数名                │
│ - 参数                  │
│ - 执行顺序              │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ 收集性能数据            │
│ - GPU Scope              │
│ - Metrics               │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ 保存到.sdp文件          │
│ (ZIP + sdp.db)          │
└─────────────────────────┘

ImportCapture阶段 (分析时):
┌─────────────────────────┐
│ ImportCapture(dbPath)   │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ 解压.sdp文件            │
│ 加载sdp.db              │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ Replay Snapshot         │
│ - 重建Vulkan对象        │
│ - 填充数据库表          │
└───────────┬─────────────┘
            │
            ↓ (GUI模式)
┌─────────────────────────┐
│ 插件生成Statistics      │
│ IStatistic.Generate...  │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ 构建TreeModel           │
│ - 查询数据库            │
│ - 组织节点层级          │
│ - 填充Values            │
└───────────┬─────────────┘
            │
            ↓
┌─────────────────────────┐
│ 用户导出CSV             │
│ TreeModel.ExportToCSV() │
└─────────────────────────┘
```

---

### 6️⃣ **关键差异：Trace vs Snapshot**

| 数据项 | Trace模式 | Snapshot模式 |
|--------|----------|--------------|
| API调用序列 | ✅ 记录在QGLApiTracePackets | ❌ 不记录（表为空） |
| DrawCall编号 | ✅ 可从m_callID推断 | ❌ 需GUI内存维护 |
| 性能指标 | ✅ SCOPEDrawStages等表 | ⚠️ 通常为空 |
| Statistics生成 | ✅ 从数据库完整恢复 | ⚠️ 可能不完整 |

**report3-13.csv的特殊性**:
- 这是**Capture时生成的数据**，不是ImportCapture生成的
- 包含562个DrawCall的完整序列
- 数据来源：API Trace（Trace模式）+ GPU Scope

---

### 7️⃣ **CLI无法复制GUI的原因**

#### A. Statistics插件不可用
```
GUI环境:
  - 加载QGLPlugin.dll
  - 实现VulkanDrawCallStatistic
  - 自动生成TreeModel

CLI环境:
  - 没有加载插件系统
  - 没有Statistics框架
  - 无法生成TreeModel
```

#### B. 数据不在数据库中
```sql
-- CLI能访问的:
SELECT * FROM VulkanSnapshotGraphicsPipelines;  -- ✅ 有数据
SELECT * FROM VulkanSnapshotTextures;           -- ✅ 有数据

-- CLI访问不到的（Snapshot模式下为空）:
SELECT * FROM QGLApiTracePackets;  -- ❌ 0 rows
SELECT * FROM SCOPEDrawStages;      -- ❌ 0 rows
```

#### C. DrawCall编号在内存中
- DrawCall# ("1.1.1", "1.1.2", ...) 是GUI运行时生成的
- 基于API调用的执行顺序
- **不写入数据库**
- CLI无法从数据库恢复这个顺序

---

### 8️⃣ **如何获取类似数据（CLI方案）**

#### 方案1: 解析GUI导出的CSV
```csharp
// 优点：获取真实的DrawCall编号和顺序
// 缺点：依赖GUI先导出CSV

public class CSVDrawCallMapping
{
    public static Dictionary<string, DrawCallData> ParseCSV(string csvPath)
    {
        var mapping = new Dictionary<string, DrawCallData>();
        
        foreach (string line in File.ReadLines(csvPath).Skip(1))
        {
            string[] fields = line.Split(',');
            string drawCallNumber = fields[0].Trim();
            string functionName = fields[1].Trim();
            
            // 从Parameters字段提取信息
            string parameters = fields[15];
            // 解析 ( commandBuffer = 923 ... )
            
            mapping[drawCallNumber] = new DrawCallData
            {
                Number = drawCallNumber,
                FunctionName = functionName,
                Parameters = parameters
                // ... 性能数据
            };
        }
        
        return mapping;
    }
}
```

#### 方案2: 使用Trace模式捕获
```csharp
// 改用Trace capture type
captureController.SetCaptureType(CaptureType.Trace);  // 2 instead of 4

// Trace模式会填充：
// - QGLApiTracePackets (API调用序列)
// - SCOPEDrawStages (DrawCall阶段数据)
// - 可以从数据库恢复DrawCall顺序
```

#### 方案3: 当前已实现 - 按Pipeline查询
```csharp
// 优点：直接从数据库查询
// 缺点：无法获取DrawCall执行顺序

var analyzer = new DrawCallAnalysis(dbPath);

// 遍历所有Pipelines（按resourceID排序）
for (int i = 0; i < pipelineCount; i++)
{
    uint[] textures = analyzer.GetTexturesForDrawCall($"{i}");
    // 但这不是真实的DrawCall顺序
}

// 或者直接用Pipeline ID
uint[] textures = DrawCallAnalysisTool.GetTextureIDsByPipelineID(dbPath, 875);
```

---

## 总结

### **关键发现：Snapshot模式的DrawCall数据来源**

#### .sdp文件结构（已验证）
```
3-13-xiaomi12.sdp (ZIP格式, 345MB)
├── sdp.db (457MB) - Snapshot静态对象数据
│   ├── VulkanSnapshotGraphicsPipelines (3223行)
│   ├── VulkanSnapshotTextures (3886行)
│   ├── QGLApiTracePackets (0行) ❌ 空表！
│   └── CaptureMetrics (13行)
├── sdpframe_002.gfxrz (169MB) - GFXReconstruct完整trace ⭐ 关键！
├── sdpframestripped_002.gfxrz (36MB) - GFXReconstruct stripped
└── version.txt
```

#### GUI导出CSV的完整流程（已确认）

1. **用户加载.sdp文件**
   ```
   GUI: File → Open → 3-13-xiaomi12.sdp
   ```

2. **ImportCapture解压文件**
   ```csharp
   snapshotView.ImportCapture(captureId, dbPath)
   // 解压.sdp → sdp.db + sdpframe_002.gfxrz
   ```

3. **VulkanProcessor Replay .gfxrz**（关键步骤！）
   ```
   日志证据:
   VulkanProcessor: QGLPluginProcessor received BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA
   VulkanProcessor: QGLPluginProcessor received BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA
   VulkanProcessor: Drawcall metrics data received (ver: 20)
   VulkanProcessor: Snapshot drawcall metrics transfer and processing: 2127ms
   ```

   **VulkanProcessor做的事**:
   - Replay sdpframe_002.gfxrz（GFXReconstruct trace文件）
   - 从trace中提取：
     - DrawCall序列 (1.1.1, 1.1.2, ..., 1.1.562)
     - 函数名 (vkCmdDispatch, vkCmdDraw, vkCmdDrawIndexed)
     - 参数 (commandBuffer, groupCountX, indexCount等)
     - **性能metrics** (BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
   - **在内存中构建DrawCall列表**（不写入数据库！）

4. **生成Statistics TreeModel**
   ```csharp
   // QGLPlugin.dll (VulkanProcessor内部)
   IStatistic.GenerateViewModels(captureID)
   {
       TreeModel model = new TreeModel(...);
       
       // 从replay结果（内存）中读取DrawCall数据
       foreach (var drawCall in replayedDrawCalls)  // ← 来自gfxrz replay
       {
           TreeNode node = new TreeNode();
           node.Values = [
               "1.1.1",              // DrawCall# (从trace replay得到)
               "vkCmdDispatch",      // 函数名
               393952,               // Write Total (从metrics得到)
               ...                   // 其他性能数据
           ];
           model.Nodes.Add(node);
       }
       
       return TreeViewStatisticDisplayViewModel { Model = model };
   }
   ```

5. **用户导出CSV**
   ```
   GUI: Statistics → Export to CSV
   TreeModel.ExportToCSV(streamWriter);
   ```

#### 为什么数据库中QGLApiTracePackets表为空？

**答案**：Snapshot模式不将API trace写入数据库，而是保存为.gfxrz文件！

| 数据存储位置 | Trace模式 | Snapshot模式 |
|-------------|----------|--------------|
| API调用序列 | → QGLApiTracePackets表 | → .gfxrz文件 ⭐ |
| 静态对象 | → VulkanSnapshot*表 | → VulkanSnapshot*表 |
| DrawCall性能 | → SCOPEDrawStages表 | → .gfxrz + replay ⭐ |

**设计原因**：
- .gfxrz文件更紧凑（压缩）
- 支持完整replay和分析
- 数据库只存储静态快照对象
- Replay时动态提取DrawCall信息

### TreeModel生成流程（修正）

1. **Capture阶段**: 
   - 录制Vulkan API调用 → **sdpframe_002.gfxrz**
   - 快照静态对象 → **sdp.db**
   - 打包为.sdp文件

2. **ImportCapture**: 
   - 解压.sdp文件
   - 加载sdp.db到内存
   - **Replay .gfxrz文件**（VulkanProcessor）

3. **VulkanProcessor Replay**（关键！）:
   - 解析.gfxrz trace文件
   - 重现DrawCall执行序列
   - 提取性能metrics
   - **在内存中构建DrawCall列表**

4. **Plugin生成TreeModel** (GUI):
   - IStatistic.GenerateViewModels()
   - 从replay结果（内存）读取DrawCall数据
   - 构建TreeModel结构
   - 填充TreeNode.Values数组

5. **UI显示**: TreeViewStatisticDisplayViewModel.Model

6. **导出CSV**: TreeModel.ExportToCSV()

### CLI的局限（修正）

#### ❌ CLI无法复制的原因

1. **VulkanProcessor是闭源DLL**
   - 只在GUI环境中工作
   - Replay .gfxrz后的DrawCall数据保存在C++插件内存中
   - 没有API暴露给C#

2. **无法访问Replay结果**
   - ImportCapture调用确实replay了.gfxrz
   - 但DrawCall序列数据在VulkanProcessor.dll内部
   - CLI无法获取这个内存数据

3. **数据库中没有DrawCall序列**
   - QGLApiTracePackets: 0行
   - SCOPEDrawStages: 0行
   - Snapshot模式设计就是如此

#### ✅ CLI能做的

1. **查询静态对象**:
   ```csharp
   - VulkanSnapshotGraphicsPipelines
   - VulkanSnapshotTextures
   - VulkanSnapshotDescriptorSetBindings
   ```

2. **解析GUI导出的CSV**:
   ```csharp
   // 从report3-13.csv中提取DrawCall信息
   var mapping = ParseCSV("report3-13.csv");
   ```

3. **直接解析.gfxrz文件**（理论可行，但需要工具）:
   - 使用gfxrecon-replay工具
   - 或自己解析.gfxrz格式

### report3-13.csv的来源（最终答案）

- **文件**: 3-13-xiaomi12.sdp
- **模式**: Snapshot模式
- **生成方式**: 
  1. GUI加载.sdp
  2. VulkanProcessor replay sdpframe_002.gfxrz
  3. 从replay中提取562个DrawCall
  4. 构建TreeModel（内存）
  5. 用户手动导出 → report3-13.csv
- **数据来源**: .gfxrz文件（不是数据库）
- **格式**: TreeModel.ExportToCSV()输出

---

## 参考文件

- `dll/project/SDPClientFramework/Sdp/TreeModel.cs` - TreeModel实现
- `dll/project/SDPClientFramework/Sdp/TreeNode.cs` - TreeNode结构
- `dll/project/SDPClientFramework/Sdp/StatisticsController.cs` - Statistics控制器
- `dll/project/SDPClientFramework/Sdp/IStatistic.cs` - Statistics接口
- `dll/project/SDPClientFramework/Sdp/TreeViewStatisticDisplayViewModel.cs` - ViewModel
- `SDPCLI/source/DrawCallAnalysis.cs` - CLI的替代实现
