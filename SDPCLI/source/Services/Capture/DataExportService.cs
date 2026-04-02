using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Handles post-capture data export (screenshot, session path tracking).
    /// Extracted from Application.ExportData / ExportSnapshotTables / ExportTableToCSV.
    /// </summary>
    public class DataExportService
    {
        private readonly SDPClient _sdpClient;
        private readonly global::Capture? _currentCapture;

        /// <summary>Session path saved during ExportData — read back into Application after calling.</summary>
        public string? LastSessionPath { get; private set; }

        public DataExportService(SDPClient sdpClient, global::Capture? currentCapture)
        {
            _sdpClient = sdpClient;
            _currentCapture = currentCapture;
        }

        public void ExportData(string sessionPath, string captureSubDir)
        {
            Console.WriteLine("\n=== Exporting Data ===");
            Console.WriteLine("All data processing is complete, now exporting...");

            try
            {
                if (_sdpClient != null && _sdpClient.IsInitialized && _sdpClient.SessionManager != null)
                {
                    string dbPath = Path.Combine(sessionPath, "sdp.db");

                    if (!File.Exists(dbPath))
                    {
                        Console.WriteLine($"\n⚠  Database not found: {dbPath}");
                        Console.WriteLine("  Skipping exports");
                        return;
                    }

                    Console.WriteLine($"\nDatabase: {dbPath}");
                    FileInfo dbInfo = new FileInfo(dbPath);
                    Console.WriteLine($"Database size: {dbInfo.Length / 1024.0:F2} KB");

                    // Export snapshot screenshot into captureSubDir
                    if (_currentCapture != null && _currentCapture.IsValid())
                    {
                        try
                        {
                            CaptureProperties captureProps = _currentCapture.GetProperties();
                            uint captureId = captureProps.captureID;

                            Console.WriteLine($"\nExporting snapshot screenshot (Capture ID: {captureId})...");

                            uint[] screenshotBufferTypes = new uint[]
                            {
                                SDPCore.BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT,
                                SDPCore.BUFFER_TYPE_QGL_CAPTURE_SCREENSHOT,
                            };

                            bool screenshotSaved = false;

                            for (int i = 0; i < screenshotBufferTypes.Length; i++)
                            {
                                uint bufferType = screenshotBufferTypes[i];
                                uint bufferSize = _sdpClient.Client!.GetBufferDataSize(captureId, bufferType, 0);

                                if (bufferSize > 0)
                                {
                                    IntPtr bufferData = Marshal.AllocHGlobal((int)bufferSize);

                                    try
                                    {
                                        bool gotData = _sdpClient.Client!.GetBufferData(captureId, bufferType, 0, bufferData, bufferSize);

                                        if (gotData)
                                        {
                                            byte[] imageData = new byte[bufferSize];
                                            Marshal.Copy(bufferData, imageData, 0, (int)bufferSize);

                                            string screenshotPath = Path.Combine(captureSubDir, "snapshot_screenshot.dat");
                                            File.WriteAllBytes(screenshotPath, imageData);

                                            Console.WriteLine($"  ✓ Screenshot saved: {screenshotPath}");
                                            screenshotSaved = true;

                                            Utility.SaveScreenshotAsImage(imageData, captureSubDir);

                                            break;
                                        }
                                    }
                                    finally
                                    {
                                        Marshal.FreeHGlobal(bufferData);
                                    }
                                }
                            }

                            if (!screenshotSaved)
                                Console.WriteLine("  ⚠  No screenshot buffer found");
                        }
                        catch (Exception screenshotEx)
                        {
                            Console.WriteLine($"  ⚠  Could not export screenshot: {screenshotEx.Message}");
                            Console.WriteLine($"     Stack trace: {screenshotEx.StackTrace}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n⚠  No valid capture object for screenshot export");
                    }

                    Console.WriteLine("\n✓ Data export complete");
                    LastSessionPath = captureSubDir;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data: {ex.Message}");
            }
        }

        public void ExportSnapshotTables(string sessionPath, string dbPath)
        {
            try
            {
                if (!File.Exists(dbPath))
                {
                    Console.WriteLine("  ⚠  Database file not found, skipping table export");
                    return;
                }

                uint captureId = 0;
                if (_currentCapture != null && _currentCapture.IsValid())
                    captureId = _currentCapture.GetProperties().captureID;

                Console.WriteLine($"  Exporting data for Capture ID: {captureId}");

                var tablesToExport = new Dictionary<string, string>
                {
                    { "SCOPEDrawStages",                 "GPU Scope DrawCall stages (frame-level performance)" },
                    { "SCOPEDrawStageMetrics",           "GPU Scope DrawCall stage metrics" },
                    { "SCOPEStageMetrics",               "GPU Scope stage metrics" },
                    { "tblGPUScopeMarkers",              "GPU Scope debug markers" },
                    { "tblGPUScopeStages",               "GPU Scope stages" },
                    { "VulkanSnapshotShaderStages",      "Vulkan shader stages used in snapshot" },
                    { "VulkanSnapshotShaderData",        "Vulkan shader binary data" },
                    { "VulkanSnapshotGraphicsPipelines", "Vulkan graphics pipelines" },
                    { "VulkanSnapshotComputePipelines",  "Vulkan compute pipelines" },
                    { "VulkanSnapshotTextures",          "Vulkan textures in snapshot" },
                    { "VulkanSnapshotMemoryBuffers",     "Vulkan memory buffers" },
                    { "DX12SnapshotMetricValue",         "DX12 snapshot metric values" },
                    { "GLESSnapshotShaderStatsData",     "OpenGL ES shader statistics" },
                };

                using (var connection = new System.Data.SQLite.SQLiteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();

                    foreach (var table in tablesToExport)
                    {
                        string tableName = table.Key;
                        string description = table.Value;

                        try
                        {
                            using (var cmd = connection.CreateCommand())
                            {
                                cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE captureID = {captureId}";
                                object result = cmd.ExecuteScalar();
                                int rowCount = Convert.ToInt32(result);

                                if (rowCount > 0)
                                {
                                    Console.WriteLine($"\n  Exporting {tableName} ({rowCount} rows)...");
                                    Console.WriteLine($"    {description}");
                                    string csvPath = Path.Combine(sessionPath, $"{tableName}.csv");
                                    ExportTableToCSV(connection, tableName, captureId, csvPath);
                                    Console.WriteLine($"    ✓ Saved to: {csvPath}");
                                }
                                else
                                {
                                    cmd.CommandText = $"SELECT COUNT(*) FROM {tableName}";
                                    result = cmd.ExecuteScalar();
                                    rowCount = Convert.ToInt32(result);
                                    if (rowCount > 0)
                                        Console.WriteLine($"  ⚠  {tableName} has {rowCount} rows but no captureID filter match");
                                }
                            }
                        }
                        catch (System.Data.SQLite.SQLiteException)
                        {
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"  ⚠  Error exporting {tableName}: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine("\n✓ Snapshot table export complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error exporting snapshot tables: {ex.Message}");
            }
        }

        private void ExportTableToCSV(System.Data.SQLite.SQLiteConnection connection, string tableName, uint captureId, string csvPath)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT * FROM {tableName} WHERE captureID = {captureId}";

                using (var reader = cmd.ExecuteReader())
                {
                    using (var writer = new StreamWriter(csvPath))
                    {
                        var columnNames = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            columnNames.Add(reader.GetName(i));
                        writer.WriteLine(string.Join(",", columnNames));

                        int rowCount = 0;
                        while (reader.Read())
                        {
                            var values = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object value = reader.GetValue(i);
                                if (value == DBNull.Value || value == null)
                                {
                                    values.Add("");
                                }
                                else if (value is byte[])
                                {
                                    values.Add($"<binary:{((byte[])value).Length} bytes>");
                                }
                                else
                                {
                                    string strValue = value.ToString()?.Replace("\"", "\"\"") ?? "";
                                    if (strValue.Contains(",") || strValue.Contains("\"") || strValue.Contains("\n"))
                                        strValue = $"\"{strValue}\"";
                                    values.Add(strValue);
                                }
                            }
                            writer.WriteLine(string.Join(",", values));
                            rowCount++;
                        }

                        if (rowCount == 0)
                            Console.WriteLine($"    ⚠  Warning: no rows written");
                    }
                }
            }
        }
    }
}
