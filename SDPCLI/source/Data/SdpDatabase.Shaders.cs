using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SnapdragonProfilerCLI.Data
{
    /// <summary>
    /// Shader stage record returned by GetShaderStages().
    /// Named differently from Models.ShaderInfo to avoid ambiguity.
    /// </summary>
    public class ShaderStageRecord
    {
        public uint   StageType      { get; set; }
        public ulong  ShaderModuleID { get; set; }
        public string EntryPoint     { get; set; } = "main";
        public uint   ShaderIndex    { get; set; }
    }

    public sealed partial class SdpDatabase
    {
        /// <summary>
        /// Returns shader stage records for a pipeline.
        /// Falls back to any captureID if the current one has no entries
        /// (replay may have overwritten the DB with a later capture).
        /// </summary>
        public List<ShaderStageRecord> GetShaderStages(uint pipelineId)
        {
            var list = new List<ShaderStageRecord>();
            using var conn = OpenConnection();

            int cid = (int)CaptureId;
            try
            {
                using var probe = new SQLiteCommand(
                    $"SELECT COUNT(*) FROM VulkanSnapshotShaderStages WHERE captureID={CaptureId} AND pipelineID={pipelineId}", conn);
                if (Convert.ToInt64(probe.ExecuteScalar()) == 0)
                {
                    using var any = new SQLiteCommand(
                        $"SELECT captureID FROM VulkanSnapshotShaderStages WHERE pipelineID={pipelineId} LIMIT 1", conn);
                    var found = any.ExecuteScalar();
                    if (found != null) cid = Convert.ToInt32(found);
                }
            }
            catch { }

            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT stageType, shaderModuleID, pName, COALESCE(shaderIndex, 0) " +
                    $"FROM VulkanSnapshotShaderStages " +
                    $"WHERE captureID = {cid} AND pipelineID = {pipelineId} " +
                    $"ORDER BY stageType", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    list.Add(new ShaderStageRecord
                    {
                        StageType      = Convert.ToUInt32(r[0]),
                        ShaderModuleID = Convert.ToUInt64(r[1]),
                        EntryPoint     = r[2]?.ToString() ?? "main",
                        ShaderIndex    = Convert.ToUInt32(r[3])
                    });
                }
            }
            catch { }

            return list;
        }

        /// <summary>
        /// Returns raw SPIR-V bytes for a shader module.
        /// Returns null if not found or empty.
        /// Falls back to any captureID if current one has no entry.
        /// </summary>
        public byte[]? ReadSpirvBytes(ulong shaderModuleId)
        {
            using var conn = OpenConnection();

            int cid = (int)CaptureId;
            try
            {
                using var probe = new SQLiteCommand(
                    $"SELECT COUNT(*) FROM VulkanSnapshotByteBuffers WHERE captureID={CaptureId} AND resourceID={shaderModuleId}", conn);
                if (Convert.ToInt64(probe.ExecuteScalar()) == 0)
                {
                    using var any = new SQLiteCommand(
                        $"SELECT captureID FROM VulkanSnapshotByteBuffers WHERE resourceID={shaderModuleId} LIMIT 1", conn);
                    var found = any.ExecuteScalar();
                    if (found != null) cid = Convert.ToInt32(found);
                }
            }
            catch { }

            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT data FROM VulkanSnapshotByteBuffers " +
                    $"WHERE captureID = {cid} AND resourceID = {shaderModuleId} LIMIT 1", conn);
                using var r = cmd.ExecuteReader();
                if (!r.Read()) return null;

                long size = r.GetBytes(0, 0, null, 0, 0);
                if (size == 0) return null;

                byte[] spirv = new byte[size];
                r.GetBytes(0, 0, spirv, 0, (int)size);
                return spirv;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ✗ ReadSpirvBytes({shaderModuleId}) failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Returns disassembly text for a pipeline + stageType.
        /// Returns null if not available (non-fatal — SPIR-V still works).
        /// </summary>
        public string? ReadShaderDisasm(uint pipelineId, uint stageType)
        {
            using var conn = OpenConnection();
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT shaderDisasm FROM VulkanSnapshotShaderData " +
                    $"WHERE captureID = {CaptureId} AND pipelineID = {pipelineId} " +
                    $"AND shaderStage = {stageType} LIMIT 1", conn);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return null;
                string? text = result.ToString();
                return string.IsNullOrEmpty(text) ? null : text;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves a pipeline ID from an encoded or plain draw call ID string.
        /// Supports "1.0.10", "1.10", "10" formats.
        /// </summary>
        public uint? ResolvePipelineFromDrawCall(string drawCallId)
        {
            if (string.IsNullOrWhiteSpace(drawCallId)) return null;
            using var conn = OpenConnection();

            var parsed = ParseDrawCallIdTuple(drawCallId);
            if (parsed == null) return null;
            var (submitIdx, cmdBufIdx, drawIdx) = parsed.Value;

            // Method 1: SCOPEDrawStages positional lookup
            try
            {
                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT pipelineID FROM SCOPEDrawStages " +
                    $"WHERE captureID = {CaptureId} ORDER BY drawCallID", conn);
                using var r = cmd.ExecuteReader();
                int idx    = 0;
                int target = (int)drawIdx - 1;
                while (r.Read())
                {
                    if (idx == target)
                    {
                        uint pid = Convert.ToUInt32(r[0]);
                        return ValidatePipelineId(conn, pid) ? pid : null;
                    }
                    idx++;
                }
            }
            catch { }

            // Method 2: offset into VulkanSnapshotGraphicsPipelines
            try
            {
                int offset = (int)drawIdx - 1;
                if (offset < 0) offset = 0;
                using (var cmd = new SQLiteCommand(
                    $"SELECT resourceID FROM VulkanSnapshotGraphicsPipelines " +
                    $"WHERE captureID = {CaptureId} ORDER BY resourceID LIMIT 1 OFFSET {offset}", conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToUInt32(result);
                }
                using (var cmd2 = new SQLiteCommand(
                    $"SELECT resourceID FROM VulkanSnapshotComputePipelines " +
                    $"WHERE captureID = {CaptureId} ORDER BY resourceID LIMIT 1 OFFSET {offset}", conn))
                {
                    var result2 = cmd2.ExecuteScalar();
                    if (result2 != null) return Convert.ToUInt32(result2);
                }
            }
            catch { }

            return null;
        }

        public List<(uint resourceID, uint layoutID, uint renderPass)> ListPipelines(int maxRows = 30)
        {
            var list = new List<(uint, uint, uint)>();
            using var conn = OpenConnection();
            using var cmd = new SQLiteCommand(
                $"SELECT resourceID, layoutID, renderPass FROM VulkanSnapshotGraphicsPipelines " +
                $"WHERE captureID={CaptureId} ORDER BY resourceID LIMIT {maxRows}", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add((Convert.ToUInt32(r[0]), Convert.ToUInt32(r[1]), Convert.ToUInt32(r[2])));
            return list;
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private bool ValidatePipelineId(SQLiteConnection conn, uint pid)
        {
            using (var cmd = new SQLiteCommand(
                $"SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines WHERE captureID={CaptureId} AND resourceID={pid}", conn))
                if (Convert.ToInt64(cmd.ExecuteScalar()) > 0) return true;
            try
            {
                using var cmd2 = new SQLiteCommand(
                    $"SELECT COUNT(*) FROM VulkanSnapshotComputePipelines WHERE captureID={CaptureId} AND resourceID={pid}", conn);
                return Convert.ToInt64(cmd2.ExecuteScalar()) > 0;
            }
            catch { return false; }
        }

        private static (uint, uint, uint)? ParseDrawCallIdTuple(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            if (id.Contains("."))
            {
                var parts = id.Split('.');
                if (parts.Length == 3 && uint.TryParse(parts[0], out uint s) &&
                    uint.TryParse(parts[1], out uint c) && uint.TryParse(parts[2], out uint d))
                    return (s, c, d);
                if (parts.Length == 2 && uint.TryParse(parts[0], out s) && uint.TryParse(parts[1], out d))
                    return (s, 0, d);
            }
            else if (uint.TryParse(id, out uint d))
                return (1, 0, d);
            return null;
        }
    }
}
