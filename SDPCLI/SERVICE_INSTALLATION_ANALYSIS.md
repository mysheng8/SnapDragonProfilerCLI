# Service Installation Analysis

## ✅ 问题已解决！关键发现

### 实际的 Service 组件

经过实际测试，Snapdragon Profiler 的服务包含以下组件：

#### 1. **APK 包（必须安装）**
- **包名**: 
  - `com.qualcomm.snapdragonprofiler.profilerlayer.arm64_v8a`
  - `com.qualcomm.snapdragonprofiler.profilerlayer.armeabi_v7a`
- **文件位置**: `android/arm64-v8a/apk/app-arm64-v8a-release.apk`
- **版本**: 2026.1.0.1282026
- **作用**: 提供 Profiler Layer 支持（非独立运行的应用）
- **安装命令**:
  ```bash
  adb install -r android/arm64-v8a/apk/app-arm64-v8a-release.apk
  ```

#### 2. **原生服务二进制文件（必须推送到设备）**
- **sdpservice** (49KB) - 主服务程序
- **qhas_sdp** (1.7MB) - 硬件抽象服务
- **目标路径**: `/data/local/tmp/`
- **推送命令**:
  ```bash
  adb push android/arm64-v8a/sdpservice /data/local/tmp/
  adb push android/arm64-v8a/qhas_sdp /data/local/tmp/
  adb shell chmod 755 /data/local/tmp/sdpservice
  adb shell chmod 755 /data/local/tmp/qhas_sdp
  ```

#### 3. **共享库文件（必须推送）**
需要推送以下共享库以支持服务运行：
- `libSDPCore.so` (2.1MB) - 核心库
- `libc++_shared.so` (1MB) - C++ 标准库
- `libDCAP.so` (2.4MB)
- `libmcpp.so` (130KB)
- `libqtrace.so` (75KB)
- `libsysmon.so` (9KB)
- `libsysmondomain.so` (17KB)
- `libsysmondomainV2.so` (17KB)
- `libsysmonslpi.so` (9KB)

**批量推送命令**:
```powershell
foreach ($lib in @('libSDPCore.so', 'libc++_shared.so', 'libDCAP.so', 'libmcpp.so', 'libqtrace.so', 'libsysmon.so', 'libsysmondomain.so', 'libsysmondomainV2.so', 'libsysmonslpi.so')) {
    adb push "android\arm64-v8a\$lib" /data/local/tmp/
}
```

---

## 🔍 诊断过程记录

从运行日志可以看到：
```
[SDPCore:INFO][SDPCore.ADBServiceManager] [bcc349da] Removing file '' from the target...
[SDPCore:INFO][SDPCore.ADBServiceManager] [bcc349da] Pushing hash file...
[SDPCore:INFO][SDPCore.ADBServiceManager] [bcc349da] Completed service installation in 0.228660 seconds.
```

但实际设备检查显示：
- ✗ sdpdaemon 进程未运行
- ✗ /data/local/tmp/ 目录下没有 service 文件
- ✗ ProcessManager 无法获取进程列表

这表明 **SDPCore 声称安装成功，但实际没有推送文件到设备**。

---

## ClientFramework 中的 Service 安装相关代码

### 1. 安装配置入口点

**文件**: `dll/project/SDPClientFramework/Sdp/ConnectionManager.cs`

#### InitCore 方法 (Line 52-160)
```csharp
public bool InitCore(SessionSettingsSelection sessionSettingsSelections)
{
    // ...
    
    // 配置是否删除服务文件
    if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(
        UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)))
    {
        DeviceManager.Get().SetDeleteServiceFilesOnExit(
            BoolConverter.Convert(userPreferences.RetrieveSetting(
                UserPreferenceModel.UserPreference.DeleteServiceFilesOnExit)));
    }
    
    // 获取环境信息（包含 PLUGINS_DIR 等路径）
    string environment = DeviceManager.Get().GetEnvironment();
    
    // ...
}
```

#### ReloadDeviceConnectionsAndSearch 方法 (Line 151-280)
```csharp
public void ReloadDeviceConnectionsAndSearch()
{
    // ...
    
    // 设置安装超时
    if (!string.IsNullOrEmpty(userPreferences.RetrieveSetting(
        UserPreferenceModel.UserPreference.InstallerTimeout)))
    {
        deviceManager.SetInstallerTimeout(
            IntConverter.Convert(userPreferences.RetrieveSetting(
                UserPreferenceModel.UserPreference.InstallerTimeout)));
    }
    
    // Find devices (触发服务安装)
    if (deviceManager.IsInitialized())
    {
        deviceManager.FindDevices();
    }
}
```

### 2. 连接到设备

**文件**: `dll/project/SDPClientFramework/Sdp/ConnectionManager.cs`

#### deviceEvents_ConnectToDevice 方法 (Line 1298-1326)
```csharp
private void deviceEvents_ConnectToDevice(object sender, DeviceEventArgs e)
{
    UserPreferenceModel userPreferences = SdpApp.ModelManager.SettingsModel.UserPreferences;
    Device deviceByName = this.GetDeviceByName(e.LookupName);
    if (deviceByName != null)
    {
        string text = userPreferences.RetrieveSetting(
            UserPreferenceModel.UserPreference.ConnectionTimeout);
        string text2 = userPreferences.RetrieveSetting(
            UserPreferenceModel.UserPreference.BaseNetworkPort);
        
        // 调用 Device.Connect() - 这会触发 SDPCore 的服务安装
        if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
        {
            deviceByName.Connect(num, num2);
        }
        // ...
    }
}
```

#### deviceEvents_RetryInstallOnDevice 方法 (Line 1288-1294)
```csharp
private void deviceEvents_RetryInstallOnDevice(object sender, DeviceEventArgs e)
{
    Device deviceByName = this.GetDeviceByName(e.LookupName);
    if (deviceByName != null)
    {
        deviceByName.RetryInstall();  // 重试安装
    }
}
```

### 3. 设备状态处理

**文件**: `dll/project/SDPClientFramework/Sdp/ConnectionController.cs`

#### InvalidateDeviceState 方法 (Line 60-112)
```csharp
public void InvalidateDeviceState(string name, string guid, bool fromDevice = false)
{
    // ...
    switch (internalDeviceDelegate.CurrentState)
    {
        case DeviceConnectionState.Scanning:
        case DeviceConnectionState.Discovered:
        case DeviceConnectionState.Installing:
            deviceState = DeviceState.Installing;
            goto IL_00BD;
        case DeviceConnectionState.InstallFailed:
            deviceState = DeviceState.InstallFailed;
            goto IL_00BD;
        case DeviceConnectionState.Ready:
            deviceState = DeviceState.Ready;
            goto IL_00BD;
        // ...
    }
}
```

---

## SDPCoreWrapper 中的关键 API

### Device.cs
```csharp
// 连接到设备（触发服务安装）
public virtual void Connect(uint timeoutSeconds, uint basePort)
{
    SDPCorePINVOKE.Device_Connect__SWIG_0(this.swigCPtr, timeoutSeconds, basePort);
}

// 重试安装
public virtual void RetryInstall()
{
    SDPCorePINVOKE.Device_RetryInstall(this.swigCPtr);
}

// 获取设备状态
public virtual DeviceConnectionState GetDeviceState()
{
    return (DeviceConnectionState)SDPCorePINVOKE.Device_GetDeviceState(this.swigCPtr);
}
```

### DeviceManager.cs
```csharp
// 设置安装超时
public bool SetInstallerTimeout(int timeout)
{
    return SDPCorePINVOKE.DeviceManager_SetInstallerTimeout(this.swigCPtr, timeout);
}

// 设置是否退出时删除服务文件
public void SetDeleteServiceFilesOnExit(bool deleteFiles)
{
    SDPCorePINVOKE.DeviceManager_SetDeleteServiceFilesOnExit(this.swigCPtr, deleteFiles);
}

// 获取环境信息（包含路径配置）
public string GetEnvironment()
{
    return SDPCorePINVOKE.DeviceManager_GetEnvironment(this.swigCPtr);
}

// 查找设备
public void FindDevices()
{
    SDPCorePINVOKE.DeviceManager_FindDevices(this.swigCPtr);
}
```

### SDPCore.cs - 关键路径常量
```csharp
public static string PLUGINS_DIR
{
    get { return SDPCorePINVOKE.PLUGINS_DIR_get(); }
}

public static string DATA_PLUGIN_DIR
{
    get { return SDPCorePINVOKE.DATA_PLUGIN_DIR_get(); }
}
```

---

## 关键发现

### 1. **服务安装是自动触发的**
- 当调用 `Device.Connect()` 时，SDPCore 会自动尝试安装服务文件
- 安装过程由 SDPCore 的 `ADBServiceManager` 处理
- 状态变化：`Discovered` → `Installing` → `Ready`

### 2. **服务文件来源**
- 服务文件应该位于：`android/arm64-v8a/` 目录
- 关键文件：
  - `sdpservice` - 主服务程序
  - `qhas_sdp` - 硬件抽象服务
  - `pluginXXX` - 各种插件
  - 共享库：`libc++_shared.so`, `libSDPCore.so` 等

### 3. **配置项**
- `DeleteServiceFilesOnExit`: 是否退出时删除服务文件
- `InstallerTimeout`: 安装超时时间（秒）
- `LinuxSSHDeployDirectory`: Linux SSH 部署目录（默认 `/data/local/tmp`）

---

## 问题根源分析

### 可能的原因：

1. **SDPCore 找不到服务文件源**
   - `GetEnvironment()` 可能返回了错误的 `PLUGINS_DIR` 路径
   - 服务文件不在期望的位置

2. **ADB 权限问题**
   - 设备可能没有足够的权限写入 `/data/local/tmp/`
   - ADB 连接可能不稳定

3. **SDPCore 配置问题**
   - 可能需要在 CLI 环境中额外配置服务文件路径
   - 环境变量可能未正确设置

### 验证步骤：

1. 检查 `DeviceManager.GetEnvironment()` 返回的路径
2. 验证 `android/arm64-v8a/` 目录是否存在且包含所有文件
3. 检查 ADB 是否有权限推送文件到设备
4. 查看 SDPCore 的详细日志（启用 DEBUG 级别）

---

## 下一步建议

1. **添加环境诊断代码**
   在 SDPCLI 的 `InitializeClient()` 中添加：
   ```csharp
   string env = DeviceManager.Get().GetEnvironment();
   Console.WriteLine($"SDPCore Environment: {env}");
   
   string pluginsDir = SDPCore.PLUGINS_DIR;
   Console.WriteLine($"PLUGINS_DIR: {pluginsDir}");
   
   string dataPluginDir = SDPCore.DATA_PLUGIN_DIR;
   Console.WriteLine($"DATA_PLUGIN_DIR: {dataPluginDir}");
   ```

2. **手动推送服务文件测试**
   ```powershell
   adb push android/arm64-v8a/sdpservice /data/local/tmp/
   adb push android/arm64-v8a/qhas_sdp /data/local/tmp/
   adb shell chmod 755 /data/local/tmp/sdpservice
   adb shell chmod 755 /data/local/tmp/qhas_sdp
   ```

3. **检查 SDPCore.dll 的工作目录**
   - SDPCore 可能期望从特定目录加载服务文件
   - 需要确认工作目录设置是否正确

