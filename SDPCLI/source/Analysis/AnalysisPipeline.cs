using System;
using System.Collections.Generic;
using System.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Analysis
{
    /// <summary>
    /// Analysis Pipeline — 4-step orchestrator:
    ///   Step 1  Collect all DrawCalls (submit1 / cmd1 filtered)
    ///   Step 2  Label each DC (shader-name rules → Category + Detail)
    ///   Step 3  Join profiler metrics CSV, export labeled CSV
    ///   Step 4  Generate Markdown summary (Top5, category stats, bottleneck)
    /// </summary>
    public class AnalysisPipeline
    {
        private readonly Services.Analysis.SdpFileService sdpFileService;
        private readonly Services.Analysis.DatabaseQueryService dbQueryService;
        private readonly Services.Analysis.DrawCallQueryService drawCallQueryService;
        private readonly Services.Analysis.DrawCallAnalysisService analysisService;
        private readonly Services.Analysis.ReportGenerationService reportService;
        private readonly Services.Analysis.DrawCallLabelService labelService;
        private readonly Services.Analysis.MetricsCsvService metricsService;
        private readonly Config config;
        private readonly ILogger logger;

        public AnalysisPipeline(
            Services.Analysis.SdpFileService sdpFileService,
            Services.Analysis.DatabaseQueryService dbQueryService,
            Services.Analysis.DrawCallQueryService drawCallQueryService,
            Services.Analysis.DrawCallAnalysisService analysisService,
            Services.Analysis.ReportGenerationService reportService,
            Services.Analysis.DrawCallLabelService labelService,
            Services.Analysis.MetricsCsvService metricsService,
            Config config,
            ILogger logger)
        {
            this.sdpFileService       = sdpFileService;
            this.dbQueryService       = dbQueryService;
            this.drawCallQueryService = drawCallQueryService;
            this.analysisService      = analysisService;
            this.reportService        = reportService;
            this.labelService         = labelService;
            this.metricsService       = metricsService;
            this.config               = config;
            this.logger               = logger;
        }

        /// <summary>Run the 4-step analysis pipeline.</summary>
        public void RunAnalysis(string sdpPath, string outputDir, uint captureId,
                                int? cmdBufferFilter = null, string? metricsCSV = null)
        {
            try
            {
                logger.Info("\n=== Analysis Pipeline ===\n");
                bool onlyReport = config.GetBool("AnalysisOnlyGenerateReport", false);
                if (onlyReport)
                    logger.Info("  ℹ AnalysisOnlyGenerateReport=true — will skip extraction and LLM labeling");
                // ── Setup: locate + open DB ───────────────────────────────────
                string? dbPath = sdpFileService.FindDatabasePath(sdpPath);
                if (string.IsNullOrEmpty(dbPath))
                    throw new Exception("sdp.db not found in .sdp file");
                dbQueryService.OpenDatabase(dbPath);

                // ── Pre-compute session paths ────────────────────────────────────
                string sdpName    = System.IO.Path.GetFileNameWithoutExtension(sdpPath);
                string sessionDir = System.IO.Path.Combine(outputDir, sdpName);

                // ── Step 1: Collect all DrawCalls ────────────────────────────
                logger.Info("Step 1: Collecting DrawCalls" +
                    (cmdBufferFilter.HasValue ? $" (CmdBuffer={cmdBufferFilter})" : "") + "...");

                var report = analysisService.AnalyzeAllDrawCalls(dbPath, captureId, cmdBufferFilter);

                logger.Info($"  → {report.AnalyzedDrawCalls} DrawCalls collected" +
                    $"  (pipelines={report.Statistics.TotalPipelines}" +
                    $"  textures={report.Statistics.TotalTextures}" +
                    $"  shaders={report.Statistics.TotalShaders})");

                // ── Output folder: outputDir/<sdp-name>/snapshot_{captureId}/ ──
                string captureOutDir  = System.IO.Path.Combine(sessionDir, $"snapshot_{captureId}");
                System.IO.Directory.CreateDirectory(captureOutDir);
                logger.Info($"  Session folder: {sessionDir}");
                logger.Info($"  Capture output: {captureOutDir}");

                // ── Step 1.5: Extract shaders and textures ────────────────────
                string shaderBaseDir  = System.IO.Path.Combine(captureOutDir, "shaders");
                string textureBaseDir = System.IO.Path.Combine(captureOutDir, "textures");

                bool shadersExist  = System.IO.Directory.Exists(shaderBaseDir);
                bool texturesExist = System.IO.Directory.Exists(textureBaseDir);

                if (onlyReport)
                {
                    logger.Info("\nStep 1.5: Extraction — SKIPPED (AnalysisOnlyGenerateReport=true)");
                }
                else if (shadersExist && texturesExist)
                {
                    logger.Info("\nStep 1.5: Skipping extraction — shaders/ and textures/ already exist.");
                }
                else
                {
                    logger.Info("\nStep 1.5: Extracting shaders and textures...");

                    // Shaders - one folder per DC named dc_{DrawCallNumber}, skips if already exists
                    if (!shadersExist)
                    {
                        string  shaderFmt      = config.Get("ShaderOutputFormat", "hlsl").Trim().ToLower();
                        string? spirvCrossPath = ResolveSpirvCrossPath();
                        int     shaderOkCount  = 0, shaderTotal = 0;
                        foreach (var dc in report.DrawCallResults)
                        {
                            if (dc.PipelineID == 0) continue;
                            shaderTotal++;
                            string dcDir = System.IO.Path.Combine(shaderBaseDir, "dc_" + dc.DrawCallNumber);
                            if (System.IO.Directory.Exists(dcDir)) { shaderOkCount++; continue; }
                            var shExt = new Tools.ShaderExtractor(dbPath, (int)captureId)
                            {
                                SpirvCrossPath     = spirvCrossPath,
                                ShaderOutputFormat = shaderFmt
                            };
                            if (shExt.ExtractShadersForPipeline(dc.PipelineID, dcDir)) shaderOkCount++;
                        }
                        logger.Info($"  \u2192 Shaders: {shaderOkCount}/{shaderTotal} DCs extracted \u2192 {shaderBaseDir}");
                    }
                    else
                    {
                        logger.Info($"  → Shaders: already exist, skipped → {shaderBaseDir}");
                    }

                    // Textures – one extraction per unique TextureID across all DCs
                    if (!texturesExist)
                    {
                        System.IO.Directory.CreateDirectory(textureBaseDir);
                        var allTexIds = report.DrawCallResults
                            .SelectMany(dc => dc.TextureIDs.Select(id => (ulong)id))
                            .Distinct().ToList();
                        var texExt = new Tools.TextureExtractor(dbPath, (int)captureId);
                        int texOk = 0;
                        foreach (var texId in allTexIds)
                        {
                            string texFile = System.IO.Path.Combine(textureBaseDir, $"texture_{texId}.png");
                            if (texExt.ExtractTexture(texId, texFile)) texOk++;
                        }
                        logger.Info($"  → Textures: {texOk}/{allTexIds.Count} extracted → {textureBaseDir}");
                    }
                    else
                    {
                        logger.Info($"  → Textures: already exist, skipped → {textureBaseDir}");
                    }
                }

                // ── Step 2: Label each DC ────────────────────────────────────
                if (onlyReport)
                {
                    logger.Info("\nStep 2: Labeling — SKIPPED, reloading from existing CSV...");
                    LoadLabelsFromCsv(report, captureOutDir);
                }
                else
                {
                logger.Info("\nStep 2: Labeling DrawCalls...");
                int labelCount = 0;
                var categorySummary = new Dictionary<string, int>();
                foreach (var dc in report.DrawCallResults)
                {
                    dc.Label = labelService.Label(dc, shaderBaseDir);
                    categorySummary.TryGetValue(dc.Label.Category, out int n);
                    categorySummary[dc.Label.Category] = n + 1;
                    labelCount++;
                    logger.Info($"  [{labelCount}/{report.DrawCallResults.Count}] DC {dc.DrawCallNumber,-12} [{dc.Label.Category}] {dc.Label.Detail}");
                }
                logger.Info($"  → Labeled {labelCount} DCs:");
                foreach (var kv in categorySummary.OrderByDescending(x => x.Value))
                    logger.Info($"    {kv.Key}: {kv.Value}");
                }

                // ── Step 3: Load metrics CSV, join, export CSV ───────────────
                logger.Info("\nStep 3: Loading metrics...");

                Dictionary<string, DrawCallMetrics> metrics = new();

                // SDK 创建的 snapshot_{captureId}/ 目录 — 我们的文件也写在这里
                string sessionDbDir = sessionDir;
                string snapshotDir  = System.IO.Path.Combine(sessionDbDir, $"snapshot_{captureId}");
                string? captureSubDir = System.IO.Directory.Exists(snapshotDir) ? snapshotDir : null;
                // Fallback to session root for old-format sdp files
                string metricsSearchDir = captureSubDir != null
                    && System.IO.File.Exists(System.IO.Path.Combine(captureSubDir, "DrawCallMetrics.csv"))
                    ? captureSubDir : sessionDbDir;
                string sessionMetricsCsv = System.IO.Path.Combine(metricsSearchDir, "DrawCallMetrics.csv");
                string sessionParamsCsv  = System.IO.Path.Combine(metricsSearchDir, "DrawCallParameters.csv");

                if (System.IO.File.Exists(sessionMetricsCsv) && System.IO.File.Exists(sessionParamsCsv))
                {
                    metrics = metricsService.LoadMetricsFromSession(metricsSearchDir);
                    logger.Info($"  → Loaded {metrics.Count} metric rows from: {metricsSearchDir}");

                    // Join: dc.DrawCallNumber is the DrawCallApiID string → direct key match
                    int joined = 0;
                    foreach (var dc in report.DrawCallResults)
                    {
                        if (metrics.TryGetValue(dc.DrawCallNumber, out var m))
                        { dc.Metrics = m; joined++; }
                    }
                    logger.Info($"  → Joined metrics to {joined} / {report.DrawCallResults.Count} DCs");
                }
                else
                {
                    // Priority 2: external Snapdragon Profiler trace CSV (old approach)
                    string? resolvedMetrics = ResolveMetricsPath(metricsCSV, sdpPath);

                    if (!string.IsNullOrEmpty(resolvedMetrics))
                    {
                        metrics = metricsService.LoadMetrics(resolvedMetrics);
                        logger.Info($"  → Loaded {metrics.Count} metric rows from: {resolvedMetrics}");

                        // Join metrics to DrawCallResults.
                        // Primary: key match on DrawCallNumber (e.g. "1.1.5").
                        // Fallback: positional "1.1.N" when DCs use raw integer IDs
                        //           but the profiler CSV uses encoded format.
                        bool metricsUseEncoded = metrics.Count > 0 &&
                            (metrics.Keys.FirstOrDefault()?.Contains('.') == true);
                        int joined = 0;
                        for (int idx = 0; idx < report.DrawCallResults.Count; idx++)
                        {
                            var dc = report.DrawCallResults[idx];
                            DrawCallMetrics? m = null;
                            if (!metrics.TryGetValue(dc.DrawCallNumber, out m) && metricsUseEncoded)
                                metrics.TryGetValue($"1.{(cmdBufferFilter ?? 1)}.{idx + 1}", out m);
                            if (m != null) { dc.Metrics = m; joined++; }
                        }
                        logger.Info($"  → Joined metrics to {joined} / {report.DrawCallResults.Count} DCs");
                    }
                    else
                    {
                        logger.Info("  → No metrics found. Place DrawCallMetrics.csv + DrawCallParameters.csv in the session folder, or set AnalysisMetricsCSV in config.ini as fallback.");
                    }
                }

                // Export labeled + metrics CSV to captureOutDir
                string labeledCsv = reportService.GenerateLabeledMetricsCsv(report, captureOutDir);
                logger.Info($"  → CSV: {labeledCsv}");

                // ── Step 4: Summary report ────────────────────────────────────
                logger.Info("\nStep 4: Generating summary...");
                string summaryMd = reportService.GenerateSummaryReport(report, captureOutDir);
                logger.Info($"  → Summary: {summaryMd}");

                logger.Info("\n=== Analysis Complete ===");
            }
            catch (Exception ex)
            {
                logger.Error($"Analysis failed: {ex.Message}");
                logger.Debug($"Stack: {ex.StackTrace}");
                throw;
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private string? ResolveSpirvCrossPath()
        {
            string sdkPath = config.Get("VulkanSDKPath", "");
            if (!string.IsNullOrWhiteSpace(sdkPath))
            {
                string c = System.IO.Path.Combine(sdkPath, "Bin", "spirv-cross.exe");
                if (System.IO.File.Exists(c)) return c;
            }
            string? envSdk = Environment.GetEnvironmentVariable("VULKAN_SDK");
            if (!string.IsNullOrWhiteSpace(envSdk))
            {
                string c = System.IO.Path.Combine(envSdk, "Bin", "spirv-cross.exe");
                if (System.IO.File.Exists(c)) return c;
            }
            string local = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spirv-cross.exe");
            if (System.IO.File.Exists(local)) return local;
            return null;
        }

        /// <summary>
        /// Resolves metrics CSV path.
        /// Priority: auto-discover next to SDP > auto-discover in sdp/ subfolder > explicit config path.
        /// The config value is a fallback so that each SDP automatically uses its own matching export.
        /// </summary>
        private static string? ResolveMetricsPath(string? configFallback, string sdpPath)
        {
            // 1. Auto-discover: look for a Snapdragon Profiler metrics CSV in the same dir as the SDP
            string sdpDir = System.IO.Path.GetDirectoryName(sdpPath) ?? ".";
            foreach (string f in System.IO.Directory.GetFiles(sdpDir, "*.csv").OrderBy(x => x))
            {
                if (LooksLikeMetricsCsv(f)) return f;
            }

            // 2. Also try a sibling "sdp/" folder
            string sdpSibling = System.IO.Path.Combine(sdpDir, "..", "sdp");
            if (System.IO.Directory.Exists(sdpSibling))
            {
                foreach (string f in System.IO.Directory.GetFiles(sdpSibling, "*.csv").OrderBy(x => x))
                {
                    if (LooksLikeMetricsCsv(f)) return f;
                }
            }

            // 3. Fall back to the explicitly configured path (may be from a different capture)
            if (!string.IsNullOrEmpty(configFallback) && System.IO.File.Exists(configFallback))
                return configFallback;

            return null;
        }

        /// <summary>
        /// Reload Category + Detail labels from the most recent DrawCallAnalysis_*.csv in sessionDir.
        /// Called when AnalysisOnlyGenerateReport=true to skip LLM labeling.
        /// </summary>
        private void LoadLabelsFromCsv(DrawCallAnalysisReport report, string sessionDir)
        {
            // Find the most recently written DrawCallAnalysis_*.csv (not Summary)
            string? csvPath = null;
            if (System.IO.Directory.Exists(sessionDir))
            {
                csvPath = System.IO.Directory.GetFiles(sessionDir, "DrawCallAnalysis_*.csv")
                    .Where(f => System.IO.Path.GetFileName(f).IndexOf("Summary", StringComparison.OrdinalIgnoreCase) < 0)
                    .OrderByDescending(f => System.IO.File.GetLastWriteTime(f))
                    .FirstOrDefault();
            }

            if (csvPath == null)
            {
                logger.Info("  ⚠ No existing DrawCallAnalysis_*.csv found — labels will be empty.");
                return;
            }

            logger.Info($"  Loading labels from: {System.IO.Path.GetFileName(csvPath)}");

            // CSV header: DrawCall,Category,Detail,...
            var labelMap = new Dictionary<string, (string Cat, string Det)>();
            bool firstLine = true;
            foreach (var raw in System.IO.File.ReadAllLines(csvPath))
            {
                if (firstLine) { firstLine = false; continue; }  // skip header
                if (string.IsNullOrWhiteSpace(raw)) continue;
                // Simple split — values are quoted by Q() helper (no embedded commas)
                var cols = raw.Split(',');
                if (cols.Length < 3) continue;
                string dc  = cols[0].Trim().Trim('"');
                string cat = cols[1].Trim().Trim('"');
                string det = cols[2].Trim().Trim('"');
                if (!string.IsNullOrEmpty(dc))
                    labelMap[dc] = (cat, det);
            }

            int matched = 0;
            foreach (var dc in report.DrawCallResults)
            {
                if (labelMap.TryGetValue(dc.DrawCallNumber, out var lbl))
                {
                    dc.Label = new Models.DrawCallLabel { Category = lbl.Cat, Detail = lbl.Det };
                    matched++;
                }
            }
            logger.Info($"  → Restored labels for {matched} / {report.DrawCallResults.Count} DCs");
        }

        private static bool LooksLikeMetricsCsv(string path)
        {
            // Exclude our own labeled output files and session-dir DB-sourced CSVs
            string name = System.IO.Path.GetFileName(path);
            if (name.StartsWith("DrawCallAnalysis_", StringComparison.OrdinalIgnoreCase))
                return false;
            if (name.Equals("DrawCallMetrics.csv", StringComparison.OrdinalIgnoreCase))
                return false;
            if (name.Equals("DrawCallParameters.csv", StringComparison.OrdinalIgnoreCase))
                return false;
            try
            {
                string firstLine = System.IO.File.ReadLines(path).FirstOrDefault() ?? "";
                // Snapdragon Profiler export has both Clocks and Fragments columns
                // but NOT a DrawCall/Category/Detail header (which our output has)
                return firstLine.IndexOf("Clocks", StringComparison.OrdinalIgnoreCase) >= 0
                    && firstLine.IndexOf("Fragments", StringComparison.OrdinalIgnoreCase) >= 0
                    && firstLine.IndexOf("Category", StringComparison.OrdinalIgnoreCase) < 0;
            }
            catch { return false; }
        }
    }
}
