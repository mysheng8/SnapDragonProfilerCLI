using System;
using System.Threading.Tasks;

namespace Sdp.Concurrency
{
	// Token: 0x0200031A RID: 794
	public static class ConcurrencyHelpers
	{
		// Token: 0x06001080 RID: 4224 RVA: 0x00033FE2 File Offset: 0x000321E2
		public static Task<bool> AsyncTrue()
		{
			return Task.Factory.StartNew<bool>(() => true);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0003400D File Offset: 0x0003220D
		public static Task<bool> AsyncFalse()
		{
			return Task.Factory.StartNew<bool>(() => false);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00034038 File Offset: 0x00032238
		public static Task FromAction(Action action)
		{
			return Task.Factory.StartNew(action);
		}
	}
}
