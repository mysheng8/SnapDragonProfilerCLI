---
type: finding
topic: Analysis Model + Question Engine design — AnalysisModel interface, category_breakdown model, Question CRUD, Dashboard
status: investigated
related_paths:
  - pySdp/analysis/status_service.py
  - pySdp/analysis/label_service.py
  - pySdp/analysis/topdc_service.py
  - pySdp/analysis/dashboard_service.py
  - pySdp/data/query.py
  - pySdp/data/db.py
  - pySdp/data/model_registry.py       (NOT YET CREATED — Phase 6)
  - pySdp/analysis/models/             (NOT YET CREATED — Phase 6)
  - pySdp/webui/routes/data.py
related_tags:
  - python
  - analysis-model
  - question-engine
  - dashboard
  - duckdb
  - category-breakdown
  - viz-type
  - model-registry
summary: |
  Full investigation of status_service output shape, what is already in DB vs what must
  be computed on-demand, proposed AnalysisModel interface and model_registry design,
  concrete category_breakdown model spec with DB queries and output shape, Question-Model
  binding design, and gap analysis for Phase 6/7 implementation.
last_updated: 2026-04-21
---

# Finding: Analysis Model + Question Engine Design

## Problem Statement

Phases 0-3 of the Python data layer are complete. DuckDB now holds persistent tables
including `snapshot_stats`, `labels`, `metrics`, `draw_calls`, `questions`, and `dashboards`.
The `questions` and `dashboards` tables are in the schema but empty and unused.

The task is to design the Phase 6 Analysis Model Registry and Phase 7 Question Engine,
using `status_service` as the reference case for the first concrete model
(`category_breakdown`).

---

## Evidence

### 1. What status_service currently produces

`generate_status_json(snapshot_dir, db=None)` reads two files:
- `label.json` — dc_id → label (category, subcategory, confidence, reason_tags)
- `metrics.json` — dc_id → metrics dict (clocks, read_total_bytes, etc.)

It produces four top-level output sections written to `snapshot_{id}_status.json`:

#### 1a. `overall` block
Scalar totals across all DCs:
```
total_dc_count, draw_dc_count, compute_dc_count,
total_clocks, total_read_bytes, total_write_bytes,
total_fragments_shaded, total_vertices_shaded,
metrics_coverage_ratio
```

#### 1b. `category_stats` list
Per-category aggregates (one entry per category, sorted alphabetically):
```
category, dc_count, percentage, clocks_sum, clocks_pct,
metrics_p50, metrics_p60, metrics_p70, metrics_p80,
metrics_p90, metrics_p95, metrics_p99
```
Each `metrics_pXX` is a dict keyed by metric name with the pXX value at that level.
Also computes `_avg_conf` (average label confidence per category) — stripped before JSON
but persisted to `snapshot_stats.avg_conf`.

#### 1c. `label_stats` block
```
avg_confidence, low_confidence_ratio, low_confidence_threshold,
llm_labeled_count, rule_labeled_count,
reason_tag_distribution (top-N by count),
low_confidence_dc_ids (up to 20 dc_ids)
```

#### 1d. `global_percentiles` block
For every metric key present across all DCs: full percentile block (p50..p99).
This is the most compute-intensive part — it loops over all DCs to build per-metric
sorted value arrays.

---

### 2. What is already in DB vs what must be computed on-demand

| Data | In DB | Table | Notes |
|---|---|---|---|
| Per-category dc_count | YES | snapshot_stats.dc_count | Written by status_service Phase 3 |
| Per-category clocks_sum | YES | snapshot_stats.clocks_sum | Written by status_service Phase 3 |
| Per-category clocks_pct | YES | snapshot_stats.clocks_pct | Written by status_service Phase 3 |
| Per-category avg_conf | YES | snapshot_stats.avg_conf | Written by status_service Phase 3 |
| Per-DC category | YES | labels.category | From Phase 3 label persistence |
| Per-DC confidence | YES | labels.confidence | From Phase 3 label persistence |
| Per-DC clocks | YES | metrics.clocks | From Phase 1 ingest |
| Per-DC all metrics | YES | metrics.* | 13 known columns from Phase 1 |
| Percentile blocks (p50..p99 per category per metric) | NO | — | Computed by status_service, written to JSON only |
| Global percentiles (p50..p99 per metric, all DCs) | NO | — | Computed in memory, not persisted to DB |
| label_stats block | NO | — | avg_conf is in snapshot_stats but other fields (low_conf_ratio, tag_dist, low_conf_dc_ids) are not |
| overall totals | NO | — | Can be derived: SUM(metrics.clocks), COUNT(draw_calls), but not stored |

**Key insight**: `snapshot_stats` stores 5 columns per category (dc_count, clocks_sum,
clocks_pct, avg_conf, computed_at). The percentile blocks (7 percentile levels × N metrics
= potentially 100+ values per category) are NOT stored in the DB and must be recomputed
or read from the JSON file.

---

### 3. DB vs File trade-off for category_breakdown model

Two approaches for the `category_breakdown` model:

**Option A: Read from DB only (snapshot_stats)**
- Query `snapshot_stats` for dc_count, clocks_sum, clocks_pct, avg_conf per category
- No re-reading JSON files
- Output is limited: no percentile blocks, no per-DC list

**Option B: Recompute from draw_calls + labels + metrics in DB**
- Query `draw_calls JOIN labels JOIN metrics` for the snapshot
- Compute percentile blocks in Python from DB rows
- Same computation as status_service but DB-sourced data instead of JSON files
- Richer output but requires scanning all DC rows

**Recommended**: Start with Option A for the minimal `category_breakdown` model
(reads snapshot_stats directly). A separate `category_percentiles` model can offer
Option B computation when deeper analysis is needed.

This is consistent with Phase 6 plan intent: the model reads from DB, not from files.

---

### 4. Current analysis services — file dependency map

| Service | Reads from files | Writes to DB | Can be replaced by DB query? |
|---|---|---|---|
| status_service | label.json + metrics.json | snapshot_stats | Partially (stats: YES; percentiles: NO without recompute) |
| label_service | dc.json | labels | YES — labels table has all fields |
| topdc_service | label.json + metrics.json + status.json + attribution_rules.json | None | Partially (data: YES; rules: need file) |
| dashboard_service | label.json + metrics.json + status.json | None | YES (all data available in DB) |

---

### 5. Proposed AnalysisModel interface

An `AnalysisModel` is a named, parameterized computation that:
- Takes `(db, snapshot_id, **params)` as input
- Reads exclusively from DuckDB (no JSON file I/O)
- Returns a structured dict with a `viz_type` hint

The interface is a duck-typed Protocol (Python 3.8+ compatible) or an ABC:

```python
# analysis/models/base.py

from typing import Any, ClassVar

class AnalysisModel:
    name: ClassVar[str]          # unique registry key, e.g. "category_breakdown"
    description: ClassVar[str]   # human-readable summary
    params_schema: ClassVar[dict]  # JSON Schema dict for params validation
    viz_type: ClassVar[str]      # default viz_type hint: "table" | "bar_chart" | "pie_chart" | ...

    def run(self, db, snapshot_id: int, **params) -> dict:
        """
        Execute the model and return a structured result dict.

        Returns:
          {
            "model_name": str,
            "snapshot_id": int,
            "viz_type": str,          # from ClassVar or overridden per-run
            "columns": [...],         # for table/chart: ordered list of column defs
            "rows": [...],            # data rows
            "summary": {...},         # optional scalar summary fields
            "metadata": {...},        # optional provenance (computed_at, params used)
          }
        """
        raise NotImplementedError
```

**Key design decisions**:
- `ClassVar` attributes (not instance vars) for metadata — allows `list_models()` to
  introspect without instantiating
- `run()` accepts `**params` so calling code does not need to know the param names
- Output dict has a fixed envelope (`model_name`, `snapshot_id`, `viz_type`, `columns`,
  `rows`, `summary`) but `rows` shape is model-specific
- No caching in the interface itself — caching is a registry concern

---

### 6. model_registry.py design

```python
# data/model_registry.py

_REGISTRY: dict[str, type[AnalysisModel]] = {}

def register(model_cls):
    """Decorator or callable to register an AnalysisModel subclass."""
    _REGISTRY[model_cls.name] = model_cls
    return model_cls

def list_models() -> list[dict]:
    """Return metadata dicts for all registered models."""
    return [
        {
            "name": cls.name,
            "description": cls.description,
            "params_schema": cls.params_schema,
            "viz_type": cls.viz_type,
        }
        for cls in _REGISTRY.values()
    ]

def run_model(name: str, db, snapshot_id: int, **params) -> dict:
    """Run a registered model by name. Raises KeyError if not found."""
    if name not in _REGISTRY:
        raise KeyError(f"Model '{name}' not registered")
    return _REGISTRY[name]().run(db, snapshot_id, **params)
```

Models register themselves at module import time via the `@register` decorator.
`data/model_registry.py` imports all model modules in `analysis/models/` to trigger
registration at server startup.

---

### 7. category_breakdown model — concrete spec

**Name**: `category_breakdown`
**Description**: Per-category DrawCall count, clock budget, and label confidence
**Viz type**: `bar_chart` (primary), `table` (secondary)
**Params schema**: `{}` (no params required, snapshot_id is always required separately)

**DB queries** (reads from snapshot_stats and labels):

Query 1 — per-category stats from snapshot_stats:
```sql
SELECT category, dc_count, clocks_sum, clocks_pct, avg_conf
FROM snapshot_stats
WHERE snapshot_id = ?
ORDER BY clocks_sum DESC
```

Query 2 — total DC count from draw_calls (for percentage calculation fallback):
```sql
SELECT COUNT(*) FROM draw_calls WHERE snapshot_id = ?
```

Query 3 — total clocks from metrics (for percentage calculation if snapshot_stats absent):
```sql
SELECT COALESCE(SUM(clocks), 0) FROM metrics WHERE snapshot_id = ?
```

**Output shape**:
```json
{
  "model_name": "category_breakdown",
  "snapshot_id": 42,
  "viz_type": "bar_chart",
  "columns": [
    {"key": "category",   "label": "Category",    "type": "string"},
    {"key": "dc_count",   "label": "DC Count",    "type": "integer"},
    {"key": "clocks_sum", "label": "Total Clocks","type": "integer"},
    {"key": "clocks_pct", "label": "Clock %",     "type": "percent"},
    {"key": "avg_conf",   "label": "Avg Confidence","type": "float"}
  ],
  "rows": [
    {"category": "Scene",       "dc_count": 45, "clocks_sum": 1200000, "clocks_pct": 42.5, "avg_conf": 0.65},
    {"category": "PostProcess", "dc_count": 12, "clocks_sum":  800000, "clocks_pct": 28.3, "avg_conf": 0.70},
    ...
  ],
  "summary": {
    "total_categories": 5,
    "total_dc_count": 100,
    "total_clocks": 2823000
  },
  "metadata": {
    "computed_at": "2026-04-21T10:00:00Z",
    "source": "snapshot_stats"
  }
}
```

**Fallback behavior**: If `snapshot_stats` is empty for the given snapshot_id (labels not
yet generated), the model falls back to raw counts from `draw_calls` and clocks from
`metrics`, grouping by `labels.category` via a JOIN. This is more expensive but always
produces a result.

---

### 8. Additional built-in models (Phase 6 scope)

| Model name | Reads from | Viz type | Key output |
|---|---|---|---|
| `category_breakdown` | snapshot_stats (or draw_calls+labels+metrics JOIN) | bar_chart | Category DC/clock distribution |
| `top_bottleneck_dcs` | draw_calls JOIN labels JOIN metrics ORDER BY clocks DESC | table | Top-N DCs by clock cost |
| `texture_hotspots` | draw_calls JOIN dc_textures JOIN textures JOIN metrics | table | Textures used by highest-clock DCs |
| `label_quality` | labels | bar_chart | Confidence distribution, reason_tag histogram |

All models use only DB reads — no JSON file access.

---

### 9. Question ↔ Model binding design

A **Question** is a saved, named binding of:
- `model_name` (FK to registered model)
- `model_params` (JSON dict — override defaults for the model)
- `viz_type` (may override the model default)
- `viz_config` (JSON dict — chart-level display config: axis labels, colors, title)
- `title` (human-readable display name)
- `is_builtin` (bool — builtin questions are seeded at startup, not user-created)

Running a question for a snapshot:
```
run_model(model_name, db, snapshot_id, **model_params)
  → structured result dict
  → apply viz_type + viz_config from Question row
  → return result with merged viz metadata
```

Questions are persisted in the `questions` table (already in schema).

**Builtin questions** (seeded at startup if not present):
- `q_category_breakdown` — Category Breakdown (bar_chart)
- `q_top10_dcs` — Top 10 DrawCalls by Clock (table)
- `q_label_quality` — Label Quality Stats (bar_chart)

---

### 10. Dashboard = ordered list of questions

A **Dashboard** is a named ordered list of question IDs stored in the `dashboards` table
(`question_ids TEXT NOT NULL DEFAULT '[]'` — JSON array of question id strings).

Running a dashboard for a snapshot:
- Load dashboard row → parse `question_ids`
- For each question_id, load the question row, run its model
- Return list of `{question_id, title, result}` in order

**Builtin dashboards** (seeded at startup if not present):
- `dash_default` — "Default Dashboard" — contains the 3 builtin questions in order

---

### 11. Viz types to support (minimal first iteration)

| viz_type | Description | Required columns fields |
|---|---|---|
| `table` | Plain tabular display | `columns` + `rows` |
| `bar_chart` | Horizontal or vertical bar chart | `columns` + `rows`; x-axis key + y-axis key in `viz_config` |
| `pie_chart` | Proportional breakdown | `columns` + `rows`; label key + value key in `viz_config` |
| `number` | Single scalar KPI | `summary` field only |

`viz_config` is a JSON dict stored in the question row, passed through to the frontend
unchanged. Example for bar_chart:
```json
{"x_key": "category", "y_key": "clocks_pct", "sort": "desc", "title": "Clock Budget by Category"}
```

---

### 12. Caching strategy

For Phase 6/7 (MVP), **no result caching**. Every `run` call recomputes from DB.

Rationale:
- DuckDB columnar scans of thousands of rows complete in milliseconds
- snapshot_stats already pre-aggregates the most expensive computation
- Caching complicates invalidation (refresh_labels invalidates stats)
- Can add caching later if profiling shows a need

---

## Analysis

### Gap 1: snapshot_stats may be empty

If a snapshot was ingested via `POST /api/data/ingest` but `generate_status_json` has not
been run (label and status not persisted via Phase 3), `snapshot_stats` will be empty for
that snapshot. The `category_breakdown` model must handle this fallback gracefully.

The fallback path queries `draw_calls JOIN labels JOIN metrics` directly — same data, more
expensive but always available if labels exist. If labels also do not exist, the model
returns an empty result set with a `warning` field.

### Gap 2: analysis/models/ directory does not exist yet

The `pySdp/analysis/models/` directory is not created. The plan calls for it in Phase 6.
It will contain:
- `__init__.py`
- `base.py` (AnalysisModel ABC)
- `category_breakdown.py`
- `top_bottleneck_dcs.py`
- `label_quality.py`

### Gap 3: model_registry.py does not exist yet

`pySdp/data/model_registry.py` is not created. It is the Phase 6 file.

### Gap 4: questions and dashboards tables are empty

The `questions` and `dashboards` tables exist in the schema (created by `ensure_schema()`)
but contain zero rows. Builtin questions and dashboards must be seeded at server startup.

### Gap 5: webui/routes/data.py needs 6 new route groups

Current `data.py` has 5 routes: `/ingest`, `/snapshots`, `/draw_calls`, `/dc/{api_id}`,
`/refresh_labels`. Phase 6+7 adds:
- `GET /api/data/models`
- `POST /api/data/models/{name}/run?snapshot_id=<n>`
- `GET/POST /api/data/questions`
- `GET/PUT/DELETE /api/data/questions/{id}`
- `POST /api/data/questions/{id}/run?snapshot_id=<n>`
- `GET/POST /api/data/dashboards`
- `GET/PUT/DELETE /api/data/dashboards/{id}`
- `POST /api/data/dashboards/{id}/run?snapshot_id=<n>`

### Gap 6: Import chain for model registration

`data/model_registry.py` must import `analysis/models/*.py` at module load time to
trigger `@register` decorations. Since `data/` must not import `analysis/` directly
(it would create a circular dependency or violate the layering rule `data/ has no
knowledge of analysis/`), the registration must happen at the `webui/server.py` level:

```python
# In server.py, after _db is created:
import data.model_registry as _model_registry
import analysis.models.category_breakdown    # triggers @register
import analysis.models.top_bottleneck_dcs    # triggers @register
```

This keeps `data/model_registry.py` as a pure dispatch registry with no knowledge of
which models exist, while `server.py` acts as the composition root that wires them.

Alternatively, `analysis/models/__init__.py` can do all imports so a single
`import analysis.models` in `server.py` registers everything.

### Gap 7: Question id generation

Questions need a stable `id` (TEXT PRIMARY KEY). Use `uuid4()` for user-created questions.
Builtin question IDs use deterministic names (`q_category_breakdown`, etc.) so seeding
is idempotent across server restarts.

---

## Impact

Phase 6 (Analysis Model Registry) + Phase 7 (Question Engine + Dashboard) are the
final two phases of the Python data layer plan. Once implemented:
- The frontend can dynamically render any analysis by calling a named model
- Users can create custom questions from any model with custom params
- Dashboards enable multi-panel analysis views
- The architecture is extensible: adding a new model = one new file in analysis/models/

---

## Related Context
- Related findings: FINDING-2026-04-21-python-data-layer-design.md
- Related plans: PLAN-2026-04-21-python-data-layer.md (Phase 6 + 7)
- Related implementations:
  - IMPL-2026-04-21-python-data-layer-phase3.md (label/stats persistence — current state)
  - IMPL-2026-04-21-python-data-layer-phase2.md (query layer — current state)
  - IMPL-2026-04-21-python-data-layer-phase1.md (schema + ingest — current state)
