using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000219 RID: 537
	public class DataPointList : List<DataPoint>
	{
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060007E1 RID: 2017 RVA: 0x00015824 File Offset: 0x00013A24
		// (remove) Token: 0x060007E2 RID: 2018 RVA: 0x0001585C File Offset: 0x00013A5C
		public event EventHandler CollectionChanged;

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00015891 File Offset: 0x00013A91
		public double MinValue
		{
			get
			{
				if (base.Count <= 0)
				{
					return 0.0;
				}
				return this.m_min_value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x000158AC File Offset: 0x00013AAC
		public double MaxValue
		{
			get
			{
				if (base.Count <= 0)
				{
					return 0.0;
				}
				return this.m_max_value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x000158C7 File Offset: 0x00013AC7
		public double MinTimestamp
		{
			get
			{
				if (base.Count != 0)
				{
					return base[0].X;
				}
				return double.MinValue;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x000158E7 File Offset: 0x00013AE7
		public double MaxTimestamp
		{
			get
			{
				if (base.Count != 0)
				{
					return base[base.Count - 1].X;
				}
				return double.MaxValue;
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00015910 File Offset: 0x00013B10
		public void AddData(DataPoint item)
		{
			SdpApp.Platform.Invoke(delegate
			{
				this.<>n__0(item);
				this.UpdateBounds(item);
				if (this.CollectionChanged != null)
				{
					this.CollectionChanged(this, EventArgs.Empty);
				}
			});
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00015948 File Offset: 0x00013B48
		private void UpdateBounds(DataPoint item)
		{
			double y = item.Y;
			if (y > this.m_max_value)
			{
				this.m_max_value = y;
			}
			if (y < this.m_min_value)
			{
				this.m_min_value = y;
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001597C File Offset: 0x00013B7C
		public void DataListChanged()
		{
			SdpApp.Platform.Invoke(delegate
			{
				if (this.CollectionChanged != null)
				{
					this.CollectionChanged(this, EventArgs.Empty);
				}
			});
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00015994 File Offset: 0x00013B94
		public new void Add(DataPoint item)
		{
			this.AddData(item);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001599D File Offset: 0x00013B9D
		public void AddImmediate(DataPoint item)
		{
			base.Add(item);
			this.UpdateBounds(item);
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000159C6 File Offset: 0x00013BC6
		public void InsertImmediate(int index, DataPoint item)
		{
			base.Insert(index, item);
			this.UpdateBounds(item);
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x04000796 RID: 1942
		private double m_min_value = double.MaxValue;

		// Token: 0x04000797 RID: 1943
		private double m_max_value = double.MinValue;
	}
}
