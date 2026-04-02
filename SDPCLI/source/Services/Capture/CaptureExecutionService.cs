using System;
using System.Collections.Generic;
using System.Threading;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Handles snapshot capture start and stop.
    /// Extracted from Application.StartCapture / StopCapture.
    /// </summary>
    public class CaptureExecutionService
    {
        private readonly SDPClient _sdpClient;
        private readonly Device? _connectedDevice;
        private readonly Config _config;
        private readonly string? _targetPackageName;
        private readonly uint _targetProcessPid;
        private readonly uint _renderingAPI;
        private readonly ClientDelegate? _clientDelegate;

        // Mutable state shared between Start/Stop
        private uint _verifiedProcessPid;

        /// <summary>
        /// The capture object — may be created during StartCapture if none existed.
        /// Read this back into Application after calling StartCapture.
        /// </summary>
        public global::Capture? CurrentCapture { get; private set; }

        public CaptureExecutionService(
            SDPClient sdpClient,
            Device? connectedDevice,
            global::Capture? existingCapture,
            Config config,
            string? targetPackageName,
            uint targetProcessPid,
            uint verifiedProcessPid,
            uint renderingAPI,
            ClientDelegate? clientDelegate)
        {
            _sdpClient = sdpClient;
            _connectedDevice = connectedDevice;
            CurrentCapture = existingCapture;
            _config = config;
            _targetPackageName = targetPackageName;
            _targetProcessPid = targetProcessPid;
            _verifiedProcessPid = verifiedProcessPid;
            _renderingAPI = renderingAPI;
            _clientDelegate = clientDelegate;
        }

        public bool StartCapture()
        {
            Console.WriteLine("\nStarting capture...");

            try
            {
                // Get renderer string from device
                string rendererString = "";
                if (_connectedDevice != null)
                {
                    try
                    {
                        rendererString = _connectedDevice.GetProperty(DeviceSettings.ProfilerDeviceRendererString);
                        if (rendererString.Equals("Unknown"))
                            rendererString = "";
                        Console.WriteLine($"Device renderer string: '{rendererString}'");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WARNING: Could not get renderer string: {ex.Message}");
                    }
                }

                // Ensure capture object exists
                if (CurrentCapture == null || !CurrentCapture.IsValid())
                {
                    Console.WriteLine("Capture not pre-created, creating now...");
                    uint capId = _sdpClient.CaptureManager.CreateCapture(4);
                    CurrentCapture = _sdpClient.CaptureManager.GetCapture(capId);
                    Console.WriteLine($"Created Snapshot capture with ID: {capId}");
                }

                uint processPid = _verifiedProcessPid;

                if (processPid == 0)
                {
                    Console.WriteLine("\n=== Retrying Process Discovery ===");
                    Console.WriteLine("Initial discovery timed out, checking if process appeared later...");

                    CliClientDelegate? simpleDelegate = _clientDelegate as CliClientDelegate;
                    if (simpleDelegate != null)
                    {
                        int currentCount = simpleDelegate.GetDiscoveredProcessCount();
                        Console.WriteLine($"OnProcessAdded callbacks discovered {currentCount} processes total");

                        if (currentCount == 0)
                        {
                            Console.WriteLine("\n⚠ CRITICAL: Still no OnProcessAdded callbacks!");
                            Console.WriteLine("Device plugins are not working. Cannot discover processes.");
                        }
                        else
                        {
                            Console.WriteLine("\nSearching for target process in discovered processes...");

                            if (_targetProcessPid > 0 && simpleDelegate.IsProcessDiscovered(_targetProcessPid))
                            {
                                processPid = _targetProcessPid;
                                Console.WriteLine($"✓ Found by PID={processPid} (discovered after initial timeout)!");
                            }
                            else if (!string.IsNullOrEmpty(_targetPackageName))
                            {
                                uint? foundPid = simpleDelegate.FindProcessByName(_targetPackageName!);
                                if (foundPid.HasValue)
                                {
                                    processPid = foundPid.Value;
                                    Console.WriteLine($"✓ Found by name '{_targetPackageName}' (PID={processPid})!");
                                }
                                else
                                {
                                    Console.WriteLine($"✗ Package '{_targetPackageName}' not in discovered processes");
                                }
                            }
                        }

                        if (processPid > 0)
                            _verifiedProcessPid = processPid;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Cannot access CliClientDelegate");
                    }
                }
                else
                {
                    Console.WriteLine($"Using previously verified PID: {processPid}");

                    if (!string.IsNullOrEmpty(_targetPackageName))
                    {
                        Console.WriteLine("\nRefreshing PID from device (in case app restarted)...");
                        try
                        {
                            var refreshPidProcess = new System.Diagnostics.Process
                            {
                                StartInfo = new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = "adb",
                                    Arguments = $"shell pidof {_targetPackageName}",
                                    RedirectStandardOutput = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                }
                            };
                            refreshPidProcess.Start();
                            string refreshPidOutput = refreshPidProcess.StandardOutput.ReadToEnd().Trim();
                            refreshPidProcess.WaitForExit();

                            if (uint.TryParse(refreshPidOutput, out uint currentPid))
                            {
                                if (currentPid != processPid)
                                {
                                    Console.WriteLine($"⚠ PID changed! Old: {processPid}, New: {currentPid}");
                                    Console.WriteLine("  App likely restarted - updating to new PID");
                                    processPid = currentPid;
                                    _verifiedProcessPid = currentPid;
                                }
                                else
                                {
                                    Console.WriteLine($"✓ PID unchanged: {processPid}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"⚠ WARNING: App not running! Cannot get PID.");
                                Console.WriteLine($"  Will try to use last known PID: {processPid}");
                            }
                        }
                        catch (Exception pidEx)
                        {
                            Console.WriteLine($"⚠ Could not refresh PID: {pidEx.Message}");
                            Console.WriteLine($"  Will use last known PID: {processPid}");
                        }
                    }
                }

                if (processPid == 0)
                {
                    Console.WriteLine("\n⚠ ERROR: No process PID available!");
                    Console.WriteLine("Process was never discovered by device plugins.");
                    Console.WriteLine("\nPossible reasons:");
                    Console.WriteLine("  1. App doesn't use GPU (Vulkan/OpenGL) - plugins can't detect it");
                    Console.WriteLine("  2. App crashed immediately after launch");
                    Console.WriteLine("  3. service/android/ plugins not properly deployed");
                    Console.WriteLine("\nCannot start capture without target process.");
                    return false;
                }

                try
                {
                    Process? targetProc = _sdpClient.ProcessManager.GetProcess(processPid);
                    if (targetProc == null || !targetProc.IsValid())
                    {
                        Console.WriteLine($"ERROR: Process PID={processPid} no longer valid");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR verifying process: {ex.Message}");
                    return false;
                }

                string metricName = _renderingAPI switch
                {
                    16 => "Vulkan Snapshot",
                    8 => "OpenGL Snapshot",
                    32 => "DX12 Snapshot",
                    _ => "Vulkan Snapshot"
                };

                Console.WriteLine($"\nActivating metrics for Snapshot capture...");
                Console.WriteLine($"Primary metric: {metricName}");

                MetricManager metricManager = _sdpClient.MetricManager;
                Metric? snapshotMetric = null;
                List<Metric> activatedMetrics = new List<Metric>();

                try
                {
                    snapshotMetric = metricManager.GetMetricByName(metricName);

                    if (snapshotMetric == null || !snapshotMetric.IsValid())
                    {
                        Console.WriteLine($"ERROR: '{metricName}' metric not found");
                        return false;
                    }

                    Console.WriteLine($"✓ Found primary metric: {metricName}");
                    bool enableMetrics = _config.GetBool("EnableMetrics", true);

                    MetricList allMetrics = metricManager.GetAllMetrics();
                    int snapshotMetricCount = 0;
                    int activatedCount = 0;

                    if (enableMetrics)
                    {
                        Console.WriteLine("\nActivating additional metrics that support Snapshot...");

                        foreach (Metric metric in allMetrics)
                        {
                            if (metric != null && metric.IsValid())
                            {
                                MetricProperties props = metric.GetProperties();
                                bool supportsSnapshot = (props.captureTypeMask & 0x04) != 0;

                                if (supportsSnapshot)
                                {
                                    snapshotMetricCount++;

                                    if (props.hidden)
                                        continue;

                                    try
                                    {
                                        IDList activeProcesses = metric.GetActiveProcesses(4);
                                        if (activeProcesses.Count > 0)
                                        {
                                            foreach (uint activePid in activeProcesses)
                                            {
                                                if (activePid != processPid)
                                                    metric.Deactivate(activePid, 4);
                                            }
                                        }

                                        bool activated = metric.Activate(processPid, 4);
                                        if (activated)
                                        {
                                            activatedMetrics.Add(metric);
                                            activatedCount++;
                                            Console.WriteLine($"  ✓ {props.name} (ID: {props.id})");
                                        }
                                    }
                                    catch (Exception activateEx)
                                    {
                                        Console.WriteLine($"  ⚠ Failed to activate {props.name}: {activateEx.Message}");
                                    }
                                }
                            }
                        }

                        Console.WriteLine($"\n✓ Activated {activatedCount} metrics (out of {snapshotMetricCount} snapshot-capable)");

                        if (activatedCount == 0)
                        {
                            Console.WriteLine("ERROR: No metrics activated - cannot capture data");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n⚠ Metrics disabled by config (EnableMetrics=false) — skipping activation");
                    }

                    // Record activated metrics to CaptureMetrics table
                    Console.WriteLine("\nRecording activated metrics to database...");
                    try
                    {
                        DataModel dataModel = _sdpClient.Client.GetDataModel();
                        Model captureManagerModel = dataModel.GetModel("CaptureManager");
                        ModelObject captureMetricsObject = captureManagerModel.GetModelObject("CaptureMetrics");

                        uint captureId = CurrentCapture!.GetProperties().captureID;
                        int recordedCount = 0;

                        foreach (Metric activeMetric in activatedMetrics)
                        {
                            try
                            {
                                MetricProperties props = activeMetric.GetProperties();
                                ModelObjectData metricData = captureMetricsObject.NewData();
                                metricData.SetAttributeValue("captureID", captureId.ToString());
                                metricData.SetAttributeValue("processID", processPid.ToString());
                                metricData.SetAttributeValue("metricID", props.id.ToString());
                                metricData.Save();
                                recordedCount++;
                            }
                            catch (Exception recordEx)
                            {
                                Console.WriteLine($"  ⚠ Failed to record metric: {recordEx.Message}");
                            }
                        }

                        Console.WriteLine($"✓ Recorded {recordedCount} metrics to CaptureMetrics table");
                    }
                    catch (Exception recordMetricsEx)
                    {
                        Console.WriteLine($"⚠ WARNING: Failed to record metrics: {recordMetricsEx.Message}");
                        Console.WriteLine("  Performance data may not be exported correctly");
                    }
                }
                catch (Exception mEx)
                {
                    Console.WriteLine($"ERROR activating metrics: {mEx.Message}");
                    return false;
                }

                CaptureSettings captureSettings = new CaptureSettings(
                    captureType: 4,
                    pid: processPid,
                    s: 0,
                    d: 0,
                    r: rendererString
                );

                Console.WriteLine("Starting snapshot capture...");

                bool started = CurrentCapture!.Start(captureSettings);
                if (!started)
                {
                    Console.WriteLine("ERROR: Failed to start capture");
                    foreach (Metric m in activatedMetrics)
                    {
                        try { m?.Deactivate(processPid, 4); } catch { }
                    }
                    return false;
                }

                Console.WriteLine("✓ Capture started");

                // Auto-stop after 1 second (snapshot pattern)
                List<Metric> metricsToDeactivate = new List<Metric>(activatedMetrics);
                uint capturedPid = processPid;
                global::Capture? captureRef = CurrentCapture;
                Thread stopThread = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    try
                    {
                        if (captureRef != null && captureRef.IsValid())
                            captureRef.Stop();

                        foreach (Metric m in metricsToDeactivate)
                        {
                            try
                            {
                                if (m != null && m.IsValid())
                                    m.Deactivate(capturedPid, 4);
                            }
                            catch { }
                        }
                    }
                    catch { }
                });
                stopThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting capture: {ex.Message}");
                return false;
            }
        }

        public void StopCapture()
        {
            Console.WriteLine("\nStopping capture...");

            try
            {
                if (CurrentCapture != null)
                {
                    bool stopped = CurrentCapture.Stop();
                    if (stopped)
                        Console.WriteLine("✓ Capture stopped successfully");
                    else
                        Console.WriteLine("Warning: Capture.Stop() returned false");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping capture: {ex.Message}");
            }
        }
    }
}
