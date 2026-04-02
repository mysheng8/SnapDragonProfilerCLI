using System;

namespace Sdp
{
	// Token: 0x020000F8 RID: 248
	public class ProgressEventArgs : EventArgs
	{
		// Token: 0x06000391 RID: 913 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public ProgressEventArgs(ProgressObject progressObject)
		{
			this.ProgressObject = progressObject;
		}

		// Token: 0x04000370 RID: 880
		public ProgressObject ProgressObject;
	}
}
