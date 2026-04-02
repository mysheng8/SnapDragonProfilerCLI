using System;

namespace Sdp
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	public class RawImageWrapper
	{
		// Token: 0x06000378 RID: 888 RVA: 0x00009A4F File Offset: 0x00007C4F
		public RawImageWrapper(string format, uint bytesPerPixel, ImagePixelType imagePixelType, uint numberChannels, byte[] data)
		{
			this.Format = format;
			this.BytesPerPixel = bytesPerPixel;
			this.ImagePixelType = imagePixelType;
			this.NumberChannels = numberChannels;
			this.Data = data;
		}

		// Token: 0x0400031B RID: 795
		public string Format;

		// Token: 0x0400031C RID: 796
		public uint BytesPerPixel;

		// Token: 0x0400031D RID: 797
		public ImagePixelType ImagePixelType;

		// Token: 0x0400031E RID: 798
		public uint NumberChannels;

		// Token: 0x0400031F RID: 799
		public byte[] Data;
	}
}
