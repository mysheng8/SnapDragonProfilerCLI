using System;

namespace Sdp
{
	// Token: 0x02000280 RID: 640
	public interface IViewController
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000B05 RID: 2821
		IView View { get; }

		// Token: 0x06000B06 RID: 2822
		ViewDesc SaveSettings();

		// Token: 0x06000B07 RID: 2823
		bool LoadSettings(ViewDesc view_desc);
	}
}
