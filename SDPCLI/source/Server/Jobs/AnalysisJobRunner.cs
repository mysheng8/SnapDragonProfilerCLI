using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnapdragonProfilerCLI.Analysis;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Services.Analysis;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    /// <summary>
    /// Executes the analysis pipeline phase-by-phase for server mode.
    ///
    /// Each phase = one AnalysisTarget flag group → one RunAnalysis() call.
    /// CancellationToken is checked at phase boundaries; current phase always completes.
    ///
    /// completedTargets accumulates after each phase so downstream phases can skip
    /// re-running expensive steps (e.g. label phase will not re-invoke LLM).
    /// </summary>
    public static class AnalysisJobRunner
    {
        private static readonly ContextLogger _log = new ContextLogger("AnalysisJob");

        private static readonly (string Phase, AnalysisTarget Mask, int ProgressEnd)[] ExecutionPlan =
        {
            ("collect_dc",      AnalysisTarget.Dc,                                                   12),
            ("extract_assets",  AnalysisTarget.Shaders | AnalysisTarget.Textures | AnalysisTarget.Buffers, 42),
            ("label_drawcalls", AnalysisTarget.Label,                                                65),
            ("join_metrics",    AnalysisTarget.Metrics,                                              75),
            ("generate_stats",  AnalysisTarget.Status | AnalysisTarget.TopDc,                       85),
            ("report_llm",      AnalysisTarget.Analysis,                                             95),
            ("dashboard",       AnalysisTarget.Dashboard,                                           100),
        };

        public static async Task RunAsync(
            AnalysisJob job,
            Config config,
            CancellationToken ct)
        {
            try
            {
                string sdpPath    = job.SdpPath;
                uint   captureId  = job.SnapshotId;
                string outputDir  = job.OutputDir ?? "";
                AnalysisTarget requested = job.TargetsEnum;

                // Build the pipeline (same pattern as Application.cs)
                var pipeline = BuildPipeline(config);

                // Resolve DB path from .sdp archive / adjacent directory
                var sdpFileService = new SdpFileService(config, _log);
                string? dbPath = sdpFileService.FindDatabasePath(sdpPath);
                if (dbPath == null)
                    throw new InvalidOperationException(
                        $"Cannot locate sdp.db for: {sdpPath}. " +
                        "Ensure the .sdp archive or adjacent session directory exists.");

                // Determine effective session directory (analysis output root)
                string sessionDir = string.IsNullOrWhiteSpace(outputDir)
                    ? ResolveDefaultSessionDir(sdpPath, config)
                    : outputDir;

                _log.Info($"Analysis job: sdp={sdpPath} captureId={captureId} sessionDir={sessionDir}");
                _log.Info($"  database: {dbPath}");
                _log.Info($"  targets requested: {requested}");

                AnalysisTarget completedTargets = AnalysisTarget.None;
                int prevProgress = 0;

                foreach (var (phase, mask, progressEnd) in ExecutionPlan)
                {
                    // ── Phase boundary: check CancellationToken ────────────
                    if (ct.IsCancellationRequested)
                    {
                        _log.Info($"  ← cancelled before phase '{phase}'");
                        throw new OperationCanceledException(ct);
                    }

                    // Skip phases not required by the requested target set
                    // (ExpandWithDependencies is applied inside RunAnalysis, but we guard here
                    //  to avoid unnecessary DB open when the phase is clearly not needed)
                    AnalysisTarget requestedEffective = requested.ExpandWithDependencies();
                    bool phaseNeeded = (requested == AnalysisTarget.All)
                                    || (requestedEffective & mask) != AnalysisTarget.None;
                    if (!phaseNeeded)
                    {
                        _log.Info($"  phase '{phase}' skipped (not in requested target)");
                        completedTargets |= mask;
                        continue;
                    }

                    job.Phase    = phase;
                    job.Progress = prevProgress;
                    _log.Info($"  → phase '{phase}' (progress {prevProgress}→{progressEnd}%)");

                    // Run this phase synchronously on a thread-pool thread
                    // (AnalysisPipeline has no async API — it uses Parallel.ForEach internally)
                    AnalysisTarget phaseMask     = mask;
                    AnalysisTarget doneTargets   = completedTargets;
                    await Task.Run(() =>
                        pipeline.RunAnalysis(
                            dbPath,
                            sessionDir,
                            captureId,
                            cmdBufferFilter: null,
                            target:           phaseMask,
                            completedTargets: doneTargets),
                        CancellationToken.None  // don't cancel mid-pipeline; honour at boundaries
                    ).ConfigureAwait(false);

                    completedTargets |= mask;
                    job.Progress = progressEnd;
                    prevProgress = progressEnd;
                }

                // ── Done ─────────────────────────────────────────────────────
                string captureOutDir = System.IO.Path.Combine(sessionDir, $"snapshot_{captureId}");
                job.Result = new
                {
                    sdpPath      = sdpPath,
                    captureId    = captureId,
                    sessionDir   = sessionDir,
                    captureDir   = captureOutDir,
                    targets      = completedTargets.ToString(),
                };
                job.Status   = JobStatus.Completed;
                job.Progress = 100;
                _log.Info("Analysis job completed");
            }
            catch (OperationCanceledException)
            {
                _log.Warning("Analysis job cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _log.Error($"Analysis job failed: {ex.Message}");
                throw;
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static AnalysisPipeline BuildPipeline(Config config)
        {
            var logger         = new ContextLogger("AnalysisPipeline");
            var sdpFileSvc     = new SdpFileService(config, logger);
            var analysisSvc    = new DrawCallAnalysisService(logger);
            var reportSvc      = new RawJsonGenerationService(config, logger);
            var llmService     = new LlmApiWrapper(config, logger);
            var categories     = config.Get("AnalysisCategories", "Scene,PostProcess,UI,Other")
                                       .Split(',').Select(s => s.Trim()).ToList();
            int maxShaderChars  = config.GetInt("LlmMaxShaderChars", 4000);
            var labelService   = new DrawCallLabelService(categories, llmService, logger, maxShaderChars);
            var metricsService = new MetricsQueryService(config);

            return new AnalysisPipeline(
                sdpFileSvc, analysisSvc, reportSvc, labelService,
                metricsService, config, logger, llmService);
        }

        private static string ResolveDefaultSessionDir(string sdpPath, Config config)
        {
            // Mirror the logic in AnalysisMode.ResolveOutputDir: use sdpPath stem under AnalysisDir
            string analysisDir = config.Get("AnalysisDir", "");
            if (string.IsNullOrWhiteSpace(analysisDir))
                analysisDir = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(sdpPath) ?? ".", "analysis");

            string stem = System.IO.Path.GetFileNameWithoutExtension(sdpPath);
            return System.IO.Path.Combine(analysisDir, stem);
        }
    }
}
