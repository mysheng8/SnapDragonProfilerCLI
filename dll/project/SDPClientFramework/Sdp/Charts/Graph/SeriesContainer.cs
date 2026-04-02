using System;
using System.Collections.Generic;

namespace Sdp.Charts.Graph
{
	// Token: 0x020002F7 RID: 759
	public class SeriesContainer
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0003003A File Offset: 0x0002E23A
		public double MinHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00030042 File Offset: 0x0002E242
		public double MaxHeight
		{
			get
			{
				return this.m_maxHeight;
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0003004A File Offset: 0x0002E24A
		public void Add(int key, Series s)
		{
			s.ResetMinMax += this.series_ResetMinMax;
			this.m_series.Add(key, s);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003006C File Offset: 0x0002E26C
		private void series_ResetMinMax(object sender, EventArgs e)
		{
			this.m_minHeight = 2147483647.0;
			this.m_maxHeight = -2147483648.0;
			foreach (KeyValuePair<int, Series> keyValuePair in this.m_series)
			{
				if (keyValuePair.Value.MaxHeight > this.m_maxHeight)
				{
					this.m_maxHeight = keyValuePair.Value.MaxHeight;
				}
				if (keyValuePair.Value.MinHeight < this.m_minHeight)
				{
					this.m_minHeight = keyValuePair.Value.MinHeight;
				}
			}
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00030124 File Offset: 0x0002E324
		public void Remove(int key)
		{
			this.m_series.Remove(key);
			this.series_ResetMinMax(this, new EventArgs());
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0003013F File Offset: 0x0002E33F
		public int Count
		{
			get
			{
				return this.m_series.Count;
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003014C File Offset: 0x0002E34C
		public Dictionary<int, Series>.Enumerator GetEnumerator()
		{
			return this.m_series.GetEnumerator();
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00030159 File Offset: 0x0002E359
		public bool ContainsKey(int key)
		{
			return this.m_series.ContainsKey(key);
		}

		// Token: 0x170002D3 RID: 723
		public Series this[int key]
		{
			get
			{
				return this.m_series[key];
			}
			set
			{
				this.m_series[key] = value;
			}
		}

		// Token: 0x04000A9F RID: 2719
		private Dictionary<int, Series> m_series = new Dictionary<int, Series>();

		// Token: 0x04000AA0 RID: 2720
		private double m_minHeight = 2147483647.0;

		// Token: 0x04000AA1 RID: 2721
		private double m_maxHeight = -2147483648.0;
	}
}
