using System;
using System.Collections.Generic;

namespace SnapdragonProfilerCLI.Server
{
    public enum DeviceStatus
    {
        Disconnected,
        Connecting,
        Connected,
        Launching,
        SessionActive,
        Capturing
    }

    public class DeviceSessionInfo
    {
        public string      SessionId       { get; }
        public DateTime    CreatedAt       { get; }
        public string      PackageActivity { get; }
        public uint        Pid             { get; set; }
        public List<uint>  CaptureIds      { get; } = new List<uint>();

        public DeviceSessionInfo(string packageActivity, uint pid)
        {
            SessionId       = "sess-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
            CreatedAt       = DateTime.UtcNow;
            PackageActivity = packageActivity;
            Pid             = pid;
        }
    }
}
