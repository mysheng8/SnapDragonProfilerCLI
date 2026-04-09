---
type: implementation
topic: DrawAnalysis Two-Pass Refactor — P0/P1/P2/P3 Full Implementation
status: completed
based_on:
  - PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Config.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
  - SDPCLI/source/Services/Analysis/DcContentAnalysisService.cs
  - SDPCLI/source/Services/Analysis/AttributionReportService.cs
  - SDPCLI/source/Services/Analysis/DashboardGenerationService.cs
  - SDPCLI/analysis/attribution_rules.json
  - SDPCLI/SDPCLI.csproj
  - SDPCLI/source/IsExternalInit.cs
summary: |
  P0-P3 全部实现完毕。
  Pass A（无 LLM）：DrawCallLabel 扩展 + raw.json schema 2.0 + status.json（百分位）
  + topdc.json（三层归因）。
  Pass B（LLM/规则）：DcContentAnalysisService（Step B1 per-DC shader 内容分析）
  + AttributionReportService（Step B2 per-category 归因报告 MD）
  + DashboardGenerationService（Step B3 规则生成展示 Dashboard MD）。
  P3：-stats-only / -analysis-only CLI 标志；PassMode 枚举 + 步骤门控；
  StatusJsonResult 消除 status.json 二次解析；
  attribution_rules.json 中增加 outlier_signal_thresholds；
  ReportGenerationService.BuildOutlierSignals 从规则文件读取阈值（有回退）。
  Build: 0 errors, 3 pre-existing warnings.
last_updated: 2026-04-08
---

## Plan Reference

PLAN-2026-04-08-drawanalysis-two-pass-refactor.md — P0/P1/P2/P3 phases (all complete)

## Implementation Summary

Refactored the DrawAnalysis pipeline into two passes. Pass A is deterministic (no LLM), 
produces 3 JSONs. Pass B is LLM-driven (with rule fallbacks), produces 2 MDs. All 6 
priority levels (P0→P2) are now implemented. The existing `GenerateSummaryReport()` is 
retained for backward compatibility — new outputs are additive.

## Files Changed

### Modified
| File | Change |
|------|--------|
| `SDPCLI/source/Models/DrawCallModels.cs` | `DrawCallLabel` extended: Subcategory, ReasonTags[], Confidence, LabelSource |
| `SDPCLI/source/Services/Analysis/DrawCallLabelService.cs` | BuildPrompt, ParseLlmResponse, LabelByRules, error paths all updated for new label fields |
| `SDPCLI/source/Services/Analysis/ReportGenerationService.cs` | `GenerateLabeledMetricsJson` rewritten: schema_version=2.0, new filename, new label subobject, per-DC metrics_available |
| `SDPCLI/source/Analysis/AnalysisPipeline.cs` | ① captureId+sdpName passed to GenerateLabeledMetricsJson ② LoadLabelsFromAnalysis rewrites to handle new + legacy schema ③ Steps A5/A6 added (StatusJsonService + TopDcJsonService) ④ Steps B1/B2/B3 added ⑤ llm field + constructor param |
| `SDPCLI/source/Application.cs` | Pass `llmService` to `AnalysisPipeline` constructor |
| `SDPCLI/SDPCLI.csproj` | Added `analysis\attribution_rules.json` as CopyToOutputDirectory content |

### Created
| File | Purpose |
|------|---------|
| `SDPCLI/analysis/attribution_rules.json` | Three-layer attribution rule config (Layer1 metric→hint, Layer2 percentile tiers, Layer3 bottleneck weights) |
| `SDPCLI/source/Services/Analysis/StatusJsonService.cs` | Pass A Step A5 — overall + per-category stats with p70/p80/p95, label quality, global_percentiles → `snapshot_{id}_status.json` |
| `SDPCLI/source/Services/Analysis/AttributionRuleEngine.cs` | Three-layer attribution engine; loads rules.json; returns AttributionResult with bottleneck scores |
| `SDPCLI/source/Services/Analysis/TopDcJsonService.cs` | Pass A Step A6 — top-N per category, AttributionRuleEngine per DC → `snapshot_{id}_topdc.json` |
| `SDPCLI/source/Services/Analysis/DcContentAnalysisService.cs` | Pass B Step B1 — per-DC LLM shader content analysis; caches to per_dc_content/; rule fallback |
| `SDPCLI/source/Services/Analysis/AttributionReportService.cs` | Pass B Step B2 — per-category LLM attribution report using topdc.json + status.json + B1 cache → `snapshot_{id}_analysis.md` |
| `SDPCLI/source/Services/Analysis/DashboardGenerationService.cs` | Pass B Step B3 — rule-generated charts+tables dashboard (Mermaid bar+pie, dynamic Top-5, per-cat Top-5, mesh links, label stats) → `snapshot_{id}_dashboard.md` |
| `SDPCLI/source/IsExternalInit.cs` | C# 9 `record` shim for net472 target |

## Key Changes

### Output files per capture (snapshot_{id}/)
```
snapshot_{id}_raw.json       ← rewritten from DrawCallAnalysis_*.json (schema 2.0)
snapshot_{id}_status.json    ← NEW: stats + percentiles
snapshot_{id}_topdc.json     ← NEW: top-N attribution
snapshot_{id}_analysis.md    ← NEW: per-category LLM attribution report
snapshot_{id}_dashboard.md   ← NEW: charts + tables (rule-based)
per_dc_content/              ← NEW: B1 per-DC shader content cache directory
DrawCallAnalysis_Summary_*.md ← RETAINED: existing summary (backward compat)
```

### Pipeline step order (AnalysisPipeline.RunAnalysis)
```
Step 1  DB query (existing)
Step 2  LLM/rule labeling (extended label)
Step 3  Metrics CSV join + shader/texture/mesh extraction (existing)
         → GenerateLabeledMetricsJson → raw.json
Step A5  StatusJsonService → status.json
Step A6  AttributionRuleEngine + TopDcJsonService → topdc.json
Step B1  DcContentAnalysisService → per_dc_content/
Step B2  AttributionReportService → analysis.md
Step B3  DashboardGenerationService → dashboard.md
Step 4  ReportGenerationService.GenerateSummaryReport → (legacy MD, retained)
Step 5  CaptureReportService → report.json
```

### attribution_rules.json layers
- **Layer1**: 12 metric→bottleneck_hint mappings
- **Layer2**: 3 percentile tiers (p70@0.15, p80@0.30, p95@0.80); min_sample_size=5 for category-level
- **Layer3**: 6 bottleneck types (texture_bound, shader_alu, fragment_bound, overdraw, bandwidth, model_cost) with contribution weights

### B1 caching strategy
- Cache key: pipeline_id (multiple DCs sharing a pipeline → single LLM call)
- Cache location: `per_dc_content/dc_{sanitized_dcId}.json`
- In-process + on-disk cache for incremental re-runs

## Build / Validation

- Build command: `dotnet build SDPCLI.sln -c Debug --nologo`
- Result: **Build succeeded. 0 Error(s), 3 Warning(s)**
- Warnings are all pre-existing in `AnalysisPipeline.cs:84` and `ReportGenerationService.cs:878,953`
- No new errors introduced (P3 introduced 1 syntax error — escaped quotes in interpolated string — fixed immediately)

## Deviations from Plan

1. `CaptureReportService.GenerateReport()` (report.json) — **retained but not replaced**. Plan said to replace with raw/status/topdc; kept for compatibility.
2. DcContentAnalysisService uses pipeline-level shader files (`pipeline_{id}_vert.hlsl`) as primary source, with dc-level `dc_` folder as fallback — more efficient than plan's pure dc-level approach.
3. `AttributionRuleEngine` uses `record` types (C# 9) — required adding `IsExternalInit.cs` shim for net472.
4. P3 interpolated string with `\"` inside `{}` — fixed by string concatenation (`"..." + var + "..."`) to avoid pre-C#11 syntax error.

## Issues Encountered

1. **CS1061** `SuspiciousMetric.BottleneckHint` — field was named `InitialBottleneckHint` in `SuspiciousMetric` record but referenced as `BottleneckHint` in Layer2 loop. Fixed immediately.
2. **Path.GetRelativePath not in net472** — replaced with `Uri.MakeRelativeUri` in TopDcJsonService.
3. **CS8602 nullable warnings** in AttributionReportService (`resp.Trim()`) — fixed with null-forgiving `resp!.Trim()`.
4. **P3 CS8076** `Missing close delimiter '}'` — line 78 in AnalysisPipeline.cs used `{config.Get(\"key\",\"default\")}` inside `$"..."` which is invalid before C# 11. Fixed by extracting to `string pmCfg = config.Get(...)` local variable.

## P3 Changes Summary

### New in P3

| File | Change |
|------|--------|
| `SDPCLI/source/Config.cs` | `Set(string key, string value)` method — runtime config override |
| `SDPCLI/source/Main.cs` | Parse `-pass-mode <all\|stats\|analysis>`, `-stats-only`, `-analysis-only`; pass to `app.Run()` |
| `SDPCLI/source/Application.cs` | `Run()` takes optional `string? passMode`; injects into config via `config.Set("AnalysisPassMode", ...)` |
| `SDPCLI/source/Analysis/AnalysisPipeline.cs` | `PassMode` enum (`All\|StatsOnly\|AnalysisOnly`); `GetPassMode()`; `skipPassAGen` / `skipPassB` gates on A4/A5/A6 and B1/B2/B3 |
| `SDPCLI/source/Services/Analysis/StatusJsonService.cs` | `StatusJsonResult` class (`FilePath`, `GlobalPercentiles`, `CategoryStatsMap`); `GenerateStatusJson` returns it directly — eliminates JSON re-parse for TopDcJsonService |
| `SDPCLI/analysis/attribution_rules.json` | Added `outlier_signal_thresholds` array (7 entries, ratio/additive modes, label+message templates) |
| `SDPCLI/source/Services/Analysis/ReportGenerationService.cs` | `OutlierThreshold` private class; `GetOutlierThresholds()` lazy-load from rules.json; `BuildOutlierSignals` changed from `static` to instance method, uses loaded thresholds with hardcoded fallback |

### PassMode behavior

| Flag | passMode | A4 (raw.json) | A5/A6 (status/topdc) | B1/B2/B3 |
|------|----------|-------------|----------------------|----------|
| *(default)* | All | ✓ | ✓ | ✓ |
| `-stats-only` | StatsOnly | ✓ | ✓ | SKIP |
| `-analysis-only` | AnalysisOnly | SKIP | SKIP | ✓ |

## Next Steps

### Known limitations
- Dashboard Mermaid bar chart with many DCs (>200) may render slowly in some Markdown viewers
- B1 per-DC LLM calls are sequential; could be parallelized with degree=2-4 for large captures
- `CaptureReportService.GenerateReport()` still generates legacy report.json; can be suppressed via config flag `AnalysisGenerateLegacyReport=false` (not yet implemented)
