using System;

namespace Sdp
{
	// Token: 0x020001BA RID: 442
	public interface IConnectionButton
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060005B1 RID: 1457
		// (remove) Token: 0x060005B2 RID: 1458
		event EventHandler<ButtonPressEventArgs> ConnectionButtonPressEvent;

		// Token: 0x060005B3 RID: 1459
		void SetConnected(bool connected, string device);
	}
}
