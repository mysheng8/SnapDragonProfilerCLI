using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200020C RID: 524
	public class MetricComparer : IEqualityComparer<uint>
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x000154A2 File Offset: 0x000136A2
		public bool Equals(uint x, uint y)
		{
			return x == y;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000154A8 File Offset: 0x000136A8
		public int GetHashCode(uint m)
		{
			return (int)m;
		}
	}
}
