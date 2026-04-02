using System;
using Cairo;

namespace Sdp.Charts.Graph
{
	// Token: 0x020002F6 RID: 758
	public class Series
	{
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06000F6E RID: 3950 RVA: 0x0002FE58 File Offset: 0x0002E058
		// (remove) Token: 0x06000F6F RID: 3951 RVA: 0x0002FE90 File Offset: 0x0002E090
		public event EventHandler<EventArgs> ResetMinMax;

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0002FEC5 File Offset: 0x0002E0C5
		// (set) Token: 0x06000F71 RID: 3953 RVA: 0x0002FECD File Offset: 0x0002E0CD
		public int SeriesID { get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0002FED6 File Offset: 0x0002E0D6
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0002FEDE File Offset: 0x0002E0DE
		public string Name { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002FEE7 File Offset: 0x0002E0E7
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x0002FEEF File Offset: 0x0002E0EF
		public Color Color { get; set; }

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002FEF8 File Offset: 0x0002E0F8
		public Series()
		{
			this.Points.PointAdded += this.points_PointAdded;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002FF4C File Offset: 0x0002E14C
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

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002FFC4 File Offset: 0x0002E1C4
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

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002FFF0 File Offset: 0x0002E1F0
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

		// Token: 0x04000A9C RID: 2716
		public double MinHeight = double.MaxValue;

		// Token: 0x04000A9D RID: 2717
		public double MaxHeight = double.MinValue;

		// Token: 0x04000A9E RID: 2718
		public readonly PointsContainer Points = new PointsContainer();
	}
}
