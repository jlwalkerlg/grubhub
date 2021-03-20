using System.Threading;
using System.Threading.Tasks;
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

        public async Task Handle(OrderDeliveredEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await hubContext.Clients
                .Users(order.RestaurantId.Value.ToString())
                .SendAsync("order-delivered", order.Id.Value, cancellationToken);
        }
    }
}
