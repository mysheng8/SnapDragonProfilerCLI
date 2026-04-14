using System;
using System.Threading;
using Newtonsoft.Json;

namespace SnapdragonProfilerCLI.Server.Jobs
{
    public class Job
    {
        public string    Id         { get; set; } = "";
        public JobType   Type       { get; set; }
        public JobStatus Status     { get; set; } = JobStatus.Pending;
        public string?   Phase      { get; set; }
        public int       Progress   { get; set; }
        public DateTime  CreatedAt  { get; set; }
        public DateTime? StartedAt  { get; set; }
        public DateTime? FinishedAt { get; set; }
        public object?   Result     { get; set; }
        public string?   Error      { get; set; }

        [JsonIgnore]
        public CancellationTokenSource Cts { get; } = new CancellationTokenSource();

        public bool IsTerminal =>
            Status == JobStatus.Completed ||
            Status == JobStatus.Failed    ||
            Status == JobStatus.Cancelled;

        /// <summary>Returns a serialisable summary safe for JSON responses.</summary>
        public object ToSummary() => new
        {
            id         = Id,
            type       = Type.ToString(),
            status     = Status.ToString(),
            phase      = Phase,
            progress   = Progress,
            createdAt  = CreatedAt,
            startedAt  = StartedAt,
            finishedAt = FinishedAt,
            result     = Result,
            error      = Error,
        };
    }
}
