using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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
                Console.WriteLine("⚠ WARNING: SDPClient already initialized");
                return true;
            }
            
            try
            {
                Console.WriteLine("=== Initializing SDPCore Client ===");
                
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
                    Console.WriteLine($"✓ Created output directory: {settings.SessionDirectoryRootPath}");
                }
                
                // 4. 初始化 Client
                client = new Client();
                if (!client.Init(settings))
                {
                    Console.WriteLine("✗ ERROR: Client.Init() failed");
                    return false;
                }
                Console.WriteLine("✓ Client initialized");
                
                // 5. 注册客户端委托
                clientDelegate = clientCallback;
                if (clientDelegate != null)
                {
                    client.RegisterEventDelegate(clientDelegate);
                    Console.WriteLine("✓ Client delegate registered");
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
                
                Console.WriteLine("✓ SDPCore client initialized successfully\n");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Failed to initialize SDPClient: {ex.Message}");
                Console.WriteLine($"   Exception Type: {ex.GetType().Name}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
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
            
            Console.WriteLine("\n=== Shutting Down SDPCore Client ===");
            
            try
            {
                // 1. 停止后台更新线程
                StopUpdateThread();
                
                // 2. 清理 managers（顺序很重要）
                Console.WriteLine("[DEBUG] Clearing managers...");
                sessionManager = null;
                captureManager = null;
                processManager = null;
                metricManager = null;
                deviceManager = null;
                Console.WriteLine("✓ Managers cleared");
                
                // 3. 关闭 Client (WARNING: This may hang or terminate process)
                if (client != null)
                {
                    try
                    {
                        Console.WriteLine("[DEBUG] About to call client.Shutdown()...");
                        Console.WriteLine("[DEBUG] (This may take a while or cause process exit)");
                        client.Shutdown();
                        Console.WriteLine("✓ Client shut down");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠ Warning during Client.Shutdown(): {ex.Message}");
                    }
                    client = null;
                }
                
                // 4. 清理日志 sink
                if (logSink != null)
                {
                    // ConsoleLogSink 会在 Client.Shutdown() 时自动清理
                    logSink = null;
                }
                
                Console.WriteLine("✓ SDPCore client shut down successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error during shutdown: {ex.Message}");
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
                Console.WriteLine($"\n=== Connecting to Device: {serialNumber} ===");
                
                // 1. 查找设备
                Console.WriteLine("Searching for device...");
                deviceManager!.FindDevices();
                Thread.Sleep(2000); // 等待设备发现
                
                DeviceList devices = deviceManager.GetDevices();
                Console.WriteLine($"Found {devices.Count} device(s)");
                
                Device? targetDevice = null;
                foreach (Device device in devices)
                {
                    if (device != null)
                    {
                        // Device 没有 GetID() 方法，使用索引或直接比较 Name
                        string? deviceName = device.GetName();
                        Console.WriteLine($"  - {deviceName ?? "Unknown"}");
                        
                        // 使用 Name 来匹配，因为 Serial 也包含在 Name 中
                        if (deviceName != null && deviceName.Contains(serialNumber))
                        {
                            targetDevice = device;
                        }
                    }
                }
                
                if (targetDevice == null)
                {
                    Console.WriteLine($"✗ Device not found: {serialNumber}");
                    return null;
                }
                
                string foundDeviceName = targetDevice.GetName();
                Console.WriteLine($"✓ Found target device: {foundDeviceName}");
                
                // 2. 注册设备委托
                deviceDelegate = new CliDeviceDelegate();
                targetDevice.RegisterEventDelegate(deviceDelegate);
                Console.WriteLine("✓ Device delegate registered");
                
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
                
                Console.WriteLine($"✓ Successfully connected to {foundDeviceName}\n");
                return targetDevice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Failed to connect device: {ex.Message}");
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
                Console.WriteLine($"Disconnecting device: {deviceName ?? "Unknown"}");
                device.Disconnect();
                Console.WriteLine("✓ Device disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Warning during disconnect: {ex.Message}");
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
                    Console.WriteLine($"Using configured device: {deviceSerial}");
                    return ConnectDevice(deviceSerial!, basePort, timeoutSeconds);
                }
                
                // 未提供设备序列号，需要发现和选择设备
                Console.WriteLine("No device serial configured, searching for devices...");
                
                deviceManager!.FindDevices();
                Thread.Sleep(2000);
                
                DeviceList devices = deviceManager.GetDevices();
                Console.WriteLine($"Found {devices.Count} device(s)");
                
                if (devices.Count == 0)
                {
                    Console.WriteLine("✗ No devices found. Please connect a device via ADB.");
                    return null;
                }
                
                string selectedSerial;
                
                if (devices.Count == 1)
                {
                    // 只有一个设备，自动选择
                    Device firstDevice = devices[0];
                    string? deviceName = firstDevice.GetName();
                    selectedSerial = deviceName ?? "Unknown";
                    Console.WriteLine($"Auto-selected device: {deviceName}");
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
                    Console.WriteLine($"Selected device: {selectedSerial}");
                }
                
                // 验证选择的序列号不为空
                if (string.IsNullOrEmpty(selectedSerial))
                {
                    Console.WriteLine("✗ ERROR: Failed to get device serial number");
                    return null;
                }
                
                // 使用选择的设备序列号连接
                return ConnectDevice(selectedSerial, basePort, timeoutSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Failed to select and connect device: {ex.Message}");
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
                Console.WriteLine("\n=== Starting Application ===");
                
                // 验证设备状态
                var state = device.GetDeviceState();
                if (state != DeviceConnectionState.Connected)
                {
                    Console.WriteLine($"✗ ERROR: Device not in Connected state (current: {state})");
                    Console.WriteLine("   StartApp requires Connected state to send commands.");
                    return 0;
                }
                
                Console.WriteLine($"Device state: {state} ✓");
                
                // 获取 packageName（executablePath 包含 package/activity）
                string execPath = settings.executablePath ?? "unknown";
                Console.WriteLine($"Starting app: {execPath}");
                
                // 调用 StartApp
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Calling device.StartApp()...");
                AppStartResponse response = device.StartApp(settings);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] StartApp() returned");
                
                Console.WriteLine($"\n=== AppStartResponse ===");
                Console.WriteLine($"  result: {response.result}");
                Console.WriteLine($"  pid: {response.pid}");
                Console.WriteLine("=========================\n");
                
                if (response.result)
                {
                    if (response.pid > 0)
                    {
                        Console.WriteLine($"✓ Application started successfully (PID: {response.pid})");
                        return response.pid;
                    }
                    else
                    {
                        // StartApp succeeded but PID is 0
                        // This is NORMAL for asynchronous app launch
                        // App is starting but ProcessManager hasn't discovered it yet
                        Console.WriteLine("⚠ StartApp succeeded but PID is 0 (app starting asynchronously)");
                        Console.WriteLine("  Waiting for app to launch (3 seconds)...");
                        
                        // Wait for app to start and ProcessManager to discover it
                        Thread.Sleep(3000);
                        
                        // Extract package name from executablePath
                        string packageName = execPath.Contains("/") 
                            ? execPath.Split('/')[0] 
                            : execPath;
                        
                        Console.WriteLine($"  Attempting to get PID via ADB for package: {packageName}");
                        
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
                                Console.WriteLine($"✓ Retrieved PID via ADB: {retrievedPid}");
                                Console.WriteLine("  App started successfully (use this PID for process discovery)");
                                return retrievedPid;
                            }
                            else
                            {
                                Console.WriteLine($"⚠ Could not get PID from ADB yet (output: '{pidOutput}')");
                                Console.WriteLine("  App may still be initializing");
                                Console.WriteLine("  Returning 0 - caller should wait for OnProcessAdded callback");
                                return 0;
                            }
                        }
                        catch (Exception pidEx)
                        {
                            Console.WriteLine($"⚠ Error getting PID via ADB: {pidEx.Message}");
                            Console.WriteLine("  Returning 0 - caller should wait for OnProcessAdded callback");
                            return 0;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("✗ StartApp failed (result = false)");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Exception during StartApp: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                
                // 诊断信息
                string execPath = settings.executablePath ?? "unknown";
                Console.WriteLine("");
                Console.WriteLine("=== Debug Suggestions ===");
                Console.WriteLine("1. Monitor device logs in real-time:");
                Console.WriteLine("   adb logcat -v time | findstr /I \"SDPCore sdp SnapdragonProfiler\"");
                Console.WriteLine("");
                Console.WriteLine("2. Check if service is running on device:");
                Console.WriteLine("   adb shell ps | findstr sdp");
                Console.WriteLine("");
                Console.WriteLine("3. Try manual launch to test app:");
                Console.WriteLine($"   adb shell am start {execPath}");
                Console.WriteLine("=========================\n");
                
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
                Console.WriteLine($"Stopping app: {packageName}");
                bool result = device.StopApp(packageName);
                
                if (result)
                {
                    Console.WriteLine("✓ App stopped");
                }
                else
                {
                    Console.WriteLine("⚠ StopApp returned false");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Warning during StopApp: {ex.Message}");
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
                Console.WriteLine("\n=== Starting Capture ===");
                
                // 创建捕获（返回 captureId）
                uint captureId = captureManager?.CreateCapture(captureType) ?? 0;
                if (captureId == 0)
                {
                    Console.WriteLine("✗ Failed to create capture");
                    return null;
                }
                
                // 获取 Capture 对象
                Capture? capture = captureManager?.GetCapture(captureId);
                if (capture == null)
                {
                    Console.WriteLine("✗ Failed to get capture object");
                    return null;
                }
                
                Console.WriteLine($"✓ Created capture (ID: {captureId})");
                
                // 启动捕获（直接传入 settings）
                if (!capture.Start(settings))
                {
                    Console.WriteLine("✗ Failed to start capture");
                    return null;
                }
                
                Console.WriteLine("✓ Capture started");
                return capture;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Failed to start capture: {ex.Message}");
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
                Console.WriteLine("⚠ WARNING: No capture complete event configured");
                return false;
            }
            
            Console.WriteLine($"Waiting for capture to complete (timeout: {timeoutSeconds}s)...");
            
            bool completed = captureCompleteEvent.WaitOne(timeoutSeconds * 1000);
            
            if (completed)
            {
                Console.WriteLine("✓ Capture completed successfully");
            }
            else
            {
                Console.WriteLine($"⚠ Capture did not complete within {timeoutSeconds} seconds");
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
                Console.WriteLine("Stopping capture...");
                capture.Stop();
                Console.WriteLine("✓ Capture stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Warning during capture stop: {ex.Message}");
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
                
                Console.WriteLine($"✓ SDPCore logging enabled (level: {levelName})");
                
                if (logLevel == LogLevel.LOG_DEBUG)
                {
                    Console.WriteLine("  → DEBUG level captures all SDPCore internal messages");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ WARNING: Could not setup logging: {ex.Message}");
            }
        }
        
        private bool InitializeManagers()
        {
            try
            {
                Console.WriteLine("\n=== Initializing Managers ===");
                
                // MetricManager（Snapshot 捕获需要）
                try
                {
                    metricManager = MetricManager.Get();
                    Console.WriteLine("✓ MetricManager initialized");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ WARNING: MetricManager init failed: {ex.Message}");
                    Console.WriteLine("  Snapshot capture may not work without MetricManager");
                }
                
                // 其他必需的 managers
                NetworkManager.Get();
                processManager = ProcessManager.Get();
                Console.WriteLine("✓ NetworkManager & ProcessManager initialized");
                
                // DeviceManager
                deviceManager = DeviceManager.Get();
                if (deviceManager == null)
                {
                    Console.WriteLine("✗ ERROR: Failed to get DeviceManager");
                    return false;
                }
                Console.WriteLine("✓ DeviceManager initialized");
                
                // SessionManager（Client.Init 已创建）
                sessionManager = SessionManager.Get();
                Console.WriteLine("✓ SessionManager obtained");
                
                // CaptureManager
                captureManager = CaptureManager.Get();
                Console.WriteLine("✓ CaptureManager obtained");
                
                Console.WriteLine("✓ All managers initialized\n");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: Failed to initialize managers: {ex.Message}");
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
                Console.WriteLine("✓ DeviceManager configured");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ WARNING: Failed to configure DeviceManager: {ex.Message}");
            }
        }
        
        private bool WaitForDeviceReady(Device device, int maxWaitSeconds)
        {
            Console.WriteLine("\nWaiting for device to reach Ready state...");
            
            DeviceConnectionState currentState = device.GetDeviceState();
            Console.WriteLine($"Initial state: {currentState}");
            
            if (currentState == DeviceConnectionState.Connected)
            {
                Console.WriteLine("✓ Device already in Connected state");
                return true;
            }
            
            for (int i = 0; i < maxWaitSeconds; i++)
            {
                currentState = device.GetDeviceState();
                
                if (currentState == DeviceConnectionState.Ready)
                {
                    Console.WriteLine($"✓ Device reached Ready state after {i+1} seconds");
                    
                    // 给服务额外时间初始化
                    Console.WriteLine("Waiting 3 seconds for service initialization...");
                    Thread.Sleep(3000);
                    return true;
                }
                else if (currentState == DeviceConnectionState.InstallFailed)
                {
                    Console.WriteLine("✗ Device installation failed!");
                    return false;
                }
                else if (currentState == DeviceConnectionState.Connected)
                {
                    Console.WriteLine("✓ Device already connected");
                    return true;
                }
                
                // 每 5 秒显示进度
                if ((i + 1) % 5 == 0)
                {
                    Console.WriteLine($"[{i+1}/{maxWaitSeconds}] Still waiting... Current state: {currentState}");
                }
                
                Thread.Sleep(1000);
            }
            
            Console.WriteLine($"⚠ WARNING: Device did not reach Ready state within {maxWaitSeconds} seconds!");
            Console.WriteLine($"   Final state: {currentState}");
            return false;
        }
        
        private bool EstablishNetworkConnection(Device device, uint basePort, int timeoutSeconds)
        {
            Console.WriteLine($"\n=== Establishing Network Connection ===");
            Console.WriteLine($"Using base port: {basePort}");
            
            // 重置日志捕获的端口
            ConsoleLogSink.ResetCapturedPort();
            
            // 调用 Connect
            Console.WriteLine($"Calling device.Connect({timeoutSeconds}, {basePort})...");
            device.Connect((uint)timeoutSeconds, basePort);
            
            // 等待连接建立
            Console.WriteLine("\nWaiting for connection state: Connected...");
            
            for (int i = 0; i < timeoutSeconds; i++)
            {
                Thread.Sleep(1000);
                DeviceConnectionState state = device.GetDeviceState();
                Console.WriteLine($"[{i+1}/{timeoutSeconds}] Connection state: {state}");
                
                if (state == DeviceConnectionState.Connected)
                {
                    Console.WriteLine($"✓ Network connection established");
                    return true;
                }
                else if (state == DeviceConnectionState.InstallFailed || state == DeviceConnectionState.Unknown)
                {
                    Console.WriteLine($"✗ Connection failed with state: {state}");
                    return false;
                }
            }
            
            DeviceConnectionState finalState = device.GetDeviceState();
            Console.WriteLine($"\n⚠ WARNING: Failed to reach Connected state!");
            Console.WriteLine($"   Final state: {finalState}");
            Console.WriteLine("\n   Possible reasons:");
            Console.WriteLine("   1. Network connection to device service failed");
            Console.WriteLine("   2. Device service not responding on network port");
            Console.WriteLine("   3. Firewall blocking connection");
            
            return false;
        }
        
        // ==================== 后台更新线程 ====================
        private void StartUpdateThread()
        {
            if (updateThread != null && updateThread.IsAlive)
            {
                Console.WriteLine("⚠ WARNING: Update thread already running");
                return;
            }
            
            Console.WriteLine("Starting SDPCore event processing thread...");
            isRunning = true;
            
            updateThread = new Thread(UpdateThreadProc)
            {
                IsBackground = true,
                Name = "SDPCore-UpdateThread"
            };
            
            updateThread.Start();
            Console.WriteLine("✓ SDPCore event processing thread started");
        }
        
        private void StopUpdateThread()
        {
            if (updateThread == null || !updateThread.IsAlive)
            {
                return;
            }
            
            Console.WriteLine("Stopping SDPCore event processing thread...");
            isRunning = false;
            
            if (!updateThread.Join(2000))
            {
                Console.WriteLine("⚠ Warning: Update thread did not stop gracefully");
            }
            else
            {
                Console.WriteLine("✓ SDPCore event processing thread stopped");
            }
            
            updateThread = null;
        }
        
        private void UpdateThreadProc()
        {
            Console.WriteLine("[UpdateThread] Started");
            
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
                    Console.WriteLine($"[UpdateThread] Exception: {ex.Message}");
                }
            }
            
            Console.WriteLine("[UpdateThread] Stopped");
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
