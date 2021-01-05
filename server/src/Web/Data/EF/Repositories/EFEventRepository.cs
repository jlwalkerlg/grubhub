using System.Threading.Tasks;
using Web.Features.Events;

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
            await context.Events.AddAsync(new EventDto(ev));
        }
    }
}
