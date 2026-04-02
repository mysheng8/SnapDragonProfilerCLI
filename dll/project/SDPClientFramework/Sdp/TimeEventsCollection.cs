using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200028E RID: 654
	public class TimeEventsCollection
	{
		// Token: 0x06000BF3 RID: 3059 RVA: 0x00022FB1 File Offset: 0x000211B1
		public TimeEventsCollection()
		{
			this.m_timeEvents = new Dictionary<uint, TimeEvents>();
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00022FC4 File Offset: 0x000211C4
		public Dictionary<uint, TimeEvents> TimeEvents
		{
			get
			{
				return this.m_timeEvents;
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00022FCC File Offset: 0x000211CC
		public TimeEvents GetTimeEvents(uint key)
		{
			TimeEvents timeEvents;
			if (!this.m_timeEvents.TryGetValue(key, out timeEvents))
			{
				return null;
			}
			return timeEvents;
		}

		// Token: 0x0400090A RID: 2314
		public EventHandler<AutoScaleEventArgs> AutoScaleToggled;

		// Token: 0x0400090B RID: 2315
		public EventHandler<InteractionModeChangedEventArgs> InteractionModeChanged;

		// Token: 0x0400090C RID: 2316
		public EventHandler<ComboBoxUpdatedEventArgs> ComboBoxUpdated;

		// Token: 0x0400090D RID: 2317
		private Dictionary<uint, TimeEvents> m_timeEvents;
	}
}
