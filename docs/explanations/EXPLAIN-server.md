---
type: explanation
topic: Server 模式完整实现
module_key: SDPCLI.Server
source_scope:
  - SDPCLI/source/Modes/ServerMode.cs
  - SDPCLI/source/Server/
module_index: docs/index/modules/SDPCLI.Server.md
based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - PLAN-2026-04-11-http-server-mode.md
status: wip
audience:
  - self
  - onboarding
last_updated: 2026-04-14
---

# SDPCLI Server 模式：工作原理与实现详解

## What this document explains

本文档详细解析 Server 模式的完整实现：HTTP REST API 路由表、异步 Job 系统（数据结构与生命周期）、设备状态机（6 状态 + TryTransition 原子转换）、4 个 JobRunner 的分阶段执行（ConnectJobRunner / LaunchJobRunner / CaptureJobRunner / AnalysisJobRunner）、ManualResetEvent 异步桥接模式、Request/Response 格式、安全设计。

## Scope

- **Included**：ServerMode 总体结构、HTTP 路由表（11 端点）、Job/AnalysisJob 数据结构、JobStatus 6 状态、JobManager.Submit() 执行模型、DeviceSession 状态机（6 状态）、TryTransition 原子操作、3 个 ManualResetEvent、4 个 JobRunner 各阶段（含进度值）、WaitHandleAsync 异步桥接、Request Body 格式、Job Response JSON 示例、典型工作流、安全设计、config.ini Server 键
- **Excluded**：AnalysisPipeline 内部步骤（见 EXPLAIN-analysis.md）、Snapshot 模式的详细时序（见 EXPLAIN-snapshot.md）、SDK DLL 架构（见 EXPLAIN-three-modes.md §二）

## Routing

- **ModuleKey**: SDPCLI.Server
- **Module Index**: [docs/index/modules/SDPCLI.Server.md](../index/modules/SDPCLI.Server.md)
- **SourceScope**: `SDPCLI/source/Modes/ServerMode.cs`, `SDPCLI/source/Server/`

## Context Basis

- **Implementations**: IMPL-2026-04-14-http-server-mode.md
- **Plans**: PLAN-2026-04-11-http-server-mode.md

## Reality Status

- **Stable**: HTTP 路由、Job 数据结构、状态机设计、AnalysisJobRunner ExecutionPlan
- **WIP**: 2026-04-14 新增，构建通过（0 错误 0 警告）但**未经实际设备测试**；CaptureJobRunner 的 CaptureExecutionService 初始化参数签名需实测验证
- **Outdated or conflicting**: `Server.AutoConnect` config 键已预留但 ConnectJobRunner 中未实现自动重连逻辑

---

## 一、设计目标

Server 模式将 Snapshot 和 Analysis 的能力包装为 **HTTP REST API**，解决的核心需求是：**让脚本、前端、CI 系统远程触发抓帧和分析，同时不阻塞调用方**。

关键设计决策：
- **所有耗时操作均为异步 Job**：connect（90s）、capture（3~5 分钟 replay）、analysis（LLM 更长）均不适合同步响应，统一返回 `202 + jobId`
- **状态机防并发**：设备操作通过原子 `TryTransition` 防止竞态（同时发两个 capture → 409）
- **Phase 边界取消**：JobManager 支持 cancel，取消在当前 phase 完成后生效，不中断 DB 写入
- **安全绑定**：只绑定 `localhost`，`sdpPath` 参数强制校验

---

## 二、总体结构

```
ServerMode.Run()
├── new DeviceSession(config)     ← 全局设备状态（包含三个 ManualResetEvent）
├── new JobManager(ttlMinutes)    ← 线程安全 Job 存储（ConcurrentDictionary）
├── new HttpServer(port, session, jobManager, config)
│   └── HttpServer.Start()        ← 后台线程：HttpListener accept loop
│       └── 每个请求 → ThreadPool.QueueUserWorkItem
│           └── HandlerRouter.Dispatch(path, method)
│               └── IHandler.Handle(ctx)  [override]
│
└── [主线程] Console 'q' 或 Ctrl+C 阻塞
    └── 周期性 jobManager.PurgeExpired()
    └── [退出时] HttpServer.Stop()
        └── JobManager.CancelAll()
        └── session.Disconnect()
```

---

## 三、HTTP 路由表

所有路由由 `HandlerRouter` 精确匹配（不使用正则，`string.Equals` 比较 path）：

| Method | Path | Handler | 同步/异步 |
|--------|------|---------|---------|
| GET | `/api/status` | `StatusHandler` | 同步 |
| GET | `/api/device` | `DeviceHandler` | 同步 |
| POST | `/api/connect` | `ConnectHandler` → `ConnectJobRunner` | 异步 Job → 202 |
| POST | `/api/disconnect` | `DisconnectHandler` | 同步（总是成功） |
| POST | `/api/session/launch` | `LaunchHandler` → `LaunchJobRunner` | 异步 Job → 202 |
| POST | `/api/capture` | `CaptureHandler` → `CaptureJobRunner` | 异步 Job → 202 |
| POST | `/api/analysis` | `AnalysisHandler` → `AnalysisJobRunner` | 异步 Job → 202 |
| GET | `/api/jobs` | `JobsHandler` | 同步 |
| GET | `/api/jobs/{id}` | `JobsHandler` | 同步 |
| POST | `/api/jobs/{id}/cancel` | `JobsHandler` | 同步（设置 CT） |
| DELETE | `/api/jobs/{id}` | `JobsHandler` | 同步 |

---

## 四、Job 系统

### 4.1 Job 数据结构

```csharp
public class Job
{
    public string    Id         { get; set; }   // "cap-20260414-120000-001"
    public JobType   Type       { get; set; }   // Connect | Launch | Capture | Analysis
    public JobStatus Status     { get; set; }   // Pending → Running → Completed/Failed/Cancelled
    public string?   Phase      { get; set; }   // 当前执行阶段名称
    public int       Progress   { get; set; }   // 0–100
    public DateTime  CreatedAt  { get; set; }
    public DateTime? StartedAt  { get; set; }
    public DateTime? FinishedAt { get; set; }
    public object?   Result     { get; set; }   // 完成后的输出（路径等）
    public string?   Error      { get; set; }   // 失败时的错误信息

    [JsonIgnore]
    public CancellationTokenSource Cts { get; set; }

    public bool IsTerminal =>
        Status is JobStatus.Completed or JobStatus.Failed or JobStatus.Cancelled;
}
```

`AnalysisJob` 是 `Job` 的子类，额外存储分析专属字段：

```csharp
public class AnalysisJob : Job
{
    public string         SdpPath      { get; set; }
    public uint           SnapshotId   { get; set; }
    public string?        OutputDir    { get; set; }
    public AnalysisTarget TargetsEnum  { get; set; }
}
```

### 4.2 JobStatus 状态

| 状态 | 说明 |
|------|------|
| `Pending` | 已创建，等待线程池调度 |
| `Running` | 执行中，`Phase` 指示当前步骤 |
| `Cancelling` | 已请求取消，当前 phase 仍在运行 |
| `Completed` | 成功，`Result` 包含输出路径等 |
| `Failed` | 异常，`Error` 包含原因 |
| `Cancelled` | 已取消 |

### 4.3 JobManager.Submit() 执行模型

```csharp
// JobManager.Submit(type, runner) 内部逻辑（伪代码）
var job = new Job { Id = GenerateId(), Type=type, Status=Pending, Cts=new() };
_jobs[job.Id] = job;
Task.Run(async () =>
{
    job.Status = Running;
    job.StartedAt = DateTime.UtcNow;
    try
    {
        await runner(job, job.Cts.Token);
        job.Status = Completed;
    }
    catch (OperationCanceledException)
    {
        job.Status = Cancelled;
    }
    catch (Exception ex)
    {
        job.Status = Failed;
        job.Error  = ex.Message;
    }
    finally { job.FinishedAt = DateTime.UtcNow; }
});
return job;
```

**SubmitAnalysis** 额外做重复检测（同一 sdpPath+snapshotId 只允许一个 Running Analysis）：

```csharp
public AnalysisJob? FindActiveAnalysis(string sdpPath, uint snapshotId)
    => _jobs.Values.OfType<AnalysisJob>()
        .FirstOrDefault(j => !j.IsTerminal
            && j.SdpPath == sdpPath
            && j.SnapshotId == snapshotId);
```

---

## 五、设备状态机

### 5.1 状态定义

```
Disconnected
  ↓ POST /api/connect → ConnectJobRunner
Connecting
  ↓ 成功
Connected
  ↓ POST /api/session/launch → LaunchJobRunner
Launching
  ↓ OnProcessAdded 确认
SessionActive
  ↓ POST /api/capture → CaptureJobRunner
Capturing
  ↓ 完成
SessionActive    ← 可继续触发下一帧
```

### 5.2 TryTransition 原子转换

```csharp
// DeviceSession.TryTransition
lock (_transitionLock)
{
    if (_status != expected) return false;
    _status = next;
    return true;
}
```

每个 Handler 在提交 Job 前检查转换，失败则直接返回 `409 Conflict`：

```csharp
// CaptureHandler.Handle()
if (!session.TryTransition(DeviceStatus.SessionActive, DeviceStatus.Capturing))
{
    WriteJson(ctx, 409, new { error = "Device not in SessionActive state", current = session.Status });
    return;
}
```

### 5.3 DeviceSession 内的三个 ManualResetEvent

SDK 回调（native 线程）触发这些事件，Job Runner 在 async 环境中等待：

| Event | 触发时机 | 等待方 |
|-------|---------|--------|
| `CaptureCompleteEvent` | `CliClientDelegate.OnCaptureComplete()` | `CaptureJobRunner` |
| `DataProcessedEvent` | `CliClientDelegate.OnDataProcessed()` | `CaptureJobRunner` |
| `ImportCompleteEvent` | Replay 完成，SDK 通知 | `CaptureJobRunner` |

---

## 六、各 Job Runner 实现

### 6.1 ConnectJobRunner (5 阶段)

```
Phase 1  initializing_sdk    (5%)   EnsureSdkInitialized → SdpApp.Init + QGLPlugin
Phase 2  finding_device      (20%)  CheckAndInstallAPKs
Phase 3  connecting          (40%)  DeviceConnectionService.Connect() → Device
Phase 4  verifying           (90%)  session.ConnectedDevice = device
Phase 5  completing          (100%) TryTransition(Connecting → Connected)
[失败时] finally: TryTransition(Connecting → Disconnected)
```

`EnsureSdkInitialized` 是全进程单例（静态 bool `SdkInitialized` 保护），避免多次 `SdpApp.Init`。

### 6.2 LaunchJobRunner (3 阶段)

```
Phase 1  checking_package  (10%)  验证 packageActivity 参数格式
Phase 2  launching_app     (40%)  AppLaunchService.Launch(packageName, activity)
Phase 3  waiting_process   (65%)  等待 OnProcessAdded（SDK 事件）
[完成]   TryTransition(Launching → SessionActive)
         session.ActiveApp = DeviceSessionInfo{Package, Activity, Pid}
[失败]   TryTransition(Launching → Connected)
```

### 6.3 CaptureJobRunner (7 阶段)

```
Phase 1  starting_capture  (10%)   CaptureExecutionService.StartCapture()
Phase 2  waiting_capture   (20%)   WaitHandleAsync(CaptureCompleteEvent, 30s, ct)
Phase 3  waiting_data      (35%)   WaitHandleAsync(DataProcessedEvent, 90s, ct)
Phase 4  importing         (50%)   QGLPluginService.ImportCapture() → VulkanSnapshot* 表
Phase 5  exporting         (70%)   VulkanSnapshotModel.Export*ToCSV() ×7 → CsvToDbService
Phase 6  screenshot        (85%)   DataExportService.ExportData()
Phase 7  archiving         (95%)   SessionArchiveService.CreateCaptureSummary()
[完成]   TryTransition(Capturing → SessionActive)
[失败]   TryTransition(Capturing → SessionActive)（always revert）

Result = { captureId, sdpPath, snapshotDir }
```

`WaitHandleAsync` 异步桥接：

```csharp
// native ManualResetEvent 在 async/await 环境中等待，同时响应 CancellationToken
private static async Task<bool> WaitHandleAsync(ManualResetEvent mre, int timeoutMs, CancellationToken ct)
{
    var waitTask   = Task.Run(() => mre.WaitOne(timeoutMs));
    var cancelTask = Task.Delay(Timeout.Infinite, ct);
    var winner     = await Task.WhenAny(waitTask, cancelTask);
    ct.ThrowIfCancellationRequested();
    return await waitTask;
}
```

### 6.4 AnalysisJobRunner (7 阶段)

`AnalysisJobRunner` 按 ExecutionPlan 分阶段调用 `AnalysisPipeline.RunAnalysis()`，每次调用传入单一目标 mask：

```csharp
private static readonly (string Phase, AnalysisTarget Mask, int ProgressEnd)[] ExecutionPlan =
{
    ("collect_dc",      AnalysisTarget.Dc,                                           12),
    ("extract_assets",  AnalysisTarget.Shaders | Textures | Buffers,                42),
    ("label_drawcalls", AnalysisTarget.Label,                                        65),
    ("join_metrics",    AnalysisTarget.Metrics,                                      75),
    ("generate_stats",  AnalysisTarget.Status | AnalysisTarget.TopDc,               85),
    ("report_llm",      AnalysisTarget.Analysis,                                     95),
    ("dashboard",       AnalysisTarget.Dashboard,                                   100),
};
```

每个 phase 前检查 CT（`ct.ThrowIfCancellationRequested()`），当前 phase 的 `RunAnalysis()` 调用在 `CancellationToken.None` 下运行，确保 DB 写入和 LLM 调用不被中途中断。

**completedTargets 累积**：每个 phase 完成后 `completedTargets |= mask`，传给下一次 `RunAnalysis()` 调用，防止 LLM 标注被重复调用（参见 EXPLAIN-analysis.md §8）。

---

## 七、Request Body 参数

```jsonc
// POST /api/connect
{
  "deviceId": "192.168.1.100:5555"   // 可选，省略则自动发现第一台设备
}

// POST /api/session/launch
{
  "packageActivity": "com.example.app/com.example.MainActivity"   // 必填
}

// POST /api/capture
{
  "outputDir": "D:/captures",   // 可选，覆盖 config.ini 的 SdpDir
  "label": "frame001"           // 可选，注释标记
}

// POST /api/analysis
{
  "sdpPath":    "D:/captures/2026-04-14.sdp",  // 必填，绝对路径，.sdp 后缀，禁 ".."
  "snapshotId": 2,                              // 必填，>= 2
  "outputDir":  "D:/analysis-out",             // 可选
  "targets":    "label,metrics,status"         // 可选，默认 all
}
```

---

## 八、Job Response 格式

`GET /api/jobs/{id}` 返回：

```jsonc
{
  "id":        "cap-20260414-120000-001",
  "type":      "Capture",                  // Connect | Launch | Capture | Analysis
  "status":    "Running",
  "phase":     "importing",                // 当前阶段
  "progress":  50,
  "createdAt": "2026-04-14T12:00:00.000Z",
  "startedAt": "2026-04-14T12:00:01.123Z",
  "result":    null,                       // 完成后填充
  "error":     null
}
```

Capture 完成后的 `result`：
```jsonc
{
  "captureId":   2,
  "sdpPath":     "D:/captures/2026-04-14T12-00-00.sdp",
  "snapshotDir": "D:/captures/2026-04-14T12-00-00/snapshot_2"
}
```

Analysis 完成后的 `result`：
```jsonc
{
  "sdpPath":    "D:/captures/2026-04-14T12-00-00.sdp",
  "captureId":  2,
  "sessionDir": "D:/analysis-out/2026-04-14T12-00-00",
  "captureDir": "D:/analysis-out/2026-04-14T12-00-00/snapshot_2",
  "targets":    "Dc, Shaders, Textures, Buffers, Label, Metrics, Status, TopDc"
}
```

---

## 九、典型工作流

```
1. POST /api/connect { "deviceId": "..." }
   GET  /api/jobs/con-... → Completed

2. POST /api/session/launch { "packageActivity": "..." }
   GET  /api/jobs/lnc-... → Completed

3. POST /api/capture {}
   GET  /api/jobs/cap-... → phase: "importing" → ... → Completed
   result.sdpPath  →  "D:/captures/2026-04-14.sdp"
   result.captureId → 2

4. POST /api/analysis { "sdpPath": "D:/captures/2026-04-14.sdp", "snapshotId": 2 }
   GET  /api/jobs/ana-... → phase: "label_drawcalls" → ... → Completed
   result.captureDir → "D:/analysis-out/2026-04-14/snapshot_2"

5. 读取分析结果：
   D:/analysis-out/2026-04-14/snapshot_2/dc.json
   D:/analysis-out/2026-04-14/snapshot_2/status.json
   D:/analysis-out/2026-04-14/snapshot_2/topdc.json
   D:/analysis-out/2026-04-14/snapshot_2/dashboard.md
```

---

## 十、安全设计

- **仅绑定 localhost**：`HttpListener` 前缀为 `http://localhost:{port}/`，不开放外部访问
- **sdpPath 校验**：`AnalysisHandler` 强制要求绝对路径 + `.sdp` 后缀 + 不含 `..`（防路径遍历）
- **无身份验证**：设计上为本机工具，无 token/auth（若需对外暴露需自行加反向代理）

---

## 十一、config.ini Server 相关键

| 键 | 默认值 | 说明 |
|----|--------|------|
| `Server.Port` | `5000` | HTTP 监听端口（可被 --port 覆盖） |
| `Server.JobTtlMinutes` | `60` | Job 在内存中保留时长，超时自动 PurgeExpired |

---

## 十二、代码现状（2026-04-14）

| 方面 | 状态 |
|------|------|
| 构建 | 0 错误，0 警告 |
| 实测 | **未经实际设备测试**；`CaptureJobRunner` 的 `CaptureExecutionService` 初始化参数签名需实测验证 |
| 重复分析保护 | `FindActiveAnalysis()` 按 (sdpPath, snapshotId) 做去重，但不阻止针对不同 snapshotId 的并发分析 |
| Server.AutoConnect | config 中预留了 `Server.AutoConnect` 键，ConnectJobRunner 中尚未实现自动重连逻辑 |
