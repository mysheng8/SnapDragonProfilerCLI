using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Services.Capture;
using SnapdragonProfilerCLI.Tools;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Snapshot Capture mode — connects to device, launches app, and captures a single GPU frame.
    /// All capture logic (previously Application.RunCaptureMode + InitializeClient) now lives here.
    /// </summary>
    public class SnapshotCaptureMode : IMode
    {
        // Dependencies injected from Application
        private readonly Config _config;
        private readonly string _testPath;
        private readonly string _sdpOutputDir;  // resolved SDP output root (where .sdp files land)
        private readonly Func<string?, string?> _readLine;
        private readonly string? _customSdpOutputPath;

        // Capture-session state
        private SDPClient? _sdpClient;
        private ClientDelegate? _clientDelegate;
        private Device? _connectedDevice;
        private Capture? _currentCapture;
        private bool _isCleanedUp;
        private string? _lastSessionPath;
        private string? _targetPackageName;
        private uint _targetProcessPid;
        private uint _verifiedProcessPid;
        private uint _renderingAPI;

        // Synchronisation events
        private readonly ManualResetEvent _captureCompleteEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _dataProcessedEvent   = new ManualResetEvent(false);
        private readonly ManualResetEvent _importCompleteEvent  = new ManualResetEvent(false);

        // Session summary tracking
        private readonly List<SessionSummaryService.CaptureEntry> _captureEntries = new();
        private List<string> _activatedMetricNames = new();

        private static readonly ContextLogger _log = new ContextLogger("Capture");

        public string Name        => "Snapshot Capture";
        public string Description => "Connect device and capture single frame as .sdp file";

        public SnapshotCaptureMode(
            Config config,
            string testPath,
            Func<string?, string?> readLine,
            string? sdpPath         = null,     // legacy (deprecated)
            string? packageActivity = null,     // new: "pkg\activity" positional arg
            string? outputArg       = null)     // -output/-o: snapshot output dir
        {
            _config              = config;
            _testPath            = testPath;
            _readLine            = readLine;
            _customSdpOutputPath = ResolveOutputPath(sdpPath, testPath);
            _sdpOutputDir        = ResolveSdpOutputDir(outputArg, config, testPath);

            // Parse package\activity positional arg and override config if provided
            if (!string.IsNullOrWhiteSpace(packageActivity))
            {
                // Accept both '\' and '/' separator
                char sep = packageActivity!.IndexOf('\\') >= 0 ? '\\' : '/';
                var parts = packageActivity.Split(new[] { sep }, 2);
                if (parts.Length == 2)
                {
                    _config.Set("PackageName",  parts[0].Trim());
                    _config.Set("ActivityName", parts[1].Trim());
                }
                else
                {
                    // No separator: treat entire string as package name
                    _config.Set("PackageName", packageActivity.Trim());
                }
            }

            // (package\activity parsed; no non-interactive mode flags)
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("\n=== Capture Mode ===\n");
                if (_customSdpOutputPath != null)
                    Console.WriteLine("Custom output path: " + _customSdpOutputPath);

                Utility.ConfigureNativeDllSearchPaths();

                if (!InitializeClient())
                {
                    Console.WriteLine("Failed to initialize client. Exiting...");
                    return;
                }

                var deviceService = new DeviceConnectionService(_config, _readLine);
                if (!deviceService.CheckAndInstallAPKs())
                {
                    Console.WriteLine("Failed to install Profiler APKs. Exiting...");
                    Cleanup();
                    return;
                }

                _connectedDevice = deviceService.Connect(_sdpClient!);
                if (_connectedDevice == null)
                {
                    Console.WriteLine("Failed to connect to device. Exiting...");
                    Cleanup();
                    return;
                }

                Console.WriteLine("\n--- Launch Application ---");
                string? packageName = _config.Get("PackageName");
                if (string.IsNullOrWhiteSpace(packageName))
                {
                    Console.Write("Enter app package name: ");
                    packageName = _readLine("");
                }
                else { Console.WriteLine("Using package from config: " + packageName); }

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    Console.WriteLine("No package name provided. Exiting...");
                    Cleanup();
                    return;
                }

                var appService = new AppLaunchService(_config, _readLine, _sdpClient!, _connectedDevice!, _clientDelegate!);
                if (!appService.SelectAndLaunch(packageName!))
                {
                    Console.WriteLine("Failed to launch app. Exiting...");
                    Cleanup();
                    return;
                }
                _targetPackageName = appService.TargetPackageName;
                _targetProcessPid  = appService.TargetProcessPid;
                _renderingAPI      = appService.RenderingAPI;
                ((CliClientDelegate?)_clientDelegate)?.SetTargetPackageName(_targetPackageName ?? "");

                bool initialDiscovery = appService.WaitForProcess();
                _verifiedProcessPid  = appService.VerifiedProcessPid;

                if (!initialDiscovery) { Console.WriteLine("\n WARNING: Initial process discovery timed out."); }
                else { Console.WriteLine("\n Process verified (PID: " + _verifiedProcessPid + ")"); }

                if (_verifiedProcessPid > 0)
                    RefreshPidIfNeeded();

                if (_clientDelegate is CliClientDelegate sd0)
                {
                    Console.WriteLine("\nProcess discovery status:");
                    Console.WriteLine("  - Total processes discovered: " + sd0.GetDiscoveredProcessCount());
                    Console.WriteLine(_verifiedProcessPid > 0
                        ? "  - Target process: VERIFIED (PID: " + _verifiedProcessPid + ")"
                        : "  - Target process: UNVERIFIED");
                }

                bool autoStart = _config.GetBool("AutoStartCapture", false);
                uint providerId = 0;
                uint captureId  = 0;

                Console.WriteLine("\n=== Ready to Capture ===");
                Console.WriteLine("Press ENTER to capture a frame, or ESC to exit");

                while (true)
                {
                    if (!autoStart)
                    {
                        Console.WriteLine("\n--- Waiting for input ---");
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape) { Console.WriteLine("ESC pressed - exiting..."); break; }
                        if (key.Key != ConsoleKey.Enter) { Console.WriteLine("Press ENTER or ESC"); continue; }
                        Console.WriteLine("ENTER - starting capture...");
                    }
                    else { Console.WriteLine("Auto-capturing frame..."); autoStart = false; }

                    try
                    {
                        // Reset events before starting so stale signals from the previous
                        // capture don't cause WaitOne() to return immediately with the old captureId.
                        _captureCompleteEvent.Reset();
                        _importCompleteEvent.Reset();
                        ((CliClientDelegate?)_clientDelegate)?.ResetDataProcessedCount();

                        var capSvc = new CaptureExecutionService(
                            _sdpClient!, _connectedDevice, _currentCapture, _config,
                            _targetPackageName, _targetProcessPid, _verifiedProcessPid,
                            _renderingAPI, _clientDelegate);

                        if (!capSvc.StartCapture())
                        {
                            Console.WriteLine("Failed to start capture. Press ENTER to retry or ESC to exit.");
                            continue;
                        }
                        _currentCapture = capSvc.CurrentCapture;
                        // Capture metric names once (same set for all captures in a session)
                        if (_activatedMetricNames.Count == 0 && capSvc.ActivatedMetricNames.Count > 0)
                            _activatedMetricNames = capSvc.ActivatedMetricNames;

                        Console.WriteLine("\nCapture in progress...");
                        bool completed = _captureCompleteEvent.WaitOne(TimeSpan.FromSeconds(30));
                        if (!completed)
                        {
                            Console.WriteLine(" WARNING: Capture did not complete within 30 seconds. Press ENTER to retry or ESC to exit.");
                            continue;
                        }
                        (providerId, captureId) = ((CliClientDelegate)_clientDelegate!).GetLastCompletedCapture();
                        Console.WriteLine("Capture completed: Provider=" + providerId + ", Capture=" + captureId);

                        // Tell delegate which captureId's API data (BufferID=2) to wait for
                        ((CliClientDelegate)_clientDelegate!).SetExpectedCaptureId(captureId);

                        WaitForDataProcessed();

                        string? sessionPath = _sdpClient?.SessionManager?.GetSessionPath();

                        // captureId comes directly from OnCaptureComplete callback — trust it
                        string baseDir = sessionPath ?? _testPath;
                        string captureSubDir = Path.Combine(baseDir, $"snapshot_{captureId}");
                        Directory.CreateDirectory(captureSubDir);
                        _log.Info($"Capture sub-directory created: {captureSubDir}");
                        Console.WriteLine($"Capture sub-directory: {captureSubDir}");

                        _log.Info($"[Replay] Starting ReplayAndGetBuffers for captureId={captureId}");
                        BinaryDataPair? dsbBuffer = ReplayAndGetBuffers(captureId, _importCompleteEvent);
                        _log.Info($"[Replay] ReplayAndGetBuffers returned: {(dsbBuffer != null ? dsbBuffer.size + " bytes" : "null")}");

                        if (dsbBuffer != null)
                        {
                            _log.Info($"[Replay] Starting ExportDrawCallData for captureId={captureId}");
                            ExportDrawCallData(captureId, dsbBuffer, sessionPath ?? _testPath, captureSubDir);
                            _log.Info($"[Replay] ExportDrawCallData completed");
                        }
                        else
                        {
                            _log.Warning($"[Replay] dsbBuffer is null — skipping ExportDrawCallData");
                        }

                        _log.Info($"[Export] Starting DataExportService.ExportData for captureId={captureId}");
                        var exportSvc = new DataExportService(_sdpClient!, _currentCapture);
                        exportSvc.ExportData(sessionPath ?? _testPath, captureSubDir);
                        _log.Info($"[Export] DataExportService.ExportData completed");
                        _lastSessionPath = sessionPath;

                        // 置 null 确保下次 ENTER 创建新的 Capture 对象，SDK 分配新 captureId
                        _currentCapture = null;

                        _captureEntries.Add(new SessionSummaryService.CaptureEntry(captureId, captureSubDir));

                        // Wait for ImportCapture's device-side replay to finish (bufferID=1).
                        // This prevents the next capture's metrics from being collected while
                        // the replay is still running on device (which causes empty DrawCallMetrics).
                        Console.WriteLine("\nWaiting for device replay to complete (safe to capture after this)...");
                        _log.Info($"[Capture] Waiting for ImportCapture replay completion (bufferID=1) — timeout=60s");
                        bool importDone = _importCompleteEvent.WaitOne(TimeSpan.FromSeconds(60));
                        if (!importDone)
                            _log.Warning("[Capture] ImportCapture trailing event not received within 60s — proceeding anyway");
                        else
                            _log.Info("[Capture] ImportCapture device replay confirmed complete");

                        _log.Info($"[Capture] === Capture Complete === data saved to: {captureSubDir}");
                        Console.WriteLine("\n=== Capture Complete ===");
                        Console.WriteLine("Data saved to: " + captureSubDir);

                        Console.WriteLine("Press ENTER to capture another frame, or ESC to exit");
                    }
                    catch (Exception ex)
                    {
                        _log.Error($"[Capture] Exception in capture loop: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                        Console.WriteLine("\nError during capture: " + ex.Message);
                        Console.WriteLine("Stack trace: " + ex.StackTrace);
                        Console.WriteLine("\nPress ENTER to retry or ESC to exit");
                    }
                }

                Console.WriteLine("\n=== Exiting Capture Loop ===");
            }
            finally
            {
                Console.WriteLine("\n=== Cleanup ===");
                // 整个 session 结束时才打包一次 .sdp（包含所有 capture 数据）
                string? finalSessionPath = _sdpClient?.SessionManager?.GetSessionPath();
                if (finalSessionPath != null)
                {
                    // Write session_summary.json before archiving
                    try
                    {
                        string renderingApiStr = _renderingAPI switch
                        {
                            16 => "Vulkan",
                            8  => "OpenGL",
                            2  => "DirectX12",
                            1  => "DirectX11",
                            4  => "OpenCL",
                            _  => "Unknown",
                        };
                        string appPackage  = _config.Get("PackageName",    "") ?? "";
                        string appActivity = _config.Get("ActivityName",   "") ?? "";
                        new SessionSummaryService().Write(
                            finalSessionPath,
                            _connectedDevice,
                            _config,
                            renderingApiStr,
                            appPackage,
                            appActivity,
                            _activatedMetricNames,
                            _captureEntries);
                    }
                    catch (Exception sumEx) { Console.WriteLine(" Warning: Could not write session summary: " + sumEx.Message); }

                    try { new SessionArchiveService().CreateSessionArchive(finalSessionPath); }
                    catch (Exception archEx) { Console.WriteLine(" Warning: Could not create session archive: " + archEx.Message); }
                }
                try
                {
                    bool closed = _sdpClient?.SessionManager?.CloseSession() ?? false;
                    if (closed) Console.WriteLine("Session closed");
                }
                catch (Exception ex) { Console.WriteLine(" Warning: Error closing session: " + ex.Message); }
                Cleanup();
            }
        }

        private static string? ResolveOutputPath(string? sdpPath, string testPath)
        {
            if (string.IsNullOrEmpty(sdpPath)) return null;
            string resolved = Path.IsPathRooted(sdpPath) ? sdpPath! : Path.GetFullPath(Path.Combine(testPath, sdpPath!));
            if (!resolved.EndsWith(".sdp", StringComparison.OrdinalIgnoreCase)) resolved += ".sdp";
            string? parent = Path.GetDirectoryName(resolved);
            if (!string.IsNullOrEmpty(parent) && !Directory.Exists(parent)) Directory.CreateDirectory(parent);
            return resolved;
        }

        /// <summary>
        /// Resolve the SDP output directory for snapshot sessions.
        /// Default = SdpDir (ProjectDir/sdp).
        /// If -output/-o is given:
        ///   absolute  → use directly (mkdir)
        ///   relative  → 1) SdpDir/arg  2) ProjectDir/arg  → create if not found
        /// </summary>
        private static string ResolveSdpOutputDir(string? outputArg, Config config, string testPathFallback)
        {
            string workDir     = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
            string projectRel  = config.Get("ProjectDir", "project");
            string projectDir  = Path.IsPathRooted(projectRel)
                ? projectRel
                : Path.GetFullPath(Path.Combine(workDir, projectRel));
            string sdpDirRel   = config.Get("SdpDir", "sdp");
            string sdpDir      = Path.IsPathRooted(sdpDirRel)
                ? sdpDirRel
                : Path.GetFullPath(Path.Combine(projectDir, sdpDirRel));

            if (string.IsNullOrWhiteSpace(outputArg))
            {
                // Default: SdpDir
                Directory.CreateDirectory(sdpDir);
                return sdpDir;
            }

            if (Path.IsPathRooted(outputArg))
            {
                Directory.CreateDirectory(outputArg!);
                return outputArg!;
            }

            // Relative: try SdpDir first, then ProjectDir
            string attempt1 = Path.GetFullPath(Path.Combine(sdpDir, outputArg!));
            if (Directory.Exists(attempt1)) return attempt1;
            string attempt2 = Path.GetFullPath(Path.Combine(projectDir, outputArg!));
            if (Directory.Exists(attempt2)) return attempt2;

            // Not found → create under SdpDir
            Directory.CreateDirectory(attempt1);
            return attempt1;
        }

        private bool InitializeClient()
        {
            Console.WriteLine("Initializing Snapdragon Profiler Client...");
            try
            {
                Console.WriteLine("Initializing SdpApp event system...");
                try { Sdp.Helpers.Globalization.SetLocale(); Console.WriteLine("Globalization initialized"); }
                catch (Exception ex) { Console.WriteLine(" Globalization.SetLocale() failed: " + ex.Message); }

                var platform = new ConsolePlatform();
                try
                {
                    if (!Sdp.SdpApp.Init(platform)) { Console.WriteLine("ERROR: SdpApp.Init() returned false"); return false; }
                }
                catch (Exception ex) { Console.WriteLine("ERROR: SdpApp.Init() threw: " + ex.Message); return false; }
                Console.WriteLine("SdpApp initialized\n");

                try { new QGLPlugin.QGLPlugin(); Console.WriteLine("QGLPlugin instantiated\n"); }
                catch (Exception ex) { Console.WriteLine(" QGLPlugin failed: " + ex.Message + "\n"); }

                _sdpClient = new SDPClient();

                var sessionSettings = new SessionSettings
                {
                    SessionDirectoryRootPath      = _sdpOutputDir,
                    MaxTotalSessionsSizeMB        = 1024,
                    CreateTimestampedSubDirectory = true
                };

                var simpleDelegate = new CliClientDelegate();
                simpleDelegate.SetCaptureCompleteEvent(_captureCompleteEvent);
                simpleDelegate.SetDataProcessedEvent(_dataProcessedEvent);
                simpleDelegate.SetImportCompleteEvent(_importCompleteEvent);

                string logLevelStr = _config.Get("LogLevel", "DEBUG").ToUpper();
                LogLevel logLevel = logLevelStr == "INFO"  ? LogLevel.LOG_INFO
                                  : logLevelStr == "WARN"  ? LogLevel.LOG_WARN
                                  : logLevelStr == "ERROR" ? LogLevel.LOG_ERROR
                                  : logLevelStr == "OFF"   ? LogLevel.LOG_OFF
                                  : LogLevel.LOG_DEBUG;

                bool ok = _sdpClient.Initialize(sessionSettings, simpleDelegate, enableConsoleLog: true, logLevel: logLevel);
                if (!ok) { Console.WriteLine("ERROR: SDPClient.Initialize() failed"); return false; }

                _sdpClient.SetCaptureCompleteEvent(_captureCompleteEvent);
                _clientDelegate = simpleDelegate;
                Console.WriteLine("Client initialized successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during initialization: " + ex.Message);
                if (ex.InnerException != null) Console.WriteLine("  Inner: " + ex.InnerException.Message);
                return false;
            }
        }

        private void Cleanup()
        {
            if (_isCleanedUp) return;
            _isCleanedUp = true;
            Console.WriteLine("\nCleaning up...");
            try
            {
                if (_sdpClient != null) { _sdpClient.Shutdown(); _sdpClient.Dispose(); _sdpClient = null; }
                _connectedDevice = null; _currentCapture = null; _clientDelegate = null;
                Console.WriteLine("Cleanup completed");
            }
            catch (Exception ex) { Console.WriteLine("Error during cleanup: " + ex.Message); }
        }

        private void RefreshPidIfNeeded()
        {
            try
            {
                var p = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo
                    { FileName = "adb", Arguments = "shell ps -p " + _verifiedProcessPid + " -o NAME",
                      RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true }};
                p.Start(); string output = p.StandardOutput.ReadToEnd(); p.WaitForExit();
                if (!output.Contains(_targetPackageName))
                {
                    Console.WriteLine("\nPID " + _verifiedProcessPid + " no longer valid - trying pidof...");
                    var p2 = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo
                        { FileName = "adb", Arguments = "shell pidof " + _targetPackageName,
                          RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true }};
                    p2.Start(); string pidOut = p2.StandardOutput.ReadToEnd().Trim(); p2.WaitForExit();
                    if (uint.TryParse(pidOut, out uint newPid)) { _verifiedProcessPid = newPid; Console.WriteLine("  Found new PID: " + newPid); }
                    else { Console.WriteLine("  App not running. Aborting."); Cleanup(); throw new InvalidOperationException("App process not found."); }
                }
                else { Console.WriteLine("PID " + _verifiedProcessPid + " still valid"); }
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex) { Console.WriteLine(" Could not verify PID: " + ex.Message); }
        }

        private void WaitForDataProcessed()
        {
            // Wait until the expected capture's API data (BufferID=2) arrives.
            // The event is set exclusively by OnDataProcessed when category=0x84000000,
            // bufferID=2, captureID=expected — so no polling needed.
            Console.WriteLine("\nWaiting for snapshot API data (BufferID=2)...");
            _log.Info("[WaitForData] Waiting for snapshot API data (BufferID=2) — timeout=180s");
            bool ready = _dataProcessedEvent.WaitOne(TimeSpan.FromSeconds(180));
            if (!ready)
            {
                _log.Warning("[WaitForData] API data not received within 180 seconds — replay may produce empty results.");
                Console.WriteLine(" WARNING: API data not received within 180 seconds — replay may produce empty results.");
            }
            else
            {
                _log.Info("[WaitForData] API data ready — proceeding to replay.");
                Console.WriteLine("API data ready — proceeding to replay.");
            }
        }

        private BinaryDataPair? ReplayAndGetBuffers(uint captureId, ManualResetEvent importCompleteEvent)
        {
            Console.WriteLine("\n=== Replaying Snapshot Data ===");
            _log.Info($"[ImportCapture] Entering ReplayAndGetBuffers captureId={captureId}");
            BinaryDataPair? dsbBuffer = null;
            try
            {
                if (_currentCapture == null || !_currentCapture.IsValid())
                {
                    _log.Warning("[ImportCapture] Capture is null or invalid — skipping replay");
                    Console.WriteLine(" Capture not valid - skipping replay");
                    return null;
                }
                string? sessionPath = _sdpClient?.SessionManager?.GetSessionPath();
                if (sessionPath == null)
                {
                    _log.Warning("[ImportCapture] Session path unavailable — skipping replay");
                    Console.WriteLine(" Session path unavailable - skipping replay");
                    return null;
                }

                string dbPath = System.IO.Path.Combine(sessionPath, "sdp.db");
                _log.Info($"[ImportCapture] Calling QGLPluginService.ImportCapture captureId={captureId} db={dbPath}");
                Console.WriteLine("Replaying gfxr for Capture ID: " + captureId + "  DB: " + dbPath);

                bool imported = QGLPluginService.ImportCapture(captureId, dbPath);
                _log.Info($"[ImportCapture] ImportCapture returned: {imported}");
                if (!imported)
                {
                    _log.Warning("[ImportCapture] ImportCapture returned false — no replay data");
                    Console.WriteLine(" ImportCapture returned false");
                    return null;
                }

                _log.Info("[ImportCapture] ImportCapture succeeded — polling VulkanSnapshotGraphicsPipelines");
                Console.WriteLine("ImportCapture succeeded - polling DB...");
                int lastRows = -1, stable = 0;
                for (int i = 0; i < 90; i++)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        using var conn = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
                        conn.Open();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines";
                        int rows = Convert.ToInt32(cmd.ExecuteScalar());
                        if (rows == lastRows) // stable regardless of 0 or N
                        {
                            if (++stable >= 3)
                            {
                                _log.Info($"[ImportCapture] DB stable at {rows} pipelines (i={i})");
                                Console.WriteLine("DB stable: " + rows + " pipelines");
                                break;
                            }
                        }
                        else
                        {
                            if (rows != lastRows && lastRows >= 0)
                            {
                                _log.Debug($"[ImportCapture] DB importing: {rows} rows ({i}s)");
                                Console.WriteLine("  Importing: " + rows + " rows (" + i + "s)");
                            }
                            stable = 0;
                        }
                        lastRows = rows;
                    }
                    catch (Exception dbEx)
                    {
                        if (i % 5 == 0)
                        {
                            _log.Debug($"[ImportCapture] DB not ready ({i}s): {dbEx.Message}");
                            Console.WriteLine("  Waiting for DB access... (" + i + "s)");
                        }
                    }
                }
                if (stable < 3)
                {
                    _log.Warning($"[ImportCapture] DB polling timed out — last row count: {lastRows}");
                    Console.WriteLine(" DB polling timed out (last: " + lastRows + " rows)");
                }

                // Wait for bufferID=1 (ImportCapture trailing event) before fetching DSBbuffer.
                // DSB is only available in the plugin cache after ImportCapture's device-side
                // replay fires OnDataProcessed(bufferID=1). Without this wait the DB polling
                // stabilises too fast (e.g. 0-row table) and GetCachedSnapshotDsbBuffer returns
                // null, producing no CSVs (observed in snapshot_3 of 2026-04-07T16-16-40).
                _log.Info("[ImportCapture] Waiting for ImportCapture trailing event (bufferID=1) — DSB available after this");
                Console.WriteLine("Waiting for device replay complete (DSB ready)...");
                bool dsbReady = importCompleteEvent.WaitOne(TimeSpan.FromSeconds(60));
                if (!dsbReady)
                    _log.Warning("[ImportCapture] importCompleteEvent timeout (60s) — DSB may be null");
                else
                    _log.Info("[ImportCapture] importCompleteEvent fired — DSB should be cached");

                _log.Info($"[ImportCapture] Getting cached buffers for captureId={captureId}");
                Console.WriteLine("\n[DEBUG] Getting buffers - session=" + sessionPath + "  captureId=" + captureId);
                if (_clientDelegate is CliClientDelegate csd)
                {
                    dsbBuffer = csd.GetCachedSnapshotDsbBuffer(captureId);
                    if (dsbBuffer != null)
                    {
                        _log.Info($"[ImportCapture] DsbBuffer retrieved: {dsbBuffer.size} bytes");
                        Console.WriteLine("  DsbBuffer: " + dsbBuffer.size + " bytes");
                    }
                    else
                    {
                        _log.Warning("[ImportCapture] No cached DsbBuffer found");
                        Console.WriteLine("  No cached DsbBuffer");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[ImportCapture] Replay threw exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                Console.WriteLine(" Replay failed: " + ex.Message);
            }
            return dsbBuffer;
        }

        private void ExportDrawCallData(uint captureId, BinaryDataPair dsbBuffer, string sessionPath, string captureSubDir)
        {
            try
            {
                _log.Info($"[ExportDrawCall] Entering ExportDrawCallData captureId={captureId}");
                Console.WriteLine("\n[DEBUG] Exporting DrawCall data...");

                BinaryDataPair? apiBuffer = null;
                if (_clientDelegate is CliClientDelegate csd)
                    apiBuffer = csd.GetCachedSnapshotApiBuffer(captureId);

                if (apiBuffer == null || apiBuffer.size == 0)
                {
                    _log.Warning("[ExportDrawCall] SnapshotApiBuffer not available — skipping DrawCall export");
                    Console.WriteLine(" SnapshotApiBuffer not available - skipping DrawCall export");
                    return;
                }
                _log.Info($"[ExportDrawCall] ApiBuffer={apiBuffer.size}B DsbBuffer={dsbBuffer.size}B — calling LoadSnapshot");

                Console.WriteLine("Loading snapshot (api=" + apiBuffer.size + "B, dsb=" + dsbBuffer.size + "B)...");
                var model = new VulkanSnapshotModel();
                model.LoadSnapshot(captureId, apiBuffer, dsbBuffer);
                _log.Info($"[ExportDrawCall] LoadSnapshot completed for captureId={captureId}");

                // dbPath 在 session 根目录，CSV 写到 captureSubDir
                string dbPath = System.IO.Path.Combine(sessionPath, "sdp.db");
                _log.Info($"[ExportDrawCall] Exporting CSVs to {captureSubDir}");
                ExportCsv("DrawCall bindings",        model.ExportDrawCallBindingsToCSV(captureId,      System.IO.Path.Combine(captureSubDir, "DrawCallBindings.csv")));
                ExportCsv("RenderTarget attachments", model.ExportRenderTargetsToCSV(captureId,          System.IO.Path.Combine(captureSubDir, "DrawCallRenderTargets.csv"), dbPath));
                ExportCsv("DrawCall parameters",      model.ExportDrawCallParametersToCSV(captureId,     System.IO.Path.Combine(captureSubDir, "DrawCallParameters.csv")));
                ExportCsv("Vertex Buffer bindings",   model.ExportDrawCallVertexBuffersToCSV(captureId,  System.IO.Path.Combine(captureSubDir, "DrawCallVertexBuffers.csv")));
                ExportCsv("Index Buffer bindings",    model.ExportDrawCallIndexBuffersToCSV(captureId,   System.IO.Path.Combine(captureSubDir, "DrawCallIndexBuffers.csv")));
                ExportCsv("Pipeline Vertex Input",    model.ExportPipelineVertexInputStateToCSV(captureId,
                    System.IO.Path.Combine(captureSubDir, "PipelineVertexInputBindings.csv"),
                    System.IO.Path.Combine(captureSubDir, "PipelineVertexInputAttributes.csv")));
                _log.Info($"[ExportDrawCall] Core CSV exports done");

                if (_clientDelegate is CliClientDelegate mcsd)
                {
                    var metricsBuffer = mcsd.GetCachedSnapshotMetricsBuffer(captureId);
                    if (metricsBuffer != null && metricsBuffer.size > 0)
                    {
                        string metricsCsvPath = System.IO.Path.Combine(captureSubDir, "DrawCallMetrics.csv");
                        _log.Info($"[ExportDrawCall] Exporting metrics CSV metricsBuffer={metricsBuffer.size}B");
                        ExportCsv("DrawCall metrics", QGLPluginService.ExportMetricsToCsv(metricsBuffer, captureId, metricsCsvPath));
                        _log.Info($"[ExportDrawCall] Metrics CSV export done");
                    }
                    else
                    {
                        _log.Warning("[ExportDrawCall] SnapshotMetricsBuffer not available — skipping DrawCallMetrics export");
                        Console.WriteLine(" SnapshotMetricsBuffer not available — skipping DrawCallMetrics export");
                    }
                }

                // CSV 导入到 session 的 sdp.db
                _log.Info($"[ExportDrawCall] Importing CSVs into sdp.db");
                new CsvToDbService().ImportAllCsvs(captureSubDir, dbPath);
                _log.Info($"[ExportDrawCall] ImportAllCsvs completed");
            }
            catch (Exception ex)
            {
                _log.Error($"[ExportDrawCall] Export threw exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                Console.WriteLine(" DrawCall export failed: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static void ExportCsv(string label, string? path)
        {
            if (path != null) Console.WriteLine(label + " CSV exported: " + System.IO.Path.GetFileName(path));
            else Console.WriteLine(" Failed to export " + label);
        }
    }
}
