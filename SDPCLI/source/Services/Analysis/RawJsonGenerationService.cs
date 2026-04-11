using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Generates <c>snapshot_{id}_raw.json</c> — the per-DC annotated output consumed by Pass B.
    /// Does NOT call LLM; labeling is done upstream by <see cref="DrawCallLabelService"/>.
    /// Rename target: RawJsonGenerationService.
    /// </summary>
    public class RawJsonGenerationService
    {
        private readonly Config config;
        private readonly ILogger logger;

        public RawJsonGenerationService(Config config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        // Step 3: labeled JSON (replaces CSV)
        // Each DC entry annotates the exact shader, texture, and mesh file paths it uses.
        // Relative paths from snapshot_{captureId}/ to session-level shared assets use "../../shaders/" etc.
        public string GenerateLabeledMetricsJson(
            DrawCallAnalysisReport report,
            string captureOutDir,
            string shaderBaseDir,
            string textureBaseDir,
            string meshBaseDir,
            uint   captureId = 0,
            string sdpName   = "")
        {
            var drawcalls = new List<object>();
            foreach (var dc in report.DrawCallResults)
            {
                var m        = dc.Metrics;
                var colorRTs = dc.RenderTargets.Where(r => r.AttachmentType == "Color").ToList();
                var depthRTs = dc.RenderTargets.Where(r =>
                    r.AttachmentType == "Depth" ||
                    r.AttachmentType == "Stencil" ||
                    r.AttachmentType == "DepthStencil").ToList();
                var firstRT  = dc.RenderTargets.FirstOrDefault();

                // Shader files: enumerate files for this pipeline in the shared shaders folder
                var shaderFiles = Directory.Exists(shaderBaseDir)
                    ? Directory.GetFiles(shaderBaseDir, $"pipeline_{dc.PipelineID}_*")
                        .OrderBy(f => f)
                        .Select(f => "../../shaders/" + Path.GetFileName(f))
                        .ToArray()
                    : Array.Empty<string>();

                // Shader stage metadata from the DC model
                var shaderStages = dc.Shaders.Select(s => new
                {
                    stage       = s.ShaderStageName,
                    module_id   = s.ShaderModuleID,
                    entry_point = s.EntryPoint,
                    file        = shaderFiles.FirstOrDefault(f =>
                        f.IndexOf($"pipeline_{dc.PipelineID}_", StringComparison.OrdinalIgnoreCase) >= 0
                        && f.IndexOf(s.ShaderStageName.ToLowerInvariant().Substring(0, 2),
                            StringComparison.OrdinalIgnoreCase) >= 0),
                }).ToArray();

                // Texture files: reference existing PNGs in shared textures folder
                var textureFiles = dc.TextureIDs
                    .Select(id =>
                    {
                        string fname = $"texture_{id}.png";
                        return File.Exists(Path.Combine(textureBaseDir, fname))
                            ? "../../textures/" + fname
                            : null;
                    })
                    .Where(p => p != null)
                    .ToArray();

                // Mesh file: one OBJ per DC keyed by ApiID in shared session-level meshes folder
                bool hasMesh = dc.ApiName.IndexOf("Dispatch",
                    StringComparison.OrdinalIgnoreCase) < 0
                    && dc.VertexBuffers.Count > 0
                    && File.Exists(Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj"));
                string? meshFile = hasMesh ? $"../../meshes/mesh_{dc.ApiID}.obj" : null;

                // Metrics: dynamic from All (all whitelist counters present in DB for this DC)
                object? metricsNode = null;
                if (m != null && m.All.Count > 0)
                {
                    var metricsObj = new JObject();
                    foreach (var kv in m.All.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase))
                        metricsObj[DrawCallMetrics.NormalizeKey(kv.Key)] = kv.Value;
                    metricsNode = metricsObj;
                }

                drawcalls.Add(new
                {
                    dc_id              = dc.DrawCallNumber,
                    api_id             = dc.ApiID,
                    api_name           = dc.ApiName,
                    pipeline_id        = dc.PipelineID,
                    layout_id          = dc.LayoutID,
                    render_pass_id     = dc.RenderPass,
                    vertex_count       = dc.VertexCount,
                    index_count        = dc.IndexCount,
                    instance_count     = dc.InstanceCount,
                    // ── Shader resources ─────────────────────────────────────
                    shader_stages      = shaderStages,
                    shader_files       = shaderFiles,
                    // ── Texture resources ─────────────────────────────────────
                    texture_ids        = dc.TextureIDs,
                    texture_files      = textureFiles,
                    textures           = dc.Textures.Select(t => new
                    {
                        texture_id = t.TextureID,
                        width      = t.Width,
                        height     = t.Height,
                        depth      = t.Depth,
                        format     = t.FormatName,
                        layers     = t.LayerCount,
                        levels     = t.LevelCount,
                    }).ToArray(),
                    // ── Mesh / geometry resources ─────────────────────────────
                    mesh_file          = meshFile,
                    vertex_buffers     = dc.VertexBuffers.Select(vb => new
                        { binding = vb.Binding, buffer_id = vb.BufferID }).ToArray(),
                    index_buffer       = dc.IndexBuffer == null ? null : (object)new
                    {
                        buffer_id  = dc.IndexBuffer.BufferID,
                        offset     = dc.IndexBuffer.Offset,
                        index_type = dc.IndexBuffer.IndexType,
                    },
                    // ── Render targets ────────────────────────────────────────
                    render_targets     = dc.RenderTargets.Select(r => new
                    {
                        attachment_index = r.AttachmentIndex,
                        attachment_type  = r.AttachmentType,
                        resource_id      = r.AttachmentResourceID,
                        renderpass_id    = r.RenderPassID,
                        framebuffer_id   = r.FramebufferID,
                        width            = r.Width,
                        height           = r.Height,
                        format           = r.FormatName,
                    }).ToArray(),
                    // ── Descriptor bindings summary ───────────────────────────
                    binding_summary = new
                    {
                        typed_buffer_view_count  = dc.BindingSummary.TypedBufferViewCount,
                        small_buffer_count       = dc.BindingSummary.SmallBufferCount,
                        has_per_instance_buffer  = dc.BindingSummary.HasPerInstanceBuffer,
                    },
                    // ── Label ─────────────────────────────────────────────────
                    label = new
                    {
                        category     = dc.Label.Category,
                        subcategory  = dc.Label.Subcategory,
                        detail       = dc.Label.Detail,
                        reason_tags  = dc.Label.ReasonTags,
                        confidence   = Math.Round(dc.Label.Confidence, 3),
                        label_source = dc.Label.LabelSource,
                    },
                    // ── GPU metrics (all whitelist counters, snake_case keys) ─
                    metrics_available  = m != null,
                    metrics            = metricsNode,
                });
            }

            var root = new
            {
                schema_version  = "2.0",
                snapshot_id     = captureId,
                sdp_name        = sdpName,
                generated_at    = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                total_dc_count  = report.DrawCallResults.Count,
                draw_calls      = drawcalls,
            };

            var settings = new JsonSerializerSettings
            {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(root, settings);
            Directory.CreateDirectory(captureOutDir);
            string fileName = captureId > 0
                ? $"snapshot_{captureId}_raw.json"
                : $"DrawCallAnalysis_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            string path = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(path, json, Encoding.UTF8);
            return path;
        }

        // ═══════════════════════════════════════════════════════════
        // Sub-JSON writers — schema 3.0  (join key: api_id)
        // ═══════════════════════════════════════════════════════════

        /// <summary>Writes dc.json — core DC params, render targets, binding summary.</summary>
        public string WriteDcJson(
            DrawCallAnalysisReport report, string captureOutDir,
            uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc => (object)new
            {
                dc_id          = dc.DrawCallNumber,
                api_id         = dc.ApiID,
                api_name       = dc.ApiName,
                pipeline_id    = dc.PipelineID,
                layout_id      = dc.LayoutID,
                render_pass_id = dc.RenderPass,
                vertex_count   = dc.VertexCount,
                index_count    = dc.IndexCount,
                instance_count = dc.InstanceCount,
                render_targets = dc.RenderTargets.Select(r => new
                {
                    attachment_index = r.AttachmentIndex,
                    attachment_type  = r.AttachmentType,
                    resource_id      = r.AttachmentResourceID,
                    renderpass_id    = r.RenderPassID,
                    framebuffer_id   = r.FramebufferID,
                    width            = r.Width,
                    height           = r.Height,
                    format           = r.FormatName,
                }).ToArray(),
                binding_summary = new
                {
                    typed_buffer_view_count = dc.BindingSummary.TypedBufferViewCount,
                    small_buffer_count      = dc.BindingSummary.SmallBufferCount,
                    has_per_instance_buffer = dc.BindingSummary.HasPerInstanceBuffer,
                },
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "dc.json");
        }

        /// <summary>Writes label.json — DC classification results.</summary>
        public string WriteLabelJson(
            DrawCallAnalysisReport report, string captureOutDir,
            uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc => (object)new
            {
                dc_id  = dc.DrawCallNumber,
                api_id = dc.ApiID,
                label  = new
                {
                    category     = dc.Label.Category,
                    subcategory  = dc.Label.Subcategory,
                    detail       = dc.Label.Detail,
                    reason_tags  = dc.Label.ReasonTags,
                    confidence   = Math.Round(dc.Label.Confidence, 3),
                    label_source = dc.Label.LabelSource,
                },
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "label.json");
        }

        /// <summary>Writes metrics.json — GPU performance counters per DC.</summary>
        public string WriteMetricsJson(
            DrawCallAnalysisReport report, string captureOutDir,
            uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc =>
            {
                var m = dc.Metrics;
                object? metricsNode = null;
                if (m != null && m.All.Count > 0)
                {
                    var metricsObj = new JObject();
                    foreach (var kv in m.All.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase))
                        metricsObj[DrawCallMetrics.NormalizeKey(kv.Key)] = kv.Value;
                    metricsNode = metricsObj;
                }
                return (object)new
                {
                    dc_id             = dc.DrawCallNumber,
                    api_id            = dc.ApiID,
                    metrics_available = m != null,
                    metrics           = metricsNode,
                };
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "metrics.json");
        }

        /// <summary>Writes shaders.json — pipeline shader stages and extracted file paths.</summary>
        public string WriteShadersJson(
            DrawCallAnalysisReport report, string captureOutDir,
            string shaderBaseDir, uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc =>
            {
                var shaderFiles = BuildShaderFileList(dc.PipelineID, shaderBaseDir);
                var shaderStages = dc.Shaders.Select(s => new
                {
                    stage       = s.ShaderStageName,
                    module_id   = s.ShaderModuleID,
                    entry_point = s.EntryPoint,
                    file        = shaderFiles.FirstOrDefault(f =>
                        f.IndexOf($"pipeline_{dc.PipelineID}_",
                            StringComparison.OrdinalIgnoreCase) >= 0
                        && f.IndexOf(s.ShaderStageName.ToLowerInvariant().Substring(0, 2),
                            StringComparison.OrdinalIgnoreCase) >= 0),
                }).ToArray();
                return (object)new
                {
                    dc_id         = dc.DrawCallNumber,
                    api_id        = dc.ApiID,
                    pipeline_id   = dc.PipelineID,
                    shader_stages = shaderStages,
                    shader_files  = shaderFiles,
                };
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "shaders.json");
        }

        /// <summary>Writes textures.json — texture IDs, metadata, and extracted PNG paths.</summary>
        public string WriteTexturesJson(
            DrawCallAnalysisReport report, string captureOutDir,
            string textureBaseDir, uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc =>
            {
                var textureFiles = dc.TextureIDs
                    .Select(id =>
                    {
                        string fname = $"texture_{id}.png";
                        return File.Exists(Path.Combine(textureBaseDir, fname))
                            ? "../../textures/" + fname : null;
                    })
                    .Where(p => p != null)
                    .ToArray();
                return (object)new
                {
                    dc_id         = dc.DrawCallNumber,
                    api_id        = dc.ApiID,
                    texture_ids   = dc.TextureIDs,
                    textures      = dc.Textures.Select(t => new
                    {
                        texture_id = t.TextureID,
                        width      = t.Width,
                        height     = t.Height,
                        depth      = t.Depth,
                        format     = t.FormatName,
                        layers     = t.LayerCount,
                        levels     = t.LevelCount,
                    }).ToArray(),
                    texture_files = textureFiles,
                };
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "textures.json");
        }

        /// <summary>Writes buffers.json — vertex/index buffers and mesh OBJ path.</summary>
        public string WriteBuffersJson(
            DrawCallAnalysisReport report, string captureOutDir,
            string meshBaseDir, uint captureId = 0, string sdpName = "")
        {
            var drawCalls = report.DrawCallResults.Select(dc =>
            {
                bool hasMesh = dc.ApiName.IndexOf("Dispatch",
                    StringComparison.OrdinalIgnoreCase) < 0
                    && dc.VertexBuffers.Count > 0
                    && File.Exists(Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj"));
                return (object)new
                {
                    dc_id          = dc.DrawCallNumber,
                    api_id         = dc.ApiID,
                    vertex_buffers = dc.VertexBuffers.Select(vb => new
                        { binding = vb.Binding, buffer_id = vb.BufferID }).ToArray(),
                    index_buffer   = dc.IndexBuffer == null ? null : (object)new
                    {
                        buffer_id  = dc.IndexBuffer.BufferID,
                        offset     = dc.IndexBuffer.Offset,
                        index_type = dc.IndexBuffer.IndexType,
                    },
                    mesh_file = hasMesh ? $"../../meshes/mesh_{dc.ApiID}.obj" : null,
                };
            }).ToList<object>();
            return SaveSnapshotJson(drawCalls, captureOutDir, captureId, sdpName, "buffers.json");
        }

        /// <summary>
        /// Writes/updates snapshot_{id}_index.json — manifest of all product files.
        /// </summary>
        public string WriteIndexJson(
            string captureOutDir, uint captureId, string sdpName,
            Dictionary<string, string> products)
        {
            var root = new
            {
                schema_version = "1.0",
                snapshot_id    = captureId,
                sdp_name       = sdpName,
                generated_at   = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                products,
            };
            string json = JsonConvert.SerializeObject(root,
                new JsonSerializerSettings { Formatting = Formatting.Indented });
            Directory.CreateDirectory(captureOutDir);
            string path = Path.Combine(captureOutDir,
                captureId > 0 ? $"snapshot_{captureId}_index.json" : "snapshot_index.json");
            File.WriteAllText(path, json, Encoding.UTF8);
            return path;
        }

        // ── Private helpers ────────────────────────────────────────────────

        private string SaveSnapshotJson(
            List<object> drawCalls, string captureOutDir,
            uint captureId, string sdpName, string fileName)
        {
            var root = new
            {
                schema_version = "3.0",
                snapshot_id    = captureId,
                sdp_name       = sdpName,
                generated_at   = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                total_dc_count = drawCalls.Count,
                draw_calls     = drawCalls,
            };
            string json = JsonConvert.SerializeObject(root, new JsonSerializerSettings
            {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            });
            Directory.CreateDirectory(captureOutDir);
            string path = Path.Combine(captureOutDir, fileName);
            File.WriteAllText(path, json, Encoding.UTF8);
            return path;
        }

        private static string[] BuildShaderFileList(long pipelineId, string shaderBaseDir)
            => Directory.Exists(shaderBaseDir)
                ? Directory.GetFiles(shaderBaseDir, $"pipeline_{pipelineId}_*")
                    .OrderBy(f => f)
                    .Select(f => "../../shaders/" + Path.GetFileName(f))
                    .ToArray()
                : Array.Empty<string>();

        // Step 3 (legacy): labeled CSV — kept for compatibility; prefer GenerateLabeledMetricsJson
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

        // Compat shims
        public string GenerateCsvReport(DrawCallAnalysisReport r, string outputDir)
            => GenerateLabeledMetricsCsv(r, outputDir);
        public string GenerateJsonReport(DrawCallAnalysisReport r, string captureOutDir,
            string shaderBaseDir, string textureBaseDir, string meshBaseDir)
            => GenerateLabeledMetricsJson(r, captureOutDir, shaderBaseDir, textureBaseDir, meshBaseDir);
        public string GenerateJsonReport(DrawCallAnalysisReport r, string captureOutDir,
            string shaderBaseDir, string textureBaseDir)
            => GenerateLabeledMetricsJson(r, captureOutDir, shaderBaseDir, textureBaseDir, "");
        private static string Q(string s) =>
            s.Contains(',') || s.Contains('"') ? $"\"{s.Replace("\"","\"\"") }\"" : s;
    }
}
