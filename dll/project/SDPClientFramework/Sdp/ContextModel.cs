using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000184 RID: 388
	public class ContextModel
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000ABA8 File Offset: 0x00008DA8
		public HashSet<uint> ContextOptions
		{
			get
			{
				object mutex = this.m_mutex;
				HashSet<uint> hashSet;
				lock (mutex)
				{
					hashSet = new HashSet<uint>(this.m_contextOptions);
				}
				return hashSet;
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		public ContextModel()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents.OptionAdded, new EventHandler<OptionEventArgs>(this.ConnectionEvents_OptionAdded));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000AC44 File Offset: 0x00008E44
		private void ConnectionEvents_OptionAdded(object sender, OptionEventArgs e)
		{
			if (SdpApp.ConnectionManager.GetOption(e.OptionId, e.ProcessId).IsOptionContextState() && e.ProcessId == 4294967295U)
			{
				object mutex = this.m_mutex;
				lock (mutex)
				{
					this.m_contextOptions.Add(e.OptionId);
				}
			}
		}

		// Token: 0x04000605 RID: 1541
		private HashSet<uint> m_contextOptions = new HashSet<uint>();

		// Token: 0x04000606 RID: 1542
		private object m_mutex = new object();
	}
}
