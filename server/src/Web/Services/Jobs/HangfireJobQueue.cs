using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;

namespace Web.Services.Jobs
{
    public class HangfireJobQueue : IJobQueue
    {
        private readonly IBackgroundJobClient client;

        public HangfireJobQueue(IBackgroundJobClient client)
        {
            this.client = client;
        }

        private Task Enqueue(Job job, CancellationToken cancellationToken = default)
        {
            client.Enqueue<HangfireJobProcessor>(processor =>
                processor.Process(job, default, default));

            return Task.CompletedTask;
        }

        public async Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
        {
            foreach (var job in jobs)
            {
                await Enqueue(job, cancellationToken);
            }
        }
    }
}
