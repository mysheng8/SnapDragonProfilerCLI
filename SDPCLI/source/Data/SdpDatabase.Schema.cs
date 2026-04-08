using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Data
{
    public sealed partial class SdpDatabase
    {
        // ── Static schema helpers (accept an open connection; caller owns lifecycle) ──

        public static bool TableExists(SQLiteConnection conn, string tableName)
        {
            using var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@n", conn);
            cmd.Parameters.AddWithValue("@n", tableName);
            return (long)cmd.ExecuteScalar() > 0;
        }

        public static bool ColumnExists(SQLiteConnection conn, string tableName, string columnName)
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
        public static string? FindColumn(SQLiteConnection conn, string tableName, params string[] substrings)
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

        public static string GetShaderStageName(uint stage) => stage switch
        {
            0x00000001 => "Vertex",
            0x00000002 => "TessellationControl",
            0x00000004 => "TessellationEvaluation",
            0x00000008 => "Geometry",
            0x00000010 => "Fragment",
            0x00000020 => "Compute",
            _ => $"Stage_{stage}"
        };

        public static string GetFormatName(uint fmt) => fmt switch
        {
            9   => "R8_UNORM",
            16  => "R8G8_UNORM",
            37  => "R8G8B8A8_UNORM",
            43  => "R8G8B8A8_SRGB",
            44  => "B8G8R8A8_UNORM",
            50  => "B8G8R8A8_SRGB",
            70  => "R16_UNORM",
            76  => "R16_SFLOAT",
            83  => "R16G16_SFLOAT",
            91  => "R16G16B16A16_UNORM",
            97  => "R16G16B16A16_SFLOAT",
            98  => "R32_UINT",
            100 => "R32_SFLOAT",
            103 => "R32G32_SFLOAT",
            106 => "R32G32B32_SFLOAT",
            109 => "R32G32B32A32_SFLOAT",
            122 => "B10G11R11_UFLOAT_PACK32",
            123 => "E5B9G9R9_UFLOAT_PACK32",
            124 => "D16_UNORM",
            125 => "X8_D24_UNORM_PACK32",
            126 => "D32_SFLOAT",
            127 => "S8_UINT",
            128 => "D16_UNORM_S8_UINT",
            129 => "D24_UNORM_S8_UINT",
            130 => "D32_SFLOAT_S8_UINT",
            131 => "BC1_RGB_UNORM",
            133 => "BC1_RGBA_UNORM",
            135 => "BC2_UNORM",
            137 => "BC3_UNORM",
            139 => "BC4_UNORM",
            141 => "BC5_UNORM",
            143 => "BC6H_UFLOAT",
            144 => "BC6H_SFLOAT",
            145 => "BC7_UNORM",
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

        /// <summary>int overload for TextureExtractor compatibility.</summary>
        public static string GetFormatName(int fmt) => GetFormatName((uint)fmt);

        // ── Instance helpers: require connection to query schema ──────────────

        public List<string> GetAllTables()
        {
            var tables = new List<string>();
            using var conn = OpenConnection();
            using var cmd  = new SQLiteCommand(
                "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                tables.Add(r.GetString(0));
            return tables;
        }

        public DatabaseMetadata GetMetadata()
        {
            var meta = new DatabaseMetadata
            {
                TableNames    = GetAllTables(),
                CaptureCount  = 1,
                RecordCounts  = new Dictionary<string, int>()
            };
            return meta;
        }

        // ── Pre-flight validation ─────────────────────────────────────────────

        /// <summary>
        /// Checks that all tables required for analysis are present.
        /// Logs FATAL / ERROR / WARNING messages via <paramref name="logger"/>.
        /// Throws <see cref="InvalidOperationException"/> if no DrawCall source
        /// exists at all (FATAL condition).
        /// </summary>
        public void ValidateForAnalysis(ILogger logger)
        {
            using var conn = OpenConnection();

            // ── Group 1: at least one DC source MUST exist ─────────────────
            bool hasParams    = TableExists(conn, "DrawCallParameters");
            bool hasScope     = TableExists(conn, "SCOPEDrawStages");
            bool hasPipelines = TableExists(conn, "VulkanSnapshotGraphicsPipelines");

            if (!hasParams && !hasScope && !hasPipelines)
                throw new InvalidOperationException(
                    "FATAL: No DrawCall source table found " +
                    "(DrawCallParameters, SCOPEDrawStages, VulkanSnapshotGraphicsPipelines all absent). " +
                    "The SDP file may be incomplete or corrupt.");

            // ── Group 2: CSV-imported tables ──────────────────────────────
            if (!hasParams)
                logger.Warning(
                    "  ⚠ [ERROR]  Table 'DrawCallParameters' missing — DC list will be inaccurate.\n" +
                    "             → Run CSV import first: SDPCLI.exe import <sdpPath>\n" +
                    "             → Or set AutoImportCsv=true in config.ini");

            CheckCsvTable(conn, logger, "DrawCallBindings",
                "per-DC pipeline and texture binding will be unavailable (PipelineID=0, no TextureIDs)");
            CheckCsvTable(conn, logger, "DrawCallVertexBuffers",
                "vertex buffer bindings unavailable — mesh extraction will produce 0 meshes");
            CheckCsvTable(conn, logger, "DrawCallIndexBuffers",
                "index buffer bindings unavailable — VB-only meshes still possible");
            CheckCsvTable(conn, logger, "DrawCallRenderTargets",
                "render target info will be empty");
            CheckCsvTable(conn, logger, "PipelineVertexInputBindings",
                "vertex input stride/rate missing — mesh geometry accuracy degraded");
            CheckCsvTable(conn, logger, "PipelineVertexInputAttributes",
                "vertex attribute location/format missing — position channel not identified");

            // ── Group 3: native SDP tables ────────────────────────────────
            CheckNativeTable(conn, logger, "VulkanSnapshotGraphicsPipelines",
                "pipeline resolution unavailable");
            CheckNativeTable(conn, logger, "VulkanSnapshotShaderStages",
                "shader stage queries unavailable — shader extraction will be skipped");
            CheckNativeTable(conn, logger, "VulkanSnapshotByteBuffers",
                "CRITICAL — no SPIR-V / texture / buffer binary data; extraction fully disabled");
            CheckNativeTable(conn, logger, "VulkanSnapshotTextures",
                "texture metadata (width/height/format) unavailable");
            CheckNativeTable(conn, logger, "VulkanSnapshotImageViews",
                "image view resolution unavailable — texture IDs may not resolve");

            logger.Info("  ✓ Database table validation complete.");
        }

        private static void CheckCsvTable(SQLiteConnection conn, ILogger logger,
            string tableName, string impact)
        {
            if (!TableExists(conn, tableName))
                logger.Warning($"  ⚠ [WARN/CSV]  '{tableName}' missing — {impact}");
        }

        private static void CheckNativeTable(SQLiteConnection conn, ILogger logger,
            string tableName, string impact)
        {
            if (!TableExists(conn, tableName))
                logger.Warning($"  ⚠ [WARN/SDP]  '{tableName}' missing — {impact}");
        }
    }
}

// ── Data-layer DTO — owned here to avoid circular dependency ──────────────────
namespace SnapdragonProfilerCLI.Data
{
    public class DatabaseMetadata
    {
        public int CaptureCount { get; set; }
        public System.Collections.Generic.List<string> TableNames { get; set; }
            = new System.Collections.Generic.List<string>();
        public System.Collections.Generic.Dictionary<string, int> RecordCounts { get; set; }
            = new System.Collections.Generic.Dictionary<string, int>();
    }
}
