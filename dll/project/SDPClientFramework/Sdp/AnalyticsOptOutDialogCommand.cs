using System;

namespace Sdp
{
	// Token: 0x0200005D RID: 93
	internal class AnalyticsOptOutDialogCommand : Command
	{
		// Token: 0x06000225 RID: 549 RVA: 0x000076A8 File Offset: 0x000058A8
		protected override async void OnExecute()
		{
			using (IDisposable dialog = SdpApp.UIManager.CreateDialog("AnalyticsOptOutDialog") as IDisposable)
			{
				if (dialog != null)
				{
					AnalyticsOptOutController dialogController = new AnalyticsOptOutController((IAnalyticsOptOutDialog)dialog);
					await dialogController.ShowDialog();
					SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.UserAnalyticsOptOut, dialogController.OptOut.ToString());
					dialogController = null;
				}
			}
			IDisposable dialog = null;
		}
	}
}
