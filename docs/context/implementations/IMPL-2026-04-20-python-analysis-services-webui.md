---
type: implementation
topic: Python WebUI analysis services — label/status/topdc/dashboard/analysis_md + Results tab
status: completed
related_plan: none (direct port of C# services)
related_findings:
  - docs/context/findings/FINDING-2026-04-15-raw-data-schema.md
  - docs/context/findings/FINDING-2026-04-15-server-api-state-audit.md
iteration_count: 1
build_validated: N/A (Python, no build step)
test_validated: N/A
files_modified:
  - pySdp/webui/routes/files.py
  - pySdp/webui/static/app.js
  - pySdp/webui/static/index.html
  - pySdp/webui/static/style.css
files_created:
  - pySdp/webui/analysis/__init__.py
  - pySdp/webui/analysis/label_service.py
  - pySdp/webui/analysis/status_service.py
  - pySdp/webui/analysis/topdc_service.py
  - pySdp/webui/analysis/dashboard_service.py
  - pySdp/webui/analysis/analysis_md_service.py
related_tags: [python, webui, analysis, label, status, topdc, dashboard, attribution, results-tab]
summary: Python port of C# analysis statistics/report services + full Results tab redesign + hybrid C#/Python analysis pipeline
last_updated: 2026-04-20
---

# Implementation: Python Analysis Services + WebUI Results Tab

## Plan Reference
No formal plan. Direct port of C# services identified in FINDING-2026-04-15-raw-data-schema.md.
Motivation: run label/status/topdc/dashboard/analysis.md from Python instead of C# to decouple from SDK process.

## What Was Implemented

### Python Analysis Package (`pySdp/webui/analysis/`)

**`analysis/__init__.py`** — empty, makes directory a Python package.

**`analysis/label_service.py`** — `generate_label_json(snapshot_dir) -> Path`
- Reads `dc.json`, classifies every draw call, writes `label.json`
- Heuristic priority: (1) RT geometry → depth-only RT → Shadow/DepthOnly (conf 0.75); compute dispatch → PostProcess/Compute (conf 0.70); fullscreen quad (3-6 verts, no IB, Color RT) → PostProcess/Fullscreen (conf 0.65)
- (2) Keyword matching on shader entry point names (lowercased, skip "main"): 7 rule groups → shadow/ui/vfx/character/terrain/postprocess/scene
- (3) Fallback: Scene/Opaque/main (conf 0.60)
- Output schema: `{ schema_version: "3.0", snapshot_id, sdp_name, generated_at, total_dc_count, draw_calls: [{dc_id, api_id, label}] }`
- BOM note: reads dc.json with `encoding="utf-8-sig"` (C# writes BOM)

**`analysis/status_service.py`** — `generate_status_json(snapshot_dir) -> Path`
- Reads `label.json` + `metrics.json` + optional `dc.json` (for api_name → draw/compute count)
- Computes: overall stats (total_dc_count, draw_dc_count, compute_dc_count, total_clocks, total_read_bytes, total_write_bytes, total_fragments_shaded, total_vertices_shaded, metrics_coverage_ratio)
- Per-category percentile blocks: p50/p60/p70/p80/p90/p95/p99 for every metric key present
- Global percentiles (same structure, across all DCs regardless of category)
- Label quality stats: avg_confidence, low_confidence_ratio (threshold 0.60), llm_labeled_count, rule_labeled_count, reason_tag_distribution (sorted desc), low_confidence_dc_ids (top 20 lowest)
- **Percentile algorithm**: nearest-rank, same as C#: `idx = int(floor(p * len(sorted_v)))`, clamped to `len-1`
- Writes `snapshot_{id}_status.json`

**`analysis/topdc_service.py`** — `generate_topdc_json(snapshot_dir, rules_path=None) -> Path`
- Default rules path: `D:/snapdragon/SDPCLI/analysis/attribution_rules.json`
- 3-layer attribution engine:
  - Layer 1: for each metric in `layer1_metric_hints`, if value > p70 of category → mark as suspicious with `bottleneck_hint`
  - Layer 2: find highest percentile tier (p99→p50) where value exceeds threshold → assign `weight` (1.0 down to 0.05); `min_sample_size_for_category=5` check
  - Layer 3: for each bottleneck in `layer3_bottleneck_weights`, sum `metric_tier_weight × contribution_weight` across contributing metrics → `bottleneck_scores`; primary/secondary if score ≥ `primary_bottleneck_min_score` (0.25)
- Selects top-N DCs per category by clocks (`top_n_per_category=5` from rules)
- Writes `snapshot_{id}_topdc.json`
- Output per DC: `{dc_id, rank, clocks, clocks_rank_in_category, metrics, attribution: {suspicious_metrics, percentile_scores, bottleneck_scores, primary_bottleneck, secondary_bottleneck, confidence_score}, category_comparison: {clocks_percentile_in_category}, shader_files: [], mesh_file: "", label}`

**`analysis/dashboard_service.py`** — `generate_dashboard_md(snapshot_dir) -> Path`
- Reads `label.json` + `metrics.json` + optional `snapshot_{id}_status.json`
- Sections: category distribution table, Mermaid xychart-beta bar (GPU clocks per DC), Mermaid pie (clock budget by category + DC count by category), dynamic top-5 global table, per-category top-5 tables, category statistics table, label quality stats (from status.json), frame summary
- **Dynamic top-5 table**: shows outlier columns only when max_ratio ≥ threshold — Fragments (1.5×), ShaderBusy (1.3×), FragInstr (1.5×), TexStall (1.5×), TexL1Miss (1.5×), ReadMB (2.0×), WriteMB (2.0×), Vertices (1.5×); bold cell when individual ratio ≥ threshold
- Writes `snapshot_{id}_dashboard.md`

**`analysis/analysis_md_service.py`** — `generate_analysis_md(snapshot_dir, llm_fn=None) -> Path`
- Section 1: overall summary from `status.json` (total DC, clocks, memory, coverage, label confidence)
- Section 2: per-category analysis — if `llm_fn` provided and returns non-empty, use it; else rule-based: top-5 DCs by clocks, bottleneck flags (ShaderBusy>150%, TexL1Miss>80%, TexFetchStall>30%, Read>4MB)
- Section 3: rule-based recommendations — top-4 categories by clock cost, per-category tip dict (PostProcess/Scene/UI/Shadow/Character/Terrain/VFX + generic fallback)
- `llm_fn` signature: `(cat: str, sdp_name: str, top_dcs: list[dict], status: dict|None) -> str`
- Writes `snapshot_{id}_analysis.md`

### API Endpoints (`routes/files.py`)

Added 4 POST endpoints (all take `snapshot_dir` as query parameter):
- `POST /api/files/label?snapshot_dir=...` — runs `generate_label_json()`
- `POST /api/files/status?snapshot_dir=...` — runs `generate_status_json()`
- `POST /api/files/topdc?snapshot_dir=...` — runs `generate_topdc_json()`
- `POST /api/files/dashboard?snapshot_dir=...` — runs `generate_dashboard_md()`
- `POST /api/files/analysis_md?snapshot_dir=...` — runs `generate_analysis_md()`

Also added `GET /api/files/analyses?root=...` — walks analysis root, returns structured run/snapshot/file tree:
```json
{ "ok": true, "data": [
  { "name": "2026-04-20T18-11-00", "path": "...", "snapshots": [
    { "id": "snapshot_2", "path": "...",
      "analysis": [...],    // md files + files with "analysis"/"dashboard" in stem
      "statistics": [...],  // label.json, status.json
      "raw": [...],         // dc.json, metrics.json, buffers.json, shaders.json, textures.json
      "per_dc": [...]       // files under per_dc_content/
    }
  ]}
]}
```
File classification rules: `index` in stem → skip; `metrics` in stem → raw; `label`/`status` → statistics; `analysis`/`dashboard` or `.md` ext → analysis; `dc`/`buffers`/`shaders`/`textures` → raw.

### Results Tab (`static/index.html` + `static/app.js` + `static/style.css`)

Full Results tab redesign:
- Run selector pills (sorted newest-first), auto-selected when `openResults()` navigates here
- Per-snapshot tabs (snapshot_2, snapshot_3…)
- 3 collapsible file sections per snapshot: Analysis (open by default), Statistics (closed), Raw (closed)
- `per_dc_content/` rendered as nested collapsible folder inside each section
- File chips open content in `#file-viewer` (markdown rendered via `marked.js`, code highlighted)

Auto-scan behaviors:
- `DOMContentLoaded`: auto-scan from localStorage `analysisRoot` or `{sdpDir}/analysis`
- `switchTab('results')`: auto-scan if no runs loaded yet
- `scanSdpFiles()`: after loading SDP cards, silently scans `{sdpDir}/analysis` and enables Results buttons on matching cards
- `openResults(captureDir)`: derives `analysisRoot = captureDir/../..`, `runName = captureDir/..`, auto-selects correct run + snapshot

### Hybrid Analysis Pipeline (`static/app.js`)

`doAnalyze()` split into two stages:

**Stage 1 (C# job, 0-70% progress)**: sends only `CS_TARGETS = {dc, shaders, textures, buffers, metrics}` to `/api/sdpcli/analysis`; these require SDK P/Invoke and must run in the SDPCLI server process.

**Stage 2 (Python steps, 70-100% progress)**: after C# job completes with `captureDir`, runs `PY_STEPS` sequentially via `_runPySteps()`:
```
label (70-76%) → status (76-82%) → topdc (82-88%) → analysis_md (88-94%) → dashboard (94-100%)
```
Each step calls `POST /api/files/{endpoint}?snapshot_dir={captureDir}`. Failure is non-fatal (logs warning, continues to next step).

User can control which Python steps run by toggling chips in Settings. All chips default to checked (`DEFAULT_TARGETS = new Set(ALL_TARGETS)`).

## Key Design Decisions

1. **Python steps are non-fatal**: C# extracts the hard SDK data; Python post-processing failure doesn't block access to raw data.
2. **status.json must exist before topdc**: topdc reads category percentiles from status.json. Order enforced by `PY_STEPS` sequence.
3. **BOM encoding throughout**: all Python services use `encoding="utf-8-sig"` when reading C#-generated JSON files.
4. **topdc rules path hardcoded**: `D:/snapdragon/SDPCLI/analysis/attribution_rules.json`. If project moves, pass `rules_path` override to `generate_topdc_json()`.
5. **C# still handles label/status/topdc/dashboard targets if selected**: the C# `AnalysisJobRunner` still processes these targets if the job includes them. The split is enforced in the frontend (`CS_TARGETS` filter). Do not remove C# target handling — it's needed for CLI/direct server use.

## Status

**Implementation Status**: completed
**Build Status**: N/A (Python, no compile step)
**Test Status**: N/A
**Ready for Production**: yes (manual testing only)
