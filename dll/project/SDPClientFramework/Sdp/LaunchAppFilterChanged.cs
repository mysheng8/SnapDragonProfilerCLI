using System;

namespace Sdp
{
	// Token: 0x020001D6 RID: 470
	public class LaunchAppFilterChanged : EventArgs
	{
		// Token: 0x040006C8 RID: 1736
		public LaunchApplicationFilters Filter = LaunchApplicationFilters.THIRD_PARTY;

		// Token: 0x040006C9 RID: 1737
		public string SelectedPackage = "";
	}
}
