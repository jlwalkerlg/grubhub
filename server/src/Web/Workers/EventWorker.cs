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
using Web.Services.Events;

namespace Web.Workers
{
    public class EventWorker : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<EventWorker> logger;
        private IServiceScope scope;
        private EventDispatcher dispatcher;
        private AppDbContext db;

        public EventWorker(
            IServiceProvider services,
            ILogger<EventWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            scope = services.CreateScope();
            dispatcher = services.GetRequiredService<EventDispatcher>();
            db = services.GetRequiredService<AppDbContext>();

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
                    var events = await db.Events
                        .Where(x => !x.Handled)
                        .OrderBy(x => x.CreatedAt)
                        .Take(5)
                        .ToListAsync(stoppingToken);

                    foreach (var ev in events)
                    {
                        await ProcessEvent(ev, stoppingToken);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessEvent(SerialisedEvent serialised, CancellationToken stoppingToken)
        {
            var ev = (Event)JsonSerializer.Deserialize(
                serialised.Json,
                Type.GetType(serialised.Type));

            try
            {
                var result = await dispatcher.Dispatch(ev, stoppingToken);

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
            catch (System.Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
