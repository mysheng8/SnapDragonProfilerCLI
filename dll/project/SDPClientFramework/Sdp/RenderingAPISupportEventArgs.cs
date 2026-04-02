using System;

namespace Sdp
{
	// Token: 0x020000C6 RID: 198
	public class RenderingAPISupportEventArgs : EventArgs
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00009961 File Offset: 0x00007B61
		public RenderingAPISupportEventArgs(RenderingAPI renderingAPI)
		{
			this.renderingAPI = renderingAPI;
		}

		// Token: 0x040002C6 RID: 710
		public RenderingAPI renderingAPI;
	}
}
