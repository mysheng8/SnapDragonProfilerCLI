# pySdp

Snapdragon Profiler 数据分析平台 — Python 层，含 WebUI、数据层、分析服务和 Python 客户端库。

---

## 架构概览

```
Browser (localhost:8000)
  └── WebUI SPA (index.html + app.js)
        │
        ├── /api/sdpcli/*    ──代理──►  SDPCLI Server (localhost:5000)   [前端专用]
        ├── /api/snapshot/*  ──►  设备/应用发现 + 快照工作流
        ├── /api/jobs/*      ──►  C# 提取 + Python 分析步骤触发
        ├── /api/files/*     ──►  本地文件服务（只读）
        ├── /api/data/*      ──►  DuckDB 数据查询（含 MCP）
        └── /api/logs/*      ──►  日志流

pySdp/
  webui/        FastAPI 应用（后端 + 静态资源）
  analysis/     Python 分析服务（labeling、status、topdc 等）
  data/         DuckDB 数据层（ingest、query、models、questions）
  pysdp/        独立 Python 客户端包（脚本/CI 使用）
```

---

## 快速开始

```powershell
# 在 snapdragon 根目录执行（首次需先编译：dotnet build SDPCLI）
.\webui.ps1
```

`webui.ps1` 自动完成：创建 `.venv` → 安装依赖 → 启动 SDPCLI Server → 启动 WebUI → 打开浏览器。按 ESC 同时停止两个进程。

```powershell
# 可选参数
.\webui.ps1 -Port 8080 -SdpcliPort 5001 -BindHost 0.0.0.0
```

**手动启动（调试用）：**
```powershell
# 终端 1 — SDPCLI Server
.\sdpcli.bat server --port 5000

# 终端 2 — WebUI
cd pySdp
.venv\Scripts\python webui/server.py --host 127.0.0.1 --port 8000 --sdpcli http://localhost:5000
```

> `SDPCLI_URL` 环境变量可覆盖默认的 `http://localhost:5000`。
>
> API 文档（Swagger）：**http://localhost:8000/api/docs**

---

## 目录结构

```
pySdp/
├── webui/
│   ├── server.py              # FastAPI 入口 + uvicorn
│   ├── routes/
│   │   ├── proxy.py           # /api/sdpcli/*      → SDPCLI Server 透传（前端 SPA 专用）
│   │   ├── snapshot_router.py # /api/snapshot/*    → 设备/应用发现 + 快照工作流
│   │   ├── jobs_router.py     # /api/jobs/*        → 提取/分析步骤触发（C# + Python）
│   │   ├── files.py           # /api/files/*       → 文件浏览与服务（只读）
│   │   ├── data.py            # /api/data/*        → DuckDB 查询端点（MCP 暴露）
│   │   └── logs.py            # /api/logs/*        → WebUI 日志流
│   ├── jobs.py                # 服务端 Pipeline Job（后台线程 + 状态管理）
│   ├── logger.py              # WebUI 日志模块
│   └── static/
│       ├── index.html         # 单页 HTML
│       ├── app.js             # 前端逻辑（原生 JS，无构建步骤）
│       └── style.css
├── analysis/
│   ├── label_service.py       # DrawCall 规则分类 → label.json + DB
│   ├── status_service.py      # 百分位统计 → status.json + DB
│   ├── topdc_service.py       # Top-DC 瓶颈归因 → topdc.json
│   ├── dashboard_service.py   # Mermaid 图表 → dashboard.md
│   ├── analysis_md_service.py # LLM 分析报告 → analysis.md
│   ├── mesh_stats_service.py  # OBJ 解析 → meshes.json
│   ├── texture_stats_service.py # 贴图尺寸 → textures.json
│   ├── vlm_screenshot_service.py # VLM 场景描述 → scene_description.md
│   ├── llm_wrapper.py         # LLM HTTP 客户端
│   └── models/
│       ├── base.py
│       ├── category_breakdown.py
│       ├── label_quality.py
│       └── top_bottleneck_dcs.py
├── data/
│   ├── db.py                  # WorkspaceDB（DuckDB 连接 + Schema DDL）
│   ├── ingest.py              # snapshot_dir → DuckDB（幂等）
│   ├── query.py               # 类型化 Read API
│   ├── model_registry.py      # 分析模型注册表
│   ├── questions.py           # Questions CRUD
│   └── dashboards.py          # Dashboards CRUD
├── pysdp/
│   ├── client.py              # SdpClient（同步阻塞 API）
│   ├── _jobs.py               # JobPoller
│   ├── _models.py             # JobStatus / DeviceInfo 数据类
│   └── exceptions.py          # 异常体系
├── examples/
│   ├── snapshot.py
│   └── batch_analysis.py
└── requirements.txt
```

---

## WebUI 功能

### Snapshot 标签页

三步操作面板：

| 步骤 | 说明 |
|------|------|
| 1 — Connect | 下拉选择 ADB 设备（↻ 刷新），留空自动选设备 |
| 2 — Launch | 下拉选择已安装 Package，选完后自动加载 Activity 下拉 |
| 3 — Capture | 触发帧抓取，带进度条，完成后显示 sdpPath / captureId |

### Analysis 标签页

- **SDP Files**：扫描目录列出 `.sdp` 文件，点击触发分析
- **Analysis Settings**（折叠）：选择 Snapshot ID + 分析 targets
- 分析时显示进度条和阶段名；多 snapshot 时格式为 `[1/3] snapshot_2 / label`
- C# 分析 Job 和 Python Pipeline Job 均持久化到 `localStorage`，刷新页面可恢复

### Questions 标签页

- 共享 Snapshot 选择器：扫描 DuckDB 已导入的 snapshot，点击切换
- 可视化分析结果：Pie / Bar 图，指标按钮切换，Pearson 相关性表

### Explorer 标签页

- DrawCall 列表（带 Category 筛选、时钟柱状图）
- DC Detail 面板：Metrics 热力图 · Textures 预览 · OBJ 3D 预览 · Shaders HLSL 预览

### Results 标签页

Snapshot 文件查看器：列出 `dc.json`、`label.json`、`status.json` 等结果文件，内联预览 JSON / Markdown。

### Logs 标签页

WebUI 服务端日志，支持 Error / Warning / Info 过滤。

---

## API 路由总览

完整文档见 Swagger：**http://localhost:8000/api/docs**

路由组按用途分为四类：

| 前缀 | Tag | 用途 |
|---|---|---|
| `/api/sdpcli/*` | `frontend` | 前端 SPA 专用透传，不做文档化 |
| `/api/snapshot/*` | `snapshot` | 设备发现、快照工作流（typed + docs） |
| `/api/jobs/*` | `jobs` | 触发 C# 提取 + Python 分析步骤 |
| `/api/files/*` | `files` | 只读文件服务 |
| `/api/data/*` | `data` | DuckDB 数据查询（含 MCP 暴露） |

详细端点列表见 [docs/explanations/EXPLAIN-api.md](../docs/explanations/EXPLAIN-api.md)。

---

## 数据层（data/）

### WorkspaceDB

单例 DuckDB 连接，Schema 包含：

| 表 | 说明 |
|---|---|
| `snapshots` | 每次 ingest 的 snapshot 元数据（路径、sdp_name、snap_index、ingested_at） |
| `draw_calls` | DC 基础参数（api_id、顶点数、实例数等） |
| `labels` | 分类结果（category、subcategory、confidence、reason_tags） |
| `metrics` | GPU 计数器（clocks、fragments_shaded、tex_fetch_stall_pct 等 ~50 列） |
| `shader_stages` | Pipeline → Shader Stage 映射 |
| `textures` / `meshes` | 资产路径 |
| `questions` / `dashboards` | 用户定义分析查询 |

### ingest.py

`ingest_snapshot(db, snapshot_dir)` — 幂等，重复调用安全。读取 `dc.json`、`label.json`、`metrics.json`、`shaders.json`、`textures.json`、`buffers.json`，写入 DuckDB。`snapshot_dir` 作为唯一键，`snap_index` 保留 C# 原始编号。

---

## 分析服务（analysis/）

Python Pipeline 执行顺序（在 C# 写完 JSON 后运行）：

```
screenshot → mesh_stats → texture_stats → ingest → label → status → topdc → analysis_md → scene_describe
```

各步骤非致命：单步失败不影响已完成的步骤。

| 服务 | 输入 | 输出 | 写 DB |
|---|---|---|---|
| `mesh_stats_service` | `meshes/*.obj` | `meshes/meshes.json` | ✓（re-ingest） |
| `texture_stats_service` | `textures/` | `textures/textures.json` | ✓（re-ingest） |
| `label_service` | `dc.json` + `shaders.json` | `label.json` | ✓ |
| `status_service` | `dc.json` + `label.json` + `metrics.json` | `status.json` | ✓ |
| `topdc_service` | `status.json` + `attribution_rules.json` | `topdc.json` | — |
| `analysis_md_service` | `topdc.json` | `analysis.md` | — |
| `vlm_screenshot_service` | screenshot + label/metrics | `scene_description.md` | ✓ |

---

## pysdp 客户端包

独立包，无需 WebUI，直接在脚本 / CI 中使用：

```python
from pysdp import SdpClient

client = SdpClient("http://localhost:5000")
client.connect()
client.launch("com.example.app/.MainActivity")
result = client.capture()
analysis = client.analyze(
    sdp_path=result["sdpPath"],
    snapshot_id=result["captureId"],
    targets="label,metrics,status,topdc",
)
print(analysis["captureDir"])
```

| 方法 | 说明 |
|---|---|
| `connect(device_id=None)` | 连接设备 |
| `launch(package_activity)` | 启动 App |
| `capture(output_dir=None, label=None)` | 触发抓帧 |
| `analyze(sdp_path, snapshot_id, targets=None)` | 离线分析 |
| `disconnect()` | 断开设备 |
| `device_status()` | 查询设备状态 |

异常：`SdpStateError` / `SdpJobError` / `SdpTimeoutError` / `SdpConnectionError`

---

## 关键约束

- **DuckDB 连接**：`WorkspaceDB` 是进程内单例；所有查询使用 `db.cursor()`（独立游标）
- **snapshot_id 冲突**：`snapshot_dir` 是唯一键；C# 同一 session 内编号可能重叠，ingest 自动分配全局唯一 ID，`snap_index` 保留原始编号
- **Render Targets**：不存 DuckDB，运行时从 `dc.json` 读取
- **Screenshot**：优先分析目录缓存，fallback 从 `.sdp` ZIP 内 `snapshot_N/*.bmp` 提取
- **MCP**：通过 `fastapi-mcp` 暴露 19 个只读查询端点；挂载点 `/mcp`
