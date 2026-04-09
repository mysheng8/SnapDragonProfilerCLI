---
type: finding
topic: attribution_rules layer1/layer3 metric 覆盖缺口 — 10 条 layer1 hint 在 layer3 无对应 contributing_metric
status: investigated
related_paths:
  - SDPCLI/analysis/attribution_rules.json
  - SDPCLI/config.ini
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
related_tags: [attribution, metrics, whitelist, normalization, percentile, bottleneck, layer3]
summary: |
  2026-04-09 第二轮检查发现：session 中新增的 12 条 layer1 hints 有 10 条未在 layer3
  contributing_metrics 中出现，等于 hint 分有效但 layer3 score 完全不响应这些 metric。
  具体缺口：shader_alu(4条), texture_bound(2条), overdraw(1条), model_cost(1条), fragment_bound(2条)。
last_updated: 2026-04-09
---

# Finding: attribution_rules metrics 覆盖缺口分析

## 1. 三套 metrics 列表对比

### A. MetricsWhitelist（config.ini 当前值，17 个）
```
% Shaders Busy, % Shaders Stalled, % Time ALUs Working,
% Time Shading Fragments, % Time Shading Vertices,
Fragments Shaded, Vertices Shaded, LRZ Pixels Killed,
Textures / Fragment,
% Texture Fetch Stall, % Texture L1 Miss, % Texture Pipes Busy,
Read Total (Bytes), Write Total (Bytes),
Preemptions, Avg Preemption Delay, Clocks
```

### B. 完整 Adreno 可用 counter（config.ini 注释列出，约 48 个）

分 5 组：Shader/ALU（19）、Geometry/Primitive（9）、Texture（11）、
Memory Bandwidth（7）、Vertex Fetch/Stall（2）、Misc（3）。

### C. attribution_rules.json layer1_metric_hints（12 个，snake_case key）
```
tex_fetch_stall_pct, tex_l1_miss_pct, tex_l2_miss_pct, tex_mem_read_bytes,
shaders_busy_pct, fragment_instructions, fragments_shaded, vertices_shaded,
vertex_instructions, vertex_mem_read_bytes, read_total_bytes, write_total_bytes
```

### D. DrawCallMetrics typed fields（13 个，当前代码绑定的字段）
与 C 完全一致，另加 `Clocks`（layer1 中不参与 hint，但用于排序/聚合）。

---

## 2. 当前缺口

### 缺口 1：一些 MetricsWhitelist 中有值但 attribution_rules 没有规则的 metrics

| counter（原始名）| 当前在 DB 中 | layer1_hints | layer3_weights | typed field |
|----------------|:-----------:|:------------:|:--------------:|:-----------:|
| % Shaders Stalled | ✅ | ❌ | ❌ | ❌ |
| % Time ALUs Working | ✅ | ❌ | ❌ | ❌ |
| % Time Shading Fragments | ✅ | ❌ | ❌ | ❌ |
| % Time Shading Vertices | ✅ | ❌ | ❌ | ❌ |
| LRZ Pixels Killed | ✅ | ❌ | ❌ | ❌ |
| Textures / Fragment | ✅ | ❌ | ❌ | ❌ |
| % Texture Pipes Busy | ✅ | ❌ | ❌ | ❌ |
| Preemptions | ✅ | ❌ | ❌ | ❌ |
| Avg Preemption Delay | ✅ | ❌ | ❌ | ❌ |
| Clocks | ✅ | ❌（排序用）| ❌ | ✅ |

### 缺口 2：完整 Adreno 列表中有诊断价值但当前 whitelist 未收录的 metrics

例如：
- `% Shaders Stalled`（已在 whitelist）→ 与 `% Shaders Busy` 结合可判断 stall vs busy 比例
- `% Wave Context Occupancy` — occupancy 低 → 内存 latency hiding 不足
- `% Instruction Cache Miss` — 指令缓存缺失 → 着色器分支复杂
- `% Vertex Fetch Stall` — 顶点拉取停滞
- `Avg Bytes / Fragment`、`Avg Bytes / Vertex` — 每片元/顶点带宽
- `% Time Compute`、`% Shader ALU Capacity Utilized`
- `Fragment ALU Instructions (Full/Half)`、`Fragment EFU Instructions`
- `Textures / Vertex`

---

## 3. 归因权重（Layer 2/3）的计算逻辑 — 是否需要归一？

### Layer 2（percentile tier 权重）

```json
"tiers": [
  { "name": "p95", "threshold": 0.95, "weight": 0.80 },
  { "name": "p80", "threshold": 0.80, "weight": 0.30 },
  { "name": "p70", "threshold": 0.70, "weight": 0.15 }
]
```

每个 suspicious metric 取最高超过的 tier 的 weight（0.15/0.30/0.80）。
这是一个**相对于 percentile 阈值的离散权重**，不是比例，不需要归一。

### Layer 3（bottleneck 贡献权重）

```
scoreAccumulator[bottleneck] += ps.WeightApplied * contribWeight
```

`contribution_weight` 是 bottleneck 内部不同 metric 的相对重要度系数，
例如 `tex_fetch_stall_pct → texture_bound: 1.0`, `tex_l1_miss_pct → texture_bound: 0.6`。

**bottleneck 之间分数不做归一**，直接比较大小取 primary/secondary。
`primary_bottleneck_min_score: 0.25` 是最低分数阈值（过滤无实质 bottleneck 的 DC）。

### 用户说"算权重时要归一"的含义

用户的意思：**当 DB 中实际存在的 metrics 数量不固定时，不同 DC 之间的得分基准不一致**，
例如一个 DC 有 8 个 metrics 触发，另一个只有 3 个，直接比较 raw score 不公平。

当前代码并**没有对 bottleneck 分数做归一**，这是一个真实的不一致问题。

归一方法有两种：

**方法 A — 按 bottleneck 的理论最高分归一**

每个 bottleneck 的理论最高分 = `sum(tier_weight_max × all_contrib_weights) for all contributing metrics`。
例如 `texture_bound` = `0.80 × (1.0 + 0.6 + 0.4)` = `1.60`。
归一后分数 = `raw_score / theoretical_max`，范围 [0,1]。

优点：跨 bottleneck 可比，缺点：当某些 contributing metrics 不在 whitelist 时，
理论最高分结果偏高，归一分反而偏低。

**方法 B — 按实际触发的 metrics 数量归一**

归一分 = `raw_score / active_contributing_metrics_count`。

优点：自适应可用 metrics 集合；缺点：不同 bottleneck 贡献 metric 数量不同，
比较仍不公平。

**方法 C（推荐）— 标准化 contribution_weight 使各 bottleneck 满分相等**

在 Layer 3 定义中，对每个 bottleneck 的 contributing_metrics 的 `contribution_weight` 做归一，
使其 sum = 1.0。然后乘以 tier_weight，所有 bottleneck 的理论最高分 = `0.80`（p95 weight）。
这样跨 bottleneck 比较天然公平，且不受 whitelist 覆盖度影响（缺失 metric 只影响分子）。

---

## 4. StatusJsonService — percentile/avg 块与 Dictionary 的关系

`BuildAvgBlock()` / `BuildPercentilesAtLevel()` / `BuildPercentileBlock()` 当前：
- 对 13 个固定 key 计算 `dcs.Average(d => d.Metrics!.XxxField)`
- 输出的 JSON key = snake_case（`"clocks"`、`"fragments_shaded"` 等）

改为 Dictionary 后，这些方法变成：
- 枚举所有 DC 的 `Metrics.All` 的 key 并集（或 whitelist key 列表）
- 对每个 key 计算 `dcs.Where(d => d.Metrics.All.ContainsKey(k)).Average(d => d.Metrics.All[k])`
- 输出 snake_case key（需要 `CounterToKey` 映射）

同时，`global_percentiles` 和 `category_stats.metrics_p70/p80/p95` 这些 JSON 块
**就是 AttributionRuleEngine 的 percentile 查找源**，它们的 key 必须与
`attribution_rules.json layer1_metric_hints` 中的 metric snake_case key 一致。

**这是打通 whitelist → DB → StatusJson → AttributionRuleEngine 的关键链路**：

```
MetricsWhitelist (config)
  → DB DrawCallMetrics table（MetricName IN whitelist）
  → DrawCallMetrics.All（Dictionary，key = 原始 counter 名）
  → StatusJsonService（计算 percentile，key = snake_case via CounterToKey）
  → status.json global_percentiles / category_stats（snake_case key）
  → AttributionRuleEngine.Attribute()（读 percentile，key = snake_case，与 rules 一致）
```

---

## 5. 扩展 attribution_rules 到完整 metrics 的建议

### 需要扩展的内容

1. **layer1_metric_hints**：新增当前 whitelist 中有值但没有 hint 的 metrics，
   例如 `% Shaders Stalled`、`% Time ALUs Working`、`Textures / Fragment`、
   `LRZ Pixels Killed`、`Preemptions`。
   同时，完整 Adreno 列表中语义明确的 counter 也应加入。

2. **layer3_bottleneck_weights**：对现有 6 个 bottleneck 补充 contributor metrics，
   并按方法 C 归一各 bottleneck 的 contribution_weight sum = 1.0。

3. **新建 bottleneck 类型**（可选）：
   - `preemption`（Preemptions / Avg Preemption Delay 高）
   - `lrz_kill`（LRZ Pixels Killed 高 → early-Z 效率）
   - `vertex_fetch`（% Vertex Fetch Stall 高）
   - `occupancy`（% Wave Context Occupancy 低）

4. **CounterToKey 映射**（在 DrawCallMetrics 中）：
   新增所有 counter 名 → snake_case key 的映射条目，
   确保 StatusJsonService 能输出正确 key 给 AttributionRuleEngine 读取。

### 扩展不影响的内容

- Layer 2 的 percentile tier 定义——不需改
- `primary_bottleneck_min_score`——不需改
- 已有 bottleneck 的 hint 和 weight——保留，可扩充优化
- AttributionRuleEngine 计算逻辑——不需改，json 驱动
