using System;
using Cairo;

namespace Sdp
{
	// Token: 0x02000234 RID: 564
	public class MetricColorArgs : EventArgs
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x0001A81D File Offset: 0x00018A1D
		public MetricColorArgs(uint metricID, uint categoryID, uint processID, uint captureID)
		{
			this.MetricID = metricID;
			this.CategoryID = categoryID;
			this.ProcessID = processID;
			this.CaptureID = captureID;
		}

		// Token: 0x040007F7 RID: 2039
		public uint MetricID;

		// Token: 0x040007F8 RID: 2040
		public uint CategoryID;

		// Token: 0x040007F9 RID: 2041
		public uint ProcessID;

		// Token: 0x040007FA RID: 2042
		public Color Color;

		// Token: 0x040007FB RID: 2043
		public uint CaptureID;
	}
}
