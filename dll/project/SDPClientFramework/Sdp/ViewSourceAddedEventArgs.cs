using System;

namespace Sdp
{
	// Token: 0x020000A9 RID: 169
	public class ViewSourceAddedEventArgs : MultiViewArgs
	{
		// Token: 0x0600033D RID: 829 RVA: 0x00009811 File Offset: 0x00007A11
		public ViewSourceAddedEventArgs(ViewSource vs)
		{
			this.ViewSource = vs;
			this.UniqueID = vs.UniqueID;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000982C File Offset: 0x00007A2C
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00009834 File Offset: 0x00007A34
		public ViewSource ViewSource { get; set; }
	}
}
