using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;
using SdpClientFramework.DesignPatterns.SingleConsumer;

namespace QGLPlugin
{
	// Token: 0x02000037 RID: 55
	public class ResourcesViewMgr
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000B280 File Offset: 0x00009480
		private static Dictionary<ulong, string> ConvertToDictionaryWithDuplicates(ModelObjectDataList dataList)
		{
			Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
			foreach (ModelObjectData modelObjectData in dataList)
			{
				ulong num = ulong.Parse(modelObjectData.GetValue("resourceID"));
				string value = modelObjectData.GetValue("objectLabel");
				dictionary[num] = value;
			}
			return dictionary;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000B2F0 File Offset: 0x000094F0
		private static Dictionary<ulong, List<uint>> ConvertShaderStagePerPipelineToDictionary(ModelObjectDataList dataList)
		{
			Dictionary<ulong, List<uint>> dictionary = new Dictionary<ulong, List<uint>>();
			foreach (ModelObjectData modelObjectData in dataList)
			{
				uint num = UintConverter.Convert(modelObjectData.GetValue("stageType"));
				ulong num2 = Uint64Converter.Convert(modelObjectData.GetValue("pipelineID"));
				List<uint> list;
				if (!dictionary.TryGetValue(num2, out list))
				{
					list = (dictionary[num2] = new List<uint>());
				}
				list.Add(num);
			}
			return dictionary;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000B384 File Offset: 0x00009584
		private static Dictionary<ulong, List<ResourcesViewMgr.StagePipelineIDTuple>> ConvertShaderStageToDictionary(ModelObjectDataList dataList)
		{
			Dictionary<ulong, List<ResourcesViewMgr.StagePipelineIDTuple>> dictionary = new Dictionary<ulong, List<ResourcesViewMgr.StagePipelineIDTuple>>();
			foreach (ModelObjectData modelObjectData in dataList)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("shaderModuleID"));
				uint num2 = UintConverter.Convert(modelObjectData.GetValue("stageType"));
				ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("pipelineID"));
				List<ResourcesViewMgr.StagePipelineIDTuple> list;
				if (!dictionary.TryGetValue(num, out list))
				{
					list = (dictionary[num] = new List<ResourcesViewMgr.StagePipelineIDTuple>());
				}
				list.Add(new ResourcesViewMgr.StagePipelineIDTuple(num2, num3));
			}
			return dictionary;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000B430 File Offset: 0x00009630
		private static Dictionary<ulong, ulong> ConvertDescSetLinksToDictionary(ModelObjectDataList dataList)
		{
			Dictionary<ulong, ulong> dictionary = new Dictionary<ulong, ulong>();
			foreach (ModelObjectData modelObjectData in dataList)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("descriptorSetID"));
				ulong num2 = Uint64Converter.Convert(modelObjectData.GetValue("descriptorSetLayoutID"));
				dictionary[num] = num2;
			}
			return dictionary;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000B4A4 File Offset: 0x000096A4
		private static Dictionary<ulong, ulong> ConvertImageViewsToDictionary(ModelObjectDataList dataList)
		{
			Dictionary<ulong, ulong> dictionary = new Dictionary<ulong, ulong>();
			foreach (ModelObjectData modelObjectData in dataList)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
				ulong num2 = Uint64Converter.Convert(modelObjectData.GetValue("imageID"));
				dictionary[num] = num2;
			}
			return dictionary;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000B518 File Offset: 0x00009718
		public ResourcesViewMgr()
		{
			DataExplorerViewEvents dataExplorerViewEvents = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents.SourceSelected = (EventHandler<SourceEventArgs>)Delegate.Combine(dataExplorerViewEvents.SourceSelected, new EventHandler<SourceEventArgs>(this.dataExplorerViewEvents_SourceSelected));
			DataExplorerViewEvents dataExplorerViewEvents2 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents2.RowSelected = (EventHandler<DataExplorerViewRowSelectedEventArgs>)Delegate.Combine(dataExplorerViewEvents2.RowSelected, new EventHandler<DataExplorerViewRowSelectedEventArgs>(this.dataExplorerViewEvents_RowSelected));
			ResourceViewEvents resourceViewEvents = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents.SourceSelected = (EventHandler<SourceEventArgs>)Delegate.Combine(resourceViewEvents.SourceSelected, new EventHandler<SourceEventArgs>(this.resourceViewEvents_SourceSelected));
			ResourceViewEvents resourceViewEvents2 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents2.ItemSelected = (EventHandler<ItemSelectedEventArgs>)Delegate.Combine(resourceViewEvents2.ItemSelected, new EventHandler<ItemSelectedEventArgs>(this.resourceViewEvents_ItemSelected));
			ResourceViewEvents resourceViewEvents3 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents3.PrepopulateCategory = (EventHandler<PrepopulateCategoryArgs>)Delegate.Combine(resourceViewEvents3.PrepopulateCategory, new EventHandler<PrepopulateCategoryArgs>(this.resourceViewEvents_PrepopulateCategory));
			ProgramViewEvents programViewEvents = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents.FormatChanged = (EventHandler<ProgramViewFormatChangedArgs>)Delegate.Combine(programViewEvents.FormatChanged, new EventHandler<ProgramViewFormatChangedArgs>(this.programViewEvents_FormatChanged));
			ResourceViewEvents resourceViewEvents4 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents4.ItemDoubleClicked = (EventHandler<ItemDoubleClickedEventArgs>)Delegate.Combine(resourceViewEvents4.ItemDoubleClicked, new EventHandler<ItemDoubleClickedEventArgs>(this.resourceViewEvents_ItemDoubleClicked));
			this.m_byteBufferGateway = new ByteBufferGateway("VulkanSnapshot", "VulkanSnapshotByteBuffers");
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000B6DA File Offset: 0x000098DA
		public int GetCurrentCaptureID()
		{
			return this.m_currentCaptureId;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000B6E2 File Offset: 0x000098E2
		private void dataExplorerViewEvents_SourceSelected(object sender, SourceEventArgs sourceArgs)
		{
			if (sourceArgs.SourceID == 353)
			{
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.ResourceViewEvents.SelectSource, this, sourceArgs);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000B70C File Offset: 0x0000990C
		private void dataExplorerViewEvents_RowSelected(object o, DataExplorerViewRowSelectedEventArgs e)
		{
			if (e.SourceID == 353)
			{
				string text = e.SelectedRow[5].ToString();
				bool flag = VkHelper.IsDrawCall(text) || VkHelper.IsDrawcallParent(text);
				if (e.NumClicks == 1)
				{
					this.UpdateHeader(e.SelectedRow[1], e.SelectedRow[2]);
					uint num;
					if (this.m_selectedApi.TryGetValue((uint)e.CaptureID, out num))
					{
						bool flag2 = false;
						if (flag && num != (uint)e.SelectedRow[7])
						{
							this.m_selectedApi[(uint)e.CaptureID] = (uint)e.SelectedRow[7];
							this.m_selectedDrawCallID[(uint)e.CaptureID] = (string)e.SelectedRow[1];
							flag2 = true;
						}
						else if (!flag && num != 4294967295U)
						{
							this.m_selectedApi[(uint)e.CaptureID] = uint.MaxValue;
							this.m_selectedDrawCallID[(uint)e.CaptureID] = string.Empty;
							flag2 = true;
						}
						if (VkHelper.IsDrawcallParent(text))
						{
							this.m_apiRange = new ParentBoundInfo
							{
								ParentApiID = (uint)e.SelectedRow[0],
								DrawcallEnd = e.SelectedRow[8].ToString()
							};
							flag2 = true;
						}
						if (flag2 && this.m_currentCaptureId != -1)
						{
							if (this.m_allBoundUsedSelection != ResourcesViewMgr.AllBoundUsed.All)
							{
								this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive, false);
								return;
							}
							uint num2;
							if (flag && this.m_selectedApi.TryGetValue((uint)this.m_currentCaptureId, out num2))
							{
								VkBoundInfo vkBoundInfo;
								QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_currentCaptureId, num2, out vkBoundInfo);
								QGLPlugin.ShaderMgr.DisplayPipelineStages(this.m_currentCaptureId, vkBoundInfo.BoundPipeline, num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		private void resourceViewEvents_SourceSelected(object sender, SourceEventArgs sourceArgs)
		{
			if (this.m_currentCaptureId == sourceArgs.CaptureID && this.m_currentSourceId == sourceArgs.SourceID)
			{
				return;
			}
			ResourcesViewMgr.InvalidateClass.CancelAll();
			this.m_statusQueue.Clear();
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ResourceViewEvents.HideStatus, this, new EventArgs());
			bool flag = false;
			if (!ResourcesViewMgr.m_captureIDList.Contains(sourceArgs.CaptureID))
			{
				ResourcesViewMgr.m_captureIDList.Add(sourceArgs.CaptureID);
				flag = true;
			}
			if (sourceArgs.SourceID == 353)
			{
				if (this.m_currentCaptureId != sourceArgs.CaptureID || this.m_currentSourceId != sourceArgs.SourceID)
				{
					VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_currentCaptureId);
					if (capture == null)
					{
						return;
					}
					capture.HasThumbnails = false;
					this.m_currentCaptureId = sourceArgs.CaptureID;
					this.m_currentSourceId = sourceArgs.SourceID;
					this.m_uiRequestHandler.SetProgramInspectorCaptureID(this.m_currentCaptureId);
					this.m_selectedApi[(uint)this.m_currentCaptureId] = capture.LastDrawCall;
					this.m_selectedDrawCallID[(uint)this.m_currentCaptureId] = capture.LastDrawCallID;
					if (!flag)
					{
						QGLPlugin.ShaderMgr.InvalidateSnapshotPipelineInfo((uint)this.m_currentCaptureId);
					}
					this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange, flag);
				}
			}
			else if (sourceArgs.SourceID == 2401 && (this.m_currentCaptureId != sourceArgs.CaptureID || this.m_currentSourceId != sourceArgs.SourceID))
			{
				this.m_currentCaptureId = sourceArgs.CaptureID;
				this.m_currentSourceId = sourceArgs.SourceID;
				if (!flag)
				{
					QGLPlugin.ShaderMgr.InvalidateTracePipelineInfo((uint)this.m_currentCaptureId);
				}
				this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange, false);
			}
			this.m_currentSourceId = sourceArgs.SourceID;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000BA50 File Offset: 0x00009C50
		private void resourceViewEvents_PrepopulateCategory(object sender, PrepopulateCategoryArgs args)
		{
			if (args.Source == 353 && args.ResourceIds.Count > 0)
			{
				List<PrepopulateCategoryArgs> list;
				if (!QGLPlugin.VkSnapshotModel.ResourcesPerDrawcall.TryGetValue((uint)args.DrawcallId, out list))
				{
					list = (QGLPlugin.VkSnapshotModel.ResourcesPerDrawcall[(uint)args.DrawcallId] = new List<PrepopulateCategoryArgs>());
				}
				list.Add(args);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000BAB6 File Offset: 0x00009CB6
		private void programViewEvents_FormatChanged(object sender, ProgramViewFormatChangedArgs args)
		{
			this.m_uiRequestHandler.UpdateDataFormat(args.DataType);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000BACC File Offset: 0x00009CCC
		private void ResetFilters()
		{
			SetActiveToolItemInFilterEventArgs setActiveToolItemInFilterEventArgs = new SetActiveToolItemInFilterEventArgs();
			setActiveToolItemInFilterEventArgs.FilterName = this.FORMAT_COMBO_FILTER_NAME;
			setActiveToolItemInFilterEventArgs.ToolItemName = "All Formats";
			SdpApp.EventsManager.Raise<SetActiveToolItemInFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ResetFilterToAll, this, setActiveToolItemInFilterEventArgs);
			this.m_formatSelection = "All Formats";
			SetActiveToolItemInFilterEventArgs setActiveToolItemInFilterEventArgs2 = new SetActiveToolItemInFilterEventArgs();
			setActiveToolItemInFilterEventArgs2.FilterName = this.MARKER_COMBO_FILTER_NAME;
			setActiveToolItemInFilterEventArgs2.ToolItemName = "All Objects";
			SdpApp.EventsManager.Raise<SetActiveToolItemInFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ResetFilterToAll, this, setActiveToolItemInFilterEventArgs2);
			this.m_markerSelection = "All Objects";
			SetActiveToolItemInFilterEventArgs setActiveToolItemInFilterEventArgs3 = new SetActiveToolItemInFilterEventArgs();
			setActiveToolItemInFilterEventArgs3.FilterName = ResourcesViewMgr.TILE_MEMORY;
			setActiveToolItemInFilterEventArgs3.ToolItemName = "All Resources";
			SdpApp.EventsManager.Raise<SetActiveToolItemInFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ResetFilterToAll, this, setActiveToolItemInFilterEventArgs3);
			this.m_tileMemoryOnly = false;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		public void OnAllBoundUsedToggled(object sender, ComboBoxSelectionChangedArgs args)
		{
			string displayString = args.Selection.DisplayString;
			if (!(displayString == "All Resources"))
			{
				if (!(displayString == "Bound Only"))
				{
					if (displayString == "Used Only")
					{
						this.m_allBoundUsedSelection = ResourcesViewMgr.AllBoundUsed.Used;
					}
				}
				else
				{
					this.m_allBoundUsedSelection = ResourcesViewMgr.AllBoundUsed.Bound;
				}
			}
			else
			{
				this.m_allBoundUsedSelection = ResourcesViewMgr.AllBoundUsed.All;
			}
			this.ResetFilters();
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive, false);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000BC09 File Offset: 0x00009E09
		public void OnFormatToggled(object sender, ComboBoxSelectionChangedArgs args)
		{
			this.m_formatSelection = args.Selection.DisplayString;
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateImageFormats, false);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000BC24 File Offset: 0x00009E24
		public void OnMarkerTextToggled(object sender, ComboBoxSelectionChangedArgs args)
		{
			this.m_markerSelection = args.Selection.DisplayString;
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker, false);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000BC3F File Offset: 0x00009E3F
		public void OnTileMemoryToggled(object sender, ComboBoxSelectionChangedArgs args)
		{
			this.m_tileMemoryOnly = !string.IsNullOrEmpty(args.Selection.LookupString);
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateTileMemory, false);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000BC64 File Offset: 0x00009E64
		public void AddStatus(SetStatusEventArgs args)
		{
			int num = 0;
			if (args.Status == StatusType.Neutral)
			{
				if (args.StatusText == "Processing Images" && this.m_statusQueue.Count > 0 && this.m_statusQueue[this.m_statusQueue.Count - 1].StatusText == "Processing used resources")
				{
					num = this.m_statusQueue.Count - 1;
				}
				else
				{
					num = this.m_statusQueue.Count;
				}
			}
			this.m_statusQueue.Insert(num, args);
			SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.ResourceViewEvents.SetStatus, this, args);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public void RemoveStatus(SetStatusEventArgs args)
		{
			if (args != null)
			{
				this.m_statusQueue.Remove(args);
				if (this.m_statusQueue.Count > 0)
				{
					SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.ResourceViewEvents.SetStatus, this, Enumerable.First<SetStatusEventArgs>(this.m_statusQueue));
					return;
				}
				SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ResourceViewEvents.HideStatus, this, new EventArgs());
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public void UpdateBinaryData()
		{
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateBinaryData, false);
			this.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate, false);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000BD90 File Offset: 0x00009F90
		private void Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType, bool newCapture = false)
		{
			ResourcesViewMgr.InvalidateClass invalidateClass = new ResourcesViewMgr.InvalidateClass(this, invalidateType, newCapture, this.m_byteBufferGateway);
			invalidateClass.Invalidate();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		private void UpdateHeader(object apiID, object apiName)
		{
			UpdateHeaderEventArgs updateHeaderEventArgs = new UpdateHeaderEventArgs();
			if (apiID != null && apiID.ToString().Length > 0)
			{
				updateHeaderEventArgs.Markup = string.Concat(new string[]
				{
					"<u>ID: ",
					(apiID != null) ? apiID.ToString() : null,
					" | Name: ",
					(apiName != null) ? apiName.ToString() : null,
					"</u>"
				});
			}
			else
			{
				updateHeaderEventArgs.Markup = "<u>Name: " + ((apiName != null) ? apiName.ToString() : null) + "</u>";
			}
			SdpApp.EventsManager.Raise<UpdateHeaderEventArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateHeader, this, updateHeaderEventArgs);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000BE60 File Offset: 0x0000A060
		private string GetPipelineIdFromBindApi(ModelObjectData apiData)
		{
			string[] array = apiData.GetValue("params").Split(new char[] { ',' });
			if (array.Length >= 3)
			{
				return array[2].Split(new char[] { '*' })[1];
			}
			return null;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		private void resourceViewEvents_ItemSelected(object sender, ItemSelectedEventArgs args)
		{
			if (args.SourceID != 353 || args.ResourceIDs.Length == 0)
			{
				if (args.SourceID == 2401 && args.ResourceIDs.Length != 0)
				{
					QGLPlugin.ShaderMgr.DisplayTraceShaderInfo(args.CaptureID, (ulong)args.ResourceIDs[0]);
				}
				return;
			}
			uint num = this.m_selectedApi[(uint)this.m_currentCaptureId];
			Dictionary<VkSnapshotModel.ResourceKey, List<uint>> dictionary;
			List<uint> list;
			if (QGLPlugin.VkSnapshotModel.DrawcallsPerResource.TryGetValue(args.CaptureID, out dictionary) && dictionary.TryGetValue(new VkSnapshotModel.ResourceKey(args.CategoryID, args.ResourceIDs[0]), out list))
			{
				DataExplorerViewSelectRowEventArgs dataExplorerViewSelectRowEventArgs = new DataExplorerViewSelectRowEventArgs();
				dataExplorerViewSelectRowEventArgs.CaptureID = args.CaptureID;
				dataExplorerViewSelectRowEventArgs.SourceID = args.SourceID;
				dataExplorerViewSelectRowEventArgs.RowElement = null;
				dataExplorerViewSelectRowEventArgs.SearchColumn = 0;
				dataExplorerViewSelectRowEventArgs.UniqueID = null;
				dataExplorerViewSelectRowEventArgs.Expand = false;
				dataExplorerViewSelectRowEventArgs.HighlightElements = Enumerable.ToArray<object>(Enumerable.Cast<object>(list));
				if (this.m_allBoundUsedSelection == ResourcesViewMgr.AllBoundUsed.All && !list.Contains(num))
				{
					dataExplorerViewSelectRowEventArgs.RowElement = list[0];
					num = list[0];
				}
				SdpApp.EventsManager.Raise<DataExplorerViewSelectRowEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectRow, this, dataExplorerViewSelectRowEventArgs);
				SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.Refilter, this, null);
			}
			switch (args.CategoryID)
			{
			case 0:
				this.UpdateTextureInfo(args.ResourceIDs[0], args.CaptureID);
				return;
			case 1:
				this.DisplayMemoryBuffers(args.ResourceIDs[0], args.CaptureID);
				return;
			case 2:
				QGLPlugin.ShaderMgr.DisplayShaderModule(args.CaptureID, (ulong)((uint)args.ResourceIDs[0]), num);
				return;
			case 3:
			case 4:
			case 5:
				this.DisplayPipeline(args.ResourceIDs[0], args.CaptureID);
				this.UpdateShaderModules(args.CaptureID);
				QGLPlugin.ShaderMgr.DisplayPipelineStages(args.CaptureID, (ulong)((uint)args.ResourceIDs[0]), num);
				return;
			case 6:
				this.DisplayImageViews(args.ResourceIDs[0], args.CaptureID);
				return;
			case 7:
				this.DisplayDescriptorSets(args.ResourceIDs[0], args.CaptureID);
				return;
			case 8:
				this.DisplayDescriptorSetLayouts(args.ResourceIDs[0], args.CaptureID);
				return;
			case 9:
				this.DisplayAccelerationStructureInfo(args.ResourceIDs[0], args.CaptureID);
				return;
			case 10:
				QGLPlugin.ShaderMgr.DisplayTraversalShader(args.CaptureID, (ulong)((uint)args.ResourceIDs[0]));
				return;
			case 11:
				this.DisplayTensors(args.ResourceIDs[0], args.CaptureID);
				return;
			case 12:
				this.DisplayTensorViews(args.ResourceIDs[0], args.CaptureID);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000C160 File Offset: 0x0000A360
		private void resourceViewEvents_ItemDoubleClicked(object sender, ItemDoubleClickedEventArgs args)
		{
			if (args.SourceID == 353)
			{
				ResourcesViewMgr.ResourceCategory categoryID = (ResourcesViewMgr.ResourceCategory)args.CategoryID;
				if (categoryID == ResourcesViewMgr.ResourceCategory.AccelerationStructureInfo)
				{
					this.DisplayAccelerationStructure(args.ResourceID, args.CaptureID);
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000C198 File Offset: 0x0000A398
		private void AddShaderToProps(List<PropertyDescriptor> props, ModelObjectData shaderStage, ref uint stageIndex, string stagesCat)
		{
			string text = "Stage[" + stageIndex.ToString() + "]:";
			stageIndex += 1U;
			ulong num = Uint64Converter.Convert(shaderStage.GetValue("shaderModuleID"));
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<ulong>(text + "shaderModuleID", typeof(ulong), num, stagesCat, text + "shaderModuleID", true);
			props.Add(propertyDescriptor);
			VkShaderStageFlagBits vkShaderStageFlagBits = (VkShaderStageFlagBits)UintConverter.Convert(shaderStage.GetValue("stageType"));
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkShaderStageFlagBits>(text + "stageType", typeof(VkShaderStageFlagBits), vkShaderStageFlagBits, stagesCat, text + "stageType", true);
			props.Add(propertyDescriptor2);
			string value = shaderStage.GetValue("pName");
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<string>(text + "entryPoint", typeof(string), value, stagesCat, text + "entryPoint", true);
			props.Add(propertyDescriptor3);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000C288 File Offset: 0x0000A488
		private void DisplayPipeline(long pipelineID, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotGraphicsPipelines");
			ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotComputePipelines");
			ModelObject modelObject3 = dataModel.GetModelObject(model, "VulkanSnapshotRaytracingPipelines");
			ModelObject modelObject4 = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLibraries");
			ModelObject modelObject5 = dataModel.GetModelObject(model, "VulkanSnapshotShaderStages");
			ModelObject modelObject6 = dataModel.GetModelObject(model, "VulkanSnapshotInputBindingDescriptions");
			ModelObject modelObject7 = dataModel.GetModelObject(model, "VulkanSnapshotInputAttributeDescriptions");
			ModelObject modelObject8 = dataModel.GetModelObject(model, "VulkanSnapshotInputAssemblyStates");
			ModelObject modelObject9 = dataModel.GetModelObject(model, "VulkanSnapshotTesselationStates");
			ModelObject modelObject10 = dataModel.GetModelObject(model, "VulkanSnapshotViewPorts");
			ModelObject modelObject11 = dataModel.GetModelObject(model, "VulkanSnapshotViewPortScissors");
			ModelObject modelObject12 = dataModel.GetModelObject(model, "VulkanSnapshotRasterizationStates");
			ModelObject modelObject13 = dataModel.GetModelObject(model, "VulkanSnapshotMultiSampleStates");
			ModelObject modelObject14 = dataModel.GetModelObject(model, "VulkanSnapshotDepthStencilStates");
			ModelObject modelObject15 = dataModel.GetModelObject(model, "VulkanSnapshotColorBlendStates");
			ModelObject modelObject16 = dataModel.GetModelObject(model, "VulkanSnapshotColorBlendAttachmentState");
			ModelObject modelObject17 = dataModel.GetModelObject(model, "VulkanSnapshotColorBlendConstants");
			ModelObject modelObject18 = dataModel.GetModelObject(model, "VulkanSnapshotDynamicStates");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				pipelineID.ToString()
			});
			ModelObjectDataList data2 = modelObject2.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				pipelineID.ToString()
			});
			ModelObjectDataList data3 = modelObject3.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				pipelineID.ToString()
			});
			if (data.Count + data2.Count + data3.Count != 1)
			{
				return;
			}
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			ModelObjectData modelObjectData;
			string text;
			if (data.Count == 1)
			{
				modelObjectData = data[0];
				text = "Graphics";
			}
			else if (data2.Count == 1)
			{
				modelObjectData = data2[0];
				text = "Compute";
			}
			else
			{
				modelObjectData = data3[0];
				text = "Raytracing";
			}
			ulong num = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
			string text2 = string.Format("{0} Pipeline {1}", text, num);
			ulong num2 = Uint64Converter.Convert(modelObjectData.GetValue("pipelineCacheID"));
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("pipelineCacheID", typeof(ulong), num2.ToString(), text2, "pipelineCacheID", true);
			list.Add(propertyDescriptor);
			ModelObjectDataList data4 = modelObject5.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			string text3 = string.Format("{0} Pipeline {1} Stages", text, num);
			uint num3 = 0U;
			foreach (ModelObjectData modelObjectData2 in data4)
			{
				this.AddShaderToProps(list, modelObjectData2, ref num3, text3);
			}
			HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed(captureId, (ulong)pipelineID, modelObject4);
			foreach (ulong num4 in pipelineLibraryUsed)
			{
				ModelObjectDataList data5 = modelObject5.GetData(new StringList
				{
					"captureID",
					captureId.ToString(),
					"pipelineID",
					num4.ToString()
				});
				foreach (ModelObjectData modelObjectData3 in data5)
				{
					this.AddShaderToProps(list, modelObjectData3, ref num3, text3);
				}
			}
			string text4 = string.Format("{0} Pipeline {1} Vertex Input State", text, num);
			ModelObjectDataList data6 = modelObject6.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			uint num5 = 0U;
			foreach (ModelObjectData modelObjectData4 in data6)
			{
				string text5 = "BindingDesc[" + num5.ToString() + "]:";
				num5 += 1U;
				uint num6 = UintConverter.Convert(modelObjectData4.GetValue("binding"));
				PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<uint>(text5 + "binding", typeof(uint), num6, text4, text5 + "binding", true);
				list.Add(propertyDescriptor2);
				uint num7 = UintConverter.Convert(modelObjectData4.GetValue("stride"));
				PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<uint>(text5 + "stride", typeof(uint), num7, text4, text5 + "stride", true);
				list.Add(propertyDescriptor3);
				VkVertexInputRate vkVertexInputRate = (VkVertexInputRate)UintConverter.Convert(modelObjectData4.GetValue("binding"));
				PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<VkVertexInputRate>(text5 + "inputRate", typeof(VkVertexInputRate), vkVertexInputRate, text4, text5 + "inputRate", true);
				list.Add(propertyDescriptor4);
			}
			ModelObjectDataList data7 = modelObject7.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			uint num8 = 0U;
			foreach (ModelObjectData modelObjectData5 in data7)
			{
				string text6 = "AttributeIndexDesc[" + num8.ToString() + "]:";
				num8 += 1U;
				uint num9 = UintConverter.Convert(modelObjectData5.GetValue("location"));
				PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<uint>(text6 + "location", typeof(uint), num9, text4, text6 + "location", true);
				list.Add(propertyDescriptor5);
				uint num10 = UintConverter.Convert(modelObjectData5.GetValue("binding"));
				PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<uint>(text6 + "binding", typeof(uint), num10, text4, text6 + "binding", true);
				list.Add(propertyDescriptor6);
				VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData5.GetValue("format"));
				PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<VkFormats>(text6 + "format", typeof(VkFormats), vkFormats, text4, text6 + "format", true);
				list.Add(propertyDescriptor7);
				uint num11 = UintConverter.Convert(modelObjectData5.GetValue("offset"));
				PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<uint>(text6 + "offset", typeof(uint), num11, text4, text6 + "offset", true);
				list.Add(propertyDescriptor8);
			}
			string text7 = string.Format("{0} Pipeline {1} InputAssembly State", text, num);
			ModelObjectDataList data8 = modelObject8.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data8.Count == 1)
			{
				ModelObjectData modelObjectData6 = data8[0];
				VkPrimitiveTopology vkPrimitiveTopology = (VkPrimitiveTopology)UintConverter.Convert(modelObjectData6.GetValue("topology"));
				PropertyDescriptor propertyDescriptor9 = new SdpPropertyDescriptor<VkPrimitiveTopology>("topology", typeof(VkPrimitiveTopology), vkPrimitiveTopology, text7, "topology", true);
				list.Add(propertyDescriptor9);
				VkBool32 vkBool = (VkBool32)UintConverter.Convert(modelObjectData6.GetValue("primitiveRestartEnable"));
				PropertyDescriptor propertyDescriptor10 = new SdpPropertyDescriptor<VkBool32>("primitiveRestartEnable", typeof(VkBool32), vkBool, text7, "primitiveRestartEnable", true);
				list.Add(propertyDescriptor10);
			}
			ModelObjectDataList data9 = modelObject9.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			string text8 = string.Format("{0} Pipeline {1} Tesselation State", text, num);
			if (data9.Count == 1)
			{
				ModelObjectData modelObjectData7 = data9[0];
				string text9 = "TesselationState:";
				uint num12 = UintConverter.Convert(modelObjectData7.GetValue("patchControlPoints"));
				PropertyDescriptor propertyDescriptor11 = new SdpPropertyDescriptor<uint>(text9 + "patchControlPoints", typeof(uint), num12, text8, text9 + "patchControlPoints", true);
				list.Add(propertyDescriptor11);
			}
			string text10 = string.Format("{0} Pipeline {1} Viewport State", text, num);
			ModelObjectDataList data10 = modelObject10.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			uint num13 = 0U;
			foreach (ModelObjectData modelObjectData8 in data10)
			{
				string text11 = "ViewPort[" + num13.ToString() + "]:";
				num13 += 1U;
				float num14 = FloatConverter.Convert(modelObjectData8.GetValue("x"));
				PropertyDescriptor propertyDescriptor12 = new SdpPropertyDescriptor<float>(text11 + "x", typeof(float), num14, text10, text11 + "x", true);
				list.Add(propertyDescriptor12);
				float num15 = FloatConverter.Convert(modelObjectData8.GetValue("y"));
				PropertyDescriptor propertyDescriptor13 = new SdpPropertyDescriptor<float>(text11 + "y", typeof(uint), num15, text10, text11 + "y", true);
				list.Add(propertyDescriptor13);
				float num16 = FloatConverter.Convert(modelObjectData8.GetValue("width"));
				PropertyDescriptor propertyDescriptor14 = new SdpPropertyDescriptor<float>(text11 + "width", typeof(float), num16, text10, text11 + "width", true);
				list.Add(propertyDescriptor14);
				float num17 = FloatConverter.Convert(modelObjectData8.GetValue("height"));
				PropertyDescriptor propertyDescriptor15 = new SdpPropertyDescriptor<float>(text11 + "height", typeof(float), num17, text10, text11 + "height", true);
				list.Add(propertyDescriptor15);
				float num18 = FloatConverter.Convert(modelObjectData8.GetValue("minDepth"));
				PropertyDescriptor propertyDescriptor16 = new SdpPropertyDescriptor<float>(text11 + "minDepth", typeof(float), num18, text10, text11 + "minDepth", true);
				list.Add(propertyDescriptor16);
				float num19 = FloatConverter.Convert(modelObjectData8.GetValue("maxDepth"));
				PropertyDescriptor propertyDescriptor17 = new SdpPropertyDescriptor<float>(text11 + "maxDepth", typeof(float), num19, text10, text11 + "maxDepth", true);
				list.Add(propertyDescriptor17);
			}
			ModelObjectDataList data11 = modelObject11.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			uint num20 = 0U;
			foreach (ModelObjectData modelObjectData9 in data11)
			{
				string text12 = "Scissor[" + num20.ToString() + "]:";
				num20 += 1U;
				uint num21 = UintConverter.Convert(modelObjectData9.GetValue("x"));
				PropertyDescriptor propertyDescriptor18 = new SdpPropertyDescriptor<uint>(text12 + "x", typeof(uint), num21, text10, text12 + "x", true);
				list.Add(propertyDescriptor18);
				uint num22 = UintConverter.Convert(modelObjectData9.GetValue("y"));
				PropertyDescriptor propertyDescriptor19 = new SdpPropertyDescriptor<uint>(text12 + "y", typeof(uint), num22, text10, text12 + "y", true);
				list.Add(propertyDescriptor19);
				uint num23 = UintConverter.Convert(modelObjectData9.GetValue("width"));
				PropertyDescriptor propertyDescriptor20 = new SdpPropertyDescriptor<uint>(text12 + "width", typeof(uint), num23, text10, text12 + "width", true);
				list.Add(propertyDescriptor20);
				uint num24 = UintConverter.Convert(modelObjectData9.GetValue("height"));
				PropertyDescriptor propertyDescriptor21 = new SdpPropertyDescriptor<uint>(text12 + "height", typeof(uint), num24, text10, text12 + "height", true);
				list.Add(propertyDescriptor21);
			}
			string text13 = string.Format("{0} Pipeline {1} Rasterization State", text, num);
			ModelObjectDataList data12 = modelObject12.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data12.Count == 1)
			{
				ModelObjectData modelObjectData10 = data12[0];
				VkBool32 vkBool2 = (VkBool32)UintConverter.Convert(modelObjectData10.GetValue("depthClampEnable"));
				PropertyDescriptor propertyDescriptor22 = new SdpPropertyDescriptor<VkBool32>("depthClampEnable", typeof(VkBool32), vkBool2, text13, "depthClampEnable", true);
				list.Add(propertyDescriptor22);
				VkBool32 vkBool3 = (VkBool32)UintConverter.Convert(modelObjectData10.GetValue("rasterizerDiscardEnable"));
				PropertyDescriptor propertyDescriptor23 = new SdpPropertyDescriptor<VkBool32>("rasterizationDiscardEnable", typeof(VkBool32), vkBool3, text13, "rasterizationDiscardEnable", true);
				list.Add(propertyDescriptor23);
				VkPolygonMode vkPolygonMode = (VkPolygonMode)UintConverter.Convert(modelObjectData10.GetValue("polygonMode"));
				PropertyDescriptor propertyDescriptor24 = new SdpPropertyDescriptor<VkPolygonMode>("polygonMode", typeof(VkPolygonMode), vkPolygonMode, text13, "polygonMode", true);
				list.Add(propertyDescriptor24);
				VkCullModeFlagBits vkCullModeFlagBits = (VkCullModeFlagBits)UintConverter.Convert(modelObjectData10.GetValue("cullMode"));
				PropertyDescriptor propertyDescriptor25 = new SdpPropertyDescriptor<VkCullModeFlagBits>("cullMode", typeof(VkCullModeFlagBits), vkCullModeFlagBits, text13, "cullMode", true);
				list.Add(propertyDescriptor25);
				VkFrontFace vkFrontFace = (VkFrontFace)UintConverter.Convert(modelObjectData10.GetValue("frontFace"));
				PropertyDescriptor propertyDescriptor26 = new SdpPropertyDescriptor<VkFrontFace>("frontFace", typeof(VkFrontFace), vkFrontFace, text13, "frontFace", true);
				list.Add(propertyDescriptor26);
				VkBool32 vkBool4 = (VkBool32)UintConverter.Convert(modelObjectData10.GetValue("depthBiasEnable"));
				PropertyDescriptor propertyDescriptor27 = new SdpPropertyDescriptor<VkBool32>("depthBiasEnable", typeof(VkBool32), vkBool4, text13, "depthBiasEnable", true);
				list.Add(propertyDescriptor27);
				float num25 = FloatConverter.Convert(modelObjectData10.GetValue("depthBiasConstantFactor"));
				PropertyDescriptor propertyDescriptor28 = new SdpPropertyDescriptor<float>("depthBiasConstantFactor", typeof(float), num25, text13, "depthBiasConstantFactor", true);
				list.Add(propertyDescriptor28);
				float num26 = FloatConverter.Convert(modelObjectData10.GetValue("depthBiasClamp"));
				PropertyDescriptor propertyDescriptor29 = new SdpPropertyDescriptor<float>("depthBiasClamp", typeof(float), num26, text13, "depthBiasClamp", true);
				list.Add(propertyDescriptor29);
				float num27 = FloatConverter.Convert(modelObjectData10.GetValue("depthBiasSlopeFactor"));
				PropertyDescriptor propertyDescriptor30 = new SdpPropertyDescriptor<float>("depthBiasSlopeFactor", typeof(float), num27, text13, "depthBiasSlopeFactor", true);
				list.Add(propertyDescriptor30);
				float num28 = FloatConverter.Convert(modelObjectData10.GetValue("lineWidth"));
				PropertyDescriptor propertyDescriptor31 = new SdpPropertyDescriptor<float>("lineWidth", typeof(float), num28, text13, "lineWidth", true);
				list.Add(propertyDescriptor31);
			}
			string text14 = string.Format("{0} Pipeline {1} MultiSample State", text, num);
			ModelObjectDataList data13 = modelObject13.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data13.Count == 1)
			{
				ModelObjectData modelObjectData11 = data13[0];
				VkSampleCountFlagBits vkSampleCountFlagBits = (VkSampleCountFlagBits)UintConverter.Convert(modelObjectData11.GetValue("rasterizationSamples"));
				PropertyDescriptor propertyDescriptor32 = new SdpPropertyDescriptor<VkSampleCountFlagBits>("rasterizationSamples", typeof(VkSampleCountFlagBits), vkSampleCountFlagBits, text14, "rasterizationSamples", true);
				list.Add(propertyDescriptor32);
				VkBool32 vkBool5 = (VkBool32)UintConverter.Convert(modelObjectData11.GetValue("sampleShadingEnable"));
				PropertyDescriptor propertyDescriptor33 = new SdpPropertyDescriptor<VkBool32>("sampleShadingEnable", typeof(VkBool32), vkBool5, text14, "sampleShadingEnable", true);
				list.Add(propertyDescriptor33);
				float num29 = FloatConverter.Convert(modelObjectData11.GetValue("minSampleShading"));
				PropertyDescriptor propertyDescriptor34 = new SdpPropertyDescriptor<float>("minSampleShading", typeof(float), num29, text14, "minSampleShading", true);
				list.Add(propertyDescriptor34);
				string text15 = "0x" + UintConverter.Convert(modelObjectData11.GetValue("pSampleMask")).ToString("X");
				PropertyDescriptor propertyDescriptor35 = new SdpPropertyDescriptor<string>("pSampleMask", typeof(string), text15, text14, "pSampleMask", true);
				list.Add(propertyDescriptor35);
				VkBool32 vkBool6 = (VkBool32)UintConverter.Convert(modelObjectData11.GetValue("alphaToCoverageEnable"));
				PropertyDescriptor propertyDescriptor36 = new SdpPropertyDescriptor<VkBool32>("alphaToCoverageEnable", typeof(VkBool32), vkBool6, text14, "alphaToCoverageEnable", true);
				list.Add(propertyDescriptor36);
				VkBool32 vkBool7 = (VkBool32)UintConverter.Convert(modelObjectData11.GetValue("alphaToOneEnable"));
				PropertyDescriptor propertyDescriptor37 = new SdpPropertyDescriptor<VkBool32>("alphaToOneEnable", typeof(VkBool32), vkBool7, text14, "alphaToOneEnable", true);
				list.Add(propertyDescriptor37);
			}
			string text16 = string.Format("{0} Pipeline {1} Depth Stencil State", text, num);
			ModelObjectDataList data14 = modelObject14.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data14.Count == 1)
			{
				ModelObjectData modelObjectData12 = data14[0];
				VkBool32 vkBool8 = (VkBool32)UintConverter.Convert(modelObjectData12.GetValue("depthTestEnable"));
				PropertyDescriptor propertyDescriptor38 = new SdpPropertyDescriptor<VkBool32>("depthTestEnable", typeof(VkBool32), vkBool8, text16, "depthTestEnable", true);
				list.Add(propertyDescriptor38);
				VkBool32 vkBool9 = (VkBool32)UintConverter.Convert(modelObjectData12.GetValue("depthWriteEnable"));
				PropertyDescriptor propertyDescriptor39 = new SdpPropertyDescriptor<VkBool32>("depthTestWriteEnable", typeof(VkBool32), vkBool9, text16, "depthTestWriteEnable", true);
				list.Add(propertyDescriptor39);
				VkCompareOp vkCompareOp = (VkCompareOp)UintConverter.Convert(modelObjectData12.GetValue("depthCompareOp"));
				PropertyDescriptor propertyDescriptor40 = new SdpPropertyDescriptor<VkCompareOp>("depthCompareOp", typeof(VkCompareOp), vkCompareOp, text16, "depthCompareOp", true);
				list.Add(propertyDescriptor40);
				VkBool32 vkBool10 = (VkBool32)UintConverter.Convert(modelObjectData12.GetValue("depthBoundsTestEnable"));
				PropertyDescriptor propertyDescriptor41 = new SdpPropertyDescriptor<VkBool32>("depthBoundsTestEnable", typeof(VkBool32), vkBool10, text16, "depthBoundsTestEnable", true);
				list.Add(propertyDescriptor41);
				VkBool32 vkBool11 = (VkBool32)UintConverter.Convert(modelObjectData12.GetValue("stencilTestEnable"));
				PropertyDescriptor propertyDescriptor42 = new SdpPropertyDescriptor<VkBool32>("stencilTestEnable", typeof(VkBool32), vkBool11, text16, "stencilTestEnable", true);
				list.Add(propertyDescriptor42);
				VkStencilOp vkStencilOp = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("front_failOp"));
				PropertyDescriptor propertyDescriptor43 = new SdpPropertyDescriptor<VkStencilOp>("Front:failOp", typeof(VkStencilOp), vkStencilOp, text16, "Front:failOp", true);
				list.Add(propertyDescriptor43);
				VkStencilOp vkStencilOp2 = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("front_passOp"));
				PropertyDescriptor propertyDescriptor44 = new SdpPropertyDescriptor<VkStencilOp>("Front:passOp", typeof(VkStencilOp), vkStencilOp2, text16, "Front:passOp", true);
				list.Add(propertyDescriptor44);
				VkStencilOp vkStencilOp3 = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("front_depthFailOp"));
				PropertyDescriptor propertyDescriptor45 = new SdpPropertyDescriptor<VkStencilOp>("Front:depthFailOp", typeof(VkStencilOp), vkStencilOp3, text16, "Front:depthFailOp", true);
				list.Add(propertyDescriptor45);
				VkCompareOp vkCompareOp2 = (VkCompareOp)UintConverter.Convert(modelObjectData12.GetValue("front_compareOp"));
				PropertyDescriptor propertyDescriptor46 = new SdpPropertyDescriptor<VkCompareOp>("Front:compareOp", typeof(VkCompareOp), vkCompareOp2, text16, "Front:compareOp", true);
				list.Add(propertyDescriptor46);
				uint num30 = UintConverter.Convert(modelObjectData12.GetValue("front_compareMask"));
				PropertyDescriptor propertyDescriptor47 = new SdpPropertyDescriptor<uint>("Front:compareMask", typeof(uint), num30, text16, "Front:compareMask", true);
				list.Add(propertyDescriptor47);
				uint num31 = UintConverter.Convert(modelObjectData12.GetValue("front_writeMask"));
				PropertyDescriptor propertyDescriptor48 = new SdpPropertyDescriptor<uint>("Front:writeMask", typeof(uint), num31, text16, "Front:writeMask", true);
				list.Add(propertyDescriptor48);
				uint num32 = UintConverter.Convert(modelObjectData12.GetValue("front_reference"));
				PropertyDescriptor propertyDescriptor49 = new SdpPropertyDescriptor<uint>("Front:reference", typeof(uint), num32, text16, "Front:reference", true);
				list.Add(propertyDescriptor49);
				VkStencilOp vkStencilOp4 = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("back_failOp"));
				PropertyDescriptor propertyDescriptor50 = new SdpPropertyDescriptor<VkStencilOp>("Back:failOp", typeof(VkStencilOp), vkStencilOp4, text16, "Back:failOp", true);
				list.Add(propertyDescriptor50);
				VkStencilOp vkStencilOp5 = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("back_passOp"));
				PropertyDescriptor propertyDescriptor51 = new SdpPropertyDescriptor<VkStencilOp>("Back:passOp", typeof(VkStencilOp), vkStencilOp5, text16, "Back:passOp", true);
				list.Add(propertyDescriptor51);
				VkStencilOp vkStencilOp6 = (VkStencilOp)UintConverter.Convert(modelObjectData12.GetValue("back_depthFailOp"));
				PropertyDescriptor propertyDescriptor52 = new SdpPropertyDescriptor<VkStencilOp>("Back:depthFailOp", typeof(VkStencilOp), vkStencilOp6, text16, "Back:depthFailOp", true);
				list.Add(propertyDescriptor52);
				VkCompareOp vkCompareOp3 = (VkCompareOp)UintConverter.Convert(modelObjectData12.GetValue("back_compareOp"));
				PropertyDescriptor propertyDescriptor53 = new SdpPropertyDescriptor<VkCompareOp>("Back:compareOp", typeof(VkCompareOp), vkCompareOp3, text16, "Back:compareOp", true);
				list.Add(propertyDescriptor53);
				uint num33 = UintConverter.Convert(modelObjectData12.GetValue("back_compareMask"));
				PropertyDescriptor propertyDescriptor54 = new SdpPropertyDescriptor<uint>("Back:compareMask", typeof(uint), num33, text16, "Back:compareMask", true);
				list.Add(propertyDescriptor54);
				uint num34 = UintConverter.Convert(modelObjectData12.GetValue("back_writeMask"));
				PropertyDescriptor propertyDescriptor55 = new SdpPropertyDescriptor<uint>("Back:writeMask", typeof(uint), num34, text16, "Back:writeMask", true);
				list.Add(propertyDescriptor55);
				uint num35 = UintConverter.Convert(modelObjectData12.GetValue("back_reference"));
				PropertyDescriptor propertyDescriptor56 = new SdpPropertyDescriptor<uint>("Back:reference", typeof(uint), num35, text16, "Back:reference", true);
				list.Add(propertyDescriptor56);
				float num36 = FloatConverter.Convert(modelObjectData12.GetValue("minDepthBounds"));
				PropertyDescriptor propertyDescriptor57 = new SdpPropertyDescriptor<float>("minDepthBounds", typeof(float), num36, text16, "minDepthBounds", true);
				list.Add(propertyDescriptor57);
				float num37 = FloatConverter.Convert(modelObjectData12.GetValue("maxDepthBounds"));
				PropertyDescriptor propertyDescriptor58 = new SdpPropertyDescriptor<float>("maxDepthBounds", typeof(float), num37, text16, "maxDepthBounds", true);
				list.Add(propertyDescriptor58);
			}
			string text17 = string.Format("{0} Pipeline {1} Color Blend State", text, num);
			ModelObjectDataList data15 = modelObject15.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			if (data15.Count == 1)
			{
				ModelObjectData modelObjectData13 = data15[0];
				VkBool32 vkBool12 = (VkBool32)UintConverter.Convert(modelObjectData13.GetValue("logicOpEnable"));
				PropertyDescriptor propertyDescriptor59 = new SdpPropertyDescriptor<VkBool32>("logicOpEnable", typeof(VkBool32), vkBool12, text17, "logicOpEnable", true);
				list.Add(propertyDescriptor59);
				VkLogicOp vkLogicOp = (VkLogicOp)UintConverter.Convert(modelObjectData13.GetValue("logicOp"));
				PropertyDescriptor propertyDescriptor60 = new SdpPropertyDescriptor<VkLogicOp>("logicOp", typeof(VkLogicOp), vkLogicOp, text17, "logicOp", true);
				list.Add(propertyDescriptor60);
				ModelObjectDataList data16 = modelObject16.GetData(new StringList
				{
					"captureID",
					captureId.ToString(),
					"pipelineID",
					pipelineID.ToString()
				});
				string text18 = "Attachment[";
				uint num38 = 0U;
				foreach (ModelObjectData modelObjectData14 in data16)
				{
					string text19 = text18 + num38.ToString() + "]:";
					num38 += 1U;
					VkBool32 vkBool13 = (VkBool32)UintConverter.Convert(modelObjectData14.GetValue("blendEnable"));
					PropertyDescriptor propertyDescriptor61 = new SdpPropertyDescriptor<VkBool32>(text19 + "blendEnable", typeof(VkBool32), vkBool13, text17, text19 + "blendEnable", true);
					list.Add(propertyDescriptor61);
					VkBlendFactor vkBlendFactor = (VkBlendFactor)UintConverter.Convert(modelObjectData14.GetValue("srcColorBlendFactor"));
					PropertyDescriptor propertyDescriptor62 = new SdpPropertyDescriptor<VkBlendFactor>(text19 + "srcColorBlendFactor", typeof(VkBlendFactor), vkBlendFactor, text17, text19 + "srcColorBlendFactor", true);
					list.Add(propertyDescriptor62);
					VkBlendFactor vkBlendFactor2 = (VkBlendFactor)UintConverter.Convert(modelObjectData14.GetValue("dstColorBlendFactor"));
					PropertyDescriptor propertyDescriptor63 = new SdpPropertyDescriptor<VkBlendFactor>(text19 + "dstColorBlendFactor", typeof(VkBlendFactor), vkBlendFactor2, text17, text19 + "dstColorBlendFactor", true);
					list.Add(propertyDescriptor63);
					VkBlendOp vkBlendOp = (VkBlendOp)UintConverter.Convert(modelObjectData14.GetValue("colorBlendOp"));
					PropertyDescriptor propertyDescriptor64 = new SdpPropertyDescriptor<VkBlendOp>(text19 + "colorBlendOp", typeof(VkBlendOp), vkBlendOp, text17, text19 + "colorBlendOp", true);
					list.Add(propertyDescriptor64);
					VkBlendFactor vkBlendFactor3 = (VkBlendFactor)UintConverter.Convert(modelObjectData14.GetValue("srcAlphaBlendFactor"));
					PropertyDescriptor propertyDescriptor65 = new SdpPropertyDescriptor<VkBlendFactor>(text19 + "srcAlphaBlendFactor", typeof(VkBlendFactor), vkBlendFactor3, text17, text19 + "srcAlphaBlendFactor", true);
					list.Add(propertyDescriptor65);
					VkBlendFactor vkBlendFactor4 = (VkBlendFactor)UintConverter.Convert(modelObjectData14.GetValue("dstAlphaBlendFactor"));
					PropertyDescriptor propertyDescriptor66 = new SdpPropertyDescriptor<VkBlendFactor>(text19 + "dstAlphaBlendFactor", typeof(VkBlendFactor), vkBlendFactor4, text17, text19 + "dstAlphaBlendFactor", true);
					list.Add(propertyDescriptor66);
					VkBlendOp vkBlendOp2 = (VkBlendOp)UintConverter.Convert(modelObjectData14.GetValue("alphaBlendOp"));
					PropertyDescriptor propertyDescriptor67 = new SdpPropertyDescriptor<VkBlendOp>(text19 + "alphaBlendOp", typeof(VkBlendOp), vkBlendOp2, text17, text19 + "alphaBlendOp", true);
					list.Add(propertyDescriptor67);
					uint num39 = UintConverter.Convert(modelObjectData14.GetValue("colorWriteMask"));
					string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkColorComponentFlagBits>(num39);
					PropertyDescriptor propertyDescriptor68 = new SdpPropertyDescriptor<string>(text19 + "colorWriteMask", typeof(string), flagsEnumStr, text17, text19 + "colorWriteMask", true);
					list.Add(propertyDescriptor68);
				}
				ModelObjectDataList data17 = modelObject17.GetData(new StringList
				{
					"captureID",
					captureId.ToString(),
					"pipelineID",
					pipelineID.ToString()
				});
				string text20 = "BlendConstants[";
				uint num40 = 0U;
				foreach (ModelObjectData modelObjectData15 in data17)
				{
					string text21 = text20 + num40.ToString() + "]:";
					num40 += 1U;
					float num41 = FloatConverter.Convert(modelObjectData15.GetValue("value"));
					PropertyDescriptor propertyDescriptor69 = new SdpPropertyDescriptor<float>(text21 + "value", typeof(float), num41, text17, text21 + "value", true);
					list.Add(propertyDescriptor69);
				}
			}
			string text22 = string.Format("{0} Pipeline {1} Color Dynamic State", text, num);
			ModelObjectDataList data18 = modelObject18.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"pipelineID",
				pipelineID.ToString()
			});
			uint num42 = 0U;
			foreach (ModelObjectData modelObjectData16 in data18)
			{
				string text23 = "DynamicState[" + num42.ToString() + "]:";
				num42 += 1U;
				VkDynamicState vkDynamicState = (VkDynamicState)UintConverter.Convert(modelObjectData16.GetValue("state"));
				PropertyDescriptor propertyDescriptor70 = new SdpPropertyDescriptor<VkDynamicState>(text23 + "state", typeof(VkDynamicState), vkDynamicState, text22, text23 + "state", true);
				list.Add(propertyDescriptor70);
			}
			ulong num43 = Uint64Converter.Convert(modelObjectData.GetValue("layoutID"));
			PropertyDescriptor propertyDescriptor71 = new SdpPropertyDescriptor<string>("layoutID", typeof(ulong), num43.ToString(), text2, "layoutID", true);
			list.Add(propertyDescriptor71);
			if (data.Count == 1)
			{
				ulong num44 = Uint64Converter.Convert(modelObjectData.GetValue("renderPass"));
				PropertyDescriptor propertyDescriptor72 = new SdpPropertyDescriptor<ulong>("renderPass", typeof(ulong), num44, text2, "renderPass", true);
				list.Add(propertyDescriptor72);
				uint num45 = UintConverter.Convert(modelObjectData.GetValue("subpass"));
				PropertyDescriptor propertyDescriptor73 = new SdpPropertyDescriptor<uint>("subpass", typeof(uint), num45, text2, "subpass", true);
				list.Add(propertyDescriptor73);
			}
			ulong num46 = Uint64Converter.Convert(modelObjectData.GetValue("basePipelineHandle"));
			PropertyDescriptor propertyDescriptor74 = new SdpPropertyDescriptor<ulong>("basePipelineHandle", typeof(ulong), num46, text2, "basePipelineHandle", true);
			list.Add(propertyDescriptor74);
			uint num47 = UintConverter.Convert(modelObjectData.GetValue("basePipelineIndex"));
			PropertyDescriptor propertyDescriptor75 = new SdpPropertyDescriptor<uint>("basePipelineIndex", typeof(uint), num47, text2, "basePipelineIndex", true);
			list.Add(propertyDescriptor75);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "Pipeline";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			GC.KeepAlive(data);
			GC.KeepAlive(data2);
			GC.KeepAlive(data3);
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
			setStatusEventArgs.Status = StatusType.Neutral;
			setStatusEventArgs.StatusText = string.Format("Retrieving info for Pipeline ID: {0}", pipelineID);
			setStatusEventArgs.Duration = 0;
			SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.ProgramViewEvents.SetStatus, null, setStatusEventArgs);
			this.m_uiRequestHandler.UpdateProgramInspector(pipelineID, this.m_byteBufferGateway, this.m_selectedDrawCallID[(uint)captureId], this.m_selectedApi[(uint)captureId]);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000E208 File Offset: 0x0000C408
		private void DisplayMemoryBuffers(long resourceHash, int captureId)
		{
			uint num = (uint)(resourceHash & (long)((ulong)(-1)));
			uint num2 = (uint)((ulong)(resourceHash & -4294967296L) >> 32);
			IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer(captureId, (ulong)num, num2);
			if (byteBuffer == null)
			{
				return;
			}
			BufferViewerInvalidateEventArgs bufferViewerInvalidateEventArgs = new BufferViewerInvalidateEventArgs();
			bufferViewerInvalidateEventArgs.Buffer = new byte[byteBuffer.BDP.size];
			Marshal.Copy(byteBuffer.BDP.data, bufferViewerInvalidateEventArgs.Buffer, 0, (int)byteBuffer.BDP.size);
			SdpApp.EventsManager.Raise<BufferViewerInvalidateEventArgs>(SdpApp.EventsManager.BufferViewerEvents.Invalidate, this, bufferViewerInvalidateEventArgs);
			if (num2 != 4294967295U)
			{
				DataExplorerViewSelectRowEventArgs dataExplorerViewSelectRowEventArgs = new DataExplorerViewSelectRowEventArgs();
				dataExplorerViewSelectRowEventArgs.SourceID = 353;
				dataExplorerViewSelectRowEventArgs.CaptureID = captureId;
				dataExplorerViewSelectRowEventArgs.RowElement = num2;
				dataExplorerViewSelectRowEventArgs.SearchColumn = 0;
				SdpApp.EventsManager.Raise<DataExplorerViewSelectRowEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectRow, this, dataExplorerViewSelectRowEventArgs);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
		private void DisplayImageViews(long resourceID, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotImageViews");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				resourceID.ToString()
			});
			if (data.Count != 1)
			{
				return;
			}
			ModelObjectData modelObjectData = data[0];
			ulong num = Uint64Converter.Convert(modelObjectData.GetValue("imageID"));
			VkImageViewType vkImageViewType = (VkImageViewType)UintConverter.Convert(modelObjectData.GetValue("viewType"));
			VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData.GetValue("format"));
			VkComponentSwizzle vkComponentSwizzle = (VkComponentSwizzle)UintConverter.Convert(modelObjectData.GetValue("componentR"));
			VkComponentSwizzle vkComponentSwizzle2 = (VkComponentSwizzle)UintConverter.Convert(modelObjectData.GetValue("componentG"));
			VkComponentSwizzle vkComponentSwizzle3 = (VkComponentSwizzle)UintConverter.Convert(modelObjectData.GetValue("componentB"));
			VkComponentSwizzle vkComponentSwizzle4 = (VkComponentSwizzle)UintConverter.Convert(modelObjectData.GetValue("componentA"));
			uint num2 = UintConverter.Convert(modelObjectData.GetValue("aspectMask"));
			uint num3 = UintConverter.Convert(modelObjectData.GetValue("baseMipLevel"));
			uint num4 = UintConverter.Convert(modelObjectData.GetValue("levelCount"));
			uint num5 = UintConverter.Convert(modelObjectData.GetValue("baseArrayLayer"));
			uint num6 = UintConverter.Convert(modelObjectData.GetValue("layerCount"));
			uint num7 = UintConverter.Convert(modelObjectData.GetValue("width"));
			uint num8 = UintConverter.Convert(modelObjectData.GetValue("height"));
			uint num9 = UintConverter.Convert(modelObjectData.GetValue("depth"));
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = string.Format("Properties for ImageView: {0}", resourceID);
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<ulong>("ImageID", typeof(ulong), num, text, "ImageID", true);
			list.Add(propertyDescriptor);
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkImageViewType>("ViewType", typeof(VkImageViewType), vkImageViewType, text, "ViewType", true);
			list.Add(propertyDescriptor2);
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<VkFormats>("VkFormats", typeof(VkFormats), vkFormats, text, "VkFormats", true);
			list.Add(propertyDescriptor3);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			string text2 = string.Format("Components", Array.Empty<object>());
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<VkComponentSwizzle>("r", typeof(VkComponentSwizzle), vkComponentSwizzle, text2, "r", true);
			list.Add(propertyDescriptor4);
			PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<VkComponentSwizzle>("g", typeof(VkComponentSwizzle), vkComponentSwizzle, text2, "g", true);
			list.Add(propertyDescriptor5);
			PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<VkComponentSwizzle>("b", typeof(VkComponentSwizzle), vkComponentSwizzle, text2, "b", true);
			list.Add(propertyDescriptor6);
			PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<VkComponentSwizzle>("a", typeof(VkComponentSwizzle), vkComponentSwizzle, text2, "a", true);
			list.Add(propertyDescriptor7);
			string text3 = string.Format("SubResourceRange", Array.Empty<object>());
			string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkImageAspectFlagBits>(num2);
			PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<string>("aspectMask", typeof(string), flagsEnumStr, text3, "aspectMask", true);
			list.Add(propertyDescriptor8);
			PropertyDescriptor propertyDescriptor9 = new SdpPropertyDescriptor<uint>("baseMipLevel", typeof(uint), num3, text3, "baseMipLevel", true);
			list.Add(propertyDescriptor9);
			PropertyDescriptor propertyDescriptor10 = new SdpPropertyDescriptor<uint>("levelCount", typeof(uint), num4, text3, "levelCount", true);
			list.Add(propertyDescriptor10);
			PropertyDescriptor propertyDescriptor11 = new SdpPropertyDescriptor<uint>("baseArrayLayer", typeof(uint), num5, text3, "baseArrayLayer", true);
			list.Add(propertyDescriptor11);
			PropertyDescriptor propertyDescriptor12 = new SdpPropertyDescriptor<uint>("layerCount", typeof(uint), num6, text3, "layerCount", true);
			list.Add(propertyDescriptor12);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "ImageView";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer(captureId, num);
			if (byteBuffer == null)
			{
				return;
			}
			IntPtr data2 = byteBuffer.BDP.data;
			if (byteBuffer.BDP.size > 0U)
			{
				byte[] array = new byte[byteBuffer.BDP.size];
				Marshal.Copy(byteBuffer.BDP.data, array, 0, (int)byteBuffer.BDP.size);
				Thread thread = new Thread(new ThreadStart(new ResourcesViewMgr.DisplayImages
				{
					Data = array,
					Format = vkFormats,
					Width = num7,
					Height = num8,
					LayerCount = num6,
					LevelCount = num4,
					Depth = num9
				}.DisplayThread));
				thread.Start();
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		private void UpdateTextureInfo(long resourceId, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotTextures");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				resourceId.ToString()
			});
			if (data.Count != 1)
			{
				return;
			}
			ModelObjectData modelObjectData = data[0];
			ulong num = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
			uint num2 = UintConverter.Convert(modelObjectData.GetValue("layerCount"));
			uint num3 = UintConverter.Convert(modelObjectData.GetValue("levelCount"));
			uint num4 = UintConverter.Convert(modelObjectData.GetValue("sampleCount"));
			uint num5 = UintConverter.Convert(modelObjectData.GetValue("width"));
			uint num6 = UintConverter.Convert(modelObjectData.GetValue("height"));
			uint num7 = UintConverter.Convert(modelObjectData.GetValue("depth"));
			string value = modelObjectData.GetValue("format");
			if (this.m_formatSelection != "All Formats" && this.m_formatSelection != VkHelper.GetTextureFormatString(value))
			{
				return;
			}
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = string.Format("Properties for {0}", resourceId);
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("Format", typeof(string), VkHelper.GetTextureFormatString(value), text, "Format", true);
			list.Add(propertyDescriptor);
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<uint>("Layer Count", typeof(uint), num3, text, "Layer Count", true);
			list.Add(propertyDescriptor2);
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<uint>("Mip Levels", typeof(uint), num3, text, "Mip Levels", true);
			list.Add(propertyDescriptor3);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "Image";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer(captureId, num);
			if (byteBuffer == null)
			{
				return;
			}
			IntPtr data2 = byteBuffer.BDP.data;
			if (byteBuffer.BDP.size > 0U)
			{
				byte[] array = new byte[byteBuffer.BDP.size];
				Marshal.Copy(byteBuffer.BDP.data, array, 0, (int)byteBuffer.BDP.size);
				Thread thread = new Thread(new ThreadStart(new ResourcesViewMgr.DisplayImages
				{
					Data = array,
					Format = (VkFormats)UintConverter.Convert(value),
					Width = num5,
					Height = num6,
					LayerCount = num2,
					LevelCount = num3,
					Depth = num7
				}.DisplayThread));
				thread.Start();
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000EAC8 File Offset: 0x0000CCC8
		private void DisplayDescriptorSets(long resourceHash, int captureId)
		{
			uint num = (uint)(resourceHash & (long)((ulong)(-1)));
			uint num2 = (uint)((ulong)(resourceHash & -4294967296L) >> 32);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSets");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				num.ToString()
			});
			ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSetBindings");
			if (data.Count != 1)
			{
				return;
			}
			ModelObjectData modelObjectData = data[0];
			uint num3 = UintConverter.Convert(modelObjectData.GetValue("descriptorPool"));
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = string.Format("Properties for Descriptor Set {0}", num);
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<uint>("Descriptor Pool", typeof(uint), num3, text, "Descriptor Pool", true);
			list.Add(propertyDescriptor);
			ModelObject modelObject3 = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSetLayoutLinks");
			ModelObjectDataList data2 = modelObject3.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"descriptorSetID",
				num.ToString()
			});
			uint num4 = 0U;
			foreach (ModelObjectData modelObjectData2 in data2)
			{
				ulong num5 = Uint64Converter.Convert(modelObjectData2.GetValue("descriptorSetLayoutID"));
				string text2 = "DescriptorSetLayout[" + num4.ToString() + "]";
				PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<ulong>(text2, typeof(ulong), num5, text, text2, true);
				num4 += 1U;
				list.Add(propertyDescriptor2);
			}
			string text3 = "Bindings";
			ModelObjectDataList data3 = modelObject2.GetData(new StringList
			{
				"captureID",
				this.m_currentCaptureId.ToString(),
				"descriptorSetID",
				num.ToString()
			});
			DescSetBindings descSetBindings = new DescSetBindings((ulong)num);
			foreach (ModelObjectData modelObjectData3 in data3)
			{
				uint num6 = UintConverter.Convert(modelObjectData3.GetValue("apiID"));
				if ((num2 != 4294967295U || num6 == 4294967295U) && (num2 == 4294967295U || num6 == 4294967295U || num6 <= num2))
				{
					uint num7 = UintConverter.Convert(modelObjectData3.GetValue("slotNum"));
					ulong num8 = Uint64Converter.Convert(modelObjectData3.GetValue("samplerID"));
					ulong num9 = Uint64Converter.Convert(modelObjectData3.GetValue("imageViewID"));
					uint num10 = UintConverter.Convert(modelObjectData3.GetValue("imageLayout"));
					ulong num11 = Uint64Converter.Convert(modelObjectData3.GetValue("bufferID"));
					ulong num12 = Uint64Converter.Convert(modelObjectData3.GetValue("offset"));
					ulong num13 = Uint64Converter.Convert(modelObjectData3.GetValue("range"));
					ulong num14 = Uint64Converter.Convert(modelObjectData3.GetValue("texBufferView"));
					ulong num15 = Uint64Converter.Convert(modelObjectData3.GetValue("accelerationStructID"));
					ulong num16 = Uint64Converter.Convert(modelObjectData3.GetValue("tensorID"));
					ulong num17 = Uint64Converter.Convert(modelObjectData3.GetValue("tensorViewID"));
					DescSetBindings.DescBindings descBindings;
					if (!descSetBindings.Bindings.TryGetValue((ulong)num7, out descBindings))
					{
						descBindings = new DescSetBindings.DescBindings(num8, num9, num10, num11, num12, num13, num14, num15, num16, num17);
						descSetBindings.Bindings[(ulong)num7] = descBindings;
					}
				}
			}
			foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair in descSetBindings.Bindings)
			{
				ulong key = keyValuePair.Key;
				ulong bufferID = keyValuePair.Value.bufferID;
				string text4 = "[" + key.ToString() + "]:BufferBinding:bufferID";
				PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<ulong>(text4, typeof(ulong), bufferID, text3, text4, true);
				list.Add(propertyDescriptor3);
				ulong offset = keyValuePair.Value.offset;
				string text5 = "[" + key.ToString() + "]:BufferBinding:offset";
				PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<ulong>(text5, typeof(ulong), offset, text3, text5, true);
				list.Add(propertyDescriptor4);
				ulong range = keyValuePair.Value.range;
				string text6 = "[" + key.ToString() + "]:BufferBinding:range";
				PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<ulong>(text6, typeof(ulong), range, text3, text6, true);
				list.Add(propertyDescriptor5);
				ulong samplerID = keyValuePair.Value.samplerID;
				string text7 = "[" + key.ToString() + "]:ImageBinding:SamplerID";
				PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<ulong>(text7, typeof(ulong), samplerID, text3, text7, true);
				list.Add(propertyDescriptor6);
				ulong imageViewID = keyValuePair.Value.imageViewID;
				string text8 = "[" + key.ToString() + "]:ImageBinding:ImageViewID";
				PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<ulong>(text8, typeof(ulong), imageViewID, text3, text8, true);
				list.Add(propertyDescriptor7);
				uint imageLayout = keyValuePair.Value.imageLayout;
				string text9 = "[" + key.ToString() + "]:ImageBinding:imageLayout";
				PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<uint>(text9, typeof(uint), imageLayout, text3, text9, true);
				list.Add(propertyDescriptor8);
				ulong texBufferview = keyValuePair.Value.texBufferview;
				string text10 = "[" + key.ToString() + "]:TexelBinding:texelBufferView";
				PropertyDescriptor propertyDescriptor9 = new SdpPropertyDescriptor<ulong>(text10, typeof(ulong), texBufferview, text3, text10, true);
				list.Add(propertyDescriptor9);
				ulong accelStructID = keyValuePair.Value.accelStructID;
				string text11 = "[" + key.ToString() + "]:accelerationStructID";
				PropertyDescriptor propertyDescriptor10 = new SdpPropertyDescriptor<ulong>(text11, typeof(ulong), accelStructID, text3, text11, true);
				list.Add(propertyDescriptor10);
				ulong tensorID = keyValuePair.Value.tensorID;
				string text12 = "[" + key.ToString() + "]:tensorID";
				PropertyDescriptor propertyDescriptor11 = new SdpPropertyDescriptor<ulong>(text12, typeof(ulong), tensorID, text3, text12, true);
				list.Add(propertyDescriptor11);
				ulong tensorViewID = keyValuePair.Value.tensorViewID;
				string text13 = "[" + key.ToString() + "]:tensorViewID";
				PropertyDescriptor propertyDescriptor12 = new SdpPropertyDescriptor<ulong>(text13, typeof(ulong), tensorViewID, text3, text13, true);
				list.Add(propertyDescriptor12);
			}
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "DescriptorSet";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			if (num2 != 4294967295U)
			{
				DataExplorerViewSelectRowEventArgs dataExplorerViewSelectRowEventArgs = new DataExplorerViewSelectRowEventArgs();
				dataExplorerViewSelectRowEventArgs.CaptureID = this.m_currentCaptureId;
				dataExplorerViewSelectRowEventArgs.SourceID = 353;
				dataExplorerViewSelectRowEventArgs.RowElement = num2;
				dataExplorerViewSelectRowEventArgs.SearchColumn = 0;
				SdpApp.EventsManager.Raise<DataExplorerViewSelectRowEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectRow, this, dataExplorerViewSelectRowEventArgs);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000F264 File Offset: 0x0000D464
		private void DisplayAccelerationStructureInfo(ModelObjectData asInfo, ulong resourceID, int captureId)
		{
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = "Acceleration Structure";
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<ulong>("Resource ID", typeof(ulong), resourceID, text, "Resource ID", true);
			list.Add(propertyDescriptor);
			VkAccelerationStructureType vkAccelerationStructureType = (VkAccelerationStructureType)UintConverter.Convert(asInfo.GetValue("type"));
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkAccelerationStructureType>("AS Type", typeof(VkAccelerationStructureType), vkAccelerationStructureType, text, "Acceleration Structure Type", true);
			list.Add(propertyDescriptor2);
			VkAccelerationStructureBuildType vkAccelerationStructureBuildType = (VkAccelerationStructureBuildType)UintConverter.Convert(asInfo.GetValue("buildType"));
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<VkAccelerationStructureBuildType>("Build Type", typeof(VkAccelerationStructureBuildType), vkAccelerationStructureBuildType, text, "Build Type", true);
			list.Add(propertyDescriptor3);
			uint num = UintConverter.Convert(asInfo.GetValue("childrenCount"));
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<uint>("BLAS Child Count", typeof(uint), num, text, "Number of BLAS Children nodes", true);
			list.Add(propertyDescriptor4);
			uint num2 = UintConverter.Convert(asInfo.GetValue("wideNodesCount"));
			PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<uint>("Nodes Count", typeof(uint), num2, text, "Number of nodes", true);
			list.Add(propertyDescriptor5);
			uint num3 = UintConverter.Convert(asInfo.GetValue("internalWideNodesCount"));
			PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<uint>("Internal Nodes Count", typeof(uint), num3, text, "Number of internal nodes", true);
			list.Add(propertyDescriptor6);
			uint num4 = UintConverter.Convert(asInfo.GetValue("primitivesCount"));
			PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<uint>("Primitives Count", typeof(uint), num4, text, "Number of basic geometry elements part of the node", true);
			list.Add(propertyDescriptor7);
			uint num5 = UintConverter.Convert(asInfo.GetValue("buildFlags"));
			string flagsEnumStr = VkHelper.GetFlagsEnumStr<BvhBuildFlags>(num5);
			PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<string>("Build Flags", typeof(string), flagsEnumStr, text, "Build Flags", true);
			list.Add(propertyDescriptor8);
			uint num6 = UintConverter.Convert(asInfo.GetValue("geometriesCount"));
			PropertyDescriptor propertyDescriptor9 = new SdpPropertyDescriptor<uint>("Geometries Count", typeof(uint), num6, text, "Number of individual Geometric objects or meshes included", true);
			list.Add(propertyDescriptor9);
			uint num7 = UintConverter.Convert(asInfo.GetValue("instancesCount"));
			PropertyDescriptor propertyDescriptor10 = new SdpPropertyDescriptor<uint>("Instances Count", typeof(uint), num7, text, "Number of Instances", true);
			list.Add(propertyDescriptor10);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "RayTracing Acceleration Structure";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			inspectorViewDisplayEventArgs.ShowASButton = true;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		private void DisplayTensors(long resourceID, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotTensors");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				resourceID.ToString()
			});
			ModelObjectData modelObjectData = data[0];
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = "Tensors";
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<ulong>("Resource ID", typeof(ulong), (ulong)resourceID, text, "Resource ID", true);
			list.Add(propertyDescriptor);
			VkTensorStructureType vkTensorStructureType = (VkTensorStructureType)UintConverter.Convert(modelObjectData.GetValue("type"));
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkTensorStructureType>("Structure Type", typeof(VkTensorStructureType), vkTensorStructureType, text, "VkStructureType value identifying this structure", true);
			list.Add(propertyDescriptor2);
			VkTensorTilingARM vkTensorTilingARM = (VkTensorTilingARM)UintConverter.Convert(modelObjectData.GetValue("tiling"));
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<VkTensorTilingARM>("Tiling", typeof(VkTensorTilingARM), vkTensorTilingARM, text, "VkTensorTilingARM value specifying the tiling of the tensor", true);
			list.Add(propertyDescriptor3);
			VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData.GetValue("format"));
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<VkFormats>("Format", typeof(VkFormats), vkFormats, text, "One component VkFormat describing the format and type of the data elements that will be contained in the tensor", true);
			list.Add(propertyDescriptor4);
			string value = modelObjectData.GetValue("pDimensions");
			PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<string>("Dimensions", typeof(string), value, text, "Array of integers providing the number of data elements in each dimension", true);
			list.Add(propertyDescriptor5);
			string value2 = modelObjectData.GetValue("pStrides");
			PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<string>("Strides", typeof(string), value2, text, "Array providing the strides in bytes for the tensor in each dimension", true);
			list.Add(propertyDescriptor6);
			uint num = UintConverter.Convert(modelObjectData.GetValue("usage"));
			string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkTensorUsageFlagBitsARM>(num);
			PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<string>("Usage", typeof(string), flagsEnumStr, text, "Bitmask of VkTensorUsageFlagBitsARM specifying the usage of the tensor", true);
			list.Add(propertyDescriptor7);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "Tensors";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			inspectorViewDisplayEventArgs.ShowTensorButton = true;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000F768 File Offset: 0x0000D968
		private void DisplayTensorViews(long resourceID, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotTensorsView");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				resourceID.ToString()
			});
			ModelObjectData modelObjectData = data[0];
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = "Tensor Views";
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<ulong>("Resource ID", typeof(ulong), (ulong)resourceID, text, "Tensor View Resource ID", true);
			list.Add(propertyDescriptor);
			VkTensorStructureType vkTensorStructureType = (VkTensorStructureType)UintConverter.Convert(modelObjectData.GetValue("type"));
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkTensorStructureType>("Structure Type", typeof(VkTensorStructureType), vkTensorStructureType, text, "VkStructureType value identifying this structure", true);
			list.Add(propertyDescriptor2);
			uint num = UintConverter.Convert(modelObjectData.GetValue("format"));
			string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkFormats>(num);
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<string>("Format", typeof(string), flagsEnumStr, text, "VkFormat describing the format and type used to interpret elements in the tensor", true);
			list.Add(propertyDescriptor3);
			uint num2 = UintConverter.Convert(modelObjectData.GetValue("tensorID"));
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<ulong>("Tensor ID", typeof(ulong), (ulong)num2, text, "VkTensorARM on which the view will be created.", true);
			list.Add(propertyDescriptor4);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "Tensor Views";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000F924 File Offset: 0x0000DB24
		private float[] GetASTransfromMatrix(ModelObjectData data)
		{
			BinaryDataPair valuePtrBinaryDataPair = data.GetValuePtrBinaryDataPair("asInstanceTransformMatrix");
			if (valuePtrBinaryDataPair != null)
			{
				IntPtr data2 = valuePtrBinaryDataPair.data;
				if (valuePtrBinaryDataPair.size > 0U)
				{
					int num = (int)(valuePtrBinaryDataPair.size / 4U);
					float[] array = new float[num];
					Marshal.Copy(valuePtrBinaryDataPair.data, array, 0, num);
					return array;
				}
			}
			return null;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000F974 File Offset: 0x0000DB74
		private void DisplayAccelerationStructureInstanceDescriptor(ModelObjectData instanceDescriptor, int captureId)
		{
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = "Top-Level AS Instance Descriptor";
			uint num = UintConverter.Convert(instanceDescriptor.GetValue("asInstanceCustomIndex"));
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<uint>("CustomIndex", typeof(uint), num, text, "SPIR-V InstanceCustomIndexKHR built-in", true);
			list.Add(propertyDescriptor);
			uint num2 = UintConverter.Convert(instanceDescriptor.GetValue("asInstanceShaderBindingOffset"));
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<uint>("Offset", typeof(uint), num2, text, "Shader Binding Table Record Offset", true);
			list.Add(propertyDescriptor2);
			uint num3 = UintConverter.Convert(instanceDescriptor.GetValue("asInstanceFlags"));
			string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkGeometryInstanceFlagBits>(num3);
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<string>("Geometry Instance Flags", typeof(string), flagsEnumStr, text, "Geometry Instance Flags", true);
			list.Add(propertyDescriptor3);
			string text2 = "0x" + UintConverter.Convert(instanceDescriptor.GetValue("asInstanceMask")).ToString("X");
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<string>("Mask", typeof(string), text2, text, "Visibility Mask", true);
			list.Add(propertyDescriptor4);
			string text3 = "0x" + Uint64Converter.Convert(instanceDescriptor.GetValue("blasDeviceAddress")).ToString("X");
			PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<string>("Device Address", typeof(string), text3, text, "Referenced Acceleration Structure Device Address", true);
			list.Add(propertyDescriptor5);
			float[] astransfromMatrix = this.GetASTransfromMatrix(instanceDescriptor);
			string text4 = "Transform Matrix";
			for (int i = 0; i < 3; i++)
			{
				string text5 = string.Format("{0:F4}, {1:F4}, {2:F4}, {3:F4}", new object[]
				{
					astransfromMatrix[i * 4],
					astransfromMatrix[i * 4 + 1],
					astransfromMatrix[i * 4 + 2],
					astransfromMatrix[i * 4 + 3]
				});
				PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<string>("Row " + i.ToString(), typeof(string), text5, text4, "Row " + i.ToString(), true);
				list.Add(propertyDescriptor6);
			}
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "RayTracing Acceleration Structure Instance Descriptor";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			inspectorViewDisplayEventArgs.ShowASButton = false;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		private void DisplayAccelerationStructureInfo(long resourceHash, int captureId)
		{
			ulong num = (ulong)(resourceHash & (long)((ulong)(-1)));
			ulong num2 = (ulong)(resourceHash & -4294967296L) >> 32;
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotASInfo");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"resourceID",
				num.ToString()
			});
			if (data.Count == 0)
			{
				return;
			}
			ModelObjectData modelObjectData = data[0];
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			VkAccelerationStructureType vkAccelerationStructureType = (VkAccelerationStructureType)UintConverter.Convert(modelObjectData.GetValue("type"));
			if (num2 != (ulong)(-1))
			{
				ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotASInstanceDescriptor");
				ModelObjectDataList data2 = modelObject2.GetData(new StringList
				{
					"captureID",
					captureId.ToString(),
					"tlasID",
					num.ToString(),
					"asInstanceDescriptorIndex",
					num2.ToString()
				});
				ModelObjectData modelObjectData2 = data2[0];
				this.DisplayAccelerationStructureInstanceDescriptor(modelObjectData2, captureId);
				return;
			}
			this.DisplayAccelerationStructureInfo(modelObjectData, num, captureId);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000FD34 File Offset: 0x0000DF34
		private void DisplayDescriptorSetLayouts(long resourceId, int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSetLayoutBindings");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				captureId.ToString(),
				"descriptorSetLayoutID",
				resourceId.ToString()
			});
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			uint num = 0U;
			foreach (ModelObjectData modelObjectData in data)
			{
				string text = string.Format("DescriptorSetLayout {0}, Binding[{1}]", resourceId, num++);
				uint num2 = UintConverter.Convert(modelObjectData.GetValue("binding"));
				PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<uint>("Binding", typeof(uint), num2, text, "Binding", true);
				list.Add(propertyDescriptor);
				VkDescriptorType vkDescriptorType = (VkDescriptorType)UintConverter.Convert(modelObjectData.GetValue("descriptorType"));
				PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<VkDescriptorType>("DescriptorType", typeof(VkDescriptorType), vkDescriptorType, text, "DescriptorType", true);
				list.Add(propertyDescriptor2);
				uint num3 = UintConverter.Convert(modelObjectData.GetValue("descriptorCount"));
				PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<uint>("Descriptor Count", typeof(uint), num3, text, "Descriptor Count", true);
				list.Add(propertyDescriptor3);
				uint num4 = UintConverter.Convert(modelObjectData.GetValue("stageFlags"));
				string flagsEnumStr = VkHelper.GetFlagsEnumStr<VkShaderStageFlagBits>(num4);
				PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<string>("StageFlags", typeof(string), flagsEnumStr, text, "StageFlags", true);
				list.Add(propertyDescriptor4);
			}
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "DescriptorSetLayouts";
			inspectorViewDisplayEventArgs.CaptureID = (uint)captureId;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000FF60 File Offset: 0x0000E160
		public void UpdateShaderModules(int captureId)
		{
			IEnumerable<IShaderStage> shaderModules = ShaderModuleGateway.GetShaderModules(captureId);
			if (shaderModules == null)
			{
				return;
			}
			uint num = this.m_selectedApi[(uint)this.m_currentCaptureId];
			List<uint> shaderProfileStages = ShaderProfileGateway.GetShaderProfileStages((uint)captureId, num);
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotObjectLabels");
			ModelObjectDataList data = modelObject.GetData("captureID", captureId.ToString());
			Dictionary<ulong, string> dictionary = ResourcesViewMgr.ConvertToDictionaryWithDuplicates(data);
			UpdateResourceNameArgs updateResourceNameArgs = new UpdateResourceNameArgs();
			foreach (IShaderStage shaderStage in shaderModules)
			{
				ulong shaderModuleID = shaderStage.ShaderModuleID;
				uint num2 = UintConverter.Convert(shaderStage.StageType);
				string shaderStageText = VkHelper.GetShaderStageText(num2, true);
				string text;
				dictionary.TryGetValue(shaderModuleID, out text);
				text = VkDebugMarkers.ColorMarkerTextRV(text);
				string text2 = string.Concat(new string[]
				{
					"ID [",
					shaderModuleID.ToString(),
					"], StageType: ",
					shaderStageText,
					" ",
					text
				});
				if (shaderProfileStages.Contains(num2))
				{
					text2 += "(Profile Available)";
				}
				updateResourceNameArgs.CategoryID = 2;
				updateResourceNameArgs.Items[(long)shaderModuleID] = text2;
			}
			SdpApp.EventsManager.Raise<UpdateResourceNameArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateResourceName, this, updateResourceNameArgs);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000100E0 File Offset: 0x0000E2E0
		private void DisplayAccelerationStructure(long resourceHash, int captureId)
		{
			ulong num = (ulong)(resourceHash & (long)((ulong)(-1)));
			ulong num2 = (ulong)(resourceHash & -4294967296L) >> 32;
			if (num2 == (ulong)(-1))
			{
				Viewer3DLoadASArgs viewer3DLoadASArgs = new Viewer3DLoadASArgs();
				viewer3DLoadASArgs.AccelerationStructureID = num;
				SdpApp.EventsManager.Raise<Viewer3DLoadASArgs>(SdpApp.EventsManager.Viewer3DEvents.RequestLoadAccelerationStructure, this, viewer3DLoadASArgs);
			}
		}

		// Token: 0x040003AA RID: 938
		private int m_currentCaptureId = -1;

		// Token: 0x040003AB RID: 939
		private int m_currentSourceId = -1;

		// Token: 0x040003AC RID: 940
		private static List<int> m_captureIDList = new List<int>();

		// Token: 0x040003AD RID: 941
		private static Dictionary<int, int> m_costMetricCount = new Dictionary<int, int>();

		// Token: 0x040003AE RID: 942
		private const string ALL_BOUND_USED_RADIO_BUTTON_FILTER = "All/Bound/Used";

		// Token: 0x040003AF RID: 943
		private const string SHOW_ALL = "All Resources";

		// Token: 0x040003B0 RID: 944
		private const string SHOW_BOUND = "Bound Only";

		// Token: 0x040003B1 RID: 945
		private const string SHOW_USED = "Used Only";

		// Token: 0x040003B2 RID: 946
		public const uint UNSELECTED = 4294967295U;

		// Token: 0x040003B3 RID: 947
		private ResourcesViewMgr.UIRequestHandler m_uiRequestHandler = new ResourcesViewMgr.UIRequestHandler();

		// Token: 0x040003B4 RID: 948
		private ResourcesViewMgr.AllBoundUsed m_allBoundUsedSelection = ResourcesViewMgr.AllBoundUsed.Used;

		// Token: 0x040003B5 RID: 949
		private const string FORMAT_COMBO_FILTER = "All Formats";

		// Token: 0x040003B6 RID: 950
		private string FORMAT_COMBO_FILTER_NAME = "Image Formats";

		// Token: 0x040003B7 RID: 951
		private string m_formatSelection = "All Formats";

		// Token: 0x040003B8 RID: 952
		internal static int FORMAT_FILTER_OFFSET = 4;

		// Token: 0x040003B9 RID: 953
		private const string MARKER_COMBO_FILTER = "All Objects";

		// Token: 0x040003BA RID: 954
		private string MARKER_COMBO_FILTER_NAME = "Debug Markers";

		// Token: 0x040003BB RID: 955
		private string m_markerSelection = "All Objects";

		// Token: 0x040003BC RID: 956
		internal static int MARKER_FILTER_OFFSET = 2;

		// Token: 0x040003BD RID: 957
		internal static string TILE_MEMORY = "pGMEM";

		// Token: 0x040003BE RID: 958
		private bool m_tileMemoryOnly;

		// Token: 0x040003BF RID: 959
		internal static int TILE_FILTER_OFFSET = 3;

		// Token: 0x040003C0 RID: 960
		private Dictionary<uint, uint> m_selectedApi = new Dictionary<uint, uint>();

		// Token: 0x040003C1 RID: 961
		private Dictionary<uint, string> m_selectedDrawCallID = new Dictionary<uint, string>();

		// Token: 0x040003C2 RID: 962
		private List<SetStatusEventArgs> m_statusQueue = new List<SetStatusEventArgs>();

		// Token: 0x040003C3 RID: 963
		private ParentBoundInfo m_apiRange;

		// Token: 0x040003C4 RID: 964
		private ByteBufferGateway m_byteBufferGateway;

		// Token: 0x040003C5 RID: 965
		private static ILogger Logger = new global::Sdp.Logging.Logger("QGL Client Plugin");

		// Token: 0x0200006D RID: 109
		private enum AllBoundUsed
		{
			// Token: 0x040004C8 RID: 1224
			All,
			// Token: 0x040004C9 RID: 1225
			Bound,
			// Token: 0x040004CA RID: 1226
			Used
		}

		// Token: 0x0200006E RID: 110
		private class StagePipelineIDTuple
		{
			// Token: 0x060001E8 RID: 488 RVA: 0x00015F6B File Offset: 0x0001416B
			public StagePipelineIDTuple(uint stageType, ulong pipelineID)
			{
				this.StageType = stageType;
				this.PipelineID = pipelineID;
			}

			// Token: 0x040004CB RID: 1227
			public uint StageType;

			// Token: 0x040004CC RID: 1228
			public ulong PipelineID;
		}

		// Token: 0x0200006F RID: 111
		internal enum ResourceCategory
		{
			// Token: 0x040004CE RID: 1230
			Images,
			// Token: 0x040004CF RID: 1231
			MemoryBuffers,
			// Token: 0x040004D0 RID: 1232
			ShaderModules,
			// Token: 0x040004D1 RID: 1233
			GraphicsPipelines,
			// Token: 0x040004D2 RID: 1234
			ComputePipelines,
			// Token: 0x040004D3 RID: 1235
			RayTracingPipelines,
			// Token: 0x040004D4 RID: 1236
			ImageViews,
			// Token: 0x040004D5 RID: 1237
			DescriptorSets,
			// Token: 0x040004D6 RID: 1238
			DescriptorSetLayouts,
			// Token: 0x040004D7 RID: 1239
			AccelerationStructureInfo,
			// Token: 0x040004D8 RID: 1240
			TraversalShaders,
			// Token: 0x040004D9 RID: 1241
			Tensors,
			// Token: 0x040004DA RID: 1242
			TensorViews
		}

		// Token: 0x02000070 RID: 112
		private class UniformBuffer
		{
			// Token: 0x040004DB RID: 1243
			public ulong memoryID;

			// Token: 0x040004DC RID: 1244
			public ulong bufferID;

			// Token: 0x040004DD RID: 1245
			public ulong offset;

			// Token: 0x040004DE RID: 1246
			public ulong range;

			// Token: 0x040004DF RID: 1247
			public uint stageFlags;

			// Token: 0x040004E0 RID: 1248
			public uint descriptorSet;

			// Token: 0x040004E1 RID: 1249
			public uint binding;
		}

		// Token: 0x02000071 RID: 113
		private class DataRange
		{
			// Token: 0x040004E2 RID: 1250
			public ulong layoutID;

			// Token: 0x040004E3 RID: 1251
			public List<ShaderStage> shaderTypes;

			// Token: 0x040004E4 RID: 1252
			public uint dataOffset;

			// Token: 0x040004E5 RID: 1253
			public uint displayOffset;

			// Token: 0x040004E6 RID: 1254
			public uint size;

			// Token: 0x040004E7 RID: 1255
			public byte[] data;

			// Token: 0x040004E8 RID: 1256
			public uint descriptorSet;

			// Token: 0x040004E9 RID: 1257
			public uint binding;
		}

		// Token: 0x02000072 RID: 114
		public struct ReflectionVariable
		{
			// Token: 0x040004EA RID: 1258
			public string name;

			// Token: 0x040004EB RID: 1259
			public string type;

			// Token: 0x040004EC RID: 1260
			public uint offset;

			// Token: 0x040004ED RID: 1261
			public uint size;

			// Token: 0x040004EE RID: 1262
			public uint descriptorSet;

			// Token: 0x040004EF RID: 1263
			public uint binding;
		}

		// Token: 0x02000073 RID: 115
		private class ProgramInspectorPopulator
		{
			// Token: 0x060001EB RID: 491 RVA: 0x00015F81 File Offset: 0x00014181
			public void SetCaptureID(int captureId)
			{
				if (captureId != this.m_captureId)
				{
					this.m_captureId = captureId;
					this.m_uniformBuffers.Clear();
					this.m_uniformRanges.Clear();
					this.m_pushConstantRanges.Clear();
				}
			}

			// Token: 0x060001EC RID: 492 RVA: 0x00015FB4 File Offset: 0x000141B4
			public void Update(long pipelineId, ByteBufferGateway byteBufferGateway, string selectedDrawCallID, uint selectedDrawAPI)
			{
				ProgramViewInvalidateEventArgs programViewInvalidateEventArgs = new ProgramViewInvalidateEventArgs();
				SdpApp.EventsManager.Raise<ProgramViewInvalidateEventArgs>(SdpApp.EventsManager.ProgramViewEvents.Invalidate, this, programViewInvalidateEventArgs);
				DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
				Model model = dataModel.GetModel("VulkanSnapshot");
				ModelObjectDataList vulkanSnapshotObjectLabels = QGLModel.GetVulkanSnapshotObjectLabels(this.m_captureId);
				Dictionary<ulong, string> dictionary = ResourcesViewMgr.ConvertToDictionaryWithDuplicates(vulkanSnapshotObjectLabels);
				ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotShaderStages");
				ModelObjectDataList data = modelObject.GetData(new StringList
				{
					"captureID",
					this.m_captureId.ToString(),
					"pipelineID",
					pipelineId.ToString()
				});
				foreach (ModelObjectData modelObjectData in data)
				{
					uint num = UintConverter.Convert(modelObjectData.GetValue("stageType"));
					ProgramViewShader programViewShader = new ProgramViewShader();
					programViewShader.ShaderId = UintConverter.Convert(modelObjectData.GetValue("shaderModuleID"));
					programViewShader.ShaderType = VkHelper.ConvertVkShaderEnum((VkShaderStageFlagBits)num);
					string text;
					dictionary.TryGetValue((ulong)programViewShader.ShaderId, out text);
					programViewShader.Label = string.Concat(new string[]
					{
						"ID [",
						programViewShader.ShaderId.ToString(),
						VkHelper.ConcatenateSlashAndStringIfNotEmpty(text),
						"], StageType: ",
						VkHelper.GetShaderStageText(num, true)
					});
					programViewInvalidateEventArgs.Shaders.Add(programViewShader);
					this.m_shaderStageToShaderID[programViewShader.ShaderType] = programViewShader.ShaderId;
				}
				if (this.InitializeUniformBuffers(ref dataModel, ref model, pipelineId, selectedDrawAPI))
				{
					this.m_uniformRanges.Clear();
					foreach (ResourcesViewMgr.UniformBuffer uniformBuffer in this.m_uniformBuffers)
					{
						IByteBuffer byteBuffer = byteBufferGateway.GetByteBuffer(this.m_captureId, uniformBuffer.bufferID, uint.MaxValue);
						BinaryDataPair binaryDataPair = ((byteBuffer != null) ? byteBuffer.BDP : null) ?? null;
						if (binaryDataPair != null)
						{
							IntPtr data2 = binaryDataPair.data;
							ResourcesViewMgr.DataRange dataRange = new ResourcesViewMgr.DataRange();
							dataRange.layoutID = (ulong)((long)uniformBuffer.GetHashCode());
							dataRange.shaderTypes = VkHelper.GetShaderStages(uniformBuffer.stageFlags);
							dataRange.dataOffset = 0U;
							dataRange.displayOffset = (uint)uniformBuffer.offset;
							dataRange.size = ((uniformBuffer.range == ulong.MaxValue) ? binaryDataPair.size : ((uint)uniformBuffer.range));
							dataRange.data = new byte[dataRange.size];
							dataRange.descriptorSet = uniformBuffer.descriptorSet;
							dataRange.binding = uniformBuffer.binding;
							Marshal.Copy(IntPtr.Add(binaryDataPair.data, (int)uniformBuffer.offset), dataRange.data, 0, (int)dataRange.size);
							ProgramViewDataRange programViewDataRange = new ProgramViewDataRange();
							programViewDataRange.LayoutID = dataRange.layoutID;
							programViewDataRange.ShaderTypes = dataRange.shaderTypes;
							programViewDataRange.Label = string.Concat(new string[]
							{
								"ID [",
								uniformBuffer.bufferID.ToString(),
								"], Offset: ",
								uniformBuffer.offset.ToString(),
								", Size: ",
								dataRange.size.ToString(),
								" bytes"
							});
							string dataType = this.m_dataType;
							this.m_dataType = "Auto";
							List<ShaderStage> list2;
							List<ProgramViewVariable> list = this.GetProgramViewVariablesFromRange(DataRangeType.Uniform, dataRange, out list2);
							if (list.Count != 0)
							{
								dataRange.shaderTypes = list2;
								programViewDataRange.ShaderTypes = list2;
								this.m_uniformRanges.Add(dataRange);
								programViewInvalidateEventArgs.UniformRanges.Add(programViewDataRange);
								this.m_dataType = dataType;
								if (!DataTypes.ShouldUseReflection(this.m_dataType))
								{
									List<ShaderStage> list3;
									list = this.GetProgramViewVariablesFromRange(DataRangeType.Uniform, dataRange, out list3);
								}
								foreach (ProgramViewVariable programViewVariable in list)
								{
									programViewInvalidateEventArgs.VkUniforms.Add(programViewVariable);
								}
							}
						}
					}
				}
				ulong layoutIDFromPipelineID = this.GetLayoutIDFromPipelineID(ref dataModel, ref model, pipelineId);
				this.m_pushConstantRanges.Clear();
				ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotPushConstantRanges");
				ModelObjectDataList data3 = modelObject2.GetData(new StringList
				{
					"captureID",
					this.m_captureId.ToString(),
					"layoutID",
					layoutIDFromPipelineID.ToString()
				});
				foreach (ModelObjectData modelObjectData2 in data3)
				{
					uint num2 = UintConverter.Convert(modelObjectData2.GetValue("stageType"));
					uint num3 = UintConverter.Convert(modelObjectData2.GetValue("offset"));
					uint num4 = UintConverter.Convert(modelObjectData2.GetValue("size"));
					ProgramViewDataRange programViewDataRange2 = new ProgramViewDataRange();
					programViewDataRange2.LayoutID = layoutIDFromPipelineID;
					programViewDataRange2.ShaderTypes = new List<ShaderStage> { VkHelper.ConvertVkShaderEnum((VkShaderStageFlagBits)num2) };
					programViewDataRange2.Label = string.Concat(new string[]
					{
						"ID [",
						layoutIDFromPipelineID.ToString(),
						"], Offset: ",
						num3.ToString(),
						", Size: ",
						num4.ToString(),
						" bytes"
					});
					programViewInvalidateEventArgs.PushConstantRanges.Add(programViewDataRange2);
					if (this.AddPushConstantRangeData(ref dataModel, ref model, num3, num4, layoutIDFromPipelineID, num2, selectedDrawCallID))
					{
						List<ShaderStage> list3;
						List<ProgramViewVariable> programViewVariablesFromRange = this.GetProgramViewVariablesFromRange(DataRangeType.PushConstant, Enumerable.Last<ResourcesViewMgr.DataRange>(this.m_pushConstantRanges), out list3);
						foreach (ProgramViewVariable programViewVariable2 in programViewVariablesFromRange)
						{
							programViewInvalidateEventArgs.PushConstants.Add(programViewVariable2);
						}
					}
				}
				programViewInvalidateEventArgs.ToolbarVisible = this.m_pushConstantRanges.Count > 0 || this.m_uniformBuffers.Count > 0;
				SdpApp.EventsManager.Raise<ProgramViewInvalidateEventArgs>(SdpApp.EventsManager.ProgramViewEvents.Invalidate, this, programViewInvalidateEventArgs);
				SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ProgramViewEvents.HideStatus, null, EventArgs.Empty);
			}

			// Token: 0x060001ED RID: 493 RVA: 0x00016664 File Offset: 0x00014864
			public void UpdateDataFormat(string dataType)
			{
				if (this.m_dataType == dataType)
				{
					return;
				}
				this.m_dataType = dataType;
				this.UpdateDataRanges();
			}

			// Token: 0x060001EE RID: 494 RVA: 0x00016684 File Offset: 0x00014884
			private bool UpdateDataRanges()
			{
				ProgramViewInvalidateVariablesEventArgs programViewInvalidateVariablesEventArgs = new ProgramViewInvalidateVariablesEventArgs();
				foreach (ResourcesViewMgr.DataRange dataRange in this.m_pushConstantRanges)
				{
					List<ShaderStage> list;
					List<ProgramViewVariable> programViewVariablesFromRange = this.GetProgramViewVariablesFromRange(DataRangeType.PushConstant, dataRange, out list);
					foreach (ProgramViewVariable programViewVariable in programViewVariablesFromRange)
					{
						programViewInvalidateVariablesEventArgs.PushConstants.Add(programViewVariable);
					}
				}
				foreach (ResourcesViewMgr.DataRange dataRange2 in this.m_uniformRanges)
				{
					List<ShaderStage> list;
					List<ProgramViewVariable> programViewVariablesFromRange2 = this.GetProgramViewVariablesFromRange(DataRangeType.Uniform, dataRange2, out list);
					foreach (ProgramViewVariable programViewVariable2 in programViewVariablesFromRange2)
					{
						programViewInvalidateVariablesEventArgs.Uniforms.Add(programViewVariable2);
					}
				}
				SdpApp.EventsManager.Raise<ProgramViewInvalidateVariablesEventArgs>(SdpApp.EventsManager.ProgramViewEvents.InvalidateVariables, this, programViewInvalidateVariablesEventArgs);
				return true;
			}

			// Token: 0x060001EF RID: 495 RVA: 0x000167D4 File Offset: 0x000149D4
			private ulong GetLayoutIDFromPipelineID(ref DataModel dataModel, ref Model model, long pipelineId)
			{
				ulong num = 0UL;
				ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPipelineLayouts");
				ModelObjectDataList data = modelObject.GetData(new StringList
				{
					"captureID",
					this.m_captureId.ToString(),
					"pipelineID",
					pipelineId.ToString()
				});
				if (Enumerable.Count<ModelObjectData>(data) > 0)
				{
					num = Uint64Converter.Convert(data[0].GetValue("layoutID"));
				}
				return num;
			}

			// Token: 0x060001F0 RID: 496 RVA: 0x00016854 File Offset: 0x00014A54
			private bool InitializeUniformBuffers(ref DataModel dataModel, ref Model model, long pipelineID, uint selectedDrawAPI)
			{
				this.m_uniformBuffers.Clear();
				VkBoundInfo vkBoundInfo;
				QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_captureId, selectedDrawAPI, out vkBoundInfo);
				ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotMemoryBufferLinks");
				ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSetLayoutLinks");
				ModelObject modelObject3 = dataModel.GetModelObject(model, "VulkanSnapshotDescriptorSetLayoutBindings");
				foreach (KeyValuePair<ulong, DescSetBindings> keyValuePair in vkBoundInfo.BoundDescriptorSets)
				{
					foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair2 in keyValuePair.Value.Bindings)
					{
						ResourcesViewMgr.UniformBuffer uniformBuffer = new ResourcesViewMgr.UniformBuffer();
						uniformBuffer.bufferID = keyValuePair2.Value.bufferID;
						uniformBuffer.offset = keyValuePair2.Value.offset;
						uniformBuffer.range = keyValuePair2.Value.range;
						uniformBuffer.descriptorSet = (uint)keyValuePair.Key;
						uniformBuffer.binding = (uint)keyValuePair2.Key;
						ModelObjectDataList data = modelObject.GetData(new StringList
						{
							"captureID",
							this.m_captureId.ToString(),
							"bufferID",
							uniformBuffer.bufferID.ToString()
						});
						if (data.Count != 0)
						{
							uniformBuffer.memoryID = Uint64Converter.Convert(data[0].GetValue("memoryID"));
							ModelObjectDataList data2 = modelObject2.GetData(new StringList
							{
								"captureID",
								this.m_captureId.ToString(),
								"descriptorSetID",
								keyValuePair.Value.DescSetID.ToString()
							});
							if (data2.Count != 0)
							{
								long num = Int64Converter.Convert(data2[0].GetValue("descriptorSetLayoutID"));
								ModelObjectDataList data3 = modelObject3.GetData(new StringList
								{
									"captureID",
									this.m_captureId.ToString(),
									"descriptorSetLayoutID",
									num.ToString()
								});
								bool flag = false;
								uniformBuffer.stageFlags = 0U;
								foreach (ModelObjectData modelObjectData in data3)
								{
									VkDescriptorType vkDescriptorType = (VkDescriptorType)UintConverter.Convert(modelObjectData.GetValue("descriptorType"));
									if (vkDescriptorType == VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER || vkDescriptorType == VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC || vkDescriptorType == VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER)
									{
										flag = true;
										uniformBuffer.stageFlags |= UintConverter.Convert(modelObjectData.GetValue("stageFlags"));
									}
								}
								if (flag)
								{
									this.m_uniformBuffers.Add(uniformBuffer);
								}
							}
						}
					}
				}
				return true;
			}

			// Token: 0x060001F1 RID: 497 RVA: 0x00016B7C File Offset: 0x00014D7C
			private bool AddPushConstantRangeData(ref DataModel dataModel, ref Model model, uint rangeOffset, uint rangeSize, ulong layoutId, uint shaderType, string selectedDrawCallID)
			{
				ResourcesViewMgr.DataRange dataRange = new ResourcesViewMgr.DataRange();
				if (string.IsNullOrEmpty(selectedDrawCallID))
				{
					return false;
				}
				ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotPushConstantData");
				ModelObjectDataList data = modelObject.GetData(new StringList
				{
					"captureID",
					this.m_captureId.ToString(),
					"layoutID",
					layoutId.ToString(),
					"stageType",
					shaderType.ToString()
				});
				if (data.Count == 0)
				{
					return false;
				}
				int num = data.Count - 1;
				ulong num2 = VkHelper.MakeSnapshotApiCallID(selectedDrawCallID);
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_captureId);
				List<Tuple<ulong, uint>> list = ((capture != null) ? capture.DrawCallIDToPushConstantSeqID : null);
				byte[] array = new byte[rangeSize];
				bool[] array2 = new bool[rangeSize];
				foreach (Tuple<ulong, uint> tuple in Enumerable.Reverse<Tuple<ulong, uint>>(list))
				{
					if (tuple.Item1 <= num2)
					{
						uint item = tuple.Item2;
						while (num >= 0 && UintConverter.Convert(data[num].GetValue("apiID")) > item)
						{
							num--;
						}
						ModelObjectData modelObjectData = data[num];
						BinaryDataPair valuePtrBinaryDataPair = modelObjectData.GetValuePtrBinaryDataPair("data");
						uint num3 = UintConverter.Convert(modelObjectData.GetValue("offset"));
						if (valuePtrBinaryDataPair != null)
						{
							IntPtr data2 = valuePtrBinaryDataPair.data;
							if (valuePtrBinaryDataPair.size >= 0U && num3 >= rangeOffset && valuePtrBinaryDataPair.size <= rangeSize - (num3 - rangeOffset))
							{
								num3 -= rangeOffset;
								byte[] array3 = new byte[valuePtrBinaryDataPair.size];
								Marshal.Copy(valuePtrBinaryDataPair.data, array3, 0, (int)valuePtrBinaryDataPair.size);
								for (uint num4 = num3; num4 < num3 + valuePtrBinaryDataPair.size; num4 += 1U)
								{
									if (!array2[(int)num4])
									{
										array[(int)num4] = array3[(int)(num4 - num3)];
										array2[(int)num4] = true;
									}
								}
								bool flag = true;
								int num5 = 0;
								while ((long)num5 < (long)((ulong)rangeSize))
								{
									if (!array2[num5])
									{
										flag = false;
									}
									num5++;
								}
								if (flag)
								{
									break;
								}
								continue;
							}
						}
						return false;
					}
				}
				dataRange.layoutID = layoutId;
				dataRange.shaderTypes = new List<ShaderStage> { VkHelper.ConvertVkShaderEnum((VkShaderStageFlagBits)shaderType) };
				dataRange.dataOffset = rangeOffset;
				dataRange.displayOffset = 0U;
				dataRange.size = rangeSize;
				dataRange.data = array;
				this.m_pushConstantRanges.Add(dataRange);
				return true;
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x00016E0C File Offset: 0x0001500C
			private List<ProgramViewVariable> GetProgramViewVariablesFromRange(DataRangeType dataRangeType, ResourcesViewMgr.DataRange dataRange, out List<ShaderStage> usedShaderStages)
			{
				List<ProgramViewVariable> list = new List<ProgramViewVariable>();
				usedShaderStages = new List<ShaderStage>();
				if (dataRange == null)
				{
					return list;
				}
				if (DataTypes.ShouldUseReflection(this.m_dataType))
				{
					SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
					if (processorPlugin == null)
					{
						return list;
					}
					using (List<ShaderStage>.Enumerator enumerator = dataRange.shaderTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ShaderStage shaderStage = enumerator.Current;
							int count = list.Count;
							uint num;
							if (this.m_shaderStageToShaderID.TryGetValue(shaderStage, out num))
							{
								BinaryDataPair localBuffer = processorPlugin.GetLocalBuffer(VkHelper.GetBufferType(dataRangeType), num, (uint)this.m_captureId);
								if (localBuffer == null)
								{
									return list;
								}
								int num2 = 0;
								while ((long)num2 < (long)((ulong)localBuffer.size))
								{
									this.DisplayReflectionVariable(ref list, ref dataRange, shaderStage, ref localBuffer, ref num2, -1);
									num2 += Marshal.SizeOf(typeof(ResourcesViewMgr.ReflectionVariable));
								}
							}
							if (count != list.Count)
							{
								usedShaderStages.Add(shaderStage);
							}
						}
						return list;
					}
				}
				int elementSize = FormatHelper.GetElementSize(this.m_dataType);
				foreach (ShaderStage shaderStage2 in dataRange.shaderTypes)
				{
					int num3 = 0;
					while ((long)(num3 + elementSize) <= (long)((ulong)dataRange.size))
					{
						ProgramViewVariable programViewVariable = new ProgramViewVariable();
						programViewVariable.LayoutID = dataRange.layoutID;
						programViewVariable.ShaderType = shaderStage2;
						programViewVariable.Offset = (uint)(num3 + (int)dataRange.dataOffset);
						programViewVariable.Type = this.m_dataType;
						programViewVariable.Value = FormatHelper.FormatData(ref dataRange.data, num3, this.m_dataType);
						list.Add(programViewVariable);
						num3 += elementSize;
					}
				}
				return list;
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x00016FE8 File Offset: 0x000151E8
			private void DisplayReflectionVariable(ref List<ProgramViewVariable> programViewVariables, ref ResourcesViewMgr.DataRange dataRange, ShaderStage shaderType, ref BinaryDataPair reflectionBDP, ref int bdpByteIndex, int currentVariableParentDisplayID)
			{
				if ((long)bdpByteIndex >= (long)((ulong)reflectionBDP.size))
				{
					return;
				}
				ResourcesViewMgr.ReflectionVariable reflectionVariable = Marshal.PtrToStructure<ResourcesViewMgr.ReflectionVariable>(reflectionBDP.data + bdpByteIndex);
				int num = reflectionVariable.type.IndexOf('(');
				if (dataRange.descriptorSet != reflectionVariable.descriptorSet || dataRange.binding != reflectionVariable.binding)
				{
					return;
				}
				ProgramViewVariable programViewVariable = new ProgramViewVariable();
				programViewVariable.LayoutID = dataRange.layoutID;
				programViewVariable.ShaderType = shaderType;
				programViewVariable.DisplayID = programViewVariables.Count;
				programViewVariable.ParentDisplayID = currentVariableParentDisplayID;
				programViewVariable.Offset = dataRange.displayOffset + reflectionVariable.offset;
				programViewVariable.Name = reflectionVariable.name;
				programViewVariable.Type = ((num < 0) ? reflectionVariable.type : reflectionVariable.type.Substring(0, num));
				if (DataTypes.IsBasicType(reflectionVariable.type))
				{
					programViewVariable.Value = FormatHelper.FormatData(ref dataRange.data, (int)(reflectionVariable.offset - dataRange.dataOffset), reflectionVariable.type);
					programViewVariables.Add(programViewVariable);
					return;
				}
				programViewVariables.Add(programViewVariable);
				uint num2 = reflectionVariable.offset - dataRange.dataOffset + reflectionVariable.size;
				uint num3 = 0U;
				while (num3 < num2 && (long)(bdpByteIndex + 2 * Marshal.SizeOf(typeof(ResourcesViewMgr.ReflectionVariable))) <= (long)((ulong)reflectionBDP.size))
				{
					bdpByteIndex += Marshal.SizeOf(typeof(ResourcesViewMgr.ReflectionVariable));
					reflectionVariable = Marshal.PtrToStructure<ResourcesViewMgr.ReflectionVariable>(reflectionBDP.data + bdpByteIndex);
					num3 = reflectionVariable.offset - dataRange.dataOffset + reflectionVariable.size;
					this.DisplayReflectionVariable(ref programViewVariables, ref dataRange, shaderType, ref reflectionBDP, ref bdpByteIndex, programViewVariable.DisplayID);
				}
			}

			// Token: 0x040004F0 RID: 1264
			private int m_captureId = -1;

			// Token: 0x040004F1 RID: 1265
			private string m_dataType = "Auto";

			// Token: 0x040004F2 RID: 1266
			private readonly HashSet<ResourcesViewMgr.UniformBuffer> m_uniformBuffers = new HashSet<ResourcesViewMgr.UniformBuffer>();

			// Token: 0x040004F3 RID: 1267
			private readonly List<ResourcesViewMgr.DataRange> m_uniformRanges = new List<ResourcesViewMgr.DataRange>();

			// Token: 0x040004F4 RID: 1268
			private readonly List<ResourcesViewMgr.DataRange> m_pushConstantRanges = new List<ResourcesViewMgr.DataRange>();

			// Token: 0x040004F5 RID: 1269
			private readonly Dictionary<ShaderStage, uint> m_shaderStageToShaderID = new Dictionary<ShaderStage, uint>();
		}

		// Token: 0x02000074 RID: 116
		private class UIRequestHandler
		{
			// Token: 0x060001F5 RID: 501 RVA: 0x000171E8 File Offset: 0x000153E8
			public void SetProgramInspectorCaptureID(int captureId)
			{
				this.m_actionQueue.Queue(delegate
				{
					this.m_programInspectorPopulator.SetCaptureID(captureId);
				});
			}

			// Token: 0x060001F6 RID: 502 RVA: 0x00017220 File Offset: 0x00015420
			public void UpdateProgramInspector(long pipelineId, ByteBufferGateway byteBufferGateway, string selectedDrawCallID, uint selectedDrawAPI)
			{
				this.m_actionQueue.Queue(delegate
				{
					this.m_programInspectorPopulator.Update(pipelineId, byteBufferGateway, selectedDrawCallID, selectedDrawAPI);
				});
			}

			// Token: 0x060001F7 RID: 503 RVA: 0x00017270 File Offset: 0x00015470
			public void UpdateDataFormat(string dataType)
			{
				this.m_actionQueue.Queue(delegate
				{
					this.m_programInspectorPopulator.UpdateDataFormat(dataType);
				});
			}

			// Token: 0x040004F6 RID: 1270
			private IActionQueue m_actionQueue = new ActionQueue(false);

			// Token: 0x040004F7 RID: 1271
			private ResourcesViewMgr.ProgramInspectorPopulator m_programInspectorPopulator = new ResourcesViewMgr.ProgramInspectorPopulator();
		}

		// Token: 0x02000075 RID: 117
		internal class InvalidateClass
		{
			// Token: 0x060001F9 RID: 505 RVA: 0x000172C8 File Offset: 0x000154C8
			public InvalidateClass(ResourcesViewMgr viewMgr, ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType, bool newCapture, ByteBufferGateway byteBuffers)
			{
				this.m_invalidateType = invalidateType;
				this.m_viewMgr = viewMgr;
				this.m_newCapture = newCapture;
				this.m_byteBufferGateway = byteBuffers;
			}

			// Token: 0x060001FA RID: 506 RVA: 0x0001736C File Offset: 0x0001556C
			public void Invalidate()
			{
				if (this.m_viewMgr.m_currentSourceId == 353)
				{
					CancellationTokenSource token = new CancellationTokenSource();
					TaskCreationOptions taskCreationOptions = TaskCreationOptions.None;
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
					{
						taskCreationOptions = TaskCreationOptions.LongRunning;
					}
					CancellationTokenSource cancellationTokenSource;
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive && ResourcesViewMgr.InvalidateClass.m_tasks.TryGetValue(this.m_invalidateType, ref cancellationTokenSource))
					{
						cancellationTokenSource.Cancel();
					}
					ResourcesViewMgr.InvalidateClass.m_tasks[this.m_invalidateType] = token;
					Task.Factory.StartNew(delegate
					{
						try
						{
							this.m_cancelToken = token.Token;
							this.InvalidateThread();
						}
						catch (OperationCanceledException)
						{
							if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
							{
								QGLPlugin.VkSnapshotModel.DrawcallsPerResource.Remove(this.m_viewMgr.m_currentCaptureId);
							}
						}
						finally
						{
							this.m_viewMgr.RemoveStatus(this.m_statusArgs);
							CancellationTokenSource cancellationTokenSource2;
							ResourcesViewMgr.InvalidateClass.m_tasks.TryRemove(this.m_invalidateType, ref cancellationTokenSource2);
						}
					}, token.Token, taskCreationOptions, TaskScheduler.Default);
					return;
				}
				if (this.m_viewMgr.m_currentSourceId == 2401)
				{
					Thread thread = new Thread(new ThreadStart(this.InvalidateThreadTrace));
					thread.Start();
				}
			}

			// Token: 0x060001FB RID: 507 RVA: 0x0001743C File Offset: 0x0001563C
			public static void CancelAll()
			{
				foreach (KeyValuePair<ResourcesViewMgr.InvalidateClass.InvalidateType, CancellationTokenSource> keyValuePair in ResourcesViewMgr.InvalidateClass.m_tasks)
				{
					keyValuePair.Value.Cancel();
				}
				ResourcesViewMgr.InvalidateClass.m_tasks.Clear();
			}

			// Token: 0x060001FC RID: 508 RVA: 0x00017498 File Offset: 0x00015698
			public void InvalidateThreadTrace()
			{
				object obj = ResourcesViewMgr.InvalidateClass.invalidateLock;
				lock (obj)
				{
					this.InitTraceInstanceVars();
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ResourceViewEvents.Clear, this, EventArgs.Empty);
					}
					this.m_statusArgs = new SetStatusEventArgs();
					this.m_statusArgs.Duration = 0;
					this.m_statusArgs.StatusText = "Processing Resources";
					this.m_statusArgs.Status = StatusType.Warning;
					this.m_viewMgr.AddStatus(this.m_statusArgs);
					this.InvalidateTraceShaderData();
					this.m_viewMgr.RemoveStatus(this.m_statusArgs);
				}
			}

			// Token: 0x060001FD RID: 509 RVA: 0x0001755C File Offset: 0x0001575C
			public void InvalidateThread()
			{
				DateTime now = DateTime.Now;
				this.m_allBoundUsed = this.m_viewMgr.m_allBoundUsedSelection;
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateBinaryData)
				{
					if (this.UpdateBinaryData())
					{
						ResourcesViewMgr.Logger.LogInformation("ResourcesView image processing: " + (DateTime.Now - now).TotalMilliseconds.ToString() + " ms");
					}
					return;
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
				{
					if (this.Prepopulate())
					{
						ResourcesViewMgr.Logger.LogInformation("ResourcesView post processing: " + (DateTime.Now - now).TotalMilliseconds.ToString() + " ms");
					}
					return;
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateImageFormats || this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker || this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateTileMemory)
				{
					this.InvalidateForFilterUpdate();
					return;
				}
				object obj = ResourcesViewMgr.InvalidateClass.invalidateLock;
				lock (obj)
				{
					this.InitInstanceVars();
					ParentBoundInfo parentBoundInfo = null;
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
					if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive)
						{
							if (this.m_viewMgr.m_apiRange != null)
							{
								parentBoundInfo = this.m_viewMgr.m_apiRange;
								this.m_viewMgr.m_apiRange = null;
								VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
								VkHelper.FirstDrawcall(parentBoundInfo.ParentApiID, capture.Builder.AllNodes, out parentBoundInfo.DrawcallStartID);
							}
						}
					}
					else
					{
						SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ResourceViewEvents.Clear, this, EventArgs.Empty);
						this.m_formatsVisible.Clear();
						this.m_viewMgr.m_formatSelection = "All Formats";
						this.m_allBoundUsed = (this.m_viewMgr.m_allBoundUsedSelection = ResourcesViewMgr.AllBoundUsed.Used);
						int num;
						if (!ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
						{
							ResourcesViewMgr.m_costMetricCount[this.m_viewMgr.m_currentCaptureId] = QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)this.m_viewMgr.m_currentCaptureId).MetricsList.Count;
						}
						if (this.m_newCapture)
						{
							VkCapture capture2 = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
							capture2.PopulateASHierarchies(this.dataModel, this.model);
							capture2.PopulateTileMemory(this.dataModel, this.model);
						}
					}
					this.m_statusArgs = new SetStatusEventArgs();
					this.m_statusArgs.Duration = 0;
					this.m_statusArgs.StatusText = "Processing Resources";
					this.m_statusArgs.Status = StatusType.Warning;
					this.m_viewMgr.AddStatus(this.m_statusArgs);
					if (!this.m_viewMgr.m_selectedApi.TryGetValue((uint)this.m_viewMgr.m_currentCaptureId, out this.m_selectedDrawAPI))
					{
						this.m_selectedDrawAPI = uint.MaxValue;
						this.m_viewMgr.m_selectedApi[(uint)this.m_viewMgr.m_currentCaptureId] = this.m_selectedDrawAPI;
						this.m_viewMgr.m_selectedDrawCallID[(uint)this.m_viewMgr.m_currentCaptureId] = string.Empty;
					}
					if (parentBoundInfo != null)
					{
						this.m_boundInfo = QGLPlugin.VkSnapshotModel.GetParentBoundInfo((uint)this.m_viewMgr.m_currentCaptureId, parentBoundInfo);
					}
					else
					{
						QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_viewMgr.m_currentCaptureId, this.m_selectedDrawAPI, out this.m_boundInfo);
					}
					if (this.m_allBoundUsed == ResourcesViewMgr.AllBoundUsed.Used)
					{
						this.UpdateUsedResources();
					}
					this.m_formatsVisible.Clear();
					this.m_debugMarkersVisible.Clear();
					this.m_hasTileMemory = false;
					this.InvalidatePipelines();
					this.InvalidateImages();
					this.InvalidateMemoryBuffers();
					this.InvalidateShaderModules();
					this.InvalidateTraversalShaders();
					this.InvalidateImageViews();
					this.InvalidateAccelerationStructure();
					this.InvalidateTensors();
					this.InvalidateTensorViews();
					this.InvalidateDescriptorSets();
					this.InvalidateDescriptorSetLayouts();
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						this.AddSorts();
						this.AddFilters();
					}
					else if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive)
					{
						SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ResourceViewEvents.ClearFilter, this, new EventArgs());
						this.AddFilters();
					}
				}
				DateTime now2 = DateTime.Now;
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
				{
					QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId).HasResources = true;
					ResourcesViewMgr.Logger.LogInformation("ResourcesView processing: " + (now2 - now).TotalMilliseconds.ToString() + " ms");
					this.m_viewMgr.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateBinaryData, false);
					this.m_viewMgr.Invalidate(ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate, false);
				}
			}

			// Token: 0x060001FE RID: 510 RVA: 0x000179EC File Offset: 0x00015BEC
			public bool UpdateBinaryData()
			{
				if (!QGLPlugin.VkSnapshotModel.ShouldGenerateImages(this.m_viewMgr.m_currentCaptureId))
				{
					return false;
				}
				QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId).HasThumbnails = true;
				this.m_statusArgs = new SetStatusEventArgs();
				this.m_statusArgs.Duration = 0;
				this.m_statusArgs.StatusText = "Processing Images";
				this.m_statusArgs.Status = StatusType.Neutral;
				this.m_viewMgr.AddStatus(this.m_statusArgs);
				uint num;
				if (this.m_viewMgr.m_selectedApi.TryGetValue((uint)this.m_viewMgr.m_currentCaptureId, out num))
				{
					VkBoundInfo vkBoundInfo;
					QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_viewMgr.m_currentCaptureId, num, out vkBoundInfo);
					if (vkBoundInfo != null)
					{
						QGLPlugin.ShaderMgr.DisplayPipelineStages(this.m_viewMgr.m_currentCaptureId, vkBoundInfo.BoundPipeline, num);
					}
				}
				Dictionary<long, byte[]> dictionary = new Dictionary<long, byte[]>();
				this.dataModel = SdpApp.ConnectionManager.GetDataModel();
				this.model = this.dataModel.GetModel("VulkanSnapshot");
				this.imageViewsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotImageViews");
				this.textureMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotTextures");
				this.textures = this.textureMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				foreach (ModelObjectData modelObjectData in this.textures)
				{
					ulong num2 = (ulong)UintConverter.Convert(modelObjectData.GetValue("resourceID"));
					IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer(this.m_viewMgr.m_currentCaptureId, num2);
					if (byteBuffer != null)
					{
						uint num3 = UintConverter.Convert(modelObjectData.GetValue("width"));
						uint num4 = UintConverter.Convert(modelObjectData.GetValue("height"));
						VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData.GetValue("format"));
						BinaryDataPair bdp = byteBuffer.BDP;
						if (bdp != null && bdp.size > 0U)
						{
							byte[] array = new byte[bdp.size];
							Marshal.Copy(bdp.data, array, 0, (int)bdp.size);
							dictionary.Add((long)num2, VkHelper.GenerateThumbnail(array, vkFormats, num3, num4, 64U, 64U));
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
				}
				if (dictionary.Count > 0)
				{
					UpdateResourcePixBufArgs updateResourcePixBufArgs = new UpdateResourcePixBufArgs();
					updateResourcePixBufArgs.CategoryID = 0;
					updateResourcePixBufArgs.Items = dictionary;
					SdpApp.EventsManager.Raise<UpdateResourcePixBufArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateResourcePixBuf, this, updateResourcePixBufArgs);
				}
				return true;
			}

			// Token: 0x060001FF RID: 511 RVA: 0x00017C9C File Offset: 0x00015E9C
			public bool Prepopulate()
			{
				this.m_allBoundUsed = ResourcesViewMgr.AllBoundUsed.Used;
				Dictionary<VkSnapshotModel.ResourceKey, List<uint>> dictionary;
				if (QGLPlugin.VkSnapshotModel.DrawcallsPerResource.TryGetValue(this.m_viewMgr.m_currentCaptureId, out dictionary))
				{
					UpdateCostBarArgs updateCostBarArgs = new UpdateCostBarArgs();
					foreach (KeyValuePair<string, Dictionary<uint, double>> keyValuePair in QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)this.m_viewMgr.m_currentCaptureId).MetricsList)
					{
						updateCostBarArgs.CostBarArgsList.Add(QGLPlugin.VkSnapshotModel.ResourceCostList[Tuple.Create<int, string>(this.m_viewMgr.m_currentCaptureId, keyValuePair.Key)]);
					}
					SdpApp.EventsManager.Raise<UpdateCostBarArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateCostBar, this, updateCostBarArgs);
					return false;
				}
				dictionary = (QGLPlugin.VkSnapshotModel.DrawcallsPerResource[this.m_viewMgr.m_currentCaptureId] = new Dictionary<VkSnapshotModel.ResourceKey, List<uint>>());
				this.m_statusArgs = new SetStatusEventArgs();
				this.m_statusArgs.Duration = 0;
				this.m_statusArgs.StatusText = "Processing used resources";
				this.m_statusArgs.Status = StatusType.Neutral;
				this.m_viewMgr.AddStatus(this.m_statusArgs);
				this.InitInstanceVars();
				foreach (KeyValuePair<uint, List<PrepopulateCategoryArgs>> keyValuePair2 in QGLPlugin.VkSnapshotModel.ResourcesPerDrawcall)
				{
					this.m_selectedDrawAPI = keyValuePair2.Key;
					QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_viewMgr.m_currentCaptureId, this.m_selectedDrawAPI, out this.m_boundInfo);
					this.UpdateUsedResources();
					this.PrepopulateInvalidate();
				}
				bool flag = false;
				if (QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId).ResourceIDs[7].Count > 1000)
				{
					ResourcesViewMgr.Logger.LogWarning("Omitting resource cost bars for the descriptor sets in capture " + this.m_viewMgr.m_currentCaptureId.ToString());
					flag = true;
				}
				VkMetricsCapturedModel metricsForCapture = QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)this.m_viewMgr.m_currentCaptureId);
				Dictionary<string, Dictionary<int, Dictionary<long, double>>> accumulatedMetrics = metricsForCapture.AccumulatedMetrics;
				Dictionary<string, Dictionary<uint, double>> metricsList = metricsForCapture.MetricsList;
				foreach (KeyValuePair<uint, List<PrepopulateCategoryArgs>> keyValuePair3 in QGLPlugin.VkSnapshotModel.ResourcesPerDrawcall)
				{
					foreach (PrepopulateCategoryArgs prepopulateCategoryArgs in keyValuePair3.Value)
					{
						foreach (long num in prepopulateCategoryArgs.ResourceIds)
						{
							VkSnapshotModel.ResourceKey resourceKey = new VkSnapshotModel.ResourceKey(prepopulateCategoryArgs.CategoryId, num);
							List<uint> list;
							if (!dictionary.TryGetValue(resourceKey, out list))
							{
								list = (dictionary[resourceKey] = new List<uint>());
							}
							list.Add(keyValuePair3.Key);
							if (prepopulateCategoryArgs.CategoryId != 7 || !flag)
							{
								foreach (KeyValuePair<string, Dictionary<uint, double>> keyValuePair4 in metricsList)
								{
									Dictionary<int, Dictionary<long, double>> dictionary2;
									if (!accumulatedMetrics.TryGetValue(keyValuePair4.Key, out dictionary2))
									{
										dictionary2 = (accumulatedMetrics[keyValuePair4.Key] = new Dictionary<int, Dictionary<long, double>>());
									}
									Dictionary<long, double> dictionary3;
									if (!dictionary2.TryGetValue(prepopulateCategoryArgs.CategoryId, out dictionary3))
									{
										dictionary3 = (dictionary2[prepopulateCategoryArgs.CategoryId] = new Dictionary<long, double>());
									}
									double num2;
									if (!dictionary3.TryGetValue(num, out num2))
									{
										dictionary3[num] = 0.0;
									}
									double num3 = 0.0;
									if (metricsList[keyValuePair4.Key].TryGetValue(keyValuePair3.Key, out num3))
									{
										Dictionary<long, double> dictionary4 = dictionary3;
										long num4 = num;
										dictionary4[num4] += num3;
									}
								}
							}
						}
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				int num5 = 0;
				foreach (KeyValuePair<string, Dictionary<uint, double>> keyValuePair5 in metricsList)
				{
					MetricsCost metricsCost = new MetricsCost();
					metricsCost.Column = 10 + num5;
					num5 += 2;
					foreach (KeyValuePair<int, Dictionary<long, double>> keyValuePair6 in accumulatedMetrics[keyValuePair5.Key])
					{
						ResourceCostDict resourceCostDict = (metricsCost.Items[keyValuePair6.Key] = new ResourceCostDict());
						if (keyValuePair6.Value.Count == 1)
						{
							resourceCostDict[Enumerable.First<KeyValuePair<long, double>>(keyValuePair6.Value).Key] = 100.0;
						}
						else
						{
							double num6 = Enumerable.Min(Enumerable.Select<KeyValuePair<long, double>, double>(keyValuePair6.Value, (KeyValuePair<long, double> r) => r.Value));
							double num7 = Enumerable.Max(Enumerable.Select<KeyValuePair<long, double>, double>(keyValuePair6.Value, (KeyValuePair<long, double> r) => r.Value));
							foreach (KeyValuePair<long, double> keyValuePair7 in keyValuePair6.Value)
							{
								if (num7 == num6)
								{
									resourceCostDict[keyValuePair7.Key] = 100.0;
								}
								else
								{
									resourceCostDict[keyValuePair7.Key] = 100.0 * (keyValuePair7.Value - num6) / (num7 - num6);
								}
							}
						}
					}
					Tuple<int, string> tuple = new Tuple<int, string>(this.m_viewMgr.m_currentCaptureId, keyValuePair5.Key);
					QGLPlugin.VkSnapshotModel.ResourceCostList[tuple] = metricsCost;
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				UpdateCostBarArgs updateCostBarArgs2 = new UpdateCostBarArgs();
				foreach (KeyValuePair<string, Dictionary<uint, double>> keyValuePair8 in QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)this.m_viewMgr.m_currentCaptureId).MetricsList)
				{
					updateCostBarArgs2.CostBarArgsList.Add(QGLPlugin.VkSnapshotModel.ResourceCostList[Tuple.Create<int, string>(this.m_viewMgr.m_currentCaptureId, keyValuePair8.Key)]);
				}
				SdpApp.EventsManager.Raise<UpdateCostBarArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateCostBar, this, updateCostBarArgs2);
				return true;
			}

			// Token: 0x06000200 RID: 512 RVA: 0x00018438 File Offset: 0x00016638
			private void PrepopulateInvalidate()
			{
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				ResourcesViewMgr.ResourceCategory[] array = new ResourcesViewMgr.ResourceCategory[]
				{
					ResourcesViewMgr.ResourceCategory.GraphicsPipelines,
					ResourcesViewMgr.ResourceCategory.ComputePipelines,
					ResourcesViewMgr.ResourceCategory.RayTracingPipelines
				};
				foreach (ResourcesViewMgr.ResourceCategory resourceCategory in array)
				{
					if (resourceCategory != ResourcesViewMgr.ResourceCategory.GraphicsPipelines)
					{
						if (resourceCategory != ResourcesViewMgr.ResourceCategory.ComputePipelines)
						{
							ModelObjectDataList modelObjectDataList = this.raytracingPipelines;
						}
						else
						{
							ModelObjectDataList modelObjectDataList = this.computePipelines;
						}
					}
					else
					{
						ModelObjectDataList modelObjectDataList = this.graphicsPipelines;
					}
					PrepopulateCategoryArgs prepopulateCategoryArgs = new PrepopulateCategoryArgs();
					prepopulateCategoryArgs.Source = 353;
					prepopulateCategoryArgs.DrawcallId = (int)this.m_selectedDrawAPI;
					prepopulateCategoryArgs.CategoryId = (int)resourceCategory;
					List<ulong> list;
					if (this.m_boundInfo != null && capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs.CategoryId, out list))
					{
						foreach (ulong num in list)
						{
							bool flag;
							if (this.m_boundInfo.IsDrawcallParent)
							{
								flag = this.m_boundInfo.ParentPipelines.Contains(num);
							}
							else
							{
								flag = num == this.m_boundInfo.BoundPipeline;
							}
							if (flag)
							{
								prepopulateCategoryArgs.ResourceIds.Add((long)num);
							}
						}
						this.m_cancelToken.ThrowIfCancellationRequested();
						SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs);
					}
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs2 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs2.Source = 353;
				prepopulateCategoryArgs2.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs2.CategoryId = 0;
				List<ulong> list2;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs2.CategoryId, out list2))
				{
					foreach (ulong num2 in list2)
					{
						if (this.m_usedImages.Contains(num2))
						{
							prepopulateCategoryArgs2.ResourceIds.Add((long)num2);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs2);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs3 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs3.Source = 353;
				prepopulateCategoryArgs3.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs3.CategoryId = 1;
				List<ulong> list3;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs3.CategoryId, out list3))
				{
					foreach (ulong num3 in list3)
					{
						if (this.m_usedMemoryBuffers.Contains(num3))
						{
							prepopulateCategoryArgs3.ResourceIds.Add((long)(num3 + 18446744069414584320UL));
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs3);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs4 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs4.Source = 353;
				prepopulateCategoryArgs4.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs4.CategoryId = 2;
				List<ulong> list4;
				if (this.m_boundInfo != null && capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs4.CategoryId, out list4))
				{
					foreach (ulong num4 in list4)
					{
						HashSet<ulong> hashSet = new HashSet<ulong>();
						List<ResourcesViewMgr.StagePipelineIDTuple> list5;
						if (this.shaderStages.TryGetValue(num4, out list5))
						{
							foreach (ResourcesViewMgr.StagePipelineIDTuple stagePipelineIDTuple in list5)
							{
								hashSet.Add(stagePipelineIDTuple.PipelineID);
							}
						}
						bool flag2;
						if (this.m_boundInfo.IsDrawcallParent)
						{
							flag2 = Enumerable.Any<ulong>(Enumerable.Intersect<ulong>(hashSet, this.m_boundInfo.ParentPipelines));
						}
						else
						{
							flag2 = hashSet.Contains(this.m_boundInfo.BoundPipeline);
						}
						if (flag2)
						{
							prepopulateCategoryArgs4.ResourceIds.Add((long)num4);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs4);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs5 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs5.Source = 353;
				prepopulateCategoryArgs5.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs5.CategoryId = 9;
				List<ulong> list6;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs5.CategoryId, out list6))
				{
					foreach (ulong num5 in list6)
					{
						if (this.m_usedAccelerationStructs.Contains(num5))
						{
							prepopulateCategoryArgs5.ResourceIds.Add((long)(num5 + 18446744069414584320UL));
							ulong[] array3;
							if (capture.ASHierarchies.TryGetValue(num5, out array3))
							{
								for (int j = 0; j < Enumerable.Count<ulong>(array3); j++)
								{
									long num6 = (long)(array3[j] + 18446744069414584320UL);
									if (!prepopulateCategoryArgs5.ResourceIds.Contains(num6))
									{
										prepopulateCategoryArgs5.ResourceIds.Add(num6);
									}
									long num7 = (long)(num5 + (ulong)((ulong)((long)j) << 32));
									if (!prepopulateCategoryArgs5.ResourceIds.Contains(num7))
									{
										prepopulateCategoryArgs5.ResourceIds.Add(num7);
									}
								}
							}
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs5);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs6 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs6.Source = 353;
				prepopulateCategoryArgs6.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs6.CategoryId = 7;
				List<ulong> list7;
				if (this.m_boundInfo != null && capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs6.CategoryId, out list7))
				{
					foreach (ulong num8 in list7)
					{
						if (this.m_boundInfo.ContainsDescriptorSet(num8))
						{
							prepopulateCategoryArgs6.ResourceIds.Add((long)(num8 + 18446744069414584320UL));
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs6);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs7 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs7.Source = 353;
				prepopulateCategoryArgs7.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs7.CategoryId = 8;
				List<ulong> list8;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs7.CategoryId, out list8))
				{
					foreach (ulong num9 in list8)
					{
						if (this.m_usedDescriptorLayouts.Contains(num9))
						{
							prepopulateCategoryArgs7.ResourceIds.Add((long)num9);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs7);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs8 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs8.Source = 353;
				prepopulateCategoryArgs8.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs8.CategoryId = 6;
				List<ulong> list9;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs8.CategoryId, out list9))
				{
					foreach (ulong num10 in list9)
					{
						if (this.m_usedImageViews.Contains(num10))
						{
							prepopulateCategoryArgs8.ResourceIds.Add((long)num10);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs8);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs9 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs9.Source = 353;
				prepopulateCategoryArgs9.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs9.CategoryId = 11;
				List<ulong> list10;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs9.CategoryId, out list10))
				{
					foreach (ulong num11 in list10)
					{
						if (this.m_usedTensors.Contains(num11))
						{
							prepopulateCategoryArgs9.ResourceIds.Add((long)num11);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs9);
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs10 = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs10.Source = 353;
				prepopulateCategoryArgs10.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs10.CategoryId = 12;
				List<ulong> list11;
				if (capture.ResourceIDs.TryGetValue(prepopulateCategoryArgs10.CategoryId, out list11))
				{
					foreach (ulong num12 in list11)
					{
						if (this.m_usedTensorViews.Contains(num12))
						{
							prepopulateCategoryArgs10.ResourceIds.Add((long)num12);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs10);
				}
			}

			// Token: 0x06000201 RID: 513 RVA: 0x00018D9C File Offset: 0x00016F9C
			public void InitTraceInstanceVars()
			{
				this.dataModel = SdpApp.ConnectionManager.GetDataModel();
				this.model = this.dataModel.GetModel("QGLModel");
				this.shaderDataObj = this.dataModel.GetModelObject(this.model, "VulkanTraceShaderData");
				this.traceGraphicsPipelines = this.shaderDataObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.shaderStagesPerPipeline = ResourcesViewMgr.ConvertShaderStagePerPipelineToDictionary(this.traceGraphicsPipelines);
			}

			// Token: 0x06000202 RID: 514 RVA: 0x00018E24 File Offset: 0x00017024
			public void InitInstanceVars()
			{
				this.dataModel = SdpApp.ConnectionManager.GetDataModel();
				this.model = this.dataModel.GetModel("VulkanSnapshot");
				this.imageViewsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotImageViews");
				this.descriptorSetLinksMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotDescriptorSetLayoutLinks");
				this.descSetLinks = ResourcesViewMgr.ConvertDescSetLinksToDictionary(this.descriptorSetLinksMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString()));
				this.textureMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotTextures");
				this.textures = this.textureMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.objLabelMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotObjectLabels");
				this.objLabels = this.objLabelMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.labels = ResourcesViewMgr.ConvertToDictionaryWithDuplicates(this.objLabels);
				this.shaderModuleMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotShaderModules");
				this.shaderStageMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotShaderStages");
				this.shaderModules = this.shaderModuleMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.shaderStages = ResourcesViewMgr.ConvertShaderStageToDictionary(this.shaderStageMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString()));
				this.graphicsPipelineMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotGraphicsPipelines");
				this.graphicsPipelines = this.graphicsPipelineMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.computePipelineMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotComputePipelines");
				this.computePipelines = this.computePipelineMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.raytracingPipelineMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotRaytracingPipelines");
				this.raytracingPipelines = this.raytracingPipelineMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.pipelineLibraryMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotPipelineLibraries");
				this.imageViews = this.imageViewsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.images = ResourcesViewMgr.ConvertImageViewsToDictionary(this.imageViews);
				this.descSetsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotDescriptorSets");
				this.descriptorSets = this.descSetsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.descSetLayoutsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotDescriptorSetLayouts");
				this.descriptorSetLayouts = this.descSetLayoutsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.memoryMdlObj = this.dataModel.GetModelObject(this.model, "VulkanSnapshotMemoryBuffers");
				this.memoryBuffers = this.memoryMdlObj.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.asInfoMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotASInfo");
				this.asInfo = this.asInfoMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.asInstanceDescriptorsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotASInstanceDescriptor");
				this.asInstanceDescriptors = this.asInstanceDescriptorsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.snapshotShaderDataMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotShaderData");
				this.traversalShaders = this.snapshotShaderDataMO.GetData(new StringList
				{
					"captureID",
					this.m_viewMgr.m_currentCaptureId.ToString(),
					"shaderStage",
					12.ToString()
				});
				this.tensorsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotTensors");
				this.tensors = this.tensorsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
				this.tensorViewsMO = this.dataModel.GetModelObject(this.model, "VulkanSnapshotTensorsView");
				this.tensorViews = this.tensorViewsMO.GetData("captureID", this.m_viewMgr.m_currentCaptureId.ToString());
			}

			// Token: 0x06000203 RID: 515 RVA: 0x00019330 File Offset: 0x00017530
			public void InvalidateForFilterUpdate()
			{
				object obj = ResourcesViewMgr.InvalidateClass.invalidateLock;
				lock (obj)
				{
					this.InitInstanceVars();
					this.m_statusArgs = new SetStatusEventArgs();
					this.m_statusArgs.Duration = 0;
					this.m_statusArgs.StatusText = "Processing filters";
					this.m_statusArgs.Status = StatusType.Warning;
					this.m_viewMgr.AddStatus(this.m_statusArgs);
					if (!this.m_viewMgr.m_selectedApi.TryGetValue((uint)this.m_viewMgr.m_currentCaptureId, out this.m_selectedDrawAPI))
					{
						this.m_selectedDrawAPI = uint.MaxValue;
						this.m_viewMgr.m_selectedApi[(uint)this.m_viewMgr.m_currentCaptureId] = this.m_selectedDrawAPI;
						this.m_viewMgr.m_selectedDrawCallID[(uint)this.m_viewMgr.m_currentCaptureId] = string.Empty;
					}
					QGLPlugin.VkSnapshotModel.GetBoundInfo((uint)this.m_viewMgr.m_currentCaptureId, this.m_selectedDrawAPI, out this.m_boundInfo);
					this.UpdateUsedResources();
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
					{
						this.InvalidatePipelines();
						this.InvalidateImages();
						this.InvalidateMemoryBuffers();
						this.InvalidateShaderModules();
						this.InvalidateTraversalShaders();
						this.InvalidateImageViews();
						this.InvalidateAccelerationStructure();
						this.InvalidateTensors();
						this.InvalidateTensorViews();
						this.InvalidateDescriptorSets();
						this.InvalidateDescriptorSetLayouts();
					}
					else if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateImageFormats)
					{
						this.InvalidateImages();
					}
					else if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateTileMemory)
					{
						this.InvalidateImages();
						this.InvalidateMemoryBuffers();
					}
				}
			}

			// Token: 0x06000204 RID: 516 RVA: 0x000194C4 File Offset: 0x000176C4
			private void AddSorts()
			{
				AddSortComboArgs addSortComboArgs = new AddSortComboArgs();
				Dictionary<string, Dictionary<uint, double>> metricsList = QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)this.m_viewMgr.m_currentCaptureId).MetricsList;
				if (metricsList.Count > 0)
				{
					addSortComboArgs.Items = new SortItem[3 + metricsList.Count];
					int num = 0;
					int num2 = 0;
					using (Dictionary<string, Dictionary<uint, double>>.Enumerator enumerator = metricsList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, Dictionary<uint, double>> keyValuePair = enumerator.Current;
							string text = "Sort by " + keyValuePair.Key + " Cost";
							addSortComboArgs.Items[3 + num2++] = new SortItem(text, 10 + num, SortItem.Sort.Descending);
							num += 2;
						}
						goto IL_00AF;
					}
				}
				addSortComboArgs.Items = new SortItem[3];
				IL_00AF:
				addSortComboArgs.Items[0] = new SortItem("Sort by Id", 0, SortItem.Sort.Ascending);
				addSortComboArgs.Items[1] = new SortItem("Sort by Size", 6, SortItem.Sort.Descending);
				addSortComboArgs.Items[2] = new SortItem("Sort by Format", 7, SortItem.Sort.Ascending);
				SdpApp.EventsManager.Raise<AddSortComboArgs>(SdpApp.EventsManager.ResourceViewEvents.AddSortComboBox, this, addSortComboArgs);
			}

			// Token: 0x06000205 RID: 517 RVA: 0x000195E8 File Offset: 0x000177E8
			private void AddFilters()
			{
				EntryViewComboFilter entryViewComboFilter = new EntryViewComboFilter();
				entryViewComboFilter.SearchStrings = new SearchString[3];
				entryViewComboFilter.SearchStrings[0] = new SearchString();
				entryViewComboFilter.SearchStrings[0].DisplayString = "All Resources";
				entryViewComboFilter.SearchStrings[0].LookupString = null;
				entryViewComboFilter.SearchStrings[0].Tooltip = "Show all resources";
				entryViewComboFilter.SearchStrings[1] = new SearchString();
				entryViewComboFilter.SearchStrings[1].DisplayString = "Bound Only";
				entryViewComboFilter.SearchStrings[1].LookupString = "true";
				entryViewComboFilter.SearchStrings[1].Tooltip = "Show resources bound with vkCmdBind* at the selected drawcall";
				entryViewComboFilter.SearchStrings[2] = new SearchString();
				entryViewComboFilter.SearchStrings[2].DisplayString = "Used Only";
				entryViewComboFilter.SearchStrings[2].LookupString = "true";
				entryViewComboFilter.SearchStrings[2].Tooltip = "Show resources used by the selected drawcall";
				switch (this.m_allBoundUsed)
				{
				case ResourcesViewMgr.AllBoundUsed.All:
					entryViewComboFilter.SearchStrings[0].Default = true;
					break;
				case ResourcesViewMgr.AllBoundUsed.Bound:
					entryViewComboFilter.SearchStrings[1].Default = true;
					break;
				case ResourcesViewMgr.AllBoundUsed.Used:
					entryViewComboFilter.SearchStrings[2].Default = true;
					break;
				}
				entryViewComboFilter.FilterName = "All/Bound/Used";
				entryViewComboFilter.OnSelectionChanged = new EventHandler<ComboBoxSelectionChangedArgs>(this.m_viewMgr.OnAllBoundUsedToggled);
				entryViewComboFilter.FilterColumn = 5;
				entryViewComboFilter.SearchMode = SearchMode.Bool;
				AddFilterEventArgs addFilterEventArgs = new AddFilterEventArgs();
				addFilterEventArgs.Filter = entryViewComboFilter;
				SdpApp.EventsManager.Raise<AddFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.AddFilter, this, addFilterEventArgs);
				if (this.m_formatsVisible.Count > 1)
				{
					EntryViewComboFilter entryViewComboFilter2 = new EntryViewComboFilter();
					entryViewComboFilter2.SearchStrings = new SearchString[this.m_formatsVisible.Count + 1];
					entryViewComboFilter2.SearchStrings[0] = new SearchString();
					entryViewComboFilter2.SearchStrings[0].DisplayString = "All Formats";
					entryViewComboFilter2.SearchStrings[0].LookupString = null;
					entryViewComboFilter2.SearchStrings[0].Default = true;
					for (int i = 0; i < this.m_formatsVisible.Count; i++)
					{
						entryViewComboFilter2.SearchStrings[i + 1] = new SearchString();
						entryViewComboFilter2.SearchStrings[i + 1].DisplayString = Enumerable.ElementAt<string>(this.m_formatsVisible, i);
						entryViewComboFilter2.SearchStrings[i + 1].LookupString = Enumerable.ElementAt<string>(this.m_formatsVisible, i);
					}
					entryViewComboFilter2.FilterName = this.m_viewMgr.FORMAT_COMBO_FILTER_NAME;
					entryViewComboFilter2.OnSelectionChanged = new EventHandler<ComboBoxSelectionChangedArgs>(this.m_viewMgr.OnFormatToggled);
					entryViewComboFilter2.FilterColumn = 5 + ResourcesViewMgr.FORMAT_FILTER_OFFSET;
					entryViewComboFilter2.SearchMode = SearchMode.Optional;
					AddFilterEventArgs addFilterEventArgs2 = new AddFilterEventArgs();
					addFilterEventArgs2.Filter = entryViewComboFilter2;
					SdpApp.EventsManager.Raise<AddFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.AddFilter, this, addFilterEventArgs2);
				}
				if (this.m_debugMarkersVisible.Count > 1)
				{
					EntryViewComboFilter entryViewComboFilter3 = new EntryViewComboFilter();
					entryViewComboFilter3.SearchStrings = new SearchString[this.m_debugMarkersVisible.Count + 1];
					entryViewComboFilter3.SearchStrings[0] = new SearchString();
					entryViewComboFilter3.SearchStrings[0].DisplayString = "All Objects";
					entryViewComboFilter3.SearchStrings[0].LookupString = null;
					entryViewComboFilter3.SearchStrings[0].Default = true;
					for (int j = 0; j < this.m_debugMarkersVisible.Count; j++)
					{
						entryViewComboFilter3.SearchStrings[j + 1] = new SearchString();
						entryViewComboFilter3.SearchStrings[j + 1].DisplayString = Enumerable.ElementAt<string>(this.m_debugMarkersVisible, j);
						entryViewComboFilter3.SearchStrings[j + 1].LookupString = Enumerable.ElementAt<string>(this.m_debugMarkersVisible, j);
					}
					entryViewComboFilter3.FilterName = this.m_viewMgr.MARKER_COMBO_FILTER_NAME;
					entryViewComboFilter3.OnSelectionChanged = new EventHandler<ComboBoxSelectionChangedArgs>(this.m_viewMgr.OnMarkerTextToggled);
					entryViewComboFilter3.FilterColumn = 5 + ResourcesViewMgr.MARKER_FILTER_OFFSET;
					entryViewComboFilter3.SearchMode = SearchMode.Strict;
					AddFilterEventArgs addFilterEventArgs3 = new AddFilterEventArgs();
					addFilterEventArgs3.Filter = entryViewComboFilter3;
					SdpApp.EventsManager.Raise<AddFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.AddFilter, this, addFilterEventArgs3);
				}
				if (this.m_hasTileMemory)
				{
					EntryViewComboFilter entryViewComboFilter4 = new EntryViewComboFilter();
					entryViewComboFilter4.SearchStrings = new SearchString[2];
					entryViewComboFilter4.SearchStrings[0] = new SearchString();
					entryViewComboFilter4.SearchStrings[0].DisplayString = "All Resources";
					entryViewComboFilter4.SearchStrings[0].LookupString = null;
					entryViewComboFilter4.SearchStrings[0].Tooltip = entryViewComboFilter.SearchStrings[0].Tooltip;
					entryViewComboFilter4.SearchStrings[1] = new SearchString();
					entryViewComboFilter4.SearchStrings[1].DisplayString = "pGMEM only";
					entryViewComboFilter4.SearchStrings[1].LookupString = bool.TrueString;
					entryViewComboFilter4.SearchStrings[1].Tooltip = "Show resources used in pGMEM";
					entryViewComboFilter4.FilterName = ResourcesViewMgr.TILE_MEMORY;
					entryViewComboFilter4.OnSelectionChanged = new EventHandler<ComboBoxSelectionChangedArgs>(this.m_viewMgr.OnTileMemoryToggled);
					entryViewComboFilter4.FilterColumn = 5 + ResourcesViewMgr.TILE_FILTER_OFFSET;
					entryViewComboFilter4.SearchMode = SearchMode.Optional;
					AddFilterEventArgs addFilterEventArgs4 = new AddFilterEventArgs();
					addFilterEventArgs4.Filter = entryViewComboFilter4;
					SdpApp.EventsManager.Raise<AddFilterEventArgs>(SdpApp.EventsManager.ResourceViewEvents.AddFilter, this, addFilterEventArgs4);
				}
			}

			// Token: 0x06000206 RID: 518 RVA: 0x00019AFC File Offset: 0x00017CFC
			private void UpdateResourcesActiveToggled(int categoryId, Dictionary<long, object> Items)
			{
				UpdateResourceCustomFilterDataArgs updateResourceCustomFilterDataArgs = new UpdateResourceCustomFilterDataArgs();
				updateResourceCustomFilterDataArgs.CategoryID = categoryId;
				updateResourceCustomFilterDataArgs.Items = Items;
				updateResourceCustomFilterDataArgs.Column = 5;
				SdpApp.EventsManager.Raise<UpdateResourceCustomFilterDataArgs>(SdpApp.EventsManager.ResourceViewEvents.UpdateResourceCustomFilterData, this, updateResourceCustomFilterDataArgs);
			}

			// Token: 0x06000207 RID: 519 RVA: 0x00019B40 File Offset: 0x00017D40
			private void UpdateUsedResources()
			{
				this.m_usedImageViews.Clear();
				this.m_usedImages.Clear();
				this.m_usedMemoryBuffers.Clear();
				this.m_usedDescriptorLayouts.Clear();
				this.m_usedAccelerationStructs.Clear();
				this.m_usedTensors.Clear();
				this.m_usedTensorViews.Clear();
				if (this.m_boundInfo == null)
				{
					return;
				}
				foreach (DescSetBindings descSetBindings in this.m_boundInfo.BoundDescriptorSets.Values)
				{
					foreach (DescSetBindings.DescBindings descBindings in descSetBindings.Bindings.Values)
					{
						this.m_usedImageViews.Add(descBindings.imageViewID);
						this.m_usedAccelerationStructs.Add(descBindings.accelStructID);
						this.m_usedMemoryBuffers.Add(descBindings.bufferID);
						this.m_usedTensors.Add(descBindings.tensorID);
						this.m_usedTensorViews.Add(descBindings.tensorViewID);
					}
					ulong num;
					if (this.descSetLinks.TryGetValue(descSetBindings.DescSetID, out num))
					{
						this.m_usedDescriptorLayouts.Add(num);
					}
				}
				foreach (ulong num2 in this.m_usedImageViews)
				{
					ulong num3;
					if (num2 != 0UL && this.images.TryGetValue(num2, out num3))
					{
						this.m_usedImages.Add(num3);
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
			}

			// Token: 0x06000208 RID: 520 RVA: 0x00019D20 File Offset: 0x00017F20
			private void InvalidateMemoryBuffers()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 1;
				addCategoryArgs.Name = "<b>Memory Buffers - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.memoryBuffers)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedMemoryBuffers.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					bool flag2 = capture.TileMemoryResources.Contains(num3);
					if (this.m_viewMgr.m_tileMemoryOnly && !flag2)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
						if (flag2)
						{
							this.m_hasTileMemory = true;
						}
					}
					switch (this.m_invalidateType)
					{
					case ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange:
					{
						uint num4 = UintConverter.Convert(modelObjectData.GetValue("size"));
						object[] array = new object[]
						{
							flag,
							num4,
							text,
							flag2.ToString()
						};
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = string.Concat(new string[]
						{
							"ID [",
							num3.ToString(),
							"] Size: ",
							num4.ToString(),
							" bytes ",
							text
						});
						if (flag2)
						{
							ResourceItem resourceItem2 = resourceItem;
							resourceItem2.Name += "\n<span foreground='orange'>pGMEM</span>";
						}
						resourceItem.Id = (long)(num3 + 18446744069414584320UL);
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = string.Concat(new string[]
						{
							"Resource ID: ",
							num3.ToString(),
							" ",
							text,
							"\nMemory Size: ",
							num4.ToString()
						});
						IEnumerable<IByteBuffer> byteBuffers = this.m_byteBufferGateway.GetByteBuffers(this.m_viewMgr.m_currentCaptureId, num3);
						if (byteBuffers == null)
						{
							addCategoryArgs.ResourceItems.Add(resourceItem);
						}
						else
						{
							foreach (IByteBuffer byteBuffer in byteBuffers)
							{
								uint sequenceID = byteBuffer.SequenceID;
								if (sequenceID != 4294967295U)
								{
									ResourceItem resourceItem3 = new ResourceItem();
									resourceItem3.Id = (long)(num3 + (ulong)sequenceID);
									resourceItem3.Name = "API Call [" + sequenceID.ToString() + "]";
									resourceItem3.Tooltip = string.Concat(new string[]
									{
										"Resource ID: ",
										text,
										"\nMemory Size: ",
										num4.ToString(),
										"\nDraw Call: ",
										sequenceID.ToString()
									});
									resourceItem3.CustomFilterObjects = array;
									resourceItem.Children.Add(resourceItem3);
								}
							}
							addCategoryArgs.ResourceItems.Add(resourceItem);
						}
						break;
					}
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateTileMemory:
						dictionary[(long)(num3 + 18446744069414584320UL)] = flag;
						break;
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(1, dictionary);
				}
				if (this.memoryBuffers.Count > 0)
				{
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = 1;
					updateCategoryNumVisible.Total = this.memoryBuffers.Count;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x06000209 RID: 521 RVA: 0x0001A268 File Offset: 0x00018468
			private void InvalidateImages()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 0;
				addCategoryArgs.Name = "<b>Images - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.Icons;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.textures)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedImages.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					string textureFormatString = VkHelper.GetTextureFormatString(modelObjectData.GetValue("format"));
					if ((this.m_viewMgr.m_formatSelection != "All Formats" && this.m_viewMgr.m_formatSelection != textureFormatString) || (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text))
					{
						flag = false;
					}
					bool flag2 = capture.TileMemoryResources.Contains(num3);
					if (this.m_viewMgr.m_tileMemoryOnly && !flag2)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						this.m_formatsVisible.Add(textureFormatString);
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
						if (flag2)
						{
							this.m_hasTileMemory = true;
						}
					}
					switch (this.m_invalidateType)
					{
					case ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange:
					{
						string value = modelObjectData.GetValue("layerCount");
						string value2 = modelObjectData.GetValue("levelCount");
						string value3 = modelObjectData.GetValue("sampleCount");
						uint num4 = UintConverter.Convert(modelObjectData.GetValue("width"));
						uint num5 = UintConverter.Convert(modelObjectData.GetValue("height"));
						uint num6 = UintConverter.Convert(modelObjectData.GetValue("depth"));
						object[] array = new object[]
						{
							flag,
							num4 * num5,
							text,
							flag2.ToString(),
							textureFormatString
						};
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "<small>[<span foreground='orange'>" + num3.ToString() + "</span>]\n";
						ResourceItem resourceItem2 = resourceItem;
						resourceItem2.Name += ((text.Length > 0) ? (text + "\n") : "");
						ResourceItem resourceItem3 = resourceItem;
						resourceItem3.Name = resourceItem3.Name + num4.ToString() + "x" + num5.ToString();
						if (flag2)
						{
							ResourceItem resourceItem4 = resourceItem;
							resourceItem4.Name += "\n<span foreground='orange'>pGMEM</span>";
						}
						if (num6 > 1U)
						{
							ResourceItem resourceItem5 = resourceItem;
							resourceItem5.Name = resourceItem5.Name + "x" + num6.ToString();
						}
						ResourceItem resourceItem6 = resourceItem;
						resourceItem6.Name = resourceItem6.Name + "</small>\n<span size='x-small' foreground='green'>" + textureFormatString + "</span>\n";
						resourceItem.Id = (long)((int)num3);
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = string.Concat(new string[]
						{
							"Resource ID: ",
							num3.ToString(),
							" ",
							text,
							"\nFormat: ",
							textureFormatString,
							"\nNum Layers: ",
							value,
							"\nMip Levels: ",
							value2,
							"\nSample Count: ",
							value3
						});
						addCategoryArgs.ResourceItems.Add(resourceItem);
						break;
					}
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateImageFormats:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateTileMemory:
						dictionary[(long)num3] = flag;
						break;
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(0, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 0;
				updateCategoryNumVisible.Total = this.textures.Count;
				updateCategoryNumVisible.Visible = num2;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x0600020A RID: 522 RVA: 0x0001A7B0 File Offset: 0x000189B0
			private void InvalidateShaderModules()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 2;
				addCategoryArgs.Name = "<b>Shader Modules - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.shaderModules)
				{
					bool flag = false;
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					uint num4 = 0U;
					HashSet<ulong> hashSet = new HashSet<ulong>();
					List<ResourcesViewMgr.StagePipelineIDTuple> list2;
					if (this.shaderStages.TryGetValue(num3, out list2))
					{
						if (list2.Count > 0)
						{
							num4 = list2[0].StageType;
						}
						foreach (ResourcesViewMgr.StagePipelineIDTuple stagePipelineIDTuple in list2)
						{
							hashSet.Add(stagePipelineIDTuple.PipelineID);
						}
					}
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						if (this.m_boundInfo != null)
						{
							if (this.m_boundInfo.IsDrawcallParent)
							{
								flag = Enumerable.Any<ulong>(Enumerable.Intersect<ulong>(hashSet, this.m_boundInfo.ParentPipelines));
							}
							else
							{
								flag = hashSet.Contains(this.m_boundInfo.BoundPipeline);
								if (!flag)
								{
									HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed(this.m_viewMgr.m_currentCaptureId, this.m_boundInfo.BoundPipeline, this.pipelineLibraryMdlObj);
									flag |= Enumerable.Any<ulong>(Enumerable.Intersect<ulong>(hashSet, pipelineLibraryUsed));
								}
							}
						}
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
					if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
						{
							dictionary[(long)num3] = flag;
						}
					}
					else
					{
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						string text2 = "";
						foreach (ulong num5 in hashSet)
						{
							text2 += (string.IsNullOrEmpty(text2) ? num5.ToString() : ("," + num5.ToString()));
						}
						string shaderStageText = VkHelper.GetShaderStageText(num4, true);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = string.Concat(new string[]
						{
							"ID [",
							num3.ToString(),
							"], StageType: ",
							shaderStageText,
							" ",
							text
						});
						resourceItem.Id = (long)num3;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = string.Concat(new string[]
						{
							"Shader Module ID: ",
							num3.ToString(),
							" ",
							text,
							"\nStageType: ",
							shaderStageText,
							"\nPipelines Used by: ",
							text2
						});
						addCategoryArgs.ResourceItems.Add(resourceItem);
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(2, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 2;
				updateCategoryNumVisible.Total = this.shaderModules.Count;
				updateCategoryNumVisible.Visible = num2;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x0600020B RID: 523 RVA: 0x0001ACD0 File Offset: 0x00018ED0
			private void InvalidateTraceShaderData()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 3;
				addCategoryArgs.Name = "<b>Pipelines - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(ulong),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				int num2 = 0;
				foreach (KeyValuePair<ulong, List<uint>> keyValuePair in this.shaderStagesPerPipeline)
				{
					ulong key = keyValuePair.Key;
					List<uint> value = keyValuePair.Value;
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						string empty = string.Empty;
						object[] array = new object[] { "true", key };
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "ID [" + string.Format("0x{0:X}", key) + "]";
						resourceItem.Id = (long)key;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = string.Format("Pipeline ID: {0}", string.Format("0x{0:X}", key));
						addCategoryArgs.ResourceItems.Add(resourceItem);
						num2++;
					}
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (this.m_invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate && addCategoryArgs.ResourceItems.Count > 0)
				{
					ItemSelectedEventArgs itemSelectedEventArgs = new ItemSelectedEventArgs();
					itemSelectedEventArgs.CategoryID = 3;
					itemSelectedEventArgs.ResourceIDs = new long[] { addCategoryArgs.ResourceItems[0].Id };
					EventHandler<ItemSelectedEventArgs> selectItem = SdpApp.EventsManager.ResourceViewEvents.SelectItem;
					if (selectItem != null)
					{
						selectItem(this, itemSelectedEventArgs);
					}
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = 3;
					updateCategoryNumVisible.Total = num2;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x0600020C RID: 524 RVA: 0x0001AF38 File Offset: 0x00019138
			private void InvalidateTraversalShaders()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 10;
				addCategoryArgs.Name = "<b>Traversal Shaders - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(ulong),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				bool flag = true;
				if (this.m_allBoundUsed == ResourcesViewMgr.AllBoundUsed.Used)
				{
					foreach (ModelObjectData modelObjectData in this.traversalShaders)
					{
						ulong num2 = Uint64Converter.Convert(modelObjectData.GetValue("pipelineID"));
						if (this.m_boundInfo != null && this.m_boundInfo.IsDrawcallParent && this.m_boundInfo.ParentPipelines.Contains(num2))
						{
							flag = false;
							break;
						}
						if (this.m_boundInfo != null && num2 == this.m_boundInfo.BoundPipeline)
						{
							flag = false;
							break;
						}
					}
				}
				int num3 = 0;
				foreach (ModelObjectData modelObjectData2 in this.traversalShaders)
				{
					ulong num4 = Uint64Converter.Convert(modelObjectData2.GetValue("pipelineID"));
					bool flag2 = false;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag2 = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						if (this.m_boundInfo != null)
						{
							if (this.m_boundInfo.IsDrawcallParent)
							{
								flag2 = this.m_boundInfo.ParentPipelines.Contains(num4);
							}
							else
							{
								flag2 = num4 == this.m_boundInfo.BoundPipeline;
							}
							if (!flag2 && flag)
							{
								HashSet<ulong> pipelineLibraryUsed = QGLPlugin.VkSnapshotModel.GetPipelineLibraryUsed(this.m_viewMgr.m_currentCaptureId, this.m_boundInfo.BoundPipeline, this.pipelineLibraryMdlObj);
								flag2 |= pipelineLibraryUsed.Contains(num4);
							}
						}
						break;
					}
					if (flag2)
					{
						num3++;
					}
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
					if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
						{
							dictionary[(long)num4] = flag2;
						}
					}
					else
					{
						object[] array = new object[] { flag2, num4 };
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "Pipeline ID [" + num4.ToString() + "]";
						resourceItem.Id = (long)num4;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = "Pipeline ID: " + num4.ToString();
						addCategoryArgs.ResourceItems.Add(resourceItem);
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(10, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 10;
				updateCategoryNumVisible.Total = this.traversalShaders.Count;
				updateCategoryNumVisible.Visible = num3;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x0600020D RID: 525 RVA: 0x0001B2C8 File Offset: 0x000194C8
			private string GetShaderString(ProfilerShaderStage shaderType)
			{
				string text = "";
				switch (shaderType)
				{
				case ProfilerShaderStage.Vertex:
					text += "Vertex";
					break;
				case ProfilerShaderStage.TessCtrl:
					text += "TessCtrl";
					break;
				case ProfilerShaderStage.TessEval:
					text += "TessEval";
					break;
				case ProfilerShaderStage.Geometry:
					text += "Geometry";
					break;
				case ProfilerShaderStage.Fragment:
					text += "Fragment";
					break;
				case ProfilerShaderStage.Compute:
					text += "Compute";
					break;
				case ProfilerShaderStage.Task:
					text += "Task";
					break;
				case ProfilerShaderStage.Mesh:
					text += "Mesh";
					break;
				}
				return text;
			}

			// Token: 0x0600020E RID: 526 RVA: 0x0001B378 File Offset: 0x00019578
			private void InvalidatePipelines()
			{
				ResourcesViewMgr.ResourceCategory[] array = new ResourcesViewMgr.ResourceCategory[]
				{
					ResourcesViewMgr.ResourceCategory.GraphicsPipelines,
					ResourcesViewMgr.ResourceCategory.ComputePipelines,
					ResourcesViewMgr.ResourceCategory.RayTracingPipelines
				};
				foreach (ResourcesViewMgr.ResourceCategory resourceCategory in array)
				{
					string text;
					ModelObjectDataList modelObjectDataList;
					if (resourceCategory != ResourcesViewMgr.ResourceCategory.GraphicsPipelines)
					{
						if (resourceCategory != ResourcesViewMgr.ResourceCategory.ComputePipelines)
						{
							text = "Raytracing";
							modelObjectDataList = this.raytracingPipelines;
						}
						else
						{
							text = "Compute";
							modelObjectDataList = this.computePipelines;
						}
					}
					else
					{
						text = "Graphics";
						modelObjectDataList = this.graphicsPipelines;
					}
					AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
					addCategoryArgs.ID = (int)resourceCategory;
					addCategoryArgs.Name = "<b>" + text + " Pipelines - [<span foreground='orange'>{0}</span>]</b>";
					addCategoryArgs.Style = ResourcesCategoryStyle.List;
					addCategoryArgs.CustomFilterColumns = new Type[]
					{
						typeof(bool),
						typeof(uint),
						typeof(string),
						typeof(string),
						typeof(string)
					};
					int num;
					if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
					{
						addCategoryArgs.CostBar = num;
					}
					else
					{
						addCategoryArgs.CostBar = 0;
					}
					Dictionary<long, object> dictionary = new Dictionary<long, object>();
					VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
					List<ulong> list;
					if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
					{
						list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
					}
					int num2 = 0;
					foreach (ModelObjectData modelObjectData in modelObjectDataList)
					{
						bool flag = false;
						ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
						if (this.m_newCapture)
						{
							list.Add(num3);
						}
						ResourcesViewMgr.AllBoundUsed allBoundUsed = this.m_allBoundUsed;
						if (allBoundUsed != ResourcesViewMgr.AllBoundUsed.All)
						{
							if (allBoundUsed - ResourcesViewMgr.AllBoundUsed.Bound <= 1)
							{
								if (this.m_boundInfo != null)
								{
									if (this.m_boundInfo.IsDrawcallParent)
									{
										flag = this.m_boundInfo.ParentPipelines.Contains(num3);
									}
									else
									{
										flag = num3 == this.m_boundInfo.BoundPipeline;
									}
								}
							}
						}
						else
						{
							flag = true;
						}
						string text2;
						this.labels.TryGetValue(num3, out text2);
						if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text2)
						{
							flag = false;
						}
						if (flag)
						{
							num2++;
							if (!string.IsNullOrEmpty(text2))
							{
								this.m_debugMarkersVisible.Add(text2);
							}
						}
						ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
						if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
						{
							if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
							{
								dictionary[(long)num3] = flag;
							}
						}
						else
						{
							object[] array3 = new object[] { flag, num3, text2 };
							ResourceItem resourceItem = new ResourceItem();
							text2 = VkDebugMarkers.ColorMarkerTextRV(text2);
							resourceItem.Name = "ID [" + num3.ToString() + "] " + text2;
							resourceItem.Id = (long)num3;
							resourceItem.CustomFilterObjects = array3;
							resourceItem.Tooltip = text + "Pipeline ID: " + num3.ToString();
							addCategoryArgs.ResourceItems.Add(resourceItem);
						}
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
					if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
					{
						SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
					}
					if (dictionary.Count > 0)
					{
						this.UpdateResourcesActiveToggled((int)resourceCategory, dictionary);
					}
					ItemSelectedEventArgs itemSelectedEventArgs = new ItemSelectedEventArgs();
					itemSelectedEventArgs.CategoryID = (int)resourceCategory;
					if (this.m_boundInfo != null)
					{
						itemSelectedEventArgs.ResourceIDs = new long[] { (long)this.m_boundInfo.BoundPipeline };
					}
					EventHandler<ItemSelectedEventArgs> selectItem = SdpApp.EventsManager.ResourceViewEvents.SelectItem;
					if (selectItem != null)
					{
						selectItem(this, itemSelectedEventArgs);
					}
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = (int)resourceCategory;
					updateCategoryNumVisible.Total = modelObjectDataList.Count;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x0600020F RID: 527 RVA: 0x0001B7B4 File Offset: 0x000199B4
			private void InvalidateImageViews()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 6;
				addCategoryArgs.Name = "<b>Image Views - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.imageViews)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedImageViews.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
					if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
						{
							dictionary[(long)num3] = flag;
						}
					}
					else
					{
						ResourceItem resourceItem = new ResourceItem();
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						resourceItem.Name = "ID [" + num3.ToString() + "] " + text;
						resourceItem.Id = (long)num3;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = "Resource ID: " + num3.ToString() + " " + text;
						addCategoryArgs.ResourceItems.Add(resourceItem);
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(6, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 6;
				updateCategoryNumVisible.Total = this.imageViews.Count;
				updateCategoryNumVisible.Visible = num2;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x06000210 RID: 528 RVA: 0x0001BB10 File Offset: 0x00019D10
			private void InvalidateAccelerationStructure()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 9;
				addCategoryArgs.Name = "<b>Acceleration Structures - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.TreeCollapsed;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				HashSet<ulong> hashSet = new HashSet<ulong>();
				HashSet<ulong> hashSet2 = new HashSet<ulong>();
				int num2 = 0;
				int num3 = 0;
				foreach (ModelObjectData modelObjectData in this.asInfo)
				{
					uint num4 = UintConverter.Convert(modelObjectData.GetValue("type"));
					ulong num5 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					long num6 = (long)(num5 + 18446744069414584320UL);
					if (this.m_newCapture)
					{
						list.Add(num5);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedAccelerationStructs.Contains(num5);
						break;
					}
					string text;
					this.labels.TryGetValue(num5, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (num4 == 0U)
					{
						if (flag)
						{
							num2++;
							if (!string.IsNullOrEmpty(text))
							{
								this.m_debugMarkersVisible.Add(text);
							}
						}
						num3++;
						ResourceItem resourceItem = new ResourceItem();
						object[] array = new object[] { flag, num5, text };
						ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
						if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
						{
							if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
							{
								dictionary[num6] = flag;
							}
						}
						else
						{
							text = VkDebugMarkers.ColorMarkerTextRV(text);
							resourceItem.Name = "TLAS ID [" + num5.ToString() + "] " + text;
							resourceItem.Id = num6;
							resourceItem.CustomFilterObjects = array;
							resourceItem.Tooltip = "TLAS ID: " + num5.ToString() + " " + text;
							addCategoryArgs.ResourceItems.Add(resourceItem);
						}
						ulong[] array2;
						if (capture.ASHierarchies.TryGetValue(num5, out array2))
						{
							uint num7 = 0U;
							while ((ulong)num7 < (ulong)((long)Enumerable.Count<ulong>(array2)))
							{
								ulong num8 = array2[(int)num7];
								long num9 = (long)(num8 + 18446744069414584320UL);
								long num10 = (long)(num5 + ((ulong)num7 << 32));
								if (flag)
								{
									num2++;
								}
								num3++;
								hashSet.Remove(num8);
								hashSet2.Add(num8);
								ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType2 = this.m_invalidateType;
								if (invalidateType2 != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
								{
									if (invalidateType2 == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType2 == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
									{
										object obj;
										if (dictionary.TryGetValue(num9, out obj))
										{
											dictionary[num9] = (bool)obj || flag;
										}
										else
										{
											dictionary[num9] = flag;
										}
										dictionary[num10] = flag;
									}
								}
								else
								{
									string text2;
									this.labels.TryGetValue(num5, out text2);
									object[] array3 = new object[] { flag, num5, text2 };
									text2 = VkDebugMarkers.ColorMarkerTextRV(text);
									ResourceItem resourceItem2 = new ResourceItem();
									resourceItem2.Id = num10;
									resourceItem2.Name = "   Instance Descriptor[" + num7.ToString() + "]";
									resourceItem2.Tooltip = "Instance Descriptor: " + num7.ToString();
									resourceItem2.CustomFilterObjects = array;
									resourceItem.Children.Add(resourceItem2);
									ResourceItem resourceItem3 = new ResourceItem();
									resourceItem3.Id = num9;
									resourceItem3.Name = "      BLAS ID [" + num8.ToString() + "] " + text2;
									resourceItem3.Tooltip = "BLAS ID: " + num8.ToString() + " " + text2;
									resourceItem3.CustomFilterObjects = array3;
									resourceItem.Children.Add(resourceItem3);
								}
								num7 += 1U;
							}
						}
					}
					else if (!hashSet2.Contains(num5))
					{
						hashSet.Add(num5);
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				foreach (ulong num11 in hashSet)
				{
					long num12 = (long)(num11 + 18446744069414584320UL);
					bool flag2 = true;
					ResourcesViewMgr.AllBoundUsed allBoundUsed = this.m_allBoundUsed;
					if (allBoundUsed != ResourcesViewMgr.AllBoundUsed.All)
					{
						if (allBoundUsed - ResourcesViewMgr.AllBoundUsed.Bound <= 1)
						{
							flag2 = false;
						}
					}
					else
					{
						flag2 = true;
					}
					string text3;
					this.labels.TryGetValue(num11, out text3);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text3)
					{
						flag2 = false;
					}
					if (flag2)
					{
						num2++;
						if (!string.IsNullOrEmpty(text3))
						{
							this.m_debugMarkersVisible.Add(text3);
						}
					}
					num3++;
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType3 = this.m_invalidateType;
					if (invalidateType3 != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType3 == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType3 == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
						{
							dictionary[num12] = flag2;
						}
					}
					else
					{
						object[] array4 = new object[] { flag2, num11, text3 };
						text3 = VkDebugMarkers.ColorMarkerTextRV(text3);
						ResourceItem resourceItem4 = new ResourceItem();
						resourceItem4.Id = num12;
						resourceItem4.Name = "BLAS ID [" + num11.ToString() + "] " + text3;
						resourceItem4.Tooltip = "BLAS ID: " + num11.ToString() + " " + text3;
						resourceItem4.CustomFilterObjects = array4;
						addCategoryArgs.ResourceItems.Add(resourceItem4);
					}
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(9, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 9;
				updateCategoryNumVisible.Total = num3;
				updateCategoryNumVisible.Visible = num2;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x06000211 RID: 529 RVA: 0x0001C230 File Offset: 0x0001A430
			private void InvalidateDescriptorSets()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 7;
				addCategoryArgs.Name = "<b>Descriptor Sets - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.descriptorSets)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					ResourcesViewMgr.AllBoundUsed allBoundUsed = this.m_allBoundUsed;
					if (allBoundUsed != ResourcesViewMgr.AllBoundUsed.All)
					{
						if (allBoundUsed - ResourcesViewMgr.AllBoundUsed.Bound <= 1)
						{
							flag = this.m_boundInfo != null && this.m_boundInfo.ContainsDescriptorSet(num3);
						}
					}
					else
					{
						flag = true;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					ResourcesViewMgr.InvalidateClass.InvalidateType invalidateType = this.m_invalidateType;
					if (invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange)
					{
						if (invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive || invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker)
						{
							dictionary[(long)(num3 + 18446744069414584320UL)] = flag;
						}
					}
					else
					{
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "ID [" + num3.ToString() + "] " + text;
						resourceItem.Id = (long)(num3 + 18446744069414584320UL);
						resourceItem.CustomFilterObjects = array;
						int num4 = 0;
						HashSet<uint> hashSet = new HashSet<uint>();
						DescSetBindings descSetBindings;
						if (QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId).AllDescSetBindings.TryGetValue(num3, out descSetBindings))
						{
							foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair in descSetBindings.Bindings)
							{
								uint apiID = keyValuePair.Value.apiID;
								if (apiID == 4294967295U)
								{
									num4++;
								}
								else if (!hashSet.Contains(apiID))
								{
									hashSet.Add(apiID);
									ResourceItem resourceItem2 = new ResourceItem();
									resourceItem2.Id = (long)(num3 + ((ulong)apiID << 32));
									resourceItem2.Name = "API Call [" + apiID.ToString() + "]";
									resourceItem2.Tooltip = "DescriptorSet ID: " + text + "\nDraw Call: " + apiID.ToString();
									resourceItem2.CustomFilterObjects = array;
									resourceItem.Children.Add(resourceItem2);
									addCategoryArgs.Style = ResourcesCategoryStyle.TreeCollapsed;
								}
							}
						}
						resourceItem.Tooltip = string.Concat(new string[]
						{
							"DescriptorSet ID: ",
							num3.ToString(),
							" ",
							text,
							"\nNumBindings: ",
							num4.ToString()
						});
						addCategoryArgs.ResourceItems.Add(resourceItem);
					}
					this.m_cancelToken.ThrowIfCancellationRequested();
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(7, dictionary);
				}
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = 7;
				updateCategoryNumVisible.Total = this.descriptorSets.Count;
				updateCategoryNumVisible.Visible = num2;
				SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
			}

			// Token: 0x06000212 RID: 530 RVA: 0x0001C6E4 File Offset: 0x0001A8E4
			private void InvalidateDescriptorSetLayouts()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 8;
				addCategoryArgs.Name = "<b>Descriptor Set Layouts - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs.Source = 353;
				prepopulateCategoryArgs.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs.CategoryId = 8;
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.descriptorSetLayouts)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedDescriptorLayouts.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					switch (this.m_invalidateType)
					{
					case ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange:
					{
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "ID [" + num3.ToString() + "] " + text;
						resourceItem.Id = (long)num3;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = "Resource ID: " + num3.ToString() + " " + text;
						addCategoryArgs.ResourceItems.Add(resourceItem);
						break;
					}
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker:
						dictionary[(long)num3] = flag;
						break;
					case ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate:
						if (flag)
						{
							prepopulateCategoryArgs.ResourceIds.Add((long)num3);
						}
						break;
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate && prepopulateCategoryArgs.ResourceIds.Count > 0)
				{
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs);
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(8, dictionary);
				}
				if (this.m_invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
				{
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = 8;
					updateCategoryNumVisible.Total = this.descriptorSetLayouts.Count;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x06000213 RID: 531 RVA: 0x0001CAB8 File Offset: 0x0001ACB8
			private void InvalidateTensors()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 11;
				addCategoryArgs.Name = "<b>Tensors - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs.Source = 353;
				prepopulateCategoryArgs.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs.CategoryId = 11;
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.tensors)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedTensors.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					switch (this.m_invalidateType)
					{
					case ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange:
					{
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "ID [" + num3.ToString() + "] " + text;
						resourceItem.Id = (long)num3;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = "Resource ID: " + num3.ToString() + " " + text;
						addCategoryArgs.ResourceItems.Add(resourceItem);
						break;
					}
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker:
						dictionary[(long)num3] = flag;
						break;
					case ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate:
						if (flag)
						{
							prepopulateCategoryArgs.ResourceIds.Add((long)num3);
						}
						break;
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate && prepopulateCategoryArgs.ResourceIds.Count > 0)
				{
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs);
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(11, dictionary);
				}
				if (this.m_invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
				{
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = 11;
					updateCategoryNumVisible.Total = this.tensors.Count;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x06000214 RID: 532 RVA: 0x0001CE90 File Offset: 0x0001B090
			private void InvalidateTensorViews()
			{
				AddCategoryArgs addCategoryArgs = new AddCategoryArgs();
				addCategoryArgs.ID = 12;
				addCategoryArgs.Name = "<b>Tensor Views - [<span foreground='orange'>{0}</span>]</b>";
				addCategoryArgs.Style = ResourcesCategoryStyle.List;
				addCategoryArgs.CustomFilterColumns = new Type[]
				{
					typeof(bool),
					typeof(uint),
					typeof(string),
					typeof(string),
					typeof(string)
				};
				int num;
				if (ResourcesViewMgr.m_costMetricCount.TryGetValue(this.m_viewMgr.m_currentCaptureId, out num))
				{
					addCategoryArgs.CostBar = num;
				}
				else
				{
					addCategoryArgs.CostBar = 0;
				}
				PrepopulateCategoryArgs prepopulateCategoryArgs = new PrepopulateCategoryArgs();
				prepopulateCategoryArgs.Source = 353;
				prepopulateCategoryArgs.DrawcallId = (int)this.m_selectedDrawAPI;
				prepopulateCategoryArgs.CategoryId = 12;
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(this.m_viewMgr.m_currentCaptureId);
				List<ulong> list;
				if (!capture.ResourceIDs.TryGetValue(addCategoryArgs.ID, out list))
				{
					list = (capture.ResourceIDs[addCategoryArgs.ID] = new List<ulong>());
				}
				Dictionary<long, object> dictionary = new Dictionary<long, object>();
				int num2 = 0;
				foreach (ModelObjectData modelObjectData in this.tensorViews)
				{
					ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
					if (this.m_newCapture)
					{
						list.Add(num3);
					}
					bool flag = true;
					switch (this.m_allBoundUsed)
					{
					case ResourcesViewMgr.AllBoundUsed.All:
						flag = true;
						break;
					case ResourcesViewMgr.AllBoundUsed.Bound:
						flag = false;
						break;
					case ResourcesViewMgr.AllBoundUsed.Used:
						flag = this.m_usedTensorViews.Contains(num3);
						break;
					}
					string text;
					this.labels.TryGetValue(num3, out text);
					if (this.m_viewMgr.m_markerSelection != "All Objects" && this.m_viewMgr.m_markerSelection != text)
					{
						flag = false;
					}
					if (flag)
					{
						num2++;
						if (!string.IsNullOrEmpty(text))
						{
							this.m_debugMarkersVisible.Add(text);
						}
					}
					switch (this.m_invalidateType)
					{
					case ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange:
					{
						object[] array = new object[] { flag, num3, text };
						text = VkDebugMarkers.ColorMarkerTextRV(text);
						ResourceItem resourceItem = new ResourceItem();
						resourceItem.Name = "ID [" + num3.ToString() + "] " + text;
						resourceItem.Id = (long)num3;
						resourceItem.CustomFilterObjects = array;
						resourceItem.Tooltip = "Resource ID: " + num3.ToString() + " " + text;
						addCategoryArgs.ResourceItems.Add(resourceItem);
						break;
					}
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateActive:
					case ResourcesViewMgr.InvalidateClass.InvalidateType.UpdateDebugMarker:
						dictionary[(long)num3] = flag;
						break;
					case ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate:
						if (flag)
						{
							prepopulateCategoryArgs.ResourceIds.Add((long)num3);
						}
						break;
					}
				}
				this.m_cancelToken.ThrowIfCancellationRequested();
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate && prepopulateCategoryArgs.ResourceIds.Count > 0)
				{
					SdpApp.EventsManager.Raise<PrepopulateCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.PrepopulateCategory, this, prepopulateCategoryArgs);
				}
				if (this.m_invalidateType == ResourcesViewMgr.InvalidateClass.InvalidateType.SourceChange && addCategoryArgs.ResourceItems.Count > 0)
				{
					SdpApp.EventsManager.Raise<AddCategoryArgs>(SdpApp.EventsManager.ResourceViewEvents.AddCategory, this, addCategoryArgs);
				}
				if (dictionary.Count > 0)
				{
					this.UpdateResourcesActiveToggled(12, dictionary);
				}
				if (this.m_invalidateType != ResourcesViewMgr.InvalidateClass.InvalidateType.Prepopulate)
				{
					UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
					updateCategoryNumVisible.CategoryID = 12;
					updateCategoryNumVisible.Total = this.tensorViews.Count;
					updateCategoryNumVisible.Visible = num2;
					SdpApp.EventsManager.Raise<UpdateCategoryNumVisible>(SdpApp.EventsManager.ResourceViewEvents.UpdateCategoryNumVisible, this, updateCategoryNumVisible);
				}
			}

			// Token: 0x040004F8 RID: 1272
			private bool m_newCapture;

			// Token: 0x040004F9 RID: 1273
			private bool m_hasTileMemory;

			// Token: 0x040004FA RID: 1274
			private ResourcesViewMgr.InvalidateClass.InvalidateType m_invalidateType;

			// Token: 0x040004FB RID: 1275
			private ResourcesViewMgr m_viewMgr;

			// Token: 0x040004FC RID: 1276
			private HashSet<ulong> m_usedImageViews = new HashSet<ulong>();

			// Token: 0x040004FD RID: 1277
			private HashSet<ulong> m_usedImages = new HashSet<ulong>();

			// Token: 0x040004FE RID: 1278
			private HashSet<ulong> m_usedDescriptorLayouts = new HashSet<ulong>();

			// Token: 0x040004FF RID: 1279
			private HashSet<ulong> m_usedMemoryBuffers = new HashSet<ulong>();

			// Token: 0x04000500 RID: 1280
			private HashSet<ulong> m_usedAccelerationStructs = new HashSet<ulong>();

			// Token: 0x04000501 RID: 1281
			private HashSet<ulong> m_usedTensors = new HashSet<ulong>();

			// Token: 0x04000502 RID: 1282
			private HashSet<ulong> m_usedTensorViews = new HashSet<ulong>();

			// Token: 0x04000503 RID: 1283
			private HashSet<string> m_formatsVisible = new HashSet<string>();

			// Token: 0x04000504 RID: 1284
			private SortedSet<string> m_debugMarkersVisible = new SortedSet<string>();

			// Token: 0x04000505 RID: 1285
			private SetStatusEventArgs m_statusArgs;

			// Token: 0x04000506 RID: 1286
			private ByteBufferGateway m_byteBufferGateway;

			// Token: 0x04000507 RID: 1287
			private uint m_selectedDrawAPI = uint.MaxValue;

			// Token: 0x04000508 RID: 1288
			private VkBoundInfo m_boundInfo;

			// Token: 0x04000509 RID: 1289
			private ResourcesViewMgr.AllBoundUsed m_allBoundUsed = ResourcesViewMgr.AllBoundUsed.Used;

			// Token: 0x0400050A RID: 1290
			private static object invalidateLock = new object();

			// Token: 0x0400050B RID: 1291
			private CancellationToken m_cancelToken;

			// Token: 0x0400050C RID: 1292
			private static readonly ConcurrentDictionary<ResourcesViewMgr.InvalidateClass.InvalidateType, CancellationTokenSource> m_tasks = new ConcurrentDictionary<ResourcesViewMgr.InvalidateClass.InvalidateType, CancellationTokenSource>();

			// Token: 0x0400050D RID: 1293
			private DataModel dataModel;

			// Token: 0x0400050E RID: 1294
			private Model model;

			// Token: 0x0400050F RID: 1295
			private ModelObject shaderDataObj;

			// Token: 0x04000510 RID: 1296
			private ModelObjectDataList traceGraphicsPipelines;

			// Token: 0x04000511 RID: 1297
			private Dictionary<ulong, List<uint>> shaderStagesPerPipeline;

			// Token: 0x04000512 RID: 1298
			private ModelObject imageViewsMO;

			// Token: 0x04000513 RID: 1299
			private ModelObject descriptorSetLinksMO;

			// Token: 0x04000514 RID: 1300
			private Dictionary<ulong, ulong> descSetLinks;

			// Token: 0x04000515 RID: 1301
			private ModelObject textureMdlObj;

			// Token: 0x04000516 RID: 1302
			private ModelObjectDataList textures;

			// Token: 0x04000517 RID: 1303
			private ModelObject objLabelMdlObj;

			// Token: 0x04000518 RID: 1304
			private ModelObjectDataList objLabels;

			// Token: 0x04000519 RID: 1305
			private Dictionary<ulong, string> labels;

			// Token: 0x0400051A RID: 1306
			private ModelObject shaderModuleMdlObj;

			// Token: 0x0400051B RID: 1307
			private ModelObject shaderStageMdlObj;

			// Token: 0x0400051C RID: 1308
			private ModelObjectDataList shaderModules;

			// Token: 0x0400051D RID: 1309
			private Dictionary<ulong, List<ResourcesViewMgr.StagePipelineIDTuple>> shaderStages;

			// Token: 0x0400051E RID: 1310
			private ModelObject graphicsPipelineMdlObj;

			// Token: 0x0400051F RID: 1311
			private ModelObjectDataList graphicsPipelines;

			// Token: 0x04000520 RID: 1312
			private ModelObject raytracingPipelineMdlObj;

			// Token: 0x04000521 RID: 1313
			private ModelObjectDataList raytracingPipelines;

			// Token: 0x04000522 RID: 1314
			private ModelObject computePipelineMdlObj;

			// Token: 0x04000523 RID: 1315
			private ModelObjectDataList computePipelines;

			// Token: 0x04000524 RID: 1316
			private ModelObject pipelineLibraryMdlObj;

			// Token: 0x04000525 RID: 1317
			private ModelObjectDataList imageViews;

			// Token: 0x04000526 RID: 1318
			private Dictionary<ulong, ulong> images;

			// Token: 0x04000527 RID: 1319
			private ModelObject descSetsMO;

			// Token: 0x04000528 RID: 1320
			private ModelObjectDataList descriptorSets;

			// Token: 0x04000529 RID: 1321
			private ModelObject descSetLayoutsMO;

			// Token: 0x0400052A RID: 1322
			private ModelObjectDataList descriptorSetLayouts;

			// Token: 0x0400052B RID: 1323
			private ModelObject memoryMdlObj;

			// Token: 0x0400052C RID: 1324
			private ModelObjectDataList memoryBuffers;

			// Token: 0x0400052D RID: 1325
			private ModelObject asInfoMO;

			// Token: 0x0400052E RID: 1326
			private ModelObjectDataList asInfo;

			// Token: 0x0400052F RID: 1327
			private ModelObject asInstanceDescriptorsMO;

			// Token: 0x04000530 RID: 1328
			private ModelObjectDataList asInstanceDescriptors;

			// Token: 0x04000531 RID: 1329
			private ModelObject snapshotShaderDataMO;

			// Token: 0x04000532 RID: 1330
			private ModelObjectDataList traversalShaders;

			// Token: 0x04000533 RID: 1331
			private ModelObject tensorsMO;

			// Token: 0x04000534 RID: 1332
			private ModelObjectDataList tensors;

			// Token: 0x04000535 RID: 1333
			private ModelObject tensorViewsMO;

			// Token: 0x04000536 RID: 1334
			private ModelObjectDataList tensorViews;

			// Token: 0x02000086 RID: 134
			public enum InvalidateType
			{
				// Token: 0x0400057F RID: 1407
				SourceChange,
				// Token: 0x04000580 RID: 1408
				UpdateActive,
				// Token: 0x04000581 RID: 1409
				Prepopulate,
				// Token: 0x04000582 RID: 1410
				UpdateBinaryData,
				// Token: 0x04000583 RID: 1411
				UpdateImageFormats,
				// Token: 0x04000584 RID: 1412
				UpdateDebugMarker,
				// Token: 0x04000585 RID: 1413
				UpdateTileMemory
			}

			// Token: 0x02000087 RID: 135
			private class ApiComparer : IComparer<uint>
			{
				// Token: 0x0600025A RID: 602 RVA: 0x0001DA3F File Offset: 0x0001BC3F
				public int Compare(uint first, uint second)
				{
					if (first == second)
					{
						return 0;
					}
					if (first > second)
					{
						return 1;
					}
					return -1;
				}
			}
		}

		// Token: 0x02000076 RID: 118
		private class DisplayImages
		{
			// Token: 0x06000216 RID: 534 RVA: 0x0001D280 File Offset: 0x0001B480
			public void DisplayThread()
			{
				ImageViewDisplayEventArgs imageViewDisplayEventArgs = new ImageViewDisplayEventArgs();
				imageViewDisplayEventArgs.ImageObjects = VkHelper.GenerateImageObjects(this.Data, this.Format, this.Width, this.Height, this.LayerCount, this.LevelCount, this.Depth);
				imageViewDisplayEventArgs.ImageType = ImageViewType.Texture2D;
				if (this.Depth > 1U)
				{
					imageViewDisplayEventArgs.ImageType = ImageViewType.Texture3D;
				}
				else if (this.LayerCount > 1U)
				{
					imageViewDisplayEventArgs.ImageType = ImageViewType.Texture2DArray;
				}
				else if (this.LevelCount > 1U)
				{
					imageViewDisplayEventArgs.ImageType = ImageViewType.Texture2DWithMimpaps;
				}
				SdpApp.EventsManager.Raise<ImageViewDisplayEventArgs>(SdpApp.EventsManager.ImageViewEvents.Display, this, imageViewDisplayEventArgs);
			}

			// Token: 0x04000537 RID: 1335
			public byte[] Data;

			// Token: 0x04000538 RID: 1336
			public VkFormats Format;

			// Token: 0x04000539 RID: 1337
			public uint Width;

			// Token: 0x0400053A RID: 1338
			public uint Height;

			// Token: 0x0400053B RID: 1339
			public uint LayerCount;

			// Token: 0x0400053C RID: 1340
			public uint LevelCount;

			// Token: 0x0400053D RID: 1341
			public uint Depth;
		}
	}
}
