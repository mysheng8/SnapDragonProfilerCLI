using System;
using System.Collections.Generic;

namespace TracePlugin
{
	// Token: 0x02000003 RID: 3
	public class ClockBase
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002664 File Offset: 0x00000864
		protected static SortedList<long, ClockBase.ClockEvent> BuildCombinedEventList(int captureID)
		{
			SortedList<long, ClockBase.ClockEvent> sortedList = new SortedList<long, ClockBase.ClockEvent>(new ClockBase.AllowDuplicateKeys());
			ModelObjectDataList eventData = FTraceEventDataRetriever.GetEventData("ClockEnable", captureID);
			foreach (ModelObjectData modelObjectData in eventData)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData);
				if (ftraceEventData.TimeStampBegin > 0L)
				{
					ClockBase.ClockEvent clockEvent = default(ClockBase.ClockEvent);
					clockEvent.Event = ClockBase.ClockEvent.EventType.Enable;
					string text;
					ftraceEventData.EventParams.TryGetValue("name", out text);
					clockEvent.Clock = text;
					sortedList.Add(ftraceEventData.TimeStampBegin, clockEvent);
				}
			}
			ModelObjectDataList eventData2 = FTraceEventDataRetriever.GetEventData("ClockDisable", captureID);
			foreach (ModelObjectData modelObjectData2 in eventData2)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData2 = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData2);
				if (ftraceEventData2.TimeStampBegin > 0L)
				{
					ClockBase.ClockEvent clockEvent2 = default(ClockBase.ClockEvent);
					clockEvent2.Event = ClockBase.ClockEvent.EventType.Disable;
					string text2;
					ftraceEventData2.EventParams.TryGetValue("name", out text2);
					clockEvent2.Clock = text2;
					sortedList.Add(ftraceEventData2.TimeStampBegin, clockEvent2);
				}
			}
			ModelObjectDataList eventData3 = FTraceEventDataRetriever.GetEventData("ClockSetRate", captureID);
			foreach (ModelObjectData modelObjectData3 in eventData3)
			{
				FTraceEventDataRetriever.FtraceEventData ftraceEventData3 = FTraceEventDataRetriever.ParseFtraceMOD(modelObjectData3);
				if (ftraceEventData3.TimeStampBegin > 0L)
				{
					ClockBase.ClockEvent clockEvent3 = default(ClockBase.ClockEvent);
					clockEvent3.Event = ClockBase.ClockEvent.EventType.SetRate;
					string text3;
					if (ftraceEventData3.EventParams.TryGetValue("name", out text3))
					{
						clockEvent3.Clock = text3;
					}
					string text4 = "";
					uint num = 0U;
					if (ftraceEventData3.EventParams.TryGetValue("state", out text4) && uint.TryParse(text4, out num))
					{
						clockEvent3.Frequency = num;
					}
					sortedList.Add(ftraceEventData3.TimeStampBegin, clockEvent3);
				}
			}
			return sortedList;
		}

		// Token: 0x0200001D RID: 29
		private class AllowDuplicateKeys : IComparer<long>
		{
			// Token: 0x060000C5 RID: 197 RVA: 0x0000A504 File Offset: 0x00008704
			public int Compare(long first, long second)
			{
				int num = first.CompareTo(second);
				if (num != 0)
				{
					return num;
				}
				return 1;
			}
		}

		// Token: 0x0200001E RID: 30
		protected struct ClockEvent
		{
			// Token: 0x040000A2 RID: 162
			public ClockBase.ClockEvent.EventType Event;

			// Token: 0x040000A3 RID: 163
			public string Clock;

			// Token: 0x040000A4 RID: 164
			public uint Frequency;

			// Token: 0x0200002D RID: 45
			public enum EventType
			{
				// Token: 0x040000D6 RID: 214
				Enable,
				// Token: 0x040000D7 RID: 215
				Disable,
				// Token: 0x040000D8 RID: 216
				SetRate
			}
		}
	}
}
