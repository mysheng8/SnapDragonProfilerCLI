---
type: plan
topic: metrics-join-fix
status: proposed
based_on:
  - FINDING-2026-04-01-metrics-join-zero.md
related_paths:
  - SDPCLI/source/Services/Analysis/MetricsCsvService.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
related_tags: [metrics, join, db-query, capture-id]
summary: >
  Fix metrics join 0/328 by changing LoadMetricsFromSession to query the
  DB for DrawCallApiID→DrawcallIdx mapping (filtered by CaptureID) instead
  of reading the stale DrawCallParameters.csv file.
last_updated: 2026-04-01
---

## Problem
`LoadMetricsFromSession` reads `DrawCallParameters.csv` to build the
`DrawCallApiID → DrawcallIdx` mapping, but this CSV is stale (old apiIDs)
while the DB contains the correct apiIDs for the current CaptureID.

Result: 0 metrics rows joined to any DrawCall in the analysis output.

## Proposed Change

### 1. `MetricsCsvService.LoadMetricsFromSession` — new signature

```csharp
public Dictionary<string, DrawCallMetrics> LoadMetricsFromSession(
    string sessionDir,
    string dbPath = null,
    uint captureId = 0)
```

**Step 1 logic** (build apiId → DrawcallIdx):

```csharp
var apiIdToDrawIdx = new Dictionary<string, uint>();

// Priority: query DB (always correct, CaptureID-filtered)
if (!string.IsNullOrEmpty(dbPath) && captureId > 0 && File.Exists(dbPath))
{
    using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;Read Only=True;"))
    {
        conn.Open();
        // Detect CaptureID column
        bool hasCaptureId = ColumnExists(conn, "DrawCallParameters", "CaptureID");
        string where = hasCaptureId ? $"WHERE CaptureID={captureId}" : "";
        using (var cmd = new SQLiteCommand(
            $"SELECT DrawCallApiID, DrawcallIdx FROM DrawCallParameters {where}", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string apiId = reader["DrawCallApiID"].ToString();
                if (uint.TryParse(reader["DrawcallIdx"].ToString(), out uint idx))
                    apiIdToDrawIdx[apiId] = idx;
            }
        }
    }
}

// Fallback: CSV file (may be stale for multi-capture SDPs)
if (apiIdToDrawIdx.Count == 0)
{
    // ... existing CSV parsing code ...
}
```

Need to add `ColumnExists` (or inline PRAGMA table_info query — same pattern
already used in `DrawCallQueryService`).

Add `using System.Data.SQLite;` to the file.

### 2. `AnalysisPipeline.cs` — update call site

```csharp
// Before:
metrics = metricsService.LoadMetricsFromSession(metricsSearchDir);

// After:
metrics = metricsService.LoadMetricsFromSession(metricsSearchDir, dbPath, captureId);
```

`dbPath` and `captureId` are already in scope at that point in the method.

### 3. No other changes needed

- The `DrawCallMetrics.csv` parsing (Step 2) is correct — `DrawID` = `DrawcallIdx`
- The final join (Step 3) is correct — result keyed by `DrawCallApiID`
- The AnalysisPipeline join `metrics.TryGetValue(dc.DrawCallNumber, ...)` is correct

## Expected Outcome After Fix

```
→ Loaded 328 metric rows from: …\snapshot_2
→ Joined metrics to 328 / 328 DCs   (or however many have metrics data)
```

## Risk
- Low. Fallback to CSV parsing preserved for offline/legacy scenarios.
- No schema changes required.
- SQLite connection opened read-only, no write impact.

## Notes on `DrawID` semantics
`DrawCallMetrics.csv.DrawID` = `DrawcallIdx` (1-based sequential index within
the command buffer). Confirmed by: Parameters CSV has DrawcallIdx=1,2,3,4;
Metrics CSV has DrawID values sparse within the same range (1-N).
