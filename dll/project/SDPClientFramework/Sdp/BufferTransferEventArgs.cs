using System;

namespace Sdp
{
	// Token: 0x020000BA RID: 186
	public class BufferTransferEventArgs : EventArgs
	{
		// Token: 0x040002A2 RID: 674
		public uint CaptureID;

		// Token: 0x040002A3 RID: 675
		public uint BufferID;

		// Token: 0x040002A4 RID: 676
		public uint BufferCategory;

		// Token: 0x040002A5 RID: 677
		public uint ProviderID;

		// Token: 0x040002A6 RID: 678
		public IntPtr BufferData;

		// Token: 0x040002A7 RID: 679
		public uint BufferDataLength;
	}
}
