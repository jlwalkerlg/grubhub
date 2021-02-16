using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Hubs;

namespace Web.Services.Notifications
{
    public class HubNotifier : INotifier
    {
        private readonly IHubContext<OrderHub> orderHubContext;

        public HubNotifier(IHubContext<OrderHub> orderHubContext)
        {
            this.orderHubContext = orderHubContext;
        }

        public async Task NotifyRestaurantOrderConfirmed(RestaurantManager manager, Order order)
        {
            await orderHubContext
                .Clients
                .Users(manager.Id.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.updated");
        }

        public async Task NotifyCustomerOrderConfirmed(User customer, Order order)
        {
            await orderHubContext
                .Clients
                .Users(customer.Id.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.updated");
        }
    }
}
