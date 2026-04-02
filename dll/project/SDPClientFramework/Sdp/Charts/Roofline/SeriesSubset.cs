using System;
using System.Collections.Generic;

namespace Sdp.Charts.Roofline
{
	// Token: 0x020002FD RID: 765
	public class SeriesSubset
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00030856 File Offset: 0x0002EA56
		public Point Min
		{
			get
			{
				return this.Points[0];
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00030864 File Offset: 0x0002EA64
		public Point Max
		{
			get
			{
				return this.Points[this.Points.Count - 1];
			}
		}

		// Token: 0x04000AB6 RID: 2742
		public List<Point> Points;
	}
}
