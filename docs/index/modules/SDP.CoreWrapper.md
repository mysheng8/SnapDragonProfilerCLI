# MODULE INDEX — SDP.CoreWrapper — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: Profiler, Device Management, Capture, Metrics, Network, Session  
Concepts: SWIG wrapper, P/Invoke, managed interop, singleton managers, event delegates  
Common Logs: Device connection, capture lifecycle, metric registration, session management  
Entry Symbols: Client, DeviceManager, CaptureManager, MetricManager, SessionManager, NetworkManager, ProcessManager

## Role
C# managed wrapper layer providing SWIG-generated P/Invoke interop to Snapdragon Profiler native core library.

## Entry Points
| Symbol | Location |
|--------|----------|
| Client.Init | [dll/project/SDPCoreWrapper/Client.cs:59](dll/project/SDPCoreWrapper/Client.cs#L59) |
| Client.Shutdown | [dll/project/SDPCoreWrapper/Client.cs:80](dll/project/SDPCoreWrapper/Client.cs#L80) |
| DeviceManager.Get | [dll/project/SDPCoreWrapper/DeviceManager.cs:51](dll/project/SDPCoreWrapper/DeviceManager.cs#L51) |
| CaptureManager.Get | [dll/project/SDPCoreWrapper/CaptureManager.cs:51](dll/project/SDPCoreWrapper/CaptureManager.cs#L51) |
| MetricManager.Get | [dll/project/SDPCoreWrapper/MetricManager.cs:53](dll/project/SDPCoreWrapper/MetricManager.cs#L53) |
| NetworkManager.Get | [dll/project/SDPCoreWrapper/NetworkManager.cs:53](dll/project/SDPCoreWrapper/NetworkManager.cs#L53) |
| SessionManager.Get | [dll/project/SDPCoreWrapper/SessionManager.cs:53](dll/project/SDPCoreWrapper/SessionManager.cs#L53) |
| ProcessManager.Get | [dll/project/SDPCoreWrapper/ProcessManager.cs:53](dll/project/SDPCoreWrapper/ProcessManager.cs#L53) |
| DataModel.Get | [dll/project/SDPCoreWrapper/DataModel.cs:53](dll/project/SDPCoreWrapper/DataModel.cs#L53) |

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| Client | Main client orchestrator, coordinates managers and lifecycle | [dll/project/SDPCoreWrapper/Client.cs:5](dll/project/SDPCoreWrapper/Client.cs#L5) |
| DeviceManager | Device discovery, connection management, device lifecycle | [dll/project/SDPCoreWrapper/DeviceManager.cs:5](dll/project/SDPCoreWrapper/DeviceManager.cs#L5) |
| CaptureManager | Capture creation, lifecycle orchestration | [dll/project/SDPCoreWrapper/CaptureManager.cs:5](dll/project/SDPCoreWrapper/CaptureManager.cs#L5) |
| MetricManager | Metric and category registration, activation | [dll/project/SDPCoreWrapper/MetricManager.cs:5](dll/project/SDPCoreWrapper/MetricManager.cs#L5) |
| SessionManager | Session open/close, path management | [dll/project/SDPCoreWrapper/SessionManager.cs:5](dll/project/SDPCoreWrapper/SessionManager.cs#L5) |
| NetworkManager | Network communication, command routing | [dll/project/SDPCoreWrapper/NetworkManager.cs:5](dll/project/SDPCoreWrapper/NetworkManager.cs#L5) |
| ProcessManager | Process tracking, data model management | [dll/project/SDPCoreWrapper/ProcessManager.cs:5](dll/project/SDPCoreWrapper/ProcessManager.cs#L5) |
| Device | Device instance, state tracking, connection control | [dll/project/SDPCoreWrapper/Device.cs:5](dll/project/SDPCoreWrapper/Device.cs#L5) |
| Capture | Individual capture instance, start/stop/pause control | [dll/project/SDPCoreWrapper/Capture.cs:5](dll/project/SDPCoreWrapper/Capture.cs#L5) |
| Metric | Metric data representation | [dll/project/SDPCoreWrapper/Metric.cs:5](dll/project/SDPCoreWrapper/Metric.cs#L5) |
| DataModel | Data model singleton, model addition/deletion | [dll/project/SDPCoreWrapper/DataModel.cs:5](dll/project/SDPCoreWrapper/DataModel.cs#L5) |
| SessionSettings | Session configuration properties | [dll/project/SDPCoreWrapper/SessionSettings.cs:5](dll/project/SDPCoreWrapper/SessionSettings.cs#L5) |
| SDPCorePINVOKE | SWIG-generated P/Invoke marshaling layer | dll/project/SDPCoreWrapper/SDPCorePINVOKE.cs |
| Logger | Logging facility | [dll/project/SDPCoreWrapper/Logger.cs:6](dll/project/SDPCoreWrapper/Logger.cs#L6) |
| SDPCore | Constants, version info, DSP/NPU modes | [dll/project/SDPCoreWrapper/SDPCore.cs:4](dll/project/SDPCoreWrapper/SDPCore.cs#L4) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| Client.Init | Initialize client with session settings | [dll/project/SDPCoreWrapper/Client.cs:59](dll/project/SDPCoreWrapper/Client.cs#L59) | Application startup |
| Client.Shutdown | Shutdown client, release resources | [dll/project/SDPCoreWrapper/Client.cs:80](dll/project/SDPCoreWrapper/Client.cs#L80) | Application exit |
| Client.Update | Process pending client events | [dll/project/SDPCoreWrapper/Client.cs:84](dll/project/SDPCoreWrapper/Client.cs#L84) | Main update loop |
| Client.GetDeviceManager | Retrieve device manager instance | [dll/project/SDPCoreWrapper/Client.cs:128](dll/project/SDPCoreWrapper/Client.cs#L128) | Device operations needed |
| Client.GetCaptureManager | Retrieve capture manager instance | [dll/project/SDPCoreWrapper/Client.cs:134](dll/project/SDPCoreWrapper/Client.cs#L134) | Capture operations needed |
| DeviceManager.Init | Initialize device manager | [dll/project/SDPCoreWrapper/DeviceManager.cs:58](dll/project/SDPCoreWrapper/DeviceManager.cs#L58) | After manager acquisition |
| DeviceManager.AddDevice | Add device by name and IP | [dll/project/SDPCoreWrapper/DeviceManager.cs:67](dll/project/SDPCoreWrapper/DeviceManager.cs#L67) | Manual device addition |
| DeviceManager.FindDevices | Discover available devices | [dll/project/SDPCoreWrapper/DeviceManager.cs:91](dll/project/SDPCoreWrapper/DeviceManager.cs#L91) | Device discovery initiated |
| DeviceManager.GetConnectedDevice | Retrieve currently connected device | [dll/project/SDPCoreWrapper/DeviceManager.cs:127](dll/project/SDPCoreWrapper/DeviceManager.cs#L127) | Check connection status |
| Device.Connect | Connect to device with timeout/port | [dll/project/SDPCoreWrapper/Device.cs:78](dll/project/SDPCoreWrapper/Device.cs#L78) | User initiates connection |
| Device.Disconnect | Disconnect from device | [dll/project/SDPCoreWrapper/Device.cs:99](dll/project/SDPCoreWrapper/Device.cs#L99) | User disconnects or error |
| Device.GetDeviceState | Query device connection state | [dll/project/SDPCoreWrapper/Device.cs:115](dll/project/SDPCoreWrapper/Device.cs#L115) | State polling |
| CaptureManager.CreateCapture | Create new capture instance | [dll/project/SDPCoreWrapper/CaptureManager.cs:55](dll/project/SDPCoreWrapper/CaptureManager.cs#L55) | User initiates capture |
| CaptureManager.GetCapture | Retrieve capture by ID | [dll/project/SDPCoreWrapper/CaptureManager.cs:61](dll/project/SDPCoreWrapper/CaptureManager.cs#L61) | Access existing capture |
| Capture.Start | Start capture with settings | [dll/project/SDPCoreWrapper/Capture.cs:52](dll/project/SDPCoreWrapper/Capture.cs#L52) | Capture initiated |
| Capture.Stop | Stop active capture | [dll/project/SDPCoreWrapper/Capture.cs:60](dll/project/SDPCoreWrapper/Capture.cs#L60) | User stops or completed |
| SessionManager.OpenSession | Open profiling session | [dll/project/SDPCoreWrapper/SessionManager.cs:59](dll/project/SDPCoreWrapper/SessionManager.cs#L59) | New session started |
| SessionManager.CloseSession | Close active session | [dll/project/SDPCoreWrapper/SessionManager.cs:68](dll/project/SDPCoreWrapper/SessionManager.cs#L68) | Session complete |
| MetricManager.AddMetricCategory | Register new metric category | [dll/project/SDPCoreWrapper/MetricManager.cs:100](dll/project/SDPCoreWrapper/MetricManager.cs#L100) | Plugin adds category |
| MetricManager.Reset | Reset metric state | [dll/project/SDPCoreWrapper/MetricManager.cs:82](dll/project/SDPCoreWrapper/MetricManager.cs#L82) | Session reset |

## Call Flow Skeleton
```
Application Start
 ├── Client.Init(SessionSettings)
 │    ├── SessionManager.OpenSession
 │    └── Managers initialized
 ├── DeviceManager.Get()
 │    ├── DeviceManager.Init()
 │    ├── DeviceManager.FindDevices()
 │    └── RegisterEventDelegate(DeviceManagerDelegate)
 ├── Device selected
 │    ├── Device.Connect(timeout, port)
 │    └── OnDeviceConnected event
 └── Capture workflow
      ├── CaptureManager.Get()
      ├── CaptureManager.CreateCapture(type)
      ├── Capture.Start(CaptureSettings)
      ├── Client.Update() [loop]
      ├── Capture.Stop()
      └── OnCaptureComplete event

Session Close
 ├── Device.Disconnect()
 ├── SessionManager.CloseSession()
 └── Client.Shutdown()
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| Client | new Client() | Application | Client.Shutdown |
| SessionSettings | Application | Client.Init, SessionManager | GC |
| DeviceManager | DeviceManager.Get | Client, Application | Manager shutdown |
| Device | DeviceManager.AddDevice | DeviceManager, Application | DeviceManager.RemoveDevice |
| CaptureManager | CaptureManager.Get | Client, Application | Manager.ShutDown |
| Capture | CaptureManager.CreateCapture | CaptureManager, Application | Capture.Dispose |
| MetricManager | MetricManager.Get | Client, Application | Manager.ShutDown |
| Metric | MetricManager.AddMetric | MetricManager, Application | Manager.Reset |
| DataModel | DataModel.Get | Client.GetDataModel | DataModel.ShutDown |
| SessionManager | SessionManager.Get | Client, Application | Manager cleanup |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| Device connection | [dll/project/SDPCoreWrapper/Device.cs:78](dll/project/SDPCoreWrapper/Device.cs#L78) | Device.Connect initiated |
| Device state | [dll/project/SDPCoreWrapper/Device.cs:115](dll/project/SDPCoreWrapper/Device.cs#L115) | Device.GetDeviceState queried |
| Capture create | [dll/project/SDPCoreWrapper/CaptureManager.cs:55](dll/project/SDPCoreWrapper/CaptureManager.cs#L55) | CaptureManager.CreateCapture |
| Capture start | [dll/project/SDPCoreWrapper/Capture.cs:52](dll/project/SDPCoreWrapper/Capture.cs#L52) | Capture.Start triggered |
| Capture stop | [dll/project/SDPCoreWrapper/Capture.cs:60](dll/project/SDPCoreWrapper/Capture.cs#L60) | Capture.Stop triggered |
| Session open | [dll/project/SDPCoreWrapper/SessionManager.cs:59](dll/project/SDPCoreWrapper/SessionManager.cs#L59) | SessionManager.OpenSession |
| Session close | [dll/project/SDPCoreWrapper/SessionManager.cs:68](dll/project/SDPCoreWrapper/SessionManager.cs#L68) | SessionManager.CloseSession |
| Client init | [dll/project/SDPCoreWrapper/Client.cs:59](dll/project/SDPCoreWrapper/Client.cs#L59) | Client.Init started |
| Client shutdown | [dll/project/SDPCoreWrapper/Client.cs:80](dll/project/SDPCoreWrapper/Client.cs#L80) | Client.Shutdown started |
| Metric added | [dll/project/SDPCoreWrapper/MetricManager.cs:100](dll/project/SDPCoreWrapper/MetricManager.cs#L100) | MetricManager.AddMetricCategory |
| Device found | [dll/project/SDPCoreWrapper/DeviceManager.cs:91](dll/project/SDPCoreWrapper/DeviceManager.cs#L91) | DeviceManager.FindDevices |
| Network shutdown | [dll/project/SDPCoreWrapper/NetworkManager.cs:62](dll/project/SDPCoreWrapper/NetworkManager.cs#L62) | NetworkManager.ShutDown |
| SWIG exception | SDPCorePINVOKE.cs | P/Invoke marshaling error |
| Device disconnect | [dll/project/SDPCoreWrapper/Device.cs:99](dll/project/SDPCoreWrapper/Device.cs#L99) | Device.Disconnect |
| Capture pause | [dll/project/SDPCoreWrapper/Capture.cs:68](dll/project/SDPCoreWrapper/Capture.cs#L68) | Capture.Pause |
| Manager reset | [dll/project/SDPCoreWrapper/MetricManager.cs:82](dll/project/SDPCoreWrapper/MetricManager.cs#L82) | MetricManager.Reset |

## Search Hints
```
Find entry:
search "class Client|DeviceManager\.Get|CaptureManager\.Get"

Jump to manager:
open dll/project/SDPCoreWrapper/DeviceManager.cs:51
open dll/project/SDPCoreWrapper/CaptureManager.cs:51
open dll/project/SDPCoreWrapper/MetricManager.cs:53

Find delegates:
search "class.*Delegate.*IDisposable"

Find SWIG layer:
search "SDPCorePINVOKE\."

Find properties:
search "class.*Properties|class.*Settings"
```
