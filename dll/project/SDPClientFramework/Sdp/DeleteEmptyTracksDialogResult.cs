using System;

namespace Sdp
{
	// Token: 0x0200029D RID: 669
	public struct DeleteEmptyTracksDialogResult
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x00023271 File Offset: 0x00021471
		public DeleteEmptyTracksDialogResult(bool yesSelected, bool retainSettings)
		{
			this.YesSelected = yesSelected;
			this.RetainSettings = retainSettings;
		}

		// Token: 0x0400092B RID: 2347
		public readonly bool YesSelected;

		// Token: 0x0400092C RID: 2348
		public readonly bool RetainSettings;
	}
}
