using System;
using System.Collections.Generic;

namespace Sdp.Charts.Gantt
{
	// Token: 0x020002F3 RID: 755
	public class Element : IComparable
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x0002FD9F File Offset: 0x0002DF9F
		public Element()
		{
			this.Separators = new List<ulong>();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002FDB4 File Offset: 0x0002DFB4
		public int CompareTo(object obj)
		{
			Element element = obj as Element;
			if ((element.xValue > (double)this.Start && element.xValue < (double)this.End) || (this.End == this.Start && Math.Abs(element.xValue - (double)this.Start) < 0.5))
			{
				return 0;
			}
			if (element.xValue <= (double)this.Start)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x04000A85 RID: 2693
		public uint BlockId;

		// Token: 0x04000A86 RID: 2694
		public long Start;

		// Token: 0x04000A87 RID: 2695
		public long End;

		// Token: 0x04000A88 RID: 2696
		public uint LabelId;

		// Token: 0x04000A89 RID: 2697
		public uint TooltipId;

		// Token: 0x04000A8A RID: 2698
		public uint ColorId;

		// Token: 0x04000A8B RID: 2699
		public uint InspectorStringId;

		// Token: 0x04000A8C RID: 2700
		public Element AnnotationLink;

		// Token: 0x04000A8D RID: 2701
		public List<ulong> Separators;

		// Token: 0x04000A8E RID: 2702
		public double xValue;
	}
}
