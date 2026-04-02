using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000212 RID: 530
	public class ICProcessInfo
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x00015787 File Offset: 0x00013987
		public ICProcessInfo()
		{
			this.FunctionBreakdowns = new Dictionary<uint, ICThreadInfo>();
			this.DebugRegions = new Dictionary<uint, List<ICEventRegion>>();
		}

		// Token: 0x04000777 RID: 1911
		public Dictionary<uint, ICThreadInfo> FunctionBreakdowns;

		// Token: 0x04000778 RID: 1912
		public Dictionary<uint, List<ICEventRegion>> DebugRegions;
	}
}
