using System;

namespace Sdp
{
	// Token: 0x0200007C RID: 124
	internal class NewCaptureCommand : Command
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x00008C67 File Offset: 0x00006E67
		public NewCaptureCommand()
		{
			this.UIName = "New Capture";
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008C7C File Offset: 0x00006E7C
		protected override void OnExecute()
		{
			GroupLayoutController currentCaptureGroupLayoutController = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController;
			if (currentCaptureGroupLayoutController != null && !currentCaptureGroupLayoutController.AlreadyCaptured)
			{
				SdpApp.UIManager.FocusCaptureWindow(currentCaptureGroupLayoutController.WindowName, "");
				return;
			}
			NewTraceWindowCommand newTraceWindowCommand = new NewTraceWindowCommand();
			newTraceWindowCommand.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Trace);
			int num = SdpApp.ModelManager.TraceModel.NextTraceCaptureNumber();
			newTraceWindowCommand.Name = "Trace " + num.ToString();
			SdpApp.CommandManager.ExecuteCommand(newTraceWindowCommand);
			if (newTraceWindowCommand.Result != null)
			{
				if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
				{
					newTraceWindowCommand.Result.SetSelectedProcess(SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess);
					newTraceWindowCommand.Result.SetFilterEntry(SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.FilterEntry);
				}
				SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController = newTraceWindowCommand.Result;
				GroupLayoutController result = newTraceWindowCommand.Result;
				result.SetName(CaptureType.Trace, num, newTraceWindowCommand.Name);
			}
		}
	}
}
