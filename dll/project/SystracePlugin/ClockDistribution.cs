using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000004 RID: 4
	internal class ClockDistribution : ClockBase, IStatistic
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000287C File Offset: 0x00000A7C
		public string Name
		{
			get
			{
				return "Clock Distribution";
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002883 File Offset: 0x00000A83
		public string Category
		{
			get
			{
				return "Clocks/Bus Votes";
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000288A File Offset: 0x00000A8A
		public string Description
		{
			get
			{
				return "Distribution of Frequency for each clock. Requires 'Power' and 'CPU Frequency' Trace metric to be enabled";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002642 File Offset: 0x00000842
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

		// Token: 0x0600000F RID: 15 RVA: 0x00002894 File Offset: 0x00000A94
		public IStatisticDisplayViewModel[] GenerateViewModels(int captureID)
		{
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel;
			if (this.m_viewModels.TryGetValue(captureID, out treeViewStatisticDisplayViewModel))
			{
				return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel };
			}
			SortedList<long, ClockBase.ClockEvent> sortedList = ClockBase.BuildCombinedEventList(captureID);
			Dictionary<string, ClockDistribution.ClockInfo> dictionary = this.BuildClockInfoDict(sortedList);
			TreeViewStatisticDisplayViewModel treeViewStatisticDisplayViewModel2 = new TreeViewStatisticDisplayViewModel();
			treeViewStatisticDisplayViewModel2.Model = this.BuildTreeModel(dictionary);
			this.m_viewModels[captureID] = treeViewStatisticDisplayViewModel2;
			return new IStatisticDisplayViewModel[] { treeViewStatisticDisplayViewModel2 };
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000028F8 File Offset: 0x00000AF8
		private Dictionary<string, ClockDistribution.ClockInfo> BuildClockInfoDict(SortedList<long, ClockBase.ClockEvent> combinedEvents)
		{
			Dictionary<string, ClockDistribution.ClockInfo> dictionary = new Dictionary<string, ClockDistribution.ClockInfo>();
			if (combinedEvents.Count == 0)
			{
				return dictionary;
			}
			foreach (KeyValuePair<long, ClockBase.ClockEvent> keyValuePair in combinedEvents)
			{
				long key = keyValuePair.Key;
				ClockBase.ClockEvent value = keyValuePair.Value;
				ClockDistribution.ClockInfo clockInfo = null;
				if (!dictionary.TryGetValue(value.Clock, out clockInfo))
				{
					clockInfo = new ClockDistribution.ClockInfo();
					dictionary[value.Clock] = clockInfo;
				}
				else
				{
					clockInfo.UpdateTimePerFreq(key);
				}
				clockInfo.lastTimeStamp = key;
				switch (value.Event)
				{
				case ClockBase.ClockEvent.EventType.Enable:
					clockInfo.Enabled = true;
					break;
				case ClockBase.ClockEvent.EventType.Disable:
					clockInfo.Enabled = false;
					break;
				case ClockBase.ClockEvent.EventType.SetRate:
					clockInfo.CurrentFrequency = value.Frequency;
					break;
				}
			}
			long num = combinedEvents.Keys[combinedEvents.Count - 1];
			foreach (KeyValuePair<string, ClockDistribution.ClockInfo> keyValuePair2 in dictionary)
			{
				string key2 = keyValuePair2.Key;
				ClockDistribution.ClockInfo value2 = keyValuePair2.Value;
				value2.UpdateTimePerFreq(num);
			}
			return dictionary;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002A48 File Offset: 0x00000C48
		private TreeModel BuildTreeModel(Dictionary<string, ClockDistribution.ClockInfo> clockNodeDict)
		{
			Type[] array = new Type[3];
			string[] array2 = new string[3];
			array[0] = typeof(string);
			array2[0] = "Clock";
			array[1] = typeof(uint);
			array2[1] = "Apps Vote";
			array[2] = typeof(double);
			array2[2] = "% Duration";
			TreeModel treeModel = new TreeModel(array);
			treeModel.ColumnNames = array2;
			treeModel.ColumnTypes = array;
			foreach (KeyValuePair<string, ClockDistribution.ClockInfo> keyValuePair in clockNodeDict)
			{
				string key = keyValuePair.Key;
				ClockDistribution.ClockInfo value = keyValuePair.Value;
				TreeNode treeNode = new TreeNode();
				treeNode.Values = new object[3];
				treeNode.Values[0] = key;
				treeModel.Nodes.Add(treeNode);
				foreach (KeyValuePair<uint, long> keyValuePair2 in value.TimePerFrequency)
				{
					uint key2 = keyValuePair2.Key;
					long value2 = keyValuePair2.Value;
					long num = value.TimePerFrequency[uint.MaxValue];
					if (key2 != 4294967295U)
					{
						TreeNode treeNode2 = new TreeNode();
						treeNode2.Values = new object[3];
						treeNode2.Values[0] = "";
						treeNode2.Values[1] = key2;
						treeNode2.Values[2] = (double)value2 / (double)num * 100.0;
						treeNode.Children.Add(treeNode2);
					}
				}
			}
			return treeModel;
		}

		// Token: 0x04000002 RID: 2
		private Dictionary<int, TreeViewStatisticDisplayViewModel> m_viewModels = new Dictionary<int, TreeViewStatisticDisplayViewModel>();

		// Token: 0x0200001F RID: 31
		private class ClockInfo
		{
			// Token: 0x060000C7 RID: 199 RVA: 0x0000A520 File Offset: 0x00008720
			public void UpdateTimePerFreq(long currentTimeStamp)
			{
				uint num = (this.Enabled ? this.CurrentFrequency : 0U);
				long num2 = currentTimeStamp - this.lastTimeStamp;
				long num3 = 0L;
				this.TimePerFrequency.TryGetValue(num, out num3);
				this.TimePerFrequency[num] = num3 + num2;
				long num4 = 0L;
				this.TimePerFrequency.TryGetValue(uint.MaxValue, out num4);
				this.TimePerFrequency[uint.MaxValue] = num4 + num2;
			}

			// Token: 0x040000A5 RID: 165
			public bool Enabled = true;

			// Token: 0x040000A6 RID: 166
			public uint CurrentFrequency;

			// Token: 0x040000A7 RID: 167
			public long lastTimeStamp;

			// Token: 0x040000A8 RID: 168
			public Dictionary<uint, long> TimePerFrequency = new Dictionary<uint, long>();

			// Token: 0x040000A9 RID: 169
			public const uint TOTAL = 4294967295U;
		}
	}
}
