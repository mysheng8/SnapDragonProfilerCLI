---
type: finding
topic: server mode snapshot/analysis coupling topology — layer-by-layer structural analysis
status: investigated
related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/Handlers/CaptureHandler.cs
  - SDPCLI/source/Server/Handlers/AnalysisHandler.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/static/app.js
related_tags:
  - server-mode
  - coupling
  - decoupling
  - snapshot
  - analysis
  - sdk-lifecycle
  - state-machine
  - process-isolation
summary: |
  Full layer-by-layer coupling topology between snapshot and analysis in server mode.
  Four coupling layers identified with distinct severities. One important correction
  to the prior finding: UpdateThreadProc has try/catch INSIDE the while loop, so
  .NET exceptions from client.Update() do NOT kill the process (the loop continues).
  The only real process killer is ConsolePlatform.ExitApplication() → Environment.Exit(0).
  Analysis is already fully decoupled at the code/data level — the only remaining
  coupling is the shared OS process. Decoupling work targets the process stability
  boundary first, then SDK session scoping, then optional process isolation.
last_updated: 2026-04-16
---

# Finding: Snapshot/Analysis Coupling Topology in Server Mode

## Correction to FINDING-2026-04-15-snapshot-analysis-mode-switch.md

The prior finding stated:

> "The SDPClient background update thread has no unhandled-exception guard (process death on
>  native callback throw)."

**This is incorrect.** `SDPClient.UpdateThreadProc()` (line 955+) has a `try/catch(Exception)` block
INSIDE the `while(isRunning)` loop:

```csharp
private void UpdateThreadProc()
{
    while (isRunning)
    {
        try
        {
            if (client != null)
            {
                client.Update();
                Interlocked.Increment(ref updateCallCount);
            }
            Thread.Sleep(16);
        }
        catch (Exception ex)
        {
            AppLogger.Warn("SDPClient", $"UpdateThread exception: {ex.Message}");
        }
    }
}
```

A .NET exception from `client.Update()` is caught, logged, and the loop continues.
The update thread is resilient to .NET exceptions. This eliminates Root Cause #2 from
the prior finding as a meaningful risk.

**The remaining single root cause of process death is `ConsolePlatform.ExitApplication()`.**

---

## Layer-by-Layer Coupling Map

### Layer 1: Process Coupling (CRITICAL — actual current failure cause)

**What is coupled**: Snapshot SDK lifecycle and Analysis share one OS process.

**Where it is**:
- `ConsolePlatform.ExitApplication()` → `Environment.Exit(0)` (ConsolePlatform.cs line 25)
- `Sdp.SdpApp.Init(platform)` registers the `ConsolePlatform` instance as the global SDK platform
- If native SDK ever calls `IPlatform.ExitApplication()` — e.g., on a device disconnect event,
  a fatal SDK internal error, or a timeout/reset — `Environment.Exit(0)` terminates the entire
  SDPCLI process immediately, bypassing all .NET exception handling.

**When it fires**:
- The `SDPClient` background update thread calls `client.Update()` at ~60Hz
- `client.Update()` dispatches native SDK events through the managed delegate layer
- Any SDK-internal fatal event that triggers `IPlatform.ExitApplication()` fires while the
  update thread is running — which is at all times when the device is connected

**Impact on analysis**: An analysis job running at the time of the SDK exit call is abruptly
terminated mid-pipeline. The user gets a 503 on the next request.

**Severity**: Critical.

---

### Layer 2: SDK Session Lifetime (HIGH — structural risk)

**What is coupled**: The `SDPClient` update thread runs continuously from `ConnectJobRunner`
all the way until `DeviceSession.Disconnect()`. There is no pause between captures.

**Lifecycle**:
```
POST /api/connect   → ConnectJobRunner.RunAsync()
                        → sdpClient = new SDPClient()
                        → sdpClient.Initialize(...)     ← starts update thread
                        → session.SdpClient = sdpClient
                        → state: Connected → SessionActive (after launch)

[analysis runs here — SDK update thread still active]
[second capture runs — SDK update thread still active]
[user disconnects the physical device — SDK may fire fatal event any time]

POST /api/disconnect → DeviceSession.Disconnect()
                        → sdpClient.Shutdown()          ← stops update thread
                        → sdpClient.Dispose()
```

**Risk**: Between two captures (while analysis is running), the Android device may:
- Get USB-disconnected
- Have the ADB bridge reset
- Time out the network connection from the SDK side

Any of these can trigger a native SDK callback → `ExitApplication()` → process death
while the analysis job is still running.

**Severity**: High. The longer the gap between captures, the higher the risk.

---

### Layer 3: State Machine (CLEAN — no coupling)

**What is NOT coupled**: `AnalysisJobRunner` makes zero calls to `DeviceSession`.
`AnalysisHandler` has no reference to `DeviceSession`. The state machine:

```
Disconnected → Connecting → Connected → Launching → SessionActive → Capturing
```

is only touched by: `ConnectJobRunner`, `LaunchJobRunner`, `CaptureJobRunner`,
`ConnectHandler`, `LaunchHandler`, `CaptureHandler`, and `DisconnectHandler`.

`AnalysisHandler` and `AnalysisJobRunner` are completely absent from state transitions.

**After a capture completes**: `CaptureJobRunner.finally` always resets
`TryTransition(Capturing → SessionActive)`. So state is correctly `SessionActive` when
the analysis runs, and the state is still `SessionActive` when the user wants to capture again.

**No action needed here.** The state machine design is sound.

---

### Layer 4: Static SDK Initialization (LOW — edge case)

**What is coupled**: `DeviceSession.SdkInitialized` is a `static bool` — it is process-scoped.
`Sdp.SdpApp.Init(platform)` is called at most once per process.

**Implication**: If `Disconnect()` is called (which calls `SdpClient.Shutdown()`), then
`Reconnect` is attempted, `EnsureSdkInitialized()` returns immediately with `true` and does NOT
re-run `SdpApp.Init()`. This is **by design** (calling Init twice may crash).

**Risk**: If `SdpClient.Shutdown()` / `client.Shutdown()` leaves the native SDK in a partially
torn-down state (known risk — see Shutdown() warning comment), a subsequent `ConnectJobRunner`
would get a new `SDPClient` instance but with stale/corrupted native singletons from the first
connection's teardown. This could cause silent failures on reconnect without a clear error.

**Severity**: Low. Only manifests after a full Disconnect + Reconnect cycle. Visible as
"connect job completes but capture never fires events."

---

### Layer 5: Frontend Stale State (UI — cosmetic risk)

**What is coupled**: `switchTab()` in `app.js` does NOT trigger `syncDevice()` immediately.
Device status is polled every 3 seconds via `setInterval(syncDevice, 3000)`. If the server
process dies while the user is on the Analysis tab, the Snapshot tab badge may show stale
`● Connected` state for up to 3 seconds when the user switches back.

**Current `switchTab` code** (app.js line 901-924):
```javascript
function switchTab(id) {
  // ... visual tab switch ...
  if (id === 'analysis') { /* scanSdpFiles */ }
  if (id === 'logs')     { /* render logs */ }
  // ← no syncDevice() call here for 'snapshot' tab
}
```

**Impact**: User may click "Capture" within the 3-second stale window → gets a 503 from the
proxy without a clear "server is down" warning.

**Severity**: Low. Does not affect correctness, only UX clarity.

---

## Summary Table

| Layer | Location | Coupling Type | Severity | Fixable in-process? |
|-------|----------|---------------|----------|---------------------|
| 1 | `ConsolePlatform.ExitApplication()` | Process death bomb | Critical | Yes — suppress Exit in server mode |
| 2 | `SDPClient` update thread lifetime | SDK runs during analysis | High | Yes — SDK session scoping |
| 3 | `DeviceSession` state machine | (no coupling — already clean) | None | N/A |
| 4 | `SdkInitialized` static bool | SDK singleton on reconnect | Low | Partially — process restart |
| 5 | Frontend `switchTab` | Stale UI badge | Low | Yes — call syncDevice on tab switch |

---

## What Is Already Clean (Do Not Change)

- `AnalysisJobRunner` — zero SDK calls, zero DeviceSession mutation
- `AnalysisPipeline` — pure C#/SQLite, fully offline
- `AnalysisHandler` — no device state dependency
- State machine transitions — correct design, analysis doesn't participate
- `UpdateThreadProc` — already has try/catch inside the while loop (resilient to .NET exceptions)

---

## Related Context

- Prior finding: FINDING-2026-04-15-snapshot-analysis-mode-switch.md (root cause diagnosis)
- Server API audit: FINDING-2026-04-15-server-api-state-audit.md
- Implementation record: IMPL-2026-04-14-http-server-mode.md
