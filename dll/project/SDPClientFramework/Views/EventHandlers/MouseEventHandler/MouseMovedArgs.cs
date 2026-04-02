using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003F RID: 63
	public class MouseMovedArgs : EventArgs
	{
		// Token: 0x0400010E RID: 270
		public Point NewPosition;

		// Token: 0x0400010F RID: 271
		public KeyModifierFlag Modifiers;

		// Token: 0x04000110 RID: 272
		public bool Handled;
	}
}
