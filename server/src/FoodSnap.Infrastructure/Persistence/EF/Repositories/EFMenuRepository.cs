using System.Threading.Tasks;
using FoodSnap.Application.Menus;
using FoodSnap.Domain.Menus;
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

        public async Task<Menu> GetById(MenuId id)
        {
            return await context.Menus
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
