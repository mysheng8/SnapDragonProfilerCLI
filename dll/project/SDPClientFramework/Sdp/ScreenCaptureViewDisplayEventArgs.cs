using System;

namespace Sdp
{
	// Token: 0x0200011F RID: 287
	[Serializable]
	public class ScreenCaptureViewDisplayEventArgs : EventArgs
	{
		// Token: 0x04000407 RID: 1031
		public ImageViewObject CaptureImage;

		// Token: 0x04000408 RID: 1032
		public uint ReplayID;

		// Token: 0x04000409 RID: 1033
		public uint CaptureID;

		// Token: 0x0400040A RID: 1034
		public uint DrawCallID;

		// Token: 0x0400040B RID: 1035
		public ScreenCaptureType Type;
	}
}
