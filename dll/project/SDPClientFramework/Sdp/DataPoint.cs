using System;

namespace Sdp
{
	// Token: 0x02000218 RID: 536
	public class DataPoint
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x000157EC File Offset: 0x000139EC
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x000157F4 File Offset: 0x000139F4
		public double X { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x000157FD File Offset: 0x000139FD
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00015805 File Offset: 0x00013A05
		public double Y { get; set; }

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001580E File Offset: 0x00013A0E
		public DataPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
