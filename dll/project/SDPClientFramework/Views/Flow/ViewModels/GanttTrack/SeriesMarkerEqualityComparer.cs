using System;
using System.Collections.Generic;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000030 RID: 48
	public class SeriesMarkerEqualityComparer : IEqualityComparer<SeriesMarkerPair>
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00003C03 File Offset: 0x00001E03
		public bool Equals(SeriesMarkerPair first, SeriesMarkerPair second)
		{
			return first.Series == second.Series && first.Marker == second.Marker;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003C23 File Offset: 0x00001E23
		public int GetHashCode(SeriesMarkerPair pair)
		{
			return pair.Marker.GetHashCode() ^ pair.Marker.GetHashCode();
		}
	}
}
