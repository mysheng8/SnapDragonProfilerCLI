using System;
using Sdp;
using SDPClientFramework.Views.EventHandlers;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;

namespace SDPClientFramework.Views.Flow.Controllers
{
	// Token: 0x02000036 RID: 54
	internal class DataViewDragEventArgs : EventArgs
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00004FD8 File Offset: 0x000031D8
		public DataViewDragEventArgs(DragEventArgs args, IDataViewMouseEventHandler handler)
		{
			this.PreviousLocation = new DataViewPoint(args.PreviousLocation, handler);
			this.CurrentLocation = new DataViewPoint(args.CurrentLocation, handler);
			this.Button = args.Button;
			this.Modifiers = args.Modifiers;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005027 File Offset: 0x00003227
		public bool IsLeftDrag()
		{
			return this.Button == MouseButton.Left;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005032 File Offset: 0x00003232
		public bool IsShiftLeftDrag()
		{
			return this.Button == MouseButton.Left && this.Modifiers.HasModifer(KeyModifierFlag.Shift);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000504A File Offset: 0x0000324A
		public bool IsShiftRightDrag()
		{
			return this.Button == MouseButton.Right && this.Modifiers.HasModifer(KeyModifierFlag.Shift);
		}

		// Token: 0x040000F1 RID: 241
		public DataViewPoint PreviousLocation;

		// Token: 0x040000F2 RID: 242
		public DataViewPoint CurrentLocation;

		// Token: 0x040000F3 RID: 243
		public MouseButton Button;

		// Token: 0x040000F4 RID: 244
		public KeyModifierFlag Modifiers;

		// Token: 0x040000F5 RID: 245
		public bool Handled;
	}
}
