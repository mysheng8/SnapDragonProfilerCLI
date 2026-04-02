using System;
using Cairo;

namespace Sdp
{
	// Token: 0x0200021E RID: 542
	public struct MetricDesc
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000160CC File Offset: 0x000142CC
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x000160D4 File Offset: 0x000142D4
		public string MetricName { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x000160DD File Offset: 0x000142DD
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x000160E5 File Offset: 0x000142E5
		public string ProcessName { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x000160EE File Offset: 0x000142EE
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x000160F6 File Offset: 0x000142F6
		public uint CategoryId { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x000160FF File Offset: 0x000142FF
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x00016107 File Offset: 0x00014307
		public uint ProviderId { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00016110 File Offset: 0x00014310
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x00016118 File Offset: 0x00014318
		public uint ProcessId { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00016121 File Offset: 0x00014321
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x00016129 File Offset: 0x00014329
		public uint MetricId { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00016132 File Offset: 0x00014332
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0001613A File Offset: 0x0001433A
		public MetricType MetricType { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00016143 File Offset: 0x00014343
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0001614B File Offset: 0x0001434B
		public bool IsPreview { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00016154 File Offset: 0x00014354
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0001615C File Offset: 0x0001435C
		public string MetricTooltip { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00016165 File Offset: 0x00014365
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0001616D File Offset: 0x0001436D
		public Color? ColorOverride { get; set; }

		// Token: 0x06000839 RID: 2105 RVA: 0x00016176 File Offset: 0x00014376
		public override bool Equals(object obj)
		{
			return obj is MetricDesc && this == (MetricDesc)obj;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00016194 File Offset: 0x00014394
		public override int GetHashCode()
		{
			string text = ((this.MetricName != null) ? this.MetricName : "");
			string text2 = ((this.ProcessName != null) ? this.ProcessName : "");
			return text.GetHashCode() ^ text2.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.ProviderId.GetHashCode() ^ this.MetricId.GetHashCode() ^ this.MetricType.GetHashCode();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001621C File Offset: 0x0001441C
		public static bool operator ==(MetricDesc a, MetricDesc b)
		{
			return a.MetricName == b.MetricName && a.ProcessName == b.ProcessName && a.CategoryId == b.CategoryId && a.ProviderId == b.ProviderId && a.MetricId == b.MetricId && a.MetricType == b.MetricType && a.IsPreview == b.IsPreview;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000162A5 File Offset: 0x000144A5
		public static bool operator !=(MetricDesc a, MetricDesc b)
		{
			return !(a == b);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000162B4 File Offset: 0x000144B4
		public static MetricDesc CreateMetricDesc(uint metricId, uint processId)
		{
			return MetricDesc.CreateMetricDesc(metricId, processId, false);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000162CC File Offset: 0x000144CC
		public static MetricDesc CreateMetricDesc(uint metricId, uint processId, bool isPreview)
		{
			MetricDesc metricDesc = default(MetricDesc);
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricId);
			Process processByID = SdpApp.ConnectionManager.GetProcessByID(processId);
			if (metricByID != null && metricByID.IsValid())
			{
				string text;
				if (processByID != null && processByID.IsValid())
				{
					text = processByID.GetProperties().name;
				}
				else
				{
					text = "";
				}
				metricDesc.MetricName = metricByID.GetProperties().name;
				metricDesc.ProcessName = text;
				metricDesc.CategoryId = metricByID.GetProperties().categoryID;
				metricDesc.ProcessId = processId;
				metricDesc.MetricId = metricByID.GetProperties().id;
				metricDesc.MetricType = MetricType.Live;
				metricDesc.IsPreview = isPreview;
			}
			else
			{
				metricDesc.MetricId = metricId;
				metricDesc.ProcessId = processId;
			}
			return metricDesc;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001638C File Offset: 0x0001458C
		public static MetricDesc CreateTransientMetricDesc(string metric_name, string tooltip = null, uint processId = 0U)
		{
			return new MetricDesc
			{
				MetricName = metric_name,
				MetricTooltip = tooltip,
				MetricType = MetricType.Transient,
				ProcessId = processId
			};
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000163C4 File Offset: 0x000145C4
		public static MetricDesc CreateCustomMetricDesc(string metric_name, string tooltip, uint customId)
		{
			return new MetricDesc
			{
				MetricName = metric_name,
				MetricTooltip = tooltip,
				MetricType = MetricType.Custom,
				MetricId = customId
			};
		}
	}
}
