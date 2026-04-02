using System;
using System.Collections.Generic;
using Cairo;
using Gtk;
using Sdp;

namespace GlobalGPUTracePlugin
{
	// Token: 0x02000002 RID: 2
	public class GlobalGPUTraceCaptureData
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public GlobalGPUTraceCaptureData(uint captureID)
		{
			this.m_captureID = captureID;
			this.m_groupController.Clear();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
		public void AddData(uint metricID, long timestamp, double data)
		{
			if (!this.m_counterData.ContainsKey(metricID))
			{
				this.m_counterData[metricID] = new DataPointList();
			}
			this.m_counterData[metricID].Add(new DataPoint((double)timestamp, data));
			if (timestamp < this.m_dataBounds.min)
			{
				this.m_dataBounds.min = timestamp;
			}
			if (timestamp > this.m_dataBounds.max)
			{
				this.m_dataBounds.max = timestamp;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002128 File Offset: 0x00000328
		public void AddDataToTracks(string metricCategoryNameforGroup)
		{
			if (!SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey((int)this.m_captureID))
			{
				return;
			}
			Application.Invoke(delegate
			{
				GroupLayoutController groupLayoutController = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[(int)this.m_captureID];
				int num = 0;
				if (this.m_groupController.ContainsKey(num))
				{
					return;
				}
				AddGroupCommand addGroupCommand = new AddGroupCommand();
				addGroupCommand.Container = groupLayoutController;
				addGroupCommand.GroupName = metricCategoryNameforGroup + " Metrics";
				SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
				this.m_groupController.Add(num, addGroupCommand.Result);
				foreach (KeyValuePair<uint, DataPointList> keyValuePair in this.m_counterData)
				{
					uint key = keyValuePair.Key;
					DataPointList value = keyValuePair.Value;
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(key);
					if (metricByID != null)
					{
						if (!this.m_graphControllers.ContainsKey(key))
						{
							AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
							addTrackToGroupCommand.Container = this.m_groupController[num];
							addTrackToGroupCommand.MetricPlugin = null;
							addTrackToGroupCommand.TrackType = TrackType.Graph;
							SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
							this.m_graphControllers[key] = addTrackToGroupCommand.Result as GraphTrackController;
						}
						GraphTrackController graphTrackController = this.m_graphControllers[key];
						if (value != null)
						{
							string name = SdpApp.ConnectionManager.GetMetricCategoryByID(metricByID.GetProperties().categoryID).GetProperties().name;
							double[] metricCategoryColor = SdpApp.ModelManager.ConnectionModel.GetMetricCategoryColor(SdpApp.ConnectionManager.GetMetricCategoryByName(name).GetProperties().id);
							graphTrackController.View.ControlPanelHeaderBackColor = new Color(metricCategoryColor[0], metricCategoryColor[1], metricCategoryColor[2]);
							string name2 = metricByID.GetProperties().name;
							AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
							addMetricToTrackCommand.Container = graphTrackController;
							addMetricToTrackCommand.MetricId = 0U;
							addMetricToTrackCommand.MetricName = name2;
							addMetricToTrackCommand.PID = 0U;
							SdpApp.CommandManager.ExecuteCommand(addMetricToTrackCommand);
							graphTrackController.SetDrawMode(GraphTrackController.DrawMode.DRAW_STEPPED_LINE);
							graphTrackController.AddTransientMetricData(name2, this.m_counterData[key]);
							if (this.m_dataBounds.min < this.m_dataBounds.max)
							{
								graphTrackController.SetDataBounds(this.m_dataBounds.min, this.m_dataBounds.max);
							}
						}
					}
				}
			});
		}

		// Token: 0x04000001 RID: 1
		private GlobalGPUTraceCaptureData.DataBounds m_dataBounds = new GlobalGPUTraceCaptureData.DataBounds(long.MaxValue, long.MinValue);

		// Token: 0x04000002 RID: 2
		private uint m_captureID;

		// Token: 0x04000003 RID: 3
		private Dictionary<uint, DataPointList> m_counterData = new Dictionary<uint, DataPointList>();

		// Token: 0x04000004 RID: 4
		private Dictionary<uint, GraphTrackController> m_graphControllers = new Dictionary<uint, GraphTrackController>();

		// Token: 0x04000005 RID: 5
		private Dictionary<int, GroupController> m_groupController = new Dictionary<int, GroupController>();

		// Token: 0x02000004 RID: 4
		public struct DataBounds
		{
			// Token: 0x0600000D RID: 13 RVA: 0x000022DC File Offset: 0x000004DC
			public DataBounds(long min, long max)
			{
				this.min = min;
				this.max = max;
			}

			// Token: 0x04000007 RID: 7
			public long min;

			// Token: 0x04000008 RID: 8
			public long max;
		}
	}
}
