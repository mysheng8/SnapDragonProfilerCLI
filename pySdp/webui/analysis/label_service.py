"""
label_service.py — Rule-based DrawCall labeling (Python port of DrawCallLabelService.cs).

Reads dc.json from a snapshot directory, classifies each draw call by keyword matching
on shader entry points and render target characteristics, writes label.json.

No LLM involved — pure rule-based path.  LLM support can be added later as a separate
pass that upgrades rule labels where confidence < threshold.
"""

from __future__ import annotations

import json
import re
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

# ── Keyword rules (same order/priority as C# DrawCallLabelService.Rules) ──────
_RULES: list[tuple[list[str], str, str]] = [
    (["shadow", "planar", "shadowmap", "shadowdepth", "shadowpass", "shadowcaster"],
     "Shadow", "Shadow pass"),
    (["ui", "hud", "canvas", "glyph", "font", "widget", "overlay", "icon", "button", "menu"],
     "UI", "UI rendering"),
    (["particle", "vfx", "emitter", "ribbon", "trail", "spark", "smoke", "fire", "explosion", "distort"],
     "VFX", "Particle / VFX"),
    (["character", "skin", "hair", "cloth", "body", "player", "hero", "humanoid", "avatar", "face", "eye"],
     "Character", "Character rendering"),
    (["terrain", "landscape", "heightfield", "heightmap"],
     "Terrain", "Terrain rendering"),
    (["blur", "bloom", "tonemap", "tonemapping", "ssao", "ao", "dof", "composite",
      "resolve", "postprocess", "taa", "fxaa", "msaa", "temporal", "upscale", "blit",
      "lut", "vignette", "chromatic", "grain", "sharpen", "motion", "velocity",
      "denoise", "irradiance", "specular", "prefilter", "brdf", "ssr", "reflection"],
     "PostProcess", "Post-process pass"),
    (["sky", "skybox", "water", "ocean", "grass", "foliage",
      "ground", "scene", "mesh", "world", "indoor", "outdoor", "building", "road"],
     "Scene", "Scene rendering"),
]

# ── Render-target heuristics ───────────────────────────────────────────────────

def _has_depth_only(render_targets: list[dict]) -> bool:
    types = {rt.get("attachment_type", "") for rt in render_targets}
    color_types = {t for t in types if "Color" in t}
    depth_types = {t for t in types if t in ("Depth", "Stencil", "DepthStencil")}
    return bool(depth_types) and not color_types


def _is_fullscreen_quad(dc: dict, render_targets: list[dict]) -> bool:
    """3-4 verts, no index buffer, has a color RT that matches screen-ish resolution."""
    vc = dc.get("vertex_count", 0)
    ic = dc.get("index_count", 0)
    return 3 <= vc <= 6 and ic == 0 and any(
        rt.get("attachment_type") == "Color" for rt in render_targets
    )


def _is_compute(dc: dict) -> bool:
    return "Dispatch" in dc.get("api_name", "")


def _rt_heuristic_label(dc: dict) -> dict | None:
    """Returns a label dict if render-target geometry triggers a clear classification."""
    rts = dc.get("render_targets", [])

    if _is_compute(dc):
        return _make_label("PostProcess", "Compute", "Compute dispatch", ["compute_dispatch"], 0.70)

    if _has_depth_only(rts):
        return _make_label("Shadow", "DepthOnly", "Depth-only / shadow map", ["depth_only"], 0.75)

    if _is_fullscreen_quad(dc, rts):
        return _make_label("PostProcess", "Fullscreen", "Fullscreen post-process quad", ["fullscreen_quad"], 0.65)

    return None


# ── Keyword matching ───────────────────────────────────────────────────────────

def _entry_points(dc: dict) -> list[str]:
    """Collect all shader entry point strings from the DC."""
    stages = dc.get("shader_stages") or []
    names: list[str] = []
    for s in stages:
        ep = (s.get("entry_point") or "").lower()
        if ep and ep not in ("main", ""):
            names.append(ep)
    return names


def _keyword_label(dc: dict) -> dict | None:
    names = _entry_points(dc)
    if not names:
        return None
    combined = " ".join(names)
    for keywords, category, detail_hint in _RULES:
        for kw in keywords:
            if kw in combined:
                return _make_label(category, "", detail_hint, [_to_tag(kw)], 0.65)
    return None


def _to_tag(keyword: str) -> str:
    return re.sub(r"[^a-z0-9]+", "_", keyword).strip("_")


def _make_label(category: str, subcategory: str, detail: str,
                reason_tags: list[str], confidence: float,
                label_source: str = "rule") -> dict:
    return {
        "category":    category,
        "subcategory": subcategory,
        "detail":      detail,
        "reason_tags": reason_tags,
        "confidence":  confidence,
        "label_source": label_source,
    }


def _label_dc(dc: dict) -> dict:
    label = _rt_heuristic_label(dc)
    if label is None:
        label = _keyword_label(dc)
    if label is None:
        label = _make_label("Scene", "Opaque", "main", ["opaque_geometry"], 0.60)
    return label


# ── Public API ─────────────────────────────────────────────────────────────────

def generate_label_json(snapshot_dir: str | Path) -> Path:
    """
    Read dc.json from snapshot_dir, classify every draw call, write label.json.
    Returns the path to the written file.
    """
    snap = Path(snapshot_dir)
    dc_path = snap / "dc.json"
    if not dc_path.exists():
        raise FileNotFoundError(f"dc.json not found in {snap}")

    dc_data = json.loads(dc_path.read_text(encoding="utf-8-sig"))
    draw_calls: list[dict[str, Any]] = dc_data.get("draw_calls", [])

    labeled: list[dict] = []
    for dc in draw_calls:
        labeled.append({
            "dc_id":  dc.get("dc_id"),
            "api_id": dc.get("api_id"),
            "label":  _label_dc(dc),
        })

    out = {
        "schema_version": "3.0",
        "snapshot_id":    dc_data.get("snapshot_id", 0),
        "sdp_name":       dc_data.get("sdp_name", ""),
        "generated_at":   datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
        "total_dc_count": len(labeled),
        "draw_calls":     labeled,
    }

    out_path = snap / "label.json"
    out_path.write_text(json.dumps(out, ensure_ascii=False, indent=2), encoding="utf-8")
    return out_path
