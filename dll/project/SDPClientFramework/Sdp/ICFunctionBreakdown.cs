using System;

namespace Sdp
{
	// Token: 0x02000214 RID: 532
	public class ICFunctionBreakdown
	{
		// Token: 0x060007D9 RID: 2009 RVA: 0x000157C3 File Offset: 0x000139C3
		public ICFunctionBreakdown()
		{
			this.CurrentFunction = new ICEventRegion();
			this.RelativeDepth = 0;
		}

		// Token: 0x0400077D RID: 1917
		public ICEventRegion CurrentFunction;

		// Token: 0x0400077E RID: 1918
		public int RelativeDepth;
	}
}
