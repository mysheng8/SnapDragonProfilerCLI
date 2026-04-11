using System;
using System.Linq;
using System.Threading;
using QGLPlugin;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Services.Capture;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// 简单的ClientDelegate实现，用于处理Client事件
    /// 
    /// CRITICAL: Dual Event Forwarding
    /// --------------------------------
    /// This delegate serves two purposes:
    /// 1. Our own event handling (captureCompleteEvent, dataProcessedEvent)
    /// 2. Forward events to SdpApp.EventsManager for QGLPlugin and other plugins
    /// 
    /// Without the forwarding, QGLPlugin would never receive events like:
    /// - ConnectionEvents.DataProcessed (needed for snapshot processing)
    /// - ConnectionEvents.CaptureCompleted
    /// - ConnectionEvents.ProcessAdded/Removed/StateChanged
    /// 
    /// This allows us to use our own SDPClient (not SdpApp.ConnectionManager)
    /// while still supporting plugins that depend on SdpApp events.
    /// </summary>
    class CliClientDelegate : ClientDelegate
    {
        private ManualResetEvent? captureCompleteEvent;
        private ManualResetEvent? dataProcessedEvent;   // Signal when SDK initial API data (bufferID=2) is ready
        private ManualResetEvent? _importCompleteEvent;  // Signal when ImportCapture device replay finishes (bufferID=1)
        private uint lastCompletedProviderId = 0;
        private uint lastCompletedCaptureId = 0;
        private Capture? realtimeCapture;  // Background realtime capture for process monitoring
        private int dataProcessedCount = 0;  // Track number of OnDataProcessed calls
        
        // Per-captureId buffer cache — 避免多 capture 并发时后一个 capture 的回调覆盖前一个的缓存
        private readonly System.Collections.Generic.Dictionary<uint, BinaryDataPair> _apiBuffers     = new System.Collections.Generic.Dictionary<uint, BinaryDataPair>();
        private readonly System.Collections.Generic.Dictionary<uint, BinaryDataPair> _dsbBuffers     = new System.Collections.Generic.Dictionary<uint, BinaryDataPair>();
        private readonly System.Collections.Generic.Dictionary<uint, BinaryDataPair> _metricsBuffers = new System.Collections.Generic.Dictionary<uint, BinaryDataPair>();

        // 方案C：只有当前期望的 captureId 收到 API data (BufferID=2) 时才 Set dataProcessedEvent
        private volatile uint _expectedCaptureIdForSignal = 0;

        // Target app package name — only this process gets verbose logging in OnProcessAdded/Removed
        private string? _targetPackageName;

        public void SetExpectedCaptureId(uint captureId)
        {
            _expectedCaptureIdForSignal = captureId;
            AppLogger.Info("Delegate", $"  Waiting for API data of capture {captureId} (BufferID=2)");
        }
        
        // Thread-safe dictionary of discovered processes (PID -> name)
        // Updated by OnProcessAdded callback, queried by main thread
        private readonly System.Collections.Concurrent.ConcurrentDictionary<uint, string> discoveredProcesses 
            = new System.Collections.Concurrent.ConcurrentDictionary<uint, string>();
        
        public CliClientDelegate() : base()
        {
            Console.WriteLine("[CliClientDelegate] Constructor called");
            Console.WriteLine($"[CliClientDelegate] Type: {this.GetType().Name}");
            Console.WriteLine($"[CliClientDelegate] Base type: {this.GetType().BaseType?.Name}");
            
            // Verify that our override methods are visible
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            var overrideMethods =methods.Where(m => m.Name.StartsWith("OnClient") || m.Name.StartsWith("OnProcess")).ToList();
            Console.WriteLine($"[CliClientDelegate] Found {overrideMethods.Count} callback methods:");
            foreach (var method in overrideMethods.Take(5))
            {
                Console.WriteLine($"  - {method.Name} (DeclaringType: {method.DeclaringType?.Name})");
            }
        }

        public void SetCaptureCompleteEvent(ManualResetEvent evt)
        {
            captureCompleteEvent = evt;
        }
        
        public void SetDataProcessedEvent(ManualResetEvent evt)
        {
            dataProcessedEvent = evt;
        }

        public void SetImportCompleteEvent(ManualResetEvent evt)
        {
            _importCompleteEvent = evt;
        }

        public void SetTargetPackageName(string packageName)
        {
            _targetPackageName = packageName;
        }

        public override void OnClientConnected()
        {
            AppLogger.Info("Delegate", "[ClientDelegate] Client connected");
            
            // Forward to SdpApp.EventsManager for QGLPlugin and other subscribers
            try
            {
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.DeviceEvents.ClientConnectACK, this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise ClientConnectACK event: {ex.Message}");
            }
            
            AppLogger.Debug("Delegate", "Starting background Realtime capture...");
            
            try
            {
                CaptureManager captureManager = CaptureManager.Get();
                uint realtimeCaptureId = captureManager.CreateCapture(1);
                realtimeCapture = captureManager.GetCapture(realtimeCaptureId);
                
                if (realtimeCapture != null && realtimeCapture.IsValid())
                {
                    CaptureSettings settings = new CaptureSettings(
                        captureType: 1,
                        pid: 0xFFFFFFFE,  // all processes
                        s: 0, d: 0, r: ""
                    );
                    bool started = realtimeCapture.Start(settings);
                    if (started)
                        AppLogger.Info("Delegate", $"✓ Background Realtime capture started (ID: {realtimeCaptureId}) — monitoring all processes");
                    else
                        AppLogger.Warn("Delegate", "Failed to start background Realtime capture — ProcessManager may not discover processes");
                }
                else
                {
                    AppLogger.Warn("Delegate", "Could not get Realtime capture object");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Exception starting Realtime capture: {ex.Message}");
            }
        }
            

        public override void OnClientDisconnected()
        {
            AppLogger.Info("Delegate", "[ClientDelegate] Client disconnected");
            
            try
            {
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.DeviceEvents.ClientDisconnectACK, this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise ClientDisconnectACK event: {ex.Message}");
            }
            
            if (realtimeCapture != null && realtimeCapture.IsValid())
            {
                try
                {
                    realtimeCapture.Stop();
                    AppLogger.Info("Delegate", "Background Realtime capture stopped");
                }
                catch (Exception ex)
                {
                    AppLogger.Warn("Delegate", $"Error stopping Realtime capture: {ex.Message}");
                }
            }
        }

        public override void OnCaptureComplete(uint providerID, uint captureID)
        {
            AppLogger.Info("Delegate", $"[ClientDelegate] Capture completed: Provider={providerID}, Capture={captureID}");
            lastCompletedProviderId = providerID;
            lastCompletedCaptureId = captureID;
            
            try
            {
                var args = new Sdp.CaptureCompletedEventArgs
                {
                    ProviderId = providerID,
                    CaptureId = captureID
                };
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.ConnectionEvents.CaptureCompleted, this, args);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise CaptureCompleted event: {ex.Message}");
            }
            
            if (captureCompleteEvent != null)
            {
                captureCompleteEvent.Set();
            }
        }
        
        public override void OnDataProcessed(uint captureID, uint bufferCategory, uint bufferID, string error)
        {
            // Called when SDPCore finishes processing a buffer (screenshot, metric data, etc.)
            // This is CRITICAL for Snapshot capture - data is not in database until this fires
            dataProcessedCount++;

            if (!string.IsNullOrEmpty(error))
                AppLogger.Warn("Delegate", $"[OnDataProcessed #{dataProcessedCount}] Capture={captureID}, Category=0x{bufferCategory:X}, Buffer={bufferID} — ERROR: {error}");
            else
                AppLogger.Info("Delegate", $"[OnDataProcessed #{dataProcessedCount}] Capture={captureID}, Category=0x{bufferCategory:X}, Buffer={bufferID}");

            // Forward to SdpApp.EventsManager for any remaining subscribers
            try
            {
                var args = new Sdp.DataProcessedEventArgs
                {
                    CaptureID = captureID,
                    BufferID = bufferID,
                    BufferCategory = bufferCategory
                };
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, args);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"  Failed to raise DataProcessed event: {ex.Message}");
            }

            // Cache buffers directly via GetLocalBuffer (does not rely on QGLPlugin static state)
            if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA)
            {
                try
                {
                    var apiBuffer = QGLPluginService.GetLocalBuffer(bufferCategory, bufferID, captureID);
                    if (apiBuffer != null && apiBuffer.size > 0)
                    {
                        lock (_apiBuffers) _apiBuffers[captureID] = apiBuffer;
                        AppLogger.Info("Delegate", $"  ✓ Cached SnapshotApiBuffer[{captureID}] (BufferID={bufferID}, {apiBuffer.size} bytes)");
                    }
                    else
                    {
                        AppLogger.Debug("Delegate", $"  GetLocalBuffer returned null/empty for BufferID={bufferID}");
                    }

                    // Always try to cache DsbBuffer (BufferID=3)
                    if (bufferID != 3)
                    {
                        var dsbBuffer = QGLPluginService.GetLocalBuffer(bufferCategory, 3, captureID);
                        if (dsbBuffer != null && dsbBuffer.size > 0)
                        {
                            lock (_dsbBuffers) _dsbBuffers[captureID] = dsbBuffer;
                            AppLogger.Info("Delegate", $"  ✓ Cached SnapshotDsbBuffer[{captureID}] (BufferID=3, {dsbBuffer.size} bytes)");
                        }
                    }
                }
                catch (Exception cacheEx)
                {
                    AppLogger.Warn("Delegate", $"  Error caching buffers: {cacheEx.Message}");
                }
            }

            // Cache metrics buffer (BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
            if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA)
            {
                try
                {
                    var metricsBuffer = QGLPluginService.GetLocalBuffer(bufferCategory, bufferID, captureID);
                    if (metricsBuffer != null && metricsBuffer.size > 0)
                    {
                        lock (_metricsBuffers) _metricsBuffers[captureID] = metricsBuffer;
                        AppLogger.Info("Delegate", $"  ✓ Cached SnapshotMetricsBuffer[{captureID}] (BufferID={bufferID}, {metricsBuffer.size} bytes)");
                    }
                    else
                    {
                        AppLogger.Debug("Delegate", $"  Metrics GetLocalBuffer returned null/empty for BufferID={bufferID}");
                    }
                }
                catch (Exception cacheEx)
                {
                    AppLogger.Warn("Delegate", $"  Error caching metrics buffer: {cacheEx.Message}");
                }
            }

            // bufferID=2: complete API stream (Number of apis > 0) — SDK initial processing done
            if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA
                && bufferID == 2
                && _expectedCaptureIdForSignal != 0
                && captureID == _expectedCaptureIdForSignal)
            {
                AppLogger.Info("Delegate", $"  ✓ API data ready for capture {captureID} — signaling dataProcessed");
                dataProcessedEvent?.Set();
            }

            // bufferID=1: empty trailing event (Number of apis = 0) — ImportCapture device-side replay complete
            if (bufferCategory == SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA
                && bufferID == 1
                && _expectedCaptureIdForSignal != 0
                && captureID == _expectedCaptureIdForSignal)
            {
                AppLogger.Info("Delegate", $"  ✓ ImportCapture replay done for capture {captureID} (bufferID=1) — signaling importComplete");
                _importCompleteEvent?.Set();
            }
        }
        
        public void ResetDataProcessedCount()
        {
            dataProcessedCount = 0;
            dataProcessedEvent?.Reset();
        }
        
        public int GetDataProcessedCount()
        {
            return dataProcessedCount;
        }

        public override void OnMetricAdded(uint providerID, uint metricID)
        {
            // Called when SDK discovers metrics from a provider
            try
            {
                Metric? metric = MetricManager.Get().GetMetric(metricID);
                if (metric != null && metric.IsValid())
                {
                    MetricProperties props = metric.GetProperties();
                    bool supportsSnapshot = (props.captureTypeMask & 0x04) != 0;
                    AppLogger.Debug("Delegate", $"[MetricAdded] {props.name} (Provider={providerID}, Snapshot={supportsSnapshot})");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"[MetricAdded] Error processing metric: {ex.Message}");
            }
        }
        
        public override void OnProcessAdded(uint pid)
        {
            // Forward to SdpApp.EventsManager for QGLPlugin and other subscribers (always required)
            try
            {
                var args = new Sdp.ProcessEventArgs { PID = pid };
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.ConnectionEvents.ProcessAdded, this, args);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise ProcessAdded event: {ex.Message}");
            }

            try
            {
                Process? proc = ProcessManager.Get().GetProcess(pid);
                if (proc == null || !proc.IsValid()) return;

                ProcessProperties props = proc.GetProperties();
                string processName = props.name;
                discoveredProcesses.TryAdd(pid, processName);

                // Only log details for the target app
                bool isTarget = !string.IsNullOrEmpty(_targetPackageName) && processName.Contains(_targetPackageName);
                if (!isTarget)
                {
                    AppLogger.Debug("Delegate", $"[ProcessAdded] PID={pid} Name={processName}");
                    return;
                }

                AppLogger.Info("Delegate", $">>> [OnProcessAdded] TARGET PID={pid} Name={processName} State={props.state} <<<");

                if (props.state != ProcessState.ProcessRunning)
                    AppLogger.Warn("Delegate", $"  Process state: {props.state} (NOT ProcessRunning)");

                try
                {
                    MetricIDList linkedMetrics = proc.GetLinkedMetrics();
                    if (linkedMetrics.Count == 0)
                    {
                        AppLogger.Warn("Delegate", "  [No linked metrics] Process hasn't used GPU APIs yet");
                    }
                    else
                    {
                        bool supportsSnapshot = false;
                        foreach (uint metricId in linkedMetrics)
                        {
                            try
                            {
                                Metric metric = MetricManager.Get().GetMetric(metricId);
                                if (metric != null && metric.IsValid())
                                {
                                    MetricProperties mp = metric.GetProperties();
                                    AppLogger.Debug("Delegate", $"    - {mp.name} (Mask: 0x{mp.captureTypeMask:X})");
                                    if ((mp.captureTypeMask & 0x04) != 0) supportsSnapshot = true;
                                }
                            }
                            catch { }
                        }
                        if (supportsSnapshot)
                            AppLogger.Info("Delegate", "  ✓ Target process supports Snapshot capture");
                        else
                            AppLogger.Warn("Delegate", "  ? No metrics support Snapshot capture");
                    }
                }
                catch (Exception lmEx)
                {
                    AppLogger.Warn("Delegate", $"  Error checking linked metrics: {lmEx.Message}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Delegate", $"[ProcessAdded] PID={pid} error: {ex.Message}");
            }
        }
        
        public override void OnProcessRemoved(uint pid)
        {
            // Forward to SdpApp.EventsManager for QGLPlugin and other subscribers (always required)
            try
            {
                var args = new Sdp.ProcessEventArgs { PID = pid };
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.ConnectionEvents.ProcessRemoved, this, args);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise ProcessRemoved event: {ex.Message}");
            }

            bool wasTarget = discoveredProcesses.TryRemove(pid, out string? removedName)
                             && !string.IsNullOrEmpty(_targetPackageName)
                             && (removedName?.Contains(_targetPackageName) ?? false);

            if (wasTarget)
                AppLogger.Info("Delegate", $"[ProcessRemoved] TARGET process removed: PID={pid} Name={removedName}");
            else
                AppLogger.Debug("Delegate", $"[ProcessRemoved] PID={pid}");
        }
        
        /// <summary>
        /// Query if a specific PID has been discovered (thread-safe)
        /// </summary>
        public bool IsProcessDiscovered(uint pid)
        {
            return discoveredProcesses.ContainsKey(pid);
        }
        
        /// <summary>
        /// Find process by package name (thread-safe)
        /// </summary>
        public uint? FindProcessByName(string packageName)
        {
            foreach (var kvp in discoveredProcesses)
            {
                if (kvp.Value.Contains(packageName))
                {
                    return kvp.Key;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get count of discovered processes (thread-safe)
        /// </summary>
        public int GetDiscoveredProcessCount()
        {
            return discoveredProcesses.Count;
        }

        public override void OnProcessStateChanged(uint pid)
        {
            // Called when a process changes state (Running/Sleeping/Dead)
            
            // Forward to SdpApp.EventsManager for QGLPlugin and other subscribers
            try
            {
                var args = new Sdp.ProcessEventArgs { PID = pid };
                Sdp.SdpApp.EventsManager.Raise(Sdp.SdpApp.EventsManager.ConnectionEvents.ProcessStateChanged, this, args);
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Delegate", $"Failed to raise ProcessStateChanged event: {ex.Message}");
            }
            
            try
            {
                Process? proc = ProcessManager.Get().GetProcess(pid);
                if (proc != null && proc.IsValid())
                {
                    ProcessProperties props = proc.GetProperties();
                    string processName = props.name;
                    ProcessState state = props.state;

                    // Non-target processes: demote to DEBUG and return early
                    bool isTarget = !string.IsNullOrEmpty(_targetPackageName)
                                    && processName.Contains(_targetPackageName);
                    if (!isTarget)
                    {
                        AppLogger.Debug("Delegate", $"[Process State Changed] PID={pid} Name={processName}, State={state}");
                        return;
                    }

                    // Target process: full INFO logging
                    AppLogger.Info("Delegate", $"[Process State Changed] PID={pid} Name={processName}, State={state}");

                    if (state != ProcessState.ProcessRunning)
                        AppLogger.Warn("Delegate", $"  Process state is {state} (not ProcessRunning) → will be removed");

                    // Check linked metrics (target process only)
                    try
                    {
                        MetricIDList linkedMetrics = proc.GetLinkedMetrics();
                        AppLogger.Debug("Delegate", $"  Linked Metrics: {linkedMetrics.Count}");

                        if (linkedMetrics.Count == 0)
                        {
                            AppLogger.Warn("Delegate", "  [No linked metrics] Target process hasn't used GPU APIs yet");
                        }
                        else
                        {
                            foreach (uint metricId in linkedMetrics)
                            {
                                try
                                {
                                    Metric metric = MetricManager.Get().GetMetric(metricId);
                                    if (metric != null && metric.IsValid())
                                    {
                                        MetricProperties mp = metric.GetProperties();
                                        AppLogger.Debug("Delegate", $"    - {mp.name} (ID: {mp.id}, CaptureTypeMask: 0x{mp.captureTypeMask:X})");
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception lmEx)
                    {
                        AppLogger.Warn("Delegate", $"  Error checking linked metrics: {lmEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Delegate", $"[Process State Changed] PID={pid} - error: {ex.Message}");
            }
        }

        public (uint providerId, uint captureId) GetLastCompletedCapture()
        {
            return (lastCompletedProviderId, lastCompletedCaptureId);
        }
        
        /// <summary>
        /// 获取缓存的 SnapshotApiBuffer（避免依赖 QGLPlugin 实例化）
        /// </summary>
        public BinaryDataPair? GetCachedSnapshotApiBuffer(uint captureId)
        {
            lock (_apiBuffers) return _apiBuffers.TryGetValue(captureId, out var buf) ? buf : null;
        }

        /// <summary>
        /// 获取缓存的 SnapshotDsbBuffer
        /// </summary>
        public BinaryDataPair? GetCachedSnapshotDsbBuffer(uint captureId)
        {
            lock (_dsbBuffers) return _dsbBuffers.TryGetValue(captureId, out var buf) ? buf : null;
        }

        /// <summary>
        /// 获取缓存的 SnapshotMetricsBuffer（BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA）
        /// </summary>
        public BinaryDataPair? GetCachedSnapshotMetricsBuffer(uint captureId)
        {
            lock (_metricsBuffers) return _metricsBuffers.TryGetValue(captureId, out var buf) ? buf : null;
        }
    }
}
