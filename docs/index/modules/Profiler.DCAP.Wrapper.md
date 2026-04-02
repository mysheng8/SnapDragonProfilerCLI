# MODULE INDEX — Profiler.DCAP.Wrapper — AUTHORITATIVE ROUTING

## Routing Keywords
**Systems**: OpenGL ES capture, EGL tracing, API trace file format, graphics profiling  
**Concepts**: SWIG P/Invoke wrapper, compression, frame indexing, API replay, trace stripping  
**Common Logs**: `libDCAP.so`, `pluginGPU-OpenGLES`, `OpenGLESDataPlugin`, DCAP file operations  
**Entry Symbols**: `CaptureFileReader.Initialize`, `DCAPFileProcessor`, `libDCAP.GenerateFrameIndex`, `GLAdapter`, `GLDecoder`

---

## Role
C# SWIG-generated P/Invoke wrapper for native libDCAP library, providing OpenGL ES 2.0/3.0+ and EGL API trace file (.dcap) reading, writing, compression, stripping, and frame indexing capabilities.

---

## Entry Points
| Symbol | Location |
|--------|----------|
| CaptureFileReader (constructor) | [dll/project/DCAPToolsWrapper/CaptureFileReader.cs:50](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L50) |
| CaptureFileReader.Initialize | [dll/project/DCAPToolsWrapper/CaptureFileReader.cs:139](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L139) |
| DCAPFileProcessor (constructor) | [dll/project/DCAPToolsWrapper/DCAPFileProcessor.cs:53](dll/project/DCAPToolsWrapper/DCAPFileProcessor.cs#L53) |
| libDCAP.GetFileSize | [dll/project/DCAPToolsWrapper/libDCAP.cs:8](dll/project/DCAPToolsWrapper/libDCAP.cs#L8) |
| libDCAP.IsFileFinalized | [dll/project/DCAPToolsWrapper/libDCAP.cs:17](dll/project/DCAPToolsWrapper/libDCAP.cs#L17) |
| libDCAP.GenerateFrameIndex | [dll/project/DCAPToolsWrapper/libDCAP.cs:63](dll/project/DCAPToolsWrapper/libDCAP.cs#L63) |
| libDCAP.FinalizeFile | [dll/project/DCAPToolsWrapper/libDCAP.cs:80](dll/project/DCAPToolsWrapper/libDCAP.cs#L80) |
| GLDecoder (constructor) | [dll/project/DCAPToolsWrapper/GLDecoder.cs:57](dll/project/DCAPToolsWrapper/GLDecoder.cs#L57) |
| EGLDecoder (constructor) | [dll/project/DCAPToolsWrapper/EGLDecoder.cs](dll/project/DCAPToolsWrapper/EGLDecoder.cs) |
| GLAdapter (constructor) | [dll/project/DCAPToolsWrapper/GLAdapter.cs:56](dll/project/DCAPToolsWrapper/GLAdapter.cs#L56) |

---

## Key Classes
| Class | Responsibility | Location |
|-------|----------------|----------|
| CaptureFileReader | Main capture file reader with decoder/handler registration, frame navigation, threading support | [dll/project/DCAPToolsWrapper/CaptureFileReader.cs:5](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L5) |
| DCAPFileProcessor | Simplified file processor for reading capture metadata, compression info, frame counts | [dll/project/DCAPToolsWrapper/DCAPFileProcessor.cs:5](dll/project/DCAPToolsWrapper/DCAPFileProcessor.cs#L5) |
| libDCAP | Static utility class for file operations: finalization, indexing, validation | [dll/project/DCAPToolsWrapper/libDCAP.cs:4](dll/project/DCAPToolsWrapper/libDCAP.cs#L4) |
| GLAdapter | OpenGL ES adapter with 200+ Process_gl* methods for API call handling (15,997 lines) | [dll/project/DCAPToolsWrapper/GLAdapter.cs:6](dll/project/DCAPToolsWrapper/GLAdapter.cs#L6) |
| EGLAdapter | EGL adapter with Process_egl* methods for EGL API calls (2,147 lines) | [dll/project/DCAPToolsWrapper/EGLAdapter.cs:6](dll/project/DCAPToolsWrapper/EGLAdapter.cs#L6) |
| GLDecoder | OpenGL ES decoder bridging file reader to GL adapters | [dll/project/DCAPToolsWrapper/GLDecoder.cs:5](dll/project/DCAPToolsWrapper/GLDecoder.cs#L5) |
| EGLDecoder | EGL decoder bridging file reader to EGL adapters | [dll/project/DCAPToolsWrapper/EGLDecoder.cs:5](dll/project/DCAPToolsWrapper/EGLDecoder.cs#L5) |
| DCAPConsumer | Base consumer processing file/block/API call events (virtual methods) | [dll/project/DCAPToolsWrapper/DCAPConsumer.cs:5](dll/project/DCAPToolsWrapper/DCAPConsumer.cs#L5) |
| DCAPCompressConsumer | Consumer with compression/decompression support (LZ4, ZSTD) | [dll/project/DCAPToolsWrapper/DCAPCompressConsumer.cs:5](dll/project/DCAPToolsWrapper/DCAPCompressConsumer.cs#L5) |
| DCAPStripConsumer | Consumer for stripping texture data to create smaller trace files | [dll/project/DCAPToolsWrapper/DCAPStripConsumer.cs:5](dll/project/DCAPToolsWrapper/DCAPStripConsumer.cs#L5) |
| DataReader | Abstract I/O reader (stdio, memory) for file reading | [dll/project/DCAPToolsWrapper/DataReader.cs:5](dll/project/DCAPToolsWrapper/DataReader.cs#L5) |
| DataWriter | Abstract I/O writer for file writing | [dll/project/DCAPToolsWrapper/DataWriter.cs:5](dll/project/DCAPToolsWrapper/DataWriter.cs#L5) |
| Compressor | Compression engine supporting LZ4 and ZSTD algorithms | [dll/project/DCAPToolsWrapper/Compressor.cs:5](dll/project/DCAPToolsWrapper/Compressor.cs#L5) |
| Decoder | Base decoder for API call processing | [dll/project/DCAPToolsWrapper/Decoder.cs:5](dll/project/DCAPToolsWrapper/Decoder.cs#L5) |
| Adapter | Base adapter for handling current thread context | [dll/project/DCAPToolsWrapper/Adapter.cs:5](dll/project/DCAPToolsWrapper/Adapter.cs#L5) |

---

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| Initialize(DataReader, fileSize) | Initialize capture file reader with I/O source | [CaptureFileReader.cs:139](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L139) | Opening .dcap file |
| AddDecoder(Decoder) | Register decoder (GL/EGL) for API call processing | [CaptureFileReader.cs:70](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L70) | Setting up reader |
| AddBlockEventHandler | Register block event handler | [CaptureFileReader.cs:60](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L60) | Custom processing needed |
| ProcessAllBlocks() | Process entire capture file sequentially | [CaptureFileReader.cs](dll/project/DCAPToolsWrapper/CaptureFileReader.cs) | Replay/analysis |
| ProcessFrameBlocks(frameID) | Process specific frame's blocks | [CaptureFileReader.cs](dll/project/DCAPToolsWrapper/CaptureFileReader.cs) | Frame seek/replay |
| GenerateFrameIndex(fileName, index) | Create frame index for seeking | [libDCAP.cs:63](dll/project/DCAPToolsWrapper/libDCAP.cs#L63) | Opening capture file |
| FinalizeFile(fileName, frameCount) | Write file trailer with metadata | [libDCAP.cs:80](dll/project/DCAPToolsWrapper/libDCAP.cs#L80) | Capture completion |
| IsFileFinalized(fileName) | Check if file has valid trailer | [libDCAP.cs:17](dll/project/DCAPToolsWrapper/libDCAP.cs#L17) | File validation |
| GetFileSize(fileName) | Query capture file size | [libDCAP.cs:8](dll/project/DCAPToolsWrapper/libDCAP.cs#L8) | File operations |
| EnsureDCAPFinalized(fileName) | Ensure file is finalized, finalize if needed | [libDCAP.cs:105](dll/project/DCAPToolsWrapper/libDCAP.cs#L105) | Opening incomplete captures |
| GetCurrentFrame() | Get current frame number during processing | [CaptureFileReader.cs:171](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L171) | Frame tracking |
| GetNumFrames() | Get total frame count | [CaptureFileReader.cs:176](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L176) | UI display |
| SetLoopFrame(frameID) | Set frame to loop from | [CaptureFileReader.cs:213](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L213) | Replay looping |
| SetFilterThreads(threadIDs) | Filter processing to specific threads | [CaptureFileReader.cs:226](dll/project/DCAPToolsWrapper/CaptureFileReader.cs#L226) | Multi-threaded analysis |
| InitializeCompression(algorithm, quality) | Setup compression (LZ4/ZSTD) | [DCAPCompressConsumer.cs:157](dll/project/DCAPToolsWrapper/DCAPCompressConsumer.cs#L157) | Writing compressed captures |
| InitializeDecompression() | Setup decompression | [DCAPCompressConsumer.cs:152](dll/project/DCAPToolsWrapper/DCAPCompressConsumer.cs#L152) | Reading compressed files |
| ProcessFunctionCall(callId, buffer, size) | Decode API function call | [Decoder.cs:69](dll/project/DCAPToolsWrapper/Decoder.cs#L69) | GL/EGL function |
| ProcessMethodCall(callId, objectId, buffer, size) | Decode API method call | [Decoder.cs:74](dll/project/DCAPToolsWrapper/Decoder.cs#L74) | Object method |
| Process_glDrawArrays(...) | Handle glDrawArrays API call | [GLAdapter.cs](dll/project/DCAPToolsWrapper/GLAdapter.cs) | DrawCall encountered |
| Process_eglSwapBuffers(...) | Handle eglSwapBuffers API call | [EGLAdapter.cs](dll/project/DCAPToolsWrapper/EGLAdapter.cs) | Frame delimiter |

---

## Call Flow Skeleton
```
Opening DCAP File
 ├── libDCAP.IsFileFinalized(fileName)
 ├── libDCAP.GetFileSize(fileName)
 ├── libDCAP.GenerateFrameIndex(fileName, frameIndex)
 ├── new CaptureFileReader()
 ├── new DataReader() → StdioReader or custom
 ├── CaptureFileReader.Initialize(dataReader, fileSize)
 ├── CaptureFileReader.AddDecoder(new GLDecoder())
 │    └── GLDecoder.AddAdapter(new GLAdapter())
 ├── CaptureFileReader.AddDecoder(new EGLDecoder())
 │    └── EGLDecoder.AddAdapter(new EGLAdapter())
 └── CaptureFileReader.AddMetaHandler(metaHandler)
 
Processing Capture
 ├── CaptureFileReader.ProcessAllBlocks()
 │    ├── For each block:
 │    │    ├── Read block header
 │    │    ├── Identify block type (MethodCall, FunctionCall, Metadata)
 │    │    ├── Parse API call ID
 │    │    ├── Decoder.SupportsId(callId) → check if GLDecoder/EGLDecoder handles it
 │    │    ├── Decoder.ProcessFunctionCall(callId, paramBuffer, size)
 │    │    │    └── GLDecoder/EGLDecoder → parse parameters
 │    │    │         └── Adapter.Process_gl*/Process_egl*(params)
 │    │    │              └── User implementation (e.g., state tracking)
 │    │    └── Trigger BlockEventHandler callbacks
 │    └── Return when all blocks processed
 │
 └── CaptureFileReader.ProcessFrameBlocks(frameID)
      └── Seek to frame, process blocks for that frame only
 
Writing Compressed Capture
 ├── new DCAPCompressConsumer(dataWriter)
 ├── DCAPCompressConsumer.InitializeCompression(ZSTD, High)
 ├── DCAPCompressConsumer.ProcessFileStart(header)
 ├── DCAPCompressConsumer.ProcessBlockStart(blockHeader)
 ├── DCAPCompressConsumer.ProcessMethodCall(header, desc, params, size)
 │    └── Compresses and writes to DataWriter
 ├── DCAPCompressConsumer.ProcessBlockEnd(trailer)
 ├── DCAPCompressConsumer.ProcessFileEnd()
 └── libDCAP.FinalizeFile(fileName, frameCount, lastFramePos)
 
Stripping Texture Data
 ├── new DCAPStripConsumer(dataWriter)
 ├── Read source with CaptureFileReader
 ├── DCAPStripConsumer.ProcessFileStart(header)
 ├── For each API call:
 │    └── DCAPStripConsumer.ProcessMethodCall(...)
 │         └── Strips texture data, writes call without textures
 └── Output: smaller .dcap file
```

---

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| CaptureFileReader | User code | API replay, analysis tools | Dispose() |
| DataReader (StdioReader) | libDCAP native | CaptureFileReader.Initialize | DataReader.Dispose() |
| Frame Index | libDCAP.GenerateFrameIndex() | CaptureFileReader seeking | Session end |
| Decoder instances | User code | CaptureFileReader.ProcessBlocks | CaptureFileReader.RemoveDecoder |
| Adapter instances | User code | Decoder.ProcessFunctionCall | Decoder.RemoveAdapter |
| Compressed data buffers | DCAPCompressConsumer | Compression engine | Frame boundary |
| API call parameter buffers | Native libDCAP | Decoder.ProcessFunctionCall | Block end |
| File header/trailer | libDCAP.FinalizeFile | File validation | File close |
| BlockEventHandler | User code | CaptureFileReader events | RemoveBlockEventHandler |

---

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `libDCAP.so` | Service logs | Native library pushed to Android device |
| `pluginGPU-OpenGLES` | Service logs | OpenGL ES plugin using libDCAP |
| `OpenGLESDataPlugin` | Service logs | Data provider using DCAP wrapper |
| `File 'libDCAP.so' already exists` | ADB service manager | Library already deployed |
| DCAP file operations | libDCAP methods | File I/O through wrapper |
| Compression errors | DCAPCompressConsumer | Compression/decompression failures |
| Frame index generation | libDCAP.GenerateFrameIndex | Building seek table |

---

## File Format Details
```
DCAP File Structure:
├── File Header
│   ├── Magic number
│   ├── Version (major.minor)
│   ├── Flags (compressed, stripped, has thread ID, has timestamp)
│   └── Metadata
│
├── Frame Blocks (repeated)
│   ├── Frame Number (delimiter)
│   ├── Block Header
│   │   ├── Block type (MethodCall, FunctionCall, Metadata)
│   │   ├── Block size (32-bit or 64-bit)
│   │   └── Thread ID (optional)
│   │
│   ├── SubBlocks (API calls)
│   │   ├── SubBlock Header
│   │   │   ├── API call ID (ApiCallType enum)
│   │   │   ├── Object ID (for method calls)
│   │   │   ├── Timestamp (optional)
│   │   │   └── Parameter buffer size
│   │   ├── Parameter Buffer
│   │   │   ├── Marshaled function arguments
│   │   │   └── Pointer data (arrays, strings)
│   │   └── Return value
│   │
│   └── Block Trailer (CRC or checksum)
│
└── File Trailer (optional)
    ├── Frame count
    ├── Last frame position
    └── Frame index offset
```

---

## Component Architecture
```
DCAPToolsWrapper.dll (56 .cs files, ~30,000 lines)
├── libDCAPPINVOKE.cs (3,802 lines)
│   └── DllImport declarations for libDCAP.dll/.so
│
├── Core Classes
│   ├── libDCAP.cs (406 lines) - Static utilities
│   ├── CaptureFileReader.cs (398 lines) - Main reader
│   └── DCAPFileProcessor.cs (255 lines) - Simplified processor
│
├── Decoders
│   ├── Decoder.cs - Base decoder
│   ├── GLDecoder.cs - OpenGL ES decoder
│   └── EGLDecoder.cs - EGL decoder
│
├── Adapters (Process API calls)
│   ├── Adapter.cs - Base adapter
│   ├── GLAdapter.cs (15,997 lines) - 200+ Process_gl* methods
│   └── EGLAdapter.cs (2,147 lines) - Process_egl* methods
│
├── Consumers (Write/transform)
│   ├── DCAPConsumer.cs (122 lines) - Base consumer
│   ├── DCAPCompressConsumer.cs (200 lines) - Compression
│   └── DCAPStripConsumer.cs (102 lines) - Texture stripping
│
├── I/O Abstraction
│   ├── DataReader.cs - Abstract reader
│   ├── DataWriter.cs - Abstract writer
│   ├── StdioReader.cs - File reader implementation
│   └── StdioWriter.cs - File writer implementation
│
├── Compression
│   ├── Compressor.cs - Compression engine
│   ├── CompressionAlgorithm.cs - Enum (None, LZ4, ZSTD)
│   └── Compressor.CompressorQuality - Enum (Low, Normal, High)
│
├── Data Structures
│   ├── FrameEntry.cs - Frame index entry (number, position)
│   ├── PointerData.cs - Parameter pointer handling
│   ├── ApiCallType.cs - Enum of API call IDs
│   ├── BlockType.cs - Enum (MethodCall, FunctionCall, Metadata)
│   ├── FrameDelim.cs - Frame delimiter
│   ├── UniformData.cs - Uniform variable data
│   ├── AttributeData.cs - Vertex attribute data
│   ├── EGLImageData.cs - EGL image data
│   └── ... (30+ data structure classes)
│
├── Metadata Handling
│   ├── MetaHandler.cs - Metadata handler base
│   ├── MetadataBlockType.cs - Metadata types
│   ├── MetadataCommand.cs - Metadata commands
│   ├── MetadataStateDesc.cs - State descriptors
│   └── MetadataText.cs - Text metadata
│
├── Enums (Generated from OpenGL ES spec)
│   ├── FrameSubBlockType.cs
│   ├── QCTPixelFormat.cs
│   └── OmitData.cs
│
└── SWIG Wrapper Types (Internal)
    ├── SWIGTYPE_p_Data__BlockEventHandler.cs
    ├── SWIGTYPE_p_Data__FileHeader.cs
    ├── SWIGTYPE_p_Data__BlockHeader.cs
    ├── SWIGTYPE_p_Data__SubBlockHeader.cs
    ├── SWIGTYPE_p_Data__FunctionCallDesc.cs
    ├── SWIGTYPE_p_Data__MethodCallDesc.cs
    ├── SWIGTYPE_p_Data__MetadataDesc.cs
    └── ... (10+ SWIGTYPE wrappers)
```

---

## Dependencies
```
Native Libraries:
  libDCAP.dll (Windows) - Core capture format library
  libDCAP.so (Android arm64-v8a, armeabi-v7a) - Device-side library

.NET:
  System.Runtime.InteropServices - P/Invoke marshaling
  System.IDisposable - Resource management

Compression:
  LZ4 (native) - Fast compression
  ZSTD (native) - High compression ratio
```

---

## Usage Patterns

### Reading a Capture File
```csharp
// 1. Validate and prepare
if (!libDCAP.IsFileFinalized("capture.dcap")) {
    libDCAP.EnsureDCAPFinalized("capture.dcap");
}
ulong fileSize = libDCAP.GetFileSize("capture.dcap");

// 2. Create reader and I/O
CaptureFileReader reader = new CaptureFileReader();
StdioReader dataReader = new StdioReader("capture.dcap");
reader.Initialize(dataReader, fileSize);

// 3. Register decoders and adapters
GLDecoder glDecoder = new GLDecoder();
MyGLAdapter adapter = new MyGLAdapter();
glDecoder.AddAdapter(adapter);
reader.AddDecoder(glDecoder);

// 4. Process
reader.ProcessAllBlocks();
// OR: reader.ProcessFrameBlocks(frameID);

// 5. Cleanup
reader.Dispose();
```

### Writing Compressed Capture
```csharp
StdioWriter writer = new StdioWriter("output.dcap");
DCAPCompressConsumer consumer = new DCAPCompressConsumer(writer);
consumer.InitializeCompression(CompressionAlgorithm.ZSTD, 
                                Compressor.CompressorQuality.High);

// Process source file with consumer handling writes
// ... (read source, pass to consumer)

consumer.Dispose();
writer.Dispose();
```

---

## Search Hints
```
Find file reader entry:
search "class CaptureFileReader"
open dll/project/DCAPToolsWrapper/CaptureFileReader.cs:5

Find OpenGL ES adapter:
search "class GLAdapter"
open dll/project/DCAPToolsWrapper/GLAdapter.cs:6

Find compression support:
search "InitializeCompression"
open dll/project/DCAPToolsWrapper/DCAPCompressConsumer.cs:157

Find frame indexing:
search "GenerateFrameIndex"
open dll/project/DCAPToolsWrapper/libDCAP.cs:63

Find native P/Invoke declarations:
search "DllImport.*libDCAP"
open dll/project/DCAPToolsWrapper/libDCAPPINVOKE.cs:9

Find decoder base:
search "class Decoder.*IDisposable"
open dll/project/DCAPToolsWrapper/Decoder.cs:5

Find API call types:
search "enum ApiCallType"
open dll/project/DCAPToolsWrapper/ApiCallType.cs
```

---

## Integration Points
| External System | Integration Method | Location |
|----------------|-------------------|----------|
| libDCAP.dll/.so | P/Invoke [DllImport("libDCAP")] | [libDCAPPINVOKE.cs:9+](dll/project/DCAPToolsWrapper/libDCAPPINVOKE.cs#L9) |
| OpenGLESDataPlugin | Uses CaptureFileReader to replay traces | Service logs |
| Android device | libDCAP.so pushed by ADBServiceManager | Service installation |
| Snapdragon Profiler GUI | Loads .dcap files for OpenGL ES analysis | File open workflows |
| SDPCLI | Can load DCAPToolsWrapper.dll plugin | Plugin deployment |

---

## Notes
- **SWIG-Generated**: All 56 files are SWIG-generated C# wrappers around native C++ libDCAP library
- **Zero Namespaces**: All types in global namespace (SWIG default pattern)
- **IDisposable Pattern**: Every SWIG class implements IDisposable for native resource cleanup
- **Handle Safety**: Uses HandleRef pattern to prevent GC premature collection during P/Invoke
- **Virtual Methods**: Adapter/Decoder/Consumer use virtual methods for C#-side overrides
- **GLAdapter Size**: 15,997 lines with 200+ Process_gl* methods (one per OpenGL ES API)
- **Compression**: Supports LZ4 (fast) and ZSTD (high ratio) via native implementation
- **Frame Seeking**: Frame index enables random access to specific frames without full replay
- **Thread Filtering**: Can filter replay to specific thread IDs for multi-threaded analysis
- **Texture Stripping**: DCAPStripConsumer creates smaller files by omitting texture data
- **File Flags**: Header flags indicate compression, stripping, thread ID presence, timestamps
- **Finalization**: DCAP files must be finalized with trailer for validity

---

## Typical Workflows

### Capture File Analysis
1. Validate file with `IsFileFinalized()`
2. Generate frame index with `GenerateFrameIndex()`
3. Create CaptureFileReader and initialize
4. Register GLDecoder + custom GLAdapter
5. Process specific frames or entire file
6. Adapter receives Process_gl* callbacks for each API call
7. Extract statistics, build state, or replay

### Capture File Compression
1. Open source .dcap with CaptureFileReader
2. Create DCAPCompressConsumer with output writer
3. Initialize compression (algorithm + quality)
4. Process source file, consumer writes compressed output
5. Finalize output file
6. Result: smaller file with same API trace

### Texture Data Stripping
1. Open source .dcap
2. Create DCAPStripConsumer
3. Process source, consumer omits texture/buffer data
4. Output: much smaller file with API calls only (no textures)
5. Useful for performance analysis without texture content
