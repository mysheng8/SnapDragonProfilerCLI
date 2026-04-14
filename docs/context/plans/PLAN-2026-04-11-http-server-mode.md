---
type: plan
topic: HTTP Server Mode — REST API for launch / capture / analysis with async job management
status: revised
based_on: []
related_paths:
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/ServerMode.cs          # new
  - SDPCLI/source/Server/                      # new directory
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/DeviceConnectionService.cs
  - SDPCLI/source/Services/Capture/CaptureExecutionService.cs
  - SDPCLI/source/Services/Capture/AppLaunchService.cs
  - SDPCLI/config.ini
related_tags:
  - server
  - http
  - rest
  - jobmanager
  - capture
  - analysis
  - async
  - state-machine
  - session
summary: |
  Adds a `server` subcommand (HttpListener, localhost:5000, no auth). All 4 operations —
  connect, launch, capture, analysis — are async jobs returning 202 + jobId. A 6-state
  DeviceStatus machine serializes operations via atomic TryTransition. AnalysisJobRunner
  uses an ExecutionPlan of 7 ordered phases; ct is checked between phases so cancel aborts
  at the next phase boundary (current phase runs to end). RunAnalysis gains a
  completedTargets parameter to prevent label/metrics re-run when status/topdc phases
  depend on them — label.json existence alone is insufficient. JobStatus gains Cancelling
  (transient: current phase still running, no more phases will start). Analysis request
  body gains targets field for fine-grained control. Duplicate analysis (same
  sdpPath+snapshotId) returns 409.
last_updated: 2026-04-13
---

# PLAN — HTTP Server Mode (Revision 2 — 2026-04-13)

## 1. Goal

Add a `server` mode to SDPCLI that exposes a local HTTP REST API.

**Confirmed decisions (from user review):**

| # | Decision |
|---|----------|
| Q1 | `config.ini` adds `Server.AutoConnect` (default `false`). If `true`, server auto-calls connect at startup using existing `PackageName`/`ActivityName` config. |
| Q2 | Launch creates a Session. Multiple captures may reuse the same session without re-launching. To start a new session the caller issues a new `POST /api/launch`. |
| Q3 | Capture conflict → `409 Conflict`. Connect/launch also serialize via state machine: wrong-state requests return `400 Bad Request`. |
| Q4 | Analysis output directory from request body; defaults to `<ProjectDir>/analysis/<sdp_basename>/` |
| Q5 | localhost-only, no authentication required. |
| Q6 | Job history in-memory only (process lifetime). Persistence is future work. |

---

## 2. Constraints

| Constraint | Detail |
|------------|--------|
| Framework | .NET 4.7.2 — `System.Net.HttpListener` built-in, zero new NuGet deps |
| JSON | `Newtonsoft.Json` already in `dll/`, all services already use it |
| IMode interface | `ServerMode.Run()` blocks on HttpListener loop until Ctrl+C |
| Connect is long-running | `WaitForDeviceReady` polls up to **60s**; `EstablishNetworkConnection` polls up to **30s** — total ~90s via `Thread.Sleep(1000)` loops — **must be async job** |
| Launch is long-running | `WaitForProcess` polls up to **30s** via `Thread.Sleep` — **must be async job** |
| Capture is long-running | Capture trigger + `WaitForDataProcessed` ~120s — **must be async job** |
| Analysis is long-running | `AnalysisPipeline.RunAnalysis` ~60s — **must be async job** |
| All 4 ops are async | Every stateful operation returns 202 + jobId. HTTP handler only gates state and submits. |
| Security | Bind to `localhost` by default; `--host *` requires explicit flag + Windows admin |

---

## 3. config.ini additions

```ini
# ---------------------------------------------------------------------------
# Server Mode
# ---------------------------------------------------------------------------
# Port for HTTP server mode (SDPCLI.bat server)
# Server.Port=5000

# If true, automatically connect to device on server startup using PackageName config.
# Default: false — wait for POST /api/connect
# Server.AutoConnect=false

# Job TTL in minutes: completed/failed/cancelled jobs older than this are purged.
# Default: 60
# Server.JobTtlMinutes=60
```

`Config.cs` already has generic `Get(key, default)` — no changes to Config.cs needed.
`ServerMode` reads these keys at construction time:

```csharp
int   port       = config.GetInt("Server.Port", 5000);
bool  autoConn   = config.Get("Server.AutoConnect", "false") == "true";
int   jobTtlMin  = config.GetInt("Server.JobTtlMinutes", 60);
```

---

## 4. DeviceStatus State Machine

The state machine is the central serialization mechanism.
**All operations validate and atomically transition state before executing.**
No separate semaphore/lock is needed — the state transition IS the lock.

```
                  ┌─────────────────────┐
                  │    Disconnected      │◄──── Disconnect() always returns here
                  └──────────┬──────────┘
                             │ POST /api/connect
                             ▼
                  ┌──────────────────────┐
                  │     Connecting        │  (sync HTTP, ~5–15s)
                  └──────────┬───────────┘
                             │ success
                             ▼
                  ┌──────────────────────┐
                  │     Connected         │
                  └──────────┬───────────┘
                             │ POST /api/launch
                             ▼
                  ┌──────────────────────┐
                  │     Launching         │  (sync HTTP, ~3–10s)
                  └──────────┬───────────┘
                             │ success
                             ▼
                  ┌──────────────────────┐◄─── POST /api/launch (re-launch)
                  │    SessionActive      │
                  └──────────┬───────────┘
                             │ POST /api/capture (async job)
                             ▼
                  ┌──────────────────────┐
                  │     Capturing         │  (async, 30–120s)
                  └──────────┬───────────┘
                             │ job Completed / Failed / Cancelled
                             ▼
                  ┌──────────────────────┐
                  │    SessionActive      │  (session persists, ready for next capture)
                  └──────────────────────┘
```

### Operation pre-condition table

Every incoming request checks status **before** attempting any work:

| Operation | Required pre-state | Error if wrong state | Transition on entry | Transition on exit |
|-----------|-------------------|---------------------|---------------------|--------------------|
| `POST /api/connect` | `Disconnected` | `409` "already connected or connecting" | → `Connecting` | → `Connected` (or → `Disconnected` on failure) |
| `POST /api/disconnect` | any | — | — | → `Disconnected` (cancels any running capture job first) |
| `POST /api/launch` | `Connected` or `SessionActive` | `400` "connect first" / `409` "device busy" | → `Launching` | → `SessionActive` (or → `Connected` on failure) |
| `POST /api/capture` | `SessionActive` | `400` "launch first" / `409` "capture in progress" | → `Capturing` (via job runner) | → `SessionActive` |
| `POST /api/analysis` | any | — | — (file-based, no state change) | — |
| `GET /api/device` | any | — | — | — |
| `GET /api/jobs/*` | any | — | — | — |

### Wrong-state responses

```
Connecting  + any device op  → 409  { error: "device busy: connecting" }
Launching   + capture/launch → 409  { error: "device busy: launching" }
Capturing   + new capture    → 409  { error: "capture already in progress" }
Capturing   + launch         → 409  { error: "device busy: capturing" }
Connected   + capture        → 400  { error: "no active session, call /api/launch first" }
Disconnected + launch/capture → 400  { error: "device not connected" }
```

### Atomic transition implementation

```csharp
// DeviceSession internal — all callers must use TryTransition to change state
private volatile DeviceStatus _status = DeviceStatus.Disconnected;
private readonly object _transitionLock = new object();

private bool TryTransition(DeviceStatus expected, DeviceStatus next)
{
    lock (_transitionLock)
    {
        if (_status != expected) return false;
        _status = next;
        return true;
    }
}

public DeviceStatus Status => _status;   // volatile read, safe without lock
```

---

## 5. Session Model

`Session` is a lightweight value nested inside `DeviceSession`.
Created when `LaunchAsync()` succeeds. Replaced on each new launch (old session overwritten).
Captures within the same session all share the same `SdpClient` and `Device` objects.

```csharp
public class DeviceSessionInfo
{
    public string   SessionId       { get; init; }   // e.g. "sess-20260413-143022"
    public DateTime CreatedAt       { get; init; }
    public string   PackageActivity { get; init; }
    public List<string> CaptureIds  { get; }         // accumulated per capture job
}
```

`DeviceSession` exposes:
```csharp
public DeviceSessionInfo? CurrentSession { get; private set; }
```

---

## 6. API Surface

### 6.1 Device lifecycle

```
POST /api/connect
  body:  { "deviceId": "optional adb serial" }
  202:   { ok:true, data:{ jobId:"con-20260413-143020" } }
  409:   { ok:false, error:"already connected or connecting" }

POST /api/disconnect
  200:   { ok:true }
  (always succeeds; cancels active connect/launch/capture job first)

GET /api/device
  200:   { ok:true, data:{ status, deviceId, session:{ sessionId, packageActivity, captureCount } } }
```

### 6.2 App / session lifecycle

```
POST /api/launch
  body:  { "packageActivity": "com.example/MainActivity" }
         (optional — falls back to config.ini PackageName + ActivityName)
  202:   { ok:true, data:{ jobId:"lnc-20260413-143022" } }
  400:   { ok:false, error:"device not connected" }
  409:   { ok:false, error:"device busy: <status>" }
```

### 6.3 Capture (async)

```
POST /api/capture
  body (all optional):
    { "outputDir": "absolute or relative path",
      "captureLabel": "optional human label" }
  202:   { ok:true, data:{ jobId:"cap-20260413-143022" } }
  400:   { ok:false, error:"no active session" / "device not connected" }
  409:   { ok:false, error:"capture already in progress" }
```

### 6.4 Analysis (async)

```
POST /api/analysis
  body:
    { "sdpPath":    "required — absolute or relative to ProjectDir",
      "snapshotId": 2,           // optional, default = latest
      "outputDir":  "optional — defaults to <ProjectDir>/analysis/<basename>/",
      "targets":    "shaders,textures,label"  // optional, default = "all"
    }                                          // maps to AnalysisTarget.Parse() — existing impl
  202:   { ok:true, data:{ jobId:"ana-20260413-143025" } }
  400:   { ok:false, error:"sdpPath required" / "path outside allowed root" }
  409:   { ok:false, error:"duplicate analysis in progress for same sdpPath+snapshotId" }
```

`targets` field values: `dc`, `shaders`, `textures`, `buffers`, `label`, `metrics`,
`status`, `topdc`, `analysis`, `dashboard`, `all`.
Dependencies are resolved automatically by `AnalysisTarget.ExpandWithDependencies()` (existing).

### 6.5 Job management

```
GET /api/jobs
  200:   { ok:true, data:[ Job, ... ] }   // all jobs, newest first

GET /api/jobs/{id}
  200:   { ok:true, data: Job }
  404:   { ok:false, error:"job not found" }

DELETE /api/jobs/{id}
  200:   { ok:true }   // cancels if Running/Pending, or removes if terminal
  404:   { ok:false, error:"job not found" }
```

### 6.6 Status

```
GET /api/status
  200:
  { ok:true, data:{
      uptime:        "00:12:34",
      device:        { status, deviceId, session },
      activeJobs:    1,
      totalJobs:     7,
      serverVersion: "1.0"
  }}
```

### 6.7 Response envelope

All responses use a consistent wrapper:

```json
{
  "ok":    true | false,
  "data":  { ... } | null,
  "error": null   | "human-readable error string"
}
```

HTTP status codes:
- `200` success (read-only ops: GET /api/device, GET /api/jobs/*, GET /api/status, POST /api/disconnect)
- `202` accepted (async job submitted: connect, launch, capture, analysis)
- `400` bad request (invalid args, wrong pre-state — caller must fix before retry)
- `404` not found
- `409` conflict (state conflict — retry after current job completes)
- `500` unexpected server error

---

## 7. Job Model

```csharp
public class Job
{
    public string     Id         { get; init; }    // "cap-20260413-143022"
    public JobType    Type       { get; init; }    // Capture | Analysis
    public JobStatus  Status     { get; set; }     // Pending|Running|Completed|Failed|Cancelled
    public string?    Phase      { get; set; }     // e.g. "waiting_capture", "exporting"
    public int        Progress   { get; set; }     // 0–100
    public DateTime   CreatedAt  { get; init; }
    public DateTime?  StartedAt  { get; set; }
    public DateTime?  FinishedAt { get; set; }
    public object?    Result     { get; set; }     // not serialized as CTS
    public string?    Error      { get; set; }

    [JsonIgnore]
    public CancellationTokenSource Cts { get; } = new CancellationTokenSource();
}
```

### Job state machine

```
Pending ──► Running ──► Completed
                  └───► Failed
Pending  ──► Cancelled
Running  ──► Cancelling ──► Cancelled   (analysis: current phase runs to end, then stops)
Running  ──► Cancelled               (connect/launch/capture: ct propagated into poll loops)
```

`Cancelling` is a transient state used only by `AnalysisJobRunner` to signal that the
current `RunAnalysis(phase)` call is still executing but no more phases will start after
it finishes. Client polling `GET /api/jobs/{id}` sees `Cancelling` + current `Phase` until
the active phase ends, then `Cancelled`.

---

## 8. JobManager

```csharp
public class JobManager
{
    private readonly ConcurrentDictionary<string, Job> _jobs = new();
    private readonly TimeSpan _ttl;   // from Server.JobTtlMinutes config

    public Job Submit(JobType type, Func<Job, CancellationToken, Task> runner);
    //  - creates Job (Status=Pending, unique Id)
    //  - Task.Run: sets StartedAt, Status=Running → calls runner → catches → sets terminal state
    //  - returns Job immediately (before runner starts)

    public Job? Get(string id)             => _jobs.TryGetValue(id, out var j) ? j : null;
    public IEnumerable<Job> List()         => _jobs.Values.OrderByDescending(j => j.CreatedAt);
    public bool Cancel(string id);         // Cts.Cancel(); if Pending → Cancelled immediately
    public void PurgeExpired();            // called before each Submit; removes terminal jobs older than TTL
}
```

Id format: `"cap-yyyyMMdd-HHmmss"` (capture) / `"ana-yyyyMMdd-HHmmss"` (analysis).
Collision guard: append `-N` suffix if same-second duplicate.

---

## 9. DeviceSession

```csharp
public class DeviceSession
{
    // ── State (atomic via TryTransition) ──────────────────────────────────
    public DeviceStatus     Status            { get; }   // volatile read
    public string?          ConnectedDeviceId { get; private set; }
    public DeviceSessionInfo? CurrentSession  { get; private set; }

    // ── SDK references ────────────────────────────────────────────────────
    private SDPClient?           _sdpClient;
    private CliClientDelegate?   _clientDelegate;
    private Device?              _device;

    // ── Async events (reused across captures within a session) ────────────
    private ManualResetEvent _captureCompleteEvent = new(false);
    private ManualResetEvent _dataProcessedEvent   = new(false);
    private ManualResetEvent _importCompleteEvent  = new(false);

    // ── State transition gate (handlers call this; job runners call it too) ──
    public bool TryTransition(DeviceStatus expected, DeviceStatus next);

    // ── Called by job runners only (never by handlers) ────────────────────
    internal Task RunConnectAsync(string? deviceId, Job job, Config config, CancellationToken ct);
    internal Task RunLaunchAsync(string? packageActivity, Job job, Config config, CancellationToken ct);
    internal Task<(string sdpPath, string captureId)>
        RunCaptureAsync(string? outputDir, Job job, Config config, CancellationToken ct);

    // ── Disconnect (synchronous, handler-called) ──────────────────────────
    public void Disconnect();  // cancels active job CTS, tears down SDPClient, → Disconnected
}
```

Handlers **never** call RunConnectAsync/RunLaunchAsync/RunCaptureAsync directly.
The sequence is always:

```
Handler:
  1. if (!TryTransition(expectedFrom, transitioning)) → return 409/400
  2. job = jobManager.Submit(type, (j, ct) => session.RunXxxAsync(..., j, ct))
  3. return 202 + job.Id

Job runner:
  ... work ...
  finally: TryTransition(transitioning, settled)   // always restore state
```

---

## 10. ConnectJobRunner Flow

```
[Handler: TryTransition(Disconnected → Connecting), submit job, return 202]

RunConnectAsync(deviceId?, job, config, ct):

  Phase="initializing_sdk"       Progress=5
  SdpApp.Init(ConsolePlatform)
  new QGLPlugin()
  SDPClient.Initialize(SessionSettings, CliClientDelegate)

  Phase="finding_device"         Progress=15
  deviceManager.FindDevices()
  Thread.Sleep(2000)  // SDK device discovery delay

  Phase="waiting_ready"          Progress=25
  Poll device.GetDeviceState() == Ready, max 60s, 1s ticks
  Check ct each tick

  Phase="establishing_network"   Progress=65
  device.Connect(timeout, basePort)
  Poll device.GetDeviceState() == Connected, max 30s, 1s ticks
  Check ct each tick

  Phase="verifying"              Progress=95
  Thread.Sleep(1000)  // SDK post-connect stabilize

  job.Result = new { deviceId = connectedDeviceName }
  job.Status = Completed, Progress=100
  TryTransition(Connecting → Connected)

finally (on failure or cancellation):
  TryTransition(Connecting → Disconnected)
  SDPClient teardown if partially initialized
```

---

## 11. LaunchJobRunner Flow

```
[Handler: validates Status==Connected, TryTransition(Connected → Launching), submit job, return 202]

RunLaunchAsync(packageActivity?, job, config, ct):

  Phase="checking_package"       Progress=10
  adb shell pm list packages <pkg>
  Check ct

  Phase="fetching_activities"    Progress=25
  adb dumpsys package | grep activities
  Check ct

  Phase="launching_app"          Progress=50
  sdpClient.Client.LaunchApp(launchTarget)  or  DeviceConnectionService.LaunchViaAdb()

  Phase="waiting_process"        Progress=65
  WaitForProcess(): poll ProcessManager / CliClientDelegate, max 30s, 1s ticks
  Check ct each tick

  Phase="activating_metrics"     Progress=85
  MetricManager: activate required + optional metrics

  job.Result = new { sessionId, packageActivity, pid }
  CurrentSession = new DeviceSessionInfo(...)
  job.Status = Completed, Progress=100
  TryTransition(Launching → SessionActive)

finally (on failure or cancellation):
  TryTransition(Launching → Connected)   // failed launch leaves device connected
```

---

## 13. AnalysisJobRunner Flow

`AnalysisPipeline.RunAnalysis()` is synchronous and has no `CancellationToken` parameter.
The runner wraps it by splitting execution into ordered phases, checking `ct` **between**
phases. Cancel aborts at the next phase boundary — the current phase runs to completion.

### 13.1 ExecutionPlan

```csharp
// Fixed ordered sequence — each entry is one RunAnalysis() call
private static readonly (string Phase, AnalysisTarget Mask, int ProgressEnd)[] ExecutionPlan =
{
    ( "collect_dc",      AnalysisTarget.Dc,                                        12 ),
    ( "extract_assets",  AnalysisTarget.Shaders | AnalysisTarget.Textures
                                                | AnalysisTarget.Buffers,          42 ),
    ( "label_drawcalls", AnalysisTarget.Label,                                     65 ),
    ( "join_metrics",    AnalysisTarget.Metrics,                                   75 ),
    ( "generate_stats",  AnalysisTarget.Status  | AnalysisTarget.TopDc,            85 ),
    ( "report_llm",      AnalysisTarget.Analysis,                                  95 ),
    ( "dashboard",       AnalysisTarget.Dashboard,                                100 ),
};
```

Notes on grouping:
- `extract_assets` combines Shaders+Textures+Buffers because `RunAnalysis` runs them with
  `Task.WhenAll` internally — splitting them into separate calls would lose the parallelism.
- `generate_stats` combines Status+TopDc because TopDc immediately consumes Status output
  in memory without re-reading the file.

### 13.2 Runner algorithm

```csharp
RunAnalysisAsync(request, job, ct):

  Phase="opening_sdp"   Progress=5
  Resolve sdpPath, extract sdp.db, determine sessionDir
  Resolve outputDir (body → default: ProjectDir/analysis/<sdp_basename>/)
  uint captureId = ResolveCaptureId(request.SnapshotId, dbPath)

  // Expand user-requested targets once (includes dependency cascade)
  AnalysisTarget requested    = AnalysisTarget.Parse(request.Targets)
  AnalysisTarget fullRequired = requested.ExpandWithDependencies()
  AnalysisTarget completed    = AnalysisTarget.None

  foreach (phase, mask, progressEnd) in ExecutionPlan:

    effective = fullRequired & mask
    if (effective == None) continue          // not needed by this request

    // ← cancellation checkpoint: before every phase
    if (ct.IsCancellationRequested):
      job.Status = JobStatus.Cancelling      // signal to poller: cancelling after this point
      // no more phases will execute
      break

    job.Phase    = phase
    job.Progress = progressEnd - 10

    // Pass completed set so pipeline can skip already-done steps
    // (requires AnalysisPipeline.RunAnalysis completedTargets param — see §20)
    await Task.Run(() =>
      pipeline.RunAnalysis(dbPath, sessionDir, captureId,
                           target:           effective,
                           completedTargets: completed))

    completed |= effective                   // accumulate for next phase
    job.Progress = progressEnd

  if (!ct.IsCancellationRequested):
    job.Status = Completed
    job.Progress = 100
    job.Result = new { outputDir = sessionDir, reportPath = ... }
  else:
    job.Status = Cancelled
```

### 13.3 Cancel behaviour examples

```
Cancel during extract_assets (Shaders+Textures running):
  → extract_assets runs to completion (may take 1–3 min)
  → label_drawcalls checkpoint fires → Cancelling → break
  → dc.json, shaders.json, textures.json on disk; label.json absent

Resume with targets=label (re-submit after cancel):
  fullRequired = Dc | Shaders | Label   (via ExpandWithDependencies)
  collect_dc:     RunAnalysis(Dc, completed=None)        → fast DB query
  extract_assets: RunAnalysis(Shaders, completed=Dc)     → per-file check, all skip
  label_drawcalls: RunAnalysis(Label, completed=Dc|Shaders) → runs LLM labeling
  → Completed
```

### 13.4 Duplicate analysis guard

Before submitting, `AnalysisHandler` checks `JobManager` for a Running/Pending job with
the same `(sdpPath, snapshotId)`. If found → `409 Conflict`.

---

## 14. Auto-Connect on Startup

In `ServerMode.Run()`:

```csharp
bool autoConnect = config.Get("Server.AutoConnect", "false")
                         .Equals("true", StringComparison.OrdinalIgnoreCase);
if (autoConnect)
{
    _log.Info("AutoConnect enabled — submitting connect job...");
    // Submit as async job so server loop starts immediately
    _jobManager.Submit(JobType.Connect,
        (j, ct) => _deviceSession.RunConnectAsync(null, j, _config, ct));
}
// start HttpListener loop
```

---

## 15. HttpServer Dispatch

```csharp
private void ProcessRequest(HttpListenerContext ctx)
{
    string method = ctx.Request.HttpMethod.ToUpper();
    string path   = ctx.Request.Url.AbsolutePath.TrimEnd('/').ToLower();

    IHandler handler = (method, path) switch
    {
        ("POST",   "/api/connect")    => new ConnectHandler(_session, _jobs, _config),
        ("POST",   "/api/disconnect") => new DisconnectHandler(_session, _jobs),
        ("GET",    "/api/device")     => new DeviceHandler(_session),
        ("POST",   "/api/launch")     => new LaunchHandler(_session, _jobs, _config),
        ("POST",   "/api/capture")    => new CaptureHandler(_session, _jobs, _config),
        ("POST",   "/api/analysis")   => new AnalysisHandler(_jobs, _config),
        ("GET",    "/api/jobs")       => new JobsHandler(_jobs),
        ("GET",  _ ) when path.StartsWith("/api/jobs/") => new JobsHandler(_jobs),
        ("DELETE",_ ) when path.StartsWith("/api/jobs/") => new JobsHandler(_jobs),
        ("GET",    "/api/status")     => new StatusHandler(_session, _jobs, _startTime),
        _ => null
    };

    if (handler == null) { WriteNotFound(ctx); return; }
    try   { handler.Handle(ctx); }
    catch (Exception ex) { WriteError500(ctx, ex.Message); }
}
```

---

## 16. File Structure

```
SDPCLI/source/
  Modes/
    ServerMode.cs                   # IMode: owns HttpServer + DeviceSession + JobManager

  Server/
    HttpServer.cs                   # HttpListener, threaded dispatch loop, Ctrl+C stop
    ApiResponse.cs                  # { bool Ok, T? Data, string? Error } with Write helpers
    DeviceSession.cs                # 6-state machine; internal RunConnectAsync/LaunchAsync/CaptureAsync
    DeviceSessionInfo.cs            # session value object (created at launch)

    Jobs/
      Job.cs                        # Job fields + CTS
      JobType.cs                    # enum Connect | Launch | Capture | Analysis
      JobStatus.cs                  # enum Pending | Running | Completed | Failed | Cancelled
      JobManager.cs                 # Submit / Get / List / Cancel / PurgeExpired

      ConnectJobRunner.cs           # 5-phase: initializing_sdk → finding_device → waiting_ready → establishing_network → verifying
      LaunchJobRunner.cs            # 5-phase: checking_package → fetching_activities → launching_app → waiting_process → activating_metrics
      CaptureJobRunner.cs           # 7-phase: starting → waiting_capture → waiting_data → importing → exporting → screenshot → archiving
      AnalysisJobRunner.cs          # ExecutionPlan (7 phases) with ct checkpoint between each;
                                    # calls RunAnalysis(target, completedTargets) per phase

    Handlers/
      BaseHandler.cs                # IHandler + ReadJsonBody<T> + WriteOk/WriteError/ValidatePath
      ConnectHandler.cs             # POST /api/connect   (gate: Disconnected → Connecting)
      DisconnectHandler.cs          # POST /api/disconnect
      DeviceHandler.cs              # GET  /api/device
      LaunchHandler.cs              # POST /api/launch    (gate: Connected → Launching)
      CaptureHandler.cs             # POST /api/capture   (gate: SessionActive → Capturing)
      AnalysisHandler.cs            # POST /api/analysis  (no state gate)
      JobsHandler.cs                # GET/DELETE /api/jobs[/{id}]
      StatusHandler.cs              # GET /api/status
```

17 new files total.

---
      CaptureHandler.cs             # POST /api/capture  (validates state, submits job)
      AnalysisHandler.cs            # POST /api/analysis (validates paths, submits job)
      JobsHandler.cs                # GET /api/jobs, GET /api/jobs/{id}, DELETE /api/jobs/{id}
      StatusHandler.cs              # GET /api/status
```

---

## 15. Application.cs / Main.cs Changes

`Main.cs` — add flag parsing:

```csharp
if ((lo == "--port") && i + 1 < args.Length) { portArg = args[++i]; continue; }
if ((lo == "--host") && i + 1 < args.Length) { hostArg = args[++i]; continue; }
```

`Application.cs` — add subcommand branch (after the existing `snapshot` branch):

```csharp
else if (subcommand == "server")
{
    int port    = portArg != null && int.TryParse(portArg, out int p) ? p
                                                                       : config.GetInt("Server.Port", 5000);
    string host = hostArg ?? "localhost";
    Console.WriteLine($"\n=== Snapdragon Profiler CLI - Server Mode (http://{host}:{port}/) ===");
    mode = new ServerMode(config, testPath, port, host);
}
```

---

## 16. Security Constraints

| Rule | Implementation |
|------|----------------|
| Bind localhost by default | Prefix = `http://localhost:{port}/` unless `--host *` (requires Windows admin) |
| Path traversal prevention | `BaseHandler.ValidatePath(path, allowedRoot)`: checks `Path.GetFullPath(path).StartsWith(allowedRoot)` |
| Request body size limit | Read at most 64 KB; close connection if exceeded |
| No credentials in responses | Job results contain file paths only |
| Error details scoped | 500 responses include message but not stack trace in production |

---

## 17. Known Limitations (document for future work)

| Limitation | Notes |
|------------|-------|
| Analysis cancel latency | Cancel fires at phase boundary; current phase (up to ~3 min for LLM) runs to end |
| Single device only | `DeviceSession` is a singleton; no multi-device support |
| Job history in-memory | Lost on process restart; persistence is future work |
| `--host *` not tested by default | Requires Windows `netsh` URL ACL setup |
| Analysis progress estimate | Progress crawls during LLM label phase; no sub-DC granularity from job endpoint |

---

## 18. Validation Plan

1. `GET /api/status` → 200, `device.status = "Disconnected"`
2. `POST /api/capture` while disconnected → 400 `"device not connected"`
3. `POST /api/connect` → 200, `GET /api/device` → `"Connected"`
4. `POST /api/capture` before launch → 400 `"no active session"`
5. `POST /api/launch` → 200, `GET /api/device` → `"SessionActive"`, `session.sessionId` set
6. `POST /api/capture` → 202, returns `jobId`
7. `POST /api/capture` while first running → 409
8. `GET /api/jobs/{id}` polls until `status = "Completed"`, `result.sdpPath` exists on disk
9. `POST /api/analysis` with returned `sdpPath` → 202 → `Completed`, `result.reportPath` exists
10. `GET /api/jobs` → lists all jobs
11. `DELETE /api/jobs/{id}` → removed; subsequent GET → 404
12. `POST /api/connect` with `Server.AutoConnect=true` in config → auto-connects on server start
13. Disconnect during capture → job status becomes `Failed` or `Cancelled`; status returns to `Disconnected`

---

## 19. Implementation Order (for Executor agent)

1. `config.ini` — add `Server.*` comment block (no code change to `Config.cs` needed)
2. `Job.cs`, `JobType.cs` (enum: `Connect | Launch | Capture | Analysis`), `JobStatus.cs` (add `Cancelling`)
3. `JobManager.cs`
4. `ApiResponse.cs` + `BaseHandler.cs` (IHandler interface)
5. `DeviceSessionInfo.cs`
6. `DeviceSession.cs` — state machine skeleton + `TryTransition` + `Disconnect`
7. `ConnectJobRunner.cs` — 5-phase connect flow
8. `LaunchJobRunner.cs` — 5-phase launch flow
9. `CaptureJobRunner.cs` — 7-phase capture flow
10. **`AnalysisPipeline.cs` — add `completedTargets` parameter (see §20)**
11. `AnalysisJobRunner.cs` — ExecutionPlan loop, ct checkpoints, completedTargets accumulation
12. `DeviceSession` — wire `RunConnectAsync`, `RunLaunchAsync`, `RunCaptureAsync` to runners
13. `HttpServer.cs` dispatch
14. `ServerMode.cs` — IMode wiring, AutoConnect job submit
15. `Main.cs` — `--port`/`--host` flags
16. `Application.cs` — `server` subcommand branch
17. All Handlers (Connect, Disconnect, Device, Launch, Capture, Analysis, Jobs, Status)
18. End-to-end validation per §18

---

## 20. Required Pipeline Change — `completedTargets` parameter

**File:** `SDPCLI/source/Analysis/AnalysisPipeline.cs`

**Problem:** When `AnalysisJobRunner` runs Phase N (e.g. `Status`) after Phase N-1 (e.g.
`Label`) has already completed, the pipeline re-computes `doLabel = true` on the second call
because `ExpandWithDependencies(Status)` includes `Label`. LLM labeling would re-run even
though `label.json` was written in the previous phase — wasting minutes.

**Root cause:** `doLabel` is computed purely from `target` flags; there is no "already done"
input. The only existing guards are per-file existence checks for binary assets
(shaders/textures/meshes), not for JSON-producing steps like labeling.

**Fix: add `completedTargets` parameter**

```csharp
// Signature change
public void RunAnalysis(string dbPath, string sessionDir, uint captureId,
                        int? cmdBufferFilter = null,
                        AnalysisTarget target = AnalysisTarget.All,
                        AnalysisTarget completedTargets = AnalysisTarget.None)  // NEW

// doLabel guard — add completedTargets check
bool doLabel = !onlyReport
            && !completedTargets.HasFlag(AnalysisTarget.Label)   // ← NEW: skip if already done
            && (targetIsAll || effectiveTarget.HasFlag(AnalysisTarget.Label));

// When label is a dependency but already completed: load from label.json instead of re-running
if (!doLabel && effectiveTarget.HasFlag(AnalysisTarget.Label))
    LoadLabelsFromLabelJson(report, captureOutDir);   // ← NEW helper (reads label.json)
```

Apply the same pattern to other JSON-producing steps:

| Flag | `do*` guard | Already-done fallback |
|------|------------|----------------------|
| `Label` | `!completedTargets.HasFlag(Label)` | `LoadLabelsFromLabelJson()` |
| `Metrics` | `!completedTargets.HasFlag(Metrics)` | `LoadMetricsFromMetricsJson()` |
| `Status` | `!completedTargets.HasFlag(Status)` | `LoadStatusResult()` (already partially exists) |

Asset extraction steps (Shaders/Textures/Buffers) already have per-file existence checks
internally — no `completedTargets` guard needed for them.

**Scope:** Changes are confined to `AnalysisPipeline.cs` + one new private helper method
`LoadLabelsFromLabelJson()`. CLI call sites (`AnalysisMode.cs`) pass `completedTargets:
Default` and are unaffected.

---

