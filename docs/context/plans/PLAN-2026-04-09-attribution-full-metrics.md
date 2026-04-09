---
type: plan
topic: attribution_rules 扩展至完整 Adreno metrics + 归一权重体系
status: proposed
based_on:
  - FINDING-2026-04-09-attribution-metrics-gap.md
  - FINDING-2026-04-09-drawcallmetrics-dict-impact.md
  - FINDING-2026-04-09-metrics-hardcode-vs-config.md
related_paths:
  - SDPCLI/analysis/attribution_rules.json
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/config.ini
related_tags: [attribution, metrics, normalization, whitelist, bottleneck, percentile]
summary: |
  1) MetricsWhitelist 扩展至完整 40+ Adreno counters。
  2) DrawCallMetrics 改为 Dictionary All，CounterToKey 覆盖全量 counter。
  3) attribution_rules.json 扩展 layer1 hints + layer3 weights，按方法C归一。
  4) StatusJsonService BuildAvg/Percentile 块动态遍历 All.Keys。
  5) AttributionRuleEngine Layer3 分数按 bottleneck 理论满分（contribution_weight sum）归一。
last_updated: 2026-04-09
---

# Plan: attribution_rules 完整 metrics 覆盖 + 归一权重

## 背景

见 FINDING-2026-04-09-attribution-metrics-gap.md。

**核心问题**：
1. `DrawCallMetrics` 只有 13 个 typed fields，`MetricsWhitelist` 有 17 个，
   Adreno 完整列表约 48 个——大量 counter 进入 DB 但无法被归因体系消费
2. attribution_rules layer3 bottleneck 分数无归一，不同 metrics 可用数量时分数不可比
3. `StatusJsonService` percentile 块硬编码 13 个字段，无法覆盖扩展 counter

---

## 实现分两个独立阶段

---

## 阶段 A：DrawCallMetrics Dictionary 化 + CounterToKey 全量映射

（前提条件：见 PLAN-2026-04-09-drawcallmetrics-dynamic-storage.md）

### A1. MetricsWhitelist 扩展（config.ini）

将 whitelist 扩展至完整 Adreno 可用 counters（约 48 个），
使 capture 侧和 DB 都收录更多 counter：

```ini
MetricsWhitelist=Clocks,Read Total (Bytes),Write Total (Bytes),
Fragments Shaded,Vertices Shaded,Reused Vertices,Pre-clipped Polygons,
LRZ Pixels Killed,Textures / Fragment,Textures / Vertex,
% Texture Fetch Stall,% Texture L1 Miss,% Texture L2 Miss,% Texture Pipes Busy,
% Linear Filtered,% Nearest Filtered,% Anisotropic Filtered,
L1 Texture Cache Miss Per Pixel,
Texture Memory Read BW (Bytes),Vertex Memory Read (Bytes),SP Memory Read (Bytes),
Avg Bytes / Fragment,Avg Bytes / Vertex,
% Shaders Busy,% Shaders Stalled,% Time ALUs Working,% Time EFUs Working,
% Time Shading Vertices,% Time Shading Fragments,% Time Compute,
% Shader ALU Capacity Utilized,% Wave Context Occupancy,% Instruction Cache Miss,
Fragment Instructions,Fragment ALU Instructions (Full),Fragment ALU Instructions (Half),
Fragment EFU Instructions,Vertex Instructions,ALU / Fragment,ALU / Vertex,
EFU / Fragment,EFU / Vertex,
Average Polygon Area,Average Vertices / Polygon,% Prims Clipped,% Prims Trivially Rejected,
% Vertex Fetch Stall,% Stalled on System Memory,
Preemptions,Avg Preemption Delay
```

> 注意：完整 whitelist 会增加 Profiler 侧内存压力（~1GB+），
> 建议分两级：常规分析 whitelist（17 个）和完整 whitelist（48 个）用 `EnableOptionalMetrics` 控制。

### A2. DrawCallMetrics.CounterToKey 全量映射

在 `DrawCallModels.cs` 中维护从原始 counter 名到 snake_case key 的映射表，
覆盖所有已知 Adreno counter 名（含上面完整列表）：

```csharp
public static readonly IReadOnlyDictionary<string, string> CounterToKey =
    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    // Misc
    ["Clocks"]                              = "clocks",
    ["Preemptions"]                         = "preemptions",
    ["Avg Preemption Delay"]                = "avg_preemption_delay",
    // Memory Bandwidth
    ["Read Total (Bytes)"]                  = "read_total_bytes",
    ["Write Total (Bytes)"]                 = "write_total_bytes",
    ["Texture Memory Read BW (Bytes)"]      = "tex_mem_read_bytes",
    ["Vertex Memory Read (Bytes)"]          = "vertex_mem_read_bytes",
    ["SP Memory Read (Bytes)"]              = "sp_mem_read_bytes",
    ["Avg Bytes / Fragment"]                = "avg_bytes_per_fragment",
    ["Avg Bytes / Vertex"]                  = "avg_bytes_per_vertex",
    // Geometry
    ["Fragments Shaded"]                    = "fragments_shaded",
    ["Vertices Shaded"]                     = "vertices_shaded",
    ["Reused Vertices"]                     = "reused_vertices",
    ["Pre-clipped Polygons"]                = "pre_clipped_polygons",
    ["LRZ Pixels Killed"]                   = "lrz_pixels_killed",
    ["Average Polygon Area"]                = "avg_polygon_area",
    ["Average Vertices / Polygon"]          = "avg_vertices_per_polygon",
    ["% Prims Clipped"]                     = "prims_clipped_pct",
    ["% Prims Trivially Rejected"]          = "prims_trivially_rejected_pct",
    // Texture
    ["% Texture Fetch Stall"]               = "tex_fetch_stall_pct",
    ["% Texture L1 Miss"]                   = "tex_l1_miss_pct",
    ["% Texture L2 Miss"]                   = "tex_l2_miss_pct",
    ["% Texture Pipes Busy"]                = "tex_pipes_busy_pct",
    ["% Linear Filtered"]                   = "linear_filtered_pct",
    ["% Nearest Filtered"]                  = "nearest_filtered_pct",
    ["% Anisotropic Filtered"]              = "anisotropic_filtered_pct",
    ["% Non-Base Level Textures"]           = "non_base_level_tex_pct",
    ["L1 Texture Cache Miss Per Pixel"]     = "l1_tex_cache_miss_per_pixel",
    ["Textures / Fragment"]                 = "textures_per_fragment",
    ["Textures / Vertex"]                   = "textures_per_vertex",
    // Shader / ALU
    ["% Shaders Busy"]                      = "shaders_busy_pct",
    ["% Shaders Stalled"]                   = "shaders_stalled_pct",
    ["% Time ALUs Working"]                 = "time_alus_working_pct",
    ["% Time EFUs Working"]                 = "time_efus_working_pct",
    ["% Time Shading Vertices"]             = "time_shading_vertices_pct",
    ["% Time Shading Fragments"]            = "time_shading_fragments_pct",
    ["% Time Compute"]                      = "time_compute_pct",
    ["% Shader ALU Capacity Utilized"]      = "shader_alu_capacity_pct",
    ["% Wave Context Occupancy"]            = "wave_context_occupancy_pct",
    ["% Instruction Cache Miss"]            = "instruction_cache_miss_pct",
    ["Fragment Instructions"]               = "fragment_instructions",
    ["Fragment ALU Instructions (Full)"]    = "fragment_alu_instr_full",
    ["Fragment ALU Instructions (Half)"]    = "fragment_alu_instr_half",
    ["Fragment EFU Instructions"]           = "fragment_efu_instructions",
    ["Vertex Instructions"]                 = "vertex_instructions",
    ["ALU / Fragment"]                      = "alu_per_fragment",
    ["ALU / Vertex"]                        = "alu_per_vertex",
    ["EFU / Fragment"]                      = "efu_per_fragment",
    ["EFU / Vertex"]                        = "efu_per_vertex",
    // Vertex Fetch / Stall
    ["% Vertex Fetch Stall"]                = "vertex_fetch_stall_pct",
    ["% Stalled on System Memory"]          = "stalled_on_system_mem_pct",
};
```

### A3. StatusJsonService — 动态 percentile 块

改为遍历每个 DC 的 `Metrics.All.Keys`（或 CounterToKey 的全量 key），
对每个 key 动态计算 avg / percentile，输出时用 `CounterToKey` 转 snake_case：

```csharp
// 从所有 DC 收集出现过的 counter 名
var allCounterNames = withM
    .SelectMany(d => d.Metrics!.All.Keys)
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToList();

var avgBlock = new JObject();
foreach (var counterName in allCounterNames)
{
    var vals = withM
        .Where(d => d.Metrics!.All.ContainsKey(counterName))
        .Select(d => d.Metrics!.All[counterName])
        .ToList();
    if (vals.Count == 0) continue;
    string key = DrawCallMetrics.CounterToKey.TryGetValue(counterName, out var k)
        ? k : counterName.ToLowerInvariant().Replace(" ", "_");
    avgBlock[key] = vals.Average();
}
```

---

## 阶段 B：attribution_rules.json 扩展 + 归一权重

### B1. Layer1 hints 扩展

新增有诊断价值的 counter hint（snake_case key 与 CounterToKey 输出一致）：

```json
// 新增条目示例
{ "metric": "shaders_stalled_pct",      "bottleneck_hint": "shader_alu",    "description": "Shader stall high → latency/dependency stall" },
{ "metric": "time_alus_working_pct",    "bottleneck_hint": "shader_alu",    "description": "ALU time high → computation bound" },
{ "metric": "wave_context_occupancy_pct","bottleneck_hint": "shader_alu",   "description": "Occupancy low → memory latency not hidden" },
{ "metric": "instruction_cache_miss_pct","bottleneck_hint": "shader_alu",   "description": "Instruction cache miss → complex branching shader" },
{ "metric": "tex_pipes_busy_pct",       "bottleneck_hint": "texture_bound", "description": "Texture pipe busy rate high" },
{ "metric": "textures_per_fragment",    "bottleneck_hint": "texture_bound", "description": "High texture fetches per fragment" },
{ "metric": "lrz_pixels_killed",        "bottleneck_hint": "overdraw",      "description": "LRZ kill count → early-Z efficiency indicator" },
{ "metric": "vertex_fetch_stall_pct",   "bottleneck_hint": "model_cost",    "description": "Vertex fetch stall → vertex buffer bandwidth" },
{ "metric": "preemptions",              "bottleneck_hint": "preemption",    "description": "GPU preemptions → scheduling overhead" },
{ "metric": "avg_preemption_delay",     "bottleneck_hint": "preemption",    "description": "Preemption delay high → long GPU work chunks" },
{ "metric": "fragment_alu_instr_full",  "bottleneck_hint": "fragment_bound","description": "Full-precision ALU instructions high" },
{ "metric": "fragment_efu_instructions","bottleneck_hint": "fragment_bound","description": "EFU instruction count high → trig/special math" }
```

### B2. Layer3 bottleneck weights — 方法 C 归一

**当前问题**：各 bottleneck 的 contributing_metrics contribution_weight 之和不等，
导致满分不统一（`texture_bound` 满分 = `0.80*(1.0+0.6+0.4)` = 1.6，
`shader_alu` 满分 = `0.80*(1.0+0.5)` = 1.2）。

**方法 C**：将每个 bottleneck 的 contribution_weight 归一，使 sum = 1.0；
Layer 3 分数范围统一为 [0, p95_weight=0.80]，bottleneck 间直接可比。

归一示例：

```json
// texture_bound: 原始权重 1.0+0.6+0.4=2.0 → 归一后 0.50+0.30+0.20
{
  "bottleneck": "texture_bound",
  "contributing_metrics": [
    { "metric": "tex_fetch_stall_pct", "contribution_weight": 0.50 },
    { "metric": "tex_l1_miss_pct",     "contribution_weight": 0.30 },
    { "metric": "tex_l2_miss_pct",     "contribution_weight": 0.20 }
  ]
}

// bandwidth: 原始权重 1.0+0.8+0.6+0.5=2.9 → 归一后 0.35+0.28+0.21+0.17 ≈ 1.0
{
  "bottleneck": "bandwidth",
  "contributing_metrics": [
    { "metric": "read_total_bytes",   "contribution_weight": 0.35 },
    { "metric": "write_total_bytes",  "contribution_weight": 0.28 },
    { "metric": "tex_mem_read_bytes", "contribution_weight": 0.21 },
    { "metric": "tex_l2_miss_pct",   "contribution_weight": 0.17 }
  ]
}
```

新增 bottleneck（可选，先评估是否有实际 DC 案例）：
```json
{
  "bottleneck": "preemption",
  "display_name": "GPU Preemption Overhead",
  "contributing_metrics": [
    { "metric": "preemptions",          "contribution_weight": 0.60 },
    { "metric": "avg_preemption_delay", "contribution_weight": 0.40 }
  ]
}
```

### B3. AttributionRuleEngine — 验证 GetMetricValues 覆盖完整 CounterToKey

当 DrawCallMetrics 改为 Dictionary 后，`GetMetricValues()` 直接返回规范化后的 All，
无须逐字段映射，天然覆盖所有 whitelist 中存在的 counter。

**关键约束**：`attribution_rules.json` 中所有 `"metric"` 字段的值
必须与 `DrawCallMetrics.CounterToKey` 的 value（snake_case）完全一致。

---

## 链路验证点（Executor 必须核查）

| 步骤 | 验证内容 |
|-----|---------|
| 1 | `CounterToKey` 中每个 value（snake_case）是否与 `attribution_rules.json` 中所有 `"metric"` 字段出现的值完全一致 |
| 2 | `StatusJsonService` 输出的 `global_percentiles` / `category_stats.metrics_p70/80/95` 的 JSON key 是否与 attributionRuleEngine 读取的 key 对齐 |
| 3 | layer3 归一后各 bottleneck 的 contribution_weight sum = 1.0（误差 < 0.001） |
| 4 | `MetricsWhitelist` 扩展后 capture 侧内存/性能是否可接受（设备侧）|

---

## 实现顺序

1. `DrawCallModels.cs`：DrawCallMetrics → Dictionary All + 完整 CounterToKey
2. `MetricsQueryService.cs`：Step 3 直接赋 All
3. `StatusJsonService.cs`：动态遍历 All.Keys，输出 snake_case key
4. 验证步骤 1/2（链路对齐）
5. `attribution_rules.json`：扩展 layer1 hints + 归一 layer3 weights + 新增 bottleneck
6. `config.ini MetricsWhitelist`：扩展至完整列表
7. 下游文件 P2 机械替换（m.Clocks → m.G("Clocks") 等）
8. dotnet build + 端到端测试

---

## 执行状态

Implementation requires the Executor agent.
