"""
files.py — Local file-system API for browsing and serving files.
"""

from pathlib import Path

from fastapi import APIRouter, Query
from fastapi.responses import FileResponse, JSONResponse

import logger as _logger_module


def make_router(db=None) -> APIRouter:
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

        _SCREENSHOT_NAMES = ["snapshot.png", "snapshot_screenshot.png", "snapshot_screenshot.jpg"]

        def _find_in_dir(d: Path) -> str | None:
            for name in _SCREENSHOT_NAMES:
                p = d / name
                if p.exists():
                    return str(p)
            for p in sorted(d.glob("*.bmp")):
                return str(p)
            return None

        def find_screenshot(snap_dir: Path) -> str | None:
            # 1. Look in the snapshot dir itself
            result = _find_in_dir(snap_dir)
            if result:
                return result
            # 2. Derive sdp capture dir by removing the "analysis" segment from the path.
            #    e.g. .../sdp/analysis/<run>/snapshot_N  →  .../sdp/<run>/snapshot_N
            try:
                parts = snap_dir.parts
                analysis_idx = next(i for i in range(len(parts) - 1, -1, -1) if parts[i].lower() == "analysis")
                sdp_parts = parts[:analysis_idx] + parts[analysis_idx + 1:]
                sdp_snap_dir = Path(*sdp_parts)
                result = _find_in_dir(sdp_snap_dir)
                if result:
                    return result
            except (StopIteration, Exception):
                pass
            return None

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
            # screenshots are surfaced via the dedicated screenshot field, not file lists
            if ext in ("png", "jpg", "jpeg", "bmp") and any(k in stem for k in ("screenshot", "snapshot")):
                return "skip"
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
                    "screenshot": find_screenshot(snap_dir),
                    "analysis":   groups["analysis"],
                    "statistics": groups["statistics"],
                    "raw":        groups["raw"],
                    "per_dc":     per_dc_files,
                })
            if snapshots:
                runs.append({"name": run_dir.name, "path": str(run_dir), "snapshots": snapshots})

        return {"ok": True, "data": runs}

    @router.get("/read", operation_id="get_file_read",
                summary="[MCP] Read a text file",
                description="Read a local text file (HLSL shader, JSON, Markdown) and return its content. "
                            "path must be an absolute filesystem path (from shader_stages[].file_path, etc).")
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

    @router.get("/raw", operation_id="get_file_raw",
                summary="[MCP] Serve raw file bytes",
                description="Serve any local file as raw bytes. Use for mesh OBJ files (mesh_file path from dc_detail). "
                            "Add download=1 to trigger browser download.")
    def serve_raw(
        path: str = Query(..., description="Absolute path to any file (served as-is)"),
        download: int = Query(default=0, description="Set to 1 to trigger browser download"),
    ):
        """Serve any local file as raw bytes — used by Three.js OBJLoader."""
        p = Path(path)
        if not p.exists() or not p.is_file():
            return JSONResponse({"ok": False, "error": f"File not found: {path}"}, status_code=404)
        headers = {}
        if download:
            headers["Content-Disposition"] = f'attachment; filename="{p.name}"'
        return FileResponse(str(p), headers=headers)

    @router.get("/image", operation_id="get_file_image",
                summary="[MCP] Serve an image file",
                description="Serve a PNG/JPG/BMP image from the local filesystem. "
                            "Use for screenshots (from /snapshots screenshot field) and textures (textures[].file_path from dc_detail).")
    def serve_image(path: str = Query(..., description="Absolute path to an image file")):
        """Serve an image file (png/jpg/bmp) from the local filesystem."""
        p = Path(path)
        if not p.exists() or not p.is_file():
            return JSONResponse({"ok": False, "error": f"File not found: {path}"}, status_code=404)
        ext = p.suffix.lstrip(".").lower()
        media = {"png": "image/png", "jpg": "image/jpeg", "jpeg": "image/jpeg", "bmp": "image/bmp"}.get(ext, "application/octet-stream")
        return FileResponse(str(p), media_type=media)

    return router
