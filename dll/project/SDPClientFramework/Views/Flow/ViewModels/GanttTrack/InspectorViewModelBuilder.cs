using System;
using System.Collections.Generic;
using System.Linq;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Helpers;
using Sdp.Logging;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000027 RID: 39
	internal class InspectorViewModelBuilder : IInspectorViewModelBuilder
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000037DF File Offset: 0x000019DF
		private IReadOnlySelectionViewModel SelectionViewModel { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000037E7 File Offset: 0x000019E7
		private Dictionary<uint, string> ToolTipModel { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000037EF File Offset: 0x000019EF
		private ILogger Logger { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000037F7 File Offset: 0x000019F7
		private ViewModelType Type { get; }

		// Token: 0x060000AA RID: 170 RVA: 0x00003800 File Offset: 0x00001A00
		public InspectorViewModelBuilder(IReadOnlySelectionViewModel selectionViewModel, Dictionary<uint, string> toolTipModel, ILogger logger, ViewModelType type)
		{
			if (selectionViewModel.GetSelectedObjectCount() == 0)
			{
				throw new ArgumentException("Must have at least one element to build a view model");
			}
			if (type == ViewModelType.SingleSelectViewModel && selectionViewModel.GetSelectedObjectCount() != 1)
			{
				throw new ArgumentException("Trying to build single select model for track with multiple selected objects");
			}
			this.SelectionViewModel = selectionViewModel;
			this.ToolTipModel = toolTipModel;
			this.Logger = logger;
			this.Type = type;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000385B File Offset: 0x00001A5B
		public InspectorViewModel Build()
		{
			if (this.Type == ViewModelType.MultiSelectViewModel)
			{
				return this.BuildMultiSelctionViewModel();
			}
			return this.BuildSingleSelectionViewModel();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003874 File Offset: 0x00001A74
		private InspectorViewModel BuildMultiSelctionViewModel()
		{
			long minTimestamp = this.SelectionViewModel.GetMinTimestamp();
			long maxTimestamp = this.SelectionViewModel.GetMaxTimestamp();
			long num = maxTimestamp - minTimestamp;
			return new MultiSelectInspectorViewModel
			{
				TimeDuration = num,
				StartTime = minTimestamp,
				EndTime = maxTimestamp,
				SeriesVMs = this.SelectionViewModel.GetSeriesSelections().Select(new Func<SeriesSelection, SeriesInspectorViewModel>(this.CreateSeriesViewModel)),
				SeriesSelections = this.SelectionViewModel.GetSeriesSelections()
			};
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000038EC File Offset: 0x00001AEC
		private InspectorViewModel BuildSingleSelectionViewModel()
		{
			Maybe<SeriesGanttObjectPair> lastSelected = this.SelectionViewModel.GetLastSelected();
			string text = lastSelected.Map<string>(new Func<SeriesGanttObjectPair, string>(this.GetToolTipString)).Expect(new ApplicationException("Building single selection view model when no objects selected"));
			string[] array = text.Split(new char[] { '\n' });
			if (array.Length == 0)
			{
				return new SingleSelectionViewModel();
			}
			string text2 = array[0];
			IEnumerable<PropertyContent> enumerable = array.Skip(1).Choose(new Func<string, Maybe<PropertyContent>>(InspectorViewModelBuilder.ToPropertyContent));
			return new SingleSelectionViewModel
			{
				CategoryName = text2,
				Properties = enumerable.ToList<PropertyContent>()
			};
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000397A File Offset: 0x00001B7A
		private string GetToolTipString(SeriesGanttObjectPair selected)
		{
			if (this.ToolTipModel == null)
			{
				return "";
			}
			return selected.Match<string>((SeriesElementPair someElement) => this.ToolTipModel[someElement.Element.TooltipId], (SeriesMarkerPair someMarker) => this.ToolTipModel[someMarker.Marker.TooltipId]);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000039A8 File Offset: 0x00001BA8
		private static Maybe<PropertyContent> ToPropertyContent(string propertyString)
		{
			string[] array = propertyString.Split(new char[] { ':', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				return new Maybe<PropertyContent>.Some(new PropertyContent
				{
					Name = array[0],
					Value = array[1].TrimStart(Array.Empty<char>())
				});
			}
			return new Maybe<PropertyContent>.None();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003A00 File Offset: 0x00001C00
		private SeriesInspectorViewModel CreateSeriesViewModel(SeriesSelection seriesSelection)
		{
			SeriesInspectorViewModel seriesInspectorViewModel = new SeriesInspectorViewModel();
			seriesInspectorViewModel.SeriesName = seriesSelection.Series.Name;
			seriesInspectorViewModel.NumberSelected = seriesSelection.Elements.Count<Element>() + seriesSelection.Markers.Count<Marker>();
			seriesInspectorViewModel.AccumulatedDuration = seriesSelection.Elements.Select((Element element) => element.End - element.Start).Sum();
			return seriesInspectorViewModel;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003A78 File Offset: 0x00001C78
		public static InspectorViewModel BuildCumulative(InspectorViewModel first, InspectorViewModel second)
		{
			MultiSelectInspectorViewModel multiSelectInspectorViewModel = first as MultiSelectInspectorViewModel;
			MultiSelectInspectorViewModel multiSelectInspectorViewModel2 = second as MultiSelectInspectorViewModel;
			if (multiSelectInspectorViewModel == null || multiSelectInspectorViewModel2 == null)
			{
				throw new ArgumentException("Trying to build cumulitive from single select view model");
			}
			return InspectorViewModelBuilder.BuildCumulative(multiSelectInspectorViewModel, multiSelectInspectorViewModel2);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003AAC File Offset: 0x00001CAC
		public static InspectorViewModel BuildCumulative(MultiSelectInspectorViewModel first, MultiSelectInspectorViewModel second)
		{
			long num = Math.Min(first.StartTime, second.StartTime);
			long num2 = Math.Max(first.EndTime, second.EndTime);
			long num3 = num2 - num;
			return new MultiSelectInspectorViewModel
			{
				StartTime = num,
				EndTime = num2,
				TimeDuration = num3,
				SeriesVMs = first.SeriesVMs.Concat(second.SeriesVMs)
			};
		}
	}
}
