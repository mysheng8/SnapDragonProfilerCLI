# MODULE INDEX — SDP.Graphics.TextureConverter — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: Graphics, Texture Processing, GPU Formats, Image Conversion
Concepts: P/Invoke, Native Interop, Texture Formats, ASTC Compression, Format Conversion, Memory Marshalling
Common Logs: TextureConverter.dll, Console.Out.WriteLine
Entry Symbols: QonvertWrapper, TextureConverterHelper, ConvertImageToRGBA, TQonvertImage, TFormats

## Role
P/Invoke wrapper for native TextureConverter.dll providing GPU texture format conversion between 100+ formats, RGBA extraction with color channel swapping, ASTC compression header manipulation, and safe memory marshalling for texture data.

## Entry Points
| Symbol | Location |
|--------|----------|
| QonvertWrapper | [TextureConverter/QonvertWrapper.cs:9](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L9) |
| QonvertWrapper.Qonvert (managed wrapper) | [TextureConverter/QonvertWrapper.cs:14](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L14) |
| QonvertWrapper.Qonvert (P/Invoke) | [TextureConverter/QonvertWrapper.cs:30](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L30) |
| TextureConverterHelper | [TextureConverter/TextureConverterHelper.cs:7](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L7) |
| TextureConverterHelper.ConvertImageToRGBA | [TextureConverter/TextureConverterHelper.cs:10](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L10) |
| TQonvertImage | [TextureConverter/TQonvertImage.cs:8](../../../dll/project/SDPClientFramework/TextureConverter/TQonvertImage.cs#L8) |
| TFormats | [TextureConverter/TFormats.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TFormats.cs#L6) |

## Key Classes
| Class | Responsibility | Location |
|------|----------------|----------|
| QonvertWrapper | P/Invoke wrapper for native Qonvert function with exception handling | [QonvertWrapper.cs:9](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L9) |
| TextureConverterHelper | High-level helper methods for texture conversion and ASTC manipulation | [TextureConverterHelper.cs:7](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L7) |
| TQonvertImage | Image descriptor struct with width, height, format, data pointers | [TQonvertImage.cs:8](../../../dll/project/SDPClientFramework/TextureConverter/TQonvertImage.cs#L8) |
| TFormatFlags | Format configuration with stride, RGBA masks, encoding, scaling, normal maps | [TFormatFlags.cs:8](../../../dll/project/SDPClientFramework/TextureConverter/TFormatFlags.cs#L8) |
| TFormats enum | 100+ GPU texture format definitions (RGBA, DXT, ETC, ASTC, ATC, YUV, etc.) | [TFormats.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TFormats.cs#L6) |
| TReturnCode enum | Error codes for texture conversion operations | [TReturnCode.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TReturnCode.cs#L6) |
| TEncodeFlag enum | Encoding quality flags (default, ATITC fast) | [TEncodeFlag.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TEncodeFlag.cs#L6) |
| TScaleFilterFlag enum | Scale filter types (nearest, mean, bilinear, bicubic, kaiser) | [TScaleFilterFlag.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TScaleFilterFlag.cs#L6) |
| TNormalMapFlag enum | Normal map generation algorithms (Roberts cross, Sobel, Prewitt) | [TNormalMapFlag.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TNormalMapFlag.cs#L6) |
| TDebugFlags enum | Debug flags for conversion operations | [TDebugFlags.cs:6](../../../dll/project/SDPClientFramework/TextureConverter/TDebugFlags.cs#L6) |
| ASTC_Header struct | ASTC file header structure (16 bytes) | [TextureConverterHelper.cs:146](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L146) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| QonvertWrapper.Qonvert (managed) | Wrapper invoking native Qonvert with exception handling | [QonvertWrapper.cs:14](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L14) | High-level texture conversion |
| QonvertWrapper.Qonvert (native) | P/Invoke to TextureConverter.dll native function | [QonvertWrapper.cs:30](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L30) | Called by managed wrapper |
| TextureConverterHelper.ConvertImageToRGBA | Convert arbitrary format to RGBA with optional BGR swap | [TextureConverterHelper.cs:10](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L10) | Texture preview/display |
| TextureConverterHelper.GetASTCHeader | Generate ASTC file header from block size and dimensions | [TextureConverterHelper.cs:99](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L99) | ASTC file creation |
| TextureConverterHelper.AddAstcHeader | Prepend ASTC header to compressed data | [TextureConverterHelper.cs:122](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L122) | ASTC file export |

## Call Flow Skeleton
```
High-Level Texture Conversion:
Client code
 └── TextureConverterHelper.ConvertImageToRGBA(data, format, width, height, flipBR, rowStride)
      ├── Validate input data
      ├── Create TQonvertImage (source)
      │    ├── Set Width, Height, Format
      │    ├── Optional: Create TFormatFlags with custom stride
      │    │    ├── GCHandle.Alloc(tformatFlags, Pinned)
      │    │    └── Marshal.StructureToPtr + set PtrToFormatFlags
      │    ├── GCHandle.Alloc(data, Pinned)
      │    └── Set DataSize, PtrToData (pinned)
      ├── Create TFormatFlags (destination)
      │    ├── Stride = width * 4
      │    ├── MaskRed/Green/Blue/Alpha (with optional BGR flip)
      │    ├── GCHandle.Alloc(tformatFlags2, Pinned)
      │    └── Marshal.StructureToPtr
      ├── Create TQonvertImage (destination)
      │    ├── Set Width, Height
      │    ├── Format = 1 (Q_FORMAT_RGBA_8UI)
      │    ├── PtrToFormatFlags = pinned flags
      │    └── DataSize = 0, PtrToData = IntPtr.Zero (query mode)
      ├── First Qonvert call (query output size)
      │    └── QonvertWrapper.Qonvert(srcImg, dstImg)
      │         └── Native Qonvert(srcImg, dstImg, IntPtr.Zero)
      │              └── TextureConverter.dll processes
      │                   └── Returns success/error code
      │                        └── Sets dstImg.DataSize
      ├── Allocate output buffer
      │    ├── array = new byte[dstImg.DataSize]
      │    ├── GCHandle.Alloc(array, Pinned)
      │    └── dstImg.PtrToData = pinned array
      ├── Second Qonvert call (actual conversion)
      │    └── QonvertWrapper.Qonvert(srcImg, dstImg)
      │         └── Native writes to dstImg.PtrToData
      └── Finally: Free all GCHandles
           └── Return byte[] or null on error

ASTC Header Addition:
Client code
 └── TextureConverterHelper.AddAstcHeader(out buffer, inputBuffer, xBlocks, yBlocks, width, height)
      ├── GetASTCHeader(out header, xBlocks, yBlocks, width, height)
      │    ├── signature = 0x5CA1AB13 (ASTC magic)
      │    ├── Set x_block, y_block, z_block (1)
      │    ├── Encode width/height as 3 bytes little-endian
      │    │    ├── BitConverter.GetBytes(width) → x_size1/2/3
      │    │    └── BitConverter.GetBytes(height) → y_size1/2/3
      │    └── Override x_block/y_block with actual block dimensions
      ├── Calculate block counts and data size
      │    ├── numXBlocks = (width + xBlocks - 1) / xBlocks
      │    ├── numYBlocks = (height + yBlocks - 1) / yBlocks
      │    └── dataSize = numXBlocks * numYBlocks * 16
      ├── Allocate combined buffer (header + data)
      │    └── combinedBuffer = new byte[sizeof(header) + dataSize]
      ├── GCHandle.Alloc(combinedBuffer, Pinned)
      ├── Marshal.StructureToPtr(header, pinnedPtr, false)
      ├── Marshal.Copy(inputBuffer, 0, pinnedPtr + headerSize, dataSize)
      └── gchandle.Free()

Exception Handling:
QonvertWrapper.Qonvert (managed)
 └── try
      └── Call native Qonvert
 └── catch (Exception ex)
      ├── Console.Out.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace)
      └── Return 9 (Q_ERROR_OTHER)

TextureConverterHelper.ConvertImageToRGBA
 └── try
      └── Conversion logic
 └── catch (Exception ex)
      ├── Console.Out.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace)
      ├── If OutOfMemoryException or NullReferenceException:
      │    └── GC.Collect() and return null
      └── Else: throw
 └── finally
      └── Free all GCHandle objects (4 handles)
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| TQonvertImage instances | ConvertImageToRGBA | QonvertWrapper.Qonvert | Scope-based |
| TFormatFlags instances | ConvertImageToRGBA | Marshal.StructureToPtr, native | Scope-based |
| GCHandle (input data) | GCHandle.Alloc in ConvertImageToRGBA | Native via PtrToData | Finally block (Free) |
| GCHandle (output buffer) | GCHandle.Alloc in ConvertImageToRGBA | Native writes to PtrToData | Finally block (Free) |
| GCHandle (format flags) | GCHandle.Alloc in ConvertImageToRGBA | Native via PtrToFormatFlags | Finally block (Free) |
| byte[] output array | ConvertImageToRGBA | Caller (returned) | Caller manages |
| ASTC_Header struct | GetASTCHeader | AddAstcHeader (marshalling) | Scope-based |
| combinedBuffer byte[] | AddAstcHeader | Caller (returned via out param) | Caller manages |
| Native texture memory | TextureConverter.dll | Native processing | Native manages |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| Console.Out.WriteLine | [QonvertWrapper.cs:24](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L24) | Exception in managed Qonvert wrapper |
| Console.Out.WriteLine | [TextureConverterHelper.cs:67](../../../dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs#L67) | Exception during RGBA conversion |
| TextureConverter.dll | [QonvertWrapper.cs:30](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L30) | Native DLL name for P/Invoke |
| TextureConverter.dll | [QonvertWrapper.cs:34](../../../dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs#L34) | DLL location constant |

## Search Hints
```
Find P/Invoke entry:
search "class QonvertWrapper"
search "DllImport.*TextureConverter.dll"

Find texture conversion:
search "ConvertImageToRGBA"
search "TextureConverterHelper"

Find texture formats:
search "enum TFormats"
search "Q_FORMAT_"

Find ASTC handling:
search "AddAstcHeader|GetASTCHeader"
search "ASTC_Header"

Find memory marshalling:
search "GCHandle.Alloc|Marshal.StructureToPtr"
search "PtrToData|PtrToFormatFlags"

Find format configuration:
search "class TFormatFlags"
search "MaskRed|MaskGreen|MaskBlue|MaskAlpha"

Find error handling:
search "TReturnCode|Q_ERROR_|Q_SUCCESS"
search "Console.Out.WriteLine"

Jump to key components:
open dll/project/SDPClientFramework/TextureConverter/QonvertWrapper.cs:9
open dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs:7
open dll/project/SDPClientFramework/TextureConverter/TFormats.cs:6
open dll/project/SDPClientFramework/TextureConverter/TQonvertImage.cs:8
```
