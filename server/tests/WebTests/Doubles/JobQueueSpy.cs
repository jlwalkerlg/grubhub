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
        public Dictionary<Job, int> Attempts { get; } = new();

        public Task Enqueue(Job job, CancellationToken cancellationToken = default)
        {
            return Enqueue(new[] { job }, cancellationToken);
        }

        public Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
        {
            foreach (var job in jobs)
            {
                Jobs.Add(job);
                Attempts.Add(job, 0);
            }

            return Task.CompletedTask;
        }

        public Task<Job> GetNextJob(CancellationToken cancellationToken = default)
        {
            var job = Jobs
                .Where(x => Attempts[x] < x.Retries)
                .FirstOrDefault();

            return Task.FromResult(job);
        }

        public Task MarkComplete(Job job, CancellationToken cancellationToken = default)
        {
            Jobs.Remove(job);
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
