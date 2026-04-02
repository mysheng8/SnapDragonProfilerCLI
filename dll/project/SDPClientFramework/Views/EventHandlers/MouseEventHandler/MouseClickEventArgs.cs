using System;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x0200003B RID: 59
	public class MouseClickEventArgs : EventArgs
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000249B File Offset: 0x0000069B
		public MouseClickEventArgs()
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005DB3 File Offset: 0x00003FB3
		public MouseClickEventArgs(MouseButtonPressedArgs e)
		{
			this.Location = e.Location;
			this.Button = e.Button;
			this.ClickType = e.ClickType;
			this.Modifiers = e.Modifiers;
		}

		// Token: 0x04000100 RID: 256
		public Point Location;

		// Token: 0x04000101 RID: 257
		public MouseButton Button;

		// Token: 0x04000102 RID: 258
		public ClickType ClickType;

		// Token: 0x04000103 RID: 259
		public KeyModifierFlag Modifiers;
	}
}
