using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Gdk;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;
using TextureConverter;

namespace QGLPlugin
{
	// Token: 0x02000032 RID: 50
	public class QGLPlugin : IMetricPlugin
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000083A2 File Offset: 0x000065A2
		public static ObservableObject<uint?> ProviderID
		{
			get
			{
				return QGLPlugin.m_providerID;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000083A9 File Offset: 0x000065A9
		internal static ShaderViewMgr ShaderMgr
		{
			get
			{
				return QGLPlugin.m_shaderViewMgr;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000083B0 File Offset: 0x000065B0
		public static BinaryDataPair SnapshotMetricsBuffer
		{
			get
			{
				return QGLPlugin.m_snapshotMetricsBuffer;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000083B7 File Offset: 0x000065B7
		public static BinaryDataPair SnapshotApiBuffer
		{
			get
			{
				return QGLPlugin.m_snapshotApiBuffer;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000083BE File Offset: 0x000065BE
		public static BinaryDataPair SnapshotDsbBuffer
		{
			get
			{
				return QGLPlugin.m_snapshotDsbBuffer;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000083C5 File Offset: 0x000065C5
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000083CC File Offset: 0x000065CC
		internal static VkSnapshotModel VkSnapshotModel { get; set; }

		// Token: 0x060000AE RID: 174 RVA: 0x000083D4 File Offset: 0x000065D4
		public QGLPlugin()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.ProviderListChanged = (EventHandler)Delegate.Combine(connectionEvents.ProviderListChanged, new EventHandler(this.connectionEvents_ProviderListChanged));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.SnapshotRequest = (EventHandler)Delegate.Combine(connectionEvents2.SnapshotRequest, new EventHandler(this.connectionEvents_SnapshotRequest));
			this.m_dataExplorerViewMgr = new DataExplorerViewMgr();
			this.m_captureViewMgr = new CaptureViewMgr();
			this.m_resourceViewMgr = new ResourcesViewMgr();
			QGLPlugin.m_shaderViewMgr = new ShaderViewMgr();
			this.m_screenCaptureViewMgr = new ScreenCaptureViewMgr();
			QGLPlugin.VkSnapshotModel = new VkSnapshotModel();
			this.m_pixelHistoryViewMgr = new PixelHistoryViewMgr();
			this.m_vertexDataViewMgr = new VertexDataViewMgr();
			this.m_snapshotViewMgr = new SnapshotViewMgr();
			this.m_dataSourcesMgr = new DataSourcesViewMgr();
			this.m_byteBufferGateway = new ByteBufferGateway("VulkanSnapshot", "VulkanSnapshotByteBuffers");
			this.InvalidateProviderId();
			ConnectionEvents connectionEvents3 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents3.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents3.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			ConnectionEvents connectionEvents4 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents4.OpenSnapshotFromSession = (EventHandler<OpenSnapshotFromSessionArgs>)Delegate.Combine(connectionEvents4.OpenSnapshotFromSession, new EventHandler<OpenSnapshotFromSessionArgs>(this.connectionEvents_OpenSnapshotFromSession));
			ConnectionEvents connectionEvents5 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents5.ClientBufferTransfer = (EventHandler<BufferTransferEventArgs>)Delegate.Combine(connectionEvents5.ClientBufferTransfer, new EventHandler<BufferTransferEventArgs>(this.connectionEvents_ClientBufferTransfer));
			ConnectionEvents connectionEvents6 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents6.BufferTransferProgress = (EventHandler<BufferTransferProgressEventArgs>)Delegate.Combine(connectionEvents6.BufferTransferProgress, new EventHandler<BufferTransferProgressEventArgs>(this.connectionEvents_BufferTransferProgress));
			InspectorViewEvents inspectorViewEvents = SdpApp.EventsManager.InspectorViewEvents;
			inspectorViewEvents.ButtonClicked = (EventHandler<ButtonClickedArgs>)Delegate.Combine(inspectorViewEvents.ButtonClicked, new EventHandler<ButtonClickedArgs>(this.inspectorViewEvents_ButtonClicked));
			Viewer3DEvents viewer3DEvents = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents.LoadAccelerationStructure = (EventHandler<Viewer3DLoadASArgs>)Delegate.Combine(viewer3DEvents.LoadAccelerationStructure, new EventHandler<Viewer3DLoadASArgs>(this.viewer3DEvents_LoadAccelerationStructure));
			this.m_bufferRegisteredDelegate = new Void_UInt_UInt_UInt_UInt_Fn(this.OnBufferRegistered);
			SdpApp.ConnectionManager.BufferRegisteredCallback(this.m_bufferRegisteredDelegate);
			SnapshotModel snapshotModel = SdpApp.ModelManager.SnapshotModel;
			int numSnapshotHandlers = snapshotModel.NumSnapshotHandlers;
			snapshotModel.NumSnapshotHandlers = numSnapshotHandlers + 1;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008724 File Offset: 0x00006924
		public static void DisplayReplay(uint captureID, uint replayID)
		{
			ModelObjectDataList vulkanReplayAttachments = QGLModel.GetVulkanReplayAttachments(captureID, replayID);
			if (vulkanReplayAttachments != null && vulkanReplayAttachments.Count > 0)
			{
				ScreenCaptureViewInvalidateEventArgs screenCaptureViewInvalidateEventArgs = new ScreenCaptureViewInvalidateEventArgs();
				foreach (ModelObjectData modelObjectData in vulkanReplayAttachments)
				{
					byte[] array = null;
					BinaryDataPair valuePtrBinaryDataPair = modelObjectData.GetValuePtrBinaryDataPair("dataPair");
					if (valuePtrBinaryDataPair != null && valuePtrBinaryDataPair.size > 0U)
					{
						array = new byte[valuePtrBinaryDataPair.size];
						Marshal.Copy(valuePtrBinaryDataPair.data, array, 0, (int)valuePtrBinaryDataPair.size);
					}
					if (array != null)
					{
						VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData.GetValue("format"));
						int num = (int)vkFormats;
						string textureFormatString = VkHelper.GetTextureFormatString(num.ToString());
						uint num2 = UintConverter.Convert(modelObjectData.GetValue("width"));
						uint num3 = UintConverter.Convert(modelObjectData.GetValue("height"));
						int num4 = IntConverter.Convert(modelObjectData.GetValue("attachmentID"));
						try
						{
							VkHelper.VFormatInfo formatInfo = VkHelper.GetFormatInfo(vkFormats);
							byte[] array2 = TextureConverterHelper.ConvertImageToRGBA(array, formatInfo.format, num2, num3, true, 0U);
							ImageViewObject imageViewObject = new ImageViewObject((int)num2, (int)num3, true, 0, 0, 0U, array2);
							imageViewObject.RawImage = new RawImageWrapper(textureFormatString, formatInfo.BPP, VkHelper.GetImagePixelType(vkFormats), formatInfo.NumberChannels, array);
							screenCaptureViewInvalidateEventArgs.Attachments[num4] = imageViewObject;
						}
						catch
						{
							string text = string.Format("Unable to convert attachment [{0}] with format: {1}", num4, textureFormatString);
							QGLPlugin.Logger.LogError(text);
						}
					}
				}
				screenCaptureViewInvalidateEventArgs.CaptureID = (int)captureID;
				screenCaptureViewInvalidateEventArgs.DrawcallID = 123;
				SdpApp.EventsManager.Raise<ScreenCaptureViewInvalidateEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.Invalidate, null, screenCaptureViewInvalidateEventArgs);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00008908 File Offset: 0x00006B08
		public bool HandlesMetric(MetricDescription metricDesc)
		{
			if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				if (string.Compare(metricDesc.Name, "Vulkan Snapshot") == 0 && string.Compare(metricDesc.CategoryName, "Vulkan") == 0)
				{
					return true;
				}
				if (string.Compare(metricDesc.Name, "Vulkan API Trace") == 0 && string.Compare(metricDesc.CategoryName, "Vulkan") == 0)
				{
					return true;
				}
				if (string.Compare(metricDesc.Name, "Vulkan GPU Scope Trace") == 0 && string.Compare(metricDesc.CategoryName, "Vulkan") == 0)
				{
					return true;
				}
				Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricDesc.Id);
				uint providerID = metricByID.GetProperties().providerID;
				uint? num = QGLPlugin.ProviderID;
				if ((providerID == num.GetValueOrDefault()) & (num != null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000089D8 File Offset: 0x00006BD8
		public string MetricDisplayName(Metric m)
		{
			MetricProperties properties = m.GetProperties();
			MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(properties.categoryID);
			if (m.IsTraceMetric() && metricCategory.GetProperties().name != "Vulkan")
			{
				return properties.name + " - Vulkan";
			}
			return properties.name;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00008A33 File Offset: 0x00006C33
		public MetricTrackType GetMetricTrackType(MetricDescription metricDesc)
		{
			return MetricTrackType.Custom;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008A36 File Offset: 0x00006C36
		public void StartCapture(MetricDescription metricDesc)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00008A36 File Offset: 0x00006C36
		public void Shutdown()
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008A38 File Offset: 0x00006C38
		public static void ClearApiBuffer()
		{
			QGLPlugin.m_snapshotApiBuffer = null;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00008A40 File Offset: 0x00006C40
		public static void ClearDsbBuffer()
		{
			QGLPlugin.m_snapshotDsbBuffer = null;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008A48 File Offset: 0x00006C48
		private QGLPlugin.CategoryIDCombo EndProgresssBar(uint bufferCategory, uint bufferID)
		{
			QGLPlugin.CategoryIDCombo categoryIDCombo;
			categoryIDCombo.BufferCategory = bufferCategory;
			categoryIDCombo.BufferID = bufferID;
			ProgressObject progressObject;
			if (this.m_captureProgressObject.TryGetValue(categoryIDCombo, out progressObject))
			{
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(progressObject));
				this.m_captureProgressObject.Remove(categoryIDCombo);
			}
			return categoryIDCombo;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008AA4 File Offset: 0x00006CA4
		private void InvalidateProviderId()
		{
			foreach (KeyValuePair<uint, DataProvider> keyValuePair in SdpApp.ModelManager.ConnectionModel.Providers)
			{
				if (keyValuePair.Value.GetProviderDesc().Name.StartsWith("VulkanDataPlugin"))
				{
					QGLPlugin.m_providerID.Value = new uint?(keyValuePair.Value.GetID());
					break;
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008B34 File Offset: 0x00006D34
		private void connectionEvents_SnapshotRequest(object sender, EventArgs e)
		{
			uint currentSnapshotProviderID = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CurrentSnapshotProviderID;
			uint? num = QGLPlugin.ProviderID;
			if ((currentSnapshotProviderID == num.GetValueOrDefault()) & (num != null))
			{
				SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
				setStatusEventArgs.Status = StatusType.Warning;
				setStatusEventArgs.StatusText = "Retrieving Snapshot";
				setStatusEventArgs.Duration = 0;
				SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SetStatus, null, setStatusEventArgs);
				if (this.m_captureProgress != null)
				{
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_captureProgress));
					this.m_captureProgress = null;
				}
				this.m_captureProgress = new ProgressObject();
				this.m_captureProgress.ShowProgressBar = false;
				this.m_captureProgress.Description = "Preparing snapshot. Device may freeze temporarily.";
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(this.m_captureProgress));
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008C30 File Offset: 0x00006E30
		private void connectionEvents_ProviderListChanged(object sender, EventArgs e)
		{
			this.InvalidateProviderId();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008C38 File Offset: 0x00006E38
		private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
		{
			int captureID = (int)args.CaptureID;
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_QGL_TRACE_DATA)
			{
				ModelObjectDataList apis = QGLModel.GetAPIs(captureID);
				if (this.m_captureProgress != null)
				{
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_captureProgress));
					this.m_captureProgress = null;
				}
				if (apis != null && apis.Count > 0)
				{
					SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.DataExplorer, (int)args.CaptureID, 2401, "Vulkan APIs", null));
					QGLModel.InitializePerfHints(captureID);
					this.m_captureViewMgr.PopulateCapture(captureID, apis);
				}
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_DATA)
			{
				SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
				setStatusEventArgs.Status = StatusType.Neutral;
				setStatusEventArgs.StatusText = string.Format("Replay received", Array.Empty<object>());
				setStatusEventArgs.Duration = 1000;
				SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SetStatus, null, setStatusEventArgs);
				QGLPlugin.WaitingForReplay = false;
				QGLPlugin.DisplayReplay(args.CaptureID, args.BufferID);
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA)
			{
				this.DisplayShaderProfiles(args.CaptureID, args.BufferID);
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA)
			{
				QGLPlugin.ShaderMgr.InvalidateSnapshotPipelineInfo(args.CaptureID);
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA)
			{
				SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.Resources, captureID, 2401, "Vulkan", null));
				QGLPlugin.ShaderMgr.InvalidateTracePipelineInfo(args.CaptureID);
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_QGL_DATA && this.m_captureProgress != null)
			{
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_captureProgress));
				this.m_captureProgress = null;
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF)
			{
				SetStatusEventArgs setStatusEventArgs2 = new SetStatusEventArgs();
				setStatusEventArgs2.Status = StatusType.Success;
				setStatusEventArgs2.StatusText = "GLTF file downloaded successfully.";
				setStatusEventArgs2.Duration = 3000;
				SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.InspectorViewEvents.SetStatus, null, setStatusEventArgs2);
				string text = SessionManager.Get().GetSessionPath() + "gltf\\";
				try
				{
					Process.Start(text);
				}
				catch
				{
					QGLPlugin.Logger.LogWarning("Unable to launch File Explorer for path: " + text);
				}
			}
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
			{
				SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
				if (processorPlugin != null)
				{
					QGLPlugin.m_snapshotMetricsBuffer = processorPlugin.GetLocalBuffer(args.BufferCategory, args.BufferID, args.CaptureID);
					if (QGLPlugin.HasSnapshotBuffers())
					{
						SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.DataExplorer, captureID, 353, "Vulkan", null));
					}
				}
				this.EndProgresssBar(args.BufferCategory, 1U);
			}
			if (args.BufferCategory != SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA)
			{
				this.EndProgresssBar(args.BufferCategory, args.BufferID);
				if (this.m_captureProgress != null)
				{
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_captureProgress));
					this.m_captureProgress = null;
				}
				return;
			}
			SDPProcessorPlugin processorPlugin2 = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
			if (processorPlugin2 == null)
			{
				return;
			}
			if (args.BufferID == 1U)
			{
				string text2 = SessionManager.Get().GetSessionPath() + "sdpframe_" + args.CaptureID.ToString().PadLeft(3, '0') + ".gfxrz";
				SdpApp.ModelManager.SnapshotModel.DataFilenames[args.CaptureID] = text2;
				if (!SdpApp.ModelManager.SnapshotModel.StrippedDataFilenames.ContainsKey(args.CaptureID))
				{
					QGLPlugin.m_snapshotApiBuffer = processorPlugin2.GetLocalBuffer(args.BufferCategory, args.BufferID, args.CaptureID);
					QGLPlugin.m_snapshotDsbBuffer = processorPlugin2.GetLocalBuffer(args.BufferCategory, 3U, args.CaptureID);
					if (QGLPlugin.HasSnapshotBuffers())
					{
						SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.DataExplorer, captureID, 353, "Vulkan", null));
					}
					this.InvalidateInitialData(captureID);
				}
				if (QGLPlugin.VkSnapshotModel.GetCapture(captureID).HasResources)
				{
					this.m_resourceViewMgr.UpdateBinaryData();
				}
				this.EndProgresssBar(SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA, 1U);
				return;
			}
			if (args.BufferID == 2U)
			{
				string text3 = SessionManager.Get().GetSessionPath() + "sdpframestripped_" + args.CaptureID.ToString().PadLeft(3, '0') + ".gfxrz";
				SdpApp.ModelManager.SnapshotModel.StrippedDataFilenames[args.CaptureID] = text3;
				if (!SdpApp.ModelManager.SnapshotModel.DataFilenames.ContainsKey(args.CaptureID))
				{
					QGLPlugin.m_snapshotApiBuffer = processorPlugin2.GetLocalBuffer(args.BufferCategory, args.BufferID, args.CaptureID);
					QGLPlugin.m_snapshotDsbBuffer = processorPlugin2.GetLocalBuffer(args.BufferCategory, 3U, args.CaptureID);
					if (QGLPlugin.HasSnapshotBuffers())
					{
						SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.DataExplorer, captureID, 353, "Vulkan", null));
					}
					this.InvalidateInitialData(captureID);
				}
				this.EndProgresssBar(SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA, 1U);
				return;
			}
			QGLPlugin.Logger.LogError("Unexpected Buffer ID received: " + args.BufferID.ToString());
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00009168 File Offset: 0x00007368
		public static void ClearMetricsBuffer()
		{
			QGLPlugin.m_snapshotMetricsBuffer = null;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00009170 File Offset: 0x00007370
		public static bool HasSnapshotBuffers()
		{
			return QGLPlugin.m_snapshotApiBuffer != null && QGLPlugin.m_snapshotMetricsBuffer != null;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009184 File Offset: 0x00007384
		private void InvalidateInitialData(int captureId)
		{
			QGLPlugin.VkSnapshotModel.PopulateDescSets((uint)captureId);
			QGLPlugin.ClearDsbBuffer();
			ScreenCaptureViewToolbarConfigEventArgs screenCaptureViewToolbarConfigEventArgs = new ScreenCaptureViewToolbarConfigEventArgs();
			screenCaptureViewToolbarConfigEventArgs.CaptureID = captureId;
			screenCaptureViewToolbarConfigEventArgs.ShowAttachmentsComboBox = true;
			screenCaptureViewToolbarConfigEventArgs.ShowContextComboBox = false;
			screenCaptureViewToolbarConfigEventArgs.ShowLegacyAttachmentsComboBox = false;
			screenCaptureViewToolbarConfigEventArgs.ShowBinningToggle = false;
			SdpApp.EventsManager.Raise<ScreenCaptureViewToolbarConfigEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.ToolbarConfig, this, screenCaptureViewToolbarConfigEventArgs);
			this.DisplayInitialScreenshot(captureId);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000091EC File Offset: 0x000073EC
		private void connectionEvents_BufferTransferProgress(object sender, BufferTransferProgressEventArgs args)
		{
			uint providerID = args.ProviderID;
			uint? num = QGLPlugin.ProviderID;
			if ((providerID == num.GetValueOrDefault()) & (num != null))
			{
				QGLPlugin.CategoryIDCombo categoryIDCombo;
				categoryIDCombo.BufferCategory = args.BufferCategory;
				categoryIDCombo.BufferID = args.BufferID;
				ProgressObject progressObject;
				if (this.m_captureProgressObject.TryGetValue(categoryIDCombo, out progressObject))
				{
					if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA || args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA || args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
					{
						progressObject.CurrentValue = args.BytesReceived / args.TotalBytes * 0.67;
					}
					else
					{
						progressObject.CurrentValue = args.BytesReceived / args.TotalBytes;
					}
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(progressObject));
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000092D0 File Offset: 0x000074D0
		private void viewer3DEvents_LoadAccelerationStructure(object sender, Viewer3DLoadASArgs args)
		{
			Viewer3DDisplayASArgs viewer3DDisplayASArgs = new Viewer3DDisplayASArgs();
			int currentCaptureID = this.m_resourceViewMgr.GetCurrentCaptureID();
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("VulkanSnapshot");
			ModelObject modelObject = model.GetModelObject("VulkanSnapshotASInfo");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"captureID",
				currentCaptureID.ToString(),
				"resourceID",
				args.AccelerationStructureID.ToString()
			});
			ModelObjectData modelObjectData = data[0];
			uint num = UintConverter.Convert(modelObjectData.GetValue("type"));
			IByteBuffer byteBuffer = this.m_byteBufferGateway.GetByteBuffer(currentCaptureID, args.AccelerationStructureID);
			byte[] array = new byte[byteBuffer.BDP.size];
			Marshal.Copy(byteBuffer.BDP.data, array, 0, (int)byteBuffer.BDP.size);
			viewer3DDisplayASArgs.Tlas = Tuple.Create<ulong, byte[]>(args.AccelerationStructureID, array);
			viewer3DDisplayASArgs.Blases = new Dictionary<ulong, byte[]>();
			if (num == 0U)
			{
				ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotASInstanceDescriptor");
				ModelObjectDataList data2 = modelObject2.GetData(new StringList
				{
					"captureID",
					currentCaptureID.ToString(),
					"tlasID",
					args.AccelerationStructureID.ToString()
				});
				foreach (ModelObjectData modelObjectData2 in data2)
				{
					uint num2 = UintConverter.Convert(modelObjectData2.GetValue("asInstanceDescriptorIndex"));
					ulong num3 = Uint64Converter.Convert(modelObjectData2.GetValue("blasID"));
					ulong num4 = Uint64Converter.Convert(modelObjectData2.GetValue("blasDeviceAddress"));
					if (num3 == 0UL && num4 != 0UL)
					{
						ModelObjectDataList data3 = modelObject.GetData(new StringList
						{
							"captureID",
							currentCaptureID.ToString(),
							"blasDeviceAddress",
							num4.ToString()
						});
						if (data3.Count > 0)
						{
							num3 = Uint64Converter.Convert(data3[0].GetValue("resourceID"));
						}
					}
					if (num3 != 0UL && !viewer3DDisplayASArgs.Blases.ContainsKey(num3))
					{
						byteBuffer = this.m_byteBufferGateway.GetByteBuffer(currentCaptureID, num3);
						byte[] array2 = new byte[byteBuffer.BDP.size];
						Marshal.Copy(byteBuffer.BDP.data, array2, 0, (int)byteBuffer.BDP.size);
						viewer3DDisplayASArgs.Blases[num3] = array2;
					}
				}
			}
			SdpApp.EventsManager.Raise<Viewer3DDisplayASArgs>(SdpApp.EventsManager.Viewer3DEvents.DisplayAccelerationStructure, this, viewer3DDisplayASArgs);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000095A0 File Offset: 0x000077A0
		private void inspectorViewEvents_ButtonClicked(object sender, ButtonClickedArgs args)
		{
			PropertyGridDescriptionObject content = args.Content;
			PropertyDescriptor propertyDescriptor = content.GetProperties().Find("Resource ID", true);
			int currentCaptureID = this.m_resourceViewMgr.GetCurrentCaptureID();
			ulong num = 0UL;
			if (propertyDescriptor != null)
			{
				num = (ulong)propertyDescriptor.GetValue(null);
			}
			if (args.Description == "Export as GLTF")
			{
				IntPtr intPtr = Marshal.AllocHGlobal(8);
				Marshal.WriteInt64(intPtr, 0, (long)num);
				SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
				setStatusEventArgs.Status = StatusType.Warning;
				setStatusEventArgs.StatusText = "Exporting GLTF file";
				setStatusEventArgs.Duration = 0;
				SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.InspectorViewEvents.SetStatus, null, setStatusEventArgs);
				SdpApp.EventsManager.Raise<BufferTransferEventArgs>(SdpApp.EventsManager.ConnectionEvents.ClientBufferTransfer, this, new BufferTransferEventArgs
				{
					CaptureID = (uint)currentCaptureID,
					BufferID = 0U,
					BufferCategory = SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF,
					ProviderID = 0U,
					BufferData = intPtr,
					BufferDataLength = 8U
				});
				Marshal.FreeHGlobal(intPtr);
				return;
			}
			if (args.Description == "Display Acceleration Structure")
			{
				Viewer3DLoadASArgs viewer3DLoadASArgs = new Viewer3DLoadASArgs();
				viewer3DLoadASArgs.AccelerationStructureID = num;
				SdpApp.EventsManager.Raise<Viewer3DLoadASArgs>(SdpApp.EventsManager.Viewer3DEvents.RequestLoadAccelerationStructure, this, viewer3DLoadASArgs);
				return;
			}
			TensorHelper.ParsedTensor parsedTensor;
			if (args.Description == "Display Tensor" && TensorHelper.TryLoadAndParseTensor((uint)currentCaptureID, num, out parsedTensor))
			{
				TensorViewDisplayEventArgs tensorViewDisplayEventArgs = new TensorViewDisplayEventArgs
				{
					CaptureID = (uint)currentCaptureID,
					TensorID = num,
					FormatName = parsedTensor.FormatName,
					Dims = parsedTensor.Dims,
					ElementSize = parsedTensor.ElementSize,
					NumChannels = parsedTensor.NumChannels,
					Matrices = parsedTensor.Matrices,
					Tiling = parsedTensor.Tiling.ToString()
				};
				SdpApp.EventsManager.Raise<TensorViewDisplayEventArgs>(SdpApp.EventsManager.TensorViewEvents.DisplayTensor, this, tensorViewDisplayEventArgs);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000978C File Offset: 0x0000798C
		private void DisplayInitialScreenshot(int captureID)
		{
			ModelObjectDataList vulkanSnapshotScreenshots = QGLModel.GetVulkanSnapshotScreenshots(captureID);
			if (vulkanSnapshotScreenshots != null && vulkanSnapshotScreenshots.Count == 1)
			{
				ModelObjectData modelObjectData = vulkanSnapshotScreenshots[0];
				if (modelObjectData != null)
				{
					BinaryDataPair valuePtrBinaryDataPair = modelObjectData.GetValuePtrBinaryDataPair("data");
					if (valuePtrBinaryDataPair != null && valuePtrBinaryDataPair.size > 0U)
					{
						byte[] array = new byte[valuePtrBinaryDataPair.size];
						Marshal.Copy(valuePtrBinaryDataPair.data, array, 0, (int)valuePtrBinaryDataPair.size);
						Pixbuf pixbuf = new Pixbuf(array);
						int num = pixbuf.Rowstride * pixbuf.Height;
						byte[] array2 = new byte[num];
						Marshal.Copy(pixbuf.Pixels, array2, 0, num);
						ScreenCaptureViewDisplayEventArgs screenCaptureViewDisplayEventArgs = new ScreenCaptureViewDisplayEventArgs();
						screenCaptureViewDisplayEventArgs.CaptureID = (uint)captureID;
						screenCaptureViewDisplayEventArgs.CaptureImage = new ImageViewObject(pixbuf.Width, pixbuf.Height, pixbuf.HasAlpha, 353, captureID, 0U, array2);
						SdpApp.EventsManager.Raise<ScreenCaptureViewDisplayEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.DisplayScreenCapture, this, screenCaptureViewDisplayEventArgs);
					}
				}
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00009888 File Offset: 0x00007A88
		private void DisplayShaderProfiles(uint captureID, uint replayID)
		{
			uint num;
			if (QGLModel.TryGetVulkanApiID(captureID, replayID, out num))
			{
				VkBoundInfo vkBoundInfo;
				QGLPlugin.VkSnapshotModel.GetBoundInfo(captureID, num, out vkBoundInfo);
				if (vkBoundInfo != null)
				{
					QGLPlugin.ShaderMgr.DisplayPipelineStages((int)captureID, vkBoundInfo.BoundPipeline, num);
					this.m_resourceViewMgr.UpdateShaderModules((int)captureID);
				}
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000098D0 File Offset: 0x00007AD0
		private void OnBufferRegistered(uint providerId, uint captureId, uint bufferCategory, uint bufferId)
		{
			uint? num = QGLPlugin.ProviderID;
			if ((providerId == num.GetValueOrDefault()) & (num != null))
			{
				if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA)
				{
					DateTime now = DateTime.Now;
					QGLPlugin.Logger.LogInformation("Snapshot buffer registered: " + (now - this.m_snapshotViewMgr.SnapshotStart).TotalMilliseconds.ToString() + " ms");
				}
				if (this.m_captureProgress != null)
				{
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_captureProgress));
					this.m_captureProgress = null;
				}
				QGLPlugin.CategoryIDCombo categoryIDCombo = this.EndProgresssBar(bufferCategory, bufferId);
				ProgressObject progressObject = new ProgressObject();
				string text = "";
				string text2 = "";
				if (!this.m_bufferCategoryTitles.TryGetValue(bufferCategory, out text) || !this.m_bufferCategoryDescriptions.TryGetValue(bufferCategory, out text2))
				{
					if (this.m_captureProgressObject.Count == 0)
					{
						this.m_captureProgress = new ProgressObject();
						this.m_captureProgress.Title = "Vulkan";
						this.m_captureProgress.Description = "Preparing snapshot";
						this.m_captureProgress.CurrentValue = 0.25;
						SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(this.m_captureProgress));
					}
					return;
				}
				progressObject.Title = text;
				progressObject.Description = text2;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(progressObject));
				this.m_captureProgressObject.Add(categoryIDCombo, progressObject);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00009A6C File Offset: 0x00007C6C
		private void connectionEvents_OpenSnapshotFromSession(object sender, OpenSnapshotFromSessionArgs args)
		{
			if (args.API == RenderingAPI.Vulkan || args.API == RenderingAPI.None)
			{
				new Task(delegate
				{
					string text = Path.GetTempFileName();
					string text2 = ".gfxrz";
					using (ZipArchive zipArchive = ZipFile.Open(args.SessionPath, ZipArchiveMode.Read))
					{
						ZipArchiveEntry zipArchiveEntry = null;
						foreach (ZipArchiveEntry zipArchiveEntry2 in zipArchive.Entries)
						{
							if (zipArchiveEntry2.Name == "sdpframe_" + args.SelectedCaptureID.ToString().PadLeft(3, '0') + ".gfxrz" || zipArchiveEntry2.Name == "sdpframestripped_" + args.SelectedCaptureID.ToString().PadLeft(3, '0') + ".gfxrz")
							{
								zipArchiveEntry = zipArchiveEntry2;
								break;
							}
						}
						if (zipArchiveEntry == null)
						{
							SdpApp.EventsManager.Raise<OpenSnapshotFromSessionResultArgs>(SdpApp.EventsManager.ConnectionEvents.OpenSnapshotFromSessionResult, null, new OpenSnapshotFromSessionResultArgs
							{
								Handled = false
							});
							return;
						}
						if (zipArchiveEntry != null)
						{
							text += text2;
							zipArchiveEntry.ExtractToFile(text, true);
						}
					}
					Device connectedDevice = DeviceManager.Get().GetConnectedDevice();
					string text3 = "Unable to open capture";
					string text4 = "sdpframe_" + args.NewCaptureID.ToString().PadLeft(3, '0');
					if (connectedDevice != null)
					{
						using (Dictionary<uint, DataProvider>.Enumerator enumerator2 = SdpApp.ModelManager.ConnectionModel.Providers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<uint, DataProvider> keyValuePair = enumerator2.Current;
								if (keyValuePair.Value.GetProviderDesc().Name == "VulkanDataPlugin")
								{
									string text5 = connectedDevice.GetProperty(DeviceSettings.ProfilerServiceDeployPath) + "/" + text4 + text2;
									if (!SdpApp.ConnectionManager.PushFile(this, text, text5, false))
									{
										text3 = "Unable to communicate with device";
										break;
									}
									Option option = SdpApp.ConnectionManager.GetOption(SDPCore.OPT_VULKAN_SNAPSHOT_FROM_SESSION, uint.MaxValue);
									if (option != null)
									{
										IntPtr intPtr = Marshal.AllocHGlobal(4);
										Marshal.WriteInt32(intPtr, (int)args.NewCaptureID);
										OptionStructData optionStructData = option.GetOptionStructData();
										optionStructData.SetValue("captureID", intPtr);
										optionStructData.SetValue("dataFile", text5);
										option.SetValue(optionStructData);
										Marshal.FreeHGlobal(intPtr);
										text3 = null;
										break;
									}
									break;
								}
							}
							goto IL_0342;
						}
					}
					if (text2 == ".gfxrz")
					{
						text3 = null;
						string text6 = SessionManager.Get().GetSessionPath() + text4 + ".gfxr";
						byte[] bytes = Encoding.UTF8.GetBytes(text + ";" + text6);
						IntPtr intPtr2 = Marshal.AllocHGlobal(bytes.Length);
						Marshal.Copy(bytes, 0, intPtr2, bytes.Length);
						SdpApp.EventsManager.Raise<BufferTransferEventArgs>(SdpApp.EventsManager.ConnectionEvents.ClientBufferTransfer, this, new BufferTransferEventArgs
						{
							CaptureID = args.NewCaptureID,
							BufferID = 1U,
							BufferCategory = SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA,
							ProviderID = ((QGLPlugin.ProviderID.Value != null) ? QGLPlugin.ProviderID.Value.Value : 0U),
							BufferData = intPtr2,
							BufferDataLength = (uint)bytes.Length
						});
					}
					IL_0342:
					if (QGLPlugin.ProviderID.Value != null || connectedDevice == null)
					{
						SdpApp.EventsManager.Raise<OpenSnapshotFromSessionResultArgs>(SdpApp.EventsManager.ConnectionEvents.OpenSnapshotFromSessionResult, null, new OpenSnapshotFromSessionResultArgs
						{
							Error = text3,
							Handled = true,
							SnapshotProviderID = ((QGLPlugin.ProviderID.Value != null) ? QGLPlugin.ProviderID.Value.Value : 0U),
							SourceDisplayName = "Vulkan"
						});
					}
				}).Start();
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009AC0 File Offset: 0x00007CC0
		private void connectionEvents_ClientBufferTransfer(object sender, BufferTransferEventArgs args)
		{
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA || args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF || args.BufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA)
			{
				SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
				if (processorPlugin != null)
				{
					processorPlugin.ProcessData(args.BufferCategory, args.BufferID, args.CaptureID, args.BufferData, args.BufferDataLength, null);
				}
			}
		}

		// Token: 0x0400038A RID: 906
		public static bool WaitingForReplay = false;

		// Token: 0x0400038B RID: 907
		private static ILogger Logger = new global::Sdp.Logging.Logger("QGL Client Plugin");

		// Token: 0x0400038C RID: 908
		private Dictionary<QGLPlugin.CategoryIDCombo, ProgressObject> m_captureProgressObject = new Dictionary<QGLPlugin.CategoryIDCombo, ProgressObject>();

		// Token: 0x0400038D RID: 909
		private Dictionary<uint, string> m_bufferCategoryDescriptions = new Dictionary<uint, string>
		{
			{
				SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA,
				"Receiving Vulkan Snapshot Binary Data"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA,
				"Receiving Vulkan Snapshot Drawcall Data"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA,
				"Receiving Vulkan Metrics Data"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_TRACE_DATA,
				"Receiving trace data"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_DATA,
				"Receiving rendering stages data"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_DATA_BOTH,
				"Gathering Vulkan API Trace Data"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_CMDBUF_DATA,
				"Gathering Vulkan API Trace Data"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_REPLAY_DATA,
				"Receiving Vulkan Drawcall Replay Data"
			}
		};

		// Token: 0x0400038E RID: 910
		private Dictionary<uint, string> m_bufferCategoryTitles = new Dictionary<uint, string>
		{
			{
				SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA,
				"Vulkan Snapshot"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA,
				"Vulkan Snapshot"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA,
				"Vulkan Snapshot"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_TRACE_DATA,
				"Vulkan"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_DATA,
				"Receiving"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_DATA_BOTH,
				"Vulkan"
			},
			{
				SDPCore.BUFFER_TYPE_QGL_CMDBUF_DATA,
				"Vulkan"
			},
			{
				SDPCore.BUFFER_TYPE_VULKAN_REPLAY_DATA,
				"Vulkan Snapshot"
			}
		};

		// Token: 0x0400038F RID: 911
		private DataSourcesViewMgr m_dataSourcesMgr;

		// Token: 0x04000390 RID: 912
		private ProgressObject m_captureProgress;

		// Token: 0x04000391 RID: 913
		private ResourcesViewMgr m_resourceViewMgr;

		// Token: 0x04000392 RID: 914
		private DataExplorerViewMgr m_dataExplorerViewMgr;

		// Token: 0x04000393 RID: 915
		private CaptureViewMgr m_captureViewMgr;

		// Token: 0x04000394 RID: 916
		private static ShaderViewMgr m_shaderViewMgr;

		// Token: 0x04000395 RID: 917
		private ScreenCaptureViewMgr m_screenCaptureViewMgr;

		// Token: 0x04000396 RID: 918
		private PixelHistoryViewMgr m_pixelHistoryViewMgr;

		// Token: 0x04000397 RID: 919
		private VertexDataViewMgr m_vertexDataViewMgr;

		// Token: 0x04000398 RID: 920
		private SnapshotViewMgr m_snapshotViewMgr;

		// Token: 0x04000399 RID: 921
		private Void_UInt_UInt_UInt_UInt_Fn m_bufferRegisteredDelegate;

		// Token: 0x0400039B RID: 923
		private static ObservableObject<uint?> m_providerID = new ObservableObject<uint?>(null);

		// Token: 0x0400039C RID: 924
		private static BinaryDataPair m_snapshotApiBuffer;

		// Token: 0x0400039D RID: 925
		private static BinaryDataPair m_snapshotDsbBuffer;

		// Token: 0x0400039E RID: 926
		private static BinaryDataPair m_snapshotMetricsBuffer;

		// Token: 0x0400039F RID: 927
		private ByteBufferGateway m_byteBufferGateway;

		// Token: 0x0200005D RID: 93
		private struct CategoryIDCombo
		{
			// Token: 0x04000482 RID: 1154
			public uint BufferCategory;

			// Token: 0x04000483 RID: 1155
			public uint BufferID;
		}

		// Token: 0x0200005E RID: 94
		public struct VulkanSnapshotApi
		{
			// Token: 0x04000484 RID: 1156
			public uint captureID;

			// Token: 0x04000485 RID: 1157
			public uint apiID;

			// Token: 0x04000486 RID: 1158
			public ulong threadID;

			// Token: 0x04000487 RID: 1159
			public ulong timestamp;

			// Token: 0x04000488 RID: 1160
			[MarshalAs(UnmanagedType.LPStr)]
			public string name;

			// Token: 0x04000489 RID: 1161
			[MarshalAs(UnmanagedType.LPStr)]
			public string parameters;

			// Token: 0x0400048A RID: 1162
			[MarshalAs(UnmanagedType.LPStr)]
			public string encodedParams;
		}

		// Token: 0x0200005F RID: 95
		public struct VulkanSnapshotMetric
		{
			// Token: 0x0400048B RID: 1163
			public uint captureID;

			// Token: 0x0400048C RID: 1164
			public uint metricID;

			// Token: 0x0400048D RID: 1165
			public ulong drawID;

			// Token: 0x0400048E RID: 1166
			public uint cmdBuffSubmitCount;

			// Token: 0x0400048F RID: 1167
			public ulong replayHandleID;

			// Token: 0x04000490 RID: 1168
			public double value;
		}

		// Token: 0x02000060 RID: 96
		public struct VulkanHandleMapping
		{
			// Token: 0x04000491 RID: 1169
			public ulong captureHandleID;

			// Token: 0x04000492 RID: 1170
			public ulong replayHandleID;

			// Token: 0x04000493 RID: 1171
			public ulong submitIndex;
		}
	}
}
