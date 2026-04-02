using System;

namespace QGLPlugin
{
	// Token: 0x02000026 RID: 38
	internal interface IShaderStage
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004F RID: 79
		uint CaptureID { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000050 RID: 80
		ulong PipelineID { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000051 RID: 81
		uint ShaderStage { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000052 RID: 82
		VkShaderStageFlagBits StageType { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000053 RID: 83
		ulong ShaderModuleID { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000054 RID: 84
		uint ShaderIndex { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000055 RID: 85
		string PName { get; }
	}
}
