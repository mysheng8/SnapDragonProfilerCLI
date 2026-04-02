using System;
using System.IO;

namespace Sdp
{
    /// <summary>
    /// File-based LogSink for GUI application to capture SDPCore.dll internal logs
    /// </summary>
    public class FileLogSink : Logger.LogSink
    {
        private readonly string logFilePath;
        private readonly object lockObject = new object();

        public FileLogSink(string logPath = null) : base(LogLevel.LOG_DEBUG)
        {
            if (string.IsNullOrEmpty(logPath))
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string logDir = Path.Combine(appData, "Qualcomm", "SnapdragonProfiler", "Logs");
                Directory.CreateDirectory(logDir);
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                logFilePath = Path.Combine(logDir, $"SDPCore_{timestamp}.log");
            }
            else
            {
                logFilePath = logPath;
                string dir = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        public override string GetName()
        {
            return "FileLogSink";
        }

        public override void Init()
        {
            lock (lockObject)
            {
                try
                {
                    File.AppendAllText(logFilePath, $"=== FileLogSink Initialized at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==={Environment.NewLine}");
                    File.AppendAllText(logFilePath, $"Log file: {logFilePath}{Environment.NewLine}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[FileLogSink] Init error: {ex.Message}");
                }
            }
        }

        public override void Write(LogLevel level, string tag, string output)
        {
            lock (lockObject)
            {
                try
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                    string levelStr = level switch
                    {
                        LogLevel.LOG_DEBUG => "DEBUG",
                        LogLevel.LOG_INFO => "INFO ",
                        LogLevel.LOG_WARN => "WARN ",
                        LogLevel.LOG_ERROR => "ERROR",
                        _ => "?????",
                    };

                    string logLine = $"[{timestamp}][{levelStr}][{tag}] {output}{Environment.NewLine}";
                    File.AppendAllText(logFilePath, logLine);
                }
                catch
                {
                    // Suppress file write errors to avoid GUI freezing
                }
            }
        }

        public string GetLogFilePath()
        {
            return logFilePath;
        }
    }
}
