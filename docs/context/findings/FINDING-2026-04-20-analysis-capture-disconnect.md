---
type: finding
topic: analysis → capture transition: disconnect, state desync, and health monitor false-positive
status: investigated
related_paths:
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/Jobs/LaunchJobRunner.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/Handlers/CaptureHandler.cs
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/static/app.js
related_tags:
  - server-mode
  - state-machine
  - capture
  - analysis
  - health-monitor
  - disconnect
  - frontend
  - tab-switch
  - target-process
summary: |
  Four root-cause candidates for the "analysis → capture causes disconnect" symptom.
  Primary (HIGH): health monitor fires a proactive Disconnect() while the second capture
  is in progress because Device.GetDeviceState() returns Unknown/InstallFailed for a
  transient reason during the post-analysis SDK restart or after the prior capture's
  SDK teardown flushes cached device state. Secondary (HIGH): TargetProcessRemoved event
  from LaunchJobRunner fires during the second capture's waiting phases, transitioning
  state from Capturing → Connected and leaving the frontend with a stale SessionActive
  state. Tertiary (MEDIUM): capture events (CaptureCompleteEvent, DataProcessedEvent,
  ImportCompleteEvent) are shared ManualResetEvents that are Reset() at the start of each
  capture — if the previous capture's trailing OnDataProcessed(bufferID=1) fires late and
  races with the Reset(), the second capture sees a spuriously pre-set event, causing a
  false early timeout that leaves state = SessionActive after finally {}, not Capturing.
  Frontend desync (LOW): switchTab('snapshot') does not call syncDevice(); after a long
  analysis the UI still shows the old device state for up to 3 seconds.
last_updated: 2026-04-20
---

# Finding: Analysis → Capture Transition: Disconnect, State Desync, and Health Monitor False-Positive

## Problem Statement

After completing an analysis job, clicking "capture" (snapshot) causes a disconnect visible
in the frontend. The UI does not reflect the state change correctly — the snapshot tab may
still show "Session Active" badge, or the capture button may become re-enabled then
immediately disabled.

---

## Context: What Phase 1 + Phase 2 Fixed (and What They Did Not)

IMPL-2026-04-16-snapshot-analysis-decoupling-p1-p2.md confirmed:

- Phase 1 eliminated `Environment.Exit(0)` — the SDPCLI process no longer terminates on SDK
  fatal events. This removes the 503 "SDPCLI Offline" symptom.
- Phase 2 added a health monitor (`HealthMonitorProc`) in `DeviceSession.cs` that polls
  `Device.GetDeviceState()` every 5s and calls `Disconnect()` on Unknown/InstallFailed.

The bug reported — "capture after analysis causes disconnect" — is a **new symptom** consistent
with the health monitor being the active disconnect trigger, not the old ExitApplication bomb.

---

## State Machine Flow: Analysis → Capture Transition

```
[After LaunchJobRunner completes]
  DeviceStatus = SessionActive
  CliClientDelegate.TargetProcessRemoved handler is REGISTERED
  DeviceHealthMonitor thread is RUNNING (polls every 5s)
  SDK update thread is RUNNING (~60Hz)

POST /api/analysis
  AnalysisHandler: NO state transition — DeviceStatus remains SessionActive
  AnalysisJobRunner: pure offline, zero SDK calls, zero DeviceSession mutation
  DeviceStatus = SessionActive  (throughout entire analysis)

[Analysis completes — DeviceStatus = SessionActive]

POST /api/capture
  CaptureHandler (CaptureHandler.cs:35):
    TryTransition(SessionActive → Capturing)   ← succeeds
    DeviceStatus = Capturing
  CaptureJobRunner starts (phases 1–7)

[Disconnect occurs DURING phases 1–7]
  DeviceStatus transitions back to Disconnected or Connected unexpectedly
  CaptureJobRunner.finally:
    TryTransition(Capturing → SessionActive)   ← FAILS (status is already Disconnected or Connected)
  DeviceStatus = Disconnected (or Connected)

[Frontend]
  Next syncDevice() poll: status = Disconnected
  UI shows disconnected state, capture button disappears
```

---

## Root Cause Candidates (Ranked by Likelihood)

### Root Cause 1: Health Monitor False-Positive Disconnect During Second Capture (HIGH)

**File**: `SDPCLI/source/Server/DeviceSession.cs`
**Lines**: `HealthMonitorProc()` — lines 173–199

**Mechanism**:

The health monitor calls `Device.GetDeviceState()` every 5 seconds and calls `Disconnect()`
if the result is `Unknown` or `InstallFailed`:

```csharp
// DeviceSession.cs:182-189
var state = Device.GetDeviceState();
if (state == DeviceConnectionState.Unknown ||
    state == DeviceConnectionState.InstallFailed)
{
    _log.Warning($"Health monitor: device state={state} — initiating proactive disconnect");
    Disconnect();
    break;
}
```

**When it misfires**: The native SDK may transiently return `Unknown` for `GetDeviceState()`
in the following situations:

1. During the `CaptureJobRunner` phase `importing` (line 104–119): `QGLPluginService.ImportCapture()`
   triggers device-side replay via the SDK. During replay initiation, the device connection
   state may briefly show as `Unknown` at the native layer while the SDK reallocates
   connection resources internally.

2. Immediately after analysis completes and the second capture starts: the analysis ran for
   a long time (minutes) with the SDK update thread active but no capture traffic. The native
   SDK connection state may drift to a "disconnected-but-reconnectable" state that manifests
   as `Unknown` before the second capture's `StartCapture()` refreshes the connection.

**Critical observation**: `Disconnect()` forcibly sets `_status = DeviceStatus.Disconnected`
(line 141, unconditional assignment — NOT a `TryTransition`). This means `Disconnect()` can
fire while `DeviceStatus = Capturing`, setting status to `Disconnected` regardless of the
current state. When `CaptureJobRunner.finally` runs afterward, `TryTransition(Capturing →
SessionActive)` silently fails (status is already Disconnected), leaving the session
permanently disconnected.

**Evidence chain**:
- `Disconnect()` at line 141: `_status = DeviceStatus.Disconnected;` — no guard on current state
- `HealthMonitorProc` loop condition: `_status != DeviceStatus.Disconnected` only checked
  at loop iteration start, not inside the `if` branch that calls `Disconnect()`
- `CaptureJobRunner.finally` at line 176: `TryTransition(Capturing, SessionActive)` is a
  guarded transition — fails silently if status is not `Capturing`

**This is the most likely cause of the observed "disconnect" after analysis.**

---

### Root Cause 2: TargetProcessRemoved Event Fires During Second Capture (HIGH)

**File**: `SDPCLI/source/Server/Jobs/LaunchJobRunner.cs`
**Lines**: 106–123 (TargetProcessRemoved handler registration)

**File**: `SDPCLI/source/CliClientDelegate.cs`
**Lines**: 407–431 (OnProcessRemoved)

**Mechanism**:

`LaunchJobRunner.RunAsync()` registers a one-shot `TargetProcessRemoved` handler on
`CliClientDelegate` after the launch succeeds:

```csharp
// LaunchJobRunner.cs:109-122
handler = pid =>
{
    csd.TargetProcessRemoved -= handler;   // self-unregister
    if (session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Connected))
    {
        _log.Info($"Target process PID={pid} removed — session auto-closed");
        session.CurrentSession    = null;
        session.TargetPackageName = null;
        ...
    }
};
csd.TargetProcessRemoved += handler;
```

This handler fires on the SDK callback thread when the target Android app process is removed
by the SDK's realtime capture background process monitor.

**When it fires during second capture**:

After the first capture completes, `CaptureJobRunner` calls `session.CurrentCapture = null`
(line 134) but does NOT tear down the realtime background capture or re-launch the app.
The app process remains alive. However, during analysis (which may take several minutes),
if the app process is killed or garbage-collected by Android OS (out-of-memory, user kills
it, etc.), the SDK's realtime capture fires `OnProcessRemoved` via the update thread.

This triggers the `TargetProcessRemoved` handler, which calls:
```csharp
session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Connected)
```

If this fires during the second capture (when status = Capturing), the `TryTransition`
silently fails (expected = SessionActive, actual = Capturing). The handler does nothing.
But if the process removal fires BEFORE the user clicks capture again (during analysis,
when status = SessionActive), the transition succeeds and status becomes `Connected`.

Now when the user clicks capture:
- `CaptureHandler.TryTransition(SessionActive → Capturing)` FAILS (status = Connected)
- Returns HTTP 409 with `"Cannot capture from state 'Connected'"`
- The frontend's `doCapture()` shows an error; `setBtn('btn-capture', state.device === 'SessionActive')`
  still returns false if state.device hasn't updated yet
- The next `syncDevice()` poll will show `Connected`, causing `refreshSteps()` to disable
  btn-capture and enable btn-launch — which looks like a disconnect/reset to the user

**Evidence chain**:
- `LaunchJobRunner.cs:113`: `TryTransition(SessionActive → Connected)` — fires any time after launch
- The handler is registered once and never unregistered on a subsequent capture
- `CliClientDelegate.OnProcessRemoved` line 421: fires on SDK update thread, calls `TargetProcessRemoved?.Invoke(pid)`

---

### Root Cause 3: Shared ManualResetEvent Race Between Captures (MEDIUM)

**File**: `SDPCLI/source/Server/Jobs/CaptureJobRunner.cs`
**Lines**: 41–44 (event Reset), 234 (ImportCompleteEvent.WaitOne)

**File**: `SDPCLI/source/Server/DeviceSession.cs`
**Lines**: 65–67 (shared event declarations)

**Mechanism**:

The three `ManualResetEvent` instances (`CaptureCompleteEvent`, `DataProcessedEvent`,
`ImportCompleteEvent`) are declared as `readonly` fields on `DeviceSession` — they are
reused across all captures in a session. `CaptureJobRunner` resets them at the start
of each capture:

```csharp
// CaptureJobRunner.cs:41-44
session.CaptureCompleteEvent.Reset();
session.DataProcessedEvent.Reset();
session.ImportCompleteEvent.Reset();
```

Race condition:

1. First capture completes. `ImportCompleteEvent` fires (line 234 WaitOne returns).
2. The SDK fires a **trailing** `OnDataProcessed(bufferID=1)` for the first capture
   on the SDK update thread, slightly after `CaptureJobRunner` returns.
3. User immediately clicks capture again. `CaptureJobRunner` starts and resets the events.
4. The trailing `OnDataProcessed` from the first capture sets `ImportCompleteEvent` again
   (because `_expectedCaptureIdForSignal` still equals the first capture's ID).
5. The second capture's `ImportCompleteEvent.WaitOne(60s)` returns immediately as if
   `ImportCapture` device-side replay completed instantly.
6. `dsbBuffer` may be null or contain stale first-capture data.
7. The capture job completes without correct data, but does NOT disconnect — this is a
   silent data corruption bug, not a disconnect bug. However, if `dsbBuffer` is null and
   the export throws an exception in `ExportDrawCallData`, the `catch` at line 288 suppresses
   the error and the job completes with partial data.

**This is not the disconnect cause**, but is a latent data integrity bug for rapid
recapture scenarios.

---

### Root Cause 4: Frontend State Desync on Tab Switch (LOW)

**File**: `pySdp/webui/static/app.js`
**Lines**: 901–923 (`switchTab()`)

**Mechanism**:

`switchTab()` does not call `syncDevice()` when switching to the snapshot tab:

```javascript
// app.js:901-923
function switchTab(id) {
  // ... visual tab switch ...
  if (id === 'analysis') { /* scanSdpFiles */ }
  if (id === 'logs')     { /* render logs */ }
  // ← NO syncDevice() for id === 'snapshot'
}
```

Device state is refreshed only by the background `setInterval(syncDevice, 3000)` timer.

After a long analysis run, the frontend may have an accurate "SessionActive" reading from
the last poll. But if the health monitor or TargetProcessRemoved fired during analysis and
changed server-side status to Disconnected/Connected, the user switching to the snapshot
tab sees "Session Active" for up to 3 seconds, clicks "Capture", gets a 409 response, and
the error message says `"Cannot capture from state 'Connected'"` or `"Cannot capture from
state 'Disconnected'"`. The UI shows an error but does not auto-refresh state — the user
sees the capture button re-enabled (because `state.device` still holds the stale value)
and can click again before `syncDevice()` corrects the state.

**Evidence chain**:
- `switchTab` at app.js:901: no `syncDevice()` call for `id === 'snapshot'`
- `startDevicePoll` at app.js:141: `setInterval(syncDevice, 3000)` — 3s lag possible
- `doCapture` at app.js:368: after 409 error, `setBtn('btn-capture', state.device === 'SessionActive')`
  re-enables the button using stale `state.device`

---

## Complete State Machine Trace: Most Likely Scenario (RC1 + RC4)

```
T+0    POST /api/analysis → 202  (DeviceStatus = SessionActive, HealthMonitor running)
T+0    Analysis runs for 2–10 minutes  (HealthMonitor polls Device.GetDeviceState() every 5s)
T+N    During analysis: Device.GetDeviceState() returns Unknown (transient SDK state)
T+N    HealthMonitorProc calls Disconnect()
         → StopHealthMonitor()           (monitor self-stops)
         → SdpClient.Shutdown()          (SDK session torn down)
         → _status = DeviceStatus.Disconnected   (FORCED, no TryTransition guard)

T+N+Δ  Analysis job still running (AnalysisJobRunner has no DeviceSession ref, continues normally)
T+N+Δ  Analysis job completes → JobStatus.Completed

[User is on Analysis tab — frontend shows analysis done]
[User switches to Snapshot tab]
T+N+Δ  switchTab('snapshot')  — NO syncDevice() called
         UI still shows old badge: "● Session Active"
         btn-capture is enabled (state.device === 'SessionActive' from stale cache)

T+N+Δ  User clicks "Capture"
T+N+Δ  doCapture() → POST /api/capture
T+N+Δ  CaptureHandler.TryTransition(SessionActive → Capturing) FAILS
         (actual status = Disconnected)
T+N+Δ  CaptureHandler returns 409: "Cannot capture from state 'Disconnected'"

T+N+Δ  doCapture error path:
         setMsg('capture', 'error', res.error)
         setBtn('btn-capture', state.device === 'SessionActive')  ← stale: re-enables button!

T+N+3s  syncDevice() fires (3s interval)
         GET /api/device → { status: 'Disconnected' }
         state.device = 'Disconnected'
         refreshSteps() → btn-capture disabled, btn-connect enabled
         UI now shows "Disconnected" — user sees disconnect happened
```

---

## Secondary Scenario (RC2): TargetProcessRemoved During Analysis

```
T+0    Analysis running  (DeviceStatus = SessionActive)
T+M    Android OS kills target app (OOM or user swipe)
T+M    SDK realtime capture fires OnProcessRemoved(targetPid)
T+M    TargetProcessRemoved handler fires:
         TryTransition(SessionActive → Connected) SUCCEEDS
         session.CurrentSession = null
         DeviceStatus = Connected

[Analysis completes normally]
[User switches to Snapshot tab — sees "Session Active" from stale state]
[User clicks Capture]
T+M+Δ  CaptureHandler.TryTransition(SessionActive → Capturing) FAILS (status = Connected)
T+M+Δ  Returns 409: "Cannot capture from state 'Connected'"
[User must click Launch again — but sees "disconnect" in the UI because badge shows Connected
 not SessionActive]
```

---

## SDK Update Thread During Analysis (Confirmed Non-Issue)

`SDPClient.UpdateThreadProc()` (confirmed in FINDING-2026-04-16) has a `try/catch` INSIDE
the `while(isRunning)` loop. .NET exceptions from `client.Update()` are caught and logged —
the update thread does not die on a .NET exception. This remains a non-issue.

The update thread is running during analysis, but AnalysisJobRunner makes zero SDK calls,
so the only way the update thread causes problems is via native SDK callbacks (ExitApplication,
which is now suppressed, or device state changes, which feed the health monitor).

---

## Frontend Desync: Technical Detail

Three independent desync paths exist in `app.js`:

**Path 1 — Tab switch lag** (app.js:901):
`switchTab('snapshot')` does not call `syncDevice()`. Maximum staleness = 3 seconds.

**Path 2 — Capture error re-enables stale button** (app.js:368):
```javascript
setBtn('btn-capture', state.device === 'SessionActive');
```
This re-enables the button on error using the *locally cached* `state.device`, which has
not been refreshed since the last poll. If the server-side status changed during analysis,
the button is re-enabled incorrectly.

**Path 3 — goAnalyze triggers tab switch without syncDevice** (app.js:535-544):
```javascript
function goAnalyze(sdpPath, captureId) {
  ...
  switchTab('analysis');   // no syncDevice here either
}
```

---

## Impact Assessment

| Root Cause | User Symptom | Severity |
|------------|-------------|----------|
| RC1: Health monitor false-positive | Full disconnect after analysis; capture 409 from Disconnected | HIGH |
| RC2: TargetProcessRemoved during analysis | Soft disconnect (SessionActive → Connected); capture 409 from Connected | HIGH |
| RC3: ManualResetEvent race | Silent data corruption on rapid recapture (no disconnect) | MEDIUM |
| RC4: Frontend tab-switch stale state | UI shows wrong badge for ≤3s; capture button re-enabled on stale state | LOW |

---

## Recommended Fixes (for Executor Agent)

### Fix 1 — Health Monitor: Guard Against Capturing State

**File**: `SDPCLI/source/Server/DeviceSession.cs`, `HealthMonitorProc()`

Before calling `Disconnect()`, check that the current state is one that makes a disconnect
safe (not `Capturing`). Add a state check inside the `HealthMonitorProc` `if` block:

```csharp
// Proposed change in HealthMonitorProc:
if (state == DeviceConnectionState.Unknown ||
    state == DeviceConnectionState.InstallFailed)
{
    // Do not disconnect while a capture is in flight
    if (_status == DeviceStatus.Capturing)
    {
        _log.Warning($"Health monitor: device state={state} but Capturing in progress — deferring disconnect");
        continue;  // or break, depending on desired behaviour
    }
    _log.Warning($"Health monitor: device state={state} — initiating proactive disconnect");
    Disconnect();
    break;
}
```

Additionally, `Disconnect()` should not override `Capturing` state without first transitioning
through `SessionActive`. Consider using `TryTransition` in `Disconnect()` instead of the
forced assignment on line 141.

### Fix 2 — TargetProcessRemoved: Register Handler Per-Capture, Not Just Per-Launch

**File**: `SDPCLI/source/Server/Jobs/LaunchJobRunner.cs`, lines 106–123

The `TargetProcessRemoved` handler is registered once at launch and fires if the process
dies. But the user may do multiple captures without re-launching. The handler's
`TryTransition(SessionActive → Connected)` is correct for the case where the process dies
between captures, but needs to also be safe during a capture.

The handler uses `TryTransition` (guarded), so it will not override `Capturing` — but this
means if the process dies DURING capture, the state remains `Capturing` and the finally
block resets to `SessionActive` without the app actually being alive. Consider: after
`CaptureJobRunner.finally` runs, poll or check whether the process is still alive before
re-enabling capture.

### Fix 3 — ManualResetEvent: Use Per-Capture Event Instances

**File**: `SDPCLI/source/Server/DeviceSession.cs` and `CaptureJobRunner.cs`

Replace the three shared `ManualResetEvent` fields with per-capture instances allocated
in `CaptureJobRunner.RunAsync()` and disposed after use. This eliminates the trailing-event
race entirely.

### Fix 4 — Frontend: Call syncDevice() on Snapshot Tab Switch

**File**: `pySdp/webui/static/app.js`, `switchTab()` function, line 907

Add `syncDevice()` when switching to the `'snapshot'` tab:

```javascript
if (id === 'snapshot') {
  syncDevice();   // refresh device state immediately on tab activation
}
```

Also fix `doCapture()` error path (line 368) to not re-enable the button using stale state:
```javascript
// Instead of:
setBtn('btn-capture', state.device === 'SessionActive');
// Use: (wait for next syncDevice to re-enable)
setBtn('btn-capture', false);
```

---

## Related Context

- FINDING-2026-04-16-snapshot-analysis-coupling-topology.md — original coupling map
- FINDING-2026-04-15-snapshot-analysis-mode-switch.md — prior ExitApplication root cause (now resolved)
- IMPL-2026-04-16-snapshot-analysis-decoupling-p1-p2.md — Phase 1+2 implementation record
- PLAN-2026-04-16-snapshot-analysis-decoupling.md — decoupling plan (Phase 1+2 done, Phase 3 pending)
