using System;
using System.Collections.Generic;

namespace Sdp.Charts.Roofline
{
	// Token: 0x020002FC RID: 764
	public class SeriesContainer
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00030552 File Offset: 0x0002E752
		public double MinHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0003055A File Offset: 0x0002E75A
		public double MaxHeight
		{
			get
			{
				return this.m_maxHeight;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00030564 File Offset: 0x0002E764
		public void Add(int key, Series s)
		{
			s.ResetMinMax += this.series_ResetMinMax;
			this.m_series.Add(key, s);
			if (s.Duration > 0f)
			{
				if (s.Duration < this.m_minDuration)
				{
					this.m_minDuration = s.Duration;
				}
				if (s.Duration > this.m_maxDuration)
				{
					this.m_maxDuration = s.Duration;
				}
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000305D4 File Offset: 0x0002E7D4
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

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003068C File Offset: 0x0002E88C
		public void Remove(int key)
		{
			this.m_series.Remove(key);
			this.series_ResetMinMax(this, new EventArgs());
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x000306A7 File Offset: 0x0002E8A7
		public int Count
		{
			get
			{
				return this.m_series.Count;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x000306B4 File Offset: 0x0002E8B4
		public Dictionary<int, Series>.Enumerator GetEnumerator()
		{
			return this.m_series.GetEnumerator();
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x000306C1 File Offset: 0x0002E8C1
		public bool ContainsKey(int key)
		{
			return this.m_series.ContainsKey(key);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000306CF File Offset: 0x0002E8CF
		public bool TryGetValue(int key, out Series series)
		{
			return this.m_series.TryGetValue(key, out series);
		}

		// Token: 0x170002DF RID: 735
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

		// Token: 0x06000FAE RID: 4014 RVA: 0x000306FC File Offset: 0x0002E8FC
		public void RecalculateLineWidths()
		{
			float num = this.m_maxDuration - this.m_minDuration;
			if (num == 0f)
			{
				foreach (KeyValuePair<int, Series> keyValuePair in this.m_series)
				{
					keyValuePair.Value.LineWidth = 15f;
				}
				return;
			}
			foreach (KeyValuePair<int, Series> keyValuePair2 in this.m_series)
			{
				if (keyValuePair2.Value.Duration > 0f)
				{
					keyValuePair2.Value.LineWidth = 15f * (1f - (this.m_maxDuration - keyValuePair2.Value.Duration) / num) + 10f;
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x000307F4 File Offset: 0x0002E9F4
		public float MinDuration
		{
			get
			{
				return this.m_minDuration;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x000307FC File Offset: 0x0002E9FC
		public float MaxDuration
		{
			get
			{
				return this.m_maxDuration;
			}
		}

		// Token: 0x04000AB1 RID: 2737
		private Dictionary<int, Series> m_series = new Dictionary<int, Series>();

		// Token: 0x04000AB2 RID: 2738
		private double m_minHeight = 2147483647.0;

		// Token: 0x04000AB3 RID: 2739
		private double m_maxHeight = -2147483648.0;

		// Token: 0x04000AB4 RID: 2740
		private float m_minDuration = float.MaxValue;

		// Token: 0x04000AB5 RID: 2741
		private float m_maxDuration = float.MinValue;
	}
}
