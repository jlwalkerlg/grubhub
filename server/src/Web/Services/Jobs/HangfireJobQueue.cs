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

        public Task Enqueue(Job job, EnqueueOptions options = null, CancellationToken cancellationToken = default)
        {
            options = options ?? new EnqueueOptions();

            client.Enqueue<HangfireJobProcessor>(processor =>
                processor.Process(job, options, default, default));

            return Task.CompletedTask;
        }

        public async Task Enqueue(
            IDictionary<Job, EnqueueOptions> jobs,
            CancellationToken cancellationToken = default)
        {
            foreach (var (job, options) in jobs)
            {
                await Enqueue(job, options, cancellationToken);
            }
        }
    }
}
