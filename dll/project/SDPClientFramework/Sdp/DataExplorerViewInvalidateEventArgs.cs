using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000140 RID: 320
	public class DataExplorerViewInvalidateEventArgs : MultiViewArgs
	{
		// Token: 0x0400049A RID: 1178
		public TreeModel Model;

		// Token: 0x0400049B RID: 1179
		public List<DataExplorerViewColumn> Columns = new List<DataExplorerViewColumn>();

		// Token: 0x0400049C RID: 1180
		public List<DataExplorerViewFilter> Filters = new List<DataExplorerViewFilter>();

		// Token: 0x0400049D RID: 1181
		public int ExpanderColumnIndex;

		// Token: 0x0400049E RID: 1182
		public bool DisableHighlightFilter;
	}
}
