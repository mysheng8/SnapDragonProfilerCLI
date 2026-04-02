using System;

namespace Sdp
{
	// Token: 0x02000074 RID: 116
	public class SetDataBoundsEventArgs : EventArgs
	{
		// Token: 0x0400019C RID: 412
		public long max;

		// Token: 0x0400019D RID: 413
		public long min;

		// Token: 0x0400019E RID: 414
		public bool ResetDataViewBounds;
	}
}
