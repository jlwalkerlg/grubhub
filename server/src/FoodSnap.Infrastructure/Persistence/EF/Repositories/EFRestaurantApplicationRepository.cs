using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
{
    public class EFRestaurantApplicationRepository : IRestaurantApplicationRepository
    {
        private readonly AppDbContext context;

        public EFRestaurantApplicationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(RestaurantApplication application)
        {
            await context.RestaurantApplications.AddAsync(application);
        }
    }
}
