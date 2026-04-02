using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000C RID: 12
	internal class GPUStateDistribution : IStatistic
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000040B3 File Offset: 0x000022B3
		public string Name
		{
			get
			{
				return "GPU State Distrubution";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000040BA File Offset: 0x000022BA
		public string Description
		{
			get
			{
				return "Distribution of time spent in KGSL power states. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000049 RID: 73 RVA: 0x000040C4 File Offset: 0x000022C4
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslPwrSetState", captureID);
			SortedDictionary<string, long> sortedDictionary = new SortedDictionary<string, long>();
			string text = "";
			long num = 0L;
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					if (num != 0L && text != "")
					{
						long num2 = 0L;
						sortedDictionary.TryGetValue(text, ref num2);
						long num3 = 0L;
						sortedDictionary.TryGetValue("total", ref num3);
						long num4 = ftraceEventData.TimeStampBegin - num;
						sortedDictionary[text] = num2 + num4;
						sortedDictionary["total"] = num3 + num4;
					}
					num = ftraceEventData.TimeStampBegin;
					string text2 = "";
					if (ftraceEventData.EventParams.TryGetValue("state", out text2))
					{
						text = text2;
					}
				}
			}
			int num5 = 3;
			Type[] array = new Type[num5];
			string[] array2 = new string[num5];
			array[0] = typeof(string);
			array2[0] = "KGSL Power State";
			array[1] = typeof(Duration);
			array2[1] = "Time Spent";
			array[2] = typeof(double);
			array2[2] = "%";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<string, long> keyValuePair in sortedDictionary)
			{
				if (!(keyValuePair.Key == "total"))
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num5];
					treeNode.Values[0] = keyValuePair.Key;
					treeNode.Values[1] = new Duration(keyValuePair.Value);
					treeNode.Values[2] = (double)keyValuePair.Value / (double)sortedDictionary["total"] * 100.0;
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x04000029 RID: 41
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
