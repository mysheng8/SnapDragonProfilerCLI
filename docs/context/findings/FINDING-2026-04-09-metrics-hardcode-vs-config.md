---
type: finding
topic: MetricsQueryService 中 G(mdict, "Clocks") 等字符串与 MetricsWhitelist 的关系
status: investigated
related_paths:
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/DashboardGenerationService.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/config.ini
related_tags: [metrics, config, hardcode, DrawCallMetrics, MetricsWhitelist]
summary: |
  G(mdict, "Clocks") 等字符串是 Adreno counter 名 → C# typed property 的结构绑定，
  不等同于 MetricsWhitelist。MetricsWhitelist 已作为 SQL 过滤器正确使用。
  彻底消除绑定字符串需要将 DrawCallMetrics 从 typed class 改为
  Dictionary<string,double>，影响 7+ 下游文件。
last_updated: 2026-04-09
---

# Finding: MetricsQueryService 硬编码问题调研

## 1. 现状

`MetricsQueryService.LoadMetrics()` 的 Step 3 中：

```csharp
result[apiId] = new DrawCallMetrics
{
    Clocks               = (long)G(mdict, "Clocks"),
    ReadTotalBytes       = (long)G(mdict, "Read Total (Bytes)"),
    FragmentsShaded      = (long)G(mdict, "Fragments Shaded"),
    ShadersBusyPct       = G(mdict, "% Shaders Busy"),
    // ... 共 13 个字段
};
```

这 13 个字符串字面量是 Adreno GPU counter 名，来自 DB 表 `DrawCallMetrics.MetricName` 列。

## 2. MetricsWhitelist 的实际作用

`config.ini` 中：
```ini
MetricsWhitelist=% Shaders Busy,...,Clocks
```

该配置有**两个使用点**：

| 使用位置 | 作用 |
|---------|------|
| `CaptureExecutionService` | capture 时决定设备激活哪些 counter，写入 DB |
| `MetricsQueryService` Step 2 | 生成 `WHERE MetricName IN (...)` SQL 过滤条件 |

`MetricsWhitelist` 是一个**平铺的名称集合**，控制"哪些 counter 存在 DB 中"。
它并不定义"哪个 counter 名 → 哪个 C# 属性"。

## 3. G(mdict, "Clocks") 字符串的本质

这些字符串是两套命名空间之间的**结构绑定（structural binding）**：

```
Adreno counter name (DB)      C# typed property (DrawCallMetrics)
─────────────────────────     ──────────────────────────────────
"Clocks"               →      DrawCallMetrics.Clocks
"Read Total (Bytes)"   →      DrawCallMetrics.ReadTotalBytes
"% Shaders Busy"       →      DrawCallMetrics.ShadersBusyPct
"% Texture L1 Miss"    →      DrawCallMetrics.TexL1MissPct
...
```

这个映射是固定的，由 GPU vendor 的 counter 命名规范决定，与"是否在
whitelist 里"无关。`MetricsWhitelist` 是 counter 子集选择；这里是名称翻译。

## 4. 下游依赖规模（typed properties 被消费的文件）

| 文件 | 用到的 typed property 数量 |
|-----|--------------------------|
| `ReportGenerationService.cs` | 10+ (Clocks, FragmentsShaded, ReadTotalBytes, WriteTotalBytes, ShadersBusyPct, TexFetchStallPct, TexL1MissPct, FragmentInstructions, VertexInstructions, TexMemReadBytes) |
| `DashboardGenerationService.cs` | 4 (Clocks, ShadersBusyPct, ReadTotalBytes, WriteTotalBytes) |
| `AttributionRuleEngine.cs` | 8 |
| `TopDcJsonService.cs` | 8 |
| `CaptureReportService.cs` | 2 (Clocks) |
| `DcContentAnalysisService.cs` | 3 (Clocks, FragmentsShaded, ...) |

共计 **7 个文件**，通过 `DrawCallInfo.Metrics.XXX` 访问 typed properties。

## 5. 能否用 MetricsWhitelist 消除 G(mdict,...) 中的字符串？

**直接答案：不能在保留现有 typed class 的前提下消除。**

原因：
- `MetricsWhitelist` 是个 `HashSet<string>`，不描述"哪个名字映射到哪个属性"
- typed property 赋值必须在编译时确定字段名，runtime 字符串无法直接映射到 `long Clocks`
- 若把 `MetricsWhitelist` 当映射来源，仍然需要一张"counter 名 → property setter"的硬编码表，效果与现在相同

**要彻底消除结构开销，需要改变 DrawCallMetrics 的类型**：
- 方案：`DrawCallMetrics.All = Dictionary<string, double>`，typed properties 变成
  `public long Clocks => (long)(All.TryGetValue("Clocks", out var v) ? v : 0);`
- 代价：要修改 `DrawCallModels.cs` + `MetricsQueryService.cs`，下游 7 个文件
  **不需改动**（typed property 接口不变）

## 6. 结论

当前 `G(mdict, "Clocks")` 等字符串：
- **不是可配置的**（它们是 GPU vendor 固定的 counter 名）
- **不能被 MetricsWhitelist 替代**（两者职责不同）
- **MetricsWhitelist 已经被正确使用**：SQL `WHERE MetricName IN (...)` 过滤

若要去除 `DrawCallMetrics` 构造中的硬编码字符串，唯一路径是将 `DrawCallMetrics`
改为以 `Dictionary<string, double>` 为底层存储，typed properties 作为只读计算属性。
