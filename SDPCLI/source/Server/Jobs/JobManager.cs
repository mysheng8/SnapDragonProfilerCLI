using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    public class JobManager
    {
        private readonly ConcurrentDictionary<string, Job> _jobs = new ConcurrentDictionary<string, Job>();
        private readonly TimeSpan _ttl;
        private static readonly ContextLogger _log = new ContextLogger("JobManager");

        public JobManager(int ttlMinutes = 60)
        {
            _ttl = TimeSpan.FromMinutes(ttlMinutes);
        }

        public Job Submit(JobType type, Func<Job, CancellationToken, Task> runner)
        {
            PurgeExpired();

            var job = new Job
            {
                Id        = MakeId(type),
                Type      = type,
                Status    = JobStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            _jobs[job.Id] = job;

            Task.Run(async () =>
            {
                job.StartedAt = DateTime.UtcNow;
                job.Status    = JobStatus.Running;
                try
                {
                    await runner(job, job.Cts.Token).ConfigureAwait(false);
                    // runner is responsible for setting Completed / Cancelled
                }
                catch (OperationCanceledException)
                {
                    if (job.Status != JobStatus.Cancelled && job.Status != JobStatus.Cancelling)
                        job.Status = JobStatus.Cancelled;
                }
                catch (Exception ex)
                {
                    job.Status = JobStatus.Failed;
                    job.Error  = ex.Message;
                    _log.Error($"Job {job.Id} failed: {ex.Message}");
                }
                finally
                {
                    if (!job.IsTerminal)
                        job.Status = JobStatus.Failed;
                    job.FinishedAt = DateTime.UtcNow;
                    job.Cts.Dispose();
                }
            });

            return job;
        }

        public Job? Get(string id) =>
            _jobs.TryGetValue(id, out var j) ? j : null;

        public IEnumerable<Job> List() =>
            _jobs.Values.OrderByDescending(j => j.CreatedAt);

        public bool Cancel(string id)
        {
            if (!_jobs.TryGetValue(id, out var job)) return false;
            if (job.IsTerminal) return false;

            job.Cts.Cancel();
            if (job.Status == JobStatus.Pending)
                job.Status = JobStatus.Cancelled;

            return true;
        }

        public bool Remove(string id) =>
            _jobs.TryRemove(id, out _);

        /// <summary>Find a Running/Pending analysis job matching sdpPath + snapshotId.</summary>
        public Job? FindActiveAnalysis(string sdpPath, uint snapshotId)
        {
            return _jobs.Values.FirstOrDefault(j =>
                j.Type == JobType.Analysis &&
                (j.Status == JobStatus.Pending || j.Status == JobStatus.Running || j.Status == JobStatus.Cancelling) &&
                j.Result == null &&   // not yet completed
                j is AnalysisJob aj &&
                string.Equals(aj.SdpPath, sdpPath, StringComparison.OrdinalIgnoreCase) &&
                aj.SnapshotId == snapshotId);
        }

        /// <summary>Submit a typed AnalysisJob carrying request parameters.</summary>
        public AnalysisJob SubmitAnalysis(
            string sdpPath, uint snapshotId, string outputDir,
            Analysis.AnalysisTarget targets,
            Func<Job, System.Threading.CancellationToken, System.Threading.Tasks.Task> runner)
        {
            PurgeExpired();

            var job = new AnalysisJob
            {
                Id         = MakeId(JobType.Analysis),
                Type       = JobType.Analysis,
                Status     = JobStatus.Pending,
                CreatedAt  = DateTime.UtcNow,
                SdpPath    = sdpPath,
                SnapshotId = snapshotId,
                OutputDir  = outputDir,
                TargetsEnum = targets,
            };

            _jobs[job.Id] = job;

            System.Threading.Tasks.Task.Run(async () =>
            {
                job.StartedAt = DateTime.UtcNow;
                job.Status    = JobStatus.Running;
                try
                {
                    await runner(job, job.Cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    if (job.Status != JobStatus.Cancelled && job.Status != JobStatus.Cancelling)
                        job.Status = JobStatus.Cancelled;
                }
                catch (Exception ex)
                {
                    job.Status = JobStatus.Failed;
                    job.Error  = ex.Message;
                    _log.Error($"Job {job.Id} failed: {ex.Message}");
                }
                finally
                {
                    if (!job.IsTerminal) job.Status = JobStatus.Failed;
                    job.FinishedAt = DateTime.UtcNow;
                    job.Cts.Dispose();
                }
            });

            return job;
        }

        public void PurgeExpired()
        {
            var cutoff = DateTime.UtcNow - _ttl;
            foreach (var kv in _jobs)
            {
                var j = kv.Value;
                if (j.IsTerminal && j.FinishedAt.HasValue && j.FinishedAt.Value < cutoff)
                    _jobs.TryRemove(kv.Key, out _);
            }
        }

        private static int _seq;
        private static string MakeId(JobType type)
        {
            string prefix = type switch
            {
                JobType.Connect  => "con",
                JobType.Launch   => "lnc",
                JobType.Capture  => "cap",
                JobType.Analysis => "ana",
                _                => "job"
            };
            string ts = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
            int    n  = Interlocked.Increment(ref _seq);
            return $"{prefix}-{ts}-{n:D3}";
        }
    }

    /// <summary>Typed job carrying analysis request parameters for duplicate detection.</summary>
    public class AnalysisJob : Job
    {
        public string  SdpPath     { get; set; } = "";
        public uint    SnapshotId  { get; set; }
        public string? OutputDir   { get; set; }
        public Analysis.AnalysisTarget TargetsEnum { get; set; } = Analysis.AnalysisTarget.All;
    }
}
