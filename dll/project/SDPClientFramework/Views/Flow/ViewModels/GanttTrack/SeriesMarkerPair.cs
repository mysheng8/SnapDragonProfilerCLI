using System;
using Sdp.Charts.Gantt;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200002F RID: 47
	public class SeriesMarkerPair : SeriesGanttObjectPair
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003BC3 File Offset: 0x00001DC3
		public int MarkerIndex
		{
			get
			{
				return this.Series.Markers.IndexOf(this.Marker);
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003B5A File Offset: 0x00001D5A
		public SeriesMarkerPair()
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003BDB File Offset: 0x00001DDB
		public SeriesMarkerPair(Series series, Marker marker)
		{
			this.Series = series;
			this.Marker = marker;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003BF1 File Offset: 0x00001DF1
		public override void Match(Action<SeriesElementPair> onElementPair, Action<SeriesMarkerPair> onMarkerPair)
		{
			onMarkerPair(this);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003BFA File Offset: 0x00001DFA
		public override T Match<T>(Func<SeriesElementPair, T> onElementPair, Func<SeriesMarkerPair, T> onMarkerPair)
		{
			return onMarkerPair(this);
		}

		// Token: 0x040000DE RID: 222
		public Series Series;

		// Token: 0x040000DF RID: 223
		public Marker Marker;
	}
}
