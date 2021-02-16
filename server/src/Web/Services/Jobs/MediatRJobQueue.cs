using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Web.Services.Jobs
{
    public class MediatRJobQueue : IJobQueue
    {
        private readonly IPublisher publisher;
        private readonly ILogger<MediatRJobQueue> logger;

        public MediatRJobQueue(IPublisher publisher, ILogger<MediatRJobQueue> logger)
        {
            this.publisher = publisher;
            this.logger = logger;
        }

        public Task<Job> Dequeue()
        {
            throw new System.NotImplementedException();
        }

        public async Task Enqueue(Job job)
        {
            while (!job.IsComplete && !job.Failed)
            {
                job.RegisterAttempt();

                try
                {
                    await publisher.Publish(job);
                    job.MarkComplete();
                }
                catch (System.Exception e)
                {
                    logger.LogCritical(e.ToString());
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }
        }
    }
}
