using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cairo;
using Sdp.Commands;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x020002A7 RID: 679
	public class GraphTrackController : TrackControllerBase
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x00026D28 File Offset: 0x00024F28
		public void SetDrawMode(GraphTrackController.DrawMode drawMode)
		{
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			graphTrackView.SetDrawMode(drawMode);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00026D48 File Offset: 0x00024F48
		public void SetBGColor(Color color)
		{
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			graphTrackView.SetBGColor(color);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00026D68 File Offset: 0x00024F68
		public void ResetBGColor()
		{
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			graphTrackView.ResetBGColor();
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00026D88 File Offset: 0x00024F88
		public override void Dispose()
		{
			base.Dispose();
			TimeEvents timeEvents = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_layoutContainer.CaptureId);
			if (timeEvents != null)
			{
				TimeEvents timeEvents2 = timeEvents;
				timeEvents2.DataViewBoundsChanged = (EventHandler)Delegate.Remove(timeEvents2.DataViewBoundsChanged, new EventHandler(this.timeEvents_DataViewBoundsChanged));
			}
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataReceived = (EventHandler<DataReceivedEventArgs>)Delegate.Remove(connectionEvents.DataReceived, new EventHandler<DataReceivedEventArgs>(this.connectionEvents_DataReceived));
			foreach (GraphTrackMetric graphTrackMetric in this.Metrics)
			{
				this.m_layoutContainer.RemoveMetric(graphTrackMetric.Descriptor);
				EnableMetricEventArgs enableMetricEventArgs = new EnableMetricEventArgs();
				enableMetricEventArgs.CaptureId = base.CaptureId;
				enableMetricEventArgs.Enable = false;
				enableMetricEventArgs.PID = graphTrackMetric.ProcessId;
				enableMetricEventArgs.MetricId = graphTrackMetric.MetricId;
				SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00026EA4 File Offset: 0x000250A4
		public GraphTrackController(ITrackViewBase trackView, GroupLayoutController layoutContainer, GroupController groupContainer)
			: base(trackView, layoutContainer, groupContainer)
		{
			this.Metrics = new GraphTrackMetricList();
			this.m_view.DragDataEntered += this.m_view_OnDragDataEntered;
			this.m_view.DragDataLeft += this.m_view_OnDragDataLeft;
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			graphTrackView.MetricRemoveRequest += this.viewMetricRemoveRequest;
			graphTrackView.MetricDragBegin += this.viewMetricDragBegin;
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null && timeModel.IsValid)
			{
				graphTrackView.SetDataViewBounds(timeModel.DataViewBoundsMin, timeModel.DataViewBoundsMax);
			}
			TimeEvents timeEvents = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_layoutContainer.CaptureId);
			if (timeEvents != null)
			{
				TimeEvents timeEvents2 = timeEvents;
				timeEvents2.DataViewBoundsChanged = (EventHandler)Delegate.Combine(timeEvents2.DataViewBoundsChanged, new EventHandler(this.timeEvents_DataViewBoundsChanged));
			}
			graphTrackView.DataViewBoundsChanged += this.view_ViewBoundsChanged;
			graphTrackView.DataBoundsChanged += this.view_DataBoundsChanged;
			TimeEventsCollection timeEventsCollection = SdpApp.EventsManager.TimeEventsCollection;
			timeEventsCollection.AutoScaleToggled = (EventHandler<AutoScaleEventArgs>)Delegate.Combine(timeEventsCollection.AutoScaleToggled, new EventHandler<AutoScaleEventArgs>(this.timeEvents_AutoScaleToggled));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataReceived = (EventHandler<DataReceivedEventArgs>)Delegate.Combine(connectionEvents.DataReceived, new EventHandler<DataReceivedEventArgs>(this.connectionEvents_DataReceived));
			this.SyncPanningToInteractionMode();
			layoutContainer.View.InteractionModeUpdated += delegate(object s, EventArgs e)
			{
				this.SyncPanningToInteractionMode();
			};
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0002703D File Offset: 0x0002523D
		private new IGraphTrackView View
		{
			get
			{
				return this.m_view as IGraphTrackView;
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0002704A File Offset: 0x0002524A
		private void SyncPanningToInteractionMode()
		{
			this.m_layoutContainer.View.InteractionMode.Match<bool>((InteractionMode someMode) => this.View.PanningEnabled = someMode == InteractionMode.Pan, () => this.View.PanningEnabled = true);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0002707C File Offset: 0x0002527C
		public override async void AddMetric(uint metricId, string metricName, uint processId, bool isPreview, string tooltip, bool isCustom, Color? color)
		{
			if (metricId == 0U)
			{
				MetricDesc metricDesc = MetricDesc.CreateTransientMetricDesc(metricName, tooltip, processId);
				await this.AddMetric(metricDesc);
			}
			else if (isCustom)
			{
				MetricDesc metricDesc2 = MetricDesc.CreateCustomMetricDesc(metricName, tooltip, metricId);
				metricDesc2.ColorOverride = color;
				await this.AddMetric(metricDesc2);
			}
			else
			{
				await this.AddMetric(MetricDesc.CreateMetricDesc(metricId, processId, isPreview));
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000270EF File Offset: 0x000252EF
		public override bool ContainsMetric(MetricDesc desc)
		{
			return this.Metrics.ContainsMetric(desc);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00027100 File Offset: 0x00025300
		private async Task AddMetric(MetricDesc metricDesc)
		{
			GraphTrackMetric graphTrackMetric = null;
			if (!this.Metrics.ContainsMetric(metricDesc))
			{
				if (metricDesc.MetricType == MetricType.Live)
				{
					GraphTrackMetric graphTrackMetric2 = await this.RemoveFromTrack(metricDesc);
					graphTrackMetric = graphTrackMetric2;
				}
				Color color;
				if (metricDesc.ColorOverride != null)
				{
					color = metricDesc.ColorOverride.Value;
				}
				else if (metricDesc.IsPreview)
				{
					color = new Color(0.35, 0.35, 0.35, 1.0);
				}
				else if (this.Metrics.Count == 0)
				{
					color = this.m_view.ControlPanelHeaderBackColor;
				}
				else
				{
					List<Color> defaultColors = GraphTrackController.m_defaultColors;
					int num = this.m_colorIndex;
					this.m_colorIndex = num + 1;
					color = defaultColors[num % GraphTrackController.m_defaultColors.Count];
					if (Math.Abs(color.R - this.m_view.ControlPanelHeaderBackColor.R) < 0.01 && Math.Abs(color.G - this.m_view.ControlPanelHeaderBackColor.G) < 0.01 && Math.Abs(color.B - this.m_view.ControlPanelHeaderBackColor.B) < 0.01)
					{
						List<Color> defaultColors2 = GraphTrackController.m_defaultColors;
						num = this.m_colorIndex;
						this.m_colorIndex = num + 1;
						color = defaultColors2[num % GraphTrackController.m_defaultColors.Count];
					}
				}
				GraphTrackMetricDesc graphTrackMetricDesc = new GraphTrackMetricDesc();
				graphTrackMetricDesc.Metric = metricDesc;
				graphTrackMetricDesc.Color = color;
				graphTrackMetricDesc.CaptureId = this.m_layoutContainer.CaptureId;
				if (graphTrackMetric != null)
				{
					graphTrackMetric.Color = color;
				}
				this.AddMetric(graphTrackMetricDesc, graphTrackMetric);
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0002714B File Offset: 0x0002534B
		private void AddMetric(GraphTrackMetricDesc desc)
		{
			this.AddMetric(desc, null);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00027158 File Offset: 0x00025358
		private void AddMetric(GraphTrackMetricDesc desc, GraphTrackMetric trackMetric)
		{
			bool flag = !this.Metrics.ContainsMetric(desc.Metric);
			if (flag)
			{
				if (trackMetric == null)
				{
					trackMetric = new GraphTrackMetric(desc);
				}
				this.Metrics.Add(trackMetric);
				IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
				graphTrackView.AddMetric(trackMetric);
				if (!desc.Metric.IsPreview && trackMetric.MetricId != 0U)
				{
					ChangeMetricDataSourcesColorArgs changeMetricDataSourcesColorArgs = new ChangeMetricDataSourcesColorArgs();
					changeMetricDataSourcesColorArgs.CaptureId = trackMetric.CaptureId;
					changeMetricDataSourcesColorArgs.MetricId = trackMetric.MetricId;
					changeMetricDataSourcesColorArgs.ProcessId = trackMetric.ProcessId;
					changeMetricDataSourcesColorArgs.color[0] = trackMetric.Color.R;
					changeMetricDataSourcesColorArgs.color[1] = trackMetric.Color.G;
					changeMetricDataSourcesColorArgs.color[2] = trackMetric.Color.B;
					SdpApp.EventsManager.Raise<ChangeMetricDataSourcesColorArgs>(SdpApp.EventsManager.ConnectionEvents.ChangeMetricDataSourcesColor, this, changeMetricDataSourcesColorArgs);
				}
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00027254 File Offset: 0x00025454
		public void AddTransientMetricData(string metricName, DataPointList listOfData)
		{
			MetricDesc metricDesc = MetricDesc.CreateTransientMetricDesc(metricName, null, 0U);
			GraphTrackMetric graphTrackMetric = this.Metrics.FindMetric(metricDesc);
			if (graphTrackMetric != null && listOfData != null)
			{
				graphTrackMetric.SetData(listOfData);
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00027284 File Offset: 0x00025484
		public void AddTransientMetricData(string metricName, double timestamp, double dataValue)
		{
			MetricDesc metricDesc = MetricDesc.CreateTransientMetricDesc(metricName, null, 0U);
			GraphTrackMetric graphTrackMetric = this.Metrics.FindMetric(metricDesc);
			if (graphTrackMetric != null)
			{
				graphTrackMetric.AddData(timestamp, dataValue);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000272B8 File Offset: 0x000254B8
		public void AddCustomMetricData(string metricName, uint id, double timestamp, double dataValue)
		{
			MetricDesc metricDesc = MetricDesc.CreateCustomMetricDesc(metricName, "", id);
			GraphTrackMetric graphTrackMetric = this.Metrics.FindMetric(metricDesc);
			if (graphTrackMetric != null)
			{
				graphTrackMetric.AddData(timestamp, dataValue);
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000272F0 File Offset: 0x000254F0
		public override async void RemoveMetric(uint metricId, string metricName, uint processId, bool forceDeleteTrackIfEmpty, bool isPreview)
		{
			if (metricId == 0U)
			{
				MetricDesc metricDesc = MetricDesc.CreateTransientMetricDesc(metricName, null, 0U);
				metricDesc.IsPreview = isPreview;
				await this.RemoveMetric(metricDesc, forceDeleteTrackIfEmpty);
			}
			else
			{
				MetricDesc metricDesc2 = MetricDesc.CreateMetricDesc(metricId, processId);
				metricDesc2.IsPreview = isPreview;
				await this.RemoveMetric(metricDesc2, forceDeleteTrackIfEmpty);
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00027354 File Offset: 0x00025554
		public async Task<GraphTrackMetric> RemoveMetricAndReturnSeries(uint metricId, uint processId, string metricName, MetricType metricType)
		{
			GraphTrackMetric graphTrackMetric2;
			if (metricType == MetricType.Transient)
			{
				MetricDesc metricDesc = MetricDesc.CreateTransientMetricDesc(metricName, null, 0U);
				metricDesc.IsPreview = false;
				GraphTrackMetric graphTrackMetric = await this.RemoveMetric(metricDesc, true, false);
				graphTrackMetric2 = graphTrackMetric;
			}
			else if (metricType == MetricType.Live)
			{
				MetricDesc metricDesc2 = MetricDesc.CreateMetricDesc(metricId, processId);
				metricDesc2.IsPreview = false;
				graphTrackMetric2 = await this.RemoveMetric(metricDesc2, true, false);
			}
			else
			{
				MetricDesc metricDesc3 = MetricDesc.CreateCustomMetricDesc(metricName, "", metricId);
				metricDesc3.IsPreview = false;
				graphTrackMetric2 = await this.RemoveMetric(metricDesc3, true, false);
			}
			return graphTrackMetric2;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000273B8 File Offset: 0x000255B8
		private async Task<GraphTrackMetric> RemoveMetric(MetricDesc desc, bool forceDeleteTrackIfEmpty)
		{
			return await this.RemoveMetric(desc, false, forceDeleteTrackIfEmpty);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0002740C File Offset: 0x0002560C
		private async Task<GraphTrackMetric> RemoveMetric(MetricDesc desc, bool moving, bool forceDeleteTrackIfEmpty)
		{
			GraphTrackMetric metricToRemove = this.Metrics.FindMetric(desc);
			if (metricToRemove == null)
			{
				metricToRemove = this.Metrics.FindMetricByID(desc.MetricId);
			}
			if (metricToRemove != null)
			{
				this.Metrics.Remove(metricToRemove);
				IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
				graphTrackView.RemoveMetric(metricToRemove);
				if (!moving)
				{
					this.m_layoutContainer.RemoveMetric(desc);
				}
				if (this.Metrics.Count == 0)
				{
					bool flag2;
					bool flag3;
					if (!forceDeleteTrackIfEmpty && !desc.IsPreview)
					{
						string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ShowDeleteEmptyGraphTracksDialog);
						bool flag = text == null || BoolConverter.Convert(text);
						if (flag)
						{
							DeleteEmptyTracksDialogResult deleteEmptyTracksDialogResult = await this.m_view.ShowDeleteEmptyTracksDialog();
							DeleteEmptyTracksDialogResult deleteEmptyTracksDialogResult2 = deleteEmptyTracksDialogResult;
							flag2 = deleteEmptyTracksDialogResult2.YesSelected;
							flag3 = deleteEmptyTracksDialogResult2.RetainSettings;
						}
						else
						{
							text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteEmptyGraphTracks);
							flag2 = text == null || BoolConverter.Convert(text);
							flag3 = false;
						}
					}
					else
					{
						flag2 = forceDeleteTrackIfEmpty;
						flag3 = false;
					}
					if (flag2)
					{
						RemoveTrackFromGroupCommand removeTrackFromGroupCommand = new RemoveTrackFromGroupCommand();
						removeTrackFromGroupCommand.Track = this;
						removeTrackFromGroupCommand.Container = this.m_groupContainer;
						SdpApp.CommandManager.ExecuteCommand(removeTrackFromGroupCommand);
					}
					if (flag3)
					{
						SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.DeleteEmptyGraphTracks, flag2.ToString());
						SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.ShowDeleteEmptyGraphTracksDialog, "False");
					}
				}
			}
			return metricToRemove;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00027467 File Offset: 0x00025667
		public override int MetricCount()
		{
			return this.Metrics.Count;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00027474 File Offset: 0x00025674
		private void viewMetricDragBegin(object sender, MetricBeginDragArgs e)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				SdpApp.ModelManager.PreviewMetricsModel.Metrics.Clear();
				foreach (uint num in e.ProcessIDs)
				{
					if (e.MetricID != 0U)
					{
						SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(e.MetricID, num));
					}
					else
					{
						List<uint> metricsByCategory = SdpApp.ConnectionManager.GetMetricsByCategory(e.CategoryID);
						foreach (uint num2 in metricsByCategory)
						{
							SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(num2, num));
						}
					}
				}
				if (this.Metrics.Count > 1)
				{
					SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType = PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_MULTI;
				}
				else
				{
					SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType = PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_SINGLE;
				}
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000275F4 File Offset: 0x000257F4
		private void viewMetricRemoveRequest(object sender, MetricRemoveRequestEventArgs e)
		{
			RemoveMetricFromTrackCommand removeMetricFromTrackCommand = new RemoveMetricFromTrackCommand();
			removeMetricFromTrackCommand.Track = this;
			removeMetricFromTrackCommand.PID = e.PID;
			removeMetricFromTrackCommand.MetricId = e.MetricId;
			removeMetricFromTrackCommand.ForceDeleteTrackIfEmpty = false;
			removeMetricFromTrackCommand.IsPreview = false;
			SdpApp.CommandManager.ExecuteCommand(removeMetricFromTrackCommand);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00027640 File Offset: 0x00025840
		public void SetDataBounds(long min, long max)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null)
			{
				timeModel.SetDataBounds(min, max, true);
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00027674 File Offset: 0x00025874
		private void view_ViewBoundsChanged(object sender, SetDataViewBoundsEventArgs e)
		{
			SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
			setDataViewBoundsCommand.Minimum = e.min;
			setDataViewBoundsCommand.Maximum = e.max;
			setDataViewBoundsCommand.Dirty = e.dirty;
			setDataViewBoundsCommand.ForceScrollOff = e.ForceScrollOff;
			setDataViewBoundsCommand.CaptureId = this.m_layoutContainer.CaptureId;
			SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000276D4 File Offset: 0x000258D4
		private void view_DataBoundsChanged(object sender, SetDataBoundsEventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null)
			{
				timeModel.SetDataBounds(e.min, e.max, e.ResetDataViewBounds);
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00027718 File Offset: 0x00025918
		private bool IsPreviewTrack()
		{
			foreach (GraphTrackMetric graphTrackMetric in this.Metrics)
			{
				if (!graphTrackMetric.Descriptor.IsPreview)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0002777C File Offset: 0x0002597C
		private void m_view_OnDragDataEntered(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer = PreviewMetricsModel.ContainerType.TRACK;
				SdpApp.ModelManager.PreviewMetricsModel.Track = this;
				if (!this.IsPreviewTrack())
				{
					foreach (PreviewMetricsModel.MetricPair metricPair in SdpApp.ModelManager.PreviewMetricsModel.Metrics)
					{
						MetricDesc metricDesc = MetricDesc.CreateMetricDesc(metricPair.MetricID, metricPair.ProcessID, true);
						MetricDesc metricDesc2 = metricDesc;
						metricDesc2.IsPreview = false;
						bool flag2 = this.ContainsMetric(metricDesc);
						bool flag3 = this.ContainsMetric(metricDesc2);
						if (!flag2 && !flag3)
						{
							SdpApp.ExecuteCommand(new AddMetricToFlowCommand
							{
								MetricId = metricPair.MetricID,
								PID = metricPair.ProcessID,
								TrackController = this,
								TrackType = TrackType.Graph,
								IsPreview = true,
								IsGlobal = (metricPair.ProcessID == uint.MaxValue),
								Container = this.m_layoutContainer
							});
						}
						if (flag3)
						{
							SdpApp.ExecuteCommand(new RemoveMetricFromTrackCommand
							{
								MetricId = metricDesc.MetricId,
								PID = metricDesc.ProcessId,
								IsPreview = true,
								Track = this.m_layoutContainer.GetMetricTrack(metricDesc),
								ForceDeleteTrackIfEmpty = true
							});
						}
					}
				}
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00027938 File Offset: 0x00025B38
		private void m_view_OnDragDataLeft(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				if ((SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.TRACK && SdpApp.ModelManager.PreviewMetricsModel.Track == this) || SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.NONE)
				{
					Thread thread = new Thread(new ThreadStart(this.RemovePreviewMetrics));
					thread.Start();
				}
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000279CC File Offset: 0x00025BCC
		private void timeEvents_AutoScaleToggled(object sender, AutoScaleEventArgs args)
		{
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			graphTrackView.FixYAxis = args.Fixed;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000279F4 File Offset: 0x00025BF4
		private void RemovePreviewMetrics()
		{
			Thread.Sleep(1000);
			new RemovePreviewMetricsCommand
			{
				Controller = this.m_layoutContainer
			}.Execute();
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00027A24 File Offset: 0x00025C24
		private async Task<GraphTrackMetric> RemoveFromTrack(MetricDesc desc)
		{
			TaskCompletionSource<GraphTrackMetric> resultPromise = new TaskCompletionSource<GraphTrackMetric>();
			GraphTrackController graphTrackController = this.m_layoutContainer.GetMetricTrack(desc) as GraphTrackController;
			GraphTrackMetric graphTrackMetric2;
			if (graphTrackController != null)
			{
				MoveMetricFromGraphTrackCommand moveMetricFromGraphTrackCommand = new MoveMetricFromGraphTrackCommand();
				moveMetricFromGraphTrackCommand.Track = graphTrackController;
				moveMetricFromGraphTrackCommand.PID = desc.ProcessId;
				moveMetricFromGraphTrackCommand.MetricId = desc.MetricId;
				moveMetricFromGraphTrackCommand.MetricName = desc.MetricName;
				moveMetricFromGraphTrackCommand.MetricType = desc.MetricType;
				moveMetricFromGraphTrackCommand.OnCompleted = delegate(GraphTrackMetric result)
				{
					resultPromise.SetResult(result);
				};
				SdpApp.CommandManager.ExecuteCommand(moveMetricFromGraphTrackCommand);
				GraphTrackMetric graphTrackMetric = await resultPromise.Task;
				graphTrackMetric2 = graphTrackMetric;
			}
			else
			{
				graphTrackMetric2 = null;
			}
			return graphTrackMetric2;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00027A70 File Offset: 0x00025C70
		protected override void m_viewMetricDropped(object sender, MetricDroppedEventArgs e)
		{
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricId);
			AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
			addMetricToTrackCommand.Container = this;
			addMetricToTrackCommand.IsPreview = false;
			if (metricByID != null && !metricByID.IsGlobal())
			{
				foreach (uint num in e.Pids)
				{
					addMetricToTrackCommand.MetricId = e.MetricId;
					addMetricToTrackCommand.PID = num;
					SdpApp.ExecuteCommand(addMetricToTrackCommand);
				}
			}
			if (metricByID != null && metricByID.IsGlobal())
			{
				addMetricToTrackCommand.MetricId = e.MetricId;
				addMetricToTrackCommand.PID = uint.MaxValue;
				SdpApp.ExecuteCommand(addMetricToTrackCommand);
			}
			new RemovePreviewMetricsCommand
			{
				Controller = this.m_layoutContainer
			}.Execute();
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00027B44 File Offset: 0x00025D44
		protected override void m_view_CategoryDropped(object sender, MetricDroppedEventArgs e)
		{
			List<uint> metricsByCategory = SdpApp.ConnectionManager.GetMetricsByCategory(e.MetricId);
			foreach (uint num in metricsByCategory)
			{
				Metric metric = MetricManager.Get().GetMetric(num);
				if ((metric.GetProperties().captureTypeMask & 1U) != 0U && !metric.GetProperties().hidden)
				{
					AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
					addMetricToTrackCommand.MetricId = num;
					addMetricToTrackCommand.Container = this;
					addMetricToTrackCommand.IsPreview = false;
					if (metric != null && !metric.IsGlobal())
					{
						foreach (uint num2 in e.Pids)
						{
							Process process = ProcessManager.Get().GetProcess(num2);
							if (process != null && process.IsMetricLinked(num))
							{
								addMetricToTrackCommand.PID = num2;
								SdpApp.ExecuteCommand(addMetricToTrackCommand);
							}
						}
					}
					if (metric != null && metric.IsGlobal())
					{
						addMetricToTrackCommand.PID = uint.MaxValue;
						SdpApp.ExecuteCommand(addMetricToTrackCommand);
					}
				}
			}
			new RemovePreviewMetricsCommand
			{
				Controller = this.m_layoutContainer
			}.Execute();
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00027C9C File Offset: 0x00025E9C
		private void timeEvents_DataViewBoundsChanged(object sender, EventArgs e)
		{
			IGraphTrackView graphTrackView = this.m_view as IGraphTrackView;
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			graphTrackView.SetDataViewBounds(timeModel.DataViewBoundsMin, timeModel.DataViewBoundsMax);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00027CE4 File Offset: 0x00025EE4
		private void connectionEvents_DataReceived(object sender, DataReceivedEventArgs e)
		{
			uint processID = e.ReceivedData.ProcessID;
			uint id = e.ReceivedData.Metric.GetProperties().id;
			GraphTrackMetric graphTrackMetric = this.Metrics.FindMetricByIDAndPID(id, processID);
			if (graphTrackMetric != null)
			{
				double num = e.ReceivedData.Timestamp;
				DoubleData doubleData = e.ReceivedData as DoubleData;
				if (doubleData != null)
				{
					graphTrackMetric.AddData(num, doubleData.Value);
					return;
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00027D57 File Offset: 0x00025F57
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00027D5F File Offset: 0x00025F5F
		public GraphTrackMetricList Metrics { get; private set; } = new GraphTrackMetricList();

		// Token: 0x06000D5E RID: 3422 RVA: 0x00027D68 File Offset: 0x00025F68
		public override TrackViewDesc SaveSettings()
		{
			GraphTrackViewDesc graphTrackViewDesc = new GraphTrackViewDesc();
			base.SaveCommonSettings(graphTrackViewDesc);
			graphTrackViewDesc.TrackType = "Graph";
			graphTrackViewDesc.Metrics = new GraphTrackMetricDescList();
			foreach (GraphTrackMetric graphTrackMetric in this.Metrics)
			{
				GraphTrackMetricDesc graphTrackMetricDesc = new GraphTrackMetricDesc();
				graphTrackMetricDesc.CaptureId = this.m_layoutContainer.CaptureId;
				graphTrackMetricDesc.Metric = graphTrackMetric.Descriptor;
				graphTrackMetricDesc.Color = graphTrackMetric.Color;
				graphTrackViewDesc.Metrics.Add(graphTrackMetricDesc);
			}
			return graphTrackViewDesc;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00027E14 File Offset: 0x00026014
		public override void LoadSettings(TrackViewDesc track_desc)
		{
			base.LoadSettings(track_desc);
			GraphTrackViewDesc graphTrackViewDesc = track_desc as GraphTrackViewDesc;
			if (graphTrackViewDesc != null)
			{
				foreach (GraphTrackMetricDesc graphTrackMetricDesc in graphTrackViewDesc.Metrics)
				{
					if (graphTrackMetricDesc != null)
					{
						this.AddMetric(graphTrackMetricDesc);
					}
				}
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00027E7C File Offset: 0x0002607C
		public void MakeMetricTransient(uint metricID)
		{
			GraphTrackMetric graphTrackMetric = this.Metrics.FindMetricByID(metricID);
			if (graphTrackMetric != null)
			{
				this.m_layoutContainer.RemoveMetric(graphTrackMetric.Descriptor);
				graphTrackMetric.MakeTransient();
			}
		}

		// Token: 0x04000960 RID: 2400
		private int m_colorIndex;

		// Token: 0x04000961 RID: 2401
		private static List<Color> m_defaultColors = new List<Color>
		{
			new Color(0.62, 0.8, 0.23),
			new Color(0.22, 0.22, 0.85),
			new Color(0.99, 0.83, 0.1),
			new Color(0.22, 0.74, 0.79),
			new Color(0.96, 0.57, 0.11),
			new Color(0.9, 0.13, 0.16),
			new Color(0.5, 0.0, 0.0),
			new Color(0.5, 0.0, 0.5),
			new Color(1.0, 0.0, 1.0)
		};

		// Token: 0x020003C8 RID: 968
		public enum DrawMode
		{
			// Token: 0x04000D44 RID: 3396
			DRAW_LINE,
			// Token: 0x04000D45 RID: 3397
			DRAW_STEPPED_LINE,
			// Token: 0x04000D46 RID: 3398
			DRAW_BAR,
			// Token: 0x04000D47 RID: 3399
			DRAW_STACKED,
			// Token: 0x04000D48 RID: 3400
			DRAW_STACKED100,
			// Token: 0x04000D49 RID: 3401
			DRAW_DISCRETE_POINTS
		}
	}
}
