using System;

namespace Sdp
{
	// Token: 0x020000B5 RID: 181
	public class OpenSnapshotFromSessionArgs : EventArgs
	{
		// Token: 0x04000292 RID: 658
		public uint SelectedCaptureID;

		// Token: 0x04000293 RID: 659
		public uint NewCaptureID;

		// Token: 0x04000294 RID: 660
		public string SessionPath;

		// Token: 0x04000295 RID: 661
		public string TempImageFile;

		// Token: 0x04000296 RID: 662
		public RenderingAPI API;
	}
}
