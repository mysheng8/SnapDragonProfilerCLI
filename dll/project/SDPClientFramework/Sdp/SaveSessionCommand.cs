using System;

namespace Sdp
{
	// Token: 0x02000071 RID: 113
	public class SaveSessionCommand : Command
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00008704 File Offset: 0x00006904
		protected override async void OnExecute()
		{
			ISaveSessionDialog saveSessionDialog = SdpApp.UIManager.CreateDialog("SaveSessionDialog") as ISaveSessionDialog;
			if (saveSessionDialog != null)
			{
				SaveSessionDialogController saveSessionDialogController = new SaveSessionDialogController(saveSessionDialog);
				if (MainWindowController.SavingProgress == null)
				{
					MainWindowController.SavingProgress = new ProgressObject();
					MainWindowController.SavingProgress.Title = "Saving Session";
					MainWindowController.SavingProgress.Description = "Saving current Profiler session";
				}
				MainWindowController.SavingProgress.CurrentValue = 0.0;
				SavingSession savingSession = new SavingSession();
				savingSession.saving = true;
				SdpApp.EventsManager.Raise<SavingSession>(SdpApp.EventsManager.ClientEvents.SavingChanged, this, savingSession);
				await saveSessionDialogController.ShowDialog();
			}
		}
	}
}
