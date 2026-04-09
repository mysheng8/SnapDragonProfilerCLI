using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapdragonProfilerCLI.Models
{
    /// <summary>
    /// DrawCall analysis result — pipeline, textures, shaders, render targets and buffers for a single draw call.
    /// </summary>
    public class DrawCallInfo
    {
        // ── Core identifiers ──────────────────────────────────────────────────
        public string DrawCallNumber { get; set; } = "";   // human-readable (e.g. "1.1.5" or "106974")
        public uint   ApiID          { get; set; }         // VkApiID from DrawCallParameters
        public string ApiName        { get; set; } = "";   // e.g. "vkCmdDraw", "vkCmdDispatch"

        // ── Pipeline / layout ─────────────────────────────────────────────────
        public uint PipelineID { get; set; }
        public uint LayoutID   { get; set; }
        public uint RenderPass { get; set; }

        // ── Label (rule-based category) ───────────────────────────────────────
        public DrawCallLabel Label { get; set; } = new DrawCallLabel();

        // ── Metrics (from profiler CSV, joined by DrawCallNumber) ─────────────
        public DrawCallMetrics? Metrics { get; set; }

        // ── Draw parameters ───────────────────────────────────────────────────
        public uint VertexCount   { get; set; }
        public uint IndexCount    { get; set; }
        public uint InstanceCount { get; set; }

        // ── Resources ─────────────────────────────────────────────────────────
        public uint[]               TextureIDs    { get; set; } = Array.Empty<uint>();
        public List<TextureInfo>    Textures      { get; set; } = new List<TextureInfo>();
        public List<ShaderInfo>     Shaders       { get; set; } = new List<ShaderInfo>();
        public List<RenderTargetInfo>   RenderTargets { get; set; } = new List<RenderTargetInfo>();
        public List<VertexBufferBinding> VertexBuffers { get; set; } = new List<VertexBufferBinding>();
        public IndexBufferBinding?  IndexBuffer   { get; set; }
        public DescriptorBindingSummary BindingSummary { get; set; } = new DescriptorBindingSummary();

        public void Print()
        {
            Console.WriteLine($"\n=== DrawCall #{DrawCallNumber}  ApiID={ApiID}  {ApiName} ===");
            Console.WriteLine($"  Pipeline={PipelineID}  Layout={LayoutID}  RenderPass={RenderPass}");
            if (VertexCount > 0 || InstanceCount > 0)
                Console.WriteLine($"  Vertices={VertexCount}  Indices={IndexCount}  Instances={InstanceCount}");

            Console.WriteLine($"\n  Shaders ({Shaders.Count}):");
            foreach (var s in Shaders)
                Console.WriteLine($"    - {s.ShaderStageName} (Module: {s.ShaderModuleID}, Entry: {s.EntryPoint})");

            if (RenderTargets.Count > 0)
            {
                Console.WriteLine($"\n  RenderTargets ({RenderTargets.Count}):");
                foreach (var rt in RenderTargets)
                    Console.WriteLine($"    [{rt.AttachmentIndex}] {rt.AttachmentType,7}  Resource={rt.AttachmentResourceID}  RP={rt.RenderPassID}  FB={rt.FramebufferID}");
            }

            if (VertexBuffers.Count > 0)
            {
                Console.WriteLine($"\n  VertexBuffers ({VertexBuffers.Count}):");
                foreach (var vb in VertexBuffers)
                    Console.WriteLine($"    binding={vb.Binding}  bufferID={vb.BufferID}");
            }

            if (IndexBuffer != null)
                Console.WriteLine($"\n  IndexBuffer: bufferID={IndexBuffer.BufferID}  offset={IndexBuffer.Offset}  type={IndexBuffer.IndexType}");

            Console.WriteLine($"\n  Textures ({TextureIDs.Length}):");
            if (Textures.Count > 0)
            {
                Console.WriteLine($"    {"ID",-10} {"Size",-20} {"Format",-20}");
                Console.WriteLine("    " + new string('-', 48));
                foreach (var t in Textures.Take(20))
                    Console.WriteLine($"    {t.TextureID,-10} {t.Width}x{t.Height}x{t.Depth,-14} {t.FormatName,-20}");
                if (Textures.Count > 20)
                    Console.WriteLine($"    ... plus {Textures.Count - 20} more");
            }
        }
    }

    /// <summary>Render target attachment for one draw call (from DrawCallRenderTargets table).</summary>
    public class RenderTargetInfo
    {
        public uint   RenderPassID         { get; set; }
        public uint   FramebufferID        { get; set; }
        public uint   AttachmentIndex      { get; set; }
        public uint   AttachmentResourceID { get; set; }
        public string AttachmentType       { get; set; } = "";  // "Color" | "Depth" | "Stencil"
        // Resolved from VulkanSnapshotImageViews + VulkanSnapshotTextures
        public uint   Width      { get; set; }
        public uint   Height     { get; set; }
        public string FormatName { get; set; } = "";  // e.g. "VK_FORMAT_R8G8B8A8_UNORM"
    }

    /// <summary>Vertex buffer binding for one draw call (from DrawCallVertexBuffers table).</summary>
    public class VertexBufferBinding
    {
        public uint Binding  { get; set; }
        public uint BufferID { get; set; }
    }

    /// <summary>Summary of descriptor set bindings from DrawCallBindings table.</summary>
    public class DescriptorBindingSummary
    {
        /// <summary>Number of typed-buffer-view slots bound (TexBufferView != 0 = Buffer&lt;T&gt;, e.g. SH probe buffer).</summary>
        public int TypedBufferViewCount { get; set; }
        /// <summary>Number of plain buffer bindings with Range &lt;= 256 bytes (likely per-object or per-instance constants).</summary>
        public int SmallBufferCount { get; set; }
        /// <summary>True when a buffer binding with exactly 32-byte range exists — per-instance stride used by GPU-skinned shaders.</summary>
        public bool HasPerInstanceBuffer { get; set; }
    }

    /// <summary>Index buffer binding for one draw call (from DrawCallIndexBuffers table).</summary>
    public class IndexBufferBinding
    {
        public uint   BufferID  { get; set; }
        public uint   Offset    { get; set; }
        public string IndexType { get; set; } = "";  // "UINT16" | "UINT32"
    }

    /// <summary>Vulkan texture metadata.</summary>
    public class TextureInfo
    {
        public uint TextureID { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }
        public uint Format { get; set; }
        public uint LayerCount { get; set; }
        public uint LevelCount { get; set; }
        public string FormatName { get; set; } = "";
    }

    /// <summary>Vulkan shader stage reference.</summary>
    public class ShaderInfo
    {
        public uint ShaderStage { get; set; }
        public ulong ShaderModuleID { get; set; }
        public string EntryPoint { get; set; } = "main";
        public string ShaderStageName { get; set; } = "";
    }

    /// <summary>Category label for a DrawCall — LLM or rule-based.</summary>
    public class DrawCallLabel
    {
        /// <summary>One of the configured categories, e.g. Scene / Character / PostProcess …</summary>
        public string   Category    { get; set; } = "Scene";
        /// <summary>Second-level classification, e.g. Opaque / Terrain / GaussianBlur.</summary>
        public string   Subcategory { get; set; } = "";
        /// <summary>Free-text description (≤120 chars), from LLM or rule hint.</summary>
        public string   Detail      { get; set; } = "";
        /// <summary>Structured tags chosen from the attribution_rules.json label_reason_tags list.</summary>
        public string[] ReasonTags  { get; set; } = Array.Empty<string>();
        /// <summary>LLM self-reported confidence [0,1]; rule path emits 0.70.</summary>
        public float    Confidence  { get; set; } = 0.70f;
        /// <summary>"llm" | "rule" | "cache"</summary>
        public string   LabelSource { get; set; } = "rule";
    }

    /// <summary>
    /// Per-DrawCall GPU performance metrics.
    /// All counter values are stored in <see cref="All"/>, keyed by the original Adreno counter
    /// name (same strings as MetricsWhitelist in config.ini).
    /// Use <see cref="G"/> for safe access, or <see cref="CounterToKey"/> to convert to snake_case
    /// JSON keys consumed by StatusJsonService / AttributionRuleEngine.
    /// </summary>
    public class DrawCallMetrics
    {
        // ── Identity ─────────────────────────────────────────────────────────
        public string DrawCallNumber { get; set; } = "";
        public string ApiName        { get; set; } = "";

        /// <summary>
        /// Raw counter values from DB DrawCallMetrics table.
        /// Keys = MetricName strings (e.g. "Clocks", "% Shaders Busy").
        /// Populated by MetricsQueryService; filtered by MetricsWhitelist at query time.
        /// </summary>
        public Dictionary<string, double> All { get; set; } =
            new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Safe accessor — returns 0.0 when key is absent.</summary>
        public double G(string counterName) =>
            All.TryGetValue(counterName, out var v) ? v : 0.0;

        // ── Convenience typed accessors (for sort/aggregate lambda sites) ────
        // These read from All, so they automatically reflect whatever counters are present.
        public long   Clocks               => (long)G("Clocks");
        public long   ReadTotalBytes       => (long)G("Read Total (Bytes)");
        public long   WriteTotalBytes      => (long)G("Write Total (Bytes)");
        public long   FragmentsShaded      => (long)G("Fragments Shaded");
        public long   VerticesShaded       => (long)G("Vertices Shaded");
        public double ShadersBusyPct       => G("% Shaders Busy");
        public double TexL1MissPct         => G("% Texture L1 Miss");
        public double TexL2MissPct         => G("% Texture L2 Miss");
        public double TexFetchStallPct     => G("% Texture Fetch Stall");
        public long   FragmentInstructions => (long)G("Fragment Instructions");
        public long   VertexInstructions   => (long)G("Vertex Instructions");
        public long   TexMemReadBytes      => (long)G("Texture Memory Read BW (Bytes)");
        public long   VertexMemReadBytes   => (long)G("Vertex Memory Read (Bytes)");

        // ── Counter name → snake_case JSON key mapping ───────────────────────
        // Used by StatusJsonService (percentile blocks) and AttributionRuleEngine (GetMetricValues).
        // Keys = original Adreno counter names; Values = snake_case keys in JSON / attribution_rules.json.
        public static readonly IReadOnlyDictionary<string, string> CounterToKey =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Misc
            ["Clocks"]                               = "clocks",
            ["Preemptions"]                          = "preemptions",
            ["Avg Preemption Delay"]                 = "avg_preemption_delay",
            // Memory Bandwidth
            ["Read Total (Bytes)"]                   = "read_total_bytes",
            ["Write Total (Bytes)"]                  = "write_total_bytes",
            ["Texture Memory Read BW (Bytes)"]       = "tex_mem_read_bytes",
            ["Vertex Memory Read (Bytes)"]           = "vertex_mem_read_bytes",
            ["SP Memory Read (Bytes)"]               = "sp_mem_read_bytes",
            ["Avg Bytes / Fragment"]                 = "avg_bytes_per_fragment",
            ["Avg Bytes / Vertex"]                   = "avg_bytes_per_vertex",
            // Geometry
            ["Fragments Shaded"]                     = "fragments_shaded",
            ["Vertices Shaded"]                      = "vertices_shaded",
            ["Reused Vertices"]                      = "reused_vertices",
            ["Pre-clipped Polygons"]                 = "pre_clipped_polygons",
            ["LRZ Pixels Killed"]                    = "lrz_pixels_killed",
            ["Average Polygon Area"]                 = "avg_polygon_area",
            ["Average Vertices / Polygon"]           = "avg_vertices_per_polygon",
            ["% Prims Clipped"]                      = "prims_clipped_pct",
            ["% Prims Trivially Rejected"]           = "prims_trivially_rejected_pct",
            // Texture
            ["% Texture Fetch Stall"]                = "tex_fetch_stall_pct",
            ["% Texture L1 Miss"]                    = "tex_l1_miss_pct",
            ["% Texture L2 Miss"]                    = "tex_l2_miss_pct",
            ["% Texture Pipes Busy"]                 = "tex_pipes_busy_pct",
            ["% Linear Filtered"]                    = "linear_filtered_pct",
            ["% Nearest Filtered"]                   = "nearest_filtered_pct",
            ["% Anisotropic Filtered"]               = "anisotropic_filtered_pct",
            ["% Non-Base Level Textures"]            = "non_base_level_tex_pct",
            ["L1 Texture Cache Miss Per Pixel"]      = "l1_tex_cache_miss_per_pixel",
            ["Textures / Fragment"]                  = "textures_per_fragment",
            ["Textures / Vertex"]                    = "textures_per_vertex",
            // Shader / ALU
            ["% Shaders Busy"]                       = "shaders_busy_pct",
            ["% Shaders Stalled"]                    = "shaders_stalled_pct",
            ["% Time ALUs Working"]                  = "time_alus_working_pct",
            ["% Time EFUs Working"]                  = "time_efus_working_pct",
            ["% Time Shading Vertices"]              = "time_shading_vertices_pct",
            ["% Time Shading Fragments"]             = "time_shading_fragments_pct",
            ["% Time Compute"]                       = "time_compute_pct",
            ["% Shader ALU Capacity Utilized"]       = "shader_alu_capacity_pct",
            ["% Wave Context Occupancy"]             = "wave_context_occupancy_pct",
            ["% Instruction Cache Miss"]             = "instruction_cache_miss_pct",
            ["Fragment Instructions"]                = "fragment_instructions",
            ["Fragment ALU Instructions (Full)"]     = "fragment_alu_instr_full",
            ["Fragment ALU Instructions (Half)"]     = "fragment_alu_instr_half",
            ["Fragment EFU Instructions"]            = "fragment_efu_instructions",
            ["Vertex Instructions"]                  = "vertex_instructions",
            ["ALU / Fragment"]                       = "alu_per_fragment",
            ["ALU / Vertex"]                         = "alu_per_vertex",
            ["EFU / Fragment"]                       = "efu_per_fragment",
            ["EFU / Vertex"]                         = "efu_per_vertex",
            // Vertex Fetch / Stall
            ["% Vertex Fetch Stall"]                 = "vertex_fetch_stall_pct",
            ["% Stalled on System Memory"]           = "stalled_on_system_mem_pct",
        };

        /// <summary>Convert a counter name to its snake_case JSON key; fallback to simple lowercase.</summary>
        public static string NormalizeKey(string counterName) =>
            CounterToKey.TryGetValue(counterName, out var k) ? k
            : counterName.ToLowerInvariant()
                .Replace("% ", "pct_").Replace("%", "pct")
                .Replace(" / ", "_per_").Replace(" ", "_")
                .Replace("(", "").Replace(")", "").Trim('_');
    }

    /// <summary>Aggregated analysis result for an entire capture.</summary>
    public class DrawCallAnalysisReport
    {
        public List<DrawCallInfo> DrawCallResults { get; set; } = new List<DrawCallInfo>();
        public CaptureStatistics Statistics { get; set; } = new CaptureStatistics();
        public int TotalDrawCalls { get; set; }
        public int AnalyzedDrawCalls { get; set; }
    }

    /// <summary>High-level statistics derived from a DrawCallAnalysisReport.</summary>
    public class CaptureStatistics
    {
        public int TotalDrawCalls { get; set; }
        public int TotalPipelines { get; set; }
        public int TotalTextures { get; set; }
        public int TotalShaders { get; set; }
        public double AvgTexturesPerDrawCall { get; set; }
    }
}
