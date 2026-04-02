using System;
using System.IO;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Handles device connectivity: APK installation and device connection.
    /// Extracted from Application.CheckAndInstallProfilerAPKs / ConnectToDevice.
    /// </summary>
    public class DeviceConnectionService
    {
        private readonly Config _config;
        private readonly Func<string?, string?> _readLine;

        public DeviceConnectionService(Config config, Func<string?, string?> readLine)
        {
            _config = config;
            _readLine = readLine;
        }

        /// <summary>Checks ADB connectivity and installs the two Profiler APK variants.</summary>
        public bool CheckAndInstallAPKs()
        {
            Console.WriteLine("\n=== Installing Profiler APKs ===");

            try
            {
                Console.WriteLine("Checking device connectivity...");
                var adbDevices = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = "devices",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                adbDevices.Start();
                string devicesOutput = adbDevices.StandardOutput.ReadToEnd();
                adbDevices.WaitForExit();

                if (!devicesOutput.Contains("device") || devicesOutput.Split('\n').Length < 3)
                {
                    Console.WriteLine("✗ No device connected via ADB");
                    Console.WriteLine("  Please connect a device and enable USB debugging.");
                    return false;
                }
                Console.WriteLine("✓ Device connected");

                string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
                string sdpcliRoot = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", ".."));

                string[] architectures = { "arm64-v8a", "armeabi-v7a" };
                int installedCount = 0;

                foreach (string arch in architectures)
                {
                    string apkPath = Path.Combine(sdpcliRoot, "android", arch, "apk", $"app-{arch}-release.apk");

                    if (!File.Exists(apkPath))
                    {
                        Console.WriteLine($"\n⚠ APK not found: {apkPath}");
                        continue;
                    }

                    Console.WriteLine($"\nInstalling {arch} APK...");

                    var adbInstall = new System.Diagnostics.Process
                    {
                        StartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "adb",
                            Arguments = $"install -r \"{apkPath}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    adbInstall.Start();
                    string installOutput = adbInstall.StandardOutput.ReadToEnd();
                    adbInstall.WaitForExit();

                    if (installOutput.Contains("Success") || adbInstall.ExitCode == 0)
                    {
                        Console.WriteLine($"  ✓ {arch} APK installed");
                        installedCount++;
                    }
                    else
                    {
                        Console.WriteLine($"  ⚠ {arch} APK installation failed: {installOutput.Trim()}");
                    }
                }

                Console.WriteLine("\nVerifying installation...");
                var adbCheck = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = "shell pm list packages com.qualcomm.snapdragonprofiler",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                adbCheck.Start();
                string installedPackages = adbCheck.StandardOutput.ReadToEnd();
                adbCheck.WaitForExit();

                int verifiedCount = 0;
                if (installedPackages.Contains("arm64_v8a"))
                {
                    Console.WriteLine("  ✓ com.qualcomm.snapdragonprofiler.profilerlayer.arm64_v8a");
                    verifiedCount++;
                }
                if (installedPackages.Contains("armeabi_v7a"))
                {
                    Console.WriteLine("  ✓ com.qualcomm.snapdragonprofiler.profilerlayer.armeabi_v7a");
                    verifiedCount++;
                }

                if (verifiedCount > 0)
                {
                    Console.WriteLine($"\n✓ Profiler APKs ready ({verifiedCount} variant(s) installed)");
                    Console.WriteLine("Ready to connect to device.\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("\n✗ No Profiler APK variants installed");
                    Console.WriteLine("  SDPCore may attempt to install required components during connection.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error during APK installation: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Connects to a device using SDPClient.
        /// Returns the connected Device, or null on failure.
        /// </summary>
        public Device? Connect(SDPClient sdpClient)
        {
            Console.WriteLine("\n=== Connecting to Device ===");

            try
            {
                if (sdpClient == null || !sdpClient.IsInitialized)
                {
                    Console.WriteLine("✗ ERROR: SDPClient not initialized");
                    return null;
                }

                string? deviceSerial = _config.Get("DeviceSerial");
                uint basePort = (uint)_config.GetInt("BasePort", 6500);

                var connectedDevice = sdpClient.SelectAndConnectDevice(
                    deviceSerial,
                    basePort,
                    30,
                    _readLine);

                if (connectedDevice == null)
                {
                    Console.WriteLine("✗ Failed to connect to device");
                    return null;
                }

                try
                {
                    string? stateMsg = connectedDevice.GetDeviceStateMsg();
                    if (!string.IsNullOrEmpty(stateMsg))
                        Console.WriteLine($"Device state message: {stateMsg}");
                }
                catch { }

                return connectedDevice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during device connection: {ex.Message}");
                Console.WriteLine($"   Exception Type: {ex.GetType().Name}");

                if (ex.InnerException != null)
                    Console.WriteLine($"   Inner Exception: {ex.InnerException.Message}");

                Console.WriteLine("\n=== Troubleshooting ===");
                Console.WriteLine("1. Ensure device is connected: adb devices");
                Console.WriteLine("2. Check ADB daemon: adb kill-server && adb start-server");
                Console.WriteLine("3. Enable USB debugging on device");
                Console.WriteLine("4. Check device logs: adb logcat -d | findstr SDPCore");

                return null;
            }
        }
    }
}
