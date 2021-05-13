using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyRestaurantOrderAcceptedListener : IEventListener<OrderAcceptedEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hub;

        public NotifyRestaurantOrderAcceptedListener(IUnitOfWork uow, IHubContext<OrderHub> hub)
        {
            this.uow = uow;
            this.hub = hub;
        }

        [Subscribe(nameof(OrderAcceptedEvent), typeof(NotifyRestaurantOrderAcceptedListener))]
        public async Task Handle(OrderAcceptedEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);
            var restaurant = await uow.Restaurants.GetById(order.RestaurantId);

            await hub.Clients
                .Users(restaurant.ManagerId.Value.ToString())
                .SendAsync("order-accepted", @event.OrderId);
        }
    }
}
