using System;

namespace Sdp
{
	// Token: 0x020001C2 RID: 450
	public class PieController : DiagramControllerBase
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x0000D7FE File Offset: 0x0000B9FE
		public PieController(IDiagramView view, GroupLayoutController layoutContainer, GroupController groupContainer)
			: base(view, layoutContainer, groupContainer)
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000D809 File Offset: 0x0000BA09
		public override void Invalidate()
		{
			this.m_viewContainer.Invalidate();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000D816 File Offset: 0x0000BA16
		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
