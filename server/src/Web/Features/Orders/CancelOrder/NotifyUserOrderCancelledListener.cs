using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyUserOrderCancelledListener : IEventListener<OrderCancelledEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderCancelledListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        public async Task Handle(OrderCancelledEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));

            await hubContext.Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.cancelled", cancellationToken);
        }
    }
}
