---
type: implementation
topic: snapshot/analysis decoupling — Phase 1 (ExitApplication suppression) + Phase 2 (device health monitor)
status: completed
based_on:
  - PLAN-2026-04-16-snapshot-analysis-decoupling.md
  - FINDING-2026-04-16-snapshot-analysis-coupling-topology.md
related_paths:
  - SDPCLI/source/ConsolePlatform.cs
  - SDPCLI/source/Server/DeviceSession.cs
  - SDPCLI/source/Server/Jobs/ConnectJobRunner.cs
  - SDPCLI/source/Modes/ServerMode.cs
summary: |
  Phase 1: ConsolePlatform now accepts an optional Action constructor parameter.
  ServerMode injects a custom action that logs the event and cancels the shutdown CTS
  instead of calling Environment.Exit(0). Eliminates the primary process-death root cause.
  Phase 2: DeviceSession now has a StartHealthMonitor()/StopHealthMonitor() background
  thread that polls Device.GetDeviceState() every 5s and proactively calls Disconnect()
  if the device enters Unknown or InstallFailed state, before the SDK fires a fatal event.
last_updated: 2026-04-16
---

## Plan Reference

PLAN-2026-04-16-snapshot-analysis-decoupling.md — Phase 1 + Phase 2B

## Implementation Summary

### Phase 1 — ExitApplication suppression

`ConsolePlatform` gains a constructor overload that accepts `Action onExitApplication`.
The default (no-arg) constructor chains to `() => Environment.Exit(0)`, preserving all
existing CLI-mode behaviour unchanged. `ExitApplication()` now calls `_onExitApplication()`.

`DeviceSession` gains an `internal Action? SdkExitAction` property. `EnsureSdkInitialized`
passes this action to `new ConsolePlatform(SdkExitAction)` when it is set, or falls back
to `new ConsolePlatform()` (default exit) when null.

`ServerMode.Run()` sets `session.SdkExitAction` immediately after creating `session` and
the `CancellationTokenSource cts`. The action logs the suppression and calls `cts.Cancel()`,
triggering a graceful server shutdown path (stop accepting, let jobs finish, disconnect).

### Phase 2 — Device health monitor

`DeviceSession` gains three new members:
- `private Thread? _healthMonitor` and `private volatile bool _healthMonitorRunning`
- `internal void StartHealthMonitor()` — starts the background thread (idempotent)
- `private void StopHealthMonitor()` — sets `_healthMonitorRunning = false`
- `private void HealthMonitorProc()` — polls `Device.GetDeviceState()` every 5s; calls
  `Disconnect()` if state is `Unknown` or `InstallFailed`

`Disconnect()` now calls `StopHealthMonitor()` as its first action to prevent the monitor
from re-entering disconnect during teardown.

`ConnectJobRunner.RunAsync()` calls `session.StartHealthMonitor()` after the connect
TryTransition succeeds, so the monitor is active for the lifetime of the device connection.

## Files Changed

| File | Change |
|------|--------|
| `SDPCLI/source/ConsolePlatform.cs` | Add `Action` constructor overload; `ExitApplication()` delegates to it |
| `SDPCLI/source/Server/DeviceSession.cs` | Add `SdkExitAction` property; update `EnsureSdkInitialized`; add health monitor fields + 3 methods; call `StopHealthMonitor()` in `Disconnect()` |
| `SDPCLI/source/Server/Jobs/ConnectJobRunner.cs` | Call `session.StartHealthMonitor()` after TryTransition to Connected |
| `SDPCLI/source/Modes/ServerMode.cs` | Set `session.SdkExitAction` after creating `cts` |

## Build / Validation

- Command: `dotnet build SDPCLI\SDPCLI.sln --configuration Debug`
- Result: **Build succeeded — 0 Warnings, 0 Errors**
- Iterations: 1

## Deviations from Plan

- Phase 2 implemented as Option B (health monitor) as recommended in the plan — no
  update thread pause/resume (Option A) was implemented since health monitor is sufficient
  and avoids breaking SDK connection assumptions.
- `SdkExitAction` is stored in `DeviceSession` (not passed through `ConnectJobRunner`
  signature), following Option A from the design discussion — no changes to `ConnectHandler`
  or its constructor needed.

## Issues Encountered

None. Build succeeded on first attempt.

## Next Steps

1. Manual integration test: connect device → run capture → kill adb → verify SDPCLI
   stays alive and logs "SDK requested ExitApplication — suppressed in server mode"
2. Run second analysis job after the above → verify it completes
3. Discuss Phase 3 (child process isolation) separately
