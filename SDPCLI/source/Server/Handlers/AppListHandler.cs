using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// GET /api/app/packages              — list all installed packages on the connected device.
    /// GET /api/app/activities?package=X  — list activities for a given package.
    ///
    /// Query params:
    ///   serial  (optional) — ADB device serial to target
    ///   package (required for /activities)
    ///
    /// Returns 200 + { ok, data: [...] }
    /// </summary>
    public class AppListHandler : BaseHandler
    {
        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "GET") { WriteError(ctx, "Method not allowed", 405); return; }

            string path   = ctx.Request.Url?.AbsolutePath.TrimEnd('/') ?? "";
            string serial = ctx.Request.QueryString["serial"] ?? "";
            string adbPrefix = string.IsNullOrEmpty(serial) ? "adb" : $"adb -s {serial}";

            if (path == "/api/app/packages")
            {
                ListPackages(ctx, adbPrefix);
            }
            else if (path == "/api/app/activities")
            {
                string? package = ctx.Request.QueryString["package"];
                if (string.IsNullOrWhiteSpace(package))
                {
                    WriteError(ctx, "package query param is required", 400);
                    return;
                }
                ListActivities(ctx, adbPrefix, package!);
            }
            else
            {
                WriteError(ctx, $"No route for {path}", 404);
            }
        }

        private void ListPackages(HttpListenerContext ctx, string adbPrefix)
        {
            try
            {
                string output = RunAdb(adbPrefix, "shell pm list packages -3");
                var packages = new List<string>();
                foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // "package:com.example.app"
                    if (line.StartsWith("package:"))
                        packages.Add(line.Substring("package:".Length).Trim());
                }
                packages.Sort();
                WriteOk(ctx, packages);
            }
            catch (Exception ex)
            {
                WriteError(ctx, $"pm list packages failed: {ex.Message}", 500);
            }
        }

        private void ListActivities(HttpListenerContext ctx, string adbPrefix, string package)
        {
            try
            {
                // dumpsys package <pkg> | grep -o 'packageName/activityName'
                string output = RunAdb(adbPrefix,
                    $"shell \"dumpsys package {package} | grep -o '[^ ]*/[^ :}}]*' | sort | uniq\"");

                var activities = new List<string>();
                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var regex = new Regex(@"^[A-Za-z0-9_.]+/[A-Za-z0-9_.]+$");

                foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = line.Trim();
                    if (regex.IsMatch(trimmed) && seen.Add(trimmed))
                        activities.Add(trimmed);
                }

                WriteOk(ctx, activities);
            }
            catch (Exception ex)
            {
                WriteError(ctx, $"dumpsys failed: {ex.Message}", 500);
            }
        }

        private static string RunAdb(string adbPrefix, string args)
        {
            // adbPrefix is either "adb" or "adb -s <serial>" — split into exe + fixed args
            string exe;
            string fullArgs;
            if (adbPrefix == "adb")
            {
                exe      = "adb";
                fullArgs = args;
            }
            else
            {
                // "adb -s <serial>"
                exe      = "adb";
                fullArgs = adbPrefix.Substring("adb".Length).Trim() + " " + args;
            }

            var proc = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName               = exe,
                    Arguments              = fullArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true,
                }
            };
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit(10000);
            return output;
        }
    }
}
