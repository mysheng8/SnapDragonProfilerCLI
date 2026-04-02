using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Sdp.Helpers;
using Sdp.Logging;
using Sdp.Views.SessionSettingsDialog;

namespace Sdp
{
	// Token: 0x020002AC RID: 684
	public class ConnectionManager : IConnectionManager
	{
		// Token: 0x06000D78 RID: 3448 RVA: 0x00028064 File Offset: 0x00026264
		public ConnectionManager()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.PauseCapture = (EventHandler<PauseCaptureEventArgs>)Delegate.Combine(connectionEvents.PauseCapture, new EventHandler<PauseCaptureEventArgs>(this.connectionEvents_PauseCapture));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.SnapshotRequest = (EventHandler)Delegate.Combine(connectionEvents2.SnapshotRequest, new EventHandler(this.connectionEvents_SnapshotRequest));
			ConnectionEvents connectionEvents3 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents3.CancelSnapshotRequest = (EventHandler)Delegate.Combine(connectionEvents3.CancelSnapshotRequest, new EventHandler(this.connectionEvents_CancelSnapshotRequest));
			ConnectionEvents connectionEvents4 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents4.StartCaptureRequest = (EventHandler<TakeCaptureArgs>)Delegate.Combine(connectionEvents4.StartCaptureRequest, new EventHandler<TakeCaptureArgs>(this.connectionEvents_StartCaptureRequest));
			ConnectionEvents connectionEvents5 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents5.StartSamplingRequest = (EventHandler)Delegate.Combine(connectionEvents5.StartSamplingRequest, new EventHandler(this.connectionEvents_StartSamplingRequest));
			ConnectionEvents connectionEvents6 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents6.StopCaptureRequest = (EventHandler)Delegate.Combine(connectionEvents6.StopCaptureRequest, new EventHandler(this.connectionEvents_StopCaptureRequest));
			ConnectionEvents connectionEvents7 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents7.StopSamplingRequest = (EventHandler)Delegate.Combine(connectionEvents7.StopSamplingRequest, new EventHandler(this.connectionEvents_StopSamplingRequest));
			ConnectionEvents connectionEvents8 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents8.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Combine(connectionEvents8.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.RetryInstallOnDevice = (EventHandler<DeviceEventArgs>)Delegate.Combine(deviceEvents.RetryInstallOnDevice, new EventHandler<DeviceEventArgs>(this.deviceEvents_RetryInstallOnDevice));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ConnectToDevice = (EventHandler<DeviceEventArgs>)Delegate.Combine(deviceEvents2.ConnectToDevice, new EventHandler<DeviceEventArgs>(this.deviceEvents_ConnectToDevice));
			DeviceEvents deviceEvents3 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents3.DisconnectFromDevice = (EventHandler<DeviceEventArgs>)Delegate.Combine(deviceEvents3.DisconnectFromDevice, new EventHandler<DeviceEventArgs>(this.deviceEvents_DisconnectFromDevice));
			DeviceEvents deviceEvents4 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents4.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents4.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectACK));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.AppShutdown = (EventHandler<EventArgs>)Delegate.Combine(clientEvents.AppShutdown, new EventHandler<EventArgs>(this.clientEvents_AppShutdown));
			SDPVersion sdpversion = new SDPVersion();
			SdpApp.ModelManager.ApplicationModel.Version = sdpversion.GetVersionString();
			SdpApp.ModelManager.ApplicationModel.BuildDate = sdpversion.GetBuildDate();
			this.RegisteredMetricsList = new List<uint>();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00028304 File Offset: 0x00026504
		public bool InitCore(SessionSettingsSelection sessionSettingsSelections)
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			this.m_client = new Client();
			SessionSettings sessionSettings = new SessionSettings
			{
				MaxTotalSessionsSizeMB = sessionSettingsSelections.MaxSizeMB,
				SessionDirectoryRootPath = sessionSettingsSelections.SessionRootPath,
				CreateTimestampedSubDirectory = true
			};
			this.m_client.Init(sessionSettings);
			this.m_clientDelegate = new InternalClientDelegate(this, this.m_client);
			this.m_client.RegisterEventDelegate(this.m_clientDelegate);
			this.m_metricDelegate = new InternalMetricDelegate(this);
			this.m_metricCategoryDelegate = new InternalMetricCategoryDelegate(this);
			MetricManager.Get().RegisterMetricEventDelegate(this.m_metricDelegate);
			MetricManager.Get().RegisterMetricCategoryEventDelegate(this.m_metricCategoryDelegate);
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)))
			{
				DeviceManager.Get().SetDeleteServiceFilesOnExit(BoolConverter.Convert(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)));
			}
			string text = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ADBPath);
			if (!string.IsNullOrEmpty(text))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PATH");
				Environment.SetEnvironmentVariable("PATH", text + ";" + environmentVariable);
			}
			string text2 = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.SSHPath);
			if (!string.IsNullOrEmpty(text2))
			{
				string environmentVariable2 = Environment.GetEnvironmentVariable("PATH");
				Environment.SetEnvironmentVariable("PATH", text2 + ";" + environmentVariable2);
			}
			string environment = DeviceManager.Get().GetEnvironment();
			Match match = new Regex("MINIMUM_ADB_VERSION=([\\d.]+)").Match(environment);
			if (match.Success)
			{
				SdpApp.ModelManager.ConnectionModel.MinimumADBVersion = match.Groups[1].Value;
			}
			else
			{
				SdpApp.ModelManager.ConnectionModel.MinimumADBVersion = "1.0.40";
			}
			bool flag = false;
			string text3 = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ShowADBVersionDialog);
			if (!string.IsNullOrEmpty(text3))
			{
				flag = BoolConverter.Convert(text3);
			}
			if (flag)
			{
				Regex regex = new Regex("ADB_USB_AVAILABLE=(\\d)");
				Match match2 = regex.Match(environment);
				if (match2.Success && string.Compare(match2.Groups[1].Value, "0") == 0)
				{
					ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
					showMessageDialogCommand.Message = "Snapdragon Profiler requires ADB version " + SdpApp.ModelManager.ConnectionModel.MinimumADBVersion + " or higher to connect over USB.\nIf already installed, please make sure it is in your <b>PATH</b>\nor specify the location on <b>File->Settings->Android->ADB Path</b>";
					showMessageDialogCommand.IconType = IconType.Warning;
					showMessageDialogCommand.HasDontShowAgainCheckBox = true;
					SdpApp.CommandManager.ExecuteCommand(showMessageDialogCommand);
					userPreferences.RecordSetting(UserPreferenceModel.UserPreference.ShowADBVersionDialog, (!showMessageDialogCommand.DontShowAgainCheckBoxValue).ToString());
				}
			}
			string text4 = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AndroidNDKPath);
			if (text4 == null)
			{
				string text5 = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.SimpleperfPath);
				if (!string.IsNullOrEmpty(text5))
				{
					int num = text5.IndexOf("android-ndk");
					if (num != -1)
					{
						int num2 = text5.IndexOf(Path.DirectorySeparatorChar, num);
						if (num2 != -1)
						{
							text4 = text5.Remove(num2);
							userPreferences.RecordSetting(UserPreferenceModel.UserPreference.AndroidNDKPath, text4);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(text4))
			{
				string text6 = Path.Combine(new string[] { text4, "simpleperf", "bin", "android", "arm64", "simpleperf" });
				if (File.Exists(text6))
				{
					DeviceManager.Get().SetSimpleperfPath(text6);
				}
				else
				{
					text6 = Path.Combine(new string[] { text4, "simpleperf", "android", "arm64", "simpleperf" });
					if (File.Exists(text6))
					{
						DeviceManager.Get().SetSimpleperfPath(text6);
					}
				}
			}
			this.ReloadDeviceConnectionsAndSearch();
			return true;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00028678 File Offset: 0x00026878
		public void ReloadDeviceConnectionsAndSearch()
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			DeviceManager deviceManager = DeviceManager.Get();
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXIPAddress)))
			{
				deviceManager.SetQNXIPAddress(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXIPAddress));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXUserName)))
			{
				deviceManager.SetQNXUsername(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXUserName));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXDeployDirectory)))
			{
				deviceManager.SetQNXDeployDirectory(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXDeployDirectory));
			}
			else
			{
				deviceManager.SetQNXDeployDirectory("/tmp/");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXPassWord)))
			{
				deviceManager.SetQNXPassword(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXPassWord));
			}
			else
			{
				deviceManager.SetQNXPassword("");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXSSHIdentityFile)))
			{
				deviceManager.SetQNXSSHIdentityFile(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXSSHIdentityFile));
			}
			else
			{
				deviceManager.SetQNXSSHIdentityFile("");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXConnectionType)))
			{
				deviceManager.SetQNXConnectionType(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXConnectionType));
			}
			else
			{
				deviceManager.SetQNXConnectionType("Telnet");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXProcessPriority)))
			{
				deviceManager.SetQNXProcessPriority(int.Parse(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.QNXProcessPriority)));
			}
			else
			{
				deviceManager.SetQNXProcessPriority(10);
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLIPAddress)))
			{
				deviceManager.SetAGLIPAddress(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLIPAddress));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLUserName)))
			{
				deviceManager.SetAGLUsername(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLUserName));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLDeployDirectory)))
			{
				deviceManager.SetAGLDeployDirectory(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLDeployDirectory));
			}
			else
			{
				deviceManager.SetAGLDeployDirectory("/tmp/");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLPassWord)))
			{
				deviceManager.SetAGLPassword(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AGLPassWord));
			}
			else
			{
				deviceManager.SetAGLPassword("");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.HLM_EnableConnection)))
			{
				deviceManager.SetHLMIsEnabled(BoolConverter.Convert(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.HLM_EnableConnection)));
			}
			else
			{
				deviceManager.SetHLMIsEnabled(false);
			}
			string text = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.WinARMIPAddress);
			if (!string.IsNullOrEmpty(text))
			{
				deviceManager.SetWinARMIPAddress(text);
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHIPAddress)))
			{
				deviceManager.SetLinuxSSHIPAddress(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHIPAddress));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHUserName)))
			{
				deviceManager.SetLinuxSSHUsername(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHUserName));
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHIdentityFile)))
			{
				deviceManager.SetLinuxSSHIdentityFile(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHIdentityFile));
			}
			else
			{
				deviceManager.SetLinuxSSHIdentityFile("");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHPassWord)))
			{
				deviceManager.SetLinuxSSHPassword(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHPassWord));
			}
			else
			{
				deviceManager.SetLinuxSSHPassword("");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHDeployDirectory)))
			{
				deviceManager.SetLinuxSSHDeployDirectory(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.LinuxSSHDeployDirectory));
			}
			else
			{
				deviceManager.SetLinuxSSHDeployDirectory("/data/local/tmp");
			}
			if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.InstallerTimeout)))
			{
				deviceManager.SetInstallerTimeout(IntConverter.Convert(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.InstallerTimeout)));
			}
			if (deviceManager.IsInitialized())
			{
				deviceManager.FindDevices();
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00028980 File Offset: 0x00026B80
		public void AddDeviceConfiguration(ConnectionSettings settings)
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			switch (settings.ConfiguredDeviceOS)
			{
			case ConnectionManager.DeviceOS.Windows:
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.WinARMIPAddress, settings.HostIP);
				break;
			case ConnectionManager.DeviceOS.AGL:
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.AGLIPAddress, settings.HostIP);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.AGLUserName, settings.Username);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.AGLDeployDirectory, settings.DeployDir);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.AGLPassWord, settings.GetDecryptedPass());
				break;
			case ConnectionManager.DeviceOS.QNX:
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXIPAddress, settings.HostIP);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXUserName, settings.Username);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXPassWord, settings.GetDecryptedPass());
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXSSHIdentityFile, settings.IdentityFile);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXConnectionType, settings.ConnectionType);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXDeployDirectory, settings.DeployDir);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.QNXProcessPriority, settings.ProcessPriority.ToString());
				break;
			case ConnectionManager.DeviceOS.HLM:
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.HLM_EnableConnection, "true");
				break;
			case ConnectionManager.DeviceOS.LinuxSSH:
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.LinuxSSHIPAddress, settings.HostIP);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.LinuxSSHUserName, settings.Username);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.LinuxSSHPassWord, settings.GetDecryptedPass());
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.LinuxSSHIdentityFile, settings.IdentityFile);
				userPreferences.RecordSetting(UserPreferenceModel.UserPreference.LinuxSSHDeployDirectory, settings.DeployDir);
				break;
			}
			this.ReloadDeviceConnectionsAndSearch();
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00028AE8 File Offset: 0x00026CE8
		public uint CreateCaptureId(CaptureType captureType)
		{
			CaptureManager captureManager = this.m_client.GetCaptureManager();
			return captureManager.CreateCapture((uint)captureType);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00028B08 File Offset: 0x00026D08
		public void SetCaptureName(uint captureId, string captureName)
		{
			if (!SdpApp.UIManager.IsCaptureNameInUse(captureName))
			{
				CaptureManager captureManager = this.m_client.GetCaptureManager();
				captureManager.SetCaptureName(captureId, captureName);
				EventHandler<CaptureNameChangedArgs> captureNameChanged = SdpApp.EventsManager.ClientEvents.CaptureNameChanged;
				if (captureNameChanged == null)
				{
					return;
				}
				captureNameChanged(this, new CaptureNameChangedArgs
				{
					CaptureId = captureId,
					CaptureName = captureName
				});
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00028B64 File Offset: 0x00026D64
		public long GetFirstTimestampTOD(uint captureId)
		{
			global::Capture capture = CaptureManager.Get().GetCapture(captureId);
			return capture.GetProperties().startTimeTOD;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00028B88 File Offset: 0x00026D88
		public void FetchBuffer(uint providerId, uint captureId, uint bufferCategory, uint bufferId, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback, bool saveToDB = true, string filename = "", Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn progressCallback = null, IntPtr tag = default(IntPtr))
		{
			this.m_client.FetchBuffer(providerId, captureId, bufferCategory, bufferId, transferCompleteCallback, tag, progressCallback, saveToDB, filename);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00028BAF File Offset: 0x00026DAF
		public void BufferRegisteredCallback(Void_UInt_UInt_UInt_UInt_Fn callback)
		{
			this.m_client.RegisterOnBufferRegistered(callback);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00028BC0 File Offset: 0x00026DC0
		public bool GetBuffer(uint captureId, uint bufferCategory, uint bufferId, out byte[] data)
		{
			data = null;
			uint bufferDataSize = this.m_client.GetBufferDataSize(captureId, bufferCategory, bufferId);
			if (bufferDataSize == 0U)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal((int)bufferDataSize);
			if (this.m_client.GetBufferData(captureId, bufferCategory, bufferId, intPtr, bufferDataSize))
			{
				data = new byte[bufferDataSize];
				Marshal.Copy(intPtr, data, 0, (int)bufferDataSize);
				Marshal.FreeHGlobal(intPtr);
				return true;
			}
			Marshal.FreeHGlobal(intPtr);
			return false;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00028C21 File Offset: 0x00026E21
		public DataModel GetDataModel()
		{
			return this.m_client.GetDataModel();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00028C30 File Offset: 0x00026E30
		public void Shutdown()
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			if (this.m_client != null)
			{
				if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)))
				{
					DeviceManager.Get().SetDeleteServiceFilesOnExit(BoolConverter.Convert(userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)));
				}
				DeviceList deviceList = this.m_client.GetDeviceList();
				if (deviceList != null)
				{
					foreach (Device device in deviceList)
					{
						if (device.GetDeviceState() == DeviceConnectionState.Connected)
						{
							device.Disconnect();
						}
					}
				}
				this.m_client.DeregisterEventDelegate(this.m_clientDelegate);
				this.m_client.Shutdown();
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00028CEC File Offset: 0x00026EEC
		public void ExportAllMetricProgressCallback(uint progress)
		{
			this.m_exportProgressObject.CurrentValue = progress / 100.0;
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(this.m_exportProgressObject));
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00028D2C File Offset: 0x00026F2C
		public void ExportAllMetricData(string path)
		{
			this.m_exportProgressObject.Title = "Export Metrics Progress";
			this.m_exportProgressObject.Description = "Exporting Metrics To CSV";
			this.m_exportProgressObject.CurrentValue = 0.0;
			SdpApp.AnalyticsManager.TrackExport("Metrics");
			if (this.m_exportMetricProgressFPtr == null)
			{
				this.m_exportMetricProgressFPtr = new Void_UInt_Fn(this.ExportAllMetricProgressCallback);
			}
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(this.m_exportProgressObject));
			this.m_client.ExportAllMetricData(path, this.m_exportMetricProgressFPtr);
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(this.m_exportProgressObject));
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00028DF3 File Offset: 0x00026FF3
		public string GetSessionPath()
		{
			return SessionManager.Get().GetSessionPath();
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00028DFF File Offset: 0x00026FFF
		public SDPProcessorPlugin GetProcessorPlugin(string name)
		{
			return ProcessorPluginMgr.Get().GetPlugin(name);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00028E0C File Offset: 0x0002700C
		public bool PushFile(object sender, string localFile, string remoteFile, bool compress)
		{
			if (this.m_client != null)
			{
				if (MainWindowController.DeviceFileTransferProgress == null)
				{
					MainWindowController.DeviceFileTransferProgress = new ProgressObject();
					MainWindowController.DeviceFileTransferProgress.Title = "Pushing file to device";
				}
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, sender, new ProgressEventArgs(MainWindowController.DeviceFileTransferProgress));
				bool flag = this.m_client.PushFile(localFile, remoteFile, compress);
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, sender, new ProgressEventArgs(MainWindowController.DeviceFileTransferProgress));
				return flag;
			}
			return false;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00028E9C File Offset: 0x0002709C
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00028EA3 File Offset: 0x000270A3
		public string ConnectedDeviceRendererString
		{
			get
			{
				return ConnectionManager.m_connectedDeviceRendererString;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					ConnectionManager.m_connectedDeviceRendererString = value;
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00028EB3 File Offset: 0x000270B3
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00028EBA File Offset: 0x000270BA
		public string CurrentRendererString
		{
			get
			{
				return ConnectionManager.m_currentRendererString;
			}
			set
			{
				ConnectionManager.m_currentRendererString = value;
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00028EC2 File Offset: 0x000270C2
		public DeviceList GetDevices()
		{
			if (this.m_client != null)
			{
				return this.m_client.GetDeviceList();
			}
			return null;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00028EDC File Offset: 0x000270DC
		public ConnectionManager.DeviceOS GetDeviceOS()
		{
			if (ConnectionManager.m_currentDeviceOS == ConnectionManager.DeviceOS.Other && this.m_client != null)
			{
				Device connectedDevice = this.m_client.GetDeviceManager().GetConnectedDevice();
				if (connectedDevice != null)
				{
					ConnectionManager.m_currentDeviceOS = ConnectionManager.ParseDeviceOSAttribute(connectedDevice.GetDeviceAttributes().osType);
				}
			}
			return ConnectionManager.m_currentDeviceOS;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00028F28 File Offset: 0x00027128
		public static ConnectionManager.DeviceOS ParseDeviceOSAttribute(string os)
		{
			if (os != null)
			{
				switch (os.Length)
				{
				case 3:
				{
					char c = os[0];
					if (c != 'A')
					{
						if (c != 'H')
						{
							if (c != 'Q')
							{
								return ConnectionManager.DeviceOS.Other;
							}
							if (!(os == "QNX"))
							{
								return ConnectionManager.DeviceOS.Other;
							}
							return ConnectionManager.DeviceOS.QNX;
						}
						else
						{
							if (!(os == "HLM"))
							{
								return ConnectionManager.DeviceOS.Other;
							}
							return ConnectionManager.DeviceOS.HLM;
						}
					}
					else
					{
						if (!(os == "AGL"))
						{
							return ConnectionManager.DeviceOS.Other;
						}
						return ConnectionManager.DeviceOS.AGL;
					}
					break;
				}
				case 4:
				case 5:
					return ConnectionManager.DeviceOS.Other;
				case 6:
					if (!(os == "WinARM"))
					{
						return ConnectionManager.DeviceOS.Other;
					}
					break;
				case 7:
				{
					char c = os[0];
					if (c != 'A')
					{
						if (c != 'Q')
						{
							if (c != 'W')
							{
								return ConnectionManager.DeviceOS.Other;
							}
							if (!(os == "Windows"))
							{
								return ConnectionManager.DeviceOS.Other;
							}
						}
						else
						{
							if (!(os == "QCLinux"))
							{
								return ConnectionManager.DeviceOS.Other;
							}
							return ConnectionManager.DeviceOS.QCLinux;
						}
					}
					else
					{
						if (!(os == "ANDROID"))
						{
							return ConnectionManager.DeviceOS.Other;
						}
						return ConnectionManager.DeviceOS.Android;
					}
					break;
				}
				case 8:
					if (!(os == "LinuxSSH"))
					{
						return ConnectionManager.DeviceOS.Other;
					}
					return ConnectionManager.DeviceOS.LinuxSSH;
				default:
					return ConnectionManager.DeviceOS.Other;
				}
				return ConnectionManager.DeviceOS.Windows;
			}
			return ConnectionManager.DeviceOS.Other;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00029027 File Offset: 0x00027227
		public static string GetDeviceDisplayName(ConnectionManager.DeviceOS os)
		{
			if (os == ConnectionManager.DeviceOS.LinuxSSH)
			{
				return "Linux SSH";
			}
			return os.ToString();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00029040 File Offset: 0x00027240
		public ConnectionManager.DeviceOS GetUnconnectedDeviceOS(string searchName)
		{
			Device unconnectedDevice = this.GetUnconnectedDevice(searchName);
			if (unconnectedDevice != null)
			{
				return ConnectionManager.ParseDeviceOSAttribute(unconnectedDevice.GetDeviceAttributes().osType);
			}
			return ConnectionManager.DeviceOS.Other;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0002906C File Offset: 0x0002726C
		private Device GetUnconnectedDevice(string searchName)
		{
			if (this.m_client != null)
			{
				DeviceList devices = this.m_client.GetDeviceManager().GetDevices();
				if (devices != null)
				{
					foreach (Device device in devices)
					{
						string name = device.GetName();
						if (searchName == name)
						{
							return device;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000290E4 File Offset: 0x000272E4
		public bool IsDeviceAutoDiscovered(string name)
		{
			Device unconnectedDevice = this.GetUnconnectedDevice(name);
			return unconnectedDevice != null && unconnectedDevice.GetDeviceAttributes().autoDetected;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002910C File Offset: 0x0002730C
		public SortedSet<RenderingAPI> GetSupportedRenderingAPIs(CaptureType capture)
		{
			SortedSet<RenderingAPI> sortedSet;
			if (this.m_supportedCaptureTypes.TryGetValue(capture, out sortedSet))
			{
				return sortedSet;
			}
			return new SortedSet<RenderingAPI>();
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00029130 File Offset: 0x00027330
		public void AddSupportedCaptureType(CaptureType capture, string metricName, uint userData)
		{
			SortedSet<RenderingAPI> sortedSet;
			if (!this.m_supportedCaptureTypes.TryGetValue(capture, out sortedSet))
			{
				sortedSet = new SortedSet<RenderingAPI>();
				this.m_supportedCaptureTypes.Add(capture, sortedSet);
				ActionEnum actionEnum = ActionEnum.Realtime;
				if (capture != CaptureType.Trace)
				{
					if (capture != CaptureType.Snapshot)
					{
						if (capture == CaptureType.Sampling)
						{
							actionEnum = ActionEnum.Sampling;
						}
					}
					else
					{
						actionEnum = ActionEnum.Snapshot;
					}
				}
				else
				{
					actionEnum = ActionEnum.NewCapture;
				}
				SdpApp.EventsManager.Raise<EnableActionArgs>(SdpApp.EventsManager.ConnectionEvents.EnableAction, this, new EnableActionArgs(actionEnum));
			}
			if (metricName != null && userData == 4294962635U)
			{
				RenderingAPI renderingAPI;
				if (metricName.StartsWith("DX11"))
				{
					renderingAPI = RenderingAPI.DirectX11;
				}
				else if (metricName.StartsWith("DX12"))
				{
					renderingAPI = RenderingAPI.DirectX12;
				}
				else if (metricName.StartsWith("OpenGL"))
				{
					renderingAPI = RenderingAPI.OpenGL;
				}
				else if (metricName.StartsWith("OpenCL"))
				{
					renderingAPI = RenderingAPI.OpenCL;
				}
				else
				{
					if (!metricName.StartsWith("Vulkan"))
					{
						ConnectionManager.Logger.LogError(string.Format("Unrecognized Rendering API : {0}", metricName));
						return;
					}
					renderingAPI = RenderingAPI.Vulkan;
				}
				if (sortedSet.Add(renderingAPI))
				{
					SdpApp.EventsManager.Raise<RenderingAPISupportEventArgs>(SdpApp.EventsManager.ConnectionEvents.RenderingAPISupport, this, new RenderingAPISupportEventArgs(renderingAPI));
				}
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00029240 File Offset: 0x00027440
		public bool SupportsCaptureType(CaptureType cap)
		{
			return this.m_supportedCaptureTypes.ContainsKey(cap);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00029250 File Offset: 0x00027450
		public bool CanLaunchApp()
		{
			ConnectionManager.DeviceOS deviceOS = this.GetDeviceOS();
			return deviceOS <= ConnectionManager.DeviceOS.Windows || deviceOS == ConnectionManager.DeviceOS.QCLinux || deviceOS == ConnectionManager.DeviceOS.HLM;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00029273 File Offset: 0x00027473
		public Device GetDeviceByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return DeviceManager.Get().GetDevice(name);
			}
			return null;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002928A File Offset: 0x0002748A
		public Device GetConnectedDevice()
		{
			if (this.m_client != null)
			{
				return this.m_client.GetDeviceManager().GetConnectedDevice();
			}
			return null;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000292A6 File Offset: 0x000274A6
		public bool IsConnected()
		{
			return this.m_client != null && this.m_client.GetDeviceManager().GetConnectedDevice() != null;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000292C8 File Offset: 0x000274C8
		public bool IsConnectedDeviceRooted()
		{
			if (this.m_client != null)
			{
				Device connectedDevice = this.m_client.GetDeviceManager().GetConnectedDevice();
				return connectedDevice != null && connectedDevice.GetProperty(DeviceSettings.ProfilerDeviceIsRooted).Equals("True");
			}
			return false;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00029308 File Offset: 0x00027508
		public bool TryGetDefaultActivity(string deviceName, string package, out string activity, out bool isPackageDebuggable)
		{
			global::System.Diagnostics.Process process = new global::System.Diagnostics.Process();
			process.StartInfo.FileName = "adb";
			process.StartInfo.Arguments = "-s " + deviceName + " shell dumpsys package " + package;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			string text = process.StandardOutput.ReadToEnd().TrimEnd(new char[] { '\r', '\n' });
			process.WaitForExit();
			isPackageDebuggable = this.IsConnectedDeviceRooted() || Regex.Match(text, "flags=\\[\\s*\\S*\\s*DEBUGGABLE").Success;
			Match match = Regex.Match(text, "android\\.intent\\.action\\.MAIN:[\\n\\r]\\s+\\S+\\s+(" + package + "/\\S+)");
			if (match.Success)
			{
				activity = match.Groups[1].Value;
				return true;
			}
			activity = null;
			return false;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000293F0 File Offset: 0x000275F0
		public void RequestAppRestart(CaptureType captureType)
		{
			this.StopSelectedApp(captureType);
			IdNamePair idNamePair = null;
			switch (captureType)
			{
			case CaptureType.Realtime:
				if (SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess;
				}
				break;
			case CaptureType.Trace:
				if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess;
				}
				break;
			case (CaptureType)3U:
				break;
			case CaptureType.Snapshot:
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					idNamePair = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess;
				}
				break;
			default:
				if (captureType == CaptureType.Sampling)
				{
					if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
					{
						idNamePair = SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess;
					}
				}
				break;
			}
			if (idNamePair == null)
			{
				return;
			}
			if (idNamePair.Id == 0U || string.IsNullOrEmpty(idNamePair.Name))
			{
				return;
			}
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice == null)
			{
				return;
			}
			string deviceName = connectedDevice.GetDeviceAttributes().deviceName;
			string name = idNamePair.Name;
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			foreach (LaunchHistory launchHistory in SdpApp.ModelManager.SettingsModel.UserPreferences.LaunchedAppHistory)
			{
				bool flag = launchHistory.Path == name && launchHistory.DeviceOS == this.GetDeviceOS();
				if (!flag && this.GetDeviceOS() == ConnectionManager.DeviceOS.Android)
				{
					string[] array = launchHistory.Path.Split(new char[] { '/' });
					string text = ((array.Length != 0) ? array[0] : launchHistory.Path);
					flag = text == name && launchHistory.DeviceOS == this.GetDeviceOS();
				}
				if (flag)
				{
					AppStartSettings appStartSettings = new AppStartSettings(launchHistory.Path, launchHistory.WorkingDirectory, launchHistory.Args, Convert.ToUInt32(launchHistory.RenderingAPIs), (uint)captureType, launchHistory.EnvironmentVariables, launchHistory.Options.ToString());
					AppStartResponse appStartResponse = connectedDevice.StartApp(appStartSettings);
					if (appStartResponse.result)
					{
						SdpApp.ModelManager.DataSourcesModel.m_procStartSettings[(ulong)appStartResponse.pid] = appStartSettings;
						ConnectionManager.Logger.LogInformation("Successfully restarted app: " + name);
						break;
					}
					ConnectionManager.Logger.LogError("Failed to restart app: " + name);
					break;
				}
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002969C File Offset: 0x0002789C
		public void StopSelectedApp(CaptureType captureType)
		{
			IdNamePair idNamePair = null;
			switch (captureType)
			{
			case CaptureType.Realtime:
				if (SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess;
				}
				break;
			case CaptureType.Trace:
				if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess;
				}
				break;
			case (CaptureType)3U:
				break;
			case CaptureType.Snapshot:
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					idNamePair = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess;
				}
				break;
			default:
				if (captureType == CaptureType.Sampling)
				{
					if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
					{
						idNamePair = SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess;
					}
				}
				break;
			}
			if (idNamePair == null)
			{
				return;
			}
			if (idNamePair.Id == 0U || string.IsNullOrEmpty(idNamePair.Name))
			{
				return;
			}
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice == null)
			{
				return;
			}
			ConnectionManager.DeviceOS deviceOS = this.GetDeviceOS();
			if (deviceOS <= ConnectionManager.DeviceOS.Windows || deviceOS == ConnectionManager.DeviceOS.QCLinux)
			{
				connectedDevice.StopApp(idNamePair.Id);
				return;
			}
			ConnectionManager.Logger.LogError(string.Format("Stop Selected App Not Implemented for {0}", deviceOS.ToString()));
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000297E4 File Offset: 0x000279E4
		public void RestartSelectedApp(CaptureType captureType, AppStartSettings appStartSettings = null)
		{
			IdNamePair idNamePair = null;
			switch (captureType)
			{
			case CaptureType.Realtime:
				if (SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController.SelectedProcess;
				}
				break;
			case CaptureType.Trace:
				if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess != null)
				{
					idNamePair = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess;
				}
				break;
			case (CaptureType)3U:
				break;
			case CaptureType.Snapshot:
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					idNamePair = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess;
				}
				break;
			default:
				if (captureType == CaptureType.Sampling)
				{
					if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
					{
						idNamePair = SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess;
					}
				}
				break;
			}
			if (idNamePair == null)
			{
				return;
			}
			if (idNamePair.Id == 0U || string.IsNullOrEmpty(idNamePair.Name))
			{
				return;
			}
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice == null)
			{
				return;
			}
			connectedDevice.StopApp(idNamePair.Id);
			if (appStartSettings != null)
			{
				appStartSettings.captureType = (uint)captureType;
				AppStartResponse appStartResponse = connectedDevice.StartApp(appStartSettings);
				SdpApp.ModelManager.DataSourcesModel.m_procStartSettings[(ulong)appStartResponse.pid] = appStartSettings;
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00029928 File Offset: 0x00027B28
		public ProcessList GetProcesses()
		{
			return ProcessManager.Get().GetAllProcesses();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00029944 File Offset: 0x00027B44
		public global::Process GetProcessByName(string name)
		{
			global::Process processByName = ProcessManager.Get().GetProcessByName(name);
			if (processByName != null && processByName.IsValid())
			{
				return processByName;
			}
			return null;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002996C File Offset: 0x00027B6C
		public global::Process GetProcessByID(uint pid)
		{
			global::Process process = ProcessManager.Get().GetProcess(pid);
			if (process != null && process.IsValid())
			{
				return process;
			}
			return null;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00029994 File Offset: 0x00027B94
		public List<uint> GetMetricsForProcess(uint pid)
		{
			List<uint> list = new List<uint>();
			global::Process processByID = this.GetProcessByID(pid);
			if (processByID != null)
			{
				MetricIDList linkedMetrics = processByID.GetLinkedMetrics();
				if (linkedMetrics != null)
				{
					foreach (uint num in linkedMetrics)
					{
						Metric metric = MetricManager.Get().GetMetric(num);
						if (metric != null && !metric.IsGlobal())
						{
							MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(metric.GetProperties().categoryID);
							if (metricCategory != null && (string.Compare(metric.GetProperties().name, "OpenGL Snapshot") != 0 || string.Compare(metricCategory.GetProperties().name, "OpenGL ES") != 0) && (string.Compare(metric.GetProperties().name, "Vulkan Snapshot") != 0 || string.Compare(metricCategory.GetProperties().name, "Vulkan") != 0) && (string.Compare(metric.GetProperties().name, "DX12 Snapshot") != 0 || string.Compare(metricCategory.GetProperties().name, "DX12") != 0))
							{
								list.Add(metric.GetProperties().id);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00029AE0 File Offset: 0x00027CE0
		public List<uint> GetGlobalMetrics()
		{
			List<uint> list = new List<uint>();
			MetricList allMetrics = MetricManager.Get().GetAllMetrics();
			if (allMetrics != null)
			{
				foreach (Metric metric in allMetrics)
				{
					if (metric.IsGlobal())
					{
						list.Add(metric.GetProperties().id);
					}
				}
			}
			return list;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00029B50 File Offset: 0x00027D50
		public Metric GetMetricByID(uint metricId)
		{
			Metric metric = MetricManager.Get().GetMetric(metricId);
			if (!metric.IsValid())
			{
				DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
				if (dataModel != null)
				{
					Model model = dataModel.GetModel("ImportSession");
					if (model != null)
					{
						ModelObject modelObject = dataModel.GetModelObject(model, "ImportedMetrics");
						if (modelObject != null)
						{
							ModelObjectDataList modelObjectData = dataModel.GetModelObjectData(modelObject, "id", metricId.ToString());
							if (modelObjectData != null && modelObjectData.Count == 1)
							{
								metric.GetProperties().id = metricId;
								metric.GetProperties().name = modelObjectData[0].GetValue("name");
								metric.GetProperties().categoryID = UintConverter.Convert(modelObjectData[0].GetValue("categoryID"));
							}
						}
					}
				}
			}
			return metric;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00029C13 File Offset: 0x00027E13
		public Metric GetMetricByName(string metricName)
		{
			return MetricManager.Get().GetMetricByName(metricName);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00029C20 File Offset: 0x00027E20
		public List<uint> GetMetricsByCategory(uint categoryId)
		{
			List<uint> list = new List<uint>();
			MetricList allMetrics = MetricManager.Get().GetAllMetrics();
			if (allMetrics != null)
			{
				foreach (Metric metric in allMetrics)
				{
					if (metric.GetProperties().categoryID == categoryId)
					{
						list.Add(metric.GetProperties().id);
					}
				}
			}
			return list;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00029C98 File Offset: 0x00027E98
		public MetricCategory GetMetricCategoryByID(uint categoryId)
		{
			return MetricManager.Get().GetMetricCategory(categoryId);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00029CA5 File Offset: 0x00027EA5
		public MetricCategory GetMetricCategoryByName(string name)
		{
			return MetricManager.Get().GetMetricCategoryByName(name);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00029CB2 File Offset: 0x00027EB2
		public Option GetOption(string name, uint processId)
		{
			if (this.m_client != null)
			{
				return this.m_client.GetOption(name, processId);
			}
			return null;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00029CCB File Offset: 0x00027ECB
		public Option GetOption(uint optionId, uint processId)
		{
			if (this.m_client != null)
			{
				return this.m_client.GetOption(optionId, processId);
			}
			return null;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00029CE4 File Offset: 0x00027EE4
		private void connectionEvents_PauseCapture(object sender, PauseCaptureEventArgs e)
		{
			global::Capture capture = CaptureManager.Get().GetCapture(e.CaptureId);
			capture.Pause(e.Pause);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00029D10 File Offset: 0x00027F10
		private void connectionEvents_StartCaptureRequest(object sender, TakeCaptureArgs e)
		{
			if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				uint duration = e.Duration;
				global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.CaptureId);
				uint num = 0U;
				if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess != null)
				{
					num = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess.Id;
				}
				capture.Start(new CaptureSettings(2U, num, 0U, duration, ConnectionManager.m_connectedDeviceRendererString));
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00029D9C File Offset: 0x00027F9C
		private void connectionEvents_StartSamplingRequest(object sender, EventArgs e)
		{
			if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
			{
				uint num = 2000U;
				string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.MaxCaptureDurationMs);
				if (!string.IsNullOrEmpty(text))
				{
					num = UintConverter.Convert(text);
				}
				global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.SamplingModel.CurrentSamplingController.CaptureId);
				uint num2 = 0U;
				if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess != null)
				{
					num2 = SdpApp.ModelManager.SamplingModel.CurrentSamplingController.SelectedProcess.Id;
				}
				capture.Start(new CaptureSettings(8U, num2, 0U, num, ConnectionManager.m_connectedDeviceRendererString));
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00029E50 File Offset: 0x00028050
		private void connectionEvents_StopCaptureRequest(object sender, EventArgs e)
		{
			if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.CaptureId);
				capture.Stop();
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00029E94 File Offset: 0x00028094
		private void connectionEvents_StopSamplingRequest(object sender, EventArgs e)
		{
			if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
			{
				global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.SamplingModel.CurrentSamplingController.CaptureId);
				capture.Stop();
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00029ED8 File Offset: 0x000280D8
		private void connectionEvents_SnapshotRequest(object sender, EventArgs e)
		{
			global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CaptureId);
			uint id = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess.Id;
			capture.Start(new CaptureSettings(4U, id, 0U, 0U, this.CurrentRendererString));
			Thread thread = new Thread(delegate
			{
				Thread.Sleep(1000);
				capture.Stop();
			});
			thread.Start();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00029F5C File Offset: 0x0002815C
		private void connectionEvents_CancelSnapshotRequest(object sender, EventArgs e)
		{
			global::Capture capture = CaptureManager.Get().GetCapture(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CaptureId);
			capture.Cancel();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00029F90 File Offset: 0x00028190
		private void connectionEvents_EnableMetric(object sender, EnableMetricEventArgs e)
		{
			SdpApp.AnalyticsManager.TrackMetric(e.MetricId, e.PID);
			Metric metric = MetricManager.Get().GetMetric(e.MetricId);
			if (metric != null)
			{
				if (e.Enable)
				{
					metric.Activate(e.PID, (uint)e.Mode);
					return;
				}
				metric.Deactivate(e.PID, (uint)e.Mode);
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00029FF8 File Offset: 0x000281F8
		private void deviceEvents_RetryInstallOnDevice(object sender, DeviceEventArgs e)
		{
			Device deviceByName = this.GetDeviceByName(e.LookupName);
			if (deviceByName != null)
			{
				deviceByName.RetryInstall();
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002A01C File Offset: 0x0002821C
		private void deviceEvents_ConnectToDevice(object sender, DeviceEventArgs e)
		{
			UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
			Device deviceByName = this.GetDeviceByName(e.LookupName);
			if (deviceByName != null)
			{
				string text = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ConnectionTimeout);
				string text2 = userPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.BaseNetworkPort);
				if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
				{
					uint num = UintConverter.Convert(text);
					uint num2 = UintConverter.Convert(text2);
					deviceByName.Connect(num, num2);
					return;
				}
				if (!string.IsNullOrEmpty(text))
				{
					uint num3 = UintConverter.Convert(text);
					deviceByName.Connect(num3);
					return;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					uint num4 = UintConverter.Convert(text2);
					deviceByName.Connect(3U, num4);
					return;
				}
				deviceByName.Connect();
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002A0C0 File Offset: 0x000282C0
		private void deviceEvents_DisconnectFromDevice(object sender, DeviceEventArgs e)
		{
			this.UnregisterEvents();
			Device deviceByName = this.GetDeviceByName(e.LookupName);
			if (deviceByName != null)
			{
				deviceByName.Disconnect();
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002A0E9 File Offset: 0x000282E9
		private void deviceEvents_ClientConnectACK(object sender, EventArgs e)
		{
			if (this.GetDeviceOS() != ConnectionManager.DeviceOS.Android)
			{
				UIManager uimanager = SdpApp.UIManager;
				if (uimanager == null)
				{
					return;
				}
				uimanager.HideLayoutMenuItem("CPU Sampling");
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002A107 File Offset: 0x00028307
		private void clientEvents_AppShutdown(object sender, EventArgs e)
		{
			this.UnregisterEvents();
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002A110 File Offset: 0x00028310
		private void UnregisterEvents()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.PauseCapture = (EventHandler<PauseCaptureEventArgs>)Delegate.Remove(connectionEvents.PauseCapture, new EventHandler<PauseCaptureEventArgs>(this.connectionEvents_PauseCapture));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.SnapshotRequest = (EventHandler)Delegate.Remove(connectionEvents2.SnapshotRequest, new EventHandler(this.connectionEvents_SnapshotRequest));
			ConnectionEvents connectionEvents3 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents3.CancelSnapshotRequest = (EventHandler)Delegate.Remove(connectionEvents3.CancelSnapshotRequest, new EventHandler(this.connectionEvents_CancelSnapshotRequest));
			ConnectionEvents connectionEvents4 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents4.StartCaptureRequest = (EventHandler<TakeCaptureArgs>)Delegate.Remove(connectionEvents4.StartCaptureRequest, new EventHandler<TakeCaptureArgs>(this.connectionEvents_StartCaptureRequest));
			ConnectionEvents connectionEvents5 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents5.StartSamplingRequest = (EventHandler)Delegate.Remove(connectionEvents5.StartSamplingRequest, new EventHandler(this.connectionEvents_StartSamplingRequest));
			ConnectionEvents connectionEvents6 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents6.StopCaptureRequest = (EventHandler)Delegate.Remove(connectionEvents6.StopCaptureRequest, new EventHandler(this.connectionEvents_StopCaptureRequest));
			ConnectionEvents connectionEvents7 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents7.StopSamplingRequest = (EventHandler)Delegate.Remove(connectionEvents7.StopSamplingRequest, new EventHandler(this.connectionEvents_StopSamplingRequest));
			ConnectionEvents connectionEvents8 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents8.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Remove(connectionEvents8.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.RetryInstallOnDevice = (EventHandler<DeviceEventArgs>)Delegate.Remove(deviceEvents.RetryInstallOnDevice, new EventHandler<DeviceEventArgs>(this.deviceEvents_RetryInstallOnDevice));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ConnectToDevice = (EventHandler<DeviceEventArgs>)Delegate.Remove(deviceEvents2.ConnectToDevice, new EventHandler<DeviceEventArgs>(this.deviceEvents_ConnectToDevice));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.AppShutdown = (EventHandler<EventArgs>)Delegate.Remove(clientEvents.AppShutdown, new EventHandler<EventArgs>(this.clientEvents_AppShutdown));
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0002A2F6 File Offset: 0x000284F6
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0002A2FD File Offset: 0x000284FD
		public static ILogger Logger { private get; set; } = new Sdp.Logging.Logger("ConnectionManager");

		// Token: 0x04000967 RID: 2407
		public List<uint> RegisteredMetricsList;

		// Token: 0x04000968 RID: 2408
		public Dictionary<SDPDataType, Delegate> DelegateDictionary;

		// Token: 0x04000969 RID: 2409
		public readonly List<LaunchAppDialogOption> LaunchAppDialogOptions = new List<LaunchAppDialogOption>();

		// Token: 0x0400096A RID: 2410
		private Dictionary<CaptureType, SortedSet<RenderingAPI>> m_supportedCaptureTypes = new Dictionary<CaptureType, SortedSet<RenderingAPI>>();

		// Token: 0x0400096B RID: 2411
		private InternalClientDelegate m_clientDelegate;

		// Token: 0x0400096C RID: 2412
		private InternalMetricDelegate m_metricDelegate;

		// Token: 0x0400096D RID: 2413
		private InternalMetricCategoryDelegate m_metricCategoryDelegate;

		// Token: 0x0400096E RID: 2414
		private Client m_client;

		// Token: 0x0400096F RID: 2415
		private ProgressObject m_exportProgressObject = new ProgressObject();

		// Token: 0x04000970 RID: 2416
		private Void_UInt_Fn m_exportMetricProgressFPtr;

		// Token: 0x04000971 RID: 2417
		private static string m_connectedDeviceRendererString = "";

		// Token: 0x04000972 RID: 2418
		private static string m_currentRendererString = "";

		// Token: 0x04000973 RID: 2419
		private static ConnectionManager.DeviceOS m_currentDeviceOS = ConnectionManager.DeviceOS.Other;

		// Token: 0x04000974 RID: 2420
		public ulong firstTimestamp;

		// Token: 0x04000975 RID: 2421
		public bool haveFirst;

		// Token: 0x020003D1 RID: 977
		// (Invoke) Token: 0x0600127C RID: 4732
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnDoubleValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, double value);

		// Token: 0x020003D2 RID: 978
		// (Invoke) Token: 0x06001280 RID: 4736
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnCustomValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, IntPtr value);

		// Token: 0x020003D3 RID: 979
		// (Invoke) Token: 0x06001284 RID: 4740
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnFloatValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, float value);

		// Token: 0x020003D4 RID: 980
		// (Invoke) Token: 0x06001288 RID: 4744
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnInt8ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, sbyte value);

		// Token: 0x020003D5 RID: 981
		// (Invoke) Token: 0x0600128C RID: 4748
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnUInt8ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, byte value);

		// Token: 0x020003D6 RID: 982
		// (Invoke) Token: 0x06001290 RID: 4752
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnInt16ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, short value);

		// Token: 0x020003D7 RID: 983
		// (Invoke) Token: 0x06001294 RID: 4756
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnUInt16ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, ushort value);

		// Token: 0x020003D8 RID: 984
		// (Invoke) Token: 0x06001298 RID: 4760
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnInt32ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, int value);

		// Token: 0x020003D9 RID: 985
		// (Invoke) Token: 0x0600129C RID: 4764
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnUInt32ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, uint value);

		// Token: 0x020003DA RID: 986
		// (Invoke) Token: 0x060012A0 RID: 4768
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnInt64ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, long value);

		// Token: 0x020003DB RID: 987
		// (Invoke) Token: 0x060012A4 RID: 4772
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnUInt64ValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, ulong value);

		// Token: 0x020003DC RID: 988
		// (Invoke) Token: 0x060012A8 RID: 4776
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnStringValueReceivedDelegate(IntPtr metric, IntPtr stream, ulong timestamp, IntPtr value);

		// Token: 0x020003DD RID: 989
		public enum DeviceOS
		{
			// Token: 0x04000D80 RID: 3456
			Android,
			// Token: 0x04000D81 RID: 3457
			Windows,
			// Token: 0x04000D82 RID: 3458
			AGL,
			// Token: 0x04000D83 RID: 3459
			[Obsolete("LE devices are no longer supported", true)]
			LE,
			// Token: 0x04000D84 RID: 3460
			QCLinux,
			// Token: 0x04000D85 RID: 3461
			QNX,
			// Token: 0x04000D86 RID: 3462
			[Obsolete("Integrity devices are no longer supported", true)]
			Integrity,
			// Token: 0x04000D87 RID: 3463
			HLM,
			// Token: 0x04000D88 RID: 3464
			LinuxSSH,
			// Token: 0x04000D89 RID: 3465
			Other
		}
	}
}
