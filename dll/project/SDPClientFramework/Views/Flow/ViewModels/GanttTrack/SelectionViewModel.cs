using System;
using System.Collections.Generic;
using System.Linq;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Helpers;
using SDPClientFramework.Views.Flow.Controllers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000032 RID: 50
	internal class SelectionViewModel : ISelectionViewModel, IReadOnlySelectionViewModel
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060000DE RID: 222 RVA: 0x00003C90 File Offset: 0x00001E90
		// (remove) Token: 0x060000DF RID: 223 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public event EventHandler SelectionUpdated;

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003CFD File Offset: 0x00001EFD
		public List<Series> Series { get; } = new List<Series>();

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003D05 File Offset: 0x00001F05
		IReadOnlyList<Series> IReadOnlySelectionViewModel.Series
		{
			get
			{
				return this.Series;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003D0D File Offset: 0x00001F0D
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003D15 File Offset: 0x00001F15
		private IDictionary<Series, SelectionViewModel.SelectionRanges> Selections { get; set; } = new Dictionary<Series, SelectionViewModel.SelectionRanges>();

		// Token: 0x060000E4 RID: 228 RVA: 0x00003D20 File Offset: 0x00001F20
		public bool IsSelected(Series series, Element element)
		{
			return this.IsSelected<uint, Element>(series, (SelectionViewModel.SelectionRanges ranges) => ranges.ElementSelections, element, series.Elements, (Element e) => e.BlockId);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003D7C File Offset: 0x00001F7C
		public bool IsSelected(Series series, Marker marker)
		{
			return this.IsSelected<long, Marker>(series, (SelectionViewModel.SelectionRanges ranges) => ranges.MarkerSelections, marker, series.Markers, (Marker m) => m.Position);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003DD8 File Offset: 0x00001FD8
		private bool IsSelected<TValue, TObject>(Series series, Func<SelectionViewModel.SelectionRanges, Maybe<SelectionViewModel.SelectionRange>> rangeSelector, TObject obj, List<TObject> objects, Func<TObject, TValue> valueSelector) where TValue : IComparable
		{
			return this.TryGetSelectionRange(series, rangeSelector).Match<bool>((SelectionViewModel.SelectionRange someRange) => SelectionViewModel.IsInRange<TValue, TObject>(someRange, obj, objects, valueSelector), () => false);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003E3C File Offset: 0x0000203C
		private Maybe<SelectionViewModel.SelectionRange> TryGetSelectionRange(Series series, Func<SelectionViewModel.SelectionRanges, Maybe<SelectionViewModel.SelectionRange>> rangeSelector)
		{
			return this.Selections.TryGetValue(series).Bind<SelectionViewModel.SelectionRange>((SelectionViewModel.SelectionRanges selections) => rangeSelector(selections));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003E74 File Offset: 0x00002074
		private static bool IsInRange<TValue, TObject>(SelectionViewModel.SelectionRange range, TObject obj, List<TObject> values, Func<TObject, TValue> getValue) where TValue : IComparable
		{
			TValue tvalue = getValue(values[range.SelectedIndexFirst]);
			TValue tvalue2 = getValue(values[range.SelectedIndexLast]);
			TValue tvalue3 = getValue(obj);
			return tvalue3.CompareTo(tvalue) >= 0 && tvalue3.CompareTo(tvalue2) <= 0;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003EE0 File Offset: 0x000020E0
		public int GetSelectedObjectCount()
		{
			return this.Selections.Select((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.Count).Sum();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003F14 File Offset: 0x00002114
		public int GetSelectedMarkerCount()
		{
			return (from markerSelection in this.Selections.Choose((KeyValuePair<Series, SelectionViewModel.SelectionRanges> selections) => selections.Value.MarkerSelections)
				select markerSelection.Count).Sum();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003F74 File Offset: 0x00002174
		public IEnumerable<SeriesSelection> GetSeriesSelections()
		{
			return this.Selections.Select(delegate(KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp)
			{
				Series series = kvp.Key;
				SelectionViewModel.SelectionRanges value = kvp.Value;
				IEnumerable<Element> enumerable = value.ElementSelections.Map<IEnumerable<Element>>((SelectionViewModel.SelectionRange range) => SelectionViewModel.GetSelectedElements(series, range)).ChooseMany<Element>();
				IEnumerable<Marker> enumerable2 = value.MarkerSelections.Map<IEnumerable<Marker>>((SelectionViewModel.SelectionRange range) => SelectionViewModel.GetSelectedMarkers(series, range)).ChooseMany<Marker>();
				return new SeriesSelection
				{
					Series = series,
					Elements = enumerable,
					Markers = enumerable2
				};
			});
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003FA0 File Offset: 0x000021A0
		private static IEnumerable<Element> GetSelectedElements(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			List<Element> range = series.Elements.GetRange(selectionRange.SelectedIndexFirst, selectionRange.SelectedIndexLast - selectionRange.SelectedIndexFirst + 1);
			if (!SelectionViewModel.AreIncreasing(range))
			{
				string text = range.Select((Element e) => string.Format("BlockID: {0}; Start: {1}; End: {2}", e.BlockId, e.Start, e.End)).Aggregate((string s1, string s2) => s1 + "\n" + s2);
				throw new ArgumentException("Block IDs must be in increasing order.\n" + text);
			}
			return range;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004032 File Offset: 0x00002232
		private static bool AreIncreasing(IEnumerable<Element> elements)
		{
			return elements.All((Element eCurrent, Element ePrevious) => eCurrent.BlockId > ePrevious.BlockId);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000405C File Offset: 0x0000225C
		private static IEnumerable<Marker> GetSelectedMarkers(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			List<Marker> range = series.Markers.GetRange(selectionRange.SelectedIndexFirst, selectionRange.SelectedIndexLast - selectionRange.SelectedIndexFirst + 1);
			if (!SelectionViewModel.AreIncreasing(range))
			{
				throw new ArgumentException("Markers must be in increasing timestamp order");
			}
			return range;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000409E File Offset: 0x0000229E
		private static bool AreIncreasing(IEnumerable<Marker> elements)
		{
			return elements.All((Marker eCurrent, Marker ePrevious) => eCurrent.Position >= ePrevious.Position);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000040C8 File Offset: 0x000022C8
		public Maybe<SeriesGanttObjectPair> GetGanttObjectAtPoint(DataViewPoint point)
		{
			Maybe<SeriesGanttObjectPair> elementMaybe = this.GetElementAtPoint(point).Cast<SeriesGanttObjectPair>();
			return elementMaybe.Match<Maybe<SeriesGanttObjectPair>>((SeriesGanttObjectPair element) => elementMaybe, () => this.GetMarkerAtPoint(point).Cast<SeriesGanttObjectPair>());
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004124 File Offset: 0x00002324
		public Maybe<SeriesElementPair> GetElementAtPoint(DataViewPoint point)
		{
			return this.GetSeriesAtRowIndex(point.RowIndex).Bind<SeriesElementPair>((Series someSeries) => this.GetElementAtPoint(someSeries, point));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004168 File Offset: 0x00002368
		private Maybe<Series> GetSeriesAtRowIndex(int rowIndex)
		{
			Series series = this.AllExpandedSeries().Skip(rowIndex).FirstOrDefault<Series>();
			if (series == null)
			{
				return new Maybe<Series>.None();
			}
			return new Maybe<Series>.Some(series);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004196 File Offset: 0x00002396
		private IEnumerable<Series> AllExpandedSeries()
		{
			return this.Series.SelectMany(new Func<Series, IEnumerable<Series>>(this.AllExpandedSeries));
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000041B0 File Offset: 0x000023B0
		private IEnumerable<Series> AllExpandedSeries(Series series)
		{
			if (series.Children.IsEmpty<Series>())
			{
				return series.ToEnumerable<Series>();
			}
			IEnumerable<Series> enumerable = series.Children.SelectMany(new Func<Series, IEnumerable<Series>>(this.AllExpandedSeries));
			if (series.IsExpanded)
			{
				return series.ToEnumerable<Series>().Concat(enumerable);
			}
			IEnumerable<Series> enumerable2 = enumerable.Where((Series s) => s.Elements.IsNotEmpty<Element>());
			if (enumerable2.Count<Series>() >= 1)
			{
				return enumerable2.First<Series>().ToEnumerable<Series>();
			}
			return series.ToEnumerable<Series>();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004240 File Offset: 0x00002440
		private Maybe<SeriesElementPair> GetElementAtPoint(Series series, DataViewPoint point)
		{
			Element element = series.Elements.FirstOrDefault((Element e) => SelectionViewModel.IsInRange(e, new Func<long, int>(point.Handler.ToDataViewXAxisPoint), point.X, point.X));
			if (element == null)
			{
				return new Maybe<SeriesElementPair>.None();
			}
			return new Maybe<SeriesElementPair>.Some(new SeriesElementPair(series, element));
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004288 File Offset: 0x00002488
		private static bool IsInRange(Element e, Func<long, int> timestampToXAxis, int minX, int maxX)
		{
			int num = timestampToXAxis(e.Start);
			int num2 = timestampToXAxis(e.End);
			return num2 >= minX && num <= maxX;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000042BC File Offset: 0x000024BC
		public Maybe<SeriesMarkerPair> GetMarkerAtPoint(DataViewPoint point)
		{
			Maybe<Series> seriesAtRowIndex = this.GetSeriesAtRowIndex(point.RowIndex);
			return seriesAtRowIndex.Bind<SeriesMarkerPair>((Series someSeries) => this.GetMarkerAtPoint(someSeries, point));
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004304 File Offset: 0x00002504
		private Maybe<SeriesMarkerPair> GetMarkerAtPoint(Series series, DataViewPoint point)
		{
			Marker marker = series.Markers.FirstOrDefault((Marker m) => SelectionViewModel.IsInRange(m, new Func<long, int>(point.Handler.ToDataViewXAxisPoint), point.X, point.X));
			if (marker != null)
			{
				return new Maybe<SeriesMarkerPair>.Some(new SeriesMarkerPair(series, marker));
			}
			return new Maybe<SeriesMarkerPair>.None();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000434C File Offset: 0x0000254C
		private static bool IsInRange(Marker m, Func<long, int> timestampToXAxis, int minX, int maxX)
		{
			minX -= 3;
			maxX += 3;
			int num = timestampToXAxis(m.Position);
			return num >= minX && num <= maxX;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004380 File Offset: 0x00002580
		public Maybe<SeriesGanttObjectPair> GetLastSelected()
		{
			Maybe<SeriesElementPair> lastElement = this.GetLastSelectedElement();
			return lastElement.Match<Maybe<SeriesGanttObjectPair>>((SeriesElementPair _some) => lastElement.Cast<SeriesGanttObjectPair>(), () => this.GetLastSelectedMarker().Cast<SeriesGanttObjectPair>());
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000043CC File Offset: 0x000025CC
		private Maybe<SeriesElementPair> GetLastSelectedElement()
		{
			if (this.Selections.IsEmpty<KeyValuePair<Series, SelectionViewModel.SelectionRanges>>())
			{
				return new Maybe<SeriesElementPair>.None();
			}
			return this.Selections.Select((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.ElementSelections.Map<SeriesElementPair>((SelectionViewModel.SelectionRange elementRange) => new SeriesElementPair(kvp.Key, kvp.Key.Elements[elementRange.SelectedIndexEnd]))).Last<Maybe<SeriesElementPair>>();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000441C File Offset: 0x0000261C
		private Maybe<SeriesMarkerPair> GetLastSelectedMarker()
		{
			if (this.Selections.IsEmpty<KeyValuePair<Series, SelectionViewModel.SelectionRanges>>())
			{
				return new Maybe<SeriesMarkerPair>.None();
			}
			return this.Selections.Select((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.MarkerSelections.Map<SeriesMarkerPair>((SelectionViewModel.SelectionRange elementRange) => new SeriesMarkerPair(kvp.Key, kvp.Key.Markers[elementRange.SelectedIndexEnd]))).Last<Maybe<SeriesMarkerPair>>();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000446B File Offset: 0x0000266B
		public int GetSelectedSeriesCount()
		{
			return this.Selections.Count;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004478 File Offset: 0x00002678
		public long GetMaxTimestamp()
		{
			return this.Selections.ChooseManyValues(delegate(KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp)
			{
				Series series = kvp.Key;
				SelectionViewModel.SelectionRanges value = kvp.Value;
				IEnumerable<Maybe<long>> enumerable = value.MarkerSelections.Map<long>((SelectionViewModel.SelectionRange range) => series.Markers[range.SelectedIndexLast].Position).ToEnumerable<Maybe<long>>();
				IEnumerable<Maybe<long>> enumerable2 = value.ElementSelections.Map<long>((SelectionViewModel.SelectionRange range) => series.Elements.GetRange(range.SelectedIndexFirst, range.Count).Max((Element e) => e.End)).ToEnumerable<Maybe<long>>();
				return enumerable.Concat(enumerable2);
			}).Max();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000044A9 File Offset: 0x000026A9
		public long GetMinTimestamp()
		{
			return this.Selections.ChooseManyValues(delegate(KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp)
			{
				Series series = kvp.Key;
				SelectionViewModel.SelectionRanges value = kvp.Value;
				IEnumerable<Maybe<long>> enumerable = value.MarkerSelections.Map<long>((SelectionViewModel.SelectionRange range) => series.Markers[range.SelectedIndexFirst].Position).ToEnumerable<Maybe<long>>();
				IEnumerable<Maybe<long>> enumerable2 = value.ElementSelections.Map<long>((SelectionViewModel.SelectionRange range) => series.Elements.GetRange(range.SelectedIndexFirst, range.Count).Min((Element e) => e.Start)).ToEnumerable<Maybe<long>>();
				return enumerable.Concat(enumerable2);
			}).Min();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000044DA File Offset: 0x000026DA
		public void SelectElement(SeriesElementPair newSelection, SelectionType modifier)
		{
			if (modifier != SelectionType.SingleSelect)
			{
				if (modifier != SelectionType.RangeSelect)
				{
					throw new ArgumentException("Invalid selection modifier enum");
				}
				this.RangeSelectElement(newSelection);
			}
			else
			{
				this.SingleSelectElement(newSelection);
			}
			EventHandler selectionUpdated = this.SelectionUpdated;
			if (selectionUpdated == null)
			{
				return;
			}
			selectionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004518 File Offset: 0x00002718
		private void SingleSelectElement(SeriesElementPair selection)
		{
			selection = this.ConvertToLinkedElementIfAnnotationElement(selection);
			this.Selections.Clear();
			this.Selections[selection.Series] = SelectionViewModel.SelectionRanges.ElementSelectionRange(new SelectionViewModel.SelectionRange(selection.ElementIndex));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004550 File Offset: 0x00002750
		private void RangeSelectElement(SeriesElementPair selection)
		{
			selection = this.ConvertToLinkedElementIfAnnotationElement(selection);
			Func<SelectionViewModel.SelectionRange> <>9__1;
			Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> <>9__2;
			this.Selections.AddOrUpdate(selection.Series, SelectionViewModel.SelectionRanges.ElementSelectionRange(new SelectionViewModel.SelectionRange(selection.ElementIndex)), delegate(SelectionViewModel.SelectionRanges currentRanges)
			{
				Func<SelectionViewModel.SelectionRange> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = () => new SelectionViewModel.SelectionRange(selection.ElementIndex));
				}
				Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> func2;
				if ((func2 = <>9__2) == null)
				{
					func2 = (<>9__2 = (SelectionViewModel.SelectionRange currentRange) => new SelectionViewModel.SelectionRange(currentRange.SelectedIndexStart, selection.ElementIndex));
				}
				return currentRanges.AddOrUpdateElementRange(func, func2);
			});
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000045B4 File Offset: 0x000027B4
		private Series GetLinkedSeries(List<Series> series, Element match)
		{
			Series series2 = null;
			foreach (Series series3 in series)
			{
				if (series3.Elements.Contains(match))
				{
					series2 = series3;
				}
				if (series3.Children.Count > 0 && series2 == null)
				{
					series2 = this.GetLinkedSeries(series3.Children, match);
				}
				if (series2 != null)
				{
					return series2;
				}
			}
			return series2;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004638 File Offset: 0x00002838
		private SeriesElementPair ConvertToLinkedElementIfAnnotationElement(SeriesElementPair selected)
		{
			if (selected.Series.IsAnnotation())
			{
				Element annotationLink = selected.Element.AnnotationLink;
				Series linkedSeries = this.GetLinkedSeries(this.Series, annotationLink);
				return new SeriesElementPair
				{
					Series = linkedSeries,
					Element = annotationLink
				};
			}
			return selected;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004681 File Offset: 0x00002881
		public void SelectMarker(SeriesMarkerPair newSelection, SelectionType modifier)
		{
			if (modifier != SelectionType.SingleSelect)
			{
				if (modifier != SelectionType.RangeSelect)
				{
					throw new ArgumentException("Invalid selection modifier enum");
				}
				this.RangeSelectMarker(newSelection);
			}
			else
			{
				this.SingleSelectMarker(newSelection);
			}
			EventHandler selectionUpdated = this.SelectionUpdated;
			if (selectionUpdated == null)
			{
				return;
			}
			selectionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000046BF File Offset: 0x000028BF
		private void SingleSelectMarker(SeriesMarkerPair selection)
		{
			this.Selections.Clear();
			this.Selections[selection.Series] = SelectionViewModel.SelectionRanges.MarkerSelectionRange(new SelectionViewModel.SelectionRange(selection.MarkerIndex));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000046F0 File Offset: 0x000028F0
		private void RangeSelectMarker(SeriesMarkerPair selection)
		{
			Func<SelectionViewModel.SelectionRange> <>9__1;
			Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> <>9__2;
			this.Selections.AddOrUpdate(selection.Series, SelectionViewModel.SelectionRanges.MarkerSelectionRange(new SelectionViewModel.SelectionRange(selection.MarkerIndex)), delegate(SelectionViewModel.SelectionRanges currentRanges)
			{
				Func<SelectionViewModel.SelectionRange> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = () => new SelectionViewModel.SelectionRange(selection.MarkerIndex));
				}
				Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> func2;
				if ((func2 = <>9__2) == null)
				{
					func2 = (<>9__2 = (SelectionViewModel.SelectionRange currentRange) => new SelectionViewModel.SelectionRange(currentRange.SelectedIndexStart, selection.MarkerIndex));
				}
				return currentRanges.AddOrUpdateMarkerRange(func, func2);
			});
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004741 File Offset: 0x00002941
		public void SelectObjectsInHighlightRegion(HighlightRegion highlightRegion)
		{
			this.Selections = this.GetObjectsInHighlightRegion(highlightRegion);
			EventHandler selectionUpdated = this.SelectionUpdated;
			if (selectionUpdated == null)
			{
				return;
			}
			selectionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004768 File Offset: 0x00002968
		private IDictionary<Series, SelectionViewModel.SelectionRanges> GetObjectsInHighlightRegion(HighlightRegion highlightRegion)
		{
			int num = Math.Min(highlightRegion.HighlightBegin.RowIndex, highlightRegion.HighlightEnd.RowIndex);
			int num2 = Math.Max(highlightRegion.HighlightBegin.RowIndex, highlightRegion.HighlightEnd.RowIndex);
			int num3 = num2 - num + 1;
			int minX = Math.Min(highlightRegion.HighlightBegin.X, highlightRegion.HighlightEnd.X);
			int maxX = Math.Max(highlightRegion.HighlightBegin.X, highlightRegion.HighlightEnd.X);
			Func<long, int> toXAxis = new Func<long, int>(highlightRegion.HighlightBegin.Handler.ToDataViewXAxisPoint);
			return (from series in this.AllExpandedSeries().Skip(num).Take(num3)
				select new KeyValuePair<Series, SelectionViewModel.SelectionRanges>(series, this.GetSelectionRanges(series, toXAxis, minX, maxX)) into kvp
				where (kvp.Value.ElementSelections.IsSome() || kvp.Value.MarkerSelections.IsSome()) && !kvp.Key.IsAnnotation()
				select kvp).ToDictionary((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Key, (KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000048AC File Offset: 0x00002AAC
		private SelectionViewModel.SelectionRanges GetSelectionRanges(Series series, Func<long, int> toXAxis, int minX, int maxX)
		{
			return new SelectionViewModel.SelectionRanges
			{
				ElementSelections = this.GetObjectSelectionRange<Element>(series.Elements, (Element e) => SelectionViewModel.IsInRange(e, toXAxis, minX, maxX)),
				MarkerSelections = this.GetObjectSelectionRange<Marker>(series.Markers, (Marker m) => SelectionViewModel.IsInRange(m, toXAxis, minX, maxX))
			};
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004918 File Offset: 0x00002B18
		private Maybe<SelectionViewModel.SelectionRange> GetObjectSelectionRange<T>(List<T> objects, Predicate<T> isInRange)
		{
			int num = objects.FindIndex(isInRange);
			if (num < 0)
			{
				return new Maybe<SelectionViewModel.SelectionRange>.None();
			}
			int num2 = objects.FindLastIndex(isInRange);
			return new Maybe<SelectionViewModel.SelectionRange>.Some(new SelectionViewModel.SelectionRange(num, num2));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000494C File Offset: 0x00002B4C
		private IEnumerable<SeriesElementPair> ToSeriesElementPair(SeriesSelection selections)
		{
			return selections.Elements.Select((Element element) => new SeriesElementPair(selections.Series, element));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004982 File Offset: 0x00002B82
		public void MoveSelection(MoveDirection direction, SelectionType selectionType)
		{
			if (direction == MoveDirection.Left)
			{
				this.SelectPreviousObject(selectionType);
				return;
			}
			if (direction != MoveDirection.Right)
			{
				throw new ArgumentException("Invalid enum");
			}
			this.SelectNextObject(selectionType);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000049A8 File Offset: 0x00002BA8
		private void SelectPreviousObject(SelectionType selectionType)
		{
			SeriesElementPair seriesElementPair = this.Selections.Choose((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.ElementSelections.Map<SeriesElementPair>((SelectionViewModel.SelectionRange range) => this.GetPreviousSelectedElement(kvp.Key, range))).LastOrDefault<SeriesElementPair>();
			SeriesMarkerPair seriesMarkerPair = this.Selections.Choose((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.MarkerSelections.Map<SeriesMarkerPair>((SelectionViewModel.SelectionRange range) => this.GetPreviousSelectedMarker(kvp.Key, range))).LastOrDefault<SeriesMarkerPair>();
			if (seriesElementPair != null)
			{
				this.SelectElement(seriesElementPair, selectionType);
				return;
			}
			if (seriesMarkerPair != null)
			{
				this.SelectMarker(seriesMarkerPair, selectionType);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004A08 File Offset: 0x00002C08
		private SeriesElementPair GetPreviousSelectedElement(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			int num = Math.Max(0, selectionRange.SelectedIndexEnd - 1);
			Element element = series.Elements[num];
			return new SeriesElementPair(series, element);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004A38 File Offset: 0x00002C38
		private SeriesMarkerPair GetPreviousSelectedMarker(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			int num = Math.Max(0, selectionRange.SelectedIndexEnd - 1);
			Marker marker = series.Markers[num];
			return new SeriesMarkerPair(series, marker);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004A68 File Offset: 0x00002C68
		private void SelectNextObject(SelectionType selectionType)
		{
			SeriesElementPair seriesElementPair = this.Selections.Choose((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.ElementSelections.Map<SeriesElementPair>((SelectionViewModel.SelectionRange range) => this.GetNextSelectedElement(kvp.Key, range))).LastOrDefault<SeriesElementPair>();
			SeriesMarkerPair seriesMarkerPair = this.Selections.Choose((KeyValuePair<Series, SelectionViewModel.SelectionRanges> kvp) => kvp.Value.MarkerSelections.Map<SeriesMarkerPair>((SelectionViewModel.SelectionRange range) => this.GetNextSelectedMarker(kvp.Key, range))).LastOrDefault<SeriesMarkerPair>();
			if (seriesElementPair != null)
			{
				this.SelectElement(seriesElementPair, selectionType);
				return;
			}
			if (seriesMarkerPair != null)
			{
				this.SelectMarker(seriesMarkerPair, selectionType);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004AC8 File Offset: 0x00002CC8
		private SeriesElementPair GetNextSelectedElement(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			int num = series.Elements.Count - 1;
			int num2 = Math.Min(num, selectionRange.SelectedIndexEnd + 1);
			Element element = series.Elements[num2];
			return new SeriesElementPair(series, element);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004B08 File Offset: 0x00002D08
		private SeriesMarkerPair GetNextSelectedMarker(Series series, SelectionViewModel.SelectionRange selectionRange)
		{
			int num = series.Markers.Count - 1;
			int num2 = Math.Min(num, selectionRange.SelectedIndexEnd + 1);
			Marker marker = series.Markers[num2];
			return new SeriesMarkerPair(series, marker);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004B46 File Offset: 0x00002D46
		public void ClearSelections()
		{
			this.Selections.Clear();
			EventHandler selectionUpdated = this.SelectionUpdated;
			if (selectionUpdated == null)
			{
				return;
			}
			selectionUpdated(this, EventArgs.Empty);
		}

		// Token: 0x040000E6 RID: 230
		private const int MARKER_RADIUS = 3;

		// Token: 0x02000327 RID: 807
		private class SelectionRanges
		{
			// Token: 0x060010A7 RID: 4263 RVA: 0x0003423E File Offset: 0x0003243E
			public static SelectionViewModel.SelectionRanges ElementSelectionRange(SelectionViewModel.SelectionRange range)
			{
				return new SelectionViewModel.SelectionRanges
				{
					ElementSelections = new Maybe<SelectionViewModel.SelectionRange>.Some(range)
				};
			}

			// Token: 0x060010A8 RID: 4264 RVA: 0x00034251 File Offset: 0x00032451
			public static SelectionViewModel.SelectionRanges MarkerSelectionRange(SelectionViewModel.SelectionRange range)
			{
				return new SelectionViewModel.SelectionRanges
				{
					MarkerSelections = new Maybe<SelectionViewModel.SelectionRange>.Some(range)
				};
			}

			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00034264 File Offset: 0x00032464
			public int Count
			{
				get
				{
					int num = this.ElementSelections.Map<int>((SelectionViewModel.SelectionRange r) => r.Count).UnwrapOr(0);
					int num2 = this.MarkerSelections.Map<int>((SelectionViewModel.SelectionRange r) => r.Count).UnwrapOr(0);
					return num + num2;
				}
			}

			// Token: 0x060010AA RID: 4266 RVA: 0x000342D6 File Offset: 0x000324D6
			public SelectionViewModel.SelectionRanges AddOrUpdateElementRange(Func<SelectionViewModel.SelectionRange> onAdd, Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> onUpdate)
			{
				return new SelectionViewModel.SelectionRanges
				{
					MarkerSelections = this.MarkerSelections,
					ElementSelections = SelectionViewModel.SelectionRanges.AddOrUpdateRange(this.ElementSelections, onAdd, onUpdate)
				};
			}

			// Token: 0x060010AB RID: 4267 RVA: 0x000342FC File Offset: 0x000324FC
			public SelectionViewModel.SelectionRanges AddOrUpdateMarkerRange(Func<SelectionViewModel.SelectionRange> onAdd, Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> onUpdate)
			{
				return new SelectionViewModel.SelectionRanges
				{
					MarkerSelections = SelectionViewModel.SelectionRanges.AddOrUpdateRange(this.MarkerSelections, onAdd, onUpdate),
					ElementSelections = this.ElementSelections
				};
			}

			// Token: 0x060010AC RID: 4268 RVA: 0x00034324 File Offset: 0x00032524
			private static Maybe<SelectionViewModel.SelectionRange> AddOrUpdateRange(Maybe<SelectionViewModel.SelectionRange> range, Func<SelectionViewModel.SelectionRange> onAdd, Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> onUpdate)
			{
				SelectionViewModel.SelectionRange selectionRange = range.Match<SelectionViewModel.SelectionRange>((SelectionViewModel.SelectionRange someRange) => onUpdate(someRange), () => onAdd());
				return new Maybe<SelectionViewModel.SelectionRange>.Some(selectionRange);
			}

			// Token: 0x04000B22 RID: 2850
			public Maybe<SelectionViewModel.SelectionRange> ElementSelections = new Maybe<SelectionViewModel.SelectionRange>.None();

			// Token: 0x04000B23 RID: 2851
			public Maybe<SelectionViewModel.SelectionRange> MarkerSelections = new Maybe<SelectionViewModel.SelectionRange>.None();

			// Token: 0x02000451 RID: 1105
			// (Invoke) Token: 0x060013E1 RID: 5089
			public delegate SelectionViewModel.SelectionRanges AddOrUpdateRangeDelegate(Func<SelectionViewModel.SelectionRange> onAdd, Func<SelectionViewModel.SelectionRange, SelectionViewModel.SelectionRange> onUpdate);
		}

		// Token: 0x02000328 RID: 808
		public abstract class SelectionIndex
		{
			// Token: 0x060010AE RID: 4270
			public abstract void Match(Action<SelectionViewModel.SelectionIndex.ElementIndex> onElementIndex, Action<SelectionViewModel.SelectionIndex.MarkerIndex> onMarkerIndex);

			// Token: 0x02000454 RID: 1108
			public sealed class ElementIndex : SelectionViewModel.SelectionIndex
			{
				// Token: 0x17000309 RID: 777
				// (get) Token: 0x060013EB RID: 5099 RVA: 0x0003C9D9 File Offset: 0x0003ABD9
				public int Index { get; }

				// Token: 0x060013EC RID: 5100 RVA: 0x0003C9E1 File Offset: 0x0003ABE1
				public ElementIndex(int index)
				{
					this.Index = index;
				}

				// Token: 0x060013ED RID: 5101 RVA: 0x0003C9F0 File Offset: 0x0003ABF0
				public override void Match(Action<SelectionViewModel.SelectionIndex.ElementIndex> onElementIndex, Action<SelectionViewModel.SelectionIndex.MarkerIndex> onMarkerIndex)
				{
					onElementIndex(this);
				}
			}

			// Token: 0x02000455 RID: 1109
			public sealed class MarkerIndex : SelectionViewModel.SelectionIndex
			{
				// Token: 0x1700030A RID: 778
				// (get) Token: 0x060013EE RID: 5102 RVA: 0x0003C9F9 File Offset: 0x0003ABF9
				public int Index { get; }

				// Token: 0x060013EF RID: 5103 RVA: 0x0003CA01 File Offset: 0x0003AC01
				public MarkerIndex(int index)
				{
					this.Index = index;
				}

				// Token: 0x060013F0 RID: 5104 RVA: 0x0003CA10 File Offset: 0x0003AC10
				public override void Match(Action<SelectionViewModel.SelectionIndex.ElementIndex> onElementIndex, Action<SelectionViewModel.SelectionIndex.MarkerIndex> onMarkerIndex)
				{
					onMarkerIndex(this);
				}
			}
		}

		// Token: 0x02000329 RID: 809
		private class SelectionRange
		{
			// Token: 0x060010B0 RID: 4272 RVA: 0x00034388 File Offset: 0x00032588
			public SelectionRange(int selectedIndexStart)
			{
				this.SelectedIndexEnd = selectedIndexStart;
				this.SelectedIndexStart = selectedIndexStart;
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x000343AB File Offset: 0x000325AB
			public SelectionRange(int selectedIDStart, int selectedIDEnd)
			{
				this.SelectedIndexStart = selectedIDStart;
				this.SelectedIndexEnd = selectedIDEnd;
			}

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000343C1 File Offset: 0x000325C1
			public int Count
			{
				get
				{
					return this.SelectedIndexLast - this.SelectedIndexFirst + 1;
				}
			}

			// Token: 0x170002F2 RID: 754
			// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000343D2 File Offset: 0x000325D2
			public int SelectedIndexStart { get; }

			// Token: 0x170002F3 RID: 755
			// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000343DA File Offset: 0x000325DA
			public int SelectedIndexEnd { get; }

			// Token: 0x170002F4 RID: 756
			// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000343E2 File Offset: 0x000325E2
			public int SelectedIndexFirst
			{
				get
				{
					return Math.Min(this.SelectedIndexStart, this.SelectedIndexEnd);
				}
			}

			// Token: 0x170002F5 RID: 757
			// (get) Token: 0x060010B6 RID: 4278 RVA: 0x000343F5 File Offset: 0x000325F5
			public int SelectedIndexLast
			{
				get
				{
					return Math.Max(this.SelectedIndexStart, this.SelectedIndexEnd);
				}
			}
		}
	}
}
