using System;
using Sdp.Functional;

namespace QGLPlugin.ModelObjectGateways.QGLApiQueueSubmitTimings
{
	// Token: 0x02000042 RID: 66
	internal interface IQGLApiQueueSubmitTimingProxy
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000150 RID: 336
		Result<uint, string> LoggingID { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000151 RID: 337
		Result<uint, string> CaptureID { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000152 RID: 338
		Result<uint, string> InstanceID { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000153 RID: 339
		Result<uint, string> CallID { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000154 RID: 340
		Result<uint, string> Index { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000155 RID: 341
		Result<ulong, string> CommandBuffer { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000156 RID: 342
		Result<ulong, string> FunctionGPUStartTimeNS { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000157 RID: 343
		Result<ulong, string> FunctionGPUEndTimeNS { get; }

		// Token: 0x06000158 RID: 344
		Result<QGLApiQueueSubmitTiming, string> ToDomain();
	}
}
