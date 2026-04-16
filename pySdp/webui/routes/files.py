"""
files.py — Local file-system API for browsing .sdp files and analysis results.
"""

from pathlib import Path

from fastapi import APIRouter, Query
from fastapi.responses import JSONResponse

import logger as _logger_module

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
