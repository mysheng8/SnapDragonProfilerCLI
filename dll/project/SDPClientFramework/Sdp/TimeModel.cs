using System;

namespace Sdp
{
	// Token: 0x02000290 RID: 656
	public class TimeModel
	{
		// Token: 0x06000BF7 RID: 3063 RVA: 0x00022FEC File Offset: 0x000211EC
		public TimeModel(uint captureId)
		{
			this.m_dataViewBoundsMin = double.MaxValue;
			this.m_dataViewBoundsMax = double.MinValue;
			this.m_dataBoundsMin = long.MaxValue;
			this.m_dataBoundsMax = long.MinValue;
			this.m_captureId = captureId;
			this.m_isValid = false;
			this.m_isScrolling = true;
			this.m_scrollingWindow = 10000000.0;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00023060 File Offset: 0x00021260
		public void SetDataViewBounds(double min, double max, bool dirty)
		{
			if (double.IsInfinity(min) || double.IsNaN(min) || double.IsInfinity(max) || double.IsNaN(max))
			{
				return;
			}
			this.m_dataViewBoundsMin = min;
			this.m_dataViewBoundsMax = max;
			this.m_dirty = dirty;
			this.m_isValid = true;
			this.m_scrollingWindow = Math.Max(max - min, 0.0);
			TimeEvents timeEvents = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_captureId);
			if (timeEvents != null)
			{
				SdpApp.EventsManager.Raise(timeEvents.DataViewBoundsChanged, this, EventArgs.Empty);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000230F0 File Offset: 0x000212F0
		public void SetDataBounds(long min, long max, bool resetDataViewBounds = true)
		{
			bool flag = false;
			if (min < this.m_dataBoundsMin)
			{
				this.m_dataBoundsMin = min;
				flag = true;
			}
			if (max > this.m_dataBoundsMax)
			{
				this.m_dataBoundsMax = max;
				flag = true;
			}
			if (flag)
			{
				if (this.m_isScrolling)
				{
					TimeEvents timeEvents = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_captureId);
					if (timeEvents != null)
					{
						this.m_dataViewBoundsMax = (double)this.m_dataBoundsMax;
						this.m_dataViewBoundsMin = this.m_dataViewBoundsMax - this.m_scrollingWindow;
						SdpApp.EventsManager.Raise(timeEvents.DataViewBoundsChanged, this, EventArgs.Empty);
					}
				}
				TimeEvents timeEvents2 = SdpApp.EventsManager.TimeEventsCollection.GetTimeEvents(this.m_captureId);
				if (timeEvents2 != null)
				{
					SdpApp.EventsManager.Raise(timeEvents2.DataBoundsChanged, this, EventArgs.Empty);
				}
			}
			if (resetDataViewBounds)
			{
				this.SetDataViewBounds((double)this.m_dataBoundsMin, (double)this.m_dataBoundsMax, flag);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x000231D0 File Offset: 0x000213D0
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x000231C7 File Offset: 0x000213C7
		public bool IsScrolling
		{
			get
			{
				return this.m_isScrolling;
			}
			set
			{
				this.m_isScrolling = value;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000231D8 File Offset: 0x000213D8
		public double DataViewBoundsMin
		{
			get
			{
				return this.m_dataViewBoundsMin;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000231E0 File Offset: 0x000213E0
		public double DataViewBoundsMax
		{
			get
			{
				return this.m_dataViewBoundsMax;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x000231E8 File Offset: 0x000213E8
		public long DataBoundsMin
		{
			get
			{
				return this.m_dataBoundsMin;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000231F0 File Offset: 0x000213F0
		public long DataBoundsMax
		{
			get
			{
				return this.m_dataBoundsMax;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x000231F8 File Offset: 0x000213F8
		public uint CaptureID
		{
			get
			{
				return this.m_captureId;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00023200 File Offset: 0x00021400
		public bool Dirty
		{
			get
			{
				return this.m_dirty;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00023208 File Offset: 0x00021408
		public bool IsValid
		{
			get
			{
				return this.m_isValid;
			}
		}

		// Token: 0x04000910 RID: 2320
		private double m_dataViewBoundsMin;

		// Token: 0x04000911 RID: 2321
		private double m_dataViewBoundsMax;

		// Token: 0x04000912 RID: 2322
		private long m_dataBoundsMin;

		// Token: 0x04000913 RID: 2323
		private long m_dataBoundsMax;

		// Token: 0x04000914 RID: 2324
		private uint m_captureId;

		// Token: 0x04000915 RID: 2325
		private bool m_dirty;

		// Token: 0x04000916 RID: 2326
		private bool m_isValid;

		// Token: 0x04000917 RID: 2327
		private bool m_isScrolling;

		// Token: 0x04000918 RID: 2328
		private double m_scrollingWindow;
	}
}
