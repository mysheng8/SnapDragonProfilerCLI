using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000142 RID: 322
	public class DataExplorerViewRowSelectedEventArgs : MultiViewArgs
	{
		// Token: 0x040004A1 RID: 1185
		public int SourceID;

		// Token: 0x040004A2 RID: 1186
		public int CaptureID;

		// Token: 0x040004A3 RID: 1187
		public object[] SelectedRow;

		// Token: 0x040004A4 RID: 1188
		public int NumClicks;

		// Token: 0x040004A5 RID: 1189
		public bool IsUserSelection;

		// Token: 0x040004A6 RID: 1190
		public bool ShaderProfilingEnabled;

		// Token: 0x040004A7 RID: 1191
		public List<DataExplorerViewColumn> Columns = new List<DataExplorerViewColumn>();
	}
}
