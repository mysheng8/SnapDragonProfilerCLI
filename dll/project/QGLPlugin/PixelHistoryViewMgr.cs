using System;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000036 RID: 54
	internal class PixelHistoryViewMgr
	{
		// Token: 0x060000EC RID: 236 RVA: 0x0000B14C File Offset: 0x0000934C
		public PixelHistoryViewMgr()
		{
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.CurrentSnapshotControllerChanged = (EventHandler)Delegate.Combine(snapshotEvents.CurrentSnapshotControllerChanged, new EventHandler(this.OnCurrentSnapshotControllerChanged));
			SnapshotEvents snapshotEvents2 = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents2.SnapshotProviderChanged = (EventHandler<SnapshotProviderChangedArgs>)Delegate.Combine(snapshotEvents2.SnapshotProviderChanged, new EventHandler<SnapshotProviderChangedArgs>(this.OnSnapshotProviderChanged));
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000B1B8 File Offset: 0x000093B8
		private void OnCurrentSnapshotControllerChanged(object sender, EventArgs args)
		{
			uint currentSnapshotProviderID = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CurrentSnapshotProviderID;
			uint? num = QGLPlugin.ProviderID;
			if ((currentSnapshotProviderID == num.GetValueOrDefault()) & (num != null))
			{
				this.SetPixelHistoryStatusMessage();
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000B200 File Offset: 0x00009400
		private void OnSnapshotProviderChanged(object sender, SnapshotProviderChangedArgs args)
		{
			uint selectedProvider = args.SelectedProvider;
			uint? num = QGLPlugin.ProviderID;
			if ((selectedProvider == num.GetValueOrDefault()) & (num != null))
			{
				this.SetPixelHistoryStatusMessage();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000B238 File Offset: 0x00009438
		private void SetPixelHistoryStatusMessage()
		{
			SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
			setStatusEventArgs.Status = StatusType.Neutral;
			setStatusEventArgs.StatusText = "Pixel history not yet supported for Vulkan";
			setStatusEventArgs.Duration = 0;
			SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.PixelHistoryEvents.SetStatus, null, setStatusEventArgs);
		}
	}
}
