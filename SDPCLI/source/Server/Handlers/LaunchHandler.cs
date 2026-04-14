using System.Net;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// POST /api/session/launch — starts an async launch job.
    ///
    /// Body (optional JSON):
    ///   { "packageActivity": "com.example.app/com.example.MainActivity" }
    ///
    /// Returns 202 + { jobId } on success.
    /// Returns 409 if not in Connected state.
    /// </summary>
    public class LaunchHandler : BaseHandler
    {
        private readonly DeviceSession _session;
        private readonly JobManager    _jobs;
        private readonly Config        _config;
        private static readonly ContextLogger _log = new ContextLogger("LaunchHandler");

        public LaunchHandler(DeviceSession session, JobManager jobs, Config config)
        {
            _session = session;
            _jobs    = jobs;
            _config  = config;
        }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "POST") { WriteError(ctx, "Method not allowed", 405); return; }

            // Gate: must be Connected
            if (!_session.TryTransition(DeviceStatus.Connected, DeviceStatus.Launching))
            {
                WriteError(ctx, $"Cannot launch from state '{_session.Status}'", 409);
                return;
            }

            var body = ReadJsonBody<LaunchRequest>(ctx.Request);
            string? packageActivity = body?.PackageActivity;

            var config = _config;
            var job = _jobs.Submit(JobType.Launch, (j, ct) =>
                LaunchJobRunner.RunAsync(packageActivity, j, _session, config, ct));

            ctx.Response.StatusCode = 202;
            WriteOk(ctx, new { jobId = job.Id });
        }

        private class LaunchRequest { public string? PackageActivity { get; set; } }
    }
}
