using System;

namespace Sdp
{
	// Token: 0x0200024E RID: 590
	public class ImageViewOnMouseHoveredOverImageEventArgs : EventArgs
	{
		// Token: 0x0400082D RID: 2093
		public uint face;

		// Token: 0x0400082E RID: 2094
		public double MipLocationX;

		// Token: 0x0400082F RID: 2095
		public double MipLocationY;

		// Token: 0x04000830 RID: 2096
		public uint Mip;

		// Token: 0x04000831 RID: 2097
		public bool InMipMap;

		// Token: 0x04000832 RID: 2098
		public int SourceID;

		// Token: 0x04000833 RID: 2099
		public uint ItemID;
	}
}
