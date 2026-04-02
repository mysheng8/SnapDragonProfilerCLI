using System;
using Sdp.Functional;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200001C RID: 28
	public interface IReadOnlyHighlightViewModel
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600008A RID: 138
		// (remove) Token: 0x0600008B RID: 139
		event EventHandler HighlightRegionUpdated;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008C RID: 140
		Maybe<HighlightRegion> HighlightRegion { get; }
	}
}
