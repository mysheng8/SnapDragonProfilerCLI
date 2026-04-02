using System;

namespace Sdp
{
	// Token: 0x020002B5 RID: 693
	public interface ITimelineView
	{
		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06000DED RID: 3565
		// (remove) Token: 0x06000DEE RID: 3566
		event EventHandler<SelectedBookmarksChangedEventArgs> SelectedBookmarksChanged;

		// Token: 0x06000DEF RID: 3567
		void SetViewBounds(double min, double max);

		// Token: 0x170002B3 RID: 691
		// (set) Token: 0x06000DF0 RID: 3568
		int TracksWidth { set; }
	}
}
