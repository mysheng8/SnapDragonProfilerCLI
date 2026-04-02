using System;

namespace Sdp
{
	// Token: 0x020001C3 RID: 451
	public interface IFlameGraphView
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060005CF RID: 1487
		// (remove) Token: 0x060005D0 RID: 1488
		event EventHandler<NodeSelectedEventArgs> NodeSelected;

		// Token: 0x060005D1 RID: 1489
		void SearchNodes(string text);

		// Token: 0x060005D2 RID: 1490
		void ChangeColorScheme(int active);

		// Token: 0x060005D3 RID: 1491
		void ZoomIn();

		// Token: 0x060005D4 RID: 1492
		void ZoomOut();

		// Token: 0x060005D5 RID: 1493
		void ResetZoom();

		// Token: 0x060005D6 RID: 1494
		void SelectNode(ulong stackID);
	}
}
