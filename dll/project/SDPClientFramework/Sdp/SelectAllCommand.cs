using System;

namespace Sdp
{
	// Token: 0x02000072 RID: 114
	public class SelectAllCommand : Command
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000873B File Offset: 0x0000693B
		protected override void OnExecute()
		{
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ClientEvents.SelectAllContent, this, EventArgs.Empty);
		}
	}
}
