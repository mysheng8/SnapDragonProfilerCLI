using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000240 RID: 576
	public class GanttTrackViewDesc : TrackViewDesc
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0001B84D File Offset: 0x00019A4D
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x0001B855 File Offset: 0x00019A55
		public uint ProcessId { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0001B85E File Offset: 0x00019A5E
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x0001B866 File Offset: 0x00019A66
		public List<uint> MetricIds { get; set; }
	}
}
