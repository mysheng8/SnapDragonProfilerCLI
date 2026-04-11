using System;
using System.IO;
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
        private readonly ILogger _logger;

        // Legacy passthrough args for modes 3-6
        private readonly string? _resourceIdArg;
        private readonly string? _outputArg;
        private readonly string? _captureIdArg;
        private readonly string? _drawCallIdArg;
        private readonly string? _pipelineIdArg;
        private readonly string? _maxDrawCallsArg;
        private readonly string? _passModeArg;

        public string Name        => "Interactive";
        public string Description => "Interactive mode selection";

        public InteractiveMode(
            Config config,
            string testPath,
            Func<string?, string?> readLine,
            string? resourceIdArg   = null,
            string? outputArg       = null,
            string? captureIdArg    = null,
            string? drawCallIdArg   = null,
            string? pipelineIdArg   = null,
            string? maxDrawCallsArg = null,
            string? passModeArg     = null,
            ILogger? logger         = null)
        {
            _config         = config;
            _testPath       = testPath;
            _readLine       = readLine;
            _resourceIdArg  = resourceIdArg;
            _outputArg      = outputArg;
            _captureIdArg   = captureIdArg;
            _drawCallIdArg  = drawCallIdArg;
            _pipelineIdArg  = pipelineIdArg;
            _maxDrawCallsArg = maxDrawCallsArg;
            _passModeArg    = passModeArg;
            _logger         = logger ?? new Logging.ContextLogger("Interactive");
        }

        public void Run()
        {
            if (!string.IsNullOrWhiteSpace(_passModeArg))
                _config.Set("AnalysisPassMode", _passModeArg!.Trim().ToLowerInvariant());

            Console.WriteLine("\n=== Snapdragon Profiler CLI ===");
            Console.WriteLine("Select mode:");
            Console.WriteLine("  1. Snapshot Capture  - Capture single frame");
            Console.WriteLine("  2. Analysis          - Analyze existing .sdp file");
            Console.WriteLine("  3. Texture Extraction - Extract textures from .sdp file");
            Console.WriteLine("  4. Shader Extraction  - Extract shaders from .sdp file");
            Console.WriteLine("  5. DrawCall Analysis  - Extract shader + textures of a specific DrawCall");
            Console.WriteLine("  6. Mesh Extraction    - Reconstruct 3D mesh from vertex/index buffers");
            Console.Write("\nEnter mode number (1/2/3/4/5/6): ");
            string? modeInput = _readLine("1");

            IMode mode;
            switch (modeInput?.Trim())
            {
                case "2":
                {
                    Console.WriteLine("\n=== Analysis Mode ===");
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
                    mode = new AnalysisMode(pipeline, sdpFileService, _config, _testPath, logger);
                    break;
                }
                case "3":
                    mode = new TextureExtractionMode(null, _resourceIdArg, _outputArg, _captureIdArg);
                    break;
                case "4":
                    mode = new ShaderExtractionMode(null, _drawCallIdArg, _pipelineIdArg, _outputArg, _captureIdArg, _config);
                    break;
                case "5":
                {
                    string root    = _config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
                    string sdpPath = ResolvePath(_config.Get("DCSdpPath"), root);
                    string outPath = ResolvePath(_config.Get("DCOutputDir"), root);
                    int cap = !string.IsNullOrWhiteSpace(_captureIdArg) &&
                              int.TryParse(_captureIdArg, out int p) ? p : 0;
                    mode = new DrawCallAnalysisMode(
                        string.IsNullOrWhiteSpace(sdpPath) ? null : sdpPath,
                        string.IsNullOrWhiteSpace(outPath) ? null : outPath,
                        cap, _config);
                    break;
                }
                case "6":
                {
                    string root    = _config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
                    string sdpPath = ResolvePath(_config.Get("DCSdpPath"), root);
                    string outPath = ResolvePath(_config.Get("DCOutputDir"), root);
                    mode = new MeshExtractionMode(
                        string.IsNullOrWhiteSpace(sdpPath) ? null : sdpPath,
                        _drawCallIdArg, string.IsNullOrWhiteSpace(outPath) ? null : outPath,
                        _captureIdArg, _maxDrawCallsArg);
                    break;
                }
                default: // "1" or anything else
                    mode = new SnapshotCaptureMode(_config, _testPath, _readLine);
                    break;
            }

            mode.Run();
        }

        private static string ResolvePath(string? path, string basePath)
        {
            if (string.IsNullOrWhiteSpace(path)) return "";
            if (Path.IsPathRooted(path)) return path!;
            return Path.GetFullPath(Path.Combine(basePath, path!));
        }
    }
}
