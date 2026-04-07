using System;
using System.IO;
using System.IO.Compression;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Extracts 3D meshes from a .sdp snapshot file by reading vertex buffer and
    /// index buffer binary data from VulkanSnapshotByteBuffers, then exporting
    /// Wavefront OBJ files.
    ///
    /// Usage examples:
    ///   -mode extract-mesh -sdp "file.sdp" -drawcall-id "1.1.5" -output "mesh.obj"
    ///   -mode extract-mesh -sdp "file.sdp" -output "meshes/"
    ///   -mode extract-mesh -sdp "file.sdp" -output "meshes/" -max-drawcalls 100
    /// </summary>
    public class MeshExtractionMode : IMode
    {
        private readonly string? _sdpPath;
        private readonly string? _drawCallIdArg;
        private readonly string? _outputArg;
        private readonly string? _captureIdArg;
        private readonly string? _maxDrawCallsArg;

        public string Name        => "MeshExtraction";
        public string Description => "Extract 3D meshes from vertex/index buffers in a .sdp snapshot file";

        public MeshExtractionMode(
            string? sdpPath,
            string? drawCallIdArg,
            string? outputArg,
            string? captureIdArg,
            string? maxDrawCallsArg)
        {
            _sdpPath         = sdpPath;
            _drawCallIdArg   = drawCallIdArg;
            _outputArg       = outputArg;
            _captureIdArg    = captureIdArg;
            _maxDrawCallsArg = maxDrawCallsArg;
        }

        public void Run()
        {
            try
            {
                // ── Parameter validation ──────────────────────────────────────
                if (string.IsNullOrWhiteSpace(_sdpPath))
                {
                    PrintUsage();
                    return;
                }

                if (!File.Exists(_sdpPath))
                {
                    Console.WriteLine($"Error: SDP file not found: {_sdpPath}");
                    return;
                }

                // Parse capture ID
                int captureId = 3;
                if (!string.IsNullOrWhiteSpace(_captureIdArg) &&
                    !int.TryParse(_captureIdArg, out captureId))
                {
                    Console.WriteLine($"Warning: Invalid capture ID '{_captureIdArg}', using default: 3");
                    captureId = 3;
                }

                // Parse max draw calls for batch mode
                int maxDrawCalls = 50;
                if (!string.IsNullOrWhiteSpace(_maxDrawCallsArg) &&
                    !int.TryParse(_maxDrawCallsArg, out maxDrawCalls))
                {
                    Console.WriteLine($"Warning: Invalid -max-drawcalls '{_maxDrawCallsArg}', using default: 50");
                    maxDrawCalls = 50;
                }

                bool batchMode = string.IsNullOrWhiteSpace(_drawCallIdArg);

                // Default output path
                string output = _outputArg ?? (batchMode ? "mesh_output" : $"drawcall_{_drawCallIdArg}.obj");

                // Single-mode: if output looks like a directory (trailing slash or existing dir),
                // append a safe filename so we don't try to stream-write to a directory path.
                if (!batchMode)
                {
                    bool looksLikeDir = output.EndsWith("/") || output.EndsWith("\\") ||
                                       Directory.Exists(output);
                    if (looksLikeDir)
                    {
                        string safeId = (_drawCallIdArg ?? "unknown").Replace('.', '_');
                        output = Path.Combine(output.TrimEnd('/', '\\'), $"drawcall_{safeId}.obj");
                    }
                }

                Console.WriteLine("\n=== Snapdragon Profiler CLI - Mesh Extraction Mode ===");
                Console.WriteLine($"  SDP File:    {_sdpPath}");
                Console.WriteLine($"  Capture ID:  {captureId}");
                if (!batchMode)
                    Console.WriteLine($"  DrawCall:    {_drawCallIdArg}");
                else
                    Console.WriteLine($"  Batch mode:  max {maxDrawCalls} draw calls");
                Console.WriteLine($"  Output:      {output}");

                // ── Extract sdp.db from ZIP ───────────────────────────────────
                string tempDir = Path.Combine(Path.GetTempPath(), $"sdpcli_mesh_{Guid.NewGuid()}");
                Directory.CreateDirectory(tempDir);
                string dbPath = Path.Combine(tempDir, "sdp.db");

                try
                {
                    Console.WriteLine("\nExtracting database from SDP file...");
                    using (var archive = ZipFile.OpenRead(_sdpPath))
                    {
                        var dbEntry = archive.GetEntry("sdp.db");
                        if (dbEntry == null)
                        {
                            Console.WriteLine("Error: sdp.db not found in SDP file");
                            return;
                        }
                        dbEntry.ExtractToFile(dbPath, overwrite: true);
                    }
                    Console.WriteLine($"  ✓ Database extracted");

                    // ── Run extraction ────────────────────────────────────────
                    var extractor = new MeshExtractor(dbPath, captureId);

                    if (batchMode)
                    {
                        // Batch: export all draw calls that have VB bindings
                        int count = extractor.ExtractAllMeshes(output, maxDrawCalls);
                        if (count == 0)
                            PrintDataAvailabilityHint();
                        else
                            Console.WriteLine($"\n✓ Extracted {count} mesh(es) to: {Path.GetFullPath(output)}");
                    }
                    else
                    {
                        // Single draw call
                        bool success = extractor.ExtractMesh(_drawCallIdArg!, output);
                        if (success)
                            Console.WriteLine($"\n✓ Mesh written: {Path.GetFullPath(output)}");
                        else
                        {
                            Console.WriteLine("\n✗ Mesh extraction failed");
                            PrintDataAvailabilityHint();
                        }
                    }
                }
                finally
                {
                    try { Directory.Delete(tempDir, recursive: true); }
                    catch { /* cleanup best-effort */ }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError during mesh extraction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Error: SDP file path is required. Use -sdp parameter.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Single draw call:");
            Console.WriteLine("    -mode extract-mesh -sdp \"file.sdp\" -drawcall-id \"1.1.5\" -output \"mesh.obj\"");
            Console.WriteLine("    -mode extract-mesh -sdp \"file.sdp\" -drawcall-id 106974 -output \"mesh.obj\"");
            Console.WriteLine();
            Console.WriteLine("  Batch (all draw calls with vertex buffers):");
            Console.WriteLine("    -mode extract-mesh -sdp \"file.sdp\" -output \"meshes/\"");
            Console.WriteLine("    -mode extract-mesh -sdp \"file.sdp\" -output \"meshes/\" -max-drawcalls 100");
            Console.WriteLine();
            Console.WriteLine("Parameters:");
            Console.WriteLine("  -sdp              Path to .sdp capture file");
            Console.WriteLine("  -drawcall-id      DrawCall ID: encoded \"1.1.5\" or plain ApiID integer");
            Console.WriteLine("                    If omitted, all draw calls with vertex buffers are exported");
            Console.WriteLine("  -output           Output .obj path (single) or directory path (batch)");
            Console.WriteLine("  -capture-id       Capture ID, default 3");
            Console.WriteLine("  -max-drawcalls    Maximum draw calls in batch mode, default 50");
        }

        private static void PrintDataAvailabilityHint()
        {
            Console.WriteLine();
            Console.WriteLine("  Hint: Mesh extraction requires vertex/index buffer binary data in");
            Console.WriteLine("  VulkanSnapshotByteBuffers. To verify data availability, run:");
            Console.WriteLine();
            Console.WriteLine("    sqlite3 sdp.db \"SELECT DrawCallApiID, BufferID FROM DrawCallIndexBuffers LIMIT 5\"");
            Console.WriteLine("    # Then with a found BufferID:");
            Console.WriteLine("    sqlite3 sdp.db \"SELECT length(data) FROM VulkanSnapshotByteBuffers WHERE resourceID = <BufferID>\"");
            Console.WriteLine();
            Console.WriteLine("  If the query returns NULL, buffer binary data is not stored in this capture.");
            Console.WriteLine("  Ensure 'Collect Buffer Data' is enabled in Snapdragon Profiler capture options.");
        }
    }
}
