using System;

namespace Sdp
{
	// Token: 0x02000281 RID: 641
	public interface IViewProvider
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000B08 RID: 2824
		string ViewTypeName { get; }

		// Token: 0x06000B09 RID: 2825
		IViewController CreateView();
	}
}
