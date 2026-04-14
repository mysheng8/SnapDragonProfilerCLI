using System.Net;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    public class DisconnectHandler : BaseHandler
    {
        private readonly DeviceSession _session;
        private static readonly ContextLogger _log = new ContextLogger("DisconnectHandler");

        public DisconnectHandler(DeviceSession session)
        {
            _session = session;
        }

        public override void Handle(HttpListenerContext ctx)
        {
            if (ctx.Request.HttpMethod != "POST") { WriteError(ctx, "Method not allowed", 405); return; }
            _session.Disconnect();
            WriteOk(ctx, new { disconnected = true });
        }
    }
}
