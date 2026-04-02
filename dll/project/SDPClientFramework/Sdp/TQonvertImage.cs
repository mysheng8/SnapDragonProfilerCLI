using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x02000181 RID: 385
	[StructLayout(LayoutKind.Sequential)]
	public class TQonvertImage
	{
		// Token: 0x040005FD RID: 1533
		public uint Width;

		// Token: 0x040005FE RID: 1534
		public uint Height;

		// Token: 0x040005FF RID: 1535
		public uint Format;

		// Token: 0x04000600 RID: 1536
		public IntPtr PtrToFormatFlags;

		// Token: 0x04000601 RID: 1537
		public uint DataSize;

		// Token: 0x04000602 RID: 1538
		public IntPtr PtrToData;
	}
}
