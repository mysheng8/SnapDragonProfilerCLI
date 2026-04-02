using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000015 RID: 21
	internal class StatisticsViewMgr
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00006740 File Offset: 0x00004940
		public StatisticsViewMgr()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.MetricAdded = (EventHandler<MetricAddedEventArgs>)Delegate.Combine(connectionEvents2.MetricAdded, new EventHandler<MetricAddedEventArgs>(this.connectionEvents_MetricAdded));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006864 File Offset: 0x00004A64
		private void connectionEvents_MetricAdded(object sender, MetricAddedEventArgs args)
		{
			string name = args.Metric.GetProperties().name;
			if (name != null)
			{
				int length = name.Length;
				if (length <= 5)
				{
					if (length != 4)
					{
						if (length != 5)
						{
							return;
						}
						if (!(name == "Power"))
						{
							return;
						}
						this.m_powerID = args.Metric.GetProperties().id;
						SdpApp.StatisticsManager.RegisterStatistic(this.m_clockTimeline);
						SdpApp.StatisticsManager.RegisterStatistic(this.m_clockDistribution);
					}
					else
					{
						char c = name[0];
						if (c != 'K')
						{
							if (c != 'M')
							{
								return;
							}
							if (!(name == "MDSS"))
							{
								return;
							}
							this.m_mdssMetID = args.Metric.GetProperties().id;
							SdpApp.StatisticsManager.RegisterStatistic(this.m_mdpCommitInfo);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_mdpTraceEvents);
							return;
						}
						else
						{
							if (!(name == "KGSL"))
							{
								return;
							}
							this.m_kgslMetID = args.Metric.GetProperties().id;
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuStateStat);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuFreqStat);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuPowerLevelStat);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuStateDist);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuBusDist);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuFreqDist);
							SdpApp.StatisticsManager.RegisterStatistic(this.m_gpuStateAndFreqDist);
							return;
						}
					}
				}
				else
				{
					switch (length)
					{
					case 10:
						if (!(name == "IRQ Events"))
						{
							return;
						}
						this.m_irqMetID = args.Metric.GetProperties().id;
						SdpApp.StatisticsManager.RegisterStatistic(this.m_irqDistStat);
						return;
					case 11:
					case 12:
						break;
					case 13:
						if (!(name == "CPU Frequency"))
						{
							return;
						}
						this.m_cpuFreqID = args.Metric.GetProperties().id;
						SdpApp.StatisticsManager.RegisterStatistic(this.m_cpuFreqStat);
						return;
					case 14:
						if (!(name == "CPU Scheduling"))
						{
							return;
						}
						this.m_cpuSchedMetID = args.Metric.GetProperties().id;
						SdpApp.StatisticsManager.RegisterStatistic(this.m_topThreadsStat);
						SdpApp.StatisticsManager.RegisterStatistic(this.m_runQueueDepth);
						return;
					default:
						if (length != 17)
						{
							return;
						}
						if (!(name == "Kernel Workqueues"))
						{
							return;
						}
						this.m_kworkerExecID = args.Metric.GetProperties().id;
						SdpApp.StatisticsManager.RegisterStatistic(this.m_kworkerFuncDist);
						SdpApp.StatisticsManager.RegisterStatistic(this.m_kworkerFuncContext);
						return;
					}
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006AFC File Offset: 0x00004CFC
		private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
		{
			uint captureID = args.CaptureID;
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_SYSTRACE_DATA)
			{
				HashSet<uint> activeMetricsForCapture = SdpApp.ModelManager.TraceModel.GetActiveMetricsForCapture(captureID);
				if (activeMetricsForCapture != null)
				{
					this.SetAvailable((int)captureID, activeMetricsForCapture);
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006B3C File Offset: 0x00004D3C
		private bool SetContainsMetric(HashSet<uint> activeMetrics, uint metricID)
		{
			if (activeMetrics.Contains(metricID) || SdpApp.ConnectionManager.GetConnectedDevice() == null)
			{
				return true;
			}
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricID);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("ImportSession");
			ModelObject modelObject = dataModel.GetModelObject(model, "ImportedMetrics");
			ModelObjectDataList modelObjectData = dataModel.GetModelObjectData(modelObject, "name", "\"" + metricByID.GetProperties().name + "\"");
			foreach (ModelObjectData modelObjectData2 in modelObjectData)
			{
				foreach (uint num in activeMetrics)
				{
					if (num.ToString() == modelObjectData2.GetValue("id"))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006C50 File Offset: 0x00004E50
		private void SetAvailable(int captureID, HashSet<uint> activeMetrics)
		{
			if (this.SetContainsMetric(activeMetrics, this.m_cpuSchedMetID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_topThreadsStat);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_runQueueDepth);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_irqMetID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_irqDistStat);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_cpuFreqID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_cpuFreqStat);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_kgslMetID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuStateStat);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuFreqStat);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuPowerLevelStat);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuStateDist);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuBusDist);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuFreqDist);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_gpuStateAndFreqDist);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_mdssMetID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_mdpCommitInfo);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_mdpTraceEvents);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_kworkerExecID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_kworkerFuncDist);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_kworkerFuncContext);
			}
			if (this.SetContainsMetric(activeMetrics, this.m_powerID) && this.SetContainsMetric(activeMetrics, this.m_cpuFreqID))
			{
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_clockTimeline);
				SdpApp.StatisticsManager.StatisticAvailable(captureID, this.m_clockDistribution);
			}
		}

		// Token: 0x04000034 RID: 52
		private ThreadTime m_topThreadsStat = new ThreadTime();

		// Token: 0x04000035 RID: 53
		private RunQueueDepth m_runQueueDepth = new RunQueueDepth();

		// Token: 0x04000036 RID: 54
		private IrqDistribution m_irqDistStat = new IrqDistribution();

		// Token: 0x04000037 RID: 55
		private KworkerFunctionDistribution m_kworkerFuncDist = new KworkerFunctionDistribution();

		// Token: 0x04000038 RID: 56
		private KWorkerFunctionContext m_kworkerFuncContext = new KWorkerFunctionContext();

		// Token: 0x04000039 RID: 57
		private CPUFrequency m_cpuFreqStat = new CPUFrequency();

		// Token: 0x0400003A RID: 58
		private GPUStateTimeline m_gpuStateStat = new GPUStateTimeline();

		// Token: 0x0400003B RID: 59
		private GPUFreqTimeline m_gpuFreqStat = new GPUFreqTimeline();

		// Token: 0x0400003C RID: 60
		private GPUBusTimeline m_gpuPowerLevelStat = new GPUBusTimeline();

		// Token: 0x0400003D RID: 61
		private GPUStateDistribution m_gpuStateDist = new GPUStateDistribution();

		// Token: 0x0400003E RID: 62
		private GPUBusDistribution m_gpuBusDist = new GPUBusDistribution();

		// Token: 0x0400003F RID: 63
		private GPUFreqDistribution m_gpuFreqDist = new GPUFreqDistribution();

		// Token: 0x04000040 RID: 64
		private GPUStateAndFreqDistribution m_gpuStateAndFreqDist = new GPUStateAndFreqDistribution();

		// Token: 0x04000041 RID: 65
		private MDPCommitInfo m_mdpCommitInfo = new MDPCommitInfo();

		// Token: 0x04000042 RID: 66
		private MDPTraceEvents m_mdpTraceEvents = new MDPTraceEvents();

		// Token: 0x04000043 RID: 67
		private ClockTimeline m_clockTimeline = new ClockTimeline();

		// Token: 0x04000044 RID: 68
		private ClockDistribution m_clockDistribution = new ClockDistribution();

		// Token: 0x04000045 RID: 69
		private uint m_cpuSchedMetID;

		// Token: 0x04000046 RID: 70
		private uint m_irqMetID;

		// Token: 0x04000047 RID: 71
		private uint m_cpuFreqID;

		// Token: 0x04000048 RID: 72
		private uint m_kgslMetID;

		// Token: 0x04000049 RID: 73
		private uint m_mdssMetID;

		// Token: 0x0400004A RID: 74
		private uint m_kworkerExecID;

		// Token: 0x0400004B RID: 75
		private uint m_powerID;

		// Token: 0x0400004C RID: 76
		private const uint TRACE_IMPORT_BUFFER_ID = 1U;
	}
}
