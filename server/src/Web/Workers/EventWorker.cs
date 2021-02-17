using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Data.EF;
using Web.Features.Events;

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

            var dispatcher = sp.GetRequiredService<EventDispatcher>();

            var db = sp.GetRequiredService<AppDbContext>();

            var events = await db.Events
                .Where(x => !x.Handled)
                .OrderBy(x => x.CreatedAt)
                .Take(10)
                .ToListAsync(stoppingToken);

            var i = 0;

            while (!stoppingToken.IsCancellationRequested && i < events.Count)
            {
                var serialised = events[i];

                try
                {
                    var ev = (Event)JsonSerializer.Deserialize(
                        serialised.Json,
                        Type.GetType(serialised.Type));

                    var result = await dispatcher.Dispatch(ev, stoppingToken);

                    if (result)
                    {
                        serialised.Handled = true;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        logger.LogError(result.Error.Message);
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
