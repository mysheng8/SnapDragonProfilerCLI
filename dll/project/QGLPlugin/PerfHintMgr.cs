using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Sdp.Helpers;

namespace QGLPlugin
{
	// Token: 0x02000002 RID: 2
	public static class PerfHintMgr
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static string GetHintName(uint hintID)
		{
			if (PerfHintMgr.m_hintNames.ContainsKey(hintID))
			{
				return PerfHintMgr.m_hintNames[hintID];
			}
			ModelObjectDataList perfHintData = QGLModel.GetPerfHintData(hintID);
			string text = ((perfHintData.Count == 1) ? perfHintData[0].GetValue("m_hintName") : "");
			if (text.Length > 0)
			{
				PerfHintMgr.m_hintNames.Add(hintID, text);
			}
			return text;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B0 File Offset: 0x000002B0
		private static string ReplaceFirst(string text, string search, string replace)
		{
			int num = text.IndexOf(search);
			if (num < 0)
			{
				return text;
			}
			return text.Substring(0, num) + replace + text.Substring(num + search.Length);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E8 File Offset: 0x000002E8
		public static List<PerfHintMgr.PerfHint> GetPerfHintsForAPICall(uint callId, uint captureId)
		{
			List<PerfHintMgr.PerfHint> list = new List<PerfHintMgr.PerfHint>();
			PerfHintMgr.m_apiHints.TryGetValue(Tuple.Create<uint, uint>(callId, captureId), out list);
			return list;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002110 File Offset: 0x00000310
		public static uint GetNumPerfHintsForAPICall(uint callId, uint captureId)
		{
			List<PerfHintMgr.PerfHint> list = new List<PerfHintMgr.PerfHint>();
			PerfHintMgr.m_apiHints.TryGetValue(Tuple.Create<uint, uint>(callId, captureId), out list);
			if (list != null)
			{
				return (uint)list.Count;
			}
			return 0U;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002144 File Offset: 0x00000344
		public static uint ProcessHintMessages(ref ModelObjectDataList hintObjectData, uint callID, uint captureID)
		{
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			PerfHintMgr.PerfHint perfHint = new PerfHintMgr.PerfHint();
			List<PerfHintMgr.Parameter> list = new List<PerfHintMgr.Parameter>();
			List<PerfHintMgr.PerfHint> list2 = new List<PerfHintMgr.PerfHint>();
			foreach (ModelObjectData modelObjectData in hintObjectData)
			{
				uint num4 = UintConverter.Convert(modelObjectData.GetValue("m_hintID"));
				uint num5 = UintConverter.Convert(modelObjectData.GetValue("m_hintInstance"));
				uint num6 = UintConverter.Convert(modelObjectData.GetValue("m_msgIdx"));
				uint num7 = UintConverter.Convert(modelObjectData.GetValue("m_paramIdx"));
				PerfHintMgr.HintKey hintKey = new PerfHintMgr.HintKey(num4, num6);
				PerfHintMgr.HintMsgData hintMsgData = new PerfHintMgr.HintMsgData();
				if ((long)num3 != (long)((ulong)num6) || (long)num2 != (long)((ulong)num5))
				{
					if (num3 != -1 && num2 != -1)
					{
						perfHint.AddMessageLine(new PerfHintMgr.HintKey((uint)num, (uint)num3), list);
						list = new List<PerfHintMgr.Parameter>();
					}
					num3 = (int)num6;
				}
				if ((long)num2 != (long)((ulong)num5))
				{
					if (num2 != -1)
					{
						perfHint.SetHintID((uint)num);
						list2.Add(perfHint);
						perfHint = new PerfHintMgr.PerfHint();
					}
					num2 = (int)num5;
					num = (int)num4;
				}
				if (PerfHintMgr.m_hintMsgCache.TryGetValue(hintKey.hintKey, out hintMsgData))
				{
					int count = hintMsgData.ParamData.Count;
				}
				else
				{
					ModelObjectDataList perfHintMessages = QGLModel.GetPerfHintMessages(num4);
					foreach (ModelObjectData modelObjectData2 in perfHintMessages)
					{
						string text = modelObjectData2.GetValue("m_msgFormat");
						int num8 = IntConverter.Convert(modelObjectData2.GetValue("m_numParams"));
						uint num9 = UintConverter.Convert(modelObjectData2.GetValue("m_msgIdx"));
						ModelObjectDataList perfHintMessagesParams = QGLModel.GetPerfHintMessagesParams(num4, num9);
						PerfHintMgr.HintKey hintKey2 = new PerfHintMgr.HintKey(num4, num9);
						List<Tuple<uint, uint>> list3 = new List<Tuple<uint, uint>>();
						foreach (string text2 in PerfHintMgr.ParamFormatList)
						{
							text = text.Replace(text2, PerfHintMgr.StringMarker);
						}
						foreach (ModelObjectData modelObjectData3 in perfHintMessagesParams)
						{
							uint num10 = UintConverter.Convert(modelObjectData3.GetValue("m_paramType"));
							uint num11 = UintConverter.Convert(modelObjectData3.GetValue("m_paramInfo"));
							list3.Add(Tuple.Create<uint, uint>(num10, num11));
						}
						PerfHintMgr.HintMsgData hintMsgData2 = new PerfHintMgr.HintMsgData(text, ref list3);
						PerfHintMgr.m_hintMsgCache[hintKey2.hintKey] = hintMsgData2;
						if (num6 == num9)
						{
							hintMsgData = hintMsgData2;
						}
					}
				}
				if (hintMsgData == null)
				{
					return 0U;
				}
				if ((ulong)num7 < (ulong)((long)hintMsgData.ParamData.Count))
				{
					uint item = hintMsgData.ParamData[(int)num7].Item1;
					uint item2 = hintMsgData.ParamData[(int)num7].Item2;
					if (item == 10U)
					{
						PerfHintMgr.Parameter parameter = new PerfHintMgr.Parameter((PerfHintMgr.VkalParamType)item, (PerfHintMgr.VkalParamInfo)item2, modelObjectData.GetValue("m_valueString"));
						list.Add(parameter);
					}
					else
					{
						PerfHintMgr.Parameter parameter2 = new PerfHintMgr.Parameter((PerfHintMgr.VkalParamType)item, (PerfHintMgr.VkalParamInfo)item2, Uint64Converter.Convert(modelObjectData.GetValue("m_valueUint")));
						list.Add(parameter2);
					}
				}
			}
			perfHint.SetHintID((uint)num);
			perfHint.AddMessageLine(new PerfHintMgr.HintKey((uint)num, (uint)num3), list);
			list2.Add(perfHint);
			PerfHintMgr.m_apiHints.Add(Tuple.Create<uint, uint>(callID, captureID), list2);
			return (uint)list2.Count;
		}

		// Token: 0x04000001 RID: 1
		public static string PerfHintKey = "Performance Hint";

		// Token: 0x04000002 RID: 2
		private static Dictionary<uint, string> m_hintNames = new Dictionary<uint, string>();

		// Token: 0x04000003 RID: 3
		private static string StringMarker = "{0}";

		// Token: 0x04000004 RID: 4
		private static Dictionary<ulong, PerfHintMgr.HintMsgData> m_hintMsgCache = new Dictionary<ulong, PerfHintMgr.HintMsgData>();

		// Token: 0x04000005 RID: 5
		private static Dictionary<Tuple<uint, uint>, List<PerfHintMgr.PerfHint>> m_apiHints = new Dictionary<Tuple<uint, uint>, List<PerfHintMgr.PerfHint>>();

		// Token: 0x04000006 RID: 6
		private static readonly List<string> ParamFormatList = new List<string> { "%s", "%i", "%d", "%u", "%X", "%p", "%llx", "%zu" };

		// Token: 0x04000007 RID: 7
		public static readonly HashSet<int> PerfHintsProcessed = new HashSet<int>();

		// Token: 0x02000046 RID: 70
		public enum VkalParamType
		{
			// Token: 0x04000400 RID: 1024
			VkalParamTypeUnknown,
			// Token: 0x04000401 RID: 1025
			VkalParamTypeUint8,
			// Token: 0x04000402 RID: 1026
			VkalParamTypeUint16,
			// Token: 0x04000403 RID: 1027
			VkalParamTypeUint32,
			// Token: 0x04000404 RID: 1028
			VkalParamTypeUint64,
			// Token: 0x04000405 RID: 1029
			VkalParamTypeInt8,
			// Token: 0x04000406 RID: 1030
			VkalParamTypeInt16,
			// Token: 0x04000407 RID: 1031
			VkalParamTypeInt32,
			// Token: 0x04000408 RID: 1032
			VkalParamTypeInt64,
			// Token: 0x04000409 RID: 1033
			VkalParamTypeChar,
			// Token: 0x0400040A RID: 1034
			VkalParamTypeString
		}

		// Token: 0x02000047 RID: 71
		public enum VkalParamInfo
		{
			// Token: 0x0400040C RID: 1036
			VkalParamInfoUnknown,
			// Token: 0x0400040D RID: 1037
			VkalParamInfoInstanceHandle,
			// Token: 0x0400040E RID: 1038
			VkalParamInfoPhysicalDeviceHandle,
			// Token: 0x0400040F RID: 1039
			VkalParamInfoDeviceHandle,
			// Token: 0x04000410 RID: 1040
			VkalParamInfoCommandBufferHandle,
			// Token: 0x04000411 RID: 1041
			VkalParamInfoQueueHandle,
			// Token: 0x04000412 RID: 1042
			VkalParamInfoSemaphoreHandle,
			// Token: 0x04000413 RID: 1043
			VkalParamInfoFenceHandle,
			// Token: 0x04000414 RID: 1044
			VkalParamInfoQueryPoolHandle,
			// Token: 0x04000415 RID: 1045
			VkalParamInfoBufferViewHandle,
			// Token: 0x04000416 RID: 1046
			VkalParamInfoDeviceMemoryHandle,
			// Token: 0x04000417 RID: 1047
			VkalParamInfoBufferHandle,
			// Token: 0x04000418 RID: 1048
			VkalParamInfoImageHandle,
			// Token: 0x04000419 RID: 1049
			VkalParamInfoPipelineHandle,
			// Token: 0x0400041A RID: 1050
			VkalParamInfoShaderModuleHandle,
			// Token: 0x0400041B RID: 1051
			VkalParamInfoSamplerHandle,
			// Token: 0x0400041C RID: 1052
			VkalParamInfoRenderPassHandle,
			// Token: 0x0400041D RID: 1053
			VkalParamInfoDescriptorPoolHandle,
			// Token: 0x0400041E RID: 1054
			VkalParamInfoDescriptorSetLayoutHandle,
			// Token: 0x0400041F RID: 1055
			VkalParamInfoFramebufferHandle,
			// Token: 0x04000420 RID: 1056
			VkalParamInfoPipelineCacheHandle,
			// Token: 0x04000421 RID: 1057
			VkalParamInfoCommandPoolHandle,
			// Token: 0x04000422 RID: 1058
			VkalParamInfoDescriptorSetHandle,
			// Token: 0x04000423 RID: 1059
			VkalParamInfoEventHandle,
			// Token: 0x04000424 RID: 1060
			VkalParamInfoPipelineLayoutHandle,
			// Token: 0x04000425 RID: 1061
			VkalParamInfoImageView,
			// Token: 0x04000426 RID: 1062
			VkalParamInfoSubpassIndex,
			// Token: 0x04000427 RID: 1063
			VkalParamInfoInputAttachmentIndex,
			// Token: 0x04000428 RID: 1064
			VkalParamInfoColorAttachmentIndex,
			// Token: 0x04000429 RID: 1065
			VkalParamInfoResolveAttachmentIndex,
			// Token: 0x0400042A RID: 1066
			VkalParamInfoDepthStencilAttachmentIndex,
			// Token: 0x0400042B RID: 1067
			VkalParamInfoCommandBufferCommand
		}

		// Token: 0x02000048 RID: 72
		public class Parameter
		{
			// Token: 0x0600015E RID: 350 RVA: 0x0001230C File Offset: 0x0001050C
			public Parameter(PerfHintMgr.VkalParamType t, PerfHintMgr.VkalParamInfo i, ulong value)
			{
				this.m_type = t;
				this.m_info = i;
				this.m_valueUint = value;
				if (this.IsHandle())
				{
					this.m_valueString = string.Format("{0:X4}", this.m_valueUint);
					return;
				}
				this.m_valueString = string.Format("{0:d}", this.m_valueUint);
			}

			// Token: 0x0600015F RID: 351 RVA: 0x00012373 File Offset: 0x00010573
			public Parameter(PerfHintMgr.VkalParamType t, PerfHintMgr.VkalParamInfo i, string value)
			{
				this.m_type = t;
				this.m_info = i;
				this.m_valueUint = 0UL;
				this.m_valueString = value;
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x06000160 RID: 352 RVA: 0x00012398 File Offset: 0x00010598
			public PerfHintMgr.VkalParamType Type
			{
				get
				{
					return this.m_type;
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x06000161 RID: 353 RVA: 0x000123A0 File Offset: 0x000105A0
			public PerfHintMgr.VkalParamInfo Info
			{
				get
				{
					return this.m_info;
				}
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x06000162 RID: 354 RVA: 0x000123A8 File Offset: 0x000105A8
			public string ValueString
			{
				get
				{
					return this.m_valueString;
				}
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x06000163 RID: 355 RVA: 0x000123B0 File Offset: 0x000105B0
			public ulong ValueUint
			{
				get
				{
					return this.m_valueUint;
				}
			}

			// Token: 0x06000164 RID: 356 RVA: 0x000123B8 File Offset: 0x000105B8
			public bool IsString()
			{
				return this.m_type == PerfHintMgr.VkalParamType.VkalParamTypeString;
			}

			// Token: 0x06000165 RID: 357 RVA: 0x000123C4 File Offset: 0x000105C4
			public bool IsInt()
			{
				return this.m_type >= PerfHintMgr.VkalParamType.VkalParamTypeUint8 && this.m_type <= PerfHintMgr.VkalParamType.VkalParamTypeInt64;
			}

			// Token: 0x06000166 RID: 358 RVA: 0x000123DD File Offset: 0x000105DD
			public bool IsHandle()
			{
				return this.m_info >= PerfHintMgr.VkalParamInfo.VkalParamInfoInstanceHandle && this.m_info <= PerfHintMgr.VkalParamInfo.VkalParamInfoPipelineLayoutHandle;
			}

			// Token: 0x0400042C RID: 1068
			private PerfHintMgr.VkalParamType m_type;

			// Token: 0x0400042D RID: 1069
			private PerfHintMgr.VkalParamInfo m_info;

			// Token: 0x0400042E RID: 1070
			private ulong m_valueUint;

			// Token: 0x0400042F RID: 1071
			private string m_valueString;
		}

		// Token: 0x02000049 RID: 73
		public class PerfHint
		{
			// Token: 0x06000167 RID: 359 RVA: 0x000123F7 File Offset: 0x000105F7
			public void AddMessageLine(PerfHintMgr.HintKey msgKey, List<PerfHintMgr.Parameter> parms)
			{
				this.m_messageLines.Add(Tuple.Create<ulong, List<PerfHintMgr.Parameter>>(msgKey.hintKey, parms));
			}

			// Token: 0x06000168 RID: 360 RVA: 0x00012410 File Offset: 0x00010610
			public string GetMessage()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Tuple<ulong, List<PerfHintMgr.Parameter>> tuple in this.m_messageLines)
				{
					PerfHintMgr.HintMsgData hintMsgData;
					if (PerfHintMgr.m_hintMsgCache.TryGetValue(tuple.Item1, out hintMsgData))
					{
						string text = hintMsgData.MsgFormat;
						foreach (PerfHintMgr.Parameter parameter in tuple.Item2)
						{
							string valueString = parameter.ValueString;
							text = PerfHintMgr.ReplaceFirst(text, PerfHintMgr.StringMarker, valueString);
						}
						stringBuilder.Append(text + Environment.NewLine);
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000169 RID: 361 RVA: 0x000124F4 File Offset: 0x000106F4
			public void SetHintID(uint id)
			{
				this.m_hintID = id;
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x0600016A RID: 362 RVA: 0x000124FD File Offset: 0x000106FD
			public uint HintID
			{
				get
				{
					return this.m_hintID;
				}
			}

			// Token: 0x04000430 RID: 1072
			private uint m_hintID;

			// Token: 0x04000431 RID: 1073
			private List<Tuple<ulong, List<PerfHintMgr.Parameter>>> m_messageLines = new List<Tuple<ulong, List<PerfHintMgr.Parameter>>>();
		}

		// Token: 0x0200004A RID: 74
		private class HintMsgData
		{
			// Token: 0x0600016C RID: 364 RVA: 0x000025B7 File Offset: 0x000007B7
			public HintMsgData()
			{
			}

			// Token: 0x0600016D RID: 365 RVA: 0x00012518 File Offset: 0x00010718
			public HintMsgData(string msg, ref List<Tuple<uint, uint>> parmData)
			{
				this.m_msgFormat = msg;
				this.m_paramData = parmData;
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x0600016E RID: 366 RVA: 0x0001252F File Offset: 0x0001072F
			public string MsgFormat
			{
				get
				{
					return this.m_msgFormat;
				}
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x0600016F RID: 367 RVA: 0x00012537 File Offset: 0x00010737
			public List<Tuple<uint, uint>> ParamData
			{
				get
				{
					return this.m_paramData;
				}
			}

			// Token: 0x04000432 RID: 1074
			private string m_msgFormat;

			// Token: 0x04000433 RID: 1075
			private List<Tuple<uint, uint>> m_paramData;
		}

		// Token: 0x0200004B RID: 75
		[StructLayout(LayoutKind.Explicit)]
		public struct HintKey
		{
			// Token: 0x06000170 RID: 368 RVA: 0x0001253F File Offset: 0x0001073F
			public HintKey(uint id, uint idx)
			{
				this.hintKey = 0UL;
				this.hintID = id;
				this.msgIdx = idx;
			}

			// Token: 0x04000434 RID: 1076
			[FieldOffset(0)]
			public uint hintID;

			// Token: 0x04000435 RID: 1077
			[FieldOffset(4)]
			public uint msgIdx;

			// Token: 0x04000436 RID: 1078
			[FieldOffset(0)]
			public ulong hintKey;
		}
	}
}
