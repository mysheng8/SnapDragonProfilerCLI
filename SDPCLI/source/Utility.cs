using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SnapdragonProfilerCLI.Logging;
using DiagProcess       = System.Diagnostics.Process;
using DiagProcessModule = System.Diagnostics.ProcessModule;
using DiagProcessInfo   = System.Diagnostics.ProcessStartInfo;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Static utility helpers: DLL paths, enum names, screenshot, ADB diagnostics, permissions.
    /// </summary>
    public static class Utility
    {
        private const string Ctx = "Utility";

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr AddDllDirectory(string NewDirectory);

        /// <summary>
        /// Configure Windows DLL search paths so that processor plugins (C++ DLLs)
        /// can locate native dependencies (SDPCore.dll, libcurl.dll, etc.).
        /// </summary>
        public static void ConfigureNativeDllSearchPaths()
        {
            try
            {
                string appDir     = AppDomain.CurrentDomain.BaseDirectory;
                string pluginsDir = Path.Combine(appDir, "plugins", "processor");

                AppLogger.Info(Ctx, "Configuring native DLL search paths...");
                AppLogger.Debug(Ctx, $"  App dir    : {appDir}");
                AppLogger.Debug(Ctx, $"  Plugins dir: {pluginsDir}");

                try
                {
                    IntPtr r1 = AddDllDirectory(appDir);
                    IntPtr r2 = AddDllDirectory(pluginsDir);
                    if (r1 != IntPtr.Zero && r2 != IntPtr.Zero)
                    {
                        AppLogger.Success(Ctx, $"Added DLL search dirs: {appDir}  +  {pluginsDir}");
                        return;
                    }
                    AppLogger.Warn(Ctx, $"AddDllDirectory partially failed (err {Marshal.GetLastWin32Error()}), falling back to SetDllDirectory");
                }
                catch (Exception ex)
                {
                    AppLogger.Warn(Ctx, $"AddDllDirectory not available: {ex.Message}");
                }

                bool ok = SetDllDirectory(appDir);
                if (ok)
                    AppLogger.Success(Ctx, $"SetDllDirectory: {appDir}");
                else
                    AppLogger.Warn(Ctx, $"SetDllDirectory failed (err {Marshal.GetLastWin32Error()}) â€” processor plugins may fail");
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"Failed to configure DLL search paths: {ex.Message}");
            }
        }

        /// <summary>Returns a human-readable label for a rendering API enum value.</summary>
        public static string GetRenderingAPIName(uint api) => api switch
        {
            0  => "None (auto-detect)",
            1  => "DirectX11",
            2  => "DirectX12",
            4  => "OpenCL",
            8  => "OpenGL",
            16 => "Vulkan",
            _  => $"Unknown ({api})"
        };

        /// <summary>Returns a human-readable label for a capture type enum value.</summary>
        public static string GetCaptureTypeName(uint type) => type switch
        {
            1 => "Realtime",
            2 => "Trace",
            4 => "Snapshot",
            8 => "Sampling",
            _ => $"Unknown ({type})"
        };

        /// <summary>
        /// Lists all loaded native DLLs and managed assemblies in the current process.
        /// Pass a non-empty <paramref name="filter"/> to narrow results by module name or path.
        /// </summary>
        public static void ListLoadedModules(string filter = "")
        {
            try
            {
                AppLogger.Info(Ctx, "=== Loaded Modules ===");
                var modules = DiagProcess.GetCurrentProcess().Modules
                    .Cast<DiagProcessModule>()
                    .OrderBy(m => m.ModuleName)
                    .ToList();

                if (!string.IsNullOrEmpty(filter))
                {
                    modules = modules.Where(m =>
                        m.ModuleName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        m.FileName  .IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
                    ).ToList();
                    AppLogger.Info(Ctx, $"Filter: {filter}");
                }

                AppLogger.Info(Ctx, $"Total modules: {modules.Count}");

                var managedNames = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => Path.GetFileName(a.Location).ToLowerInvariant())
                    .Where(n => !string.IsNullOrEmpty(n))
                    .ToHashSet();

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                var native  = modules.Where(m => m.ModuleName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                                               && !managedNames.Contains(m.ModuleName.ToLowerInvariant())).ToList();
                var managed = modules.Where(m => m.ModuleName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                                               &&  managedNames.Contains(m.ModuleName.ToLowerInvariant())).ToList();

                AppLogger.Info(Ctx, $"\nNative DLLs ({native.Count}):");
                foreach (var m in native.Take(50))
                {
                    string dir = Path.GetDirectoryName(m.FileName) ?? "";
                    string loc = dir.StartsWith(baseDir) ? "." + dir.Substring(baseDir.Length - 1) : dir;
                    AppLogger.Info(Ctx, $"  {m.ModuleName,-40} {loc}");
                }
                if (native.Count > 50) AppLogger.Info(Ctx, $"  ... and {native.Count - 50} more");

                AppLogger.Info(Ctx, $"\nManaged Assemblies ({managed.Count}):");
                foreach (var m in managed.Take(50))
                {
                    string dir = Path.GetDirectoryName(m.FileName) ?? "";
                    string loc = dir.StartsWith(baseDir) ? "." + dir.Substring(baseDir.Length - 1) : dir;
                    AppLogger.Info(Ctx, $"  {m.ModuleName,-40} {loc}");
                }
                if (managed.Count > 50) AppLogger.Info(Ctx, $"  ... and {managed.Count - 50} more");

                string pluginsDir = Path.Combine(baseDir, "plugins");
                if (Directory.Exists(pluginsDir))
                {
                    var loaded   = modules.Select(m => m.ModuleName.ToLowerInvariant()).ToHashSet();
                    var unloaded = Directory.GetFiles(pluginsDir, "*.dll", SearchOption.TopDirectoryOnly)
                        .Where(f => !loaded.Contains(Path.GetFileName(f).ToLowerInvariant()))
                        .ToList();

                    if (unloaded.Count > 0)
                    {
                        AppLogger.Info(Ctx, $"\nUnloaded plugins in plugins/ ({unloaded.Count}) â€” loaded on-demand by GUI:");
                        foreach (var f in unloaded.Take(20))
                            AppLogger.Info(Ctx, $"  {Path.GetFileName(f)}");
                        if (unloaded.Count > 20) AppLogger.Info(Ctx, $"  ... and {unloaded.Count - 20} more");
                    }
                }

                AppLogger.Info(Ctx, "======================");
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"ListLoadedModules failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves <paramref name="imageData"/> to <paramref name="sessionPath"/> as PNG or JPEG,
        /// detected from the file header. Logs a warning if the format is unrecognised.
        /// </summary>
        public static void SaveScreenshotAsImage(byte[] imageData, string sessionPath)
        {
            try
            {
                AppLogger.Debug(Ctx, $"Analysing screenshot ({imageData.Length} bytes)");

                bool isPng  = imageData.Length > 4
                           && imageData[0] == 0x89 && imageData[1] == 0x50
                           && imageData[2] == 0x4E && imageData[3] == 0x47;
                bool isJpeg = imageData.Length > 2
                           && imageData[0] == 0xFF && imageData[1] == 0xD8;

                if (isPng)
                {
                    string path = Path.Combine(sessionPath, "snapshot_screenshot.png");
                    File.WriteAllBytes(path, imageData);
                    AppLogger.Success(Ctx, $"PNG screenshot saved: {path}");
                }
                else if (isJpeg)
                {
                    string path = Path.Combine(sessionPath, "snapshot_screenshot.jpg");
                    File.WriteAllBytes(path, imageData);
                    AppLogger.Success(Ctx, $"JPEG screenshot saved: {path}");
                }
                else
                {
                    string hex = imageData.Length >= 16
                        ? string.Join(" ", imageData.Take(16).Select(b => b.ToString("X2")))
                        : "(too short)";
                    AppLogger.Warn(Ctx, $"Unknown image format â€” header bytes: {hex}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"SaveScreenshotAsImage failed: {ex.Message}");
            }
        }

        /// <summary>Runs an ADB command and returns stdout (trimmed).</summary>
        private static string RunAdb(string arguments)
        {
            using var p = new DiagProcess
            {
                StartInfo = new DiagProcessInfo
                {
                    FileName               = "adb",
                    Arguments              = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                }
            };
            p.Start();
            string output = p.StandardOutput.ReadToEnd().Trim();
            p.WaitForExit();
            return output;
        }

        /// <summary>Queries a single Android global setting via ADB. Returns null if unset.</summary>
        private static string? GetAdbSetting(string settingName)
        {
            try
            {
                string v = RunAdb($"shell settings get global {settingName}");
                return string.IsNullOrEmpty(v) || v.Equals("null", StringComparison.OrdinalIgnoreCase) ? null : v;
            }
            catch { return null; }
        }

        /// <summary>Returns true if the given package has a running process on the connected device.</summary>
        public static bool IsAppRunningOnDevice(string packageName)
        {
            try
            {
                string output = RunAdb($"shell pidof {packageName}");
                return !string.IsNullOrEmpty(output) && uint.TryParse(output, out _);
            }
            catch { return false; }
        }

        /// <summary>Grants an Android runtime permission to the given package via ADB.</summary>
        public static bool GrantPermission(string packageName, string permission)
        {
            try
            {
                using var p = new DiagProcess
                {
                    StartInfo = new DiagProcessInfo
                    {
                        FileName               = "adb",
                        Arguments              = $"shell pm grant {packageName} {permission}",
                        RedirectStandardOutput = true,
                        RedirectStandardError  = true,
                        UseShellExecute        = false,
                        CreateNoWindow         = true
                    }
                };
                p.Start();
                p.StandardOutput.ReadToEnd();
                string err = p.StandardError.ReadToEnd();
                p.WaitForExit();
                return p.ExitCode == 0 || err.Contains("is not a changeable permission");
            }
            catch { return false; }
        }

        /// <summary>
        /// Logs a detailed StartApp timeout diagnosis: app running state,
        /// INTERNET permission, and ADB device connectivity.
        /// </summary>
        public static void DiagnoseAppLaunchIssue(string packageName, string activity)
        {
            AppLogger.Info(Ctx, "=== StartApp Timeout Diagnostic ===");
            try
            {
                bool isRunning = IsAppRunningOnDevice(packageName);
                AppLogger.Info(Ctx, $"App running on device: {isRunning}");

                if (isRunning)
                {
                    AppLogger.Warn(Ctx, "App launched but SDPCore cannot connect â€” possible causes:");
                    AppLogger.Warn(Ctx, "  1. App lacks INTERNET permission");
                    AppLogger.Warn(Ctx, "  2. Network issue between profiler and app");
                    AppLogger.Warn(Ctx, "  3. App crashed before profiler connected");
                }
                else
                {
                    AppLogger.Warn(Ctx, "App NOT running â€” launch failed");
                    AppLogger.Warn(Ctx, $"  Try manual: adb shell am start -n {packageName}/{activity}");
                }

                string dumpsys    = RunAdb($"shell dumpsys package {packageName}");
                bool hasInternet  = dumpsys.Contains("android.permission.INTERNET") &&
                                    dumpsys.Contains("granted=true");
                bool hasStorage   = dumpsys.Contains("WRITE_EXTERNAL_STORAGE") &&
                                    dumpsys.Contains("granted=true");
                AppLogger.Info(Ctx, $"INTERNET permission        : {(hasInternet ? "Granted"     : "Not granted")}");
                AppLogger.Info(Ctx, $"WRITE_EXTERNAL_STORAGE     : {(hasStorage  ? "Granted"     : "Not granted")}");
                if (!hasInternet) AppLogger.Warn(Ctx, "INTERNET permission missing â€” required for SDK StartApp");

                string devices = RunAdb("devices -l");
                AppLogger.Info(Ctx, devices.Contains("device")
                    ? "ADB device connected"
                    : "No ADB device connected!");

                AppLogger.Info(Ctx, "===================================");
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"Diagnostic failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads recent logcat entries and logs lines matching <paramref name="filterTag"/>.
        /// </summary>
        public static void DumpRecentLogcat(string filterTag = "SDPCore|sdp", int lines = 100)
        {
            try
            {
                AppLogger.Info(Ctx, $"=== Recent logcat (last {lines} lines, filter: {filterTag}) ===");
                string raw = RunAdb($"logcat -d -t {lines} *:W");
                var matched = raw.Split('\n')
                    .Where(l => Regex.IsMatch(l, filterTag, RegexOptions.IgnoreCase))
                    .ToList();

                if (matched.Count > 0)
                    foreach (var l in matched) AppLogger.Info(Ctx, $"  {l}");
                else
                    AppLogger.Info(Ctx, $"  (no matches for: {filterTag})");

                AppLogger.Info(Ctx, "=====================================================");
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"DumpRecentLogcat failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts a background logcat process, streaming output to the console
        /// and appending to <c>logcat_debug.txt</c>.
        /// </summary>
        public static DiagProcess? StartLogcatMonitor(string filterTag = "SDPCore|sdp")
        {
            try
            {
                AppLogger.Info(Ctx, $"Starting logcat monitor (filter: {filterTag}) â€” output â†’ logcat_debug.txt");

                var logcatProc = new DiagProcess
                {
                    StartInfo = new DiagProcessInfo
                    {
                        FileName               = "adb",
                        Arguments              = $"logcat -v time *:W | findstr /I \"{filterTag}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError  = true,
                        UseShellExecute        = false,
                        CreateNoWindow         = false
                    }
                };

                var logFile = new StreamWriter("logcat_debug.txt", append: true) { AutoFlush = true };

                logcatProc.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        AppLogger.Debug("Logcat", e.Data);
                        logFile.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {e.Data}");
                    }
                };
                logcatProc.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        AppLogger.Warn("Logcat", e.Data);
                        logFile.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ERR: {e.Data}");
                    }
                };

                logcatProc.Start();
                logcatProc.BeginOutputReadLine();
                logcatProc.BeginErrorReadLine();
                return logcatProc;
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"StartLogcatMonitor failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Reads the four Vulkan debug-layer Android global settings via ADB and
        /// verifies they are all active. Returns true only when all settings are correct.
        /// </summary>
        public static bool VerifyVulkanLayerSettings()
        {
            try
            {
                AppLogger.Info(Ctx, "=== POST-LAUNCH Vulkan Layer Verification ===");

                string? enableGpu   = GetAdbSetting("enable_gpu_debug_layers");
                string? gpuLayers   = GetAdbSetting("gpu_debug_layers");
                string? gpuApp      = GetAdbSetting("gpu_debug_app");
                string? gpuLayerApp = GetAdbSetting("gpu_debug_layer_app");

                AppLogger.Info(Ctx, "Current device settings:");
                AppLogger.Info(Ctx, $"  enable_gpu_debug_layers = {enableGpu   ?? "(not set)"}");
                AppLogger.Info(Ctx, $"  gpu_debug_layers        = {gpuLayers   ?? "(not set)"}");
                AppLogger.Info(Ctx, $"  gpu_debug_app           = {gpuApp      ?? "(not set)"}");
                AppLogger.Info(Ctx, $"  gpu_debug_layer_app     = {gpuLayerApp ?? "(not set)"}");

                bool ok = enableGpu == "1"
                       && !string.IsNullOrEmpty(gpuLayers)
                       && !string.IsNullOrEmpty(gpuApp)
                       && !string.IsNullOrEmpty(gpuLayerApp);

                if (ok)
                {
                    AppLogger.Success(Ctx, $"All Vulkan layer settings active â€” layer: {gpuLayers}");
                }
                else
                {
                    if (enableGpu != "1")              AppLogger.Warn(Ctx, "  enable_gpu_debug_layers not set to 1");
                    if (string.IsNullOrEmpty(gpuLayers))   AppLogger.Warn(Ctx, "  gpu_debug_layers not set");
                    if (string.IsNullOrEmpty(gpuApp))      AppLogger.Warn(Ctx, "  gpu_debug_app not set");
                    if (string.IsNullOrEmpty(gpuLayerApp)) AppLogger.Warn(Ctx, "  gpu_debug_layer_app not set");
                    AppLogger.Warn(Ctx, "Vulkan layer settings incomplete â€” frame capture may fail");
                }

                AppLogger.Info(Ctx, "=============================================");
                return ok;
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"VerifyVulkanLayerSettings failed: {ex.Message}");
                return false;
            }
        }

        // â”€â”€ Permission helpers â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        /// <summary>
        /// Inspects the package via <c>adb shell dumpsys</c> and auto-grants any declared
        /// runtime permissions that are not yet granted.
        /// </summary>
        /// <returns>(isDebuggable, hasInternetPermission)</returns>
        public static (bool isDebuggable, bool hasInternetPermission) CheckAndGrantPermissions(
            string packageName, bool isDeviceRooted)
        {
            bool isDebuggable         = false;
            bool hasInternetPermission = false;
            try
            {
                AppLogger.Info(Ctx, $"Checking permissions for: {packageName}");

                string dumpsys = RunAdb($"shell dumpsys package {packageName}");

                isDebuggable = isDeviceRooted ||
                               Regex.IsMatch(dumpsys, @"flags=\[\s*\S*\s*DEBUGGABLE");
                AppLogger.Info(Ctx, $"Debuggable                   : {isDebuggable}");

                hasInternetPermission = dumpsys.Contains("android.permission.INTERNET");
                bool hasWrite = dumpsys.Contains("android.permission.WRITE_EXTERNAL_STORAGE");
                bool hasRead  = dumpsys.Contains("android.permission.READ_EXTERNAL_STORAGE");
                AppLogger.Info(Ctx, $"INTERNET declared            : {hasInternetPermission}");
                AppLogger.Info(Ctx, $"WRITE_EXTERNAL_STORAGE decl. : {hasWrite}");
                AppLogger.Info(Ctx, $"READ_EXTERNAL_STORAGE decl.  : {hasRead}");

                void TryGrant(string perm, string label)
                {
                    bool granted = Regex.IsMatch(dumpsys,
                        perm.Replace(".", @"\.") + @":.*?granted=true",
                        RegexOptions.Singleline);
                    if (granted)
                    {
                        AppLogger.Info(Ctx, $"  {label}: already granted");
                    }
                    else
                    {
                        bool ok = GrantPermission(packageName, perm);
                        if (ok) AppLogger.Info(Ctx, $"  {label}: granted");
                        else    AppLogger.Warn(Ctx, $"  {label}: grant failed â€” may require manual action");
                    }
                }

                if (hasWrite) TryGrant("android.permission.WRITE_EXTERNAL_STORAGE", "WRITE_EXTERNAL_STORAGE");
                if (hasRead)  TryGrant("android.permission.READ_EXTERNAL_STORAGE",  "READ_EXTERNAL_STORAGE");

                if (!hasInternetPermission)
                    AppLogger.Warn(Ctx, "INTERNET permission not declared â€” SDK StartApp requires it. Add to AndroidManifest.xml.");
            }
            catch (Exception ex)
            {
                AppLogger.Warn(Ctx, $"CheckAndGrantPermissions failed: {ex.Message}");
            }
            return (isDebuggable, hasInternetPermission);
        }
    }
}
