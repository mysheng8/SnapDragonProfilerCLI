# pySdp

Snapdragon Profiler 数据分析平台 — Python 层，含 WebUI、数据层、分析服务和 Python 客户端库。

---

## 架构概览

```
Browser (localhost:8000)
  └── WebUI SPA (index.html + app.js)
        │
        ├── /api/sdpcli/*  ──代理──►  SDPCLI Server (localhost:5000)
        ├── /api/files/*   ──►  本地文件服务 (files.py)
        ├── /api/data/*    ──►  DuckDB 数据层 (data.py)
        └── /api/logs/*    ──►  日志流 (logs.py)

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

---

## 目录结构

```
pySdp/
├── webui/
│   ├── server.py              # FastAPI 入口 + uvicorn
│   ├── routes/
│   │   ├── proxy.py           # /api/sdpcli/* → SDPCLI Server
│   │   ├── files.py           # /api/files/*  文件服务（浏览、预览、下载）
│   │   ├── data.py            # /api/data/*   DuckDB 查询端点
│   │   └── logs.py            # /api/logs/*   WebUI 日志流
│   ├── jobs.py                # 服务端 Pipeline Job（后台线程）
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
│   ├── llm_wrapper.py         # LLM HTTP 客户端
│   └── models/
│       ├── base.py            # AnalysisModel 基类
│       ├── category_breakdown.py
│       ├── label_quality.py
│       └── top_bottleneck_dcs.py
├── data/
│   ├── db.py                  # WorkspaceDB（DuckDB 连接 + Schema DDL）
│   ├── ingest.py              # snapshot_dir → DuckDB（幂等）
│   ├── query.py               # 类型化 Read API（draw_calls、labels、metrics、dc_detail）
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
├── requirements.txt
└── webui.bat                  # Windows 快速启动
```

---

## WebUI 功能

### Snapshot 标签页

三步操作面板：

| 步骤 | 说明 |
|------|------|
| 1 — Connect | 连接 Android 设备（ADB），留空自动选设备 |
| 2 — Launch | 启动目标 App（Package + Activity） |
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
- DC Detail 面板：
  - Metrics：数值 + `med: xxx` + 绿→红热力图着色（基于 min/median/max）
  - Textures：尺寸、格式，可展开预览
  - Buffers/Mesh：OBJ 3D 预览（线框切换 + Verts/Tris 统计）
  - Render Targets：attachment_index、type、尺寸、format
  - Shaders：HLSL 代码预览 + ⬇ 下载按钮

### Results 标签页

Snapshot 文件查看器：列出 `dc.json`、`label.json`、`status.json` 等结果文件，内联预览 JSON / Markdown。

### Logs 标签页

WebUI 服务端日志，支持 Error / Warning / Info 过滤。

---

## 数据层（data/）

### WorkspaceDB

单例 DuckDB 连接，Schema 包含：

| 表 | 说明 |
|---|---|
| `snapshots` | 每次 ingest 的 snapshot 元数据（路径、sdp_name、ingested_at） |
| `draw_calls` | DC 基础参数（api_id、顶点数、实例数等） |
| `labels` | 分类结果（category、subcategory、confidence、reason_tags） |
| `metrics` | GPU 计数器（clocks、fragments_shaded、tex_fetch_stall_pct 等 ~50 列） |
| `shader_stages` | Pipeline → Shader Stage 映射 |
| `textures` / `meshes` | 资产路径 |
| `questions` / `dashboards` | 用户定义分析查询 |

### ingest.py

`ingest_snapshot(db, snapshot_dir)` — 幂等，重复调用安全。读取 `dc.json`、`label.json`、`metrics.json`、`shaders.json`、`textures.json`、`buffers.json`，写入 DuckDB。

### query.py 主要接口

| 函数 | 返回 |
|---|---|
| `get_draw_calls(db, snapshot_id, ...)` | DC + label + metrics 列表 |
| `get_dc_detail(db, snapshot_id, api_id)` | 单个 DC 完整数据（含 metric_stats、render_targets） |
| `get_labels(db, snapshot_id)` | api_id → label 字典 |
| `get_metrics(db, snapshot_id)` | api_id → metrics 字典 |

---

## 分析服务（analysis/）

Python Pipeline 执行顺序（在 C# 写完 JSON 后运行）：

```
label_service → status_service → topdc_service → analysis_md_service → dashboard_service
```

各步骤非致命：单步失败不影响已完成的步骤。

| 服务 | 输入 | 输出 |
|---|---|---|
| `label_service` | `dc.json` + `shaders.json` | `label.json` + 写 DB `labels` 表 |
| `status_service` | `dc.json` + `label.json` + `metrics.json` | `status.json` + 写 DB |
| `topdc_service` | `status.json` + `attribution_rules.json` | `topdc.json` |
| `analysis_md_service` | `topdc.json` | `analysis.md`（LLM 或规则回退） |
| `dashboard_service` | `topdc.json` + `status.json` | `dashboard.md` |

### 服务端 Pipeline Job（jobs.py）

`POST /api/data/pipeline?snapshot_dir=...` 在后台线程运行完整 pipeline，返回 `job_id` 供轮询。Job 状态持久化在内存，浏览器刷新后可通过 `localStorage` 恢复轮询。

---

## /api/data/* 端点速查

| Method | Path | 说明 |
|---|---|---|
| `POST` | `/ingest` | 导入 snapshot_dir 到 DuckDB |
| `POST` | `/pipeline` | 启动 Python 分析 Pipeline Job |
| `GET` | `/pipeline/{job_id}` | 查询 Pipeline Job 状态 |
| `GET` | `/snapshots` | 列出所有已导入 snapshot（含截图） |
| `GET` | `/draw_calls?snapshot_id=N` | 查询 DC 列表（支持 category / tags 过滤） |
| `GET` | `/dc/{api_id}?snapshot_id=N` | 查询单个 DC 完整详情 |
| `GET` | `/label_agg?snapshot_id=N&metric=clocks&agg=sum` | 按 category 聚合 |
| `GET` | `/clock_correlation?snapshot_id=N` | Clocks 与各指标的 Pearson r |
| `GET` | `/questions` | 列出所有 Questions |
| `POST` | `/questions` | 创建 Question |
| `POST` | `/questions/{id}/run?snapshot_id=N` | 运行 Question |

---

## /api/files/* 端点速查

| Method | Path | 说明 |
|---|---|---|
| `GET` | `/list?dir=...` | 列出目录文件（按类型分组） |
| `GET` | `/read?path=...` | 读取文件内容（JSON / Markdown / 文本） |
| `GET` | `/raw?path=...` | 原始字节（Three.js OBJLoader 使用） |
| `GET` | `/raw?path=...&download=1` | 触发浏览器下载（加 Content-Disposition） |
| `GET` | `/image?path=...` | 图片文件（PNG / JPG / BMP） |
| `POST` | `/label?snapshot_dir=...` | 重新运行 label + status 服务 |

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

所有方法同步阻塞，内部轮询 Job 直到完成或超时。

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

- **DuckDB 连接**：`WorkspaceDB` 是进程内单例；所有查询使用 `db.cursor()`（独立游标），避免共享连接游标冲突
- **Render Targets**：不存 DuckDB，运行时从 `dc.json` 读取
- **Screenshot**：优先从分析目录缓存读取，fallback 从 `.sdp` ZIP 内 `snapshot_N/*.bmp` 提取
- **Ingest 幂等**：重复 ingest 同一 snapshot_dir 会先删除旧数据再写入；label 行按 valid_api_ids 过滤防止外键冲突
- **WebUI 仅本地**：`server.py` 绑定 `0.0.0.0`（开发用），生产建议改为 `127.0.0.1`
