using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000A RID: 10
	internal class GPUBusTimeline : IStatistic
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003A93 File Offset: 0x00001C93
		public string Name
		{
			get
			{
				return "GPU Bus Vote Timeline";
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003A9A File Offset: 0x00001C9A
		public string Description
		{
			get
			{
				return "List of KGSL bus level change events. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600003A RID: 58 RVA: 0x00003AA4 File Offset: 0x00001CA4
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslBuslevel", captureID);
			int num = 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(int);
			array2[1] = "Power Level";
			array[2] = typeof(int);
			array2[2] = "Bus Vote";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					treeNode.Values[0] = new Duration(ftraceEventData.TimeStampBegin);
					string text = "";
					ftraceEventData.EventParams.TryGetValue("pwrlevel", out text);
					int num2 = 0;
					if (int.TryParse(text, out num2))
					{
						treeNode.Values[1] = num2;
					}
					string text2 = "";
					ftraceEventData.EventParams.TryGetValue("bus", out text2);
					int num3 = 0;
					if (int.TryParse(text2, out num3))
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

		// Token: 0x04000027 RID: 39
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
