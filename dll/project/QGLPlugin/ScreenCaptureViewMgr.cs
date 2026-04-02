using System;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000038 RID: 56
	internal class ScreenCaptureViewMgr
	{
		// Token: 0x06000119 RID: 281 RVA: 0x0001017C File Offset: 0x0000E37C
		public ScreenCaptureViewMgr()
		{
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.SnapshotProviderChanged = (EventHandler<SnapshotProviderChangedArgs>)Delegate.Combine(snapshotEvents.SnapshotProviderChanged, new EventHandler<SnapshotProviderChangedArgs>(this.OnSnapshotProviderChanged));
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000101AF File Offset: 0x0000E3AF
		private void OnSnapshotProviderChanged(object sender, SnapshotProviderChangedArgs args)
		{
			if (args.SelectedProvider == 353U)
			{
				SdpApp.EventsManager.Raise<EnableEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.EnablePicking, null, new EnableEventArgs
				{
					Enable = false
				});
			}
		}
	}
}
