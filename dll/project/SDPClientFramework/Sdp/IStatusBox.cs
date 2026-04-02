using System;

namespace Sdp
{
	// Token: 0x020002B4 RID: 692
	public interface IStatusBox
	{
		// Token: 0x06000DEA RID: 3562
		void SetStatus(StatusType statusType, string statusBox, int duration, bool hasHelp = false, string helpTooltip = null);

		// Token: 0x06000DEB RID: 3563
		void HideStatus();

		// Token: 0x06000DEC RID: 3564
		void ShowStatus();
	}
}
