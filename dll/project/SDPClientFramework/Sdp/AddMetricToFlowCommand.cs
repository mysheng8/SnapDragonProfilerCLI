using System;

namespace Sdp
{
	// Token: 0x02000058 RID: 88
	public class AddMetricToFlowCommand : Command
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000735C File Offset: 0x0000555C
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00007364 File Offset: 0x00005564
		public uint PID { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000736D File Offset: 0x0000556D
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00007375 File Offset: 0x00005575
		public uint MetricId { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000737E File Offset: 0x0000557E
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00007386 File Offset: 0x00005586
		public GroupLayoutController Container { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000738F File Offset: 0x0000558F
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007397 File Offset: 0x00005597
		public TrackControllerBase Result { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000073A0 File Offset: 0x000055A0
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x000073A8 File Offset: 0x000055A8
		public IMetricPlugin MetricPlugin { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000073B1 File Offset: 0x000055B1
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x000073B9 File Offset: 0x000055B9
		public bool IsGlobal { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000073C2 File Offset: 0x000055C2
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x000073CA File Offset: 0x000055CA
		public TrackType TrackType { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000073D3 File Offset: 0x000055D3
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x000073DB File Offset: 0x000055DB
		public TrackControllerBase TrackController { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000073E4 File Offset: 0x000055E4
		// (set) Token: 0x060001FA RID: 506 RVA: 0x000073EC File Offset: 0x000055EC
		public bool IsPreview { get; set; }

		// Token: 0x060001FB RID: 507 RVA: 0x000073F8 File Offset: 0x000055F8
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Result = this.Container.AddMetric(this.MetricId, this.TrackType, this.PID, this.MetricPlugin, this.IsGlobal, this.TrackController, this.IsPreview);
			}
		}
	}
}
