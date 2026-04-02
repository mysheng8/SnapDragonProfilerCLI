using System;
using System.Collections.Generic;
using System.Linq;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000014 RID: 20
	internal class ThreadTime : IStatistic
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000619B File Offset: 0x0000439B
		public string Name
		{
			get
			{
				return "Thread Time Per Core";
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00005FB2 File Offset: 0x000041B2
		public string Category
		{
			get
			{
				return "Thread Level Data";
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000061A2 File Offset: 0x000043A2
		public string Description
		{
			get
			{
				return "Thread Time Per Core Statistical Data. Requires 'CPU Sched' Trace metric to be enabled";
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600007C RID: 124 RVA: 0x000061AC File Offset: 0x000043AC
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_treeModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ThreadTime.ThreadTimeDataModel threadTimeDataModel = this.BuildDataModel(captureID);
			int num = threadTimeDataModel.TimePerCoreDict.Keys.Count * 2 + 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "Name";
			array[1] = typeof(int);
			array2[1] = "PID";
			array[2] = typeof(int);
			array2[2] = "Priority";
			for (int i = 3; i < num; i += 2)
			{
				array[i] = typeof(Duration);
				array[i + 1] = typeof(double);
				uint num2 = Enumerable.ElementAt<uint>(threadTimeDataModel.TimePerCoreDict.Keys, (i - 3) / 2);
				array2[i] = ((num2 != uint.MaxValue) ? ("Core " + num2.ToString()) : "All Cores");
				array2[i + 1] = " %";
			}
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			TreeNode treeNode = new TreeNode();
			treeNode.Values = new object[num];
			treeNode.Values[0] = "Total";
			for (int j = 0; j < threadTimeDataModel.TimePerCoreDict.Values.Count * 2; j += 2)
			{
				treeNode.Values[j + 3] = Enumerable.ElementAt<Duration>(threadTimeDataModel.TimePerCoreDict.Values, j / 2);
				treeNode.Values[j + 1 + 3] = 100.0;
			}
			treeModel.Nodes.Add(treeNode);
			TreeNode treeNode2 = new TreeNode();
			treeNode2.Values = new object[num];
			treeNode2.Values[0] = "Threads";
			foreach (KeyValuePair<ThreadTime.ThreadTimeDataModel.ThreadDataModel, SortedDictionary<uint, Duration>> keyValuePair in threadTimeDataModel.TimePerThreadDict)
			{
				TreeNode treeNode3 = new TreeNode();
				treeNode3.Values = new object[num];
				ThreadTime.ThreadTimeDataModel.ThreadDataModel key = keyValuePair.Key;
				treeNode3.Values[0] = key.ThreadName;
				treeNode3.Values[1] = key.PID;
				treeNode3.Values[2] = key.Priority;
				for (int k = 0; k < threadTimeDataModel.TimePerCoreDict.Keys.Count * 2; k += 2)
				{
					Duration duration = Enumerable.ElementAt<Duration>(threadTimeDataModel.TimePerCoreDict.Values, k / 2);
					Duration duration2 = 0L;
					if (keyValuePair.Value.TryGetValue(Enumerable.ElementAt<uint>(threadTimeDataModel.TimePerCoreDict.Keys, k / 2), ref duration2))
					{
						treeNode3.Values[k + 3] = duration2;
						treeNode3.Values[k + 1 + 3] = (double)duration2 / (double)duration.Value * 100.0;
					}
				}
				treeNode2.Children.Add(treeNode3);
			}
			treeModel.Nodes.Add(treeNode2);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_treeModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006514 File Offset: 0x00004714
		private ThreadTime.ThreadTimeDataModel BuildDataModel(int captureID)
		{
			ThreadTime.ThreadTimeDataModel threadTimeDataModel = new ThreadTime.ThreadTimeDataModel();
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("Sched CPU", captureID);
			bool flag = true;
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				string text = "";
				string text2 = "";
				string text3 = "";
				ftraceEventData.EventParams.TryGetValue("Name:", out text2);
				ftraceEventData.EventParams.TryGetValue("PID:", out text);
				ftraceEventData.EventParams.TryGetValue("Priority:", out text3);
				ThreadTime.ThreadTimeDataModel.ThreadDataModel threadDataModel = new ThreadTime.ThreadTimeDataModel.ThreadDataModel();
				threadDataModel.ThreadName = text2;
				int.TryParse(text, out threadDataModel.PID);
				int.TryParse(text3, out threadDataModel.Priority);
				uint cpuNum = ftraceEventData.CpuNum;
				SortedDictionary<uint, Duration> sortedDictionary;
				if (!threadTimeDataModel.TimePerThreadDict.TryGetValue(threadDataModel, out sortedDictionary))
				{
					sortedDictionary = new SortedDictionary<uint, Duration>();
					threadTimeDataModel.TimePerThreadDict[threadDataModel] = sortedDictionary;
				}
				if (flag)
				{
					flag = false;
					long timeStampBegin = ftraceEventData.TimeStampBegin;
				}
				long timeStampEnd = ftraceEventData.TimeStampEnd;
				long num = ftraceEventData.TimeStampEnd - ftraceEventData.TimeStampBegin;
				Duration duration;
				if (!sortedDictionary.TryGetValue(cpuNum, ref duration))
				{
					duration = new Duration(0L);
				}
				sortedDictionary[cpuNum] = duration.Value + num;
				Duration duration2;
				if (!sortedDictionary.TryGetValue(4294967295U, ref duration2))
				{
					duration2 = new Duration(0L);
				}
				sortedDictionary[uint.MaxValue] = duration2.Value + num;
				Duration duration3 = 0L;
				threadTimeDataModel.TimePerCoreDict.TryGetValue(cpuNum, ref duration3);
				threadTimeDataModel.TimePerCoreDict[cpuNum] = duration3 + num;
				Duration duration4 = 0L;
				threadTimeDataModel.TimePerCoreDict.TryGetValue(uint.MaxValue, ref duration4);
				threadTimeDataModel.TimePerCoreDict[uint.MaxValue] = duration4 + num;
			}
			return threadTimeDataModel;
		}

		// Token: 0x04000032 RID: 50
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_treeModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x04000033 RID: 51
		public const uint ALL_CORES = 4294967295U;

		// Token: 0x02000025 RID: 37
		public class ThreadTimeDataModel
		{
			// Token: 0x040000B9 RID: 185
			public Dictionary<ThreadTime.ThreadTimeDataModel.ThreadDataModel, SortedDictionary<uint, Duration>> TimePerThreadDict = new Dictionary<ThreadTime.ThreadTimeDataModel.ThreadDataModel, SortedDictionary<uint, Duration>>();

			// Token: 0x040000BA RID: 186
			public SortedDictionary<uint, Duration> TimePerCoreDict = new SortedDictionary<uint, Duration>();

			// Token: 0x02000030 RID: 48
			public class ThreadDataModel
			{
				// Token: 0x060000EA RID: 234 RVA: 0x0000B04F File Offset: 0x0000924F
				public override int GetHashCode()
				{
					return this.ThreadName.GetHashCode();
				}

				// Token: 0x060000EB RID: 235 RVA: 0x0000B05C File Offset: 0x0000925C
				public override bool Equals(object obj)
				{
					ThreadTime.ThreadTimeDataModel.ThreadDataModel threadDataModel = obj as ThreadTime.ThreadTimeDataModel.ThreadDataModel;
					return ((threadDataModel != null) ? threadDataModel.ThreadName : null) == this.ThreadName;
				}

				// Token: 0x040000DD RID: 221
				public string ThreadName;

				// Token: 0x040000DE RID: 222
				public int PID;

				// Token: 0x040000DF RID: 223
				public int Priority;
			}
		}
	}
}
