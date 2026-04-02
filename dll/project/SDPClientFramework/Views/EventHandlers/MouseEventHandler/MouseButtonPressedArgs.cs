using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003E RID: 62
	public class MouseButtonPressedArgs : EventArgs
	{
		// Token: 0x04000109 RID: 265
		public Point Location;

		// Token: 0x0400010A RID: 266
		public MouseButton Button;

		// Token: 0x0400010B RID: 267
		public ClickType ClickType;

		// Token: 0x0400010C RID: 268
		public KeyModifierFlag Modifiers;

		// Token: 0x0400010D RID: 269
		public bool Handled;
	}
}
