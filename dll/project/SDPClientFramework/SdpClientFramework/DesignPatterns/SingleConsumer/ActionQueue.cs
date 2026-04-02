using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SdpClientFramework.DesignPatterns.SingleConsumer
{
	// Token: 0x02000012 RID: 18
	public class ActionQueue : IActionQueue
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002E38 File Offset: 0x00001038
		public ActionQueue(bool lazyInitialization = false)
		{
			if (!lazyInitialization)
			{
				this.CreateExecuteTask();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E59 File Offset: 0x00001059
		public virtual void Queue(Action action)
		{
			if (this.m_executeTask == null)
			{
				this.CreateExecuteTask();
			}
			this.m_actionQueue.Add(action);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E75 File Offset: 0x00001075
		private void CreateExecuteTask()
		{
			this.m_executeTask = Task.Factory.StartNew(new Action(this.ExecuteLoop), TaskCreationOptions.LongRunning);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E94 File Offset: 0x00001094
		private void ExecuteLoop()
		{
			for (;;)
			{
				Action action = this.m_actionQueue.Take();
				action();
			}
		}

		// Token: 0x040000AB RID: 171
		private Task m_executeTask;

		// Token: 0x040000AC RID: 172
		private readonly BlockingCollection<Action> m_actionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
	}
}
