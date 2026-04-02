using System;

namespace Sdp
{
	// Token: 0x0200010F RID: 271
	public class ItemSelectedEventArgs : EventArgs
	{
		// Token: 0x040003C9 RID: 969
		public int SourceID;

		// Token: 0x040003CA RID: 970
		public int CaptureID;

		// Token: 0x040003CB RID: 971
		public int CategoryID;

		// Token: 0x040003CC RID: 972
		public long[] ResourceIDs = new long[0];
	}
}
