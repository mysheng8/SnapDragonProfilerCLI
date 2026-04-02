using System;
using Sdp.Charts.Gantt;

namespace Sdp
{
	// Token: 0x0200024B RID: 587
	public class AnnotationDialogResponseArgs : EventArgs
	{
		// Token: 0x04000829 RID: 2089
		public Element Selected;

		// Token: 0x0400082A RID: 2090
		public Series ElementSeries;

		// Token: 0x0400082B RID: 2091
		public string Annotation;

		// Token: 0x0400082C RID: 2092
		public bool ResponseOk;
	}
}
