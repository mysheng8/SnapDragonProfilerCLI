using System;

namespace SdpClientFramework.DesignPatterns.SingleConsumer
{
	// Token: 0x02000014 RID: 20
	public interface IActionQueue
	{
		// Token: 0x06000045 RID: 69
		void Queue(Action action);
	}
}
