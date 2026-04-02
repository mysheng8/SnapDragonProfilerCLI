using System;
using Cairo;

namespace Sdp
{
	// Token: 0x0200021A RID: 538
	public class GraphTrackMetric : IGraphDataSeries
	{
		// Token: 0x1400005B RID: 91
		// (add) Token: 0x060007F0 RID: 2032 RVA: 0x00015A3C File Offset: 0x00013C3C
		// (remove) Token: 0x060007F1 RID: 2033 RVA: 0x00015A74 File Offset: 0x00013C74
		public event EventHandler DataChanged;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x060007F2 RID: 2034 RVA: 0x00015AAC File Offset: 0x00013CAC
		// (remove) Token: 0x060007F3 RID: 2035 RVA: 0x00015AE4 File Offset: 0x00013CE4
		public event EventHandler StateChanged;

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00015B19 File Offset: 0x00013D19
		private Metric Metric
		{
			get
			{
				if (this.MetricId == 4294967295U)
				{
					return null;
				}
				return MetricManager.Get().GetMetric(this.MetricId);
			}
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00015B36 File Offset: 0x00013D36
		public void RefreshState()
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00015B51 File Offset: 0x00013D51
		public int SeriesID
		{
			get
			{
				return (int)this.MetricId;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00015B59 File Offset: 0x00013D59
		public string Name
		{
			get
			{
				return this.m_metric_desc.MetricName;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00015B66 File Offset: 0x00013D66
		public string ProcessName
		{
			get
			{
				return this.m_metric_desc.ProcessName;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00015B73 File Offset: 0x00013D73
		public uint CategoryId
		{
			get
			{
				return this.m_metric_desc.CategoryId;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00015B80 File Offset: 0x00013D80
		public uint ProviderId
		{
			get
			{
				return this.m_metric_desc.ProviderId;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00015B8D File Offset: 0x00013D8D
		public uint ProcessId
		{
			get
			{
				return this.m_metric_desc.ProcessId;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00015B9A File Offset: 0x00013D9A
		public uint MetricId
		{
			get
			{
				return this.m_metric_desc.MetricId;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00015BA7 File Offset: 0x00013DA7
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x00015BCD File Offset: 0x00013DCD
		public string MetricTooltip
		{
			get
			{
				if (this.m_metric_desc.MetricTooltip == null)
				{
					return this.m_metric_desc.MetricName;
				}
				return this.m_metric_desc.MetricTooltip;
			}
			set
			{
				this.m_metric_desc.MetricTooltip = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00015BDB File Offset: 0x00013DDB
		public bool Enabled
		{
			get
			{
				return this.MetricId == uint.MaxValue || this.Metric.IsActive(this.ProcessId, 1U);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00015BFA File Offset: 0x00013DFA
		public uint CaptureId
		{
			get
			{
				return this.m_captureId;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00015C02 File Offset: 0x00013E02
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00015C0A File Offset: 0x00013E0A
		public Color Color
		{
			get
			{
				return this.m_color;
			}
			set
			{
				this.m_color = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00015C02 File Offset: 0x00013E02
		public Color LineColor
		{
			get
			{
				return this.m_color;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00015C14 File Offset: 0x00013E14
		public Color AreaColor
		{
			get
			{
				return new Color(this.m_color.R * 0.9, this.m_color.G * 0.9, this.m_color.B * 0.9);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00015C65 File Offset: 0x00013E65
		public DataPointList DataPoints
		{
			get
			{
				return this.m_data_points;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00015C6D File Offset: 0x00013E6D
		public double MinValue
		{
			get
			{
				return this.m_data_points.MinValue;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00015C7A File Offset: 0x00013E7A
		public double MaxValue
		{
			get
			{
				return this.m_data_points.MaxValue;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00015C87 File Offset: 0x00013E87
		public MetricDesc Descriptor
		{
			get
			{
				return this.m_metric_desc;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00015C8F File Offset: 0x00013E8F
		public MetricSettings Settings
		{
			get
			{
				return this.m_settings;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00015C98 File Offset: 0x00013E98
		public GraphTrackMetric(GraphTrackMetricDesc desc)
		{
			if (desc != null)
			{
				this.m_metric_desc = desc.Metric;
				this.m_color = desc.Color;
				this.m_captureId = desc.CaptureId;
			}
			else
			{
				this.m_metric_desc.MetricId = uint.MaxValue;
			}
			if (!this.m_metric_desc.IsPreview)
			{
				this.Init();
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00015D30 File Offset: 0x00013F30
		public void AddData(double x, object y)
		{
			double num = (double)y;
			if (double.IsInfinity(num))
			{
				num = 0.0;
			}
			DataPoint dataPoint = new DataPoint(x, num);
			this.m_data_points.AddData(dataPoint);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00015D6A File Offset: 0x00013F6A
		public void SetData(DataPointList newList)
		{
			if (newList != null)
			{
				newList.CollectionChanged += this.OnDataPointsCollectionChanged;
				this.m_data_points.CollectionChanged -= this.OnDataPointsCollectionChanged;
				this.m_data_points = newList;
				this.m_data_points.DataListChanged();
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00015DAA File Offset: 0x00013FAA
		public void MakeTransient()
		{
			this.m_metric_desc.MetricType = MetricType.Transient;
			this.m_metric_desc.MetricId = 0U;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00015DC4 File Offset: 0x00013FC4
		private void Init()
		{
			this.m_data_points.CollectionChanged += this.OnDataPointsCollectionChanged;
			this.EnableMetric();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00015DE4 File Offset: 0x00013FE4
		private void EnableMetric()
		{
			if (this.m_metric_desc.ProcessName != null)
			{
				Process processByID = SdpApp.ConnectionManager.GetProcessByID(this.m_metric_desc.ProcessId);
				if (processByID != null)
				{
					EnableMetricEventArgs enableMetricEventArgs = new EnableMetricEventArgs();
					enableMetricEventArgs.Enable = true;
					enableMetricEventArgs.CaptureId = this.m_captureId;
					enableMetricEventArgs.PID = processByID.GetProperties().pid;
					enableMetricEventArgs.MetricId = this.MetricId;
					SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs);
					return;
				}
				uint maxValue = uint.MaxValue;
				EnableMetricEventArgs enableMetricEventArgs2 = new EnableMetricEventArgs();
				enableMetricEventArgs2.Enable = true;
				enableMetricEventArgs2.CaptureId = this.m_captureId;
				enableMetricEventArgs2.PID = maxValue;
				enableMetricEventArgs2.MetricId = this.MetricId;
				SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs2);
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00015EB5 File Offset: 0x000140B5
		private void OnDataPointsCollectionChanged(object sender, EventArgs e)
		{
			if (this.DataChanged != null)
			{
				this.DataChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400079A RID: 1946
		private MetricDesc m_metric_desc;

		// Token: 0x0400079B RID: 1947
		private uint m_captureId;

		// Token: 0x0400079C RID: 1948
		private Color m_color = new Color(1.0, 1.0, 1.0);

		// Token: 0x0400079D RID: 1949
		private DataPointList m_data_points = new DataPointList();

		// Token: 0x0400079E RID: 1950
		private readonly MetricSettings m_settings = new MetricSettings();
	}
}
