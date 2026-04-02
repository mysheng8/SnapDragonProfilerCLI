using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000012 RID: 18
	internal class KworkerFunctionDistribution : IStatistic
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00005C7B File Offset: 0x00003E7B
		public string Name
		{
			get
			{
				return "KWorker Function Distribution";
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00005056 File Offset: 0x00003256
		public string Category
		{
			get
			{
				return "Thread Level Data";
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00005C82 File Offset: 0x00003E82
		public string Description
		{
			get
			{
				return "KWorker threads execution time distribution. Requires 'Kernel Workqueues' Trace metric to be enabled";
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000070 RID: 112 RVA: 0x00005C8C File Offset: 0x00003E8C
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			int num = 4;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "KWorker Thread";
			array[1] = typeof(string);
			array2[1] = "KWorker Function";
			array[2] = typeof(Duration);
			array2[2] = "Execution Time";
			array[3] = typeof(double);
			array2[3] = "% of KWorker Execution Time";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("Kernel Workqueue Execs", captureID);
			Dictionary<string, KworkerFunctionDistribution.KWorkerInfo> dictionary = new Dictionary<string, KworkerFunctionDistribution.KWorkerInfo>();
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					KworkerFunctionDistribution.KWorkerInfo kworkerInfo = null;
					string text = "";
					string text2 = "";
					if (ftraceEventData.EventParams.TryGetValue("KWorkerThread:", out text) && ftraceEventData.EventParams.TryGetValue("Function:", out text2))
					{
						if (!dictionary.TryGetValue(text, out kworkerInfo))
						{
							kworkerInfo = new KworkerFunctionDistribution.KWorkerInfo();
							kworkerInfo.First = ftraceEventData.TimeStampBegin;
							dictionary[text] = kworkerInfo;
						}
						kworkerInfo.Last = ftraceEventData.TimeStampEnd;
						long num2 = 0L;
						kworkerInfo.PerFunctionDuration.TryGetValue(text2, out num2);
						num2 += ftraceEventData.TimeStampEnd - ftraceEventData.TimeStampBegin;
						kworkerInfo.PerFunctionDuration[text2] = num2;
					}
				}
			}
			foreach (KeyValuePair<string, KworkerFunctionDistribution.KWorkerInfo> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				KworkerFunctionDistribution.KWorkerInfo value = keyValuePair.Value;
				foreach (KeyValuePair<string, long> keyValuePair2 in value.PerFunctionDuration)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					string key2 = keyValuePair2.Key;
					long value2 = keyValuePair2.Value;
					treeNode.Values[0] = key;
					treeNode.Values[1] = key2;
					treeNode.Values[2] = new Duration(value2);
					long num3 = value.Last - value.First;
					treeNode.Values[3] = (double)value2 / (double)num3 * 100.0;
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x04000030 RID: 48
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x02000024 RID: 36
		private class KWorkerInfo
		{
			// Token: 0x040000B6 RID: 182
			public long First;

			// Token: 0x040000B7 RID: 183
			public long Last;

			// Token: 0x040000B8 RID: 184
			public Dictionary<string, long> PerFunctionDuration = new Dictionary<string, long>();
		}
	}
}
