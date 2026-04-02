# MODULE INDEX — SDP.ClientFramework — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: Snapdragon Profiler, Desktop Client, UI Framework, Device Connection, Profiling
Concepts: Application Lifecycle, Event-Driven Architecture, MVC Pattern, Plugin System, Capture Management, Device Management
Common Logs: Logger.LogError, Logger.LogInformation, Logger.LogWarning, ConnectionManager, AnalyticsManager
Entry Symbols: SdpApp, MainWindowController, ConnectionManager, ModelManager, UIManager, EventsManager

## Role
Snapdragon Profiler desktop client orchestrator managing device connectivity, capture workflows (snapshot/trace/sampling/realtime), data visualization lifecycle, plugin system, command pattern, and event-driven UI coordination.

## Entry Points
| Symbol | Location |
|--------|----------|
| SdpApp | [Sdp/SdpApp.cs:9](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L9) |
| SdpApp.Init | [Sdp/SdpApp.cs:120](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L120) |
| MainWindowController | [Sdp/MainWindowController.cs:8](../../../dll/project/SDPClientFramework/Sdp/MainWindowController.cs#L8) |
| ConnectionManager | [Sdp/ConnectionManager.cs:15](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L15) |
| ConnectionManager.InitCore | [Sdp/ConnectionManager.cs:56](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L56) |
| ModelManager | [Sdp/ModelManager.cs:6](../../../dll/project/SDPClientFramework/Sdp/ModelManager.cs#L6) |
| UIManager | [Sdp/UIManager.cs:17](../../../dll/project/SDPClientFramework/Sdp/UIManager.cs#L17) |
| EventsManager | [Sdp/EventsManager.cs:6](../../../dll/project/SDPClientFramework/Sdp/EventsManager.cs#L6) |
| CommandManager | [Sdp/CommandManager.cs:7](../../../dll/project/SDPClientFramework/Sdp/CommandManager.cs#L7) |
| PluginManager | [Sdp/PluginManager.cs:9](../../../dll/project/SDPClientFramework/Sdp/PluginManager.cs#L9) |

## Key Classes
| Class | Responsibility | Location |
|------|----------------|----------|
| SdpApp | Static application facade exposing all managers and lifecycle | [SdpApp.cs:9](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L9) |
| MainWindowController | Main window orchestration, menu, toolbar, docking | [MainWindowController.cs:8](../../../dll/project/SDPClientFramework/Sdp/MainWindowController.cs#L8) |
| ConnectionManager | Device connection, metric registration, client lifecycle | [ConnectionManager.cs:15](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L15) |
| ModelManager | Central model registry (14 models) | [ModelManager.cs:6](../../../dll/project/SDPClientFramework/Sdp/ModelManager.cs#L6) |
| UIManager | View/dialog creation, layout management, plugin UI | [UIManager.cs:17](../../../dll/project/SDPClientFramework/Sdp/UIManager.cs#L17) |
| EventsManager | Event hub (27 event delegates) with type-safe Raise | [EventsManager.cs:6](../../../dll/project/SDPClientFramework/Sdp/EventsManager.cs#L6) |
| CommandManager | Undo/Redo stack for command pattern | [CommandManager.cs:7](../../../dll/project/SDPClientFramework/Sdp/CommandManager.cs#L7) |
| PluginManager | Plugin discovery from /plugins directory | [PluginManager.cs:9](../../../dll/project/SDPClientFramework/Sdp/PluginManager.cs#L9) |
| SnapshotController | Snapshot capture workflow orchestration | [SnapshotController.cs:8](../../../dll/project/SDPClientFramework/Sdp/SnapshotController.cs#L8) |
| SamplingController | CPU sampling capture workflow | [SamplingController.cs:7](../../../dll/project/SDPClientFramework/Sdp/SamplingController.cs#L7) |
| DataSourcesController | Metric/process selection UI logic | [DataSourcesController.cs:13](../../../dll/project/SDPClientFramework/Sdp/DataSourcesController.cs#L13) |
| AnalyticsManager | Analytics tracking and crash detection | [AnalyticsManager.cs:16](../../../dll/project/SDPClientFramework/Sdp/AnalyticsManager.cs#L16) |
| StatisticsManager | Statistic registration and dialog management | [StatisticsManager.cs:7](../../../dll/project/SDPClientFramework/Sdp/StatisticsManager.cs#L7) |
| InstrumentedCodeProcessor | Instrumented code data processing | [InstrumentedCodeProcessor.cs](../../../dll/project/SDPClientFramework/Sdp/InstrumentedCodeProcessor.cs) |
| ApplicationModel | Version and build date | [ApplicationModel.cs:6](../../../dll/project/SDPClientFramework/Sdp/ApplicationModel.cs#L6) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| SdpApp.Init | Initialize all managers except plugins | [SdpApp.cs:120](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L120) | Application startup |
| SdpApp.InitPlugins | Discover and load plugins | [SdpApp.cs:135](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L135) | After Init, before InitCore |
| SdpApp.InitCore | Initialize connection core with session settings | [SdpApp.cs:142](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L142) | After plugin init |
| SdpApp.Shutdown | Teardown all managers and raise AppShutdown | [SdpApp.cs:154](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L154) | Application exit |
| SdpApp.ExecuteCommand | Execute command through CommandManager | [SdpApp.cs:174](../../../dll/project/SDPClientFramework/Sdp/SdpApp.cs#L174) | UI actions |
| ConnectionManager.InitCore | Setup client, delegates, ADB/SSH paths, device search | [ConnectionManager.cs:56](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L56) | After SdpApp.InitCore |
| ConnectionManager.ReloadDeviceConnectionsAndSearch | Start device discovery | [ConnectionManager.cs:150](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L150) | Init or manual refresh |
| EventsManager.Raise | Type-safe event invocation | [EventsManager.cs:195](../../../dll/project/SDPClientFramework/Sdp/EventsManager.cs#L195) | Throughout framework |
| UIManager.LoadMainWindow | Create and load main window | [UIManager.cs:94](../../../dll/project/SDPClientFramework/Sdp/UIManager.cs#L94) | Startup after InitCore |
| UIManager.CreateDialog | Factory for dialog views | [UIManager.cs:137](../../../dll/project/SDPClientFramework/Sdp/UIManager.cs#L137) | Dialog creation |
| CommandManager.ExecuteCommand | Execute command, push to undo stack | [CommandManager.cs:29](../../../dll/project/SDPClientFramework/Sdp/CommandManager.cs#L29) | Command execution |
| CommandManager.Undo | Pop undo stack, execute Undo, push redo | [CommandManager.cs:44](../../../dll/project/SDPClientFramework/Sdp/CommandManager.cs#L44) | Ctrl+Z |
| CommandManager.Redo | Pop redo stack, execute Redo, push undo | [CommandManager.cs:54](../../../dll/project/SDPClientFramework/Sdp/CommandManager.cs#L54) | Ctrl+Y |
| PluginManager.GetMetricPlugin | Find plugin handling metric | [PluginManager.cs:72](../../../dll/project/SDPClientFramework/Sdp/PluginManager.cs#L72) | Metric processing |
| ModelManager (constructor) | Instantiate 14 models | [ModelManager.cs:9](../../../dll/project/SDPClientFramework/Sdp/ModelManager.cs#L9) | SdpApp.Init |
| SnapshotController.Invalidate | Request snapshot capture | [SnapshotController.cs:46](../../../dll/project/SDPClientFramework/Sdp/SnapshotController.cs#L46) | Capture button click |
| MainWindowController.InitMainMenu | Build menu structure | [MainWindowController.cs:56](../../../dll/project/SDPClientFramework/Sdp/MainWindowController.cs#L56) | Controller construction |
| MainWindowController.InitMainToolBar | Build toolbar | [MainWindowController.cs:62](../../../dll/project/SDPClientFramework/Sdp/MainWindowController.cs#L62) | Controller construction |

## Call Flow Skeleton
```
Application Startup:
Main
 └── SdpApp.Init(platform)
      ├── new CommandManager()
      ├── new EventsManager() → creates 27 event delegates
      ├── new ModelManager() → instantiates 14 models
      ├── new ConnectionManager()
      ├── new UIManager()
      ├── new AnalyticsManager()
      ├── new StatisticsManager()
      └── new ActionQueue()
 └── SdpApp.InitPlugins()
      ├── new PluginManager() → scans /plugins/*.dll
      └── EventsManager.Raise(RegisterDefaultToolPlugins)
 └── SdpApp.InitCore(sessionSettings)
      ├── ConnectionManager.InitCore()
      │    ├── Client.Init(sessionSettings)
      │    ├── new InternalClientDelegate
      │    ├── new InternalMetricDelegate
      │    ├── MetricManager.RegisterMetricEventDelegate
      │    ├── DeviceManager config (ADB/SSH paths, simpleperf)
      │    └── ReloadDeviceConnectionsAndSearch()
      └── EventsManager.Raise(InitComplete)
 └── UIManager.LoadMainWindow()
      ├── CreateMainWindow()
      ├── new MainWindowController(view)
      │    ├── InitMainMenu()
      │    └── InitMainToolBar()
      ├── LoadUI() → register view/dialog providers
      └── SetSelectedLayout("Connect")
 └── UIManager.ShowMainWindow()

Command Execution Flow:
User Action
 └── SdpApp.ExecuteCommand(command)
      └── CommandManager.ExecuteCommand(command)
           ├── command.Execute()
           ├── m_undo_stack.Push(command) [unless ClearsUndo]
           └── m_redo_stack.Clear()

Undo/Redo Flow:
User Undo
 └── CommandManager.Undo()
      ├── command = m_undo_stack.Pop()
      ├── command.Undo()
      └── m_redo_stack.Push(command)

Event Flow:
Component
 └── SdpApp.EventsManager.Raise<T>(eventDelegate, sender, args)
      └── eventDelegate?.Invoke(sender, args)
           └── All registered handlers execute

Capture Flow (Snapshot Example):
User clicks Capture
 └── SnapshotController.m_view_CaptureClicked
      └── ConnectionEvents.SnapshotRequest raised
           └── ConnectionManager.connectionEvents_SnapshotRequest
                ├── Client.StartCapture()
                └── ConnectionEvents.CaptureCompleted raised
                     └── SnapshotController handles completion

Plugin Discovery:
PluginManager constructor
 ├── Directory.GetFiles("plugins", "*.dll")
 ├── For each DLL:
 │    ├── Assembly.Load(assemblyName)
 │    └── For each Type in assembly:
 │         ├── If implements IMetricPlugin → m_metricPlugins.Add
 │         └── If implements IToolPlugin → m_toolPlugins.Add
 └── RegisterDefaultToolPlugins event raised

Shutdown Flow:
User exits
 └── SdpApp.Shutdown()
      ├── AnalyticsManager.Shutdown()
      ├── EventsManager.Raise(AppShutdown)
      ├── ConnectionManager.Shutdown()
      ├── PluginManager.Shutdown()
      └── Null all managers
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| CommandManager | SdpApp.Init | SdpApp.ExecuteCommand, Undo/Redo | SdpApp.Shutdown |
| EventsManager | SdpApp.Init | All components (Raise/subscribe) | SdpApp.Shutdown |
| ModelManager (and 14 models) | SdpApp.Init | All controllers/views | SdpApp.Shutdown |
| ConnectionManager | SdpApp.Init | Controllers, device operations | SdpApp.Shutdown |
| UIManager | SdpApp.Init | View/dialog creation, layout | SdpApp.Shutdown |
| PluginManager | SdpApp.InitPlugins | Metric processing, tool launch | PluginManager.Shutdown |
| m_undo_stack | CommandManager | ExecuteCommand, Undo | Clear on command execute |
| m_redo_stack | CommandManager | Redo | Clear on new command |
| Client (m_client) | ConnectionManager.InitCore | Device operations, captures | ConnectionManager.Shutdown |
| InternalClientDelegate | ConnectionManager.InitCore | Client event callbacks | ConnectionManager |
| InternalMetricDelegate | ConnectionManager.InitCore | Metric event callbacks | ConnectionManager |
| m_metricPlugins | PluginManager constructor | GetMetricPlugin | PluginManager.Shutdown |
| m_toolPlugins | PluginManager constructor | Tool registration | PluginManager.Shutdown |
| MainWindowController | UIManager.LoadMainWindow | Window lifecycle | MainWindowClosing event |
| ActionQueue (ClientActionQueue) | SdpApp.Init | Event marshalling to UI thread | SdpApp.Shutdown |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| Logger.LogError | [ConnectionManager.cs:746](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L746) | Unrecognized rendering API |
| Logger.LogInformation | [ConnectionManager.cs:903](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L903) | App restart success |
| Logger.LogError | [ConnectionManager.cs:906](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L906) | App restart failure |
| Logger.LogError | [ConnectionManager.cs:967](../../../dll/project/SDPClientFramework/Sdp/ConnectionManager.cs#L967) | Stop app not implemented for OS |
| Logger.LogWarning | [AnalyticsManager.cs:62](../../../dll/project/SDPClientFramework/Sdp/AnalyticsManager.cs#L62) | crash.txt is empty |
| Logger.LogError | [AnalyticsManager.cs:73](../../../dll/project/SDPClientFramework/Sdp/AnalyticsManager.cs#L73) | Failed to read crash.txt |
| Logger.LogError | [AnalyticsManager.cs:196](../../../dll/project/SDPClientFramework/Sdp/AnalyticsManager.cs#L196) | Analytics event error |
| Logger.LogInformation | [AnalyticsManager.cs:459](../../../dll/project/SDPClientFramework/Sdp/AnalyticsManager.cs#L459) | Crash was detected |
| m_logger.LogWarning | [GanttTrackController.cs:374](../../../dll/project/SDPClientFramework/Sdp/GanttTrackController.cs#L374) | Gantt track warning |
| Logger.LogError | [Helpers/TypeConverter.cs:30](../../../dll/project/SDPClientFramework/Sdp/Helpers/TypeConverter.cs#L30) | Type conversion error |
| m_Logger.LogError | [InternalClientDelegate.cs:35](../../../dll/project/SDPClientFramework/Sdp/InternalClientDelegate.cs#L35) | Failed to create realtime capture |
| ConnectionManager.Logger | Multiple ConnectionManager methods | Device/metric operations |
| AnalyticsManager.Logger | Multiple AnalyticsManager methods | Analytics/crash tracking |

## Search Hints
```
Find application entry:
search "class SdpApp"
search "public static bool Init"

Find manager initialization:
search "SdpApp.Init|InitCore|InitPlugins"

Find event system:
search "class EventsManager"
search "EventsManager.Raise"

Find command execution:
search "ExecuteCommand|CommandManager"

Find capture workflows:
search "SnapshotController|SamplingController|TraceModel|RealtimeModel"

Find device connection:
search "ConnectionManager.InitCore|ReloadDeviceConnectionsAndSearch"

Find plugin system:
search "PluginManager|IMetricPlugin|IToolPlugin"

Find UI lifecycle:
search "UIManager.LoadMainWindow|CreateDialog"

Find logging:
search "Logger.LogError|Logger.LogInformation|Logger.LogWarning"

Jump to orchestrators:
open dll/project/SDPClientFramework/Sdp/SdpApp.cs:9
open dll/project/SDPClientFramework/Sdp/ConnectionManager.cs:15
open dll/project/SDPClientFramework/Sdp/MainWindowController.cs:8
open dll/project/SDPClientFramework/Sdp/EventsManager.cs:6
```
