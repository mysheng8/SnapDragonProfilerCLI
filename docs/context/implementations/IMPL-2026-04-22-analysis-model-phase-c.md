---
type: implementation
topic: Analysis Model Phase C — Question run endpoint
status: completed
plan_ref: PLAN-2026-04-21-analysis-model-question-engine.md
related_paths:
  - pySdp/webui/routes/data.py   (MODIFIED)
tags:
  - python
  - question-engine
  - analysis-model
  - duckdb
  - phase7
last_updated: 2026-04-22
---

# Implementation: Analysis Model Phase C — Question Run Endpoint

## What was built

Added one new endpoint to `pySdp/webui/routes/data.py` inside the `make_router(db)` factory:

```
POST /api/data/questions/{question_id}/run?snapshot_id=<n>
```

## Endpoint code

```python
@router.post("/questions/{question_id}/run")
def run_question(
    question_id: str,
    snapshot_id: int = Query(..., description="Snapshot ID to run against"),
):
    try:
        q = _q.get_question(db, question_id)
        if q is None:
            return JSONResponse(
                {"ok": False, "error": f"Question '{question_id}' not found"},
                status_code=404,
            )
        result = _model_registry.run_model(
            q["model_name"], db, snapshot_id, **q["model_params"]
        )
        # Question's viz settings take precedence over model defaults
        result["viz_type"]   = q["viz_type"]
        result["viz_config"] = q["viz_config"]
        result["question"]   = {"id": q["id"], "title": q["title"]}
        return {"ok": True, "data": result}
    except KeyError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error(
            "run_question failed", exc=exc,
            context={"question_id": question_id, "snapshot_id": snapshot_id},
        )
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)
```

## Response shape

```json
{
  "ok": true,
  "data": {
    "model_name": "category_breakdown",
    "snapshot_id": 1,
    "viz_type": "bar_chart",
    "viz_config": {"x": "category", "y": "clocks_pct", "label": "dc_count"},
    "columns": [...],
    "rows": [...],
    "summary": {...},
    "metadata": {...},
    "question": {"id": "builtin-category-breakdown", "title": "Category Breakdown"}
  }
}
```

## Implementation notes

- No new imports were needed: `_model_registry` and `_q` were already imported in the file from Phases A and B.
- `_q.get_question()` already deserializes `model_params` and `viz_config` from JSON strings to dicts, so `**q["model_params"]` unpacking works directly.
- The endpoint is registered after `DELETE /questions/{question_id}` in the router. FastAPI matches the `/run` literal segment before `{question_id}` path param, so there is no path conflict.
- Error handling: 404 for unknown question, 404 for unknown model name (KeyError from run_model), 500 for all other exceptions with logging.

## Validation

Both validation scripts passed:

**Logic validation** (simulate endpoint in-process):
```
Run result keys: ['model_name', 'snapshot_id', 'viz_type', 'columns', 'rows', 'summary', 'metadata', 'viz_config', 'question']
Logic validation OK
Phase C logic validated
```

**Route registration validation**:
```
All routes:
  ['POST'] /ingest
  ['GET'] /snapshots
  ['GET'] /draw_calls
  ['GET'] /dc/{api_id}
  ['POST'] /refresh_labels
  ['GET'] /models
  ['POST'] /models/{name}/run
  ['GET'] /questions
  ['POST'] /questions
  ['GET'] /questions/{question_id}
  ['PUT'] /questions/{question_id}
  ['DELETE'] /questions/{question_id}
  ['POST'] /questions/{question_id}/run
Question run routes: ['/questions/{question_id}/run']
Route validation OK
```

## Files modified

| File | Change |
|---|---|
| `pySdp/webui/routes/data.py` | Added `POST /questions/{question_id}/run` endpoint inside `make_router()` |
