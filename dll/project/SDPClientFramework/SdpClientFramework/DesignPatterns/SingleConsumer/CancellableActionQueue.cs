using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SdpClientFramework.DesignPatterns.SingleConsumer
{
	// Token: 0x02000013 RID: 19
	public class CancellableActionQueue : IActionQueue<CancellationToken>
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002EB4 File Offset: 0x000010B4
		public virtual void Queue(Action<CancellationToken> action)
		{
			if (this.m_executeTask == null)
			{
				this.CreateExecuteTask();
			}
			this.m_actionQueue.Add(action);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002ED0 File Offset: 0x000010D0
		private void CreateExecuteTask()
		{
			this.m_executeTask = Task.Factory.StartNew(new Action(this.ExecuteLoop), TaskCreationOptions.LongRunning);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002EF0 File Offset: 0x000010F0
		private void ExecuteLoop()
		{
			for (;;)
			{
				Action<CancellationToken> nextAction = this.m_actionQueue.Take();
				this.CancelRunningAction();
				this.m_runningAction = Task.Factory.StartNew(delegate
				{
					nextAction(this.m_cancelSource.Token);
				});
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F3E File Offset: 0x0000113E
		private void CancelRunningAction()
		{
			if (this.m_runningAction != null)
			{
				this.m_cancelSource.Cancel();
				this.m_runningAction.Wait();
				this.m_cancelSource.Dispose();
			}
			this.m_cancelSource = new CancellationTokenSource();
		}

		// Token: 0x040000AD RID: 173
		private CancellationTokenSource m_cancelSource;

		// Token: 0x040000AE RID: 174
		private Task m_runningAction;

		// Token: 0x040000AF RID: 175
		private Task m_executeTask;

		// Token: 0x040000B0 RID: 176
		private readonly BlockingCollection<Action<CancellationToken>> m_actionQueue = new BlockingCollection<Action<CancellationToken>>();
	}
}
