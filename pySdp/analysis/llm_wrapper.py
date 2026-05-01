"""llm_wrapper.py — OpenAI-compatible LLM client with SHA-256 ring-pool cache.

Mirrors C# LlmApiWrapper + LlmResponseCache:
  - Reads config from SDPCLI/config.ini (LlmApiEndpoint, LlmApiKey, LlmModel, ...)
  - Cache key = SHA-256(prompt) first 16 bytes (32 hex chars) — same as C#
  - Cache file format identical to C# llm_cache.json — shared across both runtimes
"""
from __future__ import annotations

import hashlib
import json
import os
import threading
import time
import urllib.request
import urllib.error
from pathlib import Path

# ── Config location ────────────────────────────────────────────────────────────
# Walk up from this file to find SDPCLI/config.ini
def _find_config_ini() -> Path | None:
    here = Path(__file__).resolve()
    for ancestor in [here.parent, here.parent.parent, here.parent.parent.parent,
                     here.parent.parent.parent.parent]:
        candidate = ancestor / "SDPCLI" / "config.ini"
        if candidate.exists():
            return candidate
    return None


def _load_config() -> dict[str, str]:
    path = _find_config_ini()
    if path is None:
        return {}
    out: dict[str, str] = {}
    # config.ini has no [section] headers — parse manually
    for line in path.read_text(encoding="utf-8-sig").splitlines():
        line = line.strip()
        if not line or line.startswith("#"):
            continue
        if "=" in line:
            k, _, v = line.partition("=")
            out[k.strip()] = v.strip()
    return out


# ── Ring-pool cache ────────────────────────────────────────────────────────────

class _LlmCache:
    """Thread-safe ring-pool cache — file format identical to C# llm_cache.json."""

    def __init__(self, capacity: int, path: Path) -> None:
        self._capacity = max(8, capacity)
        self._path = path
        self._lock = threading.Lock()
        self._slots: list[dict | None] = [None] * self._capacity
        self._index: dict[str, int] = {}
        self._write_head = 0
        self._count = 0
        self._load()

    @staticmethod
    def hash(prompt: str) -> str:
        h = hashlib.sha256(prompt.encode("utf-8")).digest()
        return h[:16].hex()

    def get(self, prompt: str) -> str | None:
        key = self.hash(prompt)
        with self._lock:
            pos = self._index.get(key)
            if pos is not None and self._slots[pos] is not None:
                return self._slots[pos]["response"]  # type: ignore[index]
        return None

    def put(self, prompt: str, response: str) -> None:
        key = self.hash(prompt)
        with self._lock:
            if key in self._index:
                pos = self._index[key]
                self._slots[pos]["response"] = response  # type: ignore[index]
                self._slots[pos]["ts"] = _utc_now()  # type: ignore[index]
            else:
                victim = self._slots[self._write_head]
                if victim is not None:
                    self._index.pop(victim["key"], None)
                    self._count -= 1
                self._slots[self._write_head] = {"key": key, "response": response, "ts": _utc_now()}
                self._index[key] = self._write_head
                self._count += 1
                self._write_head = (self._write_head + 1) % self._capacity
            self._save_nolock()

    def _load(self) -> None:
        if not self._path.exists():
            return
        try:
            data = json.loads(self._path.read_text(encoding="utf-8"))
            slots = data.get("slots") or []
            load_n = min(len(slots), self._capacity)
            for i in range(load_n):
                s = slots[i]
                if not s or not s.get("key"):
                    continue
                self._slots[i] = s
                self._index[s["key"]] = i
                self._count += 1
            saved_head = data.get("write_head", self._count % self._capacity)
            self._write_head = max(0, min(saved_head, self._capacity - 1))
        except Exception:
            pass

    def _save_nolock(self) -> None:
        try:
            self._path.parent.mkdir(parents=True, exist_ok=True)
            tmp = self._path.with_suffix(".tmp")
            tmp.write_text(json.dumps({
                "capacity":   self._capacity,
                "write_head": self._write_head,
                "count":      self._count,
                "slots":      self._slots,
            }, ensure_ascii=False, indent=2), encoding="utf-8")
            if self._path.exists():
                self._path.unlink()
            tmp.rename(self._path)
        except Exception:
            pass


def _utc_now() -> str:
    return time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime())


# ── Public wrapper ─────────────────────────────────────────────────────────────

class LlmWrapper:
    """OpenAI-compatible single-turn chat client. Thread-safe."""

    def __init__(self) -> None:
        cfg = _load_config()
        self._endpoint        = cfg.get("LlmApiEndpoint", "").strip()
        self._api_key         = cfg.get("LlmApiKey",      "").strip()
        self._model           = cfg.get("LlmModel",       "gpt-4o").strip()
        self._timeout         = int(cfg.get("LlmTimeoutSeconds",  "60"))
        self._max_tokens      = int(cfg.get("LlmMaxOutputTokens", "800"))
        self.is_enabled       = bool(self._endpoint and self._api_key)
        self.last_error: str | None = None

        self._cache: _LlmCache | None = None
        # LlmCacheOverride=true → skip cache reads, always call LLM fresh (still writes)
        self._cache_override = cfg.get("LlmCacheOverride", "false").lower() == "true"
        if self.is_enabled and cfg.get("LlmCacheEnabled", "true").lower() != "false":
            capacity  = int(cfg.get("LlmCacheSize", "512"))
            cfg_path  = _find_config_ini()
            default   = str(cfg_path.parent.parent / "llm_cache.json") if cfg_path else "llm_cache.json"
            cache_path = Path(cfg.get("LlmCachePath", default).lstrip("#").strip())
            self._cache = _LlmCache(capacity, cache_path)

    def chat(self, prompt: str) -> str | None:
        """Send prompt; return response text or None on error."""
        self.last_error = None
        if not self.is_enabled:
            self.last_error = "LLM not configured"
            return None

        if self._cache and not self._cache_override:
            hit = self._cache.get(prompt)
            if hit is not None:
                return hit

        try:
            response = self._call(prompt)
        except Exception as exc:
            self.last_error = str(exc)
            return None

        if response is not None and self._cache:
            self._cache.put(prompt, response)
        return response

    def _call(self, prompt: str) -> str | None:
        body = json.dumps({
            "model":       self._model,
            "max_tokens":  self._max_tokens,
            "temperature": 0.0,
            "messages":    [{"role": "user", "content": prompt}],
        }).encode("utf-8")

        req = urllib.request.Request(
            self._endpoint,
            data=body,
            headers={
                "Content-Type":  "application/json",
                "Authorization": f"Bearer {self._api_key}",
            },
            method="POST",
        )
        try:
            with urllib.request.urlopen(req, timeout=self._timeout) as resp:
                data = json.loads(resp.read().decode("utf-8"))
                return data["choices"][0]["message"]["content"]
        except urllib.error.HTTPError as exc:
            detail = exc.read().decode("utf-8", errors="replace")[:300]
            raise RuntimeError(f"HTTP {exc.code}: {detail}") from exc


# Module-level singleton — lazy-initialised on first import
_instance: LlmWrapper | None = None
_init_lock = threading.Lock()


def get_llm() -> LlmWrapper:
    global _instance
    if _instance is None:
        with _init_lock:
            if _instance is None:
                _instance = LlmWrapper()
    return _instance


# ── VLM wrapper (vision) ───────────────────────────────────────────────────────

import base64
from pathlib import Path as _Path


class VlmWrapper:
    """OpenAI-compatible vision client. Sends image as base64 data URL.

    Reads VlmApiEndpoint / VlmApiKey / VlmModel / VlmTimeoutSeconds /
    VlmMaxOutputTokens from SDPCLI/config.ini.
    """

    def __init__(self) -> None:
        cfg = _load_config()
        self._endpoint   = cfg.get("VlmApiEndpoint", "").strip()
        self._api_key    = cfg.get("VlmApiKey",      "").strip()
        self._model      = cfg.get("VlmModel",       "").strip()
        self._timeout    = int(cfg.get("VlmTimeoutSeconds",   "60"))
        self._max_tokens = int(cfg.get("VlmMaxOutputTokens",  "2000"))
        self.is_enabled  = bool(self._endpoint and self._api_key and self._model)
        self.last_error: str | None = None

    def describe_image(self, image_path: str | _Path, prompt: str) -> str | None:
        """Send image + text prompt; return response text or None on error."""
        self.last_error = None
        if not self.is_enabled:
            self.last_error = "VLM not configured"
            return None

        p = _Path(image_path)
        if not p.exists():
            self.last_error = f"Image not found: {image_path}"
            return None

        ext = p.suffix.lstrip(".").lower()
        mime = {"png": "image/png", "jpg": "image/jpeg", "jpeg": "image/jpeg",
                "bmp": "image/bmp", "webp": "image/webp"}.get(ext, "image/png")
        b64 = base64.b64encode(p.read_bytes()).decode("ascii")
        data_url = f"data:{mime};base64,{b64}"

        body = json.dumps({
            "model":      self._model,
            "max_tokens": self._max_tokens,
            "messages": [{
                "role": "user",
                "content": [
                    {"type": "image_url", "image_url": {"url": data_url}},
                    {"type": "text",      "text": prompt},
                ],
            }],
        }).encode("utf-8")

        req = urllib.request.Request(
            self._endpoint,
            data=body,
            headers={
                "Content-Type":  "application/json",
                "Authorization": f"Bearer {self._api_key}",
            },
            method="POST",
        )
        try:
            with urllib.request.urlopen(req, timeout=self._timeout) as resp:
                data = json.loads(resp.read().decode("utf-8"))
                return data["choices"][0]["message"]["content"]
        except urllib.error.HTTPError as exc:
            detail = exc.read().decode("utf-8", errors="replace")[:300]
            self.last_error = f"HTTP {exc.code}: {detail}"
            return None
        except Exception as exc:
            self.last_error = str(exc)
            return None


_vlm_instance: VlmWrapper | None = None
_vlm_lock = threading.Lock()


def get_vlm() -> VlmWrapper:
    global _vlm_instance
    if _vlm_instance is None:
        with _vlm_lock:
            if _vlm_instance is None:
                _vlm_instance = VlmWrapper()
    return _vlm_instance
