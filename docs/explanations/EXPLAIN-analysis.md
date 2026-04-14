---
type: explanation
topic: Analysis 模式完整实现
module_key: SDPCLI.Analysis
source_scope:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/
  - SDPCLI/source/Models/
module_index: docs/index/modules/SDPCLI.Analysis.md
based_on:
  - IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
  - IMPL-2026-04-09-rawjson-service-rename.md
  - IMPL-2026-04-09-metrics-dynamic-attribution.md
  - PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
  - FINDING-2026-04-08-parallelism-thread-safety.md
status: stable
audience:
  - self
  - onboarding
last_updated: 2026-04-14
---

# SDPCLI Analysis 模式：工作原理与实现详解

## What this document explains

本文档详细解析 Analysis 模式的完整实现：AnalysisPipeline 的 Pass A/B 全流程编排、AnalysisTarget 10 标志位系统与依赖级联、核心数据模型（DrawCallInfo / DrawCallMetrics / DrawCallLabel）、所有输出文件结构（dc.json / status.json / topdc.json 等）、三层瓶颈归因引擎、以及增量执行机制。

## Scope

- **Included**：AnalysisTarget 枚举与 ExpandWithDependencies、PassMode 粗粒度控制、RunAnalysis 完整调用链（Pre-flight → Pass A Steps 1~A6 → Pass B B1~B3）、DrawCallInfo/DrawCallMetrics/DrawCallLabel/资源类型完整 C# 模型、输出目录结构、dc.json/status.json/topdc.json 完整 JSON 示例、AttributionRuleEngine 三层归因、旧格式 SDP 兼容、completedTargets 增量执行、config.ini 键
- **Excluded**：Snapshot 模式的抓帧流程（见 EXPLAIN-snapshot.md）、Server 模式的 AnalysisJobRunner 阶段细节（见 EXPLAIN-server.md）、SDK DLL 架构（见 EXPLAIN-three-modes.md §二）

## Routing

- **ModuleKey**: SDPCLI.Analysis
- **Module Index**: [docs/index/modules/SDPCLI.Analysis.md](../index/modules/SDPCLI.Analysis.md)
- **SourceScope**: `SDPCLI/source/Analysis/AnalysisPipeline.cs`, `SDPCLI/source/Services/Analysis/`, `SDPCLI/source/Models/`

## Context Basis

- **Implementations**: IMPL-2026-04-08-drawanalysis-two-pass-refactor.md, IMPL-2026-04-09-rawjson-service-rename.md, IMPL-2026-04-09-metrics-dynamic-attribution.md
- **Plans**: PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
- **Findings**: FINDING-2026-04-08-parallelism-thread-safety.md

## Reality Status

- **Stable**: AnalysisPipeline Pass A/B 全流程、AnalysisTarget 依赖级联、DrawCallInfo 数据模型、三层归因引擎、JSON 输出格式
- **WIP**: 无
- **Outdated or conflicting**: PassMode 体系（StatsOnly/AnalysisOnly）是早期设计，与 AnalysisTarget 并存有重叠，新代码优先用 AnalysisTarget；`ReportGenerationService` 已重命名为 `RawJsonGenerationService`（旧 context 文档中引用旧名）

---

## 一、设计目标

Analysis 模式面向**离线场景**：接收一个 `.sdp` 文件（ZIP 格式），不须连接设备，全程基于文件操作，输出多层次 JSON + Markdown 报告。

核心设计决策：
- **两个 Pass（A/B）**：Pass A 做数据提取和基础统计，Pass B 做高层推理（LLM + 规则）
- **AnalysisTarget 标志位**：10 个细粒度标志控制哪些输出文件需要生成，支持增量重跑
- **依赖级联**：请求某个目标会自动引入所有前置目标
- **B-only 快捷路径**：若只请求 `analysis`/`dashboard` 且 `dc.json` 已存在，可跳过数据库查询，直接从磁盘加载

---

## 二、AnalysisTarget 标志位系统

```csharp
[Flags]
public enum AnalysisTarget
{
    None      = 0,
    Dc        = 1 << 0,   // → dc.json（DrawCall 基础数据）
    Shaders   = 1 << 1,   // → shaders.json + SPIR-V/HLSL 提取
    Textures  = 1 << 2,   // → textures.json + PNG 提取
    Buffers   = 1 << 3,   // → buffers.json + OBJ mesh 提取
    Label     = 1 << 4,   // → label.json（分类标注）
    Metrics   = 1 << 5,   // → metrics.json（GPU 计数器）
    Status    = 1 << 6,   // → status.json（百分位统计）
    TopDc     = 1 << 7,   // → topdc.json（Top-N + 归因）
    Analysis  = 1 << 8,   // → analysis.md（Pass B LLM 报告）
    Dashboard = 1 << 9,   // → dashboard.md（规则图表）
    All       = (1 << 10) - 1
}
```

### 依赖级联（ExpandWithDependencies）

```
请求      →  自动补充前置
─────────────────────────────────────────────────────────
analysis  →  TopDc
dashboard →  TopDc + Status
TopDc     →  Status
Status    →  Dc + Label + Metrics
Label     →  Dc + Shaders
Metrics   →  Dc
Shaders   →  Dc
Textures  →  Dc
Buffers   →  Dc
```

示例：`-t status` 实际执行 `dc + shaders + label + metrics + status`，无需用户手动枚举。

---

## 三、PassMode（粗粒度控制）

PassMode 是早期设计的粗粒度控制（3 档），与 AnalysisTarget 并存、有重叠。新代码优先用 AnalysisTarget，旧兼容路径保留 PassMode：

| PassMode | 行为 |
|---------|------|
| `All`（默认） | 完整 Pass A + Pass B |
| `StatsOnly` | 只跑 Pass A（Steps 1~A6），跳过 B1/B2/B3 |
| `AnalysisOnly` | 只跑 Pass B（B1/B2/B3），从磁盘加载已有 JSON |

---

## 四、完整调用链

### 4.1 入口

```
SDPCLI.exe analysis <path>.sdp [-s N] [-t targets]
  └── Application.Run("analysis", ...)
      └── AnalysisMode.Run()
          ├── SdpFileService.FindDatabasePath(sdpPath)
          │   └── 解压 sdp.db 到临时目录
          ├── 扫描 ZIP 内 snapshot_N/ 目录 → 列出可选 captureId
          └── AnalysisPipeline.RunAnalysis(dbPath, sessionDir, captureId, target)
```

### 4.2 AnalysisPipeline.RunAnalysis 完整流程

```
RunAnalysis(dbPath, sessionDir, captureId, target, completedTargets)
│
├── Pre-flight: SdpDatabase.ValidateForAnalysis()
│   └── 检查必要表是否存在（DrawCallParameters / DrawCallBindings 等）
│
├── [B-only 快捷路径检查]
│   如果 target ⊆ {Analysis, Dashboard} 且 dc.json 已存在
│   └── SubJsonLoadService.LoadFromSubJsons() → 加载已有 JSON 跳过 DB 查询
│
├── ─── PASS A：数据收集与提取 ──────────────────────────────
│
├── Step 1: 收集 DrawCall
│   DrawCallAnalysisService.AnalyzeAllDrawCalls(dbPath, captureId)
│   └── DatabaseQueryService.GetDrawCallIds(captureId, cmdBufFilter)
│       └── SELECT DrawCallApiID FROM DrawCallParameters WHERE CaptureID=x ORDER BY rowid
│           DrawCallQueryService.GetDrawCallInfoByApiId() × N
│           ├── DrawCallParameters      → ApiName, VertexCount, InstanceCount ...
│           ├── DrawCallBindings        → PipelineID, ImageViewID (WHERE CaptureID=x)
│           ├── DrawCallRenderTargets   → 附件类型/格式 (ColumnExists 检查 CaptureID)
│           ├── DrawCallVertexBuffers   → Binding, BufferID
│           ├── DrawCallIndexBuffers    → BufferID, Offset, IndexType
│           ├── VulkanSnapshotShaderStages → pipelineID, stageType, entryPoint, SPIR-V
│           ├── VulkanSnapshotImageViews   → imageViewID → resourceID (textureID)
│           └── VulkanSnapshotTextures     → width, height, format
│   → 返回 DrawCallAnalysisReport
│       .DrawCallResults = List<DrawCallInfo>
│       .Statistics = { TotalPipelines, TotalTextures, TotalShaders }
│
├── Step 1.5: 并行提取 Shaders + Textures
│   ├── Task A: Parallel.ForEach(uniquePipelines, MaxDegree=ProcessorCount)
│   │   └── ShaderExtractor.ExtractShadersForPipeline(pipelineId, shaderBaseDir)
│   │       ├── SELECT spirvBlob FROM VulkanSnapshotShaderStages WHERE captureID=x AND PipelineID=y
│   │       ├── 写入 shaders/pipeline_{id}_{stage}.spv
│   │       └── spirv-cross pipeline_{id}_{stage}.spv → pipeline_{id}_{stage}.hlsl
│   └── Task B: Parallel.ForEach(allTexIds, MaxDegree=4)
│       └── TextureExtractor.ExtractTexture(texId, texturePath)
│           ├── SELECT rawBytes FROM VulkanSnapshotByteBuffers WHERE captureID=x AND resourceID=texId
│           └── Qonvert DLL 解码 → textures/texture_{id}.png
│   Task.WaitAll(shaderTask, textureTask)
│
├── Step 2: 标注 DrawCall
│   Parallel.ForEach(report.DrawCallResults, MaxDegree=LlmMaxConcurrentRequests)
│   └── DrawCallLabelService.Label(dc, shaderBaseDir)
│       ├── [LLM 路径] 读取 shaders/pipeline_{id}_*.hlsl
│       │   → 构建 prompt → HTTP → ChatGPT/LocalLLM → 解析 {Category, Detail}
│       │   → 缓存到 _llmCache[pipelineID]（ConcurrentDictionary，防重复 LLM 调用）
│       └── [规则路径（兜底）] 关键词匹配 shader 入口点名称
│           → Shadow / UI / VFX / Character / Terrain / PostProcess / Scene
│
├── Step 3: 关联 GPU 指标
│   MetricsQueryService.LoadMetrics(dbPath, captureId)
│   └── SELECT DrawCallApiID, ApiName, DrawcallIdx FROM DrawCallParameters WHERE CaptureID=x
│       SELECT DrawID, MetricName, Value FROM DrawCallMetrics WHERE CaptureID=x
│       → join by DrawcallIdx → Dictionary<DrawCallApiID, DrawCallMetrics>
│   → 将 Metrics 写回 DrawCallInfo.Metrics
│
├── Step 3.5: 提取 Meshes（3D 几何）
│   Parallel.ForEach(非 Dispatch DC, MaxDegree=4)
│   └── MeshExtractor.ExtractMesh(drawCallNum, meshBaseDir/mesh_{apiId}.obj)
│       → 若 .obj 已存在则跳过
│
├── ─── PASS A 输出（Sub-JSON 生成）────────────────────────
│
├── WriteDcJson()        → snapshot_{id}/dc.json
├── WriteShadersJson()   → snapshot_{id}/shaders.json
├── WriteTexturesJson()  → snapshot_{id}/textures.json
├── WriteBuffersJson()   → snapshot_{id}/buffers.json
├── WriteLabelJson()     → snapshot_{id}/label.json
├── WriteMetricsJson()   → snapshot_{id}/metrics.json
│
├── Step A5: StatusJsonService.GenerateStatusJson()
│   → snapshot_{id}/status.json（百分位统计）
│   → 返回 StatusJsonResult（含 GlobalPercentiles + CategoryStatsMap，供 A6 直接使用，无需重读文件）
│
├── Step A6: TopDcJsonService.GenerateTopDcJson()
│   需要：AttributionRuleEngine（加载 analysis/attribution_rules.json）
│   → snapshot_{id}/topdc.json（Top-N DC + 三层归因）
│
├── WriteIndexJson()     → snapshot_{id}/snapshot_{id}_index.json（产出文件索引）
│
├── ─── PASS B：高层推理 ────────────────────────────────────
│
├── Step B1: DcContentAnalysisService.AnalyzeAll()
│   → 逐 DC 深度检查（纹理格式、管线状态、Shader 特征）
│   → 输出缓存到 per_dc_content/
│
├── Step B2: AttributionReportService.GenerateAnalysisMd()
│   [LLM 可选] 分类归因报告
│   → snapshot_{id}/analysis.md
│
└── Step B3: DashboardGenerationService.GenerateDashboard()
    [纯规则，无 LLM] 可视化图表
    → snapshot_{id}/dashboard.md
```

---

## 五、核心数据模型

### 5.1 DrawCallInfo（内存中的 DC 对象）

```csharp
public class DrawCallInfo
{
    // 标识符
    public string DrawCallNumber { get; set; }  // 人类可读编号（如 "106974"）
    public uint   ApiID          { get; set; }  // VkApiID（DrawCallParameters 主键）
    public string ApiName        { get; set; }  // "vkCmdDraw" | "vkCmdDispatch" | ...

    // 管线与布局
    public uint PipelineID { get; set; }
    public uint LayoutID   { get; set; }
    public uint RenderPass { get; set; }

    // 标注（DrawCallLabelService 填充）
    public DrawCallLabel Label { get; set; }    // Category + Detail + ConfidenceScore

    // GPU 指标（MetricsQueryService 填充）
    public DrawCallMetrics? Metrics { get; set; }

    // 绘制参数
    public uint VertexCount   { get; set; }
    public uint IndexCount    { get; set; }
    public uint InstanceCount { get; set; }

    // 资源绑定
    public uint[]                    TextureIDs    { get; set; }
    public List<TextureInfo>         Textures      { get; set; }
    public List<ShaderInfo>          Shaders       { get; set; }
    public List<RenderTargetInfo>    RenderTargets { get; set; }
    public List<VertexBufferBinding> VertexBuffers { get; set; }
    public IndexBufferBinding?       IndexBuffer   { get; set; }
    public DescriptorBindingSummary  BindingSummary{ get; set; }
}
```

### 5.2 DrawCallMetrics（GPU 计数器）

GPU 计数器通过 `DrawCallMetrics.csv` 导入，再由 `MetricsQueryService` 关联到 `DrawCallInfo`。

```csharp
public class DrawCallMetrics
{
    // 核心时钟
    public long Clocks { get; set; }              // GPU ALU 时钟周期

    // 带宽
    public long ReadTotalBytes  { get; set; }     // 总读取字节
    public long WriteTotalBytes { get; set; }     // 总写入字节

    // 着色器活跃率（百分比）
    public double ShadersBusyPct    { get; set; } // Shader ALU 忙碌比例
    public double TexFetchStallPct  { get; set; } // Texture fetch stall 比例
    public double TexL1MissPct      { get; set; } // L1 miss 率
    public double TexL2MissPct      { get; set; } // L2 miss 率

    // 几何
    public long FragmentsShaded { get; set; }     // 着色 fragment 数
    public long VerticesShaded  { get; set; }     // 着色顶点数

    // 指令数
    public long FragmentInstructions { get; set; }
    public long VertexInstructions   { get; set; }

    // 额外内存
    public long TexMemReadBytes    { get; set; }
    public long VertexMemReadBytes { get; set; }

    // 动态扩展（DB 中存在但上面未静态定义的指标）
    public Dictionary<string, double> All { get; set; }  // 所有原始键值（用于 JSON 输出）
}
```

> `All` 字典以 `NormalizeKey()` 处理（转小写 + 去空格），JSON 输出时按 key 排序，保证格式一致。

### 5.3 DrawCallLabel（标注结果）

```csharp
public class DrawCallLabel
{
    public string Category        { get; set; }   // "Shadow" | "UI" | "VFX" | "Scene" | ...
    public string Detail          { get; set; }   // 自由文本说明
    public double ConfidenceScore { get; set; }   // LLM 置信度（规则路径固定为 1.0）
    public string ReasonTag       { get; set; }   // "llm" | "rule_keyword" | "fallback"
}
```

内置规则关键词（shader 入口点名称匹配）：

| Category | 触发关键词（节选） |
|---------|----------------|
| Shadow | shadow, shadowmap, shadowcaster |
| UI | ui, hud, glyph, canvas, overlay |
| VFX | particle, vfx, emitter, ribbon, smoke |
| Character | character, skin, hair, body, player |
| PostProcess | blur, bloom, tonemap, ssao, composite, blit |
| Terrain | terrain, landscape, heightfield |
| Scene | sky, water, grass, scene, mesh, world |

LLM 路径使用 `_llmCache[pipelineID]`（`ConcurrentDictionary`）防止同一管线多 DC 重复调用 LLM。

### 5.4 资源类型

```csharp
// 纹理元数据（VulkanSnapshotTextures + ImageViews）
public class TextureInfo
{
    public uint   TextureID  { get; set; }
    public uint   Width      { get; set; }
    public uint   Height     { get; set; }
    public uint   Depth      { get; set; }
    public string FormatName { get; set; }   // "VK_FORMAT_R8G8B8A8_UNORM" 等
}

// Shader stage（VulkanSnapshotShaderStages）
public class ShaderInfo
{
    public uint   ShaderModuleID   { get; set; }
    public string ShaderStageName  { get; set; }  // "Vertex" | "Fragment" | "Compute"
    public string EntryPoint       { get; set; }
}

// RT 附件（DrawCallRenderTargets）
public class RenderTargetInfo
{
    public uint   RenderPassID          { get; set; }
    public uint   FramebufferID         { get; set; }
    public uint   AttachmentIndex       { get; set; }
    public uint   AttachmentResourceID  { get; set; }
    public string AttachmentType        { get; set; }  // "Color" | "Depth" | "Stencil"
    public uint   Width      { get; set; }
    public uint   Height     { get; set; }
    public string FormatName { get; set; }
}

// 顶点/索引缓冲（DrawCallVertexBuffers / IndexBuffers）
public class VertexBufferBinding { public uint Binding; public uint BufferID; }
public class IndexBufferBinding  { public uint BufferID; public uint Offset; public string IndexType; }
```

---

## 六、输出文件结构

所有文件写入 `<ProjectDir>/analysis/<sdp_basename>/snapshot_{captureId}/`（分析 session 目录）。Shader/Texture/Mesh 写在 session 级共享目录（避免每个 captureId 都复制一份）。

```
analysis/<sdp_basename>/
│
├── shaders/                              ← 共享，session 级（所有 captureId 共用）
│   ├── pipeline_12345_Vertex.spv        ← SPIR-V 原始字节码
│   ├── pipeline_12345_Vertex.hlsl       ← spirv-cross 转译结果
│   ├── pipeline_12345_Fragment.spv
│   └── pipeline_12345_Fragment.hlsl
│
├── textures/                             ← 共享，session 级
│   ├── texture_9001.png
│   └── texture_9002.png
│
├── meshes/                               ← 共享，session 级
│   ├── mesh_106974.obj                  ← 按 DrawCall ApiID 命名
│   └── viewer.html                      ← 自动生成的 3D 预览页
│
└── snapshot_2/                           ← 每个 captureId 独立
    ├── dc.json                           ← Pass A: DrawCall 基础数据（含资源引用）
    ├── shaders.json                      ← Pass A: 管线 → shader 文件路径映射
    ├── textures.json                     ← Pass A: 纹理列表 + 格式信息
    ├── buffers.json                      ← Pass A: VB/IB/Mesh 引用
    ├── label.json                        ← Pass A: DC 分类标注
    ├── metrics.json                      ← Pass A: GPU 计数器（按 DC 展开）
    ├── status.json                       ← Pass A5: 百分位统计（全局 + 按类别）
    ├── topdc.json                        ← Pass A6: Top-N DC + 三层归因
    ├── snapshot_2_index.json             ← 索引清单（所有产出文件路径）
    ├── analysis.md                       ← Pass B2: LLM 归因 Markdown 报告
    ├── dashboard.md                      ← Pass B3: 规则图表 Markdown 报告
    └── per_dc_content/                   ← Pass B1: 逐 DC 深度分析缓存
        ├── dc_106974.json
        └── ...
```

### 6.1 dc.json 结构

每个条目代表一个 DrawCall，包含所有资源的文件引用（相对路径 `../../shaders/`）：

```jsonc
[
  {
    "dc_id":          "106974",      // DrawCallNumber（人类可读）
    "api_id":         106974,        // VkApiID
    "api_name":       "vkCmdDraw",
    "pipeline_id":    12345,
    "layout_id":      67,
    "render_pass_id": 89,
    "vertex_count":   1536,
    "index_count":    0,
    "instance_count": 1,

    // Shader 信息
    "shader_stages": [
      { "stage": "Vertex",   "module_id": 1001, "entry_point": "main", "file": "../../shaders/pipeline_12345_Vertex.hlsl" },
      { "stage": "Fragment", "module_id": 1002, "entry_point": "psMain", "file": "../../shaders/pipeline_12345_Fragment.hlsl" }
    ],
    "shader_files": [ "../../shaders/pipeline_12345_Vertex.hlsl", "../../shaders/pipeline_12345_Fragment.hlsl" ],

    // 纹理信息
    "texture_ids": [9001, 9002],
    "texture_files": [ "../../textures/texture_9001.png", "../../textures/texture_9002.png" ],
    "textures": [
      { "texture_id": 9001, "width": 1024, "height": 1024, "depth": 1, "format": "VK_FORMAT_BC3_UNORM_BLOCK" }
    ],

    // RT 附件
    "color_render_targets": [ { "resource_id": 301, "width": 2400, "height": 1080, "format": "VK_FORMAT_R8G8B8A8_UNORM" } ],
    "depth_render_targets": [ { "resource_id": 302, "format": "VK_FORMAT_D24_UNORM_S8_UINT" } ],

    // Vertex 几何
    "vertex_buffers": [ { "binding": 0, "buffer_id": 555 } ],
    "mesh_file": "../../meshes/mesh_106974.obj",

    // 标注
    "label": { "category": "Scene", "detail": "Opaque world geometry", "reason_tag": "rule_keyword" },

    // GPU 指标（若有）
    "metrics": {
      "clocks": 12450,
      "read_total_bytes": 3145728,
      "shaders_busy_pct": 87.3,
      "tex_fetch_stall_pct": 12.1
    }
  }
]
```

### 6.2 status.json 结构（Pass A5）

```jsonc
{
  "capture_id": 2,
  "sdp_name": "2026-04-11T09-50-42",
  "overall": {
    "total_dc_count": 312,
    "draw_dc_count":  280,
    "compute_dc_count": 32,
    "total_clocks": 18540000,
    "total_read_bytes": 25165824,
    "metrics_coverage": 0.95,          // 有 Metrics 的 DC 比例
    "global_percentiles": {
      "clocks_p50": 8200, "clocks_p80": 45000, "clocks_p95": 120000,
      "tex_fetch_stall_pct_p80": 18.5
    }
  },
  "category_stats": [
    {
      "category": "Scene",
      "count": 140,
      "total_clocks": 9800000,
      "avg_clocks": 70000,
      "metrics_p80": { "clocks": 110000, "tex_fetch_stall_pct": 22.0 },
      "metrics_p95": { "clocks": 180000, "tex_fetch_stall_pct": 35.0 }
    }
  ],
  "label_quality": {
    "llm_count": 200, "rule_count": 80, "fallback_count": 32,
    "avg_confidence": 0.82
  }
}
```

### 6.3 topdc.json 结构（Pass A6）

Top-N（默认 5）个每类高 Clocks DC，附带三层归因结果：

```jsonc
[
  {
    "category": "Scene",
    "top_dcs": [
      {
        "dc_id": "106974",
        "rank": 1,
        "clocks": 180000,
        "clocks_rank_in_category": 1,
        "metrics": { "clocks": 180000, "tex_fetch_stall_pct": 35.2, ... },
        "attribution": {
          "primary_bottleneck":   "TextureBandwidth",
          "secondary_bottleneck": "ShaderALU",
          "primary_score": 0.67,
          "hints": ["tex_fetch_stall_pct exceeds p80", "tex_l2_miss_pct exceeds p95"]
        },
        "category_comparison": {
          "clocks_pct_rank":      97,    // 在 Scene 类中的百分位排名
          "tex_stall_pct_rank":   92
        },
        "shader_files": [ "../../shaders/pipeline_12345_Fragment.hlsl" ],
        "mesh_file":    "../../meshes/mesh_106974.obj"
      }
    ]
  }
]
```

### 6.4 三层归因引擎（AttributionRuleEngine）

来源：`analysis/attribution_rules.json`

- **Layer 1（metric → bottleneck hint）**：每个指标超过 p70 阈值时触发对应 bottleneck hint
- **Layer 2（百分位档位，权重）**：p70 / p80 / p95 对应不同权重；若类别样本数 < min_sample_size 则回退到全局百分位
- **Layer 3（bottleneck 加权求和）**：每个 bottleneck（TextureBandwidth / ShaderALU / MemoryBandwidth / ...）汇总所有贡献指标的 weighted_score，取最高者为 primary

---

## 七、旧格式 SDP 兼容

| 场景 | 处理方式 |
|------|---------|
| `DrawCallRenderTargets` 表无 `CaptureID` 列 | `DrawCallQueryService.ColumnExists()` 检查，退化为只用 `DrawCallApiID` 过滤 |
| `DrawCallParameters` 表无 `CaptureID` 列 | `DatabaseQueryService` 不加 WHERE 过滤（单 capture 行为等效） |
| `CsvToDbService` 导入时表结构旧版缺列 | `CreateOrExtendTable()` → `ALTER TABLE ADD COLUMN` 补齐 |
| Texture 查询 captureId 无数据 | `TextureExtractor` fallback 到只用 `resourceID=y` |

---

## 八、增量执行（completedTargets 机制）

在 Server 模式的 `AnalysisJobRunner` 中，7 个 phase 顺序调用 `RunAnalysis()`，`completedTargets` 随阶段累积传入：

```
Phase 1: target=Dc       → completes → completedTargets |= Dc
Phase 2: target=Shaders  → completes → completedTargets |= Shaders
Phase 3: target=Label    → RunAnalysis(.., completedTargets=Dc|Shaders)
         → doLabel 检查: !completedTargets.HasFlag(Label) → 执行 Label
         → completes → completedTargets |= Label
Phase 4: target=Metrics
Phase 5: target=Status   → RunAnalysis(.., completedTargets=Dc|Shaders|Label|Metrics)
         → Step 2 检查: completedTargets.HasFlag(Label) → 跳过 LLM，LoadLabelsFromLabelJson()
```

这确保 LLM 标注不被重复调用，已完成阶段的结果从磁盘 JSON 加载。

---

## 九、config.ini 相关键

| 键 | 默认值 | 说明 |
|----|--------|------|
| `AnalysisPassMode` | `all` | `all` / `stats` / `analysis` |
| `AnalysisNoExtract` | `false` | 跳过 shader/texture 物理提取 |
| `AnalysisOnlyGenerateReport` | `false` | 等同于 AnalysisOnly |
| `ShaderOutputFormat` | `hlsl` | `hlsl` / `glsl` / `spv`（spirv-cross 目标） |
| `TextureExtractionDegree` | `4` | Texture 并行提取并发度 |
| `MeshExtractionDegree` | `4` | Mesh 提取并发度 |
| `LlmMaxConcurrentRequests` | `8` | LLM 并发 HTTP 请求上限 |
| `AnalysisCategories` | （内置 7 类） | 逗号分隔的允许分类列表 |
| `MetricsWhitelist` | `""` | 指标名白名单，空则全量导入 |
| `Session.ShadersDir` | `shaders` | shader 输出子目录名 |
| `Session.TexturesDir` | `textures` | texture 输出子目录名 |
| `Session.MeshesDir` | `meshes` | mesh 输出子目录名 |
