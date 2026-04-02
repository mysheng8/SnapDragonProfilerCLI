using System;
using System.Collections.Generic;
using Sdp;
using Sdp.Charts.Graph;
using Sdp.Helpers;

namespace TracePlugin
{
	// Token: 0x02000002 RID: 2
	public class CPUFrequency : IStatistic
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public string Category
		{
			get
			{
				return "CPU";
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000204F File Offset: 0x0000024F
		public string Description
		{
			get
			{
				return "CPU Frequency analysis statistical data. Total time spent on each core at each frequency. Requires 'CPU Frequency' Trace metric to be enabled.";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002056 File Offset: 0x00000256
		public string Name
		{
			get
			{
				return "CPU Frequency";
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			CPUFrequency.DataModel dataModel;
			if (!this.m_dataModels.TryGetValue(captureID, out dataModel))
			{
				dataModel = this.BuildDataModel(captureID);
				this.m_dataModels[captureID] = dataModel;
			}
			int num = dataModel.CpuFrequencyPerCore.Count + 2;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "CPU ID #";
			array[num - 1] = typeof(Duration);
			array2[num - 1] = "Total";
			HistogramStatisticDisplayViewModel histogramStatisticDisplayViewModel = new HistogramStatisticDisplayViewModel();
			histogramStatisticDisplayViewModel.MinX = 0.0;
			histogramStatisticDisplayViewModel.MaxX = (double)(dataModel.CpuFrequencyPerCore.Count - 1);
			histogramStatisticDisplayViewModel.ColumnLabels = new List<string>();
			histogramStatisticDisplayViewModel.Model = new TreeModel(array);
			histogramStatisticDisplayViewModel.Model.ColumnNames = array2;
			Dictionary<uint, TreeNode> dictionary = new Dictionary<uint, TreeNode>();
			foreach (KeyValuePair<uint, long> keyValuePair in dataModel.TotalTime)
			{
				TreeNode treeNode = (dictionary[keyValuePair.Key] = new TreeNode());
				treeNode.Values = new object[num];
				treeNode.Values[0] = "CPU " + keyValuePair.Key.ToString();
				treeNode.Values[num - 1] = new Duration(keyValuePair.Value);
				histogramStatisticDisplayViewModel.Model.Nodes.Add(treeNode);
			}
			int num2 = 0;
			foreach (KeyValuePair<uint, SortedDictionary<uint, long>> keyValuePair2 in dataModel.CpuFrequencyPerCore)
			{
				histogramStatisticDisplayViewModel.ColumnLabels.Add(keyValuePair2.Key.ToString());
				array[num2 + 1] = typeof(Duration);
				array2[num2 + 1] = keyValuePair2.Key.ToString();
				foreach (KeyValuePair<uint, long> keyValuePair3 in keyValuePair2.Value)
				{
					Series series = null;
					if (!histogramStatisticDisplayViewModel.Series.TryGetValue((int)keyValuePair3.Key, out series))
					{
						series = (histogramStatisticDisplayViewModel.Series[(int)keyValuePair3.Key] = new Series());
						series.SeriesID = (int)keyValuePair3.Key;
						series.Name = "CPU " + keyValuePair3.Key.ToString();
						if ((ulong)keyValuePair3.Key < (ulong)((long)FormatHelper.DefaultColors.Count))
						{
							series.Color = FormatHelper.DefaultColors[(int)keyValuePair3.Key];
						}
						else
						{
							series.Color = FormatHelper.PseudoRandomColor();
						}
					}
					series.Points.Add(new Point((double)num2, (double)keyValuePair3.Value));
					dictionary[keyValuePair3.Key].Values[num2 + 1] = new Duration(keyValuePair3.Value);
				}
				num2++;
			}
			return new IStatisticDisplayViewModel[] { histogramStatisticDisplayViewModel };
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023D4 File Offset: 0x000005D4
		private CPUFrequency.DataModel BuildDataModel(int captureID)
		{
			CPUFrequency.DataModel dataModel = new CPUFrequency.DataModel();
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("CpuFrequency", captureID);
			Dictionary<uint, CPUFrequency.DataModel.CpuFrequencyEvent> dictionary = new Dictionary<uint, CPUFrequency.DataModel.CpuFrequencyEvent>();
			Dictionary<uint, long> dictionary2 = new Dictionary<uint, long>();
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				CPUFrequency.DataModel.CpuFrequencyEvent cpuFrequencyEvent = new CPUFrequency.DataModel.CpuFrequencyEvent();
				string text = null;
				if (ftraceEventData.EventParams.TryGetValue("state", out text) && uint.TryParse(text, out cpuFrequencyEvent.Frequency))
				{
					CPUFrequency.DataModel.CpuFrequencyEvent cpuFrequencyEvent2 = null;
					if (ftraceEventData.EventParams.TryGetValue("cpuId", out text) && uint.TryParse(text, out cpuFrequencyEvent.Cpu))
					{
						if (dictionary.TryGetValue(cpuFrequencyEvent.Cpu, out cpuFrequencyEvent2))
						{
							cpuFrequencyEvent2.Time = ftraceEventData.TimeStampBegin - cpuFrequencyEvent2.Time;
							this.AddIntervalToModel(ref dataModel, cpuFrequencyEvent2);
							cpuFrequencyEvent.Time = ftraceEventData.TimeStampBegin;
						}
						else
						{
							cpuFrequencyEvent.Time = ftraceEventData.TimeStampBegin;
							dictionary2[cpuFrequencyEvent.Cpu] = cpuFrequencyEvent.Time;
						}
						dictionary[cpuFrequencyEvent.Cpu] = cpuFrequencyEvent;
					}
				}
			}
			long lastTimeStamp = FTraceEventDataRetriever.GetLastTimeStamp(captureID);
			foreach (KeyValuePair<uint, CPUFrequency.DataModel.CpuFrequencyEvent> keyValuePair in dictionary)
			{
				CPUFrequency.DataModel.CpuFrequencyEvent cpuFrequencyEvent3 = new CPUFrequency.DataModel.CpuFrequencyEvent();
				cpuFrequencyEvent3.Cpu = keyValuePair.Key;
				cpuFrequencyEvent3.Time = lastTimeStamp - keyValuePair.Value.Time;
				cpuFrequencyEvent3.Frequency = keyValuePair.Value.Frequency;
				this.AddIntervalToModel(ref dataModel, cpuFrequencyEvent3);
				dataModel.TotalTime[cpuFrequencyEvent3.Cpu] = lastTimeStamp - dictionary2[cpuFrequencyEvent3.Cpu];
			}
			return dataModel;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000025C8 File Offset: 0x000007C8
		private void AddIntervalToModel(ref CPUFrequency.DataModel dataModel, CPUFrequency.DataModel.CpuFrequencyEvent e)
		{
			SortedDictionary<uint, long> sortedDictionary = null;
			if (!dataModel.CpuFrequencyPerCore.TryGetValue(e.Frequency, ref sortedDictionary))
			{
				sortedDictionary = (dataModel.CpuFrequencyPerCore[e.Frequency] = new SortedDictionary<uint, long>());
			}
			long num = 0L;
			if (sortedDictionary.TryGetValue(e.Cpu, ref num))
			{
				sortedDictionary[e.Cpu] = num + e.Time;
				return;
			}
			sortedDictionary[e.Cpu] = e.Time;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002642 File Offset: 0x00000842
		public StatisticDisplayType[] AvailableDisplays
		{
			get
			{
				return new StatisticDisplayType[]
				{
					StatisticDisplayType.TreeView,
					StatisticDisplayType.Histogram
				};
			}
		}

		// Token: 0x04000001 RID: 1
		private Dictionary<int, CPUFrequency.DataModel> m_dataModels = new Dictionary<int, CPUFrequency.DataModel>();

		// Token: 0x0200001C RID: 28
		public class DataModel
		{
			// Token: 0x040000A0 RID: 160
			public SortedDictionary<uint, long> TotalTime = new SortedDictionary<uint, long>();

			// Token: 0x040000A1 RID: 161
			public SortedDictionary<uint, SortedDictionary<uint, long>> CpuFrequencyPerCore = new SortedDictionary<uint, SortedDictionary<uint, long>>();

			// Token: 0x0200002C RID: 44
			public class CpuFrequencyEvent
			{
				// Token: 0x060000E3 RID: 227 RVA: 0x0000AF8F File Offset: 0x0000918F
				public override int GetHashCode()
				{
					return (int)((this.Cpu + 1U) * this.Frequency);
				}

				// Token: 0x060000E4 RID: 228 RVA: 0x0000AFA0 File Offset: 0x000091A0
				public override bool Equals(object obj)
				{
					CPUFrequency.DataModel.CpuFrequencyEvent cpuFrequencyEvent = obj as CPUFrequency.DataModel.CpuFrequencyEvent;
					uint? num = ((cpuFrequencyEvent != null) ? new uint?(cpuFrequencyEvent.Cpu) : null);
					uint num2 = this.Cpu;
					if ((num.GetValueOrDefault() == num2) & (num != null))
					{
						CPUFrequency.DataModel.CpuFrequencyEvent cpuFrequencyEvent2 = obj as CPUFrequency.DataModel.CpuFrequencyEvent;
						num = ((cpuFrequencyEvent2 != null) ? new uint?(cpuFrequencyEvent2.Frequency) : null);
						num2 = this.Frequency;
						return (num.GetValueOrDefault() == num2) & (num != null);
					}
					return false;
				}

				// Token: 0x040000D2 RID: 210
				public uint Cpu;

				// Token: 0x040000D3 RID: 211
				public uint Frequency;

				// Token: 0x040000D4 RID: 212
				public long Time;
			}
		}
	}
}
