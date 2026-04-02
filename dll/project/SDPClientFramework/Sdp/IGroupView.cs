using System;

namespace Sdp
{
	// Token: 0x020002A3 RID: 675
	public interface IGroupView
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000D1D RID: 3357
		// (set) Token: 0x06000D1E RID: 3358
		string Title { get; set; }

		// Token: 0x06000D1F RID: 3359
		bool ExpandGroup(bool expand);

		// Token: 0x06000D20 RID: 3360
		ITrackViewBase AddTrack(TrackType trackType, IMetricPlugin metricPlugin);

		// Token: 0x06000D21 RID: 3361
		void RemoveTrack(ITrackViewBase track);

		// Token: 0x06000D22 RID: 3362
		void TrackExpandedCollapsed(ITrackViewBase track);

		// Token: 0x06000D23 RID: 3363
		void ResizeTrack(ITrackViewBase track, int height);

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06000D24 RID: 3364
		// (remove) Token: 0x06000D25 RID: 3365
		event EventHandler<AddTrackRequestedEventArgs> AddTrackRequested;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06000D26 RID: 3366
		// (remove) Token: 0x06000D27 RID: 3367
		event EventHandler ExpandCollapseClicked;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06000D28 RID: 3368
		// (remove) Token: 0x06000D29 RID: 3369
		event EventHandler RemoveClicked;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06000D2A RID: 3370
		// (remove) Token: 0x06000D2B RID: 3371
		event EventHandler TitleChanged;

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06000D2C RID: 3372
		// (remove) Token: 0x06000D2D RID: 3373
		event EventHandler<MetricDroppedEventArgs> MetricDropped;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06000D2E RID: 3374
		// (remove) Token: 0x06000D2F RID: 3375
		event EventHandler<MetricDroppedEventArgs> CategoryDropped;

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06000D30 RID: 3376
		// (remove) Token: 0x06000D31 RID: 3377
		event EventHandler MetricDataEntered;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06000D32 RID: 3378
		// (remove) Token: 0x06000D33 RID: 3379
		event EventHandler MetricDataLeft;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06000D34 RID: 3380
		// (remove) Token: 0x06000D35 RID: 3381
		event EventHandler<IGanttTrackView> GanttTrackAdded;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06000D36 RID: 3382
		// (remove) Token: 0x06000D37 RID: 3383
		event EventHandler<IGanttTrackView> GanttTrackRemoved;
	}
}
