using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using SnapdragonProfilerCLI.Data;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Models;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// Loads per-draw-call GPU performance metrics from the sdp.db tables
    /// that were populated by <see cref="Capture.CsvToDbService.ImportAllCsvs"/>
    /// at snapshot capture time.
    ///
    /// Required tables (both created by ImportAllCsvs):
    ///   DrawCallParameters  — DrawCallApiID, ApiName, DrawcallIdx, [CaptureID], ...
    ///   DrawCallMetrics     — DrawID (=DrawcallIdx), MetricName, Value, [CaptureID], ...
    ///
    /// Returns a dictionary keyed by DrawCallApiID string (matches DrawCallInfo.DrawCallNumber).
    /// Returns empty dict when either table is absent or has no rows for the capture.
    /// </summary>
    public class MetricsQueryService
    {
        private readonly Config _config;

        public MetricsQueryService(Config config) { _config = config; }

        public Dictionary<string, DrawCallMetrics> LoadMetrics(string dbPath, uint captureId)
        {
            var result = new Dictionary<string, DrawCallMetrics>(StringComparer.OrdinalIgnoreCase);
            try
            {
                // Build MetricName IN (...) filter from MetricsWhitelist — same config key used at
                // capture time (CaptureExecutionService) to decide which counters are recorded in DB.
                string whitelistRaw = _config.Get("MetricsWhitelist", "");
                string metricFilter = "";
                if (!string.IsNullOrWhiteSpace(whitelistRaw))
                {
                    var quoted = string.Join(",",
                        whitelistRaw.Split(',')
                            .Select(s => s.Trim())
                            .Where(s => s.Length > 0)
                            .Select(s => $"'{s.Replace("'", "''")}'")
                    );
                    if (quoted.Length > 0)
                        metricFilter = $" AND MetricName IN ({quoted})";
                }

                var db = new SdpDatabase(dbPath, captureId);
                using var conn = db.OpenConnection();

                if (!SdpDatabase.TableExists(conn, "DrawCallParameters") ||
                    !SdpDatabase.TableExists(conn, "DrawCallMetrics"))
                    return result;

                bool paramHasCaptureId  = SdpDatabase.ColumnExists(conn, "DrawCallParameters", "CaptureID");
                bool metricHasCaptureId = SdpDatabase.ColumnExists(conn, "DrawCallMetrics",     "CaptureID");

                // Step 1: DrawCallParameters → apiId → DrawcallIdx
                var apiIdToDrawIdx = new Dictionary<string, uint>();
                var apiIdToName    = new Dictionary<string, string>();
                {
                    string where = paramHasCaptureId ? $" WHERE CaptureID={captureId}" : "";
                    using var cmd = new SQLiteCommand(
                        $"SELECT DrawCallApiID, ApiName, DrawcallIdx FROM DrawCallParameters{where} ORDER BY rowid",
                        conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        string apiId = r["DrawCallApiID"]?.ToString() ?? "";
                        if (string.IsNullOrEmpty(apiId)) continue;
                        if (uint.TryParse(r["DrawcallIdx"]?.ToString(), out uint dcIdx))
                            apiIdToDrawIdx[apiId] = dcIdx;
                        apiIdToName[apiId] = r["ApiName"]?.ToString() ?? "";
                    }
                }

                if (apiIdToDrawIdx.Count == 0) return result;

                // Step 2: DrawCallMetrics → drawcallIdx → { metricName → accumulated value }
                // Filter by MetricsWhitelist so we only read metrics that were recorded at capture time.
                var drawIdxToMetrics = new Dictionary<uint, Dictionary<string, double>>();
                {
                    string where = (metricHasCaptureId ? $" WHERE CaptureID={captureId}" : " WHERE 1=1")
                                 + metricFilter;
                    using var cmd = new SQLiteCommand(
                        $"SELECT DrawID, MetricName, Value FROM DrawCallMetrics{where}", conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        if (!uint.TryParse(r["DrawID"]?.ToString(), out uint drawId)) continue;
                        string metricName = r["MetricName"]?.ToString()?.Trim() ?? "";
                        if (string.IsNullOrEmpty(metricName)) continue;
                        double.TryParse(r["Value"]?.ToString(),
                            NumberStyles.Any, CultureInfo.InvariantCulture, out double val);

                        if (!drawIdxToMetrics.TryGetValue(drawId, out var d))
                            drawIdxToMetrics[drawId] = d = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
                        d[metricName] = d.TryGetValue(metricName, out double ex) ? ex + val : val;
                    }
                }

                // Step 3: join and build DrawCallMetrics objects — no hardcoded counter names here;
                // all counter values land in All, keyed by original MetricName from DB.
                foreach (var kv in apiIdToDrawIdx)
                {
                    string apiId = kv.Key;
                    uint   dcIdx = kv.Value;
                    if (!drawIdxToMetrics.TryGetValue(dcIdx, out var mdict)) continue;

                    result[apiId] = new DrawCallMetrics
                    {
                        DrawCallNumber = apiId,
                        ApiName        = apiIdToName.TryGetValue(apiId, out var n) ? n : "",
                        All            = mdict,   // All counter values; MetricsWhitelist already filtered in SQL
                    };
                }

                AppLogger.Debug("MetricsQuery", $"{result.Count} DCs matched " +
                    $"({apiIdToDrawIdx.Count} params / {drawIdxToMetrics.Count} metric draw IDs)");
            }
            catch (Exception ex)
            {
                AppLogger.Warn("MetricsQuery", $"LoadMetrics failed: {ex.Message}");
            }
            return result;
        }
    }
}
