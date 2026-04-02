using System;

namespace Sdp
{
	// Token: 0x02000065 RID: 101
	internal class LicensesCommand : Command
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00007DE5 File Offset: 0x00005FE5
		public LicensesCommand()
		{
			this.UIName = "Licenses";
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00007DF8 File Offset: 0x00005FF8
		protected override async void OnExecute()
		{
			using (IDisposable dialog = SdpApp.UIManager.CreateDialog("LicensesDialog") as IDisposable)
			{
				if (dialog != null)
				{
					LicensesDialogController licensesDialogController = new LicensesDialogController((ILicensesDialog)dialog);
					await licensesDialogController.ShowDialog();
				}
			}
			IDisposable dialog = null;
		}
	}
}
