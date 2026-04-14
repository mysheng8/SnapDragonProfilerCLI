---
type: plan
topic: Console.WriteLine → AppLogger 迁移
status: proposed
based_on:
  - FINDING-2026-04-13-console-writeline-audit.md
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
summary: 将全部非 UI 的 Console.WriteLine 迁移到 AppLogger，保持 UI 交互打印不变。
last_updated: 2026-04-13
---

# Plan: Console.WriteLine → AppLogger 迁移

## 目标

将非用户交互的 `Console.WriteLine` 全部迁移到 `AppLogger`，让日志写入文件并可按级别过滤，同时保留 UI 交互打印不变。

## 执行顺序（按影响面排序）

### Step 1 — `SDPClient.cs` （最大，~95 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"SDPClient"`**

转换规则：
- `=== ... ===` 分隔 banner → `AppLogger.Info("SDPClient", "=== ... ===")`
- `✓ ...` 成功 → `AppLogger.Info` 或 `AppLogger.Success`
- `✗ ERROR ...` → `AppLogger.Error`
- `⚠ WARNING ...` / `⚠ Warning ...` → `AppLogger.Warn`
- `[DEBUG] ...` 前缀 → `AppLogger.Debug`
- 常规进度状态 → `AppLogger.Info`
- 高频轮询状态（连接等待循环）→ `AppLogger.Debug`
- 多行 Debug Suggestions（StartApp 异常后的 adb 建议）→ `AppLogger.Debug`

**保留为 Console.WriteLine（UI 交互）：**
- lines 385-394：设备选择菜单（`"\nAvailable devices:\n"`、编号列表、`"\nSelect device [0]: "`）

---

### Step 2 — `SnapshotCaptureMode.cs` （~50 处，import 已有）

**context = `"Snapshot"`**

转换规则：
- 模式 banner → `AppLogger.Info`
- 路径/配置信息 → `AppLogger.Info`
- 失败/退出 → `AppLogger.Error`
- 警告 → `AppLogger.Warn`
- `[DEBUG]` 显式标注 → `AppLogger.Debug`
- 例外情况 stack trace → `AppLogger.Exception` 或 `AppLogger.Error` + file-only

**保留为 Console.WriteLine：**
- line 124：`"Enter app package name: "` (Write, not WriteLine)
- line 171：`"Press ENTER to capture a frame, or ESC to exit"`
- lines 179–183：ESC/ENTER 键盘响应提示
- line 298：`"Press ENTER to capture another frame, or ESC to exit"`
- line 305：`"Press ENTER to retry or ESC to exit"`

---

### Step 3 — `CliClientDelegate.cs` （5 处，import 已有）

**context = `"Delegate"`**（已用于其他方法）

仅转换构造函数调试打印（lines 61–71）：
```csharp
// 转换前
Console.WriteLine("[CliClientDelegate] Constructor called");
// ...
// 转换后
AppLogger.Debug("Delegate", "Constructor called");
AppLogger.Debug("Delegate", $"Type: {this.GetType().Name}");
// ...
```

---

### Step 4 — `CliDeviceDelegate.cs` （3 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"DeviceDelegate"`**

```csharp
// line 12
AppLogger.Info("DeviceDelegate", $"Device connected: {name}");
// line 17
AppLogger.Info("DeviceDelegate", $"Device disconnected: {name}");
// line 22
AppLogger.Info("DeviceDelegate", $"Device state changed: {name}");
```

---

### Step 5 — `Application.cs` （~5 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"App"`**

| 原文 | 级别 |
|------|------|
| `✓ Loaded configuration from: ...` | `AppLogger.Info` |
| `ℹ No config.ini found at: ...` | `AppLogger.Info` |
| `Will use interactive mode` | `AppLogger.Info` |
| `=== ... Analysis Mode ===` banner | `AppLogger.Info` |
| `=== ... Snapshot Mode ===` banner | `AppLogger.Info` |
| `=== ... Server Mode ===` banner | `AppLogger.Info` |
| `Error: Unknown subcommand ...` | `AppLogger.Error` |

**保留：**
- `SafeReadLine` 内 lines 47, 52, 63（输入重定向反馈，面向用户）

---

### Step 6 — `Config.cs` （1 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

```csharp
AppLogger.Warn("Config", $"Failed to load config from {path}: {ex.Message}");
```

---

### Step 7 — `Tools/TextureExtractor.cs` （~20 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"Texture"`**

| 特征 | 级别 |
|------|------|
| `=== Extracting Texture ... ===` | `Info` |
| `Size: ...` / `Format: ...` 元数据 | `Debug` |
| `⚠ Skipping ...` / `⚠ No data ...` | `Warn` |
| `✓ Converted` / `✓ Saved to:` | `Info` |
| `❌ Error:` + stack | `AppLogger.Exception` |
| ASTC / TFormat 内部细节 | `Debug` |

---

### Step 8 — `Tools/ShaderExtractor.cs` （~15 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"Shader"`**

| 特征 | 级别 |
|------|------|
| `=== Extracting Shaders ... ===` | `Info` |
| `Pipeline ID:` / `Found N shader stages` | `Info` |
| `[STAGENAME] ModuleID=...` | `Debug` |
| `✗ No SPIR-V data ...` / `✗ No shaders found` | `Warn` |
| `✓ SPIR-V:` / `✓ GLSL/Disasm:` | `Info` |
| `✗ spirv-cross failed` | `Warn` |
| `=== Pipelines in capture ===` 表格 | `Info` |

---

### Step 9 — `Services/Analysis/MetricsQueryService.cs` （2 处）

**需要添加：**
```csharp
using SnapdragonProfilerCLI.Logging;
```

**context = `"MetricsQuery"`**

```csharp
// line 120
AppLogger.Debug("MetricsQuery", $"{result.Count} DCs matched ...");
// line 125
AppLogger.Warn("MetricsQuery", $"LoadMetrics failed: {ex.Message}");
```

---

## 验证

每步修改后运行：
```powershell
cd d:\snapdragon\SDPCLI; dotnet build SDPCLI.sln -c Debug --no-restore 2>&1 | Select-String " error |Build succeeded|FAILED" | Select-Object -Last 5
```

最终验证：运行 `sdpcli.bat analysis <sdp>` 确认日志正常写入文件，且控制台输出行为一致。

## 实现说明

- 不修改 Logging 基础设施
- 不改变任何 UI 交互行为（菜单、prompt、键盘输入）
- 不更改消息内容，只换输出方式
