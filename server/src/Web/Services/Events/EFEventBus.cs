using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Data.EF;

namespace Web.Services.Events
{
    public class EFEventBus : IEventBus
    {
        private readonly AppDbContext dbContext;
        private readonly List<Event> events = new();

        public EFEventBus(AppDbContext dbContext, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;

            unitOfWork.Subscribe(Flush);
        }

        private async Task Flush(IDbTransaction transaction)
        {
            var serialised = events.Select(@event =>
            {
                var type = @event.GetType().AssemblyQualifiedName;
                var json = JsonSerializer.Serialize(@event, @event.GetType());

                return new SerialisedEvent()
                {
                    OccuredAt = @event.OccuredAt,
                    Type = type,
                    Json = json,
                };
            });

            await dbContext.Events.AddRangeAsync(serialised);
        }

        public Task Publish(Event @event)
        {
            events.Add(@event);
            return Task.CompletedTask;
        }
    }
}
