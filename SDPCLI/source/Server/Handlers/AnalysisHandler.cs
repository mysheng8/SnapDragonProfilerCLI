using System.Net;
using SnapdragonProfilerCLI.Analysis;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>
    /// POST /api/analysis — starts an async analysis job.
    ///
    /// Body (required JSON):
    ///   {
    ///     "sdpPath":    "/abs/path/to/capture.sdp",   (required)
    ///     "snapshotId": 2,                             (required, >= 2)
    ///     "outputDir":  "/abs/output/dir",             (optional)
    ///     "targets":    "label,metrics,status"         (optional, default = all)
    ///   }
    ///
    /// Returns 202 + { jobId } on success.
    /// Returns 409 if an identical sdpPath+snapshotId analysis is already running.
    /// Returns 400 if sdpPath or snapshotId is missing / invalid.
    /// </summary>
    public class AnalysisHandler : BaseHandler
    {
        private readonly JobManager _jobs;
        private readonly Config     _config;
        private static readonly ContextLogger _log = new ContextLogger("AnalysisHandler");

        public AnalysisHandler(JobManager jobs, Config config)
        {
            _jobs   = jobs;
            _config = config;
        }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "POST") { WriteError(ctx, "Method not allowed", 405); return; }

            var req = ReadJsonBody<AnalysisRequest>(ctx.Request);
            if (req == null)
            {
                WriteError(ctx, "Request body required: { sdpPath, snapshotId }", 400);
                return;
            }

            if (string.IsNullOrWhiteSpace(req.SdpPath))
            {
                WriteError(ctx, "sdpPath is required", 400);
                return;
            }
            if (req.SnapshotId < 2)
            {
                WriteError(ctx, "snapshotId must be >= 2", 400);
                return;
            }

            // Path traversal guard
            if (!ValidateSdpPath(req.SdpPath!, ctx))
                return;

            // Duplicate-job guard — one active analysis per (sdpPath, snapshotId)
            var existing = _jobs.FindActiveAnalysis(req.SdpPath!, req.SnapshotId);
            if (existing != null)
            {
                WriteError(ctx, $"Analysis already running as job {existing.Id}", 409);
                return;
            }

            AnalysisTarget targets = AnalysisTargetExtensions.Parse(req.Targets);

            var config = _config;
            var job = _jobs.SubmitAnalysis(
                req.SdpPath!, req.SnapshotId, req.OutputDir ?? "", targets,
                (j, ct) => AnalysisJobRunner.RunAsync((AnalysisJob)j, config, ct));

            ctx.Response.StatusCode = 202;
            WriteOk(ctx, new { jobId = job.Id });
        }

        private bool ValidateSdpPath(string sdpPath, HttpListenerContext ctx)
        {
            if (sdpPath.Contains("..") ||
                !sdpPath.EndsWith(".sdp", System.StringComparison.OrdinalIgnoreCase))
            {
                WriteError(ctx, "sdpPath must be an absolute path ending in .sdp", 400);
                return false;
            }
            if (!System.IO.Path.IsPathRooted(sdpPath))
            {
                WriteError(ctx, "sdpPath must be an absolute path", 400);
                return false;
            }
            return true;
        }

        private class AnalysisRequest
        {
            public string?  SdpPath    { get; set; }
            public uint     SnapshotId { get; set; }
            public string?  OutputDir  { get; set; }
            public string?  Targets    { get; set; }
        }
    }
}
