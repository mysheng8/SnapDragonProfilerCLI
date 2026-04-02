using System;

namespace Sdp
{
	// Token: 0x02000055 RID: 85
	internal class AboutCommand : Command
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x000071F1 File Offset: 0x000053F1
		public AboutCommand()
		{
			this.UIName = "About";
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007204 File Offset: 0x00005404
		protected override async void OnExecute()
		{
			using (IDisposable dialog = SdpApp.UIManager.CreateDialog("AboutDialog") as IDisposable)
			{
				if (dialog != null)
				{
					AboutDialogController aboutDialogController = new AboutDialogController((IAboutDialog)dialog);
					await aboutDialogController.ShowDialog();
				}
			}
			IDisposable dialog = null;
		}
	}
}
