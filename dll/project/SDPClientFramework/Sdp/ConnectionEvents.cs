using System;

namespace Sdp
{
	// Token: 0x020000AC RID: 172
	public class ConnectionEvents
	{
		// Token: 0x06000342 RID: 834 RVA: 0x00009850 File Offset: 0x00007A50
		public ConnectionEvents()
		{
			this.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(this.OptionAdded, new EventHandler<OptionEventArgs>(this.SetVisible));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000987C File Offset: 0x00007A7C
		private void SetVisible(object o, OptionEventArgs e)
		{
			Option option = SdpApp.ConnectionManager.GetOption(e.OptionId, e.ProcessId);
			if (!option.IsOptionHidden())
			{
				if (!option.IsOptionContextState())
				{
					SdpApp.UIManager.PresentView("Options", null, false, true);
					return;
				}
				if (SdpApp.UIManager.SelectedLayout == "Snapshot")
				{
					SdpApp.UIManager.PresentView("Context", null, false, false);
				}
			}
		}

		// Token: 0x04000256 RID: 598
		public EventHandler<ProcessEventArgs> ProcessStateChanged;

		// Token: 0x04000257 RID: 599
		public EventHandler<ProcessMetricLinkedEventArgs> ProcessMetricLinked;

		// Token: 0x04000258 RID: 600
		public EventHandler<ProcessEventArgs> ProcessAdded;

		// Token: 0x04000259 RID: 601
		public EventHandler<ProcessEventArgs> ProcessRemoved;

		// Token: 0x0400025A RID: 602
		public EventHandler<EnableActionArgs> EnableAction;

		// Token: 0x0400025B RID: 603
		public EventHandler<TakeCaptureArgs> StartCaptureRequest;

		// Token: 0x0400025C RID: 604
		public EventHandler StartSamplingRequest;

		// Token: 0x0400025D RID: 605
		public EventHandler StopCaptureRequest;

		// Token: 0x0400025E RID: 606
		public EventHandler StopSamplingRequest;

		// Token: 0x0400025F RID: 607
		public EventHandler<MaxCaptureDurationExpiredEventArgs> MaxCaptureDurationExpired;

		// Token: 0x04000260 RID: 608
		public EventHandler SnapshotRequest;

		// Token: 0x04000261 RID: 609
		public EventHandler CancelSnapshotRequest;

		// Token: 0x04000262 RID: 610
		public EventHandler<OpenTraceFromSessionArgs> OpenTraceFromSession;

		// Token: 0x04000263 RID: 611
		public EventHandler<OpenSnapshotFromSessionArgs> OpenSnapshotFromSession;

		// Token: 0x04000264 RID: 612
		public EventHandler<OpenSnapshotFromSessionResultArgs> OpenSnapshotFromSessionResult;

		// Token: 0x04000265 RID: 613
		public EventHandler<MetricDictionaryChangedArgs> MetricDictionaryChanged;

		// Token: 0x04000266 RID: 614
		public EventHandler<EnableMetricEventArgs> EnableMetric;

		// Token: 0x04000267 RID: 615
		public EventHandler ProviderListChanged;

		// Token: 0x04000268 RID: 616
		public EventHandler<DataReceivedEventArgs> DataReceived;

		// Token: 0x04000269 RID: 617
		public EventHandler<CaptureCompletedEventArgs> CaptureCompleted;

		// Token: 0x0400026A RID: 618
		public EventHandler<DataProcessedEventArgs> DataProcessed;

		// Token: 0x0400026B RID: 619
		public EventHandler<BufferTransferEventArgs> ClientBufferTransfer;

		// Token: 0x0400026C RID: 620
		public EventHandler<ChangeMetricDataSourcesColorArgs> ChangeMetricDataSourcesColor;

		// Token: 0x0400026D RID: 621
		public EventHandler<MetricBeginDragArgs> MetricBeginDrag;

		// Token: 0x0400026E RID: 622
		public EventHandler MetricEndDrag;

		// Token: 0x0400026F RID: 623
		public EventHandler<MetricAddedEventArgs> MetricAdded;

		// Token: 0x04000270 RID: 624
		public EventHandler<MetricHiddenToggledEventArgs> MetricHiddenToggled;

		// Token: 0x04000271 RID: 625
		public EventHandler<MetricCategoryAddedEventArgs> MetricCategoryAdded;

		// Token: 0x04000272 RID: 626
		public EventHandler<OptionEventArgs> OptionAdded;

		// Token: 0x04000273 RID: 627
		public EventHandler<OptionCategoryAddedEventArgs> OptionCategoryAdded;

		// Token: 0x04000274 RID: 628
		public EventHandler<PauseCaptureEventArgs> PauseCapture;

		// Token: 0x04000275 RID: 629
		public EventHandler<ConfigureDeviceCompleteArgs> ConfigureDeviceComplete;

		// Token: 0x04000276 RID: 630
		public EventHandler<MetricsRecordedForCaptureArgs> MetricsRecordedForCapture;

		// Token: 0x04000277 RID: 631
		public EventHandler<BufferTransferProgressEventArgs> BufferTransferProgress;

		// Token: 0x04000278 RID: 632
		public EventHandler<AutoConnectEventArgs> AutoConnectSettingChanged;

		// Token: 0x04000279 RID: 633
		public EventHandler<AddMetricEventArgs> MetricsAddition;

		// Token: 0x0400027A RID: 634
		public EventHandler<RenderingAPISupportEventArgs> RenderingAPISupport;

		// Token: 0x0400027B RID: 635
		public EventHandler InitComplete;
	}
}
