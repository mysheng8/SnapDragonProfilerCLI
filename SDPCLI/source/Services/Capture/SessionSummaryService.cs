using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Writes a session_summary.json file to the session root directory.
    /// The JSON captures session-level metadata (device, app, SDP version) and
    /// a list of all captures with their associated file paths.
    /// </summary>
    public class SessionSummaryService
    {
        public class CaptureEntry
    {
        public uint   CaptureId     { get; }
        public string CaptureSubDir { get; }
        public CaptureEntry(uint captureId, string captureSubDir)
        { CaptureId = captureId; CaptureSubDir = captureSubDir; }
    }

        /// <summary>
        /// Generate and write session_summary.json into <paramref name="sessionPath"/>.
        /// </summary>
        public void Write(
            string            sessionPath,
            Device?           device,
            Config            config,
            string            renderingApi,
            string            appPackage,
            string            appActivity,
            List<string>      metricsActivated,
            List<CaptureEntry> captures)
        {
            try
            {
                // ── SDP version ──────────────────────────────────────────────
                string sdpVersion   = "";
                string sdpBuildDate = "";
                string versionFile  = Path.Combine(sessionPath, "version.txt");
                if (File.Exists(versionFile))
                {
                    try
                    {
                        string raw = File.ReadAllText(versionFile).Trim();
                        // format: { "version":"...", "buildDate":"..." }
                        var obj = JObject.Parse(raw);
                        sdpVersion   = obj["version"]?  .Value<string>() ?? "";
                        sdpBuildDate = obj["buildDate"]?.Value<string>() ?? "";
                    }
                    catch { /* leave empty on parse failure */ }
                }

                // ── Device attributes ────────────────────────────────────────
                string serial       = "";
                string productName  = "";
                string productModel = "";
                string manufacturer = "";
                string brand        = "";
                string platform     = "";
                string osType       = "";
                string androidVer   = "";
                string androidSdk   = "";
                string abiList      = "";

                if (device != null)
                {
                    try
                    {
                        serial = device.GetName() ?? "";
                        DeviceAttributes attr = device.GetDeviceAttributes();
                        productName  = attr.productName          ?? "";
                        productModel = attr.productModel         ?? "";
                        manufacturer = attr.productManufacturer  ?? "";
                        brand        = attr.productBrand         ?? "";
                        platform     = attr.boardPlatform        ?? "";
                        osType       = attr.osType               ?? "";
                        androidVer   = attr.buildVersionRelease  ?? "";
                        androidSdk   = attr.buildVersionSDK      ?? "";
                        abiList      = attr.abiList              ?? "";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  ⚠ SessionSummary: could not read device attributes: {ex.Message}");
                    }
                }

                // ── Timestamp from session folder name ───────────────────────
                string folderName = Path.GetFileName(sessionPath.TrimEnd(Path.DirectorySeparatorChar));
                string timestamp  = "";
                if (DateTime.TryParseExact(folderName, "yyyy-MM-ddTHH-mm-ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var ts))
                {
                    timestamp = ts.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }

                // ── Build capture list ───────────────────────────────────────
                var captureNodes = new List<object>();
                foreach (var entry in captures)
                {
                    string rel(string path) =>
                        path.StartsWith(sessionPath, StringComparison.OrdinalIgnoreCase)
                            ? path.Substring(sessionPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/')
                            : path.Replace('\\', '/');

                    string sub = entry.CaptureSubDir;
                    string subRel = rel(sub);

                    // Helper: return relative path only if the file actually exists
                    string? opt(string absPath) => File.Exists(absPath) ? rel(absPath) : null;
                    string? optDir(string absPath) => Directory.Exists(absPath) ? rel(absPath) + "/" : null;

                    string padded = entry.CaptureId.ToString("D3");

                    captureNodes.Add(new
                    {
                        capture_id    = entry.CaptureId,
                        capture_index = subRel,
                        files = new
                        {
                            screenshot     = opt(Path.Combine(sub, "snapshot.png")),
                            screenshot_bmp = opt(Path.Combine(sub, "1_screenshot.bmp")),
                            gfxrz          = opt(Path.Combine(sessionPath, $"sdpframe_{padded}.gfxrz")),
                            gfxrz_stripped = opt(Path.Combine(sessionPath, $"sdpframestripped_{padded}.gfxrz")),

                            drawcall_metrics         = opt(Path.Combine(sub, "DrawCallMetrics.csv")),
                            drawcall_bindings        = opt(Path.Combine(sub, "DrawCallBindings.csv")),
                            drawcall_parameters      = opt(Path.Combine(sub, "DrawCallParameters.csv")),
                            drawcall_render_targets  = opt(Path.Combine(sub, "DrawCallRenderTargets.csv")),
                            drawcall_index_buffers   = opt(Path.Combine(sub, "DrawCallIndexBuffers.csv")),
                            drawcall_vertex_buffers  = opt(Path.Combine(sub, "DrawCallVertexBuffers.csv")),
                            pipeline_vertex_inputs   = opt(Path.Combine(sub, "PipelineVertexInputAttributes.csv")),
                            pipeline_vertex_bindings = opt(Path.Combine(sub, "PipelineVertexInputBindings.csv")),

                            analysis_csv     = LatestFile(sub, "DrawCallAnalysis_*.csv"),
                            analysis_summary = LatestFile(sub, "DrawCallAnalysis_Summary_*.md"),

                            shaders  = optDir(Path.Combine(sub, "shaders")),
                            textures = optDir(Path.Combine(sub, "textures")),
                            meshes   = optDir(Path.Combine(sub, "meshes")),
                        }
                    });
                }

                // ── Assemble root object ─────────────────────────────────────
                var root = new
                {
                    schema_version = "1.0",
                    generated_at   = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),

                    session = new
                    {
                        timestamp    = timestamp,
                        sdp_version  = sdpVersion,
                        sdp_build_date = sdpBuildDate,
                        app_package  = appPackage,
                        app_activity = appActivity,
                        rendering_api = renderingApi,
                    },

                    device = new
                    {
                        serial,
                        product_name    = productName,
                        product_model   = productModel,
                        product_manufacturer = manufacturer,
                        product_brand   = brand,
                        board_platform  = platform,
                        os_type         = osType,
                        android_version = androidVer,
                        android_sdk     = androidSdk,
                        abi_list        = abiList,
                    },

                    files = new
                    {
                        db      = "sdp.db",
                        log     = "sdplog.txt",
                        version = "version.txt",
                    },

                    metrics_activated = metricsActivated,

                    captures = captureNodes,
                };

                // ── Serialize ────────────────────────────────────────────────
                var settings = new JsonSerializerSettings
                {
                    Formatting        = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                };
                string json     = JsonConvert.SerializeObject(root, settings);
                string outPath  = Path.Combine(sessionPath, "session_summary.json");
                File.WriteAllText(outPath, json, Encoding.UTF8);
                Console.WriteLine($"✓ Session summary written: {outPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ SessionSummary: failed to write JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns the relative path of the most-recently-modified file matching
        /// <paramref name="pattern"/> inside <paramref name="dir"/>, or null.
        /// </summary>
        private static string? LatestFile(string dir, string pattern)
        {
            if (!Directory.Exists(dir)) return null;
            var files = Directory.GetFiles(dir, pattern);
            if (files.Length == 0) return null;
            string latest = files.OrderByDescending(f => File.GetLastWriteTimeUtc(f)).First();
            // return relative to session root's parent (captureSubDir is already relative in callers)
            return latest.Replace('\\', '/').Substring(
                latest.Replace('\\', '/').IndexOf(Path.GetFileName(dir)) );
        }
    }
}
