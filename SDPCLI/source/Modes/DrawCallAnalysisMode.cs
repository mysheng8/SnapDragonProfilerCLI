using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using SnapdragonProfilerCLI.Data;
using SnapdragonProfilerCLI.Services.Analysis;
using SnapdragonProfilerCLI.Tools;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// 分析指定序号的 DrawCall 的 Shader 和 Texture。
    ///
    /// 用法:
    ///   -mode dcanalysis -sdp "file.sdp" [-capture-id 3] [-output "dcN_output/"]
    ///
    /// 序号在 config.ini 中通过 DCDrawCallIndex 配置（默认 100）。
    ///
    /// 流程:
    ///   1. 从 .sdp ZIP 中提取 sdp.db
    ///   2. 枚举所有 DrawCall，取第 DCDrawCallIndex 个（索引 N-1）
    ///   3. 调用 DrawCallQueryService.GetDrawCallInfo 获取完整资源信息
    ///   4. 生成 Markdown 诊断报告（写入文件 + 打印摘要到控制台）
    ///   5. 用 ShaderExtractor.ExtractShadersForPipeline 提取 Shader（直接用已解析的 PipelineID）
    ///   6. 用 TextureExtractor 提取 Texture（上限 DCTextureLimit 张）
    /// </summary>
    public class DrawCallAnalysisMode : IMode
    {
        private readonly string? _sdpPath;
        private readonly string? _outputDir;
        private readonly int     _captureId;
        private readonly Config  _config;

        public string Name        => "DrawCallAnalysis";
        public string Description => "分析指定序号的 DrawCall 的 Shader 和 Texture";

        public DrawCallAnalysisMode(string? sdpPath, string? outputDir, int captureId, Config config)
        {
            _sdpPath   = sdpPath;
            _outputDir = outputDir;
            _captureId = captureId;
            _config    = config;
        }

        public void Run()
        {
            // DCDrawCallIndex 支持两种格式：
            //   "31"      → 列表里第 31 条（1-based 序号）
            //   "1.1.31"  → 直接按编码 ID 查找（对应 GUI 里 submit.cmdBuf.drawcall 格式）
            string dcIndexRaw = _config.Get("DCDrawCallIndex", "100");
            bool   isEncoded  = dcIndexRaw.Contains('.');
            int    dcIndex    = 100;  // 仅 positional 模式用
            if (!isEncoded && (!int.TryParse(dcIndexRaw, out dcIndex) || dcIndex <= 0))
                dcIndex = 100;

            // 输出目录标签：编码格式用"1_1_31"，序号用"31"
            string dcLabel = isEncoded ? dcIndexRaw.Replace('.', '_') : dcIndex.ToString();

            // 贴图提取上限（超出的在报告里列出，但不实际提取）
            int textureLimit = int.TryParse(_config.Get("DCTextureLimit", "20"), out int lim) ? lim : 20;

            // ── 1. 参数校验 ────────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(_sdpPath))
            {
                Console.WriteLine("Error: SDP file path is required. Use -sdp parameter.");
                Console.WriteLine("Example: -mode dcanalysis -sdp \"file.sdp\" -output \"out/\"");
                return;
            }
            if (!File.Exists(_sdpPath))
            {
                Console.WriteLine($"Error: SDP file not found: {_sdpPath}");
                return;
            }

            string outputRoot    = string.IsNullOrWhiteSpace(_outputDir)
                ? Path.Combine(Path.GetDirectoryName(_sdpPath)!, $"dc{dcLabel}_output")
                : _outputDir!;
            string shaderOutput  = Path.Combine(outputRoot, "shaders");
            string textureOutput = Path.Combine(outputRoot, "textures");

            Directory.CreateDirectory(shaderOutput);
            Directory.CreateDirectory(textureOutput);

            // ── capture ID：未指定时扫 SDP 里的 snapshot_* 让用户选 ─────────
            int captureId = _captureId;
            if (captureId == 0)
                captureId = SelectCaptureIdFromSdp(_sdpPath!);
            if (captureId == 0) return;

            string displayId = isEncoded ? dcIndexRaw : $"#{dcIndex}";
            Console.WriteLine($"\n=== DrawCall {displayId} Analysis ===");
            Console.WriteLine($"  SDP File:   {_sdpPath}");
            Console.WriteLine($"  Capture ID: {captureId}");
            Console.WriteLine($"  Output:     {Path.GetFullPath(outputRoot)}");

            // ── 2. 提取 sdp.db ─────────────────────────────────────────────
            string tempDir = Path.Combine(Path.GetTempPath(), $"sdpcli_dcanalysis_{Guid.NewGuid()}");
            Directory.CreateDirectory(tempDir);
            string dbPath = Path.Combine(tempDir, "sdp.db");

            try
            {
                Console.WriteLine("\n[Step 1] Extracting database...");
                using (var archive = ZipFile.OpenRead(_sdpPath))
                {
                    var entry = archive.GetEntry("sdp.db");
                    if (entry == null) { Console.WriteLine("Error: sdp.db not found inside SDP archive."); return; }
                    entry.ExtractToFile(dbPath, overwrite: true);
                }
                Console.WriteLine("  ✓ sdp.db extracted");

                // ── 2b. 提取 Snapshot 预览图 ────────────────────────────────
                string? snapshotPath = null;
                Console.WriteLine("\n[Step 1b] Extracting snapshot image...");
                using (var archive = ZipFile.OpenRead(_sdpPath))
                {
                    // Entry 名称可能用 \ 或 /，尝试两种格式后再做宽泛搜索
                    string bmpName1 = $"snapshot_{captureId}\\1_screenshot.bmp";
                    string bmpName2 = $"snapshot_{captureId}/1_screenshot.bmp";
                    var bmpEntry = archive.GetEntry(bmpName1)
                                ?? archive.GetEntry(bmpName2)
                                ?? archive.Entries.FirstOrDefault(e =>
                                       e.FullName.EndsWith("1_screenshot.bmp",
                                           StringComparison.OrdinalIgnoreCase));
                    if (bmpEntry != null)
                    {
                        string bmpTemp = Path.Combine(tempDir, "snapshot.bmp");
                        bmpEntry.ExtractToFile(bmpTemp, overwrite: true);
                        // 转为 PNG；如启用 SnapshotRotateLandscape 且宽>高则顺时针旋转 90°
                        snapshotPath = Path.Combine(outputRoot, "snapshot.png");
                        bool rotateLandscape = _config.GetBool("SnapshotRotateLandscape", true);
                        using (var bmp = new System.Drawing.Bitmap(bmpTemp))
                        {
                            if (rotateLandscape)
                            {
                                var rotated = new System.Drawing.Bitmap(bmp.Height, bmp.Width);
                                using (var g = System.Drawing.Graphics.FromImage(rotated))
                                {
                                    g.TranslateTransform(0, bmp.Width);
                                    g.RotateTransform(-90);
                                    g.DrawImage(bmp, 0, 0);
                                }
                                rotated.Save(snapshotPath, System.Drawing.Imaging.ImageFormat.Png);
                                rotated.Dispose();
                            }
                            else
                            {
                                bmp.Save(snapshotPath, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        Console.WriteLine("  ✓ Snapshot saved: snapshot.png");
                    }
                    else
                    {
                        Console.WriteLine("  ⚠ No snapshot image found in SDP archive.");
                    }
                }

                // ── 3. 定位目标 DrawCall ────────────────────────────────────
                Console.WriteLine("\n[Step 2] Locating target DrawCall...");
                var db = new SdpDatabase(dbPath, (uint)captureId);

                string dcId;

                if (isEncoded)
                {
                    // 编码格式：直接用 GUI 里的 "1.1.31" 作为 ID 查找
                    dcId = dcIndexRaw;
                    Console.WriteLine($"  Mode: encoded ID → {dcId}");
                }
                else
                {
                    // 序号模式：取列表第 N 条
                    var allDrawCalls = db.GetDrawCallIds();
                    Console.WriteLine($"  Total DrawCalls found: {allDrawCalls.Count}");

                    if (allDrawCalls.Count < dcIndex)
                    {
                        Console.WriteLine($"  ✗ Not enough DrawCalls. Need at least {dcIndex}, found {allDrawCalls.Count}.");
                        return;
                    }

                    dcId = allDrawCalls[dcIndex - 1];
                    Console.WriteLine($"  Mode: positional #{dcIndex} → ID: {dcId}");
                }

                Console.WriteLine($"  ✓ Target DrawCall ID: {dcId}");

                // ── 4. 查询完整资源信息 ─────────────────────────────────────
                Console.WriteLine($"\n[Step 3] Resolving DrawCall {displayId} resources...");
                var drawCallQuerySvc = new DrawCallQueryService();
                var dcInfo = drawCallQuerySvc.GetDrawCallInfo(dbPath, (uint)captureId, dcId);

                if (dcInfo == null)
                {
                    Console.WriteLine("  ✗ Could not resolve DrawCall info.");
                    return;
                }

                Console.WriteLine($"  ✓ Pipeline={dcInfo.PipelineID}  Shaders={dcInfo.Shaders.Count}  Textures={dcInfo.TextureIDs.Length}");

                // reportPath is written after shader/texture extraction (Step 7)
                string reportPath         = Path.Combine(outputRoot, $"dc{dcLabel}_report.md");
                bool   onlyGenerateReport = _config.GetBool("DCOnlyGenerateReport", false);

                if (onlyGenerateReport)
                    Console.WriteLine("  ℹ DCOnlyGenerateReport=true — skipping shader/texture extraction");

                Console.WriteLine("\n--- Summary ---");
                Console.WriteLine($"  API      : {(string.IsNullOrEmpty(dcInfo.ApiName) ? "(unknown)" : dcInfo.ApiName)} (ApiID={dcInfo.ApiID})");
                Console.WriteLine($"  Pipeline : {dcInfo.PipelineID}");
                if (dcInfo.Shaders.Count > 0)
                    foreach (var s in dcInfo.Shaders)
                        Console.WriteLine($"  Shader   : {s.ShaderStageName} (Module={s.ShaderModuleID})");
                else
                    Console.WriteLine("  Shader   : (none found — check pipeline ID)");
                Console.WriteLine($"  Textures : {dcInfo.TextureIDs.Length} bound, extract limit={textureLimit}");
                Console.WriteLine("---------------");

                // ── 6. 提取 Shader（直接用 PipelineID）────────────────────
                if (onlyGenerateReport)
                {
                    Console.WriteLine("\n[Step 5] Shader extraction — SKIPPED (DCOnlyGenerateReport=true)");
                }
                else if (dcInfo.PipelineID == 0)
                {
                    Console.WriteLine("\n[Step 5] Extracting shaders...");
                    Console.WriteLine("  ⚠ PipelineID is 0 — skipping shader extraction.");
                }
                else
                {
                    Console.WriteLine("\n[Step 5] Extracting shaders...");
                    string  shaderFmt      = _config.Get("ShaderOutputFormat", "hlsl").Trim().ToLower();
                    string? spirvCrossPath = ResolveSpirvCrossPath();
                    if (spirvCrossPath == null)
                        Console.WriteLine("  spirv-cross not found — only .spv files will be written");

                    var shaderExtractor = new ShaderExtractor(dbPath, captureId)
                    {
                        SpirvCrossPath     = spirvCrossPath,
                        ShaderOutputFormat = shaderFmt
                    };

                    bool shaderOk = shaderExtractor.ExtractShadersForPipeline(dcInfo.PipelineID, shaderOutput);
                    Console.WriteLine(shaderOk
                        ? $"  ✓ Shaders extracted to: {Path.GetFullPath(shaderOutput)}"
                        : "  ✗ Shader extraction failed (no SPIR-V stages in database for this pipeline).");
                }

                // ── 7. 提取 Texture ────────────────────────────────────────
                if (onlyGenerateReport)
                {
                    Console.WriteLine("\n[Step 6] Texture extraction — SKIPPED (DCOnlyGenerateReport=true)");
                }
                else if (dcInfo.TextureIDs.Length == 0)
                {
                    Console.WriteLine("\n[Step 6] Extracting textures...");
                    Console.WriteLine("  ⚠ No textures bound to this DrawCall.");
                }
                else
                {
                    Console.WriteLine("\n[Step 6] Extracting textures...");
                    int total   = dcInfo.TextureIDs.Length;
                    int limit   = Math.Min(total, textureLimit);
                    int skipped = total - limit;

                    if (skipped > 0)
                        Console.WriteLine($"  {total} textures bound — extracting first {limit}, skipping {skipped}");
                    else
                        Console.WriteLine($"  {total} texture(s) bound, extracting all...");

                    var texExtractor = new TextureExtractor(dbPath, captureId);
                    int success = 0;

                    for (int i = 0; i < limit; i++)
                    {
                        ulong  texId   = dcInfo.TextureIDs[i];
                        string texFile = Path.Combine(textureOutput, $"texture_{texId}.png");
                        Console.Write($"  [{i + 1}/{limit}] texture {texId}... ");
                        if (texExtractor.ExtractTexture(texId, texFile))
                        { Console.WriteLine("✓"); success++; }
                        else
                          Console.WriteLine("✗ failed");
                    }

                    Console.WriteLine($"  Textures: {success}/{limit} extracted successfully.");
                    if (skipped > 0)
                        Console.WriteLine($"  (remaining {skipped} skipped — increase DCTextureLimit in config.ini to extract more)");
                }

                // ── 8. 生成 Markdown 诊断报告（包含实际文件链接） ──────────────
                Console.WriteLine("\n[Step 7] Generating diagnostic report...");
                string mdContent = BuildMarkdownReport(
                    dcInfo, _sdpPath!, captureId, textureLimit, displayId,
                    shaderOutput, textureOutput, snapshotPath);
                File.WriteAllText(reportPath, mdContent, Encoding.UTF8);
                Console.WriteLine($"  ✓ Report: {Path.GetFullPath(reportPath)}");

                // ── 9. 汇总 ───────────────────────────────────────────────
                Console.WriteLine($"\n=== DrawCall {displayId} Analysis Complete ===");
                Console.WriteLine($"  Report  : {Path.GetFullPath(reportPath)}");
                Console.WriteLine($"  Shaders : {Path.GetFullPath(shaderOutput)}");
                Console.WriteLine($"  Textures: {Path.GetFullPath(textureOutput)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    try { Directory.Delete(tempDir, recursive: true); } catch { }
            }
        }

        // ── Snapshot 选择 ────────────────────────────────────────────────────

        private static int SelectCaptureIdFromSdp(string sdpPath)
        {
            var ids = new System.Collections.Generic.List<uint>();
            try
            {
                using var zip = ZipFile.OpenRead(sdpPath);
                foreach (var entry in zip.Entries)
                {
                    var parts = entry.FullName.Split('/');
                    if (parts.Length >= 1 && parts[0].StartsWith("snapshot_"))
                        if (uint.TryParse(parts[0].Substring("snapshot_".Length), out uint id) && !ids.Contains(id))
                            ids.Add(id);
                }
            }
            catch { }

            ids.Sort((a, b) => a.CompareTo(b));

            if (ids.Count == 0)
            {
                Console.Write("No snapshot_* found in SDP. Enter capture ID: ");
                string? m = Console.ReadLine();
                return int.TryParse(m, out int mid) && mid > 0 ? mid : 0;
            }
            if (ids.Count == 1)
            {
                Console.WriteLine($"Found 1 snapshot: snapshot_{ids[0]}");
                return (int)ids[0];
            }
            Console.WriteLine($"\nFound {ids.Count} snapshots in SDP:");
            for (int i = 0; i < ids.Count; i++)
                Console.WriteLine($"  {i + 1}. snapshot_{ids[i]}");
            Console.Write($"Select snapshot (1-{ids.Count}): ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int sel) && sel >= 1 && sel <= ids.Count)
                return (int)ids[sel - 1];
            Console.WriteLine("Invalid selection");
            return 0;
        }

        // ── Markdown 报告 ────────────────────────────────────────────────────

        private static string BuildMarkdownReport(
            Models.DrawCallInfo dc, string sdpPath, int captureId, int textureLimit, string displayId,
            string shaderDir, string textureDir, string? snapshotPath = null)
        {
            var sb  = new StringBuilder();
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            sb.AppendLine($"# DrawCall {displayId} Diagnostic Report");
            sb.AppendLine($"Generated: {now}  ");
            sb.AppendLine($"SDP File: `{sdpPath}`  ");
            sb.AppendLine($"Capture ID: {captureId}");
            sb.AppendLine();

            // ── Snapshot 预览 ──
            if (snapshotPath != null && File.Exists(snapshotPath))
            {
                sb.AppendLine("## Frame Snapshot");
                sb.AppendLine($"![Frame Snapshot](snapshot.png)");
                sb.AppendLine();
            }

            // ── Core info ──
            sb.AppendLine("## DrawCall Info");
            sb.AppendLine("| Field | Value |");
            sb.AppendLine("|-------|-------|");
            sb.AppendLine($"| DrawCall ID | `{dc.DrawCallNumber}` |");
            sb.AppendLine($"| API | `{dc.ApiName}` (ApiID={dc.ApiID}) |");
            sb.AppendLine($"| Pipeline ID | `{dc.PipelineID}` |");
            sb.AppendLine($"| Layout ID | `{dc.LayoutID}` |");
            sb.AppendLine($"| Render Pass | `{dc.RenderPass}` |");
            if (dc.VertexCount > 0 || dc.InstanceCount > 0)
            {
                sb.AppendLine($"| Vertex Count | {dc.VertexCount} |");
                sb.AppendLine($"| Index Count | {dc.IndexCount} |");
                sb.AppendLine($"| Instance Count | {dc.InstanceCount} |");
            }
            sb.AppendLine();

            // ── Shaders ──
            sb.AppendLine("## Shaders");
            if (dc.Shaders.Count == 0)
            {
                sb.AppendLine("> ⚠ No shader stages found for pipeline `" + dc.PipelineID + "`.");
                sb.AppendLine("> Check that VulkanSnapshotShaderStages has entries for this captureID and pipelineID.");
            }
            else
            {
                sb.AppendLine("| Stage | Module ID | Entry Point | Files |");
                sb.AppendLine("|-------|-----------|-------------|-------|");
                foreach (var s in dc.Shaders)
                {
                    // Find any extracted files that match this module or stage name
                    string stageKey = s.ShaderStageName.ToLower();
                    string moduleKey = s.ShaderModuleID.ToString();
                    var matchedFiles = Directory.Exists(shaderDir)
                        ? Directory.GetFiles(shaderDir)
                            .Where(f => {
                                string fn = Path.GetFileName(f).ToLower();
                                return fn.Contains(moduleKey) || fn.Contains(stageKey);
                            })
                            .Select(f => $"[{Path.GetFileName(f)}](shaders/{Path.GetFileName(f)})")
                            .ToList()
                        : new System.Collections.Generic.List<string>();
                    string fileLinks = matchedFiles.Count > 0 ? string.Join(" · ", matchedFiles) : "–";
                    sb.AppendLine($"| {s.ShaderStageName} | `{s.ShaderModuleID}` | `{s.EntryPoint}` | {fileLinks} |");
                }
            }
            // List all extracted shader files not already linked (e.g. .spv)
            if (Directory.Exists(shaderDir))
            {
                var allShaderFiles = Directory.GetFiles(shaderDir).OrderBy(f => f).ToList();
                if (allShaderFiles.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("**Extracted shader files:**");
                    foreach (var f in allShaderFiles)
                        sb.AppendLine($"- [{Path.GetFileName(f)}](shaders/{Path.GetFileName(f)})");
                }
            }
            sb.AppendLine();

            // ── Textures ──
            sb.AppendLine($"## Textures ({dc.TextureIDs.Length} bound)");
            if (dc.TextureIDs.Length == 0)
            {
                sb.AppendLine("> No textures bound to this DrawCall.");
            }
            else
            {
                sb.AppendLine();
                if (dc.Textures.Count > 0)
                {
                    sb.AppendLine("| # | Texture ID | Width | Height | Depth | Format | Image |");
                    sb.AppendLine("|---|------------|-------|--------|-------|--------|-------|");
                    int idx = 0;
                    foreach (var t in dc.Textures)
                    {
                        string depthStr = t.Depth > 1 ? t.Depth.ToString() : "-";
                        string imgPath  = Path.Combine(textureDir, $"texture_{t.TextureID}.png");
                        string imgLink  = File.Exists(imgPath)
                            ? $"![texture_{t.TextureID}](textures/texture_{t.TextureID}.png)"
                            : "–";
                        sb.AppendLine($"| {++idx} | `{t.TextureID}` | {t.Width} | {t.Height} | {depthStr} | {t.FormatName} | {imgLink} |");
                    }
                    // IDs without metadata
                    var extraIds = dc.TextureIDs.Skip(dc.Textures.Count).ToArray();
                    if (extraIds.Length > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"**Additional texture IDs ({extraIds.Length}, no metadata):**");
                        foreach (var id in extraIds)
                        {
                            string imgPath  = Path.Combine(textureDir, $"texture_{id}.png");
                            string imgLink  = File.Exists(imgPath)
                                ? $"![texture_{id}](textures/texture_{id}.png)"
                                : $"`{id}`";
                            sb.AppendLine($"- {imgLink}");
                        }
                    }
                }
                else
                {
                    sb.AppendLine("| # | Texture ID | Image |");
                    sb.AppendLine("|---|------------|-------|");
                    int idx = 0;
                    foreach (uint id in dc.TextureIDs)
                    {
                        string imgPath = Path.Combine(textureDir, $"texture_{id}.png");
                        string imgLink = File.Exists(imgPath)
                            ? $"![texture_{id}](textures/texture_{id}.png)"
                            : "–";
                        sb.AppendLine($"| {++idx} | `{id}` | {imgLink} |");
                    }
                }
            }
            sb.AppendLine();

            // ── Render Targets ──
            if (dc.RenderTargets.Count > 0)
            {
                sb.AppendLine($"## Render Targets ({dc.RenderTargets.Count})");
                sb.AppendLine("| Idx | Type | ResourceID | RenderPassID | FramebufferID |");
                sb.AppendLine("|-----|------|-----------|--------------|---------------|");
                foreach (var rt in dc.RenderTargets)
                    sb.AppendLine($"| {rt.AttachmentIndex} | {rt.AttachmentType} | `{rt.AttachmentResourceID}` | `{rt.RenderPassID}` | `{rt.FramebufferID}` |");
                sb.AppendLine();
            }

            // ── Vertex / Index Buffers ──
            if (dc.VertexBuffers.Count > 0)
            {
                sb.AppendLine($"## Vertex Buffers ({dc.VertexBuffers.Count})");
                sb.AppendLine("| Binding | BufferID |");
                sb.AppendLine("|---------|----------|");
                foreach (var vb in dc.VertexBuffers)
                    sb.AppendLine($"| {vb.Binding} | `{vb.BufferID}` |");
                sb.AppendLine();
            }
            if (dc.IndexBuffer != null)
            {
                sb.AppendLine("## Index Buffer");
                sb.AppendLine($"BufferID=`{dc.IndexBuffer.BufferID}`  Offset={dc.IndexBuffer.Offset}  Type={dc.IndexBuffer.IndexType}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        // ── 私有辅助 ────────────────────────────────────────────────────────

        private string? ResolveSpirvCrossPath()
        {
            string sdkPath = _config.Get("VulkanSDKPath", "");
            if (!string.IsNullOrWhiteSpace(sdkPath))
            {
                string c = Path.Combine(sdkPath, "Bin", "spirv-cross.exe");
                if (File.Exists(c)) return c;
            }
            string? envSdk = Environment.GetEnvironmentVariable("VULKAN_SDK");
            if (!string.IsNullOrWhiteSpace(envSdk))
            {
                string c = Path.Combine(envSdk, "Bin", "spirv-cross.exe");
                if (File.Exists(c)) return c;
            }
            string local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spirv-cross.exe");
            if (File.Exists(local)) return local;
            return null;
        }
    }
}
