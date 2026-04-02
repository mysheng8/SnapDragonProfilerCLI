using System;
using System.Collections.Generic;
using Cairo;
using Sdp.Charts.Gantt;
using SDPClientFramework.Views.Flow.ViewModels.GanttTrack;

namespace Sdp
{
	// Token: 0x02000248 RID: 584
	public interface IGanttTrackView : ITrackViewBase
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06000971 RID: 2417
		// (remove) Token: 0x06000972 RID: 2418
		event EventHandler<SetDataViewBoundsEventArgs> DataViewBoundsChanged;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06000973 RID: 2419
		// (remove) Token: 0x06000974 RID: 2420
		event EventHandler<AnnotationDialogResponseArgs> AnnotationDialogResponse;

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000975 RID: 2421
		IDataViewMouseEventHandler DataViewMouseEventHandler { get; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000976 RID: 2422
		int DataViewHeight { get; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000977 RID: 2423
		int DataViewWidth { get; }

		// Token: 0x06000978 RID: 2424
		void Pan(int delta);

		// Token: 0x06000979 RID: 2425
		void SetZoomRange(int zoomRangeStart, int zoomRangeEnd);

		// Token: 0x0600097A RID: 2426
		void FocusOnElement(Element element);

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600097B RID: 2427
		// (set) Token: 0x0600097C RID: 2428
		Dictionary<uint, string> NameStringsModel { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600097D RID: 2429
		// (set) Token: 0x0600097E RID: 2430
		Dictionary<uint, string> TooltipStringsModel { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600097F RID: 2431
		// (set) Token: 0x06000980 RID: 2432
		HashSet<uint> StringIDsToRender { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000982 RID: 2434
		// (set) Token: 0x06000981 RID: 2433
		string SettingsWindowName { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000984 RID: 2436
		// (set) Token: 0x06000983 RID: 2435
		IReadOnlyGanttTrackViewModel ViewModel { get; set; }

		// Token: 0x06000985 RID: 2437
		void SetColorsModel(Dictionary<uint, Color> model);

		// Token: 0x06000986 RID: 2438
		Dictionary<uint, Color> GetColorsModel();

		// Token: 0x06000987 RID: 2439
		void Invalidate();

		// Token: 0x06000988 RID: 2440
		void AddConnection(Connection connection);

		// Token: 0x06000989 RID: 2441
		void SetDataBounds(long min, long max);

		// Token: 0x0600098A RID: 2442
		void SetDataViewBounds(double min, double max, bool dirty);

		// Token: 0x0600098B RID: 2443
		void OpenSettingsWindow();

		// Token: 0x0600098C RID: 2444
		void ShowAnnotateDialog(ElementSelectedEventArgs e, string existingText);
	}
}
