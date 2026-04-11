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

## 模式 2：analysis（分析已有 .sdp 文件）

**不需要**设备连接，纯离线分析。

```powershell
cd SDPCLI\bin\Debug\net472

# 分析所有 snapshot（路径相对于 SdpDir，默认 project\sdp\）
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp

# 只分析 snapshot_3
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp -s 3

# 增量：只重新生成 label（不重新提取资源）
SDPCLI.exe analysis 2026-04-11T09-50-42.sdp -s 3 -t label
```

成功输出：
```
=== Analysis Pipeline ===
  Database: D:\snapdragon\project\sdp\2026-04-11T09-50-42\sdp.db
  Session folder: D:\snapdragon\project\analysis\2026-04-11T09-50-42
  Capture output: ...\snapshot_3
Step 1: Collecting DrawCalls...
  → 1824 DrawCalls collected
Step 4: Writing JSON outputs...
  ✓ dc.json, shaders.json, textures.json, metrics.json, status.json
```

**分析输出位置**：`<ProjectDir>/analysis/<sdp_basename>/snapshot_N/`

---

## 模式 3：extract-texture（提取纹理）

从 .sdp 文件中提取指定纹理，保存为 PNG。

**步骤 1**：在 sdp.db 中找到 resourceID：
```sql
SELECT resourceID, width, height, format FROM VulkanSnapshotTextures
WHERE captureID=3 AND width > 64 ORDER BY width*height DESC LIMIT 20;
```

**步骤 2**：提取
```powershell
SDPCLI.exe extract-texture 2026-04-11T09-50-42.sdp -resource-id 23352

# 指定输出路径和 captureID
SDPCLI.exe extract-texture 2026-04-11T09-50-42.sdp -resource-id 23352 -capture-id 3 -output out\tex.png
```

---

## 模式 4：extract-shader（提取 Shader）

从 .sdp 文件中提取指定 pipeline 的 Shader（SPIR-V/GLSL）。

```powershell
# 通过 Pipeline ID 提取
SDPCLI.exe extract-shader 2026-04-11T09-50-42.sdp -pipeline-id 42

# 指定输出目录
SDPCLI.exe extract-shader 2026-04-11T09-50-42.sdp -pipeline-id 42 -output shaders\
```

---

## 参考文档

| 文档 | 说明 |
|------|------|
| [README.md](README.md) | 完整文档、架构说明 |
| [CLI_PARAMETERS.md](CLI_PARAMETERS.md) | 所有命令行参数 |
| [CONFIG_GUIDE.md](CONFIG_GUIDE.md) | config.ini 配置说明 |
