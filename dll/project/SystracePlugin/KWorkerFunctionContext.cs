using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000011 RID: 17
	internal class KWorkerFunctionContext : IStatistic
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000059C3 File Offset: 0x00003BC3
		public string Name
		{
			get
			{
				return "KWorker Function Context";
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00005056 File Offset: 0x00003256
		public string Category
		{
			get
			{
				return "Thread Level Data";
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000059CA File Offset: 0x00003BCA
		public string Description
		{
			get
			{
				return "Number of times each kworker function is called by a particular thread. Requires 'Kernel Workqueues' Trace metric to be enabled";
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600006A RID: 106 RVA: 0x000059D4 File Offset: 0x00003BD4
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			int num = 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "KWorker Thread";
			array[1] = typeof(string);
			array2[1] = "KWorker Function";
			array[2] = typeof(uint);
			array2[2] = "# times called";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("WorkqueueQueueWork", captureID);
			Dictionary<string, Dictionary<string, uint>> dictionary = new Dictionary<string, Dictionary<string, uint>>();
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					string text = "";
					string text2 = "";
					Dictionary<string, uint> dictionary2 = null;
					if (ftraceEventData.EventParams.TryGetValue("Task", out text) && ftraceEventData.EventParams.TryGetValue("Function", out text2))
					{
						if (!dictionary.TryGetValue(text, out dictionary2))
						{
							dictionary2 = new Dictionary<string, uint>();
							dictionary[text] = dictionary2;
						}
						if (!dictionary2.ContainsKey(text2))
						{
							dictionary2[text2] = 0U;
						}
						Dictionary<string, uint> dictionary3 = dictionary2;
						string text3 = text2;
						uint num2 = dictionary3[text3];
						dictionary3[text3] = num2 + 1U;
					}
				}
			}
			foreach (KeyValuePair<string, Dictionary<string, uint>> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				Dictionary<string, uint> value = keyValuePair.Value;
				foreach (KeyValuePair<string, uint> keyValuePair2 in value)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					string key2 = keyValuePair2.Key;
					uint value2 = keyValuePair2.Value;
					treeNode.Values[0] = key;
					treeNode.Values[1] = key2;
					treeNode.Values[2] = value2;
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0400002F RID: 47
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
