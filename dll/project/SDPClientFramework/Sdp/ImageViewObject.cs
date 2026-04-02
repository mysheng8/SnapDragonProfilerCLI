using System;

namespace Sdp
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	public class ImageViewObject
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000204B File Offset: 0x0000024B
		public ImageViewObject()
		{
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00009A7C File Offset: 0x00007C7C
		public ImageViewObject(int width, int height, bool hasAlpha, int sourceId, int captureId, uint itemID, byte[] data)
		{
			this.Width = width;
			this.Height = height;
			this.Data = data;
			this.HasAlpha = hasAlpha;
			this.Rowstride = width * ImageViewObject.BytesPerPixel;
			this.SourceID = sourceId;
			this.CaptureID = captureId;
			this.ItemId = itemID;
			this.RawImage = null;
		}

		// Token: 0x04000320 RID: 800
		public static int BytesPerPixel = 4;

		// Token: 0x04000321 RID: 801
		public static int BitsPerChannel = 8;

		// Token: 0x04000322 RID: 802
		public int SourceID;

		// Token: 0x04000323 RID: 803
		public int CaptureID;

		// Token: 0x04000324 RID: 804
		public uint ItemId;

		// Token: 0x04000325 RID: 805
		public int Width;

		// Token: 0x04000326 RID: 806
		public int Height;

		// Token: 0x04000327 RID: 807
		public int Rowstride;

		// Token: 0x04000328 RID: 808
		public bool HasAlpha;

		// Token: 0x04000329 RID: 809
		public byte[] Data;

		// Token: 0x0400032A RID: 810
		public RawImageWrapper RawImage;
	}
}
