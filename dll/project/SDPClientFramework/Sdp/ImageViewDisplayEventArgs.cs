using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020000DE RID: 222
	public class ImageViewDisplayEventArgs : EventArgs
	{
		// Token: 0x0400030A RID: 778
		public ImageViewObject TopImageObject;

		// Token: 0x0400030B RID: 779
		public List<List<ImageViewObject>> ImageObjects;

		// Token: 0x0400030C RID: 780
		public ImageViewType ImageType;
	}
}
