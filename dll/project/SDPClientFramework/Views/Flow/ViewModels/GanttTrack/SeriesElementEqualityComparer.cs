using System;
using System.Collections.Generic;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200002E RID: 46
	public class SeriesElementEqualityComparer : IEqualityComparer<SeriesElementPair>
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00003B8A File Offset: 0x00001D8A
		public bool Equals(SeriesElementPair first, SeriesElementPair second)
		{
			return first.Series == second.Series && first.Element == second.Element;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003BAA File Offset: 0x00001DAA
		public int GetHashCode(SeriesElementPair pair)
		{
			return pair.Series.GetHashCode() ^ pair.Element.GetHashCode();
		}
	}
}
