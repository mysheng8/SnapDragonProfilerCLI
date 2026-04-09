using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Loads attribution_rules.json and executes the three-layer bottleneck attribution
    /// computation for a single DrawCallInfo against pre-computed percentile tables.
    ///
    /// Layer 1 — metric → bottleneck hint (when metric exceeds p70 threshold)
    /// Layer 2 — choose percentile tier (p70/p80/p95), resolve weight; prefer category
    ///           percentiles, fall back to global when category sample < min_sample_size
    /// Layer 3 — weighted sum per bottleneck → primary / secondary bottleneck + score
    /// </summary>
    public class AttributionRuleEngine
    {
        // ── Layer 1 ────────────────────────────────────────────────────────────
        private record MetricHint(string Metric, string BottleneckHint);
        private readonly List<MetricHint> _layer1;

        // ── Layer 2 ────────────────────────────────────────────────────────────
        private record PercentileTier(string Name, double Threshold, double Weight);
        private readonly List<PercentileTier> _tiers;
        private readonly int _minSampleForCategory;

        // ── Layer 3 ────────────────────────────────────────────────────────────
        private record BottleneckDef(
            string Bottleneck,
            string DisplayName,
            List<(string Metric, double ContribWeight)> Contributors);
        private readonly List<BottleneckDef> _layer3;

        public double PrimaryBottleneckMinScore { get; }
        public int    TopNPerCategory           { get; }

        // ──────────────────────────────────────────────────────────────────────
        public AttributionRuleEngine(string rulesJsonPath)
        {
            if (!File.Exists(rulesJsonPath))
                throw new FileNotFoundException($"attribution_rules.json not found: {rulesJsonPath}");

            var root = JObject.Parse(File.ReadAllText(rulesJsonPath, System.Text.Encoding.UTF8));

            // Layer 1
            _layer1 = (root["layer1_metric_hints"] as JArray ?? new JArray())
                .Select(t => new MetricHint(
                    t["metric"]!.ToString(),
                    t["bottleneck_hint"]!.ToString()))
                .ToList();

            // Layer 2
            var l2 = root["layer2_percentile_tiers"]!;
            _minSampleForCategory = l2["min_sample_size_for_category"]?.ToObject<int>() ?? 5;
            _tiers = (l2["tiers"] as JArray ?? new JArray())
                .OrderByDescending(t => t["threshold"]!.ToObject<double>())
                .Select(t => new PercentileTier(
                    t["name"]!.ToString(),
                    t["threshold"]!.ToObject<double>(),
                    t["weight"]!.ToObject<double>()))
                .ToList();

            // Layer 3
            _layer3 = (root["layer3_bottleneck_weights"] as JArray ?? new JArray())
                .Select(b => new BottleneckDef(
                    b["bottleneck"]!.ToString(),
                    b["display_name"]?.ToString() ?? b["bottleneck"]!.ToString(),
                    (b["contributing_metrics"] as JArray ?? new JArray())
                        .Select(m => (m["metric"]!.ToString(), m["contribution_weight"]!.ToObject<double>()))
                        .ToList()))
                .ToList();

            PrimaryBottleneckMinScore = root["primary_bottleneck_min_score"]?.ToObject<double>() ?? 0.25;
            TopNPerCategory           = root["top_n_per_category"]?.ToObject<int>() ?? 5;
        }

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Run three-layer attribution for a single DC.
        /// </summary>
        /// <param name="dc">DC with Metrics already joined.</param>
        /// <param name="categoryPercentiles">
        ///   Percentile table for this DC's category: metric → {p70,p80,p95}.
        ///   Null values or missing keys fall back to <paramref name="globalPercentiles"/>.
        /// </param>
        /// <param name="globalPercentiles">Global percentile table: metric → {p70,p80,p95}.</param>
        /// <param name="categorySampleSize">Number of DCs with metrics in this category.</param>
        /// <returns>Attribution result, or null if DC has no metrics.</returns>
        public AttributionResult? Attribute(
            DrawCallInfo dc,
            JObject?     categoryPercentiles,
            JObject      globalPercentiles,
            int          categorySampleSize)
        {
            if (dc.Metrics == null) return null;

            var mv = GetMetricValues(dc.Metrics);

            // ── Layer 1: suspicious metrics ──────────────────────────────────
            var suspicious = new List<SuspiciousMetric>();
            foreach (var hint in _layer1)
            {
                if (!mv.TryGetValue(hint.Metric, out double val)) continue;
                double p70 = GetPercentile(hint.Metric, "p70", categoryPercentiles, globalPercentiles, categorySampleSize);
                if (p70 > 0 && val > p70)
                    suspicious.Add(new SuspiciousMetric(hint.Metric, val, hint.BottleneckHint));
            }

            // ── Layer 2: percentile scores ────────────────────────────────────
            var percentileScores = new List<PercentileScore>();
            foreach (var s in suspicious)
            {
                // Find highest tier exceeded
                string  tierName      = "";
                double  weightApplied = 0;
                foreach (var tier in _tiers)
                {
                    double threshold = GetPercentile(s.Metric, tier.Name, categoryPercentiles, globalPercentiles, categorySampleSize);
                    if (threshold > 0 && s.Value > threshold)
                    {
                        tierName      = tier.Name;
                        weightApplied = tier.Weight;
                        break; // tiers ordered descending — take highest
                    }
                }
                if (tierName == "") continue;

                double catP95  = GetPercentile(s.Metric, "p95", categoryPercentiles, globalPercentiles, categorySampleSize);
                double globP95 = GetRawPercentile(s.Metric, "p95", globalPercentiles);

                percentileScores.Add(new PercentileScore(
                    s.Metric, s.Value, catP95, globP95, tierName, weightApplied,
                    new[] { s.InitialBottleneckHint }));
            }

            // ── Layer 3: weighted bottleneck scores ───────────────────────────
            // Build a lookup: metric → sum of weighted contributions already recorded
            var scoreAccumulator = new Dictionary<string, double>();
            foreach (var b in _layer3)
                scoreAccumulator[b.Bottleneck] = 0.0;

            foreach (var ps in percentileScores)
            {
                foreach (var target in ps.BottleneckTargets)
                {
                    // Find contribution_weight for this metric→bottleneck
                    var bDef = _layer3.FirstOrDefault(b => b.Bottleneck == target);
                    if (bDef == null) continue;
                    var contrib = bDef.Contributors.FirstOrDefault(c => c.Metric == ps.Metric);
                    double contribW = contrib.Metric != null ? contrib.ContribWeight : 1.0;
                    scoreAccumulator[target] += ps.WeightApplied * contribW;
                }
            }

            var bottleneckScores = scoreAccumulator
                .Where(kv => kv.Value > 0)
                .OrderByDescending(kv => kv.Value)
                .ToList();

            string primary   = "";
            string secondary = "";
            double topScore  = 0;
            if (bottleneckScores.Count > 0)
            {
                primary  = bottleneckScores[0].Key;
                topScore = bottleneckScores[0].Value;
                if (bottleneckScores.Count > 1)
                    secondary = bottleneckScores[1].Key;
            }

            return new AttributionResult(
                suspicious,
                percentileScores,
                scoreAccumulator.Where(kv => kv.Value > 0).ToDictionary(kv => kv.Key, kv => kv.Value),
                topScore >= PrimaryBottleneckMinScore ? primary : "",
                secondary,
                topScore);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        private double GetPercentile(
            string   metric,
            string   tier,
            JObject? catPercentiles,
            JObject  globalPercentiles,
            int      catSampleSize)
        {
            bool useCat = catSampleSize >= _minSampleForCategory
                       && catPercentiles != null
                       && catPercentiles[metric] != null;
            var src = useCat ? catPercentiles! : globalPercentiles;
            return GetRawPercentile(metric, tier, src);
        }

        private static double GetRawPercentile(string metric, string tier, JObject? src)
        {
            return src?[metric]?[tier]?.ToObject<double>() ?? 0.0;
        }

        private static Dictionary<string, double> GetMetricValues(DrawCallMetrics m) =>
            m.All.ToDictionary(
                kv => DrawCallMetrics.NormalizeKey(kv.Key),
                kv => kv.Value,
                StringComparer.OrdinalIgnoreCase);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Value objects for attribution result
    // ──────────────────────────────────────────────────────────────────────────

    public record SuspiciousMetric(
        string Metric,
        double Value,
        string InitialBottleneckHint);

    public record PercentileScore(
        string   Metric,
        double   Value,
        double   CategoryP95,
        double   GlobalP95,
        string   PercentileTierName,
        double   WeightApplied,
        string[] BottleneckTargets);

    public record AttributionResult(
        List<SuspiciousMetric>      SuspiciousMetrics,
        List<PercentileScore>       PercentileScores,
        Dictionary<string, double>  BottleneckScores,
        string                      PrimaryBottleneck,
        string                      SecondaryBottleneck,
        double                      ConfidenceScore);
}
