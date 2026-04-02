using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Imports the CSV files produced by VulkanSnapshotModel.Export* methods
    /// into the session's sdp.db as custom tables.
    ///
    /// Each CSV becomes one table named after the file (without extension).
    /// The table is dropped and recreated on every import so the DB always
    /// reflects the latest capture — no stale rows from previous runs.
    ///
    /// Column types:
    ///   Columns whose name ends with "ID", "Count", "Index", "Offset", "Stride",
    ///   "Binding", "Location", "GroupCount*", or whose values are all integers are
    ///   stored as INTEGER; everything else is TEXT.
    /// </summary>
    public class CsvToDbService
    {
        /// <summary>
        /// Imports all CSV files found in <paramref name="sessionPath"/> into
        /// <paramref name="dbPath"/>.  Only files matching the known export names
        /// are imported; unrecognised CSVs are skipped.
        /// </summary>
        public void ImportAllCsvs(string sessionPath, string dbPath)
        {
            // Known CSV filenames produced by ExportDrawCallData
            var knownFiles = new[]
            {
                "DrawCallBindings.csv",
                "DrawCallRenderTargets.csv",
                "DrawCallParameters.csv",
                "DrawCallVertexBuffers.csv",
                "DrawCallIndexBuffers.csv",
                "PipelineVertexInputBindings.csv",
                "PipelineVertexInputAttributes.csv",
                "DrawCallMetrics.csv",
            };

            Console.WriteLine("\n=== Importing CSVs into sdp.db ===");

            using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            conn.Open();

            int totalRows = 0;
            foreach (var fileName in knownFiles)
            {
                string csvPath = Path.Combine(sessionPath, fileName);
                if (!File.Exists(csvPath))
                {
                    Console.WriteLine($"  skip  {fileName} (not found)");
                    continue;
                }

                try
                {
                    int rows = ImportCsv(conn, csvPath);
                    totalRows += rows;
                    Console.WriteLine($"  ✓  {fileName}  →  {rows} rows");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗  {fileName}  failed: {ex.Message}");
                }
            }

            Console.WriteLine($"Import complete — {totalRows} total rows written");
        }

        // ── Internal ────────────────────────────────────────────────────────

        private static int ImportCsv(SQLiteConnection conn, string csvPath)
        {
            string tableName = Path.GetFileNameWithoutExtension(csvPath);

            // Read all lines (skip empty trailing lines)
            var lines = new List<string>();
            foreach (var line in File.ReadLines(csvPath))
                if (!string.IsNullOrWhiteSpace(line)) lines.Add(line);

            if (lines.Count < 1) return 0;

            // Parse header
            string[] headers = SplitCsvLine(lines[0]);
            if (headers.Length == 0) return 0;

            // Peek at data rows to decide column affinity
            var sampleValues = new List<string[]>();
            for (int i = 1; i < Math.Min(lines.Count, 51); i++)
                sampleValues.Add(SplitCsvLine(lines[i]));

            string[] affinities = InferAffinities(headers, sampleValues);

            // Find CaptureID column index — present in new-format CSVs
            int captureIdCol = -1;
            for (int i = 0; i < headers.Length; i++)
                if (headers[i].Trim().Equals("CaptureID", StringComparison.OrdinalIgnoreCase))
                { captureIdCol = i; break; }

            // Ensure table exists with at least the columns in this CSV
            CreateOrExtendTable(conn, tableName, headers, affinities);

            // Remove existing rows for this capture (idempotent re-import), or wipe all for
            // legacy CSVs that have no CaptureID (preserves old single-capture behaviour).
            if (captureIdCol >= 0 && sampleValues.Count > 0)
            {
                string captureIdVal = sampleValues[0][captureIdCol].Trim();
                if (!string.IsNullOrEmpty(captureIdVal) && long.TryParse(captureIdVal, out _))
                {
                    using var del = new SQLiteCommand(
                        $"DELETE FROM [{tableName}] WHERE CaptureID={captureIdVal}", conn);
                    del.ExecuteNonQuery();
                }
            }
            else
            {
                // Legacy: no CaptureID column — wipe and replace (single-capture behaviour)
                using var del = new SQLiteCommand($"DELETE FROM [{tableName}]", conn);
                del.ExecuteNonQuery();
            }

            // Bulk insert inside one transaction
            int rowCount = 0;
            using var tx = conn.BeginTransaction();

            string colList   = string.Join(", ", Array.ConvertAll(headers, h => $"[{h}]"));
            string paramList = string.Join(", ", BuildParams(headers));
            string sql = $"INSERT INTO [{tableName}] ({colList}) VALUES ({paramList})";

            using var cmd = new SQLiteCommand(sql, conn, tx);
            foreach (var h in headers)
                cmd.Parameters.Add(new SQLiteParameter("@p_" + Sanitise(h)));

            for (int i = 1; i < lines.Count; i++)
            {
                string[] values = SplitCsvLine(lines[i]);
                for (int c = 0; c < headers.Length; c++)
                {
                    string raw = c < values.Length ? values[c] : "";
                    cmd.Parameters["@p_" + Sanitise(headers[c])].Value =
                        affinities[c] == "INTEGER" && long.TryParse(raw, out long n) ? (object)n : raw;
                }
                cmd.ExecuteNonQuery();
                rowCount++;
            }

            tx.Commit();
            return rowCount;
        }

        /// <summary>
        /// Creates the table if it doesn't exist yet; if it does exist, adds any columns
        /// present in <paramref name="headers"/> that are missing from the existing schema
        /// (e.g. CaptureID added after a legacy SDP was first opened).
        /// </summary>
        private static void CreateOrExtendTable(
            SQLiteConnection conn, string tableName, string[] headers, string[] affinities)
        {
            // Check whether table already exists
            long exists;
            using (var chk = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@n", conn))
            {
                chk.Parameters.AddWithValue("@n", tableName);
                exists = (long)chk.ExecuteScalar();
            }

            if (exists == 0)
            {
                // Fresh create
                var cols = new List<string>();
                for (int i = 0; i < headers.Length; i++)
                    cols.Add($"[{headers[i]}] {affinities[i]}");
                string ddl = $"CREATE TABLE [{tableName}] ({string.Join(", ", cols)})";
                using var create = new SQLiteCommand(ddl, conn);
                create.ExecuteNonQuery();
            }
            else
            {
                // Table exists — add missing columns (ALTER TABLE ADD COLUMN is additive-only in SQLite)
                var existingCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                using (var pragma = new SQLiteCommand($"PRAGMA table_info([{tableName}])", conn))
                using (var r = pragma.ExecuteReader())
                    while (r.Read())
                        existingCols.Add(r["name"].ToString() ?? "");

                for (int i = 0; i < headers.Length; i++)
                {
                    if (!existingCols.Contains(headers[i]))
                    {
                        using var alter = new SQLiteCommand(
                            $"ALTER TABLE [{tableName}] ADD COLUMN [{headers[i]}] {affinities[i]}", conn);
                        alter.ExecuteNonQuery();
                    }
                }
            }
        }

        // ── Affinity inference ───────────────────────────────────────────────

        private static readonly HashSet<string> IntegerSuffixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ID", "Count", "Index", "Offset", "Stride", "Binding", "Location",
            "GroupCountX", "GroupCountY", "GroupCountZ",
            "VertexCount", "IndexCount", "InstanceCount",
            "FirstVertex", "FirstIndex", "FirstInstance", "DrawCount", "VertexOffset",
            "AttachmentIndex"
        };

        private static string[] InferAffinities(string[] headers, List<string[]> samples)
        {
            var result = new string[headers.Length];
            for (int c = 0; c < headers.Length; c++)
            {
                string h = headers[c];
                // Check name heuristics first
                bool nameHint = IntegerSuffixes.Contains(h) ||
                                h.EndsWith("ID",    StringComparison.OrdinalIgnoreCase) ||
                                h.EndsWith("Count", StringComparison.OrdinalIgnoreCase) ||
                                h.StartsWith("GroupCount", StringComparison.OrdinalIgnoreCase);

                if (nameHint)
                {
                    result[c] = "INTEGER";
                    continue;
                }

                // Check sample values
                bool allInt = samples.Count > 0;
                bool allReal = samples.Count > 0;
                foreach (var row in samples)
                {
                    string v = c < row.Length ? row[c] : "";
                    if (!string.IsNullOrEmpty(v))
                    {
                        if (!long.TryParse(v, out _))  allInt  = false;
                        if (!double.TryParse(v, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out _))
                            allReal = false;
                    }
                }
                result[c] = allInt ? "INTEGER" : (allReal ? "REAL" : "TEXT");
            }
            return result;
        }

        // ── CSV parsing ──────────────────────────────────────────────────────

        private static string[] SplitCsvLine(string line)
        {
            // Simple RFC-4180 split (handles quoted fields with commas inside)
            var fields = new List<string>();
            bool inQuote = false;
            int start = 0;
            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                if (ch == '"')
                {
                    inQuote = !inQuote;
                }
                else if (ch == ',' && !inQuote)
                {
                    fields.Add(Unquote(line.Substring(start, i - start)));
                    start = i + 1;
                }
            }
            fields.Add(Unquote(line.Substring(start)));
            return fields.ToArray();
        }

        private static string Unquote(string s)
        {
            s = s.Trim();
            if (s.Length >= 2 && s[0] == '"' && s[s.Length - 1] == '"')
                s = s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
            return s;
        }

        private static string Sanitise(string name) =>
            System.Text.RegularExpressions.Regex.Replace(name, @"[^A-Za-z0-9_]", "_");

        private static IEnumerable<string> BuildParams(string[] headers)
        {
            foreach (var h in headers)
                yield return "@p_" + Sanitise(h);
        }
    }
}
