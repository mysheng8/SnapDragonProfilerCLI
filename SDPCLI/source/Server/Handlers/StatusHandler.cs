using System.Net;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>GET /api/status — always 200, confirming the server is alive.</summary>
    public class StatusHandler : BaseHandler
    {
        private static readonly ContextLogger _log = new ContextLogger("StatusHandler");

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "GET") { WriteError(ctx, "Method not allowed", 405); return; }
            WriteOk(ctx, new { status = "ok", version = "1.0" });
        }
    }
}
