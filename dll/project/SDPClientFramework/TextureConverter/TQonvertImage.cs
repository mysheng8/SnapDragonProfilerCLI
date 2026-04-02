using System;
using System.Runtime.InteropServices;

namespace TextureConverter
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential)]
	public class TQonvertImage
	{
		// Token: 0x04000095 RID: 149
		public uint Width;

		// Token: 0x04000096 RID: 150
		public uint Height;

		// Token: 0x04000097 RID: 151
		public uint Format;

		// Token: 0x04000098 RID: 152
		public IntPtr PtrToFormatFlags;

		// Token: 0x04000099 RID: 153
		public uint DataSize;

		// Token: 0x0400009A RID: 154
		public IntPtr PtrToData;
	}
}
