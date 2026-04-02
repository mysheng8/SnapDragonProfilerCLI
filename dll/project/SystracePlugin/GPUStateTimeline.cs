using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000D RID: 13
	internal class GPUStateTimeline : IStatistic
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004357 File Offset: 0x00002557
		public string Name
		{
			get
			{
				return "GPU State Timeline";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000435E File Offset: 0x0000255E
		public string Description
		{
			get
			{
				return "List of KGSL power state change events. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600004F RID: 79 RVA: 0x00004368 File Offset: 0x00002568
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslPwrSetState", captureID);
			int num = 2;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(string);
			array2[1] = "KGSL Power State";
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
					ftraceEventData.EventParams.TryGetValue("state", out text);
					treeNode.Values[1] = text;
					treeModel.Nodes.Add(treeNode);
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0400002A RID: 42
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
