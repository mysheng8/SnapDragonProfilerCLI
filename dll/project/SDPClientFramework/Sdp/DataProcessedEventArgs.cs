using System;

namespace Sdp
{
	// Token: 0x020000B9 RID: 185
	public class DataProcessedEventArgs : EventArgs
	{
		// Token: 0x0400029F RID: 671
		public uint CaptureID;

		// Token: 0x040002A0 RID: 672
		public uint BufferID;

		// Token: 0x040002A1 RID: 673
		public uint BufferCategory;
	}
}
