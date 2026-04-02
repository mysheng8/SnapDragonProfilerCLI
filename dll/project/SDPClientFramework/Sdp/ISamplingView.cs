using System;

namespace Sdp
{
	// Token: 0x020001E7 RID: 487
	public interface ISamplingView : IView
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060006C1 RID: 1729
		// (remove) Token: 0x060006C2 RID: 1730
		event EventHandler CaptureButtonToggled;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060006C3 RID: 1731
		// (remove) Token: 0x060006C4 RID: 1732
		event EventHandler<ViewBoundsEventArgs> ZoomIn;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060006C5 RID: 1733
		// (remove) Token: 0x060006C6 RID: 1734
		event EventHandler<ViewBoundsEventArgs> ZoomOut;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060006C7 RID: 1735
		// (remove) Token: 0x060006C8 RID: 1736
		event EventHandler ResetViewBounds;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060006C9 RID: 1737
		// (remove) Token: 0x060006CA RID: 1738
		event EventHandler<SearchEntryChangedArgs> SearchEntryChanged;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060006CB RID: 1739
		// (remove) Token: 0x060006CC RID: 1740
		event EventHandler NewCaptureButtonClicked;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060006CD RID: 1741
		// (remove) Token: 0x060006CE RID: 1742
		event EventHandler<ColorSchemeChangedArgs> ColorSchemeChanged;

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006CF RID: 1743
		IDataSourcesView DataSourcesView { get; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006D0 RID: 1744
		// (set) Token: 0x060006D1 RID: 1745
		bool SearchEntryVisible { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006D2 RID: 1746
		// (set) Token: 0x060006D3 RID: 1747
		bool CaptureButtonActive { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006D4 RID: 1748
		// (set) Token: 0x060006D5 RID: 1749
		bool CaptureButtonEnabled { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006D6 RID: 1750
		// (set) Token: 0x060006D7 RID: 1751
		bool NewCaptureButtonVisible { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006D8 RID: 1752
		// (set) Token: 0x060006D9 RID: 1753
		bool ZoomButtonsVisible { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006DA RID: 1754
		// (set) Token: 0x060006DB RID: 1755
		bool ColorSchemeVisible { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006DC RID: 1756
		// (set) Token: 0x060006DD RID: 1757
		bool DataSourcesVisible { get; set; }

		// Token: 0x060006DE RID: 1758
		IFlameGraphView AddFlameGraph(TreeModel model, int maxDepth, string title, float childrenPercent);

		// Token: 0x060006DF RID: 1759
		void HideDataSourcesPanel();

		// Token: 0x060006E0 RID: 1760
		void ShowDataSourcesPanel();
	}
}
