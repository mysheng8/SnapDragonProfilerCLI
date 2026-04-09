using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Pass B Step B3 — rule-based visual dashboard (no LLM).
    ///
    /// Extracts the chart and table content from the analysis into a dedicated
    /// snapshot_{id}_dashboard.md file. Content is purely deterministic:
    ///   - Frame snapshot image
    ///   - DC category distribution table + Mermaid bar chart + pie chart
    ///   - 4.1 Top-N global table (dynamic outlier columns)
    ///   - 4.1b Per-category Top-5 tables
    ///   - 3D Mesh preview links
    ///   - 4.2 Category statistics table
    ///   - Label quality summary
    ///   - Clocks breakdown summary
    ///
    /// This replaces the chart/table portion previously embedded in
    /// ReportGenerationService.GenerateSummaryReport().
    /// </summary>
    public class DashboardGenerationService
    {
        private readonly Config  _config;
        private readonly ILogger _logger;

        // Mirrors private CatStats from ReportGenerationService
        private class CatStats
        {
            public int    Count       { get; set; }
            public long   TotalClocks { get; set; }
            public double AvgClocks   { get; set; }
            public double AvgRead     { get; set; }
            public double AvgWrite    { get; set; }
            public double AvgFrags    { get; set; }
            public double AvgBusy     { get; set; }
            public double AvgL1       { get; set; }
            public double AvgStall    { get; set; }
            public double AvgFInstr   { get; set; }
        }

        public DashboardGenerationService(Config config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Generates snapshot_{captureId}_dashboard.md in captureOutDir.
        /// Returns the path of the written file.
        /// </summary>
        public string GenerateDashboard(
            DrawCallAnalysisReport report,
            string captureOutDir,
            uint   captureId = 0,
            string sdpName   = "")
        {
            var sb   = new StringBuilder();
            var dcs  = report.DrawCallResults;
            bool hasMetrics = dcs.Any(d => d.Metrics != null);

            sb.AppendLine($"# {sdpName} — 帧分析 Dashboard");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}  ");
            sb.AppendLine($"Total DrawCalls: {dcs.Count}  ");
            sb.AppendLine();

            // ── Frame Snapshot ────────────────────────────────────────────────
            AppendSnapshot(sb, captureOutDir);

            // ── Category overview table ───────────────────────────────────────
            var catGroups = dcs.GroupBy(d => d.Label.Category)
                               .OrderByDescending(g => g.Count()).ToList();
            sb.AppendLine("## DrawCall 分布\n");
            sb.AppendLine("| 分类 | DC 数 | 占比 |");
            sb.AppendLine("|------|------:|----:|");
            foreach (var g in catGroups)
                sb.AppendLine($"| {g.Key} | {g.Count()} | {g.Count() * 100.0 / dcs.Count:F1}% |");
            sb.AppendLine();

            if (!hasMetrics)
            {
                sb.AppendLine("> 未加载 Metrics CSV，以下图表不可用。");
                return WriteFile(sb, captureOutDir, captureId, sdpName);
            }

            var withM          = dcs.Where(d => d.Metrics != null).ToList();
            double globalAvgClocks = withM.Average(d => (double)d.Metrics!.Clocks);
            double globalAvgRead   = withM.Average(d => (double)d.Metrics!.ReadTotalBytes);
            double globalAvgWrite  = withM.Average(d => (double)d.Metrics!.WriteTotalBytes);
            double globalAvgFrags  = withM.Average(d => (double)d.Metrics!.FragmentsShaded);
            double globalAvgBusy   = withM.Average(d => d.Metrics!.ShadersBusyPct);
            double globalAvgL1     = withM.Average(d => d.Metrics!.TexL1MissPct);
            double globalAvgStall  = withM.Average(d => d.Metrics!.TexFetchStallPct);
            double globalAvgFInstr = withM.Average(d => (double)d.Metrics!.FragmentInstructions);
            double globalAvgVert   = withM.Average(d => (double)d.Metrics!.VerticesShaded);
            long   totalClocks     = withM.Sum(d => d.Metrics!.Clocks);

            // Per-category stats
            var catStats = new Dictionary<string, CatStats>();
            foreach (var g in catGroups)
            {
                var gm = g.Where(d => d.Metrics != null).ToList();
                if (gm.Count == 0) continue;
                catStats[g.Key] = new CatStats
                {
                    Count       = g.Count(),
                    TotalClocks = gm.Sum(d => d.Metrics!.Clocks),
                    AvgClocks   = gm.Average(d => (double)d.Metrics!.Clocks),
                    AvgRead     = gm.Average(d => (double)d.Metrics!.ReadTotalBytes),
                    AvgWrite    = gm.Average(d => (double)d.Metrics!.WriteTotalBytes),
                    AvgFrags    = gm.Average(d => (double)d.Metrics!.FragmentsShaded),
                    AvgBusy     = gm.Average(d => d.Metrics!.ShadersBusyPct),
                    AvgL1       = gm.Average(d => d.Metrics!.TexL1MissPct),
                    AvgStall    = gm.Average(d => d.Metrics!.TexFetchStallPct),
                    AvgFInstr   = gm.Average(d => (double)d.Metrics!.FragmentInstructions),
                };
            }

            var top5 = withM.OrderByDescending(d => d.Metrics!.Clocks).Take(5).ToList();

            // ── Mermaid bar chart ─────────────────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine($"## GPU Clock 分布（{withM.Count} DC）\n");
            {
                var allSorted = withM.OrderBy(d => d.DrawCallNumber).ToList();
                long chartMax = allSorted.Max(d => d.Metrics!.Clocks);
                var xLabels   = allSorted.Select(d => $"\"{d.DrawCallNumber}\"");
                var yVals     = allSorted.Select(d => d.Metrics!.Clocks.ToString());
                sb.AppendLine("```mermaid");
                sb.AppendLine($"%%{{init: {{\"xyChart\": {{\"width\": 1600, \"height\": 420, \"xAxis\": {{\"labelFontSize\": 1, \"labelPadding\": 0}}}}}}}}%%");
                sb.AppendLine("xychart-beta");
                sb.AppendLine($"    title \"GPU Clocks per DrawCall ({withM.Count} DCs)\"");
                sb.AppendLine($"    x-axis [{string.Join(", ", xLabels)}]");
                sb.AppendLine($"    y-axis \"Clocks\" 0 --> {chartMax}");
                sb.AppendLine($"    bar [{string.Join(", ", yVals)}]");
                sb.AppendLine("```");
                sb.AppendLine();
            }

            // ── Mermaid pie charts ────────────────────────────────────────────
            sb.AppendLine("## GPU Clock Budget by Category\n");
            if (catStats.Count > 0)
            {
                sb.AppendLine("```mermaid");
                sb.AppendLine("pie title \"GPU Clock Budget by Category\"");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                    sb.AppendLine($"    \"{kv.Key}\" : {kv.Value.TotalClocks}");
                sb.AppendLine("```");
                sb.AppendLine();
                sb.AppendLine("```mermaid");
                sb.AppendLine("pie title \"DrawCall Count by Category\"");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.Count))
                    sb.AppendLine($"    \"{kv.Key}\" : {kv.Value.Count}");
                sb.AppendLine("```");
                sb.AppendLine();
                sb.AppendLine("| 分类 | DC Count | Total Clocks | % of Frame |");
                sb.AppendLine("|------|----------:|-------------:|-----------:|");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                    sb.AppendLine($"| {kv.Key} | {kv.Value.Count} | {kv.Value.TotalClocks:N0} | {kv.Value.TotalClocks * 100.0 / Math.Max(totalClocks, 1):F1}% |");
                sb.AppendLine();
            }

            // ── 4.1 Top-5 table (dynamic outlier columns) ─────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## Top 5 DrawCalls by GPU Clock Cost\n");
            sb.AppendLine($"> 全局均值（{withM.Count} DC）：Clocks={globalAvgClocks:N0} | Fragments={globalAvgFrags:N0} | ShaderBusy={globalAvgBusy:F1}% | FragInstr={globalAvgFInstr:N0} | TexStall={globalAvgStall:F1}% | TexL1Miss={globalAvgL1:F1}% | Read={globalAvgRead/1048576.0:F2}MB | Write={globalAvgWrite/1048576.0:F2}MB\n");
            AppendDynamicTop5Table(sb, top5, catStats, withM,
                globalAvgClocks, globalAvgFrags, globalAvgBusy, globalAvgFInstr,
                globalAvgStall, globalAvgL1, globalAvgRead, globalAvgWrite, globalAvgVert, totalClocks);

            // ── 4.1b Per-category Top-5 ───────────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 各分类 Top 5 DrawCalls\n");
            foreach (var catGroup in catGroups)
            {
                var catWithM = catGroup.Where(d => d.Metrics != null)
                                       .OrderByDescending(d => d.Metrics!.Clocks)
                                       .Take(5).ToList();
                if (catWithM.Count == 0) continue;
                double catAvg = catGroup.Where(d => d.Metrics != null)
                                        .Average(d => (double)d.Metrics!.Clocks);
                sb.AppendLine($"### {catGroup.Key}\n");
                sb.AppendLine("| Rank | DC | Detail | Clocks | %ShaderBusy | Fragments | ReadMB | WriteMB |");
                sb.AppendLine("|-----:|-----|--------|-------:|------------:|----------:|-------:|--------:|");
                int catRank = 0;
                foreach (var dc in catWithM)
                {
                    var m = dc.Metrics!;
                    sb.AppendLine($"| {++catRank} | {dc.DrawCallNumber} | {dc.Label.Detail} | "
                        + $"{m.Clocks:N0} ({m.Clocks / Math.Max(catAvg, 1):F1}×) | {m.ShadersBusyPct:F1}% | "
                        + $"{m.FragmentsShaded:N0} | {m.ReadTotalBytes/1048576.0:F2} | {m.WriteTotalBytes/1048576.0:F2} |");
                }
                sb.AppendLine();
            }

            // ── 3D Mesh Preview ───────────────────────────────────────────────
            AppendMeshPreview(sb, captureOutDir);

            // ── 4.2 Category statistics table ─────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## Category Statistics\n");
            sb.AppendLine("| Category | Count | TotalClocks | AvgClocks | AvgReadMB | AvgWriteMB | AvgFragments | AvgShaderBusy% |");
            sb.AppendLine("|----------|------:|------------:|----------:|----------:|-----------:|-------------:|---------------:|");
            foreach (var g in catGroups)
            {
                if (!catStats.TryGetValue(g.Key, out var st))
                { sb.AppendLine($"| {g.Key} | {g.Count()} | — | — | — | — | — | — |"); continue; }
                sb.AppendLine($"| {g.Key} | {g.Count()} | {st.TotalClocks:N0} | {st.AvgClocks:N0} | {st.AvgRead/1048576.0:F2} | {st.AvgWrite/1048576.0:F2} | {st.AvgFrags:N0} | {st.AvgBusy:F1}% |");
            }
            sb.AppendLine();
            sb.AppendLine($"> **全局平均:** Clocks={globalAvgClocks:N0}  Read={globalAvgRead/1048576.0:F2}MB  Write={globalAvgWrite/1048576.0:F2}MB  Fragments={globalAvgFrags:N0}  ShaderBusy={globalAvgBusy:F1}%");
            sb.AppendLine();

            // ── Label quality stats (from status.json if available) ───────────
            AppendLabelStats(sb, captureOutDir, captureId);

            // ── Clocks summary ────────────────────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 帧汇总\n");
            sb.AppendLine($"- 总 Clock 消耗: **{totalClocks:N0}**");
            sb.AppendLine($"- Top 5 占比: **{top5.Sum(d => d.Metrics!.Clocks) * 100.0 / Math.Max(totalClocks, 1):F1}%**\n");
            sb.AppendLine("**各类别 Clock 占比：**\n");
            foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                sb.AppendLine($"- {kv.Key}: {kv.Value.TotalClocks:N0} clocks  ({kv.Value.TotalClocks * 100.0 / Math.Max(totalClocks, 1):F1}%)，共 {kv.Value.Count} 个 DC");
            sb.AppendLine();

            return WriteFile(sb, captureOutDir, captureId, sdpName);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Section helpers
        // ──────────────────────────────────────────────────────────────────────

        private void AppendSnapshot(StringBuilder sb, string captureOutDir)
        {
            try
            {
                string bmpPath = Path.Combine(captureOutDir, "1_screenshot.bmp");
                if (!File.Exists(bmpPath)) return;

                string pngPath = Path.Combine(captureOutDir, "snapshot.png");
                if (!File.Exists(pngPath))
                {
                    bool rotate = _config.GetBool("SnapshotRotateLandscape", true);
                    using var bmp = new System.Drawing.Bitmap(bmpPath);
                    using var out2 = rotate ? RotateBitmap90CW(bmp) : (System.Drawing.Bitmap)bmp.Clone();
                    out2.Save(pngPath, System.Drawing.Imaging.ImageFormat.Png);
                }

                sb.AppendLine("## Screenshot\n");
                sb.AppendLine("![Frame Snapshot](snapshot.png)\n");
            }
            catch { /* silent */ }
        }

        private static System.Drawing.Bitmap RotateBitmap90CW(System.Drawing.Bitmap src)
        {
            var r = new System.Drawing.Bitmap(src.Height, src.Width);
            using var g = System.Drawing.Graphics.FromImage(r);
            g.TranslateTransform(0, src.Width);
            g.RotateTransform(-90);
            g.DrawImage(src, 0, 0);
            return r;
        }

        private static void AppendMeshPreview(StringBuilder sb, string captureOutDir)
        {
            string meshDir = Path.Combine(captureOutDir, "meshes");
            if (!Directory.Exists(meshDir)) return;
            var objs = Directory.GetFiles(meshDir, "*.obj").OrderBy(f => f).ToList();
            if (objs.Count == 0) return;

            sb.AppendLine("---\n");
            sb.AppendLine("## 3D Mesh Preview\n");
            sb.AppendLine("> 交互式查看器（需要浏览器打开）\n");
            sb.AppendLine($"**[🔗 Open interactive 3D Viewer](meshes/viewer.html)**\n");
            sb.AppendLine("| Rank | DrawCall | OBJ |");
            sb.AppendLine("|-----:|----------|-----|");
            int n = 0;
            foreach (var f in objs)
            {
                string fname = Path.GetFileName(f);
                string dcId  = fname.Replace("drawcall_", "").Replace(".obj", "");
                sb.AppendLine($"| {++n} | DC {dcId} | [{fname}](meshes/{fname}) |");
            }
            sb.AppendLine();
        }

        private static void AppendLabelStats(StringBuilder sb, string captureOutDir, uint captureId)
        {
            string statusPath = Path.Combine(captureOutDir, $"snapshot_{captureId}_status.json");
            if (!File.Exists(statusPath)) return;

            try
            {
                var root = JObject.Parse(File.ReadAllText(statusPath, Encoding.UTF8));
                if (root["label_stats"] is not JObject ls) return;

                sb.AppendLine("---\n");
                sb.AppendLine("## Label 质量统计\n");
                double conf = ls["avg_confidence"]?.ToObject<double>() ?? 0;
                double low  = (ls["low_confidence_ratio"]?.ToObject<double>() ?? 0) * 100;
                sb.AppendLine($"- 均值置信度: **{conf:F2}**");
                sb.AppendLine($"- 低置信度（< {ls["low_confidence_threshold"]}）占比: **{low:F1}%**");
                sb.AppendLine($"- LLM 标注: **{ls["llm_labeled_count"]}**，规则标注: **{ls["rule_labeled_count"]}**");

                if (ls["reason_tag_distribution"] is JObject tags && tags.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("**Reason Tag 分布（Top 10）：**\n");
                    sb.AppendLine("| Tag | Count |");
                    sb.AppendLine("|-----|------:|");
                    foreach (var p in tags.Properties().Take(10))
                        sb.AppendLine($"| {p.Name} | {p.Value} |");
                }

                if (ls["low_confidence_dc_ids"] is JArray ids && ids.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine($"**低置信度 DC（{ids.Count} 个）：** {string.Join(", ", ids.Take(10).Select(t => t.ToString()))}");
                    if (ids.Count > 10) sb.AppendLine($"... 及另外 {ids.Count - 10} 个");
                }
                sb.AppendLine();
            }
            catch { }
        }

        private static void AppendDynamicTop5Table(
            StringBuilder sb, List<DrawCallInfo> top5,
            Dictionary<string, CatStats> catStats,
            List<DrawCallInfo> withM,
            double avgClocks, double avgFrags, double avgBusy, double avgFInstr,
            double avgStall, double avgL1, double avgRead, double avgWrite,
            double avgVert, long totalClocks)
        {
            double maxFragRatio   = top5.Max(d => d.Metrics!.FragmentsShaded     / Math.Max(avgFrags,  1.0));
            double maxBusyRatio   = top5.Max(d => d.Metrics!.ShadersBusyPct      / Math.Max(avgBusy,   0.1));
            double maxFInstrRatio = top5.Max(d => d.Metrics!.FragmentInstructions / Math.Max(avgFInstr, 1.0));
            double maxStallRatio  = top5.Max(d => d.Metrics!.TexFetchStallPct    / Math.Max(avgStall,  0.1));
            double maxL1Ratio     = top5.Max(d => d.Metrics!.TexL1MissPct        / Math.Max(avgL1,     0.1));
            double maxReadRatio   = top5.Max(d => d.Metrics!.ReadTotalBytes      / Math.Max(avgRead,   1.0));
            double maxWriteRatio  = top5.Max(d => d.Metrics!.WriteTotalBytes     / Math.Max(avgWrite,  1.0));
            double maxVertRatio   = top5.Max(d => d.Metrics!.VerticesShaded      / Math.Max(avgVert,   1.0));

            var candidateCols = new List<(string Hdr, double MaxRatio, double Thresh, double AvgVal,
                                          Func<DrawCallMetrics, double> Val,
                                          Func<DrawCallMetrics, double, string> Cell)>
            {
                ($"Fragments (avg {avgFrags:N0})",         maxFragRatio,   1.5, avgFrags,
                    m => (double)m.FragmentsShaded,
                    (m, a) => $"{m.FragmentsShaded:N0} ({m.FragmentsShaded/Math.Max(a,1):F1}×)"),
                ($"%ShaderBusy (avg {avgBusy:F1}%)",       maxBusyRatio,   1.3, avgBusy,
                    m => m.ShadersBusyPct,
                    (m, a) => $"{m.ShadersBusyPct:F1}% (+{Math.Max(0, m.ShadersBusyPct-a):F1}pp)"),
                ($"FragInstr (avg {avgFInstr:N0})",         maxFInstrRatio, 1.5, avgFInstr,
                    m => (double)m.FragmentInstructions,
                    (m, a) => $"{m.FragmentInstructions:N0} ({m.FragmentInstructions/Math.Max(a,1):F1}×)"),
                ($"%TexStall (avg {avgStall:F1}%)",         maxStallRatio,  1.5, avgStall,
                    m => m.TexFetchStallPct,
                    (m, a) => $"{m.TexFetchStallPct:F1}% (+{Math.Max(0, m.TexFetchStallPct-a):F1}pp)"),
                ($"%TexL1Miss (avg {avgL1:F1}%)",           maxL1Ratio,     1.5, avgL1,
                    m => m.TexL1MissPct,
                    (m, a) => $"{m.TexL1MissPct:F1}% (+{Math.Max(0, m.TexL1MissPct-a):F1}pp)"),
                ($"ReadMB (avg {avgRead/1048576.0:F2})",   maxReadRatio,   2.0, avgRead,
                    m => (double)m.ReadTotalBytes,
                    (m, a) => $"{m.ReadTotalBytes/1048576.0:F2} ({m.ReadTotalBytes/Math.Max(a,1):F1}×)"),
                ($"WriteMB (avg {avgWrite/1048576.0:F2})", maxWriteRatio,  2.0, avgWrite,
                    m => (double)m.WriteTotalBytes,
                    (m, a) => $"{m.WriteTotalBytes/1048576.0:F2} ({m.WriteTotalBytes/Math.Max(a,1):F1}×)"),
                ($"Vertices (avg {avgVert:N0})",            maxVertRatio,   1.5, avgVert,
                    m => (double)m.VerticesShaded,
                    (m, a) => $"{m.VerticesShaded:N0} ({m.VerticesShaded/Math.Max(a,1):F1}×)"),
            };

            var activeCols = candidateCols
                .Where(c => c.MaxRatio >= c.Thresh)
                .OrderByDescending(c => c.MaxRatio)
                .ToList();

            var hdr = new List<string> { "Rank", "DC", "Category", "Detail", $"Clocks (avg {avgClocks:N0})" };
            var sep = new List<string> { "----:", "----", "--------", "------", "-----------:" };
            foreach (var col in activeCols) { hdr.Add(col.Hdr); sep.Add("-----------:"); }
            sb.AppendLine("| " + string.Join(" | ", hdr) + " |");
            sb.AppendLine("| " + string.Join(" | ", sep) + " |");

            for (int i = 0; i < top5.Count; i++)
            {
                var d = top5[i]; var m = d.Metrics!;
                double clockRatio = m.Clocks / Math.Max(avgClocks, 1.0);
                var row = new List<string>
                {
                    (i + 1).ToString(), d.DrawCallNumber, d.Label.Category,
                    d.Label.Detail, $"{m.Clocks:N0} ({clockRatio:F1}×)"
                };
                foreach (var col in activeCols)
                {
                    double ratio = col.Val(m) / Math.Max(col.AvgVal, col.Thresh > 1.3 ? 1.0 : 0.1);
                    string cell  = col.Cell(m, col.AvgVal);
                    if (ratio >= col.Thresh) cell = $"**{cell}**";
                    row.Add(cell);
                }
                sb.AppendLine("| " + string.Join(" | ", row) + " |");
            }
            sb.AppendLine();
        }

        private static string WriteFile(StringBuilder sb, string captureOutDir, uint captureId, string sdpName)
        {
            Directory.CreateDirectory(captureOutDir);
            string fileName = captureId > 0
                ? $"snapshot_{captureId}_dashboard.md"
                : $"DrawCallDashboard_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.md";
            string outPath = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
            return outPath;
        }
    }
}
