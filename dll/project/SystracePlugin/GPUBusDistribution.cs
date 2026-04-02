using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000007 RID: 7
	internal class GPUBusDistribution : IStatistic
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00003332 File Offset: 0x00001532
		public string Name
		{
			get
			{
				return "GPU Bus Vote Distribution";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00003340 File Offset: 0x00001540
		public string Description
		{
			get
			{
				return "Distribution of time spent in KGSL bus levels. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000025 RID: 37 RVA: 0x00003348 File Offset: 0x00001548
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslBuslevel", captureID);
			SortedDictionary<int, long> sortedDictionary = new SortedDictionary<int, long>();
			int num = -1;
			long num2 = 0L;
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					if (num2 != 0L && num != -1)
					{
						long num3 = 0L;
						sortedDictionary.TryGetValue(num, ref num3);
						long num4 = 0L;
						sortedDictionary.TryGetValue(-1, ref num4);
						long num5 = ftraceEventData.TimeStampBegin - num2;
						sortedDictionary[num] = num3 + num5;
						sortedDictionary[-1] = num4 + num5;
					}
					num2 = ftraceEventData.TimeStampBegin;
					string text = "";
					int num6 = 0;
					if (ftraceEventData.EventParams.TryGetValue("bus", out text) && int.TryParse(text, out num6))
					{
						num = num6;
					}
				}
			}
			int num7 = 3;
			Type[] array = new Type[num7];
			string[] array2 = new string[num7];
			array[0] = typeof(int);
			array2[0] = "Bus Level";
			array[1] = typeof(Duration);
			array2[1] = "Time Spent";
			array[2] = typeof(double);
			array2[2] = "Percent";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<int, long> keyValuePair in sortedDictionary)
			{
				if (keyValuePair.Key != -1)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num7];
					treeNode.Values[0] = keyValuePair.Key;
					treeNode.Values[1] = new Duration(keyValuePair.Value);
					treeNode.Values[2] = (double)keyValuePair.Value / (double)sortedDictionary[-1] * 100.0;
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x04000023 RID: 35
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
