---
type: implementation
topic: Analysis Model Phase B — Question CRUD + builtin question seeding
status: completed
based_on:
  - PLAN-2026-04-21-analysis-model-question-engine.md
  - IMPL-2026-04-22-analysis-model-phase-a.md
related_paths:
  - pySdp/data/questions.py           (NEW)
  - pySdp/webui/routes/data.py        (MODIFIED — 5 question endpoints)
  - pySdp/webui/server.py             (MODIFIED — seed_builtin_questions at startup)
tags:
  - python
  - question-engine
  - duckdb
  - crud
  - phase7
date: 2026-04-22
---

# Implementation: Analysis Model Phase B — Question CRUD

## What was built

### 1. `pySdp/data/questions.py` (NEW)

Pure data-layer module for the `questions` table. No FastAPI imports. Six public functions:

- `create_question(db, *, title, model_name, model_params, viz_type, viz_config, is_builtin, question_id)` — inserts row, returns created dict
- `list_questions(db)` — all questions ordered by `created_at ASC`
- `get_question(db, question_id)` — single row or `None`
- `update_question(db, question_id, *, title, model_params, viz_type, viz_config)` — partial update, only non-None fields applied
- `delete_question(db, question_id)` — returns `True` if deleted, `False` if not found
- `seed_builtin_questions(db)` — inserts 3 builtin questions if absent, returns count inserted

All SELECT statements cast `created_at` as `VARCHAR` to avoid DuckDB's `pytz` requirement for `TIMESTAMPTZ` Python conversion. JSON fields (`model_params`, `viz_config`) are stored as TEXT and deserialized to `dict` in all returned dicts.

**Builtin questions seeded:**
| ID | Title | Model | viz_type |
|---|---|---|---|
| `builtin-category-breakdown` | Category Breakdown | `category_breakdown` | `bar_chart` |
| `builtin-top-bottleneck-dcs` | Top Bottleneck Draw Calls | `top_bottleneck_dcs` | `table` |
| `builtin-label-quality` | Label Quality | `label_quality` | `bar_chart` |

### 2. `pySdp/webui/routes/data.py` (MODIFIED)

Added at module level:
- `from typing import Any, Optional`
- `from pydantic import BaseModel`
- `from data import questions as _q`
- `QuestionCreate` Pydantic model (title, model_name, model_params, viz_type, viz_config)
- `QuestionUpdate` Pydantic model (all fields Optional)

Added 5 endpoints inside `make_router(db)`:

| Method | Path | Notes |
|---|---|---|
| `GET` | `/questions` | List all questions |
| `POST` | `/questions` | Create; validates model_name against registry; returns 400 if unknown |
| `GET` | `/questions/{question_id}` | Get by ID; 404 if not found |
| `PUT` | `/questions/{question_id}` | Partial update; 404 if not found |
| `DELETE` | `/questions/{question_id}` | Delete; 404 if not found |

All question endpoints return `{"ok": True, "data": ...}` on success and `{"ok": False, "error": ...}` on error.

### 3. `pySdp/webui/server.py` (MODIFIED)

Added after `import analysis.models`:
```python
from data.questions import seed_builtin_questions as _seed_builtin_questions
_seeded = _seed_builtin_questions(_db)
_logger_module.get_logger().info(f"Seeded {_seeded} built-in questions")
```

Seeding is idempotent — on subsequent restarts `_seeded` will be 0.

## Deviations from plan

- The spec's `QuestionCreate`/`QuestionUpdate` used `dict[str, Any]` type hints; changed to `Optional[dict]` for Python 3.9 compatibility (avoids `from __future__ import annotations` interaction with Pydantic v1).
- `_SELECT_COLS` constant added to share the `CAST(created_at AS VARCHAR)` trick across `list_questions` and `get_question`, avoiding duplication.
- `is_builtin` field: no special protection in MVP — can be updated or deleted like user questions, as specified.

## Validation

Both validation scripts passed:

```
Seeded 2 builtin questions   # (1 was already present from prior Phase A session)
Total questions: 3
Get OK: Category Breakdown
Created: <uuid>
Update OK
Delete OK
Idempotent seed OK
Phase B validation passed

routes:
  {'GET'} /questions
  {'POST'} /questions
  {'GET'} /questions/{question_id}
  {'PUT'} /questions/{question_id}
  {'DELETE'} /questions/{question_id}
Route validation OK
```

## Next phases

- **Phase C**: `POST /api/data/questions/{question_id}/run?snapshot_id=<n>` — dispatch to model, merge viz metadata
- **Phase D**: Dashboard CRUD + `POST /api/data/dashboards/{id}/run`
