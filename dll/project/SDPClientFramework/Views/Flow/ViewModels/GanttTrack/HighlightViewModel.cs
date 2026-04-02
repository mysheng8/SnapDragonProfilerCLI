using System;
using Sdp.Functional;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200001F RID: 31
	internal class HighlightViewModel : IHighlightViewModel, IReadOnlyHighlightViewModel
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000093 RID: 147 RVA: 0x000033E4 File Offset: 0x000015E4
		// (remove) Token: 0x06000094 RID: 148 RVA: 0x0000341C File Offset: 0x0000161C
		public event EventHandler HighlightRegionUpdated;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003451 File Offset: 0x00001651
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003459 File Offset: 0x00001659
		public Maybe<HighlightRegion> HighlightRegion { get; private set; } = new Maybe<HighlightRegion>.None();

		// Token: 0x06000097 RID: 151 RVA: 0x00003462 File Offset: 0x00001662
		public void SetHighlightRegion(DataViewPoint begin, DataViewPoint end, HighlightRegionType type)
		{
			this.m_type = type;
			this.SetHighlightRegion(begin, end);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003473 File Offset: 0x00001673
		private void SetHighlightRegion(DataViewPoint begin, DataViewPoint end)
		{
			this.HighlightRegion = new Maybe<HighlightRegion>.Some(new HighlightRegion
			{
				HighlightBegin = begin,
				HighlightEnd = end
			});
			EventHandler highlightRegionUpdated = this.HighlightRegionUpdated;
			if (highlightRegionUpdated == null)
			{
				return;
			}
			highlightRegionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000034A9 File Offset: 0x000016A9
		public void ClearHighlightRegion()
		{
			this.HighlightRegion = new Maybe<HighlightRegion>.None();
			EventHandler highlightRegionUpdated = this.HighlightRegionUpdated;
			if (highlightRegionUpdated == null)
			{
				return;
			}
			highlightRegionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x040000C2 RID: 194
		private HighlightRegionType m_type;
	}
}
