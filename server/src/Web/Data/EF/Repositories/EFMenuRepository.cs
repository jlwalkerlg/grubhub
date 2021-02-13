using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Features.Menus;

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
                .Include(x => x.Categories.Where(c =>
                    !Microsoft.EntityFrameworkCore.EF.Property<bool>(c, "isDeleted")))
                .ThenInclude(x => x.Items.Where(i =>
                    !Microsoft.EntityFrameworkCore.EF.Property<bool>(i, "isDeleted")))
                .OrderBy(x => x.RestaurantId)
                .SingleOrDefaultAsync(x => x.RestaurantId == id);
        }
    }
}
