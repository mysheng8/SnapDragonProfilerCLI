using System;

namespace Sdp.Views.SessionSettingsDialog
{
	// Token: 0x020002ED RID: 749
	public class SessionSettingsSelection
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0000204B File Offset: 0x0000024B
		public SessionSettingsSelection()
		{
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0002FB74 File Offset: 0x0002DD74
		public SessionSettingsSelection(uint maxSizeMb, string sessionRootPath)
		{
			this.MaxSizeMB = maxSizeMb;
			this.SessionRootPath = sessionRootPath;
		}

		// Token: 0x04000A67 RID: 2663
		public uint MaxSizeMB;

		// Token: 0x04000A68 RID: 2664
		public string SessionRootPath;
	}
}
