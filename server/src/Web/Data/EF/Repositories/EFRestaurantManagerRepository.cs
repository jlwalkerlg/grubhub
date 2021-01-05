using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Features.Users;

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
