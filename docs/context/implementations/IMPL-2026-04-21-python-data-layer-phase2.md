---
type: implementation
topic: Python data layer Phase 2 — typed query API + draw_calls / dc detail endpoints
status: completed
based_on:
  - PLAN-2026-04-21-python-data-layer.md (Phase 2 section)
  - IMPL-2026-04-21-python-data-layer-phase1.md
related_paths:
  - pySdp/data/query.py          (NEW)
  - pySdp/webui/routes/data.py   (MODIFIED)
tags:
  - python
  - data-layer
  - duckdb
  - query-layer
  - phase2
last_updated: 2026-04-21
---

# IMPL-2026-04-21: Python Data Layer Phase 2 — Query Layer

## What was built

### NEW: `pySdp/data/query.py`

Five typed read functions, all taking `db: WorkspaceDB` as first argument and returning plain Python dicts/lists:

| Function | Returns | Notes |
|---|---|---|
| `get_draw_calls(db, snapshot_id, *, category, tags)` | `list[dict]` | draw_calls LEFT JOIN labels; tags filtered Python-side |
| `get_labels(db, snapshot_id)` | `dict[int, dict]` | keyed by api_id; reason_tags parsed to list |
| `get_metrics(db, snapshot_id)` | `dict[int, dict]` | keyed by api_id; None values stripped from each dict |
| `get_dc_detail(db, snapshot_id, api_id)` | `dict | None` | full DC: base + label + metrics + shader_stages + textures + mesh_file |
| `query_dcs(db, snapshot_id, *, category, min_clocks, label_source, tags)` | `list[dict]` | joined DC+label+metrics with all optional filters ANDed; ordered clocks DESC NULLS LAST |

Key implementation details:
- `_rows_to_dicts()` helper uses `result.description` for column names (no pandas dependency)
- `reason_tags` JSON string deserialized via `json.loads()` in all functions
- `get_metrics` strips None values per row via `_filter_none_values()`
- `get_dc_detail` returns `None` (not 404) when api_id absent — 404 mapping is in the route layer
- `query_dcs` fetches reason_tags for tag filtering then strips it from output rows
- Tags filter is Python-side post-filter (avoids DuckDB JSON syntax complexity; dataset is small)

### MODIFIED: `pySdp/webui/routes/data.py`

Added two GET endpoints to the existing `make_router(db)` factory:

```
GET /api/data/draw_calls?snapshot_id=<n>[&category=<c>][&tags=shadow,depth]
    → {"ok": True, "data": [...]}

GET /api/data/dc/<api_id>?snapshot_id=<n>
    → {"ok": True, "data": {...}}
    → 404 {"ok": False, "error": "..."} if not found
```

Added import: `from data.query import get_draw_calls, get_dc_detail`

Tags parameter: `tags: str = Query(default=None)` split by comma before passing to `get_draw_calls`.

## Validation output

```
All query functions validated OK
Routes: ['/ingest', '/snapshots', '/draw_calls', '/dc/{api_id}']
Router validation OK
```

Empty-DB assertions:
- `get_draw_calls(db, 999)` → `[]`
- `get_labels(db, 999)` → `{}`
- `get_metrics(db, 999)` → `{}`
- `get_dc_detail(db, 999, 1)` → `None`
- `query_dcs(db, 999)` → `[]`

## Deviations from plan

None. Implementation matches the Phase 2 spec exactly.

## Files changed

| File | Operation |
|---|---|
| `pySdp/data/query.py` | CREATED |
| `pySdp/webui/routes/data.py` | MODIFIED (import + 2 new endpoints) |
| `docs/context/implementations/IMPL-2026-04-21-python-data-layer-phase2.md` | CREATED |
| `docs/context/INDEX.md` | MODIFIED (IMPL entry added) |
