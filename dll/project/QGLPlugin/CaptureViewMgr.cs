using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cairo;
using Gtk;
using Sdp;
using Sdp.Charts;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Helpers;

namespace QGLPlugin
{
	// Token: 0x02000033 RID: 51
	internal class CaptureViewMgr
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00009B64 File Offset: 0x00007D64
		public void PopulateCapture(int captureID, ModelObjectDataList apis)
		{
			Thread thread = new Thread(delegate
			{
				ProgressObject progress = new ProgressObject();
				progress.Title = "Vulkan API Trace";
				progress.Description = "Processing Graphs";
				ModelObjectDataList queueSubmitTimings = QGLModel.GetQueueSubmitTimings(captureID);
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(progress));
				QGLModel.GetVulkanDebugMarkerParams(captureID);
				long minDataBounds = long.MaxValue;
				long maxDataBounds = long.MinValue;
				Series cpuSeries = new Series();
				cpuSeries.Name = "CPU: API Trace";
				Dictionary<uint, Series> dictionary = new Dictionary<uint, Series>();
				List<Connection> connections = new List<Connection>();
				Dictionary<uint, string> nameStrings = new Dictionary<uint, string>();
				Dictionary<uint, string> toolTipStrings = new Dictionary<uint, string>();
				Dictionary<uint, Color> colorsModel = new Dictionary<uint, Color>();
				Dictionary<uint, CaptureViewMgr.ConnectionCacheElement> dictionary2 = new Dictionary<uint, CaptureViewMgr.ConnectionCacheElement>();
				CaptureViewMgr.DebugMarkerViewModelBuilder debugMarkerViewModelBuilder = new CaptureViewMgr.DebugMarkerViewModelBuilder((uint)captureID, nameStrings, toolTipStrings, colorsModel);
				double num = (double)(apis.Count + queueSubmitTimings.Count);
				double num2 = 0.0;
				DateTime dateTime = DateTime.Now;
				DateTime dateTime2 = DateTime.Now;
				Series adrenoPerfHintSeries = new Series();
				adrenoPerfHintSeries.Name = "GPU: Performance Hints";
				foreach (ModelObjectData modelObjectData in apis)
				{
					uint num3 = UintConverter.Convert(modelObjectData.GetValue("m_callID"));
					string value = modelObjectData.GetValue("m_functionName");
					uint hashCode = (uint)value.GetHashCode();
					ulong num4 = Uint64Converter.Convert(modelObjectData.GetValue("m_functionCPUStartTime"));
					ulong num5 = Uint64Converter.Convert(modelObjectData.GetValue("m_functionCPUEndTime"));
					uint num6 = UintConverter.Convert(modelObjectData.GetValue("m_threadID"));
					if (VkDebugMarkers.IsDebugMarkerAPICall(value))
					{
						CaptureViewMgr.TryAddDebugMarkerCall(captureID, num3, modelObjectData, debugMarkerViewModelBuilder, value, num4, num5);
					}
					ModelObjectDataList perfHintTraceMessages = QGLModel.GetPerfHintTraceMessages(captureID, num3);
					if (perfHintTraceMessages != null && perfHintTraceMessages.Count > 0)
					{
						uint num7 = PerfHintMgr.ProcessHintMessages(ref perfHintTraceMessages, num3, (uint)captureID);
						if (num7 > 0U)
						{
							if (!toolTipStrings.ContainsKey(hashCode))
							{
								toolTipStrings.Add(hashCode, value);
							}
							Marker marker = new Marker
							{
								Color = new Color(255.0, 0.0, 0.0),
								Position = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num4),
								Style = MarkerStyle.Diamond,
								Tag = (int)num3,
								TooltipId = hashCode
							};
							adrenoPerfHintSeries.Markers.Add(marker);
						}
					}
					dictionary2.Add(num3, new CaptureViewMgr.ConnectionCacheElement
					{
						ThreadID = num6,
						StartTimestamp = num4,
						EndTimestamp = num5
					});
					if (!nameStrings.ContainsKey(hashCode))
					{
						nameStrings.Add(hashCode, value);
					}
					if (!colorsModel.ContainsKey(hashCode))
					{
						colorsModel.Add(hashCode, this.RandomColor());
					}
					Element element = new Element();
					element.BlockId = num3;
					element.Start = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num4);
					element.End = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num5);
					element.ColorId = hashCode;
					element.LabelId = hashCode;
					if (!dictionary.ContainsKey(num6))
					{
						dictionary.Add(num6, new Series());
						dictionary[num6].Name = "Thread: 0x" + num6.ToString("X");
						cpuSeries.Children.Add(dictionary[num6]);
					}
					dictionary[num6].Elements.Add(element);
					minDataBounds = Math.Min(minDataBounds, element.Start);
					maxDataBounds = Math.Max(maxDataBounds, element.End);
					dateTime2 = DateTime.Now;
					num2 += 1.0;
					if ((dateTime2 - dateTime).TotalSeconds > 1.0)
					{
						dateTime = dateTime2;
						progress.CurrentValue = num2 / num;
						SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(progress));
					}
				}
				QGLModel.ProcessingDebugMarkerData.Clear();
				PerfHintMgr.PerfHintsProcessed.Add(captureID);
				SourceEventArgs sourceEventArgs = new SourceEventArgs();
				sourceEventArgs.SourceID = 2401;
				sourceEventArgs.CaptureID = captureID;
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SourceSelected, this, sourceEventArgs);
				foreach (KeyValuePair<uint, Series> keyValuePair in dictionary)
				{
					keyValuePair.Value.Elements.Sort(new Comparison<Element>(this.compareElements));
				}
				Series queueSubmitSeries = new Series();
				queueSubmitSeries.Name = "GPU: Queue Submissions";
				foreach (ModelObjectData modelObjectData2 in queueSubmitTimings)
				{
					uint num8 = UintConverter.Convert(modelObjectData2.GetValue("m_callID"));
					uint num9 = UintConverter.Convert(modelObjectData2.GetValue("m_loggingID"));
					ulong num10 = Uint64Converter.Convert(modelObjectData2.GetValue("m_commandBuffer"));
					ulong num11 = Uint64Converter.Convert(modelObjectData2.GetValue("m_functionGPUStartTimeNS")) / 1000UL;
					ulong num12 = Uint64Converter.Convert(modelObjectData2.GetValue("m_functionGPUEndTimeNS")) / 1000UL;
					if (num11 != 0UL && num12 != 0UL)
					{
						if (num11 == num12)
						{
							num12 += 1UL;
						}
						string text = "CB = 0x" + num10.ToString("X");
						uint hashCode2 = (uint)text.GetHashCode();
						if (!nameStrings.ContainsKey(hashCode2))
						{
							nameStrings.Add(hashCode2, text);
						}
						if (!colorsModel.ContainsKey(hashCode2))
						{
							colorsModel.Add(hashCode2, this.RandomColor());
						}
						CaptureViewMgr.ConnectionCacheElement connectionCacheElement;
						if (dictionary2.TryGetValue(num8, out connectionCacheElement))
						{
							ulong startTimestamp = connectionCacheElement.StartTimestamp;
							uint threadID = connectionCacheElement.ThreadID;
							if (num11 > startTimestamp || num12 > startTimestamp)
							{
								Element element2 = new Element();
								element2.BlockId = num9;
								element2.Start = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num11);
								element2.End = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num12);
								element2.LabelId = hashCode2;
								element2.ColorId = hashCode2;
								queueSubmitSeries.Elements.Add(element2);
								minDataBounds = Math.Min(minDataBounds, element2.Start);
								maxDataBounds = Math.Max(maxDataBounds, element2.End);
								ulong endTimestamp = connectionCacheElement.EndTimestamp;
								if (dictionary.ContainsKey(threadID))
								{
									Connection connection = new Connection();
									connection.Start = dictionary[threadID];
									connection.StartPosition = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)endTimestamp);
									connection.End = queueSubmitSeries;
									connection.EndPosition = SdpApp.ModelManager.TraceModel.NormalizeTimestamp((uint)captureID, (long)num12);
									connections.Add(connection);
								}
							}
						}
						dateTime2 = DateTime.Now;
						num2 += 1.0;
						if ((dateTime2 - dateTime).TotalSeconds > 1.0)
						{
							dateTime = dateTime2;
							progress.CurrentValue = num2 / num;
							SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(progress));
						}
					}
				}
				queueSubmitSeries.Elements.Sort(new Comparison<Element>(this.compareElements));
				Application.Invoke(delegate
				{
					if (SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey(captureID))
					{
						GroupLayoutController groupLayoutController = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[captureID];
						if (groupLayoutController != null)
						{
							DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
							Model model = dataModel.GetModel("CaptureManager");
							uint num13 = 0U;
							ModelObject modelObject = dataModel.GetModelObject(model, "Capture");
							ModelObjectDataList modelObjectData3 = dataModel.GetModelObjectData(modelObject, "captureID", captureID.ToString());
							if (modelObjectData3.Count != 0)
							{
								ModelObjectData modelObjectData4 = modelObjectData3[0];
								num13 = UintConverter.Convert(modelObjectData4.GetValue("processID"));
							}
							AddGroupCommand addGroupCommand = new AddGroupCommand();
							addGroupCommand.Container = groupLayoutController;
							addGroupCommand.GroupName = string.Format("Vulkan API Trace | Process: {0}", num13);
							SdpApp.ExecuteCommand(addGroupCommand);
							GroupController result = addGroupCommand.Result;
							if (result != null)
							{
								AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
								addTrackToGroupCommand.Container = result;
								addTrackToGroupCommand.MetricPlugin = null;
								addTrackToGroupCommand.TrackType = TrackType.Gantt;
								SdpApp.ExecuteCommand(addTrackToGroupCommand);
								GanttTrackController ganttTrackController = addTrackToGroupCommand.Result as GanttTrackController;
								if (ganttTrackController != null)
								{
									if (QGLModel.GanttTrackControllers.ContainsKey(captureID))
									{
										QGLModel.GanttTrackControllers.Remove(captureID);
									}
									QGLModel.GanttTrackControllers.Add(captureID, ganttTrackController);
									GanttTrackController ganttTrackController2 = ganttTrackController;
									ganttTrackController2.ElementSelected = (EventHandler<ElementSelectedEventArgs>)Delegate.Combine(ganttTrackController2.ElementSelected, new EventHandler<ElementSelectedEventArgs>(this.track_ElementSelected));
									GanttTrackController ganttTrackController3 = ganttTrackController;
									ganttTrackController3.MarkerSelected = (EventHandler<MarkerSelectedEventArgs>)Delegate.Combine(ganttTrackController3.MarkerSelected, new EventHandler<MarkerSelectedEventArgs>(this.track_MarkerSelected));
									ganttTrackController.Series.Add(cpuSeries);
									if (queueSubmitSeries.Elements.Count > 0)
									{
										ganttTrackController.Series.Add(queueSubmitSeries);
									}
									foreach (Connection connection2 in connections)
									{
										ganttTrackController.AddConnection(connection2);
									}
									if (adrenoPerfHintSeries.Markers.Count > 0)
									{
										ganttTrackController.Series.Add(adrenoPerfHintSeries);
									}
									if (debugMarkerViewModelBuilder.ViewModel.DebugMarkerRegions.Children.Count >= 1)
									{
										debugMarkerViewModelBuilder.ViewModel.DebugMarkerRegions.Children = Enumerable.ToList<Series>(Enumerable.OrderBy<Series, int>(debugMarkerViewModelBuilder.ViewModel.DebugMarkerRegions.Children, (Series q) => int.Parse(q.Name)));
										ganttTrackController.Series.Add(debugMarkerViewModelBuilder.ViewModel.DebugMarkerRegions);
									}
									if (debugMarkerViewModelBuilder.ViewModel.DebugMarkers.Markers.Count >= 1)
									{
										ganttTrackController.Series.Add(debugMarkerViewModelBuilder.ViewModel.DebugMarkers);
									}
									ganttTrackController.NameStringsModel = nameStrings;
									ganttTrackController.StringIDsToRender = new List<uint>(nameStrings.Keys);
									ganttTrackController.SetColorsModel(colorsModel);
									ganttTrackController.TooltipStringsModel = toolTipStrings;
									ganttTrackController.SetDataBounds(minDataBounds, maxDataBounds);
									ganttTrackController.Invalidate();
								}
							}
						}
					}
					SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(progress));
				});
			});
			thread.Start();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009BA4 File Offset: 0x00007DA4
		private static void TryAddDebugMarkerCall(int captureID, uint callID, ModelObjectData data, CaptureViewMgr.DebugMarkerViewModelBuilder debugMarkerViewModelBuilder, string apiName, ulong apiCPUStartTime, ulong apiCPUEndTime)
		{
			string value = data.GetValue("m_functionParams");
			debugMarkerViewModelBuilder.AddDebugMarkerAPICall(captureID, callID, apiName, value, (long)apiCPUStartTime, (long)apiCPUEndTime);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009BCC File Offset: 0x00007DCC
		private int compareElements(Element e1, Element e2)
		{
			int num = e1.Start.CompareTo(e2.Start);
			if (num == 0)
			{
				return e2.End.CompareTo(e1.End);
			}
			return num;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009C04 File Offset: 0x00007E04
		private Color RandomColor()
		{
			return new Color(this.m_randomGen.NextDouble() * 0.85, this.m_randomGen.NextDouble() * 0.85, this.m_randomGen.NextDouble() * 0.85);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00009C58 File Offset: 0x00007E58
		private void track_ElementSelected(object o, ElementSelectedEventArgs e)
		{
			int? captureIDForTrack = CaptureViewMgr.GetCaptureIDForTrack(o as GanttTrackController);
			if (!CaptureViewMgr.ValidateElementSelectedParams(captureIDForTrack))
			{
				return;
			}
			this.SelectAPICallInDataExplorer(captureIDForTrack.Value, (int)e.Selected.BlockId, e.ElementSeries.Name);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009C9D File Offset: 0x00007E9D
		private static bool ValidateElementSelectedParams(int? captureID)
		{
			if (captureID == null)
			{
				Logger.Get().Write(LogLevel.LOG_WARN, "CaptureViewMgr", "Unable to find captureID for selected track marker");
				return false;
			}
			return true;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009CC0 File Offset: 0x00007EC0
		private void track_MarkerSelected(object o, MarkerSelectedEventArgs e)
		{
			int? captureIDForTrack = CaptureViewMgr.GetCaptureIDForTrack(o as GanttTrackController);
			if (!CaptureViewMgr.ValidateMarkerSelectedParams(e, captureIDForTrack))
			{
				return;
			}
			this.SelectAPICallInDataExplorer(captureIDForTrack.Value, (int)e.Selected.Tag, e.ElementSeries.Name);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009D0C File Offset: 0x00007F0C
		private static int? GetCaptureIDForTrack(GanttTrackController track)
		{
			return Enumerable.FirstOrDefault<int?>(Enumerable.Select<KeyValuePair<int, GanttTrackController>, int?>(Enumerable.Where<KeyValuePair<int, GanttTrackController>>(QGLModel.GanttTrackControllers, (KeyValuePair<int, GanttTrackController> kvp) => kvp.Value == track), (KeyValuePair<int, GanttTrackController> kvp) => new int?(kvp.Key)));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009D68 File Offset: 0x00007F68
		private void SelectAPICallInDataExplorer(int captureID, int apiID, string instigator)
		{
			DataExplorerViewSelectRowEventArgs dataExplorerViewSelectRowEventArgs = new DataExplorerViewSelectRowEventArgs();
			dataExplorerViewSelectRowEventArgs.CaptureID = captureID;
			dataExplorerViewSelectRowEventArgs.SourceID = 2401;
			dataExplorerViewSelectRowEventArgs.RowElement = apiID;
			dataExplorerViewSelectRowEventArgs.SearchColumn = 0;
			dataExplorerViewSelectRowEventArgs.Instigator = instigator;
			SdpApp.EventsManager.Raise<DataExplorerViewSelectRowEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectRow, this, dataExplorerViewSelectRowEventArgs);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009DC4 File Offset: 0x00007FC4
		private static bool ValidateMarkerSelectedParams(MarkerSelectedEventArgs e, int? captureID)
		{
			if (captureID == null)
			{
				Logger.Get().Write(LogLevel.LOG_WARN, "CaptureViewMgr", "Unable to find captureID for selected track marker");
				return false;
			}
			if (!(e.Selected.Tag is int))
			{
				Logger.Get().Write(LogLevel.LOG_WARN, "CaptureViewMgr", "Unexpected marker log tag");
				return false;
			}
			return true;
		}

		// Token: 0x040003A0 RID: 928
		private readonly Random m_randomGen = new Random();

		// Token: 0x02000062 RID: 98
		private class ConnectionCacheElement
		{
			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060001B1 RID: 433 RVA: 0x000144A0 File Offset: 0x000126A0
			// (set) Token: 0x060001B2 RID: 434 RVA: 0x000144A8 File Offset: 0x000126A8
			public uint ThreadID { get; set; }

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060001B3 RID: 435 RVA: 0x000144B1 File Offset: 0x000126B1
			// (set) Token: 0x060001B4 RID: 436 RVA: 0x000144B9 File Offset: 0x000126B9
			public ulong StartTimestamp { get; set; }

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060001B5 RID: 437 RVA: 0x000144C2 File Offset: 0x000126C2
			// (set) Token: 0x060001B6 RID: 438 RVA: 0x000144CA File Offset: 0x000126CA
			public ulong EndTimestamp { get; set; }
		}

		// Token: 0x02000063 RID: 99
		private class DebugMarkerViewModel
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060001B8 RID: 440 RVA: 0x000144D3 File Offset: 0x000126D3
			public Series DebugMarkerRegions { get; } = new Series
			{
				Name = "Debug Marker Regions"
			};

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001B9 RID: 441 RVA: 0x000144DB File Offset: 0x000126DB
			public Series DebugMarkers { get; } = new Series
			{
				Name = "Debug Markers"
			};
		}

		// Token: 0x02000064 RID: 100
		private class DebugMarkerViewModelBuilder
		{
			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060001BB RID: 443 RVA: 0x00014517 File Offset: 0x00012717
			// (set) Token: 0x060001BC RID: 444 RVA: 0x0001451F File Offset: 0x0001271F
			public CaptureViewMgr.DebugMarkerViewModel ViewModel { get; private set; } = new CaptureViewMgr.DebugMarkerViewModel();

			// Token: 0x060001BD RID: 445 RVA: 0x00014528 File Offset: 0x00012728
			public DebugMarkerViewModelBuilder(uint captureID, Dictionary<uint, string> nameStrings, Dictionary<uint, string> tooltips, Dictionary<uint, Color> colorModel)
			{
				this.m_captureID = captureID;
				this.m_nameStrings = nameStrings;
				this.m_toolTips = tooltips;
				this.m_colorModel = colorModel;
			}

			// Token: 0x060001BE RID: 446 RVA: 0x0001457C File Offset: 0x0001277C
			public void AddDebugMarkerAPICall(int captureID, uint callID, string apiName, string functionParams, long cpuStartTime, long cpuEndTime)
			{
				if (!VkDebugMarkers.IsDebugMarkerAPICall(apiName))
				{
					throw new ArgumentException("Adding a non-debug marker call to debug marker view model builder");
				}
				if (apiName == "vkCmdDebugMarkerBeginEXT")
				{
					this.AddDebugMarkerBegin(callID, functionParams, cpuStartTime);
					return;
				}
				if (apiName == "vkCmdDebugMarkerEndEXT")
				{
					this.AddDebugMarkerEnd(cpuEndTime);
					return;
				}
				if (apiName == "vkCmdDebugMarkerInsertEXT")
				{
					this.AddDebugMarker(callID, functionParams, cpuStartTime);
				}
			}

			// Token: 0x060001BF RID: 447 RVA: 0x000145E4 File Offset: 0x000127E4
			private void AddDebugMarkerBegin(uint callID, string functionParams, long cpuStartTime)
			{
				ulong num = VkDebugMarkers.ParseCommandBufferParam(functionParams);
				int num2 = Enumerable.Count<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion>(this.m_activeRegions);
				this.m_activeRegions.Push(new CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion
				{
					BeginCallID = callID,
					CPUBeginTime = cpuStartTime,
					MarkerInfo = VkDebugMarkers.GetDebugMarkerInfo(this.m_captureID, callID),
					DepthLevel = num2
				});
				CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion debugMarkerRegion = this.m_activeRegions.Peek();
			}

			// Token: 0x060001C0 RID: 448 RVA: 0x00014647 File Offset: 0x00012847
			private void AddDebugMarkerEnd(long cpuEndTime)
			{
				this.EndMarkerRegion(cpuEndTime).IfSome(delegate(CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion debugMarker)
				{
					Element element = this.CreateRegionElement(debugMarker);
					this.AddRegionElement(element, debugMarker.DepthLevel);
				});
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x00014664 File Offset: 0x00012864
			private Maybe<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion> EndMarkerRegion(long cpuEndTime)
			{
				if (this.m_activeRegions.Count > 0)
				{
					CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion debugMarkerRegion = this.m_activeRegions.Pop();
					debugMarkerRegion.CPUEndTime = new long?(cpuEndTime);
					return new Maybe<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion>.Some(debugMarkerRegion);
				}
				Logger.Get().Write(LogLevel.LOG_WARN, "CaptureViewMgr", "Ending debug marker region received without corresponding Begin marker within current trace. Please check marker usage to ensure correct intended behavior.");
				return new Maybe<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion>.None();
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x000146B8 File Offset: 0x000128B8
			private Element CreateRegionElement(CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion region)
			{
				if (region.CPUEndTime == null)
				{
					throw new ArgumentException("Debug marker region has no end time set");
				}
				uint markerNameHash = this.GetMarkerNameHash(region.MarkerInfo.Name);
				uint markerColorHash = this.GetMarkerColorHash(region.MarkerInfo.Color);
				return new Element
				{
					BlockId = region.BeginCallID,
					Start = SdpApp.ModelManager.TraceModel.NormalizeTimestamp(this.m_captureID, region.CPUBeginTime),
					End = SdpApp.ModelManager.TraceModel.NormalizeTimestamp(this.m_captureID, region.CPUEndTime.Value),
					ColorId = markerColorHash,
					LabelId = markerNameHash
				};
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x0001476C File Offset: 0x0001296C
			private uint GetMarkerColorHash(Color color)
			{
				uint hashCode = (uint)color.GetHashCode();
				if (!this.m_colorModel.ContainsKey(hashCode))
				{
					this.m_colorModel.Add(hashCode, color);
				}
				return hashCode;
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x000147A4 File Offset: 0x000129A4
			private uint GetMarkerNameHash(string markerName)
			{
				uint hashCode = (uint)markerName.GetHashCode();
				if (!this.m_nameStrings.ContainsKey(hashCode))
				{
					this.m_nameStrings.Add(hashCode, markerName);
				}
				return hashCode;
			}

			// Token: 0x060001C5 RID: 453 RVA: 0x000147D4 File Offset: 0x000129D4
			private void AddRegionElement(Element element, int depthLevel)
			{
				Series markerRegionSeries = this.GetMarkerRegionSeries(depthLevel);
				string text = this.m_nameStrings[element.LabelId];
				markerRegionSeries.Elements.Add(element);
			}

			// Token: 0x060001C6 RID: 454 RVA: 0x00014808 File Offset: 0x00012A08
			private Series GetMarkerRegionSeries(int depthLevel)
			{
				Series series = new Series
				{
					IsExpanded = true
				};
				if (!this.m_depthMarkerRegions.TryGetValue(depthLevel, out series))
				{
					series = this.AddNewDepthRegionSeries(depthLevel);
				}
				return series;
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x0001483C File Offset: 0x00012A3C
			private Series AddNewDepthRegionSeries(int depth)
			{
				Series series = new Series
				{
					IsExpanded = false
				};
				this.m_depthMarkerRegions.Add(depth, series);
				series.Name = depth.ToString();
				this.ViewModel.DebugMarkerRegions.Children.Add(series);
				return series;
			}

			// Token: 0x060001C8 RID: 456 RVA: 0x00014888 File Offset: 0x00012A88
			private void AddDebugMarker(uint callID, string functionParams, long cpuStartTime)
			{
				DebugMarkerInfo debugMarkerInfo = VkDebugMarkers.GetDebugMarkerInfo(this.m_captureID, callID);
				uint markerToolTipHash = this.GetMarkerToolTipHash(debugMarkerInfo.Name);
				Marker marker = new Marker
				{
					Color = debugMarkerInfo.Color,
					Position = SdpApp.ModelManager.TraceModel.NormalizeTimestamp(this.m_captureID, cpuStartTime),
					Style = MarkerStyle.Diamond,
					Tag = (int)callID,
					TooltipId = markerToolTipHash
				};
				this.ViewModel.DebugMarkers.Markers.Add(marker);
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x00014910 File Offset: 0x00012B10
			private uint GetMarkerToolTipHash(string toolTip)
			{
				uint hashCode = (uint)toolTip.GetHashCode();
				if (!this.m_toolTips.ContainsKey(hashCode))
				{
					this.m_toolTips.Add(hashCode, toolTip);
				}
				return hashCode;
			}

			// Token: 0x0400049B RID: 1179
			private readonly Dictionary<int, Series> m_depthMarkerRegions = new Dictionary<int, Series>();

			// Token: 0x0400049C RID: 1180
			private readonly Stack<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion> m_activeRegions = new Stack<CaptureViewMgr.DebugMarkerViewModelBuilder.DebugMarkerRegion>();

			// Token: 0x0400049D RID: 1181
			private readonly Dictionary<uint, string> m_nameStrings;

			// Token: 0x0400049E RID: 1182
			private readonly Dictionary<uint, string> m_toolTips;

			// Token: 0x0400049F RID: 1183
			private readonly Dictionary<uint, Color> m_colorModel;

			// Token: 0x040004A0 RID: 1184
			private readonly uint m_captureID;

			// Token: 0x02000082 RID: 130
			private class DebugMarkerRegion
			{
				// Token: 0x04000570 RID: 1392
				public uint BeginCallID;

				// Token: 0x04000571 RID: 1393
				public long CPUBeginTime;

				// Token: 0x04000572 RID: 1394
				public long? CPUEndTime;

				// Token: 0x04000573 RID: 1395
				public int DepthLevel;

				// Token: 0x04000574 RID: 1396
				public DebugMarkerInfo MarkerInfo;
			}
		}
	}
}
