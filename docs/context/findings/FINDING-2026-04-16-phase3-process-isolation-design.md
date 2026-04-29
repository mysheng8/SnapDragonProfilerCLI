---
type: finding
topic: Phase 3 process isolation — technical constraints and IPC design validation
status: investigated
related_paths:
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Server/Jobs/JobManager.cs
  - SDPCLI/source/Server/Jobs/Job.cs
  - SDPCLI/source/Server/Handlers/ConnectHandler.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - pySdp/webui/static/app.js
related_tags:
  - phase3
  - process-isolation
  - ipc
  - snapshot-worker
  - analysis-worker
  - named-pipe
  - child-process
summary: |
  Technical validation of the user's Phase 3 architecture: server as main process,
  snapshot as one-at-a-time child process (named pipe IPC), analysis as parallel
  child processes (stdout JSON-lines). Key findings: Application.cs dispatch model
  makes adding new subcommands (snapshot-worker, analysis-worker) trivial; TextureConverter.dll
  is needed by analysis children (P/Invoke) but SetDllDirectory in Main.cs covers it;
  ShaderExtractor only spawns spirv-cross.exe via Process.Start (no P/Invoke); .NET 4.7.2
  has full System.IO.Pipes support; OutputDataReceived + BeginOutputReadLine is the
  correct async stdout pattern for analysis workers; SnapshotWorkerManager requires a
  single-child lock with HTTP 409 + force=true override.
last_updated: 2026-04-16
---

# Finding: Phase 3 Process Isolation — Technical Constraints

## Architecture Validated

User proposed:
1. `sdpcli server` = main process (HTTP, JobManager, no SDK)
2. Snapshot = single child process (one at a time, device constraint)
3. Analysis = parallel child processes (stateless, can run multiple in parallel)

Each child is the same `SDPCLI.exe` binary with a new internal subcommand.

---

## Constraint 1: Binary & DLL Loading

`Main.cs` calls `SetDllDirectory(dllPath)` and modifies `PATH` before any CLR type loading.
This applies to every invocation of `SDPCLI.exe` regardless of subcommand — children
inherit the correct DLL search paths automatically.

**Snapshot worker child**: needs all SDK DLLs (SDPCoreWrapper, SDPClientFramework,
QGLPlugin, TextureConverter). All are in `dll/` which is handled by `SetDllDirectory`. ✓

**Analysis worker child**: needs only `TextureConverter.dll` (via `TextureExtractor.cs` P/Invoke)
and `spirv-cross.exe` (via `ShaderExtractor.cs` `Process.Start` — not P/Invoke). Does NOT
need SDPCoreWrapper, SDPClientFramework, or QGLPlugin. `SetDllDirectory` covers TextureConverter. ✓

**Main server process**: after Phase 3, loads NO native SDK DLLs at all. The DLL setup in
`Main.cs` runs but no SDK type is ever loaded in the main process.

---

## Constraint 2: Subcommand Dispatch

`Application.Run()` dispatches on a `subcommand` string. Adding `snapshot-worker` and
`analysis-worker` requires:
- 2 new `else if (subcommand == "snapshot-worker")` / `"analysis-worker"` branches in `Application.Run()`
- 1 new CLI arg `--pipe <name>` parsed in `Main.cs` (for snapshot worker IPC)
- All other args (`--sdp`, `-s`, `-t`, `-o`) already exist and can be reused for analysis worker

Minimal changes. Existing subcommands are unaffected.

---

## Constraint 3: IPC Mechanism

**Snapshot worker — named pipe (bidirectional)**

`System.IO.Pipes.NamedPipeServerStream` and `NamedPipeClientStream` are fully available
in .NET 4.7.2. Main creates server side; child connects as client.

Pipe name format: `sdpcli-snap-{guid:N}` (e.g., `sdpcli-snap-a1b2c3d4...`).
Passed to child via `--pipe <name>` arg.

Message format: newline-delimited JSON objects (`StreamWriter` with `AutoFlush = true`
+ `StreamReader.ReadLine()`). Thread-safe via `object _pipeLock` in the proxy.

Commands (main → child):
```json
{"cmd": "connect", "deviceId": null}
{"cmd": "launch"}
{"cmd": "capture", "outputDir": "/abs/path"}
{"cmd": "disconnect"}
{"cmd": "ping"}
```

Events (child → main):
```json
{"event": "phase",            "phase": "connecting",  "progress": 40}
{"event": "connected",        "deviceId": "emulator-5554"}
{"event": "launched",         "sessionId": "sess-001"}
{"event": "capture_complete", "sdpPath": "/path/to.sdp", "captureId": 5}
{"event": "error",            "message": "..."}
{"event": "disconnected"}
{"event": "pong"}
```

**Analysis worker — stdout JSON lines (unidirectional)**

Child writes progress/result/error JSON lines to stdout; main reads them via
`Process.OutputDataReceived` event + `Process.BeginOutputReadLine()`.
This is the canonical .NET 4.7.2 async stdout pattern — no blocking.

Lines from child stdout:
```json
{"phase": "collect_dc",     "progress": 12}
{"phase": "extract_assets", "progress": 42}
{"phase": "label_drawcalls","progress": 65}
{"result": {"sdpPath": "...", "captureId": 5, "captureDir": "..."}}
{"error": "Cannot locate sdp.db"}
```

Exit code: 0 = success, 1 = failure. Main maps this to `JobStatus.Completed` / `Failed`.

Child invocation:
```
SDPCLI.exe analysis-worker --sdp <path> -s <captureId> -o <outputDir> -t <targets>
```
All args already exist in the parser. ✓

---

## Constraint 4: Single-Child Lock for Snapshot

`SnapshotWorkerManager` holds `_currentWorker` (nullable `SnapshotWorkerProxy`).

`POST /api/connect` flow:
```
if (_currentWorker != null && _currentWorker.IsAlive)
    return HTTP 409 { "ok": false, "error": "...", "confirm_required": true }

if (body.Force == true)
    _currentWorker.Kill()   // send disconnect cmd, wait 3s, then Process.Kill()

_currentWorker = SpawnWorker(pipeName, config)
// submit ConnectJob against the proxy
```

Frontend: when it receives `confirm_required: true` in the 409 body, shows a JS
`confirm()` dialog: _"A snapshot session is already active. Kill it and reconnect?"_
If confirmed, resubmits `POST /api/connect` with `{ "force": true }`.

---

## Constraint 5: Child Lifecycle

**Snapshot worker** — lives for the full device connection (not just one capture):
- Spawned on `POST /api/connect`
- Stays alive through `launch` + N captures + N archives
- Exits when: `POST /api/disconnect` → `{"cmd":"disconnect"}` → child exits 0
- Also exits if: main kills it (force-connect), or unexpected crash → main marks session Disconnected

Rationale: aligns with current UX (multi-capture sessions without reconnect).
If auto-close after each archive is desired later, it can be done with a session flag.

**Analysis worker** — lives for one analysis job:
- Spawned on `POST /api/analysis`
- Runs AnalysisPipeline phases, writes stdout progress
- Exits 0 on success / 1 on failure
- Multiple can run simultaneously (no lock)
- Cancelled via `Process.Kill()` (no cleanup needed — pure disk I/O, atomic per file)

---

## Constraint 6: JobManager Integration

`JobManager.Submit()` runs a `Func<Job, CancellationToken, Task>`. After Phase 3:

- **Snapshot jobs** (Connect/Launch/Capture): the runner sends a command to the pipe proxy
  and awaits an event response. The Task completes when the event arrives (or on error/timeout).
  The pipe proxy internally routes events to `TaskCompletionSource<T>` awaited by the runner.

- **Analysis jobs**: the runner calls `AnalysisWorkerProxy.RunAsync()` which spawns the child
  process and awaits process exit (via `Process.WaitForExitAsync()` wrapping via `TaskCompletionSource`).
  Progress updates from stdout feed into `job.Phase`/`job.Progress` directly.

`CancellationToken` propagation:
- Snapshot: `ct.Register(() => SendCommand({"cmd":"disconnect"}))` in the proxy
- Analysis: `ct.Register(() => process.Kill())` in the proxy

---

## Constraint 7: Config Availability in Children

Children are the same `SDPCLI.exe` in the same `bin/Debug/net472/` directory. `Application`
constructor loads config from `AppDomain.CurrentDomain.BaseDirectory + "config.ini"`. Since
the exe path is identical → config.ini discovery works identically. No `--config` arg needed. ✓

---

## Risk Summary

| Risk | Mitigation |
|------|------------|
| Named pipe race: child connects before server is ready | Server creates pipe first; child retries connect for 5s with 100ms intervals |
| Snapshot child crash during capture | Main detects `Process.Exited`; marks active job Failed; forces `Disconnected` state |
| Analysis stdout line lost on crash | Final line check: if process exits non-zero and no `result` line seen → mark Failed |
| TextureConverter.dll missing in analysis child | Covered by `SetDllDirectory` in Main.cs, same as today |
| spirv-cross.exe path in analysis child | Already resolved by `ShaderExtractor.SpirvCrossPath` from config, same exe dir ✓ |
| Named pipe name collision | UUID v4 pipe name per spawn — negligible collision probability |
| Windows pipe permissions | `NamedPipeServerStream` with `PipeAccessRights.ReadWrite` for current user is default — no admin required for localhost pipes |
