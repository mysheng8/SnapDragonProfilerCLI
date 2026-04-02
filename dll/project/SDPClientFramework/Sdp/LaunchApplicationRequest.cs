using System;

namespace Sdp
{
	// Token: 0x020001CF RID: 463
	public class LaunchApplicationRequest
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x0000EEEA File Offset: 0x0000D0EA
		public LaunchApplicationRequest(bool response, string executablePath, string workingDir, string commandLineArgs, RenderingAPI renderingAPIs)
		{
			this.Response = response;
			this.ExecutablePath = executablePath;
			this.WorkingDirectory = workingDir;
			this.CommandlineArguments = commandLineArgs;
			this.RenderingAPIs = renderingAPIs;
		}

		// Token: 0x040006A9 RID: 1705
		public bool Response;

		// Token: 0x040006AA RID: 1706
		public string ExecutablePath;

		// Token: 0x040006AB RID: 1707
		public string WorkingDirectory;

		// Token: 0x040006AC RID: 1708
		public string CommandlineArguments;

		// Token: 0x040006AD RID: 1709
		public RenderingAPI RenderingAPIs;
	}
}
