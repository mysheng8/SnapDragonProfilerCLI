using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000008 RID: 8
	internal class GPUFreqDistribution : IStatistic
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000035CB File Offset: 0x000017CB
		public string Name
		{
			get
			{
				return "GPU Freq Distribution";
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000035D2 File Offset: 0x000017D2
		public string Description
		{
			get
			{
				return "Distribution of time spent in KGSL frequencies. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600002B RID: 43 RVA: 0x000035DC File Offset: 0x000017DC
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			SortedDictionary<int, long> sortedDictionary = this.BuildDataModel(captureID);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = this.BuildTreeModel(sortedDictionary);
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003638 File Offset: 0x00001838
		private SortedDictionary<int, long> BuildDataModel(int captureID)
		{
			SortedDictionary<int, long> sortedDictionary = new SortedDictionary<int, long>();
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslPwrlevel", captureID);
			int num = -1;
			long num2 = 0L;
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					string text = "";
					int num3 = 0;
					if (ftraceEventData.EventParams.TryGetValue("freq", out text) && int.TryParse(text, out num3) && num != num3)
					{
						if (num2 != 0L)
						{
							this.UpdateDurations(ref sortedDictionary, num, num2, ftraceEventData.TimeStampBegin);
						}
						num2 = ftraceEventData.TimeStampBegin;
						num = num3;
					}
				}
			}
			long lastTimeStamp = FTraceEventDataRetriever.GetLastTimeStamp(captureID);
			this.UpdateDurations(ref sortedDictionary, num, num2, lastTimeStamp);
			return sortedDictionary;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003710 File Offset: 0x00001910
		private void UpdateDurations(ref SortedDictionary<int, long> durationPerFreqDict, int freq, long lastTimeStamp, long currentTimestamp)
		{
			long num = 0L;
			durationPerFreqDict.TryGetValue(freq, ref num);
			long num2 = 0L;
			durationPerFreqDict.TryGetValue(-1, ref num2);
			long num3 = currentTimestamp - lastTimeStamp;
			durationPerFreqDict[freq] = num + num3;
			durationPerFreqDict[-1] = num2 + num3;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003754 File Offset: 0x00001954
		private TreeModel BuildTreeModel(SortedDictionary<int, long> durationPerFreqDict)
		{
			int num = 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(int);
			array2[0] = "GPU Freq (Hz)";
			array[1] = typeof(Duration);
			array2[1] = "Time Spent";
			array[2] = typeof(double);
			array2[2] = "Percent";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<int, long> keyValuePair in durationPerFreqDict)
			{
				int key = keyValuePair.Key;
				long value = keyValuePair.Value;
				long num2 = durationPerFreqDict[-1];
				if (key != -1)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					treeNode.Values[0] = keyValuePair.Key;
					treeNode.Values[1] = new Duration(value);
					treeNode.Values[2] = (double)value / (double)num2 * 100.0;
					treeModel.Nodes.Add(treeNode);
				}
			}
			return treeModel;
		}

		// Token: 0x04000024 RID: 36
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x04000025 RID: 37
		private const int TOTAL = -1;
	}
}
