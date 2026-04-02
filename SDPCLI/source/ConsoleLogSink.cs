using System.Text.RegularExpressions;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Routes SDPCore C++ internal logs through AppLogger.
    /// In non-debug mode only WARN/ERROR are forwarded to console; DEBUG/INFO go to file only.
    /// Also captures the TCP port allocation message for DeviceConnectionService.
    /// </summary>
    public class ConsoleLogSink : Logger.LogSink
    {
        public static uint LastDetectedNetworkPort { get; private set; } = 0;

        public static void ResetCapturedPort() => LastDetectedNetworkPort = 0;

        private static readonly Regex portRegex =
            new Regex(@"listening on tcp://[^:]+:(\d+)", RegexOptions.Compiled);

        public ConsoleLogSink() : base(LogLevel.LOG_DEBUG) { }

        public override string GetName() => "ConsoleLogSink";

        public override void Init() =>
            AppLogger.Debug("ConsoleLogSink", "SDPCore log sink initialized");

        public override void Write(LogLevel level, string tag, string output)
        {
            // Map SDPCore level to AppLogLevel
            AppLogLevel appLevel = level switch
            {
                LogLevel.LOG_ERROR => AppLogLevel.Error,
                LogLevel.LOG_WARN  => AppLogLevel.Warn,
                LogLevel.LOG_INFO  => AppLogLevel.Info,
                _                  => AppLogLevel.Debug
            };

            // In non-debug mode suppress INFO/DEBUG from console (still written to file)
            AppLogLevel effectiveConsoleMin = AppLogger.DebugMode
                ? AppLogLevel.Debug
                : AppLogLevel.Warn;

            AppLogLevel savedMin = AppLogger.MinConsoleLevel;
            // Temporarily override for SDPCore noise suppression
            // We write directly so we can control the context display
            string context = tag.Length > 0 ? $"SDPCore.{tag}" : "SDPCore";
            if (context.Length > 20) context = context.Substring(0, 20);

            // Route: always file, console only if level >= suppress threshold
            AppLogLevel consoleThreshold = AppLogger.DebugMode ? AppLogLevel.Debug : AppLogLevel.Warn;
            if (appLevel >= consoleThreshold)
                AppLogger.Write(appLevel, context, output);
            else
                AppLogger.Write(AppLogLevel.Debug, context, output); // goes to file via DEBUG path

            // Capture port allocation
            if (tag == "SDPCore.NetCommandServer" && output.Contains("listening on tcp://"))
            {
                var match = portRegex.Match(output);
                if (match.Success && uint.TryParse(match.Groups[1].Value, out uint port))
                    LastDetectedNetworkPort = port;
            }
        }
    }
}
