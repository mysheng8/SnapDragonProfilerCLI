using System;

namespace Sdp
{
	// Token: 0x020000EF RID: 239
	public class OptionValueChangedEventArgs : EventArgs
	{
		// Token: 0x04000356 RID: 854
		public string OptionName;

		// Token: 0x04000357 RID: 855
		public object NewValue;

		// Token: 0x04000358 RID: 856
		public bool IsLocalChange;

		// Token: 0x04000359 RID: 857
		public uint Pid;
	}
}
