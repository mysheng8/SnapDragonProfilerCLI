using System;
using System.IO;
using System.Runtime.InteropServices;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// 程序入口 - 负责环境设置和DLL路径配置
    /// </summary>
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetDllDirectory(string lpPathName);

        static void Main(string[] args)
        {
            try
            {
                // ── Logging setup ─────────────────────────────────────────────
                string exeDir      = AppDomain.CurrentDomain.BaseDirectory;
                string logDir      = Path.Combine(exeDir, ".log");
                string logFilePath = Path.Combine(logDir, "consolelog.txt");
                bool   debugMode   = Array.Exists(args, a => a.ToLower() == "--debug");

                AppLogger.Initialize(logFilePath, debugMode);
                AppLogger.Banner("Snapdragon Profiler CLI");
                AppLogger.Info("Main", $"Args: {string.Join(" ", args)}");

                Console.WriteLine("=== Snapdragon Profiler Command Line Tool ===");
                Console.WriteLine($"Log: {logFilePath}\n");

                // 解析命令行参数
                string? modeArg = null;
                string? sdpArg = null;
                string? resourceIdArg = null;
                string? outputArg = null;
                string? captureIdArg = null;
                string? drawCallIdArg = null;
                string? pipelineIdArg = null;
                string? maxDrawCallsArg = null;
                
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToLower() == "-mode" && i + 1 < args.Length)
                    {
                        modeArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-sdp" && i + 1 < args.Length)
                    {
                        sdpArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-resource-id" && i + 1 < args.Length)
                    {
                        resourceIdArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-output" && i + 1 < args.Length)
                    {
                        outputArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-capture-id" && i + 1 < args.Length)
                    {
                        captureIdArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-drawcall-id" && i + 1 < args.Length)
                    {
                        drawCallIdArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-pipeline-id" && i + 1 < args.Length)
                    {
                        pipelineIdArg = args[i + 1];
                        i++;
                    }
                    else if (args[i].ToLower() == "-max-drawcalls" && i + 1 < args.Length)
                    {
                        maxDrawCallsArg = args[i + 1];
                        i++;
                    }
                }
                
                // 环境准备: 设置DLL搜索路径
                SetupEnvironment();

                // 准备输出目录
                string testPath = PrepareOutputDirectory();

                // 运行应用程序业务逻辑
                Application app = new Application(testPath);
                app.Run(modeArg, sdpArg, resourceIdArg, outputArg, captureIdArg, drawCallIdArg, pipelineIdArg, maxDrawCallsArg);
                
                // Output before closing DualWriter
                Console.WriteLine("\nPress any key to exit...");
            }
            catch (Exception ex)
            {
                AppLogger.Exception("Main", ex, "Unhandled exception");
                Console.WriteLine($"\nERROR: {ex.Message}");

                if (ex is DllNotFoundException ||
                    ex.InnerException is DllNotFoundException ||
                    ex.Message.Contains("SDPCore"))
                {
                    Console.WriteLine("\n=== DLL Loading Error ===");
                    Console.WriteLine("1. Ensure SDPCore.dll is next to the .exe");
                    Console.WriteLine("2. Run the x64 build (not x86)");
                    Console.WriteLine("3. Install Visual C++ Redistributable");
                    Console.WriteLine("4. Snapdragon Profiler must be installed");
                }

                Console.WriteLine("\nPress any key to exit...");
            }
            finally
            {
                AppLogger.Shutdown();
            }

            // Wait for key press after DualWriter cleanup
            try
            {
                // Only read key if console input is available (not redirected and not in debugger without console)
                if (!Console.IsInputRedirected && Environment.UserInteractive)
                {
                    Console.ReadKey();
                }
                else
                {
                    // Give some time to read the output before the console closes
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (InvalidOperationException)
            {
                // Ignore if console is not available
                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 设置DLL搜索路径和环境变量，确保能找到Snapdragon Profiler的依赖和设备端plugins
        /// </summary>
        static void SetupEnvironment()
        {
            string profilerPath = @"C:\Program Files\Qualcomm\Snapdragon Profiler";
            string pluginsPath = Path.Combine(profilerPath, "plugins");
            
            // 添加 Snapdragon Profiler 路径到 DLL 搜索路径
            if (Directory.Exists(profilerPath))
            {
                // 添加主目录到 DLL 搜索路径
                SetDllDirectory(profilerPath);
                Console.WriteLine($"✓ Added to DLL path: {profilerPath}");
                
                // 添加 plugins 目录到 PATH 环境变量（SetDllDirectory 只能设置一个路径）
                if (Directory.Exists(pluginsPath))
                {
                    string currentPath = Environment.GetEnvironmentVariable("PATH") ?? "";
                    Environment.SetEnvironmentVariable("PATH", $"{profilerPath};{pluginsPath};{currentPath}");
                    Console.WriteLine($"✓ Added to PATH: {pluginsPath}");
                }
            }
            else
            {
                Console.WriteLine($"⚠ WARNING: Snapdragon Profiler not found at: {profilerPath}");
            }

            // CRITICAL: Set working directory to match Snapdragon Profiler structure
            // Official structure: service/android/arm64-v8a/
            // SDPCore expects to find service/android/ relative to current directory
            string executableDir = AppDomain.CurrentDomain.BaseDirectory;
            
            Console.WriteLine("\n=== SDPCore environment setup ===");
            Console.WriteLine($"Executable directory: {executableDir}");
            
            // Verify service/android directory exists in executable directory
            string servicePath = Path.Combine(executableDir, "service");
            string androidPath = Path.Combine(servicePath, "android");
            
            if (Directory.Exists(androidPath))
            {
                Console.WriteLine($"✓ service/android/ directory found (matching official structure)");
                
                // Verify plugin directories exist
                string[] architectures = { "arm64-v8a", "armeabi-v7a" };
                foreach (string arch in architectures)
                {
                    string archPath = Path.Combine(androidPath, arch);
                    if (Directory.Exists(archPath))
                    {
                        // Count plugin files
                        string[] pluginFiles = Directory.GetFiles(archPath, "plugin*");
                        Console.WriteLine($"✓ Found {pluginFiles.Length} plugin files in service/android/{arch}/");
                        
                        // List key service files
                        string[] keyFiles = { "pluginGPU-Vulkan", "pluginCPU", "sdpservice", "qhas_sdp" };
                        foreach (string file in keyFiles)
                        {
                            string filePath = Path.Combine(archPath, file);
                            if (File.Exists(filePath))
                            {
                                var fileInfo = new FileInfo(filePath);
                                Console.WriteLine($"    ✓ {file} ({fileInfo.Length} bytes)");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"⚠ Directory not found: service/android/{arch}/");
                    }
                }
                
                // Set working directory to executable directory so SDPCore can find service/android/
                Directory.SetCurrentDirectory(executableDir);
                Console.WriteLine($"✓ Working directory set to: {Directory.GetCurrentDirectory()}");
            }
            else
            {
                Console.WriteLine($"⚠ WARNING: service/android/ directory not found at: {androidPath}");
                Console.WriteLine("  Expected structure: service/android/arm64-v8a/");
                Console.WriteLine("  Device-side plugins are MISSING!");
                Console.WriteLine("  Run 'dotnet build' to deploy plugins automatically.");
                Console.WriteLine("");
                Console.WriteLine("  ProcessManager will NOT discover processes without device plugins!");
            }
            Console.WriteLine("===================================\n");
        }

        /// <summary>
        /// 准备输出目录（项目根目录下的test文件夹）
        /// </summary>
        static string PrepareOutputDirectory()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", ".."));
            string testPath = Path.Combine(projectRoot, "test");
            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }
            return testPath;
        }
        
    }
}
