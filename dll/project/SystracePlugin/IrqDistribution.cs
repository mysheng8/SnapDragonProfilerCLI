using System;
using System.Collections.Generic;
using System.Linq;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000010 RID: 16
	public class IrqDistribution : IStatistic
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000504F File Offset: 0x0000324F
		public string Name
		{
			get
			{
				return "Irq Distribution";
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00005056 File Offset: 0x00003256
		public string Category
		{
			get
			{
				return "Thread Level Data";
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000505D File Offset: 0x0000325D
		public string Description
		{
			get
			{
				return "Irq usage analysis statistical data. Requires 'Irq' Trace metric to be enabled";
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000061 RID: 97 RVA: 0x00005064 File Offset: 0x00003264
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			IrqDistribution.IrqDataModel irqDataModel = this.BuildDataModel(captureID);
			SortedSet<uint> sortedSet = new SortedSet<uint>();
			foreach (uint num in irqDataModel.TotalIrqPerCore.Keys)
			{
				sortedSet.Add(num);
			}
			foreach (uint num2 in irqDataModel.TotalSoftIrqPerCore.Keys)
			{
				sortedSet.Add(num2);
			}
			sortedSet.Add(uint.MaxValue);
			int num3 = sortedSet.Count * 2 + 2;
			Type[] array = new Type[num3];
			string[] array2 = new string[num3];
			array[0] = typeof(string);
			array2[0] = "Name";
			array[1] = typeof(uint);
			array2[1] = "IRQ";
			for (int i = 2; i < num3; i += 2)
			{
				array[i] = typeof(Duration);
				array[i + 1] = typeof(uint);
				uint num4 = Enumerable.ElementAt<uint>(sortedSet, (i - 2) / 2);
				array2[i] = ((num4 != uint.MaxValue) ? ("Core " + num4.ToString()) : "All Cores");
				array2[i + 1] = " Count";
			}
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			TreeNode treeNode = new TreeNode();
			treeNode.Values = new object[num3];
			treeNode.Values[0] = "Total Irq Events";
			for (int j = 0; j < irqDataModel.TotalIrqPerCore.Values.Count * 2; j += 2)
			{
				treeNode.Values[j + 2] = Enumerable.ElementAt<IrqDistribution.IrqDataModel.Data>(irqDataModel.TotalIrqPerCore.Values, j / 2).Time;
				treeNode.Values[j + 1 + 2] = Enumerable.ElementAt<IrqDistribution.IrqDataModel.Data>(irqDataModel.TotalIrqPerCore.Values, j / 2).Count;
			}
			treeModel.Nodes.Add(treeNode);
			TreeNode treeNode2 = new TreeNode();
			treeNode2.Values = new object[num3];
			treeNode2.Values[0] = "Irq Distribution";
			foreach (KeyValuePair<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>> keyValuePair in irqDataModel.IrqDataPerCore)
			{
				IrqDistribution.IrqDataModel.IrqEvent key = keyValuePair.Key;
				SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> value = keyValuePair.Value;
				TreeNode treeNode3 = new TreeNode();
				treeNode3.Values = new object[num3];
				treeNode3.Values[0] = key.Name;
				treeNode3.Values[1] = key.Irq;
				for (int k = 0; k < irqDataModel.TotalIrqPerCore.Values.Count * 2; k += 2)
				{
					IrqDistribution.IrqDataModel.Data data;
					if (keyValuePair.Value.TryGetValue(Enumerable.ElementAt<uint>(irqDataModel.TotalIrqPerCore.Keys, k / 2), ref data))
					{
						treeNode3.Values[k + 2] = data.Time;
						treeNode3.Values[k + 1 + 2] = data.Count;
					}
				}
				treeNode2.Children.Add(treeNode3);
			}
			treeModel.Nodes.Add(treeNode2);
			TreeNode treeNode4 = new TreeNode();
			treeNode4.Values = new object[num3];
			treeNode4.Values[0] = "Total Softirq Events";
			for (int l = 0; l < irqDataModel.TotalSoftIrqPerCore.Values.Count * 2; l += 2)
			{
				treeNode4.Values[l + 2] = Enumerable.ElementAt<IrqDistribution.IrqDataModel.Data>(irqDataModel.TotalSoftIrqPerCore.Values, l / 2).Time;
				treeNode4.Values[l + 1 + 2] = Enumerable.ElementAt<IrqDistribution.IrqDataModel.Data>(irqDataModel.TotalSoftIrqPerCore.Values, l / 2).Count;
			}
			treeModel.Nodes.Add(treeNode4);
			TreeNode treeNode5 = new TreeNode();
			treeNode5.Values = new object[num3];
			treeNode5.Values[0] = "Softirq Distribution";
			foreach (KeyValuePair<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>> keyValuePair2 in irqDataModel.SoftIrqDataPerCore)
			{
				IrqDistribution.IrqDataModel.IrqEvent key2 = keyValuePair2.Key;
				SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> value2 = keyValuePair2.Value;
				TreeNode treeNode6 = new TreeNode();
				treeNode6.Values = new object[num3];
				treeNode6.Values[0] = key2.Name;
				treeNode6.Values[1] = key2.Irq;
				for (int m = 0; m < irqDataModel.TotalSoftIrqPerCore.Values.Count * 2; m += 2)
				{
					IrqDistribution.IrqDataModel.Data data2;
					if (keyValuePair2.Value.TryGetValue(Enumerable.ElementAt<uint>(irqDataModel.TotalSoftIrqPerCore.Keys, m / 2), ref data2))
					{
						treeNode6.Values[m + 2] = data2.Time;
						treeNode6.Values[m + 1 + 2] = data2.Count;
					}
				}
				treeNode5.Children.Add(treeNode6);
			}
			treeModel.Nodes.Add(treeNode5);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = treeModel;
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000055F8 File Offset: 0x000037F8
		private IrqDistribution.IrqDataModel BuildDataModel(int captureID)
		{
			IrqDistribution.IrqDataModel irqDataModel = new IrqDistribution.IrqDataModel();
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("IrqHandlerEntry", captureID);
			ModelObjectDataList eventData2 = FTraceEventDataRetriever.GetEventData("IrqHandlerExit", captureID);
			ModelObjectDataList eventData3 = FTraceEventDataRetriever.GetEventData("SoftirqEntry", captureID);
			ModelObjectDataList eventData4 = FTraceEventDataRetriever.GetEventData("SoftirqExit", captureID);
			IrqDistribution.MatchAndRecordIrq(eventData, eventData2, ref irqDataModel, false);
			IrqDistribution.MatchAndRecordIrq(eventData3, eventData4, ref irqDataModel, true);
			return irqDataModel;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005654 File Offset: 0x00003854
		public static void MatchAndRecordIrq(ModelObjectDataList irqEntryEvents, ModelObjectDataList irqExitEvents, ref IrqDistribution.IrqDataModel dataModel, bool soft)
		{
			int i = 0;
			LinkedList<FTraceEventDataRetriever.FtraceEventData> linkedList = new LinkedList<FTraceEventDataRetriever.FtraceEventData>();
			foreach (ModelObjectData modelObjectData in irqEntryEvents)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				string text = (soft ? "vecName" : "name");
				IrqDistribution.IrqDataModel.IrqEvent irqEvent = new IrqDistribution.IrqDataModel.IrqEvent();
				ftraceEventData.EventParams.TryGetValue(text, out irqEvent.Name);
				string text2 = (soft ? "vec" : "irq");
				string text3 = "";
				ftraceEventData.EventParams.TryGetValue(text2, out text3);
				uint.TryParse(text3, out irqEvent.Irq);
				FTraceEventDataRetriever.FtraceEventData ftraceEventData2 = null;
				for (LinkedListNode<FTraceEventDataRetriever.FtraceEventData> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					string text4 = "";
					linkedListNode.Value.EventParams.TryGetValue(text2, out text4);
					uint num;
					uint.TryParse(text4, out num);
					if (linkedListNode.Value.CpuNum == ftraceEventData.CpuNum && num == irqEvent.Irq)
					{
						ftraceEventData2 = linkedListNode.Value;
						linkedList.Remove(linkedListNode);
						break;
					}
				}
				if (ftraceEventData2 == null)
				{
					while (i < irqExitEvents.Count)
					{
						ModelObjectData modelObjectData2 = irqExitEvents[i];
						FTraceEventDataRetriever.FtraceEventData ftraceEventData3 = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData2);
						string text5 = "";
						ftraceEventData.EventParams.TryGetValue(text2, out text5);
						uint num2;
						uint.TryParse(text3, out num2);
						if (ftraceEventData3.CpuNum == ftraceEventData.CpuNum && num2 == irqEvent.Irq)
						{
							ftraceEventData2 = ftraceEventData3;
							i++;
							break;
						}
						linkedList.AddLast(ftraceEventData3);
						i++;
					}
				}
				if (ftraceEventData2 != null)
				{
					IrqDistribution.UpdateDataModelWithIrqMatches(ftraceEventData, ftraceEventData2, irqEvent, ref dataModel, soft);
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005818 File Offset: 0x00003A18
		private static void UpdateDataModelWithIrqMatches(FTraceEventDataRetriever.FtraceEventData irqEntryEvent, FTraceEventDataRetriever.FtraceEventData exitMatch, IrqDistribution.IrqDataModel.IrqEvent irqEvent, ref IrqDistribution.IrqDataModel dataModel, bool soft)
		{
			Dictionary<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>> dictionary = (soft ? dataModel.SoftIrqDataPerCore : dataModel.IrqDataPerCore);
			SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> sortedDictionary;
			if (!dictionary.TryGetValue(irqEvent, out sortedDictionary))
			{
				sortedDictionary = new SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>();
				dictionary[irqEvent] = sortedDictionary;
			}
			IrqDistribution.IrqDataModel.Data data;
			if (!sortedDictionary.TryGetValue(irqEntryEvent.CpuNum, ref data))
			{
				data = new IrqDistribution.IrqDataModel.Data();
				sortedDictionary[irqEntryEvent.CpuNum] = data;
			}
			IrqDistribution.IrqDataModel.Data data2;
			if (!sortedDictionary.TryGetValue(4294967295U, ref data2))
			{
				data2 = new IrqDistribution.IrqDataModel.Data();
				sortedDictionary[uint.MaxValue] = data2;
			}
			SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> sortedDictionary2 = (soft ? dataModel.TotalSoftIrqPerCore : dataModel.TotalIrqPerCore);
			IrqDistribution.IrqDataModel.Data data3;
			if (!sortedDictionary2.TryGetValue(irqEntryEvent.CpuNum, ref data3))
			{
				data3 = new IrqDistribution.IrqDataModel.Data();
				sortedDictionary2[irqEntryEvent.CpuNum] = data3;
			}
			IrqDistribution.IrqDataModel.Data data4;
			if (!sortedDictionary2.TryGetValue(4294967295U, ref data4))
			{
				data4 = new IrqDistribution.IrqDataModel.Data();
				sortedDictionary2[uint.MaxValue] = data4;
			}
			long num = ((exitMatch.TimeStampBegin > 0L && irqEntryEvent.TimeStampBegin > 0L) ? (exitMatch.TimeStampBegin - irqEntryEvent.TimeStampBegin) : 0L);
			data.Count += 1U;
			IrqDistribution.IrqDataModel.Data data5 = data;
			data5.Time += num;
			data2.Count += 1U;
			IrqDistribution.IrqDataModel.Data data6 = data2;
			data6.Time += num;
			data3.Count += 1U;
			IrqDistribution.IrqDataModel.Data data7 = data3;
			data7.Time += num;
			data4.Count += 1U;
			IrqDistribution.IrqDataModel.Data data8 = data4;
			data8.Time += num;
		}

		// Token: 0x0400002D RID: 45
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x0400002E RID: 46
		public const uint ALL_CORES = 4294967295U;

		// Token: 0x02000023 RID: 35
		public class IrqDataModel
		{
			// Token: 0x040000B2 RID: 178
			public SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> TotalIrqPerCore = new SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>();

			// Token: 0x040000B3 RID: 179
			public SortedDictionary<uint, IrqDistribution.IrqDataModel.Data> TotalSoftIrqPerCore = new SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>();

			// Token: 0x040000B4 RID: 180
			public Dictionary<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>> IrqDataPerCore = new Dictionary<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>>();

			// Token: 0x040000B5 RID: 181
			public Dictionary<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>> SoftIrqDataPerCore = new Dictionary<IrqDistribution.IrqDataModel.IrqEvent, SortedDictionary<uint, IrqDistribution.IrqDataModel.Data>>();

			// Token: 0x0200002E RID: 46
			public class IrqEvent
			{
				// Token: 0x060000E6 RID: 230 RVA: 0x0000B023 File Offset: 0x00009223
				public override int GetHashCode()
				{
					return this.Name.GetHashCode();
				}

				// Token: 0x060000E7 RID: 231 RVA: 0x0000B030 File Offset: 0x00009230
				public override bool Equals(object obj)
				{
					IrqDistribution.IrqDataModel.IrqEvent irqEvent = obj as IrqDistribution.IrqDataModel.IrqEvent;
					return ((irqEvent != null) ? irqEvent.Name : null) == this.Name;
				}

				// Token: 0x040000D9 RID: 217
				public string Name;

				// Token: 0x040000DA RID: 218
				public uint Irq;
			}

			// Token: 0x0200002F RID: 47
			public class Data
			{
				// Token: 0x040000DB RID: 219
				public uint Count;

				// Token: 0x040000DC RID: 220
				public Duration Time;
			}
		}
	}
}
