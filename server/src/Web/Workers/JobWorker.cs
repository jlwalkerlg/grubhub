using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Services;

namespace Web.Workers
{
    public class JobWorker : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<JobWorker> logger;
        private IServiceScope scope;
        private IJobQueue queue;
        private ISender sender;

        public JobWorker(
            IServiceProvider services,
            ILogger<JobWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            scope = services.CreateScope();
            queue = scope.ServiceProvider.GetRequiredService<IJobQueue>();
            sender = scope.ServiceProvider.GetRequiredService<ISender>();

            return base.StartAsync(cancellationToken);
        }

        public override void Dispose()
        {
            scope.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var jobs = await queue.GetNextNJobs(10, stoppingToken);

                    foreach (var job in jobs)
                    {
                        await ProcessJob(job, stoppingToken);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessJob(Job job, CancellationToken stoppingToken)
        {
            try
            {
                var result = await sender.Send(job);

                if (result)
                {
                    await queue.MarkComplete(job, stoppingToken);
                }
                else
                {
                    await queue.RegisterFailedAttempt(job, stoppingToken);
                }
            }
            catch (System.Exception ex)
            {
                await queue.RegisterFailedAttempt(job, stoppingToken);
                logger.LogError(ex.ToString());
            }
        }
    }
}
