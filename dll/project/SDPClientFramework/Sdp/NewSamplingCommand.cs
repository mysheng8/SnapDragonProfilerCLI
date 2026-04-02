using System;

namespace Sdp
{
	// Token: 0x02000067 RID: 103
	internal class NewSamplingCommand : Command
	{
		// Token: 0x0600024F RID: 591 RVA: 0x00007F2D File Offset: 0x0000612D
		public NewSamplingCommand()
		{
			this.UIName = "New Sampling";
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00007F40 File Offset: 0x00006140
		protected override void OnExecute()
		{
			SamplingController currentSamplingController = SdpApp.ModelManager.SamplingModel.CurrentSamplingController;
			if (currentSamplingController != null && !currentSamplingController.AlreadyCaptured)
			{
				SdpApp.UIManager.FocusCaptureWindow(currentSamplingController.WindowName, "");
				return;
			}
			NewSamplingWindowCommand newSamplingWindowCommand = new NewSamplingWindowCommand();
			newSamplingWindowCommand.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Sampling);
			newSamplingWindowCommand.Name = "Sampling " + SdpApp.ModelManager.SamplingModel.NextSamplingNumber().ToString();
			SdpApp.CommandManager.ExecuteCommand(newSamplingWindowCommand);
			if (newSamplingWindowCommand.Result != null)
			{
				if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
				{
					newSamplingWindowCommand.Result.SetSelectedProcess(SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess);
					newSamplingWindowCommand.Result.SetFilterEntry(SdpApp.ModelManager.SamplingModel.CurrentSamplingController.FilterEntry);
				}
				SdpApp.ModelManager.SamplingModel.CurrentSamplingController = newSamplingWindowCommand.Result;
			}
		}
	}
}
