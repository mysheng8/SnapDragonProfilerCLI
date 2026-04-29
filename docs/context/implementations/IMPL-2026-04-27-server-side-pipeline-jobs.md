---
type: implementation
date: 2026-04-27
title: Server-Side Python Analysis Pipeline Job Manager
status: complete
files_changed:
  - pySdp/webui/jobs.py  (new)
  - pySdp/webui/routes/data.py
  - pySdp/webui/static/app.js
---

# Server-Side Python Analysis Pipeline Job Manager

## Problem

Python 分析步骤（ingest → label → status → topdc → analysis_md → dashboard）由浏览器在 `_runPySteps()` 里串行 fetch，刷新页面会中断所有 Python 步骤，导致分析报告缺失。

## What Was Built

### `pySdp/webui/jobs.py` (新文件)

- `PipelineJob` dataclass：id / status / phase / progress / error / result / cancel event
  - status: `pending | running | completed | failed | cancelled`
- `PipelineJobManager`：
  - `submit(snapshot_dir, steps, db)` → 启动后台 `threading.Thread`，立即返回 job
  - `get(job_id)` / `cancel(job_id)` / `purge_expired()` (TTL 1h)
  - `_run()` 按 step 顺序执行，单步失败记录 error 但继续后续步骤（non-fatal）
- `_run_step()` 按 key 分发到各 analysis service（ingest/label/status/topdc/analysis/dashboard）
- 每步进度权重：screenshot 5 / ingest 20 / label 15 / status 15 / topdc 15 / analysis 20 / dashboard 10
- `_copy_screenshot(snapshot_dir)`: 从 `project/sdp/<run>/snapshot_N/` 复制截图到 `project/analysis/<run>/snapshot_N/`（路径转换：把 path 里最后一个 `analysis` 段替换为 `sdp`）
- 模块末尾导出单例 `pipeline_manager`

### `routes/data.py`

新增三个端点：
- `POST /api/data/pipeline?snapshot_dir=&targets=` → 提交 pipeline，返回 `{ok, job_id, steps}`
- `GET  /api/data/pipeline/{job_id}` → 轮询 `{status, phase, progress, error, result}`
- `POST /api/data/pipeline/{job_id}/cancel` → 取消

### `app.js`

- `_runPySteps()` 改为：提交 pipeline → 轮询
- `_pollPipelineJob()` 新函数：把 pipeline 0-100 进度映射到整体 70-100% 进度条
- `cancelAnalysis()` 同时取消 C# job 和 Python pipeline job
- `_resumePipelineJobIfAny()` 页面加载时检查 localStorage，恢复未完成的 pipeline 轮询
- 移除旧的 `PY_STEPS` 数组

## Data Flow

```
Before: Browser → fetch label → fetch status → fetch topdc → ...  (刷新即断)
After:  Browser → POST /pipeline → job_id → poll every 2s
        Server thread: ingest → label → status → topdc → analysis → dashboard
```

## Key Design Decision

镜像 C# JobManager 模式，使用标准库 `threading.Thread` + `threading.Lock`，不引入新依赖。
原有单步端点（/label、/status 等）保留，供手动调用。
