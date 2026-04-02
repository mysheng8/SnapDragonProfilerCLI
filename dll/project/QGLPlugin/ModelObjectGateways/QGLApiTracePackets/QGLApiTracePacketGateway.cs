using System;
using System.Collections.Generic;
using System.Linq;
using Sdp;
using Sdp.Functional;
using Sdp.Helpers;

namespace QGLPlugin.ModelObjectGateways.QGLApiTracePackets
{
	// Token: 0x02000041 RID: 65
	internal class QGLApiTracePacketGateway
	{
		// Token: 0x0600014C RID: 332 RVA: 0x0001212C File Offset: 0x0001032C
		internal static Result<IEnumerable<IQGLApiTracePacketProxy>, string> GetQGLApiTracePacket(int captureID)
		{
			StringList stringList = new StringList
			{
				"m_captureID",
				captureID.ToString()
			};
			return QGLApiTracePacketGateway.GetQGLApiTracePacket(stringList);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00012160 File Offset: 0x00010360
		private static Result<IEnumerable<IQGLApiTracePacketProxy>, string> GetQGLApiTracePacket(StringList stringList)
		{
			QGLApiTracePacketGateway.QGLApiTracePacketImpl qglapiTracePacketImpl = new QGLApiTracePacketGateway.QGLApiTracePacketImpl(stringList);
			if (qglapiTracePacketImpl.IsValid())
			{
				return new Result<IEnumerable<IQGLApiTracePacketProxy>, string>.Success(qglapiTracePacketImpl);
			}
			return new Result<IEnumerable<IQGLApiTracePacketProxy>, string>.Error("Unable to retrieve api trace packets for querry: " + stringList.AsString());
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00012198 File Offset: 0x00010398
		internal static Result<IQGLApiTracePacketProxy, string> GetQGLApiTracePacket(int captureID, uint callID)
		{
			StringList stringList = new StringList
			{
				"m_captureID",
				captureID.ToString(),
				"m_callID",
				callID.ToString()
			};
			return QGLApiTracePacketGateway.GetQGLApiTracePacket(stringList).Bind((IEnumerable<IQGLApiTracePacketProxy> apiPackets) => Enumerable.FirstOrDefault<IQGLApiTracePacketProxy>(apiPackets).ToResult("No trace packets found for querry: " + stringList.AsString()));
		}

		// Token: 0x0200007C RID: 124
		private class QGLApiTracePacketImpl : MODGatewayList<IQGLApiTracePacketProxy, QGLApiTracePacketGateway.QGLApiTracePacketImpl>, IQGLApiTracePacketProxy
		{
			// Token: 0x0600023C RID: 572 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
			public QGLApiTracePacketImpl(StringList searchString)
				: base(searchString, "QGLModel", "QGLApiTracePackets")
			{
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x0600023D RID: 573 RVA: 0x0001D80B File Offset: 0x0001BA0B
			public Result<uint, string> CaptureID
			{
				get
				{
					return base.TryGetUIntValue("m_captureID");
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x0600023E RID: 574 RVA: 0x0001D818 File Offset: 0x0001BA18
			public Result<uint, string> InstanceID
			{
				get
				{
					return base.TryGetUIntValue("m_instanceID");
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x0600023F RID: 575 RVA: 0x0001D825 File Offset: 0x0001BA25
			public Result<uint, string> CallID
			{
				get
				{
					return base.TryGetUIntValue("m_callID");
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000240 RID: 576 RVA: 0x0001D832 File Offset: 0x0001BA32
			public Result<uint, string> ThreadID
			{
				get
				{
					return base.TryGetUIntValue("m_threadID");
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x06000241 RID: 577 RVA: 0x0001D83F File Offset: 0x0001BA3F
			public Result<ulong, string> CPUStartTime
			{
				get
				{
					return base.TryGetULongValue("m_functionCPUStartTime");
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x06000242 RID: 578 RVA: 0x0001D84C File Offset: 0x0001BA4C
			public Result<ulong, string> CPUEndTime
			{
				get
				{
					return base.TryGetULongValue("m_functionCPUEndTime");
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x06000243 RID: 579 RVA: 0x0001D859 File Offset: 0x0001BA59
			public Result<string, string> FunctionName
			{
				get
				{
					return base.TryGetStringValue("m_functionName");
				}
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x06000244 RID: 580 RVA: 0x0001D866 File Offset: 0x0001BA66
			public Result<string, string> FunctionParams
			{
				get
				{
					return base.TryGetStringValue("m_functionParams");
				}
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000245 RID: 581 RVA: 0x0001D873 File Offset: 0x0001BA73
			public Result<string, string> FunctionReturnValue
			{
				get
				{
					return base.TryGetStringValue("m_functionReturnValue");
				}
			}

			// Token: 0x06000246 RID: 582 RVA: 0x0001D880 File Offset: 0x0001BA80
			public Result<QGLApiTracePacket, string> ToDomainObject()
			{
				return this.CaptureID.Map(this.InstanceID, this.CallID, this.ThreadID, this.CPUStartTime, this.CPUEndTime, this.FunctionName, this.FunctionParams, this.FunctionReturnValue, (uint captureID, uint instanceID, uint callID, uint threadID, ulong cpuStartTime, ulong cpuEndTime, string functionName, string functionParams, string functionReturnValue) => new QGLApiTracePacket(captureID, instanceID, callID, threadID, cpuStartTime, cpuEndTime, functionName, functionParams, functionReturnValue));
			}
		}

		// Token: 0x0200007D RID: 125
		private static class ColumnNames
		{
			// Token: 0x0400055E RID: 1374
			internal const string CaptureID = "m_captureID";

			// Token: 0x0400055F RID: 1375
			internal const string InstanceID = "m_instanceID";

			// Token: 0x04000560 RID: 1376
			internal const string CallID = "m_callID";

			// Token: 0x04000561 RID: 1377
			internal const string ThreadID = "m_threadID";

			// Token: 0x04000562 RID: 1378
			internal const string CPUStartTime = "m_functionCPUStartTime";

			// Token: 0x04000563 RID: 1379
			internal const string CPUEndTime = "m_functionCPUEndTime";

			// Token: 0x04000564 RID: 1380
			internal const string FunctionName = "m_functionName";

			// Token: 0x04000565 RID: 1381
			internal const string FunctionParams = "m_functionParams";

			// Token: 0x04000566 RID: 1382
			internal const string FunctionReturnValue = "m_functionReturnValue";
		}
	}
}
