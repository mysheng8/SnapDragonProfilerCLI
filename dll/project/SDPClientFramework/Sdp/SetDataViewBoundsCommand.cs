using System;

namespace Sdp
{
	// Token: 0x02000076 RID: 118
	public class SetDataViewBoundsCommand : Command
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00008826 File Offset: 0x00006A26
		public SetDataViewBoundsCommand()
		{
			this.CaptureId = 0U;
			this.Dirty = false;
			this.Minimum = -9.223372036854776E+18;
			this.Maximum = 9.223372036854776E+18;
			this.ForceScrollOff = false;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00008861 File Offset: 0x00006A61
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00008869 File Offset: 0x00006A69
		public double Minimum { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00008872 File Offset: 0x00006A72
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000887A File Offset: 0x00006A7A
		public double Maximum { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00008883 File Offset: 0x00006A83
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000888B File Offset: 0x00006A8B
		public uint CaptureId { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00008894 File Offset: 0x00006A94
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000889C File Offset: 0x00006A9C
		public bool Dirty { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000292 RID: 658 RVA: 0x000088A5 File Offset: 0x00006AA5
		// (set) Token: 0x06000293 RID: 659 RVA: 0x000088AD File Offset: 0x00006AAD
		public bool ForceScrollOff { get; set; }

		// Token: 0x06000294 RID: 660 RVA: 0x000088B8 File Offset: 0x00006AB8
		protected override void OnExecute()
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				if (this.ForceScrollOff)
				{
					SdpApp.CommandManager.ExecuteCommand(new SetScrollLockCommand((int)this.CaptureId, false));
				}
				timeModel.SetDataViewBounds(this.Minimum, this.Maximum, this.Dirty);
			}
		}
	}
}
