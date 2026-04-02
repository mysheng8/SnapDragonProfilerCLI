using System;

namespace Sdp.Charts.Roofline
{
	// Token: 0x020002FE RID: 766
	public struct Point : IComparable
	{
		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003087E File Offset: 0x0002EA7E
		public Point(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00030890 File Offset: 0x0002EA90
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

		// Token: 0x04000AB7 RID: 2743
		public double X;

		// Token: 0x04000AB8 RID: 2744
		public double Y;
	}
}
