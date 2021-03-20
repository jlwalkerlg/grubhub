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

namespace Web.Services.Events
{
    public class EventProcessor : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(3);

        private readonly IServiceProvider services;
        private readonly ILogger<EventProcessor> logger;

        public EventProcessor(IServiceProvider services, ILogger<EventProcessor> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Work(cancellationToken);

                await Task.Delay(Interval, cancellationToken);
            }
        }

        private async Task Work(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            var bus = services.GetRequiredService<IEventBus>();
            await using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var events = await db.Events.OrderBy(x => x.OccuredAt).ToListAsync(cancellationToken);

            foreach (var @event in events)
            {
                logger.LogInformation($"Processing event: {@event.Type}");

                try
                {
                    await ProcessEvent(@event, bus, cancellationToken);
                    await MarkAsProcessed(@event, db, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }
        }

        private static async Task ProcessEvent(SerialisedEvent serialised, IEventBus eventBus, CancellationToken cancellationToken)
        {
            var @event = (Event)JsonSerializer.Deserialize(serialised.Json, Type.GetType(serialised.Type)!);

            await eventBus.Publish(@event, cancellationToken);
        }

        private static async Task MarkAsProcessed(SerialisedEvent @event, AppDbContext db, CancellationToken cancellationToken)
        {
            db.Events.Remove(@event);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
