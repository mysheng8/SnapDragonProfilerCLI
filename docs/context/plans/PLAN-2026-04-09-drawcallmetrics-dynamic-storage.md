---
type: plan
topic: DrawCallMetrics 去除 typed properties，改为纯 Dictionary 动态存储
status: proposed
based_on:
  - FINDING-2026-04-09-metrics-hardcode-vs-config.md
related_paths:
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/DashboardGenerationService.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/source/Services/Analysis/DcContentAnalysisService.cs
related_tags: [metrics, DrawCallMetrics, refactor, config, dictionary]
summary: |
  DrawCallMetrics 改为纯 Dictionary<string,double> All 存储，完全移除 typed properties。
  MetricsQueryService Step 3 直接赋值 All = mdict（counter名来自MetricsWhitelist过滤后的DB）。
  下游 7 文件将 m.Clocks → m.G("Clocks")（辅助扩展方法），lambda 语法不变。
  AttributionRuleEngine.GetMetricValues() → return m.All（简化）。
  TopDcJsonService.BuildMetricsNode() → 直接序列化 m.All（简化）。
last_updated: 2026-04-09
---

# Plan: DrawCallMetrics 改为纯动态 Dictionary 存储

## 背景 & 用户诉求

用户要求 `DrawCallMetrics` 的属性不能硬编码，应通过 config 配置。
完整调研见 FINDING-2026-04-09-metrics-hardcode-vs-config.md。

硬编码存在两个层次：
- **层 A**：`MetricsQueryService G(mdict, "Clocks")` — counter 名 → property 赋值
- **层 B**：下游 7 个文件的 `m.Clocks`、`m.FragmentsShaded` 等 typed property 调用

本计划对应**方案 2（完全动态）**：完全移除 typed properties，counter 集合由 `MetricsWhitelist` 决定。

---

## 核心改动

### 1. `DrawCallModels.cs` — DrawCallMetrics 改为纯动态

```csharp
/// <summary>
/// Per-DrawCall GPU performance metrics.
/// All counter values are stored in All, keyed by Adreno counter name
/// (same strings as MetricsWhitelist in config.ini).
/// Use the G() helper or index directly: m.All["Clocks"]
/// </summary>
public class DrawCallMetrics
{
    public string DrawCallNumber { get; set; } = "";
    public string ApiName        { get; set; } = "";

    /// <summary>
    /// Raw counter values from DB DrawCallMetrics table.
    /// Keys = MetricName strings (e.g. "Clocks", "% Shaders Busy").
    /// Populated by MetricsQueryService; filtered by MetricsWhitelist.
    /// </summary>
    public Dictionary<string, double> All { get; set; } =
        new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

    /// <summary>Convenience accessor — returns 0.0 if key absent.</summary>
    public double G(string counterName) =>
        All.TryGetValue(counterName, out var v) ? v : 0.0;
}
```

**移除的字段**：`Clocks`、`ReadTotalBytes`、`WriteTotalBytes`、`FragmentsShaded`、
`VerticesShaded`、`ShadersBusyPct`、`TexL1MissPct`、`TexL2MissPct`、`TexFetchStallPct`、
`FragmentInstructions`、`VertexInstructions`、`TexMemReadBytes`、`VertexMemReadBytes`

---

### 2. `MetricsQueryService.cs` — Step 3 完全消除 G() 硬编码

```csharp
// Step 3: 直接用 mdict 构造 DrawCallMetrics — 无硬编码字符串
foreach (var kv in apiIdToDrawIdx)
{
    string apiId = kv.Key;
    uint   dcIdx = kv.Value;
    if (!drawIdxToMetrics.TryGetValue(dcIdx, out var mdict)) continue;

    result[apiId] = new DrawCallMetrics
    {
        DrawCallNumber = apiId,
        ApiName        = apiIdToName.TryGetValue(apiId, out var n) ? n : "",
        All            = mdict,   // MetricsWhitelist 已在 SQL 过滤
    };
}
```

本地 `G()` 函数也可删除。

---

### 3. 下游文件改写策略

所有 `m.Clocks` / `m.FragmentsShaded` 等替换为 `m.G("Clocks")` / `m.G("Fragments Shaded")`。

counter 名字符串（Adreno 标准名称）从被消除的 typed property 移到调用处，
但只需保留少量核心计算字段，其余可直接用 `m.All` 序列化。

#### `AttributionRuleEngine.cs` — GetMetricValues() 简化

```csharp
// Before: 显式映射 13 个字段
private static Dictionary<string, double> GetMetricValues(DrawCallMetrics m) =>
    new Dictionary<string, double> { ["clocks"] = m.Clocks, ... };

// After: 直接返回 All（counter 名已是 Adreno 原始名，rule engine 用 lowercase key 匹配）
// 注意：rule engine 使用 snake_case key（如 "clocks"），需要一次转换
private static Dictionary<string, double> GetMetricValues(DrawCallMetrics m)
{
    // m.All key = "Clocks"，rule key = "clocks"，统一 lowercase
    var d = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
    foreach (var kv in m.All)
        d[kv.Key.ToLowerInvariant().Replace(" ", "_").Replace("%_", "pct_").TrimStart('_')] = kv.Value;
    return d;
}
```

> **注意**：AttributionRuleEngine 的 rule key 格式是 snake_case（如 `clocks`、`fragments_shaded`），
> 而 `m.All` 的 key 是原始 counter 名（如 `Clocks`、`Fragments Shaded`）。
> 需要一个规范化函数，或在 AttributionRuleEngine 内部直接使用 `m.All`（改 rule key 格式）。
> **这是关键决策点**，见下方风险 R2。

#### `TopDcJsonService.cs` — BuildMetricsNode() 简化

```csharp
// Before: 显式映射 13 个字段
private static JObject BuildMetricsNode(DrawCallMetrics m) => new JObject { ["clocks"] = m.Clocks, ... };

// After: 将 m.All 直接转为 JObject（使用 snake_case 规范化后的 key）
private static JObject BuildMetricsNode(DrawCallMetrics m)
{
    var jo = new JObject();
    foreach (var kv in m.All)
        jo[NormalizeKey(kv.Key)] = kv.Value;  // "Clocks" → "clocks", "Read Total (Bytes)" → "read_total_bytes"
    return jo;
}
```

#### `ReportGenerationService.cs` — 排序 lambda

```csharp
// Before:
withM.OrderByDescending(d => d.Metrics!.Clocks)

// After:
withM.OrderByDescending(d => d.Metrics!.G("Clocks"))
```

---

## 关键决策点

### R1: counter 名规范化

`m.All` 的 key = Adreno 原始名（`"Clocks"`、`"Read Total (Bytes)"`、`"% Shaders Busy"`）  
下游 JSON / rule engine 使用 snake_case（`"clocks"`、`"read_total_bytes"`、`"shaders_busy_pct"`）

**需要一个规范化函数**（或在 Model 层提供）。建议放在 `DrawCallMetrics` 中：

```csharp
/// <summary>counter 名 → JSON/rule key 规范化（"Read Total (Bytes)" → "read_total_bytes"）</summary>
public static string NormalizeKey(string counterName) =>
    counterName
        .ToLowerInvariant()
        .Replace("% ", "pct_")
        .Replace("(bytes)", "bytes")
        .Replace(" ", "_")
        .Replace("(", "").Replace(")", "")
        .Trim('_');
```

### R2: AttributionRuleEngine rule key 映射

rule json 中使用 `"metric": "clocks"` 格式。若用 `NormalizeKey()` 函数，
`"Clocks" → "clocks"` 正确，`"Read Total (Bytes)" → "read_total_bytes"` 也正确，
但需要验证所有 counter 名的规范化结果与 rule json 中的 key 完全一致。

**Executor 必须先枚举所有 `attribution_rules.json` 中用到的 metric key，
逐一验证 `NormalizeKey(原始counter名)` 输出匹配。**

### R3: JSON 序列化重复

若 `DrawCallMetrics` 被 Newtonsoft 序列化，`All` 字典会序列化，建议加 `[JsonIgnore]`，
由 `BuildMetricsNode()` 控制输出格式。

---

## 改动文件清单

| 文件 | 改动类型 | 改动量 |
|-----|---------|-------|
| `DrawCallModels.cs` | 移除 13 个 typed properties，加 `All` + `G()` + `NormalizeKey()` | 小 |
| `MetricsQueryService.cs` | Step 3 改为直接赋 `All = mdict`，删除本地 `G()` | 小 |
| `ReportGenerationService.cs` | `m.Clocks` → `(long)m.G("Clocks")` 等，约 30 处 | 中 |
| `DashboardGenerationService.cs` | 同上，约 8 处 | 小 |
| `AttributionRuleEngine.cs` | `GetMetricValues()` 用 NormalizeKey() 转换；验证 rule key 匹配 | 中 |
| `TopDcJsonService.cs` | `BuildMetricsNode()` 改为序列化 `m.All` | 小 |
| `StatusJsonService.cs` | 显式字段改为 `m.All` 迭代 | 中 |
| `CaptureReportService.cs` | `m.Clocks` → `(long)m.G("Clocks")` | 小 |
| `DcContentAnalysisService.cs` | 同上 | 小 |
| `AttributionReportService.cs` | 同上 | 小 |

---

## 实现顺序

1. 改 `DrawCallModels.cs`：DrawCallMetrics 移除 typed properties，加 `All`/`G()`/`NormalizeKey()`
2. 改 `MetricsQueryService.cs`：Step 3 直接 `All = mdict`
3. 验证 `attribution_rules.json` 中所有 metric key 与 NormalizeKey 输出匹配
4. 改 `AttributionRuleEngine.cs`：GetMetricValues() 改用 NormalizeKey 转换
5. 改 `TopDcJsonService.cs`：BuildMetricsNode() 序列化 m.All
6. 改 `StatusJsonService.cs`：显式字段改为 m.All 迭代
7. 改 `ReportGenerationService.cs`：所有 m.Clocks 等替换
8. 改 `DashboardGenerationService.cs` / `CaptureReportService.cs` / `DcContentAnalysisService.cs` / `AttributionReportService.cs`
9. `dotnet build` 验证

---

## 执行状态

Implementation requires the Executor agent.


## 背景

见 FINDING-2026-04-09-metrics-hardcode-vs-config.md。

`G(mdict, "Clocks")` 等 13 条字符串字面量是"Adreno counter 名 → typed property"的
结构绑定，无法被 `MetricsWhitelist`（平铺名称集合）直接替代。

**要从根本上消除这些字符串，需改变 `DrawCallMetrics` 的内部存储结构。**

---

## 方案：Dictionary 作为底层存储

### 核心思路

```
Before：
  G(mdict, "Clocks")  →  DrawCallMetrics.Clocks = (long)value

After：
  DrawCallMetrics.All = mdict (直接赋值，无 G() 调用)
  DrawCallMetrics.Clocks 变成计算属性：
    public long Clocks => (long)G("Clocks");
```

字符串字面量从 `MetricsQueryService` 移回 `DrawCallMetrics` 类内部，
作为 counter 名 ↔ property 的规范文档。下游消费者（7 个文件）调用语法不变。

---

## 实现范围

### 1. `DrawCallModels.cs` — `DrawCallMetrics` 类改写

```csharp
public class DrawCallMetrics
{
    // 底层存储：Adreno counter 名 → 值（直接来自 DB）
    public Dictionary<string, double> All { get; set; } =
        new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

    private double G(string key) =>
        All.TryGetValue(key, out var v) ? v : 0.0;

    // 元信息（不在 All 里）
    public string DrawCallNumber { get; set; } = "";
    public string ApiName        { get; set; } = "";

    // 只读计算属性 — counter 名字符串集中在这里，作为规范文档
    public long   Clocks               => (long)G("Clocks");
    public long   ReadTotalBytes       => (long)G("Read Total (Bytes)");
    public long   WriteTotalBytes      => (long)G("Write Total (Bytes)");
    public long   FragmentsShaded      => (long)G("Fragments Shaded");
    public long   VerticesShaded       => (long)G("Vertices Shaded");
    public double ShadersBusyPct       => G("% Shaders Busy");
    public double TexL1MissPct         => G("% Texture L1 Miss");
    public double TexL2MissPct         => G("% Texture L2 Miss");
    public double TexFetchStallPct     => G("% Texture Fetch Stall");
    public long   FragmentInstructions => (long)G("Fragment Instructions");
    public long   VertexInstructions   => (long)G("Vertex Instructions");
    public long   TexMemReadBytes      => (long)G("Texture Memory Read BW (Bytes)");
    public long   VertexMemReadBytes   => (long)G("Vertex Memory Read (Bytes)");
}
```

### 2. `MetricsQueryService.cs` — Step 3 简化

```csharp
// Step 3：直接用 mdict 构造 DrawCallMetrics，无需 G() 调用
foreach (var kv in apiIdToDrawIdx)
{
    string apiId = kv.Key;
    uint   dcIdx = kv.Value;
    if (!drawIdxToMetrics.TryGetValue(dcIdx, out var mdict)) continue;

    result[apiId] = new DrawCallMetrics
    {
        DrawCallNumber = apiId,
        ApiName        = apiIdToName.TryGetValue(apiId, out var n) ? n : "",
        All            = mdict,   // 直接赋值，MetricsWhitelist 已过滤
    };
}
```

### 3. 下游 7 个文件 — **无需改动**

`m.Clocks`、`m.FragmentsShaded` 等调用语法与现在完全相同，
只是从 stored field 换成 computed property，对消费者透明。

---

## 额外收益

- `DrawCallMetrics.All` 暴露了 DB 中所有读到的 counter 值，可直接序列化
  进 JSON，便于未来新增 counter 不需改 C# 代码
- `MetricsWhitelist` 控制哪些 counter 被写入 DB（capture 侧）和被读出
  （SQL filter），字符串字面量只作为 property ↔ counter 的规范注释存在于
  `DrawCallMetrics` 类中，职责清晰

---

## 风险

| 风险 | 说明 |
|------|------|
| JSON 序列化 | 若 `DrawCallMetrics` 被 Newtonsoft 序列化，computed property 默认
也会序列化（重复），需检查是否加 `[JsonIgnore]` |
| 性能 | 每次访问 typed property 都走 Dictionary lookup；metric 量少（<20 条
per DC）可接受 |

---

## 实现顺序

1. 改 `DrawCallModels.cs`：DrawCallMetrics 改为 Dictionary 底层存储
2. 改 `MetricsQueryService.cs`：Step 3 直接赋 `All = mdict`，删除 G() 本地函数
3. 验证下游文件编译（不改动，仅验证）
4. dotnet build 通过

---

## 不在本计划范围内

- `MetricsWhitelist` 的格式或语义不变
- 下游服务（Report / Dashboard / Attribution / TopDc）不修改
- JSON 序列化重复字段留给后续 review

---

## 执行状态

Implementation requires the Executor agent.
