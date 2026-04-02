using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Shader 提取工具
    /// 根据 DrawCall ID 或 Pipeline ID 提取绑定的 VS/FS 等 shader
    /// 输出: .spv (SPIR-V 二进制) + .glsl (反汇编/GLSL 文本)
    /// </summary>
    public class ShaderExtractor
    {
        private readonly string _databasePath;
        private readonly int _captureId;

        /// <summary>
        /// Path to spirv-cross.exe. Set before calling Extract* methods to enable GLSL/HLSL decompilation.
        /// </summary>
        public string? SpirvCrossPath { get; set; }

        /// <summary>
        /// Controls which text formats are produced via spirv-cross.
        /// Accepted values: "hlsl" (default), "glsl", "both"
        /// </summary>
        public string ShaderOutputFormat { get; set; } = "hlsl";

        private static readonly Dictionary<uint, string> StageNames = new Dictionary<uint, string>
        {
            { 0x00000001, "vert" },
            { 0x00000002, "tesc" },
            { 0x00000004, "tese" },
            { 0x00000008, "geom" },
            { 0x00000010, "frag" },
            { 0x00000020, "comp" },
            { 0x00000100, "rgen" },  // ray generation
            { 0x00000200, "rint" },  // intersection
            { 0x00000400, "rahit" }, // any-hit
            { 0x00000800, "rchit" }, // closest-hit
            { 0x00001000, "rmiss" }, // miss
        };

        public ShaderExtractor(string databasePath, int captureId = 3)
        {
            _databasePath = databasePath;
            _captureId = captureId;
        }

        /// <summary>
        /// 根据 DrawCall ID (如 "1.0.10" 或 "10") 提取 shader
        /// </summary>
        public bool ExtractShadersForDrawCall(string drawCallId, string outputDir)
        {
            Console.WriteLine($"\n=== Extracting Shaders for DrawCall: {drawCallId} ===");

            using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;");
            conn.Open();

            // 1. 解析 drawcall ID 找到 pipelineID
            uint? pipelineId = ResolvePipelineFromDrawCall(conn, drawCallId);
            if (pipelineId == null)
            {
                Console.WriteLine($"  ✗ Could not find pipeline for drawcall '{drawCallId}'");
                return false;
            }

            Console.WriteLine($"  Pipeline ID: {pipelineId}");
            return ExtractShadersForPipeline(conn, pipelineId.Value, outputDir);
        }

        /// <summary>
        /// 直接根据 Pipeline ID 提取 shader
        /// </summary>
        public bool ExtractShadersForPipeline(uint pipelineId, string outputDir)
        {
            Console.WriteLine($"\n=== Extracting Shaders for Pipeline: {pipelineId} ===");

            using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;");
            conn.Open();

            return ExtractShadersForPipeline(conn, pipelineId, outputDir);
        }

        private bool ExtractShadersForPipeline(SQLiteConnection conn, uint pipelineId, string outputDir)
        {
            // 2. 查询该 pipeline 绑定的所有 shader stage
            var stages = GetShaderStages(conn, pipelineId);
            if (stages.Count == 0)
            {
                Console.WriteLine($"  ✗ No shader stages found for pipeline {pipelineId}");
                return false;
            }

            Console.WriteLine($"  Found {stages.Count} shader stage(s):");

            Directory.CreateDirectory(outputDir);
            bool allOk = true;

            foreach (var stage in stages)
            {
                string stageName = GetStageName(stage.StageType);
                Console.WriteLine($"\n  [{stageName.ToUpper()}] ModuleID={stage.ShaderModuleID}, Entry='{stage.EntryPoint}'");

                string baseName = $"pipeline_{pipelineId}_{stageName}";
                bool ok = true;

                // 3a. 提取 SPIR-V 二进制
                string spvPath = Path.Combine(outputDir, $"{baseName}.spv");
                bool spvOk = ExtractSpirv(conn, stage.ShaderModuleID, spvPath);
                ok &= spvOk;

                // 3b. 如果有 spirv-cross，用它反编译 GLSL 和 HLSL
                if (spvOk && File.Exists(spvPath) && !string.IsNullOrWhiteSpace(SpirvCrossPath))
                {
                    DecompileSpirv(spvPath, outputDir, baseName, stage.StageType);
                }

                // 3c. 尝试从数据库读取内置反汇编文本（可能为空）
                ExtractDisasm(conn, pipelineId, stage.StageType, stage.ShaderModuleID, stage.ShaderIndex,
                              Path.Combine(outputDir, $"{baseName}.disasm"));

                if (!ok) allOk = false;
            }

            return allOk;
        }

        // ─────────────────────────────────────────────────────────────
        // SPIR-V 反编译 (spirv-cross)
        // ─────────────────────────────────────────────────────────────

        private void DecompileSpirv(string spvPath, string outputDir, string baseName, uint stageType)
        {
            bool wantGlsl = ShaderOutputFormat.Equals("glsl", StringComparison.OrdinalIgnoreCase)
                         || ShaderOutputFormat.Equals("both", StringComparison.OrdinalIgnoreCase);
            bool wantHlsl = ShaderOutputFormat.Equals("hlsl", StringComparison.OrdinalIgnoreCase)
                         || ShaderOutputFormat.Equals("both", StringComparison.OrdinalIgnoreCase)
                         || (!wantGlsl); // default fallback to hlsl for unrecognised values

            if (wantGlsl)
                RunSpirvCross(spvPath, Path.Combine(outputDir, $"{baseName}.glsl"),
                    extraArgs: null, label: "GLSL");

            if (wantHlsl)
                RunSpirvCross(spvPath, Path.Combine(outputDir, $"{baseName}.hlsl"),
                    extraArgs: "--hlsl --shader-model 50", label: "HLSL");
        }

        private void RunSpirvCross(string spvPath, string outputPath, string? extraArgs, string label)
        {
            try
            {
                string args = $"\"{spvPath}\" --output \"{outputPath}\"";
                if (!string.IsNullOrEmpty(extraArgs))
                    args += " " + extraArgs;

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = SpirvCrossPath,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using var proc = System.Diagnostics.Process.Start(psi);
                if (proc == null)
                {
                    Console.WriteLine($"    ✗ {label}: Failed to start spirv-cross");
                    return;
                }

                string stderr = proc.StandardError.ReadToEnd();
                proc.WaitForExit(10000); // 10s timeout

                if (proc.ExitCode == 0 && File.Exists(outputPath))
                {
                    long size = new FileInfo(outputPath).Length;
                    Console.WriteLine($"    ✓ {label}: {outputPath} ({size:N0} chars)");
                }
                else
                {
                    string msg = stderr.Trim().Replace("\n", " ");
                    Console.WriteLine($"    ✗ {label}: spirv-cross failed (exit {proc.ExitCode}){(msg.Length > 0 ? ": " + msg : "")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ✗ {label}: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────
        // DrawCall → Pipeline 解析
        // ─────────────────────────────────────────────────────────────

        private uint? ResolvePipelineFromDrawCall(SQLiteConnection conn, string drawCallId)
        {
            // 支持格式: "1.0.10" / "1.10" / "10"
            var parsed = ParseDrawCallId(drawCallId);
            if (parsed == null) return null;

            var (submitIdx, cmdBufIdx, drawIdx) = parsed.Value;

            // 方法1: 尝试从 SCOPEDrawStages 按序号定位
            try
            {
                string query = $@"
                    SELECT DISTINCT pipelineID
                    FROM SCOPEDrawStages
                    WHERE captureID = {_captureId}
                    ORDER BY drawCallID";

                using var cmd = new SQLiteCommand(query, conn);
                using var r = cmd.ExecuteReader();
                int idx = 0;
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
            catch { /* 表不存在，回退 */ }

            // 方法2: 按 drawcallIdx 偏移从 Graphics/Compute Pipelines 顺序查找
            try
            {
                int offset = (int)drawIdx - 1;
                if (offset < 0) offset = 0;

                // Try graphics first
                string queryG = $@"
                    SELECT resourceID
                    FROM VulkanSnapshotGraphicsPipelines
                    WHERE captureID = {_captureId}
                    ORDER BY resourceID
                    LIMIT 1 OFFSET {offset}";
                using (var cmd = new SQLiteCommand(queryG, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToUInt32(result);
                }

                // Try compute as fallback
                string queryC = $@"
                    SELECT resourceID
                    FROM VulkanSnapshotComputePipelines
                    WHERE captureID = {_captureId}
                    ORDER BY resourceID
                    LIMIT 1 OFFSET {offset}";
                using (var cmd2 = new SQLiteCommand(queryC, conn))
                {
                    var result2 = cmd2.ExecuteScalar();
                    if (result2 != null) return Convert.ToUInt32(result2);
                }
            }
            catch { }

            return null;
        }

        private bool ValidatePipelineId(SQLiteConnection conn, uint pid)
        {
            // Check graphics pipeline first
            string q = $"SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines WHERE captureID={_captureId} AND resourceID={pid}";
            using (var cmd = new SQLiteCommand(q, conn))
                if (Convert.ToInt64(cmd.ExecuteScalar()) > 0) return true;

            // Check compute pipeline
            try
            {
                string q2 = $"SELECT COUNT(*) FROM VulkanSnapshotComputePipelines WHERE captureID={_captureId} AND resourceID={pid}";
                using var cmd2 = new SQLiteCommand(q2, conn);
                return Convert.ToInt64(cmd2.ExecuteScalar()) > 0;
            }
            catch { return false; }
        }

        private (uint, uint, uint)? ParseDrawCallId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            if (id.Contains("."))
            {
                var parts = id.Split('.');
                if (parts.Length == 3 &&
                    uint.TryParse(parts[0], out uint s) &&
                    uint.TryParse(parts[1], out uint c) &&
                    uint.TryParse(parts[2], out uint d))
                    return (s, c, d);

                if (parts.Length == 2 &&
                    uint.TryParse(parts[0], out s) &&
                    uint.TryParse(parts[1], out d))
                    return (s, 0, d);
            }
            else if (uint.TryParse(id, out uint d))
            {
                return (1, 0, d);
            }

            return null;
        }

        // ─────────────────────────────────────────────────────────────
        // Shader Stage 查询
        // ─────────────────────────────────────────────────────────────

        private class ShaderStageInfo
        {
            public uint StageType { get; }
            public ulong ShaderModuleID { get; }
            public string EntryPoint { get; }
            public uint ShaderIndex { get; }
            public ShaderStageInfo(uint stageType, ulong shaderModuleId, string entryPoint, uint shaderIndex)
            {
                StageType = stageType; ShaderModuleID = shaderModuleId; EntryPoint = entryPoint; ShaderIndex = shaderIndex;
            }
        }

        private List<ShaderStageInfo> GetShaderStages(SQLiteConnection conn, uint pipelineId)
        {
            var list = new List<ShaderStageInfo>();

            // If _captureId has no stages for this pipeline, find whichever captureID does
            // (replay may have overwritten DB with a later captureID)
            int cid = _captureId;
            try
            {
                using var probe = new SQLiteCommand(
                    $"SELECT COUNT(*) FROM VulkanSnapshotShaderStages WHERE captureID={_captureId} AND pipelineID={pipelineId}", conn);
                if (Convert.ToInt64(probe.ExecuteScalar()) == 0)
                {
                    using var any = new SQLiteCommand(
                        $"SELECT captureID FROM VulkanSnapshotShaderStages WHERE pipelineID={pipelineId} LIMIT 1", conn);
                    var found = any.ExecuteScalar();
                    if (found != null) cid = Convert.ToInt32(found);
                }
            }
            catch { }

            string query = $@"
                SELECT stageType, shaderModuleID, pName, COALESCE(shaderIndex, 0)
                FROM VulkanSnapshotShaderStages
                WHERE captureID = {cid} AND pipelineID = {pipelineId}
                ORDER BY stageType";

            using var cmd = new SQLiteCommand(query, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new ShaderStageInfo(
                    Convert.ToUInt32(r[0]),
                    Convert.ToUInt64(r[1]),
                    r[2]?.ToString() ?? "main",
                    Convert.ToUInt32(r[3])));
            }
            return list;
        }

        // ─────────────────────────────────────────────────────────────
        // SPIR-V 二进制提取
        // ─────────────────────────────────────────────────────────────

        private bool ExtractSpirv(SQLiteConnection conn, ulong shaderModuleId, string outputPath)
        {
            try
            {
                // If _captureId has no data for this module, find whichever captureID does
                int cid = _captureId;
                try
                {
                    using var probe = new SQLiteCommand(
                        $"SELECT COUNT(*) FROM VulkanSnapshotByteBuffers WHERE captureID={_captureId} AND resourceID={shaderModuleId}", conn);
                    if (Convert.ToInt64(probe.ExecuteScalar()) == 0)
                    {
                        using var any = new SQLiteCommand(
                            $"SELECT captureID FROM VulkanSnapshotByteBuffers WHERE resourceID={shaderModuleId} LIMIT 1", conn);
                        var found = any.ExecuteScalar();
                        if (found != null) cid = Convert.ToInt32(found);
                    }
                }
                catch { }

                string query = $@"
                    SELECT data FROM VulkanSnapshotByteBuffers
                    WHERE captureID = {cid} AND resourceID = {shaderModuleId}
                    LIMIT 1";

                using var cmd = new SQLiteCommand(query, conn);
                using var r = cmd.ExecuteReader();
                if (!r.Read())
                {
                    Console.WriteLine($"    ⚠ No SPIR-V data in ByteBuffers for module {shaderModuleId}");
                    return false;
                }

                long size = r.GetBytes(0, 0, null, 0, 0);
                if (size == 0)
                {
                    Console.WriteLine($"    ⚠ Empty SPIR-V data for module {shaderModuleId}");
                    return false;
                }

                byte[] spirv = new byte[size];
                r.GetBytes(0, 0, spirv, 0, (int)size);
                File.WriteAllBytes(outputPath, spirv);
                Console.WriteLine($"    ✓ SPIR-V: {outputPath} ({size:N0} bytes)");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ✗ SPIR-V extraction failed: {ex.Message}");
                return false;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // 反汇编/GLSL 提取
        // ─────────────────────────────────────────────────────────────

        private bool ExtractDisasm(SQLiteConnection conn, uint pipelineId, uint stageType, ulong shaderModuleId, uint shaderIndex, string outputPath)
        {
            try
            {
                // VulkanSnapshotShaderData: captureID, pipelineID, shaderStage, shaderIndex, shaderModuleID, shaderDisasm
                string query = $@"
                    SELECT shaderDisasm
                    FROM VulkanSnapshotShaderData
                    WHERE captureID = {_captureId}
                      AND pipelineID = {pipelineId}
                      AND shaderStage = {stageType}
                    LIMIT 1";

                using var cmd = new SQLiteCommand(query, conn);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value || string.IsNullOrEmpty(result.ToString()))
                {
                    Console.WriteLine($"    ⚠ No disassembly text available for stage {GetStageName(stageType).ToUpper()}");
                    return true; // Not fatal – SPIR-V might still be useful
                }

                File.WriteAllText(outputPath, result.ToString(), System.Text.Encoding.UTF8);
                Console.WriteLine($"    ✓ GLSL/Disasm: {outputPath} ({result.ToString()!.Length:N0} chars)");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ✗ Disasm extraction failed: {ex.Message}");
                return true; // Not fatal
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────

        private static string GetStageName(uint stageType)
        {
            return StageNames.TryGetValue(stageType, out var name) ? name : $"stage{stageType:x}";
        }

        /// <summary>
        /// 列出所有可供参考的 drawcall 和对应 pipeline 信息
        /// </summary>
        public void ListPipelines(int maxRows = 30)
        {
            Console.WriteLine($"\n=== Pipelines in capture (captureID={_captureId}) ===");
            Console.WriteLine($"{"PipelineID",-14} {"LayoutID",-12} {"RenderPass",-12}");
            Console.WriteLine(new string('-', 42));

            using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;");
            conn.Open();
            using var cmd = new SQLiteCommand(
                $"SELECT resourceID, layoutID, renderPass FROM VulkanSnapshotGraphicsPipelines WHERE captureID={_captureId} ORDER BY resourceID LIMIT {maxRows}",
                conn);
            using var r = cmd.ExecuteReader();
            int idx = 1;
            while (r.Read())
            {
                Console.WriteLine($"{r.GetInt64(0),-14} {r.GetInt64(1),-12} {r.GetInt64(2),-12}  DrawCall ~{idx}");
                idx++;
            }

            Console.WriteLine($"\nTip: use -drawcall-id <N> or -pipeline-id <ID> to extract shaders.");
        }
    }
}
