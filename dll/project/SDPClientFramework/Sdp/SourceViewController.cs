using System;
using System.Collections.Generic;
using System.Linq;

namespace Sdp
{
	// Token: 0x0200027C RID: 636
	public class SourceViewController : IViewController
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002023C File Offset: 0x0001E43C
		public SourceViewController(ISourceView view)
		{
			this.m_view = view;
			this.m_view.WindowClosed += this.m_view_WindowClosed;
			this.m_displayed = new HashSet<CodeItem>();
			SourceViewEvents sourceViewEvents = SdpApp.EventsManager.SourceViewEvents;
			sourceViewEvents.Add = (EventHandler<SourceViewAddEventArgs>)Delegate.Combine(sourceViewEvents.Add, new EventHandler<SourceViewAddEventArgs>(this.sourceViewEvents_Add));
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000202A3 File Offset: 0x0001E4A3
		private void m_view_WindowClosed(object sender, SourceWindowClosedArgs e)
		{
			if (e == null)
			{
				return;
			}
			this.RemoveTracker(e.Item);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000202B8 File Offset: 0x0001E4B8
		private void sourceViewEvents_Add(object sender, SourceViewAddEventArgs e)
		{
			CodeItem codeItem = new CodeItem();
			codeItem.SourceID = e.SourceID;
			codeItem.CaptureID = e.CaptureID;
			codeItem.Id = e.Id;
			if (!this.m_displayed.Contains(codeItem))
			{
				this.AddTracker(codeItem);
				this.m_view.Add(codeItem, e.Title, e.Code);
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002031C File Offset: 0x0001E51C
		private void AddTracker(CodeItem item)
		{
			this.m_displayed.Add(item);
			this.m_view.MakeButtonsSensitive();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00020336 File Offset: 0x0001E536
		private void RemoveTracker(CodeItem item)
		{
			if (item == null)
			{
				return;
			}
			if (this.m_displayed.Contains(item))
			{
				this.m_displayed.Remove(item);
			}
			if (this.m_displayed.Count == 0)
			{
				this.m_view.MakeButtonsInsensitive();
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00020370 File Offset: 0x0001E570
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

		// Token: 0x06000AFD RID: 2813 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0002039F File Offset: 0x0001E59F
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x040008A7 RID: 2215
		private ISourceView m_view;

		// Token: 0x040008A8 RID: 2216
		private HashSet<CodeItem> m_displayed;
	}
}
