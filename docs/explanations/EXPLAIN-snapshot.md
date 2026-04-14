---
type: explanation
topic: Snapshot 模式完整实现
module_key: SDPCLI.Snapshot
source_scope:
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Services/Capture/
module_index: docs/index/modules/SDPCLI.Snapshot.md
based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - FINDING-2026-04-07-shader-texture-export-structure.md
status: stable
audience:
  - self
  - onboarding
last_updated: 2026-04-14
---

# SDPCLI Snapshot 模式：工作原理与实现详解

## What this document explains

本文档详细解析 Snapshot 模式的完整工作流程：从 SDK 初始化、设备连接、帧抓取到数据导出的全调用链，以及 SDK 回调桥接、多帧共存、DescriptorSet 二进制处理等关键机制。

## Scope

- **Included**：完整调用链（8 阶段）、SDK VulkanSnapshot* 表写入时序、SDPCLI DrawCall* 表的三步幂等写入、输出目录结构、DescriptorSet 数据特殊处理、多帧共存设计、旧版 SDP 兼容、关键时序约束
- **Excluded**：Analysis Pipeline 细节（见 EXPLAIN-analysis.md）、Server 模式的 CaptureJobRunner（见 EXPLAIN-server.md）、SDK 架构与 DLL 依赖（见 EXPLAIN-three-modes.md §二）

## Routing

- **ModuleKey**: SDPCLI.Snapshot
- **Module Index**: [docs/index/modules/SDPCLI.Snapshot.md](../index/modules/SDPCLI.Snapshot.md)
- **SourceScope**: `SDPCLI/source/Modes/SnapshotCaptureMode.cs`, `SDPCLI/source/Services/Capture/`

## Context Basis

- **Implementations**: IMPL-2026-04-14-http-server-mode.md
- **Plans**: PLAN-2026-04-11-http-server-mode.md
- **Findings**: FINDING-2026-04-07-shader-texture-export-structure.md

## Reality Status

- **Stable**: 完整 Snapshot 流程已多次实测验证，多帧共存和 CSV 导入路径可靠
- **WIP**: 无
- **Outdated or conflicting**: `EXPLAIN-architecture.md` 引用（§五中提及）实际应指向 EXPLAIN-three-modes.md §四

---

## 一、设计目标

Snapshot 模式的目标是**完全替代 Snapdragon Profiler GUI 的手动抓帧流程**。GUI 需要：手动点击连接设备、启动 App、点击按钮触发帧、等待 replay 完成、手动导出。Snapshot 模式将这一流程压缩为一条命令，并支持在同一个 session 内连续按 ENTER 抓多帧。

关键约束：
- **事件驱动异步**：SDK 的回调（`OnCaptureComplete`、`OnDataProcessed`）在 native 线程触发，必须用 `ManualResetEvent` 桥接到主线程的等待逻辑
- **多帧数据共存**：同一个 session 写入同一个 `sdp.db`，以 `CaptureID` 列区分，绝不 DROP 旧数据
- **必须依赖 SDPClientFramework**：QGLPlugin 需要 `SdpApp.EventsManager`，无法绕过宿主框架

---

## 二、完整调用链

```
SDPCLI.exe snapshot <pkg\Activity>
  └── Application.Run("snapshot", ...)
      └── SnapshotCaptureMode.Run()
          │
          ├── [Phase 1] InitializeClient()
          │   ├── SdpApp.Init(ConsolePlatform)          ← 进程全局，只调用一次
          │   ├── new QGLPlugin()                        ← 订阅 ConnectionEvents.DataProcessed
          │   └── SDPClient.Initialize(settings, CliClientDelegate)
          │       └── Client.Init(SessionSettings)
          │           └── SessionDirectoryRootPath → <SdpDir>/<timestamp>/
          │
          ├── [Phase 2] 设备准备
          │   ├── DeviceConnectionService.CheckAndInstallAPKs()  ← adb install
          │   ├── DeviceConnectionService.Connect()
          │   │   └── SDPClient.ConnectDevice() → Device
          │   └── AppLaunchService.SelectAndLaunch(packageName)
          │       └── 发送 launch command → 等待 OnProcessAdded
          │
          └── [Phase 3] ENTER 循环（每次 ENTER 触发一帧）
              │
              ├── CaptureExecutionService.StartCapture()
              │   └── captureManager.CreateCapture(CaptureType.Snapshot=4)
              │       → Capture.Start()
              │
              ├── WaitOne(_captureCompleteEvent, 30s)
              │   └── CliClientDelegate.OnCaptureComplete() → .Set()
              │
              ├── WaitForDataProcessed()
              │   └── 轮询 _dataProcessedCount，稳定 2s（最多 90s）
              │       CliClientDelegate.OnDataProcessed() → count++
              │
              ├── scan snapshot_* dirs → 确认真实 captureId
              │   └── 扫描 SessionDir 下新增的 snapshot_{N}/ 目录名
              │
              ├── ReplayAndGetBuffers(captureId)
              │   ├── QGLPluginService.ImportCapture(srcId=captureId, dstId=captureId, dbPath)
              │   │   └── SDK 写入 VulkanSnapshot* 表（见下文 §3.1）
              │   ├── 轮询 VulkanSnapshotGraphicsPipelines 行数 → 稳定判断 replay 完成（最多 90s）
              │   ├── 等待 ImportCompleteEvent.WaitOne(60s)
              │   └── 获取 DsbBuffer（SnapshotDsbBuffer 二进制 → DescriptorSet 绑定数据）
              │
              ├── ExportDrawCallData(captureId)
              │   ├── VulkanSnapshotModel.LoadSnapshot(apiBuffer, dsbBuffer)
              │   │   └── 解析 ApiBuffer (vkCmd* 序列) + DsbBuffer (DescSet 绑定)
              │   ├── model.ExportDrawCallParametersToCsv()   → DrawCallParameters.csv
              │   ├── model.ExportDrawCallBindingsToCsv()     → DrawCallBindings.csv
              │   ├── model.ExportDrawCallRenderTargetsToCsv()→ DrawCallRenderTargets.csv
              │   ├── model.ExportDrawCallVertexBuffersToCsv()→ DrawCallVertexBuffers.csv
              │   ├── model.ExportDrawCallIndexBuffersToCsv() → DrawCallIndexBuffers.csv
              │   ├── model.ExportDrawCallMetricsToCsv()      → DrawCallMetrics.csv
              │   ├── model.ExportPipelineVertexInputsToCsv() → PipelineVertexInputs.csv
              │   └── CsvToDbService.ImportAllCsvs(captureSubDir, sdp.db)
              │       ← 每张 CSV: CREATE IF NOT EXISTS + DELETE WHERE CaptureID=x + INSERT
              │
              ├── DataExportService.ExportData()
              │   └── 截图保存 → snapshot_{captureId}/screenshot.png
              │
              └── [可选] 再按 ENTER → 下一帧（captureId+1）

[ESC] SessionArchiveService.CreateSessionArchive()
      └── ZIP(SessionDir) → <SdpDir>/<timestamp>.sdp
```

---

## 三、SDK 数据流

### 3.1 SDK 写入的表（VulkanSnapshot* 系列）

SQL 写入由 `QGLPluginService.ImportCapture()` 触发，SDK 内部完成，数据按 `captureID` 积累：

| 表 | 内容 | 写入时机 |
|----|------|---------|
| `VulkanSnapshotApis` | vkCmd* 调用序列（每个 DrawCall 一行） | Replay 开始 |
| `VulkanSnapshotGraphicsPipelines` | 管线状态对象（pipelineID → shaderStages + 其他） | Replay 进行中 |
| `VulkanSnapshotShaderStages` | SPIR-V blob（PipelineID + stageType + 二进制） | Replay 完成 |
| `VulkanSnapshotByteBuffers` | 纹理/缓冲区二进制数据（resourceID + dataPair） | Replay 完成 |
| `VulkanSnapshotTextures` | 纹理元数据（width/height/format/resourceID） | Replay 完成 |
| `VulkanSnapshotImageViews` | ImageView → Texture 的映射（imageViewID → resourceID） | Replay 完成 |

**行数稳定检测**：`VulkanSnapshotGraphicsPipelines` 行数从 0 开始增长，直到不再增长→ 判断 replay 完成。`ImportCompleteEvent` 作为最终确认信号。

### 3.2 SDPCLI 写入的表（DrawCall* 系列）

`CsvToDbService.ImportAllCsvs()` 从 CSV 文件导入，采用**三步幂等写入**（不会覆盖其他 captureId 的行）：

```sql
-- CsvToDbService 对每张表执行:
CREATE TABLE IF NOT EXISTS DrawCallParameters (...);   -- 首次创建/复用历史表
DELETE FROM DrawCallParameters WHERE CaptureID = ?;    -- 删除本 capture 的旧行（幂等）
INSERT INTO DrawCallParameters ...;                     -- 写入新行
```

| 表 | 来源 CSV | 主键约束 | 说明 |
|----|---------|---------|------|
| `DrawCallParameters` | DrawCallParameters.csv | CaptureID + DrawCallApiID | DrawCall 基本信息 |
| `DrawCallBindings` | DrawCallBindings.csv | CaptureID + DrawCallApiID | PipelineID + ImageViewID 绑定 |
| `DrawCallRenderTargets` | DrawCallRenderTargets.csv | CaptureID（动态检测列） | RT 附件信息 |
| `DrawCallVertexBuffers` | DrawCallVertexBuffers.csv | CaptureID + DrawCallApiID | VB 绑定 |
| `DrawCallIndexBuffers` | DrawCallIndexBuffers.csv | CaptureID + DrawCallApiID | IB 绑定 |
| `DrawCallMetrics` | DrawCallMetrics.csv | CaptureID + DrawcallIdx | GPU 计数器 |
| `PipelineVertexInputs` | PipelineVertexInputs.csv | CaptureID + PipelineID | 顶点输入格式 |

---

## 四、输出目录结构

```
<SdpDir>/<timestamp>/           ← SessionDir（最终压缩为 .sdp）
├── sdp.db                       ← SQLite database（包含上述所有表）
├── snapshot_2/                  ← 第 1 次 ENTER 的 captureId=2
│   ├── DrawCallParameters.csv   ← 7 张 CSV（by captureId）
│   ├── DrawCallBindings.csv
│   ├── DrawCallRenderTargets.csv
│   ├── DrawCallVertexBuffers.csv
│   ├── DrawCallIndexBuffers.csv
│   ├── DrawCallMetrics.csv
│   ├── PipelineVertexInputs.csv
│   └── screenshot.png
├── snapshot_3/                  ← 第 2 次 ENTER 的 captureId=3
│   ├── DrawCallParameters.csv
│   ...
└── session_summary.json         ← 会话级元数据（SessionSummaryService）
```

最终 `SessionArchiveService.CreateSessionArchive()` 将整个目录 ZIP 为 `<timestamp>.sdp`。

---

## 五、DescriptorSet 数据的特殊处理

`SnapshotDsbBuffer` 是 QGLPlugin 在 replay 完成后填充到内存的二进制块，**不会写入 .sdp**。SDPCLI 必须在 Capture 阶段自行解析并保存。

- 数据结构：`DescSetBindings.DescBindings`（14 字段，参见 [EXPLAIN-three-modes.md §四](EXPLAIN-three-modes.md#四数据结构参考)）
- `imageViewID` 字段 → 关联 `VulkanSnapshotImageViews.resourceID` → 找到对应纹理
- 保存目标：`DrawCallDescriptorBindings` 和 `DrawCallPipelines` 自定义表

---

## 六、多帧共存设计

同一个 session 可以按多次 ENTER 连续抓帧，每帧 `captureId` 递增（从 2 开始）：

```sql
-- 多帧共存后的 DB 状态
SELECT CaptureID, COUNT(*) FROM DrawCallParameters GROUP BY CaptureID;
-- 2 | 174
-- 3 | 182
-- 4 | 176
```

Analysis 模式在打开 .sdp 时会扫描 ZIP 内的 `snapshot_N/` 目录，列出所有可用的 captureId 供用户选择，然后所有查询加 `WHERE CaptureID=x` 精确过滤。

---

## 七、旧版 SDP 兼容

`CsvToDbService.CreateOrExtendTable()` 检测表是否已存在但缺少 `CaptureID` 列（旧格式），执行 `ALTER TABLE ADD COLUMN CaptureID INTEGER` 补齐，然后按新逻辑写入。

---

## 八、关键时序约束

```
StartCapture()
  → Capture.Start()
     ↓ SDK 开始抓帧
[30s 超时] OnCaptureComplete → _captureCompleteEvent.Set()
  → WaitForDataProcessed()（等待 OnDataProcessed 计数稳定 2s，上限 90s）
     ↓ snapshot_* 目录出现
  → scan 确认 captureId
  → ImportCapture()（触发 replay）
     ↓ SDK 回放帧，填充 VulkanSnapshot* 表
  → [90s 超时] 轮询 GraphicsPipelines 行数稳定
  → ImportCompleteEvent.Set()（最终信号）
     ↓
  → 读取 DsbBuffer（此时有效）
  → ExportDrawCallData()
  → 截图
```

在 `ImportCompleteEvent` 之前读取 DsbBuffer 会得到 null 或空数据。
