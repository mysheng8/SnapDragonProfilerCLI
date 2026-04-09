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
    /// Pass B Step B2 — per-category LLM attribution analysis report.
    ///
    /// Reads topdc.json + status.json + per_dc_content/ directory (from Step B1),
    /// calls LLM once per category with a structured prompt that includes:
    ///   - overall stats from status.json
    ///   - top-N DC bottleneck attribution from topdc.json (Layer3 results)
    ///   - shader content summaries from per_dc_content/
    ///
    /// Produces snapshot_{id}_analysis.md.
    /// Falls back to a rule-based report when LLM is unavailable.
    /// </summary>
    public class AttributionReportService
    {
        private readonly Config               _config;
        private readonly ILogger              _logger;
        private readonly Tools.LlmApiWrapper? _llm;
        private readonly DcContentAnalysisService _contentService;

        public AttributionReportService(
            Config               config,
            ILogger              logger,
            Tools.LlmApiWrapper? llm,
            DcContentAnalysisService contentService)
        {
            _config         = config;
            _logger         = logger;
            _llm            = llm;
            _contentService = contentService;
        }

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Generates the analysis.md report.
        /// Reads topdc.json and status.json from captureOutDir.
        /// Returns the path of the written file.
        /// </summary>
        public string GenerateAnalysisMd(
            DrawCallAnalysisReport report,
            string captureOutDir,
            uint   captureId = 0,
            string sdpName   = "")
        {
            // ── Load topdc.json ───────────────────────────────────────────────
            JObject? topdcRoot  = LoadJson(captureOutDir, $"snapshot_{captureId}_topdc.json");
            JObject? statusRoot = LoadJson(captureOutDir, $"snapshot_{captureId}_status.json");

            bool useLlm = _llm?.IsEnabled == true
                && string.Equals(_config.Get("ReportMode", "llm"), "llm", StringComparison.OrdinalIgnoreCase);

            var sb = new StringBuilder();
            sb.AppendLine($"# {sdpName} DrawCall 归因分析报告");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}  ");
            sb.AppendLine();

            // ── Section 1: Overall summary ────────────────────────────────────
            AppendOverallSection(sb, statusRoot, report);

            // ── Section 2: Per-category analysis ─────────────────────────────
            sb.AppendLine("## 2. 分类分析\n");

            var categories = topdcRoot?["categories"] as JArray
                          ?? BuildCategoriesFromReport(report);

            int catIndex = 1;
            foreach (var catToken in categories)
            {
                string cat      = catToken["category"]?.ToString() ?? "Unknown";
                int    dcCount  = catToken["dc_count"]?.ToObject<int>() ?? 0;

                // Category-level clocks pct from status.json
                double clocksPct = GetClocksPct(statusRoot, cat);

                sb.AppendLine($"### 2.{catIndex}. {cat} 类（{dcCount} DC，占总 clocks {clocksPct:F1}%）\n");
                catIndex++;

                var topDcs = catToken["top_dcs"] as JArray ?? new JArray();
                if (topDcs.Count == 0)
                {
                    sb.AppendLine("> 该分类无 top DC 数据。\n");
                    continue;
                }

                if (useLlm)
                {
                    string categorySection = GetLlmCategorySection(
                        cat, dcCount, clocksPct, topDcs, statusRoot, captureOutDir, sdpName);
                    sb.Append(categorySection);
                }
                else
                {
                    AppendRuleBasedCategorySection(sb, cat, topDcs, captureOutDir);
                }
            }

            // ── Section 3: Combined recommendations ──────────────────────────
            sb.AppendLine("---\n");
            sb.AppendLine("## 3. 综合建议\n");

            if (useLlm)
            {
                string overall = GetLlmOverallRecommendations(report, statusRoot, topdcRoot, sdpName);
                foreach (var line in overall.Split('\n'))
                    sb.AppendLine(string.IsNullOrWhiteSpace(line) ? "" : $"> {line.Trim()}");
            }
            else
            {
                AppendRuleBasedRecommendations(sb, report, statusRoot);
            }
            sb.AppendLine();

            // ── Write file ────────────────────────────────────────────────────
            Directory.CreateDirectory(captureOutDir);
            string fileName = captureId > 0
                ? $"snapshot_{captureId}_analysis.md"
                : $"DrawCallAnalysis_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.md";
            string outPath = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
            return outPath;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Section builders
        // ──────────────────────────────────────────────────────────────────────

        private static void AppendOverallSection(StringBuilder sb, JObject? status, DrawCallAnalysisReport report)
        {
            sb.AppendLine("## 1. 整体概览\n");
            if (status?["overall"] is JObject ov)
            {
                sb.AppendLine($"- 总 DC 数: **{ov["total_dc_count"]}**（Draw: {ov["draw_dc_count"]}，Compute: {ov["compute_dc_count"]}）");
                sb.AppendLine($"- 总 Clocks: **{(long)(ov["total_clocks"] ?? 0):N0}**");
                long readB  = (long)(ov["total_read_bytes"]  ?? 0);
                long writeB = (long)(ov["total_write_bytes"] ?? 0);
                sb.AppendLine($"- 总内存读取: **{readB / 1048576.0:F2} MB**，总写入: **{writeB / 1048576.0:F2} MB**");
                double cov = (ov["metrics_coverage_ratio"]?.ToObject<double>() ?? 0) * 100;
                sb.AppendLine($"- Metrics 覆盖率: **{cov:F1}%**");
            }
            else
            {
                int withM = report.DrawCallResults.Count(d => d.Metrics != null);
                sb.AppendLine($"- 总 DC 数: **{report.DrawCallResults.Count}**，有 Metrics: **{withM}**");
            }

            // Label quality
            if (status?["label_stats"] is JObject ls)
            {
                double avgConf  = ls["avg_confidence"]?.ToObject<double>() ?? 0;
                double lowRatio = (ls["low_confidence_ratio"]?.ToObject<double>() ?? 0) * 100;
                sb.AppendLine($"- 标签均值置信度: **{avgConf:F2}**，低置信度占比: **{lowRatio:F1}%**");
            }
            sb.AppendLine();
        }

        private string GetLlmCategorySection(
            string   cat,
            int      dcCount,
            double   clocksPct,
            JArray   topDcs,
            JObject? statusRoot,
            string   captureOutDir,
            string   sdpName)
        {
            var sb = new StringBuilder();
            try
            {
                string prompt  = BuildCategoryPrompt(cat, dcCount, clocksPct, topDcs, statusRoot, captureOutDir, sdpName);
                string? resp   = _llm!.Chat(prompt);
                if (!string.IsNullOrWhiteSpace(resp))
                {
                    sb.AppendLine(resp!.Trim());
                    sb.AppendLine();
                    return sb.ToString();
                }
                _logger.Info($"    [B2] Category {cat} — LLM empty response, falling back");
            }
            catch (Exception ex)
            {
                _logger.Info($"    [B2] Category {cat} — LLM error: {ex.Message}");
            }

            AppendRuleBasedCategorySection(sb, cat, topDcs, captureOutDir);
            return sb.ToString();
        }

        private string BuildCategoryPrompt(
            string   cat,
            int      dcCount,
            double   clocksPct,
            JArray   topDcs,
            JObject? statusRoot,
            string   captureOutDir,
            string   sdpName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是移动端Vulkan GPU性能分析专家。");
            sb.AppendLine($"以下是 {sdpName} 帧捕获中 {cat} 分类的DrawCall详细归因分析数据。");
            sb.AppendLine();
            sb.AppendLine($"## 分类统计");
            sb.AppendLine($"分类: {cat}  DC数: {dcCount}  总clocks占比: {clocksPct:F1}%");

            // Category-level avg metrics from status.json
            AppendCategoryAvgToPrompt(sb, statusRoot, cat);

            sb.AppendLine();
            sb.AppendLine($"## Top {topDcs.Count} 高耗时 DrawCall 归因分析（来自 topdc.json）");
            foreach (var dcToken in topDcs)
            {
                string dcId  = dcToken["dc_id"]?.ToString()    ?? "?";
                int    rank  = dcToken["rank"]?.ToObject<int>() ?? 0;
                long   clocks= dcToken["clocks"]?.ToObject<long>() ?? 0;
                var    attr  = dcToken["attribution"] as JObject;
                var    label = dcToken["label"] as JObject;

                sb.AppendLine();
                sb.AppendLine($"### DC #{rank}. dc_id={dcId}  ({label?["detail"]})");
                sb.AppendLine($"- clocks: {clocks:N0}");

                if (attr != null)
                {
                    string primary   = attr["primary_bottleneck"]?.ToString()   ?? "";
                    string secondary = attr["secondary_bottleneck"]?.ToString() ?? "";
                    double score     = attr["confidence_score"]?.ToObject<double>() ?? 0;
                    sb.AppendLine(!string.IsNullOrEmpty(primary)
                        ? $"- 主要瓶颈: **{primary}**（得分 {score:F2}）"
                        : "- 主要瓶颈: 未检测到明显瓶颈");
                    if (!string.IsNullOrEmpty(secondary))
                        sb.AppendLine($"- 次要瓶颈: {secondary}");

                    if (attr["suspicious_metrics"] is JArray sms && sms.Count > 0)
                    {
                        sb.AppendLine("- 关键指标:");
                        foreach (var sm in sms.Take(4))
                        {
                            string metric = sm["metric"]?.ToString() ?? "";
                            double val    = sm["value"]?.ToObject<double>() ?? 0;
                            string tier   = "";
                            // find tier from percentile_scores
                            var ps = (attr["percentile_scores"] as JArray)?
                                .FirstOrDefault(p => p["metric"]?.ToString() == metric);
                            if (ps != null) tier = ps["percentile_tier"]?.ToString() ?? "";
                            sb.AppendLine($"  - {metric}: {val:G4}{(!string.IsNullOrEmpty(tier) ? $"（{tier} 档）" : "")}");
                        }
                    }
                }

                // Shader content summary from B1 cache
                var content = _contentService.GetCached(dcId, captureOutDir);
                if (content != null)
                {
                    string passName = content["pass_name"]?.ToString()      ?? "";
                    string tech     = content["technique"]?.ToString()       ?? "";
                    var    features = content["key_features"] as JArray;
                    string featStr  = features != null
                        ? string.Join(", ", features.Select(f => f.ToString()))
                        : "";
                    sb.AppendLine($"- Shader内容摘要: {passName} — {tech}");
                    if (!string.IsNullOrEmpty(featStr))
                        sb.AppendLine($"  特征: {featStr}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("## 要求");
            sb.AppendLine($"请为 {cat} 类写一份归因分析报告，包含（用Markdown格式，中文撰写）：");
            sb.AppendLine("1. 该分类整体性能特征（1-2段）");
            sb.AppendLine("2. 每个 Top DC 的详细归因说明（结合 shader 内容和指标数据）");
            sb.AppendLine("3. 针对该分类的改进建议（2-4条，每条附具体指标依据）");
            sb.AppendLine("直接输出正文段落，不要再重复写外层标题「2.x. {cat}类」。");

            return sb.ToString();
        }

        private static void AppendCategoryAvgToPrompt(StringBuilder sb, JObject? statusRoot, string cat)
        {
            if (statusRoot?["category_stats"] is not JArray catArr) return;
            var entry = catArr.FirstOrDefault(c => c["category"]?.ToString() == cat);
            if (entry?["metrics_avg"] is not JObject avg) return;

            sb.AppendLine("分类均值指标:");
            sb.AppendLine($"  clocks={avg["clocks"]}  shaders_busy_pct={avg["shaders_busy_pct"]}%");
            sb.AppendLine($"  tex_fetch_stall_pct={avg["tex_fetch_stall_pct"]}%  tex_l1_miss_pct={avg["tex_l1_miss_pct"]}%");
            sb.AppendLine($"  read={((long)(avg["read_total_bytes"] ?? 0)) / 1048576.0:F2}MB  fragment_instructions={avg["fragment_instructions"]}");
        }

        private static void AppendRuleBasedCategorySection(StringBuilder sb, string cat, JArray topDcs, string captureOutDir)
        {
            sb.AppendLine($"#### 分类性能特征\n");
            sb.AppendLine($"{cat} 类包含以下高耗时 DrawCall：\n");

            foreach (var dcToken in topDcs)
            {
                string dcId  = dcToken["dc_id"]?.ToString() ?? "?";
                int    rank  = dcToken["rank"]?.ToObject<int>() ?? 0;
                long   clocks= dcToken["clocks"]?.ToObject<long>() ?? 0;
                var    attr  = dcToken["attribution"] as JObject;
                var    label = dcToken["label"] as JObject;

                sb.AppendLine($"##### DC #{rank} — {dcId}（{label?["detail"]}）");
                sb.AppendLine($"- **Clocks**: {clocks:N0}");

                if (attr != null)
                {
                    string primary = attr["primary_bottleneck"]?.ToString() ?? "";
                    double score   = attr["confidence_score"]?.ToObject<double>() ?? 0;
                    if (!string.IsNullOrEmpty(primary))
                        sb.AppendLine($"- **主要瓶颈**: {primary}（得分 {score:F2}）");

                    if (attr["suspicious_metrics"] is JArray sms && sms.Count > 0)
                    {
                        sb.AppendLine("- **关键指标超标**:");
                        foreach (var sm in sms.Take(3))
                            sb.AppendLine($"  - {sm["metric"]}: {sm["value"]:G4}");
                    }
                }
                sb.AppendLine();
            }
        }

        private string GetLlmOverallRecommendations(
            DrawCallAnalysisReport report,
            JObject? statusRoot,
            JObject? topdcRoot,
            string   sdpName)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("你是移动端GPU渲染与性能优化专家。");
                sb.AppendLine("根据以下整帧分析数据，给出5-8条精准、可落地的整帧优化建议。");
                sb.AppendLine("覆盖：最大瓶颈类别、根因（ALU/纹理/带宽/Fillrate）、具体移动端优化手段。");
                sb.AppendLine("只输出中文建议正文列表，不要标题行或JSON。");
                sb.AppendLine();

                if (statusRoot?["overall"] is JObject ov)
                {
                    sb.AppendLine("【全局统计】");
                    sb.AppendLine($"总DC数={ov["total_dc_count"]}  总Clocks={ov["total_clocks"]}");
                    sb.AppendLine($"Read={((long)(ov["total_read_bytes"]??0))/1048576.0:F2}MB  Write={((long)(ov["total_write_bytes"]??0))/1048576.0:F2}MB");
                    sb.AppendLine();
                }

                if (statusRoot?["category_stats"] is JArray catArr)
                {
                    sb.AppendLine("【各分类 clocks 占比（按 clocks 降序）】");
                    foreach (var c in catArr.OrderByDescending(c => c["clocks_sum"]?.ToObject<long>() ?? 0))
                        sb.AppendLine($"  {c["category"],-18} {c["dc_count"],4} DCs  {c["clocks_pct"]}%");
                    sb.AppendLine();
                }

                // Top bottlenecks across all categories from topdc.json
                if (topdcRoot?["categories"] is JArray tcats)
                {
                    sb.AppendLine("【检测到的主要瓶颈汇总】");
                    var bottleneckCounts = new Dictionary<string, int>();
                    foreach (var tcat in tcats)
                    {
                        if (tcat["top_dcs"] is not JArray topDcs) continue;
                        foreach (var dcT in topDcs)
                        {
                            string pb = dcT["attribution"]?["primary_bottleneck"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(pb))
                                bottleneckCounts[pb] = bottleneckCounts.TryGetValue(pb, out int n) ? n + 1 : 1;
                        }
                    }
                    foreach (var kv in bottleneckCounts.OrderByDescending(kv => kv.Value))
                        sb.AppendLine($"  {kv.Key}: 出现 {kv.Value} 次");
                    sb.AppendLine();
                }

                string? resp = _llm!.Chat(sb.ToString());
                if (!string.IsNullOrWhiteSpace(resp)) return resp!.Trim();
            }
            catch (Exception ex)
            {
                _logger.Info($"    [B2] Overall recommendations LLM error: {ex.Message}");
            }
            return "*(LLM未能返回综合建议)*";
        }

        private static void AppendRuleBasedRecommendations(StringBuilder sb, DrawCallAnalysisReport report, JObject? statusRoot)
        {
            sb.AppendLine("*(LLM未启用，仅显示规则归因结论)*\n");
            if (statusRoot?["category_stats"] is JArray catArr)
            {
                var top3 = catArr.OrderByDescending(c => c["clocks_sum"]?.ToObject<long>() ?? 0).Take(3);
                foreach (var c in top3)
                {
                    string cat = c["category"]?.ToString() ?? "?";
                    sb.AppendLine($"- **{cat}** 是主要 clock 消耗来源（{c["clocks_pct"]}%），建议优先分析该分类的 TopDC 归因。");
                }
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        private static JObject? LoadJson(string dir, string fileName)
        {
            string path = Path.Combine(dir, fileName);
            if (!File.Exists(path)) return null;
            try { return JObject.Parse(File.ReadAllText(path, Encoding.UTF8)); }
            catch { return null; }
        }

        private static double GetClocksPct(JObject? statusRoot, string cat)
        {
            if (statusRoot?["category_stats"] is not JArray arr) return 0.0;
            var entry = arr.FirstOrDefault(c => c["category"]?.ToString() == cat);
            return entry?["clocks_pct"]?.ToObject<double>() ?? 0.0;
        }

        /// <summary>Fallback when topdc.json doesn't exist — build a minimal categories array from report.</summary>
        private static JArray BuildCategoriesFromReport(DrawCallAnalysisReport report)
        {
            var arr = new JArray();
            foreach (var grp in report.DrawCallResults.GroupBy(d => d.Label.Category).OrderBy(g => g.Key))
            {
                var dcArr = new JArray();
                int rank  = 1;
                foreach (var dc in grp.Where(d => d.Metrics != null)
                                       .OrderByDescending(d => d.Metrics!.Clocks).Take(5))
                {
                    dcArr.Add(new JObject
                    {
                        ["dc_id"]   = dc.DrawCallNumber,
                        ["rank"]    = rank++,
                        ["clocks"]  = dc.Metrics!.Clocks,
                        ["label"]   = new JObject { ["detail"] = dc.Label.Detail },
                        ["attribution"] = new JObject()
                    });
                }
                arr.Add(new JObject
                {
                    ["category"] = grp.Key,
                    ["dc_count"] = grp.Count(),
                    ["top_dcs"]  = dcArr
                });
            }
            return arr;
        }
    }
}
