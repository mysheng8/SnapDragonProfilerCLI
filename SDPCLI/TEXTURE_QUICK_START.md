# 纹理提取功能 - 快速上手指南

## ✅ 实现状态

纹理提取功能已完整实现，包括：
- ✅ `TextureExtractor.cs` - 核心实现（直接查询 SQLite，使用 TextureConverter.dll 转换）
- ✅ `TextureExtractionMode.cs` - CLI 模式（`-mode extract-texture`）
- ✅ 编译版本可用 (`bin/Debug/net472/SDPCLI.exe`)

---

## 🚀 使用方法

### 方法1: CLI 命令（推荐）

```powershell
cd SDPCLI\bin\Debug\net472

# 基本用法（自动推断输出路径和 captureID）
SDPCLI.exe -mode extract-texture -sdp "test\2026-03-20T20-36-12.sdp" -resource-id 23352

# 指定输出文件和 captureID
SDPCLI.exe -mode extract-texture -sdp "test\capture.sdp" -resource-id 23352 -output "test\texture.png" -capture-id 3
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

### 方法2: 通过 C# 代码直接调用

在您自己的 C# 代码中：

```csharp
using SnapdragonProfilerCLI.Tools;

// 创建提取器（直接传入已解压的 sdp.db 路径）
var extractor = new TextureExtractor(
    @"D:\snapdragon\SDPCLI\test\temp\sdp.db",  // 数据库路径
    captureId: 3                                 // CaptureID
);

// 提取纹理
bool success = extractor.ExtractTexture(
    resourceId: 1028,
    outputPath: @"D:\output\texture.png"
);
```

---

## 🔍 查找可提取的纹理

### 步骤 1：解压 sdp.db
```powershell
# SDP 文件本质是 ZIP 压缩包
$zip = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path "test\capture.sdp"))
$entry = $zip.Entries | Where-Object { $_.Name -eq "sdp.db" }
New-Item -ItemType Directory -Force -Path "test\temp" | Out-Null
[System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, "test\temp\sdp.db", $true)
$zip.Dispose()
```

### 步骤 2：查询可用纹理
```powershell
sqlite3 test\temp\sdp.db ".mode column" @"
SELECT resourceID, width, height, format
FROM VulkanSnapshotTextures
WHERE captureID = 3 AND width > 64
ORDER BY width * height DESC
LIMIT 20;
"@
```

输出示例：
```
resourceID  width  height  format
----------  -----  ------  ------
4421        1920   1080    183     <- ASTC 压缩
1028        1024   1024    37      <- RGBA8
2359        1024   1024    97      <- Float16
```

---

## ⚠️ 已知限制

### 数据可用性
- ✅ **可提取**：纹理数据存储在 `VulkanSnapshotByteBuffers` 表中
- ❌ **不可提取**：纹理数据存储在 `.gfxr` / `.gfxrz` 文件中
- 如果提取失败（"No texture data found in VulkanSnapshotByteBuffers"），说明数据不在数据库中

详细说明见 [TEXTURE_QUERY_LIMITATION.md](TEXTURE_QUERY_LIMITATION.md)

### 格式支持

| Vulkan Format | Code | 支持 | 输出 |
|---------------|------|------|------|
| R8G8B8A8_UNORM | 37 | ✅ | PNG |
| B8G8R8A8_UNORM | 43 | ✅ | PNG |
| R16G16B16A16_SFLOAT | 97 | ✅ | PNG |
| R32G32B32A32_SFLOAT | 109 | ✅ | PNG |
| ASTC_4x4_UNORM_BLOCK | 183 | ✅ | PNG (解压) |

---

## 📁 相关文件

- [source/Tools/TextureExtractor.cs](source/Tools/TextureExtractor.cs) - 核心实现
- [source/Modes/TextureExtractionMode.cs](source/Modes/TextureExtractionMode.cs) - CLI 模式
- [TEXTURE_EXTRACTION_GUIDE.md](TEXTURE_EXTRACTION_GUIDE.md) - 完整文档
- [TEXTURE_EXTRACTION_README.md](TEXTURE_EXTRACTION_README.md) - 技术总结
- [TEXTURE_QUERY_LIMITATION.md](TEXTURE_QUERY_LIMITATION.md) - 已知限制说明

---

## ❓ 常见问题

**Q: 如何知道哪些纹理可以提取？**  
A: 查询 `VulkanSnapshotByteBuffers` 表中是否有该 resourceID 的数据。元数据存在（`VulkanSnapshotTextures`）不代表数据在数据库中。

**Q: 提取失败怎么办？**  
A: 如果显示 "No texture data found in VulkanSnapshotByteBuffers"，数据在 `.gfxr` 文件中，目前未支持。可用官方 Snapdragon Profiler GUI 导出。

**Q: ASTC 纹理为什么输出为 PNG 而不是 `.astc` 文件？**  
A: TextureConverter.dll 会解码 ASTC 为 RGBA，输出 PNG 是解压后的版本。

## 📚 API 参考

- `TextureExtractor(string databasePath, int captureId)` - 构造器
- `bool ExtractTexture(ulong resourceId, string outputPath)` - 提取并保存为 PNG
- [TextureConverterHelper](dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs) - 格式转换
- [VkHelper.cs](dll/project/QGLPlugin/VkHelper.cs) - Vulkan 格式映射参考


## 🚀 使用方法

### 方法1: 列出可用纹理

```powershell
cd D:\snapdragon\SDPCLI
.\list_textures.ps1 -SdpPath "test\2026-03-20T20-36-12.sdp"
```

输出示例：
```
resourceID  width  height  format
----------  -----  ------  ------
4421        1920   1080    183     <- ASTC 压缩
1028        1024   1024    37      <- RGBA8
2359        1024   1024    122     
```

### 方法2: 通过 C# 代码直接调用

在您自己的 C# 代码中：

```csharp
using SnapdragonProfilerCLI.Tools;

// 创建提取器
var extractor = new TextureExtractor(
    @"D:\snapdragon\SDPCLI\test\temp\sdp.db",  // 数据库路径
    captureId: 3                                 // CaptureID
);

// 提取纹理
bool success = extractor.ExtractTexture(
    resourceId: 1028,
    outputPath: @"D:\output\texture.png"
);
```

### 方法3: 使用编译好的工具

由于 PowerShell 脚本与 .NET 9.0/4.7.2 DLL 加载存在兼容性问题，推荐：

1. **添加到 SDPCLI 命令行**（需要实现）：
```powershell
.\bin\Debug\net472\SDPCLI.exe extract-texture --sdp "file.sdp" --resource-id 1028 --output "texture.png"
```

2. **或者在 Visual Studio 中调试运行**

## ⚠️ 已知限制

### 数据可用性
- ✅ **可提取**：纹理数据存储在 `VulkanSnapshotByteBuffers` 表中
- ❌ **不可提取**：纹理数据存储在 `.gfxr` / `.gfxrz` 文件中
- 如果 ByteBufferGateway 返回 null，说明数据不在数据库中

### PowerShell 脚本限制
- `extract_texture_simple.ps1` 遇到 DLL 加载问题
- 原因：.NET Framework 4.7.2 和 PowerShell 5.1 的兼容性
- 解决方案：使用编译好的 C# 程序或添加 CLI 命令

## 📋 支持的格式

| Vulkan Format | Code | 支持 | 输出 |
|---------------|------|------|------|
| R8G8B8A8_UNORM | 37 | ✅ | PNG |
| B8G8R8A8_UNORM | 43 | ✅ | PNG |
| R16G16B16A16_SFLOAT | 97 | ✅ | PNG |
| R32G32B32A32_SFLOAT | 109 | ✅ | PNG |
| ASTC_4x4_UNORM_BLOCK | 183 | ✅ | PNG (解压) |

## 🔧 技术实现

### 工作流程
```
SDP文件 (.sdp)
    ↓ [解压]
sdp.db (SQLite)
    ↓ [ByteBufferGateway]
二进制纹理数据 (原生内存)
    ↓ [Marshal.Copy]
byte[] 托管数组
    ↓ [TextureConverterHelper.ConvertImageToRGBA]
RGBA8888 数据
    ↓ [System.Drawing.Bitmap]
PNG 文件
```

### 核心API调用
```csharp
// 1. 读取数据
var gateway = new ByteBufferGateway("VulkanSnapshot", "VulkanSnapshotByteBuffers");
IByteBuffer buffer = gateway.GetByteBuffer(captureId, resourceId);

// 2. 复制到托管内存
byte[] data = new byte[buffer.BDP.size];
Marshal.Copy(buffer.BDP.data, data, 0, (int)buffer.BDP.size);

// 3. 转换格式
byte[] rgba = TextureConverterHelper.ConvertImageToRGBA(
    data, 
    TFormats.Q_FORMAT_RGBA_8UI, 
    width, 
    height,
    flipBR: true, 
    rowStride: 0
);

// 4. 保存图片
Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
Marshal.Copy(rgba, 0, bmpData.Scan0, rgba.Length);
bmp.Save("texture.png", ImageFormat.Png);
```

## 📁 文件

- `source/Tools/TextureExtractor.cs` - 核心实现
- `list_textures.ps1` - 列出纹理工具 ✅ 可用
- `extract_texture_simple.ps1` - 提取工具 ⚠️ DLL加载问题
- `TEXTURE_EXTRACTION_GUIDE.md` - 完整文档
- `TEXTURE_EXTRACTION_README.md` - 技术总结

## 🎯 下一步

### 推荐方案：添加 CLI 命令

在 `Application.cs` 中添加：

```csharp
if (args[0] == "extract-texture")
{
    string sdpPath = GetArg("--sdp");
    ulong resourceId = ulong.Parse(GetArg("--resource-id"));
    string output = GetArg("--output");
    
    // 从 SDP 提取 sdp.db
    string dbPath = ExtractDatabase(sdpPath);
    
    // 提取纹理
    var extractor = new TextureExtractor(dbPath, captureId: 3);
    extractor.ExtractTexture(resourceId, output);
    
    return;
}
```

然后就可以：
```powershell
.\SDPCLI.exe extract-texture --sdp "file.sdp" --resource-id 1028 --output "texture.png"
```

## ✅ 已验证功能

1. ✅ `TextureExtractor` 类编译成功
2. ✅ `list_textures.ps1` 可以列出纹理
3. ✅ 使用 ByteBufferGateway 读取数据
4. ✅ 使用 TextureConverterHelper 转换格式
5. ✅ 支持 7 种常用纹理格式

## ❓ 常见问题

**Q: extract_texture_simple.ps1 为什么不能用？**  
A: PowerShell 5.1 加载 .NET Framework 4.7.2 DLL 时有兼容性问题，特别是 SQLite 的原生互操作。建议使用编译好的 SDPCLI.exe。

**Q: 如何知道哪些纹理可以提取？**  
A: 使用 `list_textures.ps1` 查看，如果列表中有纹理，说明元数据存在，但数据可能在 `.gfxr` 文件中。需要实际尝试才能确定。

**Q: 提取失败怎么办？**  
A: 如果显示 "No texture data found in ByteBuffers"，说明数据在 `.gfxr` 文件中，目前未实现该格式解析。请使用官方 Snapdragon Profiler GUI。

## 📚 参考

- [TextureExtractor.cs](source/Tools/TextureExtractor.cs)
- [SDPClientFramework ByteBufferGateway](dll/project/SDPClientFramework/Sdp/ByteBufferGateway.cs)
- [TextureConverterHelper](dll/project/SDPClientFramework/TextureConverter/TextureConverterHelper.cs)
- [QGLPlugin VkHelper](dll/project/QGLPlugin/VkHelper.cs) - 格式映射参考
