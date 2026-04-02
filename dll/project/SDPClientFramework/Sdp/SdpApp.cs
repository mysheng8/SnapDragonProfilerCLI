using System;
using System.IO;
using Sdp.Views.SessionSettingsDialog;
using SdpClientFramework.DesignPatterns.SingleConsumer;

namespace Sdp
{
	// Token: 0x02000287 RID: 647
	public static class SdpApp
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00020460 File Offset: 0x0001E660
		public static CommandManager CommandManager
		{
			get
			{
				return SdpApp.m_command_manager;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00020467 File Offset: 0x0001E667
		public static IPlatform Platform
		{
			get
			{
				return SdpApp.m_platform;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002046E File Offset: 0x0001E66E
		public static UIManager UIManager
		{
			get
			{
				return SdpApp.m_ui_manager;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00020475 File Offset: 0x0001E675
		public static EventsManager EventsManager
		{
			get
			{
				return SdpApp.m_events_manager;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0002047C File Offset: 0x0001E67C
		public static ModelManager ModelManager
		{
			get
			{
				return SdpApp.m_model_manager;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00020483 File Offset: 0x0001E683
		public static ConnectionManager ConnectionManager
		{
			get
			{
				return SdpApp.m_connection_manager;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0002048A File Offset: 0x0001E68A
		public static InstrumentedCodeProcessor ICProcessor
		{
			get
			{
				return SdpApp.m_instrumentedCodeProcessor;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00020491 File Offset: 0x0001E691
		public static PluginManager PluginManager
		{
			get
			{
				return SdpApp.m_pluginManager;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00020498 File Offset: 0x0001E698
		public static AnalyticsManager AnalyticsManager
		{
			get
			{
				return SdpApp.m_analyticsManager;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x0002049F File Offset: 0x0001E69F
		public static StatisticsManager StatisticsManager
		{
			get
			{
				return SdpApp.m_statisticsManager;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x000204A6 File Offset: 0x0001E6A6
		public static IActionQueue ClientActionQueue
		{
			get
			{
				return SdpApp.m_actionQueue;
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000204B0 File Offset: 0x0001E6B0
		public static bool Init(IPlatform platform)
		{
			SdpApp.m_command_manager = new CommandManager();
			SdpApp.m_platform = platform;
			SdpApp.m_events_manager = new EventsManager();
			SdpApp.m_model_manager = new ModelManager();
			SdpApp.m_connection_manager = new ConnectionManager();
			SdpApp.m_instrumentedCodeProcessor = new InstrumentedCodeProcessor();
			SdpApp.m_statisticsManager = new StatisticsManager();
			SdpApp.m_ui_manager = new UIManager();
			SdpApp.m_analyticsManager = new AnalyticsManager();
			SdpApp.m_actionQueue = new ActionQueue(false);
			return true;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002051F File Offset: 0x0001E71F
		public static void InitPlugins()
		{
			SdpApp.m_pluginManager = new PluginManager();
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.PluginEvents.RegisterDefaultToolPlugins, SdpApp.m_pluginManager, EventArgs.Empty);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00020550 File Offset: 0x0001E750
		public static bool InitCore(SessionSettingsSelection sessionSettings)
		{
			bool flag = SdpApp.m_connection_manager.InitCore(sessionSettings);
			if (flag)
			{
				SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.InitComplete, SdpApp.m_connection_manager, EventArgs.Empty);
			}
			return flag;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00020590 File Offset: 0x0001E790
		public static bool Shutdown()
		{
			SdpApp.m_analyticsManager.Shutdown();
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ClientEvents.AppShutdown, SdpApp.m_events_manager, EventArgs.Empty);
			SdpApp.m_connection_manager.Shutdown();
			SdpApp.m_ui_manager = null;
			SdpApp.m_connection_manager = null;
			SdpApp.m_model_manager = null;
			SdpApp.m_events_manager = null;
			SdpApp.m_platform = null;
			SdpApp.m_command_manager = null;
			SdpApp.m_instrumentedCodeProcessor = null;
			SdpApp.m_statisticsManager = null;
			SdpApp.m_pluginManager.Shutdown();
			return true;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002060F File Offset: 0x0001E80F
		public static void ExecuteCommand(ICommand command)
		{
			if (SdpApp.m_command_manager != null && command != null)
			{
				SdpApp.m_command_manager.ExecuteCommand(command);
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00020628 File Offset: 0x0001E828
		public static string GetAppDataPath()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string text = Path.Combine(folderPath, "SDP");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00020659 File Offset: 0x0001E859
		public static bool IsCurrentVersionNewerThan(int curVersionMajor, int curVersionMinor, int compareVersionMajor, int compareVersionMinor)
		{
			return curVersionMajor > compareVersionMajor || (curVersionMajor >= compareVersionMajor && curVersionMinor >= compareVersionMinor);
		}

		// Token: 0x040008AF RID: 2223
		private static CommandManager m_command_manager;

		// Token: 0x040008B0 RID: 2224
		private static IPlatform m_platform;

		// Token: 0x040008B1 RID: 2225
		private static EventsManager m_events_manager;

		// Token: 0x040008B2 RID: 2226
		private static ModelManager m_model_manager;

		// Token: 0x040008B3 RID: 2227
		private static ConnectionManager m_connection_manager;

		// Token: 0x040008B4 RID: 2228
		private static UIManager m_ui_manager;

		// Token: 0x040008B5 RID: 2229
		private static InstrumentedCodeProcessor m_instrumentedCodeProcessor;

		// Token: 0x040008B6 RID: 2230
		private static PluginManager m_pluginManager;

		// Token: 0x040008B7 RID: 2231
		private static AnalyticsManager m_analyticsManager;

		// Token: 0x040008B8 RID: 2232
		private static StatisticsManager m_statisticsManager;

		// Token: 0x040008B9 RID: 2233
		private static IActionQueue m_actionQueue;
	}
}
