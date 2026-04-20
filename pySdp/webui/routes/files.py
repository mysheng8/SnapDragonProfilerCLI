"""
files.py — Local file-system API for browsing .sdp files and analysis results.
"""

from pathlib import Path

from fastapi import APIRouter, Query
from fastapi.responses import JSONResponse

import logger as _logger_module
from analysis.label_service import generate_label_json
from analysis.status_service import generate_status_json
from analysis.topdc_service import generate_topdc_json
from analysis.dashboard_service import generate_dashboard_md
from analysis.analysis_md_service import generate_analysis_md

router = APIRouter()


@router.get("/sdp")
def list_sdp(dir: str = Query(..., description="Root directory to search for .sdp files")):
    """Return all .sdp files under the given directory."""
    root = Path(dir)
    if not root.exists():
        return JSONResponse({"ok": False, "error": f"Directory not found: {dir}"}, status_code=404)
    if not root.is_dir():
        return JSONResponse({"ok": False, "error": f"Not a directory: {dir}"}, status_code=400)

    files = []
    for f in sorted(root.rglob("*.sdp"), key=lambda p: p.stat().st_mtime, reverse=True):
        st = f.stat()
        files.append({
            "path": str(f),
            "name": f.name,
            "size": st.st_size,
            "modified": st.st_mtime,
        })
    return {"ok": True, "data": files}


@router.get("/results")
def list_results(dir: str = Query(..., description="Analysis capture directory")):
    """Return the files in an analysis result directory."""
    root = Path(dir)
    if not root.exists():
        return JSONResponse({"ok": False, "error": f"Directory not found: {dir}"}, status_code=404)
    if not root.is_dir():
        return JSONResponse({"ok": False, "error": f"Not a directory: {dir}"}, status_code=400)

    ORDER = [".json", ".md", ".hlsl", ".obj", ".png", ".csv"]

    def sort_key(p: Path):
        try:
            return ORDER.index(p.suffix.lower())
        except ValueError:
            return len(ORDER)

    files = []
    for f in sorted(root.iterdir(), key=sort_key):
        if f.is_file():
            st = f.stat()
            files.append({
                "path": str(f),
                "name": f.name,
                "size": st.st_size,
                "modified": st.st_mtime,
                "ext": f.suffix.lstrip(".").lower(),
            })
    return {"ok": True, "data": files}


@router.get("/analyses")
def list_analyses(root: str = Query(..., description="Parent directory containing analysis run folders")):
    """Return all analysis runs under root, each with its snapshot subdirectories and files."""
    root_path = Path(root)
    if not root_path.exists():
        return JSONResponse({"ok": False, "error": f"Directory not found: {root}"}, status_code=404)
    if not root_path.is_dir():
        return JSONResponse({"ok": False, "error": f"Not a directory: {root}"}, status_code=400)

    def classify_file(f: Path) -> str:
        stem = f.stem.lower()
        ext = f.suffix.lstrip(".").lower()
        if "index" in stem:
            return "skip"
        if ext in ("md",) or any(k in stem for k in ("analysis", "dashboard")):
            return "analysis"
        if any(k in stem for k in ("label", "status")):
            return "statistics"
        if any(k in stem for k in ("dc", "buffers", "shaders", "textures", "metrics")):
            return "raw"
        return "other"

    def file_info(f: Path) -> dict:
        st = f.stat()
        return {
            "path": str(f),
            "name": f.name,
            "size": st.st_size,
            "ext": f.suffix.lstrip(".").lower(),
        }

    runs = []
    for run_dir in sorted(root_path.iterdir(), key=lambda p: p.name, reverse=True):
        if not run_dir.is_dir():
            continue
        snapshots = []
        for snap_dir in sorted(d for d in run_dir.iterdir() if d.is_dir() and d.name.startswith("snapshot_")):
            snap_id = snap_dir.name  # e.g. "snapshot_2"
            groups = {"analysis": [], "statistics": [], "raw": [], "other": []}
            per_dc_files = []
            per_dc_dir = snap_dir / "per_dc_content"
            if per_dc_dir.exists():
                for f in sorted(per_dc_dir.iterdir()):
                    if f.is_file():
                        per_dc_files.append(file_info(f))
            for f in sorted(snap_dir.iterdir()):
                if not f.is_file():
                    continue
                cat = classify_file(f)
                if cat == "skip":
                    continue
                groups[cat].append(file_info(f))
            snapshots.append({
                "id": snap_id,
                "path": str(snap_dir),
                "analysis":   groups["analysis"],
                "statistics": groups["statistics"],
                "raw":        groups["raw"],
                "per_dc":     per_dc_files,
            })
        if snapshots:
            runs.append({"name": run_dir.name, "path": str(run_dir), "snapshots": snapshots})

    return {"ok": True, "data": runs}


@router.get("/read")
def read_file(
    path: str = Query(..., description="Absolute path to the file"),
    lines: int = Query(default=0, ge=0, description="Max lines to return (0 = all)"),
):
    """Read a text file and return its content."""
    p = Path(path)
    if not p.exists():
        return JSONResponse({"ok": False, "error": f"File not found: {path}"}, status_code=404)
    if not p.is_file():
        return JSONResponse({"ok": False, "error": f"Not a file: {path}"}, status_code=400)

    try:
        content = p.read_text(encoding="utf-8", errors="replace")
        if lines > 0:
            content = "\n".join(content.splitlines()[:lines])
        return {"ok": True, "data": {"content": content, "path": str(p), "name": p.name}}
    except Exception as exc:
        _logger_module.get_logger().error(
            "File read failed", exc=exc, context={"path": path}
        )
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)


@router.post("/label")
def run_label(snapshot_dir: str = Query(..., description="Snapshot directory containing dc.json")):
    """Generate label.json from dc.json using rule-based classification."""
    try:
        out = generate_label_json(snapshot_dir)
        return {"ok": True, "data": {"path": str(out)}}
    except FileNotFoundError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error("label generation failed", exc=exc, context={"dir": snapshot_dir})
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)


@router.post("/status")
def run_status(snapshot_dir: str = Query(..., description="Snapshot directory containing label.json + metrics.json")):
    """Generate snapshot_N_status.json from label.json + metrics.json."""
    try:
        out = generate_status_json(snapshot_dir)
        return {"ok": True, "data": {"path": str(out)}}
    except FileNotFoundError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error("status generation failed", exc=exc, context={"dir": snapshot_dir})
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)


@router.post("/topdc")
def run_topdc(snapshot_dir: str = Query(..., description="Snapshot directory containing label.json + metrics.json + status.json")):
    """Generate snapshot_N_topdc.json using 3-layer attribution engine."""
    try:
        out = generate_topdc_json(snapshot_dir)
        return {"ok": True, "data": {"path": str(out)}}
    except FileNotFoundError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error("topdc generation failed", exc=exc, context={"dir": snapshot_dir})
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)


@router.post("/dashboard")
def run_dashboard(snapshot_dir: str = Query(..., description="Snapshot directory containing label.json + metrics.json")):
    """Generate snapshot_N_dashboard.md."""
    try:
        out = generate_dashboard_md(snapshot_dir)
        return {"ok": True, "data": {"path": str(out)}}
    except FileNotFoundError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error("dashboard generation failed", exc=exc, context={"dir": snapshot_dir})
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)


@router.post("/analysis_md")
def run_analysis_md(snapshot_dir: str = Query(..., description="Snapshot directory containing label.json + metrics.json")):
    """Generate snapshot_N_analysis.md (rule-based)."""
    try:
        out = generate_analysis_md(snapshot_dir)
        return {"ok": True, "data": {"path": str(out)}}
    except FileNotFoundError as exc:
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=404)
    except Exception as exc:
        _logger_module.get_logger().error("analysis_md generation failed", exc=exc, context={"dir": snapshot_dir})
        return JSONResponse({"ok": False, "error": str(exc)}, status_code=500)
