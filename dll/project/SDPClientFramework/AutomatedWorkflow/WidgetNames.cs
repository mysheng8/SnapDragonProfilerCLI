using System;
using Sdp;

namespace SDPClientFramework.AutomatedWorkflow
{
	// Token: 0x0200004C RID: 76
	public static class WidgetNames
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000630F File Offset: 0x0000450F
		public static string NewSnapshotWelcomeScreenButton
		{
			get
			{
				return WidgetNames.WelcomeScreenActionButtonName(ActionEnum.Snapshot);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006317 File Offset: 0x00004517
		public static string NewCaptureWelcomeScreenButton
		{
			get
			{
				return WidgetNames.WelcomeScreenActionButtonName(ActionEnum.NewCapture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000631F File Offset: 0x0000451F
		public static string RealTimeWelcomeScreenButton
		{
			get
			{
				return WidgetNames.WelcomeScreenActionButtonName(ActionEnum.Realtime);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006327 File Offset: 0x00004527
		public static string LAUNCH_APPLICATION_BUTTON(CaptureType captureType, int sessionNumber)
		{
			return string.Format("{0} {1} Launch Application Button", captureType, sessionNumber);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000633F File Offset: 0x0000453F
		public static string PROCESS_LIST(CaptureType captureType, int sessionNumber)
		{
			return string.Format("{0} {1} Process List", captureType, sessionNumber);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006357 File Offset: 0x00004557
		public static string METRICS_LIST(CaptureType captureType, int sessionNumber)
		{
			return string.Format("{0} {1} Metrics List", captureType, sessionNumber);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000636F File Offset: 0x0000456F
		public static string EXPORT_METRICS_TO_CSV_BUTTON(CaptureType captureType, int sessionNumber)
		{
			return string.Format("{0} {1} Export Metrics To CSV Button", captureType, sessionNumber);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006387 File Offset: 0x00004587
		public static string WelcomeScreenActionButtonName(ActionEnum action)
		{
			return string.Format("{0} ", action) + "Button";
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000063A3 File Offset: 0x000045A3
		public static string SNAPSHOT_WINDOW(int number)
		{
			return "Snapshot " + number.ToString();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000063B6 File Offset: 0x000045B6
		public static string TRACE_WINDOW(int number)
		{
			return "Trace " + number.ToString();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000063C9 File Offset: 0x000045C9
		public static string REAL_TIME_WINDOW(int number)
		{
			return "Realtime " + number.ToString();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000063DC File Offset: 0x000045DC
		public static string TRACE_CAPTURE_BUTTON(int sessionNumber)
		{
			return string.Format("Trace {0} Start Capture Button", sessionNumber);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000063EE File Offset: 0x000045EE
		public static string SNAPSHOT_CAPTURE_BUTTON(int sessionNumber)
		{
			return string.Format("Snapshot {0} Capture Button", sessionNumber);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006400 File Offset: 0x00004600
		public static string LAUNCH_APPLICATION_DIALOG_API_CHECKBOX(string api)
		{
			return "Launch Application Dialog API Checkbox " + api;
		}

		// Token: 0x0400013A RID: 314
		public const string MAIN_WINDOW = "Main Window";

		// Token: 0x0400013B RID: 315
		public const string STATUS_BAR_CONNECTION_BUTTON = "Staus Bar Connection Button";

		// Token: 0x0400013C RID: 316
		public const string AUTO_CONNECT_CHECK_BUTTON = "Auto Connect Check Button";

		// Token: 0x0400013D RID: 317
		public const string LAUNCH_APPLICATION_DIALOG_LAUNCHABLE_PACKAGE_LIST_TREE_VIEW = "Launchable Application Package List";

		// Token: 0x0400013E RID: 318
		public const string LAUNCH_APPLICATION_DIALOG_LAUNCHABLE_ACTIVITY_LIST_TREE_VIEW = "Launchable Application Activity List";

		// Token: 0x0400013F RID: 319
		public const string LAUNCH_APPLICATION_DIALOG_LAUNCH_BUTTON = "Launch Application Dialog Launch Button";

		// Token: 0x04000140 RID: 320
		public const string SAVE_FILE_DIALOG = "Save File Dialog";

		// Token: 0x04000141 RID: 321
		public const string SAVE_FILE_DIALOG_SAVE_BUTTON = "Save File Dialog Save Button";

		// Token: 0x04000142 RID: 322
		public const string EXIT_MENU_ITEM = "E_xit";

		// Token: 0x04000143 RID: 323
		public const string DATA_EXPLORER_VIEW_EXPORT_TO_CSV_BUTTON = "Data Explorer View Export To CSV Button";
	}
}
