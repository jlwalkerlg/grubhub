using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Menus;
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

        public async Task<Menu> GetByRestaurantId(Guid id)
        {
            return await context.Menus
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .OrderBy(x => x.RestaurantId)
                .SingleOrDefaultAsync(x => x.RestaurantId == id);
        }
    }
}
