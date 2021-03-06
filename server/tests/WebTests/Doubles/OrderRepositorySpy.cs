using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Orders;

namespace WebTests.Doubles
{
    public class OrderRepositorySpy : IOrderRepository
    {
        public List<Order> Orders { get; } = new();

        public Task Add(Order order)
        {
            Orders.Add(order);
            return Task.CompletedTask;
        }

        public Task<Order> GetById(string id)
        {
            return Task.FromResult(
                Orders.SingleOrDefault(x => x.Id.Value == id)
            );
        }

        public Task<Order> GetByPaymentIntentId(string paymentIntentId)
        {
            return Task.FromResult(
                Orders.SingleOrDefault(x => x.PaymentIntentId == paymentIntentId)
            );
        }
    }
}
