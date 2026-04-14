# Snapdragon Profiler Command Line Tool

无头模式的 Snapdragon Profiler CLI 工具，支持三种核心模式：

| 模式 | 用途 | 详细文档 |
|------|------|---------|
| `snapshot` | 连接设备，触发 Vulkan 帧抓取，保存 .sdp 文件 | [EXPLAIN-snapshot.md](../docs/explanations/EXPLAIN-snapshot.md) |
| `analysis` | 离线分析 .sdp 文件，生成 DrawCall 报告（JSON + Markdown） | [EXPLAIN-analysis.md](../docs/explanations/EXPLAIN-analysis.md) |
| `server` | 启动 HTTP API 服务，供脚本/CI 调用 | [EXPLAIN-server.md](../docs/explanations/EXPLAIN-server.md) |

---

## 快速开始

### 前置要求
1. 安装 [Snapdragon Profiler](https://www.qualcomm.com/developer/software/snapdragon-profiler) 到默认路径：
   ```
   C:\Program Files\Qualcomm\Snapdragon Profiler
   ```
2. .NET Framework 4.7.2+（内置于 Windows 10+，无需单独安装）
3. USB 连接 Android 设备（仅 snapshot 模式需要），启用 USB 调试

### 编译

```powershell
cd D:\snapdragon
dotnet build SDPCLI
# 输出：SDPCLI\bin\Debug\net472\SDPCLI.exe
```

---

## 使用方式

### snapshot 模式

```powershell
SDPCLI.exe snapshot com.your.app\ActivityName          # 直接抓帧
SDPCLI.exe snapshot com.your.app\ActivityName -o D:\out # 指定输出目录
SDPCLI.exe                                              # 交互菜单
```

`config.ini` 关键配置（与 exe 同目录）：
```ini
PackageName=com.your.app
ActivityName=.MainActivity
RenderingAPI=16   # 16=Vulkan
```

**核心机制**：`StartCapture()` → 等待 `OnCaptureComplete`（30s）→ 等待数据稳定 → `ImportCapture()` replay → 等待 `ImportCompleteEvent` → 读取 DsbBuffer → 导出 CSV → 压缩为 .sdp。同一 session 可连续按 ENTER 抓多帧，每帧以 `captureId` 区分，数据累积在同一 `sdp.db` 中。

输出：`<SdpDir>/<timestamp>/snapshot_N/`（CSV × 7 + screenshot）→ ZIP 为 `.sdp`

→ [完整调用链与时序约束](../docs/explanations/EXPLAIN-snapshot.md)

---

### analysis 模式

```powershell
SDPCLI.exe analysis path\to\capture.sdp              # 分析所有 snapshot
SDPCLI.exe analysis capture.sdp -s 3                 # 仅 snapshot_3
SDPCLI.exe analysis capture.sdp -s 3 -t label,status # 指定分析目标
```

**`-t` 可用值**（支持逗号组合，依赖自动补全）：

| 目标 | 输出文件 | 自动引入依赖 |
|------|---------|------------|
| `dc` | `dc.json`（DrawCall 基础数据+资源引用） | — |
| `shaders` | `shaders.json` + SPIR-V/HLSL 提取 | dc |
| `textures` | `textures.json` + PNG 提取 | dc |
| `buffers` | `buffers.json` + OBJ mesh | dc |
| `label` | `label.json`（分类：Shadow/UI/VFX/Scene/…） | dc, shaders |
| `metrics` | `metrics.json`（GPU 计数器） | dc |
| `status` | `status.json`（全局 + 分类百分位统计） | dc, label, metrics |
| `topdc` | `topdc.json`（Top-5 DC + 三层瓶颈归因） | status |
| `analysis` | `analysis.md`（LLM 推理报告） | topdc |
| `dashboard` | `dashboard.md`（规则图表） | topdc, status |

输出：`<ProjectDir>/analysis/<sdp_basename>/snapshot_N/`

→ [完整 Pipeline、数据模型与报告结构](../docs/explanations/EXPLAIN-analysis.md)

---

### server 模式

```powershell
SDPCLI.exe server              # 默认端口 5000
SDPCLI.exe server --port 8080
```

所有长操作均为异步 Job，立即返回 `202 + {jobId}`，客户端轮询 `/api/jobs/{id}` 等待完成。

**典型工作流**：
```
POST /api/connect → POST /api/session/launch → POST /api/capture → POST /api/analysis
每步均为 Job，轮询直到 "status": "Completed"，result 包含输出路径
```

**Job 状态**：`Pending` → `Running`（含 `phase` + `progress` 0~100）→ `Completed` / `Failed` / `Cancelled`

| Method | Path | 说明 |
|--------|------|------|
| `GET`  | `/api/status` | 健康检查 |
| `GET`  | `/api/device` | 设备/Session 状态 |
| `POST` | `/api/connect` | 连接设备（Job） |
| `POST` | `/api/disconnect` | 断开设备（同步） |
| `POST` | `/api/session/launch` | 启动 App（Job） |
| `POST` | `/api/capture` | 触发 Snapshot（Job） |
| `POST` | `/api/analysis` | 离线分析（Job） |
| `GET`  | `/api/jobs` | 列出所有 Job |
| `GET`  | `/api/jobs/{id}` | 查询状态/进度/结果 |
| `POST` | `/api/jobs/{id}/cancel` | 取消 Job |
| `DELETE` | `/api/jobs/{id}` | 删除已结束的 Job |

`config.ini` 可选：`Server.Port=5000` / `Server.JobTtlMinutes=60`

→ [HTTP 路由、Job 系统、状态机与各 Runner 详细实现](../docs/explanations/EXPLAIN-server.md)

---

## 命令行参数

### 子命令

| 子命令 | 说明 |
|-----------|------|
| `snapshot` | 连接设备抓帧模式 |
| `analysis` | 离线分析 .sdp 文件 |
| `server` | 启动 HTTP API 服务 |
| （无） | 交互模式，显示菜单 |

### 位置参数

| 位置 | 适用子命令 | 说明 |
|--------|----------|------|
| 第 2 位 | `analysis` | .sdp 文件路径 |
| 第 2 位 | `snapshot` | `pkg\Activity` 包名\活动 |

### 选项标志

| 标志 | 简写 | 适用子命令 | 说明 |
|--------|------|----------|------|
| `-output <dir>` | `-o` | all | 输出目录覆盖默认路径 |
| `-snapshot <N>` | `-s` | `analysis` | 已分析的 snapshot ID |
| `-target <mode>` | `-t` | `analysis` | 分析通道：`status` / `analysis` / `label` |
| `--no-extract` | 无 | all | 跳过资产提取，只写 JSON/CSV |
| `--port <N>` | 无 | `server` | HTTP 端口，默认 5000 |
| `--host <h>` | 无 | `server` | 绑定地址（保留，默认 localhost） |
| `--debug` | 无 | all | 开启 DEBUG 日志模式 |

### 已废弃标志（向后兼容）

| 旧标志 | 替代方式 |
|--------|--------|
| `-mode 1\|2` | `snapshot` / `analysis` 子命令 |
| `-sdp <path>` | `analysis <path>` |
| `-pass-mode <v>` | `-t <v>` |
| `-stats-only` | `-t status` |
| `-analysis-only` | `-t analysis` |

## 目录结构

```
SDPCLI/
├── config.ini                    # 运行时配置（与 exe 同目录）
├── source/
│   ├── Main.cs                   # 入口：DLL 路径设置、子命令路由
│   ├── Application.cs            # 应用生命周期
│   ├── Modes/                    # 模式入口
│   │   ├── SnapshotCaptureMode.cs
│   │   ├── AnalysisMode.cs
│   │   └── ServerMode.cs
│   ├── Services/
│   │   ├── Capture/              # 设备连接、抓帧、导出服务
│   │   └── Analysis/             # DB 查询、DrawCall 分析、报告生成
│   ├── Analysis/AnalysisPipeline.cs  # 分析流程编排
│   ├── Server/                   # HTTP 服务器、Handler、Job 系统
│   └── Tools/                    # TextureExtractor / ShaderExtractor / LlmApiWrapper
└── bin/Debug/net472/SDPCLI.exe
```

日志位置：`bin/Debug/net472/.log/consolelog.txt`（启动时打印完整路径，追加写入，10 MB 后轮转）

---

## 关键约束

- **DLL 部署**：`bin/Debug/` 下必须有 `SDPClientFramework.dll`、`QGLPlugin.dll`、`SDPCoreWrapper.dll`（构建时由 SDPCLI.csproj 自动从 Snapdragon Profiler 安装目录复制）
- **插件目录**：snapshot 模式需要 `bin/Debug/plugins/processor/*.dll`（见 `<Target Name="CopyPlugins">`）
- **config.ini**：必须与 SDPCLI.exe 同目录
- **server 仅本地**：HttpListener 绑定 `localhost`，不对外暴露
- **`analysis` sdpPath**：必须绝对路径 + `.sdp` 后缀，禁止 `..` 路径遍历

---

## 常见问题

**找不到 ProcessorPlugin**  
检查 `bin/Debug/plugins/processor/` 是否包含 `QGLObserverPluginProcessor.dll`。参考 SDPCLI.csproj 的 `<Target Name="CopyPlugins">` 配置。

**Texture 查询不准确 / DrawCallDescriptorBindings 为空**  
`SnapshotDsbBuffer` 仅在 snapshot 抓帧的 replay 阶段填充，不写入 .sdp。必须先经过 snapshot 模式抓帧，自定义表才有绑定数据。

**编译错误：VkSnapshotModel is internal**  
`QGLPlugin.VkSnapshotModel` 是 `internal` 类，不可直接访问。应使用 `ProcessorPlugin.GetLocalBuffer()` 公开 API。

---

## 深入了解

| 文档 | 涵盖内容 |
|------|---------|
| [EXPLAIN-snapshot.md](../docs/explanations/EXPLAIN-snapshot.md) | 完整调用链（8 阶段）、SDK VulkanSnapshot* 表写入时序、DescriptorSet 二进制处理、多帧共存设计、关键超时约束 |
| [EXPLAIN-analysis.md](../docs/explanations/EXPLAIN-analysis.md) | AnalysisPipeline Pass A/B 全流程、AnalysisTarget 10 标志位与依赖级联、DrawCallInfo/DrawCallMetrics 数据模型、dc.json/status.json/topdc.json 完整结构、三层归因引擎 |
| [EXPLAIN-server.md](../docs/explanations/EXPLAIN-server.md) | HTTP 路由表（11 个端点）、Job 数据结构与生命周期、设备状态机（6 状态）、ConnectJobRunner/LaunchJobRunner/CaptureJobRunner/AnalysisJobRunner 各阶段进度、ManualResetEvent 异步桥接 |
| [EXPLAIN-three-modes.md](../docs/explanations/EXPLAIN-three-modes.md) | 三模式关系概览、SDK 架构与 DLL 依赖（为何需要 SDPClientFramework）、关键 API 参考、DescSetBindings 数据结构、sdp.db 核心表总览 |


