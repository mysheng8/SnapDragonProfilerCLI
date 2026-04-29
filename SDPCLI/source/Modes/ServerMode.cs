using System;
using System.Threading;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Modes;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server
{
    /// <summary>
    /// IMode implementation for the HTTP server subcommand.
    ///
    /// Lifecycle:
    ///   1. Create DeviceSession and JobManager
    ///   2. Start HttpServer (binds to localhost:port)
    ///   3. Block on console input — 'q' or Ctrl+C triggers graceful shutdown
    ///   4. Stop server, purge jobs, return
    /// </summary>
    public class ServerMode : IMode
    {
        private readonly Config _config;
        private readonly string _outputPath;
        private readonly int    _port;
        private readonly string _host;   // reserved — always "localhost" for security

        private static readonly ContextLogger _log = new ContextLogger("ServerMode");

        public string Name        => "Server";
        public string Description => "Run a local HTTP REST API server for remote control";

        public ServerMode(Config config, string outputPath, int port = 5000, string host = "localhost")
        {
            _config     = config;
            _outputPath = outputPath;
            _port       = port;
            _host       = host;
        }

        public void Run()
        {
            _log.Info("\n=== Server Mode ===\n");
            _log.Info($"  Port        : {_port}");
            _log.Info($"  JobTTL      : {_config.GetInt("Server.JobTtlMinutes", 60)} min");
            _log.Info($"  Press 'q' or Ctrl+C to stop.\n");

            int     jobTtlMinutes = _config.GetInt("Server.JobTtlMinutes", 60);
            var     session       = new DeviceSession();
            var     jobManager    = new JobManager(jobTtlMinutes);
            HttpServer? server    = null;

            // Ctrl+C / SIGTERM → graceful shutdown
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _log.Info("Shutdown signal received...");
                cts.Cancel();
            };

            // Suppress SDK ExitApplication in server mode — just disconnect, keep server running
            session.SdkExitAction = () =>
            {
                _log.Warning("SDK requested ExitApplication — suppressed in server mode, disconnecting only");
                try { session.Disconnect(); } catch { /* ignore */ }
            };

            try
            {
                server = new HttpServer(_port, session, jobManager, _config);
                server.Start();

                // Block until quit signal
                while (!cts.IsCancellationRequested)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true);
                        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                        {
                            _log.Info("Quit key pressed.");
                            break;
                        }
                    }
                    Thread.Sleep(100);

                    // Periodic TTL purge
                    jobManager.PurgeExpired();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Server error: " + ex.Message);
            }
            finally
            {
                _log.Info("Stopping server...");
                server?.Stop();
                server?.Dispose();

                // Cancel all running jobs
                foreach (var job in jobManager.List())
                    jobManager.Cancel(job.Id);

                // Disconnect device if connected
                if (session.Status != DeviceStatus.Disconnected)
                    session.Disconnect();

                _log.Info("Server shutdown complete.");
            }
        }
    }
}
