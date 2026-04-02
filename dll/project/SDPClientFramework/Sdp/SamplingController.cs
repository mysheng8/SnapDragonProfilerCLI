using System;
using System.Linq;

namespace Sdp
{
	// Token: 0x020001EA RID: 490
	public class SamplingController : IViewController
	{
		// Token: 0x060006E3 RID: 1763 RVA: 0x00011E40 File Offset: 0x00010040
		public SamplingController(ISamplingView view)
		{
			this.m_view = view;
			this.m_dataSourcesController = new DataSourcesController(this.m_view.DataSourcesView);
			this.m_dataSourcesController.CaptureType = CaptureType.Sampling;
			this.m_view.CaptureButtonToggled += this.m_view_CaptureButtonToggled;
			this.m_view.NewCaptureButtonClicked += this.m_view_NewCaptureButtonClicked;
			this.m_view.SearchEntryChanged += this.m_view_SearchEntryChanged;
			this.m_view.ZoomIn += this.m_view_ZoomIn;
			this.m_view.ZoomOut += this.m_view_ZoomOut;
			this.m_view.ResetViewBounds += this.m_view_ResetViewBounds;
			this.m_view.ColorSchemeChanged += this.m_view_ColorSchemeChanged;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.CaptureCompleted = (EventHandler<CaptureCompletedEventArgs>)Delegate.Combine(connectionEvents.CaptureCompleted, new EventHandler<CaptureCompletedEventArgs>(this.connectionEvents_CaptureCompleted));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.CaptureWindowAdded = (EventHandler<CaptureWindowEventArgs>)Delegate.Combine(clientEvents.CaptureWindowAdded, new EventHandler<CaptureWindowEventArgs>(this.clientEvents_CaptureAdded));
			this.m_view.ZoomButtonsVisible = false;
			this.m_view.SearchEntryVisible = false;
			this.m_view.ColorSchemeVisible = false;
			this.m_view.NewCaptureButtonVisible = false;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00011FA3 File Offset: 0x000101A3
		public void StartCapture()
		{
			this.m_dataSourcesController.ReadOnly = true;
			this.m_view.HideDataSourcesPanel();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00011FBC File Offset: 0x000101BC
		public void StopCapture()
		{
			this.AlreadyCaptured = true;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00011FC5 File Offset: 0x000101C5
		public void SetSelectedProcess(IdNamePair process)
		{
			this.m_dataSourcesController.SetSelectedProcesses(process);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00011FD3 File Offset: 0x000101D3
		public void SetFilterEntry(string filterEntry)
		{
			if (!string.IsNullOrEmpty(filterEntry))
			{
				this.m_dataSourcesController.SetFilterEntry(filterEntry);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00011FE9 File Offset: 0x000101E9
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00011FF6 File Offset: 0x000101F6
		public bool DataSourcesVisible
		{
			get
			{
				return this.m_view.DataSourcesVisible;
			}
			set
			{
				this.m_view.DataSourcesVisible = value;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00012004 File Offset: 0x00010204
		private void m_view_CaptureButtonToggled(object sender, EventArgs e)
		{
			if (this.m_dataSourcesController.ProcessesSelected != null && this.m_dataSourcesController.ProcessesSelected.Count == 1)
			{
				if (SdpApp.ModelManager.SamplingModel.CurrentSources != null && SdpApp.ModelManager.SamplingModel.CurrentSources.Count > 0)
				{
					IdNamePair idNamePair = this.m_dataSourcesController.ProcessesSelected[0];
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(SdpApp.ModelManager.SamplingModel.CurrentSources.Keys.First<IdNamePair>().Id);
					IDList activeProcesses = metricByID.GetActiveProcesses(8U);
					bool flag = false;
					using (IDList.IDListEnumerator enumerator = activeProcesses.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							uint p = enumerator.Current;
							if (p != idNamePair.Id)
							{
								metricByID.Deactivate(p, 8U);
								IdNamePair idNamePair2 = new IdNamePair(metricByID.GetProperties().id, metricByID.GetProperties().name);
								if (SdpApp.ModelManager.SamplingModel.CurrentSources.ContainsKey(idNamePair2))
								{
									IdNamePair idNamePair3 = SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair2].Find((IdNamePair x) => x.Id == p);
									if (idNamePair3 != null)
									{
										SdpApp.ModelManager.SamplingModel.CurrentSources[idNamePair2].Remove(idNamePair3);
									}
								}
							}
							else
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						Process process = ProcessManager.Get().GetProcess(idNamePair.Id);
						if (metricByID != null && metricByID.IsValid() && process != null && process.IsValid() && process.IsMetricLinked(metricByID.GetProperties().id))
						{
							SampleCommand sampleCommand = new SampleCommand();
							sampleCommand.StartCapture = this.m_view.CaptureButtonActive;
							SdpApp.CommandManager.ExecuteCommand(sampleCommand);
							return;
						}
						this.m_dataSourcesController.SetStatus(StatusType.Warning, "Could not capture selected process", 3000);
					}
					else
					{
						this.m_dataSourcesController.SetStatus(StatusType.Neutral, "Choose a metric for the selected process", 3000);
					}
				}
				else
				{
					this.m_dataSourcesController.SetStatus(StatusType.Neutral, "Choose a metric for the selected process", 3000);
				}
			}
			else
			{
				this.m_dataSourcesController.SetStatus(StatusType.Neutral, "Select a process to capture", 3000);
			}
			this.m_view.CaptureButtonActive = false;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00012270 File Offset: 0x00010470
		private void m_view_NewCaptureButtonClicked(object sender, EventArgs e)
		{
			SdpApp.CommandManager.ExecuteCommand(new NewSamplingCommand());
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00012281 File Offset: 0x00010481
		private void m_view_SearchEntryChanged(object sender, SearchEntryChangedArgs args)
		{
			if (this.m_flameGraphController != null)
			{
				if (args.IsDefault)
				{
					this.m_flameGraphController.SearchNodes("");
					return;
				}
				this.m_flameGraphController.SearchNodes(args.Entry);
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000122B5 File Offset: 0x000104B5
		private void m_view_ColorSchemeChanged(object sender, ColorSchemeChangedArgs args)
		{
			if (this.m_flameGraphController != null)
			{
				this.m_flameGraphController.ChangeColorScheme(args.Active);
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000122D0 File Offset: 0x000104D0
		private void m_view_ZoomIn(object sender, ViewBoundsEventArgs e)
		{
			if (this.m_flameGraphController != null)
			{
				this.m_flameGraphController.ZoomIn();
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000122E5 File Offset: 0x000104E5
		private void m_view_ZoomOut(object sender, ViewBoundsEventArgs e)
		{
			if (this.m_flameGraphController != null)
			{
				this.m_flameGraphController.ZoomOut();
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000122FA File Offset: 0x000104FA
		private void m_view_ResetViewBounds(object sender, EventArgs e)
		{
			if (this.m_flameGraphController != null)
			{
				this.m_flameGraphController.ResetZoom();
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001230F File Offset: 0x0001050F
		private void connectionEvents_CaptureCompleted(object sender, CaptureCompletedEventArgs args)
		{
			if (args.CaptureId == this.CaptureId && !this.AlreadyCaptured)
			{
				this.StopCapture();
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001232D File Offset: 0x0001052D
		private void clientEvents_CaptureAdded(object sender, CaptureWindowEventArgs e)
		{
			if (sender == this)
			{
				e.Window.NameChanged += this.OnWindowNameChanged;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001234C File Offset: 0x0001054C
		public FlameGraphController AddFlameGraph(TreeModel model, int maxDepth, string title, float childrenPercent)
		{
			this.m_flameGraphController = new FlameGraphController(this.m_view.AddFlameGraph(model, maxDepth, title, childrenPercent), this);
			this.m_view.ZoomButtonsVisible = true;
			this.m_view.SearchEntryVisible = true;
			this.m_view.ColorSchemeVisible = true;
			return this.m_flameGraphController;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001239F File Offset: 0x0001059F
		IView IViewController.View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000123A7 File Offset: 0x000105A7
		public ViewDesc SaveSettings()
		{
			return null;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000123AA File Offset: 0x000105AA
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x000123B2 File Offset: 0x000105B2
		public string WindowName { get; set; }

		// Token: 0x060006F9 RID: 1785 RVA: 0x000123BC File Offset: 0x000105BC
		public void OnWindowNameChanged(object o, EventArgs e)
		{
			IDockWindow dockWindow = (IDockWindow)o;
			this.WindowName = dockWindow.Name;
			SdpApp.ConnectionManager.SetCaptureName(this.CaptureId, this.WindowName);
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x000123F2 File Offset: 0x000105F2
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x000123FA File Offset: 0x000105FA
		public uint CaptureId { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00012403 File Offset: 0x00010603
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0001240B File Offset: 0x0001060B
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
					this.m_view.NewCaptureButtonVisible = SdpApp.ConnectionManager.IsConnected();
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001243D File Offset: 0x0001063D
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00012472 File Offset: 0x00010672
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0001247A File Offset: 0x0001067A
		public string FilterEntry { get; set; }

		// Token: 0x0400070E RID: 1806
		private bool m_alreadyCaptured;

		// Token: 0x04000710 RID: 1808
		private ISamplingView m_view;

		// Token: 0x04000711 RID: 1809
		private DataSourcesController m_dataSourcesController;

		// Token: 0x04000712 RID: 1810
		private FlameGraphController m_flameGraphController;
	}
}
