using System;

namespace Sdp
{
	// Token: 0x020000C2 RID: 194
	public class BufferTransferProgressEventArgs : EventArgs
	{
		// Token: 0x040002B7 RID: 695
		public uint ProviderID;

		// Token: 0x040002B8 RID: 696
		public uint BufferCategory;

		// Token: 0x040002B9 RID: 697
		public uint BufferID;

		// Token: 0x040002BA RID: 698
		public uint CaptureID;

		// Token: 0x040002BB RID: 699
		public uint TotalBytes;

		// Token: 0x040002BC RID: 700
		public uint BytesReceived;
	}
}
