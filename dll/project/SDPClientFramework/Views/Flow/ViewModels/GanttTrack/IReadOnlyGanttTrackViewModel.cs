using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;
using Sdp.Functional;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000018 RID: 24
	public interface IReadOnlyGanttTrackViewModel
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600004A RID: 74
		// (remove) Token: 0x0600004B RID: 75
		event EventHandler SelectionUpdated;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600004C RID: 76
		// (remove) Token: 0x0600004D RID: 77
		event EventHandler HighlightRegionUpdated;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004E RID: 78
		IReadOnlyList<Series> Series { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004F RID: 79
		Maybe<HighlightRegion> HighlightRegion { get; }

		// Token: 0x06000050 RID: 80
		bool IsSelected(Series series, Element element);

		// Token: 0x06000051 RID: 81
		bool IsSelected(Series series, Marker element);

		// Token: 0x06000052 RID: 82
		int GetSelectedObjectCount();

		// Token: 0x06000053 RID: 83
		int GetSelectedMarkerCount();
	}
}
