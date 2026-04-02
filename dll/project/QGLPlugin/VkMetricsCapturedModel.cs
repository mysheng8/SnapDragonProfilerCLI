using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x0200002B RID: 43
	public class VkMetricsCapturedModel
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00006AAF File Offset: 0x00004CAF
		public IList<uint> Metrics
		{
			get
			{
				return Enumerable.ToList<uint>(this.m_capturedMetrics.Keys);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public VkMetricsCapturedModel(uint captureID)
		{
			if (QGLPlugin.SnapshotMetricsBuffer == null || QGLPlugin.SnapshotMetricsBuffer.size == 0U)
			{
				VkMetricsCapturedModel.Logger.LogWarning("No metric data found for capture " + captureID.ToString());
				return;
			}
			int num = 10;
			int num2 = Marshal.SizeOf<QGLPlugin.VulkanSnapshotMetric>(default(QGLPlugin.VulkanSnapshotMetric));
			long num3 = (long)((ulong)QGLPlugin.SnapshotMetricsBuffer.size / (ulong)((long)num2));
			IntPtr intPtr = QGLPlugin.SnapshotMetricsBuffer.data;
			int num4 = 0;
			while ((long)num4 < num3)
			{
				QGLPlugin.VulkanSnapshotMetric vulkanSnapshotMetric = Marshal.PtrToStructure<QGLPlugin.VulkanSnapshotMetric>(intPtr);
				intPtr += num2;
				if (!this.m_capturedMetrics.ContainsKey(vulkanSnapshotMetric.metricID))
				{
					this.m_capturedMetrics.Add(vulkanSnapshotMetric.metricID, num);
					num += 2;
				}
				ValueTuple<ulong, ulong, uint> valueTuple = new ValueTuple<ulong, ulong, uint>(vulkanSnapshotMetric.replayHandleID, vulkanSnapshotMetric.drawID, vulkanSnapshotMetric.cmdBuffSubmitCount);
				Dictionary<uint, double> dictionary;
				if (!this.m_metricValues.TryGetValue(valueTuple, out dictionary))
				{
					dictionary = (this.m_metricValues[valueTuple] = new Dictionary<uint, double>());
					if (this.m_fetchDrawsInOrder || vulkanSnapshotMetric.replayHandleID == 18446744073709551615UL)
					{
						ValueTuple<ulong, uint> valueTuple2 = new ValueTuple<ulong, uint>(vulkanSnapshotMetric.drawID, vulkanSnapshotMetric.cmdBuffSubmitCount);
						this.m_metricsByDrawId[valueTuple2] = dictionary;
						this.m_fetchDrawsInOrder = true;
					}
				}
				if (!dictionary.ContainsKey(vulkanSnapshotMetric.metricID))
				{
					dictionary[vulkanSnapshotMetric.metricID] = Math.Round(vulkanSnapshotMetric.value, 4);
					if (this.m_fetchDrawsInOrder)
					{
						ValueTuple<ulong, uint> valueTuple3 = new ValueTuple<ulong, uint>(vulkanSnapshotMetric.drawID, vulkanSnapshotMetric.cmdBuffSubmitCount);
						this.m_metricsByDrawId[valueTuple3] = dictionary;
					}
				}
				else
				{
					VkMetricsCapturedModel.Logger.LogError(string.Concat(new string[]
					{
						"Key collision in metric values. CommandBufferID:[",
						vulkanSnapshotMetric.replayHandleID.ToString(),
						"]DrawID:[",
						vulkanSnapshotMetric.drawID.ToString(),
						"] MetricID:[",
						vulkanSnapshotMetric.metricID.ToString(),
						"] SubmitCount:[",
						vulkanSnapshotMetric.cmdBuffSubmitCount.ToString(),
						"]"
					}));
				}
				num4++;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00006D24 File Offset: 0x00004F24
		public bool HasDrawID(ulong commandBufferID, uint drawID, uint submitCount)
		{
			ValueTuple<ulong, uint, uint> valueTuple = new ValueTuple<ulong, uint, uint>(commandBufferID, drawID, submitCount);
			Dictionary<ValueTuple<ulong, ulong, uint>, Dictionary<uint, double>> metricValues = this.m_metricValues;
			ValueTuple<ulong, uint, uint> valueTuple2 = valueTuple;
			if (metricValues.ContainsKey(new ValueTuple<ulong, ulong, uint>(valueTuple2.Item1, (ulong)valueTuple2.Item2, valueTuple2.Item3)))
			{
				return true;
			}
			if (this.m_fetchDrawsInOrder)
			{
				ValueTuple<uint, uint> valueTuple3 = new ValueTuple<uint, uint>(drawID, submitCount);
				Dictionary<ValueTuple<ulong, uint>, Dictionary<uint, double>> metricsByDrawId = this.m_metricsByDrawId;
				ValueTuple<uint, uint> valueTuple4 = valueTuple3;
				return metricsByDrawId.ContainsKey(new ValueTuple<ulong, uint>((ulong)valueTuple4.Item1, valueTuple4.Item2));
			}
			return false;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00006D98 File Offset: 0x00004F98
		public double GetMetricValue(ulong commandBufferID, uint drawID, uint submitCount, uint metricID)
		{
			ValueTuple<ulong, uint, uint> valueTuple = new ValueTuple<ulong, uint, uint>(commandBufferID, drawID, submitCount);
			Dictionary<ValueTuple<ulong, ulong, uint>, Dictionary<uint, double>> metricValues = this.m_metricValues;
			ValueTuple<ulong, uint, uint> valueTuple2 = valueTuple;
			Dictionary<uint, double> dictionary;
			if (metricValues.TryGetValue(new ValueTuple<ulong, ulong, uint>(valueTuple2.Item1, (ulong)valueTuple2.Item2, valueTuple2.Item3), out dictionary))
			{
				double num;
				if (dictionary.TryGetValue(metricID, out num))
				{
					return num;
				}
			}
			else if (this.m_fetchDrawsInOrder)
			{
				ValueTuple<uint, uint> valueTuple3 = new ValueTuple<uint, uint>(drawID, submitCount);
				Dictionary<ValueTuple<ulong, uint>, Dictionary<uint, double>> metricsByDrawId = this.m_metricsByDrawId;
				ValueTuple<uint, uint> valueTuple4 = valueTuple3;
				double num2;
				if (metricsByDrawId.TryGetValue(new ValueTuple<ulong, uint>((ulong)valueTuple4.Item1, valueTuple4.Item2), out dictionary) && dictionary.TryGetValue(metricID, out num2))
				{
					return num2;
				}
			}
			VkMetricsCapturedModel.Logger.LogError(string.Concat(new string[]
			{
				"Metric value doesn't exist for combination of CommandBufferID:[",
				commandBufferID.ToString(),
				"] DrawID:[",
				drawID.ToString(),
				"]SubmitCount: [",
				submitCount.ToString(),
				"] + MetricID:[",
				metricID.ToString(),
				"]"
			}));
			return -1.0;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00006E9C File Offset: 0x0000509C
		public bool HasMetric(uint metricID)
		{
			return this.m_capturedMetrics.ContainsKey(metricID);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00006EAC File Offset: 0x000050AC
		public int GetMetricColumn(uint metricID)
		{
			int num;
			if (this.m_capturedMetrics.TryGetValue(metricID, out num))
			{
				return num;
			}
			return 10;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006ED0 File Offset: 0x000050D0
		public string GetMetricName(uint metricID)
		{
			string text;
			if (!this.m_metricNames.TryGetValue(metricID, out text))
			{
				Metric metric = MetricManager.Get().GetMetric(metricID);
				if (metric.IsValid())
				{
					text = metric.GetProperties().name;
				}
				else
				{
					text = VkHelper.GetImportedMetricName(metricID);
					if (string.IsNullOrEmpty(text))
					{
						text = "Unknown Metric " + this.GetMetricColumn(metricID).ToString();
					}
				}
				this.m_metricNames[metricID] = text;
			}
			return text;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00006F46 File Offset: 0x00005146
		public Dictionary<string, Dictionary<uint, double>> MetricsList
		{
			get
			{
				return this.m_metricsList;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00006F4E File Offset: 0x0000514E
		public Dictionary<string, Dictionary<int, Dictionary<long, double>>> AccumulatedMetrics
		{
			get
			{
				return this.m_accumulatedMetrics;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00006F56 File Offset: 0x00005156
		public int TotalDrawcallCount
		{
			get
			{
				return this.m_metricValues.Keys.Count;
			}
		}

		// Token: 0x0400035E RID: 862
		private Dictionary<uint, int> m_capturedMetrics = new Dictionary<uint, int>();

		// Token: 0x0400035F RID: 863
		private readonly Dictionary<uint, string> m_metricNames = new Dictionary<uint, string>();

		// Token: 0x04000360 RID: 864
		private Dictionary<ValueTuple<ulong, ulong, uint>, Dictionary<uint, double>> m_metricValues = new Dictionary<ValueTuple<ulong, ulong, uint>, Dictionary<uint, double>>();

		// Token: 0x04000361 RID: 865
		private Dictionary<string, Dictionary<uint, double>> m_metricsList = new Dictionary<string, Dictionary<uint, double>>();

		// Token: 0x04000362 RID: 866
		private Dictionary<string, Dictionary<int, Dictionary<long, double>>> m_accumulatedMetrics = new Dictionary<string, Dictionary<int, Dictionary<long, double>>>();

		// Token: 0x04000363 RID: 867
		private Dictionary<ValueTuple<ulong, uint>, Dictionary<uint, double>> m_metricsByDrawId = new Dictionary<ValueTuple<ulong, uint>, Dictionary<uint, double>>();

		// Token: 0x04000364 RID: 868
		private bool m_fetchDrawsInOrder;

		// Token: 0x04000365 RID: 869
		private static ILogger Logger = new Sdp.Logging.Logger("VkMetricsCapturedModel");
	}
}
