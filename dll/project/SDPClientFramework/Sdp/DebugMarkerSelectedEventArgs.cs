using System;

namespace Sdp
{
	// Token: 0x0200009B RID: 155
	public class DebugMarkerSelectedEventArgs : EventArgs
	{
		// Token: 0x0400021B RID: 539
		public int LabelID;

		// Token: 0x0400021C RID: 540
		public int CaptureID;

		// Token: 0x0400021D RID: 541
		public string LabelName;

		// Token: 0x0400021E RID: 542
		public ulong TimestampBegin;

		// Token: 0x0400021F RID: 543
		public ulong TimestampEnd;

		// Token: 0x04000220 RID: 544
		public bool IsDebugRegion;

		// Token: 0x04000221 RID: 545
		public uint BlockID;

		// Token: 0x04000222 RID: 546
		public ulong ContextID;
	}
}
