using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x02000039 RID: 57
	internal class ShaderViewMgr
	{
		// Token: 0x0600011B RID: 283 RVA: 0x000101E4 File Offset: 0x0000E3E4
		public ShaderViewMgr()
		{
			ShaderAnalyzerEvents shaderAnalyzerEvents = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents.InvalidateShaderLanguage = (EventHandler<ShaderAnalyzerInvalidateShaderLanguageArgs>)Delegate.Combine(shaderAnalyzerEvents.InvalidateShaderLanguage, new EventHandler<ShaderAnalyzerInvalidateShaderLanguageArgs>(this.shaderAnalyzerEvents_shaderLanguageComboSelected));
			ShaderAnalyzerEvents shaderAnalyzerEvents2 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents2.ConvertShaderSource = (EventHandler<EventArgs>)Delegate.Combine(shaderAnalyzerEvents2.ConvertShaderSource, new EventHandler<EventArgs>(this.shaderAnalyzerEvents_ConvertShaderSource));
			ShaderAnalyzerEvents shaderAnalyzerEvents3 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents3.ExportShader = (EventHandler<ShaderAnalyzerExportShaderArgs>)Delegate.Combine(shaderAnalyzerEvents3.ExportShader, new EventHandler<ShaderAnalyzerExportShaderArgs>(this.shaderAnalyzerEvents_ExportShader));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			this.m_byteBufferGateway = new ByteBufferGateway("VulkanSnapshot", "VulkanSnapshotByteBuffers");
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public void DisplayShaderModule(int captureID, ulong shaderModuleID, uint selectedApi)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs;
			ShaderGroup shaderGroup;
			this.BuildEventArgs(captureID, shaderModuleID, "Shader Module", out shaderAnalyzerProgramEventArgs, out shaderGroup, 353);
			IShaderStage shaderStage = ShaderModuleGateway.GetShaderStage(captureID, shaderModuleID);
			if (shaderStage != null)
			{
				this.m_currentPipelineID = shaderStage.PipelineID;
				this.AddShaderStageToGroup(shaderStage, shaderGroup, 353U, selectedApi, 0U);
				SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.Invalidate, this, shaderAnalyzerProgramEventArgs);
				this.UpdateSnapshotShaderInfo(this.m_currentPipelineID);
				this.UpdateShaderSources();
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00010340 File Offset: 0x0000E540
		public void DisplayPipelineStages(int captureID, ulong pipelineID, uint selectedApi)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs;
			ShaderGroup shaderGroup;
			this.BuildEventArgs(captureID, pipelineID, "Shader Stages For Pipeline", out shaderAnalyzerProgramEventArgs, out shaderGroup, 353);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLibraries");
			HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed(captureID, pipelineID, modelObject);
			object obj = ShaderViewMgr.mutex;
			lock (obj)
			{
				IEnumerable<IShaderStage> shaderStages = ShaderModuleGateway.GetShaderStages(captureID, pipelineID);
				if (shaderStages != null || pipelineLibraryUsed.Count != 0)
				{
					this.m_currentPipelineID = pipelineID;
					uint num = 0U;
					if (shaderStages != null)
					{
						foreach (IShaderStage shaderStage in shaderStages)
						{
							this.AddShaderStageToGroup(shaderStage, shaderGroup, 353U, selectedApi, 0U);
							num += 1U;
						}
					}
					foreach (ulong num2 in pipelineLibraryUsed)
					{
						uint num3 = 0U;
						IEnumerable<IShaderStage> shaderStages2 = ShaderModuleGateway.GetShaderStages(captureID, num2);
						foreach (IShaderStage shaderStage2 in shaderStages2)
						{
							this.AddShaderStageToGroup(shaderStage2, shaderGroup, 353U, selectedApi, num);
							num3 += 1U;
						}
						num += num3;
					}
					string text;
					if (shaderGroup.Count() == 0 && !SdpApp.ModelManager.SnapshotModel.DataFilenames.TryGetValue((uint)captureID, out text))
					{
						shaderAnalyzerProgramEventArgs.IsWaiting = true;
					}
					SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.Invalidate, this, shaderAnalyzerProgramEventArgs);
					this.UpdateSnapshotShaderInfo(this.m_currentPipelineID);
					this.UpdateShaderSources();
				}
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00010560 File Offset: 0x0000E760
		public void DisplayTraceShaderInfo(int captureID, ulong pipelineID)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs;
			ShaderGroup shaderGroup;
			this.BuildEventArgs(captureID, pipelineID, "Trace Shader Stages For Pipeline", out shaderAnalyzerProgramEventArgs, out shaderGroup, 2401);
			IEnumerable<IShaderStage> traceShaderStages = ShaderModuleGateway.GetTraceShaderStages(captureID, (long)pipelineID);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLibraries");
			HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed(captureID, pipelineID, modelObject);
			if (traceShaderStages != null || pipelineLibraryUsed.Count != 0)
			{
				if (traceShaderStages != null)
				{
					foreach (IShaderStage shaderStage in traceShaderStages)
					{
						this.AddShaderStageToGroup(shaderStage, shaderGroup, 2401U, uint.MaxValue, 0U);
					}
				}
				foreach (ulong num in pipelineLibraryUsed)
				{
					IEnumerable<IShaderStage> shaderStages = ShaderModuleGateway.GetShaderStages(captureID, num);
					foreach (IShaderStage shaderStage2 in shaderStages)
					{
						this.AddShaderStageToGroup(shaderStage2, shaderGroup, 353U, uint.MaxValue, 0U);
					}
				}
				SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.Invalidate, this, shaderAnalyzerProgramEventArgs);
				this.m_currentPipelineID = pipelineID;
				this.UpdateTraceShaderInfo();
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public void DisplayTraversalShader(int captureID, ulong pipelineID)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs;
			ShaderGroup shaderGroup;
			this.BuildEventArgs(captureID, pipelineID, "Traversal Shader for Pipeline", out shaderAnalyzerProgramEventArgs, out shaderGroup, 353);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotShaderData");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureID.ToString(),
				"shaderStage",
				12.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data != null)
			{
				ShaderObject shaderObject = new ShaderObject(ShaderStage.Traversal, "");
				shaderGroup.AddShader(ShaderStage.Traversal, shaderObject);
				SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.Invalidate, this, shaderAnalyzerProgramEventArgs);
				this.UpdateSnapshotShaderInfo(pipelineID);
				this.UpdateShaderSources();
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000107B8 File Offset: 0x0000E9B8
		public void InvalidateTracePipelineInfo(uint captureID)
		{
			if (this.m_currentCaptureID != captureID)
			{
				this.InvalidateShaderStatProperties(captureID);
				this.m_currentCaptureID = captureID;
			}
			ShaderViewMgr.m_tracePipelineInfo.Clear();
			ModelObjectDataList vulkanShaderInfo = QGLModel.GetVulkanShaderInfo(captureID, "QGLModel", "VulkanTraceShaderData");
			foreach (ModelObjectData modelObjectData in vulkanShaderInfo)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("pipelineID"));
				int num2 = IntConverter.Convert(modelObjectData.GetValue("shaderIndex"));
				ShaderInfo shaderInfo;
				Dictionary<ShaderStage, Dictionary<int, List<uint>>> dictionary;
				Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> dictionary2;
				Dictionary<ShaderStage, Dictionary<int, string>> dictionary3;
				if (ShaderViewMgr.m_tracePipelineInfo.TryGetValue(num, out shaderInfo))
				{
					dictionary = shaderInfo.shaderStats;
					dictionary2 = shaderInfo.shaderLogStats;
					dictionary3 = shaderInfo.shaderDisasm;
				}
				else
				{
					dictionary = new Dictionary<ShaderStage, Dictionary<int, List<uint>>>();
					dictionary2 = new Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>>();
					dictionary3 = new Dictionary<ShaderStage, Dictionary<int, string>>();
					shaderInfo = new ShaderInfo(dictionary, dictionary2, dictionary3);
					ShaderViewMgr.m_tracePipelineInfo[num] = shaderInfo;
				}
				ProfilerShaderStage profilerShaderStage = (ProfilerShaderStage)UintConverter.Convert(modelObjectData.GetValue("shaderStage"));
				ShaderStage sdpshaderStage = this.GetSDPShaderStage(profilerShaderStage);
				Dictionary<int, List<uint>> dictionary4;
				if (!dictionary.TryGetValue(sdpshaderStage, out dictionary4))
				{
					dictionary4 = new Dictionary<int, List<uint>>();
					dictionary[sdpshaderStage] = dictionary4;
				}
				dictionary4[num2] = this.GetShaderStats(modelObjectData);
				Dictionary<int, List<ShaderLogStat>> dictionary5;
				if (!dictionary2.TryGetValue(sdpshaderStage, out dictionary5))
				{
					dictionary5 = new Dictionary<int, List<ShaderLogStat>>();
					dictionary2[sdpshaderStage] = dictionary5;
				}
				dictionary5[num2] = ShaderViewMgr.ExtractShaderStatsFromLogs(modelObjectData.GetValue("shaderStats"));
				Dictionary<int, string> dictionary6;
				if (!dictionary3.TryGetValue(sdpshaderStage, out dictionary6))
				{
					dictionary6 = new Dictionary<int, string>();
					dictionary3[sdpshaderStage] = dictionary6;
				}
				dictionary6[num2] = modelObjectData.GetValue("shaderDisasm");
			}
			this.UpdateTraceShaderInfo();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00010978 File Offset: 0x0000EB78
		public void InvalidateSnapshotPipelineInfo(uint captureID)
		{
			if (this.m_currentCaptureID != captureID)
			{
				this.InvalidateShaderStatProperties(captureID);
				this.m_currentCaptureID = captureID;
			}
			ShaderViewMgr.m_snapshotPipelineInfo.Clear();
			ShaderViewMgr.m_pipelineShaderSources.Clear();
			ModelObjectDataList vulkanShaderInfo = QGLModel.GetVulkanShaderInfo(captureID, "VulkanSnapshot", "VulkanSnapshotShaderData");
			foreach (ModelObjectData modelObjectData in vulkanShaderInfo)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("pipelineID"));
				int num2 = IntConverter.Convert(modelObjectData.GetValue("shaderIndex"));
				ShaderInfo shaderInfo;
				Dictionary<ShaderStage, Dictionary<int, List<uint>>> dictionary;
				Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> dictionary2;
				Dictionary<ShaderStage, Dictionary<int, string>> dictionary3;
				if (ShaderViewMgr.m_snapshotPipelineInfo.TryGetValue(num, out shaderInfo))
				{
					dictionary = shaderInfo.shaderStats;
					dictionary2 = shaderInfo.shaderLogStats;
					dictionary3 = shaderInfo.shaderDisasm;
				}
				else
				{
					dictionary = new Dictionary<ShaderStage, Dictionary<int, List<uint>>>();
					dictionary2 = new Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>>();
					dictionary3 = new Dictionary<ShaderStage, Dictionary<int, string>>();
					shaderInfo = new ShaderInfo(dictionary, dictionary2, dictionary3);
					ShaderViewMgr.m_snapshotPipelineInfo[num] = shaderInfo;
				}
				ProfilerShaderStage profilerShaderStage = (ProfilerShaderStage)UintConverter.Convert(modelObjectData.GetValue("shaderStage"));
				ShaderStage sdpshaderStage = this.GetSDPShaderStage(profilerShaderStage);
				Dictionary<int, List<uint>> dictionary4;
				if (!dictionary.TryGetValue(sdpshaderStage, out dictionary4))
				{
					dictionary4 = new Dictionary<int, List<uint>>();
					dictionary[sdpshaderStage] = dictionary4;
				}
				dictionary4[num2] = this.GetShaderStats(modelObjectData);
				Dictionary<int, List<ShaderLogStat>> dictionary5;
				if (!dictionary2.TryGetValue(sdpshaderStage, out dictionary5))
				{
					dictionary5 = new Dictionary<int, List<ShaderLogStat>>();
					dictionary2[sdpshaderStage] = dictionary5;
				}
				dictionary5[num2] = ShaderViewMgr.ExtractShaderStatsFromLogs(modelObjectData.GetValue("shaderStats"));
				Dictionary<int, string> dictionary6;
				if (!dictionary3.TryGetValue(sdpshaderStage, out dictionary6))
				{
					dictionary6 = new Dictionary<int, string>();
					dictionary3[sdpshaderStage] = dictionary6;
				}
				dictionary6[num2] = modelObjectData.GetValue("shaderDisasm");
			}
			this.UpdateSnapshotShaderInfo(this.m_currentPipelineID);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00010B48 File Offset: 0x0000ED48
		private void BuildEventArgs(int captureID, ulong resourceID, string resourceType, out ShaderAnalyzerProgramEventArgs invalidateArgs, out ShaderGroup shaderGroup, int sourceID = 353)
		{
			invalidateArgs = new ShaderAnalyzerProgramEventArgs();
			invalidateArgs.CategoryID = 3;
			invalidateArgs.ResourceID = (int)resourceID;
			invalidateArgs.Source = sourceID;
			invalidateArgs.EnableOverride = false;
			if (invalidateArgs.Source != 353 && invalidateArgs.Source != 2401)
			{
				invalidateArgs.IsEditable = true;
			}
			shaderGroup = new ShaderGroup();
			if (sourceID == 353)
			{
				shaderGroup.ResourceType = string.Format("{0}: {1}", resourceType, resourceID);
			}
			else
			{
				shaderGroup.ResourceType = string.Format("{0}: 0x{1:X}", resourceType, resourceID);
			}
			shaderGroup.CaptureID = captureID;
			shaderGroup.ResourceID = resourceID;
			shaderGroup.Separable = false;
			shaderGroup.IsBinary = false;
			invalidateArgs.ShaderObjectGroup = shaderGroup;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00010C20 File Offset: 0x0000EE20
		private unsafe void AddShaderStageToGroup(IShaderStage shaderStage, ShaderGroup shaderGroup, uint sourceID, uint selectedApi, uint shaderIndexOffset = 0U)
		{
			if (shaderStage == null)
			{
				return;
			}
			if (sourceID == 353U)
			{
				ShaderStage shaderStage2 = VkHelper.ConvertVkShaderEnum(shaderStage.StageType);
				if (!shaderGroup.CanAddShaderType(shaderStage2))
				{
					return;
				}
				IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer((int)shaderStage.CaptureID, shaderStage.ShaderModuleID);
				if (byteBuffer == null)
				{
					return;
				}
				ShaderObject shaderObject = new ShaderObject(shaderStage2, shaderStage.ShaderModuleID, SpirvDis.GetSpirvDis((byte*)(void*)byteBuffer.BDP.data, byteBuffer.BDP.size));
				shaderObject.ShaderIndex = shaderIndexOffset + shaderStage.ShaderIndex;
				shaderGroup.AddShader(shaderStage2, shaderObject);
				SpirvCross.Convert(shaderStage, this.m_currentFormat, shaderIndexOffset);
				IEnumerable<IShaderProfile> shaderProfiles = ShaderProfileGateway.GetShaderProfiles(shaderStage.CaptureID, selectedApi, (uint)shaderStage.StageType);
				if (shaderProfiles == null)
				{
					this.m_logger.LogDebug(string.Concat(new string[]
					{
						"Failed to find shader profiles for captureID:",
						shaderStage.CaptureID.ToString(),
						" apiID:",
						selectedApi.ToString(),
						" shader stage:",
						shaderStage.StageType.ToString()
					}));
					return;
				}
				Dictionary<uint, Tuple<uint, uint>> dictionary;
				ulong num;
				this.ProcessShaderProfiles(shaderStage2, shaderProfiles, out dictionary, out num);
				shaderObject.ShaderCycleCount = num;
				if (dictionary.Count > 0)
				{
					shaderObject.HitCyclePercentages = dictionary;
					return;
				}
			}
			else if (sourceID == 2401U)
			{
				ShaderStage sdpshaderStage = this.GetSDPShaderStage((ProfilerShaderStage)shaderStage.ShaderStage);
				shaderGroup.AddShader(sdpshaderStage, new ShaderObject(sdpshaderStage, "Shader Source Code is not available:"));
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00010D8C File Offset: 0x0000EF8C
		private ShaderStage GetSDPShaderStage(ProfilerShaderStage shaderType)
		{
			ShaderStage shaderStage = ShaderStage.Vertex;
			switch (shaderType)
			{
			case ProfilerShaderStage.Vertex:
				shaderStage = ShaderStage.Vertex;
				break;
			case ProfilerShaderStage.TessCtrl:
				shaderStage = ShaderStage.TessControl;
				break;
			case ProfilerShaderStage.TessEval:
				shaderStage = ShaderStage.TessEval;
				break;
			case ProfilerShaderStage.Geometry:
				shaderStage = ShaderStage.Geometry;
				break;
			case ProfilerShaderStage.Fragment:
				shaderStage = ShaderStage.Fragment;
				break;
			case ProfilerShaderStage.Compute:
				shaderStage = ShaderStage.Compute;
				break;
			case ProfilerShaderStage.VertexBinning:
				shaderStage = ShaderStage.VertexBinning;
				break;
			case ProfilerShaderStage.Task:
				shaderStage = ShaderStage.Task;
				break;
			case ProfilerShaderStage.Mesh:
				shaderStage = ShaderStage.Mesh;
				break;
			case ProfilerShaderStage.ClosestHit:
				shaderStage = ShaderStage.ClosestHit;
				break;
			case ProfilerShaderStage.AnyHit:
				shaderStage = ShaderStage.AnyHit;
				break;
			case ProfilerShaderStage.Traversal:
				shaderStage = ShaderStage.Traversal;
				break;
			case ProfilerShaderStage.RayGen:
				shaderStage = ShaderStage.RayGen;
				break;
			case ProfilerShaderStage.Callable:
				shaderStage = ShaderStage.Callable;
				break;
			case ProfilerShaderStage.Miss:
				shaderStage = ShaderStage.Miss;
				break;
			case ProfilerShaderStage.Intersection:
				shaderStage = ShaderStage.Intersection;
				break;
			}
			return shaderStage;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00010E2C File Offset: 0x0000F02C
		private ModelObjectDataList RetrieveShaderModules(StringList shaderStagesLookUpString)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			if (model == null)
			{
				return null;
			}
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotShaderStages");
			if (modelObject == null)
			{
				return null;
			}
			ModelObjectDataList data = modelObject.GetData(shaderStagesLookUpString);
			if (data == null || data.Count == 0)
			{
				return null;
			}
			return data;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00010E80 File Offset: 0x0000F080
		private List<uint> GetShaderStats(ModelObjectData data)
		{
			BinaryDataPair valuePtrBinaryDataPair = data.GetValuePtrBinaryDataPair("shaderStatValues");
			if (valuePtrBinaryDataPair != null)
			{
				IntPtr data2 = valuePtrBinaryDataPair.data;
				if (valuePtrBinaryDataPair.size > 0U)
				{
					List<uint> list = new List<uint>();
					int num = 0;
					while ((long)num < (long)((ulong)valuePtrBinaryDataPair.size))
					{
						list.Add((uint)Marshal.ReadInt32(valuePtrBinaryDataPair.data, num));
						num += 4;
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00010EDC File Offset: 0x0000F0DC
		private static List<ShaderLogStat> ExtractShaderStatsFromLogs(string shaderLogs)
		{
			List<ShaderLogStat> list = new List<ShaderLogStat>();
			string[] array = shaderLogs.Split(new char[] { '\n' });
			if (array.Length < 2)
			{
				return list;
			}
			string[] array2 = array[0].Split(new char[] { ',' });
			string[] array3 = array[1].Split(new char[] { ',' });
			uint num = 1U;
			while ((ulong)num < (ulong)((long)array2.Length) && (ulong)num < (ulong)((long)array3.Length))
			{
				uint num2 = UintConverter.Convert(array3[(int)num]);
				ShaderLogStat shaderLogStat = new ShaderLogStat
				{
					Name = array2[(int)num],
					Value = (ulong)num2
				};
				list.Add(shaderLogStat);
				num += 1U;
			}
			return list;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00010F7C File Offset: 0x0000F17C
		private void InvalidateShaderStatProperties(uint captureID)
		{
			Dictionary<uint, string> dictionary = new Dictionary<uint, string>();
			Dictionary<uint, string> dictionary2 = new Dictionary<uint, string>();
			ModelObjectDataList vulkanShaderStatProperty = QGLModel.GetVulkanShaderStatProperty(captureID);
			foreach (ModelObjectData modelObjectData in vulkanShaderStatProperty)
			{
				uint num = UintConverter.Convert(modelObjectData.GetValue("statID"));
				dictionary.Add(num, modelObjectData.GetValue("name"));
				dictionary2.Add(num, modelObjectData.GetValue("description"));
			}
			ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs shaderAnalyzerInvalidateShaderStatPropertiesEventArgs = new ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs();
			shaderAnalyzerInvalidateShaderStatPropertiesEventArgs.Names = dictionary;
			shaderAnalyzerInvalidateShaderStatPropertiesEventArgs.Descriptions = dictionary2;
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateShaderStatProperties, this, shaderAnalyzerInvalidateShaderStatPropertiesEventArgs);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00011040 File Offset: 0x0000F240
		private void UpdateTraceShaderInfo()
		{
			ulong currentPipelineID = this.m_currentPipelineID;
			ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs shaderAnalyzerInvalidateCurrentShaderStatsEventArgs = new ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs();
			ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs shaderAnalyzerInvalidateCurrentShaderLogsEventArgs = new ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs();
			ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs = new ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs();
			shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.ResourceID = currentPipelineID;
			shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.ResourceID = currentPipelineID;
			shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ResourceID = currentPipelineID;
			ShaderInfo shaderInfo;
			if (ShaderViewMgr.m_tracePipelineInfo.TryGetValue(currentPipelineID, out shaderInfo))
			{
				shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.Stats = shaderInfo.shaderStats;
				shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.LogStats = shaderInfo.shaderLogStats;
				shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ShaderDisasm = shaderInfo.shaderDisasm;
			}
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderStats, this, shaderAnalyzerInvalidateCurrentShaderStatsEventArgs);
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderLogStats, this, shaderAnalyzerInvalidateCurrentShaderLogsEventArgs);
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderDisasm, this, shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00011104 File Offset: 0x0000F304
		private void UpdateSnapshotShaderInfo(ulong pipelineID)
		{
			ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs shaderAnalyzerInvalidateCurrentShaderStatsEventArgs = new ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs();
			ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs shaderAnalyzerInvalidateCurrentShaderLogsEventArgs = new ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs();
			ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs = new ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs();
			shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.ResourceID = pipelineID;
			shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.ResourceID = pipelineID;
			shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ResourceID = pipelineID;
			int shaderIndexOffset = 0;
			ShaderInfo shaderInfo;
			if (ShaderViewMgr.m_snapshotPipelineInfo.TryGetValue(pipelineID, out shaderInfo))
			{
				shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.Stats = shaderInfo.shaderStats;
				shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.LogStats = shaderInfo.shaderLogStats;
				shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ShaderDisasm = shaderInfo.shaderDisasm;
				IEnumerable<IShaderStage> shaderStages = ShaderModuleGateway.GetShaderStages((int)this.m_currentCaptureID, pipelineID);
				if (shaderStages == null)
				{
					goto IL_00DB;
				}
				using (IEnumerator<IShaderStage> enumerator = shaderStages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IShaderStage shaderStage = enumerator.Current;
						int num = shaderIndexOffset;
						shaderIndexOffset = num + 1;
					}
					goto IL_00DB;
				}
			}
			shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.Stats = new Dictionary<ShaderStage, Dictionary<int, List<uint>>>();
			shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.LogStats = new Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>>();
			shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ShaderDisasm = new Dictionary<ShaderStage, Dictionary<int, string>>();
			IL_00DB:
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLibraries");
			HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed((int)this.m_currentCaptureID, pipelineID, modelObject);
			Func<KeyValuePair<int, List<uint>>, int> <>9__15;
			Func<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>, Dictionary<int, List<uint>>> <>9__1;
			Func<KeyValuePair<int, List<ShaderLogStat>>, int> <>9__17;
			Func<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>, Dictionary<int, List<ShaderLogStat>>> <>9__3;
			Func<KeyValuePair<int, string>, int> <>9__19;
			Func<KeyValuePair<ShaderStage, Dictionary<int, string>>, Dictionary<int, string>> <>9__5;
			foreach (ulong num2 in pipelineLibraryUsed)
			{
				if (ShaderViewMgr.m_snapshotPipelineInfo.TryGetValue(num2, out shaderInfo))
				{
					IEnumerable<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>> shaderStats = shaderInfo.shaderStats;
					Func<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>, ShaderStage> func = (KeyValuePair<ShaderStage, Dictionary<int, List<uint>>> kvp) => kvp.Key;
					Func<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>, Dictionary<int, List<uint>>> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = delegate(KeyValuePair<ShaderStage, Dictionary<int, List<uint>>> kvp)
						{
							IEnumerable<KeyValuePair<int, List<uint>>> value = kvp.Value;
							Func<KeyValuePair<int, List<uint>>, int> func7;
							if ((func7 = <>9__15) == null)
							{
								func7 = (<>9__15 = (KeyValuePair<int, List<uint>> kvp_s) => kvp_s.Key + shaderIndexOffset);
							}
							return Enumerable.ToDictionary<KeyValuePair<int, List<uint>>, int, List<uint>>(value, func7, (KeyValuePair<int, List<uint>> kvp_s) => kvp_s.Value);
						});
					}
					Dictionary<ShaderStage, Dictionary<int, List<uint>>> dictionary = Enumerable.ToDictionary<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>, ShaderStage, Dictionary<int, List<uint>>>(shaderStats, func, func2);
					IEnumerable<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>> shaderLogStats = shaderInfo.shaderLogStats;
					Func<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>, ShaderStage> func3 = (KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>> kvp) => kvp.Key;
					Func<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>, Dictionary<int, List<ShaderLogStat>>> func4;
					if ((func4 = <>9__3) == null)
					{
						func4 = (<>9__3 = delegate(KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>> kvp)
						{
							IEnumerable<KeyValuePair<int, List<ShaderLogStat>>> value2 = kvp.Value;
							Func<KeyValuePair<int, List<ShaderLogStat>>, int> func8;
							if ((func8 = <>9__17) == null)
							{
								func8 = (<>9__17 = (KeyValuePair<int, List<ShaderLogStat>> kvp_s) => kvp_s.Key + shaderIndexOffset);
							}
							return Enumerable.ToDictionary<KeyValuePair<int, List<ShaderLogStat>>, int, List<ShaderLogStat>>(value2, func8, (KeyValuePair<int, List<ShaderLogStat>> kvp_s) => kvp_s.Value);
						});
					}
					Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> dictionary2 = Enumerable.ToDictionary<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>, ShaderStage, Dictionary<int, List<ShaderLogStat>>>(shaderLogStats, func3, func4);
					IEnumerable<KeyValuePair<ShaderStage, Dictionary<int, string>>> shaderDisasm = shaderInfo.shaderDisasm;
					Func<KeyValuePair<ShaderStage, Dictionary<int, string>>, ShaderStage> func5 = (KeyValuePair<ShaderStage, Dictionary<int, string>> kvp) => kvp.Key;
					Func<KeyValuePair<ShaderStage, Dictionary<int, string>>, Dictionary<int, string>> func6;
					if ((func6 = <>9__5) == null)
					{
						func6 = (<>9__5 = delegate(KeyValuePair<ShaderStage, Dictionary<int, string>> kvp)
						{
							IEnumerable<KeyValuePair<int, string>> value3 = kvp.Value;
							Func<KeyValuePair<int, string>, int> func9;
							if ((func9 = <>9__19) == null)
							{
								func9 = (<>9__19 = (KeyValuePair<int, string> kvp_s) => kvp_s.Key + shaderIndexOffset);
							}
							return Enumerable.ToDictionary<KeyValuePair<int, string>, int, string>(value3, func9, (KeyValuePair<int, string> kvp_s) => kvp_s.Value);
						});
					}
					Dictionary<ShaderStage, Dictionary<int, string>> dictionary3 = Enumerable.ToDictionary<KeyValuePair<ShaderStage, Dictionary<int, string>>, ShaderStage, Dictionary<int, string>>(shaderDisasm, func5, func6);
					shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.Stats = Enumerable.ToDictionary<IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>, ShaderStage, Dictionary<int, List<uint>>>(Enumerable.GroupBy<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>, ShaderStage>(Enumerable.Concat<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(shaderAnalyzerInvalidateCurrentShaderStatsEventArgs.Stats, dictionary), (KeyValuePair<ShaderStage, Dictionary<int, List<uint>>> pair) => pair.Key), (IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>> group) => group.Key, delegate(IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>> group)
					{
						if (Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value != Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value)
						{
							return Enumerable.ToDictionary<KeyValuePair<int, List<uint>>, int, List<uint>>(Enumerable.Concat<KeyValuePair<int, List<uint>>>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value, Enumerable.Where<KeyValuePair<int, List<uint>>>(Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value, (KeyValuePair<int, List<uint>> x) => !Enumerable.Contains<int>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value.Keys, x.Key))), (KeyValuePair<int, List<uint>> kvp) => kvp.Key, (KeyValuePair<int, List<uint>> kvp) => kvp.Value);
						}
						return Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<uint>>>>(group).Value;
					});
					shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.LogStats = Enumerable.ToDictionary<IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>, ShaderStage, Dictionary<int, List<ShaderLogStat>>>(Enumerable.GroupBy<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>, ShaderStage>(Enumerable.Concat<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(shaderAnalyzerInvalidateCurrentShaderLogsEventArgs.LogStats, dictionary2), (KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>> pair) => pair.Key), (IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>> group) => group.Key, delegate(IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>> group)
					{
						if (Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value != Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value)
						{
							return Enumerable.ToDictionary<KeyValuePair<int, List<ShaderLogStat>>, int, List<ShaderLogStat>>(Enumerable.Concat<KeyValuePair<int, List<ShaderLogStat>>>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value, Enumerable.Where<KeyValuePair<int, List<ShaderLogStat>>>(Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value, (KeyValuePair<int, List<ShaderLogStat>> x) => !Enumerable.Contains<int>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value.Keys, x.Key))), (KeyValuePair<int, List<ShaderLogStat>> kvp) => kvp.Key, (KeyValuePair<int, List<ShaderLogStat>> kvp) => kvp.Value);
						}
						return Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, List<ShaderLogStat>>>>(group).Value;
					});
					shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ShaderDisasm = Enumerable.ToDictionary<IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, string>>>, ShaderStage, Dictionary<int, string>>(Enumerable.GroupBy<KeyValuePair<ShaderStage, Dictionary<int, string>>, ShaderStage>(Enumerable.Concat<KeyValuePair<ShaderStage, Dictionary<int, string>>>(shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs.ShaderDisasm, dictionary3), (KeyValuePair<ShaderStage, Dictionary<int, string>> pair) => pair.Key), (IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, string>>> group) => group.Key, delegate(IGrouping<ShaderStage, KeyValuePair<ShaderStage, Dictionary<int, string>>> group)
					{
						if (Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value != Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value)
						{
							return Enumerable.ToDictionary<KeyValuePair<int, string>, int, string>(Enumerable.Concat<KeyValuePair<int, string>>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value, Enumerable.Where<KeyValuePair<int, string>>(Enumerable.Last<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value, (KeyValuePair<int, string> x) => !Enumerable.Contains<int>(Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value.Keys, x.Key))), (KeyValuePair<int, string> kvp) => kvp.Key, (KeyValuePair<int, string> kvp) => kvp.Value);
						}
						return Enumerable.First<KeyValuePair<ShaderStage, Dictionary<int, string>>>(group).Value;
					});
					IEnumerable<IShaderStage> shaderStages2 = ShaderModuleGateway.GetShaderStages((int)this.m_currentCaptureID, num2);
					if (shaderStages2 != null)
					{
						foreach (IShaderStage shaderStage2 in shaderStages2)
						{
							int num = shaderIndexOffset;
							shaderIndexOffset = num + 1;
						}
					}
				}
			}
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderStats, this, shaderAnalyzerInvalidateCurrentShaderStatsEventArgs);
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderLogStats, this, shaderAnalyzerInvalidateCurrentShaderLogsEventArgs);
			SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderDisasm, this, shaderAnalyzerInvalidateCurrentShaderDisasmEventArgs);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000115B8 File Offset: 0x0000F7B8
		private void UpdateShaderSources()
		{
			ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs shaderAnalyzerInvalidateCurrentShaderSourceEventArgs = new ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs();
			shaderAnalyzerInvalidateCurrentShaderSourceEventArgs.ResourceID = this.m_currentPipelineID;
			PipelineShaderSources pipelineShaderSources;
			if (ShaderViewMgr.m_pipelineShaderSources.TryGetValue(this.m_currentPipelineID, out pipelineShaderSources))
			{
				shaderAnalyzerInvalidateCurrentShaderSourceEventArgs.ShaderSources = pipelineShaderSources.shaders;
				shaderAnalyzerInvalidateCurrentShaderSourceEventArgs.Succeeded = pipelineShaderSources.succeeded;
				SdpApp.EventsManager.Raise<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.InvalidateCurrentShaderSource, this, shaderAnalyzerInvalidateCurrentShaderSourceEventArgs);
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00011620 File Offset: 0x0000F820
		private void ProcessShaderProfiles(ShaderStage shaderType, IEnumerable<IShaderProfile> shaderProfiles, out Dictionary<uint, Tuple<uint, uint>> hitCyclePercentages, out ulong shaderCycleCount)
		{
			ulong num = 0UL;
			ulong num2 = 0UL;
			Dictionary<uint, Tuple<ulong, ulong>> dictionary = new Dictionary<uint, Tuple<ulong, ulong>>();
			foreach (IShaderProfile shaderProfile in shaderProfiles)
			{
				uint sourceLineNumber = shaderProfile.SourceLineNumber;
				uint instructionIndex = shaderProfile.InstructionIndex;
				uint counterType = shaderProfile.CounterType;
				ulong counter = shaderProfile.Counter;
				Tuple<ulong, ulong> tuple;
				if (!dictionary.TryGetValue(sourceLineNumber, out tuple))
				{
					tuple = new Tuple<ulong, ulong>(0UL, 0UL);
				}
				if (counterType == 0U)
				{
					dictionary[sourceLineNumber] = Tuple.Create<ulong, ulong>(tuple.Item1 + counter, tuple.Item2);
					if (sourceLineNumber != 0U)
					{
						num += counter;
					}
				}
				else if (counterType == 1U)
				{
					dictionary[sourceLineNumber] = Tuple.Create<ulong, ulong>(tuple.Item1, tuple.Item2 + counter);
					if (sourceLineNumber != 0U)
					{
						num2 += counter;
					}
				}
			}
			Tuple<ulong, ulong> tuple2;
			if (dictionary.TryGetValue(0U, out tuple2))
			{
				if (tuple2.Item1 != 0UL)
				{
					num = tuple2.Item1;
				}
				if (tuple2.Item2 != 0UL)
				{
					num2 = tuple2.Item2;
				}
				dictionary.Remove(0U);
			}
			shaderCycleCount = num2;
			hitCyclePercentages = new Dictionary<uint, Tuple<uint, uint>>();
			foreach (KeyValuePair<uint, Tuple<ulong, ulong>> keyValuePair in dictionary)
			{
				uint key = keyValuePair.Key;
				Tuple<ulong, ulong> value = keyValuePair.Value;
				uint num3 = 0U;
				uint num4 = 0U;
				if (num > 0UL)
				{
					num3 = Math.Min(Math.Max(0U, (uint)(value.Item1 * 100UL / num)), 100U);
				}
				if (num2 > 0UL)
				{
					num4 = Math.Min(Math.Max(0U, (uint)(value.Item2 * 100UL / num2)), 100U);
				}
				hitCyclePercentages[key] = new Tuple<uint, uint>(num3, num4);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000117F0 File Offset: 0x0000F9F0
		private void shaderAnalyzerEvents_shaderLanguageComboSelected(object sender, ShaderAnalyzerInvalidateShaderLanguageArgs args)
		{
			SpirvCross.FileFormat currentFormat = this.m_currentFormat;
			string shaderLanguage = args.ShaderLanguage;
			if (!(shaderLanguage == "HLSL"))
			{
				if (shaderLanguage == "GLSL")
				{
					this.m_currentFormat = SpirvCross.FileFormat.GLSL;
				}
			}
			else
			{
				this.m_currentFormat = SpirvCross.FileFormat.HLSL;
			}
			if (this.m_currentFormat != currentFormat)
			{
				this.ConvertCurrentShaders();
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00011846 File Offset: 0x0000FA46
		private void shaderAnalyzerEvents_ConvertShaderSource(object sender, EventArgs args)
		{
			this.ConvertCurrentShaders();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00011850 File Offset: 0x0000FA50
		private void shaderAnalyzerEvents_ExportShader(object sender, ShaderAnalyzerExportShaderArgs args)
		{
			SpirvCross.FileFormat fileFormat = SpirvCross.FileFormat.Unknown;
			if (args.ShaderLanguage == "glsl")
			{
				fileFormat = SpirvCross.FileFormat.GLSL;
			}
			else if (args.ShaderLanguage == "hlsl")
			{
				fileFormat = SpirvCross.FileFormat.HLSL;
			}
			if (fileFormat == SpirvCross.FileFormat.Unknown)
			{
				return;
			}
			IEnumerable<IShaderStage> shaderStages = ShaderModuleGateway.GetShaderStages((int)this.m_currentCaptureID, (ulong)((uint)this.m_currentPipelineID));
			if (shaderStages != null)
			{
				foreach (IShaderStage shaderStage in shaderStages)
				{
					if (args.ShaderIndex == shaderStage.ShaderIndex)
					{
						ShaderViewMgr.m_filesToSave.Add(shaderStage.ShaderIndex, args.Filename);
						SpirvCross.Convert(shaderStage, fileFormat, 0U);
						break;
					}
				}
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00011908 File Offset: 0x0000FB08
		private void ConvertCurrentShaders()
		{
			ShaderViewMgr.m_pipelineShaderSources.Clear();
			IEnumerable<IShaderStage> shaderStages = ShaderModuleGateway.GetShaderStages((int)this.m_currentCaptureID, (ulong)((uint)this.m_currentPipelineID));
			uint num = 0U;
			if (shaderStages != null)
			{
				foreach (IShaderStage shaderStage in shaderStages)
				{
					SpirvCross.Convert(shaderStage, this.m_currentFormat, 0U);
					num += 1U;
				}
			}
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLibraries");
			HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed((int)this.m_currentCaptureID, (ulong)((uint)this.m_currentPipelineID), modelObject);
			foreach (ulong num2 in pipelineLibraryUsed)
			{
				uint num3 = 0U;
				IEnumerable<IShaderStage> shaderStages2 = ShaderModuleGateway.GetShaderStages((int)this.m_currentCaptureID, num2);
				if (shaderStages2 != null)
				{
					foreach (IShaderStage shaderStage2 in shaderStages2)
					{
						SpirvCross.Convert(shaderStage2, this.m_currentFormat, num);
						num3 += 1U;
					}
					num += num3;
				}
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00011A64 File Offset: 0x0000FC64
		private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
		{
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA)
			{
				SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
				if (processorPlugin != null)
				{
					BinaryDataPair localBuffer = processorPlugin.GetLocalBuffer(args.BufferCategory, args.BufferID, args.CaptureID);
					SpirvCross.Response response = Marshal.PtrToStructure<SpirvCross.Response>(localBuffer.data);
					ShaderStage shaderType = (ShaderStage)response.Header.ShaderType;
					bool success = response.Success;
					string output = response.Output;
					string text;
					if (ShaderViewMgr.m_filesToSave.TryGetValue(response.Header.ShaderIndex, out text))
					{
						ShaderViewMgr.m_filesToSave.Remove(response.Header.ShaderIndex);
						if (success)
						{
							File.WriteAllText(text, output);
							return;
						}
						ShaderAnalyzerSaveFailedEventArgs shaderAnalyzerSaveFailedEventArgs = new ShaderAnalyzerSaveFailedEventArgs();
						shaderAnalyzerSaveFailedEventArgs.ErrorString = output;
						SdpApp.EventsManager.Raise<ShaderAnalyzerSaveFailedEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.SaveFailed, this, shaderAnalyzerSaveFailedEventArgs);
						return;
					}
					else
					{
						PipelineShaderSources pipelineShaderSources;
						if (!ShaderViewMgr.m_pipelineShaderSources.TryGetValue(this.m_currentPipelineID, out pipelineShaderSources))
						{
							pipelineShaderSources = default(PipelineShaderSources);
							pipelineShaderSources.succeeded = new Dictionary<ShaderStage, Dictionary<int, bool>>();
							pipelineShaderSources.shaders = new Dictionary<ShaderStage, Dictionary<int, string>>();
							ShaderViewMgr.m_pipelineShaderSources[this.m_currentPipelineID] = pipelineShaderSources;
						}
						Dictionary<int, bool> dictionary;
						if (!pipelineShaderSources.succeeded.TryGetValue(shaderType, out dictionary))
						{
							dictionary = new Dictionary<int, bool>();
							pipelineShaderSources.succeeded[shaderType] = dictionary;
						}
						Dictionary<int, string> dictionary2;
						if (!pipelineShaderSources.shaders.TryGetValue(shaderType, out dictionary2))
						{
							dictionary2 = new Dictionary<int, string>();
							pipelineShaderSources.shaders[shaderType] = dictionary2;
						}
						if (!dictionary2.ContainsKey((int)response.Header.ShaderIndex))
						{
							dictionary[(int)response.Header.ShaderIndex] = success;
							dictionary2[(int)response.Header.ShaderIndex] = output;
							this.UpdateShaderSources();
						}
					}
				}
			}
		}

		// Token: 0x040003C6 RID: 966
		private static object mutex = new object();

		// Token: 0x040003C7 RID: 967
		private static Dictionary<ulong, ShaderInfo> m_snapshotPipelineInfo = new Dictionary<ulong, ShaderInfo>();

		// Token: 0x040003C8 RID: 968
		private static Dictionary<ulong, ShaderInfo> m_tracePipelineInfo = new Dictionary<ulong, ShaderInfo>();

		// Token: 0x040003C9 RID: 969
		private static Dictionary<ulong, PipelineShaderSources> m_pipelineShaderSources = new Dictionary<ulong, PipelineShaderSources>();

		// Token: 0x040003CA RID: 970
		private static Dictionary<uint, string> m_filesToSave = new Dictionary<uint, string>();

		// Token: 0x040003CB RID: 971
		private SpirvCross.FileFormat m_currentFormat;

		// Token: 0x040003CC RID: 972
		private readonly ILogger m_logger = new global::Sdp.Logging.Logger("QGL ShaderViewMgr");

		// Token: 0x040003CD RID: 973
		private ulong m_currentPipelineID;

		// Token: 0x040003CE RID: 974
		private uint m_currentCaptureID;

		// Token: 0x040003CF RID: 975
		private ByteBufferGateway m_byteBufferGateway;
	}
}
