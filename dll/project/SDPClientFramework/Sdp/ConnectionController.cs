using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001B2 RID: 434
	public class ConnectionController : IViewController, IDisposable
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		public ConnectionController(IConnectionView view)
		{
			this.m_view = view;
			if (this.m_view == null)
			{
				return;
			}
			if (ConnectionController.IsAutoConnectSet)
			{
				this.m_view.AutoConnect = true;
			}
			this.m_view.ConnectClicked += this.connectionView_ConnectClicked;
			this.m_view.SelectedDeviceChanged += this.connectionView_SelectedDeviceChanged;
			this.m_view.AutoConnectClicked += this.connectionView_AutoConnectClicked;
			this.m_view.EditDeviceClicked += this.connectionView_EditDeviceClicked;
			this.m_view.DeleteDeviceClicked += this.connectionView_DeleteDeviceClicked;
			this.m_view.AddDeviceClicked += this.connectionView_AddDeviceClicked;
			this.m_view.ImportClicked += this.ConnectionView_ImportClicked;
			this.m_view.ConfigureDeviceComplete += this.connectionEvents_ConfigurationComplete;
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.DeviceListChanged = (EventHandler)Delegate.Combine(deviceEvents.DeviceListChanged, new EventHandler(this.deviceEvents_DeviceListChanged));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.SavingChanged = (EventHandler<SavingSession>)Delegate.Combine(clientEvents.SavingChanged, new EventHandler<SavingSession>(this.clientEvents_savingChanged));
			SdpApp.UIManager.MainWindowClosing += this.MainWindowClosing;
			this.InvalidateDevices();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0000C628 File Offset: 0x0000A828
		public void Dispose()
		{
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.DeviceListChanged = (EventHandler)Delegate.Remove(deviceEvents.DeviceListChanged, new EventHandler(this.deviceEvents_DeviceListChanged));
			DeviceList devices = DeviceManager.Get().GetDevices();
			if (devices != null)
			{
				foreach (Device device in devices)
				{
					string lookupName = device.GetName();
					InternalDeviceDelegate internalDeviceDelegate = this.m_deviceDelegates.Find((InternalDeviceDelegate d) => d.Name == lookupName);
					if (internalDeviceDelegate != null)
					{
						device.DeregisterEventDelegate(internalDeviceDelegate);
					}
				}
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000C6DC File Offset: 0x0000A8DC
		public void InvalidateDeviceState(string name, string guid, bool fromDevice = false)
		{
			InternalDeviceDelegate internalDeviceDelegate = this.m_deviceDelegates.Find((InternalDeviceDelegate d) => d.Name == name);
			if (internalDeviceDelegate != null)
			{
				DeviceConnectionState previousState = internalDeviceDelegate.PreviousState;
				DeviceState deviceState;
				switch (internalDeviceDelegate.CurrentState)
				{
				case DeviceConnectionState.Scanning:
				case DeviceConnectionState.Discovered:
				case DeviceConnectionState.Installing:
					deviceState = DeviceState.Installing;
					goto IL_00BD;
				case DeviceConnectionState.InstallFailed:
					deviceState = DeviceState.InstallFailed;
					goto IL_00BD;
				case DeviceConnectionState.Ready:
					if (previousState == DeviceConnectionState.Connecting)
					{
						deviceState = DeviceState.ConnectingError;
						goto IL_00BD;
					}
					if (previousState == DeviceConnectionState.Disconnecting)
					{
						deviceState = DeviceState.Unknown;
						DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
						deviceEvents.DeviceListChanged = (EventHandler)Delegate.Remove(deviceEvents.DeviceListChanged, new EventHandler(this.deviceEvents_DeviceListChanged));
						goto IL_00BD;
					}
					deviceState = DeviceState.Ready;
					goto IL_00BD;
				case DeviceConnectionState.Connecting:
					deviceState = DeviceState.Connecting;
					goto IL_00BD;
				case DeviceConnectionState.Connected:
					deviceState = DeviceState.Connected;
					goto IL_00BD;
				}
				deviceState = DeviceState.Unknown;
				IL_00BD:
				if (fromDevice && deviceState == DeviceState.Ready && DeviceManager.Get().GetNumKnownDevices() == 1U && ConnectionController.IsAutoConnectSet)
				{
					this.m_view.SelectedDevice = name;
					DeviceEventArgs deviceEventArgs = new DeviceEventArgs();
					deviceEventArgs.LookupName = name;
					SdpApp.EventsManager.Raise<DeviceEventArgs>(SdpApp.EventsManager.DeviceEvents.ConnectToDevice, this, deviceEventArgs);
				}
				if (this.m_view != null)
				{
					Device device = DeviceManager.Get().GetDevice(name);
					string text = ((device != null) ? device.GetDeviceStateMsg() : string.Empty);
					this.m_view.UpdateDeviceState(guid, deviceState, text);
				}
				this.InvalidateConnectButton(deviceState);
				return;
			}
			this.InvalidateConnectButton(DeviceState.Unknown);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000C854 File Offset: 0x0000AA54
		private void InvalidateDevices()
		{
			if (this.m_view != null)
			{
				string text = this.m_view.LastSelectedDevice;
				List<InternalDeviceDelegate> list = new List<InternalDeviceDelegate>();
				foreach (InternalDeviceDelegate internalDeviceDelegate in this.m_deviceDelegates)
				{
					if (DeviceManager.Get().GetDevice(internalDeviceDelegate.Name) == null)
					{
						list.Add(internalDeviceDelegate);
						if (text == internalDeviceDelegate.Name)
						{
							text = null;
						}
					}
				}
				foreach (InternalDeviceDelegate internalDeviceDelegate2 in list)
				{
					this.m_deviceDelegates.Remove(internalDeviceDelegate2);
				}
				this.m_view.Clear();
				DeviceList devices = DeviceManager.Get().GetDevices();
				if (devices != null && devices.Count > 0)
				{
					SortedList<int, Device> sortedList = new SortedList<int, Device>();
					foreach (Device device in devices)
					{
						string lookupName = device.GetName();
						InternalDeviceDelegate internalDeviceDelegate3 = this.m_deviceDelegates.Find((InternalDeviceDelegate d) => d.Name == lookupName);
						if (internalDeviceDelegate3 == null)
						{
							internalDeviceDelegate3 = new InternalDeviceDelegate(lookupName, device.GetDeviceState(), this);
							this.m_deviceDelegates.Add(internalDeviceDelegate3);
							device.RegisterEventDelegate(internalDeviceDelegate3);
						}
						sortedList.Add(this.m_deviceDelegates.IndexOf(internalDeviceDelegate3), device);
					}
					foreach (Device device2 in sortedList.Values)
					{
						DeviceAttributes deviceAttributes = device2.GetDeviceAttributes();
						string name = device2.GetName();
						string text2 = name;
						string productModel = deviceAttributes.GetProductModel();
						string ip = device2.GetIP();
						ConnectionManager.DeviceOS deviceOS = ConnectionManager.ParseDeviceOSAttribute(deviceAttributes.osType);
						ConnectionSettings connectionSettings;
						if (!string.IsNullOrEmpty(productModel) && !SdpApp.ModelManager.SettingsModel.UserPreferences.FindDevice(text2, out connectionSettings))
						{
							if (SdpApp.ModelManager.SettingsModel.UserPreferences.FindDevice(deviceOS, name, out connectionSettings))
							{
								SdpApp.ModelManager.SettingsModel.UserPreferences.RemoveDevice(connectionSettings, false);
								connectionSettings.LookupName = name;
								SdpApp.ModelManager.SettingsModel.UserPreferences.SaveDevice(connectionSettings);
							}
							else
							{
								connectionSettings = new ConnectionSettings(false);
								connectionSettings.DisplayName = productModel + " (" + name + ")";
								connectionSettings.LookupName = name;
								connectionSettings.HostIP = ip;
								connectionSettings.ConfiguredDeviceOS = deviceOS;
								connectionSettings.GUID = name;
								SdpApp.ModelManager.SettingsModel.UserPreferences.SaveDevice(connectionSettings);
							}
						}
					}
					foreach (KeyValuePair<string, ConnectionSettings> keyValuePair in SdpApp.ModelManager.SettingsModel.UserPreferences.ConnectionConfigurations)
					{
						this.m_view.AddDevice(keyValuePair.Value, DeviceState.Unknown, !SdpApp.ConnectionManager.IsConnected());
						this.InvalidateDeviceState(keyValuePair.Value.LookupName, keyValuePair.Value.GUID, false);
					}
					Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
					if (connectedDevice != null)
					{
						this.m_view.SelectedDevice = connectedDevice.GetName();
						return;
					}
					if (text != null)
					{
						this.m_view.SelectedDevice = text;
						return;
					}
					this.m_view.SelectedDevice = devices[0].GetName();
					return;
				}
				else
				{
					foreach (KeyValuePair<string, ConnectionSettings> keyValuePair2 in SdpApp.ModelManager.SettingsModel.UserPreferences.ConnectionConfigurations)
					{
						this.m_view.AddDevice(keyValuePair2.Value, DeviceState.Unknown, !SdpApp.ConnectionManager.IsConnected());
						this.InvalidateDeviceState(keyValuePair2.Value.LookupName, keyValuePair2.Value.GUID, false);
					}
				}
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000CD0C File Offset: 0x0000AF0C
		private void InvalidateConnectButton(DeviceState deviceState)
		{
			switch (deviceState)
			{
			case DeviceState.Unknown:
				this.m_view.ConnectSensitive = false;
				this.m_view.ConnectLabel = "Not Found";
				return;
			case DeviceState.Installing:
				this.m_view.ConnectSensitive = false;
				this.m_view.ConnectLabel = "Connect";
				return;
			case DeviceState.InstallFailed:
				this.m_view.ConnectSensitive = true;
				this.m_view.ConnectLabel = "Retry";
				return;
			case DeviceState.Ready:
				this.m_view.ConnectSensitive = true;
				this.m_view.ConnectLabel = "Connect";
				return;
			case DeviceState.Connecting:
				this.m_view.ConnectSensitive = false;
				this.m_view.ConnectLabel = "Connecting";
				return;
			case DeviceState.ConnectingError:
				this.m_view.ConnectSensitive = true;
				this.m_view.ConnectLabel = "Connect";
				return;
			case DeviceState.Connected:
				this.m_view.ConnectSensitive = true;
				this.m_view.ConnectLabel = "Disconnect & Exit";
				return;
			default:
				return;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0000CE08 File Offset: 0x0000B008
		private static bool IsAutoConnectSet
		{
			get
			{
				bool flag;
				bool.TryParse(SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AutoConnect), out flag);
				return flag;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000CE34 File Offset: 0x0000B034
		private void clientEvents_savingChanged(object sender, SavingSession args)
		{
			this.m_savingSession = args.saving;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0000CE42 File Offset: 0x0000B042
		private void deviceEvents_DeviceListChanged(object sender, EventArgs args)
		{
			this.InvalidateDevices();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000CE4C File Offset: 0x0000B04C
		private void connectionView_ConnectClicked(object sender, EventArgs e)
		{
			Device deviceByName = SdpApp.ConnectionManager.GetDeviceByName(this.m_view.SelectedDevice);
			if (deviceByName != null)
			{
				if (deviceByName.GetDeviceState() == DeviceConnectionState.InstallFailed)
				{
					DeviceEventArgs deviceEventArgs = new DeviceEventArgs();
					deviceEventArgs.LookupName = deviceByName.GetName();
					SdpApp.EventsManager.Raise<DeviceEventArgs>(SdpApp.EventsManager.DeviceEvents.RetryInstallOnDevice, this, deviceEventArgs);
				}
				if (deviceByName.GetDeviceState() == DeviceConnectionState.Ready)
				{
					DeviceEventArgs deviceEventArgs2 = new DeviceEventArgs();
					deviceEventArgs2.LookupName = deviceByName.GetName();
					SdpApp.EventsManager.Raise<DeviceEventArgs>(SdpApp.EventsManager.DeviceEvents.ConnectToDevice, this, deviceEventArgs2);
				}
				if (deviceByName.GetDeviceState() == DeviceConnectionState.Connected && !this.m_savingSession)
				{
					DeviceEventArgs deviceEventArgs3 = new DeviceEventArgs();
					deviceEventArgs3.LookupName = deviceByName.GetName();
					SdpApp.EventsManager.Raise<DeviceEventArgs>(SdpApp.EventsManager.DeviceEvents.DisconnectFromDevice, this, deviceEventArgs3);
					ExitAppCommand exitAppCommand = new ExitAppCommand();
					SdpApp.ExecuteCommand(exitAppCommand);
				}
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0000CF2D File Offset: 0x0000B12D
		private void connectionView_SelectedDeviceChanged(object sender, ConnectItemEventArgs e)
		{
			this.InvalidateDeviceState(this.m_view.SelectedDevice, e.LookupGUID, false);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000CF47 File Offset: 0x0000B147
		private void connectionView_AutoConnectClicked(object sender, AutoConnectEventArgs e)
		{
			SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.AutoConnect, e.Enabled.ToString());
			this.m_view.AutoConnect = e.Enabled;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000CF7C File Offset: 0x0000B17C
		private void connectionView_DeleteDeviceClicked(object sender, ConnectItemEventArgs e)
		{
			new ShowMessageDialogCommand
			{
				IconType = IconType.Warning,
				ButtonLayout = ButtonLayout.OKCancel,
				Message = "Are you sure you want to delete this device?",
				AffirmativeText = "Delete",
				TopLevelWindow = this.m_view.TopLevelWindow,
				OnCompleted = delegate(bool result)
				{
					if (result)
					{
						SdpApp.ModelManager.SettingsModel.UserPreferences.RemoveDevice(e.LookupGUID, true);
						List<string> availableOS = SdpApp.ModelManager.SettingsModel.UserPreferences.GetAvailableOS();
						this.m_view.RemoveDevice(e.LookupGUID, availableOS);
					}
				}
			}.Execute();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000CFF0 File Offset: 0x0000B1F0
		private void connectionView_EditDeviceClicked(object sender, ConnectItemEventArgs e)
		{
			ConnectionSettings connectionSettings;
			if (SdpApp.ModelManager.SettingsModel.UserPreferences.FindDevice(e.LookupGUID, out connectionSettings))
			{
				this.m_view.ShowAddEditDevice(connectionSettings, true, new List<string> { connectionSettings.ConfiguredDeviceOS.ToString() });
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0000D048 File Offset: 0x0000B248
		private void connectionView_AddDeviceClicked(object sender, EventArgs e)
		{
			List<string> availableOS = SdpApp.ModelManager.SettingsModel.UserPreferences.GetAvailableOS();
			if (availableOS.Count == 0)
			{
				return;
			}
			this.m_view.ShowAddEditDevice(new ConnectionSettings(true), false, availableOS);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000D088 File Offset: 0x0000B288
		private void connectionEvents_ConfigurationComplete(object sender, ConfigureDeviceCompleteArgs args)
		{
			bool flag = !args.RenameOnly;
			if (args.EditConnection && flag)
			{
				this.m_view.RemoveDevice(args.ConnectionSettings.GUID, SdpApp.ModelManager.SettingsModel.UserPreferences.GetAvailableOS());
				InternalDeviceDelegate internalDeviceDelegate = this.m_deviceDelegates.Find((InternalDeviceDelegate d) => d.Name == args.ConnectionSettings.LookupName);
				if (internalDeviceDelegate != null)
				{
					this.m_deviceDelegates.Remove(internalDeviceDelegate);
				}
			}
			if (flag)
			{
				this.m_view.AddDevice(args.ConnectionSettings, DeviceState.Unknown, !SdpApp.ConnectionManager.IsConnected());
				SdpApp.ConnectionManager.AddDeviceConfiguration(args.ConnectionSettings);
			}
			SdpApp.ModelManager.SettingsModel.UserPreferences.SaveDevice(args.ConnectionSettings);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000D172 File Offset: 0x0000B372
		private void ConnectionView_ImportClicked(object o, EventArgs e)
		{
			SdpApp.CommandManager.ExecuteCommand(new OpenSessionCommand());
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000D183 File Offset: 0x0000B383
		private void MainWindowClosing(object o, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000D18B File Offset: 0x0000B38B
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000D194 File Offset: 0x0000B394
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

		// Token: 0x06000565 RID: 1381 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x04000657 RID: 1623
		private readonly List<InternalDeviceDelegate> m_deviceDelegates = new List<InternalDeviceDelegate>();

		// Token: 0x04000658 RID: 1624
		private IConnectionView m_view;

		// Token: 0x04000659 RID: 1625
		private bool m_savingSession;
	}
}
