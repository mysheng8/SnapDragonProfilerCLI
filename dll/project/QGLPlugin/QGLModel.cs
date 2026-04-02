using System;
using System.Collections.Generic;
using Sdp;
using Sdp.Helpers;

namespace QGLPlugin
{
	// Token: 0x0200002C RID: 44
	public class QGLModel
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00006F7C File Offset: 0x0000517C
		public static void CreateVulkanReplayMappingModel()
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.AddModel("VulkanReplay");
				if (model != null)
				{
					ModelObject modelObject = model.AddObject("VulkanReplayMapping");
					modelObject.AddAttribute("captureID", SDPDataType.SDP_UINT32, 4U, 0U);
					modelObject.AddAttribute("apiID", SDPDataType.SDP_INT32, 4U, 4U);
					modelObject.AddAttribute("replayID", SDPDataType.SDP_UINT32, 4U, 8U);
					modelObject.Save();
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006FE8 File Offset: 0x000051E8
		public static bool TryGetVulkanReplayID(uint captureID, int apiID, out uint replayID)
		{
			replayID = 0U;
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanReplay");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanReplayMapping");
					if (modelObject != null)
					{
						StringList stringList = new StringList(new string[]
						{
							"captureID",
							captureID.ToString(),
							"apiID",
							apiID.ToString()
						});
						ModelObjectDataList data = modelObject.GetData(stringList);
						if (data != null && data.Count == 1)
						{
							replayID = UintConverter.Convert(data[0].GetValue("replayID"));
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000708C File Offset: 0x0000528C
		public static bool TryGetVulkanApiID(uint captureID, uint replayID, out uint apiID)
		{
			apiID = 0U;
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanReplay");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanReplayMapping");
					if (modelObject != null)
					{
						StringList stringList = new StringList(new string[]
						{
							"captureID",
							captureID.ToString(),
							"replayID",
							replayID.ToString()
						});
						ModelObjectDataList data = modelObject.GetData(stringList);
						if (data != null && data.Count == 1)
						{
							apiID = UintConverter.Convert(data[0].GetValue("apiID"));
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00007130 File Offset: 0x00005330
		public static uint AddReplayMapping(uint captureID, int apiID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanReplay");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanReplayMapping");
					if (modelObject != null)
					{
						ModelObjectData modelObjectData = modelObject.NewData();
						modelObjectData.SetAttributeValue("captureID", captureID.ToString());
						modelObjectData.SetAttributeValue("apiID", apiID.ToString());
						ModelObjectData modelObjectData2 = modelObjectData;
						string text = "replayID";
						uint num = (QGLModel.m_replayID += 1U);
						modelObjectData2.SetAttributeValue(text, num.ToString());
						modelObjectData.Save();
					}
				}
			}
			return QGLModel.m_replayID;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000071C8 File Offset: 0x000053C8
		public static ModelObjectDataList GetVulkanReplayAttachments(uint captureID, uint replayID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanReplayAttachments");
					if (modelObject != null)
					{
						StringList stringList = new StringList(new string[]
						{
							"captureID",
							captureID.ToString(),
							"replayID",
							replayID.ToString()
						});
						return modelObject.GetData(stringList);
					}
				}
			}
			return null;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000723C File Offset: 0x0000543C
		public static ModelObjectDataList GetAPIs(int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLApiTracePackets");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "m_captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000728C File Offset: 0x0000548C
		public static ModelObjectDataList GetAPIsWithAttribute(string attributeName, object attribute)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLApiTracePackets");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, attributeName, attribute.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000072D8 File Offset: 0x000054D8
		public static ModelObjectDataList GetQueueSubmitTimings(int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLApiTraceQueueSubmitTiming");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "m_captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00007328 File Offset: 0x00005528
		public static ModelObjectDataList GetPerfHintData(uint hintID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLAdrenoPerfHintData");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "m_hintID", hintID.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00007378 File Offset: 0x00005578
		public static ModelObjectDataList GetPerfHintMessages(uint hintID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLAdrenoPerfHintMsgs");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "m_hintID", hintID.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000073C8 File Offset: 0x000055C8
		public static ModelObjectDataList GetPerfHintMessagesParams(uint hintID, uint msgIdx)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLAdrenoPerfHintMsgParams");
					if (modelObject != null)
					{
						return modelObject.GetData(new StringList
						{
							"m_hintID",
							hintID.ToString(),
							"m_MsgIdx",
							msgIdx.ToString()
						});
					}
				}
			}
			return null;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00007444 File Offset: 0x00005644
		public static void InitializePerfHints(int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "QGLAdrenoPerfTraceMsgs");
					ModelObjectDataList modelObjectData = dataModel.GetModelObjectData(modelObject, "m_captureID", captureId.ToString());
					foreach (ModelObjectData modelObjectData2 in modelObjectData)
					{
						uint num = UintConverter.Convert(modelObjectData2.GetValue("m_callID"));
						Tuple<int, uint> tuple = new Tuple<int, uint>(captureId, num);
						if (!QGLModel.m_adrenoHintMsgCache.ContainsKey(tuple))
						{
							QGLModel.m_adrenoHintMsgCache.Add(tuple, new ModelObjectDataList());
						}
						QGLModel.m_adrenoHintMsgCache[tuple].Add(modelObjectData2);
					}
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00007520 File Offset: 0x00005720
		public static ModelObjectDataList GetPerfHintTraceMessages(int captureId, uint callID)
		{
			Tuple<int, uint> tuple = new Tuple<int, uint>(captureId, callID);
			if (QGLModel.m_adrenoHintMsgCache.ContainsKey(tuple))
			{
				return QGLModel.m_adrenoHintMsgCache[tuple];
			}
			return null;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00007550 File Offset: 0x00005750
		public static string GetVulkanTraceStructParams(uint captureID, uint callID)
		{
			string text;
			if (QGLModel.ProcessingDebugMarkerData.TryGetValue(callID.ToString(), out text))
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000757C File Offset: 0x0000577C
		public static void GetVulkanDebugMarkerParams(int captureID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("QGLModel");
			ModelObject modelObject = dataModel.GetModelObject(model, "QGLApiTraceStructPackets");
			ModelObjectDataList data = modelObject.GetData(new StringList
			{
				"m_captureID",
				captureID.ToString(),
				"m_structName",
				"VkDebugMarkerMarkerInfoEXT"
			});
			foreach (ModelObjectData modelObjectData in data)
			{
				QGLModel.ProcessingDebugMarkerData[modelObjectData.GetValue("m_callID")] = modelObjectData.GetValue("m_structMembers");
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00007644 File Offset: 0x00005844
		public static ModelObjectDataList GetVulkanEndCaptureImage(int captureID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanSnapshot");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanEndCapture");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "captureID", captureID.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00007694 File Offset: 0x00005894
		public static ModelObjectDataList GetVulkanSnapshotScreenshots(int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanSnapshot");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotScreenshots");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000076E4 File Offset: 0x000058E4
		public static ModelObjectDataList GetVulkanSnapshotMetricsCaptured(uint captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotMetricValue");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007734 File Offset: 0x00005934
		public static ModelObjectDataList GetVulkanShaderStatProperty(uint captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("QGLModel");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanShaderStatProperty");
					if (modelObject != null)
					{
						ModelObjectDataList modelObjectDataList = dataModel.GetModelObjectData(modelObject, "captureID", captureId.ToString());
						if (modelObjectDataList.Count == 0)
						{
							modelObjectDataList = dataModel.GetModelObjectData(modelObject, "captureID", uint.MaxValue.ToString());
						}
						return modelObjectDataList;
					}
				}
			}
			return null;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000077A4 File Offset: 0x000059A4
		public static ModelObjectDataList GetVulkanShaderInfo(uint captureId, string modelName, string tableName)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel(modelName);
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, tableName);
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000077EC File Offset: 0x000059EC
		public static ModelObjectDataList GetVulkanSnapshotObjectLabels(int captureId)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			if (dataModel != null)
			{
				Model model = dataModel.GetModel("VulkanSnapshot");
				if (model != null)
				{
					ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotObjectLabels");
					if (modelObject != null)
					{
						return dataModel.GetModelObjectData(modelObject, "captureID", captureId.ToString());
					}
				}
			}
			return null;
		}

		// Token: 0x04000366 RID: 870
		private const string QGL_MODEL = "QGLModel";

		// Token: 0x04000367 RID: 871
		private static uint m_replayID = 0U;

		// Token: 0x04000368 RID: 872
		private static Dictionary<Tuple<int, uint>, ModelObjectDataList> m_adrenoHintMsgCache = new Dictionary<Tuple<int, uint>, ModelObjectDataList>();

		// Token: 0x04000369 RID: 873
		private const string DEBUG_MARKER_ADD_STRUCT_NAME = "VkDebugMarkerMarkerInfoEXT";

		// Token: 0x0400036A RID: 874
		public static readonly Dictionary<int, GanttTrackController> GanttTrackControllers = new Dictionary<int, GanttTrackController>();

		// Token: 0x0400036B RID: 875
		public static readonly Dictionary<string, string> ProcessingDebugMarkerData = new Dictionary<string, string>();
	}
}
