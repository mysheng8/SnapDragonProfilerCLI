using System;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Simple DeviceDelegate implementation to handle device events
    /// </summary>
    public class CliDeviceDelegate : DeviceDelegate
    {
        public override void OnDeviceConnected(string name)
        {
            Console.WriteLine($"[DeviceDelegate] Device connected: {name}");
        }

        public override void OnDeviceDisconnected(string name)
        {
            Console.WriteLine($"[DeviceDelegate] Device disconnected: {name}");
        }

        public override void OnDeviceStateChanged(string name)
        {
            Console.WriteLine($"[DeviceDelegate] Device state changed: {name}");
        }
    }
}
