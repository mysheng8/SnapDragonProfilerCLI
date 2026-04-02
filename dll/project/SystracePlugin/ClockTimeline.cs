using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000005 RID: 5
	internal class ClockTimeline : ClockBase, IStatistic
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002C2B File Offset: 0x00000E2B
		public string Name
		{
			get
			{
				return "Clock Timeline";
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002883 File Offset: 0x00000A83
		public string Category
		{
			get
			{
				return "Clocks/Bus Votes";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002C32 File Offset: 0x00000E32
		public string Description
		{
			get
			{
				return "List of clock events events. Requires 'Power' and 'CPU Frequency' Trace metric to be enabled";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x06000017 RID: 23 RVA: 0x00002C3C File Offset: 0x00000E3C
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			SortedList<long, ClockBase.ClockEvent> sortedList = ClockBase.BuildCombinedEventList(captureID);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = this.BuildTreeModel(sortedList);
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002C98 File Offset: 0x00000E98
		private TreeModel BuildTreeModel(SortedList<long, ClockBase.ClockEvent> combinedEvents)
		{
			int num = 4;
			Type[] array = new Type[num];
			string[] array2 = new string[num];
			array[0] = typeof(string);
			array2[0] = "Clock";
			array[1] = typeof(Duration);
			array2[1] = "Timestamp";
			array[2] = typeof(string);
			array2[2] = "Event";
			array[3] = typeof(uint);
			array2[3] = "Frequency";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			Dictionary<string, ClockTimeline.ClockInfo> dictionary = new Dictionary<string, ClockTimeline.ClockInfo>();
			foreach (KeyValuePair<long, ClockBase.ClockEvent> keyValuePair in combinedEvents)
			{
				long key = keyValuePair.Key;
				ClockBase.ClockEvent value = keyValuePair.Value;
				ClockTimeline.ClockInfo clockInfo = null;
				if (!dictionary.TryGetValue(value.Clock, out clockInfo))
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[num];
					treeNode.Values[0] = value.Clock;
					treeModel.Nodes.Add(treeNode);
					clockInfo = new ClockTimeline.ClockInfo();
					clockInfo.TreeNode = treeNode;
					clockInfo.CurrentFrequency = 0U;
					dictionary[value.Clock] = clockInfo;
				}
				TreeNode treeNode2 = new TreeNode();
				treeNode2.Values = new object[num];
				treeNode2.Values[0] = "";
				treeNode2.Values[1] = new Duration(key);
				switch (value.Event)
				{
				case ClockBase.ClockEvent.EventType.Enable:
					treeNode2.Values[2] = "Clock Enable";
					treeNode2.Values[3] = clockInfo.CurrentFrequency;
					break;
				case ClockBase.ClockEvent.EventType.Disable:
					treeNode2.Values[2] = "Clock Disable";
					treeNode2.Values[3] = 0;
					break;
				case ClockBase.ClockEvent.EventType.SetRate:
					treeNode2.Values[2] = "Clock Set Rate";
					clockInfo.CurrentFrequency = value.Frequency;
					treeNode2.Values[3] = clockInfo.CurrentFrequency;
					break;
				}
				clockInfo.TreeNode.Children.Add(treeNode2);
			}
			return treeModel;
		}

		// Token: 0x04000003 RID: 3
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x02000020 RID: 32
		private class ClockInfo
		{
			// Token: 0x040000AA RID: 170
			public TreeNode TreeNode;

			// Token: 0x040000AB RID: 171
			public uint CurrentFrequency;
		}
	}
}
