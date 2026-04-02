using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003D RID: 61
	public interface IMouseEventHandler
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600015F RID: 351
		// (remove) Token: 0x06000160 RID: 352
		event EventHandler<MouseButtonPressedArgs> MouseButtonPressed;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000161 RID: 353
		// (remove) Token: 0x06000162 RID: 354
		event EventHandler<MouseMovedArgs> MouseMoved;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000163 RID: 355
		// (remove) Token: 0x06000164 RID: 356
		event EventHandler<MouseButtonReleasedArgs> MouseButtonReleased;

		// Token: 0x06000165 RID: 357
		Point ToLocal(Point global);

		// Token: 0x06000166 RID: 358
		Point ToGlobal(Point global);
	}
}
