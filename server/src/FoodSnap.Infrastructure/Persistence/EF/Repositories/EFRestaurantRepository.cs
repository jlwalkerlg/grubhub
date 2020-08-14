using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
{
    public class EFRestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext context;

        public EFRestaurantRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Restaurant restaurant)
        {
            await context.Restaurants.AddAsync(restaurant);
        }
    }
}