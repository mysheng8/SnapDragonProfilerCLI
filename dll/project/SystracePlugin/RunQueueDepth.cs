using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000013 RID: 19
	internal class RunQueueDepth : IStatistic
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00005FAB File Offset: 0x000041AB
		public string Name
		{
			get
			{
				return "Runqueue Depth";
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00005FB2 File Offset: 0x000041B2
		public string Category
		{
			get
			{
				return "Thread Level Data";
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00005FB9 File Offset: 0x000041B9
		public string Description
		{
			get
			{
				return "Number of runnable tasks on a CPU at given time. Requires 'CPU Sched' Trace metric to be enabled";
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000076 RID: 118 RVA: 0x00005FC0 File Offset: 0x000041C0
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
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(int);
			array2[1] = "Core";
			array[2] = typeof(int);
			array2[2] = "Runqueue Length";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("SchedEnqDeqTask", captureID);
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					treeNode.Values[0] = new Duration(ftraceEventData.TimeStampBegin);
					int num2 = -1;
					string text = "";
					if (ftraceEventData.EventParams.TryGetValue("cpu", out text) && int.TryParse(text, out num2))
					{
						treeNode.Values[1] = num2;
					}
					int num3 = -1;
					string text2 = "";
					if (ftraceEventData.EventParams.TryGetValue("nrRunning", out text2) && int.TryParse(text2, out num3))
					{
						treeNode.Values[2] = num3;
					}
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x04000031 RID: 49
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
