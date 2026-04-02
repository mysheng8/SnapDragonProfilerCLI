using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000213 RID: 531
	public class ICThreadInfo
	{
		// Token: 0x060007D8 RID: 2008 RVA: 0x000157A5 File Offset: 0x000139A5
		public ICThreadInfo()
		{
			this.Functions = new List<ICFunctionBreakdown>();
			this.Markers = new List<ICMarker>();
		}

		// Token: 0x04000779 RID: 1913
		public List<ICFunctionBreakdown> Functions;

		// Token: 0x0400077A RID: 1914
		public List<ICMarker> Markers;

		// Token: 0x0400077B RID: 1915
		public int MinDepth;

		// Token: 0x0400077C RID: 1916
		public int MaxDepth;
	}
}
