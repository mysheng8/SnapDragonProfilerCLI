---
type: implementation
topic: ReportGenerationService → RawJsonGenerationService rename and cleanup
status: completed
based_on:
  - FINDING-2026-04-09-reportgenerationservice-cleanup.md
related_paths:
  - SDPCLI/source/Services/Analysis/RawJsonGenerationService.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Application.cs
summary: Renamed ReportGenerationService to RawJsonGenerationService, deleted all LLM/summary/markdown deprecated methods and helpers, deleted CaptureReportService, updated all callers.
last_updated: 2026-04-09
---

## Plan Reference

`FINDING-2026-04-09-reportgenerationservice-cleanup.md` — confirmed `GenerateSummaryReport` and all LLM helpers were dead code paths; `CaptureReportService` had no active consumers (Step 5 was already commented out).

## Implementation Summary

1. Deleted ~850 lines of deprecated methods from `ReportGenerationService.cs`:
   - `GenerateSummaryReport()` (Markdown summary, LLM-driven)
   - `OutlierThreshold` class + `GetOutlierThresholds()` + `BuildOutlierSignals()`
   - `GetRuleBasedConclusion()`, `GetLlmBottleneckConclusion()`, `GetLlmOverallConclusion()`
   - `RotateBitmap90CW()`, `GenerateMarkdownReport()` shim, `CatStats` class
2. Removed `_llm`, `_outlierThresholds` fields and `SetLlm()` from the class header (done in prior session).
3. Replaced bottom section with clean compat shims (`GenerateCsvReport`, `GenerateJsonReport` ×2) and `Q()`.
4. Renamed file: `ReportGenerationService.cs` → `RawJsonGenerationService.cs`
5. Deleted `CaptureReportService.cs` entirely.
6. Updated `AnalysisPipeline.cs`:
   - Field type `ReportGenerationService` → `RawJsonGenerationService`
   - Removed `captureReportService` field declaration and instantiation
   - Constructor parameter type updated
7. Updated `Application.cs`:
   - Constructor call changed to `new RawJsonGenerationService(config, logger)`
   - Removed `reportService.SetLlm(llmService)` call

## Files Changed

| File | Change |
|------|--------|
| `source/Services/Analysis/RawJsonGenerationService.cs` | Renamed from `ReportGenerationService.cs`; deleted ~850 lines of deprecated methods |
| `source/Services/Analysis/CaptureReportService.cs` | **Deleted** |
| `source/Analysis/AnalysisPipeline.cs` | Field/constructor types updated; `captureReportService` removed |
| `source/Application.cs` | Constructor call and `SetLlm()` removed |

## Key Changes

- `RawJsonGenerationService` now has one active method: `GenerateLabeledMetricsJson()`, plus `GenerateLabeledMetricsCsv()` and compat shims.
- No LLM dependency in this service. LLM labeling is handled only by `DrawCallLabelService`.
- `CaptureReportService` (report.json generator) fully removed — Step 5 was already commented out in pipeline.

## Build / Validation

```
Build succeeded.
1 Warning(s) — CS8604 pre-existing null reference warning in AnalysisPipeline.cs:100
0 Error(s)
```

## Deviations from Plan

- Used PowerShell line-range deletion for the ~850-line block (file manipulation) instead of `replace_string_in_file`, due to exact string matching limitations on large blocks.
- Q() method had shell-escaping corruption after PowerShell write → fixed with `replace_string_in_file`.

## Issues Encountered

- `replace_string_in_file` failed on 800+ line exact match in prior session (Rep 3 failure).
- PowerShell string interpolation in array literals mangled Q() method double-quotes → required a follow-up fix.

## Next Steps

- None. Refactoring complete. Build clean.
