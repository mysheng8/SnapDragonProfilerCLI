---
type: finding
topic: SQLite DB access fragmentation across analysis and extraction classes
status: investigated
related_paths:
  - SDPCLI/source/Services/Analysis/DatabaseQueryService.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Services/Capture/DataExportService.cs
  - SDPCLI/source/Services/Capture/CsvToDbService.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
related_tags:
  - database
  - sqlite
  - architecture
  - refactor
  - connection-factory
summary: |
  DB access is spread across 7 classes with no shared abstraction. Each class manages
  its own connection strings, captureId, and open/dispose lifecycle independently.
  ShaderExtractor lacks Read Only=True. DatabaseQueryService is stateful (stores dbPath
  as field); DrawCallQueryService and Extractors are stateless (dbPath per-call or per-ctor).
  No unified place to change connection settings or add cross-cutting concerns (logging, metrics).
last_updated: 2026-04-08
---

# Finding: SQLite DB Access Fragmentation

## 1. Current Access Map

| Class | Namespace | How dbPath arrives | captureId | Read Only | Writes DB |
|-------|-----------|--------------------|-----------|-----------|-----------|
| `DatabaseQueryService` | Services.Analysis | `OpenDatabase(path)` stores in field | per-call param | ❌ | ❌ |
| `DrawCallQueryService` | Services.Analysis | per-call `dbPath` param | per-call `captureId` | ❌ (via `Open(dbPath)` helper) | ❌ |
| `ShaderExtractor` | Tools | constructor `_databasePath` field | constructor `_captureId` field | ❌ **missing** | ❌ |
| `TextureExtractor` | Tools | constructor `_databasePath` field | constructor `_captureId` field | ✅ | ❌ |
| `MeshExtractor` | Tools | constructor `_databasePath` field | constructor `_captureId` field | ✅ | ❌ |
| `DataExportService` | Services.Capture | per-method `dbPath` param | per-method `captureId` | ❌ | ❌ (SELECT+CSV) |
| `CsvToDbService` | Services.Capture | per-method `dbPath` param | N/A | N/A | ✅ INSERT |

**Total: 7 classes each managing their own `SQLiteConnection` construction.**

---

## 2. Connection String Inconsistencies

```
DatabaseQueryService  : "Data Source={path};Version=3;"
DrawCallQueryService  : "Data Source={path};Version=3;"           (via Open() helper)
ShaderExtractor       : "Data Source={path};Version=3;"           ← NO Read Only
TextureExtractor      : "Data Source={path};Version=3;Read Only=True;"
MeshExtractor         : "Data Source={path};Version=3;Read Only=True;"
DataExportService     : "Data Source={path}"                      ← different namespace, no Version=3
CsvToDbService        : "Data Source={path};Version=3;"           (write path, correct)
```

Three distinct formats. `DataExportService` uses `System.Data.SQLite` namespace directly
vs. `SQLite.NET` in all others.

---

## 3. Lifecycle Inconsistencies

- `DatabaseQueryService` is **stateful** — `OpenDatabase()` must be called before use;
  dbPath stored in `this.dbPath`. All other callers pass dbPath per-call.
- Extractors receive `(dbPath, captureId)` in constructor — constructor-scoped state.
- `DrawCallQueryService` is fully stateless (no fields) — can be injected as singleton.
- No class reuses a connection across calls. All open-use-dispose per query method.

---

## 4. Problems

1. **No single place to change connection settings** — adding WAL mode, busy timeout,
   cache size, or connection pooling requires touching 7 files.
2. **ShaderExtractor missing `Read Only=True`** — SQLite will upgrade to a RESERVED lock
   on write intent check at open time on Windows, causing brief block for other readers.
3. **Parallel contention risk** — during `Parallel.ForEach` for mesh/texture/shader,
   each worker opens a new connection. With SQLite's default journal mode (DELETE),
   concurrent readers on Windows can cause spurious `SQLITE_BUSY` for the first lock
   acquisition even in read-only access.
4. **No cross-cutting concerns** — no place to add query timing, retry-on-busy, or
   per-query debug logging uniformly.
5. **Constructor-scoped state in Extractors** vs. per-call state in QueryServices —
   inconsistent injection model, harder to test.

---

## 5. What Does NOT Need Unification

- `CsvToDbService` — write path with different lifecycle (per-import, single connection
  shared across all CSV imports for transactional integrity). Keep separate.
- `DataExportService` — capture path, not analysis path. Different SQLite namespace.
  Low priority to unify; only needs connection string fix.

---

## 6. Desired End State

One `SdpDatabase` context object that:
- Holds `string DbPath` and `uint CaptureId`
- Creates connections on demand via `OpenConnection(bool readOnly = true)`
- Single location for connection string, `Read Only`, `Journal Mode`, `Busy Timeout`
- Replaces `(string databasePath, int captureId)` in all Extractor constructors
- Replaces the per-call `dbPath` parameter in `DrawCallQueryService` public methods
- Replaces the `OpenDatabase()` / field pattern in `DatabaseQueryService`

Callers in `AnalysisPipeline.cs` pass one `SdpDatabase db` instance down to all children.
