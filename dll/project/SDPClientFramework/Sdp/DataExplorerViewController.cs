using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002B8 RID: 696
	public class DataExplorerViewController : IViewController
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002AFAC File Offset: 0x000291AC
		public DataExplorerViewController(IDataExplorerView view, string uniqueId)
		{
			this.m_view = view;
			this.m_uniqueID = uniqueId;
			this.m_view.SourceSelectedChanged += this.m_view_SourceSelectedChanged;
			this.m_view.RowSelected += this.m_view_RowSelected;
			this.m_view.EntryStateChanged += this.m_view_EntryStateChanged;
			this.m_view.ComboOptionSelected += this.m_view_ComboOptionSelected;
			this.m_view.CustomComboOptionSelected += this.view_CustomComboSelected;
			this.m_view.ColumnToggled += this.m_view_ColumnToggled;
			this.m_view.HighlightToggled += this.m_view_HighlightToggled;
			this.m_view.RowExpanded += this.m_view_RowExpanded;
			this.m_view.RowCollapsed += this.m_view_RowCollapsed;
			DataExplorerViewEvents dataExplorerViewEvents = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents.ViewSourceAdded = (EventHandler<ViewSourceAddedEventArgs>)Delegate.Combine(dataExplorerViewEvents.ViewSourceAdded, new EventHandler<ViewSourceAddedEventArgs>(this.dataExplorerViewModel_ViewSourceAdded));
			DataExplorerViewEvents dataExplorerViewEvents2 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents2.Invalidate = (EventHandler<DataExplorerViewInvalidateEventArgs>)Delegate.Combine(dataExplorerViewEvents2.Invalidate, new EventHandler<DataExplorerViewInvalidateEventArgs>(this.dataExplorerViewEvents_Invalidate));
			DataExplorerViewEvents dataExplorerViewEvents3 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents3.SelectRow = (EventHandler<DataExplorerViewSelectRowEventArgs>)Delegate.Combine(dataExplorerViewEvents3.SelectRow, new EventHandler<DataExplorerViewSelectRowEventArgs>(this.dataExplorerViewEvents_SelectRow));
			DataExplorerViewEvents dataExplorerViewEvents4 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents4.SelectSource = (EventHandler<SourceEventArgs>)Delegate.Combine(dataExplorerViewEvents4.SelectSource, new EventHandler<SourceEventArgs>(this.dataExplorerViewEvents_SelectSource));
			DataExplorerViewEvents dataExplorerViewEvents5 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents5.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(dataExplorerViewEvents5.SetStatus, new EventHandler<SetStatusEventArgs>(this.dataExplorerViewEvents_SetStatus));
			DataExplorerViewEvents dataExplorerViewEvents6 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents6.HideStatus = (EventHandler<MultiViewArgs>)Delegate.Combine(dataExplorerViewEvents6.HideStatus, new EventHandler<MultiViewArgs>(this.dataExplorerViewEvents_HideStatus));
			DataExplorerViewEvents dataExplorerViewEvents7 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents7.InitializeUserSelectableMetrics = (EventHandler<InitializeTreeComboArgs>)Delegate.Combine(dataExplorerViewEvents7.InitializeUserSelectableMetrics, new EventHandler<InitializeTreeComboArgs>(this.dataExplorerViewEvents_InitializeUserSelectableMetrics));
			DataExplorerViewEvents dataExplorerViewEvents8 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents8.Refilter = (EventHandler<EventArgs>)Delegate.Combine(dataExplorerViewEvents8.Refilter, new EventHandler<EventArgs>(this.dataExplorerViewEvents_Refilter));
			DataExplorerViewEvents dataExplorerViewEvents9 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents9.ShowShaderProfiling = (EventHandler)Delegate.Combine(dataExplorerViewEvents9.ShowShaderProfiling, new EventHandler(this.dataExplorerViewEvents_ShowShaderProfiling));
			DataExplorerViewEvents dataExplorerViewEvents10 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents10.UpdateSingleRow = (EventHandler<UpdateSingleRowArgs>)Delegate.Combine(dataExplorerViewEvents10.UpdateSingleRow, new EventHandler<UpdateSingleRowArgs>(this.dataExplorerViewEvents_UpdateSingleRow));
			DataExplorerViewEvents dataExplorerViewEvents11 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents11.ToggleColumn = (EventHandler<ColumnToggledEventArgs>)Delegate.Combine(dataExplorerViewEvents11.ToggleColumn, new EventHandler<ColumnToggledEventArgs>(this.dataExplorerViewEvents_ToggleColumn));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.CaptureNameChanged = (EventHandler<CaptureNameChangedArgs>)Delegate.Combine(clientEvents.CaptureNameChanged, new EventHandler<CaptureNameChangedArgs>(this.clientEvents_CaptureNameChanged));
			ScreenCaptureViewEvents screenCaptureViewEvents = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents.DisableReplay = (EventHandler<DisableReplayEventArgs>)Delegate.Combine(screenCaptureViewEvents.DisableReplay, new EventHandler<DisableReplayEventArgs>(this.OnDisableReplay));
			ClientEvents clientEvents2 = SdpApp.EventsManager.ClientEvents;
			clientEvents2.AppShutdown = (EventHandler<EventArgs>)Delegate.Combine(clientEvents2.AppShutdown, new EventHandler<EventArgs>(this.OnShutdown));
			this.m_comboFilters = new Dictionary<int, string>();
			this.m_entryFilters = new Dictionary<int, string>();
			this.m_columns = new List<DataExplorerViewColumn>();
			this.m_expandedRowsList = new Dictionary<int, List<string>>();
			this.m_processes = new Dictionary<uint, string>();
			this.m_threads = new Dictionary<uint, string>();
			ViewSource viewSource = null;
			foreach (ViewSource viewSource2 in SdpApp.ModelManager.DataExplorerViewModel.ViewSources)
			{
				if (viewSource2.UniqueID == this.m_uniqueID)
				{
					this.m_view.SetSourceVisibility(viewSource2.SourceName, viewSource2.SourceID, viewSource2.CaptureID, true);
					viewSource = viewSource2;
				}
			}
			if (viewSource != null)
			{
				this.m_view.SetSelected(viewSource.SourceID, viewSource.CaptureID);
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002B3DC File Offset: 0x000295DC
		private void OnShutdown(object sender, EventArgs args)
		{
			this.m_view.Clear();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002B3EC File Offset: 0x000295EC
		private void m_view_RowSelected(object sender, RowSelectedEventArgs e)
		{
			if (!this.m_highlightActive && e.IsUserSelection && e.SelectedRow != null && e.SelectedRow.Length != 0)
			{
				this.m_view.SetHighlightedElements(new List<object> { e.SelectedRow[0] }, 0);
			}
			if (SdpApp.EventsManager.DataExplorerViewEvents.RowSelected != null)
			{
				DataExplorerViewRowSelectedEventArgs dataExplorerViewRowSelectedEventArgs = new DataExplorerViewRowSelectedEventArgs();
				dataExplorerViewRowSelectedEventArgs.SourceID = this.m_currentSourceID;
				dataExplorerViewRowSelectedEventArgs.CaptureID = this.m_currentCaptureID;
				dataExplorerViewRowSelectedEventArgs.SelectedRow = e.SelectedRow;
				dataExplorerViewRowSelectedEventArgs.NumClicks = e.numClicks;
				dataExplorerViewRowSelectedEventArgs.UniqueID = this.m_uniqueID;
				dataExplorerViewRowSelectedEventArgs.IsUserSelection = e.IsUserSelection;
				dataExplorerViewRowSelectedEventArgs.Columns = this.m_columns;
				dataExplorerViewRowSelectedEventArgs.ShaderProfilingEnabled = e.ShaderProfilingEnabled;
				SdpApp.EventsManager.DataExplorerViewEvents.RowSelected(this, dataExplorerViewRowSelectedEventArgs);
			}
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002B4C8 File Offset: 0x000296C8
		private void m_view_SourceSelectedChanged(object sender, EventArgs e)
		{
			int num;
			int num2;
			string text;
			if (this.m_view.GetSelected(out num, out num2, out text))
			{
				this.m_currentSourceID = num;
				this.m_currentCaptureID = num2;
				this.m_view.Clear();
				this.m_view.SetExpandedList(this.m_expandedRowsList);
				SourceEventArgs sourceEventArgs = new SourceEventArgs();
				sourceEventArgs.SourceID = this.m_currentSourceID;
				sourceEventArgs.CaptureID = this.m_currentCaptureID;
				sourceEventArgs.UniqueID = this.m_uniqueID;
				sourceEventArgs.Name = text;
				SdpApp.EventsManager.Raise<SourceEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SourceSelected, this, sourceEventArgs);
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002B560 File Offset: 0x00029760
		private void dataExplorerViewEvents_SelectRow(object sender, DataExplorerViewSelectRowEventArgs args)
		{
			if (this.m_currentSourceID == args.SourceID && this.m_currentCaptureID == args.CaptureID)
			{
				List<object> list = new List<object>();
				if (args.HighlightElements == null && args.RowElement != null)
				{
					list.Add(args.RowElement);
				}
				else if (args.HighlightElements != null)
				{
					list = new List<object>(args.HighlightElements);
				}
				this.m_view.SetHighlightedElements(list, args.SearchColumn);
				if (this.m_highlightActive)
				{
					this.m_view.Refilter(this.m_comboFilters, this.m_entryFilters, true, this.m_drawcallsActive, this.m_expandedRowsList);
				}
				if (args.RowElement != null)
				{
					this.m_view.Select(args.RowElement, args.SearchColumn, args.Expand);
				}
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002B62C File Offset: 0x0002982C
		private void dataExplorerViewModel_ViewSourceAdded(object sender, ViewSourceAddedEventArgs e)
		{
			if (e.UniqueID != this.m_uniqueID)
			{
				return;
			}
			ViewSource viewSource = e.ViewSource;
			this.m_view.SetSourceVisibility(viewSource.SourceName, viewSource.SourceID, viewSource.CaptureID, true);
			this.m_view.SetSelected(viewSource.SourceID, viewSource.CaptureID);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002B68C File Offset: 0x0002988C
		private void dataExplorerViewEvents_Invalidate(object sender, DataExplorerViewInvalidateEventArgs args)
		{
			if (args.UniqueID != this.m_uniqueID)
			{
				return;
			}
			object dataExplorerViewLock = this.m_dataExplorerViewLock;
			lock (dataExplorerViewLock)
			{
				if (!args.Model.UpdateFilters)
				{
					this.m_view.Reset();
					this.m_comboFilters.Clear();
					this.m_entryFilters.Clear();
					this.m_columns = args.Columns;
				}
				foreach (DataExplorerViewColumn dataExplorerViewColumn in args.Columns)
				{
					if (dataExplorerViewColumn.HasPixbuf)
					{
						this.m_view.AddPixbufColumn(dataExplorerViewColumn);
					}
					else
					{
						this.m_view.AddTextColumn(dataExplorerViewColumn);
					}
				}
				this.m_view.ExpanderColumnIndex = args.ExpanderColumnIndex;
				this.m_view.DisableHighlightButton(args.DisableHighlightFilter);
				this.m_view.Invalidate(args.Model, args.Filters);
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002B7AC File Offset: 0x000299AC
		private void clientEvents_CaptureNameChanged(object sender, EventArgs e)
		{
			this.m_view.CaptureNameChanged();
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002B7B9 File Offset: 0x000299B9
		private void dataExplorerViewEvents_Refilter(object sender, EventArgs args)
		{
			this.m_view.Refilter(this.m_comboFilters, this.m_entryFilters, this.m_highlightActive, this.m_drawcallsActive, this.m_expandedRowsList);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002B7E4 File Offset: 0x000299E4
		private void dataExplorerViewEvents_ShowShaderProfiling(object sender, EventArgs args)
		{
			this.m_view.ShowShaderProfiling();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002B7F1 File Offset: 0x000299F1
		private void dataExplorerViewEvents_UpdateSingleRow(object sender, UpdateSingleRowArgs args)
		{
			this.m_view.UpdateSingleRow(args.Row);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002B804 File Offset: 0x00029A04
		private void dataExplorerViewEvents_ToggleColumn(object sender, ColumnToggledEventArgs args)
		{
			this.m_view.UpdateColumnVisibilty(args.Id, args.Enabled);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0002B820 File Offset: 0x00029A20
		private void m_view_ComboOptionSelected(object sender, ComboOptionSelectedEventArgs args)
		{
			this.m_drawcallsActive = args.Option == "Show Only Drawcalls";
			if (args.Option == null || this.m_drawcallsActive)
			{
				this.m_comboFilters.Remove(args.Column);
			}
			else if (!this.m_drawcallsActive)
			{
				this.m_comboFilters[args.Column] = args.Option.ToLower();
			}
			this.m_view.SetExpandedList(this.m_expandedRowsList);
			this.m_view.Refilter(this.m_comboFilters, this.m_entryFilters, this.m_highlightActive, this.m_drawcallsActive, this.m_expandedRowsList);
			args.UniqueID = this.m_uniqueID;
			SdpApp.EventsManager.Raise<ComboOptionSelectedEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.ComboOptionSelected, sender, args);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002B8EC File Offset: 0x00029AEC
		private void view_CustomComboSelected(object sender, CustomComboSelectedEventArgs args)
		{
			args.UniqueID = this.m_uniqueID;
			args.CaptureID = this.m_currentCaptureID;
			SdpApp.EventsManager.Raise<CustomComboSelectedEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.CustomComboOptionSelected, sender, args);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002B924 File Offset: 0x00029B24
		private void m_view_EntryStateChanged(object sender, EntryStateChangedEventArgs args)
		{
			bool flag = false;
			if (args.Text.Length == 0)
			{
				if (this.m_entryFilters.ContainsKey(args.Column))
				{
					flag = true;
					this.m_entryFilters.Remove(args.Column);
				}
			}
			else if (!this.m_entryFilters.ContainsKey(args.Column) || this.m_entryFilters[args.Column] != args.Text.ToLower())
			{
				flag = true;
				this.m_entryFilters[args.Column] = args.Text.ToLower();
			}
			if (flag)
			{
				this.m_view.Refilter(this.m_comboFilters, this.m_entryFilters, this.m_highlightActive, this.m_drawcallsActive, this.m_expandedRowsList);
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002B9EC File Offset: 0x00029BEC
		private void m_view_ColumnToggled(object sender, ColumnToggledEventArgs args)
		{
			args.UniqueID = this.m_uniqueID;
			args.CaptureId = this.m_currentCaptureID;
			args.SourceId = this.m_currentSourceID;
			SdpApp.EventsManager.Raise<ColumnToggledEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.ColumnToggled, this, args);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002BA38 File Offset: 0x00029C38
		private void m_view_HighlightToggled(object sender, HighlightToggledEventArgs args)
		{
			this.m_highlightActive = args.Active;
			this.m_view.Refilter(this.m_comboFilters, this.m_entryFilters, this.m_highlightActive, this.m_drawcallsActive, this.m_expandedRowsList);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002BA70 File Offset: 0x00029C70
		private void m_view_RowExpanded(object sender, RowExpandAndCollapseEventArgs args)
		{
			List<string> list;
			if (!this.m_expandedRowsList.TryGetValue(this.m_currentCaptureID, out list))
			{
				this.m_expandedRowsList[this.m_currentCaptureID] = new List<string>();
			}
			if (!this.m_expandedRowsList[this.m_currentCaptureID].Contains(args.RowPath))
			{
				this.m_expandedRowsList[this.m_currentCaptureID].Add(args.RowPath);
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002BAE4 File Offset: 0x00029CE4
		private void m_view_RowCollapsed(object sender, RowExpandAndCollapseEventArgs args)
		{
			List<string> list;
			if (this.m_expandedRowsList.TryGetValue(this.m_currentCaptureID, out list) && list.Contains(args.RowPath))
			{
				list.RemoveAll((string x) => x.Contains(args.RowPath));
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0002BB39 File Offset: 0x00029D39
		private void dataExplorerViewEvents_SelectSource(object sender, SourceEventArgs args)
		{
			if (args.UniqueID != this.m_uniqueID)
			{
				return;
			}
			this.m_view.SetSelected(args.SourceID, args.CaptureID);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0002BB66 File Offset: 0x00029D66
		private void dataExplorerViewEvents_SetStatus(object sender, SetStatusEventArgs e)
		{
			if (e.UniqueID != this.m_uniqueID)
			{
				return;
			}
			this.m_view.SetStatus(e.Status, e.StatusText, e.Duration, false, null);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0002BB9B File Offset: 0x00029D9B
		private void dataExplorerViewEvents_HideStatus(object sender, MultiViewArgs e)
		{
			if (e.UniqueID != this.m_uniqueID)
			{
				return;
			}
			this.m_view.HideStatus();
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0002BBBC File Offset: 0x00029DBC
		private void dataExplorerViewEvents_InitializeUserSelectableMetrics(object sender, InitializeTreeComboArgs e)
		{
			if (e.UniqueID != this.m_uniqueID)
			{
				return;
			}
			this.m_view.InitializeUserSelectableMetrics(e);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002BBDE File Offset: 0x00029DDE
		private void OnDisableReplay(object sender, DisableReplayEventArgs args)
		{
			this.m_view.Disable(args.Disable);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0002BBF4 File Offset: 0x00029DF4
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

		// Token: 0x06000E0D RID: 3597 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0002BC23 File Offset: 0x00029E23
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x0400098D RID: 2445
		private int m_currentSourceID;

		// Token: 0x0400098E RID: 2446
		private int m_currentCaptureID;

		// Token: 0x0400098F RID: 2447
		private Dictionary<int, string> m_comboFilters;

		// Token: 0x04000990 RID: 2448
		private Dictionary<int, string> m_entryFilters;

		// Token: 0x04000991 RID: 2449
		private List<DataExplorerViewColumn> m_columns;

		// Token: 0x04000992 RID: 2450
		private bool m_highlightActive;

		// Token: 0x04000993 RID: 2451
		private bool m_drawcallsActive;

		// Token: 0x04000994 RID: 2452
		private Dictionary<int, List<string>> m_expandedRowsList;

		// Token: 0x04000995 RID: 2453
		private Dictionary<uint, string> m_processes;

		// Token: 0x04000996 RID: 2454
		private Dictionary<uint, string> m_threads;

		// Token: 0x04000997 RID: 2455
		private object m_dataExplorerViewLock = new object();

		// Token: 0x04000998 RID: 2456
		public const string MINI_EXPLORER_VIEW_PARAM = "2";

		// Token: 0x04000999 RID: 2457
		private IDataExplorerView m_view;

		// Token: 0x0400099A RID: 2458
		private string m_uniqueID;
	}
}
