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
    /// Analysis模式 - 分析已有的.sdp文件（重构后的薄包装）
    /// </summary>
    public class AnalysisMode : IMode
    {
        private readonly AnalysisPipeline pipeline;
        private readonly SdpFileService fileService;
        private readonly Config config;
        private readonly ILogger logger;
        private readonly string outputPath;
        private readonly string? specifiedSdpPath;
        
        public string Name => "Analysis";
        public string Description => "分析已有的.sdp文件";
        
        public AnalysisMode(
            AnalysisPipeline pipeline,
            SdpFileService fileService,
            Config config,
            string outputPath,
            ILogger logger,
            string? sdpPath = null)
        {
            this.pipeline = pipeline;
            this.fileService = fileService;
            this.config = config;
            this.outputPath = outputPath;
            this.logger = logger;
            this.specifiedSdpPath = sdpPath;
        }
        
        public void Run()
        {
            logger.Info("\n=== Analysis Mode ===\n");

            string? selectedFile = null;

            // 如果命令行指定了 .sdp 文件，直接使用
            if (!string.IsNullOrEmpty(specifiedSdpPath))
            {
                selectedFile = ResolveSdpPath(specifiedSdpPath);
                if (selectedFile == null)
                {
                    logger.Error("Specified SDP file not found");
                    return;
                }
            }
            else
            {
                // 交互模式：扫描并让用户选择
                selectedFile = SelectSdpFileInteractively();
                if (selectedFile == null) return;
            }

            // AnalysisCmdBufferIndex: -1=all, 0=auto (most-DC CB), N>=1=specific
            int cmdBufIdx = config.GetInt("AnalysisCmdBufferIndex", 0);
            int? cmdBufferFilter = cmdBufIdx >= 1 ? (int?)cmdBufIdx
                                 : cmdBufIdx == 0 ? (int?)0
                                 : null; // -1 = all
            if (cmdBufIdx == -1)
                logger.Info("  CommandBuffer filter: ALL (AnalysisCmdBufferIndex=-1)");
            else if (cmdBufIdx == 0)
                logger.Info("  CommandBuffer filter: AUTO (will select CmdBufferIdx with most DCs)");
            else
                logger.Info($"  CommandBuffer filter: {cmdBufferFilter} (specific)");

            string? metricsCSV = config.Get("AnalysisMetricsCSV", "");
            if (!string.IsNullOrWhiteSpace(metricsCSV))
            {
                string root = config.Get("WorkingDirectory", AppDomain.CurrentDomain.BaseDirectory);
                if (!System.IO.Path.IsPathRooted(metricsCSV))
                    metricsCSV = System.IO.Path.GetFullPath(System.IO.Path.Combine(root, metricsCSV));
                if (!System.IO.File.Exists(metricsCSV))
                {
                    logger.Info($"  ⚠ AnalysisMetricsCSV not found: {metricsCSV} — will auto-discover");
                    metricsCSV = null;
                }
                else
                {
                    logger.Info($"  Metrics CSV: {metricsCSV}");
                }
            }
            else
            {
                metricsCSV = null;
            }

            // ── 扫描一次 captureId 列表，然后进入交互循环 ─────────────────────
            var captureIds = ScanCaptureIds(selectedFile);
            if (captureIds.Count == 0)
            {
                logger.Error("No snapshot_* found in SDP.");
                return;
            }

            while (true)
            {
                Console.WriteLine($"\nFound {captureIds.Count} snapshot(s) in SDP:");
                Console.WriteLine("  0. Analyze ALL");
                for (int i = 0; i < captureIds.Count; i++)
                    Console.WriteLine($"  {i + 1}. snapshot_{captureIds[i]}");
                Console.Write($"Select (0=all, 1-{captureIds.Count}), or ENTER to exit: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    break;

                if (!int.TryParse(input, out int sel) || sel < 0 || sel > captureIds.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    continue;
                }

                var toRun = sel == 0 ? captureIds : new List<uint> { captureIds[sel - 1] };

                foreach (var captureId in toRun)
                {
                    logger.Info($"\n--- Analyzing snapshot_{captureId} ---");
                    try
                    {
                        pipeline.RunAnalysis(selectedFile, outputPath, captureId, cmdBufferFilter, metricsCSV);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Analysis failed for snapshot_{captureId}: {ex.Message}");
                    }
                }

                Console.WriteLine("\n=== Done. Select another snapshot or press ENTER to exit. ===");
            }
        }
        
        /// <summary>
        /// 交互式选择 .sdp 文件
        /// </summary>
        private string? SelectSdpFileInteractively()
        {
            string testDir = config.Get("TestDirectory", outputPath);
            
            if (!Directory.Exists(testDir))
            {
                logger.Error($"Test directory does not exist: {testDir}");
                logger.Info("Please configure correct TestDirectory path in config.ini");
                return null;
            }
            
            var sdpFiles = fileService.ScanSdpFiles(testDir);
            
            if (sdpFiles.Count == 0)
            {
                logger.Warning("No .sdp files found");
                return null;
            }
            
            // 显示文件列表
            Console.WriteLine($"\nFound {sdpFiles.Count} .sdp file(s):\n");
            for (int i = 0; i < sdpFiles.Count; i++)
            {
                var file = sdpFiles[i];
                Console.WriteLine($"  {i + 1}. {file.RelativePath}");
                Console.WriteLine($"     Size: {file.SizeMB:F2} MB, Modified: {file.LastModified:yyyy-MM-dd HH:mm:ss}");
            }
            
            // 用户选择
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
        /// 打开 SDP（ZIP），扫描 snapshot_* 目录，交互式让用户选择；只有一个时直接返回。
        /// </summary>
        private uint SelectCaptureIdFromSdp(string sdpPath)
        {
            var ids = new List<uint>();
            try
            {
                // SDP 可能是 ZIP 文件，也可能是目录（session dir）
                if (File.Exists(sdpPath))
                {
                    using var zip = ZipFile.OpenRead(sdpPath);
                    foreach (var entry in zip.Entries)
                    {
                        // 条目名形如 "snapshot_3/1_screenshot.bmp" 或 "snapshot_3/"
                        var parts = entry.FullName.Split('/');
                        if (parts.Length >= 1 && parts[0].StartsWith("snapshot_"))
                        {
                            if (uint.TryParse(parts[0].Substring("snapshot_".Length), out uint id) && !ids.Contains(id))
                                ids.Add(id);
                        }
                    }
                }
                else if (Directory.Exists(sdpPath))
                {
                    foreach (var dir in Directory.GetDirectories(sdpPath, "snapshot_*"))
                    {
                        if (uint.TryParse(Path.GetFileName(dir).Substring("snapshot_".Length), out uint id))
                            ids.Add(id);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Could not scan snapshots in SDP: {ex.Message}");
            }

            ids.Sort();

            if (ids.Count == 0)
            {
                Console.Write("  No snapshot_* dirs found in SDP. Enter capture ID manually: ");
                string? manual = Console.ReadLine();
                if (uint.TryParse(manual, out uint manualId) && manualId > 0) return manualId;
                logger.Error("Invalid capture ID");
                return 0;
            }

            if (ids.Count == 1)
            {
                logger.Info($"  Found 1 snapshot: snapshot_{ids[0]} — using it");
                return ids[0];
            }

            // 多个 snapshot，让用户选
            Console.WriteLine($"\nFound {ids.Count} snapshots in SDP:");
            for (int i = 0; i < ids.Count; i++)
                Console.WriteLine($"  {i + 1}. snapshot_{ids[i]}");
            Console.Write($"\nSelect snapshot (1-{ids.Count}): ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int sel) && sel >= 1 && sel <= ids.Count)
                return ids[sel - 1];

            logger.Error("Invalid selection");
            return 0;
        }

        /// <summary>
        /// 解析 .sdp 路径（相对路径 → 绝对路径）
        /// </summary>
        private string? ResolveSdpPath(string path)
        {
            string testDir = config.Get("TestDirectory", outputPath);
            string resolvedPath = fileService.ResolvePath(path, testDir);
            
            if (!File.Exists(resolvedPath) && !Directory.Exists(resolvedPath))
            {
                logger.Error($"Specified SDP file not found: {resolvedPath}");
                logger.Info($"  Searched at: {resolvedPath}");
                return null;
            }
            
            logger.Info($"Analyzing specified file: {Path.GetFileName(resolvedPath)}");
            return resolvedPath;
        }
    }
}
