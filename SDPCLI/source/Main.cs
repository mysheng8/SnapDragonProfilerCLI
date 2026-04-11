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
                bool   debugMode   = Array.Exists(args, a => a == "--debug");

                AppLogger.Initialize(logFilePath, debugMode);
                AppLogger.Banner("Snapdragon Profiler CLI");
                AppLogger.Info("Main", $"Args: {string.Join(" ", args)}");

                Console.WriteLine("=== Snapdragon Profiler Command Line Tool ===");
                Console.WriteLine($"Log: {logFilePath}\n");

                // ── New CLI parser ─────────────────────────────────────────────
                // Positional subcommands: snapshot | analysis | (none = interactive)
                // First non-flag token = subcommand; second = positional arg (sdp path / pkg\activity)
                string? subcommand    = null;   // "snapshot" | "analysis" | null
                string? positionalArg = null;   // analysis→sdpPath; snapshot→packageActivity
                string? outputArg     = null;
                string? snapshotIdArg = null;   // -snapshot/-s <N>
                string? targetArg     = null;   // -target/-t <value>
                bool    noExtract     = false;  // --no-extract

                // Legacy flags kept for backward compatibility (deprecated)
                string? passModeArg   = null;
                string? resourceIdArg = null;
                string? captureIdArg  = null;
                string? drawCallIdArg = null;
                string? pipelineIdArg = null;
                string? maxDrawCallsArg = null;

                for (int i = 0; i < args.Length; i++)
                {
                    string a = args[i];

                    if (a == "--debug") { debugMode = true; continue; }

                    // Positional (non-flag) tokens
                    if (!a.StartsWith("-"))
                    {
                        if (subcommand    == null) { subcommand    = a.ToLower(); continue; }
                        if (positionalArg == null) { positionalArg = a;          continue; }
                        continue;
                    }

                    string lo = a.ToLower();

                    // New flags
                    if ((lo == "-snapshot" || lo == "-s") && i + 1 < args.Length)
                        { snapshotIdArg = args[++i]; continue; }
                    if ((lo == "-target" || lo == "-t") && i + 1 < args.Length)
                        { targetArg = args[++i]; continue; }
                    if ((lo == "-output" || lo == "-o") && i + 1 < args.Length)
                        { outputArg = args[++i]; continue; }
                    if (a == "--no-extract")
                        { noExtract = true; continue; }

                    // Legacy flags (deprecated, kept for transition)
                    if (lo == "-mode" && i + 1 < args.Length)
                    {
                        string legacyMode = args[++i].ToLower();
                        if (legacyMode == "analysis" || legacyMode == "analyze" || legacyMode == "2")
                            subcommand = "analysis";
                        else if (legacyMode == "capture" || legacyMode == "snapshot" || legacyMode == "1")
                            subcommand = "snapshot";
                        Console.WriteLine($"[DEPRECATED] -mode flag is deprecated; use positional subcommand instead.");
                        continue;
                    }
                    if (lo == "-sdp" && i + 1 < args.Length)
                    {
                        positionalArg = args[++i];
                        if (subcommand == null) subcommand = "analysis";
                        Console.WriteLine($"[DEPRECATED] -sdp flag is deprecated; use: sdpcli analysis <sdp_path>");
                        continue;
                    }
                    if (lo == "-pass-mode" && i + 1 < args.Length)
                    {
                        passModeArg = args[++i];
                        Console.WriteLine($"[DEPRECATED] -pass-mode is deprecated; use -target/-t instead (e.g. -t status or -t analysis)");
                        continue;
                    }
                    if (lo == "-stats-only")
                    {
                        passModeArg = "stats";
                        Console.WriteLine("[DEPRECATED] -stats-only is deprecated; use: -t status");
                        continue;
                    }
                    if (lo == "-analysis-only")
                    {
                        passModeArg = "analysis";
                        Console.WriteLine("[DEPRECATED] -analysis-only is deprecated; use: -t analysis");
                        continue;
                    }
                    if ((lo == "-resource-id") && i + 1 < args.Length)
                        { resourceIdArg = args[++i]; continue; }
                    if ((lo == "-capture-id") && i + 1 < args.Length)
                        { captureIdArg = args[++i]; continue; }
                    if ((lo == "-drawcall-id") && i + 1 < args.Length)
                        { drawCallIdArg = args[++i]; continue; }
                    if ((lo == "-pipeline-id") && i + 1 < args.Length)
                        { pipelineIdArg = args[++i]; continue; }
                    if ((lo == "-max-drawcalls") && i + 1 < args.Length)
                        { maxDrawCallsArg = args[++i]; continue; }
                }

                // 环境准备: 设置DLL搜索路径
                SetupEnvironment();

                // 准备输出目录
                string testPath = PrepareOutputDirectory();

                // 运行应用程序业务逻辑
                Application app = new Application(testPath);
                app.Run(
                    subcommand:     subcommand,
                    positionalArg:  positionalArg,
                    outputArg:      outputArg,
                    snapshotIdArg:  snapshotIdArg,
                    targetArg:      targetArg,
                    noExtract:      noExtract,
                    passModeArg:    passModeArg,
                    resourceIdArg:  resourceIdArg,
                    captureIdArg:   captureIdArg,
                    drawCallIdArg:  drawCallIdArg,
                    pipelineIdArg:  pipelineIdArg,
                    maxDrawCallsArg: maxDrawCallsArg);
                
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
            string executableDir = AppDomain.CurrentDomain.BaseDirectory;
            string pluginsPath   = Path.Combine(executableDir, "plugins");

            // Primary: bin 目录（build 时已将 dll/ 下所有文件复制过来）
            SetDllDirectory(executableDir);
            Console.WriteLine($"✓ DLL search path: {executableDir}");

            string currentPath = Environment.GetEnvironmentVariable("PATH") ?? "";
            string extraPaths  = executableDir;
            if (Directory.Exists(pluginsPath))
                extraPaths = $"{pluginsPath};{extraPaths}";
            Environment.SetEnvironmentVariable("PATH", $"{extraPaths};{currentPath}");
            Console.WriteLine($"✓ Added to PATH: {extraPaths}");

            // Fallback: 工程 dll/ 目录（源头，版本受 git 控制）
            // 路径: executableDir/../../../../dll  (net472 → Debug → bin → SDPCLI → repo root → dll)
            string repoDllPath = Path.GetFullPath(Path.Combine(executableDir, "..", "..", "..", "..", "dll"));
            if (Directory.Exists(repoDllPath))
            {
                Environment.SetEnvironmentVariable("PATH",
                    $"{Environment.GetEnvironmentVariable("PATH")};{repoDllPath}");
                Console.WriteLine($"✓ Fallback DLL path (repo): {repoDllPath}");
            }
            
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
