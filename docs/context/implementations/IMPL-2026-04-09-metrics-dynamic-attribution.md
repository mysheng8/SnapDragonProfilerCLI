---
type: implementation
topic: DrawCallMetrics Dictionary化 + 完整 Adreno counter 归因体系
status: completed
based_on:
  - PLAN-2026-04-09-drawcallmetrics-dynamic-storage.md
  - PLAN-2026-04-09-attribution-full-metrics.md
related_paths:
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
  - SDPCLI/analysis/attribution_rules.json
  - SDPCLI/config.ini
summary: |
  将 DrawCallMetrics 改为 Dictionary<string,double> All 为 backing store，
  完整 CounterToKey (~50 entries) 为单一真值来源；
  所有 hardcoded 的 13-field 引用替换为动态遍历；
  attribution_rules.json 扩至 26 layer1 hints + 7 bottleneck（已归一权重）；
  MetricsWhitelist 扩至 50 条全量 Adreno counter。
last_updated: 2026-04-09
---

## Plan Reference

- PLAN-2026-04-09-drawcallmetrics-dynamic-storage.md
- PLAN-2026-04-09-attribution-full-metrics.md

## Implementation Summary

### 阶段 A（本 session 前半部分，已在 token budget 前完成）
- `DrawCallModels.cs`：DrawCallMetrics 完全改写；typed stored fields → `Dictionary<string, double> All`；
  保留 computed typed properties（`Clocks`, `ReadTotalBytes` 等）用于向后兼容；
  新增 `CounterToKey`（~50 条）+ `NormalizeKey()`。
- `MetricsQueryService.cs`：Step 3 改为 `All = mdict` 直接赋值；
  Step 2 SQL WHERE 条件从 MetricsWhitelist 动态构建；
  补加缺失的 `using System.Linq;`（build error，iteration 1 修复）。

### 阶段 A（本 session 后半部分）
- `StatusJsonService.cs`：`BuildAvgBlock`, `BuildPercentilesAtLevel`, `BuildPercentileBlock` 全部改为动态遍历 `d.Metrics!.All.Keys`；
  输出 key 通过 `DrawCallMetrics.NormalizeKey()` 转 snake_case。

### 阶段 B
- `AttributionRuleEngine.cs`：`GetMetricValues()` 改为一行 `m.All.ToDictionary(kv => DrawCallMetrics.NormalizeKey(kv.Key), ...)`。
- `TopDcJsonService.cs`：`BuildMetricsNode()` 改为遍历 `m.All` loop。
- `attribution_rules.json`：layer1 hints 从 12 扩至 26 条（新增 shaders_stalled_pct, time_alus_working_pct, wave_context_occupancy_pct, instruction_cache_miss_pct, tex_pipes_busy_pct, textures_per_fragment, lrz_pixels_killed, vertex_fetch_stall_pct, preemptions, avg_preemption_delay, fragment_alu_instr_full, fragment_efu_instructions, 以及早期 2 条已存在）；layer3 contribution_weight 按方法 C 归一（sum=1.0 per bottleneck）；新增 `preemption` bottleneck。
- `config.ini`：MetricsWhitelist 从 17 条扩至 50 条全量 Adreno counter。

## Files Changed

| File | Change Type |
|------|-------------|
| `SDPCLI/source/Models/DrawCallModels.cs` | Rewrite DrawCallMetrics class |
| `SDPCLI/source/Services/Analysis/MetricsQueryService.cs` | Dynamic SQL filter + `All=mdict` + added using System.Linq |
| `SDPCLI/source/Services/Analysis/StatusJsonService.cs` | Replace 3 hardcoded builder methods |
| `SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs` | Simplify GetMetricValues() |
| `SDPCLI/source/Services/Analysis/TopDcJsonService.cs` | Simplify BuildMetricsNode() |
| `SDPCLI/analysis/attribution_rules.json` | Extend layer1 + normalize layer3 + add preemption |
| `SDPCLI/config.ini` | Expand MetricsWhitelist to 50 counters |

## Key Changes

### DrawCallMetrics

```csharp
// Before: 13 typed stored fields
public long Clocks { get; set; }
public long ReadTotalBytes { get; set; }
// ...

// After: Dictionary backing store + computed typed properties
public Dictionary<string, double> All { get; set; } = new(StringComparer.OrdinalIgnoreCase);
public double G(string counterName) => All.TryGetValue(counterName, out var v) ? v : 0.0;
public long Clocks => (long)G("Clocks");       // backward-compatible
public long ReadTotalBytes => (long)G("Read Total (Bytes)");
// ...
public static readonly IReadOnlyDictionary<string, string> CounterToKey = ...  // ~50 entries
public static string NormalizeKey(string counterName) => ...
```

### Layer3 weight normalization (Method C)

| Bottleneck | Before (sum) | After (sum) |
|---|---|---|
| texture_bound | 1.0+0.6+0.4 = 2.0 | 0.50+0.30+0.20 = 1.00 |
| shader_alu | 1.0+0.5 = 1.5 | 0.67+0.33 = 1.00 |
| fragment_bound | 1.0+0.3 = 1.3 | 0.77+0.23 = 1.00 |
| overdraw | 1.0+0.4 = 1.4 | 0.71+0.29 = 1.00 |
| bandwidth | 1.0+0.8+0.6+0.5 = 2.9 | 0.34+0.28+0.21+0.17 = 1.00 |
| model_cost | 1.0+0.7+0.5 = 2.2 | 0.45+0.32+0.23 = 1.00 |
| preemption (new) | — | 0.60+0.40 = 1.00 |

## Build / Validation

- command: `dotnet build SDPCLI.sln -c Debug --nologo`
- result: **Build succeeded** (iteration 2, after fixing missing `using System.Linq;`)

## Deviations from Plan

- `% Texture L2 Miss` counter was missing from the original config.ini MetricsWhitelist but referenced in layer3 `bandwidth` bottleneck; now included in the expanded whitelist.
- bandwidth bottleneck weight: 0.35+0.28+0.21+0.17 = 1.01 adjusted to 0.34+0.28+0.21+0.17 = 1.00 for exactness.

## Issues Encountered

1. `MetricsQueryService.cs` was missing `using System.Linq;` → `string[].Select()` CS1061 error. Fixed by adding the using directive (iteration 1).

## Next Steps

- Monitor device-side memory pressure: 50-counter whitelist may add ~1GB+ profiler layer overhead on device. If needed, expose a `EnableFullMetrics=true/false` switch in config.ini.
- Verify GPU profiler actually exports the new counters (e.g. `Fragment ALU Instructions (Full)`, `% Wave Context Occupancy`) by running a capture and inspecting the sdp.db `DrawCallMetrics` table.
- AttributionRuleEngine Layer3 score computation should be verified against a real sdp with known bottlenecks to confirm normalized scores compare correctly across bottleneck categories.
