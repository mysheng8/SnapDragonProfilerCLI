using System;
using System.Threading;

namespace Sdp
{
	// Token: 0x0200007B RID: 123
	internal class ExitAppCommand : Command
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x00008C23 File Offset: 0x00006E23
		public ExitAppCommand()
		{
			this.UIName = "E_xit";
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008C38 File Offset: 0x00006E38
		protected override async void OnExecute()
		{
			IDialog dialog = SdpApp.UIManager.CreateDialog("ExitingDialog");
			if (dialog != null)
			{
				await dialog.ShowDialog();
			}
			IPlatform platform = SdpApp.Platform;
			Thread thread = new Thread(delegate
			{
				SdpApp.Shutdown();
				platform.ExitApplication();
			});
			thread.Start();
		}
	}
}
