using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Rebuilds a <see cref="DrawCallAnalysisReport"/> from the 6 sub-JSON files
    /// (dc.json, label.json, metrics.json, shaders.json, textures.json, buffers.json)
    /// written by <see cref="RawJsonGenerationService"/> in schema 3.0.
    /// Used when the pipeline is invoked with <c>-t analysis</c> or <c>-t dashboard</c>
    /// without running the full pipeline first (i.e. in-memory report is unavailable).
    /// Join key: <c>api_id</c>.
    /// </summary>
    public class SubJsonLoadService
    {
        private readonly ILogger logger;

        public SubJsonLoadService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Load and merge all available sub-JSONs from <paramref name="captureOutDir"/>.
        /// Any missing file is silently skipped (partial loads are supported).
        /// Returns null if dc.json is missing (dc is the base join key).
        /// </summary>
        public DrawCallAnalysisReport? LoadFromSubJsons(string captureOutDir)
        {
            string dcPath       = Path.Combine(captureOutDir, "dc.json");
            string labelPath    = Path.Combine(captureOutDir, "label.json");
            string metricsPath  = Path.Combine(captureOutDir, "metrics.json");
            string shadersPath  = Path.Combine(captureOutDir, "shaders.json");
            string texturesPath = Path.Combine(captureOutDir, "textures.json");
            string buffersPath  = Path.Combine(captureOutDir, "buffers.json");

            if (!File.Exists(dcPath))
            {
                logger.Warning($"SubJsonLoadService: dc.json not found at {dcPath} — cannot rebuild report");
                return null;
            }

            // ── Load dc.json (base) ───────────────────────────────────────────
            var dcArr = LoadDrawCallArray(dcPath);
            if (dcArr == null) return null;

            // Build index: api_id -> row for O(1) merges
            var dcByApiId = new Dictionary<uint, JObject>();
            foreach (var dc in dcArr)
            {
                uint aid = dc["api_id"]?.ToObject<uint>() ?? 0;
                dcByApiId[aid] = dc;
            }

            // ── Load optional files ────────────────────────────────────────────
            var labelByApiId   = LoadIndex(labelPath);
            var metricsByApiId = LoadIndex(metricsPath);
            var shadersByApiId = LoadIndex(shadersPath);
            var texsByApiId    = LoadIndex(texturesPath);
            var bufsByApiId    = LoadIndex(buffersPath);

            // ── Reconstruct DrawCallInfo per DC ───────────────────────────────
            var results = new List<DrawCallInfo>(dcArr.Count);
            foreach (var dc in dcArr)
            {
                string dcNum  = dc["dc_id"]?.ToString() ?? "";
                uint   apiId  = dc["api_id"]?.ToObject<uint>() ?? 0;
                var    info   = new DrawCallInfo
                {
                    DrawCallNumber = dcNum,
                    ApiID          = apiId,
                    ApiName        = dc["api_name"]?.ToString() ?? "",
                    PipelineID     = dc["pipeline_id"]?.ToObject<uint>() ?? 0,
                    LayoutID       = dc["layout_id"]?.ToObject<uint>() ?? 0,
                    RenderPass     = dc["render_pass_id"]?.ToObject<uint>() ?? 0,
                    VertexCount    = dc["vertex_count"]?.ToObject<uint>() ?? 0,
                    IndexCount     = dc["index_count"]?.ToObject<uint>() ?? 0,
                    InstanceCount  = dc["instance_count"]?.ToObject<uint>() ?? 0,
                };

                // Render targets
                var rts = dc["render_targets"] as JArray;
                if (rts != null)
                {
                    foreach (var rt in rts)
                    {
                        info.RenderTargets.Add(new RenderTargetInfo
                        {
                            AttachmentIndex      = rt["attachment_index"]?.ToObject<uint>() ?? 0,
                            AttachmentType       = rt["attachment_type"]?.ToString() ?? "",
                            AttachmentResourceID = rt["resource_id"]?.ToObject<uint>() ?? 0,
                            RenderPassID         = rt["renderpass_id"]?.ToObject<uint>() ?? 0,
                            FramebufferID        = rt["framebuffer_id"]?.ToObject<uint>() ?? 0,
                            Width                = rt["width"]?.ToObject<uint>() ?? 0,
                            Height               = rt["height"]?.ToObject<uint>() ?? 0,
                            FormatName           = rt["format"]?.ToString() ?? "",
                        });
                    }
                }

                // Binding summary
                var bs = dc["binding_summary"];
                if (bs != null)
                {
                    info.BindingSummary = new DescriptorBindingSummary
                    {
                        TypedBufferViewCount = bs["typed_buffer_view_count"]?.ToObject<int>() ?? 0,
                        SmallBufferCount     = bs["small_buffer_count"]?.ToObject<int>() ?? 0,
                        HasPerInstanceBuffer = bs["has_per_instance_buffer"]?.ToObject<bool>() ?? false,
                    };
                }

                // ── label.json ────────────────────────────────────────────────
                if (labelByApiId.TryGetValue(apiId, out var lbl))
                {
                    var l = lbl["label"];
                    if (l != null)
                    {
                        info.Label = new DrawCallLabel
                        {
                            Category    = l["category"]?.ToString() ?? "Scene",
                            Subcategory = l["subcategory"]?.ToString() ?? "",
                            Detail      = l["detail"]?.ToString() ?? "",
                            ReasonTags  = l["reason_tags"]?.ToObject<string[]>() ?? Array.Empty<string>(),
                            Confidence  = l["confidence"]?.ToObject<float>() ?? 0.7f,
                            LabelSource = l["label_source"]?.ToString() ?? "cache",
                        };
                    }
                }

                // ── shaders.json ──────────────────────────────────────────────
                if (shadersByApiId.TryGetValue(apiId, out var sh))
                {
                    var stages = sh["shader_stages"] as JArray;
                    if (stages != null)
                    {
                        foreach (var s in stages)
                        {
                            info.Shaders.Add(new ShaderInfo
                            {
                                ShaderStageName = s["stage"]?.ToString() ?? "",
                                ShaderModuleID  = s["module_id"]?.ToObject<ulong>() ?? 0,
                                EntryPoint      = s["entry_point"]?.ToString() ?? "main",
                            });
                        }
                    }
                }

                // ── textures.json ─────────────────────────────────────────────
                if (texsByApiId.TryGetValue(apiId, out var tex))
                {
                    info.TextureIDs = tex["texture_ids"]?.ToObject<uint[]>() ?? Array.Empty<uint>();

                    var texArr = tex["textures"] as JArray;
                    if (texArr != null)
                    {
                        foreach (var t in texArr)
                        {
                            info.Textures.Add(new TextureInfo
                            {
                                TextureID  = t["texture_id"]?.ToObject<uint>() ?? 0,
                                Width      = t["width"]?.ToObject<uint>() ?? 0,
                                Height     = t["height"]?.ToObject<uint>() ?? 0,
                                Depth      = t["depth"]?.ToObject<uint>() ?? 0,
                                FormatName = t["format"]?.ToString() ?? "",
                                LayerCount = t["layers"]?.ToObject<uint>() ?? 0,
                                LevelCount = t["levels"]?.ToObject<uint>() ?? 0,
                            });
                        }
                    }
                }

                // ── buffers.json ──────────────────────────────────────────────
                if (bufsByApiId.TryGetValue(apiId, out var buf))
                {
                    var vbs = buf["vertex_buffers"] as JArray;
                    if (vbs != null)
                    {
                        foreach (var vb in vbs)
                        {
                            info.VertexBuffers.Add(new VertexBufferBinding
                            {
                                Binding  = vb["binding"]?.ToObject<uint>() ?? 0,
                                BufferID = vb["buffer_id"]?.ToObject<uint>() ?? 0,
                            });
                        }
                    }

                    var ib = buf["index_buffer"];
                    if (ib != null && ib.Type != JTokenType.Null)
                    {
                        info.IndexBuffer = new IndexBufferBinding
                        {
                            BufferID  = ib["buffer_id"]?.ToObject<uint>() ?? 0,
                            Offset    = ib["offset"]?.ToObject<uint>() ?? 0,
                            IndexType = ib["index_type"]?.ToString() ?? "",
                        };
                    }
                }

                // ── metrics.json ──────────────────────────────────────────────
                if (metricsByApiId.TryGetValue(apiId, out var met))
                {
                    bool avail = met["metrics_available"]?.ToObject<bool>() ?? false;
                    var  m     = met["metrics"] as JObject;
                    if (avail && m != null)
                    {
                        var dm = new DrawCallMetrics { DrawCallNumber = dcNum, ApiName = info.ApiName };
                        // Reverse lookup: snake_case key -> original counter name
                        // We store snake_case keys in metrics.json; reconstruct using CounterToKey inverse.
                        var snakeToOriginal = BuildSnakeToOriginalMap();
                        foreach (var prop in m.Properties())
                        {
                            string original = snakeToOriginal.TryGetValue(prop.Name, out var orig)
                                ? orig : prop.Name;
                            dm.All[original] = prop.Value.ToObject<double>();
                        }
                        info.Metrics = dm;
                    }
                }

                results.Add(info);
            }

            // ── Compute summary statistics ─────────────────────────────────────
            var report = new DrawCallAnalysisReport
            {
                DrawCallResults  = results,
                TotalDrawCalls   = results.Count,
                AnalyzedDrawCalls = results.Count,
            };
            report.Statistics = new CaptureStatistics
            {
                TotalDrawCalls   = results.Count,
                TotalPipelines   = results.Select(r => r.PipelineID).Distinct().Count(),
                TotalTextures    = results.SelectMany(r => r.TextureIDs).Distinct().Count(),
                TotalShaders     = results.SelectMany(r => r.Shaders.Select(s => s.ShaderModuleID)).Distinct().Count(),
            };

            logger.Info($"SubJsonLoadService: rebuilt {results.Count} DCs from {captureOutDir}");
            return report;
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private List<JObject>? LoadDrawCallArray(string path)
        {
            try
            {
                var root = JObject.Parse(File.ReadAllText(path));
                var arr  = root["draw_calls"] as JArray;
                if (arr == null)
                {
                    logger.Warning($"SubJsonLoadService: no draw_calls array in {path}");
                    return null;
                }
                return arr.Cast<JObject>().ToList();
            }
            catch (Exception ex)
            {
                logger.Warning($"SubJsonLoadService: failed to parse {path}: {ex.Message}");
                return null;
            }
        }

        private Dictionary<uint, JObject> LoadIndex(string path)
        {
            var dict = new Dictionary<uint, JObject>();
            if (!File.Exists(path)) return dict;
            try
            {
                var root = JObject.Parse(File.ReadAllText(path));
                var arr  = root["draw_calls"] as JArray;
                if (arr == null) return dict;
                foreach (var item in arr.Cast<JObject>())
                {
                    uint aid = item["api_id"]?.ToObject<uint>() ?? 0;
                    dict[aid] = item;
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"SubJsonLoadService: failed to parse {path}: {ex.Message}");
            }
            return dict;
        }

        private static Dictionary<string, string>? _snakeToOriginalCache;
        private static Dictionary<string, string> BuildSnakeToOriginalMap()
        {
            if (_snakeToOriginalCache != null) return _snakeToOriginalCache;
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in DrawCallMetrics.CounterToKey)
                d[kv.Value] = kv.Key;
            _snakeToOriginalCache = d;
            return d;
        }
    }
}
