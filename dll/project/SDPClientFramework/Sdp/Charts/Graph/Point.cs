using System;

namespace Sdp.Charts.Graph
{
	// Token: 0x020002F9 RID: 761
	public struct Point : IComparable
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x000301DD File Offset: 0x0002E3DD
		public Point(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000301F0 File Offset: 0x0002E3F0
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (obj is Point)
			{
				Point point = (Point)obj;
				return this.X.CompareTo(point.X);
			}
			throw new Exception("Object isn't a Point!");
		}

		// Token: 0x04000AA3 RID: 2723
		public double X;

		// Token: 0x04000AA4 RID: 2724
		public double Y;
	}
}
