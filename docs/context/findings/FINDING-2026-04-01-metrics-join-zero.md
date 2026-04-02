---
type: finding
topic: metrics-join-zero
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/MetricsCsvService.cs
  - SDPCLI/test/2026-04-01T20-59-18/snapshot_2/DrawCallParameters.csv
  - SDPCLI/test/2026-04-01T20-59-18/snapshot_2/DrawCallMetrics.csv
related_tags: [metrics, join, stale-csv, multi-capture]
summary: >
  Metrics join reports 0/328 DCs matched despite loading 696 metric rows.
  Root cause: LoadMetricsFromSession reads a stale DrawCallParameters.csv
  with wrong DrawCallApiIDs (4528+) while the analysis DCs use IDs 57252+.
  Fix: query the DB (DrawCallParameters table, filtered by CaptureID) for
  the apiId→DrawcallIdx mapping instead of the stale CSV file.
last_updated: 2026-04-01
---

## Symptom
```
→ Loaded 696 metric rows from: …\snapshot_2
→ Joined metrics to 0 / 328 DCs
```

## Root Cause (three-layer)

### Layer 1 — Stale CSV
`snapshot_2/DrawCallParameters.csv` contains **old data** from a previous
capture/replay session:
- Old format: **no CaptureID column** (pre-multi-capture-fix)
- DrawCallApiIDs: `4528, 4538, 4571, …` (from a different replay)
- 697 draw call rows (`DrawcallIdx` = 1…697)

The file is not regenerated when analysis runs, so it retains stale content.

### Layer 2 — Key mismatch
`LoadMetricsFromSession` builds a metrics dict keyed by `DrawCallApiID`
from the CSV (keys = 4528, 4538, …).

The analysis pipeline has `DrawCallInfo.DrawCallNumber` = apiId from the DB
(values = 57252, 57258, … for CaptureID=2).

Zero intersection → `metrics.TryGetValue(dc.DrawCallNumber, ...)` always misses.

### Layer 3 — DrawID mapping assumption
`DrawCallMetrics.csv` uses `DrawID` (sparse values: 9, 42, 114, 167, …)
which corresponds to `DrawcallIdx` in the DrawCallParameters table —
a sequential 1-based index per command buffer, NOT the DrawCallApiID.

The internal join `DrawcallIdx → DrawCallApiID` is correct in logic but
breaks when the CSV source for DrawCallApiID is stale.

## Evidence

DrawCallParameters.csv header (old format, no CaptureID):
```
DrawCallApiID,ApiName,SubmitIdx,CmdBufferIdx,DrawcallIdx,...
4528,vkCmdDraw,3,1,1,...
4538,vkCmdDraw,3,1,2,...
```

DrawCallMetrics.csv (correct, CaptureID=2):
```
CaptureID,DrawID,SubmitCount,ReplayHandleID,MetricID,MetricName,Value
2,9,1,...
2,42,1,...
```

Analysis DC range from DB (CaptureID=2):
```
DrawCallApiID = 57252, 57258, 57261, … (328 DCs)
```

Console log:
```
[INFO] Loaded 696 metric rows from: …\snapshot_2
[INFO] Joined metrics to 0 / 328 DCs
```

## Fix

**Change `MetricsCsvService.LoadMetricsFromSession`** to accept two new
parameters `string dbPath` and `uint captureId`, and use the DB for the
`DrawCallApiID → DrawcallIdx` mapping instead of the CSV:

```csharp
// New signature:
public Dictionary<string, DrawCallMetrics> LoadMetricsFromSession(
    string sessionDir, string dbPath, uint captureId)

// Step 1: query DB instead of CSV
// SELECT DrawCallApiID, DrawcallIdx FROM DrawCallParameters
// WHERE CaptureID = @captureId
// Fall back to CSV parsing only if table/DB not available.
```

**Update call site in `AnalysisPipeline.cs`**:
```csharp
// Pass dbPath and captureId to the service
metrics = metricsService.LoadMetricsFromSession(metricsSearchDir, dbPath, captureId);
```

## Why device connection is NOT broken
The console log for the current session contains ONLY `-mode analysis` runs —
no `-mode snapshot` run appears. The code changes in this session did not
touch any device connection logic (`DeviceConnectionService`, `SDPClient`,
`WaitForDeviceReady`, `EstablishNetworkConnection`). The README change is
documentation-only and has zero runtime effect.
