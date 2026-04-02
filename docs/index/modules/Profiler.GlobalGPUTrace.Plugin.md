# MODULE INDEX — Profiler.GlobalGPUTrace.Plugin — AUTHORITATIVE ROUTING

## Routing Keywords
**Systems**: GPU utilization metrics, GGPM (Global GPU Performance Monitor), trace visualization  
**Concepts**: metric plugin, graph track creation, transient data visualization, stepped line charts  
**Common Logs**: `GlobalGPUTracePlugin.dll`, `GPU % Utilization`, GGPM trace data  
**Entry Symbols**: `GlobalGPUTracePlugin` (constructor), `OnDataProcessed`, `AddDataToTracks`

---

## Role
Lightweight IMetricPlugin implementation for visualizing "GPU % Utilization" metrics. Processes GGPM trace data from database and creates stepped-line graph tracks in Snapdragon Profiler UI.

---

## Entry Points
| Symbol | Location |
|--------|----------|
| GlobalGPUTracePlugin (constructor) | [dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:11](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L11) |
| OnDataProcessed | [dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:62](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L62) |
| GlobalGPUTraceCaptureData (constructor) | [dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:11](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L11) |
| AddData(metricID, timestamp, data) | [dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:18](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L18) |
| AddDataToTracks(categoryName) | [dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:36](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L36) |

---

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| GlobalGPUTracePlugin | IMetricPlugin implementation handling "GPU % Utilization" category | [dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:8](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L8) |
| GlobalGPUTraceCaptureData | Per-capture data aggregator and UI track creator | [dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:10](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L10) |
| DataBounds | Timestamp bounds struct (min, max) | [dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:112](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L112) |

---

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| HandlesMetric(metricDesc) | Returns true if metric category is "GPU % Utilization" | [GlobalGPUTracePlugin.cs:18](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L18) | Plugin registration |
| OnDataProcessed(sender, args) | Processes BUFFER_TYPE_GGPM_TRACE_DATA events | [GlobalGPUTracePlugin.cs:62](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L62) | GGPM data received |
| AddData(metricID, timestamp, data) | Accumulates metric data points, tracks bounds | [GlobalGPUTraceCaptureData.cs:18](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L18) | Processing database rows |
| AddDataToTracks(categoryName) | Creates UI graph tracks with all accumulated data | [GlobalGPUTraceCaptureData.cs:36](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L36) | After data collection |
| GetMetricTrackType(metricDesc) | Returns MetricTrackType.Custom | [GlobalGPUTracePlugin.cs:23](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L23) | Track type query |
| MetricDisplayName(metric) | Returns metric name property | [GlobalGPUTracePlugin.cs:28](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L28) | UI display |

---

## Call Flow Skeleton
```
Plugin Initialization
 └── GlobalGPUTracePlugin()
      └── Subscribe to ConnectionEvents.DataProcessed

Data Processing Flow
 ├── OnDataProcessed(sender, DataProcessedEventArgs)
 │    ├── Check if BufferCategory == BUFFER_TYPE_GGPM_TRACE_DATA
 │    ├── Get DataModel from ConnectionManager
 │    ├── Query GlobalGPUTraceModel
 │    ├── Get tblGlobalGPUTrace ModelObject
 │    ├── Query rows by CaptureID
 │    ├── new GlobalGPUTraceCaptureData(captureID)
 │    ├── For each row:
 │    │    ├── Extract MetricID (uint)
 │    │    ├── Extract Timestamp (long)
 │    │    ├── Extract Value (double)
 │    │    └── AddData(metricID, timestamp, value)
 │    │         ├── Create DataPointList if new metric
 │    │         ├── Append DataPoint(timestamp, value)
 │    │         └── Update DataBounds (min, max)
 │    └── AddDataToTracks("Global GPU")
 │
 └── AddDataToTracks(categoryName)
      ├── Check if GroupLayoutController exists for capture
      ├── Application.Invoke(delegate)
      │    ├── Get GroupLayoutController from TraceModel
      │    ├── Create group if not exists
      │    │    ├── new AddGroupCommand()
      │    │    ├── Set GroupName = "Global GPU Metrics"
      │    │    └── ExecuteCommand → creates GroupController
      │    │
      │    └── For each metric in m_counterData:
      │         ├── Get Metric by MetricID
      │         ├── Create GraphTrackController if needed
      │         │    ├── new AddTrackToGroupCommand()
      │         │    ├── Set TrackType = Graph
      │         │    └── ExecuteCommand → creates GraphTrackController
      │         │
      │         ├── Get metric category color
      │         ├── Set graph header background color
      │         ├── new AddMetricToTrackCommand()
      │         ├── Set metric name
      │         ├── ExecuteCommand
      │         ├── SetDrawMode(DRAW_STEPPED_LINE)
      │         ├── AddTransientMetricData(name, dataPoints)
      │         └── SetDataBounds(min, max)
      │
      └── UI tracks created and populated
```

---

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| GlobalGPUTraceCaptureData | OnDataProcessed | AddDataToTracks | Method exit (transient) |
| m_counterData (per metric) | AddData() | AddDataToTracks | Plugin shutdown |
| DataPointList | AddData() | GraphTrackController.AddTransientMetricData | Track disposal |
| GraphTrackController | AddTrackToGroupCommand | UI rendering | Track removal |
| GroupController | AddGroupCommand | Track container | Group disposal |
| DataBounds (min/max) | AddData() | SetDataBounds | Method exit |

---

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `GlobalGPUTracePlugin.dll` | Console logs | Plugin assembly loaded |
| `GPU % Utilization` | [GlobalGPUTracePlugin.cs:20](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L20) | Metric category this plugin handles |
| BUFFER_TYPE_GGPM_TRACE_DATA | [GlobalGPUTracePlugin.cs:66](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L66) | Buffer type filter |
| GlobalGPUTraceModel | [GlobalGPUTracePlugin.cs:72](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L72) | Database model name |
| tblGlobalGPUTrace | [GlobalGPUTracePlugin.cs:74](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L74) | Database table name |
| "Global GPU Metrics" | [GlobalGPUTraceCaptureData.cs:54](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L54) | UI group name created |

---

## Component Architecture
```
GlobalGPUTracePlugin.dll (2 .cs files, ~150 lines)
├── GlobalGPUTracePlugin.cs (84 lines)
│   ├── IMetricPlugin implementation
│   ├── HandlesMetric() → filters "GPU % Utilization"
│   ├── OnDataProcessed() → event handler
│   └── Container property (GroupLayoutController)
│
└── GlobalGPUTraceCaptureData.cs (120 lines)
    ├── AddData() → accumulates metric points
    ├── AddDataToTracks() → creates UI visualization
    ├── DataBounds struct
    └── Private state:
        ├── m_counterData: Dictionary<metricID, DataPointList>
        ├── m_graphControllers: Dictionary<metricID, GraphTrackController>
        ├── m_groupController: Dictionary<int, GroupController>
        └── m_dataBounds: DataBounds (min, max timestamp)
```

---

## Dependencies
```
SDPClientFramework.dll:
  - SdpApp (EventsManager, ConnectionManager, CommandManager, ModelManager)
  - IMetricPlugin interface
  - DataProcessedEventArgs
  - GroupLayoutController, GraphTrackController
  - Command pattern (AddGroupCommand, AddTrackToGroupCommand, AddMetricToTrackCommand)
  - DataModel, Model, ModelObject
  - MetricTrackType enum

SDPCoreWrapper.dll:
  - SDPCore.BUFFER_TYPE_GGPM_TRACE_DATA
  - SDPCore.DSP_MODEL_ATTRIB_CAPTURE_ID

Gtk/Cairo:
  - Application.Invoke (UI thread marshaling)
  - Color (graph header colors)

Sdp.Helpers:
  - UintConverter, Int64Converter, DoubleConverter
```

---

## Database Schema
```sql
-- Model: GlobalGPUTraceModel
-- Table: tblGlobalGPUTrace
-- Columns (inferred from code):
SELECT 
    MetricID,      -- uint: Metric identifier
    Timestamp,     -- long: Time of measurement
    Value,         -- double: GPU utilization percentage
    [CaptureID]    -- uint: Query filter (DSP_MODEL_ATTRIB_CAPTURE_ID)
FROM tblGlobalGPUTrace
WHERE CaptureID = ?
```

---

## Usage Pattern
```csharp
// Plugin is auto-loaded by framework
// 1. Plugin registration (framework calls)
if (plugin.HandlesMetric(metricDesc)) {
    // Plugin handles "GPU % Utilization" category
}

// 2. Data arrives
ConnectionEvents.DataProcessed → OnDataProcessed()
    if (args.BufferCategory == BUFFER_TYPE_GGPM_TRACE_DATA) {
        // Query database
        var data = new GlobalGPUTraceCaptureData(captureID);
        foreach (row in dbResults) {
            data.AddData(metricID, timestamp, value);
        }
        // Create visualization
        data.AddDataToTracks("Global GPU");
    }

// 3. UI displays graph tracks
// User sees "Global GPU Metrics" group with stepped-line graphs
```

---

## Search Hints
```
Find plugin entry:
search "class GlobalGPUTracePlugin.*IMetricPlugin"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:8

Find data processing:
search "OnDataProcessed.*GGPM"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:62

Find track creation:
search "AddDataToTracks"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs:36

Find metric category:
search "GPU.*Utilization"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:20

Find buffer type:
search "BUFFER_TYPE_GGPM_TRACE_DATA"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:66

Find database table:
search "tblGlobalGPUTrace"
open dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs:74
```

---

## Integration Points
| External System | Integration Method | Location |
|----------------|-------------------|----------|
| SDPClientFramework | IMetricPlugin interface implementation | [GlobalGPUTracePlugin.cs:8](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L8) |
| ConnectionEvents | DataProcessed event subscription | [GlobalGPUTracePlugin.cs:14](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L14) |
| DataModel | Query GlobalGPUTraceModel database | [GlobalGPUTracePlugin.cs:72](dll/project/GlobalGPUTracePlugin/GlobalGPUTracePlugin.cs#L72) |
| TraceModel | Access GroupLayoutControllers | [GlobalGPUTraceCaptureData.cs:40](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L40) |
| CommandManager | Execute UI commands (AddGroup, AddTrack, AddMetric) | [GlobalGPUTraceCaptureData.cs:55+](dll/project/GlobalGPUTracePlugin/GlobalGPUTraceCaptureData.cs#L55) |
| GTK UI Thread | Application.Invoke for thread safety | [GlobalGPUTraceCaptureData.cs:44](dll/project/GlobalGPUTraceCaptureData.cs#L44) |

---

## Notes
- **Lightweight Plugin**: Only 2 source files, ~150 total lines
- **Single Responsibility**: Handles only "GPU % Utilization" metric category
- **Transient Data**: GlobalGPUTraceCaptureData created per data processing event, not persisted
- **UI Thread Safety**: Uses Application.Invoke for GTK UI updates from background thread
- **Stepped Line Graphs**: Specifically uses DRAW_STEPPED_LINE mode (not smooth curves)
- **Metric Colors**: Retrieves category colors from ConnectionModel for consistent branding
- **No Capture Start**: StartCapture() is empty - plugin only visualizes existing data
- **Database-Driven**: All data comes from tblGlobalGPUTrace table queries
- **GGPM Integration**: GGPM (Global GPU Performance Monitor) is the data source
- **Group Creation**: Creates "Global GPU Metrics" group in trace view
- **Per-Metric Tracks**: Each MetricID gets its own GraphTrackController
- **Data Bounds**: Tracks min/max timestamps for proper graph scaling

---

## Typical Workflow

### Plugin Loading
1. Framework discovers GlobalGPUTracePlugin.dll
2. Instantiates GlobalGPUTracePlugin
3. Plugin subscribes to ConnectionEvents.DataProcessed

### Data Visualization
1. Device captures GPU utilization metrics
2. Data stored in tblGlobalGPUTrace database table
3. Buffer transfer completes (BUFFER_TYPE_GGPM_TRACE_DATA)
4. OnDataProcessed event fires
5. Plugin queries database by CaptureID
6. Creates GlobalGPUTraceCaptureData instance
7. Accumulates DataPoints for each metric
8. Invokes AddDataToTracks on UI thread
9. Creates "Global GPU Metrics" group if needed
10. For each metric:
    - Create GraphTrackController
    - Set stepped line draw mode
    - Apply metric category color
    - Add transient data points
    - Set time bounds
11. User sees real-time graphs in trace view
