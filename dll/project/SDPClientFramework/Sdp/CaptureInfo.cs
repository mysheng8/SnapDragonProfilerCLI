using System;

namespace Sdp
{
	// Token: 0x020001E4 RID: 484
	public struct CaptureInfo
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x000100DF File Offset: 0x0000E2DF
		public CaptureInfo(int captureID, int captureType, string captureName)
		{
			this.CaptureID = captureID;
			this.CaptureType = captureType;
			this.CaptureName = captureName;
		}

		// Token: 0x040006FA RID: 1786
		public readonly int CaptureID;

		// Token: 0x040006FB RID: 1787
		public readonly int CaptureType;

		// Token: 0x040006FC RID: 1788
		public readonly string CaptureName;
	}
}
