using System;
using System.Threading;
using System.Threading.Tasks;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Services.Capture;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    /// <summary>
    /// Executes the 5-phase app launch flow for server mode.
    /// Transitions DeviceStatus: Launching → SessionActive (or → Connected on failure).
    /// </summary>
    public static class LaunchJobRunner
    {
        private static readonly ContextLogger _log = new ContextLogger("LaunchJob");

        public static async Task RunAsync(
            string? packageActivity,
            Job job,
            DeviceSession session,
            Config config,
            CancellationToken ct)
        {
            try
            {
                if (session.SdpClient == null || session.Device == null || session.ClientDelegate == null)
                    throw new InvalidOperationException("Device not connected");

                // Resolve packageActivity from body or config
                string pkgName;
                string? activityName;
                if (!string.IsNullOrWhiteSpace(packageActivity))
                {
                    char sep = packageActivity!.IndexOf('\\') >= 0 ? '\\' : '/';
                    var parts = packageActivity.Split(new[] { sep }, 2);
                    pkgName      = parts[0].Trim();
                    activityName = parts.Length == 2 ? parts[1].Trim() : null;
                }
                else
                {
                    pkgName      = config.Get("PackageName");
                    activityName = config.Get("ActivityName");
                }

                if (string.IsNullOrWhiteSpace(pkgName))
                    throw new InvalidOperationException("packageActivity required (no PackageName in config)");

                // Override config for AppLaunchService to pick up the activity
                if (activityName != null)
                    config.Set("ActivityName", activityName);
                config.Set("PackageName", pkgName);

                // ── Phase 1: checking_package ─────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "checking_package"; job.Progress = 10;
                _log.Info($"Launching: {pkgName}");

                // ── Phase 2-4: launch + wait ──────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "launching_app"; job.Progress = 40;

                var appSvc = new AppLaunchService(
                    config,
                    _ => null,   // no interactive readline in server mode
                    session.SdpClient,
                    session.Device,
                    session.ClientDelegate);

                bool launched = await Task.Run(() => appSvc.SelectAndLaunch(pkgName), ct)
                                          .ConfigureAwait(false);

                if (!launched)
                    throw new InvalidOperationException("App launch failed");

                session.TargetPackageName = appSvc.TargetPackageName;
                session.TargetProcessPid  = appSvc.TargetProcessPid;
                session.RenderingAPI      = appSvc.RenderingAPI;
                (session.ClientDelegate as CliClientDelegate)?.SetTargetPackageName(
                    session.TargetPackageName ?? "");

                // ── Phase 5: waiting_process ──────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "waiting_process"; job.Progress = 65;

                bool found = await Task.Run(() => appSvc.WaitForProcess(), ct).ConfigureAwait(false);
                session.VerifiedProcessPid = appSvc.VerifiedProcessPid;

                if (!found)
                    _log.Warning("Process not found within timeout — proceeding anyway");

                // ── Done ──────────────────────────────────────────────────────
                string resolvedPkg = pkgName + (activityName != null ? "/" + activityName : "");
                session.CurrentSession = new DeviceSessionInfo(resolvedPkg, session.VerifiedProcessPid);

                job.Result = new
                {
                    sessionId       = session.CurrentSession.SessionId,
                    packageActivity = resolvedPkg,
                    pid             = session.VerifiedProcessPid
                };
                job.Status   = JobStatus.Completed;
                job.Progress = 100;
                session.TryTransition(DeviceStatus.Launching, DeviceStatus.SessionActive);
                _log.Info("Launch job completed: " + resolvedPkg);

                // Auto-close session when the target process disappears (app killed / crashed)
                if (session.ClientDelegate is CliClientDelegate csd)
                {
                    Action<uint>? handler = null;
                    handler = pid =>
                    {
                        csd.TargetProcessRemoved -= handler;   // self-unregister, fires only once
                        if (session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Connected))
                        {
                            _log.Info($"Target process PID={pid} removed — session auto-closed");
                            session.CurrentSession    = null;
                            session.TargetPackageName = null;
                            session.TargetProcessPid  = 0;
                            session.VerifiedProcessPid = 0;
                        }
                    };
                    csd.TargetProcessRemoved += handler;
                }
            }
            catch (OperationCanceledException)
            {
                session.TryTransition(DeviceStatus.Launching, DeviceStatus.Connected);
                throw;
            }
            catch (Exception ex)
            {
                _log.Error("Launch failed: " + ex.Message);
                session.TryTransition(DeviceStatus.Launching, DeviceStatus.Connected);
                throw;
            }
        }
    }
}
