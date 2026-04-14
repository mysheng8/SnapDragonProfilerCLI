# MODULE INDEX — SDPCLI

## Role

.NET 4.7.2 CLI tool wrapping Qualcomm Snapdragon Profiler SDK.  
Three operational modes: **snapshot capture**, **analysis**, **HTTP server**.

---

## Sub-Modules

| ModuleKey | Coverage | Index |
|-----------|----------|-------|
| SDPCLI.Snapshot | Capture pipeline: device connect → GPU snapshot → CSV → sdp.db → .sdp ZIP | [SDPCLI.Snapshot.md](SDPCLI.Snapshot.md) |
| SDPCLI.Analysis | Analysis pipeline: open .sdp → query DB → extract shaders/textures → label DCs → Markdown report | [SDPCLI.Analysis.md](SDPCLI.Analysis.md) |
| SDPCLI.Server | HTTP REST server mode: localhost API, async job queue, device session state machine | [SDPCLI.Server.md](SDPCLI.Server.md) |

---

## Entry Points

| Symbol | Location |
|--------|----------|
| `Program.Main()` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L16) |
| `Program.SetupEnvironment()` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L140) |
| `Application.Run()` | [source/Application.cs](../../../SDPCLI/source/Application.cs#L100) |

---

## Mode Routing

```
Program.Main()
└── Application.Run()
    ├── mode=snapshot  → SnapshotCaptureMode.Run()   → see SDPCLI.Snapshot
    ├── mode=analysis  → AnalysisMode.Run()           → see SDPCLI.Analysis
    ├── mode=drawcall  → DrawCallAnalysisMode.Run()   → see SDPCLI.Analysis
    └── mode=server    → ServerMode.Run()             → see SDPCLI.Server
```

---

## Core Shared Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `Program` | DLL path setup, arg parse → `Application` | [source/Main.cs](../../../SDPCLI/source/Main.cs#L11) |
| `Application` | Mode routing, wires services | [source/Application.cs](../../../SDPCLI/source/Application.cs#L14) |
| `SDPClient` | Wraps `SDPCore.Client`; exposes `DeviceManager`, `SessionManager`, `CaptureManager` | [source/SDPClient.cs](../../../SDPCLI/source/SDPClient.cs) |
| `CliClientDelegate` | SDK callbacks: `OnCaptureComplete`, `OnDataProcessed`; caches `DsbBuffer`/`ApiBuffer`/`MetricsBuffer` | [source/CliClientDelegate.cs](../../../SDPCLI/source/CliClientDelegate.cs) |
