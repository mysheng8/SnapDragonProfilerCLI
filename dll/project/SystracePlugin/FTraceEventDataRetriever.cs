using System;
using System.Collections.Generic;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000006 RID: 6
	public class FTraceEventDataRetriever
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002EE0 File Offset: 0x000010E0
		public static ModelObjectDataList GetEventData(string eventName, int captureID)
		{
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 9:
				{
					char c = eventName[0];
					if (c != 'M')
					{
						if (c != 'S')
						{
							goto IL_01E7;
						}
						if (!(eventName == "Sched CPU"))
						{
							goto IL_01E7;
						}
					}
					else
					{
						if (!(eventName == "MdpCommit"))
						{
							goto IL_01E7;
						}
						goto IL_01DA;
					}
					break;
				}
				case 10:
				case 16:
				case 17:
				case 19:
				case 20:
				case 21:
					goto IL_01E7;
				case 11:
				{
					char c = eventName[0];
					if (c != 'C')
					{
						if (c != 'S')
						{
							goto IL_01E7;
						}
						if (!(eventName == "SoftirqExit"))
						{
							goto IL_01E7;
						}
						goto IL_01DA;
					}
					else
					{
						if (!(eventName == "ClockEnable"))
						{
							goto IL_01E7;
						}
						goto IL_01DA;
					}
					break;
				}
				case 12:
				{
					char c = eventName[7];
					if (c != 'E')
					{
						if (c != 'R')
						{
							if (c != 's')
							{
								goto IL_01E7;
							}
							if (!(eventName == "ClockDisable"))
							{
								goto IL_01E7;
							}
							goto IL_01DA;
						}
						else
						{
							if (!(eventName == "SoftirqRaise"))
							{
								goto IL_01E7;
							}
							goto IL_01DA;
						}
					}
					else
					{
						if (!(eventName == "SoftirqEntry"))
						{
							goto IL_01E7;
						}
						goto IL_01DA;
					}
					break;
				}
				case 13:
					if (!(eventName == "MdpSsppChange"))
					{
						goto IL_01E7;
					}
					goto IL_01DA;
				case 14:
					if (!(eventName == "IrqHandlerExit"))
					{
						goto IL_01E7;
					}
					goto IL_01DA;
				case 15:
				{
					char c = eventName[0];
					if (c != 'I')
					{
						if (c != 'K')
						{
							if (c != 'S')
							{
								goto IL_01E7;
							}
							if (!(eventName == "SchedEnqDeqTask"))
							{
								goto IL_01E7;
							}
							goto IL_01DA;
						}
						else
						{
							if (!(eventName == "KgslPwrSetState"))
							{
								goto IL_01E7;
							}
							goto IL_01DA;
						}
					}
					else
					{
						if (!(eventName == "IrqHandlerEntry"))
						{
							goto IL_01E7;
						}
						goto IL_01DA;
					}
					break;
				}
				case 18:
					if (!(eventName == "WorkqueueQueueWork"))
					{
						goto IL_01E7;
					}
					goto IL_01DA;
				case 22:
					if (!(eventName == "Kernel Workqueue Execs"))
					{
						goto IL_01E7;
					}
					break;
				default:
					goto IL_01E7;
				}
				return FTraceEventDataRetriever.GetEventData(eventName, captureID, "tblSystraceGanttElementsTable");
				IL_01DA:
				return FTraceEventDataRetriever.GetEventData(eventName, captureID, "tblSystraceMarkersTable");
			}
			IL_01E7:
			return FTraceEventDataRetriever.GetEventData(eventName, captureID, "tblSystraceGraphSeriesTable");
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000030E0 File Offset: 0x000012E0
		public static FTraceEventDataRetriever.FtraceEventData ParseFtraceMOD(ModelObjectData data)
		{
			FTraceEventDataRetriever.FtraceEventData ftraceEventData = new FTraceEventDataRetriever.FtraceEventData();
			ftraceEventData.Name = data.GetValue("eventName");
			if (string.IsNullOrEmpty(ftraceEventData.Name))
			{
				ftraceEventData.Name = data.GetValue("trackName");
			}
			uint.TryParse(data.GetValue("cpuNum"), out ftraceEventData.CpuNum);
			if (!long.TryParse(data.GetValue("timestamp"), out ftraceEventData.TimeStampBegin))
			{
				long.TryParse(data.GetValue("timestampBegin"), out ftraceEventData.TimeStampBegin);
				long.TryParse(data.GetValue("timestampEnd"), out ftraceEventData.TimeStampEnd);
			}
			ftraceEventData.EventParams = FTraceEventDataRetriever.ParseParamString(data.GetValue("params"));
			return ftraceEventData;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003198 File Offset: 0x00001398
		private static ModelObjectDataList GetEventData(string eventName, int captureID, string dataTableName)
		{
			Model model = SdpApp.ConnectionManager.GetDataModel().GetModel("SystraceModel");
			if (model != null)
			{
				ModelObject modelObject = model.GetModelObject(dataTableName);
				if (modelObject != null)
				{
					StringList stringList = new StringList();
					stringList.Add("captureID");
					stringList.Add(captureID.ToString());
					if (dataTableName == "tblSystraceGanttElementsTable")
					{
						stringList.Add("trackName");
						stringList.Add(eventName);
					}
					else
					{
						stringList.Add("eventName");
						stringList.Add(eventName);
					}
					return modelObject.GetData(stringList);
				}
			}
			return null;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003224 File Offset: 0x00001424
		public static long GetLastTimeStamp(int captureID)
		{
			long lastTimeStamp = FTraceEventDataRetriever.GetLastTimeStamp(captureID, "tblSystraceGraphSeriesTable");
			long lastTimeStamp2 = FTraceEventDataRetriever.GetLastTimeStamp(captureID, "tblSystraceGraphSeriesTable");
			long lastTimeStamp3 = FTraceEventDataRetriever.GetLastTimeStamp(captureID, "tblSystraceGanttElementsTable");
			return Math.Max(Math.Max(lastTimeStamp, lastTimeStamp2), lastTimeStamp3);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003264 File Offset: 0x00001464
		private static long GetLastTimeStamp(int captureID, string dataTableName)
		{
			Model model = SdpApp.ConnectionManager.GetDataModel().GetModel("SystraceModel");
			long num = 0L;
			if (model != null)
			{
				ModelObject modelObject = model.GetModelObject(dataTableName);
				if (modelObject != null)
				{
					ModelObjectDataList data = modelObject.GetData();
					if (data.Count > 0)
					{
						string value = data[data.Count - 1].GetValue("timestamp");
						long.TryParse(value, out num);
					}
				}
			}
			return num;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000032CC File Offset: 0x000014CC
		private static Dictionary<string, string> ParseParamString(string paramString)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = paramString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length == 2)
				{
					dictionary[array3[0]] = array3[1];
				}
			}
			return dictionary;
		}

		// Token: 0x04000004 RID: 4
		public const string SYSTRACE_MODEL_NAME = "SystraceModel";

		// Token: 0x04000005 RID: 5
		public const string SYSTRACE_GRAPH_SERIES_TBL_NAME = "tblSystraceGraphSeriesTable";

		// Token: 0x04000006 RID: 6
		public const string SYSTRACE_MARKERS_TBL_NAME = "tblSystraceMarkersTable";

		// Token: 0x04000007 RID: 7
		public const string SYSTRACE_GANTT_ELEMENTS_TBL_NAME = "tblSystraceGanttElementsTable";

		// Token: 0x04000008 RID: 8
		public const string CPU_SCHEDULING = "Sched CPU";

		// Token: 0x04000009 RID: 9
		public const string CPU_FREQUENCY = "CpuFrequency";

		// Token: 0x0400000A RID: 10
		public const string SCHED_ENQ_DES_TASK = "SchedEnqDeqTask";

		// Token: 0x0400000B RID: 11
		public const string WORKQUEUE_ACTIVATE_WORK = "WorkqueuActivateWork";

		// Token: 0x0400000C RID: 12
		public const string WORKQUEUE_EXECUTE = "Kernel Workqueue Execs";

		// Token: 0x0400000D RID: 13
		public const string WORKQUEUE_QUEUE_WORK = "WorkqueueQueueWork";

		// Token: 0x0400000E RID: 14
		public const string IRQ_HANDLER_ENTRY = "IrqHandlerEntry";

		// Token: 0x0400000F RID: 15
		public const string IRQ_HANDLER_EXIT = "IrqHandlerExit";

		// Token: 0x04000010 RID: 16
		public const string SOFTIRQ_HANDLER_ENTRY = "SoftirqEntry";

		// Token: 0x04000011 RID: 17
		public const string SOFTIRQ_HANDLER_EXIT = "SoftirqExit";

		// Token: 0x04000012 RID: 18
		public const string SOFTIRQ_HANDLER_RAISE = "SoftirqRaise";

		// Token: 0x04000013 RID: 19
		public const string KGSL_PWR_SET_STATE = "KgslPwrSetState";

		// Token: 0x04000014 RID: 20
		public const string KGSL_BUSLEVEL = "KgslBuslevel";

		// Token: 0x04000015 RID: 21
		public const string KGSL_PWRLEVEL = "KgslPwrlevel";

		// Token: 0x04000016 RID: 22
		public const string MDP_COMMIT = "MdpCommit";

		// Token: 0x04000017 RID: 23
		public const string MDP_SSPP_CHANGE = "MdpSsppChange";

		// Token: 0x04000018 RID: 24
		public const string CLOCK_ENABLE = "ClockEnable";

		// Token: 0x04000019 RID: 25
		public const string CLOCK_DISABLE = "ClockDisable";

		// Token: 0x0400001A RID: 26
		public const string CLOCK_SET_RATE = "ClockSetRate";

		// Token: 0x0400001B RID: 27
		public const string CAPTURE_ID = "captureID";

		// Token: 0x0400001C RID: 28
		public const string EVENT_NAME = "eventName";

		// Token: 0x0400001D RID: 29
		public const string PARAMS_ATTR = "params";

		// Token: 0x0400001E RID: 30
		public const string TIMESTAMP_ATTR = "timestamp";

		// Token: 0x0400001F RID: 31
		public const string TIMESTAMP_BEGIN_ATTR = "timestampBegin";

		// Token: 0x04000020 RID: 32
		public const string TIMESTAMP_END_ATTR = "timestampEnd";

		// Token: 0x04000021 RID: 33
		public const string CPU_NUM_ATTR = "cpuNum";

		// Token: 0x04000022 RID: 34
		public const string TRACK_NAME = "trackName";

		// Token: 0x02000021 RID: 33
		public class FtraceEventData
		{
			// Token: 0x040000AC RID: 172
			public string Name;

			// Token: 0x040000AD RID: 173
			public uint CpuNum;

			// Token: 0x040000AE RID: 174
			public long TimeStampBegin;

			// Token: 0x040000AF RID: 175
			public long TimeStampEnd;

			// Token: 0x040000B0 RID: 176
			public Dictionary<string, string> EventParams = new Dictionary<string, string>();
		}
	}
}
