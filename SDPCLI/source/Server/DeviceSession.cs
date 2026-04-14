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

                var platform = new ConsolePlatform();
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
            _log.Info("Disconnecting...");

            // Cancel active job if any
            try { ActiveJob?.Cts.Cancel(); }
            catch { /* ignore */ }
            ActiveJob = null;

            // Tear down SDK session
            try
            {
                SdpClient?.SessionManager?.CloseSession();
            }
            catch (Exception ex) { _log.Warning("CloseSession failed: " + ex.Message); }

            try
            {
                if (SdpClient != null) { SdpClient.Shutdown(); SdpClient.Dispose(); SdpClient = null; }
            }
            catch (Exception ex) { _log.Warning("SDPClient shutdown failed: " + ex.Message); }

            // Reset shared state
            ClientDelegate   = null;
            Device           = null;
            CurrentCapture   = null;
            ConnectedDeviceId = null;
            CurrentSession   = null;
            TargetPackageName = null;
            TargetProcessPid  = 0;
            VerifiedProcessPid = 0;
            RenderingAPI      = 0;

            // SDK init stays (process-level singleton, can reconnect)
            _status = DeviceStatus.Disconnected;
            _log.Info("Disconnected");
        }
    }
}
