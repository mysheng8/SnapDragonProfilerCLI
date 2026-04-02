using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000230 RID: 560
	public class IdNamePairComparer : IEqualityComparer<IdNamePair>
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x0001A7CA File Offset: 0x000189CA
		public bool Equals(IdNamePair x, IdNamePair y)
		{
			return x.Id == y.Id && string.Compare(x.Name, y.Name) == 0 && x.Enabled == y.Enabled;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001A7FD File Offset: 0x000189FD
		public int GetHashCode(IdNamePair pair)
		{
			return pair.Name.GetHashCode() + (int)pair.Id + pair.Enabled.GetHashCode();
		}
	}
}
