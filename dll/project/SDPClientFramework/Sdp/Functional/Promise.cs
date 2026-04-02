using System;
using System.Threading.Tasks;

namespace Sdp.Functional
{
	// Token: 0x02000316 RID: 790
	public class Promise<T>
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00033ADF File Offset: 0x00031CDF
		public Task<T> Future
		{
			get
			{
				return this.m_completionToken.Task;
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00033AEC File Offset: 0x00031CEC
		public async Task SetResultAsync(T result)
		{
			await Task.Run(delegate
			{
				this.m_completionToken.SetResult(result);
			});
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00033B37 File Offset: 0x00031D37
		public void SetResult(T result)
		{
			this.m_completionToken.SetResult(result);
		}

		// Token: 0x04000AF0 RID: 2800
		private TaskCompletionSource<T> m_completionToken = new TaskCompletionSource<T>();
	}
}
