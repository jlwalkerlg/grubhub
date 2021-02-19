using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web;
using Web.Services;

namespace WebTests.Doubles
{
    public class JobQueueSpy : IJobQueue
    {
        public List<Job> Jobs { get; } = new();
        public Dictionary<Job, int?> MaxAttempts { get; } = new();
        public Dictionary<Job, int> Attempts { get; } = new();

        public Task Enqueue(
            Job job,
            EnqueueOptions options = null,
            CancellationToken cancellationToken = default)
        {
            options = options ?? new EnqueueOptions();

            Jobs.Add(job);
            MaxAttempts.Add(job, options.MaxAttempts);
            Attempts.Add(job, 0);

            return Task.CompletedTask;
        }
    }
}
