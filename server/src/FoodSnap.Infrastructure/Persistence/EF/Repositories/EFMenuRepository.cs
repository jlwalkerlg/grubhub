using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Menus;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using Microsoft.EntityFrameworkCore;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
{
    public class EFMenuRepository : IMenuRepository
    {
        private readonly AppDbContext context;

        public EFMenuRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Menu menu)
        {
            await context.Menus.AddAsync(menu);
        }

        public async Task<Menu> GetByRestaurantId(RestaurantId id)
        {
            return await context.Menus
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .OrderBy(x => x.RestaurantId) // needed to suppress ef core warning
                .SingleOrDefaultAsync(x => x.RestaurantId == id);
        }
    }
}
