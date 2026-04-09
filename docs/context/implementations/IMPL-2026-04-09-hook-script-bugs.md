---
type: implementation
topic: check-investigator-context-writes.py hook script bug fixes
status: completed
based_on:
  - FINDING-2026-04-09-hook-script-bugs.md
related_paths:
  - .github/scripts/check-investigator-context-writes.py
summary: Fixed three bugs in the PreToolUse hook script — tool name mismatch, Windows absolute path normalization, and lstrip semantics.
last_updated: 2026-04-09
---

## Plan Reference

FINDING-2026-04-09-hook-script-bugs.md — all three bugs addressed.

## Implementation Summary

Three independent edits applied to `.github/scripts/check-investigator-context-writes.py`.

## Files Changed

- `.github/scripts/check-investigator-context-writes.py`

## Key Changes

### 1. EDIT_TOOLS — added snake_case names

```python
# Before
EDIT_TOOLS = {"editFiles", "createFile"}

# After
EDIT_TOOLS = {
    "editFiles",
    "createFile",
    "create_file",
    "replace_string_in_file",
    "multi_replace_string_in_file",
}
```

Both camelCase (VS Code internal) and snake_case (agent-declared function names) are included
to be resilient to whichever format the hook payload actually uses.

### 2. normalize_path — fixed lstrip character-set confusion

```python
# Before
return path.lstrip("./")

# After
return path.lstrip("/")
```

`lstrip("./")` strips any combination of `.` and `/` characters from the left, not the
literal string `"./"`. After `posixpath.normpath`, the only leading character that needs
stripping is `/` (from Unix absolute paths). Dot-only prefixes do not appear after normpath.

### 3. is_allowed_markdown_context_path — handles absolute Windows/Unix paths

```python
# Before
def is_allowed_markdown_context_path(path: str) -> bool:
    return path.startswith(ALLOWED_PREFIX) and path.endswith(ALLOWED_SUFFIX)

# After
def is_allowed_markdown_context_path(path: str) -> bool:
    idx = path.find(ALLOWED_PREFIX)
    if idx == -1:
        return False
    if idx != 0 and path[idx - 1] not in ("/", "\\"):
        return False
    return path.endswith(ALLOWED_SUFFIX)
```

The new check finds `"docs/context/"` anywhere in the path but requires it to appear either
at position 0 or immediately after a path separator. This handles:
- Relative: `docs/context/foo.md` → allowed
- Windows absolute: `d:/snapdragon/docs/context/foo.md` → allowed
- Unix absolute: `/workspace/docs/context/foo.md` → allowed
- Partial match: `notdocs/context/foo.md` → denied

Path traversal is handled by `posixpath.normpath` in `normalize_path` (e.g.,
`/evil/docs/context/../../../etc/passwd` normalizes to `evil/etc/passwd`).

## Build / Validation

- instructions source: inline Python test
- command used: `python -c "..."` (8 test cases covering all scenarios)
- result: all 8 cases passed, all 5 tool name checks passed

```
OK  allow=True   raw='d:\\snapdragon\\docs\\context\\foo.md'
OK  allow=True   raw='d:/snapdragon/docs/context/foo.md'
OK  allow=True   raw='/workspace/docs/context/findings/bar.md'
OK  allow=True   raw='docs/context/plans/plan.md'
OK  allow=False  raw='d:\\snapdragon\\SDPCLI\\source\\App.cs'
OK  allow=False  raw='/evil/docs/context/payload/../../../etc/passwd'
OK  allow=False  raw='docs/context/foo.cs'
OK  allow=False  raw='notdocs/context/foo.md'
```

## Deviations from Plan

None. All three bugs fixed exactly as described in the finding.

## Issues Encountered

None.

## Next Steps

- Verify actual hook payload `tool_name` format in VS Code by temporarily logging `sys.stdin`
  to a file, to confirm camelCase vs snake_case in production. Adjust `EDIT_TOOLS` if needed.
