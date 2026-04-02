using System;

namespace Sdp
{
	// Token: 0x020001B8 RID: 440
	public interface IGuidedSetupView
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060005A9 RID: 1449
		// (remove) Token: 0x060005AA RID: 1450
		event EventHandler<TakeCaptureArgs> CaptureButtonToggled;

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005AB RID: 1451
		ILaunchApplicationDialog LaunchApp { get; }

		// Token: 0x17000116 RID: 278
		// (set) Token: 0x060005AC RID: 1452
		uint CaptureDuration { set; }

		// Token: 0x060005AD RID: 1453
		void SelectNotebookPage(int page);
	}
}
