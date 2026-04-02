using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x02000041 RID: 65
	public class Point
	{
		// Token: 0x0600016A RID: 362 RVA: 0x0000204B File Offset: 0x0000024B
		public Point()
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005DEB File Offset: 0x00003FEB
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005E01 File Offset: 0x00004001
		public override string ToString()
		{
			return string.Format("[X: {0}; Y: {1}]", this.X, this.Y);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005E24 File Offset: 0x00004024
		public override bool Equals(object obj)
		{
			Point point = obj as Point;
			return point != null && this.X == point.X && this.Y == point.Y;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005E59 File Offset: 0x00004059
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x04000113 RID: 275
		public int X;

		// Token: 0x04000114 RID: 276
		public int Y;
	}
}
