using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003C RID: 60
	public class DragEventArgs : EventArgs
	{
		// Token: 0x04000104 RID: 260
		public Point PreviousLocation;

		// Token: 0x04000105 RID: 261
		public Point CurrentLocation;

		// Token: 0x04000106 RID: 262
		public MouseButton Button;

		// Token: 0x04000107 RID: 263
		public KeyModifierFlag Modifiers;

		// Token: 0x04000108 RID: 264
		public bool Handled;
	}
}
