using System;

namespace Sdp
{
	// Token: 0x02000251 RID: 593
	public interface IMainWindow : IView
	{
		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060009C0 RID: 2496
		// (remove) Token: 0x060009C1 RID: 2497
		event EventHandler ViewClosing;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060009C2 RID: 2498
		// (remove) Token: 0x060009C3 RID: 2499
		event EventHandler ViewClosed;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060009C4 RID: 2500
		// (remove) Token: 0x060009C5 RID: 2501
		event EventHandler FocusIn;

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060009C6 RID: 2502
		IMenuBar MainMenu { get; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060009C7 RID: 2503
		IStatusBar MainStatusBar { get; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060009C8 RID: 2504
		IDockHost DockingHost { get; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060009C9 RID: 2505
		IWindow TopLevelWindow { get; }

		// Token: 0x060009CA RID: 2506
		void ShowView();

		// Token: 0x060009CB RID: 2507
		void HideView();

		// Token: 0x060009CC RID: 2508
		void UpdateControls();

		// Token: 0x060009CD RID: 2509
		void CopySelectedContent();

		// Token: 0x060009CE RID: 2510
		void SelectAllContent();

		// Token: 0x060009CF RID: 2511
		void SavingChanged(SavingSession e);

		// Token: 0x060009D0 RID: 2512
		void AddCaptureTab(IDockWindow window);

		// Token: 0x060009D1 RID: 2513
		void HideCaptureTab(string windowName);

		// Token: 0x060009D2 RID: 2514
		void FocusCaptureTab(string windowName);
	}
}
