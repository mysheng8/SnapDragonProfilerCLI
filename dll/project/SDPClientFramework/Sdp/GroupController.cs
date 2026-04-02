using System;
using System.Collections.Generic;
using Sdp.Logging;
using SDPClientFramework.Views.Flow.Controllers;

namespace Sdp
{
	// Token: 0x020002A2 RID: 674
	public class GroupController
	{
		// Token: 0x06000CFF RID: 3327 RVA: 0x0002606C File Offset: 0x0002426C
		internal GroupController(IGroupView groupView, GroupLayoutController container, GanttTrackCoordinator ganttTrackCoordinator)
		{
			this.TrackControllers = new List<TrackControllerBase>();
			this.m_isDocked = true;
			this.m_isExpanded = false;
			this.m_container = container;
			this.m_view = groupView;
			this.m_view.AddTrackRequested += this.OnAddTrackRequested;
			this.m_view.ExpandCollapseClicked += this.OnExpandCollapseClicked;
			this.m_view.RemoveClicked += this.OnRemoveClicked;
			this.m_view.MetricDropped += this.m_viewMetricDropped;
			this.m_view.CategoryDropped += this.m_view_CategoryDropped;
			this.m_view.MetricDataEntered += this.m_view_DragEntered;
			this.m_view.MetricDataLeft += this.m_view_DragLeft;
			this.m_ganttTrackCoordinator = ganttTrackCoordinator;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0002615C File Offset: 0x0002435C
		public void Dispose()
		{
			this.m_view.AddTrackRequested -= this.OnAddTrackRequested;
			this.m_view.ExpandCollapseClicked -= this.OnExpandCollapseClicked;
			this.m_view.RemoveClicked -= this.OnRemoveClicked;
			this.m_view.MetricDropped -= this.m_viewMetricDropped;
			this.m_view.CategoryDropped -= this.m_view_CategoryDropped;
			this.m_view.MetricDataEntered -= this.m_view_DragEntered;
			this.m_view.MetricDataLeft -= this.m_view_DragLeft;
			object trackControllerMutex = this.m_trackControllerMutex;
			lock (trackControllerMutex)
			{
				foreach (TrackControllerBase trackControllerBase in this.TrackControllers)
				{
					this.m_view.RemoveTrack(trackControllerBase.View);
					trackControllerBase.Dispose();
				}
				this.TrackControllers.Clear();
			}
		}

		// Token: 0x170002A4 RID: 676
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00026294 File Offset: 0x00024494
		public List<long> SelectedBookmarkTimestamps
		{
			set
			{
				object trackControllerMutex = this.m_trackControllerMutex;
				lock (trackControllerMutex)
				{
					foreach (TrackControllerBase trackControllerBase in this.TrackControllers)
					{
						trackControllerBase.SelectedBookmarkTimestamps = value;
					}
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00026310 File Offset: 0x00024510
		public void SwitchView(GroupLayoutStyleType viewType)
		{
			this.m_container.SetViewType(viewType);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002631E File Offset: 0x0002451E
		public TrackControllerBase AddTrack(TrackType trackType, IMetricPlugin metricPlugin)
		{
			return this.AddTrack(trackType, metricPlugin, SdpApp.EventsManager, SdpApp.ModelManager, SdpApp.CommandManager, new Sdp.Logging.Logger("GanttTrackController"));
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00026344 File Offset: 0x00024544
		public TrackControllerBase AddTrack(TrackType trackType, IMetricPlugin metricPlugin, EventsManager eventsManager, IModelManager modelManager, ICommandManager commandManager, ILogger ganttTrackControllerLogger)
		{
			ITrackViewBase trackViewBase = this.m_view.AddTrack(trackType, metricPlugin);
			TrackControllerBase trackControllerBase = null;
			switch (trackType)
			{
			case TrackType.Graph:
			{
				trackControllerBase = new GraphTrackController(trackViewBase, this.m_container, this);
				IGraphTrackView graphTrackView = trackViewBase as IGraphTrackView;
				graphTrackView.FixYAxis = this.m_container.FixedScale;
				break;
			}
			case TrackType.Gantt:
				trackControllerBase = new GanttTrackController(trackViewBase, this.m_container, this, eventsManager, modelManager, commandManager, ganttTrackControllerLogger);
				this.m_ganttTrackCoordinator.AddGanttTrackController(trackControllerBase as GanttTrackController);
				break;
			case TrackType.ICGantt:
				trackControllerBase = new ICGanttTrackController(trackViewBase, this.m_container, this);
				break;
			}
			if (trackControllerBase != null)
			{
				TrackControllerBase trackControllerBase2 = trackControllerBase;
				trackControllerBase2.RemoveTrackRequested = (EventHandler)Delegate.Combine(trackControllerBase2.RemoveTrackRequested, new EventHandler(this.OnRemoveTrackRequested));
				TrackControllerBase trackControllerBase3 = trackControllerBase;
				trackControllerBase3.ExpandCollapseTrackRequested = (EventHandler)Delegate.Combine(trackControllerBase3.ExpandCollapseTrackRequested, new EventHandler(this.OnExpandCollapseTrackRequested));
				TrackControllerBase trackControllerBase4 = trackControllerBase;
				trackControllerBase4.ResizeTrackRequested = (EventHandler<ResizeTrackRequestEventArgs>)Delegate.Combine(trackControllerBase4.ResizeTrackRequested, new EventHandler<ResizeTrackRequestEventArgs>(this.OnResizeTrackRequested));
				object trackControllerMutex = this.m_trackControllerMutex;
				lock (trackControllerMutex)
				{
					this.TrackControllers.Add(trackControllerBase);
				}
			}
			return trackControllerBase;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002647C File Offset: 0x0002467C
		public void RemoveTrack(TrackControllerBase track)
		{
			if (track is GanttTrackController)
			{
				this.m_ganttTrackCoordinator.RemoveGanttTrackController(track as GanttTrackController);
			}
			object trackControllerMutex = this.m_trackControllerMutex;
			lock (trackControllerMutex)
			{
				if (track != null)
				{
					track.Dispose();
					this.TrackControllers.Remove(track);
					this.m_view.RemoveTrack(track.View);
					if (this.TrackControllers.Count == 0)
					{
						SdpApp.ExecuteCommand(new RemoveGroupCommand
						{
							Group = this,
							Container = this.m_container
						});
					}
				}
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00026524 File Offset: 0x00024724
		public void RemoveAllTracks()
		{
			object trackControllerMutex = this.m_trackControllerMutex;
			lock (trackControllerMutex)
			{
				foreach (TrackControllerBase trackControllerBase in this.TrackControllers)
				{
					trackControllerBase.Dispose();
					this.m_view.RemoveTrack(trackControllerBase.View);
				}
				this.TrackControllers.Clear();
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000265BC File Offset: 0x000247BC
		public DiagramControllerBase AddDiagram(DiagramType diagramType, object data)
		{
			DiagramControllerBase diagramControllerBase = null;
			IDiagramView diagramView = this.m_container.GetDiagramView(diagramType);
			diagramView.DiagramData = data;
			if (diagramType != DiagramType.PieChart)
			{
				if (diagramType == DiagramType.BlockFlowChart)
				{
					diagramControllerBase = new BlockFlowController(diagramView, this.m_container, this);
				}
			}
			else
			{
				diagramControllerBase = new PieController(diagramView, this.m_container, this);
			}
			return diagramControllerBase;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00026607 File Offset: 0x00024807
		public void RemoveMetric(MetricDesc desc)
		{
			this.m_container.RemoveMetric(desc);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00026618 File Offset: 0x00024818
		public TrackControllerBase GetMetricTrack(MetricDesc desc)
		{
			object trackControllerMutex = this.m_trackControllerMutex;
			lock (trackControllerMutex)
			{
				for (int i = 0; i < this.TrackControllers.Count; i++)
				{
					if (this.TrackControllers[i].ContainsMetric(desc))
					{
						return this.TrackControllers[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00026690 File Offset: 0x00024890
		private void m_viewMetricDropped(object sender, MetricDroppedEventArgs e)
		{
			if (SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType == PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_SINGLE)
			{
				return;
			}
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricId);
			AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
			addMetricToFlowCommand.Container = this.m_container;
			addMetricToFlowCommand.MetricId = e.MetricId;
			addMetricToFlowCommand.IsPreview = false;
			if (metricByID != null && !metricByID.IsGlobal())
			{
				foreach (uint num in e.Pids)
				{
					addMetricToFlowCommand.PID = num;
					addMetricToFlowCommand.IsGlobal = false;
					SdpApp.ExecuteCommand(addMetricToFlowCommand);
				}
			}
			if (metricByID != null && metricByID.IsGlobal())
			{
				addMetricToFlowCommand.PID = uint.MaxValue;
				addMetricToFlowCommand.IsGlobal = true;
				SdpApp.ExecuteCommand(addMetricToFlowCommand);
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00026764 File Offset: 0x00024964
		private void m_view_CategoryDropped(object sender, MetricDroppedEventArgs e)
		{
			List<uint> metricsByCategory = SdpApp.ConnectionManager.GetMetricsByCategory(e.MetricId);
			foreach (uint num in metricsByCategory)
			{
				Metric metric = MetricManager.Get().GetMetric(num);
				if ((metric.GetProperties().captureTypeMask & 1U) != 0U && !metric.GetProperties().hidden)
				{
					AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
					addMetricToFlowCommand.Container = this.m_container;
					addMetricToFlowCommand.MetricId = num;
					addMetricToFlowCommand.IsPreview = false;
					if (metric != null && !metric.IsGlobal())
					{
						foreach (uint num2 in e.Pids)
						{
							Process process = ProcessManager.Get().GetProcess(num2);
							if (process != null && process.IsMetricLinked(num))
							{
								addMetricToFlowCommand.PID = num2;
								addMetricToFlowCommand.IsGlobal = false;
								SdpApp.ExecuteCommand(addMetricToFlowCommand);
							}
						}
					}
					if (metric != null && metric.IsGlobal())
					{
						addMetricToFlowCommand.PID = uint.MaxValue;
						addMetricToFlowCommand.IsGlobal = true;
						SdpApp.ExecuteCommand(addMetricToFlowCommand);
					}
				}
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000268B4 File Offset: 0x00024AB4
		private void m_view_DragEntered(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				if (SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType != PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_SINGLE)
				{
					SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer = PreviewMetricsModel.ContainerType.GROUP;
					SdpApp.ModelManager.PreviewMetricsModel.Group = this;
					foreach (PreviewMetricsModel.MetricPair metricPair in SdpApp.ModelManager.PreviewMetricsModel.Metrics)
					{
						AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
						addMetricToFlowCommand.Container = this.m_container;
						addMetricToFlowCommand.IsGlobal = metricPair.ProcessID == uint.MaxValue;
						addMetricToFlowCommand.MetricPlugin = null;
						addMetricToFlowCommand.TrackType = TrackType.Graph;
						addMetricToFlowCommand.MetricId = metricPair.MetricID;
						addMetricToFlowCommand.PID = metricPair.ProcessID;
						addMetricToFlowCommand.IsPreview = true;
						SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand);
					}
				}
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000269D4 File Offset: 0x00024BD4
		private void m_view_DragLeft(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				if ((SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.GROUP && SdpApp.ModelManager.PreviewMetricsModel.Group == this) || SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.NONE)
				{
					new RemovePreviewMetricsCommand
					{
						Controller = this.m_container
					}.Execute();
				}
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00026A68 File Offset: 0x00024C68
		private void OnAddTrackRequested(object sender, AddTrackRequestedEventArgs e)
		{
			AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
			addTrackToGroupCommand.TrackType = e.TrackType;
			addTrackToGroupCommand.Container = this;
			SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00026A9C File Offset: 0x00024C9C
		private void OnRemoveTrackRequested(object sender, EventArgs e)
		{
			RemoveTrackFromGroupCommand removeTrackFromGroupCommand = new RemoveTrackFromGroupCommand();
			removeTrackFromGroupCommand.Container = this;
			removeTrackFromGroupCommand.Track = sender as TrackControllerBase;
			SdpApp.CommandManager.ExecuteCommand(removeTrackFromGroupCommand);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00026AD0 File Offset: 0x00024CD0
		private void OnExpandCollapseTrackRequested(object sender, EventArgs e)
		{
			TrackControllerBase trackControllerBase = sender as TrackControllerBase;
			if (trackControllerBase != null)
			{
				trackControllerBase.View.ExpandCollapse();
				this.m_view.TrackExpandedCollapsed(trackControllerBase.View);
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00026B03 File Offset: 0x00024D03
		private void OnResizeTrackRequested(object sender, ResizeTrackRequestEventArgs e)
		{
			this.m_view.ResizeTrack(e.View, e.Height);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00026B1C File Offset: 0x00024D1C
		private void OnExpandCollapseClicked(object sender, EventArgs e)
		{
			if (this.m_view.ExpandGroup(!this.m_isExpanded))
			{
				this.m_isExpanded = !this.m_isExpanded;
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00026B43 File Offset: 0x00024D43
		private void OnRemoveClicked(object sender, EventArgs e)
		{
			if (this.RemoveGroupRequested != null)
			{
				this.RemoveGroupRequested(this, EventArgs.Empty);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00026B5E File Offset: 0x00024D5E
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00026B6B File Offset: 0x00024D6B
		public string Title
		{
			get
			{
				return this.m_view.Title;
			}
			set
			{
				this.m_view.Title = value;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00026B79 File Offset: 0x00024D79
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00026B81 File Offset: 0x00024D81
		public bool IsDocked
		{
			get
			{
				return this.m_isDocked;
			}
			set
			{
				this.m_isDocked = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00026B8A File Offset: 0x00024D8A
		public IGroupView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00026B92 File Offset: 0x00024D92
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00026B9A File Offset: 0x00024D9A
		public List<TrackControllerBase> TrackControllers { get; private set; }

		// Token: 0x06000D1B RID: 3355 RVA: 0x00026BA4 File Offset: 0x00024DA4
		public GroupViewDesc SaveSettings()
		{
			GroupViewDesc groupViewDesc = new GroupViewDesc();
			groupViewDesc.Name = this.Title;
			groupViewDesc.IsDocked = this.IsDocked;
			groupViewDesc.Tracks = new TrackViewDescList();
			object trackControllerMutex = this.m_trackControllerMutex;
			lock (trackControllerMutex)
			{
				foreach (TrackControllerBase trackControllerBase in this.TrackControllers)
				{
					TrackViewDesc trackViewDesc = trackControllerBase.SaveSettings();
					groupViewDesc.Tracks.Add(trackViewDesc);
				}
			}
			return groupViewDesc;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00026C58 File Offset: 0x00024E58
		public void LoadSettings(GroupViewDesc group_desc)
		{
			if (group_desc != null)
			{
				this.Title = group_desc.Name;
				this.IsDocked = group_desc.IsDocked;
				TrackViewDescList tracks = group_desc.Tracks;
				if (tracks != null)
				{
					foreach (TrackViewDesc trackViewDesc in tracks)
					{
						TrackControllerBase trackControllerBase = null;
						string trackType = trackViewDesc.TrackType;
						if (!(trackType == "Graph"))
						{
							if (!(trackType == "Gantt"))
							{
								Console.Error.WriteLine("Unknown Tack Type: " + trackType);
							}
							else
							{
								trackControllerBase = this.AddTrack(TrackType.ICGantt, null);
							}
						}
						else
						{
							trackControllerBase = this.AddTrack(TrackType.Graph, null);
						}
						if (trackControllerBase != null)
						{
							trackControllerBase.LoadSettings(trackViewDesc);
						}
					}
				}
			}
		}

		// Token: 0x04000946 RID: 2374
		private object m_trackControllerMutex = new object();

		// Token: 0x04000947 RID: 2375
		private readonly GanttTrackCoordinator m_ganttTrackCoordinator;

		// Token: 0x04000948 RID: 2376
		public EventHandler RemoveGroupRequested;

		// Token: 0x04000949 RID: 2377
		public EventHandler DockUndockRequested;

		// Token: 0x0400094A RID: 2378
		private IGroupView m_view;

		// Token: 0x0400094B RID: 2379
		private bool m_isExpanded;

		// Token: 0x0400094C RID: 2380
		private bool m_isDocked;

		// Token: 0x0400094E RID: 2382
		private GroupLayoutController m_container;
	}
}
