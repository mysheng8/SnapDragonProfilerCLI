using System;
using System.Net;
using System.Threading;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Handlers;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server
{
    /// <summary>
    /// Minimal HTTP/1.1 server built on System.Net.HttpListener.
    /// Binds to localhost only (security requirement — no external access).
    ///
    /// Route dispatch: exact path prefix matching.
    ///   GET  /api/status               → StatusHandler
    ///   GET  /api/device               → DeviceHandler (current session state)
    ///   GET  /api/devices              → DeviceListHandler (adb devices)
    ///   GET  /api/app/packages         → AppListHandler (pm list packages)
    ///   GET  /api/app/activities       → AppListHandler (dumpsys activities for ?package=)
    ///   POST /api/connect              → ConnectHandler
    ///   POST /api/disconnect           → DisconnectHandler
    ///   POST /api/session/launch       → LaunchHandler
    ///   POST /api/capture              → CaptureHandler
    ///   POST /api/analysis             → AnalysisHandler
    ///   GET|DELETE /api/jobs           → JobsHandler (list)
    ///   GET|DELETE /api/jobs/{id}      → JobsHandler (single)
    /// </summary>
    public class HttpServer : IDisposable
    {
        private readonly HttpListener        _listener;
        private readonly HandlerRouter       _router;
        private readonly ContextLogger       _log    = new ContextLogger("HttpServer");
        private          Thread?             _thread;
        private volatile bool                _running;

        public HttpServer(int port, DeviceSession session, JobManager jobManager, Config config)
        {
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be 1-65535");

            _listener = new HttpListener();
            // Bind to localhost only — never accept remote connections
            _listener.Prefixes.Add($"http://localhost:{port}/");

            _router = new HandlerRouter(session, jobManager, config);
        }

        /// <summary>Start the listener and begin accepting requests (non-blocking).</summary>
        public void Start()
        {
            _listener.Start();
            _running = true;
            _thread  = new Thread(AcceptLoop) { IsBackground = true, Name = "HttpServer" };
            _thread.Start();
            _log.Info("Listening on " + string.Join(", ", _listener.Prefixes));
        }

        /// <summary>Stop the listener and wait for the accept loop to exit.</summary>
        public void Stop()
        {
            _running = false;
            try { _listener.Stop(); } catch { /* ignore */ }
            _thread?.Join(TimeSpan.FromSeconds(3));
            _log.Info("Server stopped");
        }

        public void Dispose()
        {
            Stop();
            _listener.Close();
        }

        // ── Accept loop ───────────────────────────────────────────────────────

        private void AcceptLoop()
        {
            while (_running)
            {
                HttpListenerContext ctx;
                try
                {
                    ctx = _listener.GetContext();
                }
                catch (HttpListenerException) when (!_running)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (_running) _log.Error("Accept error: " + ex.Message);
                    continue;
                }

                // Each request handled on a thread-pool thread to keep accept loop hot.
                // Handlers are expected to be fast (job submission, not long blocking work).
                System.Threading.ThreadPool.QueueUserWorkItem(_ => HandleRequest(ctx));
            }
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            try
            {
                _log.Debug($"{ctx.Request.HttpMethod} {ctx.Request.Url?.PathAndQuery}");
                _router.Dispatch(ctx);
            }
            catch (Exception ex)
            {
                _log.Error($"Unhandled error for {ctx.Request.Url}: {ex.Message}");
                try
                {
                    ApiResponse.Failure("Internal server error").WriteTo(ctx.Response, 500);
                }
                catch { /* response may already be sent */ }
            }
        }
    }

    /// <summary>Routes HttpListenerContext to the appropriate IHandler.</summary>
    internal class HandlerRouter
    {
        private readonly IHandler _status;
        private readonly IHandler _device;
        private readonly IHandler _deviceList;
        private readonly IHandler _appList;
        private readonly IHandler _connect;
        private readonly IHandler _disconnect;
        private readonly IHandler _launch;
        private readonly IHandler _capture;
        private readonly IHandler _analysis;
        private readonly IHandler _jobs;

        internal HandlerRouter(DeviceSession session, JobManager jobManager, Config config)
        {
            _status     = new StatusHandler();
            _device     = new DeviceHandler(session);
            _deviceList = new DeviceListHandler();
            _appList    = new AppListHandler();
            _connect    = new ConnectHandler(session, jobManager, config);
            _disconnect = new DisconnectHandler(session);
            _launch     = new LaunchHandler(session, jobManager, config);
            _capture    = new CaptureHandler(session, jobManager, config);
            _analysis   = new AnalysisHandler(jobManager, config);
            _jobs       = new JobsHandler(jobManager);
        }

        internal void Dispatch(HttpListenerContext ctx)
        {
            string path   = ctx.Request.Url?.AbsolutePath.TrimEnd('/') ?? "/";
            string method = ctx.Request.HttpMethod.ToUpperInvariant();

            // Exact and prefix matches — order matters (more-specific first)
            if (path == "/api/status")                                        { _status.Handle(ctx);     return; }
            if (path == "/api/device")                                        { _device.Handle(ctx);     return; }
            if (path == "/api/devices")                                       { _deviceList.Handle(ctx); return; }
            if (path == "/api/app/packages" || path == "/api/app/activities") { _appList.Handle(ctx);    return; }
            if (path == "/api/connect")                                       { _connect.Handle(ctx);    return; }
            if (path == "/api/disconnect")                                    { _disconnect.Handle(ctx); return; }
            if (path == "/api/session/launch")                                { _launch.Handle(ctx);     return; }
            if (path == "/api/capture")                                       { _capture.Handle(ctx);    return; }
            if (path == "/api/analysis")                                      { _analysis.Handle(ctx);   return; }
            if (path == "/api/jobs" || path.StartsWith("/api/jobs/"))         { _jobs.Handle(ctx);       return; }

            // OPTIONS preflight — allow all origins (localhost only, CORS informational)
            if (method == "OPTIONS")
            {
                ctx.Response.AddHeader("Allow", "GET, POST, DELETE, OPTIONS");
                ctx.Response.StatusCode = 204;
                ctx.Response.Close();
                return;
            }

            ApiResponse.Failure($"No route for {method} {path}").WriteTo(ctx.Response, 404);
        }
    }
}
