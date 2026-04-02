using System;

namespace Sdp
{
	// Token: 0x02000083 RID: 131
	public class SettingsCommand : Command
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x00009048 File Offset: 0x00007248
		protected override async void OnExecute()
		{
			using (IDisposable dialog = SdpApp.UIManager.CreateDialog("SettingsDialog") as IDisposable)
			{
				if (dialog != null)
				{
					SettingsDialogController settingsDialogController = new SettingsDialogController((ISettingsDialog)dialog);
					await settingsDialogController.ShowDialog();
				}
			}
			IDisposable dialog = null;
		}
	}
}
