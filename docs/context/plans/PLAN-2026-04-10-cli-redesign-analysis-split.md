---
type: plan
topic: CLI 重构 — 子命令化 + raw.json 6拆 + analysis -target 级联增量控制
status: updated
based_on:
  - FINDING-2026-04-07-shader-texture-export-structure.md
  - FINDING-2026-04-08-parallelism-thread-safety.md
  - IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
  - IMPL-2026-04-09-metrics-dynamic-attribution.md
related_paths:
  - SDPCLI/source/Main.cs
  - SDPCLI/source/Application.cs
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Modes/SnapshotCaptureMode.cs
  - SDPCLI/source/Modes/InteractiveMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/RawJsonGenerationService.cs
  - SDPCLI/source/Services/Analysis/AttributionReportService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Analysis/TopDcJsonService.cs
related_tags:
  - cli
  - subcommand
  - json-split
  - analysis-target
  - cascading-deps
  - interactive-mode
summary: >
  三步重构（含补充细化）：① CLI 去掉 -mode flag，改为 positional subcommand
  （snapshot/analysis/无参=交互）；新增 InteractiveMode 类封装交互逻辑；
  -snapshot/-s 指定 snapshot_N（从 2 开始，默认 all）；
  ② raw.json 废弃，拆为 6 个 sub-JSON（dc/label/metrics/shaders/textures/buffers），
  join key = api_id；
  ③ analysis -target/-t 控制增量更新目标，采用前置级联依赖（选 status 自动先更新
  dc/label/metrics）；shaders/textures/buffers target 同步做 extraction，
  per-file 存在检查保持有效；AttributionReportService 在 all 模式下走内存路径无需修改，
  仅 -t analysis 单独触发时需从 JSON 加载。
last_updated: 2026-04-10
---

# PLAN-2026-04-10: CLI Redesign — Subcommand + 6-JSON Split + -target Cascading

---

## 0. 背景

### 0.1 现状

```
sdpcli -mode analysis -sdp foo.sdp
sdpcli -mode capture
sdpcli -mode extract-shader ...
sdpcli -mode extract-texture ...
sdpcli -mode mesh ...
sdpcli -mode dcanalysis ...
```

`raw.json` = 单体文件，含 label + params + bindings + RT + metrics + shaders + textures + buffers

Pass 控制：`-pass-mode stats|analysis` / `-stats-only` / `-analysis-only`

代码中的「Mode」概念（`SnapshotCaptureMode`、`AnalysisMode`、`IMode` 接口）保持不变。
CLI 入口只是「去掉了 `-mode` 这个关键词」，内部代码仍叫 mode。

### 0.2 目标

```
# Analysis — SDP 文件作为 positional arg（无 -sdp 关键词）
sdpcli analysis foo.sdp                     # 全量分析（所有 snapshot）
sdpcli analysis foo.sdp -s 3               # 只分析 snapshot_3
sdpcli analysis foo.sdp -s 3 -t label      # 只重跑打标
sdpcli analysis sdp/foo.sdp                 # 相对于 SdpDir 解析

# Snapshot — 必须指定 package\activity + 操作类型
sdpcli snapshot com.ea.fcmnova\\com.ea.frostbite.FrostbiteActivity           # ENTER=截帧 / ESC=退出+.sdp

# 交互（InteractiveMode — 维持现状）
sdpcli
```

---

## 1. Step 1 — CLI 顶层路由改为 Positional Subcommand

### 1.1 语法规则

```
sdpcli [subcommand] [flags...]

subcommand:
  snapshot      → SnapshotCaptureMode（原 capture/1）
  analysis      → AnalysisMode（原 analysis/2）
  (none)        → InteractiveMode（新增 Mode 类，封装原有交互选择逻辑）
```

**废弃的顶层模式**（`-mode` flag 关键词本身从 CLI 入口移除）：
- `extract-texture` (3) — 变成 `analysis -t textures`
- `extract-shader` (4)  — 变成 `analysis -t shaders`
- `dcanalysis` (5)      — 废弃（功能已合入 analysis pipeline）
- `extract-mesh` (6)    — 变成 `analysis -t buffers`

> **代码惯例**：`-mode` 关键词从 CLI 入口移除，但代码内部类名
> （`SnapshotCaptureMode`、`AnalysisMode`、`InteractiveMode`、`IMode`）
> 保持不变，不做重命名。

### 1.2 新增 InteractiveMode

将 `Application.Run()` 中的交互式菜单逻辑提取到 `InteractiveMode.cs`：

```csharp
public class InteractiveMode : IMode
{
    // 原 Application.Run() 中 modeArg==null 分支的所有交互逻辑
    // 只提供两个选项：snapshot / analysis
    // 选择后内部实例化对应 Mode 并调用 Run()
    public string Name => "Interactive";
    public string Description => "交互式选择模式";
    public void Run() { /* ... */ }
}
```

`Application.Run()` 在无 subcommand 时直接 `new InteractiveMode(...).Run()`，自身不再内嵌交互逻辑。

### 1.3 Main.cs 解析逻辑（改动说明）

```csharp
// 新逻辑：
// args[0] = subcommand（第一个非 -flag 参数）
// args[1] = 主位置参数（analysis: SDP 路径；snapshot: package\activity）
// 其余 = flags

string? subcommand = null;
string? positionalArg = null;   // analysis → sdpPath; snapshot → packageActivity
bool debugMode = false;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--debug")          // 只接受双横线
    { debugMode = true; continue; }

    if (!args[i].StartsWith("-"))
    {
        if (subcommand == null)         subcommand    = args[i].ToLower();
        else if (positionalArg == null) positionalArg = args[i];
        continue;
    }
    // 解析 -snapshot/-s / -target/-t / -output/-o 等
}
```

- `subcommand == "snapshot"` → SnapshotCaptureMode，`positionalArg` = `"package\\activity"`
- `subcommand == "analysis"` → AnalysisMode，`positionalArg` = SDP 文件路径
- `subcommand == null` → InteractiveMode（不接受 positionalArg）
- 其他值 → 错误提示

**`--debug` flag**（全局，双横线）：开启 `DEBUG` 级别日志。
只接受 `--debug`；单横线 `-debug` **不再支持**，避免与短 flag（`-d`）前缀混淆。

### 1.4 `-snapshot`/`-s` — 指定 Snapshot 范围（analysis 专用）

```
-snapshot/-s <N>    指定单个 snapshot（N 从 2 开始）
-snapshot/-s 1      分析所有 snapshot（等价于现有 "Analyze ALL"；哨兵值 1）
(不传)              默认 = all（等价于 -s 1）
```

> **为什么从 2 开始**：SDP 中 snapshot_1 不是常规 DrawCall snapshot——
> 它对应 Realtime mode 的默认采集（内部命名 capture_1），用于打通程序链路，
> 不含独立的帧数据。实际分析目标从 snapshot_2 开始，
> 这也是测试目录中可见 snapshot_2/3/4/5 的原因。
> 因此 `1` 作为 "all" 哨兵值不会与真实 snapshot id 冲突。

### 1.5 Snapshot 模式 — 最小交互循环（ENTER=截帧 / ESC=退出+SDP）

SDK 每次 `Initialize()` 创建独立 session（`CreateTimestampedSubDirectory = true`）。
因此 `-launch` + `-capture` 二步非交互流程在架构上无效。正确模型：单进程持续运行，在同一 session 内多次截帧。

```
sdpcli snapshot [<package\activity>]   # package\activity 可选，config.ini 可指定
```

执行流程：
1. 建连 → 安装 APK → 连接设备
2. 启动 app（package/activity 从 positional arg 或 config.ini 读取）
3. 进入最小交互循环：
   - **ENTER** → 截帧，captureEntry 记录，继续循环（可多次）
   - **ESC** → 退出循环 → `CreateSessionArchive` → 生成 `.sdp` → 退出

同一进程内可截多帧，全部打包进同一 `.sdp`。

`-launch` 和 `-capture` flag **移除**（SDK 约束使其无意义，见 §1.5.2 及 FINDING-2026-04-10-snapshot-capture-design-gap.md §问题2）。

### 1.5.1 Snapshot `-output/-o` — SDP 输出目录（可选）

```
sdpcli snapshot [<package\activity>] [-output/-o <dir>]
```

**默认值**：`SdpDir`（`ProjectDir/sdp`，与 analysis 模式读取 SDP 的目录相同）。
这样 capture 产物落在 SdpDir，analysis 就能直接按默认路径找到它们。

**`-o` 指定时的路径解析规则**：

1. **绝对路径** → 直接使用（目录不存在则自动创建）
2. **相对路径** → 依次尝试：
   a. `SdpDir/<arg>`（已存在则使用）
   b. `ProjectDir/<arg>`（已存在则使用）
   c. 两者都不存在 → 在 `SdpDir/<arg>` 下自动创建

与 analysis 的 `-o` 语义对比：
- snapshot `-o` → 控制 **SDP 文件的落盘目录**（session root）
- analysis `-o` → 控制 **产物输出目录**（outputDir，即解压 + JSON/MD 的根目录）
`package\activity` 参数格式：用 `\` 或 `/` 分隔包名和 Activity 名均可：
- `com.ea.fcmnova\com.ea.frostbite.FrostbiteActivity`
- `com.ea.fcmnova/com.ea.frostbite.FrostbiteActivity`

如果 `PackageName` / `ActivityName` 已在 config.ini 中配置，命令行 positionalArg 可覆盖它们；
不传 positionalArg 时从 config.ini 读取（维持现有行为，方便脚本化）。

### 1.5.2 Snapshot 多帧工作流 — ❌ CANCELLED（SDK 架构约束）

**原设计意图**（已废弃）：`-archive` flag 聚合多次 `-capture` 到同一 `.sdp`；`-launch` + `-capture` 作为二步非交互流程。

**为什么不可实现**：

SDK `CreateTimestampedSubDirectory = true` → 每次 `SDPClient.Initialize()` 创建新的带时间戳 session dir 和独立 `sdp.db`。`sdp.db` 无法被两个不同进程的 session 共享或追加。

同理 `-launch` + `-capture` 二步流程：`-launch` 非交互路径在 `Cleanup()` 后调用 `SDPClient.Shutdown()`，连接即丢失。后续 `-capture` 必须重建连接、重新启动 app，`-launch` 步骤毫无贡献。

**正确设计**：每次 `sdpcli snapshot -capture` 生成一个独立 `.sdp`（当前行为即正确）。
`-archive` flag 和 staging dir 机制**不实现**。

详见 FINDING-2026-04-10-snapshot-capture-design-gap.md §问题2。

### 1.6 InteractiveMode — 维持现状

`InteractiveMode` 不做任何功能精简，保留完整的现有交互体验：
- 列出 snapshot / analysis 两个选项
- 选 analysis 后继续提示 SDP 文件、snapshot 编号
- 选 snapshot 后继续提示 package、activity，进入 ENTER/ESC 截帧循环

> 目的：让老用户不受影响。非交互路径仅在明确传入 subcommand 时激活。

### 1.7 兼容性保留（过渡期）

- 旧 `-mode capture`/`-mode analysis` 别名保留（打印 deprecation warning），6 个月后删除
- `-stats-only`/`-analysis-only`/`-pass-mode` 保留作为 deprecated alias，内部转为等价 `-t` 集合（见 §3.5）

---

## 2. Step 2 — raw.json 废弃，拆为 6 个 Sub-JSON

### 2.1 文件清单（确定为 6 个）

| # | 文件名 | 内容 | 生成步骤 |
|---|--------|------|----------|
| 1 | `snapshot_{id}_dc.json` | api_id / api_name / pipeline_id / layout_id / render_pass_id / vertex_count / index_count / instance_count / render_targets / binding_summary | Step 1（DB 查询）|
| 2 | `snapshot_{id}_label.json` | category / subcategory / detail / reason_tags / confidence / label_source | Step 2（rules + LLM）|
| 3 | `snapshot_{id}_metrics.json` | 全量 Adreno counters（`m.All` 动态 dict）| Step 3（metrics join）|
| 4 | `snapshot_{id}_shaders.json` | pipeline_id / shader_stages（stage/module_id/entry_point）/ shader_files | Step 1.5a（extraction + JSON）|
| 5 | `snapshot_{id}_textures.json` | texture 元数据（id/width/height/format/layers/levels）/ texture_files | Step 1.5b（extraction + JSON）|
| 6 | `snapshot_{id}_buffers.json` | vertex_buffers（binding/buffer_id）/ index_buffer / mesh_file | Step 1.5c（extraction + JSON）|

**label 独立于 dc** 的理由：dc.json 是纯 DB 查询（稳定），label.json 需 LLM 可重跑（频繁更新）。
保持独立有利于 `-t label` 单独触发而不重跑 DB 查询。

### 2.2 每个文件的 Schema 骨架

所有文件共享相同的根结构：

```jsonc
{
  "schema_version": "3.0",
  "snapshot_id": 3,
  "sdp_name": "2026-04-07T18-57-50",
  "generated_at": "...",
  "total_dc_count": 1081,
  "draw_calls": [
    { "dc_id": "1.1.31", "api_id": 106974, /* 本文件专属字段 */ }
  ]
}
```

**Join Key**：`api_id`（整数，全局唯一于 capture） — 所有 6 个文件用同一 api_id 做跨文件 join。
`dc_id` 作为辅助人类可读 ID 也出现在每个文件中。

#### `snapshot_{id}_dc.json`（params + bindings + RT）

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "api_name": "vkCmdDraw",
  "pipeline_id": 84,
  "layout_id": 7,
  "render_pass_id": 12,
  "vertex_count": 6144,
  "index_count": 0,
  "instance_count": 1,
  "render_targets": [
    {
      "attachment_index": 0,
      "attachment_type": "Color",
      "resource_id": 55,
      "renderpass_id": 12,
      "framebuffer_id": 2,
      "width": 1920, "height": 1080,
      "format": "VK_FORMAT_B10G11R11_UFLOAT_PACK32"
    }
  ],
  "binding_summary": {
    "typed_buffer_view_count": 2,
    "small_buffer_count": 1,
    "has_per_instance_buffer": true
  }
}
```

#### `snapshot_{id}_label.json`

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "label": {
    "category": "Character",
    "subcategory": "SkinMesh",
    "detail": "Skinned mesh with SH probes",
    "reason_tags": ["skinning", "instancing"],
    "confidence": 0.95,
    "label_source": "llm"
  }
}
```

#### `snapshot_{id}_metrics.json`

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "metrics_available": true,
  "metrics": {
    "clocks": 1229690,
    "read_total_bytes": 1048576,
    "tex_fetch_stall_pct": 11.55,
    // ... 全量 Adreno counter
  }
}
```

#### `snapshot_{id}_shaders.json`

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "pipeline_id": 84,
  "shader_stages": [
    {
      "stage": "Vertex",
      "module_id": 10024,
      "entry_point": "main",
      "file": "../../shaders/pipeline_84_vs.spv"
    },
    {
      "stage": "Fragment",
      "module_id": 10025,
      "entry_point": "main",
      "file": "../../shaders/pipeline_84_fs.spv"
    }
  ],
  "shader_files": ["../../shaders/pipeline_84_vs.spv", "../../shaders/pipeline_84_fs.spv"]
}
```

#### `snapshot_{id}_textures.json`

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "texture_ids": [101, 102, 103],
  "textures": [
    {
      "texture_id": 101, "width": 1024, "height": 1024, "depth": 1,
      "format": "VK_FORMAT_BC1_RGB_UNORM_BLOCK", "layers": 1, "levels": 10,
      "file": "../../textures/texture_101.png"
    }
  ]
}
```

#### `snapshot_{id}_buffers.json`

```jsonc
{
  "dc_id": "1.1.31",
  "api_id": 106974,
  "vertex_buffers": [
    { "binding": 0, "buffer_id": 201 }
  ],
  "index_buffer": { "buffer_id": 202, "offset": 0, "index_type": "UINT16" },
  "mesh_file": "../../meshes/mesh_106974.obj"
}
```

### 2.3 raw.json 的处置

`snapshot_{id}_raw.json`（schema 2.0）**废弃**：不再生成，由 6 个 sub-JSON 替代。
旧文件不主动删除，但不再更新。

### 2.4 AttributionReportService 影响分析

`AttributionReportService`（Pass B Step B2）的输入来源：
- 内存 `DrawCallAnalysisReport`（label + shaders）
- `topdc.json`（文件读取）
- `status.json`（文件读取）
- `per_dc_content/`（DcContentAnalysisService 缓存）

**结论**：
- `PassMode.All`（默认）下内存 report 始终存在 → **AttributionReportService 无需修改**
- 仅在 `-t analysis` 单独触发（相当于旧 AnalysisOnly 模式）时，内存 report 不存在
  → 需要新增 `SubJsonLoadService`，从 6 sub-JSON 重建 `DrawCallAnalysisReport`
  → AttributionReportService 本身逻辑不变，调用侧注入重建的 report

### 2.5 派生产物（status/topdc 组）不变

下列产物维持现有命名和结构，无 schema 变更：

```
snapshot_{id}_status.json     ← Pass A Step A5（读内存 report）
snapshot_{id}_topdc.json      ← Pass A Step A6（读内存 report + rules）
snapshot_{id}_analysis.md     ← Pass B Step B2（LLM）
snapshot_{id}_dashboard.md    ← Pass B Step B3（rule-based）
```

---

## 3. Step 3 — analysis -target/-t 级联增量控制

### 3.1 完整 CLI 语法

```
sdpcli analysis <sdp_path>         # SDP 文件为 positional arg（必填），无 -sdp 关键词
  [-snapshot/-s <N|1>]             # 可选（N 从 2 开始；1 或不传 = all）
  [-target/-t <target>]            # 可选，默认 all
  [-output/-o <dir>]               # 可选，默认 AnalysisDir/<sdp_basename>/
  [--debug]                        # 可选，开启 DEBUG 日志（只接受双横线）

sdpcli snapshot [<package\activity>] # package\activity 为 positional arg（可选，config.ini 可指定）
  [-output/-o <dir>]                 # 可选，默认 SdpDir（即 ProjectDir/sdp）
  [--debug]                          # 可选
  # 运行后：ENTER=截帧（可多次）/ ESC=退出+生成 .sdp
```

> **为什么用 `-target`/`-t` 而不是 `-mode`/`-m`**：
> 过渡期代码中 `-mode` 关键词作为顶层路由 flag 已存在（旧 `-mode analysis`）。
> 用 `-target` 避免两套语义在同一 flag 上冲突，降低维护风险。
> 代码内部的 Mode 类命名（`AnalysisMode` 等）与 CLI flag 名称解耦。

### 3.2 -target/-t 可选值 + 级联依赖

| 值 | 直接更新目标 | 级联前序（自动先执行）|
|----|-------------|----------------------|
| `dc` | `dc.json` | —（纯 DB 查询）|
| `shaders` | `shaders.json` + extraction | `dc` |
| `textures` | `textures.json` + extraction | `dc` |
| `buffers` | `buffers.json` + mesh extraction | `dc` |
| `label` | `label.json` | `dc`、`shaders`（LLM 读 shader 内容）|
| `metrics` | `metrics.json` | `dc` |
| `status` | `status.json` | `dc`、`label`、`metrics`（raw 层全部）|
| `topdc` | `topdc.json` | `status`（含其前序）|
| `analysis` | `analysis.md` | `topdc`（含其前序）+ `per_dc_content/`（LLM cache）|
| `dashboard` | `dashboard.md` | `status`、`topdc`（含其前序）|
| `all` | 全部（默认）| —（顺序完整执行）|

**级联行为说明**：

- 指定 `-t status` → 自动先级联执行 `dc` → `shaders` → `label` → `metrics`，再执行 `status`
- 级联执行时，已存在且未失效的 JSON 文件**不重新生成**（文件时间戳比 sdp.db 新则视为有效）
- 未指定 `-t`（默认 `all`）→ 完整 pipeline，所有步骤顺序执行（等同于现有行为）

### 3.3 Extraction 与 JSON 同步策略

`-t shaders`、`-t textures`、`-t buffers` 三个 target 均**同步执行文件 extraction**：

| target | extraction 行为 | per-file 存在检查 |
|--------|----------------|-------------------|
| `shaders` | SPIR-V/HLSL 抽取到 `session/shaders/` | 单文件存在则 skip，不重抽取 |
| `textures` | PNG 抽取到 `session/textures/` | 单文件存在则 skip |
| `buffers` | OBJ mesh 抽取到 `session/meshes/` | 单文件存在则 skip |

> P2 扩展：`--no-extract` flag（只更新 JSON 不重抽取物理文件），P0/P1 不实现。

### 3.4 依赖链（完整 DAG）

```
Step 1  DB 查询 DrawCalls
  │     └─► dc.json
  │
  ├─► Step 1.5a  extraction + shaders.json   [需要 dc.json 的 pipeline IDs]
  ├─► Step 1.5b  extraction + textures.json  [需要 dc.json 的 texture IDs]
  ├─► Step 1.5c  extraction + buffers.json   [需要 dc.json 的 VB/IB IDs]
  │
  └─► Step 2  label.json   [reads shaders/ for LLM context; needs dc+shaders]
      │
      └─► Step 3  metrics.json  [DB join; needs dc]
          │
          ├─► Step A5  status.json     [reads memory report from dc+label+metrics]
          │   └─► Step A6  topdc.json  [reads memory report + rules]
          │       ├─► Step B2  analysis.md    [LLM + per_dc_content/]
          │       └─► Step B3  dashboard.md   [rule-based]
          └─────────────────────────────────────────────────────────►┘
```

### 3.5 内部 AnalysisTarget [Flags] enum（替代旧 PassMode）

```csharp
[Flags]
public enum AnalysisTarget
{
    None      = 0,
    Dc        = 1 << 0,   // dc.json
    Shaders   = 1 << 1,   // shaders.json + extraction
    Textures  = 1 << 2,   // textures.json + extraction
    Buffers   = 1 << 3,   // buffers.json + extraction
    Label     = 1 << 4,   // label.json
    Metrics   = 1 << 5,   // metrics.json
    Status    = 1 << 6,   // status.json
    TopDc     = 1 << 7,   // topdc.json
    Analysis  = 1 << 8,   // analysis.md
    Dashboard = 1 << 9,   // dashboard.md
    All       = (1 << 10) - 1
}

// 级联展开：将用户指定的 target 展开为需要执行的完整集合
public static AnalysisTarget ExpandWithDependencies(AnalysisTarget requested)
{
    var result = requested;
    if (result.HasFlag(Analysis))  result |= TopDc;
    if (result.HasFlag(Dashboard)) result |= TopDc | Status;
    if (result.HasFlag(TopDc))     result |= Status;
    if (result.HasFlag(Status))    result |= Dc | Label | Metrics;
    if (result.HasFlag(Metrics))   result |= Dc;
    if (result.HasFlag(Label))     result |= Dc | Shaders;
    if (result.HasFlag(Shaders))   result |= Dc;
    if (result.HasFlag(Textures))  result |= Dc;
    if (result.HasFlag(Buffers))   result |= Dc;
    return result;
}
```

### 3.6 旧 PassMode 的迁移

| 旧 flag | 等价新写法 |
|---------|----------|
| `-stats-only` | `-t status`（自动级联覆盖 dc/shaders/textures/buffers/label/metrics）|
| `-analysis-only` | `-t analysis` 或 `-t dashboard` |
| `-pass-mode all` | `-t all`（或不传）|
| `-pass-mode stats` | 同 `-stats-only` |
| `-pass-mode analysis` | 同 `-analysis-only` |

过渡期：旧 flag 保留作为 deprecated alias，内部转为等价 `-t` 语义后执行。

---

## 4. 补充设计要点

### 4.1 `-snapshot`/`-s` 参数语义（确认）

```
-snapshot/-s <N>   N 为整数，从 2 开始（snapshot_1 不是常规 DC snapshot）
-snapshot/-s 1     = all，分析所有 DC-carrying snapshot（哨兵值 1）
(不传)             默认 = all（等价于 -s 1）
```

> **snapshot_1 说明**：snapshot_1 对应 Realtime mode 的默认采集（内部命名 capture_1），
> 用于打通程序链路，不含独立帧数据。因此 `1` 作为 "all" 哨兵值不与真实 snapshot id 冲突。

与现有交互逻辑的关系：
- 命令行传 `-s 3` → 直接进入 snapshot_3 分析，跳过交互
- 不传 `-s`（all）且有 `<sdp_path>` → 遍历所有 snapshot_N（N≥2）
- InteractiveMode 下 → 列出 snapshot 列表由用户选择（保持现有行为）

### 4.2 Join Key 约定（确认）

所有 6 个 sub-JSON 以 **`api_id`（整数）** 作为跨文件主 key，每行均包含 `dc_id`（辅助字段）。

```python
# 消费方 join 示例
dc_map      = { dc["api_id"]: dc for dc in dc_json["draw_calls"] }
label_map   = { dc["api_id"]: dc for dc in label_json["draw_calls"] }
metrics_map = { dc["api_id"]: dc for dc in metrics_json["draw_calls"] }
# 按 api_id 横向合并
```

所有 7 个文件（6 sub-JSON + index）中，`api_id` 集合完全一致（同一 capture 同一 DC 集合）。

### 4.3 Index Manifest（确认纳入 P1）

`snapshot_{id}_index.json` 记录所有产物路径 + 每个产物的 `generated_at` 时间戳：

```jsonc
{
  "schema_version": "1.0",
  "snapshot_id": 3,
  "sdp_name": "2026-04-07T18-57-50",
  "outputs": {
    "dc":        { "file": "snapshot_3_dc.json",        "generated_at": "..." },
    "label":     { "file": "snapshot_3_label.json",     "generated_at": "..." },
    "metrics":   { "file": "snapshot_3_metrics.json",   "generated_at": "..." },
    "shaders":   { "file": "snapshot_3_shaders.json",   "generated_at": "..." },
    "textures":  { "file": "snapshot_3_textures.json",  "generated_at": "..." },
    "buffers":   { "file": "snapshot_3_buffers.json",   "generated_at": "..." },
    "status":    { "file": "snapshot_3_status.json",    "generated_at": "..." },
    "topdc":     { "file": "snapshot_3_topdc.json",     "generated_at": "..." },
    "analysis":  { "file": "snapshot_3_analysis.md",    "generated_at": "..." },
    "dashboard": { "file": "snapshot_3_dashboard.md",   "generated_at": "..." }
  }
}
```

用于：① UI 发现产物；② 级联依赖时间戳有效性校验；③ 增量执行 skip 判断。

### 4.4 旧带时间戳文件清理

`DrawCallAnalysis_2026-04-08_*.json` 等老格式文件不主动删除，但新模式不再生成。
`RawJsonGenerationService` 改名为 `SubJsonGenerationService` 或保留名称拆分为 6 个 writer 方法。

### 4.5 SDP 文件路径解析规则（新增）

`sdpcli analysis <sdp_path>` 的路径解析顺序：

1. **绝对路径**：直接使用
2. **相对路径**：依次尝试：
   a. `SdpDir`（config.ini 新增，默认 `ProjectDir/sdp`）
   b. `ProjectDir`（config.ini 新增，默认 = `WorkingDirectory/project`）
3. 若两者都找不到 → **报错退出**，不继续执行

> **注意**：`ProjectDir` 默认为 `WorkingDirectory/project`，不等于 `WorkingDirectory` 本身。
> 这样 `WorkingDirectory` 保持为顶层根目录，所有项目文件放在 `project/` 子目录下。

**新增 config.ini 键**：

```ini
# 根目录，用于解析相对路径（SDP、output 等）
# 默认值：<WorkingDirectory>/project
ProjectDir=D:\snapdragon\project

# SDP 文件默认搜索目录
# 相对路径以 ProjectDir 为基准
# 默认值：<ProjectDir>/sdp
SdpDir=sdp
```

### 4.6 Analysis 输出目录结构（新增）

**第一步 — SDP 解压到 outputDir**

analysis 的第一步是将 SDP 解压到 outputDir（会话级目录），所有后续分析
都在 outputDir 下进行，而不是原始 SDP 位置：

```
<AnalysisDir>/<sdp_basename>/      ← outputDir（会话级，unzip 目标）
  sdp.db                           ← 从 SDP 解压的 SQLite 数据库
  *.gfxrz                          ← 从 SDP 解压的帧数据文件
  <ShaderDir>/                     ← shader 提取目录（见 §4.7）
  <TextureDir>/                    ← texture 提取目录（见 §4.7）
  <MeshDir>/                       ← mesh/buffer 提取目录（见 §4.7）
  <DataDir>/                       ← JSON/MD 产物目录（见 §4.7）
    snapshot_2/
      dc.json
      shaders.json
      textures.json
      buffers.json
      label.json
      metrics.json
      status.json
      topdc.json
      snapshot_2_analysis.md
      snapshot_2_dashboard.md
      snapshot_2_index.json
    snapshot_3/
      ...
```

**`-o` 未指定时的默认 outputDir**：
```
<AnalysisDir>/<sdp_basename>/
```

例如：`sdpcli analysis sdp/2026-04-07T18-57-50.sdp -s 3`
→ outputDir = `analysis/2026-04-07T18-57-50/`
→ JSON 产物在 `analysis/2026-04-07T18-57-50/data/snapshot_3/`

**`-o` 指定时**：覆盖 outputDir（整个会话级目录路径），自动创建。

**新增 config.ini 键**：

```ini
# Analysis 产物输出根目录
# 相对路径以 ProjectDir 为基准
# 默认值：<ProjectDir>/analysis
AnalysisDir=analysis
```

若 `-s 1`（all），各 snapshot 各自使用 `<DataDir>/snapshot_N/` 子目录。

### 4.7 提取子目录配置（新增）

所有提取类子目录从 config.ini 配置，相对于 outputDir（会话级）：

| config 键 | 默认值 | 实际路径示例 | 内容 |
|-----------|--------|-------------|------|
| `ShaderDir` | `shaders` | `analysis/<session>/shaders/` | shader 提取结果 |
| `TextureDir` | `textures` | `analysis/<session>/textures/` | texture 提取结果 |
| `MeshDir` | `meshes` | `analysis/<session>/meshes/` | mesh/buffer 提取结果 |
| `DataDir` | `data` | `analysis/<session>/data/` | JSON 和 MD 产物 |

**新增 config.ini 键**：

```ini
# 相对 outputDir 的提取子目录；默认值已满足大多数场景
ShaderDir=shaders
TextureDir=textures
MeshDir=meshes
DataDir=data
```

---

## 5. 实现优先级

### P0（核心，先做）

- [x] `Main.cs`：positional subcommand + positional arg 解析；`-snapshot`/`-s`、`-target`/`-t`、`-output`/`-o`、`--debug`（双横线）；旧 `-mode` deprecated；**移除** `-launch/-l`、`-capture/-c` flag（SDK 约束，见 FINDING §问题2）
- [x] `Application.cs`：路由改写，移出 mode 3/4/5/6；接收 `sdpPath`（analysis positional）、`packageActivity`（snapshot positional）、`targetArg`、`snapshotIdArg`、`outputArg`；无 subcommand → InteractiveMode；**移除** `launchCapture` 参数
- [x] `Modes/InteractiveMode.cs`：**新增**，封装原 Application.Run() interactive 分支；snapshot/analysis 两选项；**维持现有完整交互体验**
- [x] `Modes/AnalysisMode.cs`：接收 sdpPath（positional）、`snapshotId`（-s）、`AnalysisTarget`（-t）、`outputPath`（-o）；SDP 路径解析（SdpDir → ProjectDir）；output 路径解析（AnalysisDir → ProjectDir → 自动创建）
- [ ] `Modes/SnapshotCaptureMode.cs`：接收 `packageActivity`（positional，可覆盖 config）；**移除** `-launch/-capture` flag 分支（`_NonInteractiveMode`、`IsNonInteractive` 逻辑）；**恢复** ENTER/ESC 最小交互循环（ENTER=截帧可多次 / ESC=退出+CreateSessionArchive）；`-output/-o` 输出目录解析保留
- [x] `Config.cs`：新增 `ProjectDir`、`SdpDir`、`AnalysisDir`、`ShaderDir`、`TextureDir`、`MeshDir`、`DataDir` 键读取；解析相对路径（ProjectDir 为基准）
- [x] `RawJsonGenerationService`：拆为 6 个 writer 方法；文件名改变
- [x] `AnalysisPipeline.cs`：`AnalysisTarget` [Flags] enum + `ExpandWithDependencies()`；`RunAnalysis(target)` 步骤门控；raw.json 调用改为 6 个 writer

### P1（次要，随后）

- [x] 级联前序文件存在检查：缺少时报错并提示需要先跑哪个 target（而非 crash）
- [x] `-t analysis` 单独触发时：`SubJsonLoadService`（新增）从 6 sub-JSON 重建内存 `DrawCallAnalysisReport`
- [x] `snapshot_{id}_index.json` manifest 生成：每次有产物写出时更新对应 entry
- [x] 旧 `-stats-only`/`-analysis-only`/`-pass-mode` deprecated alias 转换
- [x] ~~**`-capture` 不再调用 `CreateSessionArchive`**~~ — **CANCELLED**：每次进程生成独立 `.sdp` 是正确行为（SDK 约束）
- [x] ~~**新增 `-archive` flag**~~ — **CANCELLED**：`sdp.db` 无法跨进程共享，不实现
- [x] ~~**Staging session directory 机制**~~ — **CANCELLED**：同上
- [x] ~~`Main.cs` + `Application.cs`：解析 `-archive`~~ — **CANCELLED**：同上

### P2（完整化）

- [x] 时间戳有效性校验（index.json `generated_at` vs `sdp.db` mtime）
- [x] `--no-extract` flag（只更新 JSON 不重抽取物理文件）
- [x] `SDPCLI.bat` 更新使用示例
- [x] `SDPCLI/CLI_PARAMETERS.md` 更新

---

## 6. 文件变更范围

| 文件 | 变更类型 | 描述 |
|------|----------|------|
| `Main.cs` | 修改 | positional subcommand + positional arg；`-snapshot`/`-s`、`-target`/`-t`、`-output`/`-o`、`--debug`（双横线）；旧 `-mode` deprecated；**移除** `-launch/-capture` flag |
| `Application.cs` | 修改 | 移出 mode 3/4/5/6 routing；接收 sdpPath/packageActivity/targetArg/snapshotIdArg/outputArg；无 subcommand → InteractiveMode；移除 launchCapture 参数 |
| `Modes/InteractiveMode.cs` | **新增** | 封装原 Application.Run() interactive 分支；维持现有完整交互体验 |
| `Modes/AnalysisMode.cs` | 修改 | 接收 sdpPath（positional）、`AnalysisTarget`（-t）、snapshotId（-s）、outputPath（-o）；SDP/output 路径解析 |
| `Modes/SnapshotCaptureMode.cs` | 修改（**待回退**）| 接收 packageActivity（positional，覆盖 config）；**移除** `-launch/-capture` 分支（`_NonInteractiveMode`/`IsNonInteractive`）；**恢复** ENTER/ESC 最小交互循环（ENTER=截帧可多次 / ESC=退出+archive）；`-output/-o` 输出目录解析保留 |
| `Config.cs` | 修改 | 新增 ProjectDir、SdpDir、AnalysisDir、ShaderDir、TextureDir、MeshDir、DataDir 键；相对路径解析 |
| `Analysis/AnalysisPipeline.cs` | 修改 | `AnalysisTarget` enum；`ExpandWithDependencies()`；`RunAnalysis(target)` 步骤门控；raw.json → 6 sub-JSON |
| `Services/Analysis/RawJsonGenerationService.cs` | 修改 | 拆为 6 writer 方法；文件名改变；可改名 `SubJsonGenerationService` |
| `Services/Analysis/SubJsonLoadService.cs` | **新增**（P1）| 从 6 sub-JSON 重建 `DrawCallAnalysisReport`（供 `-t analysis` 单独触发）|
| `AttributionReportService.cs` | **不修改**（P0/P1）| all 模式走内存路径；-t analysis 单独模式由 SubJsonLoadService 注入 |
| `DrawCallAnalysisMode.cs` | **废弃**（不删除）| 从路由中移除；代码文件暂时保留 |
| `TextureExtractionMode.cs` | **废弃** | 功能通过 `analysis -t textures` 访问 |
| `ShaderExtractionMode.cs` | **废弃** | 功能通过 `analysis -t shaders` 访问 |
| `MeshExtractionMode.cs` | **废弃** | 功能通过 `analysis -t buffers` 访问 |

---

## 7. 验证方案

```bash
# 分析 snapshot_3（相对路径，SdpDir=<ProjectDir>/sdp）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3

# 绝对路径
sdpcli analysis D:/snapdragon/project/sdp/2026-04-07T18-57-50.sdp -s 3

# 分析所有 snapshot（-s 1 = all，等价于不传）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 1
sdpcli analysis 2026-04-07T18-57-50.sdp

# 只重跑 label（修改规则后；自动级联 dc+shaders）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3 -t label

# 只更新 topdc（自动级联 dc/shaders/label/metrics/status）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3 -t topdc

# 指定输出目录（绝对）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3 -o d:/output/my-session

# 指定输出目录（相对 AnalysisDir）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3 -o custom/my-session

# 开启 debug 日志（双横线）
sdpcli analysis 2026-04-07T18-57-50.sdp -s 3 --debug

# Snapshot — launch app（完整写法 / 缩写均可）
sdpcli snapshot com.ea.fcmnova\com.ea.frostbite.FrostbiteActivity -launch
sdpcli snapshot com.ea.fcmnova\com.ea.frostbite.FrostbiteActivity -l

# Snapshot — capture frame（app 已运行）
sdpcli snapshot com.ea.fcmnova\com.ea.frostbite.FrostbiteActivity -capture
sdpcli snapshot com.ea.fcmnova\com.ea.frostbite.FrostbiteActivity -c

# Snapshot — 使用 config.ini 的 PackageName/ActivityName（不传 positional）
sdpcli snapshot -launch

# 交互（InteractiveMode — 现有体验不变）
sdpcli
```

预期产物（`<AnalysisDir>/2026-04-07T18-57-50/` = outputDir 下）：

```
2026-04-07T18-57-50/
  sdp.db                         ← 从 SDP 解压
  *.gfxrz                        ← 从 SDP 解压
  shaders/                       ← ShaderDir（默认 shaders）
    pipeline_*.spv / *.hlsl
  textures/                      ← TextureDir（默认 textures）
    texture_*.png
  meshes/                        ← MeshDir（默认 meshes）
    mesh_*.obj
  data/                          ← DataDir（默认 data）
    snapshot_3/
      dc.json                    ← Step 1
      shaders.json               ← Step 1.5a
      textures.json              ← Step 1.5b
      buffers.json               ← Step 1.5c
      label.json                 ← Step 2
      metrics.json               ← Step 3
      status.json                ← Step A5
      topdc.json                 ← Step A6
      snapshot_3_analysis.md     ← Step B2
      snapshot_3_dashboard.md    ← Step B3
      snapshot_3_index.json      ← manifest
    snapshot_4/
      ...
```

---

## 8. 新增 config.ini 键汇总

| 键名 | 默认值 | 说明 |
|------|--------|------|
| `ProjectDir` | `<WorkingDirectory>/project` | 根目录，用于解析所有相对路径（注意：不等于 WorkingDirectory）|
| `SdpDir` | `sdp`（相对 ProjectDir）| analysis SDP 文件默认搜索目录；同时也是 snapshot 模式的默认输出目录 |
| `AnalysisDir` | `analysis`（相对 ProjectDir）| analysis 产物默认输出根目录（会话级）|
| `ShaderDir` | `shaders`（相对 outputDir）| shader 提取子目录 |
| `TextureDir` | `textures`（相对 outputDir）| texture 提取子目录 |
| `MeshDir` | `meshes`（相对 outputDir）| mesh/buffer 提取子目录 |
| `DataDir` | `data`（相对 outputDir）| JSON 和 MD 产物子目录 |

**路径解析顺序（统一规则）**：

```
绝对路径           → 直接使用
相对路径（SDP）    → 先 SdpDir，再 ProjectDir，找不到 → error
相对路径（output） → 先 AnalysisDir，再 ProjectDir，不存在 → 自动 mkdir    [analysis 模式]
相对路径（output） → 先 SdpDir，再 ProjectDir，不存在 → 在 SdpDir 下自动 mkdir  [snapshot 模式]
提取子目录         → 相对于 outputDir（会话级），不存在 → 自动 mkdir
```

**snapshot `-output/-o` 默认值**：`SdpDir`（不传时直接在 SdpDir 下生成 .sdp 文件）。

**`config.ini` 参考配置片段**（新增部分）：

```ini
# ---------------------------------------------------------------------------
# Project paths
# ---------------------------------------------------------------------------
# Root directory for resolving relative paths.
# Default: <WorkingDirectory>/project  (NOT WorkingDirectory itself)
ProjectDir=D:\snapdragon\project

# Default directory to search for .sdp files (relative to ProjectDir).
# Example: sdpcli analysis 2026-04-07.sdp  →  looks in ProjectDir/SdpDir/
# Default: sdp
SdpDir=sdp

# Default root directory for analysis outputs (relative to ProjectDir).
# Each session creates: AnalysisDir/<sdp_basename>/  (then DataDir/snapshot_N/ inside)
# Default: analysis
AnalysisDir=analysis

# Sub-directories for extraction outputs, all relative to the session outputDir.
# Default values shown below:
ShaderDir=shaders
TextureDir=textures
MeshDir=meshes
DataDir=data
```

**Snapshot 模式 package/activity 覆盖逻辑**：

```
sdpcli snapshot <pkg\act> -launch   → 使用命令行 positional arg（覆盖 config）
sdpcli snapshot -launch             → 使用 config.ini 的 PackageName + ActivityName
```

两种方式均完整支持，config.ini 作为默认值，命令行可覆盖。
