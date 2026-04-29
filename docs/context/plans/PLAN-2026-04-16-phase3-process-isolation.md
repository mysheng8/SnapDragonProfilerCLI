---
type: plan
topic: Phase 3 process isolation — snapshot worker child + parallel analysis workers
status: proposed
based_on:
  - FINDING-2026-04-16-phase3-process-isolation-design.md
  - FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
  - IMPL-2026-04-16-snapshot-analysis-decoupling-p1-p2.md
related_paths:
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Server/SnapshotWorkerProxy.cs         (NEW — Plan B only)
  - SDPCLI/source/Server/SnapshotWorkerManager.cs       (NEW — Plan B only)
  - SDPCLI/source/Server/AnalysisWorkerProxy.cs         (NEW)
  - SDPCLI/source/Modes/SnapshotWorkerMode.cs           (NEW — Plan B only)
  - SDPCLI/source/Modes/AnalysisWorkerMode.cs           (NEW — Plan B only)
  - SDPCLI/source/Server/Handlers/ConnectHandler.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Server/DeviceSession.cs               (removed/stubbed — Plan B only)
  - pySdp/webui/static/app.js
related_tags:
  - phase3
  - process-isolation
  - snapshot-worker
  - analysis-worker
  - stdin-stdout
  - named-pipe
  - child-process
  - parallel-analysis
summary: |
  Two alternative IPC approaches for Phase 3. Plan A (stdin/stdout, ~150 lines new code):
  add --child flag to existing SnapshotCaptureMode and AnalysisMode; parent sends commands
  via stdin, child reports progress/results via stdout JSON lines. Reuses all existing logic,
  minimal changes. Plan B (named pipe, ~1020 lines): new SnapshotWorkerMode + SnapshotWorkerProxy
  + SnapshotWorkerManager + AnalysisWorkerMode + AnalysisWorkerProxy. Plan A is the recommended
  starting point. Plan B reserved for when named pipe bidirectional semantics are needed.
  Both plans share the same SnapshotWorkerManager single-child lock and force-connect pattern.
  Prerequisite for both: Phase 1+2 already implemented (IMPL-2026-04-16).
last_updated: 2026-04-16
---

# PLAN — Phase 3: Process Isolation

## Goal

The main `sdpcli server` process becomes completely free of native SDK dependencies.
Any native crash in a snapshot session cannot affect HTTP server availability or
in-flight analysis jobs.

---

## Architecture Overview

```
┌──────────────────────────────────────────────────────────────────────┐
│  sdpcli server (main process)                                        │
│                                                                      │
│  HttpServer  ──┐                                                     │
│  JobManager  ──┤                                                     │
│                ▼                                                     │
│  SnapshotWorkerManager      AnalysisWorkerManager                    │
│  (single child lock)        (parallel children)                      │
│      │                            │  │  │                           │
│      │ named pipe                 │  │  │  stdout JSON lines        │
│      ▼                            ▼  ▼  ▼                           │
│  ┌─────────────────┐   ┌────────────────────────────────────────┐   │
│  │ snapshot-worker │   │ analysis-worker  analysis-worker  ...  │   │
│  │  (SDK + native) │   │ (SQLite + TextureConverter, no SDK)    │   │
│  └─────────────────┘   └────────────────────────────────────────┘   │
└──────────────────────────────────────────────────────────────────────┘
```

---

## New Files

### 1. `SDPCLI/source/Modes/SnapshotWorkerMode.cs` (~300 lines)

Child-side entrypoint for the snapshot worker subprocess.

```
sdpcli snapshot-worker --pipe <pipeName>
```

Responsibilities:
- Open named pipe client: `new NamedPipeClientStream(".", pipeName, PipeDirection.InOut)`
- Retry connect for 5s (100ms intervals) so main has time to create server side
- Enter command loop: `ReadLine()` → parse JSON → dispatch
- Execute SDK operations (same logic as current ConnectJobRunner/LaunchJobRunner/CaptureJobRunner)
- Write events back to pipe as JSON lines
- Exit 0 on clean disconnect, 1 on unrecoverable error

The `ConsolePlatform` injected here uses an exit action that writes
`{"event":"error","message":"SDK requested ExitApplication"}` to the pipe and then exits,
so the main process can properly mark the job as failed before the process dies.

**Command dispatch:**

| Command | Action |
|---------|--------|
| `connect` | Run connect flow (EnsureSdkInitialized → SDPClient.Initialize → ConnectDevice) |
| `launch` | Run launch flow (AppLaunchService) |
| `capture` | Run capture flow (CaptureExecutionService → archive) |
| `disconnect` | CloseSession → SdpClient.Shutdown → write disconnected event → exit 0 |
| `ping` | Write `{"event":"pong"}` |

### 2. `SDPCLI/source/Server/SnapshotWorkerProxy.cs` (~300 lines)

Main-side manager for one snapshot worker child process.

```csharp
public class SnapshotWorkerProxy : IDisposable
{
    public bool IsAlive { get; }         // Process is running + pipe connected
    public string PipeName { get; }

    // Send a command and await the expected event response
    public Task<T> SendCommandAsync<T>(object cmd, CancellationToken ct)

    // Stream raw events into a Job (for phase/progress updates)
    public void AttachJob(Job job)

    // Force-kill the child
    public void Kill()

    public void Dispose()
}
```

Key internals:
- `NamedPipeServerStream` created first; then child process spawned
- Dedicated `Thread` reads stdout events continuously; routes to `TaskCompletionSource` waiters
  or to the attached `Job` for progress/phase updates
- `Process.Exited` event → sets `IsAlive = false`; fails any pending `TaskCompletionSource`

### 3. `SDPCLI/source/Server/SnapshotWorkerManager.cs` (~120 lines)

Single-child session lock.

```csharp
public class SnapshotWorkerManager
{
    public SnapshotWorkerProxy? Current { get; private set; }

    // Returns (proxy, null) if no existing session, or (null, "confirm_required") if locked
    public (SnapshotWorkerProxy?, string?) TrySpawn(bool force, Config config)

    public void Release()   // called on disconnect or crash cleanup
}
```

`TrySpawn(force: false)`:
- If `Current?.IsAlive == true` → return `(null, "confirm_required")`
- Else → spawn new child, set `Current`, return `(proxy, null)`

`TrySpawn(force: true)`:
- `Current?.Kill()` with 3s timeout, then `Process.Kill()`
- Spawn new child, set `Current`

### 4. `SDPCLI/source/Modes/AnalysisWorkerMode.cs` (~100 lines)

Child-side entrypoint for the analysis worker subprocess.

```
sdpcli analysis-worker --sdp <path> -s <captureId> -o <outputDir> -t <targets>
```

Responsibilities:
- Parse args (reuse existing CLI args)
- Build `AnalysisPipeline` (same as `AnalysisJobRunner.BuildPipeline`)
- Override each pipeline phase to write progress to stdout:
  ```json
  {"phase":"collect_dc","progress":12}
  ```
- Write result or error line:
  ```json
  {"result":{"sdpPath":"...","captureId":5,"captureDir":"..."}}
  {"error":"Cannot locate sdp.db"}
  ```
- Exit 0 on success, 1 on failure

### 5. `SDPCLI/source/Server/AnalysisWorkerProxy.cs` (~200 lines)

Main-side manager for one analysis worker child process.

```csharp
public static class AnalysisWorkerProxy
{
    public static async Task RunAsync(AnalysisJob job, Config config, CancellationToken ct)
}
```

Internally:
- Build args string from `job` fields
- Start `Process` with `RedirectStandardOutput = true`, `UseShellExecute = false`
- `process.BeginOutputReadLine()` + `OutputDataReceived` event → parse JSON lines → update `job.Phase`/`job.Progress`/`job.Result`/`job.Error`
- `ct.Register(() => process.Kill())` for cancellation
- Await process exit via `TaskCompletionSource` triggered by `process.Exited`
- Map exit code 0 → `job.Status = Completed`, nonzero → `job.Status = Failed`

---

## Modified Files

### `Application.cs`

Add two new branches in `Run()`:

```csharp
else if (subcommand == "snapshot-worker")
{
    string pipeName = /* from --pipe arg */ ...;
    mode = new SnapshotWorkerMode(config, testPath, pipeName);
}
else if (subcommand == "analysis-worker")
{
    mode = new AnalysisWorkerMode(config, positionalArg /*sdpPath*/,
        snapshotIdArg, targetArg, outputArg);
}
```

### `Main.cs`

Add `--pipe` arg:
```csharp
if (a == "--pipe" && i + 1 < args.Length) { pipeArg = args[++i]; continue; }
```

Pass `pipeArg` to `app.Run()`.

### `Server/Handlers/ConnectHandler.cs`

Add `Force` field to request body:
```csharp
private class ConnectRequest { public string? DeviceId { get; set; }; public bool Force { get; set; } }
```

Change body to use `SnapshotWorkerManager.TrySpawn(body.Force, config)`.
Return `409` with `{ "confirm_required": true }` when not forced.

### `Server/Jobs/AnalysisJobRunner.cs` → replaced by `AnalysisWorkerProxy`

`AnalysisHandler` submits via `AnalysisWorkerProxy.RunAsync` instead of `AnalysisJobRunner.RunAsync`.
`AnalysisJobRunner.cs` is no longer invoked from the server path (can be kept for CLI analysis mode).

### `Server/DeviceSession.cs`

`DeviceSession` is no longer the state authority in the main process for server mode.
`SnapshotWorkerManager.Current` + the proxy's event stream carry the state.

Two migration options:
- **Option A (minimal)**: Keep `DeviceSession` as a thin state mirror updated by proxy events.
  `DeviceHandler` reads from it. `ConnectHandler` / `CaptureHandler` check it.
- **Option B (clean)**: Remove `DeviceSession` from server path entirely; state flows from
  `SnapshotWorkerManager.Current.IsAlive` + a `SnapshotSessionState` value object updated by proxy.

Option A is recommended for Phase 3 to minimize risk. Option B is a follow-up refactor.

### `pySdp/webui/static/app.js`

In `doConnect()` (or wherever `POST /api/connect` is called):

```javascript
if (!res.ok && res.error?.includes('confirm_required')) {
  if (!confirm('A snapshot session is already active. Kill it and reconnect?')) return;
  // retry with force flag
  res = await apiPost(`${API}/connect`, { ...body, force: true });
}
```

---

## File Summary

| File | Status | ~Lines |
|------|--------|--------|
| `Modes/SnapshotWorkerMode.cs` | NEW | 300 |
| `Server/SnapshotWorkerProxy.cs` | NEW | 300 |
| `Server/SnapshotWorkerManager.cs` | NEW | 120 |
| `Modes/AnalysisWorkerMode.cs` | NEW | 100 |
| `Server/AnalysisWorkerProxy.cs` | NEW | 200 |
| `Application.cs` | MODIFY | +30 |
| `Main.cs` | MODIFY | +15 |
| `Server/Handlers/ConnectHandler.cs` | MODIFY | +25 |
| `Server/Jobs/AnalysisJobRunner.cs` | RETIRE (server path) | — |
| `pySdp/webui/static/app.js` | MODIFY | +20 |
| **Total new** | | ~1020 |

---

## IPC Protocol Reference

### Snapshot Worker — Named Pipe JSON Lines

**Commands (main → child):**
```json
{"cmd": "connect", "deviceId": null}
{"cmd": "launch"}
{"cmd": "capture", "outputDir": "/abs/path", "label": "frame1"}
{"cmd": "disconnect"}
{"cmd": "ping"}
```

**Events (child → main):**
```json
{"event": "phase",            "phase": "connecting",  "progress": 40}
{"event": "connected",        "deviceId": "emulator-5554"}
{"event": "launched",         "sessionId": "sess-001"}
{"event": "capture_complete", "sdpPath": "/path/file.sdp", "captureId": 5}
{"event": "error",            "message": "Device not found"}
{"event": "disconnected"}
{"event": "pong"}
```

### Analysis Worker — Stdout JSON Lines

**Child → main stdout:**
```json
{"phase": "collect_dc",      "progress": 12}
{"phase": "extract_assets",  "progress": 42}
{"phase": "label_drawcalls", "progress": 65}
{"phase": "join_metrics",    "progress": 75}
{"phase": "generate_stats",  "progress": 85}
{"phase": "report_llm",      "progress": 95}
{"phase": "dashboard",       "progress": 100}
{"result": {"sdpPath": "...", "captureId": 5, "captureDir": "..."}}
```

Or on failure:
```json
{"error": "Cannot locate sdp.db for: ..."}
```

---

## Execution Sequence

### Snapshot flow (Phase 3)

```
POST /api/connect
  → ConnectHandler.Handle()
      → workerMgr.TrySpawn(force=false)
          → if current alive → return 409 { confirm_required: true }
          → else → new SnapshotWorkerProxy(pipeName)
                 → spawn Process("SDPCLI.exe snapshot-worker --pipe <name>")
      → jobMgr.Submit(Connect, (job, ct) =>
            proxy.SendCommandAsync({"cmd":"connect"}, ct)
            → awaits {"event":"connected"} from pipe)

POST /api/session/launch
  → LaunchHandler → proxy.SendCommandAsync({"cmd":"launch"}) → awaits {"event":"launched"}

POST /api/capture
  → CaptureHandler → proxy.SendCommandAsync({"cmd":"capture"}) → awaits {"event":"capture_complete"}

POST /api/disconnect
  → DisconnectHandler → proxy.SendCommandAsync({"cmd":"disconnect"})
                     → child exits 0 → proxy.IsAlive = false
                     → workerMgr.Release()
```

### Analysis flow (Phase 3)

```
POST /api/analysis
  → AnalysisHandler → jobMgr.SubmitAnalysis(sdpPath, captureId, ...)
      → runner = (job, ct) => AnalysisWorkerProxy.RunAsync(job, config, ct)
          → spawn Process("SDPCLI.exe analysis-worker --sdp ... -s ... -o ... -t ...")
          → BeginOutputReadLine() → parse JSON lines → job.Phase/Progress/Result/Error
          → process.Exited → job.Status = Completed/Failed

# Multiple analysis jobs can be submitted simultaneously — each is an independent child
```

---

## Validation Plan

1. Build: `dotnet build SDPCLI.sln --configuration Debug` — 0 errors
2. Start server: `sdpcli server --port 5000`
3. Connect → launch → capture → verify snapshot-worker child visible in Process Explorer
4. Kill snapshot-worker child manually (Task Manager) → verify main server stays alive,
   `GET /api/device` returns `Disconnected`, new `POST /api/connect` works
5. Connect again (force=false) while child alive → verify 409 + `confirm_required: true`
6. Connect again (force=true) → verify old child killed, new child spawned
7. Submit 3 parallel `POST /api/analysis` jobs on different captures → verify all 3
   run simultaneously (3 `SDPCLI.exe` processes visible), all complete independently
8. Kill one analysis child → verify only that job fails, others continue

---

## Implementation Note

Phase 3 is the largest single change in this project (~1000 lines new code).
It should be implemented in sub-phases:

| Sub-phase | Scope | Prerequisite |
|-----------|-------|--------------|
| 3A | AnalysisWorkerMode + AnalysisWorkerProxy (analysis only) | None (analysis is already stateless) |
| 3B | SnapshotWorkerMode + SnapshotWorkerProxy + SnapshotWorkerManager | 3A not required |
| 3C | ConnectHandler force-connect + frontend confirm dialog | 3B |
| 3D | Remove DeviceSession from server main path (Option B cleanup) | 3B + 3C stable |

Sub-phase 3A can be implemented and tested independently without touching any snapshot code.

Implementation requires the Executor agent.

---

## Plan A — stdin/stdout (Recommended First Approach)

> **Status**: recorded for future implementation.  
> **Prerequisite**: Phase 1+2 already implemented.  
> **When to use**: SDK native crash has not been observed; want minimal code change to get
> process isolation benefits for analysis jobs first.

### Core Idea

Both `SnapshotCaptureMode` and `AnalysisMode` already contain all the logic.  
Add a `--child` flag that:
- Replaces `Console.ReadKey` / interactive prompts with `Console.ReadLine()` (stdin)
- Replaces `AppLogger` progress output with stdout JSON lines

No new subcommands, no new modes, no named pipes.

### Snapshot child (`sdpcli snapshot --child`)

**Stdin commands** (parent → child, one per line):
```
capture
disconnect
```

**Stdout events** (child → parent, JSON lines):
```json
{"event":"phase",            "phase":"connecting",       "progress":20}
{"event":"phase",            "phase":"launched",          "progress":50}
{"event":"ready"}
{"event":"phase",            "phase":"starting_capture",  "progress":60}
{"event":"phase",            "phase":"archiving",         "progress":90}
{"event":"capture_complete", "sdpPath":"/path/to.sdp",   "captureId":5}
{"event":"error",            "message":"Device not found"}
{"event":"disconnected"}
```

**Changes to `SnapshotCaptureMode`** (~30 lines):

```csharp
// In constructor: accept bool isChildMode
// In Run():

// Replace Console.ReadKey loop with:
if (_isChildMode)
{
    // Signal parent that we're ready
    Console.WriteLine("{\"event\":\"ready\"}");
    while (true)
    {
        string? cmd = Console.ReadLine();
        if (cmd == null || cmd == "disconnect") break;
        if (cmd != "capture") continue;
        // ... do capture ...
        Console.WriteLine($"{{\"event\":\"capture_complete\",\"sdpPath\":\"{sdpPath}\",\"captureId\":{captureId}}}");
    }
    // disconnect path → write {"event":"disconnected"} → return
}
else
{
    // existing Console.ReadKey loop unchanged
}

// Phase/progress writes: add alongside existing AppLogger calls:
if (_isChildMode)
    Console.WriteLine($"{{\"event\":\"phase\",\"phase\":\"{phase}\",\"progress\":{pct}}}");
```

**Changes to `Application.cs`** (~5 lines):
```csharp
bool childMode = Array.Exists(args, a => a == "--child");
// pass childMode into SnapshotCaptureMode constructor
```

### Analysis child (`sdpcli analysis <path> -s <id> -o <dir> -t <targets> --child`)

No stdin needed (analysis is fire-and-forget). Only stdout JSON lines.

**Stdout events**:
```json
{"phase":"collect_dc",      "progress":12}
{"phase":"extract_assets",  "progress":42}
{"phase":"label_drawcalls", "progress":65}
{"phase":"join_metrics",    "progress":75}
{"phase":"generate_stats",  "progress":85}
{"phase":"report_llm",      "progress":95}
{"phase":"dashboard",       "progress":100}
{"result":{"sdpPath":"...","captureId":5,"captureDir":"..."}}
```

**Changes to `AnalysisMode`** (~20 lines): intercept pipeline phase completion,
write JSON line to stdout if `--child` flag set.

### Main-side proxy (shared by both)

**`SnapshotWorkerProxy`** (stdin/stdout variant, ~150 lines):
```csharp
public class SnapshotWorkerProxy : IDisposable
{
    // Spawn: Process.Start with RedirectStandardInput=true, RedirectStandardOutput=true
    // ReadThread: BeginOutputReadLine() → parse JSON → TaskCompletionSource / job updates
    // SendCommand: process.StandardInput.WriteLine(cmd)
    public Task WaitForReadyAsync(CancellationToken ct)
    public Task<CaptureCompleteResult> SendCaptureAsync(CancellationToken ct)
    public Task SendDisconnectAsync()
    public bool IsAlive { get; }
    public void Kill()
}
```

**`AnalysisWorkerProxy`** (stdout only, ~100 lines):  
Same as Plan B but simpler — no stdin, just `BeginOutputReadLine()` + `process.Exited`.

### File Summary (Plan A)

| File | Change | ~Lines |
|------|--------|--------|
| `Modes/SnapshotCaptureMode.cs` | MODIFY — add `--child` branch | +30 |
| `Modes/AnalysisMode.cs` | MODIFY — add `--child` stdout progress | +20 |
| `Application.cs` | MODIFY — parse `--child`, pass to modes | +10 |
| `Server/SnapshotWorkerProxy.cs` | NEW (stdin/stdout variant) | ~150 |
| `Server/AnalysisWorkerProxy.cs` | NEW | ~100 |
| `Server/SnapshotWorkerManager.cs` | NEW (shared with Plan B) | ~120 |
| `Server/Handlers/ConnectHandler.cs` | MODIFY — force-connect | +25 |
| `pySdp/webui/static/app.js` | MODIFY — confirm dialog | +20 |
| **Total new** | | **~475** |

### Plan A vs Plan B decision criteria

| Condition | Use Plan A | Use Plan B |
|-----------|-----------|-----------|
| SDK native crashes are rare or unobserved | ✓ | |
| Want minimal code change | ✓ | |
| Single capture per session is acceptable | ✓ | |
| Multi-capture session required with low reconnect overhead | | ✓ |
| Stdout/stdin conflicts with AppLogger output at high verbosity | | ✓ |
| Need ping/pong heartbeat for liveness check | | ✓ |

> **Note on stdout conflict**: `SnapshotCaptureMode` currently writes via `AppLogger` (file log)
> and `Console.WriteLine` (human text). In `--child` mode, parent reads stdout as JSON lines.
> All `Console.WriteLine` calls that write human text must be suppressed or redirected to stderr
> when `--child` is active. AppLogger writes to file only — no conflict. The ~30 line change
> must audit all `Console.Write*` calls in the capture path.
