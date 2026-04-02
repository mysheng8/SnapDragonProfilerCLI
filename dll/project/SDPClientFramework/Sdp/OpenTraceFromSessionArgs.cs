using System;

namespace Sdp
{
	// Token: 0x020000B4 RID: 180
	public class OpenTraceFromSessionArgs : EventArgs
	{
		// Token: 0x0400028F RID: 655
		public uint SelectedCaptureID;

		// Token: 0x04000290 RID: 656
		public uint NewCaptureID;

		// Token: 0x04000291 RID: 657
		public string SessionPath;
	}
}
