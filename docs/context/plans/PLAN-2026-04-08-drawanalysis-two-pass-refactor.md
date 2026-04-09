---
type: plan
topic: DrawAnalysis 两 pass 重构 — 完整设计方案
status: proposed
based_on:
  - FINDING-2026-04-08-drawanalysis-refactor-baseline.md
  - FINDING-2026-04-07-shader-texture-export-structure.md
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
related_tags:
  - drawcall-analysis
  - json-schema
  - bottleneck
  - attribution
  - two-pass
  - llm
summary: |
  将 AnalysisPipeline 的单一流程重构为"统计 pass + 分析 pass"两阶段。
  现有两个产物（DrawCallAnalysis_*.json + DrawCallAnalysis_Summary_*.md）
  分拆为 5 个产物：raw.json（现有 JSON 微改）、status.json（MD Section 4.2 剥离 + 补百分位）、
  topdc.json（MD Section 4.3 结构化 + 全新 Layer2/Layer3 归因评分）、
  analysis.md（MD LLM 文字部分重写）、dashboard.md（MD 图表/表格部分搬移）。
  新增 analysis/attribution_rules.json 三层规则配置。
last_updated: 2026-04-08
---

# Plan: DrawAnalysis 两 Pass 重构 — 完整设计方案

---

## 0. 总体架构

### 0.1 现有产物 → 重构后产物映射

```
现有输出（2 个）                重构后输出（5 个）
─────────────────────────────────────────────────────────
DrawCallAnalysis_*.json    ──►  snapshot_{id}_raw.json     [改动最小：补 6 个字段]
                                snapshot_{id}_status.json  [MD 4.2 节剥离 + 补百分位]
                                snapshot_{id}_topdc.json   [MD 4.3 节结构化 + 全新归因]
DrawCallAnalysis_Summary_  ──►  snapshot_{id}_analysis.md  [MD LLM 文字部分重写]
    *.md                        snapshot_{id}_dashboard.md [MD 图表/表格部分搬移]
```

### 0.2 Pass 划分

```
输入: snapshot_{captureId}/ 目录
         ├── sdp.db  (DrawCall、shader、texture 原始数据)
         ├── DrawCallMetrics.csv
         └── DrawCallParameters.csv

═══════════════════════════════════════════════════════
  PASS A — 统计 Pass（无 LLM）
═══════════════════════════════════════════════════════
  Step A1  采集 raw DC 信息（DB 查询）          ← 现有 Step 1
  Step A2  LLM / 规则打标签 → 扩展 label 结构  ← 现有 Step 2（label 结构扩展）
  Step A3  join metrics CSV                   ← 现有 Step 3
  Step A4  升级输出 snapshot_{id}_raw.json     ← 替换现有 GenerateLabeledMetricsJson()
  Step A5  聚合统计 + 百分位 → status.json     ← 从 MD Section 4.2 剥离，补百分位（纯新）
  Step A6  Per-category top-N 归因计算 → topdc.json  ← 从 MD Section 4.3 剥离，补 Layer2/3（纯新）

═══════════════════════════════════════════════════════
  PASS B — 分析 Pass（LLM）
═══════════════════════════════════════════════════════
  Step B1  读取 raw.json → LLM 分析 shader + DC 内容
           中间产物: per_dc_content_{dc_id}.json（可选缓存）
           ← 重写现有 GetLlmBottleneckConclusion() prompt，输出改为 JSON 中间产物
  Step B2  读取 topdc.json + status.json + B1 结果 → LLM 写归因分析报告
           → 输出 snapshot_{id}_analysis.md
           ← 重写现有 GetLlmOverallConclusion()，按 category 分批调用
  Step B3  读取 status.json → 规则生成展示 Dashboard
           → 输出 snapshot_{id}_dashboard.md
           ← 从现有 GenerateSummaryReport() 中搬出图表/表格部分

═══════════════════════════════════════════════════════
  规则配置（静态，随代码分发）
═══════════════════════════════════════════════════════
  analysis/attribution_rules.json  （三层归因规则）
```

---

## 1. JSON Schema 设计

### 1.1 `snapshot_{id}_raw.json`

> **定位**：从 DB 查询得到的 DC 原始信息 + LLM/规则打的 label。
> **来源**：升级现有 `DrawCallAnalysis_*.json`（`GenerateLabeledMetricsJson` 输出），改动最小。
> 主要变更：① root 补 `snapshot_id` / `sdp_name`；② label 结构扩展；③ 每 DC 补 `metrics_available`。

```jsonc
{
  "schema_version": "2.0",
  "snapshot_id": 3,
  "sdp_name": "2026-04-02T14-48-01",
  "generated_at": "2026-04-08T10:30:00Z",
  "total_dc_count": 412,
  "draw_calls": [
    {
      // ── 核心标识 ────────────────────────────────────
      "dc_id":          "1.1.31",         // GUI 编码 ID 或整数 ApiID
      "api_id":         106974,           // VkApiID
      "api_name":       "vkCmdDraw",
      "pipeline_id":    84,
      "render_pass_id": 12,

      // ── Draw 参数 ────────────────────────────────────
      "vertex_count":   6144,
      "index_count":    0,
      "instance_count": 1,

      // ── Shader 引用 ──────────────────────────────────
      "shader_stages": [
        {
          "stage":       "Vertex",
          "module_id":   10024,
          "entry_point": "main",
          "file":        "../../shaders/pipeline_84_vert.hlsl"
        },
        {
          "stage":       "Fragment",
          "module_id":   10025,
          "entry_point": "main",
          "file":        "../../shaders/pipeline_84_frag.hlsl"
        }
      ],

      // ── Texture 引用 ─────────────────────────────────
      "texture_ids": [201, 202, 205],
      "texture_files": [
        "../../textures/texture_201.png",
        "../../textures/texture_202.png"
      ],
      "textures": [
        { "texture_id": 201, "width": 1024, "height": 1024,
          "format": "VK_FORMAT_R8G8B8A8_UNORM", "layers": 1, "levels": 11 }
      ],

      // ── RenderTarget ─────────────────────────────────
      "render_targets": [
        { "attachment_index": 0, "attachment_type": "Color",
          "resource_id": 900, "width": 2400, "height": 1080,
          "format": "VK_FORMAT_R8G8B8A8_UNORM" }
      ],

      // ── Mesh 引用 ────────────────────────────────────
      "mesh_file": "../../meshes/mesh_106974.obj",

      // ── Metrics（原始值，来自 CSV join）────────────────
      "metrics": {
        "clocks":                1284000,
        "read_total_bytes":      2097152,
        "write_total_bytes":     786432,
        "fragments_shaded":      345600,
        "vertices_shaded":       6144,
        "shaders_busy_pct":      87.4,
        "tex_l1_miss_pct":       34.2,
        "tex_l2_miss_pct":       18.7,
        "tex_fetch_stall_pct":   41.6,
        "fragment_instructions": 892000,
        "vertex_instructions":   18432,
        "tex_mem_read_bytes":    1572864,
        "vertex_mem_read_bytes": 98304
      },
      "metrics_available": true,

      // ── LLM / 规则 Label ──────────────────────────────
      // 这是扩展后的 label 结构（见 1.1.1 节）
      "label": {
        "category":    "Scene",
        "subcategory": "Terrain",
        "detail":      "地形高精度材质渲染，使用 PBR 流程",
        "reason_tags": ["pbr_material", "multi_texture_blend", "high_uv_sampling"],
        "confidence":  0.91,
        "label_source": "llm"   // "llm" | "rule" | "cache"
      }
    }
    // ... 其余 411 个 DC
  ]
}
```

#### 1.1.1 扩展 Label 结构详解

| 字段 | 类型 | 说明 |
|------|------|------|
| `category` | string | 一级分类：Scene / Character / Shadow / PostProcess / VFX / UI / Terrain / Compute |
| `subcategory` | string | 二级分类：Opaque / Transparent / Depth-only / SkinMesh / GaussianBlur … |
| `detail` | string | LLM 自由描述，≤ 120 字 |
| `reason_tags` | string[] | 结构化标签，从预定义集合中选取，便于统计 |
| `confidence` | float [0,1] | LLM 自报置信度；规则路径固定输出 0.70 |
| `label_source` | enum | `"llm"` / `"rule"` / `"cache"` |

预定义 `reason_tags` 集合（在 `attribution_rules.json` 的 `label_reason_tags` 节中维护）：
```
pbr_material, multi_texture_blend, high_uv_sampling,
skinned_mesh, morphing, instanced_draw, compute_dispatch,
gaussian_blur, tone_mapping, ssao, bloom, taa,
shadow_depth_write, shadow_pcf_sample,
particle_billboard, trail_ribbon,
ui_canvas, font_glyph
```

---

### 1.2 `snapshot_{id}_status.json`

> **定位**：整体 capture 级别统计聚合层（无 LLM）。
> **来源**：从现有 MD 的 Section 4.2（Category Statistics avg 表）和全局 avg 剥离，
> 关键补充：**按全量 DC 和每个 category 计算 p50/p80/p95 百分位**（现有代码完全没有，net-new）。

```jsonc
{
  "schema_version": "2.0",
  "snapshot_id": 3,
  "sdp_name": "2026-04-02T14-48-01",
  "generated_at": "2026-04-08T10:30:00Z",

  // ── 整体 DC 统计 ──────────────────────────────────────
  "overall": {
    "total_dc_count":          412,
    "draw_dc_count":           389,   // vkCmdDraw* 类
    "compute_dc_count":        23,    // vkCmdDispatch* 类
    "total_clocks":            482000000,
    "total_read_bytes":        1073741824,
    "total_write_bytes":       268435456,
    "total_fragments_shaded":  184320000,
    "total_vertices_shaded":   2457600,
    "metrics_coverage_ratio":  0.94   // 有 metrics 的 DC 比例
  },

  // ── 分类统计（每个 category 独立聚合）────────────────────
  "category_stats": [
    {
      "category":   "Scene",
      "dc_count":   158,
      "percentage": 38.3,
      "clocks_sum": 201000000,
      "clocks_pct": 41.7,     // 占总 clocks 的比例
      "metrics_avg": {
        "clocks":                1272151,
        "read_total_bytes":      2048000,
        "write_total_bytes":     409600,
        "fragments_shaded":      280000,
        "vertices_shaded":       8200,
        "shaders_busy_pct":      72.3,
        "tex_l1_miss_pct":       28.4,
        "tex_l2_miss_pct":       14.2,
        "tex_fetch_stall_pct":   33.7,
        "fragment_instructions": 740000,
        "vertex_instructions":   24600,
        "tex_mem_read_bytes":    1228800,
        "vertex_mem_read_bytes": 65536
      },
      "metrics_p50": { /* 中位数，结构同 avg */ },
      "metrics_p80": { /* 80 分位，结构同 avg */ },
      "metrics_p95": { /* 95 分位，结构同 avg */ }
    }
    // 其他 category ...
  ],

  // ── Label 质量统计 ────────────────────────────────────
  "label_stats": {
    "avg_confidence":          0.84,
    "low_confidence_ratio":    0.08,   // confidence < 0.6 的比例
    "low_confidence_threshold": 0.6,
    "llm_labeled_count":       387,
    "rule_labeled_count":      25,
    "reason_tag_distribution": {
      "pbr_material":          89,
      "multi_texture_blend":   67,
      "high_uv_sampling":      51,
      "shadow_depth_write":    43,
      "gaussian_blur":         12
      // ...
    },
    "low_confidence_dc_ids": ["1.1.88", "1.1.102", "1.1.117"]
  },

  // ── 跨分类百分位阈值（供 topdc 归因使用）───────────────
  // 记录每个 metrics 在 全量DC 上的百分位值（用于步骤 A6 的参照）
  "global_percentiles": {
    "clocks":              { "p70": 820000, "p80": 1450000, "p95": 3200000 },
    "tex_fetch_stall_pct": { "p70": 22.0,   "p80": 35.0,   "p95": 55.0   },
    "shaders_busy_pct":    { "p70": 60.0,   "p80": 75.0,   "p95": 88.0   }
    // ... 全部 metrics
  }
}
```

---

### 1.3 `snapshot_{id}_topdc.json`

> **定位**：每个 category 的 top-N DC 展开详细归因分析（规则驱动，无 LLM）。
> **来源**：从现有 MD 的 Section 4.3 剥离并结构化（`BuildOutlierSignals` → Layer1）；
> Layer2 百分位对比 + Layer3 加权评分是**完全 net-new**，现有代码无对应逻辑。

```jsonc
{
  "schema_version": "2.0",
  "snapshot_id": 3,
  "sdp_name": "2026-04-02T14-48-01",
  "generated_at": "2026-04-08T10:30:00Z",
  "top_n_per_category": 5,

  "categories": [
    {
      "category": "Scene",
      "dc_count": 158,
      "top_dcs": [
        {
          "dc_id": "1.1.31",
          "rank":  1,
          "clocks": 3840000,
          "clocks_rank_in_category": 1,

          // ── 原始 metrics 快照 ─────────────────────────
          "metrics": { /* 同 raw.json 的 metrics 节 */ },

          // ── 归因分析结果（三层计算产物）────────────────
          "attribution": {
            // Layer 1: suspicious_metrics 初步匹配
            "suspicious_metrics": [
              {
                "metric":       "tex_fetch_stall_pct",
                "value":        41.6,
                "initial_bottleneck_hint": "texture_bound"
              },
              {
                "metric":       "shaders_busy_pct",
                "value":        87.4,
                "initial_bottleneck_hint": "shader_alu"
              }
            ],

            // Layer 2: 百分位比较结果
            "percentile_scores": [
              {
                "metric":       "tex_fetch_stall_pct",
                "value":        41.6,
                "category_p95": 38.0,   // 来自 status.json category_stats
                "global_p95":   55.0,   // 来自 status.json global_percentiles
                "percentile_tier": "p95",
                "weight_applied":   0.8,
                "bottleneck_targets": ["texture_bound"]
              },
              {
                "metric":       "shaders_busy_pct",
                "value":        87.4,
                "category_p95": 85.0,
                "global_p95":   88.0,
                "percentile_tier": "p80",
                "weight_applied":   0.3,
                "bottleneck_targets": ["shader_alu", "fragment_bound"]
              }
            ],

            // Layer 3: 加权汇总得分 → 最终瓶颈
            "bottleneck_scores": {
              "texture_bound":  0.92,   // 加权累计
              "shader_alu":     0.41,
              "fragment_bound": 0.27,
              "bandwidth":      0.14,
              "model_cost":     0.05
            },
            "primary_bottleneck":   "texture_bound",
            "secondary_bottleneck": "shader_alu",
            "confidence_score":     0.92
          },

          // ── 同分类内对比（category-relative）────────────
          "category_comparison": {
            "clocks_percentile_in_category":          98,
            "tex_fetch_stall_percentile_in_category": 96,
            "shaders_busy_percentile_in_category":    89
          },

          // ── 引用的 shader/mesh 文件路径 ──────────────────
          "shader_files": ["../../shaders/pipeline_84_frag.hlsl"],
          "mesh_file":    "../../meshes/mesh_106974.obj",
          "label": { /* 同 raw.json 的 label 节 */ }
        }
        // top5 其余 DC ...
      ]
    }
    // 其他 category ...
  ]
}
```

---

### 1.4 `analysis/attribution_rules.json`

> **定位**：静态规则配置，随代码库分发，驱动 Pass A step A6 的归因计算。
> 修改此文件即可调整归因逻辑，无需重新编译。

```jsonc
{
  "schema_version": "2.0",
  "description": "DrawCall 归因规则配置 — 三层规则引擎",

  // ═══════════════════════════════════════════════════
  // LAYER 1: 指标 → 初始 bottleneck hint 映射
  // 规则：当 metric 超过 layer2 的最低阈值（p70）时，触发 hint
  // ═══════════════════════════════════════════════════
  "layer1_metric_hints": [
    {
      "metric":            "tex_fetch_stall_pct",
      "bottleneck_hint":   "texture_bound",
      "description":       "纹理 fetch stall 高 → 纹理采样带宽瓶颈"
    },
    {
      "metric":            "tex_l1_miss_pct",
      "bottleneck_hint":   "texture_bound",
      "description":       "L1 cache miss 高 → 纹理缓存未命中"
    },
    {
      "metric":            "tex_l2_miss_pct",
      "bottleneck_hint":   "texture_bound",
      "description":       "L2 cache miss 高 → 纹理内存带宽压力"
    },
    {
      "metric":            "tex_mem_read_bytes",
      "bottleneck_hint":   "bandwidth",
      "description":       "纹理读取字节数高 → 内存带宽"
    },
    {
      "metric":            "shaders_busy_pct",
      "bottleneck_hint":   "shader_alu",
      "description":       "Shader 单元繁忙率高 → ALU 算力受限"
    },
    {
      "metric":            "fragment_instructions",
      "bottleneck_hint":   "fragment_bound",
      "description":       "片元指令数高 → 片元 shader 复杂度"
    },
    {
      "metric":            "fragments_shaded",
      "bottleneck_hint":   "overdraw",
      "description":       "着色片元数高 → 过绘制 / 大面积覆盖"
    },
    {
      "metric":            "vertices_shaded",
      "bottleneck_hint":   "model_cost",
      "description":       "顶点数高 → 模型复杂度 / 几何开销"
    },
    {
      "metric":            "vertex_instructions",
      "bottleneck_hint":   "model_cost",
      "description":       "顶点指令数高 → 顶点 shader 复杂度（蒙皮/变形）"
    },
    {
      "metric":            "vertex_mem_read_bytes",
      "bottleneck_hint":   "model_cost",
      "description":       "顶点内存读取高 → 顶点数据带宽"
    },
    {
      "metric":            "read_total_bytes",
      "bottleneck_hint":   "bandwidth",
      "description":       "总读取字节数高 → 内存带宽"
    },
    {
      "metric":            "write_total_bytes",
      "bottleneck_hint":   "bandwidth",
      "description":       "总写入字节数高 → 写入带宽（MRT / resolve）"
    }
  ],

  // ═══════════════════════════════════════════════════
  // LAYER 2: 百分位阈值档位及权重
  // 优先使用 category-level 百分位；category 样本 < min_sample_size 时
  // 降级使用 global 百分位
  // ═══════════════════════════════════════════════════
  "layer2_percentile_tiers": {
    "min_sample_size_for_category": 5,
    "tiers": [
      { "name": "p95", "threshold": 0.95, "weight": 0.80 },
      { "name": "p80", "threshold": 0.80, "weight": 0.30 },
      { "name": "p70", "threshold": 0.70, "weight": 0.15 }
    ]
  },

  // ═══════════════════════════════════════════════════
  // LAYER 3: bottleneck 复合评分权重
  // 每个 bottleneck 由哪些指标的 layer2 权重加权汇总
  // formula: score(B) = Σ  weight_applied(m)  for all m → B
  // ═══════════════════════════════════════════════════
  "layer3_bottleneck_weights": [
    {
      "bottleneck":   "texture_bound",
      "display_name": "纹理采样瓶颈",
      "contributing_metrics": [
        { "metric": "tex_fetch_stall_pct", "contribution_weight": 1.0 },
        { "metric": "tex_l1_miss_pct",     "contribution_weight": 0.6 },
        { "metric": "tex_l2_miss_pct",     "contribution_weight": 0.4 }
      ],
      "description": "DC 的主耗时来源是纹理fetch等待，通常由过多采样、大 mip 未命中或带宽不足引起"
    },
    {
      "bottleneck":   "shader_alu",
      "display_name": "Shader ALU 瓶颈",
      "contributing_metrics": [
        { "metric": "shaders_busy_pct",       "contribution_weight": 1.0 },
        { "metric": "fragment_instructions",  "contribution_weight": 0.5 }
      ],
      "description": "Shader 算力饱和，片元/顶点复杂度过高"
    },
    {
      "bottleneck":   "fragment_bound",
      "display_name": "片元着色瓶颈",
      "contributing_metrics": [
        { "metric": "fragment_instructions", "contribution_weight": 1.0 },
        { "metric": "fragments_shaded",      "contribution_weight": 0.3 }
      ],
      "description": "片元shader指令量大（复杂光照/材质计算）"
    },
    {
      "bottleneck":   "overdraw",
      "display_name": "过绘制",
      "contributing_metrics": [
        { "metric": "fragments_shaded",    "contribution_weight": 1.0 },
        { "metric": "write_total_bytes",   "contribution_weight": 0.4 }
      ],
      "description": "片元数远超像素数，存在严重 overdraw"
    },
    {
      "bottleneck":   "bandwidth",
      "display_name": "内存带宽瓶颈",
      "contributing_metrics": [
        { "metric": "read_total_bytes",      "contribution_weight": 1.0 },
        { "metric": "write_total_bytes",     "contribution_weight": 0.8 },
        { "metric": "tex_mem_read_bytes",    "contribution_weight": 0.6 },
        { "metric": "tex_l2_miss_pct",       "contribution_weight": 0.5 }
      ],
      "description": "DC 受限于内存读写带宽"
    },
    {
      "bottleneck":   "model_cost",
      "display_name": "几何/模型开销",
      "contributing_metrics": [
        { "metric": "vertices_shaded",       "contribution_weight": 1.0 },
        { "metric": "vertex_instructions",   "contribution_weight": 0.7 },
        { "metric": "vertex_mem_read_bytes", "contribution_weight": 0.5 }
      ],
      "description": "顶点数量多或顶点shader复杂（蒙皮/变形）"
    }
  ],

  // ═══════════════════════════════════════════════════
  // 其他配置
  // ═══════════════════════════════════════════════════
  "top_n_per_category":        5,
  "low_confidence_threshold":  0.60,
  "primary_bottleneck_min_score": 0.25,

  // reason_tags 枚举集合（DrawCallLabelService 使用）
  "label_reason_tags": [
    "pbr_material", "multi_texture_blend", "high_uv_sampling",
    "skinned_mesh", "morphing", "instanced_draw", "compute_dispatch",
    "gaussian_blur", "tone_mapping", "ssao", "bloom", "taa",
    "shadow_depth_write", "shadow_pcf_sample",
    "particle_billboard", "trail_ribbon",
    "ui_canvas", "font_glyph",
    "depth_only", "opaque_geometry", "transparent_geometry",
    "large_render_target", "mrt_output"
  ]
}
```

---

## 2. Pass A — 统计 Pass 实现设计

### 2.1 新增服务类

| 服务类 | 职责 |
|--------|------|
| `RawJsonExportService` | 消费 `DrawCallAnalysisReport` → 写 `snapshot_{id}_raw.json` |
| `StatusJsonService` | 聚合统计，计算百分位 → 写 `snapshot_{id}_status.json` |
| `AttributionRuleEngine` | 加载 `attribution_rules.json`，执行三层归因计算 |
| `TopDcJsonService` | 对每个 category top-N DC 调用 `AttributionRuleEngine` → 写 `snapshot_{id}_topdc.json` |

### 2.2 百分位计算方法

```
对全量 DC 和每个 category 内的 DC，对每个 metric 字段：
  sorted_values = sort({dc.metrics[field] for dc in dcs if dc.metrics != null})
  percentile(p) = sorted_values[ floor(p * len(sorted_values)) ]
```

记录 p70 / p80 / p95，写入 `status.json` 的 `global_percentiles` 和
每个 `category_stats[].metrics_p50/p80/p95`。

### 2.3 Layer 2 比较逻辑

```
对 DC d 的 metric m：
  category_sample = category_stats[d.category].metrics 集合
  if len(category_sample) >= min_sample_size:
      ref_percentiles = category-level percentiles
  else:
      ref_percentiles = global_percentiles

  for tier in p95, p80, p70:
      if d.metrics[m] > ref_percentiles[m][tier.name]:
          return tier  // 取最高触发档
```

### 2.4 Layer 3 加权汇总

```
for each bottleneck B in attribution_rules.layer3_bottleneck_weights:
    score(B) = 0
    for each mc in B.contributing_metrics:
        tier = layer2_result[mc.metric]   // 已触发的档位
        if tier != null:
            score(B) += tier.weight * mc.contribution_weight

primary_bottleneck = argmax(score)
```

---

## 3. Pass B — 分析 Pass 实现设计

### 3.1 Step B1 — DC 内容分析（LLM）

> **来源**：重写现有 `GetLlmBottleneckConclusion()` 的 prompt，
> 输出从"纯文字正文"改为结构化 JSON，缓存到 `per_dc_content/`。
> 现有逻辑（shader 源码精简 + RT/Texture/Bindings 上下文）继续复用，prompt 目标精简。

**输入 Prompt 结构**（per-DC，以 pipeline 为单位缓存）：

```
你是一名 Vulkan GPU 性能工程师，请分析以下 DrawCall 的用途和渲染技术。

## DrawCall 信息
dc_id: {dc_id}
category: {label.category} / {label.subcategory}
draw_params: vertex={vertex_count}, instance={instance_count}
render_target: {size} {format}

## Shader 代码（节选）
### Vertex Shader
{vert_shader_code[:3000]}

### Fragment Shader  
{frag_shader_code[:4000]}

## 要求
以 JSON 格式返回：
{
  "pass_name": "...",        // 这个 DC 执行的渲染 Pass 名称
  "technique": "...",        // 渲染技术（PBR/Phong/SDF/…）
  "key_features": ["..."],   // 3-5 个关键特征
  "complexity_notes": "...", // shader 复杂度评语
  "optimization_hints": ["..."] // 2-3 个优化方向
}
```

**输出**：中间缓存到 `per_dc_content_{dc_id}.json`（可跳过重复分析）。

### 3.2 Step B2 — 归因分析报告（LLM）

> **来源**：重写现有 `GetLlmOverallConclusion()` 的 prompt，
> 改为按 category 批量调用（每个 category 一次），输入改为引用 topdc.json 的归因结论，
> 而非原来的纯统计数字。现有的 prompt 结构（global baseline + top5 + catStats + signals）
> 升级为引用 Layer2/Layer3 输出的百分位 tier 和 bottleneck 得分。

**输入 Prompt 结构**（按 category 批量，一次 LLM 调用覆盖一个 category 的 top-N）：

```
你是一名移动端 Vulkan GPU 性能分析专家。
以下是 Snapdragon Profiler 对 {sdp_name} 帧捕获的 DrawCall 分析结果。

## 整体统计（来自 status.json）
总 DC 数: {total_dc_count}，{category} 类: {dc_count}
分类总 clocks 占比: {clocks_pct}%

## Top {N} 高耗时 DrawCall 归因分析（来自 topdc.json）

### DC {rank}. dc_id={dc_id}  ({label.detail})
- 主要瓶颈: {primary_bottleneck}（得分 {confidence_score:.2f}）
- 次要瓶颈: {secondary_bottleneck}
- 关键指标:
  - tex_fetch_stall_pct: {value}%（同分类 p{tier}）
  - shaders_busy_pct: {value}%（global p{tier}）
- Shader 内容摘要: {per_dc_content.technique} — {per_dc_content.key_features}

## 要求
请为 {category} 类写一份归因分析报告，包含：
1. 分类整体性能特征
2. 每个 Top DC 的详细归因说明（结合 shader 内容和指标数据）
3. 改进建议（可操作的具体建议，每条附具体指标依据）
用 Markdown 格式输出，中文撰写。
```

**输出**：`snapshot_{id}_analysis.md`，结构如下：

```markdown
# {sdp_name} DrawCall 归因分析报告

## 1. 整体概览
...（来自 status.json 汇总）

## 2. 分类分析

### 2.1 Scene 类（158 DC，占总 clocks 41.7%）
#### Top DC 详细归因
##### DC #1 — 1.1.31（地形 PBR 渲染）
- **主瓶颈**: 纹理采样瓶颈（得分 0.92）
  - tex_fetch_stall_pct = 41.6%，高于分类 p95（38.0%）
  - ...
- **建议**: 
  - 使用 anisotropic filtering 降级至 x4（当前过高采样开销）
  - ...

### 2.2 Character 类 ...
### 2.3 PostProcess 类 ...

## 3. 综合建议
...
```

### 3.3 Step B3 — 展示 Dashboard（规则生成，无 LLM）

> **来源**：从现有 `GenerateSummaryReport()` 中提取图表/表格部分：
> Frame Snapshot、Overview 分类计数表、mermaid 柱状图及饼图、4.1 Top5 表、4.1b 各分类 Top5 表、
> 3D Mesh Preview 链接——这些全部是规则生成，不含 LLM 文字，整体搬移即可。

展示 Dashboard 是纯规则组合，不需要 LLM。

**输出**：`snapshot_{id}_dashboard.md`，结构：

```markdown
# {sdp_name} — 帧分析 Dashboard

## Screenshot
![Frame Screenshot](../../textures/screenshot.png)

## DrawCall 分布
### 按分类计数（饼图占位）
| 分类 | DC 数 | 占比 | Clocks 占比 |
|------|-------|------|------------|
| Scene      | 158 | 38.3% | 41.7% |
...

> 图表: [category_distribution.html](category_distribution.html)

## 各分类 Top 5 DrawCall
### Scene

| 排名 | DC ID | 主瓶颈 | Clocks | tex_fetch_stall | shaders_busy |
|------|-------|--------|--------|-----------------|--------------|
| 1 | 1.1.31 | 纹理采样 | 3,840,000 | 41.6% | 87.4% |
...

### Character
...

## Label 质量统计
- 平均置信度: 0.84
- 低置信度 DC 比例: 8%（< 0.6）
- 低置信度 DC 列表: 1.1.88, 1.1.102, 1.1.117
```

---

## 4. 文件系统布局（重构后）

```
{sessionDir}/                            # 会话目录
  shaders/                               # 共享，Session 级别
  textures/                              # 共享，Session 级别
  meshes/                                # 共享，Session 级别
  snapshot_{captureId}/
    snapshot_{captureId}_raw.json        # Pass A Step A4 产出
    snapshot_{captureId}_status.json     # Pass A Step A5 产出
    snapshot_{captureId}_topdc.json      # Pass A Step A6 产出
    snapshot_{captureId}_analysis.md     # Pass B Step B2 产出
    snapshot_{captureId}_dashboard.md    # Pass B Step B3 产出
    per_dc_content/                      # Pass B Step B1 中间缓存
      dc_1_1_31.json
      dc_1_1_44.json
      ...

{projectRoot}/
  analysis/
    attribution_rules.json               # 静态规则配置（随代码分发）
```

---

## 5. 代码改动范围

### 5.1 新增文件

| 文件 | 说明 |
|------|------|
| `source/Services/Analysis/StatusJsonService.cs` | 聚合统计 + 百分位（p50/p80/p95），产出 status.json（全新逻辑）|
| `source/Services/Analysis/AttributionRuleEngine.cs` | 加载 attribution_rules.json，执行三层归因（全新逻辑）|
| `source/Services/Analysis/TopDcJsonService.cs` | 调用 AttributionRuleEngine，产出 topdc.json（全新逻辑）|
| `source/Services/Analysis/DcContentAnalysisService.cs` | Pass B Step B1，LLM DC 内容分析，输出 per_dc_content/ JSON |
| `source/Services/Analysis/AttributionReportService.cs` | Pass B Step B2，按 category 批量调用 LLM，输出 analysis.md |
| `analysis/attribution_rules.json` | 静态三层归因规则配置文件 |

### 5.2 修改文件

| 文件 | 改动 |
|------|------|
| `source/Models/DrawCallModels.cs` | 扩展 `DrawCallLabel`（新增 subcategory / reason_tags / confidence / label_source） |
| `source/Services/Analysis/DrawCallLabelService.cs` | LLM prompt 更新以输出新 label 结构（含 reason_tags + confidence） |
| `source/Services/Analysis/ReportGenerationService.cs` | ① `GenerateLabeledMetricsJson()` 补 snapshot_id/sdp_name/metrics_available 和新 label 字段 → raw.json；② `GenerateSummaryReport()` 图表/表格部分提取为 dashboard.md；③ `GetLlmBottleneckConclusion` / `GetLlmOverallConclusion` 被 `DcContentAnalysisService` / `AttributionReportService` 替代 |
| `source/Analysis/AnalysisPipeline.cs` | 重构为两 pass，新增 `RunStatsPass()` + `RunAnalysisPass()` 入口，串联新服务类 |
| `source/Modes/AnalysisMode.cs` | 新增 `--stats-only` / `--analysis-only` 参数支持 |
| `SDPCLI/DRAWCALL_ANALYSIS_GUIDE.md` | 更新使用文档 |

### 5.3 废弃（方法级别，文件保留）

- `ReportGenerationService.GenerateSummaryReport()` 内部逻辑废弃：
  - LLM 文字部分（`GetLlmBottleneckConclusion` / `GetLlmOverallConclusion`）→ 被 `AttributionReportService` 替代
  - 图表/表格部分 → 搬移到新 `dashboard.md` 生成逻辑
  - `BuildOutlierSignals` 硬编码阈值 → 迁移到 `attribution_rules.json` Layer1
- `CaptureReportService.GenerateReport()` 的 `report.json` 格式废弃，被 raw + status + topdc 三文件取代

---

## 6. 实现优先级

| 优先 | 模块 | 理由 |
|------|------|------|
| P0 | `DrawCallLabel` 扩展 + `DrawCallLabelService` prompt 更新 | 基础数据结构，所有下游依赖 |
| P0 | `attribution_rules.json` 初始版本 | 规则引擎配置基础 |
| P0 | `GenerateLabeledMetricsJson()` 补字段 → raw.json | 改动最小，先落地新文件名和新 label |
| P1 | `StatusJsonService` + 百分位计算（net-new）| Pass A 核心，topdc.json 的 Layer2 依赖它 |
| P1 | `AttributionRuleEngine` + `TopDcJsonService`（net-new）| Pass A 核心，最大净新增 |
| P2 | `GenerateSummaryReport()` 图表部分剥离 → dashboard.md | 改动小，把现有图表搬出 |
| P2 | `DcContentAnalysisService`（Step B1）| Pass B LLM，prompt 重写 |
| P2 | `AttributionReportService`（Step B2）| Pass B LLM 报告 |
| P3 | `DashboardGenerationService`（Step B3）| 展示层 |

---

## 7. 注意事项

1. **百分位对称性**：category 内 DC 数量可能很小（例如 Terrain 只有 3 个 DC），
   此时 p95 无意义，需降级到 global percentile（已在 `attribution_rules.json` 的
   `min_sample_size_for_category` 字段控制）。

2. **metrics_available 缺失**：约 6% 的 DC 没有 metrics（`metrics_available: false`），
   归因计算跳过这些 DC，在 status.json 中通过 `metrics_coverage_ratio` 记录。

3. **LLM 调用缓存**：Step B1 的 per-DC 内容分析以 pipeline_id 为 key 缓存，
   同 pipeline 的多个 DC 共享一次 LLM 调用结果。

4. **独立运行**：Pass A 和 Pass B 应可独立运行，通过 `--stats-only` 和
   `--analysis-only` CLI 参数控制，Pass B 依赖 Pass A 的 JSON 输出。

5. **规则文件路径**：`attribution_rules.json` 路径从 config.ini 的
   `AttributionRulesPath` 读取，默认为 `{appDir}/analysis/attribution_rules.json`。
