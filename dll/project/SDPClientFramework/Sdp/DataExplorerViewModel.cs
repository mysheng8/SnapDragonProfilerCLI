using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001A5 RID: 421
	public class DataExplorerViewModel
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x0000BFE2 File Offset: 0x0000A1E2
		public void AddViewSource(ViewSource viewSource)
		{
			this.m_viewSources.Add(viewSource);
			SdpApp.EventsManager.Raise<ViewSourceAddedEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.ViewSourceAdded, this, new ViewSourceAddedEventArgs(viewSource));
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000C010 File Offset: 0x0000A210
		public void SelectViewSource(SourceEventArgs args)
		{
			ViewSource viewSource = this.m_viewSources.Find((ViewSource e) => e.CaptureID == args.CaptureID && e.SourceID == args.SourceID);
			if (viewSource != null)
			{
				args.UniqueID = viewSource.UniqueID;
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectSource, this, args);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0000C076 File Offset: 0x0000A276
		public List<ViewSource> ViewSources
		{
			get
			{
				return this.m_viewSources;
			}
		}

		// Token: 0x04000646 RID: 1606
		private readonly List<ViewSource> m_viewSources = new List<ViewSource>();
	}
}
