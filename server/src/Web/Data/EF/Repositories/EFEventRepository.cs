using System.Text.Json;
using System.Threading.Tasks;
using Web.Services.Events;

namespace Web.Data.EF.Repositories
{
    public class EFEventRepository : IEventRepository
    {
        private readonly AppDbContext context;

        public EFEventRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add<TEvent>(TEvent ev) where TEvent : Event
        {
            var type = ev.GetType().AssemblyQualifiedName;
            var json = JsonSerializer.Serialize(ev, ev.GetType());

            var serialized = new SerialisedEvent()
            {
                OccuredAt = ev.OccuredAt,
                Type = type,
                Json = json,
            };

            await context.Events.AddAsync(serialized);
        }
    }
}
