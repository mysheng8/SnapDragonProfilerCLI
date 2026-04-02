using System;
using System.IO;
using System.IO.Compression;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Extracts a single texture from a .sdp file and saves it as PNG.
    /// </summary>
    public class TextureExtractionMode : IMode
    {
        private readonly string? _sdpPath;
        private readonly string? _resourceIdArg;
        private readonly string? _outputArg;
        private readonly string? _captureIdArg;

        public string Name => "TextureExtraction";
        public string Description => "Extract a texture from a .sdp snapshot file";

        public TextureExtractionMode(string? sdpPath, string? resourceIdArg, string? outputArg, string? captureIdArg)
        {
            _sdpPath = sdpPath;
            _resourceIdArg = resourceIdArg;
            _outputArg = outputArg;
            _captureIdArg = captureIdArg;
        }

        public void Run()
        {
            try
            {
                // 1. Validate parameters
                if (string.IsNullOrWhiteSpace(_sdpPath))
                {
                    Console.WriteLine("Error: SDP file path is required. Use -sdp parameter.");
                    Console.WriteLine("Example: -mode extract-texture -sdp \"file.sdp\" -resource-id 1028 -output \"texture.png\"");
                    return;
                }

                if (!File.Exists(_sdpPath))
                {
                    Console.WriteLine($"Error: SDP file not found: {_sdpPath}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_resourceIdArg))
                {
                    Console.WriteLine("Error: Resource ID is required. Use -resource-id parameter.");
                    Console.WriteLine("Example: -resource-id 1028");
                    return;
                }

                if (!ulong.TryParse(_resourceIdArg, out ulong resourceId))
                {
                    Console.WriteLine($"Error: Invalid resource ID: {_resourceIdArg}");
                    return;
                }

                // Default output path
                string outputPath = _outputArg ?? $"texture_{resourceId}.png";
                if (string.IsNullOrWhiteSpace(_outputArg))
                    Console.WriteLine($"No output path specified, using default: {outputPath}");

                // Default capture ID
                int captureId = 3;
                if (!string.IsNullOrWhiteSpace(_captureIdArg) && !int.TryParse(_captureIdArg, out captureId))
                {
                    Console.WriteLine($"Warning: Invalid capture ID '{_captureIdArg}', using default: 3");
                    captureId = 3;
                }

                Console.WriteLine($"\nExtracting texture:");
                Console.WriteLine($"  SDP File:    {_sdpPath}");
                Console.WriteLine($"  Resource ID: {resourceId}");
                Console.WriteLine($"  Capture ID:  {captureId}");
                Console.WriteLine($"  Output:      {outputPath}");

                // 2. Extract sdp.db from SDP ZIP
                string tempDir = Path.Combine(Path.GetTempPath(), $"sdpcli_texture_{Guid.NewGuid()}");
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
                    Console.WriteLine($"✓ Database extracted to: {dbPath}");

                    // 3. Extract texture
                    Console.WriteLine("\nExtracting texture...");
                    var extractor = new TextureExtractor(dbPath, captureId);
                    bool success = extractor.ExtractTexture(resourceId, outputPath);

                    if (success)
                        Console.WriteLine($"\n✓ Texture extracted successfully: {Path.GetFullPath(outputPath)}");
                    else
                        Console.WriteLine("\n✗ Texture extraction failed");
                }
                finally
                {
                    if (Directory.Exists(tempDir))
                    {
                        try
                        {
                            Directory.Delete(tempDir, recursive: true);
                            Console.WriteLine($"✓ Temporary files cleaned up");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: Failed to cleanup temp directory: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError during texture extraction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
