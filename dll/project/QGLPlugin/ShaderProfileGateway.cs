using System;
using System.Collections.Generic;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000029 RID: 41
	internal class ShaderProfileGateway
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00006910 File Offset: 0x00004B10
		internal static List<uint> GetShaderProfileStages(uint captureID, uint drawID)
		{
			List<uint> list = new List<uint>();
			StringList stringList = new StringList
			{
				"CaptureID",
				captureID.ToString(),
				"DrawID",
				drawID.ToString()
			};
			IEnumerable<IShaderProfile> shaderProfiles = ShaderProfileGateway.GetShaderProfiles(stringList);
			if (shaderProfiles == null)
			{
				return list;
			}
			foreach (IShaderProfile shaderProfile in shaderProfiles)
			{
				if (!list.Contains(shaderProfile.Stage))
				{
					list.Add(shaderProfile.Stage);
				}
			}
			return list;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000069B8 File Offset: 0x00004BB8
		internal static IEnumerable<IShaderProfile> GetShaderProfiles(uint captureID, uint drawID, uint stage)
		{
			StringList stringList = new StringList
			{
				"CaptureID",
				captureID.ToString(),
				"DrawID",
				drawID.ToString(),
				"Stage",
				stage.ToString()
			};
			return ShaderProfileGateway.GetShaderProfiles(stringList);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006A1C File Offset: 0x00004C1C
		private static IEnumerable<IShaderProfile> GetShaderProfiles(StringList stringList)
		{
			ShaderProfileGateway.ShaderProfileListImpl shaderProfileListImpl = new ShaderProfileGateway.ShaderProfileListImpl(stringList);
			if (!shaderProfileListImpl.IsValid())
			{
				return null;
			}
			return shaderProfileListImpl;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006A3C File Offset: 0x00004C3C
		internal static IShaderProfile GetShaderProfile(uint captureID, uint drawID, uint stage)
		{
			StringList stringList = new StringList
			{
				"CaptureID",
				captureID.ToString(),
				"DrawID",
				drawID.ToString(),
				"Stage",
				stage.ToString()
			};
			ShaderProfileGateway.ShaderProfileListImpl shaderProfileListImpl = new ShaderProfileGateway.ShaderProfileListImpl(stringList);
			if (shaderProfileListImpl.IsValid())
			{
				return shaderProfileListImpl.GetValue(0);
			}
			return null;
		}

		// Token: 0x02000056 RID: 86
		private class ShaderProfileListImpl : MODGatewayList<IShaderProfile, ShaderProfileGateway.ShaderProfileListImpl>, IShaderProfile
		{
			// Token: 0x0600019C RID: 412 RVA: 0x00013E9A File Offset: 0x0001209A
			public ShaderProfileListImpl(StringList searchString)
				: base(searchString, "GLESModel", "SCOPEShaderProfiles")
			{
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x0600019D RID: 413 RVA: 0x00013EAD File Offset: 0x000120AD
			public uint CaptureID
			{
				get
				{
					return base.GetUIntValue("CaptureID");
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x0600019E RID: 414 RVA: 0x00013EBA File Offset: 0x000120BA
			public uint DrawID
			{
				get
				{
					return base.GetUIntValue("DrawID");
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600019F RID: 415 RVA: 0x00013EC7 File Offset: 0x000120C7
			public uint Stage
			{
				get
				{
					return base.GetUIntValue("Stage");
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x060001A0 RID: 416 RVA: 0x00013ED4 File Offset: 0x000120D4
			public uint InstructionIndex
			{
				get
				{
					return base.GetUIntValue("InstructionIndex");
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x060001A1 RID: 417 RVA: 0x00013EE1 File Offset: 0x000120E1
			public uint SourceMapType
			{
				get
				{
					return base.GetUIntValue("SourceMapType");
				}
			}

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x00013EEE File Offset: 0x000120EE
			public uint SourceLineNumber
			{
				get
				{
					return base.GetUIntValue("SourceLineNumber");
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060001A3 RID: 419 RVA: 0x00013EFB File Offset: 0x000120FB
			public uint CounterType
			{
				get
				{
					return base.GetUIntValue("CounterType");
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060001A4 RID: 420 RVA: 0x00013F08 File Offset: 0x00012108
			public ulong Counter
			{
				get
				{
					return base.GetULongValue("Counter");
				}
			}
		}

		// Token: 0x02000057 RID: 87
		private static class ColumnNames
		{
			// Token: 0x04000466 RID: 1126
			internal const string CaptureID = "CaptureID";

			// Token: 0x04000467 RID: 1127
			internal const string DrawID = "DrawID";

			// Token: 0x04000468 RID: 1128
			internal const string Stage = "Stage";

			// Token: 0x04000469 RID: 1129
			internal const string InstructionIndex = "InstructionIndex";

			// Token: 0x0400046A RID: 1130
			internal const string SourceMapType = "SourceMapType";

			// Token: 0x0400046B RID: 1131
			internal const string SourceLineNumber = "SourceLineNumber";

			// Token: 0x0400046C RID: 1132
			internal const string CounterType = "CounterType";

			// Token: 0x0400046D RID: 1133
			internal const string Counter = "Counter";
		}
	}
}
