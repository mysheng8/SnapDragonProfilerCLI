---
type: plan
topic: WebUI new tabs — Explorer (single DC query), Questions (label-aggregated metrics), analysis pipeline trigger wiring
status: complete
based_on:
  - FINDING-2026-04-22-webui-new-tabs-plan.md
  - IMPL-2026-04-22-analysis-model-phase-d.md
  - IMPL-2026-04-21-python-data-layer-phase2.md
  - IMPL-2026-04-21-python-data-layer-phase3.md
related_paths:
  - pySdp/webui/static/index.html        (MODIFIED)
  - pySdp/webui/static/app.js            (MODIFIED)
  - pySdp/webui/static/style.css         (MODIFIED)
  - pySdp/webui/routes/data.py           (MODIFIED — new label_metrics endpoint)
  - pySdp/webui/routes/files.py          (no change needed)
  - pySdp/webui/server.py                (no change needed)
related_tags:
  - webui
  - frontend
  - explorer
  - questions
  - drawcall
  - metrics
  - label-aggregation
  - analysis-trigger
  - duckdb
summary: |
  3-phase plan. Phase A: "Explorer" tab — snapshot selector + DC list + single DC detail
  panel (shader/texture/mesh/metrics). Phase B: "Questions" tab — new /api/data/label_metrics
  endpoint + frontend table showing per-label aggregated metrics (sum + avg). Phase C: wire
  ingest + Python steps into the analysis trigger so DuckDB is populated automatically after
  C# job completes.
last_updated: 2026-04-22
---

# Plan: WebUI New Tabs — Explorer + Questions + Analysis Trigger

## Goal

Add two new tabs to the WebUI (keeping the existing "Results" tab unchanged) and wire
the Python data pipeline (DuckDB ingest) into the analysis trigger so that the new tabs
show data automatically after analysis completes.

## Context

The backend API is fully capable:
- `GET /api/data/snapshots` — lists all ingested snapshots
- `GET /api/data/draw_calls?snapshot_id=<n>` — lists DCs for a snapshot (with label)
- `GET /api/data/dc/{api_id}?snapshot_id=<n>` — returns full DC detail (label, metrics, shaders, textures, mesh)
- `POST /api/data/ingest?snapshot_dir=<path>` — ingests snapshot into DuckDB

What is missing:
1. A new frontend tab to browse DCs and inspect a single DC
2. A new `/api/data/label_metrics` endpoint + frontend tab for per-label aggregated metrics
3. Ingest call wired into the analysis trigger in app.js

The existing "Results" tab (file viewer) must remain unchanged.

---

## Phase A: Explorer Tab

### Goal

Allow users to select an ingested snapshot, browse its draw calls, and inspect one DC's
full data (label, metrics, shader stages, textures, mesh file).

### A1. Backend — no new routes needed

All required routes are already implemented:
- `GET /api/data/snapshots`
- `GET /api/data/draw_calls?snapshot_id=<n>[&category=<c>]`
- `GET /api/data/dc/{api_id}?snapshot_id=<n>`
- `GET /api/files/read?path=<p>` (for reading shader HLSL files inline)

### A2. index.html changes

Add a new tab button between "Results" and "Logs":
```html
<button class="tab-btn" data-tab="explorer">Explorer</button>
```

Add a new tab section `#tab-explorer` after `#tab-results`:
```html
<section id="tab-explorer" class="tab-content">
  <!-- Snapshot selector card -->
  <div class="card">
    <div class="card-title">Ingested Snapshots</div>
    <div class="form-row">
      <label>Snapshot</label>
      <select id="explorer-snapshot-sel">
        <option value="">— select —</option>
      </select>
      <button class="btn-secondary btn-sm" onclick="loadExplorerSnapshots()">Refresh</button>
    </div>
    <div class="status-msg" id="explorer-snap-msg"></div>
  </div>

  <!-- DC list card (shown after snapshot selected) -->
  <div class="card" id="explorer-dc-card" style="display:none">
    <div class="card-title">Draw Calls
      <span class="muted" id="explorer-dc-count" style="font-size:12px;margin-left:8px"></span>
    </div>
    <div class="form-row">
      <label>Filter category</label>
      <input id="explorer-cat-filter" type="text" placeholder="e.g. Shadow, UI, Scene">
      <button class="btn-secondary btn-sm" onclick="loadExplorerDCs()">Filter</button>
    </div>
    <div id="explorer-dc-table"></div>
  </div>

  <!-- DC detail panel (shown after DC selected) -->
  <div id="explorer-detail-panel"></div>
</section>
```

### A3. app.js changes

Add constant at top:
```javascript
const DATA = '/api/data';
```

Add state entry:
```javascript
// Explorer state
const explorerState = {
  snapshotId: null,
  dcs: [],
  selectedApiId: null,
};
```

Add functions:
- `loadExplorerSnapshots()` — calls `GET /api/data/snapshots`, populates `#explorer-snapshot-sel`
- `onExplorerSnapshotChange()` — reads selected snapshot_id, calls `loadExplorerDCs()`
- `loadExplorerDCs()` — calls `GET /api/data/draw_calls?snapshot_id=<n>&category=<filter>`,
  renders `#explorer-dc-table` as a compact table (api_id, api_name, category, confidence, clocks)
- `loadExplorerDCDetail(apiId)` — calls `GET /api/data/dc/{api_id}?snapshot_id=<n>`,
  renders `#explorer-detail-panel` with sub-sections:
  - Base info (api_id, api_name, vertex_count, index_count)
  - Label (category, subcategory, detail, confidence, reason_tags, bottleneck_text)
  - Metrics (table of all available metric keys + values)
  - Shader stages (collapsible, each stage shows stage/entry_point/file_path with inline "View" button)
  - Textures (table: texture_id, width x height, format, file_path with inline "View" button)
  - Mesh file (file_path with inline "View" button if present)
  - "View" buttons call existing `viewFile(path, name, ext)` function

Add `switchTab` handler for `explorer` id:
```javascript
if (id === 'explorer') {
  loadExplorerSnapshots();
}
```

### A4. style.css changes

Add styles for:
- `#explorer-dc-table table` — compact table with hover row highlight
- `.dc-row` — clickable table row style (cursor:pointer, hover background)
- `#explorer-detail-panel .detail-section` — card-like subsection with collapsible header
- `.metric-table` — two-column key/value table for metrics

---

## Phase B: Questions Tab

### Goal

Show per-label metrics aggregation: for each label category, show DC count, SUM and AVG
of key metrics (clocks, shaders_busy_pct, tex_fetch_stall_pct, etc.).

### B1. Backend — new endpoint in routes/data.py

Add to `make_router(db)`:

```python
@router.get("/label_metrics")
def label_metrics(
    snapshot_id: int = Query(..., description="Snapshot ID"),
):
    """Return per-label aggregated metrics (sum + avg) for a snapshot.

    Joins draw_calls + labels + metrics, groups by labels.category.
    Returns one row per category with:
      category, dc_count,
      clocks_sum, clocks_avg,
      shaders_busy_pct_avg,
      tex_fetch_stall_pct_avg,
      tex_l1_miss_pct_avg,
      tex_l2_miss_pct_avg,
      fragments_shaded_sum, fragments_shaded_avg,
      vertices_shaded_sum, vertices_shaded_avg,
      read_bytes_sum, write_bytes_sum
    Ordered by clocks_sum DESC.
    """
    sql = """
        SELECT
            COALESCE(lb.category, 'Unlabeled') AS category,
            COUNT(dc.api_id)                    AS dc_count,
            SUM(m.clocks)                       AS clocks_sum,
            AVG(m.clocks)                       AS clocks_avg,
            AVG(m.shaders_busy_pct)             AS shaders_busy_pct_avg,
            AVG(m.tex_fetch_stall_pct)          AS tex_fetch_stall_pct_avg,
            AVG(m.tex_l1_miss_pct)              AS tex_l1_miss_pct_avg,
            AVG(m.tex_l2_miss_pct)              AS tex_l2_miss_pct_avg,
            SUM(m.fragments_shaded)             AS fragments_shaded_sum,
            AVG(m.fragments_shaded)             AS fragments_shaded_avg,
            SUM(m.vertices_shaded)              AS vertices_shaded_sum,
            AVG(m.vertices_shaded)              AS vertices_shaded_avg,
            SUM(m.read_total_bytes)             AS read_bytes_sum,
            SUM(m.write_total_bytes)            AS write_bytes_sum
        FROM draw_calls dc
        LEFT JOIN labels lb
            ON lb.snapshot_id = dc.snapshot_id AND lb.api_id = dc.api_id
        LEFT JOIN metrics m
            ON m.snapshot_id = dc.snapshot_id AND m.api_id = dc.api_id
        WHERE dc.snapshot_id = ?
        GROUP BY COALESCE(lb.category, 'Unlabeled')
        ORDER BY SUM(m.clocks) DESC NULLS LAST
    """
    try:
        result = db.conn().execute(sql, [snapshot_id])
        cols = [d[0] for d in result.description]
        rows = [dict(zip(cols, row)) for row in result.fetchall()]
        # Round float values
        for row in rows:
            for k, v in row.items():
                if isinstance(v, float):
                    row[k] = round(v, 2)
        return {"ok": True, "data": rows}
    except Exception as exc:
        _logger_module.get_logger().error(
            "label_metrics failed", exc=exc, context={"snapshot_id": snapshot_id}
        )
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)
```

### B2. index.html changes

Add tab button between "Results" and "Explorer":
```html
<button class="tab-btn" data-tab="questions">Questions</button>
```

Final tab order: Snapshot | Analysis | Results | Questions | Explorer | Logs

Add tab section `#tab-questions`:
```html
<section id="tab-questions" class="tab-content">
  <!-- Snapshot selector card -->
  <div class="card">
    <div class="card-title">Label Metrics by Category</div>
    <div class="form-row">
      <label>Snapshot</label>
      <select id="questions-snapshot-sel">
        <option value="">— select —</option>
      </select>
      <button class="btn-secondary btn-sm" onclick="loadQuestionsData()">Load</button>
    </div>
    <div class="status-msg" id="questions-msg"></div>
  </div>

  <!-- Metrics aggregation table -->
  <div class="card" id="questions-result-card" style="display:none">
    <div class="card-title">Metrics Aggregated by Label</div>
    <div id="questions-table-wrap"></div>
  </div>
</section>
```

### B3. app.js changes

Add state entry:
```javascript
const questionsState = { snapshotId: null };
```

Add functions:
- `loadQuestionsSnapshots()` — shared with Explorer (or reuse `loadExplorerSnapshots()`
  to also populate `#questions-snapshot-sel`); alternatively call the same helper
- `loadQuestionsData()` — reads `#questions-snapshot-sel`, calls
  `GET /api/data/label_metrics?snapshot_id=<n>`, renders `#questions-table-wrap` as an
  HTML table with columns:
  - Category, DC Count, Clocks Sum, Clocks Avg, Shaders Busy Avg (%), Tex Fetch Stall Avg (%),
    Tex L1 Miss Avg (%), Frags Sum, Verts Sum, Read Bytes, Write Bytes

Add `switchTab` handler for `questions` id:
```javascript
if (id === 'questions') {
  // Reuse same snapshot list as explorer
  _loadSnapshotsIntoSelect('questions-snapshot-sel');
}
```

### B4. style.css changes

Add `.questions-table` style — responsive horizontal scroll, sticky first column (category).

---

## Phase C: Wire Analysis Trigger to Python Pipeline

### Goal

After C# analysis job completes, automatically run:
1. `POST /api/data/ingest?snapshot_dir=<captureDir>` — register snapshot in DuckDB
2. `POST /api/files/label` — write label.json + persist labels to DuckDB
3. `POST /api/files/status` — write status.json + persist snapshot_stats to DuckDB
4. (then continue with) topdc, analysis_md, dashboard as before

### C1. app.js changes — PY_STEPS array

Currently (app.js line 442):
```javascript
const PY_STEPS = [
  { key: 'label',     endpoint: 'label',       phase: 'label_drawcalls' },
  { key: 'status',    endpoint: 'status',       phase: 'generate_stats'  },
  { key: 'topdc',     endpoint: 'topdc',        phase: 'generate_topdc'  },
  { key: 'analysis',  endpoint: 'analysis_md',  phase: 'report_analysis' },
  { key: 'dashboard', endpoint: 'dashboard',    phase: 'dashboard'       },
];
```

Add `ingest` as the first step:
```javascript
const PY_STEPS = [
  { key: 'ingest',    endpoint: null,           phase: 'ingest_db',      isData: true },
  { key: 'label',     endpoint: 'label',        phase: 'label_drawcalls' },
  { key: 'status',    endpoint: 'status',       phase: 'generate_stats'  },
  { key: 'topdc',     endpoint: 'topdc',        phase: 'generate_topdc'  },
  { key: 'analysis',  endpoint: 'analysis_md',  phase: 'report_analysis' },
  { key: 'dashboard', endpoint: 'dashboard',    phase: 'dashboard'       },
];
```

Add `ingest` to `ALL_TARGETS`:
```javascript
const ALL_TARGETS = ['ingest','dc','shaders','textures','buffers','label','metrics','status','topdc','analysis','dashboard'];
```

`ingest` must NOT be in `CS_TARGETS`.

The `_runPySteps` function must handle the `ingest` step differently (it calls `/api/data/ingest`
with a query param rather than `/api/files/<endpoint>` with a `snapshot_dir` param):

```javascript
async function _runPySteps(sdpPath, captureDir, selected) {
  const steps = PY_STEPS.filter(s => selected.has(s.key));
  const total  = steps.length;

  for (let i = 0; i < total; i++) {
    const step = steps[i];
    const pct  = 70 + Math.round(((i) / total) * 30);
    showProg('analysis', pct, step.phase);
    try {
      let res;
      if (step.key === 'ingest') {
        // Data endpoint uses different URL pattern
        res = await apiPost(`${DATA}/ingest?snapshot_dir=${encodeURIComponent(captureDir)}`, {});
      } else {
        res = await apiPost(`${FILES}/${step.endpoint}?snapshot_dir=${encodeURIComponent(captureDir)}`, {});
      }
      if (!res.ok) {
        console.warn(`Python step ${step.key} failed: ${res.error}`);
      }
    } catch (err) {
      console.warn(`Python step ${step.key} error: ${err.message}`);
    }
  }

  _finishAnalysis(sdpPath, captureDir, null);
}
```

### C2. Analysis settings target chips

`ingest` is added to `ALL_TARGETS` so it appears in the target checkboxes. It should be
checked by default. It is non-fatal (like all Python steps) — if it fails, the pipeline continues.

### C3. No changes to routes/files.py, server.py, or routes/data.py for this phase

The `/api/data/ingest` endpoint already exists and is correct. No backend change needed.

---

## Implementation Order

Phases are independent and can be done in any order, but recommended order:
1. Phase C first — unblocks data in DuckDB for new analyses
2. Phase A — Explorer tab (requires data in DuckDB)
3. Phase B — Questions tab (requires label_metrics endpoint + data in DuckDB)

---

## Alternatives Considered

### Alternative A: Use model/question/dashboard for Questions tab
- Use existing `category_breakdown` model + `/api/data/models/category_breakdown/run`
- Pros: reuses existing infrastructure
- Cons: category_breakdown only returns `category, dc_count, clocks_sum, clocks_pct, avg_conf` —
  not the full multi-metric aggregation the user wants. Would require a new model anyway.
- Decision: add a dedicated `label_metrics` endpoint for simplicity and full control over columns

### Alternative B: Embed Explorer inside Results tab
- Show DC detail inline in the Results file viewer
- Pros: fewer tabs, consistent location
- Cons: Results tab is file-system-centric (reads raw JSON files from disk); Explorer is
  DB-centric (queries DuckDB). Mixing them creates confusion. Separate tab is cleaner.
- Decision: separate tab

### Alternative C: Auto-ingest on server startup
- Scan all snapshot dirs and ingest at startup
- Pros: no trigger change needed
- Cons: expensive startup, unclear which dirs to scan, doesn't help for new analyses
- Decision: trigger-based ingest is more predictable

---

## Risks

- Risk 1: `ingest` step in PY_STEPS changes existing target chips UI — users may not expect
  a new "ingest" checkbox. Mitigation: default it to checked, and document that it populates
  the DB for Explorer/Questions tabs.
- Risk 2: `label_metrics` query may return NULL for DCs without metrics entries — the COALESCE
  and LEFT JOIN handle this; rows with no metrics will show NULLs for metric columns, which is
  acceptable.
- Risk 3: Explorer tab calls `loadExplorerSnapshots()` on every tab switch — if many snapshots
  are ingested this may be slow. Mitigation: debounce or only refresh when explicitly requested.
- Risk 4: Two separate snapshot selectors (Explorer + Questions) may get out of sync.
  Mitigation: share a single `_loadSnapshotsIntoSelect(selectId)` helper function.

---

## Validation

### Phase A
- Navigate to Explorer tab — snapshot selector populates from /api/data/snapshots
- Select a snapshot — DC list renders (api_id, api_name, category, clocks visible)
- Click a DC row — detail panel renders with all sub-sections
- Click "View" on a shader file — existing viewFile() opens inline viewer

### Phase B
- Navigate to Questions tab — snapshot selector populates
- Select a snapshot, click Load — table renders with one row per category
- Verify clocks_sum and clocks_avg are non-zero for ingested data
- Verify all float columns are rounded to 2 decimal places

### Phase C
- Run analysis on an SDP file with `ingest` target checked
- After completion, navigate to Explorer tab — newly analyzed snapshot appears in selector
- Navigate to Questions tab — data is populated without manual ingest call

---

## Implementation Notes (for executor agent)

### Files to modify

| File | Change |
|---|---|
| `pySdp/webui/static/index.html` | Add 2 tab buttons + 2 tab sections |
| `pySdp/webui/static/app.js` | Add DATA constant, PY_STEPS ingest entry, ALL_TARGETS update, Explorer/Questions functions, switchTab handlers |
| `pySdp/webui/static/style.css` | Add Explorer table styles, Questions table styles |
| `pySdp/webui/routes/data.py` | Add GET /label_metrics endpoint in make_router(db) |

### Files NOT to modify

| File | Reason |
|---|---|
| `pySdp/webui/routes/files.py` | No change needed — all /api/files/* routes are intact |
| `pySdp/webui/server.py` | No change needed — router registration is unchanged |
| `pySdp/data/query.py` | No change needed — label_metrics uses raw SQL in route layer |
| `pySdp/data/db.py` | No change needed — schema is unchanged |
| `pySdp/analysis/` | No change needed — all services are intact |

### Key implementation detail: shared snapshot selector helper

Both Explorer and Questions tabs need a snapshot selector. Avoid duplicating code:

```javascript
async function _loadSnapshotsIntoSelect(selectId) {
  const sel = document.getElementById(selectId);
  if (!sel) return;
  try {
    const res = await apiGet(`${DATA}/snapshots`);
    if (!res.ok) return;
    const prev = sel.value;
    sel.innerHTML = '<option value="">— select —</option>';
    (res.data || []).forEach(s => {
      const opt = document.createElement('option');
      opt.value       = s.snapshot_id;
      opt.textContent = `#${s.snapshot_id} — ${s.run_name} / ${s.sdp_name}`;
      sel.appendChild(opt);
    });
    if (prev) sel.value = prev;  // preserve selection on refresh
  } catch { /* ignore */ }
}
```

### Key implementation detail: DC table rendering

The DC list table should be compact and clickable:
```javascript
function renderExplorerDCTable(dcs) {
  // Build <table> with rows for: api_id, api_name, category (or —), confidence (or —), clocks (or —)
  // Each row: onclick = () => loadExplorerDCDetail(row.api_id)
  // Highlight selected row with .active class
}
```

### Key implementation detail: DC detail panel rendering

The detail panel should use collapsible sections (reuse the same pattern as buildSection()
in the Results tab). Sub-sections: Base Info, Label, Metrics, Shaders, Textures, Mesh.

For shader file_path and texture file_path: check if file exists before showing "View" button.
The existing `viewFile(path, name, ext)` function handles the inline viewer.

### Key implementation detail: label_metrics NULL handling

The SQL uses `LEFT JOIN metrics` — some DCs may have no metrics row. The Python route should
handle NULL values gracefully, rounding only non-None floats:
```python
for row in rows:
    for k, v in row.items():
        if isinstance(v, float) and v is not None:
            row[k] = round(v, 2)
```
