using System;
using Cairo;

namespace Sdp
{
	// Token: 0x0200023E RID: 574
	public class GraphTrackMetricDesc
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0001B812 File Offset: 0x00019A12
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x0001B81A File Offset: 0x00019A1A
		public MetricDesc Metric { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0001B823 File Offset: 0x00019A23
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0001B82B File Offset: 0x00019A2B
		public Color Color { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0001B834 File Offset: 0x00019A34
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0001B83C File Offset: 0x00019A3C
		public uint CaptureId { get; set; }
	}
}
