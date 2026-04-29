---
type: implementation
topic: Python data layer Phase 3 — label/stats persistence to DuckDB + refresh_labels endpoint
status: completed
based_on:
  - PLAN-2026-04-21-python-data-layer.md
  - IMPL-2026-04-21-python-data-layer-phase2.md
related_paths:
  - pySdp/analysis/label_service.py
  - pySdp/analysis/status_service.py
  - pySdp/webui/routes/files.py
  - pySdp/webui/routes/data.py
  - pySdp/webui/server.py
tags:
  - python
  - data-layer
  - duckdb
  - label
  - persistence
  - phase3
last_updated: 2026-04-21
---

# IMPL-2026-04-21: Python Data Layer Phase 3

## Summary

Phase 3 wires the analysis services to DuckDB so that label and stats outputs are
persisted to the DB at generation time, and adds a `refresh_labels` endpoint to
re-run and re-persist labels for any previously-ingested snapshot.

---

## Changes made

### 1. `pySdp/analysis/label_service.py`

- `generate_label_json(snapshot_dir, db=None)` — added optional `db` parameter.
- After writing `label.json`, if `db is not None`, calls `_persist_labels_to_db()`.
- `_persist_labels_to_db()` private helper:
  - Ensures snapshot row exists (INSERT OR REPLACE if absent).
  - Ensures draw_call rows exist (INSERT OR REPLACE from dc.json).
  - Upserts all labels via `executemany` into `labels` table with 11-column INSERT OR REPLACE.
  - `reason_tags` stored as `json.dumps(list)`.
  - `bottleneck_text` and `embedding` are NULL (reserved for Phase 4/5).
  - `labeled_at` is UTC ISO timestamp.
- No WorkspaceDB import at module level — `db` is duck-typed (any object with `.conn()`).
- Without `db`, function behaves identically to before (backward compat preserved).

### 2. `pySdp/analysis/status_service.py`

- `generate_status_json(snapshot_dir, db=None)` — added optional `db` parameter.
- `_persist_stats_to_db()` private helper added above the public function:
  - Upserts per-category rows into `snapshot_stats` via `executemany`.
  - `computed_at` is UTC ISO timestamp.
- In the `by_category` loop, added `avg_cat_conf` computation:
  ```python
  confs = [d["label"].get("confidence", 0.0) for d in cat_dcs]
  avg_cat_conf = round(sum(confs)/len(confs), 4) if confs else 0.0
  ```
- `_avg_conf` stored internally in `entry` dict, used for DB write, then stripped before JSON
  serialization (`entry.pop("_avg_conf", None)`).
- Without `db`, function behaves identically to before (backward compat preserved).

### 3. `pySdp/webui/routes/files.py`

- Refactored from module-level `router = APIRouter()` to `make_router(db=None) -> APIRouter`
  factory function.
- All 9 existing routes (`/sdp`, `/results`, `/analyses`, `/read`, `/label`, `/status`,
  `/topdc`, `/dashboard`, `/analysis_md`) preserved inside the factory as closures capturing `db`.
- POST `/label` now calls `generate_label_json(snapshot_dir, db=db)`.
- POST `/status` now calls `generate_status_json(snapshot_dir, db=db)`.
- When `db=None` (default), behavior is identical to the prior flat `router` pattern.

### 4. `pySdp/webui/server.py`

- Import updated: `from routes.files import make_router as _make_files_router`.
- Router registration updated: `app.include_router(_make_files_router(db=_db), prefix="/api/files", tags=["files"])`.
- `_db` (the global `WorkspaceDB` singleton created earlier in server.py) is passed to the
  files router so all label/status writes automatically persist to DuckDB.

### 5. `pySdp/webui/routes/data.py`

- Added imports: `generate_label_json` from `analysis.label_service`,
  `generate_status_json` from `analysis.status_service`.
- Added `POST /refresh_labels?snapshot_id=<n>` endpoint inside `make_router(db)`:
  1. Queries `snapshots` for `snapshot_dir`. Returns 404 if not found.
  2. Calls `generate_label_json(snapshot_dir, db=db)`.
  3. Calls `generate_status_json(snapshot_dir, db=db)`.
  4. Returns `{"ok": True, "snapshot_id": n, "snapshot_dir": snapshot_dir}`.

---

## Validation

Both validation scripts passed:

**Core DB write validation:**
```
label.json written: label.json
Labels in DB: [(100, 'Scene')]
status.json written: snapshot_42_status.json
snapshot_stats in DB: [('Scene', 1)]
Phase 3 validation passed
```

**Route structure validation:**
```
data routes: ['/ingest', '/snapshots', '/draw_calls', '/dc/{api_id}', '/refresh_labels']
files routes: ['/sdp', '/results', '/analyses', '/read', '/label', '/status', '/topdc', '/dashboard', '/analysis_md']
All route validations OK
```

**Backward compat (no db):**
```
label ok (no db): label.json
status ok (no db): snapshot_99_status.json
Backward compat OK
```

---

## Notes

- The FK chain `snapshots → draw_calls → labels` is satisfied by `_persist_labels_to_db`
  which writes both the snapshot row (if absent) and draw_call rows before inserting labels.
- `_avg_conf` is a private internal key on the entry dict — it does not appear in the
  written JSON because it is stripped before serialization.
- `refresh_labels` is intentionally stateless beyond reading `snapshot_dir` from the DB —
  it re-reads dc.json, metrics.json, and label.json from disk each time it runs.
- The `status_service` requires `metrics.json` to exist. If absent, it raises
  `FileNotFoundError` which `refresh_labels` propagates as a 404 response.
