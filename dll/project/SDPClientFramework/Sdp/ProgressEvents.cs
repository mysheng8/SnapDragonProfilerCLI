using System;

namespace Sdp
{
	// Token: 0x020000F7 RID: 247
	public class ProgressEvents
	{
		// Token: 0x0400036D RID: 877
		public EventHandler<ProgressEventArgs> BeginProgress;

		// Token: 0x0400036E RID: 878
		public EventHandler<ProgressEventArgs> EndProgress;

		// Token: 0x0400036F RID: 879
		public EventHandler<ProgressEventArgs> UpdateProgress;
	}
}
