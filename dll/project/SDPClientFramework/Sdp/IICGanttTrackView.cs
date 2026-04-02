using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200024C RID: 588
	public interface IICGanttTrackView
	{
		// Token: 0x06000993 RID: 2451
		void AddThreadData(uint tid, List<List<ICFunctionBreakdown>> depthTable);

		// Token: 0x06000994 RID: 2452
		void AddThreadMarkers(uint tid, List<ICMarker> markers);

		// Token: 0x06000995 RID: 2453
		void AddRegions(List<ICEventRegion> regions);

		// Token: 0x06000996 RID: 2454
		void ExecuteViewBoundsCommand(double min, double max, bool dirty);

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000997 RID: 2455
		// (set) Token: 0x06000998 RID: 2456
		Dictionary<uint, string> NameStringsModel { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000999 RID: 2457
		// (set) Token: 0x0600099A RID: 2458
		HashSet<uint> StringIDsToRender { get; set; }
	}
}
