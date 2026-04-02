using System;

namespace Sdp
{
	// Token: 0x02000235 RID: 565
	public class MetricCategoryArgs : EventArgs
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x0001A842 File Offset: 0x00018A42
		public MetricCategoryArgs(uint category)
		{
			this.Category = category;
		}

		// Token: 0x040007FC RID: 2044
		public uint Category;
	}
}
