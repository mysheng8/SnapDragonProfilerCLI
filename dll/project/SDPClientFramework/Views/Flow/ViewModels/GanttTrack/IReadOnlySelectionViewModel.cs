using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000028 RID: 40
	internal interface IReadOnlySelectionViewModel
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060000B5 RID: 181
		// (remove) Token: 0x060000B6 RID: 182
		event EventHandler SelectionUpdated;

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B7 RID: 183
		IReadOnlyList<Series> Series { get; }

		// Token: 0x060000B8 RID: 184
		bool IsSelected(Series series, Element element);

		// Token: 0x060000B9 RID: 185
		bool IsSelected(Series series, Marker marker);

		// Token: 0x060000BA RID: 186
		int GetSelectedSeriesCount();

		// Token: 0x060000BB RID: 187
		int GetSelectedObjectCount();

		// Token: 0x060000BC RID: 188
		int GetSelectedMarkerCount();

		// Token: 0x060000BD RID: 189
		IEnumerable<SeriesSelection> GetSeriesSelections();

		// Token: 0x060000BE RID: 190
		Maybe<SeriesGanttObjectPair> GetGanttObjectAtPoint(DataViewPoint point);

		// Token: 0x060000BF RID: 191
		Maybe<SeriesElementPair> GetElementAtPoint(DataViewPoint point);

		// Token: 0x060000C0 RID: 192
		Maybe<SeriesGanttObjectPair> GetLastSelected();

		// Token: 0x060000C1 RID: 193
		long GetMaxTimestamp();

		// Token: 0x060000C2 RID: 194
		long GetMinTimestamp();
	}
}
