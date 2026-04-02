using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001A1 RID: 417
	public class ResourcesViewModel
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x0000B8C1 File Offset: 0x00009AC1
		public void AddViewSource(ViewSource viewSource)
		{
			this.m_viewSources.Add(viewSource);
			SdpApp.EventsManager.Raise<ViewSourceAddedEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ViewSourceAdded, this, new ViewSourceAddedEventArgs(viewSource));
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0000B8EF File Offset: 0x00009AEF
		public List<ViewSource> ViewSources
		{
			get
			{
				return this.m_viewSources;
			}
		}

		// Token: 0x04000635 RID: 1589
		private readonly List<ViewSource> m_viewSources = new List<ViewSource>();
	}
}
