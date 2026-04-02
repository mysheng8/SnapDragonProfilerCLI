using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020000D7 RID: 215
	public class FrameStatsViewInvalidateEventArgs : EventArgs
	{
		// Token: 0x040002FF RID: 767
		public TreeModel Model;

		// Token: 0x04000300 RID: 768
		public List<FrameStatsViewColumn> Columns;
	}
}
