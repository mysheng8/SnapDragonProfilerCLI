# Snapdragon Profiler Snapshot 数据流分析

## 文档目的
本文档详细记录了 Snapdragon Profiler 从数据采集到保存的完整流程，GUI 的实现机制，以及 SDPCLI 在实现 per-DrawCall 资源绑定查询时遇到的问题和解决方案。

---

## 1. 数据采集到保存流程

### 1.1 Capture 阶段（设备端）

```
Android 设备运行 App
    ↓
App 调用 Vulkan API (vkDraw*, vkCmdBindDescriptorSets, etc.)
    ↓
Qualcomm Profiler Layer 拦截 API 调用
    ↓
记录 API 调用参数 → gfxr 文件（API trace）
    ↓
记录 DescriptorSet 绑定 → 原始绑定数据（未关联 DrawCall）
    ↓
记录 GPU Metrics → 性能数据
    ↓
记录 Texture/Buffer 数据 → BLOB 数据
    ↓
通过 TCP 传输到 PC 端 SDPCore
```

**关键特征**：
- **Snapshot 模式**：只捕获单帧数据
- **数据分离**：API trace（gfxr）与资源数据（BLOB）分开存储
- **未绑定 apiID**：DescriptorSet 绑定数据的 `apiID` 字段全是 `0xFFFFFFFF`（无效值）

---

### 1.2 OnDataProcessed 阶段（PC 端）

```
SDPCore 接收数据
    ↓
触发 OnDataProcessed 回调（多次，每个 buffer 一次）
    ↓
写入 session 目录
    ├─ *.gfxr 文件（API trace）
    ├─ *.bin 文件（资源数据）
    └─ sdp.db（数据库，空表）
    ↓
SimpleClientDelegate 捕获 BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA
    ↓
存储到 QGLPlugin.SnapshotDsbBuffer（全局静态 BinaryDataPair）
```

**数据库状态**：此时数据库表为空，只有原始 gfxr 和 bin 文件。

---

### 1.3 ImportCapture 阶段（Replay）

#### 1.3.1 调用触发

**GUI 触发方式**：
```csharp
// GUI: QGLPlugin.connectionEvents_OpenSnapshotFromSession
ProcessorPluginMgr.Get()
    .GetPlugin("SDP::QGLPluginProcessor")
    ?.ImportCapture(captureId, captureId, dbPath, 1, 0, 9524);
```

**SDPCLI 触发方式**（已实现）：
```csharp
// Application.cs Line 695
ProcessorPluginMgr.Get()
    .GetPlugin("SDP::QGLPluginProcessor")
    ?.ImportCapture(captureId, captureId, dbPath, 1, 0, 9524);
```

#### 1.3.2 ImportCapture 工作流程

```
ImportCapture 调用（ASYNC）
    ↓
C++ VulkanProcessor 插件开始 replay gfxr
    ↓
解析 gfxr 文件中的 API 调用
    ├─ vkCreateGraphicsPipeline → VulkanSnapshotGraphicsPipelines 表
    ├─ vkCreateImageView → VulkanSnapshotImageViews 表
    ├─ vkCreateBuffer → VulkanSnapshotBuffers 表
    ├─ vkUpdateDescriptorSets → 填充 DescriptorSet 绑定
    └─ vkCmdDraw* → 关联 DrawCall 和绑定
    ↓
更新 SnapshotDsbBuffer (QGLPlugin.SnapshotDsbBuffer)
    ├─ apiID 字段从 0xFFFFFFFF 更新为真实的 DrawCall ID
    ├─ 结构体：DescSetBindings.DescBindings
    └─ 字段：captureID, apiID, descriptorSetID, slotNum, imageViewID, bufferID, etc.
    ↓
写入数据库表
    ├─ VulkanSnapshotGraphicsPipelines
    ├─ VulkanSnapshotImageViews
    ├─ VulkanSnapshotTextures
    ├─ VulkanSnapshotBuffers
    ├─ VulkanSnapshotDescriptorSetBindings（但 apiID 仍然无效！）
    └─ VulkanSnapshotByteBuffers（图片 BLOB）
    ↓
触发 OpenSnapshotFromSessionResult 事件（GUI 监听）
```

**关键问题**：
- **数据库的 `VulkanSnapshotDescriptorSetBindings` 表 apiID 无效**（0xFFFFFFFF）
- **SnapshotDsbBuffer 内存中的 apiID 有效**（已关联 DrawCall）
- 必须在 ImportCapture 完成后**立即**读取 buffer（buffer 可能被清空）

---

## 2. GUI 实现机制

### 2.1 架构概览

```
QGLPlugin.dll (同一程序集)
├─ QGLPlugin 类
│  ├─ internal static VkSnapshotModel { get; set; }  ← 核心数据模型
│  ├─ public static SnapshotDsbBuffer  ← DescriptorSet 绑定数据
│  └─ QGLPlugin() 构造函数
│     └─ QGLPlugin.VkSnapshotModel = new VkSnapshotModel();
│
├─ DataExplorerViewMgr  ← API Call Tree 视图
│  └─ 触发 VkAPITreeModelBuilder.ProcessAllCalls()
│
├─ ResourcesViewMgr  ← Resource 视图
│  └─ 查询 VkSnapshotModel.DrawcallsPerResource
│
└─ VkAPITreeModelBuilder  ← API Trace Replay
   └─ 解析 gfxr 并填充 VkSnapshotModel
```

### 2.2 VkSnapshotModel 数据结构

```csharp
namespace QGLPlugin
{
    public class VkSnapshotModel
    {
        // Key1: captureID, Key2: drawCallID, Value: 该 DrawCall 的绑定信息
        private Dictionary<uint, Dictionary<uint, VkBoundInfo>> m_drawCallInfos;
        
        // Key1: captureID, Key2: ResourceKey (categoryID + resourceID), Value: 使用该资源的 DrawCall 列表
        private Dictionary<int, Dictionary<ResourceKey, List<uint>>> m_drawcallsPerResource;
        
        // Key: drawCallID, Value: 该 DrawCall 绑定的资源列表
        private Dictionary<uint, List<PrepopulateCategoryArgs>> m_resourcesPerDrawcall;
        
        // Key: apiID, Value: 该 API 绑定的 DescriptorSetID 列表
        private Dictionary<uint, List<ulong>> DsbByApi;
    }
    
    public class VkBoundInfo
    {
        public ulong BoundPipeline;  // 绑定的 Pipeline ID
        public Dictionary<ulong, DescSetBindings> BoundDescriptorSets;  // 绑定的 DescriptorSets
        public HashSet<ulong> DescSetIDs;  // 所有 DescriptorSet ID
    }
    
    public class DescSetBindings
    {
        public ulong DescSetID;
        public Dictionary<ulong, DescBindings> Bindings;  // Key: slotNum
    }
    
    public struct DescBindings
    {
        public uint captureID;
        public uint apiID;              // ← 关键：DrawCall API ID
        public ulong descriptorSetID;
        public uint slotNum;
        public ulong imageViewID;       // ← 关键：Image 资源 ID
        public ulong samplerID;
        public ulong bufferID;
        // ...
    }
}
```

### 2.3 GUI 数据流

#### 2.3.1 加载 Snapshot 流程

```
用户打开 Snapshot
    ↓
GUI: QGLPlugin.connectionEvents_OpenSnapshotFromSession
    ↓
调用 ImportCapture (C++ 插件 replay gfxr)
    ↓
解析 SnapshotDsbBuffer（QGLPlugin.SnapshotDsbBuffer）
    ↓
VkSnapshotModel.LoadBindingDataFromSdpBuffer()
    └─ 遍历 buffer 中的所有 DescBindings 记录
       └─ 填充 AllDescSetBindings[descriptorSetID][slotNum] = descBindings
    ↓
VkAPITreeModelBuilder.ProcessAllCalls()
    └─ 遍历所有 API 调用（从 SnapshotApiBuffer）
       ├─ vkCmdBindDescriptorSets → 更新 m_currentBoundInfo.BoundDescriptorSets
       ├─ vkCmdBindPipeline → 更新 m_currentBoundInfo.BoundPipeline
       └─ vkCmdDraw* → AddDrawCallInfo(captureID, drawCallID, m_currentBoundInfo)
    ↓
填充完成：VkSnapshotModel.m_drawCallInfos[captureID][drawCallID] 包含完整绑定
```

#### 2.3.2 查询 per-DrawCall 资源流程

```
用户在 API Call Tree 中选择 DrawCall
    ↓
GUI: DataExplorerViewMgr.dataExplorerViewEvents_RowSelected
    ↓
触发 ResourcesViewMgr.Prepopulate()
    ↓
遍历所有 DrawCall
    └─ VkSnapshotModel.GetBoundInfo(captureID, drawCallID, out VkBoundInfo)
       └─ 返回 m_drawCallInfos[captureID][drawCallID]
    ↓
遍历 VkBoundInfo.BoundDescriptorSets
    └─ 遍历 DescSetBindings.Bindings
       └─ 提取 imageViewID, bufferID, samplerID
    ↓
构建 PrepopulateCategoryArgs
    └─ CategoryId = 0 (Images)
    └─ ResourceIds = [imageViewID1, imageViewID2, ...]
    ↓
触发 ResourceViewEvents.PrepopulateCategory 事件
    ↓
GUI 显示该 DrawCall 绑定的所有资源
```

#### 2.3.3 查询图片数据流程

```
用户选择/双击 Resource 列表中的 Image
    ↓
ResourcesViewMgr.resourceViewEvents_ItemDoubleClicked
    ↓
m_byteBufferGateway.GetByteBuffer(captureId, imageID)
    └─ ByteBufferGateway 查询数据库 VulkanSnapshotByteBuffers 表
    └─ 返回 IByteBuffer (包含 BinaryDataPair)
    ↓
提取图片元数据（从 VulkanSnapshotImageViews 表）
    ├─ width, height
    └─ format (VkFormats 枚举)
    ↓
Marshal.Copy(bdp.data, imageData, 0, bdp.size)  // IntPtr → byte[]
    ↓
VkHelper.GenerateThumbnail(imageData, format, width, height, 64, 64)
    ↓
触发 UpdateResourcePixBuf 事件 → GUI 显示缩略图
```

### 2.4 为什么 GUI 能访问 `internal static VkSnapshotModel`

**关键原因：程序集边界**

```
QGLPlugin.dll (同一个程序集)
├─ QGLPlugin 类
│  └─ internal static VkSnapshotModel { get; set; }  ← internal 修饰符
├─ ResourcesViewMgr 类  ← 同一程序集，✅ 可以访问
├─ DataExplorerViewMgr 类  ← 同一程序集，✅ 可以访问
└─ 所有 QGLPlugin namespace 下的类  ← 同一程序集，✅ 都可以访问

SDPCLI.exe (独立程序集)
└─ Application.cs  ← 跨程序集，❌ 无法访问 internal 成员
```

**C# 访问修饰符规则**：
- `public`: 任何程序集都可访问
- **`internal`**: **仅同一程序集内可访问**
- `private`: 仅同一类内可访问

---

## 3. 当前问题分析

### 3.1 核心问题

**SDPCLI 无法获取 per-DrawCall 的资源绑定关系**

#### 问题表现

1. **数据库中的 apiID 无效**：
   ```sql
   SELECT captureID, apiID, descriptorSetID, slotNum, imageViewID 
   FROM VulkanSnapshotDescriptorSetBindings 
   WHERE captureID = 3 AND imageViewID > 0;
   
   -- 结果：
   -- apiID 全部为 4294967295 (0xFFFFFFFF)
   -- 无法关联到具体的 DrawCall
   ```

2. **CSV 导出的数据也无效**：
   ```csv
   captureID,apiID,descriptorSetID,slotNum,imageViewID
   3,4294967295,12345,0,67890
   3,4294967295,12346,1,67891
   ```
   - `DrawCallBindingSaver.SaveBoundInfoToCSV` 读取的是 `SnapshotDsbBuffer`
   - Buffer 中的 apiID 在 ImportCapture 之前为 0xFFFFFFFF
   - ImportCapture 会更新 buffer，但数据库不会更新

3. **无法访问 VkSnapshotModel**：
   ```csharp
   // ❌ 编译错误：无法访问 internal 成员
   var boundInfo = QGLPlugin.VkSnapshotModel.GetBoundInfo(captureID, drawCallID, out info);
   ```

#### 根本原因

1. **Snapshot 模式设计限制**：
   - Snapshot 只捕获单帧状态，不记录完整的 API 调用序列
   - DescriptorSet 绑定数据在捕获时**未关联到具体 DrawCall**
   - 需要 **replay API trace** 才能重建绑定关系

2. **数据分离存储**：
   - API trace（gfxr）：包含完整的 API 调用序列
   - 绑定数据（SnapshotDsbBuffer）：包含 DescriptorSet 绑定，但 apiID 无效
   - 数据库（sdp.db）：ImportCapture 时填充，但 `VulkanSnapshotDescriptorSetBindings` 表的 apiID 不更新

3. **访问限制**：
   - `QGLPlugin.VkSnapshotModel` 是 `internal static` 属性
   - SDPCLI 在独立程序集中，无法直接访问
   - GUI 可以访问是因为在同一个 DLL（QGLPlugin.dll）内

---

## 4. 解决方案分析

### 方案 1：反射访问 VkSnapshotModel（Hack）

#### 实现思路

```csharp
using System.Reflection;

// 1. 加载 QGLPlugin 程序集
Assembly qglPluginAssembly = Assembly.Load("QGLPlugin");

// 2. 获取 QGLPlugin 类型
Type qglPluginType = qglPluginAssembly.GetType("QGLPlugin.QGLPlugin");

// 3. 获取 VkSnapshotModel 静态属性（需要 NonPublic 标志）
PropertyInfo vkSnapshotModelProp = qglPluginType.GetProperty(
    "VkSnapshotModel", 
    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

// 4. 读取属性值
object vkSnapshotModelObj = vkSnapshotModelProp.GetValue(null);

// 5. 调用 GetBoundInfo 方法
Type vkSnapshotModelType = vkSnapshotModelObj.GetType();
MethodInfo getBoundInfoMethod = vkSnapshotModelType.GetMethod("GetBoundInfo");

object[] parameters = new object[] { captureID, drawCallID, null };
getBoundInfoMethod.Invoke(vkSnapshotModelObj, parameters);
VkBoundInfo boundInfo = (VkBoundInfo)parameters[2];  // out 参数

// 6. 遍历绑定的资源
foreach (var descSet in boundInfo.BoundDescriptorSets)
{
    foreach (var binding in descSet.Value.Bindings)
    {
        ulong imageViewID = binding.Value.imageViewID;
        Console.WriteLine($"DrawCall {drawCallID} 绑定了 Image {imageViewID}");
    }
}
```

#### 优点
- ✅ 完全复用 GUI 的数据结构和逻辑
- ✅ 可以访问完整的 per-DrawCall 绑定信息
- ✅ 不需要重新解析 gfxr

#### 缺点
- ❌ 依赖反射，性能较差
- ❌ 代码脆弱，容易因 QGLPlugin 版本变化而失效
- ❌ 类型转换复杂（需要动态处理）
- ❌ 调试困难

#### 适用场景
- 快速原型验证
- 无法修改 QGLPlugin 源码时的临时方案

---

### 方案 2：等待 ImportCapture 后立即读取 SnapshotDsbBuffer（当前方案）

#### 实现思路

```csharp
// Application.cs 已实现的流程

// 1. 调用 ImportCapture（异步）
ProcessorPluginMgr.Get()
    .GetPlugin("SDP::QGLPluginProcessor")
    ?.ImportCapture(captureId, captureId, dbPath, 1, 0, 9524);

// 2. 等待 ImportCapture 完成（轮询数据库）
for (int i = 0; i < 90; i++)
{
    Thread.Sleep(1000);
    // 查询数据库表行数，判断是否稳定
}

// 3. 等待 SnapshotDsbBuffer 准备就绪
int bufferWaitTime = 0;
while (bufferWaitTime < 30)
{
    Thread.Sleep(1000);
    var testBuffer = ProcessorPluginMgr.Get()
        .GetPlugin("SDP::QGLPluginProcessor")
        ?.GetLocalBuffer(BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA, 3, captureId);
    
    if (testBuffer != null && testBuffer.size > 0)
    {
        // 4. 立即保存到 CSV（在 buffer 被清空之前）
        tempCSVPath = DrawCallBindingSaver.SaveBoundInfoToCSV(sessionPath, captureId, testBuffer);
        break;
    }
    bufferWaitTime++;
}

// 5. 解析 CSV 并重建绑定关系（TODO）
// 需要自己实现 SnapshotApiBuffer 解析逻辑
```

#### 当前状态

✅ **已完成**：
- ImportCapture 调用
- 数据库轮询等待
- SnapshotDsbBuffer 等待
- CSV 保存

❌ **未完成**：
- **SnapshotApiBuffer 解析**（关键）
  - 需要解析 API trace 重建 DrawCall 序列
  - 需要关联 vkCmdBindDescriptorSets 和 vkCmdDraw*
  - 需要构建 DrawCall → DescriptorSet 映射

#### 优点
- ✅ 不依赖 QGLPlugin 的 internal 成员
- ✅ 数据来源可靠（从 ImportCapture 生成的 buffer）
- ✅ 可以保存为 CSV 供用户检查

#### 缺点
- ❌ **核心问题**：CSV 中的 apiID 仍然是 0xFFFFFFFF
- ❌ 需要额外解析 SnapshotApiBuffer 才能关联 DrawCall
- ❌ 时序要求严格（buffer 可能随时被清空）
- ❌ 实现复杂度高（需要自己实现 replay 逻辑）

#### 当前问题

**关键卡点**：
```
SnapshotDsbBuffer 中的 apiID = 0xFFFFFFFF
    ↓
需要通过 SnapshotApiBuffer（API trace）重建 apiID
    ↓
需要实现类似 VkAPITreeModelBuilder.ProcessAllCalls() 的逻辑
    └─ 解析 vkCmdBindDescriptorSets
    └─ 解析 vkCmdBindPipeline
    └─ 解析 vkCmdDraw*
    └─ 关联绑定和 DrawCall
```

---

### 方案 3：自己解析 SnapshotApiBuffer 构建绑定关系（终极方案）

#### 实现思路

完全复刻 GUI 的 `VkAPITreeModelBuilder.ProcessAllCalls()` 逻辑：

```csharp
public class CliVkSnapshotModel
{
    private Dictionary<uint, Dictionary<uint, CliVkBoundInfo>> drawCallInfos;
    private Dictionary<ulong, DescSetBindings> allDescSetBindings;
    
    public void BuildFromBuffers(BinaryDataPair apiBuffer, BinaryDataPair dsbBuffer)
    {
        // 1. 解析 SnapshotDsbBuffer（DescriptorSet 绑定数据）
        LoadDescriptorSetBindings(dsbBuffer);
        
        // 2. 解析 SnapshotApiBuffer（API trace）
        ParseApiTrace(apiBuffer);
    }
    
    private void LoadDescriptorSetBindings(BinaryDataPair dsbBuffer)
    {
        // 复制 VkSnapshotModel.LoadBindingDataFromSdpBuffer 逻辑
        int structSize = Marshal.SizeOf<DescBindings>();
        long numRecords = dsbBuffer.size / structSize;
        IntPtr ptr = dsbBuffer.data;
        
        for (int i = 0; i < numRecords; i++)
        {
            DescBindings descBindings = Marshal.PtrToStructure<DescBindings>(ptr);
            ptr += structSize;
            
            // 存储到 allDescSetBindings
            if (!allDescSetBindings.TryGetValue(descBindings.descriptorSetID, out var descSet))
            {
                descSet = new DescSetBindings(descBindings.descriptorSetID);
                allDescSetBindings[descBindings.descriptorSetID] = descSet;
            }
            descSet.Bindings[(ulong)descBindings.slotNum] = descBindings;
        }
    }
    
    private void ParseApiTrace(BinaryDataPair apiBuffer)
    {
        // 复制 VkAPITreeModelBuilder.ProcessAllCalls 逻辑
        
        CliVkBoundInfo currentBoundInfo = new CliVkBoundInfo();
        
        // 解析 apiBuffer 中的所有 API 调用
        foreach (var api in ParseApiBuffer(apiBuffer))
        {
            if (api.name == "vkCmdBindPipeline")
            {
                // 更新当前绑定的 Pipeline
                currentBoundInfo.BoundPipeline = ExtractPipelineID(api.parameters);
            }
            else if (api.name == "vkCmdBindDescriptorSets")
            {
                // 更新当前绑定的 DescriptorSets
                var descSetIDs = ExtractDescriptorSetIDs(api.parameters);
                foreach (var descSetID in descSetIDs)
                {
                    if (allDescSetBindings.TryGetValue(descSetID, out var descSet))
                    {
                        currentBoundInfo.BoundDescriptorSets[descSetID] = descSet;
                    }
                }
            }
            else if (IsDrawCall(api.name))
            {
                // 保存当前 DrawCall 的绑定信息
                uint drawCallID = api.apiID;
                drawCallInfos[captureID][drawCallID] = new CliVkBoundInfo(currentBoundInfo);
            }
        }
    }
    
    public void GetBoundInfo(uint captureID, uint drawCallID, out CliVkBoundInfo info)
    {
        drawCallInfos[captureID].TryGetValue(drawCallID, out info);
    }
}
```

#### 关键步骤

1. **解析 SnapshotDsbBuffer**：
   ```csharp
   int structSize = Marshal.SizeOf<DescBindings>();
   IntPtr ptr = dsbBuffer.data;
   DescBindings binding = Marshal.PtrToStructure<DescBindings>(ptr);
   ```

2. **解析 SnapshotApiBuffer**：
   - 需要了解 API buffer 的二进制格式
   - 可能需要参考 QGLPlugin 的 C++ 代码
   - 或者通过 `ProcessorPluginMgr.GetPlugin()` 间接获取解析结果

3. **重建 DrawCall 序列**：
   ```
   遍历 API trace
   ├─ 遇到 vkCmdBindDescriptorSets → 更新 currentBoundInfo
   ├─ 遇到 vkCmdBindPipeline → 更新 currentBoundInfo
   └─ 遇到 vkCmdDraw* → 保存 currentBoundInfo 到 drawCallInfos[drawCallID]
   ```

4. **查询绑定**：
   ```csharp
   GetBoundInfo(captureID, drawCallID, out var boundInfo);
   foreach (var descSet in boundInfo.BoundDescriptorSets)
   {
       foreach (var binding in descSet.Bindings)
       {
           Console.WriteLine($"DrawCall {drawCallID} 绑定了 Image {binding.imageViewID}");
       }
   }
   ```

#### 优点
- ✅ 完全独立，不依赖 QGLPlugin 的 internal 成员
- ✅ 性能最优（直接操作内存，无反射开销）
- ✅ 可维护性好（清晰的数据结构）
- ✅ 可扩展（可以添加自定义分析逻辑）

#### 缺点
- ❌ 实现难度最高
- ❌ 需要深入理解 SnapshotApiBuffer 的二进制格式
- ❌ 需要复刻大量 GUI 代码（VkAPITreeModelBuilder）
- ❌ 维护成本高（需要跟随 SDK 版本更新）

#### 实现难点

1. **SnapshotApiBuffer 格式未知**：
   - 需要逆向工程或查阅文档
   - 可能包含版本标识、API ID、参数序列化等

2. **参数解析复杂**：
   - vkCmdBindDescriptorSets 参数需要解析 JSON 或二进制格式
   - 需要提取 `firstSet` 和 `pDescriptorSets` 数组

3. **状态跟踪困难**：
   - 需要维护当前绑定状态（Pipeline, DescriptorSets）
   - 需要处理 CommandBuffer 嵌套（Primary/Secondary）

---

## 5. 推荐方案对比

| 特性 | 方案1：反射 | 方案2：读 Buffer + CSV | 方案3：自己解析 |
|------|-----------|---------------------|--------------|
| **实现难度** | 低 | 中 | 高 |
| **性能** | 差（反射） | 中（轮询等待） | 优（直接内存） |
| **可靠性** | 低（依赖内部实现） | 中（buffer 时序风险） | 高（独立实现） |
| **可维护性** | 差（脆弱） | 中（需要同步更新） | 优（清晰架构） |
| **数据完整性** | ✅ 完整 | ❌ apiID 无效 | ✅ 完整 |
| **当前进度** | 未实现 | ✅ 部分实现 | 未实现 |

---

## 6. 推荐实施路线

### 短期方案（快速验证）

**方案 1：反射访问 VkSnapshotModel**

**目标**：快速验证 per-DrawCall 资源绑定查询功能

**步骤**：
1. 在 ImportCapture 完成后，通过反射访问 `QGLPlugin.VkSnapshotModel`
2. 调用 `GetBoundInfo(captureID, drawCallID, out boundInfo)`
3. 遍历 `boundInfo.BoundDescriptorSets` 提取资源 ID
4. 查询数据库获取资源详细信息（VulkanSnapshotImageViews）
5. 生成 per-DrawCall 资源绑定报告

**预计时间**：1-2 天

---

### 中期方案（优化性能）

**方案 2 改进：读取 Buffer + 解析 SnapshotApiBuffer**

**目标**：消除反射依赖，提升性能

**步骤**：
1. 保持当前的 CSV 保存逻辑（作为调试工具）
2. 实现 SnapshotApiBuffer 解析器（简化版）
   - 只解析 vkCmdBindDescriptorSets 和 vkCmdDraw* 调用
   - 构建 DrawCall → DescriptorSet 映射
3. 结合 SnapshotDsbBuffer 中的绑定数据
4. 生成完整的 DrawCall → Resource 映射

**预计时间**：1 周

---

### 长期方案（完全独立）

**方案 3：自己实现 CliVkSnapshotModel**

**目标**：消除对 QGLPlugin 的所有依赖

**步骤**：
1. 定义 CLI 专用的数据结构（CliVkBoundInfo, CliDescSetBindings）
2. 实现完整的 SnapshotApiBuffer 解析器
3. 实现状态跟踪逻辑（Pipeline/DescriptorSet 绑定）
4. 构建 DrawCall → Resource 完整映射
5. 提供查询 API：`GetResourcesByDrawCall(captureID, drawCallID)`

**预计时间**：2-3 周

---

## 7. 总结

### 核心问题

SDPCLI 无法获取 per-DrawCall 的资源绑定关系，因为：
1. 数据库中的 apiID 无效（0xFFFFFFFF）
2. 需要 replay API trace 才能重建绑定关系
3. GUI 的 VkSnapshotModel 是 internal 成员，无法直接访问

### 根本原因

Snapshot 模式的设计特点：
- 只捕获单帧状态，不记录完整 API 序列
- 绑定数据与 DrawCall 分离存储
- 必须通过 replay 才能关联

### 解决方向

三种方案各有优劣：
1. **反射访问**：快速但脆弱
2. **读取 Buffer**：中等难度，需补充 API 解析
3. **自己实现**：难度最高但最可靠

### 推荐路线

1. **短期**：使用反射快速验证功能
2. **中期**：实现简化版 API 解析器
3. **长期**：构建独立的数据模型

---

## 附录

### A. 关键数据结构

#### DescBindings（SnapshotDsbBuffer 中的记录）

```csharp
[StructLayout(LayoutKind.Sequential)]
public struct DescBindings
{
    public uint captureID;          // Capture ID
    public ulong descriptorSetID;   // DescriptorSet ID
    public uint apiID;              // ⚠️ API Call ID (Snapshot 模式下为 0xFFFFFFFF)
    public uint slotNum;            // Binding slot number
    public ulong samplerID;         // Sampler resource ID
    public ulong imageViewID;       // ⚠️ Image resource ID
    public uint imageLayout;        // Image layout
    public ulong bufferID;          // Buffer resource ID
    public ulong offset;            // Buffer offset
    public ulong range;             // Buffer range
    public ulong texBufferview;     // Texture buffer view
    public ulong accelStructID;     // Acceleration structure ID (raytracing)
    public ulong tensorID;          // Tensor ID (AI/ML)
    public ulong tensorViewID;      // Tensor view ID
}
```

### B. 数据库表结构

#### VulkanSnapshotDescriptorSetBindings

```sql
CREATE TABLE VulkanSnapshotDescriptorSetBindings (
    captureID INTEGER,
    apiID INTEGER,              -- ⚠️ 始终为 4294967295 (0xFFFFFFFF)
    descriptorSetID INTEGER,
    slotNum INTEGER,
    imageViewID INTEGER,        -- Image 资源 ID
    bufferID INTEGER,           -- Buffer 资源 ID
    samplerID INTEGER,          -- Sampler 资源 ID
    -- ...
);
```

#### VulkanSnapshotImageViews

```sql
CREATE TABLE VulkanSnapshotImageViews (
    captureID INTEGER,
    resourceID INTEGER,         -- Image View ID
    imageID INTEGER,
    width INTEGER,
    height INTEGER,
    format INTEGER,             -- VkFormat 枚举值
    viewType INTEGER,
    -- ...
);
```

#### VulkanSnapshotTextures

```sql
CREATE TABLE VulkanSnapshotTextures (
    captureID INTEGER,
    resourceID INTEGER,         -- Texture/Image ID
    width INTEGER,
    height INTEGER,
    depth INTEGER,
    format INTEGER,
    layerCount INTEGER,
    levelCount INTEGER,
    -- ...
);
```

#### VulkanSnapshotByteBuffers

```sql
CREATE TABLE VulkanSnapshotByteBuffers (
    captureID INTEGER,
    resourceID INTEGER,         -- Resource ID（与 imageViewID 对应）
    sequenceID INTEGER,
    offset INTEGER,
    data BLOB                   -- ⚠️ 图片原始二进制数据
);
```

### C. 关键静态属性位置

```
QGLPlugin.dll
└─ QGLPlugin.QGLPlugin
   ├─ internal static VkSnapshotModel VkSnapshotModel { get; set; }  (Line 74)
   ├─ public static BinaryDataPair SnapshotDsbBuffer { get; }  (Line 64)
   └─ public static BinaryDataPair SnapshotApiBuffer { get; }  (Line 52)
```

### D. 相关文件清单

- **Application.cs** (Line 647-920): ImportCapture 和 CSV 保存逻辑
- **DrawCallBindingSaver.cs**: SnapshotDsbBuffer 解析和 CSV 导出
- **QGLPlugin/QGLPlugin.cs** (Line 74-88): VkSnapshotModel 定义和初始化
- **QGLPlugin/VkSnapshotModel.cs** (Line 140-163): LoadBindingDataFromSdpBuffer
- **QGLPlugin/VkAPITreeModelBuilder.cs** (Line 229-260): GetDescSet 和 ProcessBindDescriptorSet
- **QGLPlugin/ResourcesViewMgr.cs** (Line 2650-2720): GUI 图片数据获取流程
