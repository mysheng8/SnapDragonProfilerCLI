---
type: finding
topic: missing pre-analysis table existence validation — CSV custom tables and native SDP tables
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Capture/CsvToDbService.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
related_tags:
  - validation
  - sqlite
  - csv-import
  - analysis-pipeline
  - table-existence
summary: |
  AnalysisPipeline has no pre-flight table check before running analysis.
  Each class guards its own individual queries with scattered TableExists() calls,
  but there is no unified early warning when CSV-imported tables are missing entirely.
  When these tables are absent, the pipeline runs silently with degraded output
  (0 textures, 0 VBs, empty mesh list, no metrics join) without telling the user why.
last_updated: 2026-04-08
---

# Finding: Missing Pre-Analysis Table Validation

## 1. Complete Table Inventory

### 1a. CSV-Imported Tables (from CsvToDbService.knownFiles)

These are ONLY present if `CsvToDbService.ImportAllCsvs()` has been run for the capture.
If the user runs analysis without first importing CSVs (or uses an old SDP without re-import),
ALL of these will be absent.

| Table | Used by | Impact if missing |
|-------|---------|-------------------|
| `DrawCallParameters` | `DatabaseQueryService.GetDrawCallIds` (Priority 1), `MeshExtractor.GetDrawCallsWithVertexBuffers` | **CRITICAL**: DC list falls back to SCOPEDrawStages or pipeline-count fallback — row count and ApiIDs may be wrong |
| `DrawCallBindings` | `DrawCallQueryService.GetPipelineFromBindings`, `GetTexturesForApiId`, `GetBindingSummary` | No per-DC pipeline/texture binding — all PipelineIDs = 0, TextureIDs = [] |
| `DrawCallVertexBuffers` | `DrawCallQueryService.GetVertexBuffers`, `MeshExtractor.GetDrawCallsWithVertexBuffers` | No VB data — mesh extraction produces 0 meshes; `VertexBuffers.Count == 0` for all DCs |
| `DrawCallIndexBuffers` | `DrawCallQueryService.GetIndexBuffer` | No IB data — all IndexBuffer = null |
| `DrawCallRenderTargets` | `DrawCallQueryService.GetRenderTargets` | No render target info |
| `PipelineVertexInputBindings` | `MeshExtractor.LoadVertexInputState` | Mesh OBJ exports have no vertex stride/format — geometry is likely garbage |
| `PipelineVertexInputAttributes` | `MeshExtractor.LoadVertexInputState` | No attribute location/format — position channel not identified |
| `DrawCallMetrics` | `MetricsCsvService` (via session CSV files, not DB) | N/A — metrics loaded from flat files, not from DB |

### 1b. Native SDP Tables (should always be present)

| Table | Used by | Impact if missing |
|-------|---------|-------------------|
| `VulkanSnapshotGraphicsPipelines` | `DrawCallQueryService`, `ShaderExtractor` | No pipeline resolution — DC list falls back to pipeline-count only |
| `VulkanSnapshotShaderStages` | `DrawCallQueryService.GetShadersForPipeline`, `ShaderExtractor.GetShaderStages` | No shader info per pipeline — shader extraction completely skipped |
| `VulkanSnapshotByteBuffers` | `ShaderExtractor.ExtractSpirv`, `TextureExtractor.GetTextureData`, `MeshExtractor.ReadBufferBytes` | **CRITICAL**: no SPIR-V / no texture pixels / no mesh geometry bytes |
| `VulkanSnapshotTextures` | `DrawCallQueryService.GetTextureDetails`, `TextureExtractor.GetTextureMetadata` | No texture metadata (width/height/format) |
| `VulkanSnapshotImageViews` | `DrawCallQueryService.ResolveImageViewIds`, `ResolveRenderTargetFormat` | Texture ID resolution from image views fails |
| `VulkanSnapshotComputePipelines` | `DrawCallQueryService` (optional), `ShaderExtractor` | Compute DCs show no pipeline — non-fatal |
| `SCOPEDrawStages` | `DatabaseQueryService.GetDrawCallIds` (Priority 2 fallback) | No fallback DC list for legacy captures (non-fatal if DrawCallParameters exists) |
| `VulkanSnapshotShaderData` | `ShaderExtractor.ExtractDisasm` | No disasm text — non-fatal (SPIR-V still works) |
| `VulkanSnapshotDescriptorSetBindings` | `DrawCallQueryService.GetTextureIdsFallback` | No fallback texture list — non-fatal |

---

## 2. Current Defensive Pattern (Scattered)

Each class individually guards at query time:
```csharp
// DrawCallQueryService (15 TableExists calls spread across private methods)
if (!TableExists(conn, "DrawCallVertexBuffers")) return list;
if (!TableExists(conn, "DrawCallRenderTargets")) return list;

// MeshExtractor (5 TableExists calls)
if (!TableExists(conn, "DrawCallVertexBuffers")) return result;
if (!TableExists(conn, "DrawCallParameters"))   return result;
if (TableExists(conn, "PipelineVertexInputBindings")) { ... }
```

**Problem**: All these silent returns mean the pipeline produces empty/degraded output
with no user-visible explanation. The user sees "0 meshes extracted" but doesn't know
whether it's because no mesh data exists or because CSV import was skipped.

---

## 3. Failure Modes Without Validation

### Scenario A: CSV import never run (most common mistake)
- `DrawCallParameters` absent → DC list from `SCOPEDrawStages` or pipeline-count (wrong IDs)
- `DrawCallBindings` absent → PipelineID = 0 for all DCs → no shaders found → labeling fails
- `DrawCallVertexBuffers` absent → mesh extraction: "Found 0 draw calls with vertex buffer bindings"
- Analysis completes with zero useful output; user has no idea why

### Scenario B: New SDP format — CSV not re-exported after update
- Only `PipelineVertexInputBindings` / `PipelineVertexInputAttributes` absent
- Mesh OBJ files written but contain malformed geometry (stride = 0, no attributes)
- Silent data corruption in output

### Scenario C: Old SDP — `VulkanSnapshotByteBuffers` absent  
- Shader extraction: 0 SPV files written → labeling rejects all DCs → all labeled "Unknown"
- Texture extraction: 0 PNGs written → JSON references broken paths

---

## 4. Desired Validation Design

### 4a. Severity Classification

```
FATAL   : none of the three DC-list sources exists → abort immediately
           (DrawCallParameters AND SCOPEDrawStages AND VulkanSnapshotGraphicsPipelines all absent)

ERROR   : CSV tables entirely absent (hint: run import first)
           DrawCallParameters    → DC list unreliable
           DrawCallBindings      → no per-DC pipeline/texture binding
           DrawCallVertexBuffers → no mesh extraction possible

WARNING : missing but partially compensated
           DrawCallIndexBuffers        → IB unavailable, VB-only meshes still work
           DrawCallRenderTargets       → no render target info
           PipelineVertexInputBindings → mesh geometry accuracy degraded
           PipelineVertexInputAttributes → mesh geometry accuracy degraded

INFO    : native tables that SHOULD always be present but are checked as a sanity check
           VulkanSnapshotGraphicsPipelines → pipeline lookup degraded
           VulkanSnapshotShaderStages      → no shader extraction
           VulkanSnapshotByteBuffers       → no binary data at all
           VulkanSnapshotTextures          → no texture metadata
```

### 4b. Hint Message for Missing CSV Tables

```
  ⚠ [ERROR]  Table 'DrawCallParameters' not found.
             → CSV tables were not imported. Run: SDPCLI.exe import <sdpPath>
             → Or set AutoImportCsv=true in config.ini
```

### 4c. Placement in AnalysisPipeline.RunAnalysis

```csharp
// Immediately after: dbQueryService.OpenDatabase(dbPath!);
ValidateDatabaseTables(dbPath!);
// → logs all warnings, throws if FATAL condition
```

### 4d. Implementation Location

`ValidateDatabaseTables(string dbPath)` as a private method on `AnalysisPipeline`.

After the SdpDatabase unification (PLAN-2026-04-08-unified-db-access.md), this becomes:
```csharp
db.ValidateForAnalysis(logger);
// → moves to SdpDatabase.Schema.cs as a public method
```

---

## 5. Tables to Check (Complete Ordered List)

```csharp
// Group 1 — DC source (at least one MUST exist or abort)
"DrawCallParameters"           // CSV imported — Priority 1 DC list
"SCOPEDrawStages"              // native SDP — Priority 2 DC list
"VulkanSnapshotGraphicsPipelines" // native SDP — Priority 3 DC list

// Group 2 — CSV imported tables (all 7 queryable ones)
"DrawCallParameters"           // also in Group 1
"DrawCallBindings"             // per-DC pipeline + texture binding
"DrawCallVertexBuffers"        // per-DC VB list
"DrawCallIndexBuffers"         // per-DC IB
"DrawCallRenderTargets"        // per-DC render targets
"PipelineVertexInputBindings"  // vertex input state for mesh
"PipelineVertexInputAttributes"// vertex input attributes for mesh

// Group 3 — native SDP tables
"VulkanSnapshotGraphicsPipelines" // also in Group 1
"VulkanSnapshotShaderStages"   // shader stage queries
"VulkanSnapshotByteBuffers"    // SPIR-V / texture / buffer bytes
"VulkanSnapshotTextures"       // texture metadata
"VulkanSnapshotImageViews"     // image view to texture ID resolution
```
