---
type: finding
topic: Console.WriteLine vs AppLogger 审计
status: investigated
related_paths:
  - SDPCLI/source/SDPClient.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/CliClientDelegate.cs
  - SDPCLI/source/CliDeviceDelegate.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Config.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
related_tags: [logging, cleanup, applogger]
summary: 审计了全部源文件中的 Console.WriteLine 调用，分类为需要转 AppLogger 的日志与需要保留的 UI 交互打印。
last_updated: 2026-04-13
---

# Finding: Console.WriteLine vs AppLogger 审计

## 背景

项目存在统一日志系统 `AppLogger`（`SDPCLI/source/Logging/AppLogger.cs`），支持：
- 双通道：文件（全量）+ 控制台（可按 MinConsoleLevel 过滤）
- 级别：Debug / Info / Success / Warn / Error
- `ConsoleLogSink` 专门处理 SDPCore C++ 内部日志，与应用层日志分离

`AppLogger` 注释明确说明：用户交互（菜单、prompt、"Press ENTER"）可以继续使用 `Console.Write/WriteLine`，其他均应走 AppLogger。

## 现状

大量业务逻辑仍直接调用 `Console.WriteLine`，导致：
- 日志无法写入文件，排查问题时丢失信息
- 无法按级别过滤（DEBUG 和 ERROR 混在一起显示）
- 代码风格不一致（`CliClientDelegate.cs` 已部分迁移，`SDPClient.cs` 完全未迁移）

---

## 需要转换的文件（按工作量排序）

### 1. `SDPClient.cs` (~95 处)
- **无** `using SnapdragonProfilerCLI.Logging;` — 需要添加
- context 建议：`"SDPClient"`
- 全部状态/错误/调试信息，无 UI 交互内容（除设备选择菜单 4 行，见下文保留列表）

### 2. `SnapshotCaptureMode.cs` (~50 处)
- **已有** `using SnapdragonProfilerCLI.Logging;` — 无需新增 import
- context 建议：`"Snapshot"`
- 含大量状态、进度、错误输出；"Press ENTER…" 提示需保留（见保留列表）

### 3. `Tools/TextureExtractor.cs` (~20 处)
- **无** Logging import
- context 建议：`"Texture"`

### 4. `Tools/ShaderExtractor.cs` (~15 处)
- **无** Logging import
- context 建议：`"Shader"`

### 5. `CliClientDelegate.cs` (5 处，构造函数调试打印)
- **已有** Logging import — 其他回调方法已迁移，仅构造函数 lines 61–71 残留
- context 建议：`"Delegate"`（已用）

### 6. `CliDeviceDelegate.cs` (3 处)
- **无** Logging import
- context 建议：`"DeviceDelegate"`

### 7. `Application.cs` (~5 处)
- **无** Logging import
- config 加载状态（lines 29, 33–34）、模式 banner（lines 90, 95, 102）、未知 subcommand 错误（line 114）
- `SafeReadLine` 内的重定向提示（lines 47, 52, 63）属于 UI 反馈，保留

### 8. `Config.cs` (1 处)
- **无** Logging import
- line 46：`Warning: Failed to load config` → `AppLogger.Warn`

### 9. `Services/Analysis/MetricsQueryService.cs` (2 处)
- **无** Logging import
- lines 120, 125：查询结果 debug → `AppLogger.Debug`

---

## 需要保留的 Console.WriteLine（UI 交互）

以下打印为用户面向的交互内容，应保持 `Console.Write/WriteLine`：

| 文件 | 内容 |
|------|------|
| `InteractiveMode.cs` 全部 | 菜单展示、"Enter mode number (1/2/3): " |
| `AnalysisMode.cs` 全部菜单行 | 文件列表、snapshot 选择、键盘输入处理 |
| `SnapshotCaptureMode.cs` line 124 | `"Enter app package name: "` |
| `SnapshotCaptureMode.cs` lines 171, 298, 305 | `"Press ENTER to capture…"` / `"Press ENTER to retry or ESC to exit"` |
| `SnapshotCaptureMode.cs` lines 179–183 | ESC/ENTER 响应提示 |
| `SDPClient.cs` lines 385–394 | `"\nAvailable devices:\n"` 设备选择菜单 |
| `Application.cs` lines 47, 52, 63 | 输入重定向时的提示（SafeReadLine 内部） |

---

## 级别映射规则

| 特征 | AppLogger 级别 |
|------|------|
| `✓` / `initialized` / `started` / `succeeded` | `Info` |
| `✓` + 表示成功完成 | `Success` |
| `⚠` / `Warning` / `WARNING:` | `Warn` |
| `✗ ERROR` / `failed` / `threw` / `ERROR:` | `Error` |
| `[DEBUG]` 前缀 / 构造函数内部打印 | `Debug` |
| 常规状态/进度信息 | `Info` |
| 详细的 DB 轮询进度（高频） | `Debug` |
