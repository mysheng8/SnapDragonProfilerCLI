---
type: finding
topic: snapshot → analysis → snapshot mode switch causing "cannot find SDPCLI" error
status: investigated
related_paths:
  - SDPCLI/source/Modes/ServerMode.cs
  - SDPCLI/source/Server/HttpServer.cs
  - SDPCLI/source/Server/Jobs/JobManager.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - pySdp/webui/routes/proxy.py
  - pySdp/webui/static/app.js
related_tags:
  - server-mode
  - device-session
  - analysis
  - snapshot
  - capture
  - mode-switch
  - environment-exit
  - state-machine
summary: |
  Four independent root-cause candidates identified for the snapshot → analysis → snapshot failure.
  Primary candidate: ConsolePlatform.ExitApplication() calls Environment.Exit(0) — if the SDK
  event system ever calls IPlatform.ExitApplication() during analysis teardown or an unhandled SDK
  event, the entire SDPCLI process terminates, producing a 503 "connection refused" on the next
  request. Secondary candidates: (1) AnalysisJobRunner leaves DeviceSession state unchanged,
  but the CaptureHandler gate checks TryTransition(SessionActive→Capturing) — if the device
  was in SessionActive before analysis, it remains SessionActive after, which is correct; however
  if something set the state to Connected or Disconnected during analysis it will produce 409.
  (2) SDPClient.Shutdown() warns it "may hang or terminate process" — if Disconnect() is called
  mid-analysis (user clicks Disconnect while analysis runs) the process may hang or exit.
  (3) The frontend /api/device poll returns 503 when SDPCLI is gone; the badge shows error but
  the UI may not block subsequent Capture tab actions that then also get 503.
last_updated: 2026-04-15
---

# Finding: Snapshot → Analysis → Snapshot Mode Switch Failure

## Problem Statement

When running SDPCLI in server mode via `webui.bat` (`sdpcli server --port 5000`):
1. Snapshot tab works — connect, launch, capture succeed
2. Analysis tab runs analysis on the captured .sdp
3. Switching back to Snapshot tab and attempting capture produces "cannot find SDPCLI" or
   an unreachable error

Investigation goal: determine whether SDPCLI process exits after analysis, or whether the
failure is a state-machine error, a proxy 503, or a 409 conflict.

## Evidence

### Q1: Does AnalysisJobRunner or any analysis path call Environment.Exit or kill the process?

**Finding: YES — indirect path via ConsolePlatform.ExitApplication().**

`D:\snapdragon\SDPCLI\source\ConsolePlatform.cs` line 25:
```csharp
public void ExitApplication()
{
    Environment.Exit(0);
}
```

`ConsolePlatform` implements `Sdp.IPlatform` and is passed to `Sdp.SdpApp.Init(platform)` in
`DeviceSession.EnsureSdkInitialized()` (DeviceSession.cs line 78). The SDK framework holds a
reference to this platform instance. If the native SDK ever calls `IPlatform.ExitApplication()`
— for example on a fatal SDK internal error, or during a timeout/reset that triggers an SDK-level
shutdown — `Environment.Exit(0)` will be called, terminating the entire .NET process immediately.

**The analysis pipeline itself** (`AnalysisPipeline.RunAnalysis`) does not directly call
`Environment.Exit`. It catches all exceptions, logs them, and rethrows. The `AnalysisJobRunner`
wraps this in a try/catch that sets `job.Status = Failed`. No explicit process exit in the
analysis code path.

**The SDPClient.Shutdown()** method has a documented warning:
```
// 3. 关闭 Client (WARNING: This may hang or terminate process)
AppLogger.Debug("SDPClient", "(This may take a while or cause process exit)");
client.Shutdown();
```

`Disconnect()` in `DeviceSession.cs` calls `SdpClient.Shutdown()` and `SdpClient.Dispose()`.
If `Disconnect` is triggered — for example by the user switching tabs and clicking Disconnect,
or by a server shutdown signal — the native `client.Shutdown()` call may hang or crash the process.

### Q2: Does the analysis path reinitialize or tear down any shared SDK/DLL state?

**Finding: NO — AnalysisJobRunner is completely isolated from SDK state.**

`AnalysisJobRunner.RunAsync()` does not touch `DeviceSession` at all. It creates its own
`AnalysisPipeline` from scratch, opens the `.sdp` / `sdp.db` from disk, and reads/writes files.
It uses only `SdpFileService`, `DrawCallAnalysisService`, `RawJsonGenerationService`,
`DrawCallLabelService`, `MetricsQueryService`, and `LlmApiWrapper` — all pure .NET/SQLite.

No SDK DLL calls, no QGLPlugin, no SDPClient interaction. The analysis job is fully offline.

There is no teardown of shared native DLL state triggered by analysis.

### Q3: Is there any device session teardown triggered by the analysis job?

**Finding: NO — AnalysisJobRunner makes zero calls to DeviceSession.**

`AnalysisJobRunner` receives only `(AnalysisJob job, Config config, CancellationToken ct)`.
It has no reference to `DeviceSession`. It does not call `session.Disconnect()` or modify
`session.Status` in any way.

However, `DeviceSession.Disconnect()` IS called in `ServerMode.Run()` shutdown path:
```csharp
if (session.Status != DeviceStatus.Disconnected)
    session.Disconnect();
```
This only runs when the server itself is stopping (Ctrl+C or 'q'), not when analysis completes.

### Q4: Does the server process stay alive after analysis completes?

**Finding: YES, in the normal code path.**

`AnalysisJobRunner.RunAsync()` sets `job.Status = Completed` and returns. The task completes.
The `HttpServer` accept loop keeps running. `ServerMode.Run()` keeps polling in its sleep loop.
No path in the analysis completion logic exits the process.

The only way the server process can exit after analysis is:
1. **ConsolePlatform.ExitApplication()** is called by the SDK framework during some unhandled
   event (e.g. SDK fatal error triggered by the background update thread in `SDPClient`)
2. **SDPClient background update thread** throws an unhandled exception. The update thread is
   started inside `SDPClient.Initialize()` as a background thread. If it throws an unhandled
   exception, .NET 4.7.2 default behavior terminates the process.
3. **An unhandled exception on the HttpServer thread pool thread** — but this is handled by
   `HandleRequest(ctx)` which has a global try/catch that returns HTTP 500.

### Q5: What does "cannot find SDPCLI" mean?

**Finding: It is a 503 from the Python proxy — meaning SDPCLI process is gone (connection refused).**

`pySdp/webui/routes/proxy.py` lines 69-75:
```python
except requests.ConnectionError as exc:
    if not silent:
        log.warning("SDPCLI Server unreachable", ...)
    return JSONResponse(
        {"ok": False, "error": "Cannot connect to SDPCLI Server — is it running?"},
        status_code=503,
    )
```

The message "Cannot connect to SDPCLI Server — is it running?" appears when `requests.get/post`
raises `ConnectionError`, which happens when the TCP connection is refused — i.e. when the
SDPCLI process is no longer listening on port 5000. This is a strong indicator the SDPCLI
process has exited, not a 409 state conflict.

If it were a 409 (state conflict from the capture gate check), the error would be:
`"Cannot capture from state '...'"`  (from `CaptureHandler.cs` line 37).

### Additional State Machine Analysis

The `CaptureHandler` gate:
```csharp
if (!_session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Capturing))
{
    WriteError(ctx, $"Cannot capture from state '{_session.Status}'", 409);
    return;
}
```

After `CaptureJobRunner` finishes (whether success or failure), its `finally` block always
resets state:
```csharp
finally
{
    session.TryTransition(DeviceStatus.Capturing, DeviceStatus.SessionActive);
}
```

So after a successful capture, the state goes back to `SessionActive`. Analysis does not
change this state. Therefore, if the SDPCLI process is alive, the state machine should
correctly allow a second capture after analysis completes.

**The 409 path is NOT the cause** of the reported symptom. The "cannot find SDPCLI" error is
a 503, indicating process death.

## Analysis

The failure scenario most consistent with all evidence:

1. User completes a snapshot (state: `SessionActive`)
2. User runs analysis (state: still `SessionActive` — analysis is independent)
3. During or after analysis, something kills the SDPCLI process:
   - **Most likely**: The SDK's background update thread in `SDPClient` raises an unhandled
     exception. The `SDPClient.StartUpdateThread()` (which runs a loop calling `client.Update()`)
     runs as a background thread. If `client.Update()` triggers a native callback that calls
     `IPlatform.ExitApplication()` → `Environment.Exit(0)`, the process dies silently.
   - **Also possible**: The analysis job runs for a long time (LLM calls, large captures).
     During this time, the SDK's background update thread encounters a device timeout or
     connection reset and calls `ExitApplication()`.
   - **Less likely but possible**: An `OutOfMemoryException` during large-capture analysis
     (big texture extraction in parallel) terminates the process via unhandled exception.

4. When the user returns to the Snapshot tab, `GET /api/device` (which polls every 3 seconds)
   or the next `POST /api/capture` hits a refused connection → proxy returns 503 →
   frontend shows "Cannot connect to SDPCLI Server — is it running?"

## Impact

1. **Process reliability**: The `ConsolePlatform.ExitApplication()` → `Environment.Exit(0)`
   is a latent process termination bomb. Any SDK-internal trigger can kill the long-running
   server process silently.

2. **Background update thread**: The SDPClient update thread is not guarded with a global
   exception handler. An unhandled exception there terminates the process in .NET 4.7.2.

3. **User experience**: The frontend device polling (`syncDevice()` every 3s, `silent=true`)
   suppresses the warning log for connection errors, so the badge silently changes from
   `● SDPCLI: OK` to `● SDPCLI: Error` without alerting the user that the process died.

4. **No restart mechanism**: Neither `webui.bat` nor `proxy.py` has a watchdog/restart
   mechanism. Once SDPCLI dies, the user must manually restart it.

## Secondary Bug: Frontend Tab Switch Does Not Re-Verify Server State

When the user switches from the Analysis tab back to the Snapshot tab, `switchTab()` in
`app.js` does NOT trigger a `syncDevice()` call. The device poll runs on a 3-second timer.
If the server died 0.1 seconds after the last poll, the UI may show stale "Connected" state
for up to 3 seconds, during which the user can click "Capture" → 503.

## Root Cause Summary

| # | Cause | Probability | Severity |
|---|-------|-------------|----------|
| 1 | `ConsolePlatform.ExitApplication()` → `Environment.Exit(0)` called by SDK framework | High | Critical |
| 2 | `SDPClient` background update thread unhandled exception terminates process | Medium | Critical |
| 3 | `client.Shutdown()` hangs/crashes process during Disconnect (if user disconnects during analysis) | Medium | High |
| 4 | OOM during large-capture parallel extraction terminates process | Low | Critical |
| 5 | Frontend shows stale state after process death (up to 3s window) | Always present | Low |

## Related Context

- Related findings: FINDING-2026-04-11-analysis-mode-sdp-path-bug.md
- Related plans: PLAN-2026-04-11-http-server-mode.md
- Related implementations: IMPL-2026-04-14-http-server-mode.md
