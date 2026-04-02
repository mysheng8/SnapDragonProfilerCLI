using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200024D RID: 589
	public interface IImageView : IView
	{
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x0600099B RID: 2459
		// (remove) Token: 0x0600099C RID: 2460
		event EventHandler<ImageViewOnMouseHoveredOverImageEventArgs> MouseHoveredOverImage;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x0600099D RID: 2461
		// (remove) Token: 0x0600099E RID: 2462
		event EventHandler ZoomInClicked;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600099F RID: 2463
		// (remove) Token: 0x060009A0 RID: 2464
		event EventHandler ZoomOutClicked;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x060009A1 RID: 2465
		// (remove) Token: 0x060009A2 RID: 2466
		event EventHandler ZoomToFitClicked;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060009A3 RID: 2467
		// (remove) Token: 0x060009A4 RID: 2468
		event EventHandler ZoomToOriginalSizeClicked;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060009A5 RID: 2469
		// (remove) Token: 0x060009A6 RID: 2470
		event EventHandler<ScrollEventArgs> DrawingAreaScrollEvent;

		// Token: 0x060009A7 RID: 2471
		void ClearImage();

		// Token: 0x060009A8 RID: 2472
		void UpdateImage(ImageViewObject imageObject);

		// Token: 0x060009A9 RID: 2473
		void UpdateImage(List<List<ImageViewObject>> imageObjects, ImageViewType displayType);

		// Token: 0x060009AA RID: 2474
		void DrawImage();

		// Token: 0x060009AB RID: 2475
		void SetText(string text);

		// Token: 0x060009AC RID: 2476
		void ZoomIn(double zoomInterval);

		// Token: 0x060009AD RID: 2477
		void ZoomOut(double zoomInterval);

		// Token: 0x060009AE RID: 2478
		void ZoomToFit();

		// Token: 0x060009AF RID: 2479
		void ZoomToOriginalSize();

		// Token: 0x060009B0 RID: 2480
		void Rotate(double degrees);

		// Token: 0x060009B1 RID: 2481
		bool HasImage();
	}
}
