using System;

namespace Sdp
{
	// Token: 0x02000283 RID: 643
	public interface ICaptureViewProvider : IViewProvider
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000B0C RID: 2828
		// (set) Token: 0x06000B0D RID: 2829
		CaptureType CaptureType { get; set; }
	}
}
