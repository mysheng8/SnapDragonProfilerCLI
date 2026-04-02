using System;

namespace SdpClientFramework.DesignPatterns.SingleConsumer
{
	// Token: 0x02000015 RID: 21
	public interface IActionQueue<T>
	{
		// Token: 0x06000046 RID: 70
		void Queue(Action<T> action);
	}
}
