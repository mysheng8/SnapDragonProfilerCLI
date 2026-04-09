using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>Pre-computed output from StatusJsonService, passed directly to TopDcJsonService
    /// to avoid re-parsing the written JSON file.</summary>
    public class StatusJsonResult
    {
        public string FilePath { get; set; } = "";
        public Newtonsoft.Json.Linq.JObject GlobalPercentiles { get; set; } = new Newtonsoft.Json.Linq.JObject();
        public Dictionary<string, (Newtonsoft.Json.Linq.JObject Stats, int SampleSize)> CategoryStatsMap { get; set; }
            = new Dictionary<string, (Newtonsoft.Json.Linq.JObject Stats, int SampleSize)>();
    }

    /// <summary>
    /// Pass A Step A5 — aggregates per-capture and per-category statistics from a
    /// DrawCallAnalysisReport (which must already have Metrics joined) and writes
    /// snapshot_{captureId}_status.json to captureOutDir.
    ///
    /// Key additions over existing MD output:
    ///   - p50 / p80 / p95 percentiles per metric, global and per-category
    ///   - label quality stats (confidence distribution, reason_tag histogram)
    ///   - global_percentiles block consumed by TopDcJsonService / AttributionRuleEngine
    /// </summary>
    public class StatusJsonService
    {
        // All metric property names that appear in DrawCallMetrics, in consistent order
        private static readonly string[] MetricNames = new[]
        {
            "clocks", "read_total_bytes", "write_total_bytes",
            "fragments_shaded", "vertices_shaded",
            "shaders_busy_pct", "tex_l1_miss_pct", "tex_l2_miss_pct",
            "tex_fetch_stall_pct", "fragment_instructions", "vertex_instructions",
            "tex_mem_read_bytes", "vertex_mem_read_bytes"
        };

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Computes stats from <paramref name="report"/> and writes
        /// snapshot_{captureId}_status.json into <paramref name="captureOutDir"/>.
        /// Returns a <see cref="StatusJsonResult"/> with the file path AND the
        /// pre-computed percentile tables — pass directly to TopDcJsonService to
        /// avoid re-parsing the written file.
        /// </summary>
        public StatusJsonResult GenerateStatusJson(
            DrawCallAnalysisReport report,
            string captureOutDir,
            uint   captureId = 0,
            string sdpName   = "")
        {
            Directory.CreateDirectory(captureOutDir);

            var dcs   = report.DrawCallResults;
            var withM = dcs.Where(d => d.Metrics != null).ToList();

            // ── Overall stats ────────────────────────────────────────────────
            long totalClocks    = withM.Sum(d => d.Metrics!.Clocks);
            long totalReadBytes = withM.Sum(d => d.Metrics!.ReadTotalBytes);
            long totalWriteBytes= withM.Sum(d => d.Metrics!.WriteTotalBytes);
            long totalFragments = withM.Sum(d => d.Metrics!.FragmentsShaded);
            long totalVertices  = withM.Sum(d => d.Metrics!.VerticesShaded);
            int  drawCount      = dcs.Count(d => d.ApiName.StartsWith("vkCmdDraw", StringComparison.OrdinalIgnoreCase));
            int  computeCount   = dcs.Count(d => d.ApiName.StartsWith("vkCmdDispatch", StringComparison.OrdinalIgnoreCase));
            double coverage     = dcs.Count == 0 ? 0.0 : Math.Round((double)withM.Count / dcs.Count, 4);

            var overall = new JObject
            {
                ["total_dc_count"]         = dcs.Count,
                ["draw_dc_count"]          = drawCount,
                ["compute_dc_count"]       = computeCount,
                ["total_clocks"]           = totalClocks,
                ["total_read_bytes"]       = totalReadBytes,
                ["total_write_bytes"]      = totalWriteBytes,
                ["total_fragments_shaded"] = totalFragments,
                ["total_vertices_shaded"]  = totalVertices,
                ["metrics_coverage_ratio"] = coverage
            };

            // ── Global percentiles (across all DCs that have metrics) ─────────
            var globalPercentiles = BuildPercentileBlock(withM);

            // ── Per-category stats ────────────────────────────────────────────
            var categories  = dcs.GroupBy(d => d.Label.Category).OrderBy(g => g.Key);
            var catStatsArr = new JArray();
            foreach (var grp in categories)
            {
                string cat      = grp.Key;
                var    catDcs   = grp.ToList();
                var    catWithM = catDcs.Where(d => d.Metrics != null).ToList();

                long catClocks  = catWithM.Sum(d => d.Metrics!.Clocks);
                double clocksPct = totalClocks == 0 ? 0.0 : Math.Round(100.0 * catClocks / totalClocks, 2);
                double pct       = dcs.Count  == 0 ? 0.0 : Math.Round(100.0 * catDcs.Count / dcs.Count, 2);

                var catObj = new JObject
                {
                    ["category"]   = cat,
                    ["dc_count"]   = catDcs.Count,
                    ["percentage"] = pct,
                    ["clocks_sum"] = catClocks,
                    ["clocks_pct"] = clocksPct
                };

                if (catWithM.Count > 0)
                {
                    catObj["metrics_p50"] = BuildPercentilesAtLevel(catWithM, 0.50);
                    catObj["metrics_p60"] = BuildPercentilesAtLevel(catWithM, 0.60);
                    catObj["metrics_p70"] = BuildPercentilesAtLevel(catWithM, 0.70);
                    catObj["metrics_p80"] = BuildPercentilesAtLevel(catWithM, 0.80);
                    catObj["metrics_p90"] = BuildPercentilesAtLevel(catWithM, 0.90);
                    catObj["metrics_p95"] = BuildPercentilesAtLevel(catWithM, 0.95);
                    catObj["metrics_p99"] = BuildPercentilesAtLevel(catWithM, 0.99);
                }

                catStatsArr.Add(catObj);
            }

            // ── Label quality stats ───────────────────────────────────────────
            var labelStats = BuildLabelStats(dcs);

            // ── Root object ───────────────────────────────────────────────────
            var root = new JObject
            {
                ["schema_version"]    = "2.0",
                ["snapshot_id"]       = captureId,
                ["sdp_name"]          = sdpName,
                ["generated_at"]      = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                ["overall"]           = overall,
                ["category_stats"]    = catStatsArr,
                ["label_stats"]       = labelStats,
                ["global_percentiles"]= globalPercentiles
            };

            // ── Write file ────────────────────────────────────────────────────
            string fileName = captureId > 0
                ? $"snapshot_{captureId}_status.json"
                : $"DrawCallStatus_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string outPath = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(outPath, root.ToString(Formatting.Indented), System.Text.Encoding.UTF8);

            // ── Build CategoryStatsMap for direct use by TopDcJsonService ─────
            var catStatsMap = new Dictionary<string, (JObject Stats, int SampleSize)>();
            foreach (var token in catStatsArr)
            {
                string? cName = token["category"]?.ToString();
                int     cSize = token["dc_count"]?.ToObject<int>() ?? 0;
                if (!string.IsNullOrEmpty(cName) && token is JObject cObj)
                    catStatsMap[cName!] = (cObj, cSize);
            }

            return new StatusJsonResult
            {
                FilePath         = outPath,
                GlobalPercentiles= globalPercentiles,
                CategoryStatsMap = catStatsMap
            };
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        private static JObject BuildAvgBlock(List<DrawCallInfo> dcs)
        {
            if (dcs.Count == 0) return new JObject();
            // Collect all counter names present across DCs (respects MetricsWhitelist)
            var counterNames = dcs
                .SelectMany(d => d.Metrics!.All.Keys)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(k => k)
                .ToList();
            var obj = new JObject();
            foreach (var cn in counterNames)
            {
                var vals = dcs
                    .Where(d => d.Metrics!.All.ContainsKey(cn))
                    .Select(d => d.Metrics!.All[cn])
                    .ToList();
                if (vals.Count == 0) continue;
                string key = DrawCallMetrics.NormalizeKey(cn);
                double avg = vals.Average();
                // Round percentages to 2 dp; byte/count fields to nearest long
                obj[key] = cn.StartsWith("%") || cn.EndsWith("Pct") || cn.Contains("Miss") || cn.Contains("Stall")
                    ? (JToken)Math.Round(avg, 2)
                    : (JToken)(long)avg;
            }
            return obj;
        }

        private static JObject BuildPercentilesAtLevel(List<DrawCallInfo> dcs, double level)
        {
            if (dcs.Count == 0) return new JObject();
            var counterNames = dcs
                .SelectMany(d => d.Metrics!.All.Keys)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(k => k)
                .ToList();
            var obj = new JObject();
            foreach (var cn in counterNames)
            {
                var vals = dcs
                    .Where(d => d.Metrics!.All.ContainsKey(cn))
                    .Select(d => d.Metrics!.All[cn])
                    .ToList();
                if (vals.Count == 0) continue;
                string key = DrawCallMetrics.NormalizeKey(cn);
                double p = Percentile(vals, level);
                obj[key] = cn.StartsWith("%") || cn.EndsWith("Pct") || cn.Contains("Miss") || cn.Contains("Stall")
                    ? (JToken)Math.Round(p, 2)
                    : (JToken)p;
            }
            return obj;
        }

        /// <summary>
        /// Builds the global_percentiles block: for each metric present in data, records p70/p80/p95/p99.
        /// Keys are snake_case (via DrawCallMetrics.NormalizeKey), matching attribution_rules.json.
        /// Dynamically covers all counters in MetricsWhitelist that are present in the data.
        /// </summary>
        public static JObject BuildPercentileBlock(List<DrawCallInfo> dcs)
        {
            if (dcs.Count == 0) return new JObject();

            JObject MakeEntry(List<double> values) => new JObject
            {
                ["p50"] = Math.Round(Percentile(values, 0.50), 4),
                ["p60"] = Math.Round(Percentile(values, 0.60), 4),
                ["p70"] = Math.Round(Percentile(values, 0.70), 4),
                ["p80"] = Math.Round(Percentile(values, 0.80), 4),
                ["p90"] = Math.Round(Percentile(values, 0.90), 4),
                ["p95"] = Math.Round(Percentile(values, 0.95), 4),
                ["p99"] = Math.Round(Percentile(values, 0.99), 4),
            };

            var counterNames = dcs
                .SelectMany(d => d.Metrics!.All.Keys)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(k => k)
                .ToList();

            var result = new JObject();
            foreach (var cn in counterNames)
            {
                var vals = dcs
                    .Where(d => d.Metrics!.All.ContainsKey(cn))
                    .Select(d => d.Metrics!.All[cn])
                    .ToList();
                if (vals.Count == 0) continue;
                string key = DrawCallMetrics.NormalizeKey(cn);
                result[key] = MakeEntry(vals);
            }
            return result;
        }

        private static JObject BuildLabelStats(List<DrawCallInfo> dcs)
        {
            if (dcs.Count == 0) return new JObject();
            const double lowThreshold = 0.60;
            double avgConf  = dcs.Average(d => d.Label.Confidence);
            double lowRatio = (double)dcs.Count(d => d.Label.Confidence < lowThreshold) / dcs.Count;
            int    llmCount = dcs.Count(d => d.Label.LabelSource == "llm");
            int    ruleCount= dcs.Count(d => d.Label.LabelSource != "llm");

            // reason_tag distribution
            var tagDist = new Dictionary<string, int>();
            foreach (var dc in dcs)
                foreach (var tag in dc.Label.ReasonTags)
                    tagDist[tag] = tagDist.TryGetValue(tag, out int v) ? v + 1 : 1;
            var tagObj = new JObject();
            foreach (var kv in tagDist.OrderByDescending(kv => kv.Value))
                tagObj[kv.Key] = kv.Value;

            var lowConfIds = dcs
                .Where(d => d.Label.Confidence < lowThreshold)
                .OrderBy(d => d.Label.Confidence)
                .Take(20)
                .Select(d => d.DrawCallNumber)
                .ToList();

            return new JObject
            {
                ["avg_confidence"]           = Math.Round(avgConf, 4),
                ["low_confidence_ratio"]     = Math.Round(lowRatio, 4),
                ["low_confidence_threshold"] = lowThreshold,
                ["llm_labeled_count"]        = llmCount,
                ["rule_labeled_count"]       = ruleCount,
                ["reason_tag_distribution"]  = tagObj,
                ["low_confidence_dc_ids"]    = new JArray(lowConfIds.Cast<object>().ToArray())
            };
        }

        /// <summary>Nearest-rank percentile (0-based sorted array).</summary>
        public static double Percentile(List<double> values, double p)
        {
            if (values.Count == 0) return 0.0;
            var sorted = values.OrderBy(v => v).ToList();
            int idx    = (int)Math.Floor(p * sorted.Count);
            if (idx >= sorted.Count) idx = sorted.Count - 1;
            return sorted[idx];
        }
    }
}
