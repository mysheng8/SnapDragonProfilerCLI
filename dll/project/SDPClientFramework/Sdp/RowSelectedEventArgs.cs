using System;

namespace Sdp
{
	// Token: 0x020002BA RID: 698
	public class RowSelectedEventArgs : EventArgs
	{
		// Token: 0x0400099B RID: 2459
		public object[] SelectedRow;

		// Token: 0x0400099C RID: 2460
		public int numClicks;

		// Token: 0x0400099D RID: 2461
		public bool IsUserSelection;

		// Token: 0x0400099E RID: 2462
		public bool ShaderProfilingEnabled;
	}
}
