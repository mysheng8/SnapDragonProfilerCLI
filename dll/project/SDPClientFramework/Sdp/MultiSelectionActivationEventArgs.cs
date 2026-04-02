using System;

namespace Sdp
{
	// Token: 0x020000E8 RID: 232
	public class MultiSelectionActivationEventArgs : EventArgs
	{
		// Token: 0x06000380 RID: 896 RVA: 0x00009B37 File Offset: 0x00007D37
		public MultiSelectionActivationEventArgs(bool MultiSelect)
		{
			this.MultiSelect = MultiSelect;
		}

		// Token: 0x04000346 RID: 838
		public bool MultiSelect;

		// Token: 0x04000347 RID: 839
		public string Description = "";
	}
}
