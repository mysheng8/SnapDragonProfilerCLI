using System;
using System.IO;
using System.Linq;

namespace Sdp
{
	// Token: 0x0200027A RID: 634
	public class SnapshotController : IViewController
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
		public SnapshotController(ISnapshotView view)
		{
			this.m_view = view;
			this.m_screenCaptureController = new ScreenCaptureViewController(this.m_view.ScreenCaptureView);
			this.m_dataSourcesController = new DataSourcesController(this.m_view.DataSourcesView);
			this.m_dataSourcesController.CaptureType = CaptureType.Snapshot;
			this.m_view.CaptureClicked += this.m_view_CaptureClicked;
			this.m_view.CancelClicked += this.m_view_CancelClicked;
			this.m_view.NewSnapshotClicked += this.m_view_NewSnapshotClicked;
			this.m_view.DataSourcesView.FilterEntryChanged += this.m_dataSourceView_FilterEntryChanged;
			this.m_view.DataSourcesView.SelectedProcessesChanged += this.m_dataSourceView_SelectedProcessesChanged;
			this.m_view.NewSnapshotButtonVisible = SdpApp.ConnectionManager.IsConnected();
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectDisconnect));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEvents_ClientConnectDisconnect));
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.AddAvailableSnapshotMode = (EventHandler<AddSnapshotModeArgs>)Delegate.Combine(snapshotEvents.AddAvailableSnapshotMode, new EventHandler<AddSnapshotModeArgs>(this.OnAddSnapshotMode));
			SnapshotEvents snapshotEvents2 = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents2.AddMainWindowErrorMessage = (EventHandler<AddMainWindowErrorMessageArgs>)Delegate.Combine(snapshotEvents2.AddMainWindowErrorMessage, new EventHandler<AddMainWindowErrorMessageArgs>(this.snapshotEvents_AddMainWindowErrorMessage));
			ScreenCaptureViewEvents screenCaptureViewEvents = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents.DisplayScreenCapture = (EventHandler<ScreenCaptureViewDisplayEventArgs>)Delegate.Combine(screenCaptureViewEvents.DisplayScreenCapture, new EventHandler<ScreenCaptureViewDisplayEventArgs>(this.screenCaptureEvents_DisplayScreenCapture));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.CaptureWindowAdded = (EventHandler<CaptureWindowEventArgs>)Delegate.Combine(clientEvents.CaptureWindowAdded, new EventHandler<CaptureWindowEventArgs>(this.clientEvents_CaptureAdded));
			this.m_snapshotProviderID.OnValueChanged += delegate(object sender, ObservableObject<uint>.ValueChangedArgs args)
			{
				SdpApp.EventsManager.Raise<SnapshotProviderChangedArgs>(SdpApp.EventsManager.SnapshotEvents.SnapshotProviderChanged, this, new SnapshotProviderChangedArgs
				{
					SelectedProvider = this.m_snapshotProviderID.Value
				});
			};
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0001FBC9 File Offset: 0x0001DDC9
		public void SetName(int sessionNumber, string windowName)
		{
			this.m_view.SetName(sessionNumber);
			this.m_currentWindowName = windowName;
		}

		// Token: 0x17000214 RID: 532
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x0001FBDE File Offset: 0x0001DDDE
		public bool DataSourcesVisible
		{
			set
			{
				this.m_view.DataSourcesVisible = value;
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		public void DetachEvents()
		{
			this.m_dataSourcesController.DetachEvents();
			this.m_screenCaptureController.DetachEvents();
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.AddAvailableSnapshotMode = (EventHandler<AddSnapshotModeArgs>)Delegate.Remove(snapshotEvents.AddAvailableSnapshotMode, new EventHandler<AddSnapshotModeArgs>(this.OnAddSnapshotMode));
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0001FC3C File Offset: 0x0001DE3C
		public void AttachOpenSessionEvents()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OpenSnapshotFromSessionResult = (EventHandler<OpenSnapshotFromSessionResultArgs>)Delegate.Combine(connectionEvents.OpenSnapshotFromSessionResult, new EventHandler<OpenSnapshotFromSessionResultArgs>(this.connectionEvents_OpenSnapshotFromSessionResult));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.OpenSnapshotFromSession = (EventHandler<OpenSnapshotFromSessionArgs>)Delegate.Combine(connectionEvents2.OpenSnapshotFromSession, new EventHandler<OpenSnapshotFromSessionArgs>(this.connectionEvents_OpenSnapshotFromSession));
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0001FC9F File Offset: 0x0001DE9F
		public void SetSelectedProcess(IdNamePair process)
		{
			this.m_dataSourcesController.SetSelectedProcesses(process);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0001FCAD File Offset: 0x0001DEAD
		public void SetFilterEntry(string filterEntry)
		{
			if (!string.IsNullOrEmpty(filterEntry))
			{
				this.m_dataSourcesController.SetFilterEntry(filterEntry);
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0001FCC3 File Offset: 0x0001DEC3
		private void deviceEvents_ClientConnectDisconnect(object sender, EventArgs e)
		{
			if (this.AlreadyCaptured)
			{
				this.m_view.NewSnapshotButtonVisible = SdpApp.ConnectionManager.IsConnected();
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001FCE2 File Offset: 0x0001DEE2
		private void OnAddSnapshotMode(object sender, AddSnapshotModeArgs e)
		{
			if (this.m_snapshotProviderID.Value == 0U)
			{
				this.m_snapshotProviderID.Value = e.ProviderID;
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001FD02 File Offset: 0x0001DF02
		private void m_view_NewSnapshotClicked(object sender, EventArgs e)
		{
			SdpApp.CommandManager.ExecuteCommand(new NewSnapshotCommand());
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0001FD14 File Offset: 0x0001DF14
		private void m_view_CaptureClicked(object sender, EventArgs e)
		{
			if (this.m_dataSourcesController.ProcessesSelected == null || this.m_dataSourcesController.ProcessesSelected.Count != 1)
			{
				if (this.m_dataSourcesController.ProcessesSelected == null || this.m_dataSourcesController.ProcessesSelected.Count == 0)
				{
					this.m_dataSourcesController.SetStatus(StatusType.Warning, "Select a process to capture", 3000);
				}
				if (this.m_dataSourcesController.ProcessesSelected != null && this.m_dataSourcesController.ProcessesSelected.Count > 1)
				{
					this.m_dataSourcesController.SetStatus(StatusType.Warning, "Only one process per snapshot is supported", 3000);
				}
				return;
			}
			IdNamePair idNamePair = this.m_dataSourcesController.ProcessesSelected.First<IdNamePair>();
			if (this.m_snapshotProviderID.Value != 0U)
			{
				SnapshotRequestArgs snapshotRequestArgs = new SnapshotRequestArgs();
				snapshotRequestArgs.SelectedPID = idNamePair.Id;
				snapshotRequestArgs.SelectedProviderID = this.m_snapshotProviderID.Value;
				SdpApp.EventsManager.Raise<SnapshotRequestArgs>(SdpApp.EventsManager.SnapshotEvents.CaptureClicked, this, snapshotRequestArgs);
				SdpApp.UIManager.SetWindowName(this.m_currentWindowName, " - [" + Path.GetFileName(idNamePair.DisplayName) + "]");
				SdpApp.UIManager.FocusCaptureWindow(this.WindowName, "Snapshot");
				return;
			}
			Process processByID = SdpApp.ConnectionManager.GetProcessByID(idNamePair.Id);
			if ((processByID.GetProperties().warningFlagsSnapshot & 8U) != 0U)
			{
				this.m_dataSourcesController.RequestRestartForWrongLayer();
			}
			this.m_dataSourcesController.SetStatus(StatusType.Warning, "Can not capture selected process", 3000);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0001FE90 File Offset: 0x0001E090
		private void m_dataSourceView_FilterEntryChanged(object sender, FilterEntryChangedArgs e)
		{
			this.FilterEntry = e.FilterEntry;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0001FE9E File Offset: 0x0001E09E
		private void m_dataSourceView_SelectedProcessesChanged(object sender, SelectedProcessChangedArgs e)
		{
			if (this.AlreadyCaptured)
			{
				return;
			}
			if (e.SelectedProcesses.Count == 0)
			{
				return;
			}
			this.m_snapshotProviderID.Value = 0U;
			SdpApp.EventsManager.Raise<SelectedProcessChangedArgs>(SdpApp.EventsManager.SnapshotEvents.SelectedProcessChanged, this, e);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0001FEE0 File Offset: 0x0001E0E0
		private void m_view_CancelClicked(object sender, EventArgs e)
		{
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.CancelSnapshotRequest, this, EventArgs.Empty);
			this.CanCancel = false;
			this.AlreadyCaptured = false;
			this.m_dataSourcesController.ReadOnly = false;
			this.m_view.DataSourcesVisible = true;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0001FF32 File Offset: 0x0001E132
		private void connectionEvents_OpenSnapshotFromSession(object sender, OpenSnapshotFromSessionArgs e)
		{
			this.m_view.ShowWaitingDialog();
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OpenSnapshotFromSession = (EventHandler<OpenSnapshotFromSessionArgs>)Delegate.Remove(connectionEvents.OpenSnapshotFromSession, new EventHandler<OpenSnapshotFromSessionArgs>(this.connectionEvents_OpenSnapshotFromSession));
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0001FF6C File Offset: 0x0001E16C
		private void connectionEvents_OpenSnapshotFromSessionResult(object sender, OpenSnapshotFromSessionResultArgs e)
		{
			this.m_openSnapshotFromSessionResultCount += 1U;
			if (!e.Handled && (ulong)this.m_openSnapshotFromSessionResultCount < (ulong)((long)SdpApp.ModelManager.SnapshotModel.NumSnapshotHandlers))
			{
				return;
			}
			if (e.Handled && e.Error == null)
			{
				this.m_view.ShowResultDialog(true, "Snapshot opened successfully.\n \n" + e.Warning);
			}
			else if (e.Handled)
			{
				this.m_view.ShowResultDialog(false, e.Error + "\n \n" + e.Warning);
			}
			else
			{
				this.m_view.ShowResultDialog(false, "Not a valid capture");
			}
			this.m_openSnapshotFromSessionResultCount = 0U;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OpenSnapshotFromSessionResult = (EventHandler<OpenSnapshotFromSessionResultArgs>)Delegate.Remove(connectionEvents.OpenSnapshotFromSessionResult, new EventHandler<OpenSnapshotFromSessionResultArgs>(this.connectionEvents_OpenSnapshotFromSessionResult));
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00020046 File Offset: 0x0001E246
		private void snapshotEvents_AddMainWindowErrorMessage(object sender, AddMainWindowErrorMessageArgs args)
		{
			if (args.CaptureId == this.m_captureID)
			{
				this.m_view.AddMainWindowErrorMessage(args.Message);
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00020067 File Offset: 0x0001E267
		private void screenCaptureEvents_DisplayScreenCapture(object sender, ScreenCaptureViewDisplayEventArgs args)
		{
			if (args.CaptureID == this.m_captureID)
			{
				this.m_view.HideMainWindowErrorMessage();
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00020082 File Offset: 0x0001E282
		private void clientEvents_CaptureAdded(object sender, CaptureWindowEventArgs e)
		{
			if (sender == this)
			{
				e.Window.NameChanged += this.OnWindowNameChanged;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0002009F File Offset: 0x0001E29F
		IView IViewController.View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000123A7 File Offset: 0x000105A7
		public ViewDesc SaveSettings()
		{
			return null;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000200A7 File Offset: 0x0001E2A7
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x000200AF File Offset: 0x0001E2AF
		public string WindowName { get; set; }

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000200B8 File Offset: 0x0001E2B8
		public void OnWindowNameChanged(object o, EventArgs e)
		{
			IDockWindow dockWindow = (IDockWindow)o;
			this.WindowName = dockWindow.Name;
			SdpApp.ConnectionManager.SetCaptureName(this.CaptureId, this.WindowName);
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x000200EE File Offset: 0x0001E2EE
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x000200F8 File Offset: 0x0001E2F8
		public uint CaptureId
		{
			get
			{
				return this.m_captureID;
			}
			set
			{
				this.m_captureID = value;
				this.m_screenCaptureController.CaptureID = value;
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					SdpApp.ModelManager.DataSourcesModel.CopyExpandedCategories(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CaptureId, this.m_captureID, false);
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00020153 File Offset: 0x0001E353
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x0002015C File Offset: 0x0001E35C
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
					this.m_dataSourcesController.ReadOnly = true;
					this.m_view.DataSourcesVisible = false;
					this.m_view.CaptureButtonEnabled = false;
					this.m_view.NewSnapshotButtonSensitive = true;
				}
			}
		}

		// Token: 0x17000219 RID: 537
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x000201A8 File Offset: 0x0001E3A8
		public bool CanCancel
		{
			set
			{
				this.m_view.CancelCaptureButtonVisible = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000201B6 File Offset: 0x0001E3B6
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000201EB File Offset: 0x0001E3EB
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x000201F3 File Offset: 0x0001E3F3
		public string FilterEntry { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000201FC File Offset: 0x0001E3FC
		public uint CurrentSnapshotProviderID
		{
			get
			{
				return this.m_snapshotProviderID.Value;
			}
		}

		// Token: 0x0400089E RID: 2206
		private ObservableObject<uint> m_snapshotProviderID = new ObservableObject<uint>(0U);

		// Token: 0x0400089F RID: 2207
		private const uint UNSELECTED = 0U;

		// Token: 0x040008A0 RID: 2208
		private bool m_alreadyCaptured;

		// Token: 0x040008A1 RID: 2209
		private string m_currentWindowName;

		// Token: 0x040008A2 RID: 2210
		private ISnapshotView m_view;

		// Token: 0x040008A3 RID: 2211
		private DataSourcesController m_dataSourcesController;

		// Token: 0x040008A4 RID: 2212
		private ScreenCaptureViewController m_screenCaptureController;

		// Token: 0x040008A5 RID: 2213
		private uint m_openSnapshotFromSessionResultCount;

		// Token: 0x040008A6 RID: 2214
		private uint m_captureID;
	}
}
