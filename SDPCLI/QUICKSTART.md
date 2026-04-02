# 快速开始 - SDPCLI

SDPCLI 是 Snapdragon Profiler 的命令行工具，支持 4 种模式：**Capture（设备截图）**、**Analysis（离线分析）**、**Texture Extraction（纹理提取）**、**Shader Extraction（Shader 提取）**。

---

## 前置要求

1. 安装 [Snapdragon Profiler](https://www.qualcomm.com/developer/software/snapdragon-profiler) 到默认路径  
   `C:\Program Files\Qualcomm\Snapdragon Profiler`
2. 安装 [.NET SDK](https://dotnet.microsoft.com/download)（用于编译）
3. (Capture 模式) USB 连接 Android 设备，启用 USB 调试

---

## 编译

```powershell
cd D:\snapdragon
dotnet build SDPCLI
# 输出：SDPCLI\bin\Debug\net472\SDPCLI.exe
```

---

## 模式 1：Capture（连接设备抓帧）

**需要**：USB 连接的手机 + app 正在运行 + Vulkan 渲染

```powershell
cd SDPCLI\bin\Debug\net472

# 交互模式（推荐初次使用）
SDPCLI.exe

# 或者直接进入 Capture
SDPCLI.exe -mode capture
```

成功标志：
```
[ClientDelegate] Client connected - starting background Realtime capture...
✓ Background Realtime capture started (ID: 1)
[Process Added] PID=12345, Name=com.ea.gp.fcmnova, State=ProcessRunning
...
✓ Capture exported to: test\session_20260327_143022\
```

**config.ini 配置**（可跳过，使用交互模式）：
```ini
PackageName=com.your.app
ActivityName=.MainActivity
RenderingAPI=16       # 16=Vulkan
CaptureType=4         # 4=Snapshot
```

---

## 模式 2：Analysis（分析已有 .sdp 文件）

**不需要**设备连接，纯离线分析。

```powershell
cd SDPCLI\bin\Debug\net472

# 分析指定 .sdp 文件（相对路径基于 config.ini 中 TestDirectory 或 SDPCLI\test\）
SDPCLI.exe -mode analysis -sdp "2026-03-20T20-36-12.sdp"

# 或者绝对路径
SDPCLI.exe -mode analysis -sdp "D:\snapdragon\SDPCLI\test\2026-03-20T20-36-12.sdp"
```

成功输出：
```
=== Analysis Mode ===
Opening: test\2026-03-20T20-36-12.sdp
Extracting sdp.db...
Total DrawCalls: 2048
Report saved: test\DrawCallReport_20260327_143022.csv
```

---

## 模式 3：Texture Extraction（提取纹理）

从 .sdp 文件中提取指定纹理，保存为 PNG。

**步骤 1**：先找到可用的 resourceID （分析模式下查询，或用 sqlite3）：
```sql
-- 在 sdp.db 中执行
SELECT resourceID, width, height, format FROM VulkanSnapshotTextures 
WHERE captureID=3 AND width > 64 ORDER BY width*height DESC LIMIT 20;
```

**步骤 2**：提取
```powershell
SDPCLI.exe -mode extract-texture -sdp "test\capture.sdp" -resource-id 23352

# 指定输出路径和 captureID
SDPCLI.exe -mode extract-texture -sdp "test\capture.sdp" -resource-id 23352 -output "test\tex.png" -capture-id 3
```

成功输出：
```
=== Texture Extraction Mode ===
SDP: test\capture.sdp
ResourceID: 23352, CaptureID: 3
✓ Texture extracted: test\texture_23352.png (1024x1024 RGBA8)
```

---

## 模式 4：Shader Extraction（提取 Shader）

从 .sdp 文件中提取指定 DrawCall 使用的 Shader（GLSL/SPIR-V）。

```powershell
# 通过 DrawCall ID 提取（格式：帧.提交.DrawCall）
SDPCLI.exe -mode extract-shader -sdp "test\capture.sdp" -drawcall-id "1.1.5"

# 或者通过 Pipeline ID 提取
SDPCLI.exe -mode extract-shader -sdp "test\capture.sdp" -pipeline-id 42 -output "shaders\"
```

---

## 常见问题

### Q: "ProcessManager has 0 processes"
- 确认 `service/android/` 目录存在于 `bin\Debug\net472\service\`
- 运行 `dotnet build` 会自动部署，或参考 [PLUGIN_DEPLOYMENT.md](PLUGIN_DEPLOYMENT.md)

### Q: DLL 加载失败
- 确认 Snapdragon Profiler 安装在 `C:\Program Files\Qualcomm\Snapdragon Profiler`
- 确认使用 x64 构建，不要用 x86

### Q: 纹理提取失败（No texture data found）
- 纹理数据可能存储在 `.gfxr` 文件中而非数据库
- 参考 [TEXTURE_QUERY_LIMITATION.md](TEXTURE_QUERY_LIMITATION.md)

---

## 参考文档

| 文档 | 说明 |
|------|------|
| [README.md](README.md) | 完整文档、架构说明 |
| [CLI_PARAMETERS.md](CLI_PARAMETERS.md) | 所有命令行参数 |
| [CONFIG_GUIDE.md](CONFIG_GUIDE.md) | config.ini 配置说明 |
| [TROUBLESHOOTING.md](TROUBLESHOOTING.md) | 故障排查 |
| [PLUGIN_DEPLOYMENT.md](PLUGIN_DEPLOYMENT.md) | 设备端插件部署 |
| [TEXTURE_EXTRACTION_GUIDE.md](TEXTURE_EXTRACTION_GUIDE.md) | 纹理提取详细指南 |
| [VULKAN_SNAPSHOT_MODEL_USAGE.md](VULKAN_SNAPSHOT_MODEL_USAGE.md) | VulkanSnapshotModel API 使用 |
