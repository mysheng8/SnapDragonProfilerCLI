using System;
using System.Collections.Generic;
using Sdp;
using Sdp.Functional;
using Sdp.Helpers;

namespace QGLPlugin.ModelObjectGateways.QGLApiQueueSubmitTimings
{
	// Token: 0x02000044 RID: 68
	internal class QGLApiTraceQueueSubmitTimingGateway
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00012254 File Offset: 0x00010454
		internal static Result<IEnumerable<IQGLApiQueueSubmitTimingProxy>, string> GetQGLApiTraceQueueSubmitTiming(int captureID)
		{
			StringList stringList = new StringList
			{
				"m_captureID",
				captureID.ToString()
			};
			return QGLApiTraceQueueSubmitTimingGateway.GetQGLApiTraceQueueSubmitTiming(stringList);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00012288 File Offset: 0x00010488
		private static Result<IEnumerable<IQGLApiQueueSubmitTimingProxy>, string> GetQGLApiTraceQueueSubmitTiming(StringList stringList)
		{
			QGLApiTraceQueueSubmitTimingGateway.QGLApiTraceQueueSubmitTimingGatewayImpl qglapiTraceQueueSubmitTimingGatewayImpl = new QGLApiTraceQueueSubmitTimingGateway.QGLApiTraceQueueSubmitTimingGatewayImpl(stringList);
			if (qglapiTraceQueueSubmitTimingGatewayImpl.IsValid())
			{
				return new Result<IEnumerable<IQGLApiQueueSubmitTimingProxy>, string>.Success(qglapiTraceQueueSubmitTimingGatewayImpl);
			}
			return new Result<IEnumerable<IQGLApiQueueSubmitTimingProxy>, string>.Error("Unable to retrieve qgl api trace queue submit table for query: " + stringList.AsString());
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000122C0 File Offset: 0x000104C0
		internal static Result<IEnumerable<IQGLApiQueueSubmitTimingProxy>, string> GetQGLApiTraceQueueSubmitTiming(int captureID, uint callID)
		{
			StringList stringList = new StringList
			{
				"m_captureID",
				captureID.ToString(),
				"m_callID",
				callID.ToString()
			};
			return QGLApiTraceQueueSubmitTimingGateway.GetQGLApiTraceQueueSubmitTiming(stringList);
		}

		// Token: 0x0200007F RID: 127
		private class QGLApiTraceQueueSubmitTimingGatewayImpl : MODGatewayList<IQGLApiQueueSubmitTimingProxy, QGLApiTraceQueueSubmitTimingGateway.QGLApiTraceQueueSubmitTimingGatewayImpl>, IQGLApiQueueSubmitTimingProxy
		{
			// Token: 0x06000249 RID: 585 RVA: 0x0001D909 File Offset: 0x0001BB09
			public QGLApiTraceQueueSubmitTimingGatewayImpl(StringList searchString)
				: base(searchString, "QGLModel", "QGLApiTraceQueueSubmitTiming")
			{
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x0600024A RID: 586 RVA: 0x0001D91C File Offset: 0x0001BB1C
			public Result<uint, string> LoggingID
			{
				get
				{
					return base.TryGetUIntValue("m_loggingID");
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x0600024B RID: 587 RVA: 0x0001D929 File Offset: 0x0001BB29
			public Result<uint, string> CaptureID
			{
				get
				{
					return base.TryGetUIntValue("m_captureID");
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x0600024C RID: 588 RVA: 0x0001D936 File Offset: 0x0001BB36
			public Result<uint, string> InstanceID
			{
				get
				{
					return base.TryGetUIntValue("m_instanceID");
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x0600024D RID: 589 RVA: 0x0001D943 File Offset: 0x0001BB43
			public Result<uint, string> CallID
			{
				get
				{
					return base.TryGetUIntValue("m_callID");
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x0600024E RID: 590 RVA: 0x0001D950 File Offset: 0x0001BB50
			public Result<uint, string> Index
			{
				get
				{
					return base.TryGetUIntValue("m_index");
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x0600024F RID: 591 RVA: 0x0001D95D File Offset: 0x0001BB5D
			public Result<ulong, string> CommandBuffer
			{
				get
				{
					return base.TryGetULongValue("m_commandBuffer");
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000250 RID: 592 RVA: 0x0001D96A File Offset: 0x0001BB6A
			public Result<ulong, string> FunctionGPUStartTimeNS
			{
				get
				{
					return base.TryGetULongValue("m_functionGPUStartTimeNS");
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000251 RID: 593 RVA: 0x0001D977 File Offset: 0x0001BB77
			public Result<ulong, string> FunctionGPUEndTimeNS
			{
				get
				{
					return base.TryGetULongValue("m_functionGPUEndTimeNS");
				}
			}

			// Token: 0x06000252 RID: 594 RVA: 0x0001D984 File Offset: 0x0001BB84
			public Result<QGLApiQueueSubmitTiming, string> ToDomain()
			{
				return this.LoggingID.Map(this.CaptureID, this.InstanceID, this.CallID, this.Index, this.CommandBuffer, this.FunctionGPUStartTimeNS, this.FunctionGPUEndTimeNS, (uint loggingID, uint captureID, uint instanceID, uint callID, uint index, ulong commandBuffer, ulong functionGPUStartTimeNS, ulong functionGPUEndTimeNS) => new QGLApiQueueSubmitTiming(loggingID, captureID, instanceID, callID, index, commandBuffer, functionGPUStartTimeNS, functionGPUEndTimeNS));
			}
		}

		// Token: 0x02000080 RID: 128
		private static class ColumnNames
		{
			// Token: 0x04000568 RID: 1384
			internal const string LoggingID = "m_loggingID";

			// Token: 0x04000569 RID: 1385
			internal const string CaptureID = "m_captureID";

			// Token: 0x0400056A RID: 1386
			internal const string InstanceID = "m_instanceID";

			// Token: 0x0400056B RID: 1387
			internal const string CallID = "m_callID";

			// Token: 0x0400056C RID: 1388
			internal const string Index = "m_index";

			// Token: 0x0400056D RID: 1389
			internal const string CommandBuffer = "m_commandBuffer";

			// Token: 0x0400056E RID: 1390
			internal const string FunctionGPUStartTimeNS = "m_functionGPUStartTimeNS";

			// Token: 0x0400056F RID: 1391
			internal const string FunctionGPUEndTimeNS = "m_functionGPUEndTimeNS";
		}
	}
}
