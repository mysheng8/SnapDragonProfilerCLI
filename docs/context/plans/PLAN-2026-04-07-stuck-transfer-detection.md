---
type: plan
topic: stuck-transfer-detection
status: proposed
based_on:
  - FINDING-2026-04-07-stuck-transfer-queue.md
  - FINDING-2026-04-07-snapshot5-empty-metrics.md
related_paths:
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
related_tags: [detection, replay-metrics, stuck, warning, diagnostics]
summary: >
  Add _replayMetricsEvent (ManualResetEvent) as an early stuck-detection gate.
  Set it in OnDataProcessed when REPLAY_METRICS_DATA fires. Use it in
  WaitForDataProcessed() with a 90s timeout before the existing 180s wait.
  If timeout → emit specific stuck warning with adb cleanup command.
last_updated: 2026-04-07
---

## Objective

Detect stuck sdpservice transfer queues earlier and with actionable messaging,
replacing the current generic 180s generic timeout.

---

## Background

See [FINDING-2026-04-07-stuck-transfer-queue.md](../findings/FINDING-2026-04-07-stuck-transfer-queue.md).

Key constraints:
- `SHADER_DATA` and `GFXRECONSTRUCT_DATA` are NOT observable from C# (`OnDataProcessed`
  does not fire for them — they are internal QGLPlugin C++ events).
- `REPLAY_METRICS_DATA` IS observable via `OnDataProcessed` (confirmed in CliClientDelegate.cs L248).
- Normal: REPLAY_METRICS fires within ~20s of `_captureCompleteEvent`.
- Stuck: REPLAY_METRICS never fires.

---

## Implementation Steps

### Step 1 — Add `_replayMetricsEvent` to `CliClientDelegate.cs`

**Field** (alongside other events near top of class):
```csharp
private ManualResetEvent? _replayMetricsEvent;
```

**Setter** (alongside `SetDataProcessedEvent`, `SetImportCompleteEvent`):
```csharp
public void SetReplayMetricsEvent(ManualResetEvent evt) => _replayMetricsEvent = evt;
```

**Signal block** (inside `OnDataProcessed`, in the existing `BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA`
block, after the existing `lock (_metricsBuffers)` caching):
```csharp
if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA
    && _expectedCaptureIdForSignal != 0
    && captureID == _expectedCaptureIdForSignal)
{
    AppLogger.Info("Delegate", $"  ✓ REPLAY_METRICS received for capture {captureID} — signaling replayMetrics");
    _replayMetricsEvent?.Set();
}
```

### Step 2 — Add `_replayMetricsEvent` field to `SnapshotCaptureMode.cs`

**Field** (alongside `_dataProcessedEvent` and `_importCompleteEvent`):
```csharp
private readonly ManualResetEvent _replayMetricsEvent = new ManualResetEvent(false);
```

**Wire-up** in `InitializeClient()` (alongside other `Set...Event()` calls):
```csharp
simpleDelegate.SetReplayMetricsEvent(_replayMetricsEvent);
```

**Reset** at loop head (alongside `_captureCompleteEvent.Reset()`, `_importCompleteEvent.Reset()`):
```csharp
_replayMetricsEvent.Reset();
```

### Step 3 — Refactor `WaitForDataProcessed()` to two-stage wait

Replace current single-stage implementation:

```csharp
private bool WaitForDataProcessed()
{
    // Stage 1: wait for REPLAY_METRICS (earliest device signal)
    Console.WriteLine("\nWaiting for device to begin data transfer (REPLAY_METRICS)...");
    _log.Info("[WaitForData] Stage 1: waiting for REPLAY_METRICS_DATA — timeout=90s");
    bool gotMetrics = _replayMetricsEvent.WaitOne(TimeSpan.FromSeconds(90));

    if (!gotMetrics)
    {
        // Transfer queue is stuck — REPLY_METRICS never arrived from device
        _log.Warning("[WaitForData] ⚠ REPLAY_METRICS_DATA not received within 90s.");
        _log.Warning("[WaitForData]   sdpservice transfer queue is likely stuck (device storage full?).");
        _log.Warning("[WaitForData]   Fix: adb shell rm /data/local/tmp/SnapdragonProfiler/*.gfxrz");
        Console.WriteLine("\n  ⚠ WARNING: Device transfer queue appears stuck — this capture will produce no data.");
        Console.WriteLine("  To recover: adb shell rm /data/local/tmp/SnapdragonProfiler/*.gfxrz");
        Console.WriteLine("  Then restart the capture session.");
        return false;  // caller should skip ReplayAndGetBuffers
    }

    _log.Info("[WaitForData] Stage 1 done — REPLAY_METRICS received. Stage 2: waiting for API data (bufferID=2)...");
    Console.WriteLine("REPLAY_METRICS received — waiting for full API data...");

    // Stage 2: wait for PROCESSED_API_DATA bufferID=2 (gfxrz transfer + processing done)
    // Remaining budget: 180s total - time already spent in stage 1 (at most 90s)
    bool ready = _dataProcessedEvent.WaitOne(TimeSpan.FromSeconds(90));
    if (!ready)
    {
        _log.Warning("[WaitForData] API data (bufferID=2) not received within 90s after REPLAY_METRICS.");
        _log.Warning("[WaitForData]   gfxrz file transfer may have failed mid-transfer.");
        Console.WriteLine("  WARNING: gfxrz file transfer did not complete — replay may produce empty results.");
        return false;
    }

    _log.Info("[WaitForData] API data ready — proceeding to replay.");
    Console.WriteLine("API data ready — proceeding to replay.");
    return true;
}
```

### Step 4 — Update call site in capture loop

In the capture loop (currently line 191 area):
```csharp
// Before:
WaitForDataProcessed();

// After:
bool dataReady = WaitForDataProcessed();
if (!dataReady)
{
    _log.Warning($"[Capture] Capture {captureId} produced no data — skipping ReplayAndGetBuffers.");
    // still proceed to _importCompleteEvent.WaitOne so import state is clean
    // then show "Press ENTER" as usual
    goto waitForImport;  // or restructure with early continue
}
// ... existing ReplayAndGetBuffers etc.
waitForImport:
```

> Note: The exact restructuring depends on whether a label/flag or an early-continue
> pattern is preferred. The important contract is: if `WaitForDataProcessed` returns
> `false`, `ReplayAndGetBuffers` is skipped entirely (no point calling ImportCapture
> on empty data), but the loop continues so the user can press ENTER for next capture.

---

## Timeout Rationale

| Stage | Timeout | Basis |
|---|---|---|
| `_replayMetricsEvent` (Stage 1) | 90s | Normal = ~20s. 90s gives 4.5× margin for large/slow captures. Any stuck case will far exceed 90s. |
| `_dataProcessedEvent` (Stage 2) | 90s | Normal = ~5s after REPLAY_METRICS. 90s covers the largest observed gfxrz transfers (snap4 = 29s + processing). |
| Total worst-case | 180s | Same as before — but split into two stages with specific diagnostics. |

---

## Distinguishable Failure Modes After This Change

| Scenario | Stage that fails | Log message |
|---|---|---|
| Normal operation | — (both pass) | "API data ready — proceeding to replay" |
| Device storage full / queue stuck | Stage 1 (90s) | "REPLAY_METRICS_DATA not received... transfer queue stuck" |
| gfxrz transfer partial / corrupted | Stage 2 (90s) | "gfxrz file transfer did not complete" |
| REPLAY_METRICS empty (metrics window collision) | Stage 1 passes (empty buffer), Stage 2 may still pass | Existing empty-metrics behavior unchanged |

---

## Files to Modify

| File | Change |
|---|---|
| `SDPCLI/source/CliClientDelegate.cs` | +field `_replayMetricsEvent`, +setter, +signal block in REPLAY_METRICS section |
| `SDPCLI/source/Modes/SnapshotCaptureMode.cs` | +field, +wire-up, +reset, refactor `WaitForDataProcessed()`, update call site |

No other files need changes.

---

## Implementation requires the Executor agent.
