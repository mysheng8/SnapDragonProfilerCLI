using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnapdragonProfilerCLI.Data;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Analysis
{
    /// <summary>
    /// Controls which passes the pipeline executes.
    ///   All          — full pipeline (default)
    ///   StatsOnly    — Pass A only (Steps 1–A6), skip B1/B2/B3
    ///   AnalysisOnly — Pass B only (B1/B2/B3), skip asset extraction and A4/A5/A6 generation
    /// Set via config key AnalysisPassMode = all|stats|analysis, or CLI flags -stats-only / -analysis-only.
    /// </summary>
    public enum PassMode { All, StatsOnly, AnalysisOnly }

    /// <summary>
    /// Analysis Pipeline — 4-step orchestrator:
    ///   Step 1  Collect all DrawCalls (submit1 / cmd1 filtered)
    ///   Step 2  Label each DC (shader-name rules → Category + Detail)
    ///   Step 3  Join profiler metrics CSV, export labeled CSV
    ///   Step 4  Generate Markdown summary (Top5, category stats, bottleneck)
    /// </summary>
    public class AnalysisPipeline
    {
        private readonly Services.Analysis.SdpFileService sdpFileService;
        private readonly Services.Analysis.DrawCallAnalysisService analysisService;
        private readonly Services.Analysis.RawJsonGenerationService reportService;
        private readonly Services.Analysis.DrawCallLabelService labelService;
        private readonly Services.Analysis.MetricsQueryService metricsService;
        private readonly Config config;
        private readonly ILogger logger;
        private readonly Tools.LlmApiWrapper? _llm;

        public AnalysisPipeline(
            Services.Analysis.SdpFileService sdpFileService,
            Services.Analysis.DrawCallAnalysisService analysisService,
            Services.Analysis.RawJsonGenerationService reportService,
            Services.Analysis.DrawCallLabelService labelService,
            Services.Analysis.MetricsQueryService metricsService,
            Config config,
            ILogger logger,
            Tools.LlmApiWrapper? llm = null)
        {
            this.sdpFileService  = sdpFileService;
            this.analysisService = analysisService;
            this.reportService        = reportService;
            this.labelService         = labelService;
            this.metricsService       = metricsService;
            this.config               = config;
            this.logger               = logger;
            _llm                      = llm;
        }

        /// <summary>Run the 4-step analysis pipeline.</summary>
        public void RunAnalysis(string sdpPath, string outputDir, uint captureId,
                                int? cmdBufferFilter = null)
        {
            try
            {
                logger.Info("\n=== Analysis Pipeline ===\n");
                PassMode passMode  = GetPassMode();
                bool onlyReport    = config.GetBool("AnalysisOnlyGenerateReport", false)
                                   || passMode == PassMode.AnalysisOnly;
                bool skipPassAGen  = passMode == PassMode.AnalysisOnly;  // skip A4/A5/A6 generation
                bool skipPassB     = passMode == PassMode.StatsOnly;     // skip B1/B2/B3
                if (passMode != PassMode.All)
                {
                    string pmCfg = config.Get("AnalysisPassMode", "all");
                    logger.Info("  \u2139 PassMode=" + passMode + " (AnalysisPassMode=" + pmCfg + ")");
                }
                if (onlyReport && passMode == PassMode.AnalysisOnly)
                    logger.Info("  \u2139 AnalysisOnly — skipping extraction + Pass A generation; using existing JSONs");
                // ── Setup: locate + open DB ───────────────────────────────────
                string? dbPath = sdpFileService.FindDatabasePath(sdpPath);
                if (string.IsNullOrEmpty(dbPath))
                    throw new Exception("sdp.db not found in .sdp file");

                // Create unified DB entry point and run pre-flight validation
                var db = new SdpDatabase(dbPath!, captureId);
                logger.Info("Pre-flight: Validating database tables...");
                db.ValidateForAnalysis(logger);

                // ── Pre-compute session paths ────────────────────────────────────
                string sdpName    = System.IO.Path.GetFileNameWithoutExtension(sdpPath);
                string sessionDir = System.IO.Path.Combine(outputDir, sdpName);

                // ── Step 1: Collect all DrawCalls ────────────────────────────
                logger.Info("Step 1: Collecting DrawCalls" +
                    (cmdBufferFilter.HasValue ? $" (CmdBuffer={cmdBufferFilter})" : "") + "...");

                var report = analysisService.AnalyzeAllDrawCalls(dbPath, captureId, cmdBufferFilter);

                logger.Info($"  → {report.AnalyzedDrawCalls} DrawCalls collected" +
                    $"  (pipelines={report.Statistics.TotalPipelines}" +
                    $"  textures={report.Statistics.TotalTextures}" +
                    $"  shaders={report.Statistics.TotalShaders})");

                // ── Output folder: outputDir/<sdp-name>/snapshot_{captureId}/ ──
                string captureOutDir  = System.IO.Path.Combine(sessionDir, $"snapshot_{captureId}");
                System.IO.Directory.CreateDirectory(captureOutDir);
                logger.Info($"  Session folder: {sessionDir}");
                logger.Info($"  Capture output: {captureOutDir}");

                // ── Step 1.5: Extract shaders and textures ────────────────────
                // Shared across ALL snapshots in this session — placed at sessionDir level
                // so that assets shared between multiple captures are only written once.
                string shaderBaseDir  = System.IO.Path.Combine(sessionDir, "shaders");
                string textureBaseDir = System.IO.Path.Combine(sessionDir, "textures");

                if (onlyReport)
                {
                    logger.Info("\nStep 1.5: Extraction — SKIPPED (AnalysisOnlyGenerateReport=true)");
                }
                else
                {
                    logger.Info("\nStep 1.5: Extracting shaders and textures in parallel (shared, per-file dedup)...");
                    System.IO.Directory.CreateDirectory(shaderBaseDir);
                    System.IO.Directory.CreateDirectory(textureBaseDir);

                    // Pre-compute unique pipeline IDs and texture IDs (single-threaded, safe)
                    var uniquePipelines = report.DrawCallResults
                        .Where(dc => dc.PipelineID != 0)
                        .Select(dc => dc.PipelineID)
                        .Distinct().ToList();
                    var allTexIds = report.DrawCallResults
                        .SelectMany(dc => dc.TextureIDs.Select(id => (ulong)id))
                        .Distinct().ToList();

                    string  shaderFmt      = config.Get("ShaderOutputFormat", "hlsl").Trim().ToLower();
                    string? spirvCrossPath = ResolveSpirvCrossPath();

                    // Shader extraction — each pipeline gets its own ShaderExtractor (own SQLite connection).
                    // ShaderExtractor is not thread-safe across instances, but each instance is independent.
                    // Files are named pipeline_{id}_{stage}.* — unique per pipeline, no write conflicts.
                    int shaderOkCount = 0;
                    var shaderTask = Task.Run(() =>
                    {
                        Parallel.ForEach(
                            uniquePipelines,
                            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                            pipelineId =>
                            {
                                if (System.IO.Directory.GetFiles(shaderBaseDir,
                                        $"pipeline_{pipelineId}_*.spv").Length > 0)
                                {
                                    Interlocked.Increment(ref shaderOkCount);
                                    return;
                                }
                                var shExt = new Tools.ShaderExtractor(db)
                                {
                                    SpirvCrossPath     = spirvCrossPath,
                                    ShaderOutputFormat = shaderFmt
                                };
                                if (shExt.ExtractShadersForPipeline(pipelineId, shaderBaseDir))
                                    Interlocked.Increment(ref shaderOkCount);
                            });
                    });

                    // Texture extraction — each texture gets its own TextureExtractor (own SQLite connection).
                    // MaxDegreeOfParallelism=4: conservative limit for native Qonvert DLL concurrency.
                    // Increase via config key "TextureExtractionDegree" if your system handles more.
                    int texParallelDegree = config.GetInt("TextureExtractionDegree", 4);
                    int texOk = 0, texSkipped = 0;
                    var textureTask = Task.Run(() =>
                    {
                        Parallel.ForEach(
                            allTexIds,
                            new ParallelOptions { MaxDegreeOfParallelism = texParallelDegree },
                            texId =>
                            {
                                string texFile = System.IO.Path.Combine(textureBaseDir, $"texture_{texId}.png");
                                if (System.IO.File.Exists(texFile))
                                {
                                    Interlocked.Increment(ref texOk);
                                    Interlocked.Increment(ref texSkipped);
                                    return;
                                }
                                var texExt = new Tools.TextureExtractor(db);
                                if (texExt.ExtractTexture(texId, texFile))
                                    Interlocked.Increment(ref texOk);
                            });
                    });

                    Task.WaitAll(shaderTask, textureTask);
                    logger.Info($"  → Shaders: {shaderOkCount}/{uniquePipelines.Count} unique pipelines extracted → {shaderBaseDir}");
                    logger.Info($"  → Textures: {texOk}/{allTexIds.Count} ready ({texSkipped} already existed) → {textureBaseDir}");
                }

                // ── Step 2: Label each DC ────────────────────────────────────
                if (onlyReport)
                {
                    logger.Info("\nStep 2: Labeling — SKIPPED, reloading from existing analysis output...");
                    LoadLabelsFromAnalysis(report, captureOutDir);
                }
                else
                {
                logger.Info("\nStep 2: Labeling DrawCalls (parallel)...");
                int labelCount = 0;
                // LlmMaxConcurrentRequests limits parallel LLM HTTP calls; rule-based path is CPU-only
                // and can safely run at full core count.
                int llmMaxConcurrent = config.GetInt("LlmMaxConcurrentRequests", 8);
                var categorySummary  = new ConcurrentDictionary<string, int>();
                Parallel.ForEach(
                    report.DrawCallResults,
                    new ParallelOptions { MaxDegreeOfParallelism = llmMaxConcurrent },
                    dc =>
                    {
                        dc.Label = labelService.Label(dc, shaderBaseDir);
                        categorySummary.AddOrUpdate(dc.Label.Category, 1, (_, v) => v + 1);
                        int n = Interlocked.Increment(ref labelCount);
                        logger.Info($"  [{n}/{report.DrawCallResults.Count}] DC {dc.DrawCallNumber,-12} [{dc.Label.Category}] {dc.Label.Detail}");
                    });
                logger.Info($"  → Labeled {labelCount} DCs:");
                foreach (var kv in categorySummary.OrderByDescending(x => x.Value))
                    logger.Info($"    {kv.Key}: {kv.Value}");
                }

                // ── Step 3: Join GPU performance metrics from DB ───────────────────────
                logger.Info("\nStep 3: Joining GPU metrics from DB...");

                var metrics = metricsService.LoadMetrics(dbPath!, captureId);
                if (metrics.Count > 0)
                {
                    int joined = 0;
                    foreach (var dc in report.DrawCallResults)
                    {
                        if (metrics.TryGetValue(dc.DrawCallNumber, out var m))
                        { dc.Metrics = m; joined++; }
                    }
                    logger.Info($"  → Loaded {metrics.Count} rows; joined metrics to {joined} / {report.DrawCallResults.Count} DCs");
                }
                else
                {
                    logger.Info("  ⚠ DrawCallMetrics table absent or empty — run 'SDPCLI import' first.");
                }

                // ── Step 3.5: Extract meshes for all non-compute DrawCalls ────
                string meshBaseDir = System.IO.Path.Combine(sessionDir, "meshes");

                if (onlyReport)
                {
                    logger.Info("\nStep 3.5: Mesh extraction — SKIPPED (AnalysisOnlyGenerateReport=true)");
                }
                else
                {
                    logger.Info("\nStep 3.5: Extracting meshes for non-compute DrawCalls...");
                    System.IO.Directory.CreateDirectory(meshBaseDir);

                    var meshDcs = report.DrawCallResults
                        .Where(dc => dc.ApiName.IndexOf("Dispatch",
                            StringComparison.OrdinalIgnoreCase) < 0
                            && dc.VertexBuffers.Count > 0)
                        .ToList();

                    int meshOk = 0;
                    int meshDegree = config.GetInt("MeshExtractionDegree", 4);
                    Parallel.ForEach(meshDcs,
                        new ParallelOptions { MaxDegreeOfParallelism = meshDegree },
                        dc =>
                        {
                            string objPath = System.IO.Path.Combine(meshBaseDir, $"mesh_{dc.ApiID}.obj");
                            if (System.IO.File.Exists(objPath)) { Interlocked.Increment(ref meshOk); return; }
                            var ext = new Tools.MeshExtractor(db);
                            if (ext.ExtractMesh(dc.DrawCallNumber, objPath))
                                Interlocked.Increment(ref meshOk);
                        });

                    logger.Info($"  → Meshes: {meshOk}/{meshDcs.Count} OBJ files → {meshBaseDir}");
                }

                // Always (re)generate viewer.html if meshes/ dir has any OBJ files
                if (System.IO.Directory.Exists(meshBaseDir))
                {
                    var objFiles = System.IO.Directory.GetFiles(meshBaseDir, "*.obj");
                    if (objFiles.Length > 0)
                    {
                        GenerateMeshViewerHtml(meshBaseDir, objFiles.Select(System.IO.Path.GetFileName).ToList());
                        logger.Info($"  → Viewer: {System.IO.Path.Combine(meshBaseDir, "viewer.html")}");
                    }
                }

                // Export labeled + metrics JSON to captureOutDir
                // JSON annotates each DC with the shader, texture, and mesh file paths it references.
                if (!skipPassAGen)
                {
                string labeledJson = reportService.GenerateLabeledMetricsJson(
                    report, captureOutDir, shaderBaseDir, textureBaseDir, meshBaseDir, captureId, sdpName);
                logger.Info($"  → JSON: {labeledJson}");
                }
                else
                {
                    logger.Info("  → raw.json: SKIPPED (AnalysisOnly mode, using existing)");
                }

                // ── Step A5: Status JSON (percentile stats, no LLM) ───────────
                Services.Analysis.StatusJsonResult? statusResult = null;
                if (!skipPassAGen)
                {
                logger.Info("\nStep A5: Generating status.json...");
                try
                {
                    var statusService = new Services.Analysis.StatusJsonService();
                    statusResult = statusService.GenerateStatusJson(
                        report, captureOutDir, captureId, sdpName);
                    logger.Info($"  → Status: {statusResult.FilePath}");

                    // ── Step A6: TopDC JSON (attribution rule engine) ─────────
                    logger.Info("\nStep A6: Generating topdc.json...");
                    try
                    {
                        // Locate attribution_rules.json — next to the exe or in analysis/ subfolder
                        string rulesPath = System.IO.Path.Combine(
                            System.IO.Path.GetDirectoryName(
                                System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                            "analysis", "attribution_rules.json");
                        if (!System.IO.File.Exists(rulesPath))
                        {
                            rulesPath = System.IO.Path.Combine(
                                System.IO.Path.GetDirectoryName(sdpPath) ?? ".", "..", "analysis", "attribution_rules.json");
                        }

                        if (!System.IO.File.Exists(rulesPath))
                        {
                            logger.Info("  ⚠ attribution_rules.json not found — skipping topdc.json");
                        }
                        else
                        {
                            var engine = new Services.Analysis.AttributionRuleEngine(rulesPath);
                            // Use pre-computed percentile tables directly — no re-parse of status.json
                            var topDcService = new Services.Analysis.TopDcJsonService(engine);
                            string topDcPath = topDcService.GenerateTopDcJson(
                                report, statusResult!.GlobalPercentiles, statusResult!.CategoryStatsMap,
                                captureOutDir, shaderBaseDir, meshBaseDir, captureId, sdpName);
                            logger.Info($"  → TopDC: {topDcPath}");
                        }
                    }
                    catch (Exception topEx)
                    {
                        logger.Info($"  ⚠ topdc.json generation failed: {topEx.Message}");
                    }
                }
                catch (Exception statusEx)
                {
                    logger.Info($"  ⚠ status.json generation failed: {statusEx.Message}");
                }
                } // end if (!skipPassAGen)
                else
                {
                    logger.Info("\nStep A5+A6: SKIPPED (AnalysisOnly mode, using existing status.json / topdc.json)");
                }

                // ── Step B1: Per-DC LLM content analysis ──────────────────────
                if (skipPassB)
                {
                    logger.Info("\nStep B1/B2/B3: SKIPPED (StatsOnly mode)");
                }
                else
                {
                logger.Info("\nStep B1: DC content analysis...");
                try
                {
                    var contentService = new Services.Analysis.DcContentAnalysisService(config, logger, _llm);
                    int b1Count = contentService.AnalyzeAll(report, captureOutDir, shaderBaseDir);
                    logger.Info($"  → Analyzed {b1Count} DCs (cached to per_dc_content/)");

                    // ── Step B2: Per-category attribution report (LLM) ────────
                    logger.Info("\nStep B2: Attribution analysis report...");
                    try
                    {
                        var attrService = new Services.Analysis.AttributionReportService(
                            config, logger, _llm, contentService);
                        string analysisMd = attrService.GenerateAnalysisMd(
                            report, captureOutDir, captureId, sdpName);
                        logger.Info($"  → Analysis: {analysisMd}");
                    }
                    catch (Exception b2Ex)
                    {
                        logger.Info($"  ⚠ Step B2 failed: {b2Ex.Message}");
                    }

                    // ── Step B3: Dashboard (rule-based charts + tables) ───────
                    logger.Info("\nStep B3: Dashboard generation...");
                    try
                    {
                        var dashService = new Services.Analysis.DashboardGenerationService(config, logger);
                        string dashMd   = dashService.GenerateDashboard(
                            report, captureOutDir, captureId, sdpName);
                        logger.Info($"  → Dashboard: {dashMd}");
                    }
                    catch (Exception b3Ex)
                    {
                        logger.Info($"  ⚠ Step B3 failed: {b3Ex.Message}");
                    }
                }
                catch (Exception b1Ex)
                {
                    logger.Info($"  ⚠ Step B1 failed: {b1Ex.Message}");
                }
                } // end if (!skipPassB)

                // ── Step 4: Summary report (DEPRECATED — pending removal) ──────
                // logger.Info("\nStep 4: Generating summary...");
                // string summaryMd = reportService.GenerateSummaryReport(report, captureOutDir);
                // logger.Info($"  → Summary: {summaryMd}");

                // ── Step 5: Compact report.json (DEPRECATED — pending removal) ─
                // logger.Info("\nStep 5: Generating report.json...");
                // try
                // {
                //     captureReportService.GenerateReport(report, captureOutDir, summaryMd);
                // }
                // catch (Exception reportEx)
                // {
                //     logger.Info($"  ⚠ report.json generation failed: {reportEx.Message}");
                // }

                logger.Info("\n=== Analysis Complete ===");
            }
            catch (Exception ex)
            {
                logger.Error($"Analysis failed: {ex.Message}");
                logger.Debug($"Stack: {ex.StackTrace}");
                throw;
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private PassMode GetPassMode()
        {
            string raw = config.Get("AnalysisPassMode", "all").Trim().ToLowerInvariant();
            return raw switch
            {
                "stats"    => PassMode.StatsOnly,
                "analysis" => PassMode.AnalysisOnly,
                _          => PassMode.All
            };
        }

        private static void GenerateMeshViewerHtml(string meshDir, List<string> objFiles)
        {
            // Embed OBJ file contents inline so viewer.html works when opened as file://
            // (XHR is blocked by browsers for local files, so OBJLoader.load() fails)
            var objDataEntries = new System.Text.StringBuilder();
            foreach (var fname in objFiles)
            {
                string fullPath = System.IO.Path.Combine(meshDir, fname);
                if (!System.IO.File.Exists(fullPath)) continue;
                string content = System.IO.File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
                // Escape for JS template literal: backslash and backtick
                content = content.Replace("\\", "\\\\").Replace("`", "\\`");
                objDataEntries.AppendLine($"OBJ_DATA[\"{fname}\"] = `{content}`;");
            }

            string fileList = string.Join(", ", objFiles.Select(f => $"\"{f}\""));

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset=\"utf-8\">");
            sb.AppendLine("<title>Mesh Viewer — SDPCLI</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("  * { box-sizing: border-box; margin: 0; padding: 0; }");
            sb.AppendLine("  body { background: #12121f; display: flex; height: 100vh; font-family: monospace; }");
            sb.AppendLine("  #sidebar { width: 180px; background: #1a1a30; color: #ccc; padding: 12px; overflow-y: auto; flex-shrink: 0; }");
            sb.AppendLine("  #sidebar h3 { font-size: 11px; color: #778; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 10px; }");
            sb.AppendLine("  .dc-btn { display: block; width: 100%; margin: 3px 0; padding: 7px 8px; background: #252540;");
            sb.AppendLine("    color: #aac; border: 1px solid #333; cursor: pointer; font-size: 11px; text-align: left;");
            sb.AppendLine("    border-radius: 3px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }");
            sb.AppendLine("  .dc-btn:hover { background: #333360; border-color: #556; }");
            sb.AppendLine("  .dc-btn.active { background: #3a3a80; border-color: #88aaff; color: #fff; }");
            sb.AppendLine("  #controls { padding: 10px 0; border-top: 1px solid #2a2a45; margin-top: 10px; }");
            sb.AppendLine("  #controls button { padding: 5px 8px; background: #252540; color: #aac; border: 1px solid #333;");
            sb.AppendLine("    cursor: pointer; font-size: 10px; border-radius: 3px; margin: 2px 0; width: 100%; }");
            sb.AppendLine("  #controls button:hover { background: #333360; }");
            sb.AppendLine("  #canvas-wrap { flex: 1; position: relative; }");
            sb.AppendLine("  #canvas-wrap canvas { width: 100% !important; height: 100% !important; }");
            sb.AppendLine("  #info { position: absolute; bottom: 10px; left: 12px; color: #667; font-size: 10px; pointer-events: none; }");
            sb.AppendLine("  #status { position: absolute; top: 12px; left: 12px; color: #88aaff; font-size: 11px;");
            sb.AppendLine("    background: rgba(0,0,0,.55); padding: 4px 10px; border-radius: 3px; pointer-events: none; }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<div id=\"sidebar\">");
            sb.AppendLine("  <h3>Draw Calls</h3>");
            sb.AppendLine("  <div id=\"btnlist\"></div>");
            sb.AppendLine("  <div id=\"controls\">");
            sb.AppendLine("    <button id=\"btnWire\">Wireframe: OFF</button>");
            sb.AppendLine("    <button id=\"btnReset\">Reset Camera</button>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div id=\"canvas-wrap\">");
            sb.AppendLine("  <div id=\"status\">Loading…</div>");
            sb.AppendLine("  <div id=\"info\">Left-drag: rotate &nbsp;|&nbsp; Scroll: zoom &nbsp;|&nbsp; Right-drag: pan</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.128.0/build/three.min.js\"></script>");
            sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.128.0/examples/js/controls/OrbitControls.js\"></script>");
            sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.128.0/examples/js/loaders/OBJLoader.js\"></script>");
            // Inline OBJ data — must come AFTER OBJLoader script so the variable exists
            sb.AppendLine("<script>");
            sb.AppendLine($"const FILES=[{fileList}];");
            sb.AppendLine("const OBJ_DATA={};");
            sb.Append(objDataEntries);
            sb.AppendLine("</script>");
            sb.AppendLine("<script>");
            sb.AppendLine("const wrap=document.getElementById('canvas-wrap');");
            sb.AppendLine("const status=document.getElementById('status');");
            sb.AppendLine("const renderer=new THREE.WebGLRenderer({antialias:true});");
            sb.AppendLine("renderer.setPixelRatio(devicePixelRatio); renderer.setClearColor(0x12121f);");
            sb.AppendLine("wrap.appendChild(renderer.domElement);");
            sb.AppendLine("const scene=new THREE.Scene();");
            sb.AppendLine("const camera=new THREE.PerspectiveCamera(45,1,0.001,100000);");
            sb.AppendLine("const controls=new THREE.OrbitControls(camera,renderer.domElement);");
            sb.AppendLine("controls.enableDamping=true; controls.dampingFactor=0.08;");
            sb.AppendLine("const ambient=new THREE.AmbientLight(0xffffff,0.5); scene.add(ambient);");
            sb.AppendLine("const dir1=new THREE.DirectionalLight(0xffffff,0.9); dir1.position.set(1,2,2); scene.add(dir1);");
            sb.AppendLine("const dir2=new THREE.DirectionalLight(0x6688ff,0.4); dir2.position.set(-1,-1,-1); scene.add(dir2);");
            sb.AppendLine("const grid=new THREE.GridHelper(4,20,0x333355,0x222233); scene.add(grid);");
            sb.AppendLine("let currentObj=null, wireframe=false;");
            sb.AppendLine("let initCamPos=new THREE.Vector3(), initTarget=new THREE.Vector3();");
            sb.AppendLine("function loadFile(filename){");
            sb.AppendLine("  status.textContent='Loading '+filename+'…';");
            sb.AppendLine("  if(currentObj){scene.remove(currentObj);currentObj=null;}");
            sb.AppendLine("  const raw=OBJ_DATA[filename];");
            sb.AppendLine("  if(!raw){status.textContent='No data for '+filename;return;}");
            sb.AppendLine("  try{");
            sb.AppendLine("    const obj=new THREE.OBJLoader().parse(raw);");
            sb.AppendLine("    const box=new THREE.Box3().setFromObject(obj);");
            sb.AppendLine("    const center=box.getCenter(new THREE.Vector3());");
            sb.AppendLine("    const size=box.getSize(new THREE.Vector3());");
            sb.AppendLine("    const maxDim=Math.max(size.x,size.y,size.z,0.001);");
            sb.AppendLine("    const scale=2.0/maxDim;");
            sb.AppendLine("    obj.position.sub(center.multiplyScalar(scale));");
            sb.AppendLine("    obj.scale.setScalar(scale);");
            sb.AppendLine("    const mat=new THREE.MeshStandardMaterial({color:0x88aacc,metalness:0.15,roughness:0.7,side:THREE.DoubleSide,wireframe:wireframe});");
            sb.AppendLine("    obj.traverse(c=>{if(c.isMesh)c.material=mat;});");
            sb.AppendLine("    scene.add(obj); currentObj=obj;");
            sb.AppendLine("    let verts=0; obj.traverse(c=>{if(c.isMesh)verts+=c.geometry.attributes.position.count;});");
            sb.AppendLine("    initCamPos.set(0,size.y*scale*0.8,maxDim*scale*2.5);");
            sb.AppendLine("    initTarget.set(0,0,0);");
            sb.AppendLine("    camera.position.copy(initCamPos); controls.target.copy(initTarget); controls.update();");
            sb.AppendLine("    status.textContent=filename+' | '+verts.toLocaleString()+' vertices';");
            sb.AppendLine("  }catch(e){status.textContent='Parse error: '+(e.message||e);}");
            sb.AppendLine("}");
            sb.AppendLine("// Sidebar buttons");
            sb.AppendLine("const btnList=document.getElementById('btnlist');");
            sb.AppendLine("let activeBtn=null;");
            sb.AppendLine("FILES.forEach(f=>{");
            sb.AppendLine("  const b=document.createElement('button');");
            sb.AppendLine("  b.className='dc-btn'; b.title=f;");
            sb.AppendLine("  b.textContent=f.replace('drawcall_','DC ').replace('.obj','');");
            sb.AppendLine("  b.onclick=()=>{if(activeBtn)activeBtn.classList.remove('active');b.classList.add('active');activeBtn=b;loadFile(f);};");
            sb.AppendLine("  btnList.appendChild(b);");
            sb.AppendLine("});");
            sb.AppendLine("// Wireframe toggle");
            sb.AppendLine("document.getElementById('btnWire').onclick=function(){");
            sb.AppendLine("  wireframe=!wireframe; this.textContent='Wireframe: '+(wireframe?'ON':'OFF');");
            sb.AppendLine("  if(currentObj)currentObj.traverse(c=>{if(c.isMesh)c.material.wireframe=wireframe;});");
            sb.AppendLine("};");
            sb.AppendLine("document.getElementById('btnReset').onclick=()=>{");
            sb.AppendLine("  camera.position.copy(initCamPos); controls.target.copy(initTarget); controls.update();");
            sb.AppendLine("};");
            sb.AppendLine("// Resize");
            sb.AppendLine("function resize(){const w=wrap.clientWidth,h=wrap.clientHeight;renderer.setSize(w,h,false);camera.aspect=w/h;camera.updateProjectionMatrix();}");
            sb.AppendLine("window.addEventListener('resize',resize); resize();");
            sb.AppendLine("// Animate");
            sb.AppendLine("(function loop(){requestAnimationFrame(loop);controls.update();renderer.render(scene,camera);})();");
            sb.AppendLine("// Auto-load first");
            sb.AppendLine("if(FILES.length>0)btnList.querySelector('button').click();");
            sb.AppendLine("</script></body></html>");

            System.IO.File.WriteAllText(
                System.IO.Path.Combine(meshDir, "viewer.html"),
                sb.ToString(),
                System.Text.Encoding.UTF8);
        }

        private string? ResolveSpirvCrossPath()
        {
            string sdkPath = config.Get("VulkanSDKPath", "");
            if (!string.IsNullOrWhiteSpace(sdkPath))
            {
                string c = System.IO.Path.Combine(sdkPath, "Bin", "spirv-cross.exe");
                if (System.IO.File.Exists(c)) return c;
            }
            string? envSdk = Environment.GetEnvironmentVariable("VULKAN_SDK");
            if (!string.IsNullOrWhiteSpace(envSdk))
            {
                string c = System.IO.Path.Combine(envSdk, "Bin", "spirv-cross.exe");
                if (System.IO.File.Exists(c)) return c;
            }
            string local = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spirv-cross.exe");
            if (System.IO.File.Exists(local)) return local;
            return null;
        }

        /// <summary>
        /// Reload Category + Detail labels from the most recent DrawCallAnalysis output in captureOutDir.
        /// Tries JSON first (new format), falls back to CSV (legacy) for backward compatibility.
        /// Called when AnalysisOnlyGenerateReport=true to skip LLM labeling.
        /// </summary>
        private void LoadLabelsFromAnalysis(DrawCallAnalysisReport report, string captureOutDir)
        {
            // ── Try new-schema raw.json (schema_version 2.0) ──────────────────
            string? jsonPath = null;
            bool    isNewSchema = false;
            if (System.IO.Directory.Exists(captureOutDir))
            {
                // Prefer snapshot_*_raw.json (schema 2.0)
                jsonPath = System.IO.Directory.GetFiles(captureOutDir, "snapshot_*_raw.json")
                    .OrderByDescending(f => System.IO.File.GetLastWriteTime(f))
                    .FirstOrDefault();
                if (jsonPath != null) isNewSchema = true;

                // Fallback: legacy DrawCallAnalysis_*.json
                if (jsonPath == null)
                    jsonPath = System.IO.Directory.GetFiles(captureOutDir, "DrawCallAnalysis_*.json")
                        .Where(f => System.IO.Path.GetFileName(f).IndexOf("Summary", StringComparison.OrdinalIgnoreCase) < 0)
                        .OrderByDescending(f => System.IO.File.GetLastWriteTime(f))
                        .FirstOrDefault();
            }

            if (jsonPath != null)
            {
                logger.Info($"  Loading labels from: {System.IO.Path.GetFileName(jsonPath)}");
                try
                {
                    string text = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
                    var root    = Newtonsoft.Json.Linq.JObject.Parse(text);

                    // New schema 2.0: root["draw_calls"]  Old schema: root["drawcalls"]
                    var arr = (root["draw_calls"] ?? root["drawcalls"]) as Newtonsoft.Json.Linq.JArray;
                    if (arr != null)
                    {
                        int matched = 0;
                        foreach (var token in arr)
                        {
                            string? dcId = token["dc_id"]?.ToString();
                            if (string.IsNullOrEmpty(dcId)) continue;

                            Models.DrawCallLabel label;
                            if (isNewSchema)
                            {
                                // schema 2.0: label is a subobject
                                var lNode = token["label"];
                                label = new Models.DrawCallLabel
                                {
                                    Category    = lNode?["category"]?.ToString()    ?? "Scene",
                                    Subcategory = lNode?["subcategory"]?.ToString() ?? "",
                                    Detail      = lNode?["detail"]?.ToString()      ?? "",
                                    ReasonTags  = (lNode?["reason_tags"] as Newtonsoft.Json.Linq.JArray)
                                                    ?.Select(t => t.ToString()).ToArray()
                                                  ?? Array.Empty<string>(),
                                    Confidence  = lNode?["confidence"]?.ToObject<float>() ?? 0.70f,
                                    LabelSource = lNode?["label_source"]?.ToString()  ?? "rule",
                                };
                            }
                            else
                            {
                                // legacy schema 1.x: flat category/detail at token root
                                label = new Models.DrawCallLabel
                                {
                                    Category = token["category"]?.ToString() ?? "Scene",
                                    Detail   = token["detail"]?.ToString()   ?? "",
                                };
                            }

                            var dc = report.DrawCallResults.FirstOrDefault(d => d.DrawCallNumber == dcId);
                            if (dc != null) { dc.Label = label; matched++; }
                        }
                        logger.Info($"  → Restored labels for {matched} / {report.DrawCallResults.Count} DCs");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    logger.Info($"  ⚠ Failed to parse JSON labels: {ex.Message} — falling back to CSV.");
                }
            }

            // ── Fallback: legacy CSV ──────────────────────────────────────────
            string? csvPath = null;
            if (System.IO.Directory.Exists(captureOutDir))
            {
                csvPath = System.IO.Directory.GetFiles(captureOutDir, "DrawCallAnalysis_*.csv")
                    .Where(f => System.IO.Path.GetFileName(f).IndexOf("Summary", StringComparison.OrdinalIgnoreCase) < 0)
                    .OrderByDescending(f => System.IO.File.GetLastWriteTime(f))
                    .FirstOrDefault();
            }

            if (csvPath == null)
            {
                logger.Info("  ⚠ No existing snapshot_*_raw.json / DrawCallAnalysis_*.json / *.csv found — labels will be empty.");
                return;
            }

            logger.Info($"  Loading labels from (legacy CSV): {System.IO.Path.GetFileName(csvPath)}");
            var legacyMap = new Dictionary<string, (string Cat, string Det)>();
            bool firstLine = true;
            foreach (var rawLine in System.IO.File.ReadAllLines(csvPath))
            {
                if (firstLine) { firstLine = false; continue; }
                if (string.IsNullOrWhiteSpace(rawLine)) continue;
                var cols = rawLine.Split(',');
                if (cols.Length < 3) continue;
                string dc  = cols[0].Trim().Trim('"');
                string cat = cols[1].Trim().Trim('"');
                string det = cols[2].Trim().Trim('"');
                if (!string.IsNullOrEmpty(dc))
                    legacyMap[dc] = (cat, det);
            }
            int csvMatched = 0;
            foreach (var dc in report.DrawCallResults)
            {
                if (legacyMap.TryGetValue(dc.DrawCallNumber, out var lbl))
                {
                    dc.Label = new Models.DrawCallLabel { Category = lbl.Cat, Detail = lbl.Det };
                    csvMatched++;
                }
            }
            logger.Info($"  → Restored labels for {csvMatched} / {report.DrawCallResults.Count} DCs");
        }
    }
}
