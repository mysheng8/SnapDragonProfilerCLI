using System;
using System.Collections.Generic;

namespace Sdp.Charts.Roofline
{
	// Token: 0x020002FF RID: 767
	public class PointsContainer
	{
		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06000FB7 RID: 4023 RVA: 0x000308D0 File Offset: 0x0002EAD0
		// (remove) Token: 0x06000FB8 RID: 4024 RVA: 0x00030908 File Offset: 0x0002EB08
		public event EventHandler<PointsContainer.PointAddedArgs> PointAdded;

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0003093D File Offset: 0x0002EB3D
		public int BinarySearch(Point p)
		{
			return this.m_points.BinarySearch(p);
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0003094B File Offset: 0x0002EB4B
		public int Count
		{
			get
			{
				return this.m_points.Count;
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00030958 File Offset: 0x0002EB58
		public List<Point>.Enumerator GetEnumerator()
		{
			return this.m_points.GetEnumerator();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00030965 File Offset: 0x0002EB65
		public List<Point> GetRange(int start, int count)
		{
			return this.m_points.GetRange(start, count);
		}

		// Token: 0x170002E5 RID: 741
		public Point this[int key]
		{
			get
			{
				return this.m_points[key];
			}
			set
			{
				this.m_points[key] = value;
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00030994 File Offset: 0x0002EB94
		public void Add(Point p)
		{
			PointsContainer.PointAddedArgs pointAddedArgs = new PointsContainer.PointAddedArgs();
			pointAddedArgs.P = p;
			EventHandler<PointsContainer.PointAddedArgs> pointAdded = this.PointAdded;
			if (pointAdded != null)
			{
				pointAdded(this, pointAddedArgs);
			}
			this.m_points.Add(p);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000309CD File Offset: 0x0002EBCD
		public void AddRoofline(Point p)
		{
			p.X = RooflineMath.LogToLinear(p.X);
			p.Y = RooflineMath.LogToLinear(p.Y);
			this.Add(p);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x000309FA File Offset: 0x0002EBFA
		public void Clear()
		{
			this.m_points.Clear();
		}

		// Token: 0x04000ABA RID: 2746
		private List<Point> m_points = new List<Point>();

		// Token: 0x020003F1 RID: 1009
		public class PointAddedArgs : EventArgs
		{
			// Token: 0x04000DCA RID: 3530
			public Point P;
		}
	}
}
