using System;

namespace Sdp
{
	// Token: 0x02000259 RID: 601
	public class RadioMenuItemController : IRadioMenuItemController, IMenuItemController
	{
		// Token: 0x170001E1 RID: 481
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0001BFB2 File Offset: 0x0001A1B2
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

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001BFF1 File Offset: 0x0001A1F1
		private void m_view_MenuItemActivated(object sender, EventArgs e)
		{
			if (this.m_command != null)
			{
				SdpApp.CommandManager.ExecuteCommand(this.m_command);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0001C00B File Offset: 0x0001A20B
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0001C013 File Offset: 0x0001A213
		public string Text { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0001C01C File Offset: 0x0001A21C
		public ICommand Command
		{
			get
			{
				return this.m_command;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0001C024 File Offset: 0x0001A224
		// (set) Token: 0x060009FA RID: 2554 RVA: 0x0001C02C File Offset: 0x0001A22C
		public bool Enabled { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0001C035 File Offset: 0x0001A235
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x0001C03D File Offset: 0x0001A23D
		public bool Active { get; set; }

		// Token: 0x060009FD RID: 2557 RVA: 0x0001C046 File Offset: 0x0001A246
		public RadioMenuItemController(string label, ICommand command)
		{
			this.m_command = command;
			this.Text = label;
		}

		// Token: 0x04000846 RID: 2118
		protected IMenuItemView m_view;

		// Token: 0x04000848 RID: 2120
		private ICommand m_command;
	}
}
