using System;
using Cairo;

namespace Sdp.Charts.Roofline
{
	// Token: 0x020002FB RID: 763
	public class Series
	{
		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06000F95 RID: 3989 RVA: 0x00030350 File Offset: 0x0002E550
		// (remove) Token: 0x06000F96 RID: 3990 RVA: 0x00030388 File Offset: 0x0002E588
		public event EventHandler<EventArgs> ResetMinMax;

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x000303BD File Offset: 0x0002E5BD
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x000303C5 File Offset: 0x0002E5C5
		public int SeriesID { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x000303CE File Offset: 0x0002E5CE
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x000303D6 File Offset: 0x0002E5D6
		public string Name { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x000303DF File Offset: 0x0002E5DF
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x000303E7 File Offset: 0x0002E5E7
		public Color Color { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x000303F0 File Offset: 0x0002E5F0
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x000303F8 File Offset: 0x0002E5F8
		public bool IsRooflineCeiling { get; set; }

		// Token: 0x06000F9F RID: 3999 RVA: 0x00030404 File Offset: 0x0002E604
		public Series()
		{
			this.Points.PointAdded += this.points_PointAdded;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00030464 File Offset: 0x0002E664
		private void points_PointAdded(object sender, PointsContainer.PointAddedArgs e)
		{
			EventArgs eventArgs = null;
			if (e.P.Y > this.MaxHeight)
			{
				this.MaxHeight = e.P.Y;
				eventArgs = new EventArgs();
			}
			if (e.P.Y < this.MinHeight)
			{
				this.MinHeight = e.P.Y;
				eventArgs = new EventArgs();
			}
			if (eventArgs != null)
			{
				EventHandler<EventArgs> resetMinMax = this.ResetMinMax;
				if (resetMinMax == null)
				{
					return;
				}
				resetMinMax(this, eventArgs);
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000304DC File Offset: 0x0002E6DC
		public int GetIndexBeforePoint(Point p)
		{
			int num = this.Points.BinarySearch(p);
			if (num < 0)
			{
				num = ~num;
				if (num > 0)
				{
					num--;
				}
			}
			return num;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00030508 File Offset: 0x0002E708
		public int GetIndexAfterPoint(Point p)
		{
			int num = this.Points.BinarySearch(p);
			if (num < 0)
			{
				num = ~num;
				if (num > 0 && num < this.Points.Count)
				{
					num++;
				}
				if (num == this.Points.Count)
				{
					num--;
				}
			}
			return num;
		}

		// Token: 0x04000AAC RID: 2732
		public float LineWidth = 5f;

		// Token: 0x04000AAD RID: 2733
		public float Duration;

		// Token: 0x04000AAE RID: 2734
		public double MinHeight = double.MaxValue;

		// Token: 0x04000AAF RID: 2735
		public double MaxHeight = double.MinValue;

		// Token: 0x04000AB0 RID: 2736
		public readonly PointsContainer Points = new PointsContainer();
	}
}
