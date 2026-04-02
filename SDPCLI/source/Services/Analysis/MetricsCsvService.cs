using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnapdragonProfilerCLI.Models;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Loads a Snapdragon Profiler metrics CSV (DrawCall-level export) and returns
    /// a dictionary keyed by DrawCall number (e.g. "1.1.5").
    ///
    /// Expected CSV header (order may vary):
    ///   #, Name, Clocks, Read Total (Bytes), Write Total (Bytes),
    ///   Fragments Shaded, Vertices Shaded, % Shaders Busy,
    ///   % Texture L1 Miss, % Texture L2 Miss, % Texture Fetch Stall,
    ///   Fragment Instructions, Vertex Instructions,
    ///   Texture Memory Read BW (Bytes), Vertex Memory Read (Bytes), ...
    /// </summary>
    public class MetricsCsvService
    {
        /// <summary>
        /// Loads metrics from a CSV file. Returns empty dict if file not found / unparseable.
        /// Key = trimmed DrawCall number string (e.g. "1.1.5").
        /// </summary>
        public Dictionary<string, DrawCallMetrics> LoadMetrics(string csvPath)
        {
            var result = new Dictionary<string, DrawCallMetrics>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(csvPath) || !File.Exists(csvPath))
                return result;

            try
            {
                string[] lines = File.ReadAllLines(csvPath);
                if (lines.Length < 2) return result;

                // Parse header — trim whitespace from column names
                string[] headers = SplitCsv(lines[0]);
                var idx = BuildIndex(headers);

                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] cols = SplitCsv(lines[i]);
                    if (cols.Length == 0) continue;

                    string dcNum = cols[0].Trim();
                    if (string.IsNullOrEmpty(dcNum)) continue;

                    var m = new DrawCallMetrics { DrawCallNumber = dcNum };
                    m.ApiName              = GetStr(cols, idx, "Name");
                    m.Clocks               = GetLong(cols, idx, "Clocks");
                    m.ReadTotalBytes       = GetLong(cols, idx, "Read Total (Bytes)");
                    m.WriteTotalBytes      = GetLong(cols, idx, "Write Total (Bytes)");
                    m.FragmentsShaded      = GetLong(cols, idx, "Fragments Shaded");
                    m.VerticesShaded       = GetLong(cols, idx, "Vertices Shaded");
                    m.ShadersBusyPct       = GetDouble(cols, idx, "% Shaders Busy");
                    m.TexL1MissPct         = GetDouble(cols, idx, "% Texture L1 Miss");
                    m.TexL2MissPct         = GetDouble(cols, idx, "% Texture L2 Miss");
                    m.TexFetchStallPct     = GetDouble(cols, idx, "% Texture Fetch Stall");
                    m.FragmentInstructions = GetLong(cols, idx, "Fragment Instructions");
                    m.VertexInstructions   = GetLong(cols, idx, "Vertex Instructions");
                    m.TexMemReadBytes      = GetLong(cols, idx, "Texture Memory Read BW (Bytes)");
                    m.VertexMemReadBytes   = GetLong(cols, idx, "Vertex Memory Read (Bytes)");

                    result[dcNum] = m;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ MetricsCsvService: failed to load '{csvPath}': {ex.Message}");
            }

            return result;
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static Dictionary<string, int> BuildIndex(string[] headers)
        {
            var d = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
            {
                string h = headers[i].Trim();
                if (!d.ContainsKey(h)) d[h] = i;
            }
            return d;
        }

        private static string[] SplitCsv(string line)
        {
            // Handle quoted fields minimally (profiler CSVs are simple)
            return line.Split(',');
        }

        private static string GetStr(string[] cols, Dictionary<string, int> idx, string col)
        {
            if (!idx.TryGetValue(col, out int i) || i >= cols.Length) return "";
            return cols[i].Trim();
        }

        private static long GetLong(string[] cols, Dictionary<string, int> idx, string col)
        {
            string s = GetStr(cols, idx, col);
            return long.TryParse(s, out long v) ? v : 0;
        }

        private static double GetDouble(string[] cols, Dictionary<string, int> idx, string col)
        {
            string s = GetStr(cols, idx, col);
            return double.TryParse(s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double v) ? v : 0;
        }

        // ── New DB-sourced format ─────────────────────────────────────────────

        /// <summary>
        /// Loads per-draw-call metrics from the pair of CSVs produced by QGLPluginService
        /// that live inside the session folder (same directory as sdp.db):
        ///   DrawCallParameters.csv  — DrawCallApiID, DrawcallIdx, ...
        ///   DrawCallMetrics.csv     — CaptureID, DrawID (=DrawcallIdx), MetricName, Value
        ///
        /// Returns a dictionary keyed by DrawCallApiID string (e.g. "113579"),
        /// matching how DrawCallInfo.DrawCallNumber is populated.
        /// </summary>
        public Dictionary<string, DrawCallMetrics> LoadMetricsFromSession(string sessionDir)
        {
            var result = new Dictionary<string, DrawCallMetrics>(StringComparer.OrdinalIgnoreCase);
            string paramsPath  = Path.Combine(sessionDir, "DrawCallParameters.csv");
            string metricsPath = Path.Combine(sessionDir, "DrawCallMetrics.csv");

            if (!File.Exists(paramsPath) || !File.Exists(metricsPath))
                return result;

            try
            {
                // Step 1: DrawCallParameters.csv → map apiId → drawcallIdx
                //   Header: DrawCallApiID, ApiName, SubmitIdx, CmdBufferIdx, DrawcallIdx, ...
                var apiIdToDrawIdx = new Dictionary<string, uint>();
                var apiIdToName    = new Dictionary<string, string>();

                string[] paramLines = File.ReadAllLines(paramsPath);
                if (paramLines.Length >= 2)
                {
                    string[] ph = SplitCsv(paramLines[0]);
                    var pi = BuildIndex(ph);
                    for (int i = 1; i < paramLines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(paramLines[i])) continue;
                        string[] pc = SplitCsv(paramLines[i]);
                        string apiId = GetStr(pc, pi, "DrawCallApiID").Trim();
                        if (string.IsNullOrEmpty(apiId)) continue;
                        string dcIdxStr = GetStr(pc, pi, "DrawcallIdx").Trim();
                        if (uint.TryParse(dcIdxStr, out uint dcIdx))
                            apiIdToDrawIdx[apiId] = dcIdx;
                        apiIdToName[apiId] = GetStr(pc, pi, "ApiName");
                    }
                }

                // Step 2: DrawCallMetrics.csv → pivot: drawcallIdx → { metricName → value }
                //   Header: CaptureID, DrawID, SubmitCount, ReplayHandleID, MetricID, MetricName, Value
                var drawIdxToMetrics = new Dictionary<uint, Dictionary<string, double>>();

                string[] metricLines = File.ReadAllLines(metricsPath);
                if (metricLines.Length >= 2)
                {
                    string[] mh = SplitCsv(metricLines[0]);
                    var mi = BuildIndex(mh);
                    for (int i = 1; i < metricLines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(metricLines[i])) continue;
                        string[] mc = SplitCsv(metricLines[i]);
                        string drawIdStr = GetStr(mc, mi, "DrawID").Trim();
                        if (!uint.TryParse(drawIdStr, out uint drawId)) continue;
                        string metricName = GetStr(mc, mi, "MetricName").Trim();
                        string valueStr   = GetStr(mc, mi, "Value").Trim();
                        if (string.IsNullOrEmpty(metricName)) continue;
                        double.TryParse(valueStr, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out double val);

                        if (!drawIdxToMetrics.TryGetValue(drawId, out var d))
                            drawIdxToMetrics[drawId] = d = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
                        // Values may appear multiple times (multiple metricIDs for same name) — accumulate
                        if (d.TryGetValue(metricName, out double existing))
                            d[metricName] = existing + val;
                        else
                            d[metricName] = val;
                    }
                }

                // Step 3: join on drawcallIdx → build DrawCallMetrics per apiId
                double G(Dictionary<string, double> d, string key)
                {
                    return d.TryGetValue(key, out double v) ? v : 0.0;
                }

                foreach (var kv in apiIdToDrawIdx)
                {
                    string apiId  = kv.Key;
                    uint   dcIdx  = kv.Value;
                    if (!drawIdxToMetrics.TryGetValue(dcIdx, out var mdict))
                        continue;

                    result[apiId] = new DrawCallMetrics
                    {
                        DrawCallNumber     = apiId,
                        ApiName            = apiIdToName.TryGetValue(apiId, out var n) ? n : "",
                        Clocks             = (long)G(mdict, "Clocks"),
                        ReadTotalBytes     = (long)G(mdict, "Read Total (Bytes)"),
                        WriteTotalBytes    = (long)G(mdict, "Write Total (Bytes)"),
                        FragmentsShaded    = (long)G(mdict, "Fragments Shaded"),
                        VerticesShaded     = (long)G(mdict, "Vertices Shaded"),
                        ShadersBusyPct     = G(mdict, "% Shaders Busy"),
                        TexL1MissPct       = G(mdict, "% Texture L1 Miss"),
                        TexL2MissPct       = G(mdict, "% Texture L2 Miss"),
                        TexFetchStallPct   = G(mdict, "% Texture Fetch Stall"),
                        FragmentInstructions = (long)G(mdict, "Fragment Instructions"),
                        VertexInstructions = (long)G(mdict, "Vertex Instructions"),
                        TexMemReadBytes    = (long)G(mdict, "Texture Memory Read BW (Bytes)"),
                        VertexMemReadBytes = (long)G(mdict, "Vertex Memory Read (Bytes)"),
                    };
                }

                Console.WriteLine($"  LoadMetricsFromSession: {result.Count} DCs matched from {apiIdToDrawIdx.Count} params / {drawIdxToMetrics.Count} metric draw IDs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ LoadMetricsFromSession failed: {ex.Message}");
            }

            return result;
        }
    }
}
