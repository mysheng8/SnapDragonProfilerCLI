---
type: implementation
topic: HTTP server mode for SDPCLI
status: completed
based_on:
  - PLAN-2026-04-11-http-server-mode.md
related_paths:
  - SDPCLI/source/Server/
  - SDPCLI/source/Modes/ServerMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
summary: Implements HTTP REST server mode with async job system, device session state machine, and 4 async runners (connect/launch/capture/analysis).
last_updated: 2026-04-14
---

## Plan Reference

PLAN-2026-04-11-http-server-mode.md (Revision 3)

## Implementation Summary

Added `sdpcli server [--port N]` subcommand that starts a local HTTP REST API server
on localhost only. All 4 main operations are async jobs (202 + jobId). Clients poll
`GET /api/jobs/{id}` for completion.

## Files Changed

### New files (all under SDPCLI/source/Server/)

| File | Purpose |
|------|---------|
| `Jobs/JobType.cs` | enum: Connect/Launch/Capture/Analysis |
| `Jobs/JobStatus.cs` | enum: Pending/Running/Cancelling/Completed/Failed/Cancelled |
| `Jobs/Job.cs` | Job model with CTS and ToSummary() |
| `Jobs/JobManager.cs` | ConcurrentDictionary, Submit/SubmitAnalysis/Cancel/Remove/PurgeExpired |
| `Jobs/ConnectJobRunner.cs` | 4-phase connect: init_sdk → find_device → connecting → verifying |
| `Jobs/LaunchJobRunner.cs` | 3-phase launch: checking_package → launching_app → waiting_process |
| `Jobs/CaptureJobRunner.cs` | 7-phase capture: starting → waiting_capture → waiting_data → importing → exporting → screenshot → archiving |
| `Jobs/AnalysisJobRunner.cs` | 7-phase analysis with completedTargets accumulation |
| `DeviceSessionInfo.cs` | Value object: session metadata + DeviceStatus enum |
| `DeviceSession.cs` | State machine with TryTransition, EnsureSdkInitialized, GetInfo(), Disconnect() |
| `ApiResponse.cs` | JSON response envelope { ok, data, error } |
| `Handlers/BaseHandler.cs` | Abstract base with ReadJsonBody<T>, ValidatePath, WriteOk/WriteError |
| `Handlers/StatusHandler.cs` | GET /api/status |
| `Handlers/DeviceHandler.cs` | GET /api/device |
| `Handlers/ConnectHandler.cs` | POST /api/connect |
| `Handlers/DisconnectHandler.cs` | POST /api/disconnect |
| `Handlers/LaunchHandler.cs` | POST /api/session/launch |
| `Handlers/CaptureHandler.cs` | POST /api/capture |
| `Handlers/AnalysisHandler.cs` | POST /api/analysis |
| `Handlers/JobsHandler.cs` | GET/DELETE /api/jobs, POST /api/jobs/{id}/cancel |
| `HttpServer.cs` | HttpListener accept loop + HandlerRouter |

### New files (other paths)

| File | Purpose |
|------|---------|
| `SDPCLI/source/Modes/ServerMode.cs` | IMode implementation, starts server, blocks on q/Ctrl+C |

### Modified files

| File | Change |
|------|--------|
| `SDPCLI/config.ini` | Added Server.Port/AutoConnect/JobTtlMinutes comment block |
| `SDPCLI/source/Analysis/AnalysisPipeline.cs` | Add `completedTargets = AnalysisTarget.None` param to RunAnalysis(); add LoadLabelsFromLabelJson() helper; guard `doLabel` with `!completedTargets.HasFlag(Label)` |
| `SDPCLI/source/Application.cs` | Added `server` subcommand branch; added portArg/hostArg params |
| `SDPCLI/source/Main.cs` | Added `--port`/`--host` arg parsing; added portArg/hostArg to app.Run() |

## Key Changes

### State machine
DeviceStatus: Disconnected → Connecting → Connected → Launching → SessionActive → Capturing
- TryTransition(expected, next) is atomic (lock + compare)
- Disconnect() is always safe to call (force-resets to Disconnected)

### completedTargets parameter
When AnalysisJobRunner runs phases sequentially, each completed phase accumulates into
`completedTargets`. The next RunAnalysis() call receives this, allowing AnalysisPipeline
to skip re-labeling (expensive LLM calls) when Label was already done in a prior phase.
Fallback: LoadLabelsFromLabelJson() reads label.json written in the prior phase.

### Config injection
No Config.Current singleton was added — config is passed via constructor injection
through HandlerRouter → individual Handler → JobRunner.

### SubmitAnalysis vs Submit
JobManager.SubmitAnalysis() creates an AnalysisJob typed subclass so FindActiveAnalysis()
can do duplicate detection by (sdpPath, snapshotId) without casting.

## Build / Validation

- Command: `dotnet build SDPCLI.sln --configuration Debug`
- Result: **Build succeeded — 0 Warnings, 0 Errors**
- Iterations: 5 (missing override keywords, usings, API signature mismatches, PurgeExpired visibility)

## Deviations from Plan

1. **AnalysisJob.Targets field renamed to TargetsEnum** (type `AnalysisTarget` enum instead of `string?`) — cleaner
2. **DisconnectHandler constructor** dropped JobManager parameter — not needed (session.Disconnect() is sufficient)
3. **LoadLabelsFromLabelJson** added as new private method (not in original plan) — required by completedTargets implementation

## Issues Encountered

- BaseHandler.Handle() was abstract, not virtual — handlers required `override` not `public void`
- ReadJsonBody had signature `ReadJsonBody<T>(HttpListenerRequest)` not `ReadJsonBody<T>(context)` — fixed in all handlers
- SdpFileService, DrawCallAnalysisService, RawJsonGenerationService all require logger parameter
- PurgeExpired() was private in JobManager — needed by ServerMode's periodic loop → made public
- DeviceSession.GetInfo() was missing — added as part of this implementation

## Next Steps

1. **Integration test**: manually test `sdpcli server --port 5000` with curl or browser
2. **AutoConnect**: implement `Server.AutoConnect` config key in ConnectJobRunner to auto-connect on server start
3. **API docs**: generate OpenAPI spec from route table in HttpServer.cs
4. **Shutdown timeout**: ServerMode.Run() currently does Thread.Sleep(100) poll loop — could be replaced with WaitHandle
