using System;
using System.Linq;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Interactive mode — preserves the full original menu-driven experience.
    /// Activated when sdpcli is run with no subcommand.
    /// </summary>
    public class InteractiveMode : IMode
    {
        private readonly Config _config;
        private readonly string _testPath;
        private readonly Func<string?, string?> _readLine;
        private readonly string? _passModeArg;

        public string Name        => "Interactive";
        public string Description => "Interactive mode selection";

        public InteractiveMode(
            Config config,
            string testPath,
            Func<string?, string?> readLine,
            string? passModeArg = null)
        {
            _config      = config;
            _testPath    = testPath;
            _readLine    = readLine;
            _passModeArg = passModeArg;
        }

        public void Run()
        {
            if (!string.IsNullOrWhiteSpace(_passModeArg))
                _config.Set("AnalysisPassMode", _passModeArg!.Trim().ToLowerInvariant());

            Console.WriteLine("\n=== Snapdragon Profiler CLI ===");
            Console.WriteLine("Select mode:");
            Console.WriteLine("  1. Snapshot  - Connect device and capture frames");
            Console.WriteLine("  2. Analysis  - Analyze existing .sdp file");
            Console.WriteLine("  3. Server    - Start HTTP API server");
            Console.Write("\nEnter mode number (1/2/3): ");
            string? modeInput = _readLine("1");

            IMode mode;
            switch (modeInput?.Trim())
            {
                case "2":
                {
                    var logger          = new Logging.ContextLogger("Analysis");
                    var sdpFileService  = new Services.Analysis.SdpFileService(_config, logger);
                    var analysisService = new Services.Analysis.DrawCallAnalysisService(logger);
                    var reportService   = new Services.Analysis.RawJsonGenerationService(_config, logger);
                    var categories      = _config.Get("AnalysisCategories", "Scene,PostProcess,UI,Other")
                                                 .Split(',').Select(s => s.Trim()).ToList();
                    var llmService      = new LlmApiWrapper(_config, logger);
                    int maxChars        = _config.GetInt("LlmMaxShaderChars", 4000);
                    var labelService    = new Services.Analysis.DrawCallLabelService(categories, llmService, logger, maxChars);
                    var metricsService  = new Services.Analysis.MetricsQueryService(_config);
                    var pipeline        = new Analysis.AnalysisPipeline(
                        sdpFileService, analysisService, reportService,
                        labelService, metricsService, _config, logger, llmService);
                    // No sdpPath → AnalysisMode will prompt interactively for file + snapshot
                    mode = new AnalysisMode(pipeline, sdpFileService, _config, _testPath, logger);
                    break;
                }
                case "3":
                {
                    int port = _config.GetInt("Server.Port", 5000);
                    mode = new Server.ServerMode(_config, _testPath, port);
                    break;
                }
                default: // "1" or anything else
                    mode = new SnapshotCaptureMode(_config, _testPath, _readLine);
                    break;
            }

            mode.Run();
        }
    }
}
