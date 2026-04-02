using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000291 RID: 657
	public class TimeModelCollection
	{
		// Token: 0x06000C03 RID: 3075 RVA: 0x00023210 File Offset: 0x00021410
		public TimeModelCollection()
		{
			this.m_timeModels = new Dictionary<uint, TimeModel>();
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00023223 File Offset: 0x00021423
		public Dictionary<uint, TimeModel> TimeModels
		{
			get
			{
				return this.m_timeModels;
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002322C File Offset: 0x0002142C
		public TimeModel GetTimeModel(uint key)
		{
			TimeModel timeModel;
			if (!this.m_timeModels.TryGetValue(key, out timeModel))
			{
				return null;
			}
			return timeModel;
		}

		// Token: 0x04000919 RID: 2329
		private Dictionary<uint, TimeModel> m_timeModels;
	}
}
