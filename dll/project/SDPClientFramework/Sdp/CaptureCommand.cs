using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sdp
{
	// Token: 0x0200005E RID: 94
	internal class CaptureCommand : Command
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000076D7 File Offset: 0x000058D7
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000076DF File Offset: 0x000058DF
		public bool StartCapture { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000076E8 File Offset: 0x000058E8
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000076F0 File Offset: 0x000058F0
		public uint Duration { get; set; }

		// Token: 0x0600022B RID: 555 RVA: 0x000076FC File Offset: 0x000058FC
		protected override void OnExecute()
		{
			if (SdpApp.ModelManager.TraceModel.CurrentSources != null && SdpApp.ModelManager.TraceModel.CurrentSources.Count > 0)
			{
				if (!this.StartCapture)
				{
					GroupLayoutController currentCaptureGroupLayoutController = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController;
					currentCaptureGroupLayoutController.StopCapture();
					SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.StopCaptureRequest, this, EventArgs.Empty);
					SdpApp.AnalyticsManager.TrackEvent(AnalyticsManager.AnalyticsCategory.Capture, "TraceDuration", CaptureCommand.timer.ElapsedMilliseconds.ToString());
					SdpApp.ModelManager.SettingsModel.UserPreferences.RecordSetting(UserPreferenceModel.UserPreference.MaxCaptureDurationMs, SdpApp.ModelManager.DataSourcesModel.CaptureDuration.ToString());
					return;
				}
				CaptureCommand.timer.Restart();
				GroupLayoutController currentCaptureGroupLayoutController2 = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController;
				if (currentCaptureGroupLayoutController2 != null)
				{
					currentCaptureGroupLayoutController2.StartCapture();
					SdpApp.ModelManager.DataSourcesModel.CaptureDuration = this.Duration;
					SdpApp.CommandManager.ExecuteCommand(new NewCaptureCommand());
					SdpApp.EventsManager.Raise<TakeCaptureArgs>(SdpApp.EventsManager.ConnectionEvents.StartCaptureRequest, this, new TakeCaptureArgs
					{
						Duration = this.Duration
					});
					return;
				}
			}
			else
			{
				Dictionary<IdNamePair, List<IdNamePair>> currentSources = SdpApp.ModelManager.TraceModel.CurrentSources;
				if (currentSources != null && currentSources.Count == 0 && !this.StartCapture)
				{
					GroupLayoutController currentCaptureGroupLayoutController3 = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController;
					currentCaptureGroupLayoutController3.AppKilledWhileCapturing = true;
					SdpApp.AnalyticsManager.TrackEvent(AnalyticsManager.AnalyticsCategory.Capture, "TraceDuration", CaptureCommand.timer.ElapsedMilliseconds.ToString());
				}
			}
		}

		// Token: 0x0400017D RID: 381
		public static readonly Stopwatch timer = new Stopwatch();
	}
}
