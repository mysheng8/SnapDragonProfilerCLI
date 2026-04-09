---
type: finding
topic: ReportGenerationService 职责边界 + report.json 依赖全图 + LLM 分工 + 重命名建议
status: investigated
related_paths:
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/CaptureReportService.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Application.cs
related_tags: [cleanup, deprecated, report.json, raw.json, summary-md, rename, llm]
summary: |
  ReportGenerationService 同时负责 raw.json（活跃，无 LLM）和 DrawCallAnalysis_Summary_*.md（废弃，含 LLM）。
  LLM 打标在 DrawCallLabelService.Label() 完成（Step 2），GenerateLabeledMetricsJson 只是序列化已有结果。
  建议将 ReportGenerationService 删除 Summary 路径后重命名为 RawJsonGenerationService。
  CaptureReportService 整体可删除（report.json，无消费者）。
last_updated: 2026-04-09
---

## 背景

用户误以为 `ReportGenerationService` 只是旧的 report.json 代码，实际上该类同时承担两个不同职责。需要区分哪些方法活跃、哪些可删除。

---

## ReportGenerationService.cs 方法清单

| 方法 | 行号(约) | 输出产物 | 状态 |
|---|---|---|---|
| `GenerateLabeledMetricsJson()` | 34 | `snapshot_{id}_raw.json` | ✅ 活跃，pipeline Step A3 调用 |
| `GenerateJsonReport()` ×2 | 1082/1086 | 转发给上方 | ✅ 兼容转发层，可保留 |
| `GenerateLabeledMetricsCsv()` | 201 | `DrawCallAnalysis_*.csv` | ⚠ 旧格式，无 pipeline 调用，自注释 legacy |
| `GenerateCsvReport()` | 1080 | 转发给上方 | ⚠ 同 CSV |
| `GenerateSummaryReport()` | 248 | `DrawCallAnalysis_Summary_*.md` | ❌ 废弃，pipeline Step 4 已注释掉 |
| `BuildOutlierSignals()` | 732 | 内部工具 | ❌ 仅被 GenerateSummaryReport 调用 |
| `GetOutlierThresholds()` | (懒加载) | 内部缓存 | ❌ 仅被 BuildOutlierSignals 调用 |
| `GetLlmBottleneckConclusion()` | 811 | 内部 LLM | ❌ 仅被 GenerateSummaryReport 调用 |
| `GetLlmOverallConclusion()` | 992 | 内部 LLM | ❌ 仅被 GenerateSummaryReport 调用 |
| `RotateBitmap90CW()` | 1066 | 内部工具 | ❌ 仅被 GenerateSummaryReport 调用（DashboardGenerationService 有独立副本） |
| `GenerateMarkdownReport()` | 1078 | 转发给 GenerateSummaryReport | ❌ 随 Summary 废弃 |
| `CatStats` private class | 1094 | 内部数据 | ❌ 仅被 GenerateSummaryReport 使用 |
| `Q()` helper | 1091 | CSV 工具 | ⚠ 仅被 GenerateLabeledMetricsCsv 使用 |
| `_llm` / `_outlierThresholds` / `SetLlm()` | 18/19/29 | LLM 字段 | ❌ 仅被 GenerateSummaryReport 路径使用 |

---

## report.json 依赖全图

**生成者**：`CaptureReportService.GenerateReport()` → `report.json`

**消费者（全部扫描结果）**：
- `AnalysisPipeline.cs` Step 5（行 417–425）— **已注释掉**
- 无其他 `.cs` 文件引用 report.json 文件名
- Pass B 读取的是 `snapshot_*_raw.json`，不是 report.json

**结论**：report.json 当前既不被生成，也不被任何代码消费。

---

## 两个产物的职责对比

| 产物 | 生成方法 | 状态 | 替代者 |
|---|---|---|---|
| `snapshot_{id}_raw.json` | `ReportGenerationService.GenerateLabeledMetricsJson()` | ✅ 活跃 | — |
| `DrawCallAnalysis_Summary_*.md` | `ReportGenerationService.GenerateSummaryReport()` | ❌ 废弃 | `DashboardGenerationService` |
| `report.json` | `CaptureReportService.GenerateReport()` | ❌ 废弃 | raw + status + topdc 三文件 |
| `DrawCallAnalysis_*.csv` | `ReportGenerationService.GenerateLabeledMetricsCsv()` | ⚠ 未调用 | raw.json |

---

## 类注释误导问题

`ReportGenerationService` 类头注释写的是：
```
// TODO: DEPRECATED — generates DrawCallAnalysis_Summary_*.md (old format). Pending deletion.
```
这是**错误的**——该类同时包含活跃方法 `GenerateLabeledMetricsJson()`，不能整体删除，只能删除 Summary 相关路径。

---

## 可删除代码范围（精确）

### `ReportGenerationService.cs` 中删除：
1. `GenerateSummaryReport()` 整个方法（约 248–730 行，~480 行）
2. `BuildOutlierSignals()` + `GetOutlierThresholds()` + `OutlierThreshold` class（约 732–810 行）
3. `GetLlmBottleneckConclusion()`（约 811–991 行）
4. `GetLlmOverallConclusion()`（约 992–1065 行）
5. `RotateBitmap90CW()`（约 1066–1077 行）
6. `GenerateMarkdownReport()` 转发层（1078–1079 行）
7. `CatStats` private class（1094 行附近）
8. `_llm` 字段、`_outlierThresholds` 字段、`SetLlm()` 方法（行 18/19/29）
9. 类头部 DEPRECATED 注释改为准确描述

### 保留：
- `GenerateLabeledMetricsJson()` — raw.json 生成，活跃
- `GenerateJsonReport()` ×2 — 兼容转发层
- `GenerateLabeledMetricsCsv()` + `GenerateCsvReport()` + `Q()` — 视需要保留或注释

### 整个类删除：
- `CaptureReportService.cs` — 仅生成 report.json，无消费者

### `AnalysisPipeline.cs` 清理：
- 行 37: `captureReportService` 字段声明
- 行 59: `new CaptureReportService(logger)` 实例化
- 行 417–425: 已注释的 Step 5 block（删注释残留）

---

## 风险评估

| 删除项 | 风险 |
|---|---|
| `GenerateSummaryReport()` 及其依赖 | 低：pipeline 已注释，无入口 |
| `CaptureReportService` 整个类 | 低：Step 5 已注释，无其他调用 |
| `_llm` / `SetLlm()` | 中：需确认 `GenerateLabeledMetricsJson()` 内部是否引用 _llm（目前看不引用）|
| `GenerateLabeledMetricsCsv()` | 低：无 pipeline 调用，但保留无害 |
