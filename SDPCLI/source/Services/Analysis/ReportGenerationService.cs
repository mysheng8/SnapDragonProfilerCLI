using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    public class ReportGenerationService
    {
        private readonly Config config;
        private readonly ILogger logger;
        private Tools.LlmApiWrapper? _llm;

        public ReportGenerationService(Config config, ILogger logger, Tools.LlmApiWrapper? llm = null)
        {
            this.config = config;
            this.logger = logger;
            _llm = llm;
        }

        /// <summary>Inject LLM wrapper after construction (called from Application.cs).</summary>
        public void SetLlm(Tools.LlmApiWrapper llm) => _llm = llm;

        // Step 3: labeled CSV
        public string GenerateLabeledMetricsCsv(DrawCallAnalysisReport report, string outputDir)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DrawCall,Category,Detail,ApiName,PipelineID,ShaderCount,TextureCount," +
                          "ColorRT,ColorRTFormat,DepthRT,DepthRTFormat,RTWidth,RTHeight," +
                          "VBCount,HasIB,TypedBufViews,SmallBufs,PerInstanceBuf32," +
                          "VertexCount,IndexCount,InstanceCount," +
                          "Clocks,ReadTotal(Bytes),WriteTotal(Bytes),FragmentsShaded,VerticesShaded," +
                          "%ShadersBusy,%TexL1Miss,%TexL2Miss,%TexFetchStall," +
                          "FragmentInstructions,VertexInstructions,TexMemRead(Bytes)");
            foreach (var dc in report.DrawCallResults)
            {
                var m   = dc.Metrics;
                var colorRTs = dc.RenderTargets.Where(r => r.AttachmentType == "Color").ToList();
                var depthRTs = dc.RenderTargets.Where(r => r.AttachmentType == "Depth" || r.AttachmentType == "Stencil" || r.AttachmentType == "DepthStencil").ToList();
                var firstRT  = dc.RenderTargets.FirstOrDefault();
                string colorRTIds  = colorRTs.Count > 0 ? string.Join("|", colorRTs.Select(r => r.AttachmentResourceID)) : "";
                string colorRTFmts = colorRTs.Count > 0 ? string.Join("|", colorRTs.Select(r => string.IsNullOrEmpty(r.FormatName) ? "?" : r.FormatName)) : "";
                string depthRTIds  = depthRTs.Count > 0 ? string.Join("|", depthRTs.Select(r => r.AttachmentResourceID)) : "";
                string depthRTFmts = depthRTs.Count > 0 ? string.Join("|", depthRTs.Select(r => string.IsNullOrEmpty(r.FormatName) ? "?" : r.FormatName)) : "";
                string rtWidth     = firstRT?.Width  > 0 ? firstRT.Width.ToString()  : "";
                string rtHeight    = firstRT?.Height > 0 ? firstRT.Height.ToString() : "";
                sb.AppendLine(string.Join(",",
                    dc.DrawCallNumber, Q(dc.Label.Category), Q(dc.Label.Detail), Q(dc.ApiName),
                    dc.PipelineID, dc.Shaders.Count, dc.TextureIDs.Length,
                    Q(colorRTIds), Q(colorRTFmts), Q(depthRTIds), Q(depthRTFmts), rtWidth, rtHeight,
                    dc.VertexBuffers.Count,
                    dc.IndexBuffer != null ? 1 : 0,
                    dc.BindingSummary.TypedBufferViewCount,
                    dc.BindingSummary.SmallBufferCount,
                    dc.BindingSummary.HasPerInstanceBuffer ? 1 : 0,
                    dc.VertexCount, dc.IndexCount, dc.InstanceCount,
                    m?.Clocks              ?? 0, m?.ReadTotalBytes  ?? 0, m?.WriteTotalBytes ?? 0,
                    m?.FragmentsShaded     ?? 0, m?.VerticesShaded  ?? 0,
                    m?.ShadersBusyPct.ToString("F2")   ?? "0",
                    m?.TexL1MissPct.ToString("F2")     ?? "0",
                    m?.TexL2MissPct.ToString("F2")     ?? "0",
                    m?.TexFetchStallPct.ToString("F2") ?? "0",
                    m?.FragmentInstructions ?? 0, m?.VertexInstructions ?? 0, m?.TexMemReadBytes ?? 0));
            }
            Directory.CreateDirectory(outputDir);
            string path = Path.Combine(outputDir, $"DrawCallAnalysis_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            return path;
        }

        // Step 4: Markdown summary
        public string GenerateSummaryReport(DrawCallAnalysisReport report, string outputDir)
        {
            var sb = new StringBuilder();
            var dcs = report.DrawCallResults;
            bool hasMetrics = dcs.Any(d => d.Metrics != null);
            bool useLlm = string.Equals(config.Get("ReportMode", "llm"), "llm", StringComparison.OrdinalIgnoreCase)
                          && _llm?.IsEnabled == true;

            sb.AppendLine("# DrawCall Analysis Summary");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}  ");
            sb.AppendLine($"Total DrawCalls: {dcs.Count}  ");
            sb.AppendLine();

            // ── Frame Snapshot ────────────────────────────────────────────────
            // outputDir = snapshot_{captureId}/ — 直接在该目录下找 1_screenshot.bmp，
            // 旋转后保存为 snapshot.png 嵌入报告
            try
            {
                string bmpDirect = Path.Combine(outputDir, "1_screenshot.bmp");
                if (File.Exists(bmpDirect))
                {
                    string snapshotPng = Path.Combine(outputDir, "snapshot.png");
                    bool rotateLandscape = config.GetBool("SnapshotRotateLandscape", true);
                    using (var bmp = new System.Drawing.Bitmap(bmpDirect))
                    using (var out2 = rotateLandscape
                        ? RotateBitmap90CW(bmp) : (System.Drawing.Bitmap)bmp.Clone())
                        out2.Save(snapshotPng, System.Drawing.Imaging.ImageFormat.Png);

                    sb.AppendLine("## Frame Snapshot");
                    sb.AppendLine("![Frame Snapshot](snapshot.png)");
                    sb.AppendLine();
                }
            }
            catch { /* snapshot 提取失败时静默跳过，不影响报告其他内容 */ }

            // Category overview table
            sb.AppendLine("## Overview: DrawCall Categories");
            sb.AppendLine();
            var catGroups = dcs.GroupBy(d => d.Label.Category).OrderByDescending(g => g.Count()).ToList();
            sb.AppendLine("| Category | Count | % |");
            sb.AppendLine("|----------|------:|--:|");
            foreach (var g in catGroups)
                sb.AppendLine($"| {g.Key} | {g.Count()} | {g.Count() * 100.0 / dcs.Count:F1}% |");
            sb.AppendLine();

            if (!hasMetrics)
            {
                sb.AppendLine("> No metrics CSV loaded — set `AnalysisMetricsCSV` in config.ini to enable sections 4.1–4.3.");
                Directory.CreateDirectory(outputDir);
                string p2 = Path.Combine(outputDir, $"DrawCallAnalysis_Summary_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.md");
                File.WriteAllText(p2, sb.ToString(), Encoding.UTF8);
                return p2;
            }

            var withM = dcs.Where(d => d.Metrics != null).ToList();

            // ── Pre-compute global averages ───────────────────────────────────
            double globalAvgClocks = withM.Average(d => (double)d.Metrics!.Clocks);
            double globalAvgRead   = withM.Average(d => (double)d.Metrics!.ReadTotalBytes);
            double globalAvgWrite  = withM.Average(d => (double)d.Metrics!.WriteTotalBytes);
            double globalAvgFrags  = withM.Average(d => (double)d.Metrics!.FragmentsShaded);
            double globalAvgBusy   = withM.Average(d => d.Metrics!.ShadersBusyPct);
            double globalAvgL1     = withM.Average(d => d.Metrics!.TexL1MissPct);
            double globalAvgStall  = withM.Average(d => d.Metrics!.TexFetchStallPct);
            double globalAvgFInstr = withM.Average(d => (double)d.Metrics!.FragmentInstructions);

            // ── Category stats ────────────────────────────────────────────────
            var catStats = new Dictionary<string, CatStats>();
            foreach (var g in catGroups)
            {
                var gm = g.Where(d => d.Metrics != null).ToList();
                if (gm.Count == 0) continue;
                catStats[g.Key] = new CatStats {
                    Count = g.Count(), TotalClocks = gm.Sum(d => d.Metrics!.Clocks),
                    AvgClocks = gm.Average(d => (double)d.Metrics!.Clocks),
                    AvgRead   = gm.Average(d => (double)d.Metrics!.ReadTotalBytes),
                    AvgWrite  = gm.Average(d => (double)d.Metrics!.WriteTotalBytes),
                    AvgFrags  = gm.Average(d => (double)d.Metrics!.FragmentsShaded),
                    AvgBusy   = gm.Average(d => d.Metrics!.ShadersBusyPct),
                    AvgL1     = gm.Average(d => d.Metrics!.TexL1MissPct),
                    AvgStall  = gm.Average(d => d.Metrics!.TexFetchStallPct),
                    AvgFInstr = gm.Average(d => (double)d.Metrics!.FragmentInstructions),
                };
            }

            long totalClocks = withM.Sum(d => d.Metrics!.Clocks);
            var top5 = withM.OrderByDescending(d => d.Metrics!.Clocks).Take(5).ToList();

            // ── Bar chart: GPU Clocks per DC (ALL DCs, Mermaid xychart-beta) ───────
            sb.AppendLine("---\n");
            sb.AppendLine($"## GPU Clock Distribution ({withM.Count} DrawCalls)\n");
            if (withM.Count > 0)
            {
                var allSorted = withM.OrderBy(d => d.DrawCallNumber).ToList();
                var yVals    = allSorted.Select(d => d.Metrics!.Clocks.ToString());
                long chartMax = allSorted.Max(d => d.Metrics!.Clocks);
                // Category x-axis preserves every bar; labelFontSize:0 hides overlapping labels
                var xLabels = allSorted.Select(d => $"\"{d.DrawCallNumber}\"");
                sb.AppendLine("```mermaid");
                sb.AppendLine($"%%{{init: {{\"xyChart\": {{\"width\": 1600, \"height\": 420, \"xAxis\": {{\"labelFontSize\": 1, \"labelPadding\": 0}}}}}}}}}}}}");
                sb.AppendLine("xychart-beta");
                sb.AppendLine($"    title \"GPU Clocks per DrawCall ({withM.Count} DCs)\"");
                sb.AppendLine($"    x-axis [{string.Join(", ", xLabels)}]");
                sb.AppendLine($"    y-axis \"Clocks\" 0 --> {chartMax}");
                sb.AppendLine($"    bar [{string.Join(", ", yVals)}]");
                sb.AppendLine("```");
                sb.AppendLine();
            }

            // ── Pie chart: Category clock budget ──────────────────────────────
            sb.AppendLine("## GPU Clock Budget by Category\n");
            if (catStats.Count > 0)
            {
                sb.AppendLine("```mermaid");
                sb.AppendLine("pie title \"GPU Clock Budget by Category\"");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                    sb.AppendLine($"    \"{kv.Key}\" : {kv.Value.TotalClocks}");
                sb.AppendLine("```");
                sb.AppendLine();
                // Text fallback table under the pie
                sb.AppendLine("| Category | Total Clocks | % of Frame |");
                sb.AppendLine("|----------|-------------:|-----------:|");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                    sb.AppendLine($"| {kv.Key} | {kv.Value.TotalClocks:N0} | {kv.Value.TotalClocks * 100.0 / Math.Max(totalClocks, 1):F1}% |");
                sb.AppendLine();
            }

            // ── 4.1 Top 5 table (dynamic outlier columns) ────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 4.1  Top 5 DrawCalls by GPU Clock Cost\n");
            sb.AppendLine($"> 全局均值（{withM.Count} 个有效DC）：Clocks={globalAvgClocks:N0} | Fragments={globalAvgFrags:N0} | ShaderBusy={globalAvgBusy:F1}% | FragInstr={globalAvgFInstr:N0} | TexStall={globalAvgStall:F1}% | TexL1Miss={globalAvgL1:F1}% | Read={globalAvgRead/1048576.0:F2}MB | Write={globalAvgWrite/1048576.0:F2}MB\n");

            // For each candidate metric: header text, max ratio across top5, threshold, cell formatter
            // Threshold: column appears only when any top5 DC exceeds it
            // Percentages: use absolute-based threshold (avg+delta, min floor)
            // Count/bytes: use ratio-based threshold (1.5×)
            double maxFragRatio   = top5.Max(d => d.Metrics!.FragmentsShaded     / Math.Max(globalAvgFrags,  1.0));
            double maxBusyRatio   = top5.Max(d => d.Metrics!.ShadersBusyPct      / Math.Max(globalAvgBusy,   0.1));
            double maxFInstrRatio = top5.Max(d => d.Metrics!.FragmentInstructions / Math.Max(globalAvgFInstr, 1.0));
            double maxStallRatio  = top5.Max(d => d.Metrics!.TexFetchStallPct    / Math.Max(globalAvgStall,  0.1));
            double maxL1Ratio     = top5.Max(d => d.Metrics!.TexL1MissPct        / Math.Max(globalAvgL1,     0.1));
            double maxReadRatio   = top5.Max(d => d.Metrics!.ReadTotalBytes      / Math.Max(globalAvgRead,   1.0));
            double maxWriteRatio  = top5.Max(d => d.Metrics!.WriteTotalBytes     / Math.Max(globalAvgWrite,  1.0));
            double maxVertRatio   = top5.Max(d => d.Metrics!.VerticesShaded      / Math.Max(withM.Average(x => (double)x.Metrics!.VerticesShaded), 1.0));
            double globalAvgVert  = withM.Average(d => (double)d.Metrics!.VerticesShaded);

            // (header, maxRatio, threshold, avgForCell, rawValGetter, cellFormatter)
            var candidateCols = new List<(string Hdr, double MaxRatio, double Thresh, double AvgVal,
                                          Func<DrawCallMetrics, double> Val,
                                          Func<DrawCallMetrics, double, string> Cell)>
            {
                ($"Fragments (avg {globalAvgFrags:N0})",          maxFragRatio,   1.5, globalAvgFrags,
                    m => (double)m.FragmentsShaded,
                    (m, a) => $"{m.FragmentsShaded:N0} ({m.FragmentsShaded / Math.Max(a,1):F1}×)"),

                ($"%ShaderBusy (avg {globalAvgBusy:F1}%)",        maxBusyRatio,   1.3, globalAvgBusy,
                    m => m.ShadersBusyPct,
                    (m, a) => $"{m.ShadersBusyPct:F1}% (+{Math.Max(0, m.ShadersBusyPct - a):F1}pp)"),

                ($"FragInstr (avg {globalAvgFInstr:N0})",          maxFInstrRatio, 1.5, globalAvgFInstr,
                    m => (double)m.FragmentInstructions,
                    (m, a) => $"{m.FragmentInstructions:N0} ({m.FragmentInstructions / Math.Max(a,1):F1}×)"),

                ($"%TexStall (avg {globalAvgStall:F1}%)",          maxStallRatio,  1.5, globalAvgStall,
                    m => m.TexFetchStallPct,
                    (m, a) => $"{m.TexFetchStallPct:F1}% (+{Math.Max(0, m.TexFetchStallPct - a):F1}pp)"),

                ($"%TexL1Miss (avg {globalAvgL1:F1}%)",            maxL1Ratio,     1.5, globalAvgL1,
                    m => m.TexL1MissPct,
                    (m, a) => $"{m.TexL1MissPct:F1}% (+{Math.Max(0, m.TexL1MissPct - a):F1}pp)"),

                ($"ReadMB (avg {globalAvgRead/1048576.0:F2})",     maxReadRatio,   2.0, globalAvgRead,
                    m => (double)m.ReadTotalBytes,
                    (m, a) => $"{m.ReadTotalBytes/1048576.0:F2} ({m.ReadTotalBytes / Math.Max(a,1):F1}×)"),

                ($"WriteMB (avg {globalAvgWrite/1048576.0:F2})",   maxWriteRatio,  2.0, globalAvgWrite,
                    m => (double)m.WriteTotalBytes,
                    (m, a) => $"{m.WriteTotalBytes/1048576.0:F2} ({m.WriteTotalBytes / Math.Max(a,1):F1}×)"),

                ($"Vertices (avg {globalAvgVert:N0})",             maxVertRatio,   1.5, globalAvgVert,
                    m => (double)m.VerticesShaded,
                    (m, a) => $"{m.VerticesShaded:N0} ({m.VerticesShaded / Math.Max(a,1):F1}×)"),
            };

            // Keep only columns where at least one Top5 DC is a significant outlier,
            // sorted by max ratio descending (most impactful first)
            var activeCols = candidateCols
                .Where(c => c.MaxRatio >= c.Thresh)
                .OrderByDescending(c => c.MaxRatio)
                .ToList();

            // Build header
            var hdrParts = new System.Collections.Generic.List<string>
                { "Rank", "DC", "Category", "Detail", $"Clocks (avg {globalAvgClocks:N0})" };
            var sepParts = new System.Collections.Generic.List<string>
                { "----:", "----", "--------", "------", "-----------:" };
            foreach (var col in activeCols) { hdrParts.Add(col.Hdr); sepParts.Add("-----------:"); }
            sb.AppendLine("| " + string.Join(" | ", hdrParts) + " |");
            sb.AppendLine("| " + string.Join(" | ", sepParts) + " |");

            for (int i = 0; i < top5.Count; i++)
            {
                var d = top5[i]; var m = d.Metrics!;
                double clockRatio = m.Clocks / Math.Max(globalAvgClocks, 1.0);
                var rowParts = new System.Collections.Generic.List<string>
                {
                    (i + 1).ToString(),
                    d.DrawCallNumber,
                    d.Label.Category,
                    d.Label.Detail,
                    $"{m.Clocks:N0} ({clockRatio:F1}×)"
                };
                foreach (var col in activeCols)
                {
                    double ratio = col.Val(m) / Math.Max(col.AvgVal, col.Thresh > 1.3 ? 1.0 : 0.1);
                    string cell  = col.Cell(m, col.AvgVal);
                    if (ratio >= col.Thresh) cell = $"**{cell}**";
                    rowParts.Add(cell);
                }
                sb.AppendLine("| " + string.Join(" | ", rowParts) + " |");
            }
            sb.AppendLine();

            // ── 4.1b Per-Category Top 5 tables ───────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 4.1b  各分类 Top 5 DrawCalls\n");
            foreach (var catGroup in catGroups)
            {
                var catWithM = catGroup.Where(d => d.Metrics != null)
                                       .OrderByDescending(d => d.Metrics!.Clocks)
                                       .Take(5).ToList();
                if (catWithM.Count == 0) continue;
                sb.AppendLine($"### {catGroup.Key}\n");
                // Compute active columns for this category
                double catAvgClocks = catGroup
                    .Where(d => d.Metrics != null).Average(d => (double)d.Metrics!.Clocks);
                sb.AppendLine("| Rank | DC | Detail | Clocks | %ShaderBusy | Fragments | ReadMB | WriteMB |");
                sb.AppendLine("|-----:|-----|--------|-------:|------------:|----------:|-------:|--------:|");
                int catRank = 0;
                foreach (var dc in catWithM)
                {
                    var m = dc.Metrics!;
                    string busyStr = $"{m.ShadersBusyPct:F1}%";
                    string fragStr = $"{m.FragmentsShaded:N0}";
                    string readStr = $"{m.ReadTotalBytes / 1048576.0:F2}";
                    string wrtStr  = $"{m.WriteTotalBytes / 1048576.0:F2}";
                    double ratio   = m.Clocks / Math.Max(catAvgClocks, 1.0);
                    sb.AppendLine($"| {++catRank} | {dc.DrawCallNumber} | {dc.Label.Detail} | "
                        + $"{m.Clocks:N0} ({ratio:F1}×) | {busyStr} | {fragStr} | {readStr} | {wrtStr} |");
                }
                sb.AppendLine();
            }

            // ── 3D Mesh Preview section ──────────────────────────────────────
            string meshDir = Path.Combine(outputDir, "meshes");
            if (Directory.Exists(meshDir))
            {
                var objFiles = Directory.GetFiles(meshDir, "*.obj").OrderBy(f => f).ToList();
                if (objFiles.Count > 0)
                {
                    sb.AppendLine("---\n");
                    sb.AppendLine("## 3D Mesh Preview\n");
                    sb.AppendLine("> 交互式查看器（需要浏览器打开，不支持直接在 Markdown 渲染）");
                    sb.AppendLine();
                    sb.AppendLine($"**[🔗 Open interactive 3D Viewer](meshes/viewer.html)**\n");
                    sb.AppendLine("| Rank | DrawCall | OBJ |");
                    sb.AppendLine("|-----:|----------|-----|");
                    int meshRank = 0;
                    foreach (var f in objFiles)
                    {
                        string fname = Path.GetFileName(f);
                        string dcId  = fname.Replace("drawcall_", "").Replace(".obj", "");
                        sb.AppendLine($"| {++meshRank} | DC {dcId} | [{fname}](meshes/{fname}) |");
                    }
                    sb.AppendLine();
                }
            }
            // ── 4.2 Category statistics ───────────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 4.2  Category Statistics\n");
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

            // ── 4.3 Per-category analysis (top 5 per category, no suggestions) ───
            sb.AppendLine("---\n");
            sb.AppendLine("## 4.3  各分类 Top 5 详细分析\n");

            foreach (var catGroup in catGroups)
            {
                var catDcs = catGroup.Where(d => d.Metrics != null)
                                     .OrderByDescending(d => d.Metrics!.Clocks)
                                     .Take(5).ToList();
                if (catDcs.Count == 0) continue;

                double catAvgClocksAnalysis = catGroup
                    .Where(d => d.Metrics != null).Average(d => (double)d.Metrics!.Clocks);

                sb.AppendLine($"### {catGroup.Key}\n");

                foreach (var (dc, rank) in catDcs.Select((d, i) => (d, i + 1)))
                {
                sb.AppendLine($"#### #{rank}  DC {dc.DrawCallNumber}  {dc.Label.Detail}\n");
                var m = dc.Metrics!;
                sb.AppendLine($"**Clocks:** {m.Clocks:N0}  "
                    + $"({m.Clocks / Math.Max(globalAvgClocks, 1):F1}× 全局均值 "
                    + $"/ {m.Clocks / Math.Max(catAvgClocksAnalysis, 1):F1}× 分类均值)\n");

                var sigs = BuildOutlierSignals(m,
                    globalAvgClocks, globalAvgFrags, globalAvgBusy,
                    globalAvgFInstr, globalAvgStall, globalAvgL1,
                    globalAvgRead, globalAvgWrite);

                sb.AppendLine("**异常指标：**\n");
                if (sigs.Count == 0) sb.AppendLine("- *(无明显单项超标，综合型开销)*");
                else foreach (var s in sigs) sb.AppendLine(s);
                sb.AppendLine();

                // ── Shaders ───────────────────────────────────────────────────
                string dcShaderDir = Path.Combine(outputDir, "shaders", "dc_" + dc.DrawCallNumber);
                if (dc.Shaders.Count > 0 || Directory.Exists(dcShaderDir))
                {
                    sb.AppendLine($"**Shaders（Pipeline {dc.PipelineID}）：**\n");
                    // Stage metadata from DB
                    if (dc.Shaders.Count > 0)
                    {
                        sb.AppendLine("| Stage | ModuleID | Entry |");
                        sb.AppendLine("|-------|----------|-------|");
                        foreach (var s in dc.Shaders)
                            sb.AppendLine($"| {s.ShaderStageName} | `{s.ShaderModuleID}` | `{s.EntryPoint}` |");
                        sb.AppendLine();
                    }
                    // Extracted files with links
                    if (Directory.Exists(dcShaderDir))
                    {
                        var shaderFiles = Directory.GetFiles(dcShaderDir)
                            .OrderBy(f => f).ToList();
                        if (shaderFiles.Count > 0)
                        {
                            sb.AppendLine("提取的 Shader 文件：\n");
                            string relBase = $"shaders/dc_{dc.DrawCallNumber}";
                            foreach (var f in shaderFiles)
                            {
                                string fname = Path.GetFileName(f);
                                sb.AppendLine($"- [{fname}]({relBase}/{fname})");
                            }
                            sb.AppendLine();
                        }
                    }
                }

                // ── Mesh OBJ link ─────────────────────────────────────────────
                string dcObjPath = Path.Combine(outputDir, "meshes", $"drawcall_{dc.DrawCallNumber}.obj");
                if (File.Exists(dcObjPath))
                {
                    sb.AppendLine($"**3D Mesh：** [drawcall_{dc.DrawCallNumber}.obj](meshes/drawcall_{dc.DrawCallNumber}.obj)" +
                                  $"  ｜  [Open in viewer](meshes/viewer.html)\n");
                }

                // ── Textures ──────────────────────────────────────────────────
                string texDir = Path.Combine(outputDir, "textures");
                if (dc.Textures.Count > 0)
                {
                    sb.AppendLine($"贴图（共 {dc.TextureIDs.Length} 张）：\n");
                    sb.AppendLine("| # | ID | 宽 | 高 | 格式 | 图片 |");
                    sb.AppendLine("|---|----|----|-----|------|------|");
                    int ti = 0;
                    foreach (var t in dc.Textures)
                    {
                        string texFile = Path.Combine(texDir, $"texture_{t.TextureID}.png");
                        string imgCell = File.Exists(texFile)
                            ? $"![texture_{t.TextureID}](textures/texture_{t.TextureID}.png)"
                            : "—";
                        sb.AppendLine($"| {++ti} | `{t.TextureID}` | {t.Width} | {t.Height} | {t.FormatName} | {imgCell} |");
                    }
                    if (dc.TextureIDs.Length > dc.Textures.Count)
                        sb.AppendLine($"| … | *({dc.TextureIDs.Length - dc.Textures.Count} 个 ID 无元数据)* | | | | |");
                    sb.AppendLine();
                }
                else if (dc.TextureIDs.Length > 0)
                {
                    // Only IDs, no metadata — list with inline image links where PNG exists
                    var idLinks = dc.TextureIDs.Select(id =>
                    {
                        string texFile = Path.Combine(texDir, $"texture_{id}.png");
                        return File.Exists(texFile)
                            ? $"![texture_{id}](textures/texture_{id}.png)"
                            : $"`{id}`";
                    });
                    sb.AppendLine($"贴图 ID（共 {dc.TextureIDs.Length} 张）：\n");
                    foreach (var link in idLinks)
                        sb.AppendLine($"- {link}");
                    sb.AppendLine();
                }
                } // end foreach dc in catDcs
            } // end foreach catGroup

            // ── Final summary ──────────────────────────────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 总结\n");
            sb.AppendLine($"- 总 Clock 消耗: **{totalClocks:N0}**");
            sb.AppendLine($"- Top 5 占比: **{top5.Sum(d => d.Metrics!.Clocks) * 100.0 / Math.Max(totalClocks, 1):F1}%**\n");
            sb.AppendLine("**各类别 Clock 占比：**\n");
            foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                sb.AppendLine($"- {kv.Key}: {kv.Value.TotalClocks:N0} clocks  ({kv.Value.TotalClocks * 100.0 / Math.Max(totalClocks, 1):F1}%)，共 {kv.Value.Count} 个DC");
            sb.AppendLine();

            if (useLlm)
            {
                sb.AppendLine("**综合优化建议（LLM）：**\n");
                // Pre-compute outlier signals for each top5 DC so the overall prompt has 4.3 context
                var top5Signals = top5.Select(dc => BuildOutlierSignals(dc.Metrics!,
                    globalAvgClocks, globalAvgFrags, globalAvgBusy,
                    globalAvgFInstr, globalAvgStall, globalAvgL1,
                    globalAvgRead, globalAvgWrite)).ToList();

                string overall = GetLlmOverallConclusion(
                    top5, top5Signals, catStats, withM, totalClocks,
                    globalAvgClocks, globalAvgFrags, globalAvgBusy,
                    globalAvgFInstr, globalAvgStall, globalAvgL1,
                    globalAvgRead, globalAvgWrite);
                foreach (var line in overall.Split('\n'))
                    sb.AppendLine(string.IsNullOrWhiteSpace(line) ? "" : $"> {line.Trim()}");
                sb.AppendLine();
            }

            Directory.CreateDirectory(outputDir);
            string path = Path.Combine(outputDir, $"DrawCallAnalysis_Summary_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.md");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            return path;
        }

        // ── Outlier signal builder ────────────────────────────────────────────
        private static List<string> BuildOutlierSignals(DrawCallMetrics m,
            double avgClocks, double avgFrags, double avgBusy,
            double avgFInstr, double avgStall, double avgL1,
            double avgRead, double avgWrite)
        {
            var sigs = new List<string>();
            if (m.FragmentsShaded > avgFrags * 1.5)
                sigs.Add($"- **Fragments Shaded** ({m.FragmentsShaded/Math.Max(avgFrags,1):F1}×): 片元数量大 → overdraw高或绘制面积大");
            if (m.ShadersBusyPct > avgBusy + 10 && m.ShadersBusyPct > 70)
                sigs.Add($"- **% Shaders Busy** ({m.ShadersBusyPct:F1}%): Shader占用率高 → ALU计算密集");
            if (m.FragmentInstructions > avgFInstr * 1.5)
                sigs.Add($"- **Fragment Instructions** ({m.FragmentInstructions/Math.Max(avgFInstr,1):F1}×): 指令数高 → 复杂像素着色");
            if (m.TexFetchStallPct > avgStall + 5 && m.TexFetchStallPct > 5)
                sigs.Add($"- **% Texture Fetch Stall** ({m.TexFetchStallPct:F1}%): 纹理采样等待多 → 纹理数量多/大");
            if (m.TexL1MissPct > avgL1 + 10 && m.TexL1MissPct > 30)
                sigs.Add($"- **% Texture L1 Miss** ({m.TexL1MissPct:F1}%): L1缓存命中率低 → 纹理访问随机");
            if (m.ReadTotalBytes > avgRead * 2)
                sigs.Add($"- **Read Total** ({m.ReadTotalBytes/1048576.0:F2}MB, {m.ReadTotalBytes/Math.Max(avgRead,1):F1}×): 内存读带宽高 → 大纹理/buffer读取");
            if (m.WriteTotalBytes > avgWrite * 2)
                sigs.Add($"- **Write Total** ({m.WriteTotalBytes/1048576.0:F2}MB): 内存写带宽高 → 大分辨率RT写入");
            return sigs;
        }

        // ── Rule-based conclusion (fallback mode) ────────────────────────────
        private static string GetRuleBasedConclusion(List<string> sigs)
        {
            bool isTexBound  = sigs.Any(s => s.Contains("Stall") || s.Contains("L1 Miss"));
            bool isAluBound  = sigs.Any(s => s.Contains("Instructions") || s.Contains("Shaders Busy"));
            bool isBwBound   = sigs.Any(s => s.Contains("Read Total") || s.Contains("Write Total"));
            bool isFillBound = sigs.Any(s => s.Contains("Fragments"));
            if (isTexBound)
                return "⚡ **Texture Bound** — 优化方向：ASTC压缩、减少纹理数/分辨率、改善UV连续性、使用纹理数组。";
            if (isAluBound)
                return "⚡ **ALU Bound** — 优化方向：简化Fragment Shader、减少数学运算、使用mediump精度、合并Pass。";
            if (isBwBound)
                return "⚡ **Bandwidth Bound** — 优化方向：降低RT分辨率、使用GMEM tile memory、减少大纹理读取。";
            if (isFillBound)
                return "⚡ **Fillrate Bound** — 优化方向：减少overdraw（遮挡剔除/排序绘制）、降低分辨率、Early-Z。";
            return "ℹ **综合型** — 考虑整体简化或降低质量档位。";
        }

        // ── LLM per-DC bottleneck conclusion ─────────────────────────────────
        private string GetLlmBottleneckConclusion(DrawCallInfo dc, DrawCallMetrics m,
            List<string> sigs,
            double avgClocks, double avgFrags, double avgBusy,
            double avgFInstr, double avgStall, double avgL1,
            double avgRead, double avgWrite,
            string shaderBaseDir)
        {
            try
            {
                int maxShaderChars = config.GetInt("LlmMaxShaderChars", 20000);

                var p = new StringBuilder();
                p.AppendLine("你是移动端GPU渲染与性能优化专家。下面提供了「单个」Draw Call的上下文：Shader main函数、绑定资源（RT/贴图/Buffer）以及性能异常指标。");
                p.AppendLine("请用精炼中文完成三件事：");
                p.AppendLine("①识别渲染Pass类型（阴影/GBuffer/SSAO/粒子/PostProcess/UI等）及关键技术；");
                p.AppendLine("②指出主要瓶颈（Texture Bound/ALU Bound/Bandwidth Bound/Fillrate Bound）；");
                p.AppendLine("③给出2-3条针对此Pass和Shader的具体移动端优化建议。");
                p.AppendLine("直接输出正文，不要标题行、不要JSON。");
                p.AppendLine();

                // ── 1. DC identity ──
                p.AppendLine($"DC: {dc.DrawCallNumber} | API: {dc.ApiName} | {dc.Label.Category} — {dc.Label.Detail}");
                p.AppendLine($"Pipeline={dc.PipelineID}  RenderPass={dc.RenderPass}");
                if (dc.VertexCount > 0 || dc.IndexCount > 0)
                    p.AppendLine($"Verts={dc.VertexCount}  Indices={dc.IndexCount}  Instances={dc.InstanceCount}");

                // ── 2. Performance metrics (always included — small) ──
                p.AppendLine();
                p.AppendLine("性能指标(vs全局均值):");
                p.AppendLine($"  Clocks={m.Clocks:N0}({m.Clocks/Math.Max(avgClocks,1):F1}x)  Frags={m.FragmentsShaded:N0}({m.FragmentsShaded/Math.Max(avgFrags,1):F1}x)");
                p.AppendLine($"  %ShaderBusy={m.ShadersBusyPct:F1}%(avg={avgBusy:F1})  FragInstr={m.FragmentInstructions:N0}({m.FragmentInstructions/Math.Max(avgFInstr,1):F1}x)");
                p.AppendLine($"  %TexStall={m.TexFetchStallPct:F1}%(avg={avgStall:F1})  %TexL1Miss={m.TexL1MissPct:F1}%(avg={avgL1:F1})");
                p.AppendLine($"  Read={m.ReadTotalBytes/1048576.0:F2}MB(avg={avgRead/1048576.0:F2})  Write={m.WriteTotalBytes/1048576.0:F2}MB(avg={avgWrite/1048576.0:F2})");
                if (sigs.Count > 0)
                {
                    p.AppendLine("  异常信号: " + string.Join(" | ", sigs.Select(s => s.TrimStart('-', ' ', '*')
                        .Split(':')[0].Trim())));
                }

                // ── 3. Render Targets ──
                if (dc.RenderTargets.Count > 0)
                {
                    p.AppendLine();
                    p.AppendLine($"RT({dc.RenderTargets.Count}): " + string.Join(" | ", dc.RenderTargets.Select(rt =>
                    {
                        string sz  = (rt.Width > 0 && rt.Height > 0) ? $"{rt.Width}x{rt.Height}" : "?";
                        string fmt = string.IsNullOrEmpty(rt.FormatName) ? "?" : rt.FormatName.Replace("VK_FORMAT_", "");
                        return $"[{rt.AttachmentIndex}]{rt.AttachmentType} {sz} {fmt}";
                    })));
                }

                // ── 4. Bound Textures (compact) ──
                if (dc.Textures.Count > 0)
                {
                    p.AppendLine();
                    p.AppendLine($"Textures({dc.TextureIDs.Length}):");
                    foreach (var t in dc.Textures.Take(20))
                    {
                        string extra = (t.LayerCount > 1 || t.LevelCount > 1) ? $" L{t.LayerCount}M{t.LevelCount}" : "";
                        string fmt   = t.FormatName.Replace("VK_FORMAT_", "");
                        p.AppendLine($"  {t.Width}x{t.Height} {fmt}{extra}");
                    }
                    if (dc.Textures.Count > 20)
                        p.AppendLine($"  ...+{dc.Textures.Count - 20}张");
                }
                else if (dc.TextureIDs.Length > 0)
                {
                    p.AppendLine($"Textures: {dc.TextureIDs.Length}张(无元数据)");
                }

                // ── 5. Buffers / Bindings summary ──
                var bs = dc.BindingSummary;
                var bindParts = new List<string>();
                if (dc.VertexBuffers.Count > 0) bindParts.Add($"VB×{dc.VertexBuffers.Count}");
                if (dc.IndexBuffer != null)      bindParts.Add($"IB({dc.IndexBuffer.IndexType})");
                if (bs.TypedBufferViewCount > 0) bindParts.Add($"TypedBuf×{bs.TypedBufferViewCount}");
                if (bs.SmallBufferCount > 0)     bindParts.Add($"CBuffer×{bs.SmallBufferCount}");
                if (bs.HasPerInstanceBuffer)     bindParts.Add("PerInstance(skinning)");
                if (bindParts.Count > 0)
                {
                    p.AppendLine();
                    p.AppendLine("Bindings: " + string.Join("  ", bindParts));
                }

                // ── 6. Shader source — main entry first, then declarations summary ──
                p.AppendLine();
                p.AppendLine($"## Shader源码 ({dc.Shaders.Count} stages)");

                string dcShaderDir = Path.Combine(shaderBaseDir, "dc_" + dc.DrawCallNumber);
                int shaderCharsLeft = maxShaderChars;

                if (Directory.Exists(dcShaderDir) && shaderCharsLeft > 0)
                {
                    // Build a lookup of entry point names for quick search
                    var entryPoints = new HashSet<string>(StringComparer.Ordinal);
                    entryPoints.Add("main");
                    foreach (var s in dc.Shaders)
                        if (!string.IsNullOrEmpty(s.EntryPoint))
                            entryPoints.Add(s.EntryPoint);

                    foreach (var sf in Directory.GetFiles(dcShaderDir).OrderBy(f => f))
                    {
                        if (shaderCharsLeft <= 0) break;
                        try
                        {
                            string src = File.ReadAllText(sf);

                            // Find the earliest entry-point function definition
                            int mainPos = -1;
                            foreach (var ep in entryPoints)
                            {
                                // Match "returnType main(" or "void main\n(" patterns
                                foreach (string pat in new[] { $" {ep}(", $"\n{ep}(", $"\t{ep}(" })
                                {
                                    int pos = src.IndexOf(pat, StringComparison.Ordinal);
                                    if (pos >= 0 && (mainPos < 0 || pos < mainPos))
                                        mainPos = pos;
                                }
                            }

                            string output;
                            if (mainPos > 0)
                            {
                                // Compact preamble: extract just declaration names (cbuffer/struct/Texture2D lines),
                                // skip full bodies to save tokens
                                var preambleLines = src.Substring(0, mainPos)
                                    .Split('\n')
                                    .Where(l =>
                                    {
                                        var t = l.TrimStart();
                                        return t.StartsWith("cbuffer")     || t.StartsWith("struct ")     ||
                                               t.StartsWith("Texture")     || t.StartsWith("SamplerState")||
                                               t.StartsWith("RWTexture")   || t.StartsWith("Buffer<")     ||
                                               t.StartsWith("RWBuffer")    || t.StartsWith("layout(")     ||
                                               t.StartsWith("uniform ")    || t.StartsWith("#define ")    ||
                                               t.StartsWith("in ")         || t.StartsWith("out ");
                                    })
                                    .Take(40)  // cap at 40 declaration lines
                                    .ToList();

                                string preamble  = string.Join("\n", preambleLines);
                                string mainBlock = src.Substring(mainPos);
                                if (mainBlock.Length > shaderCharsLeft - preamble.Length - 100)
                                    mainBlock = mainBlock.Substring(0, Math.Max(0, shaderCharsLeft - preamble.Length - 100)) + "\n... [truncated]";

                                output = (preamble.Length > 0 ? preamble + "\n// ---\n" : "") + mainBlock;
                            }
                            else
                            {
                                // No main found — include full file up to budget
                                output = src.Length > shaderCharsLeft
                                    ? src.Substring(0, shaderCharsLeft) + "\n... [truncated]"
                                    : src;
                            }

                            shaderCharsLeft -= output.Length;
                            p.AppendLine();
                            p.AppendLine($"### {Path.GetFileName(sf)}");
                            p.AppendLine("```");
                            p.AppendLine(output);
                            p.AppendLine("```");
                        }
                        catch { }
                    }
                }

                string? resp = _llm!.Chat(p.ToString());
                if (!string.IsNullOrWhiteSpace(resp))
                    return resp.Trim();

                logger.Info($"    [Report LLM] DC {dc.DrawCallNumber} got empty response, using fallback");
            }
            catch (Exception ex)
            {
                logger.Info($"    [Report LLM] DC {dc.DrawCallNumber} error: {ex.Message}, using fallback");
            }
            return GetRuleBasedConclusion(sigs);
        }


        // ── LLM overall frame summary conclusion ─────────────────────────────────
        private string GetLlmOverallConclusion(
            List<DrawCallInfo> top5, List<List<string>> top5Signals,
            Dictionary<string, CatStats> catStats, List<DrawCallInfo> allWithMetrics,
            long totalClocks,
            double avgClocks, double avgFrags, double avgBusy,
            double avgFInstr, double avgStall, double avgL1,
            double avgRead, double avgWrite)
        {
            try
            {
                var p = new StringBuilder();
                p.AppendLine("你是移动端GPU渲染与性能优化专家。以下是完整的单帧性能分析数据（4.1 Top5表、4.2分类统计、4.3单DC异常信号）。");
                p.AppendLine("请综合全局情况与各类别差异，给出5-8条精准、可落地的整帧优化建议。");
                p.AppendLine("覆盖：最大瓶颈类别、瓶颈根因（ALU/纹理/带宽/Fillrate）、具体移动端优化手段（ASTC/mediump/Early-Z/GMEM Tile/遮挡剔除等）。");
                p.AppendLine("只输出中文建议正文，不要标题行或JSON。");
                p.AppendLine();

                // ── Global baseline ──
                p.AppendLine("【全局基线】");
                p.AppendLine($"总Clocks={totalClocks:N0}  DC数={allWithMetrics.Count}  均值/DC={avgClocks:N0}");
                p.AppendLine($"全局均值: Frags={avgFrags:N0}  %Busy={avgBusy:F1}%  FragInstr={avgFInstr:N0}  %TexStall={avgStall:F1}%  %L1Miss={avgL1:F1}%  Read={avgRead/1048576.0:F2}MB  Write={avgWrite/1048576.0:F2}MB");
                p.AppendLine();

                // ── 4.1: Top 5 full metrics table ──
                p.AppendLine("【4.1 Top 5 DrawCalls】");
                p.AppendLine($"  {"#",-3} {"DC",-12} {"Category",-18} {"Clocks",9} {"×avg",5} {"Frags",9} {"%Busy",6} {"FragInstr",9} {"%Stall",7} {"%L1",6} {"RdMB",7} {"WrMB",7}");
                for (int i = 0; i < top5.Count; i++)
                {
                    var d = top5[i]; var m = d.Metrics!;
                    p.AppendLine($"  #{i+1,-2} {d.DrawCallNumber,-12} {d.Label.Category,-18} {m.Clocks,9:N0} {m.Clocks/Math.Max(avgClocks,1),5:F1}x {m.FragmentsShaded,9:N0} {m.ShadersBusyPct,6:F1}% {m.FragmentInstructions,9:N0} {m.TexFetchStallPct,7:F1}% {m.TexL1MissPct,6:F1}% {m.ReadTotalBytes/1048576.0,7:F2} {m.WriteTotalBytes/1048576.0,7:F2}");
                }
                p.AppendLine();

                // ── 4.2: Category stats ──
                p.AppendLine("【4.2 Category Statistics】");
                p.AppendLine($"  {"Category",-18} {"N",4} {"Clk%",5} {"AvgClk",8} {"AvgFrags",9} {"%Busy",6} {"FragInstr",9} {"%Stall",7} {"%L1",6} {"RdMB",7} {"WrMB",7}");
                foreach (var kv in catStats.OrderByDescending(x => x.Value.TotalClocks))
                {
                    var st = kv.Value;
                    p.AppendLine($"  {kv.Key,-18} {st.Count,4} {st.TotalClocks*100.0/Math.Max(totalClocks,1),5:F1}% {st.AvgClocks,8:N0} {st.AvgFrags,9:N0} {st.AvgBusy,6:F1}% {st.AvgFInstr,9:N0} {st.AvgStall,7:F1}% {st.AvgL1,6:F1}% {st.AvgRead/1048576.0,7:F2} {st.AvgWrite/1048576.0,7:F2}");
                }
                p.AppendLine();

                // ── 4.3: Per-DC outlier signals ──
                p.AppendLine("【4.3 Top 5 单DC异常信号】");
                for (int i = 0; i < top5.Count; i++)
                {
                    var d    = top5[i]; var m = d.Metrics!;
                    var sigs = top5Signals[i];
                    string rtDesc = d.RenderTargets.Count > 0
                        ? string.Join(",", d.RenderTargets.Select(r => (r.Width > 0 ? $"{r.Width}x{r.Height}" : "?") + r.AttachmentType.Substring(0, 1)))
                        : "—";
                    p.AppendLine($"  #{i+1} DC {d.DrawCallNumber} [{d.Label.Category}] {d.Label.Detail}  RT={rtDesc}  Clocks={m.Clocks:N0}({m.Clocks/Math.Max(avgClocks,1):F1}x)");
                    if (sigs.Count > 0)
                        foreach (var s in sigs)
                            p.AppendLine("     ▶ " + s.TrimStart('-', ' ', '*').Split(':')[0].Trim());
                    else
                        p.AppendLine("     (无明显单项超标，综合型开销)");
                }

                string? resp = _llm!.Chat(p.ToString());
                if (!string.IsNullOrWhiteSpace(resp))
                    return resp.Trim();

                logger.Info("    [Report LLM] Overall conclusion got empty response, skipping");
            }
            catch (Exception ex)
            {
                logger.Info($"    [Report LLM] Overall conclusion error: {ex.Message}");
            }
            return "*(LLM未能返回综合建议)*";
        }

        // Legacy compatibility
        private static System.Drawing.Bitmap RotateBitmap90CW(System.Drawing.Bitmap src)
        {
            var rotated = new System.Drawing.Bitmap(src.Height, src.Width);
            using (var g = System.Drawing.Graphics.FromImage(rotated))
            {
                g.TranslateTransform(0, src.Width);
                g.RotateTransform(-90);
                g.DrawImage(src, 0, 0);
            }
            return rotated;
        }

        public string GenerateMarkdownReport(DrawCallAnalysisReport r, string dbPath, uint captureId, string outputDir)
            => GenerateSummaryReport(r, outputDir);
        public string GenerateCsvReport(DrawCallAnalysisReport r, string outputDir)
            => GenerateLabeledMetricsCsv(r, outputDir);

        private static string Q(string s) =>
            s.Contains(',') || s.Contains('"') ? $"\"{s.Replace("\"","\"\"") }\"" : s;

        private class CatStats {
            public int    Count; public long TotalClocks;
            public double AvgClocks, AvgRead, AvgWrite, AvgFrags, AvgBusy, AvgL1, AvgStall, AvgFInstr;
        }
    }
}
