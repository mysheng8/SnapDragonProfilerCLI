using System;

namespace Sdp
{
	// Token: 0x0200008A RID: 138
	internal class ToggleWindowVisibilityCommand : Command
	{
		// Token: 0x06000312 RID: 786 RVA: 0x00009593 File Offset: 0x00007793
		public ToggleWindowVisibilityCommand(IDockWindow dock_window)
		{
			this.m_dock_window = dock_window;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000095A2 File Offset: 0x000077A2
		protected override void OnExecute()
		{
			if (this.m_dock_window != null)
			{
				this.m_dock_window.IsVisible = !this.m_dock_window.IsVisible;
			}
		}

		// Token: 0x040001DE RID: 478
		private IDockWindow m_dock_window;
	}
}
