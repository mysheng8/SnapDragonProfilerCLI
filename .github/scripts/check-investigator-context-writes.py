#!/usr/bin/env python3
"""
Deny edit/create operations unless every target path is under docs/context/
and ends with .md.

Intended use:
- VS Code Copilot hook via PreToolUse
- Best paired with the Investigator agent

Input:
- JSON from stdin

Output:
- JSON to stdout compatible with VS Code hook protocol
"""

from __future__ import annotations

import json
import posixpath
import sys
from typing import Iterable, List


ALLOWED_PREFIX = "docs/context/"
ALLOWED_SUFFIX = ".md"
EDIT_TOOLS = {"editFiles", "createFile"}


def normalize_path(path: str) -> str:
    path = path.replace("\\", "/").strip()
    while "//" in path:
        path = path.replace("//", "/")
    path = posixpath.normpath(path)
    if path == ".":
        return path
    return path.lstrip("./")


def extract_paths(tool_name: str, tool_input: dict) -> List[str]:
    """
    Common shapes seen in hook payloads:
    - editFiles: tool_input.files = [...]
    - createFile: tool_input.path = "..."
    Also tolerates filePath / oldUri / newUri / uri.
    """
    paths: List[str] = []

    def add(value):
        if isinstance(value, str) and value.strip():
            paths.append(value)

    if isinstance(tool_input.get("files"), list):
        for item in tool_input["files"]:
            if isinstance(item, str):
                add(item)
            elif isinstance(item, dict):
                add(item.get("path"))
                add(item.get("filePath"))

    add(tool_input.get("path"))
    add(tool_input.get("filePath"))

    for key in ("oldUri", "newUri", "uri"):
        add(tool_input.get(key))

    normalized = []
    for p in paths:
        np = normalize_path(p)
        if np not in ("", "."):
            normalized.append(np)
    return normalized


def is_allowed_markdown_context_path(path: str) -> bool:
    return path.startswith(ALLOWED_PREFIX) and path.endswith(ALLOWED_SUFFIX)


def allow_response(message: str = "") -> dict:
    out = {"continue": True}
    if message:
        out["systemMessage"] = message
    return out


def deny_response(reason: str, bad_paths: Iterable[str]) -> dict:
    bad_paths = list(bad_paths)
    return {
        "continue": True,
        "hookSpecificOutput": {
            "hookEventName": "PreToolUse",
            "permissionDecision": "deny",
            "permissionDecisionReason": reason,
            "additionalContext": (
                "Investigator may edit only markdown files under docs/context/. "
                f"Blocked paths: {', '.join(bad_paths)}"
            ),
        },
        "systemMessage": (
            "Blocked edit: only docs/context/*.md files are writable for Investigator."
        ),
    }


def ask_response(reason: str) -> dict:
    return {
        "continue": True,
        "hookSpecificOutput": {
            "hookEventName": "PreToolUse",
            "permissionDecision": "ask",
            "permissionDecisionReason": reason,
            "additionalContext": "This hook allows edits only under docs/context/*.md.",
        },
    }


def main() -> int:
    try:
        payload = json.load(sys.stdin)
    except Exception as exc:
        # Fail open with warning to avoid breaking all edits if payload shape changes.
        print(json.dumps(allow_response(f"Hook parse warning: {exc}")))
        return 0

    tool_name = payload.get("tool_name", "")
    tool_input = payload.get("tool_input", {}) or {}

    if tool_name not in EDIT_TOOLS:
        print(json.dumps(allow_response()))
        return 0

    paths = extract_paths(tool_name, tool_input)

    if not paths:
        print(json.dumps(ask_response(
            "Could not determine edit target paths. Manual approval required."
        )))
        return 0

    bad_paths = [p for p in paths if not is_allowed_markdown_context_path(p)]

    if bad_paths:
        print(json.dumps(deny_response(
            "Only docs/context/*.md files may be edited in this protected flow.",
            bad_paths,
        )))
        return 0

    print(json.dumps(allow_response()))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())