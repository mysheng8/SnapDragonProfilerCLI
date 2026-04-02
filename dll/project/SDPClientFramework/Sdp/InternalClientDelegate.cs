using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020002AD RID: 685
	public class InternalClientDelegate : ClientDelegate
	{
		// Token: 0x06000DBD RID: 3517 RVA: 0x0002A331 File Offset: 0x00028531
		public InternalClientDelegate(ConnectionManager container, Client client)
		{
			this.m_client = client;
			this.m_container = container;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002A364 File Offset: 0x00028564
		public override void OnClientConnected()
		{
			this.SetRendererString();
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.DeviceEvents.ClientConnectACK, this, EventArgs.Empty);
			uint num = CaptureManager.Get().CreateCapture(1U);
			Capture capture = CaptureManager.Get().GetCapture(num);
			if (capture != null && capture.IsValid())
			{
				capture.Start(new CaptureSettings(1U, 4294967294U, 0U, 0U, ""));
				NewRealtimeCommand newRealtimeCommand = new NewRealtimeCommand();
				newRealtimeCommand.CaptureID = num;
				SdpApp.CommandManager.ExecuteCommand(newRealtimeCommand);
				return;
			}
			this.m_Logger.LogError("Failed to create a new realtime capture.");
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002A3F8 File Offset: 0x000285F8
		private void SetRendererString()
		{
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			if (connectedDevice != null)
			{
				string property = connectedDevice.GetProperty(DeviceSettings.ProfilerDeviceRendererString);
				if (!property.Equals("Unknown"))
				{
					this.m_container.ConnectedDeviceRendererString = property;
				}
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002A438 File Offset: 0x00028638
		public override void OnClientDisconnected()
		{
			SdpApp.ModelManager.ConnectionModel.Providers.Clear();
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.DeviceEvents.ClientDisconnectACK, this, EventArgs.Empty);
			this.m_container.RegisteredMetricsList.Clear();
			Capture capture = CaptureManager.Get().GetCapture(1U);
			if (capture.IsValid())
			{
				capture.Stop();
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002A4A4 File Offset: 0x000286A4
		public override void OnCaptureComplete(uint providerID, uint captureID)
		{
			CaptureCompletedEventArgs captureCompletedEventArgs = new CaptureCompletedEventArgs();
			captureCompletedEventArgs.ProviderId = providerID;
			captureCompletedEventArgs.CaptureId = captureID;
			SdpApp.EventsManager.Raise<CaptureCompletedEventArgs>(SdpApp.EventsManager.ConnectionEvents.CaptureCompleted, this, captureCompletedEventArgs);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002A4E0 File Offset: 0x000286E0
		public override void OnProcessStateChanged(uint pid)
		{
			SdpApp.EventsManager.Raise<ProcessEventArgs>(SdpApp.EventsManager.ConnectionEvents.ProcessStateChanged, this, new ProcessEventArgs
			{
				PID = pid
			});
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002A508 File Offset: 0x00028708
		public override void OnProcessMetricLinked(uint pid, uint mid)
		{
			SdpApp.EventsManager.Raise<ProcessMetricLinkedEventArgs>(SdpApp.EventsManager.ConnectionEvents.ProcessMetricLinked, this, new ProcessMetricLinkedEventArgs
			{
				PID = pid,
				MetricID = mid
			});
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002A537 File Offset: 0x00028737
		public override void OnProcessAdded(uint pid)
		{
			SdpApp.EventsManager.Raise<ProcessEventArgs>(SdpApp.EventsManager.ConnectionEvents.ProcessAdded, this, new ProcessEventArgs
			{
				PID = pid
			});
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002A55F File Offset: 0x0002875F
		public override void OnProcessRemoved(uint pid)
		{
			SdpApp.EventsManager.Raise<ProcessEventArgs>(SdpApp.EventsManager.ConnectionEvents.ProcessRemoved, this, new ProcessEventArgs
			{
				PID = pid
			});
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002A588 File Offset: 0x00028788
		public override void OnProviderConnected(uint providerID)
		{
			DataProvider provider = this.m_client.GetProvider(providerID);
			SdpApp.ModelManager.ConnectionModel.TryAddProvider(provider);
			this.m_client.RequestMetricCategories(providerID);
			this.m_client.RequestMetrics(providerID);
			this.m_client.RequestOptionCategories(providerID);
			this.m_client.RequestOptions(providerID);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002A5E8 File Offset: 0x000287E8
		public override void OnProviderDisconnected(Client client, DataProvider dataProvider)
		{
			SdpApp.ModelManager.ConnectionModel.RemoveProvider(dataProvider.GetID());
			MetricList allMetrics = MetricManager.Get().GetAllMetrics();
			MetricList metricList = new MetricList();
			foreach (Metric metric in allMetrics)
			{
				if (this.m_container.RegisteredMetricsList.Contains(metric.GetProperties().id))
				{
					this.m_container.RegisteredMetricsList.Remove(metric.GetProperties().id);
					metricList.Add(metric);
				}
			}
			MetricDictionaryChangedArgs metricDictionaryChangedArgs = new MetricDictionaryChangedArgs();
			metricDictionaryChangedArgs.Removed = metricList;
			SdpApp.EventsManager.Raise<MetricDictionaryChangedArgs>(SdpApp.EventsManager.ConnectionEvents.MetricDictionaryChanged, this, metricDictionaryChangedArgs);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002A6BC File Offset: 0x000288BC
		public override void OnDeviceListUpdated()
		{
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.DeviceEvents.DeviceListChanged, this, EventArgs.Empty);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002A6E0 File Offset: 0x000288E0
		public override void OnOptionAdded(uint providerID, uint optionID, uint processID)
		{
			OptionEventArgs optionEventArgs = new OptionEventArgs();
			optionEventArgs.ProviderId = providerID;
			optionEventArgs.OptionId = optionID;
			optionEventArgs.ProcessId = processID;
			SdpApp.EventsManager.Raise<OptionEventArgs>(SdpApp.EventsManager.ConnectionEvents.OptionAdded, this, optionEventArgs);
			Option option = SdpApp.ConnectionManager.GetOption(optionID, processID);
			if (option.IsOptionLaunchApplication())
			{
				LaunchAppDialogOption launchAppDialogOption = default(LaunchAppDialogOption);
				launchAppDialogOption.Name = option.GetName();
				launchAppDialogOption.Description = option.GetDescription();
				SdpApp.ConnectionManager.LaunchAppDialogOptions.Add(launchAppDialogOption);
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002A76C File Offset: 0x0002896C
		public override void OnOptionCategoryAdded(uint providerID, uint categoryID)
		{
			OptionCategoryAddedEventArgs optionCategoryAddedEventArgs = new OptionCategoryAddedEventArgs();
			optionCategoryAddedEventArgs.ProviderId = providerID;
			optionCategoryAddedEventArgs.OptionCategory = this.m_client.GetOptionCategory(categoryID);
			SdpApp.EventsManager.Raise<OptionCategoryAddedEventArgs>(SdpApp.EventsManager.ConnectionEvents.OptionCategoryAdded, this, optionCategoryAddedEventArgs);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002A7B4 File Offset: 0x000289B4
		public override void OnDataProcessed(uint captureID, uint bufferCategory, uint bufferID, string error)
		{
			DataProcessedEventArgs dataProcessedEventArgs = new DataProcessedEventArgs();
			dataProcessedEventArgs.CaptureID = captureID;
			dataProcessedEventArgs.BufferID = bufferID;
			dataProcessedEventArgs.BufferCategory = bufferCategory;
			SdpApp.EventsManager.Raise<DataProcessedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, dataProcessedEventArgs);
			if (!string.IsNullOrEmpty(error))
			{
				ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
				showMessageDialogCommand.Message = error;
				showMessageDialogCommand.IconType = IconType.Error;
				SdpApp.CommandManager.ExecuteCommand(showMessageDialogCommand);
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002A820 File Offset: 0x00028A20
		public override void OnDeviceMemoryLow()
		{
			new ShowMessageDialogCommand
			{
				Message = "Device is running low on memory!",
				IconType = IconType.Warning
			}.Execute();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002A84C File Offset: 0x00028A4C
		public override void OnBufferTransferProgress(uint providerID, uint captureID, uint bufferCategory, uint bufferID, uint totalBytes, uint bytesReceived)
		{
			BufferTransferProgressEventArgs bufferTransferProgressEventArgs = new BufferTransferProgressEventArgs();
			bufferTransferProgressEventArgs.ProviderID = providerID;
			bufferTransferProgressEventArgs.BufferCategory = bufferCategory;
			bufferTransferProgressEventArgs.BufferID = bufferID;
			bufferTransferProgressEventArgs.CaptureID = captureID;
			bufferTransferProgressEventArgs.TotalBytes = totalBytes;
			bufferTransferProgressEventArgs.BytesReceived = bytesReceived;
			SdpApp.EventsManager.Raise<BufferTransferProgressEventArgs>(SdpApp.EventsManager.ConnectionEvents.BufferTransferProgress, this, bufferTransferProgressEventArgs);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002A8A7 File Offset: 0x00028AA7
		public override void OnMaxCaptureDurationExpired(uint captureId)
		{
			this.m_traceTimeoutExpirationManager.CaptureTimedOut(captureId);
		}

		// Token: 0x04000977 RID: 2423
		private Client m_client;

		// Token: 0x04000978 RID: 2424
		private ConnectionManager m_container;

		// Token: 0x04000979 RID: 2425
		private InternalClientDelegate.InternalTraceTimeoutExpirationManager m_traceTimeoutExpirationManager = new InternalClientDelegate.InternalTraceTimeoutExpirationManager();

		// Token: 0x0400097A RID: 2426
		private ILogger m_Logger = new Sdp.Logging.Logger("InternalClientDelegate");

		// Token: 0x020003DF RID: 991
		private class InternalTraceTimeoutExpirationManager
		{
			// Token: 0x060012AD RID: 4781 RVA: 0x0003A988 File Offset: 0x00038B88
			public InternalTraceTimeoutExpirationManager()
			{
				ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
				connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			}

			// Token: 0x060012AE RID: 4782 RVA: 0x0003A9DC File Offset: 0x00038BDC
			public void CaptureTimedOut(uint captureId)
			{
				this.m_timedOutCaptureIds.TryAdd(captureId, 0);
				if (this.m_dataProcessedCaptureIds.ContainsKey(captureId))
				{
					this.RaiseMaxCaptureDurationExpired(captureId);
					return;
				}
				MaxCaptureDurationExpiredEventArgs maxCaptureDurationExpiredEventArgs = new MaxCaptureDurationExpiredEventArgs();
				maxCaptureDurationExpiredEventArgs.CaptureId = captureId;
				maxCaptureDurationExpiredEventArgs.DataProcessed = false;
				SdpApp.EventsManager.Raise<MaxCaptureDurationExpiredEventArgs>(SdpApp.EventsManager.ConnectionEvents.MaxCaptureDurationExpired, this, maxCaptureDurationExpiredEventArgs);
			}

			// Token: 0x060012AF RID: 4783 RVA: 0x0003AA3C File Offset: 0x00038C3C
			private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
			{
				if (!SDPCore.IsBufferCategoryApiTraceData(args.BufferCategory))
				{
					return;
				}
				this.m_dataProcessedCaptureIds.TryAdd(args.CaptureID, 0);
				if (this.m_timedOutCaptureIds.ContainsKey(args.CaptureID))
				{
					this.RaiseMaxCaptureDurationExpired(args.CaptureID);
				}
			}

			// Token: 0x060012B0 RID: 4784 RVA: 0x0003AA8C File Offset: 0x00038C8C
			private void RaiseMaxCaptureDurationExpired(uint captureId)
			{
				Task.Factory.StartNew(delegate
				{
					MaxCaptureDurationExpiredEventArgs maxCaptureDurationExpiredEventArgs = new MaxCaptureDurationExpiredEventArgs();
					DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
					Model model = ((dataModel != null) ? dataModel.GetModel("GLESModel") : null);
					ModelObject modelObject = ((dataModel != null) ? dataModel.GetModelObject(model, "tblGPUScopeMarkers") : null);
					ModelObjectDataList modelObjectDataList = ((dataModel != null) ? dataModel.GetModelObjectData(modelObject, "CaptureID", captureId.ToString()) : null);
					if (modelObjectDataList != null && modelObjectDataList.Count >= 1)
					{
						IEnumerable<ModelObjectData> enumerable = modelObjectDataList.Where((ModelObjectData m) => m.GetValue("MarkerType") == "1");
						maxCaptureDurationExpiredEventArgs.FirstFrame = enumerable.Min((ModelObjectData m) => uint.Parse(m.GetValue("Payload0")));
						maxCaptureDurationExpiredEventArgs.LastFrame = enumerable.Max((ModelObjectData m) => uint.Parse(m.GetValue("Payload0")));
						maxCaptureDurationExpiredEventArgs.AnyFrameCollected = true;
						maxCaptureDurationExpiredEventArgs.DataProcessed = true;
					}
					else
					{
						maxCaptureDurationExpiredEventArgs.AnyFrameCollected = false;
					}
					maxCaptureDurationExpiredEventArgs.CaptureId = captureId;
					SdpApp.EventsManager.Raise<MaxCaptureDurationExpiredEventArgs>(SdpApp.EventsManager.ConnectionEvents.MaxCaptureDurationExpired, this, maxCaptureDurationExpiredEventArgs);
					byte b;
					this.m_timedOutCaptureIds.TryRemove(captureId, out b);
					this.m_dataProcessedCaptureIds.TryRemove(captureId, out b);
				});
			}

			// Token: 0x04000D8B RID: 3467
			private ConcurrentDictionary<uint, byte> m_timedOutCaptureIds = new ConcurrentDictionary<uint, byte>();

			// Token: 0x04000D8C RID: 3468
			private ConcurrentDictionary<uint, byte> m_dataProcessedCaptureIds = new ConcurrentDictionary<uint, byte>();
		}
	}
}
