using System;

namespace Sdp
{
	// Token: 0x0200025E RID: 606
	public class WindowMenuItem : IMenuItemController
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0001C40A File Offset: 0x0001A60A
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0001C412 File Offset: 0x0001A612
		public IMenuItemView View
		{
			get
			{
				return this.m_view;
			}
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

		// Token: 0x06000A16 RID: 2582 RVA: 0x0001C451 File Offset: 0x0001A651
		private void m_view_MenuItemActivated(object sender, EventArgs e)
		{
			if (this.m_command != null)
			{
				SdpApp.CommandManager.ExecuteCommand(this.m_command);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0001C46B File Offset: 0x0001A66B
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0001C473 File Offset: 0x0001A673
		public string Text { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0001C47C File Offset: 0x0001A67C
		public ICommand Command
		{
			get
			{
				return this.m_command;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0001C484 File Offset: 0x0001A684
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0001C48C File Offset: 0x0001A68C
		public bool Enabled { get; set; } = true;

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001C495 File Offset: 0x0001A695
		public WindowMenuItem(string label, ICommand command)
		{
			this.Text = label;
			this.m_command = command;
		}

		// Token: 0x04000850 RID: 2128
		private ICommand m_command;

		// Token: 0x04000851 RID: 2129
		private IMenuItemView m_view;
	}
}
