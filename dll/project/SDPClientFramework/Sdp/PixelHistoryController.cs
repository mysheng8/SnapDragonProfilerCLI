using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200026C RID: 620
	public class PixelHistoryController : IViewController
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x000123A7 File Offset: 0x000105A7
		public ViewDesc SaveSettings()
		{
			return null;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0001DB75 File Offset: 0x0001BD75
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0001DB80 File Offset: 0x0001BD80
		public PixelHistoryController(IPixelHistoryView view)
		{
			this.m_view = view;
			this.m_view.SelectedChanged += this.m_view_SelectedChanged;
			PixelHistoryEvents pixelHistoryEvents = SdpApp.EventsManager.PixelHistoryEvents;
			pixelHistoryEvents.Invalidate = (EventHandler<PixelHistoryInvalidateEventArgs>)Delegate.Combine(pixelHistoryEvents.Invalidate, new EventHandler<PixelHistoryInvalidateEventArgs>(this.pixelHistoryEvents_Invalidate));
			PixelHistoryEvents pixelHistoryEvents2 = SdpApp.EventsManager.PixelHistoryEvents;
			pixelHistoryEvents2.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(pixelHistoryEvents2.SetStatus, new EventHandler<SetStatusEventArgs>(this.pixelHistoryEvents_SetStatus));
			PixelHistoryEvents pixelHistoryEvents3 = SdpApp.EventsManager.PixelHistoryEvents;
			pixelHistoryEvents3.HideStatus = (EventHandler<EventArgs>)Delegate.Combine(pixelHistoryEvents3.HideStatus, new EventHandler<EventArgs>(this.pixelHistoryEvents_HideStatus));
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0001DC34 File Offset: 0x0001BE34
		private void m_view_SelectedChanged(object sender, EventArgs e)
		{
			if (this.m_model != null && this.m_model.Items.ContainsKey(this.m_view.Selected))
			{
				PixelHistoryItemSelectedEventArgs pixelHistoryItemSelectedEventArgs = new PixelHistoryItemSelectedEventArgs();
				pixelHistoryItemSelectedEventArgs.Item = this.m_model.Items[this.m_view.Selected];
				SdpApp.EventsManager.Raise<PixelHistoryItemSelectedEventArgs>(SdpApp.EventsManager.PixelHistoryEvents.ItemSelected, this, pixelHistoryItemSelectedEventArgs);
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0001DCA8 File Offset: 0x0001BEA8
		private void pixelHistoryEvents_Invalidate(object sender, PixelHistoryInvalidateEventArgs args)
		{
			this.m_view.Clear();
			this.m_model = null;
			if (args != null && args.Model != null && args.Model.Items != null)
			{
				this.m_view.SetStatus(StatusType.Neutral, string.Concat(new string[]
				{
					"History for pixel (",
					((int)(args.X * (double)args.Screenshot.Width)).ToString(),
					",",
					((int)(args.Y * (double)args.Screenshot.Height)).ToString(),
					") received"
				}), 5000);
				this.m_model = args.Model;
				using (Dictionary<int, PixelHistoryItem>.Enumerator enumerator = args.Model.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, PixelHistoryItem> keyValuePair = enumerator.Current;
						this.m_view.AddItem(keyValuePair.Key, keyValuePair.Value);
					}
					goto IL_016C;
				}
			}
			this.m_view.SetStatus(StatusType.Warning, string.Concat(new string[]
			{
				"No drawcalls for pixel (",
				((int)(args.X * (double)args.Screenshot.Width)).ToString(),
				",",
				((int)(args.Y * (double)args.Screenshot.Height)).ToString(),
				") available"
			}), 0);
			IL_016C:
			if (args.Screenshot != null)
			{
				this.m_view.SetThumbnail((int)(args.X * (double)args.Screenshot.Width), (int)(args.Y * (double)args.Screenshot.Height), args.Screenshot);
			}
			SdpApp.AnalyticsManager.TrackInteraction(sender.ToString(), "Replaying Pixel History", "Pixel History");
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0001DE8C File Offset: 0x0001C08C
		private void pixelHistoryEvents_SetStatus(object sender, SetStatusEventArgs args)
		{
			this.m_view.SetStatus(args.Status, args.StatusText, args.Duration);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001DEAB File Offset: 0x0001C0AB
		private void pixelHistoryEvents_HideStatus(object sender, EventArgs e)
		{
			this.m_view.HideStatus();
		}

		// Token: 0x04000875 RID: 2165
		private IPixelHistoryView m_view;

		// Token: 0x04000876 RID: 2166
		private PixelHistoryViewModel m_model;
	}
}
