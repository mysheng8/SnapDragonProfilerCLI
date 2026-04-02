using System;
using System.Collections.Generic;
using Cairo;
using Sdp.Charts.Roofline;

namespace Sdp
{
	// Token: 0x020001CD RID: 461
	public interface IRooflineView : IView
	{
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600060E RID: 1550
		// (remove) Token: 0x0600060F RID: 1551
		event EventHandler<ColumnToggledEventArgs> KernelToggled;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000610 RID: 1552
		// (remove) Token: 0x06000611 RID: 1553
		event EventHandler<ViewBoundsEventArgs> ZoomIn;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000612 RID: 1554
		// (remove) Token: 0x06000613 RID: 1555
		event EventHandler<ViewBoundsEventArgs> ZoomOut;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000614 RID: 1556
		// (remove) Token: 0x06000615 RID: 1557
		event EventHandler ResetViewBounds;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000616 RID: 1558
		// (remove) Token: 0x06000617 RID: 1559
		event EventHandler<SetDataViewBoundsEventArgs> DataViewBoundsChanged;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000618 RID: 1560
		// (remove) Token: 0x06000619 RID: 1561
		event EventHandler<RooflineColorChangedEventArgs> ColorChanged;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x0600061A RID: 1562
		// (remove) Token: 0x0600061B RID: 1563
		event EventHandler<SelectAllEventArgs> SelectAllToggled;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x0600061C RID: 1564
		// (remove) Token: 0x0600061D RID: 1565
		event EventHandler<TreeFilteredEventArgs> TreeFilteredEvent;

		// Token: 0x0600061E RID: 1566
		void SetViewBounds(int zoom);

		// Token: 0x0600061F RID: 1567
		void AddSeries(List<Series> series, double minDist = 0.0, double maxDist = 0.0);

		// Token: 0x06000620 RID: 1568
		void SetSeriesVisibility(int id, bool visible);

		// Token: 0x06000621 RID: 1569
		void SetSeriesColor(int id, Color color);

		// Token: 0x06000622 RID: 1570
		void AddNodesToCategory(List<object[]> nodes, string category, uint capture, Color color, int id);

		// Token: 0x06000623 RID: 1571
		void UpdateTreeAndSeriesColor(int captureId, List<int> seriesIds, Color color);

		// Token: 0x06000624 RID: 1572
		void UpdateCaptureName(int captureId, string captureName);

		// Token: 0x06000625 RID: 1573
		void InvalidateStats(List<object[]> stats, string firstColumn, string secondColumn, string status);

		// Token: 0x06000626 RID: 1574
		void InvalidateAnalysis(List<object[]> analysis, string status);

		// Token: 0x06000627 RID: 1575
		void UpdateTickFrequency(double min, double max);

		// Token: 0x06000628 RID: 1576
		void SelectAllSeries(bool enabled, int exceptId = 0);
	}
}
