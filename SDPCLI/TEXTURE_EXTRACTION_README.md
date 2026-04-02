# 纹理提取功能实现总结

## 🎯 功能状态

### ✅ 已实现
- **TextureExtractor 类**：直接从 SQLite `VulkanSnapshotByteBuffers` 表读取纹理二进制数据
- **TextureExtractionMode**：命令行模式 `SDPCLI.exe -mode extract-texture`，一键从 .sdp 提取纹理（自动解压到临时目录）
- **TextureConverterHelper**：调用原生 TextureConverter.dll 进行格式转换（支持 RGBA8、BGRA8、Float16/32、ASTC）
- **ASTC 头部处理**：AASTC 格式进行块大小自动推断 + 文件头添加

### ❌ 已知限制
- 部分纹理数据存储在 `.gfxr` / `.gfxrz` 文件中，无法从 SQLite 提取
- 目前不支持批量提取（正在计划）
- Mipmap 层级提取需要手动指定 level


## 🚀 快速开始

### 方式1：使用 CLI 命令（推荐）

```powershell
cd SDPCLI\bin\Debug\net472

# 提取指定 resourceID 的纹理
SDPCLI.exe -mode extract-texture -sdp "test\2026-03-20T20-36-12.sdp" -resource-id 23352

# 指定输出路径和 captureID
SDPCLI.exe -mode extract-texture `-sdp "test\capture.sdp" -resource-id 23352 -output "out\texture.png" -capture-id 3
```

成功输出：
```
=== Texture Extraction Mode ===
SDP: test\2026-03-20T20-36-12.sdp
ResourceID: 23352 (captureID=3)
  Size: 4096x64
  Format: 97 (VK_FORMAT_R16G16B16A16_SFLOAT)
  Data size: 2097152 bytes
✓ Texture extracted: test\texture_23352.png
```

### 方式2：通过 C# 代码直接调用

```csharp
using SnapdragonProfilerCLI.Tools;

var extractor = new TextureExtractor(
    @"D:\snapdragon\SDPCLI\test\temp\sdp.db",
    captureId: 3
);
bool success = extractor.ExtractTexture(23352, @"D:\output\texture.png");
```

### 如何查找可用的 resourceID

先将 .sdp 解压得到 sdp.db（SDP 文件是标准 ZIP 格式），然后用 sqlite3 查询：
```powershell
$zip = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path "test\capture.sdp"))
$entry = $zip.Entries | Where-Object { $_.Name -eq "sdp.db" }
[System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, "test\temp\sdp.db", $true)
$zip.Dispose()

# 用 sqlite3 查询大尺寸纹理
sqlite3 test\temp\sdp.db ".mode column" "
SELECT resourceID, width, height, format FROM VulkanSnapshotTextures
WHERE captureID=3 AND width>64 ORDER BY width*height DESC LIMIT 20;
"
```

## 📋 支持的格式

| Vulkan Format | Code | 说明 | 支持 |
|---------------|------|------|------|
| R8G8B8A8_UNORM | 37 | RGBA 8-bit | ✅ |
| B8G8R8A8_UNORM | 43 | BGRA 8-bit | ✅ |
| R16G16B16A16_SFLOAT | 97 | RGBA Half-Float | ✅ |
| R32G32_SFLOAT | 103 | RG Float | ✅ |
| R32G32B32_SFLOAT | 106 | RGB Float | ✅ |
| R32G32B32A32_SFLOAT | 109 | RGBA Float | ✅ |
| ASTC_4x4_UNORM_BLOCK | 183 | ASTC 压缩 | ✅ |

## ⚠️ 已知限制

### 1. 数据可用性
- **并非所有纹理数据都在数据库中**
- 部分纹理可能存储在 `.gfxr` / `.gfxrz` 文件中
- 如果提取失败，显示 "No texture data found in ByteBuffers"

### 2. 格式支持
- 目前映射了最常用的 7 种格式
- 未映射的格式会使用默认 Q_FORMAT_RGBA_8UI
- 可能导致颜色错误或转换失败

### 3. 性能
- 大尺寸纹理（4096x4096+）转换需要几秒
- TextureConverter.dll 是原生 C++ 库，效率较高
- 内存复制（Marshal.Copy）有一定开销

## 📁 文件结构

```
SDPCLI/
├── source/
│   └── Tools/
│       └── TextureExtractor.cs          ← 核心实现
│   └── Modes/
│       └── TextureExtractionMode.cs     ← CLI 模式封装
├── TEXTURE_EXTRACTION_GUIDE.md           ← 完整使用文档
└── test/
    └── 2026-03-20T20-36-12.sdp          ← 测试数据
```

## 🔧 实现细节

### 核心数据流程

```
SDP 文件 (.sdp)
    ↓ [自动解压到临时目录]
sdp.db (SQLite)
    ↓ [SQL: SELECT data FROM VulkanSnapshotByteBuffers WHERE resourceID=?]
byte[] 纹理二进制数据
    ↓ [TextureConverterHelper.ConvertImageToRGBA]
RGBA8888 数据
    ↓ [System.Drawing.Bitmap]
PNG 文件
```

### 依赖的 DLL
- ✅ **TextureConverter.dll** - 原生格式转换库（由 Qualcomm 提供）
- ✅ **SDPClientFramework.dll** - 提供 TextureConverterHelper
- ✅ **System.Data.SQLite.dll** - SQLite 数据库访问
- ✅ **System.Drawing.dll** - Bitmap 和图片保存

## 🎯 下一步改进

### 短期
- [ ] 批量提取功能（一次提取所有纹理）
- [ ] Mipmap 层级选择
- [ ] 添加更多格式映射（参考 QGLPlugin/VkHelper.cs）

### 中期
- [ ] 支持 JPG/DDS 等输出格式
- [ ] 支持纹理数组和立方体贴图
- [ ] 解析 `.gfxr`/`.gfxrz` 文件格式

## 📚 参考资料

### SDPClientFramework 相关
- [ByteBufferGateway.cs](dll/project/SDPClientFramework/Sdp/ByteBufferGateway.cs)
- [TextureConverterHelper.cs](dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs)
- [TFormats.cs](dll/project/SDPClientFramework/TextureConverter/TFormats.cs)

### QGLPlugin 参考实现
- [VkHelper.cs](dll/project/QGLPlugin/VkHelper.cs) - Vulkan 格式映射
- [ResourcesViewMgr.cs](dll/project/QGLPlugin/ResourcesViewMgr.cs) - 完整的纹理显示实现

### 文档
- [SDP.Graphics.TextureConverter 模块索引](docs/index/modules/SDP.Graphics.TextureConverter.md)
- [完整使用指南](TEXTURE_EXTRACTION_GUIDE.md)

## ❓ 常见问题

### Q: 为什么某些纹理提取失败？
A: 纹理数据可能不在 sdp.db 中，而是存储在 .gfxr/.gfxrz 文件中。这些文件是 GFXReconstruct 的专有格式，目前未实现解析。

### Q: 提取的图片颜色不对？
A: 可能是格式映射错误。检查 VkFormatToTFormat() 方法是否正确映射了该格式。

### Q: 可以批量提取所有纹理吗？
A: 目前需要手动逐个提取。可以修改脚本添加循环批量处理。

### Q: 支持压缩格式吗？
A: 支持 ASTC 格式（通过 TextureConverter.dll），但输出的是解压后的 PNG。如果需要保留压缩格式，建议导出为 DDS。

## 👥 贡献

如果您发现新的格式或改进建议，请更新：
1. `TextureExtractor.cs` 中的 `VkFormatToTFormat()` 方法
2. `TEXTURE_EXTRACTION_GUIDE.md` 中的格式表
3. 添加测试用例验证新格式

## 📝 许可

本工具基于 Snapdragon Profiler SDK 开发，仅供学习和开发使用。
