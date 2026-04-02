# SDPCLI Troubleshooting Guide

## CRITICAL: Plugins Setup (Required First!)

### Problem: "Processor plugins directory NOT FOUND"

**Symptom:**
```
SDK PLUGINS_DIR: plugins
  Expected at: d:\snapdragon\SDPCLI\bin\Debug\net9.0\plugins
  ✗ Local plugins directory NOT FOUND

SDK SDP_PROCESSOR_PLUGINS_PATH: plugins/processor/
  ✗ Processor plugins directory NOT FOUND
  CRITICAL: Callbacks require processor plugins!
```

**Root Cause:**
- SDPCore expects `plugins/` directory relative to working directory
- OnProcessAdded callbacks require processor plugins to be loaded
- Without processor plugins, callbacks will NEVER fire (0 processes discovered)

**Solution:**
Run the setup script to create local plugins directory:

```powershell
# From repository root
.\SDPCLI\setup_plugins.ps1
```

**Choice 1: Symbolic Link (Recommended)**
- Saves disk space (~200MB)
- Requires Administrator privileges
- Directly uses official Profiler installation

**Choice 2: Copy Files**
- No admin required  
- Uses more disk space
- Independent of Profiler installation

**After Setup:**
```
d:\snapdragon\SDPCLI\bin\Debug\net9.0\
├── SDPCLI.exe
├── plugins/                    ← Auto-deployed by build
│   └── processor/              ← Processor DLLs for callbacks
└── service/
    └── android/                ← Device plugins (auto-deployed)
```

**Verification:**
After running setup script, you should see:
```
✓ Processor plugins loaded
Found 7 processor DLLs
```

---

# StartApp 超时问题排查

## 当前状态

### ✅ 已修复/匹配
- AppStartSettings 参数完全匹配 GUI：
  - executablePath: package/activity
  - renderingAPIs: 16 (Vulkan)
  - captureType: 4 (Snapshot)
  - launchOptions: `PerfHints:0;DisableUGDFlag:1;`
- Client.Update() 事件循环线程正在运行
- DeviceDelegate 已注册并接收事件
- 设备状态为 Ready

### ❌ 仍然失败
- Device.StartApp() 超时（60秒）
- 错误：`Timed out waiting for a response to a launch application request`

## 观察到的差异

### 服务重装
- 第一次运行：0.2秒（只推送 hash 文件）
- 第二次运行：3.3秒（删除所有文件重新安装）
- **说明**：设备状态在两次运行之间被完全重置

### OnClientConnected 未触发
- SimpleClientDelegate.OnClientConnected() 从未被 SDPCore 调用
- 这意味着 Client 没有检测到"已连接"事件

## 可能的根本原因

1. **Client 初始化不完整**
   - GUI 使用 ConnectionManager 包装了完整的初始化流程
   - 可能有我们遗漏的初始化步骤

2. **设备连接时机问题**
   - Device.Connect() 和 Client 状态可能不同步
   - 可能需要等待某个内部事件

3. **事件处理顺序**
   - Update() 线程启动时机可能太晚
   - 应该在 Client.Init() 之后立即启动

## 下一步调试

### 建议 1：在 GUI 中断点调试
在 GUI 中设置断点：
1. `ConnectionManager.InitCore()` - 查看完整初始化流程
2. `InternalClientDelegate.OnClientConnected()` - 确认何时被调用
3. `Device.Connect()` - 查看连接步骤
4. `Device.StartApp()` - 对比调用时的完整状态

### 建议 2：检查 Client 初始化
```csharp
// GUI 的 InitCore 做了：
- client.Init(sessionSettings)
- client.RegisterEventDelegate(clientDelegate)
- MetricManager.Get().RegisterMetricEventDelegate(metricDelegate)
- MetricManager.Get().RegisterMetricCategoryEventDelegate(metricCategoryDelegate)
- DeviceManager 各种设置
- ReloadDeviceConnectionsAndSearch()
```

### 建议 3：验证 Update 返回值
运行最新代码，检查：
-update_count 是否增加
- `hadEvents` 是否曾经为 true
- 是否有任何异常输出

### 建议 4：简化测试
创建最小复现环境：
1. 只连接设备，不启动应用
2. 观察 OnClientConnected 是否被调用
3. 如果没有，说明 Client 状态有问题

## CLI vs GUI 调用栈对比

### GUI 调用栈
```
Device.StartApp(AppStartSettings settings)
  ← Sdp.DataSourcesController.<ProcessLaunchRequest>b__0()
    ← Task.Run()
    ← async/await
```

### CLI 调用栈  
```
Device.StartApp(AppStartSettings settings)
  ← Application.LaunchApp()
    ← Task.Run()
    ← Task.Wait(60s) // 同步等待
```

**差异**：GUI 使用 async/await，CLI 使用 Task.Wait()
- 但这不应该导致超时，只是执行方式不同
