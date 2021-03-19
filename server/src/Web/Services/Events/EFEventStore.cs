using System.Text.Json;
using System.Threading.Tasks;
using Web.Data.EF;

namespace Web.Services.Events
{
    public class EFEventStore : IEventStore
    {
        private readonly AppDbContext context;

        public EFEventStore(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Store(Event @event)
        {
            var type = @event.GetType().AssemblyQualifiedName;
            var json = JsonSerializer.Serialize(@event, @event.GetType());

            var serialised =  new SerialisedEvent()
            {
                OccuredAt = @event.OccuredAt,
                Type = type,
                Json = json,
            };

            await context.Events.AddAsync(serialised);
        }
    }
}
