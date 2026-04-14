using System.Net;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// POST /api/capture — starts an async GPU snapshot capture job.
    ///
    /// Body (optional JSON):
    ///   { "outputDir": "/abs/or/relative/path", "label": "frame123" }
    ///
    /// Returns 202 + { jobId } on success.
    /// Returns 409 if not in SessionActive state.
    /// </summary>
    public class CaptureHandler : BaseHandler
    {
        private readonly DeviceSession _session;
        private readonly JobManager    _jobs;
        private readonly Config        _config;
        private static readonly ContextLogger _log = new ContextLogger("CaptureHandler");

        public CaptureHandler(DeviceSession session, JobManager jobs, Config config)
        {
            _session = session;
            _jobs    = jobs;
            _config  = config;
        }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "POST") { WriteError(ctx, "Method not allowed", 405); return; }

            // Gate: must be SessionActive
            if (!_session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Capturing))
            {
                WriteError(ctx, $"Cannot capture from state '{_session.Status}'", 409);
                return;
            }

            var body = ReadJsonBody<CaptureRequest>(ctx.Request);
            string? outputDir    = body?.OutputDir;
            string? captureLabel = body?.Label;

            var config = _config;
            var job = _jobs.Submit(JobType.Capture, (j, ct) =>
                CaptureJobRunner.RunAsync(outputDir, captureLabel, j, _session, config, ct));

            ctx.Response.StatusCode = 202;
            WriteOk(ctx, new { jobId = job.Id });
        }

        private class CaptureRequest
        {
            public string? OutputDir { get; set; }
            public string? Label     { get; set; }
        }
    }
}
