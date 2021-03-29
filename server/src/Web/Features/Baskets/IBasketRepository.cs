using System;
using System.Threading.Tasks;
using Web.Domain.Baskets;

namespace Web.Features.Baskets
{
    public interface IBasketRepository
    {
        Task Add(Basket basket);
        Task<Basket> Get(Guid userId, Guid restaurantId);
        Task Remove(Basket basket);
    }
}
