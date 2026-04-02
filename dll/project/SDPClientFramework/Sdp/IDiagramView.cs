using System;
using Cairo;

namespace Sdp
{
	// Token: 0x020001C5 RID: 453
	public interface IDiagramView
	{
		// Token: 0x1700011D RID: 285
		// (set) Token: 0x060005D7 RID: 1495
		object DiagramData { set; }

		// Token: 0x060005D8 RID: 1496
		void Invalidate();

		// Token: 0x060005D9 RID: 1497
		void Draw(string title, Context ct, double left, double right, double top, double bottom, SortedBlockInfoSet blocks);
	}
}
