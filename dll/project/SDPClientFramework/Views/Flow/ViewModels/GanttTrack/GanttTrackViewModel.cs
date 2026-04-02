using System;
using System.Collections.Generic;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.Helpers;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x0200001A RID: 26
	internal class GanttTrackViewModel : IGanttTrackViewModel, IReadOnlyGanttTrackViewModel
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000062 RID: 98 RVA: 0x00002F88 File Offset: 0x00001188
		// (remove) Token: 0x06000063 RID: 99 RVA: 0x00002FC0 File Offset: 0x000011C0
		public event EventHandler SelectionUpdated;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000064 RID: 100 RVA: 0x00002FF8 File Offset: 0x000011F8
		// (remove) Token: 0x06000065 RID: 101 RVA: 0x00003030 File Offset: 0x00001230
		public event EventHandler HighlightRegionUpdated;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003065 File Offset: 0x00001265
		public List<Series> Series
		{
			get
			{
				return this.SelectionViewModel.Series;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003072 File Offset: 0x00001272
		IReadOnlyList<Series> IReadOnlyGanttTrackViewModel.Series
		{
			get
			{
				return this.Series;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000307A File Offset: 0x0000127A
		private ILogger Logger { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003082 File Offset: 0x00001282
		public Maybe<HighlightRegion> HighlightRegion
		{
			get
			{
				return this.HighlightViewModel.HighlightRegion.Map<HighlightRegion>(new Func<HighlightRegion, HighlightRegion>(this.ClampToVisibleRegion));
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000030A0 File Offset: 0x000012A0
		private HighlightRegion ClampToVisibleRegion(HighlightRegion region)
		{
			return new HighlightRegion
			{
				HighlightBegin = this.ClampPointToVisibleRegion(region.HighlightBegin),
				HighlightEnd = this.ClampPointToVisibleRegion(region.HighlightEnd)
			};
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000030CC File Offset: 0x000012CC
		private DataViewPoint ClampPointToVisibleRegion(DataViewPoint point)
		{
			long num = this.ToTimestamp(0);
			long num2 = this.ToTimestamp(this.GetDataViewWidth());
			long num3 = MathExtensions.Clamp<long>(point.Timestamp, num, num2);
			if (num3 != point.Timestamp)
			{
				point.X = this.ToDataViewXAxisPoint(num3);
			}
			point.Y = MathExtensions.Clamp<int>(point.Y, 0, this.GetDataViewHeight());
			return point;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003144 File Offset: 0x00001344
		public InspectorViewModel SingleSelectInspectorViewModel
		{
			get
			{
				return new InspectorViewModelBuilder(this.SelectionViewModel, this.ToolTipModel(), this.Logger, ViewModelType.SingleSelectViewModel).Build();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003168 File Offset: 0x00001368
		public InspectorViewModel MultiSelectInspectorViewModel
		{
			get
			{
				return new InspectorViewModelBuilder(this.SelectionViewModel, this.ToolTipModel(), this.Logger, ViewModelType.MultiSelectViewModel).Build();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000318C File Offset: 0x0000138C
		private ISelectionViewModel SelectionViewModel { get; } = new SelectionViewModel();

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003194 File Offset: 0x00001394
		private IHighlightViewModel HighlightViewModel { get; } = new HighlightViewModel();

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000319C File Offset: 0x0000139C
		private Func<Dictionary<uint, string>> ToolTipModel { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000031A4 File Offset: 0x000013A4
		private Func<int> GetDataViewHeight { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000031AC File Offset: 0x000013AC
		private Func<int> GetDataViewWidth { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000031B4 File Offset: 0x000013B4
		private Func<int, long> ToTimestamp { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000031BC File Offset: 0x000013BC
		private Func<long, int> ToDataViewXAxisPoint { get; }

		// Token: 0x06000075 RID: 117 RVA: 0x000031C4 File Offset: 0x000013C4
		public GanttTrackViewModel(Func<int> getDataViewHeight, Func<int> getDataViewWidth, Func<int, long> toTimeStamp, Func<long, int> toDataViewXAxisPoint, Func<Dictionary<uint, string>> tooltipModel, ILogger logger)
		{
			this.SelectionViewModel.SelectionUpdated += delegate(object _s, EventArgs e)
			{
				EventHandler selectionUpdated = this.SelectionUpdated;
				if (selectionUpdated == null)
				{
					return;
				}
				selectionUpdated(this, e);
			};
			this.HighlightViewModel.HighlightRegionUpdated += delegate(object _s, EventArgs e)
			{
				EventHandler highlightRegionUpdated = this.HighlightRegionUpdated;
				if (highlightRegionUpdated == null)
				{
					return;
				}
				highlightRegionUpdated(this, e);
			};
			this.GetDataViewHeight = getDataViewHeight;
			this.GetDataViewWidth = getDataViewWidth;
			this.ToTimestamp = toTimeStamp;
			this.ToDataViewXAxisPoint = toDataViewXAxisPoint;
			this.ToolTipModel = tooltipModel;
			this.Logger = logger;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003248 File Offset: 0x00001448
		public bool IsSelected(Series series, Element element)
		{
			return this.SelectionViewModel.IsSelected(series, element);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003257 File Offset: 0x00001457
		public bool IsSelected(Series series, Marker marker)
		{
			return this.SelectionViewModel.IsSelected(series, marker);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003266 File Offset: 0x00001466
		public int GetSelectedObjectCount()
		{
			return this.SelectionViewModel.GetSelectedObjectCount();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003273 File Offset: 0x00001473
		public int GetSelectedMarkerCount()
		{
			return this.SelectionViewModel.GetSelectedMarkerCount();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003280 File Offset: 0x00001480
		public int GetSelectedSeriesCount()
		{
			return this.SelectionViewModel.GetSelectedSeriesCount();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000328D File Offset: 0x0000148D
		public Maybe<SeriesGanttObjectPair> GetLastSelected()
		{
			return this.SelectionViewModel.GetLastSelected();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000329A File Offset: 0x0000149A
		public Maybe<SeriesGanttObjectPair> GetGanttObjectAtPoint(DataViewPoint point)
		{
			return this.SelectionViewModel.GetGanttObjectAtPoint(point);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000032A8 File Offset: 0x000014A8
		public Maybe<SeriesElementPair> GetElementAtPoint(DataViewPoint point)
		{
			return this.SelectionViewModel.GetElementAtPoint(point);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032B6 File Offset: 0x000014B6
		public void SelectElement(SeriesElementPair newSelection, SelectionType modifier)
		{
			this.SelectionViewModel.SelectElement(newSelection, modifier);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000032C5 File Offset: 0x000014C5
		public void SelectMarker(SeriesMarkerPair newSelection, SelectionType modifier)
		{
			this.SelectionViewModel.SelectMarker(newSelection, modifier);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000032D4 File Offset: 0x000014D4
		public void SelectElementsInHighlightRegion()
		{
			this.HighlightRegion.Match(delegate(HighlightRegion someRegion)
			{
				this.SelectObjectsInHighlightRegion(someRegion);
			}, delegate
			{
				this.ClearSelections();
			});
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000032F9 File Offset: 0x000014F9
		private void SelectObjectsInHighlightRegion(HighlightRegion highlightRegion)
		{
			this.SelectionViewModel.SelectObjectsInHighlightRegion(highlightRegion);
			this.HighlightViewModel.ClearHighlightRegion();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003312 File Offset: 0x00001512
		public void MoveSelection(MoveDirection direction, SelectionType selection)
		{
			this.SelectionViewModel.MoveSelection(direction, selection);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003321 File Offset: 0x00001521
		public void ClearSelections()
		{
			this.SelectionViewModel.ClearSelections();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000332E File Offset: 0x0000152E
		public void SetHighlightRegion(DataViewPoint begin, DataViewPoint end, HighlightRegionType highlightType)
		{
			this.HighlightViewModel.SetHighlightRegion(begin, end, highlightType);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000333E File Offset: 0x0000153E
		public void ClearHighlightRegion()
		{
			this.HighlightViewModel.ClearHighlightRegion();
		}
	}
}
