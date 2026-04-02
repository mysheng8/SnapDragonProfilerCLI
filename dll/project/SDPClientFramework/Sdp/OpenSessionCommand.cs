using System;

namespace Sdp
{
	// Token: 0x0200006C RID: 108
	public class OpenSessionCommand : Command
	{
		// Token: 0x0600026E RID: 622 RVA: 0x000084B0 File Offset: 0x000066B0
		protected override async void OnExecute()
		{
			IOpenSessionDialog openSessionDialog = SdpApp.UIManager.CreateDialog("OpenSessionDialog") as IOpenSessionDialog;
			if (openSessionDialog != null)
			{
				OpenSessionDialogController openSessionDialogController = new OpenSessionDialogController(openSessionDialog);
				await openSessionDialogController.ShowDialog();
			}
		}
	}
}
