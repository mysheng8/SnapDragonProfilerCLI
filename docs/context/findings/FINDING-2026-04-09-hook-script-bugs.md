---
type: finding
topic: check-investigator-context-writes.py hook script bugs
status: investigated
related_paths:
  - .github/scripts/check-investigator-context-writes.py
related_tags:
  - hook
  - investigator
  - path-normalization
  - tool-name
summary: |
  The PreToolUse hook script has two critical bugs that cause it to either never
  activate (tool name mismatch) or always block legitimate writes (Windows absolute
  path not stripped). A minor lstrip semantics note is also recorded.
last_updated: 2026-04-09
---

# Finding: check-investigator-context-writes.py Hook Script Bugs

## Summary

The hook script at `.github/scripts/check-investigator-context-writes.py` has two critical
bugs that make it either completely ineffective or overly restrictive on Windows.

---

## Bug 1 — Tool Name Mismatch (Critical: hook never activates)

### Location

```python
EDIT_TOOLS = {"editFiles", "createFile"}
```

### Problem

The hook only intercepts tools named `editFiles` and `createFile` (camelCase).

The actual Copilot tool names visible in this session are **snake_case**:

| Expected in EDIT_TOOLS | Actual tool name |
|------------------------|-----------------|
| `createFile`           | `create_file`   |
| `editFiles`            | `replace_string_in_file` |
| (missing)              | `multi_replace_string_in_file` |

If the VS Code hook payload `tool_name` field mirrors the declared function name (snake_case),
then `tool_name not in EDIT_TOOLS` is always `True`, and `allow_response()` is returned
unconditionally — the hook is **entirely bypassed**.

### Impact

All file edits, including edits to source code, pass through without any restriction.

### Fix Required

```python
EDIT_TOOLS = {
    "editFiles",
    "createFile",
    "create_file",
    "replace_string_in_file",
    "multi_replace_string_in_file",
}
```

Verify actual `tool_name` values in hook payloads (add logging or inspect raw stdin) to
confirm the exact format, then adjust accordingly.

---

## Bug 2 — Windows Absolute Path Not Normalized to Relative (Critical: all legitimate writes blocked)

### Location

```python
def normalize_path(path: str) -> str:
    path = path.replace("\\", "/").strip()
    while "//" in path:
        path = path.replace("//", "/")
    path = posixpath.normpath(path)
    if path == ".":
        return path
    return path.lstrip("./")
```

### Problem

The instructions say Copilot tools must always use **absolute file paths**
(e.g., `d:\snapdragon\docs\context\foo.md`).

Trace through `normalize_path` for `"d:\\snapdragon\\docs\\context\\foo.md"`:

1. `replace("\\", "/")` → `"d:/snapdragon/docs/context/foo.md"`
2. `posixpath.normpath` → `"d:/snapdragon/docs/context/foo.md"` (unchanged; posixpath does not
   handle Windows drive letters)
3. `lstrip("./")` → `"d:/snapdragon/docs/context/foo.md"` (unchanged; `d` is not in `"./"`)

Result: `is_allowed_markdown_context_path("d:/snapdragon/docs/context/foo.md")` returns
`False` because the path does **not** start with `"docs/context/"`.

Every write to `docs/context/` using an absolute path is **blocked**, even though it is
a legitimate target.

### Impact

The Investigator agent cannot write any finding or plan document because all `create_file`
calls use absolute paths and will be incorrectly denied.

### Fix Required

Strip a known workspace root prefix before the `startswith` check, or use a suffix/substring
check instead of a prefix check:

Option A — strip workspace root dynamically from the path:
```python
WORKSPACE_ROOTS = [
    "d:/snapdragon/",
    "/d:/snapdragon/",   # posixpath artefact
]

def to_relative(path: str) -> str:
    for root in WORKSPACE_ROOTS:
        if path.startswith(root):
            return path[len(root):]
    return path
```

Option B — use `in` substring check (simpler but less strict):
```python
def is_allowed_markdown_context_path(path: str) -> bool:
    normalized = path.replace("\\", "/")
    return ("docs/context/" in normalized) and normalized.endswith(".md")
```

Option A is stricter. Option B is simpler but would allow paths like
`/evil/docs/context/foo.md` from other roots — acceptable risk in a local dev hook.

---

## Bug 3 — `lstrip("./")` Semantics (Minor)

### Location

```python
return path.lstrip("./")
```

### Problem

`str.lstrip(chars)` strips **individual characters** in the character set `{'.', '/'}`,
not the string `"./"` as a literal prefix.

For example:
- `"...dotfile.md".lstrip("./")` → `"dotfile.md"` (3 leading dots stripped, probably fine)
- `"/leading-slash.md".lstrip("./")` → `"leading-slash.md"` (intended)
- `"./relative.md".lstrip("./")` → `"relative.md"` (intended, posixpath already removes `./`)

In practice this is unlikely to cause incorrect allow/deny decisions for realistic paths,
but the semantics are surprising and could introduce edge-case bypasses.

### Fix Required (Low Priority)

Replace with explicit prefix removal if exact semantics are needed:
```python
while path.startswith("./") or path.startswith("/"):
    if path.startswith("./"):
        path = path[2:]
    elif path.startswith("/"):
        path = path[1:]
```

---

## Impact Summary

| Bug | Severity | Effect |
|-----|----------|--------|
| Tool name mismatch | Critical | Hook never fires; all writes pass unblocked |
| Windows absolute path | Critical | Hook always blocks writes to docs/context/ |
| `lstrip` semantics | Minor | Edge-case bypass risk only |

---

## Next Steps

Implementation requires the Executor agent.

1. Confirm actual `tool_name` values from hook payloads (log raw stdin in the hook)
2. Update `EDIT_TOOLS` to include the confirmed snake_case names
3. Fix `normalize_path` or `is_allowed_markdown_context_path` to handle absolute Windows paths
4. Optionally fix `lstrip` semantics
