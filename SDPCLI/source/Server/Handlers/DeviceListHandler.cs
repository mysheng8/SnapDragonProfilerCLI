using System;
using System.Collections.Generic;
using System.Net;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// GET /api/devices — lists connected ADB devices.
    ///
    /// Returns 200 + { ok, data: [{ serial, state }] }
    /// </summary>
    public class DeviceListHandler : BaseHandler
    {
        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "GET") { WriteError(ctx, "Method not allowed", 405); return; }

            try
            {
                var devices = new List<object>();

                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName               = "adb",
                        Arguments              = "devices",
                        RedirectStandardOutput = true,
                        RedirectStandardError  = true,
                        UseShellExecute        = false,
                        CreateNoWindow         = true,
                    }
                };
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit(5000);

                // Parse "adb devices" output — skip header line and empty lines
                foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.StartsWith("List of") || line.StartsWith("*"))
                        continue;
                    var parts = line.Split(new[] { '\t' }, 2);
                    if (parts.Length < 2) continue;
                    devices.Add(new { serial = parts[0].Trim(), state = parts[1].Trim() });
                }

                WriteOk(ctx, devices);
            }
            catch (Exception ex)
            {
                WriteError(ctx, $"adb devices failed: {ex.Message}", 500);
            }
        }
    }
}
