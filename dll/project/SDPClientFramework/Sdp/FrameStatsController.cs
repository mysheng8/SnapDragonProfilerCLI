using System;

namespace Sdp
{
	// Token: 0x020001CA RID: 458
	public class FrameStatsController : IViewController
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0000D960 File Offset: 0x0000BB60
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000D968 File Offset: 0x0000BB68
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000D998 File Offset: 0x0000BB98
		public FrameStatsController(IFrameStatsView view)
		{
			this.m_view = view;
			FrameStatsEvents frameStatsEvents = SdpApp.EventsManager.FrameStatsEvents;
			frameStatsEvents.AddSource = (EventHandler<SourceEventArgs>)Delegate.Combine(frameStatsEvents.AddSource, new EventHandler<SourceEventArgs>(this.FrameStatsViewEvents_AddSource));
			FrameStatsEvents frameStatsEvents2 = SdpApp.EventsManager.FrameStatsEvents;
			frameStatsEvents2.Invalidate = (EventHandler<FrameStatsViewInvalidateEventArgs>)Delegate.Combine(frameStatsEvents2.Invalidate, new EventHandler<FrameStatsViewInvalidateEventArgs>(this.FrameStatsViewEvents_Invalidate));
			FrameStatsEvents frameStatsEvents3 = SdpApp.EventsManager.FrameStatsEvents;
			frameStatsEvents3.DisableCalculateButton = (EventHandler<DisableEventArgs>)Delegate.Combine(frameStatsEvents3.DisableCalculateButton, new EventHandler<DisableEventArgs>(this.FrameStatsViewEvents_DisableCalculateButton));
			FrameStatsEvents frameStatsEvents4 = SdpApp.EventsManager.FrameStatsEvents;
			frameStatsEvents4.DisableContextComboBox = (EventHandler<DisableEventArgs>)Delegate.Combine(frameStatsEvents4.DisableContextComboBox, new EventHandler<DisableEventArgs>(this.FrameStatsViewEvents_DisableContextComboBox));
			FrameStatsEvents frameStatsEvents5 = SdpApp.EventsManager.FrameStatsEvents;
			frameStatsEvents5.SetContextEntries = (EventHandler<SetContextEntryArgs>)Delegate.Combine(frameStatsEvents5.SetContextEntries, new EventHandler<SetContextEntryArgs>(this.FrameStatsViewEvents_SetContextEntries));
			this.m_view.OnCalculateStatsButtonClicked += this.m_view_OnCalculateClicked;
			this.m_view.ContextSelectionChanged += this.m_view_OnContextSelectionChanged;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		private void FrameStatsViewEvents_AddSource(object sender, SourceEventArgs e)
		{
			this.m_currentSourceID = e.SourceID;
			this.m_currentCaptureID = e.CaptureID;
			SourceEventArgs sourceEventArgs = new SourceEventArgs();
			sourceEventArgs.SourceID = this.m_currentSourceID;
			sourceEventArgs.CaptureID = this.m_currentCaptureID;
			SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.FrameStatsEvents.SourceSelected, this, sourceEventArgs);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0000DB18 File Offset: 0x0000BD18
		private void FrameStatsViewEvents_Invalidate(object sender, FrameStatsViewInvalidateEventArgs args)
		{
			this.m_view.Reset();
			foreach (FrameStatsViewColumn frameStatsViewColumn in args.Columns)
			{
				this.m_view.AddTextColumn(frameStatsViewColumn.Title, frameStatsViewColumn.ModelIndex);
			}
			this.m_view.Model = args.Model;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0000DB98 File Offset: 0x0000BD98
		private void FrameStatsViewEvents_DisableCalculateButton(object sender, DisableEventArgs args)
		{
			this.m_view.DisableCaculateButton(args.Disable);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0000DBAB File Offset: 0x0000BDAB
		private void FrameStatsViewEvents_DisableContextComboBox(object sender, DisableEventArgs args)
		{
			this.m_view.DisableContextComboBox(args.Disable);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0000DBBE File Offset: 0x0000BDBE
		private void FrameStatsViewEvents_SetContextEntries(object sender, SetContextEntryArgs args)
		{
			this.m_view.SetComboBoxStrings(args.Entries);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000DBD1 File Offset: 0x0000BDD1
		private void m_view_OnCalculateClicked(object sender, EventArgs e)
		{
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.FrameStatsEvents.CalculateButtonClicked, this, EventArgs.Empty);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000DBF2 File Offset: 0x0000BDF2
		private void m_view_OnContextSelectionChanged(object sender, ContextSelectionChangedArgs e)
		{
			SdpApp.EventsManager.Raise<ContextSelectionChangedArgs>(SdpApp.EventsManager.FrameStatsEvents.ContextSelectionChanged, this, e);
		}

		// Token: 0x0400068F RID: 1679
		private IFrameStatsView m_view;

		// Token: 0x04000690 RID: 1680
		private int m_currentSourceID;

		// Token: 0x04000691 RID: 1681
		private int m_currentCaptureID;
	}
}
