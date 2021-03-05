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
using Web.Services.Events;

namespace Web.Workers
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
                    using var scope = services.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                    await using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var events = await db.Events
                        .Where(x => !x.Handled)
                        .OrderBy(x => x.OccuredAt)
                        .Take(5)
                        .ToListAsync(stoppingToken);

                    foreach (var ev in events)
                    {
                        await ProcessEvent(ev, sender, db, stoppingToken);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessEvent(
            SerialisedEvent serialised,
            ISender sender,
            AppDbContext db,
            CancellationToken stoppingToken)
        {
            logger.LogInformation($"Processing event: {serialised.Type}");

            var ev = (Event)JsonSerializer.Deserialize(
                serialised.Json,
                Type.GetType(serialised.Type)!);

            try
            {
                var result = await sender.Send(ev!, stoppingToken);

                if (result)
                {
                    serialised.Handled = true;
                    await db.SaveChangesAsync(stoppingToken);
                }
                else
                {
                    logger.LogError(result.Error.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
