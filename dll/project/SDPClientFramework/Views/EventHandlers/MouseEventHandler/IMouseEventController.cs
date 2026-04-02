using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003A RID: 58
	internal interface IMouseEventController
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000154 RID: 340
		// (remove) Token: 0x06000155 RID: 341
		event EventHandler<DragEventArgs> DragBegin;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000156 RID: 342
		// (remove) Token: 0x06000157 RID: 343
		event EventHandler<DragEventArgs> DragMove;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000158 RID: 344
		// (remove) Token: 0x06000159 RID: 345
		event EventHandler<DragEventArgs> DragEnded;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600015A RID: 346
		// (remove) Token: 0x0600015B RID: 347
		event EventHandler<MouseClickEventArgs> MouseButtonClicked;
	}
}
