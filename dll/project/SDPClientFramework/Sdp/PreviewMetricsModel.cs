using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200019F RID: 415
	public class PreviewMetricsModel
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x0000B815 File Offset: 0x00009A15
		public PreviewMetricsModel()
		{
			this.m_metrics = new List<PreviewMetricsModel.MetricPair>();
			this.CurrentContainer = PreviewMetricsModel.ContainerType.NONE;
			this.Track = null;
			this.Group = null;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0000B848 File Offset: 0x00009A48
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0000B850 File Offset: 0x00009A50
		public PreviewMetricsModel.ContainerType CurrentContainer { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0000B859 File Offset: 0x00009A59
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0000B861 File Offset: 0x00009A61
		public PreviewMetricsModel.DragType CurrentDragType { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0000B86A File Offset: 0x00009A6A
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0000B872 File Offset: 0x00009A72
		public TrackControllerBase Track { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0000B87B File Offset: 0x00009A7B
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0000B883 File Offset: 0x00009A83
		public GroupController Group { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0000B88C File Offset: 0x00009A8C
		public List<PreviewMetricsModel.MetricPair> Metrics
		{
			get
			{
				return this.m_metrics;
			}
		}

		// Token: 0x0400062D RID: 1581
		public object PreviewMetricsModelLock = new object();

		// Token: 0x04000632 RID: 1586
		private List<PreviewMetricsModel.MetricPair> m_metrics;

		// Token: 0x02000372 RID: 882
		public struct MetricPair
		{
			// Token: 0x060011AB RID: 4523 RVA: 0x000368EE File Offset: 0x00034AEE
			public MetricPair(uint metricID, uint processID)
			{
				this.MetricID = metricID;
				this.ProcessID = processID;
			}

			// Token: 0x04000C12 RID: 3090
			public uint MetricID;

			// Token: 0x04000C13 RID: 3091
			public uint ProcessID;
		}

		// Token: 0x02000373 RID: 883
		public enum ContainerType
		{
			// Token: 0x04000C15 RID: 3093
			GROUP_LAYOUT,
			// Token: 0x04000C16 RID: 3094
			GROUP,
			// Token: 0x04000C17 RID: 3095
			TRACK,
			// Token: 0x04000C18 RID: 3096
			NONE
		}

		// Token: 0x02000374 RID: 884
		public enum DragType
		{
			// Token: 0x04000C1A RID: 3098
			DATA_SOURCE_METRIC,
			// Token: 0x04000C1B RID: 3099
			DATA_SOURCE_CATEGORY,
			// Token: 0x04000C1C RID: 3100
			GRAPH_TRACK_METRIC_SINGLE,
			// Token: 0x04000C1D RID: 3101
			GRAPH_TRACK_METRIC_MULTI
		}
	}
}
