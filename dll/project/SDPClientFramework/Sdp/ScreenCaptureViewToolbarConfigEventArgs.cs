using System;

namespace Sdp
{
	// Token: 0x02000119 RID: 281
	public class ScreenCaptureViewToolbarConfigEventArgs : EventArgs
	{
		// Token: 0x040003F2 RID: 1010
		public int CaptureID;

		// Token: 0x040003F3 RID: 1011
		public bool ShowAttachmentsComboBox;

		// Token: 0x040003F4 RID: 1012
		public bool ShowContextComboBox;

		// Token: 0x040003F5 RID: 1013
		public bool ShowLegacyAttachmentsComboBox;

		// Token: 0x040003F6 RID: 1014
		public bool ShowBinningToggle;
	}
}
