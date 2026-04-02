using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cairo;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x0200022A RID: 554
	public class DataSourcesController : IViewController
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x000173CC File Offset: 0x000155CC
		public DataSourcesController(IDataSourcesView view)
			: this(view, SdpApp.EventsManager, SdpApp.ConnectionManager)
		{
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000173E0 File Offset: 0x000155E0
		public DataSourcesController(IDataSourcesView view, EventsManager eventsManager, IConnectionManager connectionManager)
		{
			this.m_processesSelected = new List<IdNamePair>();
			this.m_metricsSelected = new List<IdNamePair>();
			this.m_view = view;
			this.m_view.ReadOnly = false;
			this.m_view.MetricButtonVisible = false;
			global::Device connectedDevice = connectionManager.GetConnectedDevice();
			ConnectionEvents connectionEvents = eventsManager.ConnectionEvents;
			connectionEvents.MetricAdded = (EventHandler<MetricAddedEventArgs>)Delegate.Combine(connectionEvents.MetricAdded, new EventHandler<MetricAddedEventArgs>(this.connectionEvents_MetricAdded));
			ConnectionEvents connectionEvents2 = eventsManager.ConnectionEvents;
			connectionEvents2.MetricDictionaryChanged = (EventHandler<MetricDictionaryChangedArgs>)Delegate.Combine(connectionEvents2.MetricDictionaryChanged, new EventHandler<MetricDictionaryChangedArgs>(this.connectionEvents_MetricDictionaryChanged));
			ConnectionEvents connectionEvents3 = eventsManager.ConnectionEvents;
			connectionEvents3.ProcessAdded = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents3.ProcessAdded, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessAdded));
			ConnectionEvents connectionEvents4 = eventsManager.ConnectionEvents;
			connectionEvents4.ProcessRemoved = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents4.ProcessRemoved, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessRemoved));
			ConnectionEvents connectionEvents5 = eventsManager.ConnectionEvents;
			connectionEvents5.ProcessMetricLinked = (EventHandler<ProcessMetricLinkedEventArgs>)Delegate.Combine(connectionEvents5.ProcessMetricLinked, new EventHandler<ProcessMetricLinkedEventArgs>(this.connectionEvents_ProcessMetricLinked));
			ConnectionEvents connectionEvents6 = eventsManager.ConnectionEvents;
			connectionEvents6.ProcessStateChanged = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents6.ProcessStateChanged, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessStateChanged));
			ConnectionEvents connectionEvents7 = eventsManager.ConnectionEvents;
			connectionEvents7.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents7.OptionAdded, new EventHandler<OptionEventArgs>(this.connectionEvents_OptionAdded));
			ConnectionEvents connectionEvents8 = eventsManager.ConnectionEvents;
			connectionEvents8.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Combine(connectionEvents8.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
			ConnectionEvents connectionEvents9 = eventsManager.ConnectionEvents;
			connectionEvents9.MetricHiddenToggled = (EventHandler<MetricHiddenToggledEventArgs>)Delegate.Combine(connectionEvents9.MetricHiddenToggled, new EventHandler<MetricHiddenToggledEventArgs>(this.connectionEvents_MetricHiddenToggled));
			ConnectionEvents connectionEvents10 = eventsManager.ConnectionEvents;
			connectionEvents10.ChangeMetricDataSourcesColor = (EventHandler<ChangeMetricDataSourcesColorArgs>)Delegate.Combine(connectionEvents10.ChangeMetricDataSourcesColor, new EventHandler<ChangeMetricDataSourcesColorArgs>(this.connectionEvents_ChangeMetricDataSourcesColor));
			SnapshotEvents snapshotEvents = eventsManager.SnapshotEvents;
			snapshotEvents.SnapshotProviderChanged = (EventHandler<SnapshotProviderChangedArgs>)Delegate.Combine(snapshotEvents.SnapshotProviderChanged, new EventHandler<SnapshotProviderChangedArgs>(this.snapshotEvents_OnSnapshotProviderChanged));
			this.m_view.SelectedProcessesChanged += this.m_view_SelectedProcessesChanged;
			this.m_view.MetricToggled += this.m_viewMetricToggled;
			this.m_view.MetricDoubleClicked += this.m_viewMetricDoubleClicked;
			this.m_view.MetricDragBegin += this.m_viewMetricDragBegin;
			this.m_view.MetricDragEnd += this.m_viewMetricDragEnd;
			this.m_view.AddMetricColor += this.m_view_AddMetricColor;
			this.m_view.RemoveMetricColor += this.m_view_RemoveMetricColor;
			this.m_view.RequestMetricRecolor += this.m_view_RequestMetricRecolor;
			this.m_view.MetricCategoryExpanded += this.m_viewMetricCategoryExpanded;
			this.m_view.MetricCategoryCollapsed += this.m_viewMetricCategoryCollapsed;
			this.m_view.LaunchAppClicked += this.m_view_LaunchApplicationClicked;
			this.m_view.EnableMetricClicked += this.m_view_EnableMetricClicked;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00017714 File Offset: 0x00015914
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00017743 File Offset: 0x00015943
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x0001774B File Offset: 0x0001594B
		public bool ReadOnly
		{
			set
			{
				this.m_view.ReadOnly = value;
				if (this.m_launchAppController != null)
				{
					this.m_launchAppController.EnableEvents = !value;
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00017770 File Offset: 0x00015970
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00017778 File Offset: 0x00015978
		public GroupLayoutController Container
		{
			get
			{
				return this.m_container;
			}
			set
			{
				this.m_container = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00017781 File Offset: 0x00015981
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0001778C File Offset: 0x0001598C
		public CaptureType CaptureType
		{
			get
			{
				return this.m_captureType;
			}
			set
			{
				this.m_captureType = value;
				if (value == CaptureType.Trace || value == CaptureType.Snapshot)
				{
					this.m_launchAppController = new LaunchApplicationDialogController((this.m_view as IGuidedSetupView).LaunchApp, value);
					this.m_launchAppController.LaunchAppClicked += this.launchApp_LaunchAppClicked;
				}
				object mutex = this.m_mutex;
				lock (mutex)
				{
					this.InvalidateProcessesDictionary();
					this.InvalidateMetrics(true);
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00017818 File Offset: 0x00015A18
		public List<IdNamePair> ProcessesSelected
		{
			get
			{
				object mutex = this.m_mutex;
				List<IdNamePair> list2;
				lock (mutex)
				{
					List<IdNamePair> list = new List<IdNamePair>(this.m_processesSelected);
					list2 = list;
				}
				return list2;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00017864 File Offset: 0x00015A64
		public void DetachEvents()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.MetricAdded = (EventHandler<MetricAddedEventArgs>)Delegate.Remove(connectionEvents.MetricAdded, new EventHandler<MetricAddedEventArgs>(this.connectionEvents_MetricAdded));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.MetricDictionaryChanged = (EventHandler<MetricDictionaryChangedArgs>)Delegate.Remove(connectionEvents2.MetricDictionaryChanged, new EventHandler<MetricDictionaryChangedArgs>(this.connectionEvents_MetricDictionaryChanged));
			ConnectionEvents connectionEvents3 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents3.ProcessAdded = (EventHandler<ProcessEventArgs>)Delegate.Remove(connectionEvents3.ProcessAdded, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessAdded));
			ConnectionEvents connectionEvents4 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents4.ProcessRemoved = (EventHandler<ProcessEventArgs>)Delegate.Remove(connectionEvents4.ProcessRemoved, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessRemoved));
			ConnectionEvents connectionEvents5 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents5.ProcessMetricLinked = (EventHandler<ProcessMetricLinkedEventArgs>)Delegate.Remove(connectionEvents5.ProcessMetricLinked, new EventHandler<ProcessMetricLinkedEventArgs>(this.connectionEvents_ProcessMetricLinked));
			ConnectionEvents connectionEvents6 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents6.ProcessStateChanged = (EventHandler<ProcessEventArgs>)Delegate.Remove(connectionEvents6.ProcessStateChanged, new EventHandler<ProcessEventArgs>(this.connectionEvents_ProcessStateChanged));
			ConnectionEvents connectionEvents7 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents7.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Remove(connectionEvents7.OptionAdded, new EventHandler<OptionEventArgs>(this.connectionEvents_OptionAdded));
			ConnectionEvents connectionEvents8 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents8.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Remove(connectionEvents8.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
			ConnectionEvents connectionEvents9 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents9.MetricHiddenToggled = (EventHandler<MetricHiddenToggledEventArgs>)Delegate.Remove(connectionEvents9.MetricHiddenToggled, new EventHandler<MetricHiddenToggledEventArgs>(this.connectionEvents_MetricHiddenToggled));
			ConnectionEvents connectionEvents10 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents10.ChangeMetricDataSourcesColor = (EventHandler<ChangeMetricDataSourcesColorArgs>)Delegate.Remove(connectionEvents10.ChangeMetricDataSourcesColor, new EventHandler<ChangeMetricDataSourcesColorArgs>(this.connectionEvents_ChangeMetricDataSourcesColor));
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00017A20 File Offset: 0x00015C20
		public void Reset()
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				this.m_metricsSelected.Clear();
				this.m_processesSelected.Clear();
				this.InvalidateSelectedProcesses();
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00017A78 File Offset: 0x00015C78
		public void SetStatus(StatusType statusType, string statusText, int duration)
		{
			this.m_view.SetStatus(statusType, statusText, duration, false, null);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00017A8A File Offset: 0x00015C8A
		public void HideStatus()
		{
			this.m_view.HideStatus();
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00017A98 File Offset: 0x00015C98
		public void InvalidateMetrics(bool invalidateGlobal)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				Dictionary<uint, List<DataSourcesViewMetric>> dictionary = new Dictionary<uint, List<DataSourcesViewMetric>>();
				Dictionary<uint, List<DataSourcesViewMetric>> dictionary2 = new Dictionary<uint, List<DataSourcesViewMetric>>();
				List<uint> globalMetrics = SdpApp.ConnectionManager.GetGlobalMetrics();
				if (invalidateGlobal && globalMetrics != null)
				{
					this.PopulateCategoryDictionary(dictionary2, globalMetrics, uint.MaxValue);
				}
				if (this.m_processesSelected != null && this.m_processesSelected.Count > 0)
				{
					IEnumerable<uint> enumerable = SdpApp.ConnectionManager.GetMetricsForProcess(this.m_processesSelected[0].Id);
					if (this.m_processesSelected.Count > 1)
					{
						foreach (IdNamePair idNamePair in this.m_processesSelected)
						{
							List<uint> metricsForProcess = SdpApp.ConnectionManager.GetMetricsForProcess(idNamePair.Id);
							enumerable = enumerable.Intersect(metricsForProcess, new MetricComparer());
						}
					}
					this.PopulateCategoryDictionary(dictionary, new List<uint>(enumerable), this.m_processesSelected[0].Id);
				}
				this.m_view.InvalidateMetrics(dictionary2, dictionary, SdpApp.ModelManager.DataSourcesModel.GetExpandedCategories(this.InferCaptureId()));
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00017BFC File Offset: 0x00015DFC
		public void SetFilterEntry(string filterEnry)
		{
			this.m_view.SetFilterEntry(filterEnry);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00017C0A File Offset: 0x00015E0A
		public void SetSelectedProcesses(IdNamePair process)
		{
			this.m_view.SetSelectedProcess(process);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00017C18 File Offset: 0x00015E18
		public void RequestRestartForWrongLayer()
		{
			ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
			showMessageDialogCommand.IconType = IconType.Warning;
			showMessageDialogCommand.Message = string.Format("This app has {0} profiling enabled. Restart app with {1} profiling instead?", (this.CaptureType == CaptureType.Snapshot) ? CaptureType.Trace.ToString().ToLower() : CaptureType.Snapshot.ToString().ToLower(), this.CaptureType.ToString().ToLower());
			showMessageDialogCommand.ButtonLayout = ButtonLayout.OKCancel;
			showMessageDialogCommand.AffirmativeText = "Restart";
			showMessageDialogCommand.OnCompleted = delegate(bool result)
			{
				if (!result)
				{
					this.m_view.MetricButtonVisible = true;
					return;
				}
				ulong num = 0UL;
				object mutex = this.m_mutex;
				lock (mutex)
				{
					if (this.m_processesSelected.Count > 0)
					{
						this.m_launchedApplication = this.m_processesSelected[0].Name;
						num = (ulong)this.m_processesSelected[0].Id;
					}
				}
				if (SdpApp.ConnectionManager.GetDeviceOS() != ConnectionManager.DeviceOS.QCLinux)
				{
					SdpApp.ConnectionManager.RequestAppRestart(this.CaptureType);
					return;
				}
				if (num == 0UL)
				{
					return;
				}
				AppStartSettings appStartSettings;
				if (SdpApp.ModelManager.DataSourcesModel.m_procStartSettings.TryGetValue(num, out appStartSettings))
				{
					SdpApp.ConnectionManager.RestartSelectedApp(this.CaptureType, appStartSettings);
					return;
				}
				SdpApp.ConnectionManager.StopSelectedApp(this.CaptureType);
				this.ShowLaunchAppDialog();
			};
			SdpApp.CommandManager.ExecuteCommand(showMessageDialogCommand);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00017CBD File Offset: 0x00015EBD
		public void ShowEnableMetricsButton()
		{
			this.m_view.MetricButtonVisible = true;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00017CCC File Offset: 0x00015ECC
		private void connectionEvents_MetricAdded(object o, MetricAddedEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (this.m_container != null)
				{
					GraphTrackController graphTrackController = this.m_container.GetMetricTrack(MetricDesc.CreateMetricDesc(e.Metric.GetProperties().id, e.Metric.GetProperties().pid, false)) as GraphTrackController;
					if (graphTrackController != null)
					{
						graphTrackController.MakeMetricTransient(e.Metric.GetProperties().id);
					}
				}
				this.AddMetric(e.Metric);
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00017D6C File Offset: 0x00015F6C
		private void connectionEvents_MetricDictionaryChanged(object o, MetricDictionaryChangedArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (e.Removed.Count > 0)
				{
					this.InvalidateMetrics(true);
				}
				else
				{
					foreach (Metric metric in e.Added)
					{
						this.AddMetric(metric);
					}
				}
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00017DF8 File Offset: 0x00015FF8
		private void connectionEvents_EnableMetric(object o, EnableMetricEventArgs e)
		{
			if (this.m_container != null && e.CaptureId == this.m_container.CaptureId)
			{
				Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricId);
				if (metricByID != null)
				{
					this.m_view.UpdateMetricEnabledStatus(e.MetricId, e.Enable, e.PID, SdpApp.ModelManager.DataSourcesModel.GetMetricColors(this.m_container.CaptureId), this.m_container.CaptureId);
				}
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00017E76 File Offset: 0x00016076
		private void connectionEvents_MetricHiddenToggled(object o, MetricHiddenToggledEventArgs e)
		{
			this.m_view.UpdateMetricHidden(e.MetricID, e.Hidden);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00017E90 File Offset: 0x00016090
		private void connectionEvents_ChangeMetricDataSourcesColor(object o, ChangeMetricDataSourcesColorArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (this.m_container != null && e.CaptureId == this.m_container.CaptureId)
				{
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricId);
					if (metricByID != null)
					{
						uint categoryID = metricByID.GetProperties().categoryID;
						Color color = new Color(e.color[0], e.color[1], e.color[2]);
						SdpApp.ModelManager.DataSourcesModel.UpdateMetricColor(this.m_container.CaptureId, new MetricIDSet(e.MetricId, e.ProcessId, categoryID, e.CaptureId), color);
						this.m_view.RecolorMetricList(SdpApp.ModelManager.DataSourcesModel.GetMetricColors(this.m_container.CaptureId), this.m_processesSelected);
					}
				}
			}
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00017F8C File Offset: 0x0001618C
		private void connectionEvents_ProcessMetricLinked(object o, ProcessMetricLinkedEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (this.m_processesSelected != null && this.m_processesSelected.Count > 0 && e.PID == this.m_processesSelected[0].Id)
				{
					this.AddMetric(MetricManager.Get().GetMetric(e.MetricID));
				}
				else
				{
					this.TryAddProcess(e.PID);
				}
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001801C File Offset: 0x0001621C
		private void connectionEvents_ProcessAdded(object o, ProcessEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				this.TryAddProcess(e.PID);
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00018064 File Offset: 0x00016264
		private void connectionEvents_ProcessRemoved(object o, ProcessEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				Process processByID = SdpApp.ConnectionManager.GetProcessByID(e.PID);
				MetricIDList linkedMetrics = processByID.GetLinkedMetrics();
				if (linkedMetrics.Count > 0)
				{
					foreach (uint num in linkedMetrics)
					{
						Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(num);
						if (metricByID.IsActive(e.PID, 4294967295U) && !metricByID.IsGlobal())
						{
							metricByID.Deactivate(e.PID, uint.MaxValue);
						}
					}
				}
				this.RemoveProcess(e.PID);
				IdNamePair idNamePair = null;
				foreach (IdNamePair idNamePair2 in this.m_processesSelected)
				{
					if (idNamePair2.Id == e.PID)
					{
						idNamePair = idNamePair2;
						break;
					}
				}
				if (idNamePair != null)
				{
					this.m_processesSelected.Remove(idNamePair);
					this.InvalidateSelectedProcesses();
					this.m_view.MetricButtonVisible = false;
					this.InvalidateMetrics(false);
				}
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000181DC File Offset: 0x000163DC
		private void connectionEvents_ProcessStateChanged(object o, ProcessEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				Process processByID = SdpApp.ConnectionManager.GetProcessByID(e.PID);
				if (processByID != null && processByID.GetProperties().state != ProcessState.ProcessRunning && this.m_processes.ContainsKey(e.PID))
				{
					this.m_processes.Remove(e.PID);
					SdpApp.ModelManager.DataSourcesModel.RemoveOptions(e.PID);
				}
				else if (processByID == null && this.m_processes.ContainsKey(e.PID))
				{
					this.m_processes.Remove(e.PID);
					SdpApp.ModelManager.DataSourcesModel.RemoveOptions(e.PID);
				}
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000182B4 File Offset: 0x000164B4
		private void connectionEvents_OptionAdded(object sender, OptionEventArgs e)
		{
			Option option = SdpApp.ConnectionManager.GetOption(e.OptionId, e.ProcessId);
			if (option.IsOptionProcInfo())
			{
				SdpApp.ModelManager.DataSourcesModel.AddOption(e.ProcessId, e.OptionId);
				if (this.m_processesSelected.Count > 0)
				{
					uint id = this.m_processesSelected[0].Id;
					InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
					inspectorViewDisplayEventArgs.Content = new PropertyGridDescriptionObject();
					inspectorViewDisplayEventArgs.Content.AddPropertyGridDescriptors(new List<PropertyDescriptor>());
					List<PropertyDescriptor> list = new List<PropertyDescriptor>();
					foreach (uint num in SdpApp.ModelManager.DataSourcesModel.GetOptions(id))
					{
						option = SdpApp.ConnectionManager.GetOption(num, id);
						PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option);
						if (propertyDescriptor != null)
						{
							list.Add(propertyDescriptor);
						}
					}
					inspectorViewDisplayEventArgs.Content.AddPropertyGridDescriptors(list);
					SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
				}
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000183DC File Offset: 0x000165DC
		private void snapshotEvents_OnSnapshotProviderChanged(object sender, SnapshotProviderChangedArgs e)
		{
			if (this.CaptureType == CaptureType.Snapshot)
			{
				this.InvalidateMetrics(false);
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000183F0 File Offset: 0x000165F0
		private void m_view_AddMetricColor(object sender, MetricColorArgs e)
		{
			MetricIDSet metricIDSet = new MetricIDSet(e.MetricID, e.ProcessID, e.CategoryID, e.CaptureID);
			SdpApp.ModelManager.DataSourcesModel.AddMetricColor(this.m_container.CaptureId, metricIDSet, e.Color);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001843C File Offset: 0x0001663C
		private void m_view_RemoveMetricColor(object sender, MetricColorArgs e)
		{
			MetricIDSet metricIDSet = new MetricIDSet(e.MetricID, e.ProcessID, e.CategoryID, e.CaptureID);
			SdpApp.ModelManager.DataSourcesModel.RemoveMetricColor(this.m_container.CaptureId, metricIDSet);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00018484 File Offset: 0x00016684
		private void m_view_RequestMetricRecolor(object sender, EventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				this.m_view.RecolorMetricList(SdpApp.ModelManager.DataSourcesModel.GetMetricColors(this.InferCaptureId()), this.m_processesSelected);
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000184E4 File Offset: 0x000166E4
		private void m_viewMetricCategoryCollapsed(object sender, MetricCategoryArgs e)
		{
			SdpApp.ModelManager.DataSourcesModel.RemoveExpandedCategory(this.InferCaptureId(), e.Category);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00018501 File Offset: 0x00016701
		private void m_viewMetricCategoryExpanded(object sender, MetricCategoryArgs e)
		{
			SdpApp.ModelManager.DataSourcesModel.AddExpandedCategory(this.InferCaptureId(), e.Category);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00018520 File Offset: 0x00016720
		private void m_viewMetricDragBegin(object sender, MetricBeginDragArgs e)
		{
			object previewMetricsModelLock = SdpApp.ModelManager.PreviewMetricsModel.PreviewMetricsModelLock;
			lock (previewMetricsModelLock)
			{
				SdpApp.ModelManager.PreviewMetricsModel.Metrics.Clear();
				if (e.MetricID != 0U)
				{
					bool flag2 = SdpApp.ConnectionManager.GetMetricByID(e.MetricID).IsGlobal();
					if (flag2)
					{
						SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(e.MetricID, uint.MaxValue));
					}
					else
					{
						foreach (uint num in e.ProcessIDs)
						{
							SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(e.MetricID, num));
						}
					}
					SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType = PreviewMetricsModel.DragType.DATA_SOURCE_METRIC;
				}
				else
				{
					List<uint> metricIdsByCategory = this.m_view.GetMetricIdsByCategory(e.CategoryID);
					foreach (uint num2 in metricIdsByCategory)
					{
						Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(num2);
						bool flag3 = metricByID.IsGlobal();
						if (flag3)
						{
							SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(num2, uint.MaxValue));
						}
						else
						{
							foreach (uint num3 in e.ProcessIDs)
							{
								SdpApp.ModelManager.PreviewMetricsModel.Metrics.Add(new PreviewMetricsModel.MetricPair(num2, num3));
							}
						}
					}
					SdpApp.ModelManager.PreviewMetricsModel.CurrentDragType = PreviewMetricsModel.DragType.DATA_SOURCE_CATEGORY;
				}
			}
			SdpApp.EventsManager.Raise<MetricBeginDragArgs>(SdpApp.EventsManager.ConnectionEvents.MetricBeginDrag, this, e);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00018770 File Offset: 0x00016970
		private void m_viewMetricDragEnd(object sender, EventArgs e)
		{
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.MetricEndDrag, this, EventArgs.Empty);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00018794 File Offset: 0x00016994
		private void m_viewMetricDoubleClicked(object sender, MetricDoubleClickedEventArgs e)
		{
			if (this.CaptureType == CaptureType.Realtime)
			{
				Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(e.MetricID);
				if (metricByID != null && metricByID.IsValid())
				{
					if (metricByID.IsGlobal())
					{
						if (this.m_container.ContainsMetric(metricByID.GetProperties().id, 4294967295U))
						{
							return;
						}
						AddMetricToFlowCommand addMetricToFlowCommand = new AddMetricToFlowCommand();
						addMetricToFlowCommand.Container = this.m_container;
						addMetricToFlowCommand.TrackController = null;
						addMetricToFlowCommand.TrackType = TrackType.Graph;
						addMetricToFlowCommand.MetricId = e.MetricID;
						addMetricToFlowCommand.PID = uint.MaxValue;
						addMetricToFlowCommand.IsGlobal = true;
						addMetricToFlowCommand.IsPreview = false;
						SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand);
						return;
					}
					else
					{
						using (List<IdNamePair>.Enumerator enumerator = e.SelectedProcesses.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								IdNamePair idNamePair = enumerator.Current;
								if (!this.m_container.ContainsMetric(metricByID.GetProperties().id, idNamePair.Id))
								{
									AddMetricToFlowCommand addMetricToFlowCommand2 = new AddMetricToFlowCommand();
									addMetricToFlowCommand2.Container = this.m_container;
									addMetricToFlowCommand2.TrackController = null;
									addMetricToFlowCommand2.TrackType = TrackType.Graph;
									addMetricToFlowCommand2.MetricId = metricByID.GetProperties().id;
									addMetricToFlowCommand2.PID = idNamePair.Id;
									addMetricToFlowCommand2.IsGlobal = false;
									addMetricToFlowCommand2.IsPreview = false;
									SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand2);
								}
							}
							return;
						}
					}
				}
				List<uint> metricIdsByCategory = this.m_view.GetMetricIdsByCategory(e.MetricID);
				foreach (uint num in metricIdsByCategory)
				{
					Metric metricByID2 = SdpApp.ConnectionManager.GetMetricByID(num);
					if (metricByID2.IsGlobal())
					{
						if (!this.m_container.ContainsMetric(num, 4294967295U))
						{
							AddMetricToFlowCommand addMetricToFlowCommand3 = new AddMetricToFlowCommand();
							addMetricToFlowCommand3.Container = this.m_container;
							addMetricToFlowCommand3.TrackController = null;
							addMetricToFlowCommand3.TrackType = TrackType.Graph;
							addMetricToFlowCommand3.MetricId = num;
							addMetricToFlowCommand3.PID = uint.MaxValue;
							addMetricToFlowCommand3.IsGlobal = true;
							addMetricToFlowCommand3.IsPreview = false;
							SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand3);
						}
					}
					else
					{
						foreach (IdNamePair idNamePair2 in e.SelectedProcesses)
						{
							if (!this.m_container.ContainsMetric(num, idNamePair2.Id))
							{
								AddMetricToFlowCommand addMetricToFlowCommand4 = new AddMetricToFlowCommand();
								addMetricToFlowCommand4.Container = this.m_container;
								addMetricToFlowCommand4.TrackController = null;
								addMetricToFlowCommand4.TrackType = TrackType.Graph;
								addMetricToFlowCommand4.MetricId = num;
								addMetricToFlowCommand4.PID = idNamePair2.Id;
								addMetricToFlowCommand4.IsGlobal = false;
								addMetricToFlowCommand4.IsPreview = false;
								SdpApp.CommandManager.ExecuteCommand(addMetricToFlowCommand4);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00018AA4 File Offset: 0x00016CA4
		private void m_viewMetricToggled(object sender, MetricToggledEventArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (!e.Activatable)
				{
					bool flag2 = SdpApp.ModelManager.DataSourcesModel.GetExpandedCategories(this.InferCaptureId()).Contains(e.Id);
					this.m_view.ExpandCategory(!flag2, e.Path, this.m_captureType);
				}
				else
				{
					if (!string.IsNullOrEmpty(e.Name))
					{
						if (!e.Enabled)
						{
							this.m_metricsSelected.Add(new IdNamePair(e.Id, e.Name));
						}
						else
						{
							foreach (IdNamePair idNamePair in this.m_metricsSelected)
							{
								if (idNamePair.Id == e.Id && idNamePair.Name == e.Name)
								{
									this.m_metricsSelected.Remove(idNamePair);
									break;
								}
							}
						}
					}
					CaptureType captureType = this.CaptureType;
					if (this.CaptureType == CaptureType.Trace)
					{
						IdNamePair idNamePair2 = new IdNamePair(e.Id, e.Name);
						Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(idNamePair2.Id);
						if (!e.Enabled)
						{
							if (!SdpApp.ModelManager.TraceModel.CurrentSources.ContainsKey(idNamePair2))
							{
								SdpApp.ModelManager.TraceModel.CurrentSources.Add(idNamePair2, new List<IdNamePair>());
							}
							if (!metricByID.IsGlobal())
							{
								SdpApp.ModelManager.TraceModel.CurrentSources[idNamePair2].AddRange(this.m_processesSelected);
							}
						}
						else
						{
							if (!metricByID.IsGlobal())
							{
								foreach (IdNamePair idNamePair3 in this.m_processesSelected)
								{
									bool flag3 = false;
									IdNamePair idNamePair4 = null;
									List<IdNamePair> list = new List<IdNamePair>();
									if (SdpApp.ModelManager.TraceModel.CurrentSources.TryGetValue(idNamePair2, out list))
									{
										foreach (IdNamePair idNamePair5 in list)
										{
											if (idNamePair5.Id == idNamePair3.Id && idNamePair5.Name == idNamePair3.Name)
											{
												flag3 = true;
												idNamePair4 = idNamePair5;
												break;
											}
										}
									}
									if (flag3)
									{
										SdpApp.ModelManager.TraceModel.CurrentSources[idNamePair2].Remove(idNamePair4);
									}
								}
							}
							if (SdpApp.ModelManager.TraceModel.CurrentSources.ContainsKey(idNamePair2) && SdpApp.ModelManager.TraceModel.CurrentSources[idNamePair2].Count == 0)
							{
								SdpApp.ModelManager.TraceModel.CurrentSources.Remove(idNamePair2);
							}
						}
						if (metricByID.IsGlobal())
						{
							EnableMetricEventArgs enableMetricEventArgs = new EnableMetricEventArgs();
							enableMetricEventArgs.CaptureId = this.m_container.CaptureId;
							enableMetricEventArgs.Enable = !e.Enabled;
							enableMetricEventArgs.MetricId = idNamePair2.Id;
							enableMetricEventArgs.PID = uint.MaxValue;
							enableMetricEventArgs.Mode = CaptureType.Trace;
							SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs);
						}
						else
						{
							foreach (IdNamePair idNamePair6 in this.m_processesSelected)
							{
								EnableMetricEventArgs enableMetricEventArgs2 = new EnableMetricEventArgs();
								enableMetricEventArgs2.CaptureId = this.m_container.CaptureId;
								enableMetricEventArgs2.Enable = !e.Enabled;
								enableMetricEventArgs2.MetricId = idNamePair2.Id;
								enableMetricEventArgs2.PID = idNamePair6.Id;
								enableMetricEventArgs2.Mode = CaptureType.Trace;
								SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs2);
							}
						}
					}
					if (this.CaptureType == CaptureType.Sampling)
					{
						IdNamePair idNamePair7 = new IdNamePair(e.Id, e.Name);
						Metric metricByID2 = SdpApp.ConnectionManager.GetMetricByID(idNamePair7.Id);
						if (!e.Enabled)
						{
							if (!SdpApp.ModelManager.SamplingModel.CurrentSources.ContainsKey(idNamePair7))
							{
								SdpApp.ModelManager.SamplingModel.CurrentSources.Add(idNamePair7, new List<IdNamePair>());
							}
							if (!metricByID2.IsGlobal())
							{
								SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair7].AddRange(this.m_processesSelected);
							}
						}
						else
						{
							if (!metricByID2.IsGlobal())
							{
								foreach (IdNamePair idNamePair8 in this.m_processesSelected)
								{
									bool flag4 = false;
									IdNamePair idNamePair9 = null;
									foreach (IdNamePair idNamePair10 in SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair7])
									{
										if (idNamePair10.Id == idNamePair8.Id && idNamePair10.Name == idNamePair8.Name)
										{
											flag4 = true;
											idNamePair9 = idNamePair10;
											break;
										}
									}
									if (flag4)
									{
										SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair7].Remove(idNamePair9);
									}
								}
							}
							if (SdpApp.ModelManager.SamplingModel.CurrentSources.ContainsKey(idNamePair7) && SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair7].Count == 0)
							{
								SdpApp.ModelManager.SamplingModel.CurrentSources.Remove(idNamePair7);
							}
						}
						if (metricByID2.IsGlobal())
						{
							EnableMetricEventArgs enableMetricEventArgs3 = new EnableMetricEventArgs();
							enableMetricEventArgs3.CaptureId = this.InferCaptureId();
							enableMetricEventArgs3.Enable = !e.Enabled;
							enableMetricEventArgs3.MetricId = idNamePair7.Id;
							enableMetricEventArgs3.PID = uint.MaxValue;
							enableMetricEventArgs3.Mode = CaptureType.Sampling;
							SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs3);
						}
						else
						{
							foreach (IdNamePair idNamePair11 in this.m_processesSelected)
							{
								EnableMetricEventArgs enableMetricEventArgs4 = new EnableMetricEventArgs();
								enableMetricEventArgs4.CaptureId = this.InferCaptureId();
								enableMetricEventArgs4.Enable = !e.Enabled;
								enableMetricEventArgs4.MetricId = idNamePair7.Id;
								enableMetricEventArgs4.PID = idNamePair11.Id;
								enableMetricEventArgs4.Mode = CaptureType.Sampling;
								SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs4);
							}
						}
					}
					if (this.CaptureType == CaptureType.Snapshot)
					{
						IdNamePair idNamePair12 = new IdNamePair(e.Id, e.Name);
						Metric metricByID3 = SdpApp.ConnectionManager.GetMetricByID(idNamePair12.Id);
						if (!e.Enabled)
						{
							if (!SdpApp.ModelManager.SnapshotModel.CurrentSources.ContainsKey(idNamePair12))
							{
								SdpApp.ModelManager.SnapshotModel.CurrentSources.Add(idNamePair12, new List<IdNamePair>());
							}
							if (!metricByID3.IsGlobal())
							{
								SdpApp.ModelManager.SnapshotModel.CurrentSources[idNamePair12].AddRange(this.m_processesSelected);
							}
						}
						else
						{
							if (!metricByID3.IsGlobal())
							{
								SdpApp.ModelManager.SnapshotModel.CurrentSources.Remove(idNamePair12);
							}
							if (SdpApp.ModelManager.SnapshotModel.CurrentSources.ContainsKey(idNamePair12) && SdpApp.ModelManager.SnapshotModel.CurrentSources[idNamePair12].Count == 0)
							{
								SdpApp.ModelManager.SnapshotModel.CurrentSources.Remove(idNamePair12);
							}
						}
						if (metricByID3.IsGlobal())
						{
							EnableMetricEventArgs enableMetricEventArgs5 = new EnableMetricEventArgs();
							enableMetricEventArgs5.CaptureId = this.InferCaptureId();
							enableMetricEventArgs5.Enable = !e.Enabled;
							enableMetricEventArgs5.MetricId = idNamePair12.Id;
							enableMetricEventArgs5.PID = uint.MaxValue;
							enableMetricEventArgs5.Mode = CaptureType.Snapshot;
							SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs5);
						}
						else
						{
							foreach (IdNamePair idNamePair13 in this.m_processesSelected)
							{
								EnableMetricEventArgs enableMetricEventArgs6 = new EnableMetricEventArgs();
								enableMetricEventArgs6.CaptureId = this.InferCaptureId();
								enableMetricEventArgs6.Enable = !e.Enabled;
								enableMetricEventArgs6.MetricId = idNamePair12.Id;
								enableMetricEventArgs6.PID = idNamePair13.Id;
								enableMetricEventArgs6.Mode = CaptureType.Snapshot;
								SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs6);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00019444 File Offset: 0x00017644
		private void m_view_SelectedProcessesChanged(object sender, SelectedProcessChangedArgs e)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				if (e.SelectedProcesses.Count == 1)
				{
					this.m_view.MetricButtonVisible = false;
					uint id = e.SelectedProcesses[0].Id;
					InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
					inspectorViewDisplayEventArgs.Content = new PropertyGridDescriptionObject();
					inspectorViewDisplayEventArgs.Content.AddPropertyGridDescriptors(new List<PropertyDescriptor>());
					if (id == 4294967294U)
					{
						this.m_view.SetSelectedProcess(null);
						this.m_processesSelected.Clear();
						this.InvalidateSelectedProcesses();
						this.InvalidateMetrics(false);
						SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
						new ShowMessageDialogCommand
						{
							IconType = IconType.Warning,
							ButtonLayout = ButtonLayout.OKCancel,
							Message = this.GenerateLaunchHelpMessage(),
							AffirmativeText = "Launch...",
							OnCompleted = delegate(bool result)
							{
								if (result)
								{
									this.ShowLaunchAppDialog();
								}
							}
						}.Execute();
					}
					else
					{
						DataSourcesController.ProcessInfo processInfo = null;
						this.m_processes.TryGetValue(id, out processInfo);
						if (processInfo != null)
						{
							this.m_processesSelected = e.SelectedProcesses;
							this.InvalidateSelectedProcesses();
							Process process = ProcessManager.Get().GetProcess(id);
							uint warningFlags = this.GetWarningFlags(process, this.CaptureType);
							if (warningFlags != 0U)
							{
								foreach (object obj in Enum.GetValues(typeof(ProcessWarnings)))
								{
									ProcessWarnings processWarnings = (ProcessWarnings)obj;
									if ((warningFlags & (uint)processWarnings) != 0U)
									{
										ProcessWithWarningSelectedEventArgs processWithWarningSelectedEventArgs = new ProcessWithWarningSelectedEventArgs();
										processWithWarningSelectedEventArgs.WarningType = processWarnings;
										processWithWarningSelectedEventArgs.Pid = id;
										processWithWarningSelectedEventArgs.CaptureType = this.CaptureType;
										SdpApp.EventsManager.Raise<ProcessWithWarningSelectedEventArgs>(SdpApp.EventsManager.DataSourceViewEvents.ProcessWithWarningSelected, this, processWithWarningSelectedEventArgs);
									}
								}
							}
							this.InvalidateMetrics(false);
							List<PropertyDescriptor> list = new List<PropertyDescriptor>();
							foreach (uint num in SdpApp.ModelManager.DataSourcesModel.GetOptions(id))
							{
								Option option = SdpApp.ConnectionManager.GetOption(num, id);
								PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option);
								if (propertyDescriptor != null)
								{
									list.Add(propertyDescriptor);
								}
							}
							inspectorViewDisplayEventArgs.Content.AddPropertyGridDescriptors(list);
							inspectorViewDisplayEventArgs.Description = "";
							SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
						}
					}
				}
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00019724 File Offset: 0x00017924
		private string GenerateLaunchHelpMessage()
		{
			SortedSet<RenderingAPI> supportedRenderingAPIs = SdpApp.ConnectionManager.GetSupportedRenderingAPIs(this.CaptureType);
			ConnectionManager.DeviceOS deviceOS = SdpApp.ConnectionManager.GetDeviceOS();
			string text = "";
			string text2 = "";
			List<RenderingAPI> list = new List<RenderingAPI>();
			if (supportedRenderingAPIs.Contains(RenderingAPI.Vulkan))
			{
				list.Add(RenderingAPI.Vulkan);
			}
			if (supportedRenderingAPIs.Contains(RenderingAPI.DirectX11))
			{
				list.Add(RenderingAPI.DirectX11);
			}
			if (supportedRenderingAPIs.Contains(RenderingAPI.DirectX12))
			{
				list.Add(RenderingAPI.DirectX12);
			}
			if (supportedRenderingAPIs.Contains(RenderingAPI.OpenCL) && deviceOS == ConnectionManager.DeviceOS.Windows)
			{
				list.Add(RenderingAPI.OpenCL);
			}
			this.FormatHelpMessageHeader(list, out text2);
			text += ((text2.Length > 0) ? (text2 + " apps must be launched with the Launch Application button.\n \n") : "");
			text2 = "";
			list = new List<RenderingAPI>();
			if (supportedRenderingAPIs.Contains(RenderingAPI.OpenGL))
			{
				list.Add(RenderingAPI.OpenGL);
			}
			if (supportedRenderingAPIs.Contains(RenderingAPI.OpenCL) && deviceOS != ConnectionManager.DeviceOS.Windows)
			{
				list.Add(RenderingAPI.OpenCL);
			}
			this.FormatHelpMessageHeader(list, out text2);
			text += ((text2.Length > 0) ? (text2 + " apps must be launched after Profiler is connected\nor with the Launch Application button.\n \n") : "");
			if (supportedRenderingAPIs.Contains(RenderingAPI.OpenGL) && deviceOS == ConnectionManager.DeviceOS.Android)
			{
				text += "OpenGL apps will use the most current instrumentation libraries when launched\nthrough Profiler, which may produce better results.";
			}
			return text;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00019850 File Offset: 0x00017A50
		private void FormatHelpMessageHeader(List<RenderingAPI> headerAPIs, out string messageHeader)
		{
			if (headerAPIs.Count == 0)
			{
				messageHeader = "";
				return;
			}
			if (headerAPIs.Count == 1)
			{
				messageHeader = headerAPIs[0].ToString();
				return;
			}
			if (headerAPIs.Count == 2)
			{
				messageHeader = string.Join<RenderingAPI>(" and ", headerAPIs);
				return;
			}
			string text = headerAPIs[headerAPIs.Count - 1].ToString();
			string text2 = string.Join<RenderingAPI>(", ", headerAPIs.GetRange(0, headerAPIs.Count - 1));
			messageHeader = text2 + ", and " + text;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000198EA File Offset: 0x00017AEA
		private void m_view_LaunchApplicationClicked(object sender, EventArgs e)
		{
			this.ShowLaunchAppDialog();
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000198F2 File Offset: 0x00017AF2
		private void m_view_EnableMetricClicked(object sender, EventArgs e)
		{
			this.RequestRestartForWrongLayer();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000198FC File Offset: 0x00017AFC
		private void InvalidateProcessesDictionary()
		{
			this.m_processes.Clear();
			ProcessList allProcesses = ProcessManager.Get().GetAllProcesses();
			foreach (Process process in allProcesses)
			{
				MetricIDList linkedMetrics = process.GetLinkedMetrics();
				bool flag = false;
				foreach (uint num in linkedMetrics)
				{
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(num);
					if ((metricByID.GetProperties().captureTypeMask & (uint)this.CaptureType) > 0U)
					{
						flag = true;
						break;
					}
				}
				uint warningFlags = this.GetWarningFlags(process, this.CaptureType);
				if ((warningFlags & 8U) > 0U)
				{
					flag = true;
				}
				if (flag && !this.m_processes.ContainsKey(process.GetProperties().pid))
				{
					this.m_processes.Add(process.GetProperties().pid, new DataSourcesController.ProcessInfo(process.GetProperties().name, warningFlags));
				}
			}
			this.InvalidateProcessView();
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00019A28 File Offset: 0x00017C28
		private void InvalidateProcessView()
		{
			List<IdNamePair> list = new List<IdNamePair>();
			foreach (KeyValuePair<uint, DataSourcesController.ProcessInfo> keyValuePair in this.m_processes)
			{
				list.Add(new IdNamePair(keyValuePair.Key, keyValuePair.Value.Name, keyValuePair.Value.WarningFlags == 0U));
			}
			this.m_view.InvalidateProcesses(list, this.m_processesSelected.GetRange(0, this.m_processesSelected.Count), this.m_captureType);
			if (this.m_processes.Count == 0)
			{
				this.m_view.SetStatus(StatusType.Warning, "No eligible processes found", 0, true, "If an app is running before Profiler connects to the device, it may not show up in this list or it may not provide GPU related metrics.");
				return;
			}
			this.m_view.SetStatus(StatusType.Neutral, "Select a process to see available metrics", 3000, false, null);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00019B10 File Offset: 0x00017D10
		private void TryAddProcess(uint pid)
		{
			if (this.CaptureType == (CaptureType)0U)
			{
				return;
			}
			Process processByID = SdpApp.ConnectionManager.GetProcessByID(pid);
			if (processByID == null || !processByID.IsValid())
			{
				return;
			}
			string name = processByID.GetProperties().name;
			uint warningFlags = this.GetWarningFlags(processByID, this.CaptureType);
			DataSourcesController.ProcessInfo processInfo = null;
			this.m_processes.TryGetValue(pid, out processInfo);
			if (processInfo == null)
			{
				bool flag = false;
				MetricIDList linkedMetrics = processByID.GetLinkedMetrics();
				foreach (uint num in linkedMetrics)
				{
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(num);
					if ((metricByID.GetProperties().captureTypeMask & (uint)this.CaptureType) > 0U)
					{
						flag = true;
						break;
					}
				}
				if ((warningFlags & 8U) > 0U)
				{
					flag = true;
				}
				if (flag)
				{
					this.m_processes.Add(pid, new DataSourcesController.ProcessInfo(name, warningFlags));
					if (name == this.m_launchedApplication)
					{
						string filterEntry = this.m_view.GetFilterEntry();
						if (!string.IsNullOrEmpty(filterEntry) && !this.m_launchedApplication.Contains(filterEntry))
						{
							this.m_view.SetFilterEntry("");
						}
						this.m_processesSelected.Clear();
						this.m_processesSelected.Add(new IdNamePair(pid, name));
						this.m_launchedApplication = "";
						this.InvalidateSelectedProcesses();
					}
					this.InvalidateProcessView();
					return;
				}
			}
			else if (name != processInfo.Name || warningFlags != processInfo.WarningFlags)
			{
				processInfo.Name = name;
				processInfo.WarningFlags = warningFlags;
				this.InvalidateProcessView();
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00019CA4 File Offset: 0x00017EA4
		private void RemoveProcess(uint pid)
		{
			this.m_processes.Remove(pid);
			SdpApp.ModelManager.DataSourcesModel.RemoveOptions(pid);
			List<IdNamePair> list = new List<IdNamePair>();
			foreach (KeyValuePair<IdNamePair, List<IdNamePair>> keyValuePair in SdpApp.ModelManager.TraceModel.CurrentSources)
			{
				List<IdNamePair> list2 = new List<IdNamePair>();
				foreach (IdNamePair idNamePair in keyValuePair.Value)
				{
					if (idNamePair.Id == pid)
					{
						list2.Add(idNamePair);
					}
				}
				foreach (IdNamePair idNamePair2 in list2)
				{
					keyValuePair.Value.Remove(idNamePair2);
				}
				if (keyValuePair.Value.Count == 0 && !SdpApp.ConnectionManager.GetMetricByID(keyValuePair.Key.Id).IsGlobal())
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (IdNamePair idNamePair3 in list)
			{
				SdpApp.ModelManager.TraceModel.CurrentSources.Remove(idNamePair3);
			}
			this.InvalidateProcessView();
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00019E4C File Offset: 0x0001804C
		private void AddMetric(Metric metric)
		{
			SnapshotController currentSnapshotController = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController;
			uint num = ((currentSnapshotController == null) ? 0U : currentSnapshotController.CurrentSnapshotProviderID);
			if (this.CaptureType == CaptureType.Snapshot && num != metric.GetProperties().providerID)
			{
				return;
			}
			if (!metric.GetProperties().hidden && (metric.GetProperties().captureTypeMask & (uint)this.CaptureType) > 0U)
			{
				DataSourcesViewMetric dataSourcesViewMetric = new DataSourcesViewMetric(metric, false);
				Dictionary<uint, List<DataSourcesViewMetric>> dictionary = new Dictionary<uint, List<DataSourcesViewMetric>>();
				DataSourcesViewMetric dataSourcesViewMetric2 = dataSourcesViewMetric;
				do
				{
					List<DataSourcesViewMetric> list = new List<DataSourcesViewMetric>();
					dictionary[dataSourcesViewMetric2.ParentId] = list;
					dataSourcesViewMetric2 = new DataSourcesViewMetric(MetricManager.Get().GetMetricCategory(dataSourcesViewMetric2.ParentId), metric.IsGlobal());
					list.Add(dataSourcesViewMetric2);
				}
				while (dataSourcesViewMetric2.ParentId != DataSourcesModel.GLOBAL_CATEGORY_ID && dataSourcesViewMetric2.ParentId != DataSourcesModel.PROCESS_CATEGORY_ID);
				if (metric.IsGlobal())
				{
					this.m_view.AddMetric(true, dataSourcesViewMetric, dictionary, SdpApp.ModelManager.DataSourcesModel.GetExpandedCategories(this.InferCaptureId()));
					return;
				}
				if (this.m_processesSelected != null)
				{
					foreach (IdNamePair idNamePair in this.m_processesSelected)
					{
						Process processByID = SdpApp.ConnectionManager.GetProcessByID(idNamePair.Id);
						if (processByID != null && processByID.IsMetricLinked(metric.GetProperties().id))
						{
							if (metric.IsActive(processByID.GetProperties().pid, (uint)this.CaptureType))
							{
								dataSourcesViewMetric.Enabled = true;
							}
							this.m_view.AddMetric(false, dataSourcesViewMetric, dictionary, SdpApp.ModelManager.DataSourcesModel.GetExpandedCategories(this.InferCaptureId()));
						}
					}
				}
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001A00C File Offset: 0x0001820C
		private void PopulateCategoryDictionary(Dictionary<uint, List<DataSourcesViewMetric>> categoryDictionary, List<uint> metricIds, uint processID)
		{
			SnapshotController currentSnapshotController = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController;
			uint num = ((currentSnapshotController == null) ? 0U : currentSnapshotController.CurrentSnapshotProviderID);
			foreach (uint num2 in metricIds)
			{
				Metric metric = MetricManager.Get().GetMetric(num2);
				if (!metric.GetProperties().hidden && metric != null && metric.GetProperties().categoryID > 0U && (metric.GetProperties().captureTypeMask & (uint)this.CaptureType) > 0U && (this.CaptureType != CaptureType.Snapshot || num == metric.GetProperties().providerID))
				{
					MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(metric.GetProperties().categoryID);
					if (metricCategory != null)
					{
						List<DataSourcesViewMetric> list;
						if (!categoryDictionary.TryGetValue(metricCategory.GetProperties().id, out list))
						{
							uint id = metricCategory.GetProperties().id;
							List<DataSourcesViewMetric> list2 = new List<DataSourcesViewMetric>();
							list2.Add(new DataSourcesViewMetric(metricCategory, metric.IsGlobal()));
							list = list2;
							categoryDictionary[id] = list2;
							while (metricCategory != null && metricCategory.GetProperties().parent != 0U)
							{
								metricCategory = MetricManager.Get().GetMetricCategory(metricCategory.GetProperties().parent);
								if (!categoryDictionary.ContainsKey(metricCategory.GetProperties().id))
								{
									categoryDictionary[metricCategory.GetProperties().id] = new List<DataSourcesViewMetric>
									{
										new DataSourcesViewMetric(metricCategory, metric.IsGlobal())
									};
								}
							}
						}
						bool flag2;
						if (!metric.IsGlobal() && this.CaptureType == CaptureType.Trace)
						{
							bool flag = true;
							foreach (IdNamePair idNamePair in this.m_processesSelected)
							{
								if (!metric.IsActive(idNamePair.Id, 2U))
								{
									flag = false;
									MetricIDSet metricIDSet = new MetricIDSet(metric.GetProperties().id, idNamePair.Id, metric.GetProperties().categoryID, this.InferCaptureId());
									SdpApp.ModelManager.DataSourcesModel.RemoveMetricColor(this.InferCaptureId(), metricIDSet);
									break;
								}
							}
							flag2 = flag;
						}
						else
						{
							flag2 = metric.IsActive(processID, (uint)this.CaptureType);
						}
						list.Add(new DataSourcesViewMetric(metric, flag2));
					}
				}
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001A2A4 File Offset: 0x000184A4
		private void InvalidateSelectedProcesses()
		{
			SelectedProcessChangedArgs selectedProcessChangedArgs = new SelectedProcessChangedArgs(this.ProcessesSelected);
			SdpApp.EventsManager.Raise<SelectedProcessChangedArgs>(SdpApp.EventsManager.DataSourceViewEvents.InvalidateSelectedProcesses, this, selectedProcessChangedArgs);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001A2D8 File Offset: 0x000184D8
		private uint InferCaptureId()
		{
			if (this.CaptureType == CaptureType.Realtime && SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController != null)
			{
				return SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.CaptureId;
			}
			if (this.CaptureType == CaptureType.Trace && SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				return SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.CaptureId;
			}
			if (this.CaptureType == CaptureType.Snapshot && SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
			{
				return SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CaptureId;
			}
			if (this.CaptureType == CaptureType.Sampling && SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
			{
				return SdpApp.ModelManager.SamplingModel.CurrentSamplingController.CaptureId;
			}
			return 0U;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001A3A4 File Offset: 0x000185A4
		private uint GetWarningFlags(Process process, CaptureType captureType)
		{
			switch (captureType)
			{
			case CaptureType.Realtime:
				return process.GetProperties().warningFlagsRealTime;
			case CaptureType.Trace:
				return process.GetProperties().warningFlagsTrace;
			case (CaptureType)3U:
				break;
			case CaptureType.Snapshot:
				return process.GetProperties().warningFlagsSnapshot;
			default:
				if (captureType == CaptureType.Sampling)
				{
					return process.GetProperties().warningFlagsSampling;
				}
				break;
			}
			return 0U;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001A40C File Offset: 0x0001860C
		private async Task<bool> ShowVulkanDebuggingWarning(string package)
		{
			TaskCompletionSource<bool> resultPromise = new TaskCompletionSource<bool>();
			ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
			showMessageDialogCommand.IconType = IconType.Warning;
			showMessageDialogCommand.Message = "'" + package + "' is not a debuggable app.\nIf this app uses Vulkan for rendering, GPU profiling will not be available.\nVulkan profiling requires the app to be debuggable or a rooted device.\n\nDo you wish to continue launching this app?";
			showMessageDialogCommand.ButtonLayout = ButtonLayout.YesNo;
			showMessageDialogCommand.OnCompleted = delegate(bool result)
			{
				resultPromise.SetResult(result);
			};
			SdpApp.CommandManager.ExecuteCommand(showMessageDialogCommand);
			return await resultPromise.Task;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001A450 File Offset: 0x00018650
		private async void ShowLaunchAppDialog()
		{
			if (this.m_captureType == CaptureType.Trace || this.m_captureType == CaptureType.Snapshot)
			{
				IGuidedSetupView guidedSetupView = this.m_view as IGuidedSetupView;
				if (guidedSetupView != null)
				{
					guidedSetupView.SelectNotebookPage(DataSourcesModel.LAUNCH_PAGE);
				}
			}
			else
			{
				using (IDisposable dialog = SdpApp.UIManager.CreateDialog("LaunchApplicationDialog") as IDisposable)
				{
					ILaunchApplicationDialog launchApplicationDialog = dialog as ILaunchApplicationDialog;
					if (launchApplicationDialog != null)
					{
						LaunchApplicationDialogController dialogController = new LaunchApplicationDialogController(launchApplicationDialog, this.CaptureType);
						bool flag = await dialogController.ShowDialog();
						bool flag2 = flag;
						if (flag2)
						{
							this.ProcessLaunchRequest(dialogController);
						}
						dialogController = null;
					}
				}
				IDisposable dialog = null;
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001A488 File Offset: 0x00018688
		private async void ProcessLaunchRequest(LaunchApplicationDialogController controller)
		{
			if (controller.IsAndroid && !controller.SelectedPackageDebuggable)
			{
				TaskAwaiter<bool> taskAwaiter = this.ShowVulkanDebuggingWarning(controller.SelectedPackage).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					return;
				}
			}
			global::Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice != null)
			{
				string text = Environment.NewLine + " " + Environment.NewLine;
				RenderingAPI renderingAPI;
				controller.TryGetParam<RenderingAPI>(LaunchApplicationDialogParam.RENDERING_APIS, RenderingAPI.None, out renderingAPI);
				Dictionary<string, bool> dictionary;
				controller.TryGetParam<Dictionary<string, bool>>(LaunchApplicationDialogParam.OPTIONS, new Dictionary<string, bool>(), out dictionary);
				string text2 = "";
				foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
				{
					text2 += string.Format("{0}:{1};", keyValuePair.Key, (keyValuePair.Value > false) ? 1 : 0);
				}
				AppStartSettings settings;
				LaunchHistory history;
				string msg;
				if (controller.IsAndroid)
				{
					msg = "Snapdragon Profiler was unable to connect to " + controller.SelectedPackageActivity + "." + text;
					msg += "Verify that the application has ";
					bool flag = false;
					if (renderingAPI == RenderingAPI.Vulkan)
					{
						text2 += string.Format("PerfHints:{0};", (SdpApp.ModelManager.DataSourcesModel.PerfHintsEnabled > false) ? 1 : 0);
						msg += "the debuggable attribute set, ";
						flag = true;
					}
					if ((renderingAPI != RenderingAPI.Vulkan && renderingAPI != RenderingAPI.OpenGL) || !SdpApp.ConnectionManager.IsConnectedDeviceRooted())
					{
						msg += "internet permission enabled, ";
						flag = true;
					}
					if (flag)
					{
						msg += "and ";
					}
					msg += "the correct GPU API selected.";
					string text3 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DisableUGD);
					text2 += string.Format("DisableUGDFlag:{0};", ((string.IsNullOrEmpty(text3) || !BoolConverter.Convert(text3)) > false) ? 1 : 0);
					string text4;
					controller.TryGetParam<string>(LaunchApplicationDialogParam.INTENT_ARGUMENTS, "", out text4);
					settings = new AppStartSettings(controller.SelectedPackageActivity, text4, Convert.ToUInt32(renderingAPI), (uint)this.CaptureType, text2);
					history = new LaunchHistory(controller.SelectedPackageActivity, text4, renderingAPI, text2, SdpApp.ConnectionManager.GetDeviceOS());
					this.m_launchedApplication = controller.SelectedPackage;
				}
				else
				{
					string text5;
					controller.TryGetParam<string>(LaunchApplicationDialogParam.EXECUTABLE_PATH, "", out text5);
					string text6;
					controller.TryGetParam<string>(LaunchApplicationDialogParam.WORKING_DIRECTORY, "", out text6);
					string text7;
					controller.TryGetParam<string>(LaunchApplicationDialogParam.COMMAND_LINE_ARGUMENTS, "", out text7);
					Dictionary<string, string> dictionary2;
					controller.TryGetParam<Dictionary<string, string>>(LaunchApplicationDialogParam.ENVIRONMENT_VARIABLES, new Dictionary<string, string>(), out dictionary2);
					string text8 = text6;
					if (!text8.EndsWith("/") && !text8.EndsWith("\\"))
					{
						text8 += (text8.Contains("/") ? "/" : "\\");
					}
					string text9 = "";
					foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
					{
						text9 += keyValuePair2.Key;
						text9 += "\u001f";
						text9 += keyValuePair2.Value;
						text9 += "\u001e";
					}
					settings = new AppStartSettings(text5.ToString(), text8, text7.ToString(), Convert.ToUInt32(renderingAPI), (uint)this.CaptureType, text9.ToString(), text2.ToString());
					history = new LaunchHistory(text5.ToString(), text8, text7.ToString(), renderingAPI, text2, text9, SdpApp.ConnectionManager.GetDeviceOS());
					msg = "Snapdragon Profiler was unable to launch the specified app." + text;
					msg += "Verify the path to the app is correct and the correct GPU API is selected.";
				}
				if (settings == null || !(await Task.Run<bool>(delegate
				{
					AppStartResponse appStartResponse = connectedDevice.StartApp(settings);
					SdpApp.ModelManager.DataSourcesModel.m_procStartSettings[(ulong)appStartResponse.pid] = settings;
					if (appStartResponse.result)
					{
						SdpApp.ModelManager.SettingsModel.UserPreferences.SaveLaunchedAppHistory(history);
					}
					return appStartResponse.result;
				})))
				{
					new ShowMessageDialogCommand
					{
						Message = msg,
						IconType = IconType.Warning
					}.Execute();
					msg = null;
				}
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001A4C7 File Offset: 0x000186C7
		private void launchApp_LaunchAppClicked(object sender, EventArgs e)
		{
			this.ProcessLaunchRequest(this.m_launchAppController);
			IGuidedSetupView guidedSetupView = this.m_view as IGuidedSetupView;
			if (guidedSetupView == null)
			{
				return;
			}
			guidedSetupView.SelectNotebookPage(DataSourcesModel.RUNNING_PAGE);
		}

		// Token: 0x040007D6 RID: 2006
		private GroupLayoutController m_container;

		// Token: 0x040007D7 RID: 2007
		private IDataSourcesView m_view;

		// Token: 0x040007D8 RID: 2008
		private LaunchApplicationDialogController m_launchAppController;

		// Token: 0x040007D9 RID: 2009
		private string m_launchedApplication = "";

		// Token: 0x040007DA RID: 2010
		private Dictionary<uint, DataSourcesController.ProcessInfo> m_processes = new Dictionary<uint, DataSourcesController.ProcessInfo>();

		// Token: 0x040007DB RID: 2011
		private List<IdNamePair> m_processesSelected;

		// Token: 0x040007DC RID: 2012
		private List<IdNamePair> m_metricsSelected;

		// Token: 0x040007DD RID: 2013
		private object m_mutex = new object();

		// Token: 0x040007DE RID: 2014
		private CaptureType m_captureType;

		// Token: 0x020003A7 RID: 935
		private class ProcessInfo
		{
			// Token: 0x0600121E RID: 4638 RVA: 0x00038C32 File Offset: 0x00036E32
			public ProcessInfo(string name, uint warningFlags)
			{
				this.Name = name;
				this.WarningFlags = warningFlags;
			}

			// Token: 0x04000CE4 RID: 3300
			public string Name;

			// Token: 0x04000CE5 RID: 3301
			public uint WarningFlags;
		}
	}
}
