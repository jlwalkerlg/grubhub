using System.Threading.Tasks;
using Web.Domain.Orders;

namespace Web.Features.Orders
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<Order> GetById(OrderId orderId);
    }
}
