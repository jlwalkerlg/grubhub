using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants;

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

        public async Task<Restaurant> GetById(Guid id)
        {
            var restaurant = await context.Restaurants.FindAsync(new RestaurantId(id));

            if (restaurant != null)
            {
                await context
                    .Entry(restaurant)
                    .Collection(x => x.Cuisines)
                    .LoadAsync();
            }

            return restaurant;
        }

        public async Task<Restaurant> GetByManagerId(Guid managerId)
        {
            var restaurant = await context.Restaurants.FirstOrDefaultAsync(x => x.ManagerId == new UserId(managerId));

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
