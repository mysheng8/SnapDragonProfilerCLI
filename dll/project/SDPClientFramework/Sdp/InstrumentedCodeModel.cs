using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000211 RID: 529
	public class InstrumentedCodeModel
	{
		// Token: 0x060007D3 RID: 2003 RVA: 0x0001576C File Offset: 0x0001396C
		public InstrumentedCodeModel()
		{
			this.m_functions = new Dictionary<uint, string>();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000123A7 File Offset: 0x000105A7
		public List<ICFunctionBreakdown> GetFunctionBreakDownTree(uint pid, uint tid)
		{
			return null;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000123A7 File Offset: 0x000105A7
		public List<ICEventRegion> GetDebugRegions(uint pid, uint tid)
		{
			return null;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001577F File Offset: 0x0001397F
		public Dictionary<uint, string> Functions
		{
			get
			{
				return this.m_functions;
			}
		}

		// Token: 0x04000776 RID: 1910
		private Dictionary<uint, string> m_functions;
	}
}
