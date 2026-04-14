using System.Net;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    /// <summary>GET /api/device — returns current DeviceSessionInfo snapshot.</summary>
    public class DeviceHandler : BaseHandler
    {
        private readonly DeviceSession _session;
        private static readonly ContextLogger _log = new ContextLogger("DeviceHandler");

        public DeviceHandler(DeviceSession session) { _session = session; }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "GET") { WriteError(ctx, "Method not allowed", 405); return; }
            WriteOk(ctx, _session.GetInfo());
        }
    }
}
