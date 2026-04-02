using System;

namespace Sdp
{
	// Token: 0x0200025A RID: 602
	public class ToggleMenuItemController : IToggleMenuItemController, IMenuItemController
	{
		// Token: 0x170001E6 RID: 486
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x0001C05C File Offset: 0x0001A25C
		public IMenuItemView View
		{
			set
			{
				if (this.m_view != null)
				{
					this.m_view.MenuItemActivated -= this.m_view_MenuItemActivated;
				}
				this.m_view = value;
				this.m_view.MenuItemActivated += this.m_view_MenuItemActivated;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001C09C File Offset: 0x0001A29C
		private void m_view_MenuItemActivated(object sender, EventArgs e)
		{
			this.Active = !this.Active;
			if (this.m_command == null)
			{
				CreateViewCommand createViewCommand = new CreateViewCommand(this.Text);
				createViewCommand.Execute();
			}
			SdpApp.CommandManager.ExecuteCommand(this.m_command);
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0001C0E2 File Offset: 0x0001A2E2
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x0001C0EA File Offset: 0x0001A2EA
		public string Text { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0001C0F3 File Offset: 0x0001A2F3
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0001C0FB File Offset: 0x0001A2FB
		public ICommand Command
		{
			get
			{
				return this.m_command;
			}
			set
			{
				this.m_command = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0001C104 File Offset: 0x0001A304
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001C10C File Offset: 0x0001A30C
		public bool Active { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0001C115 File Offset: 0x0001A315
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0001C11D File Offset: 0x0001A31D
		public bool Enabled { get; set; } = true;

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001C126 File Offset: 0x0001A326
		public ToggleMenuItemController(string label, ICommand command)
		{
			this.m_command = command;
			this.Text = label;
		}

		// Token: 0x0400084B RID: 2123
		protected IMenuItemView m_view;

		// Token: 0x0400084F RID: 2127
		private ICommand m_command;
	}
}
