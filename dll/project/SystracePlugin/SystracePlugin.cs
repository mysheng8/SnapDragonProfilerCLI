using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Cairo;
using Gtk;
using Sdp;
using Sdp.Charts.Gantt;
using Sdp.Helpers;

namespace TracePlugin
{
	// Token: 0x0200001A RID: 26
	public class SystracePlugin : IMetricPlugin
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000074F4 File Offset: 0x000056F4
		public SystracePlugin()
		{
			SystracePlugin.Model = new SystraceModel();
			SystraceModel model = SystracePlugin.Model;
			model.Changed = (EventHandler)Delegate.Combine(model.Changed, new EventHandler(this.systraceEvents_DataProcessed));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.StartCaptureRequest = (EventHandler<TakeCaptureArgs>)Delegate.Combine(connectionEvents.StartCaptureRequest, new EventHandler<TakeCaptureArgs>(this.connectionEvents_StartCaptureRequest));
			this.m_processor = new SystraceProcessor();
			this.m_processGroups = new Dictionary<ulong, GroupController>();
			this.m_taskGroupCounters = new Dictionary<ulong, Dictionary<string, GraphTrackController>>();
			this.m_taskNameGanttTrackControllers = new Dictionary<ulong, Dictionary<string, TrackControllerBase>>();
			this.m_statsViewMgr = new StatisticsViewMgr();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007599 File Offset: 0x00005799
		private void connectionEvents_StartCaptureRequest(object sender, EventArgs e)
		{
			this.m_processGroups.Clear();
			this.m_taskNameGanttTrackControllers.Clear();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000075B1 File Offset: 0x000057B1
		public bool HandlesMetric(MetricDescription metricDesc)
		{
			return string.Compare(metricDesc.CategoryName, SystracePlugin.SystraceMetricCategoryName) == 0;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000075C8 File Offset: 0x000057C8
		public string MetricDisplayName(Metric m)
		{
			return m.GetProperties().name;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000075D8 File Offset: 0x000057D8
		public void StartCapture(MetricDescription metricDesc)
		{
			if (SystracePlugin.CaptureProgress == null)
			{
				SystracePlugin.CaptureProgress = new ProgressObject();
				SystracePlugin.CaptureProgress.Title = "Trace";
				SystracePlugin.CaptureProgress.Description = "Gathering Trace Capture Data";
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007638 File Offset: 0x00005838
		public MetricTrackType GetMetricTrackType(MetricDescription metricDesc)
		{
			return MetricTrackType.Custom;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000763B File Offset: 0x0000583B
		public void Shutdown()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000763D File Offset: 0x0000583D
		private ulong GetTaskHash(int taskGroupID, string taskName)
		{
			return (ulong)((long)taskName.GetHashCode() * 4294967296L + (long)taskGroupID);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007654 File Offset: 0x00005854
		private void systraceEvents_DataProcessed(object sender, EventArgs e)
		{
			if (SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey(SystracePlugin.Model.CaptureID))
			{
				GroupLayoutController container = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[SystracePlugin.Model.CaptureID];
				this.m_taskGroupCounters.Clear();
				Application.Invoke(delegate
				{
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					Console.WriteLine("Timing UI Thread execution... ");
					if (SystracePlugin.Model.TaskGroupIDData != null)
					{
						foreach (KeyValuePair<int, Dictionary<string, TaskTrackData>> keyValuePair in SystracePlugin.Model.TaskGroupIDData)
						{
							int key = keyValuePair.Key;
							Dictionary<string, TaskTrackData> value = keyValuePair.Value;
							List<string> list = Enumerable.ToList<string>(value.Keys);
							list.Sort();
							foreach (string text in list)
							{
								this.TryAddGroup(container, key, text);
								TaskTrackData taskTrackData = value[text];
								ulong taskHash = this.GetTaskHash(key, text);
								if (!this.m_taskNameGanttTrackControllers.ContainsKey(taskHash))
								{
									this.AddGanttTracks(taskTrackData, key, text);
								}
								this.AddGraphTracks(taskTrackData, key, text);
							}
						}
					}
					TimeSpan elapsed = stopwatch.Elapsed;
					string text2 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[]
					{
						elapsed.Hours,
						elapsed.Minutes,
						elapsed.Seconds,
						elapsed.Milliseconds / 10
					});
					Console.WriteLine("RunTime " + text2);
				});
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000076D0 File Offset: 0x000058D0
		private void AddGraphTracks(TaskTrackData taskTrackData, int taskGroupID, string taskName)
		{
			if (taskTrackData.DataPointData != null && taskTrackData.DataPointData.Count != 0)
			{
				ulong taskHash = this.GetTaskHash(taskGroupID, taskName);
				List<string> list = Enumerable.ToList<string>(taskTrackData.DataPointData.Keys);
				list.Sort();
				foreach (string text in list)
				{
					if (!this.m_taskGroupCounters.ContainsKey(taskHash))
					{
						this.m_taskGroupCounters.Add(taskHash, new Dictionary<string, GraphTrackController>());
					}
					Dictionary<string, GraphTrackController> dictionary = this.m_taskGroupCounters[taskHash];
					if (!dictionary.ContainsKey(text))
					{
						AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
						addTrackToGroupCommand.Container = this.m_processGroups[taskHash];
						addTrackToGroupCommand.MetricPlugin = null;
						addTrackToGroupCommand.TrackType = TrackType.Graph;
						SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
						dictionary.Add(text, addTrackToGroupCommand.Result as GraphTrackController);
					}
					GraphTrackController graphTrackController = dictionary[text];
					if (graphTrackController != null)
					{
						double[] metricCategoryColor = SdpApp.ModelManager.ConnectionModel.GetMetricCategoryColor(SdpApp.ConnectionManager.GetMetricCategoryByName(SystracePlugin.SystraceMetricCategoryName).GetProperties().id);
						graphTrackController.View.ControlPanelHeaderBackColor = new Color(metricCategoryColor[0], metricCategoryColor[1], metricCategoryColor[2]);
						AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
						addMetricToTrackCommand.Container = graphTrackController;
						addMetricToTrackCommand.MetricId = 0U;
						addMetricToTrackCommand.MetricName = text;
						addMetricToTrackCommand.PID = 0U;
						SdpApp.CommandManager.ExecuteCommand(addMetricToTrackCommand);
						graphTrackController.SetDrawMode(GraphTrackController.DrawMode.DRAW_STEPPED_LINE);
						long num = Int64Converter.Convert(taskTrackData.DataPointData[text].DataPointList.MinTimestamp);
						long num2 = Int64Converter.Convert(taskTrackData.DataPointData[text].DataPointList.MaxTimestamp);
						graphTrackController.AddTransientMetricData(text, taskTrackData.DataPointData[text].DataPointList);
						graphTrackController.SetDataBounds(num, num2);
					}
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000078D8 File Offset: 0x00005AD8
		private void AddGanttTracks(TaskTrackData taskTrackData, int taskGroupID, string taskName)
		{
			if (taskTrackData.DepthData != null && taskTrackData.DepthData.Count != 0)
			{
				ulong taskHash = this.GetTaskHash(taskGroupID, taskName);
				this.TryAddTrackToGroup(taskGroupID, taskName);
				GanttTrackController ganttTrackController = this.m_taskNameGanttTrackControllers[taskHash][taskName] as GanttTrackController;
				if (ganttTrackController != null)
				{
					double[] metricCategoryColor = SdpApp.ModelManager.ConnectionModel.GetMetricCategoryColor(SdpApp.ConnectionManager.GetMetricCategoryByName(SystracePlugin.SystraceMetricCategoryName).GetProperties().id);
					ganttTrackController.View.ControlPanelHeaderBackColor = new Color(metricCategoryColor[0], metricCategoryColor[1], metricCategoryColor[2]);
					GanttTrackController ganttTrackController2 = ganttTrackController;
					ganttTrackController2.ElementSelected = (EventHandler<ElementSelectedEventArgs>)Delegate.Combine(ganttTrackController2.ElementSelected, new EventHandler<ElementSelectedEventArgs>(this.gantt_ElementSelected));
					ganttTrackController.SetColorsModel(SystracePlugin.Model.ColorsModel);
					ganttTrackController.NameStringsModel = taskTrackData.NameStringModel;
					ganttTrackController.TooltipStringsModel = taskTrackData.TooltipStringModel;
					ganttTrackController.StringIDsToRender = taskTrackData.NameHashCodesToRender;
					ganttTrackController.SettingsWindowName = taskTrackData.Name;
					long num = long.MaxValue;
					long num2 = long.MinValue;
					List<int> list = Enumerable.ToList<int>(taskTrackData.DepthData.Keys);
					list.Sort();
					bool flag = true;
					foreach (int num3 in list)
					{
						DepthData depthData = taskTrackData.DepthData[num3];
						Series series = depthData.Series;
						series.DefaultBackColor = new Color(1.0, 1.0, 1.0);
						if (flag)
						{
							series.Name = taskName;
						}
						else
						{
							series.Name = "";
						}
						flag = false;
						if (!string.IsNullOrEmpty(depthData.Name))
						{
							series.Name = depthData.Name;
						}
						ganttTrackController.Series.Add(series);
						if (depthData.MinTimestamp < num)
						{
							num = depthData.MinTimestamp;
						}
						if (depthData.MaxTimestamp > num2)
						{
							num2 = depthData.MaxTimestamp;
						}
					}
					ganttTrackController.SetDataBounds(num, num2);
					ganttTrackController.Invalidate();
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007B04 File Offset: 0x00005D04
		private void TryAddGroup(GroupLayoutController container, int taskGroupID, string taskGroupName)
		{
			ulong taskHash = this.GetTaskHash(taskGroupID, taskGroupName);
			if (!this.m_processGroups.ContainsKey(taskHash))
			{
				AddGroupCommand addGroupCommand = new AddGroupCommand();
				addGroupCommand.Container = container;
				if (taskGroupID == -1)
				{
					addGroupCommand.GroupName = "Trace Kernel - " + taskGroupName;
				}
				else
				{
					addGroupCommand.GroupName = "Trace Process " + taskGroupID.ToString() + " - " + taskGroupName;
				}
				SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
				this.m_processGroups.Add(taskHash, addGroupCommand.Result);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00007B88 File Offset: 0x00005D88
		private void TryAddTrackToGroup(int taskGroupID, string taskName)
		{
			ulong taskHash = this.GetTaskHash(taskGroupID, taskName);
			if (!this.m_taskNameGanttTrackControllers.ContainsKey(taskHash))
			{
				this.m_taskNameGanttTrackControllers.Add(taskHash, new Dictionary<string, TrackControllerBase>());
			}
			if (!this.m_taskNameGanttTrackControllers[taskHash].ContainsKey(taskName))
			{
				AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
				addTrackToGroupCommand.Container = this.m_processGroups[taskHash];
				addTrackToGroupCommand.MetricPlugin = null;
				addTrackToGroupCommand.TrackType = TrackType.Gantt;
				SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
				this.m_taskNameGanttTrackControllers[taskHash].Add(taskName, addTrackToGroupCommand.Result);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00007C1C File Offset: 0x00005E1C
		private void gantt_ElementSelected(object sender, ElementSelectedEventArgs e)
		{
			GanttTrackController ganttTrackController = sender as GanttTrackController;
			if (ganttTrackController != null && e.SelectedElementCount == 1)
			{
				long num = e.Selected.End - e.Selected.Start;
				string text = "No Name";
				ganttTrackController.NameStringsModel.TryGetValue(e.Selected.LabelId, out text);
				InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
				PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
				List<PropertyDescriptor> list = new List<PropertyDescriptor>();
				string text2 = string.Format("Properties for {0}", text);
				PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("Name", typeof(string), text, text2, "Name", true);
				list.Add(propertyDescriptor);
				PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<string>("Start Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.Start, "#.###", "#.####"), text2, "Start Time", true);
				list.Add(propertyDescriptor2);
				PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<string>("End Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.End, "#.###", "#.####"), text2, "End Time", true);
				list.Add(propertyDescriptor3);
				PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<string>("Duration", typeof(string), FormatHelper.FormatTimeLabel(num, "#.###", "#.####"), text2, "Duration", true);
				list.Add(propertyDescriptor4);
				propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
				inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
				inspectorViewDisplayEventArgs.Description = "Gantt Element";
				SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			}
		}

		// Token: 0x04000061 RID: 97
		public static SystraceModel Model;

		// Token: 0x04000062 RID: 98
		public static string SystraceMetricCategoryName = "Trace";

		// Token: 0x04000063 RID: 99
		private SystraceProcessor m_processor;

		// Token: 0x04000064 RID: 100
		private Dictionary<ulong, GroupController> m_processGroups;

		// Token: 0x04000065 RID: 101
		private Dictionary<ulong, Dictionary<string, TrackControllerBase>> m_taskNameGanttTrackControllers;

		// Token: 0x04000066 RID: 102
		private Dictionary<ulong, Dictionary<string, GraphTrackController>> m_taskGroupCounters;

		// Token: 0x04000067 RID: 103
		public static ProgressObject CaptureProgress;

		// Token: 0x04000068 RID: 104
		private StatisticsViewMgr m_statsViewMgr;
	}
}
