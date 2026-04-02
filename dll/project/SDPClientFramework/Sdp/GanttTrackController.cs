using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Sdp.Charts.Gantt;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;
using SDPClientFramework.Views.Flow.Controllers;
using SDPClientFramework.Views.Flow.ViewModels.GanttTrack;

namespace Sdp
{
	// Token: 0x0200023A RID: 570
	public class GanttTrackController : TrackControllerBase
	{
		// Token: 0x0600090D RID: 2317 RVA: 0x0001A851 File Offset: 0x00018A51
		public GanttTrackController(ITrackViewBase trackViewBase, GroupLayoutController layoutContainer, GroupController groupContainer)
			: this(trackViewBase, layoutContainer, groupContainer, SdpApp.EventsManager, SdpApp.ModelManager, SdpApp.CommandManager, new Sdp.Logging.Logger("GantTrackController"))
		{
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001A878 File Offset: 0x00018A78
		public GanttTrackController(ITrackViewBase trackViewBase, GroupLayoutController layoutContainer, GroupController groupContainer, EventsManager eventsManager, IModelManager modelManager, ICommandManager commandManager, ILogger logger)
			: base(trackViewBase, layoutContainer, groupContainer)
		{
			this.m_modelManager = modelManager;
			this.m_commandManager = commandManager;
			this.m_eventsManager = eventsManager;
			this.m_logger = logger;
			IGanttTrackView ganttView = this.m_view as IGanttTrackView;
			ganttView.DataViewBoundsChanged += this.view_DataViewBoundsChanged;
			ganttView.SettingsClicked += this.ganttView_SettingsClicked;
			ganttView.AnnotationDialogResponse += this.ganttView_AnnotationDialogResponse;
			this.DataViewMouseController = new DataViewMouseEventController(ganttView.DataViewMouseEventHandler);
			TimeEvents timeEvents = this.m_eventsManager.TimeEventsCollection.GetTimeEvents(this.m_layoutContainer.CaptureId);
			if (timeEvents != null)
			{
				TimeEvents timeEvents2 = timeEvents;
				timeEvents2.DataViewBoundsChanged = (EventHandler)Delegate.Combine(timeEvents2.DataViewBoundsChanged, new EventHandler(this.timeEvents_DataViewBoundsChanged));
			}
			ganttView.ViewModel = (this.ViewModel = new GanttTrackViewModel(() => ganttView.DataViewHeight, () => ganttView.DataViewWidth, new Func<int, long>(ganttView.DataViewMouseEventHandler.ToTimestamp), new Func<long, int>(ganttView.DataViewMouseEventHandler.ToDataViewXAxisPoint), () => this.TooltipStringsModel, this.m_logger));
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0001A9F8 File Offset: 0x00018BF8
		private IGanttTrackView GanttTrackView
		{
			get
			{
				return this.m_view as IGanttTrackView;
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001AA08 File Offset: 0x00018C08
		internal void FocusOnElementAtPoint(DataViewPoint point)
		{
			Maybe<SeriesElementPair> elementAtPoint = this.ViewModel.GetElementAtPoint(point);
			elementAtPoint.Match(delegate(SeriesElementPair someSelection)
			{
				this.GanttTrackView.FocusOnElement(someSelection.Element);
			}, delegate
			{
			});
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001AA53 File Offset: 0x00018C53
		internal void OnElementSelected(Series selectedSeries, Element selectedElement)
		{
			this.ScrollToElement(selectedElement, false);
			EventHandler<ElementSelectedEventArgs> elementSelected = this.ElementSelected;
			if (elementSelected == null)
			{
				return;
			}
			elementSelected(this, new ElementSelectedEventArgs
			{
				ElementSeries = selectedSeries,
				Selected = selectedElement,
				SelectedElementCount = this.ViewModel.GetSelectedObjectCount()
			});
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001AA92 File Offset: 0x00018C92
		internal void OnMarkerSelected(Series selectedSeries, Marker selectedMarker)
		{
			EventHandler<MarkerSelectedEventArgs> markerSelected = this.MarkerSelected;
			if (markerSelected == null)
			{
				return;
			}
			markerSelected(this, new MarkerSelectedEventArgs
			{
				ElementSeries = selectedSeries,
				Selected = selectedMarker,
				SelectedObjectCount = this.ViewModel.GetSelectedObjectCount()
			});
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001AACC File Offset: 0x00018CCC
		internal void ShowAnnotationDialogForElementAtPoint(DataViewPoint location)
		{
			Maybe<SeriesElementPair> elementAtPoint = this.ViewModel.GetElementAtPoint(location);
			elementAtPoint.Match(delegate(SeriesElementPair seriesElementPair)
			{
				this.ShowElementAnnotationDialogForElement(seriesElementPair.Series, seriesElementPair.Element);
			}, delegate
			{
			});
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001AB18 File Offset: 0x00018D18
		private void ShowElementAnnotationDialogForElement(Series series, Element element)
		{
			ElementSelectedEventArgs elementSelectedEventArgs = new ElementSelectedEventArgs
			{
				Selected = element,
				ElementSeries = series
			};
			string elementLabelText = this.GetElementLabelText(series, element);
			this.GanttTrackView.ShowAnnotateDialog(elementSelectedEventArgs, elementLabelText);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001AB50 File Offset: 0x00018D50
		private string GetElementLabelText(Series series, Element element)
		{
			string text = "";
			if (series.IsAnnotation())
			{
				this.NameStringsModel.TryGetValue(element.LabelId, out text);
			}
			else if (element.AnnotationLink != null)
			{
				this.NameStringsModel.TryGetValue(element.AnnotationLink.LabelId, out text);
			}
			return text;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		internal void SetLassoSelectHighlightRegion(DataViewPoint start, DataViewPoint end)
		{
			if (this.IntersectsDataView(start, end))
			{
				this.ViewModel.SetHighlightRegion(start, end, HighlightRegionType.Lasso);
				return;
			}
			this.ViewModel.ClearHighlightRegion();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001ABCC File Offset: 0x00018DCC
		private bool IntersectsDataView(SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point begin, SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point end)
		{
			SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point point = new SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point(Math.Min(begin.X, end.X), Math.Min(begin.Y, end.Y));
			SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point point2 = new SDPClientFramework.Views.EventHandlers.MouseEventHandler.Point(Math.Max(begin.X, end.X), Math.Max(begin.Y, end.Y));
			return point2.Y >= 0 && point2.X >= 0 && point.Y <= this.GanttTrackView.DataViewHeight && point.X <= this.GanttTrackView.DataViewWidth;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001AC68 File Offset: 0x00018E68
		internal void SetZoomRangeHighlightRegion(DataViewPoint previousLocation, DataViewPoint currentLocation)
		{
			previousLocation.Y = 0;
			currentLocation.Y = this.GanttTrackView.DataViewHeight;
			this.ViewModel.SetHighlightRegion(previousLocation, currentLocation, HighlightRegionType.Zoom);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001AC9F File Offset: 0x00018E9F
		internal void PanGanttTrack(int delta)
		{
			this.GanttTrackView.Pan(delta);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001ACAD File Offset: 0x00018EAD
		internal void SetZoomRangeFromHighlightRegion()
		{
			this.ViewModel.HighlightRegion.Match(delegate(HighlightRegion someHighlightRegion)
			{
				this.GanttTrackView.SetZoomRange(someHighlightRegion.HighlightBegin.X, someHighlightRegion.HighlightEnd.X);
				this.ViewModel.ClearHighlightRegion();
			}, delegate
			{
			});
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001ACEC File Offset: 0x00018EEC
		private void ganttView_SettingsClicked(object sender, EventArgs e)
		{
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			ganttTrackView.OpenSettingsWindow();
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001AD0C File Offset: 0x00018F0C
		private void timeEvents_DataViewBoundsChanged(object sender, EventArgs e)
		{
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			TimeModel timeModel = this.m_modelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null)
			{
				ganttTrackView.SetDataViewBounds(timeModel.DataViewBoundsMin, timeModel.DataViewBoundsMax, timeModel.Dirty);
			}
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001AD5C File Offset: 0x00018F5C
		public void SetDataBounds(long min, long max)
		{
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			ganttTrackView.SetDataBounds(min, max);
			TimeModel timeModel = this.m_modelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null)
			{
				timeModel.SetDataBounds(min, max, true);
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001ADA8 File Offset: 0x00018FA8
		public void SetDataViewBounds(long min, long max)
		{
			TimeModel timeModel = this.m_modelManager.TimeModelCollection.GetTimeModel(this.m_layoutContainer.CaptureId);
			if (timeModel != null)
			{
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = (double)min;
				setDataViewBoundsCommand.Maximum = (double)max;
				setDataViewBoundsCommand.CaptureId = this.m_layoutContainer.CaptureId;
				this.m_commandManager.ExecuteCommand(setDataViewBoundsCommand);
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001AE08 File Offset: 0x00019008
		private void view_DataViewBoundsChanged(object sender, SetDataViewBoundsEventArgs e)
		{
			SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
			setDataViewBoundsCommand.Minimum = e.min;
			setDataViewBoundsCommand.Maximum = e.max;
			setDataViewBoundsCommand.Dirty = e.dirty;
			setDataViewBoundsCommand.CaptureId = this.m_layoutContainer.CaptureId;
			this.m_commandManager.ExecuteCommand(setDataViewBoundsCommand);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void AddMetric(uint metricId, string metricName, uint pid, bool isPreview, string tooltip, bool isCustom, Color? color)
		{
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void RemoveMetric(uint metricId, string metricName, uint pid, bool forceDeleteTrackIfEmpty, bool isPreview)
		{
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001AE5C File Offset: 0x0001905C
		public override int MetricCount()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001AE63 File Offset: 0x00019063
		public List<Series> Series
		{
			get
			{
				return this.ViewModel.Series;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0001AE70 File Offset: 0x00019070
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x0001AE90 File Offset: 0x00019090
		public int MinimumHeight
		{
			get
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				return ganttTrackView.MinimumHeight;
			}
			set
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				ganttTrackView.MinimumHeight = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0001AEB0 File Offset: 0x000190B0
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0001AED0 File Offset: 0x000190D0
		public Dictionary<uint, string> NameStringsModel
		{
			get
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				return ganttTrackView.NameStringsModel;
			}
			set
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				ganttTrackView.NameStringsModel = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0001AEF0 File Offset: 0x000190F0
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0001AF10 File Offset: 0x00019110
		public Dictionary<uint, string> TooltipStringsModel
		{
			get
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				return ganttTrackView.TooltipStringsModel;
			}
			set
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				ganttTrackView.TooltipStringsModel = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0001AF30 File Offset: 0x00019130
		public List<uint> StringIDsToRender
		{
			set
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				ganttTrackView.StringIDsToRender = new HashSet<uint>(value);
			}
		}

		// Token: 0x170001BC RID: 444
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0001AF58 File Offset: 0x00019158
		public string SettingsWindowName
		{
			set
			{
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				ganttTrackView.SettingsWindowName = value;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0001AF78 File Offset: 0x00019178
		internal IGanttTrackViewModel ViewModel { get; }

		// Token: 0x0600092D RID: 2349 RVA: 0x0001AF80 File Offset: 0x00019180
		public void SetColorsModel(Dictionary<uint, Color> model)
		{
			uint hashCode = (uint)"Annotations".GetHashCode();
			if (!model.ContainsKey(hashCode))
			{
				model.Add(hashCode, this.m_annotationColor);
			}
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			ganttTrackView.SetColorsModel(model);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001AFC4 File Offset: 0x000191C4
		public Dictionary<uint, Color> GetColorsModel()
		{
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			return ganttTrackView.GetColorsModel();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001AFE4 File Offset: 0x000191E4
		public void Invalidate()
		{
			Stack<Series> stack = new Stack<Series>(this.Series);
			while (stack.Count > 0)
			{
				Series series = stack.Pop();
				for (int i = 1; i < series.Elements.Count; i++)
				{
					Element element = series.Elements[i - 1];
					Element element2 = series.Elements[i];
					if (element.Start > element2.Start)
					{
						this.m_logger.LogWarning(string.Concat(new string[]
						{
							series.Name,
							": block #",
							i.ToString(),
							" start out of order ",
							element2.Start.ToString()
						}));
					}
					if (element.End > element2.End)
					{
						this.m_logger.LogWarning(string.Concat(new string[]
						{
							series.Name,
							": block #",
							i.ToString(),
							" end out of order ",
							element2.End.ToString()
						}));
					}
					if (element.End > element2.Start)
					{
						this.m_logger.LogWarning(string.Concat(new string[]
						{
							series.Name,
							": block #",
							i.ToString(),
							" overlapping blocks ",
							element2.Start.ToString()
						}));
					}
				}
				foreach (Series series2 in series.Children)
				{
					stack.Push(series2);
				}
			}
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			ganttTrackView.Invalidate();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001B1B4 File Offset: 0x000193B4
		private void ganttView_AnnotationDialogResponse(object sender, AnnotationDialogResponseArgs args)
		{
			if (!args.ResponseOk)
			{
				return;
			}
			Series series = this.Series.FirstOrDefault((Series s) => s.IsAnnotation());
			if (series == null)
			{
				series = new Series();
				series.Name = "Annotations";
				series.LastYOffset = args.ElementSeries.LastYOffset;
				this.Series.Add(series);
			}
			uint hashCode = (uint)args.Annotation.GetHashCode();
			if (!this.NameStringsModel.ContainsKey(hashCode))
			{
				this.NameStringsModel.Add(hashCode, args.Annotation);
				IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
				if (ganttTrackView.StringIDsToRender != null)
				{
					ganttTrackView.StringIDsToRender.Add(hashCode);
				}
			}
			if (args.ElementSeries.IsAnnotation())
			{
				args.Selected.LabelId = hashCode;
				args.Selected.InspectorStringId = hashCode;
				args.Selected.TooltipId = hashCode;
			}
			else if (args.Selected.AnnotationLink != null)
			{
				args.Selected.AnnotationLink.LabelId = hashCode;
				args.Selected.AnnotationLink.InspectorStringId = hashCode;
				args.Selected.AnnotationLink.TooltipId = hashCode;
			}
			else
			{
				Element element = new Element();
				element.BlockId = args.Selected.BlockId;
				element.Start = args.Selected.Start;
				element.End = args.Selected.End;
				element.LabelId = hashCode;
				element.ColorId = (uint)"Annotations".GetHashCode();
				element.InspectorStringId = hashCode;
				element.TooltipId = hashCode;
				element.AnnotationLink = args.Selected;
				args.Selected.AnnotationLink = element;
				series.Elements.Add(element);
				series.Elements.Sort((Element a, Element b) => a.BlockId.CompareTo(b.BlockId));
			}
			this.Invalidate();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001B3A0 File Offset: 0x000195A0
		public void AddConnection(Connection connection)
		{
			IGanttTrackView ganttTrackView = this.m_view as IGanttTrackView;
			ganttTrackView.AddConnection(connection);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001B3C0 File Offset: 0x000195C0
		public void SetSelected(uint id, bool maximize)
		{
			Element element = new Element();
			element.BlockId = id;
			Series series = null;
			foreach (Series series2 in this.Series)
			{
				int num = this.FindElement(series2, element, out series, true);
				if (num >= 0)
				{
					Element element2 = series.Elements[num];
					this.ViewModel.SelectElement(new SeriesElementPair
					{
						Series = series2,
						Element = element2
					}, SelectionType.SingleSelect);
					this.ScrollToElement(element2, maximize);
					break;
				}
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001B46C File Offset: 0x0001966C
		public bool SetSelected(Series series, uint id, bool maximize)
		{
			if (series == null)
			{
				return false;
			}
			Element element = new Element();
			element.BlockId = id;
			Series series2 = null;
			int num = this.FindElement(series, element, out series2, false);
			if (num >= 0)
			{
				Element element2 = series2.Elements[num];
				this.ViewModel.SelectElement(new SeriesElementPair
				{
					Series = series,
					Element = element2
				}, SelectionType.SingleSelect);
				this.ScrollToElement(element2, maximize);
				return true;
			}
			return false;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001B4D8 File Offset: 0x000196D8
		private int FindElement(Series seriesSearch, Element elementSearch, out Series seriesFound, bool recursive = true)
		{
			int num = -1;
			seriesFound = null;
			num = seriesSearch.Elements.BinarySearch(elementSearch, Comparer<Element>.Create((Element a, Element b) => a.BlockId.CompareTo(b.BlockId)));
			if (num >= 0)
			{
				seriesFound = seriesSearch;
				return num;
			}
			if (!recursive)
			{
				return num;
			}
			foreach (Series series in seriesSearch.Children)
			{
				num = this.FindElement(series, elementSearch, out seriesFound, true);
				if (num >= 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001B57C File Offset: 0x0001977C
		private void ScrollToElement(Element e, bool maximize)
		{
			TimeModel timeModel = this.m_modelManager.TimeModelCollection.GetTimeModel(base.CaptureId);
			if (timeModel != null)
			{
				SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
				setDataViewBoundsCommand.Minimum = timeModel.DataViewBoundsMin;
				setDataViewBoundsCommand.Maximum = timeModel.DataViewBoundsMax;
				if (maximize)
				{
					double num = setDataViewBoundsCommand.Maximum - setDataViewBoundsCommand.Minimum;
					double num2 = (double)Math.Max(e.End - e.Start, 1L);
					double num3 = num2 * 20.0;
					if (num > num3)
					{
						double num4 = num / 2.0;
						setDataViewBoundsCommand.Minimum += num4 - num3 / 2.0;
						setDataViewBoundsCommand.Maximum = setDataViewBoundsCommand.Minimum + num3;
						setDataViewBoundsCommand.Dirty = true;
					}
					else if (num < num2)
					{
						setDataViewBoundsCommand.Minimum = (double)e.Start;
						setDataViewBoundsCommand.Maximum = (double)e.End;
						setDataViewBoundsCommand.Dirty = true;
					}
					if ((double)e.Start < setDataViewBoundsCommand.Minimum || (double)e.End > setDataViewBoundsCommand.Maximum)
					{
						double num5 = (setDataViewBoundsCommand.Maximum - setDataViewBoundsCommand.Minimum) / 2.0;
						double num6 = (double)e.Start + (double)(e.End - e.Start) / 2.0;
						setDataViewBoundsCommand.Minimum = num6 - num5;
						setDataViewBoundsCommand.Maximum = num6 + num5;
						setDataViewBoundsCommand.Dirty = true;
					}
				}
				else
				{
					if ((double)e.Start < timeModel.DataViewBoundsMin)
					{
						setDataViewBoundsCommand.Maximum -= timeModel.DataViewBoundsMin - (double)e.Start;
						setDataViewBoundsCommand.Minimum = (double)e.Start;
					}
					if ((double)e.End > timeModel.DataViewBoundsMax)
					{
						setDataViewBoundsCommand.Minimum += (double)e.End - timeModel.DataViewBoundsMax;
						setDataViewBoundsCommand.Maximum = (double)e.End;
					}
				}
				if (setDataViewBoundsCommand.Minimum != timeModel.DataViewBoundsMin || setDataViewBoundsCommand.Maximum != timeModel.DataViewBoundsMax)
				{
					setDataViewBoundsCommand.CaptureId = base.CaptureId;
					this.m_commandManager.ExecuteCommand(setDataViewBoundsCommand);
				}
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public override bool ContainsMetric(MetricDesc desc)
		{
			return false;
		}

		// Token: 0x04000807 RID: 2055
		public EventHandler<ElementSelectedEventArgs> ElementSelected;

		// Token: 0x04000808 RID: 2056
		public EventHandler<MarkerSelectedEventArgs> MarkerSelected;

		// Token: 0x0400080A RID: 2058
		public const string Annotations = "Annotations";

		// Token: 0x0400080B RID: 2059
		private readonly Color m_annotationColor = new Color(0.36, 0.37, 0.38);

		// Token: 0x0400080C RID: 2060
		private readonly IModelManager m_modelManager;

		// Token: 0x0400080D RID: 2061
		private readonly ICommandManager m_commandManager;

		// Token: 0x0400080E RID: 2062
		private readonly EventsManager m_eventsManager;

		// Token: 0x0400080F RID: 2063
		private readonly ILogger m_logger;

		// Token: 0x04000810 RID: 2064
		internal readonly DataViewMouseEventController DataViewMouseController;
	}
}
