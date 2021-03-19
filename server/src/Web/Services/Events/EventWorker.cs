using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Data.EF;

namespace Web.Services.Events
{
    public class EventWorker : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(3);

        private readonly IServiceProvider services;
        private readonly ILogger<EventWorker> logger;

        public EventWorker(IServiceProvider services, ILogger<EventWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = services.CreateScope();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                await using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var events = await db.Events
                    .OrderBy(x => x.OccuredAt)
                    .ToListAsync(stoppingToken);

                foreach (var ev in events)
                {
                    logger.LogInformation($"Processing event: {ev.Type}");

                    try
                    {
                        await ProcessEvent(ev, publisher, db, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task ProcessEvent(
            SerialisedEvent serialised,
            IPublisher publisher,
            AppDbContext db,
            CancellationToken stoppingToken)
        {
            var ev = (Event)JsonSerializer.Deserialize(
                serialised.Json,
                Type.GetType(serialised.Type)!);

            await publisher.Publish(ev!, stoppingToken);

            db.Events.Remove(serialised);
            await db.SaveChangesAsync(stoppingToken);
        }
    }
}
