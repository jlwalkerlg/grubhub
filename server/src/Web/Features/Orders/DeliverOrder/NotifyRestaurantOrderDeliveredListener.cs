using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyRestaurantOrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderDeliveredListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        [Subscribe(nameof(OrderDeliveredEvent), typeof(NotifyRestaurantOrderDeliveredListener))]
        public async Task Handle(OrderDeliveredEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await hubContext.Clients
                .Users(order.RestaurantId.Value.ToString())
                .SendAsync("order-delivered", order.Id.Value);
        }
    }
}
