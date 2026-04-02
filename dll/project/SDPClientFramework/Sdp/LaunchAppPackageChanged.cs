using System;

namespace Sdp
{
	// Token: 0x020001D3 RID: 467
	public class LaunchAppPackageChanged : EventArgs
	{
		// Token: 0x040006C1 RID: 1729
		public LaunchApplicationFilters filter = LaunchApplicationFilters.THIRD_PARTY;

		// Token: 0x040006C2 RID: 1730
		public string package = "";

		// Token: 0x040006C3 RID: 1731
		public string activity = "";

		// Token: 0x040006C4 RID: 1732
		public bool shouldInvalidatePackageList = true;

		// Token: 0x040006C5 RID: 1733
		public bool shouldInvalidateActivityList = true;
	}
}
