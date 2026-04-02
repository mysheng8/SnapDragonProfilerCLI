using System;

namespace SDPClientFramework.Views.EventHandlers.KeyboardEventHandler
{
	// Token: 0x02000046 RID: 70
	public interface IKeyboardEventHandler
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000184 RID: 388
		// (remove) Token: 0x06000185 RID: 389
		event EventHandler<KeyPressedEventArgs> KeyPressed;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000186 RID: 390
		// (remove) Token: 0x06000187 RID: 391
		event EventHandler<KeyPressedEventArgs> KeyReleased;
	}
}
