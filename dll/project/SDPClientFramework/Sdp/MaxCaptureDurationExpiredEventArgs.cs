using System;

namespace Sdp
{
	// Token: 0x020000C3 RID: 195
	public class MaxCaptureDurationExpiredEventArgs : EventArgs
	{
		// Token: 0x040002BD RID: 701
		public uint CaptureId;

		// Token: 0x040002BE RID: 702
		public uint FirstFrame;

		// Token: 0x040002BF RID: 703
		public uint LastFrame;

		// Token: 0x040002C0 RID: 704
		public bool AnyFrameCollected;

		// Token: 0x040002C1 RID: 705
		public bool DataProcessed;
	}
}
