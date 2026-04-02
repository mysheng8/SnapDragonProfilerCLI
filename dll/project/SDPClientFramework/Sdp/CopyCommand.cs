using System;

namespace Sdp
{
	// Token: 0x02000060 RID: 96
	public class CopyCommand : Command
	{
		// Token: 0x06000239 RID: 569 RVA: 0x00007D00 File Offset: 0x00005F00
		protected override void OnExecute()
		{
			SdpApp.EventsManager.Raise<EventArgs>(SdpApp.EventsManager.ClientEvents.CopyFocusedContent, this, EventArgs.Empty);
		}
	}
}
