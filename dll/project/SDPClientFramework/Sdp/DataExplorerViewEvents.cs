using System;

namespace Sdp
{
	// Token: 0x0200013F RID: 319
	public class DataExplorerViewEvents
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x0000A724 File Offset: 0x00008924
		public DataExplorerViewEvents()
		{
			this.ViewSourceAdded = (EventHandler<ViewSourceAddedEventArgs>)Delegate.Combine(this.ViewSourceAdded, new EventHandler<ViewSourceAddedEventArgs>(this.SetVisible));
			this.SelectSource = (EventHandler<SourceEventArgs>)Delegate.Combine(this.SelectSource, new EventHandler<SourceEventArgs>(this.SetVisible));
			this.Invalidate = (EventHandler<DataExplorerViewInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<DataExplorerViewInvalidateEventArgs>(this.SetVisible));
			this.SelectRow = (EventHandler<DataExplorerViewSelectRowEventArgs>)Delegate.Combine(this.SelectRow, new EventHandler<DataExplorerViewSelectRowEventArgs>(this.SetVisible));
			this.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(this.SetStatus, new EventHandler<SetStatusEventArgs>(this.SetVisible));
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000A7E4 File Offset: 0x000089E4
		private void SetVisible(object o, EventArgs e)
		{
			MultiViewArgs multiViewArgs = e as MultiViewArgs;
			if (multiViewArgs != null)
			{
				SdpApp.UIManager.PresentView("DataExplorerView", multiViewArgs.UniqueID, false, false);
			}
		}

		// Token: 0x04000484 RID: 1156
		public EventHandler<ViewSourceAddedEventArgs> ViewSourceAdded;

		// Token: 0x04000485 RID: 1157
		public EventHandler<SourceEventArgs> SourceSelected;

		// Token: 0x04000486 RID: 1158
		public EventHandler<SourceEventArgs> SelectSource;

		// Token: 0x04000487 RID: 1159
		public EventHandler<DataExplorerViewInvalidateEventArgs> Invalidate;

		// Token: 0x04000488 RID: 1160
		public EventHandler<DataExplorerViewRowSelectedEventArgs> RowSelected;

		// Token: 0x04000489 RID: 1161
		public EventHandler<DataExplorerViewSelectRowEventArgs> SelectRow;

		// Token: 0x0400048A RID: 1162
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x0400048B RID: 1163
		public EventHandler<MultiViewArgs> HideStatus;

		// Token: 0x0400048C RID: 1164
		public EventHandler<ComboOptionSelectedEventArgs> ComboOptionSelected;

		// Token: 0x0400048D RID: 1165
		public EventHandler<CustomComboSelectedEventArgs> CustomComboOptionSelected;

		// Token: 0x0400048E RID: 1166
		public EventHandler<EntryStateChangedEventArgs> EntryStateChanged;

		// Token: 0x0400048F RID: 1167
		public EventHandler<InitializeTreeComboArgs> InitializeUserSelectableMetrics;

		// Token: 0x04000490 RID: 1168
		public EventHandler<ColumnToggledEventArgs> ColumnToggled;

		// Token: 0x04000491 RID: 1169
		public EventHandler<ColumnToggledEventArgs> ToggleColumn;

		// Token: 0x04000492 RID: 1170
		public EventHandler<HighlightToggledEventArgs> HighlightToggled;

		// Token: 0x04000493 RID: 1171
		public EventHandler<EventArgs> Refilter;

		// Token: 0x04000494 RID: 1172
		public EventHandler<SourceLoadedCompleteArgs> SourceLoadedComplete;

		// Token: 0x04000495 RID: 1173
		public EventHandler ShowShaderProfiling;

		// Token: 0x04000496 RID: 1174
		public EventHandler<UpdateSingleRowArgs> UpdateSingleRow;

		// Token: 0x04000497 RID: 1175
		public const string DATA_EXPLORER_TYPENAME = "DataExplorerView";

		// Token: 0x04000498 RID: 1176
		public const string DRAWCALLS_ONLY = "Show Only Drawcalls";

		// Token: 0x04000499 RID: 1177
		public const string DRAWCALLS_SHOW_ALL = "Show All";
	}
}
