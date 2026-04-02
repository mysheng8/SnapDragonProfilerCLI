using System;

namespace Sdp.Commands
{
	// Token: 0x0200031B RID: 795
	internal class MoveMetricFromGraphTrackCommand : Command
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x00034045 File Offset: 0x00032245
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x0003404D File Offset: 0x0003224D
		public GraphTrackController Track { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x00034056 File Offset: 0x00032256
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x0003405E File Offset: 0x0003225E
		public uint MetricId { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00034067 File Offset: 0x00032267
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0003406F File Offset: 0x0003226F
		public string MetricName { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x00034078 File Offset: 0x00032278
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x00034080 File Offset: 0x00032280
		public uint PID { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x00034089 File Offset: 0x00032289
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x00034091 File Offset: 0x00032291
		public Action<GraphTrackMetric> OnCompleted { private get; set; } = delegate(GraphTrackMetric result)
		{
		};

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0003409A File Offset: 0x0003229A
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x000340A2 File Offset: 0x000322A2
		public MetricType MetricType { get; set; }

		// Token: 0x0600108F RID: 4239 RVA: 0x000340AC File Offset: 0x000322AC
		protected override async void OnExecute()
		{
			if (this.Track != null)
			{
				GraphTrackMetric graphTrackMetric = await this.Track.RemoveMetricAndReturnSeries(this.MetricId, this.PID, this.MetricName, this.MetricType);
				GraphTrackMetric graphTrackMetric2 = graphTrackMetric;
				this.OnCompleted(graphTrackMetric2);
			}
		}
	}
}
