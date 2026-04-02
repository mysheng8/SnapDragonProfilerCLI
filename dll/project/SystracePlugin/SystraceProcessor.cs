using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using Cairo;
using Gtk;
using Sdp;
using Sdp.Charts;
using Sdp.Charts.Gantt;
using Sdp.Helpers;

namespace TracePlugin
{
	// Token: 0x0200001B RID: 27
	internal class SystraceProcessor
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public SystraceProcessor()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents2.OptionAdded, new EventHandler<OptionEventArgs>(this.connectionEvents_OptionAdded));
			this.random = new Random();
			this.m_storeToModelDictionary["tblSystraceGraphSeriesTable"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceGraphSeriesToModel);
			this.m_storeToModelDictionary["tblSystraceMarkersTable"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceMarkersToModel);
			this.m_storeToModelDictionary["tblSystraceGanttElementsTable"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceGanttElementsToModel);
			this.m_storeToModelDictionary["tblSystraceFunctions"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceFunctionsToModel);
			this.m_storeToModelDictionary["tblSystraceASyncFuncs"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceASyncFuncsToModel);
			this.m_storeToModelDictionary["tblSystraceCounters"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceCountersToModel);
			this.m_storeToModelDictionary["tblSystraceSchedSwitch"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceSchedSwitchToModel);
			this.m_storeToModelDictionary["tblSystraceClockSetRate"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceClockSetMarkersToModel);
			this.m_storeToModelDictionary["tblSystraceCpuFreq"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceCpuFreqChangesToModel);
			this.m_storeToModelDictionary["tblSystraceCpuIdle"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceCpuIdleToModel);
			this.m_storeToModelDictionary["tblSystraceSyncTimeline"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceSyncTimelineToModel);
			this.m_storeToModelDictionary["tblSystraceSyncWait"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceSyncWaitToModel);
			this.m_storeToModelDictionary["tblSystraceWorkExec"] = new SystraceProcessor.StoreObjectDataToModel(this.StoreSystraceWorkExecToModel);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008024 File Offset: 0x00006224
		private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
		{
			uint captureID = args.CaptureID;
			if (args.BufferCategory == SDPCore.BUFFER_TYPE_SYSTRACE_DATA)
			{
				if (SystracePlugin.CaptureProgress != null)
				{
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
				SystracePlugin.CaptureProgress = new ProgressObject();
				SystracePlugin.CaptureProgress.Title = "Trace";
				SystracePlugin.CaptureProgress.Description = "Loading Trace Data";
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
				Model model = dataModel.GetModel("SystraceModel");
				GroupController kgslGroup = null;
				ModelObject modelObject = model.GetModelObject("SystraceAdrenoCmdBatch");
				ModelObjectDataList data = modelObject.GetData("captureID", captureID.ToString());
				if (data != null && data.Count > 0 && SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey((int)captureID))
				{
					Dictionary<string, Series> taskGPUSubmittedSeries = new Dictionary<string, Series>();
					Dictionary<string, Series> taskCPUSubmittedSeries = new Dictionary<string, Series>();
					Dictionary<uint, string> namesModel2 = new Dictionary<uint, string>();
					Dictionary<uint, string> tooltipModel2 = new Dictionary<uint, string>();
					Dictionary<uint, Color> colorsModel2 = new Dictionary<uint, Color>();
					List<Connection> seriesConnections = new List<Connection>();
					long dataMin2 = long.MaxValue;
					long dataMax2 = long.MinValue;
					uint num = 1U;
					foreach (ModelObjectData modelObjectData in data)
					{
						long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampSubmitted"));
						long num3 = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
						long num4 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
						uint num5 = UintConverter.Convert(modelObjectData.GetValue("submittedThreadID"));
						string value = modelObjectData.GetValue("taskName");
						Series series = null;
						if (!taskGPUSubmittedSeries.TryGetValue(value, out series))
						{
							series = new Series
							{
								Name = value
							};
							taskGPUSubmittedSeries.Add(value, series);
						}
						Series series2 = null;
						if (!taskCPUSubmittedSeries.TryGetValue(value, out series2))
						{
							series2 = new Series
							{
								Name = value
							};
							taskCPUSubmittedSeries.Add(value, series2);
						}
						Element element = new Element();
						element.Start = num3;
						element.End = num4;
						string text = value;
						uint hashCode = (uint)value.GetHashCode();
						if (!namesModel2.ContainsKey(hashCode))
						{
							namesModel2.Add(hashCode, text);
						}
						if (!colorsModel2.ContainsKey(hashCode))
						{
							colorsModel2.Add(hashCode, this.PseudoRandomColor());
						}
						string text2 = string.Concat(new string[]
						{
							"KGSL CmdBatch\nTask Name: ",
							value,
							"\nThread: ",
							series.Name,
							"\nStart Time: ",
							FormatHelper.FormatTimeLabel(element.Start, "#.##", "#.###"),
							"\nEnd Time: ",
							FormatHelper.FormatTimeLabel(element.End, "#.##", "#.###"),
							"\nDuration: ",
							FormatHelper.FormatTimeLabel(element.End - element.Start, "#.##", "#.###")
						});
						uint hashCode2 = (uint)text2.GetHashCode();
						if (!tooltipModel2.ContainsKey(hashCode2))
						{
							tooltipModel2.Add(hashCode2, text2);
						}
						series2.Markers.Add(new Marker
						{
							Color = colorsModel2[hashCode],
							Style = MarkerStyle.FullCircle,
							Position = num2,
							TooltipId = hashCode2
						});
						element.TooltipId = hashCode2;
						element.LabelId = hashCode;
						element.ColorId = hashCode;
						element.BlockId = num++;
						series.Elements.Add(element);
						Connection connection = new Connection();
						connection.StartPosition = num2;
						connection.EndPosition = num3;
						connection.Start = series2;
						connection.End = series;
						seriesConnections.Add(connection);
						dataMin2 = Math.Min(dataMin2, element.Start);
						dataMax2 = Math.Max(dataMax2, element.End);
					}
					Application.Invoke(delegate
					{
						GroupLayoutController groupLayoutController = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[(int)captureID];
						AddGroupCommand addGroupCommand = new AddGroupCommand();
						addGroupCommand.Container = groupLayoutController;
						addGroupCommand.GroupName = "GPU Activity";
						SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
						kgslGroup = addGroupCommand.Result;
						if (kgslGroup != null)
						{
							AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
							addTrackToGroupCommand.TrackType = TrackType.Gantt;
							addTrackToGroupCommand.Container = kgslGroup;
							SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
							GanttTrackController ganttTrackController = addTrackToGroupCommand.Result as GanttTrackController;
							if (ganttTrackController != null)
							{
								Series series3 = new Series();
								series3.IsExpanded = true;
								series3.Name = "CPU";
								series3.Children.AddRange(taskCPUSubmittedSeries.Values);
								Series series4 = new Series();
								series4.IsExpanded = true;
								series4.Name = "GPU";
								series4.Children.AddRange(taskGPUSubmittedSeries.Values);
								ganttTrackController.View.ControlPanelHeaderBackColor = this.PseudoRandomColor();
								ganttTrackController.Series.Add(series4);
								ganttTrackController.Series.Add(series3);
								ganttTrackController.SetColorsModel(colorsModel2);
								ganttTrackController.NameStringsModel = namesModel2;
								ganttTrackController.TooltipStringsModel = tooltipModel2;
								foreach (Connection connection2 in seriesConnections)
								{
									ganttTrackController.AddConnection(connection2);
								}
								ganttTrackController.SetDataBounds(dataMin2, dataMax2);
								ganttTrackController.Invalidate();
								GanttTrackController ganttTrackController2 = ganttTrackController;
								ganttTrackController2.ElementSelected = (EventHandler<ElementSelectedEventArgs>)Delegate.Combine(ganttTrackController2.ElementSelected, new EventHandler<ElementSelectedEventArgs>(delegate(object o, ElementSelectedEventArgs e)
								{
									GanttTrackController ganttTrackController3 = o as GanttTrackController;
									if (ganttTrackController3 != null)
									{
										long num9 = e.Selected.End - e.Selected.Start;
										string text7 = "No Name";
										ganttTrackController3.NameStringsModel.TryGetValue(e.Selected.LabelId, out text7);
										InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
										PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
										List<PropertyDescriptor> list = new List<PropertyDescriptor>();
										string text8 = string.Format("KGSL CmdBatch for {0}", text7);
										PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("TaskName", typeof(string), text7, text8, "Task name", true);
										list.Add(propertyDescriptor);
										PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<string>("Start Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.Start, "#.##", "#.###"), text8, "Time when this event that was submitted started execution in the GPU", true);
										list.Add(propertyDescriptor2);
										PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<string>("End Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.End, "#.##", "#.###"), text8, "Time when this event that was submitted finished(retired) execution in the GPU", true);
										list.Add(propertyDescriptor3);
										PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<string>("Duration", typeof(string), FormatHelper.FormatTimeLabel(num9, "#.##", "#.###"), text8, "Duration of this event from start to retired", true);
										list.Add(propertyDescriptor4);
										PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<string>("Thread", typeof(string), e.ElementSeries.Name, text8, "Thread this block was submitted on", true);
										list.Add(propertyDescriptor5);
										propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
										inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
										inspectorViewDisplayEventArgs.Description = "Gantt Element";
										SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
									}
								}));
							}
						}
					});
				}
				ModelObject modelObject2 = model.GetModelObject("SystraceKGSLPwrSetState");
				ModelObjectDataList data2 = modelObject2.GetData("captureID", captureID.ToString());
				if (data2 != null && data2.Count > 0 && SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey((int)captureID))
				{
					Series kgslPwrStateSeries = new Series();
					kgslPwrStateSeries.Name = "KGSL Power State";
					Dictionary<uint, string> namesModel = new Dictionary<uint, string>();
					Dictionary<uint, string> tooltipModel = new Dictionary<uint, string>();
					Dictionary<uint, Color> colorsModel = new Dictionary<uint, Color>();
					long dataMin = long.MaxValue;
					long dataMax = long.MinValue;
					uint num6 = 1U;
					Element element2 = null;
					foreach (ModelObjectData modelObjectData2 in data2)
					{
						long num7 = Int64Converter.Convert(modelObjectData2.GetValue("timestamp"));
						string value2 = modelObjectData2.GetValue("state");
						if (element2 != null)
						{
							element2.End = num7;
							string text3 = string.Concat(new string[]
							{
								"KGSL Power State\nState: ",
								value2,
								"\nStart Time: ",
								FormatHelper.FormatTimeLabel(element2.Start, "#.##", "#.###"),
								"\nEnd Time: ",
								FormatHelper.FormatTimeLabel(element2.End, "#.##", "#.###"),
								"\nDuration: ",
								FormatHelper.FormatTimeLabel(element2.End - element2.Start, "#.##", "#.###")
							});
							uint hashCode3 = (uint)text3.GetHashCode();
							if (!tooltipModel.ContainsKey(hashCode3))
							{
								tooltipModel.Add(hashCode3, text3);
							}
							element2.TooltipId = hashCode3;
							kgslPwrStateSeries.Elements.Add(element2);
							dataMin = Math.Min(dataMin, element2.Start);
							dataMax = Math.Max(dataMax, element2.End);
						}
						element2 = new Element();
						element2.Start = num7;
						string text4 = value2;
						uint hashCode4 = (uint)value2.GetHashCode();
						if (!namesModel.ContainsKey(hashCode4))
						{
							namesModel.Add(hashCode4, text4);
						}
						if (!colorsModel.ContainsKey(hashCode4))
						{
							colorsModel.Add(hashCode4, this.PseudoRandomColor());
						}
						element2.LabelId = hashCode4;
						element2.ColorId = hashCode4;
						element2.BlockId = num6++;
					}
					if (kgslPwrStateSeries.Elements.Count > 0)
					{
						Application.Invoke(delegate
						{
							GroupLayoutController groupLayoutController2 = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[(int)captureID];
							if (kgslGroup == null)
							{
								AddGroupCommand addGroupCommand2 = new AddGroupCommand();
								addGroupCommand2.Container = groupLayoutController2;
								addGroupCommand2.GroupName = "KGSL Power State";
								SdpApp.CommandManager.ExecuteCommand(addGroupCommand2);
								kgslGroup = addGroupCommand2.Result;
							}
							if (kgslGroup != null)
							{
								AddTrackToGroupCommand addTrackToGroupCommand2 = new AddTrackToGroupCommand();
								addTrackToGroupCommand2.TrackType = TrackType.Gantt;
								addTrackToGroupCommand2.Container = kgslGroup;
								SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand2);
								GanttTrackController ganttTrackController4 = addTrackToGroupCommand2.Result as GanttTrackController;
								if (ganttTrackController4 != null)
								{
									ganttTrackController4.View.ControlPanelHeaderBackColor = this.PseudoRandomColor();
									ganttTrackController4.Series.Add(kgslPwrStateSeries);
									ganttTrackController4.SetColorsModel(colorsModel);
									ganttTrackController4.NameStringsModel = namesModel;
									ganttTrackController4.TooltipStringsModel = tooltipModel;
									ganttTrackController4.SetDataBounds(dataMin, dataMax);
									ganttTrackController4.Invalidate();
									GanttTrackController ganttTrackController5 = ganttTrackController4;
									ganttTrackController5.ElementSelected = (EventHandler<ElementSelectedEventArgs>)Delegate.Combine(ganttTrackController5.ElementSelected, new EventHandler<ElementSelectedEventArgs>(delegate(object o, ElementSelectedEventArgs e)
									{
										GanttTrackController ganttTrackController6 = o as GanttTrackController;
										if (ganttTrackController6 != null)
										{
											long num10 = e.Selected.End - e.Selected.Start;
											string text9 = "N/A";
											ganttTrackController6.NameStringsModel.TryGetValue(e.Selected.LabelId, out text9);
											InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs2 = new InspectorViewDisplayEventArgs();
											PropertyGridDescriptionObject propertyGridDescriptionObject2 = new PropertyGridDescriptionObject();
											List<PropertyDescriptor> list2 = new List<PropertyDescriptor>();
											string text10 = "KGSL Power State";
											PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<string>("State", typeof(string), text9, text10, "KGSL Power State", true);
											list2.Add(propertyDescriptor6);
											PropertyDescriptor propertyDescriptor7 = new SdpPropertyDescriptor<string>("Start Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.Start, "#.##", "#.###"), text10, "Time when this event that was submitted started execution in the GPU", true);
											list2.Add(propertyDescriptor7);
											PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<string>("End Time", typeof(string), FormatHelper.FormatTimeLabel(e.Selected.End, "#.##", "#.###"), text10, "Time when this event that was submitted finished(retired) execution in the GPU", true);
											list2.Add(propertyDescriptor8);
											PropertyDescriptor propertyDescriptor9 = new SdpPropertyDescriptor<string>("Duration", typeof(string), FormatHelper.FormatTimeLabel(num10, "#.##", "#.###"), text10, "Duration of this event from start to retired", true);
											list2.Add(propertyDescriptor9);
											propertyGridDescriptionObject2.AddPropertyGridDescriptors(list2);
											inspectorViewDisplayEventArgs2.Content = propertyGridDescriptionObject2;
											inspectorViewDisplayEventArgs2.Description = "Gantt Element";
											SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs2);
										}
									}));
								}
							}
						});
					}
				}
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				Console.WriteLine("Timing database extraction... ");
				SystracePlugin.Model.ClearData();
				SystracePlugin.Model.CaptureID = (int)captureID;
				Dictionary<string, ModelObjectDataList> objectDataListDict = new Dictionary<string, ModelObjectDataList>();
				int num8 = 1;
				foreach (string text5 in this.m_tableNames)
				{
					ModelObject modelObject3 = dataModel.GetModelObject(model, text5);
					ModelObjectDataList modelObjectData3 = dataModel.GetModelObjectData(modelObject3, "captureID", captureID.ToString());
					objectDataListDict.Add(text5, modelObjectData3);
					if (SystracePlugin.CaptureProgress != null)
					{
						SystracePlugin.CaptureProgress.CurrentValue = (double)(num8++ / this.m_tableNames.Length);
						SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
					}
				}
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				TimeSpan elapsed = stopwatch.Elapsed;
				string text6 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[]
				{
					elapsed.Hours,
					elapsed.Minutes,
					elapsed.Seconds,
					elapsed.Milliseconds / 10
				});
				Console.WriteLine("RunTime " + text6);
				Thread thread = new Thread(delegate
				{
					this.StoreToModel(objectDataListDict);
				});
				thread.Start();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00008994 File Offset: 0x00006B94
		private void connectionEvents_OptionAdded(object sender, OptionEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			uint optionId = e.OptionId;
			uint processId = e.ProcessId;
			Option option = SdpApp.ConnectionManager.GetOption(optionId, processId);
			if (option == null)
			{
				return;
			}
			if (option.GetName() == "EnablePerfetto")
			{
				string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.EnablePerfetto);
				bool flag = text == null || BoolConverter.Convert(text);
				option.SetValue(flag);
				return;
			}
			if (option.GetName() == "EnableSystrace")
			{
				string text2 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.EnablePerfetto);
				bool flag2 = text2 == null || BoolConverter.Convert(text2);
				option.SetValue(!flag2);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008A4C File Offset: 0x00006C4C
		private void StoreToModel(Dictionary<string, ModelObjectDataList> objectDataListDict)
		{
			SystracePlugin.CaptureProgress = new ProgressObject();
			SystracePlugin.CaptureProgress.Title = "Trace";
			SystracePlugin.CaptureProgress.Description = "Processing Trace Data";
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
			double num = 0.0;
			double num2 = 0.0;
			foreach (KeyValuePair<string, ModelObjectDataList> keyValuePair in objectDataListDict)
			{
				num += (double)keyValuePair.Value.Count;
			}
			foreach (KeyValuePair<string, ModelObjectDataList> keyValuePair2 in objectDataListDict)
			{
				SystraceProcessor.StoreObjectDataToModel storeObjectDataToModel;
				if (this.m_storeToModelDictionary.TryGetValue(keyValuePair2.Key, out storeObjectDataToModel))
				{
					storeObjectDataToModel(keyValuePair2.Value, ref num2, num);
				}
			}
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
			SystracePlugin.CaptureProgress = null;
			SdpApp.EventsManager.Raise(SystracePlugin.Model.Changed, this, null);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00008BA4 File Offset: 0x00006DA4
		private void StoreSystraceCountersToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				int num2 = IntConverter.Convert(modelObjectData.GetValue("processID"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("taskGroupID"));
				string text = Convert.ToString(modelObjectData.GetValue("processName"));
				long num4 = Int64Converter.Convert(modelObjectData.GetValue("counterValue"));
				DataPoint dataPoint = new DataPoint((double)num, (double)num4);
				SystracePlugin.Model.AddDataPoint(num3, text, text, dataPoint);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num5 = currentProgress;
					currentProgress = num5 + 1.0;
					captureProgress.CurrentValue = num5 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008CB8 File Offset: 0x00006EB8
		private void StoreSystraceSchedSwitchToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				string text = Convert.ToString(modelObjectData.GetValue("nextCommand"));
				ulong num3 = Uint64Converter.Convert(modelObjectData.GetValue("nextPID"));
				uint num4 = UintConverter.Convert(modelObjectData.GetValue("cpuNum"));
				uint hashCode = (uint)text.GetHashCode();
				if (!string.IsNullOrEmpty(text) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num5 = num2 - num;
				string text2 = string.Concat(new string[]
				{
					"PID: ",
					num3.ToString(),
					"\nName: ",
					text,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num5, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text2.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(-1, "Sched CPU", (int)num4, element, "Sched CPU" + num4.ToString());
				SystracePlugin.Model.AddStringToModel(-1, "Sched CPU", text, text2);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num6 = currentProgress;
					currentProgress = num6 + 1.0;
					captureProgress.CurrentValue = num6 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008F00 File Offset: 0x00007100
		private void StoreSystraceFunctionsToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				string text = Convert.ToString(modelObjectData.GetValue("functionName"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("taskID"));
				int num4 = IntConverter.Convert(modelObjectData.GetValue("taskGroupID"));
				int num5 = IntConverter.Convert(modelObjectData.GetValue("depth"));
				string text2 = Convert.ToString(modelObjectData.GetValue("taskName"));
				uint hashCode = (uint)text.GetHashCode();
				if (!string.IsNullOrEmpty(text) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num6 = num2 - num;
				string text3 = string.Concat(new string[]
				{
					"PID: ",
					num3.ToString(),
					"\nName: ",
					text,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num6, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text3.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(num4, text2 + "-" + num3.ToString(), num5, element, null);
				SystracePlugin.Model.AddStringToModel(num4, text2 + "-" + num3.ToString(), text, text3);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num7 = currentProgress;
					currentProgress = num7 + 1.0;
					captureProgress.CurrentValue = num7 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000917C File Offset: 0x0000737C
		private void StoreSystraceASyncFuncsToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("rowIndex"));
				int num4 = IntConverter.Convert(modelObjectData.GetValue("taskGroupID"));
				int num5 = IntConverter.Convert(modelObjectData.GetValue("taskID"));
				string text = Convert.ToString(modelObjectData.GetValue("markerName"));
				uint hashCode = (uint)text.GetHashCode();
				if (!string.IsNullOrEmpty(text) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num6 = num2 - num;
				string text2 = string.Concat(new string[]
				{
					"PID: ",
					num5.ToString(),
					"\nName: ",
					text,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num6, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text2.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(num4, "(async)", num3, element, null);
				SystracePlugin.Model.AddStringToModel(num4, "(async)", text, text2);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num7 = currentProgress;
					currentProgress = num7 + 1.0;
					captureProgress.CurrentValue = num7 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000093C8 File Offset: 0x000075C8
		private void StoreSystraceClockSetMarkersToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				string text = Convert.ToString(modelObjectData.GetValue("clockName"));
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("clockFrequency"));
				DataPoint dataPoint = new DataPoint((double)num, (double)num2);
				SystracePlugin.Model.AddDataPoint(-1, "CPU Clocks", text, dataPoint);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num3 = currentProgress;
					currentProgress = num3 + 1.0;
					captureProgress.CurrentValue = num3 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000094BC File Offset: 0x000076BC
		private void StoreSystraceCpuFreqChangesToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("clockFrequency"));
				uint num2 = UintConverter.Convert(modelObjectData.GetValue("cpuNum"));
				long num3 = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				string text = "CPU" + num2.ToString();
				DataPoint dataPoint = new DataPoint((double)num3, (double)num);
				SystracePlugin.Model.AddDataPoint(-1, "CPU Frequency", text, dataPoint);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num4 = currentProgress;
					currentProgress = num4 + 1.0;
					captureProgress.CurrentValue = num4 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000095C4 File Offset: 0x000077C4
		private void StoreSystraceCpuIdleToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("state"));
				uint num2 = UintConverter.Convert(modelObjectData.GetValue("cpuNum"));
				long num3 = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				string text = "CPU" + num2.ToString() + " Idle";
				DataPoint dataPoint = new DataPoint((double)num3, (double)num);
				SystracePlugin.Model.AddDataPoint(-1, "CPU Idle Tracking", text, dataPoint);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num4 = currentProgress;
					currentProgress = num4 + 1.0;
					captureProgress.CurrentValue = num4 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000096D0 File Offset: 0x000078D0
		private void StoreSystraceSyncTimelineToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("taskID"));
				string text = Convert.ToString(modelObjectData.GetValue("timelineName"));
				string text2 = Convert.ToString(modelObjectData.GetValue("timelineValue"));
				uint hashCode = (uint)text2.GetHashCode();
				if (!string.IsNullOrEmpty(text2) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num4 = num2 - num;
				string text3 = string.Concat(new string[]
				{
					"PID: ",
					num3.ToString(),
					"\nName: ",
					text2,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num4, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text3.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(0, text, 0, element, null);
				SystracePlugin.Model.AddStringToModel(0, text, text2, text3);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num5 = currentProgress;
					currentProgress = num5 + 1.0;
					captureProgress.CurrentValue = num5 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009904 File Offset: 0x00007B04
		private void StoreSystraceSyncWaitToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("taskID"));
				int num4 = IntConverter.Convert(modelObjectData.GetValue("taskGroupID"));
				string text = Convert.ToString(modelObjectData.GetValue("taskName"));
				string text2 = Convert.ToString(modelObjectData.GetValue("waitName"));
				string text3 = "fence_wait(\"" + text2 + "\")";
				uint hashCode = (uint)text3.GetHashCode();
				if (!string.IsNullOrEmpty(text3) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num5 = num2 - num;
				string text4 = string.Concat(new string[]
				{
					"PID: ",
					num3.ToString(),
					"\nName: ",
					text3,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num5, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text4.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(num4, text, 0, element, null);
				SystracePlugin.Model.AddStringToModel(num4, text, text3, text4);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num6 = currentProgress;
					currentProgress = num6 + 1.0;
					captureProgress.CurrentValue = num6 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00009B5C File Offset: 0x00007D5C
		private void StoreSystraceWorkExecToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				int num3 = IntConverter.Convert(modelObjectData.GetValue("taskGroupID"));
				int num4 = IntConverter.Convert(modelObjectData.GetValue("taskID"));
				string text = Convert.ToString(modelObjectData.GetValue("taskName"));
				string text2 = Convert.ToString(modelObjectData.GetValue("workFunction"));
				uint hashCode = (uint)text2.GetHashCode();
				if (!string.IsNullOrEmpty(text2) && !SystracePlugin.Model.ColorsModel.ContainsKey(hashCode))
				{
					SystracePlugin.Model.ColorsModel.Add(hashCode, this.PseudoRandomColor());
				}
				long num5 = num2 - num;
				string text3 = string.Concat(new string[]
				{
					"PID: ",
					num4.ToString(),
					"\nName: ",
					text2,
					"\nStart Time: ",
					FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
					"\nEnd Time: ",
					FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
					"\nDuration: ",
					FormatHelper.FormatTimeLabel(num5, "#.##", "#.###")
				});
				uint hashCode2 = (uint)text3.GetHashCode();
				Element element = new Element();
				element.Start = num;
				element.End = num2;
				element.LabelId = hashCode;
				element.TooltipId = hashCode2;
				element.ColorId = hashCode;
				element.BlockId = SystracePlugin.Model.GetNextBlockID();
				SystracePlugin.Model.AddElement(num3, text, 0, element, null);
				SystracePlugin.Model.AddStringToModel(num3, text, text2, text3);
				if (SystracePlugin.CaptureProgress != null)
				{
					ProgressObject captureProgress = SystracePlugin.CaptureProgress;
					double num6 = currentProgress;
					currentProgress = num6 + 1.0;
					captureProgress.CurrentValue = num6 / totalProgress;
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00009DA4 File Offset: 0x00007FA4
		private void StoreSystraceGraphSeriesToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				if (num > 0L)
				{
					int num2 = IntConverter.Convert(modelObjectData.GetValue("pid"));
					if (num2 == 0)
					{
						num2 = -1;
					}
					string value = modelObjectData.GetValue("eventName");
					string value2 = modelObjectData.GetValue("label");
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string value3 = modelObjectData.GetValue("params");
					Regex regex = new Regex("\\ +");
					string text = regex.Replace(value3, " ");
					string[] array = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string text2 in array)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array3 = text2.Split(new char[] { ' ' });
							if (array3.Length == 2)
							{
								dictionary[array3[0]] = array3[1];
							}
						}
					}
					foreach (KeyValuePair<string, string> keyValuePair in dictionary)
					{
						double num3 = 0.0;
						string key = keyValuePair.Key;
						if (double.TryParse(keyValuePair.Value, out num3))
						{
							DataPoint dataPoint = new DataPoint((double)num, num3);
							SystracePlugin.Model.AddDataPoint(num2, value, value2 + " - " + key, dataPoint);
						}
					}
					if (SystracePlugin.CaptureProgress != null)
					{
						ProgressObject captureProgress = SystracePlugin.CaptureProgress;
						double num4 = currentProgress;
						currentProgress = num4 + 1.0;
						captureProgress.CurrentValue = num4 / totalProgress;
						SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
					}
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00009FC0 File Offset: 0x000081C0
		private void StoreSystraceMarkersToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestamp"));
				if (num > 0L)
				{
					int num2 = IntConverter.Convert(modelObjectData.GetValue("pid"));
					if (num2 == 0)
					{
						num2 = -1;
					}
					string value = modelObjectData.GetValue("trackName");
					string value2 = modelObjectData.GetValue("eventName");
					int num3 = IntConverter.Convert(modelObjectData.GetValue("depth"));
					string value3 = modelObjectData.GetValue("depthName");
					uint num4 = UintConverter.Convert(modelObjectData.GetValue("color"));
					string value4 = modelObjectData.GetValue("params");
					MarkerStyle markerStyle = (MarkerStyle)UintConverter.Convert(modelObjectData.GetValue("markerStyle"));
					Marker marker = new Marker();
					marker.Position = num;
					switch (num4)
					{
					case 0U:
						marker.Color = new Color(1.0, 0.71, 0.75, 1.0);
						break;
					case 1U:
						marker.Color = new Color(0.69, 0.84, 0.69, 1.0);
						break;
					case 2U:
						marker.Color = new Color(0.0, 0.0, 1.0, 1.0);
						break;
					case 3U:
						marker.Color = new Color(1.0, 0.65, 0.0, 1.0);
						break;
					}
					marker.Style = markerStyle;
					string text = string.Concat(new string[]
					{
						value2,
						"\n",
						value4,
						"Time: ",
						FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
						"\n"
					});
					uint hashCode = (uint)text.GetHashCode();
					marker.TooltipId = hashCode;
					SystracePlugin.Model.AddMarker(num2, value, num3, marker, value3);
					SystracePlugin.Model.AddStringToModel(num2, value, value2, text);
				}
			}
			if (SystracePlugin.CaptureProgress != null)
			{
				ProgressObject captureProgress = SystracePlugin.CaptureProgress;
				double num5 = currentProgress;
				currentProgress = num5 + 1.0;
				captureProgress.CurrentValue = num5 / totalProgress;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000A27C File Offset: 0x0000847C
		private void StoreSystraceGanttElementsToModel(ModelObjectDataList list, ref double currentProgress, double totalProgress)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (ModelObjectData modelObjectData in list)
			{
				long num = Int64Converter.Convert(modelObjectData.GetValue("timestampBegin"));
				long num2 = Int64Converter.Convert(modelObjectData.GetValue("timestampEnd"));
				if (num > 0L)
				{
					string text = modelObjectData.GetValue("params");
					long num3 = num2 - num;
					if (!text.EndsWith("\n"))
					{
						text += "\n";
					}
					string text2 = string.Concat(new string[]
					{
						text,
						"Start Time: ",
						FormatHelper.FormatTimeLabel(num, "#.##", "#.###"),
						"\nEnd Time: ",
						FormatHelper.FormatTimeLabel(num2, "#.##", "#.###"),
						"\nDuration: ",
						FormatHelper.FormatTimeLabel(num3, "#.##", "#.###")
					});
					uint hashCode = (uint)text2.GetHashCode();
					string value = modelObjectData.GetValue("elementTitle");
					uint hashCode2 = (uint)value.GetHashCode();
					if (!SystracePlugin.Model.ColorsModel.ContainsKey(hashCode2))
					{
						SystracePlugin.Model.ColorsModel.Add(hashCode2, this.PseudoRandomColor());
					}
					string value2 = modelObjectData.GetValue("trackName");
					int num4 = IntConverter.Convert(modelObjectData.GetValue("depth"));
					string value3 = modelObjectData.GetValue("depthName");
					Element element = new Element();
					element.Start = num;
					element.End = num2;
					element.LabelId = hashCode2;
					element.TooltipId = hashCode;
					element.ColorId = hashCode2;
					element.BlockId = SystracePlugin.Model.GetNextBlockID();
					int num5 = IntConverter.Convert(modelObjectData.GetValue("pid"));
					if (num5 == 0)
					{
						num5 = -1;
					}
					SystracePlugin.Model.AddElement(num5, value2, num4, element, value3);
					SystracePlugin.Model.AddStringToModel(num5, value2, value, text2);
					if (SystracePlugin.CaptureProgress != null)
					{
						ProgressObject captureProgress = SystracePlugin.CaptureProgress;
						double num6 = currentProgress;
						currentProgress = num6 + 1.0;
						captureProgress.CurrentValue = num6 / totalProgress;
						SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(SystracePlugin.CaptureProgress));
					}
				}
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000A4DC File Offset: 0x000086DC
		private Color PseudoRandomColor()
		{
			return FormatHelper.PseudoRandomColor();
		}

		// Token: 0x04000069 RID: 105
		public const int TraceKernelGroupID = -1;

		// Token: 0x0400006A RID: 106
		private const string SystraceModelName = "SystraceModel";

		// Token: 0x0400006B RID: 107
		private const string SystraceCountersModelObjectName = "tblSystraceCounters";

		// Token: 0x0400006C RID: 108
		private const string SystraceSchedSwitchModelObjectName = "tblSystraceSchedSwitch";

		// Token: 0x0400006D RID: 109
		private const string SystraceFunctionModelObjectName = "tblSystraceFunctions";

		// Token: 0x0400006E RID: 110
		private const string SystraceASyncFuncModelObjectName = "tblSystraceASyncFuncs";

		// Token: 0x0400006F RID: 111
		private const string SystraceClockSetModelObjectName = "tblSystraceClockSetRate";

		// Token: 0x04000070 RID: 112
		private const string SystraceCpuFreqModelObjectName = "tblSystraceCpuFreq";

		// Token: 0x04000071 RID: 113
		private const string SystraceCpuIdleModelObjectName = "tblSystraceCpuIdle";

		// Token: 0x04000072 RID: 114
		private const string SystraceSyncTimelineModelObjectName = "tblSystraceSyncTimeline";

		// Token: 0x04000073 RID: 115
		private const string SystraceSyncWaitModelObjectName = "tblSystraceSyncWait";

		// Token: 0x04000074 RID: 116
		private const string SystraceWorkExecModelObjectName = "tblSystraceWorkExec";

		// Token: 0x04000075 RID: 117
		private const string SystraceGraphSeriesModelObjectName = "tblSystraceGraphSeriesTable";

		// Token: 0x04000076 RID: 118
		private const string SystraceMarkerModelObjectName = "tblSystraceMarkersTable";

		// Token: 0x04000077 RID: 119
		private const string SystraceGanttElementModelObjectName = "tblSystraceGanttElementsTable";

		// Token: 0x04000078 RID: 120
		private string[] m_tableNames = new string[]
		{
			"tblSystraceCounters", "tblSystraceSchedSwitch", "tblSystraceFunctions", "tblSystraceASyncFuncs", "tblSystraceClockSetRate", "tblSystraceCpuFreq", "tblSystraceCpuIdle", "tblSystraceSyncTimeline", "tblSystraceSyncWait", "tblSystraceWorkExec",
			"tblSystraceGraphSeriesTable", "tblSystraceMarkersTable", "tblSystraceGanttElementsTable"
		};

		// Token: 0x04000079 RID: 121
		public const string TimestampObjAttr = "timestamp";

		// Token: 0x0400007A RID: 122
		public const string TimestampBeginObjAttr = "timestampBegin";

		// Token: 0x0400007B RID: 123
		public const string TimestampEndObjAttr = "timestampEnd";

		// Token: 0x0400007C RID: 124
		public const string CaptureIDObjAttr = "captureID";

		// Token: 0x0400007D RID: 125
		public const string ProcessIDObjAttr = "processID";

		// Token: 0x0400007E RID: 126
		public const string TaskGroupIDObjAttr = "taskGroupID";

		// Token: 0x0400007F RID: 127
		public const string ProcessNameObjAttr = "processName";

		// Token: 0x04000080 RID: 128
		public const string CounterValueObjAttr = "counterValue";

		// Token: 0x04000081 RID: 129
		public const string NextCommandObjAttr = "nextCommand";

		// Token: 0x04000082 RID: 130
		public const string NextPIDObjAttr = "nextPID";

		// Token: 0x04000083 RID: 131
		public const string CpuNumObjAttr = "cpuNum";

		// Token: 0x04000084 RID: 132
		public const string FunctionNameObjAttr = "functionName";

		// Token: 0x04000085 RID: 133
		public const string MarkerNameObjAttr = "markerName";

		// Token: 0x04000086 RID: 134
		public const string TaskIDObjAttr = "taskID";

		// Token: 0x04000087 RID: 135
		public const string DepthObjAttr = "depth";

		// Token: 0x04000088 RID: 136
		public const string TaskNameObjAttr = "taskName";

		// Token: 0x04000089 RID: 137
		public const string RowIndexObjAttr = "rowIndex";

		// Token: 0x0400008A RID: 138
		public const string ClockNameObjAttr = "clockName";

		// Token: 0x0400008B RID: 139
		public const string ClockFreqObjAttr = "clockFrequency";

		// Token: 0x0400008C RID: 140
		public const string StateObjAttr = "state";

		// Token: 0x0400008D RID: 141
		public const string TimelineNameObjAttr = "timelineName";

		// Token: 0x0400008E RID: 142
		public const string TimelineValueObjAttr = "timelineValue";

		// Token: 0x0400008F RID: 143
		public const string WaitNameObjAttr = "waitName";

		// Token: 0x04000090 RID: 144
		public const string WorkStructObjAttr = "workStruct";

		// Token: 0x04000091 RID: 145
		public const string WorkColFunctionName = "workFunction";

		// Token: 0x04000092 RID: 146
		public const string PidObjAttr = "pid";

		// Token: 0x04000093 RID: 147
		public const string EventNameObjAttr = "eventName";

		// Token: 0x04000094 RID: 148
		public const string LabelObjAttr = "label";

		// Token: 0x04000095 RID: 149
		public const string ParamsObjAttr = "params";

		// Token: 0x04000096 RID: 150
		public const string DepthTitleObjAttr = "depthTitle";

		// Token: 0x04000097 RID: 151
		public const string DepthNameObjAttr = "depthName";

		// Token: 0x04000098 RID: 152
		public const string ColorObjAttr = "color";

		// Token: 0x04000099 RID: 153
		public const string MarkerStyleObjAttr = "markerStyle";

		// Token: 0x0400009A RID: 154
		public const string TrackNameObjAttr = "trackName";

		// Token: 0x0400009B RID: 155
		public const string ElementTitleObjAttr = "elementTitle";

		// Token: 0x0400009C RID: 156
		public const string SchedCPUNamePrefix = "Sched CPU";

		// Token: 0x0400009D RID: 157
		private const string ASyncTrackName = "(async)";

		// Token: 0x0400009E RID: 158
		private Random random;

		// Token: 0x0400009F RID: 159
		private Dictionary<string, SystraceProcessor.StoreObjectDataToModel> m_storeToModelDictionary = new Dictionary<string, SystraceProcessor.StoreObjectDataToModel>();

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x060000D7 RID: 215
		private delegate void StoreObjectDataToModel(ModelObjectDataList dataList, ref double currentProgress, double totalProgress);
	}
}
