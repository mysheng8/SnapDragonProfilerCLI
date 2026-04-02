using System;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x0200003D RID: 61
	internal class VertexDataViewMgr
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00011C64 File Offset: 0x0000FE64
		public VertexDataViewMgr()
		{
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.CurrentSnapshotControllerChanged = (EventHandler)Delegate.Combine(snapshotEvents.CurrentSnapshotControllerChanged, new EventHandler(this.OnCurrentSnapshotControllerChanged));
			SnapshotEvents snapshotEvents2 = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents2.SnapshotProviderChanged = (EventHandler<SnapshotProviderChangedArgs>)Delegate.Combine(snapshotEvents2.SnapshotProviderChanged, new EventHandler<SnapshotProviderChangedArgs>(this.OnSnapshotModeChanged));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00011CD0 File Offset: 0x0000FED0
		private void OnCurrentSnapshotControllerChanged(object sender, EventArgs args)
		{
			uint currentSnapshotProviderID = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.CurrentSnapshotProviderID;
			uint? num = QGLPlugin.ProviderID;
			if ((currentSnapshotProviderID == num.GetValueOrDefault()) & (num != null))
			{
				this.SetDrawcallDataStatusMessage();
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00011D18 File Offset: 0x0000FF18
		private void OnSnapshotModeChanged(object sender, SnapshotProviderChangedArgs args)
		{
			uint selectedProvider = args.SelectedProvider;
			uint? num = QGLPlugin.ProviderID;
			if ((selectedProvider == num.GetValueOrDefault()) & (num != null))
			{
				this.SetDrawcallDataStatusMessage();
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00011D50 File Offset: 0x0000FF50
		private void SetDrawcallDataStatusMessage()
		{
			SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
			setStatusEventArgs.Status = StatusType.Neutral;
			setStatusEventArgs.StatusText = "Drawcall Data not yet supported for Vulkan";
			setStatusEventArgs.Duration = 0;
			SdpApp.EventsManager.Raise<SetStatusEventArgs>(SdpApp.EventsManager.DrawCallDataViewEvents.SetStatus, null, setStatusEventArgs);
		}
	}
}
