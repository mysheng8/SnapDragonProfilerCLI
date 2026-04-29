using System;
using System.Collections.Generic;
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

                // snapshotId == 1 means "all snapshots" — resolve from directory
                List<uint> captureIds;
                if (captureId == 1)
                {
                    // Pass dbPath as hint: if the .sdp was extracted to a temp dir,
                    // snapshot_N subdirs will be there, not adjacent to the original file.
                    captureIds = ScanCaptureIds(sdpPath, dbPath);
                    if (captureIds.Count == 0)
                        throw new InvalidOperationException(
                            $"No snapshot_N directories found for: {sdpPath}");
                    _log.Info($"Analysis job (all): sdp={sdpPath} snapshots=[{string.Join(",", captureIds)}] sessionDir={sessionDir}");
                }
                else
                {
                    captureIds = new List<uint> { captureId };
                    _log.Info($"Analysis job: sdp={sdpPath} captureId={captureId} sessionDir={sessionDir}");
                }
                _log.Info($"  database: {dbPath}");
                _log.Info($"  targets requested: {requested}");

                // Resolve CmdBuffer filter from config — mirrors AnalysisMode.cs
                int cmdBufIdx = config.GetInt("AnalysisCmdBufferIndex", 0);
                int? cmdBufferFilter = cmdBufIdx >= 1 ? (int?)cmdBufIdx
                                     : cmdBufIdx == 0 ? (int?)0
                                     : null;   // -1 = all
                _log.Info(cmdBufIdx == -1 ? "  CommandBuffer filter: ALL"
                        : cmdBufIdx == 0  ? "  CommandBuffer filter: AUTO"
                        : $"  CommandBuffer filter: {cmdBufIdx}");

                AnalysisTarget completedTargets = AnalysisTarget.None;
                object? lastResult = null;

                int  totalSnapshots = captureIds.Count;

                foreach (uint cid in captureIds)
                {
                    int snapIndex = captureIds.IndexOf(cid);  // 0-based
                    int prevProgress = 0;
                    completedTargets = AnalysisTarget.None;

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

                        // Phase label includes snapshot position when processing multiple snapshots
                        string phaseLabel = totalSnapshots > 1
                            ? $"[{snapIndex + 1}/{totalSnapshots}] snapshot_{cid} / {phase}"
                            : phase;

                        // Scale per-snapshot 0-100 progress into this snapshot's slice of the whole job
                        int overallPrev = totalSnapshots > 1
                            ? (int)Math.Round((snapIndex * 100.0 + prevProgress) / totalSnapshots)
                            : prevProgress;
                        int overallEnd  = totalSnapshots > 1
                            ? (int)Math.Round((snapIndex * 100.0 + progressEnd) / totalSnapshots)
                            : progressEnd;

                        job.Phase    = phaseLabel;
                        job.Progress = overallPrev;
                        _log.Info($"  → phase '{phase}' snapshot_{cid} (progress {overallPrev}→{overallEnd}%)");

                        // Run this phase synchronously on a thread-pool thread
                        // (AnalysisPipeline has no async API — it uses Parallel.ForEach internally)
                        AnalysisTarget phaseMask     = mask;
                        AnalysisTarget doneTargets   = completedTargets;
                        uint           runCid        = cid;
                        int? runCmdBuf = cmdBufferFilter;
                        await Task.Run(() =>
                            pipeline.RunAnalysis(
                                dbPath,
                                sessionDir,
                                runCid,
                                cmdBufferFilter: runCmdBuf,
                                target:           phaseMask,
                                completedTargets: doneTargets),
                            CancellationToken.None  // don't cancel mid-pipeline; honour at boundaries
                        ).ConfigureAwait(false);

                        completedTargets |= mask;
                        job.Progress = overallEnd;
                        prevProgress = progressEnd;
                    }

                    lastResult = new
                    {
                        sdpPath    = sdpPath,
                        captureId  = cid,
                        sessionDir = sessionDir,
                        captureDir = System.IO.Path.Combine(sessionDir, $"snapshot_{cid}"),
                        targets    = completedTargets.ToString(),
                    };
                }

                // ── Done ─────────────────────────────────────────────────────
                job.Result = captureIds.Count == 1
                    ? lastResult
                    : new
                    {
                        sdpPath    = sdpPath,
                        captureIds = captureIds.ToArray(),
                        sessionDir = sessionDir,
                        targets    = completedTargets.ToString(),
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

        private static List<uint> ScanCaptureIds(string sdpPath, string? dbPathHint = null)
        {
            var ids = new List<uint>();
            try
            {
                // Candidate directories to search, in priority order:
                var candidates = new List<string>();

                // 1. The dir containing dbPath (covers temp-extracted .sdp archives)
                if (!string.IsNullOrEmpty(dbPathHint))
                {
                    string dbDir = System.IO.Path.GetDirectoryName(dbPathHint) ?? "";
                    if (!string.IsNullOrEmpty(dbDir) && System.IO.Directory.Exists(dbDir))
                        candidates.Add(dbDir);
                }

                // 2. sdpPath itself if it is a directory
                if (System.IO.Directory.Exists(sdpPath))
                    candidates.Add(sdpPath);

                // 3. Adjacent directory with same stem (sdp extracted beside the file)
                string adjacent = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(sdpPath) ?? "",
                    System.IO.Path.GetFileNameWithoutExtension(sdpPath));
                if (System.IO.Directory.Exists(adjacent))
                    candidates.Add(adjacent);

                foreach (string searchDir in candidates)
                {
                    foreach (var dir in System.IO.Directory.GetDirectories(searchDir, "snapshot_*"))
                    {
                        string name = System.IO.Path.GetFileName(dir);
                        if (uint.TryParse(name.Substring("snapshot_".Length), out uint id))
                            if (!ids.Contains(id)) ids.Add(id);
                    }
                    if (ids.Count > 0) break;  // found some — stop searching
                }
            }
            catch (Exception ex) { _log.Warning($"Could not scan snapshots: {ex.Message}"); }
            ids.Sort();
            return ids;
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
