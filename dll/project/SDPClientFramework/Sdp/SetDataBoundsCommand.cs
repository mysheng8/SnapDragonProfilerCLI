using System;

namespace Sdp
{
	// Token: 0x02000073 RID: 115
	public class SetDataBoundsCommand : Command
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000875C File Offset: 0x0000695C
		public SetDataBoundsCommand()
		{
			this.CaptureId = 0U;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000876B File Offset: 0x0000696B
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00008773 File Offset: 0x00006973
		public long Minimum { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000877C File Offset: 0x0000697C
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00008784 File Offset: 0x00006984
		public long Maximum { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000878D File Offset: 0x0000698D
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00008795 File Offset: 0x00006995
		public uint CaptureId { get; set; }

		// Token: 0x06000285 RID: 645 RVA: 0x000087A0 File Offset: 0x000069A0
		protected override void OnExecute()
		{
			if (this.CaptureId == 0U)
			{
				throw new Exception();
			}
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				timeModel.SetDataBounds(this.Minimum, this.Maximum, true);
			}
		}
	}
}
