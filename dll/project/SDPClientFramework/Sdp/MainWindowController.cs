using System;
using System.IO;
using Sdp.Views.SessionSettingsDialog;

namespace Sdp
{
	// Token: 0x02000288 RID: 648
	public class MainWindowController : IMainWindowController, IViewController
	{
		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06000B27 RID: 2855 RVA: 0x00020670 File Offset: 0x0001E870
		// (remove) Token: 0x06000B28 RID: 2856 RVA: 0x000206A8 File Offset: 0x0001E8A8
		public event EventHandler ViewClosing;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06000B29 RID: 2857 RVA: 0x000206E0 File Offset: 0x0001E8E0
		// (remove) Token: 0x06000B2A RID: 2858 RVA: 0x00020718 File Offset: 0x0001E918
		public event EventHandler ViewClosed;

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0002074D File Offset: 0x0001E94D
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00020758 File Offset: 0x0001E958
		public IDockHost DockingHost
		{
			get
			{
				IDockHost dockHost = null;
				if (this.m_view != null)
				{
					dockHost = this.m_view.DockingHost;
				}
				return dockHost;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0002077C File Offset: 0x0001E97C
		public static IWindow TopLevelWindow
		{
			get
			{
				return MainWindowController.m_topLevelWindow;
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00020784 File Offset: 0x0001E984
		public MainWindowController(IMainWindow view)
		{
			this.m_view = view;
			if (this.m_view != null)
			{
				this.m_view.ViewClosing += this.On_View_ViewClosing;
				this.m_view.ViewClosed += this.On_View_ViewClosed;
				this.InitMainMenu();
				this.InitMainToolBar();
				DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
				deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectACK));
				DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
				deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEvents_ClientDisconnectACK));
				ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
				clientEvents.CopyFocusedContent = (EventHandler<EventArgs>)Delegate.Combine(clientEvents.CopyFocusedContent, new EventHandler<EventArgs>(this.clientEvents_CopySelectedContent));
				ClientEvents clientEvents2 = SdpApp.EventsManager.ClientEvents;
				clientEvents2.SelectAllContent = (EventHandler<EventArgs>)Delegate.Combine(clientEvents2.SelectAllContent, new EventHandler<EventArgs>(this.clientEvents_SelectAllContent));
				ClientEvents clientEvents3 = SdpApp.EventsManager.ClientEvents;
				clientEvents3.SavingChanged = (EventHandler<SavingSession>)Delegate.Combine(clientEvents3.SavingChanged, new EventHandler<SavingSession>(this.clientEvents_savingChanged));
				ClientEvents clientEvents4 = SdpApp.EventsManager.ClientEvents;
				clientEvents4.CaptureWindowAdded = (EventHandler<CaptureWindowEventArgs>)Delegate.Combine(clientEvents4.CaptureWindowAdded, new EventHandler<CaptureWindowEventArgs>(this.clientEvents_CaptureAdded));
				ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
				connectionEvents.BufferTransferProgress = (EventHandler<BufferTransferProgressEventArgs>)Delegate.Combine(connectionEvents.BufferTransferProgress, new EventHandler<BufferTransferProgressEventArgs>(this.connectionEvents_DeviceFileTransferProgress));
				MainWindowController.m_topLevelWindow = this.m_view.TopLevelWindow;
				this.DockingHost.CaptureWindowHidden += this.dockingHost_CaptureWindowHidden;
				this.m_view.MainStatusBar.CaptureTabClicked += this.statusBar_CaptureTabClicked;
				this.m_view.MainStatusBar.RequestRename += this.statusBar_RequestRename;
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00020970 File Offset: 0x0001EB70
		public void ShowView()
		{
			if (this.m_view != null)
			{
				this.m_view.ShowView();
			}
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			if (!MainWindowController.SessionSettingsAreSet())
			{
				MainWindowController.GetSessionSelectionFromDialog();
			}
			else if (!Directory.Exists(userPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.SessionLocation)))
			{
				ShowMessageDialogCommand.ShowErrorDialog("Unable to write to the sessions folder. \nPlease go to File -> Settings -> Sessions and specify a valid directory. \nSnapdragon Profiler will not work correctly until it is addressed.\n" + userPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.SessionLocation));
			}
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ClientEvents.MainWindowShown, this, EventArgs.Empty);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000209F4 File Offset: 0x0001EBF4
		private static bool SessionSettingsAreSet()
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			return userPreferences.TryRetrieveSetting<string>(UserPreferenceModel.UserPreference.SessionLocation).Match2<double, bool>(userPreferences.TryRetrieveSetting<double>(UserPreferenceModel.UserPreference.MaxSessionsSizeMB), (string _sessionLocation, double _maxSize) => true, () => false);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00020A64 File Offset: 0x0001EC64
		private static void GetSessionSelectionFromDialog()
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			string text = userPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.SessionLocation);
			uint num = (uint)userPreferences.RetrieveSettingValueOrUseDefault<double>(UserPreferenceModel.UserPreference.MaxSessionsSizeMB);
			SessionSettingsSelection sessionSettingsSelection = (SdpApp.UIManager.CreateDialog("SessionSettingsDialog") as ISessionSettingsDialog).ShowDialog(num, text);
			userPreferences.RecordSetting(UserPreferenceModel.UserPreference.MaxSessionsSizeMB, sessionSettingsSelection.MaxSizeMB.ToString());
			userPreferences.RecordSetting(UserPreferenceModel.UserPreference.SessionLocation, sessionSettingsSelection.SessionRootPath);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00020AD1 File Offset: 0x0001ECD1
		public void HideView()
		{
			if (this.m_view != null)
			{
				this.m_view.HideView();
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00020AE8 File Offset: 0x0001ECE8
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

		// Token: 0x06000B34 RID: 2868 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00020B17 File Offset: 0x0001ED17
		public void AddMenuItem(MenuHeader header, MenuType type, IMenuItemController item, string group = "")
		{
			if (header == MenuHeader.Window)
			{
				this.m_view.MainMenu.SetMenuSensitive("Window", true);
			}
			this.m_view.MainMenu.AddMenuItem(header, type, item, group);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00020B48 File Offset: 0x0001ED48
		public void RemoveMenuItem(MenuHeader type, string view)
		{
			this.m_view.MainMenu.RemoveMenuItem(type, view);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00020B5C File Offset: 0x0001ED5C
		public void RenameMenuItem(string oldName, string newName)
		{
			this.m_view.MainMenu.RenameMenuItem(oldName, newName);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00020B70 File Offset: 0x0001ED70
		public void EnableMenuItems(MenuHeader header)
		{
			this.m_view.MainMenu.EnableMenuItems(header);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00020B83 File Offset: 0x0001ED83
		public void ToggleMenuItem(MenuHeader header, string name, bool toggled)
		{
			this.m_view.MainMenu.ToggleMenuItem(header, name, toggled);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00020B98 File Offset: 0x0001ED98
		private void InitMainMenu()
		{
			this.InitMainMenu_FileMenu();
			this.InitMainMenu_EditMenu();
			this.InitCaptureMenu();
			this.InitMainMenu_LayoutMenu();
			this.InitMainMenu_ViewMenu();
			this.InitMainMenu_WindowMenu();
			this.InitMainMenu_ToolsMenu();
			this.InitMainMenu_HelpMenu();
			this.InitMainStatusBar();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void InitMainToolBar()
		{
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00020BD0 File Offset: 0x0001EDD0
		private void InitMainMenu_FileMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_File");
			this.m_connectMenuItem = new MenuItemController("_Connect", new ChangeLayoutCommand("Connect"));
			mainMenu.AddMenuItem(MenuHeader.File, MenuType.Standard, this.m_connectMenuItem, "");
			mainMenu.AddSeparator(MenuHeader.File);
			mainMenu.AddMenuItem(MenuHeader.File, MenuType.Standard, new MenuItemController("_Load a Saved Session", new OpenSessionCommand()), "");
			mainMenu.AddMenuItem(MenuHeader.File, MenuType.Standard, new MenuItemController("_Save Session", new SaveSessionCommand()), "");
			mainMenu.AddMenuItem(MenuHeader.File, MenuType.Standard, new MenuItemController("Se_ttings", new SettingsCommand()), "");
			mainMenu.AddSeparator(MenuHeader.File);
			mainMenu.AddMenuItem(MenuHeader.File, MenuType.Standard, new MenuItemController("E_xit", new ExitAppCommand()), "");
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00020CA0 File Offset: 0x0001EEA0
		private void InitMainMenu_EditMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Edit");
			mainMenu.AddMenuItem(MenuHeader.Edit, MenuType.Standard, new CopyMenuItem
			{
				Enabled = false
			}, "");
			mainMenu.AddMenuItem(MenuHeader.Edit, MenuType.Standard, new SelectAllMenuItem
			{
				Enabled = false
			}, "");
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00020CFC File Offset: 0x0001EEFC
		private void InitCaptureMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Capture");
			mainMenu.AddMenuItem(MenuHeader.Capture, MenuType.Standard, new NewTraceCaptureMenuItem(), "");
			mainMenu.AddMenuItem(MenuHeader.Capture, MenuType.Standard, new NewSnapshotMenuItem(), "");
			mainMenu.AddMenuItem(MenuHeader.Capture, MenuType.Standard, new NewSamplingMenuItem(), "");
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00020D58 File Offset: 0x0001EF58
		private void InitMainMenu_LayoutMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Layout");
			mainMenu.AddMenuItem(MenuHeader.Layout, MenuType.Standard, new MenuItemController("Reset Layouts", new ResetLayoutsCommand()), "");
			mainMenu.AddSeparator(MenuHeader.Layout);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00020DA0 File Offset: 0x0001EFA0
		private void InitMainMenu_ViewMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_View");
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00020DC4 File Offset: 0x0001EFC4
		private void InitMainMenu_WindowMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Window");
			mainMenu.SetMenuSensitive("Window", false);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00020DF4 File Offset: 0x0001EFF4
		private void InitMainMenu_ToolsMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Tools");
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00020E18 File Offset: 0x0001F018
		private void InitMainMenu_HelpMenu()
		{
			IMenuBar mainMenu = this.m_view.MainMenu;
			mainMenu.AddTopLevelMenu("_Help");
			mainMenu.AddMenuItem(MenuHeader.Help, MenuType.Standard, new MenuItemController("Documentation", new UserGuideCommand()), "");
			mainMenu.AddSeparator(MenuHeader.Help);
			mainMenu.AddMenuItem(MenuHeader.Help, MenuType.Standard, new MenuItemController("Report Issue", new SubmitFeedbackCommand(null, null, null)), "");
			mainMenu.AddSeparator(MenuHeader.Help);
			mainMenu.AddMenuItem(MenuHeader.Help, MenuType.Standard, new MenuItemController("Licenses", new LicensesCommand()), "");
			mainMenu.AddMenuItem(MenuHeader.Help, MenuType.Standard, new MenuItemController("About", new AboutCommand()), "");
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00020EBD File Offset: 0x0001F0BD
		private void InitMainStatusBar()
		{
			if (this.m_view != null)
			{
				this.m_progressController = new ProgressController(this.m_view.MainStatusBar.ProgressView);
				this.m_connectionButtonController = new ConnectionButtonController(this.m_view.MainStatusBar.ConnectionButton);
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00020EFD File Offset: 0x0001F0FD
		private void On_View_ViewClosing(object sender, EventArgs e)
		{
			if (this.ViewClosing != null)
			{
				this.ViewClosing(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00020F18 File Offset: 0x0001F118
		private void On_View_ViewClosed(object sender, EventArgs e)
		{
			if (this.ViewClosed != null)
			{
				this.ViewClosed(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00020F33 File Offset: 0x0001F133
		private void deviceEvents_ClientConnectACK(object sender, EventArgs e)
		{
			if (this.m_view != null)
			{
				this.m_view.MainStatusBar.Connected = true;
				this.m_connectionButtonController.Update();
				this.m_connectMenuItem.Enabled = false;
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00020F65 File Offset: 0x0001F165
		private void deviceEvents_ClientDisconnectACK(object sender, EventArgs e)
		{
			if (this.m_view != null)
			{
				this.m_view.MainStatusBar.Connected = false;
				this.m_connectionButtonController.Update();
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00020F8B File Offset: 0x0001F18B
		private void clientEvents_CopySelectedContent(object sender, EventArgs e)
		{
			this.m_view.CopySelectedContent();
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00020F98 File Offset: 0x0001F198
		private void clientEvents_SelectAllContent(object sender, EventArgs e)
		{
			this.m_view.SelectAllContent();
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00020FA5 File Offset: 0x0001F1A5
		private void clientEvents_savingChanged(object sender, SavingSession e)
		{
			this.m_view.MainMenu.SetMenuSensitive("File", !e.saving);
			this.m_view.SavingChanged(e);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00020FD4 File Offset: 0x0001F1D4
		private void connectionEvents_DeviceFileTransferProgress(object sender, BufferTransferProgressEventArgs e)
		{
			if (e.BufferCategory != SDPCore.BUFFER_TYPE_DEVICE_FILE)
			{
				return;
			}
			MainWindowController.DeviceFileTransferProgress.CurrentValue = e.BytesReceived / e.TotalBytes;
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(MainWindowController.DeviceFileTransferProgress));
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002102E File Offset: 0x0001F22E
		private void clientEvents_CaptureAdded(object sender, CaptureWindowEventArgs e)
		{
			this.m_view.AddCaptureTab(e.Window);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00021041 File Offset: 0x0001F241
		private void statusBar_CaptureTabClicked(object sender, CaptureButtonArgs e)
		{
			SdpApp.UIManager.FocusCaptureWindow(e.Name, "");
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00021058 File Offset: 0x0001F258
		private void statusBar_RequestRename(object sender, RequestRenameArgs e)
		{
			SdpApp.UIManager.RequestRenameWindow(e.OldName, e.NewName);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00021070 File Offset: 0x0001F270
		public void FocusCaptureWindow(string windowName)
		{
			this.m_view.FocusCaptureTab(windowName);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002107E File Offset: 0x0001F27E
		private void dockingHost_CaptureWindowHidden(object sender, CaptureWindowFocusedArgs e)
		{
			this.m_view.HideCaptureTab(e.WindowName);
		}

		// Token: 0x040008BC RID: 2236
		private IMainWindow m_view;

		// Token: 0x040008BD RID: 2237
		public static ProgressObject SavingProgress;

		// Token: 0x040008BE RID: 2238
		public static ProgressObject DeviceFileTransferProgress;

		// Token: 0x040008BF RID: 2239
		private ProgressController m_progressController;

		// Token: 0x040008C0 RID: 2240
		private ConnectionButtonController m_connectionButtonController;

		// Token: 0x040008C1 RID: 2241
		private static IWindow m_topLevelWindow;

		// Token: 0x040008C2 RID: 2242
		private MenuItemController m_connectMenuItem;
	}
}
