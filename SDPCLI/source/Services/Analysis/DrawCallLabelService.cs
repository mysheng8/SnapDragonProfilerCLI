using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Labels a DrawCallInfo with a Category + Detail string.
    ///
    /// When an LlmApiWrapper is provided and enabled, it reads the extracted HLSL/GLSL
    /// shader source for the draw call's pipeline, builds a prompt asking the LLM to
    /// classify the pass, and parses the JSON reply.
    ///
    /// Falls back to keyword-based rule matching on shader entry point names if:
    ///   - LLM is not configured
    ///   - LLM call fails or returns an unparseable response
    ///   - Shader files have not been extracted yet
    /// </summary>
    public class DrawCallLabelService
    {
        private readonly List<string>  _allowedCategories;
        private readonly LlmApiWrapper? _llm;
        private readonly ILogger       _logger;
        private readonly int           _maxShaderChars;

        /// <summary>The configured category list, readable by external callers (e.g. pipeline logging).</summary>
        public IReadOnlyList<string> AllowedCategories => _allowedCategories;

        // ── Per-pipeline LLM cache (many DCs share the same pipeline) ─────────
        private readonly Dictionary<uint, DrawCallLabel> _llmCache = new Dictionary<uint, DrawCallLabel>();

        // ── Fallback keyword rules ────────────────────────────────────────────
        private static readonly (string[] keywords, string category, string detailHint)[] Rules =
        {
            (new[] { "shadow", "planar", "shadowmap", "shadowdepth", "shadowpass", "shadowcaster" },
             "Shadow", "Shadow pass"),
            (new[] { "ui", "hud", "canvas", "glyph", "font", "widget", "overlay", "icon", "button", "menu" },
             "UI", "UI rendering"),
            (new[] { "particle", "vfx", "emitter", "ribbon", "trail", "spark", "smoke", "fire", "explosion", "distort" },
             "VFX", "Particle / VFX"),
            (new[] { "character", "skin", "hair", "cloth", "body", "player", "hero", "humanoid", "avatar", "face", "eye" },
             "Character", "Character rendering"),
            (new[] { "terrain", "landscape", "heightfield", "heightmap" },
             "Terrain", "Terrain rendering"),
            (new[] { "blur", "bloom", "tonemap", "tonemapping", "ssao", "ao", "dof", "composite",
                     "resolve", "postprocess", "taa", "fxaa", "msaa", "temporal", "upscale", "blit",
                     "lut", "vignette", "chromatic", "grain", "sharpen", "motion", "velocity",
                     "denoise", "irradiance", "specular", "prefilter", "brdf", "ssr", "reflection" },
             "PostProcess", "Post-process pass"),
            (new[] { "sky", "skybox", "water", "ocean", "grass", "foliage",
                     "ground", "scene", "mesh", "world", "indoor", "outdoor", "building", "road" },
             "Scene", "Scene rendering"),
        };

        /// <param name="allowedCategories">Category list from config (AnalysisCategories).</param>
        /// <param name="llm">Optional generic LLM service. When non-null and enabled, LLM is used first.</param>
        /// <param name="logger">Logger for debug/info output.</param>
        /// <param name="maxShaderChars">Max characters from each shader file sent to the LLM (default 4000).</param>
        public DrawCallLabelService(
            IEnumerable<string> allowedCategories,
            LlmApiWrapper?      llm    = null,
            ILogger?            logger = null,
            int                 maxShaderChars = 4000)
        {
            _allowedCategories = new List<string>(allowedCategories);
            if (_allowedCategories.Count == 0)
                _allowedCategories = new List<string> { "Scene", "Terrain", "Character", "Shadow", "PostProcess", "VFX", "UI" };
            _llm           = llm;
            _logger        = logger ?? new NullLogger();
            _maxShaderChars = maxShaderChars;
        }

        /// <summary>
        /// Label a single draw call.
        /// If shaderBaseDir is provided and LLM is enabled, shader source is read from
        /// shaderBaseDir/dc_{DrawCallNumber}/ and sent to the LLM.
        /// Falls back to rule-based labeling on any failure.
        /// </summary>
        public DrawCallLabel Label(DrawCallInfo dc, string? shaderBaseDir = null)
        {
            // Try LLM path
            if (_llm?.IsEnabled == true && shaderBaseDir != null && dc.PipelineID > 0)
            {
                // Cache hit by pipeline (many DCs share the same pipeline/shaders)
                if (_llmCache.TryGetValue(dc.PipelineID, out var cached))
                {
                    _logger.Debug("    [LLM] cache hit pipeline " + dc.PipelineID + " -> " + cached.Category);
                    return cached;
                }

                string shaderCode = LoadShaderCode(shaderBaseDir, dc.DrawCallNumber);

                // Empty shader — skip LLM, use rule-based fallback
                if (shaderCode.Contains("(empty shader"))
                {
                    _logger.Info("    [LLM] DC " + dc.DrawCallNumber + " skipped — empty shader, using rule-based label");
                    var ruleLabel = LabelByRules(dc);
                    _llmCache[dc.PipelineID] = ruleLabel;
                    return ruleLabel;
                }

                string prompt = BuildPrompt(dc, shaderCode);

                _logger.Debug("    [LLM] calling for DC " + dc.DrawCallNumber + " (pipeline " + dc.PipelineID + ")...");
                string? response = _llm.Chat(prompt);

                if (response == null)
                {
                    string errDetail = "[LLM error: " + (_llm.LastError ?? "no response") + "]";
                    _logger.Info("    [LLM] DC " + dc.DrawCallNumber + " error: " + (_llm.LastError ?? "no response"));
                    var errLabel = new DrawCallLabel { Category = _allowedCategories[0], Detail = errDetail };
                    _llmCache[dc.PipelineID] = errLabel;
                    return errLabel;
                }

                _logger.Debug("    [LLM] \u2192 " + response.Replace("\n", " ").Substring(0, Math.Min(200, response.Length)));
                DrawCallLabel? parsed = ParseLlmResponse(response);
                if (parsed != null)
                {
                    _llmCache[dc.PipelineID] = parsed;
                    return parsed;
                }

                // Parse failed - surface the raw response as the detail
                string raw = response.Replace("\n", " ").Replace(",", " ");
                if (raw.Length > 120) raw = raw.Substring(0, 120) + "...";
                string parseErrDetail = "[LLM parse error: " + raw + "]";
                _logger.Info("    [LLM] DC " + dc.DrawCallNumber + " parse failed. Response: " + raw);
                var parseErrLabel = new DrawCallLabel { Category = _allowedCategories[0], Detail = parseErrDetail };
                _llmCache[dc.PipelineID] = parseErrLabel;
                return parseErrLabel;
            }

            // No LLM configured — minimal fallback
            return LabelByRules(dc);
        }

        // ── Prompt construction ───────────────────────────────────────────────

        private string BuildPrompt(DrawCallInfo dc, string shaderCode)
        {
            string catList = string.Join("/", _allowedCategories);
            string stages  = dc.Shaders.Count > 0
                ? string.Join(", ", dc.Shaders.Select(s => s.ShaderStageName + ":" + s.EntryPoint))
                : "none";

            var sb = new StringBuilder();
            // ── Draw call context ──────────────────────────────────────────────
            sb.AppendLine("Classify this Vulkan draw call. Reply with JSON only.");
            sb.AppendLine();
            sb.Append("API:").AppendLine(dc.ApiName);
            // Geometry scale: verts/instances help distinguish UI (few quads) vs VFX (tiny particle quads) vs Scene/Character (heavy mesh)
            uint vertsPerInstance = dc.InstanceCount > 0 ? dc.VertexCount / dc.InstanceCount : dc.VertexCount;
            sb.Append("Verts:").Append(dc.VertexCount)
              .Append("  Indices:").Append(dc.IndexCount)
              .Append("  Instances:").Append(dc.InstanceCount)
              .Append("  VertsPerInst:").Append(vertsPerInstance)
              .Append("  Textures:").AppendLine(dc.TextureIDs.Length.ToString());
            sb.Append("Shaders: ").AppendLine(stages);
            // ── Render targets ─────────────────────────────────────────────────
            if (dc.RenderTargets.Count > 0)
            {
                sb.AppendLine("Render targets:");
                foreach (var rt in dc.RenderTargets)
                {
                    string sz  = (rt.Width > 0 && rt.Height > 0) ? $" {rt.Width}x{rt.Height}" : "";
                    string fmt = !string.IsNullOrEmpty(rt.FormatName) ? $" {rt.FormatName}" : "";
                    sb.AppendLine($"  [{rt.AttachmentIndex}]{rt.AttachmentType}{sz}{fmt}");
                }
            }
            // ── Descriptor bindings summary ────────────────────────────────────
            {
                var bs = dc.BindingSummary;
                string vbStr  = dc.VertexBuffers.Count > 0 ? dc.VertexBuffers.Count.ToString() : "0 (none)";
                string ibStr  = dc.IndexBuffer != null ? dc.IndexBuffer.IndexType : "none";
                sb.AppendLine("Bindings:");
                sb.Append("  VertexBuffers:").AppendLine(vbStr);
                sb.Append("  IndexBuffer:").AppendLine(ibStr);
                sb.Append("  TypedBufferViews:").Append(bs.TypedBufferViewCount)
                  .AppendLine("  (Buffer<T> typed slots — e.g. SH probe buffer or skinned-vertex buffer)");
                sb.Append("  SmallBuffers(<=256B):").Append(bs.SmallBufferCount)
                  .AppendLine("  (per-object/per-instance constant buffers)");
                if (bs.HasPerInstanceBuffer)
                    sb.AppendLine("  PerInstanceStride32: YES — 32-byte per-instance buffer present (GPU skinning / Character signal)");
            }
            // ── Classification rules ───────────────────────────────────────────
            sb.AppendLine();
            sb.AppendLine("Category definitions:");
            sb.AppendLine("  Scene              — static world geometry rendered to the main HDR buffer (buildings, props, stadium, pitch).");
            sb.AppendLine("  Terrain            — heightfield/ground mesh, uses virtual/heightfield textures or terrain-specific lightmaps.");
            sb.AppendLine("  Character          — dynamic skinned mesh (players, crowd) with per-object SH probe lighting, no lightmap UVs.");
            sb.AppendLine("  PostProcess        — fullscreen-quad pass that reads from a previously rendered texture and writes to an RT.");
            sb.AppendLine("  VFX                — particle systems, billboard quads, or other effect geometry (many small instances).");
            sb.AppendLine("  UI                 — 2D interface elements, no depth RT, typically RGBA8 color RT.");
            sb.AppendLine("  Other              — compute dispatches.");
            sb.AppendLine("  Scene(Shadow)      — shadow map pass rendering SCENE geometry (world objects) into a depth or encoded-depth RT.");
            sb.AppendLine("  Terrain(Shadow)    — shadow map pass rendering TERRAIN geometry into a depth or encoded-depth RT.");
            sb.AppendLine("  Character(Shadow)  — shadow map pass rendering CHARACTER geometry (players/crowd) into a depth or encoded-depth RT.");
            sb.AppendLine();
            sb.AppendLine("Rules (apply in order):");
            sb.AppendLine("R1 [Render targets first — HIGHEST PRIORITY, overrides everything else]");
            sb.AppendLine("  ** RULE R1a: Depth-only RT, no Color RT → SHADOW MAP PASS. Determine which object type is being rendered from the shader (Scene/Terrain/Character) and output the matching shadow category: 'Scene(Shadow)', 'Terrain(Shadow)', or 'Character(Shadow)'. Default to 'Scene(Shadow)' if indeterminate.");
            sb.AppendLine("  ** RULE R1b: Color RT whose format has only 2 channels (any format starting with R8G8, R16G16, R32G32, RG, or similar 2-component format), AND no Depth RT present, AND geometry is a real mesh (VertexCount>6 or has IndexBuffer) → this is an ENCODED DEPTH SHADOW MAP (VSM/ESM — depth and depth² packed into the two color channels). Output the matching shadow category: 'Scene(Shadow)', 'Terrain(Shadow)', or 'Character(Shadow)'. Do NOT output bare 'Scene' — the category itself must end in '(Shadow)'.");
            sb.AppendLine("  ** RULE R1b exception: if VertexCount is 3-6 AND no IndexBuffer → fullscreen shadow blur/filter, use PostProcess instead.");
            sb.AppendLine("  Color-only RT, no Depth, R8G8B8A8/B8G8R8A8 format → UI");
            sb.AppendLine("  Color-only RT, no Depth, HDR/float format, screen-size → PostProcess");
            sb.AppendLine("  Color HDR + Depth (D24/D32), VertsPerInst<=6, many instances → VFX (particles/quads)");
            sb.AppendLine("  Color HDR + Depth (D24/D32), VertsPerInst>6, normal geometry → Scene/Character/Terrain");
            sb.AppendLine("R1b [Geometry scale signals]");
            sb.AppendLine("  UI:          typically vkCmdDraw (no index buffer), very few total verts (3-6 per quad), no depth RT.");
            sb.AppendLine("  VFX:         vkCmdDraw or indexed, VertsPerInst<=6 (billboard quads), many instances (particles).");
            sb.AppendLine("  PostProcess: vkCmdDraw with 3 or 4 verts total (fullscreen triangle/quad), no index buffer.");
            sb.AppendLine("  Character:   indexed mesh, VertexCount in hundreds–tens-of-thousands, InstanceCount usually 1-4.");
            sb.AppendLine("  Scene:       indexed mesh, VertexCount can be very large, InstanceCount 1 or many (instanced props).");
            sb.AppendLine("R2 [Shader main() for Scene/Character/Terrain]");
            sb.AppendLine("  Scene:     lightmap textures sampled in main() using TEXCOORD2 UVs (irradiance/sky-visibility/baked-visibility)");
            sb.AppendLine("  Character: per-object SH probe Buffer<float4> loaded via per-instance offset from an instance buffer — no lightmap sampling.");
            sb.AppendLine("             Also: per-instance cosmetic data (tint/recolor) read from instance buffer in main(), or skinned vertex buffer offset in PerInstanceData.");
            sb.AppendLine("  Terrain:   lightmaps sampled + terrain-specific expression shader function fields read in main(), or virtual/heightfield texture sampling");
            sb.AppendLine("R3 [cbuffer vs texture priority — CRITICAL]");
            sb.AppendLine("  The first/global cbuffer (usually register b0 or b11) is a SHARED per-pass buffer present in EVERY draw call.");
            sb.AppendLine("  It typically contains terrain, shadow, lighting structs etc. — ALL irrelevant unless explicitly READ in main().");
            sb.AppendLine("  IGNORE global cbuffer struct declarations entirely. Only what is used inside main()/frag_main() matters.");
            sb.AppendLine("  TRUST ORDER (most reliable → least): texture/sampler calls in main() > secondary cbuffers/typed buffers > global cbuffer fields.");
            sb.AppendLine("  If a typed Buffer<float4> or ByteAddressBuffer is bound for per-object SH/probe data and loaded via a per-instance");
            sb.AppendLine("  offset in main(), that is a STRONG signal of a dynamic object (Character) — terrain is always static baked and");
            sb.AppendLine("  never needs per-object SH probe buffers.");
            sb.AppendLine("  SH probe data = dynamic object lighting (Character or dynamic props). Terrain never binds per-object SH probe buffers.");
            // ── Shader source ──────────────────────────────────────────────────
            sb.AppendLine();
            sb.AppendLine("Shader (analyze what is actually computed in main()):");
            sb.AppendLine(shaderCode);
            // ── Output format ──────────────────────────────────────────────────
            sb.AppendLine();
            sb.Append("Categories: ").AppendLine(catList);
            sb.AppendLine("IMPORTANT RESTRICTIONS:");
            sb.AppendLine("  - 'Other' is ONLY valid for vkCmdDispatch compute passes. NEVER use 'Other' for vkCmdDraw or vkCmdDrawIndexed.");
            sb.AppendLine("  - If R1a/R1b apply (shadow map RT), you MUST use a shadow category. Do NOT fall back to 'Other' when uncertain about object type — default to 'Scene(Shadow)'.");
            sb.AppendLine("  - If the shader detail mentions 'shadow', 'planar shadow', or 'depth encoding', the category MUST end in '(Shadow)'.");
            sb.AppendLine("Output JSON only, no markdown: {\"category\":\"<category>\",\"detail\":\"<3-8 word description>\"}");
            return sb.ToString();
        }

        // ── Shader file loading ───────────────────────────────────────────────

        private string LoadShaderCode(string shaderBaseDir, string drawCallNumber)
        {
            string dcDir = Path.Combine(shaderBaseDir, "dc_" + drawCallNumber);
            if (!Directory.Exists(dcDir))
                return "(shader directory not found - shaders may not have been extracted yet)";

            var files = Directory.GetFiles(dcDir, "*.hlsl")
                .Concat(Directory.GetFiles(dcDir, "*.glsl"))
                .OrderBy(f => f)
                .ToList();

            if (files.Count == 0)
                return "(no decompiled shader files - only raw SPIR-V available)";

            // If ALL shader files are tiny stubs (<200 bytes) they are empty entry-point wrappers.
            // Skip LLM entirely for these (they have empty main bodies: `void frag_main() {}`).
            long totalSize = files.Sum(f => new System.IO.FileInfo(f).Length);
            if (totalSize < 200 * files.Count)
                return "(empty shader — trivial stub, no rendering operations)";

            var sb = new StringBuilder();
            foreach (var file in files)
            {
                sb.AppendLine("// === " + Path.GetFileName(file) + " ===");
                string content = File.ReadAllText(file, Encoding.UTF8);
                sb.AppendLine(ExtractRelevantShaderSections(content));
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Extracts the two most useful sections for LLM classification, instead of blindly
        /// truncating from the top (which would only show cbuffer struct declarations).
        ///
        /// Section 1 — Resource declarations (up to resourceLimit chars):
        ///   Texture/Buffer/Sampler global declarations and small per-draw cbuffers.
        ///   The giant shared per-pass cbuffer is skipped entirely (it is noise for classification).
        ///
        /// Section 2 — main() / frag_main() body (up to mainLimit chars):
        ///   The actual computation the shader performs.
        /// </summary>
        private string ExtractRelevantShaderSections(string src,
            int resourceLimit = 2500, int mainLimit = 5000)
        {
            var lines = src.Split('\n');
            var resources = new StringBuilder();
            var mainBody  = new StringBuilder();

            // ── Section 1: resource declarations, skip the giant shared cbuffer ──
            bool inSkippedCbuffer = false;
            int  braceDepth = 0;
            int  resChars   = 0;
            bool resLimitHit = false;

            foreach (var rawLine in lines)
            {
                string line = rawLine.TrimEnd();

                if (inSkippedCbuffer)
                {
                    // Count braces to find the end of the skipped block
                    foreach (char c in line) { if (c == '{') braceDepth++; else if (c == '}') braceDepth--; }
                    if (braceDepth <= 0) inSkippedCbuffer = false;
                    continue;
                }

                // Detect start of the large global per-pass cbuffer (very large, noise for LLM)
                // Heuristic: cbuffer with more than ~10 lines is the shared global one; skip it.
                // We detect by the QuadFilterWeights[120] or similar huge arrays, or simply
                // by matching a cbuffer that is NOT register(b0)/b1/b2 (small per-draw cbuffers).
                if (line.StartsWith("cbuffer ") && !line.Contains(": register(b0)") &&
                    !line.Contains(": register(b1)") && !line.Contains(": register(b2)"))
                {
                    if (line.Contains("{")) braceDepth = 1;
                    else braceDepth = 0;
                    inSkippedCbuffer = true;
                    resources.AppendLine("// [large shared cbuffer skipped — see R3]");
                    continue;
                }

                // Skip static local variables (noise)
                if (line.StartsWith("static ")) continue;

                // Include: struct definitions, small cbuffers (b0/b1/b2), Texture/Buffer/Sampler declarations
                bool isResource = line.StartsWith("Texture") || line.StartsWith("RWTexture") ||
                                  line.StartsWith("Buffer<") || line.StartsWith("RWBuffer") ||
                                  line.StartsWith("ByteAddressBuffer") || line.StartsWith("RWByteAddressBuffer") ||
                                  line.StartsWith("SamplerState") || line.StartsWith("SamplerComparisonState") ||
                                  line.StartsWith("cbuffer ") || line.StartsWith("struct ") ||
                                  (inSkippedCbuffer == false && line.StartsWith("{")) ||
                                  line.StartsWith("}") || line.StartsWith("    ") || line.StartsWith("\t");

                if (!resLimitHit)
                {
                    resources.AppendLine(line);
                    resChars += line.Length + 1;
                    if (resChars >= resourceLimit) { resLimitHit = true; resources.AppendLine("// ... (resource section truncated)"); }
                }
            }

            // ── Section 2: main / frag_main / vert_main body ──────────────────
            int mainStart = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                string t = lines[i].TrimStart();
                if (t.StartsWith("void main(") || t.StartsWith("void frag_main(") ||
                    t.StartsWith("void vert_main(") || t.StartsWith("void comp_main("))
                {
                    mainStart = i;
                    break;
                }
            }

            if (mainStart >= 0)
            {
                int chars = 0;
                for (int i = mainStart; i < lines.Length && chars < mainLimit; i++)
                {
                    mainBody.AppendLine(lines[i]);
                    chars += lines[i].Length + 1;
                }
                if (chars >= mainLimit)
                    mainBody.AppendLine("// ... (main body truncated)");
            }
            else
            {
                mainBody.AppendLine("// (main() not found in shader)");
            }

            // Detect empty shader: strip whitespace, braces, and trivial stage-forwarder calls
            // (e.g. `frag_main();` / `vert_main();`), then check if anything meaningful remains.
            string mainBodyStr = mainBody.ToString();
            string stripped = Regex.Replace(mainBodyStr,
                @"^\s*(\{|\}|[a-z]+_main\s*\(\s*\)\s*;|void\s+\w+\s*\(.*?\))\s*$",
                "", RegexOptions.Multiline).Trim();
            bool mainIsEmpty = stripped.Length < 5;  // only braces/whitespace/forwarder lines left

            if (mainIsEmpty)
                return "(empty shader — no executable statements in main)";  

            var result = new StringBuilder();
            result.AppendLine("// -- Resource declarations (textures, buffers, small cbuffers) --");
            result.Append(resources);
            result.AppendLine();
            result.AppendLine("// -- main() / frag_main() body --");
            result.Append(mainBody);
            return result.ToString();
        }

        // ── LLM response parsing ──────────────────────────────────────────────

        private DrawCallLabel? ParseLlmResponse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            int start = text.IndexOf('{');
            int end   = text.LastIndexOf('}');
            if (start < 0 || end <= start) return null;

            string jsonStr = text.Substring(start, end - start + 1);

            string rawCat = "";
            string detail = "";

            // Primary: standard JSON parse
            JObject? obj = null;
            try { obj = JObject.Parse(jsonStr); }
            catch { /* fall through to regex */ }

            if (obj != null)
            {
                rawCat = obj["category"]?.ToString()?.Trim() ?? "";
                detail = obj["detail"]?.ToString()?.Trim()   ?? "";
            }
            else
            {
                // Fallback: regex extraction for malformed JSON (e.g. missing commas)
                var catMatch = Regex.Match(jsonStr, "\"category\"\\s*:\\s*\"([^\"]+)\"");
                var detMatch = Regex.Match(jsonStr, "\"detail\"\\s*:\\s*\"([^\"]+)\"");
                if (!catMatch.Success) return null;
                rawCat = catMatch.Groups[1].Value.Trim();
                detail = detMatch.Success ? detMatch.Groups[1].Value.Trim() : "";
            }

            // Exact match first, then partial
            string? matched = _allowedCategories.FirstOrDefault(
                c => string.Equals(c, rawCat, StringComparison.OrdinalIgnoreCase));
            if (matched == null)
                matched = _allowedCategories.FirstOrDefault(
                    c => rawCat.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0);

            // Accept compound shadow categories: e.g. "Character(Shadow)" where base is an allowed category
            if (matched == null)
            {
                var shadowMatch = Regex.Match(rawCat, @"^(\w+)\(Shadow\)$", RegexOptions.IgnoreCase);
                if (shadowMatch.Success)
                {
                    string baseCategory = shadowMatch.Groups[1].Value;
                    if (_allowedCategories.Any(c => string.Equals(c, baseCategory, StringComparison.OrdinalIgnoreCase)))
                        return new DrawCallLabel { Category = baseCategory + "(Shadow)", Detail = detail };
                }
            }

            if (matched == null) return null;
            return new DrawCallLabel { Category = matched, Detail = detail };
        }

        // ── Rule-based fallback ───────────────────────────────────────────────

        private DrawCallLabel LabelByRules(DrawCallInfo dc)
        {
            string entries   = string.Join(" ", dc.Shaders.Select(s => s.EntryPoint ?? ""));
            string entryLow  = entries.ToLowerInvariant();
            bool isCompute   = string.Equals(dc.ApiName, "vkCmdDispatch", StringComparison.OrdinalIgnoreCase);
            bool isFullscreen = !isCompute
                                && (dc.VertexCount == 3 || dc.VertexCount == 4 || dc.VertexCount == 6)
                                && dc.IndexBuffer == null
                                && dc.VertexBuffers.Count <= 1;

            // RG-encoded shadow map: color-only 2-channel RT (VSM/ESM depth packed into RG channels),
            // no depth attachment, real geometry (not a fullscreen quad post-process filter)
            if (!isFullscreen && !isCompute && dc.RenderTargets.Count > 0)
            {
                bool hasDepth  = dc.RenderTargets.Any(r =>
                    r.AttachmentType == "Depth" || r.AttachmentType == "Stencil" || r.AttachmentType == "DepthStencil");
                bool allColorRG = dc.RenderTargets.All(r =>
                    r.AttachmentType != "Color" ||
                    (r.FormatName != null && (
                        r.FormatName.StartsWith("R8G8", StringComparison.OrdinalIgnoreCase) ||
                        r.FormatName.StartsWith("R16G16", StringComparison.OrdinalIgnoreCase) ||
                        r.FormatName.StartsWith("R32G32", StringComparison.OrdinalIgnoreCase))));
                bool hasAnyColor = dc.RenderTargets.Any(r => r.AttachmentType == "Color");
                if (hasAnyColor && !hasDepth && allColorRG)
                {
                    string baseCat = _allowedCategories.Contains("Scene") ? "Scene" : _allowedCategories[0];
                    return new DrawCallLabel { Category = baseCat + "(Shadow)", Detail = "Encoded shadow map (RG depth)" };
                }
            }

            foreach (var (keywords, category, detailHint) in Rules)
            {
                if (!_allowedCategories.Contains(category)) continue;
                if (HitAny(entryLow, keywords))
                {
                    string detail = PrettifyEntry(entries);
                    return new DrawCallLabel { Category = category, Detail = string.IsNullOrEmpty(detail) ? detailHint : detail };
                }
            }

            if (isCompute && _allowedCategories.Contains("PostProcess"))
                return new DrawCallLabel { Category = "PostProcess", Detail = PrettifyEntryOr(entries, "Compute pass") };

            if (isFullscreen && _allowedCategories.Contains("PostProcess"))
                return new DrawCallLabel { Category = "PostProcess", Detail = PrettifyEntryOr(entries, "Fullscreen pass") };

            string defCat = _allowedCategories.Contains("Scene") ? "Scene" : _allowedCategories[0];
            return new DrawCallLabel { Category = defCat, Detail = PrettifyEntryOr(entries, "Scene rendering") };
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static bool HitAny(string text, string[] keywords)
        {
            foreach (var kw in keywords)
                if (text.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        private static string PrettifyEntryOr(string raw, string fallback)
        {
            string p = PrettifyEntry(raw);
            return string.IsNullOrEmpty(p) ? fallback : p;
        }

        private static string PrettifyEntry(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string s = raw.Split(' ')[0].Trim();
            s = Regex.Replace(s, @"^(vs|ps|fs|cs|vert|frag|comp)", "", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, @"(?<=[a-z])(?=[A-Z])", " ");
            s = s.Replace("_", " ");
            return s.Trim();
        }

        // ── Null logger stub ─────────────────────────────────────────────────

        private class NullLogger : ILogger
        {
            public void Debug(string msg)   { }
            public void Info(string msg)    { }
            public void Warning(string msg) { }
            public void Error(string msg)   { }
            public void Success(string msg) { }
        }
    }
}
