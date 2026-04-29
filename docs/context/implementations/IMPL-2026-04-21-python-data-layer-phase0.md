# IMPL-2026-04-21-python-data-layer-phase0.md

## Topic
Python project restructure Phase 0 — move `webui/analysis/` to top-level `analysis/`, fix imports, verify server starts without ImportError.

## Status
completed

## Based On
- PLAN-2026-04-21-python-data-layer.md (Phase 0)

## What Was Done

### Files Created
- `pySdp/analysis/__init__.py` — empty package marker (`# analysis layer`)
- `pySdp/analysis/label_service.py` — moved from `pySdp/webui/analysis/label_service.py`
- `pySdp/analysis/status_service.py` — moved from `pySdp/webui/analysis/status_service.py`
- `pySdp/analysis/topdc_service.py` — moved from `pySdp/webui/analysis/topdc_service.py`
- `pySdp/analysis/dashboard_service.py` — moved from `pySdp/webui/analysis/dashboard_service.py`
- `pySdp/analysis/analysis_md_service.py` — moved from `pySdp/webui/analysis/analysis_md_service.py`

### Files Modified
- `pySdp/webui/server.py` — added one line after the existing `sys.path.insert(0, webui_dir)`:
  ```python
  sys.path.insert(0, str(Path(__file__).parent.parent))
  ```
  This inserts `pySdp/` root into `sys.path` so that `import analysis` resolves to `pySdp/analysis/` instead of the old `pySdp/webui/analysis/`.

### Files Deleted
- `pySdp/webui/analysis/` directory and all contents (5 .py files + `__init__.py` + `__pycache__/`)

### Files Unchanged
- `pySdp/webui/routes/files.py` — import lines (`from analysis.label_service import ...` etc.) remain identical; they now resolve via the new sys.path.

## Validation
- `ls pySdp/analysis/` confirms all 6 files present (`__init__.py` + 5 services).
- `ls pySdp/webui/analysis/` fails — directory does not exist.
- `python -c "import sys; sys.path.insert(0, 'pySdp'); import analysis; print(analysis.__file__)"` prints `pySdp/analysis/__init__.py` — correct resolution.

## Deviations From Plan
None. All 5 steps executed exactly as specified.

## Notes
- No business logic was changed in any of the 5 service files.
- The `analysis/` package is now a peer of `webui/`, `pysdp/`, and `data/` (future) under `pySdp/`.
- Phase 1 (WorkspaceDB + ingest) can proceed from this clean base.
