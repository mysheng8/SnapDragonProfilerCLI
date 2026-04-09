using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Pass B Step B1 — per-DC LLM content analysis.
    ///
    /// For each draw call, reads extracted shader source files and builds a compact
    /// LLM prompt that returns a structured JSON with:
    ///   pass_name, technique, key_features[], complexity_notes, optimization_hints[]
    ///
    /// Results are cached in captureOutDir/per_dc_content/dc_{dcId}.json,
    /// keyed by pipeline_id so multiple DCs sharing a pipeline incur only one LLM call.
    ///
    /// Falls back to a rule-based response when LLM is unavailable or fails.
    /// </summary>
    public class DcContentAnalysisService
    {
        private readonly Config          _config;
        private readonly ILogger         _logger;
        private readonly Tools.LlmApiWrapper? _llm;

        // pipeline_id → analysis result (in-process cache; ConcurrentDictionary for parallel-safe writes)
        private readonly ConcurrentDictionary<uint, JObject> _pipelineCache = new ConcurrentDictionary<uint, JObject>();

        public DcContentAnalysisService(Config config, ILogger logger, Tools.LlmApiWrapper? llm)
        {
            _config = config;
            _logger = logger;
            _llm    = llm;
        }

        // ──────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Runs B1 analysis for the top-N costliest DCs per category (config key B1TopNPerCategory,
        /// default 5). LLM calls are deduplicated by pipeline_id and executed in parallel
        /// (config key B1ParallelDegree, default 4).
        /// Writes per-DC JSON to captureOutDir/per_dc_content/.
        /// Returns the number of DC cache files written.
        /// </summary>
        public int AnalyzeAll(
            DrawCallAnalysisReport report,
            string captureOutDir,
            string shaderBaseDir)
        {
            string cacheDir    = Path.Combine(captureOutDir, "per_dc_content");
            Directory.CreateDirectory(cacheDir);

            int topN        = _config.GetInt("B1TopNPerCategory",  5);
            int parallelDeg = _config.GetInt("B1ParallelDegree",   4);

            // ── Select top-N per category by Clocks descending ───────────────
            var selected = report.DrawCallResults
                .Where(dc => dc.Metrics != null)
                .GroupBy(dc => dc.Label.Category)
                .SelectMany(g => g.OrderByDescending(dc => dc.Metrics!.Clocks).Take(topN))
                .ToList();

            if (selected.Count == 0) return 0;

            _logger.Info($"  [B1] Selected {selected.Count} DCs across categories (top {topN} per category)");

            // ── Deduplicate by pipeline_id; one LLM call per unique pipeline ─
            // Map pipeline_id → representative DC for LLM analysis
            var representativeByPipeline = selected
                .GroupBy(dc => dc.PipelineID)
                .ToDictionary(g => g.Key, g => g.First());

            // Separate already-cached pipelines from those needing analysis
            var toAnalyze = new List<DrawCallInfo>();
            foreach (var kvp in representativeByPipeline)
            {
                uint pid = kvp.Key;
                var  rep = kvp.Value;
                if (_pipelineCache.ContainsKey(pid)) continue;

                string repFile = Path.Combine(cacheDir, $"dc_{SanitizeDcId(rep.DrawCallNumber)}.json");
                if (File.Exists(repFile))
                {
                    try
                    {
                        var existing = JObject.Parse(File.ReadAllText(repFile, Encoding.UTF8));
                        _pipelineCache[pid] = existing;
                        continue;
                    }
                    catch { /* corrupt — re-analyse */ }
                }
                toAnalyze.Add(rep);
            }

            // ── Parallel LLM analysis for uncached pipelines ───────────────
            if (toAnalyze.Count > 0)
            {
                _logger.Info($"  [B1] Running LLM on {toAnalyze.Count} unique pipelines (degree={parallelDeg})");
                var opts = new ParallelOptions { MaxDegreeOfParallelism = parallelDeg };
                Parallel.ForEach(toAnalyze, opts, dc =>
                {
                    JObject result = AnalyzeSingle(dc, shaderBaseDir);
                    result["dc_id"]       = dc.DrawCallNumber;
                    result["pipeline_id"] = dc.PipelineID;
                    _pipelineCache[dc.PipelineID] = result;
                });
            }

            // ── Write per-DC cache files for all selected DCs ────────────────
            int written = 0;
            foreach (var dc in selected)
            {
                if (!_pipelineCache.TryGetValue(dc.PipelineID, out var pipelineResult)) continue;

                string cacheFile = Path.Combine(cacheDir, $"dc_{SanitizeDcId(dc.DrawCallNumber)}.json");
                var dcCopy = (JObject)pipelineResult.DeepClone();
                dcCopy["dc_id"]       = dc.DrawCallNumber;
                dcCopy["pipeline_id"] = dc.PipelineID;
                File.WriteAllText(cacheFile, dcCopy.ToString(Formatting.Indented), Encoding.UTF8);
                written++;
            }

            return written;
        }

        /// <summary>
        /// Returns the cached analysis for a specific DC (for use by AttributionReportService).
        /// Returns null when no analysis exists in captureOutDir.
        /// </summary>
        public JObject? GetCached(string dcId, string captureOutDir)
        {
            string cacheFile = Path.Combine(captureOutDir, "per_dc_content",
                $"dc_{SanitizeDcId(dcId)}.json");
            if (!File.Exists(cacheFile)) return null;
            try { return JObject.Parse(File.ReadAllText(cacheFile, Encoding.UTF8)); }
            catch { return null; }
        }

        // ──────────────────────────────────────────────────────────────────────
        // Single-DC analysis
        // ──────────────────────────────────────────────────────────────────────

        private JObject AnalyzeSingle(DrawCallInfo dc, string shaderBaseDir)
        {
            bool useLlm = _llm?.IsEnabled == true;

            if (useLlm)
            {
                try
                {
                    string prompt   = BuildPrompt(dc, shaderBaseDir);
                    string? resp    = _llm!.Chat(prompt);
                    if (!string.IsNullOrWhiteSpace(resp))
                    {
                        var parsed = TryParseResponse(resp!);
                        if (parsed != null) return parsed;
                    }
                    _logger.Info($"    [B1] DC {dc.DrawCallNumber} — LLM returned unparseable response, falling back");
                }
                catch (Exception ex)
                {
                    _logger.Info($"    [B1] DC {dc.DrawCallNumber} — LLM error: {ex.Message}, falling back");
                }
            }

            return BuildRuleBasedResult(dc);
        }

        private string BuildPrompt(DrawCallInfo dc, string shaderBaseDir)
        {
            int maxShaderChars = _config.GetInt("LlmMaxShaderChars", 20000);
            var sb = new StringBuilder();

            sb.AppendLine("你是Vulkan GPU性能工程师。请分析以下DrawCall的用途和渲染技术。");
            sb.AppendLine();
            sb.AppendLine("## DrawCall信息");
            sb.AppendLine($"dc_id: {dc.DrawCallNumber}");
            sb.AppendLine($"category: {dc.Label.Category} / {dc.Label.Subcategory}");
            sb.AppendLine($"api: {dc.ApiName}");
            sb.AppendLine($"draw_params: vertex={dc.VertexCount}, index={dc.IndexCount}, instance={dc.InstanceCount}");

            if (dc.RenderTargets.Count > 0)
            {
                var rt = dc.RenderTargets.FirstOrDefault(r => r.AttachmentType == "Color")
                      ?? dc.RenderTargets.First();
                sb.AppendLine($"render_target: {rt.Width}x{rt.Height} {rt.FormatName.Replace("VK_FORMAT_", "")}");
            }

            if (dc.Textures.Count > 0)
                sb.AppendLine($"textures: {dc.Textures.Count} bound ({string.Join(", ", dc.Textures.Take(5).Select(t => $"{t.Width}x{t.Height}"))})");

            // Shader source (pipeline-level files preferred, DC-level dc_ folder as fallback)
            sb.AppendLine();
            sb.AppendLine("## Shader代码（节选）");

            string dcShaderDir      = Path.Combine(shaderBaseDir, "dc_" + dc.DrawCallNumber);
            string pipelineShaderDir= shaderBaseDir;
            int    charsLeft        = maxShaderChars;
            bool   shaderAdded      = false;

            // Try pipeline-level files first (pipeline_{id}_vert.hlsl / _frag.hlsl)
            foreach (string stageSuffix in new[] { "vert", "frag", "comp" })
            {
                if (charsLeft <= 0) break;
                string candidate = Path.Combine(pipelineShaderDir, $"pipeline_{dc.PipelineID}_{stageSuffix}.hlsl");
                if (!File.Exists(candidate))
                    candidate = Path.Combine(pipelineShaderDir, $"pipeline_{dc.PipelineID}_{stageSuffix}.glsl");
                if (!File.Exists(candidate)) continue;

                AppendShaderSection(sb, candidate, ref charsLeft);
                shaderAdded = true;
            }

            // Fallback: dc-level directory
            if (!shaderAdded && Directory.Exists(dcShaderDir))
            {
                foreach (var f in Directory.GetFiles(dcShaderDir).OrderBy(x => x).Take(3))
                {
                    if (charsLeft <= 0) break;
                    AppendShaderSection(sb, f, ref charsLeft);
                }
            }

            sb.AppendLine();
            sb.AppendLine("## 要求");
            sb.AppendLine("以JSON格式返回（不要包含任何其他文字）：");
            sb.AppendLine("{");
            sb.AppendLine("  \"pass_name\": \"...\",");
            sb.AppendLine("  \"technique\": \"...\",");
            sb.AppendLine("  \"key_features\": [\"...\", \"...\", \"...\"],");
            sb.AppendLine("  \"complexity_notes\": \"...\",");
            sb.AppendLine("  \"optimization_hints\": [\"...\", \"...\"]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void AppendShaderSection(StringBuilder sb, string path, ref int charsLeft)
        {
            try
            {
                string src = File.ReadAllText(path, Encoding.UTF8);
                // Compact: keep declarations + main body
                string trimmed = ExtractMainBlock(src, charsLeft - 200);
                sb.AppendLine($"### {Path.GetFileName(path)}");
                sb.AppendLine("```");
                sb.AppendLine(trimmed);
                sb.AppendLine("```");
                charsLeft -= trimmed.Length + 50;
            }
            catch { }
        }

        private static string ExtractMainBlock(string src, int maxChars)
        {
            if (maxChars <= 0) return "[skipped — over token budget]";
            int mainPos = -1;
            foreach (string pat in new[] { " main(", "\nmain(", "\tmain(" })
            {
                int p = src.IndexOf(pat, StringComparison.Ordinal);
                if (p >= 0 && (mainPos < 0 || p < mainPos)) mainPos = p;
            }

            string preamble  = "";
            string mainBlock = src;
            if (mainPos > 0)
            {
                var decls = src.Substring(0, mainPos).Split('\n')
                    .Where(l => { var t = l.TrimStart();
                        return t.StartsWith("cbuffer") || t.StartsWith("struct ")     ||
                               t.StartsWith("Texture") || t.StartsWith("SamplerState")||
                               t.StartsWith("uniform ") || t.StartsWith("layout(")   ||
                               t.StartsWith("#define ") || t.StartsWith("in ")       ||
                               t.StartsWith("out ")    || t.StartsWith("Buffer<")    ||
                               t.StartsWith("RWBuffer") || t.StartsWith("RWTexture");
                    })
                    .Take(40);
                preamble  = string.Join("\n", decls);
                mainBlock = src.Substring(mainPos);
            }

            int budget = maxChars - preamble.Length - 20;
            if (budget > 0 && mainBlock.Length > budget)
                mainBlock = mainBlock.Substring(0, budget) + "\n... [truncated]";

            return (preamble.Length > 0 ? preamble + "\n// ---\n" : "") + mainBlock;
        }

        private static JObject? TryParseResponse(string resp)
        {
            // Strip markdown code fences if present
            string text = resp.Trim();
            if (text.StartsWith("```"))
            {
                int start = text.IndexOf('\n') + 1;
                int end   = text.LastIndexOf("```");
                if (end > start) text = text.Substring(start, end - start).Trim();
            }
            // Find first '{' in case there's leading prose
            int brace = text.IndexOf('{');
            if (brace > 0) text = text.Substring(brace);

            try
            {
                var obj = JObject.Parse(text);
                // Validate required keys
                if (obj["pass_name"] == null && obj["technique"] == null) return null;
                obj["_source"] = "llm";
                return obj;
            }
            catch { return null; }
        }

        private static JObject BuildRuleBasedResult(DrawCallInfo dc)
        {
            string cat = dc.Label.Category;
            string passName = cat switch
            {
                "Shadow"      => "Shadow Map Depth Pass",
                "PostProcess" => "Post-Processing Pass",
                "UI"          => "UI Overlay Pass",
                "Compute"     => "Compute Dispatch",
                "Character"   => "Character Rendering",
                "VFX"         => "Visual Effects / Particles",
                _             => "Scene Geometry Pass"
            };

            return new JObject
            {
                ["pass_name"]          = passName,
                ["technique"]          = dc.Label.Detail.Length > 0 ? dc.Label.Detail : cat,
                ["key_features"]       = new JArray(dc.Label.ReasonTags.Take(3).Cast<object>().ToArray()),
                ["complexity_notes"]   = $"Rule-based estimate for {cat} category (no LLM)",
                ["optimization_hints"] = new JArray(
                    "Profile with GPU hardware counters for precise bottleneck",
                    "Check shader complexity vs. target device tier"),
                ["_source"]            = "rule"
            };
        }

        private static string SanitizeDcId(string dcId)
            => dcId.Replace('.', '_').Replace('/', '_').Replace('\\', '_');
    }
}
