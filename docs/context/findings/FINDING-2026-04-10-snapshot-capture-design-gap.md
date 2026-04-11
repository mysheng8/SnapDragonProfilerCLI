---
type: finding
topic: snapshot 模式架构约束与最终设计方向（ENTER/ESC 最小交互循环）
status: investigated
related_paths:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI.bat
related_tags:
  - snapshot
  - cli
  - capture-loop
  - sdk-constraint
  - design-decision
summary: >
  SDK 架构约束：每次 SDPClient.Initialize() 创建独立 session（CreateTimestampedSubDirectory=true），
  跨进程 session 共享不可能。由此推导：(1) -launch + -capture 二步非交互流程无意义（-launch 退出后
  连接丢失，-capture 需完全重建）；(2) -archive 多帧聚合不可实现。最终设计：snapshot 子命令进入
  ENTER/ESC 最小交互循环（同一进程内多次截帧，ESC 触发 archive 生成 .sdp），移除 -launch/-capture flag。
last_updated: 2026-04-10
---

## 问题 1：旧菜单出现（可能为 binary 未重建）

### 现象

运行 `sdpcli snapshot -capture` 时出现老式交互菜单：

```
=== Snapdragon Profiler CLI ===
Select mode:
  1. Snapshot Capture  - Capture single frame
  2. Analysis          - Analyze existing .sdp file
  ...
```

### 根本原因

代码路由逻辑本身正确：
- `snapshot` 解析为 positional subcommand → `subcommand = "snapshot"`
- `-capture` 被识别为 `doCapture = true`
- Application.cs routing 进入 `SnapshotCaptureMode`（而非 InteractiveMode）

旧菜单来自 `InteractiveMode.Run()`，仅在 `subcommand == null` 路径触发。

**最可能原因**：binary 未使用最新代码重建。
`sdpcli_capture.bat` 和 `SDPCLI.bat` 均指向 `SDPCLI\bin\Debug\net472\SDPCLI.exe`，
若未执行 `dotnet build`，仍然运行含旧 Application.Run() 的旧版 exe。

### 验证方法

```batch
dotnet build SDPCLI.sln -c Debug
SDPCLI.bat snapshot -capture
```

若菜单消失 → 确认为 stale binary 问题。  
若菜单仍在 → 说明存在代码路径分叉，需进一步调查。

---

## 问题 2：SDK 架构约束 — 每个进程 = 独立 session（根本性约束）

### SDK 核心约束（已验证）

`SDPClient.Initialize()` 总是使用 `CreateTimestampedSubDirectory = true`：
- 每次 `Initialize()` 在 SdpOutputDir 下创建新的带时间戳子目录（如 `2026-04-10T12-00-00/`）
- 该子目录含独立的 `sdp.db`
- `SDPClient.Shutdown()` 关闭 `sdp.db` 并断开连接
- **SDK 无 session re-open API** — 一旦 Shutdown，该 session 无法从另一进程复用

代码证据：
- `dll/project/SDPClientFramework/Sdp/ConnectionManager.cs` line 61：`CreateTimestampedSubDirectory = true`
- `dll/project/SDPCoreWrapper/SessionSettings.cs`：`CreateTimestampedSubDirectory` 为 native P/Invoke 属性

### `-launch` + `-capture` 二步非交互工作流的架构缺陷

当前 `-launch` 非交互路径（`SnapshotCaptureMode.cs`）：

```csharp
if (nonInteractiveMode == "launch")
{
    Console.WriteLine("=== Launch complete. Exiting (use -capture to trigger frame capture). ===");
    Cleanup();   // → SDPClient.Shutdown() → 连接断开 → session dir 废弃
    return;
}
```

实际执行序列：

1. `sdpcli snapshot -launch pkg\activity`
   → `SDPClient.Initialize()` → 创建 session dir A → 连接 → 启动 app
   → `Cleanup()` → `SDPClient.Shutdown()` → **进程退出，session dir A 废弃**

2. `sdpcli snapshot -capture pkg\activity`
   → `SDPClient.Initialize()` → 创建**新的** session dir B → 重新连接
   → `AppLaunchService.SelectAndLaunch()` → **重新启动 app**
   → 截帧 → `CreateSessionArchive` → 退出

`-launch` 步骤对 `-capture` 没有任何贡献：
- 连接在 `-launch` 退出时已断开，`-capture` 必须完全重建
- App 在 `-capture` 里无论如何都会被重新启动
- 两个步骤使用完全独立的 session dir

**结论**：`-launch` + `-capture` 作为非交互二步工作流在架构上无意义。
`-launch` 仅在**交互模式**（同一进程持续运行）中有价值。

### `-archive` 多帧工作流的架构缺陷（同一根因）

每次 `-capture` 是独立进程 → 每次 `Initialize()` = 新 session dir = 新 `sdp.db`
→ 无法跨进程追加 snapshot 到同一 `sdp.db`。

`-archive` 概念所需的"共享 staging dir"与 SDK 的 per-process session 模型根本矛盾：
即使把 snapshot 数据拷贝到 staging dir，`sdp.db` 本身无法被两个不同的 session 引用。

### 正确结论

| 场景 | 结论 |
|------|------|
| 每次 `-capture` 生成独立 `.sdp` | **正确行为**（SDK 约束使然，`finally { CreateSessionArchive }` 保留） |
| `-launch` + `-capture` 二步非交互流程 | **无意义**（`-launch` 退出后连接即丢失） |
| `-archive` 多帧聚合 | **不可实现**（`sdp.db` 无法跨进程共享） |
| Interactive mode：launch 后人工触发 capture | **可行**（同一进程，session 持续存在） |

唯一有效的非交互截帧命令：`sdpcli snapshot -capture pkg\activity`（全程单进程）。

---

## 设计结论：snapshot 模式最终方案

基于 SDK 约束，正确的 snapshot 非交互化方向是：

**保留单进程 + 最小交互循环**（ENTER=截帧 / ESC=退出+archive）:

```
sdpcli snapshot [<package\activity>] [-output/-o <dir>]
```

执行流程：
1. 建连 → 安装 APK → 连接设备 → 启动 app（自动，参数来自 positional arg 或 config.ini）
2. 进入最小交互循环：
   - **ENTER** → 截帧，captureEntry 记录，继续循环（可多次捕帧）
   - **ESC** → 退出循环 → `CreateSessionArchive` → 生成 `.sdp` → 退出

同一进程内可截多帧，全部打包进同一 `.sdp`。这与 SDK 的 per-process session 模型完全兼容。

**结果**：`-launch` 和 `-capture` flag 移除。`snapshot` 子命令本身就是完整的截帧入口。

---

## 事实列表

1. `sdpcli_capture.bat` = `.\SDPCLI.exe snapshot -capture %*`，本身正确
2. `-capture` flag 在 Main.cs 中被 `lo == "-capture"` 匹配，设置 `doCapture = true`
3. Application.cs routing 在 `subcommand == "snapshot"` 且 `doCapture = true` 时进入 `SnapshotCaptureMode` ✓
4. `SnapshotCaptureMode` 的 `IsNonInteractive` 在 `doCapture=true` 时为 `true` ✓
5. 当 `IsNonInteractive` 为 `true`，capture loop 在第一次截帧后 `break` ✓
6. `finally` 块总是调用 `CreateSessionArchive` — 每次 `-capture` 生成独立 `.sdp` ✓（SDK 约束下的正确行为）
7. `-archive` flag 无需实现 — `sdp.db` 无法跨进程共享，staging dir 方案不可行
8. `-launch` 非交互模式无意义 — `Cleanup()` 后连接断开，后续 `-capture` 需完全重建连接并重新启动 app
9. `-launch` 和 `-capture` flag 移除 — snapshot 子命令直接进入 ENTER/ESC 最小交互循环
10. 正确截帧命令：`sdpcli snapshot [pkg\activity]`（同一进程内多次 ENTER 截帧，ESC 退出并生成 .sdp）
