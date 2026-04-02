using System;
using System.Collections.Generic;
using System.IO;
using Sdp.Charts.Graph;

namespace Sdp
{
	// Token: 0x020001AC RID: 428
	public class HistogramStatisticDisplayViewModel : IStatisticDisplayViewModel
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		public TreeModel Model { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0000C0C1 File Offset: 0x0000A2C1
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
		public List<string> ColumnLabels { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public StatisticDisplayType DisplayType
		{
			get
			{
				return StatisticDisplayType.Histogram;
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public void ExportToCSV(StreamWriter sw)
		{
			SdpApp.AnalyticsManager.TrackExport("Histogram Statistic Display");
			if (this.ColumnLabels != null)
			{
				sw.WriteLine("," + string.Join(",", this.ColumnLabels));
			}
			double?[,] array = new double?[this.ColumnLabels.Count, this.Series.Count];
			foreach (KeyValuePair<int, Series> keyValuePair in this.Series)
			{
				foreach (Point point in keyValuePair.Value.Points)
				{
					array[(int)point.X, keyValuePair.Key] = new double?(point.Y);
				}
			}
			for (int i = 0; i < this.Series.Count; i++)
			{
				sw.Write(this.Series[i].Name + ",");
				for (int j = 0; j < this.ColumnLabels.Count; j++)
				{
					double? num = array[j, i];
					sw.Write(num.ToString() + ",");
				}
				sw.Write(sw.NewLine);
			}
		}

		// Token: 0x04000650 RID: 1616
		public readonly Dictionary<int, Series> Series = new Dictionary<int, Series>();

		// Token: 0x04000651 RID: 1617
		public double MinX;

		// Token: 0x04000652 RID: 1618
		public double MaxX;
	}
}
