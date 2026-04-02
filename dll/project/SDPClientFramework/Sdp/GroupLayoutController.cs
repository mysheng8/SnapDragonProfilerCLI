using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cairo;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.Views.Flow.Controllers;
using SDPClientFramework.Views.Flow.ViewModels.GanttTrack;

namespace Sdp
{
	// Token: 0x020002A0 RID: 672
	public class GroupLayoutController : IViewController
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00023281 File Offset: 0x00021481
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x00023289 File Offset: 0x00021489
		public string WindowName { get; set; }

		// Token: 0x06000C87 RID: 3207 RVA: 0x00023294 File Offset: 0x00021494
		public void OnWindowNameChanged(object o, EventArgs e)
		{
			IDockWindow dockWindow = (IDockWindow)o;
			this.WindowName = dockWindow.Name;
			SdpApp.ConnectionManager.SetCaptureName(this.CaptureId, this.WindowName);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000232CC File Offset: 0x000214CC
		public void StartCapture()
		{
			this.m_dataSourcesController.ReadOnly = true;
			this.ActivateExportButton();
			this.m_view.SetExportButtonInteractable(false);
			foreach (KeyValuePair<IdNamePair, List<IdNamePair>> keyValuePair in SdpApp.ModelManager.TraceModel.CurrentSources)
			{
				MetricDescription metricDescription = new MetricDescription(SdpApp.ConnectionManager.GetMetricByID(keyValuePair.Key.Id));
				IMetricPlugin metricPlugin = SdpApp.PluginManager.GetMetricPlugin(metricDescription);
				if (metricPlugin != null)
				{
					metricPlugin.StartCapture(metricDescription);
				}
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00023374 File Offset: 0x00021574
		public void StopCapture()
		{
			this.m_view.HideDataSourcesPanel();
			this.m_view.AddTraceSummaryPanel();
			this.AlreadyCaptured = true;
			if (this.SelectedProcess != null && this.SelectedProcess != null)
			{
				SdpApp.UIManager.SetWindowName(this.m_currentWindowName, " - [" + global::System.IO.Path.GetFileName(this.SelectedProcess.DisplayName) + "]");
			}
			SdpApp.UIManager.FocusCaptureWindow(this.WindowName, "Capture");
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x000233F2 File Offset: 0x000215F2
		public IdNamePair SelectedProcess
		{
			get
			{
				if (this.m_dataSourcesController.ProcessesSelected != null && this.m_dataSourcesController.ProcessesSelected.Count >= 1)
				{
					return this.m_dataSourcesController.ProcessesSelected[0];
				}
				return null;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00023427 File Offset: 0x00021627
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0002342F File Offset: 0x0002162F
		public string FilterEntry { get; set; }

		// Token: 0x17000293 RID: 659
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00023438 File Offset: 0x00021638
		public bool CaptureButtonVisible
		{
			set
			{
				this.m_view.CaptureButtonVisible = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x00023446 File Offset: 0x00021646
		public bool ZoomButtonsVisible
		{
			set
			{
				this.m_view.ZoomButtonsVisible = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00023454 File Offset: 0x00021654
		public bool DataSourcesVisible
		{
			set
			{
				this.m_view.DataSourcesVisible = value;
				this.m_settings.DataSourcesVisible = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0002346E File Offset: 0x0002166E
		public bool AppKilledWhileCapturing
		{
			set
			{
				this.m_alreadyCaptured = value;
				if (this.m_alreadyCaptured)
				{
					this.m_view.CaptureButtonEnabled = true;
					if (SdpApp.ConnectionManager.IsConnected())
					{
						this.m_view.NewCaptureButtonVisible = true;
					}
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x000234A3 File Offset: 0x000216A3
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x000234AC File Offset: 0x000216AC
		public bool AlreadyCaptured
		{
			get
			{
				return this.m_alreadyCaptured;
			}
			set
			{
				this.m_alreadyCaptured = value;
				if (this.m_alreadyCaptured)
				{
					this.m_view.CaptureButtonEnabled = false;
					this.m_dataSourcesController.ReadOnly = true;
					this.m_view.HideDataSourcesPanel();
					if (SdpApp.ConnectionManager.IsConnected())
					{
						this.m_view.NewCaptureButtonVisible = true;
					}
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00023503 File Offset: 0x00021703
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0002350C File Offset: 0x0002170C
		public uint CaptureId
		{
			get
			{
				return this.m_captureId;
			}
			set
			{
				this.m_captureId = value;
				this.m_traceSummaryViewController.CaptureId = this.m_captureId;
				if (!SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey((int)this.m_captureId))
				{
					SdpApp.ModelManager.TraceModel.GroupLayoutControllers.Add((int)this.m_captureId, this);
				}
				TimeEvents timeEvents = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_captureId);
				if (timeEvents != null)
				{
					TimeEvents timeEvents2 = timeEvents;
					timeEvents2.DataViewBoundsChanged = (EventHandler)Delegate.Combine(timeEvents2.DataViewBoundsChanged, new EventHandler(this.timeEvents_DataViewBoundsChanged));
				}
				if (this.CaptureId > 1U)
				{
					uint num = 0U;
					foreach (GroupLayoutController groupLayoutController in SdpApp.ModelManager.TraceModel.GroupLayoutControllers.Values)
					{
						if (groupLayoutController.CaptureType == this.CaptureType && groupLayoutController.CaptureId > num && groupLayoutController.CaptureId != this.m_captureId)
						{
							num = groupLayoutController.CaptureId;
						}
					}
					if (num != 0U)
					{
						SdpApp.ModelManager.DataSourcesModel.CopyMetricColors(num, this.m_captureId, false);
						SdpApp.ModelManager.DataSourcesModel.CopyExpandedCategories(num, this.m_captureId, false);
					}
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002365C File Offset: 0x0002185C
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x00023664 File Offset: 0x00021864
		public string SDPVersion
		{
			get
			{
				return this.m_SDPVersion;
			}
			set
			{
				this.m_SDPVersion = value;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0002366D File Offset: 0x0002186D
		public bool IsPaused
		{
			get
			{
				return this.m_view.IsPaused;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0002367A File Offset: 0x0002187A
		IView IViewController.View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00023682 File Offset: 0x00021882
		public ViewDesc SaveSettings()
		{
			this.SaveCurrentConfig();
			return this.m_settings;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00023690 File Offset: 0x00021890
		public bool LoadSettings(ViewDesc view_desc)
		{
			bool flag = false;
			this.ClearGroups();
			this.InitSettings();
			GroupLayoutViewDesc groupLayoutViewDesc = view_desc as GroupLayoutViewDesc;
			if (groupLayoutViewDesc != null)
			{
				this.m_settings = groupLayoutViewDesc;
				this.m_view.DataSourcesVisible = this.m_settings.DataSourcesVisible;
				string activeConfig = this.m_settings.ActiveConfig;
				GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(activeConfig);
				if (groupLayoutConfigDesc == null)
				{
					this.CreateConfig(activeConfig);
				}
				this.RestoreConfig(groupLayoutConfigDesc);
				this.ActiveConfig = activeConfig;
				this.CaptureType = groupLayoutViewDesc.CaptureType;
				flag = true;
			}
			return flag;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00023710 File Offset: 0x00021910
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x00023738 File Offset: 0x00021938
		public string ActiveConfig
		{
			get
			{
				string text = "";
				if (this.m_settings != null)
				{
					text = this.m_settings.ActiveConfig;
				}
				return text;
			}
			set
			{
				if (this.m_settings != null && this.m_settings.ActiveConfig != value)
				{
					GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(value);
					if (groupLayoutConfigDesc != null)
					{
						this.SaveSettings();
						this.RestoreConfig(groupLayoutConfigDesc);
						this.m_settings.ActiveConfig = value;
					}
				}
			}
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00023787 File Offset: 0x00021987
		public GroupLayoutController(IGroupLayoutView view)
			: this(view, SdpApp.EventsManager, SdpApp.ConnectionManager, new Sdp.Logging.Logger("SDPClient"))
		{
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000237A4 File Offset: 0x000219A4
		public GroupLayoutController(IGroupLayoutView view, EventsManager eventsManager, IConnectionManager connectionManager, ILogger logger)
		{
			this.m_groupControllers = new List<GroupController>();
			DeviceEvents deviceEvents = eventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectDisconnect));
			DeviceEvents deviceEvents2 = eventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEvents_ClientConnectDisconnect));
			ConnectionEvents connectionEvents = eventsManager.ConnectionEvents;
			connectionEvents.CaptureCompleted = (EventHandler<CaptureCompletedEventArgs>)Delegate.Combine(connectionEvents.CaptureCompleted, new EventHandler<CaptureCompletedEventArgs>(this.connectionEvents_CaptureCompleted));
			StatisticEvents statisticEvents = eventsManager.StatisticEvents;
			statisticEvents.StatisticPerCaptureAdded = (EventHandler<StatisticPerCaptureArgs>)Delegate.Combine(statisticEvents.StatisticPerCaptureAdded, new EventHandler<StatisticPerCaptureArgs>(this.statisticEvents_StatisticPerCaptureAdded));
			ClientEvents clientEvents = eventsManager.ClientEvents;
			clientEvents.CaptureWindowAdded = (EventHandler<CaptureWindowEventArgs>)Delegate.Combine(clientEvents.CaptureWindowAdded, new EventHandler<CaptureWindowEventArgs>(this.clientEvents_CaptureAdded));
			this.m_view = view;
			this.m_view.AddGroupActivated += this.OnAddGroupActivated;
			this.m_view.ExpandAllActivated += this.OnExpandAllActivated;
			this.m_view.CollapseAllActivated += this.OnCollapseAllActivated;
			this.m_view.MetricDropped += this.m_viewMetricDropped;
			this.m_view.MetricDataEntered += this.m_viewMetricDataEntered;
			this.m_view.MetricDataLeft += this.m_viewMetricDataLeft;
			this.m_view.CategoryDropped += this.m_view_CategoryDropped;
			this.m_view.ExportButtonToggled += this.m_view_ExportButtonToggled;
			this.m_view.NewCaptureButtonClicked += this.m_view_NewCaptureButtonClicked;
			this.m_view.StatisticsActivated += this.m_view_StatisticsActivated;
			this.m_view.PauseClicked += this.m_view_PauseClicked;
			this.m_view.ResumeClicked += this.m_view_ResumeClicked;
			this.m_view.ScrollLockClicked += this.m_view_ScrollLockClicked;
			this.m_view.AutoScaleToggled += this.m_view_AutoScaleToggled;
			this.m_view.ZoomIn += this.m_view_ZoomIn;
			this.m_view.ZoomOut += this.m_view_ZoomOut;
			this.m_view.PanLeft += this.m_view_PanLeft;
			this.m_view.PanRight += this.m_view_PanRight;
			this.m_view.ResetViewBounds += this.m_view_ResetViewBounds;
			this.m_view.ComboBoxUpdated += this.m_view_ComboBoxUpdated;
			this.m_view.TracksWidthChanged += this.m_view_TracksWidthChanged;
			TimeEventsCollection timeEventsCollection = eventsManager.TimeEventsCollection;
			timeEventsCollection.AutoScaleToggled = (EventHandler<AutoScaleEventArgs>)Delegate.Combine(timeEventsCollection.AutoScaleToggled, new EventHandler<AutoScaleEventArgs>(this.timeEvents_AutoScaleToggled));
			this.m_view.TimelineView.SelectedBookmarksChanged += this.timelineView_SelectedBookmarksChanged;
			ConnectionEvents connectionEvents2 = eventsManager.ConnectionEvents;
			connectionEvents2.MetricEndDrag = (EventHandler)Delegate.Combine(connectionEvents2.MetricEndDrag, new EventHandler(this.OnMetricEndDrag));
			InspectorViewEvents inspectorViewEvents = SdpApp.EventsManager.InspectorViewEvents;
			inspectorViewEvents.MultiSelection = (EventHandler<MultiSelectionActivationEventArgs>)Delegate.Combine(inspectorViewEvents.MultiSelection, new EventHandler<MultiSelectionActivationEventArgs>(this.MultiSelectionUpdate));
			this.m_view.NewCaptureButtonVisible = false;
			this.m_view.PausingEnabled = false;
			this.m_view.ScrollLockEnabled = false;
			this.m_view.StatisticsButtonEnabled = false;
			this.m_view.FixedScale = GroupLayoutController.m_fixedScale;
			this.m_view.DataSourcesView.FilterEntryChanged += this.m_dataSourceView_FilterEntryChanged;
			this.m_view.CaptureButtonToggled += this.m_view_CaptureButtonToggled;
			this.InitSettings();
			this.m_traceSummaryViewController = new TraceSummaryViewController(this.m_view.TraceSummaryView, eventsManager);
			this.m_dataSourcesController = new DataSourcesController(this.m_view.DataSourcesView, eventsManager, connectionManager);
			this.m_dataSourcesController.Container = this;
			this.m_alreadyCaptured = false;
			this.m_globalMetricIds = new List<uint>();
			this.m_perProcessMetrics = new Dictionary<uint, List<uint>>();
			this.m_ganttTrackCoordinator = new GanttTrackCoordinator(this.m_view, eventsManager, logger);
			this.m_view.InteractionMode = new Maybe<InteractionMode>.None();
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00023BFC File Offset: 0x00021DFC
		public GroupController FindGroupInContainer(string name)
		{
			return this.m_groupControllers.Find((GroupController x) => x.Title.Contains(name));
		}

		// Token: 0x1700029D RID: 669
		// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x00023C2D File Offset: 0x00021E2D
		public bool ScrollLockToggled
		{
			set
			{
				this.m_view.ScrollLockToggled = value;
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00023C3B File Offset: 0x00021E3B
		public void SetViewType(GroupLayoutStyleType viewType)
		{
			this.m_view.SetViewType(viewType);
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00023C49 File Offset: 0x00021E49
		public void SetSelectedProcess(IdNamePair process)
		{
			this.m_dataSourcesController.SetSelectedProcesses(process);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00023C57 File Offset: 0x00021E57
		public void SetFilterEntry(string filterEntry)
		{
			if (!string.IsNullOrEmpty(filterEntry))
			{
				this.m_dataSourcesController.SetFilterEntry(filterEntry);
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00023C6D File Offset: 0x00021E6D
		public void SetName(CaptureType captureType, int sessionNumber, string windowName)
		{
			this.m_view.SetName(captureType, sessionNumber);
			this.m_currentWindowName = windowName;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00023C83 File Offset: 0x00021E83
		private void m_view_TracksWidthChanged(object sender, TracksWidthChangedEventArgs e)
		{
			this.m_view.TimelineView.TracksWidth = e.Width;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00023C9C File Offset: 0x00021E9C
		private void timelineView_SelectedBookmarksChanged(object o, SelectedBookmarksChangedEventArgs e)
		{
			foreach (GroupController groupController in this.m_groupControllers)
			{
				groupController.SelectedBookmarkTimestamps = e.SelectedBookmarkTimestamps;
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00023CF4 File Offset: 0x00021EF4
		private void m_dataSourceView_FilterEntryChanged(object sender, FilterEntryChangedArgs e)
		{
			this.FilterEntry = e.FilterEntry;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00023D04 File Offset: 0x00021F04
		public void Reset()
		{
			this.m_view.PausingEnabled = true;
			this.m_view.IsPaused = false;
			this.m_view.ScrollLockEnabled = true;
			foreach (GroupController groupController in this.m_groupControllers.ToArray())
			{
				this.m_view.RemoveGroup(groupController.View);
				groupController.Dispose();
			}
			this.m_groupControllers.Clear();
			this.m_globalMetricIds.Clear();
			this.m_perProcessMetrics.Clear();
			SdpApp.ModelManager.DataSourcesModel.Clear(this.m_captureId);
			this.m_dataSourcesController.Reset();
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00023DAC File Offset: 0x00021FAC
		public GroupController AddGroup(string name)
		{
			IGroupView groupView = this.m_view.AddGroup(name);
			GroupController groupController = new GroupController(groupView, this, this.m_ganttTrackCoordinator);
			groupController.Title = name;
			GroupController groupController2 = groupController;
			groupController2.RemoveGroupRequested = (EventHandler)Delegate.Combine(groupController2.RemoveGroupRequested, new EventHandler(this.OnRemoveGroupRequested));
			GroupController groupController3 = groupController;
			groupController3.DockUndockRequested = (EventHandler)Delegate.Combine(groupController3.DockUndockRequested, new EventHandler(this.OnDockUndockRequested));
			this.m_groupControllers.Add(groupController);
			this.m_view.SetExportButtonInteractable(true);
			if (this.m_groupControllers.Count == 1 && this.m_view.DataSourcesView is IGuidedSetupView && !this.m_alreadyCaptured)
			{
				this.StopCapture();
			}
			return groupController;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00023E66 File Offset: 0x00022066
		public void RemoveGroup(GroupController group)
		{
			if (group != null)
			{
				group.Dispose();
				this.m_groupControllers.Remove(group);
				this.m_view.RemoveGroup(group.View);
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00023E90 File Offset: 0x00022090
		public GroupLayoutConfigDesc CreateConfig(string config_name)
		{
			GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(config_name);
			if (groupLayoutConfigDesc == null)
			{
				groupLayoutConfigDesc = new GroupLayoutConfigDesc();
				groupLayoutConfigDesc.Name = config_name;
				groupLayoutConfigDesc.Groups = new GroupViewDescList();
				this.m_settings.Configs.Add(groupLayoutConfigDesc);
			}
			return groupLayoutConfigDesc;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00023ED4 File Offset: 0x000220D4
		public void DeleteConfig(string config_name)
		{
			GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(config_name);
			if (groupLayoutConfigDesc != null)
			{
				if (config_name == this.ActiveConfig)
				{
					int num = this.FindConfigIndex(config_name);
					int num2 = num - 1;
					if (num2 < 0)
					{
						num2 = 1;
					}
					if (num2 < this.m_settings.Configs.Count)
					{
						string name = this.m_settings.Configs[num2].Name;
						this.ActiveConfig = name;
					}
				}
				this.m_settings.Configs.Remove(groupLayoutConfigDesc);
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00023F50 File Offset: 0x00022150
		public void RenameConfig(string old_name, string new_name)
		{
			GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(old_name);
			if (groupLayoutConfigDesc != null)
			{
				if (old_name == this.ActiveConfig)
				{
					this.m_settings.ActiveConfig = new_name;
				}
				groupLayoutConfigDesc.Name = new_name;
			}
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00023F8C File Offset: 0x0002218C
		private void InitSettings()
		{
			this.m_settings = new GroupLayoutViewDesc();
			this.m_settings.TypeName = "Realtime";
			this.m_settings.ActiveConfig = "";
			this.m_settings.Configs = new GroupLayoutConfigDescList();
			this.m_settings.DataSourcesVisible = true;
			this.m_settings.CaptureType = (CaptureType)4277009102U;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00023FF0 File Offset: 0x000221F0
		private void SaveCurrentConfig()
		{
			string activeConfig = this.ActiveConfig;
			GroupLayoutConfigDesc groupLayoutConfigDesc = this.FindConfig(activeConfig);
			if (groupLayoutConfigDesc == null)
			{
				groupLayoutConfigDesc = this.CreateConfig(activeConfig);
			}
			groupLayoutConfigDesc.Groups.Clear();
			foreach (GroupController groupController in this.m_groupControllers)
			{
				GroupViewDesc groupViewDesc = groupController.SaveSettings();
				groupLayoutConfigDesc.Groups.Add(groupViewDesc);
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00024078 File Offset: 0x00022278
		private void RestoreConfig(GroupLayoutConfigDesc config)
		{
			if (config != null)
			{
				this.ClearGroups();
				GroupViewDescList groups = config.Groups;
				if (groups != null)
				{
					foreach (GroupViewDesc groupViewDesc in groups)
					{
						GroupController groupController = this.AddGroup(groupViewDesc.Name);
						if (groupController != null)
						{
							groupController.LoadSettings(groupViewDesc);
						}
					}
				}
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x000240EC File Offset: 0x000222EC
		private GroupLayoutConfigDesc FindConfig(string config_name)
		{
			GroupLayoutConfigDesc groupLayoutConfigDesc = null;
			foreach (GroupLayoutConfigDesc groupLayoutConfigDesc2 in this.m_settings.Configs)
			{
				if (groupLayoutConfigDesc2.Name == config_name)
				{
					groupLayoutConfigDesc = groupLayoutConfigDesc2;
					break;
				}
			}
			return groupLayoutConfigDesc;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00024154 File Offset: 0x00022354
		private int FindConfigIndex(string config_name)
		{
			int num = -1;
			for (int i = 0; i < this.m_settings.Configs.Count; i++)
			{
				GroupLayoutConfigDesc groupLayoutConfigDesc = this.m_settings.Configs[i];
				if (config_name == groupLayoutConfigDesc.Name)
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000241A4 File Offset: 0x000223A4
		private void ClearGroups()
		{
			List<GroupController> list = new List<GroupController>(this.m_groupControllers);
			foreach (GroupController groupController in list)
			{
				this.RemoveGroup(groupController);
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00024200 File Offset: 0x00022400
		private TrackType GetTrackType(Metric metric, IMetricPlugin metricPlugin)
		{
			if (metricPlugin != null)
			{
				if (metricPlugin.GetMetricTrackType(new MetricDescription(metric)) == MetricTrackType.Gantt)
				{
					return TrackType.Gantt;
				}
				if (metricPlugin.GetMetricTrackType(new MetricDescription(metric)) == MetricTrackType.Graph)
				{
					return TrackType.Graph;
				}
			}
			uint id = metric.GetProperties().id;
			MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(metric.GetProperties().categoryID);
			string text = "";
			if (metricCategory != null)
			{
				text = metricCategory.GetProperties().name;
			}
			if (text == "Systrace Metrics")
			{
				return TrackType.ICGantt;
			}
			return TrackType.Graph;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00024278 File Offset: 0x00022478
		public IDiagramView GetDiagramView(DiagramType diagramType)
		{
			IDiagramView diagramView = null;
			if (diagramType != DiagramType.PieChart)
			{
				if (diagramType == DiagramType.BlockFlowChart)
				{
					diagramView = this.m_view.BlockFlowView;
				}
			}
			else
			{
				diagramView = this.m_view.PieView;
			}
			return diagramView;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000242AC File Offset: 0x000224AC
		private GroupController TryAddGroup(string name)
		{
			GroupController groupController = null;
			foreach (GroupController groupController2 in this.m_groupControllers)
			{
				if (string.Compare(name, groupController2.Title) == 0)
				{
					groupController = groupController2;
				}
			}
			if (groupController == null)
			{
				AddGroupCommand addGroupCommand = new AddGroupCommand();
				addGroupCommand.Container = this;
				addGroupCommand.GroupName = name;
				SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
				groupController = addGroupCommand.Result;
			}
			return groupController;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00024334 File Offset: 0x00022534
		public bool ContainsMetric(uint metricId, uint pid)
		{
			if (pid == 4294967295U)
			{
				return this.m_globalMetricIds.Contains(metricId);
			}
			List<uint> list = new List<uint>();
			return this.m_perProcessMetrics.TryGetValue(pid, out list) && list.Contains(metricId);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00024371 File Offset: 0x00022571
		public void AddMetricToFlow(uint metricId, List<uint> pids)
		{
			this.AddMetricToFlow(metricId, pids, null, null);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002437D File Offset: 0x0002257D
		public void AddMetricToFlow(uint metricId, List<uint> pids, TrackControllerBase trackController)
		{
			this.AddMetricToFlow(metricId, pids, trackController, null);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00024389 File Offset: 0x00022589
		public void AddMetricToFlow(uint metricId, List<uint> pids, string metricName)
		{
			this.AddMetricToFlow(metricId, pids, null, metricName);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00024398 File Offset: 0x00022598
		public void AddMetricToFlow(uint metricId, List<uint> pids, TrackControllerBase trackController, string metricName)
		{
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricId);
			if (metricByID != null)
			{
				MetricDescription metricDescription = new MetricDescription(metricByID);
				if (metricByID.IsGlobal())
				{
					pids.Clear();
					pids.Add(uint.MaxValue);
				}
				foreach (uint num in pids)
				{
					AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
					addMetricToFlowCommand.Container = this;
					addMetricToFlowCommand.MetricId = metricId;
					addMetricToFlowCommand.IsGlobal = num == uint.MaxValue;
					addMetricToFlowCommand.MetricPlugin = SdpApp.PluginManager.GetMetricPlugin(metricDescription);
					addMetricToFlowCommand.TrackType = this.GetTrackType(metricByID, addMetricToFlowCommand.MetricPlugin);
					addMetricToFlowCommand.PID = num;
					addMetricToFlowCommand.TrackController = trackController;
					addMetricToFlowCommand.IsPreview = false;
					SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand);
					if (addMetricToFlowCommand.MetricPlugin != null)
					{
						TrackCreatedEventArgs trackCreatedEventArgs = new TrackCreatedEventArgs();
						trackCreatedEventArgs.MetricPlugin = addMetricToFlowCommand.MetricPlugin;
						trackCreatedEventArgs.TrackMetric = metricDescription;
						trackCreatedEventArgs.TrackController = addMetricToFlowCommand.Result;
						SdpApp.EventsManager.Raise<TrackCreatedEventArgs>(SdpApp.EventsManager.PluginEvents.TrackCreated, this, trackCreatedEventArgs);
					}
				}
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000244D0 File Offset: 0x000226D0
		public TrackControllerBase AddMetric(uint metricId, TrackType trackType, uint pid, IMetricPlugin metricPlugin, bool isGlobal, TrackControllerBase trackController, bool isPreview)
		{
			GroupController groupController = null;
			if (isGlobal)
			{
				if (!this.m_globalMetricIds.Contains(metricId))
				{
					this.m_globalMetricIds.Add(metricId);
				}
				if (trackController == null)
				{
					groupController = this.TryAddGroup("System");
				}
			}
			else
			{
				List<uint> list = new List<uint>();
				if (this.m_perProcessMetrics.TryGetValue(pid, out list))
				{
					if (!list.Contains(metricId))
					{
						list.Add(metricId);
					}
				}
				else
				{
					List<uint> list2 = new List<uint>();
					list2.Add(metricId);
					this.m_perProcessMetrics.Add(pid, list2);
				}
				if (trackController == null)
				{
					Process processByID = SdpApp.ConnectionManager.GetProcessByID(pid);
					if (processByID != null)
					{
						groupController = this.TryAddGroup(processByID.GetProperties().name);
					}
					else
					{
						groupController = this.TryAddGroup("System");
					}
				}
			}
			bool flag = false;
			if (groupController != null)
			{
				flag = true;
				AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
				addTrackToGroupCommand.TrackType = trackType;
				addTrackToGroupCommand.Container = groupController;
				addTrackToGroupCommand.MetricPlugin = metricPlugin;
				SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
				trackController = addTrackToGroupCommand.Result;
			}
			if (flag)
			{
				Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricId);
				double[] metricCategoryColor = SdpApp.ModelManager.ConnectionModel.GetMetricCategoryColor(metricByID.GetProperties().categoryID);
				trackController.View.ControlPanelHeaderBackColor = new Color(metricCategoryColor[0], metricCategoryColor[1], metricCategoryColor[2]);
			}
			TrackControllerBase metricTrack = this.GetMetricTrack(MetricDesc.CreateMetricDesc(metricId, pid, isPreview));
			if (metricTrack != null)
			{
				RemoveMetricFromTrackCommand removeMetricFromTrackCommand = new RemoveMetricFromTrackCommand();
				removeMetricFromTrackCommand.IsPreview = isPreview;
				removeMetricFromTrackCommand.MetricId = metricId;
				removeMetricFromTrackCommand.PID = pid;
				removeMetricFromTrackCommand.Track = metricTrack;
				removeMetricFromTrackCommand.ForceDeleteTrackIfEmpty = true;
				SdpApp.CommandManager.ExecuteCommand(removeMetricFromTrackCommand);
			}
			AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
			addMetricToTrackCommand.MetricId = metricId;
			addMetricToTrackCommand.PID = pid;
			addMetricToTrackCommand.IsPreview = isPreview;
			addMetricToTrackCommand.Container = trackController;
			SdpApp.CommandManager.ExecuteCommand(addMetricToTrackCommand);
			return trackController;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00024698 File Offset: 0x00022898
		public TrackControllerBase GetMetricTrack(MetricDesc desc)
		{
			for (int i = 0; i < this.m_groupControllers.Count; i++)
			{
				TrackControllerBase metricTrack = this.m_groupControllers[i].GetMetricTrack(desc);
				if (metricTrack != null)
				{
					return metricTrack;
				}
			}
			return null;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000246D4 File Offset: 0x000228D4
		public void RemoveMetric(MetricDesc desc)
		{
			if (desc.ProcessId != 4294967295U)
			{
				if (this.m_perProcessMetrics.ContainsKey(desc.ProcessId))
				{
					this.m_perProcessMetrics[desc.ProcessId].Remove(desc.MetricId);
					return;
				}
			}
			else
			{
				this.m_globalMetricIds.Remove(desc.MetricId);
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00024734 File Offset: 0x00022934
		private void timeEvents_DataViewBoundsChanged(object sender, EventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.m_captureId);
			if (timeModel != null)
			{
				this.m_view.TimelineView.SetViewBounds(timeModel.DataViewBoundsMin, timeModel.DataViewBoundsMax);
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00024778 File Offset: 0x00022978
		private void deviceEvents_ClientConnectDisconnect(object sender, EventArgs e)
		{
			if (SdpApp.ConnectionManager.IsConnected() && this.CaptureType == CaptureType.Realtime)
			{
				if (!this.AlreadyCaptured)
				{
					this.Reset();
				}
			}
			else
			{
				this.m_view.PausingEnabled = false;
				this.m_view.ScrollLockEnabled = false;
			}
			if (this.AlreadyCaptured && this.CaptureType == CaptureType.Trace)
			{
				this.m_view.NewCaptureButtonVisible = SdpApp.ConnectionManager.IsConnected();
			}
			if (!this.AlreadyCaptured)
			{
				this.m_view.CaptureButtonEnabled = SdpApp.ConnectionManager.IsConnected();
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00024805 File Offset: 0x00022A05
		private void connectionEvents_CaptureCompleted(object sender, CaptureCompletedEventArgs args)
		{
			if (args.CaptureId == this.CaptureId && !this.AlreadyCaptured)
			{
				this.StopCapture();
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00024823 File Offset: 0x00022A23
		private void statisticEvents_StatisticPerCaptureAdded(object sender, StatisticPerCaptureArgs args)
		{
			if ((long)args.CaptureID == (long)((ulong)this.m_captureId))
			{
				this.m_view.StatisticsButtonEnabled = true;
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00024844 File Offset: 0x00022A44
		private void m_viewMetricDataEntered(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				if (SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType != PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_SINGLE)
				{
					SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer = PreviewMetricsModel.ContainerType.GROUP_LAYOUT;
					foreach (PreviewMetricsModel.MetricPair metricPair in SdpApp.ModelManager.PreviewMetricsModel.Metrics)
					{
						if ((metricPair.ProcessID != 4294967295U || !this.m_globalMetricIds.Contains(metricPair.MetricID)) && (!this.m_perProcessMetrics.ContainsKey(metricPair.ProcessID) || !this.m_perProcessMetrics[metricPair.ProcessID].Contains(metricPair.MetricID)))
						{
							AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
							addMetricToFlowCommand.Container = this;
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
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000249BC File Offset: 0x00022BBC
		private void m_viewMetricDataLeft(object sender, EventArgs args)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				if (SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.GROUP_LAYOUT || SdpApp.ModelManager.PreviewMetricsModel.CurrentContainer == PreviewMetricsModel.ContainerType.NONE)
				{
					new RemovePreviewMetricsCommand
					{
						Controller = this
					}.Execute();
				}
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00024A38 File Offset: 0x00022C38
		private void m_viewMetricDropped(object sender, MetricDroppedEventArgs e)
		{
			if (SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType == PreviewMetricsModel.DragType.GRAPH_TRACK_METRIC_SINGLE)
			{
				return;
			}
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricId);
			AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
			addMetricToFlowCommand.Container = this;
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

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00024B08 File Offset: 0x00022D08
		private void m_view_CategoryDropped(object sender, MetricDroppedEventArgs e)
		{
			if (e.Pids.Count == 0)
			{
				e.Pids.Add(uint.MaxValue);
			}
			List<uint> metricsByCategory = SdpApp.ConnectionManager.GetMetricsByCategory(e.MetricId);
			foreach (uint num in metricsByCategory)
			{
				Metric metric = MetricManager.Get().GetMetric(num);
				if ((metric.GetProperties().captureTypeMask & 1U) != 0U && !metric.GetProperties().hidden)
				{
					AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
					addMetricToFlowCommand.Container = this;
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

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00024C6C File Offset: 0x00022E6C
		private void m_view_ComboBoxUpdated(object sender, ComboBoxUpdatedEventArgs e)
		{
			e.CaptureID = this.m_captureId;
			SdpApp.EventsManager.Raise<ComboBoxUpdatedEventArgs>(SdpApp.EventsManager.TimeEventsCollection.ComboBoxUpdated, this, e);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00024C98 File Offset: 0x00022E98
		private void m_view_ResetViewBounds(object sender, EventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null && timeModel.IsValid)
			{
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = (double)timeModel.DataBoundsMin;
				setDataViewBoundsCommand.Maximum = (double)timeModel.DataBoundsMax;
				setDataViewBoundsCommand.Dirty = true;
				setDataViewBoundsCommand.CaptureId = this.CaptureId;
				SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00024D04 File Offset: 0x00022F04
		private void m_view_PanRight(object sender, ViewBoundsEventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				double num = timeModel.DataViewBoundsMax - timeModel.DataViewBoundsMin;
				double num2 = 0.05;
				if (e.IsFast)
				{
					num2 = 0.15;
				}
				double num3 = num * num2;
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = timeModel.DataViewBoundsMin + num3;
				setDataViewBoundsCommand.Maximum = timeModel.DataViewBoundsMax + num3;
				setDataViewBoundsCommand.Dirty = false;
				setDataViewBoundsCommand.CaptureId = this.CaptureId;
				SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00024DA0 File Offset: 0x00022FA0
		private void m_view_PanLeft(object sender, ViewBoundsEventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				double num = timeModel.DataViewBoundsMax - timeModel.DataViewBoundsMin;
				double num2 = 0.05;
				if (e.IsFast)
				{
					num2 = 0.15;
				}
				double num3 = num * num2;
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = timeModel.DataViewBoundsMin - num3;
				setDataViewBoundsCommand.Maximum = timeModel.DataViewBoundsMax - num3;
				setDataViewBoundsCommand.Dirty = false;
				setDataViewBoundsCommand.CaptureId = this.CaptureId;
				SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00024E3C File Offset: 0x0002303C
		private void m_view_PauseClicked(object sender, EventArgs e)
		{
			this.m_view.IsPaused = true;
			PauseCaptureEventArgs pauseCaptureEventArgs = new PauseCaptureEventArgs();
			pauseCaptureEventArgs.CaptureId = this.m_captureId;
			pauseCaptureEventArgs.Pause = true;
			SdpApp.EventsManager.Raise<PauseCaptureEventArgs>(SdpApp.EventsManager.ConnectionEvents.PauseCapture, this, pauseCaptureEventArgs);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00024E8C File Offset: 0x0002308C
		private void m_view_ResumeClicked(object sender, EventArgs e)
		{
			this.m_view.IsPaused = false;
			PauseCaptureEventArgs pauseCaptureEventArgs = new PauseCaptureEventArgs();
			pauseCaptureEventArgs.CaptureId = this.m_captureId;
			pauseCaptureEventArgs.Pause = false;
			SdpApp.EventsManager.Raise<PauseCaptureEventArgs>(SdpApp.EventsManager.ConnectionEvents.PauseCapture, this, pauseCaptureEventArgs);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00024ED9 File Offset: 0x000230D9
		private void m_view_ScrollLockClicked(object sender, ScrollLockEventArgs e)
		{
			SdpApp.CommandManager.ExecuteCommand(new SetScrollLockCommand((int)this.CaptureId, e.Active));
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00024EF6 File Offset: 0x000230F6
		public bool FixedScale
		{
			get
			{
				return GroupLayoutController.m_fixedScale;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00024EFD File Offset: 0x000230FD
		private void m_view_AutoScaleToggled(object sender, AutoScaleEventArgs e)
		{
			if (e.Fixed != GroupLayoutController.m_fixedScale)
			{
				SdpApp.EventsManager.Raise<AutoScaleEventArgs>(SdpApp.EventsManager.TimeEventsCollection.AutoScaleToggled, this, e);
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00024F27 File Offset: 0x00023127
		private void timeEvents_AutoScaleToggled(object sender, AutoScaleEventArgs args)
		{
			this.m_view.FixedScale = (GroupLayoutController.m_fixedScale = args.Fixed);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00024F40 File Offset: 0x00023140
		private void m_view_ZoomIn(object sender, ViewBoundsEventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				double num = timeModel.DataViewBoundsMax - timeModel.DataViewBoundsMin;
				if (double.IsInfinity(num))
				{
					return;
				}
				double num2 = 0.05;
				if (e.IsFast)
				{
					num2 = 0.15;
				}
				double num3 = num * num2;
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = timeModel.DataViewBoundsMin + num3;
				setDataViewBoundsCommand.Maximum = timeModel.DataViewBoundsMax - num3;
				setDataViewBoundsCommand.Dirty = true;
				setDataViewBoundsCommand.CaptureId = this.CaptureId;
				SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00024FE4 File Offset: 0x000231E4
		private void m_view_ZoomOut(object sender, ViewBoundsEventArgs e)
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel(this.CaptureId);
			if (timeModel != null)
			{
				double num = timeModel.DataViewBoundsMax - timeModel.DataViewBoundsMin;
				if (double.IsInfinity(num))
				{
					return;
				}
				double num2 = 0.05;
				if (e.IsFast)
				{
					num2 = 0.15;
				}
				double num3 = num * num2;
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = timeModel.DataViewBoundsMin - num3;
				setDataViewBoundsCommand.Maximum = timeModel.DataViewBoundsMax + num3;
				setDataViewBoundsCommand.Dirty = true;
				setDataViewBoundsCommand.CaptureId = this.CaptureId;
				SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00025088 File Offset: 0x00023288
		private void OnAddGroupActivated(object sender, AddGroupEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Title))
			{
				AddGroupCommand addGroupCommand = new AddGroupCommand();
				addGroupCommand.Container = this;
				addGroupCommand.GroupName = e.Title;
				SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void OnExpandAllActivated(object sender, EventArgs e)
		{
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void OnCollapseAllActivated(object sender, EventArgs e)
		{
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000250C8 File Offset: 0x000232C8
		private void OnRemoveGroupRequested(object sender, EventArgs e)
		{
			RemoveGroupCommand removeGroupCommand = new RemoveGroupCommand();
			removeGroupCommand.Container = this;
			removeGroupCommand.Group = sender as GroupController;
			SdpApp.CommandManager.ExecuteCommand(removeGroupCommand);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000250FC File Offset: 0x000232FC
		private void OnDockUndockRequested(object sender, EventArgs e)
		{
			GroupController groupController = sender as GroupController;
			if (groupController != null)
			{
				bool isDocked = groupController.IsDocked;
				groupController.IsDocked = !groupController.IsDocked;
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002512C File Offset: 0x0002332C
		private void OnMetricEndDrag(object sender, EventArgs e)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				foreach (PreviewMetricsModel.MetricPair metricPair in SdpApp.ModelManager.PreviewMetricsModel.Metrics)
				{
					RemoveMetricFromTrackCommand removeMetricFromTrackCommand = new RemoveMetricFromTrackCommand();
					removeMetricFromTrackCommand.MetricId = metricPair.MetricID;
					removeMetricFromTrackCommand.PID = metricPair.ProcessID;
					removeMetricFromTrackCommand.IsPreview = true;
					removeMetricFromTrackCommand.Track = this.GetMetricTrack(MetricDesc.CreateMetricDesc(metricPair.MetricID, metricPair.ProcessID, true));
					removeMetricFromTrackCommand.ForceDeleteTrackIfEmpty = true;
					SdpApp.CommandManager.ExecuteCommand(removeMetricFromTrackCommand);
				}
				SdpApp.ModelManager.PreviewMetricsModel.Metrics.Clear();
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00025224 File Offset: 0x00023424
		private void MultiSelectionUpdate(object sender, MultiSelectionActivationEventArgs args)
		{
			this.m_exportIsFiltered = args.MultiSelect;
			SdpApp.AnalyticsManager.TrackInteraction(sender.ToString(), args.Description, "Inspector");
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0002524D File Offset: 0x0002344D
		public void ActivateExportButton()
		{
			this.m_view.SetExportTootlTip("Export trace data to CSV");
			this.m_view.ExportButtonVisible = true;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0002526C File Offset: 0x0002346C
		private void ExportTraceDataToCSV(string file)
		{
			try
			{
				SdpApp.AnalyticsManager.TrackExport("Trace Data");
				using (StreamWriter stream = new StreamWriter(file, false))
				{
					string text = "Process,Group,Process ID,Context ID,Thread ID,Track,Block Name,TimestampStart,TimestampEnd,Value, Extra Data";
					stream.WriteLine(text);
					List<string> fileLines = new List<string>();
					Action<string> <>9__1;
					foreach (GroupController groupController in this.m_groupControllers)
					{
						List<string> list = this.GenerateCsvLinesForGroup(groupController.TrackControllers, this.GenerateGroupStartingColumns(groupController));
						Action<string> action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate(string newLine)
							{
								fileLines.Add(newLine);
							});
						}
						list.ForEach(action);
					}
					fileLines.ForEach(delegate(string newLine)
					{
						stream.WriteLine(newLine);
					});
				}
			}
			catch (SystemException ex)
			{
				ShowMessageDialogCommand.ShowErrorDialog(ex.Message);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00025390 File Offset: 0x00023590
		public List<string> GenerateGroupStartingColumns(GroupController group)
		{
			string text = (group.Title.Contains("Process") ? "Process" : "System");
			string title = group.Title;
			if (group.Title.Contains("Process"))
			{
				Regex regex = new Regex("(Process:?\\s+([0-9]+)(\\s-\\s(Context:\\s(0x[a-f0-9]+))?(\\s-\\sThread:\\s(0x[a-f0-9]+))?)?)+", RegexOptions.IgnoreCase);
				string text2 = title.Substring(title.IndexOf("Process"));
				Match match = regex.Match(text2);
				if (match.Success)
				{
					string value = match.Groups[2].Value;
					string value2 = match.Groups[5].Value;
					string value3 = match.Groups[7].Value;
					string text3 = title.Remove(group.Title.IndexOf("Process"), match.Groups[1].Length).Replace(" |", "").Replace(":", "");
					return new List<string> { text, text3, value, value2, value3 };
				}
			}
			return new List<string> { text, title, "", "", "" };
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000254F0 File Offset: 0x000236F0
		public List<string> GenerateCsvLinesForGroup(List<TrackControllerBase> tracks, List<string> currentLineInfo)
		{
			List<string> lines = new List<string>();
			Action<string> <>9__0;
			Action<string> <>9__1;
			Func<string, string> <>9__2;
			foreach (TrackControllerBase trackControllerBase in tracks)
			{
				GanttTrackController ganttTrackController = trackControllerBase as GanttTrackController;
				if (ganttTrackController != null)
				{
					if (this.m_exportIsFiltered && ganttTrackController.ViewModel.GetSelectedSeriesCount() > 0)
					{
						using (IEnumerator<SeriesSelection> enumerator2 = ((MultiSelectInspectorViewModel)ganttTrackController.ViewModel.MultiSelectInspectorViewModel).SeriesSelections.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SeriesSelection seriesSelection = enumerator2.Current;
								List<string> list = this.GenerateCsvLinesForGanttData(seriesSelection.Markers.ToList<Marker>(), seriesSelection.Elements.ToList<Element>(), ganttTrackController, currentLineInfo, seriesSelection.Series.Name);
								Action<string> action;
								if ((action = <>9__0) == null)
								{
									action = (<>9__0 = delegate(string line)
									{
										lines.Add(line);
									});
								}
								list.ForEach(action);
							}
							continue;
						}
					}
					if (this.m_exportIsFiltered)
					{
						continue;
					}
					using (List<Series>.Enumerator enumerator3 = ganttTrackController.Series.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Series series = enumerator3.Current;
							List<string> list2 = this.GenerateCsvLinesForGanttSeries(series, ganttTrackController, currentLineInfo, series.Name);
							Action<string> action2;
							if ((action2 = <>9__1) == null)
							{
								action2 = (<>9__1 = delegate(string line)
								{
									lines.Add(line);
								});
							}
							list2.ForEach(action2);
						}
						continue;
					}
				}
				GraphTrackController graphTrackController = trackControllerBase as GraphTrackController;
				if (graphTrackController != null && !this.m_exportIsFiltered)
				{
					foreach (GraphTrackMetric graphTrackMetric in graphTrackController.Metrics)
					{
						foreach (DataPoint dataPoint in graphTrackMetric.DataPoints)
						{
							List<string> list3 = new List<string>
							{
								graphTrackMetric.Name,
								"",
								dataPoint.X.ToString(),
								"",
								dataPoint.Y.ToString(),
								""
							};
							list3.InsertRange(0, currentLineInfo);
							List<string> lines2 = lines;
							string text = ",";
							IEnumerable<string> enumerable = list3;
							Func<string, string> func;
							if ((func = <>9__2) == null)
							{
								func = (<>9__2 = (string word) => this.EscapeWord(word));
							}
							lines2.Add(string.Join(text, enumerable.Select(func)));
						}
					}
				}
			}
			return lines;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002582C File Offset: 0x00023A2C
		public List<string> GenerateCsvLinesForGanttData(List<Marker> markers, List<Element> elements, GanttTrackController track, List<string> currentLineInfo, string seriesName)
		{
			string text = "";
			string text2 = "";
			List<string> list = new List<string>();
			foreach (Marker marker in markers)
			{
				if (!track.TooltipStringsModel.TryGetValue(marker.TooltipId, out text))
				{
					text = "";
				}
				else
				{
					List<string> list2 = text.Split(new char[] { '\n' }).ToList<string>();
					text2 = ((list2.Count > 0) ? list2[0] : text);
				}
				List<string> list3 = new List<string>
				{
					seriesName,
					text2,
					marker.Position.ToString(),
					marker.Position.ToString(),
					""
				};
				list3.InsertRange(0, currentLineInfo);
				list.Add(string.Join(",", list3.Select((string word) => this.EscapeWord(word))) + ",\"" + this.EscapeWord(text) + "\"");
			}
			foreach (Element element in elements)
			{
				if (!track.NameStringsModel.TryGetValue(element.LabelId, out text2))
				{
					text2 = "";
				}
				if (!track.TooltipStringsModel.TryGetValue(element.TooltipId, out text))
				{
					text = "";
				}
				List<string> list4 = new List<string>
				{
					seriesName,
					text2,
					element.Start.ToString(),
					element.End.ToString(),
					""
				};
				list4.InsertRange(0, currentLineInfo);
				list.Add(string.Join(",", list4.Select((string word) => this.EscapeWord(word))) + ",\"" + this.EscapeWord(text) + "\"");
			}
			return list;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00025A60 File Offset: 0x00023C60
		private string EscapeWord(string word)
		{
			string text = word.Replace("\"", "\"\"").Replace("\n", " ");
			if (text.Contains(","))
			{
				text = "\"" + text + "\"";
			}
			return text;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00025AAC File Offset: 0x00023CAC
		public List<string> GenerateCsvLinesForGanttSeries(Series series, GanttTrackController track, List<string> currentLineInfo, string seriesName)
		{
			List<string> lines = new List<string>();
			this.GenerateCsvLinesForGanttData(series.Markers, series.Elements, track, currentLineInfo, seriesName).ForEach(delegate(string line)
			{
				lines.Add(line);
			});
			Action<string> <>9__1;
			foreach (Series series2 in series.Children)
			{
				List<string> list = this.GenerateCsvLinesForGanttSeries(series2, track, currentLineInfo, seriesName + "/" + series2.Name);
				Action<string> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(string line)
					{
						lines.Add(line);
					});
				}
				list.ForEach(action);
			}
			return lines;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00025B78 File Offset: 0x00023D78
		public void UpdateActiveMetricComboBox(HashSet<string> valuesToAdd, uint captureID)
		{
			if (captureID == this.CaptureId)
			{
				this.m_view.UpdateActiveMetricComboBox(valuesToAdd);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00025B90 File Offset: 0x00023D90
		private void m_view_CaptureButtonToggled(object sender, TakeCaptureArgs e)
		{
			SdpApp.ModelManager.DataSourcesModel.CaptureDuration = e.Duration;
			CaptureCommand captureCommand = new CaptureCommand();
			captureCommand.StartCapture = e.Enabled;
			captureCommand.Duration = e.Duration;
			SdpApp.CommandManager.ExecuteCommand(captureCommand);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00025BDC File Offset: 0x00023DDC
		private void m_view_ExportButtonToggled(object sender, FilePathEventArgs arg)
		{
			if (arg.Type == CaptureType.Trace)
			{
				this.ExportTraceDataToCSV(arg.Path);
				return;
			}
			ExportAllMetricCommand exportAllMetricCommand = new ExportAllMetricCommand();
			exportAllMetricCommand.path = arg.Path;
			SdpApp.CommandManager.ExecuteCommand(exportAllMetricCommand);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00025C1C File Offset: 0x00023E1C
		private void m_view_NewCaptureButtonClicked(object sender, EventArgs e)
		{
			SdpApp.CommandManager.ExecuteCommand(new NewCaptureCommand());
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00025C2D File Offset: 0x00023E2D
		private void m_view_StatisticsActivated(object sender, EventArgs e)
		{
			SdpApp.StatisticsManager.ShowDialog();
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00025C39 File Offset: 0x00023E39
		private void clientEvents_CaptureAdded(object sender, CaptureWindowEventArgs e)
		{
			if (sender == this)
			{
				e.Window.NameChanged += this.OnWindowNameChanged;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00025C56 File Offset: 0x00023E56
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00025C64 File Offset: 0x00023E64
		public CaptureType CaptureType
		{
			get
			{
				return this.m_dataSourcesController.CaptureType;
			}
			set
			{
				this.m_settings.CaptureType = value;
				this.m_dataSourcesController.CaptureType = value;
				if (this.CaptureType == CaptureType.Realtime)
				{
					this.CaptureButtonVisible = false;
					this.m_view.DataSourcesWidth = 260;
					this.m_view.ZoomButtonsVisible = false;
					this.m_view.PausingVisible = true;
					this.m_view.ScrollLockVisible = true;
					this.m_view.ExportButtonVisible = true;
					if (SdpApp.ConnectionManager.IsConnected())
					{
						this.Reset();
						return;
					}
				}
				else
				{
					this.m_view.ExportButtonVisible = false;
					this.m_view.ZoomButtonsVisible = true;
					this.m_view.PausingVisible = false;
					this.m_view.ScrollLockVisible = false;
					IGuidedSetupView guidedSetupView = this.m_view.DataSourcesView as IGuidedSetupView;
					guidedSetupView.CaptureDuration = SdpApp.ModelManager.DataSourcesModel.CaptureDuration;
					if (SdpApp.ModelManager.TraceModel.GroupLayoutControllers.Count > 1)
					{
						guidedSetupView.SelectNotebookPage(DataSourcesModel.RUNNING_PAGE);
					}
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0002367A File Offset: 0x0002187A
		public IGroupLayoutView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x04000931 RID: 2353
		private bool m_alreadyCaptured;

		// Token: 0x04000932 RID: 2354
		private uint m_captureId;

		// Token: 0x04000933 RID: 2355
		private string m_SDPVersion;

		// Token: 0x04000934 RID: 2356
		private readonly GanttTrackCoordinator m_ganttTrackCoordinator;

		// Token: 0x04000935 RID: 2357
		private IGroupLayoutView m_view;

		// Token: 0x04000936 RID: 2358
		private List<GroupController> m_groupControllers;

		// Token: 0x04000937 RID: 2359
		private GroupLayoutViewDesc m_settings;

		// Token: 0x04000938 RID: 2360
		private TraceSummaryViewController m_traceSummaryViewController;

		// Token: 0x04000939 RID: 2361
		private DataSourcesController m_dataSourcesController;

		// Token: 0x0400093A RID: 2362
		private List<uint> m_globalMetricIds;

		// Token: 0x0400093B RID: 2363
		private Dictionary<uint, List<uint>> m_perProcessMetrics;

		// Token: 0x0400093C RID: 2364
		private static bool m_fixedScale;

		// Token: 0x0400093D RID: 2365
		private string m_currentWindowName;

		// Token: 0x0400093E RID: 2366
		private bool m_exportIsFiltered;
	}
}
