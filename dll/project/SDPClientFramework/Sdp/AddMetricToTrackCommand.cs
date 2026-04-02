using System;
using Cairo;

namespace Sdp
{
	// Token: 0x02000059 RID: 89
	public class AddMetricToTrackCommand : Command
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007448 File Offset: 0x00005648
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00007450 File Offset: 0x00005650
		public uint PID { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007459 File Offset: 0x00005659
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00007461 File Offset: 0x00005661
		public uint MetricId { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000746A File Offset: 0x0000566A
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007472 File Offset: 0x00005672
		public string MetricName { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000747B File Offset: 0x0000567B
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00007483 File Offset: 0x00005683
		public TrackControllerBase Container { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000748C File Offset: 0x0000568C
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00007494 File Offset: 0x00005694
		public bool IsPreview { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000749D File Offset: 0x0000569D
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000074A5 File Offset: 0x000056A5
		public string MetricTooltip { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000209 RID: 521 RVA: 0x000074AE File Offset: 0x000056AE
		// (set) Token: 0x0600020A RID: 522 RVA: 0x000074B6 File Offset: 0x000056B6
		public bool IsCustom { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600020B RID: 523 RVA: 0x000074BF File Offset: 0x000056BF
		// (set) Token: 0x0600020C RID: 524 RVA: 0x000074C7 File Offset: 0x000056C7
		public Color? Color { get; set; }

		// Token: 0x0600020D RID: 525 RVA: 0x000074D0 File Offset: 0x000056D0
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Container.AddMetric(this.MetricId, this.MetricName, this.PID, this.IsPreview, this.MetricTooltip, this.IsCustom, this.Color);
			}
		}
	}
}
