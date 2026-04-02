using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x0200000E RID: 14
	internal class MDPCommitInfo : IStatistic
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000044DF File Offset: 0x000026DF
		public string Name
		{
			get
			{
				return "MDP layers/commit";
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000044E6 File Offset: 0x000026E6
		public string Category
		{
			get
			{
				return "MDP";
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000044ED File Offset: 0x000026ED
		public string Description
		{
			get
			{
				return "MDP commit info. Requires 'MDSS' Trace metric to be enabled";
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000055 RID: 85 RVA: 0x000044F4 File Offset: 0x000026F4
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			int num = 7;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(Duration);
			array2[0] = "Timestamp";
			array[1] = typeof(int);
			array2[1] = "Mixer Changed";
			array[2] = typeof(ulong);
			array2[2] = "Bandwith (bps)";
			array[3] = typeof(uint);
			array2[3] = "Clock Rate (hz)";
			array[4] = typeof(int);
			array2[4] = "Total Layers Changed";
			array[5] = typeof(ulong);
			array2[5] = "Total Source Area Changed";
			array[6] = typeof(ulong);
			array2[6] = "Total Dest Area Changed";
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
			Dictionary<uint, MDPCommitInfo.MDPSSPPChangeQueuue> dictionary = new Dictionary<uint, MDPCommitInfo.MDPSSPPChangeQueuue>();
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
						if (value.EventParams.TryGetValue("mixer", out text) && int.TryParse(text, out num2))
						{
							string text2 = "";
							int num3 = -1;
							if (value.EventParams.TryGetValue("num", out text2) && int.TryParse(text2, out num3))
							{
								int num4 = -1;
								string text3 = "";
								int num5 = -1;
								string text4 = "";
								int num6 = -1;
								string text5 = "";
								int num7 = -1;
								string text6 = "";
								if (value.EventParams.TryGetValue("srcW", out text3) && int.TryParse(text3, out num4) && value.EventParams.TryGetValue("srcH", out text4) && int.TryParse(text4, out num5) && value.EventParams.TryGetValue("dstW", out text5) && int.TryParse(text5, out num6) && value.EventParams.TryGetValue("dstH", out text6) && int.TryParse(text6, out num7))
								{
									MDPCommitInfo.MDPSSPPChangeQueuue mdpssppchangeQueuue = null;
									if (!dictionary.TryGetValue((uint)num2, out mdpssppchangeQueuue))
									{
										mdpssppchangeQueuue = new MDPCommitInfo.MDPSSPPChangeQueuue();
										dictionary[(uint)num2] = mdpssppchangeQueuue;
									}
									mdpssppchangeQueuue.AddLayer((uint)num3, (ulong)((long)(num4 * num5)), (ulong)((long)(num6 * num7)));
								}
							}
						}
					}
				}
				else
				{
					string text7 = "";
					int num8 = -1;
					string text8 = "";
					ulong num9 = 0UL;
					string text9 = "";
					uint num10 = 0U;
					if (value.EventParams.TryGetValue("num", out text7) && int.TryParse(text7, out num8) && value.EventParams.TryGetValue("bandwidth", out text8) && ulong.TryParse(text8, out num9) && value.EventParams.TryGetValue("clkRate", out text9) && uint.TryParse(text9, out num10))
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Values = new object[num];
						treeNode.Values[0] = new Duration(value.TimeStampBegin);
						treeNode.Values[1] = num8;
						treeNode.Values[2] = num9;
						treeNode.Values[3] = num10;
						MDPCommitInfo.MDPSSPPChangeQueuue mdpssppchangeQueuue2 = null;
						if (dictionary.TryGetValue((uint)num8, out mdpssppchangeQueuue2))
						{
							treeNode.Values[4] = mdpssppchangeQueuue2.GetTotalLayers();
							treeNode.Values[5] = mdpssppchangeQueuue2.GetSrcAreaChanged();
							treeNode.Values[6] = mdpssppchangeQueuue2.GetDstAreaChanged();
							mdpssppchangeQueuue2.Clear();
						}
						treeModel.Nodes.Add(treeNode);
					}
				}
			}
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x0400002B RID: 43
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x02000022 RID: 34
		private class MDPSSPPChangeQueuue
		{
			// Token: 0x060000CB RID: 203 RVA: 0x0000A5B7 File Offset: 0x000087B7
			public void AddLayer(uint layer, ulong srcAreaChanged, ulong dstAreaChanged)
			{
				this.m_layers[layer] = new Tuple<ulong, ulong>(srcAreaChanged, dstAreaChanged);
			}

			// Token: 0x060000CC RID: 204 RVA: 0x0000A5CC File Offset: 0x000087CC
			public void Clear()
			{
				this.m_layers.Clear();
			}

			// Token: 0x060000CD RID: 205 RVA: 0x0000A5DC File Offset: 0x000087DC
			public ulong GetSrcAreaChanged()
			{
				ulong num = 0UL;
				foreach (KeyValuePair<uint, Tuple<ulong, ulong>> keyValuePair in this.m_layers)
				{
					num += keyValuePair.Value.Item1;
				}
				return num;
			}

			// Token: 0x060000CE RID: 206 RVA: 0x0000A63C File Offset: 0x0000883C
			public ulong GetDstAreaChanged()
			{
				ulong num = 0UL;
				foreach (KeyValuePair<uint, Tuple<ulong, ulong>> keyValuePair in this.m_layers)
				{
					num += keyValuePair.Value.Item2;
				}
				return num;
			}

			// Token: 0x060000CF RID: 207 RVA: 0x0000A69C File Offset: 0x0000889C
			public int GetTotalLayers()
			{
				return this.m_layers.Count;
			}

			// Token: 0x040000B1 RID: 177
			private Dictionary<uint, Tuple<ulong, ulong>> m_layers = new Dictionary<uint, Tuple<ulong, ulong>>();
		}
	}
}
