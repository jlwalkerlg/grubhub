using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Hubs;

namespace Web.Services.Notifications
{
    public class Notifier : INotifier
    {
        private readonly IHubContext<OrderHub> orderHubContext;

        public Notifier(IHubContext<OrderHub> orderHubContext)
        {
            this.orderHubContext = orderHubContext;
        }

        public async Task NotifyOrderConfirmed(User user, Order order)
        {
            await orderHubContext
                .Clients
                .Users(user.Id.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.updated");
        }
    }
}
