# MODULE INDEX — SDP.UI.ViewInfrastructure — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: View Infrastructure, Event Handling, User Interaction, Timeline Visualization, UI State Management
Concepts: Mouse Events, Keyboard Events, Drag Detection, Selection Management, Highlight Regions, Gantt Coordination, View Models
Common Logs: m_logger (GanttTrackCoordinator)
Entry Symbols: IMouseEventHandler, MouseEventController, GanttTrackCoordinator, DataViewMouseEventController, GanttTrackViewModel

## Role
View infrastructure layer providing event handling abstractions (mouse/keyboard), Gantt chart interaction coordination across multiple tracks, selection/highlight view model management, and data-view-space coordinate transformation for timeline-based visualizations.

## Entry Points
| Symbol | Location |
|--------|----------|
| IMouseEventHandler | [Views/EventHandlers/MouseEventHandler/IMouseEventHandler.cs:6](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/IMouseEventHandler.cs#L6) |
| MouseEventController | [Views/EventHandlers/MouseEventHandler/MouseEventController.cs:7](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L7) |
| IMouseEventController | [Views/EventHandlers/MouseEventHandler/IMouseEventController.cs](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/IMouseEventController.cs) |
| IKeyboardEventHandler | [Views/EventHandlers/KeyboardEventHandler/IKeyboardEventHandler.cs:6](../../../dll/project/SDPClientFramework/Views/EventHandlers/KeyboardEventHandler/IKeyboardEventHandler.cs#L6) |
| GanttTrackCoordinator | [Views/Flow/Controllers/GanttTrackCoordinator.cs:17](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L17) |
| DataViewMouseEventController | [Views/Flow/Controllers/DataViewMouseEventController.cs:8](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/DataViewMouseEventController.cs#L8) |
| IGanttTrackViewModel | [Views/Flow/ViewModels/GanttTrack/IGanttTrackViewModel.cs:10](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/IGanttTrackViewModel.cs#L10) |
| GanttTrackViewModel | [Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs:12](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs#L12) |
| SelectionViewModel | [Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs:12](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs#L12) |
| HighlightViewModel | [Views/Flow/ViewModels/GanttTrack/HighlightViewModel.cs:8](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/HighlightViewModel.cs#L8) |

## Key Classes
| Class | Responsibility | Location |
|------|----------------|----------|
| MouseEventController | Drag detection and click event processing from raw mouse events | [MouseEventController.cs:7](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L7) |
| IMouseEventHandler | Raw mouse event interface (pressed, moved, released) | [IMouseEventHandler.cs:6](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/IMouseEventHandler.cs#L6) |
| IKeyboardEventHandler | Keyboard event interface (pressed, released) | [IKeyboardEventHandler.cs:6](../../../dll/project/SDPClientFramework/Views/EventHandlers/KeyboardEventHandler/IKeyboardEventHandler.cs#L6) |
| GanttTrackCoordinator | Coordinates interaction across multiple Gantt tracks with mode management | [GanttTrackCoordinator.cs:17](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L17) |
| DataViewMouseEventController | Converts mouse events to data-view-space coordinates with timestamp | [DataViewMouseEventController.cs:8](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/DataViewMouseEventController.cs#L8) |
| DataViewPoint | Point with timestamp for data-view-space coordinates | [DataViewPoint.cs:8](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/DataViewPoint.cs#L8) |
| GanttTrackViewModel | View model managing selection, highlight, and inspector for single track | [GanttTrackViewModel.cs:12](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs#L12) |
| SelectionViewModel | Manages element/marker selection ranges across series | [SelectionViewModel.cs:12](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs#L12) |
| HighlightViewModel | Manages highlight region state and updates | [HighlightViewModel.cs:8](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/HighlightViewModel.cs#L8) |
| HighlightRegion | Begin/end points defining highlight region | [HighlightRegion.cs:7](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/HighlightRegion.cs#L7) |
| InspectorViewModel | Interface for property inspection view models | [InspectorViewModel.cs:7](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/InspectorViewModel.cs#L7) |
| SingleSelectionViewModel | Inspector for single selected element | [SingleSelectionViewModel.cs:10](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SingleSelectionViewModel.cs#L10) |
| MultiSelectInspectorViewModel | Inspector for multiple selected elements | [MultiSelectInspectorViewModel.cs:11](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/MultiSelectInspectorViewModel.cs#L11) |
| SeriesElementPair | Series + Element pair for selection tracking | [SeriesElementPair.cs:7](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SeriesElementPair.cs#L7) |
| SeriesMarkerPair | Series + Marker pair for marker selection | [SeriesMarkerPair.cs:7](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SeriesMarkerPair.cs#L7) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| MouseEventController (constructor) | Subscribe to raw mouse events | [MouseEventController.cs:49](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L49) | Controller creation |
| MouseEventController.OnMouseButtonPressed | Detect single/double click press | [MouseEventController.cs:60](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L60) | Mouse button pressed |
| MouseEventController.OnMouseMoved | Detect drag begin/move or track location | [MouseEventController.cs:95](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L95) | Mouse moved |
| MouseEventController.OnMouseButtonReleased | Complete drag or fire click event | [MouseEventController.cs:133](../../../dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs#L133) | Mouse button released |
| GanttTrackCoordinator.AddGanttTrackController | Register track with coordinator | [GanttTrackCoordinator.cs:46](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L46) | Track added to layout |
| GanttTrackCoordinator.RemoveGanttTrackController | Unregister track from coordinator | [GanttTrackCoordinator.cs:73](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L73) | Track removed from layout |
| GanttTrackCoordinator.GroupLayoutView_OnKeyPressed | Handle keyboard shortcuts (1/2/3 modes, arrow keys) | [GanttTrackCoordinator.cs:80](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L80) | Key pressed in layout |
| GanttTrackCoordinator.SetGroupLayoutInteractionMode | Change interaction mode (Select/Pan/Zoom) | [GanttTrackCoordinator.cs](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs) | Mode switch requested |
| DataViewMouseEventController (constructor) | Wrap MouseEventController with data-view events | [DataViewMouseEventController.cs:33](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/DataViewMouseEventController.cs#L33) | Data view creation |
| GanttTrackViewModel.SelectElement | Select element with modifier (add/toggle/replace) | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | User clicks element |
| GanttTrackViewModel.SelectMarker | Select marker with modifier | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | User clicks marker |
| GanttTrackViewModel.SetHighlightRegion | Define highlight region for selection | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | User drags selection box |
| GanttTrackViewModel.SelectElementsInHighlightRegion | Select all elements in highlighted area | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | Drag selection completes |
| GanttTrackViewModel.MoveSelection | Navigate selection with arrow keys | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | Arrow key pressed |
| GanttTrackViewModel.ClearSelections | Deselect all elements | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | Clear selection requested |
| GanttTrackViewModel.GetElementAtPoint | Hit test for element at data-view point | [GanttTrackViewModel.cs](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs) | Mouse interaction |
| SelectionViewModel.IsSelected | Check if element/marker is selected | [SelectionViewModel.cs:40](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs#L40) | Rendering selection state |
| SelectionViewModel.GetSelectedObjectCount | Count total selected objects | [SelectionViewModel.cs:76](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs#L76) | Inspector display |
| HighlightViewModel.SetHighlightRegion | Update highlight region bounds | [HighlightViewModel.cs:22](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/HighlightViewModel.cs#L22) | Drag in progress |
| HighlightViewModel.ClearHighlightRegion | Remove highlight region | [HighlightViewModel.cs:41](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/HighlightViewModel.cs#L41) | Drag ended or cancelled |

## Call Flow Skeleton
```
Mouse Event Processing:
Raw mouse input
 └── IMouseEventHandler raises event
      └── MouseEventController.OnMouseButtonPressed/Moved/Released
           ├── Track CurrentClickType (Maybe<Click>)
           ├── Track CurrentLocation (Point)
           └── Detect patterns:
                ├── Double-click → MouseButtonClicked (immediate)
                ├── Single-click press → Store click info
                ├── Move while pressed → DragBegin (first), then DragMove
                └── Release after drag → DragEnded
                └── Release without drag → MouseButtonClicked

Data-View Mouse Events:
IDataViewMouseEventHandler
 └── DataViewMouseEventController wraps MouseEventController
      ├── Subscribe to DragBegin/Move/Ended/Clicked
      └── Convert each event:
           ├── Create DataViewDragEventArgs (with timestamp)
           ├── Use IDataViewMouseEventHandler.ToLocal() for coordinates
           └── Forward to data-view subscribers

Gantt Track Coordination:
GanttTrackCoordinator manages multiple tracks
 ├── AddGanttTrackController(ganttController)
 │    ├── Subscribe to track's DataViewMouseController events
 │    └── Add to m_ganttTrackControllers dictionary
 ├── Keyboard events from GroupLayoutView
 │    ├── Key 1/2/3 → SetGroupLayoutInteractionMode(Select/Pan/Zoom)
 │    ├── Left/Right → MoveFirstSeriesSelection
 │    └── Shift → Temporary Select mode (released restores)
 └── Mouse events from tracks
      ├── GanttView_OnClicked → Handle element/marker selection
      ├── GanttView_OnDragBegin → Start highlight region
      ├── GanttView_OnDragMove → Update highlight region
      └── GantView_OnDragEnded → SelectElementsInHighlightRegion

Selection Management:
GanttTrackViewModel
 └── SelectElement(pair, modifier)
      └── SelectionViewModel
           ├── SelectionType determines action:
           │    ├── Default → Replace selection
           │    ├── AddToSelection → Add to existing
           │    └── ToggleSelection → Toggle state
           ├── Update Selections dictionary (Series → SelectionRanges)
           ├── Maintain selection ranges (first/last indices)
           └── Raise SelectionUpdated event
                └── GanttTrackViewModel forwards to subscribers

Highlight Region:
User drags in data view
 └── GanttTrackCoordinator.GanttView_OnDragBegin
      └── GanttTrackViewModel.SetHighlightRegion(begin, end, type)
           └── HighlightViewModel.SetHighlightRegion
                ├── Store HighlightRegion (Maybe<HighlightRegion>)
                └── Raise HighlightRegionUpdated
User continues dragging
 └── GanttTrackCoordinator.GanttView_OnDragMove
      └── Update highlight region bounds
User releases
 └── GanttTrackCoordinator.GantView_OnDragEnded
      └── GanttTrackViewModel.SelectElementsInHighlightRegion()
           ├── Iterate series elements in region
           ├── Call SelectElement for each
           └── ClearHighlightRegion()

Inspector View Models:
GanttTrackViewModel property access
 ├── SingleSelectInspectorViewModel getter
 │    └── InspectorViewModelBuilder.Build()
 │         ├── Read SelectionViewModel
 │         ├── Get tooltip model
 │         └── Build SingleSelectionViewModel
 └── MultiSelectInspectorViewModel getter
      └── InspectorViewModelBuilder.Build()
           └── Build MultiSelectInspectorViewModel

Keyboard Navigation:
Key pressed (Left/Right arrows)
 └── GanttTrackCoordinator.GroupLayoutView_OnKeyPressed
      └── MoveFirstSeriesSelection(direction, modifier)
           └── Get first track's GanttTrackViewModel
                └── GanttTrackViewModel.MoveSelection(direction, modifier)
                     └── SelectionViewModel navigates to next/prev element
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| CurrentClickType (Maybe<Click>) | MouseEventController | Drag/click detection | OnMouseButtonReleased |
| CurrentLocation (Point) | MouseEventController | OnMouseMoved tracking | Controller lifetime |
| IsDragging flag | MouseEventController | Drag state machine | OnMouseButtonReleased |
| m_ganttTrackControllers dictionary | GanttTrackCoordinator | Event routing, mode setting | RemoveGanttTrackController |
| m_state (drag state) | GanttTrackCoordinator | Drag tracking across tracks | Drag end |
| Selections dictionary | SelectionViewModel | IsSelected, selection operations | ClearSelections |
| HighlightRegion (Maybe) | HighlightViewModel | Rendering highlight | ClearHighlightRegion |
| Series list | SelectionViewModel, GanttTrackViewModel | Selection/rendering | ViewModel lifetime |
| m_mouseController | DataViewMouseEventController | Event forwarding | Controller lifetime |
| SelectionRanges | SelectionViewModel (nested class) | Range-based selection tracking | Selection updates |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| m_logger | [GanttTrackCoordinator.cs:25](../../../dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs#L25) | Logger instance for coordinator |
| Logger property | [GanttTrackViewModel.cs:47](../../../dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs#L47) | Logger passed to view model |

## Search Hints
```
Find mouse event handling:
search "class MouseEventController"
search "IMouseEventHandler|MouseButtonPressed|MouseMoved|MouseButtonReleased"

Find drag detection:
search "DragBegin|DragMove|DragEnded"
search "IsDragging"

Find keyboard handling:
search "IKeyboardEventHandler|KeyPressed|KeyReleased"
search "GroupLayoutView_OnKeyPressed"

Find Gantt coordination:
search "class GanttTrackCoordinator"
search "AddGanttTrackController|RemoveGanttTrackController"

Find data-view events:
search "DataViewMouseEventController|DataViewPoint"
search "DataViewDragEventArgs|DataViewMouseClickEventArgs"

Find selection management:
search "class SelectionViewModel"
search "SelectElement|IsSelected|SelectionRanges"

Find highlight regions:
search "class HighlightViewModel"
search "SetHighlightRegion|ClearHighlightRegion|HighlightRegion"

Find view models:
search "class GanttTrackViewModel"
search "IGanttTrackViewModel|IReadOnlyGanttTrackViewModel"

Find inspector view models:
search "InspectorViewModel|SingleSelectionViewModel|MultiSelectInspectorViewModel"

Jump to key components:
open dll/project/SDPClientFramework/Views/EventHandlers/MouseEventHandler/MouseEventController.cs:7
open dll/project/SDPClientFramework/Views/Flow/Controllers/GanttTrackCoordinator.cs:17
open dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/GanttTrackViewModel.cs:12
open dll/project/SDPClientFramework/Views/Flow/ViewModels/GanttTrack/SelectionViewModel.cs:12
```
