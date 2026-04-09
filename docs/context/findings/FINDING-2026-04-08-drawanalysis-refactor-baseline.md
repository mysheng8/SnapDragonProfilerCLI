---
type: finding
topic: drawanalysis pipeline refactor baseline — current state audit
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Modes/DrawCallAnalysisMode.cs
related_tags:
  - drawcall-analysis
  - refactor
  - json-schema
  - bottleneck
  - attribution
  - llm
summary: |
  审计当前 AnalysisPipeline 实际产出的两个文件（JSON + MD）的内容结构，
  确认重构映射关系：raw.json = 现有 JSON 微改；现有 MD 拆分为
  status.json + topdc.json + analysis.md + dashboard.md。
last_updated: 2026-04-08
---

# Finding: DrawAnalysis Pipeline 现状审计

## 1. 当前实际产出文件（只有两个）

`ReportGenerationService` 当前输出：

| 文件 | 生成方法 | LLM 依赖 |
|------|---------|----------|
| `DrawCallAnalysis_*.json` | `GenerateLabeledMetricsJson()` | No |
| `DrawCallAnalysis_Summary_*.md` | `GenerateSummaryReport()` | **Yes**（可配置关闭）|

---

## 2. 现有 JSON 的内容（`GenerateLabeledMetricsJson`）

schema_version = "2.1"，root 字段：

```
schema_version, generated_at, total_drawcalls
drawcalls[]
  dc_id, api_name, pipeline_id, vertex_count, index_count, instance_count
  category, detail            ← label，但结构简单（仅 category + detail）
  shader_stages[]             ← stage/module_id/entry_point
  shader_files[]              ← 相对路径 ../../shaders/pipeline_N_*.hlsl
  texture_ids[], texture_files[]
  mesh_files[]
  vertex_buffers[], index_buffer
  render_targets{}            ← color/depth IDs + width/height（已有）
  metrics{}                   ← 原始数值，metrics 缺失时为 null
```

**相对于目标 raw.json，缺失字段：**

| 缺失 | 说明 |
|------|------|
| root 级 `snapshot_id`、`sdp_name` | 跨 snapshot 区分 |
| label.`subcategory` | 二级分类 |
| label.`reason_tags[]` | 结构化标签，供 status.json 统计 |
| label.`confidence` | LLM 自报置信度 |
| label.`label_source` | "llm" / "rule" / "cache" |
| 每 DC 的 `metrics_available` bool | 区分 null 和全零 |

---

## 3. 现有 MD 的内容分解（`GenerateSummaryReport`）

当前 MD 包含以下逻辑 section，按重构后去向分类：

| MD Section | 代码位置 | 重构后去向 |
|------------|---------|------------|
| Frame Snapshot（screenshot）| `1_screenshot.bmp` rotate embed | **dashboard.md** |
| Overview Categories 表（count/%）| `catGroups` | **dashboard.md** |
| GPU Clocks 柱状图（mermaid xychart）| 全量 DC | **dashboard.md** |
| GPU Clock Budget 饼图（mermaid pie）| `catStats` | **dashboard.md** |
| 4.1 Top 5 全局表（动态 outlier 列）| ratios vs avg | **dashboard.md** |
| 4.1b 各分类 Top 5 表 | `catWithM.Take(5)` | **dashboard.md** |
| 3D Mesh Preview 链接 | `meshes/*.obj` | **dashboard.md** |
| **4.2 Category Statistics**（avg per cat）| `catStats` | **→ status.json**（需补百分位）|
| **4.3 单 DC 异常指标**（`BuildOutlierSignals`）| 规则 vs 全局 avg | **→ topdc.json** Layer1 信号 |
| **4.3 LLM per-DC 结论**（`GetLlmBottleneckConclusion`）| shader + metrics + RT/Tex | **→ analysis.md** |
| **总结 LLM 整帧建议**（`GetLlmOverallConclusion`）| top5+catStats+signals | **→ analysis.md** |

---

## 4. 两个 LLM 调用的现有 Prompt 内容

### 4.1 `GetLlmBottleneckConclusion`（per-DC，pipeline 级缓存）

发送：DC 标识 + 分类 + 顶点数 / 性能指标（vs 全局 avg）+ 异常信号 /
RT 描述 / 绑定纹理列表（尺寸/格式，最多 20 张）/ Bindings 摘要 /
Shader 源码（entry point 优先，声明前缀精简，上限 `LlmMaxShaderChars`=20000）

要求输出：纯中文正文，Pass 类型 + 主瓶颈 + 2-3 条移动端优化建议

### 4.2 `GetLlmOverallConclusion`（整帧一次）

发送：全局基线 / Top 5 完整 metrics 表 / Category Statistics 表 / 每 Top5 DC 异常信号

要求输出：5-8 条精准整帧优化建议

---

## 5. 现有 `BuildOutlierSignals` 阈值（硬编码，非百分位）

| 指标 | 触发条件 |
|------|---------|
| FragmentsShaded | > avg × 1.5 |
| ShadersBusyPct | > avg + 10pp **且** > 70% |
| FragmentInstructions | > avg × 1.5 |
| TexFetchStallPct | > avg + 5pp **且** > 5% |
| TexL1MissPct | > avg + 10pp **且** > 30% |
| ReadTotalBytes | > avg × 2 |
| WriteTotalBytes | > avg × 2 |

这是 Layer1 的原型，重构时迁移到 `attribution_rules.json` 并升级为百分位对比。

---

## 6. DrawCallMetrics 字段清单（13 个）

| 字段名 | 含义 |
|--------|------|
| Clocks | GPU 时钟数 |
| ReadTotalBytes | 总内存读取字节 |
| WriteTotalBytes | 总内存写入字节 |
| FragmentsShaded | 着色片元数 |
| VerticesShaded | 着色顶点数 |
| ShadersBusyPct | Shader 单元繁忙率 % |
| TexL1MissPct | 纹理 L1 cache miss 率 % |
| TexL2MissPct | 纹理 L2 cache miss 率 % |
| TexFetchStallPct | 纹理 fetch stall 率 % |
| FragmentInstructions | 片元 shader 指令数 |
| VertexInstructions | 顶点 shader 指令数 |
| TexMemReadBytes | 纹理内存读取字节 |
| VertexMemReadBytes | 顶点内存读取字节 |

---

## 7. 结论：重构工作量分布

| 产物 | 来源 | 净新增工作量 |
|------|------|------------|
| `raw.json` | 现有 JSON | **小**：补 6 个字段，扩展 label 结构 |
| `status.json` | 现有 MD Section 4.2 | **中**：从 MD 剥离，补百分位计算（p80/p95）和 label 质量统计 |
| `topdc.json` | 现有 MD Section 4.3 异常信号 | **大**：结构化 Layer1，全新 Layer2 百分位对比 + Layer3 加权评分 |
| `analysis.md` | 现有 MD 的 LLM 文字部分 | **中**：拆两步 LLM，重写 prompt 引用 topdc.json 数据 |
| `dashboard.md` | 现有 MD 图表/表格部分 | **小**：搬移，去掉分析文字 |

最大净新增：`status.json` 的百分位计算 + `topdc.json` 的 Layer2/Layer3 归因评分，
这两块在现有代码中完全没有，需要全新实现。
