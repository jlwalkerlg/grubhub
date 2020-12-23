using System.Threading.Tasks;
using Application.Restaurants;
using Domain.Restaurants;

namespace Infrastructure.Persistence.EF.Repositories
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

        public async Task<Restaurant> GetById(RestaurantId id)
        {
            return await context.Restaurants.FindAsync(id);
        }
    }
}
