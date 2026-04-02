using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000B RID: 11
	internal class GPUStateAndFreqDistribution : IStatistic
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003C83 File Offset: 0x00001E83
		public string Name
		{
			get
			{
				return "GPU State and Freq Distrubution";
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003C8A File Offset: 0x00001E8A
		public string Description
		{
			get
			{
				return "Distribution of time spent in KGSL frequency and state. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003C94 File Offset: 0x00001E94
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			SortedList<long, object> sortedList = this.BuildCombinedList(captureID);
			SortedDictionary<string, long> sortedDictionary = this.BuildDataModel(sortedList);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = this.BuildTreeModel(sortedDictionary);
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003CF8 File Offset: 0x00001EF8
		private SortedList<long, object> BuildCombinedList(int captureID)
		{
			SortedList<long, object> sortedList = new SortedList<long, object>();
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslPwrSetState", captureID);
			ModelObjectDataList eventData2 = FTraceEventDataRetriever.GetEventData("KgslPwrlevel", captureID);
			if (eventData2.Count == 0 || eventData.Count == 0)
			{
				return sortedList;
			}
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					string text = "";
					if (ftraceEventData.EventParams.TryGetValue("state", out text))
					{
						sortedList.Add(ftraceEventData.TimeStampBegin, text);
					}
				}
			}
			foreach (ModelObjectData modelObjectData2 in eventData2)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData2 = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData2);
				if (ftraceEventData2.TimeStampBegin > 0L)
				{
					int num = 0;
					string text2 = "";
					if (ftraceEventData2.EventParams.TryGetValue("freq", out text2) && int.TryParse(text2, out num))
					{
						sortedList.Add(ftraceEventData2.TimeStampBegin, num);
					}
				}
			}
			return sortedList;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003E34 File Offset: 0x00002034
		private SortedDictionary<string, long> BuildDataModel(SortedList<long, object> freqAndStateEvents)
		{
			SortedDictionary<string, long> sortedDictionary = new SortedDictionary<string, long>();
			string text = null;
			int num = -1;
			long num2 = 0L;
			foreach (KeyValuePair<long, object> keyValuePair in freqAndStateEvents)
			{
				long key = keyValuePair.Key;
				object value = keyValuePair.Value;
				if (num2 != 0L && text != null)
				{
					string text2 = text;
					string text3;
					if (text2 == "NAP" || text2 == "SLUMBER")
					{
						text3 = text;
					}
					else
					{
						text3 = num.ToString();
					}
					long num3 = 0L;
					sortedDictionary.TryGetValue(text3, ref num3);
					long num4 = 0L;
					sortedDictionary.TryGetValue("total", ref num4);
					long num5 = key - num2;
					sortedDictionary[text3] = num3 + num5;
					sortedDictionary["total"] = num4 + num5;
				}
				num2 = key;
				if (value.GetType() == typeof(string))
				{
					text = (string)value;
				}
				else if (value.GetType() == typeof(int))
				{
					num = (int)value;
				}
			}
			return sortedDictionary;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003F64 File Offset: 0x00002164
		private TreeModel BuildTreeModel(SortedDictionary<string, long> durationPerStateAndFreqDict)
		{
			int num = 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "Frequency";
			array[1] = typeof(Duration);
			array2[1] = "Time Spent";
			array[2] = typeof(double);
			array2[2] = "%";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<string, long> keyValuePair in durationPerStateAndFreqDict)
			{
				if (!(keyValuePair.Key == "total"))
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					treeNode.Values[0] = keyValuePair.Key;
					treeNode.Values[1] = new Duration(keyValuePair.Value);
					treeNode.Values[2] = (double)keyValuePair.Value / (double)durationPerStateAndFreqDict["total"] * 100.0;
					treeModel.Nodes.Add(treeNode);
				}
			}
			return treeModel;
		}

		// Token: 0x04000028 RID: 40
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
