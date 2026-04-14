using System;
using System.Collections.Generic;
using System.IO;
using SnapdragonProfilerCLI.Data;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Shader 提取工具
    /// 根据 DrawCall ID 或 Pipeline ID 提取绑定的 VS/FS 等 shader
    /// 输出: .spv (SPIR-V 二进制) + .glsl (反汇编/GLSL 文本)
    /// </summary>
    public class ShaderExtractor
    {
        private readonly SdpDatabase _db;

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
            { 0x00000100, "rgen" },
            { 0x00000200, "rint" },
            { 0x00000400, "rahit" },
            { 0x00000800, "rchit" },
            { 0x00001000, "rmiss" },
        };

        /// <summary>Primary constructor — inject SdpDatabase instance.</summary>
        public ShaderExtractor(SdpDatabase db)
        {
            _db = db;
        }

        /// <summary>Backward-compatible constructor (creates its own SdpDatabase).</summary>
        public ShaderExtractor(string databasePath, int captureId = 3)
            : this(new SdpDatabase(databasePath, (uint)captureId)) { }

        /// <summary>
        /// 根据 DrawCall ID (如 "1.0.10" 或 "10") 提取 shader
        /// </summary>
        public bool ExtractShadersForDrawCall(string drawCallId, string outputDir)
        {
            AppLogger.Info("Shader", $"=== Extracting Shaders for DrawCall: {drawCallId} ===");

            uint? pipelineId = _db.ResolvePipelineFromDrawCall(drawCallId);
            if (pipelineId == null)
            {
                AppLogger.Warn("Shader", $"Could not find pipeline for drawcall '{drawCallId}'");
                return false;
            }

            AppLogger.Info("Shader", $"Pipeline ID: {pipelineId}");
            return ExtractShadersForPipeline(pipelineId.Value, outputDir);
        }

        /// <summary>
        /// 直接根据 Pipeline ID 提取 shader
        /// </summary>
        public bool ExtractShadersForPipeline(uint pipelineId, string outputDir)
        {
            AppLogger.Info("Shader", $"=== Extracting Shaders for Pipeline: {pipelineId} ===");
            return ExtractShadersForPipelineInternal(pipelineId, outputDir);
        }

        private bool ExtractShadersForPipelineInternal(uint pipelineId, string outputDir)
        {
            var stages = _db.GetShaderStages(pipelineId);
            if (stages.Count == 0)
            {
                AppLogger.Warn("Shader", $"No shader stages found for pipeline {pipelineId}");
                return false;
            }

            AppLogger.Info("Shader", $"Found {stages.Count} shader stage(s):");

            Directory.CreateDirectory(outputDir);
            bool allOk = true;

            foreach (var stage in stages)
            {
                string stageName = GetStageName(stage.StageType);
                AppLogger.Debug("Shader", $"[{stageName.ToUpper()}] ModuleID={stage.ShaderModuleID}, Entry='{stage.EntryPoint}'");

                string baseName = $"pipeline_{pipelineId}_{stageName}";
                bool ok = true;

                string spvPath = Path.Combine(outputDir, $"{baseName}.spv");
                bool spvOk = WriteSpirv(stage.ShaderModuleID, spvPath);
                ok &= spvOk;

                if (spvOk && File.Exists(spvPath) && !string.IsNullOrWhiteSpace(SpirvCrossPath))
                    DecompileSpirv(spvPath, outputDir, baseName, stage.StageType);

                WriteDisasm(pipelineId, stage.StageType,
                    Path.Combine(outputDir, $"{baseName}.disasm"));

                if (!ok) allOk = false;
            }

            return allOk;
        }

        // ─────────────────────────────────────────────────────────────
        // Write helpers (delegate SQL to _db)
        // ─────────────────────────────────────────────────────────────

        private bool WriteSpirv(ulong shaderModuleId, string outputPath)
        {
            byte[]? spirv = _db.ReadSpirvBytes(shaderModuleId);
            if (spirv == null || spirv.Length == 0)
            {
                AppLogger.Warn("Shader", $"No SPIR-V data in ByteBuffers for module {shaderModuleId}");
                return false;
            }
            File.WriteAllBytes(outputPath, spirv);
            AppLogger.Info("Shader", $"SPIR-V: {outputPath} ({spirv.Length:N0} bytes)");
            return true;
        }

        private bool WriteDisasm(uint pipelineId, uint stageType, string outputPath)
        {
            string? text = _db.ReadShaderDisasm(pipelineId, stageType);
            if (text == null)
            {
                AppLogger.Warn("Shader", $"No disassembly text available for stage {GetStageName(stageType).ToUpper()}");
                return true; // Not fatal
            }
            File.WriteAllText(outputPath, text, System.Text.Encoding.UTF8);
            AppLogger.Info("Shader", $"GLSL/Disasm: {outputPath} ({text.Length:N0} chars)");
            return true;
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
                    AppLogger.Warn("Shader", $"{label}: Failed to start spirv-cross");
                    return;
                }

                string stderr = proc.StandardError.ReadToEnd();
                proc.WaitForExit(10000); // 10s timeout

                if (proc.ExitCode == 0 && File.Exists(outputPath))
                {
                    long size = new FileInfo(outputPath).Length;
                    AppLogger.Info("Shader", $"{label}: {outputPath} ({size:N0} chars)");
                }
                else
                {
                    string msg = stderr.Trim().Replace("\n", " ");
                    AppLogger.Warn("Shader", $"{label}: spirv-cross failed (exit {proc.ExitCode}){(msg.Length > 0 ? ": " + msg : "")}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Shader", $"{label}: {ex.Message}");
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
            AppLogger.Info("Shader", $"=== Pipelines in capture (captureID={_db.CaptureId}) ===");
            AppLogger.Info("Shader", $"{"PipelineID",-14} {"LayoutID",-12} {"RenderPass",-12}");
            AppLogger.Info("Shader", new string('-', 42));

            var pipelines = _db.ListPipelines(maxRows);
            int idx = 1;
            foreach (var (resourceID, layoutID, renderPass) in pipelines)
            {
                AppLogger.Info("Shader", $"{resourceID,-14} {layoutID,-12} {renderPass,-12}  DrawCall ~{idx}");
                idx++;
            }

            AppLogger.Info("Shader", "Tip: use -drawcall-id <N> or -pipeline-id <ID> to extract shaders.");
        }
    }
}
