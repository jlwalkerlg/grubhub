using System.Collections.Generic;
using System.Linq;
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
            return Enqueue(new[] { job }, options, cancellationToken);
        }

        public Task Enqueue(
            IEnumerable<Job> jobs,
            EnqueueOptions options = null,
            CancellationToken cancellationToken = default)
        {
            options = options ?? new EnqueueOptions();

            foreach (var job in jobs)
            {
                Jobs.Add(job);
                MaxAttempts.Add(job, options.MaxAttempts);
                Attempts.Add(job, 0);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Job>> GetNextNJobs(
            int n, CancellationToken cancellationToken = default)
        {
            var jobs = Jobs
                .Where(x => MaxAttempts[x] is null || Attempts[x] < MaxAttempts[x])
                .Take(n);

            return Task.FromResult(jobs);
        }

        public Task MarkComplete(Job job, CancellationToken cancellationToken = default)
        {
            Jobs.Remove(job);
            MaxAttempts.Remove(job);
            Attempts.Remove(job);
            return Task.CompletedTask;
        }

        public Task RegisterFailedAttempt(
            Job job, CancellationToken cancellationToken = default)
        {
            Attempts[job]++;
            return Task.CompletedTask;
        }
    }
}
