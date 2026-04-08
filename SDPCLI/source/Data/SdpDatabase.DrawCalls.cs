using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using SnapdragonProfilerCLI.Models;
using ShaderInfo = SnapdragonProfilerCLI.Models.ShaderInfo;

namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // ── Public: full DrawCallInfo resolution ──────────────────────────────

        /// <summary>
        /// Resolves full DrawCallInfo.
        /// drawCallNumber may be a plain integer DrawCallApiID ("106974") or encoded ("1.1.5").
        /// </summary>
        public DrawCallInfo? GetDrawCallInfo(string drawCallNumber)
        {
            try
            {
                using var conn = OpenConnection();

                if (uint.TryParse(drawCallNumber, out uint apiId))
                    return GetDrawCallInfoByApiId(conn, apiId, drawCallNumber);

                return GetDrawCallInfoByEncodedId(conn, drawCallNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetDrawCallInfo failed for '{drawCallNumber}': {ex.Message}");
                return null;
            }
        }

        public DrawCallInfo? GetDrawCallInfoByApiId(uint apiId, string label = "")
        {
            using var conn = OpenConnection();
            return GetDrawCallInfoByApiId(conn, apiId, string.IsNullOrEmpty(label) ? apiId.ToString() : label);
        }

        public DrawCallInfo? GetPipelineInfoById(uint pipelineID)
        {
            try
            {
                using var conn = OpenConnection();
                var pl = GetPipelineByResourceID(conn, pipelineID);
                if (pl == null) return null;
                var textureIDs = GetTextureIdsFallback(conn);
                return new DrawCallInfo
                {
                    DrawCallNumber = $"Pipeline_{pipelineID}",
                    PipelineID     = pl.Value.pipelineID,
                    LayoutID       = pl.Value.layoutID,
                    RenderPass     = pl.Value.renderPass,
                    TextureIDs     = textureIDs,
                    Textures       = GetTextureDetails(conn, textureIDs),
                    Shaders        = GetShadersForPipeline(conn, pl.Value.pipelineID)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetPipelineInfoById failed: {ex.Message}");
                return null;
            }
        }

        // ── Public sub-queries (also called directly by AnalysisPipeline) ────

        public uint[] GetTexturesForApiId(uint apiId)
        {
            using var conn = OpenConnection();
            return GetTexturesForApiId(conn, apiId);
        }

        public List<VertexBufferBinding> GetVertexBuffers(uint apiId)
        {
            using var conn = OpenConnection();
            return GetVertexBuffers(conn, apiId);
        }

        public IndexBufferBinding? GetIndexBuffer(uint apiId)
        {
            using var conn = OpenConnection();
            return GetIndexBuffer(conn, apiId);
        }

        public List<ShaderInfo> GetShadersForPipeline(uint pipelineId)
        {
            using var conn = OpenConnection();
            return GetShadersForPipeline(conn, pipelineId);
        }

        public List<TextureInfo> GetTextureDetails(uint[] ids)
        {
            using var conn = OpenConnection();
            return GetTextureDetails(conn, ids);
        }

        public List<RenderTargetInfo> GetRenderTargets(uint apiId)
        {
            using var conn = OpenConnection();
            return GetRenderTargets(conn, apiId, CaptureId);
        }

        public DescriptorBindingSummary GetBindingSummary(uint apiId)
        {
            using var conn = OpenConnection();
            return GetBindingSummary(conn, apiId);
        }

        public (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByResourceID(uint pipelineID)
        {
            using var conn = OpenConnection();
            return GetPipelineByResourceID(conn, pipelineID);
        }

        // ── DC list (from DatabaseQueryService) ──────────────────────────────

        /// <summary>
        /// Returns DrawCallApiID strings for the capture using 3-priority-level logic:
        /// DrawCallParameters > SCOPEDrawStages > pipeline count.
        /// </summary>
        public List<string> GetDrawCallIds(int? cmdBufferFilter = null)
        {
            var drawCallNumbers = new List<string>();
            try
            {
                using var conn = OpenConnection();

                if (TableExists(conn, "DrawCallParameters"))
                {
                    drawCallNumbers = GetDrawCallIdsFromParameters(conn, cmdBufferFilter);
                    if (drawCallNumbers.Count > 0) return drawCallNumbers;
                }

                if (TableExists(conn, "SCOPEDrawStages"))
                {
                    drawCallNumbers = GetDrawCallIdsFromSCOPE(conn);
                    if (drawCallNumbers.Count > 0) return drawCallNumbers;
                }

                drawCallNumbers = GenerateDrawCallIdsFromPipelines(conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetDrawCallIds failed: {ex.Message}");
            }
            return drawCallNumbers;
        }

        // ── Private core lookup ───────────────────────────────────────────────

        private DrawCallInfo? GetDrawCallInfoByApiId(SQLiteConnection conn, uint apiId, string label)
        {
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

            (uint pipelineID, uint layoutID, uint renderPass)? pl = null;
            if (TableExists(conn, "DrawCallBindings"))
                pl = GetPipelineFromBindings(conn, apiId);

            if (pl == null)
                pl = GetPipelineByGlobalIndex(conn, 0);

            uint pipeId    = pl?.pipelineID ?? 0;
            uint layoutId  = pl?.layoutID   ?? 0;
            uint renderPass = pl?.renderPass ?? 0;

            uint[]              textureIDs    = GetTexturesForApiId(conn, apiId);
            var                 renderTargets = GetRenderTargets(conn, apiId, CaptureId);
            var                 vertexBuffers = GetVertexBuffers(conn, apiId);
            IndexBufferBinding? indexBuffer   = GetIndexBuffer(conn, apiId);
            var                 bindingSummary= GetBindingSummary(conn, apiId);
            var                 shaders       = pipeId > 0
                                                ? GetShadersForPipeline(conn, pipeId)
                                                : new List<ShaderInfo>();
            var                 textures      = GetTextureDetails(conn, textureIDs);

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

        private DrawCallInfo? GetDrawCallInfoByEncodedId(SQLiteConnection conn, string drawCallNumber)
        {
            var id = ParseEncodedDrawCallNumber(drawCallNumber);
            if (id == null) return null;
            uint apiId = GetApiIdByEncodedId(conn, id.Value.submitIdx, id.Value.cmdBufferIdx, id.Value.drawcallIdx);
            if (apiId == 0) return null;
            return GetDrawCallInfoByApiId(conn, apiId, drawCallNumber);
        }

        // ── Per-DC resource queries ───────────────────────────────────────────

        private uint[] GetTexturesForApiId(SQLiteConnection conn, uint apiId)
        {
            if (TableExists(conn, "DrawCallBindings"))
            {
                string? ivCol = FindColumn(conn, "DrawCallBindings", "imageview", "ImageView");
                string? dcCol = FindColumn(conn, "DrawCallBindings", "DrawCallApiID", "ApiID", "drawcall");

                if (ivCol != null && dcCol != null)
                {
                    var imageViewIDs = new List<uint>();
                    using (var cmd = new SQLiteCommand(
                        $"SELECT DISTINCT [{ivCol}] FROM DrawCallBindings " +
                        $"WHERE [{dcCol}]={apiId} AND CaptureID={CaptureId} AND [{ivCol}]>0", conn))
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) imageViewIDs.Add(Convert.ToUInt32(r[0]));

                    if (imageViewIDs.Count > 0)
                    {
                        var ids = ResolveImageViewIds(conn, imageViewIDs);
                        Console.WriteLine($"  ? {ids.Length} textures from DrawCallBindings (apiID={apiId}, {imageViewIDs.Count} views, {imageViewIDs.Count - ids.Length} filtered)");
                        return ids;
                    }
                }
            }
            return Array.Empty<uint>();
        }

        private uint[] ResolveImageViewIds(SQLiteConnection conn, List<uint> ivIDs)
        {
            var result = new List<uint>();
            foreach (var ivid in ivIDs)
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        $"SELECT imageID FROM VulkanSnapshotImageViews " +
                        $"WHERE captureID={CaptureId} AND resourceID={ivid}", conn);
                    using var r = cmd.ExecuteReader();
                    if (r.Read() && !r.IsDBNull(0))
                        result.Add(Convert.ToUInt32(r[0]));
                }
                catch { }
            }
            return result.ToArray();
        }

        private List<RenderTargetInfo> GetRenderTargets(SQLiteConnection conn, uint apiId, uint captureId)
        {
            var list = new List<RenderTargetInfo>();
            if (!TableExists(conn, "DrawCallRenderTargets")) return list;
            try
            {
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
                    ResolveRenderTargetFormat(conn, captureId, rt);
                    list.Add(rt);
                }
            }
            catch (Exception ex) { Console.WriteLine($"  RenderTargets query failed: {ex.Message}"); }
            return list;
        }

        private static void ResolveRenderTargetFormat(SQLiteConnection conn, uint captureId, RenderTargetInfo rt)
        {
            try
            {
                uint imageId = 0;
                if (TableExists(conn, "VulkanSnapshotImageViews"))
                {
                    using var cmd = new SQLiteCommand(
                        $"SELECT imageID FROM VulkanSnapshotImageViews " +
                        $"WHERE captureID={captureId} AND resourceID={rt.AttachmentResourceID} LIMIT 1", conn);
                    using var r = cmd.ExecuteReader();
                    if (r.Read() && !r.IsDBNull(0)) imageId = Convert.ToUInt32(r[0]);
                }
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
            catch { }
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
                    list.Add(new VertexBufferBinding { Binding = Convert.ToUInt32(r[0]), BufferID = Convert.ToUInt32(r[1]) });
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

        private DescriptorBindingSummary GetBindingSummary(SQLiteConnection conn, uint apiId)
        {
            var result = new DescriptorBindingSummary();
            if (!TableExists(conn, "DrawCallBindings")) return result;
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT TexBufferView, BufferID, Range FROM DrawCallBindings " +
                    $"WHERE DrawCallApiID={apiId} AND CaptureID={CaptureId}", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    long texBufView = r[0] == DBNull.Value ? 0 : Convert.ToInt64(r[0]);
                    long bufferId   = r[1] == DBNull.Value ? 0 : Convert.ToInt64(r[1]);
                    long range      = r[2] == DBNull.Value ? 0 : Convert.ToInt64(r[2]);
                    if (texBufView != 0) result.TypedBufferViewCount++;
                    if (bufferId != 0 && range > 0 && range <= 256)
                    {
                        result.SmallBufferCount++;
                        if (range == 32) result.HasPerInstanceBuffer = true;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"  BindingSummary query failed: {ex.Message}"); }
            return result;
        }

        // ── Pipeline resolution ───────────────────────────────────────────────

        private (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineFromBindings(
            SQLiteConnection conn, uint apiId)
        {
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT PipelineID FROM DrawCallBindings " +
                    $"WHERE DrawCallApiID={apiId} AND CaptureID={CaptureId} AND PipelineID>0 LIMIT 1", conn);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    uint pid = Convert.ToUInt32(r[0]);
                    var resolved = GetPipelineByResourceID(conn, pid);
                    return resolved ?? (pid, 0u, 0u);
                }
            }
            catch { }
            return null;
        }

        private (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByGlobalIndex(
            SQLiteConnection conn, int index)
        {
            if (index < 0) return null;
            using var cmd = new SQLiteCommand(
                $"SELECT resourceID,layoutID,renderPass FROM VulkanSnapshotGraphicsPipelines " +
                $"WHERE captureID={CaptureId} ORDER BY resourceID LIMIT 1 OFFSET {index}", conn);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return (Convert.ToUInt32(r[0]), Convert.ToUInt32(r[1]), Convert.ToUInt32(r[2]));
            return null;
        }

        private (uint pipelineID, uint layoutID, uint renderPass)? GetPipelineByResourceID(
            SQLiteConnection conn, uint pipelineID)
        {
            using (var cmd = new SQLiteCommand(
                $"SELECT resourceID,layoutID,renderPass FROM VulkanSnapshotGraphicsPipelines " +
                $"WHERE captureID={CaptureId} AND resourceID={pipelineID}", conn))
            using (var r = cmd.ExecuteReader())
                if (r.Read()) return (Convert.ToUInt32(r[0]), Convert.ToUInt32(r[1]), Convert.ToUInt32(r[2]));

            if (TableExists(conn, "VulkanSnapshotComputePipelines"))
            {
                using var cmd2 = new SQLiteCommand(
                    $"SELECT resourceID,layoutID FROM VulkanSnapshotComputePipelines " +
                    $"WHERE captureID={CaptureId} AND resourceID={pipelineID}", conn);
                using var r2 = cmd2.ExecuteReader();
                if (r2.Read()) return (Convert.ToUInt32(r2[0]), Convert.ToUInt32(r2[1]), 0);
            }
            return null;
        }

        // ── Texture helpers ───────────────────────────────────────────────────

        private uint[] GetTextureIdsFallback(SQLiteConnection conn)
        {
            var list = new List<uint>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT iv.imageID " +
                    $"FROM VulkanSnapshotDescriptorSetBindings dsb " +
                    $"JOIN VulkanSnapshotImageViews iv ON dsb.imageViewID=iv.resourceID AND iv.captureID=dsb.captureID " +
                    $"WHERE dsb.captureID={CaptureId} AND dsb.imageViewID>0 ORDER BY iv.imageID LIMIT 50", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read()) list.Add(Convert.ToUInt32(r[0]));
            }
            catch (Exception ex) { Console.WriteLine($"  Texture fallback warning: {ex.Message}"); }
            return list.ToArray();
        }

        private List<TextureInfo> GetTextureDetails(SQLiteConnection conn, uint[] ids)
        {
            if (ids.Length == 0) return new List<TextureInfo>();
            string idList = string.Join(",", ids.Take(50));
            var list = new List<TextureInfo>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT resourceID,width,height,depth,format,layerCount,levelCount " +
                    $"FROM VulkanSnapshotTextures WHERE captureID={CaptureId} AND resourceID IN ({idList})", conn);
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

        private List<ShaderInfo> GetShadersForPipeline(SQLiteConnection conn, uint pipelineID)
        {
            var list = new List<ShaderInfo>();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT stageType,shaderModuleID,pName FROM VulkanSnapshotShaderStages " +
                    $"WHERE captureID={CaptureId} AND pipelineID={pipelineID} ORDER BY stageType", conn);
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

        // ── DC list helpers ───────────────────────────────────────────────────

        private List<string> GetDrawCallIdsFromParameters(SQLiteConnection conn, int? cmdBufferFilter)
        {
            var ids = new List<string>();
            bool hasCaptureIdCol = false;
            bool hasCmdBufferCol = false;
            using (var probe = new SQLiteCommand("PRAGMA table_info(DrawCallParameters)", conn))
            using (var pr = probe.ExecuteReader())
            {
                while (pr.Read())
                {
                    string col = pr["name"].ToString() ?? "";
                    if (col == "CaptureID")    hasCaptureIdCol = true;
                    if (col == "CmdBufferIdx") hasCmdBufferCol = true;
                }
            }

            int resolvedCmdBuf = -1;
            if (cmdBufferFilter == 0 && hasCmdBufferCol)
            {
                string captureWhere = hasCaptureIdCol ? $"WHERE CaptureID={CaptureId}" : "";
                string autoSql = $"SELECT CmdBufferIdx FROM DrawCallParameters {captureWhere} " +
                                 "GROUP BY CmdBufferIdx ORDER BY COUNT(*) DESC LIMIT 1";
                using var ac = new SQLiteCommand(autoSql, conn);
                var scalar = ac.ExecuteScalar();
                if (scalar != null && scalar != DBNull.Value)
                    resolvedCmdBuf = Convert.ToInt32(scalar);
            }
            else if (cmdBufferFilter >= 1)
            {
                resolvedCmdBuf = cmdBufferFilter.Value;
            }

            var conditions = new List<string>();
            if (hasCaptureIdCol) conditions.Add($"CaptureID={CaptureId}");
            if (resolvedCmdBuf >= 0 && hasCmdBufferCol) conditions.Add($"CmdBufferIdx={resolvedCmdBuf}");

            string where = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
            string sql   = $"SELECT DrawCallApiID FROM DrawCallParameters{where} ORDER BY rowid";

            using var cmd = new SQLiteCommand(sql, conn);
            using var r   = cmd.ExecuteReader();
            while (r.Read()) ids.Add(r[0].ToString() ?? "");
            return ids;
        }

        private List<string> GetDrawCallIdsFromSCOPE(SQLiteConnection conn)
        {
            var drawCallNumbers = new List<string>();
            using var cmd = new SQLiteCommand(
                $"SELECT DISTINCT drawCallID FROM SCOPEDrawStages " +
                $"WHERE captureID = {CaptureId} ORDER BY drawCallID", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                ulong drawCallID = Convert.ToUInt64(r[0]);
                string s = DecodeDrawCallId(drawCallID);
                if (!string.IsNullOrEmpty(s)) drawCallNumbers.Add(s);
            }
            return drawCallNumbers;
        }

        private List<string> GenerateDrawCallIdsFromPipelines(SQLiteConnection conn)
        {
            var drawCallNumbers = new List<string>();
            using var cmd = new SQLiteCommand(
                $"SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines WHERE captureID = {CaptureId}", conn);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            for (int i = 1; i <= Math.Min(count, 100); i++)
                drawCallNumbers.Add(i.ToString());
            return drawCallNumbers;
        }

        private static string DecodeDrawCallId(ulong drawCallID)
        {
            uint submitIdx    = (uint)(drawCallID >> 48) & 0xFFFF;
            uint cmdBufferIdx = (uint)(drawCallID >> 32) & 0xFFFF;
            uint drawcallIdx  = (uint)(drawCallID & 0xFFFFFFFF);
            if (drawcallIdx == 0) return "";
            return cmdBufferIdx == 0 ? $"{submitIdx}.{drawcallIdx}" : $"{submitIdx}.{cmdBufferIdx}.{drawcallIdx}";
        }

        // ── Encoded ID helpers ────────────────────────────────────────────────

        private static uint GetApiIdByEncodedId(SQLiteConnection conn,
            uint submitIdx, uint cmdBufferIdx, uint drawcallIdx)
        {
            try
            {
                bool hasNewCols = false;
                using (var probe = new SQLiteCommand("PRAGMA table_info(DrawCallParameters)", conn))
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
    }
}
