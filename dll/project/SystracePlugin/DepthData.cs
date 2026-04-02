using System;
using System.Collections.Generic;
using Cairo;
using Sdp.Charts.Gantt;

namespace TracePlugin
{
	// Token: 0x02000019 RID: 25
	public class DepthData
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00007425 File Offset: 0x00005625
		public List<Element> BlockElements
		{
			get
			{
				return this.m_series.Elements;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00007432 File Offset: 0x00005632
		public List<Marker> MarkerElements
		{
			get
			{
				return this.m_series.Markers;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000743F File Offset: 0x0000563F
		public Series Series
		{
			get
			{
				return this.m_series;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00007447 File Offset: 0x00005647
		public int Depth
		{
			get
			{
				return this.m_depth;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000744F File Offset: 0x0000564F
		public bool UniformColor
		{
			get
			{
				return this.m_uniformColor;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00007457 File Offset: 0x00005657
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000745F File Offset: 0x0000565F
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00007468 File Offset: 0x00005668
		public bool ShowLabels
		{
			get
			{
				return this.m_showLabels;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007470 File Offset: 0x00005670
		public DepthData(int depth)
		{
			this.m_depth = depth;
			this.m_series = new Series();
			this.m_name = "";
			this.m_showLabels = true;
			this.m_uniformColor = false;
			this.m_color = new Color(0.0, 0.0, 0.0);
			this.MinTimestamp = long.MaxValue;
			this.MaxTimestamp = long.MinValue;
		}

		// Token: 0x04000059 RID: 89
		private int m_depth;

		// Token: 0x0400005A RID: 90
		private Series m_series;

		// Token: 0x0400005B RID: 91
		private string m_name;

		// Token: 0x0400005C RID: 92
		private bool m_showLabels;

		// Token: 0x0400005D RID: 93
		private bool m_uniformColor;

		// Token: 0x0400005E RID: 94
		private Color m_color;

		// Token: 0x0400005F RID: 95
		public long MinTimestamp;

		// Token: 0x04000060 RID: 96
		public long MaxTimestamp;
	}
}
