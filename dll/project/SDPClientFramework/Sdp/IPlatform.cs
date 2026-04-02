using System;

namespace Sdp
{
	// Token: 0x02000258 RID: 600
	public interface IPlatform
	{
		// Token: 0x060009EF RID: 2543
		void Invoke(EventHandler handler);

		// Token: 0x060009F0 RID: 2544
		void SetIdleHandler(IdleHandler handler);

		// Token: 0x060009F1 RID: 2545
		void ExitApplication();

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009F2 RID: 2546
		int ScreenWidth { get; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009F3 RID: 2547
		int ScreenHeight { get; }
	}
}
