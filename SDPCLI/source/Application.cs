using System;
using System.IO;
using System.Linq;
using System.Threading;
using SnapdragonProfilerCLI.Modes;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// åº”ç”¨ç¨‹åºä¸šåŠ¡é€»è¾‘
    /// </summary>
    public class Application
    {
        private readonly string testPath;
        private readonly Config config;


        public Application(string testOutputPath)
        {
            testPath = testOutputPath;
            
            // Load configuration from config.ini in current directory
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
            config = new Config(configPath);
            
            if (File.Exists(configPath))
            {
                Console.WriteLine($"âœ“ Loaded configuration from: {configPath}");
            }
            else
            {
                Console.WriteLine($"â„¹ No config.ini found at: {configPath}");
                Console.WriteLine("  Will use interactive mode");
            }
        }

        /// <summary>
        /// Safely read a line from console, handling input redirection (e.g., when debugging)
        /// </summary>
        private string? SafeReadLine(string? defaultValue = null)
        {
            if (Console.IsInputRedirected || !Environment.UserInteractive)
            {
                if (defaultValue != null)
                {
                    Console.WriteLine($"[Input redirected - using default: {defaultValue}]");
                    return defaultValue;
                }
                else
                {
                    Console.WriteLine("[Input redirected - no default available, returning empty]");
                    return "";
                }
            }

            try
            {
                return Console.ReadLine();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("[Console input not available - using default]");
                return defaultValue ?? "";
            }
        }

        /// <summary>
        /// Safely wait for key press, handling input redirection (e.g., when debugging)
        /// </summary>
        private void SafeWaitForKey(int sleepMs = 2000)
        {
            if (Console.IsInputRedirected || !Environment.UserInteractive)
            {
                Console.WriteLine($"[Input redirected - waiting {sleepMs}ms instead of key press]");
                Thread.Sleep(sleepMs);
                return;
            }

            try
            {
                Console.ReadKey();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(sleepMs);
            }
        }

        /// <summary>
        /// Application entry point - select and execute mode
        /// </summary>
        /// <param name="modeArg">Mode from command line: "capture", "analysis", "extract-texture", "extract-shader", or null for interactive</param>
        /// <param name="sdpPath">SDP file path</param>
        /// <param name="resourceId">Resource ID for texture extraction</param>
        /// <param name="outputPath">Output path / directory</param>
        /// <param name="captureId">Capture ID (optional, default 3)</param>
        /// <param name="drawCallId">DrawCall ID for shader extraction (e.g. "10" or "1.0.10")</param>
        /// <param name="pipelineId">Pipeline ID for shader extraction</param>
        public void Run(string? modeArg = null, string? sdpPath = null, string? resourceId = null, string? outputPath = null, string? captureId = null, string? drawCallId = null, string? pipelineId = null, string? maxDrawCalls = null, string? passMode = null)
        {
            // Inject CLI overrides into config before anything else reads them
            if (!string.IsNullOrWhiteSpace(passMode))
                config.Set("AnalysisPassMode", passMode!.Trim().ToLowerInvariant());
            // Create logger instance
            ILogger logger = new Logging.ContextLogger("Analysis");
            
            // Determine mode
            string? modeInput = null;
            
            if (modeArg != null)
            {
                // Command line mode specified
                string modeLower = modeArg.ToLower();
                if (modeLower == "analysis" || modeLower == "analyze" || modeLower == "2")
                {
                    modeInput = "2";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - Analysis Mode ===");
                }
                else if (modeLower == "capture" || modeLower == "snapshot" || modeLower == "1")
                {
                    modeInput = "1";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - Capture Mode ===");
                }
                else if (modeLower == "extract-texture" || modeLower == "texture")
                {
                    modeInput = "3";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - Texture Extraction Mode ===");
                }
                else if (modeLower == "extract-shader" || modeLower == "shader")
                {
                    modeInput = "4";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - Shader Extraction Mode ===");
                }
                else if (modeLower == "dcanalysis")
                {
                    modeInput = "5";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - DrawCall Analysis ===");
                }
                else if (modeLower == "extract-mesh" || modeLower == "mesh")
                {
                    modeInput = "6";
                    Console.WriteLine("\n=== Snapdragon Profiler CLI - Mesh Extraction Mode ===");
                }
                else
                {
                    Console.WriteLine($"\nError: Invalid mode '{modeArg}'");
                    Console.WriteLine("Valid modes: capture, analysis, extract-texture, extract-shader, extract-mesh");
                    return;
                }
            }
            else
            {
                // Interactive mode selection
                Console.WriteLine("\n=== Snapdragon Profiler CLI ===");
                Console.WriteLine("Select mode:");
                Console.WriteLine("  1. Snapshot Capture - Capture single frame");
                Console.WriteLine("  2. Analysis - Analyze existing .sdp file");
                Console.WriteLine("  3. Texture Extraction - Extract textures from .sdp file");
                Console.WriteLine("  4. Shader Extraction - Extract shaders from .sdp file");
                Console.WriteLine("  5. DrawCall Analysis - Extract shader + textures of a specific DrawCall");
                Console.WriteLine("  6. Mesh Extraction - Reconstruct 3D mesh from vertex/index buffers");
                Console.Write("\nEnter mode number (1/2/3/4/5/6): ");
                modeInput = SafeReadLine("1");
            }
            
            // Create and execute mode based on selection
            IMode mode;
            if (modeInput == "2")
            {
                // Analysis mode: åˆ›å»ºæœåŠ¡å’Œ Pipeline
                var sdpFileService  = new Services.Analysis.SdpFileService(config, logger);
                var analysisService = new Services.Analysis.DrawCallAnalysisService(logger);
                var reportService       = new Services.Analysis.RawJsonGenerationService(config, logger);
                var categories          = config.Get("AnalysisCategories", "场景,角色,投影,后处理,特效,UI")
                                               .Split(',').Select(s => s.Trim()).ToList();
                var llmService          = new Tools.LlmApiWrapper(config, logger);
                if (llmService.IsEnabled)
                {
                    string llmModel    = config.Get("LlmModel", "gpt-4o");
                    string llmEndpoint = config.Get("LlmApiEndpoint", "");
                    logger.Info($"  LLM labeling enabled: model={llmModel}, endpoint={llmEndpoint}");
                }
                else
                    logger.Info("  LLM labeling disabled (set LlmApiEndpoint + LlmApiKey in config.ini to enable)");
                int maxShaderChars      = config.GetInt("LlmMaxShaderChars", 4000);
                var labelService        = new Services.Analysis.DrawCallLabelService(categories, llmService, logger, maxShaderChars);
                var metricsService      = new Services.Analysis.MetricsQueryService(config);
                var analysisPipeline = new Analysis.AnalysisPipeline(
                    sdpFileService,
                    analysisService,
                    reportService,
                    labelService,
                    metricsService,
                    config,
                    logger,
                    llmService);

                mode = new AnalysisMode(analysisPipeline, sdpFileService, config, testPath, logger, sdpPath);
            }
            else if (modeInput == "3")
            {
                mode = new TextureExtractionMode(sdpPath, resourceId, outputPath, captureId);
            }
            else if (modeInput == "4")
            {
                mode = new ShaderExtractionMode(sdpPath, drawCallId, pipelineId, outputPath, captureId, config);
            }
            else if (modeInput == "6")
            {
                // Resolve SDP path: try testPath first (same as AnalysisMode), then WorkingDirectory
                string meshRoot = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
                string meshSdp  = sdpPath ?? "";
                if (!string.IsNullOrWhiteSpace(meshSdp) && !Path.IsPathRooted(meshSdp))
                {
                    string fromTest = Path.GetFullPath(Path.Combine(testPath, meshSdp));
                    string fromRoot = Path.GetFullPath(Path.Combine(meshRoot, meshSdp));
                    meshSdp = File.Exists(fromTest) ? fromTest : fromRoot;
                }
                // Resolve output relative to SDP directory (same convention as DrawCallAnalysisMode)
                string sdpDir     = string.IsNullOrWhiteSpace(meshSdp) ? meshRoot : (Path.GetDirectoryName(meshSdp) ?? meshRoot);
                string meshOutput = ResolvePath(outputPath, sdpDir);
                mode = new MeshExtractionMode(
                    string.IsNullOrWhiteSpace(meshSdp) ? sdpPath : meshSdp,
                    drawCallId,
                    string.IsNullOrWhiteSpace(meshOutput) ? outputPath : meshOutput,
                    captureId, maxDrawCalls);
            }
            else if (modeInput == "5")
            {
                // 命令行参数优先，缺失时从 config.ini 读取
                // 相对路径以 config 中的 WorkingDirectory 为基准解析
                string root = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
                string effectiveSdp    = !string.IsNullOrWhiteSpace(sdpPath)   ? sdpPath!   : ResolvePath(config.Get("DCSdpPath"),    root);
                string effectiveOutput = !string.IsNullOrWhiteSpace(outputPath) ? outputPath! : ResolvePath(config.Get("DCOutputDir"), root);
                // 命令行传入的 -sdp 也同样用 WorkingDirectory 解析
                effectiveSdp    = ResolvePath(effectiveSdp,    root);
                effectiveOutput = ResolvePath(effectiveOutput, root);
                // 0 = 未指定，DrawCallAnalysisMode.Run() 会扫 SDP 让用户选
                int cap = !string.IsNullOrWhiteSpace(captureId) && int.TryParse(captureId, out int parsed) ? parsed : 0;
                mode = new DrawCallAnalysisMode(
                    string.IsNullOrWhiteSpace(effectiveSdp)    ? null : effectiveSdp,
                    string.IsNullOrWhiteSpace(effectiveOutput) ? null : effectiveOutput,
                    cap, config);
            }
            else
            {
                mode = new SnapshotCaptureMode(config, testPath, SafeReadLine, sdpPath);
            }

            mode.Run();
        }

        /// <summary>
        /// 将相对路径基于 basePath 转换为绝对路径；已是绝对路径或空套等原样返回。
        /// </summary>
        private static string ResolvePath(string? path, string basePath)
        {
            if (string.IsNullOrWhiteSpace(path)) return "";
            if (Path.IsPathRooted(path)) return path!;
            return Path.GetFullPath(Path.Combine(basePath, path!));
        }
    }
}

