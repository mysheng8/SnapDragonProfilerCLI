using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using QGLPlugin;
using SnapdragonProfilerCLI.Logging;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Services.Capture;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    /// <summary>
    /// Executes the 7-phase GPU snapshot capture flow for server mode.
    /// Transitions DeviceStatus: Capturing → SessionActive (always, even on failure).
    ///
    /// Completion signals from SDK native callbacks:
    ///   CaptureCompleteEvent  ← OnCaptureComplete()  (phase 2, 30s timeout)
    ///   DataProcessedEvent    ← OnDataProcessed(bufferID=2) (phase 3, 180s timeout)
    ///   ImportCompleteEvent   ← OnDataProcessed(bufferID=1) (during ReplayAndGetBuffers, 60s)
    ///
    /// CancellationToken is checked around each WaitOne via Task.WhenAny pattern.
    /// </summary>
    public static class CaptureJobRunner
    {
        private static readonly ContextLogger _log = new ContextLogger("CaptureJob");

        public static async Task RunAsync(
            string? outputDir,
            string? captureLabel,
            Job job,
            DeviceSession session,
            Config config,
            CancellationToken ct)
        {
            try
            {
                if (session.SdpClient == null || session.Device == null || session.ClientDelegate == null)
                    throw new InvalidOperationException("No active session");

                // Reset events from any prior capture
                session.CaptureCompleteEvent.Reset();
                session.DataProcessedEvent.Reset();
                session.ImportCompleteEvent.Reset();
                (session.ClientDelegate as CliClientDelegate)?.ResetDataProcessedCount();

                // ── Phase 1: starting_capture ─────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "starting_capture"; job.Progress = 10;
                _log.Info("Starting capture...");

                var capSvc = new CaptureExecutionService(
                    session.SdpClient,
                    session.Device,
                    session.CurrentCapture,
                    config,
                    session.TargetPackageName,
                    session.TargetProcessPid,
                    session.VerifiedProcessPid,
                    session.RenderingAPI,
                    session.ClientDelegate);

                bool started = await Task.Run(() => capSvc.StartCapture(), ct).ConfigureAwait(false);
                if (!started)
                    throw new InvalidOperationException("CaptureExecutionService.StartCapture() failed");

                session.CurrentCapture = capSvc.CurrentCapture;

                // ── Phase 2: waiting_capture (SDK hardware capture, 30s) ───────
                ct.ThrowIfCancellationRequested();
                job.Phase = "waiting_capture"; job.Progress = 20;
                _log.Info("Waiting for capture complete event...");

                bool captureCompleted = await WaitHandleAsync(session.CaptureCompleteEvent, 30_000, ct)
                                             .ConfigureAwait(false);
                if (!captureCompleted)
                    throw new TimeoutException("Capture did not complete within 30 seconds");

                (uint providerId, uint captureId) =
                    (session.ClientDelegate as CliClientDelegate)!.GetLastCompletedCapture();
                _log.Info($"Capture complete: provider={providerId} captureId={captureId}");

                (session.ClientDelegate as CliClientDelegate)!.SetExpectedCaptureId(captureId);

                // ── Phase 3: waiting_data (API buffer ready, 180s) ─────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "waiting_data"; job.Progress = 35;
                _log.Info("Waiting for API data (bufferID=2)...");

                bool dataReady = await WaitHandleAsync(session.DataProcessedEvent, 180_000, ct)
                                      .ConfigureAwait(false);
                if (!dataReady)
                    _log.Warning("API data not received within 180s — replay may produce empty results");

                // Resolve session path
                string? sessionPath = session.SdpClient.SessionManager?.GetSessionPath();
                string baseDir      = sessionPath ?? outputDir ?? Directory.GetCurrentDirectory();
                string captureSubDir = Path.Combine(baseDir, $"snapshot_{captureId}");
                Directory.CreateDirectory(captureSubDir);

                // ── Phase 4: importing (ImportCapture + DB polling) ───────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "importing"; job.Progress = 50;

                BinaryDataPair? dsbBuffer =
                    await Task.Run(() => ReplayAndGetBuffers(captureId, session), CancellationToken.None)
                              .ConfigureAwait(false);

                // ── Phase 5: exporting (CSVs → sdp.db) ───────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "exporting"; job.Progress = 70;

                if (dsbBuffer != null)
                {
                    await Task.Run(() =>
                        ExportDrawCallData(captureId, dsbBuffer, sessionPath ?? baseDir, captureSubDir, session),
                        CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    _log.Warning("dsbBuffer null — skipping DrawCall export");
                }

                // ── Phase 6: screenshot ───────────────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "screenshot"; job.Progress = 85;

                await Task.Run(() =>
                {
                    var exportSvc = new DataExportService(session.SdpClient, session.CurrentCapture);
                    exportSvc.ExportData(sessionPath ?? baseDir, captureSubDir);
                }, CancellationToken.None).ConfigureAwait(false);

                // Reset capture for next round
                session.CurrentCapture = null;

                // ── Phase 7: archiving (.sdp ZIP) ─────────────────────────────
                ct.ThrowIfCancellationRequested();
                job.Phase = "archiving"; job.Progress = 95;

                string sdpPath = "";
                if (sessionPath != null)
                {
                    await Task.Run(() =>
                    {
                        new SessionArchiveService().CreateSessionArchive(sessionPath);
                    }, CancellationToken.None).ConfigureAwait(false);

                    sdpPath = sessionPath.TrimEnd('\\', '/') + ".sdp";
                }

                // ── Done ──────────────────────────────────────────────────────
                session.CurrentSession?.CaptureIds.Add(captureId);

                job.Result = new
                {
                    sdpPath   = sdpPath,
                    captureId = captureId,
                    sessionId = session.CurrentSession?.SessionId
                };
                job.Status   = JobStatus.Completed;
                job.Progress = 100;
                _log.Info($"Capture job completed: captureId={captureId}");
            }
            catch (OperationCanceledException)
            {
                _log.Warning("Capture job cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _log.Error($"Capture failed: {ex.Message}");
                throw;
            }
            finally
            {
                session.TryTransition(DeviceStatus.Capturing, DeviceStatus.SessionActive);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>Wraps ManualResetEvent.WaitOne with CancellationToken support.</summary>
        private static async Task<bool> WaitHandleAsync(
            ManualResetEvent mre, int timeoutMs, CancellationToken ct)
        {
            var waitTask   = Task.Run(() => mre.WaitOne(timeoutMs));
            var cancelTask = Task.Delay(Timeout.Infinite, ct);
            var winner     = await Task.WhenAny(waitTask, cancelTask).ConfigureAwait(false);
            ct.ThrowIfCancellationRequested();
            return await waitTask.ConfigureAwait(false);
        }

        private static BinaryDataPair? ReplayAndGetBuffers(uint captureId, DeviceSession session)
        {
            if (session.CurrentCapture == null || !session.CurrentCapture.IsValid())
            {
                _log.Warning("Capture invalid — skipping replay");
                return null;
            }

            string? sessionPath = session.SdpClient?.SessionManager?.GetSessionPath();
            if (sessionPath == null) { _log.Warning("No session path"); return null; }

            string dbPath = Path.Combine(sessionPath, "sdp.db");
            _log.Info($"ImportCapture captureId={captureId} db={dbPath}");

            if (!QGLPluginService.ImportCapture(captureId, dbPath))
            {
                _log.Warning("ImportCapture returned false");
                return null;
            }

            // Poll DB for stability
            int lastRows = -1, stable = 0;
            for (int i = 0; i < 90; i++)
            {
                Thread.Sleep(1000);
                try
                {
                    using var conn = new System.Data.SQLite.SQLiteConnection(
                        "Data Source=" + dbPath + ";Version=3;");
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM VulkanSnapshotGraphicsPipelines";
                    int rows = Convert.ToInt32(cmd.ExecuteScalar());
                    if (rows == lastRows && ++stable >= 3) { _log.Info($"DB stable: {rows} pipelines"); break; }
                    else if (rows != lastRows) stable = 0;
                    lastRows = rows;
                }
                catch { /* DB not ready yet */ }
            }

            // Wait for ImportComplete trailing event (DSB availability)
            bool dsbReady = session.ImportCompleteEvent.WaitOne(TimeSpan.FromSeconds(60));
            if (!dsbReady) _log.Warning("ImportCompleteEvent timeout");

            BinaryDataPair? dsbBuffer = null;
            if (session.ClientDelegate is CliClientDelegate csd)
                dsbBuffer = csd.GetCachedSnapshotDsbBuffer(captureId);

            _log.Info(dsbBuffer != null ? $"DsbBuffer: {dsbBuffer.size}B" : "No DsbBuffer cached");
            return dsbBuffer;
        }

        private static void ExportDrawCallData(
            uint captureId, BinaryDataPair dsbBuffer,
            string sessionPath, string captureSubDir,
            DeviceSession session)
        {
            try
            {
                BinaryDataPair? apiBuffer = null;
                if (session.ClientDelegate is CliClientDelegate csd)
                    apiBuffer = csd.GetCachedSnapshotApiBuffer(captureId);

                if (apiBuffer == null || apiBuffer.size == 0)
                {
                    _log.Warning("SnapshotApiBuffer not available — skipping DrawCall export");
                    return;
                }

                var model = new VulkanSnapshotModel();
                model.LoadSnapshot(captureId, apiBuffer, dsbBuffer);

                string dbPath = Path.Combine(sessionPath, "sdp.db");
                model.ExportDrawCallBindingsToCSV(captureId,       Path.Combine(captureSubDir, "DrawCallBindings.csv"));
                model.ExportRenderTargetsToCSV(captureId,          Path.Combine(captureSubDir, "DrawCallRenderTargets.csv"), dbPath);
                model.ExportDrawCallParametersToCSV(captureId,     Path.Combine(captureSubDir, "DrawCallParameters.csv"));
                model.ExportDrawCallVertexBuffersToCSV(captureId,  Path.Combine(captureSubDir, "DrawCallVertexBuffers.csv"));
                model.ExportDrawCallIndexBuffersToCSV(captureId,   Path.Combine(captureSubDir, "DrawCallIndexBuffers.csv"));
                model.ExportPipelineVertexInputStateToCSV(captureId,
                    Path.Combine(captureSubDir, "PipelineVertexInputBindings.csv"),
                    Path.Combine(captureSubDir, "PipelineVertexInputAttributes.csv"));

                if (session.ClientDelegate is CliClientDelegate mcsd)
                {
                    var metricsBuffer = mcsd.GetCachedSnapshotMetricsBuffer(captureId);
                    if (metricsBuffer != null && metricsBuffer.size > 0)
                        QGLPluginService.ExportMetricsToCsv(metricsBuffer, captureId,
                            Path.Combine(captureSubDir, "DrawCallMetrics.csv"));
                }

                new CsvToDbService().ImportAllCsvs(captureSubDir, dbPath);
                _log.Info("DrawCall export complete");
            }
            catch (Exception ex)
            {
                _log.Error("DrawCall export failed: " + ex.Message);
            }
        }
    }
}
