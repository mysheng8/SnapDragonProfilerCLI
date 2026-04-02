using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Sdp.Helpers;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x02000054 RID: 84
	public class AnalyticsManager
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00006430 File Offset: 0x00004630
		public AnalyticsManager()
		{
			AnalyticsEvents analyticsEvents = SdpApp.EventsManager.AnalyticsEvents;
			analyticsEvents.TrackEvent = (EventHandler<AnalyticsEventArgs>)Delegate.Combine(analyticsEvents.TrackEvent, new EventHandler<AnalyticsEventArgs>(this.analyticsEvents_TrackEvent));
			string text = "False";
			string text2 = Environment.GetEnvironmentVariable("SDP_ANALYTICS_OPTOUT");
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = text2.ToLower();
				int num = 0;
				int.TryParse(text2, out num);
				if (string.Compare(text2, "true") == 0 || string.Compare(text2, "yes") == 0 || num > 0)
				{
					text = "True";
				}
			}
			if (!bool.TryParse(text, out AnalyticsManager.m_analyticsOptOut))
			{
				AnalyticsManager.m_analyticsOptOut = true;
			}
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "BuildDate", SdpApp.ModelManager.ApplicationModel.BuildDate);
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "Version", SdpApp.ModelManager.ApplicationModel.Version);
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "Language", CultureInfo.InstalledUICulture.Name);
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Location, "EnglishName", Globalization.userRegion.EnglishName);
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Location, "ISORegionCode", Globalization.userRegion.ToString());
			AnalyticsManager.m_os = "Windows";
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "OSString", Environment.OSVersion.ToString());
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "ClientOS", AnalyticsManager.m_os);
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectACK));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.InitComplete = (EventHandler)Delegate.Combine(connectionEvents.InitComplete, new EventHandler(this.OnConnectionInitComplete));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.MainWindowShown = (EventHandler<EventArgs>)Delegate.Combine(clientEvents.MainWindowShown, new EventHandler<EventArgs>(this.OnMainWindowShown));
			bool flag = File.Exists(AnalyticsManager.m_crashFilePath);
			if (flag)
			{
				try
				{
					this.m_previousCrashLines = File.ReadAllLines(AnalyticsManager.m_crashFilePath);
					if (this.m_previousCrashLines == null || this.m_previousCrashLines.Length == 0 || string.IsNullOrWhiteSpace(this.m_previousCrashLines[0]))
					{
						AnalyticsManager.Logger.LogWarning("crash.txt is empty");
						flag = false;
						this.m_previousCrashLines = null;
					}
					else
					{
						AnalyticsManager.m_crashedSessionPath = ((this.m_previousCrashLines.Length > 1) ? this.m_previousCrashLines[1] : null);
					}
				}
				catch (Exception ex)
				{
					AnalyticsManager.Logger.LogError("Failed to read crash.txt: " + ex.Message);
					flag = false;
					this.m_previousCrashLines = null;
				}
			}
			if (flag && this.m_previousCrashLines != null)
			{
				this.CopyCrashFileToSessionFolder();
			}
			try
			{
				if (!Directory.Exists(AnalyticsManager.m_localAppDataPath))
				{
					Directory.CreateDirectory(AnalyticsManager.m_localAppDataPath);
				}
				StreamWriter streamWriter = File.CreateText(AnalyticsManager.m_crashFilePath);
				if (streamWriter != null)
				{
					streamWriter.WriteLine(SdpApp.ModelManager.ApplicationModel.Version);
					streamWriter.Close();
				}
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Error creating '" + AnalyticsManager.m_crashFilePath + "': " + ex2.Message);
			}
			if (flag && this.m_previousCrashLines != null)
			{
				Task.Run(delegate
				{
					this.CheckForPreviousCrash(this.m_previousCrashLines);
				});
				return;
			}
			this.WriteEventViewerBreadcrumb("SnapdragonProfiler.exe startup");
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000675C File Offset: 0x0000495C
		public Dictionary<string, object> GetHitDataForAnalytics(string clientId, string event_category, Dictionary<string, object> parameters = null)
		{
			if (parameters == null)
			{
				parameters = new ParametersDict { { "engagement_time_msec", "0" } };
			}
			else if (!parameters.ContainsKey("engagement_time_msec"))
			{
				parameters.Add("engagement_time_msec", parameters.ContainsKey("ShutdownState") ? Globalization.timer.ElapsedMilliseconds.ToString() : "0");
			}
			return new Dictionary<string, object>
			{
				{
					"app_instance_id",
					AnalyticsManager.m_appInstanceID
				},
				{ "user_id", clientId },
				{
					"events",
					new ParametersDict
					{
						{ "name", event_category },
						{ "params", parameters }
					}
				}
			};
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000680C File Offset: 0x00004A0C
		private async void analyticsEvents_TrackEvent(object sender, AnalyticsEventArgs e)
		{
			if (!AnalyticsManager.m_analyticsOptOut && !AnalyticsManager.m_failureLogged)
			{
				string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSettingValueOrUseDefault<string>(UserPreferenceModel.UserPreference.ClientID);
				if (text.Equals("-1"))
				{
					text = Guid.NewGuid().ToString();
					SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.ClientID, text);
				}
				JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
				Dictionary<string, object> hitDataForAnalytics = this.GetHitDataForAnalytics(text, e.Category.ToString(), e.Parameters);
				string text2 = javaScriptSerializer.Serialize(hitDataForAnalytics);
				if (AnalyticsManager.m_httpClient == null)
				{
					AnalyticsManager.m_httpClient = new HttpClient();
					AnalyticsManager.m_httpClient.DefaultRequestHeaders.Add("Authentication-Token", AnalyticsManager.API_ID);
					AnalyticsManager.m_basePostURL = string.Concat(new string[]
					{
						"https://www.google-analytics.com/mp/collect?v=1&measurement_id=",
						AnalyticsManager.TRACKING_ID,
						"&api_secret=",
						AnalyticsManager.API_ID,
						"&cid=",
						text,
						"&t=event"
					});
				}
				await this.PostEvent(text2);
			}
		}

		// Token: 0x060001B8 RID: 440
		[DllImport("wininet.dll")]
		private static extern bool InternetGetConnectedState(out int connDescription, int reservedValue);

		// Token: 0x060001B9 RID: 441 RVA: 0x0000684C File Offset: 0x00004A4C
		private bool IsInternetAvailable()
		{
			int num;
			return AnalyticsManager.InternetGetConnectedState(out num, 0);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006864 File Offset: 0x00004A64
		private async Task PostEvent(string jsonHitData)
		{
			if (this.IsInternetAvailable())
			{
				try
				{
					await AnalyticsManager.m_httpClient.PostAsync(AnalyticsManager.m_basePostURL, new StringContent(jsonHitData, Encoding.UTF8, "application/json"));
					return;
				}
				catch (Exception ex)
				{
					AnalyticsManager.m_failureLogged = true;
					AnalyticsManager.Logger.LogError(ex.Message);
					return;
				}
			}
			AnalyticsManager.m_failureLogged = true;
			AnalyticsManager.Logger.LogError("Error logging event: No internet available");
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000068B0 File Offset: 0x00004AB0
		public void UpdateCrashFileWithSessionPath()
		{
			try
			{
				if (File.Exists(AnalyticsManager.m_crashFilePath))
				{
					string sessionPath = SdpApp.ConnectionManager.GetSessionPath();
					if (!string.IsNullOrEmpty(sessionPath))
					{
						using (StreamWriter streamWriter = new StreamWriter(AnalyticsManager.m_crashFilePath, true))
						{
							streamWriter.WriteLine(sessionPath);
						}
					}
				}
			}
			catch (Exception ex)
			{
				AnalyticsManager.Logger.LogError("Failed to append session path to crash.txt: " + ex.Message);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00006938 File Offset: 0x00004B38
		public void Shutdown()
		{
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "ShutdownState", "Normal " + AnalyticsManager.m_os + " " + SdpApp.ModelManager.ApplicationModel.Version);
			if (File.Exists(AnalyticsManager.m_crashFilePath))
			{
				File.Delete(AnalyticsManager.m_crashFilePath);
			}
			AnalyticsEvents analyticsEvents = SdpApp.EventsManager.AnalyticsEvents;
			analyticsEvents.TrackEvent = (EventHandler<AnalyticsEventArgs>)Delegate.Remove(analyticsEvents.TrackEvent, new EventHandler<AnalyticsEventArgs>(this.analyticsEvents_TrackEvent));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000069B8 File Offset: 0x00004BB8
		public void TrackMetric(uint metricId, uint processId)
		{
			string text = this.ConstructMetricLabel(metricId, processId);
			if (!string.IsNullOrEmpty(text))
			{
				this.TrackEvent(AnalyticsManager.AnalyticsCategory.Metrics, "MetricAdded", text);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000069E4 File Offset: 0x00004BE4
		public void TrackProcess(uint processId)
		{
			global::Process processByID = SdpApp.ConnectionManager.GetProcessByID(processId);
			if (processByID != null && !string.IsNullOrEmpty(processByID.GetProperties().name))
			{
				this.TrackEvent(AnalyticsManager.AnalyticsCategory.Metrics, "ProcessAdded", processByID.GetProperties().name);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006A29 File Offset: 0x00004C29
		public void TrackException(string message, bool isFatal)
		{
			message.Insert(0, (!isFatal) ? "Non fatal error: " : "Fatal error: ");
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "ShutdownState", message);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006A50 File Offset: 0x00004C50
		public void TrackWindow(CaptureType captureType)
		{
			string text = ((captureType == CaptureType.Trace) ? "Trace" : ((captureType == CaptureType.Snapshot) ? "Snapshot" : ((captureType == CaptureType.Sampling) ? "Sampling" : "")));
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.page_view, "page_title", text);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00006A91 File Offset: 0x00004C91
		public void TrackOptions(string optionName, object optionValue)
		{
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Options, optionName.Replace(" ", ""), optionValue.ToString());
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006AB0 File Offset: 0x00004CB0
		public void TrackExport(string exportType)
		{
			this.TrackEvent(AnalyticsManager.AnalyticsCategory.Export, "Type", exportType);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public void TrackInteraction(string sender, string description, string updatedWidget)
		{
			SdpApp.EventsManager.Raise<AnalyticsEventArgs>(SdpApp.EventsManager.AnalyticsEvents.TrackEvent, this, new AnalyticsEventArgs
			{
				Category = AnalyticsManager.AnalyticsCategory.Interaction,
				Parameters = new ParametersDict
				{
					{ "Description", description },
					{ "Sender", sender },
					{ "Widget", updatedWidget }
				}
			});
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006B24 File Offset: 0x00004D24
		public void TrackEvent(AnalyticsManager.AnalyticsCategory eventCategory, string parameterName, object parameterValue)
		{
			SdpApp.EventsManager.Raise<AnalyticsEventArgs>(SdpApp.EventsManager.AnalyticsEvents.TrackEvent, this, new AnalyticsEventArgs
			{
				Category = eventCategory,
				Parameters = new ParametersDict { { parameterName, parameterValue } }
			});
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006B6C File Offset: 0x00004D6C
		private string ConstructMetricLabel(uint metricId, uint processId)
		{
			Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(metricId);
			if (metricByID != null)
			{
				uint categoryID = metricByID.GetProperties().categoryID;
				MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(categoryID);
				if (metricCategory != null)
				{
					string name = metricCategory.GetProperties().name;
					string name2 = metricByID.GetProperties().name;
					string text = (metricByID.IsGlobal() ? "System" : "Process");
					string text2 = text + "/" + name + "/";
					if (!metricByID.IsGlobal())
					{
						this.TrackProcess(processId);
						global::Process processByID = SdpApp.ConnectionManager.GetProcessByID(processId);
						if (processByID != null && !string.IsNullOrEmpty(processByID.GetProperties().name))
						{
							text2 = text2 + processByID.GetProperties().name + "/";
						}
					}
					return text2 + name2;
				}
			}
			return null;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006C48 File Offset: 0x00004E48
		private void deviceEvents_ClientConnectACK(object sender, EventArgs e)
		{
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice != null)
			{
				DeviceAttributes deviceAttributes = connectedDevice.GetDeviceAttributes();
				if (deviceAttributes != null)
				{
					string productModel = deviceAttributes.GetProductModel();
					string osType = deviceAttributes.osType;
					if (!string.IsNullOrEmpty(productModel))
					{
						this.TrackEvent(AnalyticsManager.AnalyticsCategory.Device, "ProductModel", productModel);
					}
					if (!string.IsNullOrEmpty(osType))
					{
						this.TrackEvent(AnalyticsManager.AnalyticsCategory.Device, "DeviceOSType", osType);
					}
					if (SdpApp.ConnectionManager.GetDeviceOS() == ConnectionManager.DeviceOS.Windows)
					{
						string ip = connectedDevice.GetIP();
						bool flag = ip.Contains("127.0.0.1") || ip.Contains("localhost");
						this.TrackEvent(AnalyticsManager.AnalyticsCategory.Device, "Localhost", flag);
					}
				}
				this.TrackEvent(AnalyticsManager.AnalyticsCategory.Device, "DeviceRooted", connectedDevice.GetProperty(DeviceSettings.ProfilerDeviceIsRooted));
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006D03 File Offset: 0x00004F03
		private void OnConnectionInitComplete(object sender, EventArgs e)
		{
			this.UpdateCrashFileWithSessionPath();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006D0C File Offset: 0x00004F0C
		private void ShowCrashReportDialog(string crashVersion)
		{
			SdpApp.Platform.Invoke(delegate
			{
				UserPreferenceModel userPrefs = SdpApp.ModelManager.SettingsModel.UserPreferences;
				string text = userPrefs.RetrieveSetting(UserPreferenceModel.UserPreference.ShowCrashReportDialog);
				if (string.IsNullOrEmpty(text))
				{
					text = "True";
					userPrefs.RecordSetting(UserPreferenceModel.UserPreference.ShowCrashReportDialog, text);
				}
				if (!BoolConverter.Convert(text))
				{
					try
					{
						if (File.Exists(AnalyticsManager.m_crashFilePath))
						{
							File.Delete(AnalyticsManager.m_crashFilePath);
						}
					}
					catch (Exception ex)
					{
						AnalyticsManager.Logger.LogError("Failed to delete crash.txt: " + ex.Message);
					}
					return;
				}
				ShowMessageDialogCommand cmd = new ShowMessageDialogCommand();
				cmd.Message = "Snapdragon Profiler crashed during the previous session.\nWould you like to report the crash?";
				cmd.IconType = IconType.Warning;
				cmd.ButtonLayout = ButtonLayout.YesNo;
				cmd.HasDontShowAgainCheckBox = true;
				cmd.OnCompleted = delegate(bool res)
				{
					if (res)
					{
						this.LaunchFeedbackDialog(crashVersion);
					}
					userPrefs.RecordSetting(UserPreferenceModel.UserPreference.ShowCrashReportDialog, (!cmd.DontShowAgainCheckBoxValue).ToString());
				};
				cmd.Execute();
			});
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006D44 File Offset: 0x00004F44
		private void LaunchFeedbackDialog(string crashVersion)
		{
			SubmitFeedbackCommand submitFeedbackCommand = new SubmitFeedbackCommand(AnalyticsManager.m_crashedSessionPath, "Crash Report", "Please describe how you were using Snapdragon Profiler before the crash.");
			SdpApp.CommandManager.ExecuteCommand(submitFeedbackCommand);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00006D74 File Offset: 0x00004F74
		private bool CheckEventViewerForCrash()
		{
			bool flag;
			try
			{
				if (!EventLog.Exists("Application"))
				{
					AnalyticsManager.Logger.LogWarning("Cannot access Event Viewer - unable to check for crash");
					flag = false;
				}
				else
				{
					using (EventLog eventLog = new EventLog("Application"))
					{
						if (eventLog.Entries == null || eventLog.Entries.Count == 0)
						{
							return false;
						}
						DateTime dateTime = DateTime.Now.AddHours(-24.0);
						int num = 0;
						for (int i = eventLog.Entries.Count - 1; i >= 0; i--)
						{
							EventLogEntry eventLogEntry = eventLog.Entries[i];
							num++;
							if (num >= 10000 || eventLogEntry.TimeGenerated < dateTime)
							{
								break;
							}
							if (eventLogEntry.Message.Contains("SnapdragonProfiler.exe"))
							{
								if (eventLogEntry.EntryType == EventLogEntryType.Error && (eventLogEntry.Source == "Application Error" || eventLogEntry.Source == "Windows Error Reporting"))
								{
									AnalyticsManager.Logger.LogInformation("Crash was detected");
									return true;
								}
								if (eventLogEntry.EntryType == EventLogEntryType.Information && eventLogEntry.Source == "SnapdragonProfiler" && eventLogEntry.Message.Contains("SnapdragonProfiler.exe startup"))
								{
									AnalyticsManager.Logger.LogInformation("No crash detected (startup found first)");
									return false;
								}
							}
						}
					}
					flag = false;
				}
			}
			catch (Exception ex)
			{
				AnalyticsManager.Logger.LogWarning("Could not check Event Viewer: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006F3C File Offset: 0x0000513C
		private void CopyCrashFileToSessionFolder()
		{
			try
			{
				if (!string.IsNullOrEmpty(AnalyticsManager.m_crashedSessionPath) && File.Exists(AnalyticsManager.m_crashFilePath))
				{
					string text = Path.Combine(AnalyticsManager.m_crashedSessionPath, "crash.txt");
					File.Copy(AnalyticsManager.m_crashFilePath, text, true);
				}
			}
			catch (Exception ex)
			{
				AnalyticsManager.Logger.LogError("Failed to copy crash.txt to session folder: " + ex.Message);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00006FAC File Offset: 0x000051AC
		private void CheckForPreviousCrash(string[] crashLines)
		{
			try
			{
				bool flag = this.CheckEventViewerForCrash();
				if (flag)
				{
					string text = ((crashLines.Length != 0) ? crashLines[0] : SdpApp.ModelManager.ApplicationModel.Version);
					AnalyticsManager.m_crashedSessionPath = ((crashLines.Length > 1) ? crashLines[1] : null);
					this.TrackEvent(AnalyticsManager.AnalyticsCategory.Application, "ShutdownState", "Crash " + AnalyticsManager.m_os + " " + text);
					if (string.IsNullOrEmpty(AnalyticsManager.m_crashedSessionPath))
					{
						AnalyticsManager.Logger.LogWarning("No session path found in crash.txt");
					}
					this.m_crashVersion = text;
					this.m_shouldShowCrashDialog = true;
				}
			}
			catch (Exception ex)
			{
				AnalyticsManager.Logger.LogError("Error checking for previous crash: " + ex.Message);
			}
			finally
			{
				this.WriteEventViewerBreadcrumb("SnapdragonProfiler.exe startup");
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007080 File Offset: 0x00005280
		private void OnMainWindowShown(object sender, EventArgs e)
		{
			if (this.m_shouldShowCrashDialog && !string.IsNullOrEmpty(this.m_crashVersion))
			{
				this.ShowCrashReportDialog(this.m_crashVersion);
				this.m_shouldShowCrashDialog = false;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000070AC File Offset: 0x000052AC
		private void WriteEventViewerBreadcrumb(string message)
		{
			try
			{
				if (!EventLog.SourceExists("SnapdragonProfiler"))
				{
					try
					{
						EventLog.CreateEventSource("SnapdragonProfiler", "Application");
					}
					catch (Exception ex)
					{
						AnalyticsManager.Logger.LogWarning("Could not create Event Viewer source: " + ex.Message);
					}
				}
				EventLog.WriteEntry("SnapdragonProfiler", message, EventLogEntryType.Information);
				AnalyticsManager.Logger.LogInformation("Wrote Event Viewer breadcrumb: " + message);
			}
			catch (Exception ex2)
			{
				AnalyticsManager.Logger.LogWarning("Could not write Event Viewer breadcrumb: " + ex2.Message);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007150 File Offset: 0x00005350
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00007157 File Offset: 0x00005357
		public static ILogger Logger { private get; set; } = new Sdp.Logging.Logger("AnalyticsManager");

		// Token: 0x04000145 RID: 325
		public static bool m_analyticsOptOut = false;

		// Token: 0x04000146 RID: 326
		private static bool m_failureLogged = false;

		// Token: 0x04000147 RID: 327
		private static string m_localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SDP");

		// Token: 0x04000148 RID: 328
		private static string m_crashFilePath = Path.Combine(AnalyticsManager.m_localAppDataPath, "crash.txt");

		// Token: 0x04000149 RID: 329
		private static string TRACKING_ID = "G-BHBCW3HV29";

		// Token: 0x0400014A RID: 330
		private static string API_ID = "gswuN9XkQNSVzRdXiKBuRw";

		// Token: 0x0400014B RID: 331
		private static string m_basePostURL;

		// Token: 0x0400014C RID: 332
		private static string m_appInstanceID = Guid.NewGuid().ToString("N");

		// Token: 0x0400014D RID: 333
		private static string m_os;

		// Token: 0x0400014F RID: 335
		private static HttpClient m_httpClient;

		// Token: 0x04000150 RID: 336
		private static string m_crashedSessionPath = null;

		// Token: 0x04000151 RID: 337
		private string[] m_previousCrashLines;

		// Token: 0x04000152 RID: 338
		private string m_crashVersion;

		// Token: 0x04000153 RID: 339
		private bool m_shouldShowCrashDialog;

		// Token: 0x04000154 RID: 340
		private const string SDP_EVENT_SOURCE = "SnapdragonProfiler";

		// Token: 0x04000155 RID: 341
		private const string EVENT_LOG = "Application";

		// Token: 0x0200035C RID: 860
		public enum AnalyticsCategory
		{
			// Token: 0x04000BC3 RID: 3011
			Application,
			// Token: 0x04000BC4 RID: 3012
			Capture,
			// Token: 0x04000BC5 RID: 3013
			Device,
			// Token: 0x04000BC6 RID: 3014
			Interaction,
			// Token: 0x04000BC7 RID: 3015
			Export,
			// Token: 0x04000BC8 RID: 3016
			Metrics,
			// Token: 0x04000BC9 RID: 3017
			Location,
			// Token: 0x04000BCA RID: 3018
			page_view,
			// Token: 0x04000BCB RID: 3019
			Options
		}
	}
}
