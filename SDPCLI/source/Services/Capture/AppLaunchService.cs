using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using SnapdragonProfilerCLI.Tools;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Handles app selection, launch, and process discovery.
    /// Extracted from Application.SelectAndLaunchApp / LaunchApp / WaitForProcessDiscovery.
    /// </summary>
    public class AppLaunchService
    {
        private readonly Config _config;
        private readonly Func<string?, string?> _readLine;
        private readonly SDPClient _sdpClient;
        private readonly Device _device;
        private readonly ClientDelegate _clientDelegate;

        // Output state — read these back after SelectAndLaunch / WaitForProcess
        public string? TargetPackageName { get; private set; }
        public uint TargetProcessPid { get; private set; }
        public uint LastLaunchedPid { get; private set; }
        public uint RenderingAPI { get; private set; }
        public uint VerifiedProcessPid { get; private set; }

        public AppLaunchService(
            Config config,
            Func<string?, string?> readLine,
            SDPClient sdpClient,
            Device device,
            ClientDelegate clientDelegate)
        {
            _config = config;
            _readLine = readLine;
            _sdpClient = sdpClient;
            _device = device;
            _clientDelegate = clientDelegate;
        }

        // ─── Public entry points ───────────────────────────────────────────────

        public bool SelectAndLaunch(string packageName)
        {
            Console.WriteLine($"\nSearching for package: {packageName}");

            try
            {
                // Step 1: Check package exists
                var checkProcess = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = $"shell pm list packages {packageName}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                checkProcess.Start();
                string packageOutput = checkProcess.StandardOutput.ReadToEnd();
                checkProcess.WaitForExit();

                if (string.IsNullOrWhiteSpace(packageOutput) || !packageOutput.Contains($"package:{packageName}"))
                {
                    Console.WriteLine($"ERROR: Package '{packageName}' not found on device");
                    Console.WriteLine("Tip: Use 'adb shell pm list packages | grep <keyword>' to search for packages");
                    return false;
                }

                Console.WriteLine($"✓ Package found: {packageName}");

                // Step 2: Get APK path
                Console.WriteLine("\nFetching APK path...");
                var getApkPathProcess = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = $"shell pm path {packageName}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                getApkPathProcess.Start();
                string apkPathOutput = getApkPathProcess.StandardOutput.ReadToEnd().Trim();
                getApkPathProcess.WaitForExit();

                if (apkPathOutput.StartsWith("package:"))
                {
                    string apkPath = apkPathOutput.Substring("package:".Length).Trim();
                    Console.WriteLine($"APK path: {apkPath}");
                }

                // Step 3: Get activities
                Console.WriteLine("Fetching activities...");
                var activities = new List<string>();
                string deviceSerial = _device?.GetName() ?? "";

                var dumpsysProcess = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = string.IsNullOrEmpty(deviceSerial)
                            ? $"shell \"dumpsys package | grep -i {packageName}/ | grep -o '[^ ]*/[^ :}}]*' | sort | uniq\""
                            : $"-s {deviceSerial} shell \"dumpsys package | grep -i {packageName}/ | grep -o '[^ ]*/[^ :}}]*' | sort | uniq\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                dumpsysProcess.Start();
                string dumpsysOutput = dumpsysProcess.StandardOutput.ReadToEnd();
                dumpsysProcess.WaitForExit();

                var activityRegex = new Regex(@".*?/(.*?)[\r\n]+");
                var activityMatches = activityRegex.Matches(dumpsysOutput);

                if (activityMatches != null && activityMatches.Count > 0)
                {
                    foreach (Match match in activityMatches)
                    {
                        if (match != null && match.Groups.Count == 2)
                        {
                            string activityName = match.Groups[1].Value;
                            if (!activities.Contains(activityName))
                                activities.Add(activityName);
                        }
                    }
                }

                Console.WriteLine($"Found {activities.Count} activities");
                foreach (var act in activities)
                    Console.WriteLine($"  - {act}");

                if (activities.Count == 0)
                {
                    Console.WriteLine("No launcher activities found. Trying default launch...");
                    return LaunchApp(packageName);
                }

                // Determine launch target from config or interactive
                string? configActivityName = _config.Get("ActivityName");
                int configActivityIndex = _config.GetInt("ActivityIndex", -1);

                string launchTarget;

                if (!string.IsNullOrEmpty(configActivityName))
                {
                    Console.WriteLine($"\nUsing activity from config: {configActivityName}");
                    launchTarget = $"{packageName}/{configActivityName}";
                }
                else if (configActivityIndex >= 0 && configActivityIndex <= activities.Count)
                {
                    if (configActivityIndex == 0)
                    {
                        Console.WriteLine("\nUsing package-only launch from config");
                        launchTarget = packageName;
                    }
                    else
                    {
                        string selectedActivity = activities[configActivityIndex - 1];
                        Console.WriteLine($"\nUsing activity #{configActivityIndex} from config: {selectedActivity}");
                        launchTarget = $"{packageName}/{selectedActivity}";
                    }
                }
                else
                {
                    Console.WriteLine($"\nFound {activities.Count} activity(ies):");
                    for (int i = 0; i < activities.Count; i++)
                        Console.WriteLine($"  {i + 1}. {activities[i]}");
                    Console.WriteLine("  0. Try default launch (package only)");

                    Console.Write($"\nSelect activity (0-{activities.Count}): ");
                    string? selection = _readLine("1");

                    if (!int.TryParse(selection, out int selectedIndex) || selectedIndex < 0 || selectedIndex > activities.Count)
                    {
                        Console.WriteLine("Invalid selection. Exiting...");
                        return false;
                    }

                    launchTarget = selectedIndex == 0 ? packageName : $"{packageName}/{activities[selectedIndex - 1]}";
                }

                return LaunchApp(launchTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during app selection: {ex.Message}");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return false;
            }
        }

        public bool WaitForProcess()
        {
            Console.WriteLine("\nWaiting for process discovery...");
            Console.WriteLine($"Target: {TargetPackageName}, PID: {TargetProcessPid}");

            CliClientDelegate? simpleDelegate = _clientDelegate as CliClientDelegate;
            if (simpleDelegate == null)
            {
                Console.WriteLine("ERROR: ClientDelegate is not CliClientDelegate!");
                return false;
            }

            // Query ProcessManager for all processes
            try
            {
                ProcessList allProcesses = _sdpClient.ProcessManager.GetAllProcesses();
                Console.WriteLine($"ProcessManager found {allProcesses.Count} processes");

                if (allProcesses.Count > 0)
                {
                    foreach (Process proc in allProcesses)
                    {
                        if (proc != null && proc.IsValid())
                        {
                            ProcessProperties props = proc.GetProperties();

                            if (TargetProcessPid > 0 && props.pid == TargetProcessPid)
                            {
                                VerifiedProcessPid = TargetProcessPid;
                                Console.WriteLine($"✓ Found target process: {props.name} (PID: {TargetProcessPid})");
                                return true;
                            }

                            if (!string.IsNullOrEmpty(TargetPackageName) && props.name.Contains(TargetPackageName))
                            {
                                VerifiedProcessPid = props.pid;
                                Console.WriteLine($"✓ Found target process: {props.name} (PID: {props.pid})");
                                return true;
                            }
                        }
                    }

                    Console.WriteLine("⚠ Target process not found in ProcessManager's list");
                }
                else
                {
                    Console.WriteLine("⚠ ProcessManager returned 0 processes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error querying ProcessManager: {ex.Message}");
            }

            bool processFound = false;
            int maxWaitSeconds = (TargetProcessPid > 0) ? 5 : 30;
            int elapsed = 0;

            while (elapsed < maxWaitSeconds && !processFound)
            {
                int discoveredCount = simpleDelegate.GetDiscoveredProcessCount();

                if (elapsed % 10 == 0)
                    Console.WriteLine($"[{elapsed}s] Discovered: {discoveredCount} processes");

                if (TargetProcessPid > 0 && simpleDelegate.IsProcessDiscovered(TargetProcessPid))
                {
                    VerifiedProcessPid = TargetProcessPid;

                    try
                    {
                        Process? proc = _sdpClient.ProcessManager.GetProcess(TargetProcessPid);
                        if (proc != null && proc.IsValid())
                        {
                            ProcessProperties props = proc.GetProperties();
                            Console.WriteLine($"\n✓ Target process discovered via OnProcessAdded callback!");
                            Console.WriteLine($"  Name: {props.name}");
                            Console.WriteLine($"  PID: {props.pid}");
                            Console.WriteLine($"  State: {props.state}");
                            processFound = true;
                            break;
                        }
                    }
                    catch { }
                }

                if (!processFound && !string.IsNullOrEmpty(TargetPackageName))
                {
                    uint? foundPid = simpleDelegate.FindProcessByName(TargetPackageName!);
                    if (foundPid.HasValue)
                    {
                        VerifiedProcessPid = foundPid.Value;

                        try
                        {
                            Process? proc = _sdpClient.ProcessManager.GetProcess(foundPid.Value);
                            if (proc != null && proc.IsValid())
                            {
                                ProcessProperties props = proc.GetProperties();
                                Console.WriteLine($"\n✓ Target process discovered via OnProcessAdded callback!");
                                Console.WriteLine($"  Name: {props.name}");
                                Console.WriteLine($"  PID: {props.pid}");
                                Console.WriteLine($"  State: {props.state}");
                                processFound = true;
                                break;
                            }
                        }
                        catch { }
                    }
                }

                Thread.Sleep(1000);
                elapsed++;
            }

            if (!processFound)
            {
                int finalCount = simpleDelegate.GetDiscoveredProcessCount();
                Console.WriteLine($"\n⚠ Process not found after {maxWaitSeconds}s (discovered: {finalCount})");

                if (finalCount == 0)
                    Console.WriteLine("⚠ No callbacks received - device plugins not working");

                if (TargetProcessPid > 0)
                {
                    VerifiedProcessPid = TargetProcessPid;
                    Console.WriteLine($"\n✓ WORKAROUND: Using known PID directly (PID: {TargetProcessPid})");
                    Console.WriteLine("  ProcessManager callbacks failed, but we have PID from launch.");
                    Console.WriteLine("  Will attempt capture using this PID.");
                    return true;
                }

                return false;
            }
            else
            {
                Console.WriteLine($"✓ Process verified (PID: {VerifiedProcessPid})");
                return true;
            }
        }

        // ─── Private helpers ───────────────────────────────────────────────────

        private bool LaunchApp(string packageOrActivity)
        {
            // Set output state
            TargetPackageName = packageOrActivity.Contains("/")
                ? packageOrActivity.Split('/')[0]
                : packageOrActivity;

            string packageName = TargetPackageName!;
            string activityName = packageOrActivity.Contains("/")
                ? packageOrActivity.Split('/')[1]
                : "";

            Console.WriteLine($"\nLaunching: {packageOrActivity} (package: {packageName})");

            try
            {
                if (_device == null)
                {
                    Console.WriteLine("ERROR: No connected device");
                    return false;
                }

                DeviceConnectionState currentState = _device.GetDeviceState();
                if (currentState != DeviceConnectionState.Connected)
                {
                    Console.WriteLine($"ERROR: Device not in Connected state (current: {currentState})");
                    return false;
                }

                bool isDeviceRooted = false;
                try
                {
                    string rootedProp = _device.GetProperty(DeviceSettings.ProfilerDeviceIsRooted);
                    isDeviceRooted = rootedProp.Equals("True", StringComparison.OrdinalIgnoreCase);
                    Console.WriteLine($"Device rooted status: {isDeviceRooted}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WARNING: Could not check rooted status: {ex.Message}");
                }

                var (isDebuggable, hasInternetPermission) = Utility.CheckAndGrantPermissions(packageName, isDeviceRooted);

                Console.WriteLine("\n=== Enabling Snapshot Data Capture Options ===");

                string launchOptions = "";
                string[] commonSnapshotOptions =
                {
                    "Collect Shader Data",
                    "Collect Texture Data",
                    "Collect Buffer Data",
                    "Collect Pipeline Data",
                    "Collect GPU Scope Data",
                    "Collect Screenshots",
                    "Collect API Calls",
                    "Detailed Metrics",
                };

                foreach (string optionName in commonSnapshotOptions)
                {
                    launchOptions += $"{optionName}:1;";
                    Console.WriteLine($"  ✓ {optionName}");
                }

                launchOptions += "DisableUGDFlag:1;";

                Console.WriteLine($"\n=== Complete Launch Options String ===");
                Console.WriteLine($"Length: {launchOptions.Length} characters");
                Console.WriteLine($"Content: {launchOptions}");
                Console.WriteLine($"======================================\n");

                RenderingAPI = (uint)_config.GetInt("RenderingAPI", 16);
                Console.WriteLine($"Using RenderingAPI: {RenderingAPI} ({Utility.GetRenderingAPIName(RenderingAPI)})");

                uint captureType = (uint)_config.GetInt("CaptureType", 4);
                Console.WriteLine($"Using CaptureType: {captureType} ({Utility.GetCaptureTypeName(captureType)})");

                if (captureType != 4)
                {
                    Console.WriteLine($"⚠ WARNING: CaptureType {captureType} is not Snapshot mode.");
                    Console.WriteLine("  Only Snapshot mode (4) captures a single frame.");
                    Console.WriteLine("  Other modes require continuous profiling.");
                }

                AppStartSettings appStartSettings = new AppStartSettings(
                    packageOrActivity,
                    "",
                    RenderingAPI,
                    captureType,
                    launchOptions
                );

                Console.WriteLine($"\nLaunching app: {packageOrActivity}");
                Console.WriteLine($"  RenderingAPI: {Utility.GetRenderingAPIName(RenderingAPI)}");
                Console.WriteLine($"  CaptureType: {Utility.GetCaptureTypeName(captureType)}");

                if (!packageOrActivity.Contains("/"))
                    Console.WriteLine("⚠ WARNING: Package should be in 'package/activity' format");

                if (!hasInternetPermission)
                {
                    Console.WriteLine("\n⚠ CRITICAL: INTERNET permission is missing!");
                    Console.WriteLine("SDK StartApp WILL FAIL without this permission.");
                    Console.WriteLine("\n📋 RECOMMENDED SOLUTIONS (in order of preference):");
                    Console.WriteLine("1. Add INTERNET permission to app (PERMANENT FIX):");
                    Console.WriteLine("   - Add <uses-permission android:name=\"android.permission.INTERNET\" /> to AndroidManifest.xml");
                    Console.WriteLine("   - Rebuild and reinstall the app");
                    Console.WriteLine("\n2. Manual launch + PID capture:");
                    Console.WriteLine("   - Launch app manually: adb shell am start -n " + packageOrActivity);
                    Console.WriteLine("   - Get PID: adb shell pidof " + packageName);
                    Console.WriteLine("   - Modify code to skip StartApp and use existing PID");
                    Console.WriteLine("\n⏱ Proceeding with StartApp anyway (will likely timeout in 60s)...\n");
                }

                bool startResult = false;
                try
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Calling sdpClient.StartApplication()...");

                    uint pid = _sdpClient.StartApplication(_device, appStartSettings);

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] StartApplication() returned PID: {pid}");

                    if (pid > 0)
                    {
                        LastLaunchedPid = pid;
                        TargetProcessPid = pid;
                        Console.WriteLine($"✓ Got process PID from SDPClient: {TargetProcessPid}");
                        startResult = true;
                    }
                    else
                    {
                        Console.WriteLine("⚠ StartApplication returned 0 (app launching asynchronously)");
                        Console.WriteLine("  Will wait for process discovery via OnProcessAdded callbacks");
                        TargetProcessPid = 0;
                        startResult = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Exception in StartApplication: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    startResult = false;

                    Console.WriteLine("\nDumping recent device logs...");
                    Utility.DumpRecentLogcat("SDPCore|sdp|SnapdragonProfiler", 50);
                    Utility.DiagnoseAppLaunchIssue(packageName, activityName);

                    Console.WriteLine("\n=== Debug Suggestions ===");
                    Console.WriteLine("1. Monitor device logs in real-time:");
                    Console.WriteLine("   adb logcat -v time | findstr /I \"SDPCore sdp SnapdragonProfiler\"");
                    Console.WriteLine("\n2. Check if service is running on device:");
                    Console.WriteLine("   adb shell ps | findstr sdp");
                    Console.WriteLine("\n3. Try manual launch to test app:");
                    Console.WriteLine($"   adb shell am start -n {packageName}/{activityName}");
                    Console.WriteLine("=========================\n");
                }

                if (startResult)
                {
                    if (LastLaunchedPid > 0)
                        Console.WriteLine($"✓ App launched successfully via SDPCore (PID: {LastLaunchedPid})");
                    else
                        Console.WriteLine("✓ App launch initiated via SDPCore (PID pending discovery)");

                    Console.WriteLine("Waiting for app to initialize...");
                    Thread.Sleep(3000);

                    if (RenderingAPI == 16)
                        Utility.VerifyVulkanLayerSettings();

                    return true;
                }
                else
                {
                    Console.WriteLine("ERROR: StartApp via SDPCore failed");

                    if (RenderingAPI == 16)
                        Utility.VerifyVulkanLayerSettings();

                    Console.WriteLine("\nSnapdragon Profiler was unable to connect to " + packageOrActivity);
                    Console.WriteLine("Verify that the application has:");
                    Console.WriteLine("  - Internet permission enabled");
                    Console.WriteLine("  - The correct GPU API selected");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching app: {ex.Message}");

                if (RenderingAPI == 16)
                    Utility.VerifyVulkanLayerSettings();

                return false;
            }
        }
    }
}
