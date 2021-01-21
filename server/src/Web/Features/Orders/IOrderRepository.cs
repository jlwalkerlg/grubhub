using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;

namespace Web.Features.Orders
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<Order> GetActiveOrderForUser(UserId userId);
    }
}
