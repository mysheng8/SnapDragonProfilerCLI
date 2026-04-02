using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000121 RID: 289
	public class SetContextEventArgs : EventArgs
	{
		// Token: 0x04000410 RID: 1040
		public List<uint> ContextIDs;

		// Token: 0x04000411 RID: 1041
		public uint SelectedContext;
	}
}
