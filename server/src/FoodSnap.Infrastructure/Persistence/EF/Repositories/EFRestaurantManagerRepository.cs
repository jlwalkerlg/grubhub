using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;

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
    }
}
