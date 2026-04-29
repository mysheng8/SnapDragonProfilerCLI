---
type: implementation
topic: WebUI new tabs — Explorer (single DC query), Questions (label-aggregated metrics), analysis pipeline ingest trigger
status: completed
based_on:
  - PLAN-2026-04-22-webui-new-tabs.md
  - FINDING-2026-04-22-webui-new-tabs-plan.md
related_paths:
  - pySdp/webui/static/index.html        (MODIFIED)
  - pySdp/webui/static/app.js            (MODIFIED)
  - pySdp/webui/static/style.css         (MODIFIED)
  - pySdp/webui/routes/data.py           (MODIFIED — new label_metrics endpoint)
date: 2026-04-22
---

# Implementation: WebUI New Tabs — Explorer + Questions + Analysis Trigger

## Summary

Implemented all three phases of PLAN-2026-04-22-webui-new-tabs. Four files modified,
no new files created, existing tabs and functionality unchanged.

---

## Files Modified

### pySdp/webui/routes/data.py

Added `GET /api/data/label_metrics?snapshot_id=<n>` endpoint inside `make_router(db)`.

The endpoint executes a raw DuckDB SQL query joining `draw_calls + labels + metrics`
grouped by `COALESCE(lb.category, 'Unlabeled')`, returning per-category aggregates:
- `category`, `dc_count`
- `clocks_sum`, `clocks_avg`
- `shaders_busy_pct_avg`, `tex_fetch_stall_pct_avg`, `tex_l1_miss_pct_avg`, `tex_l2_miss_pct_avg`
- `fragments_shaded_sum`, `fragments_shaded_avg`
- `vertices_shaded_sum`, `vertices_shaded_avg`
- `read_bytes_sum`, `write_bytes_sum`

Float values rounded to 2 decimal places. NULL values left as None (rendered as `—` in frontend).
Ordered by `SUM(m.clocks) DESC NULLS LAST`.

### pySdp/webui/static/index.html

- Added two tab buttons between "Results" and "Logs": `Questions` and `Explorer`
- Final tab order: Snapshot | Analysis | Results | Questions | Explorer | Logs
- Added `<section id="tab-questions">` with snapshot selector + result card
- Added `<section id="tab-explorer">` with snapshot selector + DC list card + detail panel

### pySdp/webui/static/app.js

Phase C (analysis trigger):
- Added `const DATA = '/api/data'` constant
- Updated `ALL_TARGETS` to include `'ingest'` as first entry
- Updated `PY_STEPS` to include `{ key: 'ingest', endpoint: null, phase: 'ingest_db', isData: true }` as first step
- Updated `_runPySteps()` to branch on `step.isData`: ingest calls `POST /api/data/ingest?snapshot_dir=…` instead of `POST /api/files/<endpoint>?snapshot_dir=…`

Phase A (Explorer tab):
- Added `explorerState` object tracking `snapshotId`, `dcs`, `selectedApiId`
- Added `_loadSnapshotsIntoSelect(selectId)` — shared helper that populates a `<select>` from `/api/data/snapshots`
- Added `loadExplorerSnapshots()` — loads snapshots into `#explorer-snapshot-sel`, wires `onchange`
- Added `onExplorerSnapshotChange()` — reads selected snapshot_id, triggers DC load
- Added `loadExplorerDCs()` — calls `GET /api/data/draw_calls` with optional category filter, renders table
- Added `renderExplorerDCTable(dcs)` — renders compact clickable table with api_id/api_name/category/confidence/clocks columns; highlights active row
- Added `loadExplorerDCDetail(apiId)` — calls `GET /api/data/dc/{api_id}`, renders detail panel
- Added `renderExplorerDCDetail(container, dc)` — renders 6 collapsible sections (Base Info, Label, Metrics, Shader Stages, Textures, Mesh)
- Added `_buildDetailSection(title, defaultOpen)` — reusable collapsible section builder (returns `{el, body}`)
- Added `switchTab` handler for `explorer` id: calls `loadExplorerSnapshots()`

Phase B (Questions tab):
- Added `questionsState` object tracking `snapshotId`
- Added `loadQuestionsSnapshots()` — thin wrapper around `_loadSnapshotsIntoSelect`
- Added `loadQuestionsData()` — reads snapshot selector, calls `GET /api/data/label_metrics`, renders table
- Added `renderQuestionsTable(rows)` — renders scrollable questions table with 12 columns
- Added `switchTab` handler for `questions` id: calls `_loadSnapshotsIntoSelect('questions-snapshot-sel')`

### pySdp/webui/static/style.css

Added styles for:
- `#explorer-snapshot-sel`, `#questions-snapshot-sel` — select element styling matching existing inputs
- `.explorer-dc-table` — compact table for DC list and shader/texture sub-tables in detail panel
- `.dc-row` — clickable row style with hover and `.active` highlight
- `.metric-table` — two-column key/value table for base info, label, metrics
- `.questions-table-wrap` — overflow-x scroll wrapper for the wide aggregation table
- `.questions-table` — responsive table with right-aligned numeric columns, sticky header
- `.questions-category-cell` — left-aligned, bold category column override

---

## API Routes Added

| Route | Method | Location |
|---|---|---|
| `/api/data/label_metrics` | GET | `routes/data.py` |

Total routes in `routes/data.py`: 20 (was 19).

---

## Key Implementation Decisions

### 1. ingest step uses `isData: true` flag
Rather than splitting PY_STEPS into two arrays, added an `isData` boolean flag to the
step object. `_runPySteps` branches on this flag to call `/api/data/ingest` vs
`/api/files/<endpoint>`. This keeps the step array linear and easy to extend.

### 2. Shared `_loadSnapshotsIntoSelect(selectId)` helper
Both Explorer and Questions tabs need a snapshot selector. One helper populates any
`<select>` element by ID. Called independently per tab on tab switch, so the two selectors
are decoupled — no shared state, no sync issues.

### 3. Explorer detail uses existing `viewFile()` for file previews
Shader file_path and texture file_path "View" buttons call the existing `viewFile(path, name, ext)`
function, which opens the inline file viewer in the Results tab pattern. No new viewer code needed.

### 4. `label_metrics` uses raw SQL in the route layer
Per plan alternative A analysis: `category_breakdown` model only provides 5 columns. The
`label_metrics` endpoint provides 14 columns (full multi-metric aggregation). Adding a
new model would require registration etc. Raw SQL in the route is simpler and more direct
for this reporting use case.

### 5. Explorer DC table reuses `.explorer-dc-table` class
The same CSS class is used for the DC list table and the shader/texture sub-tables in
the detail panel. This keeps consistent compact styling throughout the Explorer tab.

---

## Build Validation

`dotnet build SDPCLI` — C# compilation succeeded with 0 compiler errors. The MSBuild
copy step failed with file-lock error because SDPCLI.exe (PID 74824) was running during
the build attempt. This is a runtime conflict, not a code error — no C# source was modified.

---

## Deviations from Plan

None. All phases implemented as specified. Tab order, function names, and CSS class names
match the plan exactly.
