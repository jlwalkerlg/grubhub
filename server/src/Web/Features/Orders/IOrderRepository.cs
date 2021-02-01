using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Features.Orders
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<Order> GetActiveOrder(UserId userId, RestaurantId restaurantId);
    }
}
