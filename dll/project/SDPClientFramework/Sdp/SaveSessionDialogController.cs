using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001EC RID: 492
	public class SaveSessionDialogController : IDialogController
	{
		// Token: 0x06000702 RID: 1794 RVA: 0x00012484 File Offset: 0x00010684
		public async Task<bool> ShowDialog()
		{
			bool flag = await this.m_view.ShowDialog();
			bool flag2;
			if (flag)
			{
				flag2 = await Task.Factory.StartNew<bool>(() => this.m_view.SelectedFileLocation.Match<bool>(new Func<string, bool>(SaveSessionDialogController.SaveSession), () => false));
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000124C7 File Offset: 0x000106C7
		private static void ResetSavingStatus()
		{
			SdpApp.Platform.Invoke(delegate
			{
				SavingSession savingSession = new SavingSession();
				savingSession.saving = false;
				SdpApp.EventsManager.Raise<SavingSession>(SdpApp.EventsManager.ClientEvents.SavingChanged, null, savingSession);
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
			});
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000124F4 File Offset: 0x000106F4
		private static bool SaveSession(string fileName)
		{
			if (File.Exists(fileName))
			{
				try
				{
					File.Delete(fileName);
				}
				catch
				{
					ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
					showMessageDialogCommand.Message = "There was an error overwriting the file specified.";
					showMessageDialogCommand.IconType = IconType.Error;
					showMessageDialogCommand.OnCompleted = delegate(bool result)
					{
						SaveSessionDialogController.ResetSavingStatus();
					};
					showMessageDialogCommand.Execute();
					return true;
				}
			}
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
			string sessionPath = SessionManager.Get().GetSessionPath();
			MainWindowController.SavingProgress.CurrentValue = 0.25;
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
			CompressFilesCommand compressFilesCommand = new CompressFilesCommand();
			compressFilesCommand.Output = fileName;
			compressFilesCommand.Files = new List<string>();
			compressFilesCommand.Files.Add(sessionPath + "sdp.db");
			compressFilesCommand.Files.Add(sessionPath + "version.txt");
			if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null || SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				foreach (KeyValuePair<uint, string> keyValuePair in SdpApp.ModelManager.SnapshotModel.DataFilenames)
				{
					if (File.Exists(keyValuePair.Value))
					{
						compressFilesCommand.Files.Add(keyValuePair.Value);
					}
				}
				foreach (KeyValuePair<uint, string> keyValuePair2 in SdpApp.ModelManager.SnapshotModel.StrippedDataFilenames)
				{
					if (File.Exists(keyValuePair2.Value))
					{
						compressFilesCommand.Files.Add(keyValuePair2.Value);
					}
				}
			}
			MainWindowController.SavingProgress.CurrentValue = 0.5;
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
			try
			{
				if (SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController != null && !SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.IsPaused)
				{
					SdpApp.EventsManager.Raise<PauseCaptureEventArgs>(SdpApp.EventsManager.ConnectionEvents.PauseCapture, null, new PauseCaptureEventArgs
					{
						CaptureId = SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.CaptureId,
						Pause = true
					});
					MainWindowController.SavingProgress.CurrentValue = 0.75;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
					Thread.Sleep(500);
				}
				SdpApp.CommandManager.ExecuteCommand(compressFilesCommand);
				if (SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController != null && !SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.IsPaused)
				{
					SdpApp.EventsManager.Raise<PauseCaptureEventArgs>(SdpApp.EventsManager.ConnectionEvents.PauseCapture, null, new PauseCaptureEventArgs
					{
						CaptureId = SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.CaptureId,
						Pause = false
					});
				}
			}
			catch (Exception ex)
			{
				ShowMessageDialogCommand showMessageDialogCommand2 = new ShowMessageDialogCommand();
				showMessageDialogCommand2.Message = "There was an error saving the current session to the file specified:\n" + ex.Message;
				showMessageDialogCommand2.IconType = IconType.Error;
				showMessageDialogCommand2.OnCompleted = delegate(bool result)
				{
					SaveSessionDialogController.ResetSavingStatus();
				};
				showMessageDialogCommand2.Execute();
				return true;
			}
			MainWindowController.SavingProgress.CurrentValue = 1.0;
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, null, new ProgressEventArgs(MainWindowController.SavingProgress));
			ShowMessageDialogCommand showMessageDialogCommand3 = new ShowMessageDialogCommand();
			showMessageDialogCommand3.OnCompleted = delegate(bool result)
			{
				SaveSessionDialogController.ResetSavingStatus();
			};
			showMessageDialogCommand3.Message = "Session saved successfully";
			showMessageDialogCommand3.Execute();
			return true;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00012970 File Offset: 0x00010B70
		public SaveSessionDialogController(ISaveSessionDialog view)
		{
			this.m_view = view;
		}

		// Token: 0x04000713 RID: 1811
		private ISaveSessionDialog m_view;
	}
}
