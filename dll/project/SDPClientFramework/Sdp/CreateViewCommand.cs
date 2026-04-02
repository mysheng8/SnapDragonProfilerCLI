using System;

namespace Sdp
{
	// Token: 0x02000062 RID: 98
	internal class CreateViewCommand : Command
	{
		// Token: 0x06000240 RID: 576 RVA: 0x00007D6B File Offset: 0x00005F6B
		public CreateViewCommand(string view_name)
		{
			this.m_viewName = view_name;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00007D7A File Offset: 0x00005F7A
		protected override void OnExecute()
		{
			SdpApp.UIManager.CreateView(this.m_viewName);
		}

		// Token: 0x04000184 RID: 388
		private string m_viewName;
	}
}
