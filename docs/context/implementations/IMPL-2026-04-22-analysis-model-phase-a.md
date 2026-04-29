---
type: implementation
topic: Analysis Model Phase A — AnalysisModel ABC, model_registry, 3 builtin models, model list/run endpoints
status: completed
based_on:
  - PLAN-2026-04-21-analysis-model-question-engine.md
  - FINDING-2026-04-21-analysis-model-question-engine.md
related_paths:
  - pySdp/analysis/models/__init__.py       (NEW)
  - pySdp/analysis/models/base.py           (NEW)
  - pySdp/analysis/models/category_breakdown.py  (NEW)
  - pySdp/analysis/models/top_bottleneck_dcs.py  (NEW)
  - pySdp/analysis/models/label_quality.py  (NEW)
  - pySdp/data/model_registry.py            (NEW)
  - pySdp/webui/routes/data.py              (MODIFIED)
  - pySdp/webui/server.py                   (MODIFIED)
related_tags:
  - python
  - analysis-model
  - model-registry
  - category-breakdown
  - duckdb
  - phase6
last_updated: 2026-04-22
---

# Implementation: Analysis Model Phase A

## Summary

Phase A of the Analysis Model + Question Engine plan is complete. Six new files created, two
modified. All validation scripts pass.

---

## Files Created

### `pySdp/analysis/models/base.py`
Abstract base class `AnalysisModel` with ClassVar metadata attributes (`name`, `description`,
`params_schema`, `viz_type`) and a `_result()` convenience builder. No imports from `data/`.

### `pySdp/data/model_registry.py`
Pure dispatch registry. Stores `dict[str, Any]` — no import from `analysis/` (avoids circular
dependency). Three functions: `register(cls)` (decorator), `list_models()`, `run_model(name, db, snapshot_id, **params)`.
`run_model` raises `KeyError` if name not registered.

### `pySdp/analysis/models/category_breakdown.py`
`CategoryBreakdown` model (`name="category_breakdown"`, `viz_type="bar_chart"`).
Primary path: reads `snapshot_stats` table (pre-computed by status_service).
Fallback: JOINs `draw_calls + labels + metrics` when `snapshot_stats` is empty.
Returns dict rows with keys: `category`, `dc_count`, `clocks_sum`, `clocks_pct`, `avg_conf`.
`metadata.source` field indicates which path was taken (`"snapshot_stats"` or `"computed"`).

### `pySdp/analysis/models/top_bottleneck_dcs.py`
`TopBottleneckDcs` model (`name="top_bottleneck_dcs"`, `viz_type="table"`).
Params: `top_n` (int, default 10, max 100), optional `category` filter.
JOINs `draw_calls + labels + metrics`, orders by `m.clocks DESC NULLS LAST`.
Returns dict rows with 12 fields including `clocks`, `shaders_busy_pct`, `fragments_shaded`.

### `pySdp/analysis/models/label_quality.py`
`LabelQuality` model (`name="label_quality"`, `viz_type="bar_chart"`).
Returns confidence bucket histogram (4 buckets: high/medium/low/very low) as rows.
Summary: `avg_confidence`, `total_labeled`, `low_confidence_count`, `low_confidence_ratio`.

### `pySdp/analysis/models/__init__.py`
Imports all three model modules to trigger `@register` calls:
```python
from analysis.models import category_breakdown, top_bottleneck_dcs, label_quality
```
A single `import analysis.models` in `server.py` registers all three.

---

## Files Modified

### `pySdp/webui/server.py`
Added after `_db = WorkspaceDB()`:
```python
import analysis.models  # registers category_breakdown, top_bottleneck_dcs, label_quality
```

### `pySdp/webui/routes/data.py`
- Added `from fastapi import Body` to imports
- Added `from data import model_registry as _model_registry`
- Added two new endpoints inside `make_router(db)`:
  - `GET /models` — returns `list_models()` result
  - `POST /models/{name}/run?snapshot_id=<n>` — optional JSON body for model params; `KeyError` → 404, other exceptions → 500

---

## Import Chain (acyclic)

```
server.py
  └─ import analysis.models
       └─ analysis/models/__init__.py
            ├─ analysis/models/category_breakdown.py
            │    ├─ analysis/models/base.py      (no data/ imports)
            │    └─ data/model_registry.py       (no analysis/ imports)
            ├─ analysis/models/top_bottleneck_dcs.py
            └─ analysis/models/label_quality.py

webui/routes/data.py
  └─ data/model_registry.py  (list_models, run_model)
```

No circular dependencies. `data/model_registry.py` stores types as `Any` — zero imports from `analysis/`.

---

## Validation Output

```
Registered models: ['category_breakdown', 'top_bottleneck_dcs', 'label_quality']
category_breakdown result: {'model_name': 'category_breakdown', 'snapshot_id': 999,
  'viz_type': 'bar_chart', 'columns': [...], 'rows': [], 'summary': {'total_categories': 0,
  'total_dc_count': 0, 'total_clocks': 0}, 'metadata': {'computed_at': '...', 'source': 'computed'}}
top_bottleneck_dcs result: {'model_name': 'top_bottleneck_dcs', 'rows': [], ...}
label_quality result: {'model_name': 'label_quality', 'rows': [], ...}
Phase A validation passed

routes: ['/ingest', '/snapshots', '/draw_calls', '/dc/{api_id}', '/refresh_labels',
         '/models', '/models/{name}/run']
Router validation OK
```

---

## Deviations from Plan

None significant. The plan's `__init__.py` example used `from analysis.models import X` module-level imports
rather than triggering class registration via `@register` calls directly in `__init__.py`. The
implementation follows the plan's `@register` decorator pattern as specified in the PLAN file — each
model module applies `@register` to its class at import time; `__init__.py` imports all modules.

The fallback window function `SUM(m.clocks) OVER ()` noted as a DuckDB syntax risk in the plan was
replaced with a simpler approach: the fallback computes `total_clocks` in Python from the summed
`clocks_sum` column, avoiding the window function entirely. This is cleaner and DuckDB-safe.

---

## Next Steps (Phase B)

Phase B adds Question CRUD (5 endpoints) and builtin question seeding at startup.
See PLAN-2026-04-21-analysis-model-question-engine.md Phase B section.
