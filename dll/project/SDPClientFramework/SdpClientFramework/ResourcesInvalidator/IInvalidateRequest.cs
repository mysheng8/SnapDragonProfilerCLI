using System;

namespace SdpClientFramework.ResourcesInvalidator
{
	// Token: 0x0200000D RID: 13
	public interface IInvalidateRequest
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12
		uint InvalidateType { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13
		bool Parallel { get; }
	}
}
