# SDPCLI Command Line Parameters

## Overview
SDPCLI supports command line parameters for automated batch processing, in addition to the original interactive mode.

## Usage Modes

### 1. Interactive Mode (Default)
Run without parameters to enter interactive mode with menu selection:
```batch
SDPCLI.exe
```

### 2. Direct Analysis Mode
Analyze a specific .sdp file directly:
```batch
SDPCLI.exe -mode analysis -sdp <path_to_sdp_file>
```

### 3. Direct Capture Mode  
Capture a single Vulkan snapshot from a connected device:
```batch
SDPCLI.exe -mode capture
```

### 4. Texture Extraction Mode
Extract a texture from an .sdp file and save as PNG:
```batch
SDPCLI.exe -mode extract-texture -sdp <path_to_sdp_file> -resource-id <resourceID>
```

### 5. Shader Extraction Mode
Extract shader source (GLSL/SPIR-V) from an .sdp file:
```batch
SDPCLI.exe -mode extract-shader -sdp <path_to_sdp_file> -drawcall-id <drawcall>
```

## Parameters

### `-mode <mode_name>`
Specifies the operation mode. Valid values:
- `analysis` or `analyze` or `2` - Analysis mode
- `capture` or `snapshot` or `1` - Capture mode
- `extract-texture` or `texture` or `3` - Texture extraction mode
- `extract-shader` or `shader` or `4` - Shader extraction mode

### `-sdp <file_path>`
Specifies the .sdp file to work with (analysis, texture/shader extraction modes)
- **Relative paths**: Resolved relative to the test directory (configured in config.ini as `TestDirectory`)
- **Absolute paths**: Used as-is
- Supports both file paths and directory paths (containing extracted sdp.db)

**Examples:**
- `2026-03-20T20-36-12.sdp` → Looks in `<TestDirectory>/2026-03-20T20-36-12.sdp`
- `D:\captures\my_capture.sdp` → Uses absolute path directly

### `-resource-id <id>`
Resource ID of the texture to extract (texture extraction mode only).  
Use Analysis mode or SQL queries to discover resource IDs:
```sql
SELECT resourceID, width, height, format FROM VulkanSnapshotTextures WHERE captureID=3 ORDER BY width*height DESC LIMIT 20;
```

### `-output <path>`
Output file path (for texture extraction) or directory (for shader extraction).  
Defaults to the project `test/` directory if not specified.

### `-capture-id <id>`
Capture ID within the .sdp session (default: `3`). Rarely needs to be changed.

### `-drawcall-id <id>`
DrawCall identifier for shader extraction. Supports two formats:
- Dot notation: `"1.1.5"` (frame.submission.drawcall)
- Simple integer: `"5"` (draw index)

### `-pipeline-id <id>`
Pipeline resource ID for shader extraction (alternative to `-drawcall-id`).

## Examples

### Example 1: Analyze file in test directory (relative path)
```batch
SDPCLI.exe -mode analysis -sdp "2026-03-20T20-36-12.sdp"
```

### Example 2: Analyze with absolute path
```batch
SDPCLI.exe -mode analysis -sdp "D:\captures\my_capture.sdp"
```

### Example 3: Extract texture
```batch
SDPCLI.exe -mode extract-texture -sdp "test\capture.sdp" -resource-id 23352
SDPCLI.exe -mode extract-texture -sdp "test\capture.sdp" -resource-id 23352 -output "out\tex.png" -capture-id 3
```

### Example 4: Extract shaders for a DrawCall
```batch
SDPCLI.exe -mode extract-shader -sdp "test\capture.sdp" -drawcall-id "1.1.5" -output "shaders\"
```

### Example 5: Batch analysis
```batch
@echo off
for %%f in (captures\*.sdp) do (
    echo Analyzing %%f...
    SDPCLI.exe -mode analysis -sdp "%%f"
)
```

## Notes

1. **Build**: `dotnet build SDPCLI` → outputs to `bin\Debug\net472\SDPCLI.exe`
2. **Output Files**: Reports generated in configured test directory (default: `SDPCLI\test\`)
3. **Log Files**: Console output is saved to `bin\Debug\net472\.log\consolelog.txt`
4. **Error Handling**: Exit code indicates success (0) or failure (non-zero)
5. **Backward Compatibility**: Running without parameters maintains the original interactive experience
