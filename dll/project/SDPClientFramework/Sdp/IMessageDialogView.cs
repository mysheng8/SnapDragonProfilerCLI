using System;

namespace Sdp
{
	// Token: 0x0200025F RID: 607
	public interface IMessageDialogView : IDialog
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A1D RID: 2589
		// (set) Token: 0x06000A1E RID: 2590
		IconType IconType { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A1F RID: 2591
		// (set) Token: 0x06000A20 RID: 2592
		ButtonLayout ButtonLayout { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A21 RID: 2593
		// (set) Token: 0x06000A22 RID: 2594
		string Title { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A23 RID: 2595
		// (set) Token: 0x06000A24 RID: 2596
		string Message { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A25 RID: 2597
		// (set) Token: 0x06000A26 RID: 2598
		bool HasDontShowAgainCheckBox { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A27 RID: 2599
		// (set) Token: 0x06000A28 RID: 2600
		bool DontShowAgainCheckBoxValue { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000A29 RID: 2601
		// (set) Token: 0x06000A2A RID: 2602
		IWindow TopLevelWindow { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000A2B RID: 2603
		// (set) Token: 0x06000A2C RID: 2604
		string AffirmativeText { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A2D RID: 2605
		// (set) Token: 0x06000A2E RID: 2606
		string NegativeText { get; set; }
	}
}
