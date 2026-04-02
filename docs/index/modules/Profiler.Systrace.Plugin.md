# MODULE INDEX — Profiler.Systrace.Plugin — AUTHORITATIVE ROUTING

## Routing Keywords
**Systems**: Linux ftrace, Android systrace, kernel tracing, sched_switch, CPU scheduler  
**Concepts**: multi-depth Gantt visualization, ftrace event parsing, task groups, depth hierarchy, statistics views  
**Common Logs**: `SystracePlugin.dll`, `BUFFER_TYPE_SYSTRACE_DATA`, `tblSystrace` tables, Trace metric  
**Entry Symbols**: `SystracePlugin`, `SystraceProcessor.connectionEvents_DataProcessed`, `SystraceModel.AddElement`, `FTraceEventDataRetriever`

---

## Role
Comprehensive Linux/Android kernel trace (ftrace/systrace) visualization plugin. Parses 15+ event types from database, organizes by task/thread groups with multi-depth hierarchy, creates Gantt/graph/marker tracks, and provides extensive statistics views for CPU, GPU, IRQ, clock, display, and kworker analysis.

---

## Entry Points
| Symbol | Location |
|--------|----------|
| SystracePlugin (constructor) | [dll/project/SystracePlugin/SystracePlugin.cs:18](dll/project/SystracePlugin/SystracePlugin.cs#L18) |
| SystraceProcessor (constructor) | [dll/project/SystracePlugin/SystraceProcessor.cs:20](dll/project/SystracePlugin/SystraceProcessor.cs#L20) |
| SystraceModel (constructor) | [dll/project/SystracePlugin/SystraceModel.cs:19](dll/project/SystracePlugin/SystraceModel.cs#L19) |
| connectionEvents_DataProcessed | [dll/project/SystracePlugin/SystraceProcessor.cs:46](dll/project/SystracePlugin/SystraceProcessor.cs#L46) |
| systraceEvents_DataProcessed | [dll/project/SystracePlugin/SystracePlugin.cs:81](dll/project/SystracePlugin/SystracePlugin.cs#L81) |
| StatisticsViewMgr (constructor) | [dll/project/SystracePlugin/StatisticsViewMgr.cs:11](dll/project/SystracePlugin/StatisticsViewMgr.cs#L11) |
| FTraceEventDataRetriever.GetEventData | [dll/project/SystracePlugin/FTraceEventDataRetriever.cs](dll/project/SystracePlugin/FTraceEventDataRetriever.cs) |

---

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| SystracePlugin | IMetricPlugin orchestrator managing UI track creation and statistics | [dll/project/SystracePlugin/SystracePlugin.cs:15](dll/project/SystracePlugin/SystracePlugin.cs#L15) |
| SystraceProcessor | Database→Model transformer processing 15+ tblSystrace* tables (1,231 lines) | [dll/project/SystracePlugin/SystraceProcessor.cs:17](dll/project/SystracePlugin/SystraceProcessor.cs#L17) |
| SystraceModel | Hierarchical data model: TaskGroupID → TaskName → Depth → Elements/Markers (178 lines) | [dll/project/SystracePlugin/SystraceModel.cs:10](dll/project/SystracePlugin/SystraceModel.cs#L10) |
| TaskTrackData | Per-task visualization state: DepthData, DataPointData, string models | [dll/project/SystracePlugin/TaskTrackData.cs:7](dll/project/SystracePlugin/TaskTrackData.cs#L7) |
| StatisticsViewMgr | Statistics registration manager for GPU/CPU/IRQ/Clock/MDP (286 lines) | [dll/project/SystracePlugin/StatisticsViewMgr.cs:8](dll/project/SystracePlugin/StatisticsViewMgr.cs#L8) |
| FTraceEventDataRetriever | Ftrace event data retrieval and parsing utilities | [dll/project/SystracePlugin/FTraceEventDataRetriever.cs:8](dll/project/SystracePlugin/FTraceEventDataRetriever.cs#L8) |
| DepthData | Single depth level: elements, markers, min/max timestamps, series | [dll/project/SystracePlugin/DepthData.cs](dll/project/SystracePlugin/DepthData.cs) |
| GraphData | Graph track data: DataPointList with min/max timestamps | [dll/project/SystracePlugin/GraphData.cs](dll/project/SystracePlugin/GraphData.cs) |

---

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| HandlesMetric(metricDesc) | Returns true if category is "Trace" | [SystracePlugin.cs:40](dll/project/SystracePlugin/SystracePlugin.cs#L40) | Plugin registration |
| connectionEvents_DataProcessed | Processes BUFFER_TYPE_SYSTRACE_DATA, queries 15+ tables | [SystraceProcessor.cs:46](dll/project/SystracePlugin/SystraceProcessor.cs#L46) | Systrace data received |
| AddElement(taskGroupID, taskName, depth, element) | Adds Gantt element at specific depth | [SystraceModel.cs:82](dll/project/SystracePlugin/SystraceModel.cs#L82) | Processing events |
| AddMarker(taskGroupID, taskName, depth, marker) | Adds marker at specific depth | [SystraceModel.cs:113](dll/project/SystracePlugin/SystraceModel.cs#L113) | Processing instant events |
| AddDataPoint(taskGroupID, taskName, trackName, point) | Adds graph data point | [SystraceModel.cs:151](dll/project/SystracePlugin/SystraceModel.cs#L151) | Processing counters |
| systraceEvents_DataProcessed | Creates UI tracks from model data | [SystracePlugin.cs:81](dll/project/SystracePlugin/SystracePlugin.cs#L81) | Model.Changed event |
| AddGanttTracks(taskTrackData, taskGroupID, taskName) | Creates multi-depth Gantt tracks | [SystracePlugin.cs:177](dll/project/SystracePlugin/SystracePlugin.cs#L177) | UI creation |
| AddGraphTracks(taskTrackData, taskGroupID, taskName) | Creates stepped-line graph tracks | [SystracePlugin.cs:118](dll/project/SystracePlugin/SystracePlugin.cs#L118) | Counter visualization |
| TryAddGroup(container, taskGroupID, taskGroupName) | Creates group for task (e.g., "Trace Process 1234") | [SystracePlugin.cs:256](dll/project/SystracePlugin/SystracePlugin.cs#L256) | First task in group |
| GetEventData(eventName, captureID) | Retrieves ftrace event data from database | [FTraceEventDataRetriever.cs](dll/project/SystracePlugin/FTraceEventDataRetriever.cs) | Statistics generation |
| ParseFtraceMOD(ModelObjectData) | Parses ftrace event from database row | [FTraceEventDataRetriever.cs:181](dll/project/SystracePlugin/FTraceEventDataRetriever.cs#L181) | Event processing |
| StoreSystraceSchedSwitchToModel | Processes sched_switch events | [SystraceProcessor.cs](dll/project/SystracePlugin/SystraceProcessor.cs) | Building model |
| StoreSystraceCpuFreqChangesToModel | Processes CPU frequency change events | [SystraceProcessor.cs](dll/project/SystracePlugin/SystraceProcessor.cs) | Building model |
| GenerateViewModels(captureID) | Generates statistics views (TreeView/Histogram) | IStatistic implementations | Statistics requested |

---

## Call Flow Skeleton
```
Plugin Initialization
 ├── SystracePlugin()
 │    ├── new SystraceModel()
 │    ├── Subscribe to Model.Changed
 │    ├── Subscribe to ConnectionEvents.StartCaptureRequest
 │    ├── new SystraceProcessor()
 │    └── new StatisticsViewMgr()
 │
 └── StatisticsViewMgr()
      ├── Subscribe to ConnectionEvents.DataProcessed
      ├── Subscribe to ConnectionEvents.MetricAdded
      └── Create IStatistic instances (GPU, CPU, IRQ, Clock, MDP)

Data Processing Flow
 ├── SystraceProcessor.connectionEvents_DataProcessed
 │    ├── Check BufferCategory == BUFFER_TYPE_SYSTRACE_DATA
 │    ├── Show progress: "Loading Trace Data"
 │    ├── Get DataModel → SystraceModel
 │    │
 │    ├── Query specialized tables (e.g., SystraceAdrenoCmdBatch)
 │    │    ├── Process GPU activity
 │    │    ├── Create "GPU Activity" group
 │    │    └── Add Gantt tracks with CPU/GPU series
 │    │
 │    └── Process 15+ tblSystrace* tables via m_storeToModelDictionary
 │         ├── tblSystraceGraphSeriesTable → StoreSystraceGraphSeriesToModel
 │         ├── tblSystraceMarkersTable → StoreSystraceMarkersToModel
 │         ├── tblSystraceGanttElementsTable → StoreSystraceGanttElementsToModel
 │         ├── tblSystraceFunctions → StoreSystraceFunctionsToModel
 │         ├── tblSystraceASyncFuncs → StoreSystraceASyncFuncsToModel
 │         ├── tblSystraceCounters → StoreSystraceCountersToModel
 │         ├── tblSystraceSchedSwitch → StoreSystraceSchedSwitchToModel
 │         ├── tblSystraceClockSetRate → StoreSystraceClockSetMarkersToModel
 │         ├── tblSystraceCpuFreq → StoreSystraceCpuFreqChangesToModel
 │         ├── tblSystraceCpuIdle → StoreSystraceCpuIdleToModel
 │         ├── tblSystraceSyncTimeline → StoreSystraceSyncTimelineToModel
 │         ├── tblSystraceSyncWait → StoreSystraceSyncWaitToModel
 │         └── tblSystraceWorkExec → StoreSystraceWorkExecToModel
 │              │
 │              └── For each row:
 │                   ├── Parse taskGroupID, taskName, depth
 │                   ├── Create Element or Marker
 │                   └── SystraceModel.AddElement(taskGroupID, taskName, depth, element)
 │                        └── Update TaskGroupIDData[taskGroupID][taskName].DepthData[depth]
 │
 └── Trigger SystraceModel.Changed event
      └── SystracePlugin.systraceEvents_DataProcessed
           ├── Check GroupLayoutController exists for capture
           ├── Application.Invoke (UI thread)
           │    ├── For each TaskGroupID:
           │    │    ├── For each TaskName (sorted):
           │    │    │    ├── TryAddGroup(container, taskGroupID, taskName)
           │    │    │    │    └── Create "Trace Process XXX - TaskName" group
           │    │    │    │
           │    │    │    ├── AddGanttTracks(taskTrackData, taskGroupID, taskName)
           │    │    │    │    ├── TryAddTrackToGroup → create GanttTrackController
           │    │    │    │    ├── Set colors, name strings, tooltip strings
           │    │    │    │    ├── For each depth (sorted):
           │    │    │    │    │    ├── Create Series with depth name
           │    │    │    │    │    └── Add to GanttTrackController
           │    │    │    │    └── SetDataBounds, Invalidate
           │    │    │    │
           │    │    │    └── AddGraphTracks(taskTrackData, taskGroupID, taskName)
           │    │    │         ├── For each counter in DataPointData:
           │    │    │         │    ├── Create GraphTrackController if needed
           │    │    │         │    ├── Set stepped line draw mode
           │    │    │         │    └── AddTransientMetricData(counterName, dataPoints)
           │    │    │         └── SetDataBounds
           │    │    └── Track groups/tracks created in UI
           │    └── Print timing
           └── End progress

Statistics Generation
 └── User opens Statistics view
      ├── StatisticsManager requests views
      ├── IStatistic.GenerateViewModels(captureID)
      │    ├── FTraceEventDataRetriever.GetEventData(eventName, captureID)
      │    │    ├── Query appropriate tblSystrace* table
      │    │    └── Return ModelObjectDataList
      │    │
      │    ├── Parse events with ParseFtraceMOD
      │    ├── Build TreeModel or HistogramModel
      │    └── Return TreeViewStatisticDisplayViewModel or HistogramStatisticDisplayViewModel
      │
      └── Display in Statistics UI
```

---

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| SystraceModel | SystracePlugin constructor | SystraceProcessor, SystracePlugin UI | Plugin shutdown |
| TaskGroupIDData | SystraceModel.AddElement/AddMarker | SystracePlugin.systraceEvents_DataProcessed | Model.ClearData() |
| DepthData | AddElement/AddMarker | AddGanttTracks | ClearData() |
| TaskTrackData | Created per task in TaskGroupIDData | AddGanttTracks, AddGraphTracks | ClearData() |
| GanttTrackController | AddTrackToGroupCommand | UI rendering | Track removal |
| GraphTrackController | AddTrackToGroupCommand | Graph visualization | Track removal |
| GroupController | AddGroupCommand | Track container | Group disposal |
| m_storeToModelDictionary | SystraceProcessor constructor | connectionEvents_DataProcessed | Processor lifetime |
| IStatistic instances | StatisticsViewMgr constructor | StatisticsManager | Plugin shutdown |
| TreeModel/HistogramModel | IStatistic.GenerateViewModels | Statistics UI | View closed |

---

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `SystracePlugin.dll` | Console logs | Plugin assembly loaded |
| `BUFFER_TYPE_SYSTRACE_DATA` | [SystraceProcessor.cs:46](dll/project/SystracePlugin/SystraceProcessor.cs#L46) | Systrace buffer type |
| "Trace" | [SystracePlugin.cs:99](dll/project/SystracePlugin/SystracePlugin.cs#L99) | Metric category name |
| "Loading Trace Data" | [SystraceProcessor.cs:52](dll/project/SystracePlugin/SystraceProcessor.cs#L52) | Progress description |
| "Gathering Trace Capture Data" | [SystracePlugin.cs:62](dll/project/SystracePlugin/SystracePlugin.cs#L62) | Capture progress |
| "Trace Process XXX - TaskName" | [SystracePlugin.cs:265](dll/project/SystracePlugin/SystracePlugin.cs#L265) | Group name format |
| "Trace Kernel - TaskName" | [SystracePlugin.cs:263](dll/project/SystracePlugin/SystracePlugin.cs#L263) | Kernel task group |
| "GPU Activity" | [SystraceProcessor.cs:154](dll/project/SystraceProcessor.cs#L154) | GPU group name |
| "KGSL Power State" | [SystraceProcessor.cs:271](dll/project/SystraceProcessor.cs#L271) | KGSL group name |
| SystraceModel | Database model name | Database queries |
| tblSystrace* | Various | Database table prefix |

---

## Database Tables
```sql
-- Systrace Database Tables (15+)
tblSystraceGraphSeriesTable       -- Counter/graph data points
tblSystraceMarkersTable           -- Instant event markers
tblSystraceGanttElementsTable     -- Gantt timeline elements
tblSystraceFunctions              -- Function call traces
tblSystraceASyncFuncs             -- Asynchronous function events
tblSystraceCounters               -- Counter values
tblSystraceSchedSwitch            -- CPU scheduler context switches
tblSystraceClockSetRate           -- Clock frequency changes
tblSystraceCpuFreq                -- CPU frequency transitions
tblSystraceCpuIdle                -- CPU idle state changes
tblSystraceSyncTimeline           -- Sync primitive timeline
tblSystraceSyncWait               -- Sync wait events
tblSystraceWorkExec               -- Workqueue execution

-- Specialized tables
SystraceAdrenoCmdBatch            -- GPU command batch submissions
SystraceKGSLPwrSetState           -- KGSL GPU power state changes

-- Common columns (inferred):
captureID, timestamp, timestampBegin, timestampEnd, 
eventName, trackName, taskName, taskGroupID, depth,
cpuNum, params (key=value pairs)
```

---

## Component Architecture
```
SystracePlugin.dll (28 .cs files, ~5,000 lines)
├── Core Plugin (322 lines)
│   ├── SystracePlugin.cs - IMetricPlugin orchestrator
│   ├── SystraceProcessor.cs (1,231 lines) - Database→Model transformer
│   ├── SystraceModel.cs (178 lines) - Hierarchical data model
│   └── StatisticsViewMgr.cs (286 lines) - Statistics registrar
│
├── Data Structures
│   ├── TaskTrackData.cs (103 lines) - Per-task visualization state
│   ├── DepthData.cs - Single depth level data
│   ├── GraphData.cs - Graph track data
│   └── ThreadTime.cs - Thread time tracking
│
├── Utilities
│   └── FTraceEventDataRetriever.cs - Ftrace event parsing
│
└── Statistics (IStatistic implementations)
    ├── GPU Statistics
    │   ├── GPUStateTimeline.cs - GPU power state timeline
    │   ├── GPUStateDistribution.cs - GPU state distribution
    │   ├── GPUFreqTimeline.cs - GPU frequency timeline
    │   ├── GPUFreqDistribution.cs - GPU frequency distribution
    │   ├── GPUStateAndFreqDistribution.cs - Combined GPU analysis
    │   ├── GPUBusTimeline.cs - GPU bus activity
    │   └── GPUBusDistribution.cs - GPU bus distribution
    │
    ├── CPU Statistics
    │   ├── CPUFrequency.cs (229 lines) - CPU frequency analysis
    │   └── RunQueueDepth.cs - Scheduler run queue depth
    │
    ├── Clock Statistics
    │   ├── ClockBase.cs - Base class for clock stats
    │   ├── ClockTimeline.cs - Clock frequency timeline
    │   └── ClockDistribution.cs - Clock frequency distribution
    │
    ├── IRQ Statistics
    │   ├── IrqDistribution.cs - IRQ distribution analysis
    │   └── KWorkerFunctionContext.cs - Kworker function context
    │
    ├── Kworker Statistics
    │   └── KworkerFunctionDistribution.cs - Kworker function distribution
    │
    ├── Display (MDP) Statistics
    │   ├── MDPCommitInfo.cs - MDP commit information
    │   └── MDPTraceEvents.cs - MDP trace events
    │
    └── Thread Statistics
        └── ThreadTime.cs - Thread execution time
```

---

## Dependencies
```
SDPClientFramework.dll:
  - SdpApp (EventsManager, ConnectionManager, CommandManager, ModelManager, StatisticsManager)
  - IMetricPlugin interface
  - DataProcessedEventArgs, MetricAddedEventArgs
  - GroupLayoutController, GanttTrackController, GraphTrackController
  - Command pattern (AddGroupCommand, AddTrackToGroupCommand, AddMetricToTrackCommand)
  - DataModel, Model, ModelObject, ModelObjectDataList
  - IStatistic, IStatisticDisplayViewModel
  - TreeViewStatisticDisplayViewModel, HistogramStatisticDisplayViewModel
  - InspectorViewDisplayEventArgs, PropertyGridDescriptionObject
  - Series, Element, Marker, Connection (Gantt charts)
  - DataPoint, DataPointList (Graphs)

SDPCoreWrapper.dll:
  - SDPCore.BUFFER_TYPE_SYSTRACE_DATA
  - SDPCore.DSP_MODEL_ATTRIB_CAPTURE_ID

Gtk/Cairo:
  - Application.Invoke (UI thread marshaling)
  - Color (track colors)

Sdp.Helpers:
  - Int64Converter, UintConverter, DoubleConverter
  - FormatHelper (time formatting)

Sdp.Charts.Gantt:
  - GanttTrackController, ElementSelectedEventArgs

Sdp.Charts.Graph:
  - GraphTrackController, Point
```

---

## Visualization Types

### Multi-Depth Gantt Tracks
- **Purpose**: Show nested function calls, async events, sched_switch
- **Depth Hierarchy**: Each depth level = one series in Gantt
- **Example**: Process 1234 → Thread "main" → Depth 0 (functions), Depth 1 (nested calls)
- **Data**: Elements with start/end timestamps

### Graph Tracks (Stepped Line)
- **Purpose**: Visualize counter values over time
- **Draw Mode**: DRAW_STEPPED_LINE (discrete values)
- **Example**: CPU frequency, run queue depth
- **Data**: DataPoint(timestamp, value)

### Marker Tracks
- **Purpose**: Show instant events
- **Example**: Clock set rate changes, IRQ occurrences
- **Data**: Marker with position (timestamp)

### Statistics Views
- **TreeView**: Tabular data with sortable columns
- **Histogram**: Bar charts showing distributions
- **Example**: CPU frequency distribution per core

---

## Usage Patterns

### Task Group Organization
```
Trace Process 1234 - com.example.app (taskGroupID = 1234)
  ├── Gantt Track: "main" thread
  │    ├── Series (Depth 0): Functions
  │    ├── Series (Depth 1): Nested calls
  │    └── Series (Depth 2): Async events
  ├── Graph Track: CPU frequency
  └── Graph Track: Run queue depth

Trace Kernel - swapper/0 (taskGroupID = -1)
  ├── Gantt Track: sched_switch events
  └── Graph Track: Counters
```

### Event Processing Pattern
```csharp
// 1. Database row → SystraceModel
foreach (ModelObjectData row in dbResults) {
    int taskGroupID = GetTaskGroupID(row);
    string taskName = row.GetValue("taskName");
    int depth = GetDepth(row);
    
    Element element = CreateElement(row);
    Model.AddElement(taskGroupID, taskName, depth, element);
}

// 2. Model → UI tracks
foreach (var taskGroup in Model.TaskGroupIDData) {
    foreach (var task in taskGroup.Value) {
        TryAddGroup(taskGroup.Key, task.Key);
        AddGanttTracks(task.Value, taskGroup.Key, task.Key);
        AddGraphTracks(task.Value, taskGroup.Key, task.Key);
    }
}
```

---

## Supported Ftrace Events

### Scheduler Events
- `sched_switch` - CPU context switches
- `sched_wakeup` - Thread wakeups

### CPU Events
- `cpu_frequency` - CPU frequency changes
- `cpu_idle` - CPU idle state changes

### GPU (KGSL) Events
- `adreno_cmdbatch_submitted` - GPU command batch submissions
- `adreno_cmdbatch_retired` - GPU command completions
- `kgsl_pwr_set_state` - GPU power state changes
- `kgsl_bus_update` - GPU bus frequency changes

### Clock Events
- `clock_set_rate` - System clock frequency changes

### Display (MDP) Events
- `mdp_commit` - Display commit events
- `mdp_trace` - Display pipeline traces

### IRQ Events
- `irq_handler_entry` - IRQ handler start
- `irq_handler_exit` - IRQ handler end

### Workqueue Events
- `workqueue_execute_start` - Work execution start
- `workqueue_execute_end` - Work execution end

### Sync Events
- `sync_timeline` - Sync timeline events
- `sync_wait` - Sync wait events

### Custom Function Traces
- User-defined ftrace function calls
- Async function traces

---

## Search Hints
```
Find plugin entry:
search "class SystracePlugin.*IMetricPlugin"
open dll/project/SystracePlugin/SystracePlugin.cs:15

Find data processing:
search "connectionEvents_DataProcessed.*SYSTRACE"
open dll/project/SystracePlugin/SystraceProcessor.cs:46

Find model hierarchy:
search "AddElement.*taskGroupID.*depth"
open dll/project/SystracePlugin/SystraceModel.cs:82

Find statistics:
search "class.*: IStatistic"
open dll/project/SystracePlugin/

Find ftrace parsing:
search "FTraceEventDataRetriever"
open dll/project/SystracePlugin/FTraceEventDataRetriever.cs:8

Find database tables:
search "tblSystrace"
open dll/project/SystracePlugin/SystraceProcessor.cs:29

Find GPU activity:
search "SystraceAdrenoCmdBatch"
open dll/project/SystracePlugin/SystraceProcessor.cs:56

Find sched_switch:
search "StoreSystraceSchedSwitch"
open dll/project/SystracePlugin/SystraceProcessor.cs
```

---

## Integration Points
| External System | Integration Method | Location |
|----------------|-------------------|----------|
| SDPClientFramework | IMetricPlugin interface implementation | [SystracePlugin.cs:15](dll/project/SystracePlugin/SystracePlugin.cs#L15) |
| ConnectionEvents | DataProcessed event subscription | [SystraceProcessor.cs:24](dll/project/SystracePlugin/SystraceProcessor.cs#L24) |
| DataModel | Query SystraceModel database tables | [SystraceProcessor.cs:51](dll/project/SystracePlugin/SystraceProcessor.cs#L51) |
| TraceModel | Access GroupLayoutControllers | [SystracePlugin.cs:82](dll/project/SystracePlugin/SystracePlugin.cs#L82) |
| CommandManager | Execute UI commands (AddGroup, AddTrack, AddMetric) | [SystracePlugin.cs:258+](dll/project/SystracePlugin/SystracePlugin.cs#L258) |
| StatisticsManager | Register IStatistic implementations | [StatisticsViewMgr.cs:38+](dll/project/SystracePlugin/StatisticsViewMgr.cs#L38) |
| GTK UI Thread | Application.Invoke for thread safety | [SystracePlugin.cs:92](dll/project/SystracePlugin/SystracePlugin.cs#L92) |

---

## Notes
- **Comprehensive Coverage**: Handles 15+ different ftrace event types
- **Hierarchical Organization**: TaskGroupID → TaskName → Depth creates logical grouping
- **Multi-Depth Gantt**: Each depth level becomes a series showing nested events
- **Lazy Loading**: Statistics views generated on-demand via GenerateViewModels
- **Thread Safety**: All UI operations marshaled via Application.Invoke
- **Database-Driven**: All data comes from tblSystrace* table queries
- **Color Management**: Pseudo-random colors for consistent visual identity
- **Tooltip Support**: Rich tooltips with timing information
- **Inspector Integration**: Selected elements display properties in Inspector view
- **Metric Category**: "Trace" category distinguishes from other metrics
- **Task Hash**: (taskName.GetHashCode() * 2^32 + taskGroupID) for unique keying
- **Progress Tracking**: Shows "Loading Trace Data" during processing
- **Statistics Registration**: Conditional based on metric presence (KGSL, Power, CPU Frequency, etc.)
- **Large Processor**: SystraceProcessor is 1,231 lines handling complex transformations
- **Event Parsing**: Regex-based parameter parsing from ftrace event strings

---

## Typical Workflow

### Capture and Visualization
1. User enables "Trace" metric category
2. Device captures ftrace events (sched_switch, cpu_freq, gpu, etc.)
3. Data transferred to PC as BUFFER_TYPE_SYSTRACE_DATA
4. SystraceProcessor.connectionEvents_DataProcessed fires
5. Progress bar: "Loading Trace Data"
6. Query 15+ tblSystrace* tables from database
7. For each event type:
   - Call Store*ToModel method
   - Parse taskGroupID, taskName, depth
   - Create Element/Marker/DataPoint
   - Add to SystraceModel hierarchy
8. Trigger SystraceModel.Changed event
9. SystracePlugin.systraceEvents_DataProcessed fires
10. On UI thread (Application.Invoke):
    - For each task group/task:
      - Create "Trace Process XXX - TaskName" group
      - Create Gantt track with multi-depth series
      - Create graph tracks for counters
11. User sees hierarchical Gantt/graph tracks in UI
12. User can:
    - Expand/collapse groups
    - Select elements → Inspector shows details
    - Open Statistics view → Generate TreeView/Histogram
    - Zoom/pan timeline