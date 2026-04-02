using System;
using Sdp;
using Sdp.Helpers;

namespace GlobalGPUTracePlugin
{
	// Token: 0x02000003 RID: 3
	public class GlobalGPUTracePlugin : IMetricPlugin
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002177 File Offset: 0x00000377
		public GlobalGPUTracePlugin()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.OnDataProcessed));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021AA File Offset: 0x000003AA
		public bool HandlesMetric(MetricDescription metricDesc)
		{
			return string.Compare(metricDesc.CategoryName, "GPU % Utilization") == 0;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021BF File Offset: 0x000003BF
		public MetricTrackType GetMetricTrackType(MetricDescription metricDesc)
		{
			return MetricTrackType.Custom;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021C2 File Offset: 0x000003C2
		public string MetricDisplayName(Metric m)
		{
			return m.GetProperties().name;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021CF File Offset: 0x000003CF
		public void StartCapture(MetricDescription metricDesc)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021CF File Offset: 0x000003CF
		public void Shutdown()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021D1 File Offset: 0x000003D1
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000021D9 File Offset: 0x000003D9
		public GroupLayoutController Container
		{
			get
			{
				return this.m_container;
			}
			set
			{
				if (value != this.m_container)
				{
					this.m_container = value;
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021EC File Offset: 0x000003EC
		private void OnDataProcessed(object sender, DataProcessedEventArgs args)
		{
			if (args.BufferCategory != SDPCore.BUFFER_TYPE_GGPM_TRACE_DATA)
			{
				return;
			}
			uint captureID = args.CaptureID;
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("GlobalGPUTraceModel");
			GlobalGPUTraceCaptureData globalGPUTraceCaptureData = new GlobalGPUTraceCaptureData(captureID);
			ModelObject modelObject = dataModel.GetModelObject(model, "tblGlobalGPUTrace");
			ModelObjectDataList modelObjectData = dataModel.GetModelObjectData(modelObject, SDPCore.DSP_MODEL_ATTRIB_CAPTURE_ID, captureID.ToString());
			foreach (ModelObjectData modelObjectData2 in modelObjectData)
			{
				uint num = UintConverter.Convert(modelObjectData2.GetValue("MetricID"));
				long num2 = Int64Converter.Convert(modelObjectData2.GetValue("Timestamp"));
				double num3 = DoubleConverter.Convert(modelObjectData2.GetValue("Value"));
				globalGPUTraceCaptureData.AddData(num, num2, num3);
			}
			globalGPUTraceCaptureData.AddDataToTracks("Global GPU");
		}

		// Token: 0x04000006 RID: 6
		private GroupLayoutController m_container;
	}
}
