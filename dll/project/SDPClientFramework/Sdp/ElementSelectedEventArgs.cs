using System;
using Sdp.Charts.Gantt;

namespace Sdp
{
	// Token: 0x02000249 RID: 585
	public class ElementSelectedEventArgs : EventArgs
	{
		// Token: 0x04000823 RID: 2083
		public Element Selected;

		// Token: 0x04000824 RID: 2084
		public Series ElementSeries;

		// Token: 0x04000825 RID: 2085
		public int SelectedElementCount;
	}
}
