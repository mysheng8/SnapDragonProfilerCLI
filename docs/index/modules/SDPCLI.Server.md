# MODULE INDEX — SDPCLI.Server

## Role

HTTP REST server mode for SDPCLI.  
Binds a `System.Net.HttpListener` to `localhost:{port}` only (never external).  
All long-running operations (connect, capture, analysis) are dispatched as async `Job` objects via `JobManager`.  
`DeviceSession` holds singleton device state with an atomic state-machine (`TryTransition`).

---

## Entry Points

| Symbol | Location |
|--------|----------|
| `ServerMode.Run()` | [source/Modes/ServerMode.cs](../../../SDPCLI/source/Modes/ServerMode.cs#L42) |
| `HttpServer.Start()` | [source/Server/HttpServer.cs](../../../SDPCLI/source/Server/HttpServer.cs#L49) |

---

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `ServerMode` | IMode implementation: creates `DeviceSession` + `JobManager`, starts `HttpServer`, blocks on 'q'/Ctrl+C | [source/Modes/ServerMode.cs](../../../SDPCLI/source/Modes/ServerMode.cs) |
| `HttpServer` | `HttpListener` accept loop; routes requests by prefix to `HandlerRouter` | [source/Server/HttpServer.cs](../../../SDPCLI/source/Server/HttpServer.cs) |
| `DeviceSession` | Singleton device state machine (`Disconnected/Connecting/Connected/Busy`); all transitions via `TryTransition()` | [source/Server/DeviceSession.cs](../../../SDPCLI/source/Server/DeviceSession.cs) |
| `JobManager` | Thread-safe `ConcurrentDictionary<id, Job>`; `Submit()` runs `Task`, TTL-based expiry | [source/Server/Jobs/JobManager.cs](../../../SDPCLI/source/Server/Jobs/JobManager.cs) |
| `Job` | Job object: `Id`, `Type`, `Status`, `Error`, `Result`, `CancellationTokenSource` | [source/Server/Jobs/Job.cs](../../../SDPCLI/source/Server/Jobs/Job.cs) |

---

## Handlers

| Handler | Route | Method | Location |
|---------|-------|--------|----------|
| `StatusHandler` | `GET /api/status` | Sync | [source/Server/Handlers/StatusHandler.cs](../../../SDPCLI/source/Server/Handlers/StatusHandler.cs) |
| `DeviceHandler` | `GET /api/device` | Sync | [source/Server/Handlers/DeviceHandler.cs](../../../SDPCLI/source/Server/Handlers/DeviceHandler.cs) |
| `ConnectHandler` | `POST /api/connect` | → `ConnectJobRunner` | [source/Server/Handlers/ConnectHandler.cs](../../../SDPCLI/source/Server/Handlers/ConnectHandler.cs) |
| `DisconnectHandler` | `POST /api/disconnect` | Sync | [source/Server/Handlers/DisconnectHandler.cs](../../../SDPCLI/source/Server/Handlers/DisconnectHandler.cs) |
| `LaunchHandler` | `POST /api/session/launch` | → `LaunchJobRunner` | [source/Server/Handlers/LaunchHandler.cs](../../../SDPCLI/source/Server/Handlers/LaunchHandler.cs) |
| `CaptureHandler` | `POST /api/capture` | → `CaptureJobRunner` | [source/Server/Handlers/CaptureHandler.cs](../../../SDPCLI/source/Server/Handlers/CaptureHandler.cs) |
| `AnalysisHandler` | `POST /api/analysis` | → `AnalysisJobRunner` | [source/Server/Handlers/AnalysisHandler.cs](../../../SDPCLI/source/Server/Handlers/AnalysisHandler.cs) |
| `JobsHandler` | `GET|DELETE /api/jobs[/{id}]` | Sync | [source/Server/Handlers/JobsHandler.cs](../../../SDPCLI/source/Server/Handlers/JobsHandler.cs) |

---

## Job Runners

| Runner | Delegates To | Location |
|--------|-------------|----------|
| `ConnectJobRunner` | `DeviceConnectionService` + `AppLaunchService` | [source/Server/Jobs/ConnectJobRunner.cs](../../../SDPCLI/source/Server/Jobs/ConnectJobRunner.cs) |
| `LaunchJobRunner` | `AppLaunchService` | [source/Server/Jobs/LaunchJobRunner.cs](../../../SDPCLI/source/Server/Jobs/LaunchJobRunner.cs) |
| `CaptureJobRunner` | `CaptureExecutionService` + `QGLPluginService` + `CsvToDbService` | [source/Server/Jobs/CaptureJobRunner.cs](../../../SDPCLI/source/Server/Jobs/CaptureJobRunner.cs) |
| `AnalysisJobRunner` | `AnalysisPipeline` | [source/Server/Jobs/AnalysisJobRunner.cs](../../../SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs) |

---

## Call Flow

```
ServerMode.Run()
├── new DeviceSession()
├── new JobManager(ttlMinutes)
├── new HttpServer(port, session, jobManager, config)
├── HttpServer.Start()           ← binds to localhost:{port}
│   └── [AcceptLoop thread]
│       └── HandlerRouter.Dispatch(context)
│           ├── POST /api/connect  → ConnectHandler
│           │   └── JobManager.Submit(Connect, ConnectJobRunner)
│           │       └── DeviceConnectionService.Connect() → DeviceSession
│           ├── POST /api/capture  → CaptureHandler
│           │   └── JobManager.Submit(Capture, CaptureJobRunner)
│           │       └── [mirrors SnapshotCaptureMode inner loop]
│           ├── POST /api/analysis → AnalysisHandler
│           │   └── JobManager.Submit(Analysis, AnalysisJobRunner)
│           │       └── AnalysisPipeline.RunAnalysis()
│           └── GET /api/jobs/{id} → JobsHandler → Job.Status / Job.Result
└── [block on 'q' / Ctrl+C]
    └── HttpServer.Stop() → JobManager.CancelAll()
```

---

## DeviceSession State Machine

```
Disconnected
  └─[ConnectJobRunner]→ Connecting → Connected
                                      └─[CaptureJobRunner]→ Busy → Connected
                       [DisconnectHandler]→ Disconnected
```

---

## Log → Code Map

| Log | Location |
|-----|----------|
| `"Listening on http://localhost:{port}/"` | [HttpServer.cs](../../../SDPCLI/source/Server/HttpServer.cs#L53) |
| `"Job {id} failed: {msg}"` | [JobManager.cs](../../../SDPCLI/source/Server/Jobs/JobManager.cs) |
| `"=== Server Mode ==="` | [ServerMode.cs](../../../SDPCLI/source/Modes/ServerMode.cs#L42) |
