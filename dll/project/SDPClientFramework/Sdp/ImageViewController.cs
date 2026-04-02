using System;

namespace Sdp
{
	// Token: 0x02000250 RID: 592
	public class ImageViewController : IViewController
	{
		// Token: 0x060009B4 RID: 2484 RVA: 0x0001BD7C File Offset: 0x00019F7C
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

		// Token: 0x060009B5 RID: 2485 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0001BDAB File Offset: 0x00019FAB
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public ImageViewController(IImageView view)
		{
			this.m_view = view;
			ImageViewEvents imageViewEvents = SdpApp.EventsManager.ImageViewEvents;
			imageViewEvents.Display = (EventHandler<ImageViewDisplayEventArgs>)Delegate.Combine(imageViewEvents.Display, new EventHandler<ImageViewDisplayEventArgs>(this.OnDisplayImage));
			ImageViewEvents imageViewEvents2 = SdpApp.EventsManager.ImageViewEvents;
			imageViewEvents2.DisplayText = (EventHandler<ImageViewTextDisplayEventArgs>)Delegate.Combine(imageViewEvents2.DisplayText, new EventHandler<ImageViewTextDisplayEventArgs>(this.OnDisplayText));
			this.m_view.MouseHoveredOverImage += this.OnViewMouseHoveredOverImageEvent;
			this.m_view.ZoomInClicked += this.OnViewZoomInClicked;
			this.m_view.ZoomOutClicked += this.OnViewZoomOutClicked;
			this.m_view.ZoomToFitClicked += this.OnViewZoomToFitClicked;
			this.m_view.ZoomToOriginalSizeClicked += this.OnViewZoomToOriginalSizeClicked;
			this.m_view.DrawingAreaScrollEvent += this.OnViewScrolled;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001BEB6 File Offset: 0x0001A0B6
		protected virtual void OnViewMouseHoveredOverImageEvent(object sender, ImageViewOnMouseHoveredOverImageEventArgs args)
		{
			SdpApp.EventsManager.Raise<ImageViewOnMouseHoveredOverImageEventArgs>(SdpApp.EventsManager.ImageViewEvents.ImageViewMouseHoveredOverImageEvent, this, args);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001BED4 File Offset: 0x0001A0D4
		protected virtual void OnDisplayImage(object sender, ImageViewDisplayEventArgs args)
		{
			if (args.ImageType == ImageViewType.Texture2D && args.TopImageObject != null)
			{
				this.m_view.UpdateImage(args.TopImageObject);
			}
			else
			{
				this.m_view.UpdateImage(args.ImageObjects, args.ImageType);
			}
			this.m_view.ZoomToFit();
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0001BF26 File Offset: 0x0001A126
		protected virtual void OnDisplayText(object sender, ImageViewTextDisplayEventArgs args)
		{
			this.m_view.SetText(args.Text);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0001BF39 File Offset: 0x0001A139
		protected virtual void OnViewZoomInClicked(object sender, EventArgs args)
		{
			this.m_view.ZoomIn(0.05);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0001BF4F File Offset: 0x0001A14F
		protected virtual void OnViewZoomOutClicked(object sender, EventArgs args)
		{
			this.m_view.ZoomOut(0.05);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0001BF65 File Offset: 0x0001A165
		protected virtual void OnViewZoomToFitClicked(object sender, EventArgs args)
		{
			this.m_view.ZoomToFit();
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0001BF72 File Offset: 0x0001A172
		protected virtual void OnViewZoomToOriginalSizeClicked(object sender, EventArgs args)
		{
			this.m_view.ZoomToOriginalSize();
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0001BF7F File Offset: 0x0001A17F
		protected virtual void OnViewScrolled(object sender, ScrollEventArgs args)
		{
			if (args.IsUp)
			{
				this.m_view.ZoomIn(0.05);
				return;
			}
			this.m_view.ZoomOut(0.05);
		}

		// Token: 0x04000835 RID: 2101
		private IImageView m_view;

		// Token: 0x04000836 RID: 2102
		private const double SCROLL_INTERVAL = 0.05;
	}
}
