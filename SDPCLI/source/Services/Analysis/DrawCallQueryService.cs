using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using QGLPlugin;
using SnapdragonProfilerCLI.Models;
using ShaderInfo = SnapdragonProfilerCLI.Models.ShaderInfo;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Resolves all Vulkan resources for a DrawCall from sdp.db.
    ///
    /// Priority order for each data source:
    ///   DrawCall list   : DrawCallParameters (rowid order)
    ///   Pipeline ID     : DrawCallBindings.PipelineID (direct join on DrawCallApiID)
    ///   Textures        : DrawCallBindings.ImageViewID > VulkanSnapshotDescriptorSetBindings
    ///   Render targets  : DrawCallRenderTargets  (direct join on DrawCallApiID)
    ///   Vertex buffers  : DrawCallVertexBuffers  (direct join on DrawCallApiID)
    ///   Index buffer    : DrawCallIndexBuffers   (direct join on DrawCallApiID)
    ///   Shaders         : VulkanSnapshotShaderStages (join on pipelineID + captureID)
    ///   Texture detail  : VulkanSnapshotTextures + VulkanSnapshotImageViews
    ///
    /// Stateless -- safe to inject as singleton; dbPath and captureId are per-call.
    /// </summary>
    public class DrawCallQueryService
    {
        // -- Public API -------------------------------------------------------

        /// <summary>
        /// Resolves full DrawCall info.
        /// drawCallNumber may be a plain integer DrawCallApiID ("106974") or encoded ("1.1.5").
        /// </summary>
        public DrawCallInfo? GetDrawCallInfo(string dbPath, uint captureId, string drawCallNumber)
        {
            try
            {
                using var conn = Open(dbPath);

                // Prefer direct ApiID lookup when drawCallNumber is a plain integer
                if (uint.TryParse(drawCallNumber, out uint apiId))
                    return GetDrawCallInfoByApiId(conn, captureId, apiId, drawCallNumber);

                // Encoded format (1.1.N) -- treat N as 1-based row position in DrawCallParameters
                return GetDrawCallInfoByEncodedId(conn, captureId, drawCallNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetDrawCallInfo failed for '{drawCallNumber}': {ex.Message}");
                return null;
            }
        }

        /// <summary>Returns texture IDs bound to a DrawCall.</summary>
        public uint[] GetTexturesForDrawCall(string dbPath, uint captureId, string drawCallNumber)
        {
            var info = GetDrawCallInfo(dbPath, captureId, drawCallNumber);
            return info?.TextureIDs ?? Array.Empty<uint>();
        }

        /// <summary>Returns texture IDs for a pipeline resourceID.</summary>
        public uint[] GetTexturesForPipelineId(string dbPath, uint captureId, uint pipelineID)
        {
            try
            {
                using var conn = Open(dbPath);
                return GetTextureIdsFallback(conn, captureId);
            }
            catch (Exception ex) { Console.WriteLine($"  GetTexturesForPipelineId failed: {ex.Message}"); return Array.Empty<uint>(); }
        }

        /// <summary>Returns DrawCallInfo keyed by pipeline resourceID.</summary>
        public DrawCallInfo? GetPipelineInfoById(string dbPath, uint captureId, uint pipelineID)
        {
            try
            {
                using var conn = Open(dbPath);
                var pl = GetPipelineByResourceID(conn, captureId, pipelineID);
                if (pl == null) return null;
                var textureIDs = GetTextureIdsFallback(conn, captureId);
                return new DrawCallInfo
                {
                    DrawCallNumber = $"Pipeline_{pipelineID}",
                    PipelineID     = pl.Value.pipelineID,
                    LayoutID       = pl.Value.layoutID,
                    RenderPass     = pl.Value.renderPass,
                    TextureIDs     = textureIDs,
                    Textures       = GetTextureDetails(conn, captureId, textureIDs),
                    Shaders        = GetShadersForPipeline(conn, captureId, pl.Value.pipelineID)
                };
            }
            catch (Exception ex) { Console.WriteLine($"  GetPipelineInfoById failed: {ex.Message}"); return null; }
        }

        // -- Core lookup: DrawCallParameters-based (preferred) ----------------

        private static DrawCallInfo? GetDrawCallInfoByApiId(
            SQLiteConnection conn, uint captureId, uint apiId, string label)
        {
            // 1. DrawCall metadata from DrawCallParameters
            string apiName     = "";
            uint vertexCount   = 0, indexCount = 0, instanceCount = 0;

            if (TableExists(conn, "DrawCallParameters"))
            {
                using var cmd = new SQLiteCommand(
                    "SELECT ApiName, VertexCount, IndexCount, InstanceCount " +
                    "FROM DrawCallParameters WHERE DrawCallApiID=@id LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@id", apiId);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    apiName       = r["ApiName"].ToString() ?? "";
                    vertexCount   = Convert.ToUInt32(r["VertexCount"]);
                    indexCount    = Convert.ToUInt32(r["IndexCount"]);
                    instanceCount = Convert.ToUInt32(r["InstanceCount"]);
                }
            }

            // 2. Pipeline via DrawCallBindings
            (uint pipelineID, uint layoutID, uint renderPass)? pl = null;
            if (TableExists(conn, "DrawCallBindings"))
                pl = GetPipelineFromBindings(conn, captureId, apiId);

            // Fallback: first graphics pipeline for capture
            if (pl == null)
                pl = GetPipelineByGlobalIndex(conn, captureId, 0);

            uint pipeId    = pl?.pipelineID ?? 0;
            uint layoutId  = pl?.layoutID   ?? 0;
            uint renderPass = pl?.renderPass ?? 0;

            // 3. Textures
            uint[] textureIDs = GetTexturesForApiId(conn, captureId, apiId);

            // 4. Render targets
            var renderTargets = GetRenderTargets(conn, apiId, captureId);

            // 5. Vertex + index buffers
            var vertexBuffers = GetVertexBuffers(conn, apiId);
            var indexBuffer   = GetIndexBuffer(conn, apiId);

            // 6. Descriptor binding summary (typed buffer views, small per-object buffers)
            var bindingSummary = GetBindingSummary(conn, captureId, apiId);

            // 6. Shaders
            var shaders = pipeId > 0
                ? GetShadersForPipeline(conn, captureId, pipeId)
                : new List<ShaderInfo>();

            // 7. Texture details
            var textures = GetTextureDetails(conn, captureId, textureIDs);

            return new DrawCallInfo
            {
                DrawCallNumber = label,
                ApiID          = apiId,
                ApiName        = apiName,
                PipelineID     = pipeId,
                LayoutID       = layoutId,
                RenderPass     = renderPass,
                VertexCount    = vertexCount,
                IndexCount     = indexCount,
                InstanceCount  = instanceCount,
                TextureIDs     = textureIDs,
                Textures       = textures,
                Shaders        = shaders,
                RenderTargets  = renderTargets,
                VertexBuffers  = vertexBuffers,
                IndexBuffer    = indexBuffer,
                BindingSummary = bindingSummary
            };
        }

        // -- Encoded "1.1.N" format: positional index into DrawCallParameters --

        private static DrawCallInfo? GetDrawCallInfoByEncodedId(
            SQLiteConnection conn, uint captureId, string drawCallNumber)
        {
            var id = ParseEncodedDrawCallNumber(drawCallNumber);
            if (id == null) return null;

            // Use encoded components to look up the correct row
            uint apiId = GetApiIdByEncodedId(conn, id.Value.submitIdx, id.Value.cmdBufferIdx, id.Value.drawcallIdx);
            if (apiId == 0) return null;

            return GetDrawCallInfoByApiId(conn, captureId, apiId, drawCallNumber);
        }

        // -- Per-drawcall resource queries ------------------------------------

        /// <summary>Texture IDs for a DrawCallApiID.
        /// Prefers DrawCallBindings.imageViewID column ? VulkanSnapshotImageViews.imageID.
        /// Falls back to capture-wide VulkanSnapshotDescriptorSetBindings.</summary>
        private static uint[] GetTexturesForApiId(SQLiteConnection conn, uint captureId, uint apiId)
        {
            // -- Try DrawCallBindings (exported table) -------------------------
            if (TableExists(conn, "DrawCallBindings"))
            {
                string? ivCol = FindColumn(conn, "DrawCallBindings", "imageview", "ImageView");
                string? dcCol = FindColumn(conn, "DrawCallBindings", "DrawCallApiID", "ApiID", "drawcall");

                if (ivCol != null && dcCol != null)
                {
                    var imageViewIDs = new List<uint>();
                    using (var cmd = new SQLiteCommand(
                        $"SELECT DISTINCT [{ivCol}] FROM DrawCallBindings " +
                        $"WHERE [{dcCol}]={apiId} AND CaptureID={captureId} AND [{ivCol}]>0", conn))
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) imageViewIDs.Add(Convert.ToUInt32(r[0]));

                    if (imageViewIDs.Count > 0)
                    {
                        // We found imageView IDs — resolve to texture IDs (some may be filtered by aspectMask).
                        // Even if all are filtered out, do NOT fall back to capture-wide list.
                        var ids = ResolveImageViewIds(conn, captureId, imageViewIDs);
                        Console.WriteLine($"  ? {ids.Length} textures from DrawCallBindings (apiID={apiId}, {imageViewIDs.Count} views, {imageViewIDs.Count - ids.Length} filtered)");
                        return ids;
                    }
                }
            }

            return Array.Empty<uint>();
        }

        private static uint[] ResolveImageViewIds(SQLiteConnection conn, uint captureId, List<uint> ivIDs)
        {
            var result = new List<uint>();
            foreach (var ivid in ivIDs)
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        $"SELECT imageID FROM VulkanSnapshotImageViews " +
                        $"WHERE captureID={captureId} AND resourceID={ivid}", conn);
                    using var r = cmd.ExecuteReader();
                    if (r.Read() && !r.IsDBNull(0))
                        result.Add(Convert.ToUInt32(r[0]));
                }
                catch { }
            }
            return result.ToArray();
        }

        private static List<RenderTargetInfo> GetRenderTargets(SQLiteConnection conn, uint apiId, uint captureId)
        {
            var list = new List<RenderTargetInfo>();
            if (!TableExists(conn, "DrawCallRenderTargets")) return list;
            try
            {
                // Build WHERE clause — include CaptureID filter when the column exists
                bool hasCaptureCol = ColumnExists(conn, "DrawCallRenderTargets", "CaptureID");
                string where = hasCaptureCol
                    ? $"DrawCallApiID={apiId} AND CaptureID={captureId}"
                    : $"DrawCallApiID={apiId}";
                using var cmd = new SQLiteCommand(
                    "SELECT RenderPassID, FramebufferID, AttachmentIndex, AttachmentResourceID, AttachmentType " +
                    $"FROM DrawCallRenderTargets WHERE {where} ORDER BY AttachmentIndex", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    var rt = new RenderTargetInfo
                    {
                        RenderPassID         = Convert.ToUInt32(r[0]),
                        FramebufferID        = Convert.ToUInt32(r[1]),
                        AttachmentIndex      = Convert.ToUInt32(r[2]),
                        AttachmentResourceID = Convert.ToUInt32(r[3]),
                        AttachmentType       = r[4].ToString() ?? ""
                    };
                    // Resolve format/size via VulkanSnapshotImageViews → VulkanSnapshotTextures (accumulating tables)
                    ResolveRenderTargetFormat(conn, captureId, rt);
                    list.Add(rt);
                }
            }
            catch (Exception ex) { Console.WriteLine($"  RenderTargets query failed: {ex.Message}"); }
            return list;
        }

        private static bool IsDepthFormat(string? formatName)
        {
            if (string.IsNullOrEmpty(formatName)) return false;
            // Matches D16_UNORM, D24_UNORM_S8_UINT, D32_SFLOAT, D32_SFLOAT_S8_UINT,
            // X8_D24_UNORM_PACK32, VK_FORMAT_D16_UNORM, etc.
            return formatName!.IndexOf("D16",   StringComparison.OrdinalIgnoreCase) >= 0
                || formatName.IndexOf("D24",   StringComparison.OrdinalIgnoreCase) >= 0
                || formatName.IndexOf("D32",   StringComparison.OrdinalIgnoreCase) >= 0
                || formatName.IndexOf("DEPTH", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void ResolveRenderTargetFormat(SQLiteConnection conn, uint captureId, RenderTargetInfo rt)
        {
            try
            {
                // Try image-view path first
                uint imageId = 0;
                if (TableExists(conn, "VulkanSnapshotImageViews"))
                {
                    using var cmd = new SQLiteCommand(
                        $"SELECT imageID FROM VulkanSnapshotImageViews " +
                        $"WHERE captureID={captureId} AND resourceID={rt.AttachmentResourceID} LIMIT 1", conn);
                    using var r = cmd.ExecuteReader();
                    if (r.Read() && !r.IsDBNull(0)) imageId = Convert.ToUInt32(r[0]);
                }

                // Fall back: AttachmentResourceID might be a direct texture resourceID
                if (imageId == 0) imageId = rt.AttachmentResourceID;

                if (!TableExists(conn, "VulkanSnapshotTextures")) return;
                using var cmd2 = new SQLiteCommand(
                    $"SELECT width, height, format FROM VulkanSnapshotTextures " +
                    $"WHERE captureID={captureId} AND resourceID={imageId} LIMIT 1", conn);
                using var r2 = cmd2.ExecuteReader();
                if (r2.Read())
                {
                    rt.Width      = Convert.ToUInt32(r2[0]);
                    rt.Height     = Convert.ToUInt32(r2[1]);
                    rt.FormatName = GetFormatName(Convert.ToUInt32(r2[2]));
                }
            }
            catch { /* format resolution is best-effort */ }
        }

        private static List<VertexBufferBinding> GetVertexBuffers(SQLiteConnection conn, uint apiId)
        {
            var list = new List<VertexBufferBinding>();
            if (!TableExists(conn, "DrawCallVertexBuffers")) return list;
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT Binding, BufferID FROM DrawCallVertexBuffers WHERE DrawCallApiID={apiId} ORDER BY Binding", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    list.Add(new VertexBufferBinding
                    {
                        Binding  = Convert.ToUInt32(r[0]),
                        BufferID = Convert.ToUInt32(r[1])
                    });
            }
            catch (Exception ex) { Console.WriteLine($"  VertexBuffers query failed: {ex.Message}"); }
            return list;
        }

        private static IndexBufferBinding? GetIndexBuffer(SQLiteConnection conn, uint apiId)
        {
            if (!TableExists(conn, "DrawCallIndexBuffers")) return null;
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT BufferID, Offset, IndexType FROM DrawCallIndexBuffers WHERE DrawCallApiID={apiId} LIMIT 1", conn);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                    return new IndexBufferBinding
                    {
                        BufferID  = Convert.ToUInt32(r[0]),
                        Offset    = Convert.ToUInt32(r[1]),
                        IndexType = r[2].ToString() ?? ""
                    };
            }
            catch (Exception ex) { Console.WriteLine($"  IndexBuffer query failed: {ex.Message}"); }
            return null;
        }

        private static DescriptorBindingSummary GetBindingSummary(SQLiteConnection conn, uint captureId, uint apiId)
        {
            var result = new DescriptorBindingSummary();
            if (!TableExists(conn, "DrawCallBindings")) return result;
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT TexBufferView, BufferID, Range FROM DrawCallBindings " +
                    $"WHERE DrawCallApiID={apiId} AND CaptureID={captureId}", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    long texBufView = r[0] == DBNull.Value ? 0 : Convert.ToInt64(r[0]);
                    long bufferId   = r[1] == DBNull.Value ? 0 : Convert.ToInt64(r[1]);
                    long range      = r[2] == DBNull.Value ? 0 : Convert.ToInt64(r[2]);

                    if (texBufView != 0)
                        result.TypedBufferViewCount++;

                    if (bufferId != 0 && range > 0 && range <= 256)
                    {
                        result.SmallBufferCount++;
                        if (range == 32)
                            result.HasPerInstanceBuffer = true;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"  BindingSummary query failed: {ex.Message}"); }
            return result;
        }

        // -- Pipeline resolution -----------------------------------------------

        /// <summary>Get PipelineID from DrawCallBindings directly (DrawCallApiID + CaptureID → PipelineID).</summary>
        private static (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineFromBindings(
            SQLiteConnection conn, uint captureId, uint apiId)
        {
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT PipelineID FROM DrawCallBindings " +
                    $"WHERE DrawCallApiID={apiId} AND CaptureID={captureId} AND PipelineID>0 LIMIT 1", conn);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    uint pid = Convert.ToUInt32(r[0]);
                    var resolved = GetPipelineByResourceID(conn, captureId, pid);
                    return resolved ?? (pid, 0u, 0u);
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Get DrawCallApiID for an encoded draw call ID (submit.cmdBuf.drawcall).
        /// Prefers looking up by CmdBufferIdx+DrawcallIdx columns (new CSV format);
        /// falls back to positional OFFSET for old captures without those columns.
        /// </summary>
        private static uint GetApiIdByEncodedId(SQLiteConnection conn,
            uint submitIdx, uint cmdBufferIdx, uint drawcallIdx)
        {
            try
            {
                // New format: CmdBufferIdx and DrawcallIdx columns exported from ParseApiTrace
                bool hasNewCols = false;
                using (var probe = new SQLiteCommand(
                    "PRAGMA table_info(DrawCallParameters)", conn))
                using (var pr = probe.ExecuteReader())
                    while (pr.Read())
                        if (pr["name"].ToString() == "CmdBufferIdx") { hasNewCols = true; break; }

                if (hasNewCols)
                {
                    using var cmd = new SQLiteCommand(
                        $"SELECT DrawCallApiID FROM DrawCallParameters " +
                        $"WHERE CmdBufferIdx={cmdBufferIdx} AND DrawcallIdx={drawcallIdx} LIMIT 1", conn);
                    using var r = cmd.ExecuteReader();
                    if (r.Read()) return Convert.ToUInt32(r[0]);
                }

                // Old format fallback: treat drawcallIdx as 1-based global row position
                int position = (int)drawcallIdx - 1;
                if (position < 0) return 0;
                using var cmd2 = new SQLiteCommand(
                    $"SELECT DrawCallApiID FROM DrawCallParameters ORDER BY rowid LIMIT 1 OFFSET {position}", conn);
                using var r2 = cmd2.ExecuteReader();
                if (r2.Read()) return Convert.ToUInt32(r2[0]);
            }
            catch { }
            return 0;
        }

        private static (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByGlobalIndex(
            SQLiteConnection conn, uint captureId, int index)
        {
            if (index < 0) return null;
            using var cmd = new SQLiteCommand(
                $"SELECT resourceID,layoutID,renderPass FROM VulkanSnapshotGraphicsPipelines " +
                $"WHERE captureID={captureId} ORDER BY resourceID LIMIT 1 OFFSET {index}", conn);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return (Convert.ToUInt32(r[0]), Convert.ToUInt32(r[1]), Convert.ToUInt32(r[2]));
            return null;
        }

        private static (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByResourceID(
            SQLiteConnection conn, uint captureId, uint pipelineID)
        {
            // Graphics pipeline
            using (var cmd = new SQLiteCommand(
                $"SELECT resourceID,layoutID,renderPass FROM VulkanSnapshotGraphicsPipelines " +
                $"WHERE captureID={captureId} AND resourceID={pipelineID}", conn))
            using (var r = cmd.ExecuteReader())
                if (r.Read()) return (Convert.ToUInt32(r[0]), Convert.ToUInt32(r[1]), Convert.ToUInt32(r[2]));

            // Compute pipeline (layoutID exists, no renderPass)
            if (TableExists(conn, "VulkanSnapshotComputePipelines"))
            {
                using var cmd2 = new SQLiteCommand(
                    $"SELECT resourceID,layoutID FROM VulkanSnapshotComputePipelines " +
                    $"WHERE captureID={captureId} AND resourceID={pipelineID}", conn);
                using var r2 = cmd2.ExecuteReader();
                if (r2.Read()) return (Convert.ToUInt32(r2[0]), Convert.ToUInt32(r2[1]), 0);
            }

            return null;
        }

        // -- Capture-wide fallback textures ------------------------------------

        private static uint[] GetTextureIdsFallback(SQLiteConnection conn, uint captureId)
        {
            var list = new List<uint>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT iv.imageID " +
                    $"FROM VulkanSnapshotDescriptorSetBindings dsb " +
                    $"JOIN VulkanSnapshotImageViews iv ON dsb.imageViewID=iv.resourceID AND iv.captureID=dsb.captureID " +
                    $"WHERE dsb.captureID={captureId} AND dsb.imageViewID>0 ORDER BY iv.imageID LIMIT 50", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read()) list.Add(Convert.ToUInt32(r[0]));
            }
            catch (Exception ex) { Console.WriteLine($"  Texture fallback warning: {ex.Message}"); }
            return list.ToArray();
        }

        // -- Texture details ---------------------------------------------------

        private static List<TextureInfo> GetTextureDetails(SQLiteConnection conn, uint captureId, uint[] ids)
        {
            if (ids.Length == 0) return new List<TextureInfo>();
            string idList = string.Join(",", ids.Take(50));
            var list = new List<TextureInfo>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT resourceID,width,height,depth,format,layerCount,levelCount " +
                    $"FROM VulkanSnapshotTextures WHERE captureID={captureId} AND resourceID IN ({idList})", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    uint fmt = Convert.ToUInt32(r[4]);
                    list.Add(new TextureInfo
                    {
                        TextureID  = Convert.ToUInt32(r[0]),
                        Width      = Convert.ToUInt32(r[1]),
                        Height     = Convert.ToUInt32(r[2]),
                        Depth      = Convert.ToUInt32(r[3]),
                        Format     = fmt,
                        LayerCount = Convert.ToUInt32(r[5]),
                        LevelCount = Convert.ToUInt32(r[6]),
                        FormatName = GetFormatName(fmt)
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine($"  TextureDetails error: {ex.Message}"); }
            return list;
        }

        // -- Shaders -----------------------------------------------------------

        private static List<ShaderInfo> GetShadersForPipeline(SQLiteConnection conn, uint captureId, uint pipelineID)
        {
            var list = new List<ShaderInfo>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT stageType,shaderModuleID,pName FROM VulkanSnapshotShaderStages " +
                    $"WHERE captureID={captureId} AND pipelineID={pipelineID} ORDER BY stageType", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    uint stage = Convert.ToUInt32(r[0]);
                    list.Add(new ShaderInfo
                    {
                        ShaderStage     = stage,
                        ShaderModuleID  = Convert.ToUInt64(r[1]),
                        EntryPoint      = r[2].ToString() ?? "main",
                        ShaderStageName = GetShaderStageName(stage)
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine($"  Shader query error: {ex.Message}"); }
            return list;
        }

        // -- Schema helpers ----------------------------------------------------

        private static bool TableExists(SQLiteConnection conn, string tableName)
        {
            using var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@n", conn);
            cmd.Parameters.AddWithValue("@n", tableName);
            return (long)cmd.ExecuteScalar() > 0;
        }

        /// <summary>Returns true if <paramref name="columnName"/> exists in <paramref name="tableName"/>.</summary>
        private static bool ColumnExists(SQLiteConnection conn, string tableName, string columnName)
        {
            try
            {
                using var cmd = new SQLiteCommand($"PRAGMA table_info([{tableName}])", conn);
                using var r   = cmd.ExecuteReader();
                while (r.Read())
                    if (string.Equals(r["name"].ToString(), columnName, StringComparison.OrdinalIgnoreCase))
                        return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Returns the first column name in <paramref name="tableName"/> whose name
        /// contains any of the <paramref name="substrings"/> (case-insensitive), or null.
        /// </summary>
        private static string? FindColumn(SQLiteConnection conn, string tableName, params string[] substrings)
        {
            try
            {
                using var cmd = new SQLiteCommand($"PRAGMA table_info([{tableName}])", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string col = r["name"].ToString() ?? "";
                    foreach (var sub in substrings)
                        if (col.IndexOf(sub, StringComparison.OrdinalIgnoreCase) >= 0)
                            return col;
                }
            }
            catch { }
            return null;
        }

        // -- Parsing -----------------------------------------------------------

        private static (uint submitIdx, uint cmdBufferIdx, uint drawcallIdx)? ParseEncodedDrawCallNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;
            var parts = number.Split('.');
            if (parts.Length == 3 && uint.TryParse(parts[0], out uint s) &&
                uint.TryParse(parts[1], out uint c) && uint.TryParse(parts[2], out uint d))
                return (s, c, d);
            if (parts.Length == 2 && uint.TryParse(parts[0], out s) && uint.TryParse(parts[1], out d))
                return (s, 0, d);
            return null;
        }

        // -- Helpers -----------------------------------------------------------

        private static SQLiteConnection Open(string dbPath)
        {
            var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            conn.Open();
            return conn;
        }

        private static string GetShaderStageName(uint stage) => stage switch
        {
            0x00000001 => "Vertex",
            0x00000002 => "TessellationControl",
            0x00000004 => "TessellationEvaluation",
            0x00000008 => "Geometry",
            0x00000010 => "Fragment",
            0x00000020 => "Compute",
            _ => $"Stage_{stage}"
        };

        private static string GetFormatName(uint fmt) => fmt switch
        {
            // 8-bit
            9   => "R8_UNORM",
            16  => "R8G8_UNORM",
            37  => "R8G8B8A8_UNORM",
            43  => "R8G8B8A8_SRGB",
            44  => "B8G8R8A8_UNORM",
            50  => "B8G8R8A8_SRGB",
            // 16-bit
            70  => "R16_UNORM",
            76  => "R16_SFLOAT",
            83  => "R16G16_SFLOAT",
            91  => "R16G16B16A16_UNORM",
            97  => "R16G16B16A16_SFLOAT",
            // 32-bit
            98  => "R32_UINT",
            100 => "R32_SFLOAT",
            103 => "R32G32_SFLOAT",
            106 => "R32G32B32_SFLOAT",
            109 => "R32G32B32A32_SFLOAT",
            // HDR packed color (NOT depth)
            122 => "B10G11R11_UFLOAT_PACK32",
            123 => "E5B9G9R9_UFLOAT_PACK32",
            // Depth/Stencil (correct Vulkan enum values)
            124 => "D16_UNORM",
            125 => "X8_D24_UNORM_PACK32",
            126 => "D32_SFLOAT",
            127 => "S8_UINT",
            128 => "D16_UNORM_S8_UINT",
            129 => "D24_UNORM_S8_UINT",
            130 => "D32_SFLOAT_S8_UINT",
            // BC compressed (correct Vulkan enum values)
            131 => "BC1_RGB_UNORM",
            133 => "BC1_RGBA_UNORM",
            135 => "BC2_UNORM",
            137 => "BC3_UNORM",
            139 => "BC4_UNORM",
            141 => "BC5_UNORM",
            143 => "BC6H_UFLOAT",
            144 => "BC6H_SFLOAT",
            145 => "BC7_UNORM",
            // ASTC compressed
            157 => "ASTC_4x4_UNORM",
            159 => "ASTC_5x4_UNORM",
            161 => "ASTC_5x5_UNORM",
            163 => "ASTC_6x5_UNORM",
            165 => "ASTC_6x6_UNORM",
            167 => "ASTC_8x5_UNORM",
            169 => "ASTC_8x6_UNORM",
            171 => "ASTC_8x8_UNORM",
            173 => "ASTC_10x5_UNORM",
            175 => "ASTC_10x6_UNORM",
            177 => "ASTC_10x8_UNORM",
            179 => "ASTC_10x10_UNORM",
            181 => "ASTC_12x10_UNORM",
            183 => "ASTC_12x12_UNORM",
            _   => $"Format_{fmt}"
        };
    }
}
