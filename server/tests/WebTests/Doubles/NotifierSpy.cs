using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Services.Notifications;

namespace WebTests.Doubles
{
    public class NotifierSpy : INotifier
    {
        public List<User> Users { get; } = new();
        public List<Order> ConfirmedOrders { get; } = new();

        public Task NotifyOrderConfirmed(User user, Order order)
        {
            Users.Add(user);
            ConfirmedOrders.Add(order);
            return Task.CompletedTask;
        }
    }
}
