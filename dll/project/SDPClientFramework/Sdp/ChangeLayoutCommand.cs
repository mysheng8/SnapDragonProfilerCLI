using System;

namespace Sdp
{
	// Token: 0x02000075 RID: 117
	public class ChangeLayoutCommand : Command
	{
		// Token: 0x06000287 RID: 647 RVA: 0x000087E7 File Offset: 0x000069E7
		public ChangeLayoutCommand(string newLayout)
		{
			this.m_newLayout = newLayout;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00008804 File Offset: 0x00006A04
		protected override void OnExecute()
		{
			UIManager uimanager = SdpApp.UIManager;
			if (uimanager != null)
			{
				uimanager.SelectedLayout = this.m_newLayout;
			}
		}

		// Token: 0x0400019F RID: 415
		public const string REALTIME_LAYOUT = "Realtime";

		// Token: 0x040001A0 RID: 416
		public const string SNAPSHOT_LAYOUT = "Snapshot";

		// Token: 0x040001A1 RID: 417
		public const string TRACE_LAYOUT = "Capture";

		// Token: 0x040001A2 RID: 418
		public const string SAMPLING_LAYOUT = "CPU Sampling";

		// Token: 0x040001A3 RID: 419
		public const string INSIGHT_LAYOUT = "Insight";

		// Token: 0x040001A4 RID: 420
		public const string CONNECT_LAYOUT = "Connect";

		// Token: 0x040001A5 RID: 421
		public const string GUIDED_LAYOUT = "Guide";

		// Token: 0x040001A6 RID: 422
		private string m_newLayout = "";
	}
}
