using System;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200002C RID: 44
	public abstract class SeriesGanttObjectPair
	{
		// Token: 0x060000C9 RID: 201
		public abstract void Match(Action<SeriesElementPair> onElementPair, Action<SeriesMarkerPair> onMarkerPair);

		// Token: 0x060000CA RID: 202
		public abstract T Match<T>(Func<SeriesElementPair, T> onElementPair, Func<SeriesMarkerPair, T> onMarkerPair);
	}
}
