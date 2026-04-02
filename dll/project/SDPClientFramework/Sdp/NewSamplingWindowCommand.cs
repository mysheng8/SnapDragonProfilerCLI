using System;

namespace Sdp
{
	// Token: 0x02000068 RID: 104
	public class NewSamplingWindowCommand : Command
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00008033 File Offset: 0x00006233
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000803B File Offset: 0x0000623B
		public uint CaptureID { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00008044 File Offset: 0x00006244
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000804C File Offset: 0x0000624C
		public string Name { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00008055 File Offset: 0x00006255
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000805D File Offset: 0x0000625D
		public SamplingController Result { get; set; }

		// Token: 0x06000257 RID: 599 RVA: 0x00008068 File Offset: 0x00006268
		protected override void OnExecute()
		{
			string text = this.Name;
			int num = 1;
			while (SdpApp.UIManager.ContainsWindow(text))
			{
				text = this.Name + " (" + num.ToString() + ")";
				num++;
			}
			IViewController viewController = SdpApp.UIManager.CreateCaptureWindowTabbedWith(text, "Sampling", "Start Page", true, "CPU Sampling", true);
			SamplingController samplingController = viewController as SamplingController;
			if (samplingController != null)
			{
				samplingController.WindowName = text;
				samplingController.CaptureId = this.CaptureID;
			}
			SdpApp.AnalyticsManager.TrackWindow(CaptureType.Sampling);
			this.Result = samplingController;
		}
	}
}
