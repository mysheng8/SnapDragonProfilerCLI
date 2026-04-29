---
type: implementation
topic: Analysis Model Phase D — Dashboard CRUD + run endpoint
status: completed
based_on:
  - PLAN-2026-04-21-analysis-model-question-engine.md
related_paths:
  - pySdp/data/dashboards.py           (NEW)
  - pySdp/webui/routes/data.py         (MODIFIED)
  - pySdp/webui/server.py              (MODIFIED)
tags:
  - python
  - dashboard
  - question-engine
  - duckdb
  - crud
  - phase7
date: 2026-04-22
---

# Implementation: Analysis Model Phase D — Dashboard CRUD + Run

## Summary

Phase D completes the Question Engine by adding Dashboard support: an ordered list of
question IDs that can be run as a single batch operation, with per-panel fault isolation.

---

## Files Created

### `pySdp/data/dashboards.py` (NEW)

Pure data-layer CRUD module mirroring the pattern of `data/questions.py`.

Key design decisions:
- `_SELECT_COLS` uses `CAST(created_at AS VARCHAR)` and `CAST(updated_at AS VARCHAR)` to
  avoid DuckDB pytz dependency (same workaround as `questions.py`).
- `_row_to_dict` deserializes `question_ids` via `json.loads` — stored as `TEXT JSON array`.
- `seed_builtin_dashboards` inserts a single `builtin-overview` dashboard wiring the 3
  builtin questions (`builtin-category-breakdown`, `builtin-top-bottleneck-dcs`,
  `builtin-label-quality`). Idempotent — returns 0 on repeated calls.

Functions: `create_dashboard`, `list_dashboards`, `get_dashboard`, `update_dashboard`,
`delete_dashboard`, `seed_builtin_dashboards`.

---

## Files Modified

### `pySdp/webui/routes/data.py`

1. Added import: `from data import dashboards as _dash` (alongside existing `_q` import).
2. Added two Pydantic request body models: `DashboardCreate` and `DashboardUpdate`.
3. Added 6 new endpoints inside `make_router(db)`:

| Method | Path | Description |
|---|---|---|
| GET | `/dashboards` | List all dashboards |
| POST | `/dashboards` | Create new dashboard |
| GET | `/dashboards/{dashboard_id}` | Get single dashboard |
| PUT | `/dashboards/{dashboard_id}` | Update title / question_ids |
| DELETE | `/dashboards/{dashboard_id}` | Delete dashboard |
| POST | `/dashboards/{dashboard_id}/run` | Run all panels, non-fatal per panel |

**Dashboard run logic**: iterates `d["question_ids"]`, calls `_q.get_question` for each,
dispatches to `_model_registry.run_model`, overlays `viz_type`/`viz_config`/`question`
metadata onto the model result, and appends `{"question_id", "ok", "data"}` per panel.
Exceptions per panel are caught individually — one failing panel does not abort the rest.

### `pySdp/webui/server.py`

Added after the `seed_builtin_questions` call:

```python
from data.dashboards import seed_builtin_dashboards as _seed_builtin_dashboards
_seeded_dash = _seed_builtin_dashboards(_db)
_logger_module.get_logger().info(f"Seeded {_seeded_dash} built-in dashboards")
```

---

## Validation Results

### Data-layer validation

```
Seeded 1 builtin dashboards
List OK: Overview
Get OK, question_ids: ['builtin-category-breakdown', 'builtin-top-bottleneck-dcs', 'builtin-label-quality']
Created: <uuid>
Update OK
Delete OK
Idempotent seed OK
Dashboard run: 3 panels, all ok: True
Phase D validation passed
```

### Route validation

All 6 new routes confirmed present:
- `['GET'] /dashboards`
- `['POST'] /dashboards`
- `['GET'] /dashboards/{dashboard_id}`
- `['PUT'] /dashboards/{dashboard_id}`
- `['DELETE'] /dashboards/{dashboard_id}`
- `['POST'] /dashboards/{dashboard_id}/run`

Total routes in `make_router`: 19 (was 13 before Phase D).

---

## Deviations from Plan

None. Implementation matches the plan spec exactly. The `run_dashboard` endpoint uses
`_q.get_question` (already imported as `_dash`'s companion) rather than raw SQL, consistent
with the existing `run_question` pattern in Phase C.
