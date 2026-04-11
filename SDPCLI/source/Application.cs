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
        /// <summary>
        /// Application entry point - routes to InteractiveMode, AnalysisMode, or SnapshotCaptureMode.
        /// </summary>
        public void Run(
            string? subcommand      = null,   // "snapshot" | "analysis" | null = interactive
            string? positionalArg   = null,   // analysis->sdpPath; snapshot->packageActivity
            string? outputArg       = null,
            string? snapshotIdArg   = null,
            string? targetArg       = null,
            bool    noExtract       = false,  // --no-extract: skip asset extraction, only write JSONs
            // Legacy / deprecated pass-through
            string? passModeArg     = null,
            string? resourceIdArg   = null,
            string? captureIdArg    = null,
            string? drawCallIdArg   = null,
            string? pipelineIdArg   = null,
            string? maxDrawCallsArg = null)
        {
            if (!string.IsNullOrWhiteSpace(passModeArg))
                config.Set("AnalysisPassMode", passModeArg!.Trim().ToLowerInvariant());

            ILogger logger = new Logging.ContextLogger("Analysis");
            IMode mode;

            if (subcommand == "analysis")
            {
                Console.WriteLine("\n=== Snapdragon Profiler CLI - Analysis Mode ===");
                mode = BuildAnalysisMode(positionalArg, snapshotIdArg, targetArg, outputArg, noExtract, logger);
            }
            else if (subcommand == "snapshot")
            {
                Console.WriteLine("\n=== Snapdragon Profiler CLI - Snapshot Mode ===");
                mode = new SnapshotCaptureMode(config, testPath, SafeReadLine,
                    packageActivity: positionalArg,
                    outputArg:       outputArg);
            }
            else if (subcommand == null)
            {
                mode = new InteractiveMode(config, testPath, SafeReadLine,
                    resourceIdArg, outputArg, captureIdArg, drawCallIdArg,
                    pipelineIdArg, maxDrawCallsArg, passModeArg, logger);
            }
            else
            {
                Console.WriteLine($"Error: Unknown subcommand '{subcommand}'. Valid: snapshot, analysis");
                return;
            }

            mode.Run();
        }

        private IMode BuildAnalysisMode(
            string? sdpPath, string? snapshotIdArg, string? targetArg, string? outputArg,
            bool noExtract, ILogger logger)
        {
            if (noExtract)
                config.Set("AnalysisNoExtract", "true");

            var sdpFileService   = new Services.Analysis.SdpFileService(config, logger);
            var analysisService  = new Services.Analysis.DrawCallAnalysisService(logger);
            var reportService    = new Services.Analysis.RawJsonGenerationService(config, logger);
            var categories       = config.Get("AnalysisCategories", "Scene,PostProcess,UI,Other")
                                         .Split(',').Select(s => s.Trim()).ToList();
            var llmService       = new Tools.LlmApiWrapper(config, logger);
            if (llmService.IsEnabled)
                logger.Info($"  LLM labeling enabled: model={config.Get("LlmModel","gpt-4o")}");
            else
                logger.Info("  LLM labeling disabled");

            int maxShaderChars   = config.GetInt("LlmMaxShaderChars", 4000);
            var labelService     = new Services.Analysis.DrawCallLabelService(categories, llmService, logger, maxShaderChars);
            var metricsService   = new Services.Analysis.MetricsQueryService(config);
            var pipeline         = new Analysis.AnalysisPipeline(
                sdpFileService, analysisService, reportService, labelService,
                metricsService, config, logger, llmService);

            // -s 1 = all (sentinel); -s 0 = all (legacy); N>=2 = specific snapshot
            int? snapshotId = null;
            if (!string.IsNullOrWhiteSpace(snapshotIdArg) &&
                int.TryParse(snapshotIdArg, out int sid) && sid >= 2)
                snapshotId = sid;

            return new AnalysisMode(
                pipeline, sdpFileService, config, testPath, logger,
                sdpPath:    sdpPath,
                snapshotId: snapshotId,
                targetArg:  targetArg,
                outputArg:  outputArg);
        }

        /// <summary>
        /// Resolve a relative path against basePath; absolute paths or empty strings pass through.
        /// </summary>
        private static string ResolvePath(string? path, string basePath)
        {
            if (string.IsNullOrWhiteSpace(path)) return "";
            if (Path.IsPathRooted(path)) return path!;
            return Path.GetFullPath(Path.Combine(basePath, path!));
        }
    }
}