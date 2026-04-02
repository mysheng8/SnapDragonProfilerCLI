using System;

namespace SDPClientFramework.Views.EventHandlers.KeyboardEventHandler
{
	// Token: 0x02000047 RID: 71
	public class KeyPressedEventArgs : EventArgs
	{
		// Token: 0x04000127 RID: 295
		public Key Key;

		// Token: 0x04000128 RID: 296
		public KeyModifierFlag Modifer;

		// Token: 0x04000129 RID: 297
		public bool Handled;
	}
}
