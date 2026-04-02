using System;

namespace Sdp
{
	// Token: 0x02000294 RID: 660
	public class ComboBoxUpdatedEventArgs : EventArgs
	{
		// Token: 0x06000C59 RID: 3161 RVA: 0x0002324C File Offset: 0x0002144C
		public ComboBoxUpdatedEventArgs(string selectedMetric, uint captureID)
		{
			this.SelectedMetric = selectedMetric;
			this.CaptureID = captureID;
		}

		// Token: 0x0400091E RID: 2334
		public string SelectedMetric;

		// Token: 0x0400091F RID: 2335
		public uint CaptureID;
	}
}
