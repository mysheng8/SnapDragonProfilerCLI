using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000F RID: 15
	internal class MDPTraceEvents : IStatistic
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00004A1F File Offset: 0x00002C1F
		public string Name
		{
			get
			{
				return "MDP Trace Events";
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000044E6 File Offset: 0x000026E6
		public string Category
		{
			get
			{
				return "MDP";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004A26 File Offset: 0x00002C26
		public string Description
		{
			get
			{
				return "MDP commit details. Requires 'MDSS' Trace metric to be enabled";
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600005B RID: 91 RVA: 0x00004A30 File Offset: 0x00002C30
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			int num = 10;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(string);
			array2[1] = "Function";
			array[2] = typeof(int);
			array2[2] = "Mixer";
			array[3] = typeof(ulong);
			array2[3] = "Bandwidth (bps)";
			array[4] = typeof(int);
			array2[4] = "Clock Rate (hz)";
			array[5] = typeof(int);
			array2[5] = "Num Layer";
			array[6] = typeof(int);
			array2[6] = "Format";
			array[7] = typeof(string);
			array2[7] = "Layer Size (x*y)";
			array[8] = typeof(int);
			array2[8] = "Source Area";
			array[9] = typeof(int);
			array2[9] = "Dest Area";
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("MdpCommit", captureID);
			ModelObjectDataList eventData2 = FTraceEventDataRetriever.GetEventData("MdpSsppChange", captureID);
			SortedList<long, FTraceEventDataRetriever.FtraceEventData> sortedList = new SortedList<long, FTraceEventDataRetriever.FtraceEventData>();
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					sortedList.Add(ftraceEventData.TimeStampBegin, ftraceEventData);
				}
			}
			foreach (ModelObjectData modelObjectData2 in eventData2)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData2 = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData2);
				if (ftraceEventData2.TimeStampBegin > 0L)
				{
					sortedList.Add(ftraceEventData2.TimeStampBegin, ftraceEventData2);
				}
			}
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<long, FTraceEventDataRetriever.FtraceEventData> keyValuePair in sortedList)
			{
				FTraceEventDataRetriever.FtraceEventData value = keyValuePair.Value;
				string name = value.Name;
				if (!(name == "MdpCommit"))
				{
					if (name == "MdpSsppChange")
					{
						string text = "";
						int num2 = -1;
						int num3 = -1;
						string text2 = "";
						int num4 = -1;
						string text3 = "";
						string text4 = "";
						string text5 = "";
						int num5 = -1;
						string text6 = "";
						int num6 = -1;
						string text7 = "";
						int num7 = -1;
						string text8 = "";
						int num8 = -1;
						string text9 = "";
						if (value.EventParams.TryGetValue("mixer", out text) && int.TryParse(text, out num2) && value.EventParams.TryGetValue("stage", out text2) && int.TryParse(text2, out num3) && value.EventParams.TryGetValue("format", out text3) && int.TryParse(text3, out num4) && value.EventParams.TryGetValue("imgW", out text4) && value.EventParams.TryGetValue("imgH", out text5) && value.EventParams.TryGetValue("srcW", out text6) && int.TryParse(text6, out num5) && value.EventParams.TryGetValue("srcH", out text7) && int.TryParse(text7, out num6) && value.EventParams.TryGetValue("dstW", out text8) && int.TryParse(text8, out num7) && value.EventParams.TryGetValue("dstH", out text9) && int.TryParse(text9, out num8))
						{
							TreeNode treeNode = new TreeNode();
							treeNode.Values = new object[num];
							treeNode.Values[0] = new Duration(value.TimeStampBegin);
							treeNode.Values[1] = "mdp_sspp_change";
							treeNode.Values[2] = num2;
							treeNode.Values[5] = num3;
							treeNode.Values[6] = num4;
							treeNode.Values[7] = text4 + "x" + text5;
							treeNode.Values[8] = num5 * num6;
							treeNode.Values[9] = num7 * num8;
							treeModel.Nodes.Add(treeNode);
						}
					}
				}
				else
				{
					string text10 = "";
					int num9 = -1;
					string text11 = "";
					ulong num10 = 0UL;
					string text12 = "";
					int num11 = 0;
					if (value.EventParams.TryGetValue("num", out text10) && int.TryParse(text10, out num9) && value.EventParams.TryGetValue("bandwidth", out text11) && ulong.TryParse(text11, out num10) && value.EventParams.TryGetValue("clkRate", out text12) && int.TryParse(text12, out num11))
					{
						TreeNode treeNode2 = new TreeNode();
						treeNode2.Values = new object[num];
						treeNode2.Values[0] = new Duration(value.TimeStampBegin);
						treeNode2.Values[1] = "mdp_commit";
						treeNode2.Values[2] = num9;
						treeNode2.Values[3] = num10;
						treeNode2.Values[4] = num11;
						treeModel.Nodes.Add(treeNode2);
					}
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0400002C RID: 44
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();
	}
}
