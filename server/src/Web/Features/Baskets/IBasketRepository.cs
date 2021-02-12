using System.Threading.Tasks;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Features.Baskets
{
    public interface IBasketRepository
    {
        Task Add(Basket basket);
        Task<Basket> Get(UserId userId, RestaurantId restaurantId);
        Task Remove(Basket basket);
    }
}
