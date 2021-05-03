using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    [CapSubscribe(nameof(NotifyRestaurantOrderDeliveredListener))]
    public class NotifyRestaurantOrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderDeliveredListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        [CapSubscribe(nameof(OrderDeliveredEvent), isPartial: true)]
        public async Task Handle(OrderDeliveredEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await hubContext.Clients
                .Users(order.RestaurantId.Value.ToString())
                .SendAsync("order-delivered", order.Id.Value);
        }
    }
}
