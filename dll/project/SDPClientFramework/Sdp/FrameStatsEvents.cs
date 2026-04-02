using System;

namespace Sdp
{
	// Token: 0x020000D6 RID: 214
	public class FrameStatsEvents
	{
		// Token: 0x0600036C RID: 876 RVA: 0x00009978 File Offset: 0x00007B78
		public FrameStatsEvents()
		{
			this.AddSource = (EventHandler<SourceEventArgs>)Delegate.Combine(this.AddSource, new EventHandler<SourceEventArgs>(this.SetVisible));
			this.Invalidate = (EventHandler<FrameStatsViewInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<FrameStatsViewInvalidateEventArgs>(this.SetVisible));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000099CF File Offset: 0x00007BCF
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("FrameStatsView", null, false, false);
		}

		// Token: 0x040002F4 RID: 756
		public EventHandler<SourceEventArgs> AddSource;

		// Token: 0x040002F5 RID: 757
		public EventHandler<ScreenCaptureViewDisplayEventArgs> OverdrawStatsScreenCapture;

		// Token: 0x040002F6 RID: 758
		public EventHandler<SourceEventArgs> SourceSelected;

		// Token: 0x040002F7 RID: 759
		public EventHandler<FrameStatsViewInvalidateEventArgs> Invalidate;

		// Token: 0x040002F8 RID: 760
		public EventHandler<OverdrawStatsRequestedEventsArgs> OverdrawStatsRequest;

		// Token: 0x040002F9 RID: 761
		public EventHandler<DisableEventArgs> DisableCalculateButton;

		// Token: 0x040002FA RID: 762
		public EventHandler<DisableEventArgs> DisableContextComboBox;

		// Token: 0x040002FB RID: 763
		public EventHandler<EventArgs> CalculateButtonClicked;

		// Token: 0x040002FC RID: 764
		public EventHandler<ContextSelectionChangedArgs> ContextSelectionChanged;

		// Token: 0x040002FD RID: 765
		public EventHandler<SetContextEntryArgs> SetContextEntries;

		// Token: 0x040002FE RID: 766
		public const string FRAME_STATS_VIEW_TYPENAME = "FrameStatsView";
	}
}
