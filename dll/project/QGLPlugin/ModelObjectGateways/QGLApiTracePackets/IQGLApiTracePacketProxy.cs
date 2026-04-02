using System;
using Sdp.Functional;

namespace QGLPlugin.ModelObjectGateways.QGLApiTracePackets
{
	// Token: 0x0200003F RID: 63
	internal interface IQGLApiTracePacketProxy
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000141 RID: 321
		Result<uint, string> CaptureID { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000142 RID: 322
		Result<uint, string> InstanceID { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000143 RID: 323
		Result<uint, string> CallID { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000144 RID: 324
		Result<uint, string> ThreadID { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000145 RID: 325
		Result<ulong, string> CPUStartTime { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000146 RID: 326
		Result<ulong, string> CPUEndTime { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000147 RID: 327
		Result<string, string> FunctionName { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000148 RID: 328
		Result<string, string> FunctionParams { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000149 RID: 329
		Result<string, string> FunctionReturnValue { get; }

		// Token: 0x0600014A RID: 330
		Result<QGLApiTracePacket, string> ToDomainObject();
	}
}
