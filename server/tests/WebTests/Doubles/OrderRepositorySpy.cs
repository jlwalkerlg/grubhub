using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;
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

        public Task<Order> GetActiveOrderForUser(UserId userId)
        {
            return Task.FromResult(
                Orders.SingleOrDefault(x => x.UserId == userId)
            );
        }
    }
}
