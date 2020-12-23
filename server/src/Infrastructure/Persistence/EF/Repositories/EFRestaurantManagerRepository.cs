using System.Threading.Tasks;
using Application.Users;
using Domain.Users;

namespace Infrastructure.Persistence.EF.Repositories
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
