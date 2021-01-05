using System.Threading.Tasks;
using Web.Features.Restaurants;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Repositories
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
            var restaurant = await context.Restaurants.FindAsync(id);

            if (restaurant != null)
            {
                await context
                    .Entry(restaurant)
                    .Collection(x => x.Cuisines)
                    .LoadAsync();
            }

            return restaurant;
        }
    }
}
