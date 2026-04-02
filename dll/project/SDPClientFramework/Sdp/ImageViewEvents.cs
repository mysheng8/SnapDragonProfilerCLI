using System;

namespace Sdp
{
	// Token: 0x020000DC RID: 220
	public class ImageViewEvents
	{
		// Token: 0x06000373 RID: 883 RVA: 0x000099E4 File Offset: 0x00007BE4
		public ImageViewEvents()
		{
			this.Display = (EventHandler<ImageViewDisplayEventArgs>)Delegate.Combine(this.Display, new EventHandler<ImageViewDisplayEventArgs>(this.SetVisible));
			this.DisplayText = (EventHandler<ImageViewTextDisplayEventArgs>)Delegate.Combine(this.DisplayText, new EventHandler<ImageViewTextDisplayEventArgs>(this.SetVisible));
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00009A3B File Offset: 0x00007C3B
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("ImageView", null, false, false);
		}

		// Token: 0x04000305 RID: 773
		public EventHandler<ImageViewDisplayEventArgs> Display;

		// Token: 0x04000306 RID: 774
		public EventHandler<ImageViewTextDisplayEventArgs> DisplayText;

		// Token: 0x04000307 RID: 775
		public EventHandler<ImageViewOnMouseHoveredOverImageEventArgs> ImageViewMouseHoveredOverImageEvent;

		// Token: 0x04000308 RID: 776
		public const string IMAGE_VIEW_TYPENAME = "ImageView";
	}
}
