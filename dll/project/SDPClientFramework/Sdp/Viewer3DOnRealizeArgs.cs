using System;

namespace Sdp
{
	// Token: 0x02000164 RID: 356
	public class Viewer3DOnRealizeArgs : EventArgs
	{
		// Token: 0x0400052C RID: 1324
		public string DisplayTypeName;

		// Token: 0x0400052D RID: 1325
		public IntPtr DisplayHandle;

		// Token: 0x0400052E RID: 1326
		public IntPtr WindowHandle;

		// Token: 0x0400052F RID: 1327
		public uint Width;

		// Token: 0x04000530 RID: 1328
		public uint Height;
	}
}
