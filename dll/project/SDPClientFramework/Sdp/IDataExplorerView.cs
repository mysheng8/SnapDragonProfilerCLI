using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002B9 RID: 697
	public interface IDataExplorerView : IView, IStatusBox
	{
		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06000E0F RID: 3599
		// (remove) Token: 0x06000E10 RID: 3600
		event EventHandler SourceSelectedChanged;

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06000E11 RID: 3601
		// (remove) Token: 0x06000E12 RID: 3602
		event EventHandler<RowSelectedEventArgs> RowSelected;

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06000E13 RID: 3603
		// (remove) Token: 0x06000E14 RID: 3604
		event EventHandler<EntryStateChangedEventArgs> EntryStateChanged;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06000E15 RID: 3605
		// (remove) Token: 0x06000E16 RID: 3606
		event EventHandler<ComboOptionSelectedEventArgs> ComboOptionSelected;

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06000E17 RID: 3607
		// (remove) Token: 0x06000E18 RID: 3608
		event EventHandler<CustomComboSelectedEventArgs> CustomComboOptionSelected;

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06000E19 RID: 3609
		// (remove) Token: 0x06000E1A RID: 3610
		event EventHandler<ColumnToggledEventArgs> ColumnToggled;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06000E1B RID: 3611
		// (remove) Token: 0x06000E1C RID: 3612
		event EventHandler<HighlightToggledEventArgs> HighlightToggled;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06000E1D RID: 3613
		// (remove) Token: 0x06000E1E RID: 3614
		event EventHandler<RowExpandAndCollapseEventArgs> RowExpanded;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06000E1F RID: 3615
		// (remove) Token: 0x06000E20 RID: 3616
		event EventHandler<RowExpandAndCollapseEventArgs> RowCollapsed;

		// Token: 0x06000E21 RID: 3617
		void Clear();

		// Token: 0x06000E22 RID: 3618
		void Invalidate(TreeModel model, IEnumerable<DataExplorerViewFilter> filters);

		// Token: 0x170002B5 RID: 693
		// (set) Token: 0x06000E23 RID: 3619
		int ExpanderColumnIndex { set; }

		// Token: 0x06000E24 RID: 3620
		bool GetSelected(out int sourceId, out int captureId, out string label);

		// Token: 0x06000E25 RID: 3621
		void SetSelected(int sourceId, int captureId);

		// Token: 0x06000E26 RID: 3622
		void Reset();

		// Token: 0x06000E27 RID: 3623
		void SetSourceVisibility(string label, int sourceId, int captureId, bool visible);

		// Token: 0x06000E28 RID: 3624
		void Select(object rowElement, int searchColumn, bool expand);

		// Token: 0x06000E29 RID: 3625
		void SetHighlightedElements(List<object> HighlightElements, int searchColumn);

		// Token: 0x06000E2A RID: 3626
		void AddPixbufColumn(DataExplorerViewColumn column);

		// Token: 0x06000E2B RID: 3627
		void AddTextColumn(DataExplorerViewColumn column);

		// Token: 0x06000E2C RID: 3628
		void Refilter(Dictionary<int, string> comboFilters, Dictionary<int, string> entryFilters, bool highlightActive, bool drawcallsActive, Dictionary<int, List<string>> expandedRowsList);

		// Token: 0x06000E2D RID: 3629
		void Disable(bool disable);

		// Token: 0x06000E2E RID: 3630
		void DisableHighlightButton(bool disable);

		// Token: 0x06000E2F RID: 3631
		void InitializeUserSelectableMetrics(InitializeTreeComboArgs args);

		// Token: 0x06000E30 RID: 3632
		void CaptureNameChanged();

		// Token: 0x06000E31 RID: 3633
		void SetExpandedList(Dictionary<int, List<string>> expandedRowsList);

		// Token: 0x06000E32 RID: 3634
		void ShowShaderProfiling();

		// Token: 0x06000E33 RID: 3635
		void UpdateSingleRow(object[] row);

		// Token: 0x06000E34 RID: 3636
		void UpdateColumnVisibilty(uint column, bool enabled);
	}
}
