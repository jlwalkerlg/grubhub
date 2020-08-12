using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
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
