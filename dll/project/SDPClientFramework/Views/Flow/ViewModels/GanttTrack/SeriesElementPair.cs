using System;
using Sdp.Charts.Gantt;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200002D RID: 45
	public class SeriesElementPair : SeriesGanttObjectPair
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003B42 File Offset: 0x00001D42
		public int ElementIndex
		{
			get
			{
				return this.Series.Elements.IndexOf(this.Element);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003B5A File Offset: 0x00001D5A
		public SeriesElementPair()
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003B62 File Offset: 0x00001D62
		public SeriesElementPair(Series series, Element element)
		{
			this.Series = series;
			this.Element = element;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003B78 File Offset: 0x00001D78
		public override void Match(Action<SeriesElementPair> onElementPair, Action<SeriesMarkerPair> onMarkerPair)
		{
			onElementPair(this);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003B81 File Offset: 0x00001D81
		public override T Match<T>(Func<SeriesElementPair, T> onElementPair, Func<SeriesMarkerPair, T> onMarkerPair)
		{
			return onElementPair(this);
		}

		// Token: 0x040000DC RID: 220
		public Series Series;

		// Token: 0x040000DD RID: 221
		public Element Element;
	}
}
