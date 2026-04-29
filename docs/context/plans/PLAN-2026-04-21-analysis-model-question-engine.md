---
type: plan
topic: Analysis Model + Question Engine — AnalysisModel interface, model_registry, category_breakdown, Question CRUD, Dashboard
status: proposed
based_on:
  - FINDING-2026-04-21-analysis-model-question-engine.md
  - PLAN-2026-04-21-python-data-layer.md
  - IMPL-2026-04-21-python-data-layer-phase3.md
related_paths:
  - pySdp/data/model_registry.py            (NEW — Phase A)
  - pySdp/analysis/models/__init__.py       (NEW — Phase A)
  - pySdp/analysis/models/base.py           (NEW — Phase A)
  - pySdp/analysis/models/category_breakdown.py  (NEW — Phase A)
  - pySdp/analysis/models/top_bottleneck_dcs.py  (NEW — Phase A)
  - pySdp/analysis/models/label_quality.py  (NEW — Phase A)
  - pySdp/webui/routes/data.py              (MODIFIED — Phase B + C + D)
  - pySdp/webui/server.py                   (MODIFIED — Phase A, model registration)
related_tags:
  - python
  - analysis-model
  - question-engine
  - dashboard
  - duckdb
  - model-registry
  - category-breakdown
  - viz-type
  - phase6
  - phase7
summary: |
  4-phase plan. Phase A: AnalysisModel ABC + model_registry + 3 builtin models
  (category_breakdown, top_bottleneck_dcs, label_quality) + model list/run endpoints.
  Phase B: Question CRUD (create, list, get, update, delete) backed by questions table,
  builtin question seeding at startup. Phase C: Question run endpoint — run model,
  return structured result with viz_type hint. Phase D: Dashboard CRUD + run — ordered
  list of questions, run all in sequence.
last_updated: 2026-04-21
---

# Plan: Analysis Model + Question Engine

## Goal

Implement Phase 6 (Analysis Model Registry) and Phase 7 (Question Engine + Dashboard)
of the Python data layer, using `category_breakdown` as the first concrete model.

After these phases, every analysis result flows through a named, parameterized model,
and users can create saved "questions" that bind a model + viz_type for reuse across
multiple snapshots.

---

## Context

From FINDING-2026-04-21-analysis-model-question-engine.md:

- Phases 0-3 are complete: DuckDB schema, ingest, query layer, label/stats persistence
- `snapshot_stats` table has per-category (dc_count, clocks_sum, clocks_pct, avg_conf)
- `questions` and `dashboards` tables exist in schema but are empty
- `pySdp/analysis/models/` directory does not yet exist
- `pySdp/data/model_registry.py` does not yet exist
- Import chain for model registration must be orchestrated in `server.py` (composition root)
- No result caching needed for MVP (DuckDB sub-millisecond for small scan sizes)

---

## Phase A: AnalysisModel Interface + Model Registry + Builtin Models

**Goal**: Create the model layer — abstract base, registry, and 3 concrete models.
No HTTP changes yet — validate by importing and calling models directly.

### Files to create

#### 1. `pySdp/analysis/models/__init__.py`

Empty or single-line import trigger:
```python
from analysis.models import category_breakdown, top_bottleneck_dcs, label_quality
```
This one import in `__init__.py` causes all three modules to execute their
`@register` calls when `import analysis.models` runs in `server.py`.

#### 2. `pySdp/analysis/models/base.py`

```python
from __future__ import annotations
from typing import ClassVar, Any

class AnalysisModel:
    """Abstract base class for all analysis models.

    Subclasses must declare ClassVar attributes and implement run().
    """
    name: ClassVar[str]
    description: ClassVar[str]
    params_schema: ClassVar[dict]   # JSON Schema dict — {} means no params
    viz_type: ClassVar[str]         # default: "table" | "bar_chart" | "pie_chart" | "number"

    def run(self, db, snapshot_id: int, **params) -> dict:
        raise NotImplementedError

    # ── Convenience: build standard result envelope ───────────────────────
    def _result(self, snapshot_id: int, columns: list, rows: list,
                summary: dict | None = None, viz_type: str | None = None,
                metadata: dict | None = None) -> dict:
        return {
            "model_name":  self.name,
            "snapshot_id": snapshot_id,
            "viz_type":    viz_type or self.viz_type,
            "columns":     columns,
            "rows":        rows,
            "summary":     summary or {},
            "metadata":    metadata or {},
        }
```

#### 3. `pySdp/data/model_registry.py`

```python
from __future__ import annotations
from analysis.models.base import AnalysisModel

_REGISTRY: dict[str, type[AnalysisModel]] = {}

def register(model_cls: type[AnalysisModel]) -> type[AnalysisModel]:
    """Decorator: register an AnalysisModel subclass."""
    _REGISTRY[model_cls.name] = model_cls
    return model_cls

def list_models() -> list[dict]:
    return [
        {
            "name":          cls.name,
            "description":   cls.description,
            "params_schema": cls.params_schema,
            "viz_type":      cls.viz_type,
        }
        for cls in _REGISTRY.values()
    ]

def run_model(name: str, db, snapshot_id: int, **params) -> dict:
    if name not in _REGISTRY:
        raise KeyError(f"Model '{name}' not registered. Available: {list(_REGISTRY)}")
    return _REGISTRY[name]().run(db, snapshot_id, **params)
```

Note: `model_registry.py` is in `data/` because it is a pure dispatch mechanism with no
analysis logic. It imports `AnalysisModel` from `analysis/models/base.py`. This is a
one-way dependency (`data → analysis/base` only; `analysis/models/` → `data/model_registry`
for the `@register` decorator). This is acceptable because `base.py` has zero DB imports.

#### 4. `pySdp/analysis/models/category_breakdown.py`

```python
from __future__ import annotations
from datetime import datetime, timezone
from analysis.models.base import AnalysisModel
from data.model_registry import register

@register
class CategoryBreakdown(AnalysisModel):
    name = "category_breakdown"
    description = "Per-category DrawCall count, clock budget, and label confidence"
    params_schema = {}
    viz_type = "bar_chart"

    def run(self, db, snapshot_id: int, **params) -> dict:
        conn = db.conn()

        # Primary path: read from snapshot_stats (pre-computed, fast)
        rows_raw = conn.execute(
            """
            SELECT category, dc_count, clocks_sum, clocks_pct, avg_conf
            FROM snapshot_stats
            WHERE snapshot_id = ?
            ORDER BY clocks_sum DESC
            """,
            [snapshot_id],
        ).fetchall()

        if not rows_raw:
            # Fallback: compute from draw_calls + labels + metrics
            rows_raw = self._compute_fallback(conn, snapshot_id)
            source = "computed"
        else:
            source = "snapshot_stats"

        total_clocks = sum(r[2] for r in rows_raw)   # clocks_sum col
        total_dcs    = sum(r[1] for r in rows_raw)   # dc_count col

        rows = [
            {
                "category":   r[0],
                "dc_count":   r[1],
                "clocks_sum": r[2],
                "clocks_pct": r[3],
                "avg_conf":   r[4],
            }
            for r in rows_raw
        ]

        columns = [
            {"key": "category",   "label": "Category",       "type": "string"},
            {"key": "dc_count",   "label": "DC Count",       "type": "integer"},
            {"key": "clocks_sum", "label": "Total Clocks",   "type": "integer"},
            {"key": "clocks_pct", "label": "Clock %",        "type": "percent"},
            {"key": "avg_conf",   "label": "Avg Confidence", "type": "float"},
        ]

        summary = {
            "total_categories": len(rows),
            "total_dc_count":   total_dcs,
            "total_clocks":     total_clocks,
        }

        metadata = {
            "computed_at": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
            "source":      source,
        }

        return self._result(snapshot_id, columns, rows, summary, metadata=metadata)

    def _compute_fallback(self, conn, snapshot_id: int) -> list[tuple]:
        """Fallback when snapshot_stats is empty: compute from raw tables."""
        result = conn.execute(
            """
            SELECT
                COALESCE(lb.category, 'Unknown') AS category,
                COUNT(dc.api_id) AS dc_count,
                COALESCE(SUM(m.clocks), 0) AS clocks_sum,
                CASE
                    WHEN SUM(m.clocks) OVER () = 0 THEN 0.0
                    ELSE ROUND(100.0 * COALESCE(SUM(m.clocks), 0)
                               / NULLIF(SUM(m.clocks) OVER (), 0), 2)
                END AS clocks_pct,
                ROUND(AVG(COALESCE(lb.confidence, 0.0)), 4) AS avg_conf
            FROM draw_calls dc
            LEFT JOIN labels lb
                ON lb.snapshot_id = dc.snapshot_id AND lb.api_id = dc.api_id
            LEFT JOIN metrics m
                ON m.snapshot_id = dc.snapshot_id AND m.api_id = dc.api_id
            WHERE dc.snapshot_id = ?
            GROUP BY COALESCE(lb.category, 'Unknown')
            ORDER BY clocks_sum DESC
            """,
            [snapshot_id],
        ).fetchall()
        return result
```

#### 5. `pySdp/analysis/models/top_bottleneck_dcs.py`

```python
from __future__ import annotations
from datetime import datetime, timezone
from analysis.models.base import AnalysisModel
from data.model_registry import register

@register
class TopBottleneckDcs(AnalysisModel):
    name = "top_bottleneck_dcs"
    description = "Top N DrawCalls by GPU clock cost with label and metric details"
    params_schema = {
        "type": "object",
        "properties": {
            "top_n":     {"type": "integer", "default": 10, "minimum": 1, "maximum": 100},
            "category":  {"type": "string"},
        }
    }
    viz_type = "table"

    def run(self, db, snapshot_id: int, **params) -> dict:
        top_n    = int(params.get("top_n", 10))
        category = params.get("category")
        conn = db.conn()

        sql = """
            SELECT
                dc.api_id,
                dc.dc_id,
                dc.api_name,
                COALESCE(lb.category, 'Unknown')    AS category,
                COALESCE(lb.subcategory, '')        AS subcategory,
                COALESCE(lb.detail, '')             AS detail,
                ROUND(COALESCE(lb.confidence, 0.0), 4) AS confidence,
                m.clocks,
                m.read_total_bytes,
                m.write_total_bytes,
                m.fragments_shaded,
                m.shaders_busy_pct
            FROM draw_calls dc
            LEFT JOIN labels lb
                ON lb.snapshot_id = dc.snapshot_id AND lb.api_id = dc.api_id
            LEFT JOIN metrics m
                ON m.snapshot_id = dc.snapshot_id AND m.api_id = dc.api_id
            WHERE dc.snapshot_id = ?
        """
        params_list: list = [snapshot_id]
        if category:
            sql += " AND lb.category = ?"
            params_list.append(category)
        sql += " ORDER BY m.clocks DESC NULLS LAST LIMIT ?"
        params_list.append(top_n)

        raw = conn.execute(sql, params_list).fetchall()
        col_names = [
            "api_id", "dc_id", "api_name", "category", "subcategory",
            "detail", "confidence", "clocks", "read_total_bytes",
            "write_total_bytes", "fragments_shaded", "shaders_busy_pct",
        ]
        rows = [dict(zip(col_names, r)) for r in raw]

        columns = [
            {"key": "api_id",          "label": "API ID",        "type": "integer"},
            {"key": "api_name",        "label": "API Call",      "type": "string"},
            {"key": "category",        "label": "Category",      "type": "string"},
            {"key": "detail",          "label": "Detail",        "type": "string"},
            {"key": "clocks",          "label": "Clocks",        "type": "integer"},
            {"key": "shaders_busy_pct","label": "Shader Busy %", "type": "percent"},
            {"key": "fragments_shaded","label": "Fragments",     "type": "integer"},
            {"key": "read_total_bytes","label": "Read Bytes",    "type": "integer"},
        ]

        return self._result(
            snapshot_id, columns, rows,
            summary={"returned": len(rows), "top_n": top_n},
            metadata={"computed_at": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
                      "params": {"top_n": top_n, "category": category}},
        )
```

#### 6. `pySdp/analysis/models/label_quality.py`

```python
from __future__ import annotations
from datetime import datetime, timezone
from analysis.models.base import AnalysisModel
from data.model_registry import register

@register
class LabelQuality(AnalysisModel):
    name = "label_quality"
    description = "Label confidence distribution and reason-tag histogram"
    params_schema = {}
    viz_type = "bar_chart"

    def run(self, db, snapshot_id: int, **params) -> dict:
        conn = db.conn()

        # Confidence buckets
        buckets_raw = conn.execute(
            """
            SELECT
                CASE
                    WHEN confidence >= 0.9 THEN 'high (>=0.9)'
                    WHEN confidence >= 0.7 THEN 'medium (0.7-0.9)'
                    WHEN confidence >= 0.5 THEN 'low (0.5-0.7)'
                    ELSE 'very low (<0.5)'
                END AS bucket,
                COUNT(*) AS count
            FROM labels
            WHERE snapshot_id = ?
            GROUP BY bucket
            ORDER BY MIN(confidence) DESC
            """,
            [snapshot_id],
        ).fetchall()

        rows = [{"bucket": r[0], "count": r[1]} for r in buckets_raw]

        # Scalar summary
        summary_raw = conn.execute(
            """
            SELECT
                ROUND(AVG(confidence), 4),
                COUNT(*),
                SUM(CASE WHEN confidence < 0.6 THEN 1 ELSE 0 END)
            FROM labels WHERE snapshot_id = ?
            """,
            [snapshot_id],
        ).fetchone()

        avg_conf, total, low_count = (summary_raw or (0.0, 0, 0))

        columns = [
            {"key": "bucket", "label": "Confidence Bucket", "type": "string"},
            {"key": "count",  "label": "DC Count",          "type": "integer"},
        ]

        return self._result(
            snapshot_id, columns, rows,
            summary={
                "avg_confidence":        avg_conf,
                "total_labeled":         total,
                "low_confidence_count":  low_count,
                "low_confidence_ratio":  round(low_count / max(total, 1), 4),
            },
            metadata={"computed_at": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")},
        )
```

### Modify: `pySdp/webui/server.py`

Add model registration after `_db` is created:

```python
# After: _db = WorkspaceDB()
# Add:
import analysis.models   # triggers all @register decorators via __init__.py
from data import model_registry as _model_registry
```

### Add model endpoints to `pySdp/webui/routes/data.py`

```python
from data import model_registry as _model_registry

@router.get("/models")
def list_models():
    return {"ok": True, "data": _model_registry.list_models()}

@router.post("/models/{name}/run")
def run_model(
    name: str,
    snapshot_id: int = Query(...),
    params: dict = Body(default={}),
):
    try:
        result = _model_registry.run_model(name, db, snapshot_id, **params)
        return {"ok": True, "data": result}
    except KeyError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)
```

### Phase A Validation

```python
# Smoke test (no server required):
from data.db import WorkspaceDB
import analysis.models
from data.model_registry import run_model, list_models

db = WorkspaceDB(":memory:")
models = list_models()
assert any(m["name"] == "category_breakdown" for m in models)
assert any(m["name"] == "top_bottleneck_dcs" for m in models)
assert any(m["name"] == "label_quality" for m in models)

# With empty DB, category_breakdown should return empty rows without error
result = run_model("category_breakdown", db, 999)
assert result["model_name"] == "category_breakdown"
assert result["rows"] == []
print("Phase A validation passed")
```

---

## Phase B: Question CRUD

**Goal**: CRUD endpoints for the `questions` table. Seed builtin questions at startup.

### Builtin question seeding

Add `_seed_builtin_questions(db)` function, called from `server.py` after model
registration:

```python
_BUILTIN_QUESTIONS = [
    {
        "id":           "q_category_breakdown",
        "title":        "Category Breakdown",
        "model_name":   "category_breakdown",
        "model_params": {},
        "viz_type":     "bar_chart",
        "viz_config":   {"x_key": "category", "y_key": "clocks_pct",
                         "sort": "desc", "title": "Clock Budget by Category"},
        "is_builtin":   True,
    },
    {
        "id":           "q_top10_dcs",
        "title":        "Top 10 DrawCalls by Clock",
        "model_name":   "top_bottleneck_dcs",
        "model_params": {"top_n": 10},
        "viz_type":     "table",
        "viz_config":   {"title": "Top 10 DrawCalls by GPU Clock Cost"},
        "is_builtin":   True,
    },
    {
        "id":           "q_label_quality",
        "title":        "Label Quality",
        "model_name":   "label_quality",
        "model_params": {},
        "viz_type":     "bar_chart",
        "viz_config":   {"x_key": "bucket", "y_key": "count",
                         "title": "Label Confidence Distribution"},
        "is_builtin":   True,
    },
]

def _seed_builtin_questions(db: WorkspaceDB) -> None:
    conn = db.conn()
    now = datetime.now(timezone.utc).isoformat()
    for q in _BUILTIN_QUESTIONS:
        existing = conn.execute(
            "SELECT id FROM questions WHERE id=?", [q["id"]]
        ).fetchone()
        if not existing:
            conn.execute(
                "INSERT INTO questions VALUES (?,?,?,?,?,?,?,?)",
                [q["id"], q["title"], q["model_name"],
                 json.dumps(q["model_params"]), q["viz_type"],
                 json.dumps(q["viz_config"]), q["is_builtin"], now],
            )
```

### New endpoints (add to `webui/routes/data.py`)

```
GET  /api/data/questions
     → {ok, data: [{id, title, model_name, model_params, viz_type, viz_config, is_builtin, created_at}]}

POST /api/data/questions
     body: {title, model_name, model_params?, viz_type, viz_config?}
     → {ok, data: {id, ...}}

GET  /api/data/questions/{id}
     → {ok, data: {...}}  or 404

PUT  /api/data/questions/{id}
     body: {title?, model_params?, viz_type?, viz_config?}
     → {ok, data: {id, ...}}
     Note: is_builtin=True questions — title/viz_config may be updated; model_name cannot change

DELETE /api/data/questions/{id}
     → {ok}
     Note: is_builtin=True questions cannot be deleted (return 403)
```

### Phase B Validation

```python
# Route structure check
routes = [r.path for r in router.routes]
assert "/questions" in routes
assert "/questions/{id}" in routes
# Seeding idempotency: run twice, count stays the same
_seed_builtin_questions(db)
_seed_builtin_questions(db)
count = db.conn().execute("SELECT COUNT(*) FROM questions WHERE is_builtin=true").fetchone()[0]
assert count == 3
print("Phase B validation passed")
```

---

## Phase C: Question Run Endpoint

**Goal**: Run a question for a specific snapshot — dispatch to model, merge viz metadata.

### New endpoint

```
POST /api/data/questions/{id}/run?snapshot_id=<n>

Response:
{
  "ok": true,
  "data": {
    "question_id":   "q_category_breakdown",
    "title":         "Category Breakdown",
    "viz_type":      "bar_chart",
    "viz_config":    {...},
    "result":        {  ...model output dict...  }
  }
}
```

### Implementation in `webui/routes/data.py`

```python
@router.post("/questions/{question_id}/run")
def run_question(
    question_id: str,
    snapshot_id: int = Query(...),
):
    try:
        row = db.conn().execute(
            "SELECT id, title, model_name, model_params, viz_type, viz_config "
            "FROM questions WHERE id=?", [question_id]
        ).fetchone()
        if row is None:
            return JSONResponse({"ok": False, "error": f"Question '{question_id}' not found"}, status_code=404)

        q_id, title, model_name, model_params_str, viz_type, viz_config_str = row
        model_params = json.loads(model_params_str or "{}")
        viz_config   = json.loads(viz_config_str or "{}")

        result = _model_registry.run_model(model_name, db, snapshot_id, **model_params)

        # Merge viz metadata from question into result
        result["viz_type"]   = viz_type      # question overrides model default
        result["viz_config"] = viz_config

        return {
            "ok": True,
            "data": {
                "question_id": q_id,
                "title":       title,
                "viz_type":    viz_type,
                "viz_config":  viz_config,
                "result":      result,
            },
        }
    except KeyError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)
```

### Phase C Validation

```python
# With test data ingested and labels generated:
# POST /api/data/questions/q_category_breakdown/run?snapshot_id=1
# Response should have ok=True, data.result.model_name == "category_breakdown"
# data.viz_type == "bar_chart"
# data.result.rows is a list
print("Phase C validation: call run_question in-process with test db")
result = run_question_logic(db, "q_category_breakdown", snapshot_id=test_snapshot_id)
assert result["ok"]
assert result["data"]["viz_type"] == "bar_chart"
assert isinstance(result["data"]["result"]["rows"], list)
```

---

## Phase D: Dashboard CRUD + Run

**Goal**: Named ordered list of questions; running a dashboard runs all questions in order.

### Builtin dashboard seeding

```python
_BUILTIN_DASHBOARDS = [
    {
        "id":           "dash_default",
        "title":        "Default Dashboard",
        "question_ids": ["q_category_breakdown", "q_top10_dcs", "q_label_quality"],
    },
]
```

Seed at startup: `_seed_builtin_dashboards(db)` — same idempotent INSERT OR IGNORE pattern.

### New endpoints

```
GET  /api/data/dashboards
     → {ok, data: [{id, title, question_ids, created_at, updated_at}]}

POST /api/data/dashboards
     body: {title, question_ids: [str, ...]}
     → {ok, data: {id, ...}}

GET  /api/data/dashboards/{id}
     → {ok, data: {...}}

PUT  /api/data/dashboards/{id}
     body: {title?, question_ids?}
     → {ok, data: {...}}

DELETE /api/data/dashboards/{id}
     → {ok}
     Note: builtin dashboards return 403

POST /api/data/dashboards/{id}/run?snapshot_id=<n>
     → {
         ok: true,
         data: {
           dashboard_id: str,
           title: str,
           snapshot_id: int,
           panels: [
             {
               question_id: str,
               title: str,
               viz_type: str,
               viz_config: {...},
               result: {...}   # or error: str if model run fails
             },
             ...
           ]
         }
       }
```

### Dashboard run implementation

```python
@router.post("/dashboards/{dashboard_id}/run")
def run_dashboard(dashboard_id: str, snapshot_id: int = Query(...)):
    row = db.conn().execute(
        "SELECT id, title, question_ids FROM dashboards WHERE id=?",
        [dashboard_id]
    ).fetchone()
    if row is None:
        return JSONResponse({"ok": False, "error": "Dashboard not found"}, status_code=404)

    dash_id, title, question_ids_str = row
    question_ids = json.loads(question_ids_str or "[]")

    panels = []
    for qid in question_ids:
        try:
            qrow = db.conn().execute(
                "SELECT id, title, model_name, model_params, viz_type, viz_config "
                "FROM questions WHERE id=?", [qid]
            ).fetchone()
            if qrow is None:
                panels.append({"question_id": qid, "error": f"Question '{qid}' not found"})
                continue
            _, qtitle, model_name, model_params_str, viz_type, viz_config_str = qrow
            model_params = json.loads(model_params_str or "{}")
            viz_config   = json.loads(viz_config_str or "{}")
            result = _model_registry.run_model(model_name, db, snapshot_id, **model_params)
            result["viz_type"]   = viz_type
            result["viz_config"] = viz_config
            panels.append({
                "question_id": qid,
                "title":       qtitle,
                "viz_type":    viz_type,
                "viz_config":  viz_config,
                "result":      result,
            })
        except Exception as exc:
            panels.append({"question_id": qid, "error": str(exc)})

    return {
        "ok": True,
        "data": {
            "dashboard_id": dash_id,
            "title":        title,
            "snapshot_id":  snapshot_id,
            "panels":       panels,
        },
    }
```

Note: Dashboard run is non-fatal per panel — a failing model produces `{"error": "..."}` in
that panel slot instead of aborting the entire response.

### Phase D Validation

```python
_seed_builtin_dashboards(db)
count = db.conn().execute("SELECT COUNT(*) FROM dashboards WHERE id='dash_default'").fetchone()[0]
assert count == 1
# Run default dashboard with test snapshot
result = run_dashboard_logic(db, "dash_default", snapshot_id=test_snapshot_id)
assert result["ok"]
assert len(result["data"]["panels"]) == 3
print("Phase D validation passed")
```

---

## File Map Summary

| File | Action | Phase |
|---|---|---|
| pySdp/analysis/models/__init__.py | NEW | A |
| pySdp/analysis/models/base.py | NEW | A |
| pySdp/analysis/models/category_breakdown.py | NEW | A |
| pySdp/analysis/models/top_bottleneck_dcs.py | NEW | A |
| pySdp/analysis/models/label_quality.py | NEW | A |
| pySdp/data/model_registry.py | NEW | A |
| pySdp/webui/server.py | MODIFIED (model registration + seeding calls) | A+B+D |
| pySdp/webui/routes/data.py | MODIFIED (model + question + dashboard endpoints) | A+B+C+D |

No changes to: `data/db.py`, `data/ingest.py`, `data/query.py`,
`analysis/status_service.py`, `analysis/label_service.py`.

---

## Dependency Layer Clarification

```
server.py  →  analysis/models/__init__.py  →  analysis/models/category_breakdown.py
                                           →  analysis/models/top_bottleneck_dcs.py
                                           →  analysis/models/label_quality.py
analysis/models/*.py  →  analysis/models/base.py
analysis/models/*.py  →  data/model_registry.py  (for @register)
data/model_registry.py  →  analysis/models/base.py  (for type hint only)
webui/routes/data.py  →  data/model_registry.py  (list_models, run_model)
```

The `data/model_registry.py` → `analysis/models/base.py` import is a type-only reference
(the `type[AnalysisModel]` annotation). This does NOT create a circular dependency because
`base.py` has zero imports from `data/`.

---

## Alternatives Considered

### Alternative 1: Put model_registry in analysis/ instead of data/

- Pros: Clearer separation (registry with models)
- Cons: `webui/routes/data.py` would import from `analysis/` to dispatch models; the
  HTTP dispatch layer (`data.py`) should not reach into `analysis/` logic. `data/` as
  the registry keeps the HTTP layer's dependency on `data/` only.
- Decision: Keep registry in `data/` as a pure dispatch table.

### Alternative 2: Each model as a function, not a class

- Pros: Simpler — no ABC needed
- Cons: Cannot store metadata (`name`, `description`, `params_schema`, `viz_type`) on
  a function without setattr hacks. Class makes introspection clean via ClassVar.
- Decision: Use ABC with ClassVar.

### Alternative 3: Cache model results in a `model_results` table

- Pros: Faster repeated queries
- Cons: Cache invalidation complexity when labels are refreshed; DuckDB sub-ms for
  small scan sizes; adds a new table and cache-miss logic
- Decision: No caching for MVP. Add if profiling shows need.

### Alternative 4: Question `viz_config` as a typed Pydantic model

- Pros: Validation at create time
- Cons: Different models need different viz_config shapes; TEXT JSON is simpler and
  passes through to frontend unchanged
- Decision: TEXT JSON.

---

## Risks

### Risk 1: Import circularity at registration time
`analysis/models/category_breakdown.py` imports `@register` from `data/model_registry.py`.
`data/model_registry.py` imports `AnalysisModel` from `analysis/models/base.py`.
`analysis/models/base.py` has no imports from `data/`.
This chain is acyclic. Risk is LOW.

### Risk 2: snapshot_stats empty for a snapshot
If a snapshot is ingested but `generate_status_json` has not been run, `category_breakdown`
falls back to a JOIN query. The fallback window function `SUM(m.clocks) OVER ()` must be
verified for DuckDB syntax. Risk is MEDIUM — verify during implementation.

### Risk 3: DuckDB in-memory for tests may differ from file-backed behavior
The validation script uses `WorkspaceDB(":memory:")`. DuckDB accepts `:memory:` as a valid
path string for an in-memory database. Risk is LOW — confirmed behavior.

### Risk 4: `question_ids` order in dashboards
`question_ids TEXT NOT NULL DEFAULT '[]'` stores a JSON array. JSON arrays preserve order,
so dashboard panel order is stable. Risk is LOW.

---

## Validation Summary

| Phase | Validation |
|---|---|
| A | Import test: list_models() returns 3 models; run_model("category_breakdown", db, 999) returns empty rows without error |
| B | Seeding idempotency; CRUD route structure check; DELETE builtin returns 403 |
| C | run_question for q_category_breakdown returns ok=True, viz_type=bar_chart, result.rows is list |
| D | Seeding idempotency; run_dashboard for dash_default returns 3 panels; one panel error does not abort response |

---

## Implementation Notes for Executor Agent

1. Create `pySdp/analysis/models/` directory first.
2. Create `base.py` before any other model file (dependency chain starts here).
3. `model_registry.py` goes in `pySdp/data/` — not in `analysis/`.
4. In `server.py`, add `import analysis.models` AFTER `_db = WorkspaceDB()`.
5. Add seeding calls (`_seed_builtin_questions`, `_seed_builtin_dashboards`) AFTER
   model registration in `server.py`.
6. All new routes go inside the existing `make_router(db)` factory in `data.py`.
7. For Phase C and D, import `json` at the top of `data.py` (may already be present).
8. The `Body(default={})` import for Phase C requires `from fastapi import Body` in `data.py`.
9. Dashboard run should catch per-panel exceptions individually — do NOT let one failing
   model abort the entire dashboard response.

---

Implementation requires the executor agent (/execute).
