---
type: finding
topic: stuck-sdpservice-transfer-queue
status: investigated
related_paths:
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/test/2026-04-07T16-16-40/sdplog.txt
related_tags: [storage, transfer, stuck, snaps-5-6-7, detection, replay-metrics]
summary: >
  In session 2026-04-07T16-16-40, snaps 5/6/7 produced completely empty
  directories. Root cause: device-side gfxrz transfer queue stuck after
  cumulative storage exhaustion from snaps 2-4. The C# layer has no early
  warning — current detection only fires after WaitForDataProcessed(180s)
  timeout. REPLAY_METRICS_DATA (observable via OnDataProcessed) is the
  earliest detectable signal; a targeted 90s timeout on that event enables
  faster, more specific stuck diagnosis.
last_updated: 2026-04-07
---

## Session Under Investigation

`d:\snapdragon\SDPCLI\test\2026-04-07T16-16-40\`

Snap directories present: 1, 2, 3, 4, 5, 6, 7, 8 (8 total)
Snap directories with content: 1, 2, 3, 4 only. Snaps 5, 6, 7 are empty.

---

## Observable Symptom (snap5)

`snapshot_5/` directory exists but is completely empty (no CSVs, no gfxrz, nothing).
Same for snapshot_6/ and snapshot_7/.

---

## Evidence

### Normal transfer timing (snap1 baseline)

From sdplog:

```
16:18:50  Vulkan Snapshot deactivated            (capture done)
16:19:05  BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA   +15s
16:19:10  BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA    +20s  ← OnDataProcessed fires
16:19:11  BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA    +21s
16:19:14  Snapshot file transfer: 3129ms            +24s
16:19:15  Number of apis: 77493 (bufferID=2)        +25s  ← _dataProcessedEvent fires
16:19:15  Number of apis: 0    (bufferID=1)         +25s  ← _importCompleteEvent fires
```

Total time from deactivate to `_dataProcessedEvent`: ~25 seconds.

### Snap4 (last successful capture)

```
16:24:13  Vulkan Snapshot deactivated
16:24:28  SHADER_DATA              +15s
...
16:26:11  Snapshot file transfer: 29041ms   ← 29s transfer (large gfxrz)
16:26:...  Number of apis: ...
16:27:01  Number of apis: 0 (importComplete)
```

### Snap5 (first stuck capture)

```
16:27:10  Vulkan Snapshot deactivated
16:28:21  SHADER_DATA              +71s  ← already 4.7× slower than normal
(nothing after this)
```

`REPLAY_METRICS_DATA` never appeared in sdplog.
No `OnDataProcessed` callback was triggered for snap5 after SHADER_DATA.

### Snap6, snap7

Snaps 6 and 7 activated/deactivated normally (16:38:31 and 16:42:42), but received
**zero** buffer events. Not even SHADER_DATA appeared in sdplog for these.

### Connection health (confirmed NOT disconnected)

Thread 11172 is present throughout the entire 6-minute gap (16:27 → 16:38).
No "disconnect", "connection lost", or socket error lines appear in sdplog.
The connection remained alive; the sdpservice transfer thread was silently stuck.

---

## Root Cause

### Cumulative gfxrz file accumulation on device

| Capture | Snapshot file transfer time |
|---|---|
| snap1 | 3,129 ms |
| snap2 | ~7,000 ms (estimated from data) |
| snap3 | ~23,000 ms |
| snap4 | 29,041 ms |

Each `*.gfxrz` file is transferred from device but the prior files are NOT deleted.
After 4 captures, device `/data/local/tmp/SnapdragonProfiler/` accumulated multiple
large gfxrz files. By snap5's SHADER_DATA arrival (71s instead of 15s), the sdpservice
adb pull queue was already severely congested.

After SHADER_DATA, sdpservice should initiate the gfxrz file transfer. With device
storage saturated (or the adb transfer queue backlogged), the transfer block never
completed → REPLAY_METRICS_DATA was never generated → `OnDataProcessed` never fired.

### Why snaps 6 and 7 received nothing at all

By the time snaps 6/7 were captured, the sdpservice transfer thread for snap5's
gfxrz was pinned in a permanent wait state. New captures could activate/deactivate
metrics (those are lightweight RPC calls), but their data enqueued behind snap5's
stuck transfer and never drained.

---

## The Two Observable Signals from C#

`SHADER_DATA` and `GFXRECONSTRUCT_DATA` are **not observable** from C# via
`OnDataProcessed`. They are internal logs emitted by QGLPluginProcessor in C++.

`REPLAY_METRICS_DATA` **is observable** from C# via `OnDataProcessed`:
```csharp
// CliClientDelegate.cs line 248
if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
{
    lock (_metricsBuffers) _metricsBuffers[captureID] = metricsBuffer;
}
```

The full observable signal sequence from C# is therefore:
1. `REPLAY_METRICS_DATA` → `OnDataProcessed` fires (normal: ~20s after deactivate)
2. `PROCESSED_API_DATA bufferID=2` → `_dataProcessedEvent.Set()` (normal: ~25s)
3. `PROCESSED_API_DATA bufferID=1` → `_importCompleteEvent.Set()` (normal: ~25s)

---

## Current Detection Gap

`WaitForDataProcessed()` in `SnapshotCaptureMode.cs` (line 413):
```csharp
bool ready = _dataProcessedEvent.WaitOne(TimeSpan.FromSeconds(180));
if (!ready)
    _log.Warning("[WaitForData] API data not received within 180 seconds — replay may produce empty results.");
```

Problems:
1. **Too slow**: 180-second wait before any warning is shown.
2. **Too vague**: Cannot distinguish "stuck before REPLAY_METRICS" vs "REPLAY_METRICS 
   arrived but gfxrz transfer failed afterward".
3. **No actionable guidance**: Does not tell user to clean device storage.

---

## Proposed Detection Point

`REPLAY_METRICS_DATA` via `OnDataProcessed` is the earliest signal that device-side
data has begun arriving. In stuck cases it never fires.

Add a `_replayMetricsEvent: ManualResetEvent` to `CliClientDelegate`, set when
`bufferCategory == BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA && captureID == _expectedCaptureIdForSignal`.

Use it as a 90-second early-exit gate in `WaitForDataProcessed()`:

```
wait _replayMetricsEvent (90s)
  ├── fires    → REPLAY_METRICS arrived → continue normal WaitForDataProcessed
  └── timeout  → TRANSFER STUCK
                 → warn user with specific message + adb cleanup command
                 → skip full 180s wait (already known to be empty)
```

This reduces detection latency from 180s → 90s and enables actionable messaging.

---

## Actionable Fix for Users

When stuck is confirmed (REPLAY_METRICS not received within 90s):

```
adb shell rm /data/local/tmp/SnapdragonProfiler/*.gfxrz
```

Then restart the capture session. Consider capturing no more than 3-4 snaps per
session before clearing, depending on gfxrz file sizes.
