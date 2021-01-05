using System.Threading.Tasks;
using Web.Features.Users;
using Web.Domain.Users;

namespace Web.Data.EF.Repositories
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
