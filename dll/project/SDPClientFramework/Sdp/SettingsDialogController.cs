using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x02000278 RID: 632
	public class SettingsDialogController : IDialogController
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x0001EF20 File Offset: 0x0001D120
		public async Task<bool> ShowDialog()
		{
			bool flag = await this.m_view.ShowDialog();
			bool flag2 = flag;
			if (flag2)
			{
				string text = "A path entered does not exist, ignore if intended.\n Invalid Paths:\n";
				bool flag3 = false;
				foreach (KeyValuePair<UserPreferenceModel.UserPreference, PropertiesItem> keyValuePair in this.m_settings)
				{
					if (keyValuePair.Value.Value != null)
					{
						SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(keyValuePair.Key, keyValuePair.Value.Value.ToString());
					}
					if (keyValuePair.Value.Error)
					{
						flag3 = true;
						string[] array = new string[6];
						array[0] = text;
						array[1] = "    -";
						array[2] = keyValuePair.Value.Name;
						array[3] = " : ";
						int num = 4;
						object value = keyValuePair.Value.Value;
						array[num] = ((value != null) ? value.ToString() : null);
						array[5] = "\n ";
						text = string.Concat(array);
					}
				}
				if (flag3)
				{
					ShowMessageDialogCommand.ShowErrorDialog(text);
				}
			}
			return flag2;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001EF64 File Offset: 0x0001D164
		public SettingsDialogController(ISettingsDialog view)
		{
			this.m_view = view;
			this.m_propertiesWidgetController = new PropertiesWidgetController(this.m_view.PropertiesWidgetView);
			PropertiesItemSpinner propertiesItemSpinner = new PropertiesItemSpinner
			{
				Min = 1.0,
				Max = 120.0,
				Step = 1.0,
				Name = "Connection timeout (seconds)",
				Description = "Amount of time in seconds to wait for a connection attempt. If the timeout is reached the current connection attempt is aborted.",
				Category = "Device"
			};
			string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ConnectionTimeout);
			if (!string.IsNullOrEmpty(text))
			{
				propertiesItemSpinner.Value = UintConverter.Convert(text);
			}
			else
			{
				propertiesItemSpinner.Value = 30.0;
			}
			this.m_settings.Add(UserPreferenceModel.UserPreference.ConnectionTimeout, propertiesItemSpinner);
			this.m_propertiesWidgetController.AddProperty(propertiesItemSpinner);
			PropertiesItemSpinner propertiesItemSpinner2 = new PropertiesItemSpinner
			{
				Min = 1000.0,
				Max = 32767.0,
				Step = 1.0,
				Name = "Base network port",
				Description = "The base network port that Snapdragon Profiler will use to communicate with devices. A range of ports starting with this one and incrementing upwards will be used. Changes to this value will not affect any existing device connections.",
				Category = "Device"
			};
			string text2 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.BaseNetworkPort);
			if (!string.IsNullOrEmpty(text2))
			{
				propertiesItemSpinner2.Value = UintConverter.Convert(text2);
			}
			else
			{
				propertiesItemSpinner2.Value = 6500.0;
			}
			this.m_settings.Add(UserPreferenceModel.UserPreference.BaseNetworkPort, propertiesItemSpinner2);
			this.m_propertiesWidgetController.AddProperty(propertiesItemSpinner2);
			PropertiesItemSpinner propertiesItemSpinner3 = new PropertiesItemSpinner
			{
				Min = 20.0,
				Max = 120.0,
				Step = 1.0,
				Name = "Installer timeout (seconds)",
				Description = "Amount of time in seconds to allow for installation of service files. If the timeout is reached the installation is aborted.",
				Category = "Device"
			};
			string text3 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.InstallerTimeout);
			if (!string.IsNullOrEmpty(text3))
			{
				propertiesItemSpinner3.Value = UintConverter.Convert(text3);
			}
			else
			{
				propertiesItemSpinner3.Value = 30.0;
			}
			this.m_settings.Add(UserPreferenceModel.UserPreference.InstallerTimeout, propertiesItemSpinner3);
			this.m_propertiesWidgetController.AddProperty(propertiesItemSpinner3);
			string text4 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit);
			bool flag = text4 != null && BoolConverter.Convert(text4);
			PropertiesItem propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Clean up on exit";
			propertiesItem.Description = "If disabled, Profiler will leave some files on target devices to avoid copying them again after the first time a device is connected. This significantly reduces the time required for subsequent Profiler connections to the same device.";
			propertiesItem.Category = "Device";
			propertiesItem.Value = flag;
			this.m_settings.Add(UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			bool flag2;
			bool.TryParse(SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AutoConnect), out flag2);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Autoconnect";
			propertiesItem.Description = "Automatically connect if a single device is detected";
			propertiesItem.Category = "Device";
			propertiesItem.Value = flag2;
			this.m_settings.Add(UserPreferenceModel.UserPreference.AutoConnect, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			string text5 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.SessionLocation);
			PropertiesItem propertiesItem2 = new PropertiesItem
			{
				ItemType = ItemType.FolderPath,
				Name = "Sessions folder root path (restart required)",
				Description = "The root directory where Snapdragon Profiler session files will be saved. Each session will be saved in a timestamped subdirectory.",
				Category = "Sessions",
				Value = text5
			};
			this.m_settings.Add(UserPreferenceModel.UserPreference.SessionLocation, propertiesItem2);
			this.m_propertiesWidgetController.AddProperty(propertiesItem2);
			PropertiesItemSpinner propertiesItemSpinner4 = new PropertiesItemSpinner
			{
				Min = 0.0,
				Max = 50000.0,
				Step = 100.0,
				Name = "Maximum Snapdragon Profiler sessions size (MB)",
				Description = "The maximum size (in MB) that will be used to store Snapdragon Profiler session files. Files from previous sessions will be deleted, beginning with the oldest, to keep the total size under this limit. ",
				Category = "Sessions",
				Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSettingValueOrUseDefault<double>(UserPreferenceModel.UserPreference.MaxSessionsSizeMB)
			};
			this.m_settings.Add(UserPreferenceModel.UserPreference.MaxSessionsSizeMB, propertiesItemSpinner4);
			this.m_propertiesWidgetController.AddProperty(propertiesItemSpinner4);
			string text6 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DeleteEmptyGraphTracks);
			bool flag3 = text6 == null || BoolConverter.Convert(text6);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Remove empty tracks automatically";
			propertiesItem.Description = "If enabled, a track within the real-time or trace captures will be deleted if all metrics it contains are deleted or moved to another track.";
			propertiesItem.Category = "Capture";
			propertiesItem.Value = flag3;
			this.m_settings.Add(UserPreferenceModel.UserPreference.DeleteEmptyGraphTracks, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			string text7 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ShowDeleteEmptyGraphTracksDialog);
			bool flag4 = text7 == null || BoolConverter.Convert(text7);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Confirm before removing empty tracks";
			propertiesItem.Description = "If enabled, when all metrics are removed from a track, the user will be asked if the track should be deleted or not.";
			propertiesItem.Category = "Capture";
			propertiesItem.Value = flag4;
			this.m_settings.Add(UserPreferenceModel.UserPreference.ShowDeleteEmptyGraphTracksDialog, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			PropertiesItemSpinner propertiesItemSpinner5 = new PropertiesItemSpinner
			{
				Min = 500.0,
				Max = 10000.0,
				Step = 100.0,
				Name = "Maximum Trace capture duration (ms)",
				Description = "Maximum duration of a Trace capture. If a capture isn't stopped within this duration Snapdragon Profiler will automatically stop it.",
				Category = "Capture"
			};
			string text8 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.MaxCaptureDurationMs);
			if (!string.IsNullOrEmpty(text8))
			{
				propertiesItemSpinner5.Value = UintConverter.Convert(text8);
			}
			else
			{
				propertiesItemSpinner5.Value = 2000.0;
			}
			this.m_settings.Add(UserPreferenceModel.UserPreference.MaxCaptureDurationMs, propertiesItemSpinner5);
			this.m_propertiesWidgetController.AddProperty(propertiesItemSpinner5);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.FolderPath;
			propertiesItem.Name = "Vulkan SDK path";
			propertiesItem.Description = "Sets the location of the Vulkan SDK. Snapdragon Profiler will use spirv-dis to convert binary SPIR-V to human readable SPIR-V.";
			propertiesItem.Category = "Capture";
			propertiesItem.Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.VulkanSDKPath);
			this.m_settings.Add(UserPreferenceModel.UserPreference.VulkanSDKPath, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.FolderPath;
			propertiesItem.Name = "ADB path (restart required)";
			propertiesItem.Description = "Sets the location of ADB. If empty, Snapdragon Profiler will try to get the location from the system path. If this value is modified Snapdragon Profiler needs to be restarted for the change to take effect.";
			propertiesItem.Category = "Android";
			propertiesItem.Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ADBPath);
			this.m_settings.Add(UserPreferenceModel.UserPreference.ADBPath, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.FolderPath;
			propertiesItem.Name = "Android NDK path (restart required)";
			propertiesItem.Description = "Sets the location of the Android NDK";
			propertiesItem.Category = "Android";
			propertiesItem.Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.AndroidNDKPath);
			this.m_settings.Add(UserPreferenceModel.UserPreference.AndroidNDKPath, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			string text9 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.ShowADBVersionDialog);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Check ADB version";
			propertiesItem.Description = "If ADB version is below " + SdpApp.ModelManager.ConnectionModel.MinimumADBVersion + ", Snapdragon Profiler will notify the user that it will not work for device connections via USB.";
			propertiesItem.Category = "Android";
			propertiesItem.Value = string.IsNullOrEmpty(text9) || BoolConverter.Convert(text9);
			this.m_settings.Add(UserPreferenceModel.UserPreference.ShowADBVersionDialog, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			string text10 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.DisableUGD);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Disable pre-release graphics driver";
			propertiesItem.Description = "Apps will use pre-release graphics drivers (if available, Android 10+) when launched via Launch Application unless this box is checked.";
			propertiesItem.Category = "Android";
			propertiesItem.Value = !string.IsNullOrEmpty(text10) && BoolConverter.Convert(text10);
			this.m_settings.Add(UserPreferenceModel.UserPreference.DisableUGD, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			string text11 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.EnablePerfetto);
			bool flag5 = text11 == null || BoolConverter.Convert(text11);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.Checkbox;
			propertiesItem.Name = "Enable Perfetto trace (restart required)";
			propertiesItem.Description = "Disable this if Perfetto metrics are not captured correctly, which will revert to using Systrace instead. Note that Systrace will automatically be used on devices that do not support Perfetto. If this value is modified Snapdragon Profiler needs to be restarted for the change to take effect.";
			propertiesItem.Category = "Android";
			propertiesItem.Value = flag5;
			this.m_settings.Add(UserPreferenceModel.UserPreference.EnablePerfetto, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			propertiesItem = new PropertiesItem();
			propertiesItem.ItemType = ItemType.FolderPath;
			propertiesItem.Name = "SSH path (restart required)";
			propertiesItem.Description = "Sets the location of an SSH client (used for connecting to some types of devices). If empty, Snapdragon Profiler will try to get the location from the system path. If this value is modified Snapdragon Profiler needs to be restarted for the change to take effect.";
			propertiesItem.Category = "SSH";
			propertiesItem.Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.SSHPath);
			this.m_settings.Add(UserPreferenceModel.UserPreference.SSHPath, propertiesItem);
			this.m_propertiesWidgetController.AddProperty(propertiesItem);
			PropertiesItem propertiesItem3 = new PropertiesItem
			{
				ItemType = ItemType.ColorPicker,
				Name = "Low Value Color",
				Description = "Color to be used for low values when displaying a heat map, i.e. trace drawcall blocks",
				Category = "Accessibility",
				Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.PerformanceLowColor)
			};
			this.m_settings.Add(UserPreferenceModel.UserPreference.PerformanceLowColor, propertiesItem3);
			this.m_propertiesWidgetController.AddProperty(propertiesItem3);
			PropertiesItem propertiesItem4 = new PropertiesItem
			{
				ItemType = ItemType.ColorPicker,
				Name = "High Value Color",
				Description = "Color to be used for high values when recoloring trace's drawcall blocks based on metrics",
				Category = "Accessibility",
				Value = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.PerformanceHighColor)
			};
			this.m_settings.Add(UserPreferenceModel.UserPreference.PerformanceHighColor, propertiesItem4);
			this.m_propertiesWidgetController.AddProperty(propertiesItem4);
		}

		// Token: 0x04000899 RID: 2201
		private PropertiesWidgetController m_propertiesWidgetController;

		// Token: 0x0400089A RID: 2202
		private ISettingsDialog m_view;

		// Token: 0x0400089B RID: 2203
		private readonly Dictionary<UserPreferenceModel.UserPreference, PropertiesItem> m_settings = new Dictionary<UserPreferenceModel.UserPreference, PropertiesItem>();
	}
}
