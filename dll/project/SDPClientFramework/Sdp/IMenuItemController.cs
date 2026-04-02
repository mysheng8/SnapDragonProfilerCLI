using System;

namespace Sdp
{
	// Token: 0x020001F4 RID: 500
	public interface IMenuItemController
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600073A RID: 1850
		string Text { get; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600073B RID: 1851
		ICommand Command { get; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600073C RID: 1852
		bool Enabled { get; }

		// Token: 0x17000159 RID: 345
		// (set) Token: 0x0600073D RID: 1853
		IMenuItemView View { set; }
	}
}
