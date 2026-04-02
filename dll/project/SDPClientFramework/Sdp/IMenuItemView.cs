using System;

namespace Sdp
{
	// Token: 0x020001F5 RID: 501
	public interface IMenuItemView
	{
		// Token: 0x14000059 RID: 89
		// (add) Token: 0x0600073E RID: 1854
		// (remove) Token: 0x0600073F RID: 1855
		event EventHandler MenuItemActivated;

		// Token: 0x1700015A RID: 346
		// (set) Token: 0x06000740 RID: 1856
		bool Enabled { set; }
	}
}
