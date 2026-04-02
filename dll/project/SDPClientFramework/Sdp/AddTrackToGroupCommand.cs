using System;

namespace Sdp
{
	// Token: 0x0200005C RID: 92
	public class AddTrackToGroupCommand : Command
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000763D File Offset: 0x0000583D
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00007645 File Offset: 0x00005845
		public TrackType TrackType { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000764E File Offset: 0x0000584E
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00007656 File Offset: 0x00005856
		public GroupController Container { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000765F File Offset: 0x0000585F
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00007667 File Offset: 0x00005867
		public TrackControllerBase Result { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00007670 File Offset: 0x00005870
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00007678 File Offset: 0x00005878
		public IMetricPlugin MetricPlugin { get; set; }

		// Token: 0x06000223 RID: 547 RVA: 0x00007681 File Offset: 0x00005881
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Result = this.Container.AddTrack(this.TrackType, this.MetricPlugin);
			}
		}
	}
}
