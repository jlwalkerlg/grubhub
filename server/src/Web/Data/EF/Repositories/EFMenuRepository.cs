using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Microsoft.EntityFrameworkCore;

namespace Web.Data.EF.Repositories
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
