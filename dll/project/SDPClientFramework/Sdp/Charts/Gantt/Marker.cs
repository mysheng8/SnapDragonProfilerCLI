using System;
using Cairo;

namespace Sdp.Charts.Gantt
{
	// Token: 0x020002F4 RID: 756
	public class Marker
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x0002FE26 File Offset: 0x0002E026
		public Marker()
		{
			this.Color = default(Color);
			this.Style = MarkerStyle.None;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002FE41 File Offset: 0x0002E041
		public override string ToString()
		{
			return string.Format("Position: {0}", this.Position);
		}

		// Token: 0x04000A8F RID: 2703
		public Color Color;

		// Token: 0x04000A90 RID: 2704
		public MarkerStyle Style;

		// Token: 0x04000A91 RID: 2705
		public long Position;

		// Token: 0x04000A92 RID: 2706
		public uint TooltipId;

		// Token: 0x04000A93 RID: 2707
		public object Tag;
	}
}
