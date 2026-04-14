using System;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Simple DeviceDelegate implementation to handle device events
    /// </summary>
    public class CliDeviceDelegate : DeviceDelegate
    {
        public override void OnDeviceConnected(string name)
        {
            AppLogger.Info("DeviceDelegate", $"Device connected: {name}");
        }

        public override void OnDeviceDisconnected(string name)
        {
            AppLogger.Info("DeviceDelegate", $"Device disconnected: {name}");
        }

        public override void OnDeviceStateChanged(string name)
        {
            AppLogger.Info("DeviceDelegate", $"Device state changed: {name}");
        }
    }
}
