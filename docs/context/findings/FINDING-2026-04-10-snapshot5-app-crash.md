---
type: finding
topic: snapshot5-empty-device-instability
status: investigated
related_paths:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Services/Capture/CaptureExecutionService.cs
  - SDPCLI/bin/Debug/net472/.log/consolelog.txt
  - project/sdp/2026-04-10T20-11-30/sdplog.txt
related_tags: [snapshot5, empty-directory, device-instability, system-crash, 180s-timeout, import-capture, replay-metrics]
summary: >
  Session 2026-04-10T20-11-30：snapshot_5 目录被创建但完全为空。
  目标 App (com.ea.gp.fcmnova PID=31279) 全程未崩溃。
  根本原因是 capture 5 的 GPU replay 窗口期间设备发生系统级崩溃
  （com.android.quicksearchbox + com.miui.home 等系统进程崩溃，
  触发 zygote64 和大规模进程重启风暴），设备侧 GPU profiler 数据通路
  被中断，BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA 永远未生成，
  导致 180s 超时后 ImportCapture 返回 false，snapshot_5/ 为空。
last_updated: 2026-04-11
---

## 观察到的症状

会话 `project/sdp/2026-04-10T20-11-30/` 下：

| 文件/目录 | 状态 |
|---|---|
| `sdpframe_002.gfxrz` | ✓ 完整 |
| `sdpframe_003.gfxrz` | ✓ 完整 |
| `sdpframe_004.gfxrz` | ✓ 完整（但传输耗时 25767ms，异常慢） |
| `sdpframe_005.gfxrz` | ✗ 不存在 |
| `snapshot_2/` | ✓ 有数据 |
| `snapshot_3/` | ✓ 有数据 |
| `snapshot_4/` | ✓ 有数据 |
| `snapshot_5/` | ✗ 目录存在，但完全为空 |

---

## 证据链

### 1. 目标 App 状态确认（consolelog.txt）

目标 App `com.ea.gp.fcmnova` 在本 session 中运行为 **PID=31279**：
```
[20:11:47.743][Delegate] [ProcessAdded] PID=31279 Name=com.ea.gp.fcmnova
[20:11:53.761][Utility]  gpu_debug_app = com.ea.gp.fcmnova
```

**在整个 session（20:11–20:24）期间，PID=31279 从未出现 `ProcessRemoved` 记录**。
目标 App 全程存活，并非崩溃。

### 2. 设备系统级崩溃（consolelog.txt）

Capture 5 在 `20:20:19` 触发后约 42 秒，设备发生系统级崩溃，大量系统进程死亡并重启：

```
[20:21:04.073][Delegate] [ProcessRemoved] PID=10579   ← com.android.quicksearchbox（160 GPU metrics）
[20:21:04.284][Delegate] [ProcessRemoved] PID=8878    ← com.miui.home（160 GPU metrics）
```

**注意**：PID=10579 是 `com.android.quicksearchbox`，PID=8878 是 `com.miui.home`——
两者均为系统 UI 进程，与目标 App 无关。它们有 160 个 GPU 指标是因为这些系统 App
也使用 GPU 渲染（Android Launcher 和搜索框均有 GPU 绘制层）。

崩溃后设备进入大规模进程重启风暴（20:21:08–20:22:47）：
- `com.miui.home`, `com.android.camera`, `com.xiaomi.xmsf`, `zygote64` 等系统进程全部重启
- 这是典型的 Launcher crash → zygote restart → 系统恢复序列

### 3. Shader Data 抵达，但 Replay Metrics 永远未到（sdplog.txt）

```
20:21:17 — BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA received   ← 已在传输中，正常到达
              （此后 sdplog 对 capture 5 的记录完全终止）
              BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA — 永远未到达
```

`VULKAN_SNAPSHOT_SHADER_DATA` 是 capture 触发后就开始传输的 shader 数据，在系统稳定时
已在途，所以正常到达。`VULKAN_REPLAY_METRICS_DATA` 需要设备侧 GPU profiler 完成一轮完整
帧回放才能生成，系统崩溃重启期间 GPU profiler 数据通路被中断，回放从未完成。

### 4. 180 秒超时触发（consolelog.txt）

```
20:20:19.936  [WaitForData] Waiting for snapshot API data (BufferID=2) — timeout=180s
20:23:19.943  [WaitForData] API data not received within 180 seconds — replay may produce empty results.
```

等待 180 秒（20:20:19 → 20:23:19），没有收到 BufferID=2。

### 5. ImportCapture 直接返回 False（consolelog.txt）

```
20:23:19.945  [ImportCapture] Calling QGLPluginService.ImportCapture captureId=5 db=.../sdp.db
20:23:19.945  [ImportCapture] ImportCapture returned: False
20:23:19.945  [ImportCapture] ImportCapture returned false — no replay data
20:23:19.945  [Replay] ReplayAndGetBuffers returned: null
20:23:19.945  [Replay] dsbBuffer is null — skipping ExportDrawCallData
```

在没有设备侧 REPLAY_METRICS_DATA 的情况下，`QGLPluginService.ImportCapture` 直接返回 `false`，
`dsbBuffer` 为 null，`ExportDrawCallData` 被跳过。

### 6. 第二个 60 秒超时（consolelog.txt）

```
20:23:19.945  [Capture] Waiting for ImportCapture replay completion (bufferID=1) — timeout=60s
20:24:19.957  [Capture] ImportCapture trailing event not received within 60s — proceeding anyway
20:24:19.957  [Capture] === Capture Complete === data saved to: .../snapshot_5
```

等待 trailing event（bufferID=1）也超时。最终 "Capture Complete" 打印，但 `snapshot_5/` 为空。

---

## 完整时间序列

| 时间 | 事件 |
|---|---|
| `20:11:47` | 目标 App PID=31279 (com.ea.gp.fcmnova) 启动，全程存活 |
| `20:20:19.936` | Capture 5 完成，开始等待 BufferID=2（timeout=180s） |
| `20:21:02–20:21:04` | **系统进程崩溃**：PID=10579 (com.android.quicksearchbox), PID=8878 (com.miui.home) |
| `20:21:08–20:22:47` | 设备大规模进程重启风暴（crash recovery，含 zygote64） |
| `20:21:17.452` | SHADER_DATA 到达（已在传输中，早于系统崩溃介入） |
| `20:23:19.943` | **180 秒超时**，REPLAY_METRICS_DATA 永远未到 |
| `20:23:19.945` | `ImportCapture` 返回 `false`，`dsbBuffer = null` |
| `20:23:19.945` | `ExportDrawCallData` 被跳过 |
| `20:23:19.945` | 开始等待 trailing event（bufferID=1，timeout=60s） |
| `20:24:19.957` | **60 秒超时**，proceeding anyway |
| `20:24:19.957` | "Capture Complete" 打印，`snapshot_5/` 为空 |

---

## 关联异常：Snapshot 4 文件传输极慢

Snapshot 4 的文件传输耗时 **25767ms**（正常约 700ms，慢 37 倍），
发生在 capture 5 触发之前。这可能是设备系统即将发生崩溃前的 I/O 瓶颈或 GPU 压力迹象，
属于先兆信号。

---

## 根本原因

**设备系统级崩溃**（Launcher 进程和 QuickSearch 进程崩溃，触发 zygote64 重启和大规模进程恢复）
发生在 capture 5 的 GPU replay 窗口期（20:21:02–20:21:04），
中断了设备侧 GPU profiler 的数据通路，导致 `BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA`
永远不会生成。目标 App `com.ea.gp.fcmnova`（PID=31279）本身全程存活，并非崩溃。

这与两次历史失败的根因均不同：
- 2026-04-07：race condition（ImportCapture(4) 的第二次 replay 占据了 snap5 的 metrics 采集窗口）
- 2026-04-10：设备系统级崩溃中断 GPU profiler 数据通路（目标 App 未崩溃）

**诊断盲区**：现有日志无法区分"目标 App 崩溃"和"系统进程崩溃导致 profiler 中断"
两种场景——两者在 SDPCLI 侧都表现为相同的 180s 超时。

---

## 当前代码行为的评估

| 行为 | 评估 |
|---|---|
| 180s 后超时继续 | ✓ 合理（不能无限等待） |
| 超时后调用 `ImportCapture` | ✓ 合理（尝试 best-effort） |
| `ImportCapture` 返回 false 时跳过 ExportDrawCallData | ✓ 合理 |
| 60s 后 trailing event 未到时 proceeding | ✓ 合理 |
| `snapshot_5/` 目录存在但完全为空 | ⚠ 用户无法区分"空快照"和"成功快照" |
| 日志 "Capture Complete" 在 snapshot 为空时仍然打印 | ⚠ 误导性——应区分 empty/complete |
| `sdpframe_005.gfxrz` 不存在 | ⚠ frame 文件未传完——系统崩溃可能中断了传输 |
| 系统进程崩溃时无任何告警 | ⚠ 无法区分"目标 App 崩溃"和"系统进程崩溃"两种失败模式 |

---

## 建议（供实现阶段参考）

1. **区分 Capture 结果类型**：在最终 "Capture Complete" 日志和目录中标识 `status=empty/partial/complete`
2. **空快照目录处理**：写入 `capture_result.json`（说明 status、reason、时间），避免用户混淆
3. **目标 App 退出检测**：当目标 App PID 消失时发出 WARNING（必须过滤 `_targetPackageName`，
   不能用"有 GPU metrics 的进程退出"作为判据——系统进程有大量 GPU metrics 会触发误报）
4. **文件传输异常慢的告警**：25767ms 传输时间应触发 WARNING，作为设备负载异常的早期告警
5. **`OnProcessStateChanged` 日志过滤**：对非目标进程降到 DEBUG，避免系统进程重启噪声淹没信号

---

## 结论

本次 snapshot_5 为空原因是外部因素（设备系统崩溃中断 GPU profiler 数据通路），
不是 SDPCLI 代码的 bug。目标 App 本身未崩溃。代码行为符合预期，
但诊断可见性不足：日志无法区分"目标 App 崩溃"和"系统进程崩溃"两种场景，
都表现为静默等待 180s 超时。
