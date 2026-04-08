---
type: plan
topic: unified SQLite DB access via SdpDatabase — Data/ entry point, full query abstraction
status: revised
based_on:
  - FINDING-2026-04-08-db-access-fragmentation.md
  - FINDING-2026-04-08-table-validation.md
related_paths:
  - SDPCLI/source/Data/SdpDatabase.cs                             (NEW — core factory)
  - SDPCLI/source/Data/SdpDatabase.Schema.cs                      (NEW — schema helpers)
  - SDPCLI/source/Data/SdpDatabase.DrawCalls.cs                   (NEW — DC queries)
  - SDPCLI/source/Data/SdpDatabase.Shaders.cs                     (NEW — shader queries)
  - SDPCLI/source/Data/SdpDatabase.Textures.cs                    (NEW — texture queries)
  - SDPCLI/source/Data/SdpDatabase.Buffers.cs                     (NEW — VB/IB/byte buffer queries)
  - SDPCLI/source/Services/Analysis/DatabaseQueryService.cs       (becomes thin caller)
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs       (becomes thin caller)
  - SDPCLI/source/Tools/ShaderExtractor.cs                        (SQL removed → calls SdpDatabase)
  - SDPCLI/source/Tools/TextureExtractor.cs                       (SQL removed → calls SdpDatabase)
  - SDPCLI/source/Tools/MeshExtractor.cs                          (SQL removed → calls SdpDatabase)
  - SDPCLI/source/Analysis/AnalysisPipeline.cs                    (one SdpDatabase per capture)
related_tags:
  - database
  - sqlite
  - architecture
  - refactor
  - query-abstraction
  - partial-class
summary: |
  SdpDatabase placed under source/Data/ (namespace SnapdragonProfilerCLI.Data) as the
  single unified entry point for ALL DB operations — separate data layer, not mixed into Tools.
  Implemented via C# partial classes so queries are grouped by domain while remaining
  one logical class. All 5 in-scope classes (ShaderExtractor, TextureExtractor, MeshExtractor,
  DrawCallQueryService, DatabaseQueryService) lose direct SQL; they call SdpDatabase methods.
  SdpDatabase.Capture.cs eliminated: DatabaseQueryService's GetDrawCallIds (+ 3 helpers) fold
  into SdpDatabase.DrawCalls.cs; GetAllTables/GetMetadata fold into SdpDatabase.Schema.cs.
  Shared helpers (TableExists, ColumnExists, FindColumn, GetFormatName) deduped into Schema.cs.
  Fixes ShaderExtractor missing Read Only. Adds WAL + busy_timeout.
  CsvToDbService and DataExportService remain out of scope.
last_updated: 2026-04-08
---

# Plan: Unified SQLite DB Access via SdpDatabase (Data/, Full Query Abstraction)

## Design Principles

### 1. Single Logical Entry Point

`SdpDatabase` (in `Data/`) is the **only class that touches SQLiteConnection**.
All other classes call `SdpDatabase` methods — no raw SQL outside of it.

### 2. Partial Class Organisation (not a god-class)

Query methods are split by domain into partial class files (all `Data/SdpDatabase.*.cs`)
so the file stays manageable. C# `partial class` keeps them one logical type.

### 3. Connection Factory Pattern — Preserved

`OpenConnection()` still creates a new independent connection per call.
Callers that need to run multiple queries in one logical unit open once, pass the
connection into a block, then dispose. This preserves the parallel-safe
"one connection per worker" model.

### 4. Query Ownership Migration

| From class | Query domain | Moves to partial file |
|------------|-------------|----------------------|
| `DrawCallQueryService` | DC info, pipeline, VBs, IB, textures, shaders, render targets | `SdpDatabase.DrawCalls.cs` |
| `DatabaseQueryService` | `GetDrawCallIds` + 3 private DC-list helpers | `SdpDatabase.DrawCalls.cs` |
| `DatabaseQueryService` | `GetAllTables`, `GetMetadata` | `SdpDatabase.Schema.cs` |
| `ShaderExtractor` | ShaderStages, SPIR-V bytes, disasm text | `SdpDatabase.Shaders.cs` |
| `TextureExtractor` | TextureMetadata, texture byte buffer | `SdpDatabase.Textures.cs` |
| `MeshExtractor` | VB/IB bytes, VertexInputState, DC-with-VB list | `SdpDatabase.Buffers.cs` |
| All 4 classes | `TableExists`, `ColumnExists`, `FindColumn`, `GetFormatName`, `GetShaderStageName` | `SdpDatabase.Schema.cs` |

### 5. Services / Extractors Become Thin Callers

- `ShaderExtractor` — file I/O + spirv-cross only; SQL replaced by `_db.GetShaderStages()`, `_db.ReadSpirvBytes()`, `_db.ReadShaderDisasm()`
- `TextureExtractor` — format conversion + file save only; SQL replaced by `_db.GetTextureMetadata()`, `_db.ReadTextureBytes()`
- `MeshExtractor` — OBJ geometry only; SQL replaced by `_db.ReadBufferBytes()`, `_db.LoadVertexInputState()`, `_db.GetDrawCallsWithVertexBuffers()`
- `DrawCallQueryService` — orchestration only; entire query implementation moved to `SdpDatabase.DrawCalls.cs`
- `DatabaseQueryService` — thin shell only; `GetDrawCallIds` SQL → `SdpDatabase.DrawCalls.cs`; `GetAllTables`/`GetMetadata` → `SdpDatabase.Schema.cs`; `OpenDatabase()` deleted

---

## Phase 1 — New Files: Data/SdpDatabase.*.cs

### 1a. `Data/SdpDatabase.cs` — Core Factory

**Namespace:** `SnapdragonProfilerCLI.Data`

```csharp
using System;
using System.Data.SQLite;

namespace SnapdragonProfilerCLI.Data
{
    /// <summary>
    /// Unified entry point for all sdp.db access.
    /// Holds DbPath + CaptureId; creates SQLiteConnections on demand.
    ///
    /// Thread-safe: immutable after construction; each OpenConnection() call
    /// returns a new independent connection — safe for Parallel.ForEach workers.
    ///
    /// Split into domain-specific partial class files:
    ///   SdpDatabase.Schema.cs   — TableExists, ColumnExists, FindColumn, format name helpers
    ///   SdpDatabase.DrawCalls.cs — full DrawCallInfo resolution pipeline
    ///   SdpDatabase.Shaders.cs  — ShaderStages, SPIR-V, Disasm
    ///   SdpDatabase.Textures.cs — TextureMetadata, raw texture bytes
    ///   SdpDatabase.Buffers.cs  — VB/IB bytes, VertexInputState
    ///   (no Capture.cs: DrawCallIds folds into DrawCalls.cs; GetAllTables/GetMetadata into Schema.cs)
    /// </summary>
    public sealed partial class SdpDatabase
    {
        public string DbPath    { get; }
        public uint   CaptureId { get; }

        public SdpDatabase(string dbPath, uint captureId)
        {
            DbPath    = dbPath;
            CaptureId = captureId;
        }

        /// <summary>
        /// Opens and returns a new SQLiteConnection with standard settings.
        /// Caller is responsible for disposing (use `using`).
        /// </summary>
        public SQLiteConnection OpenConnection(bool readOnly = true)
        {
            string cs = readOnly
                ? $"Data Source={DbPath};Version=3;Read Only=True;"
                : $"Data Source={DbPath};Version=3;";

            var conn = new SQLiteConnection(cs);
            conn.Open();

            if (!readOnly)
            {
                using var wal = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
                wal.ExecuteNonQuery();
            }

            using var timeout = new SQLiteCommand("PRAGMA busy_timeout=2000;", conn);
            timeout.ExecuteNonQuery();

            return conn;
        }

        /// <summary>Convenience: open → action → dispose.</summary>
        public void WithConnection(Action<SQLiteConnection> action, bool readOnly = true)
        {
            using var conn = OpenConnection(readOnly);
            action(conn);
        }
    }
}
```

---

### 1b. `Data/SdpDatabase.Schema.cs` — Shared Schema Helpers

All helper methods currently **duplicated across 4+ classes** move here.
Public so service classes can call them without re-implementing.

```csharp
namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // Static helpers — accept an open connection (caller owns lifecycle)
        public static bool TableExists(SQLiteConnection conn, string tableName) { ... }
        public static bool ColumnExists(SQLiteConnection conn, string table, string column) { ... }
        public static string? FindColumn(SQLiteConnection conn, string table, params string[] substrings) { ... }
        public static string GetShaderStageName(uint stage) { ... }  // moved from DrawCallQueryService
        public static string GetFormatName(uint vkFormat) { ... }    // moved from DrawCallQueryService
        public static string GetFormatName(int  vkFormat) { ... }    // moved from TextureExtractor (int overload)
    }
}
```

---

### 1c. `Data/SdpDatabase.DrawCalls.cs` — Draw Call Query Domain

Absorbs all SQL from `DrawCallQueryService` (private static methods become public instance methods):

```csharp
namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // --- Draw call resolution ---
        public DrawCallInfo? GetDrawCallInfo(string drawCallNumber) { ... }
        public DrawCallInfo? GetDrawCallInfoByApiId(uint apiId, string label = "") { ... }

        // --- Sub-queries (public — also used by AnalysisPipeline scans) ---
        public uint[] GetTexturesForApiId(uint apiId) { ... }
        public List<VertexBufferBinding> GetVertexBuffers(uint apiId) { ... }
        public IndexBufferBinding?       GetIndexBuffer(uint apiId) { ... }
        public List<ShaderInfo>          GetShadersForPipeline(uint pipelineId) { ... }
        public List<TextureInfo>         GetTextureDetails(uint[] ids) { ... }
        public List<RenderTargetInfo>    GetRenderTargets(uint apiId) { ... }
        public DescriptorBindingSummary  GetBindingSummary(uint apiId) { ... }

        // --- Pipeline helpers ---
        public (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByResourceID(uint pipelineID) { ... }
    }
}
```

---

### 1d. `Data/SdpDatabase.Shaders.cs` — Shader Query Domain

Absorbs SQL from `ShaderExtractor` (private methods become public):

```csharp
namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // Returns shader stages for a pipeline (stage type, moduleID, entrypoint)
        public List<ShaderStageRecord> GetShaderStages(uint pipelineId) { ... }

        // Returns raw SPIR-V bytes for a shader module (null = not found)
        public byte[]? ReadSpirvBytes(ulong shaderModuleId) { ... }

        // Returns disassembly text for a pipeline stage (null = not available)
        public string? ReadShaderDisasm(uint pipelineId, uint stageType) { ... }

        // Returns pipeline ID resolved from an encoded draw call ID
        public uint? ResolvePipelineFromDrawCall(string drawCallId) { ... }
        public uint? GetPipelineByGlobalIndex(int index) { ... }
    }
}
```

---

### 1e. `Data/SdpDatabase.Textures.cs` — Texture Query Domain

Absorbs SQL from `TextureExtractor`:

```csharp
namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        public TextureMetadata? GetTextureMetadata(ulong resourceId) { ... }
        // Returns raw pixel/compressed bytes for a texture resource
        public byte[]? ReadTextureBytes(ulong resourceId) { ... }
    }
}
```

---

### 1f. `Data/SdpDatabase.Buffers.cs` — Buffer Query Domain

Absorbs SQL from `MeshExtractor`:

```csharp
namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // Read multi-chunk binary data from VulkanSnapshotByteBuffers
        public byte[]? ReadBufferBytes(uint resourceId) { ... }

        // Load pipeline vertex input binding + attribute descriptions
        public (List<VertexBindingDesc>, List<VertexAttrDesc>) LoadVertexInputState(uint pipelineId) { ... }

        // List DrawCallApiIDs that have entries in DrawCallVertexBuffers
        public List<uint> GetDrawCallsWithVertexBuffers(int maxCount = 500) { ... }
    }
}
```

---

### ~~1g. `Data/SdpDatabase.Capture.cs`~~ — ELIMINATED

`SdpDatabase.Capture.cs` is not needed. All three methods from `DatabaseQueryService` have
a natural home in existing partial files:

| Method | Destination | Reason |
|--------|-------------|--------|
| `GetDrawCallIds(cmdBufferFilter)` + 3 private helpers (`GetDrawCallIdsFromParameters`, `GetDrawCallIdsFromSCOPE`, `GenerateDrawCallIdsFromPipelines`) | `SdpDatabase.DrawCalls.cs` | DC-list query is the "list" side of the same domain as DC-detail query |
| `GetAllTables(conn)` | `SdpDatabase.Schema.cs` | Schema introspection |
| `GetMetadata()` | `SdpDatabase.Schema.cs` | Calls `GetAllTables`, same domain |

Extension to `SdpDatabase.DrawCalls.cs` (appended):
```csharp
// Returns DrawCallApiID strings for the capture (3-priority-level logic)
// Priority: DrawCallParameters > SCOPEDrawStages > pipeline count
public List<string> GetDrawCallIds(int? cmdBufferFilter = null) { ... }
private List<string> GetDrawCallIdsFromParameters(SQLiteConnection conn, int? cmdBufferFilter) { ... }
private List<string> GetDrawCallIdsFromSCOPE(SQLiteConnection conn) { ... }
private List<string> GenerateDrawCallIdsFromPipelines(SQLiteConnection conn) { ... }
```

Extension to `SdpDatabase.Schema.cs` (appended):
```csharp
public List<string>        GetAllTables() { ... }
public DatabaseMetadata    GetMetadata()  { ... }
```

---

## Phase 2 — Extractor Refactor (SQL Removed)

**Files:** `ShaderExtractor.cs`, `TextureExtractor.cs`, `MeshExtractor.cs`

All direct SQL methods are deleted. Each extractor stores `SdpDatabase _db`, calls its domain methods.

### ShaderExtractor

```csharp
public ShaderExtractor(SdpDatabase db) { _db = db; }

// ExtractShadersForPipeline becomes:
private bool ExtractShadersForPipeline(uint pipelineId, string outputDir)
{
    var stages = _db.GetShaderStages(pipelineId);          // SQL moved to SdpDatabase.Shaders.cs
    foreach (var stage in stages)
    {
        byte[]? spirv = _db.ReadSpirvBytes(stage.ShaderModuleID);  // SQL moved
        if (spirv != null) File.WriteAllBytes(spvPath, spirv);
        string? disasm = _db.ReadShaderDisasm(pipelineId, stage.StageType);  // SQL moved
        // spirv-cross decompilation stays here (file I/O only)
    }
}

// Backward-compat (requires using SnapdragonProfilerCLI.Data; in the file):
public ShaderExtractor(string databasePath, int captureId = 3)
    : this(new Data.SdpDatabase(databasePath, (uint)captureId)) { }
```

### TextureExtractor

```csharp
public TextureExtractor(SdpDatabase db) { _db = db; }

// GetTextureData becomes: return _db.ReadTextureBytes(resourceId);
// GetTextureMetadata becomes: return _db.GetTextureMetadata(resourceId);
// All format conversion + file-save logic stays in TextureExtractor

// Backward-compat (requires using SnapdragonProfilerCLI.Data; in the file):
public TextureExtractor(string databasePath, int captureId = 3)
    : this(new Data.SdpDatabase(databasePath, (uint)captureId)) { }
```

### MeshExtractor

```csharp
public MeshExtractor(SdpDatabase db) { _db = db; }

// ReadBufferBytes     → _db.ReadBufferBytes(resourceId)
// LoadVertexInputState → _db.LoadVertexInputState(pipelineId)
// GetDrawCallsWithVertexBuffers → _db.GetDrawCallsWithVertexBuffers(maxCount)
// ExtractMesh still calls DrawCallQueryService → which now uses same _db instance

// Backward-compat (requires using SnapdragonProfilerCLI.Data; in the file):
public MeshExtractor(string databasePath, int captureId = 3)
    : this(new Data.SdpDatabase(databasePath, (uint)captureId)) { }
```

---

## Phase 3 — DrawCallQueryService (Thin Caller)

All SQL private methods moved to `SdpDatabase.DrawCalls.cs`.

`DrawCallQueryService` becomes a thin orchestration wrapper:

```csharp
public class DrawCallQueryService
{
    private readonly SdpDatabase _db;
    public DrawCallQueryService(SdpDatabase db) { _db = db; }

    // Public API unchanged (other callers not affected)
    public DrawCallInfo? GetDrawCallInfo(string drawCallNumber)
        => _db.GetDrawCallInfo(drawCallNumber);

    public uint[] GetTexturesForDrawCall(string drawCallNumber)
        => _db.GetDrawCallInfo(drawCallNumber)?.TextureIDs ?? Array.Empty<uint>();

    // etc. — all delegate to _db
}
```

Public method signatures change:
- OLD: `GetDrawCallInfo(string dbPath, uint captureId, string drawCallNumber)`
- NEW: `GetDrawCallInfo(string drawCallNumber)` — captureId/dbPath supplied via constructor `_db`

---

## Phase 4 — DatabaseQueryService (Thin Caller, No Capture.cs needed)

SQL split to existing partial files (`DrawCalls.cs` and `Schema.cs`).
`DatabaseQueryService` becomes three one-liner delegates + one deleted method:

```csharp
public class DatabaseQueryService
{
    private readonly SdpDatabase _db;
    private readonly ILogger     _logger;

    public DatabaseQueryService(SdpDatabase db, ILogger logger)
    { _db = db; _logger = logger; }

    // OpenDatabase() DELETED — DB is already bound in SdpDatabase constructor

    // GetDrawCallIds SQL → SdpDatabase.DrawCalls.cs
    public List<string> GetDrawCallIds(uint captureId, int? cmdBufferFilter = null)
        => _db.GetDrawCallIds(cmdBufferFilter);  // captureId from _db.CaptureId

    // TableExists wrapper → SdpDatabase.Schema.cs static method
    public bool TableExists(string tableName)
    {
        using var conn = _db.OpenConnection();
        return SdpDatabase.TableExists(conn, tableName);
    }

    // GetMetadata → SdpDatabase.Schema.cs
    public DatabaseMetadata GetMetadata() => _db.GetMetadata();
}
```

`captureId` parameter on `GetDrawCallIds` is now ignored (compatibility shim) — `_db.CaptureId`
is used internally. Can be removed later once all callers are updated.

---

## Phase 5 — AnalysisPipeline.cs (call sites)

Current construction pattern (spread across Step 1.5 and Step 3.5 lambdas):

```csharp
var shExt  = new Tools.ShaderExtractor(dbPath, (int)captureId) { ... };
var texExt = new Tools.TextureExtractor(dbPath, (int)captureId);
var ext    = new Tools.MeshExtractor(dbPath!, (int)captureId);
```

After refactor — create one `SdpDatabase` instance early, pass everywhere:

```csharp
// After OpenDatabase() -- SdpDatabase now in Data namespace:
var db = new Data.SdpDatabase(dbPath!, captureId);

// Step 1.5 shader lambda (Tools namespace unchanged):
var shExt = new Tools.ShaderExtractor(db) { SpirvCrossPath = ..., ShaderOutputFormat = ... };

// Step 1.5 texture lambda:
var texExt = new Tools.TextureExtractor(db);

// Step 3.5 mesh lambda:
var ext = new Tools.MeshExtractor(db);

// Step 1 (DrawCallQueryService) -- remove dbPath/captureId params:
var dcSvc = new DrawCallQueryService(db);

// DatabaseQueryService -- inject db instead of calling OpenDatabase():
var dbSvc = new DatabaseQueryService(db, logger);
```

Note: Add `using SnapdragonProfilerCLI.Data;` at the top of `AnalysisPipeline.cs`.
`using SnapdragonProfilerCLI.Tools;` is already present and stays for Extractor types.

---

## Phase 6 — ShaderExtractor Read Only Fix

While changing the constructor, also fix the missing `Read Only=True`:

```csharp
// ShaderExtractor reads only — all internal query methods use read-only connection
using var conn = _db.OpenConnection(readOnly: true);
```

This eliminates the brief RESERVED lock that Windows SQLite acquires when a connection
opens without `Read Only=True`.

---

## Out of Scope

| Class | Reason |
|-------|--------|
| `CsvToDbService` | Write path; single connection for transactional multi-CSV import. Keep as-is. |
| `DataExportService` | Capture path; uses `System.Data.SQLite` namespace directly (different assembly). Separate concern. |

---

## Phase 6 — ShaderExtractor Read Only Fix

Now automatic: since `ShaderExtractor` calls `_db.OpenConnection(readOnly: true)` (default),
the missing `Read Only=True` is fixed as a side-effect of Phase 2.

---

## Affected Files Summary

| File | Change |
|------|--------|
| `source/Data/SdpDatabase.cs` | **NEW** — core factory (partial class root) |
| `source/Data/SdpDatabase.Schema.cs` | **NEW** — `TableExists`, `ColumnExists`, `FindColumn`, `GetFormatName`, `GetShaderStageName` |
| `source/Data/SdpDatabase.DrawCalls.cs` | **NEW** — all DC/pipeline/texture/shader/VB queries from `DrawCallQueryService` |
| `source/Data/SdpDatabase.Shaders.cs` | **NEW** — `GetShaderStages`, `ReadSpirvBytes`, `ReadShaderDisasm`, `ResolvePipelineFromDrawCall` from `ShaderExtractor` |
| `source/Data/SdpDatabase.Textures.cs` | **NEW** — `GetTextureMetadata`, `ReadTextureBytes` from `TextureExtractor` |
| `source/Data/SdpDatabase.Buffers.cs` | **NEW** — `ReadBufferBytes`, `LoadVertexInputState`, `GetDrawCallsWithVertexBuffers` from `MeshExtractor` |
| `source/Data/SdpDatabase.DrawCalls.cs` | also absorbs `GetDrawCallIds` + 3 private DC-list helpers from `DatabaseQueryService` |
| `source/Data/SdpDatabase.Schema.cs` | also absorbs `GetAllTables`, `GetMetadata` from `DatabaseQueryService` |
| ~~`source/Data/SdpDatabase.Capture.cs`~~ | **ELIMINATED** — no unique domain; content distributed above |
| `source/Tools/ShaderExtractor.cs` | SQL removed; constructor → `SdpDatabase`; adds `using SnapdragonProfilerCLI.Data;` |
| `source/Tools/TextureExtractor.cs` | SQL removed; constructor → `SdpDatabase`; adds `using SnapdragonProfilerCLI.Data;` |
| `source/Tools/MeshExtractor.cs` | SQL removed; constructor → `SdpDatabase`; adds `using SnapdragonProfilerCLI.Data;` |
| `source/Services/Analysis/DrawCallQueryService.cs` | SQL removed; thin delegate to `SdpDatabase`; adds `using SnapdragonProfilerCLI.Data;` |
| `source/Services/Analysis/DatabaseQueryService.cs` | SQL removed; thin delegate to `SdpDatabase`; remove `OpenDatabase()` |
| `source/Analysis/AnalysisPipeline.cs` | Add `using SnapdragonProfilerCLI.Data;`; create `Data.SdpDatabase` once after DB open; pass to all children |

## Out of Scope (unchanged)

| Class | Reason |
|-------|--------|
| `CsvToDbService` | Write path; single connection, transactional multi-CSV import. Keep as-is. |
| `DataExportService` | Uses `System.Data.SQLite` namespace directly (different assembly linkage). Separate concern. |

---

## Key Design Decisions

1. **Separate `Data` namespace** — `SdpDatabase` lives in `SnapdragonProfilerCLI.Data` under `source/Data/`. This keeps the data access layer architecturally separate from `Tools` (which contains execution logic: ShaderExtractor, TextureExtractor, MeshExtractor). Callers in `Tools` and `Services` add `using SnapdragonProfilerCLI.Data;`.
2. **Partial class, not sub-classes** — keeps `db.GetShaderStages()` callable directly; avoids `db.Shaders.GetStages()` double-indirection.
3. **DTOs stay in `Models/`** — `DrawCallInfo`, `ShaderInfo`, `TextureInfo`, `VertexBufferBinding`, etc. are not moved.
4. **Backward-compat constructors** — all three Extractors keep `(string databasePath, int captureId)` constructor wrapping `new SdpDatabase(...)` so standalone CLI tool modes continue working without AnalysisPipeline.
5. **`DrawCallQueryService` remains** — AnalysisPipeline injects it, and its public method signatures are the stable API surface; it becomes a thin shell over `SdpDatabase`.
6. **`SdpDatabase.Capture.cs` eliminated** — `DatabaseQueryService` had 4 public methods; each maps cleanly to an existing domain file (`DrawCalls.cs` for DC-list logic, `Schema.cs` for structural queries). No new file needed.

---

## Phase 7 — Pre-Analysis Table Validation

**Trigger**: Immediately after `SdpDatabase` is constructed at the top of `RunAnalysis`.

**Based on**: `FINDING-2026-04-08-table-validation.md`

### 7a. New method: `SdpDatabase.ValidateForAnalysis(ILogger logger)` → `SdpDatabase.Schema.cs`

```csharp
/// <summary>
/// Checks that all tables required for analysis are present.
/// Logs FATAL / ERROR / WARNING messages and throws if no DC source exists at all.
/// Should be called once per SdpDatabase instance before running analysis.
/// </summary>
public void ValidateForAnalysis(ILogger logger)
{
    using var conn = OpenConnection();

    // ── Group 1: DC source — at least one MUST exist ─────────────────
    bool hasParams   = TableExists(conn, "DrawCallParameters");
    bool hasScope    = TableExists(conn, "SCOPEDrawStages");
    bool hasPipelines= TableExists(conn, "VulkanSnapshotGraphicsPipelines");

    if (!hasParams && !hasScope && !hasPipelines)
        throw new InvalidOperationException(
            "FATAL: No DrawCall source table found " +
            "(DrawCallParameters, SCOPEDrawStages, VulkanSnapshotGraphicsPipelines all absent). " +
            "The SDP file may be incomplete or corrupt.");

    // ── Group 2: CSV-imported custom tables ──────────────────────────
    // Detect whether CSV import has ever been run by checking DrawCallParameters.
    // If absent, all other CSV tables are almost certainly absent too.
    if (!hasParams)
        logger.Warning(
            "  ⚠ [ERROR]  Table 'DrawCallParameters' missing — DC list will be inaccurate.\n" +
            "             → Run CSV import before analysis: SDPCLI.exe import <sdpPath>\n" +
            "             → Or set AutoImportCsv=true in config.ini");

    CheckCsvTable(conn, logger, "DrawCallBindings",
        "per-DC pipeline and texture binding — PipelineID=0, no textures per DC");
    CheckCsvTable(conn, logger, "DrawCallVertexBuffers",
        "vertex buffer bindings — mesh extraction will produce 0 meshes");
    CheckCsvTable(conn, logger, "DrawCallIndexBuffers",
        "index buffer bindings — IB unavailable (VB-only meshes still possible)");
    CheckCsvTable(conn, logger, "DrawCallRenderTargets",
        "render target info — RenderTargets will be empty");
    CheckCsvTable(conn, logger, "PipelineVertexInputBindings",
        "vertex input stride/rate — mesh geometry accuracy degraded");
    CheckCsvTable(conn, logger, "PipelineVertexInputAttributes",
        "vertex attribute location/format — mesh position channel not identified");

    // ── Group 3: Native SDP tables ───────────────────────────────────
    CheckNativeTable(conn, logger, "VulkanSnapshotGraphicsPipelines",
        "pipeline resolution unavailable");
    CheckNativeTable(conn, logger, "VulkanSnapshotShaderStages",
        "shader stage query unavailable — shader extraction will be skipped");
    CheckNativeTable(conn, logger, "VulkanSnapshotByteBuffers",
        "CRITICAL — no SPIR-V/texture/buffer binary data; extraction completely disabled");
    CheckNativeTable(conn, logger, "VulkanSnapshotTextures",
        "texture metadata (width/height/format) unavailable");
    CheckNativeTable(conn, logger, "VulkanSnapshotImageViews",
        "image view resolution unavailable — texture IDs may not resolve");

    logger.Info("  ✓ Database table validation complete.");
}

private static void CheckCsvTable(SQLiteConnection conn, ILogger logger,
    string tableName, string impact)
{
    if (!TableExists(conn, tableName))
        logger.Warning($"  ⚠ [WARN/CSV]  '{tableName}' missing — {impact}");
}

private static void CheckNativeTable(SQLiteConnection conn, ILogger logger,
    string tableName, string impact)
{
    if (!TableExists(conn, tableName))
        logger.Warning($"  ⚠ [WARN/SDP]  '{tableName}' missing — {impact}");
}
```

### 7b. Call site in `AnalysisPipeline.RunAnalysis`

```csharp
// Setup: locate + open DB
string? dbPath = sdpFileService.FindDatabasePath(sdpPath);
if (string.IsNullOrEmpty(dbPath))
    throw new Exception("sdp.db not found in .sdp file");

var db = new Data.SdpDatabase(dbPath!, captureId);    // Phase 5

// ── Pre-flight: table validation ─────────────────────────────────
logger.Info("Pre-flight: Validating database tables...");
db.ValidateForAnalysis(logger);                        // Phase 7 — throws on FATAL

dbQueryService = new DatabaseQueryService(db, logger); // Phase 4
// ... rest of pipeline
```

### 7c. Expected Console Output

```
Pre-flight: Validating database tables...
  ⚠ [WARN/CSV]  'DrawCallBindings' missing — per-DC pipeline and texture binding — PipelineID=0, no textures per DC
  ⚠ [WARN/CSV]  'DrawCallVertexBuffers' missing — vertex buffer bindings — mesh extraction will produce 0 meshes
  ⚠ [WARN/CSV]  'PipelineVertexInputBindings' missing — vertex input stride/rate — mesh geometry accuracy degraded
  ⚠ [WARN/CSV]  'PipelineVertexInputAttributes' missing — vertex attribute location/format — mesh position channel not identified
  ✓ Database table validation complete.
```

Or when CSV import was completely skipped:
```
Pre-flight: Validating database tables...
  ⚠ [ERROR]  Table 'DrawCallParameters' missing — DC list will be inaccurate.
             → Run CSV import before analysis: SDPCLI.exe import <sdpPath>
             → Or set AutoImportCsv=true in config.ini
  ⚠ [WARN/CSV]  'DrawCallBindings' missing — ...
  ⚠ [WARN/CSV]  'DrawCallVertexBuffers' missing — ...
  ... (all 6 CSV tables)
  ✓ Database table validation complete.
```

### 7d. Affected Files

| File | Change |
|------|--------|
| `source/Data/SdpDatabase.Schema.cs` | Add `ValidateForAnalysis(ILogger)`, `CheckCsvTable`, `CheckNativeTable` |
| `source/Analysis/AnalysisPipeline.cs` | Add `db.ValidateForAnalysis(logger)` call immediately after `SdpDatabase` construction |

## Implementation requires the Executor agent.
