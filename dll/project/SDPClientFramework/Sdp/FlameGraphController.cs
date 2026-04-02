using System;

namespace Sdp
{
	// Token: 0x020001C1 RID: 449
	public class FlameGraphController
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x0000D850 File Offset: 0x0000BA50
		public FlameGraphController(IFlameGraphView view, SamplingController parent)
		{
			this.m_container = parent;
			this.m_view = view;
			this.m_view.NodeSelected += this.m_view_NodeSelected;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000D87D File Offset: 0x0000BA7D
		public void SearchNodes(string searchEntry)
		{
			this.m_view.SearchNodes(searchEntry.ToLower());
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000D890 File Offset: 0x0000BA90
		public void ChangeColorScheme(int active)
		{
			this.m_view.ChangeColorScheme(active);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000D89E File Offset: 0x0000BA9E
		public void ZoomIn()
		{
			this.m_view.ZoomIn();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0000D8AB File Offset: 0x0000BAAB
		public void ZoomOut()
		{
			this.m_view.ZoomOut();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		public void ResetZoom()
		{
			this.m_view.ResetZoom();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0000D8C5 File Offset: 0x0000BAC5
		public void SelectNode(ulong stackID)
		{
			this.m_view.SelectNode(stackID);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0000D8D3 File Offset: 0x0000BAD3
		private void m_view_NodeSelected(object sender, NodeSelectedEventArgs args)
		{
			SdpApp.EventsManager.Raise<NodeSelectedEventArgs>(SdpApp.EventsManager.SamplingEvents.NodeSelected, this, new NodeSelectedEventArgs(args.StackID, this.m_container.CaptureId));
		}

		// Token: 0x04000681 RID: 1665
		private IFlameGraphView m_view;

		// Token: 0x04000682 RID: 1666
		private SamplingController m_container;
	}
}
