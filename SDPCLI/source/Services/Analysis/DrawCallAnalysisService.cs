using System;
using System.Collections.Generic;
using System.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Orchestrates full-capture DrawCall analysis.
    ///
    /// Responsibility: enumerate all DrawCalls (via DatabaseQueryService),
    /// resolve each one's resources (via DrawCallQueryService), then aggregate
    /// results into a DrawCallAnalysisReport.
    ///
    /// Layer position:  DatabaseQueryService (schema / enumeration)
    ///               →  DrawCallQueryService  (per-drawcall resolution)
    ///               →  DrawCallAnalysisService (orchestration)  ← this class
    /// </summary>
    public class DrawCallAnalysisService
    {
        private readonly DatabaseQueryService _dbQuery;
        private readonly DrawCallQueryService _drawCallQuery;
        private readonly ILogger _logger;

        public DrawCallAnalysisService(
            DatabaseQueryService dbQuery,
            DrawCallQueryService drawCallQuery,
            ILogger logger)
        {
            _dbQuery       = dbQuery;
            _drawCallQuery = drawCallQuery;
            _logger        = logger;
        }

        /// <summary>
        /// Enumerates every DrawCall in the capture, resolves its resources,
        /// and returns an aggregated report with statistics.
        /// </summary>
        public DrawCallAnalysisReport AnalyzeAllDrawCalls(string dbPath, uint captureId, int? cmdBufferFilter = null)
        {
            _logger.Info($"Analyzing DrawCalls for Capture ID: {captureId}" +
                (cmdBufferFilter == null ? "" :
                 cmdBufferFilter == 0   ? ", CommandBuffer=AUTO" :
                 $", CommandBuffer={cmdBufferFilter}"));

            var drawCallIds = _dbQuery.GetDrawCallIds(captureId, cmdBufferFilter);

            if (drawCallIds.Count == 0)
            {
                _logger.Warning("No DrawCalls found in database");
                drawCallIds = new List<string> { "1", "2", "3", "1.1", "1.1.1", "1.1.2" };
                _logger.Info("Using fallback example DrawCalls");
            }
            else
            {
                _logger.Success($"Found {drawCallIds.Count} DrawCalls in database");
            }

            var results = new List<DrawCallInfo>();
            int analyzedCount = 0;

            foreach (var dc in drawCallIds)
            {
                try
                {
                    var info = _drawCallQuery.GetDrawCallInfo(dbPath, captureId, dc);
                    if (info != null)
                    {
                        results.Add(info);
                        analyzedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Debug($"Failed to analyze DrawCall {dc}: {ex.Message}");
                }
            }

            _logger.Success($"Analyzed {analyzedCount} DrawCalls");

            return new DrawCallAnalysisReport
            {
                DrawCallResults  = results,
                Statistics       = GenerateStatistics(results),
                TotalDrawCalls   = drawCallIds.Count,
                AnalyzedDrawCalls = analyzedCount
            };
        }

        private static CaptureStatistics GenerateStatistics(List<DrawCallInfo> results)
        {
            if (results.Count == 0) return new CaptureStatistics();
            return new CaptureStatistics
            {
                TotalDrawCalls        = results.Count,
                TotalPipelines        = results.Select(r => r.PipelineID).Distinct().Count(),
                TotalTextures         = results.SelectMany(r => r.TextureIDs).Distinct().Count(),
                TotalShaders          = results.SelectMany(r => r.Shaders).Select(s => s.ShaderModuleID).Distinct().Count(),
                AvgTexturesPerDrawCall = results.Average(r => r.TextureIDs.Length)
            };
        }
    }
}
