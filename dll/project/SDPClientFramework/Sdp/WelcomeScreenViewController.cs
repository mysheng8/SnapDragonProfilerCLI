using System;

namespace Sdp
{
	// Token: 0x020002CD RID: 717
	public class WelcomeScreenViewController : IViewController
	{
		// Token: 0x06000E98 RID: 3736 RVA: 0x0002D070 File Offset: 0x0002B270
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

		// Token: 0x06000E99 RID: 3737 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0002D09F File Offset: 0x0002B29F
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0002D0A8 File Offset: 0x0002B2A8
		public WelcomeScreenViewController(IWelcomeScreenView view)
		{
			this.m_view = view;
			this.m_view.ActionSelected += this.m_view_ActionSelected;
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectACK));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEvents_ClientDisconnectACK));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.EnableAction = (EventHandler<EnableActionArgs>)Delegate.Combine(connectionEvents.EnableAction, new EventHandler<EnableActionArgs>(this.deviceEvents_EnableAction));
			PluginEvents pluginEvents = SdpApp.EventsManager.PluginEvents;
			pluginEvents.RegisterToolPlugin = (EventHandler)Delegate.Combine(pluginEvents.RegisterToolPlugin, new EventHandler(this.pluginEvents_RegisterToolPlugin));
			WelcomeScreenAction welcomeScreenAction = new WelcomeScreenAction();
			welcomeScreenAction.Icon = "Sdp.Resources.group_fff_32.png";
			welcomeScreenAction.Title = SDPLocalization.GetString("Start a Session");
			welcomeScreenAction.Description = SDPLocalization.GetString("Connect to a device and start a profiling session");
			welcomeScreenAction.Action = ActionEnum.ConnectToDevice;
			welcomeScreenAction.NeedsConnectedDevice = false;
			welcomeScreenAction.DeviceErrorCheckMask = 0U;
			welcomeScreenAction.DeviceCheckFailureMessage = welcomeScreenAction.Description;
			this.m_view.AddAction(welcomeScreenAction);
			WelcomeScreenAction welcomeScreenAction2 = new WelcomeScreenAction();
			welcomeScreenAction2.Icon = "Sdp.Resources.graph-view_fff_32.png";
			welcomeScreenAction2.Title = "Realtime Performance Analysis";
			welcomeScreenAction2.Description = "Visualize realtime performance metrics";
			welcomeScreenAction2.Action = ActionEnum.Realtime;
			welcomeScreenAction2.NeedsConnectedDevice = true;
			welcomeScreenAction2.DeviceErrorCheckMask = 2U;
			welcomeScreenAction2.DeviceCheckFailureMessage = welcomeScreenAction2.Description;
			this.m_view.AddAction(welcomeScreenAction2);
			this.m_newCaptureAction = new WelcomeScreenAction();
			this.m_newCaptureAction.Icon = "Sdp.Resources.chart_gantt_fff_32.png";
			this.m_newCaptureAction.Title = SDPLocalization.GetString("System Trace Analysis");
			this.m_newCaptureAction.Description = SDPLocalization.GetString("Analyze system utilization and activity");
			this.m_newCaptureAction.Action = ActionEnum.NewCapture;
			this.m_newCaptureAction.NeedsConnectedDevice = true;
			this.m_newCaptureAction.DeviceErrorCheckMask = 4U;
			this.m_newCaptureAction.DeviceCheckFailureMessage = this.m_newCaptureAction.Description;
			this.m_view.AddAction(this.m_newCaptureAction);
			WelcomeScreenAction welcomeScreenAction3 = new WelcomeScreenAction();
			welcomeScreenAction3.Icon = "Sdp.Resources.snapshot_fff_32.png";
			welcomeScreenAction3.Title = "Snapshot GPU Frame Capture";
			welcomeScreenAction3.Description = "GPU frame debugging and performance analysis";
			welcomeScreenAction3.Action = ActionEnum.Snapshot;
			welcomeScreenAction3.NeedsConnectedDevice = true;
			welcomeScreenAction3.DeviceErrorCheckMask = 8U;
			welcomeScreenAction3.DeviceCheckFailureMessage = welcomeScreenAction3.Description;
			this.m_view.AddAction(welcomeScreenAction3);
			WelcomeScreenAction welcomeScreenAction4 = new WelcomeScreenAction();
			welcomeScreenAction4.Icon = "Sdp.Resources.graph-scatter_fff_32.png";
			welcomeScreenAction4.Title = "CPU Sampling";
			welcomeScreenAction4.Description = "Flame graph visualization of CPU execution";
			welcomeScreenAction4.Action = ActionEnum.Sampling;
			welcomeScreenAction4.NeedsConnectedDevice = true;
			welcomeScreenAction4.DeviceErrorCheckMask = 17U;
			welcomeScreenAction4.DeviceCheckFailureMessage = "Sampling captures are only available on Android devices and require the Android NDK simpleperf tool (version 13 or later). Set the location of the Android NDK in File->Settings->Android then restart Snapdragon Profiler.";
			this.m_view.AddAction(welcomeScreenAction4);
			WelcomeScreenAction welcomeScreenAction5 = new WelcomeScreenAction();
			welcomeScreenAction5.Icon = "Sdp.Resources.file-input_fff_32.png";
			welcomeScreenAction5.Title = SDPLocalization.GetString("Load a Saved Session");
			welcomeScreenAction5.Description = SDPLocalization.GetString("Load a previously saved Snapdragon Profiler session file.");
			welcomeScreenAction5.Action = ActionEnum.LoadSavedSession;
			welcomeScreenAction5.NeedsConnectedDevice = false;
			welcomeScreenAction5.DeviceErrorCheckMask = 0U;
			welcomeScreenAction5.DeviceCheckFailureMessage = welcomeScreenAction5.Description;
			this.m_view.AddAction(welcomeScreenAction5);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
		private void m_view_ActionSelected(object sender, ActionSelectedEventArgs e)
		{
			WelcomeScreenAction selected = e.Selected;
			switch (selected.Action)
			{
			case ActionEnum.Realtime:
				SdpApp.UIManager.FocusCaptureWindow(SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.WindowName, "");
				return;
			case ActionEnum.ConnectToDevice:
				SdpApp.CommandManager.ExecuteCommand(new ChangeLayoutCommand("Connect"));
				return;
			case ActionEnum.NewCapture:
				SdpApp.CommandManager.ExecuteCommand(new NewCaptureCommand());
				return;
			case ActionEnum.OpenCapture:
			case ActionEnum.ImportCapture:
				break;
			case ActionEnum.Snapshot:
				SdpApp.CommandManager.ExecuteCommand(new NewSnapshotCommand());
				return;
			case ActionEnum.Sampling:
				SdpApp.CommandManager.ExecuteCommand(new NewSamplingCommand());
				return;
			case ActionEnum.LoadSavedSession:
				SdpApp.CommandManager.ExecuteCommand(new OpenSessionCommand());
				return;
			case ActionEnum.ImportGfxrCapture:
				if (this.m_gfxReconstructImportCommand != null)
				{
					SdpApp.CommandManager.ExecuteCommand(this.m_gfxReconstructImportCommand);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0002D4B0 File Offset: 0x0002B6B0
		private void pluginEvents_RegisterToolPlugin(object sender, EventArgs e)
		{
			IToolPlugin toolPlugin = sender as IToolPlugin;
			if (toolPlugin != null && toolPlugin.Name == "Import GfxReconstruct Capture")
			{
				this.m_gfxReconstructImportCommand = new LaunchToolCommand
				{
					ToolPlugin = toolPlugin
				};
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0002D4EC File Offset: 0x0002B6EC
		private void deviceEvents_ClientConnectACK(object sender, EventArgs e)
		{
			this.m_view.RemoveAction(ActionEnum.ConnectToDevice);
			uint num = 0U;
			num |= ((!DeviceManager.Get().IsSimpleperfAvailable()) ? 1U : 0U);
			num |= (SdpApp.ConnectionManager.SupportsCaptureType(CaptureType.Sampling) ? 0U : 16U);
			num |= (SdpApp.ConnectionManager.SupportsCaptureType(CaptureType.Realtime) ? 0U : 2U);
			num |= (SdpApp.ConnectionManager.SupportsCaptureType(CaptureType.Trace) ? 0U : 4U);
			num |= (SdpApp.ConnectionManager.SupportsCaptureType(CaptureType.Snapshot) ? 0U : 8U);
			if (this.m_gfxReconstructImportCommand == null)
			{
				num |= 32U;
			}
			this.m_view.Connected(num);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0002D581 File Offset: 0x0002B781
		private void deviceEvents_ClientDisconnectACK(object sender, EventArgs e)
		{
			this.m_view.Disconnected();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0002D58E File Offset: 0x0002B78E
		private void deviceEvents_EnableAction(object sender, EnableActionArgs e)
		{
			this.m_view.ShowButton(e.action);
		}

		// Token: 0x040009DC RID: 2524
		private IWelcomeScreenView m_view;

		// Token: 0x040009DD RID: 2525
		private WelcomeScreenAction m_newCaptureAction;

		// Token: 0x040009DE RID: 2526
		private LaunchToolCommand m_gfxReconstructImportCommand;
	}
}
