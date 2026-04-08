---
type: finding
topic: mesh export — current state and gaps for analysis pipeline integration
status: investigated
related_paths:
  - SDPCLI/source/Tools/MeshExtractor.cs
  - SDPCLI/source/Modes/MeshExtractionMode.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Services/Analysis/ReportGenerationService.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Models/VulkanSnapshotModel.cs
related_tags:
  - mesh
  - vertex-buffer
  - index-buffer
  - OBJ
  - drawcall-analysis
  - parallel
  - json
  - pipeline-integration
summary: |
  MeshExtractor.cs is fully implemented (OBJ from VB/IB binary via VulkanSnapshotByteBuffers).
  Step 3.5 in AnalysisPipeline already calls it, but only for top-5 DCs, into a per-capture
  folder, with a folder-level existence check and serial execution. mesh_files is absent from
  DrawCallAnalysis JSON. All four gaps have clear fix paths.
last_updated: 2026-04-08
---

# Finding: Mesh Export — Current State and Gaps for Analysis Pipeline Integration

## 1. What Is Already Implemented

### 1.1 MeshExtractor.cs (`source/Tools/MeshExtractor.cs`)

Fully functional extraction tool:

- `ExtractMesh(drawCallNumber, outputPath)` — single DrawCall → writes Wavefront OBJ
- `ExtractAllMeshes(outputDir, maxDrawCalls)` — batch mode, DrawCalls that have VB bindings
- `ReadBufferBytes(resourceId)` — reads raw VB/IB from `VulkanSnapshotByteBuffers` via:
  ```sql
  SELECT data FROM VulkanSnapshotByteBuffers
  WHERE captureID=? AND resourceID=? ORDER BY sequenceID
  ```
- `LoadVertexInputState(pipelineId)` — reads `PipelineVertexInputBindings` and
  `PipelineVertexInputAttributes` tables, returns stride + attribute list
- Full vertex attribute decoding: `R32G32B32_SFLOAT`, `R8G8B8A8_SNORM`, `R16G16_SFLOAT`,
  `R32G32B32A32_SFLOAT`, etc.
- Index buffer parsing: UINT16 and UINT32, with `firstIndex`/`vertexOffset` support
- Heuristic attribute role detection: position (location=0 or R32G32B32_SFLOAT), normal,
  UV (R32G32_SFLOAT or R16G16_SFLOAT)

**Thread safety**: `ReadBufferBytes` and `LoadVertexInputState` each open a NEW
`SQLiteConnection` per call — no shared mutable connection state. Output filenames are
unique per draw call. The class is safe for full parallel use.

### 1.2 MeshExtractionMode.cs (`source/Modes/MeshExtractionMode.cs`)

Standalone CLI mode (`-mode extract-mesh`) that:
- Extracts `sdp.db` from the ZIP
- Calls `MeshExtractor.ExtractMesh` or `ExtractAllMeshes`
- Not integrated into `AnalysisMode`/`AnalysisPipeline`

### 1.3 Step 3.5 in AnalysisPipeline.cs (lines 275–316)

Already calls `MeshExtractor` inside the analysis pipeline. Current behavior:

```
meshBaseDir = captureOutDir/meshes/          ← per-capture, NOT shared
scope       = top-5 DCs by GPU Clocks        ← not all non-compute DCs
existence   = Directory.Exists(meshBaseDir)  ← folder-level, too coarse
naming      = drawcall_{DrawCallNumber}.obj  ← DrawCallNumber ("1.1.5") not globally unique
parallelism = serial foreach                 ← no parallelism
```

---

## 2. Gaps

### Gap 1 — Scope: top-5 only

Current code picks **top-5 by GPU Clocks** (or first 5 if no metrics).  
Target: ALL non-compute DCs that have at least one vertex buffer binding.

**Filter definition:**
- `dc.ApiName.IndexOf("Dispatch", StringComparison.OrdinalIgnoreCase) < 0` — excludes compute
- `dc.VertexBuffers.Count > 0` — confirmed to have VB data in DrawCallVertexBuffers table

Note: DCs without VB bindings (e.g. full-screen quads driven by shadow-pass compute) emit no
geometry on the CPU path and have no data in `VulkanSnapshotByteBuffers` as vertex content.
`ExtractMesh` already handles this gracefully (returns `false`).

---

### Gap 2 — Location: per-capture vs. session-shared

`meshBaseDir = captureOutDir/meshes/`  
Two captures of the same object produce identical VB binary → identical OBJ content.

Same rationale as shaders/textures: move to **`sessionDir/meshes/`** (shared across all captures).

---

### Gap 3 — Existence check: folder-level

```csharp
else if (Directory.Exists(meshBaseDir))
    // SKIP the entire step
```

One existing OBJ blocks ALL extraction for new DCs. Must switch to **per-file existence check**:

```csharp
string objPath = Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj");
if (File.Exists(objPath)) { skip; continue; }
```

**File naming change**: `drawcall_{DrawCallNumber}.obj` → `mesh_{dc.ApiID}.obj`

- `dc.ApiID` is a `uint` directly from `DrawCallParameters.DrawCallApiID`
- Globally unique within a capture and consistent across re-analysis runs
- No dots or special chars — safe as a filename
- Example: `mesh_106974.obj`

---

### Gap 4 — No mesh_files in DrawCallAnalysis JSON

`GenerateLabeledMetricsJson` currently builds `shader_files` and `texture_files` per DC.
`mesh_files` field is absent.

Each DC's mesh file is deterministically at: `../../meshes/mesh_{dc.ApiID}.obj`  
(relative from `snapshot_{captureId}/DrawCallAnalysis_*.json` → `<sdpName>/meshes/`)

The method signature needs:
```csharp
public string GenerateLabeledMetricsJson(
    DrawCallAnalysisReport report,
    string captureOutDir,
    string shaderBaseDir,
    string textureBaseDir,
    string meshBaseDir)        // ← NEW
```

---

### Gap 5 — No parallelism

`MeshExtractor` is thread-safe (per-call independent SQLite connections + unique output paths).
The extraction loop should use `Parallel.ForEach` with a configurable degree, mirroring the
texture extraction pattern.

Degree recommendation: default 4 (I/O-bound, same as `TextureExtractionDegree`).

`Console.WriteLine` inside `MeshExtractor` cannot be replaced without modifying it; the output
will be interleaved under parallelism, which is acceptable for diagnostic logs.

---

## 3. DrawCallInfo Model Assessment

`DrawCallInfo` already carries:

| Field | Used for mesh? |
|-------|----------------|
| `ApiID` | ✅ Key for OBJ filename (`mesh_{ApiID}.obj`) |
| `ApiName` | ✅ Filter out "Dispatch" (compute) |
| `VertexBuffers` | ✅ Filter: `Count > 0` confirms VB present |
| `IndexBuffer` | ✅ Already in model, used by `ExtractMesh` |
| `VertexCount`, `IndexCount` | ✅ Already in model |

No new field is needed in `DrawCallInfo` or `DrawCallAnalysisReport`.  
The `mesh_files` list is composed at JSON generation time from `meshBaseDir` + `dc.ApiID`.

---

## 4. Relative Path from JSON to Mesh

```
<sdpName>/
  meshes/
    mesh_106974.obj       ← meshBaseDir (sessionDir/meshes/)
  snapshot_3/
    DrawCallAnalysis_2026-04-08_….json  ← captureOutDir
```

Relative path from JSON → mesh: `../../meshes/mesh_{dc.ApiID}.obj`

Same pattern as shaders (`../../shaders/`) and textures (`../../textures/`).

---

## 5. Summary of Required Changes

| # | File | Change |
|---|------|--------|
| 1 | `AnalysisPipeline.cs` | Move `meshBaseDir` to `sessionDir/meshes/` |
| 2 | `AnalysisPipeline.cs` | Expand scope: all non-compute DCs with `VertexBuffers.Count > 0` |
| 3 | `AnalysisPipeline.cs` | Per-file existence check: `File.Exists("mesh_{dc.ApiID}.obj")` |
| 4 | `AnalysisPipeline.cs` | File naming: `mesh_{dc.ApiID}.obj` |
| 5 | `AnalysisPipeline.cs` | Parallel: `Parallel.ForEach` + `Interlocked.Increment` + degree config |
| 6 | `AnalysisPipeline.cs` | Pass `meshBaseDir` to `GenerateLabeledMetricsJson` |
| 7 | `ReportGenerationService.cs` | Add `meshBaseDir` parameter, add `mesh_files` field per DC |
| 8 | `config.ini` | Add `MeshExtractionDegree=4` |
