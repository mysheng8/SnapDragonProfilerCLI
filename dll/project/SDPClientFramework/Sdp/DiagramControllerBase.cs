using System;

namespace Sdp
{
	// Token: 0x020001C0 RID: 448
	public abstract class DiagramControllerBase : IDisposable
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0000D81E File Offset: 0x0000BA1E
		public DiagramControllerBase(IDiagramView view, GroupLayoutController layoutContainer, GroupController groupContainer)
		{
			this.m_viewContainer = view;
			this.m_layoutContainer = layoutContainer;
			this.m_groupContainer = groupContainer;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00008AEF File Offset: 0x00006CEF
		public virtual void Dispose()
		{
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00008AEF File Offset: 0x00006CEF
		public virtual void Invalidate()
		{
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0000D83B File Offset: 0x0000BA3B
		public uint CaptureId
		{
			get
			{
				return this.m_layoutContainer.CaptureId;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0000D848 File Offset: 0x0000BA48
		public IDiagramView View
		{
			get
			{
				return this.m_viewContainer;
			}
		}

		// Token: 0x0400067E RID: 1662
		protected IDiagramView m_viewContainer;

		// Token: 0x0400067F RID: 1663
		protected GroupLayoutController m_layoutContainer;

		// Token: 0x04000680 RID: 1664
		protected GroupController m_groupContainer;
	}
}
