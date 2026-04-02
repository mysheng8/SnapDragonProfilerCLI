using System;

namespace Sdp
{
	// Token: 0x02000070 RID: 112
	internal class SampleCommand : Command
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00008657 File Offset: 0x00006857
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000865F File Offset: 0x0000685F
		public bool StartCapture { get; set; }

		// Token: 0x06000278 RID: 632 RVA: 0x00008668 File Offset: 0x00006868
		protected override void OnExecute()
		{
			if (SdpApp.ModelManager.SamplingModel.CurrentSources != null && SdpApp.ModelManager.SamplingModel.CurrentSources.Count > 0)
			{
				SamplingController currentSamplingController = SdpApp.ModelManager.SamplingModel.CurrentSamplingController;
				if (this.StartCapture)
				{
					if (currentSamplingController != null)
					{
						currentSamplingController.StartCapture();
						SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.StartSamplingRequest, this, EventArgs.Empty);
						return;
					}
				}
				else
				{
					currentSamplingController.StopCapture();
					SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.StopSamplingRequest, this, EventArgs.Empty);
				}
			}
		}
	}
}
