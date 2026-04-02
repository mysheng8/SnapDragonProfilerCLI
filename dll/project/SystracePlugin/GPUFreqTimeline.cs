using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000009 RID: 9
	internal class GPUFreqTimeline : IStatistic
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000038A3 File Offset: 0x00001AA3
		public string Name
		{
			get
			{
				return "GPU Freq Timeline";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003339 File Offset: 0x00001539
		public string Category
		{
			get
			{
				return "GPU Data";
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000038AA File Offset: 0x00001AAA
		public string Description
		{
			get
			{
				return "List of KGSL frequency change events. Requires 'KGSL' Trace metric to be enabled";
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000034 RID: 52 RVA: 0x000038B4 File Offset: 0x00001AB4
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("KgslPwrlevel", captureID);
			int num = 3;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(int);
			array2[1] = "Power Level";
			array[2] = typeof(int);
			array2[2] = "GPU Freq (Hz)";
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
					ftraceEventData.EventParams.TryGetValue("freq", out text2);
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

		// Token: 0x04000026 RID: 38
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
