using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000029 RID: 41
	internal interface ISelectionViewModel : IReadOnlySelectionViewModel
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C3 RID: 195
		List<Series> Series { get; }

		// Token: 0x060000C4 RID: 196
		void SelectElement(SeriesElementPair newSelection, SelectionType modifier);

		// Token: 0x060000C5 RID: 197
		void SelectMarker(SeriesMarkerPair newSelection, SelectionType modifier);

		// Token: 0x060000C6 RID: 198
		void SelectObjectsInHighlightRegion(HighlightRegion highlightRegion);

		// Token: 0x060000C7 RID: 199
		void MoveSelection(MoveDirection direction, SelectionType modifier);

		// Token: 0x060000C8 RID: 200
		void ClearSelections();
	}
}
