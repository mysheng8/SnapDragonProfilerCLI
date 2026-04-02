using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Cairo;
using Sdp.Functional;
using Sdp.Helpers;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020001FD RID: 509
	public class UserPreferenceModel
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0001365C File Offset: 0x0001185C
		public string UserPreferenceLocation
		{
			get
			{
				return Environment.GetEnvironmentVariable("SDP_USER_PREFERENCE_LOCATION").ToMaybe().Match<string>((string path) => path, () => UserPreferenceModel.DefaultUserPreferenceLocation);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x000136BB File Offset: 0x000118BB
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x000136C3 File Offset: 0x000118C3
		public List<LaunchHistory> LaunchedAppHistory { get; set; } = new List<LaunchHistory>();

		// Token: 0x0600075C RID: 1884 RVA: 0x000136CC File Offset: 0x000118CC
		public UserPreferenceModel(UserPreferenceModel.UserPreferenceFileReader settingsFileReader)
		{
			this.UserPreferenceFilePath = global::System.IO.Path.Combine(this.UserPreferenceLocation, "SDPUserPreferences.pref");
			this.UserDeviceSettingsFilePath = global::System.IO.Path.Combine(this.UserPreferenceLocation, "SDPUserConfiguration.json");
			IEnumerable<string> enumerable = settingsFileReader(this.UserPreferenceFilePath);
			this.LoadSettings(enumerable);
			this.GeneratePreferedLowToHighPerformanceColors();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001376C File Offset: 0x0001196C
		public UserPreferenceModel()
			: this(new UserPreferenceModel.UserPreferenceFileReader(UserPreferenceModel.TryReadPreferenceFile))
		{
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00013780 File Offset: 0x00011980
		private static IEnumerable<string> TryReadPreferenceFile(string prefFilePath)
		{
			if (File.Exists(prefFilePath))
			{
				return File.ReadLines(prefFilePath);
			}
			return new string[0];
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00013798 File Offset: 0x00011998
		private void LoadSettings(IEnumerable<string> textLines)
		{
			this.m_UserPreferencesDictionary.Clear();
			Dictionary<UserPreferenceModel.UserPreference, string> dictionary = new Dictionary<UserPreferenceModel.UserPreference, string>();
			bool flag = !this.LoadDeviceConnectionSettings();
			foreach (string text in textLines)
			{
				char[] array = new char[] { '=' };
				string[] array2 = text.Split(array);
				if (array2.Length == 2)
				{
					try
					{
						UserPreferenceModel.UserPreference userPreference = (UserPreferenceModel.UserPreference)Enum.Parse(typeof(UserPreferenceModel.UserPreference), array2[0]);
						if (flag && this.IsDeviceConfiguration(userPreference))
						{
							dictionary[userPreference] = array2[1];
						}
						else
						{
							this.m_UserPreferencesDictionary[userPreference] = array2[1];
						}
					}
					catch (ArgumentException)
					{
					}
				}
			}
			string text2;
			if (this.m_UserPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.MaxCaptureDuration, out text2) && !this.m_UserPreferencesDictionary.ContainsKey(UserPreferenceModel.UserPreference.MaxCaptureDurationMs))
			{
				uint num = UintConverter.Convert(text2) * 1000U;
				if (num < 500U)
				{
					num = 500U;
				}
				if (num > 10000U)
				{
					num = 10000U;
				}
				this.m_UserPreferencesDictionary[UserPreferenceModel.UserPreference.MaxCaptureDurationMs] = num.ToString();
			}
			if (dictionary.Keys.Count > 0)
			{
				this.MigrateToDeviceConnectionSettings(dictionary);
			}
			this.LoadLaunchedAppHistory();
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000138E8 File Offset: 0x00011AE8
		private void MigrateToDeviceConnectionSettings(Dictionary<UserPreferenceModel.UserPreference, string> tempPreferencesDictionary)
		{
			string text;
			if (tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXIPAddress, out text))
			{
				this.GenerateDeviceConnectionSetting(tempPreferencesDictionary, ConnectionManager.DeviceOS.QNX, text);
			}
			if (tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.AGLIPAddress, out text))
			{
				this.GenerateDeviceConnectionSetting(tempPreferencesDictionary, ConnectionManager.DeviceOS.AGL, text);
			}
			if (tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.WinARMIPAddress, out text))
			{
				this.GenerateDeviceConnectionSetting(tempPreferencesDictionary, ConnectionManager.DeviceOS.Windows, text);
			}
			if (tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.LinuxSSHIPAddress, out text))
			{
				this.GenerateDeviceConnectionSetting(tempPreferencesDictionary, ConnectionManager.DeviceOS.LinuxSSH, text);
			}
			string text2 = "";
			if (tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.HLM_EnableConnection, out text2) && BoolConverter.Convert(text2))
			{
				this.GenerateDeviceConnectionSetting(tempPreferencesDictionary, ConnectionManager.DeviceOS.HLM, "127.0.0.1");
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00013970 File Offset: 0x00011B70
		private void GenerateDeviceConnectionSetting(Dictionary<UserPreferenceModel.UserPreference, string> tempPreferencesDictionary, ConnectionManager.DeviceOS os, string ip)
		{
			string text = ConnectionManager.GetDeviceDisplayName(os) + " Device";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = 10.ToString();
			string text6 = "";
			string text7 = "";
			switch (os)
			{
			case ConnectionManager.DeviceOS.AGL:
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.AGLUserName, out text2);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.AGLPassWord, out text3);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.AGLDeployDirectory, out text4);
				break;
			case ConnectionManager.DeviceOS.QNX:
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXUserName, out text2);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXPassWord, out text3);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXDeployDirectory, out text4);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXProcessPriority, out text5);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXSSHIdentityFile, out text6);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.QNXConnectionType, out text7);
				break;
			case ConnectionManager.DeviceOS.LinuxSSH:
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.LinuxSSHUserName, out text2);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.LinuxSSHPassWord, out text3);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.LinuxSSHIdentityFile, out text6);
				tempPreferencesDictionary.TryGetValue(UserPreferenceModel.UserPreference.LinuxSSHDeployDirectory, out text4);
				break;
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "";
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "";
			}
			if (string.IsNullOrEmpty(text4))
			{
				text4 = "";
			}
			if (string.IsNullOrEmpty(text6))
			{
				text6 = "";
			}
			if (string.IsNullOrEmpty(text5))
			{
				text5 = 10.ToString();
			}
			if (string.IsNullOrEmpty(text7))
			{
				text7 = "Telnet";
			}
			string text8 = Guid.NewGuid().ToString();
			ConnectionSettings connectionSettings = new ConnectionSettings(true);
			connectionSettings.ConfiguredDeviceOS = os;
			connectionSettings.DisplayName = text;
			connectionSettings.GUID = text8;
			connectionSettings.LookupName = text8;
			connectionSettings.Edited = true;
			connectionSettings.HostIP = ip;
			connectionSettings.Username = text2;
			connectionSettings.SetandEncryptPass(text3);
			connectionSettings.DeployDir = text4;
			connectionSettings.IdentityFile = text6;
			connectionSettings.ProcessPriority = IntConverter.Convert(text5);
			connectionSettings.ConnectionType = text7;
			this.SaveDevice(connectionSettings);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00013B6C File Offset: 0x00011D6C
		private void RecordSettings(Func<string, TextWriter> textWriterFactory)
		{
			TextWriter textWriter = textWriterFactory(this.UserPreferenceFilePath);
			foreach (UserPreferenceModel.UserPreference userPreference in this.m_UserPreferencesDictionary.Keys)
			{
				if (!this.IsDeviceConfiguration(userPreference))
				{
					string text = this.m_UserPreferencesDictionary[userPreference];
					textWriter.WriteLine(userPreference.ToString() + "=" + text);
				}
			}
			textWriter.Close();
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00013C04 File Offset: 0x00011E04
		private bool IsDeviceConfiguration(UserPreferenceModel.UserPreference pref)
		{
			switch (pref)
			{
			case UserPreferenceModel.UserPreference.QNXIPAddress:
			case UserPreferenceModel.UserPreference.QNXUserName:
			case UserPreferenceModel.UserPreference.QNXPassWord:
			case UserPreferenceModel.UserPreference.QNXDeployDirectory:
			case UserPreferenceModel.UserPreference.QNXProcessPriority:
			case UserPreferenceModel.UserPreference.QNXSSHIdentityFile:
			case UserPreferenceModel.UserPreference.QNXConnectionType:
			case UserPreferenceModel.UserPreference.AGLIPAddress:
			case UserPreferenceModel.UserPreference.AGLUserName:
			case UserPreferenceModel.UserPreference.AGLPassWord:
			case UserPreferenceModel.UserPreference.AGLDeployDirectory:
			case UserPreferenceModel.UserPreference.LinuxSSHIPAddress:
			case UserPreferenceModel.UserPreference.LinuxSSHUserName:
			case UserPreferenceModel.UserPreference.LinuxSSHPassWord:
			case UserPreferenceModel.UserPreference.LinuxSSHIdentityFile:
			case UserPreferenceModel.UserPreference.LinuxSSHDeployDirectory:
			case UserPreferenceModel.UserPreference.HLM_EnableConnection:
			case UserPreferenceModel.UserPreference.WinARMIPAddress:
				return true;
			}
			return false;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00013C98 File Offset: 0x00011E98
		public void SaveDevice(ConnectionSettings settings)
		{
			if (settings.Edited)
			{
				this.m_deviceConfigurations[settings.GUID] = settings;
				string text = this.serializer.Serialize(this.m_deviceConfigurations);
				try
				{
					File.WriteAllText(this.UserDeviceSettingsFilePath, text);
				}
				catch (Exception ex)
				{
					this.m_logger.LogError("Error writing user configurations: " + ex.Message.ToString());
				}
				this.m_uneditedConfigurations.Remove(settings.GUID);
				return;
			}
			this.m_uneditedConfigurations[settings.GUID] = settings;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00013D38 File Offset: 0x00011F38
		public void RemoveDevice(string guid, bool save)
		{
			this.m_deviceConfigurations.Remove(guid);
			if (save)
			{
				string text = this.serializer.Serialize(this.m_deviceConfigurations);
				try
				{
					File.WriteAllText(this.UserDeviceSettingsFilePath, text);
				}
				catch (Exception ex)
				{
					this.m_logger.LogError("Error writing user configurations: " + ex.Message.ToString());
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00013DA8 File Offset: 0x00011FA8
		public void RemoveDevice(ConnectionSettings settings, bool save)
		{
			bool flag = false;
			foreach (KeyValuePair<string, ConnectionSettings> keyValuePair in this.m_deviceConfigurations)
			{
				if (keyValuePair.Key == settings.GUID)
				{
					flag = true;
					this.m_deviceConfigurations.Remove(keyValuePair.Key);
					break;
				}
			}
			if (!flag)
			{
				foreach (KeyValuePair<string, ConnectionSettings> keyValuePair2 in this.m_uneditedConfigurations)
				{
					if (keyValuePair2.Key == settings.GUID)
					{
						flag = true;
						this.m_uneditedConfigurations.Remove(keyValuePair2.Key);
						break;
					}
				}
			}
			if (save)
			{
				string text = this.serializer.Serialize(this.m_deviceConfigurations);
				try
				{
					File.WriteAllText(this.UserDeviceSettingsFilePath, text);
				}
				catch (Exception ex)
				{
					this.m_logger.LogError("Error writing user configurations: " + ex.Message.ToString());
				}
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00013EE4 File Offset: 0x000120E4
		public string GetDeviceGUID(string lookupName, ConnectionManager.DeviceOS os)
		{
			foreach (KeyValuePair<string, ConnectionSettings> keyValuePair in this.m_deviceConfigurations)
			{
				if (keyValuePair.Value.LookupName == lookupName && keyValuePair.Value.ConfiguredDeviceOS == os)
				{
					return keyValuePair.Value.GUID;
				}
			}
			foreach (KeyValuePair<string, ConnectionSettings> keyValuePair2 in this.m_uneditedConfigurations)
			{
				if (keyValuePair2.Value.LookupName == lookupName && keyValuePair2.Value.ConfiguredDeviceOS == os)
				{
					return keyValuePair2.Value.GUID;
				}
			}
			return "";
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00013FDC File Offset: 0x000121DC
		private bool LoadDeviceConnectionSettings()
		{
			if (File.Exists(this.UserDeviceSettingsFilePath))
			{
				try
				{
					string text = File.ReadAllText(this.UserDeviceSettingsFilePath);
					if (!string.IsNullOrEmpty(text))
					{
						this.m_deviceConfigurations = this.serializer.Deserialize<Dictionary<string, ConnectionSettings>>(text);
						return true;
					}
				}
				catch (Exception ex)
				{
					this.m_logger.LogError("Error reading user configurations: " + ex.Message.ToString());
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00014058 File Offset: 0x00012258
		private bool LoadLaunchedAppHistory()
		{
			string defaultValue = "";
			this.TryRetrieveSetting<string>(UserPreferenceModel.UserPreference.LastLaunchedApp).IfSome(delegate(string prefValue)
			{
				defaultValue = prefValue;
			});
			if (!string.IsNullOrEmpty(defaultValue))
			{
				this.LaunchedAppHistory = this.serializer.Deserialize<List<LaunchHistory>>(defaultValue);
				return true;
			}
			return false;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000140B8 File Offset: 0x000122B8
		public void SaveLaunchedAppHistory(LaunchHistory history)
		{
			int num = 5;
			this.LaunchedAppHistory.RemoveAll((LaunchHistory tmpHistory) => tmpHistory.IsMatchingActivity(history));
			this.LaunchedAppHistory.Add(history);
			while (this.LaunchedAppHistory.Count > num)
			{
				this.LaunchedAppHistory.RemoveAt(0);
			}
			string text = this.serializer.Serialize(this.LaunchedAppHistory);
			this.RecordSetting(UserPreferenceModel.UserPreference.LastLaunchedApp, text);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00014134 File Offset: 0x00012334
		private static string GenerateEncryptKey()
		{
			string text;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("SDPD3cKeyPhr@sE!", Encoding.UTF8.GetBytes("SDPD3cKeyS@lT2"), 10000))
			{
				text = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(16));
			}
			return text;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001418C File Offset: 0x0001238C
		public static string EncryptPassword(string pass)
		{
			string text;
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(UserPreferenceModel.m_decriptKey);
				aes.IV = new byte[16];
				ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(pass);
						}
					}
					text = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			return text;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00014264 File Offset: 0x00012464
		public static string DecryptPassword(string encryptedPass)
		{
			if (string.IsNullOrEmpty(encryptedPass))
			{
				return "";
			}
			string text;
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(UserPreferenceModel.m_decriptKey);
				aes.IV = new byte[16];
				ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedPass)))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(cryptoStream))
						{
							text = streamReader.ReadToEnd();
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00014344 File Offset: 0x00012544
		public bool FindDevice(string guid, out ConnectionSettings cs)
		{
			if (this.m_deviceConfigurations.TryGetValue(guid, out cs))
			{
				return true;
			}
			if (this.m_uneditedConfigurations.TryGetValue(guid, out cs))
			{
				return true;
			}
			cs = new ConnectionSettings(false);
			return false;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00014374 File Offset: 0x00012574
		public bool FindDevice(ConnectionManager.DeviceOS os, string lookupName, out ConnectionSettings cs)
		{
			bool flag = SdpApp.ConnectionManager.IsDeviceAutoDiscovered(lookupName);
			foreach (ConnectionSettings connectionSettings in this.m_deviceConfigurations.Values)
			{
				if (connectionSettings.ConfiguredDeviceOS == os && (!flag || lookupName == connectionSettings.LookupName))
				{
					cs = connectionSettings;
					return true;
				}
			}
			foreach (ConnectionSettings connectionSettings2 in this.m_uneditedConfigurations.Values)
			{
				if (connectionSettings2.ConfiguredDeviceOS == os && (!flag || lookupName == connectionSettings2.LookupName))
				{
					cs = connectionSettings2;
					return true;
				}
			}
			cs = new ConnectionSettings(false);
			return false;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00014464 File Offset: 0x00012664
		public List<string> GetAvailableOS()
		{
			Dictionary<ConnectionManager.DeviceOS, string> dictionary = (from ConnectionManager.DeviceOS x in Enum.GetValues(typeof(ConnectionManager.DeviceOS))
				where !this.GetNeverAvailableOS().Contains(x)
				select x).ToDictionary((ConnectionManager.DeviceOS x) => x, (ConnectionManager.DeviceOS x) => x.ToString());
			foreach (KeyValuePair<string, ConnectionSettings> keyValuePair in this.m_deviceConfigurations)
			{
				dictionary.Remove(keyValuePair.Value.ConfiguredDeviceOS);
			}
			return dictionary.Values.ToList<string>();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00014538 File Offset: 0x00012738
		private List<ConnectionManager.DeviceOS> GetNeverAvailableOS()
		{
			List<ConnectionManager.DeviceOS> list = new List<ConnectionManager.DeviceOS>
			{
				ConnectionManager.DeviceOS.Other,
				ConnectionManager.DeviceOS.Android,
				ConnectionManager.DeviceOS.QCLinux
			};
			list.Add(ConnectionManager.DeviceOS.HLM);
			return list;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00014569 File Offset: 0x00012769
		public int GetMaxOSAllowed()
		{
			return (from ConnectionManager.DeviceOS x in Enum.GetValues(typeof(ConnectionManager.DeviceOS))
				where !this.GetNeverAvailableOS().Contains(x)
				select x).Count<ConnectionManager.DeviceOS>();
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00014598 File Offset: 0x00012798
		public Dictionary<string, ConnectionSettings> ConnectionConfigurations
		{
			get
			{
				return this.m_deviceConfigurations.Concat(this.m_uneditedConfigurations).ToDictionary((KeyValuePair<string, ConnectionSettings> x) => x.Key, (KeyValuePair<string, ConnectionSettings> x) => x.Value);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000145FC File Offset: 0x000127FC
		private void GeneratePreferedLowToHighPerformanceColors()
		{
			string text = this.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.PerformanceLowColor);
			string text2 = this.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.PerformanceHighColor);
			string[] array = text.Split(new char[] { ',' });
			string[] array2 = text2.Split(new char[] { ',' });
			if (array.Length != 3 || array2.Length != 3)
			{
				this.m_lowToHighPerformanceColors = new Color[]
				{
					new Color(0.207, 0.509, 0.168),
					new Color(0.902, 0.243, 0.231)
				};
				return;
			}
			this.m_lowToHighPerformanceColors = new Color[]
			{
				new Color(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2])),
				new Color(double.Parse(array2[0]), double.Parse(array2[1]), double.Parse(array2[2]))
			};
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000146FA File Offset: 0x000128FA
		public Color[] GetLowToHighPerformanceColors()
		{
			return this.m_lowToHighPerformanceColors;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00014702 File Offset: 0x00012902
		private bool IsColorPerferenceConfiguration(UserPreferenceModel.UserPreference setting)
		{
			return setting - UserPreferenceModel.UserPreference.PerformanceLowColor <= 1;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001470E File Offset: 0x0001290E
		public T RetrieveSettingValueOrUseDefault<T>(UserPreferenceModel.UserPreference setting)
		{
			return this.TryRetrieveSetting<T>(setting).UnwrapOr((T)((object)UserPreferenceModel.DefaultValues[setting]));
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001472C File Offset: 0x0001292C
		public Maybe<T> TryRetrieveSetting<T>(UserPreferenceModel.UserPreference setting)
		{
			return this.RetrieveSetting(setting).ToMaybe().Bind<T>(delegate(string value)
			{
				Maybe<T> maybe;
				try
				{
					maybe = new Maybe<T>.Some((T)((object)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value)));
				}
				catch (Exception ex)
				{
					this.m_logger.LogWarning(string.Format("Invalid user settings found for {0}: {1}", setting, ex.Message));
					maybe = new Maybe<T>.None();
				}
				return maybe;
			});
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001476F File Offset: 0x0001296F
		public string RetrieveSetting(UserPreferenceModel.UserPreference setting)
		{
			if (this.m_UserPreferencesDictionary.ContainsKey(setting))
			{
				return this.m_UserPreferencesDictionary[setting];
			}
			return null;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001478D File Offset: 0x0001298D
		public void RecordSetting(UserPreferenceModel.UserPreference variable, string setting, Func<string, TextWriter> textWriterFactory)
		{
			this.m_UserPreferencesDictionary[variable] = setting;
			this.RecordSettings(textWriterFactory);
			if (this.IsColorPerferenceConfiguration(variable))
			{
				this.GeneratePreferedLowToHighPerformanceColors();
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000147B2 File Offset: 0x000129B2
		public void RecordSetting(UserPreferenceModel.UserPreference variable, string setting)
		{
			this.RecordSetting(variable, setting, delegate(string path)
			{
				string directoryName = global::System.IO.Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				return File.CreateText(path);
			});
		}

		// Token: 0x0400072A RID: 1834
		private Dictionary<UserPreferenceModel.UserPreference, string> m_UserPreferencesDictionary = new Dictionary<UserPreferenceModel.UserPreference, string>();

		// Token: 0x0400072B RID: 1835
		public const string USER_PREFERENCE_LOCATION_ENV_VAR = "SDP_USER_PREFERENCE_LOCATION";

		// Token: 0x0400072C RID: 1836
		public const uint MIN_CAPTURE_DURATION = 500U;

		// Token: 0x0400072D RID: 1837
		public const uint MAX_CAPTURE_DURATION = 10000U;

		// Token: 0x0400072E RID: 1838
		public const uint DEFAULT_CAPTURE_DURATION = 2000U;

		// Token: 0x0400072F RID: 1839
		public const uint MIN_PROCESS_PRIORITY = 2U;

		// Token: 0x04000730 RID: 1840
		public const uint MAX_PROCESS_PRIORITY = 64U;

		// Token: 0x04000731 RID: 1841
		public static readonly string DefaultUserPreferenceLocation = global::System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SDP");

		// Token: 0x04000732 RID: 1842
		public const string UserPreferenceFileName = "SDPUserPreferences.pref";

		// Token: 0x04000733 RID: 1843
		public static readonly string DefaultUserPreferenceFilePath = global::System.IO.Path.Combine(UserPreferenceModel.DefaultUserPreferenceLocation, "SDPUserPreferences.pref");

		// Token: 0x04000735 RID: 1845
		public readonly string UserPreferenceFilePath;

		// Token: 0x04000736 RID: 1846
		private ILogger m_logger = new Sdp.Logging.Logger("UserPreferenceModel");

		// Token: 0x04000737 RID: 1847
		private const string UserDeviceSettingsFileName = "SDPUserConfiguration.json";

		// Token: 0x04000738 RID: 1848
		private readonly string UserDeviceSettingsFilePath;

		// Token: 0x04000739 RID: 1849
		private readonly Dictionary<string, ConnectionSettings> m_uneditedConfigurations = new Dictionary<string, ConnectionSettings>();

		// Token: 0x0400073A RID: 1850
		private Dictionary<string, ConnectionSettings> m_deviceConfigurations = new Dictionary<string, ConnectionSettings>();

		// Token: 0x0400073B RID: 1851
		private JavaScriptSerializer serializer = new JavaScriptSerializer();

		// Token: 0x0400073C RID: 1852
		private static readonly string m_decriptKey = UserPreferenceModel.GenerateEncryptKey();

		// Token: 0x0400073D RID: 1853
		private Color[] m_lowToHighPerformanceColors;

		// Token: 0x0400073E RID: 1854
		public static readonly IReadOnlyDictionary<UserPreferenceModel.UserPreference, object> DefaultValues = new Dictionary<UserPreferenceModel.UserPreference, object>
		{
			{
				UserPreferenceModel.UserPreference.SessionLocation,
				global::System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SnapdragonProfiler")
			},
			{
				UserPreferenceModel.UserPreference.MaxSessionsSizeMB,
				2000.0
			},
			{
				UserPreferenceModel.UserPreference.ClientID,
				"-1"
			},
			{
				UserPreferenceModel.UserPreference.PerformanceLowColor,
				"0.207,0.509,0.168"
			},
			{
				UserPreferenceModel.UserPreference.PerformanceHighColor,
				"0.902,0.243,0.231"
			},
			{
				UserPreferenceModel.UserPreference.ShowDataExplorerFancyTooltip,
				true
			}
		};

		// Token: 0x02000397 RID: 919
		public enum UserPreference
		{
			// Token: 0x04000C8B RID: 3211
			ShowDeleteEmptyGraphTracksDialog,
			// Token: 0x04000C8C RID: 3212
			DeleteEmptyGraphTracks,
			// Token: 0x04000C8D RID: 3213
			UserAnalyticsOptOut,
			// Token: 0x04000C8E RID: 3214
			ADBPath,
			// Token: 0x04000C8F RID: 3215
			SSHPath,
			// Token: 0x04000C90 RID: 3216
			ConnectionTimeout,
			// Token: 0x04000C91 RID: 3217
			MaxCaptureDuration,
			// Token: 0x04000C92 RID: 3218
			ShowADBVersionDialog,
			// Token: 0x04000C93 RID: 3219
			ShowCrashReportDialog,
			// Token: 0x04000C94 RID: 3220
			CheckDeviceBuild,
			// Token: 0x04000C95 RID: 3221
			EnablePerformanceHints,
			// Token: 0x04000C96 RID: 3222
			BaseNetworkPort,
			// Token: 0x04000C97 RID: 3223
			QNXIPAddress,
			// Token: 0x04000C98 RID: 3224
			QNXUserName,
			// Token: 0x04000C99 RID: 3225
			QNXPassWord,
			// Token: 0x04000C9A RID: 3226
			QNXDeployDirectory,
			// Token: 0x04000C9B RID: 3227
			QNXProcessPriority,
			// Token: 0x04000C9C RID: 3228
			QNXSSHIdentityFile,
			// Token: 0x04000C9D RID: 3229
			QNXConnectionType,
			// Token: 0x04000C9E RID: 3230
			AGLIPAddress,
			// Token: 0x04000C9F RID: 3231
			AGLUserName,
			// Token: 0x04000CA0 RID: 3232
			AGLPassWord,
			// Token: 0x04000CA1 RID: 3233
			AGLDeployDirectory,
			// Token: 0x04000CA2 RID: 3234
			LinuxSSHIPAddress,
			// Token: 0x04000CA3 RID: 3235
			LinuxSSHUserName,
			// Token: 0x04000CA4 RID: 3236
			LinuxSSHPassWord,
			// Token: 0x04000CA5 RID: 3237
			LinuxSSHIdentityFile,
			// Token: 0x04000CA6 RID: 3238
			LinuxSSHDeployDirectory,
			// Token: 0x04000CA7 RID: 3239
			AutomaticallyEnableStoragePermissions,
			// Token: 0x04000CA8 RID: 3240
			SimpleperfPath,
			// Token: 0x04000CA9 RID: 3241
			DisableUGD,
			// Token: 0x04000CAA RID: 3242
			AndroidNDKPath,
			// Token: 0x04000CAB RID: 3243
			VulkanSDKPath,
			// Token: 0x04000CAC RID: 3244
			DeleteServiceFilesOnExit,
			// Token: 0x04000CAD RID: 3245
			AutoConnect,
			// Token: 0x04000CAE RID: 3246
			HLM_EnableConnection,
			// Token: 0x04000CAF RID: 3247
			MaxCaptureDurationMs,
			// Token: 0x04000CB0 RID: 3248
			SessionLocation,
			// Token: 0x04000CB1 RID: 3249
			MaxSessionsSizeMB,
			// Token: 0x04000CB2 RID: 3250
			EnablePerfetto,
			// Token: 0x04000CB3 RID: 3251
			ClientID,
			// Token: 0x04000CB4 RID: 3252
			WinARMIPAddress,
			// Token: 0x04000CB5 RID: 3253
			InstallerTimeout,
			// Token: 0x04000CB6 RID: 3254
			LastLaunchedApp,
			// Token: 0x04000CB7 RID: 3255
			PerformanceLowColor,
			// Token: 0x04000CB8 RID: 3256
			PerformanceHighColor,
			// Token: 0x04000CB9 RID: 3257
			ShowDataExplorerFancyTooltip
		}

		// Token: 0x02000398 RID: 920
		// (Invoke) Token: 0x060011FE RID: 4606
		public delegate IEnumerable<string> UserPreferenceFileReader(string preferenceFilePath);
	}
}
