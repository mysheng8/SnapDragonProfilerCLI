using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000233 RID: 563
	public class MetricDoubleClickedEventArgs : EventArgs
	{
		// Token: 0x040007F5 RID: 2037
		public uint MetricID;

		// Token: 0x040007F6 RID: 2038
		public List<IdNamePair> SelectedProcesses;
	}
}
