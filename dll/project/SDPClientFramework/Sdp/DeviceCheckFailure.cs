using System;

namespace Sdp
{
	// Token: 0x020002C8 RID: 712
	public enum DeviceCheckFailure
	{
		// Token: 0x040009C2 RID: 2498
		NoSimplePerf = 1,
		// Token: 0x040009C3 RID: 2499
		RealtimeNotSupported,
		// Token: 0x040009C4 RID: 2500
		TraceNotSupported = 4,
		// Token: 0x040009C5 RID: 2501
		SnapshotNotSupported = 8,
		// Token: 0x040009C6 RID: 2502
		SamplingNotSupported = 16,
		// Token: 0x040009C7 RID: 2503
		ImportGfxrCaptureNotSupported = 32
	}
}
