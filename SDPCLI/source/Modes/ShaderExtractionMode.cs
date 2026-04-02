using System;
using System.IO;
using System.IO.Compression;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Extracts shaders bound to a DrawCall or Pipeline from a .sdp file.
    /// Outputs SPIR-V binaries and optionally GLSL/HLSL via spirv-cross.
    /// </summary>
    public class ShaderExtractionMode : IMode
    {
        private readonly string? _sdpPath;
        private readonly string? _drawCallId;
        private readonly string? _pipelineIdArg;
        private readonly string? _outputDir;
        private readonly string? _captureIdArg;
        private readonly Config _config;

        public string Name => "ShaderExtraction";
        public string Description => "Extract shaders from a .sdp snapshot file";

        public ShaderExtractionMode(
            string? sdpPath,
            string? drawCallId,
            string? pipelineIdArg,
            string? outputDir,
            string? captureIdArg,
            Config config)
        {
            _sdpPath = sdpPath;
            _drawCallId = drawCallId;
            _pipelineIdArg = pipelineIdArg;
            _outputDir = outputDir;
            _captureIdArg = captureIdArg;
            _config = config;
        }

        public void Run()
        {
            try
            {
                // 1. Validate parameters
                if (string.IsNullOrWhiteSpace(_sdpPath))
                {
                    Console.WriteLine("Error: SDP file path is required. Use -sdp parameter.");
                    Console.WriteLine("Example: -mode extract-shader -sdp \"file.sdp\" -drawcall-id 10 -output \"shaders/\"");
                    return;
                }

                if (!File.Exists(_sdpPath))
                {
                    Console.WriteLine($"Error: SDP file not found: {_sdpPath}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_drawCallId) && string.IsNullOrWhiteSpace(_pipelineIdArg))
                {
                    Console.WriteLine("Error: Either -drawcall-id or -pipeline-id is required.");
                    Console.WriteLine("Example: -drawcall-id 10   or   -drawcall-id 1.0.10   or   -pipeline-id 12345");
                    return;
                }

                // Default output directory
                string outputPath = string.IsNullOrWhiteSpace(_outputDir)
                    ? $"shaders_{_drawCallId ?? _pipelineIdArg}"
                    : _outputDir!;

                // Parse capture ID
                int captureId = 3;
                if (!string.IsNullOrWhiteSpace(_captureIdArg) && !int.TryParse(_captureIdArg, out captureId))
                {
                    Console.WriteLine($"Warning: Invalid capture ID '{_captureIdArg}', using default: 3");
                    captureId = 3;
                }

                Console.WriteLine($"\nExtracting shaders:");
                Console.WriteLine($"  SDP File: {_sdpPath}");
                if (!string.IsNullOrWhiteSpace(_drawCallId))
                    Console.WriteLine($"  DrawCall ID: {_drawCallId}");
                if (!string.IsNullOrWhiteSpace(_pipelineIdArg))
                    Console.WriteLine($"  Pipeline ID: {_pipelineIdArg}");
                Console.WriteLine($"  Capture ID:  {captureId}");
                Console.WriteLine($"  Output Dir:  {outputPath}");

                // 2. Extract sdp.db from SDP ZIP
                string tempDir = Path.Combine(Path.GetTempPath(), $"sdpcli_shader_{Guid.NewGuid()}");
                Directory.CreateDirectory(tempDir);
                string dbPath = Path.Combine(tempDir, "sdp.db");

                try
                {
                    Console.WriteLine("\nExtracting database from SDP file...");
                    using (ZipArchive archive = ZipFile.OpenRead(_sdpPath))
                    {
                        var dbEntry = archive.GetEntry("sdp.db");
                        if (dbEntry == null)
                        {
                            Console.WriteLine("Error: sdp.db not found in SDP file");
                            return;
                        }
                        dbEntry.ExtractToFile(dbPath, overwrite: true);
                    }
                    Console.WriteLine($"✓ Database extracted");

                    // 3. Resolve spirv-cross
                    string? spirvCrossPath = ResolveSpirvCrossPath();
                    if (spirvCrossPath != null)
                        Console.WriteLine($"  spirv-cross: {spirvCrossPath}");
                    else
                        Console.WriteLine("  spirv-cross: not found — GLSL/HLSL decompilation will be skipped\n  (Set VulkanSDKPath in config.ini to enable)");

                    // 4. Extract shaders
                    string shaderFmt = _config.Get("ShaderOutputFormat", "hlsl").Trim().ToLower();
                    Console.WriteLine($"  Output format: {shaderFmt}");

                    var extractor = new ShaderExtractor(dbPath, captureId)
                    {
                        SpirvCrossPath = spirvCrossPath,
                        ShaderOutputFormat = shaderFmt
                    };

                    bool success;
                    if (!string.IsNullOrWhiteSpace(_drawCallId))
                    {
                        success = extractor.ExtractShadersForDrawCall(_drawCallId!, outputPath);
                    }
                    else
                    {
                        if (!uint.TryParse(_pipelineIdArg, out uint pipelineId))
                        {
                            Console.WriteLine($"Error: Invalid pipeline ID: {_pipelineIdArg}");
                            return;
                        }
                        success = extractor.ExtractShadersForPipeline(pipelineId, outputPath);
                    }

                    if (success)
                        Console.WriteLine($"\n✓ Shaders extracted to: {Path.GetFullPath(outputPath)}");
                    else
                        Console.WriteLine("\n✗ Shader extraction failed or incomplete");
                }
                finally
                {
                    if (Directory.Exists(tempDir))
                    {
                        try { Directory.Delete(tempDir, recursive: true); }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError during shader extraction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Resolves spirv-cross.exe from config VulkanSDKPath, VULKAN_SDK env var, or PATH.
        /// </summary>
        private string? ResolveSpirvCrossPath()
        {
            // 1. Config: VulkanSDKPath
            string sdkPath = _config.Get("VulkanSDKPath", "").Trim();
            if (!string.IsNullOrEmpty(sdkPath))
            {
                string c = Path.Combine(sdkPath, "Bin", "spirv-cross.exe");
                if (File.Exists(c)) return c;
                c = Path.Combine(sdkPath, "Bin", "spirv-cross");
                if (File.Exists(c)) return c;
            }

            // 2. VULKAN_SDK environment variable
            string? envSdk = Environment.GetEnvironmentVariable("VULKAN_SDK");
            if (!string.IsNullOrEmpty(envSdk))
            {
                string c = Path.Combine(envSdk, "Bin", "spirv-cross.exe");
                if (File.Exists(c)) return c;
                c = Path.Combine(envSdk, "Bin", "spirv-cross");
                if (File.Exists(c)) return c;
            }

            // 3. System PATH
            foreach (string dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            {
                try
                {
                    string c = Path.Combine(dir.Trim(), "spirv-cross.exe");
                    if (File.Exists(c)) return c;
                }
                catch { }
            }

            return null;
        }
    }
}
