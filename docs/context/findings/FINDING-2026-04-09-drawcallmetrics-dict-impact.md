---
type: finding
topic: DrawCallMetrics 改为 Dictionary 的完整下游影响分析
status: investigated
related_paths:
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/DashboardGenerationService.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/source/Services/Analysis/AttributionReportService.cs
  - SDPCLI/source/Services/Analysis/DcContentAnalysisService.cs
related_tags: [metrics, DrawCallMetrics, dictionary, impact-analysis, downstream]
summary: |
  DrawCallMetrics 改为纯 Dictionary 后，下游消费按模式分 4 类：
  (P1) JSON 序列化——直接简化，迭代 All；
  (P2) 排序/聚合 lambda——字符串查找替换 m.Clocks → m.G("Clocks")；
  (P3) Func<DrawCallMetrics,double> lambda 列表——写法不变，仅内部改字典查找；
  (P4) switch/case 映射——可用字典查找直接替代 switch。
  影响文件共 9 个；规范化函数（counter名→snake_case）是关键共享组件。
last_updated: 2026-04-09
---

# Finding: DrawCallMetrics 改 Dictionary 的完整下游影响

## 1. 现有 DrawCallMetrics 的 13 个 typed fields

```
Clocks               ← "Clocks"
ReadTotalBytes       ← "Read Total (Bytes)"
WriteTotalBytes      ← "Write Total (Bytes)"
FragmentsShaded      ← "Fragments Shaded"
VerticesShaded       ← "Vertices Shaded"
ShadersBusyPct       ← "% Shaders Busy"
TexL1MissPct         ← "% Texture L1 Miss"
TexL2MissPct         ← "% Texture L2 Miss"
TexFetchStallPct     ← "% Texture Fetch Stall"
FragmentInstructions ← "Fragment Instructions"
VertexInstructions   ← "Vertex Instructions"
TexMemReadBytes      ← "Texture Memory Read BW (Bytes)"
VertexMemReadBytes   ← "Vertex Memory Read (Bytes)"
```

改为 Dictionary 后，所有访问形式：`m.Clocks` → `m.G("Clocks")` 或 `(long)m.All["Clocks"]`

---

## 2. 下游使用模式分类（P1~P4）

### P1 — 全字段显式 JSON 序列化（最容易简化）

这类代码把 13 个字段逐一写到 JObject/匿名对象，改为字典后可以**循环替代**。

**文件/方法**：

| 文件 | 方法 | 改法 |
|-----|-----|-----|
| `ReportGenerationService.cs` | `GenerateLabeledMetricsJson()` `metricsNode` 匿名对象 (13 行) | `m.All` 转匿名对象或直接用 JObject(m.All) |
| `StatusJsonService.cs` | `BuildAvgBlock()` JObject 13 行 | 遍历固定 key 列表，对每个 key 求 Average |
| `StatusJsonService.cs` | `BuildPercentilesAtLevel()` JObject 13 行 | 遍历 key 列表计算 Percentile |
| `StatusJsonService.cs` | `BuildPercentileBlock()` JObject 13 行 | 遍历 key 列表 |
| `TopDcJsonService.cs` | `BuildMetricsNode()` JObject 13 行 | 序列化 m.All（需 counter名→snake_case 规范化）|
| `AttributionRuleEngine.cs` | `GetMetricValues()` Dictionary 13 行 | 直接 return m.All（或 normalize keys）|

**关键依赖**：`StatusJsonService` 中遍历时需要固定 key 列表
（因为要计算所有 DC 对同一 key 的 Average/Percentile，必须先知道 key 集合）。
解法：从 `MetricsWhitelist` 解析出 key 列表，或用第一个 DC 的 `All.Keys`。

**`AttributionRuleEngine.GetMetricValues()`**：rule engine 用 snake_case key
（如 `"clocks"`、`"fragments_shaded"`），而 `m.All` 是原始 counter 名
（`"Clocks"`、`"Fragments Shaded"`）。**需要规范化函数**。

---

### P2 — 排序 / 聚合 lambda（量最大，改法固定）

```csharp
// Before
withM.OrderByDescending(d => d.Metrics!.Clocks)
withM.Sum(d => d.Metrics!.Clocks)
withM.Average(d => (double)d.Metrics!.Clocks)

// After（使用辅助方法 G）
withM.OrderByDescending(d => d.Metrics!.G("Clocks"))
withM.Sum(d => d.Metrics!.G("Clocks"))
withM.Average(d => d.Metrics!.G("Clocks"))
```

**文件/行数**：

| 文件 | 涉及字段 | 大约行数 |
|-----|---------|---------|
| `ReportGenerationService.cs` | Clocks（主）, FragmentsShaded, ReadTotalBytes, WriteTotalBytes, ShadersBusyPct, TexFetchStallPct, TexL1MissPct, FragmentInstructions, VerticesShaded | ~30 处 |
| `DashboardGenerationService.cs` | Clocks, FragmentsShaded, ReadTotalBytes, WriteTotalBytes, TexFetchStallPct, TexL1MissPct, FragmentInstructions, ShadersBusyPct, VerticesShaded | ~15 处 |
| `TopDcJsonService.cs` | Clocks, TexFetchStallPct, ShadersBusyPct | ~6 处 |
| `StatusJsonService.cs` | Clocks（排序/汇总）, ReadTotalBytes, WriteTotalBytes, FragmentsShaded, VerticesShaded | ~10 处 |
| `CaptureReportService.cs` | Clocks, FragmentsShaded, ShadersBusyPct, TexFetchStallPct, ReadTotalBytes, WriteTotalBytes, FragmentInstructions, VertexInstructions, TexL1MissPct | ~20 处 |
| `AttributionReportService.cs` | Clocks | ~5 处 |
| `DcContentAnalysisService.cs` | Clocks | 1 处 |

总计约 **87 处**，全是机械替换，无逻辑变化。

---

### P3 — `Func<DrawCallMetrics, double>` lambda 列表（写法保留，内部改）

```csharp
// Before（DashboardGenerationService.cs / ReportGenerationService.cs）
m => (double)m.FragmentsShaded,
(m, a) => $"{m.FragmentsShaded:N0} ({m.FragmentsShaded/Math.Max(a,1):F1}×)"

// After
m => m.G("Fragments Shaded"),
(m, a) => { var v = (long)m.G("Fragments Shaded"); return $"{v:N0} ({v/Math.Max(a,1):F1}×)"; }
```

**文件**：`ReportGenerationService.cs` candidateCols 列表（8 项）、`DashboardGenerationService.cs` candidateCols 列表（8 项）。

lambda 签名 `Func<DrawCallMetrics, double>` 不变，只改 lambda 体内部。
**无需改接口，只改实现字符串**。

---

### P4 — switch/case 按 metric key 分发（可用字典直接替代）

```csharp
// Before（ReportGenerationService.cs BuildOutlierSignals）
switch (t.Metric)
{
    case "fragments_shaded":  value = m.FragmentsShaded;  avg = avgFrags;  break;
    case "shaders_busy_pct":  value = m.ShadersBusyPct;   avg = avgBusy;   break;
    case "read_total_bytes":  value = m.ReadTotalBytes;    avg = avgRead;   break;
    // ...
    default: continue;
}

// After
// 构建 avg 字典（与 rule key 对应）
var metricAvgs = new Dictionary<string, double> {
    ["clocks"]               = avgClocks,
    ["fragments_shaded"]     = avgFrags,
    ["shaders_busy_pct"]     = avgBusy,
    ...
};
// 用规范化函数把 m.All 的 key 转为 snake_case
var normalized = m.All.ToDictionary(
    kv => DrawCallMetrics.NormalizeKey(kv.Key), kv => kv.Value,
    StringComparer.OrdinalIgnoreCase);
double value = normalized.GetValueOrDefault(t.Metric, 0);
double avg   = metricAvgs.GetValueOrDefault(t.Metric, 0);
```

**文件**：`ReportGenerationService.cs BuildOutlierSignals()`（7 个 case）、`CaptureReportService.cs BuildDcTags()`（类似 switch）。

switch 可完全消除，改为字典查找。

---

## 3. 规范化函数（NormalizeKey）的必要性

`m.All` 存储原始 Adreno counter 名，下游 JSON key / rule key 全部是 snake_case。
必须有统一的转换函数：

```
"Clocks"                        → "clocks"
"Fragments Shaded"              → "fragments_shaded"
"% Shaders Busy"                → "shaders_busy_pct"   ← "%" 需特殊处理
"Read Total (Bytes)"            → "read_total_bytes"    ← 括号需去掉
"% Texture L1 Miss"             → "tex_l1_miss_pct"     ← 多词缩写
"Texture Memory Read BW (Bytes)"→ "tex_mem_read_bytes"  ← 缩写
"% Texture Fetch Stall"         → "tex_fetch_stall_pct"
"Vertex Memory Read (Bytes)"    → "vertex_mem_read_bytes"
```

**关键问题**：`"% Texture L1 Miss"` → `"tex_l1_miss_pct"` **无法纯粹通过机械规则推导**（因为 "Texture L1 Miss" 没有单词 "tex"）。
这意味着纯字符串变换无法覆盖所有 counter 名，**仍然需要一张显式映射表**：

```csharp
// 在 DrawCallMetrics 中维护
private static readonly Dictionary<string, string> CounterToKey = new()
{
    ["Clocks"]                         = "clocks",
    ["Read Total (Bytes)"]             = "read_total_bytes",
    ["Write Total (Bytes)"]            = "write_total_bytes",
    ["Fragments Shaded"]               = "fragments_shaded",
    ["Vertices Shaded"]                = "vertices_shaded",
    ["% Shaders Busy"]                 = "shaders_busy_pct",
    ["% Texture L1 Miss"]              = "tex_l1_miss_pct",
    ["% Texture L2 Miss"]              = "tex_l2_miss_pct",
    ["% Texture Fetch Stall"]          = "tex_fetch_stall_pct",
    ["Fragment Instructions"]          = "fragment_instructions",
    ["Vertex Instructions"]            = "vertex_instructions",
    ["Texture Memory Read BW (Bytes)"] = "tex_mem_read_bytes",
    ["Vertex Memory Read (Bytes)"]     = "vertex_mem_read_bytes",
};

public static string NormalizeKey(string counterName) =>
    CounterToKey.TryGetValue(counterName, out var k) ? k
    : counterName.ToLowerInvariant().Replace(" ", "_").Replace("%", "pct").Replace("(", "").Replace(")", "").Trim('_');
```

这张映射表**仍然是硬编码**，但它集中在 DrawCallMetrics 一处，
且仅处理"已知 counter 名 → JSON key" 的翻译，逻辑上是正确的归属位置。
新增 counter（MetricsWhitelist 新增条目）时只需在此表加一行，
不需改动任何下游服务。

---

## 4. 影响汇总

| 改动类型 | 文件数 | 改动量 | 逻辑变化 |
|---------|-------|-------|---------|
| P1 JSON 序列化 — 简化或用循环替代 | 5 | 小 | 低（相同输出） |
| P2 排序/聚合 lambda — 机械替换 | 7 | 中（~87处） | 无 |
| P3 Func<DrawCallMetrics,double> lambda — 内部改字典查找 | 2 | 小 | 无 |
| P4 switch/case — 改字典查找 | 2 | 小 | 低 |
| DrawCallMetrics + NormalizeKey/CounterToKey | 1 | 小 | 核心 |
| MetricsQueryService Step 3 — 直接赋 All | 1 | 极小 | 高价值 |

---

## 5. 结论

**改为 Dictionary 后，下游代码量净减少**（P1 类从 13 行变为循环/m.All；P4 类 switch 消除）。
P2 类是量最大的机械替换，但全部是 `m.Clocks` → `m.G("Clocks")` 形式，
可以用正则批量替换辅助。

最关键的共享基础设施是 `DrawCallMetrics.CounterToKey` 映射表，
它决定了"原始 counter 名 → snake_case JSON key"的转换是否正确。
此映射表仍然是硬编码，但职责明确、位置唯一，未来只需在此一处维护。
