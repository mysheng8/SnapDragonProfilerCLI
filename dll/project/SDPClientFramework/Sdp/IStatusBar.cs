using System;

namespace Sdp
{
	// Token: 0x0200018E RID: 398
	public interface IStatusBar
	{
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060004AC RID: 1196
		// (remove) Token: 0x060004AD RID: 1197
		event EventHandler<CaptureButtonArgs> CaptureTabClicked;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060004AE RID: 1198
		// (remove) Token: 0x060004AF RID: 1199
		event EventHandler<RequestRenameArgs> RequestRename;

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004B0 RID: 1200
		// (set) Token: 0x060004B1 RID: 1201
		bool Connected { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004B2 RID: 1202
		IProgressView ProgressView { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004B3 RID: 1203
		IConnectionButton ConnectionButton { get; }
	}
}
