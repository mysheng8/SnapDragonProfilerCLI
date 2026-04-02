using System;

namespace Sdp
{
	// Token: 0x020001BE RID: 446
	public class BlockFlowController : DiagramControllerBase
	{
		// Token: 0x060005BC RID: 1468 RVA: 0x0000D7FE File Offset: 0x0000B9FE
		public BlockFlowController(IDiagramView view, GroupLayoutController layoutContainer, GroupController groupContainer)
			: base(view, layoutContainer, groupContainer)
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0000D809 File Offset: 0x0000BA09
		public override void Invalidate()
		{
			this.m_viewContainer.Invalidate();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000D816 File Offset: 0x0000BA16
		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
