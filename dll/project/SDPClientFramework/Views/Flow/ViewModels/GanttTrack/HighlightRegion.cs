using System;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200001E RID: 30
	public class HighlightRegion
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00003384 File Offset: 0x00001584
		public override bool Equals(object other)
		{
			HighlightRegion highlightRegion = other as HighlightRegion;
			return highlightRegion != null && this.HighlightBegin.Equals(highlightRegion.HighlightBegin) && this.HighlightEnd.Equals(highlightRegion.HighlightEnd);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000033C1 File Offset: 0x000015C1
		public override string ToString()
		{
			return string.Format("Begin: {0}; End:{1};", this.HighlightBegin, this.HighlightEnd);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000033D9 File Offset: 0x000015D9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040000BE RID: 190
		public DataViewPoint HighlightBegin;

		// Token: 0x040000BF RID: 191
		public DataViewPoint HighlightEnd;
	}
}
