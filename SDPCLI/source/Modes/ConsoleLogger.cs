using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Default ILogger that routes to AppLogger with "App" context.
    /// Use ContextLogger with a meaningful name instead where possible.
    /// </summary>
    public class ConsoleLogger : ContextLogger
    {
        public ConsoleLogger() : base("App") { }
    }
}
