---
type: plan
topic: capture-diagnostics
status: proposed
based_on:
  - FINDING-2026-04-10-snapshot5-app-crash.md
related_paths:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/CliClientDelegate.cs
related_tags: [snapshot, diagnostics, timeout, capture-result]
summary: >
  针对 snapshot_5 调查暴露的三个诊断盲区，在不改变捕帧逻辑前提下
  增加：Capture 完成状态区分、snapshot_N/ 写诊断文件、
  WaitForDataProcessed 等待时长告警、OnProcessStateChanged 日志过滤。
last_updated: 2026-04-11
---

## 背景

`FINDING-2026-04-10-snapshot5-app-crash.md` 确认：snapshot_5 为空的原因是
设备系统级崩溃中断了 GPU profiler 数据通路，`VULKAN_REPLAY_METRICS_DATA`
永远不会生成，`WaitForDataProcessed()` 静默等待 180 秒后才发现失败。

Delegate 层在此期间完全没有收到任何可提前感知失败的 signal——180s 超时是唯一机制。
"目标 App 崩溃检测"（C1）因此无效：本次目标 App PID=31279 全程存活，
是设备系统进程崩溃导致的中断，delegate 无法区分。

现有代码逻辑正确，但存在三个诊断盲区：

| # | 盲区 | 影响 |
|---|---|---|
| C2 | "Capture Complete" 不区分空/完整 | 用户误以为成功 |
| C3 | snapshot_N/ 为空时无诊断信息 | 事后无法判断失败原因 |
| C4 | 设备压力加剧时无 WARNING | 错过可操作的先兆信号 |

---

## 设计目标

- 不改变核心捕帧逻辑（`StartCapture`, `ReplayAndGetBuffers`, `ExportDrawCallData`）
- 不引入新的外部依赖
- 所有改动都是纯诊断性的（告警 + 写文件）
- 每项改动独立，可分批实现

---

## C2：Capture 完成状态区分

### 问题

目前无论 `dsbBuffer` 是否为 null，都执行：
```csharp
_log.Info($"[Capture] === Capture Complete === data saved to: {captureSubDir}");
Console.WriteLine("=== Capture Complete ===");
```

### 方案

在 capture loop 中，根据 `dsbBuffer` 是否为 null 和 wait result 输出不同消息：

```csharp
string captureStatus = (dsbBuffer != null) ? "COMPLETE" : "EMPTY";
string captureReason = (waitResult == CaptureDataWaitResult.Timeout) ? " (180s timeout)" : "";

_log.Info($"[Capture] === Capture {captureStatus}{captureReason} === data saved to: {captureSubDir}");
Console.WriteLine($"\n=== Capture {captureStatus}{captureReason} ===");
Console.WriteLine("Data saved to: " + captureSubDir);
```

新增枚举（嵌套在 `SnapshotCaptureMode.cs` 内）：
```csharp
private enum CaptureDataWaitResult { Ready, Timeout }
```

`WaitForDataProcessed()` 改为返回 `CaptureDataWaitResult`，供 C3/C4 复用。

无改动到现有逻辑路径，仅改日志字符串。

---

## C3：snapshot_N/ 写诊断文件 capture_result.json

### 问题

`snapshot_5/` 目录为空，事后无法从目录本身判断失败原因。

### 方案

在 `DataExportService.ExportData()` 调用之后、capture loop 末尾（`_captureEntries.Add` 之前），
写入 `captureSubDir/capture_result.json`：

```json
{
  "captureId": 5,
  "timestamp": "2026-04-10T20:24:19",
  "status": "empty",
  "reason": "timeout_180s",
  "waitDataSeconds": 180.0,
  "importCaptureResult": false,
  "dsbBufferAvailable": false
}
```

**`status` 值**：

| `status` | 条件 |
|---|---|
| `"complete"` | `dsbBuffer != null` |
| `"empty"` | `dsbBuffer == null` |

**`reason` 值**：

| `reason` | 条件 |
|---|---|
| `"timeout_180s"` | `waitResult == Timeout` |
| `"import_failed"` | `waitResult == Ready` 但 `ImportCapture` 返回 false |
| `"ok"` | 完整成功 |

文件始终写入（包括成功情况），方便工具和人眼快速确认。

**写文件位置**：`SnapshotCaptureMode.cs` capture loop 中，`ExportData` 之后。
不需要新服务类，直接用 `File.WriteAllText` + `System.Text.Json.JsonSerializer` 或手写 JSON 字符串。

---

## C4：WaitForDataProcessed 等待时长告警

### 问题

原设计用连续两次 capture 间隔时间作为设备压力代理，但该值由用户按 ENTER 时机主导，无效。

**正确指标是 `captureComplete → dataProcessed(Buffer=2)` 的实际等待时长**，
直接反映设备侧 GPU profiler 处理帧数据的速度。本次 session 的实测数据：

| Capture | 等待时长 | 状态 |
|---|---|---|
| 2 | 9.8s | 正常 |
| 3 | 22.1s | 正常 |
| 4 | **113.4s** | 严重劣化（慢 11 倍） |
| 5 | 180s 超时 | 失败 |

Capture 4 结束时若有告警，用户在 capture 5 之前就能看到设备已严重过载。

### 方案

在 `WaitForDataProcessed()` 内用 `Stopwatch` 记录实际等待时长：

```csharp
private CaptureDataWaitResult WaitForDataProcessed()
{
    _log.Info("[WaitForData] Waiting for snapshot API data (BufferID=2) — timeout=180s");
    var sw = System.Diagnostics.Stopwatch.StartNew();
    bool ready = _dataProcessedEvent.WaitOne(TimeSpan.FromSeconds(180));
    sw.Stop();
    double elapsed = sw.Elapsed.TotalSeconds;

    if (ready)
    {
        if (elapsed > 30.0)
            _log.Warning($"[WaitForData] API data took {elapsed:F1}s (>30s — device under pressure)");
        else
            _log.Info($"[WaitForData] API data ready in {elapsed:F1}s");
        return CaptureDataWaitResult.Ready;
    }
    _log.Warning($"[WaitForData] API data not received within 180s");
    return CaptureDataWaitResult.Timeout;
}
```

阈值 30s：正常值 9.8–22.1s，异常值 113.4s，30s 能清晰区分两侧。
等待时长同时写入 C3 的 `capture_result.json`（`waitDataSeconds` 字段）。

---

## C5：OnProcessStateChanged 过滤非目标进程日志

### 问题

`OnProcessStateChanged` 对每个 PID 都以 INFO 级别打印：
```
[INFO][Delegate] [Process State Changed] PID=31279, State=ProcessRunning
[WARN][Delegate]   [No linked metrics] Process hasn't used GPU APIs yet — GUI would skip it
```
系统进程（`com.miui.home`, `zygote64`, `com.xiaomi.xmsf` 等）产生大量无意义日志，
淹没目标 App 的信号。与此对比，`OnProcessAdded` 已有 `isTarget` 检查，对非目标进程用
`AppLogger.Debug` 并提前 return。

`OnProcessStateChanged` 缺少同样的过滤。

### 方案

在 `OnProcessStateChanged` 的 try 块中，取得 `processName` 后立即做目标判断：

```csharp
public override void OnProcessStateChanged(uint pid)
{
    // Forward to SdpApp.EventsManager (always required, unconditional)
    try { ... }
    catch { ... }

    try
    {
        Process? proc = ProcessManager.Get().GetProcess(pid);
        if (proc == null || !proc.IsValid()) return;

        ProcessProperties props = proc.GetProperties();
        string processName = props.name;
        ProcessState state = props.state;

        // ── 非目标进程：降到 DEBUG，提前返回 ──────────────────────────
        bool isTarget = !string.IsNullOrEmpty(_targetPackageName)
                        && processName.Contains(_targetPackageName);
        if (!isTarget)
        {
            AppLogger.Debug("Delegate", $"[Process State Changed] PID={pid} Name={processName}, State={state}");
            return;
        }

        // ── 目标进程：保留完整 INFO 日志 ──────────────────────────────
        AppLogger.Info("Delegate", $"[Process State Changed] PID={pid} Name={processName}, State={state}");

        if (state != ProcessState.ProcessRunning)
            AppLogger.Warn("Delegate", $"  Process state is {state} (not ProcessRunning) → will be removed");

        // linked metrics check (仅对目标进程)
        try
        {
            MetricIDList linkedMetrics = proc.GetLinkedMetrics();
            AppLogger.Debug("Delegate", $"  Linked Metrics: {linkedMetrics.Count}");
            if (linkedMetrics.Count == 0)
            {
                AppLogger.Warn("Delegate", "  [No linked metrics] Target process hasn't used GPU APIs yet");
            }
            else
            {
                foreach (uint metricId in linkedMetrics)
                {
                    try
                    {
                        Metric metric = MetricManager.Get().GetMetric(metricId);
                        if (metric != null && metric.IsValid())
                        {
                            MetricProperties mp = metric.GetProperties();
                            AppLogger.Debug("Delegate", $"    - {mp.name} (ID: {mp.id}, CaptureTypeMask: 0x{mp.captureTypeMask:X})");
                        }
                    }
                    catch { }
                }
            }
        }
        catch (Exception lmEx)
        {
            AppLogger.Warn("Delegate", $"  Error checking linked metrics: {lmEx.Message}");
        }
    }
    catch (Exception ex)
    {
        AppLogger.Error("Delegate", $"[Process State Changed] PID={pid} - error: {ex.Message}");
    }
}
```

### 注意事项

- SdpApp.EventsManager forward 必须在过滤之前，无论是否目标进程都需要 forward
- `_targetPackageName` 为 null（未设置时）：`isTarget = false` → 全部降到 DEBUG，行为退化但安全

---

## 实现优先级

| 优先级 | 改动 | 关键点 |
|---|---|---|
| P1 | C5 OnProcessStateChanged 过滤 | 改动最小，立即减少噪声 |
| P1 | C2 状态区分日志 | 改动最小，立即生效 |
| P2 | C4 等待时长告警 | 需改 WaitForDataProcessed 返回值，可与 C2 一批 |
| P2 | C3 capture_result.json | 依赖 C2/C4 的返回值和等待时长 |

C5 + C2 + C4 可作为最小批次实现（改动集中在 `SnapshotCaptureMode.cs` + `CliClientDelegate.cs` 各一处）。C3 再写 JSON 即可。

---

## 文件改动范围

| 文件 | 改动内容 |
|---|---|
| `SDPCLI/source/CliClientDelegate.cs` | C5: `OnProcessStateChanged` 加 `isTarget` 过滤 |
| `SDPCLI/source/Modes/SnapshotCaptureMode.cs` | C2: 日志字符串; C4: `WaitForDataProcessed` 返回值 + Stopwatch; C3: 写 JSON |

新增类/文件：**无**（`CaptureDataWaitResult` 枚举嵌套在 `SnapshotCaptureMode.cs` 内）。

---

## 验证方法

| 场景 | 预期结果 |
|---|---|
| 正常 capture（<30s） | `[WaitForData] API data ready in X.Xs`；`capture_result.json` status=complete, reason=ok |
| 设备压力大（>30s） | `[WaitForData] API data took XXs (>30s — device under pressure)` WARNING |
| 180s 超时 | `=== Capture EMPTY (180s timeout) ===`；`capture_result.json` status=empty, reason=timeout_180s |
| 非目标进程 state change | consolelog 中降为 DEBUG，不再出现 INFO 噪声 |

---

## 执行状态

未修改任何实现文件。
**Implementation requires the Executor agent.**
