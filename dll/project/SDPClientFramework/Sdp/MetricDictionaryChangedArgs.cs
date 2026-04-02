using System;

namespace Sdp
{
	// Token: 0x020000C5 RID: 197
	public class MetricDictionaryChangedArgs : EventArgs
	{
		// Token: 0x040002C4 RID: 708
		public MetricList Added = new MetricList();

		// Token: 0x040002C5 RID: 709
		public MetricList Removed = new MetricList();
	}
}
