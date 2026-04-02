using System;

namespace Sdp
{
	// Token: 0x020001A0 RID: 416
	public class RealtimeModel
	{
		// Token: 0x06000512 RID: 1298 RVA: 0x0000B894 File Offset: 0x00009A94
		public int GetNextRealtimeCaptureNumber()
		{
			int nextTraceCaptureNumber = this.m_nextTraceCaptureNumber;
			this.m_nextTraceCaptureNumber = nextTraceCaptureNumber + 1;
			return nextTraceCaptureNumber;
		}

		// Token: 0x04000633 RID: 1587
		public GroupLayoutController CurrentGroupLayoutController;

		// Token: 0x04000634 RID: 1588
		private int m_nextTraceCaptureNumber = 1;
	}
}
