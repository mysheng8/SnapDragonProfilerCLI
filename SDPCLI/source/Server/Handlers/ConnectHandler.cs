using System.Net;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// POST /api/connect — starts an async connect job.
    ///
    /// Body (optional JSON):
    ///   { "deviceId": "192.168.1.100:5555" }
    ///
    /// Returns 202 + { jobId } on success.
    /// Returns 409 if already connecting / connected / in active session.
    /// </summary>
    public class ConnectHandler : BaseHandler
    {
        private readonly DeviceSession _session;
        private readonly JobManager    _jobs;
        private readonly Config        _config;
        private static readonly ContextLogger _log = new ContextLogger("ConnectHandler");

        public ConnectHandler(DeviceSession session, JobManager jobs, Config config)
        {
            _session = session;
            _jobs    = jobs;
            _config  = config;
        }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "POST") { WriteError(ctx, "Method not allowed", 405); return; }

            // Gate: must be Disconnected
            if (!_session.TryTransition(DeviceStatus.Disconnected, DeviceStatus.Connecting))
            {
                WriteError(ctx, $"Cannot connect from state '{_session.Status}'", 409);
                return;
            }

            string? deviceId = null;
            var body = ReadJsonBody<ConnectRequest>(ctx.Request);
            deviceId = body?.DeviceId;

            var config = _config;
            var job = _jobs.Submit(JobType.Connect, (j, ct) =>
                ConnectJobRunner.RunAsync(deviceId, j, _session, config, ct));

            ctx.Response.StatusCode = 202;
            WriteOk(ctx, new { jobId = job.Id });
        }

        private class ConnectRequest { public string? DeviceId { get; set; } }
    }
}
