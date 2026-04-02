using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000031 RID: 49
	public class SeriesSelection
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00003C3C File Offset: 0x00001E3C
		public SeriesSelection()
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003C5A File Offset: 0x00001E5A
		public SeriesSelection(Series series, IEnumerable<Element> elements, IEnumerable<Marker> markers)
		{
			this.Series = series;
			this.Elements = elements;
			this.Markers = markers;
		}

		// Token: 0x040000E0 RID: 224
		public Series Series;

		// Token: 0x040000E1 RID: 225
		public IEnumerable<Element> Elements = new List<Element>();

		// Token: 0x040000E2 RID: 226
		public IEnumerable<Marker> Markers = new List<Marker>();
	}
}
