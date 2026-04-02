# Texture Query Limitation in Snapshot Mode

## Problem Summary

SDPCLI cannot retrieve accurate per-DrawCall texture bindings when analyzing Vulkan Snapshot captures.

## Root Cause

Based on analysis of QGLPlugin source code (documented in `docs/index/modules/Profiler.QGLPlugin.ResourceBinding.md`):

### Texture Bindings Are NOT in SQLite Database

1. **Binary Buffer Storage**: Descriptor set bindings (textures, buffers, samplers) are stored in `SnapshotDsbBuffer`, a binary buffer embedded in the .sdp file.

2. **Database Limitation**: The SQLite table `VulkanSnapshotDescriptorSetBindings` exists, but:
   - `apiID` column = `4294967295` (UINT_MAX) for ALL records
   - No per-DrawCall binding information
   - Only contains general binding structure, not runtime state

3. **GUI Solution**: Snapdragon Profiler GUI uses:
   ```csharp
   VkSnapshotModel.PopulateDescSets(captureID)
   {
       // Unmarshals binary buffer using Marshal.PtrToStructure
       DescSetBindings.DescBindings descBindings = Marshal.PtrToStructure<...>(SnapshotDsbBuffer.data);
       
       // Stores in memory: AllDescSetBindings[descriptorSetID]
       // Indexed by: m_drawCallInfos[captureID][apiID]
   }
   
   VkSnapshotModel.GetBoundInfo(captureID, drawCallID)
   {
       // Returns VkBoundInfo with BoundDescriptorSets
       // Each descriptor set contains imageViewID → texture mappings
   }
   ```

## Current Fallback Implementation

SDPCLI returns **all textures** in the capture (up to 50) for every DrawCall:

```csharp
SELECT DISTINCT iv.imageID
FROM VulkanSnapshotDescriptorSetBindings dsb
JOIN VulkanSnapshotImageViews iv ON dsb.imageViewID = iv.resourceID
WHERE dsb.captureID = ? AND dsb.imageViewID > 0
LIMIT 50
```

**Result**: Every DrawCall shows the same texture list (inherent limitation).

## Solution Options

### Option 1: Parse Binary Buffer (Complex)
**Pros**: Accurate per-DrawCall bindings  
**Cons**: Requires:
- Extract .sdp archive
- Locate SnapshotDsbBuffer in archive
- Implement binary buffer parsing (native struct unmarshaling)
- Complex reverse engineering

### Option 2: Use SDPCore.dll via P/Invoke (Moderate)
**Pros**: Reuse existing QGLPlugin logic  
**Cons**: Requires:
- Load SDPCore.dll and dependencies
- Call VkSnapshotModel.PopulateDescSets()
- Tight coupling to GUI implementation

### Option 3: Accept Limitation (Current)
**Pros**: Simple, works for general texture enumeration  
**Cons**: Cannot identify which textures each DrawCall actually uses

## Recommendation

**For current CLI use case** (general DrawCall analysis):
- Keep current fallback implementation
- Clearly document limitation in reports
- Consider this acceptable for:
  - Getting a list of all textures in a capture
  - Identifying pipeline/shader usage
  - General performance analysis

**For future enhancement** (if per-DrawCall accuracy needed):
- Implement Option 1 (binary buffer parsing)
- Create a separate tool for .sdp binary buffer extraction
- Document the SnapshotDsbBuffer format

## Verification

You can verify this limitation by:

1. Run Analysis mode on any .sdp file
2. Check generated report - all DrawCalls show identical texture lists
3. Compare with Snapdragon Profiler GUI
   - GUI shows different textures per DrawCall
   - GUI has access to binary buffer via VkSnapshotModel

## Related Documentation

- Technical details: `docs/index/modules/Profiler.QGLPlugin.ResourceBinding.md`
- QGLPlugin source: `dll/project/QGLPlugin/VkSnapshotModel.cs:147` (PopulateDescSets)
- Binary structure: `dll/project/QGLPlugin/DescSetBindings.cs` (DescBindings struct)
