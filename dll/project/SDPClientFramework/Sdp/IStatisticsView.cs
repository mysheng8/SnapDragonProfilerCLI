using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001E0 RID: 480
	public interface IStatisticsView : IDialog
	{
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x0600068E RID: 1678
		// (remove) Token: 0x0600068F RID: 1679
		event EventHandler SelectedCaptureChanged;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000690 RID: 1680
		// (remove) Token: 0x06000691 RID: 1681
		event EventHandler SelectedStatisticChanged;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000692 RID: 1682
		// (remove) Token: 0x06000693 RID: 1683
		event EventHandler ShowAllToggled;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000694 RID: 1684
		// (remove) Token: 0x06000695 RID: 1685
		event EventHandler<ViewBoundsEventArgs> ZoomIn;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000696 RID: 1686
		// (remove) Token: 0x06000697 RID: 1687
		event EventHandler<ViewBoundsEventArgs> ZoomOut;

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000698 RID: 1688
		// (remove) Token: 0x06000699 RID: 1689
		event EventHandler ResetViewBounds;

		// Token: 0x0600069A RID: 1690
		void AddCapture(int captureID, CaptureType captureType);

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600069B RID: 1691
		int SelectedCaptureID { get; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600069C RID: 1692
		IStatistic SelectedStatistic { get; }

		// Token: 0x0600069D RID: 1693
		void InvalidateStatisticsList(Dictionary<IStatistic, StatisticState> statistics, IStatistic selectedStatistic);

		// Token: 0x0600069E RID: 1694
		void ClearOutputArea();

		// Token: 0x0600069F RID: 1695
		void InvalidateOutputArea(IStatistic stat, IStatisticDisplayViewModel[] statVM);

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006A0 RID: 1696
		// (set) Token: 0x060006A1 RID: 1697
		DataPage SelectedDataPage { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006A2 RID: 1698
		bool ShowAll { get; }

		// Token: 0x1700013F RID: 319
		// (set) Token: 0x060006A3 RID: 1699
		bool StatisticsListSensitive { set; }

		// Token: 0x060006A4 RID: 1700
		void SetViewBounds(int zoom);
	}
}
