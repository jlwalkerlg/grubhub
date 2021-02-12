using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Baskets;

namespace Web.Data.EF.Repositories
{
    public class EFBasketRepository : IBasketRepository
    {
        private readonly AppDbContext context;

        public EFBasketRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Basket order)
        {
            await context.Baskets.AddAsync(order);
        }

        public async Task<Basket> Get(UserId userId, RestaurantId restaurantId)
        {
            return await context.Baskets
                .Where(x => x.UserId == userId &&
                            x.RestaurantId == restaurantId)
                .Include(x => x.Items)
                .OrderBy(x => "id")
                .SingleOrDefaultAsync();
        }

        public Task Remove(Basket basket)
        {
            context.Baskets.Remove(basket);
            return Task.CompletedTask;
        }
    }
}
