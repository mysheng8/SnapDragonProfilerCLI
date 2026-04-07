---
type: plan
topic: cmdbuffer-index-semantics
status: proposed
based_on: []
related_paths:
  - SDPCLI/config.ini
  - SDPCLI/source/Modes/AnalysisMode.cs
  - SDPCLI/source/Services/Analysis/DatabaseQueryService.cs
  - SDPCLI/source/Services/Analysis/DrawCallAnalysisService.cs
related_tags: [analysis, cmdbuffer, auto-detect, filter]
summary: >
  Redefine AnalysisCmdBufferIndex three-value semantics:
  -1 = all CBs, 0 = auto (most-DC CB), N>=1 = specific CB.
  Fixes the root cause of "No metrics" on frames where primary CB != 1.
last_updated: 2026-04-07
---

## Background

`DrawCallParameters.csv` 中 `CmdBufferIdx` 并非总是 `1`。
本次测试帧 snapshot_3/4 的所有 DC 都在 `CmdBufferIdx=2`，而 config 默认
`AnalysisCmdBufferIndex=1` → filter 返回 0 行 → 降级生成序列 ID → metrics join 全
空 → "No metrics CSV loaded"。

## New Semantics

| 值 | 含义 |
|----|------|
| `-1` | 分析所有 CommandBuffer（无 filter） |
| `0` | **Auto**：在 `DrawCallParameters` 中统计每个 `CmdBufferIdx` 的 DC 数，取最多的那个作 filter |
| `N >= 1` | 指定 CommandBuffer（与原来一致） |

默认值改为 `0`（auto），比硬编码 `1` 更健壮。

---

## File-by-file Changes

### 1. `SDPCLI/config.ini` — 注释 + 默认值

```ini
# Which CommandBuffer to analyse.
#   -1 = all CommandBuffers (no filter)
#    0 = auto: use the CommandBuffer with the most DrawCalls (recommended)
# N>=1 = specific CommandBuffer (1-based, matches Snapdragon Profiler GUI)
# Default: 0 (auto)
AnalysisCmdBufferIndex=0
```

---

### 2. `SDPCLI/source/Modes/AnalysisMode.cs` — 读取 & 日志

```csharp
// -1 = all, 0 = auto, >= 1 = specific
int cmdBufIdx = config.GetInt("AnalysisCmdBufferIndex", 0);
// -1 → null (no filter), 0 → 0 (auto sentinel), >= 1 → specific
int? cmdBufferFilter = cmdBufIdx >= 1 ? (int?)cmdBufIdx
                     : cmdBufIdx == 0 ? (int?)0
                     : null; // -1 = all

if (cmdBufIdx == -1)
    logger.Info("  CommandBuffer filter: ALL (AnalysisCmdBufferIndex=-1)");
else if (cmdBufIdx == 0)
    logger.Info("  CommandBuffer filter: AUTO (will select CmdBufferIdx with most DCs)");
else
    logger.Info($"  CommandBuffer filter: {cmdBufferFilter} (specific)");
```

旧代码（2 行需替换）：
```csharp
// 旧
int cmdBufIdx = config.GetInt("AnalysisCmdBufferIndex", 1);
int? cmdBufferFilter = cmdBufIdx > 0 ? (int?)cmdBufIdx : null;
if (cmdBufferFilter.HasValue)
    logger.Info($"  CommandBuffer filter: {cmdBufferFilter} (set AnalysisCmdBufferIndex=0 to analyse all)");
else
    logger.Info($"  CommandBuffer filter: ALL");
```

---

### 3. `SDPCLI/source/Services/Analysis/DatabaseQueryService.cs`

#### 3a. `GetDrawCallIds` — 日志行 L69

```csharp
// 旧
(cmdBufferFilter > 0 ? $" (CmdBuffer={cmdBufferFilter})" : "")

// 新
(cmdBufferFilter.HasValue
    ? (cmdBufferFilter > 0 ? $" (CmdBuffer={cmdBufferFilter})" : " (CmdBuffer=AUTO)")
    : "")
```

#### 3b. `GetDrawCallIdsFromParameters` — 核心变化

在 `conditions` 构建之前，插入 auto-detect 逻辑：

```csharp
private List<string> GetDrawCallIdsFromParameters(SQLiteConnection conn, uint captureId, int? cmdBufferFilter = null)
{
    var ids = new List<string>();

    bool hasCaptureIdCol = false;
    bool hasCmdBufferCol = false;
    using (var probe = new SQLiteCommand("PRAGMA table_info(DrawCallParameters)", conn))
    using (var pr = probe.ExecuteReader())
    {
        while (pr.Read())
        {
            string col = pr["name"].ToString() ?? "";
            if (col == "CaptureID")    hasCaptureIdCol = true;
            if (col == "CmdBufferIdx") hasCmdBufferCol = true;
        }
    }

    // ── Auto-detect: pick CmdBufferIdx with most DCs ──────────────────────
    int resolvedCmdBuf = -1; // -1 means "no specific CB filter"
    if (cmdBufferFilter == 0 && hasCmdBufferCol)
    {
        string captureWhere = hasCaptureIdCol ? $"WHERE CaptureID={captureId}" : "";
        string autoSql = $@"
            SELECT CmdBufferIdx
            FROM DrawCallParameters
            {captureWhere}
            GROUP BY CmdBufferIdx
            ORDER BY COUNT(*) DESC
            LIMIT 1";

        using (var ac = new SQLiteCommand(autoSql, conn))
        {
            var scalar = ac.ExecuteScalar();
            if (scalar != null && scalar != DBNull.Value)
                resolvedCmdBuf = Convert.ToInt32(scalar);
        }
        logger.Debug($"  Auto-detected primary CmdBufferIdx={resolvedCmdBuf}");
    }
    else if (cmdBufferFilter > 0)
    {
        resolvedCmdBuf = cmdBufferFilter.Value;
    }
    // cmdBufferFilter == null (-1/all)  → resolvedCmdBuf stays -1 (no CB filter)
    // ─────────────────────────────────────────────────────────────────────────

    var conditions = new List<string>();
    if (hasCaptureIdCol)               conditions.Add($"CaptureID={captureId}");
    if (resolvedCmdBuf >= 0 && hasCmdBufferCol) conditions.Add($"CmdBufferIdx={resolvedCmdBuf}");

    string where = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
    string sql   = $"SELECT DrawCallApiID FROM DrawCallParameters{where} ORDER BY rowid";

    using var cmd = new SQLiteCommand(sql, conn);
    using var r   = cmd.ExecuteReader();
    while (r.Read())
        ids.Add(r[0].ToString() ?? "");
    return ids;
}
```

**关键点**：原来 fallback（0行时再查）的设计 **不需要了**，因为 auto-detect 本身就
先选最优 CB。直接用选出的 CB 查，结果直接正确。

---

### 4. `SDPCLI/source/Services/Analysis/DrawCallAnalysisService.cs` — 日志行 L43

```csharp
// 旧
(cmdBufferFilter > 0 ? $", CommandBuffer={cmdBufferFilter}" : "")

// 新
(cmdBufferFilter == null ? ""
 : cmdBufferFilter == 0  ? ", CommandBuffer=AUTO"
 : $", CommandBuffer={cmdBufferFilter}")
```

---

## Impact Assessment

- `AnalysisCmdBufferIndex=0`（新默认）对所有 game frame 都健壮，无论用的是 CB1 还是 CB2
- `-1` 给需要分析全部 CB 的场景（调试用）
- 原有 `N>=1` 语义不变，向后兼容
- 不影响 SCOPE path 或 pipeline-count 降级路径

## Implementation Notes

- `GetDrawCallIdsFromParameters` 里 `using var` 语法在 net472 下需要 `{}` block，
  但这在该方法里已经是现有风格，照旧即可。
- `cmdBufferFilter == 0` 的 sentinel 在整个调用链都是 `int?`，
  `AnalysisMode.cs` 显式传 `(int?)0`，类型安全无问题。

## Status

Implementation requires the Executor agent.
