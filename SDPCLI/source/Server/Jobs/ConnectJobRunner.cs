using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using QGLPlugin;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Services.Capture;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    /// <summary>
    /// Executes the 5-phase device connection flow for server mode.
    /// Called by DeviceSession; transitions state machine on success/failure.
    /// </summary>
    public static class ConnectJobRunner
    {
        private static readonly ContextLogger _log = new ContextLogger("ConnectJob");

        public static async Task RunAsync(
            string? deviceId,
            Job job,
            DeviceSession session,
            Config config,
            CancellationToken ct)
        {
            try
            {
                // ── Phase 1: initializing_sdk ─────────────────────────────────
                job.Phase = "initializing_sdk"; job.Progress = 5;
                if (!session.EnsureSdkInitialized(config))
                    throw new InvalidOperationException("SDK initialization failed");

                string sdpOutputDir = ResolveSdpOutputDir(config);
                var sessionSettings = new SessionSettings
                {
                    SessionDirectoryRootPath      = sdpOutputDir,
                    MaxTotalSessionsSizeMB        = 0,
                    CreateTimestampedSubDirectory = true
                };

                var sdpClient  = new SDPClient();
                var cliDelegate = new CliClientDelegate();
                cliDelegate.SetCaptureCompleteEvent(session.CaptureCompleteEvent);
                cliDelegate.SetDataProcessedEvent(session.DataProcessedEvent);
                cliDelegate.SetImportCompleteEvent(session.ImportCompleteEvent);

                string logLevelStr = config.Get("LogLevel", "DEBUG").ToUpper();
                LogLevel logLevel = logLevelStr == "INFO"  ? LogLevel.LOG_INFO
                                  : logLevelStr == "WARN"  ? LogLevel.LOG_WARN
                                  : logLevelStr == "ERROR" ? LogLevel.LOG_ERROR
                                  : logLevelStr == "OFF"   ? LogLevel.LOG_OFF
                                  : LogLevel.LOG_DEBUG;

                if (!sdpClient.Initialize(sessionSettings, cliDelegate, enableConsoleLog: true, logLevel: logLevel))
                    throw new InvalidOperationException("SDPClient.Initialize() failed");

                session.SdpClient      = sdpClient;
                session.ClientDelegate = cliDelegate;
                _log.Info("SDPClient initialized");

                // ── Phase 2: finding_device ───────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "finding_device"; job.Progress = 20;

                var deviceSvcFinder = new DeviceConnectionService(config, _ => null);
                if (!deviceSvcFinder.CheckAndInstallAPKs())
                    throw new InvalidOperationException("APK installation check failed");

                // ── Phase 3: connecting ───────────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "connecting"; job.Progress = 40;
                _log.Info("Connecting to device...");

                Device? device = await Task.Run(() =>
                {
                    var svc = new DeviceConnectionService(config, _ => null);
                    return svc.Connect(sdpClient);
                }, ct).ConfigureAwait(false);

                if (device == null)
                    throw new InvalidOperationException("Failed to connect to device");

                session.Device            = device;
                session.ConnectedDeviceId = !string.IsNullOrEmpty(deviceId) ? deviceId
                                                                              : device.GetName() ?? "unknown";
                sdpClient.SetCaptureCompleteEvent(session.CaptureCompleteEvent);
                _log.Info($"Connected: {session.ConnectedDeviceId}");

                // ── Phase 4: verifying ────────────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "verifying"; job.Progress = 90;
                await Task.Delay(500, ct).ConfigureAwait(false);   // brief stabilize

                // ── Done ──────────────────────────────────────────────────────
                job.Result   = new { deviceId = session.ConnectedDeviceId, status = "Connected" };
                job.Status   = JobStatus.Completed;
                job.Progress = 100;
                session.TryTransition(DeviceStatus.Connecting, DeviceStatus.Connected);
                session.StartHealthMonitor();
                _log.Info("Connect job completed");
            }
            catch (OperationCanceledException)
            {
                TeardownOnFailure(session);
                session.TryTransition(DeviceStatus.Connecting, DeviceStatus.Disconnected);
                throw;
            }
            catch (Exception ex)
            {
                _log.Error("Connect failed: " + ex.Message);
                TeardownOnFailure(session);
                session.TryTransition(DeviceStatus.Connecting, DeviceStatus.Disconnected);
                throw;
            }
        }

        private static void TeardownOnFailure(DeviceSession session)
        {
            try { session.SdpClient?.Shutdown(); session.SdpClient?.Dispose(); }
            catch { /* ignore */ }
            session.SdpClient      = null;
            session.ClientDelegate = null;
            session.Device         = null;
        }

        private static string ResolveSdpOutputDir(Config config)
        {
            string workDir    = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
            string projectRel = config.Get("ProjectDir", "project");
            string projectDir = System.IO.Path.IsPathRooted(projectRel)
                ? projectRel
                : System.IO.Path.GetFullPath(System.IO.Path.Combine(workDir, projectRel));
            string sdpDirRel  = config.Get("SdpDir", "sdp");
            string sdpDir     = System.IO.Path.IsPathRooted(sdpDirRel)
                ? sdpDirRel
                : System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDir, sdpDirRel));
            Directory.CreateDirectory(sdpDir);
            return sdpDir;
        }
    }
}
