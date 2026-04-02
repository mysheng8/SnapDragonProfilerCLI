using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000019 RID: 25
	internal interface IGanttTrackViewModel : IReadOnlyGanttTrackViewModel
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000054 RID: 84
		List<Series> Series { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000055 RID: 85
		InspectorViewModel SingleSelectInspectorViewModel { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000056 RID: 86
		InspectorViewModel MultiSelectInspectorViewModel { get; }

		// Token: 0x06000057 RID: 87
		int GetSelectedSeriesCount();

		// Token: 0x06000058 RID: 88
		Maybe<SeriesGanttObjectPair> GetLastSelected();

		// Token: 0x06000059 RID: 89
		Maybe<SeriesElementPair> GetElementAtPoint(DataViewPoint point);

		// Token: 0x0600005A RID: 90
		Maybe<SeriesGanttObjectPair> GetGanttObjectAtPoint(DataViewPoint point);

		// Token: 0x0600005B RID: 91
		void SelectElement(SeriesElementPair newSelection, SelectionType modifier);

		// Token: 0x0600005C RID: 92
		void SelectMarker(SeriesMarkerPair newSelection, SelectionType modifier);

		// Token: 0x0600005D RID: 93
		void SelectElementsInHighlightRegion();

		// Token: 0x0600005E RID: 94
		void MoveSelection(MoveDirection direction, SelectionType modifier);

		// Token: 0x0600005F RID: 95
		void ClearSelections();

		// Token: 0x06000060 RID: 96
		void SetHighlightRegion(DataViewPoint begin, DataViewPoint end, HighlightRegionType highlightType);

		// Token: 0x06000061 RID: 97
		void ClearHighlightRegion();
	}
}
