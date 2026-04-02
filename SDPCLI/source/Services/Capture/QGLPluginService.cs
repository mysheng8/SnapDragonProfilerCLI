using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Single access point for the C++ QGLPluginProcessor.
    ///
    /// Replaces the repeated pattern:
    ///     ProcessorPluginMgr.Get().GetPlugin("SDP::QGLPluginProcessor")
    ///
    /// The processor is loaded automatically by SDPClient.Initialize() (C++ side).
    /// This class is stateless — no instance state, safe to call from anywhere.
    /// </summary>
    public static class QGLPluginService
    {
        private const string PluginName = "SDP::QGLPluginProcessor";

        /// <summary>
        /// Returns the QGLPluginProcessor, or null if not yet loaded.
        /// Callers should treat null as "plugin unavailable, skip replay".
        /// </summary>
        public static SDPProcessorPlugin? GetProcessor()
        {
            try
            {
                return ProcessorPluginMgr.Get().GetPlugin(PluginName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ QGLPluginService.GetProcessor failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Triggers gfxr replay for <paramref name="captureId"/>, which populates
        /// sdp.db (VulkanSnapshot* tables) and fires OnDataProcessed callbacks.
        /// </summary>
        /// <returns>True if ImportCapture returned true, false otherwise.</returns>
        public static bool ImportCapture(uint captureId, string dbPath, int flags = 1)
        {
            var plugin = GetProcessor();
            if (plugin == null)
            {
                Console.WriteLine("  ⚠ ImportCapture skipped: QGLPluginProcessor not available");
                return false;
            }

            try
            {
                bool ok = plugin.ImportCapture(captureId, captureId, dbPath, flags, 0, 9524);
                if (!ok) Console.WriteLine("  ⚠ ImportCapture returned false");
                return ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ ImportCapture threw: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Fetches a raw binary buffer from the plugin cache (e.g. ApiBuffer or DsbBuffer).
        /// </summary>
        /// <returns>The buffer, or null if unavailable.</returns>
        public static BinaryDataPair? GetLocalBuffer(uint bufferCategory, uint bufferId, uint captureId)
        {
            var plugin = GetProcessor();
            if (plugin == null) return null;

            try
            {
                return plugin.GetLocalBuffer(bufferCategory, bufferId, captureId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ GetLocalBuffer({bufferId}) failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>Returns true if the processor plugin is currently registered.</summary>
        public static bool IsAvailable()
        {
            try { return ProcessorPluginMgr.Get().GetPlugin(PluginName) != null; }
            catch { return false; }
        }

        /// <summary>
        /// Parses the BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA binary blob and writes
        /// a long-format CSV:   CaptureID, DrawID, SubmitCount, ReplayHandleID, MetricID, MetricName, Value
        ///
        /// This mirrors the internal VkMetricsCapturedModel parsing logic without
        /// depending on QGLPlugin's internal static buffers.
        /// </summary>
        /// <returns>The path written, or null on failure.</returns>
        public static string? ExportMetricsToCsv(BinaryDataPair metricsBuffer, uint captureId, string csvPath)
        {
            if (metricsBuffer == null || metricsBuffer.size == 0)
            {
                Console.WriteLine("  ExportMetricsToCsv: buffer is null/empty — skipping");
                return null;
            }

            try
            {
                int structSize = Marshal.SizeOf<VulkanSnapshotMetricLayout>();
                long count = (long)(metricsBuffer.size / (uint)structSize);
                if (count == 0)
                {
                    Console.WriteLine("  ExportMetricsToCsv: no metric entries in buffer");
                    return null;
                }

                // Resolve metric names once per metricID
                var metricNames = new Dictionary<uint, string>();
                string GetMetricName(uint id)
                {
                    if (metricNames.TryGetValue(id, out var cached)) return cached;
                    string name;
                    try
                    {
                        Metric m = MetricManager.Get().GetMetric(id);
                        name = (m != null && m.IsValid()) ? m.GetProperties().name : VkHelper.GetImportedMetricName(id);
                        if (string.IsNullOrEmpty(name)) name = $"Metric_{id}";
                    }
                    catch { name = $"Metric_{id}"; }
                    metricNames[id] = name;
                    return name;
                }

                // Parse and write
                Directory.CreateDirectory(Path.GetDirectoryName(csvPath)!);
                using var sw = new StreamWriter(csvPath, false, Encoding.UTF8);
                sw.WriteLine("CaptureID,DrawID,SubmitCount,ReplayHandleID,MetricID,MetricName,Value");

                IntPtr ptr = metricsBuffer.data;
                for (long i = 0; i < count; i++, ptr += structSize)
                {
                    var entry = Marshal.PtrToStructure<VulkanSnapshotMetricLayout>(ptr);
                    string metricName = GetMetricName(entry.metricID);
                    // Escape metric name for CSV
                    string safeName = metricName.Contains(",") ? $"\"{metricName}\"" : metricName;
                    sw.WriteLine($"{entry.captureID},{entry.drawID},{entry.cmdBuffSubmitCount},{entry.replayHandleID},{entry.metricID},{safeName},{entry.value:R}");
                }

                Console.WriteLine($"  Metrics CSV written: {Path.GetFileName(csvPath)} ({count} entries)");
                return csvPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ExportMetricsToCsv failed: {ex.Message}");
                return null;
            }
        }

        // Mirrors QGLPlugin.QGLPlugin.VulkanSnapshotMetric (public nested struct).
        // Sequential layout ensures Marshal.PtrToStructure reads the correct byte offsets.
        [StructLayout(LayoutKind.Sequential)]
        private struct VulkanSnapshotMetricLayout
        {
            public uint  captureID;
            public uint  metricID;
            public ulong drawID;
            public uint  cmdBuffSubmitCount;
            public ulong replayHandleID;
            public double value;
        }
    }
}
