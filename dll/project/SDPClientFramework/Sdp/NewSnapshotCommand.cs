using System;

namespace Sdp
{
	// Token: 0x02000069 RID: 105
	internal class NewSnapshotCommand : Command
	{
		// Token: 0x06000259 RID: 601 RVA: 0x000080FA File Offset: 0x000062FA
		public NewSnapshotCommand()
		{
			this.UIName = "New Snapshot";
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00008110 File Offset: 0x00006310
		protected override void OnExecute()
		{
			if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null && !SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.AlreadyCaptured)
			{
				SdpApp.UIManager.FocusCaptureWindow(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.WindowName, "");
				return;
			}
			if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
			{
				SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.DetachEvents();
			}
			NewSnapshotWindowCommand newSnapshotWindowCommand = new NewSnapshotWindowCommand();
			newSnapshotWindowCommand.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Snapshot);
			int num = SdpApp.ModelManager.SnapshotModel.NextSnapshotNumber();
			newSnapshotWindowCommand.Name = "Snapshot " + num.ToString();
			SdpApp.CommandManager.ExecuteCommand(newSnapshotWindowCommand);
			if (newSnapshotWindowCommand.Result != null)
			{
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					newSnapshotWindowCommand.Result.SetSelectedProcess(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess);
					newSnapshotWindowCommand.Result.SetFilterEntry(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.FilterEntry);
				}
				SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController = newSnapshotWindowCommand.Result;
				newSnapshotWindowCommand.Result.SetName(num, newSnapshotWindowCommand.Name);
			}
		}
	}
}
