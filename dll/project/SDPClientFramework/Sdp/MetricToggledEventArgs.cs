using System;

namespace Sdp
{
	// Token: 0x02000232 RID: 562
	public class MetricToggledEventArgs : EventArgs
	{
		// Token: 0x040007F0 RID: 2032
		public uint Id;

		// Token: 0x040007F1 RID: 2033
		public string Name;

		// Token: 0x040007F2 RID: 2034
		public bool Enabled;

		// Token: 0x040007F3 RID: 2035
		public bool Activatable;

		// Token: 0x040007F4 RID: 2036
		public string Path;
	}
}
