using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Gdk;
using QGLPlugin.ModelObjectGateways.QGLApiQueueSubmitTimings;
using QGLPlugin.ModelObjectGateways.QGLApiTracePackets;
using Sdp;
using Sdp.Functional;
using Sdp.Helpers;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x02000034 RID: 52
	internal class DataExplorerViewMgr
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00009E30 File Offset: 0x00008030
		public DataExplorerViewMgr()
		{
			DataExplorerViewEvents dataExplorerViewEvents = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents.SourceSelected = (EventHandler<SourceEventArgs>)Delegate.Combine(dataExplorerViewEvents.SourceSelected, new EventHandler<SourceEventArgs>(this.dataExplorerViewEvents_SourceSelected));
			DataExplorerViewEvents dataExplorerViewEvents2 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents2.RowSelected = (EventHandler<DataExplorerViewRowSelectedEventArgs>)Delegate.Combine(dataExplorerViewEvents2.RowSelected, new EventHandler<DataExplorerViewRowSelectedEventArgs>(this.dataExplorerViewEvents_RowSelected));
			DataExplorerViewEvents dataExplorerViewEvents3 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents3.SelectRow = (EventHandler<DataExplorerViewSelectRowEventArgs>)Delegate.Combine(dataExplorerViewEvents3.SelectRow, new EventHandler<DataExplorerViewSelectRowEventArgs>(this.dataExplorerViewEvents_SelectRow));
			DataExplorerViewEvents dataExplorerViewEvents4 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents4.CustomComboOptionSelected = (EventHandler<CustomComboSelectedEventArgs>)Delegate.Combine(dataExplorerViewEvents4.CustomComboOptionSelected, new EventHandler<CustomComboSelectedEventArgs>(this.dataExplorerViewEvents_CustomComboSelected));
			ResourceViewEvents resourceViewEvents = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents.SourceSelected = (EventHandler<SourceEventArgs>)Delegate.Combine(resourceViewEvents.SourceSelected, new EventHandler<SourceEventArgs>(this.resourceViewEvents_SourceSelected));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents.OptionAdded, new EventHandler<OptionEventArgs>(this.connectionEvents_OptionAdded));
			QGLModel.CreateVulkanReplayMappingModel();
			this.m_logger = new global::Sdp.Logging.Logger("QGL Client Plugin");
			DataExplorerViewMgr.m_replayNone = Pixbuf.LoadFromResource("QGLPlugin.Resources.replay_none.png");
			DataExplorerViewMgr.m_replayLight = Pixbuf.LoadFromResource("QGLPlugin.Resources.replay_light.png");
			DataExplorerViewMgr.m_replayDark = Pixbuf.LoadFromResource("QGLPlugin.Resources.replay_dark.png");
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009F94 File Offset: 0x00008194
		private void PopulateAPITrace(int captureID)
		{
			if (!PerfHintMgr.PerfHintsProcessed.Contains(captureID))
			{
				return;
			}
			Thread thread = new Thread(delegate
			{
				ProgressObject progressObject = new ProgressObject();
				progressObject.Title = "Vulkan API Trace";
				progressObject.Description = "Processing Data Explorer";
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(progressObject));
				ModelObjectDataList apis = QGLModel.GetAPIs(captureID);
				if (apis != null && apis.Count > 0)
				{
					double num = (double)apis.Count;
					double num2 = 0.0;
					DateTime dateTime = DateTime.Now;
					DateTime dateTime2 = DateTime.Now;
					DataExplorerViewInvalidateEventArgs dataExplorerViewInvalidateEventArgs = new DataExplorerViewInvalidateEventArgs();
					dataExplorerViewInvalidateEventArgs.Filters = new List<DataExplorerViewFilter>();
					DataExplorerViewEntryFilter dataExplorerViewEntryFilter = new DataExplorerViewEntryFilter();
					dataExplorerViewEntryFilter.Label = "Filter by API name...";
					dataExplorerViewEntryFilter.FilterColumn = 2;
					dataExplorerViewInvalidateEventArgs.Filters.Add(dataExplorerViewEntryFilter);
					List<Type> list = new List<Type>
					{
						typeof(int),
						typeof(int),
						typeof(string),
						typeof(uint),
						typeof(string),
						typeof(string),
						typeof(long),
						typeof(double),
						typeof(double),
						typeof(string)
					};
					uint num3 = 0U;
					int num4 = 0;
					TreeModel treeModel = new TreeModel(list.ToArray());
					treeModel.Container = TreeModel.ContainerType.List;
					foreach (ModelObjectData modelObjectData in apis)
					{
						int num5 = IntConverter.Convert(modelObjectData.GetValue("m_callID"));
						string value = modelObjectData.GetValue("m_functionName");
						string text = modelObjectData.GetValue("m_functionParams");
						text = text.Replace("<", "").Replace(">", "");
						string value2 = modelObjectData.GetValue("m_functionReturnValue");
						long num6 = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)Uint64Converter.Convert(modelObjectData.GetValue("m_functionCPUStartTime")));
						long num7 = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)Uint64Converter.Convert(modelObjectData.GetValue("m_functionCPUEndTime")));
						uint num8 = UintConverter.Convert(modelObjectData.GetValue("m_threadID"));
						uint numPerfHintsForAPICall = PerfHintMgr.GetNumPerfHintsForAPICall((uint)num5, (uint)captureID);
						num3 += numPerfHintsForAPICall;
						TreeNode treeNode = new TreeNode();
						treeNode.Values = new object[]
						{
							num5,
							num4++,
							value,
							numPerfHintsForAPICall,
							text,
							value2,
							num7 - num6,
							(double)num6 / 1000.0,
							(double)num7 / 1000.0,
							"0x" + num8.ToString("X")
						};
						treeModel.Nodes.Add(treeNode);
						dateTime2 = DateTime.Now;
						num2 += 1.0;
						if ((dateTime2 - dateTime).TotalSeconds > 1.0)
						{
							dateTime = dateTime2;
							progressObject.CurrentValue = num2 / num;
							SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(progressObject));
						}
					}
					dataExplorerViewInvalidateEventArgs.Model = treeModel;
					dataExplorerViewInvalidateEventArgs.Columns = new List<DataExplorerViewColumn>();
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("#", 1, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("API Name", 2, TreeModel.ParserType.STRING));
					if (num3 > 0U)
					{
						dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Perf Hints", 3, TreeModel.ParserType.STRING));
					}
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("CPU Duration (us)", 6, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("CPU Start (ms)", 7, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("CPU End (ms)", 8, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Thread ID", 9, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Return", 5, TreeModel.ParserType.STRING));
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Parameters", 4, TreeModel.ParserType.STRING));
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(progressObject));
					SdpApp.EventsManager.Raise<DataExplorerViewInvalidateEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.Invalidate, this, dataExplorerViewInvalidateEventArgs);
				}
			});
			thread.Start();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00009FE0 File Offset: 0x000081E0
		private void PopulateQueueSubmit(int captureID)
		{
			ModelObjectDataList queueSubmitTimings = QGLModel.GetQueueSubmitTimings(captureID);
			if (queueSubmitTimings != null && queueSubmitTimings.Count > 0)
			{
				DataExplorerViewInvalidateEventArgs dataExplorerViewInvalidateEventArgs = new DataExplorerViewInvalidateEventArgs();
				dataExplorerViewInvalidateEventArgs.Columns = new List<DataExplorerViewColumn>();
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("LoggingID", 0, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("InstanceID", 1, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("CallID", 2, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Index", 3, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Command Buffer", 4, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("GPU Start (ns)", 5, TreeModel.ParserType.STRING));
				dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("GPU End (ns)", 6, TreeModel.ParserType.STRING));
				List<Type> list = new List<Type>
				{
					typeof(int),
					typeof(uint),
					typeof(uint),
					typeof(uint),
					typeof(ulong),
					typeof(ulong),
					typeof(ulong)
				};
				TreeModel treeModel = new TreeModel(list.ToArray());
				foreach (ModelObjectData modelObjectData in queueSubmitTimings)
				{
					int num = IntConverter.Convert(modelObjectData.GetValue("m_loggingID"));
					uint num2 = UintConverter.Convert(modelObjectData.GetValue("m_instanceID"));
					uint num3 = UintConverter.Convert(modelObjectData.GetValue("m_callID"));
					uint num4 = UintConverter.Convert(modelObjectData.GetValue("m_index"));
					ulong num5 = Uint64Converter.Convert(modelObjectData.GetValue("m_commandBuffer"));
					ulong num6 = Uint64Converter.Convert(modelObjectData.GetValue("m_functionGPUStartTimeNS"));
					ulong num7 = Uint64Converter.Convert(modelObjectData.GetValue("m_functionGPUEndTimeNS"));
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[] { num, num2, num3, num4, num5, num6, num7 };
					treeModel.Nodes.Add(treeNode);
				}
				dataExplorerViewInvalidateEventArgs.Model = treeModel;
				SdpApp.EventsManager.Raise<DataExplorerViewInvalidateEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.Invalidate, this, dataExplorerViewInvalidateEventArgs);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A2A0 File Offset: 0x000084A0
		private void PopulateSnapshot(int captureID)
		{
			DateTime now = DateTime.Now;
			QGLPlugin.VkSnapshotModel.SnapshotStarted();
			VkMetricsCapturedModel metricsForCapture = QGLPlugin.VkSnapshotModel.GetMetricsForCapture((uint)captureID);
			VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(captureID);
			this.m_modelTypes = new List<Type>
			{
				typeof(uint),
				typeof(string),
				typeof(string),
				typeof(ObjectWithToolTip),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(uint),
				typeof(string),
				typeof(ObjectWithToolTip)
			};
			DataExplorerViewInvalidateEventArgs dataExplorerViewInvalidateEventArgs = new DataExplorerViewInvalidateEventArgs();
			dataExplorerViewInvalidateEventArgs.ExpanderColumnIndex = 2;
			dataExplorerViewInvalidateEventArgs.Columns = new List<DataExplorerViewColumn>();
			dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("#", 1, TreeModel.ParserType.STRING));
			DataExplorerViewColumn dataExplorerViewColumn = new DataExplorerViewColumn("", 9, TreeModel.ParserType.STRING);
			dataExplorerViewColumn.Visible = false;
			dataExplorerViewColumn.HasPixbuf = true;
			dataExplorerViewInvalidateEventArgs.Columns.Add(dataExplorerViewColumn);
			dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Name", 2, TreeModel.ParserType.STRING));
			uint num = uint.MaxValue;
			foreach (uint num2 in metricsForCapture.Metrics)
			{
				int metricColumn = metricsForCapture.GetMetricColumn(num2);
				this.m_modelTypes.Add(typeof(double));
				this.m_modelTypes.Add(typeof(ObjectWithToolTip));
				Metric metric = MetricManager.Get().GetMetric(num2);
				if (metric.IsValid())
				{
					string name = metric.GetProperties().name;
					if (name.Equals(SDPCore.LRZStateMetricName))
					{
						num = num2;
					}
					dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn(name, metricColumn + 1, metricColumn, false, TreeModel.ParserType.STRING));
				}
				else
				{
					string importedMetricName = VkHelper.GetImportedMetricName(num2);
					if (!string.IsNullOrEmpty(importedMetricName))
					{
						dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn(importedMetricName, metricColumn + 1, metricColumn, false, TreeModel.ParserType.STRING));
						if (importedMetricName.Equals(SDPCore.LRZStateMetricName))
						{
							num = num2;
						}
					}
					else
					{
						dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Unknown metric " + metricColumn.ToString(), metricColumn + 1, metricColumn, false, TreeModel.ParserType.STRING));
					}
				}
			}
			dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Parameters", 3, TreeModel.ParserType.JSON));
			dataExplorerViewInvalidateEventArgs.Columns.Add(new DataExplorerViewColumn("Thread", 4, TreeModel.ParserType.STRING));
			if (capture.Builder == null)
			{
				capture.Builder = new VkAPITreeModelBuilder((uint)captureID, metricsForCapture, num);
				capture.Builder.ProcessAllCalls();
			}
			TreeModel treeModel = new TreeModel(this.m_modelTypes.ToArray());
			treeModel.Nodes = capture.Builder.DrawcallNodes;
			treeModel.Container = TreeModel.ContainerType.List;
			if (capture.Builder.DrawcallNodes.Count > 0)
			{
				treeModel.InitialSelectedNode = Enumerable.Last<TreeNode>(capture.Builder.DrawcallNodes);
			}
			dataExplorerViewInvalidateEventArgs.Filters = new List<DataExplorerViewFilter>();
			if (capture.Builder.Drawcalls.Count > 0)
			{
				DataExplorerCustomComboFilter dataExplorerCustomComboFilter = new DataExplorerCustomComboFilter();
				dataExplorerCustomComboFilter.FilterColumn = 5;
				dataExplorerCustomComboFilter.Title = "Drawcalls";
				dataExplorerCustomComboFilter.SearchStrings = Enumerable.ToArray<string>(Enumerable.Prepend<string>(Enumerable.Prepend<string>(capture.Builder.Drawcalls, "Show All"), "Show Only Drawcalls"));
				dataExplorerViewInvalidateEventArgs.Filters.Add(dataExplorerCustomComboFilter);
			}
			if (capture.Builder.Threads.Count > 1)
			{
				DataExplorerViewComboFilter dataExplorerViewComboFilter = new DataExplorerViewComboFilter();
				dataExplorerViewComboFilter.FilterColumn = 4;
				dataExplorerViewComboFilter.SearchStrings = Enumerable.ToArray<string>(Enumerable.Prepend<string>(capture.Builder.Threads.Values, "All Threads"));
				dataExplorerViewComboFilter.Title = "Threads";
				dataExplorerViewInvalidateEventArgs.Filters.Add(dataExplorerViewComboFilter);
			}
			DataExplorerViewEntryFilter dataExplorerViewEntryFilter = new DataExplorerViewEntryFilter();
			dataExplorerViewEntryFilter.FilterColumn = 5;
			dataExplorerViewEntryFilter.Label = "Filter by API name...";
			dataExplorerViewInvalidateEventArgs.Filters.Add(dataExplorerViewEntryFilter);
			DataExplorerViewEntryFilter dataExplorerViewEntryFilter2 = new DataExplorerViewEntryFilter();
			dataExplorerViewEntryFilter2.FilterColumn = 6;
			dataExplorerViewEntryFilter2.Label = "Filter by parameters...";
			dataExplorerViewInvalidateEventArgs.Filters.Add(dataExplorerViewEntryFilter2);
			DateTime now2 = DateTime.Now;
			this.m_logger.LogInformation("DataExplorer processing: " + (now2 - now).TotalMilliseconds.ToString() + " ms");
			dataExplorerViewInvalidateEventArgs.Model = treeModel;
			SdpApp.EventsManager.Raise<DataExplorerViewInvalidateEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.Invalidate, this, dataExplorerViewInvalidateEventArgs);
			ShaderAnalyzerAddSnapshotAPIsEventArgs shaderAnalyzerAddSnapshotAPIsEventArgs = new ShaderAnalyzerAddSnapshotAPIsEventArgs();
			shaderAnalyzerAddSnapshotAPIsEventArgs.CaptureID = captureID;
			shaderAnalyzerAddSnapshotAPIsEventArgs.Source = 353;
			shaderAnalyzerAddSnapshotAPIsEventArgs.Model = treeModel;
			SdpApp.EventsManager.Raise<ShaderAnalyzerAddSnapshotAPIsEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.AddSnapshotAPIs, this, shaderAnalyzerAddSnapshotAPIsEventArgs);
			Option option = SdpApp.ConnectionManager.GetOption(SDPCore.OPT_VULKAN_SHADER_PROFILING_SUPPORT, uint.MaxValue);
			if (option != null)
			{
				bool flag;
				option.GetValue(out flag);
				if (flag)
				{
					SdpApp.EventsManager.Raise(SdpApp.EventsManager.DataExplorerViewEvents.ShowShaderProfiling, this, EventArgs.Empty);
				}
			}
			QGLPlugin.VkSnapshotModel.SnapshotFinished();
			SdpApp.EventsManager.Raise<MultiViewArgs>(SdpApp.EventsManager.DataExplorerViewEvents.HideStatus, this, MultiViewArgs.Empty);
			SdpApp.CommandManager.ExecuteCommand(new AddSourceCommand(MultiSourceViews.Resources, captureID, 353, "Vulkan", null));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A834 File Offset: 0x00008A34
		private string FindParameter(string encodedParameters, int indexOfParameter)
		{
			if (indexOfParameter <= 0)
			{
				return null;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < indexOfParameter; i++)
			{
				num = num2 + 1;
				num2 = encodedParameters.IndexOf(';', num);
				if (num2 == 0)
				{
					return null;
				}
			}
			return encodedParameters.Substring(num, num2 - num);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000A874 File Offset: 0x00008A74
		private void resourceViewEvents_SourceSelected(object sender, SourceEventArgs sourceArgs)
		{
			if (sourceArgs.SourceID == 353)
			{
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectSource, this, sourceArgs);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		private void dataExplorerViewEvents_SourceSelected(object sender, SourceEventArgs e)
		{
			this.m_selectedRow = null;
			int sourceID = e.SourceID;
			if (sourceID == 353)
			{
				this.PopulateSnapshot(e.CaptureID);
				return;
			}
			if (sourceID == 2401)
			{
				this.PopulateAPITrace(e.CaptureID);
				return;
			}
			if (sourceID != 2402)
			{
				return;
			}
			this.PopulateQueueSubmit(e.CaptureID);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000A8FC File Offset: 0x00008AFC
		private void dataExplorerViewEvents_CustomComboSelected(object sender, CustomComboSelectedEventArgs e)
		{
			if (e.SourceID == 353)
			{
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture(e.CaptureID);
				TreeModel treeModel = new TreeModel(this.m_modelTypes.ToArray());
				if (e.Option == "Show All")
				{
					treeModel.Nodes = capture.Builder.AllNodes;
				}
				else
				{
					treeModel.Container = TreeModel.ContainerType.List;
					if (e.Option == null)
					{
						treeModel.Nodes = capture.Builder.DrawcallNodes;
					}
					else
					{
						foreach (TreeNode treeNode in capture.Builder.DrawcallNodes)
						{
							if (treeNode.Values[5].ToString() == e.Option)
							{
								treeModel.Nodes.Add(treeNode);
							}
						}
					}
				}
				treeModel.UpdateFilters = true;
				if (this.m_selectedRow != null)
				{
					treeModel.InitialSelectedNode = treeModel.FindNode(this.m_selectedRow, 0);
				}
				DataExplorerViewInvalidateEventArgs dataExplorerViewInvalidateEventArgs = new DataExplorerViewInvalidateEventArgs();
				dataExplorerViewInvalidateEventArgs.Model = treeModel;
				dataExplorerViewInvalidateEventArgs.ExpanderColumnIndex = 2;
				SdpApp.EventsManager.Raise<DataExplorerViewInvalidateEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.Invalidate, this, dataExplorerViewInvalidateEventArgs);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000AA40 File Offset: 0x00008C40
		private void dataExplorerViewEvents_SelectRow(object sender, DataExplorerViewSelectRowEventArgs args)
		{
			this.m_rowSelectedInstigator = args.Instigator;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000AA50 File Offset: 0x00008C50
		private void dataExplorerViewEvents_RowSelected(object o, DataExplorerViewRowSelectedEventArgs e)
		{
			if (e.SourceID == 2401)
			{
				int captureID = e.CaptureID;
				uint num = (uint)((int)e.SelectedRow[0]);
				if (e.NumClicks == 2)
				{
					DataExplorerViewMgr.SelectGantElement(captureID, num);
					return;
				}
				if (e.NumClicks == 1)
				{
					this.TryDisplayVkApiCallInInspectorView(captureID, num);
					return;
				}
			}
			else if (e.SourceID == 353)
			{
				this.m_selectedRow = e.SelectedRow;
				if (e.NumClicks == 2 && !QGLPlugin.WaitingForReplay)
				{
					if ((ulong)SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CaptureId != (ulong)((long)e.CaptureID))
					{
						SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
						setStatusEventArgs.Status = StatusType.Error;
						setStatusEventArgs.StatusText = string.Format("Can only replay drawcalls from latest snapshot.", Array.Empty<object>());
						setStatusEventArgs.Duration = 5000;
						SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SetStatus, null, setStatusEventArgs);
						return;
					}
					string text = (string)e.SelectedRow[1];
					string text2 = (string)e.SelectedRow[8];
					ulong num2 = VkHelper.MakeSnapshotApiCallID(text2);
					if (num2 == 18446744073709551615UL)
					{
						SetStatusEventArgs setStatusEventArgs2 = new SetStatusEventArgs();
						setStatusEventArgs2.Status = StatusType.Error;
						setStatusEventArgs2.StatusText = string.Format("Invalid API call selected. Can only playback drawcalls.", Array.Empty<object>());
						setStatusEventArgs2.Duration = 5000;
						SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SetStatus, null, setStatusEventArgs2);
						return;
					}
					int num3 = (int)((uint)e.SelectedRow[0]);
					uint num4;
					if (QGLModel.TryGetVulkanReplayID((uint)e.CaptureID, num3, out num4))
					{
						QGLPlugin.DisplayReplay((uint)e.CaptureID, num4);
					}
					else if (SdpApp.ConnectionManager.IsConnected())
					{
						num4 = QGLModel.AddReplayMapping((uint)e.CaptureID, num3);
						SetStatusEventArgs setStatusEventArgs3 = new SetStatusEventArgs();
						setStatusEventArgs3.Status = StatusType.Warning;
						setStatusEventArgs3.StatusText = string.Format("Retrieving replay for drawcall: {0}", text);
						setStatusEventArgs3.Duration = 0;
						SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SetStatus, null, setStatusEventArgs3);
						Option option = SdpApp.ConnectionManager.GetOption(this.m_replayRequestOptionID, 4294967294U);
						if (option != null)
						{
							OptionStructData optionStructData = option.GetOptionStructData();
							optionStructData.SetValue("captureID", e.CaptureID.ToString());
							optionStructData.SetValue("replayID", num4.ToString());
							optionStructData.SetValue("apicallID", num2.ToString());
							optionStructData.SetValue("shaderProfilingEnabled", e.ShaderProfilingEnabled.ToString().ToLower());
							option.SetValue(optionStructData);
						}
						ObjectWithToolTip objectWithToolTip = e.SelectedRow[9] as ObjectWithToolTip;
						bool flag;
						if (e.ShaderProfilingEnabled)
						{
							objectWithToolTip.Value = DataExplorerViewMgr.m_replayDark;
							objectWithToolTip.ToolTip = "Shader profiling replay available";
							flag = true;
						}
						else
						{
							objectWithToolTip.Value = DataExplorerViewMgr.m_replayLight;
							objectWithToolTip.ToolTip = "Replay available";
							flag = true;
						}
						if (flag)
						{
							ColumnToggledEventArgs columnToggledEventArgs = new ColumnToggledEventArgs();
							columnToggledEventArgs.Id = 1U;
							columnToggledEventArgs.Enabled = true;
							SdpApp.EventsManager.Raise<ColumnToggledEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.ToggleColumn, this, columnToggledEventArgs);
							UpdateSingleRowArgs updateSingleRowArgs = new UpdateSingleRowArgs();
							updateSingleRowArgs.Row = e.SelectedRow;
							SdpApp.EventsManager.Raise<UpdateSingleRowArgs>(SdpApp.EventsManager.DataExplorerViewEvents.UpdateSingleRow, this, updateSingleRowArgs);
						}
						QGLPlugin.WaitingForReplay = true;
					}
					SdpApp.UIManager.FocusCaptureWindow(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.WindowName, "");
				}
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000ADBB File Offset: 0x00008FBB
		private static void SelectGantElement(int captureID, uint apiID)
		{
			if (QGLModel.GanttTrackControllers.ContainsKey(captureID))
			{
				QGLModel.GanttTrackControllers[captureID].SetSelected(apiID, true);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000ADDC File Offset: 0x00008FDC
		private void TryDisplayVkApiCallInInspectorView(int captureID, uint callID)
		{
			QGLApiTracePacketGateway.GetQGLApiTracePacket(captureID, callID).Bind((IQGLApiTracePacketProxy proxy) => proxy.ToDomainObject()).Match(delegate(QGLApiTracePacket apiPacket)
			{
				this.DisplayVkApiCallInInspectorView(apiPacket);
			}, delegate(string error)
			{
				this.LogAndDisplayError(error);
			});
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000AE34 File Offset: 0x00009034
		private void DisplayVkApiCallInInspectorView(QGLApiTracePacket apiPacket)
		{
			if (this.m_rowSelectedInstigator != null && this.m_rowSelectedInstigator.Contains("Performance Hint"))
			{
				this.CheckDisplayPerfHint(apiPacket.CallID, apiPacket.CaptureID);
			}
			else if (apiPacket.FunctionName == "vkQueueSubmit")
			{
				this.DisplayVkQueueSubmitCall(apiPacket);
			}
			else
			{
				this.ClearInspectorView();
			}
			this.m_rowSelectedInstigator = null;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000AE98 File Offset: 0x00009098
		private void CheckDisplayPerfHint(uint callID, uint captureID)
		{
			List<PerfHintMgr.PerfHint> perfHintsForAPICall = PerfHintMgr.GetPerfHintsForAPICall(callID, captureID);
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			if (perfHintsForAPICall != null)
			{
				foreach (PerfHintMgr.PerfHint perfHint in perfHintsForAPICall)
				{
					string message = perfHint.GetMessage();
					SdpPropertyDescriptor<string> sdpPropertyDescriptor = new SdpPropertyDescriptor<string>("Performance Hint", typeof(string), PerfHintMgr.GetHintName(perfHint.HintID), "Performance Hints", message, true);
					list.Add(sdpPropertyDescriptor);
				}
				InspectorController.UpdateInspectorView(this, list);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000AF30 File Offset: 0x00009130
		private void DisplayVkQueueSubmitCall(QGLApiTracePacket apiPacket)
		{
			QGLApiTraceQueueSubmitTimingGateway.GetQGLApiTraceQueueSubmitTiming((int)apiPacket.CaptureID, apiPacket.CallID).Bind(delegate(IEnumerable<IQGLApiQueueSubmitTimingProxy> proxies)
			{
				IEnumerable<Result<QGLApiQueueSubmitTiming, string>> enumerable = Enumerable.Select<IQGLApiQueueSubmitTimingProxy, Result<QGLApiQueueSubmitTiming, string>>(proxies, (IQGLApiQueueSubmitTimingProxy proxy) => proxy.ToDomain());
				return enumerable.TraverseResults<QGLApiQueueSubmitTiming, string>();
			}).Match(delegate(IEnumerable<QGLApiQueueSubmitTiming> queueSubmitTimings)
			{
				IEnumerable<PropertyDescriptor> vkQueueSubmitProperties = this.GetVkQueueSubmitProperties(apiPacket, queueSubmitTimings);
				InspectorController.UpdateInspectorView(this, vkQueueSubmitProperties);
			}, delegate(string error)
			{
				this.LogAndDisplayError(error);
			});
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000AFAD File Offset: 0x000091AD
		private IEnumerable<PropertyDescriptor> GetVkQueueSubmitProperties(QGLApiTracePacket apiPacket, IEnumerable<QGLApiQueueSubmitTiming> queueSubmitTimings)
		{
			DataExplorerViewMgr.<GetVkQueueSubmitProperties>d__16 <GetVkQueueSubmitProperties>d__ = new DataExplorerViewMgr.<GetVkQueueSubmitProperties>d__16(-2);
			<GetVkQueueSubmitProperties>d__.<>3__apiPacket = apiPacket;
			<GetVkQueueSubmitProperties>d__.<>3__queueSubmitTimings = queueSubmitTimings;
			return <GetVkQueueSubmitProperties>d__;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000AFC4 File Offset: 0x000091C4
		private void LogAndDisplayError(string errorMessage)
		{
			IEnumerable<SdpPropertyDescriptor<string>> enumerable = new SdpPropertyDescriptor<string>("Data Retrieval Error", typeof(string), errorMessage, "Error", errorMessage, true).ToEnumerable<SdpPropertyDescriptor<string>>();
			InspectorController.UpdateInspectorView(this, enumerable);
			this.m_logger.LogError(errorMessage);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000B006 File Offset: 0x00009206
		private void ClearInspectorView()
		{
			InspectorController.UpdateInspectorView(this, new List<PropertyDescriptor>());
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000B014 File Offset: 0x00009214
		private void connectionEvents_OptionAdded(object sender, OptionEventArgs args)
		{
			if (args != null)
			{
				Option option = SdpApp.ConnectionManager.GetOption(args.OptionId, args.ProcessId);
				string name = option.GetName();
				if (name == SDPCore.OPT_VULKAN_REPLAY_REQUEST)
				{
					this.m_replayRequestOptionID = args.OptionId;
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000B05B File Offset: 0x0000925B
		public static Pixbuf IconNone
		{
			get
			{
				return DataExplorerViewMgr.m_replayNone;
			}
		}

		// Token: 0x040003A1 RID: 929
		private string m_rowSelectedInstigator;

		// Token: 0x040003A2 RID: 930
		private uint m_replayRequestOptionID;

		// Token: 0x040003A3 RID: 931
		private readonly ILogger m_logger;

		// Token: 0x040003A4 RID: 932
		private object[] m_selectedRow;

		// Token: 0x040003A5 RID: 933
		private List<Type> m_modelTypes = new List<Type>();

		// Token: 0x040003A6 RID: 934
		private static Pixbuf m_replayNone;

		// Token: 0x040003A7 RID: 935
		private static Pixbuf m_replayLight;

		// Token: 0x040003A8 RID: 936
		private static Pixbuf m_replayDark;
	}
}
