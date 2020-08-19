using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
{
    public class EFRestaurantManagerRepository : IRestaurantManagerRepository
    {
        private readonly AppDbContext context;

        public EFRestaurantManagerRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(RestaurantManager manager)
        {
            await context.RestaurantManagers.AddAsync(manager);
        }

        public async Task<bool> EmailExists(string email)
        {
            var count = await context.RestaurantManagers
                .Where(x => x.Email.Address == email)
                .CountAsync();

            return count > 0;
        }
    }
}
