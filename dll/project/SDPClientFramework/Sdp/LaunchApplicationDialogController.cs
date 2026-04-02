using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020001D9 RID: 473
	internal class LaunchApplicationDialogController : IDialogController
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000657 RID: 1623 RVA: 0x0000F288 File Offset: 0x0000D488
		// (remove) Token: 0x06000658 RID: 1624 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
		public event EventHandler LaunchAppClicked;

		// Token: 0x06000659 RID: 1625 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public async Task<bool> ShowDialog()
		{
			IDialog dialog = this.m_view as IDialog;
			return await ((dialog != null) ? dialog.ShowDialog() : null);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0000F33C File Offset: 0x0000D53C
		public LaunchApplicationDialogController(ILaunchApplicationDialog view, CaptureType captureType)
		{
			this.m_view = view;
			this.m_captureType = captureType;
			this.m_view.SelectedPackageChanged += this.m_view_SelectedPackageChanged;
			this.m_view.PackageFilterChanged += this.m_view_PackageFilterChanged;
			this.m_view.SelectedActivityChanged += this.m_view_SelectedActivityChanged;
			this.m_view.ParamChanged += this.m_view_ParamChanged;
			this.m_view.LaunchAppClicked += this.view_LaunchApplicationClicked;
			this.m_view.RefreshAppsClicked += this.view_RefreshAppsClicked;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.RenderingAPISupport = (EventHandler<RenderingAPISupportEventArgs>)Delegate.Combine(connectionEvents.RenderingAPISupport, new EventHandler<RenderingAPISupportEventArgs>(this.connectionEvents_RenderingAPISupport));
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice != null)
			{
				this.m_deviceName = connectedDevice.GetDeviceAttributes().deviceName;
			}
			this.m_view.UpdateView(captureType, SdpApp.ModelManager.SettingsModel.UserPreferences.LaunchedAppHistory);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0000F4A4 File Offset: 0x0000D6A4
		private void InvalidatePackageList(LaunchApplicationFilters packageFilter, string selectedPackage = "")
		{
			string[] packageList = this.GetPackageList(packageFilter);
			if (this.IsAndroid)
			{
				if (packageList.Length == 0)
				{
					this.m_view.SetStatus(StatusType.Error, "No packages detected", 0, false, null);
					return;
				}
				if (string.IsNullOrEmpty(selectedPackage))
				{
					this.m_view.SetStatus(StatusType.Neutral, "Please select a package to launch", 0, false, null);
				}
				this.m_view.InvalidatePackageList(packageList, selectedPackage);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0000F503 File Offset: 0x0000D703
		public bool IsAndroid
		{
			get
			{
				return this.m_view.IsAndroid;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0000F510 File Offset: 0x0000D710
		public bool SelectedPackageDebuggable
		{
			get
			{
				return this.m_selectedPackageDebuggable;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0000F518 File Offset: 0x0000D718
		public string SelectedPackage
		{
			get
			{
				return this.m_selectedPackage;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0000F520 File Offset: 0x0000D720
		public string SelectedActivity
		{
			get
			{
				return this.m_selectedActivity;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0000F528 File Offset: 0x0000D728
		public string SelectedPackageActivity
		{
			get
			{
				return this.m_selectedPackage + "/" + this.m_selectedActivity;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0000F540 File Offset: 0x0000D740
		public bool TryGetParam<T>(LaunchApplicationDialogParam param, T defaultValue, out T outValue)
		{
			outValue = defaultValue;
			bool flag;
			try
			{
				switch (param)
				{
				case LaunchApplicationDialogParam.EXECUTABLE_PATH:
					if (outValue.GetType() == typeof(string))
					{
						outValue = (T)((object)this.m_launchApplicationExecutablePath);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.WORKING_DIRECTORY:
					if (outValue.GetType() == typeof(string))
					{
						outValue = (T)((object)this.m_launchApplicationWorkingDirectory);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.COMMAND_LINE_ARGUMENTS:
					if (outValue.GetType() == typeof(string))
					{
						outValue = (T)((object)this.m_launchApplicationCommandlineArgs);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.INTENT_ARGUMENTS:
					if (outValue.GetType() == typeof(string))
					{
						outValue = (T)((object)this.m_launchApplicationIntentArgs);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.RENDERING_APIS:
					if (outValue.GetType() == typeof(RenderingAPI))
					{
						outValue = (T)((object)this.m_launchApplicationRenderingAPIs);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.ENVIRONMENT_VARIABLES:
					if (outValue.GetType() == typeof(Dictionary<string, string>))
					{
						outValue = (T)((object)this.m_launchApplicationEnvironmentVariables);
						return true;
					}
					break;
				case LaunchApplicationDialogParam.OPTIONS:
					if (outValue.GetType() == typeof(Dictionary<string, bool>))
					{
						outValue = (T)((object)this.m_launchApplicationOptions);
						return true;
					}
					break;
				}
				flag = false;
			}
			catch
			{
				outValue = defaultValue;
				flag = false;
			}
			return flag;
		}

		// Token: 0x1700012F RID: 303
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0000F728 File Offset: 0x0000D928
		public bool EnableEvents
		{
			set
			{
				if (value)
				{
					ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
					connectionEvents.RenderingAPISupport = (EventHandler<RenderingAPISupportEventArgs>)Delegate.Combine(connectionEvents.RenderingAPISupport, new EventHandler<RenderingAPISupportEventArgs>(this.connectionEvents_RenderingAPISupport));
					return;
				}
				ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
				connectionEvents2.RenderingAPISupport = (EventHandler<RenderingAPISupportEventArgs>)Delegate.Remove(connectionEvents2.RenderingAPISupport, new EventHandler<RenderingAPISupportEventArgs>(this.connectionEvents_RenderingAPISupport));
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0000F78F File Offset: 0x0000D98F
		private void m_view_PackageFilterChanged(object sender, LaunchAppFilterChanged e)
		{
			this.InvalidatePackageList(e.Filter, e.SelectedPackage);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		private void m_view_SelectedPackageChanged(object sender, LaunchAppPackageChanged e)
		{
			if (this.IsAndroid)
			{
				this.m_selectedPackage = e.package;
				this.m_selectedActivity = e.activity;
				if (e.shouldInvalidatePackageList)
				{
					this.m_view.InvalidatePackageList(this.GetPackageList(e.filter), this.m_selectedPackage);
				}
				if (!string.IsNullOrEmpty(this.m_selectedPackage) && e.shouldInvalidateActivityList)
				{
					string text = null;
					bool flag = true;
					bool flag2 = SdpApp.ConnectionManager.TryGetDefaultActivity(this.m_deviceName, this.m_selectedPackage, out text, out flag);
					this.m_view.LaunchButtonSensitive = flag2;
					if (flag2)
					{
						this.m_selectedPackageDebuggable = flag;
						Match match = Regex.Match(text, "[^/]+$");
						if (match.Success)
						{
							text = match.Groups[0].Value;
						}
						this.m_view.SetStatus(StatusType.Success, "Found default activity " + text, 5000, false, null);
						if (string.IsNullOrEmpty(this.m_selectedActivity))
						{
							this.m_selectedActivity = text;
						}
					}
					else
					{
						this.m_view.SetStatus(StatusType.Error, "Selected package does not contain a default activity", 0, false, null);
					}
					this.m_view.InvalidateActivityList(this.GetActivityList(this.m_selectedPackage), text, this.m_selectedActivity);
					return;
				}
				this.m_view.LaunchButtonSensitive = false;
				this.m_view.SetStatus(StatusType.Warning, "No package selected", 0, false, null);
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0000F8F6 File Offset: 0x0000DAF6
		private void m_view_SelectedActivityChanged(object sender, LaunchAppActivityChanged e)
		{
			if (this.IsAndroid)
			{
				this.m_selectedActivity = e.activity;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0000F90C File Offset: 0x0000DB0C
		private string[] GetPackageList(LaunchApplicationFilters packageFilter)
		{
			List<string> list = new List<string>();
			if (this.IsAndroid)
			{
				string text = "-3";
				if (packageFilter != LaunchApplicationFilters.ALL)
				{
					if (packageFilter == LaunchApplicationFilters.THIRD_PARTY)
					{
						text = "-3";
					}
					else
					{
						this.m_errorLogger.LogError("Unknown Filter " + packageFilter.ToString() + " for packages.");
					}
				}
				else
				{
					text = "-a";
				}
				global::System.Diagnostics.Process process = new global::System.Diagnostics.Process();
				process.StartInfo.FileName = "adb";
				process.StartInfo.Arguments = string.Concat(new string[] { "-s ", this.m_deviceName, " shell pm list packages ", text, " | sort" });
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();
				string text2 = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				Regex regex = new Regex("package:(.*?)[\\r\\n]+");
				MatchCollection matchCollection = regex.Matches(text2);
				if (matchCollection != null && matchCollection.Count > 0)
				{
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						if (match != null && match.Groups.Count == 2)
						{
							string value = match.Groups[1].Value;
							if (!value.Contains("com.qualcomm.snapdragonprofiler.profilerlayer"))
							{
								list.Add(value);
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		private string[] GetActivityList(string package)
		{
			List<string> list = new List<string>();
			if (this.IsAndroid)
			{
				global::System.Diagnostics.Process process = new global::System.Diagnostics.Process();
				process.StartInfo.FileName = "adb";
				process.StartInfo.Arguments = string.Concat(new string[] { "-s ", this.m_deviceName, " shell \"dumpsys package | grep -i ", package, "/ | grep -o '[^ ]*/[^ :}]*' | sort | uniq\"" });
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();
				string text = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				Regex regex = new Regex(".*?/(.*?)[\\r\\n]+");
				MatchCollection matchCollection = regex.Matches(text);
				if (matchCollection != null && matchCollection.Count > 0)
				{
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						if (match != null && match.Groups.Count == 2)
						{
							list.Add(match.Groups[1].Value);
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
		private void m_view_ParamChanged(object sender, LaunchAppParamChanged e)
		{
			LaunchApplicationDialogParam paramId = e.paramId;
			if (paramId != LaunchApplicationDialogParam.PERF_HINTS_ENABLE)
			{
				if (paramId != LaunchApplicationDialogParam.RENDERING_APIS)
				{
					if (paramId == LaunchApplicationDialogParam.OPTIONS)
					{
						this.m_view.TryGetParam<Dictionary<string, bool>>(LaunchApplicationDialogParam.OPTIONS, new Dictionary<string, bool>(), out this.m_launchApplicationOptions);
					}
				}
				else
				{
					this.m_view.TryGetParam<RenderingAPI>(LaunchApplicationDialogParam.RENDERING_APIS, RenderingAPI.None, out this.m_launchApplicationRenderingAPIs);
				}
			}
			else
			{
				bool flag;
				this.m_view.TryGetParam<bool>(LaunchApplicationDialogParam.PERF_HINTS_ENABLE, false, out flag);
				SdpApp.ModelManager.DataSourcesModel.PerfHintsEnabled = flag;
			}
			if (!this.IsAndroid)
			{
				switch (e.paramId)
				{
				case LaunchApplicationDialogParam.EXECUTABLE_PATH:
					this.m_view.TryGetParam<string>(LaunchApplicationDialogParam.EXECUTABLE_PATH, "", out this.m_launchApplicationExecutablePath);
					return;
				case LaunchApplicationDialogParam.WORKING_DIRECTORY:
					this.m_view.TryGetParam<string>(LaunchApplicationDialogParam.WORKING_DIRECTORY, "", out this.m_launchApplicationWorkingDirectory);
					return;
				case LaunchApplicationDialogParam.COMMAND_LINE_ARGUMENTS:
					this.m_view.TryGetParam<string>(LaunchApplicationDialogParam.COMMAND_LINE_ARGUMENTS, "", out this.m_launchApplicationCommandlineArgs);
					return;
				case LaunchApplicationDialogParam.INTENT_ARGUMENTS:
				case LaunchApplicationDialogParam.RENDERING_APIS:
					break;
				case LaunchApplicationDialogParam.ENVIRONMENT_VARIABLES:
					this.m_view.TryGetParam<Dictionary<string, string>>(LaunchApplicationDialogParam.ENVIRONMENT_VARIABLES, new Dictionary<string, string>(), out this.m_launchApplicationEnvironmentVariables);
					return;
				default:
					return;
				}
			}
			else
			{
				LaunchApplicationDialogParam paramId2 = e.paramId;
				if (paramId2 == LaunchApplicationDialogParam.INTENT_ARGUMENTS)
				{
					this.m_view.TryGetParam<string>(LaunchApplicationDialogParam.INTENT_ARGUMENTS, "", out this.m_launchApplicationIntentArgs);
				}
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0000FD23 File Offset: 0x0000DF23
		private void view_LaunchApplicationClicked(object sender, EventArgs e)
		{
			EventHandler launchAppClicked = this.LaunchAppClicked;
			if (launchAppClicked == null)
			{
				return;
			}
			launchAppClicked(this, EventArgs.Empty);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0000F78F File Offset: 0x0000D98F
		private void view_RefreshAppsClicked(object sender, LaunchAppFilterChanged e)
		{
			this.InvalidatePackageList(e.Filter, e.SelectedPackage);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0000FD3B File Offset: 0x0000DF3B
		private void connectionEvents_RenderingAPISupport(object sender, RenderingAPISupportEventArgs e)
		{
			this.m_view.UpdateRenderingAPIGrid();
		}

		// Token: 0x040006D2 RID: 1746
		private ILaunchApplicationDialog m_view;

		// Token: 0x040006D3 RID: 1747
		public ILogger m_errorLogger = new Sdp.Logging.Logger("Launch Application Controller");

		// Token: 0x040006D4 RID: 1748
		private string m_selectedPackage;

		// Token: 0x040006D5 RID: 1749
		private string m_selectedActivity;

		// Token: 0x040006D6 RID: 1750
		private bool m_selectedPackageDebuggable;

		// Token: 0x040006D7 RID: 1751
		private string m_deviceName;

		// Token: 0x040006D8 RID: 1752
		private CaptureType m_captureType;

		// Token: 0x040006D9 RID: 1753
		private string m_launchApplicationExecutablePath = "";

		// Token: 0x040006DA RID: 1754
		private string m_launchApplicationWorkingDirectory = "";

		// Token: 0x040006DB RID: 1755
		private string m_launchApplicationCommandlineArgs = "";

		// Token: 0x040006DC RID: 1756
		private string m_launchApplicationIntentArgs = "";

		// Token: 0x040006DD RID: 1757
		private Dictionary<string, bool> m_launchApplicationOptions = new Dictionary<string, bool>();

		// Token: 0x040006DE RID: 1758
		private RenderingAPI m_launchApplicationRenderingAPIs;

		// Token: 0x040006DF RID: 1759
		private Dictionary<string, string> m_launchApplicationEnvironmentVariables = new Dictionary<string, string>();
	}
}
