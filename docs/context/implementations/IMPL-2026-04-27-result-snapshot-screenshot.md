---
type: implementation
date: 2026-04-27
title: Results Tab Snapshot Screenshot Display + sdp Fallback
status: complete
files_changed:
  - pySdp/webui/routes/files.py
  - pySdp/webui/static/app.js
  - pySdp/webui/static/style.css
---

# Results Tab Snapshot Screenshot Display

## Problem

Results tab 切换不同 snapshot 时无法直观判断是哪个画面，用户不知道每个 snapshot 对应哪个镜头。

另一个问题：截图只存在于 `project/sdp/<run>/snapshot_N/`，analysis 输出目录 `project/analysis/<run>/snapshot_N/` 里没有截图文件，导致截图找不到。

## What Was Built

### `routes/files.py`

- 新增 `_find_in_dir()` 辅助函数：按优先级查找截图（`snapshot.png` → `snapshot_screenshot.png/jpg` → 第一个 `.bmp`）
- 新增 `find_screenshot(snap_dir)` 函数：
  1. 先在 analysis snapshot 目录本身找
  2. Fallback：把路径中的 `analysis` 段替换为 `sdp`，去对应 sdp session 目录找截图（`project/analysis/<run>/snapshot_N` → `project/sdp/<run>/snapshot_N`）
- `list_analyses` 返回的每个 snapshot 对象新增 `screenshot` 字段（绝对路径或 `null`）
- `classify_file()` 将截图文件（stem 含 `screenshot` 或 `snapshot` 的 png/jpg/bmp）标记为 `skip`，不在 Raw 区重复显示
- 新增 `GET /api/files/image?path=` 端点：直接从本地文件系统提供图片，正确设置 `Content-Type`（png/jpg/bmp）

### `app.js`

- `buildSnapPanel()` 顶部插入截图块：若 `snap.screenshot` 非空，渲染 `<img>` 指向 `/api/files/image?path=...`，加载失败自动隐藏

### `style.css`

- 新增 `.snap-screenshot` / `.snap-screenshot-img`：最大高度 240px，黑色背景，圆角边框，`object-fit: contain` 保持比例

## Key Design Decision

截图不复制到 analysis 目录，而是在查询时动态从 sdp 目录读取。路径推导完全对称，无需配置。
