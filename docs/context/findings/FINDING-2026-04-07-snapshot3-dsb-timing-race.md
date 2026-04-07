---
type: finding
topic: snapshot3-missing-csvs-dsb-timing-race
status: investigated
related_paths:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/Services/Capture/QGLPluginService.cs
  - SDPCLI/test/2026-04-07T16-16-40/sdplog.txt
  - SDPCLI/test/2026-04-07T16-16-40/session_summary.json
related_tags: [dsb-buffer, import-capture, timing-race, missing-csvs, replay-and-get-buffers]
summary: >
  snapshot_3 in session 2026-04-07T16-16-40 has only 1_screenshot.bmp and no
  CSVs. Root cause: GetCachedSnapshotDsbBuffer(3) returns null when called from
  ReplayAndGetBuffers because the DSB buffer is not yet cached. DSB is cached
  inside OnDataProcessed(bufferID=1) — the ImportCapture trailing event —
  but ReplayAndGetBuffers returns before that event fires, due to the DB
  polling stabilizing too quickly on an empty/small table.
last_updated: 2026-04-07
---

## Session

`d:\snapdragon\SDPCLI\test\2026-04-07T16-16-40\`

---

## Observable Symptom

`snapshot_3/` contains only `1_screenshot.bmp` — zero CSVs.
`session_summary.json` confirms capture_id=3 has only `screenshot_bmp` and
`gfxrz`/`gfxrz_stripped`; no CSV file entries.

`snapshot_2` and `snapshot_4` both have all 8 CSVs.

---

## Evidence

### sdplog "Number of apis" events

| Snap | captureId | apis:N (bufferID=2) | apis:0 (bufferID=1) | Δ (N→0) |
|------|-----------|---------------------|---------------------|---------|
| snap2 | 2 | 16:19:15 (77,493) | 16:19:18 | **+3s** |
| snap3 | 3 | 16:22:58 (412,045) | 16:23:26 | **+28s** |
| snap4 | 4 | 16:26:40 (447,844) | 16:27:01 | **+21s** |

`sdpframe_003.gfxrz` IS present on disk — gfxrz transfer succeeded.
`Number of apis: 412045` confirms full API data was processed by SDK.
`Number of apis: 0` confirms `ImportCapture(3, dbPath)` ran to completion.

No errors in sdplog. No SDPCLI application log in session directory.

Between the line `Number of apis: 412045` (line 303) and
`Number of apis: 0` (line 304), sdplog has **zero other entries** — a silent
28-second gap.

---

## Root Cause: DSB Timing Race in `ReplayAndGetBuffers`

### DSB caching mechanism

`CliClientDelegate.OnDataProcessed` (CliClientDelegate.cs L215–240) tries to
cache the DSB buffer (bufferID=3) proactively:

```csharp
// Inside OnDataProcessed, for BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA
if (bufferID != 3)
{
    var dsbBuffer = QGLPluginService.GetLocalBuffer(bufferCategory, 3, captureID);
    if (dsbBuffer != null && dsbBuffer.size > 0)
        lock (_dsbBuffers) _dsbBuffers[captureID] = dsbBuffer;  // ← only caches if available
}
```

This means DSB is attempted at every PROCESSED_API_DATA callback:
- **bufferID=2** fires (initial processing, "Number of apis: 412045"):
  `GetLocalBuffer(3)` is called. At this point, `ImportCapture` has NOT
  been called yet → DSB not generated → returns null → `_dsbBuffers[3]`
  never set.
- **bufferID=1** fires (ImportCapture trailing, "Number of apis: 0"):
  `GetLocalBuffer(3)` is called again. At this point, `ImportCapture`
  has just finished its device replay → DSB IS now available → caches
  into `_dsbBuffers[3]`.

### The race: `ReplayAndGetBuffers` returns before bufferID=1 fires

`ReplayAndGetBuffers(3)` (SnapshotCaptureMode.cs L427–530):
1. Starts at 16:22:58 (when `_dataProcessedEvent` fires on bufferID=2).
2. Calls `ImportCapture(3, dbPath)` → triggers device replay asynchronously.
3. Polls `VulkanSnapshotGraphicsPipelines` for 3 consecutive equal counts.
4. If DB already has 0 rows and stays at 0, stabilizes in just **~3 seconds**.
5. `Thread.Sleep(2000)` → total elapsed ≈ 5 seconds.
6. Calls `GetCachedSnapshotDsbBuffer(3)` at ~16:23:03 → **null** (bufferID=1
   hasn't fired yet; fires at 16:23:26, 23 seconds later).
7. Returns null.

`ExportDrawCallData(3, dsbBuffer=null, ...)` is never called → no CSVs.  
`DataExportService.ExportData(...)` IS called (produces only screenshot).

### Why snap2 and snap4 succeed

| Snap | apis:0 fires | Estimated ReplayAndGetBuffers return | DSB available before return? |
|------|-------------|--------------------------------------|------------------------------|
| snap2 | +3s | ~5-8s (DB stable + 2s sleep) | **Yes** (+3s < 5-8s → DSB cached just first) |
| snap3 | +28s | ~5s (empty table stabilizes fast) | **No** (returns at ~+5s, DSB at +28s) |
| snap4 | +21s | ~18-21s (large table, many rows, slow stabilize) | **Borderline** (close timing) |

For **snap2**: `ImportCapture` replay is fast (77K apis, small gfxrz, 3s).
bufferID=1 fires at +3s. `ReplayAndGetBuffers` DB polling + Thread.Sleep finishes at
~+5-8s. DSB is already in `_dsbBuffers[2]` when polled → succeeds.

For **snap3**: `ImportCapture` replay takes 28s (412K apis, 23s original gfxrz
transfer, complex frame with `submissionId=1065391168` anomaly in REPLAY_METRICS).
But the DB polling loop exits almost immediately (0 rows, stable) + Thread.Sleep(2000).
Total: ~5 seconds. Returns at ~16:23:03 — 23 seconds before bufferID=1 fires.

For **snap4**: 447K apis, 29s gfxrz, but DB polling takes longer (more pipelines
being inserted), likely stabilizing after ~18-19 seconds. Combined with Thread.Sleep(2000),
returns at approximately 16:26:59 — just before apis:0 fires at 16:27:01 (tight race,
may succeed by 1-2 seconds or by DSB being cached on the last DB poll).

### Additional anomaly in snap3 REPLAY_METRICS

Snap3 VulkanTraceProcessor warnings show:
```
surfaceIdx=6, submissionId=1065391168
surfaceIdx=18, submissionId=1065391168
surfaceIdx=29, submissionId=1065391168
```

`submissionId=1065391168 = 0x3F800000 = 1.0f as IEEE 754 float`. This is
suspicious — submission IDs should be small integers. Snap2/snap4 show
`submissionId=0`. This anomaly may explain why snap3's ImportCapture replay
takes longer (QGLPlugin has to handle unusual submission data), but does not
directly cause the DSB caching failure.

---

## Code Path to Fix

### `ReplayAndGetBuffers` in `SnapshotCaptureMode.cs`

Current code (after DB polling):
```csharp
Thread.Sleep(2000);

// Get cached buffers:
dsbBuffer = csd.GetCachedSnapshotDsbBuffer(captureId);
```

**Problem**: `Thread.Sleep(2000)` provides no guarantee that bufferID=1 has
fired to cache DSB.

**Fix**: After DB polling, wait for `_importCompleteEvent` (which is Set by
bufferID=1 callback) before calling `GetCachedSnapshotDsbBuffer`.

Conceptual fix:
```csharp
// Instead of Thread.Sleep(2000):
// Pass _importCompleteEvent as parameter to ReplayAndGetBuffers,
// OR expose a WaitForImportComplete(TimeSpan) helper.

// Wait for bufferID=1 to fire (DSB now guaranteed available):
bool dsbReady = _importCompleteEvent.WaitOne(TimeSpan.FromSeconds(60));
if (!dsbReady)
    _log.Warning("[ImportCapture] DSB availability timeout — proceeding with null");

dsbBuffer = csd.GetCachedSnapshotDsbBuffer(captureId);
```

This ensures DSB is always retrieved AFTER its source event (bufferID=1) fires.
The current end-of-loop `_importCompleteEvent.WaitOne(60s)` becomes redundant
(already fired) but harmless.

---

## Impact

| Scenario | Current behavior | After fix |
|---|---|---|
| Fast ImportCapture (snap2, ~3s) | Works by luck | Works reliably |
| Slow ImportCapture, slow DB stable (snap4, ~21s) | Works by borderline luck | Works reliably |
| Slow ImportCapture, fast DB stable (snap3, ~28s replay, ~0-row DB) | **Fails — no CSVs** | Works reliably |

---

## Files to Modify

| File | Change |
|---|---|
| `SDPCLI/source/Modes/SnapshotCaptureMode.cs` | Pass `_importCompleteEvent` to `ReplayAndGetBuffers`. Replace `Thread.Sleep(2000)` with `_importCompleteEvent.WaitOne(60s)`. Remove duplicate end-of-loop wait or keep as no-op guard. |
| `SDPCLI/source/Modes/SnapshotCaptureMode.cs` | Signature: `private BinaryDataPair? ReplayAndGetBuffers(uint captureId, ManualResetEvent importCompleteEvent)` |

No changes needed to `CliClientDelegate.cs` — caching logic is correct.
