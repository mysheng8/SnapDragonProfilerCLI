using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Generates report.json for a single capture — a compact, machine-readable
    /// digest of the full DrawCallAnalysis_Summary_*.md report.
    ///
    /// Fields:
    ///   frame          — aggregate frame-level stats
    ///   category_budget — clock cost breakdown by DC category
    ///   bottlenecks    — rule-derived tags (alu_bound, overdraw, …)
    ///   key_findings   — string list computed from data
    ///   suggestions    — extracted from LLM section of the summary md
    ///   top_drawcalls  — top-N DCs by clock cost with per-DC bottleneck tags
    /// </summary>
    public class CaptureReportService
    {
        private readonly ILogger _logger;

        public CaptureReportService(ILogger logger)
        {
            _logger = logger;
        }

        // ── Bottleneck tag constants ──────────────────────────────────────────
        private const string TAG_ALU_BOUND           = "alu_bound";
        private const string TAG_FRAGMENT_HEAVY      = "fragment_shader_heavy";
        private const string TAG_TEXTURE_BANDWIDTH   = "texture_bandwidth";
        private const string TAG_TEX_FETCH_STALL     = "tex_fetch_stall";
        private const string TAG_OVERDRAW            = "overdraw";
        private const string TAG_HIGH_DC_COUNT       = "high_drawcall_count";
        private const string TAG_MEMORY_WRITE        = "high_write_bandwidth";
        private const string TAG_VERTEX_HEAVY        = "vertex_shader_heavy";

        // ── Thresholds (relative) ─────────────────────────────────────────────
        private const double BUSY_BOUND_THRESHOLD    = 80.0;    // %ShaderBusy absolute
        private const double STALL_THRESHOLD         = 5.0;     // %TexFetchStall absolute
        private const double FRAGS_MULT_THRESHOLD    = 10.0;    // ×avg
        private const double READ_MULT_THRESHOLD     = 3.0;     // ×avg
        private const double WRITE_MULT_THRESHOLD    = 3.0;     // ×avg
        private const double FRAG_INSTR_MULT_THRESHOLD = 10.0;  // ×avg
        private const double VERT_INSTR_MULT_THRESHOLD = 5.0;   // ×avg
        private const int    HIGH_DC_PER_CAT         = 200;

        public string GenerateReport(
            DrawCallAnalysisReport report,
            string captureOutDir,   // snapshot_N/ absolute path
            string? summaryMdPath,  // path to the most-recent Summary md (may be null)
            int topN = 5)
        {
            var dcs         = report.DrawCallResults;
            var withMetrics = dcs.Where(d => d.Metrics != null).ToList();
            bool hasMetrics = withMetrics.Count > 0;

            // ── Global frame stats ────────────────────────────────────────────
            long   totalClocks   = withMetrics.Sum(d => d.Metrics!.Clocks);
            double avgClocks     = hasMetrics ? withMetrics.Average(d => d.Metrics!.Clocks) : 0;
            double avgFrags      = hasMetrics ? withMetrics.Average(d => d.Metrics!.FragmentsShaded) : 0;
            double avgBusy       = hasMetrics ? withMetrics.Average(d => d.Metrics!.ShadersBusyPct) : 0;
            double avgStall      = hasMetrics ? withMetrics.Average(d => d.Metrics!.TexFetchStallPct) : 0;
            double avgReadBytes  = hasMetrics ? withMetrics.Average(d => d.Metrics!.ReadTotalBytes) : 0;
            double avgWriteBytes = hasMetrics ? withMetrics.Average(d => d.Metrics!.WriteTotalBytes) : 0;
            double avgFInstr     = hasMetrics ? withMetrics.Average(d => d.Metrics!.FragmentInstructions) : 0;
            double avgVInstr     = hasMetrics ? withMetrics.Average(d => d.Metrics!.VertexInstructions) : 0;

            // ── Top N by clocks ───────────────────────────────────────────────
            var topDcs = hasMetrics
                ? withMetrics.OrderByDescending(d => d.Metrics!.Clocks).Take(topN).ToList()
                : dcs.Take(topN).ToList();

            // ── Category budget ───────────────────────────────────────────────
            var catGroups = withMetrics
                .GroupBy(d => d.Label.Category)
                .Select(g => new
                {
                    name      = g.Key,
                    drawcalls = g.Count(),
                    clocks    = g.Sum(d => d.Metrics!.Clocks),
                    pct       = totalClocks > 0 ? Math.Round(g.Sum(d => d.Metrics!.Clocks) * 100.0 / totalClocks, 1) : 0.0
                })
                .OrderByDescending(x => x.clocks)
                .ToList<object>();

            // ── Frame-level bottleneck tags ───────────────────────────────────
            var frameTags = new HashSet<string>();

            if (hasMetrics)
            {
                double topAvgBusy = topDcs.Where(d => d.Metrics != null)
                    .Select(d => d.Metrics!.ShadersBusyPct).DefaultIfEmpty(0).Average();
                if (topAvgBusy > BUSY_BOUND_THRESHOLD)
                    frameTags.Add(TAG_ALU_BOUND);

                double topAvgFInstr = topDcs.Where(d => d.Metrics != null)
                    .Select(d => (double)d.Metrics!.FragmentInstructions).DefaultIfEmpty(0).Average();
                if (avgFInstr > 0 && topAvgFInstr / avgFInstr > FRAG_INSTR_MULT_THRESHOLD)
                    frameTags.Add(TAG_FRAGMENT_HEAVY);

                double topAvgRead = topDcs.Where(d => d.Metrics != null)
                    .Select(d => (double)d.Metrics!.ReadTotalBytes).DefaultIfEmpty(0).Average();
                if (avgReadBytes > 0 && topAvgRead / avgReadBytes > READ_MULT_THRESHOLD)
                    frameTags.Add(TAG_TEXTURE_BANDWIDTH);

                bool anyStall = topDcs.Any(d => d.Metrics != null && d.Metrics!.TexFetchStallPct > STALL_THRESHOLD);
                if (anyStall) frameTags.Add(TAG_TEX_FETCH_STALL);

                bool anyOverdraw = topDcs.Any(d => d.Metrics != null
                    && avgFrags > 0 && d.Metrics!.FragmentsShaded / avgFrags > FRAGS_MULT_THRESHOLD);
                if (anyOverdraw) frameTags.Add(TAG_OVERDRAW);

                bool highDcCat = withMetrics.GroupBy(d => d.Label.Category)
                    .Any(g => g.Count() >= HIGH_DC_PER_CAT);
                if (highDcCat) frameTags.Add(TAG_HIGH_DC_COUNT);

                double topAvgVInstr = topDcs.Where(d => d.Metrics != null)
                    .Select(d => (double)d.Metrics!.VertexInstructions).DefaultIfEmpty(0).Average();
                if (avgVInstr > 0 && topAvgVInstr / avgVInstr > VERT_INSTR_MULT_THRESHOLD)
                    frameTags.Add(TAG_VERTEX_HEAVY);
            }

            // ── Key findings ──────────────────────────────────────────────────
            var findings = new List<string>();
            if (hasMetrics && topDcs.Count > 0)
            {
                var top1 = topDcs[0];
                double top5pct = totalClocks > 0
                    ? topDcs.Sum(d => d.Metrics?.Clocks ?? 0) * 100.0 / totalClocks : 0;
                findings.Add($"Top {topDcs.Count} DCs account for {top5pct:F1}% of total frame clocks ({totalClocks:N0})");

                if (top1.Metrics != null)
                    findings.Add($"Heaviest DC {top1.DrawCallNumber} ({top1.Label.Category}): {top1.Metrics.Clocks:N0} clocks " +
                                 $"({(avgClocks > 0 ? top1.Metrics.Clocks / avgClocks : 0):F1}× avg), " +
                                 $"%ShaderBusy={top1.Metrics.ShadersBusyPct:F1}%");

                // dominant category
                var topCat = withMetrics.GroupBy(d => d.Label.Category)
                    .OrderByDescending(g => g.Sum(x => x.Metrics!.Clocks)).FirstOrDefault();
                if (topCat != null)
                {
                    double catPct = totalClocks > 0
                        ? topCat.Sum(x => x.Metrics!.Clocks) * 100.0 / totalClocks : 0;
                    findings.Add($"{topCat.Key} is the dominant category: {catPct:F1}% of clocks, {topCat.Count()} DCs");
                }

                // global texture miss
                double globalL1 = withMetrics.Average(d => d.Metrics!.TexL1MissPct);
                findings.Add($"Global avg %%TexL1Miss={globalL1:F1}%, %%ShaderBusy={avgBusy:F1}%, %%TexFetchStall={avgStall:F1}%");

                // any stall hotspot
                var stallDc = topDcs.Where(d => d.Metrics != null && d.Metrics.TexFetchStallPct > STALL_THRESHOLD)
                    .OrderByDescending(d => d.Metrics!.TexFetchStallPct).FirstOrDefault();
                if (stallDc != null)
                    findings.Add($"DC {stallDc.DrawCallNumber} has high texture fetch stall: {stallDc.Metrics!.TexFetchStallPct:F1}%");
            }

            // ── Suggestions extracted from summary md ─────────────────────────
            var suggestions = ExtractSuggestionsFromMd(summaryMdPath);

            // ── Per-DC top list ───────────────────────────────────────────────
            var topDcNodes = new List<object>();
            for (int i = 0; i < topDcs.Count; i++)
            {
                var dc = topDcs[i];
                var m  = dc.Metrics;
                var tags = BuildDcTags(m, avgClocks, avgFrags, avgBusy, avgReadBytes,
                                       avgWriteBytes, avgFInstr, avgVInstr);
                topDcNodes.Add(new
                {
                    rank        = i + 1,
                    dc_id       = dc.DrawCallNumber,
                    category    = dc.Label.Category,
                    detail      = dc.Label.Detail,
                    clocks      = m?.Clocks ?? 0,
                    clocks_vs_avg = avgClocks > 0 && m != null
                        ? Math.Round(m.Clocks / avgClocks, 1) : 0.0,
                    shader_busy_pct       = m != null ? Math.Round(m.ShadersBusyPct,       1) : 0.0,
                    fragments             = m?.FragmentsShaded ?? 0,
                    fragment_instructions = m?.FragmentInstructions ?? 0,
                    tex_fetch_stall_pct   = m != null ? Math.Round(m.TexFetchStallPct, 1) : 0.0,
                    read_mb    = m != null ? Math.Round(m.ReadTotalBytes  / 1048576.0, 2) : 0.0,
                    write_mb   = m != null ? Math.Round(m.WriteTotalBytes / 1048576.0, 2) : 0.0,
                    bottleneck_tags = tags,
                });
            }

            // ── Relative paths helper ─────────────────────────────────────────
            string captureSubName = Path.GetFileName(
                captureOutDir.TrimEnd(Path.DirectorySeparatorChar));
            string? screenshotRel = File.Exists(Path.Combine(captureOutDir, "snapshot.png"))
                ? $"{captureSubName}/snapshot.png" : null;
            string? summaryRel = summaryMdPath != null && File.Exists(summaryMdPath)
                ? $"{captureSubName}/{Path.GetFileName(summaryMdPath)}" : null;

            // ── Assemble root ─────────────────────────────────────────────────
            var root = new
            {
                schema_version = "1.0",
                generated_at   = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                capture_index  = captureSubName,
                source_summary = summaryRel,
                screenshot     = screenshotRel,

                frame = new
                {
                    total_drawcalls        = dcs.Count,
                    total_clocks           = totalClocks,
                    avg_clocks_per_dc      = (long)Math.Round(avgClocks),
                    avg_shader_busy_pct    = Math.Round(avgBusy,  2),
                    avg_tex_fetch_stall_pct = Math.Round(avgStall, 2),
                    avg_read_mb            = Math.Round(avgReadBytes  / 1048576.0, 3),
                    avg_fragments          = (long)Math.Round(avgFrags),
                },

                category_budget = catGroups,
                bottlenecks     = frameTags.OrderBy(t => t).ToList(),
                key_findings    = findings,
                suggestions     = suggestions,
                top_drawcalls   = topDcNodes,
            };

            // ── Write ─────────────────────────────────────────────────────────
            var settings = new JsonSerializerSettings
            {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            string json    = JsonConvert.SerializeObject(root, settings);
            string outPath = Path.Combine(captureOutDir, "report.json");
            File.WriteAllText(outPath, json, Encoding.UTF8);
            _logger.Info($"  → report.json: {outPath}");
            return outPath;
        }

        // ── Per-DC tag builder ────────────────────────────────────────────────
        private static List<string> BuildDcTags(
            DrawCallMetrics? m,
            double avgClocks, double avgFrags, double avgBusy,
            double avgReadBytes, double avgWriteBytes,
            double avgFInstr, double avgVInstr)
        {
            var tags = new List<string>();
            if (m == null) return tags;

            if (m.ShadersBusyPct > BUSY_BOUND_THRESHOLD)
                tags.Add(TAG_ALU_BOUND);
            if (avgFInstr > 0 && m.FragmentInstructions / avgFInstr > FRAG_INSTR_MULT_THRESHOLD)
                tags.Add(TAG_FRAGMENT_HEAVY);
            if (avgReadBytes > 0 && m.ReadTotalBytes / avgReadBytes > READ_MULT_THRESHOLD)
                tags.Add(TAG_TEXTURE_BANDWIDTH);
            if (m.TexFetchStallPct > STALL_THRESHOLD)
                tags.Add(TAG_TEX_FETCH_STALL);
            if (avgFrags > 0 && m.FragmentsShaded / avgFrags > FRAGS_MULT_THRESHOLD)
                tags.Add(TAG_OVERDRAW);
            if (avgWriteBytes > 0 && m.WriteTotalBytes / avgWriteBytes > WRITE_MULT_THRESHOLD)
                tags.Add(TAG_MEMORY_WRITE);
            if (avgVInstr > 0 && m.VertexInstructions / avgVInstr > VERT_INSTR_MULT_THRESHOLD)
                tags.Add(TAG_VERTEX_HEAVY);

            return tags;
        }

        // ── Extract numbered suggestions from the md "综合优化建议（LLM）" block ───
        private List<string> ExtractSuggestionsFromMd(string? mdPath)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(mdPath) || !File.Exists(mdPath))
                return result;
            try
            {
                string text  = File.ReadAllText(mdPath, Encoding.UTF8);
                // Find the block after "综合优化建议" marker
                int markerIdx = text.IndexOf("综合优化建议", StringComparison.Ordinal);
                if (markerIdx < 0) return result;
                string block = text.Substring(markerIdx);

                // Match lines like: > 1.  **Title:** body  or simple > N. text
                var regex = new Regex(
                    @"^>\s*\d+\.\s+\*\*(.+?)\*\*[：:]?\s*(.*)",
                    RegexOptions.Multiline);

                foreach (Match m in regex.Matches(block))
                {
                    string title = m.Groups[1].Value.Trim();
                    string body  = m.Groups[2].Value.Trim();
                    string line  = string.IsNullOrEmpty(body) ? title : $"{title}: {body}";
                    result.Add(line);
                }

                // Fallback: plain "> N. text" without bold markers
                if (result.Count == 0)
                {
                    var plain = new Regex(@"^>\s*\d+\.\s+(.+)", RegexOptions.Multiline);
                    foreach (Match m in plain.Matches(block))
                        result.Add(m.Groups[1].Value.Trim());
                }
            }
            catch (Exception ex)
            {
                _logger.Info($"  ⚠ CaptureReportService: could not parse suggestions from md: {ex.Message}");
            }
            return result;
        }
    }
}
