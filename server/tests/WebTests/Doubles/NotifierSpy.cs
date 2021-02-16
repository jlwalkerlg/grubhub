using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Services.Notifications;

namespace WebTests.Doubles
{
    public class NotifierSpy : INotifier
    {
        public Dictionary<User, List<Order>> Customers { get; } = new();
        public Dictionary<RestaurantManager, List<Order>> Managers { get; } = new();

        public Task NotifyRestaurantOrderConfirmed(RestaurantManager manager, Order order)
        {
            if (!Managers.ContainsKey(manager))
            {
                Managers.Add(manager, new());
            }

            Managers[manager].Add(order);

            return Task.CompletedTask;
        }

        public Task NotifyCustomerOrderConfirmed(User customer, Order order)
        {
            if (!Customers.ContainsKey(customer))
            {
                Customers.Add(customer, new());
            }

            Customers[customer].Add(order);

            return Task.CompletedTask;
        }
    }
}
