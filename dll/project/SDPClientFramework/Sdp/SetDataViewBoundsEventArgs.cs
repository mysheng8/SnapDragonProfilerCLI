using System;

namespace Sdp
{
	// Token: 0x02000077 RID: 119
	public class SetDataViewBoundsEventArgs : EventArgs
	{
		// Token: 0x040001AC RID: 428
		public double max;

		// Token: 0x040001AD RID: 429
		public double min;

		// Token: 0x040001AE RID: 430
		public bool dirty;

		// Token: 0x040001AF RID: 431
		public bool ForceScrollOff;
	}
}
