using System;
using Sdp;
using SDPClientFramework.Views.EventHandlers;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;

namespace SDPClientFramework.Views.Flow.Controllers
{
	// Token: 0x02000035 RID: 53
	internal class DataViewMouseClickEventArgs : EventArgs
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00004F42 File Offset: 0x00003142
		public DataViewMouseClickEventArgs(MouseClickEventArgs args, IDataViewMouseEventHandler handler)
		{
			this.Location = new DataViewPoint(args.Location, handler);
			this.Button = args.Button;
			this.ClickType = args.ClickType;
			this.Modifiers = args.Modifiers;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004F80 File Offset: 0x00003180
		public bool IsLeftClick()
		{
			return this.Button == MouseButton.Left && this.ClickType == ClickType.SingleClick;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004F95 File Offset: 0x00003195
		public bool IsShiftLeftClick()
		{
			return this.Button == MouseButton.Left && this.ClickType == ClickType.SingleClick && this.Modifiers.HasModifer(KeyModifierFlag.Shift);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004FB5 File Offset: 0x000031B5
		public bool IsDoubleLeftClick()
		{
			return this.Button == MouseButton.Left && this.ClickType == ClickType.DoubleClick;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004FCA File Offset: 0x000031CA
		public bool IsRightClick()
		{
			return this.Button == MouseButton.Right;
		}

		// Token: 0x040000ED RID: 237
		public DataViewPoint Location;

		// Token: 0x040000EE RID: 238
		public MouseButton Button;

		// Token: 0x040000EF RID: 239
		public ClickType ClickType;

		// Token: 0x040000F0 RID: 240
		public KeyModifierFlag Modifiers;
	}
}
