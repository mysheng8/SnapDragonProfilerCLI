# MODULE INDEX — PySdp.Analysis — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: label_service, status_service, topdc_service, dashboard_service, analysis_md_service, llm_wrapper
Concepts: DrawCall classification, rule-based labeling, LLM labeling, bottleneck attribution, GPU category stats, Markdown report, Mermaid chart
Common Logs: label generation failed, status generation failed, topdc generation failed, dashboard generation failed, analysis_md generation failed
Entry Symbols: generate_label_json, generate_status_json, generate_topdc_json, generate_dashboard_md, generate_analysis_md, _label_dc, _label_dc_with_llm, LlmWrapper

## Role

Python analysis services that consume C# JSON outputs (dc.json, metrics.json) and produce classification artifacts (label.json, status.json, topdc.json, dashboard.md, analysis.md) using rule-based and optional LLM-based DrawCall labeling, bottleneck attribution, and per-category GPU metric aggregation.

## Entry Points

| Symbol | Location |
|--------|----------|
| `generate_label_json(snapshot_dir, db)` | [pySdp/analysis/label_service.py](../../../pySdp/analysis/label_service.py#L477) |
| `generate_status_json(snapshot_dir, db)` | [pySdp/analysis/status_service.py](../../../pySdp/analysis/status_service.py) |
| `generate_topdc_json(snapshot_dir)` | [pySdp/analysis/topdc_service.py](../../../pySdp/analysis/topdc_service.py) |
| `generate_dashboard_md(snapshot_dir)` | [pySdp/analysis/dashboard_service.py](../../../pySdp/analysis/dashboard_service.py) |
| `generate_analysis_md(snapshot_dir)` | [pySdp/analysis/analysis_md_service.py](../../../pySdp/analysis/analysis_md_service.py) |

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `LlmWrapper` | HTTP client for LLM endpoint; reads config.ini for URL/model; disk-level prompt cache | [pySdp/analysis/llm_wrapper.py](../../../pySdp/analysis/llm_wrapper.py) |
| `AnalysisModel` (base) | Abstract base for registered analysis models (run method contract) | [pySdp/analysis/models/base.py](../../../pySdp/analysis/models/base.py) |
| `CategoryBreakdownModel` | Per-category clocks/fragments/bandwidth aggregation model | [pySdp/analysis/models/category_breakdown.py](../../../pySdp/analysis/models/category_breakdown.py) |
| `TopBottleneckDcsModel` | Top-N DrawCalls by clocks within a category | [pySdp/analysis/models/top_bottleneck_dcs.py](../../../pySdp/analysis/models/top_bottleneck_dcs.py) |
| `LabelQualityModel` | Label confidence distribution and rule-vs-llm source stats | [pySdp/analysis/models/label_quality.py](../../../pySdp/analysis/models/label_quality.py) |

## Key Methods

| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| `_label_dc(dc)` | Rule-based DC classification: RT format → shader keyword → geometry heuristics | [label_service.py:56](../../../pySdp/analysis/label_service.py#L56) | generate_label_json per DC |
| `_label_dc_with_llm(dc, run_dir)` | LLM labeling with per-pipeline cache; falls back to _label_dc on failure | [label_service.py:443](../../../pySdp/analysis/label_service.py#L443) | generate_label_json if LLM enabled |
| `_build_llm_prompt(dc, shader_code)` | Build classification prompt with RT/geometry/shader context + category rules | [label_service.py:283](../../../pySdp/analysis/label_service.py#L283) | _label_dc_with_llm |
| `_extract_shader_sections(src)` | Extract resource declarations + main() body from HLSL; truncates large cbuffers | [label_service.py:174](../../../pySdp/analysis/label_service.py#L174) | _load_shader_code |
| `_persist_labels_to_db(db, ...)` | Write snapshots + draw_calls + labels rows to DuckDB | [label_service.py:525](../../../pySdp/analysis/label_service.py#L525) | generate_label_json when db provided |
| `generate_status_json(snapshot_dir, db)` | Aggregate labels + metrics into per-category stats; write status.json | [status_service.py](../../../pySdp/analysis/status_service.py) | pipeline step "status" |
| `generate_topdc_json(snapshot_dir)` | 3-layer attribution engine; reads attribution_rules.json | [topdc_service.py](../../../pySdp/analysis/topdc_service.py) | pipeline step "topdc" |
| `generate_analysis_md(snapshot_dir)` | Per-category rule-based analysis narrative + LLM hook; write analysis.md | [analysis_md_service.py](../../../pySdp/analysis/analysis_md_service.py) | pipeline step "analysis" |

## Call Flow Skeleton

```
generate_label_json(snapshot_dir, db)
├── read dc.json → draw_calls list
├── _pipeline_llm_cache.clear()
└── for each dc:
    ├── _label_dc_with_llm(dc, run_dir)     [if LLM enabled + pipeline_id known]
    │   ├── get_llm().chat(prompt)
    │   ├── _parse_llm_response()
    │   └── cache result by pipeline_id
    └── _label_dc(dc)                       [fallback / when LLM disabled]
        ├── R1a: depth-only RT → Shadow
        ├── R1b: RG-format RT → Shadow (VSM/ESM)
        ├── keyword match on shader entry_points
        ├── R2: vkCmdDispatch → Compute/Character
        ├── R3: fullscreen quad → PostProcess
        └── R4: geometry heuristics (UI/VFX/Scene)
    write label.json
    _persist_labels_to_db(db, ...) [if db provided]
```

## Label Categories

| Category | Trigger |
|----------|---------|
| `Scene(Shadow)` | Depth-only RT or RG-format color-only RT |
| `PostProcess` | Fullscreen quad (3-6 verts, no IB) or compute blur/tone |
| `UI` | Color-only RT, RGBA8, no depth |
| `VFX` | Color+depth, many instances, verts-per-instance ≤ 6 |
| `Character` | Skinning/morph compute dispatch or SH probe Buffer in shader |
| `Terrain` | Heightfield/landscape keywords in entry point |
| `Shadow` | "shadow" keyword in entry point |
| `Scene` | Default fallback (opaque geometry) |

## Data Ownership Map

| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| `label.json` | `generate_label_json` | `generate_status_json`, `generate_topdc_json`, `generate_analysis_md`, `ingest_snapshot` | n/a (file artifact) |
| `status.json` | `generate_status_json` | `generate_topdc_json`, `generate_dashboard_md` | n/a |
| `topdc.json` | `generate_topdc_json` | `generate_analysis_md` | n/a |
| `dashboard.md` | `generate_dashboard_md` | browser display | n/a |
| `analysis.md` | `generate_analysis_md` | browser display | n/a |
| `_pipeline_llm_cache` | `_label_dc_with_llm` | `_label_dc_with_llm` | `generate_label_json` (cleared per snapshot) |

## Log → Code Map

| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `"label generation failed"` | [routes/files.py:231](../../../pySdp/webui/routes/files.py#L231) | WebUI route exception |
| `"status generation failed"` | [routes/files.py:242](../../../pySdp/webui/routes/files.py#L242) | WebUI route exception |
| `"topdc generation failed"` | [routes/files.py:253](../../../pySdp/webui/routes/files.py#L253) | WebUI route exception |
| `"analysis_md generation failed"` | [routes/files.py:278](../../../pySdp/webui/routes/files.py#L278) | WebUI route exception |
| `"(empty shader"` | [label_service.py:247](../../../pySdp/analysis/label_service.py#L247) | trivial stub shader skips LLM |
| `"no decompiled shader files"` | [label_service.py:268](../../../pySdp/analysis/label_service.py#L268) | only raw SPIR-V, LLM skipped |

## Search Hints

```
Find rule-based label logic:
grep -n "_RULES" pySdp/analysis/label_service.py

Find LLM prompt builder:
grep -n "_build_llm_prompt" pySdp/analysis/label_service.py

Find attribution rules engine:
open pySdp/analysis/topdc_service.py

Jump:
open pySdp/analysis/label_service.py:56    # _label_dc (rule engine)
open pySdp/analysis/label_service.py:443   # _label_dc_with_llm
open pySdp/analysis/label_service.py:477   # generate_label_json (public API)
```
