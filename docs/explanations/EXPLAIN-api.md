# pySdp WebUI — API 端点文档

完整交互式文档见 Swagger UI：**http://localhost:8000/api/docs**

本文档按路由组说明各端点的用途、参数和返回结构。

---

## 路由组概览

| 前缀 | Tag | 调用方 | 说明 |
|---|---|---|---|
| `/api/sdpcli/*` | `frontend` | 浏览器 SPA | 透传到 SDPCLI Server，不做文档化 |
| `/api/snapshot/*` | `snapshot` | 外部/CI/MCP | 设备发现、快照工作流（typed + OpenAPI docs） |
| `/api/jobs/*` | `jobs` | 外部/CI | 触发 C# 提取 + Python 分析步骤 |
| `/api/files/*` | `files` | 外部/MCP | 只读文件浏览与服务 |
| `/api/data/*` | `data` | 外部/MCP | DuckDB 数据查询（`[MCP]` 标注的端点已暴露给 MCP） |
| `/api/logs/*` | `logs` | 浏览器 SPA | WebUI 日志流 |

---

## /api/snapshot — 快照工作流

需要 SDPCLI Server 运行中。所有异步命令返回 `{ ok, data: { jobId } }`，通过 `GET /api/snapshot/jobs/{job_id}` 轮询。

### 设备发现

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/snapshot/devices` | — | 列出 ADB 连接设备，返回 `[{ serial, state }]` |
| `GET` | `/api/snapshot/app/packages` | `serial?` | 列出设备已安装三方包名 |
| `GET` | `/api/snapshot/app/activities` | `package`, `serial?` | 列出指定包的 Activity |
| `GET` | `/api/snapshot/device` | — | 当前 SDPCLI session 状态 |

### 快照命令

| Method | Path | Body | 说明 |
|---|---|---|---|
| `POST` | `/api/snapshot/connect` | `{ deviceId? }` | 连接 ADB 设备，返回 jobId（202） |
| `POST` | `/api/snapshot/disconnect` | — | 断开连接（同步） |
| `POST` | `/api/snapshot/launch` | `{ packageActivity? }` | 启动 App，格式 `com.example/.MainActivity`，返回 jobId |
| `POST` | `/api/snapshot/capture` | `{ outputDir?, label? }` | 触发帧捕获，返回 jobId |

### Job 轮询

| Method | Path | 说明 |
|---|---|---|
| `GET` | `/api/snapshot/jobs/{job_id}` | 轮询 connect/launch/capture job 状态 |
| `POST` | `/api/snapshot/jobs/{job_id}/cancel` | 取消 job |

**Job 状态字段：**
```json
{
  "ok": true,
  "data": {
    "id": "abc123",
    "status": "running",   // pending | running | completed | failed | cancelled
    "phase": "connecting",
    "progress": 40,
    "result": { ... }
  }
}
```

---

## /api/jobs — 分析触发

### C# 提取

| Method | Path | Body | 说明 |
|---|---|---|---|
| `POST` | `/api/jobs/reply_extract` | `{ sdpPath, snapshotId, targets?, outputDir? }` | 提交 C# 提取 job，返回 C# jobId（轮询用 `GET /api/sdpcli/jobs/{id}`） |

`targets` 默认 `dc,shaders,textures,buffers,metrics`（所有 C# 提取步骤）。

### Python 单步（同步）

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `POST` | `/api/jobs/ingest` | `snapshot_dir` | 将 snapshot 导入 DuckDB（幂等） |
| `POST` | `/api/jobs/screenshot` | `snapshot_dir` | 从 `.sdp` 提取截图到 snapshot 目录 |
| `POST` | `/api/jobs/mesh_stats` | `snapshot_dir` | 解析 mesh OBJ → `meshes.json`，然后 re-ingest |
| `POST` | `/api/jobs/texture_stats` | `snapshot_dir` | 读取贴图尺寸 → `textures.json`，然后 re-ingest |
| `POST` | `/api/jobs/label` | `snapshot_dir` | 规则分类 → `label.json`，写 DB |
| `POST` | `/api/jobs/scene_describe` | `snapshot_dir` | VLM 场景描述 → `scene_description.md`，写 DB |
| `POST` | `/api/jobs/report` | `snapshot_dir` | status → topdc → analysis_md 串行执行 |

**步骤依赖关系：**
```
reply_extract (C#)
  → ingest
  → mesh_stats   (需要 meshes/ 目录存在)
  → texture_stats (需要 textures/ 目录存在)
  → label        (需要 dc.json)
  → report       (需要 label.json + metrics.json)
    ├─ status
    ├─ topdc      (需要 status.json)
    └─ analysis_md (需要 status.json)
  → scene_describe (需要 screenshot)
```

### Python Pipeline（异步）

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `POST` | `/api/jobs/pipeline` | `snapshot_dir`, `targets?` | 后台线程运行多步，返回 `job_id` |
| `GET` | `/api/jobs/pipeline/{job_id}` | — | 轮询 pipeline job 状态和进度 |
| `POST` | `/api/jobs/pipeline/{job_id}/cancel` | — | 请求取消 |

`targets` 默认全部步骤：`screenshot,mesh_stats,texture_stats,ingest,label,status,topdc,analysis,describe`

**Pipeline Job 返回：**
```json
{
  "ok": true,
  "data": {
    "job_id": "py-abc123",
    "status": "running",
    "phase": "label_drawcalls",
    "progress": 45,
    "error": null,
    "result": {
      "ingest": { "snapshot_id": 3, "counts": { ... } },
      "label":  { "path": "/.../label.json" }
    }
  }
}
```

---

## /api/files — 文件服务（只读）

### 目录浏览

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/files/sdp` | `dir` | 递归列出目录下所有 `.sdp` 文件（按修改时间排序） |
| `GET` | `/api/files/results` | `dir` | 列出分析结果目录文件（按扩展名分组排序） |
| `GET` | `/api/files/analyses` | `root` | 递归列出所有 analysis run → snapshot → 文件（含截图路径） |

### 文件读取（MCP 暴露）

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/files/read` | `path`, `lines?` | 读取文本文件（HLSL / JSON / Markdown），返回内容字符串 |
| `GET` | `/api/files/raw` | `path`, `download?` | 原始字节流（OBJ 文件等），`download=1` 触发下载 |
| `GET` | `/api/files/image` | `path` | 图片文件（PNG / JPG / BMP），设置正确 Content-Type |

---

## /api/data — DuckDB 查询

> `[MCP]` 标注的端点已通过 `fastapi-mcp` 暴露给 MCP 客户端（如 Claude Desktop）。

### Snapshot

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/snapshots` **[MCP]** | — | 列出所有已导入 snapshot（含截图路径、snap_index） |
| `POST` | `/api/data/refresh_labels` | `snapshot_id` | 对已导入 snapshot 重新运行 label + status |

### Draw Calls

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/draw_calls` **[MCP]** | `snapshot_id`, `category?`, `tags?` | 查询 DC 列表（含 label + 关键指标） |
| `GET` | `/api/data/dc/{api_id}` **[MCP]** | `snapshot_id` | 查询单个 DC 完整详情（metrics、shaders、textures、mesh、render_targets） |

### 指标分析

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/available_metrics` **[MCP]** | `snapshot_id` | 列出有数据的 GPU counter 列名 |
| `GET` | `/api/data/label_agg` **[MCP]** | `snapshot_id`, `metric?`, `agg?` | 单指标按 category 聚合（sum/avg/median/max/min/variance） |
| `GET` | `/api/data/label_agg_all` **[MCP]** | `snapshot_id`, `agg?` | 所有指标按 category 聚合 |
| `GET` | `/api/data/label_agg_multi` **[MCP]** | `snapshot_id` | 所有指标 × 5种聚合函数，一次返回 |
| `GET` | `/api/data/label_metrics` **[MCP]** | `snapshot_id` | 关键指标（clocks、shaders_busy、tex_miss 等）按 category 汇总 |
| `GET` | `/api/data/label_correlations` **[MCP]** | `snapshot_id`, `metric?` | 各 category 内 clocks 与指定指标的 Pearson r |
| `GET` | `/api/data/clock_correlation` **[MCP]** | `snapshot_id`, `category?` | clocks 与所有指标的 Pearson r，按 \|r\| 排序 |

### 分析模型

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/models` **[MCP]** | — | 列出所有注册的分析模型 |
| `POST` | `/api/data/models/{name}/run` | `snapshot_id`, body? | 运行指定模型 |

### Questions

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/questions` **[MCP]** | — | 列出所有 Questions |
| `POST` | `/api/data/questions` | body | 创建 Question |
| `GET` | `/api/data/questions/{id}` **[MCP]** | — | 获取单个 Question |
| `PUT` | `/api/data/questions/{id}` | body | 更新 Question |
| `DELETE` | `/api/data/questions/{id}` | — | 删除 Question |
| `POST` | `/api/data/questions/{id}/run` | `snapshot_id` | 运行 Question |

### Dashboards

| Method | Path | 参数 | 说明 |
|---|---|---|---|
| `GET` | `/api/data/dashboards` **[MCP]** | — | 列出所有 Dashboards |
| `POST` | `/api/data/dashboards` | body | 创建 Dashboard |
| `GET` | `/api/data/dashboards/{id}` **[MCP]** | — | 获取单个 Dashboard |
| `PUT` | `/api/data/dashboards/{id}` | body | 更新 Dashboard |
| `DELETE` | `/api/data/dashboards/{id}` | — | 删除 Dashboard |
| `POST` | `/api/data/dashboards/{id}/run` | `snapshot_id` | 运行 Dashboard 所有 panels |

---

## MCP 集成

通过 `fastapi-mcp` 在 `/mcp` 挂载，暴露 19 个只读查询工具：

```
snapshots, draw_calls, dc_detail,
available_metrics, label_agg, label_agg_all, label_agg_multi, label_metrics,
label_correlations, clock_correlation,
models, questions, question, dashboards, dashboard,
file_read, file_raw, file_image
```

**Claude Desktop 配置示例（`claude_desktop_config.json`）：**
```json
{
  "mcpServers": {
    "pySdp": {
      "url": "http://localhost:8000/mcp"
    }
  }
}
```

---

## 通用约定

**成功响应：**
```json
{ "ok": true, "data": { ... } }
```

**错误响应：**
```json
{ "ok": false, "error": "描述信息" }
```

**snapshot_id 说明：**
- `snapshot_id`：DuckDB 全局唯一主键（ingest 时分配）
- `snap_index`：C# 原始编号（同一 session 内从 1 开始，不同 SDP 可能重叠）
- 所有查询端点使用 `snapshot_id`，`snap_index` 仅供展示

**参数均为 Query String**（FastAPI 约定），POST body 为 JSON。
