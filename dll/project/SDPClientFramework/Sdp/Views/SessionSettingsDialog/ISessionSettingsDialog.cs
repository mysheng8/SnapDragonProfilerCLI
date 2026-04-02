using System;

namespace Sdp.Views.SessionSettingsDialog
{
	// Token: 0x020002EE RID: 750
	public interface ISessionSettingsDialog : IDialog
	{
		// Token: 0x06000F4D RID: 3917
		SessionSettingsSelection ShowDialog(uint defaultAllocationSize, string defaultFileLocation);
	}
}
