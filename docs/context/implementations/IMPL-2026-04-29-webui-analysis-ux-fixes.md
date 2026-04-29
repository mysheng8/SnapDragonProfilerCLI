---
topic: WebUI analysis UX fixes — proxy timeouts, multi-snapshot pipeline, job persistence, explorer DC detail enhancements
status: completed
date: 2026-04-29
---

## Summary

Full-session fixes applied to the analysis flow, Explorer DC detail panel, and mesh viewer across `pySdp/webui/` and `pySdp/data/`.

---

## 1. Proxy timeout increase (`pySdp/webui/routes/proxy.py`)

**Problem**: Long-running routes (capture, analysis) were failing with "Failed to fetch" due to hardcoded 30s timeout.

**Fix**: Added `timeout` parameter to `_fwd()` (default 30s). Per-route overrides:

| Route | Timeout |
|---|---|
| `POST /connect` | 120s |
| `POST /disconnect` | 60s |
| `POST /session/launch` | 120s |
| `POST /capture` | 300s |
| `POST /analysis` | 60s |
| all GET polling | 30s |

---

## 2. Multi-snapshot Python pipeline (`app.js`)

**Problem**: `snapshotId=1` (all snapshots) → C# returns `{ captureIds, sessionDir }`, not `{ captureDir }`. JS errored "No captureDir in job result".

**Fix**: Detect both result shapes after C# job completes:
- Single: `result.captureDir` → `_runPySteps(captureDir)`
- Multi: `result.captureIds + result.sessionDir` → reconstruct `sessionDir/snapshot_N` for each id → `_runPyStepsAll(captureDirs)`

`_runPyStepsAll` runs the Python pipeline for each snapshot dir sequentially; per-snapshot failures are non-fatal.

---

## 3. Per-snapshot progress label (`AnalysisJobRunner.cs`)

**Problem**: `job.Phase` only showed phase name with no snapshot context.

**Fix**: When `captureIds.Count > 1`:
- Phase format: `[1/3] snapshot_2 / collect_dc`
- Progress: scaled per snapshot — snapshot `i` of `n` occupies the `[i/n … (i+1)/n]` band of 0–100

---

## 4. Job persistence across page refresh (`app.js`)

**Problem**: Refreshing the browser lost both the active C# analysis job and the Python pipeline job, requiring the user to restart the analysis from scratch.

**Fix — C# job (`activeCsJob`)**: On C# job submit, save `{ jobId, sdpPath, allSelected }` to `localStorage`. On page load, `_resumeCsJobIfAny()` checks localStorage; if job is still running, resumes polling; if job is already done, jumps directly to the Python pipeline phase.

**Fix — Python pipeline job (`activePipelineJob`)**: `_runPyStepsAll` saves `{ jobId, snapshotDir, pendingDirs, sdpPath, allSelected }` to `localStorage` per snapshot. On page load, `_resumePipelineJobIfAny()` resumes polling the in-progress job.

Both keys are cleared from localStorage on terminal completion or error.

---

## 5. Shared snapshot picker auto-refresh after analysis (`app.js`)

**Problem**: After analysis completed, the Questions/Explorer shared snapshot picker was not updated, requiring a manual Scan click.

**Fix**: `_finishAnalysis()` now calls `sharedSnapScan()` and auto-selects the newly ingested snapshot by matching `snapshot_dir` against the scan results.

**Fix**: Removed the empty-dir early return guard in `sharedSnapScan()` — DuckDB does not require a directory parameter.

---

## 6. ingest foreign key constraint fix (`pySdp/data/ingest.py`)

**Problem**: Re-ingesting a snapshot after re-capture caused a DuckDB foreign key violation because `label.json` from the previous run contained `api_id` values that no longer existed in the new `draw_calls`.

**Fix**: Filter `label_rows` against `valid_api_ids` (the set of api_ids just inserted into `draw_calls`) before INSERT:

```python
valid_api_ids = {row[1] for row in dc_rows}
label_rows = [r for r in label_rows if r[1] in valid_api_ids]
```

---

## 7. Screenshot extraction from `.sdp` archive (`pySdp/webui/routes/data.py`, `pySdp/webui/jobs.py`)

**Problem**: Screenshots were looked up in the snapshot's filesystem directory, which gets cleaned after archiving. The `.sdp` file (a ZIP) contains `snapshot_N/1_screenshot.bmp` but this was never extracted.

**Fix — `list_snapshots`**: `_find_screenshot()` now first looks in the analysis snapshot dir (already-extracted cache), then falls back to extracting `snapshot_N/*.bmp` directly from the `.sdp` ZIP using `zipfile`. Extracted file is cached to the analysis snapshot dir for future calls.

**Fix — `jobs.py` `_copy_screenshot`**: During pipeline execution, `_find_sdp_file()` locates the `.sdp` archive by reading `sdp_name` from `dc.json`, then `_copy_screenshot` extracts `snapshot_N/*.{bmp,png,jpg}` from the ZIP into the analysis dir.

---

## 8. DC detail panel — metric heatmap with median (`app.js`, `pySdp/data/query.py`, `style.css`)

**Problem**: Explorer DC detail showed raw metric values with no context — no indication of whether a value was high or low relative to the snapshot.

**Fix — backend (`query.py` `get_dc_detail`)**: Added `metric_stats` field to the response. Uses DuckDB `percentile_cont(0.5) WITHIN GROUP (ORDER BY col)` + `min` + `max` for every metric column in one query per snapshot.

**Fix — frontend (`app.js`)**: Each metric row is colored with a green→yellow→red heatmap:
```
t = (val - min) / (max - min)
r = t < 0.5 ? t*2*255 : 255
g = t < 0.5 ? 255 : (1-t)*2*255
background: rgba(r, g, 0, 0.18)
```
Median value shown as `med: xxx` in muted text below the metric value.

**Fix — CSS (`style.css`)**: Added `.dc-metric-row` padding and `.dc-metric-median` style.

---

## 9. DC detail panel — Render Targets section (`app.js`, `pySdp/data/query.py`)

**Problem**: Render targets are not stored in DuckDB — they live in `dc.json`. The `get_dc_detail` function had a `try/except: pass` block that silently swallowed any error when reading the snapshot dir from DB and then reading `dc.json`.

**Root cause**: `get_dc_detail` used `db.conn()` (shared connection) for all sequential queries. After the `metric_stats` `percentile_cont` query, the shared connection's cursor state interfered with subsequent `.execute()` calls, causing the `snapshot_dir` lookup to silently fail.

**Fix (`query.py`)**: Rewrote `get_dc_detail` to use `db.cursor()` (independent cursor per query) for every single query in the function. Changed `except Exception: pass` to `except Exception as e: dc["_rt_error"] = str(e)` to surface errors during debugging.

**Fix (`app.js`)**: Changed `_buildDetailSection(..., false)` (collapsed) to `_buildDetailSection(..., true)` (expanded by default) for the Render Targets section.

**Section order**: Textures → Mesh → Render Targets → Shaders

Render target item shows: `[index] type · WxH · format`. Expandable row with `resource_id`, `renderpass_id`, `framebuffer_id`, `width`, `height`, `format`, `attachment_type`.

---

## 10. Mesh viewer — wireframe toggle + vertex/triangle stats (`app.js`, `style.css`)

**Problem**: Complex meshes were hard to inspect in solid shading mode. No geometry statistics were shown.

**Fix**: Added toolbar overlay to the mesh viewer:
- **Wireframe button**: toggles `material.wireframe` on all meshes in the scene; button highlights when active.
- **Stats**: after OBJ load, traverses all `Mesh` children, sums `position.count` (vertices) and computes triangle count from index buffer or position count. Displayed as `Verts: N · Tris: M`.

**CSS**: Added `.mesh-viewer-toolbar` (absolute overlay, top-left), `.mesh-viewer-toolbar .btn-secondary.active` (accent highlight), `.mesh-viewer-stats` (muted white text).

---

## 11. Shader file download (`app.js`, `pySdp/webui/routes/files.py`)

**Problem**: Shader files (HLSL/SPIR-V) were viewable inline but not downloadable.

**Fix — backend (`files.py`)**: Added `download: int = Query(default=0)` parameter to `GET /api/files/raw`. When `download=1`, sets `Content-Disposition: attachment; filename="<name>"` header on the `FileResponse`.

**Fix — frontend (`app.js`)**: Each shader item in the DC detail panel now has a `⬇` anchor tag:
```js
dlBtn.href = `${FILES}/raw?path=${encodeURIComponent(fp)}&download=1`;
dlBtn.download = name;
dlBtn.onclick = e => e.stopPropagation(); // prevent expand toggle
```
Appended to `.dc-sublist-label` element inside the item header row.

---

## Files Modified

| File | Change |
|---|---|
| `pySdp/webui/routes/proxy.py` | Per-route timeout overrides on `_fwd()` |
| `pySdp/webui/routes/data.py` | `_find_screenshot` ZIP extraction; `list_snapshots` passes `sdp_name` |
| `pySdp/webui/routes/files.py` | `GET /raw` `download` param → `Content-Disposition` header |
| `pySdp/webui/jobs.py` | `_find_sdp_file`, `_copy_screenshot` extracts from `.sdp` ZIP |
| `pySdp/data/ingest.py` | Filter stale label rows by `valid_api_ids` before INSERT |
| `pySdp/data/query.py` | `get_dc_detail`: all queries use `db.cursor()`; `metric_stats` via `percentile_cont`; `render_targets` from `dc.json` |
| `pySdp/webui/static/app.js` | Multi-snapshot pipeline, job persistence, snap picker auto-refresh, metric heatmap, RT section, mesh wireframe+stats, shader download |
| `pySdp/webui/static/style.css` | `.dc-metric-row`, `.dc-metric-median`, `.mesh-viewer-toolbar`, `.mesh-viewer-stats` |
| `pySdp/webui/static/index.html` | Cache bust version bump |
| `SDPCLI/source/Server/Jobs/AnalysisJobRunner.cs` | Per-snapshot phase label + scaled progress |
