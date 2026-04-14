using System.Net;
using System.Linq;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Server.Jobs;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    public class JobsHandler : BaseHandler
    {
        private readonly JobManager _jobs;
        private static readonly ContextLogger _log = new ContextLogger("JobsHandler");

        public JobsHandler(JobManager jobs) { _jobs = jobs; }

        public override void Handle(HttpListenerContext ctx)
        {
            string path   = ctx.Request.Url?.AbsolutePath.TrimEnd('/') ?? "";
            string method = ctx.Request.HttpMethod.ToUpperInvariant();

            if (method == "POST" && path.Contains("/cancel"))
            {
                string id = ExtractId(path.Replace("/cancel", ""));
                HandleCancel(ctx, id);
                return;
            }

            if (method == "GET" && (path == "/api/jobs" || path == "/api/jobs/"))
            {
                HandleList(ctx);
                return;
            }

            if (method == "GET")
            {
                string id = ExtractId(path);
                HandleGet(ctx, id);
                return;
            }

            if (method == "DELETE")
            {
                string id = ExtractId(path);
                HandleDelete(ctx, id);
                return;
            }

            WriteError(ctx, $"Method {method} not allowed", 405);
        }

        private void HandleList(HttpListenerContext ctx)
        {
            var list = _jobs.List()
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => j.ToSummary())
                .ToList();
            WriteOk(ctx, list);
        }

        private void HandleGet(HttpListenerContext ctx, string id)
        {
            var job = _jobs.Get(id);
            if (job == null) { WriteError(ctx, $"Job '{id}' not found", 404); return; }
            WriteOk(ctx, job.ToSummary());
        }

        private void HandleCancel(HttpListenerContext ctx, string id)
        {
            var job = _jobs.Get(id);
            if (job == null) { WriteError(ctx, $"Job '{id}' not found", 404); return; }
            bool cancelled = _jobs.Cancel(id);
            WriteOk(ctx, new { cancelled, jobId = id, status = job.Status.ToString() });
        }

        private void HandleDelete(HttpListenerContext ctx, string id)
        {
            var job = _jobs.Get(id);
            if (job == null) { WriteError(ctx, $"Job '{id}' not found", 404); return; }
            if (!job.IsTerminal) { WriteError(ctx, "Cannot delete a running job — cancel it first", 409); return; }
            _jobs.Remove(id);
            WriteOk(ctx, new { deleted = true, jobId = id });
        }

        private static string ExtractId(string path)
        {
            int last = path.LastIndexOf('/');
            return last >= 0 ? path.Substring(last + 1) : path;
        }
    }
}
