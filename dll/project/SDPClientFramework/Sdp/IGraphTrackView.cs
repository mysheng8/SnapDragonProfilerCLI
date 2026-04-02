using System;
using Cairo;

namespace Sdp
{
	// Token: 0x020002A8 RID: 680
	public interface IGraphTrackView
	{
		// Token: 0x06000D65 RID: 3429
		void SetDrawMode(GraphTrackController.DrawMode drawMode);

		// Token: 0x06000D66 RID: 3430
		void SetBGColor(Color color);

		// Token: 0x06000D67 RID: 3431
		void ResetBGColor();

		// Token: 0x06000D68 RID: 3432
		void SetDataViewBounds(double min, double max);

		// Token: 0x06000D69 RID: 3433
		void AddMetric(GraphTrackMetric metric);

		// Token: 0x06000D6A RID: 3434
		void RemoveMetric(GraphTrackMetric metric);

		// Token: 0x170002AC RID: 684
		// (set) Token: 0x06000D6B RID: 3435
		bool FixYAxis { set; }

		// Token: 0x170002AD RID: 685
		// (set) Token: 0x06000D6C RID: 3436
		bool PanningEnabled { set; }

		// Token: 0x170002AE RID: 686
		// (set) Token: 0x06000D6D RID: 3437
		GraphTrackFlags Flags { set; }

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06000D6E RID: 3438
		// (remove) Token: 0x06000D6F RID: 3439
		event EventHandler<MetricRemoveRequestEventArgs> MetricRemoveRequest;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06000D70 RID: 3440
		// (remove) Token: 0x06000D71 RID: 3441
		event EventHandler<SetDataViewBoundsEventArgs> DataViewBoundsChanged;

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06000D72 RID: 3442
		// (remove) Token: 0x06000D73 RID: 3443
		event EventHandler<SetDataBoundsEventArgs> DataBoundsChanged;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06000D74 RID: 3444
		// (remove) Token: 0x06000D75 RID: 3445
		event EventHandler<MetricBeginDragArgs> MetricDragBegin;
	}
}
