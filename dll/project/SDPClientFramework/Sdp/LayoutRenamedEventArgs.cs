using System;

namespace Sdp
{
	// Token: 0x0200028A RID: 650
	public class LayoutRenamedEventArgs : EventArgs
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x00022A2D File Offset: 0x00020C2D
		public LayoutRenamedEventArgs(string old_name, string new_name)
		{
			this.OldName = old_name;
			this.NewName = new_name;
		}

		// Token: 0x040008D9 RID: 2265
		public string OldName;

		// Token: 0x040008DA RID: 2266
		public string NewName;
	}
}
