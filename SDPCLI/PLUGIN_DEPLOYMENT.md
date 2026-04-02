# 设备端 Plugins 部署说明

## 问题

ProcessManager 无法发现进程，因为设备端 plugins 没有被正确部署。

## 原因

SDPCore 在连接设备时会自动推送以下文件到设备：
- `service/android/arm64-v8a/plugin*` - 各种插件（GPU、CPU、Network 等）
- `service/android/arm64-v8a/sdpservice` - 设备端服务
- `service/android/arm64-v8a/qhas_sdp` - 硬件抽象服务

这些插件运行在设备上，负责监控进程（如检测使用 Vulkan/OpenGL 的应用），然后通过 `DataProvider.AddProcess()` 报告给 Windows 端的 ProcessManager。

**如果 SDPCore 找不到这些文件，就无法推送到设备，ProcessManager 就永远看不到进程！**

> **路径说明**： SDPCLI 源目录下是 `android/`，构建时会自动复制到 `bin\Debug\net472\service\android\`。SDPCore 从**当前工作目录**查找 `service/android/`。

## 解决方案

### 自动部署（推荐）

构建项目时会自动运行 `DeployAndroidPlugins` 构建目标，将 `android/` 目录复制到输出 `service/android/`：

```bash
cd SDPCLI
dotnet build
```

输出日志会显示：
```
=== Deploying Android Service Files ===
Copying android/ to bin\Debug\net472\service/android/
✓ Android service files deployed to service/android/
```

### 手动部署

如果自动部署失败，可以手动复制：

```powershell
# PowerShell
New-Item -ItemType Directory -Force -Path "bin\Debug\net472\service\android"
Copy-Item -Recurse -Force android\* bin\Debug\net472\service\android\
```

### 验证部署

运行程序时会显示：

```
=== SDPCore environment setup ===
Executable directory: D:\snapdragon\SDPCLI\bin\Debug\net472\
✓ service/android/ directory found (matching official structure)
✓ Found 17 plugin files in service/android/arm64-v8a/
    ✓ pluginGPU-Vulkan (123456 bytes)
    ✓ pluginCPU (78901 bytes)
    ✓ sdpservice (234567 bytes)
    ✓ qhas_sdp (345678 bytes)
✓ Working directory set to: D:\snapdragon\SDPCLI\bin\Debug\net472
```

## 运行流程

1. **Windows 端启动**
   - Main.cs 检查 `bin/Debug/net472/service/android/` 是否存在
   - 设置当前工作目录为可执行文件目录
   
2. **连接设备**
   - SDPCore 从当前目录查找 `service/android/arm64-v8a/` 或 `service/android/armeabi-v7a/`（根据设备架构）
   - 通过 ADB 推送所有 plugin 文件、sdpservice、qhas_sdp 到设备
   
3. **设备端运行**
   - sdpservice 作为后台服务启动
   - 各个 plugin（如 pluginGPU-Vulkan）开始监控系统
   - 检测到使用 Vulkan 的应用时，报告给 SDPCore
   
4. **Windows 端接收**
   - SDPCore 收到设备端 plugin 的报告
   - 调用 `ProcessManager.AddProcess(pid, name, ...)`
   - 触发 `OnProcessAdded(pid)` 回调
   
5. **CLI 可以捕获**
   - ProcessManager 现在有进程列表
   - WaitForProcessDiscovery() 找到目标进程
   - StartCapture() 可以正确指定 PID

## 故障排查

### 症状：ProcessManager 始终为空

**检查清单：**
1. ✓ `bin/Debug/net472/service/android/` 目录存在
2. ✓ 包含 `arm64-v8a/` 和 `armeabi-v7a/` 子目录
3. ✓ 每个子目录有 plugin* 文件
4. ✓ 包含 `sdpservice` 和 `qhas_sdp`
5. ✓ 设备通过 ADB 连接（`adb devices` 显示设备）
6. ✓ 应用正在使用 Vulkan/OpenGL（不是纯 CPU 应用）

### 日志示例（成功）

运行 SDPCLI 时应该看到：

```
[ClientDelegate] Client connected - starting background Realtime capture...
✓ Background Realtime capture started (ID: 1)
  This enables ProcessManager to monitor app processes.
  
[Process Added] PID=12345, Name=com.ea.gp.fcmnova, State=ProcessRunning
```

### 日志示例（失败）

```
⚠ WARNING: service/android/ directory not found at: D:\snapdragon\SDPCLI\bin\Debug\net472\service\android
  Device-side plugins are MISSING!
  Run 'dotnet build' to deploy plugins automatically.
  
[... later ...]
  
[Attempt 1/20] ProcessManager has 0 processes
[Attempt 5/20] ProcessManager has 0 processes
[Attempt 20/20] ProcessManager has 0 processes
⚠ Process Discovery FAILED after 10 seconds
```

## 相关文件

- `SDPCLI.csproj` - 包含自动部署的构建目标（`DeployAndroidPlugins`）
- `Main.cs` - `SetupEnvironment()` 函数检查和验证 `service/android/` 目录
- `Application.cs` - Process discovery 逻辑
- `SimpleClientDelegate.cs` - OnProcessAdded 回调

## 技术细节

**为什么要复制到输出目录？**

SDPCore 从**当前工作目录**查找 `service/android/` 文件夹，而不是从环境变量或注册表。工作目录通常是可执行文件所在目录（`bin/Debug/net472/`），所以必须在那里有 `service/android/` 副本。

**目录结构**：
```
bin\Debug\net472\
├── SDPCLI.exe
├── plugins\                    ← Processor DLL（调用 OnProcessAdded 回调需要）
│   └── processor\
└── service\
    └── android\               ← 设备端插件（自动部署）
        ├── arm64-v8a\
        │   ├── pluginGPU-Vulkan
        │   ├── sdpservice
        │   └── ...
        └── armeabi-v7a\
```
