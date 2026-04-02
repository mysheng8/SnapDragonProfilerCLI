using System;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200001D RID: 29
	internal interface IHighlightViewModel : IReadOnlyHighlightViewModel
	{
		// Token: 0x0600008D RID: 141
		void SetHighlightRegion(DataViewPoint begin, DataViewPoint end, HighlightRegionType highlightType);

		// Token: 0x0600008E RID: 142
		void ClearHighlightRegion();
	}
}
