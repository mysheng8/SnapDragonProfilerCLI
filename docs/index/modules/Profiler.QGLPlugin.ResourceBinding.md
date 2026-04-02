# MODULE INDEX — Profiler.QGLPlugin.ResourceBinding — AUTHORITATIVE ROUTING

## Routing Keywords
**Systems:** Profiler, QGLPlugin, VulkanSnapshot  
**Concepts:** descriptor set binding, texture binding, resource binding, drawcall resources, imageView, pipeline bindings  
**Common Logs:** descriptorSet, imageView, texture binding, bound resources  
**Entry Symbols:** PopulateDescSets, GetBoundInfo, UpdateDescSet, BoundDescriptorSets

## Role
Manages runtime binding of Vulkan descriptor sets (textures, buffers, samplers) to DrawCalls via binary buffer loading and in-memory tracking.

## Entry Points
| Symbol | Location |
|--------|----------|
| VkSnapshotModel.PopulateDescSets() | [dll/project/QGLPlugin/VkSnapshotModel.cs:147](dll/project/QGLPlugin/VkSnapshotModel.cs#L147) |
| VkSnapshotModel.GetBoundInfo() | [dll/project/QGLPlugin/VkSnapshotModel.cs:28](dll/project/QGLPlugin/VkSnapshotModel.cs#L28) |
| VkSnapshotModel.AddDrawCallInfo() | [dll/project/QGLPlugin/VkSnapshotModel.cs:99](dll/project/QGLPlugin/VkSnapshotModel.cs#L99) |
| VkAPITreeModelBuilder.UpdateDescSet() | [dll/project/QGLPlugin/VkAPITreeModelBuilder.cs:279](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L279) |

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| VkBoundInfo | Holds bound pipeline + descriptor sets for single DrawCall | [dll/project/QGLPlugin/VkBoundInfo.cs:7](dll/project/QGLPlugin/VkBoundInfo.cs#L7) |
| DescSetBindings | Container for descriptor set bindings (by slotNum) | [dll/project/QGLPlugin/DescSetBindings.cs:7](dll/project/QGLPlugin/DescSetBindings.cs#L7) |
| DescSetBindings.DescBindings | Individual binding with imageViewID, bufferID, samplerID | [dll/project/QGLPlugin/DescSetBindings.cs:36](dll/project/QGLPlugin/DescSetBindings.cs#L36) |
| VkSnapshotModel | Root model managing m_drawCallInfos mapping | [dll/project/QGLPlugin/VkSnapshotModel.cs:12](dll/project/QGLPlugin/VkSnapshotModel.cs#L12) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| PopulateDescSets(captureID) | Load binary buffer into AllDescSetBindings | [VkSnapshotModel.cs:147](dll/project/QGLPlugin/VkSnapshotModel.cs#L147) | Snapshot loaded |
| GetBoundInfo(captureID, drawCallID) | Retrieve bound resources for DrawCall | [VkSnapshotModel.cs:28](dll/project/QGLPlugin/VkSnapshotModel.cs#L28) | User selects DrawCall |
| AddDrawCallInfo(captureID, apiID, info) | Store DrawCall binding state | [VkSnapshotModel.cs:99](dll/project/QGLPlugin/VkSnapshotModel.cs#L99) | DrawCall processed |
| UpdateDescSet(DescBindings d) | Add binding to m_descBindingInfo | [VkAPITreeModelBuilder.cs:279](dll/project/QGLPlugin/VkAPITreeModelBuilder.cs#L279) | Building tree model |
| UpdateUsedResources() | Extract imageViews from BoundDescriptorSets | [ResourcesViewMgr.cs:3308](dll/project/QGLPlugin/ResourcesViewMgr.cs#L3308) | Filtering used resources |
| GetPipelineDescriptorSets(captureID, pipelineID) | Enumerate descriptor sets used by pipeline | [VkSnapshotModel.cs:106](dll/project/QGLPlugin/VkSnapshotModel.cs#L106) | Analyzing pipeline usage |

## Call Flow Skeleton
```
Snapshot Load
 ├── PopulateDescSets(captureID)
 │    ├── Load SnapshotDsbBuffer (binary buffer)
 │    ├── Unmarshal DescSetBindings.DescBindings structs
 │    ├── Store in capture.AllDescSetBindings[descriptorSetID]
 │    └── Index by apiID in DsbByApi
 │
 ├── VkAPITreeModelBuilder.ProcessAllCalls()
 │    ├── For each DrawCall API
 │    ├── UpdateDescSet(apiID) → iterate DsbByApi
 │    └── Store in m_descBindingInfo
 │
 └── AddDrawCallInfo(captureID, apiID, VkBoundInfo)
      ├── VkBoundInfo.BoundDescriptorSets[descSetID] = DescSetBindings
      └── m_drawCallInfos[captureID][apiID] = VkBoundInfo

DrawCall Selection
 ├── GetBoundInfo(captureID, drawCallID)
 │    └── Return m_drawCallInfos[captureID][drawCallID]
 │
 └── UpdateUsedResources()
      ├── foreach BoundDescriptorSets.Values
      ├── foreach Bindings.Values → extract imageViewID
      └── Populate m_usedImageViews, m_usedImages
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| m_drawCallInfos | AddDrawCallInfo() | GetBoundInfo() | Capture close |
| AllDescSetBindings | PopulateDescSets() | UpdateDescSet() | VkCapture destructor |
| BoundDescriptorSets | VkBoundInfo constructor | UpdateUsedResources() | DrawCall clear |
| SnapshotDsbBuffer | Native plugin | PopulateDescSets() | Native cleanup |
| m_descBindingInfo | UpdateDescSet() | GetDescSet() | Tree rebuild |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| descriptorSet | DescSetBindings.DescSetID | Vulkan descriptor set handle |
| imageViewID | DescBindings.imageViewID | Image view bound to slot |
| samplerID | DescBindings.samplerID | Sampler bound to slot |
| bufferID | DescBindings.bufferID | Buffer bound to slot |
| slotNum | DescBindings.slotNum | Binding slot number |
| apiID | DescBindings.apiID | API call when binding occurred |
| BoundPipeline | VkBoundInfo.BoundPipeline | Pipeline bound at DrawCall |
| texture binding | imageViewID → imageID lookup | Texture resource ID |

## Critical Architecture Notes

### Binary Buffer Source
- **SnapshotDsbBuffer**: Native binary buffer containing all descriptor bindings
- **Not in SQLite database**: VulkanSnapshotDescriptorSetBindings has `apiID = UINT_MAX` in Snapshot mode
- **Must unmarshal**: Use `Marshal.PtrToStructure` to extract DescSetBindings.DescBindings

### Binding Resolution Chain
```
DrawCall (apiID)
 → VkBoundInfo
   → BoundDescriptorSets[descriptorSetID]
     → DescSetBindings
       → Bindings[slotNum]
         → DescBindings.imageViewID
           → VulkanSnapshotImageViews.imageID
             → Texture resource
```

### Snapshot Mode Limitation
- **Runtime bindings not stored per-DrawCall** in database
- Must load binary buffer to get actual bindings
- CLI tools cannot access bindings without binary buffer parsing

## Search Hints
```
Find entry:
search "PopulateDescSets"
search "GetBoundInfo"

Find texture extraction:
search "imageViewID"
search "BoundDescriptorSets.Values"

Jump to binding storage:
open dll/project/QGLPlugin/VkSnapshotModel.cs:147
open dll/project/QGLPlugin/VkBoundInfo.cs:7

Find usage tracking:
search "UpdateUsedResources"
search "m_usedImageViews"
```

## CLI Integration Guide

To query textures for DrawCall in CLI without binary buffer access:

**Problem**: Database table `VulkanSnapshotDescriptorSetBindings` only has `apiID = 4294967295` (no per-DrawCall records)

**Solution Options**:
1. **Parse binary buffer** from .sdp file (complex, requires native interop)
2. **Load via SDPCore.dll** using VkSnapshotModel (coupling to GUI code)
3. **Accept limitation**: Return all textures in capture (current fallback)

**Recommended Query** (fallback):
```sql
-- Get all textures in capture (no per-DrawCall filtering possible)
SELECT DISTINCT iv.imageID
FROM VulkanSnapshotDescriptorSetBindings dsb
JOIN VulkanSnapshotImageViews iv 
  ON dsb.imageViewID = iv.resourceID
WHERE dsb.captureID = ? AND dsb.imageViewID > 0
LIMIT 50
```

**Future Enhancement**: Extract and parse SnapshotDsbBuffer from .sdp archives
