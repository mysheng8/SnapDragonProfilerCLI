using System;

namespace Sdp
{
	// Token: 0x020002BE RID: 702
	public class ResourcesController : IViewController
	{
		// Token: 0x06000E3A RID: 3642 RVA: 0x0002BC74 File Offset: 0x00029E74
		public ResourcesController(IResourcesView view)
		{
			this.m_view = view;
			this.m_view.SourceSelectedChanged += this.m_view_SourceSelectedChanged;
			this.m_view.ResourceSelected += this.m_view_ResourceSelected;
			this.m_view.ResourceDoubleClicked += this.m_view_ResourceDoubleClicked;
			ResourceViewEvents resourceViewEvents = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents.ViewSourceAdded = (EventHandler<ViewSourceAddedEventArgs>)Delegate.Combine(resourceViewEvents.ViewSourceAdded, new EventHandler<ViewSourceAddedEventArgs>(this.resourcesViewModel_ViewSourceAdded));
			ResourceViewEvents resourceViewEvents2 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents2.Invalidate = (EventHandler<ResourceViewInvalidateEventArgs>)Delegate.Combine(resourceViewEvents2.Invalidate, new EventHandler<ResourceViewInvalidateEventArgs>(this.resourceViewEvents_Invalidate));
			ResourceViewEvents resourceViewEvents3 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents3.AddCategory = (EventHandler<AddCategoryArgs>)Delegate.Combine(resourceViewEvents3.AddCategory, new EventHandler<AddCategoryArgs>(this.resourceViewEvents_AddCategory));
			ResourceViewEvents resourceViewEvents4 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents4.AddResource = (EventHandler<AddResourceArgs>)Delegate.Combine(resourceViewEvents4.AddResource, new EventHandler<AddResourceArgs>(this.resourceViewEvents_AddResource));
			ResourceViewEvents resourceViewEvents5 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents5.UpdateResourcePixBuf = (EventHandler<UpdateResourcePixBufArgs>)Delegate.Combine(resourceViewEvents5.UpdateResourcePixBuf, new EventHandler<UpdateResourcePixBufArgs>(this.resourceViewEvents_UpdateResourcePixBuf));
			ResourceViewEvents resourceViewEvents6 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents6.UpdateResourceCustomFilterData = (EventHandler<UpdateResourceCustomFilterDataArgs>)Delegate.Combine(resourceViewEvents6.UpdateResourceCustomFilterData, new EventHandler<UpdateResourceCustomFilterDataArgs>(this.resourceViewEvents_UpdateResourceCustomFilterData));
			ResourceViewEvents resourceViewEvents7 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents7.UpdateResourceName = (EventHandler<UpdateResourceNameArgs>)Delegate.Combine(resourceViewEvents7.UpdateResourceName, new EventHandler<UpdateResourceNameArgs>(this.resourceViewEvents_UpdateResourceName));
			ResourceViewEvents resourceViewEvents8 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents8.UpdateCategoryNumVisible = (EventHandler<UpdateCategoryNumVisible>)Delegate.Combine(resourceViewEvents8.UpdateCategoryNumVisible, new EventHandler<UpdateCategoryNumVisible>(this.resourceViewEvents_UpdateCategoryNumVisible));
			ResourceViewEvents resourceViewEvents9 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents9.Clear = (EventHandler<EventArgs>)Delegate.Combine(resourceViewEvents9.Clear, new EventHandler<EventArgs>(this.resourceViewEvents_Clear));
			ResourceViewEvents resourceViewEvents10 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents10.ClearFilter = (EventHandler<EventArgs>)Delegate.Combine(resourceViewEvents10.ClearFilter, new EventHandler<EventArgs>(this.resourceViewEvents_ClearFilter));
			ResourceViewEvents resourceViewEvents11 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents11.ClearCategory = (EventHandler<ClearCategoryEventArgs>)Delegate.Combine(resourceViewEvents11.ClearCategory, new EventHandler<ClearCategoryEventArgs>(this.resourceViewEvents_ClearCategory));
			ResourceViewEvents resourceViewEvents12 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents12.SelectSource = (EventHandler<SourceEventArgs>)Delegate.Combine(resourceViewEvents12.SelectSource, new EventHandler<SourceEventArgs>(this.resourceViewEvents_SelectSource));
			ResourceViewEvents resourceViewEvents13 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents13.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(resourceViewEvents13.SetStatus, new EventHandler<SetStatusEventArgs>(this.resourceViewEvents_SetStatus));
			ResourceViewEvents resourceViewEvents14 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents14.HideStatus = (EventHandler<EventArgs>)Delegate.Combine(resourceViewEvents14.HideStatus, new EventHandler<EventArgs>(this.resourceViewEvents_HideStatus));
			ResourceViewEvents resourceViewEvents15 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents15.DisableResourceView = (EventHandler<DisableEventArgs>)Delegate.Combine(resourceViewEvents15.DisableResourceView, new EventHandler<DisableEventArgs>(this.resourceViewEvent_Disable));
			ResourceViewEvents resourceViewEvents16 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents16.UpdateHeader = (EventHandler<UpdateHeaderEventArgs>)Delegate.Combine(resourceViewEvents16.UpdateHeader, new EventHandler<UpdateHeaderEventArgs>(this.resourcesViewEvents_UpdateHeader));
			ResourceViewEvents resourceViewEvents17 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents17.UpdateCostBar = (EventHandler<UpdateCostBarArgs>)Delegate.Combine(resourceViewEvents17.UpdateCostBar, new EventHandler<UpdateCostBarArgs>(this.ResourcesViewEvents_UpdateCostBar));
			ResourceViewEvents resourceViewEvents18 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents18.AddSortComboBox = (EventHandler<AddSortComboArgs>)Delegate.Combine(resourceViewEvents18.AddSortComboBox, new EventHandler<AddSortComboArgs>(this.ResourcesViewEvents_AddComboBox));
			ResourceViewEvents resourceViewEvents19 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents19.SelectItem = (EventHandler<ItemSelectedEventArgs>)Delegate.Combine(resourceViewEvents19.SelectItem, new EventHandler<ItemSelectedEventArgs>(this.ResourcesViewEvents_SelectItem));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.CaptureNameChanged = (EventHandler<CaptureNameChangedArgs>)Delegate.Combine(clientEvents.CaptureNameChanged, new EventHandler<CaptureNameChangedArgs>(this.clientEvents_CaptureNameChanged));
			foreach (ViewSource viewSource in SdpApp.ModelManager.ResourcesViewModel.ViewSources)
			{
				this.m_view.AddSource(viewSource.SourceID, viewSource.CaptureID, viewSource.SourceName);
			}
			if (SdpApp.ModelManager.ResourcesViewModel.ViewSources.Count > 0)
			{
				ViewSource viewSource2 = SdpApp.ModelManager.ResourcesViewModel.ViewSources[SdpApp.ModelManager.ResourcesViewModel.ViewSources.Count - 1];
				this.m_currentSourceID = viewSource2.SourceID;
				this.m_currentCaptureID = viewSource2.CaptureID;
				this.m_view.SetSource(viewSource2.SourceID, viewSource2.CaptureID);
			}
			ResourceViewEvents resourceViewEvents20 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents20.AddFilter = (EventHandler<AddFilterEventArgs>)Delegate.Combine(resourceViewEvents20.AddFilter, new EventHandler<AddFilterEventArgs>(this.resourceViewEvents_AddFilter));
			ResourceViewEvents resourceViewEvents21 = SdpApp.EventsManager.ResourceViewEvents;
			resourceViewEvents21.ResetFilterToAll = (EventHandler<SetActiveToolItemInFilterEventArgs>)Delegate.Combine(resourceViewEvents21.ResetFilterToAll, new EventHandler<SetActiveToolItemInFilterEventArgs>(this.resourceViewEvents_ResetFilterToAll));
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0002C15C File Offset: 0x0002A35C
		private void m_view_SourceSelectedChanged(object sender, EventArgs e)
		{
			if (this.m_view.GetSource(out this.m_currentSourceID, out this.m_currentCaptureID))
			{
				SourceEventArgs sourceEventArgs = new SourceEventArgs();
				sourceEventArgs.SourceID = this.m_currentSourceID;
				sourceEventArgs.CaptureID = this.m_currentCaptureID;
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.ResourceViewEvents.SourceSelected, this, sourceEventArgs);
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002C1BC File Offset: 0x0002A3BC
		private void m_view_ResourceSelected(object sender, ResourceSelectedEventArgs e)
		{
			ItemSelectedEventArgs itemSelectedEventArgs = new ItemSelectedEventArgs();
			itemSelectedEventArgs.SourceID = this.m_currentSourceID;
			itemSelectedEventArgs.CaptureID = this.m_currentCaptureID;
			itemSelectedEventArgs.CategoryID = e.CategoryId;
			itemSelectedEventArgs.ResourceIDs = e.ResourceIds;
			SdpApp.EventsManager.Raise<ItemSelectedEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ItemSelected, this, itemSelectedEventArgs);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0002C21C File Offset: 0x0002A41C
		private void m_view_ResourceDoubleClicked(object sender, ResourceDoubleClickedEventArgs e)
		{
			ItemDoubleClickedEventArgs itemDoubleClickedEventArgs = new ItemDoubleClickedEventArgs();
			itemDoubleClickedEventArgs.SourceID = this.m_currentSourceID;
			itemDoubleClickedEventArgs.CaptureID = this.m_currentCaptureID;
			itemDoubleClickedEventArgs.CategoryID = e.CategoryId;
			itemDoubleClickedEventArgs.ResourceID = e.ResourceId;
			SdpApp.EventsManager.Raise<ItemDoubleClickedEventArgs>(SdpApp.EventsManager.ResourceViewEvents.ItemDoubleClicked, this, itemDoubleClickedEventArgs);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0002C27C File Offset: 0x0002A47C
		private void resourcesViewModel_ViewSourceAdded(object sender, ViewSourceAddedEventArgs e)
		{
			this.m_view.AddSource(e.ViewSource.SourceID, e.ViewSource.CaptureID, e.ViewSource.SourceName);
			this.m_currentSourceID = e.ViewSource.SourceID;
			this.m_currentCaptureID = e.ViewSource.CaptureID;
			this.m_view.SetSource(e.ViewSource.SourceID, e.ViewSource.CaptureID);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0002C2F8 File Offset: 0x0002A4F8
		private void resourceViewEvents_AddFilter(object sender, AddFilterEventArgs e)
		{
			if (e.Filter is EntryViewComboFilter)
			{
				EntryViewComboFilter entryViewComboFilter = e.Filter as EntryViewComboFilter;
				this.m_view.AddComboFilter(entryViewComboFilter.FilterName, entryViewComboFilter.FilterColumn, entryViewComboFilter.SearchStrings, entryViewComboFilter.SearchMode, entryViewComboFilter.OnSelectionChanged);
				return;
			}
			if (e.Filter is EntryViewCheckBoxFilter)
			{
				EntryViewCheckBoxFilter entryViewCheckBoxFilter = e.Filter as EntryViewCheckBoxFilter;
				this.m_view.AddCheckBoxFilter(entryViewCheckBoxFilter.FilterName, entryViewCheckBoxFilter.FilterColumn, entryViewCheckBoxFilter.Label, entryViewCheckBoxFilter.ToolTip, entryViewCheckBoxFilter.OnToggle);
				return;
			}
			if (e.Filter is EntryViewRadioButtonFilter)
			{
				EntryViewRadioButtonFilter entryViewRadioButtonFilter = e.Filter as EntryViewRadioButtonFilter;
				this.m_view.AddRadioButtonFilter(entryViewRadioButtonFilter.FilterName, entryViewRadioButtonFilter.FilterColumn, entryViewRadioButtonFilter.SelectedIndex, entryViewRadioButtonFilter.Labels, entryViewRadioButtonFilter.ToolTips, entryViewRadioButtonFilter.OnToggle);
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0002C3D3 File Offset: 0x0002A5D3
		private void resourceViewEvents_ResetFilterToAll(object sender, SetActiveToolItemInFilterEventArgs e)
		{
			this.m_view.ResetFilterToAll(e.FilterName, e.ToolItemName);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0002C3EC File Offset: 0x0002A5EC
		private void resourceViewEvents_Invalidate(object sender, ResourceViewInvalidateEventArgs e)
		{
			this.m_view.Clear();
			foreach (Category category in e.Categories)
			{
				this.m_view.AddResourcesCategory(category.Id, category.Name, category.Style, category.CustomFilterColumns, category.ResourceItems, 0);
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0002C470 File Offset: 0x0002A670
		private void resourceViewEvents_AddCategory(object sender, AddCategoryArgs e)
		{
			this.m_view.AddResourcesCategory(e.ID, e.Name, e.Style, e.CustomFilterColumns, e.ResourceItems, e.CostBar);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0002C4A1 File Offset: 0x0002A6A1
		private void resourceViewEvents_AddResource(object sender, AddResourceArgs e)
		{
			this.m_view.AddResource(e.CategoryID, e.Id, e.Name, e.Tooltip, e.Data, e.Children, e.CustomFilterObjects);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0002C4D8 File Offset: 0x0002A6D8
		private void resourceViewEvents_UpdateResourcePixBuf(object sender, UpdateResourcePixBufArgs e)
		{
			this.m_view.UpdateResourceThumbnail(e.CategoryID, e.Items);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0002C4F4 File Offset: 0x0002A6F4
		private void resourceViewEvents_UpdateResourceCustomFilterData(object sender, UpdateResourceCustomFilterDataArgs e)
		{
			if (e.Items.Count > 0)
			{
				this.m_view.UpdateResourceCustomFilterData(e.CategoryID, e.Column, e.Items);
				return;
			}
			this.m_view.UpdateResourceCustomFilterData(e.CategoryID, e.Id, e.Data, e.Column);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0002C550 File Offset: 0x0002A750
		private void resourceViewEvents_UpdateResourceName(object sender, UpdateResourceNameArgs e)
		{
			this.m_view.UpdateResourceName(e.CategoryID, e.Items);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002C569 File Offset: 0x0002A769
		private void resourceViewEvents_UpdateCategoryNumVisible(object sender, UpdateCategoryNumVisible e)
		{
			this.m_view.UpdateVisibleItemsLabel(e.CategoryID, e.Total, e.Visible);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0002C588 File Offset: 0x0002A788
		private void resourceViewEvents_Clear(object sender, EventArgs e)
		{
			this.m_view.Clear();
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0002C595 File Offset: 0x0002A795
		private void resourceViewEvents_ClearFilter(object sender, EventArgs e)
		{
			this.m_view.ClearFilter();
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0002C5A2 File Offset: 0x0002A7A2
		private void resourceViewEvents_ClearCategory(object sender, ClearCategoryEventArgs e)
		{
			this.m_view.ClearCategory(e.Id);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002C5B5 File Offset: 0x0002A7B5
		private void resourceViewEvents_SelectSource(object sender, SourceEventArgs e)
		{
			this.m_view.SetSource(e.SourceID, e.CaptureID);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002C5CE File Offset: 0x0002A7CE
		private void resourceViewEvents_SetStatus(object sender, SetStatusEventArgs e)
		{
			this.m_view.SetStatus(e.Status, e.StatusText, e.Duration, false, null);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002C5EF File Offset: 0x0002A7EF
		private void resourceViewEvents_HideStatus(object sender, EventArgs e)
		{
			this.m_view.HideStatus();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0002C5FC File Offset: 0x0002A7FC
		private void resourceViewEvent_Disable(object sender, DisableEventArgs e)
		{
			this.m_view.Disable(e.Disable);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002C60F File Offset: 0x0002A80F
		private void resourcesViewEvents_UpdateHeader(object sender, UpdateHeaderEventArgs e)
		{
			this.m_view.UpdateHeader(e.Markup);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002C624 File Offset: 0x0002A824
		private void ResourcesViewEvents_UpdateCostBar(object sender, UpdateCostBarArgs e)
		{
			foreach (MetricsCost metricsCost in e.CostBarArgsList)
			{
				this.m_view.UpdateCostBar(metricsCost.Items, metricsCost.Column);
			}
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0002C688 File Offset: 0x0002A888
		private void clientEvents_CaptureNameChanged(object sender, EventArgs e)
		{
			this.m_view.CaptureNameChanged();
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002C695 File Offset: 0x0002A895
		private void ResourcesViewEvents_AddComboBox(object sender, AddSortComboArgs e)
		{
			this.m_view.AddSortCombo(e.Items);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002C6A8 File Offset: 0x0002A8A8
		private void ResourcesViewEvents_SelectItem(object sender, ItemSelectedEventArgs e)
		{
			if (e.ResourceIDs != null)
			{
				this.m_view.SelectItem(e.CategoryID, e.ResourceIDs);
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0002C6CC File Offset: 0x0002A8CC
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0002C6FB File Offset: 0x0002A8FB
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x040009A4 RID: 2468
		private int m_currentSourceID;

		// Token: 0x040009A5 RID: 2469
		private int m_currentCaptureID;

		// Token: 0x040009A6 RID: 2470
		private IResourcesView m_view;
	}
}
