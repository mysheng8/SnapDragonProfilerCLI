---
type: finding
topic: 3D model reconstruction from vertex buffer and index buffer
status: investigated
related_paths:
  - SDPCLI/source/Models/VulkanSnapshotModel.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - dll/project/QGLPlugin/ResourcesViewMgr.cs
  - SDPCLI/SNAPSHOT_DATA_FLOW_ANALYSIS.md
related_tags:
  - vertex-buffer
  - index-buffer
  - 3d-model
  - mesh-extraction
  - OBJ
  - VulkanSnapshotByteBuffers
summary: |
  调研从 index buffer 和 vertex buffer 重建 3D 模型的可行性。
  已有的基础设施已能定位 VB/IB 资源 ID，关键未知量是 VulkanSnapshotByteBuffers
  表中是否实际保存了 vertex/index buffer 的二进制数据。
last_updated: 2026-04-03
---

# 调研：从 IndexBuffer 和 VertexBuffer 重建 3D 模型

## 1. 已有基础设施（可直接复用）

### 1.1 DrawCall 绑定的 VB/IB 资源 ID

`DrawCallVertexBuffers` 和 `DrawCallIndexBuffers` 两张数据库表已经存在并被查询：

```sql
-- 顶点缓冲区：取 DrawCallApiID 对应的所有绑定
SELECT Binding, BufferID FROM DrawCallVertexBuffers WHERE DrawCallApiID = ?

-- 索引缓冲区：取一个 DrawCall 对应的
SELECT BufferID, Offset, IndexType FROM DrawCallIndexBuffers WHERE DrawCallApiID = ? LIMIT 1
```

`DrawCallQueryService.cs` 中已有 `GetVertexBuffers()` 和 `GetIndexBuffer()` 实现，返回
`VertexBufferBinding` / `IndexBufferBinding` 对象（包含 `BufferID`, `Offset`, `IndexType`）。

### 1.2 Pipeline Vertex Input State（顶点格式描述）

`VulkanSnapshotModel.ProcessCreateGraphicsPipelines()` 已从 API Trace 解析
`vkCreateGraphicsPipelines.pVertexInputState`，填充 `_pipelineVertexInputStates` 字典：

```csharp
public class VulkanPipelineVertexInputState
{
    public ulong PipelineId { get; set; }
    public List<VertexInputBinding> Bindings { get; set; }     // stride, inputRate
    public List<VertexInputAttribute> Attributes { get; set; } // location, binding, format, offset
}
```

`VertexInputAttribute.Format` 是 `VkFormat` 枚举值（例如 `106 = VK_FORMAT_R32G32B32_SFLOAT`），
直接告诉解析器每个顶点属性的类型和大小。

另外，数据库中也有专用表：
- `VulkanSnapshotInputBindingDescriptions` — 与 pipelineID 关联的 binding stride/inputRate
- `VulkanSnapshotInputAttributeDescriptions` — 与 pipelineID 关联的 location/format/offset

### 1.3 DrawCall 绘制参数

`DrawCallParameters` 表已存有：
```
vkCmdDraw        → vertexCount, instanceCount, firstVertex, firstInstance
vkCmdDrawIndexed → indexCount, instanceCount, firstIndex, vertexOffset, firstInstance
```

这些参数决定从 VB/IB 取多少顶点和索引来重建网格。

### 1.4 VulkanSnapshotByteBuffers — 通用二进制 BLOB 表

GUI (`ResourcesViewMgr.cs`) 通过同一个 `ByteBufferGateway` 从 `VulkanSnapshotByteBuffers`
读取多种资源：
- 纹理（TextureExtractor 已实现）
- Shader 二进制（ShaderExtractor 已实现）
- Tensor 数据（TensorHelper）
- **Memory Buffers**（category 1，`DisplayMemoryBuffers()` → `m_byteBufferGateway.GetByteBuffer()`）

查询模式统一：
```sql
SELECT data FROM VulkanSnapshotByteBuffers
WHERE captureID = ? AND resourceID = ?
ORDER BY sequenceID
```

---

## 2. 关键未知量：VB/IB Binary Data 是否存入数据库

### 2.1 根本问题

Snapdragon Profiler 已确认将以下数据写入 `VulkanSnapshotByteBuffers`：
- 纹理 BLOB
- Shader SPIR-V binary
- 通过 Descriptor Set 绑定的 Buffer（Uniform / Storage Buffer）

**待证实**：vertex buffer 和 index buffer（通过 `vkCmdBindVertexBuffers` / `vkCmdBindIndexBuffer` 绑定，
而非通过 Descriptor Set 绑定）的原始 binary data 是否同样写入 `VulkanSnapshotByteBuffers`。

### 2.2 证据链分析

**支持"已存入"的证据**：
1. SNAPSHOT_DATA_FLOW_ANALYSIS.md 第 1.1 节明确记录：
   ```
   记录 Texture/Buffer 数据 → BLOB 数据
   ```
   措辞是 `Texture/Buffer`，暗示 buffer binary 也被捕获。
2. 捕获选项 `Collect Buffer Data: 1` 已启用（SNAPSHOT_OPTIONS.md）。
3. GUI `DisplayMemoryBuffers()` 通过 `ByteBufferGateway` 读取 buffer binary，
   说明至少部分 buffer binary 已在库中。

**不确定性来源**：
1. `DisplayMemoryBuffers()` 的 category 1 (Memory Buffers) 是否包含 VB/IB，
   还是仅包含 Descriptor-bound Uniform/Storage Buffer，需要实际查询验证。
2. `VulkanSnapshotBuffers` 表存储 `vkCreateBuffer` 的元数据，
   `VulkanSnapshotMemoryBuffers` 存储内存分配信息，
   但这两张表不包含实际 binary data。
3. VB/IB 的 `resourceID`（来自 `DrawCallVertexBuffers.BufferID`）是否
   有对应行在 `VulkanSnapshotByteBuffers` 中，**必须实测**。

### 2.3 验证方法（需要 Executor 执行）

对已有 .sdp 文件 (`SDPCLI/test/` 下任意一个) 执行：

```powershell
# 1. 解压 sdp.db
$sdp = "SDPCLI\test\2026-04-02T17-03-18.sdp"
Add-Type -Assembly System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::OpenRead($sdp)
$entry = $zip.Entries | Where-Object {$_.Name -eq "sdp.db"}
$entry.ExtractToFile("C:\temp\sdp_test.db", $true)
$zip.Dispose()

# 2. 查一个 DrawCall 的 VB BufferID
sqlite3 C:\temp\sdp_test.db "SELECT DrawCallApiID, Binding, BufferID FROM DrawCallVertexBuffers LIMIT 5"

# 3. 用那个 BufferID 查 ByteBuffers 表
sqlite3 C:\temp\sdp_test.db "SELECT captureID, resourceID, length(data) FROM VulkanSnapshotByteBuffers WHERE resourceID = <found BufferID>"

# 4. 对比 VulkanSnapshotBuffers 表的记录
sqlite3 C:\temp\sdp_test.db "SELECT captureID, resourceID, size, usageFlags FROM VulkanSnapshotBuffers LIMIT 20"
```

---

## 3. 重建流程（基于已有信息推导）

假设 VB/IB binary 可获取，完整重建流程如下：

```
DrawCall ApiID
  │
  ├─► DrawCallIndexBuffers → (BufferID, Offset, IndexType)
  │       │
  │       └─► VulkanSnapshotByteBuffers[BufferID] → raw bytes
  │               │
  │               └─► 按 IndexType (UINT16/UINT32) 读取索引数组
  │
  ├─► DrawCallVertexBuffers → [{Binding, BufferID}, ...]
  │       │
  │       └─► 对每个 binding: VulkanSnapshotByteBuffers[BufferID] → raw bytes
  │
  ├─► DrawCallBindings.PipelineID
  │       │
  │       └─► VulkanSnapshotInputAttributeDescriptions[pipelineID]
  │               → [{location, binding, format, offset}, ...]
  │               → 告知如何解释 VB binary（如 location=0 = position, format=R32G32B32_SFLOAT）
  │
  └─► DrawCallParameters → indexCount, firstIndex, vertexOffset
          │
          └─► 切片范围：从 firstIndex 取 indexCount 个索引，
                        vertex offset = vertexOffset
```

**顶点格式解析（VkFormat → 字节宽度）**：
```
VK_FORMAT_R32G32B32_SFLOAT    (106) → 3 × float = 12 字节 (xyz position)
VK_FORMAT_R32G32_SFLOAT       (103) → 2 × float =  8 字节 (uv)
VK_FORMAT_R32G32B32A32_SFLOAT (109) → 4 × float = 16 字节 (tangent/color)
VK_FORMAT_R8G8B8A8_UNORM       (37) → 4 × byte  =  4 字节 (packed normal)
VK_FORMAT_R16G16_SFLOAT        (83) → 2 × half  =  4 字节 (uv compressed)
```

**输出格式**：Wavefront OBJ（`.obj`）是最简单可行的选择：
- 无外部依赖，纯文本格式
- 支持 position (v), normal (vn), texcoord (vt), face (f)
- 所有 3D 软件（Blender, Maya, MeshLab）均可直接导入

---

## 4. 实现复杂度评估

| 步骤 | 复杂度 | 备注 |
|------|--------|------|
| 读取 IB binary → 索引数组 | 低 | IndexType 已知（UINT16/UINT32），直接 BinaryReader |
| 读取 VB binary → 顶点 struct | 中 | 需要依据 VkFormat 动态解析每个 attribute |
| 识别 Position/Normal/UV attribute | 中 | 需要启发式（location=0 ≈ position，format 推断） |
| 输出 OBJ 文件 | 低 | 标准文本格式，无需第三方库 |
| 验证 VB/IB binary 实际可取 | **高风险** | 这是整个功能的前提，需要实测 |

---

## 5. 对比已知限制

| 已知问题 | 3D 模型提取的影响 |
|----------|------------------|
| Snapshot 只捕获单帧 | 只能提取该帧绑定的 mesh，不影响功能目标 |
| Skinned mesh 在 GPU 上蒙皮 | 提取到的是预蒙皮前的绑定姿 T-pose mesh，非当前帧形态 |
| Instanced draw calls | `instanceCount > 1`: 同一 mesh 多次绘制，只输出一份几何 |
| Indirect draw calls | `indexCount = 0`，需读 indirect buffer（复杂度高，建议跳过） |
| Position 在 DynamicBuffer 中 | 某些 GPU skinned mesh 顶点可能来自 compute shader 输出，VB binary 中无 position |
