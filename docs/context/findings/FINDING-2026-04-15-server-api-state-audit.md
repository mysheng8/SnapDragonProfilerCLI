---
type: finding
topic: HTTP server API endpoint inventory and DeviceStatus state machine audit
status: investigated
related_paths:
  - SDPCLI/source/Server/HttpServer.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/DeviceSessionInfo.cs
  - SDPCLI/source/Server/ApiResponse.cs
  - SDPCLI/source/Server/Handlers/StatusHandler.cs
  - SDPCLI/source/Server/Handlers/DeviceHandler.cs
  - SDPCLI/source/Server/Handlers/ConnectHandler.cs
  - SDPCLI/source/Server/Handlers/DisconnectHandler.cs
  - SDPCLI/source/Server/Handlers/LaunchHandler.cs
  - SDPCLI/source/Server/Handlers/CaptureHandler.cs
  - SDPCLI/source/Server/Handlers/AnalysisHandler.cs
  - SDPCLI/source/Server/Handlers/JobsHandler.cs
  - SDPCLI/source/Server/Jobs/Job.cs
  - SDPCLI/source/Server/Jobs/JobManager.cs
  - SDPCLI/source/Server/Jobs/JobStatus.cs
  - SDPCLI/source/Server/Jobs/JobType.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Server/Jobs/LaunchJobRunner.cs
  - SDPCLI/source/Server/Jobs/CaptureJobRunner.cs
  - SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs
  - SDPCLI/source/Modes/ServerMode.cs
  - SDPCLI/source/ConsolePlatform.cs
  - pySdp/webui/routes/proxy.py
  - pySdp/webui/static/app.js
related_tags:
  - server-mode
  - http-api
  - device-session
  - state-machine
  - webui
  - endpoints
summary: |
  Complete inventory of all 9 registered HTTP routes (StatusHandler through JobsHandler),
  exact DeviceStatus state machine with transition guards, exact JSON shape returned by
  GET /api/device, and confirmation that the server does NOT exit after archive. Three
  state-visibility gaps identified that block correct WebUI gating: no active-job field
  on /api/device, no lastError field, and the Disconnected state cannot distinguish
  "clean idle" from "failed mid-connect".
last_updated: 2026-04-15
---

# Finding: HTTP Server API Endpoint Inventory and State Machine Audit

## Problem Statement

Audit every HTTP endpoint registered in SDPCLI server mode, map the complete DeviceStatus
state machine including all transitions and guards, document the exact JSON shape of
GET /api/device, and determine whether the server process exits after archive completes —
in order to design a WebUI that accurately reflects live server state and gates buttons.

## Evidence

### 1. All Registered HTTP Endpoints

Route dispatch is in `HandlerRouter.Dispatch()` in `HttpServer.cs`. Matching is exact-path
or prefix (`/api/jobs` and `/api/jobs/...`), checked in order.

| Method         | Path                     | Handler Class         | Description                                                                                  | Success Response                                 | Error Codes         |
|----------------|--------------------------|-----------------------|----------------------------------------------------------------------------------------------|--------------------------------------------------|---------------------|
| GET            | /api/status              | StatusHandler         | Health check — always 200. Confirms server is alive.                                         | `{ ok, data: { status:"ok", version:"1.0" } }`  | 405                 |
| GET            | /api/device              | DeviceHandler         | Returns current DeviceStatus + connectedDeviceId + session info snapshot.                    | `{ ok, data: { status, connectedDevice, session } }` | 405             |
| POST           | /api/connect             | ConnectHandler        | Starts async connect job. Gates on Disconnected. Transitions to Connecting.                  | 202 `{ ok, data: { jobId } }`                   | 405, 409            |
| POST           | /api/disconnect          | DisconnectHandler     | Synchronous force-disconnect. Always succeeds. Resets state to Disconnected.                 | `{ ok, data: { disconnected:true } }`           | 405                 |
| POST           | /api/session/launch      | LaunchHandler         | Starts async launch job. Gates on Connected. Transitions to Launching.                       | 202 `{ ok, data: { jobId } }`                   | 405, 409            |
| POST           | /api/capture             | CaptureHandler        | Starts async capture job. Gates on SessionActive. Transitions to Capturing.                  | 202 `{ ok, data: { jobId } }`                   | 405, 409            |
| POST           | /api/analysis            | AnalysisHandler       | Starts async analysis job. No device-state gate. Duplicate guard by (sdpPath, snapshotId).  | 202 `{ ok, data: { jobId } }`                   | 400, 405, 409       |
| GET            | /api/jobs                | JobsHandler           | Lists all jobs (running + terminal), descending by createdAt.                               | `{ ok, data: [ ...job summaries ] }`            | 405                 |
| GET            | /api/jobs/{id}           | JobsHandler           | Gets single job by ID.                                                                       | `{ ok, data: { job summary } }`                 | 404                 |
| POST           | /api/jobs/{id}/cancel    | JobsHandler           | Cancels a running or pending job via CancellationTokenSource.Cancel().                       | `{ ok, data: { cancelled, jobId, status } }`    | 404                 |
| DELETE         | /api/jobs/{id}           | JobsHandler           | Removes a terminal job record from memory. Rejects if job is still running.                  | `{ ok, data: { deleted, jobId } }`              | 404, 409            |
| OPTIONS        | (any path)               | HandlerRouter         | CORS preflight — 204, Allow header.                                                          | 204                                             | —                   |
| (any method)   | (unmatched)              | HandlerRouter         | 404 "No route for METHOD /path"                                                              | `{ ok:false, error }`                           | 404                 |

**Important note on /api/status vs /api/device**: `/api/status` is the "ping" / health-check
endpoint (`{ status:"ok", version:"1.0" }`). It carries NO device state. `/api/device` carries
device state but is NOT a health check — if SDPCLI is unreachable, the proxy returns 503 before
this route is ever hit.

#### Is there a dedicated /health or /ping endpoint?

There is no `/health` or `/ping` route. `/api/status` fills this role but it is not at a
conventional health-check path. The name collision risk: callers expecting `/api/status` to
return device status will be confused — it returns server liveness, not device state.

### 2. DeviceStatus Enum

Defined in `DeviceSessionInfo.cs`:

```csharp
public enum DeviceStatus
{
    Disconnected,   // 0 — initial / post-disconnect
    Connecting,     // 1 — connect job running
    Connected,      // 2 — device found, SDK initialized
    Launching,      // 3 — launch job running
    SessionActive,  // 4 — app running, ready to capture
    Capturing       // 5 — capture job running
}
```

### 3. State Machine — All Transitions

#### Guarded transitions (TryTransition — atomic CAS, fails if wrong state)

```
TryTransition(expected, next) → bool
  lock(_transitionLock) { if (_status != expected) return false; _status = next; return true; }
```

| Trigger                         | Expected         | Next             | On Failure (transition returns false)        |
|---------------------------------|------------------|------------------|----------------------------------------------|
| ConnectHandler.Handle()         | Disconnected     | Connecting       | Returns HTTP 409 "Cannot connect from state" |
| LaunchHandler.Handle()          | Connected        | Launching        | Returns HTTP 409 "Cannot launch from state"  |
| CaptureHandler.Handle()         | SessionActive    | Capturing        | Returns HTTP 409 "Cannot capture from state" |
| ConnectJobRunner (success)      | Connecting       | Connected        | No-op (already changed by error/cancel path) |
| ConnectJobRunner (cancel/fail)  | Connecting       | Disconnected     | No-op                                        |
| LaunchJobRunner (success)       | Launching        | SessionActive    | No-op                                        |
| LaunchJobRunner (cancel/fail)   | Launching        | Connected        | No-op                                        |
| CaptureJobRunner finally block  | Capturing        | SessionActive    | No-op — always runs, even on failure         |

#### Forced transitions (direct assignment — bypass lock)

| Trigger                             | Result state |
|-------------------------------------|--------------|
| DeviceSession.Disconnect() called   | Disconnected (direct `_status = DeviceStatus.Disconnected`) |
| DisconnectHandler (calls Disconnect) | Disconnected |
| ServerMode.Run() finally block      | Disconnected |

#### AnalysisJobRunner — NO state transitions

`AnalysisJobRunner` has zero references to `DeviceSession`. It does not read or write
`_status`. The device state is entirely unaffected by analysis job start, progress, or
completion.

#### State Machine Diagram (text)

```
                    ┌─────────────────────────────────────────────────┐
                    │                  Disconnect()                   │
                    │            (any state → Disconnected)           │
                    └─────────────────────────────────────────────────┘
                                          │
                    ┌─────────────────────▼──────┐
                    │       Disconnected          │◄──── initial state / post-disconnect
                    └─────────────────────────────┘
                         │
    POST /api/connect     │  TryTransition(Disconnected→Connecting)
    (gates: must be       │  fails if not Disconnected → 409
    Disconnected)         │
                    ┌─────▼───────────┐
                    │   Connecting    │  connect job running
                    └────────┬───────┘
                             │ ConnectJobRunner success
                             │  TryTransition(Connecting→Connected)
                    ┌────────▼───────┐   ConnectJobRunner fail/cancel
                    │   Connected    │◄── TryTransition(Connecting→Disconnected)
                    └────────┬───────┘
                             │
    POST /api/session/launch  │  TryTransition(Connected→Launching)
    (gates: must be           │  fails if not Connected → 409
    Connected)                │
                    ┌─────────▼──────┐
                    │   Launching    │  launch job running
                    └────────┬───────┘
                             │ LaunchJobRunner success
                             │  TryTransition(Launching→SessionActive)
                    ┌────────▼──────────┐   LaunchJobRunner fail/cancel
                    │  SessionActive    │◄── TryTransition(Launching→Connected)
                    └────────┬──────────┘
                             │
    POST /api/capture         │  TryTransition(SessionActive→Capturing)
    (gates: must be           │  fails if not SessionActive → 409
    SessionActive)            │
                    ┌─────────▼──────┐
                    │   Capturing    │  capture job running
                    └────────┬───────┘
                             │ CaptureJobRunner finally (always)
                             │  TryTransition(Capturing→SessionActive)
                    ┌────────▼──────────┐
                    │  SessionActive    │◄── ready for next capture
                    └───────────────────┘

  POST /api/analysis runs concurrently with ANY of the above states.
  It does not gate on DeviceStatus and does not modify it.
```

### 4. Exact JSON Shape of GET /api/device

Source: `DeviceSession.GetInfo()` returns an anonymous object; serialized by `ApiResponse.Success()` via
Newtonsoft.Json with `CamelCasePropertyNamesContractResolver` and `NullValueHandling.Ignore`.

```json
{
  "ok": true,
  "data": {
    "status": "Disconnected",
    "connectedDevice": null,
    "session": null
  }
}
```

When connected and session active:

```json
{
  "ok": true,
  "data": {
    "status": "SessionActive",
    "connectedDevice": "192.168.1.100:5555",
    "session": {
      "sessionId": "sess-20260415-143022",
      "createdAt": "2026-04-15T14:30:22Z",
      "packageActivity": "com.example.app/com.example.MainActivity",
      "pid": 12345,
      "captureIds": [2, 3]
    }
  }
}
```

`session` is a `DeviceSessionInfo` object (set by `LaunchJobRunner` on success, cleared by
`Disconnect()`). It contains:
- `sessionId` (string): "sess-" + UTC timestamp at creation
- `createdAt` (DateTime): UTC
- `packageActivity` (string): package/activity used for launch
- `pid` (uint): process ID (verified by `WaitForProcess`)
- `captureIds` (List<uint>): accumulates one entry per completed capture

`connectedDevice` (string): set from `deviceId` request param or `device.GetName()`. Cleared on
`Disconnect()`.

When `NullValueHandling.Ignore` applies, `null` fields are omitted from the JSON response.
So a disconnected server returns:
```json
{ "ok": true, "data": { "status": "Disconnected" } }
```

### 5. Job Summary Shape (GET /api/jobs or /api/jobs/{id})

`Job.ToSummary()` returns:

```json
{
  "ok": true,
  "data": {
    "id":          "cap-20260415-143022-001",
    "type":        "Capture",
    "status":      "Completed",
    "phase":       "archiving",
    "progress":    100,
    "createdAt":   "2026-04-15T14:30:22Z",
    "startedAt":   "2026-04-15T14:30:22.1Z",
    "finishedAt":  "2026-04-15T14:32:45.9Z",
    "result":      { "sdpPath": "/path/to/session.sdp", "captureId": 2, "sessionId": "sess-..." },
    "error":       null
  }
}
```

Job ID format: `{prefix}-{yyyyMMdd-HHmmss}-{sequence:D3}`
- `con-` = Connect
- `lnc-` = Launch
- `cap-` = Capture
- `ana-` = Analysis

JobStatus values: `Pending | Running | Cancelling | Completed | Failed | Cancelled`

JobType values: `Connect | Launch | Capture | Analysis`

Phase strings per job type:

| Job Type | Phases (in order)                                                                                          |
|----------|-------------------------------------------------------------------------------------------------------------|
| Connect  | `initializing_sdk` → `finding_device` → `connecting` → `verifying`                                        |
| Launch   | `checking_package` → `launching_app` → `waiting_process`                                                  |
| Capture  | `starting_capture` → `waiting_capture` → `waiting_data` → `importing` → `exporting` → `screenshot` → `archiving` |
| Analysis | `collect_dc` → `extract_assets` → `label_drawcalls` → `join_metrics` → `generate_stats` → `report_llm` → `dashboard` |

### 6. Does the Server Exit After Archive?

**Answer: NO — the server does NOT exit after archive completes in the normal code path.**

Evidence from `CaptureJobRunner.RunAsync()`:

1. Phase 7 ("archiving") calls `SessionArchiveService().CreateSessionArchive(sessionPath)` on a
   thread-pool thread.
2. After archive: sets `job.Result`, `job.Status = Completed`, `job.Progress = 100`.
3. The `finally` block executes: `session.TryTransition(Capturing, SessionActive)`.
4. The async task completes. `JobManager.Submit()` sets `job.FinishedAt` and disposes the CTS.
5. Control returns to the `HttpServer` accept loop, which continues running.
6. `ServerMode.Run()` main loop continues sleeping/polling.

No `Environment.Exit`, no `Process.Kill`, no shutdown signal is generated by the capture/archive
completion.

**Exception — indirect exit path via ConsolePlatform.ExitApplication():**

The ONLY `Environment.Exit` in the codebase is in `ConsolePlatform.ExitApplication()` (line 25).
This is called if the native Qualcomm SDK framework invokes `IPlatform.ExitApplication()`. This
can happen unpredictably during the background SDK update thread that runs continuously while the
`SDPClient` is initialized. This is a latent risk, NOT a deterministic post-archive exit.

Detailed analysis of this risk is in `FINDING-2026-04-15-snapshot-analysis-mode-switch.md`.

### 7. Sub-states and Events Not Captured in DeviceStatus

The following operational sub-states exist in code but are NOT visible via any HTTP endpoint:

#### A. Job-phase sub-states within each top-level DeviceStatus

While `DeviceStatus.Connecting` is active, the connect job may be in `initializing_sdk`,
`finding_device`, `connecting`, or `verifying`. The only way to observe this is to poll
`GET /api/jobs/{jobId}` for the specific connect job. `GET /api/device` returns only the
coarse `Connecting` state — no active-job reference.

Same pattern for Launching (3 phases), Capturing (7 phases), and Analysis (7 phases, but
analysis does not affect DeviceStatus at all).

#### B. No active-job cross-reference on /api/device

`GET /api/device` does NOT return the ID of any currently-running job associated with the
current state transition. If a client polls `/api/device` and sees `status: "Capturing"`, it
has no way to know which job ID to poll for progress without scanning `GET /api/jobs` and
filtering by `type=Capture, status=Running`.

#### C. No lastError field

When a connect/launch/capture job fails, the device state regresses (e.g. `Connecting →
Disconnected`, `Launching → Connected`). The error message is stored in `job.Error`, but
`GET /api/device` exposes no `lastError` or `lastFailedJobId` field. A WebUI watching only
`/api/device` cannot distinguish "cleanly disconnected" from "connect failed".

#### D. Transient states: Connected after launch failure

`LaunchJobRunner` on cancel/fail calls `TryTransition(Launching, Connected)`. This is
intentional — the device is still connected, only the app launch failed. The WebUI must handle
the `Connected` state appearing after a `Launching` state without a successful `SessionActive`,
and re-enable the "Launch" button accordingly. The current `app.js` does this via
`refreshSteps()` driven by the device poll — this works correctly.

#### E. Analysis has no device state representation

Analysis jobs run fully independently of `DeviceStatus`. When an analysis job is running,
`GET /api/device` shows `SessionActive` (or whatever it was before). There is no status
signal that analysis is in progress. A WebUI that wants to show "analyzing..." while analysis
runs must independently track the analysis job ID and poll it.

#### F. Session capture history on /api/device

`DeviceSessionInfo.captureIds` accumulates all capture IDs for the current session and is
included in the `/api/device` response under `session.captureIds`. This IS queryable — the
WebUI can read completed capture IDs from here without job polling.

#### G. SDK initialization state

`DeviceSession.SdkInitialized` is a static boolean. Once `EnsureSdkInitialized()` succeeds, it
stays `true` for the process lifetime. There is no endpoint to query whether SDK init has been
done. The first connect job always attempts SDK init; subsequent connects skip it.

### 8. ApiResponse Envelope

All responses use the same envelope defined in `ApiResponse.cs`:

```json
{ "ok": true|false, "data": <object or null>, "error": "<string or omitted>" }
```

- Serialized with camelCase property names
- `NullValueHandling.Ignore` — null fields are omitted from output
- `Content-Type: application/json; charset=utf-8`

## Analysis

### What Is Queryable for a WebUI Today

| UI Need                              | Available Now                        | How                                               |
|--------------------------------------|--------------------------------------|---------------------------------------------------|
| Is server alive?                     | Yes                                  | GET /api/status → `{ status:"ok" }` (200 = alive) |
| Current device phase (coarse)        | Yes                                  | GET /api/device → `data.status`                   |
| Connected device name                | Yes                                  | GET /api/device → `data.connectedDevice`          |
| Session ID + package                 | Yes                                  | GET /api/device → `data.session`                  |
| Completed capture IDs for session    | Yes                                  | GET /api/device → `data.session.captureIds`       |
| Active job progress for any operation | Yes (indirect)                      | GET /api/jobs (filter by type+status=Running)     |
| Job phase string and % progress      | Yes                                  | GET /api/jobs/{id} → `phase`, `progress`          |
| Last error from failed operation     | Partial — in job only                | GET /api/jobs/{id} → `error`; not in /api/device  |
| Is analysis running?                 | Partial — via job list only          | GET /api/jobs (filter type=Analysis, status=Running) |
| Server version                       | Yes                                  | GET /api/status → `data.version`                  |

### Gaps Blocking Correct WebUI Gating

**Gap 1: No activeJobId on /api/device**

When the UI observes a transition state (`Connecting`, `Launching`, `Capturing`), it needs to
poll the associated job for phase/progress. Currently, the UI must do a full `GET /api/jobs`
list scan to find the running job. This works but is fragile — if two jobs of the same type
exist (one running, one completed), the scan must correctly pick the running one.

Recommended addition to `DeviceSession.GetInfo()`:
```csharp
activeJobId = ActiveJob?.Id
```
`ActiveJob` is already a field on `DeviceSession` (set by handlers when they submit a job to
allow cancellation-via-disconnect). Exposing it in `/api/device` response would eliminate the
job-list scan.

**Gap 2: No lastError on /api/device**

When a job fails and the state regresses (e.g. `Connecting → Disconnected`), the `/api/device`
response just shows `Disconnected` with no indication of why. The UI must either poll all recent
jobs to find the failed one, or maintain its own job-id bookkeeping.

Recommended addition: `lastError` (string|null) and `lastFailedJobId` (string|null) on
`DeviceSession`, written by job runners on failure and cleared on next successful transition
or `Disconnect()`.

**Gap 3: No /health endpoint at standard path**

`/api/status` fills the liveness role but is not at a conventional health-check path.
Proxy infrastructure, load-balancers, or monitoring tools typically probe `/health` or
`/healthz`. The pySdp proxy does not currently probe a health endpoint — it infers liveness
from whether `/api/device` succeeds or raises `ConnectionError`.

**Gap 4: Analysis running state not surfaced on /api/device**

If the UI wants a global "system busy" indicator that covers analysis, it must track analysis
jobs separately. Adding an `analysisJobId` (string|null) field to `/api/device` — set when an
analysis job is submitted, cleared on terminal — would let the UI show analysis progress without
a separate job-list scan.

**Gap 5: /api/status version field is static**

`{ version: "1.0" }` is a hardcoded string. There is no way to know the server's build date
or commit hash. For WebUI diagnostics, a build timestamp would be useful.

## Impact

The current API is sufficient for the existing `app.js` WebUI because it tracks job IDs locally
in JS variables (`jobId` per operation section) and polls them directly. However:

1. On page reload, all JS state is lost — the UI shows stale button enablement until the next
   device poll fires (up to 3 seconds).
2. If a job was submitted before page reload, the UI cannot recover progress display without
   scanning `/api/jobs`.
3. The "is analysis running?" question cannot be answered from `/api/device` alone.

## Related Context

- Related findings: FINDING-2026-04-15-snapshot-analysis-mode-switch.md
- Related implementations: IMPL-2026-04-14-http-server-mode.md
- Related plans: PLAN-2026-04-11-http-server-mode.md
