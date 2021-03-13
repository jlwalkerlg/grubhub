using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web;
using Web.Services;
using Web.Services.Jobs;

namespace WebTests.Doubles
{
    public class JobQueueSpy : IJobQueue
    {
        public List<Job> Jobs { get; } = new();

        public Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
        {
            foreach (var job in jobs)
            {
                Jobs.Add(job);
            }

            return Task.CompletedTask;
        }
    }
}
