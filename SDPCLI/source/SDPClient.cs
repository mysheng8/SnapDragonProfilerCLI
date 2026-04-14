using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// SDPCore 客户端封装类
    /// 管理 SDK 生命周期、设备连接、Manager 访问
    /// 职责：封装所有 SDPCore 交互，提供高层次 API
    /// </summary>
    public class SDPClient : IDisposable
    {
        // ==================== 私有成员 ====================
        private Client? client;
        private Thread? updateThread;
        private bool isRunning = false;
        private bool isDisposed = false;
        private int updateCallCount = 0;
        
        // Manager 引用
        private DeviceManager? deviceManager;
        private ProcessManager? processManager;
        private SessionManager? sessionManager;
        private CaptureManager? captureManager;
        private MetricManager? metricManager;
        
        // 事件委托
        private ClientDelegate? clientDelegate;
        private DeviceDelegate? deviceDelegate;
        
        // 日志
        private ConsoleLogSink? logSink;
        
        // 事件同步
        private ManualResetEvent? captureCompleteEvent;
        
        // ==================== 公共属性 ====================
        /// <summary>SDK 是否已初始化</summary>
        public bool IsInitialized => client != null;
        
        /// <summary>后台更新线程是否运行中</summary>
        public bool IsUpdateThreadRunning => isRunning;
        
        /// <summary>Update() 调用次数（诊断用）</summary>
        public int UpdateCallCount => updateCallCount;
        
        /// <summary>底层 SDPCore Client 实例（用于高级操作）</summary>
        public Client Client => client 
            ?? throw new InvalidOperationException("Client not initialized");
        
        /// <summary>设备管理器</summary>
        public DeviceManager DeviceManager => deviceManager 
            ?? throw new InvalidOperationException("Client not initialized");
        
        /// <summary>进程管理器</summary>
        public ProcessManager ProcessManager => processManager 
            ?? throw new InvalidOperationException("ProcessManager not available");
        
        /// <summary>会话管理器</summary>
        public SessionManager SessionManager => sessionManager 
            ?? throw new InvalidOperationException("SessionManager not available");
        
        /// <summary>捕获管理器</summary>
        public CaptureManager CaptureManager => captureManager 
            ?? throw new InvalidOperationException("CaptureManager not available");
        
        /// <summary>指标管理器</summary>
        public MetricManager MetricManager => metricManager 
            ?? throw new InvalidOperationException("MetricManager not available");
        
        // ==================== 初始化/清理 ====================
        /// <summary>
        /// 初始化 SDPCore 客户端
        /// </summary>
        /// <param name="settings">会话设置</param>
        /// <param name="clientCallback">客户端事件回调</param>
        /// <param name="enableConsoleLog">是否启用控制台日志</param>
        /// <param name="logLevel">日志级别</param>
        /// <returns>初始化是否成功</returns>
        public bool Initialize(SessionSettings settings, 
                              ClientDelegate? clientCallback = null,
                              bool enableConsoleLog = true,
                              LogLevel logLevel = LogLevel.LOG_DEBUG)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(SDPClient));
            }
            
            if (IsInitialized)
            {
                AppLogger.Warn("SDPClient", "SDPClient already initialized");
                return true;
            }
            
            try
            {
                AppLogger.Info("SDPClient", "=== Initializing SDPCore Client ===");
                
                // 1. 配置 DLL 搜索路径（Windows 需要）
                Utility.ConfigureNativeDllSearchPaths();
                
                // 2. 设置日志系统
                if (enableConsoleLog)
                {
                    SetupLogging(logLevel);
                }
                
                // 3. 创建测试输出目录
                if (!Directory.Exists(settings.SessionDirectoryRootPath))
                {
                    Directory.CreateDirectory(settings.SessionDirectoryRootPath);
                    AppLogger.Info("SDPClient", $"Created output directory: {settings.SessionDirectoryRootPath}");
                }
                
                // 4. 初始化 Client
                client = new Client();
                if (!client.Init(settings))
                {
                    AppLogger.Error("SDPClient", "Client.Init() failed");
                    return false;
                }
                AppLogger.Info("SDPClient", "Client initialized");
                
                // 5. 注册客户端委托
                clientDelegate = clientCallback;
                if (clientDelegate != null)
                {
                    client.RegisterEventDelegate(clientDelegate);
                    AppLogger.Info("SDPClient", "Client delegate registered");
                }
                
                // 6. 初始化各个 Manager
                if (!InitializeManagers())
                {
                    return false;
                }
                
                // 7. 配置 DeviceManager
                ConfigureDeviceManager();
                
                // 8. 启动后台更新线程
                StartUpdateThread();
                
                AppLogger.Info("SDPClient", "SDPCore client initialized successfully");
                return true;
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Failed to initialize SDPClient: {ex.Message}");
                AppLogger.Debug("SDPClient", $"Exception Type: {ex.GetType().Name}");
                
                if (ex.InnerException != null)
                {
                    AppLogger.Debug("SDPClient", $"Inner Exception: {ex.InnerException.Message}");
                }
                
                return false;
            }
        }
        
        /// <summary>
        /// 关闭 SDPCore 客户端并清理资源
        /// </summary>
        public void Shutdown()
        {
            if (isDisposed || !IsInitialized)
            {
                return;
            }
            
            AppLogger.Info("SDPClient", "=== Shutting Down SDPCore Client ===");
            
            try
            {
                // 1. 停止后台更新线程
                StopUpdateThread();
                
                // 2. 清理 managers（顺序很重要）
                AppLogger.Debug("SDPClient", "Clearing managers...");
                sessionManager = null;
                captureManager = null;
                processManager = null;
                metricManager = null;
                deviceManager = null;
                AppLogger.Info("SDPClient", "Managers cleared");
                
                // 3. 关闭 Client (WARNING: This may hang or terminate process)
                if (client != null)
                {
                    try
                    {
                        AppLogger.Debug("SDPClient", "About to call client.Shutdown()...");
                        AppLogger.Debug("SDPClient", "(This may take a while or cause process exit)");
                        client.Shutdown();
                        AppLogger.Info("SDPClient", "Client shut down");
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Warn("SDPClient", $"Warning during Client.Shutdown(): {ex.Message}");
                    }
                    client = null;
                }
                
                // 4. 清理日志 sink
                if (logSink != null)
                {
                    // ConsoleLogSink 会在 Client.Shutdown() 时自动清理
                    logSink = null;
                }
                
                AppLogger.Info("SDPClient", "SDPCore client shut down successfully");
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Error during shutdown: {ex.Message}");
            }
        }
        
        // ==================== 设备操作 ====================
        /// <summary>
        /// 连接到指定设备
        /// </summary>
        /// <param name="serialNumber">设备序列号</param>
        /// <param name="basePort">基础端口号（默认 6500）</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns>已连接的设备对象，失败返回 null</returns>
        public Device? ConnectDevice(string serialNumber, uint basePort = 6500, int timeoutSeconds = 30)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("SDPClient not initialized");
            }
            
            try
            {
                AppLogger.Info("SDPClient", $"=== Connecting to Device: {serialNumber} ===");
                
                // 1. 查找设备
                AppLogger.Info("SDPClient", "Searching for device...");
                deviceManager!.FindDevices();
                Thread.Sleep(2000); // 等待设备发现
                
                DeviceList devices = deviceManager.GetDevices();
                AppLogger.Info("SDPClient", $"Found {devices.Count} device(s)");
                
                Device? targetDevice = null;
                foreach (Device device in devices)
                {
                    if (device != null)
                    {
                        // Device 没有 GetID() 方法，使用索引或直接比较 Name
                        string? deviceName = device.GetName();
                        AppLogger.Debug("SDPClient", $"  - {deviceName ?? "Unknown"}");
                        
                        // 使用 Name 来匹配，因为 Serial 也包含在 Name 中
                        if (deviceName != null && deviceName.Contains(serialNumber))
                        {
                            targetDevice = device;
                        }
                    }
                }
                
                if (targetDevice == null)
                {
                    AppLogger.Error("SDPClient", $"Device not found: {serialNumber}");
                    return null;
                }
                
                string foundDeviceName = targetDevice.GetName();
                AppLogger.Info("SDPClient", $"Found target device: {foundDeviceName}");
                
                // 2. 注册设备委托
                deviceDelegate = new CliDeviceDelegate();
                targetDevice.RegisterEventDelegate(deviceDelegate);
                AppLogger.Info("SDPClient", "Device delegate registered");
                
                // 3. 等待设备进入 Ready 状态
                if (!WaitForDeviceReady(targetDevice, 60))
                {
                    return null;
                }
                
                // 4. 建立网络连接
                if (!EstablishNetworkConnection(targetDevice, basePort, timeoutSeconds))
                {
                    return null;
                }
                
                // 5. 等待 native SDK 触发 OnClientConnected（会自动调用一次，无需手动调用）
                Thread.Sleep(1000);
                
                AppLogger.Info("SDPClient", $"Successfully connected to {foundDeviceName}");
                return targetDevice;
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Failed to connect device: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 断开设备连接
        /// </summary>
        public void DisconnectDevice(Device? device)
        {
            if (device == null)
            {
                return;
            }
            
            try
            {
                string? deviceName = device.GetName();
                AppLogger.Info("SDPClient", $"Disconnecting device: {deviceName ?? "Unknown"}");
                device.Disconnect();
                AppLogger.Info("SDPClient", "Device disconnected");
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Warning during disconnect: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 设备选择和连接（自动发现、选择、连接）
        /// </summary>
        /// <param name="deviceSerial">设备序列号（可选，为null时自动发现和选择）</param>
        /// <param name="basePort">基础端口号</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <param name="readLineFunc">用户输入函数（用于多设备选择）</param>
        /// <returns>已连接的设备对象，失败返回 null</returns>
        public Device? SelectAndConnectDevice(
            string? deviceSerial, 
            uint basePort = 6500, 
            int timeoutSeconds = 30,
            Func<string, string?>? readLineFunc = null)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("SDPClient not initialized");
            }
            
            try
            {
                // 如果已提供设备序列号，直接连接
                if (!string.IsNullOrEmpty(deviceSerial))
                {
                    AppLogger.Info("SDPClient", $"Using configured device: {deviceSerial}");
                    return ConnectDevice(deviceSerial!, basePort, timeoutSeconds);
                }
                
                // 未提供设备序列号，需要发现和选择设备
                AppLogger.Info("SDPClient", "No device serial configured, searching for devices...");
                
                deviceManager!.FindDevices();
                Thread.Sleep(2000);
                
                DeviceList devices = deviceManager.GetDevices();
                AppLogger.Info("SDPClient", $"Found {devices.Count} device(s)");
                
                if (devices.Count == 0)
                {
                    AppLogger.Error("SDPClient", "No devices found. Please connect a device via ADB.");
                    return null;
                }
                
                string selectedSerial;
                
                if (devices.Count == 1)
                {
                    // 只有一个设备，自动选择
                    Device firstDevice = devices[0];
                    string? deviceName = firstDevice.GetName();
                    selectedSerial = deviceName ?? "Unknown";
                    AppLogger.Info("SDPClient", $"Auto-selected device: {deviceName}");
                }
                else
                {
                    // 多个设备，列出供用户选择
                    Console.WriteLine("\nAvailable devices:");
                    for (int i = 0; i < devices.Count; i++)
                    {
                        Device device = devices[i];
                        string? name = device?.GetName();
                        DeviceConnectionState state = device?.GetDeviceState() ?? DeviceConnectionState.Unknown;
                        Console.WriteLine($"  [{i}] {name ?? "Unknown"} (State: {state})");
                    }
                    
                    Console.Write("\nSelect device [0]: ");
                    
                    // 使用提供的输入函数或默认值
                    string? input = readLineFunc?.Invoke("0") ?? "0";
                    
                    if (!int.TryParse(input, out int selectedIndex) || 
                        selectedIndex < 0 || selectedIndex >= devices.Count)
                    {
                        selectedIndex = 0;
                    }
                    
                    Device selectedDevice = devices[selectedIndex];
                    selectedSerial = selectedDevice.GetName() ?? "Unknown";
                    AppLogger.Info("SDPClient", $"Selected device: {selectedSerial}");
                }
                
                // 验证选择的序列号不为空
                if (string.IsNullOrEmpty(selectedSerial))
                {
                    AppLogger.Error("SDPClient", "Failed to get device serial number");
                    return null;
                }
                
                // 使用选择的设备序列号连接
                return ConnectDevice(selectedSerial, basePort, timeoutSeconds);
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Failed to select and connect device: {ex.Message}");
                return null;
            }
        }
        
        // ==================== 应用操作 ====================
        /// <summary>
        /// 在设备上启动应用
        /// </summary>
        /// <param name="device">目标设备</param>
        /// <param name="settings">应用启动设置</param>
        /// <returns>应用进程 PID，失败返回 0</returns>
        public uint StartApplication(Device? device, AppStartSettings settings)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            
            try
            {
                AppLogger.Info("SDPClient", "=== Starting Application ===");
                
                // 验证设备状态
                var state = device.GetDeviceState();
                if (state != DeviceConnectionState.Connected)
                {
                    AppLogger.Error("SDPClient", $"Device not in Connected state (current: {state})");
                    AppLogger.Debug("SDPClient", "StartApp requires Connected state to send commands.");
                    return 0;
                }
                
                AppLogger.Info("SDPClient", $"Device state: {state}");
                
                // 获取 packageName（executablePath 包含 package/activity）
                string execPath = settings.executablePath ?? "unknown";
                AppLogger.Info("SDPClient", $"Starting app: {execPath}");
                
                // 调用 StartApp
                AppLogger.Debug("SDPClient", $"[{DateTime.Now:HH:mm:ss.fff}] Calling device.StartApp()...");
                AppStartResponse response = device.StartApp(settings);
                AppLogger.Debug("SDPClient", $"[{DateTime.Now:HH:mm:ss.fff}] StartApp() returned");
                
                AppLogger.Debug("SDPClient", $"AppStartResponse: result={response.result}, pid={response.pid}");
                
                if (response.result)
                {
                    if (response.pid > 0)
                    {
                        AppLogger.Info("SDPClient", $"Application started successfully (PID: {response.pid})");
                        return response.pid;
                    }
                    else
                    {
                        // StartApp succeeded but PID is 0
                        // This is NORMAL for asynchronous app launch
                        // App is starting but ProcessManager hasn't discovered it yet
                        AppLogger.Warn("SDPClient", "StartApp succeeded but PID is 0 (app starting asynchronously)");
                        AppLogger.Debug("SDPClient", "Waiting for app to launch (3 seconds)...");
                        
                        // Wait for app to start and ProcessManager to discover it
                        Thread.Sleep(3000);
                        
                        // Extract package name from executablePath
                        string packageName = execPath.Contains("/") 
                            ? execPath.Split('/')[0] 
                            : execPath;
                        
                        AppLogger.Debug("SDPClient", $"Attempting to get PID via ADB for package: {packageName}");
                        
                        // Try to get PID via adb
                        try
                        {
                            var pidProcess = new System.Diagnostics.Process
                            {
                                StartInfo = new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = "adb",
                                    Arguments = $"shell pidof {packageName}",
                                    RedirectStandardOutput = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                }
                            };
                            pidProcess.Start();
                            string pidOutput = pidProcess.StandardOutput.ReadToEnd().Trim();
                            pidProcess.WaitForExit();
                            
                            if (uint.TryParse(pidOutput, out uint retrievedPid))
                            {
                                AppLogger.Info("SDPClient", $"Retrieved PID via ADB: {retrievedPid}");
                                AppLogger.Debug("SDPClient", "App started successfully (use this PID for process discovery)");
                                return retrievedPid;
                            }
                            else
                            {
                                AppLogger.Warn("SDPClient", $"Could not get PID from ADB yet (output: '{pidOutput}')");
                                AppLogger.Debug("SDPClient", "App may still be initializing");
                                AppLogger.Debug("SDPClient", "Returning 0 - caller should wait for OnProcessAdded callback");
                                return 0;
                            }
                        }
                        catch (Exception pidEx)
                        {
                            AppLogger.Warn("SDPClient", $"Error getting PID via ADB: {pidEx.Message}");
                            AppLogger.Debug("SDPClient", "Returning 0 - caller should wait for OnProcessAdded callback");
                            return 0;
                        }
                    }
                }
                else
                {
                    AppLogger.Error("SDPClient", "StartApp failed (result = false)");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Exception during StartApp: {ex.Message}");
                AppLogger.Debug("SDPClient", $"Stack trace: {ex.StackTrace}");
                
                // 诊断信息
                string execPath = settings.executablePath ?? "unknown";
                AppLogger.Debug("SDPClient", "=== Debug Suggestions ===");
                AppLogger.Debug("SDPClient", "1. Monitor device logs in real-time:");
                AppLogger.Debug("SDPClient", "   adb logcat -v time | findstr /I \"SDPCore sdp SnapdragonProfiler\"");
                AppLogger.Debug("SDPClient", "2. Check if service is running on device:");
                AppLogger.Debug("SDPClient", "   adb shell ps | findstr sdp");
                AppLogger.Debug("SDPClient", "3. Try manual launch to test app:");
                AppLogger.Debug("SDPClient", $"   adb shell am start {execPath}");
                
                return 0;
            }
        }
        
        /// <summary>
        /// 停止应用
        /// </summary>
        public bool StopApplication(Device? device, string packageName)
        {
            if (device == null)
            {
                return false;
            }
            
            try
            {
                AppLogger.Info("SDPClient", $"Stopping app: {packageName}");
                bool result = device.StopApp(packageName);
                
                if (result)
                {
                    AppLogger.Info("SDPClient", "App stopped");
                }
                else
                {
                    AppLogger.Warn("SDPClient", "StopApp returned false");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Warning during StopApp: {ex.Message}");
                return false;
            }
        }
        
        // ==================== 捕获操作 ====================
        /// <summary>
        /// 开始性能捕获
        /// </summary>
        /// <param name="device">目标设备</param>
        /// <param name="settings">捕获设置</param>
        /// <param name="captureType">捕获类型（Snapshot=4, Trace=2, etc.）</param>
        /// <returns>Capture 对象，失败返回 null</returns>
        public Capture? StartCapture(Device? device, CaptureSettings settings, uint captureType = 4)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            
            try
            {
                AppLogger.Info("SDPClient", "=== Starting Capture ===");
                
                // 创建捕获（返回 captureId）
                uint captureId = captureManager?.CreateCapture(captureType) ?? 0;
                if (captureId == 0)
                {
                    AppLogger.Error("SDPClient", "Failed to create capture");
                    return null;
                }
                
                // 获取 Capture 对象
                Capture? capture = captureManager?.GetCapture(captureId);
                if (capture == null)
                {
                    AppLogger.Error("SDPClient", "Failed to get capture object");
                    return null;
                }
                
                AppLogger.Info("SDPClient", $"Created capture (ID: {captureId})");
                
                // 启动捕获（直接传入 settings）
                if (!capture.Start(settings))
                {
                    AppLogger.Error("SDPClient", "Failed to start capture");
                    return null;
                }
                
                AppLogger.Info("SDPClient", "Capture started");
                return capture;
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Failed to start capture: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 等待捕获完成
        /// </summary>
        /// <param name="capture">捕获对象</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns>是否成功完成</returns>
        public bool WaitForCaptureCompletion(Capture capture, int timeoutSeconds = 60)
        {
            if (capture == null)
            {
                return false;
            }
            
            if (captureCompleteEvent == null)
            {
                AppLogger.Warn("SDPClient", "No capture complete event configured");
                return false;
            }
            
            AppLogger.Info("SDPClient", $"Waiting for capture to complete (timeout: {timeoutSeconds}s)...");
            
            bool completed = captureCompleteEvent.WaitOne(timeoutSeconds * 1000);
            
            if (completed)
            {
                AppLogger.Info("SDPClient", "Capture completed successfully");
            }
            else
            {
                AppLogger.Warn("SDPClient", $"Capture did not complete within {timeoutSeconds} seconds");
            }
            
            return completed;
        }
        
        /// <summary>
        /// 停止捕获
        /// </summary>
        public void StopCapture(Capture capture)
        {
            if (capture == null)
            {
                return;
            }
            
            try
            {
                AppLogger.Info("SDPClient", "Stopping capture...");
                capture.Stop();
                AppLogger.Info("SDPClient", "Capture stopped");
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Warning during capture stop: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 设置捕获完成事件（用于等待捕获完成）
        /// </summary>
        public void SetCaptureCompleteEvent(ManualResetEvent evt)
        {
            captureCompleteEvent = evt;
            
            // 如果已有 clientDelegate 是 CliClientDelegate，更新它的事件
            if (clientDelegate != null && clientDelegate.GetType() == typeof(CliClientDelegate))
            {
                ((CliClientDelegate)clientDelegate).SetCaptureCompleteEvent(evt);
            }
        }
        
        // ==================== 私有辅助方法 ====================
        private void SetupLogging(LogLevel logLevel)
        {
            try
            {
                Logger logger = Logger.Get();
                logSink = new ConsoleLogSink();
                logSink.Init();
                logSink.SetLevel(logLevel);
                logger.AddSink(logSink);
                
                string levelName = logLevel switch
                {
                    LogLevel.LOG_DEBUG => "DEBUG",
                    LogLevel.LOG_INFO => "INFO",
                    LogLevel.LOG_WARN => "WARN",
                    LogLevel.LOG_ERROR => "ERROR",
                    LogLevel.LOG_OFF => "OFF",
                    _ => "UNKNOWN"
                };
                
                AppLogger.Info("SDPClient", $"SDPCore logging enabled (level: {levelName})");
                
                if (logLevel == LogLevel.LOG_DEBUG)
                {
                    AppLogger.Debug("SDPClient", "DEBUG level captures all SDPCore internal messages");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Could not setup logging: {ex.Message}");
            }
        }
        
        private bool InitializeManagers()
        {
            try
            {
                AppLogger.Info("SDPClient", "=== Initializing Managers ===");
                
                // MetricManager（Snapshot 捕获需要）
                try
                {
                    metricManager = MetricManager.Get();
                    AppLogger.Info("SDPClient", "MetricManager initialized");
                }
                catch (Exception ex)
                {
                    AppLogger.Warn("SDPClient", $"MetricManager init failed: {ex.Message}");
                    AppLogger.Warn("SDPClient", "Snapshot capture may not work without MetricManager");
                }
                
                // 其他必需的 managers
                NetworkManager.Get();
                processManager = ProcessManager.Get();
                AppLogger.Info("SDPClient", "NetworkManager & ProcessManager initialized");
                
                // DeviceManager
                deviceManager = DeviceManager.Get();
                if (deviceManager == null)
                {
                    AppLogger.Error("SDPClient", "Failed to get DeviceManager");
                    return false;
                }
                AppLogger.Info("SDPClient", "DeviceManager initialized");
                
                // SessionManager（Client.Init 已创建）
                sessionManager = SessionManager.Get();
                AppLogger.Info("SDPClient", "SessionManager obtained");
                
                // CaptureManager
                captureManager = CaptureManager.Get();
                AppLogger.Info("SDPClient", "CaptureManager obtained");
                
                AppLogger.Info("SDPClient", "All managers initialized");
                return true;
            }
            catch (Exception ex)
            {
                AppLogger.Error("SDPClient", $"Failed to initialize managers: {ex.Message}");
                return false;
            }
        }
        
        private void ConfigureDeviceManager()
        {
            if (deviceManager == null)
            {
                return;
            }
            
            try
            {
                deviceManager.SetDeleteServiceFilesOnExit(false);
                deviceManager.SetInstallerTimeout(120);
                AppLogger.Info("SDPClient", "DeviceManager configured");
            }
            catch (Exception ex)
            {
                AppLogger.Warn("SDPClient", $"Failed to configure DeviceManager: {ex.Message}");
            }
        }
        
        private bool WaitForDeviceReady(Device device, int maxWaitSeconds)
        {
            AppLogger.Info("SDPClient", "Waiting for device to reach Ready state...");
            
            DeviceConnectionState currentState = device.GetDeviceState();
            AppLogger.Info("SDPClient", $"Initial state: {currentState}");
            
            if (currentState == DeviceConnectionState.Connected)
            {
                AppLogger.Info("SDPClient", "Device already in Connected state");
                return true;
            }
            
            for (int i = 0; i < maxWaitSeconds; i++)
            {
                currentState = device.GetDeviceState();
                
                if (currentState == DeviceConnectionState.Ready)
                {
                    AppLogger.Info("SDPClient", $"Device reached Ready state after {i+1} seconds");
                    
                    // 给服务额外时间初始化
                    AppLogger.Debug("SDPClient", "Waiting 3 seconds for service initialization...");
                    Thread.Sleep(3000);
                    return true;
                }
                else if (currentState == DeviceConnectionState.InstallFailed)
                {
                    AppLogger.Error("SDPClient", "Device installation failed!");
                    return false;
                }
                else if (currentState == DeviceConnectionState.Connected)
                {
                    AppLogger.Info("SDPClient", "Device already connected");
                    return true;
                }
                
                // 每 5 秒显示进度
                if ((i + 1) % 5 == 0)
                {
                    AppLogger.Debug("SDPClient", $"[{i+1}/{maxWaitSeconds}] Still waiting... Current state: {currentState}");
                }
                
                Thread.Sleep(1000);
            }
            
            AppLogger.Warn("SDPClient", $"Device did not reach Ready state within {maxWaitSeconds} seconds!");
            AppLogger.Warn("SDPClient", $"Final state: {currentState}");
            return false;
        }
        
        private bool EstablishNetworkConnection(Device device, uint basePort, int timeoutSeconds)
        {
            AppLogger.Info("SDPClient", "=== Establishing Network Connection ===");
            AppLogger.Info("SDPClient", $"Using base port: {basePort}");
            
            // 重置日志捕获的端口
            ConsoleLogSink.ResetCapturedPort();
            
            // 调用 Connect
            AppLogger.Debug("SDPClient", $"Calling device.Connect({timeoutSeconds}, {basePort})...");
            device.Connect((uint)timeoutSeconds, basePort);
            
            // 等待连接建立
            AppLogger.Info("SDPClient", "Waiting for connection state: Connected...");
            
            for (int i = 0; i < timeoutSeconds; i++)
            {
                Thread.Sleep(1000);
                DeviceConnectionState state = device.GetDeviceState();
                AppLogger.Debug("SDPClient", $"[{i+1}/{timeoutSeconds}] Connection state: {state}");
                
                if (state == DeviceConnectionState.Connected)
                {
                    AppLogger.Info("SDPClient", "Network connection established");
                    return true;
                }
                else if (state == DeviceConnectionState.InstallFailed || state == DeviceConnectionState.Unknown)
                {
                    AppLogger.Error("SDPClient", $"Connection failed with state: {state}");
                    return false;
                }
            }
            
            DeviceConnectionState finalState = device.GetDeviceState();
            AppLogger.Warn("SDPClient", "Failed to reach Connected state!");
            AppLogger.Warn("SDPClient", $"Final state: {finalState}");
            AppLogger.Warn("SDPClient", "Possible reasons: 1. Network connection to device service failed  2. Device service not responding on network port  3. Firewall blocking connection");
            
            return false;
        }
        
        // ==================== 后台更新线程 ====================
        private void StartUpdateThread()
        {
            if (updateThread != null && updateThread.IsAlive)
            {
                AppLogger.Warn("SDPClient", "Update thread already running");
                return;
            }
            
            AppLogger.Info("SDPClient", "Starting SDPCore event processing thread...");
            isRunning = true;
            
            updateThread = new Thread(UpdateThreadProc)
            {
                IsBackground = true,
                Name = "SDPCore-UpdateThread"
            };
            
            updateThread.Start();
            AppLogger.Info("SDPClient", "SDPCore event processing thread started");
        }
        
        private void StopUpdateThread()
        {
            if (updateThread == null || !updateThread.IsAlive)
            {
                return;
            }
            
            AppLogger.Info("SDPClient", "Stopping SDPCore event processing thread...");
            isRunning = false;
            
            if (!updateThread.Join(2000))
            {
                AppLogger.Warn("SDPClient", "Update thread did not stop gracefully");
            }
            else
            {
                AppLogger.Info("SDPClient", "SDPCore event processing thread stopped");
            }
            
            updateThread = null;
        }
        
        private void UpdateThreadProc()
        {
            AppLogger.Debug("SDPClient", "UpdateThread started");
            
            while (isRunning)
            {
                try
                {
                    if (client != null)
                    {
                        client.Update();  // 处理 SDPCore 事件和回调
                        Interlocked.Increment(ref updateCallCount);
                    }
                    Thread.Sleep(16);  // ~60 FPS，匹配典型 GUI 消息循环频率
                }
                catch (Exception ex)
                {
                    AppLogger.Warn("SDPClient", $"UpdateThread exception: {ex.Message}");
                }
            }
            
            AppLogger.Debug("SDPClient", "UpdateThread stopped");
        }
        
        // ==================== IDisposable ====================
        public void Dispose()
        {
            if (!isDisposed)
            {
                Shutdown();
                isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
        
        ~SDPClient()
        {
            Dispose();
        }
    }
}
