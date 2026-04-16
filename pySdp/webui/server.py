"""
server.py — pySdp WebUI entry point.

Starts a FastAPI + uvicorn server on localhost:8000.
Serves static files at /static/ and proxies SDPCLI API calls.

Usage:
    python webui/server.py [--port 8000] [--host 127.0.0.1] [--sdpcli http://localhost:5000]
"""

import argparse
import os
import sys
from pathlib import Path

# ── Parse CLI args early so env vars are set before routes are imported ───────
if __name__ == "__main__":
    _p = argparse.ArgumentParser(description="pySdp WebUI", add_help=False)
    _p.add_argument("--port",   type=int, default=8000)
    _p.add_argument("--host",   default="127.0.0.1")
    _p.add_argument("--sdpcli", default=None, metavar="URL",
                    help="SDPCLI Server URL (default: http://localhost:5000)")
    _p.add_argument("-h", "--help", action="store_true")
    _args, _ = _p.parse_known_args()
    if _args.sdpcli:
        os.environ["SDPCLI_URL"] = _args.sdpcli

# Make webui/ importable when run as `python webui/server.py`
sys.path.insert(0, str(Path(__file__).parent))

from fastapi import FastAPI, Request                  # noqa: E402
from fastapi.responses import FileResponse, JSONResponse  # noqa: E402
from fastapi.staticfiles import StaticFiles           # noqa: E402

from routes.proxy import router as proxy_router       # noqa: E402
from routes.files import router as files_router       # noqa: E402
from routes.logs  import router as logs_router        # noqa: E402
import logger as _logger_module                       # noqa: E402

# ── App ───────────────────────────────────────────────────────────────────────

STATIC_DIR = Path(__file__).parent / "static"

app = FastAPI(title="pySdp WebUI", docs_url=None, redoc_url=None)

# ── Exception middleware — catches any unhandled error in route handlers ──────

@app.middleware("http")
async def _exception_middleware(request: Request, call_next):
    try:
        return await call_next(request)
    except Exception as exc:
        _logger_module.get_logger().error(
            f"Unhandled exception: {exc}",
            exc=exc,
            context={"method": request.method, "path": str(request.url.path)},
        )
        return JSONResponse(
            {"ok": False, "error": f"Internal server error: {exc}"},
            status_code=500,
        )

# ── Routers ───────────────────────────────────────────────────────────────────

app.include_router(proxy_router, prefix="/api/sdpcli")
app.include_router(files_router, prefix="/api/files")
app.include_router(logs_router,  prefix="/api/logs")
app.mount("/static", StaticFiles(directory=str(STATIC_DIR)), name="static")


@app.get("/")
def root():
    return FileResponse(str(STATIC_DIR / "index.html"))


# ── Entry point ───────────────────────────────────────────────────────────────

if __name__ == "__main__":
    from routes.proxy import SDPCLI_BASE
    import uvicorn

    if getattr(_args, "help", False):
        _p.print_help()
        sys.exit(0)

    log = _logger_module.get_logger()
    log.info("WebUI starting", context={
        "host": _args.host, "port": _args.port, "sdpcli": SDPCLI_BASE
    })

    print(f"pySdp WebUI   →  http://{_args.host}:{_args.port}")
    print(f"SDPCLI Server →  {SDPCLI_BASE}")
    print(f"Log file      →  {Path(__file__).parent / 'logs' / 'webui.log'}")
    uvicorn.run(app, host=_args.host, port=_args.port, log_level="warning")
