using System;

namespace Sdp
{
	// Token: 0x020000FA RID: 250
	public class ResourceViewEvents
	{
		// Token: 0x06000393 RID: 915 RVA: 0x00009BD8 File Offset: 0x00007DD8
		public ResourceViewEvents()
		{
			this.ViewSourceAdded = (EventHandler<ViewSourceAddedEventArgs>)Delegate.Combine(this.ViewSourceAdded, new EventHandler<ViewSourceAddedEventArgs>(this.SetVisible));
			this.SelectSource = (EventHandler<SourceEventArgs>)Delegate.Combine(this.SelectSource, new EventHandler<SourceEventArgs>(this.SetVisible));
			this.Invalidate = (EventHandler<ResourceViewInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<ResourceViewInvalidateEventArgs>(this.SetVisible));
			this.SelectItem = (EventHandler<ItemSelectedEventArgs>)Delegate.Combine(this.SelectItem, new EventHandler<ItemSelectedEventArgs>(this.SetVisible));
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00009C73 File Offset: 0x00007E73
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("ResourcesView", null, false, false);
		}

		// Token: 0x04000376 RID: 886
		public EventHandler<ViewSourceAddedEventArgs> ViewSourceAdded;

		// Token: 0x04000377 RID: 887
		public EventHandler<SourceEventArgs> SourceSelected;

		// Token: 0x04000378 RID: 888
		public EventHandler<ResourceViewInvalidateEventArgs> Invalidate;

		// Token: 0x04000379 RID: 889
		public EventHandler<EventArgs> Clear;

		// Token: 0x0400037A RID: 890
		public EventHandler<EventArgs> ClearFilter;

		// Token: 0x0400037B RID: 891
		public EventHandler<ClearCategoryEventArgs> ClearCategory;

		// Token: 0x0400037C RID: 892
		public EventHandler<AddCategoryArgs> AddCategory;

		// Token: 0x0400037D RID: 893
		public EventHandler<AddFormatFilterArgs> AddFilterFormat;

		// Token: 0x0400037E RID: 894
		public EventHandler<PrepopulateCategoryArgs> PrepopulateCategory;

		// Token: 0x0400037F RID: 895
		public EventHandler<AddResourceArgs> AddResource;

		// Token: 0x04000380 RID: 896
		public EventHandler<UpdateResourcePixBufArgs> UpdateResourcePixBuf;

		// Token: 0x04000381 RID: 897
		public EventHandler<UpdateResourceCustomFilterDataArgs> UpdateResourceCustomFilterData;

		// Token: 0x04000382 RID: 898
		public EventHandler<UpdateResourceNameArgs> UpdateResourceName;

		// Token: 0x04000383 RID: 899
		public EventHandler<UpdateCategoryNumVisible> UpdateCategoryNumVisible;

		// Token: 0x04000384 RID: 900
		public EventHandler<ItemSelectedEventArgs> ItemSelected;

		// Token: 0x04000385 RID: 901
		public EventHandler<ItemDoubleClickedEventArgs> ItemDoubleClicked;

		// Token: 0x04000386 RID: 902
		public EventHandler<SourceEventArgs> SelectSource;

		// Token: 0x04000387 RID: 903
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x04000388 RID: 904
		public EventHandler<EventArgs> HideStatus;

		// Token: 0x04000389 RID: 905
		public EventHandler<DisableEventArgs> DisableResourceView;

		// Token: 0x0400038A RID: 906
		public EventHandler<AddFilterEventArgs> AddFilter;

		// Token: 0x0400038B RID: 907
		public EventHandler<SetActiveToolItemInFilterEventArgs> ResetFilterToAll;

		// Token: 0x0400038C RID: 908
		public EventHandler<UpdateHeaderEventArgs> UpdateHeader;

		// Token: 0x0400038D RID: 909
		public EventHandler<UpdateCostBarArgs> UpdateCostBar;

		// Token: 0x0400038E RID: 910
		public EventHandler<AddSortComboArgs> AddSortComboBox;

		// Token: 0x0400038F RID: 911
		public EventHandler<ItemSelectedEventArgs> SelectItem;

		// Token: 0x04000390 RID: 912
		public const string RESOURCES_VIEW_TYPENAME = "ResourcesView";
	}
}
