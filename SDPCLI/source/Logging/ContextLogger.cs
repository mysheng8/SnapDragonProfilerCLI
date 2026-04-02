using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Logging
{
    /// <summary>
    /// ILogger implementation that routes through AppLogger with a fixed context string.
    ///
    /// Inject this into Analysis services instead of the old ConsoleLogger:
    ///   ILogger logger = new ContextLogger("DrawCallAnalysis");
    /// </summary>
    public class ContextLogger : ILogger
    {
        private readonly string _context;

        public ContextLogger(string context)
        {
            _context = context;
        }

        public void Info(string message)    => AppLogger.Info   (_context, message);
        public void Warning(string message) => AppLogger.Warn   (_context, message);
        public void Error(string message)   => AppLogger.Error  (_context, message);
        public void Debug(string message)   => AppLogger.Debug  (_context, message);
        public void Success(string message) => AppLogger.Success (_context, message);
    }
}
