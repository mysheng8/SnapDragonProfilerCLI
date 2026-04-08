---
type: plan
topic: 3D model extraction from vertex/index buffers + analysis pipeline mesh integration
status: partially-implemented
based_on:
  - FINDING-2026-04-03-3d-model-extraction-feasibility.md
  - FINDING-2026-04-08-mesh-export-pipeline-integration.md
related_paths:
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Modes/MeshExtractionMode.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Models/VulkanSnapshotModel.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/SDPCLI/config.ini
related_tags:
  - vertex-buffer
  - index-buffer
  - 3d-model
  - mesh-extraction
  - OBJ
  - drawcall-analysis
  - parallel
  - json
  - pipeline-integration
summary: |
  Phases 0-5 implemented: MeshExtractor.cs (OBJ from VB/IB binary) and MeshExtractionMode.cs
  (standalone CLI mode) are fully functional. Phase 6 integrates mesh export into the batch
  analysis pipeline: session-shared meshes/ folder, all non-compute DCs with VB bindings,
  per-file existence check, parallel Parallel.ForEach, and mesh_files field in
  DrawCallAnalysis JSON.
last_updated: 2026-04-08
---

# 计划：从 Vertex Buffer / Index Buffer 提取 3D 模型

## 前提条件（BLOCKER）

**必须先执行验证实验**（见 FINDING 第 2.3 节），确认
`VulkanSnapshotByteBuffers` 中确实存有 vertex/index buffer 的 binary data。

若验证失败（无数据），则本 Plan 需要转向 **从 gfxr 文件解析**（复杂度极高，暂不考虑）。

---

## 整体设计原则

与 TextureExtractor / ShaderExtractor 保持一致：
- 新增 `MeshExtractor.cs`（位于 `source/Tools/`）
- 新增 `MeshExtractionMode.cs`（位于 `source/Modes/`）
- 不修改现有数据库查询逻辑，在 DrawCallQueryService 上叠加使用

---

## Phase 0：验证（必须先完成）

**目标**：确认技术前提。

执行以下 SQL 对 `SDPCLI/test/` 下的任意 `.sdp` 文件验证：

```sql
-- Step 1: 取一个有 IndexBuffer 的 DrawCall
SELECT d.DrawCallApiID, ib.BufferID, ib.IndexType
FROM DrawCallIndexBuffers ib
JOIN DrawCallParameters d ON d.DrawCallApiID = ib.DrawCallApiID
LIMIT 5;

-- Step 2: 查找 BufferID 在 ByteBuffers 表是否有数据
SELECT captureID, resourceID, length(data) as dataBytes
FROM VulkanSnapshotByteBuffers
WHERE resourceID = <ib.BufferID from Step 1>;

-- Step 3: 查找 VertexBuffer
SELECT vb.Binding, vb.BufferID, length(bb.data) as dataBytes
FROM DrawCallVertexBuffers vb
LEFT JOIN VulkanSnapshotByteBuffers bb ON bb.resourceID = vb.BufferID
WHERE vb.DrawCallApiID = <same DrawCallApiID>
  AND bb.captureID = 3;
```

**预期结果 A（乐观）**：`dataBytes > 0` → 实施 Phase 1-4。  
**预期结果 B（悲观）**：所有 `dataBytes` 为 NULL → VB/IB binary 未存入数据库，需要替代方案。

---

## Phase 1：新建 MeshExtractor 工具类

**文件**：`SDPCLI/source/Tools/MeshExtractor.cs`

### 1.1 类结构

```csharp
public class MeshExtractor
{
    private readonly string _databasePath;
    private readonly int _captureId;

    public MeshExtractor(string databasePath, int captureId = 3) { ... }

    /// <summary>
    /// 提取单个 DrawCall 的 mesh 并输出 OBJ 文件。
    /// drawCallNumber 支持 "1.1.5" 或纯整数 ApiID。
    /// </summary>
    public bool ExtractMesh(string drawCallNumber, string outputPath) { ... }

    /// <summary>
    /// 批量提取多个 DrawCall 到同一目录，文件名 = drawcall_<apiID>.obj
    /// </summary>
    public int ExtractAllMeshes(string outputDir, int maxDrawCalls = 50) { ... }
}
```

### 1.2 ExtractMesh 内部流程

```
1. DrawCallQueryService.GetDrawCallInfo(dbPath, captureId, drawCallNumber)
   → DrawCallInfo { VertexBuffers, IndexBuffer, PipelineID, IndexCount, VertexCount }

2. 读取 Pipeline Vertex Input State（优先从数据库表）
   → VulkanSnapshotInputAttributeDescriptions → [{location, binding, format, offset}]
   → VulkanSnapshotInputBindingDescriptions  → [{binding, stride}]

3. 若无 IndexBuffer 或 VertexBuffers 为空 → 返回失败

4. 读取 IndexBuffer binary
   SELECT data FROM VulkanSnapshotByteBuffers
   WHERE captureID = ? AND resourceID = ?
   → 按 IndexType (UINT16/UINT32) 解析索引数组

5. 对每个 VertexBufferBinding:
   SELECT data FROM VulkanSnapshotByteBuffers
   WHERE captureID = ? AND resourceID = ?
   → binding 对应的 stride 来自 VertexInputBinding
   → 按每个 attribute 的 format/offset 解析属性值

6. 识别关键 attribute（启发式）:
   - POSITION  : location==0 或 format==R32G32B32_SFLOAT
   - NORMAL    : location==1 或 format==R32G32B32_SFLOAT (第二个)
   - TEXCOORD  : format==R32G32_SFLOAT 或 R16G16_SFLOAT

7. 按 firstIndex/indexCount/vertexOffset (from DrawCallParameters) 裁切范围

8. 写出 OBJ 文件（见 Phase 2）
```

---

## Phase 2：VkFormat → 字节解析

**关键映射表**（需在 MeshExtractor 内实现）：

```csharp
private static int GetFormatByteSize(uint vkFormat) => vkFormat switch
{
    100 => 4,   // VK_FORMAT_R32_SFLOAT
    103 => 8,   // VK_FORMAT_R32G32_SFLOAT
    106 => 12,  // VK_FORMAT_R32G32B32_SFLOAT       ← 最常见 position/normal
    109 => 16,  // VK_FORMAT_R32G32B32A32_SFLOAT    ← tangent
     37 => 4,   // VK_FORMAT_R8G8B8A8_UNORM         ← packed color/normal
     83 => 4,   // VK_FORMAT_R16G16_SFLOAT          ← compressed UV
     97 => 8,   // VK_FORMAT_R16G16B16A16_SFLOAT
    _   => 0
};

private static float[] ReadFloatAttribute(byte[] vbData, int vertexOffset, uint attrOffset, uint vkFormat)
{
    int byteOffset = vertexOffset + (int)attrOffset;
    using var ms = new MemoryStream(vbData, byteOffset, GetFormatByteSize(vkFormat));
    using var br = new BinaryReader(ms);
    return vkFormat switch
    {
        106 => new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },   // xyz
        103 => new[] { br.ReadSingle(), br.ReadSingle() },                    // xy
        109 => new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
         83 => new[] { HalfToFloat(br.ReadUInt16()), HalfToFloat(br.ReadUInt16()) },
        ...
    };
}
```

---

## Phase 3：OBJ 写出器

```csharp
private static void WriteObjFile(string path, ObjMesh mesh)
{
    using var writer = new StreamWriter(path);
    writer.WriteLine($"# Extracted by SDPCLI");
    writer.WriteLine($"# DrawCall: {mesh.DrawCallId}  Vertices: {mesh.Vertices.Count}  Faces: {mesh.Faces.Count}");
    writer.WriteLine();

    // Vertices
    foreach (var v in mesh.Vertices)
        writer.WriteLine($"v {v.X:F6} {v.Y:F6} {v.Z:F6}");

    // Normals
    if (mesh.Normals.Count > 0)
        foreach (var n in mesh.Normals)
            writer.WriteLine($"vn {n.X:F6} {n.Y:F6} {n.Z:F6}");

    // UVs
    if (mesh.TexCoords.Count > 0)
        foreach (var uv in mesh.TexCoords)
            writer.WriteLine($"vt {uv.X:F6} {uv.Y:F6}");

    // Faces (triangles, OBJ is 1-based)
    writer.WriteLine();
    bool hasNormals = mesh.Normals.Count > 0;
    bool hasUvs = mesh.TexCoords.Count > 0;
    for (int i = 0; i + 2 < mesh.Indices.Count; i += 3)
    {
        int a = mesh.Indices[i] + 1;
        int b = mesh.Indices[i + 1] + 1;
        int c = mesh.Indices[i + 2] + 1;
        if (hasNormals && hasUvs)
            writer.WriteLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
        else if (hasNormals)
            writer.WriteLine($"f {a}//{a} {b}//{b} {c}//{c}");
        else
            writer.WriteLine($"f {a} {b} {c}");
    }
}
```

---

## Phase 4：新建 MeshExtractionMode

**文件**：`SDPCLI/source/Modes/MeshExtractionMode.cs`

仿照 `TextureExtractionMode.cs`：

```
参数：
  -mode extract-mesh
  -sdp <path>         .sdp 文件路径
  -drawcall <id>      指定 DrawCall（"1.1.5" 或 ApiID），不指定则批量导出
  -output <dir>       输出目录（默认 ./mesh_output/）
  -capture-id <n>     Capture ID（默认 3）
  -max-drawcalls <n>  批量导出时的上限（默认 50）

流程：
  1. 解压 sdp.db（与 TextureExtractionMode 相同）
  2. new MeshExtractor(dbPath, captureId).ExtractMesh(drawCall, output)
  3. 打印结果统计
```

---

## Phase 5：注册 CLI 参数

**文件**：`SDPCLI/source/Main.cs`（或 Application.cs，需看 Mode 注册位置）

添加 case `"extract-mesh"` 分支，实例化 `MeshExtractionMode`。

---

## 已知风险与降级策略

### 风险 1：VB/IB binary 未在数据库中
- **概率**：中（取决于 Profiler 版本和捕获选项）
- **降级**：输出 mesh 骨架（顶点 = 0,0,0）+ 索引结构报告，供调试分析用
- **更彻底的降级**：解析 .gfxr 文件（RAW API trace），复杂度极高，暂不规划

### 风险 2：顶点属性识别错误（position 不在 location=0）
- **降级**：输出原始字节解释报告给用户，让用户通过 -position-location 参数指定
- 也可以输出所有 attribute 候选，让用户选择

### 风险 3：GPU skinned mesh（position 来自 compute 输出）
- **特征**：VB binding 的数据全为 0 或 VB binary 不存在
- **处理**：检测并打印警告，输出空 OBJ + 原因说明

### 风险 4：Indirect draw / `indexCount = 0`
- **处理**：直接跳过，输出警告 "IndirectDraw: index count unknown"

### 风险 5：Primitive Topology 不是 TRIANGLE_LIST
- 常见还有 TRIANGLE_STRIP：需要 strip→fan 展开
- 降级：只支持 TRIANGLE_LIST，其他 topology 打印警告并跳过

---

## 实现优先级

| 优先级 | 内容 | 预估工作量 |
|--------|------|-----------|
| P0（前提） | Phase 0 验证 SQL | 10 分钟 |
| P1 | MeshExtractor - 基础框架 + IB 解析 | 2-3 小时 |
| P2 | MeshExtractor - VB 解析 + OBJ 输出 | 3-4 小时 |
| P3 | MeshExtractionMode.cs + Main.cs 注册 | 1 小时 |
| P4 | PipelineVertexInputState 从 DB 表读取（替代 API trace 解析） | 1 小时 |
| P5 | TRIANGLE_STRIP 支持 | 1 小时 |

---

## 依赖关系

```
Phase 0 (验证) ─────────────► Phase 1-5 (全部实现)
                                    │
Phase 1 (MeshExtractor 框架) ──────► Phase 2 (VkFormat 解析)
                                    │
Phase 2 ──────────────────────────► Phase 3 (OBJ 写出)
                                    │
Phase 3 ──────────────────────────► Phase 4 (Mode 集成)
                                    │
Phase 4 ──────────────────────────► Phase 5 (Main.cs 注册)
```

---

## Implementation Status (Phases 0–5)

All original phases are **implemented** as of 2026-04-08:

- `MeshExtractor.cs` — fully functional (VB/IB decode, OBJ writer, thread-safe independent SQLite per call)
- `MeshExtractionMode.cs` — standalone `-mode extract-mesh` CLI with batch and single-DC modes
- Step 3.5 in `AnalysisPipeline.cs` — already calls `MeshExtractor` (top-5 DCs, per-capture, serial)

See `FINDING-2026-04-08-mesh-export-pipeline-integration.md` for the gap analysis.

---

## Phase 6 — Analysis Pipeline Mesh Integration

Upgrades Step 3.5 from "top-5, per-capture, serial" to
"all non-compute DCs, session-shared, parallel, JSON-annotated".

### 6a. AnalysisPipeline.cs — Move meshBaseDir to sessionDir

```csharp
// BEFORE
string meshBaseDir = Path.Combine(captureOutDir, "meshes");
// AFTER
string meshBaseDir = Path.Combine(sessionDir, "meshes");
```

### 6b. AnalysisPipeline.cs — Expand scope + per-file check + parallel

Move Step 3.5 BEFORE the JSON generation call (Step 3), then replace body:

```csharp
Directory.CreateDirectory(meshBaseDir);
var meshDcs = report.DrawCallResults
    .Where(dc => dc.ApiName.IndexOf("Dispatch",
        StringComparison.OrdinalIgnoreCase) < 0 && dc.VertexBuffers.Count > 0)
    .ToList();

int meshOk = 0;
Parallel.ForEach(meshDcs,
    new ParallelOptions { MaxDegreeOfParallelism = _config.MeshExtractionDegree },
    dc =>
    {
        string objPath = Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj");
        if (File.Exists(objPath)) { Interlocked.Increment(ref meshOk); return; }
        var ext = new Tools.MeshExtractor(dbPath!, (int)captureId);
        if (ext.ExtractMesh(dc.DrawCallNumber, objPath))
            Interlocked.Increment(ref meshOk);
    });
logger.Info($"  -> Meshes: {meshOk}/{meshDcs.Count} OBJ files -> {meshBaseDir}");
```

Key points:
- File naming: `mesh_{dc.ApiID}.obj` — ApiID is `uint`, globally unique, no special chars
- `MeshExtractor` opens its own SQLite connections per call — safe at any concurrency degree
- `MeshExtractionDegree` default = 4 (I/O-bound, same as `TextureExtractionDegree`)

### 6c. AnalysisPipeline.cs — Pass meshBaseDir to JSON generator

```csharp
string labeledJson = reportService.GenerateLabeledMetricsJson(
    report, captureOutDir, shaderBaseDir, textureBaseDir, meshBaseDir);
```

### 6d. ReportGenerationService.cs — Add meshBaseDir + mesh_files

Signature:
```csharp
public string GenerateLabeledMetricsJson(
    DrawCallAnalysisReport report, string captureOutDir,
    string shaderBaseDir, string textureBaseDir,
    string meshBaseDir)    // NEW
```

Per-DC resolution inside the loop:
```csharp
bool hasMesh = dc.ApiName.IndexOf("Dispatch", StringComparison.OrdinalIgnoreCase) < 0
    && dc.VertexBuffers.Count > 0
    && File.Exists(Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj"));

string[] meshFiles = hasMesh
    ? new[] { $"../../meshes/mesh_{dc.ApiID}.obj" }
    : Array.Empty<string>();
```

Add to the DC anonymous object:
```csharp
mesh_files     = meshFiles,
vertex_buffers = dc.VertexBuffers.Select(vb => new
    { binding = vb.Binding, buffer_id = vb.BufferID }).ToArray(),
index_buffer   = dc.IndexBuffer == null ? null : (object)new
    { buffer_id = dc.IndexBuffer.BufferID, index_type = dc.IndexBuffer.IndexType },
```

Bump `schema_version` from `"2.0"` to `"2.1"`.

### 6e. Resulting JSON structure (schema_version 2.1)

```json
{
  "schema_version": "2.1",
  "drawcalls": [{
    "dc_id": "1.1.31",
    "api_name": "vkCmdDrawIndexed",
    "pipeline_id": 1234,
    "shader_files":   ["../../shaders/pipeline_1234_vert.hlsl"],
    "texture_files":  ["../../textures/texture_456.png"],
    "mesh_files":     ["../../meshes/mesh_106974.obj"],
    "vertex_buffers": [{"binding": 0, "buffer_id": 5678}],
    "index_buffer":   {"buffer_id": 9012, "index_type": "UINT16"},
    "vertex_count":   5832,
    "index_count":    8748
  }]
}
```

### 6f. config.ini

```ini
MeshExtractionDegree=4
```

Add alongside `TextureExtractionDegree=4`.

### 6g. viewer.html

`GenerateMeshViewerHtml` only needs its path updated from `captureOutDir/meshes/`
to `sessionDir/meshes/`. No logic change required.

---

## Affected Files Summary (Phase 6)

| File | Change |
|------|--------|
| `AnalysisPipeline.cs` | `meshBaseDir` to `sessionDir/meshes/`; all non-compute DCs with VBs; per-file check; `Parallel.ForEach`; reorder before Step 3 |
| `ReportGenerationService.cs` | Add `meshBaseDir` param; `mesh_files`, `vertex_buffers`, `index_buffer` per DC; `schema_version` to `"2.1"` |
| `Config.cs` | Add `MeshExtractionDegree` int property (default 4) |
| `config.ini` | Add `MeshExtractionDegree=4` |

## Implementation requires the Executor agent.
