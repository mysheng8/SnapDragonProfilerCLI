---
type: plan
topic: step-by-step decoupling of snapshot and analysis in server mode
status: proposed
based_on:
  - FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
  - FINDING-2026-04-15-snapshot-analysis-mode-switch.md
  - IMPL-2026-04-14-http-server-mode.md
related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - pySdp/webui/static/app.js
related_tags:
  - decoupling
  - server-mode
  - snapshot
  - analysis
  - sdk-lifecycle
  - process-stability
  - process-isolation
summary: |
  Three-phase decoupling plan. Phase 1 (high priority, low risk): suppress
  ConsolePlatform.ExitApplication() in server mode — replace Environment.Exit(0) with
  a logged no-op or CancellationToken signal. Phase 2 (medium): SDK session scoping —
  stop the update thread between captures and restart it only when needed. Phase 3
  (optional, long-term): process isolation — run snapshot in a child process so SDK
  crashes cannot affect the HTTP server or analysis jobs.
last_updated: 2026-04-16
---

# PLAN — Step-by-Step Snapshot/Analysis Decoupling

## Context

The existing finding (FINDING-2026-04-16-snapshot-analysis-coupling-topology.md) identifies
four coupling layers. Analysis is already structurally decoupled from snapshot at the code
and state-machine level. The remaining coupling is:

1. **Critical**: `ConsolePlatform.ExitApplication()` → `Environment.Exit(0)` — SDK can kill
   the entire server process at any time via a single native callback.
2. **High**: `SDPClient` update thread stays active between captures and during analysis.
3. **Low**: Static `SdkInitialized` singleton on reconnect.
4. **Low**: Frontend tab-switch doesn't call `syncDevice()`.

The plan proceeds in three phases ordered by risk/effort ratio.

---

## Phase 1 — Suppress ExitApplication in Server Mode (High ROI, Low Risk)

### Goal

Prevent a native SDK `IPlatform.ExitApplication()` call from terminating the server process.

### Root cause

```csharp
// ConsolePlatform.cs line 24-27
public void ExitApplication()
{
    Environment.Exit(0);   // ← kills the entire SDPCLI process
}
```

`SdpApp.Init(new ConsolePlatform())` registers this platform globally. Any native SDK
event that decides the app should exit calls this method, which is currently a hard kill.

### Solution: ExitApplication action injection

Replace `Environment.Exit(0)` with a configurable `Action` passed at construction time.

#### Change 1 — `ConsolePlatform.cs`

```csharp
public class ConsolePlatform : IPlatform
{
    private readonly Action _onExitApplication;

    // Default ctor: use Environment.Exit (preserves CLI-mode behaviour)
    public ConsolePlatform() : this(() => Environment.Exit(0)) { }

    // Server-mode ctor: caller provides exit action
    public ConsolePlatform(Action onExitApplication)
    {
        _onExitApplication = onExitApplication;
    }

    public void ExitApplication()
    {
        _onExitApplication();
    }
    // ... rest unchanged ...
}
```

#### Change 2 — `DeviceSession.EnsureSdkInitialized()`

Pass a server-mode exit action to `ConsolePlatform`:

```csharp
internal bool EnsureSdkInitialized(Config config, Action? onSdkExit = null)
{
    if (SdkInitialized) return true;
    try
    {
        // ...
        Action exitAction = onSdkExit ?? (() => Environment.Exit(0));
        var platform = new ConsolePlatform(exitAction);
        if (!Sdp.SdpApp.Init(platform)) { ... }
        // ...
    }
}
```

#### Change 3 — `ConnectJobRunner.RunAsync()` (Phase 1 init)

```csharp
// In Phase 1 (initializing_sdk):
// Pass a CancellationTokenSource that will notify ServerMode instead of killing the process
Action onSdkExit = () =>
{
    _log.Error("SDK requested ExitApplication — suppressed in server mode");
    // Signal via the CTS that was passed from ServerMode
    serverShutdownCts?.Cancel();
};
if (!session.EnsureSdkInitialized(config, onSdkExit))
    throw new InvalidOperationException("SDK initialization failed");
```

This requires passing a `CancellationTokenSource` from `ServerMode.Run()` into
`ConnectJobRunner`. ServerMode can then detect SDK exit requests and shut down gracefully
rather than hard-dying.

#### Change 4 — `ServerMode.Run()` (optional enhancement)

When `serverShutdownCts` is cancelled by the SDK exit signal:
- Log the event clearly: "SDK requested ExitApplication — server entering graceful shutdown"
- Stop accepting new jobs
- Let running jobs finish (with timeout)
- Disconnect device

### Validation

After this change, trigger a deliberate SDK exit condition (e.g., force-kill the adb daemon
while connected) and verify:
1. SDPCLI process stays alive
2. Log shows "SDK requested ExitApplication — suppressed in server mode"
3. HTTP server remains reachable
4. `GET /api/device` returns `Disconnected` (after DeviceSession detects the disconnect)
5. A new `POST /api/analysis` can be submitted and completes successfully

### Estimated scope

- Files changed: `ConsolePlatform.cs`, `Server/DeviceSession.cs`,
  `Server/Jobs/ConnectJobRunner.cs`, `Modes/ServerMode.cs`
- Risk: Low. ExitApplication is only called by the SDK on fatal conditions.
  In CLI modes (snapshot, analysis, interactive), the default `Environment.Exit(0)` behaviour
  is preserved via the default constructor.

---

## Phase 2 — SDK Session Scoping (Medium ROI, Medium Risk)

### Goal

Reduce the window during which the SDK update thread is active. Currently it runs from
`POST /api/connect` until `POST /api/disconnect`. This means the SDK is active during
analysis, during idle waits between captures, and while the user has left the device
connected indefinitely.

### Core idea: pause the update thread between captures

After `CaptureJobRunner` completes archiving and before the device becomes idle in
`SessionActive` state, optionally pause (stop) the update thread. Restart it only when
the next capture or launch job begins.

#### Option A: Explicit update-thread pause/resume

Add `SDPClient.PauseUpdateThread()` / `ResumeUpdateThread()`:

```csharp
public void PauseUpdateThread()
{
    isRunning = false;
    updateThread?.Join(2000);
    // Don't null updateThread — it can be restarted
}

public void ResumeUpdateThread()
{
    if (updateThread == null || !updateThread.IsAlive)
        StartUpdateThread();
}
```

`CaptureJobRunner.finally` block would call `session.SdpClient?.PauseUpdateThread()`.
`CaptureHandler` would call `session.SdpClient?.ResumeUpdateThread()` before transitioning
state to `Capturing`.

**Pro**: Minimal window of native event activity.
**Con**: The SDK may internally require continuous Update() calls to maintain connection
health — pausing may cause the device to disconnect itself. Needs empirical testing.

#### Option B: Keep update thread running but improve disconnect detection

Rather than pausing the thread, add a DeviceConnectionState monitor inside
`DeviceSession` that polls the device state periodically. When the device disconnects,
proactively call `session.Disconnect()` before the SDK fires a fatal event.

```csharp
// DeviceSession — add a background health monitor
private Thread? _healthMonitor;

internal void StartHealthMonitor()
{
    _healthMonitor = new Thread(() =>
    {
        while (_status != DeviceStatus.Disconnected)
        {
            Thread.Sleep(5000);
            try
            {
                if (Device == null) break;
                var state = Device.GetDeviceState();
                if (state == DeviceConnectionState.Unknown ||
                    state == DeviceConnectionState.InstallFailed)
                {
                    _log.Warning($"Device health check: {state} — initiating disconnect");
                    Disconnect();
                    break;
                }
            }
            catch { break; }
        }
    }) { IsBackground = true, Name = "DeviceHealthMonitor" };
    _healthMonitor.Start();
}
```

`ConnectJobRunner` calls `session.StartHealthMonitor()` after connect succeeds.

**Pro**: Doesn't change update thread mechanics. Works with SDK connection assumptions.
**Con**: Doesn't fully eliminate the ExitApplication window (device can die between polls).
         Phase 1 is still required.

### Recommended approach: Phase 1 first, then Option B

Phase 1 makes the process resilient to SDK exit requests. Phase 2/Option B adds proactive
disconnect detection. Together they eliminate the common scenario where a USB disconnect
fires a fatal SDK event during analysis.

### Estimated scope

- Option B files changed: `Server/DeviceSession.cs`, `Server/Jobs/ConnectJobRunner.cs`
- Risk: Medium. Health monitor adds a background thread; proactive disconnect may affect
  users who legitimately reconnect the device mid-session.

---

## Phase 3 — Process Isolation (Optional, Long-Term)

### Goal

Full decoupling: snapshot SDK runs in a dedicated child process. The HTTP server and
analysis pipeline run in the main process. Any native crash in the snapshot child cannot
affect HTTP server availability or in-flight analysis jobs.

### Architecture

```
┌─────────────────────────────────────────────────────────────┐
│  SDPCLI server process (main)                               │
│  ┌───────────────────┐  ┌────────────────────────────────┐  │
│  │  HttpServer       │  │  AnalysisJobRunner (offline)   │  │
│  │  JobManager       │  │  (pure C#/SQLite, no SDK)      │  │
│  └────────┬──────────┘  └────────────────────────────────┘  │
│           │ IPC (named pipe / stdin/stdout JSON)             │
│           ▼                                                  │
│  ┌────────────────────┐                                      │
│  │  SnapshotWorker    │ ← child process                      │
│  │  (SDPClient, SDK)  │   spawned on demand, killed on       │
│  │  ConsolePlatform   │   disconnect or crash                │
│  └────────────────────┘                                      │
└─────────────────────────────────────────────────────────────┘
```

**IPC protocol**: The child process reads JSON commands from stdin and writes JSON events
to stdout. Parent process wraps this in a `SnapshotWorkerProxy` class that mirrors the
current `DeviceSession`/`CaptureJobRunner` API surface.

**Child process crash handling**: Parent detects `Process.Exited`, marks session as
`Disconnected`, logs the event, emits an error result for the active job. Analysis jobs
are unaffected.

### Why this is long-term

- Significant implementation effort (~500-700 lines of new IPC infrastructure)
- Requires the Vulkan SDK DLLs and all native dependencies to be loadable in the child
- Named pipe or stdin/stdout JSON must handle binary event data (capture events carry buffers)
- The `BinaryDataPair` DSB buffer (from `CliClientDelegate.GetCachedSnapshotDsbBuffer`)
  currently lives in process memory — must be serialized/passed via IPC or shared memory
- .NET 4.7.2 has no `System.IO.Pipes.PipeStream` for async named pipes in the target
  framework; would need `System.IO.Pipes` with Thread-based read loop

**Triggers for implementing Phase 3**:
- Repeated production failures where Phase 1+2 are insufficient
- Requirement to run concurrent snapshot + analysis jobs
- Requirement to support multiple simultaneous connected devices

---

## Execution Order

| Phase | Priority | Risk | Estimated Files | Dependency |
|-------|----------|------|-----------------|------------|
| Phase 1: Suppress ExitApplication | High | Low | 4 | None |
| Phase 2B: Health monitor | Medium | Medium | 2 | After Phase 1 |
| Phase 3: Process isolation | Low | High | ~10 new | After Phase 1+2 |
| Frontend: syncDevice on tab switch | Quick win | Negligible | 1 (app.js) | None |

### Quick win (no dependency): Frontend

In `switchTab()` (app.js line 901), add `syncDevice()` call when switching to the
snapshot tab:

```javascript
function switchTab(id) {
  // ... existing code ...
  if (id === 'snapshot') syncDevice();   // ← add this
}
```

This eliminates the 3-second stale state window that causes a 503 click on a dead server.

---

## Validation Plan for Phase 1

1. Build: `dotnet build SDPCLI.sln --configuration Debug` — must succeed with 0 errors
2. Start server: `sdpcli server --port 5000`
3. Connect device → run capture → verify capture completes
4. Simulate SDK exit: `adb kill-server` while connected (forces device disconnect)
5. Verify SDPCLI process stays alive (port 5000 still responds)
6. Verify `GET /api/device` returns `{ status: "Disconnected" }` or an error state
7. Start a new analysis job on a previously captured .sdp → verify it completes
8. Reconnect → run second capture → verify success

---

## Implementation Note

**Phase 1 is the only phase that must be implemented before any further testing.**
Phase 2 and Phase 3 are risk-reducers, not correctness fixes.

Implementation requires the Executor agent.
