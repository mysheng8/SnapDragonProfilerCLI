using System;
using Cairo;

namespace Sdp
{
	// Token: 0x0200021B RID: 539
	public class MetricSettings
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x00015ED0 File Offset: 0x000140D0
		public MetricSettings()
		{
			this.SMAEnabled = false;
			this.SMALength = 250L;
			this.SMAColor = null;
			this.CMAEnabled = false;
			this.CMAColor = null;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00015F1B File Offset: 0x0001411B
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00015F23 File Offset: 0x00014123
		public bool SMAEnabled { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00015F2C File Offset: 0x0001412C
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00015F34 File Offset: 0x00014134
		public long SMALength { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00015F3D File Offset: 0x0001413D
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00015F45 File Offset: 0x00014145
		public Color? SMAColor { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00015F4E File Offset: 0x0001414E
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00015F56 File Offset: 0x00014156
		public int SMASeriesID { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00015F5F File Offset: 0x0001415F
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00015F67 File Offset: 0x00014167
		public bool CMAEnabled { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00015F70 File Offset: 0x00014170
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00015F78 File Offset: 0x00014178
		public Color? CMAColor { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00015F81 File Offset: 0x00014181
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00015F89 File Offset: 0x00014189
		public int CMASeriesID { get; set; }
	}
}
