namespace SnapdragonProfilerCLI.Server.Jobs
{
    public enum JobStatus
    {
        Pending,
        Running,
        Cancelling,   // analysis only: current phase still running, no more phases after it
        Completed,
        Failed,
        Cancelled
    }
}
