using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Data.EF;

namespace Web.BackgroundServices
{
    public class EventWorker : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<EventWorker> logger;

        public EventWorker(
            IServiceProvider services,
            ILogger<EventWorker> logger)
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
                catch (System.Exception e)
                {
                    logger.LogCritical(e.ToString());
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var processor = sp.GetRequiredService<EventProcessor>();
            var db = sp.GetRequiredService<AppDbContext>();

            var events = db.Events
                .Where(x => !x.Handled)
                .OrderBy(x => x.CreatedAt)
                .Take(10);

            // TODO: wrap in transaction?

            foreach (var e in events)
            {
                await processor.ProcessEvent(e);
            }

            await db.SaveChangesAsync(stoppingToken);
        }
    }
}
