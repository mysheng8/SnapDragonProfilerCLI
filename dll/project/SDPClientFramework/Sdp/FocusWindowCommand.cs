using System;

namespace Sdp
{
	// Token: 0x02000063 RID: 99
	internal class FocusWindowCommand : Command
	{
		// Token: 0x06000242 RID: 578 RVA: 0x00007D8C File Offset: 0x00005F8C
		public FocusWindowCommand(IDockWindow dockWindow)
		{
			this.m_dockWindow = dockWindow;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00007D9B File Offset: 0x00005F9B
		protected override void OnExecute()
		{
			if (this.m_dockWindow != null)
			{
				SdpApp.UIManager.FocusCaptureWindow(this.m_dockWindow.Name, "");
			}
		}

		// Token: 0x04000185 RID: 389
		private IDockWindow m_dockWindow;
	}
}
