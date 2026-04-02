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

    /// <summary>Rule-based category label for a DrawCall.</summary>
    public class DrawCallLabel
    {
        public string Category { get; set; } = "场景";  // 场景/角色/投影/后处理/特效/UI
        public string Detail   { get; set; } = "";      // e.g. "平面阴影高斯模糊"
    }

    /// <summary>Per-DrawCall performance metrics loaded from profiler CSV.</summary>
    public class DrawCallMetrics
    {
        public string DrawCallNumber       { get; set; } = "";
        public string ApiName              { get; set; } = "";
        public long   Clocks               { get; set; }
        public long   ReadTotalBytes       { get; set; }
        public long   WriteTotalBytes      { get; set; }
        public long   FragmentsShaded      { get; set; }
        public long   VerticesShaded       { get; set; }
        public double ShadersBusyPct       { get; set; }
        public double TexL1MissPct         { get; set; }
        public double TexL2MissPct         { get; set; }
        public double TexFetchStallPct     { get; set; }
        public long   FragmentInstructions { get; set; }
        public long   VertexInstructions   { get; set; }
        public long   TexMemReadBytes      { get; set; }
        public long   VertexMemReadBytes   { get; set; }
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
