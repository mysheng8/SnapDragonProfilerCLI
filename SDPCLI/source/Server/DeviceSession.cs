using System;
using System.Threading;
using QGLPlugin;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Services.Capture;

namespace SnapdragonProfilerCLI.Server
{
    /// <summary>
    /// Singleton device connection state for server mode.
    /// All state transitions are atomic via TryTransition().
    /// Job runners call TryTransition to move the machine and set it back in finally blocks.
    /// </summary>
    public class DeviceSession
    {
        private static readonly ContextLogger _log = new ContextLogger("DeviceSession");

        // ── State machine ─────────────────────────────────────────────────────
        private volatile DeviceStatus _status = DeviceStatus.Disconnected;
        private readonly object _transitionLock = new object();
        private Thread? _healthMonitor;
        private ManualResetEventSlim? _healthMonitorStop;
        private volatile bool _isDisconnecting = false;

        public DeviceStatus Status => _status;

        /// <summary>Returns a JSON-serialisable snapshot of the current device state.</summary>
        public object GetInfo() => new
        {
            status          = _status.ToString(),
            connectedDevice = ConnectedDeviceId,
            session         = CurrentSession,
        };

        public bool TryTransition(DeviceStatus expected, DeviceStatus next)
        {
            lock (_transitionLock)
            {
                if (_status != expected) return false;
                _status = next;
                return true;
            }
        }

        // ── Public state ──────────────────────────────────────────────────────
        public string?           ConnectedDeviceId { get; internal set; }
        public DeviceSessionInfo? CurrentSession   { get; internal set; }

        // SDK exit override — set by ServerMode to suppress Environment.Exit(0)
        internal Action?         SdkExitAction     { get; set; }

        // ── SDK references (internal — job runners only) ──────────────────────
        internal SDPClient?         SdpClient        { get; set; }
        internal CliClientDelegate? ClientDelegate   { get; set; }
        internal Device?            Device           { get; set; }
        internal global::Capture?   CurrentCapture   { get; set; }
        internal static bool        SdkInitialized   { get; private set; }  // process-scoped singleton

        // Capture-specific state
        internal string? TargetPackageName   { get; set; }
        internal uint    TargetProcessPid    { get; set; }
        internal uint    VerifiedProcessPid  { get; set; }
        internal uint    RenderingAPI        { get; set; }

        // ── Event handles (reused across captures in a session) ───────────────
        internal readonly ManualResetEvent CaptureCompleteEvent = new ManualResetEvent(false);
        internal readonly ManualResetEvent DataProcessedEvent   = new ManualResetEvent(false);
        internal readonly ManualResetEvent ImportCompleteEvent  = new ManualResetEvent(false);

        // Reference to the currently active capture or launch job (for cancellation via disconnect)
        internal Jobs.Job? ActiveJob { get; set; }

        // ── SDK init (called once per process) ───────────────────────────────
        internal bool EnsureSdkInitialized(Config config)
        {
            if (SdkInitialized) return true;
            try
            {
                _log.Info("Initializing SdpApp...");
                try { Sdp.Helpers.Globalization.SetLocale(); }
                catch (Exception ex) { _log.Warning("Globalization.SetLocale failed: " + ex.Message); }

                var platform = new ConsolePlatform(SdkExitAction);
                if (!Sdp.SdpApp.Init(platform))
                {
                    _log.Error("SdpApp.Init() returned false");
                    return false;
                }
                _log.Info("SdpApp initialized");

                try { new QGLPlugin.QGLPlugin(); _log.Info("QGLPlugin instantiated"); }
                catch (Exception ex) { _log.Warning("QGLPlugin failed: " + ex.Message); }

                SdkInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("SDK init failed: " + ex.Message);
                return false;
            }
        }

        // ── Disconnect (synchronous) ──────────────────────────────────────────
        public void Disconnect()
        {
            // Re-entrancy guard: SDK Shutdown() may fire ExitApplication → SdkExitAction → Disconnect()
            if (_isDisconnecting) return;
            _isDisconnecting = true;
            try { _disconnectCore(); }
            finally { _isDisconnecting = false; }
        }

        private void _disconnectCore()
        {
            _log.Info("Disconnecting...");

            StopHealthMonitor();

            // Cancel active job if any
            try { ActiveJob?.Cts.Cancel(); }
            catch { /* ignore */ }
            ActiveJob = null;

            // Snapshot and null out SDK references before calling Shutdown so any
            // re-entrant ExitApplication callback sees nothing to tear down.
            var client = SdpClient;
            SdpClient      = null;
            ClientDelegate = null;
            Device         = null;
            CurrentCapture = null;

            // Tear down SDK session
            try { client?.SessionManager?.CloseSession(); }
            catch (Exception ex) { _log.Warning("CloseSession failed: " + ex.Message); }

            try
            {
                if (client != null) { client.Shutdown(); client.Dispose(); }
            }
            catch (Exception ex) { _log.Warning("SDPClient shutdown failed: " + ex.Message); }

            // Reset remaining shared state
            ConnectedDeviceId  = null;
            CurrentSession     = null;
            TargetPackageName  = null;
            TargetProcessPid   = 0;
            VerifiedProcessPid = 0;
            RenderingAPI       = 0;

            // SDK init stays (process-level singleton, can reconnect)
            _status = DeviceStatus.Disconnected;
            _log.Info("Disconnected");
        }

        // ── Device health monitor ─────────────────────────────────────────────
        internal void StartHealthMonitor()
        {
            lock (_transitionLock)
            {
                if (_healthMonitor != null && _healthMonitor.IsAlive) return;
                _healthMonitorStop = new ManualResetEventSlim(false);
                _healthMonitor = new Thread(HealthMonitorProc)
                {
                    IsBackground = true,
                    Name         = "DeviceHealthMonitor",
                };
                _healthMonitor.Start();
            }
            _log.Info("Device health monitor started");
        }

        private void StopHealthMonitor()
        {
            ManualResetEventSlim? stop;
            lock (_transitionLock)
            {
                stop = _healthMonitorStop;
                _healthMonitorStop = null;
                _healthMonitor = null;
            }
            stop?.Set();
        }

        private void HealthMonitorProc()
        {
            var stop = _healthMonitorStop;
            while (stop != null && !stop.IsSet && _status != DeviceStatus.Disconnected)
            {
                if (stop.Wait(5000)) break;
                if (stop.IsSet || _status == DeviceStatus.Disconnected) break;
                try
                {
                    if (Device == null) break;
                    var state = Device.GetDeviceState();
                    if (state == DeviceConnectionState.Unknown ||
                        state == DeviceConnectionState.InstallFailed)
                    {
                        // Skip disconnect while a capture is in flight — transient Unknown is normal during SDK replay
                        if (_status == DeviceStatus.Capturing)
                        {
                            _log.Warning($"Health monitor: device state={state} but status=Capturing — deferring disconnect");
                            continue;
                        }
                        _log.Warning($"Health monitor: device state={state} — initiating proactive disconnect");
                        Disconnect();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _log.Warning($"Health monitor: GetDeviceState() threw: {ex.Message}");
                    break;
                }
            }
            _log.Info("Device health monitor stopped");
        }
    }
}
