using System;
using System.Collections.Generic;

namespace Sdp.Charts.Graph
{
	// Token: 0x020002FA RID: 762
	public class PointsContainer
	{
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06000F8A RID: 3978 RVA: 0x00030230 File Offset: 0x0002E430
		// (remove) Token: 0x06000F8B RID: 3979 RVA: 0x00030268 File Offset: 0x0002E468
		public event EventHandler<PointsContainer.PointAddedArgs> PointAdded;

		// Token: 0x06000F8C RID: 3980 RVA: 0x0003029D File Offset: 0x0002E49D
		public int BinarySearch(Point p)
		{
			return this.m_points.BinarySearch(p);
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x000302AB File Offset: 0x0002E4AB
		public int Count
		{
			get
			{
				return this.m_points.Count;
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000302B8 File Offset: 0x0002E4B8
		public List<Point>.Enumerator GetEnumerator()
		{
			return this.m_points.GetEnumerator();
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x000302C5 File Offset: 0x0002E4C5
		public List<Point> GetRange(int start, int count)
		{
			return this.m_points.GetRange(start, count);
		}

		// Token: 0x170002D7 RID: 727
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

		// Token: 0x06000F92 RID: 3986 RVA: 0x000302F4 File Offset: 0x0002E4F4
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

		// Token: 0x06000F93 RID: 3987 RVA: 0x0003032D File Offset: 0x0002E52D
		public void Clear()
		{
			this.m_points.Clear();
		}

		// Token: 0x04000AA6 RID: 2726
		private List<Point> m_points = new List<Point>();

		// Token: 0x020003F0 RID: 1008
		public class PointAddedArgs : EventArgs
		{
			// Token: 0x04000DC9 RID: 3529
			public Point P;
		}
	}
}
