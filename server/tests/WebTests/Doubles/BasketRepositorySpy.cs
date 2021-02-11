using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Baskets;

namespace WebTests.Doubles
{
    public class BasketRepositorySpy : IBasketRepository
    {
        public List<Basket> Baskets { get; } = new();

        public Task Add(Basket order)
        {
            Baskets.Add(order);
            return Task.CompletedTask;
        }

        public Task<Basket> Get(UserId userId, RestaurantId restaurantId)
        {
            return Task.FromResult(
                Baskets.SingleOrDefault(x =>
                    x.UserId == userId && x.RestaurantId == restaurantId)
            );
        }
    }
}
