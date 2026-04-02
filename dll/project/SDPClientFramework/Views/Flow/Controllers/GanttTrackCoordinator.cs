using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sdp;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.Views.EventHandlers;
using SDPClientFramework.Views.EventHandlers.KeyboardEventHandler;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;
using SDPClientFramework.Views.Flow.ViewModels.GanttTrack;

namespace SDPClientFramework.Views.Flow.Controllers
{
	// Token: 0x02000037 RID: 55
	internal class GanttTrackCoordinator
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00005064 File Offset: 0x00003264
		public GanttTrackCoordinator(IGroupLayoutView groupLayoutView, EventsManager eventsManager, ILogger logger)
		{
			GanttTrackCoordinator <>4__this = this;
			this.m_groupLayoutView = groupLayoutView;
			this.m_eventsManager = eventsManager;
			this.m_logger = logger;
			TimeEventsCollection timeEventsCollection = SdpApp.EventsManager.TimeEventsCollection;
			timeEventsCollection.InteractionModeChanged = (EventHandler<InteractionModeChangedEventArgs>)Delegate.Combine(timeEventsCollection.InteractionModeChanged, new EventHandler<InteractionModeChangedEventArgs>(this.timeEvents_InteractionModeChanged));
			groupLayoutView.KeyPressed += delegate(object _s, KeyPressedEventArgs e)
			{
				<>4__this.GroupLayoutView_OnKeyPressed(groupLayoutView, e);
			};
			groupLayoutView.KeyReleased += delegate(object _s, KeyPressedEventArgs e)
			{
				<>4__this.GroupLayoutView_OnKeyReleased(groupLayoutView, e);
			};
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000511F File Offset: 0x0000331F
		public void timeEvents_InteractionModeChanged(object sender, InteractionModeChangedEventArgs args)
		{
			GanttTrackCoordinator.m_currentEnabledButton = args.InteractionModeButtonType;
			this.SetGroupLayoutInteractionMode(this.m_groupLayoutView, args.InteractionModeButtonType);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005140 File Offset: 0x00003340
		public void AddGanttTrackController(GanttTrackController ganttController)
		{
			IGanttTrackView view = ganttController.View as IGanttTrackView;
			if (this.m_ganttTrackControllers.IsEmpty)
			{
				this.m_groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(GanttTrackCoordinator.m_currentEnabledButton);
			}
			this.m_ganttTrackControllers.TryAdd(view, ganttController);
			ganttController.DataViewMouseController.MouseButtonClicked += delegate(object s, DataViewMouseClickEventArgs e)
			{
				this.GanttView_OnClicked(view, e);
			};
			ganttController.DataViewMouseController.DragBegin += delegate(object s, DataViewDragEventArgs e)
			{
				this.GanttView_OnDragBegin(view, e);
			};
			ganttController.DataViewMouseController.DragMoved += delegate(object s, DataViewDragEventArgs e)
			{
				this.GanttView_OnDragMove(view, e);
			};
			ganttController.DataViewMouseController.DragEnded += delegate(object s, DataViewDragEventArgs e)
			{
				this.GantView_OnDragEnded(view, e);
			};
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000051FC File Offset: 0x000033FC
		public void RemoveGanttTrackController(GanttTrackController ganttController)
		{
			GanttTrackController ganttTrackController;
			this.m_ganttTrackControllers.TryRemove(ganttController.View as IGanttTrackView, out ganttTrackController);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005224 File Offset: 0x00003424
		private void GroupLayoutView_OnKeyPressed(IGroupLayoutView groupLayoutView, KeyPressedEventArgs e)
		{
			e.Handled = true;
			switch (e.Key)
			{
			case Key.Left:
			case Key.Right:
				this.MoveFirstSeriesSelection(this.ToMoveDirection(e.Key), this.ToSelectionType(e.Modifer));
				return;
			case Key.Up:
			case Key.Down:
				e.Handled = true;
				return;
			case Key._1:
				this.SetGroupLayoutInteractionMode(groupLayoutView, InteractionMode.Select);
				return;
			case Key._2:
				this.SetGroupLayoutInteractionMode(groupLayoutView, InteractionMode.Pan);
				return;
			case Key._3:
				this.SetGroupLayoutInteractionMode(groupLayoutView, InteractionMode.Zoom);
				return;
			case Key.Shift:
				this.m_shiftResetMode.Match(delegate(Maybe<InteractionMode> some)
				{
				}, delegate
				{
					this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState someDragState)
					{
					}, delegate
					{
						this.m_shiftResetMode = new Maybe<Maybe<InteractionMode>>.Some(this.m_groupLayoutView.InteractionMode);
						this.m_groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(InteractionMode.Select);
					});
				});
				e.Handled = true;
				return;
			case Key.Page_Up:
			case Key.Page_Down:
				e.Handled = true;
				return;
			default:
				e.Handled = false;
				return;
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005304 File Offset: 0x00003504
		private void GroupLayoutView_OnKeyReleased(IGroupLayoutView groupLayoutView, KeyPressedEventArgs e)
		{
			e.Handled = false;
			switch (e.Key)
			{
			case Key.Shift:
				this.m_shiftResetMode.Bind(delegate(Maybe<InteractionMode> somePrevious)
				{
					Action <>9__7;
					somePrevious.Bind(delegate(InteractionMode previousState)
					{
						Maybe<GanttTrackCoordinator.GanttTrackDragState> state = this.m_state;
						Action<GanttTrackCoordinator.GanttTrackDragState> action = delegate(GanttTrackCoordinator.GanttTrackDragState someDragState)
						{
						};
						Action action2;
						if ((action2 = <>9__7) == null)
						{
							action2 = (<>9__7 = delegate
							{
								this.m_groupLayoutView.InteractionMode = somePrevious;
							});
						}
						state.Match(action, action2);
					});
				});
				this.m_shiftResetMode = new Maybe<Maybe<InteractionMode>>.None();
				return;
			case Key.Page_Up:
				this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState some)
				{
					some.HandleOnPageUp(e);
				}, delegate
				{
				});
				return;
			case Key.Page_Down:
				this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState some)
				{
					some.HandleOnPageDown(e);
				}, delegate
				{
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000053E4 File Offset: 0x000035E4
		private void SetGroupLayoutInteractionMode(IGroupLayoutView groupLayoutView, InteractionMode mode)
		{
			Func<Maybe<InteractionMode>, Maybe<InteractionMode>> <>9__2;
			Func<Maybe<InteractionMode>> <>9__3;
			this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState someDragState)
			{
			}, delegate
			{
				Maybe<Maybe<InteractionMode>> shiftResetMode = this.m_shiftResetMode;
				Func<Maybe<InteractionMode>, Maybe<InteractionMode>> func;
				if ((func = <>9__2) == null)
				{
					func = (<>9__2 = (Maybe<InteractionMode> shiftPressed) => groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(InteractionMode.Select));
				}
				Func<Maybe<InteractionMode>> func2;
				if ((func2 = <>9__3) == null)
				{
					func2 = (<>9__3 = () => groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(mode));
				}
				shiftResetMode.Match<Maybe<InteractionMode>>(func, func2);
			});
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005444 File Offset: 0x00003644
		private void MoveFirstSeriesSelection(MoveDirection direction, SelectionType selectionType)
		{
			if (this.GetNumberSeriesSelected() == 1)
			{
				GanttTrackController first = this.m_ganttTrackControllers.Values.First((GanttTrackController controller) => controller.ViewModel.GetSelectedObjectCount() > 0);
				first.ViewModel.MoveSelection(direction, selectionType);
				Action<SeriesElementPair> <>9__3;
				Action<SeriesMarkerPair> <>9__4;
				first.ViewModel.GetLastSelected().Match(delegate(SeriesGanttObjectPair selection)
				{
					Action<SeriesElementPair> action;
					if ((action = <>9__3) == null)
					{
						action = (<>9__3 = delegate(SeriesElementPair elementSelection)
						{
							first.OnElementSelected(elementSelection.Series, elementSelection.Element);
						});
					}
					Action<SeriesMarkerPair> action2;
					if ((action2 = <>9__4) == null)
					{
						action2 = (<>9__4 = delegate(SeriesMarkerPair markerSelection)
						{
							first.OnMarkerSelected(markerSelection.Series, markerSelection.Marker);
						});
					}
					selection.Match(action, action2);
				}, delegate
				{
					throw new ApplicationException("No selection count found when selection count was 1");
				});
				this.OnSelectionUpdated();
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000054F0 File Offset: 0x000036F0
		private int GetNumberSeriesSelected()
		{
			return this.m_ganttTrackControllers.Sum((KeyValuePair<IGanttTrackView, GanttTrackController> controller) => controller.Value.ViewModel.GetSelectedSeriesCount());
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000551C File Offset: 0x0000371C
		private MoveDirection ToMoveDirection(Key key)
		{
			if (key == Key.Left)
			{
				return MoveDirection.Left;
			}
			if (key != Key.Right)
			{
				throw new ArgumentException(string.Format("No conversion to move direction for {0}", key));
			}
			return MoveDirection.Right;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005540 File Offset: 0x00003740
		private SelectionType ToSelectionType(KeyModifierFlag flag)
		{
			if (flag.HasModifer(KeyModifierFlag.Shift))
			{
				return SelectionType.RangeSelect;
			}
			return SelectionType.SingleSelect;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005550 File Offset: 0x00003750
		private void GanttView_OnClicked(IGanttTrackView ganttTrackView, DataViewMouseClickEventArgs e)
		{
			GanttTrackController controllerForView = this.GetControllerForView(ganttTrackView);
			if (e.IsShiftLeftClick())
			{
				this.SelectGanttObjectAtPoint(controllerForView, e.Location, SelectionType.RangeSelect);
				return;
			}
			if (e.IsLeftClick())
			{
				this.SelectGanttObjectAtPoint(controllerForView, e.Location, SelectionType.SingleSelect);
				return;
			}
			if (e.IsRightClick())
			{
				controllerForView.ShowAnnotationDialogForElementAtPoint(e.Location);
				return;
			}
			if (e.IsDoubleLeftClick())
			{
				controllerForView.FocusOnElementAtPoint(e.Location);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000055BC File Offset: 0x000037BC
		private void SelectGanttObjectAtPoint(GanttTrackController controller, DataViewPoint location, SelectionType selectionType)
		{
			Maybe<SeriesGanttObjectPair> ganttObjectAtPoint = controller.ViewModel.GetGanttObjectAtPoint(location);
			Action<SeriesElementPair> <>9__2;
			Action<SeriesMarkerPair> <>9__3;
			Func<GanttTrackController, bool> <>9__4;
			ganttObjectAtPoint.Match(delegate(SeriesGanttObjectPair selection)
			{
				Action<SeriesElementPair> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(SeriesElementPair elementSelection)
					{
						controller.ViewModel.SelectElement(elementSelection, selectionType);
						controller.OnElementSelected(elementSelection.Series, elementSelection.Element);
					});
				}
				Action<SeriesMarkerPair> action2;
				if ((action2 = <>9__3) == null)
				{
					action2 = (<>9__3 = delegate(SeriesMarkerPair markerSelection)
					{
						controller.ViewModel.SelectMarker(markerSelection, selectionType);
						controller.OnMarkerSelected(markerSelection.Series, markerSelection.Marker);
					});
				}
				selection.Match(action, action2);
				if (selectionType == SelectionType.SingleSelect)
				{
					IEnumerable<GanttTrackController> values = this.m_ganttTrackControllers.Values;
					Func<GanttTrackController, bool> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = (GanttTrackController c) => c != controller);
					}
					IEnumerable<GanttTrackController> enumerable = values.Where(func);
					GanttTrackCoordinator.ClearSelections(enumerable);
				}
			}, delegate
			{
				GanttTrackCoordinator.ClearSelections(this.m_ganttTrackControllers.Values);
			});
			this.OnSelectionUpdated();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000561C File Offset: 0x0000381C
		private static void ClearSelections(IEnumerable<GanttTrackController> controllers)
		{
			foreach (IGanttTrackViewModel ganttTrackViewModel in controllers.Select((GanttTrackController c) => c.ViewModel))
			{
				ganttTrackViewModel.ClearSelections();
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005688 File Offset: 0x00003888
		private void GanttView_OnDragBegin(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
		{
			GanttTrackController controllerForView = this.GetControllerForView(ganttTrackView);
			if (e.IsShiftLeftDrag())
			{
				this.TransitionTo(new Maybe<GanttTrackCoordinator.GanttTrackDragState>.Some(new GanttTrackCoordinator.ResetState(this, GanttTrackCoordinator.SelectModeState(this))));
				this.m_groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(InteractionMode.Select);
			}
			else if (e.IsLeftDrag())
			{
				this.TransitionTo(new Maybe<GanttTrackCoordinator.GanttTrackDragState>.Some(this.CreateDragState()));
			}
			else if (e.IsShiftRightDrag())
			{
				this.TransitionTo(new Maybe<GanttTrackCoordinator.GanttTrackDragState>.Some(new GanttTrackCoordinator.ResetState(this, GanttTrackCoordinator.ZoomModeState(this))));
				this.m_groupLayoutView.InteractionMode = new Maybe<InteractionMode>.Some(InteractionMode.Zoom);
			}
			this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState some)
			{
				some.HandleOnDragBegin(ganttTrackView, e);
			}, delegate
			{
			});
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005778 File Offset: 0x00003978
		private void GanttView_OnDragMove(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
		{
			this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState some)
			{
				some.HandleOnDragMoved(ganttTrackView, e);
			}, delegate
			{
			});
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000057D0 File Offset: 0x000039D0
		private void GantView_OnDragEnded(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
		{
			this.m_state.Match(delegate(GanttTrackCoordinator.GanttTrackDragState some)
			{
				some.HandleOnDragEnd(ganttTrackView, e);
			}, delegate
			{
			});
			this.TransitionTo(new Maybe<GanttTrackCoordinator.GanttTrackDragState>.None());
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005832 File Offset: 0x00003A32
		private void TransitionTo(Maybe<GanttTrackCoordinator.GanttTrackDragState> state)
		{
			this.m_state = state;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000583C File Offset: 0x00003A3C
		private GanttTrackController GetControllerForView(IGanttTrackView view)
		{
			GanttTrackController ganttTrackController = null;
			this.m_ganttTrackControllers.TryGetValue(view, out ganttTrackController);
			return ganttTrackController;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000585B File Offset: 0x00003A5B
		private int GetTotalElementsSelected()
		{
			return this.m_ganttTrackControllers.Values.Select((GanttTrackController controller) => controller.ViewModel.GetSelectedObjectCount()).DefaultIfEmpty(0).Sum();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005897 File Offset: 0x00003A97
		private int GetTotalMarkersSelected()
		{
			return this.m_ganttTrackControllers.Values.Select((GanttTrackController controller) => controller.ViewModel.GetSelectedMarkerCount()).DefaultIfEmpty(0).Sum();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000058D3 File Offset: 0x00003AD3
		private GanttTrackCoordinator.GanttTrackDragState CreateDragState()
		{
			return this.m_groupLayoutView.InteractionMode.Match<GanttTrackCoordinator.GanttTrackDragState>((InteractionMode interactionMode) => this.CreateDragState(interactionMode), delegate
			{
				throw new ApplicationException("Interaction mode should always be set if there are any ganttrackcontrollers");
			});
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005910 File Offset: 0x00003B10
		private GanttTrackCoordinator.GanttTrackDragState CreateDragState(InteractionMode interactionMode)
		{
			switch (interactionMode)
			{
			case InteractionMode.Select:
				return GanttTrackCoordinator.SelectModeState(this);
			case InteractionMode.Pan:
				return new GanttTrackCoordinator.PanModeState(this);
			case InteractionMode.Zoom:
				return GanttTrackCoordinator.ZoomModeState(this);
			default:
				throw new ArgumentException("Unknown interaction mode enum");
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005948 File Offset: 0x00003B48
		private static GanttTrackCoordinator.MultiTrackDragState SelectModeState(GanttTrackCoordinator mediator)
		{
			return new GanttTrackCoordinator.MultiTrackDragState(mediator, delegate(GanttTrackController controller, DataViewPoint start, DataViewPoint end)
			{
				controller.SetLassoSelectHighlightRegion(start, end);
			}, delegate(GanttTrackController controller, DataViewPoint start, DataViewPoint end)
			{
				controller.SetLassoSelectHighlightRegion(start, end);
			}, delegate(GanttTrackController _)
			{
				ICollection<GanttTrackController> values = mediator.m_ganttTrackControllers.Values;
				foreach (GanttTrackController ganttTrackController in values)
				{
					ganttTrackController.ViewModel.SelectElementsInHighlightRegion();
				}
				int totalElementsSelected = mediator.GetTotalElementsSelected();
				if (totalElementsSelected == 1)
				{
					GanttTrackController selectedController = values.First((GanttTrackController c) => c.ViewModel.GetSelectedObjectCount() != 0);
					Action<SeriesElementPair> <>9__6;
					Action<SeriesMarkerPair> <>9__7;
					selectedController.ViewModel.GetLastSelected().Match(delegate(SeriesGanttObjectPair selection)
					{
						Action<SeriesElementPair> action;
						if ((action = <>9__6) == null)
						{
							action = (<>9__6 = delegate(SeriesElementPair elementSelection)
							{
								selectedController.OnElementSelected(elementSelection.Series, elementSelection.Element);
							});
						}
						Action<SeriesMarkerPair> action2;
						if ((action2 = <>9__7) == null)
						{
							action2 = (<>9__7 = delegate(SeriesMarkerPair markerSelection)
							{
								selectedController.OnMarkerSelected(markerSelection.Series, markerSelection.Marker);
							});
						}
						selection.Match(action, action2);
					}, delegate
					{
						throw new ApplicationException("No selection count found when selection count was 1");
					});
				}
				mediator.OnSelectionUpdated();
			});
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000059B8 File Offset: 0x00003BB8
		private void OnSelectionUpdated()
		{
			int totalElementsSelected = this.GetTotalElementsSelected();
			if (totalElementsSelected == 0)
			{
				this.ClearInspectorView();
				return;
			}
			if (totalElementsSelected == 1 && this.GetTotalMarkersSelected() == 1)
			{
				this.UpdateInspectorViewForSingleSelection();
				return;
			}
			if (totalElementsSelected > 1)
			{
				this.UpdateInspectorViewForMultiSelection();
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000059F4 File Offset: 0x00003BF4
		private void UpdateInspectorViewForSingleSelection()
		{
			try
			{
				InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = (from controller in this.m_ganttTrackControllers.Values
					where controller.ViewModel.GetSelectedObjectCount() > 0
					select controller.ViewModel.SingleSelectInspectorViewModel).First<InspectorViewModel>().ToEventArgs();
				this.m_eventsManager.Raise<InspectorViewDisplayEventArgs>(this.m_eventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
				this.m_eventsManager.Raise<MultiSelectionActivationEventArgs>(this.m_eventsManager.InspectorViewEvents.MultiSelection, this, new MultiSelectionActivationEventArgs(false)
				{
					Description = "Gantt Multi Select"
				});
			}
			catch (ArgumentException ex)
			{
				this.DisplaySelectionError(ex.Message);
				this.m_logger.LogError(ex.Message);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005ADC File Offset: 0x00003CDC
		private void UpdateInspectorViewForMultiSelection()
		{
			try
			{
				InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = (from controller in this.m_ganttTrackControllers.Values
					where controller.ViewModel.GetSelectedObjectCount() > 0
					select controller.ViewModel.MultiSelectInspectorViewModel).Aggregate(new Func<InspectorViewModel, InspectorViewModel, InspectorViewModel>(InspectorViewModelBuilder.BuildCumulative)).ToEventArgs();
				this.m_eventsManager.Raise<InspectorViewDisplayEventArgs>(this.m_eventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
				this.m_eventsManager.Raise<MultiSelectionActivationEventArgs>(this.m_eventsManager.InspectorViewEvents.MultiSelection, this, new MultiSelectionActivationEventArgs(true)
				{
					Description = "Gantt Multi Select"
				});
			}
			catch (ArgumentException ex)
			{
				this.DisplaySelectionError(ex.Message);
				this.m_logger.LogError(ex.Message);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005BD0 File Offset: 0x00003DD0
		private void DisplaySelectionError(string error)
		{
			SdpPropertyDescriptor<string> sdpPropertyDescriptor = new SdpPropertyDescriptor<string>("Error", typeof(string), "An error occured while calculating the selection statistics", "Selection Error", error, true);
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs
			{
				Content = new PropertyGridDescriptionObject(new List<PropertyDescriptor> { sdpPropertyDescriptor }),
				Description = "Error Selection"
			};
			this.m_eventsManager.Raise<InspectorViewDisplayEventArgs>(this.m_eventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005C44 File Offset: 0x00003E44
		private void ClearInspectorView()
		{
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs
			{
				Content = new PropertyGridDescriptionObject(new List<PropertyDescriptor>())
			};
			this.m_eventsManager.Raise<InspectorViewDisplayEventArgs>(this.m_eventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
			this.m_eventsManager.Raise<MultiSelectionActivationEventArgs>(this.m_eventsManager.InspectorViewEvents.MultiSelection, this, new MultiSelectionActivationEventArgs(false)
			{
				Description = "Gantt Multi Select"
			});
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005CB4 File Offset: 0x00003EB4
		private static GanttTrackCoordinator.MultiTrackDragState ZoomModeState(GanttTrackCoordinator mediator)
		{
			return new GanttTrackCoordinator.MultiTrackDragState(mediator, delegate(GanttTrackController controller, DataViewPoint start, DataViewPoint end)
			{
				controller.SetZoomRangeHighlightRegion(start, end);
			}, delegate(GanttTrackController controller, DataViewPoint start, DataViewPoint end)
			{
				controller.SetZoomRangeHighlightRegion(start, end);
			}, delegate(GanttTrackController controller)
			{
				controller.SetZoomRangeFromHighlightRegion();
				IEnumerable<GanttTrackController> values = mediator.m_ganttTrackControllers.Values;
				Func<GanttTrackController, bool> <>9__3;
				Func<GanttTrackController, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = (GanttTrackController c) => c != controller);
				}
				foreach (GanttTrackController ganttTrackController in values.Where(func))
				{
					ganttTrackController.ViewModel.ClearHighlightRegion();
				}
			});
		}

		// Token: 0x040000F6 RID: 246
		private readonly ILogger m_logger;

		// Token: 0x040000F7 RID: 247
		private readonly ConcurrentDictionary<IGanttTrackView, GanttTrackController> m_ganttTrackControllers = new ConcurrentDictionary<IGanttTrackView, GanttTrackController>();

		// Token: 0x040000F8 RID: 248
		private readonly IGroupLayoutView m_groupLayoutView;

		// Token: 0x040000F9 RID: 249
		private readonly EventsManager m_eventsManager;

		// Token: 0x040000FA RID: 250
		private Maybe<GanttTrackCoordinator.GanttTrackDragState> m_state = new Maybe<GanttTrackCoordinator.GanttTrackDragState>.None();

		// Token: 0x040000FB RID: 251
		private static InteractionMode m_currentEnabledButton = InteractionMode.Pan;

		// Token: 0x040000FC RID: 252
		private Maybe<Maybe<InteractionMode>> m_shiftResetMode = new Maybe<Maybe<InteractionMode>>.None();

		// Token: 0x02000343 RID: 835
		internal abstract class GanttTrackDragState
		{
			// Token: 0x0600110D RID: 4365 RVA: 0x00034BE1 File Offset: 0x00032DE1
			public GanttTrackDragState(GanttTrackCoordinator mediator)
			{
				this.m_mediator = mediator;
			}

			// Token: 0x0600110E RID: 4366
			public abstract void HandleOnDragBegin(IGanttTrackView ganttTrackView, DataViewDragEventArgs args);

			// Token: 0x0600110F RID: 4367
			public abstract void HandleOnDragMoved(IGanttTrackView ganttTrackView, DataViewDragEventArgs args);

			// Token: 0x06001110 RID: 4368
			public abstract void HandleOnDragEnd(IGanttTrackView ganttTrackView, DataViewDragEventArgs args);

			// Token: 0x06001111 RID: 4369 RVA: 0x00008AEF File Offset: 0x00006CEF
			public virtual void HandleOnPageDown(KeyPressedEventArgs args)
			{
			}

			// Token: 0x06001112 RID: 4370 RVA: 0x00008AEF File Offset: 0x00006CEF
			public virtual void HandleOnPageUp(KeyPressedEventArgs args)
			{
			}

			// Token: 0x04000B6B RID: 2923
			protected GanttTrackCoordinator m_mediator;
		}

		// Token: 0x02000344 RID: 836
		// (Invoke) Token: 0x06001114 RID: 4372
		internal delegate void DragMoveStrategy(GanttTrackController controller, DataViewPoint dragStart, DataViewPoint dragEnd);

		// Token: 0x02000345 RID: 837
		internal class MultiTrackDragState : GanttTrackCoordinator.GanttTrackDragState
		{
			// Token: 0x06001117 RID: 4375 RVA: 0x00034BF0 File Offset: 0x00032DF0
			public MultiTrackDragState(GanttTrackCoordinator mediator, GanttTrackCoordinator.DragMoveStrategy dragBeginStrategy, GanttTrackCoordinator.DragMoveStrategy dragMovedStrategy, Action<GanttTrackController> dragEndedStrategy)
				: base(mediator)
			{
				this.m_dragBeginStrategy = dragBeginStrategy;
				this.m_dragMovedStrategy = dragMovedStrategy;
				this.m_dragEndedStrategy = dragEndedStrategy;
			}

			// Token: 0x06001118 RID: 4376 RVA: 0x00034C10 File Offset: 0x00032E10
			public override void HandleOnDragBegin(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.m_globalDragStart = ganttTrackView.DataViewMouseEventHandler.ToGlobal(e.PreviousLocation);
				this.m_globalDragCurrent = ganttTrackView.DataViewMouseEventHandler.ToGlobal(e.CurrentLocation);
				this.m_startScrollPos = this.m_mediator.m_groupLayoutView.CurrentScrollPos;
				GanttTrackController controllerForView = this.m_mediator.GetControllerForView(ganttTrackView);
				this.m_dragBeginStrategy(controllerForView, e.PreviousLocation, e.CurrentLocation);
				this.m_globalDragCurrent = e.CurrentLocation;
				e.Handled = true;
			}

			// Token: 0x06001119 RID: 4377 RVA: 0x00034C9C File Offset: 0x00032E9C
			public override void HandleOnDragMoved(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				Point point = ganttTrackView.DataViewMouseEventHandler.ToGlobal(e.CurrentLocation);
				this.m_globalDragCurrent = point;
				this.dragUntil(point);
				e.Handled = true;
			}

			// Token: 0x0600111A RID: 4378 RVA: 0x00034CD0 File Offset: 0x00032ED0
			public override void HandleOnDragEnd(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				GanttTrackController controllerForView = this.m_mediator.GetControllerForView(ganttTrackView);
				this.m_dragEndedStrategy(controllerForView);
			}

			// Token: 0x0600111B RID: 4379 RVA: 0x00034CF6 File Offset: 0x00032EF6
			public override void HandleOnPageDown(KeyPressedEventArgs args)
			{
				this.dragUntil(this.m_globalDragCurrent);
				args.Handled = true;
			}

			// Token: 0x0600111C RID: 4380 RVA: 0x00034CF6 File Offset: 0x00032EF6
			public override void HandleOnPageUp(KeyPressedEventArgs args)
			{
				this.dragUntil(this.m_globalDragCurrent);
				args.Handled = true;
			}

			// Token: 0x0600111D RID: 4381 RVA: 0x00034D0C File Offset: 0x00032F0C
			private void dragUntil(Point globalDragCurrent)
			{
				int num = this.m_startScrollPos - this.m_mediator.m_groupLayoutView.CurrentScrollPos;
				Point point = new Point(this.m_globalDragStart.X, this.m_globalDragStart.Y + num);
				foreach (KeyValuePair<IGanttTrackView, GanttTrackController> keyValuePair in this.m_mediator.m_ganttTrackControllers)
				{
					IGanttTrackView key = keyValuePair.Key;
					GanttTrackController value = keyValuePair.Value;
					DataViewPoint dataViewPoint = new DataViewPoint(key.DataViewMouseEventHandler.ToLocal(point), key.DataViewMouseEventHandler);
					DataViewPoint dataViewPoint2 = new DataViewPoint(key.DataViewMouseEventHandler.ToLocal(globalDragCurrent), key.DataViewMouseEventHandler);
					this.m_dragMovedStrategy(value, dataViewPoint, dataViewPoint2);
				}
			}

			// Token: 0x04000B6C RID: 2924
			private Point m_globalDragStart;

			// Token: 0x04000B6D RID: 2925
			private Point m_globalDragCurrent;

			// Token: 0x04000B6E RID: 2926
			private int m_startScrollPos;

			// Token: 0x04000B6F RID: 2927
			private readonly GanttTrackCoordinator.DragMoveStrategy m_dragBeginStrategy;

			// Token: 0x04000B70 RID: 2928
			private readonly GanttTrackCoordinator.DragMoveStrategy m_dragMovedStrategy;

			// Token: 0x04000B71 RID: 2929
			private readonly Action<GanttTrackController> m_dragEndedStrategy;
		}

		// Token: 0x02000346 RID: 838
		internal class PanModeState : GanttTrackCoordinator.GanttTrackDragState
		{
			// Token: 0x0600111E RID: 4382 RVA: 0x00034DE8 File Offset: 0x00032FE8
			public PanModeState(GanttTrackCoordinator mediator)
				: base(mediator)
			{
			}

			// Token: 0x0600111F RID: 4383 RVA: 0x00034DF1 File Offset: 0x00032FF1
			public override void HandleOnDragBegin(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.Pan(ganttTrackView, e);
			}

			// Token: 0x06001120 RID: 4384 RVA: 0x00034DF1 File Offset: 0x00032FF1
			public override void HandleOnDragMoved(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.Pan(ganttTrackView, e);
			}

			// Token: 0x06001121 RID: 4385 RVA: 0x00008AEF File Offset: 0x00006CEF
			public override void HandleOnDragEnd(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
			}

			// Token: 0x06001122 RID: 4386 RVA: 0x00034DFC File Offset: 0x00032FFC
			private void Pan(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				GanttTrackController controllerForView = this.m_mediator.GetControllerForView(ganttTrackView);
				controllerForView.PanGanttTrack(e.PreviousLocation.X - e.CurrentLocation.X);
			}
		}

		// Token: 0x02000347 RID: 839
		internal class ResetState : GanttTrackCoordinator.GanttTrackDragState
		{
			// Token: 0x06001123 RID: 4387 RVA: 0x00034E33 File Offset: 0x00033033
			public ResetState(GanttTrackCoordinator mediator, GanttTrackCoordinator.GanttTrackDragState dragState)
				: base(mediator)
			{
				this.m_previousMode = mediator.m_shiftResetMode.Expect(new ApplicationException("Reset drag state created but reset shift state not set"));
				this.m_dragState = dragState;
			}

			// Token: 0x06001124 RID: 4388 RVA: 0x00034E5E File Offset: 0x0003305E
			public override void HandleOnDragBegin(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.m_dragState.HandleOnDragBegin(ganttTrackView, e);
			}

			// Token: 0x06001125 RID: 4389 RVA: 0x00034E6D File Offset: 0x0003306D
			public override void HandleOnDragMoved(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.m_dragState.HandleOnDragMoved(ganttTrackView, e);
			}

			// Token: 0x06001126 RID: 4390 RVA: 0x00034E7C File Offset: 0x0003307C
			public override void HandleOnDragEnd(IGanttTrackView ganttTrackView, DataViewDragEventArgs e)
			{
				this.m_dragState.HandleOnDragEnd(ganttTrackView, e);
				this.m_mediator.m_groupLayoutView.InteractionMode = this.m_mediator.m_shiftResetMode.Match<Maybe<InteractionMode>>((Maybe<InteractionMode> some) => new Maybe<InteractionMode>.Some(InteractionMode.Select), () => this.m_previousMode);
			}

			// Token: 0x06001127 RID: 4391 RVA: 0x00034EE1 File Offset: 0x000330E1
			public override void HandleOnPageDown(KeyPressedEventArgs args)
			{
				this.m_dragState.HandleOnPageDown(args);
			}

			// Token: 0x06001128 RID: 4392 RVA: 0x00034EEF File Offset: 0x000330EF
			public override void HandleOnPageUp(KeyPressedEventArgs args)
			{
				this.m_dragState.HandleOnPageUp(args);
			}

			// Token: 0x04000B72 RID: 2930
			private readonly Maybe<InteractionMode> m_previousMode;

			// Token: 0x04000B73 RID: 2931
			private readonly GanttTrackCoordinator.GanttTrackDragState m_dragState;
		}
	}
}
