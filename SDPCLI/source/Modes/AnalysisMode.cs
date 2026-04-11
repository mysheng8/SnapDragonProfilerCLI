using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SnapdragonProfilerCLI.Analysis;
using SnapdragonProfilerCLI.Services.Analysis;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// Analysis mode — analyzes an existing .sdp file.
    /// Non-interactive when sdpPath is provided via positional arg.
    /// Interactive (file + snapshot selection) when no sdpPath given.
    /// </summary>
    public class AnalysisMode : IMode
    {
        private readonly AnalysisPipeline pipeline;
        private readonly SdpFileService fileService;
        private readonly Config config;
        private readonly ILogger logger;
        private readonly string outputPath;
        private readonly string? specifiedSdpPath;
        private readonly int? specifiedSnapshotId;  // null = all (>= 2 means specific)
        private readonly string? targetArg;         // -target/-t value (reserved for P1)
        private readonly string? outputArg;         // -output/-o override

        public string Name => "Analysis";
        public string Description => "Analyze existing .sdp file";

        public AnalysisMode(
            AnalysisPipeline pipeline,
            SdpFileService fileService,
            Config config,
            string outputPath,
            ILogger logger,
            string? sdpPath    = null,
            int?    snapshotId = null,
            string? targetArg  = null,
            string? outputArg  = null)
        {
            this.pipeline            = pipeline;
            this.fileService         = fileService;
            this.config              = config;
            this.outputPath          = outputPath;
            this.logger              = logger;
            this.specifiedSdpPath    = sdpPath;
            this.specifiedSnapshotId = snapshotId;
            this.targetArg           = targetArg;
            this.outputArg           = outputArg;
        }
        
        public void Run()
        {
            logger.Info("\n=== Analysis Mode ===\n");

            string? selectedFile = null;

            if (!string.IsNullOrEmpty(specifiedSdpPath))
            {
                // Non-interactive: resolve via new SdpDir/ProjectDir chain
                selectedFile = ResolveSdpPath(specifiedSdpPath!);
                if (selectedFile == null)
                {
                    logger.Error("Specified SDP file not found");
                    return;
                }
            }
            else
            {
                // Interactive: scan and let user choose
                selectedFile = SelectSdpFileInteractively();
                if (selectedFile == null) return;
            }

            int cmdBufIdx = config.GetInt("AnalysisCmdBufferIndex", 0);
            int? cmdBufferFilter = cmdBufIdx >= 1 ? (int?)cmdBufIdx
                                 : cmdBufIdx == 0 ? (int?)0
                                 : null;
            if (cmdBufIdx == -1)
                logger.Info("  CommandBuffer filter: ALL (AnalysisCmdBufferIndex=-1)");
            else if (cmdBufIdx == 0)
                logger.Info("  CommandBuffer filter: AUTO");
            else
                logger.Info($"  CommandBuffer filter: {cmdBufferFilter}");

            var captureIds = ScanCaptureIds(selectedFile);
            if (captureIds.Count == 0)
            {
                logger.Error("No snapshot_* found in SDP.");
                return;
            }

            // Resolve output directory: -output/-o arg → AnalysisDir/<basename> (default)
            string resolvedOutput = ResolveOutputDir(selectedFile);
            var analysisTarget = AnalysisTargetExtensions.Parse(targetArg);

            // Resolve .sdp path → actual sdp.db path before calling pipeline
            string? dbPath = fileService.FindDatabasePath(selectedFile);
            if (dbPath == null)
            {
                logger.Error("Could not locate sdp.db. Ensure the .sdp archive or adjacent session directory is intact.");
                return;
            }
            logger.Info($"  Database: {dbPath}");

            // Non-interactive path: snapshotId specified or single snapshot
            if (specifiedSdpPath != null)
            {
                List<uint> toRun;
                if (specifiedSnapshotId.HasValue)
                {
                    uint sid = (uint)specifiedSnapshotId.Value;
                    if (!captureIds.Contains(sid))
                    {
                        logger.Error($"snapshot_{sid} not found in SDP. Available: " +
                            string.Join(", ", captureIds.Select(x => $"snapshot_{x}")));
                        return;
                    }
                    toRun = new List<uint> { sid };
                }
                else
                {
                    // -s 1 or omitted = all
                    toRun = captureIds;
                }

                foreach (var captureId in toRun)
                {
                    logger.Info($"\n--- Analyzing snapshot_{captureId} ---");
                    try { pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, cmdBufferFilter, analysisTarget); }
                    catch (Exception ex) { logger.Error($"Analysis failed for snapshot_{captureId}: {ex.Message}"); }
                }
                return;
            }

            // Interactive loop
            while (true)
            {
                Console.WriteLine($"\nFound {captureIds.Count} snapshot(s) in SDP:");
                Console.WriteLine("  0. Analyze ALL");
                for (int i = 0; i < captureIds.Count; i++)
                    Console.WriteLine($"  {i + 1}. snapshot_{captureIds[i]}");
                Console.Write($"Select (0=all, 1-{captureIds.Count}), or ESC to exit: ");
                string input = "";
                while (true)
                {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Escape) { Console.WriteLine(); return; }
                    if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                    if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsDigit(key.KeyChar))
                    {
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }

                if (string.IsNullOrWhiteSpace(input)) break;
                if (!int.TryParse(input, out int sel) || sel < 0 || sel > captureIds.Count)
                { Console.WriteLine("Invalid selection."); continue; }

                var runList = sel == 0 ? captureIds : new List<uint> { captureIds[sel - 1] };
                foreach (var captureId in runList)
                {
                    logger.Info($"\n--- Analyzing snapshot_{captureId} ---");
                    try { pipeline.RunAnalysis(dbPath, resolvedOutput, captureId, cmdBufferFilter, analysisTarget); }
                    catch (Exception ex) { logger.Error($"Analysis failed for snapshot_{captureId}: {ex.Message}"); }
                }
                Console.WriteLine("\n=== Done. Select another snapshot or ESC to exit. ===");
            }
        }
        private string? SelectSdpFileInteractively()
        {
            // Scan SdpDir first, fall back to ProjectDir, then legacy TestDirectory
            string sdpDir = GetSdpDir();

            if (!Directory.Exists(sdpDir))
            {
                // Fallback: legacy TestDirectory key
                string testDir = config.Get("TestDirectory", outputPath);
                if (Directory.Exists(testDir))
                {
                    logger.Info($"  SdpDir not found ({sdpDir}); falling back to TestDirectory ({testDir})");
                    sdpDir = testDir;
                }
                else
                {
                    logger.Error($"SDP directory does not exist: {sdpDir}");
                    logger.Info("Configure SdpDir (or TestDirectory) in config.ini");
                    return null;
                }
            }

            var sdpFiles = fileService.ScanSdpFiles(sdpDir);

            if (sdpFiles.Count == 0)
            {
                logger.Warning($"No .sdp files found in {sdpDir}");
                return null;
            }

            Console.WriteLine($"\nFound {sdpFiles.Count} .sdp file(s) in {sdpDir}:\n");
            for (int i = 0; i < sdpFiles.Count; i++)
            {
                var file = sdpFiles[i];
                Console.WriteLine($"  {i + 1}. {file.RelativePath}");
                Console.WriteLine($"     Size: {file.SizeMB:F2} MB, Modified: {file.LastModified:yyyy-MM-dd HH:mm:ss}");
            }

            Console.Write("\nEnter file number to analyze: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int selection) || selection < 1 || selection > sdpFiles.Count)
            {
                logger.Error("Invalid selection");
                return null;
            }

            string selectedPath = sdpFiles[selection - 1].FilePath;
            logger.Info($"\nSelected: {Path.GetFileName(selectedPath)}");
            return selectedPath;
        }

        private string GetSdpDir()
        {
            string projectDir = GetProjectDir();
            string sdpDirRel  = config.Get("SdpDir", "sdp");
            if (Path.IsPathRooted(sdpDirRel)) return sdpDirRel;
            return Path.GetFullPath(Path.Combine(projectDir, sdpDirRel));
        }
        
        /// <summary>
        /// 扫描所有 snapshot_* captureId，逻辑与 SdpFileService.FindDatabasePath 对齐：
        /// 优先使用 sdpPath 同级的同名目录（已展开的 session 目录），与 pipeline 保持一致。
        /// </summary>
        private List<uint> ScanCaptureIds(string sdpPath)
        {
            var ids = new List<uint>();
            try
            {
                // Case 1: sdpPath 本身就是目录
                string searchDir = Directory.Exists(sdpPath) ? sdpPath : "";

                // Case 2: 同名相邻目录（与 FindDatabasePath 保持一致）
                if (string.IsNullOrEmpty(searchDir))
                {
                    string adjacent = Path.Combine(
                        Path.GetDirectoryName(sdpPath) ?? "",
                        Path.GetFileNameWithoutExtension(sdpPath));
                    if (Directory.Exists(adjacent))
                        searchDir = adjacent;
                }

                if (!string.IsNullOrEmpty(searchDir))
                {
                    foreach (var dir in Directory.GetDirectories(searchDir, "snapshot_*"))
                        if (uint.TryParse(Path.GetFileName(dir).Substring("snapshot_".Length), out uint id))
                            ids.Add(id);
                }
            }
            catch (Exception ex) { logger.Warning($"Could not scan snapshots: {ex.Message}"); }
            ids.Sort();
            return ids;
        }

        /// <summary>
        /// Resolve output directory for analysis products.
        /// Priority: -output/-o arg → AnalysisDir/<sdp_basename> (default).
        /// </summary>
        private string ResolveOutputDir(string sdpPath)
        {
            if (!string.IsNullOrWhiteSpace(outputArg))
            {
                // -o given: absolute or relative to AnalysisDir
                if (Path.IsPathRooted(outputArg)) return outputArg!;
                string analysisDir = GetAnalysisDir();
                string resolved = Path.GetFullPath(Path.Combine(analysisDir, outputArg!));
                Directory.CreateDirectory(resolved);
                return resolved;
            }
            // Default: AnalysisDir/<sdp_basename>
            string sdpBasename = Path.GetFileNameWithoutExtension(sdpPath);
            string sessionDir  = Path.Combine(GetAnalysisDir(), sdpBasename);
            Directory.CreateDirectory(sessionDir);
            return sessionDir;
        }

        private string GetAnalysisDir()
        {
            string projectDir  = GetProjectDir();
            string analysisRel = config.Get("AnalysisDir", "analysis");
            if (Path.IsPathRooted(analysisRel)) return analysisRel;
            return Path.GetFullPath(Path.Combine(projectDir, analysisRel));
        }

        private string GetProjectDir()
        {
            string workDir    = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
            string projectRel = config.Get("ProjectDir", "project");
            if (Path.IsPathRooted(projectRel)) return projectRel;
            return Path.GetFullPath(Path.Combine(workDir, projectRel));
        }

        /// <summary>
        /// Resolve SDP path: absolute → direct; relative → SdpDir, then ProjectDir.
        /// </summary>
        private string? ResolveSdpPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                if (File.Exists(path) || Directory.Exists(path)) return path;
                logger.Error($"SDP not found: {path}");
                return null;
            }
            // Try SdpDir first
            string sdpDir = GetSdpDir();
            string attempt1 = Path.GetFullPath(Path.Combine(sdpDir, path));
            if (File.Exists(attempt1) || Directory.Exists(attempt1))
            { logger.Info($"SDP resolved via SdpDir: {attempt1}"); return attempt1; }
            // Then ProjectDir
            string attempt2 = Path.GetFullPath(Path.Combine(GetProjectDir(), path));
            if (File.Exists(attempt2) || Directory.Exists(attempt2))
            { logger.Info($"SDP resolved via ProjectDir: {attempt2}"); return attempt2; }

            logger.Error($"SDP not found: '{path}'  (searched: {attempt1}  and  {attempt2})");
            return null;
        }
    }
}