using System;

namespace Sdp
{
	// Token: 0x0200008B RID: 139
	public enum CaptureType : uint
	{
		// Token: 0x040001E0 RID: 480
		Realtime = 1U,
		// Token: 0x040001E1 RID: 481
		Trace,
		// Token: 0x040001E2 RID: 482
		Snapshot = 4U,
		// Token: 0x040001E3 RID: 483
		Sampling = 8U,
		// Token: 0x040001E4 RID: 484
		All = 4294967295U,
		// Token: 0x040001E5 RID: 485
		INVALID = 4277009102U
	}
}
