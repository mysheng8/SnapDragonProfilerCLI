using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000118 RID: 280
	public class ScreenCaptureViewInvalidateEventArgs : EventArgs
	{
		// Token: 0x040003EF RID: 1007
		public int CaptureID;

		// Token: 0x040003F0 RID: 1008
		public int DrawcallID;

		// Token: 0x040003F1 RID: 1009
		public readonly Dictionary<int, ImageViewObject> Attachments = new Dictionary<int, ImageViewObject>();
	}
}
