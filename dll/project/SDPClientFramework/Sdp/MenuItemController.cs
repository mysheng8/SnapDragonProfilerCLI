using System;

namespace Sdp
{
	// Token: 0x020001FA RID: 506
	public class MenuItemController : IMenuItemController
	{
		// Token: 0x1700015E RID: 350
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x000131B0 File Offset: 0x000113B0
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

		// Token: 0x06000746 RID: 1862 RVA: 0x000131EF File Offset: 0x000113EF
		private void m_view_MenuItemActivated(object sender, EventArgs e)
		{
			if (this.m_command != null)
			{
				SdpApp.CommandManager.ExecuteCommand(this.m_command);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00013209 File Offset: 0x00011409
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x00013211 File Offset: 0x00011411
		public string Text { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001321A File Offset: 0x0001141A
		public ICommand Command
		{
			get
			{
				return this.m_command;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x00013222 File Offset: 0x00011422
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0001322A File Offset: 0x0001142A
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
				if (this.m_view != null)
				{
					this.m_view.Enabled = value;
				}
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00013247 File Offset: 0x00011447
		public MenuItemController(string label, ICommand command)
		{
			this.m_command = command;
			this.Text = label;
		}

		// Token: 0x04000720 RID: 1824
		protected IMenuItemView m_view;

		// Token: 0x04000722 RID: 1826
		private ICommand m_command;

		// Token: 0x04000723 RID: 1827
		private bool m_enabled = true;
	}
}
