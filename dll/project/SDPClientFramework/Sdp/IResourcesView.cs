using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002BF RID: 703
	public interface IResourcesView : IView, IStatusBox
	{
		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06000E57 RID: 3671
		// (remove) Token: 0x06000E58 RID: 3672
		event EventHandler SourceSelectedChanged;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06000E59 RID: 3673
		// (remove) Token: 0x06000E5A RID: 3674
		event EventHandler<ResourceSelectedEventArgs> ResourceSelected;

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06000E5B RID: 3675
		// (remove) Token: 0x06000E5C RID: 3676
		event EventHandler<ResourceDoubleClickedEventArgs> ResourceDoubleClicked;

		// Token: 0x06000E5D RID: 3677
		void AddSource(int sourceId, int captureId, string name);

		// Token: 0x06000E5E RID: 3678
		bool GetSource(out int sourceId, out int captureId);

		// Token: 0x06000E5F RID: 3679
		void SetSource(int sourceId, int captureId);

		// Token: 0x06000E60 RID: 3680
		void AddComboFilter(string filterName, int filterColumn, SearchString[] searchStrings, SearchMode searchMode, EventHandler<ComboBoxSelectionChangedArgs> onSelectionChangedHandler);

		// Token: 0x06000E61 RID: 3681
		void AddCheckBoxFilter(string filterName, int filterColumn, string label, string tooltip, EventHandler<CheckBoxToggledArgs> onToggleEventHandler);

		// Token: 0x06000E62 RID: 3682
		void AddRadioButtonFilter(string filterName, int filterColumn, int selectedIndex, string[] labels, string[] tooltip, EventHandler<RadioButtonToggledArgs> onToggleEventHandler);

		// Token: 0x06000E63 RID: 3683
		void ResetFilterToAll(string filterName, string toolItemName);

		// Token: 0x06000E64 RID: 3684
		void Clear();

		// Token: 0x06000E65 RID: 3685
		void ClearFilter();

		// Token: 0x06000E66 RID: 3686
		void ClearCategory(int categoryID);

		// Token: 0x06000E67 RID: 3687
		void AddResource(int categoryId, long resourceId, string resourceName, string resourceTooltip, byte[] data, List<ResourceItem> children, object[] customFilterObjects);

		// Token: 0x06000E68 RID: 3688
		void UpdateResourceCustomFilterData(int categoryId, long resourceId, object customFilterData, int customFilterColumn);

		// Token: 0x06000E69 RID: 3689
		void UpdateResourceCustomFilterData(int categoryId, int column, Dictionary<long, object> resourceItems);

		// Token: 0x06000E6A RID: 3690
		void UpdateResourceThumbnail(int categoryId, Dictionary<long, byte[]> resourceItems);

		// Token: 0x06000E6B RID: 3691
		void UpdateResourceName(int categoryId, Dictionary<long, string> resourceItems);

		// Token: 0x06000E6C RID: 3692
		void UpdateCostBar(Dictionary<int, ResourceCostDict> categories, int column);

		// Token: 0x06000E6D RID: 3693
		void UpdateVisibleItemsLabel(int categoryId, int total, int visible);

		// Token: 0x06000E6E RID: 3694
		void AddResourcesCategory(int id, string categoryName, ResourcesCategoryStyle style, Type[] customFilterColumns, IReadOnlyList<ResourceItem> resourceItems, int numOfCostMetrics);

		// Token: 0x06000E6F RID: 3695
		void Disable(bool disable);

		// Token: 0x06000E70 RID: 3696
		void UpdateHeader(string markup);

		// Token: 0x06000E71 RID: 3697
		void AddSortCombo(SortItem[] items);

		// Token: 0x06000E72 RID: 3698
		void CaptureNameChanged();

		// Token: 0x06000E73 RID: 3699
		void SelectItem(int categoryId, long[] resourceIds);
	}
}
