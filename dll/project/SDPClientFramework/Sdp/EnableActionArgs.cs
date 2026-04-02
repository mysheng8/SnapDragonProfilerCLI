using System;

namespace Sdp
{
	// Token: 0x020000B1 RID: 177
	public class EnableActionArgs : EventArgs
	{
		// Token: 0x06000348 RID: 840 RVA: 0x00009921 File Offset: 0x00007B21
		public EnableActionArgs(ActionEnum actionEnabled)
		{
			this.action = actionEnabled;
		}

		// Token: 0x0400028B RID: 651
		public ActionEnum action;
	}
}
