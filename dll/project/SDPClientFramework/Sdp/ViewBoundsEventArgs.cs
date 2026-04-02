using System;

namespace Sdp
{
	// Token: 0x02000296 RID: 662
	public class ViewBoundsEventArgs : EventArgs
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x00023262 File Offset: 0x00021462
		public ViewBoundsEventArgs(bool fast)
		{
			this.IsFast = fast;
		}

		// Token: 0x04000921 RID: 2337
		public bool IsFast;
	}
}
