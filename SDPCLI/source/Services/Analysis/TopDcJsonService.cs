using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Pass A Step A6 — selects top-N DCs per category ranked by Clocks, runs
    /// AttributionRuleEngine three-layer analysis, and writes
    /// snapshot_{captureId}_topdc.json to captureOutDir.
    ///
    /// Depends on:
    ///   - DrawCallAnalysisReport with Metrics joined
    ///   - statusGlobalPercentiles  (from StatusJsonService output or pre-computed)
    ///   - statusCategoryPercentiles per category
    ///   - AttributionRuleEngine loaded from attribution_rules.json
    /// </summary>
    public class TopDcJsonService
    {
        private readonly AttributionRuleEngine _engine;

        public TopDcJsonService(AttributionRuleEngine engine)
        {
            _engine = engine;
        }

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Generates topdc.json from the report and pre-computed percentile tables
        /// that come out of <see cref="StatusJsonService"/>.
        ///
        /// <paramref name="globalPercentiles"/>  — root["global_percentiles"] from status.json
        /// <paramref name="categoryPercentilesMap"/> — map of category-name → root["category_stats"][n]
        ///   (the whole category stats object, which contains metrics_p80, metrics_p95… keyed
        ///   under the metric names)
        /// </summary>
        public string GenerateTopDcJson(
            DrawCallAnalysisReport report,
            JObject  globalPercentiles,
            Dictionary<string, (JObject Stats, int SampleSize)> categoryStatsMap,
            string   captureOutDir,
            string   shaderBaseDir = "",
            string   meshBaseDir   = "",
            uint     captureId     = 0,
            string   sdpName       = "")
        {
            Directory.CreateDirectory(captureOutDir);
            int topN = _engine.TopNPerCategory;

            var categoriesArr = new JArray();
            var categories    = report.DrawCallResults
                .Where(d => d.Metrics != null)
                .GroupBy(d => d.Label.Category)
                .OrderBy(g => g.Key);

            foreach (var grp in categories)
            {
                string cat     = grp.Key;
                var    catDcs  = grp.ToList();

                // Build category percentile lookup block for AttributionRuleEngine
                JObject? catPercBlock = null;
                int      catSample   = 0;
                if (categoryStatsMap.TryGetValue(cat, out var catInfo))
                {
                    catPercBlock = BuildCategoryPercentileBlock(catInfo.Stats);
                    catSample    = catInfo.SampleSize;
                }

                // Top-N by Clocks descending
                var topDcs = catDcs
                    .OrderByDescending(d => d.Metrics!.Clocks)
                    .Take(topN)
                    .ToList();

                var topDcsArr = new JArray();
                for (int i = 0; i < topDcs.Count; i++)
                {
                    var dc   = topDcs[i];
                    var attr = _engine.Attribute(dc, catPercBlock, globalPercentiles, catSample);

                    // Category-relative percentile ranks
                    int clocksRankPct = PercentileRankInGroup(dc.Metrics!.Clocks, catDcs, d => d.Metrics!.Clocks);
                    int texRankPct    = PercentileRankInGroup(dc.Metrics.TexFetchStallPct, catDcs, d => d.Metrics!.TexFetchStallPct);
                    int shaderRankPct = PercentileRankInGroup(dc.Metrics.ShadersBusyPct,   catDcs, d => d.Metrics!.ShadersBusyPct);

                    var dcObj = new JObject
                    {
                        ["dc_id"]  = dc.DrawCallNumber,
                        ["rank"]   = i + 1,
                        ["clocks"] = dc.Metrics!.Clocks,
                        ["clocks_rank_in_category"] = i + 1,
                        ["metrics"] = BuildMetricsNode(dc.Metrics),
                        ["attribution"] = attr != null ? BuildAttributionNode(attr) : new JObject(),
                        ["category_comparison"] = new JObject
                        {
                            ["clocks_percentile_in_category"]          = clocksRankPct,
                            ["tex_fetch_stall_percentile_in_category"] = texRankPct,
                            ["shaders_busy_percentile_in_category"]    = shaderRankPct
                        },
                        ["shader_files"] = BuildShaderFilesArray(dc, shaderBaseDir, captureOutDir),
                        ["mesh_file"]    = ResolveMeshFile(dc, meshBaseDir, captureOutDir),
                        ["label"]        = BuildLabelNode(dc.Label)
                    };
                    topDcsArr.Add(dcObj);
                }

                categoriesArr.Add(new JObject
                {
                    ["category"] = cat,
                    ["dc_count"] = catDcs.Count,
                    ["top_dcs"]  = topDcsArr
                });
            }

            var root = new JObject
            {
                ["schema_version"]       = "2.0",
                ["snapshot_id"]          = captureId,
                ["sdp_name"]             = sdpName,
                ["generated_at"]         = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                ["top_n_per_category"]   = topN,
                ["categories"]           = categoriesArr
            };

            string fileName = captureId > 0
                ? $"snapshot_{captureId}_topdc.json"
                : $"DrawCallTopDc_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string outPath = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(outPath, root.ToString(Formatting.Indented), System.Text.Encoding.UTF8);
            return outPath;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Converts a category_stats entry (which has metrics_p80 / metrics_p95 subobjects
        /// keyed by metric name) into a flat metric → {p70,p80,p95} object that
        /// AttributionRuleEngine expects.
        /// </summary>
        private static JObject? BuildCategoryPercentileBlock(JObject catStats)
        {
            var p70 = catStats["metrics_p70"] as JObject;
            var p80 = catStats["metrics_p80"] as JObject;
            var p95 = catStats["metrics_p95"] as JObject;
            if (p80 == null || p95 == null) return null;

            var block = new JObject();
            foreach (var prop in p80.Properties())
            {
                block[prop.Name] = new JObject
                {
                    ["p70"] = p70?[prop.Name] ?? 0,
                    ["p80"] = p80[prop.Name],
                    ["p95"] = p95[prop.Name] ?? 0
                };
            }
            return block;
        }

        private static JObject BuildAttributionNode(AttributionResult attr)
        {
            var suspArr = new JArray(attr.SuspiciousMetrics.Select(s => new JObject
            {
                ["metric"]                   = s.Metric,
                ["value"]                    = s.Value,
                ["initial_bottleneck_hint"]  = s.InitialBottleneckHint
            }).Cast<object>().ToArray());

            var psArr = new JArray(attr.PercentileScores.Select(ps => new JObject
            {
                ["metric"]            = ps.Metric,
                ["value"]             = ps.Value,
                ["category_p95"]      = ps.CategoryP95,
                ["global_p95"]        = ps.GlobalP95,
                ["percentile_tier"]   = ps.PercentileTierName,
                ["weight_applied"]    = ps.WeightApplied,
                ["bottleneck_targets"]= new JArray(ps.BottleneckTargets.Cast<object>().ToArray())
            }).Cast<object>().ToArray());

            var scoresObj = new JObject();
            foreach (var kv in attr.BottleneckScores.OrderByDescending(kv => kv.Value))
                scoresObj[kv.Key] = Math.Round(kv.Value, 4);

            return new JObject
            {
                ["suspicious_metrics"]   = suspArr,
                ["percentile_scores"]    = psArr,
                ["bottleneck_scores"]    = scoresObj,
                ["primary_bottleneck"]   = attr.PrimaryBottleneck,
                ["secondary_bottleneck"] = attr.SecondaryBottleneck,
                ["confidence_score"]     = Math.Round(attr.ConfidenceScore, 4)
            };
        }

        private static JObject BuildMetricsNode(DrawCallMetrics m)
        {
            var obj = new JObject();
            foreach (var kv in m.All.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase))
                obj[DrawCallMetrics.NormalizeKey(kv.Key)] = kv.Value;
            return obj;
        }

        private static JObject BuildLabelNode(DrawCallLabel lbl) => new JObject
        {
            ["category"]     = lbl.Category,
            ["subcategory"]  = lbl.Subcategory,
            ["detail"]       = lbl.Detail,
            ["reason_tags"]  = new JArray(lbl.ReasonTags.Cast<object>().ToArray()),
            ["confidence"]   = lbl.Confidence,
            ["label_source"] = lbl.LabelSource
        };

        private static JArray BuildShaderFilesArray(DrawCallInfo dc, string shaderBaseDir, string captureOutDir)
        {
            var arr = new JArray();
            if (string.IsNullOrEmpty(shaderBaseDir)) return arr;
            foreach (var s in dc.Shaders)
            {
                string stagePfx = s.ShaderStageName?.ToLowerInvariant().StartsWith("vert") == true ? "vert" : "frag";
                string candidate= Path.Combine(shaderBaseDir, $"pipeline_{dc.PipelineID}_{stagePfx}.hlsl");
                if (File.Exists(candidate))
                {
                    string rel = MakeRelative(candidate, captureOutDir);
                    arr.Add(rel);
                }
            }
            return arr;
        }

        private static string ResolveMeshFile(DrawCallInfo dc, string meshBaseDir, string captureOutDir)
        {
            if (string.IsNullOrEmpty(meshBaseDir)) return "";
            string candidate = Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj");
            if (File.Exists(candidate))
                return MakeRelative(candidate, captureOutDir);
            return "";
        }

        /// <summary>
        /// Hand-rolled relative-path helper compatible with .NET Framework 4.x
        /// (Path.GetRelativePath is .NET Core 2.0+ only).
        /// </summary>
        private static string MakeRelative(string absPath, string fromDir)
        {
            try
            {
                // Ensure both paths end without trailing separator for Uri comparison
                string from = fromDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
                var fromUri = new Uri(from);
                var toUri   = new Uri(absPath);
                Uri relUri  = fromUri.MakeRelativeUri(toUri);
                return Uri.UnescapeDataString(relUri.ToString()).Replace('/', Path.DirectorySeparatorChar).Replace('\\', '/');
            }
            catch { return absPath.Replace('\\', '/'); }
        }

        /// <summary>Returns the percentile rank (0-99) of <paramref name="value"/> in the group.</summary>
        private static int PercentileRankInGroup<T>(
            T value,
            List<DrawCallInfo> group,
            Func<DrawCallInfo, T> selector) where T : IComparable<T>
        {
            if (group.Count == 0) return 0;
            int below = group.Count(d => selector(d).CompareTo(value) < 0);
            return (int)Math.Round(100.0 * below / group.Count);
        }
    }
}
