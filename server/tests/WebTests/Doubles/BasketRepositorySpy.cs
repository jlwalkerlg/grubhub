using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Baskets;
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

        public Task<Basket> Get(Guid userId, Guid restaurantId)
        {
            return Task.FromResult(
                Baskets.SingleOrDefault(x =>
                    x.UserId.Value == userId && x.RestaurantId.Value == restaurantId)
            );
        }

        public Task Remove(Basket basket)
        {
            Baskets.Remove(basket);
            return Task.CompletedTask;
        }
    }
}
