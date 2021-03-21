using System.Text.Json;
using System.Threading.Tasks;
using Web.Data.EF;

namespace Web.Services.Events
{
    public class EFOutbox : IOutbox
    {
        private readonly AppDbContext context;

        public EFOutbox(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Event @event)
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
