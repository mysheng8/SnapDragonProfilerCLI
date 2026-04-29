---
type: plan
topic: Python project restructure + persistent data layer — DuckDB global DB, layered architecture
status: proposed
based_on:
  - FINDING-2026-04-21-python-data-layer-design.md
  - FINDING-2026-04-15-raw-data-schema.md
  - IMPL-2026-04-20-python-analysis-services-webui.md
related_paths:
  - pySdp/pysdp/                       (KEEP — C# client SDK)
  - pySdp/data/                        (NEW — data layer)
  - pySdp/analysis/                    (NEW — moved from webui/analysis/)
  - pySdp/webui/                       (THINNED — presentation only)
tags:
  - python
  - data-layer
  - duckdb
  - architecture
  - restructure
  - drawcall-analysis
  - persistence
  - fts
  - vector
  - label
  - metrics
  - dashboard
last_updated: 2026-04-21
---

# PLAN-2026-04-21: Python Project Restructure + Persistent Data Layer

## Architecture Decision (settled)

### 分层原则

```
C# SDPCLI                     硬件边界：SDK P/Invoke、raw JSON 导出、HTTP Server
      ↑ HTTP
pySdp/
  pysdp/      Layer 1: C# 客户端封装（SdpClient — 已稳定，不动）
  data/       Layer 2: 数据层（DuckDB — 本 Plan 新建）
  analysis/   Layer 3: 分析层（从 webui/analysis/ 迁出）
  webui/      Layer 4: 表现层（变薄 — 只做 HTTP 参数解析）
```

### 依赖方向（严格单向）

```
webui  →  analysis  →  data  →  DuckDB
webui  →  data
pysdp  →  (C# HTTP，独立)
```

- `data/` 不知道 HTTP 和 FastAPI 存在
- `analysis/` 不知道 FastAPI 存在
- `webui/routes/` 不含任何业务逻辑，只做参数解析 + 调用 data/analysis
- `pysdp/` 与其他三层并列，不是上下级

### 技术选型（settled）

| 决策 | 结论 | 理由 |
|---|---|---|
| 数据库 | **DuckDB** 单一全局 DB | 列存储 OLAP；内置 FTS（BM25）；原生向量列；单文件零配置 |
| DB 路径 | `pySdp/data/sdp.db`（可配置） | 全局，不按 workspace 碎片化；snapshot_dir 作数据字段 |
| 向量检索 | DuckDB `FLOAT[]` + `array_cosine_similarity()` | 当前规模全表扫描够用；>50 万 DC 时加 LanceDB |
| Parquet | 仅按需导出，不作主存储 | DuckDB 本身列存储；Parquet 不支持 UPSERT/FTS/向量 |
| 依赖 | `pip install duckdb` | 唯一新增依赖 |

---

## 目标目录结构

```
pySdp/
  pysdp/                    # Layer 1 — C# client SDK（不动）
    __init__.py
    client.py               # SdpClient
    _jobs.py
    _models.py
    exceptions.py

  data/                     # Layer 2 — 数据层（全部新建）
    __init__.py
    db.py                   # WorkspaceDB：连接管理、schema 创建
    ingest.py               # ingest_snapshot(snapshot_dir) → DB
    query.py                # 读 API：get_draw_calls / query_dcs / ...
    model_registry.py       # 分析模型注册表（Phase 6）

  analysis/                 # Layer 3 — 分析层（从 webui/analysis/ 迁移）
    __init__.py
    label_service.py        # MOVE from webui/analysis/
    status_service.py       # MOVE
    topdc_service.py        # MOVE
    dashboard_service.py    # MOVE
    analysis_md_service.py  # MOVE
    models/                 # 分析模型实现（Phase 6）
      __init__.py
      category_breakdown.py
      top_bottleneck_dcs.py
      texture_hotspots.py

  webui/                    # Layer 4 — 表现层（变薄）
    server.py               # FastAPI app + router 注册
    logger.py               # 不动
    routes/
      proxy.py              # 不动（C# HTTP 代理）
      files.py              # THINNED：去掉业务逻辑，调用 analysis/
      data.py               # NEW：/api/data/* endpoints
      logs.py               # 不动
    static/                 # 不动

  examples/                 # 不动
  requirements.txt          # 增加 duckdb
  webui.bat / webui.ps1     # 不动
```

---

## Phase 0 — 项目重组（迁移，不改功能）

**Goal**: 把 `webui/analysis/` 迁移到顶层 `analysis/`，修正 import 路径，验证服务不 break。

### 操作

1. 新建 `pySdp/analysis/__init__.py`
2. 移动 5 个服务文件：`webui/analysis/*.py` → `analysis/`
3. 更新 `webui/routes/files.py` 中的 import：
   ```python
   # 旧
   from analysis.label_service import generate_label_json
   # 新
   from analysis.label_service import generate_label_json   # 路径不变，因为 webui/ 运行时 sys.path 需调整
   ```
4. `webui/server.py` 在 `sys.path.insert` 时加入 `pySdp/` 根目录，使 `import analysis` 能解析到顶层

### 修改文件
- `pySdp/analysis/` — NEW（5 个文件移入）
- `pySdp/webui/server.py` — sys.path 加 `pySdp/` 根
- `pySdp/webui/routes/files.py` — import 路径更新
- `pySdp/webui/analysis/` — 删除（迁移完成后）

### Validation
- `python webui/server.py` 启动无 ImportError
- POST `/api/files/label?snapshot_dir=...` 返回 200

---

## Phase 1 — Schema + Ingestion

**Goal**: 建立 DuckDB 全局 DB，ingest 单个 snapshot 目录的所有 C# JSON 输出。

### 新文件

- `pySdp/data/__init__.py`
- `pySdp/data/db.py` — `WorkspaceDB`
- `pySdp/data/ingest.py` — `ingest_snapshot(snapshot_dir)`
- `pySdp/webui/routes/data.py` — `/api/data/*` endpoints

### `WorkspaceDB` (data/db.py)

```python
class WorkspaceDB:
    """全局单例，管理 DuckDB 连接。"""
    def __init__(self, db_path: Path): ...
    def conn(self) -> duckdb.DuckDBPyConnection: ...
    def ensure_schema(self): ...
    def close(self): ...
```

DB 路径优先级：
1. 环境变量 `SDP_DB_PATH`
2. 启动参数 `--db /path/to/sdp.db`
3. 默认：`{pySdp_root}/data/sdp.db`

### Schema

```sql
-- Core（C# ingest 后不变）

CREATE TABLE IF NOT EXISTS snapshots (
    snapshot_id   INTEGER PRIMARY KEY,
    sdp_name      TEXT        NOT NULL,
    run_name      TEXT        NOT NULL,
    snapshot_dir  TEXT        NOT NULL,
    ingested_at   TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS draw_calls (
    snapshot_id   INTEGER NOT NULL REFERENCES snapshots(snapshot_id),
    api_id        INTEGER NOT NULL,
    dc_id         INTEGER NOT NULL,
    api_name      TEXT    NOT NULL DEFAULT '',
    pipeline_id   INTEGER,
    vertex_count  INTEGER NOT NULL DEFAULT 0,
    index_count   INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (snapshot_id, api_id)
);

CREATE TABLE IF NOT EXISTS shader_stages (
    snapshot_id   INTEGER NOT NULL REFERENCES snapshots(snapshot_id),
    pipeline_id   INTEGER NOT NULL,
    stage         TEXT    NOT NULL,
    module_id     INTEGER,
    entry_point   TEXT    NOT NULL DEFAULT '',
    file_path     TEXT    NOT NULL DEFAULT '',
    PRIMARY KEY (snapshot_id, pipeline_id, stage)
);

CREATE TABLE IF NOT EXISTS dc_shader_stages (
    snapshot_id   INTEGER NOT NULL,
    api_id        INTEGER NOT NULL,
    pipeline_id   INTEGER NOT NULL,
    stage         TEXT    NOT NULL,
    PRIMARY KEY (snapshot_id, api_id, pipeline_id, stage),
    FOREIGN KEY (snapshot_id, api_id) REFERENCES draw_calls(snapshot_id, api_id)
);

CREATE TABLE IF NOT EXISTS textures (
    snapshot_id   INTEGER NOT NULL REFERENCES snapshots(snapshot_id),
    texture_id    INTEGER NOT NULL,
    width INTEGER, height INTEGER, depth INTEGER,
    format TEXT, layers INTEGER, levels INTEGER,
    file_path TEXT NOT NULL DEFAULT '',
    PRIMARY KEY (snapshot_id, texture_id)
);

CREATE TABLE IF NOT EXISTS dc_textures (
    snapshot_id INTEGER NOT NULL,
    api_id      INTEGER NOT NULL,
    texture_id  INTEGER NOT NULL,
    PRIMARY KEY (snapshot_id, api_id, texture_id),
    FOREIGN KEY (snapshot_id, api_id) REFERENCES draw_calls(snapshot_id, api_id)
);

CREATE TABLE IF NOT EXISTS meshes (
    snapshot_id INTEGER NOT NULL,
    api_id      INTEGER NOT NULL,
    mesh_file   TEXT    NOT NULL DEFAULT '',
    PRIMARY KEY (snapshot_id, api_id),
    FOREIGN KEY (snapshot_id, api_id) REFERENCES draw_calls(snapshot_id, api_id)
);

-- Derived（Python 写入，可刷新）

CREATE TABLE IF NOT EXISTS labels (
    snapshot_id     INTEGER     NOT NULL,
    api_id          INTEGER     NOT NULL,
    category        TEXT        NOT NULL DEFAULT '',
    subcategory     TEXT        NOT NULL DEFAULT '',
    detail          TEXT        NOT NULL DEFAULT '',
    reason_tags     TEXT        NOT NULL DEFAULT '[]',  -- JSON array
    confidence      DOUBLE      NOT NULL DEFAULT 0.0,
    label_source    TEXT        NOT NULL DEFAULT 'rule',
    bottleneck_text TEXT,                               -- LLM 写入（Phase 4）
    embedding       FLOAT[],                            -- 向量（Phase 5）
    labeled_at      TIMESTAMPTZ NOT NULL,
    PRIMARY KEY (snapshot_id, api_id),
    FOREIGN KEY (snapshot_id, api_id) REFERENCES draw_calls(snapshot_id, api_id)
);

CREATE TABLE IF NOT EXISTS metrics (
    snapshot_id           INTEGER NOT NULL,
    api_id                INTEGER NOT NULL,
    clocks                BIGINT,
    read_total_bytes      BIGINT,
    write_total_bytes     BIGINT,
    fragments_shaded      BIGINT,
    vertices_shaded       BIGINT,
    shaders_busy_pct      DOUBLE,
    shaders_stalled_pct   DOUBLE,
    tex_fetch_stall_pct   DOUBLE,
    tex_l1_miss_pct       DOUBLE,
    tex_l2_miss_pct       DOUBLE,
    tex_pipes_busy_pct    DOUBLE,
    time_alus_working_pct DOUBLE,
    -- 余下 ~40 列 ingest 时动态 ADD COLUMN IF NOT EXISTS
    PRIMARY KEY (snapshot_id, api_id),
    FOREIGN KEY (snapshot_id, api_id) REFERENCES draw_calls(snapshot_id, api_id)
);

CREATE TABLE IF NOT EXISTS snapshot_stats (
    snapshot_id  INTEGER     NOT NULL REFERENCES snapshots(snapshot_id),
    category     TEXT        NOT NULL,
    dc_count     INTEGER     NOT NULL,
    clocks_sum   BIGINT      NOT NULL,
    clocks_pct   DOUBLE,
    avg_conf     DOUBLE,
    computed_at  TIMESTAMPTZ NOT NULL,
    PRIMARY KEY (snapshot_id, category)
);

CREATE TABLE IF NOT EXISTS questions (
    id           TEXT    PRIMARY KEY,
    title        TEXT    NOT NULL,
    model_name   TEXT    NOT NULL,
    model_params TEXT    NOT NULL DEFAULT '{}',
    viz_type     TEXT    NOT NULL,
    viz_config   TEXT    NOT NULL DEFAULT '{}',
    is_builtin   BOOLEAN NOT NULL DEFAULT false,
    created_at   TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS dashboards (
    id           TEXT    PRIMARY KEY,
    title        TEXT    NOT NULL,
    question_ids TEXT    NOT NULL DEFAULT '[]',
    created_at   TIMESTAMPTZ NOT NULL,
    updated_at   TIMESTAMPTZ NOT NULL
);
```

### `ingest_snapshot(snapshot_dir)` (data/ingest.py)

顺序（FK 依赖）：snapshots → draw_calls → shader_stages → dc_shader_stages → textures → dc_textures → meshes → metrics → labels（可选）。全部 INSERT OR REPLACE，单事务。

### New API endpoints (webui/routes/data.py)

```
POST /api/data/ingest?snapshot_dir=<path>
     → { ok, snapshot_id, counts: {draw_calls, shaders, textures, meshes, metrics, labels} }

GET  /api/data/snapshots
     → { ok, data: [{snapshot_id, sdp_name, run_name, ingested_at}] }
```

### Modified
- `pySdp/webui/server.py`: 注册 data_router，传入 db 实例
- `pySdp/requirements.txt`: 加 `duckdb`

### Validation
- ingest 返回 draw_calls count = dc.json total_dc_count
- 第二次 ingest 同一目录幂等（row count 不变）

---

## Phase 2 — Query Layer

**Goal**: typed read API，analysis/ 层可用 DB 替代重读 JSON。

### data/query.py

```python
def get_draw_calls(db, snapshot_id, *, category=None, tags=None) -> list[dict]: ...
def get_labels(db, snapshot_id) -> dict[int, dict]: ...      # keyed by api_id
def get_metrics(db, snapshot_id) -> dict[int, dict]: ...     # keyed by api_id
def get_dc_detail(db, snapshot_id, api_id) -> dict: ...      # DC + label + metrics + shaders + textures + mesh
def query_dcs(db, snapshot_id, *, category=None, min_clocks=None,
              label_source=None, tags=None) -> list[dict]: ...
```

### New endpoints

```
GET /api/data/draw_calls?snapshot_id=<n>[&category=<c>][&tags=shadow,depth]
GET /api/data/dc/<api_id>?snapshot_id=<n>
```

---

## Phase 3 — Label + Stats Persistence

**Goal**: label 写 DB，stats 持久化，支持刷新，不再每次重算。

`analysis/label_service.py` 在写 label.json 后同步写入 labels 表。  
`analysis/status_service.py` 优先从 DB 读（query layer），结果同步写 snapshot_stats。

### New endpoint

```
POST /api/data/refresh_labels?snapshot_id=<n>
     → 重跑 label_service + 更新 labels + 重算 snapshot_stats
```

---

## Phase 4 — LLM Bottleneck + FTS

**Goal**: LLM 写入的 bottleneck 文本持久化 + BM25 全文检索。

bottleneck_text 列已在 Phase 1 schema 中预留。FTS 索引在首次写入后建立：

```sql
PRAGMA create_fts_index('labels', 'api_id', 'bottleneck_text', stemmer='english');
```

### New endpoints

```
POST /api/data/dc/<api_id>/bottleneck?snapshot_id=<n>
     body: { text: "..." }

GET  /api/data/search?q=<text>[&snapshot_id=<n>]
     → BM25 排序结果 [{snapshot_id, api_id, score, category}]
```

---

## Phase 5 — 向量检索

**Goal**: embedding 存 DB，支持语义相似 DC 检索。

embedding 列（FLOAT[]）已在 Phase 1 schema 中预留。

```sql
-- 相似检索（全表扫描，<50 万行够用）
SELECT snapshot_id, api_id,
       array_cosine_similarity(embedding, ?) AS sim
FROM labels WHERE embedding IS NOT NULL
ORDER BY sim DESC LIMIT 10;
```

升级路径：>50 万 DC 时加 LanceDB，共享 `(snapshot_id, api_id)` 作 join key。

### New endpoints

```
POST /api/data/dc/<api_id>/embedding?snapshot_id=<n>
     body: { embedding: [float, ...] }

POST /api/data/similar?snapshot_id=<n>&api_id=<n>&top_k=10
```

---

## Phase 6 — Analysis Model Registry

**Goal**: 注册命名分析函数，从 DB 查询产生结构化输出，供问题引擎调用。

- `data/model_registry.py`：`register()` / `list_models()` / `run_model(name, db, snapshot_id, **params)`
- `analysis/models/`：每个模型一个文件，实现 `AnalysisModel` 接口

内置模型：`category_breakdown`、`top_bottleneck_dcs`、`texture_hotspots`、`bottleneck_search`

```
GET  /api/data/models
POST /api/data/models/<name>/run?snapshot_id=<n>
```

---

## Phase 7 — Question Engine + Dashboard

**Goal**: 命名问题绑定模型 + 可视化类型；Dashboard 是问题列表。Schema 已在 Phase 1 中创建。

```
GET/POST       /api/data/questions
PUT/DELETE     /api/data/questions/<id>
POST           /api/data/questions/<id>/run?snapshot_id=<n>

GET/POST       /api/data/dashboards
PUT/DELETE     /api/data/dashboards/<id>
POST           /api/data/dashboards/<id>/run?snapshot_id=<n>
```

---

## 导出（Parquet，按需）

```python
duckdb.sql("COPY draw_calls TO 'export.parquet' (FORMAT PARQUET)")
```

未来可加 `GET /api/data/export?table=draw_calls&format=parquet`。

---

## Implementation Order

| Phase | 内容 | 优先级 | 前置 |
|---|---|---|---|
| 0 | 项目重组（analysis 迁移） | **先做** | — |
| 1 | Schema + Ingest | Critical | 0 |
| 2 | Query Layer | High | 1 |
| 3 | Label/Stats Persist | High | 1+2 |
| 4 | LLM Bottleneck + FTS | Medium | 3 |
| 5 | 向量检索 | Medium | 4 |
| 6 | Analysis Model Registry | Medium | 2 |
| 7 | Question Engine + Dashboard | Low | 6 |

---

## File Map Summary

| File | 操作 | Phase |
|---|---|---|
| pySdp/analysis/__init__.py | NEW | 0 |
| pySdp/analysis/label_service.py | MOVE from webui/analysis/ | 0 |
| pySdp/analysis/status_service.py | MOVE | 0 |
| pySdp/analysis/topdc_service.py | MOVE | 0 |
| pySdp/analysis/dashboard_service.py | MOVE | 0 |
| pySdp/analysis/analysis_md_service.py | MOVE | 0 |
| pySdp/webui/analysis/ | DELETE（迁移后） | 0 |
| pySdp/webui/routes/files.py | MODIFIED（import 路径） | 0 |
| pySdp/webui/server.py | MODIFIED（sys.path + db 注入） | 0+1 |
| pySdp/data/__init__.py | NEW | 1 |
| pySdp/data/db.py | NEW | 1 |
| pySdp/data/ingest.py | NEW | 1 |
| pySdp/webui/routes/data.py | NEW | 1 |
| pySdp/requirements.txt | MODIFIED（+ duckdb） | 1 |
| pySdp/data/query.py | NEW | 2 |
| pySdp/data/model_registry.py | NEW | 6 |
| pySdp/analysis/models/ | NEW dir | 6 |
