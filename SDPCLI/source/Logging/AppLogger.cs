using System;
using System.IO;
using System.Text;
using System.Threading;

namespace SnapdragonProfilerCLI.Logging
{
    /// <summary>
    /// Central static logger for the entire application.
    ///
    /// Two output channels:
    ///   File   — always written, every level, with thread ID in debug mode.
    ///   Console— only written when level >= MinConsoleLevel (config-driven).
    ///
    /// Format:
    ///   [LEVEL][HH:mm:ss.fff][Context      ] message
    ///   [DEBUG][HH:mm:ss.fff][T:08][Context] message   (debug mode adds thread ID)
    ///
    /// User-facing interaction (menus, prompts, "Press ENTER") should continue to
    /// use plain Console.Write/WriteLine — those are NOT routed through AppLogger.
    /// </summary>
    public static class AppLogger
    {
        // ── Configuration ─────────────────────────────────────────────────────
        public static AppLogLevel MinConsoleLevel { get; private set; } = AppLogLevel.Info;
        public static bool DebugMode             { get; private set; } = false;

        private static readonly object _lock = new object();
        private static StreamWriter?   _fileWriter;
        private static string?         _logPath;

        // Rotation: rename file when it exceeds this size (bytes)
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        // ── Initialisation ────────────────────────────────────────────────────

        /// <summary>
        /// Call once from Main before anything else.
        /// </summary>
        public static void Initialize(string logPath, bool debugMode = false)
        {
            lock (_lock)
            {
                _logPath  = logPath;
                DebugMode = debugMode;
                MinConsoleLevel = debugMode ? AppLogLevel.Debug : AppLogLevel.Info;

                try
                {
                    string? dir = Path.GetDirectoryName(logPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    RotateIfNeeded(logPath);
                    _fileWriter = new StreamWriter(logPath, append: true, Encoding.UTF8)
                    { AutoFlush = true };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AppLogger] Failed to open log file: {ex.Message}");
                }
            }
        }

        /// <summary>Flush and close the log file. Call from Main finally block.</summary>
        public static void Shutdown()
        {
            lock (_lock)
            {
                try { _fileWriter?.Flush(); _fileWriter?.Dispose(); }
                catch { }
                _fileWriter = null;
            }
        }

        // ── Public log methods ─────────────────────────────────────────────────

        public static void Debug(string context, string message) =>
            Write(AppLogLevel.Debug, context, message);

        public static void Info(string context, string message) =>
            Write(AppLogLevel.Info, context, message);

        public static void Success(string context, string message) =>
            Write(AppLogLevel.Success, context, message);

        public static void Warn(string context, string message) =>
            Write(AppLogLevel.Warn, context, message);

        public static void Error(string context, string message) =>
            Write(AppLogLevel.Error, context, message);

        /// <summary>
        /// Logs an exception. Short summary to console; full stack trace to file only.
        /// </summary>
        public static void Exception(string context, Exception ex, string? note = null)
        {
            string summary = note != null
                ? $"{note} — {ex.GetType().Name}: {ex.Message}"
                : $"{ex.GetType().Name}: {ex.Message}";

            Write(AppLogLevel.Error, context, summary);

            // Full detail to file only
            if (ex.StackTrace != null)
                WriteFileOnly(context, $"  Stack: {ex.StackTrace.Replace("\n", "\n  ")}");

            if (ex.InnerException != null)
                WriteFileOnly(context, $"  Inner: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }

        /// <summary>
        /// Write a separator / banner line (goes to both channels at INFO level).
        /// </summary>
        public static void Banner(string text)
        {
            string line = $"=== {text} ===";
            Write(AppLogLevel.Info, "", line);
        }

        // ── Core write ─────────────────────────────────────────────────────────

        public static void Write(AppLogLevel level, string context, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string levelTag  = LevelTag(level);
            string ctx       = PadContext(context);

            string fileLine, consoleLine;

            if (DebugMode)
            {
                int tid = Thread.CurrentThread.ManagedThreadId;
                fileLine    = $"[{levelTag}][{timestamp}][T:{tid:D2}][{ctx}] {message}";
                consoleLine = fileLine;
            }
            else
            {
                fileLine    = $"[{levelTag}][{timestamp}][{ctx}] {message}";
                consoleLine = fileLine;
            }

            lock (_lock)
            {
                // Always write to file
                try { _fileWriter?.WriteLine(fileLine); }
                catch { }

                // Write to console only if level passes filter
                if (level >= MinConsoleLevel)
                    WriteConsole(level, consoleLine);
            }
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static void WriteFileOnly(string context, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            lock (_lock)
            {
                try { _fileWriter?.WriteLine($"[     ][{timestamp}][{PadContext(context)}] {message}"); }
                catch { }
            }
        }

        private static void WriteConsole(AppLogLevel level, string line)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = level switch
            {
                AppLogLevel.Error   => ConsoleColor.Red,
                AppLogLevel.Warn    => ConsoleColor.Yellow,
                AppLogLevel.Success => ConsoleColor.Green,
                AppLogLevel.Debug   => ConsoleColor.DarkGray,
                _                   => ConsoleColor.White
            };
            Console.WriteLine(line);
            Console.ForegroundColor = prev;
        }

        private static string LevelTag(AppLogLevel level) => level switch
        {
            AppLogLevel.Debug   => "DEBUG",
            AppLogLevel.Info    => "INFO ",
            AppLogLevel.Success => "OK   ",
            AppLogLevel.Warn    => "WARN ",
            AppLogLevel.Error   => "ERROR",
            _                   => "LOG  "
        };

        // Pad/truncate context to fixed width for aligned columns
        private const int ContextWidth = 16;
        private static string PadContext(string ctx) =>
            ctx.Length >= ContextWidth ? ctx.Substring(0, ContextWidth) : ctx.PadRight(ContextWidth);

        private static void RotateIfNeeded(string logPath)
        {
            if (!File.Exists(logPath)) return;
            try
            {
                var info = new FileInfo(logPath);
                if (info.Length < MaxFileSizeBytes) return;

                string backup = Path.ChangeExtension(logPath, ".1.txt");
                if (File.Exists(backup)) File.Delete(backup);
                File.Move(logPath, backup);
            }
            catch { /* best-effort */ }
        }
    }
}
