using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Services;

namespace Web.BackgroundServices
{
    public class JobWorker : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<JobWorker> logger;

        public JobWorker(
            IServiceProvider services,
            ILogger<JobWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork(stoppingToken);
                }
                catch (System.Exception ex)
                {
                    logger.LogCritical(ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var queue = sp.GetRequiredService<IJobQueue>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var job = await queue.GetNextJob(stoppingToken);

                    var result = Result.Ok();

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
                    logger.LogCritical(ex.ToString());
                }
            }
        }
    }
}
