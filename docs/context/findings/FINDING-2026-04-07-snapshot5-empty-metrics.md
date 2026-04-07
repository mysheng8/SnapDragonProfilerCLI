---
type: finding
topic: snapshot5-empty-metrics
status: investigated
related_paths:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Services/Capture/CaptureExecutionService.cs
  - SDPCLI/test/2026-04-07T11-51-24/sdplog.txt
related_tags: [metrics, importcapture, replay, race-condition, drawcall-metrics]
summary: >
  snapshot_5 has no DrawCallMetrics.csv because the device-side GPU metrics
  hardware collected 0 per-drawcall entries. Root cause: snap5's 1-second
  metrics window (12:02:17–12:02:18) overlapped with ImportCapture(4)'s
  second device-side replay (which completed at 12:02:21).
last_updated: 2026-04-07
---

## Observable Symptom

`snapshot_5/DrawCallMetrics.csv` does not exist.
All other CSVs (DrawCallParameters, DrawCallBindings, etc.) exist correctly.
The analysis summary shows:

> No metrics CSV loaded — set `AnalysisMetricsCSV` in config.ini to enable sections 4.1–4.3.

## Evidence

### sdplog event comparison

**snap4 (working)** — REPLAY_METRICS at `12:01:26`:
```
12:01:26.862 VulkanTraceProcessor: Adding compute stage/draw ...  (×N)
12:01:26.865 VulkanProcessor: Drawcall metrics data received from an old SCOPE version (ver: 20).
12:01:26.865 VulkanProcessor: Snapshot drawcall metrics transfer and processing: 343.986200 ms
```

**snap5 (broken)** — REPLAY_METRICS at `12:03:54`:
```
12:03:54.345 VulkanProcessor: QGLPluginProcessor received a BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA
12:03:56.369 VulkanProcessor: QGLPluginProcessor received a BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA
(no VulkanTraceProcessor events, no processing log)
```

The SDK received a REPLAY_METRICS buffer for snap5 but processed **0 drawcall entries**.
`QGLPluginService.GetLocalBuffer(BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA, ...)` therefore
returns null/empty → `_metricsBuffers[5]` is never populated.

### Timeline reconstruction

| Timestamp | Event |
|---|---|
| 12:01:57 | snap4 APIs=415154 → `_dataProcessedEvent` fires |
| 12:01:57 | main thread: `ReplayAndGetBuffers(4)` → `ImportCapture(4)` called |
| 12:01:58 | SDK: snap4 drawcall metrics re-processed (2nd pass from ImportCapture) |
| **12:02:17** | snap5: metrics ACTIVATE — 1-second window begins |
| **12:02:18** | snap5: metrics DEACTIVATE |
| **12:02:21** | `Number of apis: 0` — ImportCapture(4)'s 2nd replay trailing event arrives |

ImportCapture(4)'s second device-side replay was still executing (12:02:21)
while snap5's 1-second metrics window ran (12:02:17–12:02:18).

## Root Cause

`ReplayAndGetBuffers(captureId)` calls `QGLPluginService.ImportCapture(captureId, dbPath)`
which triggers a **second replay** on the device. This second replay:

1. Runs asynchronously on the device after our main thread has already moved on.
2. Occupies the GPU's per-drawcall metrics hardware during the replay (collecting metrics
   for the replayed draw calls, not for the live application).
3. When the NEXT capture's metrics are activated immediately after, the 1-second collection
   window collides with the ongoing replay activity on device.
4. The device collects 0 valid per-drawcall entries for snap5 during that 1 second.
5. An empty REPLAY_METRICS_DATA buffer is sent back — no CSV is written.

## Why snap2/3/4/6 worked but snap5 didn't

| Capture | ENTER pressed (approx) | Prev ImportCapture tail |
|---|---|---|
| snap2 → snap3 | ~11:56:14 | ~11:54:13 (apis:0) — completed ~2 min earlier ✓ |
| snap3 → snap4 | ~11:59:47 | ~11:58:13 (apis:0) — completed ~1.5 min earlier ✓ |
| snap4 → snap5 | ~12:02:12 | 12:02:21 (apis:0) — still in flight! ✗ |
| snap5 → snap6 | ~12:05:41 | 12:04:43 (apis:0) — completed ~1 min earlier ✓ |

For snap5, the user pressed ENTER only ~15 seconds after snap4's `WaitForDataProcessed`
fired, which was not enough time for the device-side ImportCapture(4) replay to complete.
For all other transitions, there was at least 60–90 seconds between captures.

## Contributing Factor: Redundant ImportCapture

`ReplayAndGetBuffers` unconditionally calls `ImportCapture(captureId, dbPath)`
even though the SDK has ALREADY done its own initial replay (which populated the DB
and fired the `_dataProcessedEvent` via bufferID=2). This second ImportCapture call
generates an unnecessary third party replay that conflicts with subsequent captures.

## Proposed Fixes

### Fix 1 (safest): Skip ImportCapture if DB is already populated

In `ReplayAndGetBuffers`, before calling `ImportCapture`:
- Query `VulkanSnapshotGraphicsPipelines` row count.
- If rows > 0 (SDK's initial replay already populated the DB), skip `ImportCapture`.
- Only call `ImportCapture` if the DB is empty (fallback for edge cases).

```csharp
// Check if the DB was already populated by SDK's own initial replay
bool dbAlreadyPopulated = false;
try {
    using var conn = new SQLiteConnection(...);
    conn.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines";
    int rows = Convert.ToInt32(cmd.ExecuteScalar());
    dbAlreadyPopulated = rows > 0;
} catch { }

bool imported;
if (dbAlreadyPopulated) {
    _log.Info("[ImportCapture] DB already populated — skipping ImportCapture to avoid replay conflict");
    imported = true;
} else {
    imported = QGLPluginService.ImportCapture(captureId, dbPath);
}
```

### Fix 2 (belt-and-suspenders): Add pre-activation delay in StartCapture

In `CaptureExecutionService.StartCapture()`, before the metrics activation loop,
add a configurable delay (`MetricsActivationDelay`, default 2000ms) to allow
any in-flight device-side replay to settle:

```csharp
int activationDelay = _config.GetInt("MetricsActivationDelay", 2000);
if (activationDelay > 0) {
    Console.WriteLine($"  Waiting {activationDelay}ms before metrics activation...");
    Thread.Sleep(activationDelay);
}
```

Config key to add:
```ini
# Milliseconds to wait before activating metrics (allows previous replay to settle).
# Increase if DrawCallMetrics.csv is missing in rapid consecutive captures.
# Default: 2000
MetricsActivationDelay=2000
```

### Recommended: Apply both Fix 1 and Fix 2

Fix 1 eliminates the core cause (redundant ImportCapture).
Fix 2 acts as a safety margin for any other concurrent device activities.

## Impact on Other Captures

- Fix 1 eliminates the second device-side replay entirely for captures where
  the SDK already processed the data. This speeds up `ReplayAndGetBuffers`
  and removes the main source of interference.
- Fix 2 adds 2 seconds per capture at the start — a minor cost compared to
  the 60–120 second overall capture cycle.

## Status

**Implementation requires the Executor agent.**
