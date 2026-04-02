using System;
using System.Collections.Generic;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000027 RID: 39
	internal class ShaderModuleGateway
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000067A0 File Offset: 0x000049A0
		internal static IEnumerable<IShaderStage> GetShaderModules(int captureID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString()
			};
			return ShaderModuleGateway.GetShaderStages(stringList, "VulkanSnapshot", "VulkanSnapshotShaderStages");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000067DC File Offset: 0x000049DC
		internal static IEnumerable<IShaderStage> GetShaderStages(int captureID, ulong pipelineID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString(),
				"pipelineID",
				pipelineID.ToString()
			};
			return ShaderModuleGateway.GetShaderStages(stringList, "VulkanSnapshot", "VulkanSnapshotShaderStages");
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006830 File Offset: 0x00004A30
		internal static IEnumerable<IShaderStage> GetTraceShaderStages(int captureID, long pipelineID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString(),
				"pipelineID",
				pipelineID.ToString()
			};
			return ShaderModuleGateway.GetShaderStages(stringList, "QGLModel", "VulkanTraceShaderData");
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00006884 File Offset: 0x00004A84
		private static IEnumerable<IShaderStage> GetShaderStages(StringList searchString, string modelName, string tableName)
		{
			ShaderModuleGateway.ShaderStageListImpl shaderStageListImpl = new ShaderModuleGateway.ShaderStageListImpl(searchString, modelName, tableName);
			if (!shaderStageListImpl.IsValid())
			{
				return null;
			}
			return shaderStageListImpl;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000068A8 File Offset: 0x00004AA8
		internal static IShaderStage GetShaderStage(int captureID, ulong shaderModuleID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString(),
				"shaderModuleID",
				shaderModuleID.ToString()
			};
			ShaderModuleGateway.ShaderStageListImpl shaderStageListImpl = new ShaderModuleGateway.ShaderStageListImpl(stringList, "VulkanSnapshot", "VulkanSnapshotShaderStages");
			if (shaderStageListImpl.IsValid())
			{
				return shaderStageListImpl.GetValue(0);
			}
			return null;
		}

		// Token: 0x02000054 RID: 84
		private class ShaderStageListImpl : MODGatewayList<IShaderStage, ShaderModuleGateway.ShaderStageListImpl>, IShaderStage
		{
			// Token: 0x06000194 RID: 404 RVA: 0x00013E34 File Offset: 0x00012034
			public ShaderStageListImpl(StringList searchString, string modelName, string tableName)
				: base(searchString, modelName, tableName)
			{
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x06000195 RID: 405 RVA: 0x00013E3F File Offset: 0x0001203F
			public uint CaptureID
			{
				get
				{
					return base.GetUIntValue("captureID");
				}
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x06000196 RID: 406 RVA: 0x00013E4C File Offset: 0x0001204C
			public ulong PipelineID
			{
				get
				{
					return base.GetULongValue("pipelineID");
				}
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x06000197 RID: 407 RVA: 0x00013E59 File Offset: 0x00012059
			public VkShaderStageFlagBits StageType
			{
				get
				{
					return (VkShaderStageFlagBits)base.GetUIntValue("stageType");
				}
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x06000198 RID: 408 RVA: 0x00013E66 File Offset: 0x00012066
			public uint ShaderStage
			{
				get
				{
					return base.GetUIntValue("shaderStage");
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000199 RID: 409 RVA: 0x00013E73 File Offset: 0x00012073
			public ulong ShaderModuleID
			{
				get
				{
					return base.GetULongValue("shaderModuleID");
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x0600019A RID: 410 RVA: 0x00013E80 File Offset: 0x00012080
			public uint ShaderIndex
			{
				get
				{
					return base.GetUIntValue("shaderIndex");
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x0600019B RID: 411 RVA: 0x00013E8D File Offset: 0x0001208D
			public string PName
			{
				get
				{
					return base.GetStringValue("pName");
				}
			}
		}

		// Token: 0x02000055 RID: 85
		private static class ColumnNames
		{
			// Token: 0x0400045F RID: 1119
			internal const string CaptureID = "captureID";

			// Token: 0x04000460 RID: 1120
			internal const string PipelineID = "pipelineID";

			// Token: 0x04000461 RID: 1121
			internal const string StageType = "stageType";

			// Token: 0x04000462 RID: 1122
			internal const string ShaderStage = "shaderStage";

			// Token: 0x04000463 RID: 1123
			internal const string ShaderModuleID = "shaderModuleID";

			// Token: 0x04000464 RID: 1124
			internal const string ShaderIndex = "shaderIndex";

			// Token: 0x04000465 RID: 1125
			internal const string PName = "pName";
		}
	}
}
