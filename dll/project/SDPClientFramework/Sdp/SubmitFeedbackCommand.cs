using System;

namespace Sdp
{
	// Token: 0x02000086 RID: 134
	internal class SubmitFeedbackCommand : Command
	{
		// Token: 0x060002FB RID: 763 RVA: 0x000093E9 File Offset: 0x000075E9
		public SubmitFeedbackCommand(string sessionPath = null, string suggestedTitle = null, string suggestedDescription = null)
		{
			this.UIName = "Submit Feedback";
			this.m_sessionPath = sessionPath;
			this.m_initialTitle = suggestedTitle;
			this.m_initialDescription = suggestedDescription;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00009414 File Offset: 0x00007614
		protected override async void OnExecute()
		{
			using (IDisposable dialog = SdpApp.UIManager.CreateDialog("SubmitFeedbackDialog") as IDisposable)
			{
				if (dialog != null)
				{
					SubmitFeedbackController submitFeedbackController = new SubmitFeedbackController((ISubmitFeedbackDialog)dialog, this.m_sessionPath, this.m_initialTitle, this.m_initialDescription);
					await submitFeedbackController.ShowDialog();
				}
			}
			IDisposable dialog = null;
		}

		// Token: 0x040001D3 RID: 467
		private string m_sessionPath;

		// Token: 0x040001D4 RID: 468
		private string m_initialTitle;

		// Token: 0x040001D5 RID: 469
		private string m_initialDescription;
	}
}
