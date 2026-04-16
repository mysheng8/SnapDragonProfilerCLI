---
type: finding
topic: complete raw data schema for analysis pipeline — SQLite tables, CSV files, extracted assets, JSON outputs
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Data/SdpDatabase.cs
  - SDPCLI/source/Data/SdpDatabase.Schema.cs
  - SDPCLI/source/Data/SdpDatabase.DrawCalls.cs
  - SDPCLI/source/Data/SdpDatabase.Shaders.cs
  - SDPCLI/source/Data/SdpDatabase.Textures.cs
  - SDPCLI/source/Data/SdpDatabase.Buffers.cs
  - SDPCLI/source/Models/DrawCallModels.cs
  - SDPCLI/source/Models/VulkanSnapshotModel.cs
  - SDPCLI/source/Services/Analysis/DrawCallAnalysisService.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Services/Analysis/MetricsQueryService.cs
  - SDPCLI/source/Services/Analysis/RawJsonGenerationService.cs
  - SDPCLI/source/Services/Analysis/StatusJsonService.cs
  - SDPCLI/source/Services/Capture/CsvToDbService.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Tools/MeshExtractor.cs
related_tags:
  - database
  - sqlite
  - csv
  - schema
  - json
  - shader
  - texture
  - mesh
  - drawcall-analysis
  - python
summary: |
  Complete inventory of every raw data artifact the analysis pipeline reads or writes.
  Covers: 9 SQLite tables (5 native SDP + 7 CSV-imported + 1 metrics), the exact CSV
  column headers for each CSV export, extracted file types and naming conventions,
  and the exact JSON field names for dc.json / shaders.json / textures.json /
  buffers.json / label.json / metrics.json / status.json and the legacy raw.json.
last_updated: 2026-04-15
---

# Finding: Raw Data Schema — Analysis Pipeline

## Problem Statement

Document every raw data artifact the SDPCLI analysis pipeline reads and writes
so that Python consumers can reliably parse them without inspecting C# source.

---

## 1. SQLite Database — `sdp.db`

The database is opened by `SdpDatabase` (read-only by default).
All tables are keyed by `captureID` (uint).

### 1.1 Native SDP Tables (SDK-written, fragile — format driven by Qualcomm SDK)

| Table | Key columns read by SDPCLI |
|---|---|
| `VulkanSnapshotGraphicsPipelines` | `captureID`, `resourceID` (pipelineID), `layoutID`, `renderPass` |
| `VulkanSnapshotComputePipelines` | `captureID`, `resourceID` (pipelineID), `layoutID` |
| `VulkanSnapshotShaderStages` | `captureID`, `pipelineID`, `stageType`, `shaderModuleID`, `pName`, `shaderIndex` |
| `VulkanSnapshotByteBuffers` | `captureID`, `resourceID`, `sequenceID`, `data` (BLOB) |
| `VulkanSnapshotTextures` | `captureID`, `resourceID`, `width`, `height`, `depth`, `format`, `layerCount`, `levelCount` |
| `VulkanSnapshotImageViews` | `captureID`, `resourceID`, `imageID`, `format` |
| `VulkanSnapshotShaderData` | `captureID`, `pipelineID`, `shaderStage`, `shaderDisasm` |
| `SCOPEDrawStages` | `captureID`, `drawCallID` (encoded uint64), `pipelineID` |
| `VulkanSnapshotDescriptorSetBindings` | `captureID`, `imageViewID` (join to ImageViews) |

`VulkanSnapshotByteBuffers` stores SPIR-V shader bytes, texture pixels, and vertex/index
buffer bytes all keyed by `resourceID`. Multiple rows per resource ordered by `sequenceID`
(chunks) — always read all chunks and concatenate.

### 1.2 CSV-Imported Tables (SDPCLI-written, stable — format driven by VulkanSnapshotModel)

These tables are created by `CsvToDbService.ImportAllCsvs` from the 7 CSV files
produced by `VulkanSnapshotModel.Export*`. Format is SDPCLI-controlled and stable.

#### `DrawCallParameters`

Columns (CSV header order):
```
DrawCallApiID, CaptureID, ApiName, SubmitIdx, CmdBufferIdx, DrawcallIdx,
VertexCount, IndexCount, InstanceCount, FirstVertex, FirstIndex, VertexOffset,
FirstInstance, DrawCount, GroupCountX, GroupCountY, GroupCountZ
```

Used by: `GetDrawCallIds` (primary DC source), `MetricsQueryService` (DrawcallIdx join).

#### `DrawCallBindings`

Columns (CSV header order):
```
DrawCallApiID, CaptureID, PipelineID, DescriptorSetID, SlotNum,
ImageViewID, BufferID, SamplerID, ImageLayout, TexBufferView,
Offset, Range, AccelStructID, TensorID, TensorViewID
```

Used by: `GetTexturesForApiId` (ImageViewID → VulkanSnapshotImageViews.imageID),
`GetPipelineFromBindings` (PipelineID), `GetBindingSummary` (TexBufferView, BufferID, Range).

#### `DrawCallRenderTargets`

Columns (CSV header order):
```
DrawCallApiID, CaptureID, RenderPassID, FramebufferID,
AttachmentIndex, AttachmentResourceID, AttachmentType
```

`AttachmentType` values: `"Color"`, `"DepthStencil"`.

Used by: `GetRenderTargets` → resolved with `VulkanSnapshotImageViews` + `VulkanSnapshotTextures`
to populate `width`, `height`, `FormatName` on `RenderTargetInfo`.

#### `DrawCallVertexBuffers`

Columns (CSV header order):
```
DrawCallApiID, CaptureID, Binding, BufferID
```

Used by: `GetVertexBuffers`, `GetDrawCallsWithVertexBuffers`, mesh extraction gate in pipeline.

#### `DrawCallIndexBuffers`

Columns (CSV header order):
```
DrawCallApiID, CaptureID, BufferID, Offset, IndexType
```

`IndexType` values: `"UINT16"`, `"UINT32"`, `"UINT8"`, `"NONE"`, `"NULL"`.

Used by: `GetIndexBuffer`, mesh extraction.

#### `PipelineVertexInputBindings`

Columns (CSV header order):
```
PipelineID, CaptureID, Binding, Stride, InputRate
```

`InputRate` values: `"VERTEX"` or `"INSTANCE"` (string) or numeric 0/1.

Used by: `LoadVertexInputState` → mesh extraction stride map.

#### `PipelineVertexInputAttributes`

Columns (CSV header order):
```
PipelineID, CaptureID, Location, Binding, Format, Offset
```

`Format` is a VkFormat integer (e.g. 106 = R32G32B32_SFLOAT).

Used by: `LoadVertexInputState` → mesh extraction attribute selection.

### 1.3 Metrics Table (SDPCLI-imported, stable)

#### `DrawCallMetrics`

Columns:
```
DrawID (= DrawcallIdx from DrawCallParameters), CaptureID, MetricName, Value
```

`MetricName` values are Adreno counter strings from `MetricsWhitelist` in config.ini.
The mapping from counter name to snake_case JSON key is defined in
`DrawCallMetrics.CounterToKey` (see section 5).

Join path: `DrawCallParameters.DrawcallIdx` = `DrawCallMetrics.DrawID`.
Used by: `MetricsQueryService.LoadMetrics`.

---

## 2. CSV Files (Snapshot Output — written by VulkanSnapshotModel.Export* methods)

These 7 CSVs are produced during snapshot capture and stored alongside `sdp.db`.
They are imported into SQLite by `CsvToDbService` before analysis.

| CSV Filename | First row (header) |
|---|---|
| `DrawCallParameters.csv` | `DrawCallApiID,CaptureID,ApiName,SubmitIdx,CmdBufferIdx,DrawcallIdx,VertexCount,IndexCount,InstanceCount,FirstVertex,FirstIndex,VertexOffset,FirstInstance,DrawCount,GroupCountX,GroupCountY,GroupCountZ` |
| `DrawCallBindings.csv` | `DrawCallApiID,CaptureID,PipelineID,DescriptorSetID,SlotNum,ImageViewID,BufferID,SamplerID,ImageLayout,TexBufferView,Offset,Range,AccelStructID,TensorID,TensorViewID` |
| `DrawCallRenderTargets.csv` | `DrawCallApiID,CaptureID,RenderPassID,FramebufferID,AttachmentIndex,AttachmentResourceID,AttachmentType` |
| `DrawCallVertexBuffers.csv` | `DrawCallApiID,CaptureID,Binding,BufferID` |
| `DrawCallIndexBuffers.csv` | `DrawCallApiID,CaptureID,BufferID,Offset,IndexType` |
| `PipelineVertexInputBindings.csv` | `PipelineID,CaptureID,Binding,Stride,InputRate` |
| `PipelineVertexInputAttributes.csv` | `PipelineID,CaptureID,Location,Binding,Format,Offset` |

There is also `DrawCallMetrics.csv` (8th file imported by `CsvToDbService`) whose
header is not defined by `VulkanSnapshotModel` — it comes from the SDK profiler export
and has columns: `DrawID, CaptureID, MetricName, Value`.

**Stability**: All 7 VulkanSnapshotModel CSVs are SDPCLI-controlled (stable).
`DrawCallMetrics.csv` format is SDK-driven (fragile — counter names may change).

---

## 3. Extracted Asset Files

### 3.1 ShaderExtractor

**Input tables**: `VulkanSnapshotShaderStages` (stage list), `VulkanSnapshotByteBuffers` (SPIR-V BLOB),
`VulkanSnapshotShaderData` (disassembly text, optional).

**Input fields**:
- `VulkanSnapshotShaderStages`: `captureID`, `pipelineID`, `stageType`, `shaderModuleID`, `pName`, `shaderIndex`
- `VulkanSnapshotByteBuffers`: `captureID`, `resourceID` (= shaderModuleID), `sequenceID`, `data`
- `VulkanSnapshotShaderData`: `captureID`, `pipelineID`, `shaderStage`, `shaderDisasm`

**Output files** (written to `sessionDir/shaders/`):

| Filename pattern | Content | Condition |
|---|---|---|
| `pipeline_{pipelineID}_{stage}.spv` | Raw SPIR-V binary | Always attempted |
| `pipeline_{pipelineID}_{stage}.hlsl` | HLSL source (spirv-cross) | When `ShaderOutputFormat=hlsl` or `both` |
| `pipeline_{pipelineID}_{stage}.glsl` | GLSL source (spirv-cross) | When `ShaderOutputFormat=glsl` or `both` |
| `pipeline_{pipelineID}_{stage}.disasm` | GPU assembly text | When `VulkanSnapshotShaderData` has a row |

Stage suffix values: `vert`, `frag`, `comp`, `tesc`, `tese`, `geom`, `rgen`, `rint`, `rahit`, `rchit`, `rmiss`.

**Stability**: Output naming convention is SDPCLI-controlled (stable). File existence depends
on SDK data being present.

### 3.2 TextureExtractor

**Input tables**: `VulkanSnapshotTextures` (metadata), `VulkanSnapshotByteBuffers` (pixel data).

**Input fields**:
- `VulkanSnapshotTextures`: `captureID`, `resourceID`, `width`, `height`, `depth`, `format`, `layerCount`, `levelCount`
- `VulkanSnapshotByteBuffers`: `captureID`, `resourceID`, `sequenceID`, `data`

**Output files** (written to `sessionDir/textures/`):

| Filename pattern | Content |
|---|---|
| `texture_{resourceID}.png` | PNG (RGBA8 via TextureConverter.dll conversion) |

For 3D textures (`depth > 1`), only the first depth slice is written.
For ASTC formats (VkFormat 157–184), an ASTC file header is prepended before conversion.

**Stability**: Naming convention is SDPCLI-controlled (stable). Pixel fidelity depends on
TextureConverter.dll format support.

### 3.3 MeshExtractor

**Input tables**: `DrawCallVertexBuffers`, `DrawCallIndexBuffers`, `PipelineVertexInputBindings`,
`PipelineVertexInputAttributes`, `VulkanSnapshotByteBuffers` (buffer binary data).

**Input fields**:
- Vertex buffer binaries keyed by `BufferID` from `DrawCallVertexBuffers`
- Index buffer binary keyed by `BufferID` from `DrawCallIndexBuffers`
- Vertex layout from `PipelineVertexInputBindings` (Binding, Stride, InputRate)
- Vertex attribute from `PipelineVertexInputAttributes` (Location, Binding, Format, Offset)

**Output files** (written to `sessionDir/meshes/`):

| Filename pattern | Content |
|---|---|
| `mesh_{ApiID}.obj` | Wavefront OBJ (positions + optional normals + optional UVs) |

OBJ header comments include: DrawCall number, ApiID, ApiName, PipelineID, vertex count, face count.

Position attribute selection heuristic:
1. Location=0 with format R32G32B32_SFLOAT (106) or R32G32B32A32_SFLOAT (109)
2. Location=0 (any format)
3. First R32G32B32_SFLOAT or R32G32B32A32_SFLOAT

**Stability**: Naming convention is SDPCLI-controlled (stable).

---

## 4. DrawCallAnalysisService — What It Reads

`DrawCallAnalysisService.AnalyzeAllDrawCalls(dbPath, captureId, cmdBufferFilter)` orchestrates:

1. `SdpDatabase.GetDrawCallIds(cmdBufferFilter)` — 3-priority fallback:
   - Priority 1: `SELECT DrawCallApiID FROM DrawCallParameters WHERE CaptureID=? ORDER BY rowid`
   - Priority 2: `SELECT DISTINCT drawCallID FROM SCOPEDrawStages WHERE captureID=?` (encodes submit/cmdbuf/draw indices)
   - Priority 3: Generate sequential strings 1..N from pipeline count

2. For each DC id: `SdpDatabase.GetDrawCallInfo(drawCallId)` — resolves:
   - `DrawCallParameters`: `ApiName`, `VertexCount`, `IndexCount`, `InstanceCount`
   - `DrawCallBindings`: `PipelineID`, `ImageViewID` list
   - `VulkanSnapshotImageViews`: resolves ImageViewID → imageID (textureID)
   - `VulkanSnapshotTextures`: metadata per textureID
   - `VulkanSnapshotShaderStages`: shader stages per pipelineID
   - `DrawCallRenderTargets`: render target list (resolved via ImageViews + Textures)
   - `DrawCallVertexBuffers`: vertex buffer bindings
   - `DrawCallIndexBuffers`: index buffer binding
   - `DrawCallBindings` (again): TexBufferView, BufferID, Range for BindingSummary

Returns `DrawCallAnalysisReport` with:
- `DrawCallResults: List<DrawCallInfo>` — one entry per DC
- `Statistics.TotalPipelines`, `TotalTextures`, `TotalShaders`, `AvgTexturesPerDrawCall`

---

## 5. DrawCallLabelService — Input to LLM

When LLM is enabled, `DrawCallLabelService.Label(dc, shaderBaseDir)` builds a prompt
containing these fields from `DrawCallInfo`:

```
dc.ApiName
dc.VertexCount, dc.IndexCount, dc.InstanceCount
dc.TextureIDs.Length
dc.Shaders[].ShaderStageName + EntryPoint
dc.RenderTargets[].AttachmentIndex, AttachmentType, Width, Height, FormatName
dc.VertexBuffers.Count
dc.IndexBuffer.IndexType  (or "none")
dc.BindingSummary.TypedBufferViewCount
dc.BindingSummary.SmallBufferCount
dc.BindingSummary.HasPerInstanceBuffer
```

Plus HLSL/GLSL shader source read from `shaderBaseDir/pipeline_{pipelineID}_*.hlsl`
(up to 2500 chars of resource declarations + 5000 chars of main() body).

Expected LLM JSON response fields:
```json
{
  "category":    "<string from allowed categories>",
  "subcategory": "<string>",
  "detail":      "<3-8 word description>",
  "reason_tags": ["tag1", "tag2"],
  "confidence":  0.9
}
```

---

## 6. MetricsQueryService — Join Tables

`MetricsQueryService.LoadMetrics(dbPath, captureId)` performs:

```sql
-- Step 1: DrawCallParameters -> apiId -> DrawcallIdx
SELECT DrawCallApiID, ApiName, DrawcallIdx FROM DrawCallParameters
[WHERE CaptureID={captureId}]

-- Step 2: DrawCallMetrics -> DrawcallIdx -> {MetricName -> Value}
SELECT DrawID, MetricName, Value FROM DrawCallMetrics
[WHERE CaptureID={captureId}]
[AND MetricName IN (MetricsWhitelist from config.ini)]
```

Join key: `DrawCallParameters.DrawcallIdx = DrawCallMetrics.DrawID`.

Result: `Dictionary<string, DrawCallMetrics>` keyed by `DrawCallApiID` string.

Counter name → snake_case JSON key mapping (`DrawCallMetrics.CounterToKey`):

| Counter Name (DB MetricName) | JSON Key |
|---|---|
| `Clocks` | `clocks` |
| `Preemptions` | `preemptions` |
| `Avg Preemption Delay` | `avg_preemption_delay` |
| `Read Total (Bytes)` | `read_total_bytes` |
| `Write Total (Bytes)` | `write_total_bytes` |
| `Texture Memory Read BW (Bytes)` | `tex_mem_read_bytes` |
| `Vertex Memory Read (Bytes)` | `vertex_mem_read_bytes` |
| `SP Memory Read (Bytes)` | `sp_mem_read_bytes` |
| `Avg Bytes / Fragment` | `avg_bytes_per_fragment` |
| `Avg Bytes / Vertex` | `avg_bytes_per_vertex` |
| `Fragments Shaded` | `fragments_shaded` |
| `Vertices Shaded` | `vertices_shaded` |
| `Reused Vertices` | `reused_vertices` |
| `Pre-clipped Polygons` | `pre_clipped_polygons` |
| `LRZ Pixels Killed` | `lrz_pixels_killed` |
| `Average Polygon Area` | `avg_polygon_area` |
| `Average Vertices / Polygon` | `avg_vertices_per_polygon` |
| `% Prims Clipped` | `prims_clipped_pct` |
| `% Prims Trivially Rejected` | `prims_trivially_rejected_pct` |
| `% Texture Fetch Stall` | `tex_fetch_stall_pct` |
| `% Texture L1 Miss` | `tex_l1_miss_pct` |
| `% Texture L2 Miss` | `tex_l2_miss_pct` |
| `% Texture Pipes Busy` | `tex_pipes_busy_pct` |
| `% Linear Filtered` | `linear_filtered_pct` |
| `% Nearest Filtered` | `nearest_filtered_pct` |
| `% Anisotropic Filtered` | `anisotropic_filtered_pct` |
| `% Non-Base Level Textures` | `non_base_level_tex_pct` |
| `L1 Texture Cache Miss Per Pixel` | `l1_tex_cache_miss_per_pixel` |
| `Textures / Fragment` | `textures_per_fragment` |
| `Textures / Vertex` | `textures_per_vertex` |
| `% Shaders Busy` | `shaders_busy_pct` |
| `% Shaders Stalled` | `shaders_stalled_pct` |
| `% Time ALUs Working` | `time_alus_working_pct` |
| `% Time EFUs Working` | `time_efus_working_pct` |
| `% Time Shading Vertices` | `time_shading_vertices_pct` |
| `% Time Shading Fragments` | `time_shading_fragments_pct` |
| `% Time Compute` | `time_compute_pct` |
| `% Shader ALU Capacity Utilized` | `shader_alu_capacity_pct` |
| `% Wave Context Occupancy` | `wave_context_occupancy_pct` |
| `% Instruction Cache Miss` | `instruction_cache_miss_pct` |
| `Fragment Instructions` | `fragment_instructions` |
| `Fragment ALU Instructions (Full)` | `fragment_alu_instr_full` |
| `Fragment ALU Instructions (Half)` | `fragment_alu_instr_half` |
| `Fragment EFU Instructions` | `fragment_efu_instructions` |
| `Vertex Instructions` | `vertex_instructions` |
| `ALU / Fragment` | `alu_per_fragment` |
| `ALU / Vertex` | `alu_per_vertex` |
| `EFU / Fragment` | `efu_per_fragment` |
| `EFU / Vertex` | `efu_per_vertex` |
| `% Vertex Fetch Stall` | `vertex_fetch_stall_pct` |
| `% Stalled on System Memory` | `stalled_on_system_mem_pct` |

---

## 7. RawJsonGenerationService — Output JSON Schemas

### 7.1 Sub-JSON files (schema 3.0) — current production format

All sub-JSON files share the same root envelope:

```json
{
  "schema_version": "3.0",
  "snapshot_id":    <uint captureId>,
  "sdp_name":       "<string>",
  "generated_at":   "2026-04-15T10:00:00Z",
  "total_dc_count": <int>,
  "draw_calls":     [ ... ]
}
```

Join key across all sub-JSONs: `api_id` (uint) = `DrawCallApiID`.
Secondary key: `dc_id` (string) = encoded draw call number (e.g. "1.1.5" or "106974").

#### `dc.json` — draw_calls[] entry

```json
{
  "dc_id":          "<string>",
  "api_id":         <uint>,
  "api_name":       "<string>",
  "pipeline_id":    <uint>,
  "layout_id":      <uint>,
  "render_pass_id": <uint>,
  "vertex_count":   <uint>,
  "index_count":    <uint>,
  "instance_count": <uint>,
  "render_targets": [
    {
      "attachment_index": <uint>,
      "attachment_type":  "<Color|Depth|Stencil|DepthStencil>",
      "resource_id":      <uint>,
      "renderpass_id":    <uint>,
      "framebuffer_id":   <uint>,
      "width":            <uint>,
      "height":           <uint>,
      "format":           "<string>"
    }
  ],
  "binding_summary": {
    "typed_buffer_view_count": <int>,
    "small_buffer_count":      <int>,
    "has_per_instance_buffer": <bool>
  }
}
```

#### `shaders.json` — draw_calls[] entry

```json
{
  "dc_id":       "<string>",
  "api_id":      <uint>,
  "pipeline_id": <uint>,
  "shader_stages": [
    {
      "stage":       "<Vertex|Fragment|Compute|...>",
      "module_id":   <ulong>,
      "entry_point": "<string>",
      "file":        "../../shaders/pipeline_{id}_{stage}.hlsl"
    }
  ],
  "shader_files": ["../../shaders/pipeline_{id}_{stage}.spv", ...]
}
```

#### `textures.json` — draw_calls[] entry

```json
{
  "dc_id":       "<string>",
  "api_id":      <uint>,
  "texture_ids": [<uint>, ...],
  "textures": [
    {
      "texture_id": <uint>,
      "width":      <uint>,
      "height":     <uint>,
      "depth":      <uint>,
      "format":     "<string>",
      "layers":     <uint>,
      "levels":     <uint>
    }
  ],
  "texture_files": ["../../textures/texture_{id}.png", ...]
}
```

#### `buffers.json` — draw_calls[] entry

```json
{
  "dc_id":    "<string>",
  "api_id":   <uint>,
  "vertex_buffers": [
    { "binding": <uint>, "buffer_id": <uint> }
  ],
  "index_buffer": {
    "buffer_id":  <uint>,
    "offset":     <uint>,
    "index_type": "<UINT16|UINT32>"
  },
  "mesh_file": "../../meshes/mesh_{api_id}.obj"
}
```

`index_buffer` and `mesh_file` are `null` / omitted when absent (NullValueHandling.Ignore).

#### `label.json` — draw_calls[] entry

```json
{
  "dc_id":  "<string>",
  "api_id": <uint>,
  "label": {
    "category":     "<string>",
    "subcategory":  "<string>",
    "detail":       "<string>",
    "reason_tags":  ["<string>", ...],
    "confidence":   <float 0–1>,
    "label_source": "<llm|rule|cache>"
  }
}
```

#### `metrics.json` — draw_calls[] entry

```json
{
  "dc_id":             "<string>",
  "api_id":            <uint>,
  "metrics_available": <bool>,
  "metrics": {
    "clocks":             <long>,
    "read_total_bytes":   <long>,
    "write_total_bytes":  <long>,
    "fragments_shaded":   <long>,
    "vertices_shaded":    <long>,
    "shaders_busy_pct":   <double>,
    "tex_l1_miss_pct":    <double>,
    "tex_l2_miss_pct":    <double>,
    "tex_fetch_stall_pct":<double>,
    "fragment_instructions": <long>,
    "vertex_instructions":   <long>,
    "tex_mem_read_bytes":    <long>,
    ...any other MetricsWhitelist counters present in DB...
  }
}
```

`metrics` is `null` / omitted when `metrics_available=false`.

#### `snapshot_{captureId}_index.json` — manifest

```json
{
  "schema_version": "1.0",
  "snapshot_id":    <uint>,
  "sdp_name":       "<string>",
  "generated_at":   "<ISO8601>",
  "products": {
    "dc":        "<absolute path to dc.json>",
    "shaders":   "<absolute path to shaders.json>",
    "textures":  "<absolute path to textures.json>",
    "buffers":   "<absolute path to buffers.json>",
    "label":     "<absolute path to label.json>",
    "metrics":   "<absolute path to metrics.json>",
    "status":    "<absolute path to status.json>",
    "topdc":     "<absolute path to topdc.json>",
    "analysis":  "<absolute path to analysis.md>",
    "dashboard": "<absolute path to dashboard.md>"
  }
}
```

### 7.2 Legacy `snapshot_{captureId}_raw.json` (schema 2.0)

Superseded by the sub-JSON files above. Still written by `GenerateLabeledMetricsJson`.
Root: `schema_version: "2.0"`, `draw_calls: [...]`.
Each entry contains the union of all fields from dc.json + shaders.json + textures.json
+ buffers.json + label.json + metrics.json. Can be detected by `schema_version == "2.0"`.

---

## 8. StatusJsonService — `status.json` Output Schema

Written to `captureOutDir/snapshot_{captureId}_status.json`.

```json
{
  "schema_version": "2.0",
  "snapshot_id":    <uint>,
  "sdp_name":       "<string>",
  "generated_at":   "<ISO8601>",
  "overall": {
    "total_dc_count":         <int>,
    "draw_dc_count":          <int>,
    "compute_dc_count":       <int>,
    "total_clocks":           <long>,
    "total_read_bytes":       <long>,
    "total_write_bytes":      <long>,
    "total_fragments_shaded": <long>,
    "total_vertices_shaded":  <long>,
    "metrics_coverage_ratio": <double 0–1>
  },
  "category_stats": [
    {
      "category":   "<string>",
      "dc_count":   <int>,
      "percentage": <double>,
      "clocks_sum": <long>,
      "clocks_pct": <double>,
      "metrics_p50": { "<snake_key>": <value>, ... },
      "metrics_p60": { ... },
      "metrics_p70": { ... },
      "metrics_p80": { ... },
      "metrics_p90": { ... },
      "metrics_p95": { ... },
      "metrics_p99": { ... }
    }
  ],
  "label_stats": {
    "avg_confidence":           <double>,
    "low_confidence_ratio":     <double>,
    "low_confidence_threshold": 0.6,
    "llm_labeled_count":        <int>,
    "rule_labeled_count":       <int>,
    "reason_tag_distribution":  { "<tag>": <count>, ... },
    "low_confidence_dc_ids":    ["<dc_id>", ...]
  },
  "global_percentiles": {
    "<snake_key>": {
      "p50": <double>, "p60": <double>, "p70": <double>,
      "p80": <double>, "p90": <double>, "p95": <double>, "p99": <double>
    },
    ...
  }
}
```

Percentile keys use the same snake_case mapping as `metrics.json` (see section 6).

---

## 9. Directory Layout

```
<sessionDir>/                        e.g. workspace/my_session/
  sdp.db
  DrawCallParameters.csv             (7 CSV files — imported to sdp.db)
  DrawCallBindings.csv
  DrawCallRenderTargets.csv
  DrawCallVertexBuffers.csv
  DrawCallIndexBuffers.csv
  PipelineVertexInputBindings.csv
  PipelineVertexInputAttributes.csv
  DrawCallMetrics.csv
  shaders/                           config key: Session.ShadersDir (default "shaders")
    pipeline_{pipelineID}_{stage}.spv
    pipeline_{pipelineID}_{stage}.hlsl
    pipeline_{pipelineID}_{stage}.glsl
    pipeline_{pipelineID}_{stage}.disasm
  textures/                          config key: Session.TexturesDir (default "textures")
    texture_{resourceID}.png
  meshes/                            config key: Session.MeshesDir (default "meshes")
    mesh_{apiID}.obj
    viewer.html
  snapshot_{captureId}/              per-capture output subfolder
    dc.json
    shaders.json
    textures.json
    buffers.json
    label.json
    metrics.json
    status.json
    topdc.json
    analysis.md
    dashboard.md
    snapshot_{captureId}_index.json
    snapshot_{captureId}_raw.json    (legacy schema 2.0)
    per_dc_content/                  (Pass B intermediate results)
```

---

## 10. Stability Classification

| Artifact | Stability | Notes |
|---|---|---|
| CSV files (7 VulkanSnapshotModel exports) | Stable | Format defined by SDPCLI VulkanSnapshotModel |
| `DrawCallMetrics.csv` | Fragile | Counter names from SDK/Adreno |
| Native SDP tables (`VulkanSnapshot*`) | Fragile | SDK-controlled schema |
| `DrawCallParameters` etc. (SQLite imported) | Stable | Mirror of CSV columns above |
| `DrawCallMetrics` (SQLite imported) | Fragile | Mirror of SDK counter names |
| All sub-JSON output files (schema 3.0) | Stable | Defined in RawJsonGenerationService |
| `status.json` (schema 2.0) | Stable | Defined in StatusJsonService |
| `snapshot_*_raw.json` (schema 2.0) | Stable but deprecated | Superseded by sub-JSONs |

---

## Related Context

- Related findings: FINDING-2026-04-08-db-access-fragmentation.md, FINDING-2026-04-08-table-validation.md
- Related plans: PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
- Related implementations: IMPL-2026-04-14-http-server-mode.md
