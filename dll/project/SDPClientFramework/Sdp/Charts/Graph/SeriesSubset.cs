using System;
using System.Collections.Generic;

namespace Sdp.Charts.Graph
{
	// Token: 0x020002F8 RID: 760
	public class SeriesSubset
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x000301B5 File Offset: 0x0002E3B5
		public Point Min
		{
			get
			{
				return this.Points[0];
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x000301C3 File Offset: 0x0002E3C3
		public Point Max
		{
			get
			{
				return this.Points[this.Points.Count - 1];
			}
		}

		// Token: 0x04000AA2 RID: 2722
		public List<Point> Points;
	}
}
